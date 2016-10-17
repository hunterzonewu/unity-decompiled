// Decompiled with JetBrains decompiler
// Type: UnityEngine.SparseTexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for handling Sparse Textures.</para>
  /// </summary>
  public sealed class SparseTexture : Texture
  {
    /// <summary>
    ///   <para>Get sparse texture tile width (Read Only).</para>
    /// </summary>
    public int tileWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get sparse texture tile height (Read Only).</para>
    /// </summary>
    public int tileHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is the sparse texture actually created? (Read Only)</para>
    /// </summary>
    public bool isCreated { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Create a sparse texture.</para>
    /// </summary>
    /// <param name="width">Texture width in pixels.</param>
    /// <param name="height">Texture height in pixels.</param>
    /// <param name="format">Texture format.</param>
    /// <param name="mipCount">Mipmap count. Pass -1 to create full mipmap chain.</param>
    /// <param name="linear">Whether texture data will be in linear or sRGB color space (default is sRGB).</param>
    public SparseTexture(int width, int height, TextureFormat format, int mipCount)
    {
      SparseTexture.Internal_Create(this, width, height, format, mipCount, false);
    }

    /// <summary>
    ///   <para>Create a sparse texture.</para>
    /// </summary>
    /// <param name="width">Texture width in pixels.</param>
    /// <param name="height">Texture height in pixels.</param>
    /// <param name="format">Texture format.</param>
    /// <param name="mipCount">Mipmap count. Pass -1 to create full mipmap chain.</param>
    /// <param name="linear">Whether texture data will be in linear or sRGB color space (default is sRGB).</param>
    public SparseTexture(int width, int height, TextureFormat format, int mipCount, bool linear)
    {
      SparseTexture.Internal_Create(this, width, height, format, mipCount, linear);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] SparseTexture mono, int width, int height, TextureFormat format, int mipCount, bool linear);

    /// <summary>
    ///   <para>Update sparse texture tile with color values.</para>
    /// </summary>
    /// <param name="tileX">Tile X coordinate.</param>
    /// <param name="tileY">Tile Y coordinate.</param>
    /// <param name="miplevel">Mipmap level of the texture.</param>
    /// <param name="data">Tile color data.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UpdateTile(int tileX, int tileY, int miplevel, Color32[] data);

    /// <summary>
    ///   <para>Update sparse texture tile with raw pixel values.</para>
    /// </summary>
    /// <param name="tileX">Tile X coordinate.</param>
    /// <param name="tileY">Tile Y coordinate.</param>
    /// <param name="miplevel">Mipmap level of the texture.</param>
    /// <param name="data">Tile raw pixel data.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UpdateTileRaw(int tileX, int tileY, int miplevel, byte[] data);

    /// <summary>
    ///   <para>Unload sparse texture tile.</para>
    /// </summary>
    /// <param name="tileX">Tile X coordinate.</param>
    /// <param name="tileY">Tile Y coordinate.</param>
    /// <param name="miplevel">Mipmap level of the texture.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UnloadTile(int tileX, int tileY, int miplevel);
  }
}
