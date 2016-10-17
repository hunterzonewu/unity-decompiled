// Decompiled with JetBrains decompiler
// Type: ClangCompilerSettingsx86
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

internal class ClangCompilerSettingsx86 : ICompilerSettings
{
  public string CompilerPath
  {
    get
    {
      return "clang++";
    }
  }

  public string LinkerPath
  {
    get
    {
      return "ld";
    }
  }

  public string[] LibPaths
  {
    get
    {
      return new string[0];
    }
  }

  public string MachineSpecification
  {
    get
    {
      return "i386";
    }
  }
}
