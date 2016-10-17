// Decompiled with JetBrains decompiler
// Type: ParticleSystemCurveEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

internal class ParticleSystemCurveEditor
{
  private static CurveEditorSettings m_CurveEditorSettings = new CurveEditorSettings();
  private int m_LastTopMostCurveID = -1;
  public const float k_PresetsHeight = 30f;
  private List<ParticleSystemCurveEditor.CurveData> m_AddedCurves;
  private CurveEditor m_CurveEditor;
  private Color[] m_Colors;
  private List<Color> m_AvailableColors;
  private DoubleCurvePresetsContentsForPopupWindow m_DoubleCurvePresets;
  internal static ParticleSystemCurveEditor.Styles s_Styles;

  public void OnDisable()
  {
    this.m_CurveEditor.OnDisable();
    Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
  }

  public void OnDestroy()
  {
    this.m_DoubleCurvePresets.GetPresetLibraryEditor().UnloadUsedLibraries();
  }

  public void Refresh()
  {
    this.ContentChanged();
    AnimationCurvePreviewCache.ClearCache();
  }

  public void Init()
  {
    if (this.m_AddedCurves != null)
      return;
    this.m_AddedCurves = new List<ParticleSystemCurveEditor.CurveData>();
    this.m_Colors = new Color[6]
    {
      new Color(1f, 0.6196079f, 0.1294118f),
      new Color(0.8745098f, 0.2117647f, 0.5803922f),
      new Color(0.0f, 0.6862745f, 1f),
      new Color(1f, 0.9215686f, 0.0f),
      new Color(0.1960784f, 1f, 0.2666667f),
      new Color(0.9803922f, 0.0f, 0.0f)
    };
    this.m_AvailableColors = new List<Color>((IEnumerable<Color>) this.m_Colors);
    ParticleSystemCurveEditor.m_CurveEditorSettings.useFocusColors = true;
    ParticleSystemCurveEditor.m_CurveEditorSettings.showAxisLabels = false;
    ParticleSystemCurveEditor.m_CurveEditorSettings.hRangeMin = 0.0f;
    ParticleSystemCurveEditor.m_CurveEditorSettings.vRangeMin = 0.0f;
    ParticleSystemCurveEditor.m_CurveEditorSettings.vRangeMax = 1f;
    ParticleSystemCurveEditor.m_CurveEditorSettings.hRangeMax = 1f;
    ParticleSystemCurveEditor.m_CurveEditorSettings.vSlider = false;
    ParticleSystemCurveEditor.m_CurveEditorSettings.hSlider = false;
    ParticleSystemCurveEditor.m_CurveEditorSettings.wrapColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    ParticleSystemCurveEditor.m_CurveEditorSettings.hTickLabelOffset = 5f;
    ParticleSystemCurveEditor.m_CurveEditorSettings.allowDraggingCurvesAndRegions = true;
    ParticleSystemCurveEditor.m_CurveEditorSettings.allowDeleteLastKeyInCurve = false;
    ParticleSystemCurveEditor.m_CurveEditorSettings.hTickStyle = new TickStyle()
    {
      color = new Color(0.0f, 0.0f, 0.0f, 0.2f),
      distLabel = 30,
      stubs = false,
      centerLabel = true
    };
    ParticleSystemCurveEditor.m_CurveEditorSettings.vTickStyle = new TickStyle()
    {
      color = new Color(0.0f, 0.0f, 0.0f, 0.2f),
      distLabel = 20,
      stubs = false,
      centerLabel = true
    };
    this.m_CurveEditor = new CurveEditor(new Rect(0.0f, 0.0f, 1000f, 100f), this.CreateCurveWrapperArray(), false);
    this.m_CurveEditor.settings = ParticleSystemCurveEditor.m_CurveEditorSettings;
    this.m_CurveEditor.leftmargin = 40f;
    CurveEditor curveEditor = this.m_CurveEditor;
    float num1 = 25f;
    this.m_CurveEditor.bottommargin = num1;
    float num2 = num1;
    this.m_CurveEditor.topmargin = num2;
    double num3 = (double) num2;
    curveEditor.rightmargin = (float) num3;
    this.m_CurveEditor.SetShownHRangeInsideMargins(ParticleSystemCurveEditor.m_CurveEditorSettings.hRangeMin, ParticleSystemCurveEditor.m_CurveEditorSettings.hRangeMax);
    this.m_CurveEditor.SetShownVRangeInsideMargins(ParticleSystemCurveEditor.m_CurveEditorSettings.vRangeMin, ParticleSystemCurveEditor.m_CurveEditorSettings.hRangeMax);
    this.m_CurveEditor.ignoreScrollWheelUntilClicked = false;
    Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
  }

  private void UndoRedoPerformed()
  {
    this.ContentChanged();
  }

  private void UpdateRangeBasedOnShownCurves()
  {
    bool flag = false;
    for (int index = 0; index < this.m_AddedCurves.Count; ++index)
      flag |= this.m_AddedCurves[index].m_SignedRange;
    float num = !flag ? 0.0f : -1f;
    if ((double) num == (double) ParticleSystemCurveEditor.m_CurveEditorSettings.vRangeMin)
      return;
    ParticleSystemCurveEditor.m_CurveEditorSettings.vRangeMin = num;
    this.m_CurveEditor.settings = ParticleSystemCurveEditor.m_CurveEditorSettings;
    this.m_CurveEditor.SetShownVRangeInsideMargins(ParticleSystemCurveEditor.m_CurveEditorSettings.vRangeMin, ParticleSystemCurveEditor.m_CurveEditorSettings.hRangeMax);
  }

  public bool IsAdded(SerializedProperty min, SerializedProperty max)
  {
    return this.FindIndex(min, max) != -1;
  }

  public bool IsAdded(SerializedProperty max)
  {
    return this.FindIndex((SerializedProperty) null, max) != -1;
  }

  public void AddCurve(ParticleSystemCurveEditor.CurveData curveData)
  {
    this.Add(curveData);
  }

  public void RemoveCurve(SerializedProperty max)
  {
    this.RemoveCurve((SerializedProperty) null, max);
  }

  public void RemoveCurve(SerializedProperty min, SerializedProperty max)
  {
    if (!this.Remove(this.FindIndex(min, max)))
      return;
    this.ContentChanged();
    this.UpdateRangeBasedOnShownCurves();
  }

  public Color GetCurveColor(SerializedProperty max)
  {
    int index = this.FindIndex(max);
    if (index >= 0 && index < this.m_AddedCurves.Count)
      return this.m_AddedCurves[index].m_Color;
    return new Color(0.8f, 0.8f, 0.8f, 0.7f);
  }

  public void AddCurveDataIfNeeded(string curveName, ParticleSystemCurveEditor.CurveData curveData)
  {
    Vector3 vector3 = SessionState.GetVector3(curveName, Vector3.zero);
    if (!(vector3 != Vector3.zero))
      return;
    Color c2 = new Color(vector3.x, vector3.y, vector3.z);
    curveData.m_Color = c2;
    this.AddCurve(curveData);
    for (int index = 0; index < this.m_AvailableColors.Count; ++index)
    {
      if (ParticleSystemCurveEditor.SameColor(this.m_AvailableColors[index], c2))
      {
        this.m_AvailableColors.RemoveAt(index);
        break;
      }
    }
  }

  public void SetVisible(SerializedProperty curveProp, bool visible)
  {
    int index = this.FindIndex(curveProp);
    if (index < 0)
      return;
    this.m_AddedCurves[index].m_Visible = visible;
  }

  private static bool SameColor(Color c1, Color c2)
  {
    if ((double) Mathf.Abs(c1.r - c2.r) < 0.00999999977648258 && (double) Mathf.Abs(c1.g - c2.g) < 0.00999999977648258)
      return (double) Mathf.Abs(c1.b - c2.b) < 0.00999999977648258;
    return false;
  }

  private int FindIndex(SerializedProperty prop)
  {
    return this.FindIndex((SerializedProperty) null, prop);
  }

  private int FindIndex(SerializedProperty min, SerializedProperty max)
  {
    if (max == null)
      return -1;
    if (min == null)
    {
      for (int index = 0; index < this.m_AddedCurves.Count; ++index)
      {
        if (this.m_AddedCurves[index].m_Max == max)
          return index;
      }
    }
    else
    {
      for (int index = 0; index < this.m_AddedCurves.Count; ++index)
      {
        if (this.m_AddedCurves[index].m_Max == max && this.m_AddedCurves[index].m_Min == min)
          return index;
      }
    }
    return -1;
  }

  private void Add(ParticleSystemCurveEditor.CurveData cd)
  {
    this.m_CurveEditor.SelectNone();
    this.m_AddedCurves.Add(cd);
    this.ContentChanged();
    SessionState.SetVector3(cd.m_UniqueName, new Vector3(cd.m_Color.r, cd.m_Color.g, cd.m_Color.b));
    this.UpdateRangeBasedOnShownCurves();
  }

  private bool Remove(int index)
  {
    if (index >= 0 && index < this.m_AddedCurves.Count)
    {
      this.m_AvailableColors.Add(this.m_AddedCurves[index].m_Color);
      SessionState.EraseVector3(this.m_AddedCurves[index].m_UniqueName);
      this.m_AddedCurves.RemoveAt(index);
      if (this.m_AddedCurves.Count == 0)
        this.m_AvailableColors = new List<Color>((IEnumerable<Color>) this.m_Colors);
      return true;
    }
    Debug.Log((object) "Invalid index in ParticleSystemCurveEditor::Remove");
    return false;
  }

  private void RemoveTopMost()
  {
    int curveID;
    if (!this.m_CurveEditor.GetTopMostCurveID(out curveID))
      return;
    for (int index = 0; index < this.m_AddedCurves.Count; ++index)
    {
      ParticleSystemCurveEditor.CurveData addedCurve = this.m_AddedCurves[index];
      if (addedCurve.m_MaxId == curveID || addedCurve.m_MinId == curveID)
      {
        this.Remove(index);
        this.ContentChanged();
        this.UpdateRangeBasedOnShownCurves();
        break;
      }
    }
  }

  private void RemoveSelected()
  {
    bool flag = false;
    List<CurveSelection> selectedCurves = this.m_CurveEditor.selectedCurves;
    for (int index1 = 0; index1 < selectedCurves.Count; ++index1)
    {
      int curveId = selectedCurves[index1].curveID;
      for (int index2 = 0; index2 < this.m_AddedCurves.Count; ++index2)
      {
        ParticleSystemCurveEditor.CurveData addedCurve = this.m_AddedCurves[index2];
        if (addedCurve.m_MaxId == curveId || addedCurve.m_MinId == curveId)
        {
          flag |= this.Remove(index2);
          break;
        }
      }
    }
    if (flag)
    {
      this.ContentChanged();
      this.UpdateRangeBasedOnShownCurves();
    }
    this.m_CurveEditor.SelectNone();
  }

  private void RemoveAll()
  {
    bool flag = false;
    while (this.m_AddedCurves.Count > 0)
      flag |= this.Remove(0);
    if (!flag)
      return;
    this.ContentChanged();
    this.UpdateRangeBasedOnShownCurves();
  }

  public Color GetAvailableColor()
  {
    if (this.m_AvailableColors.Count == 0)
      this.m_AvailableColors = new List<Color>((IEnumerable<Color>) this.m_Colors);
    int index = this.m_AvailableColors.Count - 1;
    Color availableColor = this.m_AvailableColors[index];
    this.m_AvailableColors.RemoveAt(index);
    return availableColor;
  }

  public void OnGUI(Rect rect)
  {
    this.Init();
    if (ParticleSystemCurveEditor.s_Styles == null)
      ParticleSystemCurveEditor.s_Styles = new ParticleSystemCurveEditor.Styles();
    Rect position = new Rect(rect.x, rect.y, rect.width, rect.height - 30f);
    Rect rect1 = new Rect(rect.x, rect.y + position.height, rect.width, 30f);
    GUI.Label(position, GUIContent.none, ParticleSystemCurveEditor.s_Styles.curveEditorBackground);
    this.m_CurveEditor.rect = position;
    this.m_CurveEditor.OnGUI();
    foreach (CurveWrapper animationCurve in this.m_CurveEditor.animationCurves)
    {
      if (animationCurve.getAxisUiScalarsCallback != null && animationCurve.setAxisUiScalarsCallback != null)
      {
        Vector2 newAxisScalars = animationCurve.getAxisUiScalarsCallback();
        if ((double) newAxisScalars.y > 1000000.0)
        {
          newAxisScalars.y = 1000000f;
          animationCurve.setAxisUiScalarsCallback(newAxisScalars);
        }
      }
    }
    this.DoLabelForTopMostCurve(new Rect(rect.x + 4f, rect.y, rect.width, 20f));
    this.DoRemoveSelectedButton(new Rect(position.x, position.y, position.width, 24f));
    this.DoOptimizeCurveButton(rect1);
    rect1.x += 30f;
    rect1.width -= 60f;
    this.PresetCurveButtons(rect1, rect);
    this.SaveChangedCurves();
  }

  private void DoLabelForTopMostCurve(Rect rect)
  {
    int curveID;
    if (!this.m_CurveEditor.IsDraggingCurveOrRegion() && this.m_CurveEditor.selectedCurves.Count > 1 || !this.m_CurveEditor.GetTopMostCurveID(out curveID))
      return;
    for (int index = 0; index < this.m_AddedCurves.Count; ++index)
    {
      if (this.m_AddedCurves[index].m_MaxId == curveID || this.m_AddedCurves[index].m_MinId == curveID)
      {
        ParticleSystemCurveEditor.s_Styles.yAxisHeader.normal.textColor = this.m_AddedCurves[index].m_Color;
        GUI.Label(rect, this.m_AddedCurves[index].m_DisplayName, ParticleSystemCurveEditor.s_Styles.yAxisHeader);
        break;
      }
    }
  }

  private void SetConstantCurve(CurveWrapper cw, float constantValue)
  {
    Keyframe[] keyframeArray = new Keyframe[1];
    keyframeArray[0].time = 0.0f;
    keyframeArray[0].value = constantValue;
    cw.curve.keys = keyframeArray;
    cw.changed = true;
  }

  private void SetCurve(CurveWrapper cw, AnimationCurve curve)
  {
    Keyframe[] keyframeArray = new Keyframe[curve.keys.Length];
    Array.Copy((Array) curve.keys, (Array) keyframeArray, keyframeArray.Length);
    cw.curve.keys = keyframeArray;
    cw.changed = true;
  }

  private void SetTopMostCurve(DoubleCurve doubleCurve)
  {
    int curveID;
    if (!this.m_CurveEditor.GetTopMostCurveID(out curveID))
      return;
    for (int index = 0; index < this.m_AddedCurves.Count; ++index)
    {
      ParticleSystemCurveEditor.CurveData addedCurve = this.m_AddedCurves[index];
      if (addedCurve.m_MaxId == curveID || addedCurve.m_MinId == curveID)
      {
        if (doubleCurve.signedRange == addedCurve.m_SignedRange)
        {
          if (addedCurve.m_MaxId > 0)
            this.SetCurve(this.m_CurveEditor.GetCurveFromID(addedCurve.m_MaxId), doubleCurve.maxCurve);
          if (addedCurve.m_MinId > 0)
            this.SetCurve(this.m_CurveEditor.GetCurveFromID(addedCurve.m_MinId), doubleCurve.minCurve);
        }
        else
          Debug.LogWarning((object) "Cannot assign a curves with different signed range");
      }
    }
  }

  private DoubleCurve CreateDoubleCurveFromTopMostCurve()
  {
    int curveID;
    if (this.m_CurveEditor.GetTopMostCurveID(out curveID))
    {
      for (int index = 0; index < this.m_AddedCurves.Count; ++index)
      {
        ParticleSystemCurveEditor.CurveData addedCurve = this.m_AddedCurves[index];
        if (addedCurve.m_MaxId == curveID || addedCurve.m_MinId == curveID)
        {
          AnimationCurve maxCurve = (AnimationCurve) null;
          AnimationCurve minCurve = (AnimationCurve) null;
          if (addedCurve.m_Min != null)
            minCurve = addedCurve.m_Min.animationCurveValue;
          if (addedCurve.m_Max != null)
            maxCurve = addedCurve.m_Max.animationCurveValue;
          return new DoubleCurve(minCurve, maxCurve, addedCurve.m_SignedRange);
        }
      }
    }
    return (DoubleCurve) null;
  }

  private void PresetDropDown(Rect rect)
  {
    if (!EditorGUI.ButtonMouseDown(rect, EditorGUI.GUIContents.titleSettingsIcon, FocusType.Native, EditorStyles.inspectorTitlebarText) || this.CreateDoubleCurveFromTopMostCurve() == null)
      return;
    this.InitDoubleCurvePresets();
    if (this.m_DoubleCurvePresets == null)
      return;
    this.m_DoubleCurvePresets.doubleCurveToSave = this.CreateDoubleCurveFromTopMostCurve();
    PopupWindow.Show(rect, (PopupWindowContent) this.m_DoubleCurvePresets);
  }

  private void InitDoubleCurvePresets()
  {
    int curveID;
    if (!this.m_CurveEditor.GetTopMostCurveID(out curveID) || this.m_DoubleCurvePresets != null && this.m_LastTopMostCurveID == curveID)
      return;
    this.m_LastTopMostCurveID = curveID;
    this.m_DoubleCurvePresets = new DoubleCurvePresetsContentsForPopupWindow(this.CreateDoubleCurveFromTopMostCurve(), (System.Action<DoubleCurve>) (presetDoubleCurve =>
    {
      this.SetTopMostCurve(presetDoubleCurve);
      InternalEditorUtility.RepaintAllViews();
    }));
    this.m_DoubleCurvePresets.InitIfNeeded();
  }

  private void PresetCurveButtons(Rect position, Rect curveEditorRect)
  {
    if (this.m_CurveEditor.animationCurves.Length == 0)
      return;
    this.InitDoubleCurvePresets();
    if (this.m_DoubleCurvePresets == null)
      return;
    DoubleCurvePresetLibrary currentLib = this.m_DoubleCurvePresets.GetPresetLibraryEditor().GetCurrentLib();
    int num1 = Mathf.Min(!((UnityEngine.Object) currentLib != (UnityEngine.Object) null) ? 0 : currentLib.Count(), 9);
    float width = 30f;
    float height = 15f;
    float num2 = 10f;
    float num3 = (float) ((double) num1 * (double) width + (double) (num1 - 1) * (double) num2);
    float num4 = (float) (((double) position.width - (double) num3) * 0.5);
    float y = (float) (((double) position.height - (double) height) * 0.5);
    float x = 3f;
    if ((double) num4 > 0.0)
      x = num4;
    this.PresetDropDown(new Rect(x - 20f + position.x, y + position.y, 16f, 16f));
    GUI.BeginGroup(position);
    Color.white.a *= 0.6f;
    for (int index = 0; index < num1; ++index)
    {
      if (index > 0)
        x += num2;
      Rect rect = new Rect(x, y, width, height);
      ParticleSystemCurveEditor.s_Styles.presetTooltip.tooltip = currentLib.GetName(index);
      if (GUI.Button(rect, ParticleSystemCurveEditor.s_Styles.presetTooltip, GUIStyle.none))
      {
        DoubleCurve preset = currentLib.GetPreset(index) as DoubleCurve;
        if (preset != null)
        {
          this.SetTopMostCurve(preset);
          this.m_CurveEditor.ClearSelection();
        }
      }
      if (Event.current.type == EventType.Repaint)
        currentLib.Draw(rect, index);
      x += width;
    }
    GUI.EndGroup();
  }

  private void DoOptimizeCurveButton(Rect rect)
  {
    if (this.m_CurveEditor.IsDraggingCurveOrRegion())
      return;
    Rect position = new Rect((float) ((double) rect.xMax - 10.0 - 14.0), rect.y + (float) (((double) rect.height - 14.0) * 0.5), 14f, 14f);
    int num = 0;
    List<CurveSelection> selectedCurves = this.m_CurveEditor.selectedCurves;
    if (selectedCurves.Count > 0)
    {
      for (int index = 0; index < selectedCurves.Count; ++index)
      {
        CurveWrapper curveWrapper = selectedCurves[index].curveWrapper;
        num += !AnimationUtility.IsValidPolynomialCurve(curveWrapper.curve) ? 0 : 1;
      }
      if (selectedCurves.Count == num || !GUI.Button(position, ParticleSystemCurveEditor.s_Styles.optimizeCurveText, ParticleSystemCurveEditor.s_Styles.plus))
        return;
      for (int index = 0; index < selectedCurves.Count; ++index)
      {
        CurveWrapper curveWrapper = selectedCurves[index].curveWrapper;
        if (!AnimationUtility.IsValidPolynomialCurve(curveWrapper.curve))
        {
          AnimationUtility.ConstrainToPolynomialCurve(curveWrapper.curve);
          curveWrapper.changed = true;
        }
      }
      this.m_CurveEditor.SelectNone();
    }
    else
    {
      int curveID;
      if (!this.m_CurveEditor.GetTopMostCurveID(out curveID))
        return;
      CurveWrapper curveWrapperById = this.m_CurveEditor.getCurveWrapperById(curveID);
      if (AnimationUtility.IsValidPolynomialCurve(curveWrapperById.curve) || !GUI.Button(position, ParticleSystemCurveEditor.s_Styles.optimizeCurveText, ParticleSystemCurveEditor.s_Styles.plus))
        return;
      AnimationUtility.ConstrainToPolynomialCurve(curveWrapperById.curve);
      curveWrapperById.changed = true;
    }
  }

  private void DoRemoveSelectedButton(Rect rect)
  {
    if (this.m_CurveEditor.animationCurves.Length == 0)
      return;
    float num = 14f;
    if (!GUI.Button(new Rect((float) ((double) rect.x + (double) rect.width - (double) num - 10.0), rect.y + (float) (((double) rect.height - (double) num) * 0.5), num, num), ParticleSystemCurveEditor.s_Styles.removeCurveText, ParticleSystemCurveEditor.s_Styles.minus))
      return;
    if (this.m_CurveEditor.selectedCurves.Count > 0)
      this.RemoveSelected();
    else
      this.RemoveTopMost();
  }

  private void SaveCurve(SerializedProperty prop, CurveWrapper cw)
  {
    prop.animationCurveValue = cw.curve;
    cw.changed = false;
  }

  private void SaveChangedCurves()
  {
    CurveWrapper[] animationCurves = this.m_CurveEditor.animationCurves;
    bool flag = false;
    for (int index1 = 0; index1 < animationCurves.Length; ++index1)
    {
      CurveWrapper cw = animationCurves[index1];
      if (cw.changed)
      {
        for (int index2 = 0; index2 < this.m_AddedCurves.Count; ++index2)
        {
          if (this.m_AddedCurves[index2].m_MaxId == cw.id)
          {
            this.SaveCurve(this.m_AddedCurves[index2].m_Max, cw);
            break;
          }
          if (this.m_AddedCurves[index2].IsRegion() && this.m_AddedCurves[index2].m_MinId == cw.id)
          {
            this.SaveCurve(this.m_AddedCurves[index2].m_Min, cw);
            break;
          }
        }
        flag = true;
      }
    }
    if (!flag)
      return;
    AnimationCurvePreviewCache.ClearCache();
    HandleUtility.Repaint();
  }

  private CurveWrapper CreateCurveWrapper(SerializedProperty curve, int id, int regionId, Color color, bool signedRange, CurveWrapper.GetAxisScalarsCallback getAxisScalarsCallback, CurveWrapper.SetAxisScalarsCallback setAxisScalarsCallback)
  {
    float end = 1f;
    CurveWrapper curveWrapper = new CurveWrapper();
    curveWrapper.id = id;
    curveWrapper.regionId = regionId;
    curveWrapper.color = color;
    curveWrapper.renderer = (CurveRenderer) new NormalCurveRenderer(curve.animationCurveValue);
    curveWrapper.renderer.SetCustomRange(0.0f, end);
    curveWrapper.vRangeMin = !signedRange ? 0.0f : -1f;
    curveWrapper.getAxisUiScalarsCallback = getAxisScalarsCallback;
    curveWrapper.setAxisUiScalarsCallback = setAxisScalarsCallback;
    return curveWrapper;
  }

  private CurveWrapper[] CreateCurveWrapperArray()
  {
    List<CurveWrapper> curveWrapperList = new List<CurveWrapper>();
    int num = 0;
    for (int index = 0; index < this.m_AddedCurves.Count; ++index)
    {
      ParticleSystemCurveEditor.CurveData addedCurve = this.m_AddedCurves[index];
      if (addedCurve.m_Visible)
      {
        int regionId = -1;
        if (addedCurve.IsRegion())
          regionId = ++num;
        if (addedCurve.m_Max != null)
          curveWrapperList.Add(this.CreateCurveWrapper(addedCurve.m_Max, addedCurve.m_MaxId, regionId, addedCurve.m_Color, addedCurve.m_SignedRange, addedCurve.m_GetAxisScalarsCallback, addedCurve.m_SetAxisScalarsCallback));
        if (addedCurve.m_Min != null)
          curveWrapperList.Add(this.CreateCurveWrapper(addedCurve.m_Min, addedCurve.m_MinId, regionId, addedCurve.m_Color, addedCurve.m_SignedRange, addedCurve.m_GetAxisScalarsCallback, addedCurve.m_SetAxisScalarsCallback));
      }
    }
    return curveWrapperList.ToArray();
  }

  private void ContentChanged()
  {
    this.m_CurveEditor.animationCurves = this.CreateCurveWrapperArray();
    ParticleSystemCurveEditor.m_CurveEditorSettings.showAxisLabels = this.m_CurveEditor.animationCurves.Length > 0;
  }

  internal class Styles
  {
    public GUIStyle curveEditorBackground = (GUIStyle) "AnimationCurveEditorBackground";
    public GUIStyle curveSwatch = (GUIStyle) "PopupCurveEditorSwatch";
    public GUIStyle curveSwatchArea = (GUIStyle) "PopupCurveSwatchBackground";
    public GUIStyle minus = (GUIStyle) "OL Minus";
    public GUIStyle plus = (GUIStyle) "OL Plus";
    public GUIStyle yAxisHeader = new GUIStyle(ParticleSystemStyles.Get().label);
    public GUIContent optimizeCurveText = new GUIContent(string.Empty, "Click to optimize curve. Optimized curves are defined by having at most 3 keys, with a key at both ends");
    public GUIContent removeCurveText = new GUIContent(string.Empty, "Remove selected curve(s)");
    public GUIContent curveLibraryPopup = new GUIContent(string.Empty, "Open curve library");
    public GUIContent presetTooltip = new GUIContent();
  }

  public class CurveData
  {
    public SerializedProperty m_Max;
    public SerializedProperty m_Min;
    public bool m_SignedRange;
    public Color m_Color;
    public string m_UniqueName;
    public GUIContent m_DisplayName;
    public CurveWrapper.GetAxisScalarsCallback m_GetAxisScalarsCallback;
    public CurveWrapper.SetAxisScalarsCallback m_SetAxisScalarsCallback;
    public int m_MaxId;
    public int m_MinId;
    public bool m_Visible;
    private static int s_IdCounter;

    public CurveData(string name, GUIContent displayName, SerializedProperty min, SerializedProperty max, Color color, bool signedRange, CurveWrapper.GetAxisScalarsCallback getAxisScalars, CurveWrapper.SetAxisScalarsCallback setAxisScalars, bool visible)
    {
      this.m_UniqueName = name;
      this.m_DisplayName = displayName;
      this.m_SignedRange = signedRange;
      this.m_Min = min;
      this.m_Max = max;
      if (this.m_Min != null)
        this.m_MinId = ++ParticleSystemCurveEditor.CurveData.s_IdCounter;
      if (this.m_Max != null)
        this.m_MaxId = ++ParticleSystemCurveEditor.CurveData.s_IdCounter;
      this.m_Color = color;
      this.m_GetAxisScalarsCallback = getAxisScalars;
      this.m_SetAxisScalarsCallback = setAxisScalars;
      this.m_Visible = visible;
      if (this.m_Max != null && this.m_MaxId != 0)
        return;
      Debug.LogError((object) "Max curve should always be valid! (Min curve can be null)");
    }

    public bool IsRegion()
    {
      return this.m_Min != null;
    }
  }
}
