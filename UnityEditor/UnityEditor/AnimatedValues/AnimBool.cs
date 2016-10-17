// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimatedValues.AnimBool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.AnimatedValues
{
  /// <summary>
  ///   <para>Lerp from 0 - 1.</para>
  /// </summary>
  [Serializable]
  public class AnimBool : BaseAnimValue<bool>
  {
    [SerializeField]
    private float m_Value;

    /// <summary>
    ///   <para>Retuns the float value of the tween.</para>
    /// </summary>
    public float faded
    {
      get
      {
        this.GetValue();
        return this.m_Value;
      }
    }

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="value">Start Value.</param>
    /// <param name="callback"></param>
    public AnimBool()
      : base(false)
    {
    }

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="value">Start Value.</param>
    /// <param name="callback"></param>
    public AnimBool(bool value)
      : base(value)
    {
    }

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="value">Start Value.</param>
    /// <param name="callback"></param>
    public AnimBool(UnityAction callback)
      : base(false, callback)
    {
    }

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="value">Start Value.</param>
    /// <param name="callback"></param>
    public AnimBool(bool value, UnityAction callback)
      : base(value, callback)
    {
    }

    /// <summary>
    ///   <para>Type specific implementation of BaseAnimValue_1.GetValue.</para>
    /// </summary>
    /// <returns>
    ///   <para>Current value.</para>
    /// </returns>
    protected override bool GetValue()
    {
      float a = !this.target ? 1f : 0.0f;
      float b = 1f - a;
      this.m_Value = Mathf.Lerp(a, b, this.lerpPosition);
      return (double) this.m_Value > 0.5;
    }

    /// <summary>
    ///   <para>Returns a value between from and to depending on the current value of the bools animation.</para>
    /// </summary>
    /// <param name="from">Value to lerp from.</param>
    /// <param name="to">Value to lerp to.</param>
    public float Fade(float from, float to)
    {
      return Mathf.Lerp(from, to, this.faded);
    }
  }
}
