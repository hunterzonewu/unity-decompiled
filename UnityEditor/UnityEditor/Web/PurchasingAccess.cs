// Decompiled with JetBrains decompiler
// Type: UnityEditor.Web.PurchasingAccess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Security;
using UnityEditor.Connect;
using UnityEngine;
using UnityEngine.Connect;

namespace UnityEditor.Web
{
  [InitializeOnLoad]
  internal class PurchasingAccess : CloudServiceAccess
  {
    private static readonly Uri kPackageUri = new Uri("https://public-cdn.cloud.unity3d.com/UnityEngine.Cloud.Purchasing.unitypackage");
    private const string kServiceName = "Purchasing";
    private const string kServiceDisplayName = "In App Purchasing";
    private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/purchasing";
    private bool m_InstallInProgress;

    static PurchasingAccess()
    {
      UnityConnectServiceCollection.instance.AddService(new UnityConnectServiceData("Purchasing", "https://public-cdn.cloud.unity3d.com/editor/5.3/production/cloud/purchasing", (CloudServiceAccess) new PurchasingAccess(), "unity/project/cloud/purchasing"));
    }

    public override string GetServiceName()
    {
      return "Purchasing";
    }

    public override string GetServiceDisplayName()
    {
      return "In App Purchasing";
    }

    public override bool IsServiceEnabled()
    {
      return UnityPurchasingSettings.enabled;
    }

    public override void EnableService(bool enabled)
    {
      UnityPurchasingSettings.enabled = enabled;
    }

    public void InstallUnityPackage()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PurchasingAccess.\u003CInstallUnityPackage\u003Ec__AnonStoreyB8 packageCAnonStoreyB8 = new PurchasingAccess.\u003CInstallUnityPackage\u003Ec__AnonStoreyB8();
      // ISSUE: reference to a compiler-generated field
      packageCAnonStoreyB8.\u003C\u003Ef__this = this;
      if (this.m_InstallInProgress)
        return;
      // ISSUE: reference to a compiler-generated field
      packageCAnonStoreyB8.originalCallback = ServicePointManager.ServerCertificateValidationCallback;
      if (Application.platform != RuntimePlatform.OSXEditor)
        ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) ((a, b, c, d) => true);
      this.m_InstallInProgress = true;
      // ISSUE: reference to a compiler-generated field
      packageCAnonStoreyB8.location = FileUtil.GetUniqueTempPathInProject();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      packageCAnonStoreyB8.location = Path.ChangeExtension(packageCAnonStoreyB8.location, ".unitypackage");
      WebClient webClient = new WebClient();
      // ISSUE: reference to a compiler-generated method
      webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(packageCAnonStoreyB8.\u003C\u003Em__227);
      // ISSUE: reference to a compiler-generated field
      webClient.DownloadFileAsync(PurchasingAccess.kPackageUri, packageCAnonStoreyB8.location);
    }

    private void ExecuteJSMethod(string name)
    {
      this.ExecuteJSMethod(name, (string) null);
    }

    private void ExecuteJSMethod(string name, string arg)
    {
      string scriptCode = string.Format("UnityPurchasing.{0}({1})", (object) name, arg != null ? (object) string.Format("\"{0}\"", (object) arg) : (object) string.Empty);
      WebView webView = this.GetWebView();
      if ((UnityEngine.Object) webView == (UnityEngine.Object) null)
        return;
      webView.ExecuteJavascript(scriptCode);
    }
  }
}
