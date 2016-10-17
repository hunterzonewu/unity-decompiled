// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.NavigationDrawer
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>PropertyDrawer for Navigation.</para>
  /// </summary>
  [CustomPropertyDrawer(typeof (Navigation), true)]
  public class NavigationDrawer : PropertyDrawer
  {
    private static NavigationDrawer.Styles s_Styles;

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
      if (NavigationDrawer.s_Styles == null)
        NavigationDrawer.s_Styles = new NavigationDrawer.Styles();
      Rect position = pos;
      position.height = EditorGUIUtility.singleLineHeight;
      SerializedProperty propertyRelative1 = prop.FindPropertyRelative("m_Mode");
      Navigation.Mode navigationMode = NavigationDrawer.GetNavigationMode(propertyRelative1);
      EditorGUI.PropertyField(position, propertyRelative1, NavigationDrawer.s_Styles.navigationContent);
      ++EditorGUI.indentLevel;
      position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      if (navigationMode == Navigation.Mode.Explicit)
      {
        SerializedProperty propertyRelative2 = prop.FindPropertyRelative("m_SelectOnUp");
        SerializedProperty propertyRelative3 = prop.FindPropertyRelative("m_SelectOnDown");
        SerializedProperty propertyRelative4 = prop.FindPropertyRelative("m_SelectOnLeft");
        SerializedProperty propertyRelative5 = prop.FindPropertyRelative("m_SelectOnRight");
        EditorGUI.PropertyField(position, propertyRelative2);
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, propertyRelative3);
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, propertyRelative4);
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, propertyRelative5);
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      }
      --EditorGUI.indentLevel;
    }

    private static Navigation.Mode GetNavigationMode(SerializedProperty navigation)
    {
      return (Navigation.Mode) navigation.enumValueIndex;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
      SerializedProperty propertyRelative = prop.FindPropertyRelative("m_Mode");
      if (propertyRelative == null)
        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      switch (NavigationDrawer.GetNavigationMode(propertyRelative))
      {
        case Navigation.Mode.None:
          return EditorGUIUtility.singleLineHeight;
        case Navigation.Mode.Explicit:
          return (float) (5.0 * (double) EditorGUIUtility.singleLineHeight + 5.0 * (double) EditorGUIUtility.standardVerticalSpacing);
        default:
          return EditorGUIUtility.singleLineHeight + 1f * EditorGUIUtility.standardVerticalSpacing;
      }
    }

    private class Styles
    {
      public readonly GUIContent navigationContent;

      public Styles()
      {
        this.navigationContent = new GUIContent("Navigation");
      }
    }
  }
}
