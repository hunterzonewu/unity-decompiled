// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporterSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Stores settings of a TextureImporter.</para>
  /// </summary>
  [Serializable]
  public sealed class TextureImporterSettings
  {
    [SerializeField]
    private int m_MipMapMode;
    [SerializeField]
    private int m_EnableMipMap;
    [SerializeField]
    private int m_GenerateMipsInLinearSpace;
    [SerializeField]
    private int m_FadeOut;
    [SerializeField]
    private int m_BorderMipMap;
    [SerializeField]
    private int m_MipMapFadeDistanceStart;
    [SerializeField]
    private int m_MipMapFadeDistanceEnd;
    [SerializeField]
    private int m_ConvertToNormalMap;
    [SerializeField]
    private int m_NormalMap;
    [SerializeField]
    private float m_HeightScale;
    [SerializeField]
    private int m_NormalMapFilter;
    [SerializeField]
    private int m_GrayScaleToAlpha;
    [SerializeField]
    private int m_IsReadable;
    [SerializeField]
    private int m_TextureFormat;
    [SerializeField]
    private int m_RecommendedTextureFormat;
    [SerializeField]
    private int m_MaxTextureSize;
    [SerializeField]
    private int m_NPOTScale;
    [SerializeField]
    private int m_Lightmap;
    [SerializeField]
    private int m_LinearTexture;
    [SerializeField]
    private int m_RGBM;
    [SerializeField]
    private int m_CompressionQuality;
    [SerializeField]
    private int m_AllowsAlphaSplit;
    [SerializeField]
    private int m_SpriteMode;
    [SerializeField]
    private uint m_SpriteExtrude;
    [SerializeField]
    private int m_SpriteMeshType;
    [SerializeField]
    private int m_Alignment;
    [SerializeField]
    private Vector2 m_SpritePivot;
    [SerializeField]
    private float m_SpritePixelsToUnits;
    [SerializeField]
    private Vector4 m_SpriteBorder;
    [SerializeField]
    private int m_GenerateCubemap;
    [SerializeField]
    private int m_CubemapConvolution;
    [SerializeField]
    private int m_CubemapConvolutionSteps;
    [SerializeField]
    private float m_CubemapConvolutionExponent;
    [SerializeField]
    private int m_SeamlessCubemap;
    [SerializeField]
    private int m_AlphaIsTransparency;
    [SerializeField]
    private int m_FilterMode;
    [SerializeField]
    private int m_Aniso;
    [SerializeField]
    private float m_MipBias;
    [SerializeField]
    private int m_WrapMode;

    public TextureImporterMipFilter mipmapFilter
    {
      get
      {
        return (TextureImporterMipFilter) this.m_MipMapMode;
      }
      set
      {
        this.m_MipMapMode = (int) value;
      }
    }

    public bool mipmapEnabled
    {
      get
      {
        return this.m_EnableMipMap != 0;
      }
      set
      {
        this.m_EnableMipMap = !value ? 0 : 1;
      }
    }

    public bool generateMipsInLinearSpace
    {
      get
      {
        return this.m_GenerateMipsInLinearSpace != 0;
      }
      set
      {
        this.m_GenerateMipsInLinearSpace = !value ? 0 : 1;
      }
    }

    public bool linearTexture
    {
      get
      {
        return this.m_LinearTexture != 0;
      }
      set
      {
        this.m_LinearTexture = !value ? 0 : 1;
      }
    }

    public bool fadeOut
    {
      get
      {
        return this.m_FadeOut != 0;
      }
      set
      {
        this.m_FadeOut = !value ? 0 : 1;
      }
    }

    public bool borderMipmap
    {
      get
      {
        return this.m_BorderMipMap != 0;
      }
      set
      {
        this.m_BorderMipMap = !value ? 0 : 1;
      }
    }

    public int mipmapFadeDistanceStart
    {
      get
      {
        return this.m_MipMapFadeDistanceStart;
      }
      set
      {
        this.m_MipMapFadeDistanceStart = value;
      }
    }

    public int mipmapFadeDistanceEnd
    {
      get
      {
        return this.m_MipMapFadeDistanceEnd;
      }
      set
      {
        this.m_MipMapFadeDistanceEnd = value;
      }
    }

    public bool convertToNormalMap
    {
      get
      {
        return this.m_ConvertToNormalMap != 0;
      }
      set
      {
        this.m_ConvertToNormalMap = !value ? 0 : 1;
      }
    }

    public bool normalMap
    {
      get
      {
        return this.m_NormalMap != 0;
      }
      set
      {
        this.m_NormalMap = !value ? 0 : 1;
      }
    }

    public float heightmapScale
    {
      get
      {
        return this.m_HeightScale;
      }
      set
      {
        this.m_HeightScale = value;
      }
    }

    public TextureImporterNormalFilter normalMapFilter
    {
      get
      {
        return (TextureImporterNormalFilter) this.m_NormalMapFilter;
      }
      set
      {
        this.m_NormalMapFilter = (int) value;
      }
    }

    public bool grayscaleToAlpha
    {
      get
      {
        return this.m_GrayScaleToAlpha != 0;
      }
      set
      {
        this.m_GrayScaleToAlpha = !value ? 0 : 1;
      }
    }

    public bool readable
    {
      get
      {
        return this.m_IsReadable != 0;
      }
      set
      {
        this.m_IsReadable = !value ? 0 : 1;
      }
    }

    public TextureImporterFormat textureFormat
    {
      get
      {
        return (TextureImporterFormat) this.m_TextureFormat;
      }
      set
      {
        this.m_TextureFormat = (int) value;
      }
    }

    public int maxTextureSize
    {
      get
      {
        return this.m_MaxTextureSize;
      }
      set
      {
        this.m_MaxTextureSize = value;
      }
    }

    public TextureImporterNPOTScale npotScale
    {
      get
      {
        return (TextureImporterNPOTScale) this.m_NPOTScale;
      }
      set
      {
        this.m_NPOTScale = (int) value;
      }
    }

    public bool lightmap
    {
      get
      {
        return this.m_Lightmap != 0;
      }
      set
      {
        this.m_Lightmap = !value ? 0 : 1;
      }
    }

    /// <summary>
    ///   <para>RGBM encoding mode for HDR textures in TextureImporter.</para>
    /// </summary>
    public TextureImporterRGBMMode rgbm
    {
      get
      {
        return (TextureImporterRGBMMode) this.m_RGBM;
      }
      set
      {
        this.m_RGBM = (int) value;
      }
    }

    public TextureImporterGenerateCubemap generateCubemap
    {
      get
      {
        return (TextureImporterGenerateCubemap) this.m_GenerateCubemap;
      }
      set
      {
        this.m_GenerateCubemap = (int) value;
      }
    }

    /// <summary>
    ///   <para>Convolution mode.</para>
    /// </summary>
    public TextureImporterCubemapConvolution cubemapConvolution
    {
      get
      {
        return (TextureImporterCubemapConvolution) this.m_CubemapConvolution;
      }
      set
      {
        this.m_CubemapConvolution = (int) value;
      }
    }

    /// <summary>
    ///   <para>Defines how many different Phong exponents to store in mip maps. Higher value will give better transition between glossy and rough reflections, but will need higher texture resolution.</para>
    /// </summary>
    public int cubemapConvolutionSteps
    {
      get
      {
        return this.m_CubemapConvolutionSteps;
      }
      set
      {
        this.m_CubemapConvolutionSteps = value;
      }
    }

    /// <summary>
    ///   <para>Defines how fast Phong exponent wears off in mip maps. Higher value will apply less blur to high resolution mip maps.</para>
    /// </summary>
    public float cubemapConvolutionExponent
    {
      get
      {
        return this.m_CubemapConvolutionExponent;
      }
      set
      {
        this.m_CubemapConvolutionExponent = value;
      }
    }

    public bool seamlessCubemap
    {
      get
      {
        return this.m_SeamlessCubemap != 0;
      }
      set
      {
        this.m_SeamlessCubemap = !value ? 0 : 1;
      }
    }

    public UnityEngine.FilterMode filterMode
    {
      get
      {
        return (UnityEngine.FilterMode) this.m_FilterMode;
      }
      set
      {
        this.m_FilterMode = (int) value;
      }
    }

    public int aniso
    {
      get
      {
        return this.m_Aniso;
      }
      set
      {
        this.m_Aniso = value;
      }
    }

    public float mipmapBias
    {
      get
      {
        return this.m_MipBias;
      }
      set
      {
        this.m_MipBias = value;
      }
    }

    public TextureWrapMode wrapMode
    {
      get
      {
        return (TextureWrapMode) this.m_WrapMode;
      }
      set
      {
        this.m_WrapMode = (int) value;
      }
    }

    public int compressionQuality
    {
      get
      {
        return this.m_CompressionQuality;
      }
      set
      {
        this.m_CompressionQuality = value;
      }
    }

    /// <summary>
    ///   <para>Allow Alpha splitting on the imported texture when needed (for example ETC1 compression for textures with transparency).</para>
    /// </summary>
    public bool allowsAlphaSplit
    {
      get
      {
        return this.m_AllowsAlphaSplit != 0;
      }
      set
      {
        this.m_AllowsAlphaSplit = !value ? 0 : 1;
      }
    }

    public bool alphaIsTransparency
    {
      get
      {
        return this.m_AlphaIsTransparency != 0;
      }
      set
      {
        this.m_AlphaIsTransparency = !value ? 0 : 1;
      }
    }

    /// <summary>
    ///   <para>Sprite texture import mode.</para>
    /// </summary>
    public int spriteMode
    {
      get
      {
        return this.m_SpriteMode;
      }
      set
      {
        this.m_SpriteMode = value;
      }
    }

    /// <summary>
    ///   <para>The number of pixels in the sprite that correspond to one unit in world space.</para>
    /// </summary>
    public float spritePixelsPerUnit
    {
      get
      {
        return this.m_SpritePixelsToUnits;
      }
      set
      {
        this.m_SpritePixelsToUnits = value;
      }
    }

    /// <summary>
    ///   <para>Scale factor between pixels in the sprite graphic and world space units.</para>
    /// </summary>
    [Obsolete("Use spritePixelsPerUnit property instead.")]
    public float spritePixelsToUnits
    {
      get
      {
        return this.m_SpritePixelsToUnits;
      }
      set
      {
        this.m_SpritePixelsToUnits = value;
      }
    }

    /// <summary>
    ///   <para>The number of blank pixels to leave between the edge of the graphic and the mesh.</para>
    /// </summary>
    public uint spriteExtrude
    {
      get
      {
        return this.m_SpriteExtrude;
      }
      set
      {
        this.m_SpriteExtrude = value;
      }
    }

    public SpriteMeshType spriteMeshType
    {
      get
      {
        return (SpriteMeshType) this.m_SpriteMeshType;
      }
      set
      {
        this.m_SpriteMeshType = (int) value;
      }
    }

    /// <summary>
    ///   <para>Edge-relative alignment of the sprite graphic.</para>
    /// </summary>
    public int spriteAlignment
    {
      get
      {
        return this.m_Alignment;
      }
      set
      {
        this.m_Alignment = value;
      }
    }

    /// <summary>
    ///   <para>Pivot point of the Sprite relative to its graphic's rectangle.</para>
    /// </summary>
    public Vector2 spritePivot
    {
      get
      {
        return this.m_SpritePivot;
      }
      set
      {
        this.m_SpritePivot = value;
      }
    }

    /// <summary>
    ///   <para>Border sizes of the generated sprites.</para>
    /// </summary>
    public Vector4 spriteBorder
    {
      get
      {
        return this.m_SpriteBorder;
      }
      set
      {
        this.m_SpriteBorder = value;
      }
    }

    /// <summary>
    ///   <para>Test texture importer settings for equality.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Equal(TextureImporterSettings a, TextureImporterSettings b);

    /// <summary>
    ///   <para>Copy parameters into another TextureImporterSettings object.</para>
    /// </summary>
    /// <param name="target">TextureImporterSettings object to copy settings to.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void CopyTo(TextureImporterSettings target);

    /// <summary>
    ///   <para>Configure parameters to import a texture for a purpose of type, as described TextureImporterType|here.</para>
    /// </summary>
    /// <param name="type">Texture type. See TextureImporterType.</param>
    /// <param name="applyAll">If false, change only specific properties. Exactly which, depends on type.</param>
    public void ApplyTextureType(TextureImporterType type, bool applyAll)
    {
      TextureImporterSettings.Internal_ApplyTextureType(this, type, applyAll);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_ApplyTextureType(TextureImporterSettings s, TextureImporterType type, bool applyAll);
  }
}
