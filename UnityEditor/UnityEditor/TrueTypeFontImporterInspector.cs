// Decompiled with JetBrains decompiler
// Type: UnityEditor.TrueTypeFontImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (TrueTypeFontImporter))]
  internal class TrueTypeFontImporterInspector : AssetImporterInspector
  {
    private static GUIContent[] kCharacterStrings = new GUIContent[6]{ new GUIContent("Dynamic"), new GUIContent("Unicode"), new GUIContent("ASCII default set"), new GUIContent("ASCII upper case"), new GUIContent("ASCII lower case"), new GUIContent("Custom set") };
    private static int[] kCharacterValues = new int[6]{ -2, -1, 0, 1, 2, 3 };
    private static GUIContent[] kRenderingModeStrings = new GUIContent[4]{ new GUIContent("Smooth"), new GUIContent("Hinted Smooth"), new GUIContent("Hinted Raster"), new GUIContent("OS Default") };
    private static int[] kRenderingModeValues = new int[4]{ 0, 1, 2, 3 };
    private static GUIContent[] kAscentCalculationModeStrings = new GUIContent[3]{ new GUIContent("Legacy version 2 mode (glyph bounding boxes)"), new GUIContent("Face ascender metric"), new GUIContent("Face bounding box metric") };
    private static int[] kAscentCalculationModeValues = new int[3]{ 0, 1, 2 };
    private string m_FontNamesString = string.Empty;
    private string m_DefaultFontNamesString = string.Empty;
    private SerializedProperty m_FontSize;
    private SerializedProperty m_TextureCase;
    private SerializedProperty m_IncludeFontData;
    private SerializedProperty m_FontNamesArraySize;
    private SerializedProperty m_FallbackFontReferencesArraySize;
    private SerializedProperty m_CustomCharacters;
    private SerializedProperty m_FontRenderingMode;
    private SerializedProperty m_AscentCalculationMode;
    private bool? m_FormatSupported;
    private bool m_ReferencesExpanded;

    private void OnEnable()
    {
      this.m_FontSize = this.serializedObject.FindProperty("m_FontSize");
      this.m_TextureCase = this.serializedObject.FindProperty("m_ForceTextureCase");
      this.m_IncludeFontData = this.serializedObject.FindProperty("m_IncludeFontData");
      this.m_FontNamesArraySize = this.serializedObject.FindProperty("m_FontNames.Array.size");
      this.m_CustomCharacters = this.serializedObject.FindProperty("m_CustomCharacters");
      this.m_FontRenderingMode = this.serializedObject.FindProperty("m_FontRenderingMode");
      this.m_AscentCalculationMode = this.serializedObject.FindProperty("m_AscentCalculationMode");
      this.m_FallbackFontReferencesArraySize = this.serializedObject.FindProperty("m_FallbackFontReferences.Array.size");
      if (this.targets.Length != 1)
        return;
      this.m_DefaultFontNamesString = this.GetDefaultFontNames();
      this.m_FontNamesString = this.GetFontNames();
      this.SetFontNames(this.m_FontNamesString);
    }

    private string GetDefaultFontNames()
    {
      return (this.target as TrueTypeFontImporter).fontTTFName;
    }

    private string GetFontNames()
    {
      TrueTypeFontImporter target = this.target as TrueTypeFontImporter;
      string str = string.Empty;
      string[] fontNames = target.fontNames;
      for (int index = 0; index < fontNames.Length; ++index)
      {
        str += fontNames[index];
        if (index < fontNames.Length - 1)
          str += ", ";
      }
      if (str == string.Empty)
        str = this.m_DefaultFontNamesString;
      return str;
    }

    private void SetFontNames(string fontNames)
    {
      string[] _names;
      if (fontNames == this.m_DefaultFontNamesString)
      {
        _names = new string[0];
      }
      else
      {
        _names = fontNames.Split(',');
        for (int index = 0; index < _names.Length; ++index)
          _names[index] = _names[index].Trim();
      }
      this.m_FontNamesArraySize.intValue = _names.Length;
      SerializedProperty serializedProperty1 = this.m_FontNamesArraySize.Copy();
      for (int index = 0; index < _names.Length; ++index)
      {
        serializedProperty1.Next(false);
        serializedProperty1.stringValue = _names[index];
      }
      Font[] fontArray = (this.target as TrueTypeFontImporter).LookupFallbackFontReferences(_names);
      this.m_FallbackFontReferencesArraySize.intValue = fontArray.Length;
      SerializedProperty serializedProperty2 = this.m_FallbackFontReferencesArraySize.Copy();
      for (int index = 0; index < fontArray.Length; ++index)
      {
        serializedProperty2.Next(false);
        serializedProperty2.objectReferenceValue = (Object) fontArray[index];
      }
    }

    private void ShowFormatUnsupportedGUI()
    {
      GUILayout.Space(5f);
      EditorGUILayout.HelpBox("Format of selected font is not supported by Unity.", MessageType.Warning);
    }

    private static string GetUniquePath(string basePath, string extension)
    {
      for (int index = 0; index < 10000; ++index)
      {
        string path = basePath + (index != 0 ? string.Empty + (object) index : string.Empty) + "." + extension;
        if (!File.Exists(path))
          return path;
      }
      return string.Empty;
    }

    [MenuItem("CONTEXT/TrueTypeFontImporter/Create Editable Copy")]
    private static void CreateEditableCopy(MenuCommand command)
    {
      TrueTypeFontImporter context = command.context as TrueTypeFontImporter;
      if (context.fontTextureCase == FontTextureCase.Dynamic)
      {
        EditorUtility.DisplayDialog("Cannot generate editabled font asset for dynamic fonts", "Please reimport the font in a different mode.", "Ok");
      }
      else
      {
        string str = Path.GetDirectoryName(context.assetPath) + "/" + Path.GetFileNameWithoutExtension(context.assetPath);
        EditorGUIUtility.PingObject((Object) context.GenerateEditableFont(TrueTypeFontImporterInspector.GetUniquePath(str + "_copy", "fontsettings")));
      }
    }

    public override void OnInspectorGUI()
    {
      if (!this.m_FormatSupported.HasValue)
      {
        this.m_FormatSupported = new bool?(true);
        foreach (Object target in this.targets)
        {
          TrueTypeFontImporter typeFontImporter = target as TrueTypeFontImporter;
          if ((Object) typeFontImporter == (Object) null || !typeFontImporter.IsFormatSupported())
            this.m_FormatSupported = new bool?(false);
        }
      }
      bool? formatSupported = this.m_FormatSupported;
      if ((formatSupported.GetValueOrDefault() ? 0 : (formatSupported.HasValue ? 1 : 0)) != 0)
      {
        this.ShowFormatUnsupportedGUI();
      }
      else
      {
        EditorGUILayout.PropertyField(this.m_FontSize);
        if (this.m_FontSize.intValue < 1)
          this.m_FontSize.intValue = 1;
        if (this.m_FontSize.intValue > 500)
          this.m_FontSize.intValue = 500;
        EditorGUILayout.IntPopup(this.m_FontRenderingMode, TrueTypeFontImporterInspector.kRenderingModeStrings, TrueTypeFontImporterInspector.kRenderingModeValues, new GUIContent("Rendering Mode"), new GUILayoutOption[0]);
        EditorGUILayout.IntPopup(this.m_TextureCase, TrueTypeFontImporterInspector.kCharacterStrings, TrueTypeFontImporterInspector.kCharacterValues, new GUIContent("Character"), new GUILayoutOption[0]);
        EditorGUILayout.IntPopup(this.m_AscentCalculationMode, TrueTypeFontImporterInspector.kAscentCalculationModeStrings, TrueTypeFontImporterInspector.kAscentCalculationModeValues, new GUIContent("Ascent Calculation Mode"), new GUILayoutOption[0]);
        if (!this.m_TextureCase.hasMultipleDifferentValues)
        {
          if (this.m_TextureCase.intValue != -2)
          {
            if (this.m_TextureCase.intValue == 3)
            {
              EditorGUI.BeginChangeCheck();
              GUILayout.BeginHorizontal();
              EditorGUILayout.PrefixLabel("Custom Chars");
              EditorGUI.showMixedValue = this.m_CustomCharacters.hasMultipleDifferentValues;
              string source = EditorGUILayout.TextArea(this.m_CustomCharacters.stringValue, GUI.skin.textArea, GUILayout.MinHeight(32f));
              EditorGUI.showMixedValue = false;
              GUILayout.EndHorizontal();
              if (EditorGUI.EndChangeCheck())
                this.m_CustomCharacters.stringValue = new string(source.Distinct<char>().ToArray<char>()).Replace("\n", string.Empty).Replace("\r", string.Empty);
            }
          }
          else
          {
            EditorGUILayout.PropertyField(this.m_IncludeFontData, new GUIContent("Incl. Font Data"), new GUILayoutOption[0]);
            if (this.targets.Length == 1)
            {
              EditorGUI.BeginChangeCheck();
              GUILayout.BeginHorizontal();
              EditorGUILayout.PrefixLabel("Font Names");
              GUI.SetNextControlName("fontnames");
              this.m_FontNamesString = EditorGUILayout.TextArea(this.m_FontNamesString, (GUIStyle) "TextArea", GUILayout.MinHeight(32f));
              GUILayout.EndHorizontal();
              GUILayout.BeginHorizontal();
              GUILayout.FlexibleSpace();
              EditorGUI.BeginDisabledGroup(this.m_FontNamesString == this.m_DefaultFontNamesString);
              if (GUILayout.Button("Reset", (GUIStyle) "MiniButton", new GUILayoutOption[0]))
              {
                GUI.changed = true;
                if (GUI.GetNameOfFocusedControl() == "fontnames")
                  GUIUtility.keyboardControl = 0;
                this.m_FontNamesString = this.m_DefaultFontNamesString;
              }
              EditorGUI.EndDisabledGroup();
              GUILayout.EndHorizontal();
              if (EditorGUI.EndChangeCheck())
                this.SetFontNames(this.m_FontNamesString);
              this.m_ReferencesExpanded = EditorGUILayout.Foldout(this.m_ReferencesExpanded, "References to other fonts in project");
              if (this.m_ReferencesExpanded)
              {
                EditorGUILayout.HelpBox("These are automatically generated by the inspector if any of the font names you supplied match fonts present in your project, which will then be used as fallbacks for this font.", MessageType.Info);
                if (this.m_FallbackFontReferencesArraySize.intValue > 0)
                {
                  SerializedProperty property = this.m_FallbackFontReferencesArraySize.Copy();
                  while (property.NextVisible(true) && property.depth == 1)
                    EditorGUILayout.PropertyField(property, true, new GUILayoutOption[0]);
                }
                else
                {
                  EditorGUI.BeginDisabledGroup(true);
                  GUILayout.Label("No references to other fonts in project.");
                  EditorGUI.EndDisabledGroup();
                }
              }
            }
          }
        }
        this.ApplyRevertGUI();
      }
    }
  }
}
