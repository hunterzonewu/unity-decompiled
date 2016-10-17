// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimatedValues.AnimQuaternion
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.AnimatedValues
{
  /// <summary>
  ///   <para>An animated Quaternion value.</para>
  /// </summary>
  [Serializable]
  public class AnimQuaternion : BaseAnimValue<Quaternion>
  {
    [SerializeField]
    private Quaternion m_Value;

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="value">Start Value.</param>
    /// <param name="callback"></param>
    public AnimQuaternion(Quaternion value)
      : base(value)
    {
    }

    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="value">Start Value.</param>
    /// <param name="callback"></param>
    public AnimQuaternion(Quaternion value, UnityAction callback)
      : base(value, callback)
    {
    }

    /// <summary>
    ///   <para>Type specific implementation of BaseAnimValue_1.GetValue.</para>
    /// </summary>
    /// <returns>
    ///   <para>Current Value.</para>
    /// </returns>
    protected override Quaternion GetValue()
    {
      this.m_Value = Quaternion.Slerp(this.start, this.target, this.lerpPosition);
      return this.m_Value;
    }
  }
}
