// Decompiled with JetBrains decompiler
// Type: UnityEngine.MatchTargetWeightMask
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>To specify position and rotation weight mask for Animator::MatchTarget.</para>
  /// </summary>
  public struct MatchTargetWeightMask
  {
    private Vector3 m_PositionXYZWeight;
    private float m_RotationWeight;

    /// <summary>
    ///   <para>Position XYZ weight.</para>
    /// </summary>
    public Vector3 positionXYZWeight
    {
      get
      {
        return this.m_PositionXYZWeight;
      }
      set
      {
        this.m_PositionXYZWeight = value;
      }
    }

    /// <summary>
    ///   <para>Rotation weight.</para>
    /// </summary>
    public float rotationWeight
    {
      get
      {
        return this.m_RotationWeight;
      }
      set
      {
        this.m_RotationWeight = value;
      }
    }

    /// <summary>
    ///   <para>MatchTargetWeightMask contructor.</para>
    /// </summary>
    /// <param name="positionXYZWeight">Position XYZ weight.</param>
    /// <param name="rotationWeight">Rotation weight.</param>
    public MatchTargetWeightMask(Vector3 positionXYZWeight, float rotationWeight)
    {
      this.m_PositionXYZWeight = positionXYZWeight;
      this.m_RotationWeight = rotationWeight;
    }
  }
}
