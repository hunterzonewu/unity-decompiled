// Decompiled with JetBrains decompiler
// Type: UnityEditor.PreferenceItem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The PreferenceItem attribute allows you to add preferences sections to the Preferences Window.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class PreferenceItem : Attribute
  {
    public string name;

    /// <summary>
    ///   <para>Creates a section in the Preferences Window called name and invokes the static function following it for the section's GUI.</para>
    /// </summary>
    /// <param name="name"></param>
    public PreferenceItem(string name)
    {
      this.name = name;
    }
  }
}
