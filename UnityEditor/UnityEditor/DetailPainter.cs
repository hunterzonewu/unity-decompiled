// Decompiled with JetBrains decompiler
// Type: UnityEditor.DetailPainter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class DetailPainter
  {
    public int size;
    public float opacity;
    public float targetStrength;
    public Brush brush;
    public TerrainData terrainData;
    public TerrainTool tool;
    public bool randomizeDetails;
    public bool clearSelectedOnly;

    public void Paint(float xCenterNormalized, float yCenterNormalized, int detailIndex)
    {
      if (detailIndex >= this.terrainData.detailPrototypes.Length)
        return;
      int num1 = Mathf.FloorToInt(xCenterNormalized * (float) this.terrainData.detailWidth);
      int num2 = Mathf.FloorToInt(yCenterNormalized * (float) this.terrainData.detailHeight);
      int num3 = Mathf.RoundToInt((float) this.size) / 2;
      int num4 = Mathf.RoundToInt((float) this.size) % 2;
      int xBase = Mathf.Clamp(num1 - num3, 0, this.terrainData.detailWidth - 1);
      int yBase = Mathf.Clamp(num2 - num3, 0, this.terrainData.detailHeight - 1);
      int num5 = Mathf.Clamp(num1 + num3 + num4, 0, this.terrainData.detailWidth);
      int num6 = Mathf.Clamp(num2 + num3 + num4, 0, this.terrainData.detailHeight);
      int num7 = num5 - xBase;
      int num8 = num6 - yBase;
      int[] numArray = new int[1]{ detailIndex };
      if ((double) this.targetStrength < 0.0 && !this.clearSelectedOnly)
        numArray = this.terrainData.GetSupportedLayers(xBase, yBase, num7, num8);
      for (int index1 = 0; index1 < numArray.Length; ++index1)
      {
        int[,] detailLayer = this.terrainData.GetDetailLayer(xBase, yBase, num7, num8, numArray[index1]);
        for (int index2 = 0; index2 < num8; ++index2)
        {
          for (int index3 = 0; index3 < num7; ++index3)
          {
            float t = this.opacity * this.brush.GetStrengthInt(xBase + index3 - (num1 - num3 + num4), yBase + index2 - (num2 - num3 + num4));
            float targetStrength = this.targetStrength;
            float num9 = Mathf.Lerp((float) detailLayer[index2, index3], targetStrength, t);
            detailLayer[index2, index3] = Mathf.RoundToInt(num9 - 0.5f + Random.value);
          }
        }
        this.terrainData.SetDetailLayer(xBase, yBase, numArray[index1], detailLayer);
      }
    }
  }
}
