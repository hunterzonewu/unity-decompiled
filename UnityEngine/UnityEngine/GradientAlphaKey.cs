// Decompiled with JetBrains decompiler
// Type: UnityEngine.GradientAlphaKey
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Alpha key used by Gradient.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct GradientAlphaKey
  {
    /// <summary>
    ///   <para>Alpha channel of key.</para>
    /// </summary>
    public float alpha;
    /// <summary>
    ///   <para>Time of the key (0 - 1).</para>
    /// </summary>
    public float time;

    /// <summary>
    ///   <para>Gradient alpha key.</para>
    /// </summary>
    /// <param name="alpha">Alpha of key (0 - 1).</param>
    /// <param name="time">Time of the key (0 - 1).</param>
    public GradientAlphaKey(float alpha, float time)
    {
      this.alpha = alpha;
      this.time = time;
    }
  }
}
