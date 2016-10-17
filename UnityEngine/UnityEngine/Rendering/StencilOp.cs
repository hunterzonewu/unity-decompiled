// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.StencilOp
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>Specifies the operation that's performed on the stencil buffer when rendering.</para>
  /// </summary>
  public enum StencilOp
  {
    Keep,
    Zero,
    Replace,
    IncrementSaturate,
    DecrementSaturate,
    Invert,
    IncrementWrap,
    DecrementWrap,
  }
}
