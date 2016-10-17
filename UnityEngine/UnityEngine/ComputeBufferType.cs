// Decompiled with JetBrains decompiler
// Type: UnityEngine.ComputeBufferType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>ComputeBuffer type.</para>
  /// </summary>
  [Flags]
  public enum ComputeBufferType
  {
    Default = 0,
    Raw = 1,
    Append = 2,
    Counter = 4,
    DrawIndirect = 256,
    GPUMemory = 512,
  }
}
