// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.CloudServiceAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Connect;

namespace UnityEditor.Web
{
  internal abstract class CloudServiceAccess
  {
    private const string kServiceEnabled = "ServiceEnabled";

    public abstract string GetServiceName();

    protected WebView GetWebView()
    {
      return UnityConnectServiceCollection.instance.GetWebViewFromServiceName(this.GetServiceName());
    }

    protected string GetSafeServiceName()
    {
      return this.GetServiceName().Replace(' ', '_');
    }

    public virtual string GetServiceDisplayName()
    {
      return this.GetServiceName();
    }

    public virtual bool IsServiceEnabled()
    {
      bool result;
      bool.TryParse(this.GetServiceConfig("ServiceEnabled"), out result);
      return result;
    }

    public virtual void EnableService(bool enabled)
    {
      this.SetServiceConfig("ServiceEnabled", enabled.ToString());
    }

    public string GetServiceConfig(string key)
    {
      string name = this.GetSafeServiceName() + "_" + key;
      string empty = string.Empty;
      if (PlayerSettings.GetPropertyOptionalString(name, ref empty))
        return empty;
      return string.Empty;
    }

    public void SetServiceConfig(string key, string value)
    {
      string name = this.GetSafeServiceName() + "_" + key;
      string empty = string.Empty;
      if (!PlayerSettings.GetPropertyOptionalString(name, ref empty))
        PlayerSettings.InitializePropertyString(name, value);
      else
        PlayerSettings.SetPropertyString(name, value);
    }

    public void GoBackToHub()
    {
      UnityConnectServiceCollection.instance.ShowService("Hub", true);
    }
  }
}
