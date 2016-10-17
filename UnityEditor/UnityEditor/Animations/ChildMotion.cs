// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.ChildMotion
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Structure that represents a motion in the context of its parent blend tree.</para>
  /// </summary>
  public struct ChildMotion
  {
    private Motion m_Motion;
    private float m_Threshold;
    private Vector2 m_Position;
    private float m_TimeScale;
    private float m_CycleOffset;
    private string m_DirectBlendParameter;
    private bool m_Mirror;

    /// <summary>
    ///   <para>The motion itself.</para>
    /// </summary>
    public Motion motion
    {
      get
      {
        return this.m_Motion;
      }
      set
      {
        this.m_Motion = value;
      }
    }

    /// <summary>
    ///   <para>The threshold of the child. Used in 1D blend trees.</para>
    /// </summary>
    public float threshold
    {
      get
      {
        return this.m_Threshold;
      }
      set
      {
        this.m_Threshold = value;
      }
    }

    /// <summary>
    ///   <para>The position of the child. Used in 2D blend trees.</para>
    /// </summary>
    public Vector2 position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.m_Position = value;
      }
    }

    /// <summary>
    ///   <para>The relative speed of the child.</para>
    /// </summary>
    public float timeScale
    {
      get
      {
        return this.m_TimeScale;
      }
      set
      {
        this.m_TimeScale = value;
      }
    }

    /// <summary>
    ///   <para>Normalized time offset of the child.</para>
    /// </summary>
    public float cycleOffset
    {
      get
      {
        return this.m_CycleOffset;
      }
      set
      {
        this.m_CycleOffset = value;
      }
    }

    /// <summary>
    ///   <para>The parameter used by the child when used in a BlendTree of type BlendTreeType.Direct.</para>
    /// </summary>
    public string directBlendParameter
    {
      get
      {
        return this.m_DirectBlendParameter;
      }
      set
      {
        this.m_DirectBlendParameter = value;
      }
    }

    /// <summary>
    ///   <para>Mirror of the child.</para>
    /// </summary>
    public bool mirror
    {
      get
      {
        return this.m_Mirror;
      }
      set
      {
        this.m_Mirror = value;
      }
    }
  }
}
