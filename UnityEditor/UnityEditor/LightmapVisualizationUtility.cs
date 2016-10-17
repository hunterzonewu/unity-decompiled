// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightmapVisualizationUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngineInternal;

namespace UnityEditor
{
  internal sealed class LightmapVisualizationUtility
  {
    public static Vector4 GetLightmapTilingOffset(LightmapType lightmapType)
    {
      Vector4 vector4;
      LightmapVisualizationUtility.INTERNAL_CALL_GetLightmapTilingOffset(lightmapType, out vector4);
      return vector4;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetLightmapTilingOffset(LightmapType lightmapType, out Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D GetGITexture(GITextureType textureType);

    public static void DrawTextureWithUVOverlay(Texture2D texture, GameObject gameObject, Rect drawableArea, Rect position, GITextureType textureType, bool drawSpecularUV)
    {
      LightmapVisualizationUtility.INTERNAL_CALL_DrawTextureWithUVOverlay(texture, gameObject, ref drawableArea, ref position, textureType, drawSpecularUV);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_DrawTextureWithUVOverlay(Texture2D texture, GameObject gameObject, ref Rect drawableArea, ref Rect position, GITextureType textureType, bool drawSpecularUV);
  }
}
