// Decompiled with JetBrains decompiler
// Type: UnityEngine.AssetBundleManifest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Manifest for all the assetBundle in the build.</para>
  /// </summary>
  public sealed class AssetBundleManifest : Object
  {
    /// <summary>
    ///   <para>Get all the AssetBundles in the manifest.</para>
    /// </summary>
    /// <returns>
    ///   <para>An array of asset bundle names.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string[] GetAllAssetBundles();

    /// <summary>
    ///   <para>Get all the AssetBundles with variant in the manifest.</para>
    /// </summary>
    /// <returns>
    ///   <para>An array of asset bundle names.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string[] GetAllAssetBundlesWithVariant();

    /// <summary>
    ///   <para>Get the hash for the given AssetBundle.</para>
    /// </summary>
    /// <param name="assetBundleName">Name of the asset bundle.</param>
    /// <returns>
    ///   <para>The 128-bit hash for the asset bundle.</para>
    /// </returns>
    public Hash128 GetAssetBundleHash(string assetBundleName)
    {
      Hash128 hash128;
      AssetBundleManifest.INTERNAL_CALL_GetAssetBundleHash(this, assetBundleName, out hash128);
      return hash128;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetAssetBundleHash(AssetBundleManifest self, string assetBundleName, out Hash128 value);

    /// <summary>
    ///   <para>Get the direct dependent AssetBundles for the given AssetBundle.</para>
    /// </summary>
    /// <param name="assetBundleName">Name of the asset bundle.</param>
    /// <returns>
    ///   <para>Array of asset bundle names this asset bundle depends on.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string[] GetDirectDependencies(string assetBundleName);

    /// <summary>
    ///   <para>Get all the dependent AssetBundles for the given AssetBundle.</para>
    /// </summary>
    /// <param name="assetBundleName">Name of the asset bundle.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string[] GetAllDependencies(string assetBundleName);
  }
}
