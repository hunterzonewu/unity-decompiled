// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorUserBuildSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>User build settings for the Editor</para>
  /// </summary>
  public sealed class EditorUserBuildSettings
  {
    /// <summary>
    ///   <para>Triggered in response to SwitchActiveBuildTarget.</para>
    /// </summary>
    public static System.Action activeBuildTargetChanged;

    /// <summary>
    ///   <para>The currently selected build target group.</para>
    /// </summary>
    public static extern BuildTargetGroup selectedBuildTargetGroup { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The currently selected target for a standalone build.</para>
    /// </summary>
    public static extern BuildTarget selectedStandaloneTarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>PSM Build Subtarget.</para>
    /// </summary>
    public static extern PSMBuildSubtarget psmBuildSubtarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>PS Vita Build subtarget.</para>
    /// </summary>
    public static extern PSP2BuildSubtarget psp2BuildSubtarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>PS4 Build Subtarget.</para>
    /// </summary>
    public static extern PS4BuildSubtarget ps4BuildSubtarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>SCE Build subtarget.</para>
    /// </summary>
    public static extern SCEBuildSubtarget sceBuildSubtarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Are null references actively checked?</para>
    /// </summary>
    public static extern bool explicitNullChecks { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Build submission materials.</para>
    /// </summary>
    public static extern bool needSubmissionMaterials { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Build data compressed with PSArc.</para>
    /// </summary>
    public static extern bool compressWithPsArc { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Force installation of package, even if error.</para>
    /// </summary>
    public static extern bool forceInstallation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables a Linux headless build.</para>
    /// </summary>
    public static extern bool enableHeadlessMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is build script only enabled.</para>
    /// </summary>
    public static extern bool buildScriptsOnly { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Xbox Build subtarget.</para>
    /// </summary>
    public static extern XboxBuildSubtarget xboxBuildSubtarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Wii U player debug level.</para>
    /// </summary>
    public static extern WiiUBuildDebugLevel wiiUBuildDebugLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Built player postprocessing options.</para>
    /// </summary>
    public static extern WiiUBuildOutput wiiuBuildOutput { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable network API.</para>
    /// </summary>
    public static extern bool wiiUEnableNetAPI { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Boot mode of a devkit.</para>
    /// </summary>
    public static extern int wiiUBootMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Selected Xbox Run Method.</para>
    /// </summary>
    public static extern XboxRunMethod xboxRunMethod { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>When building an Xbox One Streaming Install package (makepkg.exe) The layout generation code in Unity will assign each scene and associated assets to individual chunks. Unity will mark scene 0 as being part of the launch range, IE the set of chunks required to launch the game, you may include additional scenes in this launch range if you desire, this specifies a range of scenes (starting at 0) to be included in the launch set. </para>
    /// </summary>
    public static extern int streamingInstallLaunchRange { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The currently selected Xbox One Deploy Method.</para>
    /// </summary>
    public static extern XboxOneDeployMethod xboxOneDeployMethod { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Windows account username associated with PC share folder.</para>
    /// </summary>
    public static extern string xboxOneUsername { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>Network shared folder path e.g.
    /// MYCOMPUTER\SHAREDFOLDER\.</para>
    ///       </summary>
    public static extern string xboxOneNetworkSharePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static string xboxOneAdditionalDebugPorts { get; set; }

    /// <summary>
    ///   <para>Android platform options.</para>
    /// </summary>
    public static extern MobileTextureSubtarget androidBuildSubtarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Target Windows SDK.</para>
    /// </summary>
    public static extern WSASDK wsaSDK { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern WSAUWPBuildType wsaUWPBuildType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern WSABuildAndRunDeployTarget wsaBuildAndRunDeployTarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate and reference C# projects from your main solution.</para>
    /// </summary>
    public static extern bool wsaGenerateReferenceProjects { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The texture compression type to be used when building.</para>
    /// </summary>
    public static extern MobileTextureSubtarget blackberryBuildSubtarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The build type to be used.</para>
    /// </summary>
    public static extern BlackBerryBuildType blackberryBuildType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The texture compression type to be used when building.</para>
    /// </summary>
    public static extern MobileTextureSubtarget tizenBuildSubtarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Select the streaming option for a webplayer build.</para>
    /// </summary>
    public static extern bool webPlayerStreamed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Build the webplayer along with the UnityObject.js file (so it doesn't need to be downloaded).</para>
    /// </summary>
    public static extern bool webPlayerOfflineDeployment { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The currently active build target.</para>
    /// </summary>
    public static extern BuildTarget activeBuildTarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>DEFINE directives for the compiler.</para>
    /// </summary>
    public static extern string[] activeScriptCompilationDefines { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Enables a development build.</para>
    /// </summary>
    public static extern bool development { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Optimization level for WebGL.</para>
    /// </summary>
    public static extern int webGLOptimizationLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Start the player with a connection to the profiler.</para>
    /// </summary>
    public static extern bool connectProfiler { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable source-level debuggers to connect.</para>
    /// </summary>
    public static extern bool allowDebugging { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Export Android Project for use wih Android Studio or Eclipse.</para>
    /// </summary>
    public static extern bool exportAsGoogleAndroidProject { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Symlink runtime libraries with an iOS Xcode project.</para>
    /// </summary>
    public static extern bool symlinkLibraries { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool symlinkTrampoline { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern iOSBuildType iOSBuildConfigType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Create a .cia "download image" for deploying to test kits (3DS).</para>
    /// </summary>
    public static extern bool n3dsCreateCIAFile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Place the built player in the build folder.</para>
    /// </summary>
    public static extern bool installInBuildFolder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Force full optimizations for script complilation in Development builds.</para>
    /// </summary>
    public static extern bool forceOptimizeScriptCompilation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>Enables or Disables .NET Native for specific build configuration.
    /// More information - https:msdn.microsoft.comen-uslibrary/dn584397(v=vs.110).aspx.</para>
    ///       </summary>
    /// <param name="config">Build configuration.</param>
    /// <param name="enabled">Is enabled?</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetWSADotNetNative(WSABuildType config, bool enabled);

    /// <summary>
    ///         <para>Is .NET Native enabled for specific build configuration.
    /// More information - https:msdn.microsoft.comen-uslibrary/dn584397(v=vs.110).aspx.</para>
    ///       </summary>
    /// <param name="config">Build configuration.</param>
    /// <returns>
    ///   <para>True if .NET Native is enabled for the specific build configuration.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetWSADotNetNative(WSABuildType config);

    /// <summary>
    ///   <para>Select a new build target to be active.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <returns>
    ///   <para>True if the build target was successfully switched, false otherwise (for example, if license checks fail, files are missing, or if the user has cancelled the operation via the UI).</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SwitchActiveBuildTarget(BuildTarget target);

    internal static void Internal_ActiveBuildTargetChanged()
    {
      if (EditorUserBuildSettings.activeBuildTargetChanged == null)
        return;
      EditorUserBuildSettings.activeBuildTargetChanged();
    }

    /// <summary>
    ///   <para>Get the current location for the build.</para>
    /// </summary>
    /// <param name="target"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetBuildLocation(BuildTarget target);

    /// <summary>
    ///   <para>Set a new location for the build.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="location"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetBuildLocation(BuildTarget target, string location);
  }
}
