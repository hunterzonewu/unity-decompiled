// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreePainter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class TreePainter
  {
    public static float brushSize = 40f;
    public static float spacing = 0.8f;
    public static bool lockWidthToHeight = true;
    public static bool randomRotation = true;
    public static bool allowHeightVar = true;
    public static bool allowWidthVar = true;
    public static float treeColorAdjustment = 0.4f;
    public static float treeHeight = 1f;
    public static float treeHeightVariation = 0.1f;
    public static float treeWidth = 1f;
    public static float treeWidthVariation = 0.1f;
    public static int selectedTree = -1;

    private static Color GetTreeColor()
    {
      Color color = Color.white * UnityEngine.Random.Range(1f, 1f - TreePainter.treeColorAdjustment);
      color.a = 1f;
      return color;
    }

    private static float GetTreeHeight()
    {
      float num = !TreePainter.allowHeightVar ? 0.0f : TreePainter.treeHeightVariation;
      return TreePainter.treeHeight * UnityEngine.Random.Range(1f - num, 1f + num);
    }

    private static float GetTreeWidth()
    {
      float num = !TreePainter.allowWidthVar ? 0.0f : TreePainter.treeWidthVariation;
      return TreePainter.treeWidth * UnityEngine.Random.Range(1f - num, 1f + num);
    }

    private static float GetTreeRotation()
    {
      if (TreePainter.randomRotation)
        return UnityEngine.Random.Range(0.0f, 6.283185f);
      return 0.0f;
    }

    public static void PlaceTrees(Terrain terrain, float xBase, float yBase)
    {
      int prototypeCount = TerrainInspectorUtil.GetPrototypeCount(terrain.terrainData);
      if (TreePainter.selectedTree == -1 || TreePainter.selectedTree >= prototypeCount || !TerrainInspectorUtil.PrototypeIsRenderable(terrain.terrainData, TreePainter.selectedTree))
        return;
      int num1 = 0;
      TreeInstance instance = new TreeInstance();
      instance.position = new Vector3(xBase, 0.0f, yBase);
      instance.color = (Color32) TreePainter.GetTreeColor();
      instance.lightmapColor = (Color32) Color.white;
      instance.prototypeIndex = TreePainter.selectedTree;
      instance.heightScale = TreePainter.GetTreeHeight();
      instance.widthScale = !TreePainter.lockWidthToHeight ? TreePainter.GetTreeWidth() : instance.heightScale;
      instance.rotation = TreePainter.GetTreeRotation();
      if (Event.current.type != EventType.MouseDrag && (double) TreePainter.brushSize <= 1.0 || TerrainInspectorUtil.CheckTreeDistance(terrain.terrainData, instance.position, instance.prototypeIndex, TreePainter.spacing))
      {
        terrain.AddTreeInstance(instance);
        ++num1;
      }
      Vector3 prototypeExtent = TerrainInspectorUtil.GetPrototypeExtent(terrain.terrainData, TreePainter.selectedTree);
      prototypeExtent.y = 0.0f;
      float num2 = TreePainter.brushSize / (float) ((double) prototypeExtent.magnitude * (double) TreePainter.spacing * 0.5);
      int num3 = Mathf.Clamp((int) ((double) num2 * (double) num2 * 0.5), 0, 100);
      for (int index = 1; index < num3 && num1 < num3; ++index)
      {
        Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
        insideUnitCircle.x *= TreePainter.brushSize / terrain.terrainData.size.x;
        insideUnitCircle.y *= TreePainter.brushSize / terrain.terrainData.size.z;
        Vector3 position = new Vector3(xBase + insideUnitCircle.x, 0.0f, yBase + insideUnitCircle.y);
        if ((double) position.x >= 0.0 && (double) position.x <= 1.0 && ((double) position.z >= 0.0 && (double) position.z <= 1.0) && TerrainInspectorUtil.CheckTreeDistance(terrain.terrainData, position, TreePainter.selectedTree, TreePainter.spacing * 0.5f))
        {
          instance = new TreeInstance();
          instance.position = position;
          instance.color = (Color32) TreePainter.GetTreeColor();
          instance.lightmapColor = (Color32) Color.white;
          instance.prototypeIndex = TreePainter.selectedTree;
          instance.heightScale = TreePainter.GetTreeHeight();
          instance.widthScale = !TreePainter.lockWidthToHeight ? TreePainter.GetTreeWidth() : instance.heightScale;
          instance.rotation = TreePainter.GetTreeRotation();
          terrain.AddTreeInstance(instance);
          ++num1;
        }
      }
    }

    public static void RemoveTrees(Terrain terrain, float xBase, float yBase, bool clearSelectedOnly)
    {
      float radius = TreePainter.brushSize / terrain.terrainData.size.x;
      terrain.RemoveTrees(new Vector2(xBase, yBase), radius, !clearSelectedOnly ? -1 : TreePainter.selectedTree);
    }

    public static void MassPlaceTrees(TerrainData terrainData, int numberOfTrees, bool randomTreeColor, bool keepExistingTrees)
    {
      int length = terrainData.treePrototypes.Length;
      if (length == 0)
      {
        Debug.Log((object) "Can't place trees because no prototypes are defined");
      }
      else
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) terrainData, "Mass Place Trees");
        TreeInstance[] treeInstanceArray1 = new TreeInstance[numberOfTrees];
        int num = 0;
        while (num < treeInstanceArray1.Length)
        {
          TreeInstance treeInstance = new TreeInstance();
          treeInstance.position = new Vector3(UnityEngine.Random.value, 0.0f, UnityEngine.Random.value);
          if ((double) terrainData.GetSteepness(treeInstance.position.x, treeInstance.position.z) < 30.0)
          {
            treeInstance.color = (Color32) (!randomTreeColor ? Color.white : TreePainter.GetTreeColor());
            treeInstance.lightmapColor = (Color32) Color.white;
            treeInstance.prototypeIndex = UnityEngine.Random.Range(0, length);
            treeInstance.heightScale = TreePainter.GetTreeHeight();
            treeInstance.widthScale = !TreePainter.lockWidthToHeight ? TreePainter.GetTreeWidth() : treeInstance.heightScale;
            treeInstance.rotation = TreePainter.GetTreeRotation();
            treeInstanceArray1[num++] = treeInstance;
          }
        }
        if (keepExistingTrees)
        {
          TreeInstance[] treeInstances = terrainData.treeInstances;
          TreeInstance[] treeInstanceArray2 = new TreeInstance[treeInstances.Length + treeInstanceArray1.Length];
          Array.Copy((Array) treeInstances, 0, (Array) treeInstanceArray2, 0, treeInstances.Length);
          Array.Copy((Array) treeInstanceArray1, 0, (Array) treeInstanceArray2, treeInstances.Length, treeInstanceArray1.Length);
          treeInstanceArray1 = treeInstanceArray2;
        }
        terrainData.treeInstances = treeInstanceArray1;
        terrainData.RecalculateTreePositions();
      }
    }
  }
}
