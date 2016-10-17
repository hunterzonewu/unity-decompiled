// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Director.AnimationMixerPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director
{
  /// <summary>
  ///   <para>Playable used to mix AnimationPlayables.</para>
  /// </summary>
  public class AnimationMixerPlayable : AnimationPlayable
  {
    public AnimationMixerPlayable()
      : base(false)
    {
      this.m_Ptr = IntPtr.Zero;
      this.InstantiateEnginePlayable();
    }

    public AnimationMixerPlayable(bool final)
      : base(false)
    {
      this.m_Ptr = IntPtr.Zero;
      if (!final)
        return;
      this.InstantiateEnginePlayable();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InstantiateEnginePlayable();

    /// <summary>
    ///   <para>Automatically creates an AnimationClipPlayable for each supplied AnimationClip, then sets them as inputs to the mixer.</para>
    /// </summary>
    /// <param name="clips">AnimationClips to be used as inputs.</param>
    /// <returns>
    ///   <para>Returns false if the creation of the AnimationClipPlayables failed, or if the connection failed.</para>
    /// </returns>
    public bool SetInputs(AnimationClip[] clips)
    {
      if (clips == null)
        throw new NullReferenceException("Parameter clips was null. You need to pass in a valid array of clips.");
      AnimationPlayable[] animationPlayableArray = new AnimationPlayable[clips.Length];
      for (int index = 0; index < clips.Length; ++index)
        animationPlayableArray[index] = (AnimationPlayable) new AnimationClipPlayable(clips[index]);
      return this.SetInputs((IEnumerable<AnimationPlayable>) animationPlayableArray);
    }
  }
}
