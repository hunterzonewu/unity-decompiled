// Decompiled with JetBrains decompiler
// Type: UnityEditor.PlayerSettingsEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.Modules;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (PlayerSettings))]
  internal class PlayerSettingsEditor : Editor
  {
    private static GUIContent[] kRenderPaths = new GUIContent[4]{ new GUIContent("Forward"), new GUIContent("Deferred"), new GUIContent("Legacy Vertex Lit"), new GUIContent("Legacy Deferred (light prepass)") };
    private static int[] kRenderPathValues = new int[4]{ 1, 3, 0, 2 };
    private static Dictionary<ScriptingImplementation, GUIContent> m_NiceScriptingBackendNames = new Dictionary<ScriptingImplementation, GUIContent>() { { ScriptingImplementation.Mono2x, EditorGUIUtility.TextContent("Mono2x") }, { ScriptingImplementation.WinRTDotNET, EditorGUIUtility.TextContent(".NET") }, { ScriptingImplementation.IL2CPP, EditorGUIUtility.TextContent("IL2CPP") } };
    private SavedInt m_SelectedSection = new SavedInt("PlayerSettings.ShownSection", -1);
    private PlayerSettingsEditor.WebPlayerTemplateManager m_WebPlayerTemplateManager = new PlayerSettingsEditor.WebPlayerTemplateManager();
    private Dictionary<BuildTarget, ReorderableList> m_GraphicsDeviceLists = new Dictionary<BuildTarget, ReorderableList>();
    private AnimBool[] m_SectionAnimators = new AnimBool[6];
    private readonly AnimBool m_ShowDefaultIsNativeResolution = new AnimBool();
    private readonly AnimBool m_ShowResolution = new AnimBool();
    private const int kSlotSize = 64;
    private const int kMaxPreviewSize = 96;
    private const int kIconSpacing = 6;
    private static PlayerSettingsEditor.Styles s_Styles;
    private BuildPlayerWindow.BuildPlatform[] validPlatforms;
    private SerializedProperty m_StripEngineCode;
    private SerializedProperty m_ApplicationBundleIdentifier;
    private SerializedProperty m_ApplicationBundleVersion;
    private SerializedProperty m_IPhoneApplicationDisplayName;
    private SerializedProperty m_IPhoneBuildNumber;
    private SerializedProperty m_LocationUsageDescription;
    private SerializedProperty m_ApiCompatibilityLevel;
    private SerializedProperty m_IPhoneStrippingLevel;
    private SerializedProperty m_IPhoneScriptCallOptimization;
    private SerializedProperty m_AotOptions;
    private SerializedProperty m_DefaultScreenOrientation;
    private SerializedProperty m_AllowedAutoRotateToPortrait;
    private SerializedProperty m_AllowedAutoRotateToPortraitUpsideDown;
    private SerializedProperty m_AllowedAutoRotateToLandscapeRight;
    private SerializedProperty m_AllowedAutoRotateToLandscapeLeft;
    private SerializedProperty m_UseOSAutoRotation;
    private SerializedProperty m_Use32BitDisplayBuffer;
    private SerializedProperty m_DisableDepthAndStencilBuffers;
    private SerializedProperty m_iosShowActivityIndicatorOnLoading;
    private SerializedProperty m_androidShowActivityIndicatorOnLoading;
    private SerializedProperty m_IPhoneSdkVersion;
    private SerializedProperty m_IPhoneTargetOSVersion;
    private SerializedProperty m_tvOSSdkVersion;
    private SerializedProperty m_tvOSTargetOSVersion;
    private SerializedProperty m_AndroidProfiler;
    private SerializedProperty m_UIPrerenderedIcon;
    private SerializedProperty m_UIRequiresPersistentWiFi;
    private SerializedProperty m_UIStatusBarHidden;
    private SerializedProperty m_UIRequiresFullScreen;
    private SerializedProperty m_UIStatusBarStyle;
    private SerializedProperty m_IOSAppInBackgroundBehavior;
    private SerializedProperty m_IOSAllowHTTPDownload;
    private SerializedProperty m_SubmitAnalytics;
    private SerializedProperty m_TargetDevice;
    private SerializedProperty m_AccelerometerFrequency;
    private SerializedProperty m_useOnDemandResources;
    private SerializedProperty m_OverrideIPodMusic;
    private SerializedProperty m_PrepareIOSForRecording;
    private SerializedProperty m_EnableInternalProfiler;
    private SerializedProperty m_ActionOnDotNetUnhandledException;
    private SerializedProperty m_LogObjCUncaughtExceptions;
    private SerializedProperty m_EnableCrashReportAPI;
    private SerializedProperty m_XboxTitleId;
    private SerializedProperty m_XboxImageXexPath;
    private SerializedProperty m_XboxSpaPath;
    private SerializedProperty m_XboxGenerateSpa;
    private SerializedProperty m_XboxDeployKinectResources;
    private SerializedProperty m_XboxPIXTextureCapture;
    private SerializedProperty m_XboxEnableAvatar;
    private SerializedProperty m_XboxEnableKinect;
    private SerializedProperty m_XboxEnableKinectAutoTracking;
    private SerializedProperty m_XboxEnableHeadOrientation;
    private SerializedProperty m_XboxDeployHeadOrientation;
    private SerializedProperty m_XboxDeployKinectHeadPosition;
    private SerializedProperty m_XboxSplashScreen;
    private SerializedProperty m_XboxEnableSpeech;
    private SerializedProperty m_XboxSpeechDB;
    private SerializedProperty m_XboxEnableFitness;
    private SerializedProperty m_XboxAdditionalTitleMemorySize;
    private SerializedProperty m_XboxEnableGuest;
    private SerializedProperty m_VideoMemoryForVertexBuffers;
    private SerializedProperty m_ps3SplashScreen;
    private SerializedProperty m_CompanyName;
    private SerializedProperty m_ProductName;
    private SerializedProperty m_DefaultCursor;
    private SerializedProperty m_CursorHotspot;
    private SerializedProperty m_ShowUnitySplashScreen;
    private SerializedProperty m_DefaultScreenWidth;
    private SerializedProperty m_DefaultScreenHeight;
    private SerializedProperty m_DefaultScreenWidthWeb;
    private SerializedProperty m_DefaultScreenHeightWeb;
    private SerializedProperty m_RenderingPath;
    private SerializedProperty m_MobileRenderingPath;
    private SerializedProperty m_ActiveColorSpace;
    private SerializedProperty m_MTRendering;
    private SerializedProperty m_MobileMTRendering;
    private SerializedProperty m_StripUnusedMeshComponents;
    private SerializedProperty m_VertexChannelCompressionMask;
    private SerializedProperty m_DisplayResolutionDialog;
    private SerializedProperty m_DefaultIsFullScreen;
    private SerializedProperty m_DefaultIsNativeResolution;
    private SerializedProperty m_UsePlayerLog;
    private SerializedProperty m_PreloadShaders;
    private SerializedProperty m_PreloadedAssets;
    private SerializedProperty m_BakeCollisionMeshes;
    private SerializedProperty m_ResizableWindow;
    private SerializedProperty m_UseMacAppStoreValidation;
    private SerializedProperty m_MacFullscreenMode;
    private SerializedProperty m_D3D9FullscreenMode;
    private SerializedProperty m_D3D11FullscreenMode;
    private SerializedProperty m_VisibleInBackground;
    private SerializedProperty m_AllowFullscreenSwitch;
    private SerializedProperty m_ForceSingleInstance;
    private SerializedProperty m_RunInBackground;
    private SerializedProperty m_CaptureSingleScreen;
    private SerializedProperty m_ResolutionDialogBanner;
    private SerializedProperty m_VirtualRealitySplashScreen;
    private SerializedProperty m_SupportedAspectRatios;
    private SerializedProperty m_SkinOnGPU;
    private SerializedProperty m_WebPlayerTemplate;
    private int selectedPlatform;
    private int scriptingDefinesControlID;
    private ISettingEditorExtension[] m_SettingsExtensions;
    private static Texture2D s_WarningIcon;

    private bool IsMobileTarget(BuildTargetGroup targetGroup)
    {
      if (targetGroup != BuildTargetGroup.iPhone && targetGroup != BuildTargetGroup.tvOS && (targetGroup != BuildTargetGroup.Android && targetGroup != BuildTargetGroup.BlackBerry) && targetGroup != BuildTargetGroup.Tizen)
        return targetGroup == BuildTargetGroup.SamsungTV;
      return true;
    }

    private static bool IsWP8Player(BuildTargetGroup target)
    {
      return target == BuildTargetGroup.WP8;
    }

    public SerializedProperty FindPropertyAssert(string name)
    {
      SerializedProperty property = this.serializedObject.FindProperty(name);
      if (property == null)
        Debug.LogError((object) ("Failed to find:" + name));
      return property;
    }

    private void OnEnable()
    {
      this.validPlatforms = BuildPlayerWindow.GetValidPlatforms().ToArray();
      this.m_IPhoneSdkVersion = this.FindPropertyAssert("iPhoneSdkVersion");
      this.m_IPhoneTargetOSVersion = this.FindPropertyAssert("iPhoneTargetOSVersion");
      this.m_IPhoneStrippingLevel = this.FindPropertyAssert("iPhoneStrippingLevel");
      this.m_IPhoneBuildNumber = this.FindPropertyAssert("iPhoneBuildNumber");
      this.m_StripEngineCode = this.FindPropertyAssert("stripEngineCode");
      this.m_tvOSSdkVersion = this.FindPropertyAssert("tvOSSdkVersion");
      this.m_tvOSTargetOSVersion = this.FindPropertyAssert("tvOSTargetOSVersion");
      this.m_IPhoneScriptCallOptimization = this.FindPropertyAssert("iPhoneScriptCallOptimization");
      this.m_AndroidProfiler = this.FindPropertyAssert("AndroidProfiler");
      this.m_CompanyName = this.FindPropertyAssert("companyName");
      this.m_ProductName = this.FindPropertyAssert("productName");
      this.m_DefaultCursor = this.FindPropertyAssert("defaultCursor");
      this.m_CursorHotspot = this.FindPropertyAssert("cursorHotspot");
      this.m_ShowUnitySplashScreen = this.FindPropertyAssert("m_ShowUnitySplashScreen");
      this.m_UIPrerenderedIcon = this.FindPropertyAssert("uIPrerenderedIcon");
      this.m_ResolutionDialogBanner = this.FindPropertyAssert("resolutionDialogBanner");
      this.m_VirtualRealitySplashScreen = this.FindPropertyAssert("m_VirtualRealitySplashScreen");
      this.m_UIRequiresFullScreen = this.FindPropertyAssert("uIRequiresFullScreen");
      this.m_UIStatusBarHidden = this.FindPropertyAssert("uIStatusBarHidden");
      this.m_UIStatusBarStyle = this.FindPropertyAssert("uIStatusBarStyle");
      this.m_RenderingPath = this.FindPropertyAssert("m_RenderingPath");
      this.m_MobileRenderingPath = this.FindPropertyAssert("m_MobileRenderingPath");
      this.m_ActiveColorSpace = this.FindPropertyAssert("m_ActiveColorSpace");
      this.m_MTRendering = this.FindPropertyAssert("m_MTRendering");
      this.m_MobileMTRendering = this.FindPropertyAssert("m_MobileMTRendering");
      this.m_StripUnusedMeshComponents = this.FindPropertyAssert("StripUnusedMeshComponents");
      this.m_VertexChannelCompressionMask = this.FindPropertyAssert("VertexChannelCompressionMask");
      this.m_ApplicationBundleIdentifier = this.serializedObject.FindProperty("bundleIdentifier");
      if (this.m_ApplicationBundleIdentifier == null)
        this.m_ApplicationBundleIdentifier = this.FindPropertyAssert("iPhoneBundleIdentifier");
      this.m_ApplicationBundleVersion = this.serializedObject.FindProperty("bundleVersion");
      if (this.m_ApplicationBundleVersion == null)
        this.m_ApplicationBundleVersion = this.FindPropertyAssert("iPhoneBundleVersion");
      this.m_useOnDemandResources = this.FindPropertyAssert("useOnDemandResources");
      this.m_AccelerometerFrequency = this.FindPropertyAssert("accelerometerFrequency");
      this.m_OverrideIPodMusic = this.FindPropertyAssert("Override IPod Music");
      this.m_PrepareIOSForRecording = this.FindPropertyAssert("Prepare IOS For Recording");
      this.m_UIRequiresPersistentWiFi = this.FindPropertyAssert("uIRequiresPersistentWiFi");
      this.m_IOSAppInBackgroundBehavior = this.FindPropertyAssert("iosAppInBackgroundBehavior");
      this.m_IOSAllowHTTPDownload = this.FindPropertyAssert("iosAllowHTTPDownload");
      this.m_SubmitAnalytics = this.FindPropertyAssert("submitAnalytics");
      this.m_ApiCompatibilityLevel = this.FindPropertyAssert("apiCompatibilityLevel");
      this.m_AotOptions = this.FindPropertyAssert("aotOptions");
      this.m_LocationUsageDescription = this.FindPropertyAssert("locationUsageDescription");
      this.m_EnableInternalProfiler = this.FindPropertyAssert("enableInternalProfiler");
      this.m_ActionOnDotNetUnhandledException = this.FindPropertyAssert("actionOnDotNetUnhandledException");
      this.m_LogObjCUncaughtExceptions = this.FindPropertyAssert("logObjCUncaughtExceptions");
      this.m_EnableCrashReportAPI = this.FindPropertyAssert("enableCrashReportAPI");
      this.m_DefaultScreenWidth = this.FindPropertyAssert("defaultScreenWidth");
      this.m_DefaultScreenHeight = this.FindPropertyAssert("defaultScreenHeight");
      this.m_DefaultScreenWidthWeb = this.FindPropertyAssert("defaultScreenWidthWeb");
      this.m_DefaultScreenHeightWeb = this.FindPropertyAssert("defaultScreenHeightWeb");
      this.m_RunInBackground = this.FindPropertyAssert("runInBackground");
      this.m_DefaultScreenOrientation = this.FindPropertyAssert("defaultScreenOrientation");
      this.m_AllowedAutoRotateToPortrait = this.FindPropertyAssert("allowedAutorotateToPortrait");
      this.m_AllowedAutoRotateToPortraitUpsideDown = this.FindPropertyAssert("allowedAutorotateToPortraitUpsideDown");
      this.m_AllowedAutoRotateToLandscapeRight = this.FindPropertyAssert("allowedAutorotateToLandscapeRight");
      this.m_AllowedAutoRotateToLandscapeLeft = this.FindPropertyAssert("allowedAutorotateToLandscapeLeft");
      this.m_UseOSAutoRotation = this.FindPropertyAssert("useOSAutorotation");
      this.m_Use32BitDisplayBuffer = this.FindPropertyAssert("use32BitDisplayBuffer");
      this.m_DisableDepthAndStencilBuffers = this.FindPropertyAssert("disableDepthAndStencilBuffers");
      this.m_iosShowActivityIndicatorOnLoading = this.FindPropertyAssert("iosShowActivityIndicatorOnLoading");
      this.m_androidShowActivityIndicatorOnLoading = this.FindPropertyAssert("androidShowActivityIndicatorOnLoading");
      this.m_DefaultIsFullScreen = this.FindPropertyAssert("defaultIsFullScreen");
      this.m_DefaultIsNativeResolution = this.FindPropertyAssert("defaultIsNativeResolution");
      this.m_CaptureSingleScreen = this.FindPropertyAssert("captureSingleScreen");
      this.m_DisplayResolutionDialog = this.FindPropertyAssert("displayResolutionDialog");
      this.m_SupportedAspectRatios = this.FindPropertyAssert("m_SupportedAspectRatios");
      this.m_WebPlayerTemplate = this.FindPropertyAssert("webPlayerTemplate");
      this.m_TargetDevice = this.FindPropertyAssert("targetDevice");
      this.m_UsePlayerLog = this.FindPropertyAssert("usePlayerLog");
      this.m_PreloadShaders = this.FindPropertyAssert("preloadShaders");
      this.m_PreloadedAssets = this.FindPropertyAssert("preloadedAssets");
      this.m_BakeCollisionMeshes = this.FindPropertyAssert("bakeCollisionMeshes");
      this.m_ResizableWindow = this.FindPropertyAssert("resizableWindow");
      this.m_UseMacAppStoreValidation = this.FindPropertyAssert("useMacAppStoreValidation");
      this.m_D3D9FullscreenMode = this.FindPropertyAssert("d3d9FullscreenMode");
      this.m_D3D11FullscreenMode = this.FindPropertyAssert("d3d11FullscreenMode");
      this.m_VisibleInBackground = this.FindPropertyAssert("visibleInBackground");
      this.m_AllowFullscreenSwitch = this.FindPropertyAssert("allowFullscreenSwitch");
      this.m_MacFullscreenMode = this.FindPropertyAssert("macFullscreenMode");
      this.m_SkinOnGPU = this.FindPropertyAssert("gpuSkinning");
      this.m_ForceSingleInstance = this.FindPropertyAssert("forceSingleInstance");
      this.m_XboxTitleId = this.FindPropertyAssert("XboxTitleId");
      this.m_XboxImageXexPath = this.FindPropertyAssert("XboxImageXexPath");
      this.m_XboxSpaPath = this.FindPropertyAssert("XboxSpaPath");
      this.m_XboxGenerateSpa = this.FindPropertyAssert("XboxGenerateSpa");
      this.m_XboxDeployKinectResources = this.FindPropertyAssert("XboxDeployKinectResources");
      this.m_XboxPIXTextureCapture = this.FindPropertyAssert("xboxPIXTextureCapture");
      this.m_XboxEnableAvatar = this.FindPropertyAssert("xboxEnableAvatar");
      this.m_XboxEnableKinect = this.FindPropertyAssert("xboxEnableKinect");
      this.m_XboxEnableKinectAutoTracking = this.FindPropertyAssert("xboxEnableKinectAutoTracking");
      this.m_XboxSplashScreen = this.FindPropertyAssert("XboxSplashScreen");
      this.m_XboxEnableSpeech = this.FindPropertyAssert("xboxEnableSpeech");
      this.m_XboxSpeechDB = this.FindPropertyAssert("xboxSpeechDB");
      this.m_XboxEnableFitness = this.FindPropertyAssert("xboxEnableFitness");
      this.m_XboxAdditionalTitleMemorySize = this.FindPropertyAssert("xboxAdditionalTitleMemorySize");
      this.m_XboxEnableHeadOrientation = this.FindPropertyAssert("xboxEnableHeadOrientation");
      this.m_XboxDeployHeadOrientation = this.FindPropertyAssert("xboxDeployKinectHeadOrientation");
      this.m_XboxDeployKinectHeadPosition = this.FindPropertyAssert("xboxDeployKinectHeadPosition");
      this.m_XboxEnableGuest = this.FindPropertyAssert("xboxEnableGuest");
      this.m_VideoMemoryForVertexBuffers = this.FindPropertyAssert("videoMemoryForVertexBuffers");
      this.m_ps3SplashScreen = this.FindPropertyAssert("ps3SplashScreen");
      this.m_SettingsExtensions = new ISettingEditorExtension[this.validPlatforms.Length];
      for (int index = 0; index < this.validPlatforms.Length; ++index)
      {
        string buildTargetGroup = ModuleManager.GetTargetStringFromBuildTargetGroup(this.validPlatforms[index].targetGroup);
        this.m_SettingsExtensions[index] = ModuleManager.GetEditorSettingsExtension(buildTargetGroup);
        if (this.m_SettingsExtensions[index] != null)
          this.m_SettingsExtensions[index].OnEnable(this);
      }
      for (int index = 0; index < this.m_SectionAnimators.Length; ++index)
        this.m_SectionAnimators[index] = new AnimBool(this.m_SelectedSection.value == index, new UnityAction(((Editor) this).Repaint));
      this.m_ShowDefaultIsNativeResolution.value = this.m_DefaultIsFullScreen.boolValue;
      this.m_ShowResolution.value = (!this.m_DefaultIsFullScreen.boolValue ? 0 : (this.m_DefaultIsNativeResolution.boolValue ? 1 : 0)) == 0;
      this.m_ShowDefaultIsNativeResolution.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowResolution.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    private void OnDisable()
    {
      this.m_WebPlayerTemplateManager.ClearTemplates();
    }

    public override bool UseDefaultMargins()
    {
      return false;
    }

    public override void OnInspectorGUI()
    {
      if (PlayerSettingsEditor.s_Styles == null)
        PlayerSettingsEditor.s_Styles = new PlayerSettingsEditor.Styles();
      this.serializedObject.Update();
      EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
      this.CommonSettings();
      EditorGUILayout.EndVertical();
      EditorGUILayout.Space();
      EditorGUI.BeginChangeCheck();
      int selectedPlatform = this.selectedPlatform;
      this.selectedPlatform = EditorGUILayout.BeginPlatformGrouping(this.validPlatforms, (GUIContent) null);
      if (EditorGUI.EndChangeCheck())
      {
        if (EditorGUI.s_DelayedTextEditor.IsEditingControl(this.scriptingDefinesControlID))
        {
          EditorGUI.EndEditingActiveTextField();
          GUIUtility.keyboardControl = 0;
          PlayerSettings.SetScriptingDefineSymbolsForGroup(this.validPlatforms[selectedPlatform].targetGroup, EditorGUI.s_DelayedTextEditor.text);
        }
        GUI.FocusControl(string.Empty);
      }
      GUILayout.Label("Settings for " + this.validPlatforms[this.selectedPlatform].title.text);
      EditorGUIUtility.labelWidth = Mathf.Max(150f, EditorGUIUtility.labelWidth - 8f);
      BuildPlayerWindow.BuildPlatform validPlatform = this.validPlatforms[this.selectedPlatform];
      BuildTargetGroup targetGroup = validPlatform.targetGroup;
      this.ResolutionSectionGUI(targetGroup, this.m_SettingsExtensions[this.selectedPlatform]);
      this.IconSectionGUI(targetGroup, this.m_SettingsExtensions[this.selectedPlatform]);
      this.SplashSectionGUI(validPlatform, targetGroup, this.m_SettingsExtensions[this.selectedPlatform]);
      this.DebugAndCrashReportingGUI(validPlatform, targetGroup, this.m_SettingsExtensions[this.selectedPlatform]);
      this.OtherSectionGUI(validPlatform, targetGroup, this.m_SettingsExtensions[this.selectedPlatform]);
      this.PublishSectionGUI(targetGroup, this.m_SettingsExtensions[this.selectedPlatform]);
      EditorGUILayout.EndPlatformGrouping();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void CommonSettings()
    {
      EditorGUILayout.PropertyField(this.m_CompanyName);
      EditorGUILayout.PropertyField(this.m_ProductName);
      EditorGUILayout.Space();
      GUI.changed = false;
      string empty = string.Empty;
      Texture2D[] icons = PlayerSettings.GetIconsForPlatform(empty);
      int[] widthsForPlatform = PlayerSettings.GetIconWidthsForPlatform(empty);
      if (icons.Length != widthsForPlatform.Length)
      {
        icons = new Texture2D[widthsForPlatform.Length];
        PlayerSettings.SetIconsForPlatform(empty, icons);
      }
      icons[0] = (Texture2D) EditorGUILayout.ObjectField(PlayerSettingsEditor.s_Styles.defaultIcon, (UnityEngine.Object) icons[0], typeof (Texture2D), false, new GUILayoutOption[0]);
      if (GUI.changed)
        PlayerSettings.SetIconsForPlatform(empty, icons);
      GUILayout.Space(3f);
      this.m_DefaultCursor.objectReferenceValue = EditorGUILayout.ObjectField(PlayerSettingsEditor.s_Styles.defaultCursor, this.m_DefaultCursor.objectReferenceValue, typeof (Texture2D), false, new GUILayoutOption[0]);
      EditorGUI.PropertyField(EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), 0, PlayerSettingsEditor.s_Styles.cursorHotspot), this.m_CursorHotspot, GUIContent.none);
    }

    private bool BeginSettingsBox(int nr, GUIContent header)
    {
      bool enabled = GUI.enabled;
      GUI.enabled = true;
      EditorGUILayout.BeginVertical(PlayerSettingsEditor.s_Styles.categoryBox, new GUILayoutOption[0]);
      Rect rect = GUILayoutUtility.GetRect(20f, 18f);
      rect.x += 3f;
      rect.width += 6f;
      bool flag = GUI.Toggle(rect, this.m_SelectedSection.value == nr, header, EditorStyles.inspectorTitlebarText);
      if (GUI.changed)
      {
        this.m_SelectedSection.value = !flag ? -1 : nr;
        GUIUtility.keyboardControl = 0;
      }
      this.m_SectionAnimators[nr].target = flag;
      GUI.enabled = enabled;
      return EditorGUILayout.BeginFadeGroup(this.m_SectionAnimators[nr].faded);
    }

    private void EndSettingsBox()
    {
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.EndVertical();
    }

    private void ShowNoSettings()
    {
      GUILayout.Label(EditorGUIUtility.TextContent("Not applicable for this platform."), EditorStyles.miniLabel, new GUILayoutOption[0]);
    }

    private void ShowSharedNote()
    {
      GUILayout.Label(EditorGUIUtility.TextContent("* Shared setting between multiple platforms."), EditorStyles.miniLabel, new GUILayoutOption[0]);
    }

    private void IconSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      GUI.changed = false;
      if (this.BeginSettingsBox(1, EditorGUIUtility.TextContent("Icon")))
      {
        bool flag1 = true;
        if (settingsExtension != null)
          flag1 = settingsExtension.UsesStandardIcons();
        if (flag1)
        {
          bool flag2 = this.selectedPlatform < 0;
          BuildPlayerWindow.BuildPlatform buildPlatform = (BuildPlayerWindow.BuildPlatform) null;
          targetGroup = BuildTargetGroup.Standalone;
          string platform = string.Empty;
          if (!flag2)
          {
            buildPlatform = this.validPlatforms[this.selectedPlatform];
            targetGroup = buildPlatform.targetGroup;
            platform = buildPlatform.name;
          }
          bool enabled = GUI.enabled;
          if (targetGroup == BuildTargetGroup.XBOX360 || targetGroup == BuildTargetGroup.WebPlayer || (PlayerSettingsEditor.IsWP8Player(targetGroup) || targetGroup == BuildTargetGroup.SamsungTV) || targetGroup == BuildTargetGroup.WebGL)
          {
            this.ShowNoSettings();
            EditorGUILayout.Space();
          }
          else if (targetGroup != BuildTargetGroup.Metro)
          {
            Texture2D[] icons = PlayerSettings.GetIconsForPlatform(platform);
            int[] widthsForPlatform = PlayerSettings.GetIconWidthsForPlatform(platform);
            int[] heightsForPlatform = PlayerSettings.GetIconHeightsForPlatform(platform);
            bool flag3 = true;
            if (flag2)
            {
              if (icons.Length != widthsForPlatform.Length)
              {
                icons = new Texture2D[widthsForPlatform.Length];
                PlayerSettings.SetIconsForPlatform(platform, icons);
              }
            }
            else
            {
              GUI.changed = false;
              flag3 = GUILayout.Toggle(icons.Length == widthsForPlatform.Length, "Override for " + buildPlatform.name);
              GUI.enabled = enabled && flag3;
              if (GUI.changed || !flag3 && icons.Length > 0)
              {
                icons = !flag3 ? new Texture2D[0] : new Texture2D[widthsForPlatform.Length];
                PlayerSettings.SetIconsForPlatform(platform, icons);
              }
            }
            GUI.changed = false;
            for (int index = 0; index < widthsForPlatform.Length; ++index)
            {
              int num1 = Mathf.Min(96, widthsForPlatform[index]);
              int b = (int) ((double) heightsForPlatform[index] * (double) num1 / (double) widthsForPlatform[index]);
              Rect rect = GUILayoutUtility.GetRect(64f, (float) (Mathf.Max(64, b) + 6));
              float num2 = Mathf.Min(rect.width, (float) ((double) EditorGUIUtility.labelWidth + 4.0 + 64.0 + 6.0 + 96.0));
              string text = widthsForPlatform[index].ToString() + "x" + (object) heightsForPlatform[index];
              GUI.Label(new Rect(rect.x, rect.y, (float) ((double) num2 - 96.0 - 64.0 - 12.0), 20f), text);
              if (flag3)
              {
                int num3 = 64;
                int num4 = (int) ((double) heightsForPlatform[index] / (double) widthsForPlatform[index] * 64.0);
                icons[index] = (Texture2D) EditorGUI.ObjectField(new Rect((float) ((double) rect.x + (double) num2 - 96.0 - 64.0 - 6.0), rect.y, (float) num3, (float) num4), (UnityEngine.Object) icons[index], typeof (Texture2D), false);
              }
              Rect position = new Rect((float) ((double) rect.x + (double) num2 - 96.0), rect.y, (float) num1, (float) b);
              Texture2D forPlatformAtSize = PlayerSettings.GetIconForPlatformAtSize(platform, widthsForPlatform[index], heightsForPlatform[index]);
              if ((UnityEngine.Object) forPlatformAtSize != (UnityEngine.Object) null)
                GUI.DrawTexture(position, (Texture) forPlatformAtSize);
              else
                GUI.Box(position, string.Empty);
            }
            if (GUI.changed)
              PlayerSettings.SetIconsForPlatform(platform, icons);
            GUI.enabled = enabled;
            if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
            {
              EditorGUILayout.PropertyField(this.m_UIPrerenderedIcon, EditorGUIUtility.TextContent("Prerendered Icon"), new GUILayoutOption[0]);
              EditorGUILayout.Space();
            }
          }
        }
        if (settingsExtension != null)
          settingsExtension.IconSectionGUI();
      }
      this.EndSettingsBox();
    }

    private static bool TargetSupportsOptionalBuiltinSplashScreen(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      if (settingsExtension != null)
        return settingsExtension.CanShowUnitySplashScreen();
      if (targetGroup != BuildTargetGroup.Standalone)
        return PlayerSettingsEditor.IsWP8Player(targetGroup);
      return true;
    }

    private static bool TargetSupportsVirtualReality(BuildTargetGroup targetGroup)
    {
      if (targetGroup != BuildTargetGroup.Standalone && targetGroup != BuildTargetGroup.Android)
        return targetGroup == BuildTargetGroup.PS4;
      return true;
    }

    private void SplashSectionGUI(BuildPlayerWindow.BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      GUI.changed = false;
      if (this.BeginSettingsBox(2, EditorGUIUtility.TextContent("Splash Image")))
      {
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          this.m_ResolutionDialogBanner.objectReferenceValue = EditorGUILayout.ObjectField(EditorGUIUtility.TextContent("Config Dialog Banner"), this.m_ResolutionDialogBanner.objectReferenceValue, typeof (Texture2D), false, new GUILayoutOption[0]);
          EditorGUILayout.Space();
        }
        if (targetGroup == BuildTargetGroup.WebPlayer)
        {
          this.ShowNoSettings();
          EditorGUILayout.Space();
        }
        if (targetGroup == BuildTargetGroup.XBOX360)
        {
          this.m_XboxSplashScreen.objectReferenceValue = EditorGUILayout.ObjectField(EditorGUIUtility.TextContent("Xbox 360 splash screen"), this.m_XboxSplashScreen.objectReferenceValue, typeof (Texture2D), false, new GUILayoutOption[0]);
          EditorGUILayout.Space();
        }
        bool flag = targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android;
        if (targetGroup == BuildTargetGroup.PS3)
        {
          this.BuiltinSplashScreenField();
          flag = true;
          if (this.m_ShowUnitySplashScreen.boolValue && this.m_ps3SplashScreen.objectReferenceValue != (UnityEngine.Object) null)
            this.m_ps3SplashScreen.objectReferenceValue = (UnityEngine.Object) null;
          EditorGUI.BeginDisabledGroup(this.m_ShowUnitySplashScreen.boolValue);
          ++EditorGUI.indentLevel;
          this.m_ps3SplashScreen.objectReferenceValue = EditorGUILayout.ObjectField(EditorGUIUtility.TextContent("Splash Screen Image for PS3"), this.m_ps3SplashScreen.objectReferenceValue, typeof (Texture2D), false, new GUILayoutOption[0]);
          --EditorGUI.indentLevel;
          EditorGUI.EndDisabledGroup();
          EditorGUILayout.Space();
        }
        EditorGUI.BeginDisabledGroup(!InternalEditorUtility.HasAdvancedLicenseOnBuildTarget(platform.DefaultTarget) && targetGroup != BuildTargetGroup.iPhone && targetGroup != BuildTargetGroup.tvOS && targetGroup != BuildTargetGroup.XboxOne);
        if (PlayerSettingsEditor.TargetSupportsVirtualReality(targetGroup))
          this.m_VirtualRealitySplashScreen.objectReferenceValue = EditorGUILayout.ObjectField(EditorGUIUtility.TextContent("Virtual Reality Splash Image"), this.m_VirtualRealitySplashScreen.objectReferenceValue, typeof (Texture2D), false, new GUILayoutOption[0]);
        if (PlayerSettingsEditor.TargetSupportsOptionalBuiltinSplashScreen(targetGroup, settingsExtension))
        {
          this.BuiltinSplashScreenField();
          flag = true;
        }
        if (settingsExtension != null)
          settingsExtension.SplashSectionGUI();
        EditorGUI.EndDisabledGroup();
        if (flag)
          this.ShowSharedNote();
      }
      this.EndSettingsBox();
    }

    private void BuiltinSplashScreenField()
    {
      EditorGUI.BeginDisabledGroup(!Application.HasProLicense());
      EditorGUILayout.PropertyField(this.m_ShowUnitySplashScreen, EditorGUIUtility.TextContent("Show Unity Splash Screen*"), new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
    }

    public void ResolutionSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      GUI.changed = false;
      if (this.BeginSettingsBox(0, EditorGUIUtility.TextContent("Resolution and Presentation")))
      {
        if (settingsExtension != null)
        {
          float h = 16f;
          float midWidth = (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0);
          float maxWidth = (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0);
          settingsExtension.ResolutionSectionGUI(h, midWidth, maxWidth);
        }
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          GUILayout.Label(EditorGUIUtility.TextContent("Resolution"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_DefaultIsFullScreen);
          this.m_ShowDefaultIsNativeResolution.target = this.m_DefaultIsFullScreen.boolValue;
          if (EditorGUILayout.BeginFadeGroup(this.m_ShowDefaultIsNativeResolution.faded))
            EditorGUILayout.PropertyField(this.m_DefaultIsNativeResolution);
          if ((double) this.m_ShowDefaultIsNativeResolution.faded != 0.0 && (double) this.m_ShowDefaultIsNativeResolution.faded != 1.0)
            EditorGUILayout.EndFadeGroup();
          this.m_ShowResolution.target = (!this.m_DefaultIsFullScreen.boolValue ? 0 : (this.m_DefaultIsNativeResolution.boolValue ? 1 : 0)) == 0;
          if (EditorGUILayout.BeginFadeGroup(this.m_ShowResolution.faded))
          {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.m_DefaultScreenWidth, EditorGUIUtility.TextContent("Default Screen Width"), new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck() && this.m_DefaultScreenWidth.intValue < 1)
              this.m_DefaultScreenWidth.intValue = 1;
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.m_DefaultScreenHeight, EditorGUIUtility.TextContent("Default Screen Height"), new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck() && this.m_DefaultScreenHeight.intValue < 1)
              this.m_DefaultScreenHeight.intValue = 1;
          }
          if ((double) this.m_ShowResolution.faded != 0.0 && (double) this.m_ShowResolution.faded != 1.0)
            EditorGUILayout.EndFadeGroup();
        }
        if (targetGroup == BuildTargetGroup.WebPlayer)
        {
          GUILayout.Label(EditorGUIUtility.TextContent("Resolution"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_DefaultScreenWidthWeb, EditorGUIUtility.TextContent("Default Screen Width*"), new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck() && this.m_DefaultScreenWidthWeb.intValue < 1)
            this.m_DefaultScreenWidthWeb.intValue = 1;
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_DefaultScreenHeightWeb, EditorGUIUtility.TextContent("Default Screen Height*"), new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck() && this.m_DefaultScreenHeightWeb.intValue < 1)
            this.m_DefaultScreenHeightWeb.intValue = 1;
        }
        if (targetGroup == BuildTargetGroup.XBOX360)
        {
          this.ShowNoSettings();
          EditorGUILayout.Space();
        }
        if (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.WebPlayer || targetGroup == BuildTargetGroup.BlackBerry)
          EditorGUILayout.PropertyField(this.m_RunInBackground, EditorGUIUtility.TextContent("Run In Background*"), new GUILayoutOption[0]);
        if (settingsExtension != null && settingsExtension.SupportsOrientation() || PlayerSettingsEditor.IsWP8Player(targetGroup))
        {
          GUILayout.Label(EditorGUIUtility.TextContent("Orientation"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUI.BeginDisabledGroup(PlayerSettings.virtualRealitySupported);
          EditorGUILayout.PropertyField(this.m_DefaultScreenOrientation, EditorGUIUtility.TextContent("Default Orientation*"), new GUILayoutOption[0]);
          if (PlayerSettings.virtualRealitySupported)
            EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("This setting is overridden by Virtual Reality Support.").text, MessageType.Info);
          if (this.m_DefaultScreenOrientation.enumValueIndex == 4)
          {
            if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.Tizen)
              EditorGUILayout.PropertyField(this.m_UseOSAutoRotation, EditorGUIUtility.TextContent("Use Animated Autorotation|If set OS native animated autorotation method will be used. Otherwise orientation will be changed immediately."), new GUILayoutOption[0]);
            ++EditorGUI.indentLevel;
            GUILayout.Label(EditorGUIUtility.TextContent("Allowed Orientations for Auto Rotation"), EditorStyles.boldLabel, new GUILayoutOption[0]);
            if (!this.m_AllowedAutoRotateToPortrait.boolValue && !this.m_AllowedAutoRotateToPortraitUpsideDown.boolValue && !this.m_AllowedAutoRotateToLandscapeRight.boolValue && !this.m_AllowedAutoRotateToLandscapeLeft.boolValue)
            {
              this.m_AllowedAutoRotateToPortrait.boolValue = true;
              Debug.LogError((object) "All orientations are disabled. Allowing portrait");
            }
            EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToPortrait, EditorGUIUtility.TextContent("Portrait"), new GUILayoutOption[0]);
            if (!PlayerSettingsEditor.IsWP8Player(targetGroup) && (targetGroup != BuildTargetGroup.Metro || EditorUserBuildSettings.wsaSDK != WSASDK.PhoneSDK81))
              EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToPortraitUpsideDown, EditorGUIUtility.TextContent("Portrait Upside Down"), new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToLandscapeRight, EditorGUIUtility.TextContent("Landscape Right"), new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToLandscapeLeft, EditorGUIUtility.TextContent("Landscape Left"), new GUILayoutOption[0]);
            --EditorGUI.indentLevel;
          }
          EditorGUI.EndDisabledGroup();
        }
        if (targetGroup == BuildTargetGroup.iPhone)
        {
          GUILayout.Label(EditorGUIUtility.TextContent("Multitasking Support"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_UIRequiresFullScreen, EditorGUIUtility.TextContent("Requires Fullscreen"), new GUILayoutOption[0]);
          EditorGUILayout.Space();
          GUILayout.Label(EditorGUIUtility.TextContent("Status Bar"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_UIStatusBarHidden, EditorGUIUtility.TextContent("Status Bar Hidden"), new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_UIStatusBarStyle, EditorGUIUtility.TextContent("Status Bar Style"), new GUILayoutOption[0]);
          EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        if (targetGroup == BuildTargetGroup.Standalone)
        {
          GUILayout.Label(EditorGUIUtility.TextContent("Standalone Player Options"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_CaptureSingleScreen);
          EditorGUILayout.PropertyField(this.m_DisplayResolutionDialog);
          EditorGUILayout.PropertyField(this.m_UsePlayerLog);
          EditorGUILayout.PropertyField(this.m_ResizableWindow);
          EditorGUILayout.PropertyField(this.m_UseMacAppStoreValidation, EditorGUIUtility.TempContent("Mac App Store Validation"), new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_MacFullscreenMode);
          EditorGUILayout.PropertyField(this.m_D3D9FullscreenMode, EditorGUIUtility.TempContent("D3D9 Fullscreen Mode"), new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_D3D11FullscreenMode, EditorGUIUtility.TempContent("D3D11 Fullscreen Mode"), new GUILayoutOption[0]);
          GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows);
          bool disabled = graphicsApIs.Length >= 1 && graphicsApIs[0] == GraphicsDeviceType.Direct3D9 && this.m_D3D9FullscreenMode.intValue == 0;
          if (disabled)
            this.m_VisibleInBackground.boolValue = false;
          EditorGUI.BeginDisabledGroup(disabled);
          EditorGUILayout.PropertyField(this.m_VisibleInBackground, EditorGUIUtility.TempContent("Visible In Background"), new GUILayoutOption[0]);
          EditorGUI.EndDisabledGroup();
          EditorGUILayout.PropertyField(this.m_AllowFullscreenSwitch, EditorGUIUtility.TempContent("Allow Fullscreen Switch"), new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_ForceSingleInstance);
          EditorGUILayout.PropertyField(this.m_SupportedAspectRatios, true, new GUILayoutOption[0]);
          EditorGUILayout.Space();
        }
        if (targetGroup == BuildTargetGroup.WebPlayer)
        {
          GUILayout.Label(EditorGUIUtility.TextContent("WebPlayer Template"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          this.m_WebPlayerTemplateManager.SelectionUI(this.m_WebPlayerTemplate);
          EditorGUILayout.Space();
        }
        if (this.IsMobileTarget(targetGroup))
        {
          if (targetGroup != BuildTargetGroup.Tizen && targetGroup != BuildTargetGroup.iPhone && targetGroup != BuildTargetGroup.tvOS)
            EditorGUILayout.PropertyField(this.m_Use32BitDisplayBuffer, EditorGUIUtility.TextContent("Use 32-bit Display Buffer*|If set Display Buffer will be created to hold 32-bit color values. Use it only if you see banding, as it has performance implications."), new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_DisableDepthAndStencilBuffers, EditorGUIUtility.TextContent("Disable Depth and Stencil*"), new GUILayoutOption[0]);
        }
        if (targetGroup == BuildTargetGroup.iPhone)
          EditorGUILayout.PropertyField(this.m_iosShowActivityIndicatorOnLoading, EditorGUIUtility.TextContent("Show Loading Indicator"), new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.Android)
          EditorGUILayout.PropertyField(this.m_androidShowActivityIndicatorOnLoading, EditorGUIUtility.TextContent("Show Loading Indicator"), new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.Android)
          EditorGUILayout.Space();
        this.ShowSharedNote();
      }
      this.EndSettingsBox();
    }

    private void ShowDisabledFakeEnumPopup(PlayerSettingsEditor.FakeEnum enumValue)
    {
      GUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel(this.m_ApiCompatibilityLevel.displayName);
      EditorGUI.BeginDisabledGroup(true);
      EditorGUILayout.EnumPopup((Enum) enumValue);
      EditorGUI.EndDisabledGroup();
      GUILayout.EndHorizontal();
    }

    private void SyncPlatformAPIsList(BuildTarget target)
    {
      if (!this.m_GraphicsDeviceLists.ContainsKey(target))
        return;
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(target);
      List<GraphicsDeviceType> graphicsDeviceTypeList = graphicsApIs == null ? new List<GraphicsDeviceType>() : ((IEnumerable<GraphicsDeviceType>) graphicsApIs).ToList<GraphicsDeviceType>();
      this.m_GraphicsDeviceLists[target].list = (IList) graphicsDeviceTypeList;
    }

    private void AddGraphicsDeviceMenuSelected(object userData, string[] options, int selected)
    {
      BuildTarget buildTarget = (BuildTarget) userData;
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(buildTarget);
      if (graphicsApIs == null)
        return;
      GraphicsDeviceType graphicsDeviceType = (GraphicsDeviceType) Enum.Parse(typeof (GraphicsDeviceType), options[selected], true);
      List<GraphicsDeviceType> list = ((IEnumerable<GraphicsDeviceType>) graphicsApIs).ToList<GraphicsDeviceType>();
      list.Add(graphicsDeviceType);
      GraphicsDeviceType[] array = list.ToArray();
      PlayerSettings.SetGraphicsAPIs(buildTarget, array);
      this.SyncPlatformAPIsList(buildTarget);
    }

    private void AddGraphicsDeviceElement(BuildTarget target, Rect rect, ReorderableList list)
    {
      GraphicsDeviceType[] supportedGraphicsApIs = PlayerSettings.GetSupportedGraphicsAPIs(target);
      if (supportedGraphicsApIs == null || supportedGraphicsApIs.Length == 0)
        return;
      string[] options = new string[supportedGraphicsApIs.Length];
      bool[] enabled = new bool[supportedGraphicsApIs.Length];
      for (int index = 0; index < supportedGraphicsApIs.Length; ++index)
      {
        options[index] = supportedGraphicsApIs[index].ToString();
        enabled[index] = !list.list.Contains((object) supportedGraphicsApIs[index]);
      }
      EditorUtility.DisplayCustomMenu(rect, options, enabled, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.AddGraphicsDeviceMenuSelected), (object) target);
    }

    private bool CanRemoveGraphicsDeviceElement(ReorderableList list)
    {
      return list.list.Count >= 2;
    }

    private void RemoveGraphicsDeviceElement(BuildTarget target, ReorderableList list)
    {
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(target);
      if (graphicsApIs == null)
        return;
      if (graphicsApIs.Length < 2)
      {
        EditorApplication.Beep();
      }
      else
      {
        List<GraphicsDeviceType> list1 = ((IEnumerable<GraphicsDeviceType>) graphicsApIs).ToList<GraphicsDeviceType>();
        list1.RemoveAt(list.index);
        GraphicsDeviceType[] array = list1.ToArray();
        this.ApplyChangedGraphicsAPIList(target, array, list.index == 0);
      }
    }

    private void ReorderGraphicsDeviceElement(BuildTarget target, ReorderableList list)
    {
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(target);
      GraphicsDeviceType[] array = ((List<GraphicsDeviceType>) list.list).ToArray();
      bool firstEntryChanged = graphicsApIs[0] != array[0];
      this.ApplyChangedGraphicsAPIList(target, array, firstEntryChanged);
    }

    private void ApplyChangedGraphicsAPIList(BuildTarget target, GraphicsDeviceType[] apis, bool firstEntryChanged)
    {
      bool flag1 = true;
      bool flag2 = false;
      if (firstEntryChanged && PlayerSettingsEditor.WillEditorUseFirstGraphicsAPI(target))
      {
        flag1 = false;
        if (EditorUtility.DisplayDialog("Changing editor graphics device", "Changing active graphics API requires reloading all graphics objects, it might take a while", "Apply", "Cancel") && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
          flag1 = true;
          flag2 = true;
        }
      }
      if (flag1)
      {
        PlayerSettings.SetGraphicsAPIs(target, apis);
        this.SyncPlatformAPIsList(target);
      }
      else
        this.m_GraphicsDeviceLists.Remove(target);
      if (!flag2)
        return;
      ShaderUtil.RecreateGfxDevice();
      GUIUtility.ExitGUI();
    }

    private void DrawGraphicsDeviceElement(BuildTarget target, Rect rect, int index, bool selected, bool focused)
    {
      string text = this.m_GraphicsDeviceLists[target].list[index].ToString();
      if (text == "Direct3D12")
        text = "Direct3D12 (Experimental)";
      if (target == BuildTarget.StandaloneOSXUniversal && text == "Metal")
        text = "Metal (Experimental)";
      if (target == BuildTarget.WebGL)
      {
        if (text == "OpenGLES3")
          text = "WebGL 2.0 (Experimental)";
        else if (text == "OpenGLES2")
          text = "WebGL 1.0";
      }
      GUI.Label(rect, text, EditorStyles.label);
    }

    private static bool WillEditorUseFirstGraphicsAPI(BuildTarget targetPlatform)
    {
      if (Application.platform == RuntimePlatform.WindowsEditor && targetPlatform == BuildTarget.StandaloneWindows)
        return true;
      if (Application.platform == RuntimePlatform.OSXEditor)
        return targetPlatform == BuildTarget.StandaloneOSXUniversal;
      return false;
    }

    private void OpenGLES31OptionsGUI(BuildTargetGroup targetGroup, BuildTarget targetPlatform)
    {
      if (targetGroup != BuildTargetGroup.Android)
        return;
      GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(targetPlatform);
      if (!((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES3) || ((IEnumerable<GraphicsDeviceType>) graphicsApIs).Contains<GraphicsDeviceType>(GraphicsDeviceType.OpenGLES2))
        return;
      bool flag1 = false;
      bool flag2 = false;
      PlayerSettings.GetPropertyOptionalBool("RequireES31", ref flag1, targetGroup);
      PlayerSettings.GetPropertyOptionalBool("RequireES31AEP", ref flag2, targetGroup);
      EditorGUI.BeginChangeCheck();
      flag1 = EditorGUILayout.Toggle("Require ES3.1", flag1, new GUILayoutOption[0]);
      bool flag3 = EditorGUILayout.Toggle("Require ES3.1+AEP", flag2, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      PlayerSettings.InitializePropertyBool("RequireES31", false, targetGroup);
      PlayerSettings.InitializePropertyBool("RequireES31AEP", false, targetGroup);
      PlayerSettings.SetPropertyBool("RequireES31", flag1, targetGroup);
      PlayerSettings.SetPropertyBool("RequireES31AEP", flag3, targetGroup);
    }

    private void GraphicsAPIsGUIOnePlatform(BuildTargetGroup targetGroup, BuildTarget targetPlatform, string platformTitle)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerSettingsEditor.\u003CGraphicsAPIsGUIOnePlatform\u003Ec__AnonStoreyA0 platformCAnonStoreyA0 = new PlayerSettingsEditor.\u003CGraphicsAPIsGUIOnePlatform\u003Ec__AnonStoreyA0();
      // ISSUE: reference to a compiler-generated field
      platformCAnonStoreyA0.targetPlatform = targetPlatform;
      // ISSUE: reference to a compiler-generated field
      platformCAnonStoreyA0.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      GraphicsDeviceType[] supportedGraphicsApIs = PlayerSettings.GetSupportedGraphicsAPIs(platformCAnonStoreyA0.targetPlatform);
      if (supportedGraphicsApIs == null || supportedGraphicsApIs.Length < 2)
        return;
      EditorGUI.BeginChangeCheck();
      // ISSUE: reference to a compiler-generated field
      bool defaultGraphicsApIs = PlayerSettings.GetUseDefaultGraphicsAPIs(platformCAnonStoreyA0.targetPlatform);
      bool automatic = EditorGUILayout.Toggle("Auto Graphics API" + (platformTitle ?? string.Empty), defaultGraphicsApIs, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(this.target, "Changed Graphics API Settings");
        // ISSUE: reference to a compiler-generated field
        PlayerSettings.SetUseDefaultGraphicsAPIs(platformCAnonStoreyA0.targetPlatform, automatic);
      }
      if (automatic)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PlayerSettingsEditor.\u003CGraphicsAPIsGUIOnePlatform\u003Ec__AnonStoreyA1 platformCAnonStoreyA1 = new PlayerSettingsEditor.\u003CGraphicsAPIsGUIOnePlatform\u003Ec__AnonStoreyA1();
      // ISSUE: reference to a compiler-generated field
      platformCAnonStoreyA1.\u003C\u003Ef__ref\u0024160 = platformCAnonStoreyA0;
      // ISSUE: reference to a compiler-generated field
      platformCAnonStoreyA1.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      if (PlayerSettingsEditor.WillEditorUseFirstGraphicsAPI(platformCAnonStoreyA0.targetPlatform))
        EditorGUILayout.HelpBox("Reordering the list will switch editor to the first available platform", MessageType.Info, true);
      // ISSUE: reference to a compiler-generated field
      platformCAnonStoreyA1.displayTitle = "Graphics APIs";
      if (platformTitle != null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        platformCAnonStoreyA1.displayTitle = platformCAnonStoreyA1.displayTitle + platformTitle;
      }
      // ISSUE: reference to a compiler-generated field
      if (!this.m_GraphicsDeviceLists.ContainsKey(platformCAnonStoreyA0.targetPlatform))
      {
        // ISSUE: reference to a compiler-generated field
        GraphicsDeviceType[] graphicsApIs = PlayerSettings.GetGraphicsAPIs(platformCAnonStoreyA0.targetPlatform);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.m_GraphicsDeviceLists.Add(platformCAnonStoreyA0.targetPlatform, new ReorderableList(graphicsApIs == null ? (IList) new List<GraphicsDeviceType>() : (IList) ((IEnumerable<GraphicsDeviceType>) graphicsApIs).ToList<GraphicsDeviceType>(), typeof (GraphicsDeviceType), true, true, true, true)
        {
          onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(platformCAnonStoreyA1.\u003C\u003Em__1CD),
          onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.CanRemoveGraphicsDeviceElement),
          onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(platformCAnonStoreyA1.\u003C\u003Em__1CE),
          onReorderCallback = new ReorderableList.ReorderCallbackDelegate(platformCAnonStoreyA1.\u003C\u003Em__1CF),
          drawElementCallback = new ReorderableList.ElementCallbackDelegate(platformCAnonStoreyA1.\u003C\u003Em__1D0),
          drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(platformCAnonStoreyA1.\u003C\u003Em__1D1),
          elementHeight = 16f
        });
      }
      // ISSUE: reference to a compiler-generated field
      this.m_GraphicsDeviceLists[platformCAnonStoreyA0.targetPlatform].DoLayoutList();
      // ISSUE: reference to a compiler-generated field
      this.OpenGLES31OptionsGUI(targetGroup, platformCAnonStoreyA0.targetPlatform);
    }

    private void GraphicsAPIsGUI(BuildTargetGroup targetGroup, BuildTarget target)
    {
      if (targetGroup == BuildTargetGroup.Standalone)
      {
        this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneWindows, " for Windows");
        this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneOSXUniversal, " for Mac");
        this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneLinuxUniversal, " for Linux");
      }
      else
        this.GraphicsAPIsGUIOnePlatform(targetGroup, target, (string) null);
    }

    public void DebugAndCrashReportingGUI(BuildPlayerWindow.BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      if (targetGroup != BuildTargetGroup.iPhone && targetGroup != BuildTargetGroup.tvOS)
        return;
      GUI.changed = false;
      if (this.BeginSettingsBox(3, EditorGUIUtility.TextContent("Debugging and crash reporting")))
      {
        GUILayout.Label(EditorGUIUtility.TextContent("Debugging"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_EnableInternalProfiler, EditorGUIUtility.TextContent("Enable Internal Profiler"), new GUILayoutOption[0]);
        EditorGUILayout.Space();
        GUILayout.Label(EditorGUIUtility.TextContent("Crash Reporting"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_ActionOnDotNetUnhandledException, EditorGUIUtility.TextContent("On .Net UnhandledException"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_LogObjCUncaughtExceptions, EditorGUIUtility.TextContent("Log Obj-C Uncaught Exceptions"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_EnableCrashReportAPI, EditorGUIUtility.TextContent("Enable CrashReport API"), new GUILayoutOption[0]);
        EditorGUILayout.Space();
      }
      this.EndSettingsBox();
    }

    public static void BuildDisabledEnumPopup(GUIContent selected, GUIContent uiString)
    {
      EditorGUI.BeginDisabledGroup(true);
      EditorGUI.Popup(EditorGUILayout.GetControlRect(true, new GUILayoutOption[0]), uiString, 0, new GUIContent[1]{ selected });
      EditorGUI.EndDisabledGroup();
    }

    public static void BuildEnumPopup<T>(SerializedProperty prop, GUIContent uiString, T[] options, GUIContent[] optionNames)
    {
      int intValue = prop.intValue;
      int num = PlayerSettingsEditor.BuildEnumPopup<T>(uiString, intValue, options, optionNames);
      if (num == intValue)
        return;
      prop.intValue = num;
      prop.serializedObject.ApplyModifiedProperties();
    }

    public static int BuildEnumPopup<T>(GUIContent uiString, int selected, T[] options, GUIContent[] optionNames)
    {
      T obj = (T) (ValueType) selected;
      int selectedIndex = 0;
      for (int index = 1; index < options.Length; ++index)
      {
        if (obj.Equals((object) options[index]))
        {
          selectedIndex = index;
          break;
        }
      }
      int index1 = EditorGUILayout.Popup(uiString, selectedIndex, optionNames, new GUILayoutOption[0]);
      return (int) (object) options[index1];
    }

    public static int BuildEnumPopup<T>(GUIContent uiString, BuildTargetGroup targetGroup, string propertyName, T[] options, GUIContent[] optionNames)
    {
      int selected = 0;
      if (!PlayerSettings.GetPropertyOptionalInt(propertyName, ref selected, targetGroup))
        selected = (int) (object) default (T);
      return PlayerSettingsEditor.BuildEnumPopup<T>(uiString, selected, options, optionNames);
    }

    public void OtherSectionGUI(BuildPlayerWindow.BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      GUI.changed = false;
      if (this.BeginSettingsBox(4, EditorGUIUtility.TextContent("Other Settings")))
      {
        GUILayout.Label(EditorGUIUtility.TextContent("Rendering"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.WebPlayer || (this.IsMobileTarget(targetGroup) || targetGroup == BuildTargetGroup.WebGL) || (targetGroup == BuildTargetGroup.PS3 || targetGroup == BuildTargetGroup.PSP2 || (targetGroup == BuildTargetGroup.PSM || targetGroup == BuildTargetGroup.PS4)) || (targetGroup == BuildTargetGroup.Metro || targetGroup == BuildTargetGroup.WiiU))
          EditorGUILayout.IntPopup(!this.IsMobileTarget(targetGroup) ? this.m_RenderingPath : this.m_MobileRenderingPath, PlayerSettingsEditor.kRenderPaths, PlayerSettingsEditor.kRenderPathValues, EditorGUIUtility.TextContent("Rendering Path*"), new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.WebPlayer || (targetGroup == BuildTargetGroup.PS3 || targetGroup == BuildTargetGroup.PS4) || (targetGroup == BuildTargetGroup.Metro || targetGroup == BuildTargetGroup.WiiU || targetGroup == BuildTargetGroup.XBOX360))
        {
          EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_ActiveColorSpace, EditorGUIUtility.TextContent("Color Space*"), new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
          {
            this.serializedObject.ApplyModifiedProperties();
            GUIUtility.ExitGUI();
          }
          EditorGUI.EndDisabledGroup();
          if (QualitySettings.activeColorSpace != QualitySettings.desiredColorSpace)
            EditorGUILayout.HelpBox(PlayerSettingsEditor.s_Styles.colorSpaceWarning.text, MessageType.Warning);
        }
        this.GraphicsAPIsGUI(targetGroup, platform.DefaultTarget);
        if (targetGroup == BuildTargetGroup.XBOX360 || targetGroup == BuildTargetGroup.PSP2 || (targetGroup == BuildTargetGroup.PSM || targetGroup == BuildTargetGroup.Android) || targetGroup == BuildTargetGroup.SamsungTV)
        {
          if (this.IsMobileTarget(targetGroup))
            this.m_MobileMTRendering.boolValue = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Multithreaded Rendering*"), this.m_MobileMTRendering.boolValue, new GUILayoutOption[0]);
          else
            this.m_MTRendering.boolValue = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Multithreaded Rendering*"), this.m_MTRendering.boolValue, new GUILayoutOption[0]);
        }
        else if (targetGroup == BuildTargetGroup.PSP2 || targetGroup == BuildTargetGroup.PSM)
          this.m_MTRendering.boolValue = !Unsupported.IsDeveloperBuild() || EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Multithreaded Rendering*"), this.m_MTRendering.boolValue, new GUILayoutOption[0]);
        bool flag1 = targetGroup != BuildTargetGroup.PS3;
        bool flag2 = targetGroup != BuildTargetGroup.PS3 && targetGroup != BuildTargetGroup.XBOX360;
        if (settingsExtension != null)
        {
          flag1 = settingsExtension.SupportsStaticBatching();
          flag2 = settingsExtension.SupportsDynamicBatching();
        }
        int staticBatching;
        int dynamicBatching1;
        PlayerSettings.GetBatchingForPlatform(platform.DefaultTarget, out staticBatching, out dynamicBatching1);
        bool flag3 = false;
        if (!flag1 && staticBatching == 1)
        {
          staticBatching = 0;
          flag3 = true;
        }
        if (!flag2 && dynamicBatching1 == 1)
        {
          dynamicBatching1 = 0;
          flag3 = true;
        }
        if (flag3)
          PlayerSettings.SetBatchingForPlatform(platform.DefaultTarget, staticBatching, dynamicBatching1);
        EditorGUI.BeginChangeCheck();
        EditorGUI.BeginDisabledGroup(!flag1);
        if (GUI.enabled)
          staticBatching = !EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Static Batching"), staticBatching != 0, new GUILayoutOption[0]) ? 0 : 1;
        else
          EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Static Batching"), false, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(!flag2);
        int dynamicBatching2 = !EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Dynamic Batching"), dynamicBatching1 != 0, new GUILayoutOption[0]) ? 0 : 1;
        EditorGUI.EndDisabledGroup();
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(this.target, "Changed Batching Settings");
          PlayerSettings.SetBatchingForPlatform(platform.DefaultTarget, staticBatching, dynamicBatching2);
        }
        if (targetGroup == BuildTargetGroup.XBOX360 || targetGroup == BuildTargetGroup.WiiU || (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.WebPlayer) || (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || (targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.PSP2)) || (targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.PSM || targetGroup == BuildTargetGroup.Metro))
        {
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_SkinOnGPU, targetGroup == BuildTargetGroup.PS4 ? EditorGUIUtility.TextContent("Compute Skinning*|Use Compute pipeline for Skinning") : EditorGUIUtility.TextContent("GPU Skinning*|Use DX11/ES3 GPU Skinning"), new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
            ShaderUtil.RecreateSkinnedMeshResources();
        }
        if (targetGroup == BuildTargetGroup.XBOX360)
          this.m_XboxPIXTextureCapture.boolValue = EditorGUILayout.Toggle("Enable PIX texture capture", this.m_XboxPIXTextureCapture.boolValue, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.Metro)
          PlayerSettings.stereoscopic3D = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Stereoscopic rendering*"), PlayerSettings.stereoscopic3D, new GUILayoutOption[0]);
        if (PlayerSettingsEditor.TargetSupportsVirtualReality(targetGroup))
          PlayerSettings.virtualRealitySupported = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Virtual Reality Supported"), PlayerSettings.virtualRealitySupported, new GUILayoutOption[0]);
        EditorGUILayout.Space();
        if (settingsExtension != null && settingsExtension.HasIdentificationGUI())
        {
          GUILayout.Label(EditorGUIUtility.TextContent("Identification"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          if (settingsExtension.HasBundleIdentifier())
            EditorGUILayout.PropertyField(this.m_ApplicationBundleIdentifier, EditorGUIUtility.TextContent("Bundle Identifier"), new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_ApplicationBundleVersion, EditorGUIUtility.TextContent("Version*"), new GUILayoutOption[0]);
          if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
            EditorGUILayout.PropertyField(this.m_IPhoneBuildNumber, EditorGUIUtility.TextContent("Build"), new GUILayoutOption[0]);
          if (settingsExtension != null)
            settingsExtension.IdentificationSectionGUI();
          EditorGUILayout.Space();
        }
        GUILayout.Label(EditorGUIUtility.TextContent("Configuration"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        IScriptingImplementations scriptingImplementations = ModuleManager.GetScriptingImplementations(targetGroup);
        if (scriptingImplementations == null)
        {
          PlayerSettingsEditor.BuildDisabledEnumPopup(EditorGUIUtility.TextContent("Default"), EditorGUIUtility.TextContent("Scripting Backend"));
        }
        else
        {
          ScriptingImplementation[] scriptingImplementationArray = scriptingImplementations.Enabled();
          int num1 = 0;
          PlayerSettings.GetPropertyOptionalInt("ScriptingBackend", ref num1, targetGroup);
          bool flag4 = this.m_tvOSSdkVersion.intValue == 0;
          int num2;
          if (targetGroup == BuildTargetGroup.tvOS && flag4)
          {
            num2 = 1;
            PlayerSettingsEditor.BuildDisabledEnumPopup(new GUIContent("IL2CPP"), EditorGUIUtility.TextContent("Scripting Backend"));
          }
          else
            num2 = PlayerSettingsEditor.BuildEnumPopup<ScriptingImplementation>(EditorGUIUtility.TextContent("Scripting Backend"), targetGroup, "ScriptingBackend", scriptingImplementationArray, PlayerSettingsEditor.GetNiceScriptingBackendNames(scriptingImplementationArray));
          if (num2 != num1)
            PlayerSettings.SetPropertyInt("ScriptingBackend", num2, targetGroup);
          if (targetGroup == BuildTargetGroup.Android && num2 == 1)
            EditorGUILayout.HelpBox("IL2CPP on Android is experimental and unsupported", MessageType.Info);
        }
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || (targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.Metro) || targetGroup == BuildTargetGroup.BlackBerry || PlayerSettingsEditor.IsWP8Player(targetGroup))
        {
          if (targetGroup == BuildTargetGroup.iPhone)
          {
            EditorGUILayout.PropertyField(this.m_TargetDevice);
            EditorGUILayout.PropertyField(this.m_IPhoneSdkVersion, EditorGUIUtility.TextContent("Target SDK"), new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_IPhoneTargetOSVersion, EditorGUIUtility.TextContent("Target minimum iOS Version"), new GUILayoutOption[0]);
          }
          if (targetGroup == BuildTargetGroup.tvOS)
          {
            EditorGUILayout.PropertyField(this.m_tvOSSdkVersion, EditorGUIUtility.TextContent("Target SDK"), new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_tvOSTargetOSVersion, EditorGUIUtility.TextContent("Target minimum tvOS Version"), new GUILayoutOption[0]);
          }
          if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
          {
            EditorGUILayout.PropertyField(this.m_useOnDemandResources, EditorGUIUtility.TextContent("Use on demand resources"), new GUILayoutOption[0]);
            if (this.m_useOnDemandResources.boolValue && this.m_IPhoneTargetOSVersion.intValue < 40)
              this.m_IPhoneTargetOSVersion.intValue = 40;
          }
          if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Metro || PlayerSettingsEditor.IsWP8Player(targetGroup))
            EditorGUILayout.PropertyField(this.m_AccelerometerFrequency, EditorGUIUtility.TextContent("Accelerometer Frequency"), new GUILayoutOption[0]);
          if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
            EditorGUILayout.PropertyField(this.m_LocationUsageDescription, EditorGUIUtility.TextContent("Location Usage Description"), new GUILayoutOption[0]);
          if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
          {
            EditorGUILayout.PropertyField(this.m_OverrideIPodMusic, EditorGUIUtility.TextContent("Override iPod Music"), new GUILayoutOption[0]);
            if (targetGroup == BuildTargetGroup.iPhone)
              EditorGUILayout.PropertyField(this.m_PrepareIOSForRecording, EditorGUIUtility.TextContent("Prepare iOS for Recording"), new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_UIRequiresPersistentWiFi, EditorGUIUtility.TextContent("Requires Persistent WiFi"), new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_IOSAppInBackgroundBehavior, EditorGUIUtility.TextContent("Behavior in Background"), new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_IOSAllowHTTPDownload, EditorGUIUtility.TextContent("Allow downloads over HTTP (nonsecure)"), new GUILayoutOption[0]);
          }
        }
        EditorGUI.BeginDisabledGroup(!Application.HasProLicense());
        bool flag5 = !this.m_SubmitAnalytics.boolValue;
        bool flag6 = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Disable HW Statistics|Disables HW Statistics (Pro Only)"), flag5, new GUILayoutOption[0]);
        if (flag5 != flag6)
          this.m_SubmitAnalytics.boolValue = !flag6;
        if (!Application.HasProLicense())
          this.m_SubmitAnalytics.boolValue = true;
        EditorGUI.EndDisabledGroup();
        if (settingsExtension != null)
          settingsExtension.ConfigurationSectionGUI();
        EditorGUILayout.LabelField(EditorGUIUtility.TextContent("Scripting Define Symbols"));
        EditorGUI.BeginChangeCheck();
        string defines = EditorGUILayout.DelayedTextField(PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup), EditorStyles.textField, new GUILayoutOption[0]);
        this.scriptingDefinesControlID = EditorGUIUtility.s_LastControlID;
        if (EditorGUI.EndChangeCheck())
          PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
        EditorGUILayout.Space();
        GUILayout.Label(EditorGUIUtility.TextContent("Optimization"), EditorStyles.boldLabel, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.WebPlayer)
          this.ShowDisabledFakeEnumPopup(PlayerSettingsEditor.FakeEnum.WebplayerSubset);
        else if (targetGroup == BuildTargetGroup.WiiU)
          this.ShowDisabledFakeEnumPopup(PlayerSettingsEditor.FakeEnum.WiiUSubset);
        else if (targetGroup == BuildTargetGroup.Metro)
        {
          this.ShowDisabledFakeEnumPopup(PlayerSettingsEditor.FakeEnum.WSASubset);
        }
        else
        {
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_ApiCompatibilityLevel);
          if (EditorGUI.EndChangeCheck())
            PlayerSettings.SetApiCompatibilityInternal(this.m_ApiCompatibilityLevel.intValue);
        }
        EditorGUILayout.PropertyField(this.m_BakeCollisionMeshes, EditorGUIUtility.TextContent("Prebake Collision Meshes|Bake collision data into the meshes on build time"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_PreloadShaders, EditorGUIUtility.TextContent("Preload Shaders"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_PreloadedAssets, EditorGUIUtility.TextContent("Preloaded Assets|Assets to load at start up in the player"), true, new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || (targetGroup == BuildTargetGroup.XBOX360 || targetGroup == BuildTargetGroup.XboxOne) || (targetGroup == BuildTargetGroup.WiiU || targetGroup == BuildTargetGroup.PS3 || targetGroup == BuildTargetGroup.PS4) || targetGroup == BuildTargetGroup.PSP2)
          EditorGUILayout.PropertyField(this.m_AotOptions, EditorGUIUtility.TextContent("AOT Compilation Options"), new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || (targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.BlackBerry) || (targetGroup == BuildTargetGroup.Tizen || targetGroup == BuildTargetGroup.WebGL || (targetGroup == BuildTargetGroup.PS3 || targetGroup == BuildTargetGroup.WiiU)) || (targetGroup == BuildTargetGroup.PSP2 || targetGroup == BuildTargetGroup.PS4 || (targetGroup == BuildTargetGroup.XBOX360 || targetGroup == BuildTargetGroup.XboxOne)) || targetGroup == BuildTargetGroup.Metro)
        {
          int num = -1;
          PlayerSettings.GetPropertyOptionalInt("ScriptingBackend", ref num, targetGroup);
          if (targetGroup == BuildTargetGroup.WebGL || num == 1)
            EditorGUILayout.PropertyField(this.m_StripEngineCode, EditorGUIUtility.TextContent("Strip Engine Code*|Strip Unused Engine Code - Note that byte code stripping of managed assemblies is always enabled for the IL2CPP scripting backend."), new GUILayoutOption[0]);
          else if (num != 2)
            EditorGUILayout.PropertyField(this.m_IPhoneStrippingLevel, EditorGUIUtility.TextContent("Stripping Level*"), new GUILayoutOption[0]);
        }
        if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
          EditorGUILayout.PropertyField(this.m_IPhoneScriptCallOptimization, EditorGUIUtility.TextContent("Script Call Optimization"), new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.Android)
          EditorGUILayout.PropertyField(this.m_AndroidProfiler, EditorGUIUtility.TextContent("Enable Internal Profiler"), new GUILayoutOption[0]);
        EditorGUILayout.Space();
        VertexChannelCompressionFlags intValue = (VertexChannelCompressionFlags) this.m_VertexChannelCompressionMask.intValue;
        this.m_VertexChannelCompressionMask.intValue = (int) EditorGUILayout.EnumMaskPopup(PlayerSettingsEditor.s_Styles.vertexChannelCompressionMask, (Enum) intValue);
        if (targetGroup != BuildTargetGroup.PSM)
          EditorGUILayout.PropertyField(this.m_StripUnusedMeshComponents, EditorGUIUtility.TextContent("Optimize Mesh Data*|Remove unused mesh components"), new GUILayoutOption[0]);
        if (targetGroup == BuildTargetGroup.PS3 || targetGroup == BuildTargetGroup.PSP2 || targetGroup == BuildTargetGroup.PSM)
        {
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.PropertyField(this.m_VideoMemoryForVertexBuffers, EditorGUIUtility.TextContent("Mesh Video Mem*|How many megabytes of video memory to use for mesh data before we use main memory"), new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
          {
            if (this.m_VideoMemoryForVertexBuffers.intValue < 0)
              this.m_VideoMemoryForVertexBuffers.intValue = 0;
            else if (this.m_VideoMemoryForVertexBuffers.intValue > 192)
              this.m_VideoMemoryForVertexBuffers.intValue = 192;
          }
        }
        EditorGUILayout.Space();
        this.ShowSharedNote();
      }
      this.EndSettingsBox();
    }

    private static GUIContent[] GetNiceScriptingBackendNames(ScriptingImplementation[] scriptingBackends)
    {
      GUIContent[] guiContentArray = new GUIContent[scriptingBackends.Length];
      for (int index = 0; index < scriptingBackends.Length; ++index)
      {
        if (!PlayerSettingsEditor.m_NiceScriptingBackendNames.ContainsKey(scriptingBackends[index]))
          throw new NotImplementedException("Missing nice scripting implementation name");
        guiContentArray[index] = PlayerSettingsEditor.m_NiceScriptingBackendNames[scriptingBackends[index]];
      }
      return guiContentArray;
    }

    private void AutoAssignProperty(SerializedProperty property, string packageDir, string fileName)
    {
      if (property.stringValue.Length != 0 && File.Exists(Path.Combine(packageDir, property.stringValue)) || !File.Exists(Path.Combine(packageDir, fileName)))
        return;
      property.stringValue = fileName;
    }

    public void BrowseablePathProperty(string propertyLabel, SerializedProperty property, string browsePanelTitle, string extension, string dir)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel(EditorGUIUtility.TextContent(propertyLabel));
      GUIContent content = new GUIContent("...");
      Vector2 vector2 = GUI.skin.GetStyle("Button").CalcSize(content);
      if (GUILayout.Button(content, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.MaxWidth(vector2.x) }))
      {
        GUI.FocusControl(string.Empty);
        string text = EditorGUIUtility.TextContent(browsePanelTitle).text;
        string str1 = !string.IsNullOrEmpty(dir) ? dir.Replace('\\', '/') + "/" : Directory.GetCurrentDirectory().Replace('\\', '/') + "/";
        string empty = string.Empty;
        string str2 = !string.IsNullOrEmpty(extension) ? EditorUtility.OpenFilePanel(text, str1, extension) : EditorUtility.OpenFolderPanel(text, str1, string.Empty);
        if (str2.StartsWith(str1))
          str2 = str2.Substring(str1.Length);
        if (!string.IsNullOrEmpty(str2))
        {
          property.stringValue = str2;
          this.serializedObject.ApplyModifiedProperties();
          GUIUtility.ExitGUI();
        }
      }
      GUIContent guiContent;
      if (string.IsNullOrEmpty(property.stringValue))
      {
        guiContent = EditorGUIUtility.TextContent("Not selected.");
        EditorGUI.BeginDisabledGroup(true);
      }
      else
      {
        guiContent = EditorGUIUtility.TempContent(property.stringValue);
        EditorGUI.BeginDisabledGroup(false);
      }
      EditorGUI.BeginChangeCheck();
      GUILayoutOption[] guiLayoutOptionArray = new GUILayoutOption[2]{ GUILayout.Width(32f), GUILayout.ExpandWidth(true) };
      string str = EditorGUILayout.TextArea(guiContent.text, guiLayoutOptionArray);
      EditorGUI.EndDisabledGroup();
      if (EditorGUI.EndChangeCheck() && string.IsNullOrEmpty(str))
      {
        property.stringValue = string.Empty;
        this.serializedObject.ApplyModifiedProperties();
        GUI.FocusControl(string.Empty);
        GUIUtility.ExitGUI();
      }
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
    }

    internal static void BuildFileBoxButton(SerializedProperty prop, string uiString, string directory, string ext)
    {
      PlayerSettingsEditor.BuildFileBoxButton(prop, uiString, directory, ext, (System.Action) null);
    }

    internal static void BuildFileBoxButton(SerializedProperty prop, string uiString, string directory, string ext, System.Action onSelect)
    {
      float num = 16f;
      Rect rect = GUILayoutUtility.GetRect((float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0), (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0), num, num, EditorStyles.layerMaskField, (GUILayoutOption[]) null);
      float labelWidth = EditorGUIUtility.labelWidth;
      Rect position = new Rect(rect.x + EditorGUI.indent, rect.y, labelWidth - EditorGUI.indent, rect.height);
      EditorGUI.TextArea(new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height), prop.stringValue.Length != 0 ? prop.stringValue : "Not selected.", EditorStyles.label);
      if (!GUI.Button(position, EditorGUIUtility.TextContent(uiString)))
        return;
      string path = EditorUtility.OpenFilePanel(EditorGUIUtility.TextContent(uiString).text, directory, ext);
      string projectRelativePath = FileUtil.GetProjectRelativePath(path);
      prop.stringValue = !(projectRelativePath != string.Empty) ? path : projectRelativePath;
      if (onSelect != null)
        onSelect();
      prop.serializedObject.ApplyModifiedProperties();
      GUIUtility.ExitGUI();
    }

    public void PublishSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
    {
      if (targetGroup != BuildTargetGroup.Metro && targetGroup != BuildTargetGroup.XBOX360 && (targetGroup != BuildTargetGroup.PS3 && targetGroup != BuildTargetGroup.PSP2) && (targetGroup != BuildTargetGroup.PSM && (settingsExtension == null || !settingsExtension.HasPublishSection())))
        return;
      GUI.changed = false;
      if (this.BeginSettingsBox(5, EditorGUIUtility.TextContent("Publishing Settings")))
      {
        string directory = FileUtil.DeleteLastPathNameComponent(Application.dataPath);
        float h = 16f;
        float midWidth = (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0);
        float maxWidth = (float) (80.0 + (double) EditorGUIUtility.fieldWidth + 5.0);
        if (settingsExtension != null)
          settingsExtension.PublishSectionGUI(h, midWidth, maxWidth);
        if (targetGroup != BuildTargetGroup.PSM)
          ;
        if (targetGroup == BuildTargetGroup.XBOX360)
        {
          this.m_XboxAdditionalTitleMemorySize = this.serializedObject.FindProperty("xboxAdditionalTitleMemorySize");
          this.m_XboxAdditionalTitleMemorySize.intValue = (int) EditorGUILayout.Slider(EditorGUIUtility.TextContent("Extra title memory (1GB)"), (float) this.m_XboxAdditionalTitleMemorySize.intValue, 0.0f, 416f, new GUILayoutOption[0]);
          if (this.m_XboxAdditionalTitleMemorySize.intValue > 0)
            PlayerSettingsEditor.ShowWarning(EditorGUIUtility.TextContent("If the target is a retail console, or a standard 512MB XDK, the executable produced may fail to run."));
          GUILayout.Label(EditorGUIUtility.TextContent("Submission"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_XboxTitleId, EditorGUIUtility.TextContent("Title Id"), new GUILayoutOption[0]);
          EditorGUILayout.Space();
          GUILayout.Label(EditorGUIUtility.TextContent("Image Conversion"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          PlayerSettingsEditor.BuildFileBoxButton(this.m_XboxImageXexPath, "ImageXEX config override", directory, "cfg", (System.Action) null);
          EditorGUILayout.Space();
          GUILayout.Label(EditorGUIUtility.TextContent("Xbox Live"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          System.Action onSelect = (System.Action) (() =>
          {
            if (this.m_XboxTitleId.stringValue.Length != 0)
              return;
            Debug.LogWarning((object) "Title id must be present when using a SPA file.");
          });
          PlayerSettingsEditor.BuildFileBoxButton(this.m_XboxSpaPath, "SPA config", directory, "spa", onSelect);
          if (this.m_XboxSpaPath.stringValue.Length > 0)
          {
            bool boolValue = this.m_XboxGenerateSpa.boolValue;
            this.m_XboxGenerateSpa.boolValue = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Generate _SPAConfig.cs"), boolValue, new GUILayoutOption[0]);
            if (!boolValue && this.m_XboxGenerateSpa.boolValue)
              InternalEditorUtility.Xbox360GenerateSPAConfig(this.m_XboxSpaPath.stringValue);
          }
          this.m_XboxEnableGuest.boolValue = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Enable Guest accounts"), this.m_XboxEnableGuest.boolValue, new GUILayoutOption[0]);
          EditorGUILayout.Space();
          GUILayout.Label(EditorGUIUtility.TextContent("Services"), EditorStyles.boldLabel, new GUILayoutOption[0]);
          this.m_XboxEnableAvatar.boolValue = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Enable Avatar rendering"), this.m_XboxEnableAvatar.boolValue, new GUILayoutOption[0]);
          this.KinectGUI();
        }
      }
      this.EndSettingsBox();
    }

    private void KinectGUI()
    {
      GUILayout.Label(EditorGUIUtility.TextContent("Kinect"), EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_XboxEnableKinect.boolValue = EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Enable Kinect"), this.m_XboxEnableKinect.boolValue, new GUILayoutOption[0]);
      if (this.m_XboxEnableKinect.boolValue)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10f);
        this.m_XboxEnableHeadOrientation.boolValue = GUILayout.Toggle(this.m_XboxEnableHeadOrientation.boolValue, new GUIContent("Head Orientation", "Head orientation support"));
        this.m_XboxEnableKinectAutoTracking.boolValue = GUILayout.Toggle(this.m_XboxEnableKinectAutoTracking.boolValue, new GUIContent("Auto Tracking", "Automatic player tracking"));
        this.m_XboxEnableFitness.boolValue = GUILayout.Toggle(this.m_XboxEnableFitness.boolValue, new GUIContent("Fitness", "Fitness support"));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Space(10f);
        this.m_XboxEnableSpeech.boolValue = GUILayout.Toggle(this.m_XboxEnableSpeech.boolValue, new GUIContent("Speech", "Speech Recognition Support"));
        GUILayout.EndHorizontal();
        this.m_XboxDeployKinectResources.boolValue = true;
        if (this.m_XboxEnableHeadOrientation.boolValue)
          this.m_XboxDeployHeadOrientation.boolValue = true;
      }
      GUILayout.Label(EditorGUIUtility.TextContent("Deploy Kinect resources"), EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      GUI.enabled = !this.m_XboxEnableKinect.boolValue;
      this.m_XboxDeployKinectResources.boolValue = GUILayout.Toggle(this.m_XboxDeployKinectResources.boolValue, new GUIContent("Base", "Identity and Skeleton Database files"));
      GUI.enabled = (!this.m_XboxEnableHeadOrientation.boolValue ? 0 : (this.m_XboxEnableKinect.boolValue ? 1 : 0)) == 0;
      this.m_XboxDeployHeadOrientation.boolValue = GUILayout.Toggle(this.m_XboxDeployHeadOrientation.boolValue, new GUIContent("Head Orientation", "Head orientation database"));
      GUI.enabled = true;
      this.m_XboxDeployKinectHeadPosition.boolValue = GUILayout.Toggle(this.m_XboxDeployKinectHeadPosition.boolValue, new GUIContent("Head Position", "Head position database"));
      GUILayout.EndHorizontal();
      GUILayout.Label(EditorGUIUtility.TextContent("Speech"));
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 1) != 0, new GUIContent("en-US", "Speech database: English - US, Canada")) == ((this.m_XboxSpeechDB.intValue & 1) != 0) ? 0 : 1;
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 2) != 0, new GUIContent("fr-CA", "Speech database: French - Canada")) == ((this.m_XboxSpeechDB.intValue & 2) != 0) ? 0 : 2;
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 4) != 0, new GUIContent("en-GB", "Speech database: English - United Kingdom, Ireland")) == ((this.m_XboxSpeechDB.intValue & 4) != 0) ? 0 : 4;
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 8) != 0, new GUIContent("es-MX", "Speech database: Spanish - Mexico")) == ((this.m_XboxSpeechDB.intValue & 8) != 0) ? 0 : 8;
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 16) != 0, new GUIContent("ja-JP", "Speech database: Japanese - Japan")) == ((this.m_XboxSpeechDB.intValue & 16) != 0) ? 0 : 16;
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 32) != 0, new GUIContent("fr-FR", "Speech database: French - France, Switzerland")) == ((this.m_XboxSpeechDB.intValue & 32) != 0) ? 0 : 32;
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 64) != 0, new GUIContent("es-ES", "Speech database: Spanish - Spain")) == ((this.m_XboxSpeechDB.intValue & 64) != 0) ? 0 : 64;
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 128) != 0, new GUIContent("de-DE", "Speech database: German - Germany, Austria, Switzerland")) == ((this.m_XboxSpeechDB.intValue & 128) != 0) ? 0 : 128;
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 256) != 0, new GUIContent("it-IT", "Speech database: Italian - Italy")) == ((this.m_XboxSpeechDB.intValue & 256) != 0) ? 0 : 256;
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 512) != 0, new GUIContent("en-AU", "Speech database: English - Australia, New Zealand")) == ((this.m_XboxSpeechDB.intValue & 512) != 0) ? 0 : 512;
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Space(10f);
      this.m_XboxSpeechDB.intValue ^= GUILayout.Toggle((this.m_XboxSpeechDB.intValue & 1024) != 0, new GUIContent("pt-BR", "Speech database: Portuguese - Brazil")) == ((this.m_XboxSpeechDB.intValue & 1024) != 0) ? 0 : 1024;
      GUILayout.EndHorizontal();
    }

    private static void ShowWarning(GUIContent warningMessage)
    {
      if ((UnityEngine.Object) PlayerSettingsEditor.s_WarningIcon == (UnityEngine.Object) null)
        PlayerSettingsEditor.s_WarningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
      warningMessage.image = (Texture) PlayerSettingsEditor.s_WarningIcon;
      GUILayout.Space(5f);
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      GUILayout.Label(warningMessage, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
      GUILayout.EndVertical();
    }

    private class Styles
    {
      public GUIStyle categoryBox = new GUIStyle(EditorStyles.helpBox);
      public GUIContent colorSpaceWarning = EditorGUIUtility.TextContent("Selected color space is not supported on this hardware.");
      public GUIContent cursorHotspot = EditorGUIUtility.TextContent("Cursor Hotspot");
      public GUIContent defaultCursor = EditorGUIUtility.TextContent("Default Cursor");
      public GUIContent defaultIcon = EditorGUIUtility.TextContent("Default Icon");
      public readonly GUIContent vertexChannelCompressionMask = EditorGUIUtility.TextContent("Vertex Compression|Select which vertex channels should be compressed. Compression can save memory and bandwidth but precision will be lower.");

      public Styles()
      {
        this.categoryBox.padding.left = 14;
      }
    }

    private enum FakeEnum
    {
      WebplayerSubset,
      WiiUSubset,
      WSASubset,
    }

    internal class WebPlayerTemplateManager : WebTemplateManagerBase
    {
      private const string kWebTemplateDefaultIconResource = "BuildSettings.Web.Small";

      public override string customTemplatesFolder
      {
        get
        {
          return Path.Combine(Application.dataPath.Replace('/', Path.DirectorySeparatorChar), "WebPlayerTemplates");
        }
      }

      public override string builtinTemplatesFolder
      {
        get
        {
          return Path.Combine(Path.Combine(EditorApplication.applicationContentsPath.Replace('/', Path.DirectorySeparatorChar), "Resources"), "WebPlayerTemplates");
        }
      }

      public override Texture2D defaultIcon
      {
        get
        {
          return (Texture2D) EditorGUIUtility.IconContent("BuildSettings.Web.Small").image;
        }
      }
    }
  }
}
