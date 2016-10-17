// Decompiled with JetBrains decompiler
// Type: UnityEditor.GccCompilerSettingsx86_64
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class GccCompilerSettingsx86_64 : ICompilerSettings
  {
    public string[] LibPaths
    {
      get
      {
        return new string[0];
      }
    }

    public string CompilerPath
    {
      get
      {
        return "/usr/bin/g++";
      }
    }

    public string LinkerPath
    {
      get
      {
        return this.CompilerPath;
      }
    }

    public string MachineSpecification
    {
      get
      {
        return "-m64";
      }
    }
  }
}
