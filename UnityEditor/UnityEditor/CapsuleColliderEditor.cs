// Decompiled with JetBrains decompiler
// Type: UnityEditor.CapsuleColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (CapsuleCollider))]
  [CanEditMultipleObjects]
  internal class CapsuleColliderEditor : Collider3DEditorBase
  {
    private SerializedProperty m_Center;
    private SerializedProperty m_Radius;
    private SerializedProperty m_Height;
    private SerializedProperty m_Direction;
    private int m_HandleControlID;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
      this.m_Height = this.serializedObject.FindProperty("m_Height");
      this.m_Direction = this.serializedObject.FindProperty("m_Direction");
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
      EditorGUILayout.PropertyField(this.m_Height);
      EditorGUILayout.PropertyField(this.m_Direction);
      this.serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
      bool flag = GUIUtility.hotControl == this.m_HandleControlID;
      CapsuleCollider target = (CapsuleCollider) this.target;
      Color color = Handles.color;
      Handles.color = !target.enabled ? Handles.s_ColliderHandleColorDisabled : Handles.s_ColliderHandleColor;
      bool enabled = GUI.enabled;
      if (!this.editingCollider && !flag)
      {
        GUI.enabled = false;
        Handles.color = new Color(1f, 0.0f, 0.0f, 1f / 1000f);
      }
      Vector3 capsuleExtents = ColliderUtil.GetCapsuleExtents(target);
      float num1 = capsuleExtents.y + 2f * capsuleExtents.x;
      float x = capsuleExtents.x;
      Matrix4x4 capsuleTransform = ColliderUtil.CalculateCapsuleTransform(target);
      int hotControl = GUIUtility.hotControl;
      float height = target.height;
      Vector3 localPos = Vector3.left * num1 * 0.5f;
      float num2 = CapsuleColliderEditor.SizeHandle(localPos, Vector3.left, capsuleTransform, true);
      if (!GUI.changed)
        num2 = CapsuleColliderEditor.SizeHandle(-localPos, Vector3.right, capsuleTransform, true);
      if (GUI.changed)
      {
        float num3 = num1 / target.height;
        height += num2 / num3;
      }
      float radius = target.radius;
      float num4 = CapsuleColliderEditor.SizeHandle(Vector3.forward * x, Vector3.forward, capsuleTransform, true);
      if (!GUI.changed)
        num4 = CapsuleColliderEditor.SizeHandle(-Vector3.forward * x, -Vector3.forward, capsuleTransform, true);
      if (!GUI.changed)
        num4 = CapsuleColliderEditor.SizeHandle(Vector3.up * x, Vector3.up, capsuleTransform, true);
      if (!GUI.changed)
        num4 = CapsuleColliderEditor.SizeHandle(-Vector3.up * x, -Vector3.up, capsuleTransform, true);
      if (GUI.changed)
      {
        float num3 = Mathf.Max(capsuleExtents.z / target.radius, capsuleExtents.x / target.radius);
        radius += num4 / num3;
      }
      if (hotControl != GUIUtility.hotControl && GUIUtility.hotControl != 0)
        this.m_HandleControlID = GUIUtility.hotControl;
      if (GUI.changed)
      {
        Undo.RecordObject((Object) target, "Modify Capsule Collider");
        target.radius = Mathf.Max(radius, 1E-05f);
        target.height = Mathf.Max(height, 1E-05f);
      }
      Handles.color = color;
      GUI.enabled = enabled;
    }

    private static float SizeHandle(Vector3 localPos, Vector3 localPullDir, Matrix4x4 matrix, bool isEdgeHandle)
    {
      Vector3 vector3_1 = matrix.MultiplyVector(localPullDir);
      Vector3 vector3_2 = matrix.MultiplyPoint(localPos);
      float handleSize = HandleUtility.GetHandleSize(vector3_2);
      bool changed = GUI.changed;
      GUI.changed = false;
      Color color = Handles.color;
      float num1 = 0.0f;
      if (isEdgeHandle)
        num1 = Mathf.Cos(0.7853982f);
      if ((!Camera.current.orthographic ? (double) Vector3.Dot((Camera.current.transform.position - vector3_2).normalized, vector3_1) : (double) Vector3.Dot(-Camera.current.transform.forward, vector3_1)) < -(double) num1)
        Handles.color = new Color(Handles.color.r, Handles.color.g, Handles.color.b, Handles.color.a * Handles.backfaceAlphaMultiplier);
      Vector3 point = Handles.Slider(vector3_2, vector3_1, handleSize * 0.03f, new Handles.DrawCapFunction(Handles.DotCap), 0.0f);
      float num2 = 0.0f;
      if (GUI.changed)
        num2 = HandleUtility.PointOnLineParameter(point, vector3_2, vector3_1);
      GUI.changed |= changed;
      Handles.color = color;
      return num2;
    }
  }
}
