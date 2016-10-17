// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildAssetBundleOptions
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Asset Bundle building options.</para>
  /// </summary>
  [System.Flags]
  public enum BuildAssetBundleOptions
  {
    None = 0,
    UncompressedAssetBundle = 1,
    [Obsolete("This has been made obsolete. It is always enabled in the new AssetBundle build system introduced in 5.0.")] CollectDependencies = 2,
    [Obsolete("This has been made obsolete. It is always enabled in the new AssetBundle build system introduced in 5.0.")] CompleteAssets = 4,
    DisableWriteTypeTree = 8,
    DeterministicAssetBundle = 16,
    ForceRebuildAssetBundle = 32,
    IgnoreTypeTreeChanges = 64,
    AppendHashToAssetBundleName = 128,
    ChunkBasedCompression = 256,
  }
}
