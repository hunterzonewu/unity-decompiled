// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Utility functions to assist with working with shaders from the editor.</para>
  /// </summary>
  public sealed class ShaderUtil
  {
    internal static Rect rawViewportRect
    {
      get
      {
        Rect rect;
        ShaderUtil.INTERNAL_get_rawViewportRect(out rect);
        return rect;
      }
      set
      {
        ShaderUtil.INTERNAL_set_rawViewportRect(ref value);
      }
    }

    internal static Rect rawScissorRect
    {
      get
      {
        Rect rect;
        ShaderUtil.INTERNAL_get_rawScissorRect(out rect);
        return rect;
      }
      set
      {
        ShaderUtil.INTERNAL_set_rawScissorRect(ref value);
      }
    }

    /// <summary>
    ///   <para>Does the current hardware support render textues.</para>
    /// </summary>
    public static extern bool hardwareSupportsRectRenderTexture { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern bool hardwareSupportsFullNPOT { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetAvailableShaderCompilerPlatforms();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void FetchCachedErrors(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetShaderErrorCount(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern ShaderError[] GetShaderErrors(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetComputeShaderErrorCount(ComputeShader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern ShaderError[] GetComputeShaderErrors(ComputeShader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetComboCount(Shader s, bool usedBySceneOnly);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasSurfaceShaders(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasFixedFunctionShaders(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasShaderSnippets(Shader s);

    /// <summary>
    ///   <para>Get the number of properties in Shader s.</para>
    /// </summary>
    /// <param name="s">The shader to check against.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetPropertyCount(Shader s);

    /// <summary>
    ///   <para>Get the description of the shader propery at index propertyIdx of Shader s.</para>
    /// </summary>
    /// <param name="s">The shader to check against.</param>
    /// <param name="propertyIdx">The property index to use.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetPropertyDescription(Shader s, int propertyIdx);

    /// <summary>
    ///   <para>Get the name of the shader propery at index propertyIdx of Shader s.</para>
    /// </summary>
    /// <param name="s">The shader to check against.</param>
    /// <param name="propertyIdx">The property index to use.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetPropertyName(Shader s, int propertyIdx);

    /// <summary>
    ///   <para>Get the ShaderProperyType of the shader propery at index propertyIdx of Shader s.</para>
    /// </summary>
    /// <param name="s">The shader to check against.</param>
    /// <param name="propertyIdx">The property index to use.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ShaderUtil.ShaderPropertyType GetPropertyType(Shader s, int propertyIdx);

    /// <summary>
    ///   <para>Get Limits for a range property at index propertyIdx of Shader s.</para>
    /// </summary>
    /// <param name="defminmax">Which value to get: 0 = default, 1 = min, 2 = max.</param>
    /// <param name="s">The shader to check against.</param>
    /// <param name="propertyIdx">The property index to use.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetRangeLimits(Shader s, int propertyIdx, int defminmax);

    /// <summary>
    ///   <para>Gets the ShaderPropertyTexDim of the texture at property index propertyIdx of Shader s.</para>
    /// </summary>
    /// <param name="s">The shader to check against.</param>
    /// <param name="propertyIdx">The property index to use.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ShaderUtil.ShaderPropertyTexDim GetTexDim(Shader s, int propertyIdx);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetTextureDimension(Texture t);

    /// <summary>
    ///   <para>Is the shader propery at index propertyIdx of Shader s hidden?</para>
    /// </summary>
    /// <param name="s">The shader to check against.</param>
    /// <param name="propertyIdx">The property index to use.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsShaderPropertyHidden(Shader s, int propertyIdx);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string[] GetShaderPropertyAttributes(Shader s, string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasShadowCasterPass(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasTangentChannel(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool DoesIgnoreProjector(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetRenderQueue(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetLOD(Shader s);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetDependency(Shader s, string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetTextureBindingIndex(Shader s, int texturePropertyID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OpenCompiledShader(Shader shader, int mode, int customPlatformsMask, bool includeAllVariants);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OpenCompiledComputeShader(ComputeShader shader, bool allVariantsAndPlatforms);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OpenParsedSurfaceShader(Shader shader);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OpenGeneratedFixedFunctionShader(Shader shader);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OpenShaderSnippets(Shader shader);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OpenShaderCombinations(Shader shader, bool usedBySceneOnly);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CalculateLightmapStrippingFromCurrentScene();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void CalculateFogStrippingFromCurrentScene();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SaveCurrentShaderVariantCollection(string path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ClearCurrentShaderVariantCollection();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ReloadAllShaders();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetCurrentShaderVariantCollectionShaderCount();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetCurrentShaderVariantCollectionVariantCount();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void GetShaderVariantEntries(Shader shader, ShaderVariantCollection skipAlreadyInCollection, out int[] types, out string[] keywords);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool AddNewShaderToCollection(Shader shader, ShaderVariantCollection collection);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_rawViewportRect(out Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_rawViewportRect(ref Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_rawScissorRect(out Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_rawScissorRect(ref Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void RecreateGfxDevice();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void RecreateSkinnedMeshResources();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Shader CreateShaderAsset(string source);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UpdateShaderAsset(Shader shader, string source);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern MaterialProperty[] GetMaterialProperties(Object[] mats);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern MaterialProperty GetMaterialProperty(Object[] mats, string name);

    internal static MaterialProperty GetMaterialProperty(Object[] mats, int propertyIndex)
    {
      return ShaderUtil.GetMaterialProperty_Index(mats, propertyIndex);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern MaterialProperty GetMaterialProperty_Index(Object[] mats, int propertyIndex);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ApplyProperty(MaterialProperty prop, int propertyMask, string undoName);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ApplyMaterialPropertyBlockToMaterialProperty(MaterialPropertyBlock propertyBlock, MaterialProperty materialProperty);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ApplyMaterialPropertyToMaterialPropertyBlock(MaterialProperty materialProperty, int propertyMask, MaterialPropertyBlock propertyBlock);

    /// <summary>
    ///   <para>Type of a given texture property.</para>
    /// </summary>
    public enum ShaderPropertyType
    {
      Color,
      Vector,
      Float,
      Range,
      TexEnv,
    }

    internal enum ShaderCompilerPlatformType
    {
      OpenGL,
      D3D9,
      Xbox360,
      PS3,
      D3D11,
      OpenGLES20,
      OpenGLES20Desktop,
      Flash,
      D3D11_9x,
      OpenGLES30,
      PSVita,
      PS4,
      XboxOne,
      PSM,
      Metal,
      OpenGLCore,
      N3DS,
      WiiU,
      Count,
    }

    /// <summary>
    ///   <para>Representation of the texture dimensions.</para>
    /// </summary>
    public enum ShaderPropertyTexDim
    {
      TexDimUnknown = -1,
      TexDimNone = 0,
      TexDimDeprecated1D = 1,
      TexDim2D = 2,
      TexDim3D = 3,
      TexDimCUBE = 4,
      TexDimAny = 5,
      TexDimRECT = 5,
    }
  }
}
