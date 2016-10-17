// Decompiled with JetBrains decompiler
// Type: UnityEditor.Connect.UnityConnectPrefs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Connect
{
  internal class UnityConnectPrefs
  {
    public static string[] kEnvironmentFamilies = new string[4]{ "Production", "Staging", "Dev", "Custom" };
    protected static Dictionary<string, UnityConnectPrefs.CloudPanelPref> m_CloudPanelPref = new Dictionary<string, UnityConnectPrefs.CloudPanelPref>();
    public const int kProductionEnv = 0;
    public const int kCustomEnv = 3;
    public const string kSvcEnvPref = "CloudPanelServer";
    public const string kSvcCustomUrlPref = "CloudPanelCustomUrl";
    public const string kSvcCustomPortPref = "CloudPanelCustomPort";

    protected static UnityConnectPrefs.CloudPanelPref GetPanelPref(string serviceName)
    {
      if (UnityConnectPrefs.m_CloudPanelPref.ContainsKey(serviceName))
        return UnityConnectPrefs.m_CloudPanelPref[serviceName];
      UnityConnectPrefs.CloudPanelPref cloudPanelPref = new UnityConnectPrefs.CloudPanelPref(serviceName);
      UnityConnectPrefs.m_CloudPanelPref.Add(serviceName, cloudPanelPref);
      return cloudPanelPref;
    }

    public static int GetServiceEnv(string serviceName)
    {
      if (Unsupported.IsDeveloperBuild() || UnityConnect.preferencesEnabled)
        return EditorPrefs.GetInt(UnityConnectPrefs.ServicePrefKey("CloudPanelServer", serviceName));
      for (int index = 0; index < UnityConnectPrefs.kEnvironmentFamilies.Length; ++index)
      {
        if (UnityConnectPrefs.kEnvironmentFamilies[index].Equals(UnityConnect.instance.configuration, StringComparison.InvariantCultureIgnoreCase))
          return index;
      }
      return 0;
    }

    public static string ServicePrefKey(string baseKey, string serviceName)
    {
      return baseKey + "/" + serviceName;
    }

    public static string FixUrl(string url, string serviceName)
    {
      string str1 = url;
      int serviceEnv = UnityConnectPrefs.GetServiceEnv(serviceName);
      if (serviceEnv != 0)
      {
        if (str1.StartsWith("http://") || str1.StartsWith("https://"))
          return serviceEnv != 3 ? str1.ToLower().Replace("/" + UnityConnectPrefs.kEnvironmentFamilies[0].ToLower() + "/", "/" + UnityConnectPrefs.kEnvironmentFamilies[serviceEnv].ToLower() + "/") : EditorPrefs.GetString(UnityConnectPrefs.ServicePrefKey("CloudPanelCustomUrl", serviceName)) + ":" + (object) EditorPrefs.GetInt(UnityConnectPrefs.ServicePrefKey("CloudPanelCustomPort", serviceName));
        if (str1.StartsWith("file://"))
        {
          string str2 = str1.Substring(7);
          if (serviceEnv == 3)
            str2 = EditorPrefs.GetString(UnityConnectPrefs.ServicePrefKey("CloudPanelCustomUrl", serviceName)) + ":" + (object) EditorPrefs.GetInt(UnityConnectPrefs.ServicePrefKey("CloudPanelCustomPort", serviceName));
          return str2;
        }
        if (!str1.StartsWith("file://") && !str1.StartsWith("http://") && !str1.StartsWith("https://"))
          return "http://" + str1;
      }
      return str1;
    }

    public static void ShowPanelPrefUI()
    {
      List<string> allServiceNames = UnityConnectServiceCollection.instance.GetAllServiceNames();
      bool flag = false;
      using (List<string>.Enumerator enumerator = allServiceNames.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          UnityConnectPrefs.CloudPanelPref panelPref = UnityConnectPrefs.GetPanelPref(current);
          int result = EditorGUILayout.Popup(current, panelPref.m_CloudPanelServer, UnityConnectPrefs.kEnvironmentFamilies, new GUILayoutOption[0]);
          if (result != panelPref.m_CloudPanelServer)
          {
            panelPref.m_CloudPanelServer = result;
            flag = true;
          }
          if (panelPref.m_CloudPanelServer == 3)
          {
            ++EditorGUI.indentLevel;
            string str = EditorGUILayout.TextField("Custom server URL", panelPref.m_CloudPanelCustomUrl, new GUILayoutOption[0]);
            if (str != panelPref.m_CloudPanelCustomUrl)
            {
              panelPref.m_CloudPanelCustomUrl = str;
              flag = true;
            }
            int.TryParse(EditorGUILayout.TextField("Custom server port", panelPref.m_CloudPanelCustomPort.ToString(), new GUILayoutOption[0]), out result);
            if (result != panelPref.m_CloudPanelCustomPort)
            {
              panelPref.m_CloudPanelCustomPort = result;
              flag = true;
            }
            --EditorGUI.indentLevel;
          }
        }
      }
      if (!flag)
        return;
      UnityConnectServiceCollection.instance.ReloadServices();
    }

    public static void StorePanelPrefs()
    {
      if (!Unsupported.IsDeveloperBuild() && !UnityConnect.preferencesEnabled)
        return;
      using (Dictionary<string, UnityConnectPrefs.CloudPanelPref>.Enumerator enumerator = UnityConnectPrefs.m_CloudPanelPref.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Value.StoreCloudServicePref();
      }
    }

    protected class CloudPanelPref
    {
      public string m_ServiceName;
      public int m_CloudPanelServer;
      public string m_CloudPanelCustomUrl;
      public int m_CloudPanelCustomPort;

      public CloudPanelPref(string serviceName)
      {
        this.m_ServiceName = serviceName;
        this.m_CloudPanelServer = UnityConnectPrefs.GetServiceEnv(this.m_ServiceName);
        this.m_CloudPanelCustomUrl = EditorPrefs.GetString(UnityConnectPrefs.ServicePrefKey("CloudPanelCustomUrl", this.m_ServiceName));
        this.m_CloudPanelCustomPort = EditorPrefs.GetInt(UnityConnectPrefs.ServicePrefKey("CloudPanelCustomPort", this.m_ServiceName));
      }

      public void StoreCloudServicePref()
      {
        EditorPrefs.SetInt(UnityConnectPrefs.ServicePrefKey("CloudPanelServer", this.m_ServiceName), this.m_CloudPanelServer);
        EditorPrefs.SetString(UnityConnectPrefs.ServicePrefKey("CloudPanelCustomUrl", this.m_ServiceName), this.m_CloudPanelCustomUrl);
        EditorPrefs.SetInt(UnityConnectPrefs.ServicePrefKey("CloudPanelCustomPort", this.m_ServiceName), this.m_CloudPanelCustomPort);
      }
    }
  }
}
