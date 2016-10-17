// Decompiled with JetBrains decompiler
// Type: UnityEditor.NavMeshObstacleInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (NavMeshObstacle))]
  internal class NavMeshObstacleInspector : Editor
  {
    private SerializedProperty m_Shape;
    private SerializedProperty m_Center;
    private SerializedProperty m_Extents;
    private SerializedProperty m_Carve;
    private SerializedProperty m_MoveThreshold;
    private SerializedProperty m_TimeToStationary;
    private SerializedProperty m_CarveOnlyStationary;

    private void OnEnable()
    {
      this.m_Shape = this.serializedObject.FindProperty("m_Shape");
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_Extents = this.serializedObject.FindProperty("m_Extents");
      this.m_Carve = this.serializedObject.FindProperty("m_Carve");
      this.m_MoveThreshold = this.serializedObject.FindProperty("m_MoveThreshold");
      this.m_TimeToStationary = this.serializedObject.FindProperty("m_TimeToStationary");
      this.m_CarveOnlyStationary = this.serializedObject.FindProperty("m_CarveOnlyStationary");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_Shape);
      if (EditorGUI.EndChangeCheck())
      {
        this.serializedObject.ApplyModifiedProperties();
        (this.target as NavMeshObstacle).FitExtents();
        this.serializedObject.Update();
      }
      EditorGUILayout.PropertyField(this.m_Center);
      if (this.m_Shape.enumValueIndex == 0)
      {
        EditorGUI.BeginChangeCheck();
        float num1 = EditorGUILayout.FloatField("Radius", this.m_Extents.vector3Value.x, new GUILayoutOption[0]);
        float num2 = EditorGUILayout.FloatField("Height", this.m_Extents.vector3Value.y * 2f, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
          this.m_Extents.vector3Value = new Vector3(num1, num2 / 2f, num1);
      }
      else if (this.m_Shape.enumValueIndex == 1)
      {
        EditorGUI.BeginChangeCheck();
        Vector3 vector3 = EditorGUILayout.Vector3Field("Size", this.m_Extents.vector3Value * 2f);
        if (EditorGUI.EndChangeCheck())
          this.m_Extents.vector3Value = vector3 / 2f;
      }
      EditorGUILayout.PropertyField(this.m_Carve);
      if (this.m_Carve.boolValue)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_MoveThreshold);
        EditorGUILayout.PropertyField(this.m_TimeToStationary);
        EditorGUILayout.PropertyField(this.m_CarveOnlyStationary);
        --EditorGUI.indentLevel;
      }
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
