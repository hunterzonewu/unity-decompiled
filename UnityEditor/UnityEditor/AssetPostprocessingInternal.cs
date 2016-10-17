// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetPostprocessingInternal
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetPostprocessingInternal
  {
    private static ArrayList m_PostprocessStack;
    private static ArrayList m_ImportProcessors;

    private static void LogPostProcessorMissingDefaultConstructor(System.Type type)
    {
      Debug.LogErrorFormat("{0} requires a default constructor to be used as an asset post processor", (object) type);
    }

    private static void PostprocessAllAssets(string[] importedAssets, string[] addedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPathAssets)
    {
      object[] parameters = new object[4]
      {
        (object) importedAssets,
        (object) deletedAssets,
        (object) movedAssets,
        (object) movedFromPathAssets
      };
      foreach (System.Type type in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        MethodInfo method = type.GetMethod("OnPostprocessAllAssets", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
          method.Invoke((object) null, parameters);
      }
      SyncVS.PostprocessSyncProject(importedAssets, addedAssets, deletedAssets, movedAssets, movedFromPathAssets);
    }

    private static void PreprocessAssembly(string pathName)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPreprocessAssembly", (object[]) new string[1]{ pathName });
    }

    internal static void CallOnGeneratedCSProjectFiles()
    {
      object[] parameters = new object[0];
      foreach (MethodBase methodBase in AssetPostprocessingInternal.AllPostProcessorMethodsNamed("OnGeneratedCSProjectFiles"))
        methodBase.Invoke((object) null, parameters);
    }

    internal static bool OnPreGeneratingCSProjectFiles()
    {
      object[] parameters = new object[0];
      bool flag = false;
      foreach (MethodInfo methodInfo in AssetPostprocessingInternal.AllPostProcessorMethodsNamed("OnPreGeneratingCSProjectFiles"))
      {
        object obj = methodInfo.Invoke((object) null, parameters);
        if (methodInfo.ReturnType == typeof (bool))
          flag |= (bool) obj;
      }
      return flag;
    }

    private static IEnumerable<MethodInfo> AllPostProcessorMethodsNamed(string callbackName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)).Select<System.Type, MethodInfo>(new Func<System.Type, MethodInfo>(new AssetPostprocessingInternal.\u003CAllPostProcessorMethodsNamed\u003Ec__AnonStorey31()
      {
        callbackName = callbackName
      }.\u003C\u003Em__46)).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method != null));
    }

    private static void InitPostprocessors(string pathName)
    {
      AssetPostprocessingInternal.m_ImportProcessors = new ArrayList();
      foreach (System.Type type in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        try
        {
          AssetPostprocessor instance = Activator.CreateInstance(type) as AssetPostprocessor;
          instance.assetPath = pathName;
          AssetPostprocessingInternal.m_ImportProcessors.Add((object) instance);
        }
        catch (MissingMethodException ex)
        {
          AssetPostprocessingInternal.LogPostProcessorMissingDefaultConstructor(type);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      }
      AssetPostprocessingInternal.m_ImportProcessors.Sort((IComparer) new AssetPostprocessingInternal.CompareAssetImportPriority());
      AssetPostprocessingInternal.PostprocessStack postprocessStack = new AssetPostprocessingInternal.PostprocessStack();
      postprocessStack.m_ImportProcessors = AssetPostprocessingInternal.m_ImportProcessors;
      if (AssetPostprocessingInternal.m_PostprocessStack == null)
        AssetPostprocessingInternal.m_PostprocessStack = new ArrayList();
      AssetPostprocessingInternal.m_PostprocessStack.Add((object) postprocessStack);
    }

    private static void CleanupPostprocessors()
    {
      if (AssetPostprocessingInternal.m_PostprocessStack == null)
        return;
      AssetPostprocessingInternal.m_PostprocessStack.RemoveAt(AssetPostprocessingInternal.m_PostprocessStack.Count - 1);
      if (AssetPostprocessingInternal.m_PostprocessStack.Count == 0)
        return;
      AssetPostprocessingInternal.m_ImportProcessors = ((AssetPostprocessingInternal.PostprocessStack) AssetPostprocessingInternal.m_PostprocessStack[AssetPostprocessingInternal.m_PostprocessStack.Count - 1]).m_ImportProcessors;
    }

    private static uint[] GetMeshProcessorVersions()
    {
      List<uint> uintList = new List<uint>();
      foreach (System.Type type1 in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        try
        {
          AssetPostprocessor instance = Activator.CreateInstance(type1) as AssetPostprocessor;
          System.Type type2 = instance.GetType();
          bool flag1 = type2.GetMethod("OnPreprocessModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          bool flag2 = type2.GetMethod("OnProcessMeshAssingModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          bool flag3 = type2.GetMethod("OnPostprocessModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          uint version = instance.GetVersion();
          if ((int) version != 0)
          {
            if (!flag1 && !flag2)
            {
              if (!flag3)
                continue;
            }
            uintList.Add(version);
          }
        }
        catch (MissingMethodException ex)
        {
          AssetPostprocessingInternal.LogPostProcessorMissingDefaultConstructor(type1);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      }
      return uintList.ToArray();
    }

    private static void PreprocessMesh(string pathName)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPreprocessModel", (object[]) null);
    }

    private static void PreprocessSpeedTree(string pathName)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPreprocessSpeedTree", (object[]) null);
    }

    private static void PreprocessAnimation(string pathName)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPreprocessAnimation", (object[]) null);
    }

    private static Material ProcessMeshAssignMaterial(Renderer renderer, Material material)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
      {
        object[] args = new object[2]
        {
          (object) material,
          (object) renderer
        };
        object obj = AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnAssignMaterialModel", args);
        if ((bool) ((UnityEngine.Object) (obj as Material)))
          return obj as Material;
      }
      return (Material) null;
    }

    private static bool ProcessMeshHasAssignMaterial()
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
      {
        if (importProcessor.GetType().GetMethod("OnAssignMaterialModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)
          return true;
      }
      return false;
    }

    private static void PostprocessMesh(GameObject gameObject)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPostprocessModel", new object[1]
        {
          (object) gameObject
        });
    }

    private static void PostprocessSpeedTree(GameObject gameObject)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPostprocessSpeedTree", new object[1]
        {
          (object) gameObject
        });
    }

    private static void PostprocessGameObjectWithUserProperties(GameObject go, string[] prop_names, object[] prop_values)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPostprocessGameObjectWithUserProperties", new object[3]
        {
          (object) go,
          (object) prop_names,
          (object) prop_values
        });
    }

    private static uint[] GetTextureProcessorVersions()
    {
      List<uint> uintList = new List<uint>();
      foreach (System.Type type1 in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        try
        {
          AssetPostprocessor instance = Activator.CreateInstance(type1) as AssetPostprocessor;
          System.Type type2 = instance.GetType();
          bool flag1 = type2.GetMethod("OnPreprocessTexture", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          bool flag2 = type2.GetMethod("OnPostprocessTexture", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          uint version = instance.GetVersion();
          if ((int) version != 0)
          {
            if (!flag1)
            {
              if (!flag2)
                continue;
            }
            uintList.Add(version);
          }
        }
        catch (MissingMethodException ex)
        {
          AssetPostprocessingInternal.LogPostProcessorMissingDefaultConstructor(type1);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      }
      return uintList.ToArray();
    }

    private static void PreprocessTexture(string pathName)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPreprocessTexture", (object[]) null);
    }

    private static void PostprocessTexture(Texture2D tex, string pathName)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPostprocessTexture", new object[1]{ (object) tex });
    }

    private static void PostprocessSprites(Texture2D tex, string pathName, Sprite[] sprites)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPostprocessSprites", new object[2]
        {
          (object) tex,
          (object) sprites
        });
    }

    private static uint[] GetAudioProcessorVersions()
    {
      List<uint> uintList = new List<uint>();
      foreach (System.Type type1 in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
      {
        try
        {
          AssetPostprocessor instance = Activator.CreateInstance(type1) as AssetPostprocessor;
          System.Type type2 = instance.GetType();
          bool flag1 = type2.GetMethod("OnPreprocessAudio", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          bool flag2 = type2.GetMethod("OnPostprocessAudio", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null;
          uint version = instance.GetVersion();
          if ((int) version != 0)
          {
            if (!flag1)
            {
              if (!flag2)
                continue;
            }
            uintList.Add(version);
          }
        }
        catch (MissingMethodException ex)
        {
          AssetPostprocessingInternal.LogPostProcessorMissingDefaultConstructor(type1);
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
      }
      return uintList.ToArray();
    }

    private static void PreprocessAudio(string pathName)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPreprocessAudio", (object[]) null);
    }

    private static void PostprocessAudio(AudioClip tex, string pathName)
    {
      foreach (AssetPostprocessor importProcessor in AssetPostprocessingInternal.m_ImportProcessors)
        AttributeHelper.InvokeMemberIfAvailable((object) importProcessor, "OnPostprocessAudio", new object[1]{ (object) tex });
    }

    private static void PostprocessAssetbundleNameChanged(string assetPAth, string prevoiusAssetBundleName, string newAssetBundleName)
    {
      object[] args = new object[3]
      {
        (object) assetPAth,
        (object) prevoiusAssetBundleName,
        (object) newAssetBundleName
      };
      foreach (System.Type type in EditorAssemblies.SubclassesOf(typeof (AssetPostprocessor)))
        AttributeHelper.InvokeMemberIfAvailable((object) (Activator.CreateInstance(type) as AssetPostprocessor), "OnPostprocessAssetbundleNameChanged", args);
    }

    internal class CompareAssetImportPriority : IComparer
    {
      int IComparer.Compare(object xo, object yo)
      {
        return ((AssetPostprocessor) xo).GetPostprocessOrder().CompareTo(((AssetPostprocessor) yo).GetPostprocessOrder());
      }
    }

    internal class PostprocessStack
    {
      internal ArrayList m_ImportProcessors;
    }
  }
}
