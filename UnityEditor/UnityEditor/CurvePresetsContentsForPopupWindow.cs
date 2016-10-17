// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurvePresetsContentsForPopupWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class CurvePresetsContentsForPopupWindow : PopupWindowContent
  {
    private PresetLibraryEditor<CurvePresetLibrary> m_CurveLibraryEditor;
    private PresetLibraryEditorState m_CurveLibraryEditorState;
    private AnimationCurve m_Curve;
    private CurveLibraryType m_CurveLibraryType;
    private bool m_WantsToClose;
    private System.Action<AnimationCurve> m_PresetSelectedCallback;

    public AnimationCurve curveToSaveAsPreset
    {
      get
      {
        return this.m_Curve;
      }
      set
      {
        this.m_Curve = value;
      }
    }

    public string currentPresetLibrary
    {
      get
      {
        this.InitIfNeeded();
        return this.m_CurveLibraryEditor.currentLibraryWithoutExtension;
      }
      set
      {
        this.InitIfNeeded();
        this.m_CurveLibraryEditor.currentLibraryWithoutExtension = value;
      }
    }

    public CurvePresetsContentsForPopupWindow(AnimationCurve animCurve, CurveLibraryType curveLibraryType, System.Action<AnimationCurve> presetSelectedCallback)
    {
      this.m_CurveLibraryType = curveLibraryType;
      this.m_Curve = animCurve;
      this.m_PresetSelectedCallback = presetSelectedCallback;
    }

    public static string GetBasePrefText(CurveLibraryType curveLibraryType)
    {
      return CurvePresetsContentsForPopupWindow.GetExtension(curveLibraryType);
    }

    private static string GetExtension(CurveLibraryType curveLibraryType)
    {
      switch (curveLibraryType)
      {
        case CurveLibraryType.Unbounded:
          return PresetLibraryLocations.GetCurveLibraryExtension(false);
        case CurveLibraryType.NormalizedZeroToOne:
          return PresetLibraryLocations.GetCurveLibraryExtension(true);
        default:
          Debug.LogError((object) "Enum not handled!");
          return "curves";
      }
    }

    public override void OnClose()
    {
      this.m_CurveLibraryEditorState.TransferEditorPrefsState(false);
    }

    public PresetLibraryEditor<CurvePresetLibrary> GetPresetLibraryEditor()
    {
      return this.m_CurveLibraryEditor;
    }

    public void InitIfNeeded()
    {
      if (this.m_CurveLibraryEditorState == null)
      {
        this.m_CurveLibraryEditorState = new PresetLibraryEditorState(CurvePresetsContentsForPopupWindow.GetBasePrefText(this.m_CurveLibraryType));
        this.m_CurveLibraryEditorState.TransferEditorPrefsState(true);
      }
      if (this.m_CurveLibraryEditor != null)
        return;
      this.m_CurveLibraryEditor = new PresetLibraryEditor<CurvePresetLibrary>(new ScriptableObjectSaveLoadHelper<CurvePresetLibrary>(CurvePresetsContentsForPopupWindow.GetExtension(this.m_CurveLibraryType), SaveType.Text), this.m_CurveLibraryEditorState, new System.Action<int, object>(this.ItemClickedCallback));
      this.m_CurveLibraryEditor.addDefaultPresets += new System.Action<PresetLibrary>(this.AddDefaultPresetsToLibrary);
      this.m_CurveLibraryEditor.presetsWasReordered += new System.Action(this.OnPresetsWasReordered);
      this.m_CurveLibraryEditor.previewAspect = 4f;
      this.m_CurveLibraryEditor.minMaxPreviewHeight = new Vector2(24f, 24f);
      this.m_CurveLibraryEditor.showHeader = true;
    }

    private void OnPresetsWasReordered()
    {
      InternalEditorUtility.RepaintAllViews();
    }

    public override void OnGUI(Rect rect)
    {
      this.InitIfNeeded();
      this.m_CurveLibraryEditor.OnGUI(rect, (object) this.m_Curve);
      if (!this.m_WantsToClose)
        return;
      this.editorWindow.Close();
    }

    private void ItemClickedCallback(int clickCount, object presetObject)
    {
      AnimationCurve animationCurve = presetObject as AnimationCurve;
      if (animationCurve == null)
        Debug.LogError((object) ("Incorrect object passed " + presetObject));
      this.m_PresetSelectedCallback(animationCurve);
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(240f, 330f);
    }

    private void AddDefaultPresetsToLibrary(PresetLibrary presetLibrary)
    {
      CurvePresetLibrary curvePresetLibrary = presetLibrary as CurvePresetLibrary;
      if ((UnityEngine.Object) curvePresetLibrary == (UnityEngine.Object) null)
      {
        Debug.Log((object) ("Incorrect preset library, should be a CurvePresetLibrary but was a " + (object) presetLibrary.GetType()));
      }
      else
      {
        using (List<AnimationCurve>.Enumerator enumerator = new List<AnimationCurve>() { new AnimationCurve(CurveEditorWindow.GetConstantKeys(1f)), new AnimationCurve(CurveEditorWindow.GetLinearKeys()), new AnimationCurve(CurveEditorWindow.GetEaseInKeys()), new AnimationCurve(CurveEditorWindow.GetEaseOutKeys()), new AnimationCurve(CurveEditorWindow.GetEaseInOutKeys()) }.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AnimationCurve current = enumerator.Current;
            curvePresetLibrary.Add((object) current, string.Empty);
          }
        }
      }
    }
  }
}
