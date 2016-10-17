// Decompiled with JetBrains decompiler
// Type: UnityEditor.HeightmapFilters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class HeightmapFilters
  {
    private static void WobbleStuff(float[,] heights, TerrainData terrain)
    {
      for (int index1 = 0; index1 < heights.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < heights.GetLength(1); ++index2)
          heights[index1, index2] = (float) (((double) heights[index1, index2] + 1.0) / 2.0);
      }
    }

    private static void Noise(float[,] heights, TerrainData terrain)
    {
      for (int index1 = 0; index1 < heights.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < heights.GetLength(1); ++index2)
          heights[index1, index2] += Random.value * 0.01f;
      }
    }

    public static void Smooth(float[,] heights, TerrainData terrain)
    {
      float[,] numArray = heights.Clone() as float[,];
      int length1 = heights.GetLength(1);
      int length2 = heights.GetLength(0);
      for (int index1 = 1; index1 < length2 - 1; ++index1)
      {
        for (int index2 = 1; index2 < length1 - 1; ++index2)
        {
          float num = (0.0f + numArray[index1, index2] + numArray[index1, index2 - 1] + numArray[index1, index2 + 1] + numArray[index1 - 1, index2] + numArray[index1 + 1, index2]) / 5f;
          heights[index1, index2] = num;
        }
      }
    }

    public static void Smooth(TerrainData terrain)
    {
      int heightmapWidth = terrain.heightmapWidth;
      int heightmapHeight = terrain.heightmapHeight;
      float[,] heights = terrain.GetHeights(0, 0, heightmapWidth, heightmapHeight);
      HeightmapFilters.Smooth(heights, terrain);
      terrain.SetHeights(0, 0, heights);
    }

    public static void Flatten(TerrainData terrain, float height)
    {
      int heightmapWidth = terrain.heightmapWidth;
      float[,] heights = new float[terrain.heightmapHeight, heightmapWidth];
      for (int index1 = 0; index1 < heights.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < heights.GetLength(1); ++index2)
          heights[index1, index2] = height;
      }
      terrain.SetHeights(0, 0, heights);
    }
  }
}
