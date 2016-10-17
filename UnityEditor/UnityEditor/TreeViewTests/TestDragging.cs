// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewTests.TestDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.TreeViewTests
{
  internal class TestDragging : TreeViewDragging
  {
    private const string k_GenericDragID = "FooDragging";
    private BackendData m_BackendData;

    public TestDragging(TreeView treeView, BackendData data)
      : base(treeView)
    {
      this.m_BackendData = data;
    }

    public override void StartDrag(TreeViewItem draggedNode, List<int> draggedItemIDs)
    {
      DragAndDrop.PrepareStartDrag();
      DragAndDrop.SetGenericData("FooDragging", (object) new TestDragging.FooDragData(this.GetItemsFromIDs((IEnumerable<int>) draggedItemIDs)));
      DragAndDrop.objectReferences = new UnityEngine.Object[0];
      DragAndDrop.StartDrag(draggedItemIDs.Count.ToString() + " Foo" + (draggedItemIDs.Count <= 1 ? (object) string.Empty : (object) "s"));
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      TestDragging.FooDragData genericData = DragAndDrop.GetGenericData("FooDragging") as TestDragging.FooDragData;
      FooTreeViewItem fooTreeViewItem1 = targetItem as FooTreeViewItem;
      FooTreeViewItem fooTreeViewItem2 = parentItem as FooTreeViewItem;
      if (fooTreeViewItem2 == null || genericData == null)
        return DragAndDropVisualMode.None;
      bool flag = this.ValidDrag(parentItem, genericData.m_DraggedItems);
      if (perform && flag)
      {
        List<BackendData.Foo> list = genericData.m_DraggedItems.Where<TreeViewItem>((Func<TreeViewItem, bool>) (x => x is FooTreeViewItem)).Select<TreeViewItem, BackendData.Foo>((Func<TreeViewItem, BackendData.Foo>) (x => ((FooTreeViewItem) x).foo)).ToList<BackendData.Foo>();
        int[] array = genericData.m_DraggedItems.Where<TreeViewItem>((Func<TreeViewItem, bool>) (x => x is FooTreeViewItem)).Select<TreeViewItem, int>((Func<TreeViewItem, int>) (x => x.id)).ToArray<int>();
        this.m_BackendData.ReparentSelection(fooTreeViewItem2.foo, fooTreeViewItem1.foo, list);
        this.m_TreeView.ReloadData();
        this.m_TreeView.SetSelection(array, true);
      }
      return flag ? DragAndDropVisualMode.Move : DragAndDropVisualMode.None;
    }

    private bool ValidDrag(TreeViewItem parent, List<TreeViewItem> draggedItems)
    {
      for (TreeViewItem treeViewItem = parent; treeViewItem != null; treeViewItem = treeViewItem.parent)
      {
        if (draggedItems.Contains(treeViewItem))
          return false;
      }
      return true;
    }

    private List<TreeViewItem> GetItemsFromIDs(IEnumerable<int> draggedItemIDs)
    {
      return TreeViewUtility.FindItemsInList(draggedItemIDs, this.m_TreeView.data.GetRows());
    }

    private class FooDragData
    {
      public List<TreeViewItem> m_DraggedItems;

      public FooDragData(List<TreeViewItem> draggedItems)
      {
        this.m_DraggedItems = draggedItems;
      }
    }
  }
}
