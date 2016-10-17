// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class TreeViewDragging : ITreeViewDragging
  {
    protected TreeViewDragging.DropData m_DropData = new TreeViewDragging.DropData();
    private const double k_DropExpandTimeout = 0.7;
    protected TreeView m_TreeView;

    public bool drawRowMarkerAbove { get; set; }

    public TreeViewDragging(TreeView treeView)
    {
      this.m_TreeView = treeView;
    }

    public virtual void OnInitialize()
    {
    }

    public int GetDropTargetControlID()
    {
      return this.m_DropData.dropTargetControlID;
    }

    public int GetRowMarkerControlID()
    {
      return this.m_DropData.rowMarkerControlID;
    }

    public virtual bool CanStartDrag(TreeViewItem targetItem, List<int> draggedItemIDs, Vector2 mouseDownPosition)
    {
      return true;
    }

    public abstract void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs);

    public abstract DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPosition);

    public virtual bool DragElement(TreeViewItem targetItem, Rect targetItemRect, bool firstItem)
    {
      if (targetItem == null)
      {
        if (this.m_DropData != null)
        {
          this.m_DropData.dropTargetControlID = 0;
          this.m_DropData.rowMarkerControlID = 0;
        }
        bool perform = Event.current.type == EventType.DragPerform;
        DragAndDrop.visualMode = this.DoDrag((TreeViewItem) null, (TreeViewItem) null, perform, TreeViewDragging.DropPosition.Below);
        if (DragAndDrop.visualMode != DragAndDropVisualMode.None && perform)
          this.FinalizeDragPerformed(true);
        return false;
      }
      Vector2 mousePosition = Event.current.mousePosition;
      bool flag = this.m_TreeView.data.CanBeParent(targetItem);
      Rect rect = targetItemRect;
      float betweenHalfHeight = !flag ? targetItemRect.height * 0.5f : this.m_TreeView.gui.halfDropBetweenHeight;
      if (firstItem)
        rect.yMin -= betweenHalfHeight;
      rect.yMax += betweenHalfHeight;
      if (!rect.Contains(mousePosition))
        return false;
      TreeViewDragging.DropPosition dropPosition = (double) mousePosition.y < (double) targetItemRect.yMax - (double) betweenHalfHeight ? (!firstItem || (double) mousePosition.y > (double) targetItemRect.yMin + (double) betweenHalfHeight ? (!flag ? TreeViewDragging.DropPosition.Above : TreeViewDragging.DropPosition.Upon) : TreeViewDragging.DropPosition.Above) : TreeViewDragging.DropPosition.Below;
      TreeViewItem parentItem = !this.m_TreeView.data.IsExpanded(targetItem) || !targetItem.hasChildren ? targetItem.parent : targetItem;
      DragAndDropVisualMode andDropVisualMode1 = DragAndDropVisualMode.None;
      if (Event.current.type == EventType.DragPerform)
      {
        if (dropPosition == TreeViewDragging.DropPosition.Upon)
          andDropVisualMode1 = this.DoDrag(targetItem, targetItem, true, dropPosition);
        if (andDropVisualMode1 == DragAndDropVisualMode.None && parentItem != null)
          andDropVisualMode1 = this.DoDrag(parentItem, targetItem, true, dropPosition);
        if (andDropVisualMode1 != DragAndDropVisualMode.None)
        {
          this.FinalizeDragPerformed(false);
        }
        else
        {
          this.DragCleanup(true);
          this.m_TreeView.NotifyListenersThatDragEnded((int[]) null, false);
        }
      }
      else
      {
        if (this.m_DropData == null)
          this.m_DropData = new TreeViewDragging.DropData();
        this.m_DropData.dropTargetControlID = 0;
        this.m_DropData.rowMarkerControlID = 0;
        int itemControlId = TreeView.GetItemControlID(targetItem);
        this.HandleAutoExpansion(itemControlId, targetItem, targetItemRect, betweenHalfHeight, mousePosition);
        if (dropPosition == TreeViewDragging.DropPosition.Upon)
          andDropVisualMode1 = this.DoDrag(targetItem, targetItem, false, dropPosition);
        if (andDropVisualMode1 != DragAndDropVisualMode.None)
        {
          this.m_DropData.dropTargetControlID = itemControlId;
          DragAndDrop.visualMode = andDropVisualMode1;
        }
        else if (targetItem != null && parentItem != null)
        {
          DragAndDropVisualMode andDropVisualMode2 = this.DoDrag(parentItem, targetItem, false, dropPosition);
          if (andDropVisualMode2 != DragAndDropVisualMode.None)
          {
            this.drawRowMarkerAbove = dropPosition == TreeViewDragging.DropPosition.Above;
            this.m_DropData.rowMarkerControlID = itemControlId;
            this.m_DropData.dropTargetControlID = !this.drawRowMarkerAbove ? TreeView.GetItemControlID(parentItem) : 0;
            DragAndDrop.visualMode = andDropVisualMode2;
          }
        }
      }
      Event.current.Use();
      return true;
    }

    private void FinalizeDragPerformed(bool revertExpanded)
    {
      this.DragCleanup(revertExpanded);
      DragAndDrop.AcceptDrag();
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences);
      bool draggedItemsFromOwnTreeView = true;
      if (objectList.Count > 0 && objectList[0] != (UnityEngine.Object) null && TreeViewUtility.FindItemInList<TreeViewItem>(objectList[0].GetInstanceID(), this.m_TreeView.data.GetRows()) == null)
        draggedItemsFromOwnTreeView = false;
      int[] draggedIDs = new int[objectList.Count];
      for (int index = 0; index < objectList.Count; ++index)
      {
        if (!(objectList[index] == (UnityEngine.Object) null))
          draggedIDs[index] = objectList[index].GetInstanceID();
      }
      this.m_TreeView.NotifyListenersThatDragEnded(draggedIDs, draggedItemsFromOwnTreeView);
    }

    protected virtual void HandleAutoExpansion(int itemControlID, TreeViewItem targetItem, Rect targetItemRect, float betweenHalfHeight, Vector2 currentMousePos)
    {
      float contentIndent = this.m_TreeView.gui.GetContentIndent(targetItem);
      bool flag1 = new Rect(targetItemRect.x + contentIndent, targetItemRect.y + betweenHalfHeight, targetItemRect.width - contentIndent, targetItemRect.height - betweenHalfHeight * 2f).Contains(currentMousePos);
      if (itemControlID != this.m_DropData.lastControlID || !flag1 || this.m_DropData.expandItemBeginPosition != currentMousePos)
      {
        this.m_DropData.lastControlID = itemControlID;
        this.m_DropData.expandItemBeginTimer = (double) Time.realtimeSinceStartup;
        this.m_DropData.expandItemBeginPosition = currentMousePos;
      }
      bool flag2 = (double) Time.realtimeSinceStartup - this.m_DropData.expandItemBeginTimer > 0.7;
      bool flag3 = flag1 && flag2;
      if (targetItem == null || !flag3 || (!targetItem.hasChildren || this.m_TreeView.data.IsExpanded(targetItem)))
        return;
      if (this.m_DropData.expandedArrayBeforeDrag == null)
        this.m_DropData.expandedArrayBeforeDrag = this.GetCurrentExpanded().ToArray();
      this.m_TreeView.data.SetExpanded(targetItem, true);
      this.m_DropData.expandItemBeginTimer = (double) Time.realtimeSinceStartup;
      this.m_DropData.lastControlID = 0;
    }

    public virtual void DragCleanup(bool revertExpanded)
    {
      if (this.m_DropData == null)
        return;
      if (this.m_DropData.expandedArrayBeforeDrag != null && revertExpanded)
        this.RestoreExpanded(new List<int>((IEnumerable<int>) this.m_DropData.expandedArrayBeforeDrag));
      this.m_DropData = new TreeViewDragging.DropData();
    }

    public List<int> GetCurrentExpanded()
    {
      return this.m_TreeView.data.GetRows().Where<TreeViewItem>((Func<TreeViewItem, bool>) (item => this.m_TreeView.data.IsExpanded(item))).Select<TreeViewItem, int>((Func<TreeViewItem, int>) (item => item.id)).ToList<int>();
    }

    public void RestoreExpanded(List<int> ids)
    {
      using (List<TreeViewItem>.Enumerator enumerator = this.m_TreeView.data.GetRows().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TreeViewItem current = enumerator.Current;
          this.m_TreeView.data.SetExpanded(current, ids.Contains(current.id));
        }
      }
    }

    protected class DropData
    {
      public int[] expandedArrayBeforeDrag;
      public int lastControlID;
      public int dropTargetControlID;
      public int rowMarkerControlID;
      public double expandItemBeginTimer;
      public Vector2 expandItemBeginPosition;
    }

    public enum DropPosition
    {
      Upon,
      Below,
      Above,
    }
  }
}
