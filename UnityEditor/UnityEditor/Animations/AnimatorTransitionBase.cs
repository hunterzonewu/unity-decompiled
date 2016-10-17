// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorTransitionBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Base class for animator transitions. Transitions define when and how the state machine switches from one state to another.</para>
  /// </summary>
  public class AnimatorTransitionBase : Object
  {
    private PushUndoIfNeeded undoHandler = new PushUndoIfNeeded(true);

    /// <summary>
    ///   <para>Mutes all other transitions in the source state.</para>
    /// </summary>
    public bool solo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mutes the transition. The transition will never occur.</para>
    /// </summary>
    public bool mute { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the transition destination the exit of the current state machine.</para>
    /// </summary>
    public bool isExit { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The destination state machine of the transition.</para>
    /// </summary>
    public AnimatorStateMachine destinationStateMachine { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The destination state of the transition.</para>
    /// </summary>
    public AnimatorState destinationState { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animations.AnimatorCondition conditions that need to be met for a transition to happen.</para>
    /// </summary>
    public AnimatorCondition[] conditions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal bool pushUndo
    {
      set
      {
        this.undoHandler.pushUndo = value;
      }
    }

    public string GetDisplayName(Object source)
    {
      if (source is AnimatorState)
        return this.GetDisplayNameStateSource(source as AnimatorState);
      return this.GetDisplayNameStateMachineSource(source as AnimatorStateMachine);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal string GetDisplayNameStateSource(AnimatorState source);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal string GetDisplayNameStateMachineSource(AnimatorStateMachine source);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string BuildTransitionName(string source, string destination);

    /// <summary>
    ///   <para>Utility function to add a condition to a transition.</para>
    /// </summary>
    /// <param name="mode">The Animations.AnimatorCondition mode of the condition.</param>
    /// <param name="threshold">The threshold value of the condition.</param>
    /// <param name="parameter">The name of the parameter.</param>
    public void AddCondition(AnimatorConditionMode mode, float threshold, string parameter)
    {
      this.undoHandler.DoUndo((Object) this, "Condition added");
      AnimatorCondition[] conditions = this.conditions;
      ArrayUtility.Add<AnimatorCondition>(ref conditions, new AnimatorCondition()
      {
        mode = mode,
        parameter = parameter,
        threshold = threshold
      });
      this.conditions = conditions;
    }

    /// <summary>
    ///   <para>Utility function to remove a condition from the transition.</para>
    /// </summary>
    /// <param name="condition">The condition to remove.</param>
    public void RemoveCondition(AnimatorCondition condition)
    {
      this.undoHandler.DoUndo((Object) this, "Condition removed");
      AnimatorCondition[] conditions = this.conditions;
      ArrayUtility.Remove<AnimatorCondition>(ref conditions, condition);
      this.conditions = conditions;
    }
  }
}
