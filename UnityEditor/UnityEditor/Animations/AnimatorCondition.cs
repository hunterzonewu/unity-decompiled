// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorCondition
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Condition that is used to determine if a transition must be taken.</para>
  /// </summary>
  public struct AnimatorCondition
  {
    private AnimatorConditionMode m_ConditionMode;
    private string m_ConditionEvent;
    private float m_EventTreshold;

    /// <summary>
    ///   <para>The mode of the condition.</para>
    /// </summary>
    public AnimatorConditionMode mode
    {
      get
      {
        return this.m_ConditionMode;
      }
      set
      {
        this.m_ConditionMode = value;
      }
    }

    /// <summary>
    ///   <para>The name of the parameter used in the condition.</para>
    /// </summary>
    public string parameter
    {
      get
      {
        return this.m_ConditionEvent;
      }
      set
      {
        this.m_ConditionEvent = value;
      }
    }

    /// <summary>
    ///   <para>The AnimatorParameter's threshold value for the condition to be true.</para>
    /// </summary>
    public float threshold
    {
      get
      {
        return this.m_EventTreshold;
      }
      set
      {
        this.m_EventTreshold = value;
      }
    }
  }
}
