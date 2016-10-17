// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoAssemblyStripping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor
{
  internal class MonoAssemblyStripping
  {
    private static void ReplaceFile(string src, string dst)
    {
      if (File.Exists(dst))
        FileUtil.DeleteFileOrDirectory(dst);
      FileUtil.CopyFileOrDirectory(src, dst);
    }

    public static void MonoCilStrip(BuildTarget buildTarget, string managedLibrariesDirectory, string[] fileNames)
    {
      string str1 = Path.Combine(BuildPipeline.GetBuildToolsDirectory(buildTarget), "mono-cil-strip.exe");
      foreach (string fileName in fileNames)
      {
        Process process = MonoProcessUtility.PrepareMonoProcess(buildTarget, managedLibrariesDirectory);
        string path2 = fileName + ".out";
        process.StartInfo.Arguments = "\"" + str1 + "\"";
        ProcessStartInfo startInfo = process.StartInfo;
        string str2 = startInfo.Arguments + " \"" + fileName + "\" \"" + fileName + ".out\"";
        startInfo.Arguments = str2;
        MonoProcessUtility.RunMonoProcess(process, "byte code stripper", Path.Combine(managedLibrariesDirectory, path2));
        MonoAssemblyStripping.ReplaceFile(managedLibrariesDirectory + "/" + path2, managedLibrariesDirectory + "/" + fileName);
        File.Delete(managedLibrariesDirectory + "/" + path2);
      }
    }

    public static string GenerateBlackList(string librariesFolder, RuntimeClassRegistry usedClasses, string[] allAssemblies)
    {
      string path2 = "tmplink.xml";
      usedClasses.SynchronizeClasses();
      using (TextWriter w = (TextWriter) new StreamWriter(Path.Combine(librariesFolder, path2)))
      {
        w.WriteLine("<linker>");
        w.WriteLine("<assembly fullname=\"UnityEngine\">");
        using (List<string>.Enumerator enumerator = usedClasses.GetAllManagedClassesAsString().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            string current = enumerator.Current;
            w.WriteLine(string.Format("<type fullname=\"UnityEngine.{0}\" preserve=\"{1}\"/>", (object) current, (object) usedClasses.GetRetentionLevel(current)));
          }
        }
        w.WriteLine("</assembly>");
        DefaultAssemblyResolver assemblyResolver1 = new DefaultAssemblyResolver();
        ((BaseAssemblyResolver) assemblyResolver1).AddSearchDirectory(librariesFolder);
        foreach (string allAssembly in allAssemblies)
        {
          DefaultAssemblyResolver assemblyResolver2 = assemblyResolver1;
          string withoutExtension = Path.GetFileNameWithoutExtension(allAssembly);
          ReaderParameters readerParameters1 = new ReaderParameters();
          readerParameters1.set_AssemblyResolver((IAssemblyResolver) assemblyResolver1);
          ReaderParameters readerParameters2 = readerParameters1;
          AssemblyDefinition assemblyDefinition = ((BaseAssemblyResolver) assemblyResolver2).Resolve(withoutExtension, readerParameters2);
          w.WriteLine("<assembly fullname=\"{0}\">", (object) ((AssemblyNameReference) assemblyDefinition.get_Name()).get_Name());
          if (((AssemblyNameReference) assemblyDefinition.get_Name()).get_Name().StartsWith("UnityEngine."))
          {
            using (List<string>.Enumerator enumerator = usedClasses.GetAllManagedClassesAsString().GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string current = enumerator.Current;
                w.WriteLine(string.Format("<type fullname=\"UnityEngine.{0}\" preserve=\"{1}\"/>", (object) current, (object) usedClasses.GetRetentionLevel(current)));
              }
            }
          }
          MonoAssemblyStripping.GenerateBlackListTypeXML(w, (IList<TypeDefinition>) assemblyDefinition.get_MainModule().get_Types(), usedClasses.GetAllManagedBaseClassesAsString());
          w.WriteLine("</assembly>");
        }
        w.WriteLine("</linker>");
      }
      return path2;
    }

    public static string GenerateLinkXmlToPreserveDerivedTypes(string stagingArea, string librariesFolder, RuntimeClassRegistry usedClasses)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MonoAssemblyStripping.\u003CGenerateLinkXmlToPreserveDerivedTypes\u003Ec__AnonStorey6C typesCAnonStorey6C = new MonoAssemblyStripping.\u003CGenerateLinkXmlToPreserveDerivedTypes\u003Ec__AnonStorey6C();
      // ISSUE: reference to a compiler-generated field
      typesCAnonStorey6C.usedClasses = usedClasses;
      string fullPath = Path.GetFullPath(Path.Combine(stagingArea, "preserved_derived_types.xml"));
      // ISSUE: reference to a compiler-generated field
      typesCAnonStorey6C.resolver = new DefaultAssemblyResolver();
      // ISSUE: reference to a compiler-generated field
      ((BaseAssemblyResolver) typesCAnonStorey6C.resolver).AddSearchDirectory(librariesFolder);
      using (TextWriter textWriter = (TextWriter) new StreamWriter(fullPath))
      {
        textWriter.WriteLine("<linker>");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        using (HashSet<AssemblyDefinition>.Enumerator enumerator1 = MonoAssemblyStripping.CollectAssembliesRecursive(((IEnumerable<string>) typesCAnonStorey6C.usedClasses.GetUserAssemblies()).Where<string>(new Func<string, bool>(typesCAnonStorey6C.\u003C\u003Em__EC)).Select<string, AssemblyDefinition>(new Func<string, AssemblyDefinition>(typesCAnonStorey6C.\u003C\u003Em__ED))).GetEnumerator())
        {
          while (enumerator1.MoveNext())
          {
            AssemblyDefinition current1 = enumerator1.Current;
            if (!(((AssemblyNameReference) current1.get_Name()).get_Name() == "UnityEngine"))
            {
              HashSet<TypeDefinition> typesToPreserve = new HashSet<TypeDefinition>();
              // ISSUE: reference to a compiler-generated field
              MonoAssemblyStripping.CollectBlackListTypes(typesToPreserve, (IList<TypeDefinition>) current1.get_MainModule().get_Types(), typesCAnonStorey6C.usedClasses.GetAllManagedBaseClassesAsString());
              if (typesToPreserve.Count != 0)
              {
                textWriter.WriteLine("<assembly fullname=\"{0}\">", (object) ((AssemblyNameReference) current1.get_Name()).get_Name());
                using (HashSet<TypeDefinition>.Enumerator enumerator2 = typesToPreserve.GetEnumerator())
                {
                  while (enumerator2.MoveNext())
                  {
                    TypeDefinition current2 = enumerator2.Current;
                    textWriter.WriteLine("<type fullname=\"{0}\" preserve=\"all\"/>", (object) ((TypeReference) current2).get_FullName());
                  }
                }
                textWriter.WriteLine("</assembly>");
              }
            }
          }
        }
        textWriter.WriteLine("</linker>");
      }
      return fullPath;
    }

    private static HashSet<AssemblyDefinition> CollectAssembliesRecursive(IEnumerable<AssemblyDefinition> assemblies)
    {
      HashSet<AssemblyDefinition> assemblyDefinitionSet = new HashSet<AssemblyDefinition>(assemblies, (IEqualityComparer<AssemblyDefinition>) new MonoAssemblyStripping.AssemblyDefinitionComparer());
      int num = 0;
      while (assemblyDefinitionSet.Count > num)
      {
        num = assemblyDefinitionSet.Count;
        assemblyDefinitionSet.UnionWith(((IEnumerable<AssemblyDefinition>) ((IEnumerable<AssemblyDefinition>) assemblyDefinitionSet).ToArray<AssemblyDefinition>()).SelectMany<AssemblyDefinition, AssemblyDefinition>((Func<AssemblyDefinition, IEnumerable<AssemblyDefinition>>) (assembly =>
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          MonoAssemblyStripping.\u003CCollectAssembliesRecursive\u003Ec__AnonStorey6D recursiveCAnonStorey6D = new MonoAssemblyStripping.\u003CCollectAssembliesRecursive\u003Ec__AnonStorey6D();
          // ISSUE: reference to a compiler-generated field
          recursiveCAnonStorey6D.assembly = assembly;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          return ((IEnumerable<AssemblyNameReference>) recursiveCAnonStorey6D.assembly.get_MainModule().get_AssemblyReferences()).Select<AssemblyNameReference, AssemblyDefinition>(new Func<AssemblyNameReference, AssemblyDefinition>(recursiveCAnonStorey6D.\u003C\u003Em__EF));
        })));
      }
      return assemblyDefinitionSet;
    }

    private static void CollectBlackListTypes(HashSet<TypeDefinition> typesToPreserve, IList<TypeDefinition> types, List<string> baseTypes)
    {
      if (types == null)
        return;
      using (IEnumerator<TypeDefinition> enumerator1 = ((IEnumerable<TypeDefinition>) types).GetEnumerator())
      {
        while (((IEnumerator) enumerator1).MoveNext())
        {
          TypeDefinition current1 = enumerator1.Current;
          if (current1 != null)
          {
            using (List<string>.Enumerator enumerator2 = baseTypes.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                string current2 = enumerator2.Current;
                if (MonoAssemblyStripping.DoesTypeEnheritFrom((TypeReference) current1, current2))
                {
                  typesToPreserve.Add(current1);
                  break;
                }
              }
            }
            MonoAssemblyStripping.CollectBlackListTypes(typesToPreserve, (IList<TypeDefinition>) current1.get_NestedTypes(), baseTypes);
          }
        }
      }
    }

    private static void GenerateBlackListTypeXML(TextWriter w, IList<TypeDefinition> types, List<string> baseTypes)
    {
      HashSet<TypeDefinition> typesToPreserve = new HashSet<TypeDefinition>();
      MonoAssemblyStripping.CollectBlackListTypes(typesToPreserve, types, baseTypes);
      using (HashSet<TypeDefinition>.Enumerator enumerator = typesToPreserve.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TypeDefinition current = enumerator.Current;
          w.WriteLine("<type fullname=\"{0}\" preserve=\"all\"/>", (object) ((TypeReference) current).get_FullName());
        }
      }
    }

    private static bool DoesTypeEnheritFrom(TypeReference type, string typeName)
    {
      TypeDefinition typeDefinition;
      for (; type != null; type = typeDefinition.get_BaseType())
      {
        if (type.get_FullName() == typeName)
          return true;
        typeDefinition = type.Resolve();
        if (typeDefinition == null)
          return false;
      }
      return false;
    }

    private static string StripperExe()
    {
      return Application.platform == RuntimePlatform.WindowsEditor ? "Tools/UnusedBytecodeStripper.exe" : "Tools/UnusedByteCodeStripper/UnusedBytecodeStripper.exe";
    }

    public static void MonoLink(BuildTarget buildTarget, string managedLibrariesDirectory, string[] input, string[] allAssemblies, RuntimeClassRegistry usedClasses)
    {
      Process process = MonoProcessUtility.PrepareMonoProcess(buildTarget, managedLibrariesDirectory);
      string buildToolsDirectory = BuildPipeline.GetBuildToolsDirectory(buildTarget);
      string path2 = (string) null;
      string path1 = Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), MonoAssemblyStripping.StripperExe());
      string str1 = Path.Combine(Path.GetDirectoryName(path1), "link.xml");
      string str2 = Path.Combine(managedLibrariesDirectory, "output");
      Directory.CreateDirectory(str2);
      process.StartInfo.Arguments = "\"" + path1 + "\" -l none -c link";
      foreach (string str3 in input)
      {
        ProcessStartInfo startInfo = process.StartInfo;
        string str4 = startInfo.Arguments + " -a \"" + str3 + "\"";
        startInfo.Arguments = str4;
      }
      ProcessStartInfo startInfo1 = process.StartInfo;
      string str5 = startInfo1.Arguments + " -out output -x \"" + str1 + "\" -d \"" + managedLibrariesDirectory + "\"";
      startInfo1.Arguments = str5;
      string path3 = Path.Combine(buildToolsDirectory, "link.xml");
      if (File.Exists(path3))
      {
        ProcessStartInfo startInfo2 = process.StartInfo;
        string str3 = startInfo2.Arguments + " -x \"" + path3 + "\"";
        startInfo2.Arguments = str3;
      }
      string path4 = Path.Combine(Path.GetDirectoryName(path1), "Core.xml");
      if (File.Exists(path4))
      {
        ProcessStartInfo startInfo2 = process.StartInfo;
        string str3 = startInfo2.Arguments + " -x \"" + path4 + "\"";
        startInfo2.Arguments = str3;
      }
      foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Assets"), "link.xml", SearchOption.AllDirectories))
      {
        ProcessStartInfo startInfo2 = process.StartInfo;
        string str3 = startInfo2.Arguments + " -x \"" + file + "\"";
        startInfo2.Arguments = str3;
      }
      if (usedClasses != null)
      {
        path2 = MonoAssemblyStripping.GenerateBlackList(managedLibrariesDirectory, usedClasses, allAssemblies);
        ProcessStartInfo startInfo2 = process.StartInfo;
        string str3 = startInfo2.Arguments + " -x \"" + path2 + "\"";
        startInfo2.Arguments = str3;
      }
      foreach (string file in Directory.GetFiles(Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(EditorUserBuildSettings.activeBuildTarget, BuildOptions.None), "Whitelists"), "*.xml"))
      {
        ProcessStartInfo startInfo2 = process.StartInfo;
        string str3 = startInfo2.Arguments + " -x \"" + file + "\"";
        startInfo2.Arguments = str3;
      }
      MonoProcessUtility.RunMonoProcess(process, "assemblies stripper", Path.Combine(str2, "mscorlib.dll"));
      MonoAssemblyStripping.DeleteAllDllsFrom(managedLibrariesDirectory);
      MonoAssemblyStripping.CopyAllDlls(managedLibrariesDirectory, str2);
      foreach (string file in Directory.GetFiles(managedLibrariesDirectory))
      {
        if (file.Contains(".mdb") && !File.Exists(file.Replace(".mdb", string.Empty)))
          FileUtil.DeleteFileOrDirectory(file);
      }
      if (path2 != null)
        FileUtil.DeleteFileOrDirectory(Path.Combine(managedLibrariesDirectory, path2));
      FileUtil.DeleteFileOrDirectory(str2);
    }

    private static void CopyFiles(IEnumerable<string> files, string fromDir, string toDir)
    {
      foreach (string file in files)
        FileUtil.ReplaceFile(Path.Combine(fromDir, file), Path.Combine(toDir, file));
    }

    private static void CopyAllDlls(string fromDir, string toDir)
    {
      foreach (FileInfo file in new DirectoryInfo(toDir).GetFiles("*.dll"))
        FileUtil.ReplaceFile(Path.Combine(toDir, file.Name), Path.Combine(fromDir, file.Name));
    }

    private static void DeleteAllDllsFrom(string managedLibrariesDirectory)
    {
      foreach (FileSystemInfo file in new DirectoryInfo(managedLibrariesDirectory).GetFiles("*.dll"))
        FileUtil.DeleteFileOrDirectory(file.FullName);
    }

    private class AssemblyDefinitionComparer : IEqualityComparer<AssemblyDefinition>
    {
      public bool Equals(AssemblyDefinition x, AssemblyDefinition y)
      {
        return x.get_FullName() == y.get_FullName();
      }

      public int GetHashCode(AssemblyDefinition obj)
      {
        return obj.get_FullName().GetHashCode();
      }
    }
  }
}
