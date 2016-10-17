// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorWrapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal class EditorWrapper : IDisposable
  {
    private Editor editor;
    public EditorWrapper.VoidDelegate OnSceneDrag;

    public string name
    {
      get
      {
        return this.editor.target.name;
      }
    }

    private EditorWrapper()
    {
    }

    ~EditorWrapper()
    {
      Debug.LogError((object) "Failed to dispose EditorWrapper.");
    }

    public bool HasPreviewGUI()
    {
      return this.editor.HasPreviewGUI();
    }

    public void OnPreviewSettings()
    {
      this.editor.OnPreviewSettings();
    }

    public void OnPreviewGUI(Rect position, GUIStyle background)
    {
      this.editor.OnPreviewGUI(position, background);
    }

    public void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      if (!((UnityEngine.Object) this.editor != (UnityEngine.Object) null))
        return;
      this.editor.OnInteractivePreviewGUI(r, background);
    }

    internal void OnAssetStoreInspectorGUI()
    {
      if (!((UnityEngine.Object) this.editor != (UnityEngine.Object) null))
        return;
      this.editor.OnAssetStoreInspectorGUI();
    }

    public string GetInfoString()
    {
      return this.editor.GetInfoString();
    }

    public static EditorWrapper Make(UnityEngine.Object obj, EditorFeatures requirements)
    {
      EditorWrapper editorWrapper = new EditorWrapper();
      if (editorWrapper.Init(obj, requirements))
        return editorWrapper;
      editorWrapper.Dispose();
      return (EditorWrapper) null;
    }

    private bool Init(UnityEngine.Object obj, EditorFeatures requirements)
    {
      this.editor = Editor.CreateEditor(obj);
      if ((UnityEngine.Object) this.editor == (UnityEngine.Object) null || (requirements & EditorFeatures.PreviewGUI) > EditorFeatures.None && !this.editor.HasPreviewGUI())
        return false;
      MethodInfo method = this.editor.GetType().GetMethod("OnSceneDrag", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (method != null)
      {
        this.OnSceneDrag = (EditorWrapper.VoidDelegate) Delegate.CreateDelegate(typeof (EditorWrapper.VoidDelegate), (object) this.editor, method);
      }
      else
      {
        if ((requirements & EditorFeatures.OnSceneDrag) > EditorFeatures.None)
          return false;
        this.OnSceneDrag = new EditorWrapper.VoidDelegate(this.DefaultOnSceneDrag);
      }
      return true;
    }

    private void DefaultOnSceneDrag(SceneView sceneView)
    {
    }

    public void Dispose()
    {
      if ((UnityEngine.Object) this.editor != (UnityEngine.Object) null)
      {
        this.OnSceneDrag = (EditorWrapper.VoidDelegate) null;
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.editor);
        this.editor = (Editor) null;
      }
      GC.SuppressFinalize((object) this);
    }

    public delegate void VoidDelegate(SceneView sceneView);
  }
}
