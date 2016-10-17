// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityConnectServiceData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor.Web;

namespace UnityEditor.Connect
{
  internal class UnityConnectServiceData
  {
    private readonly string m_ServiceName;
    private readonly string m_HtmlSourcePath;
    private readonly CloudServiceAccess m_JavascriptGlobalObject;
    private readonly string m_JsGlobalObjectName;

    public string serviceName
    {
      get
      {
        return this.m_ServiceName;
      }
    }

    public string serviceUrl
    {
      get
      {
        return UnityConnectPrefs.FixUrl(this.m_HtmlSourcePath, this.m_ServiceName);
      }
    }

    public CloudServiceAccess serviceJsGlobalObject
    {
      get
      {
        return this.m_JavascriptGlobalObject;
      }
    }

    public string serviceJsGlobalObjectName
    {
      get
      {
        return this.m_JsGlobalObjectName;
      }
    }

    public UnityConnectServiceData(string serviceName, string htmlSourcePath, CloudServiceAccess jsGlobalObject, string jsGlobalObjectName)
    {
      if (string.IsNullOrEmpty(serviceName))
        throw new ArgumentNullException("serviceName");
      if (string.IsNullOrEmpty(htmlSourcePath))
        throw new ArgumentNullException("htmlSourcePath");
      this.m_ServiceName = serviceName;
      this.m_HtmlSourcePath = htmlSourcePath;
      this.m_JavascriptGlobalObject = jsGlobalObject;
      this.m_JsGlobalObjectName = jsGlobalObjectName;
      if (this.m_JavascriptGlobalObject == null)
        return;
      if (string.IsNullOrEmpty(this.m_JsGlobalObjectName))
        this.m_JsGlobalObjectName = this.m_ServiceName;
      JSProxyMgr.GetInstance().AddGlobalObject(this.m_JsGlobalObjectName, (object) this.m_JavascriptGlobalObject);
    }

    public void EnableService(bool enabled)
    {
      if (this.m_JavascriptGlobalObject == null)
        return;
      this.m_JavascriptGlobalObject.EnableService(enabled);
    }
  }
}
