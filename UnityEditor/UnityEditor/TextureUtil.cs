// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class TextureUtil
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetStorageMemorySize(Texture t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetRuntimeMemorySize(Texture t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsNonPowerOfTwo(Texture2D t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern TextureUsageMode GetUsageMode(Texture t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetBytesFromTextureFormat(TextureFormat inFormat);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetRowBytesFromWidthAndFormat(int width, TextureFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsValidTextureFormat(TextureFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsCompressedTextureFormat(TextureFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern TextureFormat GetTextureFormat(Texture texture);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsAlphaOnlyTextureFormat(TextureFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasAlphaTextureFormat(TextureFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetTextureFormatString(TextureFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetTextureColorSpaceString(Texture texture);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern TextureFormat ConvertToAlphaTextureFormat(TextureFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsDepthRTFormat(RenderTextureFormat format);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasMipMap(Texture t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetGLWidth(Texture t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetGLHeight(Texture t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int CountMipmaps(Texture t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetLinearSampled(Texture t);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetDefaultCompressionQuality();

    public static Vector4 GetTexelSizeVector(Texture t)
    {
      Vector4 vector4;
      TextureUtil.INTERNAL_CALL_GetTexelSizeVector(t, out vector4);
      return vector4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetTexelSizeVector(Texture t, out Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D GetSourceTexture(Cubemap cubemapRef, CubemapFace face);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetSourceTexture(Cubemap cubemapRef, CubemapFace face, Texture2D tex);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CopyTextureIntoCubemapFace(Texture2D textureRef, Cubemap cubemapRef, CubemapFace face);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CopyCubemapFaceIntoTexture(Cubemap cubemapRef, CubemapFace face, Texture2D textureRef);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool ReformatCubemap(ref Cubemap cubemap, int width, int height, TextureFormat textureFormat, bool useMipmap, bool linear);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool ReformatTexture(ref Texture2D texture, int width, int height, TextureFormat textureFormat, bool useMipmap, bool linear);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAnisoLevelNoDirty(Texture tex, int level);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetWrapModeNoDirty(Texture tex, TextureWrapMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetMipMapBiasNoDirty(Texture tex, float bias);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetFilterModeNoDirty(Texture tex, UnityEngine.FilterMode mode);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool DoesTextureStillNeedToBeCompressed(string assetPath);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsCubemapReadable(Cubemap cubemapRef);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void MarkCubemapReadable(Cubemap cubemapRef, bool readable);
  }
}
