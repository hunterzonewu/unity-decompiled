// Decompiled with JetBrains decompiler
// Type: UnityEditor.TargetChoiceHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class TargetChoiceHandler
  {
    internal static void DuplicateArrayElement(object userData)
    {
      SerializedProperty serializedProperty = (SerializedProperty) userData;
      serializedProperty.DuplicateCommand();
      serializedProperty.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    internal static void DeleteArrayElement(object userData)
    {
      SerializedProperty serializedProperty = (SerializedProperty) userData;
      serializedProperty.DeleteCommand();
      serializedProperty.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    internal static void SetPrefabOverride(object userData)
    {
      SerializedProperty serializedProperty = (SerializedProperty) userData;
      serializedProperty.prefabOverride = false;
      serializedProperty.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    internal static void SetToValueOfTarget(SerializedProperty property, Object target)
    {
      property.SetToValueOfTarget(target);
      property.serializedObject.ApplyModifiedProperties();
      EditorUtility.ForceReloadInspectors();
    }

    private static void TargetChoiceForwardFunction(object userData)
    {
      PropertyAndTargetHandler andTargetHandler = (PropertyAndTargetHandler) userData;
      andTargetHandler.function(andTargetHandler.property, andTargetHandler.target);
    }

    internal static void AddSetToValueOfTargetMenuItems(GenericMenu menu, SerializedProperty property, TargetChoiceHandler.TargetChoiceMenuFunction func)
    {
      SerializedProperty property1 = property.serializedObject.FindProperty(property.propertyPath);
      Object[] targetObjects = property.serializedObject.targetObjects;
      List<string> stringList = new List<string>();
      foreach (Object target in targetObjects)
      {
        string textAndTooltip = "Set to Value of " + target.name;
        if (stringList.Contains(textAndTooltip))
        {
          int num = 1;
          while (true)
          {
            textAndTooltip = "Set to Value of " + target.name + " (" + (object) num + ")";
            if (stringList.Contains(textAndTooltip))
              ++num;
            else
              break;
          }
        }
        stringList.Add(textAndTooltip);
        menu.AddItem(EditorGUIUtility.TextContent(textAndTooltip), false, new GenericMenu.MenuFunction2(TargetChoiceHandler.TargetChoiceForwardFunction), (object) new PropertyAndTargetHandler(property1, target, func));
      }
    }

    internal delegate void TargetChoiceMenuFunction(SerializedProperty property, Object target);
  }
}
