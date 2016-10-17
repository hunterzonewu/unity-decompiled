// Decompiled with JetBrains decompiler
// Type: UnityEditor.CharacterControllerEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (CharacterController))]
  [CanEditMultipleObjects]
  internal class CharacterControllerEditor : Editor
  {
    private SerializedProperty m_Height;
    private SerializedProperty m_Radius;
    private SerializedProperty m_SlopeLimit;
    private SerializedProperty m_StepOffset;
    private SerializedProperty m_SkinWidth;
    private SerializedProperty m_MinMoveDistance;
    private SerializedProperty m_Center;
    private SerializedProperty m_Direction;
    private int m_HandleControlID;

    public void OnEnable()
    {
      this.m_Height = this.serializedObject.FindProperty("m_Height");
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
      this.m_SlopeLimit = this.serializedObject.FindProperty("m_SlopeLimit");
      this.m_StepOffset = this.serializedObject.FindProperty("m_StepOffset");
      this.m_SkinWidth = this.serializedObject.FindProperty("m_SkinWidth");
      this.m_MinMoveDistance = this.serializedObject.FindProperty("m_MinMoveDistance");
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_HandleControlID = -1;
    }

    public void OnDisable()
    {
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_SlopeLimit);
      EditorGUILayout.PropertyField(this.m_StepOffset);
      EditorGUILayout.PropertyField(this.m_SkinWidth);
      EditorGUILayout.PropertyField(this.m_MinMoveDistance);
      EditorGUILayout.PropertyField(this.m_Center);
      EditorGUILayout.PropertyField(this.m_Radius);
      EditorGUILayout.PropertyField(this.m_Height);
      this.serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
      bool flag = GUIUtility.hotControl == this.m_HandleControlID;
      CharacterController target = (CharacterController) this.target;
      Color color = Handles.color;
      Handles.color = !target.enabled ? Handles.s_ColliderHandleColorDisabled : Handles.s_ColliderHandleColor;
      bool enabled = GUI.enabled;
      if (!Event.current.shift && !flag)
      {
        GUI.enabled = false;
        Handles.color = new Color(1f, 0.0f, 0.0f, 1f / 1000f);
      }
      float a = target.height * target.transform.lossyScale.y;
      float num1 = target.radius * Mathf.Max(target.transform.lossyScale.x, target.transform.lossyScale.z);
      float num2 = Mathf.Max(a, num1 * 2f);
      Matrix4x4 matrix = Matrix4x4.TRS(target.transform.TransformPoint(target.center), Quaternion.identity, Vector3.one);
      int hotControl = GUIUtility.hotControl;
      Vector3 localPos = Vector3.up * num2 * 0.5f;
      float num3 = CharacterControllerEditor.SizeHandle(localPos, Vector3.up, matrix, true);
      if (!GUI.changed)
        num3 = CharacterControllerEditor.SizeHandle(-localPos, Vector3.down, matrix, true);
      if (GUI.changed)
      {
        Undo.RecordObject((Object) target, "Character Controller Resize");
        float num4 = num2 / target.height;
        target.height += num3 / num4;
      }
      float num5 = CharacterControllerEditor.SizeHandle(Vector3.left * num1, Vector3.left, matrix, true);
      if (!GUI.changed)
        num5 = CharacterControllerEditor.SizeHandle(-Vector3.left * num1, -Vector3.left, matrix, true);
      if (!GUI.changed)
        num5 = CharacterControllerEditor.SizeHandle(Vector3.forward * num1, Vector3.forward, matrix, true);
      if (!GUI.changed)
        num5 = CharacterControllerEditor.SizeHandle(-Vector3.forward * num1, -Vector3.forward, matrix, true);
      if (GUI.changed)
      {
        Undo.RecordObject((Object) target, "Character Controller Resize");
        float num4 = num1 / target.radius;
        target.radius += num5 / num4;
      }
      if (hotControl != GUIUtility.hotControl && GUIUtility.hotControl != 0)
        this.m_HandleControlID = GUIUtility.hotControl;
      if (GUI.changed)
      {
        target.radius = Mathf.Max(target.radius, 1E-05f);
        target.height = Mathf.Max(target.height, 1E-05f);
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
