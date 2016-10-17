// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.AnalyticsAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Connect;
using UnityEngine.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class AnalyticsAccess : CloudServiceAccess
  {
    private const string kServiceName = "Analytics";
    private const string kServiceDisplayName = "Analytics";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/analytics";

    static AnalyticsAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Analytics", "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/analytics", (CloudServiceAccess) new AnalyticsAccess(), "unity/project/cloud/analytics"));
    }

    public override string GetServiceName()
    {
      return "Analytics";
    }

    public override string GetServiceDisplayName()
    {
      return "Analytics";
    }

    public override bool IsServiceEnabled()
    {
      return UnityAnalyticsSettings.enabled;
    }

    public override void EnableService(bool enabled)
    {
      UnityAnalyticsSettings.enabled = enabled;
    }
  }
}
