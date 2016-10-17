// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimatedValues.BaseAnimValue`1
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor.AnimatedValues
{
  /// <summary>
  ///   <para>Abstract base class for Animated Values.</para>
  /// </summary>
  public abstract class BaseAnimValue<T>
  {
    private double m_LerpPosition = 1.0;
    public float speed = 2f;
    private T m_Start;
    [SerializeField]
    private T m_Target;
    private double m_LastTime;
    [NonSerialized]
    public UnityEvent valueChanged;
    private bool m_Animating;

    public bool isAnimating
    {
      get
      {
        return this.m_Animating;
      }
    }

    protected float lerpPosition
    {
      get
      {
        double num = 1.0 - this.m_LerpPosition;
        return (float) (1.0 - num * num * num * num);
      }
    }

    protected T start
    {
      get
      {
        return this.m_Start;
      }
    }

    public T target
    {
      get
      {
        return this.m_Target;
      }
      set
      {
        if (this.m_Target.Equals((object) value))
          return;
        this.BeginAnimating(value, this.value);
      }
    }

    public T value
    {
      get
      {
        return this.GetValue();
      }
      set
      {
        this.StopAnim(value);
      }
    }

    protected BaseAnimValue(T value)
    {
      this.m_Start = value;
      this.m_Target = value;
      this.valueChanged = new UnityEvent();
    }

    protected BaseAnimValue(T value, UnityAction callback)
    {
      this.m_Start = value;
      this.m_Target = value;
      this.valueChanged = new UnityEvent();
      this.valueChanged.AddListener(callback);
    }

    private static T2 Clamp<T2>(T2 val, T2 min, T2 max) where T2 : IComparable<T2>
    {
      if (val.CompareTo(min) < 0)
        return min;
      if (val.CompareTo(max) > 0)
        return max;
      return val;
    }

    protected void BeginAnimating(T newTarget, T newStart)
    {
      this.m_Start = newStart;
      this.m_Target = newTarget;
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
      this.m_Animating = true;
      this.m_LastTime = EditorApplication.timeSinceStartup;
      this.m_LerpPosition = 0.0;
    }

    private void Update()
    {
      if (!this.m_Animating)
        return;
      this.UpdateLerpPosition();
      if (this.valueChanged != null)
        this.valueChanged.Invoke();
      if ((double) this.lerpPosition < 1.0)
        return;
      this.m_Animating = false;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
    }

    private void UpdateLerpPosition()
    {
      double timeSinceStartup = EditorApplication.timeSinceStartup;
      this.m_LerpPosition = BaseAnimValue<T>.Clamp<double>(this.m_LerpPosition + (timeSinceStartup - this.m_LastTime) * (double) this.speed, 0.0, 1.0);
      this.m_LastTime = timeSinceStartup;
    }

    protected void StopAnim(T newValue)
    {
      bool flag = false;
      if ((!newValue.Equals((object) this.GetValue()) || this.m_LerpPosition < 1.0) && this.valueChanged != null)
        flag = true;
      this.m_Target = newValue;
      this.m_Start = newValue;
      this.m_LerpPosition = 1.0;
      this.m_Animating = false;
      if (!flag)
        return;
      this.valueChanged.Invoke();
    }

    protected abstract T GetValue();
  }
}
