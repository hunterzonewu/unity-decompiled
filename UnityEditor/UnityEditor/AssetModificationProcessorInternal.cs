// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetModificationProcessorInternal
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetModificationProcessorInternal
  {
    private static IEnumerable<System.Type> assetModificationProcessors;
    internal static MethodInfo[] isOpenForEditMethods;

    private static IEnumerable<System.Type> AssetModificationProcessors
    {
      get
      {
        if (AssetModificationProcessorInternal.assetModificationProcessors == null)
        {
          List<System.Type> typeList = new List<System.Type>();
          typeList.AddRange(EditorAssemblies.SubclassesOf(typeof (AssetModificationProcessor)));
          typeList.AddRange(EditorAssemblies.SubclassesOf(typeof (global::AssetModificationProcessor)));
          AssetModificationProcessorInternal.assetModificationProcessors = (IEnumerable<System.Type>) typeList.ToArray();
        }
        return AssetModificationProcessorInternal.assetModificationProcessors;
      }
    }

    private static bool CheckArgumentTypes(System.Type[] types, MethodInfo method)
    {
      System.Reflection.ParameterInfo[] parameters = method.GetParameters();
      if (types.Length != parameters.Length)
      {
        Debug.LogWarning((object) ("Parameter count did not match. Expected: " + types.Length.ToString() + " Got: " + parameters.Length.ToString() + " in " + method.DeclaringType.ToString() + "." + method.Name));
        return false;
      }
      int index = 0;
      foreach (System.Type type in types)
      {
        System.Reflection.ParameterInfo parameterInfo = parameters[index];
        if (type != parameterInfo.ParameterType)
        {
          Debug.LogWarning((object) ("Parameter type mismatch at parameter " + (object) index + ". Expected: " + type.ToString() + " Got: " + parameterInfo.ParameterType.ToString() + " in " + method.DeclaringType.ToString() + "." + method.Name));
          return false;
        }
        ++index;
      }
      return true;
    }

    private static bool CheckArgumentTypesAndReturnType(System.Type[] types, MethodInfo method, System.Type returnType)
    {
      if (returnType == method.ReturnType)
        return AssetModificationProcessorInternal.CheckArgumentTypes(types, method);
      Debug.LogWarning((object) ("Return type mismatch. Expected: " + returnType.ToString() + " Got: " + method.ReturnType.ToString() + " in " + method.DeclaringType.ToString() + "." + method.Name));
      return false;
    }

    private static bool CheckArguments(object[] args, MethodInfo method)
    {
      System.Type[] types = new System.Type[args.Length];
      for (int index = 0; index < args.Length; ++index)
        types[index] = args[index].GetType();
      return AssetModificationProcessorInternal.CheckArgumentTypes(types, method);
    }

    private static bool CheckArgumentsAndReturnType(object[] args, MethodInfo method, System.Type returnType)
    {
      System.Type[] types = new System.Type[args.Length];
      for (int index = 0; index < args.Length; ++index)
        types[index] = args[index].GetType();
      return AssetModificationProcessorInternal.CheckArgumentTypesAndReturnType(types, method, returnType);
    }

    private static void OnWillCreateAsset(string path)
    {
      foreach (System.Type modificationProcessor in AssetModificationProcessorInternal.AssetModificationProcessors)
      {
        MethodInfo method = modificationProcessor.GetMethod("OnWillCreateAsset", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
        {
          object[] objArray = new object[1]{ (object) path };
          if (AssetModificationProcessorInternal.CheckArguments(objArray, method))
            method.Invoke((object) null, objArray);
        }
      }
    }

    private static void FileModeChanged(string[] assets, UnityEditor.VersionControl.FileMode mode)
    {
      if (!Provider.enabled || !Provider.PromptAndCheckoutIfNeeded(assets, string.Empty))
        return;
      Provider.SetFileMode(assets, mode);
    }

    private static void OnWillSaveAssets(string[] assets, out string[] assetsThatShouldBeSaved, out string[] assetsThatShouldBeReverted, int explicitlySaveScene)
    {
      assetsThatShouldBeReverted = new string[0];
      assetsThatShouldBeSaved = assets;
      bool flag = assets.Length > 0 && EditorPrefs.GetBool("VerifySavingAssets", false) && InternalEditorUtility.isHumanControllingUs;
      if (explicitlySaveScene != 0 && assets.Length == 1 && assets[0].EndsWith(".unity"))
        flag = false;
      if (flag)
        AssetSaveDialog.ShowWindow(assets, out assetsThatShouldBeSaved);
      else
        assetsThatShouldBeSaved = assets;
      foreach (System.Type modificationProcessor in AssetModificationProcessorInternal.AssetModificationProcessors)
      {
        MethodInfo method = modificationProcessor.GetMethod("OnWillSaveAssets", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
        {
          object[] objArray = new object[1]
          {
            (object) assetsThatShouldBeSaved
          };
          if (AssetModificationProcessorInternal.CheckArguments(objArray, method))
          {
            string[] strArray = (string[]) method.Invoke((object) null, objArray);
            if (strArray != null)
              assetsThatShouldBeSaved = strArray;
          }
        }
      }
      if (assetsThatShouldBeSaved == null)
        return;
      List<string> stringList = new List<string>();
      foreach (string assetOrMetaFilePath in assetsThatShouldBeSaved)
      {
        if (!AssetDatabase.IsOpenForEdit(assetOrMetaFilePath))
          stringList.Add(assetOrMetaFilePath);
      }
      assets = stringList.ToArray();
      if (assets.Length == 0 || Provider.PromptAndCheckoutIfNeeded(assets, string.Empty))
        return;
      Debug.LogError((object) ("Could not check out the following files in version control before saving: " + string.Join(", ", assets)));
      assetsThatShouldBeSaved = new string[0];
    }

    private static void RequireTeamLicense()
    {
      if (!InternalEditorUtility.HasTeamLicense())
        throw new MethodAccessException("Requires team license");
    }

    private static AssetMoveResult OnWillMoveAsset(string fromPath, string toPath, string[] newPaths, string[] NewMetaPaths)
    {
      AssetMoveResult assetMoveResult1 = AssetMoveResult.DidNotMove;
      if (!InternalEditorUtility.HasTeamLicense())
        return assetMoveResult1;
      AssetMoveResult assetMoveResult2 = AssetModificationHook.OnWillMoveAsset(fromPath, toPath);
      foreach (System.Type modificationProcessor in AssetModificationProcessorInternal.AssetModificationProcessors)
      {
        MethodInfo method = modificationProcessor.GetMethod("OnWillMoveAsset", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
        {
          AssetModificationProcessorInternal.RequireTeamLicense();
          object[] objArray = new object[2]
          {
            (object) fromPath,
            (object) toPath
          };
          if (AssetModificationProcessorInternal.CheckArgumentsAndReturnType(objArray, method, assetMoveResult2.GetType()))
            assetMoveResult2 |= (AssetMoveResult) method.Invoke((object) null, objArray);
        }
      }
      return assetMoveResult2;
    }

    private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
    {
      AssetDeleteResult assetDeleteResult = AssetDeleteResult.DidNotDelete;
      if (!InternalEditorUtility.HasTeamLicense())
        return assetDeleteResult;
      foreach (System.Type modificationProcessor in AssetModificationProcessorInternal.AssetModificationProcessors)
      {
        MethodInfo method = modificationProcessor.GetMethod("OnWillDeleteAsset", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
        {
          AssetModificationProcessorInternal.RequireTeamLicense();
          object[] objArray = new object[2]
          {
            (object) assetPath,
            (object) options
          };
          if (AssetModificationProcessorInternal.CheckArgumentsAndReturnType(objArray, method, assetDeleteResult.GetType()))
            assetDeleteResult |= (AssetDeleteResult) method.Invoke((object) null, objArray);
        }
      }
      if (assetDeleteResult != AssetDeleteResult.DidNotDelete)
        return assetDeleteResult;
      return AssetModificationHook.OnWillDeleteAsset(assetPath, options);
    }

    internal static MethodInfo[] GetIsOpenForEditMethods()
    {
      if (AssetModificationProcessorInternal.isOpenForEditMethods == null)
      {
        List<MethodInfo> methodInfoList = new List<MethodInfo>();
        foreach (System.Type modificationProcessor in AssetModificationProcessorInternal.AssetModificationProcessors)
        {
          MethodInfo method = modificationProcessor.GetMethod("IsOpenForEdit", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
          if (method != null)
          {
            AssetModificationProcessorInternal.RequireTeamLicense();
            string empty = string.Empty;
            bool flag = false;
            if (AssetModificationProcessorInternal.CheckArgumentTypesAndReturnType(new System.Type[2]
            {
              empty.GetType(),
              empty.GetType().MakeByRefType()
            }, method, flag.GetType()))
              methodInfoList.Add(method);
          }
        }
        AssetModificationProcessorInternal.isOpenForEditMethods = methodInfoList.ToArray();
      }
      return AssetModificationProcessorInternal.isOpenForEditMethods;
    }

    internal static bool IsOpenForEdit(string assetPath, out string message)
    {
      message = string.Empty;
      if (string.IsNullOrEmpty(assetPath))
        return true;
      bool flag = AssetModificationHook.IsOpenForEdit(assetPath, out message);
      foreach (MethodInfo openForEditMethod in AssetModificationProcessorInternal.GetIsOpenForEditMethods())
      {
        object[] parameters = new object[2]
        {
          (object) assetPath,
          (object) message
        };
        if (!(bool) openForEditMethod.Invoke((object) null, parameters))
        {
          message = parameters[1] as string;
          return false;
        }
      }
      return flag;
    }

    internal static void OnStatusUpdated()
    {
      WindowPending.OnStatusUpdated();
      foreach (System.Type modificationProcessor in AssetModificationProcessorInternal.AssetModificationProcessors)
      {
        MethodInfo method = modificationProcessor.GetMethod("OnStatusUpdated", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
        {
          AssetModificationProcessorInternal.RequireTeamLicense();
          object[] objArray = new object[0];
          if (AssetModificationProcessorInternal.CheckArgumentsAndReturnType(objArray, method, typeof (void)))
            method.Invoke((object) null, objArray);
        }
      }
    }

    private enum FileMode
    {
      Binary,
      Text,
    }
  }
}
