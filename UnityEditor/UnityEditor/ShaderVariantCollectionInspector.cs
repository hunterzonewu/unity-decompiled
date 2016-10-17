// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShaderVariantCollectionInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (ShaderVariantCollection))]
  internal class ShaderVariantCollectionInspector : Editor
  {
    private SerializedProperty m_Shaders;

    public virtual void OnEnable()
    {
      this.m_Shaders = this.serializedObject.FindProperty("m_Shaders");
    }

    private static Rect GetAddRemoveButtonRect(Rect r)
    {
      Vector2 vector2 = ShaderVariantCollectionInspector.Styles.invisibleButton.CalcSize(ShaderVariantCollectionInspector.Styles.iconRemove);
      return new Rect(r.xMax - vector2.x, r.y + (float) (int) ((double) r.height / 2.0 - (double) vector2.y / 2.0), vector2.x, vector2.y);
    }

    private void DisplayAddVariantsWindow(Shader shader, ShaderVariantCollection collection)
    {
      AddShaderVariantWindow.PopupData data = new AddShaderVariantWindow.PopupData();
      data.shader = shader;
      data.collection = collection;
      string[] keywords;
      ShaderUtil.GetShaderVariantEntries(shader, collection, out data.types, out keywords);
      if (keywords.Length == 0)
      {
        EditorApplication.Beep();
      }
      else
      {
        data.keywords = new string[keywords.Length][];
        for (int index = 0; index < keywords.Length; ++index)
          data.keywords[index] = keywords[index].Split(' ');
        AddShaderVariantWindow.ShowAddVariantWindow(data);
        GUIUtility.ExitGUI();
      }
    }

    private void DrawShaderEntry(int shaderIndex)
    {
      SerializedProperty arrayElementAtIndex1 = this.m_Shaders.GetArrayElementAtIndex(shaderIndex);
      Shader objectReferenceValue = (Shader) arrayElementAtIndex1.FindPropertyRelative("first").objectReferenceValue;
      SerializedProperty propertyRelative = arrayElementAtIndex1.FindPropertyRelative("second.variants");
      using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        Rect rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.boldLabel);
        Rect removeButtonRect = ShaderVariantCollectionInspector.GetAddRemoveButtonRect(rect);
        rect.xMax = removeButtonRect.x;
        GUI.Label(rect, objectReferenceValue.name, EditorStyles.boldLabel);
        if (GUI.Button(removeButtonRect, ShaderVariantCollectionInspector.Styles.iconRemove, ShaderVariantCollectionInspector.Styles.invisibleButton))
        {
          this.m_Shaders.DeleteArrayElementAtIndex(shaderIndex);
          return;
        }
      }
      for (int index = 0; index < propertyRelative.arraySize; ++index)
      {
        SerializedProperty arrayElementAtIndex2 = propertyRelative.GetArrayElementAtIndex(index);
        string str = arrayElementAtIndex2.FindPropertyRelative("keywords").stringValue;
        if (string.IsNullOrEmpty(str))
          str = "<no keywords>";
        PassType intValue = (PassType) arrayElementAtIndex2.FindPropertyRelative("passType").intValue;
        Rect rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniLabel);
        Rect removeButtonRect = ShaderVariantCollectionInspector.GetAddRemoveButtonRect(rect);
        rect.xMax = removeButtonRect.x;
        GUI.Label(rect, ((int) intValue).ToString() + " " + str, EditorStyles.miniLabel);
        if (GUI.Button(removeButtonRect, ShaderVariantCollectionInspector.Styles.iconRemove, ShaderVariantCollectionInspector.Styles.invisibleButton))
          propertyRelative.DeleteArrayElementAtIndex(index);
      }
      if (!GUI.Button(ShaderVariantCollectionInspector.GetAddRemoveButtonRect(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniLabel)), ShaderVariantCollectionInspector.Styles.iconAdd, ShaderVariantCollectionInspector.Styles.invisibleButton))
        return;
      this.DisplayAddVariantsWindow(objectReferenceValue, this.target as ShaderVariantCollection);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      for (int shaderIndex = 0; shaderIndex < this.m_Shaders.arraySize; ++shaderIndex)
        this.DrawShaderEntry(shaderIndex);
      if (GUILayout.Button("Add shader"))
      {
        ObjectSelector.get.Show((Object) null, typeof (Shader), (SerializedProperty) null, false);
        ObjectSelector.get.objectSelectorID = "ShaderVariantSelector".GetHashCode();
        GUIUtility.ExitGUI();
      }
      if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "ObjectSelectorClosed" && ObjectSelector.get.objectSelectorID == "ShaderVariantSelector".GetHashCode())
      {
        Shader currentObject = ObjectSelector.GetCurrentObject() as Shader;
        if ((Object) currentObject != (Object) null)
          ShaderUtil.AddNewShaderToCollection(currentObject, this.target as ShaderVariantCollection);
        Event.current.Use();
        GUIUtility.ExitGUI();
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    private class Styles
    {
      public static readonly GUIContent iconAdd = EditorGUIUtility.IconContent("Toolbar Plus", "Add variant");
      public static readonly GUIContent iconRemove = EditorGUIUtility.IconContent("Toolbar Minus", "Remove entry");
      public static readonly GUIStyle invisibleButton = (GUIStyle) "InvisibleButton";
    }
  }
}
