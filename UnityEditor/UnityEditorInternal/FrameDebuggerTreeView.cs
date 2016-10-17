// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FrameDebuggerTreeView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class FrameDebuggerTreeView
  {
    internal readonly TreeView m_TreeView;
    internal FrameDebuggerTreeView.FDTreeViewDataSource m_DataSource;
    private readonly FrameDebuggerWindow m_FrameDebugger;

    public FrameDebuggerTreeView(FrameDebuggerEvent[] frameEvents, TreeViewState treeViewState, FrameDebuggerWindow window, Rect startRect)
    {
      this.m_FrameDebugger = window;
      this.m_TreeView = new TreeView((EditorWindow) window, treeViewState);
      this.m_DataSource = new FrameDebuggerTreeView.FDTreeViewDataSource(this.m_TreeView, frameEvents);
      FrameDebuggerTreeView.FDTreeViewGUI fdTreeViewGui = new FrameDebuggerTreeView.FDTreeViewGUI(this.m_TreeView);
      this.m_TreeView.Init(startRect, (ITreeViewDataSource) this.m_DataSource, (ITreeViewGUI) fdTreeViewGui, (ITreeViewDragging) null);
      this.m_TreeView.ReloadData();
      this.m_TreeView.selectionChangedCallback += new System.Action<int[]>(this.SelectionChanged);
    }

    private void SelectionChanged(int[] selectedIDs)
    {
      if (selectedIDs.Length < 1)
        return;
      int selectedId = selectedIDs[0];
      int newLimit = selectedId;
      if (newLimit <= 0)
      {
        FrameDebuggerTreeView.FDTreeViewItem node = this.m_TreeView.FindNode(selectedId) as FrameDebuggerTreeView.FDTreeViewItem;
        if (node != null)
          newLimit = node.m_EventIndex;
      }
      if (newLimit <= 0)
        return;
      this.m_FrameDebugger.ChangeFrameEventLimit(newLimit);
    }

    public void SelectFrameEventIndex(int eventIndex)
    {
      int[] selection = this.m_TreeView.GetSelection();
      if (selection.Length > 0)
      {
        FrameDebuggerTreeView.FDTreeViewItem node = this.m_TreeView.FindNode(selection[0]) as FrameDebuggerTreeView.FDTreeViewItem;
        if (node != null && eventIndex == node.m_EventIndex)
          return;
      }
      this.m_TreeView.SetSelection(new int[1]{ eventIndex }, 1 != 0);
    }

    public void OnGUI(Rect rect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard);
      this.m_TreeView.OnGUI(rect, controlId);
    }

    private class FDTreeViewItem : TreeViewItem
    {
      public FrameDebuggerEvent m_FrameEvent;
      public int m_ChildEventCount;
      public int m_EventIndex;

      public FDTreeViewItem(int id, int depth, FrameDebuggerTreeView.FDTreeViewItem parent, string displayName)
        : base(id, depth, (TreeViewItem) parent, displayName)
      {
        this.m_EventIndex = id;
      }
    }

    private class FDTreeViewGUI : TreeViewGUI
    {
      private const float kSmallMargin = 4f;

      public FDTreeViewGUI(TreeView treeView)
        : base(treeView)
      {
      }

      protected override Texture GetIconForNode(TreeViewItem item)
      {
        return (Texture) null;
      }

      protected override void DrawIconAndLabel(Rect rect, TreeViewItem itemRaw, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
      {
        FrameDebuggerTreeView.FDTreeViewItem fdTreeViewItem = (FrameDebuggerTreeView.FDTreeViewItem) itemRaw;
        float contentIndent = this.GetContentIndent((TreeViewItem) fdTreeViewItem);
        rect.x += contentIndent;
        rect.width -= contentIndent;
        if (fdTreeViewItem.m_ChildEventCount > 0)
        {
          Rect position = rect;
          position.width -= 4f;
          GUIContent content = EditorGUIUtility.TempContent(fdTreeViewItem.m_ChildEventCount.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          GUIStyle rowTextRight = FrameDebuggerWindow.styles.rowTextRight;
          rowTextRight.Draw(position, content, false, false, false, false);
          rect.width -= rowTextRight.CalcSize(content).x + 8f;
        }
        string t = fdTreeViewItem.id > 0 ? FrameDebuggerWindow.s_FrameEventTypeNames[(int) fdTreeViewItem.m_FrameEvent.type] + fdTreeViewItem.displayName : fdTreeViewItem.displayName;
        if (string.IsNullOrEmpty(t))
          t = "<unknown scope>";
        GUIContent content1 = EditorGUIUtility.TempContent(t);
        FrameDebuggerWindow.styles.rowText.Draw(rect, content1, false, false, false, selected && focused);
      }

      protected override void RenameEnded()
      {
      }
    }

    internal class FDTreeViewDataSource : TreeViewDataSource
    {
      private FrameDebuggerEvent[] m_FrameEvents;

      public FDTreeViewDataSource(TreeView treeView, FrameDebuggerEvent[] frameEvents)
        : base(treeView)
      {
        this.m_FrameEvents = frameEvents;
        this.rootIsCollapsable = false;
        this.showRootNode = false;
      }

      public void SetEvents(FrameDebuggerEvent[] frameEvents)
      {
        bool flag = this.m_FrameEvents == null || this.m_FrameEvents.Length < 1;
        this.m_FrameEvents = frameEvents;
        this.m_NeedRefreshVisibleFolders = true;
        this.ReloadData();
        if (!flag)
          return;
        this.SetExpandedWithChildren(this.m_RootItem, true);
      }

      public override bool IsRenamingItemAllowed(TreeViewItem item)
      {
        return false;
      }

      public override bool CanBeMultiSelected(TreeViewItem item)
      {
        return false;
      }

      private static void CloseLastHierarchyLevel(List<FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel> eventStack, int prevFrameEventIndex)
      {
        int index = eventStack.Count - 1;
        eventStack[index].item.children = eventStack[index].children;
        eventStack[index].item.m_EventIndex = prevFrameEventIndex;
        if (eventStack[index].item.parent != null)
          ((FrameDebuggerTreeView.FDTreeViewItem) eventStack[index].item.parent).m_ChildEventCount += eventStack[index].item.m_ChildEventCount;
        eventStack.RemoveAt(index);
      }

      public override void FetchData()
      {
        FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel treeHierarchyLevel1 = new FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel(0, 0, string.Empty, (FrameDebuggerTreeView.FDTreeViewItem) null);
        List<FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel> eventStack = new List<FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel>();
        eventStack.Add(treeHierarchyLevel1);
        int num = -1;
        for (int index1 = 0; index1 < this.m_FrameEvents.Length; ++index1)
        {
          string[] strArray = ("/" + (FrameDebuggerUtility.GetFrameEventInfoName(index1) ?? string.Empty)).Split('/');
          int index2 = 0;
          while (index2 < eventStack.Count && index2 < strArray.Length && !(strArray[index2] != eventStack[index2].item.displayName))
            ++index2;
          while (eventStack.Count > 0 && eventStack.Count > index2)
            FrameDebuggerTreeView.FDTreeViewDataSource.CloseLastHierarchyLevel(eventStack, index1);
          for (int index3 = index2; index3 < strArray.Length; ++index3)
          {
            FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel treeHierarchyLevel2 = eventStack[eventStack.Count - 1];
            FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel treeHierarchyLevel3 = new FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel(eventStack.Count - 1, --num, strArray[index3], treeHierarchyLevel2.item);
            treeHierarchyLevel2.children.Add((TreeViewItem) treeHierarchyLevel3.item);
            eventStack.Add(treeHierarchyLevel3);
          }
          GameObject frameEventGameObject = FrameDebuggerUtility.GetFrameEventGameObject(index1);
          string displayName = !(bool) ((UnityEngine.Object) frameEventGameObject) ? string.Empty : " " + frameEventGameObject.name;
          FrameDebuggerTreeView.FDTreeViewDataSource.FDTreeHierarchyLevel treeHierarchyLevel4 = eventStack[eventStack.Count - 1];
          treeHierarchyLevel4.children.Add((TreeViewItem) new FrameDebuggerTreeView.FDTreeViewItem(index1 + 1, eventStack.Count - 1, treeHierarchyLevel4.item, displayName)
          {
            m_FrameEvent = this.m_FrameEvents[index1]
          });
          ++treeHierarchyLevel4.item.m_ChildEventCount;
        }
        while (eventStack.Count > 0)
          FrameDebuggerTreeView.FDTreeViewDataSource.CloseLastHierarchyLevel(eventStack, this.m_FrameEvents.Length);
        this.m_RootItem = (TreeViewItem) treeHierarchyLevel1.item;
      }

      private class FDTreeHierarchyLevel
      {
        internal readonly FrameDebuggerTreeView.FDTreeViewItem item;
        internal readonly List<TreeViewItem> children;

        internal FDTreeHierarchyLevel(int depth, int id, string name, FrameDebuggerTreeView.FDTreeViewItem parent)
        {
          this.item = new FrameDebuggerTreeView.FDTreeViewItem(id, depth, parent, name);
          this.children = new List<TreeViewItem>();
        }
      }
    }
  }
}
