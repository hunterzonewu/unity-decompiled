// Decompiled with JetBrains decompiler
// Type: UnityEditor.DelayedDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (DelayedAttribute))]
  internal sealed class DelayedDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.Float)
        EditorGUI.DelayedFloatField(position, property, label);
      else if (property.propertyType == SerializedPropertyType.Integer)
        EditorGUI.DelayedIntField(position, property, label);
      else if (property.propertyType == SerializedPropertyType.String)
        EditorGUI.DelayedTextField(position, property, label);
      else
        EditorGUI.LabelField(position, label.text, "Use Delayed with float, int, or string.");
    }
  }
}
