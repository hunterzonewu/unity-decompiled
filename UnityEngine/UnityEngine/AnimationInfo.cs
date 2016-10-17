// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.ComponentModel;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information about what animation clips is played and its weight.</para>
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("Use AnimatorClipInfo instead (UnityUpgradable) -> AnimatorClipInfo", true)]
  public struct AnimationInfo
  {
    /// <summary>
    ///   <para>Animation clip that is played.</para>
    /// </summary>
    public AnimationClip clip
    {
      get
      {
        return (AnimationClip) null;
      }
    }

    /// <summary>
    ///   <para>The weight of the animation clip.</para>
    /// </summary>
    public float weight
    {
      get
      {
        return 0.0f;
      }
    }
  }
}
