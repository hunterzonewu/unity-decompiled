// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Runner
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Diagnostics;
using UnityEditor.Scripting;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Runner
  {
    internal static void RunManagedProgram(string exe, string args)
    {
      Runner.RunManagedProgram(exe, args, Application.dataPath + "/..", (CompilerOutputParserBase) null);
    }

    internal static void RunManagedProgram(string exe, string args, string workingDirectory, CompilerOutputParserBase parser)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      Program program;
      if (Application.platform == RuntimePlatform.WindowsEditor)
        program = new Program(new ProcessStartInfo()
        {
          Arguments = args,
          CreateNoWindow = true,
          FileName = exe
        });
      else
        program = (Program) new ManagedProgram(MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"), "4.0", exe, args);
      using (program)
      {
        program.GetProcessStartInfo().WorkingDirectory = workingDirectory;
        program.Start();
        program.WaitForExit();
        stopwatch.Stop();
        Console.WriteLine("{0} exited after {1} ms.", (object) exe, (object) stopwatch.ElapsedMilliseconds);
        if (program.ExitCode != 0)
        {
          if (parser != null)
          {
            string[] errorOutput = program.GetErrorOutput();
            string[] standardOutput = program.GetStandardOutput();
            foreach (CompilerMessage compilerMessage in parser.Parse(errorOutput, standardOutput, true))
              UnityEngine.Debug.LogPlayerBuildError(compilerMessage.message, compilerMessage.file, compilerMessage.line, compilerMessage.column);
          }
          UnityEngine.Debug.LogError((object) ("Failed running " + exe + " " + args + "\n\n" + program.GetAllOutput()));
          throw new Exception(string.Format("{0} did not run properly!", (object) exe));
        }
      }
    }

    public static void RunNativeProgram(string exe, string args)
    {
      using (NativeProgram nativeProgram = new NativeProgram(exe, args))
      {
        nativeProgram.Start();
        nativeProgram.WaitForExit();
        if (nativeProgram.ExitCode != 0)
        {
          UnityEngine.Debug.LogError((object) ("Failed running " + exe + " " + args + "\n\n" + nativeProgram.GetAllOutput()));
          throw new Exception(string.Format("{0} did not run properly!", (object) exe));
        }
      }
    }
  }
}
