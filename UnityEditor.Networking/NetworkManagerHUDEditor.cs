// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkManagerHUDEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (NetworkManagerHUD), true)]
  public class NetworkManagerHUDEditor : Editor
  {
    private SerializedProperty m_ShowGUIProperty;
    private SerializedProperty m_OffsetXProperty;
    private SerializedProperty m_OffsetYProperty;
    protected GUIContent m_ShowNetworkLabel;
    protected GUIContent m_ShowServerLabel;
    protected GUIContent m_ShowServerConnectionsLabel;
    protected GUIContent m_ShowServerObjectsLabel;
    protected GUIContent m_ShowClientLabel;
    protected GUIContent m_ShowClientObjectsLabel;
    protected GUIContent m_ShowMatchMakerLabel;
    protected GUIContent m_ShowControlsLabel;
    protected GUIContent m_ShowRuntimeGuiLabel;
    protected GUIContent m_OffsetXLabel;
    protected GUIContent m_OffsetYLabel;
    private bool m_ShowServer;
    private bool m_ShowServerConnections;
    private bool m_ShowServerObjects;
    private bool m_ShowClient;
    private bool m_ShowClientObjects;
    private bool m_ShowMatchMaker;
    private bool m_ShowControls;
    private bool m_Initialized;
    private NetworkManagerHUD m_ManagerHud;
    private NetworkManager m_Manager;
    private List<bool> m_ShowDetailForConnections;
    private List<bool> m_ShowPlayersForConnections;
    private List<bool> m_ShowVisibleForConnections;
    private List<bool> m_ShowOwnedForConnections;

    private void Init()
    {
      if (this.m_Initialized && this.m_ShowGUIProperty != null)
        return;
      this.m_Initialized = true;
      this.m_ManagerHud = this.target as NetworkManagerHUD;
      if ((Object) this.m_ManagerHud != (Object) null)
        this.m_Manager = this.m_ManagerHud.manager;
      this.m_ShowGUIProperty = this.serializedObject.FindProperty("showGUI");
      this.m_OffsetXProperty = this.serializedObject.FindProperty("offsetX");
      this.m_OffsetYProperty = this.serializedObject.FindProperty("offsetY");
      this.m_ShowServerLabel = new GUIContent("Server Info", "Details of internal server state");
      this.m_ShowServerConnectionsLabel = new GUIContent("Server Connections", "List of local and remote network connections to the server");
      this.m_ShowServerObjectsLabel = new GUIContent("Server Objects", "Networked objects spawned by the server");
      this.m_ShowClientLabel = new GUIContent("Client Info", "Details of internal client state");
      this.m_ShowClientObjectsLabel = new GUIContent("Client Objects", "Networked objects created on the client");
      this.m_ShowMatchMakerLabel = new GUIContent("MatchMaker Info", "Details about the matchmaker state");
      this.m_ShowControlsLabel = new GUIContent("Runtime Controls", "Buttons for controlling network state at runtime");
      this.m_ShowRuntimeGuiLabel = new GUIContent("Show Runtime GUI", "Show the default network control GUI when the game is running");
      this.m_OffsetXLabel = new GUIContent("GUI Horizontal Offset", "Horizontal offset of runtime GUI");
      this.m_OffsetYLabel = new GUIContent("GUI Vertical Offset", "Vertical offset of runtime GUI");
    }

    private void ShowServerConnections()
    {
      this.m_ShowServerConnections = EditorGUILayout.Foldout(this.m_ShowServerConnections, this.m_ShowServerConnectionsLabel);
      if (!this.m_ShowServerConnections)
        return;
      ++EditorGUI.indentLevel;
      if (this.m_ShowDetailForConnections == null)
      {
        this.m_ShowDetailForConnections = new List<bool>();
        this.m_ShowPlayersForConnections = new List<bool>();
        this.m_ShowVisibleForConnections = new List<bool>();
        this.m_ShowOwnedForConnections = new List<bool>();
      }
      while (this.m_ShowDetailForConnections.Count < NetworkServer.connections.Count)
      {
        this.m_ShowDetailForConnections.Add(false);
        this.m_ShowPlayersForConnections.Add(false);
        this.m_ShowVisibleForConnections.Add(false);
        this.m_ShowOwnedForConnections.Add(false);
      }
      int index = 0;
      foreach (NetworkConnection connection in NetworkServer.connections)
      {
        if (connection == null)
        {
          ++index;
        }
        else
        {
          this.m_ShowDetailForConnections[index] = (EditorGUILayout.Foldout((this.m_ShowDetailForConnections[index] ? 1 : 0) != 0, "Conn: " + (object) connection.connectionId + " (" + connection.address + ")") ? 1 : 0) != 0;
          if (this.m_ShowDetailForConnections[index])
          {
            ++EditorGUI.indentLevel;
            this.m_ShowPlayersForConnections[index] = EditorGUILayout.Foldout(this.m_ShowPlayersForConnections[index], "Players");
            if (this.m_ShowPlayersForConnections[index])
            {
              ++EditorGUI.indentLevel;
              using (List<PlayerController>.Enumerator enumerator = connection.playerControllers.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  PlayerController current = enumerator.Current;
                  EditorGUILayout.ObjectField("Player: " + (object) current.playerControllerId, (Object) current.gameObject, typeof (GameObject), true, new GUILayoutOption[0]);
                }
              }
              --EditorGUI.indentLevel;
            }
            this.m_ShowVisibleForConnections[index] = EditorGUILayout.Foldout(this.m_ShowVisibleForConnections[index], "Visible Objects");
            if (this.m_ShowVisibleForConnections[index])
            {
              ++EditorGUI.indentLevel;
              using (HashSet<NetworkIdentity>.Enumerator enumerator = connection.visList.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  NetworkIdentity current = enumerator.Current;
                  EditorGUILayout.ObjectField("NetId: " + (object) current.netId, (Object) current, typeof (NetworkIdentity), true, new GUILayoutOption[0]);
                }
              }
              --EditorGUI.indentLevel;
            }
            if (connection.clientOwnedObjects != null)
            {
              this.m_ShowOwnedForConnections[index] = EditorGUILayout.Foldout(this.m_ShowOwnedForConnections[index], "Owned Objects");
              if (this.m_ShowOwnedForConnections[index])
              {
                ++EditorGUI.indentLevel;
                using (HashSet<NetworkInstanceId>.Enumerator enumerator = connection.clientOwnedObjects.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    NetworkInstanceId current = enumerator.Current;
                    GameObject localObject = NetworkServer.FindLocalObject(current);
                    EditorGUILayout.ObjectField("Owned: " + (object) current, (Object) localObject, typeof (NetworkIdentity), true, new GUILayoutOption[0]);
                  }
                }
                --EditorGUI.indentLevel;
              }
            }
            --EditorGUI.indentLevel;
          }
          ++index;
        }
      }
      --EditorGUI.indentLevel;
    }

    private void ShowServerObjects()
    {
      this.m_ShowServerObjects = EditorGUILayout.Foldout(this.m_ShowServerObjects, this.m_ShowServerObjectsLabel);
      if (!this.m_ShowServerObjects)
        return;
      ++EditorGUI.indentLevel;
      using (Dictionary<NetworkInstanceId, NetworkIdentity>.Enumerator enumerator = NetworkServer.objects.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<NetworkInstanceId, NetworkIdentity> current = enumerator.Current;
          string label = "NetId:" + (object) current.Key;
          GameObject gameObject = (GameObject) null;
          if ((Object) current.Value != (Object) null)
          {
            NetworkIdentity component = current.Value.GetComponent<NetworkIdentity>();
            label = label + " SceneId:" + (object) component.sceneId;
            gameObject = current.Value.gameObject;
          }
          EditorGUILayout.ObjectField(label, (Object) gameObject, typeof (GameObject), true, new GUILayoutOption[0]);
        }
      }
      --EditorGUI.indentLevel;
    }

    private void ShowServerInfo()
    {
      if (!NetworkServer.active)
        return;
      this.m_ShowServer = EditorGUILayout.Foldout(this.m_ShowServer, this.m_ShowServerLabel);
      if (!this.m_ShowServer)
        return;
      ++EditorGUI.indentLevel;
      EditorGUILayout.BeginVertical();
      this.ShowServerConnections();
      this.ShowServerObjects();
      EditorGUILayout.EndVertical();
      --EditorGUI.indentLevel;
    }

    private void ShowClientObjects()
    {
      this.m_ShowClientObjects = EditorGUILayout.Foldout(this.m_ShowClientObjects, this.m_ShowClientObjectsLabel);
      if (!this.m_ShowClientObjects)
        return;
      ++EditorGUI.indentLevel;
      using (Dictionary<NetworkInstanceId, NetworkIdentity>.Enumerator enumerator = ClientScene.objects.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<NetworkInstanceId, NetworkIdentity> current = enumerator.Current;
          string label = "NetId:" + (object) current.Key;
          GameObject gameObject = (GameObject) null;
          if ((Object) current.Value != (Object) null)
          {
            NetworkIdentity component = current.Value.GetComponent<NetworkIdentity>();
            label = label + " SceneId:" + (object) component.sceneId;
            gameObject = current.Value.gameObject;
          }
          EditorGUILayout.ObjectField(label, (Object) gameObject, typeof (GameObject), true, new GUILayoutOption[0]);
        }
      }
      --EditorGUI.indentLevel;
    }

    private void ShowClientInfo()
    {
      if (!NetworkClient.active)
        return;
      this.m_ShowClient = EditorGUILayout.Foldout(this.m_ShowClient, this.m_ShowClientLabel);
      if (!this.m_ShowClient)
        return;
      ++EditorGUI.indentLevel;
      EditorGUILayout.BeginVertical();
      int num = 0;
      using (List<NetworkClient>.Enumerator enumerator1 = NetworkClient.allClients.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          NetworkClient current = enumerator1.Current;
          if (current.connection == null)
          {
            EditorGUILayout.TextField("client " + (object) num + ": ", current.GetType().Name + " Conn: null", new GUILayoutOption[0]);
          }
          else
          {
            EditorGUILayout.TextField("client " + (object) num + ":", current.GetType().Name + " Conn: " + (object) current.connection, new GUILayoutOption[0]);
            ++EditorGUI.indentLevel;
            using (List<PlayerController>.Enumerator enumerator2 = current.connection.playerControllers.GetEnumerator())
            {
              while (enumerator2.MoveNext())
                EditorGUILayout.LabelField("Player", enumerator2.Current.ToString(), new GUILayoutOption[0]);
            }
            --EditorGUI.indentLevel;
          }
          ++num;
        }
      }
      this.ShowClientObjects();
      EditorGUILayout.EndVertical();
      --EditorGUI.indentLevel;
    }

    private void ShowMatchMakerInfo()
    {
      if ((Object) this.m_Manager.matchMaker == (Object) null)
        return;
      this.m_ShowMatchMaker = EditorGUILayout.Foldout(this.m_ShowMatchMaker, this.m_ShowMatchMakerLabel);
      if (!this.m_ShowMatchMaker)
        return;
      ++EditorGUI.indentLevel;
      EditorGUILayout.BeginVertical();
      EditorGUILayout.LabelField("MatchMaker Host:", this.m_Manager.matchHost, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("MatchMaker Port:", this.m_Manager.matchPort.ToString(), new GUILayoutOption[0]);
      this.m_Manager.matchName = EditorGUILayout.TextField("Room Name:", this.m_Manager.matchName, new GUILayoutOption[0]);
      this.m_Manager.matchSize = (uint) EditorGUILayout.IntField("Room Max Size:", (int) this.m_Manager.matchSize, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Uri:", this.m_Manager.matchMaker.baseUri.ToString(), new GUILayoutOption[0]);
      if (this.m_Manager.matchInfo != null)
        EditorGUILayout.LabelField("match", this.m_Manager.matchInfo.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.EndVertical();
      --EditorGUI.indentLevel;
    }

    private static Object GetSceneObject(string sceneObjectName)
    {
      if (string.IsNullOrEmpty(sceneObjectName))
        return (Object) null;
      foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
      {
        if (scene.path.IndexOf(sceneObjectName) != -1)
          return AssetDatabase.LoadAssetAtPath(scene.path, typeof (Object));
      }
      return (Object) null;
    }

    private static Rect GetButtonRect()
    {
      Rect controlRect = EditorGUILayout.GetControlRect();
      float num = controlRect.width / 6f;
      return new Rect(controlRect.xMin + num, controlRect.yMin, controlRect.width - num * 2f, controlRect.height);
    }

    private void ShowControls()
    {
      this.m_ShowControls = EditorGUILayout.Foldout(this.m_ShowControls, this.m_ShowControlsLabel);
      if (!this.m_ShowControls)
        return;
      if (!string.IsNullOrEmpty(NetworkManager.networkSceneName))
        EditorGUILayout.ObjectField("Current Scene:", NetworkManagerHUDEditor.GetSceneObject(NetworkManager.networkSceneName), typeof (Object), true, new GUILayoutOption[0]);
      EditorGUILayout.Separator();
      if (!NetworkClient.active && !NetworkServer.active && (Object) this.m_Manager.matchMaker == (Object) null)
      {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Toggle(false, "LAN Host", EditorStyles.miniButton, new GUILayoutOption[0]))
          this.m_Manager.StartHost();
        if (GUILayout.Toggle(false, "LAN Server", EditorStyles.miniButton, new GUILayoutOption[0]))
          this.m_Manager.StartServer();
        if (GUILayout.Toggle(false, "LAN Client", EditorStyles.miniButton, new GUILayoutOption[0]))
          this.m_Manager.StartClient();
        if (GUILayout.Toggle(false, "Start Matchmaker", EditorStyles.miniButton, new GUILayoutOption[0]))
        {
          this.m_Manager.StartMatchMaker();
          this.m_ShowMatchMaker = true;
        }
        EditorGUILayout.EndHorizontal();
      }
      if (NetworkClient.active && !ClientScene.ready && GUI.Button(NetworkManagerHUDEditor.GetButtonRect(), "Client Ready"))
      {
        ClientScene.Ready(this.m_Manager.client.connection);
        if (ClientScene.localPlayers.Count == 0)
          ClientScene.AddPlayer((short) 0);
      }
      if ((NetworkServer.active || NetworkClient.active) && GUI.Button(NetworkManagerHUDEditor.GetButtonRect(), "Stop"))
      {
        this.m_Manager.StopServer();
        this.m_Manager.StopClient();
      }
      if (!NetworkServer.active && !NetworkClient.active)
      {
        EditorGUILayout.Separator();
        if ((Object) this.m_Manager.matchMaker != (Object) null && this.m_Manager.matchInfo == null)
        {
          if (this.m_Manager.matches == null)
          {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(false, "Create Internet Match", EditorStyles.miniButton, new GUILayoutOption[0]))
              this.m_Manager.matchMaker.CreateMatch(this.m_Manager.matchName, this.m_Manager.matchSize, true, string.Empty, new NetworkMatch.ResponseDelegate<CreateMatchResponse>(this.m_Manager.OnMatchCreate));
            if (GUILayout.Toggle(false, "Find Internet Match", EditorStyles.miniButton, new GUILayoutOption[0]))
              this.m_Manager.matchMaker.ListMatches(0, 20, string.Empty, new NetworkMatch.ResponseDelegate<ListMatchResponse>(this.m_Manager.OnMatchList));
            if (GUILayout.Toggle(false, "Stop MatchMaker", EditorStyles.miniButton, new GUILayoutOption[0]))
              this.m_Manager.StopMatchMaker();
            EditorGUILayout.EndHorizontal();
            this.m_Manager.matchName = EditorGUILayout.TextField("Room Name:", this.m_Manager.matchName, new GUILayoutOption[0]);
            this.m_Manager.matchSize = (uint) EditorGUILayout.IntField("Room Size:", (int) this.m_Manager.matchSize, new GUILayoutOption[0]);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(false, "Use Local Relay", EditorStyles.miniButton, new GUILayoutOption[0]))
              this.m_Manager.SetMatchHost("localhost", 1337, false);
            if (GUILayout.Toggle(false, "Use Internet Relay", EditorStyles.miniButton, new GUILayoutOption[0]))
              this.m_Manager.SetMatchHost("mm.unet.unity3d.com", 80, false);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
          }
          else
          {
            using (List<MatchDesc>.Enumerator enumerator = this.m_Manager.matches.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                MatchDesc current = enumerator.Current;
                if (GUI.Button(NetworkManagerHUDEditor.GetButtonRect(), "Join Match:" + current.name))
                {
                  this.m_Manager.matchName = current.name;
                  this.m_Manager.matchSize = (uint) current.currentSize;
                  this.m_Manager.matchMaker.JoinMatch(current.networkId, string.Empty, new NetworkMatch.ResponseDelegate<JoinMatchResponse>(this.m_Manager.OnMatchJoined));
                }
              }
            }
            if (GUI.Button(NetworkManagerHUDEditor.GetButtonRect(), "Stop MatchMaker"))
              this.m_Manager.StopMatchMaker();
          }
        }
      }
      EditorGUILayout.Separator();
    }

    public override void OnInspectorGUI()
    {
      this.Init();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_ShowGUIProperty, this.m_ShowRuntimeGuiLabel, new GUILayoutOption[0]);
      if (this.m_ManagerHud.showGUI)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_OffsetXProperty, this.m_OffsetXLabel, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_OffsetYProperty, this.m_OffsetYLabel, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
      }
      this.serializedObject.ApplyModifiedProperties();
      if (!Application.isPlaying)
        return;
      this.ShowControls();
      this.ShowServerInfo();
      this.ShowClientInfo();
      this.ShowMatchMakerInfo();
    }
  }
}
