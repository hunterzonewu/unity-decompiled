// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeAO
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TreeAO
  {
    private const int kWorkLayer = 29;
    private const float occlusion = 0.5f;
    private static bool kDebug;
    private static Vector3[] directions;

    private static int PermuteCuboid(Vector3[] dirs, int offset, float x, float y, float z)
    {
      dirs[offset] = new Vector3(x, y, z);
      dirs[offset + 1] = new Vector3(x, y, -z);
      dirs[offset + 2] = new Vector3(x, -y, z);
      dirs[offset + 3] = new Vector3(x, -y, -z);
      dirs[offset + 4] = new Vector3(-x, y, z);
      dirs[offset + 5] = new Vector3(-x, y, -z);
      dirs[offset + 6] = new Vector3(-x, -y, z);
      dirs[offset + 7] = new Vector3(-x, -y, -z);
      return offset + 8;
    }

    public static void InitializeDirections()
    {
      float num = (float) ((1.0 + (double) Mathf.Sqrt(5f)) / 2.0);
      TreeAO.directions = new Vector3[60];
      TreeAO.directions[0] = new Vector3(0.0f, 1f, 3f * num);
      TreeAO.directions[1] = new Vector3(0.0f, 1f, -3f * num);
      TreeAO.directions[2] = new Vector3(0.0f, -1f, 3f * num);
      TreeAO.directions[3] = new Vector3(0.0f, -1f, -3f * num);
      TreeAO.directions[4] = new Vector3(1f, 3f * num, 0.0f);
      TreeAO.directions[5] = new Vector3(1f, -3f * num, 0.0f);
      TreeAO.directions[6] = new Vector3(-1f, 3f * num, 0.0f);
      TreeAO.directions[7] = new Vector3(-1f, -3f * num, 0.0f);
      TreeAO.directions[8] = new Vector3(3f * num, 0.0f, 1f);
      TreeAO.directions[9] = new Vector3(3f * num, 0.0f, -1f);
      TreeAO.directions[10] = new Vector3(-3f * num, 0.0f, 1f);
      TreeAO.directions[11] = new Vector3(-3f * num, 0.0f, -1f);
      int offset1 = 12;
      int offset2 = TreeAO.PermuteCuboid(TreeAO.directions, offset1, 2f, (float) (1.0 + 2.0 * (double) num), num);
      int offset3 = TreeAO.PermuteCuboid(TreeAO.directions, offset2, (float) (1.0 + 2.0 * (double) num), num, 2f);
      int offset4 = TreeAO.PermuteCuboid(TreeAO.directions, offset3, num, 2f, (float) (1.0 + 2.0 * (double) num));
      int offset5 = TreeAO.PermuteCuboid(TreeAO.directions, offset4, 1f, 2f + num, 2f * num);
      int offset6 = TreeAO.PermuteCuboid(TreeAO.directions, offset5, 2f + num, 2f * num, 1f);
      TreeAO.PermuteCuboid(TreeAO.directions, offset6, 2f * num, 1f, 2f + num);
      for (int index = 0; index < TreeAO.directions.Length; ++index)
        TreeAO.directions[index] = TreeAO.directions[index].normalized;
    }

    public static void CalcSoftOcclusion(Mesh mesh)
    {
      GameObject gameObject = new GameObject("Test");
      gameObject.layer = 29;
      gameObject.AddComponent<MeshFilter>().mesh = mesh;
      gameObject.AddComponent<MeshCollider>();
      if (TreeAO.directions == null)
        TreeAO.InitializeDirections();
      Vector4[] vector4Array1 = new Vector4[TreeAO.directions.Length];
      for (int index = 0; index < TreeAO.directions.Length; ++index)
        vector4Array1[index] = new Vector4(TreeAO.GetWeight(1, TreeAO.directions[index]), TreeAO.GetWeight(2, TreeAO.directions[index]), TreeAO.GetWeight(3, TreeAO.directions[index]), TreeAO.GetWeight(0, TreeAO.directions[index]));
      Vector3[] vertices = mesh.vertices;
      Vector4[] vector4Array2 = new Vector4[vertices.Length];
      float num1 = 0.0f;
      for (int index1 = 0; index1 < vertices.Length; ++index1)
      {
        Vector4 zero = Vector4.zero;
        Vector3 v = gameObject.transform.TransformPoint(vertices[index1]);
        for (int index2 = 0; index2 < TreeAO.directions.Length; ++index2)
        {
          float num2 = Mathf.Pow(0.5f, (float) TreeAO.CountIntersections(v, gameObject.transform.TransformDirection(TreeAO.directions[index2]), 3f));
          zero += vector4Array1[index2] * num2;
        }
        zero /= (float) TreeAO.directions.Length;
        num1 += zero.w;
        vector4Array2[index1] = zero;
      }
      float num3 = num1 / (float) vertices.Length;
      for (int index = 0; index < vertices.Length; ++index)
        vector4Array2[index].w -= num3;
      mesh.tangents = vector4Array2;
      Object.DestroyImmediate((Object) gameObject);
    }

    private static int CountIntersections(Vector3 v, Vector3 dist, float length)
    {
      v += dist * 0.01f;
      if (!TreeAO.kDebug)
        return Physics.RaycastAll(v, dist, length, 536870912).Length + Physics.RaycastAll(v + dist * length, -dist, length, 536870912).Length;
      RaycastHit[] raycastHitArray1 = Physics.RaycastAll(v, dist, length, 536870912);
      int length1 = raycastHitArray1.Length;
      float num = 0.0f;
      if (length1 > 0)
        num = raycastHitArray1[raycastHitArray1.Length - 1].distance;
      RaycastHit[] raycastHitArray2 = Physics.RaycastAll(v + dist * length, -dist, length, 536870912);
      if (raycastHitArray2.Length > 0)
      {
        if ((double) (length - raycastHitArray2[0].distance) > (double) num)
          ;
      }
      return length1 + raycastHitArray2.Length;
    }

    private static float GetWeight(int coeff, Vector3 dir)
    {
      switch (coeff)
      {
        case 0:
          return 0.5f;
        case 1:
          return 0.5f * dir.x;
        case 2:
          return 0.5f * dir.y;
        case 3:
          return 0.5f * dir.z;
        default:
          Debug.Log((object) "Only defined up to 3");
          return 0.0f;
      }
    }
  }
}
