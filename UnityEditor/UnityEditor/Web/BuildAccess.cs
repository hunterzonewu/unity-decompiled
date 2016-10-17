// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.BuildAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class BuildAccess : CloudServiceAccess
  {
    private const string kServiceName = "Build";
    private const string kServiceDisplayName = "Unity Build";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/build";

    static BuildAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Build", "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/build", (CloudServiceAccess) new BuildAccess(), "unity/project/cloud/build"));
    }

    public override string GetServiceName()
    {
      return "Build";
    }

    public override string GetServiceDisplayName()
    {
      return "Unity Build";
    }
  }
}
