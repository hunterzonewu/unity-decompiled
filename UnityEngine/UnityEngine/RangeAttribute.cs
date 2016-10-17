// Decompiled with JetBrains decompiler
// Type: UnityEngine.RangeAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Attribute used to make a float or int variable in a script be restricted to a specific range.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class RangeAttribute : PropertyAttribute
  {
    public readonly float min;
    public readonly float max;

    /// <summary>
    ///   <para>Attribute used to make a float or int variable in a script be restricted to a specific range.</para>
    /// </summary>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="max">The maximum allowed value.</param>
    public RangeAttribute(float min, float max)
    {
      this.min = min;
      this.max = max;
    }
  }
}
