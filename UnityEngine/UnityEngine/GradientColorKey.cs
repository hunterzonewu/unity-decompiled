// Decompiled with JetBrains decompiler
// Type: UnityEngine.GradientColorKey
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Color key used by Gradient.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct GradientColorKey
  {
    /// <summary>
    ///   <para>Color of key.</para>
    /// </summary>
    public Color color;
    /// <summary>
    ///   <para>Time of the key (0 - 1).</para>
    /// </summary>
    public float time;

    /// <summary>
    ///   <para>Gradient color key.</para>
    /// </summary>
    /// <param name="color">Color of key.</param>
    /// <param name="time">Time of the key (0 - 1).</param>
    /// <param name="col"></param>
    public GradientColorKey(Color col, float time)
    {
      this.color = col;
      this.time = time;
    }
  }
}
