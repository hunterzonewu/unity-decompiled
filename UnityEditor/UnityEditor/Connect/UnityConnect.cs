// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityConnect
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEditor.Web;
using UnityEngine;

namespace UnityEditor.Connect
{
  [InitializeOnLoad]
  internal sealed class UnityConnect
  {
    private static readonly UnityConnect s_Instance = new UnityConnect();

    public static extern bool preferencesEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern bool skipMissingUPID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool online { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool loggedIn { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool projectValid { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool workingOffline { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string configuration { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public string lastErrorMessage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int lastErrorCode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public UserInfo userInfo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public ProjectInfo projectInfo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public ConnectInfo connectInfo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool canBuildWithUPID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static UnityConnect instance
    {
      get
      {
        return UnityConnect.s_Instance;
      }
    }

    public event StateChangedDelegate StateChanged;

    public event ProjectStateChangedDelegate ProjectStateChanged;

    static UnityConnect()
    {
      JSProxyMgr.GetInstance().AddGlobalObject("unity/connect", (object) UnityConnect.s_Instance);
    }

    private UnityConnect()
    {
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetConfigurationURL(CloudConfigUrl config);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetEnvironment();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetAPIVersion();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetUserName();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetAccessToken();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetProjectGUID();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetProjectName();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetOrganizationId();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public string GetOrganizationName();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetOrganizationForeignKey();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RefreshProject();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearCache();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Logout();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void WorkOffline(bool rememberDecision);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ShowLogin();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void OpenAuthorizedURLInWebBrowser(string url);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void BindProject(string projectGUID, string projectName, string organizationId);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UnbindProject();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool SetCOPPACompliance(COPPACompliance compliance);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearErrors();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void UnhandledError(string request, int responseCode, string response);

    public void GoToHub(string page)
    {
      UnityConnectServiceCollection.instance.ShowService("Hub", page, true);
    }

    public ProjectInfo GetProjectInfo()
    {
      return this.projectInfo;
    }

    public UserInfo GetUserInfo()
    {
      return this.userInfo;
    }

    public ConnectInfo GetConnectInfo()
    {
      return this.connectInfo;
    }

    public string GetConfigurationUrlByIndex(int index)
    {
      if (index == 0)
        return this.GetConfigurationURL(CloudConfigUrl.CloudCore);
      if (index == 1)
        return this.GetConfigurationURL(CloudConfigUrl.CloudCollab);
      if (index == 2)
        return this.GetConfigurationURL(CloudConfigUrl.CloudWebauth);
      if (index == 3)
        return this.GetConfigurationURL(CloudConfigUrl.CloudLogin);
      return string.Empty;
    }

    public string GetCoreConfigurationUrl()
    {
      return this.GetConfigurationURL(CloudConfigUrl.CloudCore);
    }

    public bool DisplayDialog(string title, string message, string okBtn, string cancelBtn)
    {
      return EditorUtility.DisplayDialog(title, message, okBtn, cancelBtn);
    }

    public bool SetCOPPACompliance(int compliance)
    {
      return this.SetCOPPACompliance((COPPACompliance) compliance);
    }

    private static void OnStateChanged()
    {
      StateChangedDelegate stateChanged = UnityConnect.instance.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged(UnityConnect.instance.connectInfo);
    }

    private static void OnProjectStateChanged()
    {
      ProjectStateChangedDelegate projectStateChanged = UnityConnect.instance.ProjectStateChanged;
      if (projectStateChanged == null)
        return;
      projectStateChanged(UnityConnect.instance.projectInfo);
    }
  }
}
