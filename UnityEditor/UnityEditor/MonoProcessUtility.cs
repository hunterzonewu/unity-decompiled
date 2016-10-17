// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoProcessUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
  internal class MonoProcessUtility
  {
    public static string ProcessToString(Process process)
    {
      return process.StartInfo.FileName + " " + process.StartInfo.Arguments + " current dir : " + process.StartInfo.WorkingDirectory + "\n";
    }

    public static void RunMonoProcess(Process process, string name, string resultingFile)
    {
      MonoProcessRunner monoProcessRunner = new MonoProcessRunner();
      bool flag = monoProcessRunner.Run(process);
      if (process.ExitCode != 0 || !File.Exists(resultingFile))
      {
        string message = "Failed " + name + ": " + MonoProcessUtility.ProcessToString(process) + " result file exists: " + (object) File.Exists(resultingFile) + ". Timed out: " + (object) !flag + "\n\n" + "stdout:\n" + (object) monoProcessRunner.Output + "\n" + "stderr:\n" + (object) monoProcessRunner.Error + "\n";
        Console.WriteLine(message);
        throw new UnityException(message);
      }
    }

    public static string GetMonoExec(BuildTarget buildTarget)
    {
      string monoBinDirectory = BuildPipeline.GetMonoBinDirectory(buildTarget);
      if (Application.platform == RuntimePlatform.OSXEditor)
        return Path.Combine(monoBinDirectory, "mono");
      return Path.Combine(monoBinDirectory, "mono.exe");
    }

    public static string GetMonoPath(BuildTarget buildTarget)
    {
      return BuildPipeline.GetMonoLibDirectory(buildTarget) + (object) Path.PathSeparator + ".";
    }

    public static Process PrepareMonoProcess(BuildTarget target, string workDir)
    {
      Process process = new Process();
      process.StartInfo.FileName = MonoProcessUtility.GetMonoExec(target);
      process.StartInfo.EnvironmentVariables["_WAPI_PROCESS_HANDLE_OFFSET"] = "5";
      process.StartInfo.EnvironmentVariables["MONO_PATH"] = MonoProcessUtility.GetMonoPath(target);
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.WorkingDirectory = workDir;
      return process;
    }
  }
}
