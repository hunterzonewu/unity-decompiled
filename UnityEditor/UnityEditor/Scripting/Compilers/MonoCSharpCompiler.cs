// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.MonoCSharpCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor.Utils;

namespace UnityEditor.Scripting.Compilers
{
  internal class MonoCSharpCompiler : MonoScriptCompilerBase
  {
    public MonoCSharpCompiler(MonoIsland island, bool runUpdater)
      : base(island, runUpdater)
    {
    }

    protected override Program StartCompiler()
    {
      List<string> arguments = new List<string>() { "-debug", "-target:library", "-nowarn:0169", "-out:" + ScriptCompilerBase.PrepareFileName(this._island._output) };
      foreach (string reference in this._island._references)
        arguments.Add("-r:" + ScriptCompilerBase.PrepareFileName(reference));
      foreach (string str in ((IEnumerable<string>) this._island._defines).Distinct<string>())
        arguments.Add("-define:" + str);
      foreach (string file in this._island._files)
        arguments.Add(ScriptCompilerBase.PrepareFileName(file));
      foreach (string additionalReference in this.GetAdditionalReferences())
      {
        string str = Path.Combine(this.GetProfileDirectory(), additionalReference);
        if (File.Exists(str))
          arguments.Add("-r:" + ScriptCompilerBase.PrepareFileName(str));
      }
      return (Program) this.StartCompiler(this._island._target, this.GetCompilerPath(arguments), arguments);
    }

    private string[] GetAdditionalReferences()
    {
      return new string[2]{ "System.Runtime.Serialization.dll", "System.Xml.Linq.dll" };
    }

    private string GetCompilerPath(List<string> arguments)
    {
      string profileDirectory = this.GetProfileDirectory();
      string[] strArray = new string[3]{ "smcs", "gmcs", "mcs" };
      foreach (string str in strArray)
      {
        string path = Path.Combine(profileDirectory, str + ".exe");
        if (File.Exists(path))
          return path;
      }
      throw new ApplicationException("Unable to find csharp compiler in " + profileDirectory);
    }

    protected override CompilerOutputParserBase CreateOutputParser()
    {
      return (CompilerOutputParserBase) new MonoCSharpCompilerOutputParser();
    }

    public static string[] Compile(string[] sources, string[] references, string[] defines, string outputFile)
    {
      using (MonoCSharpCompiler monoCsharpCompiler = new MonoCSharpCompiler(new MonoIsland(BuildTarget.StandaloneWindows, "unity", sources, references, defines, outputFile), false))
      {
        monoCsharpCompiler.BeginCompiling();
        while (!monoCsharpCompiler.Poll())
          Thread.Sleep(50);
        return ((IEnumerable<CompilerMessage>) monoCsharpCompiler.GetCompilerMessages()).Select<CompilerMessage, string>((Func<CompilerMessage, string>) (cm => cm.message)).ToArray<string>();
      }
    }
  }
}
