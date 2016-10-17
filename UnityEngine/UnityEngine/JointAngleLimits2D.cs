// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointAngleLimits2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Angular limits on the rotation of a Rigidbody2D object around a HingeJoint2D.</para>
  /// </summary>
  public struct JointAngleLimits2D
  {
    private float m_LowerAngle;
    private float m_UpperAngle;

    /// <summary>
    ///   <para>Lower angular limit of rotation.</para>
    /// </summary>
    public float min
    {
      get
      {
        return this.m_LowerAngle;
      }
      set
      {
        this.m_LowerAngle = value;
      }
    }

    /// <summary>
    ///   <para>Upper angular limit of rotation.</para>
    /// </summary>
    public float max
    {
      get
      {
        return this.m_UpperAngle;
      }
      set
      {
        this.m_UpperAngle = value;
      }
    }
  }
}
