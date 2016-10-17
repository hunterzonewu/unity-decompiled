// Decompiled with JetBrains decompiler
// Type: UnityEngine.HeaderAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Use this PropertyAttribute to add a header above some fields in the Inspector.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
  public class HeaderAttribute : PropertyAttribute
  {
    /// <summary>
    ///   <para>The header text.</para>
    /// </summary>
    public readonly string header;

    /// <summary>
    ///   <para>Add a header above some fields in the Inspector.</para>
    /// </summary>
    /// <param name="header">The header text.</param>
    public HeaderAttribute(string header)
    {
      this.header = header;
    }
  }
}
