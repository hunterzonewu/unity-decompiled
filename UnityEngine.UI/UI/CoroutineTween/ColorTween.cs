// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.CoroutineTween.ColorTween
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using UnityEngine.Events;

namespace UnityEngine.UI.CoroutineTween
{
  internal struct ColorTween : ITweenValue
  {
    private ColorTween.ColorTweenCallback m_Target;
    private Color m_StartColor;
    private Color m_TargetColor;
    private ColorTween.ColorTweenMode m_TweenMode;
    private float m_Duration;
    private bool m_IgnoreTimeScale;

    public Color startColor
    {
      get
      {
        return this.m_StartColor;
      }
      set
      {
        this.m_StartColor = value;
      }
    }

    public Color targetColor
    {
      get
      {
        return this.m_TargetColor;
      }
      set
      {
        this.m_TargetColor = value;
      }
    }

    public ColorTween.ColorTweenMode tweenMode
    {
      get
      {
        return this.m_TweenMode;
      }
      set
      {
        this.m_TweenMode = value;
      }
    }

    public float duration
    {
      get
      {
        return this.m_Duration;
      }
      set
      {
        this.m_Duration = value;
      }
    }

    public bool ignoreTimeScale
    {
      get
      {
        return this.m_IgnoreTimeScale;
      }
      set
      {
        this.m_IgnoreTimeScale = value;
      }
    }

    public void TweenValue(float floatPercentage)
    {
      if (!this.ValidTarget())
        return;
      Color color = Color.Lerp(this.m_StartColor, this.m_TargetColor, floatPercentage);
      if (this.m_TweenMode == ColorTween.ColorTweenMode.Alpha)
      {
        color.r = this.m_StartColor.r;
        color.g = this.m_StartColor.g;
        color.b = this.m_StartColor.b;
      }
      else if (this.m_TweenMode == ColorTween.ColorTweenMode.RGB)
        color.a = this.m_StartColor.a;
      this.m_Target.Invoke(color);
    }

    public void AddOnChangedCallback(UnityAction<Color> callback)
    {
      if (this.m_Target == null)
        this.m_Target = new ColorTween.ColorTweenCallback();
      this.m_Target.AddListener(callback);
    }

    public bool GetIgnoreTimescale()
    {
      return this.m_IgnoreTimeScale;
    }

    public float GetDuration()
    {
      return this.m_Duration;
    }

    public bool ValidTarget()
    {
      return this.m_Target != null;
    }

    public enum ColorTweenMode
    {
      All,
      RGB,
      Alpha,
    }

    public class ColorTweenCallback : UnityEvent<Color>
    {
    }
  }
}
