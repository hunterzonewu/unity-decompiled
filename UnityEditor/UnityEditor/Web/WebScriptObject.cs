// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.WebScriptObject
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Web
{
  internal class WebScriptObject : ScriptableObject
  {
    private WebView m_WebView;

    public WebView webView
    {
      get
      {
        return this.m_WebView;
      }
      set
      {
        this.m_WebView = value;
      }
    }

    private WebScriptObject()
    {
      this.m_WebView = (WebView) null;
    }

    public bool ProcessMessage(string jsonRequest, WebViewV8CallbackCSharp callback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WebScriptObject.\u003CProcessMessage\u003Ec__AnonStoreyA3 messageCAnonStoreyA3 = new WebScriptObject.\u003CProcessMessage\u003Ec__AnonStoreyA3();
      // ISSUE: reference to a compiler-generated field
      messageCAnonStoreyA3.callback = callback;
      if ((Object) this.m_WebView != (Object) null)
      {
        // ISSUE: reference to a compiler-generated method
        return JSProxyMgr.GetInstance().DoMessage(jsonRequest, new JSProxyMgr.ExecCallback(messageCAnonStoreyA3.\u003C\u003Em__1D4), this.m_WebView);
      }
      return false;
    }

    public bool processMessage(string jsonRequest, WebViewV8CallbackCSharp callback)
    {
      return this.ProcessMessage(jsonRequest, callback);
    }
  }
}
