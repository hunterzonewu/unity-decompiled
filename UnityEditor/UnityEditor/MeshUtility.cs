// Decompiled with JetBrains decompiler
// Type: UnityEditor.MeshUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Various utilities for mesh manipulation.</para>
  /// </summary>
  public sealed class MeshUtility
  {
    /// <summary>
    ///   <para>Will insert per-triangle uv2 in mesh and handle vertex splitting etc.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="triUV"></param>
    public static void SetPerTriangleUV2(Mesh src, Vector2[] triUV)
    {
      int num = InternalMeshUtil.CalcTriangleCount(src);
      int length = triUV.Length;
      if (length != 3 * num)
        Debug.LogError((object) ("mesh contains " + (object) num + " triangles but " + (object) length + " uvs are provided"));
      else
        MeshUtility.SetPerTriangleUV2NoCheck(src, triUV);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPerTriangleUV2NoCheck(Mesh src, Vector2[] triUV);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Vector2[] ComputeTextureBoundingHull(Texture texture, int vertexCount);

    /// <summary>
    ///   <para>Change the mesh compression setting for a mesh.</para>
    /// </summary>
    /// <param name="mesh">The mesh to set the compression mode for.</param>
    /// <param name="compression">The compression mode to set.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetMeshCompression(Mesh mesh, ModelImporterMeshCompression compression);

    /// <summary>
    ///   <para>Returns the mesh compression setting for a Mesh.</para>
    /// </summary>
    /// <param name="mesh">The mesh to get information on.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ModelImporterMeshCompression GetMeshCompression(Mesh mesh);

    /// <summary>
    ///   <para>Optimizes the mesh for GPU access.</para>
    /// </summary>
    /// <param name="mesh"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Optimize(Mesh mesh);
  }
}
