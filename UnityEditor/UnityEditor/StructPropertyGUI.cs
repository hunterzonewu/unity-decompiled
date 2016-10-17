// Decompiled with JetBrains decompiler
// Type: UnityEditor.StructPropertyGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class StructPropertyGUI
  {
    internal static void JointSpring(Rect position, SerializedProperty property)
    {
      StructPropertyGUI.GenericStruct(position, property);
    }

    internal static void WheelFrictionCurve(Rect position, SerializedProperty property)
    {
      StructPropertyGUI.GenericStruct(position, property);
    }

    internal static void GenericStruct(Rect position, SerializedProperty property)
    {
      GUI.Label(EditorGUI.IndentedRect(position), property.displayName, EditorStyles.label);
      position.y += 16f;
      StructPropertyGUI.DoChildren(position, property);
    }

    private static void DoChildren(Rect position, SerializedProperty property)
    {
      float depth = (float) property.depth;
      position.height = 16f;
      ++EditorGUI.indentLevel;
      SerializedProperty property1 = property.Copy();
      property1.NextVisible(true);
      while ((double) property1.depth == (double) depth + 1.0)
      {
        EditorGUI.PropertyField(position, property1);
        position.y += 16f;
        property1.NextVisible(false);
      }
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
    }
  }
}
