// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreSearchResults
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssetStoreSearchResults : AssetStoreResultBase<AssetStoreSearchResults>
  {
    internal List<AssetStoreSearchResults.Group> groups = new List<AssetStoreSearchResults.Group>();

    public AssetStoreSearchResults(AssetStoreResultBase<AssetStoreSearchResults>.Callback c)
      : base(c)
    {
    }

    protected override void Parse(Dictionary<string, JSONValue> dict)
    {
      using (List<JSONValue>.Enumerator enumerator = dict["groups"].AsList(true).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          JSONValue current = enumerator.Current;
          AssetStoreSearchResults.Group group = AssetStoreSearchResults.Group.Create();
          this.ParseList(current, ref group);
          this.groups.Add(group);
        }
      }
      JSONValue jsonValue = dict["query"]["offsets"];
      List<JSONValue> jsonValueList = dict["query"]["limits"].AsList(true);
      int index = 0;
      using (List<JSONValue>.Enumerator enumerator = jsonValue.AsList(true).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          JSONValue current = enumerator.Current;
          AssetStoreSearchResults.Group group = this.groups[index];
          group.offset = (int) current.AsFloat(true);
          group.limit = (int) jsonValueList[index].AsFloat(true);
          this.groups[index] = group;
          ++index;
        }
      }
    }

    private static string StripExtension(string path)
    {
      if (path == null)
        return (string) null;
      int length = path.LastIndexOf(".");
      if (length < 0)
        return path;
      return path.Substring(0, length);
    }

    private void ParseList(JSONValue matches, ref AssetStoreSearchResults.Group group)
    {
      List<AssetStoreAsset> assets = group.assets;
      if (matches.ContainsKey("error"))
        this.error = matches["error"].AsString(true);
      if (matches.ContainsKey("warnings"))
        this.warnings = matches["warnings"].AsString(true);
      if (matches.ContainsKey("name"))
        group.name = matches["name"].AsString(true);
      if (matches.ContainsKey("label"))
        group.label = matches["label"].AsString(true);
      group.label = group.label ?? group.name;
      if (matches.ContainsKey("total_found"))
        group.totalFound = (int) matches["total_found"].AsFloat(true);
      if (!matches.ContainsKey("matches"))
        return;
      using (List<JSONValue>.Enumerator enumerator = matches["matches"].AsList(true).GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          JSONValue current = enumerator.Current;
          AssetStoreAsset assetStoreAsset = new AssetStoreAsset();
          if (current.ContainsKey("id") && current.ContainsKey("name") && current.ContainsKey("package_id"))
          {
            assetStoreAsset.id = (int) current["id"].AsFloat();
            assetStoreAsset.name = current["name"].AsString();
            assetStoreAsset.displayName = AssetStoreSearchResults.StripExtension(assetStoreAsset.name);
            assetStoreAsset.packageID = (int) current["package_id"].AsFloat();
            if (current.ContainsKey("static_preview_url"))
              assetStoreAsset.staticPreviewURL = current["static_preview_url"].AsString();
            if (current.ContainsKey("dynamic_preview_url"))
              assetStoreAsset.dynamicPreviewURL = current["dynamic_preview_url"].AsString();
            assetStoreAsset.className = !current.ContainsKey("class_name") ? string.Empty : current["class_name"].AsString();
            if (current.ContainsKey("price"))
              assetStoreAsset.price = current["price"].AsString();
            assets.Add(assetStoreAsset);
          }
        }
      }
    }

    internal struct Group
    {
      public List<AssetStoreAsset> assets;
      public int totalFound;
      public string label;
      public string name;
      public int offset;
      public int limit;

      public static AssetStoreSearchResults.Group Create()
      {
        return new AssetStoreSearchResults.Group()
        {
          assets = new List<AssetStoreAsset>(),
          label = string.Empty,
          name = string.Empty,
          offset = 0,
          limit = -1
        };
      }
    }
  }
}
