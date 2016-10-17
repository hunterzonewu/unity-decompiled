// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.NativeProgram
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Diagnostics;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class NativeProgram : Program
  {
    public NativeProgram(string executable, string arguments)
    {
      this._process.StartInfo = new ProcessStartInfo()
      {
        Arguments = arguments,
        CreateNoWindow = true,
        FileName = executable,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        WorkingDirectory = Application.dataPath + "/..",
        UseShellExecute = false
      };
    }
  }
}
