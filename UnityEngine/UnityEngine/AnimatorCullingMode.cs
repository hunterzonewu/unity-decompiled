// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorCullingMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Culling mode for the Animator.</para>
  /// </summary>
  public enum AnimatorCullingMode
  {
    AlwaysAnimate = 0,
    [Obsolete("Enum member AnimatorCullingMode.BasedOnRenderers has been deprecated. Use AnimatorCullingMode.CullUpdateTransforms instead (UnityUpgradable) -> CullUpdateTransforms", true)] BasedOnRenderers = 1,
    CullUpdateTransforms = 1,
    CullCompletely = 2,
  }
}
