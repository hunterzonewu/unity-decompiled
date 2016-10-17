// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkMigrationManagerEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEditor
{
  [CustomEditor(typeof (NetworkMigrationManager), true)]
  public class NetworkMigrationManagerEditor : Editor
  {
    private bool m_Initialized;
    private NetworkMigrationManager m_Manager;
    private SerializedProperty m_HostMigrationProperty;
    private SerializedProperty m_ShowGUIProperty;
    private SerializedProperty m_OffsetXProperty;
    private SerializedProperty m_OffsetYProperty;
    private GUIContent m_HostMigrationLabel;
    private bool m_ShowPeers;
    private bool m_ShowPlayers;

    private void Init()
    {
      if (this.m_Initialized && this.m_HostMigrationProperty != null)
        return;
      this.m_Initialized = true;
      this.m_Manager = this.target as NetworkMigrationManager;
      this.m_HostMigrationProperty = this.serializedObject.FindProperty("m_HostMigration");
      this.m_ShowGUIProperty = this.serializedObject.FindProperty("m_ShowGUI");
      this.m_OffsetXProperty = this.serializedObject.FindProperty("m_OffsetX");
      this.m_OffsetYProperty = this.serializedObject.FindProperty("m_OffsetY");
      this.m_HostMigrationLabel = new GUIContent("Use Host Migration", "s.");
    }

    public override void OnInspectorGUI()
    {
      this.Init();
      this.serializedObject.Update();
      this.DrawControls();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void DrawControls()
    {
      if ((Object) this.m_Manager == (Object) null)
        return;
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_HostMigrationProperty, this.m_HostMigrationLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_ShowGUIProperty);
      if (this.m_Manager.showGUI)
      {
        EditorGUILayout.PropertyField(this.m_OffsetXProperty);
        EditorGUILayout.PropertyField(this.m_OffsetYProperty);
      }
      if (EditorGUI.EndChangeCheck())
        this.serializedObject.ApplyModifiedProperties();
      if (!Application.isPlaying)
        return;
      EditorGUILayout.Separator();
      EditorGUILayout.LabelField("Disconnected From Host", this.m_Manager.disconnectedFromHost.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Waiting to become New Host", this.m_Manager.waitingToBecomeNewHost.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Waitingto Reconnect to New Host", this.m_Manager.waitingReconnectToNewHost.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Your ConnectionId", this.m_Manager.oldServerConnectionId.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("New Host Address", this.m_Manager.newHostAddress, new GUILayoutOption[0]);
      if (this.m_Manager.peers != null)
      {
        this.m_ShowPeers = EditorGUILayout.Foldout(this.m_ShowPeers, "Peers");
        if (this.m_ShowPeers)
        {
          ++EditorGUI.indentLevel;
          foreach (PeerInfoMessage peer in this.m_Manager.peers)
            EditorGUILayout.LabelField("Peer: ", peer.ToString(), new GUILayoutOption[0]);
          --EditorGUI.indentLevel;
        }
      }
      if (this.m_Manager.pendingPlayers == null)
        return;
      this.m_ShowPlayers = EditorGUILayout.Foldout(this.m_ShowPlayers, "Pending Players");
      if (!this.m_ShowPlayers)
        return;
      ++EditorGUI.indentLevel;
      using (Dictionary<int, NetworkMigrationManager.ConnectionPendingPlayers>.KeyCollection.Enumerator enumerator1 = this.m_Manager.pendingPlayers.Keys.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          int current1 = enumerator1.Current;
          EditorGUILayout.LabelField("Connection: ", current1.ToString(), new GUILayoutOption[0]);
          ++EditorGUI.indentLevel;
          using (List<NetworkMigrationManager.PendingPlayerInfo>.Enumerator enumerator2 = this.m_Manager.pendingPlayers[current1].players.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              NetworkMigrationManager.PendingPlayerInfo current2 = enumerator2.Current;
              EditorGUILayout.ObjectField("Player netId:" + (object) current2.netId + " contId:" + (object) current2.playerControllerId, (Object) current2.obj, typeof (GameObject), false, new GUILayoutOption[0]);
            }
          }
          --EditorGUI.indentLevel;
        }
      }
      --EditorGUI.indentLevel;
    }
  }
}
