// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimationClipPair
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>This class defines a pair of clips used by AnimatorOverrideController.</para>
  /// </summary>
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class AnimationClipPair
  {
    /// <summary>
    ///   <para>The original clip from the controller.</para>
    /// </summary>
    public AnimationClip originalClip;
    /// <summary>
    ///   <para>The override animation clip.</para>
    /// </summary>
    public AnimationClip overrideClip;
  }
}
