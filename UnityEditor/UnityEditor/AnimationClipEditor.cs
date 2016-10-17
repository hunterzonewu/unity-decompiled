// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationClipEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AnimationClip))]
  internal class AnimationClipEditor : Editor
  {
    private static GUIContent s_GreenLightIcon = EditorGUIUtility.IconContent("lightMeter/greenLight");
    private static GUIContent s_LightRimIcon = EditorGUIUtility.IconContent("lightMeter/lightRim");
    private static GUIContent s_OrangeLightIcon = EditorGUIUtility.IconContent("lightMeter/orangeLight");
    private static GUIContent s_RedLightIcon = EditorGUIUtility.IconContent("lightMeter/redLight");
    private static string s_LoopMeterStr = "LoopMeter";
    private static int s_LoopMeterHint = AnimationClipEditor.s_LoopMeterStr.GetHashCode();
    private static string s_LoopOrientationMeterStr = "LoopOrientationMeter";
    private static int s_LoopOrientationMeterHint = AnimationClipEditor.s_LoopOrientationMeterStr.GetHashCode();
    private static string s_LoopPositionYMeterStr = "LoopPostionYMeter";
    private static int s_LoopPositionYMeterHint = AnimationClipEditor.s_LoopPositionYMeterStr.GetHashCode();
    private static string s_LoopPositionXZMeterStr = "LoopPostionXZMeter";
    private static int s_LoopPositionXZMeterHint = AnimationClipEditor.s_LoopPositionXZMeterStr.GetHashCode();
    public static float s_EventTimelineMax = 1.05f;
    private static bool m_ShowCurves = false;
    private static bool m_ShowEvents = false;
    private static GUIContent prevKeyContent = EditorGUIUtility.IconContent("Animation.PrevKey", "|Go to previous key frame.");
    private static GUIContent nextKeyContent = EditorGUIUtility.IconContent("Animation.NextKey", "|Go to next key frame.");
    private static GUIContent addKeyframeContent = EditorGUIUtility.IconContent("Animation.AddKeyframe", "|Add Keyframe.");
    private float m_StopFrame = 1f;
    private Vector2[][][] m_QualityCurves = new Vector2[4][][];
    private const int kSamplesPerSecond = 60;
    private const int kPose = 0;
    private const int kRotation = 1;
    private const int kHeight = 2;
    private const int kPosition = 3;
    private static AnimationClipEditor.Styles styles;
    private AvatarMask m_Mask;
    private AnimationClipInfoProperties m_ClipInfo;
    private AnimationClip m_Clip;
    private UnityEditor.Animations.AnimatorController m_Controller;
    private AnimatorStateMachine m_StateMachine;
    private AnimatorState m_State;
    private AvatarPreview m_AvatarPreview;
    private TimeArea m_TimeArea;
    private TimeArea m_EventTimeArea;
    private bool m_DraggingRange;
    private bool m_DraggingRangeBegin;
    private bool m_DraggingRangeEnd;
    private float m_DraggingStartFrame;
    private float m_DraggingStopFrame;
    private float m_DraggingAdditivePoseFrame;
    private bool m_LoopTime;
    private bool m_LoopBlend;
    private bool m_LoopBlendOrientation;
    private bool m_LoopBlendPositionY;
    private bool m_LoopBlendPositionXZ;
    private float m_StartFrame;
    private float m_AdditivePoseFrame;
    private EventManipulationHandler m_EventManipulationHandler;
    private bool m_DirtyQualityCurves;

    public AvatarMask mask
    {
      get
      {
        return this.m_Mask;
      }
      set
      {
        this.m_Mask = value;
      }
    }

    public string[] takeNames { get; set; }

    public int takeIndex { get; set; }

    internal static void EditWithImporter(AnimationClip clip)
    {
      ModelImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) clip)) as ModelImporter;
      if (!(bool) ((UnityEngine.Object) atPath))
        return;
      Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(atPath.assetPath);
      EditorPrefs.SetInt((Editor.CreateEditor((UnityEngine.Object) atPath) as ModelImporterEditor).GetType().Name + "ActiveEditorIndex", 2);
      int num = 0;
      ModelImporterClipAnimation[] clipAnimations = atPath.clipAnimations;
      for (int index = 0; index < clipAnimations.Length; ++index)
      {
        if (clipAnimations[index].name == clip.name)
          num = index;
      }
      EditorPrefs.SetInt("ModelImporterClipEditor.ActiveClipIndex", num);
    }

    private void UpdateEventsPopupClipInfo(AnimationClipInfoProperties info)
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (AnimationEventPopup));
      AnimationEventPopup animationEventPopup = objectsOfTypeAll.Length <= 0 ? (AnimationEventPopup) null : (AnimationEventPopup) objectsOfTypeAll[0];
      if (!(bool) ((UnityEngine.Object) animationEventPopup) || animationEventPopup.clipInfo != this.m_ClipInfo)
        return;
      animationEventPopup.clipInfo = info;
    }

    public void ShowRange(AnimationClipInfoProperties info)
    {
      this.UpdateEventsPopupClipInfo(info);
      this.m_ClipInfo = info;
      info.AssignToPreviewClip(this.m_Clip);
    }

    private void InitController()
    {
      if (this.m_AvatarPreview == null || !((UnityEngine.Object) this.m_AvatarPreview.Animator != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
      {
        this.m_Controller = new UnityEditor.Animations.AnimatorController();
        this.m_Controller.pushUndo = false;
        this.m_Controller.hideFlags = HideFlags.HideAndDontSave;
        this.m_Controller.AddLayer("preview");
        this.m_StateMachine = this.m_Controller.layers[0].stateMachine;
        this.m_StateMachine.pushUndo = false;
        this.m_StateMachine.hideFlags = HideFlags.HideAndDontSave;
        if ((UnityEngine.Object) this.mask != (UnityEngine.Object) null)
        {
          UnityEditor.Animations.AnimatorControllerLayer[] layers = this.m_Controller.layers;
          layers[0].avatarMask = this.mask;
          this.m_Controller.layers = layers;
        }
      }
      if ((UnityEngine.Object) this.m_State == (UnityEngine.Object) null)
      {
        this.m_State = this.m_StateMachine.AddState("preview");
        this.m_State.pushUndo = false;
        UnityEditor.Animations.AnimatorControllerLayer[] layers = this.m_Controller.layers;
        this.m_State.motion = (Motion) this.m_Clip;
        this.m_Controller.layers = layers;
        this.m_State.iKOnFeet = this.m_AvatarPreview.IKOnFeet;
        this.m_State.hideFlags = HideFlags.HideAndDontSave;
      }
      UnityEditor.Animations.AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
      if (!((UnityEngine.Object) UnityEditor.Animations.AnimatorController.GetEffectiveAnimatorController(this.m_AvatarPreview.Animator) != (UnityEngine.Object) this.m_Controller))
        return;
      UnityEditor.Animations.AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
    }

    internal override void OnHeaderIconGUI(Rect iconRect)
    {
      bool flag = AssetPreview.IsLoadingAssetPreview(this.target.GetInstanceID());
      Texture2D texture2D = AssetPreview.GetAssetPreview(this.target);
      if (!(bool) ((UnityEngine.Object) texture2D))
      {
        if (flag)
          this.Repaint();
        texture2D = AssetPreview.GetMiniThumbnail(this.target);
      }
      GUI.DrawTexture(iconRect, (Texture) texture2D);
    }

    internal override void OnHeaderTitleGUI(Rect titleRect, string header)
    {
      if (this.m_ClipInfo != null)
        this.m_ClipInfo.name = EditorGUI.DelayedTextField(titleRect, this.m_ClipInfo.name, EditorStyles.textField);
      else
        base.OnHeaderTitleGUI(titleRect, header);
    }

    internal override void OnHeaderControlsGUI()
    {
      if (this.m_ClipInfo != null && this.takeNames != null && this.takeNames.Length > 1)
      {
        EditorGUIUtility.labelWidth = 80f;
        this.takeIndex = EditorGUILayout.Popup("Source Take", this.takeIndex, this.takeNames, new GUILayoutOption[0]);
      }
      else
      {
        base.OnHeaderControlsGUI();
        if (!((UnityEngine.Object) (AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(this.target)) as ModelImporter) != (UnityEngine.Object) null) || this.m_ClipInfo != null)
          return;
        if (!GUILayout.Button("Edit...", EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
          return;
        AnimationClipEditor.EditWithImporter(this.target as AnimationClip);
      }
    }

    private void DestroyController()
    {
      if (this.m_AvatarPreview != null && (UnityEngine.Object) this.m_AvatarPreview.Animator != (UnityEngine.Object) null)
        UnityEditor.Animations.AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, (UnityEditor.Animations.AnimatorController) null);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Controller);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_State);
      this.m_Controller = (UnityEditor.Animations.AnimatorController) null;
      this.m_StateMachine = (AnimatorStateMachine) null;
      this.m_State = (AnimatorState) null;
    }

    private void SetPreviewAvatar()
    {
      this.DestroyController();
      this.InitController();
    }

    private void Init()
    {
      if (AnimationClipEditor.styles == null)
        AnimationClipEditor.styles = new AnimationClipEditor.Styles();
      if (this.m_AvatarPreview != null)
        return;
      this.m_AvatarPreview = new AvatarPreview((Animator) null, this.target as Motion);
      this.m_AvatarPreview.OnAvatarChangeFunc = new AvatarPreview.OnAvatarChange(this.SetPreviewAvatar);
      this.m_AvatarPreview.fps = Mathf.RoundToInt((this.target as AnimationClip).frameRate);
      this.m_AvatarPreview.ShowIKOnFeetButton = (this.target as Motion).isHumanMotion;
    }

    private void OnEnable()
    {
      if (AnimationClipEditor.styles == null)
        AnimationClipEditor.styles = new AnimationClipEditor.Styles();
      this.m_Clip = this.target as AnimationClip;
      if (this.m_TimeArea == null)
      {
        this.m_TimeArea = new TimeArea(true);
        this.m_TimeArea.hRangeLocked = false;
        this.m_TimeArea.vRangeLocked = true;
        this.m_TimeArea.hSlider = true;
        this.m_TimeArea.vSlider = false;
        this.m_TimeArea.hRangeMin = this.m_Clip.startTime;
        this.m_TimeArea.hRangeMax = this.m_Clip.stopTime;
        this.m_TimeArea.margin = 10f;
        this.m_TimeArea.scaleWithWindow = true;
        this.m_TimeArea.SetShownHRangeInsideMargins(this.m_Clip.startTime, this.m_Clip.stopTime);
        this.m_TimeArea.hTicks.SetTickModulosForFrameRate(this.m_Clip.frameRate);
        this.m_TimeArea.ignoreScrollWheelUntilClicked = true;
      }
      if (this.m_EventTimeArea == null)
      {
        this.m_EventTimeArea = new TimeArea(true);
        this.m_EventTimeArea.hRangeLocked = true;
        this.m_EventTimeArea.vRangeLocked = true;
        this.m_EventTimeArea.hSlider = false;
        this.m_EventTimeArea.vSlider = false;
        this.m_EventTimeArea.hRangeMin = 0.0f;
        this.m_EventTimeArea.hRangeMax = AnimationClipEditor.s_EventTimelineMax;
        this.m_EventTimeArea.margin = 10f;
        this.m_EventTimeArea.scaleWithWindow = true;
        this.m_EventTimeArea.SetShownHRangeInsideMargins(0.0f, AnimationClipEditor.s_EventTimelineMax);
        this.m_EventTimeArea.hTicks.SetTickModulosForFrameRate(60f);
        this.m_EventTimeArea.ignoreScrollWheelUntilClicked = true;
      }
      if (this.m_EventManipulationHandler != null)
        return;
      this.m_EventManipulationHandler = new EventManipulationHandler(this.m_EventTimeArea);
    }

    private void OnDisable()
    {
      this.DestroyController();
      if (this.m_AvatarPreview != null)
        this.m_AvatarPreview.OnDestroy();
      AnimationEventPopup.ClosePopup();
    }

    public override bool HasPreviewGUI()
    {
      this.Init();
      return this.m_AvatarPreview != null;
    }

    public override void OnPreviewSettings()
    {
      this.m_AvatarPreview.DoPreviewSettings();
    }

    private void CalculateQualityCurves()
    {
      for (int index = 0; index < 4; ++index)
        this.m_QualityCurves[index] = new Vector2[2][];
      for (int index = 0; index < 2; ++index)
      {
        float num1 = Mathf.Clamp(this.m_ClipInfo.firstFrame / this.m_Clip.frameRate, this.m_Clip.startTime, this.m_Clip.stopTime);
        float num2 = Mathf.Clamp(this.m_ClipInfo.lastFrame / this.m_Clip.frameRate, this.m_Clip.startTime, this.m_Clip.stopTime);
        float num3 = index != 0 ? num1 : num2;
        float num4 = index != 0 ? num1 : 0.0f;
        float num5 = index != 0 ? this.m_Clip.stopTime : num2;
        int num6 = Mathf.FloorToInt(num4 * 60f);
        int num7 = Mathf.CeilToInt(num5 * 60f);
        this.m_QualityCurves[0][index] = new Vector2[num7 - num6 + 1];
        this.m_QualityCurves[1][index] = new Vector2[num7 - num6 + 1];
        this.m_QualityCurves[2][index] = new Vector2[num7 - num6 + 1];
        this.m_QualityCurves[3][index] = new Vector2[num7 - num6 + 1];
        MuscleClipEditorUtilities.CalculateQualityCurves(this.m_Clip, new QualityCurvesTime()
        {
          fixedTime = num3,
          variableEndStart = num4,
          variableEndEnd = num5,
          q = index
        }, this.m_QualityCurves[0][index], this.m_QualityCurves[1][index], this.m_QualityCurves[2][index], this.m_QualityCurves[3][index]);
      }
      this.m_DirtyQualityCurves = false;
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      bool flag = Event.current.type == EventType.Repaint;
      this.InitController();
      if (flag)
        this.m_AvatarPreview.timeControl.Update();
      AnimationClip target = this.target as AnimationClip;
      AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(target);
      this.m_AvatarPreview.timeControl.loop = true;
      if (flag && (UnityEngine.Object) this.m_AvatarPreview.PreviewObject != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.m_AvatarPreview.Animator != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) this.m_State != (UnityEngine.Object) null)
            this.m_State.iKOnFeet = this.m_AvatarPreview.IKOnFeet;
          this.m_AvatarPreview.Animator.Play(0, 0, (double) animationClipSettings.stopTime - (double) animationClipSettings.startTime == 0.0 ? 0.0f : (float) (((double) this.m_AvatarPreview.timeControl.currentTime - (double) animationClipSettings.startTime) / ((double) animationClipSettings.stopTime - (double) animationClipSettings.startTime)));
          this.m_AvatarPreview.Animator.Update(this.m_AvatarPreview.timeControl.deltaTime);
        }
        else
          target.SampleAnimation(this.m_AvatarPreview.PreviewObject, this.m_AvatarPreview.timeControl.currentTime);
      }
      this.m_AvatarPreview.DoAvatarPreview(r, background);
    }

    public void ClipRangeGUI(ref float startFrame, ref float stopFrame, out bool changedStart, out bool changedStop, bool showAdditivePoseFrame, ref float additivePoseframe, out bool changedAdditivePoseframe)
    {
      changedStart = false;
      changedStop = false;
      changedAdditivePoseframe = false;
      this.m_DraggingRangeBegin = false;
      this.m_DraggingRangeEnd = false;
      bool disabled = (double) startFrame + 0.00999999977648258 < (double) this.m_Clip.startTime * (double) this.m_Clip.frameRate || (double) startFrame - 0.00999999977648258 > (double) this.m_Clip.stopTime * (double) this.m_Clip.frameRate || (double) stopFrame + 0.00999999977648258 < (double) this.m_Clip.startTime * (double) this.m_Clip.frameRate || (double) stopFrame - 0.00999999977648258 > (double) this.m_Clip.stopTime * (double) this.m_Clip.frameRate;
      bool flag = false;
      if (disabled)
      {
        GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
        GUILayout.Label("The clip range is outside of the range of the source take.", EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.Space(5f);
        if (GUILayout.Button("Clamp Range"))
          flag = true;
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
      }
      Rect rect = GUILayoutUtility.GetRect(10f, 33f);
      GUI.Label(rect, string.Empty, (GUIStyle) "TE Toolbar");
      if (Event.current.type == EventType.Repaint)
        this.m_TimeArea.rect = rect;
      this.m_TimeArea.BeginViewGUI();
      this.m_TimeArea.EndViewGUI();
      rect.height -= 15f;
      int controlId1 = GUIUtility.GetControlID(3126789, FocusType.Passive);
      int controlId2 = GUIUtility.GetControlID(3126789, FocusType.Passive);
      int controlId3 = GUIUtility.GetControlID(3126789, FocusType.Passive);
      GUI.BeginGroup(new Rect(rect.x + 1f, rect.y + 1f, rect.width - 2f, rect.height - 2f));
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Rect& local = @rect;
      float num1 = -1f;
      rect.y = num1;
      double num2 = (double) num1;
      // ISSUE: explicit reference operation
      (^local).x = (float) num2;
      float pixel1 = this.m_TimeArea.FrameToPixel(startFrame, this.m_Clip.frameRate, rect);
      float pixel2 = this.m_TimeArea.FrameToPixel(stopFrame, this.m_Clip.frameRate, rect);
      GUI.Label(new Rect(pixel1, rect.y, pixel2 - pixel1, rect.height), string.Empty, EditorStyles.selectionRect);
      this.m_TimeArea.TimeRuler(rect, this.m_Clip.frameRate);
      float x = this.m_TimeArea.TimeToPixel(this.m_AvatarPreview.timeControl.currentTime, rect) - 0.5f;
      Handles.color = new Color(1f, 0.0f, 0.0f, 0.5f);
      Handles.DrawLine((Vector3) new Vector2(x, rect.yMin), (Vector3) new Vector2(x, rect.yMax));
      Handles.DrawLine((Vector3) new Vector2(x + 1f, rect.yMin), (Vector3) new Vector2(x + 1f, rect.yMax));
      Handles.color = Color.white;
      EditorGUI.BeginDisabledGroup(disabled);
      float time1 = startFrame / this.m_Clip.frameRate;
      switch (this.m_TimeArea.BrowseRuler(rect, controlId1, ref time1, 0.0f, false, (GUIStyle) "TL InPoint"))
      {
        case TimeArea.TimeRulerDragMode.Cancel:
          startFrame = this.m_DraggingStartFrame;
          goto case TimeArea.TimeRulerDragMode.None;
        case TimeArea.TimeRulerDragMode.None:
          float time2 = stopFrame / this.m_Clip.frameRate;
          switch (this.m_TimeArea.BrowseRuler(rect, controlId2, ref time2, 0.0f, false, (GUIStyle) "TL OutPoint"))
          {
            case TimeArea.TimeRulerDragMode.Cancel:
              stopFrame = this.m_DraggingStopFrame;
              goto case TimeArea.TimeRulerDragMode.None;
            case TimeArea.TimeRulerDragMode.None:
              if (showAdditivePoseFrame)
              {
                float time3 = additivePoseframe / this.m_Clip.frameRate;
                switch (this.m_TimeArea.BrowseRuler(rect, controlId3, ref time3, 0.0f, false, (GUIStyle) "TL playhead"))
                {
                  case TimeArea.TimeRulerDragMode.Cancel:
                    additivePoseframe = this.m_DraggingAdditivePoseFrame;
                    break;
                  case TimeArea.TimeRulerDragMode.None:
                    break;
                  default:
                    additivePoseframe = time3 * this.m_Clip.frameRate;
                    additivePoseframe = MathUtils.RoundBasedOnMinimumDifference(additivePoseframe, (float) ((double) this.m_TimeArea.PixelDeltaToTime(rect) * (double) this.m_Clip.frameRate * 10.0));
                    changedAdditivePoseframe = true;
                    break;
                }
              }
              EditorGUI.EndDisabledGroup();
              if (GUIUtility.hotControl == controlId1)
                changedStart = true;
              if (GUIUtility.hotControl == controlId2)
                changedStop = true;
              if (GUIUtility.hotControl == controlId3)
                changedAdditivePoseframe = true;
              GUI.EndGroup();
              EditorGUI.BeginDisabledGroup(disabled);
              EditorGUILayout.BeginHorizontal();
              EditorGUI.BeginChangeCheck();
              startFrame = EditorGUILayout.FloatField(AnimationClipEditor.styles.StartFrame, Mathf.Round(startFrame * 1000f) / 1000f, new GUILayoutOption[0]);
              if (EditorGUI.EndChangeCheck())
                changedStart = true;
              GUILayout.FlexibleSpace();
              EditorGUI.BeginChangeCheck();
              stopFrame = EditorGUILayout.FloatField(AnimationClipEditor.styles.EndFrame, Mathf.Round(stopFrame * 1000f) / 1000f, new GUILayoutOption[0]);
              if (EditorGUI.EndChangeCheck())
                changedStop = true;
              EditorGUILayout.EndHorizontal();
              EditorGUI.EndDisabledGroup();
              changedStart = changedStart | flag;
              changedStop = changedStop | flag;
              if (changedStart)
                startFrame = Mathf.Clamp(startFrame, this.m_Clip.startTime * this.m_Clip.frameRate, Mathf.Clamp(stopFrame, this.m_Clip.startTime * this.m_Clip.frameRate, stopFrame));
              if (changedStop)
                stopFrame = Mathf.Clamp(stopFrame, startFrame, this.m_Clip.stopTime * this.m_Clip.frameRate);
              if (changedAdditivePoseframe)
                additivePoseframe = Mathf.Clamp(additivePoseframe, this.m_Clip.startTime * this.m_Clip.frameRate, this.m_Clip.stopTime * this.m_Clip.frameRate);
              if (changedStart || changedStop || changedAdditivePoseframe)
              {
                if (!this.m_DraggingRange)
                  this.m_DraggingRangeBegin = true;
                this.m_DraggingRange = true;
              }
              else if (this.m_DraggingRange && GUIUtility.hotControl == 0 && Event.current.type == EventType.Repaint)
              {
                this.m_DraggingRangeEnd = true;
                this.m_DraggingRange = false;
                this.m_DirtyQualityCurves = true;
                this.Repaint();
              }
              GUILayout.Space(10f);
              return;
            default:
              stopFrame = time2 * this.m_Clip.frameRate;
              stopFrame = MathUtils.RoundBasedOnMinimumDifference(stopFrame, (float) ((double) this.m_TimeArea.PixelDeltaToTime(rect) * (double) this.m_Clip.frameRate * 10.0));
              changedStop = true;
              goto case TimeArea.TimeRulerDragMode.None;
          }
        default:
          startFrame = time1 * this.m_Clip.frameRate;
          startFrame = MathUtils.RoundBasedOnMinimumDifference(startFrame, (float) ((double) this.m_TimeArea.PixelDeltaToTime(rect) * (double) this.m_Clip.frameRate * 10.0));
          changedStart = true;
          goto case TimeArea.TimeRulerDragMode.None;
      }
    }

    private string GetStatsText()
    {
      string str = string.Empty;
      if (this.targets.Length == 1 && (this.target as Motion).isHumanMotion)
        str = str + "Average Velocity: " + this.m_Clip.averageSpeed.ToString("0.000") + "\nAverage Angular Y Speed: " + ((float) ((double) this.m_Clip.averageAngularSpeed * 180.0 / 3.14159274101257)).ToString("0.0") + " deg/s";
      if (this.m_ClipInfo == null)
      {
        AnimationClipStats animationClipStats1 = new AnimationClipStats();
        animationClipStats1.Reset();
        for (int index = 0; index < this.targets.Length; ++index)
        {
          AnimationClip target = this.targets[index] as AnimationClip;
          if ((UnityEngine.Object) target != (UnityEngine.Object) null)
          {
            AnimationClipStats animationClipStats2 = AnimationUtility.GetAnimationClipStats(target);
            animationClipStats1.Combine(animationClipStats2);
          }
        }
        if (str.Length != 0)
          str += (string) (object) '\n';
        float num1 = (float) ((double) animationClipStats1.constantCurves / (double) animationClipStats1.totalCurves * 100.0);
        float num2 = (float) ((double) animationClipStats1.denseCurves / (double) animationClipStats1.totalCurves * 100.0);
        float num3 = (float) ((double) animationClipStats1.streamCurves / (double) animationClipStats1.totalCurves * 100.0);
        str = str + string.Format("Curves Pos: {0} Quaternion: {1} Euler: {2} Scale: {3} Muscles: {4} Generic: {5} PPtr: {6}\n", (object) animationClipStats1.positionCurves, (object) animationClipStats1.quaternionCurves, (object) animationClipStats1.eulerCurves, (object) animationClipStats1.scaleCurves, (object) animationClipStats1.muscleCurves, (object) animationClipStats1.genericCurves, (object) animationClipStats1.pptrCurves) + string.Format("Curves Total: {0}, Constant: {1} ({2}%) Dense: {3} ({4}%) Stream: {5} ({6}%)\n", (object) animationClipStats1.totalCurves, (object) animationClipStats1.constantCurves, (object) num1.ToString("0.0"), (object) animationClipStats1.denseCurves, (object) num2.ToString("0.0"), (object) animationClipStats1.streamCurves, (object) num3.ToString("0.0")) + EditorUtility.FormatBytes(animationClipStats1.size);
      }
      return str;
    }

    private float GetClipLength()
    {
      if (this.m_ClipInfo == null)
        return this.m_Clip.length;
      return (this.m_ClipInfo.lastFrame - this.m_ClipInfo.firstFrame) / this.m_Clip.frameRate;
    }

    internal override void OnAssetStoreInspectorGUI()
    {
      this.OnInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
      this.Init();
      EditorGUIUtility.labelWidth = 50f;
      EditorGUIUtility.fieldWidth = 30f;
      EditorGUILayout.BeginHorizontal();
      EditorGUI.BeginDisabledGroup(true);
      GUILayout.Label("Length", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(46f)
      });
      GUILayout.Label(this.GetClipLength().ToString("0.000"), EditorStyles.miniLabel, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      GUILayout.Label(((double) this.m_Clip.frameRate).ToString() + " FPS", EditorStyles.miniLabel, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      EditorGUILayout.EndHorizontal();
      if (!this.m_Clip.legacy)
        this.MuscleClipGUI();
      else
        this.AnimationClipGUI();
    }

    private void AnimationClipGUI()
    {
      if (this.m_ClipInfo != null)
      {
        float firstFrame = this.m_ClipInfo.firstFrame;
        float lastFrame = this.m_ClipInfo.lastFrame;
        float additivePoseframe = 0.0f;
        bool changedStart = false;
        bool changedStop = false;
        bool changedAdditivePoseframe = false;
        this.ClipRangeGUI(ref firstFrame, ref lastFrame, out changedStart, out changedStop, false, ref additivePoseframe, out changedAdditivePoseframe);
        if (changedStart)
          this.m_ClipInfo.firstFrame = firstFrame;
        if (changedStop)
          this.m_ClipInfo.lastFrame = lastFrame;
        this.m_AvatarPreview.timeControl.startTime = firstFrame / this.m_Clip.frameRate;
        this.m_AvatarPreview.timeControl.stopTime = lastFrame / this.m_Clip.frameRate;
      }
      else
      {
        this.m_AvatarPreview.timeControl.startTime = 0.0f;
        this.m_AvatarPreview.timeControl.stopTime = this.m_Clip.length;
      }
      EditorGUIUtility.labelWidth = 0.0f;
      EditorGUIUtility.fieldWidth = 0.0f;
      if (this.m_ClipInfo != null)
        this.m_ClipInfo.loop = EditorGUILayout.Toggle("Add Loop Frame", this.m_ClipInfo.loop, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      int num = (int) EditorGUILayout.EnumPopup("Wrap Mode", (Enum) (WrapModeFixed) (this.m_ClipInfo == null ? (int) this.m_Clip.wrapMode : this.m_ClipInfo.wrapMode), new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (this.m_ClipInfo != null)
        this.m_ClipInfo.wrapMode = num;
      else
        this.m_Clip.wrapMode = (WrapMode) num;
    }

    private void CurveGUI()
    {
      if (this.m_ClipInfo == null)
        return;
      if ((double) this.m_AvatarPreview.timeControl.currentTime == double.NegativeInfinity)
        this.m_AvatarPreview.timeControl.Update();
      float normalizedTime = this.m_AvatarPreview.timeControl.normalizedTime;
      for (int index1 = 0; index1 < this.m_ClipInfo.GetCurveCount(); ++index1)
      {
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(GUIContent.none, (GUIStyle) "OL Minus", new GUILayoutOption[1]{ GUILayout.Width(17f) }))
        {
          this.m_ClipInfo.RemoveCurve(index1);
        }
        else
        {
          GUILayout.BeginVertical(GUILayout.Width(125f));
          string curveName = this.m_ClipInfo.GetCurveName(index1);
          string name = EditorGUILayout.DelayedTextField(curveName, EditorStyles.textField, new GUILayoutOption[0]);
          if (curveName != name)
            this.m_ClipInfo.SetCurveName(index1, name);
          SerializedProperty curveProperty = this.m_ClipInfo.GetCurveProperty(index1);
          AnimationCurve animationCurveValue = curveProperty.animationCurveValue;
          int length1 = animationCurveValue.length;
          bool disabled = false;
          int index2 = length1 - 1;
          for (int index3 = 0; index3 < length1; ++index3)
          {
            if ((double) Mathf.Abs(animationCurveValue.keys[index3].time - normalizedTime) < 9.99999974737875E-05)
            {
              disabled = true;
              index2 = index3;
              break;
            }
            if ((double) animationCurveValue.keys[index3].time > (double) normalizedTime)
            {
              index2 = index3;
              break;
            }
          }
          GUILayout.BeginHorizontal();
          if (GUILayout.Button(AnimationClipEditor.prevKeyContent) && index2 > 0)
          {
            --index2;
            this.m_AvatarPreview.timeControl.normalizedTime = animationCurveValue.keys[index2].time;
          }
          if (GUILayout.Button(AnimationClipEditor.nextKeyContent))
          {
            if (disabled && index2 < length1 - 1)
              ++index2;
            this.m_AvatarPreview.timeControl.normalizedTime = animationCurveValue.keys[index2].time;
          }
          EditorGUI.BeginDisabledGroup(!disabled);
          string fieldFormatString = EditorGUI.kFloatFieldFormatString;
          EditorGUI.kFloatFieldFormatString = "n3";
          float num1 = animationCurveValue.Evaluate(normalizedTime);
          float num2 = EditorGUILayout.FloatField(num1, GUILayout.Width(60f));
          EditorGUI.kFloatFieldFormatString = fieldFormatString;
          EditorGUI.EndDisabledGroup();
          bool flag = false;
          if ((double) num1 != (double) num2)
          {
            if (disabled)
              animationCurveValue.RemoveKey(index2);
            flag = true;
          }
          EditorGUI.BeginDisabledGroup(disabled);
          if (GUILayout.Button(AnimationClipEditor.addKeyframeContent))
            flag = true;
          EditorGUI.EndDisabledGroup();
          if (flag)
          {
            animationCurveValue.AddKey(new Keyframe()
            {
              time = normalizedTime,
              value = num2,
              inTangent = 0.0f,
              outTangent = 0.0f
            });
            this.m_ClipInfo.SetCurve(index1, animationCurveValue);
            AnimationCurvePreviewCache.ClearCache();
          }
          GUILayout.EndHorizontal();
          GUILayout.EndVertical();
          EditorGUILayout.CurveField(curveProperty, EditorGUI.kCurveColor, new Rect(), new GUILayoutOption[1]
          {
            GUILayout.Height(40f)
          });
          Rect lastRect = GUILayoutUtility.GetLastRect();
          int length2 = animationCurveValue.length;
          Handles.color = Color.red;
          Handles.DrawLine(new Vector3(lastRect.x + normalizedTime * lastRect.width, lastRect.y, 0.0f), new Vector3(lastRect.x + normalizedTime * lastRect.width, lastRect.y + lastRect.height, 0.0f));
          for (int index3 = 0; index3 < length2; ++index3)
          {
            float time = animationCurveValue.keys[index3].time;
            Handles.color = Color.white;
            Handles.DrawLine(new Vector3(lastRect.x + time * lastRect.width, (float) ((double) lastRect.y + (double) lastRect.height - 10.0), 0.0f), new Vector3(lastRect.x + time * lastRect.width, lastRect.y + lastRect.height, 0.0f));
          }
        }
        GUILayout.EndHorizontal();
      }
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(GUIContent.none, (GUIStyle) "OL Plus", new GUILayoutOption[1]{ GUILayout.Width(17f) }))
        this.m_ClipInfo.AddCurve();
      GUILayout.EndHorizontal();
    }

    private void EventsGUI()
    {
      if (this.m_ClipInfo == null)
        return;
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(AnimationClipEditor.styles.AddEventContent, new GUILayoutOption[1]{ GUILayout.Width(25f) }))
      {
        this.m_ClipInfo.AddEvent(Mathf.Clamp01(this.m_AvatarPreview.timeControl.normalizedTime));
        this.m_EventManipulationHandler.SelectEvent(this.m_ClipInfo.GetEvents(), this.m_ClipInfo.GetEventCount() - 1, this.m_ClipInfo);
      }
      Rect rect1 = GUILayoutUtility.GetRect(10f, 33f);
      rect1.xMin += 5f;
      rect1.xMax -= 4f;
      GUI.Label(rect1, string.Empty, (GUIStyle) "TE Toolbar");
      if (Event.current.type == EventType.Repaint)
        this.m_EventTimeArea.rect = rect1;
      rect1.height -= 15f;
      this.m_EventTimeArea.TimeRuler(rect1, 100f);
      GUI.BeginGroup(new Rect(rect1.x + 1f, rect1.y + 1f, rect1.width - 2f, rect1.height - 2f));
      Rect rect2 = new Rect(-1f, -1f, rect1.width, rect1.height);
      AnimationEvent[] events = this.m_ClipInfo.GetEvents();
      if (this.m_EventManipulationHandler.HandleEventManipulation(rect2, ref events, this.m_ClipInfo))
        this.m_ClipInfo.SetEvents(events);
      float x = this.m_EventTimeArea.TimeToPixel(this.m_AvatarPreview.timeControl.normalizedTime, rect2) - 0.5f;
      Handles.color = new Color(1f, 0.0f, 0.0f, 0.5f);
      Handles.DrawLine((Vector3) new Vector2(x, rect2.yMin), (Vector3) new Vector2(x, rect2.yMax));
      Handles.DrawLine((Vector3) new Vector2(x + 1f, rect2.yMin), (Vector3) new Vector2(x + 1f, rect2.yMax));
      Handles.color = Color.white;
      GUI.EndGroup();
      GUILayout.EndHorizontal();
      this.m_EventManipulationHandler.DrawInstantTooltip(rect1);
    }

    private void MuscleClipGUI()
    {
      EditorGUI.BeginChangeCheck();
      this.InitController();
      AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(this.m_Clip);
      bool isHumanMotion = (this.target as Motion).isHumanMotion;
      bool flag1 = AnimationUtility.HasMotionCurves(this.m_Clip);
      bool flag2 = AnimationUtility.HasRootCurves(this.m_Clip);
      bool flag3 = AnimationUtility.HasGenericRootTransform(this.m_Clip);
      bool flag4 = AnimationUtility.HasMotionFloatCurves(this.m_Clip);
      this.m_StartFrame = !this.m_DraggingRange ? animationClipSettings.startTime * this.m_Clip.frameRate : this.m_StartFrame;
      this.m_StopFrame = !this.m_DraggingRange ? animationClipSettings.stopTime * this.m_Clip.frameRate : this.m_StopFrame;
      this.m_AdditivePoseFrame = !this.m_DraggingRange ? animationClipSettings.additiveReferencePoseTime * this.m_Clip.frameRate : this.m_AdditivePoseFrame;
      bool changedStart = false;
      bool changedStop = false;
      bool changedAdditivePoseframe = false;
      if (this.m_ClipInfo != null)
      {
        if (isHumanMotion)
        {
          if (this.m_DirtyQualityCurves)
            this.CalculateQualityCurves();
          if (this.m_QualityCurves[0] == null && Event.current.type == EventType.Repaint)
          {
            this.m_DirtyQualityCurves = true;
            this.Repaint();
          }
        }
        this.ClipRangeGUI(ref this.m_StartFrame, ref this.m_StopFrame, out changedStart, out changedStop, animationClipSettings.hasAdditiveReferencePose, ref this.m_AdditivePoseFrame, out changedAdditivePoseframe);
      }
      float startTime = this.m_StartFrame / this.m_Clip.frameRate;
      float stopTime = this.m_StopFrame / this.m_Clip.frameRate;
      float num1 = this.m_AdditivePoseFrame / this.m_Clip.frameRate;
      if (!this.m_DraggingRange)
      {
        animationClipSettings.startTime = startTime;
        animationClipSettings.stopTime = stopTime;
        animationClipSettings.additiveReferencePoseTime = num1;
      }
      this.m_AvatarPreview.timeControl.startTime = startTime;
      this.m_AvatarPreview.timeControl.stopTime = stopTime;
      if (changedStart)
        this.m_AvatarPreview.timeControl.nextCurrentTime = startTime;
      if (changedStop)
        this.m_AvatarPreview.timeControl.nextCurrentTime = stopTime;
      if (changedAdditivePoseframe)
        this.m_AvatarPreview.timeControl.nextCurrentTime = num1;
      EditorGUIUtility.labelWidth = 0.0f;
      EditorGUIUtility.fieldWidth = 0.0f;
      MuscleClipQualityInfo muscleClipQualityInfo = MuscleClipEditorUtilities.GetMuscleClipQualityInfo(this.m_Clip, startTime, stopTime);
      this.LoopToggle(EditorGUILayout.GetControlRect(), AnimationClipEditor.styles.LoopTime, ref animationClipSettings.loopTime);
      EditorGUI.BeginDisabledGroup(!animationClipSettings.loopTime);
      ++EditorGUI.indentLevel;
      Rect controlRect1 = EditorGUILayout.GetControlRect();
      this.LoopToggle(controlRect1, AnimationClipEditor.styles.LoopPose, ref animationClipSettings.loopBlend);
      animationClipSettings.cycleOffset = EditorGUILayout.FloatField(AnimationClipEditor.styles.LoopCycleOffset, animationClipSettings.cycleOffset, new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
      EditorGUI.EndDisabledGroup();
      EditorGUILayout.Space();
      bool flag5 = isHumanMotion && (changedStart || changedStop);
      if (flag2 && !flag1)
      {
        GUILayout.Label("Root Transform Rotation", EditorStyles.label, new GUILayoutOption[0]);
        ++EditorGUI.indentLevel;
        Rect controlRect2 = EditorGUILayout.GetControlRect();
        this.LoopToggle(controlRect2, AnimationClipEditor.styles.BakeIntoPoseOrientation, ref animationClipSettings.loopBlendOrientation);
        int selectedIndex1 = !animationClipSettings.keepOriginalOrientation ? 1 : 0;
        int num2 = EditorGUILayout.Popup(!animationClipSettings.loopBlendOrientation ? AnimationClipEditor.styles.BasedUponStartOrientation : AnimationClipEditor.styles.BasedUponOrientation, selectedIndex1, !isHumanMotion ? AnimationClipEditor.styles.BasedUponRotationOpt : AnimationClipEditor.styles.BasedUponRotationHumanOpt, new GUILayoutOption[0]);
        animationClipSettings.keepOriginalOrientation = num2 == 0;
        if (flag5)
          EditorGUILayout.GetControlRect();
        else
          animationClipSettings.orientationOffsetY = EditorGUILayout.FloatField(AnimationClipEditor.styles.OrientationOffsetY, animationClipSettings.orientationOffsetY, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
        EditorGUILayout.Space();
        GUILayout.Label("Root Transform Position (Y)", EditorStyles.label, new GUILayoutOption[0]);
        ++EditorGUI.indentLevel;
        Rect controlRect3 = EditorGUILayout.GetControlRect();
        this.LoopToggle(controlRect3, AnimationClipEditor.styles.BakeIntoPosePositionY, ref animationClipSettings.loopBlendPositionY);
        if (isHumanMotion)
        {
          int selectedIndex2 = !animationClipSettings.keepOriginalPositionY ? (!animationClipSettings.heightFromFeet ? 1 : 2) : 0;
          switch (EditorGUILayout.Popup(!animationClipSettings.loopBlendPositionY ? AnimationClipEditor.styles.BasedUponPositionY : AnimationClipEditor.styles.BasedUponStartPositionY, selectedIndex2, AnimationClipEditor.styles.BasedUponPositionYHumanOpt, new GUILayoutOption[0]))
          {
            case 0:
              animationClipSettings.keepOriginalPositionY = true;
              animationClipSettings.heightFromFeet = false;
              break;
            case 1:
              animationClipSettings.keepOriginalPositionY = false;
              animationClipSettings.heightFromFeet = false;
              break;
            default:
              animationClipSettings.keepOriginalPositionY = false;
              animationClipSettings.heightFromFeet = true;
              break;
          }
        }
        else
        {
          int selectedIndex2 = !animationClipSettings.keepOriginalPositionY ? 1 : 0;
          int num3 = EditorGUILayout.Popup(!animationClipSettings.loopBlendPositionY ? AnimationClipEditor.styles.BasedUponPositionY : AnimationClipEditor.styles.BasedUponStartPositionY, selectedIndex2, AnimationClipEditor.styles.BasedUponPositionYOpt, new GUILayoutOption[0]);
          animationClipSettings.keepOriginalPositionY = num3 == 0;
        }
        if (flag5)
          EditorGUILayout.GetControlRect();
        else
          animationClipSettings.level = EditorGUILayout.FloatField(AnimationClipEditor.styles.PositionOffsetY, animationClipSettings.level, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
        EditorGUILayout.Space();
        GUILayout.Label("Root Transform Position (XZ)", EditorStyles.label, new GUILayoutOption[0]);
        ++EditorGUI.indentLevel;
        Rect controlRect4 = EditorGUILayout.GetControlRect();
        this.LoopToggle(controlRect4, AnimationClipEditor.styles.BakeIntoPosePositionXZ, ref animationClipSettings.loopBlendPositionXZ);
        int selectedIndex3 = !animationClipSettings.keepOriginalPositionXZ ? 1 : 0;
        int num4 = EditorGUILayout.Popup(!animationClipSettings.loopBlendPositionXZ ? AnimationClipEditor.styles.BasedUponPositionXZ : AnimationClipEditor.styles.BasedUponStartPositionXZ, selectedIndex3, !isHumanMotion ? AnimationClipEditor.styles.BasedUponPositionXZOpt : AnimationClipEditor.styles.BasedUponPositionXZHumanOpt, new GUILayoutOption[0]);
        animationClipSettings.keepOriginalPositionXZ = num4 == 0;
        --EditorGUI.indentLevel;
        EditorGUILayout.Space();
        if (isHumanMotion)
        {
          this.LoopQualityLampAndCurve(controlRect1, muscleClipQualityInfo.loop, AnimationClipEditor.s_LoopMeterHint, changedStart, changedStop, this.m_QualityCurves[0]);
          this.LoopQualityLampAndCurve(controlRect2, muscleClipQualityInfo.loopOrientation, AnimationClipEditor.s_LoopOrientationMeterHint, changedStart, changedStop, this.m_QualityCurves[1]);
          this.LoopQualityLampAndCurve(controlRect3, muscleClipQualityInfo.loopPositionY, AnimationClipEditor.s_LoopPositionYMeterHint, changedStart, changedStop, this.m_QualityCurves[2]);
          this.LoopQualityLampAndCurve(controlRect4, muscleClipQualityInfo.loopPositionXZ, AnimationClipEditor.s_LoopPositionXZMeterHint, changedStart, changedStop, this.m_QualityCurves[3]);
        }
      }
      if (isHumanMotion)
      {
        if (flag1)
          this.LoopQualityLampAndCurve(controlRect1, muscleClipQualityInfo.loop, AnimationClipEditor.s_LoopMeterHint, changedStart, changedStop, this.m_QualityCurves[0]);
        animationClipSettings.mirror = EditorGUILayout.Toggle(AnimationClipEditor.styles.Mirror, animationClipSettings.mirror, new GUILayoutOption[0]);
      }
      if (this.m_ClipInfo != null)
      {
        animationClipSettings.hasAdditiveReferencePose = EditorGUILayout.Toggle(AnimationClipEditor.styles.HasAdditiveReferencePose, animationClipSettings.hasAdditiveReferencePose, new GUILayoutOption[0]);
        EditorGUI.BeginDisabledGroup(!animationClipSettings.hasAdditiveReferencePose);
        ++EditorGUI.indentLevel;
        this.m_AdditivePoseFrame = EditorGUILayout.FloatField(AnimationClipEditor.styles.AdditiveReferencePoseFrame, this.m_AdditivePoseFrame, new GUILayoutOption[0]);
        this.m_AdditivePoseFrame = Mathf.Clamp(this.m_AdditivePoseFrame, this.m_Clip.startTime * this.m_Clip.frameRate, this.m_Clip.stopTime * this.m_Clip.frameRate);
        animationClipSettings.additiveReferencePoseTime = this.m_AdditivePoseFrame / this.m_Clip.frameRate;
        --EditorGUI.indentLevel;
        EditorGUI.EndDisabledGroup();
      }
      if (flag1)
      {
        EditorGUILayout.Space();
        GUILayout.Label(AnimationClipEditor.styles.MotionCurves, EditorStyles.label, new GUILayoutOption[0]);
      }
      if (this.m_ClipInfo == null && flag3 && !flag4)
      {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (flag1)
        {
          if (GUILayout.Button("Remove Root Motion Curves"))
            AnimationUtility.SetGenerateMotionCurves(this.m_Clip, false);
        }
        else if (GUILayout.Button("Generate Root Motion Curves"))
          AnimationUtility.SetGenerateMotionCurves(this.m_Clip, true);
        GUILayout.EndHorizontal();
      }
      string statsText = this.GetStatsText();
      if (statsText != string.Empty)
        GUILayout.Label(statsText, EditorStyles.helpBox, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      if (this.m_ClipInfo != null)
      {
        bool changed = GUI.changed;
        AnimationClipEditor.m_ShowCurves = EditorGUILayout.Foldout(AnimationClipEditor.m_ShowCurves, AnimationClipEditor.styles.Curves);
        GUI.changed = changed;
        if (AnimationClipEditor.m_ShowCurves)
          this.CurveGUI();
      }
      if (this.m_ClipInfo != null)
      {
        bool changed = GUI.changed;
        AnimationClipEditor.m_ShowEvents = EditorGUILayout.Foldout(AnimationClipEditor.m_ShowEvents, "Events");
        GUI.changed = changed;
        if (AnimationClipEditor.m_ShowEvents)
          this.EventsGUI();
      }
      if (this.m_DraggingRangeBegin)
      {
        this.m_LoopTime = animationClipSettings.loopTime;
        this.m_LoopBlend = animationClipSettings.loopBlend;
        this.m_LoopBlendOrientation = animationClipSettings.loopBlendOrientation;
        this.m_LoopBlendPositionY = animationClipSettings.loopBlendPositionY;
        this.m_LoopBlendPositionXZ = animationClipSettings.loopBlendPositionXZ;
        animationClipSettings.loopTime = false;
        animationClipSettings.loopBlend = false;
        animationClipSettings.loopBlendOrientation = false;
        animationClipSettings.loopBlendPositionY = false;
        animationClipSettings.loopBlendPositionXZ = false;
        this.m_DraggingStartFrame = animationClipSettings.startTime * this.m_Clip.frameRate;
        this.m_DraggingStopFrame = animationClipSettings.stopTime * this.m_Clip.frameRate;
        this.m_DraggingAdditivePoseFrame = animationClipSettings.additiveReferencePoseTime * this.m_Clip.frameRate;
        animationClipSettings.startTime = 0.0f;
        animationClipSettings.stopTime = this.m_Clip.length;
        AnimationUtility.SetAnimationClipSettingsNoDirty(this.m_Clip, animationClipSettings);
        this.DestroyController();
      }
      if (this.m_DraggingRangeEnd)
      {
        animationClipSettings.loopTime = this.m_LoopTime;
        animationClipSettings.loopBlend = this.m_LoopBlend;
        animationClipSettings.loopBlendOrientation = this.m_LoopBlendOrientation;
        animationClipSettings.loopBlendPositionY = this.m_LoopBlendPositionY;
        animationClipSettings.loopBlendPositionXZ = this.m_LoopBlendPositionXZ;
      }
      if (!EditorGUI.EndChangeCheck() && !this.m_DraggingRangeEnd || this.m_DraggingRange)
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Clip, "Muscle Clip Edit");
      AnimationUtility.SetAnimationClipSettingsNoDirty(this.m_Clip, animationClipSettings);
      EditorUtility.SetDirty((UnityEngine.Object) this.m_Clip);
      this.DestroyController();
    }

    private void LoopToggle(Rect r, GUIContent content, ref bool val)
    {
      if (!this.m_DraggingRange)
      {
        val = EditorGUI.Toggle(r, content, val);
      }
      else
      {
        EditorGUI.LabelField(r, content, GUIContent.none);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.Toggle(r, " ", false);
        EditorGUI.EndDisabledGroup();
      }
    }

    private void LoopQualityLampAndCurve(Rect position, float value, int lightMeterHint, bool changedStart, bool changedStop, Vector2[][] curves)
    {
      if (this.m_ClipInfo == null)
        return;
      GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
      style.alignment = TextAnchor.MiddleRight;
      Rect position1 = position;
      position1.xMax -= 20f;
      position1.xMin += EditorGUIUtility.labelWidth;
      GUI.Label(position1, "loop match", style);
      if (Event.current.GetTypeForControl(GUIUtility.GetControlID(lightMeterHint, FocusType.Native, position)) == EventType.Repaint)
      {
        Rect position2 = position;
        float num = (float) ((22.0 - (double) position2.height) / 2.0);
        position2.y -= num;
        position2.xMax += num;
        position2.height = 22f;
        position2.xMin = position2.xMax - 22f;
        if ((double) value < 0.330000013113022)
          GUI.DrawTexture(position2, AnimationClipEditor.s_RedLightIcon.image);
        else if ((double) value < 0.660000026226044)
          GUI.DrawTexture(position2, AnimationClipEditor.s_OrangeLightIcon.image);
        else
          GUI.DrawTexture(position2, AnimationClipEditor.s_GreenLightIcon.image);
        GUI.DrawTexture(position2, AnimationClipEditor.s_LightRimIcon.image);
      }
      if (!changedStart && !changedStop)
        return;
      Rect rect = position;
      rect.y += rect.height + 1f;
      rect.height = 18f;
      GUI.color = new Color(0.0f, 0.0f, 0.0f, EditorGUIUtility.isProSkin ? 0.3f : 0.8f);
      GUI.DrawTexture(rect, (Texture) EditorGUIUtility.whiteTexture);
      rect = new RectOffset(-1, -1, -1, -1).Add(rect);
      GUI.color = EditorGUIUtility.isProSkin ? new Color(0.254902f, 0.254902f, 0.254902f, 1f) : new Color(0.3529412f, 0.3529412f, 0.3529412f, 1f);
      GUI.DrawTexture(rect, (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = Color.white;
      GUI.BeginGroup(rect);
      Matrix4x4 drawingToViewMatrix = this.m_TimeArea.drawingToViewMatrix;
      drawingToViewMatrix.m00 = rect.width / this.m_TimeArea.shownArea.width;
      drawingToViewMatrix.m11 = rect.height - 1f;
      drawingToViewMatrix.m03 = -this.m_TimeArea.shownArea.x * rect.width / this.m_TimeArea.shownArea.width;
      drawingToViewMatrix.m13 = 0.0f;
      Vector2[] curve = curves[!changedStart ? 1 : 0];
      Vector3[] points = new Vector3[curve.Length];
      Color[] colors = new Color[curve.Length];
      Color color1 = new Color(1f, 0.3f, 0.3f);
      Color color2 = new Color(1f, 0.8f, 0.0f);
      Color color3 = new Color(0.0f, 1f, 0.0f);
      for (int index = 0; index < points.Length; ++index)
      {
        points[index] = (Vector3) curve[index];
        points[index] = drawingToViewMatrix.MultiplyPoint3x4(points[index]);
        colors[index] = 1.0 - (double) curve[index].y >= 0.330000013113022 ? (1.0 - (double) curve[index].y >= 0.660000026226044 ? color3 : color2) : color1;
      }
      Handles.DrawAAPolyLine(colors, points);
      GUI.color = new Color(0.3f, 0.6f, 1f);
      GUI.DrawTexture(new Rect(drawingToViewMatrix.MultiplyPoint3x4(new Vector3((!changedStart ? this.m_StopFrame : this.m_StartFrame) / this.m_Clip.frameRate, 0.0f, 0.0f)).x, 0.0f, 1f, rect.height), (Texture) EditorGUIUtility.whiteTexture);
      GUI.DrawTexture(new Rect(drawingToViewMatrix.MultiplyPoint3x4(new Vector3((!changedStart ? this.m_StartFrame : this.m_StopFrame) / this.m_Clip.frameRate, 0.0f, 0.0f)).x, 0.0f, 1f, rect.height), (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = Color.white;
      GUI.EndGroup();
    }

    private class Styles
    {
      public GUIContent StartFrame = EditorGUIUtility.TextContent("Start|Start frame of the clip.");
      public GUIContent EndFrame = EditorGUIUtility.TextContent("End|End frame of the clip.");
      public GUIContent HasAdditiveReferencePose = EditorGUIUtility.TextContent("Additive Reference Pose|Enable to define the additive reference pose frame.");
      public GUIContent AdditiveReferencePoseFrame = EditorGUIUtility.TextContent("Pose Frame|Pose Frame.");
      public GUIContent LoopTime = EditorGUIUtility.TextContent("Loop Time|Enable to make the animation plays through and then restarts when the end is reached.");
      public GUIContent LoopPose = EditorGUIUtility.TextContent("Loop Pose|Enable to make the animation loop seamlessly.");
      public GUIContent LoopCycleOffset = EditorGUIUtility.TextContent("Cycle Offset|Offset to the cycle of a looping animation, if we want to start it at a different time.");
      public GUIContent MotionCurves = EditorGUIUtility.TextContent("Root Motion is driven by curves");
      public GUIContent BakeIntoPoseOrientation = EditorGUIUtility.TextContent("Bake Into Pose|Enable to make root rotation be baked into the movement of the bones. Disable to make root rotation be stored as root motion.");
      public GUIContent OrientationOffsetY = EditorGUIUtility.TextContent("Offset|Offset to the root rotation (in degrees).");
      public GUIContent BasedUponOrientation = EditorGUIUtility.TextContent("Based Upon|What the root rotation is based upon.");
      public GUIContent BasedUponStartOrientation = EditorGUIUtility.TextContent("Based Upon (at Start)|What the root rotation is based upon.");
      public GUIContent[] BasedUponRotationHumanOpt = new GUIContent[2]{ EditorGUIUtility.TextContent("Original|Keeps the rotation as it is authored in the source file."), EditorGUIUtility.TextContent("Body Orientation|Keeps the upper body pointing forward.") };
      public GUIContent[] BasedUponRotationOpt = new GUIContent[2]{ EditorGUIUtility.TextContent("Original|Keeps the rotation as it is authored in the source file."), EditorGUIUtility.TextContent("Root Node Rotation|Keeps the upper body pointing forward.") };
      public GUIContent BakeIntoPosePositionY = EditorGUIUtility.TextContent("Bake Into Pose|Enable to make vertical root motion be baked into the movement of the bones. Disable to make vertical root motion be stored as root motion.");
      public GUIContent PositionOffsetY = EditorGUIUtility.TextContent("Offset|Offset to the vertical root position.");
      public GUIContent BasedUponPositionY = EditorGUIUtility.TextContent("Based Upon|What the vertical root position is based upon.");
      public GUIContent BasedUponStartPositionY = EditorGUIUtility.TextContent("Based Upon (at Start)|What the vertical root position is based upon.");
      public GUIContent[] BasedUponPositionYHumanOpt = new GUIContent[3]{ EditorGUIUtility.TextContent("Original|Keeps the vertical position as it is authored in the source file."), EditorGUIUtility.TextContent("Center of Mass|Keeps the center of mass aligned with root transform position."), EditorGUIUtility.TextContent("Feet|Keeps the feet aligned with the root transform position.") };
      public GUIContent[] BasedUponPositionYOpt = new GUIContent[2]{ EditorGUIUtility.TextContent("Original|Keeps the vertical position as it is authored in the source file."), EditorGUIUtility.TextContent("Root Node Position") };
      public GUIContent BakeIntoPosePositionXZ = EditorGUIUtility.TextContent("Bake Into Pose|Enable to make horizontal root motion be baked into the movement of the bones. Disable to make horizontal root motion be stored as root motion.");
      public GUIContent BasedUponPositionXZ = EditorGUIUtility.TextContent("Based Upon|What the horizontal root position is based upon.");
      public GUIContent BasedUponStartPositionXZ = EditorGUIUtility.TextContent("Based Upon (at Start)|What the horizontal root position is based upon.");
      public GUIContent[] BasedUponPositionXZHumanOpt = new GUIContent[2]{ EditorGUIUtility.TextContent("Original|Keeps the horizontal position as it is authored in the source file."), EditorGUIUtility.TextContent("Center of Mass|Keeps the center of mass aligned with root transform position.") };
      public GUIContent[] BasedUponPositionXZOpt = new GUIContent[2]{ EditorGUIUtility.TextContent("Original|Keeps the horizontal position as it is authored in the source file."), EditorGUIUtility.TextContent("Root Node Position") };
      public GUIContent Mirror = EditorGUIUtility.TextContent("Mirror|Mirror left and right in this clip.");
      public GUIContent Curves = EditorGUIUtility.TextContent("Curves|Parameter-related curves.");
      public GUIContent AddEventContent = EditorGUIUtility.IconContent("Animation.AddEvent", "|Add Event.");
    }
  }
}
