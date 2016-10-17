// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioChorusFilter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Audio Chorus Filter takes an Audio Clip and processes it creating a chorus effect.</para>
  /// </summary>
  public sealed class AudioChorusFilter : Behaviour
  {
    /// <summary>
    ///   <para>Volume of original signal to pass to output. 0.0 to 1.0. Default = 0.5.</para>
    /// </summary>
    public float dryMix { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Volume of 1st chorus tap. 0.0 to 1.0. Default = 0.5.</para>
    /// </summary>
    public float wetMix1 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Volume of 2nd chorus tap. This tap is 90 degrees out of phase of the first tap. 0.0 to 1.0. Default = 0.5.</para>
    /// </summary>
    public float wetMix2 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Volume of 3rd chorus tap. This tap is 90 degrees out of phase of the second tap. 0.0 to 1.0. Default = 0.5.</para>
    /// </summary>
    public float wetMix3 { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Chorus delay in ms. 0.1 to 100.0. Default = 40.0 ms.</para>
    /// </summary>
    public float delay { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Chorus modulation rate in hz. 0.0 to 20.0. Default = 0.8 hz.</para>
    /// </summary>
    public float rate { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Chorus modulation depth. 0.0 to 1.0. Default = 0.03.</para>
    /// </summary>
    public float depth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Chorus feedback. Controls how much of the wet signal gets fed back into the chorus buffer. 0.0 to 1.0. Default = 0.0.</para>
    /// </summary>
    [Obsolete("feedback is deprecated, this property does nothing.")]
    public float feedback { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
