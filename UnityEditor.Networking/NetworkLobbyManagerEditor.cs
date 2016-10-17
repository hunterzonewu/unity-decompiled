// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkLobbyManagerEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (NetworkLobbyManager), true)]
  internal class NetworkLobbyManagerEditor : NetworkManagerEditor
  {
    private SerializedProperty m_ShowLobbyGUIProperty;
    private SerializedProperty m_MaxPlayersProperty;
    private SerializedProperty m_MaxPlayersPerConnectionProperty;
    private SerializedProperty m_MinPlayersProperty;
    private SerializedProperty m_LobbyPlayerPrefabProperty;
    private SerializedProperty m_GamePlayerPrefabProperty;
    private GUIContent m_LobbySceneLabel;
    private GUIContent m_PlaySceneLabel;
    private GUIContent m_MaxPlayersLabel;
    private GUIContent m_MaxPlayersPerConnectionLabel;
    private GUIContent m_MinPlayersLabel;
    private bool ShowSlots;

    private void InitLobby()
    {
      if (!this.m_Initialized)
      {
        this.m_LobbySceneLabel = new GUIContent("Lobby Scene", "The scene loaded for the lobby");
        this.m_PlaySceneLabel = new GUIContent("Play Scene", "The scene loaded to play the game");
        this.m_MaxPlayersLabel = new GUIContent("Max Players", "The maximum number of players allowed in the lobby.");
        this.m_MaxPlayersPerConnectionLabel = new GUIContent("Max Players Per Connection", "The maximum number of players that each connection/client can have in the lobby. Defaults to 1.");
        this.m_MinPlayersLabel = new GUIContent("Minimum Players", "The minimum number of players required to be ready for the game to start. If this is zero then the game can start with any number of players.");
        this.m_ShowLobbyGUIProperty = this.serializedObject.FindProperty("m_ShowLobbyGUI");
        this.m_MaxPlayersProperty = this.serializedObject.FindProperty("m_MaxPlayers");
        this.m_MaxPlayersPerConnectionProperty = this.serializedObject.FindProperty("m_MaxPlayersPerConnection");
        this.m_MinPlayersProperty = this.serializedObject.FindProperty("m_MinPlayers");
        this.m_LobbyPlayerPrefabProperty = this.serializedObject.FindProperty("m_LobbyPlayerPrefab");
        this.m_GamePlayerPrefabProperty = this.serializedObject.FindProperty("m_GamePlayerPrefab");
        NetworkLobbyManager target = this.target as NetworkLobbyManager;
        if ((Object) target == (Object) null)
          return;
        if (target.lobbyScene != string.Empty && (Object) this.GetSceneObject(target.lobbyScene) == (Object) null)
        {
          Debug.LogWarning((object) ("LobbyScene '" + target.lobbyScene + "' not found. You must repopulate the LobbyScene slot of the NetworkLobbyManager"));
          target.lobbyScene = string.Empty;
        }
        if (target.playScene != string.Empty && (Object) this.GetSceneObject(target.playScene) == (Object) null)
        {
          Debug.LogWarning((object) ("PlayScene '" + target.playScene + "' not found. You must repopulate the PlayScene slot of the NetworkLobbyManager"));
          target.playScene = string.Empty;
        }
      }
      this.Init();
    }

    public override void OnInspectorGUI()
    {
      if (this.m_DontDestroyOnLoadProperty == null || this.m_DontDestroyOnLoadLabel == null)
        this.m_Initialized = false;
      this.InitLobby();
      NetworkLobbyManager target = this.target as NetworkLobbyManager;
      if ((Object) target == (Object) null)
        return;
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_DontDestroyOnLoadProperty, this.m_DontDestroyOnLoadLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_RunInBackgroundProperty, this.m_RunInBackgroundLabel, new GUILayoutOption[0]);
      if (EditorGUILayout.PropertyField(this.m_LogLevelProperty))
        LogFilter.currentLogLevel = (int) this.m_NetworkManager.logLevel;
      this.ShowLobbyScenes();
      EditorGUILayout.PropertyField(this.m_ShowLobbyGUIProperty);
      EditorGUILayout.PropertyField(this.m_MaxPlayersProperty, this.m_MaxPlayersLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_MaxPlayersPerConnectionProperty, this.m_MaxPlayersPerConnectionLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_MinPlayersProperty, this.m_MinPlayersLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_LobbyPlayerPrefabProperty);
      EditorGUI.BeginChangeCheck();
      Object @object = EditorGUILayout.ObjectField("Game Player Prefab", (Object) target.gamePlayerPrefab, typeof (NetworkIdentity), false, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        if (@object == (Object) null)
        {
          this.m_GamePlayerPrefabProperty.objectReferenceValue = (Object) null;
        }
        else
        {
          NetworkIdentity networkIdentity = @object as NetworkIdentity;
          if ((Object) networkIdentity != (Object) null && (Object) networkIdentity.gameObject != (Object) target.gamePlayerPrefab)
            this.m_GamePlayerPrefabProperty.objectReferenceValue = (Object) networkIdentity.gameObject;
        }
      }
      EditorGUILayout.Separator();
      this.ShowNetworkInfo();
      this.ShowSpawnInfo();
      this.ShowConfigInfo();
      this.ShowSimulatorInfo();
      this.serializedObject.ApplyModifiedProperties();
      this.ShowDerivedProperties(typeof (NetworkLobbyManager), typeof (NetworkManager));
      if (!Application.isPlaying)
        return;
      EditorGUILayout.Separator();
      this.ShowLobbySlots();
    }

    protected void ShowLobbySlots()
    {
      NetworkLobbyManager target = this.target as NetworkLobbyManager;
      if ((Object) target == (Object) null)
        return;
      this.ShowSlots = EditorGUILayout.Foldout(this.ShowSlots, "LobbySlots");
      if (!this.ShowSlots)
        return;
      ++EditorGUI.indentLevel;
      foreach (NetworkLobbyPlayer lobbySlot in target.lobbySlots)
      {
        if (!((Object) lobbySlot == (Object) null))
          EditorGUILayout.ObjectField("Slot " + (object) lobbySlot.slot, (Object) lobbySlot.gameObject, typeof (Object), true, new GUILayoutOption[0]);
      }
      --EditorGUI.indentLevel;
    }

    private void SetLobbyScene(NetworkLobbyManager lobby, string sceneName)
    {
      this.serializedObject.FindProperty("m_LobbyScene").stringValue = sceneName;
      this.serializedObject.FindProperty("m_OfflineScene").stringValue = sceneName;
      EditorUtility.SetDirty((Object) lobby);
    }

    private void SetPlayScene(NetworkLobbyManager lobby, string sceneName)
    {
      this.serializedObject.FindProperty("m_PlayScene").stringValue = sceneName;
      this.serializedObject.FindProperty("m_OnlineScene").stringValue = string.Empty;
      EditorUtility.SetDirty((Object) lobby);
    }

    protected void ShowLobbyScenes()
    {
      NetworkLobbyManager target = this.target as NetworkLobbyManager;
      if ((Object) target == (Object) null)
        return;
      SceneAsset sceneObject1 = this.GetSceneObject(target.lobbyScene);
      EditorGUI.BeginChangeCheck();
      Object object1 = EditorGUILayout.ObjectField(this.m_LobbySceneLabel, (Object) sceneObject1, typeof (SceneAsset), false, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        if (object1 == (Object) null)
          this.SetLobbyScene(target, string.Empty);
        else if (object1.name != target.offlineScene)
        {
          if ((Object) this.GetSceneObject(object1.name) == (Object) null)
            Debug.LogWarning((object) ("The scene " + object1.name + " cannot be used. To use this scene add it to the build settings for the project"));
          else
            this.SetLobbyScene(target, object1.name);
        }
      }
      SceneAsset sceneObject2 = this.GetSceneObject(target.playScene);
      EditorGUI.BeginChangeCheck();
      Object object2 = EditorGUILayout.ObjectField(this.m_PlaySceneLabel, (Object) sceneObject2, typeof (SceneAsset), false, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (object2 == (Object) null)
      {
        this.SetPlayScene(target, string.Empty);
      }
      else
      {
        if (!(object2.name != this.m_NetworkManager.onlineScene))
          return;
        if ((Object) this.GetSceneObject(object2.name) == (Object) null)
          Debug.LogWarning((object) ("The scene " + object2.name + " cannot be used. To use this scene add it to the build settings for the project"));
        else
          this.SetPlayScene(target, object2.name);
      }
    }
  }
}
