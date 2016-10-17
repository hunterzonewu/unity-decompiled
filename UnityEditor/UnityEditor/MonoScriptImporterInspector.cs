// Decompiled with JetBrains decompiler
// Type: UnityEditor.MonoScriptImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (MonoImporter))]
  internal class MonoScriptImporterInspector : AssetImporterInspector
  {
    private const int m_RowHeight = 16;
    private static GUIContent s_HelpIcon;
    private static GUIContent s_TitleSettingsIcon;
    private SerializedObject m_TargetObject;
    private SerializedProperty m_Icon;

    internal override void OnHeaderControlsGUI()
    {
      TextAsset target = this.assetEditor.target as TextAsset;
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("Open...", EditorStyles.miniButton, new GUILayoutOption[0]))
      {
        AssetDatabase.OpenAsset((UnityEngine.Object) target);
        GUIUtility.ExitGUI();
      }
      if (!(bool) ((UnityEngine.Object) (target as MonoScript)) || !GUILayout.Button("Execution Order...", EditorStyles.miniButton, new GUILayoutOption[0]))
        return;
      EditorApplication.ExecuteMenuItem("Edit/Project Settings/Script Execution Order");
      GUIUtility.ExitGUI();
    }

    internal override void OnHeaderIconGUI(Rect iconRect)
    {
      if (this.m_Icon == null)
      {
        this.m_TargetObject = new SerializedObject(this.assetEditor.targets);
        this.m_Icon = this.m_TargetObject.FindProperty("m_Icon");
      }
      EditorGUI.ObjectIconDropDown(iconRect, this.assetEditor.targets, true, (Texture2D) null, this.m_Icon);
    }

    [MenuItem("CONTEXT/MonoImporter/Reset")]
    private static void ResetDefaultReferences(MenuCommand command)
    {
      MonoImporter context = command.context as MonoImporter;
      context.SetDefaultReferences(new string[0], new UnityEngine.Object[0]);
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((UnityEngine.Object) context));
    }

    private static bool IsTypeCompatible(System.Type type)
    {
      return type != null && (type.IsSubclassOf(typeof (MonoBehaviour)) || type.IsSubclassOf(typeof (ScriptableObject)));
    }

    private void ShowFieldInfo(System.Type type, MonoImporter importer, List<string> names, List<UnityEngine.Object> objects, ref bool didModify)
    {
      if (!MonoScriptImporterInspector.IsTypeCompatible(type))
        return;
      this.ShowFieldInfo(type.BaseType, importer, names, objects, ref didModify);
      foreach (System.Reflection.FieldInfo field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!field.IsPublic)
        {
          object[] customAttributes = field.GetCustomAttributes(typeof (SerializeField), true);
          if (customAttributes == null || customAttributes.Length == 0)
            continue;
        }
        if (field.FieldType.IsSubclassOf(typeof (UnityEngine.Object)) || field.FieldType == typeof (UnityEngine.Object))
        {
          UnityEngine.Object defaultReference = importer.GetDefaultReference(field.Name);
          UnityEngine.Object @object = EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(field.Name), defaultReference, field.FieldType, false, new GUILayoutOption[0]);
          names.Add(field.Name);
          objects.Add(@object);
          if (defaultReference != @object)
            didModify = true;
        }
      }
    }

    public override void OnInspectorGUI()
    {
      MonoImporter target = this.target as MonoImporter;
      MonoScript script = target.GetScript();
      if (!(bool) ((UnityEngine.Object) script))
        return;
      System.Type type = script.GetClass();
      if (!MonoScriptImporterInspector.IsTypeCompatible(type))
        EditorGUILayout.HelpBox("No MonoBehaviour scripts in the file, or their names do not match the file name.", MessageType.Info);
      Vector2 iconSize = EditorGUIUtility.GetIconSize();
      EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
      List<string> names = new List<string>();
      List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
      bool didModify = false;
      this.ShowFieldInfo(type, target, names, objects, ref didModify);
      EditorGUIUtility.SetIconSize(iconSize);
      if (!didModify)
        return;
      target.SetDefaultReferences(names.ToArray(), objects.ToArray());
      AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((UnityEngine.Object) target));
    }
  }
}
