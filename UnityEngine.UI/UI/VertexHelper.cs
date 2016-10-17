// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.VertexHelper
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A utility class that can aid in the generation of meshes for the UI.</para>
  /// </summary>
  public class VertexHelper : IDisposable
  {
    private static readonly Vector4 s_DefaultTangent = new Vector4(1f, 0.0f, 0.0f, -1f);
    private static readonly Vector3 s_DefaultNormal = Vector3.back;
    private List<Vector3> m_Positions = ListPool<Vector3>.Get();
    private List<Color32> m_Colors = ListPool<Color32>.Get();
    private List<Vector2> m_Uv0S = ListPool<Vector2>.Get();
    private List<Vector2> m_Uv1S = ListPool<Vector2>.Get();
    private List<Vector3> m_Normals = ListPool<Vector3>.Get();
    private List<Vector4> m_Tangents = ListPool<Vector4>.Get();
    private List<int> m_Indices = ListPool<int>.Get();

    /// <summary>
    ///   <para>Current number of vertices in the buffer.</para>
    /// </summary>
    public int currentVertCount
    {
      get
      {
        return this.m_Positions.Count;
      }
    }

    /// <summary>
    ///   <para>Get the number of indices set on the VertexHelper.</para>
    /// </summary>
    public int currentIndexCount
    {
      get
      {
        return this.m_Indices.Count;
      }
    }

    public VertexHelper()
    {
    }

    public VertexHelper(Mesh m)
    {
      this.m_Positions.AddRange((IEnumerable<Vector3>) m.vertices);
      this.m_Colors.AddRange((IEnumerable<Color32>) m.colors32);
      this.m_Uv0S.AddRange((IEnumerable<Vector2>) m.uv);
      this.m_Uv1S.AddRange((IEnumerable<Vector2>) m.uv2);
      this.m_Normals.AddRange((IEnumerable<Vector3>) m.normals);
      this.m_Tangents.AddRange((IEnumerable<Vector4>) m.tangents);
      this.m_Indices.AddRange((IEnumerable<int>) m.GetIndices(0));
    }

    /// <summary>
    ///   <para>Clear all vertices from the stream.</para>
    /// </summary>
    public void Clear()
    {
      this.m_Positions.Clear();
      this.m_Colors.Clear();
      this.m_Uv0S.Clear();
      this.m_Uv1S.Clear();
      this.m_Normals.Clear();
      this.m_Tangents.Clear();
      this.m_Indices.Clear();
    }

    public void PopulateUIVertex(ref UIVertex vertex, int i)
    {
      vertex.position = this.m_Positions[i];
      vertex.color = this.m_Colors[i];
      vertex.uv0 = this.m_Uv0S[i];
      vertex.uv1 = this.m_Uv1S[i];
      vertex.normal = this.m_Normals[i];
      vertex.tangent = this.m_Tangents[i];
    }

    /// <summary>
    ///   <para>Set a UIVertex at the given index.</para>
    /// </summary>
    /// <param name="vertex"></param>
    /// <param name="i"></param>
    public void SetUIVertex(UIVertex vertex, int i)
    {
      this.m_Positions[i] = vertex.position;
      this.m_Colors[i] = vertex.color;
      this.m_Uv0S[i] = vertex.uv0;
      this.m_Uv1S[i] = vertex.uv1;
      this.m_Normals[i] = vertex.normal;
      this.m_Tangents[i] = vertex.tangent;
    }

    /// <summary>
    ///   <para>Fill the given mesh with the stream data.</para>
    /// </summary>
    /// <param name="mesh"></param>
    public void FillMesh(Mesh mesh)
    {
      mesh.Clear();
      if (this.m_Positions.Count >= 65000)
        throw new ArgumentException("Mesh can not have more than 65000 vertices");
      mesh.SetVertices(this.m_Positions);
      mesh.SetColors(this.m_Colors);
      mesh.SetUVs(0, this.m_Uv0S);
      mesh.SetUVs(1, this.m_Uv1S);
      mesh.SetNormals(this.m_Normals);
      mesh.SetTangents(this.m_Tangents);
      mesh.SetTriangles(this.m_Indices, 0);
      mesh.RecalculateBounds();
    }

    /// <summary>
    ///   <para>Cleanup allocated memory.</para>
    /// </summary>
    public void Dispose()
    {
      ListPool<Vector3>.Release(this.m_Positions);
      ListPool<Color32>.Release(this.m_Colors);
      ListPool<Vector2>.Release(this.m_Uv0S);
      ListPool<Vector2>.Release(this.m_Uv1S);
      ListPool<Vector3>.Release(this.m_Normals);
      ListPool<Vector4>.Release(this.m_Tangents);
      ListPool<int>.Release(this.m_Indices);
      this.m_Positions = (List<Vector3>) null;
      this.m_Colors = (List<Color32>) null;
      this.m_Uv0S = (List<Vector2>) null;
      this.m_Uv1S = (List<Vector2>) null;
      this.m_Normals = (List<Vector3>) null;
      this.m_Tangents = (List<Vector4>) null;
      this.m_Indices = (List<int>) null;
    }

    /// <summary>
    ///   <para>Add a single vertex to the stream.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="uv0"></param>
    /// <param name="uv1"></param>
    /// <param name="normal"></param>
    /// <param name="tangent"></param>
    /// <param name="v"></param>
    public void AddVert(Vector3 position, Color32 color, Vector2 uv0, Vector2 uv1, Vector3 normal, Vector4 tangent)
    {
      this.m_Positions.Add(position);
      this.m_Colors.Add(color);
      this.m_Uv0S.Add(uv0);
      this.m_Uv1S.Add(uv1);
      this.m_Normals.Add(normal);
      this.m_Tangents.Add(tangent);
    }

    /// <summary>
    ///   <para>Add a single vertex to the stream.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="uv0"></param>
    /// <param name="uv1"></param>
    /// <param name="normal"></param>
    /// <param name="tangent"></param>
    /// <param name="v"></param>
    public void AddVert(Vector3 position, Color32 color, Vector2 uv0)
    {
      this.AddVert(position, color, uv0, Vector2.zero, VertexHelper.s_DefaultNormal, VertexHelper.s_DefaultTangent);
    }

    /// <summary>
    ///   <para>Add a single vertex to the stream.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="uv0"></param>
    /// <param name="uv1"></param>
    /// <param name="normal"></param>
    /// <param name="tangent"></param>
    /// <param name="v"></param>
    public void AddVert(UIVertex v)
    {
      this.AddVert(v.position, v.color, v.uv0, v.uv1, v.normal, v.tangent);
    }

    /// <summary>
    ///   <para>Add a triangle to the buffer.</para>
    /// </summary>
    /// <param name="idx0">Index 0.</param>
    /// <param name="idx1">Index 1.</param>
    /// <param name="idx2">Index 2.</param>
    public void AddTriangle(int idx0, int idx1, int idx2)
    {
      this.m_Indices.Add(idx0);
      this.m_Indices.Add(idx1);
      this.m_Indices.Add(idx2);
    }

    /// <summary>
    ///   <para>Add a quad to the stream.</para>
    /// </summary>
    /// <param name="verts">4 Vertices representing the quad.</param>
    public void AddUIVertexQuad(UIVertex[] verts)
    {
      int currentVertCount = this.currentVertCount;
      for (int index = 0; index < 4; ++index)
        this.AddVert(verts[index].position, verts[index].color, verts[index].uv0, verts[index].uv1, verts[index].normal, verts[index].tangent);
      this.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
      this.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    public void AddUIVertexStream(List<UIVertex> verts, List<int> indices)
    {
      if (verts != null)
        CanvasRenderer.AddUIVertexStream(verts, this.m_Positions, this.m_Colors, this.m_Uv0S, this.m_Uv1S, this.m_Normals, this.m_Tangents);
      if (indices == null)
        return;
      this.m_Indices.AddRange((IEnumerable<int>) indices);
    }

    public void AddUIVertexTriangleStream(List<UIVertex> verts)
    {
      if (verts == null)
        return;
      CanvasRenderer.SplitUIVertexStreams(verts, this.m_Positions, this.m_Colors, this.m_Uv0S, this.m_Uv1S, this.m_Normals, this.m_Tangents, this.m_Indices);
    }

    public void GetUIVertexStream(List<UIVertex> stream)
    {
      if (stream == null)
        return;
      CanvasRenderer.CreateUIVertexStream(stream, this.m_Positions, this.m_Colors, this.m_Uv0S, this.m_Uv1S, this.m_Normals, this.m_Tangents, this.m_Indices);
    }
  }
}
