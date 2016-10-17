// Decompiled with JetBrains decompiler
// Type: UnityEditor.VisualizationBlendTree
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Animations;
using UnityEngine;

namespace UnityEditor
{
  internal class VisualizationBlendTree
  {
    private AnimatorController m_Controller;
    private AnimatorStateMachine m_StateMachine;
    private AnimatorState m_State;
    private BlendTree m_BlendTree;
    private Animator m_Animator;
    private bool m_ControllerIsDirty;

    public Animator animator
    {
      get
      {
        return this.m_Animator;
      }
    }

    public bool controllerDirty
    {
      get
      {
        return this.m_ControllerIsDirty;
      }
    }

    public void Init(BlendTree blendTree, Animator animator)
    {
      this.m_BlendTree = blendTree;
      this.m_Animator = animator;
      this.m_Animator.logWarnings = false;
      this.m_Animator.fireEvents = false;
      this.m_Animator.enabled = false;
      this.m_Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
      this.CreateStateMachine();
    }

    protected virtual void ControllerDirty()
    {
      this.m_ControllerIsDirty = true;
    }

    private void CreateParameters()
    {
      for (int index = 0; index < this.m_BlendTree.recursiveBlendParameterCount; ++index)
        this.m_Controller.AddParameter(this.m_BlendTree.GetRecursiveBlendParameter(index), AnimatorControllerParameterType.Float);
    }

    private void CreateStateMachine()
    {
      if (!((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null))
        return;
      this.m_Controller = new AnimatorController();
      this.m_Controller.pushUndo = false;
      this.m_Controller.AddLayer("viz");
      this.m_StateMachine = this.m_Controller.layers[0].stateMachine;
      this.m_StateMachine.pushUndo = false;
      this.CreateParameters();
      this.m_State = this.m_StateMachine.AddState("viz");
      this.m_State.pushUndo = false;
      this.m_State.motion = (Motion) this.m_BlendTree;
      this.m_State.iKOnFeet = false;
      this.m_State.hideFlags = HideFlags.HideAndDontSave;
      this.m_StateMachine.hideFlags = HideFlags.HideAndDontSave;
      this.m_Controller.hideFlags = HideFlags.HideAndDontSave;
      AnimatorController.SetAnimatorController(this.m_Animator, this.m_Controller);
      this.m_Controller.OnAnimatorControllerDirty += new System.Action(this.ControllerDirty);
      this.m_ControllerIsDirty = false;
    }

    private void ClearStateMachine()
    {
      if ((UnityEngine.Object) this.m_Animator != (UnityEngine.Object) null)
        AnimatorController.SetAnimatorController(this.m_Animator, (AnimatorController) null);
      if ((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null)
        this.m_Controller.OnAnimatorControllerDirty -= new System.Action(this.ControllerDirty);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Controller);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_State);
      this.m_StateMachine = (AnimatorStateMachine) null;
      this.m_Controller = (AnimatorController) null;
      this.m_State = (AnimatorState) null;
    }

    public void Reset()
    {
      this.ClearStateMachine();
      this.CreateStateMachine();
    }

    public void Destroy()
    {
      this.ClearStateMachine();
    }

    public void Update()
    {
      if (this.m_ControllerIsDirty)
        this.Reset();
      int blendParameterCount = this.m_BlendTree.recursiveBlendParameterCount;
      if (this.m_Controller.parameters.Length < blendParameterCount)
        return;
      for (int index = 0; index < blendParameterCount; ++index)
      {
        string recursiveBlendParameter = this.m_BlendTree.GetRecursiveBlendParameter(index);
        float inputBlendValue = this.m_BlendTree.GetInputBlendValue(recursiveBlendParameter);
        this.animator.SetFloat(recursiveBlendParameter, inputBlendValue);
      }
      this.animator.EvaluateController();
    }
  }
}
