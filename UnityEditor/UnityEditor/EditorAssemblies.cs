// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorAssemblies
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  internal static class EditorAssemblies
  {
    internal static List<RuntimeInitializeClassInfo> m_RuntimeInitializeClassInfoList;
    internal static int m_TotalNumRuntimeInitializeMethods;

    internal static Assembly[] loadedAssemblies { get; private set; }

    internal static IEnumerable<System.Type> loadedTypes
    {
      get
      {
        return ((IEnumerable<Assembly>) EditorAssemblies.loadedAssemblies).SelectMany<Assembly, System.Type>((Func<Assembly, IEnumerable<System.Type>>) (assembly => (IEnumerable<System.Type>) AssemblyHelper.GetTypesFromAssembly(assembly)));
      }
    }

    internal static IEnumerable<System.Type> SubclassesOf(System.Type parent)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return EditorAssemblies.loadedTypes.Where<System.Type>(new Func<System.Type, bool>(new EditorAssemblies.\u003CSubclassesOf\u003Ec__AnonStorey38()
      {
        parent = parent
      }.\u003C\u003Em__4E));
    }

    private static void SetLoadedEditorAssemblies(Assembly[] assemblies)
    {
      EditorAssemblies.loadedAssemblies = assemblies;
      EditorAssemblies.ProcessInitializeOnLoadAttributes();
    }

    private static RuntimeInitializeClassInfo[] GetRuntimeInitializeClassInfos()
    {
      if (EditorAssemblies.m_RuntimeInitializeClassInfoList == null)
        return (RuntimeInitializeClassInfo[]) null;
      return EditorAssemblies.m_RuntimeInitializeClassInfoList.ToArray();
    }

    private static int GetTotalNumRuntimeInitializeMethods()
    {
      return EditorAssemblies.m_TotalNumRuntimeInitializeMethods;
    }

    private static void StoreRuntimeInitializeClassInfo(System.Type type, List<string> methodNames, List<RuntimeInitializeLoadType> loadTypes)
    {
      EditorAssemblies.m_RuntimeInitializeClassInfoList.Add(new RuntimeInitializeClassInfo()
      {
        assemblyName = type.Assembly.GetName().Name.ToString(),
        className = type.ToString(),
        methodNames = methodNames.ToArray(),
        loadTypes = loadTypes.ToArray()
      });
      EditorAssemblies.m_TotalNumRuntimeInitializeMethods += methodNames.Count;
    }

    private static void ProcessStaticMethodAttributes(System.Type type)
    {
      List<string> methodNames = (List<string>) null;
      List<RuntimeInitializeLoadType> loadTypes = (List<RuntimeInitializeLoadType>) null;
      MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      for (int index = 0; index < methods.GetLength(0); ++index)
      {
        MethodInfo methodInfo = methods[index];
        if (Attribute.IsDefined((MemberInfo) methodInfo, typeof (RuntimeInitializeOnLoadMethodAttribute)))
        {
          RuntimeInitializeLoadType initializeLoadType = RuntimeInitializeLoadType.AfterSceneLoad;
          object[] customAttributes = methodInfo.GetCustomAttributes(typeof (RuntimeInitializeOnLoadMethodAttribute), false);
          if (customAttributes != null && customAttributes.Length > 0)
            initializeLoadType = ((RuntimeInitializeOnLoadMethodAttribute) customAttributes[0]).loadType;
          if (methodNames == null)
          {
            methodNames = new List<string>();
            loadTypes = new List<RuntimeInitializeLoadType>();
          }
          methodNames.Add(methodInfo.Name);
          loadTypes.Add(initializeLoadType);
        }
        if (Attribute.IsDefined((MemberInfo) methodInfo, typeof (InitializeOnLoadMethodAttribute)))
          methodInfo.Invoke((object) null, (object[]) null);
      }
      if (methodNames == null)
        return;
      EditorAssemblies.StoreRuntimeInitializeClassInfo(type, methodNames, loadTypes);
    }

    private static void ProcessEditorInitializeOnLoad(System.Type type)
    {
      try
      {
        RuntimeHelpers.RunClassConstructor(type.TypeHandle);
      }
      catch (TypeInitializationException ex)
      {
        Debug.LogError((object) ex.InnerException);
      }
    }

    private static void ProcessInitializeOnLoadAttributes()
    {
      EditorAssemblies.m_TotalNumRuntimeInitializeMethods = 0;
      EditorAssemblies.m_RuntimeInitializeClassInfoList = new List<RuntimeInitializeClassInfo>();
      foreach (System.Type loadedType in EditorAssemblies.loadedTypes)
      {
        if (loadedType.IsDefined(typeof (InitializeOnLoadAttribute), false))
          EditorAssemblies.ProcessEditorInitializeOnLoad(loadedType);
        EditorAssemblies.ProcessStaticMethodAttributes(loadedType);
      }
    }
  }
}
