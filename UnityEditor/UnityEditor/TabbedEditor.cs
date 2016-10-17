// Decompiled with JetBrains decompiler
// Type: UnityEditor.TabbedEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal abstract class TabbedEditor : Editor
  {
    protected System.Type[] m_SubEditorTypes;
    protected string[] m_SubEditorNames;
    private int m_ActiveEditorIndex;
    private Editor m_ActiveEditor;

    public Editor activeEditor
    {
      get
      {
        return this.m_ActiveEditor;
      }
    }

    internal virtual void OnEnable()
    {
      this.m_ActiveEditorIndex = EditorPrefs.GetInt(this.GetType().Name + "ActiveEditorIndex", 0);
      if (!((UnityEngine.Object) this.m_ActiveEditor == (UnityEngine.Object) null))
        return;
      this.m_ActiveEditor = Editor.CreateEditor(this.targets, this.m_SubEditorTypes[this.m_ActiveEditorIndex]);
    }

    private void OnDestroy()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.activeEditor);
    }

    public override void OnInspectorGUI()
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      EditorGUI.BeginChangeCheck();
      this.m_ActiveEditorIndex = GUILayout.Toolbar(this.m_ActiveEditorIndex, this.m_SubEditorNames);
      if (EditorGUI.EndChangeCheck())
      {
        EditorPrefs.SetInt(this.GetType().Name + "ActiveEditorIndex", this.m_ActiveEditorIndex);
        Editor activeEditor = this.activeEditor;
        this.m_ActiveEditor = (Editor) null;
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) activeEditor);
        this.m_ActiveEditor = Editor.CreateEditor(this.targets, this.m_SubEditorTypes[this.m_ActiveEditorIndex]);
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      this.activeEditor.OnInspectorGUI();
    }

    public override void OnPreviewSettings()
    {
      this.activeEditor.OnPreviewSettings();
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      this.activeEditor.OnInteractivePreviewGUI(r, background);
    }

    public override bool HasPreviewGUI()
    {
      return this.activeEditor.HasPreviewGUI();
    }
  }
}
