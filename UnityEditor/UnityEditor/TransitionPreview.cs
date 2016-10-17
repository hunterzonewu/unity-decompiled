// Decompiled with JetBrains decompiler
// Type: UnityEditor.TransitionPreview
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace UnityEditor
{
  internal class TransitionPreview
  {
    private List<Vector2> m_ParameterMinMax = new List<Vector2>();
    private TransitionPreview.TransitionInfo m_RefTransitionInfo = new TransitionPreview.TransitionInfo();
    private bool m_MustResample = true;
    private float m_LastEvalTime = -1f;
    private bool m_ValidTransition = true;
    private float m_LeftStateWeightB = 1f;
    private float m_LeftStateTimeB = 1f;
    private float m_RightStateWeightB = 1f;
    private float m_RightStateTimeB = 1f;
    private List<Timeline.PivotSample> m_SrcPivotList = new List<Timeline.PivotSample>();
    private List<Timeline.PivotSample> m_DstPivotList = new List<Timeline.PivotSample>();
    private AvatarPreview m_AvatarPreview;
    private Timeline m_Timeline;
    private AnimatorController m_Controller;
    private AnimatorStateMachine m_StateMachine;
    private List<TransitionPreview.ParameterInfo> m_ParameterInfoList;
    private AnimatorStateTransition m_RefTransition;
    private AnimatorStateTransition m_Transition;
    private AnimatorState m_SrcState;
    private AnimatorState m_DstState;
    private AnimatorState m_RefSrcState;
    private AnimatorState m_RefDstState;
    private Motion m_SrcMotion;
    private Motion m_DstMotion;
    private bool m_ShowBlendValue;
    private bool m_MustSampleMotions;
    private bool m_IsResampling;
    private AvatarMask m_LayerMask;
    private int m_LayerIndex;
    private float m_LeftStateWeightA;
    private float m_LeftStateTimeA;
    private float m_RightStateWeightA;
    private float m_RightStateTimeA;

    public bool mustResample
    {
      get
      {
        return this.m_MustResample;
      }
      set
      {
        this.m_MustResample = value;
      }
    }

    private int FindParameterInfo(List<TransitionPreview.ParameterInfo> parameterInfoList, string name)
    {
      int num = -1;
      for (int index = 0; index < parameterInfoList.Count && num == -1; ++index)
      {
        if (parameterInfoList[index].m_Name == name)
          num = index;
      }
      return num;
    }

    private void SetMotion(AnimatorState state, int layerIndex, Motion motion)
    {
      AnimatorControllerLayer[] layers = this.m_Controller.layers;
      state.motion = motion;
      this.m_Controller.layers = layers;
    }

    private void CopyStateForPreview(AnimatorState src, ref AnimatorState dst)
    {
      dst.iKOnFeet = src.iKOnFeet;
      dst.speed = src.speed;
      dst.mirror = src.mirror;
      dst.motion = src.motion;
    }

    private void CopyTransitionForPreview(AnimatorStateTransition src, ref AnimatorStateTransition dst)
    {
      if (!((UnityEngine.Object) src != (UnityEngine.Object) null))
        return;
      dst.duration = src.duration;
      dst.offset = src.offset;
      dst.exitTime = src.exitTime;
      dst.hasFixedDuration = src.hasFixedDuration;
    }

    private bool MustResample(TransitionPreview.TransitionInfo info)
    {
      if (!this.mustResample)
        return !info.IsEqual(this.m_RefTransitionInfo);
      return true;
    }

    private void WriteParametersInController()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Controller))
        return;
      int length = this.m_Controller.parameters.Length;
      for (int index = 0; index < length; ++index)
      {
        string name = this.m_Controller.parameters[index].name;
        int parameterInfo = this.FindParameterInfo(this.m_ParameterInfoList, name);
        if (parameterInfo != -1)
          this.m_AvatarPreview.Animator.SetFloat(name, this.m_ParameterInfoList[parameterInfo].m_Value);
      }
    }

    private void ResampleTransition(AnimatorStateTransition transition, AvatarMask layerMask, TransitionPreview.TransitionInfo info, Animator previewObject)
    {
      this.m_IsResampling = true;
      this.m_MustResample = false;
      bool flag1 = (UnityEngine.Object) this.m_RefTransition != (UnityEngine.Object) transition;
      this.m_RefTransition = transition;
      this.m_RefTransitionInfo = info;
      this.m_LayerMask = layerMask;
      if (this.m_AvatarPreview != null)
      {
        this.m_AvatarPreview.OnDestroy();
        this.m_AvatarPreview = (AvatarPreview) null;
      }
      this.ClearController();
      Motion motion = this.m_RefSrcState.motion;
      this.Init(previewObject, !((UnityEngine.Object) motion != (UnityEngine.Object) null) ? this.m_RefDstState.motion : motion);
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_AvatarPreview.Animator.allowConstantClipSamplingOptimization = false;
      this.m_StateMachine.defaultState = this.m_DstState;
      this.m_Transition.mute = true;
      AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
      this.m_AvatarPreview.Animator.Update(1E-05f);
      this.WriteParametersInController();
      this.m_AvatarPreview.Animator.SetLayerWeight(this.m_LayerIndex, 1f);
      float length1 = this.m_AvatarPreview.Animator.GetCurrentAnimatorStateInfo(this.m_LayerIndex).length;
      this.m_StateMachine.defaultState = this.m_SrcState;
      this.m_Transition.mute = false;
      AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
      this.m_AvatarPreview.Animator.Update(1E-05f);
      this.WriteParametersInController();
      this.m_AvatarPreview.Animator.SetLayerWeight(this.m_LayerIndex, 1f);
      float length2 = this.m_AvatarPreview.Animator.GetCurrentAnimatorStateInfo(this.m_LayerIndex).length;
      if (this.m_LayerIndex > 0)
        this.m_AvatarPreview.Animator.stabilizeFeet = false;
      float num1 = (float) ((double) length2 * (double) this.m_RefTransition.exitTime + (double) this.m_Transition.duration * (!this.m_RefTransition.hasFixedDuration ? (double) length2 : 1.0)) + length1;
      if ((double) num1 > 2000.0)
      {
        Debug.LogWarning((object) "Transition duration is longer than 2000 second, Disabling previewer.");
        this.m_ValidTransition = false;
      }
      else
      {
        float num2 = (double) this.m_RefTransition.exitTime <= 0.0 ? length2 : length2 * this.m_RefTransition.exitTime;
        float a1 = (double) num2 <= 0.0 ? 0.03333334f : Mathf.Min(Mathf.Max(num2 / 300f, 0.03333334f), num2 / 5f);
        float a2 = (double) length1 <= 0.0 ? 0.03333334f : Mathf.Min(Mathf.Max(length1 / 300f, 0.03333334f), length1 / 5f);
        float num3 = Mathf.Max(a1, num1 / 600f);
        float num4 = Mathf.Max(a2, num1 / 600f);
        float deltaTime = num3;
        float num5 = 0.0f;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        this.m_AvatarPreview.Animator.StartRecording(-1);
        this.m_LeftStateWeightA = 0.0f;
        this.m_LeftStateTimeA = 0.0f;
        this.m_AvatarPreview.Animator.Update(0.0f);
        while (!flag4)
        {
          this.m_AvatarPreview.Animator.Update(deltaTime);
          AnimatorStateInfo animatorStateInfo = this.m_AvatarPreview.Animator.GetCurrentAnimatorStateInfo(this.m_LayerIndex);
          num5 += deltaTime;
          if (!flag2)
          {
            this.m_LeftStateWeightA = this.m_LeftStateWeightB = animatorStateInfo.normalizedTime;
            this.m_LeftStateTimeA = this.m_LeftStateTimeB = num5;
            flag2 = true;
          }
          if (flag3 && (double) num5 >= (double) num1)
            flag4 = true;
          if (!flag3 && animatorStateInfo.IsName(this.m_DstState.name))
          {
            this.m_RightStateWeightA = animatorStateInfo.normalizedTime;
            this.m_RightStateTimeA = num5;
            flag3 = true;
          }
          if (!flag3)
          {
            this.m_LeftStateWeightB = animatorStateInfo.normalizedTime;
            this.m_LeftStateTimeB = num5;
          }
          if (flag3)
          {
            this.m_RightStateWeightB = animatorStateInfo.normalizedTime;
            this.m_RightStateTimeB = num5;
          }
          if (this.m_AvatarPreview.Animator.IsInTransition(this.m_LayerIndex))
            deltaTime = num4;
        }
        float num6 = num5;
        this.m_AvatarPreview.Animator.StopRecording();
        if (Mathf.Approximately(this.m_LeftStateWeightB, this.m_LeftStateWeightA) || Mathf.Approximately(this.m_RightStateWeightB, this.m_RightStateWeightA))
        {
          Debug.LogWarning((object) "Difference in effective length between states is too big. Transition preview will be disabled.");
          this.m_ValidTransition = false;
        }
        else
        {
          float num7 = (float) (((double) this.m_LeftStateTimeB - (double) this.m_LeftStateTimeA) / ((double) this.m_LeftStateWeightB - (double) this.m_LeftStateWeightA));
          float num8 = (float) (((double) this.m_RightStateTimeB - (double) this.m_RightStateTimeA) / ((double) this.m_RightStateWeightB - (double) this.m_RightStateWeightA));
          if (this.m_MustSampleMotions)
          {
            this.m_MustSampleMotions = false;
            this.m_SrcPivotList.Clear();
            this.m_DstPivotList.Clear();
            float num9 = num4;
            this.m_StateMachine.defaultState = this.m_DstState;
            this.m_Transition.mute = true;
            AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
            this.m_AvatarPreview.Animator.Update(0.0f);
            this.m_AvatarPreview.Animator.SetLayerWeight(this.m_LayerIndex, 1f);
            this.m_AvatarPreview.Animator.Update(1E-07f);
            this.WriteParametersInController();
            float num10 = 0.0f;
            while ((double) num10 <= (double) num8)
            {
              this.m_DstPivotList.Add(new Timeline.PivotSample()
              {
                m_Time = num10,
                m_Weight = this.m_AvatarPreview.Animator.pivotWeight
              });
              this.m_AvatarPreview.Animator.Update(num9 * 2f);
              num10 += num9 * 2f;
            }
            float num11 = num3;
            this.m_StateMachine.defaultState = this.m_SrcState;
            this.m_Transition.mute = true;
            AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
            this.m_AvatarPreview.Animator.Update(1E-07f);
            this.WriteParametersInController();
            this.m_AvatarPreview.Animator.SetLayerWeight(this.m_LayerIndex, 1f);
            float num12 = 0.0f;
            while ((double) num12 <= (double) num7)
            {
              this.m_SrcPivotList.Add(new Timeline.PivotSample()
              {
                m_Time = num12,
                m_Weight = this.m_AvatarPreview.Animator.pivotWeight
              });
              this.m_AvatarPreview.Animator.Update(num11 * 2f);
              num12 += num11 * 2f;
            }
            this.m_Transition.mute = false;
            AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
            this.m_AvatarPreview.Animator.Update(1E-07f);
            this.WriteParametersInController();
          }
          this.m_Timeline.StopTime = this.m_AvatarPreview.timeControl.stopTime = num6;
          this.m_AvatarPreview.timeControl.currentTime = this.m_Timeline.Time;
          if (flag1)
          {
            Timeline timeline = this.m_Timeline;
            float num9 = this.m_AvatarPreview.timeControl.currentTime = this.m_AvatarPreview.timeControl.startTime = 0.0f;
            this.m_Timeline.StartTime = num9;
            double num10 = (double) num9;
            timeline.Time = (float) num10;
            this.m_Timeline.ResetRange();
          }
          this.m_AvatarPreview.Animator.StartPlayback();
          this.m_IsResampling = false;
        }
      }
    }

    public void SetTransition(AnimatorStateTransition transition, AnimatorState sourceState, AnimatorState destinationState, AnimatorControllerLayer srcLayer, Animator previewObject)
    {
      this.m_RefSrcState = sourceState;
      this.m_RefDstState = destinationState;
      TransitionPreview.TransitionInfo info = new TransitionPreview.TransitionInfo();
      info.Set(transition, sourceState, destinationState);
      if (!this.MustResample(info))
        return;
      this.ResampleTransition(transition, srcLayer.avatarMask, info, previewObject);
    }

    private void OnPreviewAvatarChanged()
    {
      this.m_RefTransitionInfo = new TransitionPreview.TransitionInfo();
      this.ClearController();
      this.CreateController();
      this.CreateParameterInfoList();
    }

    private void ClearController()
    {
      if (this.m_AvatarPreview != null && (UnityEngine.Object) this.m_AvatarPreview.Animator != (UnityEngine.Object) null)
        AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, (AnimatorController) null);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Controller);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_SrcState);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_DstState);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Transition);
      this.m_StateMachine = (AnimatorStateMachine) null;
      this.m_Controller = (AnimatorController) null;
      this.m_SrcState = (AnimatorState) null;
      this.m_DstState = (AnimatorState) null;
      this.m_Transition = (AnimatorStateTransition) null;
    }

    private void CreateParameterInfoList()
    {
      this.m_ParameterInfoList = new List<TransitionPreview.ParameterInfo>();
      if (!(bool) ((UnityEngine.Object) this.m_Controller) || this.m_Controller.parameters == null)
        return;
      int length = this.m_Controller.parameters.Length;
      for (int index = 0; index < length; ++index)
        this.m_ParameterInfoList.Add(new TransitionPreview.ParameterInfo()
        {
          m_Name = this.m_Controller.parameters[index].name
        });
    }

    private void CreateController()
    {
      if (!((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null) || this.m_AvatarPreview == null || (!((UnityEngine.Object) this.m_AvatarPreview.Animator != (UnityEngine.Object) null) || !((UnityEngine.Object) this.m_RefTransition != (UnityEngine.Object) null)))
        return;
      this.m_LayerIndex = 0;
      this.m_Controller = new AnimatorController();
      this.m_Controller.pushUndo = false;
      this.m_Controller.hideFlags = HideFlags.HideAndDontSave;
      this.m_Controller.AddLayer("preview");
      bool flag = true;
      if ((UnityEngine.Object) this.m_LayerMask != (UnityEngine.Object) null)
      {
        for (AvatarMaskBodyPart index = AvatarMaskBodyPart.Root; index < AvatarMaskBodyPart.LastBodyPart && flag; ++index)
        {
          if (!this.m_LayerMask.GetHumanoidBodyPartActive(index))
            flag = false;
        }
        if (!flag)
        {
          this.m_Controller.AddLayer("Additionnal");
          ++this.m_LayerIndex;
          AnimatorControllerLayer[] layers = this.m_Controller.layers;
          layers[this.m_LayerIndex].avatarMask = this.m_LayerMask;
          this.m_Controller.layers = layers;
        }
      }
      this.m_StateMachine = this.m_Controller.layers[this.m_LayerIndex].stateMachine;
      this.m_StateMachine.pushUndo = false;
      this.m_StateMachine.hideFlags = HideFlags.HideAndDontSave;
      this.m_SrcMotion = this.m_RefSrcState.motion;
      this.m_DstMotion = this.m_RefDstState.motion;
      this.m_ParameterMinMax.Clear();
      if ((bool) ((UnityEngine.Object) this.m_SrcMotion) && this.m_SrcMotion is BlendTree)
      {
        BlendTree srcMotion = this.m_SrcMotion as BlendTree;
        for (int index = 0; index < srcMotion.recursiveBlendParameterCount; ++index)
        {
          string recursiveBlendParameter = srcMotion.GetRecursiveBlendParameter(index);
          if (this.m_Controller.IndexOfParameter(recursiveBlendParameter) == -1)
          {
            this.m_Controller.AddParameter(recursiveBlendParameter, AnimatorControllerParameterType.Float);
            this.m_ParameterMinMax.Add(new Vector2(srcMotion.GetRecursiveBlendParameterMin(index), srcMotion.GetRecursiveBlendParameterMax(index)));
          }
        }
      }
      if ((bool) ((UnityEngine.Object) this.m_DstMotion) && this.m_DstMotion is BlendTree)
      {
        BlendTree dstMotion = this.m_DstMotion as BlendTree;
        for (int index1 = 0; index1 < dstMotion.recursiveBlendParameterCount; ++index1)
        {
          string recursiveBlendParameter = dstMotion.GetRecursiveBlendParameter(index1);
          int index2 = this.m_Controller.IndexOfParameter(recursiveBlendParameter);
          if (index2 == -1)
          {
            this.m_Controller.AddParameter(recursiveBlendParameter, AnimatorControllerParameterType.Float);
            this.m_ParameterMinMax.Add(new Vector2(dstMotion.GetRecursiveBlendParameterMin(index1), dstMotion.GetRecursiveBlendParameterMax(index1)));
          }
          else
            this.m_ParameterMinMax[index2] = new Vector2(Mathf.Min(dstMotion.GetRecursiveBlendParameterMin(index1), this.m_ParameterMinMax[index2][0]), Mathf.Max(dstMotion.GetRecursiveBlendParameterMax(index1), this.m_ParameterMinMax[index2][1]));
        }
      }
      this.m_SrcState = this.m_StateMachine.AddState(this.m_RefSrcState.name);
      this.m_SrcState.pushUndo = false;
      this.m_SrcState.hideFlags = HideFlags.HideAndDontSave;
      this.m_DstState = this.m_StateMachine.AddState(this.m_RefDstState.name);
      this.m_DstState.pushUndo = false;
      this.m_DstState.hideFlags = HideFlags.HideAndDontSave;
      this.CopyStateForPreview(this.m_RefSrcState, ref this.m_SrcState);
      this.CopyStateForPreview(this.m_RefDstState, ref this.m_DstState);
      this.m_Transition = this.m_SrcState.AddTransition(this.m_DstState, true);
      this.m_Transition.pushUndo = false;
      this.m_Transition.hideFlags = HideFlags.DontSave;
      this.CopyTransitionForPreview(this.m_RefTransition, ref this.m_Transition);
      this.DisableIKOnFeetIfNeeded();
      AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
      this.m_Controller.OnAnimatorControllerDirty += new System.Action(this.ControllerDirty);
    }

    private void ControllerDirty()
    {
      if (this.m_IsResampling)
        return;
      this.m_MustResample = true;
    }

    private void DisableIKOnFeetIfNeeded()
    {
      bool flag = false;
      if ((UnityEngine.Object) this.m_SrcMotion == (UnityEngine.Object) null || (UnityEngine.Object) this.m_DstMotion == (UnityEngine.Object) null)
        flag = true;
      if (this.m_LayerIndex > 0)
        flag = !this.m_LayerMask.hasFeetIK;
      if (!flag)
        return;
      this.m_SrcState.iKOnFeet = false;
      this.m_DstState.iKOnFeet = false;
    }

    private void Init(Animator scenePreviewObject, Motion motion)
    {
      if (this.m_AvatarPreview == null)
      {
        this.m_AvatarPreview = new AvatarPreview(scenePreviewObject, motion);
        this.m_AvatarPreview.OnAvatarChangeFunc = new AvatarPreview.OnAvatarChange(this.OnPreviewAvatarChanged);
        this.m_AvatarPreview.ShowIKOnFeetButton = false;
      }
      if (this.m_Timeline == null)
      {
        this.m_Timeline = new Timeline();
        this.m_MustSampleMotions = true;
      }
      this.CreateController();
      if (this.m_ParameterInfoList != null)
        return;
      this.CreateParameterInfoList();
    }

    public void DoTransitionPreview()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      if (Event.current.type == EventType.Repaint)
        this.m_AvatarPreview.timeControl.Update();
      this.DoTimeline();
      AnimatorControllerParameter[] parameters = this.m_Controller.parameters;
      if (parameters.Length <= 0)
        return;
      this.m_ShowBlendValue = EditorGUILayout.Foldout(this.m_ShowBlendValue, "BlendTree Parameters");
      if (!this.m_ShowBlendValue)
        return;
      for (int index = 0; index < parameters.Length; ++index)
      {
        AnimatorControllerParameter parameter = this.m_Controller.parameters[index];
        float num1 = this.m_ParameterInfoList[index].m_Value;
        float num2 = EditorGUILayout.Slider(parameter.name, num1, this.m_ParameterMinMax[index][0], this.m_ParameterMinMax[index][1], new GUILayoutOption[0]);
        if ((double) num2 != (double) num1)
        {
          this.m_ParameterInfoList[index].m_Value = num2;
          this.mustResample = true;
          this.m_MustSampleMotions = true;
        }
      }
    }

    private void DoTimeline()
    {
      if (!this.m_ValidTransition)
        return;
      float num1 = (float) (((double) this.m_LeftStateTimeB - (double) this.m_LeftStateTimeA) / ((double) this.m_LeftStateWeightB - (double) this.m_LeftStateWeightA));
      float num2 = (float) (((double) this.m_RightStateTimeB - (double) this.m_RightStateTimeA) / ((double) this.m_RightStateWeightB - (double) this.m_RightStateWeightA));
      float num3 = this.m_Transition.duration * (!this.m_RefTransition.hasFixedDuration ? num1 : 1f);
      this.m_Timeline.SrcStartTime = 0.0f;
      this.m_Timeline.SrcStopTime = num1;
      this.m_Timeline.SrcName = this.m_RefSrcState.name;
      this.m_Timeline.HasExitTime = this.m_RefTransition.hasExitTime;
      this.m_Timeline.srcLoop = (bool) ((UnityEngine.Object) this.m_SrcMotion) && this.m_SrcMotion.isLooping;
      this.m_Timeline.dstLoop = (bool) ((UnityEngine.Object) this.m_DstMotion) && this.m_DstMotion.isLooping;
      this.m_Timeline.TransitionStartTime = this.m_RefTransition.exitTime * num1;
      this.m_Timeline.TransitionStopTime = this.m_Timeline.TransitionStartTime + num3;
      this.m_Timeline.Time = this.m_AvatarPreview.timeControl.currentTime;
      this.m_Timeline.DstStartTime = this.m_Timeline.TransitionStartTime - this.m_RefTransition.offset * num2;
      this.m_Timeline.DstStopTime = this.m_Timeline.DstStartTime + num2;
      this.m_Timeline.SampleStopTime = this.m_AvatarPreview.timeControl.stopTime;
      if ((double) this.m_Timeline.TransitionStopTime == double.PositiveInfinity)
        this.m_Timeline.TransitionStopTime = Mathf.Min(this.m_Timeline.DstStopTime, this.m_Timeline.SrcStopTime);
      this.m_Timeline.DstName = this.m_RefDstState.name;
      this.m_Timeline.SrcPivotList = this.m_SrcPivotList;
      this.m_Timeline.DstPivotList = this.m_DstPivotList;
      Rect controlRect = EditorGUILayout.GetControlRect(false, 150f, EditorStyles.label, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      bool flag = this.m_Timeline.DoTimeline(controlRect);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (flag)
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_RefTransition, "Edit Transition");
        this.m_RefTransition.exitTime = this.m_Timeline.TransitionStartTime / this.m_Timeline.SrcDuration;
        this.m_RefTransition.duration = this.m_Timeline.TransitionDuration / (!this.m_RefTransition.hasFixedDuration ? this.m_Timeline.SrcDuration : 1f);
        this.m_RefTransition.offset = (this.m_Timeline.TransitionStartTime - this.m_Timeline.DstStartTime) / this.m_Timeline.DstDuration;
      }
      this.m_AvatarPreview.timeControl.nextCurrentTime = Mathf.Clamp(this.m_Timeline.Time, 0.0f, this.m_AvatarPreview.timeControl.stopTime);
    }

    public void OnDisable()
    {
      this.ClearController();
    }

    public void OnDestroy()
    {
      this.ClearController();
      if (this.m_Timeline != null)
        this.m_Timeline = (Timeline) null;
      if (this.m_AvatarPreview == null)
        return;
      this.m_AvatarPreview.OnDestroy();
      this.m_AvatarPreview = (AvatarPreview) null;
    }

    public bool HasPreviewGUI()
    {
      return true;
    }

    public void OnPreviewSettings()
    {
      if (this.m_AvatarPreview == null)
        return;
      this.m_AvatarPreview.DoPreviewSettings();
    }

    public void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      if (this.m_AvatarPreview == null || !((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null))
        return;
      if ((double) this.m_LastEvalTime != (double) this.m_AvatarPreview.timeControl.currentTime && Event.current.type == EventType.Repaint)
      {
        this.m_AvatarPreview.Animator.playbackTime = this.m_AvatarPreview.timeControl.currentTime;
        this.m_AvatarPreview.Animator.Update(0.0f);
        this.m_LastEvalTime = this.m_AvatarPreview.timeControl.currentTime;
      }
      this.m_AvatarPreview.DoAvatarPreview(r, background);
    }

    private class ParameterInfo
    {
      public string m_Name;
      public float m_Value;
    }

    private class TransitionInfo
    {
      private AnimatorState m_SrcState;
      private AnimatorState m_DstState;
      private float m_TransitionDuration;
      private float m_TransitionOffset;
      private float m_ExitTime;

      public TransitionInfo()
      {
        this.Init();
      }

      public bool IsEqual(TransitionPreview.TransitionInfo info)
      {
        if ((UnityEngine.Object) this.m_SrcState == (UnityEngine.Object) info.m_SrcState && (UnityEngine.Object) this.m_DstState == (UnityEngine.Object) info.m_DstState && (Mathf.Approximately(this.m_TransitionDuration, info.m_TransitionDuration) && Mathf.Approximately(this.m_TransitionOffset, info.m_TransitionOffset)))
          return Mathf.Approximately(this.m_ExitTime, info.m_ExitTime);
        return false;
      }

      private void Init()
      {
        this.m_SrcState = (AnimatorState) null;
        this.m_DstState = (AnimatorState) null;
        this.m_TransitionDuration = 0.0f;
        this.m_TransitionOffset = 0.0f;
        this.m_ExitTime = 0.5f;
      }

      public void Set(AnimatorStateTransition transition, AnimatorState srcState, AnimatorState dstState)
      {
        if ((UnityEngine.Object) transition != (UnityEngine.Object) null)
        {
          this.m_SrcState = srcState;
          this.m_DstState = dstState;
          this.m_TransitionDuration = transition.duration;
          this.m_TransitionOffset = transition.offset;
          this.m_ExitTime = 0.5f;
        }
        else
          this.Init();
      }
    }
  }
}
