// Decompiled with JetBrains decompiler
// Type: UnityEngine.MultilineAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Attribute to make a string be edited with a multi-line textfield.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class MultilineAttribute : PropertyAttribute
  {
    public readonly int lines;

    /// <summary>
    ///   <para>Attribute used to make a string value be shown in a multiline textarea.</para>
    /// </summary>
    /// <param name="lines">How many lines of text to make room for. Default is 3.</param>
    public MultilineAttribute()
    {
      this.lines = 3;
    }

    /// <summary>
    ///   <para>Attribute used to make a string value be shown in a multiline textarea.</para>
    /// </summary>
    /// <param name="lines">How many lines of text to make room for. Default is 3.</param>
    public MultilineAttribute(int lines)
    {
      this.lines = lines;
    }
  }
}
