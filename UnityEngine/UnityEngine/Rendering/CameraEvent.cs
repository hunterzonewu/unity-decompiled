// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.CameraEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Defines a place in camera's rendering to attach Rendering.CommandBuffer objects to.</para>
  /// </summary>
  public enum CameraEvent
  {
    BeforeDepthTexture,
    AfterDepthTexture,
    BeforeDepthNormalsTexture,
    AfterDepthNormalsTexture,
    BeforeGBuffer,
    AfterGBuffer,
    BeforeLighting,
    AfterLighting,
    BeforeFinalPass,
    AfterFinalPass,
    BeforeForwardOpaque,
    AfterForwardOpaque,
    BeforeImageEffectsOpaque,
    AfterImageEffectsOpaque,
    BeforeSkybox,
    AfterSkybox,
    BeforeForwardAlpha,
    AfterForwardAlpha,
    BeforeImageEffects,
    AfterImageEffects,
    AfterEverything,
    BeforeReflections,
    AfterReflections,
  }
}
