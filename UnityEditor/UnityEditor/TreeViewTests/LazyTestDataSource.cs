// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewTests.LazyTestDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor.TreeViewTests
{
  internal class LazyTestDataSource : LazyTreeViewDataSource
  {
    private BackendData m_Backend;

    public int itemCounter { get; private set; }

    public LazyTestDataSource(TreeView treeView, BackendData data)
      : base(treeView)
    {
      this.m_Backend = data;
      this.FetchData();
    }

    public override void FetchData()
    {
      this.itemCounter = 1;
      this.m_RootItem = (TreeViewItem) new FooTreeViewItem(this.m_Backend.root.id, 0, (TreeViewItem) null, this.m_Backend.root.name, this.m_Backend.root);
      this.AddVisibleChildrenRecursive(this.m_Backend.root, this.m_RootItem);
      this.m_VisibleRows = new List<TreeViewItem>();
      this.GetVisibleItemsRecursive(this.m_RootItem, this.m_VisibleRows);
      this.m_NeedRefreshVisibleFolders = false;
    }

    private void AddVisibleChildrenRecursive(BackendData.Foo source, TreeViewItem dest)
    {
      if (this.IsExpanded(source.id))
      {
        if (source.children == null || source.children.Count <= 0)
          return;
        dest.children = new List<TreeViewItem>(source.children.Count);
        for (int index = 0; index < source.children.Count; ++index)
        {
          BackendData.Foo child = source.children[index];
          dest.children.Add((TreeViewItem) new FooTreeViewItem(child.id, dest.depth + 1, dest, child.name, child));
          ++this.itemCounter;
          this.AddVisibleChildrenRecursive(child, dest.children[index]);
        }
      }
      else
      {
        if (!source.hasChildren)
          return;
        dest.children = new List<TreeViewItem>()
        {
          new TreeViewItem(-1, -1, (TreeViewItem) null, string.Empty)
        };
      }
    }

    public override bool CanBeParent(TreeViewItem item)
    {
      return item.hasChildren;
    }

    protected override HashSet<int> GetParentsAbove(int id)
    {
      HashSet<int> intSet = new HashSet<int>();
      for (BackendData.Foo foo = BackendData.FindNodeRecursive(this.m_Backend.root, id); foo != null; foo = foo.parent)
      {
        if (foo.parent != null)
          intSet.Add(foo.parent.id);
      }
      return intSet;
    }

    protected override HashSet<int> GetParentsBelow(int id)
    {
      return this.m_Backend.GetParentsBelow(id);
    }
  }
}
