// Decompiled with JetBrains decompiler
// Type: UnityEditor.ExportPackageOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Export package option. Multiple options can be combined together using the | operator.</para>
  /// </summary>
  [System.Flags]
  public enum ExportPackageOptions
  {
    Default = 0,
    Interactive = 1,
    Recurse = 2,
    IncludeDependencies = 4,
    IncludeLibraryAssets = 8,
  }
}
