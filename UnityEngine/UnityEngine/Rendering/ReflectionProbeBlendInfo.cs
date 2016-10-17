// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.ReflectionProbeBlendInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
  /// <summary>
  ///   <para>ReflectionProbeBlendInfo contains information required for blending probes.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct ReflectionProbeBlendInfo
  {
    /// <summary>
    ///   <para>Reflection Probe used in blending.</para>
    /// </summary>
    public ReflectionProbe probe;
    /// <summary>
    ///   <para>Specifies the weight used in the interpolation between two probes, value varies from 0.0 to 1.0.</para>
    /// </summary>
    public float weight;
  }
}
