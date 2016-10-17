// Decompiled with JetBrains decompiler
// Type: MSVCCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor.Utils;

internal class MSVCCompiler : NativeCompiler
{
  private readonly string m_CompilerOptions = "/bigobj /Od /Zi /MTd /MP /EHsc /D_SECURE_SCL=0 /D_HAS_ITERATOR_DEBUGGING=0";
  private readonly string[] m_IncludePaths = new string[2]
  {
    "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\vc\\include",
    "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.0A\\Include"
  };
  private readonly string[] m_Libraries = new string[7]
  {
    "user32.lib",
    "advapi32.lib",
    "ole32.lib",
    "oleaut32.lib",
    "ws2_32.lib",
    "Shell32.lib",
    "Psapi.lib"
  };
  private readonly ICompilerSettings m_Settings;
  private readonly string m_DefFile;

  protected override string objectFileExtension
  {
    get
    {
      return "obj";
    }
  }

  public MSVCCompiler(ICompilerSettings settings, string defFile)
  {
    this.m_Settings = settings;
    this.m_DefFile = defFile;
  }

  public override void CompileDynamicLibrary(string outputFile, IEnumerable<string> sources, IEnumerable<string> includePaths, IEnumerable<string> libraries, IEnumerable<string> libraryPaths)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MSVCCompiler.\u003CCompileDynamicLibrary\u003Ec__AnonStorey75 libraryCAnonStorey75 = new MSVCCompiler.\u003CCompileDynamicLibrary\u003Ec__AnonStorey75();
    // ISSUE: reference to a compiler-generated field
    libraryCAnonStorey75.\u003C\u003Ef__this = this;
    string[] array = sources.ToArray<string>();
    string str1 = NativeCompiler.Aggregate(((IEnumerable<string>) array).Select<string, string>(new Func<string, string>(((NativeCompiler) this).ObjectFileFor)), " \"", "\" " + Environment.NewLine);
    // ISSUE: reference to a compiler-generated field
    libraryCAnonStorey75.includePathsString = NativeCompiler.Aggregate(includePaths.Union<string>((IEnumerable<string>) this.m_IncludePaths), "/I \"", "\" ");
    string str2 = NativeCompiler.Aggregate(libraries.Union<string>((IEnumerable<string>) this.m_Libraries), " ", " ");
    string str3 = NativeCompiler.Aggregate(libraryPaths.Union<string>((IEnumerable<string>) this.m_Settings.LibPaths), "/LIBPATH:\"", "\" ");
    this.GenerateEmptyPdbFile(outputFile);
    // ISSUE: reference to a compiler-generated method
    NativeCompiler.ParallelFor<string>(array, new Action<string>(libraryCAnonStorey75.\u003C\u003Em__108));
    string contents = string.Format(" {0} {1} {2} /DEBUG /INCREMENTAL:NO /MACHINE:{4} /DLL /out:\"{3}\" /MAP /DEF:\"{5}\" ", (object) str1, (object) str2, (object) str3, (object) outputFile, (object) this.m_Settings.MachineSpecification, (object) this.m_DefFile);
    string tempFileName = Path.GetTempFileName();
    File.WriteAllText(tempFileName, contents);
    this.Execute(string.Format("@{0}", (object) tempFileName), "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\VC\\bin\\link.exe");
    this.ExecuteCommand(Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/MapFileParser/MapFileParser.exe"), "-format=MSVC", "\"" + Path.GetFullPath(Path.Combine(Path.GetDirectoryName(outputFile), Path.GetFileNameWithoutExtension(outputFile) + ".map")) + "\"", "\"" + Path.GetFullPath(Path.Combine(Path.GetDirectoryName(outputFile), "SymbolMap")) + "\"");
  }

  private void GenerateEmptyPdbFile(string outputFile)
  {
    string tempFileName = Path.GetTempFileName();
    File.WriteAllText(tempFileName, " /* **** */");
    string fullPath = Path.GetFullPath(Path.GetDirectoryName(outputFile));
    string str = Path.Combine(fullPath, Path.GetFileNameWithoutExtension(outputFile) + ".pdb");
    Directory.CreateDirectory(fullPath);
    this.Execute(string.Format("{0}", (object) string.Format(" -c /Tp {0} /Zi /Fd\"{1}\"", (object) tempFileName, (object) str)), this.m_Settings.CompilerPath);
  }

  private void Compile(string file, string includePaths)
  {
    this.Execute(string.Format("{0}", (object) string.Format(" /c {0} \"{1}\" {2} /Fo{3}\\ ", (object) this.m_CompilerOptions, (object) file, (object) includePaths, (object) Path.GetDirectoryName(file))), this.m_Settings.CompilerPath);
  }

  protected override void SetupProcessStartInfo(ProcessStartInfo startInfo)
  {
    startInfo.CreateNoWindow = true;
    if (!startInfo.EnvironmentVariables.ContainsKey("PATH"))
    {
      startInfo.EnvironmentVariables.Add("PATH", "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE");
    }
    else
    {
      string environmentVariable = startInfo.EnvironmentVariables["PATH"];
      string str = "C:\\Program Files (x86)\\Microsoft Visual Studio 10.0\\Common7\\IDE" + (object) Path.PathSeparator + environmentVariable;
      startInfo.EnvironmentVariables["PATH"] = str;
    }
  }
}
