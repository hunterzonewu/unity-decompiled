// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildPackageResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class BuildPackageResult : AssetStoreResultBase<BuildPackageResult>
  {
    internal AssetStoreAsset asset;
    internal int packageID;

    internal BuildPackageResult(AssetStoreAsset asset, AssetStoreResultBase<BuildPackageResult>.Callback c)
      : base(c)
    {
      this.asset = asset;
      this.packageID = -1;
    }

    protected override void Parse(Dictionary<string, JSONValue> dict)
    {
      dict = dict["download"].AsDict();
      this.packageID = int.Parse(dict["id"].AsString());
      if (this.packageID != this.asset.packageID)
      {
        Debug.LogError((object) "Got asset store server build result from mismatching package");
      }
      else
      {
        this.asset.previewInfo.packageUrl = !dict.ContainsKey("url") ? string.Empty : dict["url"].AsString(true);
        this.asset.previewInfo.encryptionKey = !dict.ContainsKey("key") ? string.Empty : dict["key"].AsString(true);
        this.asset.previewInfo.buildProgress = (float) ((!dict["progress"].IsFloat() ? (double) float.Parse(dict["progress"].AsString(true)) : (double) dict["progress"].AsFloat(true)) / 100.0);
      }
    }
  }
}
