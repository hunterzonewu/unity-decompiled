// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointDriveMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The ConfigurableJoint attempts to attain position / velocity targets based on this flag.</para>
  /// </summary>
  [Flags]
  [Obsolete("JointDriveMode is no longer supported")]
  public enum JointDriveMode
  {
    [Obsolete("JointDriveMode.None is no longer supported")] None = 0,
    [Obsolete("JointDriveMode.Position is no longer supported")] Position = 1,
    [Obsolete("JointDriveMode.Velocity is no longer supported")] Velocity = 2,
    [Obsolete("JointDriveMode.PositionAndvelocity is no longer supported")] PositionAndVelocity = Velocity | Position,
  }
}
