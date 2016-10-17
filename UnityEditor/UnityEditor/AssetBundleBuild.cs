// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetBundleBuild
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>AssetBundle building map entry.</para>
  /// </summary>
  public struct AssetBundleBuild
  {
    /// <summary>
    ///   <para>AssetBundle name.</para>
    /// </summary>
    public string assetBundleName;
    /// <summary>
    ///   <para>AssetBundle variant.</para>
    /// </summary>
    public string assetBundleVariant;
    /// <summary>
    ///   <para>Asset names which belong to the given AssetBundle.</para>
    /// </summary>
    public string[] assetNames;
  }
}
