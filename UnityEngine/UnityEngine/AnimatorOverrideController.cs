// Decompiled with JetBrains decompiler
// Type: UnityEngine.AnimatorOverrideController
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Interface to control AnimatorOverrideController.</para>
  /// </summary>
  public sealed class AnimatorOverrideController : RuntimeAnimatorController
  {
    internal AnimatorOverrideController.OnOverrideControllerDirtyCallback OnOverrideControllerDirty;

    /// <summary>
    ///   <para>The Controller that the AnimatorOverrideController overrides.</para>
    /// </summary>
    public RuntimeAnimatorController runtimeAnimatorController { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public AnimationClip this[string name]
    {
      get
      {
        return this.Internal_GetClipByName(name, true);
      }
      set
      {
        this.Internal_SetClipByName(name, value);
      }
    }

    public AnimationClip this[AnimationClip clip]
    {
      get
      {
        return this.Internal_GetClip(clip, true);
      }
      set
      {
        this.Internal_SetClip(clip, value);
      }
    }

    /// <summary>
    ///   <para>Returns the list of orignal clip from the controller and their override clip.</para>
    /// </summary>
    public AnimationClipPair[] clips
    {
      get
      {
        AnimationClip[] originalClips = this.GetOriginalClips();
        Dictionary<AnimationClip, bool> dictionary = new Dictionary<AnimationClip, bool>(originalClips.Length);
        foreach (AnimationClip index in originalClips)
          dictionary[index] = true;
        AnimationClip[] array = new AnimationClip[dictionary.Count];
        dictionary.Keys.CopyTo(array, 0);
        AnimationClipPair[] animationClipPairArray = new AnimationClipPair[array.Length];
        for (int index = 0; index < array.Length; ++index)
        {
          animationClipPairArray[index] = new AnimationClipPair();
          animationClipPairArray[index].originalClip = array[index];
          animationClipPairArray[index].overrideClip = this.Internal_GetClip(array[index], false);
        }
        return animationClipPairArray;
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
          this.Internal_SetClip(value[index].originalClip, value[index].overrideClip, false);
        this.Internal_SetDirty();
      }
    }

    public AnimatorOverrideController()
    {
      AnimatorOverrideController.Internal_CreateAnimationSet(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateAnimationSet([Writable] AnimatorOverrideController self);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private AnimationClip Internal_GetClipByName(string name, bool returnEffectiveClip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetClipByName(string name, AnimationClip clip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private AnimationClip Internal_GetClip(AnimationClip originalClip, bool returnEffectiveClip);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetClip(AnimationClip originalClip, AnimationClip overrideClip, [DefaultValue("true")] bool notify);

    [ExcludeFromDocs]
    private void Internal_SetClip(AnimationClip originalClip, AnimationClip overrideClip)
    {
      bool notify = true;
      this.Internal_SetClip(originalClip, overrideClip, notify);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetDirty();

    internal static void OnInvalidateOverrideController(AnimatorOverrideController controller)
    {
      if (controller.OnOverrideControllerDirty == null)
        return;
      controller.OnOverrideControllerDirty();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private AnimationClip[] GetOriginalClips();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private AnimationClip[] GetOverrideClips();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void PerformOverrideClipListCleanup();

    internal delegate void OnOverrideControllerDirtyCallback();
  }
}
