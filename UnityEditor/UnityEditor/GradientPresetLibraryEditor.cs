// Decompiled with JetBrains decompiler
// Type: UnityEditor.GradientPresetLibraryEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (GradientPresetLibrary))]
  internal class GradientPresetLibraryEditor : Editor
  {
    private GenericPresetLibraryInspector<GradientPresetLibrary> m_GenericPresetLibraryInspector;

    public void OnEnable()
    {
      this.m_GenericPresetLibraryInspector = new GenericPresetLibraryInspector<GradientPresetLibrary>(this.target, "Gradient Preset Library", new System.Action<string>(this.OnEditButtonClicked));
      this.m_GenericPresetLibraryInspector.presetSize = new Vector2(72f, 16f);
      this.m_GenericPresetLibraryInspector.lineSpacing = 4f;
    }

    public void OnDestroy()
    {
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnDestroy();
    }

    public override void OnInspectorGUI()
    {
      this.m_GenericPresetLibraryInspector.itemViewMode = PresetLibraryEditorState.GetItemViewMode("Gradient");
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnInspectorGUI();
    }

    private void OnEditButtonClicked(string libraryPath)
    {
      GradientPicker.Show(new Gradient());
      GradientPicker.instance.currentPresetLibrary = libraryPath;
    }
  }
}
