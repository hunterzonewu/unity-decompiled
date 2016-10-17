// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Texture importer lets you modify Texture2D import settings from editor scripts.</para>
  /// </summary>
  public sealed class TextureImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Format of imported texture.</para>
    /// </summary>
    public TextureImporterFormat textureFormat { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Maximum texture size.</para>
    /// </summary>
    public int maxTextureSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Quality of Texture Compression in the range [0..100].</para>
    /// </summary>
    public int compressionQuality { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate alpha channel from intensity?</para>
    /// </summary>
    public bool grayscaleToAlpha { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Cubemap generation mode.</para>
    /// </summary>
    public TextureImporterGenerateCubemap generateCubemap { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Scaling mode for non power of two textures.</para>
    /// </summary>
    public TextureImporterNPOTScale npotScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is texture data readable from scripts.</para>
    /// </summary>
    public bool isReadable { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate mip maps for the texture?</para>
    /// </summary>
    public bool mipmapEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Keep texture borders the same when generating mipmaps?</para>
    /// </summary>
    public bool borderMipmap { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is texture storing non-color data?</para>
    /// </summary>
    public bool linearTexture { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mipmap filtering mode.</para>
    /// </summary>
    public TextureImporterMipFilter mipmapFilter { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Fade out mip levels to gray color?</para>
    /// </summary>
    public bool fadeout { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mip level where texture begins to fade out.</para>
    /// </summary>
    public int mipmapFadeDistanceStart { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mip level where texture is faded out completely.</para>
    /// </summary>
    public int mipmapFadeDistanceEnd { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should mip maps be generated with gamma correction?</para>
    /// </summary>
    public bool generateMipsInLinearSpace { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("correctGamma Property deprecated. Use generateMipsInLinearSpace instead.")]
    public bool correctGamma { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Convert heightmap to normal map?</para>
    /// </summary>
    public bool convertToNormalmap { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is this texture a normal map?</para>
    /// </summary>
    public bool normalmap { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Normal map filtering mode.</para>
    /// </summary>
    public TextureImporterNormalFilter normalmapFilter { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Amount of bumpyness in the heightmap.</para>
    /// </summary>
    public float heightmapScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is this texture a lightmap?</para>
    /// </summary>
    public bool lightmap { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Anisotropic filtering level of the texture.</para>
    /// </summary>
    public int anisoLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Filtering mode of the texture.</para>
    /// </summary>
    public UnityEngine.FilterMode filterMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Wrap mode (Repeat or Clamp) of the texture.</para>
    /// </summary>
    public TextureWrapMode wrapMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mip map bias of the texture.</para>
    /// </summary>
    public float mipMapBias { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool alphaIsTransparency { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns true if this TextureImporter is setup for Sprite packing.</para>
    /// </summary>
    public bool qualifiesForSpritePacking { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Selects Single or Manual import mode for Sprite textures.</para>
    /// </summary>
    public SpriteImportMode spriteImportMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Array representing the sections of the atlas corresponding to individual sprite graphics.</para>
    /// </summary>
    public SpriteMetaData[] spritesheet { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Selects the Sprite packing tag.</para>
    /// </summary>
    public string spritePackingTag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of pixels in the sprite that correspond to one unit in world space.</para>
    /// </summary>
    public float spritePixelsPerUnit { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Scale factor for mapping pixels in the graphic to units in world space.</para>
    /// </summary>
    [Obsolete("Use spritePixelsPerUnit property instead.")]
    public float spritePixelsToUnits { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The point in the Sprite object's coordinate space where the graphic is located.</para>
    /// </summary>
    public Vector2 spritePivot
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_spritePivot(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_spritePivot(ref value);
      }
    }

    /// <summary>
    ///   <para>Border sizes of the generated sprites.</para>
    /// </summary>
    public Vector4 spriteBorder
    {
      get
      {
        Vector4 vector4;
        this.INTERNAL_get_spriteBorder(out vector4);
        return vector4;
      }
      set
      {
        this.INTERNAL_set_spriteBorder(ref value);
      }
    }

    /// <summary>
    ///   <para>Which type of texture are we dealing with here.</para>
    /// </summary>
    public TextureImporterType textureType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Getter for the flag that allows Alpha splitting on the imported texture when needed (for example ETC1 compression for textures with transparency).</para>
    /// </summary>
    /// <returns>
    ///   <para>True if the importer allows alpha split on the imported texture, False otherwise.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetAllowsAlphaSplitting();

    /// <summary>
    ///   <para>Setter for the flag that allows Alpha splitting on the imported texture when needed (for example ETC1 compression for textures with transparency).</para>
    /// </summary>
    /// <param name="flag"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetAllowsAlphaSplitting(bool flag);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool GetPlatformTextureSettings(string platform, out int maxTextureSize, out TextureImporterFormat textureFormat, out int compressionQuality);

    public bool GetPlatformTextureSettings(string platform, out int maxTextureSize, out TextureImporterFormat textureFormat)
    {
      int compressionQuality = 0;
      return this.GetPlatformTextureSettings(platform, out maxTextureSize, out textureFormat, out compressionQuality);
    }

    /// <summary>
    ///   <para>Set specific target platform settings.</para>
    /// </summary>
    /// <param name="platform">The platforms whose settings are to be changed (see below).</param>
    /// <param name="maxTextureSize">Maximum texture width/height in pixels.</param>
    /// <param name="textureFormat">Data format for the texture.</param>
    /// <param name="compressionQuality">Value from 0..100, equivalent to the standard JPEG quality setting.</param>
    /// <param name="allowsAlphaSplit">Allows splitting of imported texture into RGB+A so that ETC1 compression can be applied (Android only, and works only on textures that are a part of some atlas).</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetPlatformTextureSettings(string platform, int maxTextureSize, TextureImporterFormat textureFormat, int compressionQuality, bool allowsAlphaSplit);

    [ExcludeFromDocs]
    public void SetPlatformTextureSettings(string platform, int maxTextureSize, TextureImporterFormat textureFormat)
    {
      bool allowsAlphaSplit = false;
      this.SetPlatformTextureSettings(platform, maxTextureSize, textureFormat, allowsAlphaSplit);
    }

    /// <summary>
    ///   <para>Set specific target platform settings.</para>
    /// </summary>
    /// <param name="platform">The platforms whose settings are to be changed (see below).</param>
    /// <param name="maxTextureSize">Maximum texture width/height in pixels.</param>
    /// <param name="textureFormat">Data format for the texture.</param>
    /// <param name="compressionQuality">Value from 0..100, equivalent to the standard JPEG quality setting.</param>
    /// <param name="allowsAlphaSplit">Allows splitting of imported texture into RGB+A so that ETC1 compression can be applied (Android only, and works only on textures that are a part of some atlas).</param>
    public void SetPlatformTextureSettings(string platform, int maxTextureSize, TextureImporterFormat textureFormat, [DefaultValue("false")] bool allowsAlphaSplit)
    {
      this.SetPlatformTextureSettings(platform, maxTextureSize, textureFormat, 50, allowsAlphaSplit);
    }

    /// <summary>
    ///   <para>Clear specific target platform settings.</para>
    /// </summary>
    /// <param name="platform">The platform whose settings are to be cleared (see below).</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearPlatformTextureSettings(string platform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern TextureImporterFormat FullToSimpleTextureFormat(TextureImporterFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern TextureImporterFormat SimpleToFullTextureFormat2(TextureImporterFormat simpleFormat, TextureImporterType tType, TextureImporterSettings settings, bool doesTextureContainAlpha, bool sourceWasHDR, BuildTarget destinationPlatform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_spritePivot(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_spritePivot(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_spriteBorder(out Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_spriteBorder(ref Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void GetWidthAndHeight(ref int width, ref int height);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool IsSourceTextureHDR();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsTextureFormatETC1Compression(TextureFormat fmt);

    /// <summary>
    ///   <para>Does textures source image have alpha channel.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool DoesSourceTextureHaveAlpha();

    /// <summary>
    ///   <para>Does textures source image have RGB channels.</para>
    /// </summary>
    [WrapperlessIcall]
    [Obsolete("DoesSourceTextureHaveColor always returns true in Unity.")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool DoesSourceTextureHaveColor();

    /// <summary>
    ///   <para>Read texture settings into TextureImporterSettings class.</para>
    /// </summary>
    /// <param name="dest"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ReadTextureSettings(TextureImporterSettings dest);

    /// <summary>
    ///   <para>Set texture importers settings from TextureImporterSettings class.</para>
    /// </summary>
    /// <param name="src"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetTextureSettings(TextureImporterSettings src);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal string GetImportWarnings();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ReadTextureImportInstructions(BuildTarget target, out TextureFormat desiredFormat, out ColorSpace colorSpace, out int compressionQuality);
  }
}
