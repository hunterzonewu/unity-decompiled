// Decompiled with JetBrains decompiler
// Type: UnityEngine.StateMachineBehaviour
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Experimental.Director;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>StateMachineBehaviour is a component that can be added to a state machine state. It's the base class every script on a state derives from.</para>
  /// </summary>
  [RequiredByNativeCode]
  public abstract class StateMachineBehaviour : ScriptableObject
  {
    public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public virtual void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public virtual void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public virtual void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    /// <summary>
    ///   <para>Called on the first Update frame when making a transition to a StateMachine. This is not called when making a transition into a StateMachine sub-state.</para>
    /// </summary>
    /// <param name="animator">The Animator playing this state machine.</param>
    /// <param name="stateMachinePathHash">The full path hash for this state machine.</param>
    public virtual void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
    }

    /// <summary>
    ///   <para>Called on the last Update frame when making a transition out of a StateMachine. This is not called when making a transition into a StateMachine sub-state.</para>
    /// </summary>
    /// <param name="animator">The Animator playing this state machine.</param>
    /// <param name="stateMachinePathHash">The full path hash for this state machine.</param>
    public virtual void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
    }

    public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
    }

    public virtual void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
    }

    public virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
    }

    public virtual void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
    }

    public virtual void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
    }

    public virtual void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
    }

    public virtual void OnStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
    }
  }
}
