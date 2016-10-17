// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorStateMachine
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngineInternal;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>A graph controlling the interaction of states. Each state references a motion.</para>
  /// </summary>
  public sealed class AnimatorStateMachine : UnityEngine.Object
  {
    private PushUndoIfNeeded undoHandler = new PushUndoIfNeeded(true);

    /// <summary>
    ///   <para>The list of states.</para>
    /// </summary>
    public ChildAnimatorState[] states { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The list of sub state machines.</para>
    /// </summary>
    public ChildAnimatorStateMachine[] stateMachines { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The state that the state machine will be in when it starts.</para>
    /// </summary>
    public AnimatorState defaultState { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The position of the AnyState node.</para>
    /// </summary>
    public Vector3 anyStatePosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_anyStatePosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_anyStatePosition(ref value);
      }
    }

    /// <summary>
    ///   <para>The position of the entry node.</para>
    /// </summary>
    public Vector3 entryPosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_entryPosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_entryPosition(ref value);
      }
    }

    /// <summary>
    ///   <para>The position of the exit node.</para>
    /// </summary>
    public Vector3 exitPosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_exitPosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_exitPosition(ref value);
      }
    }

    /// <summary>
    ///   <para>The position of the parent state machine node. Only valid when in a hierachic state machine.</para>
    /// </summary>
    public Vector3 parentStateMachinePosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_parentStateMachinePosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_parentStateMachinePosition(ref value);
      }
    }

    /// <summary>
    ///   <para>The list of AnyState transitions.</para>
    /// </summary>
    public AnimatorStateTransition[] anyStateTransitions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The list of entry transitions in the state machine.</para>
    /// </summary>
    public AnimatorTransition[] entryTransitions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The Behaviour list assigned to this state machine.</para>
    /// </summary>
    public StateMachineBehaviour[] behaviours { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal int transitionCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool pushUndo
    {
      set
      {
        this.undoHandler.pushUndo = value;
      }
    }

    internal List<ChildAnimatorState> statesRecursive
    {
      get
      {
        List<ChildAnimatorState> childAnimatorStateList = new List<ChildAnimatorState>();
        childAnimatorStateList.AddRange((IEnumerable<ChildAnimatorState>) this.states);
        for (int index = 0; index < this.stateMachines.Length; ++index)
          childAnimatorStateList.AddRange((IEnumerable<ChildAnimatorState>) this.stateMachines[index].stateMachine.statesRecursive);
        return childAnimatorStateList;
      }
    }

    internal List<ChildAnimatorStateMachine> stateMachinesRecursive
    {
      get
      {
        List<ChildAnimatorStateMachine> animatorStateMachineList = new List<ChildAnimatorStateMachine>();
        animatorStateMachineList.AddRange((IEnumerable<ChildAnimatorStateMachine>) this.stateMachines);
        for (int index = 0; index < this.stateMachines.Length; ++index)
          animatorStateMachineList.AddRange((IEnumerable<ChildAnimatorStateMachine>) this.stateMachines[index].stateMachine.stateMachinesRecursive);
        return animatorStateMachineList;
      }
    }

    internal List<AnimatorStateTransition> anyStateTransitionsRecursive
    {
      get
      {
        List<AnimatorStateTransition> animatorStateTransitionList = new List<AnimatorStateTransition>();
        animatorStateTransitionList.AddRange((IEnumerable<AnimatorStateTransition>) this.anyStateTransitions);
        foreach (ChildAnimatorStateMachine stateMachine in this.stateMachines)
          animatorStateTransitionList.AddRange((IEnumerable<AnimatorStateTransition>) stateMachine.stateMachine.anyStateTransitionsRecursive);
        return animatorStateTransitionList;
      }
    }

    [Obsolete("stateCount is obsolete. Use .states.Length  instead.", true)]
    private int stateCount
    {
      get
      {
        return 0;
      }
    }

    [Obsolete("stateMachineCount is obsolete. Use .stateMachines.Length instead.", true)]
    private int stateMachineCount
    {
      get
      {
        return 0;
      }
    }

    [Obsolete("uniqueNameHash does not exist anymore.", true)]
    private int uniqueNameHash
    {
      get
      {
        return -1;
      }
    }

    public AnimatorStateMachine()
    {
      AnimatorStateMachine.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(AnimatorStateMachine mono);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_anyStatePosition(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_anyStatePosition(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_entryPosition(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_entryPosition(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_exitPosition(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_exitPosition(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_parentStateMachinePosition(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_parentStateMachinePosition(ref Vector3 value);

    /// <summary>
    ///   <para>Gets the list of all outgoing state machine transitions from given state machine.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public AnimatorTransition[] GetStateMachineTransitions(AnimatorStateMachine sourceStateMachine);

    /// <summary>
    ///   <para>Sets the list of all outgoing state machine transitions from given state machine.</para>
    /// </summary>
    /// <param name="stateMachine">The source state machine.</param>
    /// <param name="transitions">The outgoing transitions.</param>
    /// <param name="sourceStateMachine"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetStateMachineTransitions(AnimatorStateMachine sourceStateMachine, AnimatorTransition[] transitions);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void AddBehaviour(int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveBehaviour(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal MonoScript GetBehaviourMonoScript(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private ScriptableObject Internal_AddStateMachineBehaviourWithType(System.Type stateMachineBehaviourType);

    /// <summary>
    ///   <para>Adds a state machine behaviour class of type stateMachineBehaviourType to the AnimatorStateMachine. C# Users can use a generic version.</para>
    /// </summary>
    /// <param name="stateMachineBehaviourType"></param>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
    public StateMachineBehaviour AddStateMachineBehaviour(System.Type stateMachineBehaviourType)
    {
      return (StateMachineBehaviour) this.Internal_AddStateMachineBehaviourWithType(stateMachineBehaviourType);
    }

    public T AddStateMachineBehaviour<T>() where T : StateMachineBehaviour
    {
      return this.AddStateMachineBehaviour(typeof (T)) as T;
    }

    /// <summary>
    ///   <para>Makes a unique state name in the context of the parent state machine.</para>
    /// </summary>
    /// <param name="name">Desired name for the state.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string MakeUniqueStateName(string name);

    /// <summary>
    ///   <para>Makes a unique state machine name in the context of the parent state machine.</para>
    /// </summary>
    /// <param name="name">Desired name for the state machine.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string MakeUniqueStateMachineName(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void Clear();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveStateInternal(AnimatorState state);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveStateMachineInternal(AnimatorStateMachine stateMachine);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void MoveState(AnimatorState state, AnimatorStateMachine target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void MoveStateMachine(AnimatorStateMachine stateMachine, AnimatorStateMachine target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool HasState(AnimatorState state, bool recursive);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool HasStateMachine(AnimatorStateMachine state, bool recursive);

    internal Vector3 GetStatePosition(AnimatorState state)
    {
      ChildAnimatorState[] states = this.states;
      for (int index = 0; index < states.Length; ++index)
      {
        if ((UnityEngine.Object) state == (UnityEngine.Object) states[index].state)
          return states[index].position;
      }
      return Vector3.zero;
    }

    internal void SetStatePosition(AnimatorState state, Vector3 position)
    {
      ChildAnimatorState[] states = this.states;
      for (int index = 0; index < states.Length; ++index)
      {
        if ((UnityEngine.Object) state == (UnityEngine.Object) states[index].state)
        {
          states[index].position = position;
          this.states = states;
          break;
        }
      }
    }

    internal Vector3 GetStateMachinePosition(AnimatorStateMachine stateMachine)
    {
      ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
      for (int index = 0; index < stateMachines.Length; ++index)
      {
        if ((UnityEngine.Object) stateMachine == (UnityEngine.Object) stateMachines[index].stateMachine)
          return stateMachines[index].position;
      }
      return Vector3.zero;
    }

    internal void SetStateMachinePosition(AnimatorStateMachine stateMachine, Vector3 position)
    {
      ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
      for (int index = 0; index < stateMachines.Length; ++index)
      {
        if ((UnityEngine.Object) stateMachine == (UnityEngine.Object) stateMachines[index].stateMachine)
        {
          stateMachines[index].position = position;
          this.stateMachines = stateMachines;
          break;
        }
      }
    }

    /// <summary>
    ///   <para>Utility function to add a state to the state machine.</para>
    /// </summary>
    /// <param name="name">The name of the new state.</param>
    /// <param name="position">The position of the state node.</param>
    /// <returns>
    ///   <para>The AnimatorState that was created for this state.</para>
    /// </returns>
    public AnimatorState AddState(string name)
    {
      return this.AddState(name, this.states.Length <= 0 ? new Vector3(200f, 0.0f, 0.0f) : this.states[this.states.Length - 1].position + new Vector3(35f, 65f));
    }

    /// <summary>
    ///   <para>Utility function to add a state to the state machine.</para>
    /// </summary>
    /// <param name="name">The name of the new state.</param>
    /// <param name="position">The position of the state node.</param>
    /// <returns>
    ///   <para>The AnimatorState that was created for this state.</para>
    /// </returns>
    public AnimatorState AddState(string name, Vector3 position)
    {
      AnimatorState state = new AnimatorState();
      state.hideFlags = HideFlags.HideInHierarchy;
      state.name = this.MakeUniqueStateName(name);
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) state, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      this.AddState(state, position);
      return state;
    }

    /// <summary>
    ///   <para>Utility function to add a state to the state machine.</para>
    /// </summary>
    /// <param name="state">The state to add.</param>
    /// <param name="position">The position of the state node.</param>
    public void AddState(AnimatorState state, Vector3 position)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "State added");
      ChildAnimatorState childAnimatorState = new ChildAnimatorState();
      childAnimatorState.state = state;
      childAnimatorState.position = position;
      ChildAnimatorState[] states = this.states;
      ArrayUtility.Add<ChildAnimatorState>(ref states, childAnimatorState);
      this.states = states;
    }

    /// <summary>
    ///   <para>Utility function to remove a state from the state machine.</para>
    /// </summary>
    /// <param name="state">The state to remove.</param>
    public void RemoveState(AnimatorState state)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "State removed");
      this.undoHandler.DoUndo((UnityEngine.Object) state, "State removed");
      this.RemoveStateInternal(state);
    }

    /// <summary>
    ///   <para>Utility function to add a state machine to the state machine.</para>
    /// </summary>
    /// <param name="name">The name of the new state machine.</param>
    /// <param name="position">The position of the state machine node.</param>
    /// <returns>
    ///   <para>The newly created Animations.AnimatorStateMachine state machine.</para>
    /// </returns>
    public AnimatorStateMachine AddStateMachine(string name)
    {
      return this.AddStateMachine(name, Vector3.zero);
    }

    /// <summary>
    ///   <para>Utility function to add a state machine to the state machine.</para>
    /// </summary>
    /// <param name="name">The name of the new state machine.</param>
    /// <param name="position">The position of the state machine node.</param>
    /// <returns>
    ///   <para>The newly created Animations.AnimatorStateMachine state machine.</para>
    /// </returns>
    public AnimatorStateMachine AddStateMachine(string name, Vector3 position)
    {
      AnimatorStateMachine stateMachine = new AnimatorStateMachine();
      stateMachine.hideFlags = HideFlags.HideInHierarchy;
      stateMachine.name = this.MakeUniqueStateMachineName(name);
      this.AddStateMachine(stateMachine, position);
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) stateMachine, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      return stateMachine;
    }

    /// <summary>
    ///   <para>Utility function to add a state machine to the state machine.</para>
    /// </summary>
    /// <param name="stateMachine">The state machine to add.</param>
    /// <param name="position">The position of the state machine node.</param>
    public void AddStateMachine(AnimatorStateMachine stateMachine, Vector3 position)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "StateMachine " + stateMachine.name + " added");
      ChildAnimatorStateMachine animatorStateMachine = new ChildAnimatorStateMachine();
      animatorStateMachine.stateMachine = stateMachine;
      animatorStateMachine.position = position;
      ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
      ArrayUtility.Add<ChildAnimatorStateMachine>(ref stateMachines, animatorStateMachine);
      this.stateMachines = stateMachines;
    }

    /// <summary>
    ///   <para>Utility function to remove a state machine from its parent state machine.</para>
    /// </summary>
    /// <param name="stateMachine">The state machine to remove.</param>
    public void RemoveStateMachine(AnimatorStateMachine stateMachine)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "StateMachine removed");
      this.undoHandler.DoUndo((UnityEngine.Object) stateMachine, "StateMachine removed");
      this.RemoveStateMachineInternal(stateMachine);
    }

    private AnimatorStateTransition AddAnyStateTransition()
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "AnyState Transition Added");
      AnimatorStateTransition[] stateTransitions = this.anyStateTransitions;
      AnimatorStateTransition animatorStateTransition = new AnimatorStateTransition();
      animatorStateTransition.hasExitTime = false;
      animatorStateTransition.hasFixedDuration = true;
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) animatorStateTransition, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      animatorStateTransition.hideFlags = HideFlags.HideInHierarchy;
      ArrayUtility.Add<AnimatorStateTransition>(ref stateTransitions, animatorStateTransition);
      this.anyStateTransitions = stateTransitions;
      return animatorStateTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an AnyState transition to the specified state or statemachine.</para>
    /// </summary>
    /// <param name="destinationState">The destination state.</param>
    /// <param name="destinationStateMachine">The destination statemachine.</param>
    public AnimatorStateTransition AddAnyStateTransition(AnimatorState destinationState)
    {
      AnimatorStateTransition animatorStateTransition = this.AddAnyStateTransition();
      animatorStateTransition.destinationState = destinationState;
      return animatorStateTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an AnyState transition to the specified state or statemachine.</para>
    /// </summary>
    /// <param name="destinationState">The destination state.</param>
    /// <param name="destinationStateMachine">The destination statemachine.</param>
    public AnimatorStateTransition AddAnyStateTransition(AnimatorStateMachine destinationStateMachine)
    {
      AnimatorStateTransition animatorStateTransition = this.AddAnyStateTransition();
      animatorStateTransition.destinationStateMachine = destinationStateMachine;
      return animatorStateTransition;
    }

    /// <summary>
    ///   <para>Utility function to remove an AnyState transition from the state machine.</para>
    /// </summary>
    /// <param name="transition">The AnyStat transition to remove.</param>
    public bool RemoveAnyStateTransition(AnimatorStateTransition transition)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CRemoveAnyStateTransition\u003Ec__AnonStorey18 transitionCAnonStorey18 = new AnimatorStateMachine.\u003CRemoveAnyStateTransition\u003Ec__AnonStorey18();
      // ISSUE: reference to a compiler-generated field
      transitionCAnonStorey18.transition = transition;
      // ISSUE: reference to a compiler-generated method
      if (!new List<AnimatorStateTransition>((IEnumerable<AnimatorStateTransition>) this.anyStateTransitions).Any<AnimatorStateTransition>(new Func<AnimatorStateTransition, bool>(transitionCAnonStorey18.\u003C\u003Em__20)))
        return false;
      this.undoHandler.DoUndo((UnityEngine.Object) this, "AnyState Transition Removed");
      AnimatorStateTransition[] stateTransitions = this.anyStateTransitions;
      // ISSUE: reference to a compiler-generated field
      ArrayUtility.Remove<AnimatorStateTransition>(ref stateTransitions, transitionCAnonStorey18.transition);
      this.anyStateTransitions = stateTransitions;
      // ISSUE: reference to a compiler-generated field
      if (MecanimUtilities.AreSameAsset((UnityEngine.Object) this, (UnityEngine.Object) transitionCAnonStorey18.transition))
      {
        // ISSUE: reference to a compiler-generated field
        Undo.DestroyObjectImmediate((UnityEngine.Object) transitionCAnonStorey18.transition);
      }
      return true;
    }

    internal void RemoveAnyStateTransitionRecursive(AnimatorStateTransition transition)
    {
      if (this.RemoveAnyStateTransition(transition))
        return;
      using (List<ChildAnimatorStateMachine>.Enumerator enumerator = this.stateMachinesRecursive.GetEnumerator())
      {
        do
          ;
        while (enumerator.MoveNext() && !enumerator.Current.stateMachine.RemoveAnyStateTransition(transition));
      }
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition from the source state machine to the destination.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    /// <param name="destinationStateMachine">The destination state machine.</param>
    /// <param name="destinationState">The destination state.</param>
    /// <returns>
    ///   <para>The Animations.AnimatorTransition transition that was created.</para>
    /// </returns>
    public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine)
    {
      AnimatorStateMachine destinationStateMachine = (AnimatorStateMachine) null;
      return this.AddStateMachineTransition(sourceStateMachine, destinationStateMachine);
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition from the source state machine to the destination.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    /// <param name="destinationStateMachine">The destination state machine.</param>
    /// <param name="destinationState">The destination state.</param>
    /// <returns>
    ///   <para>The Animations.AnimatorTransition transition that was created.</para>
    /// </returns>
    public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorStateMachine destinationStateMachine)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "StateMachine Transition Added");
      AnimatorTransition[] machineTransitions = this.GetStateMachineTransitions(sourceStateMachine);
      AnimatorTransition animatorTransition = new AnimatorTransition();
      if ((bool) ((UnityEngine.Object) destinationStateMachine))
        animatorTransition.destinationStateMachine = destinationStateMachine;
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) animatorTransition, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      animatorTransition.hideFlags = HideFlags.HideInHierarchy;
      ArrayUtility.Add<AnimatorTransition>(ref machineTransitions, animatorTransition);
      this.SetStateMachineTransitions(sourceStateMachine, machineTransitions);
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition from the source state machine to the destination.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    /// <param name="destinationStateMachine">The destination state machine.</param>
    /// <param name="destinationState">The destination state.</param>
    /// <returns>
    ///   <para>The Animations.AnimatorTransition transition that was created.</para>
    /// </returns>
    public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorState destinationState)
    {
      AnimatorTransition animatorTransition = this.AddStateMachineTransition(sourceStateMachine);
      animatorTransition.destinationState = destinationState;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition from the source state machine to the exit of it's parent state machine.</para>
    /// </summary>
    /// <param name="sourceStateMachine">The source state machine.</param>
    public AnimatorTransition AddStateMachineExitTransition(AnimatorStateMachine sourceStateMachine)
    {
      AnimatorTransition animatorTransition = this.AddStateMachineTransition(sourceStateMachine);
      animatorTransition.isExit = true;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to remove an outgoing transition from source state machine.</para>
    /// </summary>
    /// <param name="transition">The transition to remove.</param>
    /// <param name="sourceStateMachine">The source state machine.</param>
    public bool RemoveStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorTransition transition)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "StateMachine Transition Removed");
      AnimatorTransition[] machineTransitions = this.GetStateMachineTransitions(sourceStateMachine);
      int length = machineTransitions.Length;
      ArrayUtility.Remove<AnimatorTransition>(ref machineTransitions, transition);
      this.SetStateMachineTransitions(sourceStateMachine, machineTransitions);
      if (MecanimUtilities.AreSameAsset((UnityEngine.Object) this, (UnityEngine.Object) transition))
        Undo.DestroyObjectImmediate((UnityEngine.Object) transition);
      return length != machineTransitions.Length;
    }

    private AnimatorTransition AddEntryTransition()
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Entry Transition Added");
      AnimatorTransition[] entryTransitions = this.entryTransitions;
      AnimatorTransition animatorTransition = new AnimatorTransition();
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) animatorTransition, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      animatorTransition.hideFlags = HideFlags.HideInHierarchy;
      ArrayUtility.Add<AnimatorTransition>(ref entryTransitions, animatorTransition);
      this.entryTransitions = entryTransitions;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an incoming transition to the exit of it's parent state machine.</para>
    /// </summary>
    /// <param name="destinationState">The destination Animations.AnimatorState state.</param>
    /// <param name="destinationStateMachine">The destination Animations.AnimatorStateMachine state machine.</param>
    public AnimatorTransition AddEntryTransition(AnimatorState destinationState)
    {
      AnimatorTransition animatorTransition = this.AddEntryTransition();
      animatorTransition.destinationState = destinationState;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an incoming transition to the exit of it's parent state machine.</para>
    /// </summary>
    /// <param name="destinationState">The destination Animations.AnimatorState state.</param>
    /// <param name="destinationStateMachine">The destination Animations.AnimatorStateMachine state machine.</param>
    public AnimatorTransition AddEntryTransition(AnimatorStateMachine destinationStateMachine)
    {
      AnimatorTransition animatorTransition = this.AddEntryTransition();
      animatorTransition.destinationStateMachine = destinationStateMachine;
      return animatorTransition;
    }

    /// <summary>
    ///   <para>Utility function to remove an entry transition from the state machine.</para>
    /// </summary>
    /// <param name="transition">The transition to remove.</param>
    public bool RemoveEntryTransition(AnimatorTransition transition)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CRemoveEntryTransition\u003Ec__AnonStorey19 transitionCAnonStorey19 = new AnimatorStateMachine.\u003CRemoveEntryTransition\u003Ec__AnonStorey19();
      // ISSUE: reference to a compiler-generated field
      transitionCAnonStorey19.transition = transition;
      // ISSUE: reference to a compiler-generated method
      if (!new List<AnimatorTransition>((IEnumerable<AnimatorTransition>) this.entryTransitions).Any<AnimatorTransition>(new Func<AnimatorTransition, bool>(transitionCAnonStorey19.\u003C\u003Em__21)))
        return false;
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Entry Transition Removed");
      AnimatorTransition[] entryTransitions = this.entryTransitions;
      // ISSUE: reference to a compiler-generated field
      ArrayUtility.Remove<AnimatorTransition>(ref entryTransitions, transitionCAnonStorey19.transition);
      this.entryTransitions = entryTransitions;
      // ISSUE: reference to a compiler-generated field
      if (MecanimUtilities.AreSameAsset((UnityEngine.Object) this, (UnityEngine.Object) transitionCAnonStorey19.transition))
      {
        // ISSUE: reference to a compiler-generated field
        Undo.DestroyObjectImmediate((UnityEngine.Object) transitionCAnonStorey19.transition);
      }
      return true;
    }

    internal ChildAnimatorState FindState(int nameHash)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new List<ChildAnimatorState>((IEnumerable<ChildAnimatorState>) this.states).Find(new Predicate<ChildAnimatorState>(new AnimatorStateMachine.\u003CFindState\u003Ec__AnonStorey1A() { nameHash = nameHash }.\u003C\u003Em__22));
    }

    internal ChildAnimatorState FindState(string name)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new List<ChildAnimatorState>((IEnumerable<ChildAnimatorState>) this.states).Find(new Predicate<ChildAnimatorState>(new AnimatorStateMachine.\u003CFindState\u003Ec__AnonStorey1B() { name = name }.\u003C\u003Em__23));
    }

    internal bool HasState(AnimatorState state)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return this.statesRecursive.Any<ChildAnimatorState>(new Func<ChildAnimatorState, bool>(new AnimatorStateMachine.\u003CHasState\u003Ec__AnonStorey1C() { state = state }.\u003C\u003Em__24));
    }

    internal bool IsDirectParent(AnimatorStateMachine stateMachine)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return ((IEnumerable<ChildAnimatorStateMachine>) this.stateMachines).Any<ChildAnimatorStateMachine>(new Func<ChildAnimatorStateMachine, bool>(new AnimatorStateMachine.\u003CIsDirectParent\u003Ec__AnonStorey1D() { stateMachine = stateMachine }.\u003C\u003Em__25));
    }

    internal bool HasStateMachine(AnimatorStateMachine child)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return this.stateMachinesRecursive.Any<ChildAnimatorStateMachine>(new Func<ChildAnimatorStateMachine, bool>(new AnimatorStateMachine.\u003CHasStateMachine\u003Ec__AnonStorey1E() { child = child }.\u003C\u003Em__26));
    }

    internal bool HasTransition(AnimatorState stateA, AnimatorState stateB)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CHasTransition\u003Ec__AnonStorey1F transitionCAnonStorey1F = new AnimatorStateMachine.\u003CHasTransition\u003Ec__AnonStorey1F();
      // ISSUE: reference to a compiler-generated field
      transitionCAnonStorey1F.stateB = stateB;
      // ISSUE: reference to a compiler-generated field
      transitionCAnonStorey1F.stateA = stateA;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!((IEnumerable<AnimatorStateTransition>) transitionCAnonStorey1F.stateA.transitions).Any<AnimatorStateTransition>(new Func<AnimatorStateTransition, bool>(transitionCAnonStorey1F.\u003C\u003Em__27)))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return ((IEnumerable<AnimatorStateTransition>) transitionCAnonStorey1F.stateB.transitions).Any<AnimatorStateTransition>(new Func<AnimatorStateTransition, bool>(transitionCAnonStorey1F.\u003C\u003Em__28));
      }
      return true;
    }

    internal AnimatorStateMachine FindParent(AnimatorStateMachine stateMachine)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CFindParent\u003Ec__AnonStorey20 parentCAnonStorey20 = new AnimatorStateMachine.\u003CFindParent\u003Ec__AnonStorey20();
      // ISSUE: reference to a compiler-generated field
      parentCAnonStorey20.stateMachine = stateMachine;
      // ISSUE: reference to a compiler-generated method
      if (((IEnumerable<ChildAnimatorStateMachine>) this.stateMachines).Any<ChildAnimatorStateMachine>(new Func<ChildAnimatorStateMachine, bool>(parentCAnonStorey20.\u003C\u003Em__29)))
        return this;
      // ISSUE: reference to a compiler-generated method
      return this.stateMachinesRecursive.Find(new Predicate<ChildAnimatorStateMachine>(parentCAnonStorey20.\u003C\u003Em__2A)).stateMachine;
    }

    internal AnimatorStateMachine FindStateMachine(string path)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStorey21 machineCAnonStorey21 = new AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStorey21();
      // ISSUE: reference to a compiler-generated field
      machineCAnonStorey21.smNames = path.Split('.');
      AnimatorStateMachine animatorStateMachine = this;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStorey22 machineCAnonStorey22 = new AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStorey22();
      // ISSUE: reference to a compiler-generated field
      machineCAnonStorey22.\u003C\u003Ef__ref\u002433 = machineCAnonStorey21;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (machineCAnonStorey22.i = 1; machineCAnonStorey22.i < machineCAnonStorey21.smNames.Length - 1 && (UnityEngine.Object) animatorStateMachine != (UnityEngine.Object) null; machineCAnonStorey22.i = machineCAnonStorey22.i + 1)
      {
        // ISSUE: reference to a compiler-generated method
        int index = Array.FindIndex<ChildAnimatorStateMachine>(animatorStateMachine.stateMachines, new Predicate<ChildAnimatorStateMachine>(machineCAnonStorey22.\u003C\u003Em__2B));
        animatorStateMachine = index < 0 ? (AnimatorStateMachine) null : animatorStateMachine.stateMachines[index].stateMachine;
      }
      if ((UnityEngine.Object) animatorStateMachine == (UnityEngine.Object) null)
        return this;
      return animatorStateMachine;
    }

    internal AnimatorStateMachine FindStateMachine(AnimatorState state)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStorey23 machineCAnonStorey23 = new AnimatorStateMachine.\u003CFindStateMachine\u003Ec__AnonStorey23();
      // ISSUE: reference to a compiler-generated field
      machineCAnonStorey23.state = state;
      // ISSUE: reference to a compiler-generated field
      if (this.HasState(machineCAnonStorey23.state, false))
        return this;
      List<ChildAnimatorStateMachine> machinesRecursive = this.stateMachinesRecursive;
      // ISSUE: reference to a compiler-generated method
      int index = machinesRecursive.FindIndex(new Predicate<ChildAnimatorStateMachine>(machineCAnonStorey23.\u003C\u003Em__2C));
      if (index >= 0)
        return machinesRecursive[index].stateMachine;
      return (AnimatorStateMachine) null;
    }

    internal AnimatorStateTransition FindTransition(AnimatorState destinationState)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new List<AnimatorStateTransition>((IEnumerable<AnimatorStateTransition>) this.anyStateTransitions).Find(new Predicate<AnimatorStateTransition>(new AnimatorStateMachine.\u003CFindTransition\u003Ec__AnonStorey24() { destinationState = destinationState }.\u003C\u003Em__2D));
    }

    [Obsolete("GetTransitionsFromState is obsolete. Use AnimatorState.transitions instead.", true)]
    private AnimatorState GetTransitionsFromState(AnimatorState state)
    {
      return (AnimatorState) null;
    }
  }
}
