// Decompiled with JetBrains decompiler
// Type: UnityEditor.SplatPainter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SplatPainter
  {
    public int size;
    public float strength;
    public Brush brush;
    public float target;
    public TerrainData terrainData;
    public TerrainTool tool;

    private float ApplyBrush(float height, float brushStrength)
    {
      if ((double) this.target > (double) height)
      {
        height += brushStrength;
        height = Mathf.Min(height, this.target);
        return height;
      }
      height -= brushStrength;
      height = Mathf.Max(height, this.target);
      return height;
    }

    private void Normalize(int x, int y, int splatIndex, float[,,] alphamap)
    {
      float num1 = alphamap[y, x, splatIndex];
      float num2 = 0.0f;
      int length = alphamap.GetLength(2);
      for (int index = 0; index < length; ++index)
      {
        if (index != splatIndex)
          num2 += alphamap[y, x, index];
      }
      if ((double) num2 > 0.01)
      {
        float num3 = (1f - num1) / num2;
        for (int index = 0; index < length; ++index)
        {
          if (index != splatIndex)
            alphamap[y, x, index] *= num3;
        }
      }
      else
      {
        for (int index = 0; index < length; ++index)
          alphamap[y, x, index] = index != splatIndex ? 0.0f : 1f;
      }
    }

    public void Paint(float xCenterNormalized, float yCenterNormalized, int splatIndex)
    {
      if (splatIndex >= this.terrainData.alphamapLayers)
        return;
      int num1 = Mathf.FloorToInt(xCenterNormalized * (float) this.terrainData.alphamapWidth);
      int num2 = Mathf.FloorToInt(yCenterNormalized * (float) this.terrainData.alphamapHeight);
      int num3 = Mathf.RoundToInt((float) this.size) / 2;
      int num4 = Mathf.RoundToInt((float) this.size) % 2;
      int x1 = Mathf.Clamp(num1 - num3, 0, this.terrainData.alphamapWidth - 1);
      int y1 = Mathf.Clamp(num2 - num3, 0, this.terrainData.alphamapHeight - 1);
      int num5 = Mathf.Clamp(num1 + num3 + num4, 0, this.terrainData.alphamapWidth);
      int num6 = Mathf.Clamp(num2 + num3 + num4, 0, this.terrainData.alphamapHeight);
      int width = num5 - x1;
      int height = num6 - y1;
      float[,,] alphamaps = this.terrainData.GetAlphamaps(x1, y1, width, height);
      for (int y2 = 0; y2 < height; ++y2)
      {
        for (int x2 = 0; x2 < width; ++x2)
        {
          float strengthInt = this.brush.GetStrengthInt(x1 + x2 - (num1 - num3 + num4), y1 + y2 - (num2 - num3 + num4));
          float num7 = this.ApplyBrush(alphamaps[y2, x2, splatIndex], strengthInt * this.strength);
          alphamaps[y2, x2, splatIndex] = num7;
          this.Normalize(x2, y2, splatIndex, alphamaps);
        }
      }
      this.terrainData.SetAlphamaps(x1, y1, alphamaps);
    }
  }
}
