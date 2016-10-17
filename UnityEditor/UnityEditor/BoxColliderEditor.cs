// Decompiled with JetBrains decompiler
// Type: UnityEditor.BoxColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (BoxCollider))]
  internal class BoxColliderEditor : Collider3DEditorBase
  {
    private static readonly int s_BoxHash = "BoxColliderEditor".GetHashCode();
    private readonly BoxEditor m_BoxEditor = new BoxEditor(true, BoxColliderEditor.s_BoxHash);
    private SerializedProperty m_Center;
    private SerializedProperty m_Size;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_Size = this.serializedObject.FindProperty("m_Size");
      this.m_BoxEditor.OnEnable();
      this.m_BoxEditor.SetAlwaysDisplayHandles(true);
    }

    public override void OnDisable()
    {
      base.OnDisable();
      this.m_BoxEditor.OnDisable();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.InspectorEditButtonGUI();
      EditorGUILayout.PropertyField(this.m_IsTrigger);
      EditorGUILayout.PropertyField(this.m_Material);
      EditorGUILayout.PropertyField(this.m_Center);
      EditorGUILayout.PropertyField(this.m_Size);
      this.serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      BoxCollider target = (BoxCollider) this.target;
      Vector3 center = target.center;
      Vector3 size = target.size;
      Color color = Handles.s_ColliderHandleColor;
      if (!target.enabled)
        color = Handles.s_ColliderHandleColorDisabled;
      if (!this.m_BoxEditor.OnSceneGUI(target.transform, color, ref center, ref size))
        return;
      Undo.RecordObject((Object) target, "Modify Box Collider");
      target.center = center;
      target.size = size;
    }
  }
}
