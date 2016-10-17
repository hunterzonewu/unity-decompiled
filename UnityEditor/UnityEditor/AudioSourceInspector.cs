// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioSourceInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (AudioSource))]
  internal class AudioSourceInspector : Editor
  {
    private static CurveEditorSettings m_CurveEditorSettings = new CurveEditorSettings();
    internal static Color kRolloffCurveColor = new Color(0.9f, 0.3f, 0.2f, 1f);
    internal static Color kSpatialCurveColor = new Color(0.25f, 0.7f, 0.2f, 1f);
    internal static Color kSpreadCurveColor = new Color(0.25f, 0.55f, 0.95f, 1f);
    internal static Color kLowPassCurveColor = new Color(0.8f, 0.25f, 0.9f, 1f);
    internal static Color kReverbZoneMixCurveColor = new Color(0.7f, 0.7f, 0.2f, 1f);
    internal bool[] m_SelectedCurves = new bool[0];
    private const int kRolloffCurveID = 0;
    private const int kSpatialBlendCurveID = 1;
    private const int kSpreadCurveID = 2;
    private const int kLowPassCurveID = 3;
    private const int kReverbZoneMixCurveID = 4;
    internal const float kMaxCutoffFrequency = 22000f;
    private const float EPSILON = 0.0001f;
    private SerializedProperty m_AudioClip;
    private SerializedProperty m_PlayOnAwake;
    private SerializedProperty m_Volume;
    private SerializedProperty m_Pitch;
    private SerializedProperty m_Loop;
    private SerializedProperty m_Mute;
    private SerializedProperty m_Spatialize;
    private SerializedProperty m_Priority;
    private SerializedProperty m_PanLevel;
    private SerializedProperty m_DopplerLevel;
    private SerializedProperty m_MinDistance;
    private SerializedProperty m_MaxDistance;
    private SerializedProperty m_Pan2D;
    private SerializedProperty m_RolloffMode;
    private SerializedProperty m_BypassEffects;
    private SerializedProperty m_BypassListenerEffects;
    private SerializedProperty m_BypassReverbZones;
    private SerializedProperty m_OutputAudioMixerGroup;
    private SerializedObject m_LowpassObject;
    private AudioSourceInspector.AudioCurveWrapper[] m_AudioCurves;
    private bool m_RefreshCurveEditor;
    private CurveEditor m_CurveEditor;
    private Vector3 m_LastListenerPosition;
    private bool m_Expanded3D;
    private static AudioSourceInspector.Styles ms_Styles;

    private void OnEnable()
    {
      this.m_AudioClip = this.serializedObject.FindProperty("m_audioClip");
      this.m_PlayOnAwake = this.serializedObject.FindProperty("m_PlayOnAwake");
      this.m_Volume = this.serializedObject.FindProperty("m_Volume");
      this.m_Pitch = this.serializedObject.FindProperty("m_Pitch");
      this.m_Loop = this.serializedObject.FindProperty("Loop");
      this.m_Mute = this.serializedObject.FindProperty("Mute");
      this.m_Spatialize = this.serializedObject.FindProperty("Spatialize");
      this.m_Priority = this.serializedObject.FindProperty("Priority");
      this.m_DopplerLevel = this.serializedObject.FindProperty("DopplerLevel");
      this.m_MinDistance = this.serializedObject.FindProperty("MinDistance");
      this.m_MaxDistance = this.serializedObject.FindProperty("MaxDistance");
      this.m_Pan2D = this.serializedObject.FindProperty("Pan2D");
      this.m_RolloffMode = this.serializedObject.FindProperty("rolloffMode");
      this.m_BypassEffects = this.serializedObject.FindProperty("BypassEffects");
      this.m_BypassListenerEffects = this.serializedObject.FindProperty("BypassListenerEffects");
      this.m_BypassReverbZones = this.serializedObject.FindProperty("BypassReverbZones");
      this.m_OutputAudioMixerGroup = this.serializedObject.FindProperty("OutputAudioMixerGroup");
      this.m_AudioCurves = new AudioSourceInspector.AudioCurveWrapper[5]
      {
        new AudioSourceInspector.AudioCurveWrapper(AudioSourceInspector.AudioCurveType.Volume, "Volume", 0, AudioSourceInspector.kRolloffCurveColor, this.serializedObject.FindProperty("rolloffCustomCurve"), 0.0f, 1f),
        new AudioSourceInspector.AudioCurveWrapper(AudioSourceInspector.AudioCurveType.SpatialBlend, "Spatial Blend", 1, AudioSourceInspector.kSpatialCurveColor, this.serializedObject.FindProperty("panLevelCustomCurve"), 0.0f, 1f),
        new AudioSourceInspector.AudioCurveWrapper(AudioSourceInspector.AudioCurveType.Spread, "Spread", 2, AudioSourceInspector.kSpreadCurveColor, this.serializedObject.FindProperty("spreadCustomCurve"), 0.0f, 1f),
        new AudioSourceInspector.AudioCurveWrapper(AudioSourceInspector.AudioCurveType.Lowpass, "Low-Pass", 3, AudioSourceInspector.kLowPassCurveColor, (SerializedProperty) null, 0.0f, 1f),
        new AudioSourceInspector.AudioCurveWrapper(AudioSourceInspector.AudioCurveType.ReverbZoneMix, "Reverb Zone Mix", 4, AudioSourceInspector.kReverbZoneMixCurveColor, this.serializedObject.FindProperty("reverbZoneMixCustomCurve"), 0.0f, 1.1f)
      };
      AudioSourceInspector.m_CurveEditorSettings.hRangeMin = 0.0f;
      AudioSourceInspector.m_CurveEditorSettings.vRangeMin = 0.0f;
      AudioSourceInspector.m_CurveEditorSettings.vRangeMax = 1.1f;
      AudioSourceInspector.m_CurveEditorSettings.hRangeMax = 1f;
      AudioSourceInspector.m_CurveEditorSettings.vSlider = false;
      AudioSourceInspector.m_CurveEditorSettings.hSlider = false;
      AudioSourceInspector.m_CurveEditorSettings.hTickStyle = new TickStyle()
      {
        color = new Color(0.0f, 0.0f, 0.0f, 0.15f),
        distLabel = 30
      };
      AudioSourceInspector.m_CurveEditorSettings.vTickStyle = new TickStyle()
      {
        color = new Color(0.0f, 0.0f, 0.0f, 0.15f),
        distLabel = 20
      };
      this.m_CurveEditor = new CurveEditor(new Rect(0.0f, 0.0f, 1000f, 100f), new CurveWrapper[0], false);
      this.m_CurveEditor.settings = AudioSourceInspector.m_CurveEditorSettings;
      this.m_CurveEditor.margin = 25f;
      this.m_CurveEditor.SetShownHRangeInsideMargins(0.0f, 1f);
      this.m_CurveEditor.SetShownVRangeInsideMargins(0.0f, 1.1f);
      this.m_CurveEditor.ignoreScrollWheelUntilClicked = true;
      this.m_LastListenerPosition = AudioUtil.GetListenerPos();
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.m_Expanded3D = EditorPrefs.GetBool("AudioSourceExpanded3D", this.m_Expanded3D);
    }

    private void OnDisable()
    {
      this.m_CurveEditor.OnDisable();
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      EditorPrefs.SetBool("AudioSourceExpanded3D", this.m_Expanded3D);
    }

    private CurveWrapper[] GetCurveWrapperArray()
    {
      List<CurveWrapper> curveWrapperList = new List<CurveWrapper>();
      foreach (AudioSourceInspector.AudioCurveWrapper audioCurve in this.m_AudioCurves)
      {
        if (audioCurve.curveProp != null)
        {
          AnimationCurve curve = audioCurve.curveProp.animationCurveValue;
          bool flag;
          if (audioCurve.type == AudioSourceInspector.AudioCurveType.Volume)
          {
            AudioRolloffMode enumValueIndex = (AudioRolloffMode) this.m_RolloffMode.enumValueIndex;
            if (this.m_RolloffMode.hasMultipleDifferentValues)
              flag = false;
            else if (enumValueIndex == AudioRolloffMode.Custom)
            {
              flag = !audioCurve.curveProp.hasMultipleDifferentValues;
            }
            else
            {
              flag = !this.m_MinDistance.hasMultipleDifferentValues && !this.m_MaxDistance.hasMultipleDifferentValues;
              if (enumValueIndex == AudioRolloffMode.Linear)
                curve = AnimationCurve.Linear(this.m_MinDistance.floatValue / this.m_MaxDistance.floatValue, 1f, 1f, 0.0f);
              else if (enumValueIndex == AudioRolloffMode.Logarithmic)
                curve = AudioSourceInspector.Logarithmic(this.m_MinDistance.floatValue / this.m_MaxDistance.floatValue, 1f, 1f);
            }
          }
          else
            flag = !audioCurve.curveProp.hasMultipleDifferentValues;
          if (flag)
          {
            if (curve.length == 0)
              Debug.LogError((object) (audioCurve.legend.text + " curve has no keys!"));
            else
              curveWrapperList.Add(this.GetCurveWrapper(curve, audioCurve));
          }
        }
      }
      return curveWrapperList.ToArray();
    }

    private CurveWrapper GetCurveWrapper(AnimationCurve curve, AudioSourceInspector.AudioCurveWrapper audioCurve)
    {
      float num = EditorGUIUtility.isProSkin ? 1f : 0.9f;
      Color color = new Color(num, num, num, 1f);
      CurveWrapper curveWrapper = new CurveWrapper();
      curveWrapper.id = audioCurve.id;
      curveWrapper.groupId = -1;
      curveWrapper.color = audioCurve.color * color;
      curveWrapper.hidden = false;
      curveWrapper.readOnly = false;
      curveWrapper.renderer = (CurveRenderer) new NormalCurveRenderer(curve);
      curveWrapper.renderer.SetCustomRange(0.0f, 1f);
      curveWrapper.getAxisUiScalarsCallback = new CurveWrapper.GetAxisScalarsCallback(this.GetAxisScalars);
      return curveWrapper;
    }

    public Vector2 GetAxisScalars()
    {
      return new Vector2(this.m_MaxDistance.floatValue, 1f);
    }

    private static float LogarithmicValue(float distance, float minDistance, float rolloffScale)
    {
      if ((double) distance > (double) minDistance && (double) rolloffScale != 1.0)
      {
        distance -= minDistance;
        distance *= rolloffScale;
        distance += minDistance;
      }
      if ((double) distance < 9.99999997475243E-07)
        distance = 1E-06f;
      return minDistance / distance;
    }

    private static AnimationCurve Logarithmic(float timeStart, float timeEnd, float logBase)
    {
      List<Keyframe> keyframeList = new List<Keyframe>();
      float num1 = 2f;
      timeStart = Mathf.Max(timeStart, 0.0001f);
      float num2 = timeStart;
      while ((double) num2 < (double) timeEnd)
      {
        float num3 = AudioSourceInspector.LogarithmicValue(num2, timeStart, logBase);
        float num4 = num2 / 50f;
        float num5 = (float) (((double) AudioSourceInspector.LogarithmicValue(num2 + num4, timeStart, logBase) - (double) AudioSourceInspector.LogarithmicValue(num2 - num4, timeStart, logBase)) / ((double) num4 * 2.0));
        keyframeList.Add(new Keyframe(num2, num3, num5, num5));
        num2 *= num1;
      }
      float num6 = AudioSourceInspector.LogarithmicValue(timeEnd, timeStart, logBase);
      float num7 = timeEnd / 50f;
      float num8 = (float) (((double) AudioSourceInspector.LogarithmicValue(timeEnd + num7, timeStart, logBase) - (double) AudioSourceInspector.LogarithmicValue(timeEnd - num7, timeStart, logBase)) / ((double) num7 * 2.0));
      keyframeList.Add(new Keyframe(timeEnd, num6, num8, num8));
      return new AnimationCurve(keyframeList.ToArray());
    }

    private static void InitStyles()
    {
      if (AudioSourceInspector.ms_Styles != null)
        return;
      AudioSourceInspector.ms_Styles = new AudioSourceInspector.Styles();
    }

    private void Update()
    {
      Vector3 listenerPos = AudioUtil.GetListenerPos();
      if ((double) (this.m_LastListenerPosition - listenerPos).sqrMagnitude <= 9.99999974737875E-05)
        return;
      this.m_LastListenerPosition = listenerPos;
      this.Repaint();
    }

    private void UndoRedoPerformed()
    {
      this.m_RefreshCurveEditor = true;
    }

    private void HandleLowPassFilter()
    {
      AudioSourceInspector.AudioCurveWrapper audioCurve = this.m_AudioCurves[3];
      AudioLowPassFilter[] audioLowPassFilterArray = new AudioLowPassFilter[this.targets.Length];
      for (int index = 0; index < this.targets.Length; ++index)
      {
        audioLowPassFilterArray[index] = ((Component) this.targets[index]).GetComponent<AudioLowPassFilter>();
        if ((UnityEngine.Object) audioLowPassFilterArray[index] == (UnityEngine.Object) null)
        {
          this.m_LowpassObject = (SerializedObject) null;
          audioCurve.curveProp = (SerializedProperty) null;
          return;
        }
      }
      if (audioCurve.curveProp != null)
        return;
      this.m_LowpassObject = new SerializedObject((UnityEngine.Object[]) audioLowPassFilterArray);
      audioCurve.curveProp = this.m_LowpassObject.FindProperty("lowpassLevelCustomCurve");
    }

    public override void OnInspectorGUI()
    {
      AudioSourceInspector.InitStyles();
      this.serializedObject.Update();
      if (this.m_LowpassObject != null)
        this.m_LowpassObject.Update();
      this.HandleLowPassFilter();
      foreach (AudioSourceInspector.AudioCurveWrapper audioCurve in this.m_AudioCurves)
      {
        CurveWrapper curveWrapperById = this.m_CurveEditor.getCurveWrapperById(audioCurve.id);
        if (audioCurve.curveProp != null)
        {
          AnimationCurve animationCurveValue = audioCurve.curveProp.animationCurveValue;
          if (curveWrapperById == null != audioCurve.curveProp.hasMultipleDifferentValues)
            this.m_RefreshCurveEditor = true;
          else if (curveWrapperById != null)
          {
            if (curveWrapperById.curve.length == 0)
              this.m_RefreshCurveEditor = true;
            else if (animationCurveValue.length >= 1 && (double) animationCurveValue.keys[0].value != (double) curveWrapperById.curve.keys[0].value)
              this.m_RefreshCurveEditor = true;
          }
        }
        else if (curveWrapperById != null)
          this.m_RefreshCurveEditor = true;
      }
      this.UpdateWrappersAndLegend();
      EditorGUILayout.PropertyField(this.m_AudioClip, AudioSourceInspector.ms_Styles.audioClipLabel, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_OutputAudioMixerGroup, AudioSourceInspector.ms_Styles.outputMixerGroupLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Mute);
      if (AudioUtil.canUseSpatializerEffect)
        EditorGUILayout.PropertyField(this.m_Spatialize);
      EditorGUILayout.PropertyField(this.m_BypassEffects);
      bool flag = ((IEnumerable<UnityEngine.Object>) this.targets).Any<UnityEngine.Object>((Func<UnityEngine.Object, bool>) (t => (UnityEngine.Object) (t as AudioSource).outputAudioMixerGroup != (UnityEngine.Object) null));
      if (flag)
        EditorGUI.BeginDisabledGroup(true);
      EditorGUILayout.PropertyField(this.m_BypassListenerEffects);
      if (flag)
        EditorGUI.EndDisabledGroup();
      EditorGUILayout.PropertyField(this.m_BypassReverbZones);
      EditorGUILayout.PropertyField(this.m_PlayOnAwake);
      EditorGUILayout.PropertyField(this.m_Loop);
      EditorGUILayout.Space();
      EditorGUIUtility.sliderLabels.SetLabels(AudioSourceInspector.ms_Styles.priorityLeftLabel, AudioSourceInspector.ms_Styles.priorityRightLabel);
      EditorGUILayout.IntSlider(this.m_Priority, 0, 256, AudioSourceInspector.ms_Styles.priorityLabel, new GUILayoutOption[0]);
      EditorGUIUtility.sliderLabels.SetLabels((GUIContent) null, (GUIContent) null);
      EditorGUILayout.Space();
      EditorGUILayout.Slider(this.m_Volume, 0.0f, 1f, AudioSourceInspector.ms_Styles.volumeLabel, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUILayout.Slider(this.m_Pitch, -3f, 3f, AudioSourceInspector.ms_Styles.pitchLabel, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUIUtility.sliderLabels.SetLabels(AudioSourceInspector.ms_Styles.panLeftLabel, AudioSourceInspector.ms_Styles.panRightLabel);
      EditorGUILayout.Slider(this.m_Pan2D, -1f, 1f, AudioSourceInspector.ms_Styles.panStereoLabel, new GUILayoutOption[0]);
      EditorGUIUtility.sliderLabels.SetLabels((GUIContent) null, (GUIContent) null);
      EditorGUILayout.Space();
      EditorGUIUtility.sliderLabels.SetLabels(AudioSourceInspector.ms_Styles.spatialLeftLabel, AudioSourceInspector.ms_Styles.spatialRightLabel);
      AudioSourceInspector.AnimProp(AudioSourceInspector.ms_Styles.spatialBlendLabel, this.m_AudioCurves[1].curveProp, 0.0f, 1f, false);
      EditorGUIUtility.sliderLabels.SetLabels((GUIContent) null, (GUIContent) null);
      EditorGUILayout.Space();
      AudioSourceInspector.AnimProp(AudioSourceInspector.ms_Styles.reverbZoneMixLabel, this.m_AudioCurves[4].curveProp, 0.0f, 1.1f, false);
      EditorGUILayout.Space();
      this.m_Expanded3D = EditorGUILayout.Foldout(this.m_Expanded3D, "3D Sound Settings");
      if (this.m_Expanded3D)
      {
        ++EditorGUI.indentLevel;
        this.Audio3DGUI();
        --EditorGUI.indentLevel;
      }
      this.serializedObject.ApplyModifiedProperties();
      if (this.m_LowpassObject == null)
        return;
      this.m_LowpassObject.ApplyModifiedProperties();
    }

    private static void SetRolloffToTarget(SerializedProperty property, UnityEngine.Object target)
    {
      property.SetToValueOfTarget(target);
      property.serializedObject.FindProperty("rolloffMode").SetToValueOfTarget(target);
      property.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    private void Audio3DGUI()
    {
      EditorGUILayout.Slider(this.m_DopplerLevel, 0.0f, 5f, AudioSourceInspector.ms_Styles.dopplerLevelLabel, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      AudioSourceInspector.AnimProp(AudioSourceInspector.ms_Styles.spreadLabel, this.m_AudioCurves[2].curveProp, 0.0f, 360f, true);
      if (this.m_RolloffMode.hasMultipleDifferentValues || this.m_RolloffMode.enumValueIndex == 2 && this.m_AudioCurves[0].curveProp.hasMultipleDifferentValues)
      {
        EditorGUILayout.TargetChoiceField(this.m_AudioCurves[0].curveProp, AudioSourceInspector.ms_Styles.rolloffLabel, new TargetChoiceHandler.TargetChoiceMenuFunction(AudioSourceInspector.SetRolloffToTarget), new GUILayoutOption[0]);
      }
      else
      {
        EditorGUILayout.PropertyField(this.m_RolloffMode, AudioSourceInspector.ms_Styles.rolloffLabel, new GUILayoutOption[0]);
        if (this.m_RolloffMode.enumValueIndex != 2)
        {
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_MinDistance);
          if (EditorGUI.EndChangeCheck())
            this.m_MinDistance.floatValue = Mathf.Clamp(this.m_MinDistance.floatValue, 0.0f, this.m_MaxDistance.floatValue / 1.01f);
        }
        else
        {
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.LabelField(this.m_MinDistance.displayName, AudioSourceInspector.ms_Styles.controlledByCurveLabel, new GUILayoutOption[0]);
          EditorGUI.EndDisabledGroup();
        }
      }
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_MaxDistance);
      if (EditorGUI.EndChangeCheck())
        this.m_MaxDistance.floatValue = Mathf.Min(Mathf.Max(Mathf.Max(this.m_MaxDistance.floatValue, 0.01f), this.m_MinDistance.floatValue * 1.01f), 1000000f);
      if (EditorGUI.EndChangeCheck())
        this.m_RefreshCurveEditor = true;
      Rect aspectRect = GUILayoutUtility.GetAspectRect(1.333f, GUI.skin.textField);
      aspectRect.xMin += EditorGUI.indent;
      if (Event.current.type != EventType.Layout && Event.current.type != EventType.Used)
        this.m_CurveEditor.rect = new Rect(aspectRect.x, aspectRect.y, aspectRect.width, aspectRect.height);
      this.UpdateWrappersAndLegend();
      GUI.Label(this.m_CurveEditor.drawRect, GUIContent.none, (GUIStyle) "TextField");
      this.m_CurveEditor.hRangeLocked = Event.current.shift;
      this.m_CurveEditor.vRangeLocked = EditorGUI.actionKey;
      this.m_CurveEditor.OnGUI();
      if (this.targets.Length == 1)
      {
        AudioSource target = (AudioSource) this.target;
        if (UnityEngine.Object.FindObjectOfType(typeof (AudioListener)) != (UnityEngine.Object) null)
          this.DrawLabel("Listener", (AudioUtil.GetListenerPos() - target.transform.position).magnitude, aspectRect);
      }
      this.DrawLegend();
      foreach (AudioSourceInspector.AudioCurveWrapper audioCurve in this.m_AudioCurves)
      {
        if (this.m_CurveEditor.getCurveWrapperById(audioCurve.id) != null && this.m_CurveEditor.getCurveWrapperById(audioCurve.id).changed)
        {
          AnimationCurve curve = this.m_CurveEditor.getCurveWrapperById(audioCurve.id).curve;
          if (curve.length > 0)
          {
            audioCurve.curveProp.animationCurveValue = curve;
            this.m_CurveEditor.getCurveWrapperById(audioCurve.id).changed = false;
            if (audioCurve.type == AudioSourceInspector.AudioCurveType.Volume)
              this.m_RolloffMode.enumValueIndex = 2;
          }
        }
      }
    }

    private void UpdateWrappersAndLegend()
    {
      if (!this.m_RefreshCurveEditor)
        return;
      this.m_CurveEditor.animationCurves = this.GetCurveWrapperArray();
      this.SyncShownCurvesToLegend(this.GetShownAudioCurves());
      this.m_RefreshCurveEditor = false;
    }

    private void DrawLegend()
    {
      List<Rect> rectList = new List<Rect>();
      List<AudioSourceInspector.AudioCurveWrapper> shownAudioCurves = this.GetShownAudioCurves();
      Rect rect = GUILayoutUtility.GetRect(10f, 20f);
      rect.x += 4f + EditorGUI.indent;
      rect.width -= 8f + EditorGUI.indent;
      int num = Mathf.Min(75, Mathf.FloorToInt(rect.width / (float) shownAudioCurves.Count));
      for (int index = 0; index < shownAudioCurves.Count; ++index)
        rectList.Add(new Rect(rect.x + (float) (num * index), rect.y, (float) num, rect.height));
      bool flag1 = false;
      if (shownAudioCurves.Count != this.m_SelectedCurves.Length)
      {
        this.m_SelectedCurves = new bool[shownAudioCurves.Count];
        flag1 = true;
      }
      if (EditorGUIExt.DragSelection(rectList.ToArray(), ref this.m_SelectedCurves, GUIStyle.none) || flag1)
      {
        bool flag2 = false;
        for (int index = 0; index < shownAudioCurves.Count; ++index)
        {
          if (this.m_SelectedCurves[index])
            flag2 = true;
        }
        if (!flag2)
        {
          for (int index = 0; index < shownAudioCurves.Count; ++index)
            this.m_SelectedCurves[index] = true;
        }
        this.SyncShownCurvesToLegend(shownAudioCurves);
      }
      for (int index = 0; index < shownAudioCurves.Count; ++index)
      {
        EditorGUI.DrawLegend(rectList[index], shownAudioCurves[index].color, shownAudioCurves[index].legend.text, this.m_SelectedCurves[index]);
        if (shownAudioCurves[index].curveProp.hasMultipleDifferentValues)
          GUI.Button(new Rect(rectList[index].x, rectList[index].y + 20f, rectList[index].width, 20f), "Different");
      }
    }

    private List<AudioSourceInspector.AudioCurveWrapper> GetShownAudioCurves()
    {
      return ((IEnumerable<AudioSourceInspector.AudioCurveWrapper>) this.m_AudioCurves).Where<AudioSourceInspector.AudioCurveWrapper>((Func<AudioSourceInspector.AudioCurveWrapper, bool>) (f => this.m_CurveEditor.getCurveWrapperById(f.id) != null)).ToList<AudioSourceInspector.AudioCurveWrapper>();
    }

    private void SyncShownCurvesToLegend(List<AudioSourceInspector.AudioCurveWrapper> curves)
    {
      if (curves.Count != this.m_SelectedCurves.Length)
        return;
      for (int index = 0; index < curves.Count; ++index)
        this.m_CurveEditor.getCurveWrapperById(curves[index].id).hidden = !this.m_SelectedCurves[index];
      this.m_CurveEditor.animationCurves = this.m_CurveEditor.animationCurves;
    }

    private void DrawLabel(string label, float value, Rect r)
    {
      Vector2 vector2 = AudioSourceInspector.ms_Styles.labelStyle.CalcSize(new GUIContent(label));
      vector2.x += 2f;
      Vector2 viewTransformPoint1 = this.m_CurveEditor.DrawingToViewTransformPoint(new Vector2(value / this.m_MaxDistance.floatValue, 0.0f));
      Vector2 viewTransformPoint2 = this.m_CurveEditor.DrawingToViewTransformPoint(new Vector2(value / this.m_MaxDistance.floatValue, 1f));
      GUI.BeginGroup(r);
      Color color = Handles.color;
      Handles.color = new Color(1f, 0.0f, 0.0f, 0.3f);
      Handles.DrawLine(new Vector3(viewTransformPoint1.x, viewTransformPoint1.y, 0.0f), new Vector3(viewTransformPoint2.x, viewTransformPoint2.y, 0.0f));
      Handles.DrawLine(new Vector3(viewTransformPoint1.x + 1f, viewTransformPoint1.y, 0.0f), new Vector3(viewTransformPoint2.x + 1f, viewTransformPoint2.y, 0.0f));
      Handles.color = color;
      GUI.Label(new Rect(Mathf.Floor(viewTransformPoint2.x - vector2.x / 2f), 2f, vector2.x, 15f), label, AudioSourceInspector.ms_Styles.labelStyle);
      GUI.EndGroup();
    }

    internal static void AnimProp(GUIContent label, SerializedProperty prop, float min, float max, bool useNormalizedValue)
    {
      AudioSourceInspector.InitStyles();
      if (prop.hasMultipleDifferentValues)
      {
        EditorGUILayout.TargetChoiceField(prop, label);
      }
      else
      {
        AnimationCurve animationCurveValue = prop.animationCurveValue;
        if (animationCurveValue == null)
          Debug.LogError((object) (label.text + " curve is null!"));
        else if (animationCurveValue.length == 0)
        {
          Debug.LogError((object) (label.text + " curve has no keys!"));
        }
        else
        {
          if (animationCurveValue.length != 1)
          {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField(label.text, AudioSourceInspector.ms_Styles.controlledByCurveLabel, new GUILayoutOption[0]);
            EditorGUI.EndDisabledGroup();
          }
          else
          {
            float num1 = MathUtils.DiscardLeastSignificantDecimal(!useNormalizedValue ? animationCurveValue.keys[0].value : Mathf.Lerp(min, max, animationCurveValue.keys[0].value));
            EditorGUI.BeginChangeCheck();
            float num2 = (double) max <= (double) min ? EditorGUILayout.Slider(label, num1, max, min, new GUILayoutOption[0]) : EditorGUILayout.Slider(label, num1, min, max, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck())
            {
              Keyframe key = animationCurveValue.keys[0];
              key.time = 0.0f;
              key.value = !useNormalizedValue ? num2 : Mathf.InverseLerp(min, max, num2);
              animationCurveValue.MoveKey(0, key);
            }
          }
          prop.animationCurveValue = animationCurveValue;
        }
      }
    }

    private void OnSceneGUI()
    {
      AudioSource target = (AudioSource) this.target;
      Color color = Handles.color;
      Handles.color = !target.enabled ? new Color(0.3f, 0.4f, 0.6f, 0.5f) : new Color(0.5f, 0.7f, 1f, 0.5f);
      Vector3 position = target.transform.position;
      EditorGUI.BeginChangeCheck();
      float num1 = Handles.RadiusHandle(Quaternion.identity, position, target.minDistance, true);
      float num2 = Handles.RadiusHandle(Quaternion.identity, position, target.maxDistance, true);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject((UnityEngine.Object) target, "AudioSource Distance");
        target.minDistance = num1;
        target.maxDistance = num2;
        this.m_RefreshCurveEditor = true;
      }
      Handles.color = color;
    }

    private class AudioCurveWrapper
    {
      public AudioSourceInspector.AudioCurveType type;
      public GUIContent legend;
      public int id;
      public Color color;
      public SerializedProperty curveProp;
      public float rangeMin;
      public float rangeMax;

      public AudioCurveWrapper(AudioSourceInspector.AudioCurveType type, string legend, int id, Color color, SerializedProperty curveProp, float rangeMin, float rangeMax)
      {
        this.type = type;
        this.legend = new GUIContent(legend);
        this.id = id;
        this.color = color;
        this.curveProp = curveProp;
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
      }
    }

    private enum AudioCurveType
    {
      Volume,
      SpatialBlend,
      Lowpass,
      Spread,
      ReverbZoneMix,
    }

    private class Styles
    {
      public GUIStyle labelStyle = (GUIStyle) "ProfilerBadge";
      public GUIContent rolloffLabel = new GUIContent("Volume Rolloff", "Which type of rolloff curve to use");
      public string controlledByCurveLabel = "Controlled by curve";
      public GUIContent audioClipLabel = new GUIContent("AudioClip", "The AudioClip asset played by the AudioSource. Can be undefined if the AudioSource is generating a live stream of audio via OnAudioFilterRead.");
      public GUIContent panStereoLabel = new GUIContent("Stereo Pan", "Only valid for Mono and Stereo AudioClips. Mono sounds will be panned at constant power left and right. Stereo sounds will Stereo sounds have each left/right value faded up and down according to the specified pan value.");
      public GUIContent spatialBlendLabel = new GUIContent("Spatial Blend", "Sets how much this AudioSource is treated as a 3D source. 3D sources are affected by spatial position and spread. If 3D Pan Level is 0, all spatial attenuation is ignored.");
      public GUIContent reverbZoneMixLabel = new GUIContent("Reverb Zone Mix", "Sets how much of the signal this AudioSource is mixing into the global reverb associated with the zones. [0, 1] is a linear range (like volume) while [1, 1.1] lets you boost the reverb mix by 10 dB.");
      public GUIContent dopplerLevelLabel = new GUIContent("Doppler Level", "Specifies how much the pitch is changed based on the relative velocity between AudioListener and AudioSource.");
      public GUIContent spreadLabel = new GUIContent("Spread", "Sets the spread of a 3d sound in speaker space");
      public GUIContent outputMixerGroupLabel = new GUIContent("Output", "Set whether the sound should play through an Audio Mixer first or directly to the Audio Listener");
      public GUIContent volumeLabel = new GUIContent("Volume", "Sets the overall volume of the sound.");
      public GUIContent pitchLabel = new GUIContent("Pitch", "Sets the frequency of the sound. Use this to slow down or speed up the sound.");
      public GUIContent priorityLabel = new GUIContent("Priority", "Sets the priority of the source. Note that a sound with a larger priority value will more likely be stolen by sounds with smaller priority values.");
      public GUIContent priorityLeftLabel = new GUIContent("High");
      public GUIContent priorityRightLabel = new GUIContent("Low");
      public GUIContent spatialLeftLabel = new GUIContent("2D");
      public GUIContent spatialRightLabel = new GUIContent("3D");
      public GUIContent panLeftLabel = new GUIContent("Left");
      public GUIContent panRightLabel = new GUIContent("Right");
    }
  }
}
