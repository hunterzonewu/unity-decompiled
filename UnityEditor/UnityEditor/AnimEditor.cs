// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AnimEditor : ScriptableObject
  {
    private static List<AnimEditor> s_AnimationWindows = new List<AnimEditor>();
    internal static PrefColor kEulerXColor = new PrefColor("Testing/EulerX", 1f, 0.0f, 1f, 1f);
    internal static PrefColor kEulerYColor = new PrefColor("Testing/EulerY", 1f, 1f, 0.0f, 1f);
    internal static PrefColor kEulerZColor = new PrefColor("Testing/EulerZ", 0.0f, 1f, 1f, 1f);
    internal static PrefKey kAnimationPrevFrame = new PrefKey("Animation/Previous Frame", ",");
    internal static PrefKey kAnimationNextFrame = new PrefKey("Animation/Next Frame", ".");
    internal static PrefKey kAnimationPrevKeyframe = new PrefKey("Animation/Previous Keyframe", "&,");
    internal static PrefKey kAnimationNextKeyframe = new PrefKey("Animation/Next Keyframe", "&.");
    internal static PrefKey kAnimationRecordKeyframe = new PrefKey("Animation/Record Keyframe", "k");
    internal static PrefKey kAnimationShowCurvesToggle = new PrefKey("Animation/Show curves", "c");
    internal const int kSliderThickness = 15;
    internal const int kLayoutRowHeight = 18;
    internal const int kIntFieldWidth = 35;
    internal const int kHierarchyMinWidth = 300;
    internal const float kDisabledRulerAlpha = 0.12f;
    [SerializeField]
    private SplitterState m_HorizontalSplitter;
    [SerializeField]
    private AnimationWindowState m_State;
    [SerializeField]
    private DopeSheetEditor m_DopeSheet;
    [SerializeField]
    private AnimationWindowHierarchy m_Hierarchy;
    [SerializeField]
    private AnimationWindowClipPopup m_ClipPopup;
    [SerializeField]
    private AnimationEventTimeLine m_Events;
    [SerializeField]
    private CurveEditor m_CurveEditor;
    [SerializeField]
    private EditorWindow m_OwnerWindow;
    [NonSerialized]
    private Rect m_Position;
    [NonSerialized]
    private bool m_TriggerFraming;
    [NonSerialized]
    private bool m_StylesInitialized;
    [NonSerialized]
    private float m_PreviousUpdateTime;
    [NonSerialized]
    private bool m_Initialized;

    public bool locked
    {
      get
      {
        return this.m_State.locked;
      }
      set
      {
        this.m_State.locked = value;
      }
    }

    public bool stateDisabled
    {
      get
      {
        return this.m_State.disabled;
      }
    }

    private float hierarchyWidth
    {
      get
      {
        return (float) this.m_HorizontalSplitter.realSizes[0];
      }
    }

    private float contentWidth
    {
      get
      {
        return (float) this.m_HorizontalSplitter.realSizes[1];
      }
    }

    public static List<AnimEditor> GetAllAnimationWindows()
    {
      return AnimEditor.s_AnimationWindows;
    }

    public void OnBreadcrumbGUI(EditorWindow parent, Rect position)
    {
      this.m_DopeSheet.m_Owner = parent;
      this.m_OwnerWindow = parent;
      this.m_Position = position;
      if (!this.m_Initialized)
        this.Initialize();
      this.m_State.OnGUI();
      this.ClampSplitterSize();
      GUILayout.BeginHorizontal();
      SplitterGUILayout.BeginHorizontalSplit(this.m_HorizontalSplitter);
      EditorGUI.BeginDisabledGroup(this.m_State.disabled);
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[0]);
      this.RecordButtonOnGUI();
      this.PlayButtonOnGUI();
      this.FrameNavigationOnGUI();
      this.AddKeyframeButtonOnGUI();
      this.AddEventButtonOnGUI();
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal(EditorStyles.toolbarButton, new GUILayoutOption[0]);
      this.ClipSelectionDropDownOnGUI();
      this.FrameRateInputFieldOnGUI();
      GUILayout.EndHorizontal();
      this.HierarchyOnGUI();
      GUILayout.BeginHorizontal(AnimationWindowStyles.miniToolbar, new GUILayoutOption[0]);
      this.TabSelectionButtonsOnGUI();
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      this.TimeRulerOnGUI();
      this.EventLineOnGUI();
      this.MainContentOnGUI();
      GUILayout.EndVertical();
      SplitterGUILayout.EndHorizontalSplit();
      GUILayout.EndHorizontal();
      this.RenderEventTooltip();
      EditorGUI.EndDisabledGroup();
      this.HandleFrameNavigationHotKeys();
    }

    private void MainContentOnGUI()
    {
      Rect contentLayoutRect = this.GetContentLayoutRect();
      if (this.m_State.animatorIsOptimized)
      {
        Vector2 vector2 = GUI.skin.label.CalcSize(AnimationWindowStyles.animatorOptimizedText);
        GUI.Label(new Rect((float) ((double) contentLayoutRect.x + (double) contentLayoutRect.width * 0.5 - (double) vector2.x * 0.5), (float) ((double) contentLayoutRect.y + (double) contentLayoutRect.height * 0.5 - (double) vector2.y * 0.5), vector2.x, vector2.y), AnimationWindowStyles.animatorOptimizedText);
      }
      else
      {
        if (this.m_State.disabled)
          this.SetupWizardOnGUI(contentLayoutRect);
        else if (this.m_State.showCurveEditor)
          this.CurveEditorOnGUI(contentLayoutRect);
        else
          this.DopeSheetOnGUI(contentLayoutRect);
        this.HandleCopyPaste();
        AnimationWindowUtility.DrawVerticalSplitLine(new Vector2(contentLayoutRect.xMin + 1f, contentLayoutRect.yMin), new Vector2(contentLayoutRect.xMin + 1f, contentLayoutRect.yMax));
      }
    }

    private Rect GetContentLayoutRect()
    {
      return GUILayoutUtility.GetRect(this.contentWidth, this.contentWidth, 0.0f, float.MaxValue, new GUILayoutOption[1]
      {
        GUILayout.ExpandHeight(true)
      });
    }

    public void Update()
    {
      if ((UnityEngine.Object) this.m_State == (UnityEngine.Object) null)
        return;
      this.PlaybackUpdate();
    }

    public void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
      AnimEditor.s_AnimationWindows.Add(this);
      if ((UnityEngine.Object) this.m_State == (UnityEngine.Object) null)
      {
        this.m_State = ScriptableObject.CreateInstance(typeof (AnimationWindowState)) as AnimationWindowState;
        this.m_State.hideFlags = HideFlags.HideAndDontSave;
        this.m_State.animEditor = this;
        this.InitializeHorizontalSplitter();
        this.InitializeClipSelection();
        this.InitializeDopeSheet();
        this.InitializeEvents();
        this.InitializeCurveEditor();
      }
      this.InitializeNonserializedValues();
      this.m_State.timeArea = !this.m_State.showCurveEditor ? (TimeArea) this.m_DopeSheet : (TimeArea) this.m_CurveEditor;
      this.m_DopeSheet.state = this.m_State;
      this.m_ClipPopup.state = this.m_State;
      this.m_State.onClipSelectionChanged += new System.Action(this.OnClipSelectionChange);
      this.m_State.OnSelectionChange();
      this.m_CurveEditor.curvesUpdated += new CurveEditor.CallbackFunction(this.SaveChangedCurvesFromCurveEditor);
    }

    public void OnDisable()
    {
      AnimEditor.s_AnimationWindows.Remove(this);
      if (this.m_CurveEditor != null)
        this.m_CurveEditor.curvesUpdated -= new CurveEditor.CallbackFunction(this.SaveChangedCurvesFromCurveEditor);
      this.m_State.onClipSelectionChanged -= new System.Action(this.OnClipSelectionChange);
      this.m_State.OnDisable();
    }

    public void OnSelectionChange()
    {
      this.m_State.OnSelectionChange();
      this.Repaint();
    }

    public void OnControllerChange()
    {
      this.m_State.OnControllerChange();
      this.Repaint();
    }

    public void OnClipSelectionChange()
    {
      this.m_TriggerFraming = true;
    }

    private void PlaybackUpdate()
    {
      if (!this.m_State.playing)
        return;
      float num = Time.realtimeSinceStartup - this.m_PreviousUpdateTime;
      this.m_PreviousUpdateTime = Time.realtimeSinceStartup;
      this.m_State.currentTime += num;
      if ((double) this.m_State.currentTime > (double) this.m_State.maxTime)
        this.m_State.currentTime = this.m_State.minTime;
      this.m_State.currentTime = Mathf.Clamp(this.m_State.currentTime, this.m_State.minTime, this.m_State.maxTime);
      this.m_State.ResampleAnimation();
      this.Repaint();
    }

    private void SetupWizardOnGUI(Rect position)
    {
      Rect position1 = new Rect(position.x, position.y, position.width - 15f, position.height - 15f);
      GUI.BeginClip(position1);
      GUI.enabled = true;
      this.m_State.showCurveEditor = false;
      this.m_State.timeArea = (TimeArea) this.m_DopeSheet;
      this.m_State.timeArea.SetShownHRangeInsideMargins(0.0f, 1f);
      if ((bool) ((UnityEngine.Object) Selection.activeGameObject) && !EditorUtility.IsPersistent((UnityEngine.Object) Selection.activeGameObject))
      {
        string str = (bool) ((UnityEngine.Object) this.m_State.activeRootGameObject) || (bool) ((UnityEngine.Object) this.m_State.activeAnimationClip) ? AnimationWindowStyles.animationClip.text : AnimationWindowStyles.animatorAndAnimationClip.text;
        GUIContent content = GUIContent.Temp(string.Format(AnimationWindowStyles.formatIsMissing.text, (object) Selection.activeGameObject.name, (object) str));
        Vector2 vector2 = GUI.skin.label.CalcSize(content);
        Rect position2 = new Rect((float) ((double) position1.width * 0.5 - (double) vector2.x * 0.5), (float) ((double) position1.height * 0.5 - (double) vector2.y * 0.5), vector2.x, vector2.y);
        GUI.Label(position2, content);
        if (GUI.Button(new Rect((float) ((double) position1.width * 0.5 - 35.0), position2.yMax + 3f, 70f, 20f), AnimationWindowStyles.create) && AnimationWindowUtility.InitializeGameobjectForAnimation(Selection.activeGameObject))
        {
          this.m_State.activeAnimationClip = AnimationUtility.GetAnimationClips(AnimationWindowUtility.GetClosestAnimationPlayerComponentInParents(Selection.activeGameObject.transform).gameObject)[0];
          this.m_State.recording = true;
          this.m_State.currentTime = 0.0f;
          this.m_State.ResampleAnimation();
        }
      }
      else
      {
        Color color = GUI.color;
        GUI.color = Color.gray;
        Vector2 vector2 = GUI.skin.label.CalcSize(AnimationWindowStyles.noAnimatableObjectSelectedText);
        GUI.Label(new Rect((float) ((double) position1.width * 0.5 - (double) vector2.x * 0.5), (float) ((double) position1.height * 0.5 - (double) vector2.y * 0.5), vector2.x, vector2.y), AnimationWindowStyles.noAnimatableObjectSelectedText);
        GUI.color = color;
      }
      GUI.EndClip();
      GUI.enabled = false;
    }

    private void EventLineOnGUI()
    {
      Rect rect = GUILayoutUtility.GetRect(this.m_Position.width - this.hierarchyWidth, 18f);
      rect.width -= 15f;
      GUI.Label(rect, GUIContent.none, AnimationWindowStyles.eventBackground);
      if (!this.m_State.disabled)
        this.DrawPlayHead(rect.yMin - 1f, rect.yMax);
      this.m_Events.EventLineGUI(rect, this.m_State);
    }

    private void RenderEventTooltip()
    {
      this.m_Events.DrawInstantTooltip(this.m_Position);
    }

    private void TabSelectionButtonsOnGUI()
    {
      GUILayout.FlexibleSpace();
      EditorGUI.BeginChangeCheck();
      GUILayout.Toggle((!this.m_State.showCurveEditor ? 1 : 0) != 0, AnimationWindowStyles.dopesheet, AnimationWindowStyles.miniToolbarButton, new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      GUILayout.Toggle((this.m_State.showCurveEditor ? 1 : 0) != 0, AnimationWindowStyles.curves, AnimationWindowStyles.miniToolbarButton, new GUILayoutOption[1]
      {
        GUILayout.Width(80f)
      });
      if (!EditorGUI.EndChangeCheck() && !AnimEditor.kAnimationShowCurvesToggle.activated)
        return;
      this.SwitchBetweenCurvesAndDopesheet();
    }

    private void HierarchyOnGUI()
    {
      Rect rect = GUILayoutUtility.GetRect(this.hierarchyWidth, this.hierarchyWidth, 0.0f, float.MaxValue, new GUILayoutOption[1]
      {
        GUILayout.ExpandHeight(true)
      });
      if (this.m_State.disabled)
        return;
      this.m_Hierarchy.OnGUI(rect);
    }

    private void FrameRateInputFieldOnGUI()
    {
      GUILayout.Label(AnimationWindowStyles.samples, AnimationWindowStyles.toolbarLabel, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      int num = EditorGUILayout.IntField((int) this.m_State.frameRate, EditorStyles.toolbarTextField, new GUILayoutOption[1]
      {
        GUILayout.Width(35f)
      });
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_State.frameRate = (float) num;
    }

    private void ClipSelectionDropDownOnGUI()
    {
      this.m_ClipPopup.OnGUI();
    }

    private void DopeSheetOnGUI(Rect position)
    {
      Rect rect = new Rect(position.xMin, position.yMin, position.width - 15f, position.height);
      if (Event.current.type == EventType.Repaint)
      {
        this.m_DopeSheet.rect = rect;
        this.m_DopeSheet.SetTickMarkerRanges();
        this.m_DopeSheet.RecalculateBounds();
      }
      if (this.m_State.showCurveEditor)
        return;
      if (this.m_TriggerFraming && Event.current.type == EventType.Repaint)
      {
        this.m_DopeSheet.FrameClip();
        this.m_TriggerFraming = false;
      }
      Rect position1 = new Rect(position.xMin, position.yMin, position.width - 15f, position.height - 15f);
      Rect position2 = new Rect(position1.xMin, position1.yMin, position1.width, 16f);
      this.m_DopeSheet.BeginViewGUI();
      if (!this.m_State.disabled)
      {
        this.m_DopeSheet.TimeRuler(position1, this.m_State.frameRate, false, true, 0.12f);
        this.m_DopeSheet.DrawMasterDopelineBackground(position2);
        this.DrawPlayHead(position1.yMin - 1f, position1.yMax);
      }
      this.m_DopeSheet.OnGUI(position1, this.m_State.hierarchyState.scrollPos * -1f);
      this.m_DopeSheet.EndViewGUI();
      Rect position3 = new Rect(rect.xMax, rect.yMin, 15f, position1.height);
      float bottomValue = Mathf.Max(this.m_DopeSheet.contentHeight, position.height);
      this.m_State.hierarchyState.scrollPos.y = GUI.VerticalScrollbar(position3, this.m_State.hierarchyState.scrollPos.y, position.height, 0.0f, bottomValue);
    }

    private void CurveEditorOnGUI(Rect position)
    {
      if (Event.current.type == EventType.Repaint)
      {
        this.m_CurveEditor.rect = position;
        this.m_CurveEditor.SetTickMarkerRanges();
        this.m_CurveEditor.RecalculateBounds();
      }
      if (!this.m_State.showCurveEditor)
        return;
      Rect position1 = new Rect(position.xMin, position.yMin, position.width - 15f, position.height - 15f);
      this.m_CurveEditor.vSlider = this.m_State.showCurveEditor;
      this.m_CurveEditor.hSlider = this.m_State.showCurveEditor;
      this.UpdateCurveEditorData();
      if (this.m_State.m_FrameCurveEditor)
      {
        this.m_CurveEditor.FrameSelected(false, true);
        this.m_State.m_FrameCurveEditor = false;
      }
      this.m_CurveEditor.BeginViewGUI();
      if (!this.m_State.disabled)
      {
        GUI.Box(position1, GUIContent.none, AnimationWindowStyles.curveEditorBackground);
        this.m_CurveEditor.GridGUI();
        this.DrawPlayHead(position1.yMin, position1.yMax);
      }
      EditorGUI.BeginDisabledGroup(this.m_State.animationIsReadOnly);
      EditorGUI.BeginChangeCheck();
      this.m_CurveEditor.CurveGUI();
      if (EditorGUI.EndChangeCheck())
      {
        this.SaveChangedCurvesFromCurveEditor();
        this.UpdateSelectedKeysFromCurveEditor();
      }
      EditorGUI.EndDisabledGroup();
      this.m_CurveEditor.EndViewGUI();
    }

    private void TimeRulerOnGUI()
    {
      Rect rect = GUILayoutUtility.GetRect(this.m_Position.width - this.hierarchyWidth, 18f);
      Rect position = new Rect(rect.xMin, rect.yMin, rect.width - 15f, rect.height);
      GUI.Box(rect, GUIContent.none, EditorStyles.toolbarButton);
      this.m_State.timeArea.TimeRuler(position, this.m_State.frameRate, true, false, 1f);
      if (!this.m_State.disabled)
      {
        this.RenderEndOfClipOverlay(position.yMin, position.yMax);
        this.DrawPlayHead(position.yMin, position.yMax);
      }
      EditorGUI.BeginChangeCheck();
      int num = Mathf.Max(Mathf.RoundToInt(GUI.HorizontalSlider(position, (float) this.m_State.frame, this.m_State.minVisibleFrame, this.m_State.maxVisibleFrame, GUIStyle.none, GUIStyle.none)), 0);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_State.frame = num;
      this.m_State.recording = true;
      this.m_State.ResampleAnimation();
    }

    private void AddEventButtonOnGUI()
    {
      if (!GUILayout.Button(AnimationWindowStyles.addEventContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        return;
      AnimationEventPopup.Create(this.m_State.activeRootGameObject, this.m_State.activeAnimationClip, this.m_State.currentTime, this.m_OwnerWindow);
    }

    private void AddKeyframeButtonOnGUI()
    {
      if (!GUILayout.Button(AnimationWindowStyles.addKeyframeContent, EditorStyles.toolbarButton, new GUILayoutOption[0]) && !AnimEditor.kAnimationRecordKeyframe.activated)
        return;
      AnimationWindowUtility.AddSelectedKeyframes(this.m_State, this.m_State.time);
    }

    private void FrameNavigationOnGUI()
    {
      if (GUILayout.Button(AnimationWindowStyles.prevKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.MoveToPreviousKeyframe();
      if (GUILayout.Button(AnimationWindowStyles.nextKeyContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.MoveToNextKeyframe();
      EditorGUI.BeginChangeCheck();
      int num = EditorGUILayout.IntField(this.m_State.frame, EditorStyles.toolbarTextField, new GUILayoutOption[1]
      {
        GUILayout.Width(35f)
      });
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_State.frame = num;
    }

    private void HandleFrameNavigationHotKeys()
    {
      if (AnimEditor.kAnimationPrevKeyframe.activated)
        this.MoveToPreviousKeyframe();
      if (AnimEditor.kAnimationNextKeyframe.activated)
        this.MoveToNextKeyframe();
      if (AnimEditor.kAnimationNextFrame.activated)
        ++this.m_State.frame;
      if (AnimEditor.kAnimationPrevFrame.activated)
        --this.m_State.frame;
      if (!AnimEditor.kAnimationPrevKeyframe.activated && !AnimEditor.kAnimationNextKeyframe.activated && (!AnimEditor.kAnimationNextFrame.activated && !AnimEditor.kAnimationPrevFrame.activated))
        return;
      this.Repaint();
    }

    private void MoveToPreviousKeyframe()
    {
      this.m_State.currentTime = AnimationWindowUtility.GetPreviousKeyframeTime((!this.m_State.showCurveEditor ? this.m_State.allCurves : this.m_State.activeCurves).ToArray(), this.m_State.FrameToTime((float) this.m_State.frame), this.m_State.frameRate);
      this.m_State.frame = this.m_State.TimeToFrameFloor(this.m_State.currentTime);
    }

    private void MoveToNextKeyframe()
    {
      this.m_State.currentTime = AnimationWindowUtility.GetNextKeyframeTime((!this.m_State.showCurveEditor ? this.m_State.allCurves : this.m_State.activeCurves).ToArray(), this.m_State.FrameToTime((float) this.m_State.frame), this.m_State.frameRate);
      this.m_State.frame = this.m_State.TimeToFrameFloor(this.m_State.currentTime);
    }

    private void PlayButtonOnGUI()
    {
      EditorGUI.BeginChangeCheck();
      this.m_State.playing = GUILayout.Toggle(this.m_State.playing, AnimationWindowStyles.playContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_PreviousUpdateTime = Time.realtimeSinceStartup;
    }

    private void RecordButtonOnGUI()
    {
      EditorGUI.BeginChangeCheck();
      this.m_State.recording = GUILayout.Toggle(this.m_State.recording, AnimationWindowStyles.recordContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck() || !this.m_State.recording)
        return;
      this.m_State.ResampleAnimation();
    }

    private void SwitchBetweenCurvesAndDopesheet()
    {
      this.m_State.showCurveEditor = !this.m_State.showCurveEditor;
      if (this.m_State.showCurveEditor)
      {
        this.UpdateCurveEditorData();
        this.UpdateSelectedKeysToCurveEditor();
        AnimationWindowUtility.SyncTimeArea((TimeArea) this.m_DopeSheet, (TimeArea) this.m_CurveEditor);
        this.m_State.timeArea = (TimeArea) this.m_CurveEditor;
        this.m_CurveEditor.RecalculateBounds();
        this.m_CurveEditor.FrameSelected(false, true);
      }
      else
      {
        this.UpdateSelectedKeysFromCurveEditor();
        AnimationWindowUtility.SyncTimeArea((TimeArea) this.m_CurveEditor, (TimeArea) this.m_DopeSheet);
        this.m_State.timeArea = (TimeArea) this.m_DopeSheet;
      }
    }

    private void DrawPlayHead(float minY, float maxY)
    {
      GUIClip.Push(new Rect(this.hierarchyWidth - 1f, 0.0f, (float) ((double) this.m_Position.width - (double) this.hierarchyWidth - 15.0), this.m_Position.height), Vector2.zero, Vector2.zero, false);
      AnimationWindowUtility.DrawPlayHead(this.m_State.TimeToPixel(this.m_State.currentTime), minY, maxY, 1f);
      GUIClip.Pop();
    }

    private void RenderEndOfClipOverlay(float minY, float maxY)
    {
      Rect rect1 = new Rect(this.hierarchyWidth - 1f, 0.0f, (float) ((double) this.m_Position.width - (double) this.hierarchyWidth - 15.0), this.m_Position.height);
      Rect rect2 = new Rect(rect1.xMin, minY, rect1.width, maxY - minY);
      AnimationWindowUtility.DrawEndOfClip(rect2, this.m_State.TimeToPixel(this.m_State.maxTime) + rect2.xMin);
    }

    private void ClampSplitterSize()
    {
      this.m_HorizontalSplitter.realSizes[1] = (int) Mathf.Min(this.m_Position.width - this.hierarchyWidth, (float) this.m_HorizontalSplitter.realSizes[1]);
    }

    private void SaveChangedCurvesFromCurveEditor()
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_State.activeAnimationClip, "Edit Curve");
      foreach (CurveWrapper animationCurve in this.m_CurveEditor.animationCurves)
      {
        if (animationCurve.changed)
        {
          AnimationUtility.SetEditorCurve(this.m_State.activeAnimationClip, animationCurve.binding, animationCurve.curve);
          animationCurve.changed = false;
        }
      }
      this.m_State.ResampleAnimation();
    }

    private void UpdateSelectedKeysFromCurveEditor()
    {
      this.m_State.ClearKeySelections();
      using (List<CurveSelection>.Enumerator enumerator = this.m_CurveEditor.selectedCurves.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationWindowKeyframe animationWindowKeyframe = AnimationWindowUtility.CurveSelectionToAnimationWindowKeyframe(enumerator.Current, this.m_State.allCurves);
          if (animationWindowKeyframe != null)
            this.m_State.SelectKey(animationWindowKeyframe);
        }
      }
    }

    private void UpdateSelectedKeysToCurveEditor()
    {
      this.m_CurveEditor.ClearSelection();
      using (List<AnimationWindowKeyframe>.Enumerator enumerator = this.m_State.selectedKeys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          CurveSelection curveSelection = AnimationWindowUtility.AnimationWindowKeyframeToCurveSelection(enumerator.Current, this.m_CurveEditor);
          if (curveSelection != null)
            this.m_CurveEditor.AddSelection(curveSelection);
        }
      }
    }

    private void HandleCopyPaste()
    {
      if (Event.current.type != EventType.ValidateCommand && Event.current.type != EventType.ExecuteCommand)
        return;
      string commandName = Event.current.commandName;
      if (commandName == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (AnimEditor.\u003C\u003Ef__switch\u0024mapB == null)
      {
        // ISSUE: reference to a compiler-generated field
        AnimEditor.\u003C\u003Ef__switch\u0024mapB = new Dictionary<string, int>(2)
        {
          {
            "Copy",
            0
          },
          {
            "Paste",
            1
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!AnimEditor.\u003C\u003Ef__switch\u0024mapB.TryGetValue(commandName, out num))
        return;
      if (num != 0)
      {
        if (num != 1)
          return;
        if (Event.current.type == EventType.ExecuteCommand)
        {
          this.m_State.PasteKeys();
          this.UpdateCurveEditorData();
          this.UpdateSelectedKeysToCurveEditor();
        }
        Event.current.Use();
      }
      else
      {
        if (Event.current.type == EventType.ExecuteCommand)
          this.m_State.CopyKeys();
        Event.current.Use();
      }
    }

    private void UpdateCurveEditorData()
    {
      this.m_CurveEditor.animationCurves = this.m_State.activeCurveWrappers.ToArray();
    }

    public void Repaint()
    {
      if (!((UnityEngine.Object) this.m_OwnerWindow != (UnityEngine.Object) null))
        return;
      this.m_OwnerWindow.Repaint();
    }

    private void Initialize()
    {
      AnimationWindowStyles.Initialize();
      this.InitializeHierarchy();
      this.m_CurveEditor.m_PlayHead = (IPlayHead) this.m_State;
      this.m_Initialized = true;
    }

    private void InitializeClipSelection()
    {
      this.m_ClipPopup = new AnimationWindowClipPopup();
    }

    private void InitializeHierarchy()
    {
      this.m_Hierarchy = new AnimationWindowHierarchy(this.m_State, this.m_OwnerWindow, new Rect(0.0f, 0.0f, this.hierarchyWidth, 100f));
    }

    private void InitializeDopeSheet()
    {
      this.m_DopeSheet = new DopeSheetEditor(this.m_OwnerWindow);
      this.m_DopeSheet.SetTickMarkerRanges();
      this.m_DopeSheet.hSlider = true;
      this.m_DopeSheet.shownArea = new Rect(1f, 1f, 1f, 1f);
      this.m_DopeSheet.rect = new Rect(0.0f, 0.0f, this.contentWidth, 100f);
      this.m_DopeSheet.hTicks.SetTickModulosForFrameRate(this.m_State.frameRate);
    }

    private void InitializeEvents()
    {
      this.m_Events = new AnimationEventTimeLine(this.m_OwnerWindow);
    }

    private void InitializeCurveEditor()
    {
      this.m_CurveEditor = new CurveEditor(new Rect(0.0f, 0.0f, this.contentWidth, 100f), new CurveWrapper[0], false);
      CurveEditorSettings curveEditorSettings = new CurveEditorSettings();
      curveEditorSettings.hTickStyle.distMin = 30;
      curveEditorSettings.hTickStyle.distFull = 80;
      curveEditorSettings.hTickStyle.distLabel = 0;
      if (EditorGUIUtility.isProSkin)
      {
        curveEditorSettings.vTickStyle.color = new Color(1f, 1f, 1f, curveEditorSettings.vTickStyle.color.a);
        curveEditorSettings.vTickStyle.labelColor = new Color(1f, 1f, 1f, curveEditorSettings.vTickStyle.labelColor.a);
      }
      curveEditorSettings.vTickStyle.distMin = 15;
      curveEditorSettings.vTickStyle.distFull = 40;
      curveEditorSettings.vTickStyle.distLabel = 30;
      curveEditorSettings.vTickStyle.stubs = true;
      curveEditorSettings.hRangeMin = 0.0f;
      curveEditorSettings.hRangeLocked = false;
      curveEditorSettings.vRangeLocked = false;
      curveEditorSettings.hSlider = true;
      curveEditorSettings.vSlider = true;
      this.m_CurveEditor.shownArea = new Rect(1f, 1f, 1f, 1f);
      this.m_CurveEditor.settings = curveEditorSettings;
      this.m_CurveEditor.m_PlayHead = (IPlayHead) this.m_State;
    }

    private void InitializeHorizontalSplitter()
    {
      this.m_HorizontalSplitter = new SplitterState(new float[2]{ 300f, 900f }, new int[2]{ 300, 300 }, (int[]) null);
      this.m_HorizontalSplitter.realSizes[0] = 300;
      this.m_HorizontalSplitter.realSizes[1] = (int) this.m_Position.width - 300;
      this.ClampSplitterSize();
    }

    private void InitializeNonserializedValues()
    {
      this.m_State.onFrameRateChange += (System.Action<float>) (newFrameRate =>
      {
        this.m_CurveEditor.invSnap = newFrameRate;
        this.m_CurveEditor.hTicks.SetTickModulosForFrameRate(newFrameRate);
      });
    }
  }
}
