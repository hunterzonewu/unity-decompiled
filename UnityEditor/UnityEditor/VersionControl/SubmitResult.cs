// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.SubmitResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>The status of an operation returned by the VCS.</para>
  /// </summary>
  public enum SubmitResult
  {
    OK = 1,
    Error = 2,
    ConflictingFiles = 4,
    UnaddedFiles = 8,
  }
}
