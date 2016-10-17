// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TerrainEditorUtility
  {
    internal static void RemoveSplatTexture(TerrainData terrainData, int index)
    {
      Undo.RegisterCompleteObjectUndo((Object) terrainData, "Remove texture");
      int alphamapWidth = terrainData.alphamapWidth;
      int alphamapHeight = terrainData.alphamapHeight;
      float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
      int length1 = alphamaps.GetLength(2);
      int length2 = length1 - 1;
      float[,,] map = new float[alphamapHeight, alphamapWidth, length2];
      for (int index1 = 0; index1 < alphamapHeight; ++index1)
      {
        for (int index2 = 0; index2 < alphamapWidth; ++index2)
        {
          for (int index3 = 0; index3 < index; ++index3)
            map[index1, index2, index3] = alphamaps[index1, index2, index3];
          for (int index3 = index + 1; index3 < length1; ++index3)
            map[index1, index2, index3 - 1] = alphamaps[index1, index2, index3];
        }
      }
      for (int index1 = 0; index1 < alphamapHeight; ++index1)
      {
        for (int index2 = 0; index2 < alphamapWidth; ++index2)
        {
          float num1 = 0.0f;
          for (int index3 = 0; index3 < length2; ++index3)
            num1 += map[index1, index2, index3];
          if ((double) num1 >= 0.01)
          {
            float num2 = 1f / num1;
            for (int index3 = 0; index3 < length2; ++index3)
              map[index1, index2, index3] *= num2;
          }
          else
          {
            for (int index3 = 0; index3 < length2; ++index3)
              map[index1, index2, index3] = index3 != 0 ? 0.0f : 1f;
          }
        }
      }
      SplatPrototype[] splatPrototypes = terrainData.splatPrototypes;
      SplatPrototype[] splatPrototypeArray = new SplatPrototype[splatPrototypes.Length - 1];
      for (int index1 = 0; index1 < index; ++index1)
        splatPrototypeArray[index1] = splatPrototypes[index1];
      for (int index1 = index + 1; index1 < length1; ++index1)
        splatPrototypeArray[index1 - 1] = splatPrototypes[index1];
      terrainData.splatPrototypes = splatPrototypeArray;
      terrainData.SetAlphamaps(0, 0, map);
    }

    internal static void RemoveTree(Terrain terrain, int index)
    {
      TerrainData terrainData = terrain.terrainData;
      if ((Object) terrainData == (Object) null)
        return;
      Undo.RegisterCompleteObjectUndo((Object) terrainData, "Remove tree");
      terrainData.RemoveTreePrototype(index);
    }

    internal static void RemoveDetail(Terrain terrain, int index)
    {
      TerrainData terrainData = terrain.terrainData;
      if ((Object) terrainData == (Object) null)
        return;
      Undo.RegisterCompleteObjectUndo((Object) terrainData, "Remove detail object");
      terrainData.RemoveDetailPrototype(index);
    }

    internal static bool IsLODTreePrototype(GameObject prefab)
    {
      if ((Object) prefab != (Object) null)
        return (Object) prefab.GetComponent<LODGroup>() != (Object) null;
      return false;
    }
  }
}
