// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUILayoutGroup
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine
{
  internal class GUILayoutGroup : GUILayoutEntry
  {
    public List<GUILayoutEntry> entries = new List<GUILayoutEntry>();
    public bool isVertical = true;
    public bool sameSize = true;
    public int windowID = -1;
    protected int m_StretchableCountX = 100;
    protected int m_StretchableCountY = 100;
    protected float m_ChildMinWidth = 100f;
    protected float m_ChildMaxWidth = 100f;
    protected float m_ChildMinHeight = 100f;
    protected float m_ChildMaxHeight = 100f;
    private readonly RectOffset m_Margin = new RectOffset();
    public bool resetCoords;
    public float spacing;
    public bool isWindow;
    private int m_Cursor;
    protected bool m_UserSpecifiedWidth;
    protected bool m_UserSpecifiedHeight;

    public override RectOffset margin
    {
      get
      {
        return this.m_Margin;
      }
    }

    public GUILayoutGroup()
      : base(0.0f, 0.0f, 0.0f, 0.0f, GUIStyle.none)
    {
    }

    public GUILayoutGroup(GUIStyle _style, GUILayoutOption[] options)
      : base(0.0f, 0.0f, 0.0f, 0.0f, _style)
    {
      if (options != null)
        this.ApplyOptions(options);
      this.m_Margin.left = _style.margin.left;
      this.m_Margin.right = _style.margin.right;
      this.m_Margin.top = _style.margin.top;
      this.m_Margin.bottom = _style.margin.bottom;
    }

    public override void ApplyOptions(GUILayoutOption[] options)
    {
      if (options == null)
        return;
      base.ApplyOptions(options);
      foreach (GUILayoutOption option in options)
      {
        switch (option.type)
        {
          case GUILayoutOption.Type.fixedWidth:
          case GUILayoutOption.Type.minWidth:
          case GUILayoutOption.Type.maxWidth:
            this.m_UserSpecifiedHeight = true;
            break;
          case GUILayoutOption.Type.fixedHeight:
          case GUILayoutOption.Type.minHeight:
          case GUILayoutOption.Type.maxHeight:
            this.m_UserSpecifiedWidth = true;
            break;
          case GUILayoutOption.Type.spacing:
            this.spacing = (float) (int) option.value;
            break;
        }
      }
    }

    protected override void ApplyStyleSettings(GUIStyle style)
    {
      base.ApplyStyleSettings(style);
      RectOffset margin = style.margin;
      this.m_Margin.left = margin.left;
      this.m_Margin.right = margin.right;
      this.m_Margin.top = margin.top;
      this.m_Margin.bottom = margin.bottom;
    }

    public void ResetCursor()
    {
      this.m_Cursor = 0;
    }

    public Rect PeekNext()
    {
      if (this.m_Cursor < this.entries.Count)
        return this.entries[this.m_Cursor].rect;
      throw new ArgumentException("Getting control " + (object) this.m_Cursor + "'s position in a group with only " + (object) this.entries.Count + " controls when doing " + (object) Event.current.rawType + "\nAborting");
    }

    public GUILayoutEntry GetNext()
    {
      if (this.m_Cursor < this.entries.Count)
      {
        GUILayoutEntry entry = this.entries[this.m_Cursor];
        ++this.m_Cursor;
        return entry;
      }
      throw new ArgumentException("Getting control " + (object) this.m_Cursor + "'s position in a group with only " + (object) this.entries.Count + " controls when doing " + (object) Event.current.rawType + "\nAborting");
    }

    public Rect GetLast()
    {
      if (this.m_Cursor == 0)
      {
        Debug.LogError((object) "You cannot call GetLast immediately after beginning a group.");
        return GUILayoutEntry.kDummyRect;
      }
      if (this.m_Cursor <= this.entries.Count)
        return this.entries[this.m_Cursor - 1].rect;
      Debug.LogError((object) ("Getting control " + (object) this.m_Cursor + "'s position in a group with only " + (object) this.entries.Count + " controls when doing " + (object) Event.current.type));
      return GUILayoutEntry.kDummyRect;
    }

    public void Add(GUILayoutEntry e)
    {
      this.entries.Add(e);
    }

    public override void CalcWidth()
    {
      if (this.entries.Count == 0)
      {
        this.maxWidth = this.minWidth = (float) this.style.padding.horizontal;
      }
      else
      {
        int b1 = 0;
        int b2 = 0;
        this.m_ChildMinWidth = 0.0f;
        this.m_ChildMaxWidth = 0.0f;
        this.m_StretchableCountX = 0;
        bool flag = true;
        if (this.isVertical)
        {
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              current.CalcWidth();
              RectOffset margin = current.margin;
              if (current.style != GUILayoutUtility.spaceStyle)
              {
                if (!flag)
                {
                  b1 = Mathf.Min(margin.left, b1);
                  b2 = Mathf.Min(margin.right, b2);
                }
                else
                {
                  b1 = margin.left;
                  b2 = margin.right;
                  flag = false;
                }
                this.m_ChildMinWidth = Mathf.Max(current.minWidth + (float) margin.horizontal, this.m_ChildMinWidth);
                this.m_ChildMaxWidth = Mathf.Max(current.maxWidth + (float) margin.horizontal, this.m_ChildMaxWidth);
              }
              this.m_StretchableCountX += current.stretchWidth;
            }
          }
          this.m_ChildMinWidth -= (float) (b1 + b2);
          this.m_ChildMaxWidth -= (float) (b1 + b2);
        }
        else
        {
          int num1 = 0;
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              current.CalcWidth();
              RectOffset margin = current.margin;
              if (current.style != GUILayoutUtility.spaceStyle)
              {
                int num2;
                if (!flag)
                {
                  num2 = num1 <= margin.left ? margin.left : num1;
                }
                else
                {
                  num2 = 0;
                  flag = false;
                }
                this.m_ChildMinWidth += current.minWidth + this.spacing + (float) num2;
                this.m_ChildMaxWidth += current.maxWidth + this.spacing + (float) num2;
                num1 = margin.right;
                this.m_StretchableCountX += current.stretchWidth;
              }
              else
              {
                this.m_ChildMinWidth += current.minWidth;
                this.m_ChildMaxWidth += current.maxWidth;
                this.m_StretchableCountX += current.stretchWidth;
              }
            }
          }
          this.m_ChildMinWidth -= this.spacing;
          this.m_ChildMaxWidth -= this.spacing;
          if (this.entries.Count != 0)
          {
            b1 = this.entries[0].margin.left;
            b2 = num1;
          }
          else
            b1 = b2 = 0;
        }
        float num3;
        float num4;
        if (this.style != GUIStyle.none || this.m_UserSpecifiedWidth)
        {
          num3 = (float) Mathf.Max(this.style.padding.left, b1);
          num4 = (float) Mathf.Max(this.style.padding.right, b2);
        }
        else
        {
          this.m_Margin.left = b1;
          this.m_Margin.right = b2;
          num3 = num4 = 0.0f;
        }
        this.minWidth = Mathf.Max(this.minWidth, this.m_ChildMinWidth + num3 + num4);
        if ((double) this.maxWidth == 0.0)
        {
          this.stretchWidth += this.m_StretchableCountX + (!this.style.stretchWidth ? 0 : 1);
          this.maxWidth = this.m_ChildMaxWidth + num3 + num4;
        }
        else
          this.stretchWidth = 0;
        this.maxWidth = Mathf.Max(this.maxWidth, this.minWidth);
        if ((double) this.style.fixedWidth == 0.0)
          return;
        this.maxWidth = this.minWidth = this.style.fixedWidth;
        this.stretchWidth = 0;
      }
    }

    public override void SetHorizontal(float x, float width)
    {
      base.SetHorizontal(x, width);
      if (this.resetCoords)
        x = 0.0f;
      RectOffset padding = this.style.padding;
      if (this.isVertical)
      {
        if (this.style != GUIStyle.none)
        {
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              float num = (float) Mathf.Max(current.margin.left, padding.left);
              float x1 = x + num;
              float width1 = width - (float) Mathf.Max(current.margin.right, padding.right) - num;
              if (current.stretchWidth != 0)
                current.SetHorizontal(x1, width1);
              else
                current.SetHorizontal(x1, Mathf.Clamp(width1, current.minWidth, current.maxWidth));
            }
          }
        }
        else
        {
          float num1 = x - (float) this.margin.left;
          float num2 = width + (float) this.margin.horizontal;
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              if (current.stretchWidth != 0)
                current.SetHorizontal(num1 + (float) current.margin.left, num2 - (float) current.margin.horizontal);
              else
                current.SetHorizontal(num1 + (float) current.margin.left, Mathf.Clamp(num2 - (float) current.margin.horizontal, current.minWidth, current.maxWidth));
            }
          }
        }
      }
      else
      {
        if (this.style != GUIStyle.none)
        {
          float a1 = (float) padding.left;
          float a2 = (float) padding.right;
          if (this.entries.Count != 0)
          {
            a1 = Mathf.Max(a1, (float) this.entries[0].margin.left);
            a2 = Mathf.Max(a2, (float) this.entries[this.entries.Count - 1].margin.right);
          }
          x += a1;
          width -= a2 + a1;
        }
        float num1 = width - this.spacing * (float) (this.entries.Count - 1);
        float t = 0.0f;
        if ((double) this.m_ChildMinWidth != (double) this.m_ChildMaxWidth)
          t = Mathf.Clamp((float) (((double) num1 - (double) this.m_ChildMinWidth) / ((double) this.m_ChildMaxWidth - (double) this.m_ChildMinWidth)), 0.0f, 1f);
        float num2 = 0.0f;
        if ((double) num1 > (double) this.m_ChildMaxWidth && this.m_StretchableCountX > 0)
          num2 = (num1 - this.m_ChildMaxWidth) / (float) this.m_StretchableCountX;
        int num3 = 0;
        bool flag = true;
        using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GUILayoutEntry current = enumerator.Current;
            float f = Mathf.Lerp(current.minWidth, current.maxWidth, t) + num2 * (float) current.stretchWidth;
            if (current.style != GUILayoutUtility.spaceStyle)
            {
              int num4 = current.margin.left;
              if (flag)
              {
                num4 = 0;
                flag = false;
              }
              int num5 = num3 <= num4 ? num4 : num3;
              x += (float) num5;
              num3 = current.margin.right;
            }
            current.SetHorizontal(Mathf.Round(x), Mathf.Round(f));
            x += f + this.spacing;
          }
        }
      }
    }

    public override void CalcHeight()
    {
      if (this.entries.Count == 0)
      {
        this.maxHeight = this.minHeight = (float) this.style.padding.vertical;
      }
      else
      {
        int b1 = 0;
        int b2 = 0;
        this.m_ChildMinHeight = 0.0f;
        this.m_ChildMaxHeight = 0.0f;
        this.m_StretchableCountY = 0;
        if (this.isVertical)
        {
          int a = 0;
          bool flag = true;
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              current.CalcHeight();
              RectOffset margin = current.margin;
              if (current.style != GUILayoutUtility.spaceStyle)
              {
                int num;
                if (!flag)
                {
                  num = Mathf.Max(a, margin.top);
                }
                else
                {
                  num = 0;
                  flag = false;
                }
                this.m_ChildMinHeight += current.minHeight + this.spacing + (float) num;
                this.m_ChildMaxHeight += current.maxHeight + this.spacing + (float) num;
                a = margin.bottom;
                this.m_StretchableCountY += current.stretchHeight;
              }
              else
              {
                this.m_ChildMinHeight += current.minHeight;
                this.m_ChildMaxHeight += current.maxHeight;
                this.m_StretchableCountY += current.stretchHeight;
              }
            }
          }
          this.m_ChildMinHeight -= this.spacing;
          this.m_ChildMaxHeight -= this.spacing;
          if (this.entries.Count != 0)
          {
            b1 = this.entries[0].margin.top;
            b2 = a;
          }
          else
            b2 = b1 = 0;
        }
        else
        {
          bool flag = true;
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              current.CalcHeight();
              RectOffset margin = current.margin;
              if (current.style != GUILayoutUtility.spaceStyle)
              {
                if (!flag)
                {
                  b1 = Mathf.Min(margin.top, b1);
                  b2 = Mathf.Min(margin.bottom, b2);
                }
                else
                {
                  b1 = margin.top;
                  b2 = margin.bottom;
                  flag = false;
                }
                this.m_ChildMinHeight = Mathf.Max(current.minHeight, this.m_ChildMinHeight);
                this.m_ChildMaxHeight = Mathf.Max(current.maxHeight, this.m_ChildMaxHeight);
              }
              this.m_StretchableCountY += current.stretchHeight;
            }
          }
        }
        float num1;
        float num2;
        if (this.style != GUIStyle.none || this.m_UserSpecifiedHeight)
        {
          num1 = (float) Mathf.Max(this.style.padding.top, b1);
          num2 = (float) Mathf.Max(this.style.padding.bottom, b2);
        }
        else
        {
          this.m_Margin.top = b1;
          this.m_Margin.bottom = b2;
          num1 = num2 = 0.0f;
        }
        this.minHeight = Mathf.Max(this.minHeight, this.m_ChildMinHeight + num1 + num2);
        if ((double) this.maxHeight == 0.0)
        {
          this.stretchHeight += this.m_StretchableCountY + (!this.style.stretchHeight ? 0 : 1);
          this.maxHeight = this.m_ChildMaxHeight + num1 + num2;
        }
        else
          this.stretchHeight = 0;
        this.maxHeight = Mathf.Max(this.maxHeight, this.minHeight);
        if ((double) this.style.fixedHeight == 0.0)
          return;
        this.maxHeight = this.minHeight = this.style.fixedHeight;
        this.stretchHeight = 0;
      }
    }

    public override void SetVertical(float y, float height)
    {
      base.SetVertical(y, height);
      if (this.entries.Count == 0)
        return;
      RectOffset padding = this.style.padding;
      if (this.resetCoords)
        y = 0.0f;
      if (this.isVertical)
      {
        if (this.style != GUIStyle.none)
        {
          float a1 = (float) padding.top;
          float a2 = (float) padding.bottom;
          if (this.entries.Count != 0)
          {
            a1 = Mathf.Max(a1, (float) this.entries[0].margin.top);
            a2 = Mathf.Max(a2, (float) this.entries[this.entries.Count - 1].margin.bottom);
          }
          y += a1;
          height -= a2 + a1;
        }
        float num1 = height - this.spacing * (float) (this.entries.Count - 1);
        float t = 0.0f;
        if ((double) this.m_ChildMinHeight != (double) this.m_ChildMaxHeight)
          t = Mathf.Clamp((float) (((double) num1 - (double) this.m_ChildMinHeight) / ((double) this.m_ChildMaxHeight - (double) this.m_ChildMinHeight)), 0.0f, 1f);
        float num2 = 0.0f;
        if ((double) num1 > (double) this.m_ChildMaxHeight && this.m_StretchableCountY > 0)
          num2 = (num1 - this.m_ChildMaxHeight) / (float) this.m_StretchableCountY;
        int num3 = 0;
        bool flag = true;
        using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GUILayoutEntry current = enumerator.Current;
            float f = Mathf.Lerp(current.minHeight, current.maxHeight, t) + num2 * (float) current.stretchHeight;
            if (current.style != GUILayoutUtility.spaceStyle)
            {
              int num4 = current.margin.top;
              if (flag)
              {
                num4 = 0;
                flag = false;
              }
              int num5 = num3 <= num4 ? num4 : num3;
              y += (float) num5;
              num3 = current.margin.bottom;
            }
            current.SetVertical(Mathf.Round(y), Mathf.Round(f));
            y += f + this.spacing;
          }
        }
      }
      else if (this.style != GUIStyle.none)
      {
        using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GUILayoutEntry current = enumerator.Current;
            float num = (float) Mathf.Max(current.margin.top, padding.top);
            float y1 = y + num;
            float height1 = height - (float) Mathf.Max(current.margin.bottom, padding.bottom) - num;
            if (current.stretchHeight != 0)
              current.SetVertical(y1, height1);
            else
              current.SetVertical(y1, Mathf.Clamp(height1, current.minHeight, current.maxHeight));
          }
        }
      }
      else
      {
        float num1 = y - (float) this.margin.top;
        float num2 = height + (float) this.margin.vertical;
        using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GUILayoutEntry current = enumerator.Current;
            if (current.stretchHeight != 0)
              current.SetVertical(num1 + (float) current.margin.top, num2 - (float) current.margin.vertical);
            else
              current.SetVertical(num1 + (float) current.margin.top, Mathf.Clamp(num2 - (float) current.margin.vertical, current.minHeight, current.maxHeight));
          }
        }
      }
    }

    public override string ToString()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < GUILayoutEntry.indent; ++index)
        empty2 += " ";
      string str1 = empty1 + base.ToString() + " Margins: " + (object) this.m_ChildMinHeight + " {\n";
      GUILayoutEntry.indent += 4;
      using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GUILayoutEntry current = enumerator.Current;
          str1 = str1 + current.ToString() + "\n";
        }
      }
      string str2 = str1 + empty2 + "}";
      GUILayoutEntry.indent -= 4;
      return str2;
    }
  }
}
