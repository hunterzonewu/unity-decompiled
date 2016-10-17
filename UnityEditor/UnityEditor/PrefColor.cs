// Decompiled with JetBrains decompiler
// Type: UnityEditor.PrefColor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Globalization;
using UnityEngine;

namespace UnityEditor
{
  internal class PrefColor : IPrefType
  {
    private string m_name;
    private Color m_color;
    private Color m_DefaultColor;

    public Color Color
    {
      get
      {
        return this.m_color;
      }
      set
      {
        this.m_color = value;
      }
    }

    public string Name
    {
      get
      {
        return this.m_name;
      }
    }

    public PrefColor()
    {
    }

    public PrefColor(string name, float defaultRed, float defaultGreen, float defaultBlue, float defaultAlpha)
    {
      this.m_name = name;
      this.m_color = this.m_DefaultColor = new Color(defaultRed, defaultGreen, defaultBlue, defaultAlpha);
      PrefColor prefColor = Settings.Get<PrefColor>(name, this);
      this.m_name = prefColor.Name;
      this.m_color = prefColor.Color;
    }

    public static implicit operator Color(PrefColor pcolor)
    {
      return pcolor.Color;
    }

    public string ToUniqueString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0};{1};{2};{3};{4}", (object) this.m_name, (object) this.Color.r, (object) this.Color.g, (object) this.Color.b, (object) this.Color.a);
    }

    public void FromUniqueString(string s)
    {
      string[] strArray = s.Split(';');
      if (strArray.Length != 5)
      {
        Debug.LogError((object) "Parsing PrefColor failed");
      }
      else
      {
        this.m_name = strArray[0];
        strArray[1] = strArray[1].Replace(',', '.');
        strArray[2] = strArray[2].Replace(',', '.');
        strArray[3] = strArray[3].Replace(',', '.');
        strArray[4] = strArray[4].Replace(',', '.');
        float result1;
        float result2;
        float result3;
        float result4;
        if (float.TryParse(strArray[1], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result1) & float.TryParse(strArray[2], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result2) & float.TryParse(strArray[3], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result3) & float.TryParse(strArray[4], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, out result4))
          this.m_color = new Color(result1, result2, result3, result4);
        else
          Debug.LogError((object) "Parsing PrefColor failed");
      }
    }

    internal void ResetToDefault()
    {
      this.m_color = this.m_DefaultColor;
    }
  }
}
