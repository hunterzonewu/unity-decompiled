// Decompiled with JetBrains decompiler
// Type: UnityEditor.FlowLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class FlowLayout : GUILayoutGroup
  {
    private int m_Lines;
    private FlowLayout.LineInfo[] m_LineInfo;

    public override void CalcWidth()
    {
      bool flag = (double) this.minWidth != 0.0;
      base.CalcWidth();
      if (this.isVertical || flag)
        return;
      this.minWidth = 0.0f;
      using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.minWidth = Mathf.Max(this.m_ChildMinWidth, enumerator.Current.minWidth);
      }
    }

    public override void SetHorizontal(float x, float width)
    {
      base.SetHorizontal(x, width);
      if (this.resetCoords)
        x = 0.0f;
      if (this.isVertical)
      {
        Debug.LogError((object) "Wordwrapped vertical group. Don't. Just Don't");
      }
      else
      {
        this.m_Lines = 0;
        float num = 0.0f;
        using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GUILayoutEntry current = enumerator.Current;
            if ((double) current.rect.xMax - (double) num > (double) x + (double) width)
            {
              num = current.rect.x - (float) current.margin.left;
              ++this.m_Lines;
            }
            current.SetHorizontal(current.rect.x - num, current.rect.width);
            current.rect.y = (float) this.m_Lines;
          }
        }
        ++this.m_Lines;
      }
    }

    public override void CalcHeight()
    {
      if (this.entries.Count == 0)
      {
        this.maxHeight = this.minHeight = 0.0f;
      }
      else
      {
        this.m_ChildMinHeight = this.m_ChildMaxHeight = 0.0f;
        int num1 = 0;
        int num2 = 0;
        this.m_StretchableCountY = 0;
        if (!this.isVertical)
        {
          this.m_LineInfo = new FlowLayout.LineInfo[this.m_Lines];
          for (int index = 0; index < this.m_Lines; ++index)
          {
            this.m_LineInfo[index].topBorder = 10000;
            this.m_LineInfo[index].bottomBorder = 10000;
          }
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              current.CalcHeight();
              int y = (int) current.rect.y;
              this.m_LineInfo[y].minSize = Mathf.Max(current.minHeight, this.m_LineInfo[y].minSize);
              this.m_LineInfo[y].maxSize = Mathf.Max(current.maxHeight, this.m_LineInfo[y].maxSize);
              this.m_LineInfo[y].topBorder = Mathf.Min(current.margin.top, this.m_LineInfo[y].topBorder);
              this.m_LineInfo[y].bottomBorder = Mathf.Min(current.margin.bottom, this.m_LineInfo[y].bottomBorder);
            }
          }
          for (int index = 0; index < this.m_Lines; ++index)
          {
            this.m_ChildMinHeight += (float) (double) this.m_LineInfo[index].minSize;
            this.m_ChildMaxHeight += (float) (double) this.m_LineInfo[index].maxSize;
          }
          for (int index = 1; index < this.m_Lines; ++index)
          {
            float num3 = (float) Mathf.Max(this.m_LineInfo[index - 1].bottomBorder, this.m_LineInfo[index].topBorder);
            this.m_ChildMinHeight += (float) (double) num3;
            this.m_ChildMaxHeight += (float) (double) num3;
          }
          num1 = this.m_LineInfo[0].topBorder;
          num2 = this.m_LineInfo[this.m_LineInfo.Length - 1].bottomBorder;
        }
        this.margin.top = num1;
        this.margin.bottom = num2;
        float num4;
        float num5 = num4 = 0.0f;
        this.minHeight = Mathf.Max(this.minHeight, this.m_ChildMinHeight + num5 + num4);
        if ((double) this.maxHeight == 0.0)
        {
          this.stretchHeight += this.m_StretchableCountY + (!this.style.stretchHeight ? 0 : 1);
          this.maxHeight = this.m_ChildMaxHeight + num5 + num4;
        }
        else
          this.stretchHeight = 0;
        this.maxHeight = Mathf.Max(this.maxHeight, this.minHeight);
      }
    }

    public override void SetVertical(float y, float height)
    {
      if (this.entries.Count == 0)
        base.SetVertical(y, height);
      else if (this.isVertical)
      {
        base.SetVertical(y, height);
      }
      else
      {
        if (this.resetCoords)
          y = 0.0f;
        float num1 = y - (float) this.margin.top;
        float num2 = y + (float) this.margin.vertical - this.spacing * (float) (this.m_Lines - 1);
        float t = 0.0f;
        if ((double) this.m_ChildMinHeight != (double) this.m_ChildMaxHeight)
          t = Mathf.Clamp((float) (((double) num2 - (double) this.m_ChildMinHeight) / ((double) this.m_ChildMaxHeight - (double) this.m_ChildMinHeight)), 0.0f, 1f);
        float num3 = num1;
        for (int index = 0; index < this.m_Lines; ++index)
        {
          if (index > 0)
            num3 += (float) Mathf.Max(this.m_LineInfo[index].topBorder, this.m_LineInfo[index - 1].bottomBorder);
          this.m_LineInfo[index].start = num3;
          this.m_LineInfo[index].size = Mathf.Lerp(this.m_LineInfo[index].minSize, this.m_LineInfo[index].maxSize, t);
          num3 += this.m_LineInfo[index].size + this.spacing;
        }
        using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GUILayoutEntry current = enumerator.Current;
            FlowLayout.LineInfo lineInfo = this.m_LineInfo[(int) current.rect.y];
            if (current.stretchHeight != 0)
              current.SetVertical(lineInfo.start + (float) current.margin.top, lineInfo.size - (float) current.margin.vertical);
            else
              current.SetVertical(lineInfo.start + (float) current.margin.top, Mathf.Clamp(lineInfo.size - (float) current.margin.vertical, current.minHeight, current.maxHeight));
          }
        }
      }
    }

    private struct LineInfo
    {
      public float minSize;
      public float maxSize;
      public float start;
      public float size;
      public int topBorder;
      public int bottomBorder;
      public int expandSize;
    }
  }
}
