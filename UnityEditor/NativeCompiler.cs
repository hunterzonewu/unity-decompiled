// Decompiled with JetBrains decompiler
// Type: NativeCompiler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor.Utils;

internal abstract class NativeCompiler : INativeCompiler
{
  protected virtual string objectFileExtension
  {
    get
    {
      return "o";
    }
  }

  public abstract void CompileDynamicLibrary(string outFile, IEnumerable<string> sources, IEnumerable<string> includePaths, IEnumerable<string> libraries, IEnumerable<string> libraryPaths);

  protected virtual void SetupProcessStartInfo(ProcessStartInfo startInfo)
  {
  }

  protected void Execute(string arguments, string compilerPath)
  {
    ProcessStartInfo startInfo = new ProcessStartInfo(compilerPath, arguments);
    this.SetupProcessStartInfo(startInfo);
    this.RunProgram(startInfo);
  }

  protected void ExecuteCommand(string command, params string[] arguments)
  {
    ProcessStartInfo startInfo = new ProcessStartInfo(command, ((IEnumerable<string>) arguments).Aggregate<string>((Func<string, string, string>) ((buff, s) => buff + " " + s)));
    this.SetupProcessStartInfo(startInfo);
    this.RunProgram(startInfo);
  }

  private void RunProgram(ProcessStartInfo startInfo)
  {
    using (Program program = new Program(startInfo))
    {
      program.Start();
      do
        ;
      while (!program.WaitForExit(100));
      string str = string.Empty;
      string[] standardOutput = program.GetStandardOutput();
      if (standardOutput.Length > 0)
        str = ((IEnumerable<string>) standardOutput).Aggregate<string>((Func<string, string, string>) ((buf, s) => buf + Environment.NewLine + s));
      string[] errorOutput = program.GetErrorOutput();
      if (errorOutput.Length > 0)
        str += ((IEnumerable<string>) errorOutput).Aggregate<string>((Func<string, string, string>) ((buf, s) => buf + Environment.NewLine + s));
      if (program.ExitCode != 0)
      {
        UnityEngine.Debug.LogError((object) ("Failed running " + startInfo.FileName + " " + startInfo.Arguments + "\n\n" + str));
        throw new Exception("IL2CPP compile failed.");
      }
    }
  }

  protected static string Aggregate(IEnumerable<string> items, string prefix, string suffix)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    return items.Aggregate<string, string>(string.Empty, new Func<string, string, string>(new NativeCompiler.\u003CAggregate\u003Ec__AnonStorey6F()
    {
      prefix = prefix,
      suffix = suffix
    }.\u003C\u003Em__F5));
  }

  internal static void ParallelFor<T>(T[] sources, Action<T> action)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    NativeCompiler.\u003CParallelFor\u003Ec__AnonStorey70<T> forCAnonStorey70 = new NativeCompiler.\u003CParallelFor\u003Ec__AnonStorey70<T>();
    // ISSUE: reference to a compiler-generated field
    forCAnonStorey70.sources = sources;
    // ISSUE: reference to a compiler-generated field
    forCAnonStorey70.action = action;
    Thread[] threadArray = new Thread[Environment.ProcessorCount];
    NativeCompiler.Counter counter = new NativeCompiler.Counter();
    for (int index = 0; index < threadArray.Length; ++index)
    {
      // ISSUE: reference to a compiler-generated method
      threadArray[index] = new Thread(new ParameterizedThreadStart(forCAnonStorey70.\u003C\u003Em__F6));
    }
    foreach (Thread thread in threadArray)
      thread.Start((object) counter);
    foreach (Thread thread in threadArray)
      thread.Join();
  }

  protected internal static IEnumerable<string> AllSourceFilesIn(string directory)
  {
    return ((IEnumerable<string>) Directory.GetFiles(directory, "*.cpp", SearchOption.AllDirectories)).Concat<string>((IEnumerable<string>) Directory.GetFiles(directory, "*.c", SearchOption.AllDirectories));
  }

  protected internal static bool IsSourceFile(string source)
  {
    string extension = Path.GetExtension(source);
    if (!(extension == "cpp"))
      return extension == "c";
    return true;
  }

  protected string ObjectFileFor(string source)
  {
    return Path.ChangeExtension(source, this.objectFileExtension);
  }

  private class Counter
  {
    public int index;
  }
}
