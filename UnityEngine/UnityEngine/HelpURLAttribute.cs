// Decompiled with JetBrains decompiler
// Type: UnityEngine.HelpURLAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Provide a custom documentation URL for a class.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class HelpURLAttribute : Attribute
  {
    /// <summary>
    ///   <para>The documentation URL specified for this class.</para>
    /// </summary>
    public string URL { get; private set; }

    /// <summary>
    ///   <para>Initialize the HelpURL attribute with a documentation url.</para>
    /// </summary>
    /// <param name="url">The custom documentation URL for this class.</param>
    public HelpURLAttribute(string url)
    {
      this.URL = url;
    }
  }
}
