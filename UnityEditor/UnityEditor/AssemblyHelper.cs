// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor.Modules;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AssemblyHelper
  {
    private const int kDefaultDepth = 10;

    public static void CheckForAssemblyFileNameMismatch(string assemblyPath)
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(assemblyPath);
      string internalAssemblyName = AssemblyHelper.ExtractInternalAssemblyName(assemblyPath);
      if (!(withoutExtension != internalAssemblyName))
        return;
      UnityEngine.Debug.LogWarning((object) ("Assembly '" + internalAssemblyName + "' has non matching file name: '" + Path.GetFileName(assemblyPath) + "'. This can cause build issues on some platforms."));
    }

    public static string[] GetNamesOfAssembliesLoadedInCurrentDomain()
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      List<string> stringList = new List<string>();
      foreach (Assembly assembly in assemblies)
      {
        try
        {
          stringList.Add(assembly.Location);
        }
        catch (NotSupportedException ex)
        {
        }
      }
      return stringList.ToArray();
    }

    public static Assembly FindLoadedAssemblyWithName(string s)
    {
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        try
        {
          if (assembly.Location.Contains(s))
            return assembly;
        }
        catch (NotSupportedException ex)
        {
        }
      }
      return (Assembly) null;
    }

    public static string ExtractInternalAssemblyName(string path)
    {
      return ((AssemblyNameReference) AssemblyDefinition.ReadAssembly(path).get_Name()).get_Name();
    }

    private static AssemblyDefinition GetAssemblyDefinitionCached(string path, Dictionary<string, AssemblyDefinition> cache)
    {
      if (cache.ContainsKey(path))
        return cache[path];
      AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(path);
      cache[path] = assemblyDefinition;
      return assemblyDefinition;
    }

    private static bool IgnoreAssembly(string assemblyPath, BuildTarget target)
    {
      switch (target)
      {
        case BuildTarget.WSAPlayer:
        case BuildTarget.WP8Player:
          if (assemblyPath.IndexOf("mscorlib.dll") != -1 || assemblyPath.IndexOf("System.") != -1 || (assemblyPath.IndexOf("Windows.dll") != -1 || assemblyPath.IndexOf("Microsoft.") != -1) || (assemblyPath.IndexOf("Windows.") != -1 || assemblyPath.IndexOf("WinRTLegacy.dll") != -1 || assemblyPath.IndexOf("platform.dll") != -1))
            return true;
          break;
      }
      return AssemblyHelper.IsInternalAssembly(assemblyPath);
    }

    private static void AddReferencedAssembliesRecurse(string assemblyPath, List<string> alreadyFoundAssemblies, string[] allAssemblyPaths, string[] foldersToSearch, Dictionary<string, AssemblyDefinition> cache, BuildTarget target)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey2D recurseCAnonStorey2D = new AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey2D();
      // ISSUE: reference to a compiler-generated field
      recurseCAnonStorey2D.target = target;
      // ISSUE: reference to a compiler-generated field
      if (AssemblyHelper.IgnoreAssembly(assemblyPath, recurseCAnonStorey2D.target))
        return;
      AssemblyDefinition definitionCached = AssemblyHelper.GetAssemblyDefinitionCached(assemblyPath, cache);
      if (definitionCached == null)
        throw new ArgumentException("Referenced Assembly " + Path.GetFileName(assemblyPath) + " could not be found!");
      if (alreadyFoundAssemblies.IndexOf(assemblyPath) != -1)
        return;
      alreadyFoundAssemblies.Add(assemblyPath);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      IEnumerable<string> source = ((IEnumerable<PluginImporter>) PluginImporter.GetImporters(recurseCAnonStorey2D.target)).Where<PluginImporter>(new Func<PluginImporter, bool>(recurseCAnonStorey2D.\u003C\u003Em__40)).Select<PluginImporter, string>((Func<PluginImporter, string>) (i => Path.GetFileName(i.assetPath))).Distinct<string>();
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey2E recurseCAnonStorey2E = new AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey2E();
      using (Collection<AssemblyNameReference>.Enumerator enumerator = definitionCached.get_MainModule().get_AssemblyReferences().GetEnumerator())
      {
        // ISSUE: explicit reference operation
        while (((Collection<AssemblyNameReference>.Enumerator) @enumerator).MoveNext())
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: explicit reference operation
          recurseCAnonStorey2E.referencedAssembly = ((Collection<AssemblyNameReference>.Enumerator) @enumerator).get_Current();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!(recurseCAnonStorey2E.referencedAssembly.get_Name() == "BridgeInterface") && !(recurseCAnonStorey2E.referencedAssembly.get_Name() == "WinRTBridge") && (!(recurseCAnonStorey2E.referencedAssembly.get_Name() == "UnityEngineProxy") && !AssemblyHelper.IgnoreAssembly(recurseCAnonStorey2E.referencedAssembly.get_Name() + ".dll", recurseCAnonStorey2D.target)))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            string assemblyName = AssemblyHelper.FindAssemblyName(recurseCAnonStorey2E.referencedAssembly.get_FullName(), recurseCAnonStorey2E.referencedAssembly.get_Name(), allAssemblyPaths, foldersToSearch, cache);
            if (assemblyName == string.Empty)
            {
              bool flag = false;
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey2F recurseCAnonStorey2F = new AssemblyHelper.\u003CAddReferencedAssembliesRecurse\u003Ec__AnonStorey2F();
              // ISSUE: reference to a compiler-generated field
              recurseCAnonStorey2F.\u003C\u003Ef__ref\u002446 = recurseCAnonStorey2E;
              string[] strArray = new string[2]
              {
                ".dll",
                ".winmd"
              };
              foreach (string str in strArray)
              {
                // ISSUE: reference to a compiler-generated field
                recurseCAnonStorey2F.extension = str;
                // ISSUE: reference to a compiler-generated method
                if (source.Any<string>(new Func<string, bool>(recurseCAnonStorey2F.\u003C\u003Em__42)))
                {
                  flag = true;
                  break;
                }
              }
              if (!flag)
              {
                // ISSUE: reference to a compiler-generated field
                throw new ArgumentException(string.Format("The Assembly {0} is referenced by {1} ('{2}'). But the dll is not allowed to be included or could not be found.", (object) recurseCAnonStorey2E.referencedAssembly.get_Name(), (object) ((AssemblyNameReference) definitionCached.get_MainModule().get_Assembly().get_Name()).get_Name(), (object) assemblyPath));
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              AssemblyHelper.AddReferencedAssembliesRecurse(assemblyName, alreadyFoundAssemblies, allAssemblyPaths, foldersToSearch, cache, recurseCAnonStorey2D.target);
            }
          }
        }
      }
    }

    private static string FindAssemblyName(string fullName, string name, string[] allAssemblyPaths, string[] foldersToSearch, Dictionary<string, AssemblyDefinition> cache)
    {
      for (int index = 0; index < allAssemblyPaths.Length; ++index)
      {
        if (((AssemblyNameReference) AssemblyHelper.GetAssemblyDefinitionCached(allAssemblyPaths[index], cache).get_MainModule().get_Assembly().get_Name()).get_Name() == name)
          return allAssemblyPaths[index];
      }
      foreach (string path1 in foldersToSearch)
      {
        string path = Path.Combine(path1, name + ".dll");
        if (File.Exists(path))
          return path;
      }
      return string.Empty;
    }

    public static string[] FindAssembliesReferencedBy(string[] paths, string[] foldersToSearch, BuildTarget target)
    {
      List<string> alreadyFoundAssemblies = new List<string>();
      string[] allAssemblyPaths = paths;
      Dictionary<string, AssemblyDefinition> cache = new Dictionary<string, AssemblyDefinition>();
      for (int index = 0; index < paths.Length; ++index)
        AssemblyHelper.AddReferencedAssembliesRecurse(paths[index], alreadyFoundAssemblies, allAssemblyPaths, foldersToSearch, cache, target);
      for (int index = 0; index < paths.Length; ++index)
        alreadyFoundAssemblies.Remove(paths[index]);
      return alreadyFoundAssemblies.ToArray();
    }

    public static string[] FindAssembliesReferencedBy(string path, string[] foldersToSearch, BuildTarget target)
    {
      return AssemblyHelper.FindAssembliesReferencedBy(new string[1]{ path }, foldersToSearch, target);
    }

    private static bool IsTypeMonoBehaviourOrScriptableObject(AssemblyDefinition assembly, TypeReference type)
    {
      if (type == null || type.get_FullName() == "System.Object")
        return false;
      Assembly assembly1 = (Assembly) null;
      if (type.get_Scope().get_Name() == "UnityEngine")
        assembly1 = typeof (MonoBehaviour).Assembly;
      else if (type.get_Scope().get_Name() == "UnityEditor")
        assembly1 = typeof (EditorWindow).Assembly;
      else if (type.get_Scope().get_Name() == "UnityEngine.UI")
        assembly1 = AssemblyHelper.FindLoadedAssemblyWithName("UnityEngine.UI");
      if (assembly1 != null)
      {
        string name = !type.get_IsGenericInstance() ? type.get_FullName() : type.get_Namespace() + "." + type.get_Name();
        System.Type type1 = assembly1.GetType(name);
        if (type1 == typeof (MonoBehaviour) || type1.IsSubclassOf(typeof (MonoBehaviour)) || (type1 == typeof (ScriptableObject) || type1.IsSubclassOf(typeof (ScriptableObject))))
          return true;
      }
      TypeDefinition typeDefinition = (TypeDefinition) null;
      try
      {
        typeDefinition = type.Resolve();
      }
      catch (AssemblyResolutionException ex)
      {
      }
      if (typeDefinition != null)
        return AssemblyHelper.IsTypeMonoBehaviourOrScriptableObject(assembly, typeDefinition.get_BaseType());
      return false;
    }

    public static void ExtractAllClassesThatInheritMonoBehaviourAndScriptableObject(string path, out string[] classNamesArray, out string[] classNameSpacesArray)
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      ReaderParameters readerParameters = new ReaderParameters();
      DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();
      ((BaseAssemblyResolver) assemblyResolver).AddSearchDirectory(Path.GetDirectoryName(path));
      readerParameters.set_AssemblyResolver((IAssemblyResolver) assemblyResolver);
      AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path, readerParameters);
      using (Collection<ModuleDefinition>.Enumerator enumerator1 = assembly.get_Modules().GetEnumerator())
      {
        // ISSUE: explicit reference operation
        while (((Collection<ModuleDefinition>.Enumerator) @enumerator1).MoveNext())
        {
          // ISSUE: explicit reference operation
          using (Collection<TypeDefinition>.Enumerator enumerator2 = ((Collection<ModuleDefinition>.Enumerator) @enumerator1).get_Current().get_Types().GetEnumerator())
          {
            // ISSUE: explicit reference operation
            while (((Collection<TypeDefinition>.Enumerator) @enumerator2).MoveNext())
            {
              // ISSUE: explicit reference operation
              TypeDefinition current = ((Collection<TypeDefinition>.Enumerator) @enumerator2).get_Current();
              TypeReference baseType = current.get_BaseType();
              try
              {
                if (AssemblyHelper.IsTypeMonoBehaviourOrScriptableObject(assembly, baseType))
                {
                  stringList1.Add(((TypeReference) current).get_Name());
                  stringList2.Add(((TypeReference) current).get_Namespace());
                }
              }
              catch (Exception ex)
              {
                UnityEngine.Debug.LogError((object) ("Failed to extract " + ((TypeReference) current).get_FullName() + " class of base type " + baseType.get_FullName() + " when inspecting " + path));
              }
            }
          }
        }
      }
      classNamesArray = stringList1.ToArray();
      classNameSpacesArray = stringList2.ToArray();
    }

    public static AssemblyTypeInfoGenerator.ClassInfo[] ExtractAssemblyTypeInfo(BuildTarget targetPlatform, bool isEditor, string assemblyPathName, string[] searchDirs)
    {
      try
      {
        ICompilationExtension compilationExtension = ModuleManager.GetCompilationExtension(ModuleManager.GetTargetStringFromBuildTarget(targetPlatform));
        string[] extraAssemblyPaths = compilationExtension.GetCompilerExtraAssemblyPaths(isEditor, assemblyPathName);
        if (extraAssemblyPaths != null && extraAssemblyPaths.Length > 0)
        {
          List<string> stringList = new List<string>((IEnumerable<string>) searchDirs);
          stringList.AddRange((IEnumerable<string>) extraAssemblyPaths);
          searchDirs = stringList.ToArray();
        }
        IAssemblyResolver assemblyResolver = compilationExtension.GetAssemblyResolver(isEditor, assemblyPathName, searchDirs);
        return (assemblyResolver != null ? new AssemblyTypeInfoGenerator(assemblyPathName, assemblyResolver) : new AssemblyTypeInfoGenerator(assemblyPathName, searchDirs)).GatherClassInfo();
      }
      catch (Exception ex)
      {
        throw new Exception("ExtractAssemblyTypeInfo: Failed to process " + assemblyPathName + ", " + (object) ex);
      }
    }

    internal static System.Type[] GetTypesFromAssembly(Assembly assembly)
    {
      if (assembly == null)
        return new System.Type[0];
      try
      {
        return assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException ex)
      {
        return new System.Type[0];
      }
    }

    [DebuggerHidden]
    internal static IEnumerable<T> FindImplementors<T>(Assembly assembly) where T : class
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyHelper.\u003CFindImplementors\u003Ec__Iterator2<T> implementorsCIterator2 = new AssemblyHelper.\u003CFindImplementors\u003Ec__Iterator2<T>()
      {
        assembly = assembly,
        \u003C\u0024\u003Eassembly = assembly
      };
      // ISSUE: reference to a compiler-generated field
      implementorsCIterator2.\u0024PC = -2;
      return (IEnumerable<T>) implementorsCIterator2;
    }

    public static bool IsManagedAssembly(string file)
    {
      DllType dllType = InternalEditorUtility.DetectDotNetDll(file);
      if (dllType != DllType.Unknown)
        return dllType != DllType.Native;
      return false;
    }

    public static bool IsInternalAssembly(string file)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssemblyHelper.\u003CIsInternalAssembly\u003Ec__AnonStorey30 assemblyCAnonStorey30 = new AssemblyHelper.\u003CIsInternalAssembly\u003Ec__AnonStorey30();
      // ISSUE: reference to a compiler-generated field
      assemblyCAnonStorey30.file = file;
      // ISSUE: reference to a compiler-generated field
      if (!ModuleManager.IsRegisteredModule(assemblyCAnonStorey30.file))
      {
        // ISSUE: reference to a compiler-generated method
        return ((IEnumerable<string>) ModuleUtils.GetAdditionalReferencesForUserScripts()).Any<string>(new Func<string, bool>(assemblyCAnonStorey30.\u003C\u003Em__43));
      }
      return true;
    }

    internal static ICollection<string> FindAssemblies(string basePath)
    {
      return AssemblyHelper.FindAssemblies(basePath, 10);
    }

    internal static ICollection<string> FindAssemblies(string basePath, int maxDepth)
    {
      List<string> stringList = new List<string>();
      if (maxDepth == 0)
        return (ICollection<string>) stringList;
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(basePath);
        stringList.AddRange(((IEnumerable<FileInfo>) directoryInfo.GetFiles()).Where<FileInfo>((Func<FileInfo, bool>) (file => AssemblyHelper.IsManagedAssembly(file.FullName))).Select<FileInfo, string>((Func<FileInfo, string>) (file => file.FullName)));
        foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
          stringList.AddRange((IEnumerable<string>) AssemblyHelper.FindAssemblies(directory.FullName, maxDepth - 1));
      }
      catch (Exception ex)
      {
      }
      return (ICollection<string>) stringList;
    }
  }
}
