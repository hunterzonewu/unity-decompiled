// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Audio importer lets you modify AudioClip import settings from editor scripts.</para>
  /// </summary>
  public sealed class AudioImporter : AssetImporter
  {
    /// <summary>
    ///   <para>The default sample settings for the AudioClip importer.</para>
    /// </summary>
    public AudioImporterSampleSettings defaultSampleSettings { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Force this clip to mono?</para>
    /// </summary>
    public bool forceToMono { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Corresponding to the "Load In Background" flag in the AudioClip inspector, when this flag is set, the loading of the clip will happen delayed without blocking the main thread.</para>
    /// </summary>
    public bool loadInBackground
    {
      get
      {
        return this.Internal_GetLoadInBackground();
      }
      set
      {
        this.Internal_SetLoadInBackground(value);
      }
    }

    /// <summary>
    ///   <para>Preloads audio data of the clip when the clip asset is loaded. When this flag is off, scripts have to call AudioClip.LoadAudioData() to load the data before the clip can be played. Properties like length, channels and format are available before the audio data has been loaded.</para>
    /// </summary>
    public bool preloadAudioData
    {
      get
      {
        return this.Internal_GetPreloadAudioData();
      }
      set
      {
        this.Internal_SetPreloadAudioData(value);
      }
    }

    [Obsolete("Setting and getting the compression format is not used anymore (use compressionFormat in defaultSampleSettings instead). Source audio file is assumed to be PCM Wav.")]
    private AudioImporterFormat format
    {
      get
      {
        return this.defaultSampleSettings.compressionFormat == AudioCompressionFormat.PCM ? AudioImporterFormat.Native : AudioImporterFormat.Compressed;
      }
      set
      {
        AudioImporterSampleSettings defaultSampleSettings = this.defaultSampleSettings;
        defaultSampleSettings.compressionFormat = value != AudioImporterFormat.Native ? AudioCompressionFormat.Vorbis : AudioCompressionFormat.PCM;
        this.defaultSampleSettings = defaultSampleSettings;
      }
    }

    [Obsolete("Setting and getting import channels is not used anymore (use forceToMono instead)", true)]
    public AudioImporterChannels channels
    {
      get
      {
        return AudioImporterChannels.Automatic;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Compression bitrate.</para>
    /// </summary>
    [Obsolete("AudioImporter.compressionBitrate is no longer supported", true)]
    public int compressionBitrate { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("AudioImporter.loopable is no longer supported. All audio assets encoded by Unity are by default loopable.")]
    public bool loopable { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("AudioImporter.hardware is no longer supported. All mixing of audio is done by software and only some platforms use hardware acceleration to perform decoding.")]
    public bool hardware { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("Setting/Getting decompressOnLoad is deprecated. Use AudioImporterSampleSettings.loadType instead.")]
    private bool decompressOnLoad
    {
      get
      {
        return this.defaultSampleSettings.loadType == AudioClipLoadType.DecompressOnLoad;
      }
      set
      {
        AudioImporterSampleSettings defaultSampleSettings = this.defaultSampleSettings;
        defaultSampleSettings.loadType = !value ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
        this.defaultSampleSettings = defaultSampleSettings;
      }
    }

    [Obsolete("AudioImporter.quality is no longer supported. Use AudioImporterSampleSettings.")]
    private float quality
    {
      get
      {
        return this.defaultSampleSettings.quality;
      }
      set
      {
        AudioImporterSampleSettings defaultSampleSettings = this.defaultSampleSettings;
        defaultSampleSettings.quality = value;
        this.defaultSampleSettings = defaultSampleSettings;
      }
    }

    [Obsolete("AudioImporter.threeD is no longer supported")]
    public bool threeD { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("AudioImporter.durationMS is deprecated.", true)]
    internal int durationMS { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.frequency is deprecated.", true)]
    internal int frequency { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.origChannelCount is deprecated.", true)]
    internal int origChannelCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.origIsCompressible is deprecated.", true)]
    internal bool origIsCompressible { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.origIsMonoForcable is deprecated.", true)]
    internal bool origIsMonoForcable { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.defaultBitrate is deprecated.", true)]
    internal int defaultBitrate { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("AudioImporter.origType is deprecated.", true)]
    internal AudioType origType
    {
      get
      {
        return AudioType.UNKNOWN;
      }
    }

    [Obsolete("AudioImporter.origFileSize is deprecated.", true)]
    internal int origFileSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal int origSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal int compSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns whether a given build target has its sample settings currently overridden.</para>
    /// </summary>
    /// <param name="platform">The platform to query if this AudioImporter has an override for.</param>
    /// <returns>
    ///   <para>Returns true if the platform is currently overriden in this AudioImporter.</para>
    /// </returns>
    public bool ContainsSampleSettingsOverride(string platform)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName != BuildTargetGroup.Unknown)
        return this.Internal_ContainsSampleSettingsOverride(targetGroupByName);
      Debug.LogError((object) ("Unknown platform passed to AudioImporter.ContainsSampleSettingsOverride (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS3', 'PS4', 'PSP2', 'PSM', 'XBox360', 'XboxOne', 'WP8', 'WSA' or 'BlackBerry'"));
      return false;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool Internal_ContainsSampleSettingsOverride(BuildTargetGroup platformGroup);

    /// <summary>
    ///   <para>Return the current override settings for the given platform.</para>
    /// </summary>
    /// <param name="platform">The platform to get the override settings for.</param>
    /// <returns>
    ///   <para>The override sample settings for the given platform.</para>
    /// </returns>
    public AudioImporterSampleSettings GetOverrideSampleSettings(string platform)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName != BuildTargetGroup.Unknown)
        return this.Internal_GetOverrideSampleSettings(targetGroupByName);
      Debug.LogError((object) ("Unknown platform passed to AudioImporter.GetOverrideSampleSettings (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS3', 'PS4', 'PSP2', 'PSM', 'XBox360', 'XboxOne', 'WP8', 'WSA' or 'BlackBerry'"));
      return this.defaultSampleSettings;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal AudioImporterSampleSettings Internal_GetOverrideSampleSettings(BuildTargetGroup platformGroup);

    /// <summary>
    ///   <para>Sets the override sample settings for the given platform.</para>
    /// </summary>
    /// <param name="platform">The platform which will have the sample settings overridden.</param>
    /// <param name="settings">The override settings for the given platform.</param>
    /// <returns>
    ///   <para>Returns true if the settings were successfully overriden. Some setting overrides are not possible for the given platform, in which case false is returned and the settings are not registered.</para>
    /// </returns>
    public bool SetOverrideSampleSettings(string platform, AudioImporterSampleSettings settings)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName != BuildTargetGroup.Unknown)
        return this.Internal_SetOverrideSampleSettings(targetGroupByName, settings);
      Debug.LogError((object) ("Unknown platform passed to AudioImporter.SetOverrideSampleSettings (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS3', 'PS4', 'PSP2', 'PSM', 'XBox360', 'XboxOne', 'WP8', 'WSA' or 'BlackBerry'"));
      return false;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool Internal_SetOverrideSampleSettings(BuildTargetGroup platformGroup, AudioImporterSampleSettings settings);

    /// <summary>
    ///   <para>Clears the sample settings override for the given platform.</para>
    /// </summary>
    /// <param name="platform">The platform to clear the overrides for.</param>
    /// <returns>
    ///   <para>Returns true if any overrides were actually cleared.</para>
    /// </returns>
    public bool ClearSampleSettingOverride(string platform)
    {
      BuildTargetGroup targetGroupByName = BuildPipeline.GetBuildTargetGroupByName(platform);
      if (targetGroupByName != BuildTargetGroup.Unknown)
        return this.Internal_ClearSampleSettingOverride(targetGroupByName);
      Debug.LogError((object) ("Unknown platform passed to AudioImporter.ClearSampleSettingOverride (" + platform + "), please use one of 'Web', 'Standalone', 'iOS', 'Android', 'WebGL', 'PS3', 'PS4', 'PSP2', 'PSM', 'XBox360', 'XboxOne', 'WP8', 'WSA' or 'BlackBerry'"));
      return false;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool Internal_ClearSampleSettingOverride(BuildTargetGroup platform);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetLoadInBackground(bool flag);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool Internal_GetLoadInBackground();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetPreloadAudioData(bool flag);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool Internal_GetPreloadAudioData();

    [Obsolete("AudioImporter.updateOrigData is deprecated.", true)]
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void updateOrigData();

    [Obsolete("AudioImporter.minBitrate is deprecated.", true)]
    internal int minBitrate(AudioType type)
    {
      return 0;
    }

    [Obsolete("AudioImporter.maxBitrate is deprecated.", true)]
    internal int maxBitrate(AudioType type)
    {
      return 0;
    }
  }
}
