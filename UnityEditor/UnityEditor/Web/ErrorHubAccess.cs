// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.ErrorHubAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class ErrorHubAccess : CloudServiceAccess
  {
    private static string kServiceUrl = "file://" + EditorApplication.userJavascriptPackagesPath + "unityeditor-cloud-hub/dist/index.html?failure=unity_connect";
    public const string kServiceName = "ErrorHub";

    public static ErrorHubAccess instance { get; private set; }

    public string errorMessage { get; set; }

    static ErrorHubAccess()
    {
      ErrorHubAccess.instance = new ErrorHubAccess();
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("ErrorHub", ErrorHubAccess.kServiceUrl, (CloudServiceAccess) ErrorHubAccess.instance, "unity/project/cloud/errorhub"));
    }

    public override string GetServiceName()
    {
      return "ErrorHub";
    }
  }
}
