// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utils.ProcessOutputStreamReader
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace UnityEditor.Utils
{
  internal class ProcessOutputStreamReader
  {
    private readonly Func<bool> hostProcessExited;
    private readonly StreamReader stream;
    internal List<string> lines;
    private Thread thread;

    internal ProcessOutputStreamReader(Process p, StreamReader stream)
      : this(new Func<bool>(new ProcessOutputStreamReader.\u003CProcessOutputStreamReader\u003Ec__AnonStoreyBF() { p = p }.\u003C\u003Em__22E), stream)
    {
      // ISSUE: reference to a compiler-generated method (out of statement scope)
      // ISSUE: object of a compiler-generated type is created (out of statement scope)
    }

    internal ProcessOutputStreamReader(Func<bool> hostProcessExited, StreamReader stream)
    {
      this.hostProcessExited = hostProcessExited;
      this.stream = stream;
      this.lines = new List<string>();
      this.thread = new Thread(new ThreadStart(this.ThreadFunc));
      this.thread.Start();
    }

    private void ThreadFunc()
    {
      if (this.hostProcessExited())
        return;
      while (this.stream.BaseStream != null)
      {
        string str = this.stream.ReadLine();
        if (str == null)
          break;
        lock (this.lines)
          this.lines.Add(str);
      }
    }

    internal string[] GetOutput()
    {
      if (this.hostProcessExited())
        this.thread.Join();
      lock (this.lines)
        return this.lines.ToArray();
    }
  }
}
