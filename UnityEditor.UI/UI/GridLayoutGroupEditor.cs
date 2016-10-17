// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.GridLayoutGroupEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the GridLayout Component.</para>
  /// </summary>
  [CustomEditor(typeof (GridLayoutGroup), true)]
  [CanEditMultipleObjects]
  public class GridLayoutGroupEditor : Editor
  {
    private SerializedProperty m_Padding;
    private SerializedProperty m_CellSize;
    private SerializedProperty m_Spacing;
    private SerializedProperty m_StartCorner;
    private SerializedProperty m_StartAxis;
    private SerializedProperty m_ChildAlignment;
    private SerializedProperty m_Constraint;
    private SerializedProperty m_ConstraintCount;

    protected virtual void OnEnable()
    {
      this.m_Padding = this.serializedObject.FindProperty("m_Padding");
      this.m_CellSize = this.serializedObject.FindProperty("m_CellSize");
      this.m_Spacing = this.serializedObject.FindProperty("m_Spacing");
      this.m_StartCorner = this.serializedObject.FindProperty("m_StartCorner");
      this.m_StartAxis = this.serializedObject.FindProperty("m_StartAxis");
      this.m_ChildAlignment = this.serializedObject.FindProperty("m_ChildAlignment");
      this.m_Constraint = this.serializedObject.FindProperty("m_Constraint");
      this.m_ConstraintCount = this.serializedObject.FindProperty("m_ConstraintCount");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Padding, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_CellSize, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Spacing, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_StartCorner, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_StartAxis, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_ChildAlignment, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Constraint, true, new GUILayoutOption[0]);
      if (this.m_Constraint.enumValueIndex > 0)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_ConstraintCount, true, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
      }
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
