// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioImporter))]
  [CanEditMultipleObjects]
  internal class AudioImporterInspector : AssetImporterInspector
  {
    public SerializedProperty m_ForceToMono;
    public SerializedProperty m_Normalize;
    public SerializedProperty m_PreloadAudioData;
    public SerializedProperty m_LoadInBackground;
    public SerializedProperty m_OrigSize;
    public SerializedProperty m_CompSize;
    private AudioImporterInspector.SampleSettingProperties m_DefaultSampleSettings;
    private Dictionary<BuildTargetGroup, AudioImporterInspector.SampleSettingProperties> m_SampleSettingOverrides;

    [DebuggerHidden]
    private IEnumerable<AudioImporter> GetAllAudioImporterTargets()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioImporterInspector.\u003CGetAllAudioImporterTargets\u003Ec__Iterator7 targetsCIterator7 = new AudioImporterInspector.\u003CGetAllAudioImporterTargets\u003Ec__Iterator7() { \u003C\u003Ef__this = this };
      // ISSUE: reference to a compiler-generated field
      targetsCIterator7.\u0024PC = -2;
      return (IEnumerable<AudioImporter>) targetsCIterator7;
    }

    private bool SyncSettingsToBackend()
    {
      BuildPlayerWindow.BuildPlatform[] array = BuildPlayerWindow.GetValidPlatforms().ToArray();
      foreach (AudioImporter audioImporterTarget in this.GetAllAudioImporterTargets())
      {
        AudioImporterSampleSettings defaultSampleSettings = audioImporterTarget.defaultSampleSettings;
        if (this.m_DefaultSampleSettings.loadTypeChanged)
          defaultSampleSettings.loadType = this.m_DefaultSampleSettings.settings.loadType;
        if (this.m_DefaultSampleSettings.sampleRateSettingChanged)
          defaultSampleSettings.sampleRateSetting = this.m_DefaultSampleSettings.settings.sampleRateSetting;
        if (this.m_DefaultSampleSettings.sampleRateOverrideChanged)
          defaultSampleSettings.sampleRateOverride = this.m_DefaultSampleSettings.settings.sampleRateOverride;
        if (this.m_DefaultSampleSettings.compressionFormatChanged)
          defaultSampleSettings.compressionFormat = this.m_DefaultSampleSettings.settings.compressionFormat;
        if (this.m_DefaultSampleSettings.qualityChanged)
          defaultSampleSettings.quality = this.m_DefaultSampleSettings.settings.quality;
        if (this.m_DefaultSampleSettings.conversionModeChanged)
          defaultSampleSettings.conversionMode = this.m_DefaultSampleSettings.settings.conversionMode;
        audioImporterTarget.defaultSampleSettings = defaultSampleSettings;
        foreach (BuildPlayerWindow.BuildPlatform buildPlatform in array)
        {
          BuildTargetGroup targetGroup = buildPlatform.targetGroup;
          if (this.m_SampleSettingOverrides.ContainsKey(targetGroup))
          {
            AudioImporterInspector.SampleSettingProperties sampleSettingOverride = this.m_SampleSettingOverrides[targetGroup];
            if (sampleSettingOverride.overrideIsForced && !sampleSettingOverride.forcedOverrideState)
              audioImporterTarget.Internal_ClearSampleSettingOverride(targetGroup);
            else if (audioImporterTarget.Internal_ContainsSampleSettingsOverride(targetGroup) || sampleSettingOverride.overrideIsForced && sampleSettingOverride.forcedOverrideState)
            {
              AudioImporterSampleSettings overrideSampleSettings = audioImporterTarget.Internal_GetOverrideSampleSettings(targetGroup);
              if (sampleSettingOverride.loadTypeChanged)
                overrideSampleSettings.loadType = sampleSettingOverride.settings.loadType;
              if (sampleSettingOverride.sampleRateSettingChanged)
                overrideSampleSettings.sampleRateSetting = sampleSettingOverride.settings.sampleRateSetting;
              if (sampleSettingOverride.sampleRateOverrideChanged)
                overrideSampleSettings.sampleRateOverride = sampleSettingOverride.settings.sampleRateOverride;
              if (sampleSettingOverride.compressionFormatChanged)
                overrideSampleSettings.compressionFormat = sampleSettingOverride.settings.compressionFormat;
              if (sampleSettingOverride.qualityChanged)
                overrideSampleSettings.quality = sampleSettingOverride.settings.quality;
              if (sampleSettingOverride.conversionModeChanged)
                overrideSampleSettings.conversionMode = sampleSettingOverride.settings.conversionMode;
              audioImporterTarget.Internal_SetOverrideSampleSettings(targetGroup, overrideSampleSettings);
            }
            this.m_SampleSettingOverrides[targetGroup] = sampleSettingOverride;
          }
        }
      }
      this.m_DefaultSampleSettings.ClearChangedFlags();
      foreach (BuildPlayerWindow.BuildPlatform buildPlatform in array)
      {
        BuildTargetGroup targetGroup = buildPlatform.targetGroup;
        if (this.m_SampleSettingOverrides.ContainsKey(targetGroup))
        {
          AudioImporterInspector.SampleSettingProperties sampleSettingOverride = this.m_SampleSettingOverrides[targetGroup];
          sampleSettingOverride.ClearChangedFlags();
          this.m_SampleSettingOverrides[targetGroup] = sampleSettingOverride;
        }
      }
      return true;
    }

    private bool ResetSettingsFromBackend()
    {
      if (this.GetAllAudioImporterTargets().Any<AudioImporter>())
      {
        AudioImporter audioImporter = this.GetAllAudioImporterTargets().First<AudioImporter>();
        this.m_DefaultSampleSettings.settings = audioImporter.defaultSampleSettings;
        this.m_DefaultSampleSettings.ClearChangedFlags();
        this.m_SampleSettingOverrides = new Dictionary<BuildTargetGroup, AudioImporterInspector.SampleSettingProperties>();
        using (List<BuildPlayerWindow.BuildPlatform>.Enumerator enumerator = BuildPlayerWindow.GetValidPlatforms().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            BuildTargetGroup targetGroup = enumerator.Current.targetGroup;
            foreach (AudioImporter audioImporterTarget in this.GetAllAudioImporterTargets())
            {
              if (audioImporterTarget.Internal_ContainsSampleSettingsOverride(targetGroup))
              {
                this.m_SampleSettingOverrides[targetGroup] = new AudioImporterInspector.SampleSettingProperties()
                {
                  settings = audioImporterTarget.Internal_GetOverrideSampleSettings(targetGroup)
                };
                break;
              }
            }
            if (!this.m_SampleSettingOverrides.ContainsKey(targetGroup))
              this.m_SampleSettingOverrides[targetGroup] = new AudioImporterInspector.SampleSettingProperties()
              {
                settings = audioImporter.Internal_GetOverrideSampleSettings(targetGroup)
              };
          }
        }
      }
      return true;
    }

    public bool CurrentPlatformHasAutoTranslatedCompression()
    {
      BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
      foreach (AudioImporter audioImporterTarget in this.GetAllAudioImporterTargets())
      {
        AudioCompressionFormat compressionFormat1 = audioImporterTarget.defaultSampleSettings.compressionFormat;
        if (!audioImporterTarget.Internal_ContainsSampleSettingsOverride(buildTargetGroup))
        {
          AudioCompressionFormat compressionFormat2 = audioImporterTarget.Internal_GetOverrideSampleSettings(buildTargetGroup).compressionFormat;
          if (compressionFormat1 != compressionFormat2)
            return true;
        }
      }
      return false;
    }

    public bool IsHardwareSound(AudioCompressionFormat format)
    {
      switch (format)
      {
        case AudioCompressionFormat.VAG:
        case AudioCompressionFormat.HEVAG:
        case AudioCompressionFormat.XMA:
        case AudioCompressionFormat.GCADPCM:
          return true;
        default:
          return false;
      }
    }

    public bool CurrentSelectionContainsHardwareSounds()
    {
      BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
      foreach (AudioImporter audioImporterTarget in this.GetAllAudioImporterTargets())
      {
        if (this.IsHardwareSound(audioImporterTarget.Internal_GetOverrideSampleSettings(buildTargetGroup).compressionFormat))
          return true;
      }
      return false;
    }

    public void OnEnable()
    {
      this.m_ForceToMono = this.serializedObject.FindProperty("m_ForceToMono");
      this.m_Normalize = this.serializedObject.FindProperty("m_Normalize");
      this.m_PreloadAudioData = this.serializedObject.FindProperty("m_PreloadAudioData");
      this.m_LoadInBackground = this.serializedObject.FindProperty("m_LoadInBackground");
      this.m_OrigSize = this.serializedObject.FindProperty("m_PreviewData.m_OrigSize");
      this.m_CompSize = this.serializedObject.FindProperty("m_PreviewData.m_CompSize");
      this.ResetSettingsFromBackend();
    }

    internal override void ResetValues()
    {
      base.ResetValues();
      this.ResetSettingsFromBackend();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.UpdateIfDirtyOrScript();
      bool selectionContainsTrackerFile = false;
      foreach (AssetImporter audioImporterTarget in this.GetAllAudioImporterTargets())
      {
        string lowerInvariant = FileUtil.GetPathExtension(audioImporterTarget.assetPath).ToLowerInvariant();
        if (lowerInvariant == "mod" || lowerInvariant == "it" || (lowerInvariant == "s3m" || lowerInvariant == "xm"))
        {
          selectionContainsTrackerFile = true;
          break;
        }
      }
      this.OnAudioImporterGUI(selectionContainsTrackerFile);
      int bytes1 = 0;
      int bytes2 = 0;
      foreach (AudioImporter audioImporterTarget in this.GetAllAudioImporterTargets())
      {
        bytes1 += audioImporterTarget.origSize;
        bytes2 += audioImporterTarget.compSize;
      }
      GUILayout.Space(10f);
      EditorGUILayout.HelpBox("Original Size: \t" + EditorUtility.FormatBytes(bytes1) + "\nImported Size: \t" + EditorUtility.FormatBytes(bytes2) + "\nRatio: \t\t" + (100f * (float) bytes2 / (float) bytes1).ToString("0.00") + "%", MessageType.Info);
      if (this.CurrentPlatformHasAutoTranslatedCompression())
      {
        GUILayout.Space(10f);
        EditorGUILayout.HelpBox("The selection contains different compression formats to the default settings for the current build platform.", MessageType.Info);
      }
      if (this.CurrentSelectionContainsHardwareSounds())
      {
        GUILayout.Space(10f);
        EditorGUILayout.HelpBox("The selection contains sounds that are decompressed in hardware. Advanced mixing is not available for these sounds.", MessageType.Info);
      }
      this.ApplyRevertGUI();
    }

    private AudioImporterInspector.MultiValueStatus GetMultiValueStatus(BuildTargetGroup platform)
    {
      AudioImporterInspector.MultiValueStatus multiValueStatus;
      multiValueStatus.multiLoadType = false;
      multiValueStatus.multiSampleRateSetting = false;
      multiValueStatus.multiSampleRateOverride = false;
      multiValueStatus.multiCompressionFormat = false;
      multiValueStatus.multiQuality = false;
      multiValueStatus.multiConversionMode = false;
      if (this.GetAllAudioImporterTargets().Any<AudioImporter>())
      {
        AudioImporter audioImporter1 = this.GetAllAudioImporterTargets().First<AudioImporter>();
        AudioImporterSampleSettings importerSampleSettings1 = platform != BuildTargetGroup.Unknown ? audioImporter1.Internal_GetOverrideSampleSettings(platform) : audioImporter1.defaultSampleSettings;
        IEnumerable<AudioImporter> audioImporterTargets = this.GetAllAudioImporterTargets();
        AudioImporter[] audioImporterArray = new AudioImporter[1]{ audioImporter1 };
        foreach (AudioImporter audioImporter2 in audioImporterTargets.Except<AudioImporter>((IEnumerable<AudioImporter>) audioImporterArray))
        {
          AudioImporterSampleSettings importerSampleSettings2 = platform != BuildTargetGroup.Unknown ? audioImporter2.Internal_GetOverrideSampleSettings(platform) : audioImporter2.defaultSampleSettings;
          multiValueStatus.multiLoadType |= importerSampleSettings1.loadType != importerSampleSettings2.loadType;
          multiValueStatus.multiSampleRateSetting |= importerSampleSettings1.sampleRateSetting != importerSampleSettings2.sampleRateSetting;
          multiValueStatus.multiSampleRateOverride |= (int) importerSampleSettings1.sampleRateOverride != (int) importerSampleSettings2.sampleRateOverride;
          multiValueStatus.multiCompressionFormat |= importerSampleSettings1.compressionFormat != importerSampleSettings2.compressionFormat;
          multiValueStatus.multiQuality |= (double) importerSampleSettings1.quality != (double) importerSampleSettings2.quality;
          multiValueStatus.multiConversionMode |= importerSampleSettings1.conversionMode != importerSampleSettings2.conversionMode;
        }
      }
      return multiValueStatus;
    }

    private AudioImporterInspector.OverrideStatus GetOverrideStatus(BuildTargetGroup platform)
    {
      bool flag1 = false;
      bool flag2 = false;
      if (this.GetAllAudioImporterTargets().Any<AudioImporter>())
      {
        AudioImporter audioImporter1 = this.GetAllAudioImporterTargets().First<AudioImporter>();
        flag2 = audioImporter1.Internal_ContainsSampleSettingsOverride(platform);
        IEnumerable<AudioImporter> audioImporterTargets = this.GetAllAudioImporterTargets();
        AudioImporter[] audioImporterArray = new AudioImporter[1]{ audioImporter1 };
        foreach (AudioImporter audioImporter2 in audioImporterTargets.Except<AudioImporter>((IEnumerable<AudioImporter>) audioImporterArray))
        {
          bool flag3 = audioImporter2.Internal_ContainsSampleSettingsOverride(platform);
          if (flag3 != flag2)
            flag1 = ((flag1 ? 1 : 0) | 1) != 0;
          flag2 |= flag3;
        }
      }
      if (!flag2)
        return AudioImporterInspector.OverrideStatus.NoOverrides;
      return flag1 ? AudioImporterInspector.OverrideStatus.MixedOverrides : AudioImporterInspector.OverrideStatus.AllOverrides;
    }

    private AudioCompressionFormat[] GetFormatsForPlatform(BuildTargetGroup platform)
    {
      List<AudioCompressionFormat> compressionFormatList = new List<AudioCompressionFormat>();
      if (platform == BuildTargetGroup.WebGL)
      {
        compressionFormatList.Add(AudioCompressionFormat.AAC);
        return compressionFormatList.ToArray();
      }
      compressionFormatList.Add(AudioCompressionFormat.PCM);
      if (platform != BuildTargetGroup.PS3 && platform != BuildTargetGroup.PSM && platform != BuildTargetGroup.PSP2)
        compressionFormatList.Add(AudioCompressionFormat.Vorbis);
      compressionFormatList.Add(AudioCompressionFormat.ADPCM);
      if (platform != BuildTargetGroup.Standalone && platform != BuildTargetGroup.WebPlayer && (platform != BuildTargetGroup.Metro && platform != BuildTargetGroup.WiiU) && (platform != BuildTargetGroup.XboxOne && platform != BuildTargetGroup.XBOX360 && platform != BuildTargetGroup.Unknown))
        compressionFormatList.Add(AudioCompressionFormat.MP3);
      if (platform == BuildTargetGroup.PSM)
        compressionFormatList.Add(AudioCompressionFormat.VAG);
      if (platform == BuildTargetGroup.PSP2)
        compressionFormatList.Add(AudioCompressionFormat.HEVAG);
      if (platform == BuildTargetGroup.WiiU)
        compressionFormatList.Add(AudioCompressionFormat.GCADPCM);
      if (platform == BuildTargetGroup.XboxOne)
        compressionFormatList.Add(AudioCompressionFormat.XMA);
      return compressionFormatList.ToArray();
    }

    private bool CompressionFormatHasQuality(AudioCompressionFormat format)
    {
      switch (format)
      {
        case AudioCompressionFormat.Vorbis:
        case AudioCompressionFormat.MP3:
        case AudioCompressionFormat.XMA:
        case AudioCompressionFormat.AAC:
          return true;
        default:
          return false;
      }
    }

    private void OnSampleSettingGUI(BuildTargetGroup platform, AudioImporterInspector.MultiValueStatus status, bool selectionContainsTrackerFile, ref AudioImporterInspector.SampleSettingProperties properties, bool disablePreloadAudioDataOption)
    {
      EditorGUI.showMixedValue = status.multiLoadType && !properties.loadTypeChanged;
      EditorGUI.BeginChangeCheck();
      AudioClipLoadType audioClipLoadType = (AudioClipLoadType) EditorGUILayout.EnumPopup("Load Type", (Enum) properties.settings.loadType, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        properties.settings.loadType = audioClipLoadType;
        properties.loadTypeChanged = true;
      }
      EditorGUI.BeginDisabledGroup(disablePreloadAudioDataOption);
      if (disablePreloadAudioDataOption)
        EditorGUILayout.Toggle("Preload Audio Data", false, new GUILayoutOption[0]);
      else
        EditorGUILayout.PropertyField(this.m_PreloadAudioData);
      EditorGUI.EndDisabledGroup();
      if (selectionContainsTrackerFile)
        return;
      AudioCompressionFormat[] formatsForPlatform = this.GetFormatsForPlatform(platform);
      EditorGUI.showMixedValue = status.multiCompressionFormat && !properties.compressionFormatChanged;
      EditorGUI.BeginChangeCheck();
      AudioCompressionFormat compressionFormat = (AudioCompressionFormat) EditorGUILayout.IntPopup("Compression Format", (int) properties.settings.compressionFormat, Array.ConvertAll<AudioCompressionFormat, string>(formatsForPlatform, (Converter<AudioCompressionFormat, string>) (value => value.ToString())), Array.ConvertAll<AudioCompressionFormat, int>(formatsForPlatform, (Converter<AudioCompressionFormat, int>) (value => (int) value)), new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        properties.settings.compressionFormat = compressionFormat;
        properties.compressionFormatChanged = true;
      }
      if (this.CompressionFormatHasQuality(properties.settings.compressionFormat))
      {
        EditorGUI.showMixedValue = status.multiQuality && !properties.qualityChanged;
        EditorGUI.BeginChangeCheck();
        int num = EditorGUILayout.IntSlider("Quality", (int) Mathf.Clamp(properties.settings.quality * 100f, 1f, 100f), 1, 100, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          properties.settings.quality = 0.01f * (float) num;
          properties.qualityChanged = true;
        }
      }
      EditorGUI.showMixedValue = status.multiSampleRateSetting && !properties.sampleRateSettingChanged;
      EditorGUI.BeginChangeCheck();
      AudioSampleRateSetting sampleRateSetting = (AudioSampleRateSetting) EditorGUILayout.EnumPopup("Sample Rate Setting", (Enum) properties.settings.sampleRateSetting, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        properties.settings.sampleRateSetting = sampleRateSetting;
        properties.sampleRateSettingChanged = true;
      }
      if (properties.settings.sampleRateSetting == AudioSampleRateSetting.OverrideSampleRate)
      {
        EditorGUI.showMixedValue = status.multiSampleRateOverride && !properties.sampleRateOverrideChanged;
        EditorGUI.BeginChangeCheck();
        int num = EditorGUILayout.IntPopup("Sample Rate", (int) properties.settings.sampleRateOverride, AudioImporterInspector.Styles.kSampleRateStrings, AudioImporterInspector.Styles.kSampleRateValues, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          properties.settings.sampleRateOverride = (uint) num;
          properties.sampleRateOverrideChanged = true;
        }
      }
      EditorGUI.showMixedValue = false;
    }

    private void OnAudioImporterGUI(bool selectionContainsTrackerFile)
    {
      if (!selectionContainsTrackerFile)
      {
        EditorGUILayout.PropertyField(this.m_ForceToMono);
        ++EditorGUI.indentLevel;
        EditorGUI.BeginDisabledGroup(!this.m_ForceToMono.boolValue);
        EditorGUILayout.PropertyField(this.m_Normalize);
        EditorGUI.EndDisabledGroup();
        --EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_LoadInBackground);
      }
      BuildPlayerWindow.BuildPlatform[] array = BuildPlayerWindow.GetValidPlatforms().ToArray();
      GUILayout.Space(10f);
      int index = EditorGUILayout.BeginPlatformGrouping(array, GUIContent.Temp("Default"));
      if (index == -1)
      {
        bool disablePreloadAudioDataOption = this.m_DefaultSampleSettings.settings.loadType == AudioClipLoadType.Streaming;
        this.OnSampleSettingGUI(BuildTargetGroup.Unknown, this.GetMultiValueStatus(BuildTargetGroup.Unknown), selectionContainsTrackerFile, ref this.m_DefaultSampleSettings, disablePreloadAudioDataOption);
      }
      else
      {
        BuildTargetGroup targetGroup = array[index].targetGroup;
        AudioImporterInspector.SampleSettingProperties sampleSettingOverride = this.m_SampleSettingOverrides[targetGroup];
        AudioImporterInspector.OverrideStatus overrideStatus = this.GetOverrideStatus(targetGroup);
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = overrideStatus == AudioImporterInspector.OverrideStatus.MixedOverrides && !sampleSettingOverride.overrideIsForced;
        bool flag1 = sampleSettingOverride.overrideIsForced && sampleSettingOverride.forcedOverrideState || !sampleSettingOverride.overrideIsForced && overrideStatus != AudioImporterInspector.OverrideStatus.NoOverrides;
        bool flag2 = EditorGUILayout.Toggle("Override for " + array[index].name, flag1, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          sampleSettingOverride.forcedOverrideState = flag2;
          sampleSettingOverride.overrideIsForced = true;
        }
        bool disablePreloadAudioDataOption = (sampleSettingOverride.overrideIsForced && sampleSettingOverride.forcedOverrideState || this.GetOverrideStatus(targetGroup) == AudioImporterInspector.OverrideStatus.AllOverrides) && sampleSettingOverride.settings.loadType == AudioClipLoadType.Streaming;
        AudioImporterInspector.MultiValueStatus multiValueStatus = this.GetMultiValueStatus(targetGroup);
        EditorGUI.BeginDisabledGroup((!sampleSettingOverride.overrideIsForced || !sampleSettingOverride.forcedOverrideState ? (overrideStatus == AudioImporterInspector.OverrideStatus.AllOverrides ? 1 : 0) : 1) == 0);
        this.OnSampleSettingGUI(targetGroup, multiValueStatus, selectionContainsTrackerFile, ref sampleSettingOverride, disablePreloadAudioDataOption);
        EditorGUI.EndDisabledGroup();
        this.m_SampleSettingOverrides[targetGroup] = sampleSettingOverride;
      }
      EditorGUILayout.EndPlatformGrouping();
    }

    internal override bool HasModified()
    {
      if (base.HasModified() || this.m_DefaultSampleSettings.HasModified())
        return true;
      using (Dictionary<BuildTargetGroup, AudioImporterInspector.SampleSettingProperties>.ValueCollection.Enumerator enumerator = this.m_SampleSettingOverrides.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.HasModified())
            return true;
        }
      }
      return false;
    }

    internal override void Apply()
    {
      base.Apply();
      this.SyncSettingsToBackend();
      using (List<ProjectBrowser>.Enumerator enumerator = ProjectBrowser.GetAllProjectBrowsers().GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Repaint();
      }
    }

    private static class Styles
    {
      public static readonly string[] kSampleRateStrings = new string[7]{ "8,000 Hz", "11,025 Hz", "22,050 Hz", "44,100 Hz", "48,000 Hz", "96,000 Hz", "192,000 Hz" };
      public static readonly int[] kSampleRateValues = new int[7]{ 8000, 11025, 22050, 44100, 48000, 96000, 192000 };
    }

    private struct MultiValueStatus
    {
      public bool multiLoadType;
      public bool multiSampleRateSetting;
      public bool multiSampleRateOverride;
      public bool multiCompressionFormat;
      public bool multiQuality;
      public bool multiConversionMode;
    }

    private struct SampleSettingProperties
    {
      public AudioImporterSampleSettings settings;
      public bool forcedOverrideState;
      public bool overrideIsForced;
      public bool loadTypeChanged;
      public bool sampleRateSettingChanged;
      public bool sampleRateOverrideChanged;
      public bool compressionFormatChanged;
      public bool qualityChanged;
      public bool conversionModeChanged;

      public bool HasModified()
      {
        if (!this.overrideIsForced && !this.loadTypeChanged && (!this.sampleRateSettingChanged && !this.sampleRateOverrideChanged) && (!this.compressionFormatChanged && !this.qualityChanged))
          return this.conversionModeChanged;
        return true;
      }

      public void ClearChangedFlags()
      {
        this.forcedOverrideState = false;
        this.overrideIsForced = false;
        this.loadTypeChanged = false;
        this.sampleRateSettingChanged = false;
        this.sampleRateOverrideChanged = false;
        this.compressionFormatChanged = false;
        this.qualityChanged = false;
        this.conversionModeChanged = false;
      }
    }

    private enum OverrideStatus
    {
      NoOverrides,
      MixedOverrides,
      AllOverrides,
    }
  }
}
