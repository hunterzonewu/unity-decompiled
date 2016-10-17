// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.SpriteStateDrawer
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>PropertyDrawer for SpriteState.</para>
  /// </summary>
  [CustomPropertyDrawer(typeof (SpriteState), true)]
  public class SpriteStateDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
    {
      Rect position = rect;
      position.height = EditorGUIUtility.singleLineHeight;
      SerializedProperty propertyRelative1 = prop.FindPropertyRelative("m_HighlightedSprite");
      SerializedProperty propertyRelative2 = prop.FindPropertyRelative("m_PressedSprite");
      SerializedProperty propertyRelative3 = prop.FindPropertyRelative("m_DisabledSprite");
      EditorGUI.PropertyField(position, propertyRelative1);
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.PropertyField(position, propertyRelative2);
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      EditorGUI.PropertyField(position, propertyRelative3);
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
      return (float) (3.0 * (double) EditorGUIUtility.singleLineHeight + 2.0 * (double) EditorGUIUtility.standardVerticalSpacing);
    }
  }
}
