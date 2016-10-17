// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class TreeViewDataSource : ITreeViewDataSource
  {
    protected bool m_NeedRefreshVisibleFolders = true;
    protected readonly TreeView m_TreeView;
    protected TreeViewItem m_RootItem;
    protected List<TreeViewItem> m_VisibleRows;
    protected TreeViewItem m_FakeItem;
    public System.Action onVisibleRowsChanged;

    public bool showRootNode { get; set; }

    public bool rootIsCollapsable { get; set; }

    public bool alwaysAddFirstItemToSearchResult { get; set; }

    public TreeViewItem root
    {
      get
      {
        return this.m_RootItem;
      }
    }

    protected List<int> expandedIDs
    {
      get
      {
        return this.m_TreeView.state.expandedIDs;
      }
      set
      {
        this.m_TreeView.state.expandedIDs = value;
      }
    }

    public virtual int rowCount
    {
      get
      {
        return this.GetRows().Count;
      }
    }

    public TreeViewDataSource(TreeView treeView)
    {
      this.m_TreeView = treeView;
      this.showRootNode = true;
      this.rootIsCollapsable = false;
    }

    public virtual void OnInitialize()
    {
    }

    public abstract void FetchData();

    public void ReloadData()
    {
      this.m_FakeItem = (TreeViewItem) null;
      this.FetchData();
    }

    public virtual TreeViewItem FindItem(int id)
    {
      return TreeViewUtility.FindItem(id, this.m_RootItem);
    }

    public virtual bool IsRevealed(int id)
    {
      return TreeView.GetIndexOfID(this.GetRows(), id) >= 0;
    }

    public virtual void RevealItem(int id)
    {
      if (this.IsRevealed(id))
        return;
      TreeViewItem treeViewItem = this.FindItem(id);
      if (treeViewItem == null)
        return;
      for (TreeViewItem parent = treeViewItem.parent; parent != null; parent = parent.parent)
        this.SetExpanded(parent, true);
    }

    public virtual void OnSearchChanged()
    {
      this.m_NeedRefreshVisibleFolders = true;
    }

    protected void GetVisibleItemsRecursive(TreeViewItem item, List<TreeViewItem> items)
    {
      if (item != this.m_RootItem || this.showRootNode)
        items.Add(item);
      if (!item.hasChildren || !this.IsExpanded(item))
        return;
      using (List<TreeViewItem>.Enumerator enumerator = item.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.GetVisibleItemsRecursive(enumerator.Current, items);
      }
    }

    protected void SearchRecursive(TreeViewItem item, string search, List<TreeViewItem> searchResult)
    {
      if (item.displayName.ToLower().Contains(search))
        searchResult.Add(item);
      if (item.children == null)
        return;
      using (List<TreeViewItem>.Enumerator enumerator = item.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.SearchRecursive(enumerator.Current, search, searchResult);
      }
    }

    protected virtual List<TreeViewItem> ExpandedRows(TreeViewItem root)
    {
      List<TreeViewItem> items = new List<TreeViewItem>();
      this.GetVisibleItemsRecursive(this.m_RootItem, items);
      return items;
    }

    protected virtual List<TreeViewItem> Search(TreeViewItem root, string search)
    {
      List<TreeViewItem> searchResult = new List<TreeViewItem>();
      if (this.showRootNode)
      {
        this.SearchRecursive(root, search, searchResult);
        searchResult.Sort((IComparer<TreeViewItem>) new TreeViewItemAlphaNumericSort());
      }
      else
      {
        int num = !this.alwaysAddFirstItemToSearchResult ? 0 : 1;
        if (root.hasChildren)
        {
          for (int index = num; index < root.children.Count; ++index)
            this.SearchRecursive(root.children[index], search, searchResult);
          searchResult.Sort((IComparer<TreeViewItem>) new TreeViewItemAlphaNumericSort());
          if (this.alwaysAddFirstItemToSearchResult)
            searchResult.Insert(0, root.children[0]);
        }
      }
      return searchResult;
    }

    public virtual int GetRow(int id)
    {
      List<TreeViewItem> rows = this.GetRows();
      for (int index = 0; index < rows.Count; ++index)
      {
        if (rows[index].id == id)
          return index;
      }
      return -1;
    }

    public virtual TreeViewItem GetItem(int row)
    {
      return this.GetRows()[row];
    }

    public virtual List<TreeViewItem> GetRows()
    {
      this.InitIfNeeded();
      return this.m_VisibleRows;
    }

    public virtual void InitIfNeeded()
    {
      if (this.m_VisibleRows != null && !this.m_NeedRefreshVisibleFolders)
        return;
      if (this.m_RootItem != null)
      {
        this.m_VisibleRows = !this.m_TreeView.isSearching ? this.ExpandedRows(this.m_RootItem) : this.Search(this.m_RootItem, this.m_TreeView.searchString.ToLower());
      }
      else
      {
        Debug.LogError((object) "TreeView root item is null. Ensure that your TreeViewDataSource sets up at least a root item.");
        this.m_VisibleRows = new List<TreeViewItem>();
      }
      this.m_NeedRefreshVisibleFolders = false;
      if (this.onVisibleRowsChanged != null)
        this.onVisibleRowsChanged();
      this.m_TreeView.Repaint();
    }

    public virtual int[] GetExpandedIDs()
    {
      return this.expandedIDs.ToArray();
    }

    public virtual void SetExpandedIDs(int[] ids)
    {
      this.expandedIDs = new List<int>((IEnumerable<int>) ids);
      this.expandedIDs.Sort();
      this.m_NeedRefreshVisibleFolders = true;
      this.OnExpandedStateChanged();
    }

    public virtual bool IsExpanded(int id)
    {
      return this.expandedIDs.BinarySearch(id) >= 0;
    }

    public virtual bool SetExpanded(int id, bool expand)
    {
      bool flag = this.IsExpanded(id);
      if (expand == flag)
        return false;
      if (expand)
      {
        this.expandedIDs.Add(id);
        this.expandedIDs.Sort();
      }
      else
        this.expandedIDs.Remove(id);
      this.m_NeedRefreshVisibleFolders = true;
      this.OnExpandedStateChanged();
      return true;
    }

    public virtual void SetExpandedWithChildren(TreeViewItem fromItem, bool expand)
    {
      Stack<TreeViewItem> treeViewItemStack = new Stack<TreeViewItem>();
      treeViewItemStack.Push(fromItem);
      HashSet<int> intSet = new HashSet<int>();
      while (treeViewItemStack.Count > 0)
      {
        TreeViewItem treeViewItem = treeViewItemStack.Pop();
        if (treeViewItem.hasChildren)
        {
          intSet.Add(treeViewItem.id);
          using (List<TreeViewItem>.Enumerator enumerator = treeViewItem.children.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              TreeViewItem current = enumerator.Current;
              treeViewItemStack.Push(current);
            }
          }
        }
      }
      HashSet<int> source = new HashSet<int>((IEnumerable<int>) this.expandedIDs);
      if (expand)
        source.UnionWith((IEnumerable<int>) intSet);
      else
        source.ExceptWith((IEnumerable<int>) intSet);
      this.SetExpandedIDs(source.ToArray<int>());
    }

    public virtual void SetExpanded(TreeViewItem item, bool expand)
    {
      this.SetExpanded(item.id, expand);
    }

    public virtual bool IsExpanded(TreeViewItem item)
    {
      return this.IsExpanded(item.id);
    }

    public virtual bool IsExpandable(TreeViewItem item)
    {
      if (this.m_TreeView.isSearching)
        return false;
      return item.hasChildren;
    }

    public virtual bool CanBeMultiSelected(TreeViewItem item)
    {
      return true;
    }

    public virtual bool CanBeParent(TreeViewItem item)
    {
      return true;
    }

    public virtual void OnExpandedStateChanged()
    {
      if (this.m_TreeView.expandedStateChanged == null)
        return;
      this.m_TreeView.expandedStateChanged();
    }

    public virtual bool IsRenamingItemAllowed(TreeViewItem item)
    {
      return true;
    }

    public virtual void InsertFakeItem(int id, int parentID, string name, Texture2D icon)
    {
      Debug.LogError((object) "InsertFakeItem missing implementation");
    }

    public virtual bool HasFakeItem()
    {
      return this.m_FakeItem != null;
    }

    public virtual void RemoveFakeItem()
    {
      if (!this.HasFakeItem())
        return;
      List<TreeViewItem> rows = this.GetRows();
      int indexOfId = TreeView.GetIndexOfID(rows, this.m_FakeItem.id);
      if (indexOfId != -1)
        rows.RemoveAt(indexOfId);
      this.m_FakeItem = (TreeViewItem) null;
    }
  }
}
