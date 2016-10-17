// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurvePresetLibraryEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (CurvePresetLibrary))]
  internal class CurvePresetLibraryEditor : Editor
  {
    private GenericPresetLibraryInspector<CurvePresetLibrary> m_GenericPresetLibraryInspector;
    private CurveLibraryType m_CurveLibraryType;

    public void OnEnable()
    {
      this.m_CurveLibraryType = this.GetCurveLibraryTypeFromExtension(Path.GetExtension(AssetDatabase.GetAssetPath(this.target.GetInstanceID())).TrimStart('.'));
      this.m_GenericPresetLibraryInspector = new GenericPresetLibraryInspector<CurvePresetLibrary>(this.target, this.GetHeader(), new System.Action<string>(this.OnEditButtonClicked));
      this.m_GenericPresetLibraryInspector.presetSize = new Vector2(72f, 20f);
      this.m_GenericPresetLibraryInspector.lineSpacing = 5f;
    }

    public void OnDestroy()
    {
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnDestroy();
    }

    public override void OnInspectorGUI()
    {
      this.m_GenericPresetLibraryInspector.itemViewMode = PresetLibraryEditorState.GetItemViewMode(CurvePresetsContentsForPopupWindow.GetBasePrefText(this.m_CurveLibraryType));
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnInspectorGUI();
    }

    private void OnEditButtonClicked(string libraryPath)
    {
      Rect curveRanges = this.GetCurveRanges();
      CurveEditorSettings settings = new CurveEditorSettings();
      if ((double) curveRanges.width > 0.0 && (double) curveRanges.height > 0.0 && ((double) curveRanges.width != double.PositiveInfinity && (double) curveRanges.height != double.PositiveInfinity))
      {
        settings.hRangeMin = curveRanges.xMin;
        settings.hRangeMax = curveRanges.xMax;
        settings.vRangeMin = curveRanges.yMin;
        settings.vRangeMax = curveRanges.yMax;
      }
      CurveEditorWindow.curve = new AnimationCurve();
      CurveEditorWindow.color = new Color(0.0f, 0.8f, 0.0f);
      CurveEditorWindow.instance.Show(GUIView.current, settings);
      CurveEditorWindow.instance.currentPresetLibrary = libraryPath;
    }

    private string GetHeader()
    {
      switch (this.m_CurveLibraryType)
      {
        case CurveLibraryType.Unbounded:
          return "Curve Preset Library";
        case CurveLibraryType.NormalizedZeroToOne:
          return "Curve Preset Library (Normalized 0 - 1)";
        default:
          return "Curve Preset Library ?";
      }
    }

    private Rect GetCurveRanges()
    {
      switch (this.m_CurveLibraryType)
      {
        case CurveLibraryType.Unbounded:
          return new Rect();
        case CurveLibraryType.NormalizedZeroToOne:
          return new Rect(0.0f, 0.0f, 1f, 1f);
        default:
          return new Rect();
      }
    }

    private CurveLibraryType GetCurveLibraryTypeFromExtension(string extension)
    {
      string libraryExtension1 = PresetLibraryLocations.GetCurveLibraryExtension(true);
      string libraryExtension2 = PresetLibraryLocations.GetCurveLibraryExtension(false);
      if (extension.Equals(libraryExtension1, StringComparison.OrdinalIgnoreCase))
        return CurveLibraryType.NormalizedZeroToOne;
      if (extension.Equals(libraryExtension2, StringComparison.OrdinalIgnoreCase))
        return CurveLibraryType.Unbounded;
      Debug.LogError((object) "Extension not recognized!");
      return CurveLibraryType.NormalizedZeroToOne;
    }
  }
}
