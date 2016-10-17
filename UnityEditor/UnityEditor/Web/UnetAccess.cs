// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.UnetAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Connect;
using UnityEngine;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class UnetAccess : CloudServiceAccess
  {
    private const string kServiceName = "UNet";
    private const string kServiceDisplayName = "Multiplayer";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/unet";
    private const string kMultiplayerNetworkingIdKey = "CloudNetworkingId";

    static UnetAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("UNet", "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/unet", (CloudServiceAccess) new UnetAccess(), "unity/project/cloud/networking"));
    }

    public override string GetServiceName()
    {
      return "UNet";
    }

    public override string GetServiceDisplayName()
    {
      return "Multiplayer";
    }

    public void SetMultiplayerId(int id)
    {
      PlayerSettings.InitializePropertyInt("CloudNetworkingId", id);
      PlayerPrefs.SetString("CloudNetworkingId", id.ToString());
      PlayerPrefs.Save();
    }
  }
}
