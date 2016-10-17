// Decompiled with JetBrains decompiler
// Type: UnityEditor.PlayerSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Player Settings is where you define various parameters for the final game that you will build in Unity. Some of these values are used in the Resolution Dialog that launches when you open a standalone game.</para>
  /// </summary>
  public sealed class PlayerSettings : UnityEngine.Object
  {
    internal static readonly char[] defineSplits = new char[3]{ ';', ',', ' ' };
    private static SerializedObject _serializedObject;

    /// <summary>
    ///   <para>The name of your company.</para>
    /// </summary>
    public static extern string companyName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The name of your product.</para>
    /// </summary>
    public static extern string productName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the builtin Unity splash screen be shown?</para>
    /// </summary>
    public static extern bool showUnitySplashScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A unique cloud project identifier. It is unique for every project (Read Only).</para>
    /// </summary>
    public static string cloudProjectId
    {
      get
      {
        return PlayerSettings.cloudProjectIdRaw;
      }
      internal set
      {
        PlayerSettings.cloudProjectIdRaw = value;
      }
    }

    private static extern string cloudProjectIdRaw { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static Guid productGUID
    {
      get
      {
        return new Guid(PlayerSettings.productGUIDRaw);
      }
    }

    private static extern byte[] productGUIDRaw { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Set the rendering color space for the current project.</para>
    /// </summary>
    public static extern ColorSpace colorSpace { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Default horizontal dimension of stand-alone player window.</para>
    /// </summary>
    public static extern int defaultScreenWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Default vertical dimension of stand-alone player window.</para>
    /// </summary>
    public static extern int defaultScreenHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Default horizontal dimension of web player window.</para>
    /// </summary>
    public static extern int defaultWebScreenWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Default vertical dimension of web player window.</para>
    /// </summary>
    public static extern int defaultWebScreenHeight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Defines the behaviour of the Resolution Dialog on product launch.</para>
    /// </summary>
    public static extern ResolutionDialogSetting displayResolutionDialog { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If enabled, the game will default to fullscreen mode.</para>
    /// </summary>
    public static extern bool defaultIsFullScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool defaultIsNativeResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If enabled, your game will continue to run after lost focus.</para>
    /// </summary>
    public static extern bool runInBackground { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Defines if fullscreen games should darken secondary displays.</para>
    /// </summary>
    public static extern bool captureSingleScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Write a log file with debugging information.</para>
    /// </summary>
    public static extern bool usePlayerLog { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use resizable window in standalone player builds.</para>
    /// </summary>
    public static extern bool resizableWindow { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Pre bake collision meshes on player build.</para>
    /// </summary>
    public static extern bool bakeCollisionMeshes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable receipt validation for the Mac App Store.</para>
    /// </summary>
    public static extern bool useMacAppStoreValidation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Define how to handle fullscreen mode in Mac OS X standalones.</para>
    /// </summary>
    public static extern MacFullscreenMode macFullscreenMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Define how to handle fullscreen mode in Windows standalones (Direct3D 9 mode).</para>
    /// </summary>
    public static extern D3D9FullscreenMode d3d9FullscreenMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Define how to handle fullscreen mode in Windows standalones (Direct3D 11 mode).</para>
    /// </summary>
    public static extern D3D11FullscreenMode d3d11FullscreenMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable virtual reality support.</para>
    /// </summary>
    public static extern bool virtualRealitySupported { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>On Windows, show the application in the background if Fullscreen Windowed mode is used.</para>
    /// </summary>
    public static extern bool visibleInBackground { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If enabled, allows the user to switch between full screen and windowed mode using OS specific keyboard short cuts.</para>
    /// </summary>
    public static extern bool allowFullscreenSwitch { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Restrict standalone players to a single concurrent running instance.</para>
    /// </summary>
    public static extern bool forceSingleInstance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("The option alwaysDisplayWatermark is deprecated and is always false.")]
    public static bool alwaysDisplayWatermark
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>First level to have access to all Resources.Load assets in Streamed Web Players.</para>
    /// </summary>
    [Obsolete("Use AssetBundles instead for streaming data", true)]
    public static int firstStreamedLevelWithResources
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>The image to display in the Resolution Dialog window.</para>
    /// </summary>
    public static extern Texture2D resolutionDialogBanner { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Virtual Reality specific splash screen.</para>
    /// </summary>
    public static extern Texture2D virtualRealitySplashScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The bundle identifier of the iPhone application.</para>
    /// </summary>
    public static extern string iPhoneBundleIdentifier { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern string webPlayerTemplate { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern string[] templateCustomKeys { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern string spritePackerPolicy { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Password used for interacting with the Android Keystore.</para>
    /// </summary>
    public static extern string keystorePass { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Password for the key used for signing an Android application.</para>
    /// </summary>
    public static extern string keyaliasPass { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Xbox 360 title id.</para>
    /// </summary>
    public static extern string xboxTitleId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Xbox 360 ImageXex override configuration file path.</para>
    /// </summary>
    public static extern string xboxImageXexFilePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Xbox 360 SPA file path.</para>
    /// </summary>
    public static extern string xboxSpaFilePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Xbox 360 auto-generation of _SPAConfig.cs.</para>
    /// </summary>
    public static extern bool xboxGenerateSpa { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool xboxEnableGuest { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Xbox 360 Kinect resource file deployment.</para>
    /// </summary>
    public static extern bool xboxDeployKinectResources { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Xbox 360 Kinect Head Orientation file deployment.</para>
    /// </summary>
    public static extern bool xboxDeployKinectHeadOrientation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Xbox 360 Kinect Head Position file deployment.</para>
    /// </summary>
    public static extern bool xboxDeployKinectHeadPosition { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Xbox 360 splash screen.</para>
    /// </summary>
    public static extern Texture2D xboxSplashScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int xboxAdditionalTitleMemorySize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Xbox 360 Kinect title flag - if false, the Kinect APIs are inactive.</para>
    /// </summary>
    public static extern bool xboxEnableKinect { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Xbox 360 Kinect automatic skeleton tracking.</para>
    /// </summary>
    public static extern bool xboxEnableKinectAutoTracking { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Xbox 360 Kinect Enable Speech Engine.</para>
    /// </summary>
    public static extern bool xboxEnableSpeech { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Xbox 360 Kinect Speech DB.</para>
    /// </summary>
    public static extern uint xboxSpeechDB { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Enable GPU skinning on capable platforms.</para>
    /// </summary>
    public static extern bool gpuSkinning { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool xboxPIXTextureCapture { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Xbox 360 Avatars.</para>
    /// </summary>
    public static extern bool xboxEnableAvatar { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern int xboxOneResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Enables internal profiler.</para>
    /// </summary>
    public static extern bool enableInternalProfiler { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Sets the crash behavior on .NET unhandled exception.</para>
    /// </summary>
    public static extern ActionOnDotNetUnhandledException actionOnDotNetUnhandledException { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Are ObjC uncaught exceptions logged?</para>
    /// </summary>
    public static extern bool logObjCUncaughtExceptions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables CrashReport API.</para>
    /// </summary>
    public static extern bool enableCrashReportAPI { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Describes the reason for access to the user's location data.</para>
    /// </summary>
    public static extern string locationUsageDescription { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Application bundle identifier shared between iOS &amp; Android platforms.</para>
    /// </summary>
    public static extern string bundleIdentifier { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Application bundle version shared between iOS &amp; Android platforms.</para>
    /// </summary>
    public static extern string bundleVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should status bar be hidden. Shared between iOS &amp; Android platforms.</para>
    /// </summary>
    public static extern bool statusBarHidden { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Managed code stripping level.</para>
    /// </summary>
    public static extern StrippingLevel strippingLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Remove unused Engine code from your build (IL2CPP-only).</para>
    /// </summary>
    public static extern bool stripEngineCode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Default screen orientation for mobiles.</para>
    /// </summary>
    public static extern UIOrientation defaultInterfaceOrientation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is auto-rotation to portrait supported?</para>
    /// </summary>
    public static extern bool allowedAutorotateToPortrait { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is auto-rotation to portrait upside-down supported?</para>
    /// </summary>
    public static extern bool allowedAutorotateToPortraitUpsideDown { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is auto-rotation to landscape right supported?</para>
    /// </summary>
    public static extern bool allowedAutorotateToLandscapeRight { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is auto-rotation to landscape left supported?</para>
    /// </summary>
    public static extern bool allowedAutorotateToLandscapeLeft { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Let the OS autorotate the screen as the device orientation changes.</para>
    /// </summary>
    public static extern bool useAnimatedAutorotation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>32-bit Display Buffer is used.</para>
    /// </summary>
    public static extern bool use32BitDisplayBuffer { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("targetGlesGraphics is ignored, use SetGraphicsAPIs/GetGraphicsAPIs")]
    public static extern TargetGlesGraphics targetGlesGraphics { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("targetIOSGraphics is ignored, use SetGraphicsAPIs/GetGraphicsAPIs")]
    public static extern TargetIOSGraphics targetIOSGraphics { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>.NET API compatibility level.</para>
    /// </summary>
    public static extern ApiCompatibilityLevel apiCompatibilityLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should unused Mesh components be excluded from game build?</para>
    /// </summary>
    public static extern bool stripUnusedMeshComponents { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the advanced version being used?</para>
    /// </summary>
    public static extern bool advancedLicense { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Additional AOT compilation options. Shared by AOT platforms.</para>
    /// </summary>
    public static extern string aotOptions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Accelerometer update frequency.</para>
    /// </summary>
    public static extern int accelerometerFrequency { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is multi-threaded rendering enabled?</para>
    /// </summary>
    public static extern bool MTRendering { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool mobileMTRendering { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Which rendering path is enabled?</para>
    /// </summary>
    public static extern RenderingPath renderingPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern RenderingPath mobileRenderingPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should Direct3D 11 be used when available?</para>
    /// </summary>
    [Obsolete("Use SetGraphicsAPIs/GetGraphicsAPIs instead")]
    public static extern bool useDirect3D11 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool submitAnalytics { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should player render in stereoscopic 3d on supported hardware?</para>
    /// </summary>
    public static extern bool stereoscopic3D { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern UnityEngine.Object InternalGetPlayerSettingsObject();

    internal static SerializedObject GetSerializedObject()
    {
      if (PlayerSettings._serializedObject == null)
        PlayerSettings._serializedObject = new SerializedObject(PlayerSettings.InternalGetPlayerSettingsObject());
      return PlayerSettings._serializedObject;
    }

    internal static SerializedProperty FindProperty(string name)
    {
      SerializedProperty property = PlayerSettings.GetSerializedObject().FindProperty(name);
      if (property == null)
        Debug.LogError((object) ("Failed to find:" + name));
      return property;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPropertyIntInternal(string name, int value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InitializePropertyIntInternal(string name, int value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetPropertyIntInternal(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPropertyBoolInternal(string name, bool value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InitializePropertyBoolInternal(string name, bool value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool GetPropertyBoolInternal(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPropertyStringInternal(string name, string value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InitializePropertyStringInternal(string name, string value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetPropertyStringInternal(string name);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetPropertyNameForBuildTargetGroupInternal(BuildTargetGroup target, string name);

    internal static string GetPropertyNameForBuildTarget(BuildTargetGroup target, string name)
    {
      string targetGroupInternal = PlayerSettings.GetPropertyNameForBuildTargetGroupInternal(target, name);
      if (targetGroupInternal != string.Empty)
        return targetGroupInternal;
      throw new ArgumentException("Failed to get property name for the given target.");
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void AddEnum(string className, string propertyName, int value, string valueName);

    [ExcludeFromDocs]
    public static void SetPropertyInt(string name, int value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      PlayerSettings.SetPropertyInt(name, value, target);
    }

    /// <summary>
    ///   <para>Sets a PlayerSettings named int property.</para>
    /// </summary>
    /// <param name="name">Name of the property.</param>
    /// <param name="value">Value of the property (int).</param>
    /// <param name="target">BuildTarget for which the property should apply (use default value BuildTargetGroup.Unknown to apply to all targets).</param>
    public static void SetPropertyInt(string name, int value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      PlayerSettings.SetPropertyIntInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name), value);
    }

    public static void SetPropertyInt(string name, int value, BuildTarget target)
    {
      PlayerSettings.SetPropertyInt(name, value, BuildPipeline.GetBuildTargetGroup(target));
    }

    [ExcludeFromDocs]
    public static int GetPropertyInt(string name)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      return PlayerSettings.GetPropertyInt(name, target);
    }

    /// <summary>
    ///   <para>Returns a PlayerSettings named int property (with an optional build target it should apply to).</para>
    /// </summary>
    /// <param name="name">Name of the property.</param>
    /// <param name="target">BuildTarget for which the property should apply (use default value BuildTargetGroup.Unknown to apply to all targets).</param>
    /// <returns>
    ///   <para>The current value of the property.</para>
    /// </returns>
    public static int GetPropertyInt(string name, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      return PlayerSettings.GetPropertyIntInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name));
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetPropertyOptionalInt(string name, ref int value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target);

    [ExcludeFromDocs]
    public static bool GetPropertyOptionalInt(string name, ref int value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      return PlayerSettings.GetPropertyOptionalInt(name, ref value, target);
    }

    [ExcludeFromDocs]
    internal static void InitializePropertyInt(string name, int value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      PlayerSettings.InitializePropertyInt(name, value, target);
    }

    internal static void InitializePropertyInt(string name, int value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      PlayerSettings.InitializePropertyIntInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name), value);
    }

    [ExcludeFromDocs]
    public static void SetPropertyBool(string name, bool value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      PlayerSettings.SetPropertyBool(name, value, target);
    }

    /// <summary>
    ///   <para>Sets a PlayerSettings named bool property.</para>
    /// </summary>
    /// <param name="name">Name of the property.</param>
    /// <param name="value">Value of the property (bool).</param>
    /// <param name="target">BuildTarget for which the property should apply (use default value BuildTargetGroup.Unknown to apply to all targets).</param>
    public static void SetPropertyBool(string name, bool value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      PlayerSettings.SetPropertyBoolInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name), value);
    }

    public static void SetPropertyBool(string name, bool value, BuildTarget target)
    {
      PlayerSettings.SetPropertyBool(name, value, BuildPipeline.GetBuildTargetGroup(target));
    }

    [ExcludeFromDocs]
    public static bool GetPropertyBool(string name)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      return PlayerSettings.GetPropertyBool(name, target);
    }

    /// <summary>
    ///   <para>Returns a PlayerSettings named bool property (with an optional build target it should apply to).</para>
    /// </summary>
    /// <param name="name">Name of the property.</param>
    /// <param name="target">BuildTarget for which the property should apply (use default value BuildTargetGroup.Unknown to apply to all targets).</param>
    /// <returns>
    ///   <para>The current value of the property.</para>
    /// </returns>
    public static bool GetPropertyBool(string name, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      return PlayerSettings.GetPropertyBoolInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name));
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetPropertyOptionalBool(string name, ref bool value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target);

    [ExcludeFromDocs]
    public static bool GetPropertyOptionalBool(string name, ref bool value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      return PlayerSettings.GetPropertyOptionalBool(name, ref value, target);
    }

    [ExcludeFromDocs]
    internal static void InitializePropertyBool(string name, bool value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      PlayerSettings.InitializePropertyBool(name, value, target);
    }

    internal static void InitializePropertyBool(string name, bool value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      PlayerSettings.InitializePropertyBoolInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name), value);
    }

    [ExcludeFromDocs]
    public static void SetPropertyString(string name, string value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      PlayerSettings.SetPropertyString(name, value, target);
    }

    /// <summary>
    ///   <para>Sets a PlayerSettings named string property.</para>
    /// </summary>
    /// <param name="name">Name of the property.</param>
    /// <param name="value">Value of the property (string).</param>
    /// <param name="target">BuildTarget for which the property should apply (use default value BuildTargetGroup.Unknown to apply to all targets).</param>
    public static void SetPropertyString(string name, string value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      PlayerSettings.SetPropertyStringInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name), value);
    }

    public static void SetPropertyString(string name, string value, BuildTarget target)
    {
      PlayerSettings.SetPropertyString(name, value, BuildPipeline.GetBuildTargetGroup(target));
    }

    [ExcludeFromDocs]
    public static string GetPropertyString(string name)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      return PlayerSettings.GetPropertyString(name, target);
    }

    /// <summary>
    ///   <para>Returns a PlayerSettings named string property (with an optional build target it should apply to).</para>
    /// </summary>
    /// <param name="name">Name of the property.</param>
    /// <param name="target">BuildTarget for which the property should apply (use default value BuildTargetGroup.Unknown to apply to all targets).</param>
    /// <returns>
    ///   <para>The current value of the property.</para>
    /// </returns>
    public static string GetPropertyString(string name, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      return PlayerSettings.GetPropertyStringInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name));
    }

    [ExcludeFromDocs]
    public static bool GetPropertyOptionalString(string name, ref string value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      return PlayerSettings.GetPropertyOptionalString(name, ref value, target);
    }

    public static bool GetPropertyOptionalString(string name, ref string value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      string optionalStringInternal = PlayerSettings.GetPropertyOptionalStringInternal(name, target);
      if (optionalStringInternal == null)
        return false;
      value = optionalStringInternal;
      return true;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetPropertyOptionalStringInternal(string name, BuildTargetGroup target);

    [ExcludeFromDocs]
    internal static void InitializePropertyString(string name, string value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      PlayerSettings.InitializePropertyString(name, value, target);
    }

    internal static void InitializePropertyString(string name, string value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      PlayerSettings.InitializePropertyStringInternal(PlayerSettings.GetPropertyNameForBuildTarget(target, name), value);
    }

    [ExcludeFromDocs]
    internal static void InitializePropertyEnum(string name, object value)
    {
      BuildTargetGroup target = BuildTargetGroup.Unknown;
      PlayerSettings.InitializePropertyEnum(name, value, target);
    }

    internal static void InitializePropertyEnum(string name, object value, [DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
    {
      string nameForBuildTarget = PlayerSettings.GetPropertyNameForBuildTarget(target, name);
      string[] names = Enum.GetNames(value.GetType());
      Array values = Enum.GetValues(value.GetType());
      for (int index = 0; index < names.Length; ++index)
        PlayerSettings.AddEnum("PlayerSettings", nameForBuildTarget, (int) values.GetValue(index), names[index]);
      PlayerSettings.InitializePropertyIntInternal(nameForBuildTarget, (int) value);
    }

    /// <summary>
    ///   <para>Returns whether or not the specified aspect ratio is enabled.</para>
    /// </summary>
    /// <param name="aspectRatio"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool HasAspectRatio(AspectRatio aspectRatio);

    /// <summary>
    ///   <para>Enables the specified aspect ratio.</para>
    /// </summary>
    /// <param name="aspectRatio"></param>
    /// <param name="enable"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetAspectRatio(AspectRatio aspectRatio, bool enable);

    /// <summary>
    ///   <para>Returns the list of assigned icons for the specified platform.</para>
    /// </summary>
    /// <param name="platform"></param>
    public static Texture2D[] GetIconsForTargetGroup(BuildTargetGroup platform)
    {
      Texture2D[] iconsForPlatform = PlayerSettings.GetIconsForPlatform(PlayerSettings.GetPlatformName(platform));
      if (iconsForPlatform.Length == 0)
        return new Texture2D[PlayerSettings.GetIconSizesForTargetGroup(platform).Length];
      return iconsForPlatform;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D[] GetIconsForPlatform(string platform);

    /// <summary>
    ///   <para>Assign a list of icons for the specified platform.</para>
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="icons"></param>
    public static void SetIconsForTargetGroup(BuildTargetGroup platform, Texture2D[] icons)
    {
      PlayerSettings.SetIconsForPlatform(PlayerSettings.GetPlatformName(platform), icons);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetIconsForPlatform(string platform, Texture2D[] icons);

    /// <summary>
    ///   <para>Returns a list of icon sizes for the specified platform.</para>
    /// </summary>
    /// <param name="platform"></param>
    public static int[] GetIconSizesForTargetGroup(BuildTargetGroup platform)
    {
      return PlayerSettings.GetIconWidthsForPlatform(PlayerSettings.GetPlatformName(platform));
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int[] GetIconWidthsForPlatform(string platform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int[] GetIconHeightsForPlatform(string platform);

    internal static string GetPlatformName(BuildTargetGroup targetGroup)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      BuildPlayerWindow.BuildPlatform buildPlatform = BuildPlayerWindow.GetValidPlatforms().Find(new Predicate<BuildPlayerWindow.BuildPlatform>(new PlayerSettings.\u003CGetPlatformName\u003Ec__AnonStorey15()
      {
        targetGroup = targetGroup
      }.\u003C\u003Em__1B));
      if (buildPlatform == null)
        return string.Empty;
      return buildPlatform.name;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D GetIconForPlatformAtSize(string platform, int width, int height);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void GetBatchingForPlatform(BuildTarget platform, out int staticBatching, out int dynamicBatching);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetBatchingForPlatform(BuildTarget platform, int staticBatching, int dynamicBatching);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern GraphicsDeviceType[] GetSupportedGraphicsAPIs(BuildTarget platform);

    /// <summary>
    ///   <para>Get graphics APIs to be used on a build platform.</para>
    /// </summary>
    /// <param name="platform">Platform to get APIs for.</param>
    /// <returns>
    ///   <para>Array of graphics APIs.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern GraphicsDeviceType[] GetGraphicsAPIs(BuildTarget platform);

    /// <summary>
    ///   <para>Set graphics APIs to be used on a build platform.</para>
    /// </summary>
    /// <param name="platform">Platform to set APIs for.</param>
    /// <param name="apis">Array of graphics APIs.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetGraphicsAPIs(BuildTarget platform, GraphicsDeviceType[] apis);

    /// <summary>
    ///   <para>Is a build platform using automatic graphics API choice?</para>
    /// </summary>
    /// <param name="platform">Platform to get the flag for.</param>
    /// <returns>
    ///   <para>Should best available graphics API be used.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetUseDefaultGraphicsAPIs(BuildTarget platform);

    /// <summary>
    ///   <para>Should a build platform use automatic graphics API choice.</para>
    /// </summary>
    /// <param name="platform">Platform to set the flag for.</param>
    /// <param name="automatic">Should best available graphics API be used?</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetUseDefaultGraphicsAPIs(BuildTarget platform, bool automatic);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetTemplateCustomValue(string key, string value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetTemplateCustomValue(string key);

    /// <summary>
    ///   <para>Get user-specified symbols for script compilation for the given build target group.</para>
    /// </summary>
    /// <param name="targetGroup"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetScriptingDefineSymbolsForGroup(BuildTargetGroup targetGroup);

    /// <summary>
    ///   <para>Set user-specified symbols for script compilation for the given build target group.</para>
    /// </summary>
    /// <param name="targetGroup"></param>
    /// <param name="defines"></param>
    public static void SetScriptingDefineSymbolsForGroup(BuildTargetGroup targetGroup, string defines)
    {
      if (!string.IsNullOrEmpty(defines))
        defines = string.Join(";", defines.Split(PlayerSettings.defineSplits, StringSplitOptions.RemoveEmptyEntries));
      PlayerSettings.SetScriptingDefineSymbolsForGroupInternal(targetGroup, defines);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetScriptingDefineSymbolsForGroupInternal(BuildTargetGroup targetGroup, string defines);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetApiCompatibilityInternal(int value);

    public sealed class PSM
    {
    }

    /// <summary>
    ///   <para>Android specific player settings.</para>
    /// </summary>
    public sealed class Android
    {
      /// <summary>
      ///   <para>Disable Depth and Stencil Buffers.</para>
      /// </summary>
      public static extern bool disableDepthAndStencilBuffers { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>24-bit Depth Buffer is used.</para>
      /// </summary>
      [Obsolete("This has been replaced by disableDepthAndStencilBuffers")]
      public static bool use24BitDepthBuffer
      {
        get
        {
          return !PlayerSettings.Android.disableDepthAndStencilBuffers;
        }
        set
        {
        }
      }

      /// <summary>
      ///   <para>Android bundle version code.</para>
      /// </summary>
      public static extern int bundleVersionCode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Minimal Android SDK version.</para>
      /// </summary>
      public static extern AndroidSdkVersions minSdkVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Preferred application install location.</para>
      /// </summary>
      public static extern AndroidPreferredInstallLocation preferredInstallLocation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Force internet permission flag.</para>
      /// </summary>
      public static extern bool forceInternetPermission { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Force SD card permission.</para>
      /// </summary>
      public static extern bool forceSDCardPermission { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Provide a build that is Android TV compatible.</para>
      /// </summary>
      public static extern bool androidTVCompatibility { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Publish the build as a game rather than a regular application. This option affects devices running Android 5.0 Lollipop and later</para>
      /// </summary>
      public static extern bool androidIsGame { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      internal static extern bool androidBannerEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      internal static extern AndroidGamepadSupportLevel androidGamepadSupportLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      internal static extern bool createWallpaper { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Android target device.</para>
      /// </summary>
      public static extern AndroidTargetDevice targetDevice { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Android splash screen scale mode.</para>
      /// </summary>
      public static extern AndroidSplashScreenScale splashScreenScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Android keystore name.</para>
      /// </summary>
      public static extern string keystoreName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Android keystore password.</para>
      /// </summary>
      public static extern string keystorePass { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Android key alias name.</para>
      /// </summary>
      public static extern string keyaliasName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Android key alias password.</para>
      /// </summary>
      public static extern string keyaliasPass { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>License verification flag.</para>
      /// </summary>
      public static extern bool licenseVerification { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      /// <summary>
      ///   <para>Use APK Expansion Files.</para>
      /// </summary>
      public static extern bool useAPKExpansionFiles { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Application should show ActivityIndicator when loading.</para>
      /// </summary>
      public static extern AndroidShowActivityIndicatorOnLoading showActivityIndicatorOnLoading { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern AndroidBanner[] GetAndroidBanners();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern Texture2D GetAndroidBannerForHeight(int height);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern void SetAndroidBanners(Texture2D[] banners);
    }

    public sealed class BlackBerry
    {
      public static extern string deviceAddress { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string devicePassword { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string tokenPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string tokenExpires { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string tokenAuthor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string tokenAuthorId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string cskPassword { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string saveLogPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern bool HasSharedPermissions();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void SetSharedPermissions(bool enable);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern bool HasCameraPermissions();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void SetCameraPermissions(bool enable);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern bool HasGPSPermissions();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void SetGPSPermissions(bool enable);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern bool HasIdentificationPermissions();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void SetIdentificationPermissions(bool enable);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern bool HasMicrophonePermissions();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void SetMicrophonePermissions(bool enable);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern bool HasGamepadSupport();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void SetGamepadSupport(bool enable);
    }

    /// <summary>
    ///   <para>iOS specific player settings.</para>
    /// </summary>
    public sealed class iOS
    {
      /// <summary>
      ///   <para>iOS application display name.</para>
      /// </summary>
      public static extern string applicationDisplayName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The build number of the bundle.</para>
      /// </summary>
      public static extern string buildNumber { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Script calling optimization.</para>
      /// </summary>
      public static extern ScriptCallOptimizationLevel scriptCallOptimization { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Active iOS SDK version used for build.</para>
      /// </summary>
      public static extern iOSSdkVersion sdkVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Deployment minimal version of iOS.</para>
      /// </summary>
      public static extern iOSTargetOSVersion targetOSVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Targeted device.</para>
      /// </summary>
      public static extern iOSTargetDevice targetDevice { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("use Screen.SetResolution at runtime", true)]
      public static extern iOSTargetResolution targetResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Icon is prerendered.</para>
      /// </summary>
      public static extern bool prerenderedIcon { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Application requires persistent WiFi.</para>
      /// </summary>
      public static extern bool requiresPersistentWiFi { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>RequiresFullScreen maps to Apple's plist build setting UIRequiresFullScreen, which is used to opt out of being eligible to participate in Slide Over and Split View for iOS 9.0 multitasking.</para>
      /// </summary>
      public static extern bool requiresFullScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Status bar style.</para>
      /// </summary>
      public static extern iOSStatusBarStyle statusBarStyle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Application should exit when suspended to background.</para>
      /// </summary>
      [Obsolete("use appInBackgroundBehavior")]
      public static bool exitOnSuspend
      {
        get
        {
          return PlayerSettings.iOS.appInBackgroundBehavior == iOSAppInBackgroundBehavior.Exit;
        }
        set
        {
          PlayerSettings.iOS.appInBackgroundBehavior = iOSAppInBackgroundBehavior.Exit;
        }
      }

      public static extern iOSAppInBackgroundBehavior appInBackgroundBehavior { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Should insecure HTTP downloads be allowed?</para>
      /// </summary>
      public static extern bool allowHTTPDownload { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Application should show ActivityIndicator when loading.</para>
      /// </summary>
      public static extern iOSShowActivityIndicatorOnLoading showActivityIndicatorOnLoading { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Determines iPod playing behavior.</para>
      /// </summary>
      public static extern bool overrideIPodMusic { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Indicates whether application will use On Demand Resources (ODR) API.</para>
      /// </summary>
      public static extern bool useOnDemandResources { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern string[] GetAssetBundleVariantsWithDeviceRequirements();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern bool CheckAssetBundleVariantHasDeviceRequirements(string name);

      internal static iOSDeviceRequirementGroup GetDeviceRequirementsForAssetBundleVariant(string name)
      {
        if (!PlayerSettings.iOS.CheckAssetBundleVariantHasDeviceRequirements(name))
          return (iOSDeviceRequirementGroup) null;
        return new iOSDeviceRequirementGroup(name);
      }

      internal static void RemoveDeviceRequirementsForAssetBundleVariant(string name)
      {
        iOSDeviceRequirementGroup assetBundleVariant = PlayerSettings.iOS.GetDeviceRequirementsForAssetBundleVariant(name);
        for (int index = 0; index < assetBundleVariant.count; ++index)
          assetBundleVariant.RemoveAt(0);
      }

      internal static iOSDeviceRequirementGroup AddDeviceRequirementsForAssetBundleVariant(string name)
      {
        return new iOSDeviceRequirementGroup(name);
      }
    }

    /// <summary>
    ///   <para>Nintendo 3DS player settings.</para>
    /// </summary>
    public sealed class Nintendo3DS
    {
      /// <summary>
      ///   <para>Disable depth/stencil buffers, to free up memory.</para>
      /// </summary>
      public static extern bool disableDepthAndStencilBuffers { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Disable sterescopic (3D) view on the upper screen.</para>
      /// </summary>
      public static extern bool disableStereoscopicView { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Enable shared L/R command list, for increased performance with stereoscopic rendering.</para>
      /// </summary>
      public static extern bool enableSharedListOpt { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Enable vsync.</para>
      /// </summary>
      public static extern bool enableVSync { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specify true when using expanded save data.</para>
      /// </summary>
      public static extern bool useExtSaveData { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specify true to enable static memory compression or false to disable it.</para>
      /// </summary>
      public static extern bool compressStaticMem { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specify the expanded save data number using 20 bits.</para>
      /// </summary>
      public static extern string extSaveDataNumber { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specify the stack size of the main thread, in bytes.</para>
      /// </summary>
      public static extern int stackSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The 3DS target platform.</para>
      /// </summary>
      public static extern PlayerSettings.Nintendo3DS.TargetPlatform targetPlatform { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specifies the title region settings.</para>
      /// </summary>
      public static extern PlayerSettings.Nintendo3DS.Region region { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Distribution media size.</para>
      /// </summary>
      public static extern PlayerSettings.Nintendo3DS.MediaSize mediaSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Application Logo Style.</para>
      /// </summary>
      public static extern PlayerSettings.Nintendo3DS.LogoStyle logoStyle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The title of the application.</para>
      /// </summary>
      public static extern string title { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specifies the product code, or the add-on content code.</para>
      /// </summary>
      public static extern string productCode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The unique ID of the application, issued by Nintendo.  (0x00300 -&gt; 0xf7fff)</para>
      /// </summary>
      public static extern string applicationId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Nintendo 3DS target platform.</para>
      /// </summary>
      public enum TargetPlatform
      {
        Nintendo3DS = 1,
        NewNintendo3DS = 2,
      }

      /// <summary>
      ///   <para>Nintendo 3DS Title region.</para>
      /// </summary>
      public enum Region
      {
        Japan = 1,
        America = 2,
        Europe = 3,
        China = 4,
        Korea = 5,
        Taiwan = 6,
        All = 7,
      }

      /// <summary>
      ///   <para>Nintendo 3DS distribution media size.</para>
      /// </summary>
      public enum MediaSize
      {
        _128MB,
        _256MB,
        _512MB,
        _1GB,
        _2GB,
      }

      /// <summary>
      ///   <para>Nintendo 3DS logo style specification.</para>
      /// </summary>
      public enum LogoStyle
      {
        Nintendo,
        Distributed,
        iQue,
        Licensed,
      }
    }

    /// <summary>
    ///   <para>PS3 specific player settings.</para>
    /// </summary>
    public sealed class PS3
    {
      /// <summary>
      ///   <para>Texture to use for PS3 Splash Screen on boot.</para>
      /// </summary>
      public static extern Texture2D ps3SplashScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The amount of video memory (in MB) that is set aside for vertex data allocations. Allocations which do not fit into the area are allocated from system memory.</para>
      /// </summary>
      public static extern int videoMemoryForVertexBuffers { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Amount of video memory (in MB) to use as audio storage.</para>
      /// </summary>
      public static extern int videoMemoryForAudio { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Toggle for verbose memory statistics.</para>
      /// </summary>
      public static extern bool EnableVerboseMemoryStats { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>UseSPUForUmbra</para>
      /// </summary>
      public static extern bool UseSPUForUmbra { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>EnableMoveSupport</para>
      /// </summary>
      public static extern bool EnableMoveSupport { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>DisableDolbyEncoding</para>
      /// </summary>
      public static extern bool DisableDolbyEncoding { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>titleConfigPath</para>
      /// </summary>
      public static extern string titleConfigPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>dlcConfigPath</para>
      /// </summary>
      public static extern string dlcConfigPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>thumbnailPath</para>
      /// </summary>
      public static extern string thumbnailPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>backgroundPath</para>
      /// </summary>
      public static extern string backgroundPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>soundPath</para>
      /// </summary>
      public static extern string soundPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>npTrophyCommId</para>
      /// </summary>
      public static extern string npTrophyCommId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>npAgeRating</para>
      /// </summary>
      public static extern int npAgeRating { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>npCommunicationPassphrase</para>
      /// </summary>
      public static extern string npCommunicationPassphrase { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>npTrophyCommSig</para>
      /// </summary>
      public static extern string npTrophyCommSig { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>npTrophyPackagePath</para>
      /// </summary>
      public static extern string npTrophyPackagePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>bootCheckMaxSaveGameSizeKB</para>
      /// </summary>
      public static extern int bootCheckMaxSaveGameSizeKB { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>TrialMode.</para>
      /// </summary>
      public static extern bool trialMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>saveGameSlots</para>
      /// </summary>
      public static extern int saveGameSlots { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
    }

    public sealed class PS4
    {
      public static extern string npTrophyPackPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int npAgeRating { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string npTitleSecret { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int parentalLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int applicationParameter1 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int applicationParameter2 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int applicationParameter3 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int applicationParameter4 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string passcode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string monoEnv { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool playerPrefsSupport { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string contentID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern PlayerSettings.PS4.PS4AppCategory category { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int appType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string masterVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string appVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern PlayerSettings.PS4.PS4RemotePlayKeyAssignment remotePlayKeyAssignment { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string remotePlayKeyMappingDir { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int playTogetherPlayerCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern PlayerSettings.PS4.PS4EnterButtonAssignment enterButtonAssignment { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string paramSfxPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int videoOutPixelFormat { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int videoOutResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string PronunciationXMLPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string PronunciationSIGPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string BackgroundImagePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string StartupImagePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string SaveDataImagePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string SdkOverride { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string BGMPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string ShareFilePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string ShareOverlayImagePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string PrivacyGuardImagePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string PatchPkgPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string PatchLatestPkgPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string PatchChangeinfoPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string NPtitleDatPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      internal static extern bool useDebugIl2cppLibs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool pnSessions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool pnPresence { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool pnFriends { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool pnGameCustomData { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int downloadDataSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int garlicHeapSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool reprojectionSupport { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool useAudio3dBackend { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int audio3dVirtualSpeakerCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int socialScreenEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool attribUserManagement { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool attribMoveSupport { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool attrib3DSupport { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool attribShareSupport { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool attribExclusiveVR { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool disableAutoHideSplash { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int attribCpuUsage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string[] includedModules { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS4 application category.</para>
      /// </summary>
      public enum PS4AppCategory
      {
        Application,
        Patch,
      }

      /// <summary>
      ///   <para>Remote Play key assignment.</para>
      /// </summary>
      public enum PS4RemotePlayKeyAssignment
      {
        None = -1,
        PatternA = 0,
        PatternB = 1,
        PatternC = 2,
        PatternD = 3,
        PatternE = 4,
        PatternF = 5,
        PatternG = 6,
        PatternH = 7,
      }

      /// <summary>
      ///   <para>PS4 enter button assignment.</para>
      /// </summary>
      public enum PS4EnterButtonAssignment
      {
        CircleButton,
        CrossButton,
      }
    }

    /// <summary>
    ///   <para>PS Vita specific player settings.</para>
    /// </summary>
    public sealed class PSVita
    {
      /// <summary>
      ///   <para>Path specifying wher to copy a trophy pack from.</para>
      /// </summary>
      public static extern string npTrophyPackPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita power mode.</para>
      /// </summary>
      public static extern PlayerSettings.PSVita.PSVitaPowerMode powerMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Aquire PS Vita background music.</para>
      /// </summary>
      public static extern bool acquireBGM { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Support Game Boot Message or Game Joining Presence.</para>
      /// </summary>
      public static extern bool npSupportGBMorGJP { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita TV boot mode.</para>
      /// </summary>
      public static extern PlayerSettings.PSVita.PSVitaTvBootMode tvBootMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita TV Disable Emu flag.</para>
      /// </summary>
      public static extern bool tvDisableEmu { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Indicates that this is an upgradable (trial) type application which can be converted to a full application by purchasing an upgrade.</para>
      /// </summary>
      public static extern bool upgradable { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specifies whether or not a health warning will be added to the software manual.</para>
      /// </summary>
      public static extern bool healthWarning { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Support for the PS Vita location library was removed by SCE in SDK 3.570.</para>
      /// </summary>
      [Obsolete("useLibLocation has no effect as of SDK 3.570")]
      public static extern bool useLibLocation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specifies whether or not to show the PS Vita information bar when the application starts.</para>
      /// </summary>
      public static extern bool infoBarOnStartup { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specifies the color of the PS Vita information bar, true = white, false = black.</para>
      /// </summary>
      public static extern bool infoBarColor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      internal static extern bool useDebugIl2cppLibs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Specifies whether circle or cross will be used as the default enter button.</para>
      /// </summary>
      public static extern PlayerSettings.PSVita.PSVitaEnterButtonAssignment enterButtonAssignment { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Save data quota.</para>
      /// </summary>
      public static extern int saveDataQuota { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita parental level.</para>
      /// </summary>
      public static extern int parentalLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The applications short title.</para>
      /// </summary>
      public static extern string shortTitle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The applications content ID.</para>
      /// </summary>
      public static extern string contentID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The package build category.</para>
      /// </summary>
      public static extern PlayerSettings.PSVita.PSVitaAppCategory category { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita content master version.</para>
      /// </summary>
      public static extern string masterVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The PS Vita application version.</para>
      /// </summary>
      public static extern string appVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Support for the PS Vita twitter dialog was removed by SCE in SDK 3.570.</para>
      /// </summary>
      [Obsolete("AllowTwitterDialog has no effect as of SDK 3.570")]
      public static extern bool AllowTwitterDialog { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PSN Age rating.</para>
      /// </summary>
      public static extern int npAgeRating { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita NP Title Data File.</para>
      /// </summary>
      public static extern string npTitleDatPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita NP Communications ID.</para>
      /// </summary>
      public static extern string npCommunicationsID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita NP Passphrase.</para>
      /// </summary>
      public static extern string npCommsPassphrase { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita NP Signature.</para>
      /// </summary>
      public static extern string npCommsSig { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Path specifying where to copy the package parameter file (param.sfx) from.</para>
      /// </summary>
      public static extern string paramSfxPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita sofware manual.</para>
      /// </summary>
      public static extern string manualPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita Live area gate image.</para>
      /// </summary>
      public static extern string liveAreaGatePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita Live area background image.</para>
      /// </summary>
      public static extern string liveAreaBackroundPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita Live area path.</para>
      /// </summary>
      public static extern string liveAreaPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita Live area trial path.</para>
      /// </summary>
      public static extern string liveAreaTrialPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>For cumlative patch packages.</para>
      /// </summary>
      public static extern string patchChangeInfoPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>For building cumulative patch packages.</para>
      /// </summary>
      public static extern string patchOriginalPackage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>32 character password for use if you want to access the contents of a package.</para>
      /// </summary>
      public static extern string packagePassword { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Keystone file.</para>
      /// </summary>
      public static extern string keystoneFile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita memory expansion mode.</para>
      /// </summary>
      public static extern PlayerSettings.PSVita.PSVitaMemoryExpansionMode memoryExpansionMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita DRM Type.</para>
      /// </summary>
      public static extern PlayerSettings.PSVita.PSVitaDRMType drmType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>PS Vita media type.</para>
      /// </summary>
      public static extern int storageType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Should always = 01.00.</para>
      /// </summary>
      public static extern int mediaCapacity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Power mode enum.</para>
      /// </summary>
      public enum PSVitaPowerMode
      {
        ModeA,
        ModeB,
        ModeC,
      }

      /// <summary>
      ///   <para>PS Vita TV boot mode enum.</para>
      /// </summary>
      public enum PSVitaTvBootMode
      {
        Default,
        PSVitaBootablePSVitaTvBootable,
        PSVitaBootablePSVitaTvNotBootable,
      }

      /// <summary>
      ///   <para>Enter button assignment enum.</para>
      /// </summary>
      public enum PSVitaEnterButtonAssignment
      {
        Default,
        CircleButton,
        CrossButton,
      }

      /// <summary>
      ///   <para>Application package category enum.</para>
      /// </summary>
      public enum PSVitaAppCategory
      {
        Application,
        ApplicationPatch,
      }

      /// <summary>
      ///   <para>Memory expansion mode enum.</para>
      /// </summary>
      public enum PSVitaMemoryExpansionMode
      {
        None,
        ExpandBy29MB,
        ExpandBy77MB,
        ExpandBy109MB,
      }

      /// <summary>
      ///   <para>DRM type enum.</para>
      /// </summary>
      public enum PSVitaDRMType
      {
        PaidFor,
        Free,
      }
    }

    /// <summary>
    ///   <para>Samsung Smart TV specific Player Settings.</para>
    /// </summary>
    public sealed class SamsungTV
    {
      /// <summary>
      ///   <para>The address used when accessing the device.</para>
      /// </summary>
      public static extern string deviceAddress { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The description of the created product.</para>
      /// </summary>
      public static extern string productDescription { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Author of the created product.</para>
      /// </summary>
      public static extern string productAuthor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Product author's e-mail.</para>
      /// </summary>
      public static extern string productAuthorEmail { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The author's website link.</para>
      /// </summary>
      public static extern string productLink { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The category of the created product.</para>
      /// </summary>
      public static extern PlayerSettings.SamsungTV.SamsungTVProductCategories productCategory { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Types of available product categories.</para>
      /// </summary>
      public enum SamsungTVProductCategories
      {
        Games,
        Videos,
        Sports,
        Lifestyle,
        Information,
        Education,
        Kids,
      }
    }

    /// <summary>
    ///   <para>Tizen application capabilities.</para>
    /// </summary>
    public enum TizenCapability
    {
      Location,
      DataSharing,
      NetworkGet,
      WifiDirect,
      CallHistoryRead,
      Power,
      ContactWrite,
      MessageWrite,
      ContentWrite,
      Push,
      AccountRead,
      ExternalStorage,
      Recorder,
      PackageManagerInfo,
      NFCCardEmulation,
      CalendarWrite,
      WindowPrioritySet,
      VolumeSet,
      CallHistoryWrite,
      AlarmSet,
      Call,
      Email,
      ContactRead,
      Shortcut,
      KeyManager,
      LED,
      NetworkProfile,
      AlarmGet,
      Display,
      CalendarRead,
      NFC,
      AccountWrite,
      Bluetooth,
      Notification,
      NetworkSet,
      ExternalStorageAppData,
      Download,
      Telephony,
      MessageRead,
      MediaStorage,
      Internet,
      Camera,
      Haptic,
      AppManagerLaunch,
      SystemSettings,
    }

    /// <summary>
    ///   <para>Tizen specific player settings.</para>
    /// </summary>
    public sealed class Tizen
    {
      /// <summary>
      ///   <para>Description of your project to be displayed in the Tizen Store.</para>
      /// </summary>
      public static extern string productDescription { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>URL of your project to be displayed in the Tizen Store.</para>
      /// </summary>
      public static extern string productURL { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Name of the security profile to code sign Tizen applications with.</para>
      /// </summary>
      public static extern string signingProfileName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static void SetCapability(PlayerSettings.TizenCapability capability, bool value)
      {
        PlayerSettings.Tizen.InternalSetCapability(capability.ToString(), value.ToString());
      }

      public static bool GetCapability(PlayerSettings.TizenCapability capability)
      {
        string capability1 = PlayerSettings.Tizen.InternalGetCapability(capability.ToString());
        if (string.IsNullOrEmpty(capability1))
          return false;
        try
        {
          return (bool) TypeDescriptor.GetConverter(typeof (bool)).ConvertFromString(capability1);
        }
        catch
        {
          Debug.LogError((object) ("Failed to parse value  ('" + capability.ToString() + "," + capability1 + "') to bool type."));
          return false;
        }
      }

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void InternalSetCapability(string name, string value);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern string InternalGetCapability(string name);
    }

    /// <summary>
    ///   <para>tvOS specific player settings.</para>
    /// </summary>
    public sealed class tvOS
    {
      /// <summary>
      ///   <para>Active tvOS SDK version used for build.</para>
      /// </summary>
      public static extern tvOSSdkVersion sdkVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Deployment minimal version of tvOS.</para>
      /// </summary>
      public static extern tvOSTargetOSVersion targetOSVersion { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern Texture2D[] GetSmallIconLayers();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern Texture2D[] GetLargeIconLayers();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern Texture2D[] GetTopShelfImageLayers();

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern void SetSmallIconLayers(Texture2D[] layers);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern void SetLargeIconLayers(Texture2D[] layers);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      internal static extern void SetTopShelfImageLayers(Texture2D[] layers);
    }

    public sealed class WiiU
    {
      public static extern string titleID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string groupID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int commonSaveSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int accountSaveSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string olvAccessKey { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string tinCode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string joinGameId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string joinGameModeMask { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int commonBossSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int accountBossSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string[] addOnUniqueIDs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool supportsNunchuk { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool supportsClassicController { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool supportsBalanceBoard { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool supportsMotionPlus { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool supportsProController { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool allowScreenCapture { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int controllerCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int mainThreadStackSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int loaderThreadStackSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern int systemHeapSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern WiiUTVResolution tvResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern Texture2D tvStartupScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      public static extern Texture2D gamePadStartupScreen { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      public static extern int gamePadMSAA { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string profilerLibraryPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool drcBufferDisabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
    }

    public enum WSAApplicationShowName
    {
      NotSet,
      AllLogos,
      NoLogos,
      StandardLogoOnly,
      WideLogoOnly,
    }

    public enum WSADefaultTileSize
    {
      NotSet,
      Medium,
      Wide,
    }

    public enum WSAApplicationForegroundText
    {
      Light = 1,
      Dark = 2,
    }

    /// <summary>
    ///   <para>Compilation overrides for C# files.</para>
    /// </summary>
    public enum WSACompilationOverrides
    {
      None,
      UseNetCore,
      UseNetCorePartially,
    }

    public enum WSACapability
    {
      EnterpriseAuthentication,
      InternetClient,
      InternetClientServer,
      MusicLibrary,
      PicturesLibrary,
      PrivateNetworkClientServer,
      RemovableStorage,
      SharedUserCertificates,
      VideosLibrary,
      WebCam,
      Proximity,
      Microphone,
      Location,
      HumanInterfaceDevice,
      AllJoyn,
      BlockedChatMessages,
      Chat,
      CodeGeneration,
      Objects3D,
      PhoneCall,
      UserAccountInformation,
      VoipCall,
      Bluetooth,
    }

    /// <summary>
    ///   <para>Various image scales, supported by Windows Store Apps.</para>
    /// </summary>
    public enum WSAImageScale
    {
      Target16 = 16,
      Target24 = 24,
      Target48 = 48,
      _80 = 80,
      _100 = 100,
      _125 = 125,
      _140 = 140,
      _150 = 150,
      _180 = 180,
      _200 = 200,
      _240 = 240,
      Target256 = 256,
      _400 = 400,
    }

    /// <summary>
    ///   <para>Image types, supported by Windows Store Apps.</para>
    /// </summary>
    public enum WSAImageType
    {
      PackageLogo = 1,
      SplashScreenImage = 2,
      StoreTileLogo = 11,
      StoreTileWideLogo = 12,
      StoreTileSmallLogo = 13,
      StoreSmallTile = 14,
      StoreLargeTile = 15,
      PhoneAppIcon = 21,
      PhoneSmallTile = 22,
      PhoneMediumTile = 23,
      PhoneWideTile = 24,
      PhoneSplashScreen = 25,
      UWPSquare44x44Logo = 31,
      UWPSquare71x71Logo = 32,
      UWPSquare150x150Logo = 33,
      UWPSquare310x310Logo = 34,
      UWPWide310x150Logo = 35,
    }

    public sealed class WSA
    {
      public static extern string packageName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string packageLogo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string packageLogo140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string packageLogo180 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string packageLogo240 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      private static extern string packageVersionRaw { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string commandLineArgsFile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string certificatePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      internal static extern string certificatePassword { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      public static extern string certificateSubject { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      public static extern string certificateIssuer { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      private static extern long certificateNotAfterRaw { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      public static extern string applicationDescription { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileLogo80 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileLogo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileLogo140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileLogo180 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileWideLogo80 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileWideLogo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileWideLogo140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileWideLogo180 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileSmallLogo80 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileSmallLogo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileSmallLogo140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeTileSmallLogo180 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeSmallTile80 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeSmallTile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeSmallTile140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeSmallTile180 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeLargeTile80 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeLargeTile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeLargeTile140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeLargeTile180 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeSplashScreenImage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeSplashScreenImageScale140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string storeSplashScreenImageScale180 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneAppIcon { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneAppIcon140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneAppIcon240 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneSmallTile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneSmallTile140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneSmallTile240 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneMediumTile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneMediumTile140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneMediumTile240 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneWideTile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneWideTile140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneWideTile240 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneSplashScreenImage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneSplashScreenImageScale140 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      [Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()")]
      public static extern string phoneSplashScreenImageScale240 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern string tileShortName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern PlayerSettings.WSAApplicationShowName tileShowName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool mediumTileShowName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool largeTileShowName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool wideTileShowName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern PlayerSettings.WSADefaultTileSize defaultTileSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern PlayerSettings.WSACompilationOverrides compilationOverrides { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern PlayerSettings.WSAApplicationForegroundText tileForegroundText { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static Color tileBackgroundColor
      {
        get
        {
          Color color;
          PlayerSettings.WSA.INTERNAL_get_tileBackgroundColor(out color);
          return color;
        }
        set
        {
          PlayerSettings.WSA.INTERNAL_set_tileBackgroundColor(ref value);
        }
      }

      public static extern bool enableIndependentInputSource { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      public static extern bool enableLowLatencyPresentationAPI { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      private static extern bool splashScreenUseBackgroundColor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      private static Color splashScreenBackgroundColorRaw
      {
        get
        {
          Color color;
          PlayerSettings.WSA.INTERNAL_get_splashScreenBackgroundColorRaw(out color);
          return color;
        }
        set
        {
          PlayerSettings.WSA.INTERNAL_set_splashScreenBackgroundColorRaw(ref value);
        }
      }

      public static Version packageVersion
      {
        get
        {
          try
          {
            return new Version(PlayerSettings.WSA.ValidatePackageVersion(PlayerSettings.WSA.packageVersionRaw));
          }
          catch (Exception ex)
          {
            throw new Exception(string.Format("{0}, the raw string was {1}", (object) ex.Message, (object) PlayerSettings.WSA.packageVersionRaw));
          }
        }
        set
        {
          PlayerSettings.WSA.packageVersionRaw = value.ToString();
        }
      }

      public static DateTime? certificateNotAfter
      {
        get
        {
          long certificateNotAfterRaw = PlayerSettings.WSA.certificateNotAfterRaw;
          if (certificateNotAfterRaw != 0L)
            return new DateTime?(DateTime.FromFileTime(certificateNotAfterRaw));
          return new DateTime?();
        }
      }

      public static Color? splashScreenBackgroundColor
      {
        get
        {
          if (PlayerSettings.WSA.splashScreenUseBackgroundColor)
            return new Color?(PlayerSettings.WSA.splashScreenBackgroundColorRaw);
          return new Color?();
        }
        set
        {
          PlayerSettings.WSA.splashScreenUseBackgroundColor = value.HasValue;
          if (!value.HasValue)
            return;
          PlayerSettings.WSA.splashScreenBackgroundColorRaw = value.Value;
        }
      }

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern string GetWSAImage(PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void SetWSAImage(string image, PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern bool SetCertificate(string path, string password);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void INTERNAL_get_tileBackgroundColor(out Color value);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void INTERNAL_set_tileBackgroundColor(ref Color value);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void INTERNAL_get_splashScreenBackgroundColorRaw(out Color value);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void INTERNAL_set_splashScreenBackgroundColorRaw(ref Color value);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern void InternalSetCapability(string name, string value);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      private static extern string InternalGetCapability(string name);

      internal static string ValidatePackageVersion(string value)
      {
        if (new Regex("^(\\d+)\\.(\\d+)\\.(\\d+)\\.(\\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant).IsMatch(value))
          return value;
        return "1.0.0.0";
      }

      private static void ValidateWSAImageType(PlayerSettings.WSAImageType type)
      {
        PlayerSettings.WSAImageType wsaImageType = type;
        switch (wsaImageType)
        {
          case PlayerSettings.WSAImageType.StoreTileLogo:
            break;
          case PlayerSettings.WSAImageType.StoreTileWideLogo:
            break;
          case PlayerSettings.WSAImageType.StoreTileSmallLogo:
            break;
          case PlayerSettings.WSAImageType.StoreSmallTile:
            break;
          case PlayerSettings.WSAImageType.StoreLargeTile:
            break;
          case PlayerSettings.WSAImageType.PhoneAppIcon:
            break;
          case PlayerSettings.WSAImageType.PhoneSmallTile:
            break;
          case PlayerSettings.WSAImageType.PhoneMediumTile:
            break;
          case PlayerSettings.WSAImageType.PhoneWideTile:
            break;
          case PlayerSettings.WSAImageType.PhoneSplashScreen:
            break;
          case PlayerSettings.WSAImageType.UWPSquare44x44Logo:
            break;
          case PlayerSettings.WSAImageType.UWPSquare71x71Logo:
            break;
          case PlayerSettings.WSAImageType.UWPSquare150x150Logo:
            break;
          case PlayerSettings.WSAImageType.UWPSquare310x310Logo:
            break;
          case PlayerSettings.WSAImageType.UWPWide310x150Logo:
            break;
          default:
            if (wsaImageType == PlayerSettings.WSAImageType.PackageLogo || wsaImageType == PlayerSettings.WSAImageType.SplashScreenImage)
              break;
            throw new Exception("Unknown WSA image type: " + (object) type);
        }
      }

      private static void ValidateWSAImageScale(PlayerSettings.WSAImageScale scale)
      {
        switch (scale)
        {
          case PlayerSettings.WSAImageScale.Target16:
            break;
          case PlayerSettings.WSAImageScale.Target24:
            break;
          case PlayerSettings.WSAImageScale.Target48:
            break;
          case PlayerSettings.WSAImageScale._80:
            break;
          case PlayerSettings.WSAImageScale._100:
            break;
          case PlayerSettings.WSAImageScale._125:
            break;
          case PlayerSettings.WSAImageScale._140:
            break;
          case PlayerSettings.WSAImageScale._150:
            break;
          case PlayerSettings.WSAImageScale._180:
            break;
          case PlayerSettings.WSAImageScale._200:
            break;
          case PlayerSettings.WSAImageScale._240:
            break;
          case PlayerSettings.WSAImageScale.Target256:
            break;
          case PlayerSettings.WSAImageScale._400:
            break;
          default:
            throw new Exception("Unknown image scale: " + (object) scale);
        }
      }

      public static string GetVisualAssetsImage(PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale)
      {
        PlayerSettings.WSA.ValidateWSAImageType(type);
        PlayerSettings.WSA.ValidateWSAImageScale(scale);
        return PlayerSettings.WSA.GetWSAImage(type, scale);
      }

      public static void SetVisualAssetsImage(string image, PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale)
      {
        PlayerSettings.WSA.ValidateWSAImageType(type);
        PlayerSettings.WSA.ValidateWSAImageScale(scale);
        PlayerSettings.WSA.SetWSAImage(image, type, scale);
      }

      public static void SetCapability(PlayerSettings.WSACapability capability, bool value)
      {
        PlayerSettings.WSA.InternalSetCapability(capability.ToString(), value.ToString());
      }

      public static bool GetCapability(PlayerSettings.WSACapability capability)
      {
        string capability1 = PlayerSettings.WSA.InternalGetCapability(capability.ToString());
        if (string.IsNullOrEmpty(capability1))
          return false;
        try
        {
          return (bool) TypeDescriptor.GetConverter(typeof (bool)).ConvertFromString(capability1);
        }
        catch
        {
          Debug.LogError((object) ("Failed to parse value  ('" + capability.ToString() + "," + capability1 + "') to bool type."));
          return false;
        }
      }
    }

    /// <summary>
    ///   <para>Xbox One Specific Player Settings.</para>
    /// </summary>
    public sealed class XboxOne
    {
      /// <summary>
      ///   <para>Xbox One Product ID to use when building a streaming install package.</para>
      /// </summary>
      public static extern string ProductId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Xbox One makepkg.exe option. Specifies the update key required when building game updates.</para>
      /// </summary>
      public static extern string UpdateKey { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Xbox One makepkg option. Specifies the Sandbox ID to be used when building a streaming install package.</para>
      /// </summary>
      public static extern string SandboxId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Xbox One Content ID to be used in constructing game package.</para>
      /// </summary>
      public static extern string ContentId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The TitleID uniquely identifying your title to Xbox Live services.</para>
      /// </summary>
      public static extern string TitleId { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The service configuration ID for your title.</para>
      /// </summary>
      public static extern string SCID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Turns on notifications that you are gaining/losing GPU resources.</para>
      /// </summary>
      public static extern bool EnableVariableGPU { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Disabling the kinect frees up additional GPU resources for use.</para>
      /// </summary>
      public static extern bool DisableKinectGpuReservation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Enable sampling profiler in development builds to provide better profile markers for PIX, including Start and Update markers for script behaviors.</para>
      /// </summary>
      public static extern bool EnablePIXSampling { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Turns on the ability for the render thread to service the normal general purpose job queue while waiting for rendering related work. This is considered an advanced feature and users should only enable if they have a good understanding of their pix profile.</para>
      /// </summary>
      public static extern bool EnableRenderThreadRunsJobs { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>(optional override) Location of Xbox One Game OS image file to link into ERA package being created.</para>
      /// </summary>
      public static extern string GameOsOverridePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Xbox One optional parameter that causes the makepkg process to use your specified layout file rather than generating one for you. Required if working with asset bundles.</para>
      /// </summary>
      public static extern string PackagingOverridePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Xbox One optional parameter that causes the makepkg process to encrypt the package for performance testing or with final retail encryption.</para>
      /// </summary>
      public static extern XboxOneEncryptionLevel PackagingEncryption { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>The update granularity the package will be built with.</para>
      /// </summary>
      public static extern XboxOnePackageUpdateGranularity PackageUpdateGranularity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Xbox One optional parameter that lets you use a specified Manifest file rather than generated for you.</para>
      /// </summary>
      public static extern string AppManifestOverridePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Indicates if the game is a standalone game or a content package to an existing game.</para>
      /// </summary>
      public static extern bool IsContentPackage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Xbox One Version Identifier used in the Application Manifest.</para>
      /// </summary>
      public static extern string Version { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>A friendly description that can be displayed to users.</para>
      /// </summary>
      public static extern string Description { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Get the names of the socket descriptions for the project.</para>
      /// </summary>
      public static extern string[] SocketNames { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      /// <summary>
      ///   <para>Get the list of projects that can load this content package.  This setting is only available for content packages.</para>
      /// </summary>
      public static extern string[] AllowedProductIds { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      /// <summary>
      ///   <para>Sets the size of the persistent local storage.</para>
      /// </summary>
      public static extern uint PersistentLocalStorageSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>This option controls the mono trace log in XboxOne builds. This is a very verbose and expensive tool that can be used to help with debugging a potential mono issue.</para>
      /// </summary>
      public static extern int monoLoggingLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

      /// <summary>
      ///   <para>Mark a specific capability as enabled or enabled in the appxmanifest.xml file.  Please see the XDK whitepaper titled "Xbox One Submission Validator" for valid capability names.</para>
      /// </summary>
      /// <param name="capability">The name of the capability to set.</param>
      /// <param name="value">Whether or not to enable the capability.</param>
      public static void SetCapability(string capability, bool value)
      {
        PlayerSettings.SetPropertyBool(capability, value, BuildTargetGroup.XboxOne);
      }

      /// <summary>
      ///   <para>Returns a bool indicating if the given capability is enabled or not.  Please see the XDK whitepaper titled "Xbox One Submission Validator" for valid capability names.</para>
      /// </summary>
      /// <param name="capability"></param>
      public static bool GetCapability(string capability)
      {
        try
        {
          bool flag = false;
          PlayerSettings.GetPropertyOptionalBool(capability, ref flag, BuildTargetGroup.XboxOne);
          return flag;
        }
        catch
        {
          return false;
        }
      }

      internal static void SetSupportedLanguage(string language, bool enabled)
      {
        PlayerSettings.SetPropertyBool(language, enabled, BuildTargetGroup.XboxOne);
      }

      internal static bool GetSupportedLanguage(string language)
      {
        try
        {
          bool flag = false;
          PlayerSettings.GetPropertyOptionalBool(language, ref flag, BuildTargetGroup.XboxOne);
          return flag;
        }
        catch
        {
          return false;
        }
      }

      /// <summary>
      ///   <para>Remove the socket description with the given name.</para>
      /// </summary>
      /// <param name="name"></param>
      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void RemoveSocketDefinition(string name);

      /// <summary>
      ///   <para>Set the values for the socket description with the given name.</para>
      /// </summary>
      /// <param name="name">The name of the socket description.</param>
      /// <param name="port">The port or port range the socket can use.</param>
      /// <param name="protocol">The protocol the socket uses.</param>
      /// <param name="usages">The allowed usage flags for this socket description.</param>
      /// <param name="templateName">The name of the device association template.</param>
      /// <param name="sessionRequirment">Mutiplayer requirement setting for the device association template.</param>
      /// <param name="deviceUsages">The allowed usage flags for the device association template.</param>
      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void SetSocketDefinition(string name, string port, int protocol, int[] usages, string templateName, int sessionRequirment, int[] deviceUsages);

      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void GetSocketDefinition(string name, out string port, out int protocol, out int[] usages, out string templateName, out int sessionRequirment, out int[] deviceUsages);

      /// <summary>
      ///   <para>Remove a ProductId from the list of products that can load the content package created from your project.  This setting is only available for content packages.</para>
      /// </summary>
      /// <param name="id"></param>
      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void RemoveAllowedProductId(string id);

      /// <summary>
      ///   <para>Add a ProductId to the list of products that can load the content package created from your project.  This setting is only available for content packages.</para>
      /// </summary>
      /// <param name="id"></param>
      /// <returns>
      ///   <para>Returns false if the product Id was already in the allowed list.</para>
      /// </returns>
      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern bool AddAllowedProductId(string id);

      /// <summary>
      ///   <para>Update the value at the given index in the list of products that can load this content package.  You can use the PlayerSettings.XboxOne.AllowedProductIds property to get the existing productIds and determine their indexes.</para>
      /// </summary>
      /// <param name="idx"></param>
      /// <param name="id"></param>
      [WrapperlessIcall]
      [MethodImpl(MethodImplOptions.InternalCall)]
      public static extern void UpdateAllowedProductId(int idx, string id);

      internal static void InitializeGameRating(string name, int value)
      {
        PlayerSettings.InitializePropertyInt(name, value, BuildTargetGroup.XboxOne);
      }

      /// <summary>
      ///   <para>Set the rating value that is specified in the appxmanifest.xml file.</para>
      /// </summary>
      /// <param name="name">The name of the ratings board that you are setting the rating value for.</param>
      /// <param name="value">The new rating level.  The meaning of the value depends on the name of the ratings board.  The value corresponds to the entries the rating board's drop down menu, top most entry being 0 and each item lower in the list being 1 higher than the previous.</param>
      public static void SetGameRating(string name, int value)
      {
        PlayerSettings.SetPropertyInt(name, value, BuildTargetGroup.XboxOne);
      }

      /// <summary>
      ///   <para>Get the rating value that is specified in the appxmanifest.xml file for the given ratings board.</para>
      /// </summary>
      /// <param name="name">The name of the ratings board that you want the rating value for.</param>
      /// <returns>
      ///   <para>The current rating level.  The meaning of the value depends on the name of the ratings board.  The value corresponds to the entries the rating board's drop down menu, top most entry being 0 and each item lower in the list being 1 higher than the previous.</para>
      /// </returns>
      public static int GetGameRating(string name)
      {
        try
        {
          int num = 0;
          PlayerSettings.GetPropertyOptionalInt(name, ref num, BuildTargetGroup.XboxOne);
          return num;
        }
        catch
        {
          return 0;
        }
      }
    }
  }
}
