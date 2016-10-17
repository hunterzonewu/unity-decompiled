// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.AtlasSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Sprites
{
  /// <summary>
  ///   <para>Describes the final atlas texture.</para>
  /// </summary>
  public struct AtlasSettings
  {
    /// <summary>
    ///   <para>The format of the atlas texture.</para>
    /// </summary>
    public TextureFormat format;
    /// <summary>
    ///   <para>Desired color space of the atlas.</para>
    /// </summary>
    public ColorSpace colorSpace;
    /// <summary>
    ///   <para>Quality of atlas texture compression in the range [0..100].</para>
    /// </summary>
    public int compressionQuality;
    /// <summary>
    ///   <para>Filtering mode of the atlas texture.</para>
    /// </summary>
    public UnityEngine.FilterMode filterMode;
    /// <summary>
    ///   <para>Maximum width of the atlas texture.</para>
    /// </summary>
    public int maxWidth;
    /// <summary>
    ///   <para>Maximum height of the atlas texture.</para>
    /// </summary>
    public int maxHeight;
    /// <summary>
    ///   <para>The amount of extra padding between packed sprites.</para>
    /// </summary>
    public uint paddingPower;
    /// <summary>
    ///   <para>Anisotropic filtering level of the atlas texture.</para>
    /// </summary>
    public int anisoLevel;
    /// <summary>
    ///   <para>Should sprite atlas textures generate mip maps?</para>
    /// </summary>
    public bool generateMipMaps;
    /// <summary>
    ///   <para>Allows Sprite Packer to rotate/flip the Sprite to ensure optimal Packing.</para>
    /// </summary>
    public bool enableRotation;
    /// <summary>
    ///   <para>Marks this atlas so that it contains textures that have been flagged for Alpha splitting when needed (for example ETC1 compression for textures with transparency).</para>
    /// </summary>
    public bool allowsAlphaSplitting;
  }
}
