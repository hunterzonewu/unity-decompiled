// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorPresetLibraryEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (ColorPresetLibrary))]
  internal class ColorPresetLibraryEditor : Editor
  {
    private GenericPresetLibraryInspector<ColorPresetLibrary> m_GenericPresetLibraryInspector;

    public void OnEnable()
    {
      this.m_GenericPresetLibraryInspector = new GenericPresetLibraryInspector<ColorPresetLibrary>(this.target, "Color Preset Library", new System.Action<string>(this.OnEditButtonClicked));
      this.m_GenericPresetLibraryInspector.useOnePixelOverlappedGrid = true;
      this.m_GenericPresetLibraryInspector.maxShowNumPresets = 2000;
    }

    public void OnDestroy()
    {
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnDestroy();
    }

    public override void OnInspectorGUI()
    {
      this.m_GenericPresetLibraryInspector.itemViewMode = PresetLibraryEditorState.GetItemViewMode(ColorPicker.presetsEditorPrefID);
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnInspectorGUI();
    }

    private void OnEditButtonClicked(string libraryPath)
    {
      ColorPicker.Show(GUIView.current, Color.white);
      ColorPicker.get.currentPresetLibrary = libraryPath;
    }
  }
}
