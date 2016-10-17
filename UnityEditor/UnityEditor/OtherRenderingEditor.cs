// Decompiled with JetBrains decompiler
// Type: UnityEditor.OtherRenderingEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (RenderSettings))]
  internal class OtherRenderingEditor : Editor
  {
    private const string kShowEditorKey = "ShowOtherRenderingEditorFoldout";
    protected SerializedProperty m_HaloStrength;
    protected SerializedProperty m_FlareStrength;
    protected SerializedProperty m_FlareFadeSpeed;
    protected SerializedProperty m_HaloTexture;
    protected SerializedProperty m_SpotCookie;
    private bool m_ShowEditor;

    public virtual void OnEnable()
    {
      this.m_HaloStrength = this.serializedObject.FindProperty("m_HaloStrength");
      this.m_FlareStrength = this.serializedObject.FindProperty("m_FlareStrength");
      this.m_FlareFadeSpeed = this.serializedObject.FindProperty("m_FlareFadeSpeed");
      this.m_HaloTexture = this.serializedObject.FindProperty("m_HaloTexture");
      this.m_SpotCookie = this.serializedObject.FindProperty("m_SpotCookie");
      this.m_ShowEditor = SessionState.GetBool("ShowOtherRenderingEditorFoldout", false);
    }

    public virtual void OnDisable()
    {
      SessionState.SetBool("ShowOtherRenderingEditorFoldout", this.m_ShowEditor);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.Space();
      this.m_ShowEditor = EditorGUILayout.FoldoutTitlebar(this.m_ShowEditor, OtherRenderingEditor.Styles.otherHeader);
      if (!this.m_ShowEditor)
        return;
      ++EditorGUI.indentLevel;
      EditorGUILayout.PropertyField(this.m_HaloTexture);
      EditorGUILayout.Slider(this.m_HaloStrength, 0.0f, 1f);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_FlareFadeSpeed);
      EditorGUILayout.Slider(this.m_FlareStrength, 0.0f, 1f);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_SpotCookie);
      --EditorGUI.indentLevel;
      this.serializedObject.ApplyModifiedProperties();
    }

    internal class Styles
    {
      public static readonly GUIContent otherHeader = EditorGUIUtility.TextContent("Other Settings");
    }
  }
}
