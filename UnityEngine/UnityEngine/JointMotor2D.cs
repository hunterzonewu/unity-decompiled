// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointMotor2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Parameters for the optional motor force applied to a Joint2D.</para>
  /// </summary>
  public struct JointMotor2D
  {
    private float m_MotorSpeed;
    private float m_MaximumMotorTorque;

    /// <summary>
    ///   <para>The desired speed for the Rigidbody2D to reach as it moves with the joint.</para>
    /// </summary>
    public float motorSpeed
    {
      get
      {
        return this.m_MotorSpeed;
      }
      set
      {
        this.m_MotorSpeed = value;
      }
    }

    /// <summary>
    ///   <para>The maximum force that can be applied to the Rigidbody2D at the joint to attain the target speed.</para>
    /// </summary>
    public float maxMotorTorque
    {
      get
      {
        return this.m_MaximumMotorTorque;
      }
      set
      {
        this.m_MaximumMotorTorque = value;
      }
    }
  }
}
