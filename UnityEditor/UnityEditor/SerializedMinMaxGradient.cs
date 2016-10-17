// Decompiled with JetBrains decompiler
// Type: UnityEditor.SerializedMinMaxGradient
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class SerializedMinMaxGradient
  {
    public SerializedProperty m_MaxGradient;
    public SerializedProperty m_MinGradient;
    public SerializedProperty m_MaxColor;
    public SerializedProperty m_MinColor;
    private SerializedProperty m_MinMaxState;
    public bool m_AllowColor;
    public bool m_AllowGradient;
    public bool m_AllowRandomBetweenTwoColors;
    public bool m_AllowRandomBetweenTwoGradients;

    public MinMaxGradientState state
    {
      get
      {
        return (MinMaxGradientState) this.m_MinMaxState.intValue;
      }
      set
      {
        this.SetMinMaxState(value);
      }
    }

    public SerializedMinMaxGradient(SerializedModule m)
    {
      this.Init(m, "gradient");
    }

    public SerializedMinMaxGradient(SerializedModule m, string name)
    {
      this.Init(m, name);
    }

    private void Init(SerializedModule m, string name)
    {
      this.m_MaxGradient = m.GetProperty(name, "maxGradient");
      this.m_MinGradient = m.GetProperty(name, "minGradient");
      this.m_MaxColor = m.GetProperty(name, "maxColor");
      this.m_MinColor = m.GetProperty(name, "minColor");
      this.m_MinMaxState = m.GetProperty(name, "minMaxState");
      this.m_AllowColor = true;
      this.m_AllowGradient = true;
      this.m_AllowRandomBetweenTwoColors = true;
      this.m_AllowRandomBetweenTwoGradients = true;
    }

    private void SetMinMaxState(MinMaxGradientState newState)
    {
      if (newState == this.state)
        return;
      this.m_MinMaxState.intValue = (int) newState;
    }

    public static Color GetGradientAsColor(SerializedProperty gradientProp)
    {
      return gradientProp.gradientValue.constantColor;
    }

    public static void SetGradientAsColor(SerializedProperty gradientProp, Color color)
    {
      gradientProp.gradientValue.constantColor = color;
      GradientPreviewCache.ClearCache();
    }
  }
}
