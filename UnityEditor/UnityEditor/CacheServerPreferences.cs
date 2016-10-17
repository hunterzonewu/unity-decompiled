// Decompiled with JetBrains decompiler
// Type: UnityEditor.CacheServerPreferences
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class CacheServerPreferences
  {
    private static bool s_PrefsLoaded;
    private static CacheServerPreferences.ConnectionState s_ConnectionState;
    private static bool s_CacheServerEnabled;
    private static string s_CacheServerIPAddress;

    public static void ReadPreferences()
    {
      CacheServerPreferences.s_CacheServerIPAddress = EditorPrefs.GetString("CacheServerIPAddress", CacheServerPreferences.s_CacheServerIPAddress);
      CacheServerPreferences.s_CacheServerEnabled = EditorPrefs.GetBool("CacheServerEnabled");
    }

    public static void WritePreferences()
    {
      EditorPrefs.SetString("CacheServerIPAddress", CacheServerPreferences.s_CacheServerIPAddress);
      EditorPrefs.SetBool("CacheServerEnabled", CacheServerPreferences.s_CacheServerEnabled);
    }

    [PreferenceItem("Cache Server")]
    public static void OnGUI()
    {
      GUILayout.Space(10f);
      if (!InternalEditorUtility.HasTeamLicense())
        GUILayout.Label(EditorGUIUtility.TempContent("You need to have a Pro or Team license to use the cache server.", (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning)), EditorStyles.helpBox, new GUILayoutOption[0]);
      EditorGUI.BeginDisabledGroup(!InternalEditorUtility.HasTeamLicense());
      if (!CacheServerPreferences.s_PrefsLoaded)
      {
        CacheServerPreferences.ReadPreferences();
        CacheServerPreferences.s_PrefsLoaded = true;
      }
      if (CacheServerPreferences.s_CacheServerEnabled && CacheServerPreferences.s_ConnectionState == CacheServerPreferences.ConnectionState.Unknown)
        CacheServerPreferences.s_ConnectionState = !InternalEditorUtility.CanConnectToCacheServer() ? CacheServerPreferences.ConnectionState.Failure : CacheServerPreferences.ConnectionState.Success;
      EditorGUI.BeginChangeCheck();
      CacheServerPreferences.s_CacheServerEnabled = EditorGUILayout.Toggle("Use Cache Server", CacheServerPreferences.s_CacheServerEnabled, new GUILayoutOption[0]);
      EditorGUI.BeginDisabledGroup(!CacheServerPreferences.s_CacheServerEnabled);
      CacheServerPreferences.s_CacheServerIPAddress = EditorGUILayout.DelayedTextField("IP Address", CacheServerPreferences.s_CacheServerIPAddress, new GUILayoutOption[0]);
      if (GUI.changed)
        CacheServerPreferences.s_ConnectionState = CacheServerPreferences.ConnectionState.Unknown;
      GUILayout.Space(5f);
      if (GUILayout.Button("Check Connection", new GUILayoutOption[1]{ GUILayout.Width(150f) }))
        CacheServerPreferences.s_ConnectionState = !InternalEditorUtility.CanConnectToCacheServer() ? CacheServerPreferences.ConnectionState.Failure : CacheServerPreferences.ConnectionState.Success;
      GUILayout.Space(-25f);
      switch (CacheServerPreferences.s_ConnectionState)
      {
        case CacheServerPreferences.ConnectionState.Unknown:
          GUILayout.Space(44f);
          break;
        case CacheServerPreferences.ConnectionState.Success:
          EditorGUILayout.HelpBox("Connection successful.", MessageType.Info, false);
          break;
        case CacheServerPreferences.ConnectionState.Failure:
          EditorGUILayout.HelpBox("Connection failed.", MessageType.Warning, false);
          break;
      }
      EditorGUI.EndDisabledGroup();
      if (EditorGUI.EndChangeCheck())
      {
        CacheServerPreferences.WritePreferences();
        CacheServerPreferences.ReadPreferences();
      }
      EditorGUI.EndDisabledGroup();
    }

    private enum ConnectionState
    {
      Unknown,
      Success,
      Failure,
    }
  }
}
