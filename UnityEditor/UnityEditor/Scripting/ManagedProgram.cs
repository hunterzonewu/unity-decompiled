// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.ManagedProgram
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Diagnostics;
using System.IO;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.Scripting
{
  internal class ManagedProgram : Program
  {
    public ManagedProgram(string monodistribution, string profile, string executable, string arguments)
      : this(monodistribution, profile, executable, arguments, true)
    {
    }

    public ManagedProgram(string monodistribution, string profile, string executable, string arguments, bool setMonoEnvironmentVariables)
    {
      string str1 = ManagedProgram.PathCombine(monodistribution, "bin", "mono");
      string str2 = ManagedProgram.PathCombine(monodistribution, "lib", "mono", profile);
      if (Application.platform == RuntimePlatform.WindowsEditor)
        str1 = CommandLineFormatter.PrepareFileName(str1 + ".exe");
      ProcessStartInfo processStartInfo = new ProcessStartInfo() { Arguments = CommandLineFormatter.PrepareFileName(executable) + " " + arguments, CreateNoWindow = true, FileName = str1, RedirectStandardError = true, RedirectStandardOutput = true, WorkingDirectory = Application.dataPath + "/..", UseShellExecute = false };
      if (setMonoEnvironmentVariables)
      {
        processStartInfo.EnvironmentVariables["MONO_PATH"] = str2;
        processStartInfo.EnvironmentVariables["MONO_CFG_DIR"] = ManagedProgram.PathCombine(monodistribution, "etc");
      }
      this._process.StartInfo = processStartInfo;
    }

    private static string PathCombine(params string[] parts)
    {
      string path1 = parts[0];
      for (int index = 1; index < parts.Length; ++index)
        path1 = Path.Combine(path1, parts[index]);
      return path1;
    }
  }
}
