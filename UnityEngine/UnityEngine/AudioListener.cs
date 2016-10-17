// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioListener
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Representation of a listener in 3D space.</para>
  /// </summary>
  public sealed class AudioListener : Behaviour
  {
    /// <summary>
    ///   <para>Controls the game sound volume (0.0 to 1.0).</para>
    /// </summary>
    public static extern float volume { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The paused state of the audio system.</para>
    /// </summary>
    public static extern bool pause { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>This lets you set whether the Audio Listener should be updated in the fixed or dynamic update.</para>
    /// </summary>
    public AudioVelocityUpdateMode velocityUpdateMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetOutputDataHelper(float[] samples, int channel);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetSpectrumDataHelper(float[] samples, int channel, FFTWindow window);

    /// <summary>
    ///   <para>Deprecated Version. Returns a block of the listener (master)'s output data.</para>
    /// </summary>
    /// <param name="numSamples"></param>
    /// <param name="channel"></param>
    [Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
    public static float[] GetOutputData(int numSamples, int channel)
    {
      float[] samples = new float[numSamples];
      AudioListener.GetOutputDataHelper(samples, channel);
      return samples;
    }

    /// <summary>
    ///   <para>Provides a block of the listener (master)'s output data.</para>
    /// </summary>
    /// <param name="samples">The array to populate with audio samples. Its length must be a power of 2.</param>
    /// <param name="channel">The channel to sample from.</param>
    public static void GetOutputData(float[] samples, int channel)
    {
      AudioListener.GetOutputDataHelper(samples, channel);
    }

    /// <summary>
    ///   <para>Deprecated Version. Returns a block of the listener (master)'s spectrum data.</para>
    /// </summary>
    /// <param name="numSamples"></param>
    /// <param name="channel"></param>
    /// <param name="window"></param>
    [Obsolete("GetSpectrumData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
    public static float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
    {
      float[] samples = new float[numSamples];
      AudioListener.GetSpectrumDataHelper(samples, channel, window);
      return samples;
    }

    /// <summary>
    ///   <para>Provides a block of the listener (master)'s spectrum data.</para>
    /// </summary>
    /// <param name="samples">The array to populate with audio samples. Its length must be a power of 2.</param>
    /// <param name="channel">The channel to sample from.</param>
    /// <param name="window">The FFTWindow type to use when sampling.</param>
    public static void GetSpectrumData(float[] samples, int channel, FFTWindow window)
    {
      AudioListener.GetSpectrumDataHelper(samples, channel, window);
    }
  }
}
