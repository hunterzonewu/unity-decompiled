// Decompiled with JetBrains decompiler
// Type: UnityEditor.ASEditorBackend
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal class ASEditorBackend
  {
    public const string kServerSettingsFile = "Library/ServerPreferences.plist";
    public const string kUserName = "Maint UserName";
    public const string kPassword = "Maint Password";
    public const string kTimeout = "Maint Timeout";
    public const string kSettingsType = "Maint settings type";
    public const string kConnectionSettings = "Maint Connection Settings";
    public const string kPortNumber = "Maint port number";
    public const string kServer = "Maint Server";
    public const string kDatabaseName = "Maint database name";
    public const string kProjectName = "Maint project name";
    public const int kDefaultServerPort = 10733;
    public static ASMainWindow asMainWin;
    private static string s_TestingConflictResClass;
    private static string s_TestingConflictResFunction;

    public static ASMainWindow ASWin
    {
      get
      {
        if ((UnityEngine.Object) ASEditorBackend.asMainWin != (UnityEngine.Object) null)
          return ASEditorBackend.asMainWin;
        return EditorWindow.GetWindowDontShow<ASMainWindow>();
      }
    }

    public static void DoAS()
    {
      if (ASEditorBackend.ASWin.Error)
        return;
      ASEditorBackend.ASWin.Show();
      ASEditorBackend.ASWin.Focus();
    }

    public static void ShowASConflictResolutionsWindow(string[] conflicting)
    {
      ASEditorBackend.ASWin.ShowConflictResolutions(conflicting);
    }

    public static void CommitItemsChanged()
    {
      if (!((UnityEngine.Object) ASEditorBackend.asMainWin != (UnityEngine.Object) null) && (!((UnityEngine.Object) ASEditorBackend.asMainWin == (UnityEngine.Object) null) || Resources.FindObjectsOfTypeAll(typeof (ASMainWindow)).Length == 0))
        return;
      ASEditorBackend.ASWin.CommitItemsChanged();
    }

    public static void CBReinitCommitWindow(int actionResult)
    {
      if (ASEditorBackend.ASWin.asCommitWin == null)
        return;
      ASEditorBackend.ASWin.asCommitWin.Reinit(actionResult != 0);
    }

    public static void CBCommitFinished(int actionResult)
    {
      if (ASEditorBackend.ASWin.asCommitWin == null)
        return;
      ASEditorBackend.ASWin.asCommitWin.CommitFinished(actionResult != 0);
    }

    public static void CBOverviewsCommitFinished(int actionResult)
    {
      if (!((UnityEngine.Object) ASEditorBackend.ASWin != (UnityEngine.Object) null))
        return;
      ASEditorBackend.ASWin.CommitFinished(actionResult != 0);
    }

    public static void CBReinitOnSuccess(int actionResult)
    {
      if (actionResult != 0)
        ASEditorBackend.ASWin.Reinit();
      else
        ASEditorBackend.ASWin.Repaint();
    }

    public static void CBReinitASMainWindow()
    {
      ASEditorBackend.ASWin.Reinit();
    }

    public static void CBDoDiscardChanges(int actionResult)
    {
      ASEditorBackend.ASWin.DoDiscardChanges(actionResult != 0);
    }

    public static void CBInitUpdatePage(int actionResult)
    {
      ASEditorBackend.ASWin.InitUpdatePage(actionResult != 0);
    }

    public static void CBInitHistoryPage(int actionResult)
    {
      ASEditorBackend.ASWin.InitHistoryPage(actionResult != 0);
    }

    public static void CBInitOverviewPage(int actionResult)
    {
      ASEditorBackend.ASWin.InitOverviewPage(actionResult != 0);
    }

    public static bool SettingsIfNeeded()
    {
      return ASEditorBackend.InitializeMaintBinding();
    }

    public static bool SettingsAreValid()
    {
      PListConfig plistConfig = new PListConfig("Library/ServerPreferences.plist");
      return (plistConfig["Maint UserName"].Length == 0 || plistConfig["Maint Server"].Length == 0 || (plistConfig["Maint database name"].Length == 0 || plistConfig["Maint Timeout"].Length == 0) ? 1 : (plistConfig["Maint port number"].Length == 0 ? 1 : 0)) == 0;
    }

    internal static string GetPassword(string server, string user)
    {
      return EditorPrefs.GetString("ASPassword::" + server + "::" + user, string.Empty);
    }

    internal static void SetPassword(string server, string user, string password)
    {
      EditorPrefs.SetString("ASPassword::" + server + "::" + user, password);
    }

    internal static void AddUser(string server, string user)
    {
      EditorPrefs.SetString("ASUser::" + server, user);
    }

    internal static string GetUser(string server)
    {
      return EditorPrefs.GetString("ASUser::" + server, string.Empty);
    }

    public static bool InitializeMaintBinding()
    {
      PListConfig plistConfig = new PListConfig("Library/ServerPreferences.plist");
      string str1 = plistConfig["Maint UserName"];
      string server = plistConfig["Maint Server"];
      string str2 = plistConfig["Maint project name"];
      string str3 = plistConfig["Maint database name"];
      string str4 = plistConfig["Maint port number"];
      int result;
      if (!int.TryParse(plistConfig["Maint Timeout"], out result))
        result = 5;
      if (server.Length == 0 || str2.Length == 0 || (str3.Length == 0 || str1.Length == 0))
      {
        AssetServer.SetProjectName(string.Empty);
        return false;
      }
      AssetServer.SetProjectName(string.Format("{0} @ {1}", (object) str2, (object) server));
      string connectionString = "host='" + server + "' user='" + str1 + "' password='" + ASEditorBackend.GetPassword(server, str1) + "' dbname='" + str3 + "' port='" + str4 + "' sslmode=disable " + plistConfig["Maint Connection Settings"];
      AssetServer.Initialize(str1, connectionString, result);
      return true;
    }

    public static void Testing_SetActionFinishedCallback(string klass, string name)
    {
      AssetServer.SaveString("s_TestingClass", klass);
      AssetServer.SaveString("s_TestingFunction", name);
      AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", "Testing_DummyCallback");
    }

    private static void Testing_DummyCallback(bool success)
    {
      ASEditorBackend.Testing_Invoke(AssetServer.GetAndRemoveString("s_TestingClass"), AssetServer.GetAndRemoveString("s_TestingFunction"), (object) success);
    }

    private static void Testing_SetExceptionHandler(string exceptionHandlerClass, string exceptionHandlerFunction)
    {
      AssetServer.SaveString("s_ExceptionHandlerClass", exceptionHandlerClass);
      AssetServer.SaveString("s_ExceptionHandlerFunction", exceptionHandlerFunction);
    }

    private static void Testing_Invoke(string klass, string method, params object[] prms)
    {
      try
      {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly.GetName().Name != "UnityEditor" && assembly.GetName().Name != "UnityEngine")
          {
            foreach (System.Type type in AssemblyHelper.GetTypesFromAssembly(assembly))
            {
              if (type.Name == klass)
                type.InvokeMember(method, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, (object) null, prms);
            }
          }
        }
      }
      catch (Exception ex)
      {
        if (ex.InnerException != null && ex.InnerException.GetType() == typeof (ExitGUIException))
          throw ex;
        ASEditorBackend.Testing_Invoke(AssetServer.GetString("s_ExceptionHandlerClass"), AssetServer.GetString("s_ExceptionHandlerFunction"), (object) ex);
      }
    }

    public static void Testing_SetActiveDatabase(string host, int port, string projectName, string dbName, string user, string pwd)
    {
      PListConfig plistConfig = new PListConfig("Library/ServerPreferences.plist");
      plistConfig["Maint Server"] = host;
      plistConfig["Maint UserName"] = user;
      plistConfig["Maint database name"] = dbName;
      plistConfig["Maint port number"] = port.ToString();
      plistConfig["Maint project name"] = projectName;
      plistConfig["Maint Password"] = string.Empty;
      plistConfig["Maint settings type"] = "manual";
      plistConfig["Maint Timeout"] = "5";
      plistConfig["Maint Connection Settings"] = string.Empty;
      plistConfig.Save();
    }

    public static bool Testing_SetupDatabase(string host, int port, string adminUser, string adminPwd, string user, string pwd, string projectName)
    {
      AssetServer.AdminSetCredentials(host, port, adminUser, adminPwd);
      MaintDatabaseRecord[] maintDatabaseRecordArray = AssetServer.AdminRefreshDatabases();
      if (maintDatabaseRecordArray == null)
        return false;
      foreach (MaintDatabaseRecord maintDatabaseRecord in maintDatabaseRecordArray)
      {
        if (maintDatabaseRecord.name == projectName)
          AssetServer.AdminDeleteDB(projectName);
      }
      if (AssetServer.AdminCreateDB(projectName) == 0)
        return false;
      string databaseName = AssetServer.GetDatabaseName(host, adminUser, adminPwd, port.ToString(), projectName);
      if (!AssetServer.AdminSetUserEnabled(databaseName, user, user, string.Empty, 1))
        return false;
      ASEditorBackend.Testing_SetActiveDatabase(host, port, projectName, databaseName, user, pwd);
      return true;
    }

    public static string[] Testing_GetAllDatabaseNames()
    {
      MaintDatabaseRecord[] maintDatabaseRecordArray = AssetServer.AdminRefreshDatabases();
      string[] strArray = new string[maintDatabaseRecordArray.Length];
      for (int index = 0; index < maintDatabaseRecordArray.Length; ++index)
        strArray[index] = maintDatabaseRecordArray[index].name;
      return strArray;
    }

    public static void Testing_SetConflictResolutionFunction(string klass, string fn)
    {
      ASEditorBackend.s_TestingConflictResClass = klass;
      ASEditorBackend.s_TestingConflictResFunction = fn;
    }

    public static void Testing_DummyConflictResolutionFunction(string[] conflicting)
    {
      ASEditorBackend.Testing_Invoke(ASEditorBackend.s_TestingConflictResClass, ASEditorBackend.s_TestingConflictResFunction, (object) conflicting);
    }
  }
}
