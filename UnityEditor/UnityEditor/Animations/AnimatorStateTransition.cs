// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorStateTransition
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Transitions define when and how the state machine switch from one state to another. AnimatorStateTransition always originate from an Animator State (or AnyState) and have timing parameters.</para>
  /// </summary>
  public sealed class AnimatorStateTransition : AnimatorTransitionBase
  {
    /// <summary>
    ///   <para>The duration of the transition.</para>
    /// </summary>
    public float duration { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The time at which the destination state will start.</para>
    /// </summary>
    public float offset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Which AnimatorState transitions can interrupt the Transition.</para>
    /// </summary>
    public TransitionInterruptionSource interruptionSource { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The Transition can be interrupted by a transition that has a higher priority.</para>
    /// </summary>
    public bool orderedInterruption { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The normalized time of the source state when the condition is true.</para>
    /// </summary>
    public float exitTime { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>When active the transition will have an exit time condition.</para>
    /// </summary>
    public bool hasExitTime { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>When active the transition duration will have a fixed duration.</para>
    /// </summary>
    public bool hasFixedDuration { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set to true to allow or disallow transition to self during AnyState transition.</para>
    /// </summary>
    public bool canTransitionToSelf { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Creates a new animator state transition.</para>
    /// </summary>
    public AnimatorStateTransition()
    {
      AnimatorStateTransition.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(AnimatorStateTransition mono);
  }
}
