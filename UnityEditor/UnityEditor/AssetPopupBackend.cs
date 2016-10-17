// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetPopupBackend
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetPopupBackend
  {
    public static void AssetPopup<T>(SerializedProperty serializedProperty, GUIContent label, string fileExtension) where T : Object, new()
    {
      bool showMixedValue = EditorGUI.showMixedValue;
      EditorGUI.showMixedValue = serializedProperty.hasMultipleDifferentValues;
      string referenceTypeString = serializedProperty.objectReferenceTypeString;
      GUIContent buttonContent = !serializedProperty.hasMultipleDifferentValues ? (!(serializedProperty.objectReferenceValue != (Object) null) ? GUIContent.Temp("Default") : GUIContent.Temp(serializedProperty.objectReferenceStringValue)) : EditorGUI.mixedValueContent;
      Rect buttonRect;
      if (AudioMixerEffectGUI.PopupButton(label, buttonContent, EditorStyles.popup, out buttonRect))
        AssetPopupBackend.ShowAssetsPopupMenu<T>(buttonRect, referenceTypeString, serializedProperty, fileExtension);
      EditorGUI.showMixedValue = showMixedValue;
    }

    private static void ShowAssetsPopupMenu<T>(Rect buttonRect, string typeName, SerializedProperty serializedProperty, string fileExtension) where T : Object, new()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AssetPopupBackend.\u003CShowAssetsPopupMenu\u003Ec__AnonStorey76<T> menuCAnonStorey76 = new AssetPopupBackend.\u003CShowAssetsPopupMenu\u003Ec__AnonStorey76<T>();
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey76.typeName = typeName;
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey76.fileExtension = fileExtension;
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey76.serializedProperty = serializedProperty;
      GenericMenu genericMenu = new GenericMenu();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num = !(menuCAnonStorey76.serializedProperty.objectReferenceValue != (Object) null) ? 0 : menuCAnonStorey76.serializedProperty.objectReferenceValue.GetInstanceID();
      // ISSUE: reference to a compiler-generated field
      genericMenu.AddItem(new GUIContent("Default"), (num == 0 ? 1 : 0) != 0, new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback), (object) new object[2]
      {
        (object) 0,
        (object) menuCAnonStorey76.serializedProperty
      });
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      // ISSUE: reference to a compiler-generated field
      SearchFilter filter = new SearchFilter() { classNames = new string[1]{ menuCAnonStorey76.typeName } };
      hierarchyProperty.SetSearchFilter(filter);
      hierarchyProperty.Reset();
      while (hierarchyProperty.Next((int[]) null))
      {
        // ISSUE: reference to a compiler-generated field
        genericMenu.AddItem(new GUIContent(hierarchyProperty.name), (hierarchyProperty.instanceID == num ? 1 : 0) != 0, new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback), (object) new object[2]
        {
          (object) hierarchyProperty.instanceID,
          (object) menuCAnonStorey76.serializedProperty
        });
      }
      // ISSUE: reference to a compiler-generated field
      int classId = BaseObjectTools.StringToClassID(menuCAnonStorey76.typeName);
      if (classId > 0)
      {
        foreach (BuiltinResource builtinResource in EditorGUIUtility.GetBuiltinResourceList(classId))
        {
          // ISSUE: reference to a compiler-generated field
          genericMenu.AddItem(new GUIContent(builtinResource.m_Name), (builtinResource.m_InstanceID == num ? 1 : 0) != 0, new GenericMenu.MenuFunction2(AssetPopupBackend.AssetPopupMenuCallback), (object) new object[2]
          {
            (object) builtinResource.m_InstanceID,
            (object) menuCAnonStorey76.serializedProperty
          });
        }
      }
      genericMenu.AddSeparator(string.Empty);
      // ISSUE: reference to a compiler-generated method
      genericMenu.AddItem(new GUIContent("Create New..."), false, new GenericMenu.MenuFunction(menuCAnonStorey76.\u003C\u003Em__10A));
      genericMenu.DropDown(buttonRect);
    }

    private static void AssetPopupMenuCallback(object userData)
    {
      object[] objArray = userData as object[];
      int instanceID = (int) objArray[0];
      SerializedProperty serializedProperty = (SerializedProperty) objArray[1];
      serializedProperty.objectReferenceValue = EditorUtility.InstanceIDToObject(instanceID);
      serializedProperty.m_SerializedObject.ApplyModifiedProperties();
    }
  }
}
