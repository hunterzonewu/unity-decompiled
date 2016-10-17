// Decompiled with JetBrains decompiler
// Type: UnityEngine.Audio.AudioMixerSnapshot
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.Audio
{
  /// <summary>
  ///   <para>Object representing a snapshot in the mixer.</para>
  /// </summary>
  public class AudioMixerSnapshot : Object
  {
    public AudioMixer audioMixer { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal AudioMixerSnapshot()
    {
    }

    /// <summary>
    ///   <para>Performs an interpolated transition towards this snapshot over the time interval specified.</para>
    /// </summary>
    /// <param name="timeToReach">Relative time after which this snapshot should be reached from any current state.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void TransitionTo(float timeToReach);
  }
}
