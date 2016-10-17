// Decompiled with JetBrains decompiler
// Type: UnityEditor.RangeDrawer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomPropertyDrawer(typeof (RangeAttribute))]
  internal sealed class RangeDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      RangeAttribute attribute = (RangeAttribute) this.attribute;
      if (property.propertyType == SerializedPropertyType.Float)
        EditorGUI.Slider(position, property, attribute.min, attribute.max, label);
      else if (property.propertyType == SerializedPropertyType.Integer)
        EditorGUI.IntSlider(position, property, (int) attribute.min, (int) attribute.max, label);
      else
        EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
    }
  }
}
