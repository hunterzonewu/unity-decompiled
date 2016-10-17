// Decompiled with JetBrains decompiler
// Type: UnityEngine.Mesh
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A class that allows creating or modifying meshes from scripts.</para>
  /// </summary>
  public sealed class Mesh : Object
  {
    /// <summary>
    ///   <para>Returns state of the Read/Write Enabled checkbox when model was imported.</para>
    /// </summary>
    public bool isReadable { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool canAccess { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns a copy of the vertex positions or assigns a new vertex positions array.</para>
    /// </summary>
    public Vector3[] vertices { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The normals of the mesh.</para>
    /// </summary>
    public Vector3[] normals { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The tangents of the mesh.</para>
    /// </summary>
    public Vector4[] tangents { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The base texture coordinates of the mesh.</para>
    /// </summary>
    public Vector2[] uv { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The second texture coordinate set of the mesh, if present.</para>
    /// </summary>
    public Vector2[] uv2 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The third texture coordinate set of the mesh, if present.</para>
    /// </summary>
    public Vector2[] uv3 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The fourth texture coordinate set of the mesh, if present.</para>
    /// </summary>
    public Vector2[] uv4 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The bounding volume of the mesh.</para>
    /// </summary>
    public Bounds bounds
    {
      get
      {
        Bounds bounds;
        this.INTERNAL_get_bounds(out bounds);
        return bounds;
      }
      set
      {
        this.INTERNAL_set_bounds(ref value);
      }
    }

    /// <summary>
    ///   <para>Vertex colors of the mesh.</para>
    /// </summary>
    public Color[] colors { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Vertex colors of the mesh.</para>
    /// </summary>
    public Color32[] colors32 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>An array containing all triangles in the mesh.</para>
    /// </summary>
    public int[] triangles { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the number of vertices in the mesh (Read Only).</para>
    /// </summary>
    public int vertexCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of submeshes. Every material has a separate triangle list.</para>
    /// </summary>
    public int subMeshCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The bone weights of each vertex.</para>
    /// </summary>
    public BoneWeight[] boneWeights { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The bind poses. The bind pose at each index refers to the bone with the same index.</para>
    /// </summary>
    public Matrix4x4[] bindposes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns BlendShape count on this mesh.</para>
    /// </summary>
    public int blendShapeCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property Mesh.uv1 has been deprecated. Use Mesh.uv2 instead (UnityUpgradable) -> uv2", true)]
    public Vector2[] uv1
    {
      get
      {
        return (Vector2[]) null;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Creates an empty mesh.</para>
    /// </summary>
    public Mesh()
    {
      Mesh.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] Mesh mono);

    /// <summary>
    ///   <para>Clears all vertex data and all triangle indices.</para>
    /// </summary>
    /// <param name="keepVertexLayout"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Clear([DefaultValue("true")] bool keepVertexLayout);

    [ExcludeFromDocs]
    public void Clear()
    {
      this.Clear(true);
    }

    public void SetVertices(List<Vector3> inVertices)
    {
      this.SetVerticesInternal((object) inVertices);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetVerticesInternal(object vertices);

    public void SetNormals(List<Vector3> inNormals)
    {
      this.SetNormalsInternal((object) inNormals);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetNormalsInternal(object normals);

    public void SetTangents(List<Vector4> inTangents)
    {
      this.SetTangentsInternal((object) inTangents);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetTangentsInternal(object tangents);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Array ExtractListData(object list);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetUVsInternal(Array uvs, int channel, int dim, int arraySize);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void GetUVsInternal(Array uvs, int channel, int dim);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool CheckCanAccessUVChannel(int channel);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ResizeList(object list, int size);

    private void GetUVsImpl<T>(int channel, List<T> uvs, int dim)
    {
      if (uvs == null)
        throw new ArgumentException("The result uvs list cannot be null");
      if (this.CheckCanAccessUVChannel(channel))
      {
        int vertexCount = this.vertexCount;
        if (vertexCount > uvs.Capacity)
          uvs.Capacity = vertexCount;
        Mesh.ResizeList((object) uvs, vertexCount);
        this.GetUVsInternal(Mesh.ExtractListData((object) uvs), channel, dim);
      }
      else
        uvs.Clear();
    }

    public void SetUVs(int channel, List<Vector2> uvs)
    {
      this.SetUVsInternal(Mesh.ExtractListData((object) uvs), channel, 2, uvs.Count);
    }

    public void SetUVs(int channel, List<Vector3> uvs)
    {
      this.SetUVsInternal(Mesh.ExtractListData((object) uvs), channel, 3, uvs.Count);
    }

    public void SetUVs(int channel, List<Vector4> uvs)
    {
      this.SetUVsInternal(Mesh.ExtractListData((object) uvs), channel, 4, uvs.Count);
    }

    public void GetUVs(int channel, List<Vector2> uvs)
    {
      this.GetUVsImpl<Vector2>(channel, uvs, 2);
    }

    public void GetUVs(int channel, List<Vector3> uvs)
    {
      this.GetUVsImpl<Vector3>(channel, uvs, 3);
    }

    public void GetUVs(int channel, List<Vector4> uvs)
    {
      this.GetUVsImpl<Vector4>(channel, uvs, 4);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_bounds(out Bounds value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_bounds(ref Bounds value);

    public void SetColors(List<Color> inColors)
    {
      this.SetColorsInternal((object) inColors);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetColorsInternal(object colors);

    public void SetColors(List<Color32> inColors)
    {
      this.SetColors32Internal((object) inColors);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetColors32Internal(object colors);

    /// <summary>
    ///   <para>Recalculate the bounding volume of the mesh from the vertices.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RecalculateBounds();

    /// <summary>
    ///   <para>Recalculates the normals of the mesh from the triangles and vertices.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RecalculateNormals();

    /// <summary>
    ///   <para>Optimizes the mesh for display.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Optimize();

    /// <summary>
    ///   <para>Returns the triangle list for the submesh.</para>
    /// </summary>
    /// <param name="submesh"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int[] GetTriangles(int submesh);

    /// <summary>
    ///   <para>Sets the triangle list for the submesh.</para>
    /// </summary>
    /// <param name="inTriangles"></param>
    /// <param name="submesh"></param>
    /// <param name="triangles"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetTriangles(int[] triangles, int submesh);

    public void SetTriangles(List<int> inTriangles, int submesh)
    {
      this.SetTrianglesInternal((object) inTriangles, submesh);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetTrianglesInternal(object triangles, int submesh);

    /// <summary>
    ///   <para>Returns the index buffer for the submesh.</para>
    /// </summary>
    /// <param name="submesh"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int[] GetIndices(int submesh);

    /// <summary>
    ///   <para>Sets the index buffer for the submesh.</para>
    /// </summary>
    /// <param name="indices"></param>
    /// <param name="topology"></param>
    /// <param name="submesh"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetIndices(int[] indices, MeshTopology topology, int submesh);

    /// <summary>
    ///   <para>Gets the topology of a submesh.</para>
    /// </summary>
    /// <param name="submesh"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public MeshTopology GetTopology(int submesh);

    /// <summary>
    ///   <para>Combines several meshes into this mesh.</para>
    /// </summary>
    /// <param name="combine">Descriptions of the meshes to combine.</param>
    /// <param name="mergeSubMeshes">Should all meshes be combined into a single submesh?</param>
    /// <param name="useMatrices">Should the transforms supplied in the CombineInstance array be used or ignored?</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void CombineMeshes(CombineInstance[] combine, [DefaultValue("true")] bool mergeSubMeshes, [DefaultValue("true")] bool useMatrices);

    [ExcludeFromDocs]
    public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes)
    {
      bool useMatrices = true;
      this.CombineMeshes(combine, mergeSubMeshes, useMatrices);
    }

    [ExcludeFromDocs]
    public void CombineMeshes(CombineInstance[] combine)
    {
      bool useMatrices = true;
      bool mergeSubMeshes = true;
      this.CombineMeshes(combine, mergeSubMeshes, useMatrices);
    }

    /// <summary>
    ///   <para>Optimize mesh for frequent updates.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void MarkDynamic();

    /// <summary>
    ///   <para>Upload previously done mesh modifications to the graphics API.</para>
    /// </summary>
    /// <param name="markNoLogerReadable">Frees up system memory copy of mesh data when set to true.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UploadMeshData(bool markNoLogerReadable);

    /// <summary>
    ///   <para>Returns index of BlendShape by given name.</para>
    /// </summary>
    /// <param name="blendShapeName"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetBlendShapeIndex(string blendShapeName);

    /// <summary>
    ///   <para>Returns name of BlendShape by given index.</para>
    /// </summary>
    /// <param name="shapeIndex"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetBlendShapeName(int shapeIndex);

    /// <summary>
    ///   <para>Returns the frame count for a blend shape.</para>
    /// </summary>
    /// <param name="shapeIndex">The shape index to get frame count from.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetBlendShapeFrameCount(int shapeIndex);

    /// <summary>
    ///   <para>Returns the weight of a blend shape frame.</para>
    /// </summary>
    /// <param name="shapeIndex">The shape index of the frame.</param>
    /// <param name="frameIndex">The frame index to get the weight from.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetBlendShapeFrameWeight(int shapeIndex, int frameIndex);

    /// <summary>
    ///   <para>Retreives deltaVertices, deltaNormals and deltaTangents of a blend shape frame.</para>
    /// </summary>
    /// <param name="shapeIndex">The shape index of the frame.</param>
    /// <param name="frameIndex">The frame index to get the weight from.</param>
    /// <param name="deltaVertices">Delta vertices output array for the frame being retreived.</param>
    /// <param name="deltaNormals">Delta normals output array for the frame being retreived.</param>
    /// <param name="deltaTangents">Delta tangents output array for the frame being retreived.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void GetBlendShapeFrameVertices(int shapeIndex, int frameIndex, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents);

    /// <summary>
    ///   <para>Clears all blend shapes from Mesh.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearBlendShapes();

    /// <summary>
    ///   <para>Adds a new blend shape frame.</para>
    /// </summary>
    /// <param name="shapeName">Name of the blend shape to add a frame to.</param>
    /// <param name="frameWeight">Weight for the frame being added.</param>
    /// <param name="deltaVertices">Delta vertices for the frame being added.</param>
    /// <param name="deltaNormals">Delta normals for the frame being added.</param>
    /// <param name="deltaTangents">Delta tangents for the frame being added.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void AddBlendShapeFrame(string shapeName, float frameWeight, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents);
  }
}
