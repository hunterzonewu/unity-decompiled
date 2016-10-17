// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnassignedReferenceException
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
  [Serializable]
  public class UnassignedReferenceException : SystemException
  {
    private const int Result = -2147467261;
    private string unityStackTrace;

    public UnassignedReferenceException()
      : base("A Unity Runtime error occurred!")
    {
      this.HResult = -2147467261;
    }

    public UnassignedReferenceException(string message)
      : base(message)
    {
      this.HResult = -2147467261;
    }

    public UnassignedReferenceException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.HResult = -2147467261;
    }

    protected UnassignedReferenceException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
