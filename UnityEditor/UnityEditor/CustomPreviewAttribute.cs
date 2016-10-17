// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomPreviewAttribute
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Adds an extra preview in the Inspector for the specified type.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public sealed class CustomPreviewAttribute : Attribute
  {
    internal System.Type m_Type;

    /// <summary>
    ///   <para>Tells a DefaultPreview which class it's a preview for.</para>
    /// </summary>
    /// <param name="type">The type you want to create a custom preview for.</param>
    public CustomPreviewAttribute(System.Type type)
    {
      this.m_Type = type;
    }
  }
}
