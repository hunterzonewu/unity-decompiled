// Decompiled with JetBrains decompiler
// Type: UnityEngine.AssetBundle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngineInternal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>AssetBundles let you stream additional assets via the WWW class and instantiate them at runtime. AssetBundles are created via BuildPipeline.BuildAssetBundle.</para>
  /// </summary>
  public sealed class AssetBundle : Object
  {
    /// <summary>
    ///   <para>Main asset that was supplied when building the asset bundle (Read Only).</para>
    /// </summary>
    public Object mainAsset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Return true if the AssetBundle is a streamed scene AssetBundle.</para>
    /// </summary>
    public bool isStreamedSceneAssetBundle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AssetBundleCreateRequest LoadFromFileAsync(string path, [DefaultValue("0")] uint crc, [DefaultValue("0")] ulong offset);

    /// <summary>
    ///   <para>Asynchronously loads an AssetBundle from a file on disk.</para>
    /// </summary>
    /// <param name="path">Path of the file on disk.</param>
    /// <param name="crc">An optional CRC-32 checksum of the uncompressed content. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match.</param>
    /// <param name="offset">An optional byte offset. This value specifies where to start reading the AssetBundle from.</param>
    /// <returns>
    ///   <para>Asynchronous create request for an AssetBundle. Use AssetBundleCreateRequest.assetBundle property to get an AssetBundle once it is loaded.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static AssetBundleCreateRequest LoadFromFileAsync(string path, uint crc)
    {
      ulong offset = 0;
      return AssetBundle.LoadFromFileAsync(path, crc, offset);
    }

    [ExcludeFromDocs]
    public static AssetBundleCreateRequest LoadFromFileAsync(string path)
    {
      ulong offset = 0;
      uint crc = 0;
      return AssetBundle.LoadFromFileAsync(path, crc, offset);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AssetBundle LoadFromFile(string path, [DefaultValue("0")] uint crc, [DefaultValue("0")] ulong offset);

    /// <summary>
    ///   <para>Synchronously loads an AssetBundle from a file on disk.</para>
    /// </summary>
    /// <param name="path">Path of the file on disk.</param>
    /// <param name="crc">An optional CRC-32 checksum of the uncompressed content. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match.</param>
    /// <param name="offset">An optional byte offset. This value specifies where to start reading the AssetBundle from.</param>
    /// <returns>
    ///   <para>Loaded AssetBundle object or null if failed.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static AssetBundle LoadFromFile(string path, uint crc)
    {
      ulong offset = 0;
      return AssetBundle.LoadFromFile(path, crc, offset);
    }

    [ExcludeFromDocs]
    public static AssetBundle LoadFromFile(string path)
    {
      ulong offset = 0;
      uint crc = 0;
      return AssetBundle.LoadFromFile(path, crc, offset);
    }

    /// <summary>
    ///   <para>Asynchronously create an AssetBundle from a memory region.</para>
    /// </summary>
    /// <param name="binary">Array of bytes with the AssetBundle data.</param>
    /// <param name="crc">An optional CRC-32 checksum of the uncompressed content. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match.</param>
    /// <returns>
    ///   <para>Asynchronous create request for an AssetBundle. Use AssetBundleCreateRequest.assetBundle property to get an AssetBundle once it is loaded.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AssetBundleCreateRequest LoadFromMemoryAsync(byte[] binary, [DefaultValue("0")] uint crc);

    [ExcludeFromDocs]
    public static AssetBundleCreateRequest LoadFromMemoryAsync(byte[] binary)
    {
      uint crc = 0;
      return AssetBundle.LoadFromMemoryAsync(binary, crc);
    }

    /// <summary>
    ///   <para>Synchronously create an AssetBundle from a memory region.</para>
    /// </summary>
    /// <param name="binary">Array of bytes with the AssetBundle data.</param>
    /// <param name="crc">An optional CRC-32 checksum of the uncompressed content. If this is non-zero, then the content will be compared against the checksum before loading it, and give an error if it does not match.</param>
    /// <returns>
    ///   <para>Loaded AssetBundle object or null if failed.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern AssetBundle LoadFromMemory(byte[] binary, [DefaultValue("0")] uint crc);

    [ExcludeFromDocs]
    public static AssetBundle LoadFromMemory(byte[] binary)
    {
      uint crc = 0;
      return AssetBundle.LoadFromMemory(binary, crc);
    }

    /// <summary>
    ///   <para>Check if an AssetBundle contains a specific object.</para>
    /// </summary>
    /// <param name="name"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool Contains(string name);

    [Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
    public Object Load(string name)
    {
      return (Object) null;
    }

    [Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
    public T Load<T>(string name) where T : Object
    {
      return (T) null;
    }

    [Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
    [WrapperlessIcall]
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Object Load(string name, System.Type type);

    [WrapperlessIcall]
    [Obsolete("Method LoadAsync has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAssetAsync instead and check the documentation for details.", true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public AssetBundleRequest LoadAsync(string name, System.Type type);

    [WrapperlessIcall]
    [Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Object[] LoadAll(System.Type type);

    [Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
    public Object[] LoadAll()
    {
      return (Object[]) null;
    }

    [Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
    public T[] LoadAll<T>() where T : Object
    {
      return (T[]) null;
    }

    /// <summary>
    ///   <para>Loads asset with name of type T from the bundle.</para>
    /// </summary>
    /// <param name="name"></param>
    public Object LoadAsset(string name)
    {
      return this.LoadAsset(name, typeof (Object));
    }

    public T LoadAsset<T>(string name) where T : Object
    {
      return (T) this.LoadAsset(name, typeof (T));
    }

    /// <summary>
    ///   <para>Loads asset with name of a given type from the bundle.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
    public Object LoadAsset(string name, System.Type type)
    {
      if (name == null)
        throw new NullReferenceException("The input asset name cannot be null.");
      if (name.Length == 0)
        throw new ArgumentException("The input asset name cannot be empty.");
      if (type == null)
        throw new NullReferenceException("The input type cannot be null.");
      return this.LoadAsset_Internal(name, type);
    }

    [WrapperlessIcall]
    [TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Object LoadAsset_Internal(string name, System.Type type);

    /// <summary>
    ///   <para>Asynchronously loads asset with name of a given T from the bundle.</para>
    /// </summary>
    /// <param name="name"></param>
    public AssetBundleRequest LoadAssetAsync(string name)
    {
      return this.LoadAssetAsync(name, typeof (Object));
    }

    public AssetBundleRequest LoadAssetAsync<T>(string name)
    {
      return this.LoadAssetAsync(name, typeof (T));
    }

    /// <summary>
    ///   <para>Asynchronously loads asset with name of a given type from the bundle.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public AssetBundleRequest LoadAssetAsync(string name, System.Type type)
    {
      if (name == null)
        throw new NullReferenceException("The input asset name cannot be null.");
      if (name.Length == 0)
        throw new ArgumentException("The input asset name cannot be empty.");
      if (type == null)
        throw new NullReferenceException("The input type cannot be null.");
      return this.LoadAssetAsync_Internal(name, type);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private AssetBundleRequest LoadAssetAsync_Internal(string name, System.Type type);

    /// <summary>
    ///   <para>Loads asset and sub assets with name from the bundle.</para>
    /// </summary>
    /// <param name="name"></param>
    public Object[] LoadAssetWithSubAssets(string name)
    {
      return this.LoadAssetWithSubAssets(name, typeof (Object));
    }

    public T[] LoadAssetWithSubAssets<T>(string name) where T : Object
    {
      return Resources.ConvertObjects<T>(this.LoadAssetWithSubAssets(name, typeof (T)));
    }

    /// <summary>
    ///   <para>Loads asset and sub assets with name of a given type from the bundle.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public Object[] LoadAssetWithSubAssets(string name, System.Type type)
    {
      if (name == null)
        throw new NullReferenceException("The input asset name cannot be null.");
      if (name.Length == 0)
        throw new ArgumentException("The input asset name cannot be empty.");
      if (type == null)
        throw new NullReferenceException("The input type cannot be null.");
      return this.LoadAssetWithSubAssets_Internal(name, type);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal Object[] LoadAssetWithSubAssets_Internal(string name, System.Type type);

    /// <summary>
    ///   <para>Loads asset with sub assets with name from the bundle asynchronously.</para>
    /// </summary>
    /// <param name="name"></param>
    public AssetBundleRequest LoadAssetWithSubAssetsAsync(string name)
    {
      return this.LoadAssetWithSubAssetsAsync(name, typeof (Object));
    }

    public AssetBundleRequest LoadAssetWithSubAssetsAsync<T>(string name)
    {
      return this.LoadAssetWithSubAssetsAsync(name, typeof (T));
    }

    /// <summary>
    ///   <para>Loads asset with sub assets with name of a given type from the bundle asynchronously.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public AssetBundleRequest LoadAssetWithSubAssetsAsync(string name, System.Type type)
    {
      if (name == null)
        throw new NullReferenceException("The input asset name cannot be null.");
      if (name.Length == 0)
        throw new ArgumentException("The input asset name cannot be empty.");
      if (type == null)
        throw new NullReferenceException("The input type cannot be null.");
      return this.LoadAssetWithSubAssetsAsync_Internal(name, type);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private AssetBundleRequest LoadAssetWithSubAssetsAsync_Internal(string name, System.Type type);

    /// <summary>
    ///   <para>Loads all assets contained in the asset bundle that inherit from type T.</para>
    /// </summary>
    public Object[] LoadAllAssets()
    {
      return this.LoadAllAssets(typeof (Object));
    }

    public T[] LoadAllAssets<T>() where T : Object
    {
      return Resources.ConvertObjects<T>(this.LoadAllAssets(typeof (T)));
    }

    /// <summary>
    ///   <para>Loads all assets contained in the asset bundle that inherit from type.</para>
    /// </summary>
    /// <param name="type"></param>
    public Object[] LoadAllAssets(System.Type type)
    {
      if (type == null)
        throw new NullReferenceException("The input type cannot be null.");
      return this.LoadAssetWithSubAssets_Internal(string.Empty, type);
    }

    /// <summary>
    ///   <para>Loads all assets contained in the asset bundle that inherit from T asynchronously.</para>
    /// </summary>
    public AssetBundleRequest LoadAllAssetsAsync()
    {
      return this.LoadAllAssetsAsync(typeof (Object));
    }

    public AssetBundleRequest LoadAllAssetsAsync<T>()
    {
      return this.LoadAllAssetsAsync(typeof (T));
    }

    /// <summary>
    ///   <para>Loads all assets contained in the asset bundle that inherit from type asynchronously.</para>
    /// </summary>
    /// <param name="type"></param>
    public AssetBundleRequest LoadAllAssetsAsync(System.Type type)
    {
      if (type == null)
        throw new NullReferenceException("The input type cannot be null.");
      return this.LoadAssetWithSubAssetsAsync_Internal(string.Empty, type);
    }

    /// <summary>
    ///   <para>Unloads all assets in the bundle.</para>
    /// </summary>
    /// <param name="unloadAllLoadedObjects"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Unload(bool unloadAllLoadedObjects);

    [Obsolete("This method is deprecated. Use GetAllAssetNames() instead.")]
    public string[] AllAssetNames()
    {
      return this.GetAllAssetNames();
    }

    /// <summary>
    ///   <para>Return all asset names in the AssetBundle.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string[] GetAllAssetNames();

    /// <summary>
    ///   <para>Return all the scene asset paths (paths to *.unity assets) in the AssetBundle.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string[] GetAllScenePaths();

    /// <summary>
    ///   <para>Loads an asset bundle from a disk.</para>
    /// </summary>
    /// <param name="path">Path of the file on disk
    /// 
    /// See Also: WWW.assetBundle, WWW.LoadFromCacheOrDownload.</param>
    [Obsolete("Method CreateFromFile has been renamed to LoadFromFile (UnityUpgradable) -> LoadFromFile(*)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static AssetBundle CreateFromFile(string path)
    {
      return (AssetBundle) null;
    }

    /// <summary>
    ///   <para>Asynchronously create an AssetBundle from a memory region.</para>
    /// </summary>
    /// <param name="binary"></param>
    [Obsolete("Method CreateFromMemory has been renamed to LoadFromMemoryAsync (UnityUpgradable) -> LoadFromMemoryAsync(*)", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static AssetBundleCreateRequest CreateFromMemory(byte[] binary)
    {
      return (AssetBundleCreateRequest) null;
    }

    /// <summary>
    ///   <para>Synchronously create an AssetBundle from a memory region.</para>
    /// </summary>
    /// <param name="binary">Array of bytes with the AssetBundle data.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Method CreateFromMemoryImmediate has been renamed to LoadFromMemory (UnityUpgradable) -> LoadFromMemory(*)", true)]
    public static AssetBundle CreateFromMemoryImmediate(byte[] binary)
    {
      return (AssetBundle) null;
    }
  }
}
