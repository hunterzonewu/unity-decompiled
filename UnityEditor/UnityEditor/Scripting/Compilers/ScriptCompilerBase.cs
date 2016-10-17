// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.ScriptCompilerBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.Scripting.Compilers
{
  internal abstract class ScriptCompilerBase : IDisposable
  {
    private Program process;
    private string _responseFile;
    protected MonoIsland _island;

    protected ScriptCompilerBase(MonoIsland island)
    {
      this._island = island;
    }

    protected abstract Program StartCompiler();

    protected abstract CompilerOutputParserBase CreateOutputParser();

    protected string[] GetErrorOutput()
    {
      return this.process.GetErrorOutput();
    }

    protected string[] GetStandardOutput()
    {
      return this.process.GetStandardOutput();
    }

    protected bool CompilingForWSA()
    {
      return this._island._target == BuildTarget.WSAPlayer;
    }

    public void BeginCompiling()
    {
      if (this.process != null)
        throw new InvalidOperationException("Compilation has already begun!");
      this.process = this.StartCompiler();
    }

    public virtual void Dispose()
    {
      if (this.process != null)
      {
        this.process.Dispose();
        this.process = (Program) null;
      }
      if (this._responseFile == null)
        return;
      File.Delete(this._responseFile);
      this._responseFile = (string) null;
    }

    public virtual bool Poll()
    {
      if (this.process == null)
        return true;
      return this.process.HasExited;
    }

    protected void AddCustomResponseFileIfPresent(List<string> arguments, string responseFileName)
    {
      string path = Path.Combine("Assets", responseFileName);
      if (!File.Exists(path))
        return;
      arguments.Add("@" + path);
    }

    protected string GenerateResponseFile(List<string> arguments)
    {
      this._responseFile = CommandLineFormatter.GenerateResponseFile((IEnumerable<string>) arguments);
      return this._responseFile;
    }

    protected static string PrepareFileName(string fileName)
    {
      return CommandLineFormatter.PrepareFileName(fileName);
    }

    public virtual CompilerMessage[] GetCompilerMessages()
    {
      if (!this.Poll())
        Debug.LogWarning((object) "Compile process is not finished yet. This should not happen.");
      this.DumpStreamOutputToLog();
      return this.CreateOutputParser().Parse(this.GetStreamContainingCompilerMessages(), this.CompilationHadFailure()).ToArray<CompilerMessage>();
    }

    protected bool CompilationHadFailure()
    {
      return this.process.ExitCode != 0;
    }

    protected virtual string[] GetStreamContainingCompilerMessages()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this.GetErrorOutput());
      stringList.Add(string.Empty);
      stringList.AddRange((IEnumerable<string>) this.GetStandardOutput());
      return stringList.ToArray();
    }

    private void DumpStreamOutputToLog()
    {
      bool flag = this.CompilationHadFailure();
      string[] errorOutput = this.GetErrorOutput();
      if (!flag && errorOutput.Length == 0)
        return;
      Console.WriteLine(string.Empty);
      Console.WriteLine("-----Compiler Commandline Arguments:");
      this.process.LogProcessStartInfo();
      string[] standardOutput = this.GetStandardOutput();
      Console.WriteLine("-----CompilerOutput:-stdout--exitcode: " + (object) this.process.ExitCode + "--compilationhadfailure: " + (object) flag + "--outfile: " + this._island._output);
      foreach (string str in standardOutput)
        Console.WriteLine(str);
      Console.WriteLine("-----CompilerOutput:-stderr----------");
      foreach (string str in errorOutput)
        Console.WriteLine(str);
      Console.WriteLine("-----EndCompilerOutput---------------");
    }
  }
}
