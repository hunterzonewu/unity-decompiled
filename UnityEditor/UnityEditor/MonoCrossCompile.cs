// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoCrossCompile
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace UnityEditor
{
  internal class MonoCrossCompile
  {
    public static string ArtifactsPath;

    public static void CrossCompileAOTDirectory(BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string additionalOptions)
    {
      MonoCrossCompile.CrossCompileAOTDirectory(buildTarget, crossCompileOptions, sourceAssembliesFolder, targetCrossCompiledASMFolder, string.Empty, additionalOptions);
    }

    public static void CrossCompileAOTDirectory(BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string pathExtension, string additionalOptions)
    {
      string buildToolsDirectory = BuildPipeline.GetBuildToolsDirectory(buildTarget);
      string crossCompilerAbsolutePath = Application.platform != RuntimePlatform.OSXEditor ? Path.Combine(Path.Combine(buildToolsDirectory, pathExtension), "mono-xcompiler.exe") : Path.Combine(Path.Combine(buildToolsDirectory, pathExtension), "mono-xcompiler");
      sourceAssembliesFolder = Path.Combine(Directory.GetCurrentDirectory(), sourceAssembliesFolder);
      targetCrossCompiledASMFolder = Path.Combine(Directory.GetCurrentDirectory(), targetCrossCompiledASMFolder);
      foreach (string file in Directory.GetFiles(sourceAssembliesFolder))
      {
        if (!(Path.GetExtension(file) != ".dll"))
        {
          string fileName = Path.GetFileName(file);
          string output = Path.Combine(targetCrossCompiledASMFolder, fileName + ".s");
          if (EditorUtility.DisplayCancelableProgressBar("Building Player", "AOT cross compile " + fileName, 0.95f))
            throw new OperationCanceledException();
          MonoCrossCompile.CrossCompileAOT(buildTarget, crossCompilerAbsolutePath, sourceAssembliesFolder, crossCompileOptions, fileName, output, additionalOptions);
        }
      }
    }

    public static bool CrossCompileAOTDirectoryParallel(BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string additionalOptions)
    {
      return MonoCrossCompile.CrossCompileAOTDirectoryParallel(buildTarget, crossCompileOptions, sourceAssembliesFolder, targetCrossCompiledASMFolder, string.Empty, additionalOptions);
    }

    public static bool CrossCompileAOTDirectoryParallel(BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string pathExtension, string additionalOptions)
    {
      string buildToolsDirectory = BuildPipeline.GetBuildToolsDirectory(buildTarget);
      return MonoCrossCompile.CrossCompileAOTDirectoryParallel(Application.platform != RuntimePlatform.OSXEditor ? Path.Combine(Path.Combine(buildToolsDirectory, pathExtension), "mono-xcompiler.exe") : Path.Combine(Path.Combine(buildToolsDirectory, pathExtension), "mono-xcompiler"), buildTarget, crossCompileOptions, sourceAssembliesFolder, targetCrossCompiledASMFolder, additionalOptions);
    }

    private static bool WaitForBuildOfFile(List<ManualResetEvent> events, ref long timeout)
    {
      long num1 = DateTime.Now.Ticks / 10000L;
      int index = WaitHandle.WaitAny((WaitHandle[]) events.ToArray(), (int) timeout);
      long num2 = DateTime.Now.Ticks / 10000L;
      if (index == 258)
        return false;
      events.RemoveAt(index);
      timeout = timeout - (num2 - num1);
      if (timeout < 0L)
        timeout = 0L;
      return true;
    }

    public static void DisplayAOTProgressBar(int totalFiles, int filesFinished)
    {
      EditorUtility.DisplayProgressBar("Building Player", string.Format("AOT cross compile ({0}/{1})", (object) (filesFinished + 1).ToString(), (object) totalFiles.ToString()), 0.95f);
    }

    public static bool CrossCompileAOTDirectoryParallel(string crossCompilerPath, BuildTarget buildTarget, CrossCompileOptions crossCompileOptions, string sourceAssembliesFolder, string targetCrossCompiledASMFolder, string additionalOptions)
    {
      sourceAssembliesFolder = Path.Combine(Directory.GetCurrentDirectory(), sourceAssembliesFolder);
      targetCrossCompiledASMFolder = Path.Combine(Directory.GetCurrentDirectory(), targetCrossCompiledASMFolder);
      int workerThreads = 1;
      int completionPortThreads = 1;
      ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
      List<MonoCrossCompile.JobCompileAOT> jobCompileAotList = new List<MonoCrossCompile.JobCompileAOT>();
      List<ManualResetEvent> events = new List<ManualResetEvent>();
      bool flag = true;
      List<string> stringList = new List<string>(((IEnumerable<string>) Directory.GetFiles(sourceAssembliesFolder)).Where<string>((Func<string, bool>) (path => Path.GetExtension(path) == ".dll")));
      int count = stringList.Count;
      int filesFinished = 0;
      MonoCrossCompile.DisplayAOTProgressBar(count, filesFinished);
      long timeout = (long) Math.Min(1800000, (count + 3) * 1000 * 30);
      using (List<string>.Enumerator enumerator = stringList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string fileName = Path.GetFileName(enumerator.Current);
          string output = Path.Combine(targetCrossCompiledASMFolder, fileName + ".s");
          MonoCrossCompile.JobCompileAOT jobCompileAot = new MonoCrossCompile.JobCompileAOT(buildTarget, crossCompilerPath, sourceAssembliesFolder, crossCompileOptions, fileName, output, additionalOptions);
          jobCompileAotList.Add(jobCompileAot);
          events.Add(jobCompileAot.m_doneEvent);
          ThreadPool.QueueUserWorkItem(new WaitCallback(jobCompileAot.ThreadPoolCallback));
          if (events.Count >= Environment.ProcessorCount)
          {
            flag = MonoCrossCompile.WaitForBuildOfFile(events, ref timeout);
            MonoCrossCompile.DisplayAOTProgressBar(count, filesFinished);
            ++filesFinished;
            if (!flag)
              break;
          }
        }
      }
      while (events.Count > 0)
      {
        flag = MonoCrossCompile.WaitForBuildOfFile(events, ref timeout);
        MonoCrossCompile.DisplayAOTProgressBar(count, filesFinished);
        ++filesFinished;
        if (!flag)
          break;
      }
      using (List<MonoCrossCompile.JobCompileAOT>.Enumerator enumerator = jobCompileAotList.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          MonoCrossCompile.JobCompileAOT current = enumerator.Current;
          if (current.m_Exception != null)
          {
            UnityEngine.Debug.LogErrorFormat("Cross compilation job {0} failed.\n{1}", new object[2]
            {
              (object) current.m_input,
              (object) current.m_Exception
            });
            flag = false;
          }
        }
      }
      return flag;
    }

    private static bool IsDebugableAssembly(string fname)
    {
      fname = Path.GetFileName(fname);
      return fname.StartsWith("Assembly", StringComparison.OrdinalIgnoreCase);
    }

    private static void CrossCompileAOT(BuildTarget target, string crossCompilerAbsolutePath, string assembliesAbsoluteDirectory, CrossCompileOptions crossCompileOptions, string input, string output, string additionalOptions)
    {
      string empty = string.Empty;
      if (!MonoCrossCompile.IsDebugableAssembly(input))
      {
        crossCompileOptions &= ~CrossCompileOptions.Debugging;
        crossCompileOptions &= ~CrossCompileOptions.LoadSymbols;
      }
      bool flag1 = (crossCompileOptions & CrossCompileOptions.Debugging) != CrossCompileOptions.Dynamic;
      bool flag2 = (crossCompileOptions & CrossCompileOptions.LoadSymbols) != CrossCompileOptions.Dynamic;
      bool flag3 = flag1 || flag2;
      if (flag3)
        empty += "--debug ";
      if (flag1)
        empty += "--optimize=-linears ";
      string str1 = empty + "--aot=full,asmonly,";
      if (flag3)
        str1 += "write-symbols,";
      if ((crossCompileOptions & CrossCompileOptions.Debugging) != CrossCompileOptions.Dynamic)
        str1 += "soft-debug,";
      else if (!flag3)
        str1 += "nodebug,";
      if (target != BuildTarget.iOS)
        str1 += "print-skipped,";
      if (additionalOptions != null & additionalOptions.Trim().Length > 0)
        str1 = str1 + additionalOptions.Trim() + ",";
      string fileName = Path.GetFileName(output);
      string str2 = Path.Combine(assembliesAbsoluteDirectory, fileName);
      if ((crossCompileOptions & CrossCompileOptions.FastICall) != CrossCompileOptions.Dynamic)
        str1 += "ficall,";
      if ((crossCompileOptions & CrossCompileOptions.Static) != CrossCompileOptions.Dynamic)
        str1 += "static,";
      string str3 = str1 + "outfile=\"" + fileName + "\" \"" + input + "\" ";
      Process process = new Process();
      process.StartInfo.FileName = crossCompilerAbsolutePath;
      process.StartInfo.Arguments = str3;
      process.StartInfo.EnvironmentVariables["MONO_PATH"] = assembliesAbsoluteDirectory;
      process.StartInfo.EnvironmentVariables["GAC_PATH"] = assembliesAbsoluteDirectory;
      process.StartInfo.EnvironmentVariables["GC_DONT_GC"] = "yes please";
      if ((crossCompileOptions & CrossCompileOptions.ExplicitNullChecks) != CrossCompileOptions.Dynamic)
        process.StartInfo.EnvironmentVariables["MONO_DEBUG"] = "explicit-null-checks";
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.RedirectStandardOutput = true;
      if (MonoCrossCompile.ArtifactsPath != null)
      {
        if (!Directory.Exists(MonoCrossCompile.ArtifactsPath))
          Directory.CreateDirectory(MonoCrossCompile.ArtifactsPath);
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", process.StartInfo.FileName + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", process.StartInfo.Arguments + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", assembliesAbsoluteDirectory + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", str2 + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "output.txt", input + "\n");
        File.AppendAllText(MonoCrossCompile.ArtifactsPath + "houtput.txt", fileName + "\n\n");
        File.Copy(assembliesAbsoluteDirectory + "\\" + input, MonoCrossCompile.ArtifactsPath + "\\" + input, true);
      }
      process.StartInfo.WorkingDirectory = assembliesAbsoluteDirectory;
      MonoProcessUtility.RunMonoProcess(process, "AOT cross compiler", str2);
      File.Move(str2, output);
      if ((crossCompileOptions & CrossCompileOptions.Static) != CrossCompileOptions.Dynamic)
        return;
      string str4 = Path.Combine(assembliesAbsoluteDirectory, fileName + ".def");
      if (!File.Exists(str4))
        return;
      File.Move(str4, output + ".def");
    }

    private class JobCompileAOT
    {
      public ManualResetEvent m_doneEvent = new ManualResetEvent(false);
      private BuildTarget m_target;
      private string m_crossCompilerAbsolutePath;
      private string m_assembliesAbsoluteDirectory;
      private CrossCompileOptions m_crossCompileOptions;
      public string m_input;
      public string m_output;
      public string m_additionalOptions;
      public Exception m_Exception;

      public JobCompileAOT(BuildTarget target, string crossCompilerAbsolutePath, string assembliesAbsoluteDirectory, CrossCompileOptions crossCompileOptions, string input, string output, string additionalOptions)
      {
        this.m_target = target;
        this.m_crossCompilerAbsolutePath = crossCompilerAbsolutePath;
        this.m_assembliesAbsoluteDirectory = assembliesAbsoluteDirectory;
        this.m_crossCompileOptions = crossCompileOptions;
        this.m_input = input;
        this.m_output = output;
        this.m_additionalOptions = additionalOptions;
      }

      public void ThreadPoolCallback(object threadContext)
      {
        try
        {
          MonoCrossCompile.CrossCompileAOT(this.m_target, this.m_crossCompilerAbsolutePath, this.m_assembliesAbsoluteDirectory, this.m_crossCompileOptions, this.m_input, this.m_output, this.m_additionalOptions);
        }
        catch (Exception ex)
        {
          this.m_Exception = ex;
        }
        this.m_doneEvent.Set();
      }
    }
  }
}
