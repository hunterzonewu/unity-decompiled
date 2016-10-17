// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationCullingType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>This enum controlls culling of Animation component.</para>
  /// </summary>
  public enum AnimationCullingType
  {
    AlwaysAnimate,
    BasedOnRenderers,
    [Obsolete("Enum member AnimatorCullingMode.BasedOnClipBounds has been deprecated. Use AnimationCullingType.AlwaysAnimate or AnimationCullingType.BasedOnRenderers instead")] BasedOnClipBounds,
    [Obsolete("Enum member AnimatorCullingMode.BasedOnUserBounds has been deprecated. Use AnimationCullingType.AlwaysAnimate or AnimationCullingType.BasedOnRenderers instead")] BasedOnUserBounds,
  }
}
