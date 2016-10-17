// Decompiled with JetBrains decompiler
// Type: UnityEngine.TextAreaAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Attribute to make a string be edited with a height-flexible and scrollable text area.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class TextAreaAttribute : PropertyAttribute
  {
    /// <summary>
    ///   <para>The minimum amount of lines the text area will use.</para>
    /// </summary>
    public readonly int minLines;
    /// <summary>
    ///   <para>The maximum amount of lines the text area can show before it starts using a scrollbar.</para>
    /// </summary>
    public readonly int maxLines;

    /// <summary>
    ///   <para>Attribute to make a string be edited with a height-flexible and scrollable text area.</para>
    /// </summary>
    /// <param name="minLines">The minimum amount of lines the text area will use.</param>
    /// <param name="maxLines">The maximum amount of lines the text area can show before it starts using a scrollbar.</param>
    public TextAreaAttribute()
    {
      this.minLines = 3;
      this.maxLines = 3;
    }

    /// <summary>
    ///   <para>Attribute to make a string be edited with a height-flexible and scrollable text area.</para>
    /// </summary>
    /// <param name="minLines">The minimum amount of lines the text area will use.</param>
    /// <param name="maxLines">The maximum amount of lines the text area can show before it starts using a scrollbar.</param>
    public TextAreaAttribute(int minLines, int maxLines)
    {
      this.minLines = minLines;
      this.maxLines = maxLines;
    }
  }
}
