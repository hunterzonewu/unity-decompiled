// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.StateMachineBehaviourContext
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>This class contains all the owner's information for this State Machine Behaviour.</para>
  /// </summary>
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class StateMachineBehaviourContext
  {
    /// <summary>
    ///   <para>The Animations.AnimatorController that owns this state machine behaviour.</para>
    /// </summary>
    public AnimatorController animatorController;
    /// <summary>
    ///   <para>The object that owns this state machine behaviour. Could be an Animations.AnimatorState or Animations.AnimatorStateMachine.</para>
    /// </summary>
    public UnityEngine.Object animatorObject;
    /// <summary>
    ///   <para>The animator's layer index that owns this state machine behaviour.</para>
    /// </summary>
    public int layerIndex;
  }
}
