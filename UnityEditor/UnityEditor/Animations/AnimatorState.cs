// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngineInternal;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>States are the basic building blocks of a state machine. Each state contains a Motion ( AnimationClip or BlendTree) which will play while the character is in that state. When an event in the game triggers a state transition, the character will be left in a new state whose animation sequence will then take over.</para>
  /// </summary>
  public sealed class AnimatorState : UnityEngine.Object
  {
    private PushUndoIfNeeded undoHandler = new PushUndoIfNeeded(true);

    /// <summary>
    ///   <para>The hashed name of the state.</para>
    /// </summary>
    public int nameHash { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The motion assigned to this state.</para>
    /// </summary>
    public Motion motion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The default speed of the motion.</para>
    /// </summary>
    public float speed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>Offset at which the animation loop starts. Useful for synchronizing looped animations.
    /// Units is normalized time.</para>
    ///       </summary>
    public float cycleOffset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the state be mirrored.</para>
    /// </summary>
    public bool mirror { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should Foot IK be respected for this state.</para>
    /// </summary>
    public bool iKOnFeet { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Whether or not the AnimatorStates writes back the default values for properties that are not animated by its Motion.</para>
    /// </summary>
    public bool writeDefaultValues { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A tag can be used to identify a state.</para>
    /// </summary>
    public string tag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The animator controller parameter that drives the speed value.</para>
    /// </summary>
    public string speedParameter { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The animator controller parameter that drives the cycle offset value.</para>
    /// </summary>
    public string cycleOffsetParameter { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The animator controller parameter that drives the mirror value.</para>
    /// </summary>
    public string mirrorParameter { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Define if the speed value is driven by an Animator controller parameter or by the value set in the editor.</para>
    /// </summary>
    public bool speedParameterActive { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Define if the cycle offset value is driven by an Animator controller parameter or by the value set in the editor.</para>
    /// </summary>
    public bool cycleOffsetParameterActive { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Define if the mirror value is driven by an Animator controller parameter or by the value set in the editor.</para>
    /// </summary>
    public bool mirrorParameterActive { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The transitions that are going out of the state.</para>
    /// </summary>
    public AnimatorStateTransition[] transitions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The Behaviour list assigned to this state.</para>
    /// </summary>
    public StateMachineBehaviour[] behaviours { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal bool pushUndo
    {
      set
      {
        this.undoHandler.pushUndo = value;
      }
    }

    [Obsolete("uniqueName does not exist anymore. Consider using .name instead.", true)]
    public string uniqueName
    {
      get
      {
        return string.Empty;
      }
    }

    [Obsolete("uniqueNameHash does not exist anymore.", true)]
    public int uniqueNameHash
    {
      get
      {
        return -1;
      }
    }

    public AnimatorState()
    {
      AnimatorState.Internal_Create(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create(AnimatorState mono);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void AddBehaviour(int instanceID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveBehaviour(int index);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private ScriptableObject Internal_AddStateMachineBehaviourWithType(System.Type stateMachineBehaviourType);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal MonoScript GetBehaviourMonoScript(int index);

    /// <summary>
    ///   <para>Adds a state machine behaviour class of type stateMachineBehaviourType to the AnimatorState. C# Users can use a generic version.</para>
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
    ///   <para>Utility function to add an outgoing transition.</para>
    /// </summary>
    /// <param name="transition">The transition to add.</param>
    public void AddTransition(AnimatorStateTransition transition)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Transition added");
      AnimatorStateTransition[] transitions = this.transitions;
      ArrayUtility.Add<AnimatorStateTransition>(ref transitions, transition);
      this.transitions = transitions;
    }

    /// <summary>
    ///   <para>Utility function to remove a transition from the state.</para>
    /// </summary>
    /// <param name="transition">Transition to remove.</param>
    public void RemoveTransition(AnimatorStateTransition transition)
    {
      this.undoHandler.DoUndo((UnityEngine.Object) this, "Transition removed");
      AnimatorStateTransition[] transitions = this.transitions;
      ArrayUtility.Remove<AnimatorStateTransition>(ref transitions, transition);
      this.transitions = transitions;
      if (!MecanimUtilities.AreSameAsset((UnityEngine.Object) this, (UnityEngine.Object) transition))
        return;
      Undo.DestroyObjectImmediate((UnityEngine.Object) transition);
    }

    private AnimatorStateTransition CreateTransition(bool setDefaultExitTime)
    {
      AnimatorStateTransition newTransition = new AnimatorStateTransition();
      newTransition.hasExitTime = false;
      newTransition.hasFixedDuration = true;
      if (AssetDatabase.GetAssetPath((UnityEngine.Object) this) != string.Empty)
        AssetDatabase.AddObjectToAsset((UnityEngine.Object) newTransition, AssetDatabase.GetAssetPath((UnityEngine.Object) this));
      newTransition.hideFlags = HideFlags.HideInHierarchy;
      if (setDefaultExitTime)
        this.SetDefaultTransitionExitTime(ref newTransition);
      return newTransition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition to the destination state.</para>
    /// </summary>
    /// <param name="defaultExitTime">If true, the exit time will be the equivalent of 0.25 second.</param>
    /// <param name="destinationState">The destination state.</param>
    public AnimatorStateTransition AddTransition(AnimatorState destinationState)
    {
      AnimatorStateTransition transition = this.CreateTransition(false);
      transition.destinationState = destinationState;
      this.AddTransition(transition);
      return transition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition to the destination state machine.</para>
    /// </summary>
    /// <param name="defaultExitTime">If true, the exit time will be the equivalent of 0.25 second.</param>
    /// <param name="destinationStateMachine">The destination state machine.</param>
    public AnimatorStateTransition AddTransition(AnimatorStateMachine destinationStateMachine)
    {
      AnimatorStateTransition transition = this.CreateTransition(false);
      transition.destinationStateMachine = destinationStateMachine;
      this.AddTransition(transition);
      return transition;
    }

    private void SetDefaultTransitionExitTime(ref AnimatorStateTransition newTransition)
    {
      newTransition.hasExitTime = true;
      if (!((UnityEngine.Object) this.motion != (UnityEngine.Object) null) || (double) this.motion.averageDuration <= 0.0)
        return;
      float num = 0.25f / this.motion.averageDuration;
      newTransition.duration = !newTransition.hasFixedDuration ? num : 0.25f;
      newTransition.exitTime = 1f - num;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition to the destination state.</para>
    /// </summary>
    /// <param name="defaultExitTime">If true, the exit time will be the equivalent of 0.25 second.</param>
    /// <param name="destinationState">The destination state.</param>
    public AnimatorStateTransition AddTransition(AnimatorState destinationState, bool defaultExitTime)
    {
      AnimatorStateTransition transition = this.CreateTransition(defaultExitTime);
      transition.destinationState = destinationState;
      this.AddTransition(transition);
      return transition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition to the destination state machine.</para>
    /// </summary>
    /// <param name="defaultExitTime">If true, the exit time will be the equivalent of 0.25 second.</param>
    /// <param name="destinationStateMachine">The destination state machine.</param>
    public AnimatorStateTransition AddTransition(AnimatorStateMachine destinationStateMachine, bool defaultExitTime)
    {
      AnimatorStateTransition transition = this.CreateTransition(defaultExitTime);
      transition.destinationStateMachine = destinationStateMachine;
      this.AddTransition(transition);
      return transition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition to the exit of the state's parent state machine.</para>
    /// </summary>
    /// <param name="defaultExitTime">If true, the exit time will be the equivalent of 0.25 second.</param>
    /// <returns>
    ///   <para>The Animations.AnimatorStateTransition that was added.</para>
    /// </returns>
    public AnimatorStateTransition AddExitTransition()
    {
      AnimatorStateTransition transition = this.CreateTransition(false);
      transition.isExit = true;
      this.AddTransition(transition);
      return transition;
    }

    /// <summary>
    ///   <para>Utility function to add an outgoing transition to the exit of the state's parent state machine.</para>
    /// </summary>
    /// <param name="defaultExitTime">If true, the exit time will be the equivalent of 0.25 second.</param>
    /// <returns>
    ///   <para>The Animations.AnimatorStateTransition that was added.</para>
    /// </returns>
    public AnimatorStateTransition AddExitTransition(bool defaultExitTime)
    {
      AnimatorStateTransition transition = this.CreateTransition(false);
      transition.isExit = true;
      if (defaultExitTime)
        this.SetDefaultTransitionExitTime(ref transition);
      this.AddTransition(transition);
      return transition;
    }

    internal AnimatorStateMachine FindParent(AnimatorStateMachine root)
    {
      if (root.HasState(this, false))
        return root;
      return root.stateMachinesRecursive.Find((Predicate<ChildAnimatorStateMachine>) (sm => sm.stateMachine.HasState(this, false))).stateMachine;
    }

    internal AnimatorStateTransition FindTransition(AnimatorState destinationState)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new List<AnimatorStateTransition>((IEnumerable<AnimatorStateTransition>) this.transitions).Find(new Predicate<AnimatorStateTransition>(new AnimatorState.\u003CFindTransition\u003Ec__AnonStorey17() { destinationState = destinationState }.\u003C\u003Em__1F));
    }

    [Obsolete("GetMotion() is obsolete. Use motion", true)]
    public Motion GetMotion()
    {
      return (Motion) null;
    }
  }
}
