// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoAOTRegistration
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;
using Mono.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
  internal class MonoAOTRegistration
  {
    private static void ExtractNativeMethodsFromTypes(ICollection<TypeDefinition> types, ArrayList res)
    {
      using (IEnumerator<TypeDefinition> enumerator1 = ((IEnumerable<TypeDefinition>) types).GetEnumerator())
      {
        while (((IEnumerator) enumerator1).MoveNext())
        {
          TypeDefinition current1 = enumerator1.Current;
          using (Collection<MethodDefinition>.Enumerator enumerator2 = current1.get_Methods().GetEnumerator())
          {
            // ISSUE: explicit reference operation
            while (((Collection<MethodDefinition>.Enumerator) @enumerator2).MoveNext())
            {
              // ISSUE: explicit reference operation
              MethodDefinition current2 = ((Collection<MethodDefinition>.Enumerator) @enumerator2).get_Current();
              if (current2.get_IsStatic() && current2.get_IsPInvokeImpl() && current2.get_PInvokeInfo().get_Module().get_Name().Equals("__Internal"))
              {
                if (res.Contains((object) ((MemberReference) current2).get_Name()))
                  throw new SystemException("Duplicate native method found : " + ((MemberReference) current2).get_Name() + ". Please check your source carefully.");
                res.Add((object) ((MemberReference) current2).get_Name());
              }
            }
          }
          if (current1.get_HasNestedTypes())
            MonoAOTRegistration.ExtractNativeMethodsFromTypes((ICollection<TypeDefinition>) current1.get_NestedTypes(), res);
        }
      }
    }

    private static ArrayList BuildNativeMethodList(AssemblyDefinition[] assemblies)
    {
      ArrayList res = new ArrayList();
      foreach (AssemblyDefinition assembly in assemblies)
      {
        if (!"System".Equals(((AssemblyNameReference) assembly.get_Name()).get_Name()))
          MonoAOTRegistration.ExtractNativeMethodsFromTypes((ICollection<TypeDefinition>) assembly.get_MainModule().get_Types(), res);
      }
      return res;
    }

    public static HashSet<string> BuildReferencedTypeList(AssemblyDefinition[] assemblies)
    {
      HashSet<string> stringSet = new HashSet<string>();
      foreach (AssemblyDefinition assembly in assemblies)
      {
        if (!((AssemblyNameReference) assembly.get_Name()).get_Name().StartsWith("System") && !((AssemblyNameReference) assembly.get_Name()).get_Name().Equals("UnityEngine"))
        {
          using (IEnumerator<TypeReference> enumerator = assembly.get_MainModule().GetTypeReferences().GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              TypeReference current = enumerator.Current;
              stringSet.Add(current.get_FullName());
            }
          }
        }
      }
      return stringSet;
    }

    public static void WriteCPlusPlusFileForStaticAOTModuleRegistration(BuildTarget buildTarget, string file, CrossCompileOptions crossCompileOptions, bool advancedLic, string targetDevice, bool stripping, RuntimeClassRegistry usedClassRegistry, AssemblyReferenceChecker checker)
    {
      using (TextWriter output = (TextWriter) new StreamWriter(file))
      {
        string[] assemblyFileNames = checker.GetAssemblyFileNames();
        AssemblyDefinition[] assemblyDefinitions = checker.GetAssemblyDefinitions();
        bool flag = (crossCompileOptions & CrossCompileOptions.FastICall) != CrossCompileOptions.Dynamic;
        ArrayList arrayList = MonoAOTRegistration.BuildNativeMethodList(assemblyDefinitions);
        if (buildTarget == BuildTarget.iOS)
        {
          output.WriteLine("#include \"RegisterMonoModules.h\"");
          output.WriteLine("#include <stdio.h>");
        }
        output.WriteLine(string.Empty);
        output.WriteLine("#if defined(TARGET_IPHONE_SIMULATOR) && TARGET_IPHONE_SIMULATOR");
        output.WriteLine("    #define DECL_USER_FUNC(f) void f() __attribute__((weak_import))");
        output.WriteLine("    #define REGISTER_USER_FUNC(f)\\");
        output.WriteLine("        do {\\");
        output.WriteLine("        if(f != NULL)\\");
        output.WriteLine("            mono_dl_register_symbol(#f, (void*)f);\\");
        output.WriteLine("        else\\");
        output.WriteLine("            ::printf_console(\"Symbol '%s' not found. Maybe missing implementation for Simulator?\\n\", #f);\\");
        output.WriteLine("        }while(0)");
        output.WriteLine("#else");
        output.WriteLine("    #define DECL_USER_FUNC(f) void f() ");
        output.WriteLine("    #if !defined(__arm64__)");
        output.WriteLine("    #define REGISTER_USER_FUNC(f) mono_dl_register_symbol(#f, (void*)&f)");
        output.WriteLine("    #else");
        output.WriteLine("        #define REGISTER_USER_FUNC(f)");
        output.WriteLine("    #endif");
        output.WriteLine("#endif");
        output.WriteLine("extern \"C\"\n{");
        output.WriteLine("    typedef void* gpointer;");
        output.WriteLine("    typedef int gboolean;");
        if (buildTarget == BuildTarget.iOS)
        {
          output.WriteLine("    const char*         UnityIPhoneRuntimeVersion = \"{0}\";", (object) Application.unityVersion);
          output.WriteLine("    void                mono_dl_register_symbol (const char* name, void *addr);");
          output.WriteLine("#if !defined(__arm64__)");
          output.WriteLine("    extern int          mono_ficall_flag;");
          output.WriteLine("#endif");
        }
        output.WriteLine("    void                mono_aot_register_module(gpointer *aot_info);");
        output.WriteLine("#if __ORBIS__ || SN_TARGET_PSP2");
        output.WriteLine("#define DLL_EXPORT __declspec(dllexport)");
        output.WriteLine("#else");
        output.WriteLine("#define DLL_EXPORT");
        output.WriteLine("#endif");
        output.WriteLine("#if !(TARGET_IPHONE_SIMULATOR)");
        output.WriteLine("    extern gboolean     mono_aot_only;");
        for (int index = 0; index < assemblyFileNames.Length; ++index)
        {
          string str1 = assemblyFileNames[index];
          string str2 = ((AssemblyNameReference) assemblyDefinitions[index].get_Name()).get_Name().Replace(".", "_").Replace("-", "_").Replace(" ", "_");
          output.WriteLine("    extern gpointer*    mono_aot_module_{0}_info; // {1}", (object) str2, (object) str1);
        }
        output.WriteLine("#endif // !(TARGET_IPHONE_SIMULATOR)");
        foreach (string str in arrayList)
          output.WriteLine("    DECL_USER_FUNC({0});", (object) str);
        output.WriteLine("}");
        output.WriteLine("DLL_EXPORT void RegisterMonoModules()");
        output.WriteLine("{");
        output.WriteLine("#if !(TARGET_IPHONE_SIMULATOR) && !defined(__arm64__)");
        output.WriteLine("    mono_aot_only = true;");
        if (buildTarget == BuildTarget.iOS)
          output.WriteLine("    mono_ficall_flag = {0};", !flag ? (object) "false" : (object) "true");
        foreach (AssemblyDefinition assemblyDefinition in assemblyDefinitions)
        {
          string str = ((AssemblyNameReference) assemblyDefinition.get_Name()).get_Name().Replace(".", "_").Replace("-", "_").Replace(" ", "_");
          output.WriteLine("    mono_aot_register_module(mono_aot_module_{0}_info);", (object) str);
        }
        output.WriteLine("#endif // !(TARGET_IPHONE_SIMULATOR) && !defined(__arm64__)");
        output.WriteLine(string.Empty);
        if (buildTarget == BuildTarget.iOS)
        {
          foreach (string str in arrayList)
            output.WriteLine("    REGISTER_USER_FUNC({0});", (object) str);
        }
        output.WriteLine("}");
        output.WriteLine(string.Empty);
        AssemblyDefinition unityEngine = (AssemblyDefinition) null;
        for (int index = 0; index < assemblyFileNames.Length; ++index)
        {
          if (assemblyFileNames[index] == "UnityEngine.dll")
            unityEngine = assemblyDefinitions[index];
        }
        if (buildTarget == BuildTarget.iOS)
        {
          AssemblyDefinition[] assemblies = new AssemblyDefinition[1]{ unityEngine };
          MonoAOTRegistration.GenerateRegisterInternalCalls(assemblies, output);
          MonoAOTRegistration.ResolveDefinedNativeClassesFromMono(assemblies, usedClassRegistry);
          MonoAOTRegistration.ResolveReferencedUnityEngineClassesFromMono(assemblyDefinitions, unityEngine, usedClassRegistry);
          MonoAOTRegistration.GenerateRegisterModules(usedClassRegistry, output, stripping);
          if (stripping && usedClassRegistry != null)
            MonoAOTRegistration.GenerateRegisterClassesForStripping(usedClassRegistry, output);
          else
            MonoAOTRegistration.GenerateRegisterClasses(usedClassRegistry, output);
        }
        output.Close();
      }
    }

    public static void ResolveReferencedUnityEngineClassesFromMono(AssemblyDefinition[] assemblies, AssemblyDefinition unityEngine, RuntimeClassRegistry res)
    {
      if (res == null)
        return;
      foreach (AssemblyDefinition assembly in assemblies)
      {
        if (assembly != unityEngine)
        {
          using (IEnumerator<TypeReference> enumerator = assembly.get_MainModule().GetTypeReferences().GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              TypeReference current = enumerator.Current;
              if (current.get_Namespace().StartsWith("UnityEngine"))
              {
                string name = current.get_Name();
                res.AddMonoClass(name);
              }
            }
          }
        }
      }
    }

    public static void ResolveDefinedNativeClassesFromMono(AssemblyDefinition[] assemblies, RuntimeClassRegistry res)
    {
      if (res == null)
        return;
      foreach (AssemblyDefinition assembly in assemblies)
      {
        using (Collection<TypeDefinition>.Enumerator enumerator = assembly.get_MainModule().get_Types().GetEnumerator())
        {
          // ISSUE: explicit reference operation
          while (((Collection<TypeDefinition>.Enumerator) @enumerator).MoveNext())
          {
            // ISSUE: explicit reference operation
            TypeDefinition current = ((Collection<TypeDefinition>.Enumerator) @enumerator).get_Current();
            if (current.get_Fields().get_Count() > 0 || current.get_Methods().get_Count() > 0 || current.get_Properties().get_Count() > 0)
            {
              string name = ((TypeReference) current).get_Name();
              res.AddMonoClass(name);
            }
          }
        }
      }
    }

    public static void GenerateRegisterModules(RuntimeClassRegistry allClasses, TextWriter output, bool strippingEnabled)
    {
      allClasses.SynchronizeClasses();
      HashSet<string> modulesToRegister = CodeStrippingUtils.GetNativeModulesToRegister(!strippingEnabled ? (HashSet<string>) null : new HashSet<string>((IEnumerable<string>) allClasses.GetAllNativeClassesAsString()));
      modulesToRegister.Add("IMGUI");
      using (HashSet<string>.Enumerator enumerator = modulesToRegister.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          output.WriteLine("\textern \"C\" void RegisterModule_" + current + "();");
        }
      }
      output.WriteLine("void RegisterStaticallyLinkedModules()");
      output.WriteLine("{");
      using (HashSet<string>.Enumerator enumerator = modulesToRegister.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          output.WriteLine("\tRegisterModule_" + current + "();");
        }
      }
      output.WriteLine("}");
    }

    public static void GenerateRegisterClassesForStripping(RuntimeClassRegistry allClasses, TextWriter output)
    {
      output.Write("void RegisterAllClasses() \n{\n");
      allClasses.SynchronizeClasses();
      using (List<string>.Enumerator enumerator = allClasses.GetAllNativeClassesAsString().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          output.WriteLine(string.Format("extern int RegisterClass_{0}();\nRegisterClass_{0}();", (object) current));
        }
      }
      output.Write("\n}\n");
    }

    public static void GenerateRegisterClasses(RuntimeClassRegistry allClasses, TextWriter output)
    {
      output.Write("void RegisterAllClasses() \n{\n");
      output.Write("void RegisterAllClassesIPhone();\nRegisterAllClassesIPhone();\n");
      output.Write("\n}\n");
    }

    public static void GenerateRegisterInternalCalls(AssemblyDefinition[] assemblies, TextWriter output)
    {
      output.Write("void RegisterAllStrippedInternalCalls ()\n{\n");
      foreach (AssemblyDefinition assembly in assemblies)
        MonoAOTRegistration.GenerateRegisterInternalCallsForTypes((IEnumerable<TypeDefinition>) assembly.get_MainModule().get_Types(), output);
      output.Write("\n}\n");
    }

    private static void GenerateRegisterInternalCallsForTypes(IEnumerable<TypeDefinition> types, TextWriter output)
    {
      using (IEnumerator<TypeDefinition> enumerator1 = types.GetEnumerator())
      {
        while (((IEnumerator) enumerator1).MoveNext())
        {
          TypeDefinition current1 = enumerator1.Current;
          using (Collection<MethodDefinition>.Enumerator enumerator2 = current1.get_Methods().GetEnumerator())
          {
            // ISSUE: explicit reference operation
            while (((Collection<MethodDefinition>.Enumerator) @enumerator2).MoveNext())
            {
              // ISSUE: explicit reference operation
              MethodDefinition current2 = ((Collection<MethodDefinition>.Enumerator) @enumerator2).get_Current();
              MonoAOTRegistration.GenerateInternalCallMethod(current1, current2, output);
            }
          }
          MonoAOTRegistration.GenerateRegisterInternalCallsForTypes((IEnumerable<TypeDefinition>) current1.get_NestedTypes(), output);
        }
      }
    }

    private static void GenerateInternalCallMethod(TypeDefinition typeDefinition, MethodDefinition method, TextWriter output)
    {
      if (!method.get_IsInternalCall())
        return;
      string str = (((TypeReference) typeDefinition).get_FullName() + "_" + ((MemberReference) method).get_Name()).Replace('/', '_').Replace('.', '_');
      if (str.Contains("UnityEngine_Serialization"))
        return;
      output.WriteLine("\tvoid Register_{0} ();", (object) str);
      output.WriteLine("\tRegister_{0} ();", (object) str);
    }
  }
}
