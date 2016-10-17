// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkManagerEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using System;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (NetworkManager), true)]
  public class NetworkManagerEditor : Editor
  {
    protected SerializedProperty m_DontDestroyOnLoadProperty;
    protected SerializedProperty m_RunInBackgroundProperty;
    protected SerializedProperty m_ScriptCRCCheckProperty;
    private SerializedProperty m_NetworkAddressProperty;
    private SerializedProperty m_NetworkPortProperty;
    private SerializedProperty m_ServerBindToIPProperty;
    private SerializedProperty m_ServerBindAddressProperty;
    private SerializedProperty m_MaxDelayProperty;
    protected SerializedProperty m_LogLevelProperty;
    private SerializedProperty m_MatchHostProperty;
    private SerializedProperty m_MatchPortProperty;
    private SerializedProperty m_PlayerPrefabProperty;
    private SerializedProperty m_AutoCreatePlayerProperty;
    private SerializedProperty m_PlayerSpawnMethodProperty;
    private SerializedProperty m_SpawnListProperty;
    private SerializedProperty m_CustomConfigProperty;
    private SerializedProperty m_UseWebSocketsProperty;
    private SerializedProperty m_UseSimulatorProperty;
    private SerializedProperty m_SimulatedLatencyProperty;
    private SerializedProperty m_PacketLossPercentageProperty;
    private SerializedProperty m_ChannelListProperty;
    private ReorderableList m_ChannelList;
    private GUIContent m_ShowNetworkLabel;
    private GUIContent m_ShowSpawnLabel;
    private GUIContent m_OfflineSceneLabel;
    private GUIContent m_OnlineSceneLabel;
    protected GUIContent m_DontDestroyOnLoadLabel;
    protected GUIContent m_RunInBackgroundLabel;
    protected GUIContent m_ScriptCRCCheckLabel;
    private GUIContent m_MaxConnectionsLabel;
    private GUIContent m_MinUpdateTimeoutLabel;
    private GUIContent m_ConnectTimeoutLabel;
    private GUIContent m_DisconnectTimeoutLabel;
    private GUIContent m_PingTimeoutLabel;
    private GUIContent m_ThreadAwakeTimeoutLabel;
    private GUIContent m_ReactorModelLabel;
    private GUIContent m_ReactorMaximumReceivedMessagesLabel;
    private GUIContent m_ReactorMaximumSentMessagesLabel;
    private GUIContent m_UseWebSocketsLabel;
    private GUIContent m_UseSimulatorLabel;
    private GUIContent m_LatencyLabel;
    private GUIContent m_PacketLossPercentageLabel;
    private ReorderableList m_SpawnList;
    protected bool m_Initialized;
    protected NetworkManager m_NetworkManager;

    protected void Init()
    {
      if (this.m_Initialized)
        return;
      this.m_Initialized = true;
      this.m_NetworkManager = this.target as NetworkManager;
      this.m_ShowNetworkLabel = new GUIContent("Network Info", "Network host names and ports");
      this.m_ShowSpawnLabel = new GUIContent("Spawn Info", "Registered spawnable objects");
      this.m_OfflineSceneLabel = new GUIContent("Offline Scene", "The scene loaded when the network goes offline (disconnected from server)");
      this.m_OnlineSceneLabel = new GUIContent("Online Scene", "The scene loaded when the network comes online (connected to server)");
      this.m_DontDestroyOnLoadLabel = new GUIContent("Dont Destroy On Load", "Persist the network manager across scene changes.");
      this.m_RunInBackgroundLabel = new GUIContent("Run in Background", "This ensures that the application runs when it does not have focus. This is required when testing multiple instances on a single machine, but not recommended for shipping on mobile platforms.");
      this.m_ScriptCRCCheckLabel = new GUIContent("Script CRC Check", "Enables a CRC check between server and client that ensures the NetworkBehaviour scripts match. This may not be appropriate in some cases, such a when the client and server are different Unity projects.");
      this.m_MaxConnectionsLabel = new GUIContent("Max Connections", "Maximum number of network connections");
      this.m_MinUpdateTimeoutLabel = new GUIContent("Min Update Timeout", "Minimum time network thread waits for events");
      this.m_ConnectTimeoutLabel = new GUIContent("Connect Timeout", "Time to wait for timeout on connecting");
      this.m_DisconnectTimeoutLabel = new GUIContent("Disconnect Timeout", "Time to wait for detecting disconnect");
      this.m_PingTimeoutLabel = new GUIContent("Ping Timeout", "Time to wait for ping messages");
      this.m_ThreadAwakeTimeoutLabel = new GUIContent("Thread Awake Timeout", "The minimum time period when system will check if there are any messages for send (or receive).");
      this.m_ReactorModelLabel = new GUIContent("Reactor Model", "Defines reactor model for the network library");
      this.m_ReactorMaximumReceivedMessagesLabel = new GUIContent("Reactor Max Recv Messages", "Defines maximum amount of messages in the receive queue");
      this.m_ReactorMaximumSentMessagesLabel = new GUIContent("Reactor Max Sent Messages", "Defines maximum message count in sent queue");
      this.m_UseWebSocketsLabel = new GUIContent("Use WebSockets", "This makes the server listen for connections using WebSockets. This allows WebGL clients to connect to the server.");
      this.m_UseSimulatorLabel = new GUIContent("Use Network Simulator", "This simulates network latency and packet loss on clients. Useful for testing under internet-like conditions");
      this.m_LatencyLabel = new GUIContent("Simulated Average Latency", "The amount of delay in milliseconds to add to network packets");
      this.m_PacketLossPercentageLabel = new GUIContent("Simulated Packet Loss", "The percentage of packets that should be dropped");
      this.m_DontDestroyOnLoadProperty = this.serializedObject.FindProperty("m_DontDestroyOnLoad");
      this.m_RunInBackgroundProperty = this.serializedObject.FindProperty("m_RunInBackground");
      this.m_ScriptCRCCheckProperty = this.serializedObject.FindProperty("m_ScriptCRCCheck");
      this.m_LogLevelProperty = this.serializedObject.FindProperty("m_LogLevel");
      this.m_NetworkAddressProperty = this.serializedObject.FindProperty("m_NetworkAddress");
      this.m_NetworkPortProperty = this.serializedObject.FindProperty("m_NetworkPort");
      this.m_ServerBindToIPProperty = this.serializedObject.FindProperty("m_ServerBindToIP");
      this.m_ServerBindAddressProperty = this.serializedObject.FindProperty("m_ServerBindAddress");
      this.m_MaxDelayProperty = this.serializedObject.FindProperty("m_MaxDelay");
      this.m_MatchHostProperty = this.serializedObject.FindProperty("m_MatchHost");
      this.m_MatchPortProperty = this.serializedObject.FindProperty("m_MatchPort");
      this.m_PlayerPrefabProperty = this.serializedObject.FindProperty("m_PlayerPrefab");
      this.m_AutoCreatePlayerProperty = this.serializedObject.FindProperty("m_AutoCreatePlayer");
      this.m_PlayerSpawnMethodProperty = this.serializedObject.FindProperty("m_PlayerSpawnMethod");
      this.m_SpawnListProperty = this.serializedObject.FindProperty("m_SpawnPrefabs");
      this.m_SpawnList = new ReorderableList(this.serializedObject, this.m_SpawnListProperty);
      this.m_SpawnList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(NetworkManagerEditor.DrawHeader);
      this.m_SpawnList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawChild);
      this.m_SpawnList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.Changed);
      this.m_SpawnList.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(this.AddButton);
      this.m_SpawnList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveButton);
      this.m_SpawnList.onChangedCallback = new ReorderableList.ChangedCallbackDelegate(this.Changed);
      this.m_SpawnList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.Changed);
      this.m_SpawnList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.Changed);
      this.m_SpawnList.elementHeight = 16f;
      this.m_CustomConfigProperty = this.serializedObject.FindProperty("m_CustomConfig");
      this.m_ChannelListProperty = this.serializedObject.FindProperty("m_Channels");
      this.m_ChannelList = new ReorderableList(this.serializedObject, this.m_ChannelListProperty);
      this.m_ChannelList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(NetworkManagerEditor.ChannelDrawHeader);
      this.m_ChannelList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.ChannelDrawChild);
      this.m_ChannelList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.ChannelChanged);
      this.m_ChannelList.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(this.ChannelAddButton);
      this.m_ChannelList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.ChannelRemoveButton);
      this.m_ChannelList.onChangedCallback = new ReorderableList.ChangedCallbackDelegate(this.ChannelChanged);
      this.m_ChannelList.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.ChannelChanged);
      this.m_ChannelList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.ChannelChanged);
      this.m_UseWebSocketsProperty = this.serializedObject.FindProperty("m_UseWebSockets");
      this.m_UseSimulatorProperty = this.serializedObject.FindProperty("m_UseSimulator");
      this.m_SimulatedLatencyProperty = this.serializedObject.FindProperty("m_SimulatedLatency");
      this.m_PacketLossPercentageProperty = this.serializedObject.FindProperty("m_PacketLossPercentage");
    }

    private static void ShowPropertySuffix(GUIContent content, SerializedProperty prop, string suffix)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.PropertyField(prop, content, new GUILayoutOption[0]);
      GUILayout.Label(suffix, EditorStyles.toolbarTextField, new GUILayoutOption[1]
      {
        GUILayout.Width(64f)
      });
      EditorGUILayout.EndHorizontal();
    }

    protected void ShowSimulatorInfo()
    {
      EditorGUILayout.PropertyField(this.m_UseSimulatorProperty, this.m_UseSimulatorLabel, new GUILayoutOption[0]);
      if (!this.m_UseSimulatorProperty.boolValue)
        return;
      ++EditorGUI.indentLevel;
      if (Application.isPlaying && this.m_NetworkManager.client != null)
      {
        EditorGUILayout.LabelField(this.m_LatencyLabel, new GUIContent(this.m_NetworkManager.simulatedLatency.ToString() + " milliseconds"), new GUILayoutOption[0]);
        EditorGUILayout.LabelField(this.m_PacketLossPercentageLabel, new GUIContent(((double) this.m_NetworkManager.packetLossPercentage).ToString() + "%"), new GUILayoutOption[0]);
      }
      else
      {
        int simulatedLatency = this.m_NetworkManager.simulatedLatency;
        EditorGUILayout.BeginHorizontal();
        int num1 = EditorGUILayout.IntSlider(this.m_LatencyLabel, simulatedLatency, 1, 400, new GUILayoutOption[0]);
        GUILayout.Label("millsec", EditorStyles.toolbarTextField, new GUILayoutOption[1]
        {
          GUILayout.Width(64f)
        });
        EditorGUILayout.EndHorizontal();
        if (num1 != simulatedLatency)
          this.m_SimulatedLatencyProperty.intValue = num1;
        float packetLossPercentage = this.m_NetworkManager.packetLossPercentage;
        EditorGUILayout.BeginHorizontal();
        float num2 = EditorGUILayout.Slider(this.m_PacketLossPercentageLabel, packetLossPercentage, 0.0f, 20f, new GUILayoutOption[0]);
        GUILayout.Label("%", EditorStyles.toolbarTextField, new GUILayoutOption[1]
        {
          GUILayout.Width(64f)
        });
        EditorGUILayout.EndHorizontal();
        if ((double) num2 != (double) packetLossPercentage)
          this.m_PacketLossPercentageProperty.floatValue = num2;
      }
      --EditorGUI.indentLevel;
    }

    protected void ShowConfigInfo()
    {
      bool customConfig = this.m_NetworkManager.customConfig;
      EditorGUILayout.PropertyField(this.m_CustomConfigProperty, new GUIContent("Advanced Configuration"), new GUILayoutOption[0]);
      if (this.m_CustomConfigProperty.boolValue && !customConfig && this.m_NetworkManager.channels.Count == 0)
      {
        this.m_NetworkManager.channels.Add(QosType.ReliableSequenced);
        this.m_NetworkManager.channels.Add(QosType.Unreliable);
        this.m_NetworkManager.customConfig = true;
        this.m_CustomConfigProperty.serializedObject.Update();
        this.m_ChannelList.serializedProperty.serializedObject.Update();
      }
      if (!this.m_NetworkManager.customConfig)
        return;
      ++EditorGUI.indentLevel;
      SerializedProperty property1 = this.serializedObject.FindProperty("m_MaxConnections");
      NetworkManagerEditor.ShowPropertySuffix(this.m_MaxConnectionsLabel, property1, "connections");
      this.m_ChannelList.DoLayoutList();
      property1.isExpanded = EditorGUILayout.Foldout(property1.isExpanded, "Timeouts");
      if (property1.isExpanded)
      {
        ++EditorGUI.indentLevel;
        SerializedProperty property2 = this.serializedObject.FindProperty("m_ConnectionConfig.m_MinUpdateTimeout");
        SerializedProperty property3 = this.serializedObject.FindProperty("m_ConnectionConfig.m_ConnectTimeout");
        SerializedProperty property4 = this.serializedObject.FindProperty("m_ConnectionConfig.m_DisconnectTimeout");
        SerializedProperty property5 = this.serializedObject.FindProperty("m_ConnectionConfig.m_PingTimeout");
        NetworkManagerEditor.ShowPropertySuffix(this.m_MinUpdateTimeoutLabel, property2, "millisec");
        NetworkManagerEditor.ShowPropertySuffix(this.m_ConnectTimeoutLabel, property3, "millisec");
        NetworkManagerEditor.ShowPropertySuffix(this.m_DisconnectTimeoutLabel, property4, "millisec");
        NetworkManagerEditor.ShowPropertySuffix(this.m_PingTimeoutLabel, property5, "millisec");
        --EditorGUI.indentLevel;
      }
      SerializedProperty property6 = this.serializedObject.FindProperty("m_GlobalConfig.m_ThreadAwakeTimeout");
      property6.isExpanded = EditorGUILayout.Foldout(property6.isExpanded, "Global Config");
      if (property6.isExpanded)
      {
        ++EditorGUI.indentLevel;
        SerializedProperty property2 = this.serializedObject.FindProperty("m_GlobalConfig.m_ReactorModel");
        SerializedProperty property3 = this.serializedObject.FindProperty("m_GlobalConfig.m_ReactorMaximumReceivedMessages");
        SerializedProperty property4 = this.serializedObject.FindProperty("m_GlobalConfig.m_ReactorMaximumSentMessages");
        NetworkManagerEditor.ShowPropertySuffix(this.m_ThreadAwakeTimeoutLabel, property6, "millisec");
        EditorGUILayout.PropertyField(property2, this.m_ReactorModelLabel, new GUILayoutOption[0]);
        NetworkManagerEditor.ShowPropertySuffix(this.m_ReactorMaximumReceivedMessagesLabel, property3, "messages");
        NetworkManagerEditor.ShowPropertySuffix(this.m_ReactorMaximumSentMessagesLabel, property4, "messages");
        --EditorGUI.indentLevel;
      }
      --EditorGUI.indentLevel;
    }

    protected void ShowSpawnInfo()
    {
      this.m_PlayerPrefabProperty.isExpanded = EditorGUILayout.Foldout(this.m_PlayerPrefabProperty.isExpanded, this.m_ShowSpawnLabel);
      if (!this.m_PlayerPrefabProperty.isExpanded)
        return;
      ++EditorGUI.indentLevel;
      if (this.m_NetworkManager.GetType() != typeof (NetworkLobbyManager))
        EditorGUILayout.PropertyField(this.m_PlayerPrefabProperty);
      EditorGUILayout.PropertyField(this.m_AutoCreatePlayerProperty);
      EditorGUILayout.PropertyField(this.m_PlayerSpawnMethodProperty);
      EditorGUI.BeginChangeCheck();
      this.m_SpawnList.DoLayoutList();
      if (EditorGUI.EndChangeCheck())
        this.serializedObject.ApplyModifiedProperties();
      --EditorGUI.indentLevel;
    }

    protected SceneAsset GetSceneObject(string sceneObjectName)
    {
      if (string.IsNullOrEmpty(sceneObjectName))
        return (SceneAsset) null;
      foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
      {
        if (scene.path.IndexOf(sceneObjectName) != -1)
          return AssetDatabase.LoadAssetAtPath(scene.path, typeof (SceneAsset)) as SceneAsset;
      }
      if (LogFilter.logWarn)
        Debug.LogWarning((object) ("Scene [" + sceneObjectName + "] cannot be used with networking. Add this scene to the 'Scenes in the Build' in build settings."));
      return (SceneAsset) null;
    }

    protected void ShowNetworkInfo()
    {
      this.m_NetworkAddressProperty.isExpanded = EditorGUILayout.Foldout(this.m_NetworkAddressProperty.isExpanded, this.m_ShowNetworkLabel);
      if (!this.m_NetworkAddressProperty.isExpanded)
        return;
      ++EditorGUI.indentLevel;
      if (EditorGUILayout.PropertyField(this.m_UseWebSocketsProperty, this.m_UseWebSocketsLabel, new GUILayoutOption[0]))
        NetworkServer.useWebSockets = this.m_NetworkManager.useWebSockets;
      EditorGUILayout.PropertyField(this.m_NetworkAddressProperty);
      EditorGUILayout.PropertyField(this.m_NetworkPortProperty);
      EditorGUILayout.PropertyField(this.m_ServerBindToIPProperty);
      if (this.m_NetworkManager.serverBindToIP)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_ServerBindAddressProperty);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.PropertyField(this.m_ScriptCRCCheckProperty, this.m_ScriptCRCCheckLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_MaxDelayProperty);
      EditorGUILayout.PropertyField(this.m_MatchHostProperty);
      EditorGUILayout.PropertyField(this.m_MatchPortProperty);
      --EditorGUI.indentLevel;
      Utility.useRandomSourceID = EditorGUILayout.Toggle("Multiple Client Mode:", Utility.useRandomSourceID, new GUILayoutOption[0]);
    }

    protected void ShowScenes()
    {
      UnityEngine.Object object1 = EditorGUILayout.ObjectField(this.m_OfflineSceneLabel, (UnityEngine.Object) this.GetSceneObject(this.m_NetworkManager.offlineScene), typeof (SceneAsset), false, new GUILayoutOption[0]);
      if (object1 == (UnityEngine.Object) null)
      {
        this.serializedObject.FindProperty("m_OfflineScene").stringValue = string.Empty;
        EditorUtility.SetDirty(this.target);
      }
      else if (object1.name != this.m_NetworkManager.offlineScene)
      {
        if ((UnityEngine.Object) this.GetSceneObject(object1.name) == (UnityEngine.Object) null)
        {
          Debug.LogWarning((object) ("The scene " + object1.name + " cannot be used. To use this scene add it to the build settings for the project"));
        }
        else
        {
          this.serializedObject.FindProperty("m_OfflineScene").stringValue = object1.name;
          EditorUtility.SetDirty(this.target);
        }
      }
      UnityEngine.Object object2 = EditorGUILayout.ObjectField(this.m_OnlineSceneLabel, (UnityEngine.Object) this.GetSceneObject(this.m_NetworkManager.onlineScene), typeof (SceneAsset), false, new GUILayoutOption[0]);
      if (object2 == (UnityEngine.Object) null)
      {
        this.serializedObject.FindProperty("m_OnlineScene").stringValue = string.Empty;
        EditorUtility.SetDirty(this.target);
      }
      else
      {
        if (!(object2.name != this.m_NetworkManager.onlineScene))
          return;
        if ((UnityEngine.Object) this.GetSceneObject(object2.name) == (UnityEngine.Object) null)
        {
          Debug.LogWarning((object) ("The scene " + object2.name + " cannot be used. To use this scene add it to the build settings for the project"));
        }
        else
        {
          this.serializedObject.FindProperty("m_OnlineScene").stringValue = object2.name;
          EditorUtility.SetDirty(this.target);
        }
      }
    }

    protected void ShowDerivedProperties(System.Type baseType, System.Type superType)
    {
      bool flag = true;
      SerializedProperty iterator = this.serializedObject.GetIterator();
      bool enterChildren = true;
      while (iterator.NextVisible(enterChildren))
      {
        System.Reflection.FieldInfo field = baseType.GetField(iterator.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        PropertyInfo property = baseType.GetProperty(iterator.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field == null && superType != null)
          field = superType.GetField(iterator.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property == null && superType != null)
          property = superType.GetProperty(iterator.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field == null && property == null)
        {
          if (flag)
          {
            flag = false;
            EditorGUI.BeginChangeCheck();
            this.serializedObject.Update();
            EditorGUILayout.Separator();
          }
          EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
          enterChildren = false;
        }
      }
      if (flag)
        return;
      this.serializedObject.ApplyModifiedProperties();
      EditorGUI.EndChangeCheck();
    }

    public override void OnInspectorGUI()
    {
      if (this.m_DontDestroyOnLoadProperty == null || this.m_DontDestroyOnLoadLabel == null)
        this.m_Initialized = false;
      this.Init();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_DontDestroyOnLoadProperty, this.m_DontDestroyOnLoadLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_RunInBackgroundProperty, this.m_RunInBackgroundLabel, new GUILayoutOption[0]);
      if (EditorGUILayout.PropertyField(this.m_LogLevelProperty))
        LogFilter.currentLogLevel = (int) this.m_NetworkManager.logLevel;
      this.ShowScenes();
      this.ShowNetworkInfo();
      this.ShowSpawnInfo();
      this.ShowConfigInfo();
      this.ShowSimulatorInfo();
      this.serializedObject.ApplyModifiedProperties();
      this.ShowDerivedProperties(typeof (NetworkManager), (System.Type) null);
    }

    private static void DrawHeader(Rect headerRect)
    {
      GUI.Label(headerRect, "Registered Spawnable Prefabs:");
    }

    internal void DrawChild(Rect r, int index, bool isActive, bool isFocused)
    {
      GameObject objectReferenceValue = (GameObject) this.m_SpawnListProperty.GetArrayElementAtIndex(index).objectReferenceValue;
      GUIContent label;
      if ((UnityEngine.Object) objectReferenceValue == (UnityEngine.Object) null)
      {
        label = new GUIContent("Empty", "Drag a prefab with a NetworkIdentity here");
      }
      else
      {
        NetworkIdentity component = objectReferenceValue.GetComponent<NetworkIdentity>();
        label = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? new GUIContent(objectReferenceValue.name, "No Network Identity") : new GUIContent(objectReferenceValue.name, "AssetId: [" + (object) component.assetId + "]");
      }
      GameObject gameObject = (GameObject) EditorGUI.ObjectField(r, label, (UnityEngine.Object) objectReferenceValue, typeof (GameObject), false);
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      {
        this.m_NetworkManager.spawnPrefabs[index] = (GameObject) null;
        EditorUtility.SetDirty(this.target);
      }
      else if ((bool) ((UnityEngine.Object) gameObject.GetComponent<NetworkIdentity>()))
      {
        if (!((UnityEngine.Object) this.m_NetworkManager.spawnPrefabs[index] != (UnityEngine.Object) gameObject))
          return;
        this.m_NetworkManager.spawnPrefabs[index] = gameObject;
        EditorUtility.SetDirty(this.target);
      }
      else
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Prefab " + (object) gameObject + " cannot be added as spawnable as it doesn't have a NetworkIdentity."));
      }
    }

    internal void Changed(ReorderableList list)
    {
      EditorUtility.SetDirty(this.target);
    }

    internal void AddButton(Rect rect, ReorderableList list)
    {
      this.m_NetworkManager.spawnPrefabs.Add((GameObject) null);
      this.m_SpawnList.index = this.m_SpawnList.count - 1;
      EditorUtility.SetDirty(this.target);
    }

    internal void RemoveButton(ReorderableList list)
    {
      this.m_NetworkManager.spawnPrefabs.RemoveAt(this.m_SpawnList.index);
      this.m_SpawnListProperty.DeleteArrayElementAtIndex(this.m_SpawnList.index);
      if (list.index >= this.m_SpawnListProperty.arraySize)
        list.index = this.m_SpawnListProperty.arraySize - 1;
      EditorUtility.SetDirty(this.target);
    }

    private static void ChannelDrawHeader(Rect headerRect)
    {
      GUI.Label(headerRect, "Qos Channels:");
    }

    internal void ChannelDrawChild(Rect r, int index, bool isActive, bool isFocused)
    {
      QosType channel = this.m_NetworkManager.channels[index];
      QosType qosType = (QosType) EditorGUI.EnumPopup(r, "Channel #" + (object) index, (Enum) channel);
      if (qosType == channel)
        return;
      this.m_NetworkManager.channels[index] = qosType;
      EditorUtility.SetDirty(this.target);
    }

    internal void ChannelChanged(ReorderableList list)
    {
      EditorUtility.SetDirty(this.target);
    }

    internal void ChannelAddButton(Rect rect, ReorderableList list)
    {
      this.m_NetworkManager.channels.Add(QosType.Reliable);
      this.m_ChannelList.index = this.m_ChannelList.count - 1;
      EditorUtility.SetDirty(this.target);
    }

    internal void ChannelRemoveButton(ReorderableList list)
    {
      if (this.m_NetworkManager.channels.Count == 1)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "Cannot remove channel. There must be at least one QoS channel.");
      }
      else
      {
        this.m_NetworkManager.channels.RemoveAt(this.m_ChannelList.index);
        this.m_ChannelListProperty.DeleteArrayElementAtIndex(this.m_ChannelList.index);
        if (list.index >= this.m_ChannelListProperty.arraySize)
          list.index = this.m_ChannelListProperty.arraySize - 1;
        EditorUtility.SetDirty(this.target);
      }
    }
  }
}
