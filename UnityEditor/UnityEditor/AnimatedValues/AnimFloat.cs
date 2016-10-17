// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimatedValues.AnimFloat
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.AnimatedValues
{
  /// <summary>
  ///   <para>An animated float value.</para>
  /// </summary>
  [Serializable]
  public class AnimFloat : BaseAnimValue<float>
  {
    [SerializeField]
    private float m_Value;

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="value">Start Value.</param>
    /// <param name="callback"></param>
    public AnimFloat(float value)
      : base(value)
    {
    }

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="value">Start Value.</param>
    /// <param name="callback"></param>
    public AnimFloat(float value, UnityAction callback)
      : base(value, callback)
    {
    }

    /// <summary>
    ///   <para>Type specific implementation of BaseAnimValue_1.GetValue.</para>
    /// </summary>
    /// <returns>
    ///   <para>Current Value.</para>
    /// </returns>
    protected override float GetValue()
    {
      this.m_Value = Mathf.Lerp(this.start, this.target, this.lerpPosition);
      return this.m_Value;
    }
  }
}
