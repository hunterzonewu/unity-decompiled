// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.MergeMethod
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Which method to use when merging.</para>
  /// </summary>
  [System.Flags]
  public enum MergeMethod
  {
    MergeNone = 0,
    MergeAll = 1,
    [Obsolete("This member is no longer supported (UnityUpgradable) -> MergeNone", true)] MergeNonConflicting = 2,
  }
}
