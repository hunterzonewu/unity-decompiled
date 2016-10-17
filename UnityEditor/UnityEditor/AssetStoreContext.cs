// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreContext
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor.Web;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [InitializeOnLoad]
  internal sealed class AssetStoreContext
  {
    private static Regex s_StandardPackageRegExp = new Regex("/Standard Packages/(Character\\ Controller|Glass\\ Refraction\\ \\(Pro\\ Only\\)|Image\\ Effects\\ \\(Pro\\ Only\\)|Light\\ Cookies|Light\\ Flares|Particles|Physic\\ Materials|Projectors|Scripts|Standard\\ Assets\\ \\(Mobile\\)|Skyboxes|Terrain\\ Assets|Toon\\ Shading|Tree\\ Creator|Water\\ \\(Basic\\)|Water\\ \\(Pro\\ Only\\))\\.unitypackage$", RegexOptions.IgnoreCase);
    private static Regex s_GeneratedIDRegExp = new Regex("^\\{(.*)\\}$");
    private static Regex s_InvalidPathCharsRegExp = new Regex("[^a-zA-Z0-9() _-]");
    internal bool docked;
    internal string initialOpenURL;
    private static AssetStoreContext s_Instance;

    static AssetStoreContext()
    {
      AssetStoreContext.GetInstance();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SessionSetString(string key, string value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string SessionGetString(string key);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SessionRemoveString(string key);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SessionHasString(string key);

    public static AssetStoreContext GetInstance()
    {
      if (AssetStoreContext.s_Instance == null)
      {
        AssetStoreContext.s_Instance = new AssetStoreContext();
        JSProxyMgr.GetInstance().AddGlobalObject("AssetStoreContext", (object) AssetStoreContext.s_Instance);
      }
      return AssetStoreContext.s_Instance;
    }

    public string GetInitialOpenURL()
    {
      if (this.initialOpenURL == null)
        return string.Empty;
      string initialOpenUrl = this.initialOpenURL;
      this.initialOpenURL = (string) null;
      return initialOpenUrl;
    }

    public string GetAuthToken()
    {
      return InternalEditorUtility.GetAuthToken();
    }

    public int[] GetLicenseFlags()
    {
      return InternalEditorUtility.GetLicenseFlags();
    }

    public string GetString(string key)
    {
      return EditorPrefs.GetString(key);
    }

    public int GetInt(string key)
    {
      return EditorPrefs.GetInt(key);
    }

    public float GetFloat(string key)
    {
      return EditorPrefs.GetFloat(key);
    }

    public void SetString(string key, string value)
    {
      EditorPrefs.SetString(key, value);
    }

    public void SetInt(string key, int value)
    {
      EditorPrefs.SetInt(key, value);
    }

    public void SetFloat(string key, float value)
    {
      EditorPrefs.SetFloat(key, value);
    }

    public bool HasKey(string key)
    {
      return EditorPrefs.HasKey(key);
    }

    public void DeleteKey(string key)
    {
      EditorPrefs.DeleteKey(key);
    }

    public int GetSkinIndex()
    {
      return EditorGUIUtility.skinIndex;
    }

    public bool GetDockedStatus()
    {
      return this.docked;
    }

    public bool OpenPackage(string id)
    {
      return this.OpenPackage(id, "default");
    }

    public bool OpenPackage(string id, string action)
    {
      return AssetStoreContext.OpenPackageInternal(id);
    }

    public static bool OpenPackageInternal(string id)
    {
      Match match = AssetStoreContext.s_GeneratedIDRegExp.Match(id);
      if (match.Success && File.Exists(match.Groups[1].Value))
      {
        AssetDatabase.ImportPackage(match.Groups[1].Value, true);
        return true;
      }
      foreach (PackageInfo package in PackageInfo.GetPackageList())
      {
        if (package.jsonInfo != string.Empty)
        {
          JSONValue jsonValue = JSONParser.SimpleParse(package.jsonInfo);
          string str = !jsonValue.Get("id").IsNull() ? jsonValue["id"].AsString(true) : (string) null;
          if (str != null && str == id && File.Exists(package.packagePath))
          {
            AssetDatabase.ImportPackage(package.packagePath, true);
            return true;
          }
        }
      }
      Debug.LogError((object) ("Unknown package ID " + id));
      return false;
    }

    public void OpenBrowser(string url)
    {
      Application.OpenURL(url);
    }

    public void Download(AssetStoreContext.Package package, AssetStoreContext.DownloadInfo downloadInfo)
    {
      AssetStoreContext.Download(downloadInfo.id, downloadInfo.url, downloadInfo.key, package.title, package.publisher.label, package.category.label, (AssetStoreUtils.DownloadDoneCallback) null);
    }

    public static void Download(string package_id, string url, string key, string package_name, string publisher_name, string category_name, AssetStoreUtils.DownloadDoneCallback doneCallback)
    {
      string[] destination = AssetStoreContext.PackageStorePath(publisher_name, category_name, package_name, package_id, url);
      JSONValue jsonValue1 = JSONParser.SimpleParse(AssetStoreUtils.CheckDownload(package_id, url, destination, key));
      if (jsonValue1.Get("in_progress").AsBool(true))
      {
        Debug.Log((object) ("Will not download " + package_name + ". Download is already in progress."));
      }
      else
      {
        string str1 = jsonValue1.Get("download.url").AsString(true);
        string str2 = jsonValue1.Get("download.key").AsString(true);
        bool resumeOK = str1 == url && str2 == key;
        JSONValue jsonValue2 = new JSONValue();
        jsonValue2["url"] = (JSONValue) url;
        jsonValue2["key"] = (JSONValue) key;
        JSONValue jsonValue3 = new JSONValue();
        jsonValue3["download"] = jsonValue2;
        AssetStoreUtils.Download(package_id, url, destination, key, jsonValue3.ToString(), resumeOK, doneCallback);
      }
    }

    public static string[] PackageStorePath(string publisher_name, string category_name, string package_name, string package_id, string url)
    {
      string[] strArray = new string[3]
      {
        publisher_name,
        category_name,
        package_name
      };
      for (int index = 0; index < 3; ++index)
        strArray[index] = AssetStoreContext.s_InvalidPathCharsRegExp.Replace(strArray[index], string.Empty);
      if (strArray[2] == string.Empty)
        strArray[2] = AssetStoreContext.s_InvalidPathCharsRegExp.Replace(package_id, string.Empty);
      if (strArray[2] == string.Empty)
        strArray[2] = AssetStoreContext.s_InvalidPathCharsRegExp.Replace(url, string.Empty);
      return strArray;
    }

    public AssetStoreContext.PackageList GetPackageList()
    {
      Dictionary<string, AssetStoreContext.Package> dictionary = new Dictionary<string, AssetStoreContext.Package>();
      foreach (PackageInfo package1 in PackageInfo.GetPackageList())
      {
        AssetStoreContext.Package package2 = new AssetStoreContext.Package();
        if (package1.jsonInfo == string.Empty)
        {
          package2.title = Path.GetFileNameWithoutExtension(package1.packagePath);
          package2.id = package1.packagePath;
          if (this.IsBuiltinStandardAsset(package1.packagePath))
          {
            package2.publisher = new AssetStoreContext.LabelAndId()
            {
              label = "Unity Technologies",
              id = "1"
            };
            package2.category = new AssetStoreContext.LabelAndId()
            {
              label = "Prefab Packages",
              id = "4"
            };
            package2.version = "3.5.0.0";
          }
        }
        else
        {
          JSONValue json = JSONParser.SimpleParse(package1.jsonInfo);
          if (!json.IsNull())
          {
            package2.Initialize(json);
            if (package2.id == null)
            {
              JSONValue jsonValue = json.Get("link.id");
              package2.id = jsonValue.IsNull() ? package1.packagePath : jsonValue.AsString();
            }
          }
          else
            continue;
        }
        package2.local_icon = package1.iconURL;
        package2.local_path = package1.packagePath;
        if (!dictionary.ContainsKey(package2.id) || dictionary[package2.id].version_id == null || dictionary[package2.id].version_id == "-1" || package2.version_id != null && package2.version_id != "-1" && int.Parse(dictionary[package2.id].version_id) <= int.Parse(package2.version_id))
          dictionary[package2.id] = package2;
      }
      AssetStoreContext.Package[] array = dictionary.Values.ToArray<AssetStoreContext.Package>();
      return new AssetStoreContext.PackageList()
      {
        results = array
      };
    }

    private bool IsBuiltinStandardAsset(string path)
    {
      return AssetStoreContext.s_StandardPackageRegExp.IsMatch(path);
    }

    public class DownloadInfo
    {
      public string url;
      public string key;
      public string id;
    }

    public class LabelAndId
    {
      public string label;
      public string id;

      public void Initialize(JSONValue json)
      {
        if (json.ContainsKey("label"))
          this.label = json["label"].AsString();
        if (!json.ContainsKey("id"))
          return;
        this.id = json["id"].AsString();
      }

      public override string ToString()
      {
        return string.Format("{{label={0}, id={1}}}", (object) this.label, (object) this.id);
      }
    }

    public class Link
    {
      public string type;
      public string id;

      public void Initialize(JSONValue json)
      {
        if (json.ContainsKey("type"))
          this.type = json["type"].AsString();
        if (!json.ContainsKey("id"))
          return;
        this.id = json["id"].AsString();
      }

      public override string ToString()
      {
        return string.Format("{{type={0}, id={1}}}", (object) this.type, (object) this.id);
      }
    }

    public class Package
    {
      public string title;
      public string id;
      public string version;
      public string version_id;
      public string local_icon;
      public string local_path;
      public string pubdate;
      public string description;
      public AssetStoreContext.LabelAndId publisher;
      public AssetStoreContext.LabelAndId category;
      public AssetStoreContext.Link link;

      public void Initialize(JSONValue json)
      {
        if (json.ContainsKey("title"))
          this.title = json["title"].AsString();
        if (json.ContainsKey("id"))
          this.id = json["id"].AsString();
        if (json.ContainsKey("version"))
          this.version = json["version"].AsString();
        if (json.ContainsKey("version_id"))
          this.version_id = json["version_id"].AsString();
        if (json.ContainsKey("local_icon"))
          this.local_icon = json["local_icon"].AsString();
        if (json.ContainsKey("local_path"))
          this.local_path = json["local_path"].AsString();
        if (json.ContainsKey("pubdate"))
          this.pubdate = json["pubdate"].AsString();
        if (json.ContainsKey("description"))
          this.description = json["description"].AsString();
        if (json.ContainsKey("publisher"))
        {
          this.publisher = new AssetStoreContext.LabelAndId();
          this.publisher.Initialize(json["publisher"]);
        }
        if (json.ContainsKey("category"))
        {
          this.category = new AssetStoreContext.LabelAndId();
          this.category.Initialize(json["category"]);
        }
        if (!json.ContainsKey("link"))
          return;
        this.link = new AssetStoreContext.Link();
        this.link.Initialize(json["link"]);
      }

      public override string ToString()
      {
        return string.Format("{{title={0}, id={1}, publisher={2}, category={3}, pubdate={8}, version={4}, version_id={5}, description={9}, link={10}, local_icon={6}, local_path={7}}}", (object) this.title, (object) this.id, (object) this.publisher, (object) this.category, (object) this.version, (object) this.version_id, (object) this.local_icon, (object) this.local_path, (object) this.pubdate, (object) this.description, (object) this.link);
      }
    }

    public class PackageList
    {
      public AssetStoreContext.Package[] results;
    }
  }
}
