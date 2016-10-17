// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioEchoFilter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Audio Echo Filter repeats a sound after a given Delay, attenuating.</para>
  /// </summary>
  public sealed class AudioEchoFilter : Behaviour
  {
    /// <summary>
    ///   <para>Echo delay in ms. 10 to 5000. Default = 500.</para>
    /// </summary>
    public float delay { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Echo decay per delay. 0 to 1. 1.0 = No decay, 0.0 = total decay (i.e. simple 1 line delay). Default = 0.5.</para>
    /// </summary>
    public float decayRatio { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Volume of original signal to pass to output. 0.0 to 1.0. Default = 1.0.</para>
    /// </summary>
    public float dryMix { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Volume of echo signal to pass to output. 0.0 to 1.0. Default = 1.0.</para>
    /// </summary>
    public float wetMix { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
