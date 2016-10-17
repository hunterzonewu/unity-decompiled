// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.IL2CPPUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class IL2CPPUtils
  {
    internal static string editorIl2cppFolder
    {
      get
      {
        return Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, Application.platform != RuntimePlatform.OSXEditor ? "il2cpp" : "Frameworks/il2cpp"));
      }
    }

    internal static IIl2CppPlatformProvider PlatformProviderForNotModularPlatform(BuildTarget target, bool developmentBuild)
    {
      throw new Exception("Platform unsupported, or already modular.");
    }

    internal static IL2CPPBuilder RunIl2Cpp(string tempFolder, string stagingAreaData, IIl2CppPlatformProvider platformProvider, System.Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry, bool developmentBuild)
    {
      IL2CPPBuilder il2CppBuilder = new IL2CPPBuilder(tempFolder, stagingAreaData, platformProvider, modifyOutputBeforeCompile, runtimeClassRegistry, developmentBuild);
      il2CppBuilder.Run();
      return il2CppBuilder;
    }

    internal static IL2CPPBuilder RunIl2Cpp(string stagingAreaData, IIl2CppPlatformProvider platformProvider, System.Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry, bool developmentBuild)
    {
      IL2CPPBuilder il2CppBuilder = new IL2CPPBuilder(stagingAreaData, stagingAreaData, platformProvider, modifyOutputBeforeCompile, runtimeClassRegistry, developmentBuild);
      il2CppBuilder.Run();
      return il2CppBuilder;
    }

    internal static void CopyEmbeddedResourceFiles(string tempFolder, string destinationFolder)
    {
      foreach (string str in ((IEnumerable<string>) Directory.GetFiles(Paths.Combine(IL2CPPBuilder.GetCppOutputPath(tempFolder), "Data", "Resources"))).Where<string>((Func<string, bool>) (f => f.EndsWith("-resources.dat"))))
        File.Copy(str, Paths.Combine(destinationFolder, Path.GetFileName(str)), 1 != 0);
    }

    internal static void CopySymmapFile(string tempFolder, string destinationFolder)
    {
      string str = Path.Combine(tempFolder, "SymbolMap");
      if (!File.Exists(str))
        return;
      File.Copy(str, Path.Combine(destinationFolder, "SymbolMap"), true);
    }

    internal static void CopyMetadataFiles(string tempFolder, string destinationFolder)
    {
      foreach (string str in ((IEnumerable<string>) Directory.GetFiles(Paths.Combine(IL2CPPBuilder.GetCppOutputPath(tempFolder), "Data", "Metadata"))).Where<string>((Func<string, bool>) (f => f.EndsWith("-metadata.dat"))))
        File.Copy(str, Paths.Combine(destinationFolder, Path.GetFileName(str)), 1 != 0);
    }
  }
}
