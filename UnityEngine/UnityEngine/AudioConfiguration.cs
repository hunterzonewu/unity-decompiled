// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioConfiguration
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Specifies the current properties or desired properties to be set for the audio system.</para>
  /// </summary>
  public struct AudioConfiguration
  {
    /// <summary>
    ///   <para>The current speaker mode used by the audio output device.</para>
    /// </summary>
    public AudioSpeakerMode speakerMode;
    /// <summary>
    ///   <para>The length of the DSP buffer in samples determining the latency of sounds by the audio output device.</para>
    /// </summary>
    public int dspBufferSize;
    /// <summary>
    ///   <para>The current sample rate of the audio output device used.</para>
    /// </summary>
    public int sampleRate;
    /// <summary>
    ///   <para>The current maximum number of simultaneously audible sounds in the game.</para>
    /// </summary>
    public int numRealVoices;
    /// <summary>
    ///   <para>The  maximum number of managed sounds in the game. Beyond this limit sounds will simply stop playing.</para>
    /// </summary>
    public int numVirtualVoices;
  }
}
