// Decompiled with JetBrains decompiler
// Type: UnityEditor.FogEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (RenderSettings))]
  internal class FogEditor : Editor
  {
    private const string kShowEditorKey = "ShowFogEditorFoldout";
    protected SerializedProperty m_Fog;
    protected SerializedProperty m_FogColor;
    protected SerializedProperty m_FogMode;
    protected SerializedProperty m_FogDensity;
    protected SerializedProperty m_LinearFogStart;
    protected SerializedProperty m_LinearFogEnd;
    private bool m_ShowEditor;

    public virtual void OnEnable()
    {
      this.m_Fog = this.serializedObject.FindProperty("m_Fog");
      this.m_FogColor = this.serializedObject.FindProperty("m_FogColor");
      this.m_FogMode = this.serializedObject.FindProperty("m_FogMode");
      this.m_FogDensity = this.serializedObject.FindProperty("m_FogDensity");
      this.m_LinearFogStart = this.serializedObject.FindProperty("m_LinearFogStart");
      this.m_LinearFogEnd = this.serializedObject.FindProperty("m_LinearFogEnd");
      this.m_ShowEditor = SessionState.GetBool("ShowFogEditorFoldout", false);
    }

    public virtual void OnDisable()
    {
      SessionState.SetBool("ShowFogEditorFoldout", this.m_ShowEditor);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.m_ShowEditor = EditorGUILayout.ToggleTitlebar(this.m_ShowEditor, FogEditor.Styles.fogHeader, this.m_Fog);
      if (this.m_ShowEditor)
      {
        ++EditorGUI.indentLevel;
        EditorGUI.BeginDisabledGroup(!this.m_Fog.boolValue);
        EditorGUILayout.PropertyField(this.m_FogColor);
        EditorGUILayout.PropertyField(this.m_FogMode);
        ++EditorGUI.indentLevel;
        if (this.m_FogMode.intValue != 1)
        {
          EditorGUILayout.PropertyField(this.m_FogDensity, FogEditor.Styles.fogDensity, new GUILayoutOption[0]);
        }
        else
        {
          EditorGUILayout.PropertyField(this.m_LinearFogStart, FogEditor.Styles.fogLinearStart, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LinearFogEnd, FogEditor.Styles.fogLinearEnd, new GUILayoutOption[0]);
        }
        --EditorGUI.indentLevel;
        if (SceneView.IsUsingDeferredRenderingPath())
          EditorGUILayout.HelpBox(FogEditor.Styles.fogWarning.text, MessageType.Info);
        EditorGUILayout.EndFadeGroup();
        EditorGUI.EndDisabledGroup();
        --EditorGUI.indentLevel;
      }
      this.serializedObject.ApplyModifiedProperties();
    }

    internal class Styles
    {
      public static readonly GUIContent fogHeader = EditorGUIUtility.TextContent("Fog");
      public static readonly GUIContent fogWarning = EditorGUIUtility.TextContent("Fog does not affect opaque objects in Deferred Shading. Use Global Fog image effect.");
      public static readonly GUIContent fogDensity = EditorGUIUtility.TextContent("Density");
      public static readonly GUIContent fogLinearStart = EditorGUIUtility.TextContent("Start");
      public static readonly GUIContent fogLinearEnd = EditorGUIUtility.TextContent("End");
    }
  }
}
