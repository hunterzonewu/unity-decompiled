// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProfilerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Profiler", useTypeNameAsIconName = true)]
  internal class ProfilerWindow : EditorWindow, IProfilerWindowController
  {
    private static List<ProfilerWindow> m_ProfilerWindows = new List<ProfilerWindow>();
    private static readonly int s_HashControlID = "ProfilerSearchField".GetHashCode();
    private string m_SearchString = string.Empty;
    private SplitterState m_VertSplit = new SplitterState(new float[2]{ 50f, 50f }, new int[2]{ 50, 50 }, (int[]) null);
    private SplitterState m_ViewSplit = new SplitterState(new float[2]{ 70f, 30f }, new int[2]{ 450, 50 }, (int[]) null);
    private SplitterState m_NetworkSplit = new SplitterState(new float[2]{ 20f, 80f }, new int[2]{ 100, 100 }, (int[]) null);
    [SerializeField]
    private bool m_Recording = true;
    private AttachProfilerUI m_AttachProfilerUI = new AttachProfilerUI();
    private Vector2 m_GraphPos = Vector2.zero;
    private Vector2[] m_PaneScroll = new Vector2[9];
    private int m_CurrentFrame = -1;
    private int m_LastFrameFromTick = -1;
    private int m_PrevLastFrame = -1;
    private int m_LastAudioProfilerFrame = -1;
    private float[] m_ChartOldMax = new float[2]{ -1f, -1f };
    private float m_ChartMaxClamp = 70000f;
    private bool m_GatherObjectReferences = true;
    private string[] msgNames = new string[16]{ "UserMessage", "ObjectDestroy", "ClientRpc", "ObjectSpawn", "Owner", "Command", "LocalPlayerTransform", "SyncEvent", "SyncVars", "SyncList", "ObjectSpawnScene", "NetworkInfo", "SpawnFinished", "ObjectHide", "CRC", "ClientAuthority" };
    private bool[] msgFoldouts = new bool[15]{ true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
    private const float kRowHeight = 16f;
    private const float kIndentPx = 16f;
    private const float kBaseIndent = 8f;
    private const float kSmallMargin = 4f;
    private const float kNameColumnSize = 350f;
    private const float kColumnSize = 80f;
    private const float kFoldoutSize = 14f;
    private const int kFirst = -999999;
    private const int kLast = 999999;
    private const string kProfilerColumnSettings = "VisibleProfilerColumnsV2";
    private const string kProfilerDetailColumnSettings = "VisibleProfilerDetailColumns";
    private const string kProfilerGPUColumnSettings = "VisibleProfilerGPUColumns";
    private const string kProfilerGPUDetailColumnSettings = "VisibleProfilerGPUDetailColumns";
    private const string kProfilerVisibleGraphsSettings = "VisibleProfilerGraphs";
    private const string kSearchControlName = "ProfilerSearchField";
    private static ProfilerWindow.Styles ms_Styles;
    private bool m_FocusSearchField;
    private string m_ActiveNativePlatformSupportModule;
    private ProfilerViewType m_ViewType;
    private ProfilerArea m_CurrentArea;
    private ProfilerMemoryView m_ShowDetailedMemoryPane;
    private ProfilerAudioView m_ShowDetailedAudioPane;
    private ProfilerChart[] m_Charts;
    private ProfilerHierarchyGUI m_CPUHierarchyGUI;
    private ProfilerHierarchyGUI m_GPUHierarchyGUI;
    private ProfilerHierarchyGUI m_CPUDetailHierarchyGUI;
    private ProfilerHierarchyGUI m_GPUDetailHierarchyGUI;
    private ProfilerTimelineGUI m_CPUTimelineGUI;
    private MemoryTreeList m_ReferenceListView;
    private MemoryTreeListClickable m_MemoryListView;
    [SerializeField]
    private AudioProfilerTreeViewState m_AudioProfilerTreeViewState;
    private AudioProfilerView m_AudioProfilerView;
    private AudioProfilerBackend m_AudioProfilerBackend;

    private bool wantsMemoryRefresh
    {
      get
      {
        return this.m_MemoryListView.GetRoot() == null;
      }
    }

    private void BuildColumns()
    {
      ProfilerColumn[] profilerColumnArray1 = new ProfilerColumn[8]{ ProfilerColumn.FunctionName, ProfilerColumn.TotalPercent, ProfilerColumn.SelfPercent, ProfilerColumn.Calls, ProfilerColumn.GCMemory, ProfilerColumn.TotalTime, ProfilerColumn.SelfTime, ProfilerColumn.WarningCount };
      ProfilerColumn[] profilerColumnArray2 = new ProfilerColumn[7]{ ProfilerColumn.ObjectName, ProfilerColumn.TotalPercent, ProfilerColumn.SelfPercent, ProfilerColumn.Calls, ProfilerColumn.GCMemory, ProfilerColumn.TotalTime, ProfilerColumn.SelfTime };
      this.m_CPUHierarchyGUI = new ProfilerHierarchyGUI((IProfilerWindowController) this, "VisibleProfilerColumnsV2", profilerColumnArray1, ProfilerWindow.ProfilerColumnNames(profilerColumnArray1), false, ProfilerColumn.TotalTime);
      this.m_CPUTimelineGUI = new ProfilerTimelineGUI((IProfilerWindowController) this);
      string text = EditorGUIUtility.TextContent("Object").text;
      string[] columnNames1 = ProfilerWindow.ProfilerColumnNames(profilerColumnArray2);
      columnNames1[0] = text;
      this.m_CPUDetailHierarchyGUI = new ProfilerHierarchyGUI((IProfilerWindowController) this, "VisibleProfilerDetailColumns", profilerColumnArray2, columnNames1, true, ProfilerColumn.TotalTime);
      ProfilerColumn[] profilerColumnArray3 = new ProfilerColumn[4]{ ProfilerColumn.FunctionName, ProfilerColumn.TotalGPUPercent, ProfilerColumn.DrawCalls, ProfilerColumn.TotalGPUTime };
      ProfilerColumn[] profilerColumnArray4 = new ProfilerColumn[4]{ ProfilerColumn.ObjectName, ProfilerColumn.TotalGPUPercent, ProfilerColumn.DrawCalls, ProfilerColumn.TotalGPUTime };
      this.m_GPUHierarchyGUI = new ProfilerHierarchyGUI((IProfilerWindowController) this, "VisibleProfilerGPUColumns", profilerColumnArray3, ProfilerWindow.ProfilerColumnNames(profilerColumnArray3), false, ProfilerColumn.TotalGPUTime);
      string[] columnNames2 = ProfilerWindow.ProfilerColumnNames(profilerColumnArray4);
      columnNames2[0] = text;
      this.m_GPUDetailHierarchyGUI = new ProfilerHierarchyGUI((IProfilerWindowController) this, "VisibleProfilerGPUDetailColumns", profilerColumnArray4, columnNames2, true, ProfilerColumn.TotalGPUTime);
    }

    private static string[] ProfilerColumnNames(ProfilerColumn[] columns)
    {
      string[] names = Enum.GetNames(typeof (ProfilerColumn));
      string[] strArray = new string[columns.Length];
      for (int index = 0; index < columns.Length; ++index)
      {
        switch (columns[index])
        {
          case ProfilerColumn.FunctionName:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Overview");
            break;
          case ProfilerColumn.TotalPercent:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Total");
            break;
          case ProfilerColumn.SelfPercent:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Self");
            break;
          case ProfilerColumn.Calls:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Calls");
            break;
          case ProfilerColumn.GCMemory:
            strArray[index] = LocalizationDatabase.GetLocalizedString("GC Alloc");
            break;
          case ProfilerColumn.TotalTime:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Time ms");
            break;
          case ProfilerColumn.SelfTime:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Self ms");
            break;
          case ProfilerColumn.DrawCalls:
            strArray[index] = LocalizationDatabase.GetLocalizedString("DrawCalls");
            break;
          case ProfilerColumn.TotalGPUTime:
            strArray[index] = LocalizationDatabase.GetLocalizedString("GPU ms");
            break;
          case ProfilerColumn.SelfGPUTime:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Self ms");
            break;
          case ProfilerColumn.TotalGPUPercent:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Total");
            break;
          case ProfilerColumn.SelfGPUPercent:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Self");
            break;
          case ProfilerColumn.WarningCount:
            strArray[index] = LocalizationDatabase.GetLocalizedString("|Warnings");
            break;
          case ProfilerColumn.ObjectName:
            strArray[index] = LocalizationDatabase.GetLocalizedString("Name");
            break;
          default:
            strArray[index] = "ProfilerColumn." + names[(int) columns[index]];
            break;
        }
      }
      return strArray;
    }

    public void SetSelectedPropertyPath(string path)
    {
      if (!(ProfilerDriver.selectedPropertyPath != path))
        return;
      ProfilerDriver.selectedPropertyPath = path;
      this.UpdateCharts();
    }

    public void ClearSelectedPropertyPath()
    {
      if (!(ProfilerDriver.selectedPropertyPath != string.Empty))
        return;
      this.m_CPUHierarchyGUI.selectedIndex = -1;
      ProfilerDriver.selectedPropertyPath = string.Empty;
      this.UpdateCharts();
    }

    public ProfilerProperty CreateProperty(bool details)
    {
      ProfilerProperty profilerProperty = new ProfilerProperty();
      ProfilerColumn profilerSortColumn = this.m_CurrentArea != ProfilerArea.CPU ? (!details ? this.m_GPUHierarchyGUI.sortType : this.m_GPUDetailHierarchyGUI.sortType) : (!details ? this.m_CPUHierarchyGUI.sortType : this.m_CPUDetailHierarchyGUI.sortType);
      profilerProperty.SetRoot(this.GetActiveVisibleFrameIndex(), profilerSortColumn, this.m_ViewType);
      profilerProperty.onlyShowGPUSamples = this.m_CurrentArea == ProfilerArea.GPU;
      return profilerProperty;
    }

    public int GetActiveVisibleFrameIndex()
    {
      if (this.m_CurrentFrame == -1)
        return this.m_LastFrameFromTick;
      return this.m_CurrentFrame;
    }

    public void SetSearch(string searchString)
    {
      this.m_SearchString = !string.IsNullOrEmpty(searchString) ? searchString : string.Empty;
    }

    public string GetSearch()
    {
      return this.m_SearchString;
    }

    public bool IsSearching()
    {
      if (!string.IsNullOrEmpty(this.m_SearchString))
        return this.m_SearchString.Length > 0;
      return false;
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      ProfilerWindow.m_ProfilerWindows.Add(this);
      this.Initialize();
    }

    private void Initialize()
    {
      int len = ProfilerDriver.maxHistoryLength - 1;
      this.m_Charts = new ProfilerChart[9];
      Color[] colors = ProfilerColors.colors;
      for (ProfilerArea area = ProfilerArea.CPU; area < ProfilerArea.AreaCount; ++area)
      {
        float dataScale = 1f;
        Chart.ChartType type = Chart.ChartType.Line;
        string[] propertiesForArea = ProfilerDriver.GetGraphStatisticsPropertiesForArea(area);
        int length = propertiesForArea.Length;
        if (area == ProfilerArea.GPU || area == ProfilerArea.CPU)
        {
          type = Chart.ChartType.StackedFill;
          dataScale = 1f / 1000f;
        }
        ProfilerChart profilerChart = new ProfilerChart(area, type, dataScale, length);
        for (int index = 0; index < length; ++index)
          profilerChart.m_Series[index] = new ChartSeries(propertiesForArea[index], len, colors[index]);
        this.m_Charts[(int) area] = profilerChart;
      }
      if (this.m_ReferenceListView == null)
        this.m_ReferenceListView = new MemoryTreeList((EditorWindow) this, (MemoryTreeList) null);
      if (this.m_MemoryListView == null)
        this.m_MemoryListView = new MemoryTreeListClickable((EditorWindow) this, this.m_ReferenceListView);
      this.UpdateCharts();
      this.BuildColumns();
      foreach (ProfilerChart chart in this.m_Charts)
        chart.LoadAndBindSettings();
    }

    private void CheckForPlatformModuleChange()
    {
      if (!(this.m_ActiveNativePlatformSupportModule != EditorUtility.GetActiveNativePlatformSupportModuleName()))
        return;
      ProfilerDriver.ResetHistory();
      this.Initialize();
      this.m_ActiveNativePlatformSupportModule = EditorUtility.GetActiveNativePlatformSupportModuleName();
    }

    private void OnDisable()
    {
      ProfilerWindow.m_ProfilerWindows.Remove(this);
    }

    private void Awake()
    {
      if (!Profiler.supported)
        return;
      Profiler.enabled = this.m_Recording;
    }

    private void OnDestroy()
    {
      if (!Profiler.supported)
        return;
      Profiler.enabled = false;
    }

    private void OnFocus()
    {
      if (!Profiler.supported)
        return;
      Profiler.enabled = this.m_Recording;
    }

    private void OnLostFocus()
    {
    }

    private static void ShowProfilerWindow()
    {
      EditorWindow.GetWindow<ProfilerWindow>(false);
    }

    [RequiredByNativeCode]
    private static void RepaintAllProfilerWindows()
    {
      using (List<ProfilerWindow>.Enumerator enumerator = ProfilerWindow.m_ProfilerWindows.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ProfilerWindow current = enumerator.Current;
          if (ProfilerDriver.lastFrameIndex != current.m_LastFrameFromTick)
          {
            current.m_LastFrameFromTick = ProfilerDriver.lastFrameIndex;
            current.RepaintImmediately();
          }
        }
      }
    }

    private static void SetMemoryProfilerInfo(ObjectMemoryInfo[] memoryInfo, int[] referencedIndices)
    {
      using (List<ProfilerWindow>.Enumerator enumerator = ProfilerWindow.m_ProfilerWindows.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ProfilerWindow current = enumerator.Current;
          if (current.wantsMemoryRefresh)
            current.m_MemoryListView.SetRoot(MemoryElementDataManager.GetTreeRoot(memoryInfo, referencedIndices));
        }
      }
    }

    private static void SetProfileDeepScripts(bool deep)
    {
      if (ProfilerDriver.deepProfiling == deep)
        return;
      bool flag = true;
      if (EditorApplication.isPlaying)
        flag = !deep ? EditorUtility.DisplayDialog("Disable deep script profiling", "Disabling deep profiling requires reloading all scripts", "Reload", "Cancel") : EditorUtility.DisplayDialog("Enable deep script profiling", "Enabling deep profiling requires reloading scripts.", "Reload", "Cancel");
      if (!flag)
        return;
      ProfilerDriver.deepProfiling = deep;
      InternalEditorUtility.RequestScriptReload();
    }

    private string PickFrameLabel()
    {
      if (this.m_CurrentFrame == -1)
        return "Current";
      return (this.m_CurrentFrame + 1).ToString() + " / " + (object) (ProfilerDriver.lastFrameIndex + 1);
    }

    private void PrevFrame()
    {
      int previousFrameIndex = ProfilerDriver.GetPreviousFrameIndex(this.m_CurrentFrame);
      if (previousFrameIndex == -1)
        return;
      this.SetCurrentFrame(previousFrameIndex);
    }

    private void NextFrame()
    {
      int nextFrameIndex = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame);
      if (nextFrameIndex == -1)
        return;
      this.SetCurrentFrame(nextFrameIndex);
    }

    private static void DrawEmptyCPUOrRenderingDetailPane()
    {
      GUILayout.Box(string.Empty, ProfilerWindow.ms_Styles.header, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      GUILayout.Label("Select Line for per-object breakdown", EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      GUILayout.EndVertical();
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
    }

    private void DrawCPUOrRenderingToolbar(ProfilerProperty property)
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.m_ViewType = (ProfilerViewType) EditorGUILayout.IntPopup((int) this.m_ViewType, new string[3]
      {
        "Hierarchy",
        "Timeline",
        "Raw Hierarchy"
      }, new int[3]{ 0, 1, 2 }, EditorStyles.toolbarDropDown, new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      });
      GUILayout.FlexibleSpace();
      GUILayout.Label(string.Format("CPU:{0}ms   GPU:{1}ms", (object) property.frameTime, (object) property.frameGpuTime), EditorStyles.miniLabel, new GUILayoutOption[0]);
      GUI.enabled = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame) == -1;
      if (GUILayout.Button(!GUI.enabled ? ProfilerWindow.ms_Styles.noFrameDebugger : ProfilerWindow.ms_Styles.frameDebugger, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        FrameDebuggerWindow.ShowFrameDebuggerWindow().EnableIfNeeded();
      GUI.enabled = true;
      if (ProfilerInstrumentationPopup.InstrumentationEnabled && GUILayout.Button(ProfilerWindow.ms_Styles.profilerInstrumentation, EditorStyles.toolbarDropDown, new GUILayoutOption[0]))
        ProfilerInstrumentationPopup.Show(GUILayoutUtility.topLevel.GetLast());
      GUILayout.FlexibleSpace();
      this.SearchFieldGUI();
      EditorGUILayout.EndHorizontal();
      this.HandleCommandEvents();
    }

    private void HandleCommandEvents()
    {
      Event current = Event.current;
      EventType type = current.type;
      switch (type)
      {
        case EventType.ExecuteCommand:
        case EventType.ValidateCommand:
          bool flag = type == EventType.ExecuteCommand;
          if (!(Event.current.commandName == "Find"))
            break;
          if (flag)
            this.m_FocusSearchField = true;
          current.Use();
          break;
      }
    }

    internal void SearchFieldGUI()
    {
      Event current = Event.current;
      Rect rect = GUILayoutUtility.GetRect(50f, 300f, 16f, 16f, EditorStyles.toolbarSearchField);
      if (this.m_ViewType == ProfilerViewType.Timeline)
        return;
      GUI.SetNextControlName("ProfilerSearchField");
      if (this.m_FocusSearchField)
      {
        EditorGUI.FocusTextInControl("ProfilerSearchField");
        if (Event.current.type == EventType.Repaint)
          this.m_FocusSearchField = false;
      }
      if (current.type == EventType.KeyDown && current.keyCode == KeyCode.Escape && GUI.GetNameOfFocusedControl() == "ProfilerSearchField")
        this.m_SearchString = string.Empty;
      if (current.type == EventType.KeyDown && (current.keyCode == KeyCode.DownArrow || current.keyCode == KeyCode.UpArrow) && GUI.GetNameOfFocusedControl() == "ProfilerSearchField")
      {
        this.m_CPUHierarchyGUI.SelectFirstRow();
        this.m_CPUHierarchyGUI.SetKeyboardFocus();
        this.Repaint();
        current.Use();
      }
      bool flag = this.m_CPUHierarchyGUI.selectedIndex != -1;
      EditorGUI.BeginChangeCheck();
      this.m_SearchString = EditorGUI.ToolbarSearchField(GUIUtility.GetControlID(ProfilerWindow.s_HashControlID, FocusType.Keyboard, this.position), rect, this.m_SearchString, false);
      if (!EditorGUI.EndChangeCheck() || this.IsSearching() || (GUIUtility.keyboardControl != 0 || !flag))
        return;
      this.m_CPUHierarchyGUI.FrameSelection();
    }

    private static bool CheckFrameData(ProfilerProperty property)
    {
      if (property.frameDataReady)
        return true;
      GUILayout.Label(ProfilerWindow.ms_Styles.noData, ProfilerWindow.ms_Styles.background, new GUILayoutOption[0]);
      return false;
    }

    private void DrawCPUOrRenderingPane(ProfilerHierarchyGUI mainPane, ProfilerHierarchyGUI detailPane, ProfilerTimelineGUI timelinePane)
    {
      ProfilerProperty property1 = this.CreateProperty(false);
      this.DrawCPUOrRenderingToolbar(property1);
      if (!ProfilerWindow.CheckFrameData(property1))
        property1.Cleanup();
      else if (timelinePane != null && this.m_ViewType == ProfilerViewType.Timeline)
      {
        float height = (float) this.m_VertSplit.realSizes[1] - (EditorStyles.toolbar.CalcHeight(GUIContent.none, 10f) + 2f);
        timelinePane.DoGUI(this.GetActiveVisibleFrameIndex(), this.position.width, this.position.height - height, height);
        property1.Cleanup();
      }
      else
      {
        SplitterGUILayout.BeginHorizontalSplit(this.m_ViewSplit);
        GUILayout.BeginVertical();
        bool expandAll = false;
        mainPane.DoGUI(property1, this.m_SearchString, expandAll);
        property1.Cleanup();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        ProfilerProperty property2 = this.CreateProperty(true);
        ProfilerProperty detailedProperty = mainPane.GetDetailedProperty(property2);
        property2.Cleanup();
        if (detailedProperty != null)
        {
          detailPane.DoGUI(detailedProperty, string.Empty, expandAll);
          detailedProperty.Cleanup();
        }
        else
          ProfilerWindow.DrawEmptyCPUOrRenderingDetailPane();
        GUILayout.EndVertical();
        SplitterGUILayout.EndHorizontalSplit();
      }
    }

    private void DrawMemoryPane(SplitterState splitter)
    {
      this.DrawMemoryToolbar();
      if (this.m_ShowDetailedMemoryPane == ProfilerMemoryView.Simple)
        this.DrawOverviewText(ProfilerArea.Memory);
      else
        this.DrawDetailedMemoryPane(splitter);
    }

    private void DrawDetailedMemoryPane(SplitterState splitter)
    {
      SplitterGUILayout.BeginHorizontalSplit(splitter);
      this.m_MemoryListView.OnGUI();
      this.m_ReferenceListView.OnGUI();
      SplitterGUILayout.EndHorizontalSplit();
    }

    private static Rect GenerateRect(ref int row, int indent)
    {
      Rect rect = new Rect((float) ((double) indent * 16.0 + 8.0), (float) row * 16f, 0.0f, 16f);
      rect.xMax = 350f;
      row = row + 1;
      return rect;
    }

    private void DrawNetworkOperationsPane()
    {
      SplitterGUILayout.BeginHorizontalSplit(this.m_NetworkSplit);
      GUILayout.Label(ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex()), EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      this.m_PaneScroll[(int) this.m_CurrentArea] = GUILayout.BeginScrollView(this.m_PaneScroll[(int) this.m_CurrentArea], ProfilerWindow.ms_Styles.background);
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      EditorGUILayout.LabelField("Operation Detail");
      EditorGUILayout.LabelField("Over 5 Ticks");
      EditorGUILayout.LabelField("Over 10 Ticks");
      EditorGUILayout.LabelField("Total");
      EditorGUILayout.EndHorizontal();
      ++EditorGUI.indentLevel;
      for (short key = 0; (int) key < this.msgNames.Length; ++key)
      {
        if (NetworkDetailStats.m_NetworkOperations.ContainsKey(key))
        {
          this.msgFoldouts[(int) key] = EditorGUILayout.Foldout(this.msgFoldouts[(int) key], this.msgNames[(int) key] + ":");
          if (this.msgFoldouts[(int) key])
          {
            EditorGUILayout.BeginVertical();
            NetworkDetailStats.NetworkOperationDetails networkOperation = NetworkDetailStats.m_NetworkOperations[key];
            ++EditorGUI.indentLevel;
            using (Dictionary<string, NetworkDetailStats.NetworkOperationEntryDetails>.KeyCollection.Enumerator enumerator = networkOperation.m_Entries.Keys.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                string current = enumerator.Current;
                int time = (int) Time.time;
                NetworkDetailStats.NetworkOperationEntryDetails entry = networkOperation.m_Entries[current];
                if (entry.m_IncomingTotal > 0)
                {
                  EditorGUILayout.BeginHorizontal();
                  EditorGUILayout.LabelField("IN:" + current);
                  EditorGUILayout.LabelField(entry.m_IncomingSequence.GetFiveTick(time).ToString());
                  EditorGUILayout.LabelField(entry.m_IncomingSequence.GetTenTick(time).ToString());
                  EditorGUILayout.LabelField(entry.m_IncomingTotal.ToString());
                  EditorGUILayout.EndHorizontal();
                }
                if (entry.m_OutgoingTotal > 0)
                {
                  EditorGUILayout.BeginHorizontal();
                  EditorGUILayout.LabelField("OUT:" + current);
                  EditorGUILayout.LabelField(entry.m_OutgoingSequence.GetFiveTick(time).ToString());
                  EditorGUILayout.LabelField(entry.m_OutgoingSequence.GetTenTick(time).ToString());
                  EditorGUILayout.LabelField(entry.m_OutgoingTotal.ToString());
                  EditorGUILayout.EndHorizontal();
                }
              }
            }
            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
          }
        }
      }
      --EditorGUI.indentLevel;
      GUILayout.EndScrollView();
      SplitterGUILayout.EndHorizontalSplit();
    }

    private void DrawAudioPane()
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      ProfilerAudioView profilerAudioView = this.m_ShowDetailedAudioPane;
      if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Stats, "Stats", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.Stats;
      if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Channels, "Channels", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.Channels;
      if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Groups, "Groups", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.Groups;
      if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.ChannelsAndGroups, "Channels and groups", EditorStyles.toolbarButton, new GUILayoutOption[0]))
        profilerAudioView = ProfilerAudioView.ChannelsAndGroups;
      if (profilerAudioView != this.m_ShowDetailedAudioPane)
      {
        this.m_ShowDetailedAudioPane = profilerAudioView;
        this.m_LastAudioProfilerFrame = -1;
      }
      if (this.m_ShowDetailedAudioPane == ProfilerAudioView.Stats)
      {
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        this.DrawOverviewText(this.m_CurrentArea);
      }
      else
      {
        GUILayout.Space(5f);
        bool flag1 = GUILayout.Toggle(AudioUtil.resetAllAudioClipPlayCountsOnPlay, "Reset play count on play", EditorStyles.toolbarButton, new GUILayoutOption[0]);
        if (flag1 != AudioUtil.resetAllAudioClipPlayCountsOnPlay)
          AudioUtil.resetAllAudioClipPlayCountsOnPlay = flag1;
        if (Unsupported.IsDeveloperBuild())
        {
          GUILayout.Space(5f);
          bool flag2 = EditorPrefs.GetBool("AudioProfilerShowAllGroups");
          bool flag3 = GUILayout.Toggle(flag2, "Show all groups (dev-builds only)", EditorStyles.toolbarButton, new GUILayoutOption[0]);
          if (flag2 != flag3)
            EditorPrefs.SetBool("AudioProfilerShowAllGroups", flag3);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        Rect rect1 = GUILayoutUtility.GetRect(20f, 20000f, 10f, 10000f);
        Rect position = new Rect(rect1.x, rect1.y, 230f, rect1.height);
        Rect rect2 = new Rect(position.xMax, rect1.y, rect1.width - position.width, rect1.height);
        string overviewText = ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex());
        Vector2 vector2 = EditorStyles.wordWrappedLabel.CalcSize(GUIContent.Temp(overviewText));
        this.m_PaneScroll[(int) this.m_CurrentArea] = GUI.BeginScrollView(position, this.m_PaneScroll[(int) this.m_CurrentArea], new Rect(0.0f, 0.0f, vector2.x, vector2.y));
        GUI.Label(new Rect(3f, 3f, vector2.x, vector2.y), overviewText, EditorStyles.wordWrappedLabel);
        GUI.EndScrollView();
        EditorGUI.DrawRect(new Rect(position.xMax - 1f, position.y, 1f, position.height), Color.black);
        if (this.m_AudioProfilerTreeViewState == null)
          this.m_AudioProfilerTreeViewState = new AudioProfilerTreeViewState();
        if (this.m_AudioProfilerBackend == null)
          this.m_AudioProfilerBackend = new AudioProfilerBackend(this.m_AudioProfilerTreeViewState);
        ProfilerProperty property = this.CreateProperty(false);
        if (ProfilerWindow.CheckFrameData(property))
        {
          if (this.m_CurrentFrame == -1 || this.m_LastAudioProfilerFrame != this.m_CurrentFrame)
          {
            this.m_LastAudioProfilerFrame = this.m_CurrentFrame;
            AudioProfilerInfo[] audioProfilerInfo = property.GetAudioProfilerInfo();
            if (audioProfilerInfo != null && audioProfilerInfo.Length > 0)
            {
              List<AudioProfilerInfoWrapper> data = new List<AudioProfilerInfoWrapper>();
              foreach (AudioProfilerInfo info in audioProfilerInfo)
              {
                bool flag2 = (info.flags & 64) != 0;
                if ((this.m_ShowDetailedAudioPane != ProfilerAudioView.Channels || !flag2) && (this.m_ShowDetailedAudioPane != ProfilerAudioView.Groups || flag2))
                  data.Add(new AudioProfilerInfoWrapper(info, property.GetAudioProfilerNameByOffset(info.assetNameOffset), property.GetAudioProfilerNameByOffset(info.objectNameOffset), this.m_ShowDetailedAudioPane == ProfilerAudioView.Channels));
              }
              this.m_AudioProfilerBackend.SetData(data);
              if (this.m_AudioProfilerView == null)
              {
                this.m_AudioProfilerView = new AudioProfilerView((EditorWindow) this, this.m_AudioProfilerTreeViewState);
                this.m_AudioProfilerView.Init(rect2, this.m_AudioProfilerBackend);
              }
            }
          }
          if (this.m_AudioProfilerView != null)
            this.m_AudioProfilerView.OnGUI(rect2, this.m_ShowDetailedAudioPane == ProfilerAudioView.Channels);
        }
        property.Cleanup();
      }
    }

    private static void DrawBackground(int row, bool selected)
    {
      Rect position = new Rect(1f, 16f * (float) row, GUIClip.visibleRect.width, 16f);
      GUIStyle guiStyle = row % 2 != 0 ? ProfilerWindow.ms_Styles.entryOdd : ProfilerWindow.ms_Styles.entryEven;
      if (Event.current.type != EventType.Repaint)
        return;
      guiStyle.Draw(position, GUIContent.none, false, false, selected, false);
    }

    private void DrawMemoryToolbar()
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.m_ShowDetailedMemoryPane = (ProfilerMemoryView) EditorGUILayout.EnumPopup((Enum) this.m_ShowDetailedMemoryPane, EditorStyles.toolbarDropDown, new GUILayoutOption[1]
      {
        GUILayout.Width(70f)
      });
      GUILayout.Space(5f);
      if (this.m_ShowDetailedMemoryPane == ProfilerMemoryView.Detailed)
      {
        if (GUILayout.Button("Take Sample: " + this.m_AttachProfilerUI.GetConnectedProfiler(), EditorStyles.toolbarButton, new GUILayoutOption[0]))
          this.RefreshMemoryData();
        this.m_GatherObjectReferences = GUILayout.Toggle(this.m_GatherObjectReferences, ProfilerWindow.ms_Styles.gatherObjectReferences, EditorStyles.toolbarButton, new GUILayoutOption[0]);
        if (this.m_AttachProfilerUI.IsEditor())
          GUILayout.Label("Memory usage in editor is not as it would be in a player", EditorStyles.toolbarButton, new GUILayoutOption[0]);
      }
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void RefreshMemoryData()
    {
      this.m_MemoryListView.SetRoot((MemoryElement) null);
      ProfilerDriver.RequestObjectMemoryInfo(this.m_GatherObjectReferences);
    }

    private static void UpdateChartGrid(float timeMax, ChartData data)
    {
      if ((double) timeMax < 1500.0)
        data.SetGrid(new float[3]
        {
          1000f,
          250f,
          100f
        }, new string[3]
        {
          "1ms (1000FPS)",
          "0.25ms (4000FPS)",
          "0.1ms (10000FPS)"
        });
      else if ((double) timeMax < 10000.0)
        data.SetGrid(new float[3]
        {
          8333f,
          4000f,
          1000f
        }, new string[3]
        {
          "8ms (120FPS)",
          "4ms (250FPS)",
          "1ms (1000FPS)"
        });
      else if ((double) timeMax < 30000.0)
        data.SetGrid(new float[3]
        {
          16667f,
          10000f,
          5000f
        }, new string[3]
        {
          "16ms (60FPS)",
          "10ms (100FPS)",
          "5ms (200FPS)"
        });
      else if ((double) timeMax < 100000.0)
        data.SetGrid(new float[3]
        {
          66667f,
          33333f,
          16667f
        }, new string[3]
        {
          "66ms (15FPS)",
          "33ms (30FPS)",
          "16ms (60FPS)"
        });
      else
        data.SetGrid(new float[3]
        {
          500000f,
          200000f,
          66667f
        }, new string[3]
        {
          "500ms (2FPS)",
          "200ms (5FPS)",
          "66ms (15FPS)"
        });
    }

    private void UpdateCharts()
    {
      int num1 = ProfilerDriver.maxHistoryLength - 1;
      int num2 = ProfilerDriver.lastFrameIndex - num1;
      int firstSelectableFrame = Mathf.Max(ProfilerDriver.firstFrameIndex, num2);
      foreach (ProfilerChart chart in this.m_Charts)
      {
        float num3 = 1f;
        float[] scale = new float[chart.m_Series.Length];
        for (int index = 0; index < chart.m_Series.Length; ++index)
        {
          float maxValue;
          ProfilerDriver.GetStatisticsValues(ProfilerDriver.GetStatisticsIdentifier(chart.m_Series[index].identifierName), num2, chart.m_DataScale, chart.m_Series[index].data, out maxValue);
          maxValue = Mathf.Max(maxValue, 0.0001f);
          if ((double) maxValue > (double) num3)
            num3 = maxValue;
          float num4 = chart.m_Type != Chart.ChartType.Line ? 1f / maxValue : (float) (1.0 / ((double) maxValue * (1.04999995231628 + (double) index * 0.0500000007450581)));
          scale[index] = num4;
        }
        if (chart.m_Type == Chart.ChartType.Line)
          chart.m_Data.AssignScale(scale);
        if (chart.m_Area == ProfilerArea.NetworkMessages || chart.m_Area == ProfilerArea.NetworkOperations)
        {
          for (int index = 0; index < chart.m_Series.Length; ++index)
            scale[index] = 0.9f / num3;
          chart.m_Data.AssignScale(scale);
          chart.m_Data.maxValue = num3;
        }
        chart.m_Data.Assign(chart.m_Series, num2, firstSelectableFrame);
      }
      bool flag = ProfilerDriver.selectedPropertyPath != string.Empty && this.m_CurrentArea == ProfilerArea.CPU;
      ProfilerChart chart1 = this.m_Charts[0];
      if (flag)
      {
        chart1.m_Data.hasOverlay = true;
        foreach (ChartSeries chartSeries in chart1.m_Series)
        {
          int statisticsIdentifier = ProfilerDriver.GetStatisticsIdentifier("Selected" + chartSeries.identifierName);
          chartSeries.CreateOverlayData();
          float maxValue;
          ProfilerDriver.GetStatisticsValues(statisticsIdentifier, num2, chart1.m_DataScale, chartSeries.overlayData, out maxValue);
        }
      }
      else
        chart1.m_Data.hasOverlay = false;
      for (ProfilerArea profilerArea = ProfilerArea.CPU; profilerArea <= ProfilerArea.GPU; ++profilerArea)
      {
        ProfilerChart chart2 = this.m_Charts[(int) profilerArea];
        float val1 = 0.0f;
        float num3 = 0.0f;
        for (int index1 = 0; index1 < num1; ++index1)
        {
          float num4 = 0.0f;
          for (int index2 = 0; index2 < chart2.m_Series.Length; ++index2)
          {
            if (chart2.m_Series[index2].enabled)
              num4 += chart2.m_Series[index2].data[index1];
          }
          if ((double) num4 > (double) val1)
            val1 = num4;
          if ((double) num4 > (double) num3 && index1 + num2 >= firstSelectableFrame + 1)
            num3 = num4;
        }
        if ((double) num3 != 0.0)
          val1 = num3;
        float num5 = Math.Min(val1, this.m_ChartMaxClamp);
        if ((double) this.m_ChartOldMax[(int) profilerArea] > 0.0)
          num5 = Mathf.Lerp(this.m_ChartOldMax[(int) profilerArea], num5, 0.4f);
        this.m_ChartOldMax[(int) profilerArea] = num5;
        chart2.m_Data.AssignScale(new float[1]
        {
          (float) (1.0 / (double) num5)
        });
        ProfilerWindow.UpdateChartGrid(num5, chart2.m_Data);
      }
      string str = (string) null;
      if (ProfilerDriver.isGPUProfilerBuggyOnDriver)
        str = "Graphics card driver returned invalid timing information. Please update to a newer version if available.";
      else if (!ProfilerDriver.isGPUProfilerSupported)
      {
        str = "GPU profiling is not supported by the graphics card driver. Please update to a newer version if available.";
        if (Application.platform == RuntimePlatform.OSXEditor)
          str = ProfilerDriver.isGPUProfilerSupportedByOS ? "GPU profiling is not supported by the graphics card driver (or it was disabled because of driver bugs)." : "GPU profiling requires Mac OS X 10.7 (Lion) and a capable video card. GPU profiling is currently not supported on mobile.";
      }
      this.m_Charts[1].m_Chart.m_NotSupportedWarning = str;
    }

    private void AddAreaClick(object userData, string[] options, int selected)
    {
      this.m_Charts[selected].active = true;
    }

    private void DrawMainToolbar()
    {
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      Rect rect = GUILayoutUtility.GetRect(ProfilerWindow.ms_Styles.addArea, EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(120f) });
      if (EditorGUI.ButtonMouseDown(rect, ProfilerWindow.ms_Styles.addArea, FocusType.Native, EditorStyles.toolbarDropDown))
      {
        int length = this.m_Charts.Length;
        string[] options = new string[length];
        bool[] enabled = new bool[length];
        for (int index = 0; index < length; ++index)
        {
          options[index] = ((ProfilerArea) index).ToString();
          enabled[index] = !this.m_Charts[index].active;
        }
        EditorUtility.DisplayCustomMenu(rect, options, enabled, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.AddAreaClick), (object) null);
      }
      GUILayout.FlexibleSpace();
      this.m_Recording = GUILayout.Toggle(this.m_Recording, ProfilerWindow.ms_Styles.profilerRecord, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      Profiler.enabled = this.m_Recording;
      ProfilerWindow.SetProfileDeepScripts(GUILayout.Toggle(ProfilerDriver.deepProfiling, ProfilerWindow.ms_Styles.deepProfile, EditorStyles.toolbarButton, new GUILayoutOption[0]));
      ProfilerDriver.profileEditor = GUILayout.Toggle(ProfilerDriver.profileEditor, ProfilerWindow.ms_Styles.profileEditor, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      this.m_AttachProfilerUI.OnGUILayout((EditorWindow) this);
      if (GUILayout.Button(ProfilerWindow.ms_Styles.clearData, EditorStyles.toolbarButton, new GUILayoutOption[0]))
      {
        ProfilerDriver.ClearAllFrames();
        NetworkDetailStats.m_NetworkOperations.Clear();
      }
      GUILayout.Space(5f);
      GUILayout.FlexibleSpace();
      this.FrameNavigationControls();
      GUILayout.EndHorizontal();
    }

    private void FrameNavigationControls()
    {
      if (this.m_CurrentFrame > ProfilerDriver.lastFrameIndex)
        this.SetCurrentFrameDontPause(ProfilerDriver.lastFrameIndex);
      GUILayout.Label(ProfilerWindow.ms_Styles.frame, EditorStyles.miniLabel, new GUILayoutOption[0]);
      GUILayout.Label("   " + this.PickFrameLabel(), EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        GUILayout.Width(100f)
      });
      GUI.enabled = ProfilerDriver.GetPreviousFrameIndex(this.m_CurrentFrame) != -1;
      if (GUILayout.Button(ProfilerWindow.ms_Styles.prevFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.PrevFrame();
      GUI.enabled = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame) != -1;
      if (GUILayout.Button(ProfilerWindow.ms_Styles.nextFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        this.NextFrame();
      GUI.enabled = true;
      GUILayout.Space(10f);
      if (!GUILayout.Button(ProfilerWindow.ms_Styles.currentFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        return;
      this.SetCurrentFrame(-1);
      this.m_LastFrameFromTick = ProfilerDriver.lastFrameIndex;
    }

    private static void DrawOtherToolbar()
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void DrawOverviewText(ProfilerArea area)
    {
      this.m_PaneScroll[(int) area] = GUILayout.BeginScrollView(this.m_PaneScroll[(int) area], ProfilerWindow.ms_Styles.background);
      GUILayout.Label(ProfilerDriver.GetOverviewText(area, this.GetActiveVisibleFrameIndex()), EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      GUILayout.EndScrollView();
    }

    private void DrawPane(ProfilerArea area)
    {
      ProfilerWindow.DrawOtherToolbar();
      this.DrawOverviewText(area);
    }

    private void SetCurrentFrameDontPause(int frame)
    {
      this.m_CurrentFrame = frame;
    }

    private void SetCurrentFrame(int frame)
    {
      if (frame != -1 && Profiler.enabled && (!ProfilerDriver.profileEditor && this.m_CurrentFrame != frame) && EditorApplication.isPlayingOrWillChangePlaymode)
        EditorApplication.isPaused = true;
      if (ProfilerInstrumentationPopup.InstrumentationEnabled)
        ProfilerInstrumentationPopup.UpdateInstrumentableFunctions();
      this.SetCurrentFrameDontPause(frame);
    }

    private void OnGUI()
    {
      this.CheckForPlatformModuleChange();
      if (ProfilerWindow.ms_Styles == null)
        ProfilerWindow.ms_Styles = new ProfilerWindow.Styles();
      this.DrawMainToolbar();
      SplitterGUILayout.BeginVerticalSplit(this.m_VertSplit);
      this.m_GraphPos = EditorGUILayout.BeginScrollView(this.m_GraphPos, ProfilerWindow.ms_Styles.profilerGraphBackground, new GUILayoutOption[0]);
      if (this.m_PrevLastFrame != ProfilerDriver.lastFrameIndex)
      {
        this.UpdateCharts();
        this.m_PrevLastFrame = ProfilerDriver.lastFrameIndex;
      }
      int num = this.m_CurrentFrame;
      Chart.ChartAction[] chartActionArray = new Chart.ChartAction[this.m_Charts.Length];
      for (int index = 0; index < this.m_Charts.Length; ++index)
      {
        ProfilerChart chart = this.m_Charts[index];
        if (chart.active)
          num = chart.DoChartGUI(num, this.m_CurrentArea, out chartActionArray[index]);
      }
      bool flag = false;
      if (num != this.m_CurrentFrame)
      {
        this.SetCurrentFrame(num);
        flag = true;
      }
      for (int index = 0; index < this.m_Charts.Length; ++index)
      {
        ProfilerChart chart = this.m_Charts[index];
        if (chart.active)
        {
          if (chartActionArray[index] == Chart.ChartAction.Closed)
          {
            if (this.m_CurrentArea == (ProfilerArea) index)
              this.m_CurrentArea = ProfilerArea.CPU;
            chart.active = false;
          }
          else if (chartActionArray[index] == Chart.ChartAction.Activated)
          {
            this.m_CurrentArea = (ProfilerArea) index;
            if (this.m_CurrentArea != ProfilerArea.CPU && this.m_CPUHierarchyGUI.selectedIndex != -1)
              this.ClearSelectedPropertyPath();
            flag = true;
          }
        }
      }
      if (flag)
      {
        this.Repaint();
        GUIUtility.ExitGUI();
      }
      GUILayout.EndScrollView();
      GUILayout.BeginVertical();
      switch (this.m_CurrentArea)
      {
        case ProfilerArea.CPU:
          this.DrawCPUOrRenderingPane(this.m_CPUHierarchyGUI, this.m_CPUDetailHierarchyGUI, this.m_CPUTimelineGUI);
          break;
        case ProfilerArea.GPU:
          this.DrawCPUOrRenderingPane(this.m_GPUHierarchyGUI, this.m_GPUDetailHierarchyGUI, (ProfilerTimelineGUI) null);
          break;
        case ProfilerArea.Memory:
          this.DrawMemoryPane(this.m_ViewSplit);
          break;
        case ProfilerArea.Audio:
          this.DrawAudioPane();
          break;
        case ProfilerArea.NetworkMessages:
          this.DrawPane(this.m_CurrentArea);
          break;
        case ProfilerArea.NetworkOperations:
          this.DrawNetworkOperationsPane();
          break;
        default:
          this.DrawPane(this.m_CurrentArea);
          break;
      }
      GUILayout.EndVertical();
      SplitterGUILayout.EndVerticalSplit();
    }

    void IProfilerWindowController.Repaint()
    {
      this.Repaint();
    }

    internal class Styles
    {
      public GUIContent addArea = EditorGUIUtility.TextContent("Add Profiler|Add a profiler area");
      public GUIContent deepProfile = EditorGUIUtility.TextContent("Deep Profile|Instrument all mono calls to investigate scripts");
      public GUIContent profileEditor = EditorGUIUtility.TextContent("Profile Editor|Enable profiling of the editor");
      public GUIContent noData = EditorGUIUtility.TextContent("No frame data available");
      public GUIContent frameDebugger = EditorGUIUtility.TextContent("Frame Debugger|Open Frame Debugger");
      public GUIContent noFrameDebugger = EditorGUIUtility.TextContent("Frame Debugger|Open Frame Debugger (Current frame needs to be selected)");
      public GUIContent gatherObjectReferences = EditorGUIUtility.TextContent("Gather object references|Collect reference information to see where objects are referenced from. Disable this to save memory");
      public GUIContent profilerRecord = EditorGUIUtility.TextContentWithIcon("Record|Record profiling information", "Profiler.Record");
      public GUIContent profilerInstrumentation = EditorGUIUtility.TextContent("Instrumentation|Add Profiler Instrumentation to selected functions");
      public GUIContent prevFrame = EditorGUIUtility.IconContent("Profiler.PrevFrame", "|Go back one frame");
      public GUIContent nextFrame = EditorGUIUtility.IconContent("Profiler.NextFrame", "|Go one frame forwards");
      public GUIContent currentFrame = EditorGUIUtility.TextContent("Current|Go to current frame");
      public GUIContent frame = EditorGUIUtility.TextContent("Frame: ");
      public GUIContent clearData = EditorGUIUtility.TextContent("Clear");
      public GUIContent[] reasons = ProfilerWindow.Styles.GetLocalizedReasons();
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle header = (GUIStyle) "OL title";
      public GUIStyle label = (GUIStyle) "OL label";
      public GUIStyle entryEven = (GUIStyle) "OL EntryBackEven";
      public GUIStyle entryOdd = (GUIStyle) "OL EntryBackOdd";
      public GUIStyle foldout = (GUIStyle) "IN foldout";
      public GUIStyle profilerGraphBackground = (GUIStyle) "ProfilerScrollviewBackground";

      public Styles()
      {
        this.profilerGraphBackground.overflow.left = -170;
      }

      internal static GUIContent[] GetLocalizedReasons()
      {
        return new GUIContent[11]{ EditorGUIUtility.TextContent("Scene object (Unloaded by loading a new scene or destroying it)"), EditorGUIUtility.TextContent("Builtin Resource (Never unloaded)"), EditorGUIUtility.TextContent("Object is marked Don't Save. (Must be explicitly destroyed or it will leak)"), EditorGUIUtility.TextContent("Asset is dirty and must be saved first (Editor only)"), null, EditorGUIUtility.TextContent("Asset type created from code or stored in the scene, referenced from native code."), EditorGUIUtility.TextContent("Asset type created from code or stored in the scene, referenced from scripts and native code."), null, EditorGUIUtility.TextContent("Asset referenced from native code."), EditorGUIUtility.TextContent("Asset referenced from scripts and native code."), EditorGUIUtility.TextContent("Not Applicable") };
      }
    }
  }
}
