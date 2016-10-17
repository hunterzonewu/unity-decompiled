// Decompiled with JetBrains decompiler
// Type: UnityEditor.LazyTreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;

namespace UnityEditor
{
  internal abstract class LazyTreeViewDataSource : TreeViewDataSource
  {
    public LazyTreeViewDataSource(TreeView treeView)
      : base(treeView)
    {
    }

    public static List<TreeViewItem> CreateChildListForCollapsedParent()
    {
      return new List<TreeViewItem>() { (TreeViewItem) null };
    }

    public static bool IsChildListForACollapsedParent(List<TreeViewItem> childList)
    {
      if (childList != null && childList.Count == 1)
        return childList[0] == null;
      return false;
    }

    protected abstract HashSet<int> GetParentsAbove(int id);

    protected abstract HashSet<int> GetParentsBelow(int id);

    public override void RevealItem(int itemID)
    {
      HashSet<int> source = new HashSet<int>((IEnumerable<int>) this.expandedIDs);
      int count = source.Count;
      HashSet<int> parentsAbove = this.GetParentsAbove(itemID);
      source.UnionWith((IEnumerable<int>) parentsAbove);
      if (count == source.Count)
        return;
      this.SetExpandedIDs(source.ToArray<int>());
      if (!this.m_NeedRefreshVisibleFolders)
        return;
      this.FetchData();
    }

    public override TreeViewItem FindItem(int itemID)
    {
      this.RevealItem(itemID);
      return base.FindItem(itemID);
    }

    public override void SetExpandedWithChildren(TreeViewItem item, bool expand)
    {
      HashSet<int> source = new HashSet<int>((IEnumerable<int>) this.expandedIDs);
      HashSet<int> parentsBelow = this.GetParentsBelow(item.id);
      if (expand)
        source.UnionWith((IEnumerable<int>) parentsBelow);
      else
        source.ExceptWith((IEnumerable<int>) parentsBelow);
      this.SetExpandedIDs(source.ToArray<int>());
    }

    public override bool SetExpanded(int id, bool expand)
    {
      if (!base.SetExpanded(id, expand))
        return false;
      InternalEditorUtility.expandedProjectWindowItems = this.expandedIDs.ToArray();
      return true;
    }

    public override void InitIfNeeded()
    {
      if (this.m_VisibleRows != null && !this.m_NeedRefreshVisibleFolders)
        return;
      this.FetchData();
      this.m_NeedRefreshVisibleFolders = false;
      if (this.onVisibleRowsChanged != null)
        this.onVisibleRowsChanged();
      this.m_TreeView.Repaint();
    }

    public override List<TreeViewItem> GetRows()
    {
      this.InitIfNeeded();
      return this.m_VisibleRows;
    }
  }
}
