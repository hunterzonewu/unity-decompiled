// Decompiled with JetBrains decompiler
// Type: UnityEngine.TooltipAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Specify a tooltip for a field.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class TooltipAttribute : PropertyAttribute
  {
    /// <summary>
    ///   <para>The tooltip text.</para>
    /// </summary>
    public readonly string tooltip;

    /// <summary>
    ///   <para>Specify a tooltip for a field.</para>
    /// </summary>
    /// <param name="tooltip">The tooltip text.</param>
    public TooltipAttribute(string tooltip)
    {
      this.tooltip = tooltip;
    }
  }
}
