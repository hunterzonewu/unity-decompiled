// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerChart
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class ProfilerChart
  {
    private const string kPrefCharts = "ProfilerChart";
    private bool m_Active;
    public ProfilerArea m_Area;
    public Chart.ChartType m_Type;
    public float m_DataScale;
    public Chart m_Chart;
    public ChartData m_Data;
    public ChartSeries[] m_Series;
    public GUIContent m_Icon;
    private static string[] s_LocalizedChartNames;

    public bool active
    {
      get
      {
        return this.m_Active;
      }
      set
      {
        if (this.m_Active == value)
          return;
        this.m_Active = value;
        this.ApplyActiveState();
        this.SaveActiveState();
      }
    }

    public ProfilerChart(ProfilerArea area, Chart.ChartType type, float dataScale, int seriesCount)
    {
      this.m_Area = area;
      this.m_Type = type;
      this.m_DataScale = dataScale;
      this.m_Chart = new Chart();
      this.m_Data = new ChartData();
      this.m_Series = new ChartSeries[seriesCount];
      this.m_Active = this.ReadActiveState();
      this.ApplyActiveState();
    }

    private string GetLocalizedChartName()
    {
      if (ProfilerChart.s_LocalizedChartNames == null)
        ProfilerChart.s_LocalizedChartNames = new string[9]
        {
          LocalizationDatabase.GetLocalizedString("CPU Usage|Graph out the various CPU areas"),
          LocalizationDatabase.GetLocalizedString("GPU Usage|Graph out the various GPU areas"),
          LocalizationDatabase.GetLocalizedString("Rendering"),
          LocalizationDatabase.GetLocalizedString("Memory|Graph out the various memory usage areas"),
          LocalizationDatabase.GetLocalizedString("Audio"),
          LocalizationDatabase.GetLocalizedString("Physics"),
          LocalizationDatabase.GetLocalizedString("Physics (2D)"),
          LocalizationDatabase.GetLocalizedString("Network Messages"),
          LocalizationDatabase.GetLocalizedString("Network Operations")
        };
      return ProfilerChart.s_LocalizedChartNames[(int) this.m_Area];
    }

    public int DoChartGUI(int currentFrame, ProfilerArea currentArea, out Chart.ChartAction action)
    {
      if (Event.current.type == EventType.Repaint)
      {
        string[] strArray = new string[this.m_Series.Length];
        for (int index = 0; index < this.m_Series.Length; ++index)
        {
          int statisticsIdentifier = ProfilerDriver.GetStatisticsIdentifier(!this.m_Data.hasOverlay ? this.m_Series[index].identifierName : "Selected" + this.m_Series[index].identifierName);
          strArray[index] = ProfilerDriver.GetFormattedStatisticsValue(currentFrame, statisticsIdentifier);
        }
        this.m_Data.selectedLabels = strArray;
      }
      if (this.m_Icon == null)
        this.m_Icon = EditorGUIUtility.TextContentWithIcon(this.GetLocalizedChartName(), "Profiler." + Enum.GetName(typeof (ProfilerArea), (object) this.m_Area));
      return this.m_Chart.DoGUI(this.m_Type, currentFrame, this.m_Data, this.m_Area, currentArea == this.m_Area, this.m_Icon, out action);
    }

    public void LoadAndBindSettings()
    {
      this.m_Chart.LoadAndBindSettings("ProfilerChart" + (object) this.m_Area, this.m_Data);
    }

    private void ApplyActiveState()
    {
      if (this.m_Area != ProfilerArea.GPU)
        return;
      ProfilerDriver.profileGPU = this.active;
    }

    private bool ReadActiveState()
    {
      if (this.m_Area == ProfilerArea.GPU)
        return SessionState.GetBool("ProfilerChart" + (object) this.m_Area, false);
      return EditorPrefs.GetBool("ProfilerChart" + (object) this.m_Area, true);
    }

    private void SaveActiveState()
    {
      if (this.m_Area == ProfilerArea.GPU)
        SessionState.SetBool("ProfilerChart" + (object) this.m_Area, this.m_Active);
      else
        EditorPrefs.SetBool("ProfilerChart" + (object) this.m_Area, this.m_Active);
    }
  }
}
