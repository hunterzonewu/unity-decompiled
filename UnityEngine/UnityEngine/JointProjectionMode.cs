// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointProjectionMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Determines how to snap physics joints back to its constrained position when it drifts off too much.</para>
  /// </summary>
  public enum JointProjectionMode
  {
    None,
    PositionAndRotation,
    [Obsolete("JointProjectionMode.PositionOnly is no longer supported", true)] PositionOnly,
  }
}
