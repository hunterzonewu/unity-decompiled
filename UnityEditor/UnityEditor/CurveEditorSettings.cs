// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveEditorSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CurveEditorSettings
  {
    private TickStyle m_HTickStyle = new TickStyle();
    private TickStyle m_VTickStyle = new TickStyle();
    private float m_HRangeMin = float.NegativeInfinity;
    private float m_HRangeMax = float.PositiveInfinity;
    private float m_VRangeMin = float.NegativeInfinity;
    private float m_VRangeMax = float.PositiveInfinity;
    public Color wrapColor = new Color(1f, 1f, 1f, 0.5f);
    public bool showAxisLabels = true;
    public bool allowDraggingCurvesAndRegions = true;
    private bool m_ScaleWithWindow = true;
    private bool m_HSlider = true;
    private bool m_VSlider = true;
    private bool m_HRangeLocked;
    private bool m_VRangeLocked;
    public float hTickLabelOffset;
    public bool useFocusColors;
    public bool allowDeleteLastKeyInCurve;

    internal TickStyle hTickStyle
    {
      get
      {
        return this.m_HTickStyle;
      }
      set
      {
        this.m_HTickStyle = value;
      }
    }

    internal TickStyle vTickStyle
    {
      get
      {
        return this.m_VTickStyle;
      }
      set
      {
        this.m_VTickStyle = value;
      }
    }

    internal bool hRangeLocked
    {
      get
      {
        return this.m_HRangeLocked;
      }
      set
      {
        this.m_HRangeLocked = value;
      }
    }

    internal bool vRangeLocked
    {
      get
      {
        return this.m_VRangeLocked;
      }
      set
      {
        this.m_VRangeLocked = value;
      }
    }

    public float hRangeMin
    {
      get
      {
        return this.m_HRangeMin;
      }
      set
      {
        this.m_HRangeMin = value;
      }
    }

    public float hRangeMax
    {
      get
      {
        return this.m_HRangeMax;
      }
      set
      {
        this.m_HRangeMax = value;
      }
    }

    public float vRangeMin
    {
      get
      {
        return this.m_VRangeMin;
      }
      set
      {
        this.m_VRangeMin = value;
      }
    }

    public float vRangeMax
    {
      get
      {
        return this.m_VRangeMax;
      }
      set
      {
        this.m_VRangeMax = value;
      }
    }

    public bool hasUnboundedRanges
    {
      get
      {
        if ((double) this.m_HRangeMin != double.NegativeInfinity && (double) this.m_HRangeMax != double.PositiveInfinity && (double) this.m_VRangeMin != double.NegativeInfinity)
          return (double) this.m_VRangeMax == double.PositiveInfinity;
        return true;
      }
    }

    internal bool scaleWithWindow
    {
      get
      {
        return this.m_ScaleWithWindow;
      }
      set
      {
        this.m_ScaleWithWindow = value;
      }
    }

    public bool hSlider
    {
      get
      {
        return this.m_HSlider;
      }
      set
      {
        this.m_HSlider = value;
      }
    }

    public bool vSlider
    {
      get
      {
        return this.m_VSlider;
      }
      set
      {
        this.m_VSlider = value;
      }
    }

    public CurveEditorSettings()
    {
      if (EditorGUIUtility.isProSkin)
        this.wrapColor = new Color(0.65f, 0.65f, 0.65f, 0.5f);
      else
        this.wrapColor = new Color(1f, 1f, 1f, 0.5f);
    }
  }
}
