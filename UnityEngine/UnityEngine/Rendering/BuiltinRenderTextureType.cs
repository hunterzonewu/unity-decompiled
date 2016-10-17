// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.BuiltinRenderTextureType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Built-in temporary render textures produced during camera's rendering.</para>
  /// </summary>
  public enum BuiltinRenderTextureType
  {
    None = 0,
    CurrentActive = 1,
    CameraTarget = 2,
    Depth = 3,
    DepthNormals = 4,
    PrepassNormalsSpec = 7,
    PrepassLight = 8,
    PrepassLightSpec = 9,
    GBuffer0 = 10,
    GBuffer1 = 11,
    GBuffer2 = 12,
    GBuffer3 = 13,
    Reflections = 14,
  }
}
