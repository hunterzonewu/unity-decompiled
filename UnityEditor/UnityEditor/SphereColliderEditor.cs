// Decompiled with JetBrains decompiler
// Type: UnityEditor.SphereColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SphereCollider))]
  internal class SphereColliderEditor : Collider3DEditorBase
  {
    private SerializedProperty m_Center;
    private SerializedProperty m_Radius;
    private int m_HandleControlID;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
      this.m_HandleControlID = -1;
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.InspectorEditButtonGUI();
      EditorGUILayout.PropertyField(this.m_IsTrigger);
      EditorGUILayout.PropertyField(this.m_Material);
      EditorGUILayout.PropertyField(this.m_Center);
      EditorGUILayout.PropertyField(this.m_Radius);
      this.serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
      bool flag = GUIUtility.hotControl == this.m_HandleControlID;
      SphereCollider target = (SphereCollider) this.target;
      Color color = Handles.color;
      Handles.color = !target.enabled ? Handles.s_ColliderHandleColorDisabled : Handles.s_ColliderHandleColor;
      bool enabled = GUI.enabled;
      if (!this.editingCollider && !flag)
      {
        GUI.enabled = false;
        Handles.color = new Color(0.0f, 0.0f, 0.0f, 1f / 1000f);
      }
      Vector3 lossyScale = target.transform.lossyScale;
      float num1 = Mathf.Max(Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y)), Mathf.Abs(lossyScale.z));
      float radius = Mathf.Max(Mathf.Abs(num1 * target.radius), 1E-05f);
      Vector3 position = target.transform.TransformPoint(target.center);
      Quaternion rotation = target.transform.rotation;
      int hotControl = GUIUtility.hotControl;
      float num2 = Handles.RadiusHandle(rotation, position, radius, true);
      if (GUI.changed)
      {
        Undo.RecordObject((Object) target, "Adjust Radius");
        target.radius = num2 * 1f / num1;
      }
      if (hotControl != GUIUtility.hotControl && GUIUtility.hotControl != 0)
        this.m_HandleControlID = GUIUtility.hotControl;
      Handles.color = color;
      GUI.enabled = enabled;
    }
  }
}
