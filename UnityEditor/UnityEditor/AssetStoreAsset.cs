// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreAsset
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  public sealed class AssetStoreAsset
  {
    public int id;
    public string name;
    public string displayName;
    public string staticPreviewURL;
    public string dynamicPreviewURL;
    public string className;
    public string price;
    public int packageID;
    internal AssetStoreAsset.PreviewInfo previewInfo;
    public Texture2D previewImage;
    internal AssetBundleCreateRequest previewBundleRequest;
    internal AssetBundle previewBundle;
    internal Object previewAsset;
    internal bool disposed;

    public Object Preview
    {
      get
      {
        if (this.previewAsset != (Object) null)
          return this.previewAsset;
        return (Object) this.previewImage;
      }
    }

    public bool HasLivePreview
    {
      get
      {
        return this.previewAsset != (Object) null;
      }
    }

    internal string DebugString
    {
      get
      {
        string str = string.Format("id: {0}\nname: {1}\nstaticPreviewURL: {2}\ndynamicPreviewURL: {3}\nclassName: {4}\nprice: {5}\npackageID: {6}", (object) this.id, (object) (this.name ?? "N/A"), (object) (this.staticPreviewURL ?? "N/A"), (object) (this.dynamicPreviewURL ?? "N/A"), (object) (this.className ?? "N/A"), (object) this.price, (object) this.packageID);
        if (this.previewInfo != null)
          str += string.Format("previewInfo {{\n    packageName: {0}\n    packageShortUrl: {1}\n    packageSize: {2}\n    packageVersion: {3}\n    packageRating: {4}\n    packageAssetCount: {5}\n    isPurchased: {6}\n    isDownloadable: {7}\n    publisherName: {8}\n    encryptionKey: {9}\n    packageUrl: {10}\n    buildProgress: {11}\n    downloadProgress: {12}\n    categoryName: {13}\n}}", (object) (this.previewInfo.packageName ?? "N/A"), (object) (this.previewInfo.packageShortUrl ?? "N/A"), (object) this.previewInfo.packageSize, (object) (this.previewInfo.packageVersion ?? "N/A"), (object) this.previewInfo.packageRating, (object) this.previewInfo.packageAssetCount, (object) this.previewInfo.isPurchased, (object) this.previewInfo.isDownloadable, (object) (this.previewInfo.publisherName ?? "N/A"), (object) (this.previewInfo.encryptionKey ?? "N/A"), (object) (this.previewInfo.packageUrl ?? "N/A"), (object) this.previewInfo.buildProgress, (object) this.previewInfo.downloadProgress, (object) (this.previewInfo.categoryName ?? "N/A"));
        return str;
      }
    }

    public AssetStoreAsset()
    {
      this.disposed = false;
    }

    public void Dispose()
    {
      if ((Object) this.previewImage != (Object) null)
      {
        Object.DestroyImmediate((Object) this.previewImage);
        this.previewImage = (Texture2D) null;
      }
      if ((Object) this.previewBundle != (Object) null)
      {
        this.previewBundle.Unload(true);
        this.previewBundle = (AssetBundle) null;
        this.previewAsset = (Object) null;
      }
      this.disposed = true;
    }

    internal class PreviewInfo
    {
      public string packageName;
      public string packageShortUrl;
      public int packageSize;
      public string packageVersion;
      public int packageRating;
      public int packageAssetCount;
      public bool isPurchased;
      public bool isDownloadable;
      public string publisherName;
      public string encryptionKey;
      public string packageUrl;
      public float buildProgress;
      public float downloadProgress;
      public string categoryName;
    }
  }
}
