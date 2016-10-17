// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioImporterSampleSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///         <para>This structure contains a collection of settings used to define how an AudioClip should be imported.
  /// 
  /// This  structure is used with the AudioImporter to define how the AudioClip should be imported and treated during loading within the scene.</para>
  ///       </summary>
  public struct AudioImporterSampleSettings
  {
    /// <summary>
    ///   <para>LoadType defines how the imported AudioClip data should be loaded.</para>
    /// </summary>
    public AudioClipLoadType loadType;
    /// <summary>
    ///   <para>Defines how the sample rate is modified (if at all) of the importer audio file.</para>
    /// </summary>
    public AudioSampleRateSetting sampleRateSetting;
    /// <summary>
    ///   <para>Target sample rate to convert to when samplerateSetting is set to OverrideSampleRate.</para>
    /// </summary>
    public uint sampleRateOverride;
    /// <summary>
    ///   <para>CompressionFormat defines the compression type that the audio file is encoded to. Different compression types have different performance and audio artifact characteristics.</para>
    /// </summary>
    public AudioCompressionFormat compressionFormat;
    /// <summary>
    ///         <para>Audio compression quality (0-1)
    /// 
    /// Amount of compression. The value roughly corresponds to the ratio between the resulting and the source file sizes.</para>
    ///       </summary>
    public float quality;
    public int conversionMode;
  }
}
