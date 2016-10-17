// Decompiled with JetBrains decompiler
// Type: UnityEditor.CodeStrippingUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;
using Mono.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class CodeStrippingUtils
  {
    public static readonly string[] NativeClassBlackList = new string[6]{ "PreloadData", "Material", "Cubemap", "Texture3D", "RenderTexture", "Mesh" };
    public static readonly Dictionary<string, string> NativeClassDependencyBlackList = new Dictionary<string, string>() { { "ParticleSystemRenderer", "ParticleSystem" } };
    private static readonly string[] s_UserAssemblies = new string[5]{ "Assembly-CSharp.dll", "Assembly-CSharp-firstpass.dll", "Assembly-UnityScript.dll", "Assembly-UnityScript-firstpass.dll", "UnityEngine.Analytics.dll" };

    public static string[] UserAssemblies
    {
      get
      {
        return CodeStrippingUtils.s_UserAssemblies;
      }
    }

    public static HashSet<string> GetModulesFromICalls(string icallsListFile)
    {
      string[] strArray = File.ReadAllLines(icallsListFile);
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string icall in strArray)
      {
        string icallModule = ModuleMetadata.GetICallModule(icall);
        if (!string.IsNullOrEmpty(icallModule))
          stringSet.Add(icallModule);
      }
      return stringSet;
    }

    public static void GenerateDependencies(string strippedAssemblyDir, string icallsListFile, RuntimeClassRegistry rcr, out HashSet<string> nativeClasses, out HashSet<string> nativeModules)
    {
      string[] userAssemblies = CodeStrippingUtils.GetUserAssemblies(strippedAssemblyDir);
      nativeClasses = !PlayerSettings.stripEngineCode ? (HashSet<string>) null : CodeStrippingUtils.GenerateNativeClassList(rcr, strippedAssemblyDir, userAssemblies);
      if (nativeClasses != null)
        CodeStrippingUtils.ExcludeModuleManagers(ref nativeClasses);
      nativeModules = CodeStrippingUtils.GetNativeModulesToRegister(nativeClasses);
      if (nativeClasses != null && icallsListFile != null)
      {
        HashSet<string> modulesFromIcalls = CodeStrippingUtils.GetModulesFromICalls(icallsListFile);
        int classId = BaseObjectTools.StringToClassID("GlobalGameManager");
        using (HashSet<string>.Enumerator enumerator = modulesFromIcalls.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            foreach (int moduleClass in ModuleMetadata.GetModuleClasses(enumerator.Current))
            {
              if (BaseObjectTools.IsDerivedFromClassID(moduleClass, classId))
                nativeClasses.Add(BaseObjectTools.ClassIDToString(moduleClass));
            }
          }
        }
        nativeModules.UnionWith((IEnumerable<string>) modulesFromIcalls);
      }
      new AssemblyReferenceChecker().CollectReferencesFromRoots(strippedAssemblyDir, (IEnumerable<string>) userAssemblies, true, 0.0f, true);
    }

    public static void WriteModuleAndClassRegistrationFile(string strippedAssemblyDir, string icallsListFile, string outputDir, RuntimeClassRegistry rcr, IEnumerable<string> classesToSkip)
    {
      HashSet<string> nativeClasses;
      HashSet<string> nativeModules;
      CodeStrippingUtils.GenerateDependencies(strippedAssemblyDir, icallsListFile, rcr, out nativeClasses, out nativeModules);
      CodeStrippingUtils.WriteModuleAndClassRegistrationFile(Path.Combine(outputDir, "UnityClassRegistration.cpp"), nativeModules, nativeClasses, new HashSet<string>(classesToSkip));
    }

    public static HashSet<string> GetNativeModulesToRegister(HashSet<string> nativeClasses)
    {
      if (nativeClasses == null)
        return CodeStrippingUtils.GetAllStrippableModules();
      return CodeStrippingUtils.GetRequiredStrippableModules(nativeClasses);
    }

    private static HashSet<string> GetClassNames(IEnumerable<int> classIds)
    {
      HashSet<string> stringSet = new HashSet<string>();
      foreach (int classId in classIds)
        stringSet.Add(BaseObjectTools.ClassIDToString(classId));
      return stringSet;
    }

    private static HashSet<string> GetAllStrippableModules()
    {
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string moduleName in ModuleMetadata.GetModuleNames())
      {
        if (ModuleMetadata.GetModuleStrippable(moduleName))
          stringSet.Add(moduleName);
      }
      return stringSet;
    }

    private static HashSet<string> GetRequiredStrippableModules(HashSet<string> nativeClasses)
    {
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string moduleName in ModuleMetadata.GetModuleNames())
      {
        if (ModuleMetadata.GetModuleStrippable(moduleName))
        {
          HashSet<string> classNames = CodeStrippingUtils.GetClassNames((IEnumerable<int>) ModuleMetadata.GetModuleClasses(moduleName));
          if (nativeClasses.Overlaps((IEnumerable<string>) classNames))
            stringSet.Add(moduleName);
        }
      }
      return stringSet;
    }

    private static void ExcludeModuleManagers(ref HashSet<string> nativeClasses)
    {
      string[] moduleNames = ModuleMetadata.GetModuleNames();
      int classId = BaseObjectTools.StringToClassID("GlobalGameManager");
      foreach (string moduleName in moduleNames)
      {
        if (ModuleMetadata.GetModuleStrippable(moduleName))
        {
          int[] moduleClasses = ModuleMetadata.GetModuleClasses(moduleName);
          HashSet<int> intSet = new HashSet<int>();
          HashSet<string> stringSet = new HashSet<string>();
          foreach (int num in moduleClasses)
          {
            if (BaseObjectTools.IsDerivedFromClassID(num, classId))
              intSet.Add(num);
            else
              stringSet.Add(BaseObjectTools.ClassIDToString(num));
          }
          if (stringSet.Count != 0 && !nativeClasses.Overlaps((IEnumerable<string>) stringSet))
          {
            using (HashSet<int>.Enumerator enumerator = intSet.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                int current = enumerator.Current;
                nativeClasses.Remove(BaseObjectTools.ClassIDToString(current));
              }
            }
          }
        }
      }
    }

    private static HashSet<string> GenerateNativeClassList(RuntimeClassRegistry rcr, string directory, string[] rootAssemblies)
    {
      HashSet<string> stringSet1 = CodeStrippingUtils.CollectNativeClassListFromRoots(directory, rootAssemblies);
      foreach (string nativeClassBlack in CodeStrippingUtils.NativeClassBlackList)
        stringSet1.Add(nativeClassBlack);
      using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = CodeStrippingUtils.NativeClassDependencyBlackList.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          if (stringSet1.Contains(current))
          {
            string classDependencyBlack = CodeStrippingUtils.NativeClassDependencyBlackList[current];
            stringSet1.Add(classDependencyBlack);
          }
        }
      }
      using (List<string>.Enumerator enumerator = rcr.GetAllNativeClassesIncludingManagersAsString().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          int classId = BaseObjectTools.StringToClassID(current);
          if (classId != -1 && !BaseObjectTools.IsBaseObject(classId))
            stringSet1.Add(current);
        }
      }
      HashSet<string> stringSet2 = new HashSet<string>();
      using (HashSet<string>.Enumerator enumerator = stringSet1.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          for (int ID = BaseObjectTools.StringToClassID(enumerator.Current); !BaseObjectTools.IsBaseObject(ID); ID = BaseObjectTools.GetSuperClassID(ID))
            stringSet2.Add(BaseObjectTools.ClassIDToString(ID));
        }
      }
      return stringSet2;
    }

    private static HashSet<string> CollectNativeClassListFromRoots(string directory, string[] rootAssemblies)
    {
      HashSet<string> stringSet = new HashSet<string>();
      using (HashSet<string>.Enumerator enumerator = CodeStrippingUtils.CollectManagedTypeReferencesFromRoots(directory, rootAssemblies).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          int classId = BaseObjectTools.StringToClassID(current);
          if (classId != -1 && !BaseObjectTools.IsBaseObject(classId))
            stringSet.Add(current);
        }
      }
      return stringSet;
    }

    private static HashSet<string> CollectManagedTypeReferencesFromRoots(string directory, string[] rootAssemblies)
    {
      HashSet<string> stringSet = new HashSet<string>();
      AssemblyReferenceChecker referenceChecker = new AssemblyReferenceChecker();
      bool withMethods = false;
      bool ignoreSystemDlls = false;
      referenceChecker.CollectReferencesFromRoots(directory, (IEnumerable<string>) rootAssemblies, withMethods, 0.0f, ignoreSystemDlls);
      string[] assemblyFileNames = referenceChecker.GetAssemblyFileNames();
      AssemblyDefinition[] assemblyDefinitions = referenceChecker.GetAssemblyDefinitions();
      foreach (AssemblyDefinition assemblyDefinition in assemblyDefinitions)
      {
        using (Collection<TypeDefinition>.Enumerator enumerator = assemblyDefinition.get_MainModule().get_Types().GetEnumerator())
        {
          // ISSUE: explicit reference operation
          while (((Collection<TypeDefinition>.Enumerator) @enumerator).MoveNext())
          {
            // ISSUE: explicit reference operation
            TypeDefinition current = ((Collection<TypeDefinition>.Enumerator) @enumerator).get_Current();
            if (((TypeReference) current).get_Namespace().StartsWith("UnityEngine") && (current.get_Fields().get_Count() > 0 || current.get_Methods().get_Count() > 0 || current.get_Properties().get_Count() > 0))
            {
              string name = ((TypeReference) current).get_Name();
              stringSet.Add(name);
            }
          }
        }
      }
      AssemblyDefinition assemblyDefinition1 = (AssemblyDefinition) null;
      for (int index = 0; index < assemblyFileNames.Length; ++index)
      {
        if (assemblyFileNames[index] == "UnityEngine.dll")
          assemblyDefinition1 = assemblyDefinitions[index];
      }
      foreach (AssemblyDefinition assemblyDefinition2 in assemblyDefinitions)
      {
        if (assemblyDefinition2 != assemblyDefinition1)
        {
          using (IEnumerator<TypeReference> enumerator = assemblyDefinition2.get_MainModule().GetTypeReferences().GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              TypeReference current = enumerator.Current;
              if (current.get_Namespace().StartsWith("UnityEngine"))
              {
                string name = current.get_Name();
                stringSet.Add(name);
              }
            }
          }
        }
      }
      return stringSet;
    }

    private static void WriteStaticallyLinkedModuleRegistration(TextWriter w, HashSet<string> nativeModules, HashSet<string> nativeClasses)
    {
      w.WriteLine("struct ClassRegistrationContext;");
      w.WriteLine("void InvokeRegisterStaticallyLinkedModuleClasses(ClassRegistrationContext& context)");
      w.WriteLine("{");
      if (nativeClasses == null)
      {
        w.WriteLine("\tvoid RegisterStaticallyLinkedModuleClasses(ClassRegistrationContext&);");
        w.WriteLine("\tRegisterStaticallyLinkedModuleClasses(context);");
      }
      else
        w.WriteLine("\t// Do nothing (we're in stripping mode)");
      w.WriteLine("}");
      w.WriteLine();
      w.WriteLine("void RegisterStaticallyLinkedModulesGranular()");
      w.WriteLine("{");
      using (HashSet<string>.Enumerator enumerator = nativeModules.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          w.WriteLine("\tvoid RegisterModule_" + current + "();");
          w.WriteLine("\tRegisterModule_" + current + "();");
          w.WriteLine();
        }
      }
      w.WriteLine("}");
    }

    private static void WriteModuleAndClassRegistrationFile(string file, HashSet<string> nativeModules, HashSet<string> nativeClasses, HashSet<string> classesToSkip)
    {
      using (TextWriter w = (TextWriter) new StreamWriter(file))
      {
        CodeStrippingUtils.WriteStaticallyLinkedModuleRegistration(w, nativeModules, nativeClasses);
        w.WriteLine();
        w.WriteLine("void RegisterAllClasses()");
        w.WriteLine("{");
        if (nativeClasses == null)
        {
          w.WriteLine("\tvoid RegisterAllClassesGranular();");
          w.WriteLine("\tRegisterAllClassesGranular();");
        }
        else
        {
          w.WriteLine("\t//Total: {0} classes", (object) nativeClasses.Count);
          int num = 0;
          using (HashSet<string>.Enumerator enumerator = nativeClasses.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              w.WriteLine("\t//{0}. {1}", (object) num, (object) current);
              if (classesToSkip.Contains(current))
              {
                w.WriteLine("\t//Skipping {0}", (object) current);
              }
              else
              {
                w.WriteLine("\tvoid RegisterClass_{0}();", (object) current);
                w.WriteLine("\tRegisterClass_{0}();", (object) current);
              }
              w.WriteLine();
              ++num;
            }
          }
        }
        w.WriteLine("}");
        w.Close();
      }
    }

    private static string[] GetUserAssemblies(string strippedAssemblyDir)
    {
      List<string> stringList = new List<string>();
      foreach (string userAssembly in CodeStrippingUtils.s_UserAssemblies)
        stringList.AddRange(CodeStrippingUtils.GetAssembliesInDirectory(strippedAssemblyDir, userAssembly));
      return stringList.ToArray();
    }

    private static IEnumerable<string> GetAssembliesInDirectory(string strippedAssemblyDir, string assemblyName)
    {
      return (IEnumerable<string>) Directory.GetFiles(strippedAssemblyDir, assemblyName, SearchOption.TopDirectoryOnly);
    }
  }
}
