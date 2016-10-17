// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.PassType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Shader pass type for Unity's lighting pipeline.</para>
  /// </summary>
  public enum PassType
  {
    Normal = 0,
    Vertex = 1,
    VertexLM = 2,
    VertexLMRGBM = 3,
    ForwardBase = 4,
    ForwardAdd = 5,
    LightPrePassBase = 6,
    LightPrePassFinal = 7,
    ShadowCaster = 8,
    Deferred = 10,
    Meta = 11,
  }
}
