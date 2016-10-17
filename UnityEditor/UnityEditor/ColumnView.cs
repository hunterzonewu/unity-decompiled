// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColumnView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ColumnView
  {
    private string m_SearchText = string.Empty;
    public float columnWidth = 150f;
    public int minimumNumberOfColumns = 1;
    private int m_ColumnToFocusKeyboard = -1;
    private static ColumnView.Styles s_Styles;
    private readonly List<ListViewState> m_ListViewStates;
    private readonly List<int> m_CachedSelectedIndices;
    private Vector2 m_ScrollPosition;

    public string searchText
    {
      get
      {
        return this.m_SearchText;
      }
    }

    public bool isSearching
    {
      get
      {
        return this.searchText != string.Empty;
      }
    }

    public ColumnView()
    {
      this.m_ListViewStates = new List<ListViewState>();
      this.m_CachedSelectedIndices = new List<int>();
    }

    private static void InitStyles()
    {
      if (ColumnView.s_Styles != null)
        return;
      ColumnView.s_Styles = new ColumnView.Styles();
    }

    public void SetSelected(int column, int selectionIndex)
    {
      if (this.m_ListViewStates.Count == column)
        this.m_ListViewStates.Add(new ListViewState());
      if (this.m_CachedSelectedIndices.Count == column)
        this.m_CachedSelectedIndices.Add(-1);
      this.m_CachedSelectedIndices[column] = selectionIndex;
      this.m_ListViewStates[column].row = selectionIndex;
    }

    public void SetKeyboardFocusColumn(int column)
    {
      this.m_ColumnToFocusKeyboard = column;
    }

    public void OnGUI(List<ColumnViewElement> elements, ColumnView.ObjectColumnFunction previewColumnFunction)
    {
      this.OnGUI(elements, previewColumnFunction, (ColumnView.ObjectColumnFunction) null, (ColumnView.ObjectColumnFunction) null, (ColumnView.ObjectColumnGetDataFunction) null);
    }

    public void OnGUI(List<ColumnViewElement> elements, ColumnView.ObjectColumnFunction previewColumnFunction, ColumnView.ObjectColumnFunction selectedSearchItemFunction, ColumnView.ObjectColumnFunction selectedRegularItemFunction, ColumnView.ObjectColumnGetDataFunction getDataForDraggingFunction)
    {
      ColumnView.InitStyles();
      this.m_ScrollPosition = GUILayout.BeginScrollView(this.m_ScrollPosition);
      GUILayout.BeginHorizontal();
      List<ColumnViewElement> columnViewElements = elements;
      int columnIndex = 0;
      object selectedObject;
      do
      {
        if (this.m_ListViewStates.Count == columnIndex)
          this.m_ListViewStates.Add(new ListViewState());
        if (this.m_CachedSelectedIndices.Count == columnIndex)
          this.m_CachedSelectedIndices.Add(-1);
        ListViewState listViewState = this.m_ListViewStates[columnIndex];
        listViewState.totalRows = columnViewElements.Count;
        if (columnIndex == 0)
          GUILayout.BeginVertical(GUILayout.MaxWidth(this.columnWidth));
        int cachedSelectedIndex = this.m_CachedSelectedIndices[columnIndex];
        int index1 = this.DoListColumn(listViewState, columnViewElements, columnIndex, cachedSelectedIndex, columnIndex != 0 ? (ColumnView.ObjectColumnFunction) null : selectedSearchItemFunction, selectedRegularItemFunction, getDataForDraggingFunction);
        if (Event.current.type == EventType.Layout && this.m_ColumnToFocusKeyboard == columnIndex)
        {
          this.m_ColumnToFocusKeyboard = -1;
          GUIUtility.keyboardControl = listViewState.ID;
          if (listViewState.row == -1 && columnViewElements.Count != 0)
            index1 = listViewState.row = 0;
        }
        if (columnIndex == 0)
        {
          if (this.isSearching)
          {
            KeyCode keyCode = ColumnView.StealImportantListviewKeys();
            if (keyCode != KeyCode.None)
              ListViewShared.SendKey(this.m_ListViewStates[0], keyCode);
          }
          this.m_SearchText = EditorGUILayout.ToolbarSearchField(this.m_SearchText);
          GUILayout.EndVertical();
        }
        if (index1 >= columnViewElements.Count)
          index1 = -1;
        if (Event.current.type == EventType.Layout && this.m_CachedSelectedIndices[columnIndex] != index1 && this.m_ListViewStates.Count > columnIndex + 1)
        {
          int index2 = columnIndex + 1;
          int count = this.m_ListViewStates.Count - (columnIndex + 1);
          this.m_ListViewStates.RemoveRange(index2, count);
          this.m_CachedSelectedIndices.RemoveRange(index2, count);
        }
        this.m_CachedSelectedIndices[columnIndex] = index1;
        selectedObject = index1 <= -1 ? (object) null : columnViewElements[index1].value;
        columnViewElements = selectedObject as List<ColumnViewElement>;
        ++columnIndex;
      }
      while (columnViewElements != null);
      for (; columnIndex < this.minimumNumberOfColumns; ++columnIndex)
        this.DoDummyColumn();
      ColumnView.DoPreviewColumn(selectedObject, previewColumnFunction);
      GUILayout.EndHorizontal();
      GUILayout.EndScrollView();
    }

    private static void DoItemSelectedEvent(ColumnView.ObjectColumnFunction selectedRegularItemFunction, object value)
    {
      if (selectedRegularItemFunction != null)
        selectedRegularItemFunction(value);
      Event.current.Use();
    }

    private void DoSearchItemSelectedEvent(ColumnView.ObjectColumnFunction selectedSearchItemFunction, object value)
    {
      this.m_SearchText = string.Empty;
      ColumnView.DoItemSelectedEvent(selectedSearchItemFunction, value);
    }

    private void DoDummyColumn()
    {
      GUILayout.Box(GUIContent.none, ColumnView.s_Styles.background, new GUILayoutOption[1]
      {
        GUILayout.Width(this.columnWidth + 1f)
      });
    }

    private static void DoPreviewColumn(object selectedObject, ColumnView.ObjectColumnFunction previewColumnFunction)
    {
      GUILayout.BeginVertical(ColumnView.s_Styles.background, new GUILayoutOption[0]);
      if (previewColumnFunction != null)
        previewColumnFunction(selectedObject);
      GUILayout.EndVertical();
    }

    private int DoListColumn(ListViewState listView, List<ColumnViewElement> columnViewElements, int columnIndex, int selectedIndex, ColumnView.ObjectColumnFunction selectedSearchItemFunction, ColumnView.ObjectColumnFunction selectedRegularItemFunction, ColumnView.ObjectColumnGetDataFunction getDataForDraggingFunction)
    {
      if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return && listView.row > -1)
      {
        if (this.isSearching && selectedSearchItemFunction != null)
          this.DoSearchItemSelectedEvent(selectedSearchItemFunction, columnViewElements[selectedIndex].value);
        if (!this.isSearching && GUIUtility.keyboardControl == listView.ID && selectedRegularItemFunction != null)
          ColumnView.DoItemSelectedEvent(selectedRegularItemFunction, columnViewElements[selectedIndex].value);
      }
      if (GUIUtility.keyboardControl == listView.ID && Event.current.type == EventType.KeyDown && !this.isSearching)
      {
        switch (Event.current.keyCode)
        {
          case KeyCode.RightArrow:
            this.m_ColumnToFocusKeyboard = columnIndex + 1;
            Event.current.Use();
            break;
          case KeyCode.LeftArrow:
            this.m_ColumnToFocusKeyboard = columnIndex - 1;
            Event.current.Use();
            break;
        }
      }
      ListViewState state = listView;
      GUIStyle background = ColumnView.s_Styles.background;
      GUILayoutOption[] guiLayoutOptionArray = new GUILayoutOption[1]{ GUILayout.Width(this.columnWidth) };
      foreach (ListViewElement element in ListViewGUILayout.ListView(state, background, guiLayoutOptionArray))
      {
        ColumnViewElement columnViewElement = columnViewElements[element.row];
        if (element.row == listView.row && Event.current.type == EventType.Repaint)
        {
          Rect position = element.position;
          ++position.x;
          ++position.y;
          ColumnView.s_Styles.selected.Draw(position, false, true, true, GUIUtility.keyboardControl == listView.ID);
        }
        GUILayout.Label(columnViewElement.name);
        if (columnViewElement.value is List<ColumnViewElement>)
        {
          Rect position = element.position;
          position.x = (float) ((double) position.xMax - (double) ColumnView.s_Styles.categoryArrowIcon.width - 5.0);
          position.y += 2f;
          GUI.Label(position, (Texture) ColumnView.s_Styles.categoryArrowIcon);
        }
        this.DoDoubleClick(element, columnViewElement, selectedSearchItemFunction, selectedRegularItemFunction);
        ColumnView.DoDragAndDrop(listView, element, columnViewElements, getDataForDraggingFunction);
      }
      if (Event.current.type == EventType.Layout)
        selectedIndex = listView.row;
      return selectedIndex;
    }

    private static void DoDragAndDrop(ListViewState listView, ListViewElement element, List<ColumnViewElement> columnViewElements, ColumnView.ObjectColumnGetDataFunction getDataForDraggingFunction)
    {
      if (GUIUtility.hotControl == listView.ID && Event.current.type == EventType.MouseDown && (element.position.Contains(Event.current.mousePosition) && Event.current.button == 0))
        ((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), listView.ID)).mouseDownPosition = Event.current.mousePosition;
      if (GUIUtility.hotControl != listView.ID || Event.current.type != EventType.MouseDrag || (!GUIClip.visibleRect.Contains(Event.current.mousePosition) || !((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), listView.ID)).CanStartDrag()))
        return;
      object data = getDataForDraggingFunction != null ? getDataForDraggingFunction(columnViewElements[listView.row].value) : (object) null;
      if (data == null)
        return;
      DragAndDrop.PrepareStartDrag();
      DragAndDrop.objectReferences = new Object[0];
      DragAndDrop.paths = (string[]) null;
      DragAndDrop.SetGenericData("CustomDragData", data);
      DragAndDrop.StartDrag(columnViewElements[listView.row].name);
      Event.current.Use();
    }

    private void DoDoubleClick(ListViewElement element, ColumnViewElement columnViewElement, ColumnView.ObjectColumnFunction selectedSearchItemFunction, ColumnView.ObjectColumnFunction selectedRegularItemFunction)
    {
      if (Event.current.type != EventType.MouseDown || !element.position.Contains(Event.current.mousePosition) || (Event.current.button != 0 || Event.current.clickCount != 2))
        return;
      if (this.isSearching)
        this.DoSearchItemSelectedEvent(selectedSearchItemFunction, columnViewElement.value);
      else
        ColumnView.DoItemSelectedEvent(selectedRegularItemFunction, columnViewElement.value);
    }

    private static KeyCode StealImportantListviewKeys()
    {
      if (Event.current.type == EventType.KeyDown)
      {
        KeyCode keyCode = Event.current.keyCode;
        switch (keyCode)
        {
          case KeyCode.UpArrow:
          case KeyCode.DownArrow:
          case KeyCode.PageUp:
          case KeyCode.PageDown:
            Event.current.Use();
            return keyCode;
        }
      }
      return KeyCode.None;
    }

    public class Styles
    {
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle selected = (GUIStyle) "PR Label";
      public Texture2D categoryArrowIcon = EditorStyles.foldout.normal.background;
    }

    public delegate void ObjectColumnFunction(object value);

    public delegate object ObjectColumnGetDataFunction(object value);
  }
}
