// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerHierarchyGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class ProfilerHierarchyGUI
  {
    private static int hierarchyViewHash = "HierarchyView".GetHashCode();
    private Vector2 m_TextScroll = Vector2.zero;
    private SerializedStringTable m_ExpandedHash = new SerializedStringTable();
    private int m_SelectedIndex = -1;
    private ProfilerColumn m_SortType = ProfilerColumn.TotalTime;
    private string m_DetailViewSelectedProperty = string.Empty;
    private const float kRowHeight = 16f;
    private const float kFoldoutSize = 14f;
    private const float kIndent = 16f;
    private const float kSmallMargin = 4f;
    private const float kBaseIndent = 4f;
    private const float kInstrumentationButtonWidth = 30f;
    private const float kInstrumentationButtonOffset = 5f;
    private const int kFirst = -999999;
    private const int kLast = 999999;
    private const float kScrollbarWidth = 16f;
    protected static ProfilerHierarchyGUI.Styles ms_Styles;
    private IProfilerWindowController m_Window;
    private SplitterState m_Splitter;
    private ProfilerColumn[] m_ColumnsToShow;
    private string[] m_ColumnNames;
    private bool[] m_VisibleColumns;
    private float[] m_SplitterRelativeSizes;
    private int[] m_SplitterMinWidths;
    private string m_ColumnSettingsName;
    private GUIContent[] m_HeaderContent;
    private GUIContent m_SearchHeader;
    private bool m_ExpandAll;
    private int m_ScrollViewHeight;
    private int m_DoScroll;
    private bool m_DetailPane;
    private ProfilerHierarchyGUI.SearchResults m_SearchResults;
    private bool m_SetKeyboardFocus;

    protected static ProfilerHierarchyGUI.Styles styles
    {
      get
      {
        return ProfilerHierarchyGUI.ms_Styles ?? (ProfilerHierarchyGUI.ms_Styles = new ProfilerHierarchyGUI.Styles());
      }
    }

    public int selectedIndex
    {
      get
      {
        if (this.IsSearchActive())
          return this.m_SearchResults.selectedSearchIndex;
        return this.m_SelectedIndex;
      }
      set
      {
        if (this.IsSearchActive())
          this.m_SearchResults.selectedSearchIndex = value;
        else
          this.m_SelectedIndex = value;
      }
    }

    public ProfilerColumn sortType
    {
      get
      {
        return this.m_SortType;
      }
      private set
      {
        this.m_SortType = value;
      }
    }

    public ProfilerHierarchyGUI(IProfilerWindowController window, string columnSettingsName, ProfilerColumn[] columnsToShow, string[] columnNames, bool detailPane, ProfilerColumn sort)
    {
      this.m_Window = window;
      this.m_ColumnNames = columnNames;
      this.m_ColumnSettingsName = columnSettingsName;
      this.m_ColumnsToShow = columnsToShow;
      this.m_DetailPane = detailPane;
      this.m_SortType = sort;
      this.m_HeaderContent = new GUIContent[columnNames.Length];
      this.m_Splitter = (SplitterState) null;
      for (int index = 0; index < this.m_HeaderContent.Length; ++index)
        this.m_HeaderContent[index] = !this.m_ColumnNames[index].StartsWith("|") ? new GUIContent(this.m_ColumnNames[index]) : EditorGUIUtility.IconContent("ProfilerColumn." + columnsToShow[index].ToString(), this.m_ColumnNames[index]);
      if (columnsToShow.Length != columnNames.Length)
        throw new ArgumentException("Number of columns to show does not match number of column names.");
      this.m_SearchHeader = new GUIContent("Search");
      this.m_VisibleColumns = new bool[columnNames.Length];
      for (int index = 0; index < this.m_VisibleColumns.Length; ++index)
        this.m_VisibleColumns[index] = true;
      this.m_SearchResults = new ProfilerHierarchyGUI.SearchResults();
      this.m_SearchResults.Init(100);
      this.m_Window.Repaint();
    }

    public void SetKeyboardFocus()
    {
      this.m_SetKeyboardFocus = true;
    }

    public void SelectFirstRow()
    {
      this.MoveSelection(-999999);
    }

    private void SetupSplitter()
    {
      if (this.m_Splitter != null && this.m_SplitterMinWidths != null)
        return;
      this.m_SplitterRelativeSizes = new float[this.m_ColumnNames.Length + 1];
      this.m_SplitterMinWidths = new int[this.m_ColumnNames.Length + 1];
      for (int index = 0; index < this.m_ColumnNames.Length; ++index)
      {
        this.m_SplitterMinWidths[index] = (int) ProfilerHierarchyGUI.styles.header.CalcSize(this.m_HeaderContent[index]).x;
        this.m_SplitterRelativeSizes[index] = 70f;
        if ((UnityEngine.Object) this.m_HeaderContent[index].image != (UnityEngine.Object) null)
          this.m_SplitterRelativeSizes[index] = 1f;
      }
      this.m_SplitterMinWidths[this.m_ColumnNames.Length] = 16;
      this.m_SplitterRelativeSizes[this.m_ColumnNames.Length] = 0.0f;
      if (this.m_ColumnsToShow[0] == ProfilerColumn.FunctionName)
      {
        this.m_SplitterRelativeSizes[0] = 400f;
        this.m_SplitterMinWidths[0] = 100;
      }
      this.m_Splitter = new SplitterState(this.m_SplitterRelativeSizes, this.m_SplitterMinWidths, (int[]) null);
      string str = EditorPrefs.GetString(this.m_ColumnSettingsName);
      for (int index = 0; index < this.m_ColumnNames.Length; ++index)
      {
        if (index < str.Length && (int) str[index] == 48)
          this.SetColumnVisible(index, false);
      }
    }

    public ProfilerProperty GetDetailedProperty(ProfilerProperty property)
    {
      bool enterChildren = true;
      string selectedPropertyPath = ProfilerDriver.selectedPropertyPath;
      while (property.Next(enterChildren))
      {
        string propertyPath = property.propertyPath;
        if (propertyPath == selectedPropertyPath)
        {
          ProfilerProperty profilerProperty = new ProfilerProperty();
          profilerProperty.InitializeDetailProperty(property);
          return profilerProperty;
        }
        if (property.HasChildren)
          enterChildren = this.IsExpanded(propertyPath);
      }
      return (ProfilerProperty) null;
    }

    private void DoScroll()
    {
      this.m_DoScroll = 2;
    }

    public void FrameSelection()
    {
      if (string.IsNullOrEmpty(ProfilerDriver.selectedPropertyPath))
        return;
      this.m_Window.SetSearch(string.Empty);
      string[] strArray = ProfilerDriver.selectedPropertyPath.Split('/');
      string expandedName = strArray[0];
      for (int index = 1; index < strArray.Length; ++index)
      {
        this.SetExpanded(expandedName, true);
        expandedName = expandedName + "/" + strArray[index];
      }
      this.DoScroll();
    }

    private void MoveSelection(int steps)
    {
      if (this.IsSearchActive())
      {
        this.m_SearchResults.MoveSelection(steps, this);
      }
      else
      {
        int num1 = this.m_SelectedIndex + steps;
        if (num1 < 0)
          num1 = 0;
        ProfilerProperty property = this.m_Window.CreateProperty(this.m_DetailPane);
        if (this.m_DetailPane)
        {
          ProfilerProperty detailedProperty = this.GetDetailedProperty(property);
          property.Cleanup();
          property = detailedProperty;
        }
        if (property == null)
          return;
        bool enterChildren = true;
        int num2 = 0;
        int instanceId = -1;
        while (property.Next(enterChildren))
        {
          if (this.m_DetailPane && property.instanceIDs != null && (property.instanceIDs.Length > 0 && property.instanceIDs[0] != 0))
            instanceId = property.instanceIDs[0];
          if (num2 != num1)
          {
            if (property.HasChildren)
              enterChildren = !this.m_DetailPane && this.IsExpanded(property.propertyPath);
            ++num2;
          }
          else
            break;
        }
        if (this.m_DetailPane)
          this.m_DetailViewSelectedProperty = ProfilerHierarchyGUI.DetailViewSelectedPropertyPath(property, instanceId);
        else
          this.m_Window.SetSelectedPropertyPath(property.propertyPath);
        property.Cleanup();
      }
    }

    private void SetExpanded(string expandedName, bool expanded)
    {
      if (expanded == this.IsExpanded(expandedName))
        return;
      if (expanded)
        this.m_ExpandedHash.Set(expandedName);
      else
        this.m_ExpandedHash.Remove(expandedName);
    }

    private void HandleKeyboard(int id)
    {
      Event current = Event.current;
      if (current.GetTypeForControl(id) != EventType.KeyDown || id != GUIUtility.keyboardControl)
        return;
      bool flag = this.IsSearchActive();
      int steps = 0;
      switch (current.keyCode)
      {
        case KeyCode.UpArrow:
          steps = -1;
          break;
        case KeyCode.DownArrow:
          steps = 1;
          break;
        case KeyCode.RightArrow:
          if (!flag)
          {
            this.SetExpanded(ProfilerDriver.selectedPropertyPath, true);
            break;
          }
          break;
        case KeyCode.LeftArrow:
          if (!flag)
          {
            this.SetExpanded(ProfilerDriver.selectedPropertyPath, false);
            break;
          }
          break;
        case KeyCode.Insert:
          return;
        case KeyCode.Home:
          steps = -999999;
          break;
        case KeyCode.End:
          steps = 999999;
          break;
        case KeyCode.PageUp:
          if (Application.platform == RuntimePlatform.OSXEditor)
          {
            this.m_TextScroll.y -= (float) this.m_ScrollViewHeight;
            if ((double) this.m_TextScroll.y < 0.0)
              this.m_TextScroll.y = 0.0f;
            current.Use();
            return;
          }
          steps = -Mathf.RoundToInt((float) this.m_ScrollViewHeight / 16f);
          break;
        case KeyCode.PageDown:
          if (Application.platform == RuntimePlatform.OSXEditor)
          {
            this.m_TextScroll.y += (float) this.m_ScrollViewHeight;
            current.Use();
            return;
          }
          steps = Mathf.RoundToInt((float) this.m_ScrollViewHeight / 16f);
          break;
        default:
          return;
      }
      if (steps != 0)
        this.MoveSelection(steps);
      this.DoScroll();
      current.Use();
    }

    private bool IsSearchActive()
    {
      return this.m_Window.IsSearching();
    }

    private void DrawColumnsHeader(string searchString)
    {
      bool flag = false;
      GUILayout.BeginHorizontal();
      if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
      {
        flag = true;
        Event.current.type = EventType.Used;
      }
      SplitterGUILayout.BeginHorizontalSplit(this.m_Splitter, GUIStyle.none, new GUILayoutOption[0]);
      this.DrawTitle(!this.IsSearchActive() ? this.m_HeaderContent[0] : this.m_SearchHeader, 0);
      for (int index = 1; index < this.m_ColumnNames.Length; ++index)
        this.DrawTitle(this.m_HeaderContent[index], index);
      SplitterGUILayout.EndHorizontalSplit();
      GUILayout.EndHorizontal();
      if (flag)
      {
        Event.current.type = EventType.MouseDown;
        this.HandleHeaderMouse(GUILayoutUtility.GetLastRect());
      }
      GUILayout.Space(1f);
    }

    private bool IsExpanded(string expanded)
    {
      if (this.m_ExpandAll)
        return true;
      return this.m_ExpandedHash.Contains(expanded);
    }

    private void SetExpanded(ProfilerProperty property, bool expanded)
    {
      this.SetExpanded(property.propertyPath, expanded);
    }

    private int DrawProfilingData(ProfilerProperty property, string searchString, int id)
    {
      int num = !this.IsSearchActive() ? this.DrawTreeView(property, id) : this.DrawSearchResult(property, searchString, id);
      if (num == 0)
      {
        Rect rowRect = this.GetRowRect(0);
        rowRect.height = 1f;
        GUI.Label(rowRect, GUIContent.none, ProfilerHierarchyGUI.styles.entryEven);
      }
      return num;
    }

    private int DrawSearchResult(ProfilerProperty property, string searchString, int id)
    {
      if (!this.AllowSearching())
      {
        this.DoSearchingDisabledInfoGUI();
        return 0;
      }
      this.m_SearchResults.Filter(property, this.m_ColumnsToShow, searchString, this.m_Window.GetActiveVisibleFrameIndex(), this.sortType);
      this.m_SearchResults.Draw(this, id);
      return this.m_SearchResults.numRows;
    }

    private int DrawTreeView(ProfilerProperty property, int id)
    {
      this.m_SelectedIndex = -1;
      bool enterChildren = true;
      int rowCount = 0;
      string selectedPropertyPath = ProfilerDriver.selectedPropertyPath;
      while (property.Next(enterChildren))
      {
        string propertyPath = property.propertyPath;
        bool selected = !this.m_DetailPane ? propertyPath == selectedPropertyPath : this.m_DetailViewSelectedProperty != string.Empty && this.m_DetailViewSelectedProperty == ProfilerHierarchyGUI.DetailViewSelectedPropertyPath(property);
        if (selected)
          this.m_SelectedIndex = rowCount;
        enterChildren = ((Event.current.type != EventType.Layout ? 1 : 0) & (this.m_ScrollViewHeight == 0 ? 1 : ((double) rowCount * 16.0 > (double) this.m_ScrollViewHeight + (double) this.m_TextScroll.y ? 0 : ((double) (rowCount + 1) * 16.0 > (double) this.m_TextScroll.y ? 1 : 0)))) == 0 ? property.HasChildren && this.IsExpanded(propertyPath) : this.DrawProfileDataItem(property, rowCount, selected, id);
        ++rowCount;
      }
      return rowCount;
    }

    private void DoSearchingDisabledInfoGUI()
    {
      EditorGUI.BeginDisabledGroup(true);
      TextAnchor alignment = EditorStyles.label.alignment;
      EditorStyles.label.alignment = TextAnchor.MiddleCenter;
      GUI.Label(new Rect(0.0f, 10f, GUIClip.visibleRect.width, 30f), ProfilerHierarchyGUI.styles.disabledSearchText, EditorStyles.label);
      EditorStyles.label.alignment = alignment;
      EditorGUI.EndDisabledGroup();
    }

    private bool AllowSearching()
    {
      return !Profiler.enabled || !ProfilerDriver.profileEditor && !EditorApplication.isPlaying || !ProfilerDriver.deepProfiling;
    }

    private void UnselectIfClickedOnEmptyArea(int rowCount)
    {
      if (Event.current.type != EventType.MouseDown || (double) Event.current.mousePosition.y <= (double) GUILayoutUtility.GetRect(GUIClip.visibleRect.width, (float) (16.0 * (double) rowCount), new GUILayoutOption[1]{ GUILayout.MinHeight(16f * (float) rowCount) }).y || (double) Event.current.mousePosition.y >= (double) Screen.height)
        return;
      if (!this.m_DetailPane)
        this.m_Window.ClearSelectedPropertyPath();
      else
        this.m_DetailViewSelectedProperty = string.Empty;
      Event.current.Use();
    }

    private void HandleHeaderMouse(Rect columnHeaderRect)
    {
      Event current = Event.current;
      if (current.type != EventType.MouseDown || current.button != 1 || !columnHeaderRect.Contains(current.mousePosition))
        return;
      GUIUtility.hotControl = 0;
      EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), this.m_ColumnNames, this.GetVisibleDropDownIndexList(), new EditorUtility.SelectMenuItemFunction(this.ColumnContextClick), (object) null);
      current.Use();
    }

    private void SetColumnVisible(int index, bool enabled)
    {
      this.SetupSplitter();
      if (index == 0 || this.m_VisibleColumns[index] == enabled)
        return;
      this.m_VisibleColumns[index] = enabled;
      int index1 = 0;
      for (int index2 = 0; index2 < index; ++index2)
      {
        if (this.ColIsVisible(index2))
          ++index1;
      }
      if (enabled)
      {
        ArrayUtility.Insert<float>(ref this.m_Splitter.relativeSizes, index1, this.m_SplitterRelativeSizes[index]);
        ArrayUtility.Insert<int>(ref this.m_Splitter.minSizes, index1, this.m_SplitterMinWidths[index]);
      }
      else
      {
        ArrayUtility.RemoveAt<float>(ref this.m_Splitter.relativeSizes, index1);
        ArrayUtility.RemoveAt<int>(ref this.m_Splitter.minSizes, index1);
      }
      this.m_Splitter = new SplitterState(this.m_Splitter.relativeSizes, this.m_Splitter.minSizes, (int[]) null);
      this.SaveColumns();
    }

    private int[] GetVisibleDropDownIndexList()
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < this.m_ColumnNames.Length; ++index)
      {
        if (this.m_VisibleColumns[index])
          intList.Add(index);
      }
      return intList.ToArray();
    }

    private void SaveColumns()
    {
      string empty = string.Empty;
      for (int index = 0; index < this.m_VisibleColumns.Length; ++index)
        empty += (string) (object) (char) (!this.ColIsVisible(index) ? 48 : 49);
      EditorPrefs.SetString(this.m_ColumnSettingsName, empty);
    }

    private bool ColIsVisible(int index)
    {
      if (index < 0 || index > this.m_VisibleColumns.Length)
        return false;
      return this.m_VisibleColumns[index];
    }

    private void ColumnContextClick(object userData, string[] options, int selected)
    {
      this.SetColumnVisible(selected, !this.ColIsVisible(selected));
    }

    protected void DrawTextColumn(ref Rect currentRect, string text, int index, float margin, bool selected)
    {
      if (index != 0)
        currentRect.x += (float) this.m_Splitter.realSizes[index - 1];
      currentRect.x += margin;
      currentRect.width = (float) this.m_Splitter.realSizes[index] - margin;
      ProfilerHierarchyGUI.styles.numberLabel.Draw(currentRect, text, false, false, false, selected);
      currentRect.x -= margin;
    }

    private static string DetailViewSelectedPropertyPath(ProfilerProperty property)
    {
      if (property == null || property.instanceIDs == null || (property.instanceIDs.Length == 0 || property.instanceIDs[0] == 0))
        return string.Empty;
      return ProfilerHierarchyGUI.DetailViewSelectedPropertyPath(property, property.instanceIDs[0]);
    }

    private static string DetailViewSelectedPropertyPath(ProfilerProperty property, int instanceId)
    {
      return property.propertyPath + "/" + (object) instanceId;
    }

    private Rect GetRowRect(int rowIndex)
    {
      return new Rect(1f, 16f * (float) rowIndex, GUIClip.visibleRect.width, 16f);
    }

    private GUIStyle GetRowBackgroundStyle(int rowIndex)
    {
      if (rowIndex % 2 == 0)
        return ProfilerHierarchyGUI.styles.entryEven;
      return ProfilerHierarchyGUI.styles.entryOdd;
    }

    private void RowMouseDown(string propertyPath)
    {
      if (propertyPath == ProfilerDriver.selectedPropertyPath)
        this.m_Window.ClearSelectedPropertyPath();
      else
        this.m_Window.SetSelectedPropertyPath(propertyPath);
    }

    private bool DrawProfileDataItem(ProfilerProperty property, int rowCount, bool selected, int id)
    {
      bool expanded = false;
      Event current = Event.current;
      Rect rowRect = this.GetRowRect(rowCount);
      Rect currentRect = rowRect;
      GUIStyle rowBackgroundStyle = this.GetRowBackgroundStyle(rowCount);
      if (current.type == EventType.Repaint)
        rowBackgroundStyle.Draw(currentRect, GUIContent.none, false, false, selected, false);
      float num = (float) ((double) property.depth * 16.0 + 4.0);
      if (property.HasChildren)
      {
        bool flag = this.IsExpanded(property.propertyPath);
        GUI.changed = false;
        float x = num - 14f;
        expanded = GUI.Toggle(new Rect(x, currentRect.y, 14f, 16f), flag, GUIContent.none, ProfilerHierarchyGUI.styles.foldout);
        if (GUI.changed)
          this.SetExpanded(property, expanded);
        num = x + 16f;
      }
      string column = property.GetColumn(this.m_ColumnsToShow[0]);
      if (current.type == EventType.Repaint)
        this.DrawTextColumn(ref currentRect, column, 0, this.m_ColumnsToShow[0] != ProfilerColumn.FunctionName ? 4f : num, selected);
      if (ProfilerInstrumentationPopup.InstrumentationEnabled && ProfilerInstrumentationPopup.FunctionHasInstrumentationPopup(column))
      {
        Rect rect = new Rect(Mathf.Clamp((float) ((double) currentRect.x + (double) num + 5.0) + ProfilerHierarchyGUI.styles.numberLabel.CalcSize(new GUIContent(column)).x, 0.0f, (float) ((double) this.m_Splitter.realSizes[0] - 30.0 + 2.0)), currentRect.y, 30f, 16f);
        if (GUI.Button(rect, ProfilerHierarchyGUI.styles.instrumentationIcon, ProfilerHierarchyGUI.styles.miniPullDown))
          ProfilerInstrumentationPopup.Show(rect, column);
      }
      if (current.type == EventType.Repaint)
      {
        ProfilerHierarchyGUI.styles.numberLabel.alignment = TextAnchor.MiddleRight;
        int index1 = 1;
        for (int index2 = 1; index2 < this.m_VisibleColumns.Length; ++index2)
        {
          if (this.ColIsVisible(index2))
          {
            currentRect.x += (float) this.m_Splitter.realSizes[index1 - 1];
            currentRect.width = (float) this.m_Splitter.realSizes[index1] - 4f;
            ++index1;
            ProfilerHierarchyGUI.styles.numberLabel.Draw(currentRect, property.GetColumn(this.m_ColumnsToShow[index2]), false, false, false, selected);
          }
        }
        ProfilerHierarchyGUI.styles.numberLabel.alignment = TextAnchor.MiddleLeft;
      }
      if (current.type == EventType.MouseDown && rowRect.Contains(current.mousePosition))
      {
        GUIUtility.hotControl = 0;
        if (!EditorGUI.actionKey)
        {
          if (this.m_DetailPane)
          {
            if (current.clickCount == 1 && property.instanceIDs.Length > 0)
            {
              string str = ProfilerHierarchyGUI.DetailViewSelectedPropertyPath(property);
              if (this.m_DetailViewSelectedProperty != str)
              {
                this.m_DetailViewSelectedProperty = str;
                UnityEngine.Object gameObject = EditorUtility.InstanceIDToObject(property.instanceIDs[0]);
                if (gameObject is Component)
                  gameObject = (UnityEngine.Object) ((Component) gameObject).gameObject;
                if (gameObject != (UnityEngine.Object) null)
                  EditorGUIUtility.PingObject(gameObject.GetInstanceID());
              }
              else
                this.m_DetailViewSelectedProperty = string.Empty;
            }
            else if (current.clickCount == 2)
            {
              ProfilerHierarchyGUI.SelectObjectsInHierarchyView(property);
              this.m_DetailViewSelectedProperty = ProfilerHierarchyGUI.DetailViewSelectedPropertyPath(property);
            }
          }
          else
            this.RowMouseDown(property.propertyPath);
          this.DoScroll();
        }
        else if (!this.m_DetailPane)
          this.m_Window.ClearSelectedPropertyPath();
        else
          this.m_DetailViewSelectedProperty = string.Empty;
        GUIUtility.keyboardControl = id;
        current.Use();
      }
      if (selected && GUIUtility.keyboardControl == id && current.type == EventType.KeyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter))
        ProfilerHierarchyGUI.SelectObjectsInHierarchyView(property);
      return expanded;
    }

    private static void SelectObjectsInHierarchyView(ProfilerProperty property)
    {
      int[] instanceIds = property.instanceIDs;
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
      foreach (int instanceID in instanceIds)
      {
        UnityEngine.Object @object = EditorUtility.InstanceIDToObject(instanceID);
        Component component = @object as Component;
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          objectList.Add((UnityEngine.Object) component.gameObject);
        else if (@object != (UnityEngine.Object) null)
          objectList.Add(@object);
      }
      if (objectList.Count == 0)
        return;
      Selection.objects = objectList.ToArray();
    }

    private void DrawTitle(GUIContent name, int index)
    {
      if (!this.ColIsVisible(index))
        return;
      ProfilerColumn profilerColumn = this.m_ColumnsToShow[index];
      bool flag = this.sortType == profilerColumn;
      int num;
      if (index == 0)
        num = GUILayout.Toggle(flag, name, ProfilerHierarchyGUI.styles.header, new GUILayoutOption[0]) ? 1 : 0;
      else
        num = GUILayout.Toggle((flag ? 1 : 0) != 0, name, ProfilerHierarchyGUI.styles.rightHeader, new GUILayoutOption[1]
        {
          GUILayout.Width((float) this.m_SplitterMinWidths[index])
        }) ? 1 : 0;
      if (num == 0)
        return;
      this.sortType = profilerColumn;
    }

    private void DoScrolling()
    {
      if (this.m_DoScroll <= 0)
        return;
      --this.m_DoScroll;
      if (this.m_DoScroll == 0)
      {
        float max = 16f * (float) this.selectedIndex;
        this.m_TextScroll.y = Mathf.Clamp(this.m_TextScroll.y, (float) ((double) max - (double) this.m_ScrollViewHeight + 16.0), max);
      }
      else
        this.m_Window.Repaint();
    }

    public void DoGUI(ProfilerProperty property, string searchString, bool expandAll)
    {
      this.m_ExpandAll = expandAll;
      this.SetupSplitter();
      this.DoScrolling();
      int controlId = GUIUtility.GetControlID(ProfilerHierarchyGUI.hierarchyViewHash, FocusType.Keyboard);
      this.DrawColumnsHeader(searchString);
      this.m_TextScroll = EditorGUILayout.BeginScrollView(this.m_TextScroll, ProfilerHierarchyGUI.ms_Styles.background, new GUILayoutOption[0]);
      int rowCount = this.DrawProfilingData(property, searchString, controlId);
      property.Cleanup();
      this.UnselectIfClickedOnEmptyArea(rowCount);
      if (Event.current.type == EventType.Repaint)
        this.m_ScrollViewHeight = (int) GUIClip.visibleRect.height;
      GUILayout.EndScrollView();
      this.HandleKeyboard(controlId);
      if (!this.m_SetKeyboardFocus || Event.current.type != EventType.Repaint)
        return;
      this.m_SetKeyboardFocus = false;
      GUIUtility.keyboardControl = controlId;
      this.m_Window.Repaint();
    }

    internal class Styles
    {
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle header = (GUIStyle) "OL title";
      public GUIStyle rightHeader = (GUIStyle) "OL title TextRight";
      public GUIStyle entryEven = (GUIStyle) "OL EntryBackEven";
      public GUIStyle entryOdd = (GUIStyle) "OL EntryBackOdd";
      public GUIStyle numberLabel = (GUIStyle) "OL Label";
      public GUIStyle foldout = (GUIStyle) "IN foldout";
      public GUIStyle miniPullDown = (GUIStyle) "MiniPullDown";
      public GUIContent disabledSearchText = new GUIContent("Showing search results are disabled while recording with deep profiling.\nStop recording to view search results.");
      public GUIContent notShowingAllResults = new GUIContent("...", "Narrow your search. Not all search results can be shown.");
      public GUIContent instrumentationIcon = EditorGUIUtility.IconContent("Profiler.Record", "Record|Record profiling information");
    }

    internal class SearchResults
    {
      private ProfilerHierarchyGUI.SearchResults.SearchResult[] m_SearchResults;
      private int m_NumResultsUsed;
      private ProfilerColumn[] m_ColumnsToShow;
      private int m_SelectedSearchIndex;
      private bool m_FoundAllResults;
      private string m_LastSearchString;
      private int m_LastFrameIndex;
      private ProfilerColumn m_LastSortType;

      public int numRows
      {
        get
        {
          return this.m_NumResultsUsed + (!this.m_FoundAllResults ? 1 : 0);
        }
      }

      public int selectedSearchIndex
      {
        get
        {
          return this.m_SelectedSearchIndex;
        }
        set
        {
          this.m_SelectedSearchIndex = value >= this.m_NumResultsUsed ? -1 : value;
          if (this.m_SelectedSearchIndex < 0)
            return;
          string propertyPath = this.m_SearchResults[this.m_SelectedSearchIndex].propertyPath;
          if (!(propertyPath != ProfilerDriver.selectedPropertyPath))
            return;
          ProfilerDriver.selectedPropertyPath = propertyPath;
        }
      }

      public void Init(int maxNumberSearchResults)
      {
        this.m_SearchResults = new ProfilerHierarchyGUI.SearchResults.SearchResult[maxNumberSearchResults];
        this.m_NumResultsUsed = 0;
        this.m_LastSearchString = string.Empty;
        this.m_LastFrameIndex = -1;
        this.m_FoundAllResults = false;
        this.m_ColumnsToShow = (ProfilerColumn[]) null;
        this.m_SelectedSearchIndex = -1;
      }

      public void Filter(ProfilerProperty property, ProfilerColumn[] columns, string searchString, int frameIndex, ProfilerColumn sortType)
      {
        if (searchString == this.m_LastSearchString && frameIndex == this.m_LastFrameIndex && sortType == this.m_LastSortType)
          return;
        this.m_LastSearchString = searchString;
        this.m_LastFrameIndex = frameIndex;
        this.m_LastSortType = sortType;
        this.IterateProfilingData(property, columns, searchString);
      }

      private void IterateProfilingData(ProfilerProperty property, ProfilerColumn[] columns, string searchString)
      {
        this.m_NumResultsUsed = 0;
        this.m_ColumnsToShow = columns;
        this.m_FoundAllResults = true;
        this.m_SelectedSearchIndex = -1;
        int index1 = 0;
        string selectedPropertyPath = ProfilerDriver.selectedPropertyPath;
        while (property.Next(true))
        {
          if (index1 >= this.m_SearchResults.Length)
          {
            this.m_FoundAllResults = false;
            break;
          }
          string propertyPath = property.propertyPath;
          int startIndex = Mathf.Max(propertyPath.LastIndexOf('/'), 0);
          if (propertyPath.IndexOf(searchString, startIndex, StringComparison.CurrentCultureIgnoreCase) > -1)
          {
            string[] strArray = new string[this.m_ColumnsToShow.Length];
            for (int index2 = 0; index2 < this.m_ColumnsToShow.Length; ++index2)
              strArray[index2] = property.GetColumn(this.m_ColumnsToShow[index2]);
            this.m_SearchResults[index1].propertyPath = propertyPath;
            this.m_SearchResults[index1].columnValues = strArray;
            if (propertyPath == selectedPropertyPath)
              this.m_SelectedSearchIndex = index1;
            ++index1;
          }
        }
        this.m_NumResultsUsed = index1;
      }

      public void Draw(ProfilerHierarchyGUI gui, int controlID)
      {
        this.HandleCommandEvents(gui);
        Event current = Event.current;
        string selectedPropertyPath = ProfilerDriver.selectedPropertyPath;
        int firstRowVisible;
        int lastRowVisible;
        ProfilerHierarchyGUI.SearchResults.GetFirstAndLastRowVisible(this.m_NumResultsUsed, 16f, gui.m_TextScroll.y, (float) gui.m_ScrollViewHeight, out firstRowVisible, out lastRowVisible);
        for (int rowIndex = firstRowVisible; rowIndex <= lastRowVisible; ++rowIndex)
        {
          bool flag = selectedPropertyPath == this.m_SearchResults[rowIndex].propertyPath;
          Rect rowRect = gui.GetRowRect(rowIndex);
          GUIStyle rowBackgroundStyle = gui.GetRowBackgroundStyle(rowIndex);
          if (current.type == EventType.MouseDown && rowRect.Contains(current.mousePosition))
          {
            this.m_SelectedSearchIndex = rowIndex;
            gui.RowMouseDown(this.m_SearchResults[rowIndex].propertyPath);
            GUIUtility.keyboardControl = controlID;
            current.Use();
          }
          if (current.type == EventType.Repaint)
          {
            rowBackgroundStyle.Draw(rowRect, GUIContent.none, false, false, flag, GUIUtility.keyboardControl == controlID);
            if (rowRect.Contains(current.mousePosition))
            {
              string tooltip = this.m_SearchResults[rowIndex].propertyPath.Replace("/", "/\n");
              if (this.m_SelectedSearchIndex >= 0)
                tooltip += "\n\n(Press 'F' to frame selection)";
              GUI.Label(rowRect, GUIContent.Temp(string.Empty, tooltip));
            }
            gui.DrawTextColumn(ref rowRect, this.m_SearchResults[rowIndex].columnValues[0], 0, 4f, flag);
            ProfilerHierarchyGUI.styles.numberLabel.alignment = TextAnchor.MiddleRight;
            int index1 = 1;
            for (int index2 = 1; index2 < gui.m_VisibleColumns.Length; ++index2)
            {
              if (gui.ColIsVisible(index2))
              {
                rowRect.x += (float) gui.m_Splitter.realSizes[index1 - 1];
                rowRect.width = (float) gui.m_Splitter.realSizes[index1] - 4f;
                ++index1;
                ProfilerHierarchyGUI.styles.numberLabel.Draw(rowRect, this.m_SearchResults[rowIndex].columnValues[index2], false, false, false, flag);
              }
            }
            ProfilerHierarchyGUI.styles.numberLabel.alignment = TextAnchor.MiddleLeft;
          }
        }
        if (this.m_FoundAllResults || current.type != EventType.Repaint)
          return;
        int numResultsUsed = this.m_NumResultsUsed;
        Rect currentRect = new Rect(1f, 16f * (float) numResultsUsed, GUIClip.visibleRect.width, 16f);
        GUIStyle guiStyle = numResultsUsed % 2 != 0 ? ProfilerHierarchyGUI.styles.entryOdd : ProfilerHierarchyGUI.styles.entryEven;
        GUI.Label(currentRect, GUIContent.Temp(string.Empty, ProfilerHierarchyGUI.styles.notShowingAllResults.tooltip), GUIStyle.none);
        guiStyle.Draw(currentRect, GUIContent.none, false, false, false, false);
        gui.DrawTextColumn(ref currentRect, ProfilerHierarchyGUI.styles.notShowingAllResults.text, 0, 4f, false);
      }

      private static void GetFirstAndLastRowVisible(int numRows, float rowHeight, float scrollBarY, float scrollAreaHeight, out int firstRowVisible, out int lastRowVisible)
      {
        firstRowVisible = (int) Mathf.Floor(scrollBarY / rowHeight);
        lastRowVisible = firstRowVisible + (int) Mathf.Ceil(scrollAreaHeight / rowHeight);
        firstRowVisible = Mathf.Max(firstRowVisible, 0);
        lastRowVisible = Mathf.Min(lastRowVisible, numRows - 1);
      }

      public void MoveSelection(int steps, ProfilerHierarchyGUI gui)
      {
        int index = Mathf.Clamp(this.m_SelectedSearchIndex + steps, 0, this.m_NumResultsUsed - 1);
        if (index == this.m_SelectedSearchIndex)
          return;
        this.m_SelectedSearchIndex = index;
        gui.m_Window.SetSelectedPropertyPath(this.m_SearchResults[index].propertyPath);
      }

      private void HandleCommandEvents(ProfilerHierarchyGUI gui)
      {
        Event current = Event.current;
        EventType type = current.type;
        switch (type)
        {
          case EventType.ExecuteCommand:
          case EventType.ValidateCommand:
            bool flag = type == EventType.ExecuteCommand;
            if (!(Event.current.commandName == "FrameSelected"))
              break;
            if (flag)
              gui.FrameSelection();
            current.Use();
            break;
        }
      }

      private struct SearchResult
      {
        public string propertyPath;
        public string[] columnValues;
      }
    }
  }
}
