// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.InputFieldEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the InputField Component.</para>
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (InputField), true)]
  public class InputFieldEditor : SelectableEditor
  {
    private SerializedProperty m_TextComponent;
    private SerializedProperty m_Text;
    private SerializedProperty m_ContentType;
    private SerializedProperty m_LineType;
    private SerializedProperty m_InputType;
    private SerializedProperty m_CharacterValidation;
    private SerializedProperty m_KeyboardType;
    private SerializedProperty m_CharacterLimit;
    private SerializedProperty m_CaretBlinkRate;
    private SerializedProperty m_CaretWidth;
    private SerializedProperty m_CaretColor;
    private SerializedProperty m_CustomCaretColor;
    private SerializedProperty m_SelectionColor;
    private SerializedProperty m_HideMobileInput;
    private SerializedProperty m_Placeholder;
    private SerializedProperty m_OnValueChanged;
    private SerializedProperty m_OnEndEdit;
    private SerializedProperty m_ReadOnly;
    private AnimBool m_CustomColor;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_TextComponent = this.serializedObject.FindProperty("m_TextComponent");
      this.m_Text = this.serializedObject.FindProperty("m_Text");
      this.m_ContentType = this.serializedObject.FindProperty("m_ContentType");
      this.m_LineType = this.serializedObject.FindProperty("m_LineType");
      this.m_InputType = this.serializedObject.FindProperty("m_InputType");
      this.m_CharacterValidation = this.serializedObject.FindProperty("m_CharacterValidation");
      this.m_KeyboardType = this.serializedObject.FindProperty("m_KeyboardType");
      this.m_CharacterLimit = this.serializedObject.FindProperty("m_CharacterLimit");
      this.m_CaretBlinkRate = this.serializedObject.FindProperty("m_CaretBlinkRate");
      this.m_CaretWidth = this.serializedObject.FindProperty("m_CaretWidth");
      this.m_CaretColor = this.serializedObject.FindProperty("m_CaretColor");
      this.m_CustomCaretColor = this.serializedObject.FindProperty("m_CustomCaretColor");
      this.m_SelectionColor = this.serializedObject.FindProperty("m_SelectionColor");
      this.m_HideMobileInput = this.serializedObject.FindProperty("m_HideMobileInput");
      this.m_Placeholder = this.serializedObject.FindProperty("m_Placeholder");
      this.m_OnValueChanged = this.serializedObject.FindProperty("m_OnValueChanged");
      this.m_OnEndEdit = this.serializedObject.FindProperty("m_OnEndEdit");
      this.m_ReadOnly = this.serializedObject.FindProperty("m_ReadOnly");
      this.m_CustomColor = new AnimBool(this.m_CustomCaretColor.boolValue);
      this.m_CustomColor.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    protected override void OnDisable()
    {
      base.OnDisable();
      this.m_CustomColor.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    /// <summary>
    ///   <para>See: Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      base.OnInspectorGUI();
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_TextComponent);
      if (this.m_TextComponent != null && this.m_TextComponent.objectReferenceValue != (Object) null && (this.m_TextComponent.objectReferenceValue as Text).supportRichText)
        EditorGUILayout.HelpBox("Using Rich Text with input is unsupported.", MessageType.Warning);
      EditorGUI.BeginDisabledGroup(this.m_TextComponent == null || this.m_TextComponent.objectReferenceValue == (Object) null);
      EditorGUILayout.PropertyField(this.m_Text);
      EditorGUILayout.PropertyField(this.m_CharacterLimit);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_ContentType);
      if (!this.m_ContentType.hasMultipleDifferentValues)
      {
        ++EditorGUI.indentLevel;
        if (this.m_ContentType.enumValueIndex == 0 || this.m_ContentType.enumValueIndex == 1 || this.m_ContentType.enumValueIndex == 9)
          EditorGUILayout.PropertyField(this.m_LineType);
        if (this.m_ContentType.enumValueIndex == 9)
        {
          EditorGUILayout.PropertyField(this.m_InputType);
          EditorGUILayout.PropertyField(this.m_KeyboardType);
          EditorGUILayout.PropertyField(this.m_CharacterValidation);
        }
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_Placeholder);
      EditorGUILayout.PropertyField(this.m_CaretBlinkRate);
      EditorGUILayout.PropertyField(this.m_CaretWidth);
      EditorGUILayout.PropertyField(this.m_CustomCaretColor);
      this.m_CustomColor.target = this.m_CustomCaretColor.boolValue;
      if (EditorGUILayout.BeginFadeGroup(this.m_CustomColor.faded))
        EditorGUILayout.PropertyField(this.m_CaretColor);
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.PropertyField(this.m_SelectionColor);
      EditorGUILayout.PropertyField(this.m_HideMobileInput);
      EditorGUILayout.PropertyField(this.m_ReadOnly);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_OnValueChanged);
      EditorGUILayout.PropertyField(this.m_OnEndEdit);
      EditorGUI.EndDisabledGroup();
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
