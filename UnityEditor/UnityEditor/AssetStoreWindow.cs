// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetStoreWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Text;
using UnityEditor.Web;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(icon = "Asset Store", title = "Asset Store")]
  internal class AssetStoreWindow : EditorWindow, IHasCustomMenu
  {
    internal WebView webView;
    internal WebScriptObject scriptObject;
    private int m_CurrentSkin;
    private bool m_IsDocked;
    private bool m_IsOffline;
    private bool m_SyncingFocus;
    private int m_RepeatedShow;

    public static void OpenURL(string url)
    {
      AssetStoreWindow.Init().InvokeJSMethod("document.AssetStore", "openURL", (object) url);
      AssetStoreContext.GetInstance().initialOpenURL = url;
    }

    public static AssetStoreWindow Init()
    {
      AssetStoreWindow window = EditorWindow.GetWindow<AssetStoreWindow>(new System.Type[1]
      {
        typeof (SceneView)
      });
      window.SetMinMaxSizes();
      window.Show();
      return window;
    }

    private void SetMinMaxSizes()
    {
      this.minSize = new Vector2(400f, 100f);
      this.maxSize = new Vector2(2048f, 2048f);
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Reload"), false, new GenericMenu.MenuFunction(this.Reload));
    }

    public void Logout()
    {
      this.InvokeJSMethod("document.AssetStore.login", "logout");
    }

    public void Reload()
    {
      this.m_CurrentSkin = EditorGUIUtility.skinIndex;
      this.m_IsDocked = this.docked;
      this.webView.Reload();
    }

    public void OnLoadError(string url)
    {
      if (!(bool) this.webView)
        return;
      if (this.m_IsOffline)
      {
        Debug.LogErrorFormat("Unexpected error: Failed to load offline Asset Store page (url={0})", (object) url);
      }
      else
      {
        this.m_IsOffline = true;
        this.webView.LoadFile(AssetStoreUtils.GetOfflinePath());
      }
    }

    public void OnInitScripting()
    {
      this.SetScriptObject();
    }

    public void OnOpenExternalLink(string url)
    {
      if (!url.StartsWith("http://") && !url.StartsWith("https://"))
        return;
      Application.OpenURL(url);
    }

    public void OnEnable()
    {
      this.SetMinMaxSizes();
      this.titleContent = this.GetLocalizedTitleContent();
      AssetStoreUtils.RegisterDownloadDelegate((ScriptableObject) this);
    }

    public void OnDisable()
    {
      AssetStoreUtils.UnRegisterDownloadDelegate((ScriptableObject) this);
    }

    public void OnDownloadProgress(string id, string message, ulong bytes, ulong total)
    {
      this.InvokeJSMethod("document.AssetStore.pkgs", "OnDownloadProgress", (object) id, (object) message, (object) bytes, (object) total);
    }

    public void OnGUI()
    {
      Rect webViewRect = GUIClip.Unclip(new Rect(0.0f, 0.0f, this.position.width, this.position.height));
      if (!(bool) this.webView)
        this.InitWebView(webViewRect);
      if (this.m_RepeatedShow-- > 0)
        this.Refresh();
      if (Event.current.type != EventType.Layout)
        return;
      this.webView.SetSizeAndPosition((int) webViewRect.x, (int) webViewRect.y, (int) webViewRect.width, (int) webViewRect.height);
      if (this.m_CurrentSkin != EditorGUIUtility.skinIndex)
      {
        this.m_CurrentSkin = EditorGUIUtility.skinIndex;
        this.InvokeJSMethod("document.AssetStore", "refreshSkinIndex");
      }
      this.UpdateDockStatusIfNeeded();
    }

    public void UpdateDockStatusIfNeeded()
    {
      if (this.m_IsDocked == this.docked)
        return;
      this.m_IsDocked = this.docked;
      if (!((UnityEngine.Object) this.scriptObject != (UnityEngine.Object) null))
        return;
      AssetStoreContext.GetInstance().docked = this.docked;
      this.InvokeJSMethod("document.AssetStore", "updateDockStatus");
    }

    public void Refresh()
    {
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

    public void OnBecameInvisible()
    {
      if (!(bool) this.webView)
        return;
      this.webView.SetHostView((GUIView) null);
    }

    public void OnDestroy()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.webView);
    }

    private void InitWebView(Rect webViewRect)
    {
      this.m_CurrentSkin = EditorGUIUtility.skinIndex;
      this.m_IsDocked = this.docked;
      this.m_IsOffline = false;
      if (!(bool) this.webView)
      {
        int x = (int) webViewRect.x;
        int y = (int) webViewRect.y;
        int width = (int) webViewRect.width;
        int height = (int) webViewRect.height;
        this.webView = ScriptableObject.CreateInstance<WebView>();
        this.webView.InitWebView((GUIView) this.m_Parent, x, y, width, height, false);
        this.webView.hideFlags = HideFlags.HideAndDontSave;
        this.webView.AllowRightClickMenu(true);
        if (this.hasFocus)
          this.SetFocus(true);
      }
      this.webView.SetDelegateObject((ScriptableObject) this);
      this.webView.LoadFile(AssetStoreUtils.GetLoaderPath());
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
      this.webView.DefineScriptObject("window.unityScriptObject", (ScriptableObject) this.scriptObject);
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
      if ((bool) this.webView)
      {
        if (value)
        {
          this.webView.SetHostView((GUIView) this.m_Parent);
          this.webView.Show();
          this.m_RepeatedShow = 5;
        }
        this.webView.SetFocus(value);
      }
      this.m_SyncingFocus = false;
    }
  }
}
