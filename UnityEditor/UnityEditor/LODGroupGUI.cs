// Decompiled with JetBrains decompiler
// Type: UnityEditor.LODGroupGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal static class LODGroupGUI
  {
    public static readonly Color[] kLODColors = new Color[8]{ new Color(0.4831376f, 0.6211768f, 0.0219608f, 1f), new Color(0.279216f, 0.4078432f, 0.5835296f, 1f), new Color(0.2070592f, 0.5333336f, 0.6556864f, 1f), new Color(0.5333336f, 0.16f, 0.0282352f, 1f), new Color(0.3827448f, 0.2886272f, 0.5239216f, 1f), new Color(0.8f, 0.4423528f, 0.0f, 1f), new Color(0.4486272f, 0.4078432f, 0.050196f, 1f), new Color(0.7749016f, 0.6368624f, 0.0250984f, 1f) };
    public static readonly Color kCulledLODColor = new Color(0.4f, 0.0f, 0.0f, 1f);
    public const int kSceneLabelHalfWidth = 100;
    public const int kSceneLabelHeight = 45;
    public const int kSceneHeaderOffset = 40;
    public const int kSliderBarTopMargin = 18;
    public const int kSliderBarHeight = 30;
    public const int kSliderBarBottomMargin = 16;
    public const int kRenderersButtonHeight = 60;
    public const int kButtonPadding = 2;
    public const int kDeleteButtonSize = 20;
    public const int kSelectedLODRangePadding = 3;
    public const int kRenderAreaForegroundPadding = 3;
    private static LODGroupGUI.GUIStyles s_Styles;

    public static LODGroupGUI.GUIStyles Styles
    {
      get
      {
        if (LODGroupGUI.s_Styles == null)
          LODGroupGUI.s_Styles = new LODGroupGUI.GUIStyles();
        return LODGroupGUI.s_Styles;
      }
    }

    public static float DelinearizeScreenPercentage(float percentage)
    {
      if (Mathf.Approximately(0.0f, percentage))
        return 0.0f;
      return Mathf.Sqrt(percentage);
    }

    public static float LinearizeScreenPercentage(float percentage)
    {
      return percentage * percentage;
    }

    public static Rect CalcLODButton(Rect totalRect, float percentage)
    {
      return new Rect((float) ((double) totalRect.x + (double) Mathf.Round(totalRect.width * (1f - percentage)) - 5.0), totalRect.y, 10f, totalRect.height);
    }

    public static Rect GetCulledBox(Rect totalRect, float previousLODPercentage)
    {
      Rect rect = LODGroupGUI.CalcLODRange(totalRect, previousLODPercentage, 0.0f);
      rect.height -= 2f;
      --rect.width;
      rect.center += new Vector2(0.0f, 1f);
      return rect;
    }

    public static List<LODGroupGUI.LODInfo> CreateLODInfos(int numLODs, Rect area, Func<int, string> nameGen, Func<int, float> heightGen)
    {
      List<LODGroupGUI.LODInfo> lodInfoList = new List<LODGroupGUI.LODInfo>();
      for (int lodLevel = 0; lodLevel < numLODs; ++lodLevel)
      {
        LODGroupGUI.LODInfo lodInfo = new LODGroupGUI.LODInfo(lodLevel, nameGen(lodLevel), heightGen(lodLevel));
        lodInfo.m_ButtonPosition = LODGroupGUI.CalcLODButton(area, lodInfo.ScreenPercent);
        float startPercent = lodLevel != 0 ? lodInfoList[lodLevel - 1].ScreenPercent : 1f;
        lodInfo.m_RangePosition = LODGroupGUI.CalcLODRange(area, startPercent, lodInfo.ScreenPercent);
        lodInfoList.Add(lodInfo);
      }
      return lodInfoList;
    }

    public static void SetSelectedLODLevelPercentage(float newScreenPercentage, int lod, List<LODGroupGUI.LODInfo> lods)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LODGroupGUI.\u003CSetSelectedLODLevelPercentage\u003Ec__AnonStorey96 percentageCAnonStorey96 = new LODGroupGUI.\u003CSetSelectedLODLevelPercentage\u003Ec__AnonStorey96();
      // ISSUE: reference to a compiler-generated field
      percentageCAnonStorey96.lods = lods;
      // ISSUE: reference to a compiler-generated field
      percentageCAnonStorey96.lod = lod;
      float num1 = 0.0f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      LODGroupGUI.LODInfo lodInfo1 = percentageCAnonStorey96.lods.FirstOrDefault<LODGroupGUI.LODInfo>(new Func<LODGroupGUI.LODInfo, bool>(percentageCAnonStorey96.\u003C\u003Em__17E));
      if (lodInfo1 != null)
        num1 = lodInfo1.ScreenPercent;
      float num2 = 1f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      LODGroupGUI.LODInfo lodInfo2 = percentageCAnonStorey96.lods.FirstOrDefault<LODGroupGUI.LODInfo>(new Func<LODGroupGUI.LODInfo, bool>(percentageCAnonStorey96.\u003C\u003Em__17F));
      if (lodInfo2 != null)
        num2 = lodInfo2.ScreenPercent;
      float max = Mathf.Clamp01(num2);
      float min = Mathf.Clamp01(num1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      percentageCAnonStorey96.lods[percentageCAnonStorey96.lod].ScreenPercent = Mathf.Clamp(newScreenPercentage, min, max);
    }

    public static void DrawLODSlider(Rect area, IList<LODGroupGUI.LODInfo> lods, int selectedLevel)
    {
      LODGroupGUI.Styles.m_LODSliderBG.Draw(area, GUIContent.none, false, false, false, false);
      for (int index = 0; index < lods.Count; ++index)
      {
        LODGroupGUI.LODInfo lod = lods[index];
        LODGroupGUI.DrawLODRange(lod, index != 0 ? lods[index - 1].RawScreenPercent : 1f, index == selectedLevel);
        LODGroupGUI.DrawLODButton(lod);
      }
      LODGroupGUI.DrawCulledRange(area, lods.Count <= 0 ? 1f : lods[lods.Count - 1].RawScreenPercent);
    }

    public static void DrawMixedValueLODSlider(Rect area)
    {
      LODGroupGUI.Styles.m_LODSliderBG.Draw(area, GUIContent.none, false, false, false, false);
      Rect culledBox = LODGroupGUI.GetCulledBox(area, 1f);
      Color color = GUI.color;
      GUI.color = LODGroupGUI.kLODColors[1] * 0.6f;
      LODGroupGUI.Styles.m_LODSliderRange.Draw(culledBox, GUIContent.none, false, false, false, false);
      GUI.color = color;
      GUIStyle style = new GUIStyle(EditorStyles.whiteLargeLabel) { alignment = TextAnchor.MiddleCenter };
      GUI.Label(area, "---", style);
    }

    private static Rect CalcLODRange(Rect totalRect, float startPercent, float endPercent)
    {
      float num1 = Mathf.Round(totalRect.width * (1f - startPercent));
      float num2 = Mathf.Round(totalRect.width * (1f - endPercent));
      return new Rect(totalRect.x + num1, totalRect.y, num2 - num1, totalRect.height);
    }

    private static void DrawLODButton(LODGroupGUI.LODInfo currentLOD)
    {
      EditorGUIUtility.AddCursorRect(currentLOD.m_ButtonPosition, MouseCursor.ResizeHorizontal);
    }

    private static void DrawLODRange(LODGroupGUI.LODInfo currentLOD, float previousLODPercentage, bool isSelected)
    {
      Color backgroundColor = GUI.backgroundColor;
      string text = string.Format("{0}\n{1:0}%", (object) currentLOD.LODName, (object) (float) ((double) previousLODPercentage * 100.0));
      if (isSelected)
      {
        Rect rangePosition = currentLOD.m_RangePosition;
        rangePosition.width -= 6f;
        rangePosition.height -= 6f;
        rangePosition.center += new Vector2(3f, 3f);
        LODGroupGUI.Styles.m_LODSliderRangeSelected.Draw(currentLOD.m_RangePosition, GUIContent.none, false, false, false, false);
        GUI.backgroundColor = LODGroupGUI.kLODColors[currentLOD.LODLevel];
        if ((double) rangePosition.width > 0.0)
          LODGroupGUI.Styles.m_LODSliderRange.Draw(rangePosition, GUIContent.none, false, false, false, false);
        LODGroupGUI.Styles.m_LODSliderText.Draw(currentLOD.m_RangePosition, text, false, false, false, false);
      }
      else
      {
        GUI.backgroundColor = LODGroupGUI.kLODColors[currentLOD.LODLevel];
        GUI.backgroundColor *= 0.6f;
        LODGroupGUI.Styles.m_LODSliderRange.Draw(currentLOD.m_RangePosition, GUIContent.none, false, false, false, false);
        LODGroupGUI.Styles.m_LODSliderText.Draw(currentLOD.m_RangePosition, text, false, false, false, false);
      }
      GUI.backgroundColor = backgroundColor;
    }

    private static void DrawCulledRange(Rect totalRect, float previousLODPercentage)
    {
      if (Mathf.Approximately(previousLODPercentage, 0.0f))
        return;
      Rect culledBox = LODGroupGUI.GetCulledBox(totalRect, LODGroupGUI.DelinearizeScreenPercentage(previousLODPercentage));
      Color color = GUI.color;
      GUI.color = LODGroupGUI.kCulledLODColor;
      LODGroupGUI.Styles.m_LODSliderRange.Draw(culledBox, GUIContent.none, false, false, false, false);
      GUI.color = color;
      string text = string.Format("Culled\n{0:0}%", (object) (float) ((double) previousLODPercentage * 100.0));
      LODGroupGUI.Styles.m_LODSliderText.Draw(culledBox, text, false, false, false, false);
    }

    public class GUIStyles
    {
      public readonly GUIStyle m_LODSliderBG = (GUIStyle) "LODSliderBG";
      public readonly GUIStyle m_LODSliderRange = (GUIStyle) "LODSliderRange";
      public readonly GUIStyle m_LODSliderRangeSelected = (GUIStyle) "LODSliderRangeSelected";
      public readonly GUIStyle m_LODSliderText = (GUIStyle) "LODSliderText";
      public readonly GUIStyle m_LODSliderTextSelected = (GUIStyle) "LODSliderTextSelected";
      public readonly GUIStyle m_LODStandardButton = (GUIStyle) "Button";
      public readonly GUIStyle m_LODRendererButton = (GUIStyle) "LODRendererButton";
      public readonly GUIStyle m_LODRendererAddButton = (GUIStyle) "LODRendererAddButton";
      public readonly GUIStyle m_LODRendererRemove = (GUIStyle) "LODRendererRemove";
      public readonly GUIStyle m_LODBlackBox = (GUIStyle) "LODBlackBox";
      public readonly GUIStyle m_LODCameraLine = (GUIStyle) "LODCameraLine";
      public readonly GUIStyle m_LODSceneText = (GUIStyle) "LODSceneText";
      public readonly GUIStyle m_LODRenderersText = (GUIStyle) "LODRenderersText";
      public readonly GUIStyle m_LODLevelNotifyText = (GUIStyle) "LODLevelNotifyText";
      public readonly GUIContent m_IconRendererPlus = EditorGUIUtility.IconContent("Toolbar Plus", "Add New Renderers");
      public readonly GUIContent m_IconRendererMinus = EditorGUIUtility.IconContent("Toolbar Minus", "Remove Renderer");
      public readonly GUIContent m_CameraIcon = EditorGUIUtility.IconContent("Camera Icon");
      public readonly GUIContent m_UploadToImporter = EditorGUIUtility.TextContent("Upload to Importer|Upload the modified screen percentages to the model importer.");
      public readonly GUIContent m_UploadToImporterDisabled = EditorGUIUtility.TextContent("Upload to Importer|Number of LOD's in the scene instance differ from the number of LOD's in the imported model.");
      public readonly GUIContent m_RecalculateBounds = EditorGUIUtility.TextContent("Bounds|Recalculate bounds for the current LOD group.");
      public readonly GUIContent m_LightmapScale = EditorGUIUtility.TextContent("Lightmap Scale|Set the lightmap scale to match the LOD percentages.");
      public readonly GUIContent m_RendersTitle = EditorGUIUtility.TextContent("Renderers:");
      public readonly GUIContent m_AnimatedCrossFadeInvalidText = EditorGUIUtility.TextContent("Animated cross-fading is currently disabled. Please enable \"Animate Between Next LOD\" on either the current or the previous LOD.");
      public readonly GUIContent m_AnimatedCrossFadeInconsistentText = EditorGUIUtility.TextContent("Animated cross-fading is currently disabled. \"Animate Between Next LOD\" is enabled but the next LOD is not in Animated Cross Fade mode.");
      public readonly GUIContent m_AnimateBetweenPreviousLOD = EditorGUIUtility.TextContent("Animate Between Previous LOD|Cross-fade animation plays when transits between this LOD and the previous (lower) LOD.");
    }

    public class LODInfo
    {
      public Rect m_ButtonPosition;
      public Rect m_RangePosition;

      public int LODLevel { get; private set; }

      public string LODName { get; private set; }

      public float RawScreenPercent { get; set; }

      public float ScreenPercent
      {
        get
        {
          return LODGroupGUI.DelinearizeScreenPercentage(this.RawScreenPercent);
        }
        set
        {
          this.RawScreenPercent = LODGroupGUI.LinearizeScreenPercentage(value);
        }
      }

      public LODInfo(int lodLevel, string name, float screenPercentage)
      {
        this.LODLevel = lodLevel;
        this.LODName = name;
        this.RawScreenPercent = screenPercentage;
      }
    }
  }
}
