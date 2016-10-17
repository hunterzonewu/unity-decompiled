// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.ChildAnimatorStateMachine
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Animations
{
  /// <summary>
  ///   <para>Structure that represents a state machine in the context of its parent state machine.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct ChildAnimatorStateMachine
  {
    private AnimatorStateMachine m_StateMachine;
    private Vector3 m_Position;

    /// <summary>
    ///   <para>The state machine.</para>
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
    ///   <para>The position the the state machine node in the context of its parent state machine.</para>
    /// </summary>
    public Vector3 position
    {
      get
      {
        return this.m_Position;
      }
      set
      {
        this.m_Position = value;
      }
    }
  }
}
