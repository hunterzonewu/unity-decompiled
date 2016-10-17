// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor
{
  internal static class TreeViewUtility
  {
    public static List<TreeViewItem> FindItemsInList(IEnumerable<int> itemIDs, List<TreeViewItem> treeViewItems)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return treeViewItems.Where<TreeViewItem>(new Func<TreeViewItem, bool>(new TreeViewUtility.\u003CFindItemsInList\u003Ec__AnonStorey7B() { itemIDs = itemIDs }.\u003C\u003Em__11B)).ToList<TreeViewItem>();
    }

    public static TreeViewItem FindItemInList<T>(int id, List<T> treeViewItems) where T : TreeViewItem
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      return (TreeViewItem) treeViewItems.FirstOrDefault<T>(new Func<T, bool>(new TreeViewUtility.\u003CFindItemInList\u003Ec__AnonStorey7C<T>() { id = id }.\u003C\u003Em__11C));
    }

    public static TreeViewItem FindItem(int id, TreeViewItem searchFromThisItem)
    {
      return TreeViewUtility.FindItemRecursive(id, searchFromThisItem);
    }

    private static TreeViewItem FindItemRecursive(int id, TreeViewItem item)
    {
      if (item == null)
        return (TreeViewItem) null;
      if (item.id == id)
        return item;
      if (!item.hasChildren)
        return (TreeViewItem) null;
      using (List<TreeViewItem>.Enumerator enumerator = item.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TreeViewItem current = enumerator.Current;
          TreeViewItem itemRecursive = TreeViewUtility.FindItemRecursive(id, current);
          if (itemRecursive != null)
            return itemRecursive;
        }
      }
      return (TreeViewItem) null;
    }

    public static void DebugPrintToEditorLogRecursive(TreeViewItem item)
    {
      if (item == null)
        return;
      Console.WriteLine(new string(' ', item.depth * 3) + item.displayName);
      if (!item.hasChildren)
        return;
      using (List<TreeViewItem>.Enumerator enumerator = item.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          TreeViewUtility.DebugPrintToEditorLogRecursive(enumerator.Current);
      }
    }

    public static void SetChildParentReferences(List<TreeViewItem> visibleItems, TreeViewItem root)
    {
      for (int index = 0; index < visibleItems.Count; ++index)
        visibleItems[index].parent = (TreeViewItem) null;
      int capacity = 0;
      for (int parentIndex = 0; parentIndex < visibleItems.Count; ++parentIndex)
      {
        TreeViewUtility.SetChildParentReferences(parentIndex, visibleItems);
        if (visibleItems[parentIndex].parent == null)
          ++capacity;
      }
      if (capacity <= 0)
        return;
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>(capacity);
      for (int index = 0; index < visibleItems.Count; ++index)
      {
        if (visibleItems[index].parent == null)
        {
          treeViewItemList.Add(visibleItems[index]);
          visibleItems[index].parent = root;
        }
      }
      root.children = treeViewItemList;
    }

    private static void SetChildren(TreeViewItem item, List<TreeViewItem> newChildList)
    {
      if (LazyTreeViewDataSource.IsChildListForACollapsedParent(item.children) && newChildList == null)
        return;
      item.children = newChildList;
    }

    private static void SetChildParentReferences(int parentIndex, List<TreeViewItem> visibleItems)
    {
      TreeViewItem visibleItem = visibleItems[parentIndex];
      if (visibleItem.children != null && visibleItem.children.Count > 0 && visibleItem.children[0] != null)
        return;
      int depth = visibleItem.depth;
      int capacity = 0;
      for (int index = parentIndex + 1; index < visibleItems.Count; ++index)
      {
        if (visibleItems[index].depth == depth + 1)
          ++capacity;
        if (visibleItems[index].depth <= depth)
          break;
      }
      List<TreeViewItem> newChildList = (List<TreeViewItem>) null;
      if (capacity != 0)
      {
        newChildList = new List<TreeViewItem>(capacity);
        int num = 0;
        for (int index = parentIndex + 1; index < visibleItems.Count; ++index)
        {
          if (visibleItems[index].depth == depth + 1)
          {
            visibleItems[index].parent = visibleItem;
            newChildList.Add(visibleItems[index]);
            ++num;
          }
          if (visibleItems[index].depth <= depth)
            break;
        }
      }
      TreeViewUtility.SetChildren(visibleItem, newChildList);
    }
  }
}
