// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ChartData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditorInternal
{
  internal class ChartData
  {
    public ChartSeries[] charts;
    public int[] chartOrder;
    public float[] scale;
    public float[] grid;
    public string[] gridLabels;
    public string[] selectedLabels;
    public int firstFrame;
    public int firstSelectableFrame;
    public bool hasOverlay;
    public float maxValue;

    public int NumberOfFrames
    {
      get
      {
        return this.charts[0].data.Length;
      }
    }

    public void Assign(ChartSeries[] items, int firstFrame, int firstSelectableFrame)
    {
      this.charts = items;
      this.firstFrame = firstFrame;
      this.firstSelectableFrame = firstSelectableFrame;
      if (this.chartOrder != null && this.chartOrder.Length == items.Length)
        return;
      this.chartOrder = new int[items.Length];
      for (int index = 0; index < this.chartOrder.Length; ++index)
        this.chartOrder[index] = this.chartOrder.Length - 1 - index;
    }

    public void AssignScale(float[] scale)
    {
      this.scale = scale;
    }

    public void SetGrid(float[] grid, string[] labels)
    {
      this.grid = grid;
      this.gridLabels = labels;
    }
  }
}
