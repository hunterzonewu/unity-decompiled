// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextAreaDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (TextAreaAttribute))]
  internal sealed class TextAreaDrawer : PropertyDrawer
  {
    private Vector2 m_ScrollPosition = new Vector2();
    private const int kLineHeight = 13;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.String)
      {
        label = EditorGUI.BeginProperty(position, label, property);
        Rect labelPosition = position;
        labelPosition.height = 16f;
        position.yMin += labelPosition.height;
        EditorGUI.HandlePrefixLabel(position, labelPosition, label);
        EditorGUI.BeginChangeCheck();
        string str = EditorGUI.ScrollableTextAreaInternal(position, property.stringValue, ref this.m_ScrollPosition, EditorStyles.textArea);
        if (EditorGUI.EndChangeCheck())
          property.stringValue = str;
        EditorGUI.EndProperty();
      }
      else
        EditorGUI.LabelField(position, label.text, "Use TextAreaDrawer with string.");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      TextAreaAttribute attribute = this.attribute as TextAreaAttribute;
      return 32f + (float) ((Mathf.Clamp(Mathf.CeilToInt(EditorStyles.textArea.CalcHeight(GUIContent.Temp(property.stringValue), EditorGUIUtility.contextWidth) / 13f), attribute.minLines, attribute.maxLines) - 1) * 13);
    }
  }
}
