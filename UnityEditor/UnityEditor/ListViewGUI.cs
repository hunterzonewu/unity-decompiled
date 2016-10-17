// Decompiled with JetBrains decompiler
// Type: UnityEditor.ListViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class ListViewGUI
  {
    private static int[] dummyWidths = new int[1];
    internal static ListViewShared.InternalListViewState ilvState = new ListViewShared.InternalListViewState();
    private static int listViewHash = "ListView".GetHashCode();

    public static ListViewShared.ListViewElementsEnumerator ListView(Rect pos, ListViewState state)
    {
      return ListViewGUI.DoListView(pos, state, (int[]) null, string.Empty);
    }

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, GUIStyle style, params GUILayoutOption[] options)
    {
      return ListViewGUI.ListView(state, (ListViewOptions) 0, (int[]) null, string.Empty, style, options);
    }

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, int[] colWidths, GUIStyle style, params GUILayoutOption[] options)
    {
      return ListViewGUI.ListView(state, (ListViewOptions) 0, colWidths, string.Empty, style, options);
    }

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, ListViewOptions lvOptions, GUIStyle style, params GUILayoutOption[] options)
    {
      return ListViewGUI.ListView(state, lvOptions, (int[]) null, string.Empty, style, options);
    }

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, ListViewOptions lvOptions, string dragTitle, GUIStyle style, params GUILayoutOption[] options)
    {
      return ListViewGUI.ListView(state, lvOptions, (int[]) null, dragTitle, style, options);
    }

    public static ListViewShared.ListViewElementsEnumerator ListView(ListViewState state, ListViewOptions lvOptions, int[] colWidths, string dragTitle, GUIStyle style, params GUILayoutOption[] options)
    {
      GUILayout.BeginHorizontal(style, new GUILayoutOption[0]);
      state.scrollPos = EditorGUILayout.BeginScrollView(state.scrollPos, options);
      ListViewGUI.ilvState.beganHorizontal = true;
      state.draggedFrom = -1;
      state.draggedTo = -1;
      state.fileNames = (string[]) null;
      if ((lvOptions & ListViewOptions.wantsReordering) != (ListViewOptions) 0)
        ListViewGUI.ilvState.wantsReordering = true;
      if ((lvOptions & ListViewOptions.wantsExternalFiles) != (ListViewOptions) 0)
        ListViewGUI.ilvState.wantsExternalFiles = true;
      if ((lvOptions & ListViewOptions.wantsToStartCustomDrag) != (ListViewOptions) 0)
        ListViewGUI.ilvState.wantsToStartCustomDrag = true;
      if ((lvOptions & ListViewOptions.wantsToAcceptCustomDrag) != (ListViewOptions) 0)
        ListViewGUI.ilvState.wantsToAcceptCustomDrag = true;
      return ListViewGUI.DoListView(GUILayoutUtility.GetRect(1f, (float) (state.totalRows * state.rowHeight + 3)), state, colWidths, string.Empty);
    }

    public static ListViewShared.ListViewElementsEnumerator DoListView(Rect pos, ListViewState state, int[] colWidths, string dragTitle)
    {
      int controlId = GUIUtility.GetControlID(ListViewGUI.listViewHash, FocusType.Native);
      state.ID = controlId;
      state.selectionChanged = false;
      Rect rect = (double) GUIClip.visibleRect.x < 0.0 || (double) GUIClip.visibleRect.y < 0.0 ? pos : ((double) pos.y >= 0.0 ? new Rect(0.0f, state.scrollPos.y, GUIClip.visibleRect.width, GUIClip.visibleRect.height) : new Rect(0.0f, 0.0f, GUIClip.visibleRect.width, GUIClip.visibleRect.height));
      if ((double) rect.width <= 0.0)
        rect.width = 1f;
      if ((double) rect.height <= 0.0)
        rect.height = 1f;
      ListViewGUI.ilvState.rect = rect;
      int yFrom = (int) ((-(double) pos.y + (double) rect.yMin) / (double) state.rowHeight);
      int yTo = yFrom + (int) Math.Ceiling((((double) rect.yMin - (double) pos.y) % (double) state.rowHeight + (double) rect.height) / (double) state.rowHeight) - 1;
      if (colWidths == null)
      {
        ListViewGUI.dummyWidths[0] = (int) rect.width;
        colWidths = ListViewGUI.dummyWidths;
      }
      ListViewGUI.ilvState.invisibleRows = yFrom;
      ListViewGUI.ilvState.endRow = yTo;
      ListViewGUI.ilvState.rectHeight = (int) rect.height;
      ListViewGUI.ilvState.state = state;
      if (yFrom < 0)
        yFrom = 0;
      if (yTo >= state.totalRows)
        yTo = state.totalRows - 1;
      return new ListViewShared.ListViewElementsEnumerator(ListViewGUI.ilvState, colWidths, yFrom, yTo, dragTitle, new Rect(0.0f, (float) (yFrom * state.rowHeight), pos.width, (float) state.rowHeight));
    }

    public static bool MultiSelection(int prevSelected, int currSelected, ref int initialSelected, ref bool[] selectedItems)
    {
      return ListViewShared.MultiSelection(ListViewGUI.ilvState, prevSelected, currSelected, ref initialSelected, ref selectedItems);
    }

    public static bool HasMouseUp(Rect r)
    {
      return ListViewShared.HasMouseUp(ListViewGUI.ilvState, r, 0);
    }

    public static bool HasMouseDown(Rect r)
    {
      return ListViewShared.HasMouseDown(ListViewGUI.ilvState, r, 0);
    }

    public static bool HasMouseDown(Rect r, int button)
    {
      return ListViewShared.HasMouseDown(ListViewGUI.ilvState, r, button);
    }
  }
}
