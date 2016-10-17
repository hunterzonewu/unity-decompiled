// Decompiled with JetBrains decompiler
// Type: UnityEngine.BillboardAsset
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>BillboardAsset describes how a billboard is rendered.</para>
  /// </summary>
  public sealed class BillboardAsset : Object
  {
    /// <summary>
    ///   <para>Width of the billboard.</para>
    /// </summary>
    public float width { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Height of the billboard.</para>
    /// </summary>
    public float height { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Height of the billboard that is below ground.</para>
    /// </summary>
    public float bottom { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Number of pre-baked images that can be switched when the billboard is viewed from different angles.</para>
    /// </summary>
    public int imageCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of vertices in the billboard mesh. The mesh is not necessarily a quad. It can be a more complex shape which fits the actual image more precisely.</para>
    /// </summary>
    public int vertexCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of indices in the billboard mesh. The mesh is not necessarily a quad. It can be a more complex shape which fits the actual image more precisely.</para>
    /// </summary>
    public int indexCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The material used for rendering.</para>
    /// </summary>
    public Material material { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void MakeRenderMesh(Mesh mesh, float widthScale, float heightScale, float rotation);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void MakeMaterialProperties(MaterialPropertyBlock properties, Camera camera);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void MakePreviewMesh(Mesh mesh);
  }
}
