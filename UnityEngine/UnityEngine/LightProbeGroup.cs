// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightProbeGroup
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Light Probe Group.</para>
  /// </summary>
  public sealed class LightProbeGroup : Behaviour
  {
    /// <summary>
    ///   <para>Editor only function to access and modify probe positions.</para>
    /// </summary>
    public Vector3[] probePositions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
