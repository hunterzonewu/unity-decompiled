// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.HorizontalOrVerticalLayoutGroupEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>The Editor for the HorizontalOrVerticalLayoutGroup class.</para>
  /// </summary>
  [CustomEditor(typeof (HorizontalOrVerticalLayoutGroup), true)]
  [CanEditMultipleObjects]
  public class HorizontalOrVerticalLayoutGroupEditor : Editor
  {
    private SerializedProperty m_Padding;
    private SerializedProperty m_Spacing;
    private SerializedProperty m_ChildAlignment;
    private SerializedProperty m_ChildForceExpandWidth;
    private SerializedProperty m_ChildForceExpandHeight;

    protected virtual void OnEnable()
    {
      this.m_Padding = this.serializedObject.FindProperty("m_Padding");
      this.m_Spacing = this.serializedObject.FindProperty("m_Spacing");
      this.m_ChildAlignment = this.serializedObject.FindProperty("m_ChildAlignment");
      this.m_ChildForceExpandWidth = this.serializedObject.FindProperty("m_ChildForceExpandWidth");
      this.m_ChildForceExpandHeight = this.serializedObject.FindProperty("m_ChildForceExpandHeight");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Padding, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_Spacing, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_ChildAlignment, true, new GUILayoutOption[0]);
      Rect position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), -1, new GUIContent("Child Force Expand"));
      position.width = Mathf.Max(50f, (float) (((double) position.width - 4.0) / 3.0));
      EditorGUIUtility.labelWidth = 50f;
      this.ToggleLeft(position, this.m_ChildForceExpandWidth, new GUIContent("Width"));
      position.x += position.width + 2f;
      this.ToggleLeft(position, this.m_ChildForceExpandHeight, new GUIContent("Height"));
      this.serializedObject.ApplyModifiedProperties();
    }

    private void ToggleLeft(Rect position, SerializedProperty property, GUIContent label)
    {
      bool boolValue = property.boolValue;
      EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
      EditorGUI.BeginChangeCheck();
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      EditorGUI.ToggleLeft(position, label, boolValue);
      EditorGUI.indentLevel = indentLevel;
      if (EditorGUI.EndChangeCheck())
        property.boolValue = property.hasMultipleDifferentValues || !property.boolValue;
      EditorGUI.showMixedValue = false;
    }
  }
}
