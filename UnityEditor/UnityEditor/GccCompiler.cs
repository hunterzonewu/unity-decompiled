// Decompiled with JetBrains decompiler
// Type: UnityEditor.GccCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor
{
  internal class GccCompiler : NativeCompiler
  {
    private readonly ICompilerSettings m_Settings;

    protected override string objectFileExtension
    {
      get
      {
        return "o";
      }
    }

    public GccCompiler(ICompilerSettings settings)
    {
      this.m_Settings = settings;
    }

    private void Compile(string file, string includePaths)
    {
      this.Execute(string.Format(" -c {0} -O0 -Wno-unused-value -Wno-invalid-offsetof -fvisibility=hidden -fno-rtti {1} {2} -o {3}", (object) this.m_Settings.MachineSpecification, (object) includePaths, (object) file, (object) this.ObjectFileFor(file)), this.m_Settings.CompilerPath);
    }

    public override void CompileDynamicLibrary(string outFile, IEnumerable<string> sources, IEnumerable<string> includePaths, IEnumerable<string> libraries, IEnumerable<string> libraryPaths)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GccCompiler.\u003CCompileDynamicLibrary\u003Ec__AnonStorey73 libraryCAnonStorey73 = new GccCompiler.\u003CCompileDynamicLibrary\u003Ec__AnonStorey73();
      // ISSUE: reference to a compiler-generated field
      libraryCAnonStorey73.\u003C\u003Ef__this = this;
      string[] array = sources.ToArray<string>();
      // ISSUE: reference to a compiler-generated field
      libraryCAnonStorey73.includeDirs = includePaths.Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((current, sourceDir) => current + "-I" + sourceDir + " "));
      string empty = string.Empty;
      string str = NativeCompiler.Aggregate(libraryPaths.Union<string>((IEnumerable<string>) this.m_Settings.LibPaths), "-L", " ");
      // ISSUE: reference to a compiler-generated method
      NativeCompiler.ParallelFor<string>(array, new System.Action<string>(libraryCAnonStorey73.\u003C\u003Em__FD));
      this.ExecuteCommand(this.m_Settings.LinkerPath, string.Format("-shared {0} -o {1}", (object) this.m_Settings.MachineSpecification, (object) outFile), ((IEnumerable<string>) array).Where<string>(new Func<string, bool>(NativeCompiler.IsSourceFile)).Select<string, string>(new Func<string, string>(((NativeCompiler) this).ObjectFileFor)).Aggregate<string>((Func<string, string, string>) ((buff, s) => buff + " " + s)), str, empty);
    }
  }
}
