// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.RevertMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Defines the behaviour of the version control revert methods.</para>
  /// </summary>
  [System.Flags]
  public enum RevertMode
  {
    Normal = 0,
    Unchanged = 1,
    KeepModifications = 2,
  }
}
