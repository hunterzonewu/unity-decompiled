// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.AnimatorControllerLayer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>The Animation Layer contains a state machine that controls animations of a model or part of it.</para>
  /// </summary>
  public sealed class AnimatorControllerLayer
  {
    private int m_SyncedLayerIndex = -1;
    private string m_Name;
    private AnimatorStateMachine m_StateMachine;
    private AvatarMask m_AvatarMask;
    private StateMotionPair[] m_Motions;
    private StateBehavioursPair[] m_Behaviours;
    private AnimatorLayerBlendingMode m_BlendingMode;
    private bool m_IKPass;
    private float m_DefaultWeight;
    private bool m_SyncedLayerAffectsTiming;

    /// <summary>
    ///   <para>The name of the layer.</para>
    /// </summary>
    public string name
    {
      get
      {
        return this.m_Name;
      }
      set
      {
        this.m_Name = value;
      }
    }

    /// <summary>
    ///   <para>The state machine for the layer.</para>
    /// </summary>
    public AnimatorStateMachine stateMachine
    {
      get
      {
        return this.m_StateMachine;
      }
      set
      {
        this.m_StateMachine = value;
      }
    }

    /// <summary>
    ///   <para>The AvatarMask that is used to mask the animation on the given layer.</para>
    /// </summary>
    public AvatarMask avatarMask
    {
      get
      {
        return this.m_AvatarMask;
      }
      set
      {
        this.m_AvatarMask = value;
      }
    }

    /// <summary>
    ///   <para>The blending mode used by the layer. It is not taken into account for the first layer.</para>
    /// </summary>
    public AnimatorLayerBlendingMode blendingMode
    {
      get
      {
        return this.m_BlendingMode;
      }
      set
      {
        this.m_BlendingMode = value;
      }
    }

    /// <summary>
    ///   <para>Specifies the index of the Synced Layer.</para>
    /// </summary>
    public int syncedLayerIndex
    {
      get
      {
        return this.m_SyncedLayerIndex;
      }
      set
      {
        this.m_SyncedLayerIndex = value;
      }
    }

    /// <summary>
    ///   <para>When active, the layer will have an IK pass when evaluated. It will trigger an OnAnimatorIK callback.</para>
    /// </summary>
    public bool iKPass
    {
      get
      {
        return this.m_IKPass;
      }
      set
      {
        this.m_IKPass = value;
      }
    }

    /// <summary>
    ///   <para>The default blending weight that the layers has. It is not taken into account for the first layer.</para>
    /// </summary>
    public float defaultWeight
    {
      get
      {
        return this.m_DefaultWeight;
      }
      set
      {
        this.m_DefaultWeight = value;
      }
    }

    /// <summary>
    ///   <para>When active, the layer will take control of the duration of the Synced Layer.</para>
    /// </summary>
    public bool syncedLayerAffectsTiming
    {
      get
      {
        return this.m_SyncedLayerAffectsTiming;
      }
      set
      {
        this.m_SyncedLayerAffectsTiming = value;
      }
    }

    /// <summary>
    ///   <para>Gets the override motion for the state on the given layer.</para>
    /// </summary>
    /// <param name="state">The state which we want to get the motion.</param>
    public Motion GetOverrideMotion(AnimatorState state)
    {
      if (this.m_Motions != null)
      {
        foreach (StateMotionPair motion in this.m_Motions)
        {
          if ((Object) motion.m_State == (Object) state)
            return motion.m_Motion;
        }
      }
      return (Motion) null;
    }

    /// <summary>
    ///   <para>Sets the override motion for the state on the given layer.</para>
    /// </summary>
    /// <param name="state">The state which we want to set the motion.</param>
    /// <param name="motion">The motion that will be set.</param>
    public void SetOverrideMotion(AnimatorState state, Motion motion)
    {
      if (this.m_Motions == null)
        this.m_Motions = new StateMotionPair[0];
      for (int index = 0; index < this.m_Motions.Length; ++index)
      {
        if ((Object) this.m_Motions[index].m_State == (Object) state)
        {
          this.m_Motions[index].m_Motion = motion;
          return;
        }
      }
      StateMotionPair stateMotionPair;
      stateMotionPair.m_State = state;
      stateMotionPair.m_Motion = motion;
      ArrayUtility.Add<StateMotionPair>(ref this.m_Motions, stateMotionPair);
    }

    /// <summary>
    ///   <para>Gets the override behaviour list for the state on the given layer.</para>
    /// </summary>
    /// <param name="state">The state which we want to get the behaviour list.</param>
    public StateMachineBehaviour[] GetOverrideBehaviours(AnimatorState state)
    {
      if (this.m_Behaviours != null)
      {
        foreach (StateBehavioursPair behaviour in this.m_Behaviours)
        {
          if ((Object) behaviour.m_State == (Object) state)
            return behaviour.m_Behaviours;
        }
      }
      return new StateMachineBehaviour[0];
    }

    public void SetOverrideBehaviours(AnimatorState state, StateMachineBehaviour[] behaviours)
    {
      if (this.m_Behaviours == null)
        this.m_Behaviours = new StateBehavioursPair[0];
      for (int index = 0; index < this.m_Behaviours.Length; ++index)
      {
        if ((Object) this.m_Behaviours[index].m_State == (Object) state)
        {
          this.m_Behaviours[index].m_Behaviours = behaviours;
          return;
        }
      }
      StateBehavioursPair stateBehavioursPair;
      stateBehavioursPair.m_State = state;
      stateBehavioursPair.m_Behaviours = behaviours;
      ArrayUtility.Add<StateBehavioursPair>(ref this.m_Behaviours, stateBehavioursPair);
    }
  }
}
