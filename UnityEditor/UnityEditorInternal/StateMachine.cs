// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.StateMachine
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditorInternal
{
  [Obsolete("StateMachine is obsolete. Use UnityEditor.Animations.AnimatorStateMachine instead (UnityUpgradable) -> UnityEditor.Animations.AnimatorStateMachine", true)]
  public class StateMachine : UnityEngine.Object
  {
    public State defaultState
    {
      get
      {
        return (State) null;
      }
      set
      {
      }
    }

    public Vector3 anyStatePosition
    {
      get
      {
        return new Vector3();
      }
      set
      {
      }
    }

    public Vector3 parentStateMachinePosition
    {
      get
      {
        return new Vector3();
      }
      set
      {
      }
    }

    public State GetState(int index)
    {
      return (State) null;
    }

    public State AddState(string stateName)
    {
      return (State) null;
    }

    public StateMachine GetStateMachine(int index)
    {
      return (StateMachine) null;
    }

    public StateMachine AddStateMachine(string stateMachineName)
    {
      return (StateMachine) null;
    }

    public Transition AddTransition(State src, State dst)
    {
      return (Transition) null;
    }

    public Transition AddAnyStateTransition(State dst)
    {
      return (Transition) null;
    }

    public Vector3 GetStateMachinePosition(int i)
    {
      return new Vector3();
    }

    public Transition[] GetTransitionsFromState(State srcState)
    {
      return (Transition[]) null;
    }
  }
}
