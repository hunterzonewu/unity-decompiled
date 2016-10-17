// Decompiled with JetBrains decompiler
// Type: UnityEditor.StructPropertyGUILayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class StructPropertyGUILayout
  {
    internal static void JointSpring(SerializedProperty property, params GUILayoutOption[] options)
    {
      StructPropertyGUILayout.GenericStruct(property, options);
    }

    internal static void WheelFrictionCurve(SerializedProperty property, params GUILayoutOption[] options)
    {
      StructPropertyGUILayout.GenericStruct(property, options);
    }

    internal static void GenericStruct(SerializedProperty property, params GUILayoutOption[] options)
    {
      float num = (float) (16.0 + 16.0 * (double) StructPropertyGUILayout.GetChildrenCount(property));
      StructPropertyGUI.GenericStruct(GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, num, num, EditorStyles.layerMaskField, options), property);
    }

    internal static int GetChildrenCount(SerializedProperty property)
    {
      int depth = property.depth;
      SerializedProperty serializedProperty = property.Copy();
      serializedProperty.NextVisible(true);
      int num = 0;
      while (serializedProperty.depth == depth + 1)
      {
        ++num;
        serializedProperty.NextVisible(false);
      }
      return num;
    }
  }
}
