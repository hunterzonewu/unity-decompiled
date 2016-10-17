// Decompiled with JetBrains decompiler
// Type: ClangCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.Utils;

internal class ClangCompiler : NativeCompiler
{
  private readonly ICompilerSettings m_Settings;

  protected override string objectFileExtension
  {
    get
    {
      return "o";
    }
  }

  public ClangCompiler(ICompilerSettings settings)
  {
    this.m_Settings = settings;
  }

  public override void CompileDynamicLibrary(string outFile, IEnumerable<string> sources, IEnumerable<string> includePaths, IEnumerable<string> libraries, IEnumerable<string> libraryPaths)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ClangCompiler.\u003CCompileDynamicLibrary\u003Ec__AnonStorey71 libraryCAnonStorey71 = new ClangCompiler.\u003CCompileDynamicLibrary\u003Ec__AnonStorey71();
    // ISSUE: reference to a compiler-generated field
    libraryCAnonStorey71.\u003C\u003Ef__this = this;
    string[] array = sources.ToArray<string>();
    // ISSUE: reference to a compiler-generated field
    libraryCAnonStorey71.includeDirs = includePaths.Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((current, sourceDir) => current + "-I" + sourceDir + " "));
    string str1 = NativeCompiler.Aggregate(libraries, "-force_load ", " ");
    string str2 = NativeCompiler.Aggregate(libraryPaths.Union<string>((IEnumerable<string>) this.m_Settings.LibPaths), "-L", " ");
    // ISSUE: reference to a compiler-generated method
    NativeCompiler.ParallelFor<string>(array, new Action<string>(libraryCAnonStorey71.\u003C\u003Em__F8));
    string str3 = "\"" + Path.GetFullPath(Path.Combine(Path.GetDirectoryName(outFile), Path.GetFileNameWithoutExtension(outFile) + ".map")) + "\"";
    this.ExecuteCommand("ld", "-dylib", "-arch " + this.m_Settings.MachineSpecification, "-macosx_version_min 10.6", "-lSystem", "-lstdc++", "-map", str3, "-o " + outFile, ((IEnumerable<string>) array).Select<string, string>(new Func<string, string>(((NativeCompiler) this).ObjectFileFor)).Aggregate<string>((Func<string, string, string>) ((buff, s) => buff + " " + s)), str2, str1);
    string command = Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/MapFileParser/MapFileParser");
    string str4 = "\"" + Path.GetFullPath(Path.Combine(Path.GetDirectoryName(outFile), "SymbolMap")) + "\"";
    this.ExecuteCommand(command, "-format=Clang", str3, str4);
  }

  private void Compile(string file, string includePaths)
  {
    this.Execute(string.Format(" -c -arch {0} -stdlib=libstdc++ -O0 -Wno-unused-value -Wno-invalid-offsetof -fvisibility=hidden -fno-rtti -I/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/include {1} -isysroot /Applications/Xcode.app/Contents/Developer/Platforms/MacOSX.platform/Developer/SDKs/MacOSX10.8.sdk -mmacosx-version-min=10.6 -single_module -compatibility_version 1 -current_version 1 {2} -o {3}", (object) this.m_Settings.MachineSpecification, (object) includePaths, (object) file, (object) this.ObjectFileFor(file)), this.m_Settings.CompilerPath);
  }

  protected override void SetupProcessStartInfo(ProcessStartInfo startInfo)
  {
  }
}
