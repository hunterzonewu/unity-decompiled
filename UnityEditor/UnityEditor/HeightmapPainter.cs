// Decompiled with JetBrains decompiler
// Type: UnityEditor.HeightmapPainter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class HeightmapPainter
  {
    public int size;
    public float strength;
    public float targetHeight;
    public TerrainTool tool;
    public Brush brush;
    public TerrainData terrainData;

    private float Smooth(int x, int y)
    {
      float num1 = 0.0f;
      float num2 = 1f / this.terrainData.size.y;
      return (num1 + this.terrainData.GetHeight(x, y) * num2 + this.terrainData.GetHeight(x + 1, y) * num2 + this.terrainData.GetHeight(x - 1, y) * num2 + (float) ((double) this.terrainData.GetHeight(x + 1, y + 1) * (double) num2 * 0.75) + (float) ((double) this.terrainData.GetHeight(x - 1, y + 1) * (double) num2 * 0.75) + (float) ((double) this.terrainData.GetHeight(x + 1, y - 1) * (double) num2 * 0.75) + (float) ((double) this.terrainData.GetHeight(x - 1, y - 1) * (double) num2 * 0.75) + this.terrainData.GetHeight(x, y + 1) * num2 + this.terrainData.GetHeight(x, y - 1) * num2) / 8f;
    }

    private float ApplyBrush(float height, float brushStrength, int x, int y)
    {
      if (this.tool == TerrainTool.PaintHeight)
        return height + brushStrength;
      if (this.tool == TerrainTool.SetHeight)
      {
        if ((double) this.targetHeight > (double) height)
        {
          height += brushStrength;
          height = Mathf.Min(height, this.targetHeight);
          return height;
        }
        height -= brushStrength;
        height = Mathf.Max(height, this.targetHeight);
        return height;
      }
      if (this.tool == TerrainTool.SmoothHeight)
        return Mathf.Lerp(height, this.Smooth(x, y), brushStrength);
      return height;
    }

    public void PaintHeight(float xCenterNormalized, float yCenterNormalized)
    {
      int num1;
      int num2;
      if (this.size % 2 == 0)
      {
        num1 = Mathf.CeilToInt(xCenterNormalized * (float) (this.terrainData.heightmapWidth - 1));
        num2 = Mathf.CeilToInt(yCenterNormalized * (float) (this.terrainData.heightmapHeight - 1));
      }
      else
      {
        num1 = Mathf.RoundToInt(xCenterNormalized * (float) (this.terrainData.heightmapWidth - 1));
        num2 = Mathf.RoundToInt(yCenterNormalized * (float) (this.terrainData.heightmapHeight - 1));
      }
      int num3 = this.size / 2;
      int num4 = this.size % 2;
      int xBase = Mathf.Clamp(num1 - num3, 0, this.terrainData.heightmapWidth - 1);
      int yBase = Mathf.Clamp(num2 - num3, 0, this.terrainData.heightmapHeight - 1);
      int num5 = Mathf.Clamp(num1 + num3 + num4, 0, this.terrainData.heightmapWidth);
      int num6 = Mathf.Clamp(num2 + num3 + num4, 0, this.terrainData.heightmapHeight);
      int width = num5 - xBase;
      int height = num6 - yBase;
      float[,] heights = this.terrainData.GetHeights(xBase, yBase, width, height);
      for (int index1 = 0; index1 < height; ++index1)
      {
        for (int index2 = 0; index2 < width; ++index2)
        {
          float strengthInt = this.brush.GetStrengthInt(xBase + index2 - (num1 - num3), yBase + index1 - (num2 - num3));
          float num7 = this.ApplyBrush(heights[index1, index2], strengthInt * this.strength, index2 + xBase, index1 + yBase);
          heights[index1, index2] = num7;
        }
      }
      this.terrainData.SetHeightsDelayLOD(xBase, yBase, heights);
    }
  }
}
