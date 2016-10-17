// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.ICompilationExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using Mono.Cecil;

namespace UnityEditor.Modules
{
  internal interface ICompilationExtension
  {
    CSharpCompiler GetCsCompiler(bool buildingForEditor, string assemblyName);

    string[] GetCompilerExtraAssemblyPaths(bool isEditor, string assemblyPathName);

    IAssemblyResolver GetAssemblyResolver(bool buildingForEditor, string assemblyPath, string[] searchDirectories);
  }
}
