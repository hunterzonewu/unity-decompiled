// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetDeleteResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Result of Asset delete operation</para>
  /// </summary>
  [System.Flags]
  public enum AssetDeleteResult
  {
    DidNotDelete = 0,
    FailedDelete = 1,
    DidDelete = 2,
  }
}
