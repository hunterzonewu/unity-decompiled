// Decompiled with JetBrains decompiler
// Type: UnityEngine.HumanPose
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Retargetable humanoid pose.</para>
  /// </summary>
  public struct HumanPose
  {
    /// <summary>
    ///   <para>The human body position for that pose.</para>
    /// </summary>
    public Vector3 bodyPosition;
    /// <summary>
    ///   <para>The human body orientation for that pose.</para>
    /// </summary>
    public Quaternion bodyRotation;
    /// <summary>
    ///   <para>The array of muscle values for that pose.</para>
    /// </summary>
    public float[] muscles;

    internal void Init()
    {
      if (this.muscles != null && this.muscles.Length != HumanTrait.MuscleCount)
        throw new ArgumentException("Bad array size for HumanPose.muscles. Size must equal HumanTrait.MuscleCount");
      if (this.muscles != null)
        return;
      this.muscles = new float[HumanTrait.MuscleCount];
      if ((double) this.bodyRotation.x != 0.0 || (double) this.bodyRotation.y != 0.0 || ((double) this.bodyRotation.z != 0.0 || (double) this.bodyRotation.w != 0.0))
        return;
      this.bodyRotation.w = 1f;
    }
  }
}
