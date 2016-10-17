// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeAOImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TreeAOImporter : AssetPostprocessor
  {
    private void OnPostprocessModel(GameObject root)
    {
      if (this.assetPath.ToLower().IndexOf("ambient-occlusion") == -1)
        return;
      foreach (MeshFilter componentsInChild in root.GetComponentsInChildren(typeof (MeshFilter)))
      {
        if ((Object) componentsInChild.sharedMesh != (Object) null)
        {
          Mesh sharedMesh = componentsInChild.sharedMesh;
          TreeAO.CalcSoftOcclusion(sharedMesh);
          Bounds bounds = sharedMesh.bounds;
          Color[] colorArray = sharedMesh.colors;
          Vector3[] vertices = sharedMesh.vertices;
          Vector4[] tangents = sharedMesh.tangents;
          if (colorArray.Length == 0)
          {
            colorArray = new Color[sharedMesh.vertexCount];
            for (int index = 0; index < colorArray.Length; ++index)
              colorArray[index] = Color.white;
          }
          float b1 = 0.0f;
          for (int index = 0; index < tangents.Length; ++index)
            b1 = Mathf.Max(tangents[index].w, b1);
          float b2 = 0.0f;
          for (int index = 0; index < colorArray.Length; ++index)
            b2 = Mathf.Max(new Vector2(vertices[index].x, vertices[index].z).magnitude, b2);
          for (int index = 0; index < colorArray.Length; ++index)
          {
            float num1 = new Vector2(vertices[index].x, vertices[index].z).magnitude / b2;
            float num2 = (vertices[index].y - bounds.min.y) / bounds.size.y;
            colorArray[index].a = (float) ((double) num2 * (double) num1 * 0.600000023841858 + (double) num2 * 0.5);
          }
          sharedMesh.colors = colorArray;
        }
      }
    }
  }
}
