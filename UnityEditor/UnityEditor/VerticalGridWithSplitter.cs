// Decompiled with JetBrains decompiler
// Type: UnityEditor.VerticalGridWithSplitter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class VerticalGridWithSplitter
  {
    private int m_Columns = 1;
    private int m_Rows;
    private float m_Height;
    private float m_HorizontalSpacing;
    private int m_SplitAfterRow;
    private float m_CurrentSplitHeight;
    private float m_LastSplitUpdate;
    private float m_TargetSplitHeight;

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

    public void InitNumRowsAndColumns(int itemCount, int maxNumRows)
    {
      this.m_Columns = (int) Mathf.Floor((float) (((double) this.fixedWidth - (double) this.leftMargin - (double) this.rightMargin) / ((double) this.itemSize.x + (double) this.minHorizontalSpacing)));
      this.m_Columns = Mathf.Max(this.m_Columns, 1);
      this.m_HorizontalSpacing = 0.0f;
      if (this.m_Columns > 1)
        this.m_HorizontalSpacing = (this.fixedWidth - ((float) this.m_Columns * this.itemSize.x + this.leftMargin + this.rightMargin)) / (float) (this.m_Columns - 1);
      this.m_Rows = Mathf.Min(maxNumRows, (int) Mathf.Ceil((float) itemCount / (float) this.m_Columns));
      this.m_Height = (float) this.m_Rows * (this.itemSize.y + this.verticalSpacing) - this.verticalSpacing + this.topMargin + this.bottomMargin;
    }

    public Rect CalcRect(int itemIdx, float yOffset)
    {
      float num = Mathf.Floor((float) (itemIdx / this.columns));
      return new Rect(((float) itemIdx - num * (float) this.columns) * (this.itemSize.x + this.horizontalSpacing) + this.leftMargin, num * (this.itemSize.y + this.verticalSpacing) + this.topMargin + yOffset, this.itemSize.x, this.itemSize.y);
    }

    public int GetMaxVisibleItems(float height)
    {
      return (int) Mathf.Ceil((float) (((double) height - (double) this.topMargin - (double) this.bottomMargin) / ((double) this.itemSize.y + (double) this.verticalSpacing))) * this.columns;
    }

    public void ResetSplit()
    {
      this.m_SplitAfterRow = -1;
      this.m_CurrentSplitHeight = 0.0f;
      this.m_LastSplitUpdate = -1f;
      this.m_TargetSplitHeight = 0.0f;
    }

    public void OpenSplit(int splitAfterRowIndex, int numItems)
    {
      float num = (float) (int) Mathf.Ceil((float) numItems / (float) this.m_Columns) * (this.itemSize.y + this.verticalSpacing) - this.verticalSpacing + this.topMargin + this.bottomMargin;
      this.m_SplitAfterRow = splitAfterRowIndex;
      this.m_TargetSplitHeight = num;
      this.m_LastSplitUpdate = Time.realtimeSinceStartup;
    }

    public Rect CalcSplitRect(int splitIndex, float yOffset)
    {
      return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    }

    public void CloseSplit()
    {
      this.m_TargetSplitHeight = 0.0f;
    }

    public bool UpdateSplitAnimationOnGUI()
    {
      if (this.m_SplitAfterRow != -1)
      {
        this.m_CurrentSplitHeight = (Time.realtimeSinceStartup - this.m_LastSplitUpdate) * this.m_TargetSplitHeight;
        this.m_LastSplitUpdate = Time.realtimeSinceStartup;
        if ((double) this.m_CurrentSplitHeight != (double) this.m_TargetSplitHeight && Event.current.type == EventType.Repaint)
        {
          this.m_CurrentSplitHeight = Mathf.MoveTowards(this.m_CurrentSplitHeight, this.m_TargetSplitHeight, 0.03f);
          if ((double) this.m_CurrentSplitHeight == 0.0 && (double) this.m_TargetSplitHeight == 0.0)
            this.ResetSplit();
          return true;
        }
      }
      return false;
    }
  }
}
