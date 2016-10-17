// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.BlendTreePreviewUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal
{
  public sealed class BlendTreePreviewUtility
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetRootBlendTreeChildWeights(Animator animator, int layerIndex, int stateHash, float[] weightArray);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CalculateRootBlendTreeChildWeights(Animator animator, int layerIndex, int stateHash, float[] weightArray, float blendX, float blendY);

    public static void CalculateBlendTexture(Animator animator, int layerIndex, int stateHash, Texture2D blendTexture, Texture2D[] weightTextures, Rect rect)
    {
      BlendTreePreviewUtility.INTERNAL_CALL_CalculateBlendTexture(animator, layerIndex, stateHash, blendTexture, weightTextures, ref rect);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CalculateBlendTexture(Animator animator, int layerIndex, int stateHash, Texture2D blendTexture, Texture2D[] weightTextures, ref Rect rect);
  }
}
