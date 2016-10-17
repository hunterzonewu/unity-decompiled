// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Serialization.Weaver
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.UNetWeaver;
using UnityEditor.Modules;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Scripting.Serialization
{
  internal static class Weaver
  {
    public static bool ShouldWeave(string name)
    {
      return !name.Contains("Boo.") && !name.Contains("Mono.") && (!name.Contains("System") && name.EndsWith(".dll"));
    }

    private static ManagedProgram SerializationWeaverProgramWith(string arguments, string playerPackage)
    {
      return Weaver.ManagedProgramFor(playerPackage + "/SerializationWeaver/SerializationWeaver.exe", arguments);
    }

    private static ManagedProgram ManagedProgramFor(string exe, string arguments)
    {
      return new ManagedProgram(MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"), "4.0", exe, arguments);
    }

    private static ICompilationExtension GetCompilationExtension()
    {
      return ModuleManager.GetCompilationExtension(ModuleManager.GetTargetStringFromBuildTarget(EditorUserBuildSettings.activeBuildTarget));
    }

    private static void QueryAssemblyPathsAndResolver(ICompilationExtension compilationExtension, string file, bool editor, out string[] assemblyPaths, out IAssemblyResolver assemblyResolver)
    {
      assemblyResolver = compilationExtension.GetAssemblyResolver(editor, file, (string[]) null);
      assemblyPaths = ((IEnumerable<string>) compilationExtension.GetCompilerExtraAssemblyPaths(editor, file)).ToArray<string>();
    }

    public static void WeaveAssembliesInFolder(string folder, string playerPackage)
    {
      ICompilationExtension compilationExtension = Weaver.GetCompilationExtension();
      string unityEngine = Path.Combine(folder, "UnityEngine.dll");
      foreach (string str in ((IEnumerable<string>) Directory.GetFiles(folder)).Where<string>((Func<string, bool>) (f => Weaver.ShouldWeave(Path.GetFileName(f)))))
      {
        string[] assemblyPaths;
        IAssemblyResolver assemblyResolver;
        Weaver.QueryAssemblyPathsAndResolver(compilationExtension, str, false, out assemblyPaths, out assemblyResolver);
        Weaver.WeaveInto(str, str, unityEngine, playerPackage, assemblyPaths, assemblyResolver);
      }
    }

    public static bool WeaveUnetFromEditor(string assemblyPath, string destPath, string unityEngine, string unityUNet, bool buildingForEditor)
    {
      Console.WriteLine("WeaveUnetFromEditor " + assemblyPath);
      string[] assemblyPaths;
      IAssemblyResolver assemblyResolver;
      Weaver.QueryAssemblyPathsAndResolver(Weaver.GetCompilationExtension(), assemblyPath, buildingForEditor, out assemblyPaths, out assemblyResolver);
      return Weaver.WeaveInto(unityUNet, destPath, unityEngine, assemblyPath, assemblyPaths, assemblyResolver);
    }

    public static bool WeaveInto(string unityUNet, string destPath, string unityEngine, string assemblyPath, string[] extraAssemblyPaths, IAssemblyResolver assemblyResolver)
    {
      IEnumerable<MonoIsland> monoIslands = ((IEnumerable<MonoIsland>) InternalEditorUtility.GetMonoIslands()).Where<MonoIsland>((Func<MonoIsland, bool>) (i => 0 < i._files.Length));
      string fullName = Directory.GetParent(Application.dataPath).FullName;
      string[] strArray = (string[]) null;
      foreach (MonoIsland island in monoIslands)
      {
        if (destPath.Equals(island._output))
        {
          strArray = Weaver.GetReferences(island, fullName);
          break;
        }
      }
      if (strArray == null)
      {
        Debug.LogError((object) ("Weaver failure: unable to locate assemblies (no matching project) for: " + destPath));
        return false;
      }
      List<string> stringList = new List<string>();
      foreach (string path in strArray)
        stringList.Add(Path.GetDirectoryName(path));
      if (extraAssemblyPaths != null)
        stringList.AddRange((IEnumerable<string>) extraAssemblyPaths);
      try
      {
        if (!Program.Process(unityEngine, unityUNet, Path.GetDirectoryName(destPath), new string[1]{ assemblyPath }, stringList.ToArray(), assemblyResolver, new System.Action<string>(Debug.LogWarning), new System.Action<string>(Debug.LogError)))
        {
          Debug.LogError((object) "Failure generating network code.");
          return false;
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Exception generating network code: " + ex.ToString() + " " + ex.StackTrace));
      }
      return true;
    }

    public static string[] GetReferences(MonoIsland island, string projectDirectory)
    {
      List<string> stringList = new List<string>();
      foreach (string str in new List<string>().Union<string>((IEnumerable<string>) island._references))
      {
        string fileName = Path.GetFileName(str);
        if (string.IsNullOrEmpty(fileName) || !fileName.Contains("UnityEditor.dll") && !fileName.Contains("UnityEngine.dll"))
        {
          string file = !Path.IsPathRooted(str) ? Path.Combine(projectDirectory, str) : str;
          if (AssemblyHelper.IsManagedAssembly(file) && !AssemblyHelper.IsInternalAssembly(file))
            stringList.Add(file);
        }
      }
      return stringList.ToArray();
    }
  }
}
