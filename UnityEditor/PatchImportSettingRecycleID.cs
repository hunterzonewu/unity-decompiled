// Decompiled with JetBrains decompiler
// Type: PatchImportSettingRecycleID
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor;

internal class PatchImportSettingRecycleID
{
  private const int kMaxObjectsPerClassID = 100000;

  public static void Patch(SerializedObject serializedObject, int classID, string oldName, string newName)
  {
    PatchImportSettingRecycleID.PatchMultiple(serializedObject, classID, new string[1]{ oldName }, new string[1]{ newName });
  }

  public static void PatchMultiple(SerializedObject serializedObject, int classID, string[] oldNames, string[] newNames)
  {
    int length = oldNames.Length;
    foreach (SerializedProperty serializedProperty in serializedObject.FindProperty("m_FileIDToRecycleName"))
    {
      SerializedProperty propertyRelative1 = serializedProperty.FindPropertyRelative("first");
      if (propertyRelative1.intValue >= 100000 * classID && propertyRelative1.intValue < 100000 * (classID + 1))
      {
        SerializedProperty propertyRelative2 = serializedProperty.FindPropertyRelative("second");
        int index = Array.IndexOf<string>(oldNames, propertyRelative2.stringValue);
        if (index >= 0)
        {
          propertyRelative2.stringValue = newNames[index];
          if (--length == 0)
            break;
        }
      }
    }
  }
}
