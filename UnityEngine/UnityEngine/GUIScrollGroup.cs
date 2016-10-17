// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIScrollGroup
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class GUIScrollGroup : GUILayoutGroup
  {
    public bool allowHorizontalScroll = true;
    public bool allowVerticalScroll = true;
    public float calcMinWidth;
    public float calcMaxWidth;
    public float calcMinHeight;
    public float calcMaxHeight;
    public float clientWidth;
    public float clientHeight;
    public bool needsHorizontalScrollbar;
    public bool needsVerticalScrollbar;
    public GUIStyle horizontalScrollbar;
    public GUIStyle verticalScrollbar;

    [RequiredByNativeCode]
    public GUIScrollGroup()
    {
    }

    public override void CalcWidth()
    {
      float minWidth = this.minWidth;
      float maxWidth = this.maxWidth;
      if (this.allowHorizontalScroll)
      {
        this.minWidth = 0.0f;
        this.maxWidth = 0.0f;
      }
      base.CalcWidth();
      this.calcMinWidth = this.minWidth;
      this.calcMaxWidth = this.maxWidth;
      if (!this.allowHorizontalScroll)
        return;
      if ((double) this.minWidth > 32.0)
        this.minWidth = 32f;
      if ((double) minWidth != 0.0)
        this.minWidth = minWidth;
      if ((double) maxWidth == 0.0)
        return;
      this.maxWidth = maxWidth;
      this.stretchWidth = 0;
    }

    public override void SetHorizontal(float x, float width)
    {
      float width1 = !this.needsVerticalScrollbar ? width : width - this.verticalScrollbar.fixedWidth - (float) this.verticalScrollbar.margin.left;
      if (this.allowHorizontalScroll && (double) width1 < (double) this.calcMinWidth)
      {
        this.needsHorizontalScrollbar = true;
        this.minWidth = this.calcMinWidth;
        this.maxWidth = this.calcMaxWidth;
        base.SetHorizontal(x, this.calcMinWidth);
        this.rect.width = width;
        this.clientWidth = this.calcMinWidth;
      }
      else
      {
        this.needsHorizontalScrollbar = false;
        if (this.allowHorizontalScroll)
        {
          this.minWidth = this.calcMinWidth;
          this.maxWidth = this.calcMaxWidth;
        }
        base.SetHorizontal(x, width1);
        this.rect.width = width;
        this.clientWidth = width1;
      }
    }

    public override void CalcHeight()
    {
      float minHeight = this.minHeight;
      float maxHeight = this.maxHeight;
      if (this.allowVerticalScroll)
      {
        this.minHeight = 0.0f;
        this.maxHeight = 0.0f;
      }
      base.CalcHeight();
      this.calcMinHeight = this.minHeight;
      this.calcMaxHeight = this.maxHeight;
      if (this.needsHorizontalScrollbar)
      {
        float num = this.horizontalScrollbar.fixedHeight + (float) this.horizontalScrollbar.margin.top;
        this.minHeight += (float) (double) num;
        this.maxHeight += (float) (double) num;
      }
      if (!this.allowVerticalScroll)
        return;
      if ((double) this.minHeight > 32.0)
        this.minHeight = 32f;
      if ((double) minHeight != 0.0)
        this.minHeight = minHeight;
      if ((double) maxHeight == 0.0)
        return;
      this.maxHeight = maxHeight;
      this.stretchHeight = 0;
    }

    public override void SetVertical(float y, float height)
    {
      float height1 = height;
      if (this.needsHorizontalScrollbar)
        height1 -= this.horizontalScrollbar.fixedHeight + (float) this.horizontalScrollbar.margin.top;
      if (this.allowVerticalScroll && (double) height1 < (double) this.calcMinHeight)
      {
        if (!this.needsHorizontalScrollbar && !this.needsVerticalScrollbar)
        {
          this.clientWidth = this.rect.width - this.verticalScrollbar.fixedWidth - (float) this.verticalScrollbar.margin.left;
          if ((double) this.clientWidth < (double) this.calcMinWidth)
            this.clientWidth = this.calcMinWidth;
          float width = this.rect.width;
          this.SetHorizontal(this.rect.x, this.clientWidth);
          this.CalcHeight();
          this.rect.width = width;
        }
        float minHeight = this.minHeight;
        float maxHeight = this.maxHeight;
        this.minHeight = this.calcMinHeight;
        this.maxHeight = this.calcMaxHeight;
        base.SetVertical(y, this.calcMinHeight);
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
        this.rect.height = height;
        this.clientHeight = this.calcMinHeight;
      }
      else
      {
        if (this.allowVerticalScroll)
        {
          this.minHeight = this.calcMinHeight;
          this.maxHeight = this.calcMaxHeight;
        }
        base.SetVertical(y, height1);
        this.rect.height = height;
        this.clientHeight = height1;
      }
    }
  }
}
