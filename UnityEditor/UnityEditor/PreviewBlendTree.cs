// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreviewBlendTree
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Animations;
using UnityEngine;

namespace UnityEditor
{
  internal class PreviewBlendTree
  {
    private AnimatorController m_Controller;
    private AvatarPreview m_AvatarPreview;
    private AnimatorStateMachine m_StateMachine;
    private AnimatorState m_State;
    private BlendTree m_BlendTree;
    private bool m_ControllerIsDirty;
    private bool m_PrevIKOnFeet;

    public Animator PreviewAnimator
    {
      get
      {
        return this.m_AvatarPreview.Animator;
      }
    }

    protected virtual void ControllerDirty()
    {
      this.m_ControllerIsDirty = true;
    }

    public void Init(BlendTree blendTree, Animator animator)
    {
      this.m_BlendTree = blendTree;
      if (this.m_AvatarPreview == null)
      {
        this.m_AvatarPreview = new AvatarPreview(animator, (Motion) this.m_BlendTree);
        this.m_AvatarPreview.OnAvatarChangeFunc = new AvatarPreview.OnAvatarChange(this.OnPreviewAvatarChanged);
        this.m_PrevIKOnFeet = this.m_AvatarPreview.IKOnFeet;
      }
      this.CreateStateMachine();
    }

    public void CreateParameters()
    {
      for (int index = 0; index < this.m_BlendTree.recursiveBlendParameterCount; ++index)
        this.m_Controller.AddParameter(this.m_BlendTree.GetRecursiveBlendParameter(index), AnimatorControllerParameterType.Float);
    }

    private void CreateStateMachine()
    {
      if (this.m_AvatarPreview == null || !((UnityEngine.Object) this.m_AvatarPreview.Animator != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
      {
        this.m_Controller = new AnimatorController();
        this.m_Controller.pushUndo = false;
        this.m_Controller.AddLayer("preview");
        this.m_StateMachine = this.m_Controller.layers[0].stateMachine;
        this.m_StateMachine.pushUndo = false;
        this.CreateParameters();
        this.m_State = this.m_StateMachine.AddState("preview");
        this.m_State.pushUndo = false;
        this.m_State.motion = (Motion) this.m_BlendTree;
        this.m_State.iKOnFeet = this.m_AvatarPreview.IKOnFeet;
        this.m_State.hideFlags = HideFlags.HideAndDontSave;
        this.m_Controller.hideFlags = HideFlags.HideAndDontSave;
        this.m_StateMachine.hideFlags = HideFlags.HideAndDontSave;
        AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
        this.m_Controller.OnAnimatorControllerDirty += new System.Action(this.ControllerDirty);
        this.m_ControllerIsDirty = false;
      }
      if (!((UnityEngine.Object) AnimatorController.GetEffectiveAnimatorController(this.m_AvatarPreview.Animator) != (UnityEngine.Object) this.m_Controller))
        return;
      AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
    }

    private void ClearStateMachine()
    {
      if (this.m_AvatarPreview != null && (UnityEngine.Object) this.m_AvatarPreview.Animator != (UnityEngine.Object) null)
        AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, (AnimatorController) null);
      if ((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null)
        this.m_Controller.OnAnimatorControllerDirty -= new System.Action(this.ControllerDirty);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Controller);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_State);
      this.m_StateMachine = (AnimatorStateMachine) null;
      this.m_Controller = (AnimatorController) null;
      this.m_State = (AnimatorState) null;
    }

    private void OnPreviewAvatarChanged()
    {
      this.ResetStateMachine();
    }

    public void ResetStateMachine()
    {
      this.ClearStateMachine();
      this.CreateStateMachine();
    }

    public void OnDisable()
    {
      this.ClearStateMachine();
      this.m_AvatarPreview.OnDestroy();
    }

    public void OnDestroy()
    {
      this.ClearStateMachine();
      if (this.m_AvatarPreview == null)
        return;
      this.m_AvatarPreview.OnDestroy();
      this.m_AvatarPreview = (AvatarPreview) null;
    }

    private void UpdateAvatarState()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if ((UnityEngine.Object) this.m_AvatarPreview.PreviewObject == (UnityEngine.Object) null || this.m_ControllerIsDirty)
      {
        this.m_AvatarPreview.ResetPreviewInstance();
        if ((bool) ((UnityEngine.Object) this.m_AvatarPreview.PreviewObject))
          this.ResetStateMachine();
      }
      if (!(bool) ((UnityEngine.Object) this.m_AvatarPreview.Animator))
        return;
      if (this.m_PrevIKOnFeet != this.m_AvatarPreview.IKOnFeet)
      {
        this.m_PrevIKOnFeet = this.m_AvatarPreview.IKOnFeet;
        Vector3 rootPosition = this.m_AvatarPreview.Animator.rootPosition;
        Quaternion rootRotation = this.m_AvatarPreview.Animator.rootRotation;
        this.ResetStateMachine();
        this.m_AvatarPreview.Animator.Update(this.m_AvatarPreview.timeControl.currentTime);
        this.m_AvatarPreview.Animator.Update(0.0f);
        this.m_AvatarPreview.Animator.rootPosition = rootPosition;
        this.m_AvatarPreview.Animator.rootRotation = rootRotation;
      }
      if ((bool) ((UnityEngine.Object) this.m_AvatarPreview.Animator))
      {
        for (int index = 0; index < this.m_BlendTree.recursiveBlendParameterCount; ++index)
        {
          string recursiveBlendParameter = this.m_BlendTree.GetRecursiveBlendParameter(index);
          float inputBlendValue = this.m_BlendTree.GetInputBlendValue(recursiveBlendParameter);
          this.m_AvatarPreview.Animator.SetFloat(recursiveBlendParameter, inputBlendValue);
        }
      }
      this.m_AvatarPreview.timeControl.loop = true;
      float num1 = 1f;
      float num2 = 0.0f;
      if (this.m_AvatarPreview.Animator.layerCount > 0)
      {
        AnimatorStateInfo animatorStateInfo = this.m_AvatarPreview.Animator.GetCurrentAnimatorStateInfo(0);
        num1 = animatorStateInfo.length;
        num2 = animatorStateInfo.normalizedTime;
      }
      this.m_AvatarPreview.timeControl.startTime = 0.0f;
      this.m_AvatarPreview.timeControl.stopTime = num1;
      this.m_AvatarPreview.timeControl.Update();
      float deltaTime = this.m_AvatarPreview.timeControl.deltaTime;
      if (!this.m_BlendTree.isLooping)
      {
        if ((double) num2 >= 1.0)
          deltaTime -= num1;
        else if ((double) num2 < 0.0)
          deltaTime += num1;
      }
      this.m_AvatarPreview.Animator.Update(deltaTime);
    }

    public void TestForReset()
    {
      if (!((UnityEngine.Object) this.m_State != (UnityEngine.Object) null) || this.m_AvatarPreview == null || this.m_State.iKOnFeet == this.m_AvatarPreview.IKOnFeet)
        return;
      this.ResetStateMachine();
    }

    public bool HasPreviewGUI()
    {
      return true;
    }

    public void OnPreviewSettings()
    {
      this.m_AvatarPreview.DoPreviewSettings();
    }

    public void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      this.UpdateAvatarState();
      this.m_AvatarPreview.DoAvatarPreview(r, background);
    }
  }
}
