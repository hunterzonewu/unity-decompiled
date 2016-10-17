// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.OnlineState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>Represent the connection state of the version control provider.</para>
  /// </summary>
  [System.Flags]
  public enum OnlineState
  {
    Updating = 0,
    Online = 1,
    Offline = 2,
  }
}
