// Decompiled with JetBrains decompiler
// Type: UnityEngine.Texture3D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for handling 3D Textures, Use this to create.</para>
  /// </summary>
  public sealed class Texture3D : Texture
  {
    /// <summary>
    ///   <para>The depth of the texture (Read Only).</para>
    /// </summary>
    public int depth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The format of the pixel data in the texture (Read Only).</para>
    /// </summary>
    public TextureFormat format { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Create a new empty 3D Texture.</para>
    /// </summary>
    /// <param name="width">Width of texture in pixels.</param>
    /// <param name="height">Height of texture in pixels.</param>
    /// <param name="depth">Depth of texture in pixels.</param>
    /// <param name="format">Texture data format.</param>
    /// <param name="mipmap">Should the texture have mipmaps?</param>
    public Texture3D(int width, int height, int depth, TextureFormat format, bool mipmap)
    {
      Texture3D.Internal_Create(this, width, height, depth, format, mipmap);
    }

    /// <summary>
    ///   <para>Returns an array of pixel colors representing one mip level of the 3D texture.</para>
    /// </summary>
    /// <param name="miplevel"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Color[] GetPixels([DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color[] GetPixels()
    {
      return this.GetPixels(0);
    }

    /// <summary>
    ///   <para>Returns an array of pixel colors representing one mip level of the 3D texture.</para>
    /// </summary>
    /// <param name="miplevel"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Color32[] GetPixels32([DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public Color32[] GetPixels32()
    {
      return this.GetPixels32(0);
    }

    /// <summary>
    ///   <para>Sets pixel colors of a 3D texture.</para>
    /// </summary>
    /// <param name="colors">The colors to set the pixels to.</param>
    /// <param name="miplevel">The mipmap level to be affected by the new colors.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetPixels(Color[] colors, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels(Color[] colors)
    {
      int miplevel = 0;
      this.SetPixels(colors, miplevel);
    }

    /// <summary>
    ///   <para>Sets pixel colors of a 3D texture.</para>
    /// </summary>
    /// <param name="colors">The colors to set the pixels to.</param>
    /// <param name="miplevel">The mipmap level to be affected by the new colors.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetPixels32(Color32[] colors, [DefaultValue("0")] int miplevel);

    [ExcludeFromDocs]
    public void SetPixels32(Color32[] colors)
    {
      int miplevel = 0;
      this.SetPixels32(colors, miplevel);
    }

    /// <summary>
    ///   <para>Actually apply all previous SetPixels changes.</para>
    /// </summary>
    /// <param name="updateMipmaps"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Apply([DefaultValue("true")] bool updateMipmaps);

    [ExcludeFromDocs]
    public void Apply()
    {
      this.Apply(true);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] Texture3D mono, int width, int height, int depth, TextureFormat format, bool mipmap);
  }
}
