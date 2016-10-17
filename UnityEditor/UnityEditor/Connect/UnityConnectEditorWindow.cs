// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityConnectEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Web;
using UnityEngine;

namespace UnityEditor.Connect
{
  internal class UnityConnectEditorWindow : WebViewEditorWindow
  {
    private List<string> m_ServiceUrls;
    private bool m_ClearInitialOpenURL;

    public string ErrorUrl { get; set; }

    public string currentUrl
    {
      get
      {
        return this.m_InitialOpenURL;
      }
      set
      {
        if (this.m_InitialOpenURL == value)
          return;
        this.m_InitialOpenURL = value;
        this.LoadPage();
      }
    }

    protected UnityConnectEditorWindow()
    {
      this.m_ServiceUrls = new List<string>();
      this.m_ClearInitialOpenURL = true;
    }

    public static UnityConnectEditorWindow Create(string title, List<string> serviceUrls)
    {
      UnityConnectEditorWindow[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (UnityConnectEditorWindow)) as UnityConnectEditorWindow[];
      if (objectsOfTypeAll != null)
      {
        using (IEnumerator<UnityConnectEditorWindow> enumerator = ((IEnumerable<UnityConnectEditorWindow>) objectsOfTypeAll).Where<UnityConnectEditorWindow>((Func<UnityConnectEditorWindow, bool>) (win => (UnityEngine.Object) win != (UnityEngine.Object) null)).GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            UnityConnectEditorWindow current = enumerator.Current;
            current.titleContent = new GUIContent(title);
            return current;
          }
        }
      }
      UnityConnectEditorWindow window = EditorWindow.GetWindow<UnityConnectEditorWindow>(title, new System.Type[1]{ typeof (InspectorWindow) });
      window.m_ClearInitialOpenURL = false;
      window.initialOpenUrl = serviceUrls[0];
      window.Init();
      return window;
    }

    public bool UrlsMatch(List<string> referenceUrls)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UnityConnectEditorWindow.\u003CUrlsMatch\u003Ec__AnonStoreyB6 matchCAnonStoreyB6 = new UnityConnectEditorWindow.\u003CUrlsMatch\u003Ec__AnonStoreyB6();
      // ISSUE: reference to a compiler-generated field
      matchCAnonStoreyB6.referenceUrls = referenceUrls;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ServiceUrls.Count != matchCAnonStoreyB6.referenceUrls.Count)
        return false;
      // ISSUE: reference to a compiler-generated method
      return !this.m_ServiceUrls.Where<string>(new Func<string, int, bool>(matchCAnonStoreyB6.\u003C\u003Em__221)).Any<string>();
    }

    public new void OnEnable()
    {
      this.m_ServiceUrls = UnityConnectServiceCollection.instance.GetAllServiceUrls();
      base.OnEnable();
    }

    public new void OnInitScripting()
    {
      base.OnInitScripting();
    }

    public new void ToggleMaximize()
    {
      base.ToggleMaximize();
    }

    public new void OnLoadError(string url)
    {
      if ((UnityEngine.Object) this.webView == (UnityEngine.Object) null)
        return;
      this.webView.LoadFile(EditorApplication.userJavascriptPackagesPath + "unityeditor-cloud-hub/dist/index.html?failure=load_error&reload_url=" + WWW.EscapeURL(url));
      if (!url.StartsWith("http://") && !url.StartsWith("https://"))
        return;
      this.UnregisterWebviewUrl(url);
    }

    public new void OnGUI()
    {
      if (this.m_ClearInitialOpenURL)
      {
        this.m_ClearInitialOpenURL = false;
        this.m_InitialOpenURL = this.m_ServiceUrls.Count <= 0 ? (string) null : UnityConnectServiceCollection.instance.GetUrlForService("Hub");
      }
      base.OnGUI();
    }
  }
}
