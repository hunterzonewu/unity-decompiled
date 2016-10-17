// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetPreview
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Utility for fetching asset previews by instance ID of assets, See AssetPreview.GetAssetPreview. Since previews are loaded asynchronously methods are provided for requesting if all previews have been fully loaded, see AssetPreview.IsLoadingAssetPreviews. Loaded previews are stored in a cache, the size of the cache can be controlled by calling [AssetPreview.SetPreviewTextureCacheSize].</para>
  /// </summary>
  public sealed class AssetPreview
  {
    private const int kSharedClientID = 0;

    /// <summary>
    ///   <para>Returns a preview texture for an asset.</para>
    /// </summary>
    /// <param name="asset"></param>
    public static Texture2D GetAssetPreview(UnityEngine.Object asset)
    {
      if (asset != (UnityEngine.Object) null)
        return AssetPreview.GetAssetPreview(asset.GetInstanceID());
      return (Texture2D) null;
    }

    internal static Texture2D GetAssetPreview(int instanceID)
    {
      return AssetPreview.GetAssetPreview(instanceID, 0);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D GetAssetPreview(int instanceID, int clientID);

    /// <summary>
    ///   <para>Loading previews is asynchronous so it is useful to know if a specific asset preview is in the process of being loaded so client code e.g can repaint while waiting for the loading to finish.</para>
    /// </summary>
    /// <param name="instanceID">InstanceID of the assset that a preview has been requested for by: AssetPreview.GetAssetPreview().</param>
    public static bool IsLoadingAssetPreview(int instanceID)
    {
      return AssetPreview.IsLoadingAssetPreview(instanceID, 0);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsLoadingAssetPreview(int instanceID, int clientID);

    /// <summary>
    ///   <para>Loading previews is asynchronous so it is useful to know if any requested previews are in the process of being loaded so client code e.g can repaint while waiting.</para>
    /// </summary>
    public static bool IsLoadingAssetPreviews()
    {
      return AssetPreview.IsLoadingAssetPreviews(0);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsLoadingAssetPreviews(int clientID);

    internal static bool HasAnyNewPreviewTexturesAvailable()
    {
      return AssetPreview.HasAnyNewPreviewTexturesAvailable(0);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool HasAnyNewPreviewTexturesAvailable(int clientID);

    /// <summary>
    ///   <para>Set the asset preview cache to a size that can hold all visible previews on the screen at once.</para>
    /// </summary>
    /// <param name="size">The number of previews that can be loaded into the cache before the least used previews are being unloaded.</param>
    public static void SetPreviewTextureCacheSize(int size)
    {
      AssetPreview.SetPreviewTextureCacheSize(size, 0);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPreviewTextureCacheSize(int size, int clientID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ClearTemporaryAssetPreviews();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void DeletePreviewTextureManagerByID(int clientID);

    /// <summary>
    ///   <para>Returns the thumbnail for an object (like the ones you see in the project view).</para>
    /// </summary>
    /// <param name="obj"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Texture2D GetMiniThumbnail(UnityEngine.Object obj);

    /// <summary>
    ///   <para>Returns the thumbnail for the type.</para>
    /// </summary>
    /// <param name="type"></param>
    public static Texture2D GetMiniTypeThumbnail(System.Type type)
    {
      return !typeof (MonoBehaviour).IsAssignableFrom(type) ? AssetPreview.INTERNAL_GetMiniTypeThumbnailFromType(type) : EditorGUIUtility.LoadIcon(type.FullName.Replace('.', '/') + " Icon");
    }

    internal static Texture2D GetMiniTypeThumbnail(UnityEngine.Object obj)
    {
      return AssetPreview.INTERNAL_GetMiniTypeThumbnailFromObject(obj);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D GetMiniTypeThumbnailFromClassID(int classID);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D INTERNAL_GetMiniTypeThumbnailFromObject(UnityEngine.Object monoObj);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Texture2D INTERNAL_GetMiniTypeThumbnailFromType(System.Type type);
  }
}
