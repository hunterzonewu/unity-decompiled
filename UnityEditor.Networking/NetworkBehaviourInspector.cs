// Decompiled with JetBrains decompiler
// Type: UnityEditor.NetworkBehaviourInspector
// Assembly: UnityEditor.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6916FF53-78D9-4C64-A56C-55C7AE140DAE
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.Networking.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (NetworkBehaviour), true)]
  public class NetworkBehaviourInspector : Editor
  {
    protected List<string> m_SyncVarNames = new List<string>();
    private bool m_Initialized;
    private System.Type m_ScriptClass;
    private bool m_HasOnSerialize;
    private bool[] m_ShowSyncLists;
    protected GUIContent m_NetworkChannelLabel;
    protected GUIContent m_NetworkSendIntervalLabel;

    private void Init(MonoScript script)
    {
      this.m_Initialized = true;
      this.m_ScriptClass = script.GetClass();
      this.m_NetworkChannelLabel = new GUIContent("Network Channel", "QoS channel used for updates. Use the [NetworkSettings] class attribute to change this.");
      this.m_NetworkSendIntervalLabel = new GUIContent("Network Send Interval", "Maximum update rate in seconds. Use the [NetworkSettings] class attribute to change this, or implement GetNetworkSendInterval");
      foreach (System.Reflection.FieldInfo field in this.m_ScriptClass.GetFields(BindingFlags.Instance | BindingFlags.Public))
      {
        if (((Attribute[]) field.GetCustomAttributes(typeof (SyncVarAttribute), true)).Length > 0)
          this.m_SyncVarNames.Add(field.Name);
      }
      MethodInfo method = script.GetClass().GetMethod("OnSerialize");
      if (method != null && method.DeclaringType != typeof (NetworkBehaviour))
        this.m_HasOnSerialize = true;
      int length = 0;
      foreach (System.Reflection.FieldInfo field in this.serializedObject.targetObject.GetType().GetFields())
      {
        if (field.FieldType.BaseType != null && field.FieldType.BaseType.Name.Contains("SyncList"))
          ++length;
      }
      if (length <= 0)
        return;
      this.m_ShowSyncLists = new bool[length];
    }

    public override void OnInspectorGUI()
    {
      if (!this.m_Initialized)
      {
        this.serializedObject.Update();
        SerializedProperty property = this.serializedObject.FindProperty("m_Script");
        if (property == null)
          return;
        this.Init(property.objectReferenceValue as MonoScript);
      }
      EditorGUI.BeginChangeCheck();
      this.serializedObject.Update();
      SerializedProperty iterator = this.serializedObject.GetIterator();
      for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
      {
        bool flag = this.m_SyncVarNames.Contains(iterator.name);
        if (iterator.propertyType == SerializedPropertyType.ObjectReference)
        {
          EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
        }
        else
        {
          EditorGUILayout.BeginHorizontal();
          EditorGUILayout.PropertyField(iterator, true, new GUILayoutOption[0]);
          if (flag)
            GUILayout.Label("SyncVar", EditorStyles.toolbarTextField, new GUILayoutOption[1]
            {
              GUILayout.Width(52f)
            });
          EditorGUILayout.EndHorizontal();
        }
      }
      this.serializedObject.ApplyModifiedProperties();
      EditorGUI.EndChangeCheck();
      int index = 0;
      foreach (System.Reflection.FieldInfo field in this.serializedObject.targetObject.GetType().GetFields())
      {
        if (field.FieldType.BaseType != null && field.FieldType.BaseType.Name.Contains("SyncList"))
        {
          this.m_ShowSyncLists[index] = (EditorGUILayout.Foldout((this.m_ShowSyncLists[index] ? 1 : 0) != 0, "SyncList " + field.Name + "  [" + field.FieldType.Name + "]") ? 1 : 0) != 0;
          if (this.m_ShowSyncLists[index])
          {
            ++EditorGUI.indentLevel;
            IEnumerable enumerable = field.GetValue((object) this.serializedObject.targetObject) as IEnumerable;
            if (enumerable != null)
            {
              int num = 0;
              IEnumerator enumerator = enumerable.GetEnumerator();
              while (enumerator.MoveNext())
              {
                if (enumerator.Current != null)
                  EditorGUILayout.LabelField("Item:" + (object) num, enumerator.Current.ToString(), new GUILayoutOption[0]);
                ++num;
              }
            }
            --EditorGUI.indentLevel;
          }
          ++index;
        }
      }
      if (!this.m_HasOnSerialize)
        return;
      NetworkBehaviour target = this.target as NetworkBehaviour;
      if (!((UnityEngine.Object) target != (UnityEngine.Object) null))
        return;
      EditorGUILayout.LabelField(this.m_NetworkChannelLabel, new GUIContent(target.GetNetworkChannel().ToString()), new GUILayoutOption[0]);
      EditorGUILayout.LabelField(this.m_NetworkSendIntervalLabel, new GUIContent(target.GetNetworkSendInterval().ToString()), new GUILayoutOption[0]);
    }
  }
}
