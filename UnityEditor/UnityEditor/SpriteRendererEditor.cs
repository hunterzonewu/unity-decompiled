// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteRendererEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (SpriteRenderer))]
  internal class SpriteRendererEditor : RendererEditorBase
  {
    private GUIContent m_FlipLabel = EditorGUIUtility.TextContent("Flip");
    private GUIContent m_MaterialStyle = EditorGUIUtility.TextContent("Material");
    private SerializedProperty m_Sprite;
    private SerializedProperty m_Color;
    private SerializedProperty m_Material;
    private SerializedProperty m_FlipX;
    private SerializedProperty m_FlipY;
    private static Texture2D s_WarningIcon;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Sprite = this.serializedObject.FindProperty("m_Sprite");
      this.m_Color = this.serializedObject.FindProperty("m_Color");
      this.m_FlipX = this.serializedObject.FindProperty("m_FlipX");
      this.m_FlipY = this.serializedObject.FindProperty("m_FlipY");
      this.m_Material = this.serializedObject.FindProperty("m_Materials.Array");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Sprite);
      EditorGUILayout.PropertyField(this.m_Color, true, new GUILayoutOption[0]);
      this.FlipToggles();
      if (this.m_Material.arraySize == 0)
        this.m_Material.InsertArrayElementAtIndex(0);
      Rect rect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f);
      EditorGUI.showMixedValue = this.m_Material.hasMultipleDifferentValues;
      Object objectReferenceValue = this.m_Material.GetArrayElementAtIndex(0).objectReferenceValue;
      Object @object = EditorGUI.ObjectField(rect, this.m_MaterialStyle, objectReferenceValue, typeof (Material), false);
      if (@object != objectReferenceValue)
        this.m_Material.GetArrayElementAtIndex(0).objectReferenceValue = @object;
      EditorGUI.showMixedValue = false;
      this.RenderSortingLayerFields();
      this.CheckForErrors();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void FlipToggles()
    {
      GUILayout.BeginHorizontal();
      Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.numberField);
      int controlId = GUIUtility.GetControlID(8476, FocusType.Keyboard, rect);
      Rect r = EditorGUI.PrefixLabel(rect, controlId, this.m_FlipLabel);
      r.width = 30f;
      this.FlipToggle(r, "X", this.m_FlipX);
      r.x += 30f;
      this.FlipToggle(r, "Y", this.m_FlipY);
      GUILayout.EndHorizontal();
    }

    private void FlipToggle(Rect r, string label, SerializedProperty property)
    {
      bool boolValue = property.boolValue;
      EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
      EditorGUI.BeginChangeCheck();
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      bool flag = EditorGUI.ToggleLeft(r, label, boolValue);
      EditorGUI.indentLevel = indentLevel;
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObjects(this.targets, "Edit Constraints");
        property.boolValue = flag;
      }
      EditorGUI.showMixedValue = false;
    }

    private void CheckForErrors()
    {
      if (this.IsMaterialTextureAtlasConflict())
        SpriteRendererEditor.ShowError("Material has CanUseSpriteAtlas=False tag. Sprite texture has atlasHint set. Rendering artifacts possible.");
      bool tiled;
      if (!this.DoesMaterialHaveSpriteTexture(out tiled))
      {
        SpriteRendererEditor.ShowError("Material does not have a _MainTex texture property. It is required for SpriteRenderer.");
      }
      else
      {
        if (!tiled)
          return;
        SpriteRendererEditor.ShowError("Material texture property _MainTex has offset/scale set. It is incompatible with SpriteRenderer.");
      }
    }

    private bool IsMaterialTextureAtlasConflict()
    {
      Material sharedMaterial = (this.target as SpriteRenderer).sharedMaterial;
      if ((Object) sharedMaterial == (Object) null || !(sharedMaterial.GetTag("CanUseSpriteAtlas", false).ToLower() == "false"))
        return false;
      TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((Object) (this.m_Sprite.objectReferenceValue as Sprite))) as TextureImporter;
      return (Object) atPath != (Object) null && atPath.spritePackingTag != null && atPath.spritePackingTag.Length > 0;
    }

    private bool DoesMaterialHaveSpriteTexture(out bool tiled)
    {
      tiled = false;
      Material sharedMaterial = (this.target as SpriteRenderer).sharedMaterial;
      if ((Object) sharedMaterial == (Object) null)
        return true;
      if (sharedMaterial.HasProperty("_MainTex"))
      {
        Vector2 textureOffset = sharedMaterial.GetTextureOffset("_MainTex");
        Vector2 textureScale = sharedMaterial.GetTextureScale("_MainTex");
        if ((double) textureOffset.x != 0.0 || (double) textureOffset.y != 0.0 || ((double) textureScale.x != 1.0 || (double) textureScale.y != 1.0))
          tiled = true;
      }
      return sharedMaterial.HasProperty("_MainTex");
    }

    private static void ShowError(string error)
    {
      if ((Object) SpriteRendererEditor.s_WarningIcon == (Object) null)
        SpriteRendererEditor.s_WarningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
      GUIContent content = new GUIContent(error) { image = (Texture) SpriteRendererEditor.s_WarningIcon };
      GUILayout.Space(5f);
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      GUILayout.Label(content, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
      GUILayout.EndVertical();
    }
  }
}
