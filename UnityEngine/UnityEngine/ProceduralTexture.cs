// Decompiled with JetBrains decompiler
// Type: UnityEngine.ProceduralTexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for ProceduralTexture handling.</para>
  /// </summary>
  public sealed class ProceduralTexture : Texture
  {
    /// <summary>
    ///   <para>Check whether the ProceduralMaterial that generates this ProceduralTexture is set to an output format with an alpha channel.</para>
    /// </summary>
    public bool hasAlpha { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The format of the pixel data in the texture (Read Only).</para>
    /// </summary>
    public TextureFormat format { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The output type of this ProceduralTexture.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public ProceduralOutputType GetProceduralOutputType();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal ProceduralMaterial GetProceduralMaterial();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool HasBeenGenerated();

    /// <summary>
    ///         <para>Grab pixel values from a ProceduralTexture.
    /// </para>
    ///       </summary>
    /// <param name="x">X-coord of the top-left corner of the rectangle to grab.</param>
    /// <param name="y">Y-coord of the top-left corner of the rectangle to grab.</param>
    /// <param name="blockWidth">Width of rectangle to grab.</param>
    /// <param name="blockHeight">Height of the rectangle to grab.
    /// Get the pixel values from a rectangular area of a ProceduralTexture into an array.
    /// The block is specified by its x,y offset in the texture and by its width and height. The block is "flattened" into the array by scanning the pixel values across rows one by one.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Color32[] GetPixels32(int x, int y, int blockWidth, int blockHeight);
  }
}
