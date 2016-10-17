// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Director.AnimationClipPlayable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director
{
  /// <summary>
  ///   <para>Playable that plays an AnimationClip. Can be used as an input to an AnimationPlayable.</para>
  /// </summary>
  public sealed class AnimationClipPlayable : AnimationPlayable
  {
    /// <summary>
    ///   <para>AnimationClip played by this playable.</para>
    /// </summary>
    public AnimationClip clip { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The speed at which the AnimationClip is played.</para>
    /// </summary>
    public float speed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AnimationClipPlayable(AnimationClip clip)
      : base(false)
    {
      this.m_Ptr = IntPtr.Zero;
      this.InstantiateEnginePlayable(clip);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InstantiateEnginePlayable(AnimationClip clip);

    public override int AddInput(AnimationPlayable source)
    {
      Debug.LogError((object) "AnimationClipPlayable doesn't support adding inputs");
      return -1;
    }

    public override bool SetInput(AnimationPlayable source, int index)
    {
      Debug.LogError((object) "AnimationClipPlayable doesn't support setting inputs");
      return false;
    }

    public override bool SetInputs(IEnumerable<AnimationPlayable> sources)
    {
      Debug.LogError((object) "AnimationClipPlayable doesn't support setting inputs");
      return false;
    }

    public override bool RemoveInput(int index)
    {
      Debug.LogError((object) "AnimationClipPlayable doesn't support removing inputs");
      return false;
    }

    public override bool RemoveInput(AnimationPlayable playable)
    {
      Debug.LogError((object) "AnimationClipPlayable doesn't support removing inputs");
      return false;
    }

    public override bool RemoveAllInputs()
    {
      Debug.LogError((object) "AnimationClipPlayable doesn't support removing inputs");
      return false;
    }
  }
}
