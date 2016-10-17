// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreClient
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetStoreClient
  {
    private const string kUnauthSessionID = "26c4202eb475d02864b40827dfff11a14657aa41";
    private static string s_AssetStoreUrl;
    private static string s_AssetStoreSearchUrl;
    private static AssetStoreClient.LoginState sLoginState;
    private static string sLoginErrorMessage;

    public static string LoginErrorMessage
    {
      get
      {
        return AssetStoreClient.sLoginErrorMessage;
      }
    }

    private static string VersionParams
    {
      get
      {
        return "unityversion=" + Uri.EscapeDataString(Application.unityVersion) + "&skip_terms=1";
      }
    }

    private static string AssetStoreUrl
    {
      get
      {
        if (AssetStoreClient.s_AssetStoreUrl == null)
          AssetStoreClient.s_AssetStoreUrl = AssetStoreUtils.GetAssetStoreUrl();
        return AssetStoreClient.s_AssetStoreUrl;
      }
    }

    private static string AssetStoreSearchUrl
    {
      get
      {
        if (AssetStoreClient.s_AssetStoreSearchUrl == null)
          AssetStoreClient.s_AssetStoreSearchUrl = AssetStoreUtils.GetAssetStoreSearchUrl();
        return AssetStoreClient.s_AssetStoreSearchUrl;
      }
    }

    private static string SavedSessionID
    {
      get
      {
        if (AssetStoreClient.RememberSession)
          return EditorPrefs.GetString("kharma.sessionid", string.Empty);
        return string.Empty;
      }
      set
      {
        EditorPrefs.SetString("kharma.sessionid", value);
      }
    }

    public static bool HasSavedSessionID
    {
      get
      {
        return !string.IsNullOrEmpty(AssetStoreClient.SavedSessionID);
      }
    }

    internal static string ActiveSessionID
    {
      get
      {
        if (AssetStoreContext.SessionHasString("kharma.active_sessionid"))
          return AssetStoreContext.SessionGetString("kharma.active_sessionid");
        return string.Empty;
      }
      set
      {
        AssetStoreContext.SessionSetString("kharma.active_sessionid", value);
      }
    }

    public static bool HasActiveSessionID
    {
      get
      {
        return !string.IsNullOrEmpty(AssetStoreClient.ActiveSessionID);
      }
    }

    private static string ActiveOrUnauthSessionID
    {
      get
      {
        string activeSessionId = AssetStoreClient.ActiveSessionID;
        if (activeSessionId == string.Empty)
          return "26c4202eb475d02864b40827dfff11a14657aa41";
        return activeSessionId;
      }
    }

    public static bool RememberSession
    {
      get
      {
        return EditorPrefs.GetString("kharma.remember_session") == "1";
      }
      set
      {
        EditorPrefs.SetString("kharma.remember_session", !value ? "0" : "1");
      }
    }

    static AssetStoreClient()
    {
      ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) ((param0, param1, param2, param3) => true);
    }

    private static string APIUrl(string path)
    {
      return string.Format("{0}/api{2}.json?{1}", (object) AssetStoreClient.AssetStoreUrl, (object) AssetStoreClient.VersionParams, (object) path);
    }

    private static string APISearchUrl(string path)
    {
      return string.Format("{0}/public-api{2}.json?{1}", (object) AssetStoreClient.AssetStoreSearchUrl, (object) AssetStoreClient.VersionParams, (object) path);
    }

    private static string GetToken()
    {
      return InternalEditorUtility.GetAuthToken();
    }

    public static bool LoggedIn()
    {
      return !string.IsNullOrEmpty(AssetStoreClient.ActiveSessionID);
    }

    public static bool LoggedOut()
    {
      return string.IsNullOrEmpty(AssetStoreClient.ActiveSessionID);
    }

    public static bool LoginError()
    {
      return AssetStoreClient.sLoginState == AssetStoreClient.LoginState.LOGIN_ERROR;
    }

    public static bool LoginInProgress()
    {
      return AssetStoreClient.sLoginState == AssetStoreClient.LoginState.IN_PROGRESS;
    }

    internal static void LoginWithCredentials(string username, string password, bool rememberMe, AssetStoreClient.DoneLoginCallback callback)
    {
      if (AssetStoreClient.sLoginState == AssetStoreClient.LoginState.IN_PROGRESS)
      {
        Debug.LogError((object) "Tried to login with credentials while already in progress of logging in");
      }
      else
      {
        AssetStoreClient.sLoginState = AssetStoreClient.LoginState.IN_PROGRESS;
        AssetStoreClient.RememberSession = rememberMe;
        string str = AssetStoreClient.AssetStoreUrl + "/login?skip_terms=1";
        AssetStoreClient.sLoginErrorMessage = (string) null;
        AsyncHTTPClient asyncHttpClient = new AsyncHTTPClient(str.Replace("http://", "https://"));
        asyncHttpClient.postData = "user=" + username + "&pass=" + password;
        asyncHttpClient.header["X-Unity-Session"] = "26c4202eb475d02864b40827dfff11a14657aa41" + AssetStoreClient.GetToken();
        asyncHttpClient.doneCallback = AssetStoreClient.WrapLoginCallback(callback);
        asyncHttpClient.Begin();
      }
    }

    internal static void LoginWithRememberedSession(AssetStoreClient.DoneLoginCallback callback)
    {
      if (AssetStoreClient.sLoginState == AssetStoreClient.LoginState.IN_PROGRESS)
      {
        Debug.LogError((object) "Tried to login with remembered session while already in progress of logging in");
      }
      else
      {
        AssetStoreClient.sLoginState = AssetStoreClient.LoginState.IN_PROGRESS;
        if (!AssetStoreClient.RememberSession)
          AssetStoreClient.SavedSessionID = string.Empty;
        string _toUrl = AssetStoreClient.AssetStoreUrl + "/login?skip_terms=1&reuse_session=" + AssetStoreClient.SavedSessionID;
        AssetStoreClient.sLoginErrorMessage = (string) null;
        AsyncHTTPClient asyncHttpClient = new AsyncHTTPClient(_toUrl);
        asyncHttpClient.header["X-Unity-Session"] = "26c4202eb475d02864b40827dfff11a14657aa41" + AssetStoreClient.GetToken();
        asyncHttpClient.doneCallback = AssetStoreClient.WrapLoginCallback(callback);
        asyncHttpClient.Begin();
      }
    }

    private static AsyncHTTPClient.DoneCallback WrapLoginCallback(AssetStoreClient.DoneLoginCallback callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new AsyncHTTPClient.DoneCallback(new AssetStoreClient.\u003CWrapLoginCallback\u003Ec__AnonStorey52()
      {
        callback = callback
      }.\u003C\u003Em__8F);
    }

    public static void Logout()
    {
      AssetStoreClient.ActiveSessionID = string.Empty;
      AssetStoreClient.SavedSessionID = string.Empty;
      AssetStoreClient.sLoginState = AssetStoreClient.LoginState.LOGGED_OUT;
    }

    private static AsyncHTTPClient CreateJSONRequest(string url, AssetStoreClient.DoneCallback callback)
    {
      AsyncHTTPClient asyncHttpClient = new AsyncHTTPClient(url);
      asyncHttpClient.header["X-Unity-Session"] = AssetStoreClient.ActiveOrUnauthSessionID + AssetStoreClient.GetToken();
      asyncHttpClient.doneCallback = AssetStoreClient.WrapJsonCallback(callback);
      asyncHttpClient.Begin();
      return asyncHttpClient;
    }

    private static AsyncHTTPClient CreateJSONRequestPost(string url, Dictionary<string, string> param, AssetStoreClient.DoneCallback callback)
    {
      AsyncHTTPClient asyncHttpClient = new AsyncHTTPClient(url);
      asyncHttpClient.header["X-Unity-Session"] = AssetStoreClient.ActiveOrUnauthSessionID + AssetStoreClient.GetToken();
      asyncHttpClient.postDictionary = param;
      asyncHttpClient.doneCallback = AssetStoreClient.WrapJsonCallback(callback);
      asyncHttpClient.Begin();
      return asyncHttpClient;
    }

    private static AsyncHTTPClient CreateJSONRequestPost(string url, string postData, AssetStoreClient.DoneCallback callback)
    {
      AsyncHTTPClient asyncHttpClient = new AsyncHTTPClient(url);
      asyncHttpClient.header["X-Unity-Session"] = AssetStoreClient.ActiveOrUnauthSessionID + AssetStoreClient.GetToken();
      asyncHttpClient.postData = postData;
      asyncHttpClient.doneCallback = AssetStoreClient.WrapJsonCallback(callback);
      asyncHttpClient.Begin();
      return asyncHttpClient;
    }

    private static AsyncHTTPClient CreateJSONRequestDelete(string url, AssetStoreClient.DoneCallback callback)
    {
      AsyncHTTPClient asyncHttpClient = new AsyncHTTPClient(url, "DELETE");
      asyncHttpClient.header["X-Unity-Session"] = AssetStoreClient.ActiveOrUnauthSessionID + AssetStoreClient.GetToken();
      asyncHttpClient.doneCallback = AssetStoreClient.WrapJsonCallback(callback);
      asyncHttpClient.Begin();
      return asyncHttpClient;
    }

    private static AsyncHTTPClient.DoneCallback WrapJsonCallback(AssetStoreClient.DoneCallback callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return new AsyncHTTPClient.DoneCallback(new AssetStoreClient.\u003CWrapJsonCallback\u003Ec__AnonStorey53()
      {
        callback = callback
      }.\u003C\u003Em__90);
    }

    private static AssetStoreResponse ParseContent(AsyncHTTPClient job)
    {
      AssetStoreResponse assetStoreResponse = new AssetStoreResponse();
      assetStoreResponse.job = job;
      assetStoreResponse.dict = (Dictionary<string, JSONValue>) null;
      assetStoreResponse.ok = false;
      AsyncHTTPClient.State state = job.state;
      string text = job.text;
      if (!AsyncHTTPClient.IsSuccess(state))
      {
        Console.WriteLine(text);
        return assetStoreResponse;
      }
      string status;
      string message;
      assetStoreResponse.dict = AssetStoreClient.ParseJSON(text, out status, out message);
      if (status == "error")
      {
        Debug.LogError((object) ("Request error (" + status + "): " + message));
        return assetStoreResponse;
      }
      assetStoreResponse.ok = true;
      return assetStoreResponse;
    }

    private static Dictionary<string, JSONValue> ParseJSON(string content, out string status, out string message)
    {
      message = (string) null;
      status = (string) null;
      JSONValue jsonValue;
      try
      {
        jsonValue = new JSONParser(content).Parse();
      }
      catch (JSONParseException ex)
      {
        Debug.Log((object) ("Error parsing server reply: " + content));
        Debug.Log((object) ex.Message);
        return (Dictionary<string, JSONValue>) null;
      }
      Dictionary<string, JSONValue> dictionary;
      try
      {
        dictionary = jsonValue.AsDict(true);
        if (dictionary == null)
        {
          Debug.Log((object) ("Error parsing server message: " + content));
          return (Dictionary<string, JSONValue>) null;
        }
        if (dictionary.ContainsKey("result") && dictionary["result"].IsDict())
          dictionary = dictionary["result"].AsDict(true);
        if (dictionary.ContainsKey("message"))
          message = dictionary["message"].AsString(true);
        if (dictionary.ContainsKey("status"))
          status = dictionary["status"].AsString(true);
        else if (dictionary.ContainsKey("error"))
        {
          status = dictionary["error"].AsString(true);
          if (status == string.Empty)
            status = "ok";
        }
        else
          status = "ok";
      }
      catch (JSONTypeException ex)
      {
        Debug.Log((object) ("Error parsing server reply. " + content));
        Debug.Log((object) ex.Message);
        return (Dictionary<string, JSONValue>) null;
      }
      return dictionary;
    }

    internal static AsyncHTTPClient SearchAssets(string searchString, string[] requiredClassNames, string[] assetLabels, List<AssetStoreClient.SearchCount> counts, AssetStoreResultBase<AssetStoreSearchResults>.Callback callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreClient.\u003CSearchAssets\u003Ec__AnonStorey54 assetsCAnonStorey54 = new AssetStoreClient.\u003CSearchAssets\u003Ec__AnonStorey54();
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = string.Empty;
      string str4 = string.Empty;
      using (List<AssetStoreClient.SearchCount>.Enumerator enumerator = counts.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AssetStoreClient.SearchCount current = enumerator.Current;
          str1 = str1 + str4 + (object) current.offset;
          str2 = str2 + str4 + (object) current.limit;
          str3 = str3 + str4 + current.name;
          str4 = ",";
        }
      }
      if (Array.Exists<string>(requiredClassNames, (Predicate<string>) (a => a.Equals("MonoScript", StringComparison.OrdinalIgnoreCase))))
      {
        Array.Resize<string>(ref requiredClassNames, requiredClassNames.Length + 1);
        requiredClassNames[requiredClassNames.Length - 1] = "Script";
      }
      string url = string.Format("{0}&q={1}&c={2}&l={3}&O={4}&N={5}&G={6}", (object) AssetStoreClient.APISearchUrl("/search/assets"), (object) Uri.EscapeDataString(searchString), (object) Uri.EscapeDataString(string.Join(",", requiredClassNames)), (object) Uri.EscapeDataString(string.Join(",", assetLabels)), (object) str1, (object) str2, (object) str3);
      // ISSUE: reference to a compiler-generated field
      assetsCAnonStorey54.r = new AssetStoreSearchResults(callback);
      // ISSUE: reference to a compiler-generated method
      return AssetStoreClient.CreateJSONRequest(url, new AssetStoreClient.DoneCallback(assetsCAnonStorey54.\u003C\u003Em__92));
    }

    internal static AsyncHTTPClient AssetsInfo(List<AssetStoreAsset> assets, AssetStoreResultBase<AssetStoreAssetsInfo>.Callback callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreClient.\u003CAssetsInfo\u003Ec__AnonStorey55 infoCAnonStorey55 = new AssetStoreClient.\u003CAssetsInfo\u003Ec__AnonStorey55();
      string url = AssetStoreClient.APIUrl("/assets/list");
      using (List<AssetStoreAsset>.Enumerator enumerator = assets.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AssetStoreAsset current = enumerator.Current;
          url = url + "&id=" + current.id.ToString();
        }
      }
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey55.r = new AssetStoreAssetsInfo(callback, assets);
      // ISSUE: reference to a compiler-generated method
      return AssetStoreClient.CreateJSONRequest(url, new AssetStoreClient.DoneCallback(infoCAnonStorey55.\u003C\u003Em__93));
    }

    internal static AsyncHTTPClient DirectPurchase(int packageID, string password, AssetStoreResultBase<PurchaseResult>.Callback callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreClient.\u003CDirectPurchase\u003Ec__AnonStorey56 purchaseCAnonStorey56 = new AssetStoreClient.\u003CDirectPurchase\u003Ec__AnonStorey56();
      string url = AssetStoreClient.APIUrl(string.Format("/purchase/direct/{0}", (object) packageID.ToString()));
      // ISSUE: reference to a compiler-generated field
      purchaseCAnonStorey56.r = new PurchaseResult(callback);
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary["password"] = password;
      // ISSUE: reference to a compiler-generated method
      return AssetStoreClient.CreateJSONRequestPost(url, dictionary, new AssetStoreClient.DoneCallback(purchaseCAnonStorey56.\u003C\u003Em__94));
    }

    internal static AsyncHTTPClient BuildPackage(AssetStoreAsset asset, AssetStoreResultBase<BuildPackageResult>.Callback callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetStoreClient.\u003CBuildPackage\u003Ec__AnonStorey57 packageCAnonStorey57 = new AssetStoreClient.\u003CBuildPackage\u003Ec__AnonStorey57();
      string url = AssetStoreClient.APIUrl("/content/download/" + asset.packageID.ToString());
      // ISSUE: reference to a compiler-generated field
      packageCAnonStorey57.r = new BuildPackageResult(asset, callback);
      // ISSUE: reference to a compiler-generated method
      return AssetStoreClient.CreateJSONRequest(url, new AssetStoreClient.DoneCallback(packageCAnonStorey57.\u003C\u003Em__95));
    }

    internal enum LoginState
    {
      LOGGED_OUT,
      IN_PROGRESS,
      LOGGED_IN,
      LOGIN_ERROR,
    }

    internal struct SearchCount
    {
      public string name;
      public int offset;
      public int limit;
    }

    public delegate void DoneCallback(AssetStoreResponse response);

    public delegate void DoneLoginCallback(string errorMessage);
  }
}
