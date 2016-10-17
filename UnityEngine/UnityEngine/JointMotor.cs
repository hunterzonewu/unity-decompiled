// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointMotor
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>The JointMotor is used to motorize a joint.</para>
  /// </summary>
  public struct JointMotor
  {
    private float m_TargetVelocity;
    private float m_Force;
    private int m_FreeSpin;

    /// <summary>
    ///   <para>The motor will apply a force up to force to achieve targetVelocity.</para>
    /// </summary>
    public float targetVelocity
    {
      get
      {
        return this.m_TargetVelocity;
      }
      set
      {
        this.m_TargetVelocity = value;
      }
    }

    /// <summary>
    ///   <para>The motor will apply a force.</para>
    /// </summary>
    public float force
    {
      get
      {
        return this.m_Force;
      }
      set
      {
        this.m_Force = value;
      }
    }

    /// <summary>
    ///   <para>If freeSpin is enabled the motor will only accelerate but never slow down.</para>
    /// </summary>
    public bool freeSpin
    {
      get
      {
        return this.m_FreeSpin == 1;
      }
      set
      {
        this.m_FreeSpin = !value ? 0 : 1;
      }
    }
  }
}
