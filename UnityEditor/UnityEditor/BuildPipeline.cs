// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildPipeline
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Lets you programmatically build players or AssetBundles which can be loaded from the web.</para>
  /// </summary>
  public sealed class BuildPipeline
  {
    /// <summary>
    ///   <para>Is a player currently being built?</para>
    /// </summary>
    public static extern bool isBuildingPlayer { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern BuildTargetGroup GetBuildTargetGroup(BuildTarget platform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern BuildTargetGroup GetBuildTargetGroupByName(string platform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern BuildTarget GetBuildTargetByName(string platform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetBuildTargetGroupDisplayName(BuildTargetGroup targetPlatformGroup);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetBuildTargetName(BuildTarget targetPlatform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetEditorTargetName();

    /// <summary>
    ///   <para>Lets you manage cross-references and dependencies between different asset bundles and player builds.</para>
    /// </summary>
    [Obsolete("PushAssetDependencies has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void PushAssetDependencies();

    /// <summary>
    ///   <para>Lets you manage cross-references and dependencies between different asset bundles and player builds.</para>
    /// </summary>
    [Obsolete("PopAssetDependencies has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void PopAssetDependencies();

    private static void LogBuildExceptionAndExit(string buildFunctionName, Exception exception)
    {
      Debug.LogErrorFormat("Internal Error in {0}:", (object) buildFunctionName);
      Debug.LogException(exception);
      EditorApplication.Exit(1);
    }

    /// <summary>
    ///   <para>Builds a player.</para>
    /// </summary>
    /// <param name="levels">The scenes to be included in the build. If empty, the currently open scene will be built. Paths are relative to the project folder (AssetsMyLevelsMyScene.unity).</param>
    /// <param name="locationPathName">The path where the application will be built.</param>
    /// <param name="target">The BuildTarget to build.</param>
    /// <param name="options">Additional BuildOptions, like whether to run the built player.</param>
    /// <returns>
    ///   <para>An error message if an error occurred.</para>
    /// </returns>
    public static string BuildPlayer(string[] levels, string locationPathName, BuildTarget target, BuildOptions options)
    {
      try
      {
        uint crc;
        return BuildPipeline.BuildPlayerInternal(levels, locationPathName, target, options, out crc);
      }
      catch (Exception ex)
      {
        BuildPipeline.LogBuildExceptionAndExit("BuildPipeline.BuildPlayer", ex);
        return string.Empty;
      }
    }

    /// <summary>
    ///   <para>Builds one or more scenes and all their dependencies into a compressed asset bundle.</para>
    /// </summary>
    /// <param name="levels">Pathnames of levels to include in the asset bundle.</param>
    /// <param name="locationPath">Pathname for the output asset bundle.</param>
    /// <param name="target">Runtime platform on which the asset bundle will be used.</param>
    /// <param name="crc">Output parameter to receive CRC checksum of generated assetbundle.</param>
    /// <param name="options">Build options. See BuildOptions for possible values.</param>
    /// <returns>
    ///   <para>String with an error message, empty on success.</para>
    /// </returns>
    [Obsolete("BuildStreamedSceneAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static string BuildStreamedSceneAssetBundle(string[] levels, string locationPath, BuildTarget target, BuildOptions options)
    {
      return BuildPipeline.BuildPlayer(levels, locationPath, target, options | BuildOptions.BuildAdditionalStreamedScenes);
    }

    /// <summary>
    ///   <para>Builds one or more scenes and all their dependencies into a compressed asset bundle.</para>
    /// </summary>
    /// <param name="levels">Pathnames of levels to include in the asset bundle.</param>
    /// <param name="locationPath">Pathname for the output asset bundle.</param>
    /// <param name="target">Runtime platform on which the asset bundle will be used.</param>
    /// <param name="crc">Output parameter to receive CRC checksum of generated assetbundle.</param>
    /// <param name="options">Build options. See BuildOptions for possible values.</param>
    /// <returns>
    ///   <para>String with an error message, empty on success.</para>
    /// </returns>
    [Obsolete("BuildStreamedSceneAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static string BuildStreamedSceneAssetBundle(string[] levels, string locationPath, BuildTarget target)
    {
      return BuildPipeline.BuildPlayer(levels, locationPath, target, BuildOptions.BuildAdditionalStreamedScenes);
    }

    [Obsolete("BuildStreamedSceneAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static string BuildStreamedSceneAssetBundle(string[] levels, string locationPath, BuildTarget target, out uint crc, BuildOptions options)
    {
      crc = 0U;
      try
      {
        return BuildPipeline.BuildPlayerInternal(levels, locationPath, target, options | BuildOptions.BuildAdditionalStreamedScenes, out crc);
      }
      catch (Exception ex)
      {
        BuildPipeline.LogBuildExceptionAndExit("BuildPipeline.BuildStreamedSceneAssetBundle", ex);
        return string.Empty;
      }
    }

    [Obsolete("BuildStreamedSceneAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static string BuildStreamedSceneAssetBundle(string[] levels, string locationPath, BuildTarget target, out uint crc)
    {
      return BuildPipeline.BuildStreamedSceneAssetBundle(levels, locationPath, target, out crc, BuildOptions.None);
    }

    private static string BuildPlayerInternal(string[] levels, string locationPathName, BuildTarget target, BuildOptions options, out uint crc)
    {
      crc = 0U;
      if ((BuildOptions.EnableHeadlessMode & options) != BuildOptions.None && (BuildOptions.Development & options) != BuildOptions.None)
        return "Unsupported build setting: cannot build headless development player";
      if (target == BuildTarget.WP8Player)
        return "Windows Phone 8.0 is no longer supported, please switch to Windows Phone 8.1";
      if (target == BuildTarget.WSAPlayer && EditorUserBuildSettings.wsaSDK == WSASDK.SDK80)
        return "Windows SDK 8.0 is no longer supported, please switch to Windows SDK 8.1";
      return BuildPipeline.BuildPlayerInternalNoCheck(levels, locationPathName, target, options, false, out crc);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string BuildPlayerInternalNoCheck(string[] levels, string locationPathName, BuildTarget target, BuildOptions options, bool delayToAfterScriptReload, out uint crc);

    /// <summary>
    ///   <para>Builds an asset bundle.</para>
    /// </summary>
    /// <param name="mainAsset">Lets you specify a specific object that can be conveniently retrieved using AssetBundle.mainAsset.</param>
    /// <param name="assets">An array of assets to write into the bundle.</param>
    /// <param name="pathName">The filename where to write the compressed asset bundle.</param>
    /// <param name="assetBundleOptions">Automatically include dependencies or always include complete assets instead of just the exact referenced objects.</param>
    /// <param name="targetPlatform">The platform to build the bundle for.</param>
    /// <param name="crc">The optional crc output parameter can be used to get a CRC checksum for the generated AssetBundle, which can be used to verify content when downloading AssetBundles using WWW.LoadFromCacheOrDownload.</param>
    [Obsolete("BuildAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundle(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string pathName, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform)
    {
      uint crc;
      return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, out crc, assetBundleOptions, targetPlatform);
    }

    /// <summary>
    ///   <para>Builds an asset bundle.</para>
    /// </summary>
    /// <param name="mainAsset">Lets you specify a specific object that can be conveniently retrieved using AssetBundle.mainAsset.</param>
    /// <param name="assets">An array of assets to write into the bundle.</param>
    /// <param name="pathName">The filename where to write the compressed asset bundle.</param>
    /// <param name="assetBundleOptions">Automatically include dependencies or always include complete assets instead of just the exact referenced objects.</param>
    /// <param name="targetPlatform">The platform to build the bundle for.</param>
    /// <param name="crc">The optional crc output parameter can be used to get a CRC checksum for the generated AssetBundle, which can be used to verify content when downloading AssetBundles using WWW.LoadFromCacheOrDownload.</param>
    [Obsolete("BuildAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundle(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string pathName, BuildAssetBundleOptions assetBundleOptions)
    {
      BuildTarget targetPlatform = BuildTarget.WebPlayer;
      return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, assetBundleOptions, targetPlatform);
    }

    /// <summary>
    ///   <para>Builds an asset bundle.</para>
    /// </summary>
    /// <param name="mainAsset">Lets you specify a specific object that can be conveniently retrieved using AssetBundle.mainAsset.</param>
    /// <param name="assets">An array of assets to write into the bundle.</param>
    /// <param name="pathName">The filename where to write the compressed asset bundle.</param>
    /// <param name="assetBundleOptions">Automatically include dependencies or always include complete assets instead of just the exact referenced objects.</param>
    /// <param name="targetPlatform">The platform to build the bundle for.</param>
    /// <param name="crc">The optional crc output parameter can be used to get a CRC checksum for the generated AssetBundle, which can be used to verify content when downloading AssetBundles using WWW.LoadFromCacheOrDownload.</param>
    [Obsolete("BuildAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundle(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string pathName)
    {
      BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
      return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, assetBundleOptions);
    }

    [Obsolete("BuildAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundle(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string pathName, out uint crc, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform)
    {
      crc = 0U;
      try
      {
        return BuildPipeline.BuildAssetBundleInternal(mainAsset, assets, (string[]) null, pathName, assetBundleOptions, targetPlatform, out crc);
      }
      catch (Exception ex)
      {
        BuildPipeline.LogBuildExceptionAndExit("BuildPipeline.BuildAssetBundle", ex);
        return false;
      }
    }

    [Obsolete("BuildAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundle(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string pathName, out uint crc, BuildAssetBundleOptions assetBundleOptions)
    {
      BuildTarget targetPlatform = BuildTarget.WebPlayer;
      return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, out crc, assetBundleOptions, targetPlatform);
    }

    [Obsolete("BuildAssetBundle has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundle(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string pathName, out uint crc)
    {
      BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
      return BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, out crc, assetBundleOptions);
    }

    /// <summary>
    ///   <para>Builds an asset bundle, with custom names for the assets.</para>
    /// </summary>
    /// <param name="assets">A collection of assets to be built into the asset bundle. Asset bundles can contain any asset found in the project folder.</param>
    /// <param name="assetNames">An array of strings of the same size as the number of assets.
    /// These will be used as asset names, which you can then pass to AssetBundle.Load to load a specific asset. Use BuildAssetBundle to just use the asset's path names instead.</param>
    /// <param name="pathName">The location where the compressed asset bundle will be written to.</param>
    /// <param name="assetBundleOptions">Automatically include dependencies or always include complete assets instead of just the exact referenced objects.</param>
    /// <param name="targetPlatform">The platform where the asset bundle will be used.</param>
    /// <param name="crc">An optional output parameter used to get a CRC checksum for the generated AssetBundle. (Used to verify content when downloading AssetBundles using WWW.LoadFromCacheOrDownload.)</param>
    [Obsolete("BuildAssetBundleExplicitAssetNames has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundleExplicitAssetNames(UnityEngine.Object[] assets, string[] assetNames, string pathName, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform)
    {
      uint crc;
      return BuildPipeline.BuildAssetBundleExplicitAssetNames(assets, assetNames, pathName, out crc, assetBundleOptions, targetPlatform);
    }

    /// <summary>
    ///   <para>Builds an asset bundle, with custom names for the assets.</para>
    /// </summary>
    /// <param name="assets">A collection of assets to be built into the asset bundle. Asset bundles can contain any asset found in the project folder.</param>
    /// <param name="assetNames">An array of strings of the same size as the number of assets.
    /// These will be used as asset names, which you can then pass to AssetBundle.Load to load a specific asset. Use BuildAssetBundle to just use the asset's path names instead.</param>
    /// <param name="pathName">The location where the compressed asset bundle will be written to.</param>
    /// <param name="assetBundleOptions">Automatically include dependencies or always include complete assets instead of just the exact referenced objects.</param>
    /// <param name="targetPlatform">The platform where the asset bundle will be used.</param>
    /// <param name="crc">An optional output parameter used to get a CRC checksum for the generated AssetBundle. (Used to verify content when downloading AssetBundles using WWW.LoadFromCacheOrDownload.)</param>
    [Obsolete("BuildAssetBundleExplicitAssetNames has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundleExplicitAssetNames(UnityEngine.Object[] assets, string[] assetNames, string pathName, BuildAssetBundleOptions assetBundleOptions)
    {
      BuildTarget targetPlatform = BuildTarget.WebPlayer;
      return BuildPipeline.BuildAssetBundleExplicitAssetNames(assets, assetNames, pathName, assetBundleOptions, targetPlatform);
    }

    /// <summary>
    ///   <para>Builds an asset bundle, with custom names for the assets.</para>
    /// </summary>
    /// <param name="assets">A collection of assets to be built into the asset bundle. Asset bundles can contain any asset found in the project folder.</param>
    /// <param name="assetNames">An array of strings of the same size as the number of assets.
    /// These will be used as asset names, which you can then pass to AssetBundle.Load to load a specific asset. Use BuildAssetBundle to just use the asset's path names instead.</param>
    /// <param name="pathName">The location where the compressed asset bundle will be written to.</param>
    /// <param name="assetBundleOptions">Automatically include dependencies or always include complete assets instead of just the exact referenced objects.</param>
    /// <param name="targetPlatform">The platform where the asset bundle will be used.</param>
    /// <param name="crc">An optional output parameter used to get a CRC checksum for the generated AssetBundle. (Used to verify content when downloading AssetBundles using WWW.LoadFromCacheOrDownload.)</param>
    [Obsolete("BuildAssetBundleExplicitAssetNames has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundleExplicitAssetNames(UnityEngine.Object[] assets, string[] assetNames, string pathName)
    {
      BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
      return BuildPipeline.BuildAssetBundleExplicitAssetNames(assets, assetNames, pathName, assetBundleOptions);
    }

    [Obsolete("BuildAssetBundleExplicitAssetNames has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundleExplicitAssetNames(UnityEngine.Object[] assets, string[] assetNames, string pathName, out uint crc, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform)
    {
      crc = 0U;
      try
      {
        return BuildPipeline.BuildAssetBundleInternal((UnityEngine.Object) null, assets, assetNames, pathName, assetBundleOptions, targetPlatform, out crc);
      }
      catch (Exception ex)
      {
        BuildPipeline.LogBuildExceptionAndExit("BuildPipeline.BuildAssetBundleExplicitAssetNames", ex);
        return false;
      }
    }

    [Obsolete("BuildAssetBundleExplicitAssetNames has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundleExplicitAssetNames(UnityEngine.Object[] assets, string[] assetNames, string pathName, out uint crc, BuildAssetBundleOptions assetBundleOptions)
    {
      BuildTarget targetPlatform = BuildTarget.WebPlayer;
      return BuildPipeline.BuildAssetBundleExplicitAssetNames(assets, assetNames, pathName, out crc, assetBundleOptions, targetPlatform);
    }

    [Obsolete("BuildAssetBundleExplicitAssetNames has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
    public static bool BuildAssetBundleExplicitAssetNames(UnityEngine.Object[] assets, string[] assetNames, string pathName, out uint crc)
    {
      BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
      return BuildPipeline.BuildAssetBundleExplicitAssetNames(assets, assetNames, pathName, out crc, assetBundleOptions);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool BuildAssetBundleInternal(UnityEngine.Object mainAsset, UnityEngine.Object[] assets, string[] assetNames, string pathName, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform, out uint crc);

    [ExcludeFromDocs]
    public static AssetBundleManifest BuildAssetBundles(string outputPath, BuildAssetBundleOptions assetBundleOptions)
    {
      BuildTarget targetPlatform = BuildTarget.WebPlayer;
      return BuildPipeline.BuildAssetBundles(outputPath, assetBundleOptions, targetPlatform);
    }

    [ExcludeFromDocs]
    public static AssetBundleManifest BuildAssetBundles(string outputPath)
    {
      BuildTarget targetPlatform = BuildTarget.WebPlayer;
      BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.None;
      return BuildPipeline.BuildAssetBundles(outputPath, assetBundleOptions, targetPlatform);
    }

    /// <summary>
    ///   <para>Build all AssetBundles specified in the editor.</para>
    /// </summary>
    /// <param name="outputPath">Output path for the AssetBundles.</param>
    /// <param name="assetBundleOptions">AssetBundle building options.</param>
    /// <param name="targetPlatform">Target build platform.</param>
    public static AssetBundleManifest BuildAssetBundles(string outputPath, [DefaultValue("BuildAssetBundleOptions.None")] BuildAssetBundleOptions assetBundleOptions, [DefaultValue("BuildTarget.WebPlayer")] BuildTarget targetPlatform)
    {
      if (!Directory.Exists(outputPath))
      {
        Debug.LogError((object) ("The output path \"" + outputPath + "\" doesn't exist"));
        return (AssetBundleManifest) null;
      }
      try
      {
        return BuildPipeline.BuildAssetBundlesInternal(outputPath, assetBundleOptions, targetPlatform);
      }
      catch (Exception ex)
      {
        BuildPipeline.LogBuildExceptionAndExit("BuildPipeline.BuildAssetBundles", ex);
        return (AssetBundleManifest) null;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AssetBundleManifest BuildAssetBundlesInternal(string outputPath, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform);

    [ExcludeFromDocs]
    public static AssetBundleManifest BuildAssetBundles(string outputPath, AssetBundleBuild[] builds, BuildAssetBundleOptions assetBundleOptions)
    {
      BuildTarget targetPlatform = BuildTarget.WebPlayer;
      return BuildPipeline.BuildAssetBundles(outputPath, builds, assetBundleOptions, targetPlatform);
    }

    [ExcludeFromDocs]
    public static AssetBundleManifest BuildAssetBundles(string outputPath, AssetBundleBuild[] builds)
    {
      BuildTarget targetPlatform = BuildTarget.WebPlayer;
      BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.None;
      return BuildPipeline.BuildAssetBundles(outputPath, builds, assetBundleOptions, targetPlatform);
    }

    /// <summary>
    ///   <para>Build AssetBundles from a building map.</para>
    /// </summary>
    /// <param name="outputPath">Output path for the AssetBundles.</param>
    /// <param name="assetBundleOptions">AssetBundle building options.</param>
    /// <param name="targetPlatform">Target build platform.</param>
    /// <param name="builds">AssetBundle building map.</param>
    public static AssetBundleManifest BuildAssetBundles(string outputPath, AssetBundleBuild[] builds, [DefaultValue("BuildAssetBundleOptions.None")] BuildAssetBundleOptions assetBundleOptions, [DefaultValue("BuildTarget.WebPlayer")] BuildTarget targetPlatform)
    {
      if (!Directory.Exists(outputPath))
      {
        Debug.LogError((object) ("The output path \"" + outputPath + "\" doesn't exist"));
        return (AssetBundleManifest) null;
      }
      if (builds == null)
      {
        Debug.LogError((object) "AssetBundleBuild cannot be null.");
        return (AssetBundleManifest) null;
      }
      try
      {
        return BuildPipeline.BuildAssetBundlesWithInfoInternal(outputPath, builds, assetBundleOptions, targetPlatform);
      }
      catch (Exception ex)
      {
        BuildPipeline.LogBuildExceptionAndExit("BuildPipeline.BuildAssetBundles", ex);
        return (AssetBundleManifest) null;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AssetBundleManifest BuildAssetBundlesWithInfoInternal(string outputPath, AssetBundleBuild[] builds, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetCRCForAssetBundle(string targetPath, out uint crc);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GetHashForAssetBundle(string targetPath, out Hash128 hash);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool LicenseCheck(BuildTarget target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsBuildTargetSupported(BuildTarget target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetBuildTargetAdvancedLicenseName(BuildTarget target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetPlaybackEngineDirectory(BuildTarget target, BuildOptions options);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPlaybackEngineDirectory(BuildTarget target, BuildOptions options, string playbackEngineDirectory);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetBuildToolsDirectory(BuildTarget target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetMonoBinDirectory(BuildTarget target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetMonoLibDirectory(BuildTarget target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetMonoProfileLibDirectory(BuildTarget target, string profile);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetBuildTargetGroupName(BuildTarget target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsUnityScriptEvalSupported(BuildTarget target);
  }
}
