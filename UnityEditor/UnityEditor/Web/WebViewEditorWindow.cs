// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.WebViewEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using UnityEngine;

namespace UnityEditor.Web
{
  internal class WebViewEditorWindow : EditorWindow, ISerializationCallbackReceiver, IHasCustomMenu
  {
    private const int k_RepaintTimerDelay = 30;
    [SerializeField]
    protected string m_InitialOpenURL;
    [SerializeField]
    protected string m_GlobalObjectTypeName;
    protected object m_GlobalObject;
    internal WebView webView;
    internal WebScriptObject scriptObject;
    [SerializeField]
    private List<string> m_RegisteredViewURLs;
    [SerializeField]
    private List<WebView> m_RegisteredViewInstances;
    private Dictionary<string, WebView> m_RegisteredViews;
    private bool m_SyncingFocus;
    private int m_RepeatedShow;
    private Timer m_PostLoadTimer;

    public string initialOpenUrl
    {
      get
      {
        return this.m_InitialOpenURL;
      }
      set
      {
        this.m_InitialOpenURL = value;
      }
    }

    protected WebViewEditorWindow()
    {
      this.m_RegisteredViewURLs = new List<string>();
      this.m_RegisteredViewInstances = new List<WebView>();
      this.m_RegisteredViews = new Dictionary<string, WebView>();
      this.m_GlobalObject = (object) null;
      Resolution currentResolution = Screen.currentResolution;
      int num1 = currentResolution.width < 1024 ? currentResolution.width : 1024;
      int num2 = currentResolution.height < 896 ? currentResolution.height - 96 : 800;
      this.position = new Rect((float) ((currentResolution.width - num1) / 2), (float) ((currentResolution.height - num2) / 2), (float) num1, (float) num2);
      this.m_RepeatedShow = 0;
    }

    public static WebViewEditorWindow Create<T>(string title, string sourcesPath, int minWidth, int minHeight, int maxWidth, int maxHeight) where T : new()
    {
      WebViewEditorWindow instance = ScriptableObject.CreateInstance<WebViewEditorWindow>();
      instance.titleContent = new GUIContent(title);
      instance.minSize = new Vector2((float) minWidth, (float) minHeight);
      instance.maxSize = new Vector2((float) maxWidth, (float) maxHeight);
      instance.m_InitialOpenURL = sourcesPath;
      instance.m_GlobalObjectTypeName = typeof (T).FullName;
      instance.Init();
      instance.Show();
      return instance;
    }

    public static WebViewEditorWindow CreateBase(string title, string sourcesPath, int minWidth, int minHeight, int maxWidth, int maxHeight)
    {
      WebViewEditorWindow window = EditorWindow.GetWindow<WebViewEditorWindow>(title);
      window.minSize = new Vector2((float) minWidth, (float) minHeight);
      window.maxSize = new Vector2((float) maxWidth, (float) maxHeight);
      window.m_InitialOpenURL = sourcesPath;
      window.m_GlobalObjectTypeName = (string) null;
      window.Init();
      window.Show();
      return window;
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Reload"), false, new GenericMenu.MenuFunction(this.Reload));
      if (!Unsupported.IsDeveloperBuild())
        return;
      menu.AddItem(new GUIContent("About"), false, new GenericMenu.MenuFunction(this.About));
    }

    public void Logout()
    {
    }

    public void Reload()
    {
      if ((UnityEngine.Object) this.webView == (UnityEngine.Object) null)
        return;
      this.webView.Reload();
    }

    public void About()
    {
      if ((UnityEngine.Object) this.webView == (UnityEngine.Object) null)
        return;
      this.webView.LoadURL("chrome://version");
    }

    public void OnLoadError(string url)
    {
      if ((bool) this.webView)
        ;
    }

    public void ToggleMaximize()
    {
      this.maximized = !this.maximized;
      this.Refresh();
      this.SetFocus(true);
    }

    public void Init()
    {
      if (this.m_GlobalObject != null || string.IsNullOrEmpty(this.m_GlobalObjectTypeName))
        return;
      System.Type type = System.Type.GetType(this.m_GlobalObjectTypeName);
      if (type == null)
        return;
      this.m_GlobalObject = Activator.CreateInstance(type);
      JSProxyMgr.GetInstance().AddGlobalObject(this.m_GlobalObject.GetType().Name, this.m_GlobalObject);
    }

    public void OnGUI()
    {
      Rect webViewRect = GUIClip.Unclip(new Rect(0.0f, 0.0f, this.position.width, this.position.height));
      if (this.m_RepeatedShow-- > 0)
        this.Refresh();
      if (this.m_InitialOpenURL == null)
        return;
      if (!(bool) this.webView)
        this.InitWebView(webViewRect);
      if (Event.current.type != EventType.Layout)
        return;
      this.webView.SetSizeAndPosition((int) webViewRect.x, (int) webViewRect.y, (int) webViewRect.width, (int) webViewRect.height);
    }

    public void OnBatchMode()
    {
      Rect webViewRect = GUIClip.Unclip(new Rect(0.0f, 0.0f, this.position.width, this.position.height));
      if (this.m_InitialOpenURL == null || (bool) this.webView)
        return;
      this.InitWebView(webViewRect);
    }

    public void Refresh()
    {
      if ((UnityEngine.Object) this.webView == (UnityEngine.Object) null)
        return;
      this.webView.Hide();
      this.webView.Show();
    }

    public void OnFocus()
    {
      this.SetFocus(true);
    }

    public void OnLostFocus()
    {
      this.SetFocus(false);
    }

    public void OnEnable()
    {
      this.Init();
    }

    public void OnBecameInvisible()
    {
      if (!(bool) this.webView)
        return;
      this.webView.SetHostView((GUIView) null);
    }

    public void OnDestroy()
    {
      if ((UnityEngine.Object) this.webView != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.webView);
      this.m_GlobalObject = (object) null;
      using (Dictionary<string, WebView>.ValueCollection.Enumerator enumerator = this.m_RegisteredViews.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          WebView current = enumerator.Current;
          if ((UnityEngine.Object) current != (UnityEngine.Object) null)
            UnityEngine.Object.DestroyImmediate((UnityEngine.Object) current);
        }
      }
      this.m_RegisteredViews.Clear();
      this.m_RegisteredViewURLs.Clear();
      this.m_RegisteredViewInstances.Clear();
    }

    public void OnBeforeSerialize()
    {
      this.m_RegisteredViewURLs = new List<string>();
      this.m_RegisteredViewInstances = new List<WebView>();
      using (Dictionary<string, WebView>.Enumerator enumerator = this.m_RegisteredViews.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, WebView> current = enumerator.Current;
          this.m_RegisteredViewURLs.Add(current.Key);
          this.m_RegisteredViewInstances.Add(current.Value);
        }
      }
    }

    public void OnAfterDeserialize()
    {
      this.m_RegisteredViews = new Dictionary<string, WebView>();
      for (int index = 0; index != Math.Min(this.m_RegisteredViewURLs.Count, this.m_RegisteredViewInstances.Count); ++index)
        this.m_RegisteredViews.Add(this.m_RegisteredViewURLs[index], this.m_RegisteredViewInstances[index]);
    }

    private void DoPostLoadTask()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.DoPostLoadTask);
      this.RepaintImmediately();
    }

    private void RaisePostLoadCondition(object obj, ElapsedEventArgs args)
    {
      this.m_PostLoadTimer.Stop();
      this.m_PostLoadTimer = (Timer) null;
      EditorApplication.update += new EditorApplication.CallbackFunction(this.DoPostLoadTask);
    }

    private static string MakeUrlKey(string webViewUrl)
    {
      int length1 = webViewUrl.IndexOf("#");
      string str = length1 == -1 ? webViewUrl : webViewUrl.Substring(0, length1);
      int length2 = str.LastIndexOf("/");
      if (length2 == str.Length - 1)
        return str.Substring(0, length2);
      return str;
    }

    protected void UnregisterWebviewUrl(string webViewUrl)
    {
      this.m_RegisteredViews[WebViewEditorWindow.MakeUrlKey(webViewUrl)] = (WebView) null;
    }

    private void RegisterWebviewUrl(string webViewUrl, WebView view)
    {
      this.m_RegisteredViews[WebViewEditorWindow.MakeUrlKey(webViewUrl)] = view;
    }

    private bool FindWebView(string webViewUrl, out WebView webView)
    {
      webView = (WebView) null;
      return this.m_RegisteredViews.TryGetValue(WebViewEditorWindow.MakeUrlKey(webViewUrl), out webView);
    }

    private void InitWebView(Rect webViewRect)
    {
      if (!(bool) this.webView)
      {
        int x = (int) webViewRect.x;
        int y = (int) webViewRect.y;
        int width = (int) webViewRect.width;
        int height = (int) webViewRect.height;
        this.webView = ScriptableObject.CreateInstance<WebView>();
        this.RegisterWebviewUrl(this.m_InitialOpenURL, this.webView);
        this.webView.InitWebView((GUIView) this.m_Parent, x, y, width, height, false);
        this.webView.hideFlags = HideFlags.HideAndDontSave;
        this.SetFocus(this.hasFocus);
      }
      this.webView.SetDelegateObject((ScriptableObject) this);
      if (this.m_InitialOpenURL.StartsWith("http"))
      {
        this.webView.LoadURL(this.m_InitialOpenURL);
        this.m_PostLoadTimer = new Timer(30.0);
        this.m_PostLoadTimer.Elapsed += new ElapsedEventHandler(this.RaisePostLoadCondition);
        this.m_PostLoadTimer.Enabled = true;
      }
      else if (this.m_InitialOpenURL.StartsWith("file"))
        this.webView.LoadFile(this.m_InitialOpenURL);
      else
        this.webView.LoadFile(Path.Combine(Uri.EscapeUriString(Path.Combine(EditorApplication.applicationContentsPath, "Resources")), this.m_InitialOpenURL));
    }

    public void OnInitScripting()
    {
      this.SetScriptObject();
    }

    protected void NotifyVisibility(bool visible)
    {
      if ((UnityEngine.Object) this.webView == (UnityEngine.Object) null)
        return;
      this.webView.ExecuteJavascript("document.dispatchEvent(new CustomEvent('showWebView',{ detail: { visible:" + (!visible ? "false" : "true") + "}, bubbles: true, cancelable: false }));");
    }

    public WebView GetWebViewFromURL(string url)
    {
      return this.m_RegisteredViews[WebViewEditorWindow.MakeUrlKey(url)];
    }

    protected void LoadPage()
    {
      if (!(bool) this.webView)
        return;
      WebView webView;
      if (!this.FindWebView(this.m_InitialOpenURL, out webView) || (UnityEngine.Object) webView == (UnityEngine.Object) null)
      {
        this.NotifyVisibility(false);
        this.webView.SetHostView((GUIView) null);
        this.webView = (WebView) null;
        this.InitWebView(GUIClip.Unclip(new Rect(0.0f, 0.0f, this.position.width, this.position.height)));
        this.NotifyVisibility(true);
      }
      else
      {
        if (!((UnityEngine.Object) webView != (UnityEngine.Object) this.webView))
          return;
        this.NotifyVisibility(false);
        webView.SetHostView((GUIView) this.m_Parent);
        this.webView.SetHostView((GUIView) null);
        this.webView = webView;
        this.NotifyVisibility(true);
        this.webView.Show();
      }
    }

    private void CreateScriptObject()
    {
      if ((UnityEngine.Object) this.scriptObject != (UnityEngine.Object) null)
        return;
      this.scriptObject = ScriptableObject.CreateInstance<WebScriptObject>();
      this.scriptObject.hideFlags = HideFlags.HideAndDontSave;
      this.scriptObject.webView = this.webView;
    }

    private void SetScriptObject()
    {
      if (!(bool) this.webView)
        return;
      this.CreateScriptObject();
      this.webView.DefineScriptObject("window.webScriptObject", (ScriptableObject) this.scriptObject);
    }

    private void InvokeJSMethod(string objectName, string name, params object[] args)
    {
      if (!(bool) this.webView)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(objectName);
      stringBuilder.Append('.');
      stringBuilder.Append(name);
      stringBuilder.Append('(');
      bool flag1 = true;
      foreach (object obj in args)
      {
        if (!flag1)
          stringBuilder.Append(',');
        bool flag2 = obj is string;
        if (flag2)
          stringBuilder.Append('"');
        stringBuilder.Append(obj);
        if (flag2)
          stringBuilder.Append('"');
        flag1 = false;
      }
      stringBuilder.Append(");");
      this.webView.ExecuteJavascript(stringBuilder.ToString());
    }

    private void SetFocus(bool value)
    {
      if (this.m_SyncingFocus)
        return;
      this.m_SyncingFocus = true;
      if ((UnityEngine.Object) this.webView != (UnityEngine.Object) null)
      {
        if (value)
        {
          this.webView.SetHostView((GUIView) this.m_Parent);
          if (Application.platform != RuntimePlatform.WindowsEditor)
            this.m_RepeatedShow = 15;
          else
            this.webView.Show();
        }
        this.webView.SetApplicationFocus((UnityEngine.Object) this.m_Parent != (UnityEngine.Object) null && this.m_Parent.hasFocus && this.hasFocus);
        this.webView.SetFocus(value);
      }
      this.m_SyncingFocus = false;
    }
  }
}
