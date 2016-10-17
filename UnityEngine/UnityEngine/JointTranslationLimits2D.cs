// Decompiled with JetBrains decompiler
// Type: UnityEngine.JointTranslationLimits2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Motion limits of a Rigidbody2D object along a SliderJoint2D.</para>
  /// </summary>
  public struct JointTranslationLimits2D
  {
    private float m_LowerTranslation;
    private float m_UpperTranslation;

    /// <summary>
    ///   <para>Minimum distance the Rigidbody2D object can move from the Slider Joint's anchor.</para>
    /// </summary>
    public float min
    {
      get
      {
        return this.m_LowerTranslation;
      }
      set
      {
        this.m_LowerTranslation = value;
      }
    }

    /// <summary>
    ///   <para>Maximum distance the Rigidbody2D object can move from the Slider Joint's anchor.</para>
    /// </summary>
    public float max
    {
      get
      {
        return this.m_UpperTranslation;
      }
      set
      {
        this.m_UpperTranslation = value;
      }
    }
  }
}
