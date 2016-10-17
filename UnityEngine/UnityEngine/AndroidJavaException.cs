// Decompiled with JetBrains decompiler
// Type: UnityEngine.AndroidJavaException
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  public sealed class AndroidJavaException : Exception
  {
    private string mJavaStackTrace;

    public override string StackTrace
    {
      get
      {
        return this.mJavaStackTrace + base.StackTrace;
      }
    }

    internal AndroidJavaException(string message, string javaStackTrace)
      : base(message)
    {
      this.mJavaStackTrace = javaStackTrace;
    }
  }
}
