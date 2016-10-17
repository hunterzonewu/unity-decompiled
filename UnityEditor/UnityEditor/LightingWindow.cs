// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineInternal;

namespace UnityEditor
{
  [EditorWindowTitle(icon = "Lighting", title = "Lighting")]
  internal class LightingWindow : EditorWindow
  {
    private static string[] s_BakeModeOptions = new string[2]{ "Bake Reflection Probes", "Clear Baked Data" };
    private static bool s_IsVisible = false;
    private GUIContent[] kConcurrentJobsTypeStrings = new GUIContent[3]{ new GUIContent("Min"), new GUIContent("Low"), new GUIContent("High") };
    private int[] kConcurrentJobsTypeValues = new int[3]{ 0, 1, 2 };
    private float kButtonWidth = 120f;
    private GUIContent[] kModeStrings = new GUIContent[3]{ new GUIContent("Non-Directional"), new GUIContent("Directional"), new GUIContent("Directional Specular") };
    private int[] kModeValues = new int[3]{ 0, 1, 2 };
    private GUIContent[] kMaxAtlasSizeStrings = new GUIContent[8]{ new GUIContent("32"), new GUIContent("64"), new GUIContent("128"), new GUIContent("256"), new GUIContent("512"), new GUIContent("1024"), new GUIContent("2048"), new GUIContent("4096") };
    private int[] kMaxAtlasSizeValues = new int[8]{ 32, 64, 128, 256, 512, 1024, 2048, 4096 };
    private LightingWindow.Mode m_Mode = LightingWindow.Mode.BakeSettings;
    private Vector2 m_ScrollPosition = Vector2.zero;
    private AnimBool m_ShowIndirectResolution = new AnimBool();
    private PreviewResizer m_PreviewResizer = new PreviewResizer();
    private const string kGlobalIlluminationUnityManualPage = "file:///unity/Manual/GlobalIllumination.html";
    private const string kShowRealtimeSettingsKey = "ShowRealtimeLightingSettings";
    private const string kShowBakeSettingsKey = "ShowBakedLightingSettings";
    private const string kShowGeneralSettingsKey = "ShowGeneralLightingSettings";
    private const float kToolbarPadding = 38f;
    private LightingWindowObjectTab m_ObjectTab;
    private LightingWindowLightmapPreviewTab m_LightmapPreviewTab;
    private bool m_ShowDevOptions;
    private Editor m_LightingEditor;
    private Editor m_FogEditor;
    private Editor m_OtherRenderingEditor;
    private bool m_ShowRealtimeSettings;
    private bool m_ShowBakeSettings;
    private bool m_ShowGeneralSettings;
    private static LightingWindow.Styles s_Styles;

    private static LightingWindow.Styles styles
    {
      get
      {
        return LightingWindow.s_Styles ?? (LightingWindow.s_Styles = new LightingWindow.Styles());
      }
    }

    private UnityEngine.Object renderSettings
    {
      get
      {
        return RenderSettings.GetRenderSettings();
      }
    }

    private Editor lightingEditor
    {
      get
      {
        Editor.CreateCachedEditor(this.renderSettings, typeof (LightingEditor), ref this.m_LightingEditor);
        (this.m_LightingEditor as LightingEditor).parentWindow = (EditorWindow) this;
        return this.m_LightingEditor;
      }
    }

    private Editor fogEditor
    {
      get
      {
        Editor.CreateCachedEditor(this.renderSettings, typeof (FogEditor), ref this.m_FogEditor);
        return this.m_FogEditor;
      }
    }

    private Editor otherRenderingEditor
    {
      get
      {
        Editor.CreateCachedEditor(this.renderSettings, typeof (OtherRenderingEditor), ref this.m_OtherRenderingEditor);
        return this.m_OtherRenderingEditor;
      }
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      this.m_LightmapPreviewTab = new LightingWindowLightmapPreviewTab();
      this.m_ObjectTab = new LightingWindowObjectTab();
      this.m_ObjectTab.OnEnable((EditorWindow) this);
      this.m_ShowRealtimeSettings = SessionState.GetBool("ShowRealtimeLightingSettings", true);
      this.m_ShowBakeSettings = SessionState.GetBool("ShowBakedLightingSettings", true);
      this.m_ShowGeneralSettings = SessionState.GetBool("ShowGeneralLightingSettings", true);
      this.UpdateAnimatedBools(true);
      this.autoRepaintOnSceneChange = true;
      this.m_PreviewResizer.Init("LightmappingPreview");
      EditorApplication.searchChanged += new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
      this.Repaint();
    }

    private void UpdateAnimatedBools(bool initialize)
    {
      this.SetOptions(this.m_ShowIndirectResolution, initialize, !Lightmapping.realtimeLightmapsEnabled);
    }

    private void SetOptions(AnimBool animBool, bool initialize, bool targetValue)
    {
      if (initialize)
      {
        animBool.value = targetValue;
        animBool.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      }
      else
        animBool.target = targetValue;
    }

    private void OnDisable()
    {
      this.ClearCachedProperties();
      this.m_ObjectTab.OnDisable();
      SessionState.SetBool("ShowRealtimeLightingSettings", this.m_ShowRealtimeSettings);
      SessionState.SetBool("ShowBakedLightingSettings", this.m_ShowBakeSettings);
      SessionState.SetBool("ShowGeneralLightingSettings", this.m_ShowGeneralSettings);
      EditorApplication.searchChanged -= new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
    }

    private void ClearCachedProperties()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_LightingEditor);
      this.m_LightingEditor = (Editor) null;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_FogEditor);
      this.m_FogEditor = (Editor) null;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_OtherRenderingEditor);
      this.m_OtherRenderingEditor = (Editor) null;
    }

    private void OnBecameVisible()
    {
      if (LightingWindow.s_IsVisible)
        return;
      LightingWindow.s_IsVisible = true;
      LightmapVisualization.enabled = true;
      LightingWindow.RepaintSceneAndGameViews();
    }

    private void OnBecameInvisible()
    {
      LightingWindow.s_IsVisible = false;
      LightmapVisualization.enabled = false;
      LightingWindow.RepaintSceneAndGameViews();
    }

    private void OnSelectionChange()
    {
      this.m_LightmapPreviewTab.UpdateLightmapSelection();
      if (this.m_Mode != LightingWindow.Mode.ObjectSettings && this.m_Mode != LightingWindow.Mode.Maps)
        return;
      this.Repaint();
    }

    internal static void RepaintSceneAndGameViews()
    {
      SceneView.RepaintAll();
      GameView.RepaintAll();
    }

    private void OnGUI()
    {
      this.UpdateAnimatedBools(false);
      EditorGUIUtility.labelWidth = 130f;
      EditorGUILayout.Space();
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(38f);
      this.ModeToggle();
      GUILayout.FlexibleSpace();
      this.DrawHelpGUI();
      if (this.m_Mode == LightingWindow.Mode.BakeSettings)
        this.DrawSettingsGUI();
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
      this.m_ScrollPosition = EditorGUILayout.BeginScrollView(this.m_ScrollPosition);
      switch (this.m_Mode)
      {
        case LightingWindow.Mode.ObjectSettings:
          this.m_ObjectTab.ObjectSettings();
          break;
        case LightingWindow.Mode.BakeSettings:
          this.lightingEditor.OnInspectorGUI();
          this.EnlightenBakeSettings();
          this.fogEditor.OnInspectorGUI();
          this.otherRenderingEditor.OnInspectorGUI();
          break;
        case LightingWindow.Mode.Maps:
          this.m_LightmapPreviewTab.Maps();
          break;
      }
      EditorGUILayout.EndScrollView();
      EditorGUILayout.Space();
      GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;
      this.Buttons();
      GUI.enabled = true;
      EditorGUILayout.Space();
      this.Summary();
      this.PreviewSection();
    }

    private void DrawHelpGUI()
    {
      if (!GUI.Button(GUILayoutUtility.GetRect(16f, 16f), new GUIContent(EditorGUI.GUIContents.helpIcon), GUIStyle.none))
        return;
      Help.ShowHelpPage("file:///unity/Manual/GlobalIllumination.html");
    }

    private void DrawSettingsGUI()
    {
      Rect rect = GUILayoutUtility.GetRect(16f, 16f);
      if (!EditorGUI.ButtonMouseDown(rect, EditorGUI.GUIContents.titleSettingsIcon, FocusType.Native, GUIStyle.none))
        return;
      EditorUtility.DisplayCustomMenu(rect, new GUIContent[1]
      {
        new GUIContent("Reset")
      }, -1, new EditorUtility.SelectMenuItemFunction(this.ResetSettings), (object) null);
    }

    private void ResetSettings(object userData, string[] options, int selected)
    {
      RenderSettings.Reset();
      LightmapEditorSettings.Reset();
      LightmapSettings.Reset();
    }

    private void PreviewSection()
    {
      EditorGUILayout.BeginHorizontal(GUIContent.none, (GUIStyle) "preToolbar", GUILayout.Height(17f));
      GUILayout.FlexibleSpace();
      GUI.Label(GUILayoutUtility.GetLastRect(), "Preview", (GUIStyle) "preToolbar2");
      EditorGUILayout.EndHorizontal();
      float height = this.m_PreviewResizer.ResizeHandle(this.position, 100f, 250f, 17f);
      Rect r = new Rect(0.0f, this.position.height - height, this.position.width, height);
      switch (this.m_Mode)
      {
        case LightingWindow.Mode.ObjectSettings:
          if (!(bool) ((UnityEngine.Object) Selection.activeGameObject))
            break;
          this.m_ObjectTab.ObjectPreview(r);
          break;
        case LightingWindow.Mode.Maps:
          if ((double) height <= 0.0)
            break;
          this.m_LightmapPreviewTab.LightmapPreview(r);
          break;
      }
    }

    private void ModeToggle()
    {
      this.m_Mode = (LightingWindow.Mode) GUILayout.Toolbar((int) this.m_Mode, LightingWindow.styles.ModeToggles, (GUIStyle) "LargeButton", new GUILayoutOption[1]
      {
        GUILayout.Width(this.position.width - 76f)
      });
    }

    private void DeveloperBuildEnlightenSettings(SerializedObject so)
    {
      if (!Unsupported.IsDeveloperBuild())
        return;
      this.m_ShowDevOptions = EditorGUILayout.Foldout(this.m_ShowDevOptions, "Debug [internal]");
      if (!this.m_ShowDevOptions)
        return;
      SerializedProperty property1 = so.FindProperty("m_GISettings.m_BounceScale");
      SerializedProperty property2 = so.FindProperty("m_GISettings.m_TemporalCoherenceThreshold");
      ++EditorGUI.indentLevel;
      Lightmapping.concurrentJobsType = (Lightmapping.ConcurrentJobsType) EditorGUILayout.IntPopup(LightingWindow.styles.ConcurrentJobs, (int) Lightmapping.concurrentJobsType, this.kConcurrentJobsTypeStrings, this.kConcurrentJobsTypeValues, new GUILayoutOption[0]);
      Lightmapping.enlightenForceUpdates = EditorGUILayout.Toggle(LightingWindow.styles.ForceUpdates, Lightmapping.enlightenForceUpdates, new GUILayoutOption[0]);
      Lightmapping.enlightenForceWhiteAlbedo = EditorGUILayout.Toggle(LightingWindow.styles.ForceWhiteAlbedo, Lightmapping.enlightenForceWhiteAlbedo, new GUILayoutOption[0]);
      Lightmapping.filterMode = (UnityEngine.FilterMode) EditorGUILayout.EnumPopup(EditorGUIUtility.TempContent("Filter Mode"), (Enum) Lightmapping.filterMode, new GUILayoutOption[0]);
      EditorGUILayout.Slider(property1, 0.0f, 10f, LightingWindow.styles.BounceScale, new GUILayoutOption[0]);
      EditorGUILayout.Slider(property2, 0.0f, 1f, LightingWindow.styles.UpdateThreshold, new GUILayoutOption[0]);
      if (GUILayout.Button("Clear disk cache", new GUILayoutOption[1]{ GUILayout.Width(this.kButtonWidth) }))
      {
        Lightmapping.Clear();
        Lightmapping.ClearDiskCache();
      }
      if (GUILayout.Button("Print state to console", new GUILayoutOption[1]{ GUILayout.Width(this.kButtonWidth) }))
        Lightmapping.PrintStateToConsole();
      if (GUILayout.Button("Reset albedo/emissive", new GUILayoutOption[1]{ GUILayout.Width(this.kButtonWidth) }))
        GIDebugVisualisation.ResetRuntimeInputTextures();
      if (GUILayout.Button("Reset environment", new GUILayoutOption[1]{ GUILayout.Width(this.kButtonWidth) }))
        DynamicGI.UpdateEnvironment();
      --EditorGUI.indentLevel;
    }

    private void EnlightenBakeSettings()
    {
      SerializedObject so = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
      SerializedProperty property1 = so.FindProperty("m_GISettings.m_EnableRealtimeLightmaps");
      SerializedProperty property2 = so.FindProperty("m_GISettings.m_EnableBakedLightmaps");
      this.RealtimeGUI(so, property1, property2);
      this.BakedGUI(so, property1, property2);
      this.GeneralSettingsGUI(so, property1, property2);
      so.ApplyModifiedProperties();
    }

    private void GeneralSettingsGUI(SerializedObject so, SerializedProperty enableRealtimeGI, SerializedProperty enableBakedGI)
    {
      this.m_ShowGeneralSettings = EditorGUILayout.FoldoutTitlebar(this.m_ShowGeneralSettings, LightingWindow.styles.GeneralGILabel);
      if (!this.m_ShowGeneralSettings)
        return;
      SerializedProperty property1 = so.FindProperty("m_GISettings.m_AlbedoBoost");
      SerializedProperty property2 = so.FindProperty("m_GISettings.m_IndirectOutputScale");
      SerializedProperty property3 = so.FindProperty("m_LightmapEditorSettings.m_LightmapParameters");
      SerializedProperty property4 = so.FindProperty("m_LightmapsMode");
      EditorGUI.BeginDisabledGroup(!enableBakedGI.boolValue && !enableRealtimeGI.boolValue);
      ++EditorGUI.indentLevel;
      EditorGUILayout.IntPopup(property4, this.kModeStrings, this.kModeValues, LightingWindow.s_Styles.DirectionalMode, new GUILayoutOption[0]);
      if (property4.intValue == 1)
        EditorGUILayout.HelpBox(LightingWindow.s_Styles.NoDirectionalInSM2AndGLES2.text, MessageType.Warning);
      if (property4.intValue == 2)
        EditorGUILayout.HelpBox(LightingWindow.s_Styles.NoDirectionalSpecularInSM2AndGLES2.text, MessageType.Warning);
      EditorGUILayout.Slider(property2, 0.0f, 5f, LightingWindow.styles.IndirectOutputScale, new GUILayoutOption[0]);
      EditorGUILayout.Slider(property1, 1f, 10f, LightingWindow.styles.AlbedoBoost, new GUILayoutOption[0]);
      if (LightingWindowObjectTab.LightmapParametersGUI(property3, LightingWindow.styles.DefaultLightmapParameters))
        this.m_Mode = LightingWindow.Mode.ObjectSettings;
      this.DeveloperBuildEnlightenSettings(so);
      EditorGUI.EndDisabledGroup();
      --EditorGUI.indentLevel;
    }

    private void BakedGUI(SerializedObject so, SerializedProperty enableRealtimeGI, SerializedProperty enableBakedGI)
    {
      this.m_ShowBakeSettings = EditorGUILayout.ToggleTitlebar(this.m_ShowBakeSettings, LightingWindow.styles.BakedGILabel, enableBakedGI);
      if (!this.m_ShowBakeSettings)
        return;
      SerializedProperty property1 = so.FindProperty("m_LightmapEditorSettings.m_Resolution");
      SerializedProperty property2 = so.FindProperty("m_LightmapEditorSettings.m_BakeResolution");
      SerializedProperty property3 = so.FindProperty("m_LightmapEditorSettings.m_Padding");
      SerializedProperty property4 = so.FindProperty("m_LightmapEditorSettings.m_CompAOExponent");
      SerializedProperty property5 = so.FindProperty("m_LightmapEditorSettings.m_AOMaxDistance");
      SerializedProperty property6 = so.FindProperty("m_LightmapEditorSettings.m_TextureCompression");
      SerializedProperty property7 = so.FindProperty("m_LightmapEditorSettings.m_FinalGather");
      SerializedProperty property8 = so.FindProperty("m_LightmapEditorSettings.m_FinalGatherRayCount");
      SerializedProperty property9 = so.FindProperty("m_LightmapEditorSettings.m_TextureWidth");
      ++EditorGUI.indentLevel;
      EditorGUI.BeginDisabledGroup(!enableBakedGI.boolValue);
      LightingWindow.DrawLightmapResolutionField(property2, LightingWindow.styles.BakeResolution);
      GUILayout.BeginHorizontal();
      EditorGUILayout.PropertyField(property3, LightingWindow.styles.Padding, new GUILayoutOption[0]);
      GUILayout.Label(" texels", LightingWindow.styles.labelStyle, new GUILayoutOption[0]);
      GUILayout.EndHorizontal();
      EditorGUILayout.PropertyField(property6, LightingWindow.s_Styles.TextureCompression, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      this.m_ShowIndirectResolution.target = !enableRealtimeGI.boolValue;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowIndirectResolution.faded))
      {
        LightingWindow.DrawLightmapResolutionField(property1, LightingWindow.styles.IndirectResolution);
        EditorGUILayout.Space();
      }
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.Slider(property4, 0.0f, 10f, LightingWindow.styles.AmbientOcclusion, new GUILayoutOption[0]);
      if ((double) property4.floatValue > 0.0)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(property5, LightingWindow.styles.AOMaxDistance, new GUILayoutOption[0]);
        if ((double) property5.floatValue < 0.0)
          property5.floatValue = 0.0f;
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.PropertyField(property7, LightingWindow.s_Styles.FinalGather, new GUILayoutOption[0]);
      if (property7.boolValue)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(property8, LightingWindow.styles.FinalGatherRayCount, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.IntPopup(property9, this.kMaxAtlasSizeStrings, this.kMaxAtlasSizeValues, LightingWindow.styles.MaxAtlasSize, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      --EditorGUI.indentLevel;
    }

    private void RealtimeGUI(SerializedObject so, SerializedProperty enableRealtimeGI, SerializedProperty enableBakedGI)
    {
      this.m_ShowRealtimeSettings = EditorGUILayout.ToggleTitlebar(this.m_ShowRealtimeSettings, LightingWindow.styles.RealtimeGILabel, enableRealtimeGI);
      if (!this.m_ShowRealtimeSettings)
        return;
      SerializedProperty property1 = so.FindProperty("m_RuntimeCPUUsage");
      SerializedProperty property2 = so.FindProperty("m_LightmapEditorSettings.m_Resolution");
      ++EditorGUI.indentLevel;
      EditorGUI.BeginDisabledGroup(!enableRealtimeGI.boolValue);
      LightingWindow.DrawLightmapResolutionField(property2, LightingWindow.styles.Resolution);
      EditorGUILayout.IntPopup(property1, LightingWindow.styles.RuntimeCPUUsageStrings, LightingWindow.styles.RuntimeCPUUsageValues, LightingWindow.styles.RuntimeCPUUsage, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      --EditorGUI.indentLevel;
    }

    private static void DrawLightmapResolutionField(SerializedProperty resolution, GUIContent label)
    {
      GUILayout.BeginHorizontal();
      EditorGUILayout.PropertyField(resolution, label, new GUILayoutOption[0]);
      GUILayout.Label(" texels per unit", LightingWindow.styles.labelStyle, new GUILayoutOption[0]);
      GUILayout.EndHorizontal();
    }

    private void BakeDropDownCallback(object data)
    {
      switch ((LightingWindow.BakeMode) data)
      {
        case LightingWindow.BakeMode.BakeReflectionProbes:
          this.DoBakeReflectionProbes();
          break;
        case LightingWindow.BakeMode.Clear:
          this.DoClear();
          break;
      }
    }

    private void Buttons()
    {
      bool flag = Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.Iterative;
      if (flag)
        EditorGUILayout.HelpBox("Baking of lightmaps is automatic because the workflow mode is set to 'Auto'. The lightmap data is stored in the GI cache.", MessageType.Info);
      if ((bool) ((UnityEngine.Object) Lightmapping.lightingDataAsset) && !Lightmapping.lightingDataAsset.isValid)
        EditorGUILayout.HelpBox(Lightmapping.lightingDataAsset.validityErrorMessage, MessageType.Error);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      EditorGUI.BeginChangeCheck();
      bool disabled = GUILayout.Toggle(flag, LightingWindow.styles.ContinuousBakeLabel);
      if (EditorGUI.EndChangeCheck())
      {
        Lightmapping.giWorkflowMode = !disabled ? Lightmapping.GIWorkflowMode.OnDemand : Lightmapping.GIWorkflowMode.Iterative;
        InspectorWindow.RepaintAllInspectors();
      }
      EditorGUI.BeginDisabledGroup(disabled);
      if (disabled || !Lightmapping.isRunning)
      {
        if (EditorGUI.ButtonWithDropdownList(LightingWindow.styles.BuildLabel, LightingWindow.s_BakeModeOptions, new GenericMenu.MenuFunction2(this.BakeDropDownCallback), new GUILayoutOption[1]{ GUILayout.Width(180f) }))
        {
          this.DoBake();
          GUIUtility.ExitGUI();
        }
      }
      else if (GUILayout.Button("Cancel", new GUILayoutOption[1]{ GUILayout.Width(this.kButtonWidth) }))
      {
        Lightmapping.Cancel();
        Analytics.Track("/LightMapper/Cancel");
      }
      EditorGUI.EndDisabledGroup();
      GUILayout.EndHorizontal();
    }

    private void DoBake()
    {
      Analytics.Track("/LightMapper/Start");
      Analytics.Event("LightMapper", "Mode", LightmapSettings.lightmapsMode.ToString(), 1);
      Analytics.Event("LightMapper", "Button", "BakeScene", 1);
      Lightmapping.BakeAsync();
    }

    private void DoClear()
    {
      Lightmapping.ClearLightingDataAsset();
      Lightmapping.Clear();
      Analytics.Track("/LightMapper/Clear");
    }

    private void DoBakeReflectionProbes()
    {
      Lightmapping.BakeAllReflectionProbesSnapshots();
      Analytics.Track("/LightMapper/BakeAllReflectionProbesSnapshots");
    }

    private void Summary()
    {
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      int bytes = 0;
      int num = 0;
      Dictionary<Vector2, int> dictionary1 = new Dictionary<Vector2, int>();
      bool flag1 = false;
      foreach (LightmapData lightmap in LightmapSettings.lightmaps)
      {
        if (!((UnityEngine.Object) lightmap.lightmapFar == (UnityEngine.Object) null))
        {
          ++num;
          Vector2 key = new Vector2((float) lightmap.lightmapFar.width, (float) lightmap.lightmapFar.height);
          if (dictionary1.ContainsKey(key))
          {
            Dictionary<Vector2, int> dictionary2;
            Vector2 index;
            (dictionary2 = dictionary1)[index = key] = dictionary2[index] + 1;
          }
          else
            dictionary1.Add(key, 1);
          bytes += TextureUtil.GetStorageMemorySize((Texture) lightmap.lightmapFar);
          if ((bool) ((UnityEngine.Object) lightmap.lightmapNear))
          {
            bytes += TextureUtil.GetStorageMemorySize((Texture) lightmap.lightmapNear);
            flag1 = true;
          }
        }
      }
      string str = num.ToString() + (!flag1 ? " non-directional" : " directional") + " lightmap" + (num != 1 ? "s" : string.Empty);
      bool flag2 = true;
      using (Dictionary<Vector2, int>.Enumerator enumerator = dictionary1.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<Vector2, int> current = enumerator.Current;
          str += !flag2 ? ", " : ": ";
          flag2 = false;
          if (current.Value > 1)
            str = str + (object) current.Value + "x";
          str = str + (object) current.Key.x + "x" + (object) current.Key.y + "px";
        }
      }
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      GUILayout.Label(str + " ", LightingWindow.styles.labelStyle, new GUILayoutOption[0]);
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      GUILayout.Label(EditorUtility.FormatBytes(bytes), LightingWindow.styles.labelStyle, new GUILayoutOption[0]);
      GUILayout.Label(num != 0 ? string.Empty : "No Lightmaps", LightingWindow.styles.labelStyle, new GUILayoutOption[0]);
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
    }

    [MenuItem("Window/Lighting", false, 2098)]
    private static void CreateLightingWindow()
    {
      LightingWindow window = EditorWindow.GetWindow<LightingWindow>();
      window.minSize = new Vector2(300f, 360f);
      window.Show();
    }

    private enum Mode
    {
      ObjectSettings,
      BakeSettings,
      Maps,
    }

    private enum BakeMode
    {
      BakeReflectionProbes,
      Clear,
    }

    private class Styles
    {
      public GUIContent[] ModeToggles = new GUIContent[3]{ EditorGUIUtility.TextContent("Object|Bake settings for the currently selected object."), EditorGUIUtility.TextContent("Scene|Global GI settings."), EditorGUIUtility.TextContent("Lightmaps|The editable list of lightmaps.") };
      public int[] RuntimeCPUUsageValues = new int[4]{ 25, 50, 75, 100 };
      public GUIContent[] RuntimeCPUUsageStrings = new GUIContent[4]{ EditorGUIUtility.TextContent("Low (default)"), EditorGUIUtility.TextContent("Medium"), EditorGUIUtility.TextContent("High"), EditorGUIUtility.TextContent("Unlimited") };
      public GUIContent RuntimeCPUUsage = EditorGUIUtility.TextContent("CPU Usage|How much CPU usage to assign to the final lighting calculations at runtime. Increasing this makes the system react faster to changes in lighting at a cost of using more CPU time.");
      public GUIContent RealtimeGILabel = EditorGUIUtility.TextContent("Precomputed Realtime GI|Settings used in Precomputed Realtime Global Illumination where it is precomputed how indirect light can bounce between static objects, but the final lighting is done at runtime. Lights, ambient lighting in addition to the materials and emission of static objects can still be changed at runtime. Only static objects can affect GI by blocking and bouncing light, but non-static objects can receive bounced light via light probes.");
      public GUIContent BakedGILabel = EditorGUIUtility.TextContent("Baked GI|Settings used in Baked Global Illumination where direct and indirect lighting for static objects is precalculated and saved (baked) into lightmaps for use at runtime. This is useful when lights are known to be static, for mobile, for low end devices and other situations where there is not enough processing power to use Precomputed Realtime GI. You can toggle on each light whether it should be included in the bake.");
      public GUIContent GeneralGILabel = EditorGUIUtility.TextContent("General GI|Settings that apply to both Global Illumination modes (Precomputed Realtime and Baked).");
      public GUIContent ContinuousBakeLabel = EditorGUIUtility.TextContent("Auto|Automatically detects changes and builds lighting.");
      public GUIContent BuildLabel = EditorGUIUtility.TextContent("Build|Perform the precomputation (for Precomputed Realtime GI) and/or bake (for Baked GI) for the GI modes that are currently enabled.");
      public GUIContent IndirectResolution = EditorGUIUtility.TextContent("Indirect Resolution|Indirect lightmap resolution in texels per world unit.");
      public GUIContent UpdateRealtimeProbeLabel = EditorGUIUtility.TextContent("Update Realtime Probes");
      public GUIContent BounceScale = EditorGUIUtility.TextContent("Bounce Scale|Multiplier for indirect lighting. Use with care.");
      public GUIContent UpdateThreshold = EditorGUIUtility.TextContent("Update Threshold|Threshold for updating realtime GI. A lower value causes more frequent updates (default 1.0).");
      public GUIContent AlbedoBoost = EditorGUIUtility.TextContent("Bounce Boost|When light bounces off a surface it is multiplied by the albedo of this surface. This values intensifies albedo and thus affects how strong the light bounces from one surface to the next. Used for realtime and baked lightmaps.");
      public GUIContent IndirectOutputScale = EditorGUIUtility.TextContent("Indirect Intensity|Scales indirect lighting. Indirect is composed of bounce, emission and ambient lighting. Changes the amount of indirect light within the scene. Used for realtime and baked lightmaps.");
      public GUIContent Resolution = EditorGUIUtility.TextContent("Realtime Resolution|Realtime lightmap resolution in texels per world unit. This value is multiplied by the resolution in LightmapParameters to give the output lightmap resolution. This should generally be an order of magnitude less than what is common for baked lightmaps to keep the precompute time manageable and the performance at runtime acceptable.");
      public GUIContent BakeResolution = EditorGUIUtility.TextContent("Baked Resolution|Baked lightmap resolution in texels per world unit.");
      public GUIContent ConcurrentJobs = EditorGUIUtility.TextContent("Concurrent Jobs|The amount of simultaneously scheduled jobs.");
      public GUIContent ForceWhiteAlbedo = EditorGUIUtility.TextContent("Force White Albedo|Force white albedo during lighting calculations.");
      public GUIContent ForceUpdates = EditorGUIUtility.TextContent("Force Updates|Force continuous updates of runtime indirect lighting calculations.");
      public GUIContent AmbientOcclusion = EditorGUIUtility.TextContent("Ambient Occlusion|Changes contrast of ambient occlusion. Is only applied to the indirect lighting. Used for baked lightmaps.");
      public GUIContent AOMaxDistance = EditorGUIUtility.TextContent("Max Distance|Beyond this distance a ray is considered to be unoccluded. 0 for infinitely long rays.");
      public GUIContent DirectionalMode = EditorGUIUtility.TextContent("Directional Mode|Lightmaps encode incoming dominant light direction. More expensive in terms of memory and fill rate.");
      public GUIContent NoDirectionalSpecularInSM2AndGLES2 = EditorGUIUtility.TextContent("Directional Specular lightmaps cannot be decoded on SM2.0 hardware nor when using GLES2.0. There is currently no fallback.");
      public GUIContent NoDirectionalInSM2AndGLES2 = EditorGUIUtility.TextContent("Directional lightmaps cannot be decoded on SM2.0 hardware nor when using GLES2.0. They will fallback to Non-Directional lightmaps.");
      public GUIContent Padding = EditorGUIUtility.TextContent("Baked Padding|Texel separation between shapes.");
      public GUIContent MaxAtlasSize = EditorGUIUtility.TextContent("Atlas Size|The size of a lightmap.");
      public GUIContent TextureCompression = EditorGUIUtility.TextContent("Compressed|Improves performance and lowers space requirements but might introduce artifacts.");
      public GUIContent FinalGather = EditorGUIUtility.TextContent("Final Gather|Whether to use final gather. Final gather will improve visual quality significantly by using ray tracing at bake resolution for the last light bounce. This will increase bake time.");
      public GUIContent FinalGatherRayCount = EditorGUIUtility.TextContent("Ray Count|How many rays to use for final gather per bake output texel.");
      public GUIContent DefaultLightmapParameters = EditorGUIUtility.TextContent("Default Parameters|Lets you configure default lightmapping parameters for the scene. Objects will be automatically grouped by unique parameter sets.");
      public GUIContent SceneViewLightmapDisplay = EditorGUIUtility.TextContent("LightmapDisplay");
      public GUIStyle labelStyle = EditorStyles.wordWrappedMiniLabel;
    }
  }
}
