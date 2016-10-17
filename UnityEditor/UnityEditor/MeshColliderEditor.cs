// Decompiled with JetBrains decompiler
// Type: UnityEditor.MeshColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (MeshCollider))]
  internal class MeshColliderEditor : Collider3DEditorBase
  {
    private SerializedProperty m_Mesh;
    private SerializedProperty m_Convex;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Mesh = this.serializedObject.FindProperty("m_Mesh");
      this.m_Convex = this.serializedObject.FindProperty("m_Convex");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_Convex, MeshColliderEditor.Texts.convextText, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck() && !this.m_Convex.boolValue)
        this.m_IsTrigger.boolValue = false;
      ++EditorGUI.indentLevel;
      EditorGUI.BeginDisabledGroup(!this.m_Convex.boolValue);
      EditorGUILayout.PropertyField(this.m_IsTrigger, MeshColliderEditor.Texts.isTriggerText, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      --EditorGUI.indentLevel;
      EditorGUILayout.PropertyField(this.m_Material);
      EditorGUILayout.PropertyField(this.m_Mesh);
      this.serializedObject.ApplyModifiedProperties();
    }

    private static class Texts
    {
      public static GUIContent isTriggerText = new GUIContent("Is Trigger", "Is this collider a trigger? Triggers are only supported on convex colliders.");
      public static GUIContent convextText = new GUIContent("Convex", "Is this collider convex?");
    }
  }
}
