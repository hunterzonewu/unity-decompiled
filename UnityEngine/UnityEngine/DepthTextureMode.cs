// Decompiled with JetBrains decompiler
// Type: UnityEngine.DepthTextureMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Depth texture generation mode for Camera.</para>
  /// </summary>
  [Flags]
  public enum DepthTextureMode
  {
    None = 0,
    Depth = 1,
    DepthNormals = 2,
  }
}
