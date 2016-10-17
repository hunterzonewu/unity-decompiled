// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityConnectServiceCollection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Web;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Connect
{
  internal class UnityConnectServiceCollection
  {
    private string m_CurrentServiceName = string.Empty;
    private string m_CurrentPageName = string.Empty;
    private const string kDrawerContainerTitle = "Services";
    private static UnityConnectServiceCollection s_UnityConnectEditor;
    private static UnityConnectEditorWindow s_UnityConnectEditorWindow;
    private readonly Dictionary<string, UnityConnectServiceData> m_Services;

    private bool isDrawerOpen
    {
      get
      {
        UnityConnectEditorWindow[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (UnityConnectEditorWindow)) as UnityConnectEditorWindow[];
        if (objectsOfTypeAll != null)
          return ((IEnumerable<UnityConnectEditorWindow>) objectsOfTypeAll).Any<UnityConnectEditorWindow>((Func<UnityConnectEditorWindow, bool>) (win => (UnityEngine.Object) win != (UnityEngine.Object) null));
        return false;
      }
    }

    public static UnityConnectServiceCollection instance
    {
      get
      {
        if (UnityConnectServiceCollection.s_UnityConnectEditor == null)
        {
          UnityConnectServiceCollection.s_UnityConnectEditor = new UnityConnectServiceCollection();
          UnityConnectServiceCollection.s_UnityConnectEditor.Init();
        }
        return UnityConnectServiceCollection.s_UnityConnectEditor;
      }
    }

    private UnityConnectServiceCollection()
    {
      this.m_Services = new Dictionary<string, UnityConnectServiceData>();
      UnityConnect.instance.StateChanged += new StateChangedDelegate(this.InstanceStateChanged);
    }

    protected void InstanceStateChanged(ConnectInfo state)
    {
      if (!this.isDrawerOpen || !state.ready)
        return;
      string actualServiceName = this.GetActualServiceName(this.m_CurrentServiceName, state);
      if (!(actualServiceName != this.m_CurrentServiceName) && (!((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow != (UnityEngine.Object) null) || !(this.m_Services[actualServiceName].serviceUrl != UnityConnectServiceCollection.s_UnityConnectEditorWindow.currentUrl)))
        return;
      bool forceFocus = (bool) ((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow) && (bool) UnityConnectServiceCollection.s_UnityConnectEditorWindow.webView && UnityConnectServiceCollection.s_UnityConnectEditorWindow.webView.HasApplicationFocus();
      this.ShowService(actualServiceName, forceFocus);
    }

    private void Init()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("UnityConnectEditor", (object) this);
    }

    private void EnsureDrawerIsVisible(bool forceFocus)
    {
      if ((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow == (UnityEngine.Object) null || !UnityConnectServiceCollection.s_UnityConnectEditorWindow.UrlsMatch(this.GetAllServiceUrls()))
      {
        string title = "Services";
        int serviceEnv = UnityConnectPrefs.GetServiceEnv(this.m_CurrentServiceName);
        if (serviceEnv != 0)
          title = title + " [" + UnityConnectPrefs.kEnvironmentFamilies[serviceEnv] + "]";
        UnityConnectServiceCollection.s_UnityConnectEditorWindow = UnityConnectEditorWindow.Create(title, this.GetAllServiceUrls());
        UnityConnectServiceCollection.s_UnityConnectEditorWindow.ErrorUrl = this.m_Services["ErrorHub"].serviceUrl;
        UnityConnectServiceCollection.s_UnityConnectEditorWindow.minSize = new Vector2(275f, 50f);
      }
      string str = this.m_Services[this.m_CurrentServiceName].serviceUrl;
      if (this.m_CurrentPageName.Length > 0)
        str = str + "/#/" + this.m_CurrentPageName;
      UnityConnectServiceCollection.s_UnityConnectEditorWindow.currentUrl = str;
      UnityConnectServiceCollection.s_UnityConnectEditorWindow.Show();
      if (!InternalEditorUtility.isApplicationActive || !forceFocus)
        return;
      UnityConnectServiceCollection.s_UnityConnectEditorWindow.Focus();
    }

    public void ReloadServices()
    {
      if ((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow != (UnityEngine.Object) null)
      {
        UnityConnectServiceCollection.s_UnityConnectEditorWindow.Close();
        UnityConnectServiceCollection.s_UnityConnectEditorWindow = (UnityConnectEditorWindow) null;
      }
      UnityConnect.instance.ClearCache();
    }

    public bool AddService(UnityConnectServiceData cloudService)
    {
      if (this.m_Services.ContainsKey(cloudService.serviceName))
        return false;
      this.m_Services[cloudService.serviceName] = cloudService;
      return true;
    }

    public bool ServiceExist(string serviceName)
    {
      return this.m_Services.ContainsKey(serviceName);
    }

    public bool ShowService(string serviceName, bool forceFocus)
    {
      return this.ShowService(serviceName, string.Empty, forceFocus);
    }

    public bool ShowService(string serviceName, string atPage, bool forceFocus)
    {
      if (!this.m_Services.ContainsKey(serviceName))
        return false;
      ConnectInfo connectInfo = UnityConnect.instance.connectInfo;
      this.m_CurrentServiceName = this.GetActualServiceName(serviceName, connectInfo);
      this.m_CurrentPageName = atPage;
      this.EnsureDrawerIsVisible(forceFocus);
      return true;
    }

    private string GetActualServiceName(string desiredServiceName, ConnectInfo state)
    {
      if (!state.online)
        return "ErrorHub";
      if (!state.ready)
        return "Hub";
      if (state.maintenance)
        return "ErrorHub";
      if (desiredServiceName != "Hub" && state.online && !state.loggedIn || (desiredServiceName == "ErrorHub" && state.online || string.IsNullOrEmpty(desiredServiceName)))
        return "Hub";
      return desiredServiceName;
    }

    public void EnableService(string name, bool enabled)
    {
      if (!this.m_Services.ContainsKey(name))
        return;
      this.m_Services[name].EnableService(enabled);
    }

    public string GetUrlForService(string serviceName)
    {
      if (this.m_Services.ContainsKey(serviceName))
        return this.m_Services[serviceName].serviceUrl;
      return string.Empty;
    }

    public UnityConnectServiceData GetServiceFromUrl(string searchUrl)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return this.m_Services.FirstOrDefault<KeyValuePair<string, UnityConnectServiceData>>(new Func<KeyValuePair<string, UnityConnectServiceData>, bool>(new UnityConnectServiceCollection.\u003CGetServiceFromUrl\u003Ec__AnonStoreyB7() { searchUrl = searchUrl }.\u003C\u003Em__223)).Value;
    }

    public List<string> GetAllServiceNames()
    {
      return this.m_Services.Keys.ToList<string>();
    }

    public List<string> GetAllServiceUrls()
    {
      return this.m_Services.Values.Select<UnityConnectServiceData, string>((Func<UnityConnectServiceData, string>) (unityConnectData => unityConnectData.serviceUrl)).ToList<string>();
    }

    public UnityConnectServiceCollection.ServiceInfo[] GetAllServiceInfos()
    {
      return this.m_Services.Select<KeyValuePair<string, UnityConnectServiceData>, UnityConnectServiceCollection.ServiceInfo>((Func<KeyValuePair<string, UnityConnectServiceData>, UnityConnectServiceCollection.ServiceInfo>) (item => new UnityConnectServiceCollection.ServiceInfo(item.Value.serviceName, item.Value.serviceUrl, item.Value.serviceJsGlobalObjectName, item.Value.serviceJsGlobalObject.IsServiceEnabled()))).ToArray<UnityConnectServiceCollection.ServiceInfo>();
    }

    public WebView GetWebViewFromServiceName(string serviceName)
    {
      if ((UnityEngine.Object) UnityConnectServiceCollection.s_UnityConnectEditorWindow == (UnityEngine.Object) null || !UnityConnectServiceCollection.s_UnityConnectEditorWindow.UrlsMatch(this.GetAllServiceUrls()))
        return (WebView) null;
      if (!this.m_Services.ContainsKey(serviceName))
        return (WebView) null;
      ConnectInfo connectInfo = UnityConnect.instance.connectInfo;
      string serviceUrl = this.m_Services[this.GetActualServiceName(serviceName, connectInfo)].serviceUrl;
      return UnityConnectServiceCollection.s_UnityConnectEditorWindow.GetWebViewFromURL(serviceUrl);
    }

    public class ServiceInfo
    {
      public string name;
      public string url;
      public string unityPath;
      public bool enabled;

      public ServiceInfo(string name, string url, string unityPath, bool enabled)
      {
        this.name = name;
        this.url = url;
        this.unityPath = unityPath;
        this.enabled = enabled;
      }
    }
  }
}
