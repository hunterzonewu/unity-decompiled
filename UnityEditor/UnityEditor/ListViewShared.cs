// Decompiled with JetBrains decompiler
// Type: UnityEditor.ListViewShared
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ListViewShared
  {
    public static bool OSX = Application.platform == RuntimePlatform.OSXEditor;
    internal static int dragControlID = -1;

    private static bool DoLVPageUpDown(ListViewShared.InternalListViewState ilvState, ref int selectedRow, ref Vector2 scrollPos, bool up)
    {
      int num = ilvState.endRow - ilvState.invisibleRows;
      if (up)
      {
        if (ListViewShared.OSX)
        {
          scrollPos.y -= (float) (ilvState.state.rowHeight * num);
          if ((double) scrollPos.y < 0.0)
            scrollPos.y = 0.0f;
        }
        else
        {
          selectedRow = selectedRow - num;
          if (selectedRow < 0)
            selectedRow = 0;
          return true;
        }
      }
      else if (ListViewShared.OSX)
      {
        scrollPos.y += (float) (ilvState.state.rowHeight * num);
      }
      else
      {
        selectedRow = selectedRow + num;
        if (selectedRow >= ilvState.state.totalRows)
          selectedRow = ilvState.state.totalRows - 1;
        return true;
      }
      return false;
    }

    internal static bool ListViewKeyboard(ListViewShared.InternalListViewState ilvState, int totalCols)
    {
      if (Event.current.type != EventType.KeyDown || ilvState.state.totalRows == 0 || (GUIUtility.keyboardControl != ilvState.state.ID || Event.current.GetTypeForControl(ilvState.state.ID) != EventType.KeyDown))
        return false;
      return ListViewShared.SendKey(ilvState, Event.current.keyCode, totalCols);
    }

    internal static void SendKey(ListViewState state, KeyCode keyCode)
    {
      ListViewShared.SendKey((ListViewShared.InternalListViewState) state.ilvState, keyCode, 1);
    }

    internal static bool SendKey(ListViewShared.InternalListViewState ilvState, KeyCode keyCode, int totalCols)
    {
      ListViewState state = ilvState.state;
      switch (keyCode)
      {
        case KeyCode.UpArrow:
          if (state.row > 0)
          {
            --state.row;
            break;
          }
          break;
        case KeyCode.DownArrow:
          if (state.row < state.totalRows - 1)
          {
            ++state.row;
            break;
          }
          break;
        case KeyCode.RightArrow:
          if (state.column < totalCols - 1)
          {
            ++state.column;
            break;
          }
          break;
        case KeyCode.LeftArrow:
          if (state.column > 0)
          {
            --state.column;
            break;
          }
          break;
        case KeyCode.Home:
          state.row = 0;
          break;
        case KeyCode.End:
          state.row = state.totalRows - 1;
          break;
        case KeyCode.PageUp:
          if (!ListViewShared.DoLVPageUpDown(ilvState, ref state.row, ref state.scrollPos, true))
          {
            Event.current.Use();
            return false;
          }
          break;
        case KeyCode.PageDown:
          if (!ListViewShared.DoLVPageUpDown(ilvState, ref state.row, ref state.scrollPos, false))
          {
            Event.current.Use();
            return false;
          }
          break;
        default:
          return false;
      }
      state.scrollPos = ListViewShared.ListViewScrollToRow(ilvState, state.scrollPos, state.row);
      Event.current.Use();
      return true;
    }

    internal static bool HasMouseDown(ListViewShared.InternalListViewState ilvState, Rect r)
    {
      return ListViewShared.HasMouseDown(ilvState, r, 0);
    }

    internal static bool HasMouseDown(ListViewShared.InternalListViewState ilvState, Rect r, int button)
    {
      if (Event.current.type != EventType.MouseDown || Event.current.button != button || !r.Contains(Event.current.mousePosition))
        return false;
      GUIUtility.hotControl = ilvState.state.ID;
      GUIUtility.keyboardControl = ilvState.state.ID;
      Event.current.Use();
      return true;
    }

    internal static bool HasMouseUp(ListViewShared.InternalListViewState ilvState, Rect r)
    {
      return ListViewShared.HasMouseUp(ilvState, r, 0);
    }

    internal static bool HasMouseUp(ListViewShared.InternalListViewState ilvState, Rect r, int button)
    {
      if (Event.current.type != EventType.MouseUp || Event.current.button != button || !r.Contains(Event.current.mousePosition))
        return false;
      GUIUtility.hotControl = 0;
      Event.current.Use();
      return true;
    }

    internal static bool MultiSelection(ListViewShared.InternalListViewState ilvState, int prevSelected, int currSelected, ref int initialSelected, ref bool[] selectedItems)
    {
      bool shift = Event.current.shift;
      bool actionKey = EditorGUI.actionKey;
      bool flag = false;
      if ((shift || actionKey) && initialSelected == -1)
        initialSelected = prevSelected;
      if (shift)
      {
        int num1 = Math.Min(initialSelected, currSelected);
        int num2 = Math.Max(initialSelected, currSelected);
        if (!actionKey)
        {
          for (int index = 0; index < num1; ++index)
          {
            if (selectedItems[index])
              flag = true;
            selectedItems[index] = false;
          }
          for (int index = num2 + 1; index < selectedItems.Length; ++index)
          {
            if (selectedItems[index])
              flag = true;
            selectedItems[index] = false;
          }
        }
        if (num1 < 0)
          num1 = num2;
        for (int index = num1; index <= num2; ++index)
        {
          if (!selectedItems[index])
            flag = true;
          selectedItems[index] = true;
        }
      }
      else if (actionKey)
      {
        selectedItems[currSelected] = !selectedItems[currSelected];
        initialSelected = currSelected;
        flag = true;
      }
      else
      {
        if (!selectedItems[currSelected])
          flag = true;
        for (int index = 0; index < selectedItems.Length; ++index)
        {
          if (selectedItems[index] && currSelected != index)
            flag = true;
          selectedItems[index] = false;
        }
        initialSelected = -1;
        selectedItems[currSelected] = true;
      }
      if (ilvState != null)
        ilvState.state.scrollPos = ListViewShared.ListViewScrollToRow(ilvState, currSelected);
      return flag;
    }

    internal static Vector2 ListViewScrollToRow(ListViewShared.InternalListViewState ilvState, int row)
    {
      return ListViewShared.ListViewScrollToRow(ilvState, ilvState.state.scrollPos, row);
    }

    internal static int ListViewScrollToRow(ListViewShared.InternalListViewState ilvState, int currPosY, int row)
    {
      return (int) ListViewShared.ListViewScrollToRow(ilvState, new Vector2(0.0f, (float) currPosY), row).y;
    }

    internal static Vector2 ListViewScrollToRow(ListViewShared.InternalListViewState ilvState, Vector2 currPos, int row)
    {
      if (ilvState.invisibleRows < row && ilvState.endRow > row)
        return currPos;
      currPos.y = row > ilvState.invisibleRows ? (float) (ilvState.state.rowHeight * (row + 1) - ilvState.rectHeight) : (float) (ilvState.state.rowHeight * row);
      if ((double) currPos.y < 0.0)
        currPos.y = 0.0f;
      else if ((double) currPos.y > (double) (ilvState.state.totalRows * ilvState.state.rowHeight - ilvState.rectHeight))
        currPos.y = (float) (ilvState.state.totalRows * ilvState.state.rowHeight - ilvState.rectHeight);
      return currPos;
    }

    internal class InternalListViewState
    {
      public int id = -1;
      public int invisibleRows;
      public int endRow;
      public int rectHeight;
      public ListViewState state;
      public bool beganHorizontal;
      public Rect rect;
      public bool wantsReordering;
      public bool wantsExternalFiles;
      public bool wantsToStartCustomDrag;
      public bool wantsToAcceptCustomDrag;
      public int dragItem;
    }

    internal class InternalLayoutedListViewState : ListViewShared.InternalListViewState
    {
      public ListViewGUILayout.GUILayoutedListViewGroup group;
    }

    internal class Constants
    {
      public static string insertion = "PR Insertion";
    }

    internal class ListViewElementsEnumerator : IDisposable, IEnumerator, IEnumerator<ListViewElement>
    {
      private int xPos = -1;
      private int yPos = -1;
      private int[] colWidths;
      private int xTo;
      private int yFrom;
      private int yTo;
      private Rect firstRect;
      private Rect rect;
      private ListViewElement element;
      private ListViewShared.InternalListViewState ilvState;
      private ListViewShared.InternalLayoutedListViewState ilvStateL;
      private bool quiting;
      private bool isLayouted;
      private string dragTitle;

      ListViewElement IEnumerator<ListViewElement>.Current
      {
        get
        {
          return this.element;
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.element;
        }
      }

      internal ListViewElementsEnumerator(ListViewShared.InternalListViewState ilvState, int[] colWidths, int yFrom, int yTo, string dragTitle, Rect firstRect)
      {
        this.colWidths = colWidths;
        this.xTo = colWidths.Length - 1;
        this.yFrom = yFrom;
        this.yTo = yTo;
        this.firstRect = firstRect;
        this.rect = firstRect;
        this.quiting = ilvState.state.totalRows == 0;
        this.ilvState = ilvState;
        this.ilvStateL = ilvState as ListViewShared.InternalLayoutedListViewState;
        this.isLayouted = this.ilvStateL != null;
        this.dragTitle = dragTitle;
        ilvState.state.customDraggedFromID = 0;
        this.Reset();
      }

      public bool MoveNext()
      {
        if (this.xPos > -1)
        {
          if (ListViewShared.HasMouseDown(this.ilvState, this.rect))
          {
            this.ilvState.state.selectionChanged = true;
            this.ilvState.state.row = this.yPos;
            this.ilvState.state.column = this.xPos;
            this.ilvState.state.scrollPos = ListViewShared.ListViewScrollToRow(this.ilvState, this.yPos);
            if ((this.ilvState.wantsReordering || this.ilvState.wantsToStartCustomDrag) && GUIUtility.hotControl == this.ilvState.state.ID)
            {
              ((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), this.ilvState.state.ID)).mouseDownPosition = Event.current.mousePosition;
              this.ilvState.dragItem = this.yPos;
              ListViewShared.dragControlID = this.ilvState.state.ID;
            }
          }
          if ((this.ilvState.wantsReordering || this.ilvState.wantsToStartCustomDrag) && (GUIUtility.hotControl == this.ilvState.state.ID && Event.current.type == EventType.MouseDrag) && GUIClip.visibleRect.Contains(Event.current.mousePosition))
          {
            if (((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), this.ilvState.state.ID)).CanStartDrag())
            {
              DragAndDrop.PrepareStartDrag();
              DragAndDrop.objectReferences = new UnityEngine.Object[0];
              DragAndDrop.paths = (string[]) null;
              if (this.ilvState.wantsReordering)
              {
                this.ilvState.state.dropHereRect = new Rect(this.ilvState.rect.x, 0.0f, this.ilvState.rect.width, (float) (this.ilvState.state.rowHeight * 2));
                DragAndDrop.StartDrag(this.dragTitle);
              }
              else if (this.ilvState.wantsToStartCustomDrag)
              {
                DragAndDrop.SetGenericData("CustomDragID", (object) this.ilvState.state.ID);
                DragAndDrop.StartDrag(this.dragTitle);
              }
            }
            Event.current.Use();
          }
        }
        ++this.xPos;
        if (this.xPos > this.xTo)
        {
          this.xPos = 0;
          ++this.yPos;
          this.rect.x = this.firstRect.x;
          this.rect.width = (float) this.colWidths[0];
          if (this.yPos > this.yTo)
            this.quiting = true;
          else
            this.rect.y += this.rect.height;
        }
        else
        {
          if (this.xPos >= 1)
            this.rect.x += (float) this.colWidths[this.xPos - 1];
          this.rect.width = (float) this.colWidths[this.xPos];
        }
        this.element.row = this.yPos;
        this.element.column = this.xPos;
        this.element.position = this.rect;
        if (this.element.row >= this.ilvState.state.totalRows)
          this.quiting = true;
        if (this.isLayouted && Event.current.type == EventType.Layout && this.yFrom + 1 == this.yPos)
          this.quiting = true;
        if (this.isLayouted && this.yPos != this.yFrom)
          GUILayout.EndHorizontal();
        if (this.quiting)
        {
          if (this.ilvState.state.drawDropHere && Event.current.GetTypeForControl(this.ilvState.state.ID) == EventType.Repaint)
          {
            GUIStyle insertion = (GUIStyle) ListViewShared.Constants.insertion;
            insertion.Draw(insertion.margin.Remove(this.ilvState.state.dropHereRect), false, false, false, false);
          }
          if (ListViewShared.ListViewKeyboard(this.ilvState, this.colWidths.Length))
            this.ilvState.state.selectionChanged = true;
          if (Event.current.GetTypeForControl(this.ilvState.state.ID) == EventType.MouseUp)
            GUIUtility.hotControl = 0;
          if (this.ilvState.wantsReordering && GUIUtility.hotControl == this.ilvState.state.ID)
          {
            ListViewState state = this.ilvState.state;
            switch (Event.current.type)
            {
              case EventType.DragUpdated:
                DragAndDrop.visualMode = !this.ilvState.rect.Contains(Event.current.mousePosition) ? DragAndDropVisualMode.None : DragAndDropVisualMode.Move;
                Event.current.Use();
                if (DragAndDrop.visualMode != DragAndDropVisualMode.None)
                {
                  state.dropHereRect.y = (float) ((Mathf.RoundToInt(Event.current.mousePosition.y / (float) state.rowHeight) - 1) * state.rowHeight);
                  if ((double) state.dropHereRect.y >= (double) (state.rowHeight * state.totalRows))
                    state.dropHereRect.y = (float) (state.rowHeight * (state.totalRows - 1));
                  state.drawDropHere = true;
                  break;
                }
                break;
              case EventType.DragPerform:
                if (GUIClip.visibleRect.Contains(Event.current.mousePosition))
                {
                  this.ilvState.state.draggedFrom = this.ilvState.dragItem;
                  this.ilvState.state.draggedTo = Mathf.RoundToInt(Event.current.mousePosition.y / (float) state.rowHeight);
                  if (this.ilvState.state.draggedTo > this.ilvState.state.totalRows)
                    this.ilvState.state.draggedTo = this.ilvState.state.totalRows;
                  this.ilvState.state.row = this.ilvState.state.draggedTo <= this.ilvState.state.draggedFrom ? this.ilvState.state.draggedTo : this.ilvState.state.draggedTo - 1;
                  this.ilvState.state.selectionChanged = true;
                  DragAndDrop.AcceptDrag();
                  Event.current.Use();
                  this.ilvState.wantsReordering = false;
                  this.ilvState.state.drawDropHere = false;
                }
                GUIUtility.hotControl = 0;
                break;
              case EventType.DragExited:
                this.ilvState.wantsReordering = false;
                this.ilvState.state.drawDropHere = false;
                GUIUtility.hotControl = 0;
                break;
            }
          }
          else if (this.ilvState.wantsExternalFiles)
          {
            switch (Event.current.type)
            {
              case EventType.DragUpdated:
                if (GUIClip.visibleRect.Contains(Event.current.mousePosition) && DragAndDrop.paths != null && DragAndDrop.paths.Length != 0)
                {
                  DragAndDrop.visualMode = !this.ilvState.rect.Contains(Event.current.mousePosition) ? DragAndDropVisualMode.None : DragAndDropVisualMode.Copy;
                  Event.current.Use();
                  if (DragAndDrop.visualMode != DragAndDropVisualMode.None)
                  {
                    this.ilvState.state.dropHereRect = new Rect(this.ilvState.rect.x, (float) ((Mathf.RoundToInt(Event.current.mousePosition.y / (float) this.ilvState.state.rowHeight) - 1) * this.ilvState.state.rowHeight), this.ilvState.rect.width, (float) this.ilvState.state.rowHeight);
                    if ((double) this.ilvState.state.dropHereRect.y >= (double) (this.ilvState.state.rowHeight * this.ilvState.state.totalRows))
                      this.ilvState.state.dropHereRect.y = (float) (this.ilvState.state.rowHeight * (this.ilvState.state.totalRows - 1));
                    this.ilvState.state.drawDropHere = true;
                    break;
                  }
                  break;
                }
                break;
              case EventType.DragPerform:
                if (GUIClip.visibleRect.Contains(Event.current.mousePosition))
                {
                  this.ilvState.state.fileNames = DragAndDrop.paths;
                  DragAndDrop.AcceptDrag();
                  Event.current.Use();
                  this.ilvState.wantsExternalFiles = false;
                  this.ilvState.state.drawDropHere = false;
                  this.ilvState.state.draggedTo = Mathf.RoundToInt(Event.current.mousePosition.y / (float) this.ilvState.state.rowHeight);
                  if (this.ilvState.state.draggedTo > this.ilvState.state.totalRows)
                    this.ilvState.state.draggedTo = this.ilvState.state.totalRows;
                  this.ilvState.state.row = this.ilvState.state.draggedTo;
                }
                GUIUtility.hotControl = 0;
                break;
              case EventType.DragExited:
                this.ilvState.wantsExternalFiles = false;
                this.ilvState.state.drawDropHere = false;
                GUIUtility.hotControl = 0;
                break;
            }
          }
          else if (this.ilvState.wantsToAcceptCustomDrag && ListViewShared.dragControlID != this.ilvState.state.ID)
          {
            switch (Event.current.type)
            {
              case EventType.DragUpdated:
                if (GUIClip.visibleRect.Contains(Event.current.mousePosition) && DragAndDrop.GetGenericData("CustomDragID") != null)
                {
                  DragAndDrop.visualMode = !this.ilvState.rect.Contains(Event.current.mousePosition) ? DragAndDropVisualMode.None : DragAndDropVisualMode.Move;
                  Event.current.Use();
                  break;
                }
                break;
              case EventType.DragPerform:
                object genericData = DragAndDrop.GetGenericData("CustomDragID");
                if (GUIClip.visibleRect.Contains(Event.current.mousePosition) && genericData != null)
                {
                  this.ilvState.state.customDraggedFromID = (int) genericData;
                  DragAndDrop.AcceptDrag();
                  Event.current.Use();
                }
                GUIUtility.hotControl = 0;
                break;
              case EventType.DragExited:
                GUIUtility.hotControl = 0;
                break;
            }
          }
          if (this.ilvState.beganHorizontal)
          {
            EditorGUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            this.ilvState.beganHorizontal = false;
          }
          if (this.isLayouted)
          {
            GUILayoutUtility.EndLayoutGroup();
            EditorGUILayout.EndScrollView();
          }
          this.ilvState.wantsReordering = false;
          this.ilvState.wantsExternalFiles = false;
        }
        else if (this.isLayouted)
        {
          if (this.yPos != this.yFrom)
          {
            this.ilvStateL.group.ResetCursor();
            this.ilvStateL.group.AddY();
          }
          else
            this.ilvStateL.group.AddY((float) (this.ilvState.invisibleRows * this.ilvState.state.rowHeight));
        }
        if (this.isLayouted)
        {
          if (!this.quiting)
            GUILayout.BeginHorizontal(GUIStyle.none, new GUILayoutOption[0]);
          else
            GUILayout.EndHorizontal();
        }
        return !this.quiting;
      }

      public void Reset()
      {
        this.xPos = -1;
        this.yPos = this.yFrom;
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) this;
      }

      public void Dispose()
      {
      }
    }
  }
}
