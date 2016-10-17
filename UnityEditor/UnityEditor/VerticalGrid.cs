// Decompiled with JetBrains decompiler
// Type: UnityEditor.VerticalGrid
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class VerticalGrid
  {
    private int m_Columns = 1;
    private int m_Rows;
    private float m_Height;
    private float m_HorizontalSpacing;

    public int columns
    {
      get
      {
        return this.m_Columns;
      }
    }

    public int rows
    {
      get
      {
        return this.m_Rows;
      }
    }

    public float height
    {
      get
      {
        return this.m_Height;
      }
    }

    public float horizontalSpacing
    {
      get
      {
        return this.m_HorizontalSpacing;
      }
    }

    public float fixedWidth { get; set; }

    public Vector2 itemSize { get; set; }

    public float verticalSpacing { get; set; }

    public float minHorizontalSpacing { get; set; }

    public float topMargin { get; set; }

    public float bottomMargin { get; set; }

    public float rightMargin { get; set; }

    public float leftMargin { get; set; }

    public float fixedHorizontalSpacing { get; set; }

    public bool useFixedHorizontalSpacing { get; set; }

    public void InitNumRowsAndColumns(int itemCount, int maxNumRows)
    {
      if (this.useFixedHorizontalSpacing)
      {
        this.m_Columns = this.CalcColumns();
        this.m_HorizontalSpacing = this.fixedHorizontalSpacing;
        this.m_Rows = Mathf.Min(maxNumRows, this.CalcRows(itemCount));
        this.m_Height = (float) this.m_Rows * (this.itemSize.y + this.verticalSpacing) - this.verticalSpacing + this.topMargin + this.bottomMargin;
      }
      else
      {
        this.m_Columns = this.CalcColumns();
        this.m_HorizontalSpacing = Mathf.Max(0.0f, (this.fixedWidth - ((float) this.m_Columns * this.itemSize.x + this.leftMargin + this.rightMargin)) / (float) this.m_Columns);
        this.m_Rows = Mathf.Min(maxNumRows, this.CalcRows(itemCount));
        if (this.m_Rows == 1)
          this.m_HorizontalSpacing = this.minHorizontalSpacing;
        this.m_Height = (float) this.m_Rows * (this.itemSize.y + this.verticalSpacing) - this.verticalSpacing + this.topMargin + this.bottomMargin;
      }
    }

    public int CalcColumns()
    {
      return Mathf.Max((int) Mathf.Floor((float) (((double) this.fixedWidth - (double) this.leftMargin - (double) this.rightMargin) / ((double) this.itemSize.x + (!this.useFixedHorizontalSpacing ? (double) this.minHorizontalSpacing : (double) this.fixedHorizontalSpacing)))), 1);
    }

    public int CalcRows(int itemCount)
    {
      int num = (int) Mathf.Ceil((float) itemCount / (float) this.CalcColumns());
      if (num < 0)
        return int.MaxValue;
      return num;
    }

    public Rect CalcRect(int itemIdx, float yOffset)
    {
      float num1 = Mathf.Floor((float) (itemIdx / this.columns));
      float num2 = (float) itemIdx - num1 * (float) this.columns;
      if (this.useFixedHorizontalSpacing)
        return new Rect(this.leftMargin + num2 * (this.itemSize.x + this.fixedHorizontalSpacing), num1 * (this.itemSize.y + this.verticalSpacing) + this.topMargin + yOffset, this.itemSize.x, this.itemSize.y);
      return new Rect((float) ((double) this.leftMargin + (double) this.horizontalSpacing * 0.5 + (double) num2 * ((double) this.itemSize.x + (double) this.horizontalSpacing)), num1 * (this.itemSize.y + this.verticalSpacing) + this.topMargin + yOffset, this.itemSize.x, this.itemSize.y);
    }

    public int GetMaxVisibleItems(float height)
    {
      return (int) Mathf.Ceil((float) (((double) height - (double) this.topMargin - (double) this.bottomMargin) / ((double) this.itemSize.y + (double) this.verticalSpacing))) * this.columns;
    }

    public bool IsVisibleInScrollView(float scrollViewHeight, float scrollPos, float gridStartY, int maxIndex, out int startIndex, out int endIndex)
    {
      startIndex = endIndex = 0;
      float num1 = scrollPos;
      float num2 = scrollPos + scrollViewHeight;
      float num3 = gridStartY + this.topMargin;
      if ((double) num3 > (double) num2 || (double) num3 + (double) this.height < (double) num1)
        return false;
      float num4 = this.itemSize.y + this.verticalSpacing;
      int num5 = Mathf.FloorToInt((num1 - num3) / num4);
      startIndex = num5 * this.columns;
      startIndex = Mathf.Clamp(startIndex, 0, maxIndex);
      int num6 = Mathf.FloorToInt((num2 - num3) / num4);
      endIndex = (num6 + 1) * this.columns - 1;
      endIndex = Mathf.Clamp(endIndex, 0, maxIndex);
      return true;
    }

    public override string ToString()
    {
      return string.Format("VerticalGrid: rows {0}, columns {1}, fixedWidth {2}, itemSize {3}", (object) this.rows, (object) this.columns, (object) this.fixedWidth, (object) this.itemSize);
    }
  }
}
