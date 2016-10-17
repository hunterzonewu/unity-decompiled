// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkIdentityEditor
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  [CustomEditor(typeof (NetworkIdentity), true)]
  [CanEditMultipleObjects]
  public class NetworkIdentityEditor : Editor
  {
    private SerializedProperty m_ServerOnlyProperty;
    private SerializedProperty m_LocalPlayerAuthorityProperty;
    private GUIContent m_ServerOnlyLabel;
    private GUIContent m_LocalPlayerAuthorityLabel;
    private GUIContent m_SpawnLabel;
    private NetworkIdentity m_NetworkIdentity;
    private bool m_Initialized;
    private bool m_ShowObservers;

    private void Init()
    {
      if (this.m_Initialized)
        return;
      this.m_Initialized = true;
      this.m_NetworkIdentity = this.target as NetworkIdentity;
      this.m_ServerOnlyProperty = this.serializedObject.FindProperty("m_ServerOnly");
      this.m_LocalPlayerAuthorityProperty = this.serializedObject.FindProperty("m_LocalPlayerAuthority");
      this.m_ServerOnlyLabel = new GUIContent("Server Only", "True if the object should only exist on the server.");
      this.m_LocalPlayerAuthorityLabel = new GUIContent("Local Player Authority", "True if this object will be controlled by a player on a client.");
      this.m_SpawnLabel = new GUIContent("Spawn Object", "This causes an unspawned server object to be spawned on clients");
    }

    public override void OnInspectorGUI()
    {
      if (this.m_ServerOnlyProperty == null)
        this.m_Initialized = false;
      this.Init();
      this.serializedObject.Update();
      if (this.m_ServerOnlyProperty.boolValue)
      {
        EditorGUILayout.PropertyField(this.m_ServerOnlyProperty, this.m_ServerOnlyLabel, new GUILayoutOption[0]);
        EditorGUILayout.LabelField("Local Player Authority cannot be set for server-only objects");
      }
      else if (this.m_LocalPlayerAuthorityProperty.boolValue)
      {
        EditorGUILayout.LabelField("Server Only cannot be set for Local Player Authority objects");
        EditorGUILayout.PropertyField(this.m_LocalPlayerAuthorityProperty, this.m_LocalPlayerAuthorityLabel, new GUILayoutOption[0]);
      }
      else
      {
        EditorGUILayout.PropertyField(this.m_ServerOnlyProperty, this.m_ServerOnlyLabel, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_LocalPlayerAuthorityProperty, this.m_LocalPlayerAuthorityLabel, new GUILayoutOption[0]);
      }
      this.serializedObject.ApplyModifiedProperties();
      if (!Application.isPlaying)
        return;
      EditorGUILayout.Separator();
      if (this.m_NetworkIdentity.observers != null && this.m_NetworkIdentity.observers.Count > 0)
      {
        this.m_ShowObservers = EditorGUILayout.Foldout(this.m_ShowObservers, "Observers");
        if (this.m_ShowObservers)
        {
          ++EditorGUI.indentLevel;
          foreach (NetworkConnection observer in this.m_NetworkIdentity.observers)
          {
            GameObject gameObject = (GameObject) null;
            using (List<PlayerController>.Enumerator enumerator = observer.playerControllers.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                PlayerController current = enumerator.Current;
                if (current != null)
                {
                  gameObject = current.gameObject;
                  break;
                }
              }
            }
            if ((bool) ((Object) gameObject))
              EditorGUILayout.ObjectField("Connection " + (object) observer.connectionId, (Object) gameObject, typeof (GameObject), false, new GUILayoutOption[0]);
            else
              EditorGUILayout.TextField("Connection " + (object) observer.connectionId);
          }
          --EditorGUI.indentLevel;
        }
      }
      if (PrefabUtility.GetPrefabType((Object) this.m_NetworkIdentity.gameObject) == PrefabType.Prefab || !this.m_NetworkIdentity.gameObject.activeSelf || (!this.m_NetworkIdentity.netId.IsEmpty() || !NetworkServer.active))
        return;
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(this.m_SpawnLabel);
      if (GUILayout.Toggle(false, "Spawn", EditorStyles.miniButtonLeft, new GUILayoutOption[0]))
      {
        NetworkServer.Spawn(this.m_NetworkIdentity.gameObject);
        EditorUtility.SetDirty(this.target);
      }
      EditorGUILayout.EndHorizontal();
    }
  }
}
