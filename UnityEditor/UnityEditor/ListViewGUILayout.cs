// Decompiled with JetBrains decompiler
// Type: UnityEditor.ListViewGUILayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class ListViewGUILayout
  {
    private static int layoutedListViewHash = "layoutedListView".GetHashCode();
    private static ListViewState lvState = (ListViewState) null;
    private static int listViewHash = "ListView".GetHashCode();
    private static int[] dummyWidths = new int[1];
    private static Rect dummyRect = new Rect(0.0f, 0.0f, 1f, 1f);

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, GUIStyle style, params GUILayoutOption[] options)
    {
      return ListViewGUILayout.ListView(state, (ListViewOptions) 0, string.Empty, style, options);
    }

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, string dragTitle, GUIStyle style, params GUILayoutOption[] options)
    {
      return ListViewGUILayout.ListView(state, (ListViewOptions) 0, dragTitle, style, options);
    }

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, ListViewOptions lvOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return ListViewGUILayout.ListView(state, lvOptions, string.Empty, style, options);
    }

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, ListViewOptions lvOptions, string dragTitle, GUIStyle style, params GUILayoutOption[] options)
    {
      ListViewGUILayout.lvState = state;
      GUILayout.BeginHorizontal(style, options);
      state.scrollPos = EditorGUILayout.BeginScrollView(state.scrollPos, options);
      ListViewGUILayout.BeginLayoutedListview(state, GUIStyle.none);
      state.draggedFrom = -1;
      state.draggedTo = -1;
      state.fileNames = (string[]) null;
      if ((lvOptions & ListViewOptions.wantsReordering) != (ListViewOptions) 0)
        state.ilvState.wantsReordering = true;
      if ((lvOptions & ListViewOptions.wantsExternalFiles) != (ListViewOptions) 0)
        state.ilvState.wantsExternalFiles = true;
      if ((lvOptions & ListViewOptions.wantsToStartCustomDrag) != (ListViewOptions) 0)
        state.ilvState.wantsToStartCustomDrag = true;
      if ((lvOptions & ListViewOptions.wantsToAcceptCustomDrag) != (ListViewOptions) 0)
        state.ilvState.wantsToAcceptCustomDrag = true;
      return ListViewGUILayout.DoListView(state, (int[]) null, dragTitle);
    }

    private static ListViewShared.ListViewElementsEnumerator DoListView(ListViewState state, int[] colWidths, string dragTitle)
    {
      Rect rect = ListViewGUILayout.dummyRect;
      int yFrom = 0;
      int yTo = 0;
      ListViewShared.InternalLayoutedListViewState ilvState = state.ilvState;
      int controlId = GUIUtility.GetControlID(ListViewGUILayout.listViewHash, FocusType.Native);
      state.ID = controlId;
      state.selectionChanged = false;
      ilvState.state = state;
      if (Event.current.type != EventType.Layout)
      {
        rect = new Rect(0.0f, state.scrollPos.y, GUIClip.visibleRect.width, GUIClip.visibleRect.height);
        if ((double) rect.width <= 0.0)
          rect.width = 1f;
        if ((double) rect.height <= 0.0)
          rect.height = 1f;
        state.ilvState.rect = rect;
        yFrom = (int) rect.yMin / state.rowHeight;
        yTo = yFrom + (int) Math.Ceiling(((double) rect.yMin % (double) state.rowHeight + (double) rect.height) / (double) state.rowHeight) - 1;
        ilvState.invisibleRows = yFrom;
        ilvState.endRow = yTo;
        ilvState.rectHeight = (int) rect.height;
        if (yFrom < 0)
          yFrom = 0;
        if (yTo >= state.totalRows)
          yTo = state.totalRows - 1;
      }
      if (colWidths == null)
      {
        ListViewGUILayout.dummyWidths[0] = (int) rect.width;
        colWidths = ListViewGUILayout.dummyWidths;
      }
      return new ListViewShared.ListViewElementsEnumerator((ListViewShared.InternalListViewState) ilvState, colWidths, yFrom, yTo, dragTitle, new Rect(0.0f, (float) (yFrom * state.rowHeight), rect.width, (float) state.rowHeight));
    }

    private static void BeginLayoutedListview(ListViewState state, GUIStyle style, params GUILayoutOption[] options)
    {
      ListViewGUILayout.GUILayoutedListViewGroup layoutedListViewGroup = (ListViewGUILayout.GUILayoutedListViewGroup) GUILayoutUtility.BeginLayoutGroup(style, (GUILayoutOption[]) null, typeof (ListViewGUILayout.GUILayoutedListViewGroup));
      layoutedListViewGroup.state = state;
      state.ilvState.group = layoutedListViewGroup;
      GUIUtility.GetControlID(ListViewGUILayout.layoutedListViewHash, FocusType.Native);
      if (Event.current.type != EventType.Layout)
        return;
      layoutedListViewGroup.resetCoords = false;
      layoutedListViewGroup.isVertical = true;
      layoutedListViewGroup.ApplyOptions(options);
    }

    public static bool MultiSelection(int prevSelected, int currSelected, ref int initialSelected, ref bool[] selectedItems)
    {
      return ListViewShared.MultiSelection((ListViewShared.InternalListViewState) ListViewGUILayout.lvState.ilvState, prevSelected, currSelected, ref initialSelected, ref selectedItems);
    }

    public static bool HasMouseUp(Rect r)
    {
      return ListViewShared.HasMouseUp((ListViewShared.InternalListViewState) ListViewGUILayout.lvState.ilvState, r, 0);
    }

    public static bool HasMouseDown(Rect r)
    {
      return ListViewShared.HasMouseDown((ListViewShared.InternalListViewState) ListViewGUILayout.lvState.ilvState, r, 0);
    }

    public static bool HasMouseDown(Rect r, int button)
    {
      return ListViewShared.HasMouseDown((ListViewShared.InternalListViewState) ListViewGUILayout.lvState.ilvState, r, button);
    }

    internal class GUILayoutedListViewGroup : GUILayoutGroup
    {
      internal ListViewState state;

      public override void CalcWidth()
      {
        base.CalcWidth();
        this.minWidth = 0.0f;
        this.maxWidth = 0.0f;
        this.stretchWidth = 10000;
      }

      public override void CalcHeight()
      {
        this.minHeight = 0.0f;
        this.maxHeight = 0.0f;
        base.CalcHeight();
        this.margin.top = 0;
        this.margin.bottom = 0;
        if ((double) this.minHeight == 0.0)
        {
          this.minHeight = 1f;
          this.maxHeight = 1f;
          this.state.rowHeight = 1;
        }
        else
        {
          this.state.rowHeight = (int) this.minHeight;
          this.minHeight *= (float) (double) this.state.totalRows;
          this.maxHeight *= (float) (double) this.state.totalRows;
        }
      }

      private void AddYRecursive(GUILayoutEntry e, float y)
      {
        e.rect.y += y;
        GUILayoutGroup guiLayoutGroup = e as GUILayoutGroup;
        if (guiLayoutGroup == null)
          return;
        for (int index = 0; index < guiLayoutGroup.entries.Count; ++index)
          this.AddYRecursive(guiLayoutGroup.entries[index], y);
      }

      public void AddY()
      {
        if (this.entries.Count <= 0)
          return;
        this.AddYRecursive(this.entries[0], this.entries[0].minHeight);
      }

      public void AddY(float val)
      {
        if (this.entries.Count <= 0)
          return;
        this.AddYRecursive(this.entries[0], val);
      }
    }
  }
}
