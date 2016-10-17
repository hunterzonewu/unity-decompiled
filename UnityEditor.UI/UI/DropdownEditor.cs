// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.DropdownEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom editor for the Dropdown component.</para>
  /// </summary>
  [CustomEditor(typeof (Dropdown), true)]
  [CanEditMultipleObjects]
  public class DropdownEditor : SelectableEditor
  {
    private SerializedProperty m_Template;
    private SerializedProperty m_CaptionText;
    private SerializedProperty m_CaptionImage;
    private SerializedProperty m_ItemText;
    private SerializedProperty m_ItemImage;
    private SerializedProperty m_OnSelectionChanged;
    private SerializedProperty m_Value;
    private SerializedProperty m_Options;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_Template = this.serializedObject.FindProperty("m_Template");
      this.m_CaptionText = this.serializedObject.FindProperty("m_CaptionText");
      this.m_CaptionImage = this.serializedObject.FindProperty("m_CaptionImage");
      this.m_ItemText = this.serializedObject.FindProperty("m_ItemText");
      this.m_ItemImage = this.serializedObject.FindProperty("m_ItemImage");
      this.m_OnSelectionChanged = this.serializedObject.FindProperty("m_OnValueChanged");
      this.m_Value = this.serializedObject.FindProperty("m_Value");
      this.m_Options = this.serializedObject.FindProperty("m_Options");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      EditorGUILayout.Space();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Template);
      EditorGUILayout.PropertyField(this.m_CaptionText);
      EditorGUILayout.PropertyField(this.m_CaptionImage);
      EditorGUILayout.PropertyField(this.m_ItemText);
      EditorGUILayout.PropertyField(this.m_ItemImage);
      EditorGUILayout.PropertyField(this.m_Value);
      EditorGUILayout.PropertyField(this.m_Options);
      EditorGUILayout.PropertyField(this.m_OnSelectionChanged);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
