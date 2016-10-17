// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnityLogWriter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace UnityEngine
{
  internal sealed class UnityLogWriter : TextWriter
  {
    public override Encoding Encoding
    {
      get
      {
        return Encoding.UTF8;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void WriteStringToUnityLog(string s);

    public static void Init()
    {
      Console.SetOut((TextWriter) new UnityLogWriter());
    }

    public override void Write(char value)
    {
      UnityLogWriter.WriteStringToUnityLog(value.ToString());
    }

    public override void Write(string s)
    {
      UnityLogWriter.WriteStringToUnityLog(s);
    }
  }
}
