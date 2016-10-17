// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkDiscoveryEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (NetworkDiscovery), true)]
  public class NetworkDiscoveryEditor : Editor
  {
    private bool m_Initialized;
    private NetworkDiscovery m_Discovery;
    private SerializedProperty m_BroadcastPortProperty;
    private SerializedProperty m_BroadcastKeyProperty;
    private SerializedProperty m_BroadcastVersionProperty;
    private SerializedProperty m_BroadcastSubVersionProperty;
    private SerializedProperty m_BroadcastIntervalProperty;
    private SerializedProperty m_UseNetworkManagerProperty;
    private SerializedProperty m_BroadcastDataProperty;
    private SerializedProperty m_ShowGUIProperty;
    private SerializedProperty m_OffsetXProperty;
    private SerializedProperty m_OffsetYProperty;
    private GUIContent m_BroadcastPortLabel;
    private GUIContent m_BroadcastKeyLabel;
    private GUIContent m_BroadcastVersionLabel;
    private GUIContent m_BroadcastSubVersionLabel;
    private GUIContent m_BroadcastIntervalLabel;
    private GUIContent m_UseNetworkManagerLabel;
    private GUIContent m_BroadcastDataLabel;

    private void Init()
    {
      if (this.m_Initialized && this.m_BroadcastPortProperty != null)
        return;
      this.m_Initialized = true;
      this.m_Discovery = this.target as NetworkDiscovery;
      this.m_BroadcastPortProperty = this.serializedObject.FindProperty("m_BroadcastPort");
      this.m_BroadcastKeyProperty = this.serializedObject.FindProperty("m_BroadcastKey");
      this.m_BroadcastVersionProperty = this.serializedObject.FindProperty("m_BroadcastVersion");
      this.m_BroadcastSubVersionProperty = this.serializedObject.FindProperty("m_BroadcastSubVersion");
      this.m_BroadcastIntervalProperty = this.serializedObject.FindProperty("m_BroadcastInterval");
      this.m_UseNetworkManagerProperty = this.serializedObject.FindProperty("m_UseNetworkManager");
      this.m_BroadcastDataProperty = this.serializedObject.FindProperty("m_BroadcastData");
      this.m_ShowGUIProperty = this.serializedObject.FindProperty("m_ShowGUI");
      this.m_OffsetXProperty = this.serializedObject.FindProperty("m_OffsetX");
      this.m_OffsetYProperty = this.serializedObject.FindProperty("m_OffsetY");
      this.m_BroadcastPortLabel = new GUIContent("Broadcast Port", "The network port to broadcast to, and listen on.");
      this.m_BroadcastKeyLabel = new GUIContent("Broadcast Key", "The key to broadcast. This key typically identifies the application.");
      this.m_BroadcastVersionLabel = new GUIContent("Broadcast Version", "The version of the application to broadcast. This is used to match versions of the same application.");
      this.m_BroadcastSubVersionLabel = new GUIContent("Broadcast SubVersion", "The sub-version of the application to broadcast.");
      this.m_BroadcastIntervalLabel = new GUIContent("Broadcast Interval", "How often in milliseconds to broadcast when running as a server.");
      this.m_UseNetworkManagerLabel = new GUIContent("Use NetworkManager", "Broadcast information from the NetworkManager, and auto-join matching games using the NetworkManager.");
      this.m_BroadcastDataLabel = new GUIContent("Broadcast Data", "The data to broadcast when not using the NetworkManager");
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
      if ((Object) this.m_Discovery == (Object) null)
        return;
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_BroadcastPortProperty, this.m_BroadcastPortLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_BroadcastKeyProperty, this.m_BroadcastKeyLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_BroadcastVersionProperty, this.m_BroadcastVersionLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_BroadcastSubVersionProperty, this.m_BroadcastSubVersionLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_BroadcastIntervalProperty, this.m_BroadcastIntervalLabel, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_UseNetworkManagerProperty, this.m_UseNetworkManagerLabel, new GUILayoutOption[0]);
      if (this.m_Discovery.useNetworkManager)
        EditorGUILayout.LabelField(this.m_BroadcastDataLabel, new GUIContent(this.m_BroadcastDataProperty.stringValue), new GUILayoutOption[0]);
      else
        EditorGUILayout.PropertyField(this.m_BroadcastDataProperty, this.m_BroadcastDataLabel, new GUILayoutOption[0]);
      EditorGUILayout.Separator();
      EditorGUILayout.PropertyField(this.m_ShowGUIProperty);
      if (this.m_Discovery.showGUI)
      {
        EditorGUILayout.PropertyField(this.m_OffsetXProperty);
        EditorGUILayout.PropertyField(this.m_OffsetYProperty);
      }
      if (EditorGUI.EndChangeCheck())
        this.serializedObject.ApplyModifiedProperties();
      if (!Application.isPlaying)
        return;
      EditorGUILayout.Separator();
      EditorGUILayout.LabelField("hostId", this.m_Discovery.hostId.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("running", this.m_Discovery.running.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("isServer", this.m_Discovery.isServer.ToString(), new GUILayoutOption[0]);
      EditorGUILayout.LabelField("isClient", this.m_Discovery.isClient.ToString(), new GUILayoutOption[0]);
    }
  }
}
