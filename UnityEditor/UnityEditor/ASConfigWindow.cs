// Decompiled with JetBrains decompiler
// Type: UnityEditor.ASConfigWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class ASConfigWindow
  {
    private ListViewState serversLv = new ListViewState(0);
    private ListViewState projectsLv = new ListViewState(0);
    private string serverAddress = string.Empty;
    private string projectName = string.Empty;
    private string userName = string.Empty;
    private string password = string.Empty;
    private string port = string.Empty;
    private const int listLenghts = 20;
    private static ASMainWindow.Constants constants;
    private bool resetKeyboardControl;
    private string[] projectsList;
    private string[] serversList;
    private PListConfig plc;
    private ASMainWindow parentWin;

    public ASConfigWindow(ASMainWindow parent)
    {
      this.parentWin = parent;
      this.LoadConfig();
    }

    private void LoadConfig()
    {
      PListConfig plistConfig = new PListConfig("Library/ServerPreferences.plist");
      this.serverAddress = plistConfig["Maint Server"];
      this.userName = plistConfig["Maint UserName"];
      this.port = plistConfig["Maint port number"];
      this.projectName = plistConfig["Maint project name"];
      this.password = ASEditorBackend.GetPassword(this.serverAddress, this.userName);
      if (this.port != string.Empty && this.port != 10733.ToString())
      {
        ASConfigWindow asConfigWindow = this;
        string str = asConfigWindow.serverAddress + ":" + this.port;
        asConfigWindow.serverAddress = str;
      }
      this.serversList = InternalEditorUtility.GetEditorSettingsList("ASServer", 20);
      this.serversLv.totalRows = this.serversList.Length;
      if (!ArrayUtility.Contains<string>(this.serversList, this.serverAddress))
        return;
      this.serversLv.row = ArrayUtility.IndexOf<string>(this.serversList, this.serverAddress);
    }

    private void GetUserAndPassword()
    {
      string user = ASEditorBackend.GetUser(this.serverAddress);
      if (user != string.Empty)
        this.userName = user;
      string password = ASEditorBackend.GetPassword(this.serverAddress, user);
      if (!(password != string.Empty))
        return;
      this.password = password;
    }

    private void GetDefaultPListConfig()
    {
      this.plc = new PListConfig("Library/ServerPreferences.plist");
      this.plc["Maint Server"] = string.Empty;
      this.plc["Maint UserName"] = string.Empty;
      this.plc["Maint database name"] = string.Empty;
      this.plc["Maint port number"] = string.Empty;
      this.plc["Maint project name"] = string.Empty;
      this.plc["Maint Password"] = string.Empty;
      if (this.plc["Maint settings type"] == string.Empty)
        this.plc["Maint settings type"] = "manual";
      if (this.plc["Maint Timeout"] == string.Empty)
        this.plc["Maint Timeout"] = "5";
      if (!(this.plc["Maint Connection Settings"] == string.Empty))
        return;
      this.plc["Maint Connection Settings"] = string.Empty;
    }

    private void DoShowProjects()
    {
      int result = 10733;
      string server = this.serverAddress;
      if (server.IndexOf(":") > 0)
      {
        int.TryParse(server.Substring(server.IndexOf(":") + 1), out result);
        server = server.Substring(0, server.IndexOf(":"));
      }
      AssetServer.AdminSetCredentials(server, result, this.userName, this.password);
      MaintDatabaseRecord[] maintDatabaseRecordArray = AssetServer.AdminRefreshDatabases();
      if (maintDatabaseRecordArray != null)
      {
        this.projectsList = new string[maintDatabaseRecordArray.Length];
        for (int index = 0; index < maintDatabaseRecordArray.Length; ++index)
          this.projectsList[index] = maintDatabaseRecordArray[index].name;
        this.projectsLv.totalRows = maintDatabaseRecordArray.Length;
        this.GetDefaultPListConfig();
        this.plc["Maint Server"] = server;
        this.plc["Maint UserName"] = this.userName;
        this.plc["Maint port number"] = this.port;
        this.plc.Save();
        ASEditorBackend.SetPassword(server, this.userName, this.password);
        ASEditorBackend.AddUser(this.serverAddress, this.userName);
      }
      else
        this.projectsLv.totalRows = 0;
    }

    private void ClearConfig()
    {
      if (!EditorUtility.DisplayDialog("Clear Configuration", "Are you sure you want to disconnect from Asset Server project and clear all configuration values?", "Clear", "Cancel"))
        return;
      this.plc = new PListConfig("Library/ServerPreferences.plist");
      this.plc.Clear();
      this.plc.Save();
      this.LoadConfig();
      this.projectsLv.totalRows = 0;
      ASEditorBackend.InitializeMaintBinding();
      this.resetKeyboardControl = true;
    }

    private void DoConnect()
    {
      AssetServer.RemoveMaintErrorsFromConsole();
      int result = 10733;
      string server = this.serverAddress;
      if (server.IndexOf(":") > 0)
      {
        int.TryParse(server.Substring(server.IndexOf(":") + 1), out result);
        server = server.Substring(0, server.IndexOf(":"));
      }
      this.port = result.ToString();
      string databaseName = AssetServer.GetDatabaseName(server, this.userName, this.password, this.port, this.projectName);
      this.GetDefaultPListConfig();
      this.plc["Maint Server"] = server;
      this.plc["Maint UserName"] = this.userName;
      this.plc["Maint database name"] = databaseName;
      this.plc["Maint port number"] = this.port;
      this.plc["Maint project name"] = this.projectName;
      this.plc.Save();
      if (ArrayUtility.Contains<string>(this.serversList, this.serverAddress))
        ArrayUtility.Remove<string>(ref this.serversList, this.serverAddress);
      ArrayUtility.Insert<string>(ref this.serversList, 0, this.serverAddress);
      ASEditorBackend.AddUser(this.serverAddress, this.userName);
      ASEditorBackend.SetPassword(this.serverAddress, this.userName, this.password);
      InternalEditorUtility.SaveEditorSettingsList("ASServer", this.serversList, 20);
      if (databaseName != string.Empty)
      {
        ASEditorBackend.InitializeMaintBinding();
        this.parentWin.Reinit();
        GUIUtility.ExitGUI();
      }
      else
      {
        this.parentWin.NeedsSetup = true;
        this.parentWin.Repaint();
      }
    }

    private void ServersPopup()
    {
      if (this.serversList.Length <= 0)
        return;
      int index = EditorGUILayout.Popup(-1, this.serversList, ASConfigWindow.constants.dropDown, new GUILayoutOption[1]
      {
        GUILayout.MaxWidth(18f)
      });
      if (index < 0)
        return;
      GUIUtility.keyboardControl = 0;
      GUIUtility.hotControl = 0;
      this.resetKeyboardControl = true;
      this.serverAddress = this.serversList[index];
      this.parentWin.Repaint();
    }

    private void DoConfigGUI()
    {
      bool enabled1 = GUI.enabled;
      bool changed1 = GUI.changed;
      GUI.changed = false;
      bool flag1 = false;
      bool flag2 = false;
      Event current = Event.current;
      if (current.type == EventType.KeyDown)
      {
        bool flag3;
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
          flag3 = (int) current.character == 10 || (int) current.character == 3 || current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter;
        }
        else
        {
          if (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)
            current.Use();
          flag3 = (int) current.character == 10 || (int) current.character == 3;
        }
        if (flag3)
        {
          string ofFocusedControl = GUI.GetNameOfFocusedControl();
          if (ofFocusedControl != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (ASConfigWindow.\u003C\u003Ef__switch\u0024map10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ASConfigWindow.\u003C\u003Ef__switch\u0024map10 = new Dictionary<string, int>(2)
              {
                {
                  "password",
                  0
                },
                {
                  "project",
                  1
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (ASConfigWindow.\u003C\u003Ef__switch\u0024map10.TryGetValue(ofFocusedControl, out num))
            {
              if (num != 0)
              {
                if (num == 1)
                {
                  flag2 = true;
                  goto label_16;
                }
              }
              else
              {
                flag1 = true;
                goto label_16;
              }
            }
          }
          current.Use();
        }
      }
label_16:
      GUILayout.BeginHorizontal();
      this.serverAddress = EditorGUILayout.TextField("Server", this.serverAddress, new GUILayoutOption[0]);
      this.ServersPopup();
      GUILayout.EndHorizontal();
      if (GUI.changed)
        this.GetUserAndPassword();
      GUI.changed |= changed1;
      this.userName = EditorGUILayout.TextField("User Name", this.userName, new GUILayoutOption[0]);
      GUI.SetNextControlName("password");
      this.password = EditorGUILayout.PasswordField("Password", this.password, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.enabled = this.userName != string.Empty && this.password != string.Empty && this.serverAddress != string.Empty && enabled1;
      if (GUILayout.Button("Show Projects", new GUILayoutOption[1]
      {
        GUILayout.MinWidth(100f)
      }) || flag1 && GUI.enabled)
      {
        this.DoShowProjects();
        this.projectName = string.Empty;
        EditorGUI.FocusTextInControl("project");
      }
      bool enabled2 = GUI.enabled;
      GUI.enabled = enabled1;
      if (GUILayout.Button("Clear Configuration"))
        this.ClearConfig();
      GUI.enabled = enabled2;
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      bool changed2 = GUI.changed;
      GUI.changed = false;
      GUI.SetNextControlName("project");
      this.projectName = EditorGUILayout.TextField("Project Name", this.projectName, new GUILayoutOption[0]);
      GUI.changed |= changed2;
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.enabled = this.userName != string.Empty && this.password != string.Empty && (this.serverAddress != string.Empty && this.projectName != string.Empty) && enabled1;
      if (GUILayout.Button("Connect", ASConfigWindow.constants.bigButton, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(100f)
      }) || flag2 && GUI.enabled)
        this.DoConnect();
      GUI.enabled = enabled1;
      GUILayout.EndHorizontal();
    }

    private void DoProjectsGUI()
    {
      GUILayout.BeginVertical(ASConfigWindow.constants.groupBox, new GUILayoutOption[0]);
      GUILayout.Label("Projects on Server", ASConfigWindow.constants.title, new GUILayoutOption[0]);
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.projectsLv, ASConfigWindow.constants.background))
      {
        if (listViewElement.row == this.projectsLv.row && Event.current.type == EventType.Repaint)
          ASConfigWindow.constants.entrySelected.Draw(listViewElement.position, false, false, false, false);
        GUILayout.Label(this.projectsList[listViewElement.row], ASConfigWindow.constants.element, new GUILayoutOption[0]);
      }
      if (this.projectsLv.selectionChanged)
        this.projectName = this.projectsList[this.projectsLv.row];
      GUILayout.EndVertical();
    }

    public bool DoGUI()
    {
      if (ASConfigWindow.constants == null)
        ASConfigWindow.constants = new ASMainWindow.Constants();
      if (this.resetKeyboardControl)
      {
        this.resetKeyboardControl = false;
        GUIUtility.keyboardControl = 0;
      }
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical(ASConfigWindow.constants.groupBox, new GUILayoutOption[0]);
      GUILayout.Box("Server Connection", ASConfigWindow.constants.title, new GUILayoutOption[0]);
      GUILayout.BeginVertical(ASConfigWindow.constants.contentBox, new GUILayoutOption[0]);
      this.DoConfigGUI();
      if (AssetServer.GetAssetServerError() != string.Empty)
      {
        GUILayout.Space(10f);
        GUILayout.Label(AssetServer.GetAssetServerError(), ASConfigWindow.constants.errorLabel, new GUILayoutOption[0]);
        GUILayout.Space(10f);
      }
      GUILayout.EndVertical();
      GUILayout.EndVertical();
      this.DoProjectsGUI();
      GUILayout.EndHorizontal();
      return true;
    }
  }
}
