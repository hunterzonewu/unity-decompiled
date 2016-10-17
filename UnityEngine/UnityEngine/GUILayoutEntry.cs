// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUILayoutEntry
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  internal class GUILayoutEntry
  {
    internal static Rect kDummyRect = new Rect(0.0f, 0.0f, 1f, 1f);
    protected static int indent = 0;
    public Rect rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    private GUIStyle m_Style = GUIStyle.none;
    public float minWidth;
    public float maxWidth;
    public float minHeight;
    public float maxHeight;
    public int stretchWidth;
    public int stretchHeight;

    public GUIStyle style
    {
      get
      {
        return this.m_Style;
      }
      set
      {
        this.m_Style = value;
        this.ApplyStyleSettings(value);
      }
    }

    public virtual RectOffset margin
    {
      get
      {
        return this.style.margin;
      }
    }

    public GUILayoutEntry(float _minWidth, float _maxWidth, float _minHeight, float _maxHeight, GUIStyle _style)
    {
      this.minWidth = _minWidth;
      this.maxWidth = _maxWidth;
      this.minHeight = _minHeight;
      this.maxHeight = _maxHeight;
      if (_style == null)
        _style = GUIStyle.none;
      this.style = _style;
    }

    public GUILayoutEntry(float _minWidth, float _maxWidth, float _minHeight, float _maxHeight, GUIStyle _style, GUILayoutOption[] options)
    {
      this.minWidth = _minWidth;
      this.maxWidth = _maxWidth;
      this.minHeight = _minHeight;
      this.maxHeight = _maxHeight;
      this.style = _style;
      this.ApplyOptions(options);
    }

    public virtual void CalcWidth()
    {
    }

    public virtual void CalcHeight()
    {
    }

    public virtual void SetHorizontal(float x, float width)
    {
      this.rect.x = x;
      this.rect.width = width;
    }

    public virtual void SetVertical(float y, float height)
    {
      this.rect.y = y;
      this.rect.height = height;
    }

    protected virtual void ApplyStyleSettings(GUIStyle style)
    {
      this.stretchWidth = (double) style.fixedWidth != 0.0 || !style.stretchWidth ? 0 : 1;
      this.stretchHeight = (double) style.fixedHeight != 0.0 || !style.stretchHeight ? 0 : 1;
      this.m_Style = style;
    }

    public virtual void ApplyOptions(GUILayoutOption[] options)
    {
      if (options == null)
        return;
      foreach (GUILayoutOption option in options)
      {
        switch (option.type)
        {
          case GUILayoutOption.Type.fixedWidth:
            this.minWidth = this.maxWidth = (float) option.value;
            this.stretchWidth = 0;
            break;
          case GUILayoutOption.Type.fixedHeight:
            this.minHeight = this.maxHeight = (float) option.value;
            this.stretchHeight = 0;
            break;
          case GUILayoutOption.Type.minWidth:
            this.minWidth = (float) option.value;
            if ((double) this.maxWidth < (double) this.minWidth)
            {
              this.maxWidth = this.minWidth;
              break;
            }
            break;
          case GUILayoutOption.Type.maxWidth:
            this.maxWidth = (float) option.value;
            if ((double) this.minWidth > (double) this.maxWidth)
              this.minWidth = this.maxWidth;
            this.stretchWidth = 0;
            break;
          case GUILayoutOption.Type.minHeight:
            this.minHeight = (float) option.value;
            if ((double) this.maxHeight < (double) this.minHeight)
            {
              this.maxHeight = this.minHeight;
              break;
            }
            break;
          case GUILayoutOption.Type.maxHeight:
            this.maxHeight = (float) option.value;
            if ((double) this.minHeight > (double) this.maxHeight)
              this.minHeight = this.maxHeight;
            this.stretchHeight = 0;
            break;
          case GUILayoutOption.Type.stretchWidth:
            this.stretchWidth = (int) option.value;
            break;
          case GUILayoutOption.Type.stretchHeight:
            this.stretchHeight = (int) option.value;
            break;
        }
      }
      if ((double) this.maxWidth != 0.0 && (double) this.maxWidth < (double) this.minWidth)
        this.maxWidth = this.minWidth;
      if ((double) this.maxHeight == 0.0 || (double) this.maxHeight >= (double) this.minHeight)
        return;
      this.maxHeight = this.minHeight;
    }

    public override string ToString()
    {
      string empty = string.Empty;
      for (int index = 0; index < GUILayoutEntry.indent; ++index)
        empty += " ";
      object[] objArray = new object[12];
      objArray[0] = (object) empty;
      int index1 = 1;
      string str1 = UnityString.Format("{1}-{0} (x:{2}-{3}, y:{4}-{5})", (object) (this.style == null ? "NULL" : this.style.name), (object) this.GetType(), (object) this.rect.x, (object) this.rect.xMax, (object) this.rect.y, (object) this.rect.yMax);
      objArray[index1] = (object) str1;
      int index2 = 2;
      string str2 = "   -   W: ";
      objArray[index2] = (object) str2;
      int index3 = 3;
      // ISSUE: variable of a boxed type
      __Boxed<float> minWidth = (ValueType) this.minWidth;
      objArray[index3] = (object) minWidth;
      int index4 = 4;
      string str3 = "-";
      objArray[index4] = (object) str3;
      int index5 = 5;
      // ISSUE: variable of a boxed type
      __Boxed<float> maxWidth = (ValueType) this.maxWidth;
      objArray[index5] = (object) maxWidth;
      int index6 = 6;
      string str4 = this.stretchWidth == 0 ? string.Empty : "+";
      objArray[index6] = (object) str4;
      int index7 = 7;
      string str5 = ", H: ";
      objArray[index7] = (object) str5;
      int index8 = 8;
      // ISSUE: variable of a boxed type
      __Boxed<float> minHeight = (ValueType) this.minHeight;
      objArray[index8] = (object) minHeight;
      int index9 = 9;
      string str6 = "-";
      objArray[index9] = (object) str6;
      int index10 = 10;
      // ISSUE: variable of a boxed type
      __Boxed<float> maxHeight = (ValueType) this.maxHeight;
      objArray[index10] = (object) maxHeight;
      int index11 = 11;
      string str7 = this.stretchHeight == 0 ? string.Empty : "+";
      objArray[index11] = (object) str7;
      return string.Concat(objArray);
    }
  }
}
