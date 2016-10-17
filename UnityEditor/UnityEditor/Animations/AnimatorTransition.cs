// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorTransition
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Transitions define when and how the state machine switch from on state to another. AnimatorTransition always originate from a StateMachine or a StateMachine entry. They do not define timing parameters.</para>
  /// </summary>
  public sealed class AnimatorTransition : AnimatorTransitionBase
  {
    /// <summary>
    ///   <para>Creates a new animator transition.</para>
    /// </summary>
    public AnimatorTransition()
    {
      AnimatorTransition.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(AnimatorTransition mono);
  }
}
