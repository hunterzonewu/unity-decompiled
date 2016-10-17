// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreAssetSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal static class AssetStoreAssetSelection
  {
    internal static Dictionary<int, AssetStoreAsset> s_SelectedAssets;

    public static int Count
    {
      get
      {
        if (AssetStoreAssetSelection.s_SelectedAssets == null)
          return 0;
        return AssetStoreAssetSelection.s_SelectedAssets.Count;
      }
    }

    public static bool Empty
    {
      get
      {
        if (AssetStoreAssetSelection.s_SelectedAssets == null)
          return true;
        return AssetStoreAssetSelection.s_SelectedAssets.Count == 0;
      }
    }

    public static void AddAsset(AssetStoreAsset searchResult, Texture2D placeholderPreviewImage)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreAssetSelection.\u003CAddAsset\u003Ec__AnonStorey4E assetCAnonStorey4E = new AssetStoreAssetSelection.\u003CAddAsset\u003Ec__AnonStorey4E();
      // ISSUE: reference to a compiler-generated field
      assetCAnonStorey4E.searchResult = searchResult;
      if ((Object) placeholderPreviewImage != (Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey4E.searchResult.previewImage = AssetStoreAssetSelection.ScaleImage(placeholderPreviewImage, 256, 256);
      }
      // ISSUE: reference to a compiler-generated field
      assetCAnonStorey4E.searchResult.previewInfo = (AssetStoreAsset.PreviewInfo) null;
      // ISSUE: reference to a compiler-generated field
      assetCAnonStorey4E.searchResult.previewBundleRequest = (AssetBundleCreateRequest) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!string.IsNullOrEmpty(assetCAnonStorey4E.searchResult.dynamicPreviewURL) && (Object) assetCAnonStorey4E.searchResult.previewBundle == (Object) null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AssetStoreAssetSelection.\u003CAddAsset\u003Ec__AnonStorey4D assetCAnonStorey4D = new AssetStoreAssetSelection.\u003CAddAsset\u003Ec__AnonStorey4D();
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey4D.\u003C\u003Ef__ref\u002478 = assetCAnonStorey4E;
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey4E.searchResult.disposed = false;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey4D.client = new AsyncHTTPClient(assetCAnonStorey4E.searchResult.dynamicPreviewURL);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        assetCAnonStorey4D.client.doneCallback = new AsyncHTTPClient.DoneCallback(assetCAnonStorey4D.\u003C\u003Em__8A);
        // ISSUE: reference to a compiler-generated field
        assetCAnonStorey4D.client.Begin();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (!string.IsNullOrEmpty(assetCAnonStorey4E.searchResult.staticPreviewURL))
        {
          // ISSUE: reference to a compiler-generated field
          AssetStoreAssetSelection.DownloadStaticPreview(assetCAnonStorey4E.searchResult);
        }
      }
      // ISSUE: reference to a compiler-generated field
      AssetStoreAssetSelection.AddAssetInternal(assetCAnonStorey4E.searchResult);
      AssetStoreAssetSelection.RefreshFromServer((AssetStoreAssetSelection.AssetsRefreshed) null);
    }

    internal static void AddAssetInternal(AssetStoreAsset searchResult)
    {
      if (AssetStoreAssetSelection.s_SelectedAssets == null)
        AssetStoreAssetSelection.s_SelectedAssets = new Dictionary<int, AssetStoreAsset>();
      AssetStoreAssetSelection.s_SelectedAssets[searchResult.id] = searchResult;
    }

    private static void DownloadStaticPreview(AssetStoreAsset searchResult)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreAssetSelection.\u003CDownloadStaticPreview\u003Ec__AnonStorey50 previewCAnonStorey50 = new AssetStoreAssetSelection.\u003CDownloadStaticPreview\u003Ec__AnonStorey50();
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey50.searchResult = searchResult;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey50.client = new AsyncHTTPClient(previewCAnonStorey50.searchResult.staticPreviewURL);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      previewCAnonStorey50.client.doneCallback = new AsyncHTTPClient.DoneCallback(previewCAnonStorey50.\u003C\u003Em__8B);
      // ISSUE: reference to a compiler-generated field
      previewCAnonStorey50.client.Begin();
    }

    public static void RefreshFromServer(AssetStoreAssetSelection.AssetsRefreshed callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreAssetSelection.\u003CRefreshFromServer\u003Ec__AnonStorey51 serverCAnonStorey51 = new AssetStoreAssetSelection.\u003CRefreshFromServer\u003Ec__AnonStorey51();
      // ISSUE: reference to a compiler-generated field
      serverCAnonStorey51.callback = callback;
      if (AssetStoreAssetSelection.s_SelectedAssets.Count == 0)
        return;
      List<AssetStoreAsset> assets = new List<AssetStoreAsset>();
      using (Dictionary<int, AssetStoreAsset>.Enumerator enumerator = AssetStoreAssetSelection.s_SelectedAssets.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AssetStoreAsset> current = enumerator.Current;
          assets.Add(current.Value);
        }
      }
      // ISSUE: reference to a compiler-generated method
      AssetStoreClient.AssetsInfo(assets, new AssetStoreResultBase<AssetStoreAssetsInfo>.Callback(serverCAnonStorey51.\u003C\u003Em__8C));
    }

    private static Texture2D ScaleImage(Texture2D source, int w, int h)
    {
      if (source.width % 4 != 0)
        return (Texture2D) null;
      Texture2D texture2D = new Texture2D(w, h, TextureFormat.RGB24, false, true);
      Color[] pixels = texture2D.GetPixels(0);
      double num1 = 1.0 / (double) w;
      double num2 = 1.0 / (double) h;
      double num3 = 0.0;
      double num4 = 0.0;
      int index1 = 0;
      for (int index2 = 0; index2 < h; ++index2)
      {
        int num5 = 0;
        while (num5 < w)
        {
          pixels[index1] = source.GetPixelBilinear((float) num3, (float) num4);
          num3 += num1;
          ++num5;
          ++index1;
        }
        num3 = 0.0;
        num4 += num2;
      }
      texture2D.SetPixels(pixels, 0);
      texture2D.Apply();
      return texture2D;
    }

    public static bool ContainsAsset(int id)
    {
      if (AssetStoreAssetSelection.s_SelectedAssets != null)
        return AssetStoreAssetSelection.s_SelectedAssets.ContainsKey(id);
      return false;
    }

    public static void Clear()
    {
      if (AssetStoreAssetSelection.s_SelectedAssets == null)
        return;
      using (Dictionary<int, AssetStoreAsset>.Enumerator enumerator = AssetStoreAssetSelection.s_SelectedAssets.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Value.Dispose();
      }
      AssetStoreAssetSelection.s_SelectedAssets.Clear();
    }

    public static AssetStoreAsset GetFirstAsset()
    {
      if (AssetStoreAssetSelection.s_SelectedAssets == null)
        return (AssetStoreAsset) null;
      Dictionary<int, AssetStoreAsset>.Enumerator enumerator = AssetStoreAssetSelection.s_SelectedAssets.GetEnumerator();
      if (!enumerator.MoveNext())
        return (AssetStoreAsset) null;
      return enumerator.Current.Value;
    }

    public delegate void AssetsRefreshed();
  }
}
