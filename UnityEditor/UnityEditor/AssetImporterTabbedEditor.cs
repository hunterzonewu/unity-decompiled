// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetImporterTabbedEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal abstract class AssetImporterTabbedEditor : AssetImporterInspector
  {
    protected System.Type[] m_SubEditorTypes;
    protected string[] m_SubEditorNames;
    private int m_ActiveEditorIndex;
    private AssetImporterInspector m_ActiveEditor;

    public AssetImporterInspector activeEditor
    {
      get
      {
        return this.m_ActiveEditor;
      }
    }

    internal override Editor assetEditor
    {
      get
      {
        return base.assetEditor;
      }
      set
      {
        base.assetEditor = value;
        if (!(bool) ((UnityEngine.Object) this.activeEditor))
          return;
        this.activeEditor.assetEditor = this.assetEditor;
      }
    }

    internal virtual void OnEnable()
    {
      this.m_ActiveEditorIndex = EditorPrefs.GetInt(this.GetType().Name + "ActiveEditorIndex", 0);
      if (!((UnityEngine.Object) this.m_ActiveEditor == (UnityEngine.Object) null))
        return;
      this.m_ActiveEditor = Editor.CreateEditor(this.targets, this.m_SubEditorTypes[this.m_ActiveEditorIndex]) as AssetImporterInspector;
    }

    private void OnDestroy()
    {
      AssetImporterInspector activeEditor = this.activeEditor;
      if (!((UnityEngine.Object) activeEditor != (UnityEngine.Object) null))
        return;
      this.m_ActiveEditor = (AssetImporterInspector) null;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) activeEditor);
    }

    public override void OnInspectorGUI()
    {
      EditorGUI.BeginDisabledGroup(false);
      GUI.enabled = true;
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      EditorGUI.BeginChangeCheck();
      this.m_ActiveEditorIndex = GUILayout.Toolbar(this.m_ActiveEditorIndex, this.m_SubEditorNames);
      if (EditorGUI.EndChangeCheck())
      {
        EditorPrefs.SetInt(this.GetType().Name + "ActiveEditorIndex", this.m_ActiveEditorIndex);
        AssetImporterInspector activeEditor = this.activeEditor;
        this.m_ActiveEditor = (AssetImporterInspector) null;
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) activeEditor);
        this.m_ActiveEditor = Editor.CreateEditor(this.targets, this.m_SubEditorTypes[this.m_ActiveEditorIndex]) as AssetImporterInspector;
        this.m_ActiveEditor.assetEditor = this.assetEditor;
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      EditorGUI.EndDisabledGroup();
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
      if ((UnityEngine.Object) this.activeEditor == (UnityEngine.Object) null)
        return false;
      return this.activeEditor.HasPreviewGUI();
    }
  }
}
