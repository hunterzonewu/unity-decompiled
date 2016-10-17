// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioDistortionFilter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Audio Distortion Filter distorts the sound from an AudioSource or.</para>
  /// </summary>
  public sealed class AudioDistortionFilter : Behaviour
  {
    /// <summary>
    ///   <para>Distortion value. 0.0 to 1.0. Default = 0.5.</para>
    /// </summary>
    public float distortionLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
