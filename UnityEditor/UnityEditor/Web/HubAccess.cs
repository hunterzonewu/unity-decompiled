// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.HubAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class HubAccess : CloudServiceAccess
  {
    public const string kServiceName = "Hub";
    private const string kServiceDisplayName = "Services";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/hub";

    static HubAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Hub", "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/hub", (CloudServiceAccess) new HubAccess(), "unity/project/cloud/hub"));
    }

    public override string GetServiceName()
    {
      return "Hub";
    }

    public override string GetServiceDisplayName()
    {
      return "Services";
    }

    public UnityConnectServiceCollection.ServiceInfo[] GetServices()
    {
      return UnityConnectServiceCollection.instance.GetAllServiceInfos();
    }

    public void ShowService(string name)
    {
      UnityConnectServiceCollection.instance.ShowService(name, true);
    }

    public void EnableCloudService(string name, bool enabled)
    {
      UnityConnectServiceCollection.instance.EnableService(name, enabled);
    }

    [MenuItem("Window/Services %0", false, 1999)]
    private static void ShowMyWindow()
    {
      UnityConnectServiceCollection.instance.ShowService("Hub", true);
    }
  }
}
