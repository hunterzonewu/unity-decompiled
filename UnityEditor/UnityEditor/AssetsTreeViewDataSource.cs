// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetsTreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetsTreeViewDataSource : LazyTreeViewDataSource
  {
    private const HierarchyType k_HierarchyType = HierarchyType.Assets;
    private readonly int m_RootInstanceID;

    public bool foldersOnly { get; set; }

    public bool foldersFirst { get; set; }

    public AssetsTreeViewDataSource(TreeView treeView, int rootInstanceID, bool showRootNode, bool rootNodeIsCollapsable)
      : base(treeView)
    {
      this.m_RootInstanceID = rootInstanceID;
      this.showRootNode = showRootNode;
      this.rootIsCollapsable = rootNodeIsCollapsable;
    }

    private static string CreateDisplayName(int instanceID)
    {
      return Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(instanceID));
    }

    public override void FetchData()
    {
      this.m_RootItem = new TreeViewItem(this.m_RootInstanceID, 0, (TreeViewItem) null, AssetsTreeViewDataSource.CreateDisplayName(this.m_RootInstanceID));
      if (!this.showRootNode)
        this.SetExpanded(this.m_RootItem, true);
      IHierarchyProperty hierarchyProperty = (IHierarchyProperty) new HierarchyProperty(HierarchyType.Assets);
      hierarchyProperty.Reset();
      if (!hierarchyProperty.Find(this.m_RootInstanceID, (int[]) null))
        Debug.LogError((object) ("Root Asset with id " + (object) this.m_RootInstanceID + " not found!!"));
      int minDepth = hierarchyProperty.depth + (!this.showRootNode ? 1 : 0);
      int[] array = this.expandedIDs.ToArray();
      Texture2D texture = EditorGUIUtility.FindTexture(EditorResourcesUtility.emptyFolderIconName);
      this.m_VisibleRows = new List<TreeViewItem>();
      while (hierarchyProperty.NextWithDepthCheck(array, minDepth))
      {
        if (!this.foldersOnly || hierarchyProperty.isFolder)
        {
          int depth = hierarchyProperty.depth - minDepth;
          TreeViewItem treeViewItem = !hierarchyProperty.isFolder ? (TreeViewItem) new AssetsTreeViewDataSource.NonFolderTreeItem(hierarchyProperty.instanceID, depth, (TreeViewItem) null, hierarchyProperty.name) : (TreeViewItem) new AssetsTreeViewDataSource.FolderTreeItem(hierarchyProperty.instanceID, depth, (TreeViewItem) null, hierarchyProperty.name);
          treeViewItem.icon = !hierarchyProperty.isFolder || hierarchyProperty.hasChildren ? hierarchyProperty.icon : texture;
          if (hierarchyProperty.hasChildren)
            treeViewItem.AddChild((TreeViewItem) null);
          this.m_VisibleRows.Add(treeViewItem);
        }
      }
      TreeViewUtility.SetChildParentReferences(this.m_VisibleRows, this.m_RootItem);
      if (this.foldersFirst)
      {
        AssetsTreeViewDataSource.FoldersFirstRecursive(this.m_RootItem);
        this.m_VisibleRows.Clear();
        this.GetVisibleItemsRecursive(this.m_RootItem, this.m_VisibleRows);
      }
      this.m_NeedRefreshVisibleFolders = false;
      this.m_TreeView.SetSelection(Selection.instanceIDs, false);
    }

    private static void FoldersFirstRecursive(TreeViewItem item)
    {
      if (!item.hasChildren)
        return;
      TreeViewItem[] array = item.children.ToArray();
      for (int sourceIndex = 0; sourceIndex < item.children.Count; ++sourceIndex)
      {
        if (array[sourceIndex] != null)
        {
          if (array[sourceIndex] is AssetsTreeViewDataSource.NonFolderTreeItem)
          {
            for (int index = sourceIndex + 1; index < array.Length; ++index)
            {
              if (array[index] is AssetsTreeViewDataSource.FolderTreeItem)
              {
                TreeViewItem treeViewItem = array[index];
                int length = index - sourceIndex;
                Array.Copy((Array) array, sourceIndex, (Array) array, sourceIndex + 1, length);
                array[sourceIndex] = treeViewItem;
                break;
              }
            }
          }
          AssetsTreeViewDataSource.FoldersFirstRecursive(array[sourceIndex]);
        }
      }
      item.children = new List<TreeViewItem>((IEnumerable<TreeViewItem>) array);
    }

    protected override HashSet<int> GetParentsAbove(int id)
    {
      return new HashSet<int>((IEnumerable<int>) ProjectWindowUtil.GetAncestors(id));
    }

    protected override HashSet<int> GetParentsBelow(int id)
    {
      HashSet<int> intSet = new HashSet<int>();
      IHierarchyProperty hierarchyProperty = (IHierarchyProperty) new HierarchyProperty(HierarchyType.Assets);
      if (hierarchyProperty.Find(id, (int[]) null))
      {
        intSet.Add(id);
        int depth = hierarchyProperty.depth;
        while (hierarchyProperty.Next((int[]) null) && hierarchyProperty.depth > depth)
        {
          if (hierarchyProperty.hasChildren)
            intSet.Add(hierarchyProperty.instanceID);
        }
      }
      return intSet;
    }

    public override void OnExpandedStateChanged()
    {
      InternalEditorUtility.expandedProjectWindowItems = this.expandedIDs.ToArray();
      base.OnExpandedStateChanged();
    }

    public override bool IsRenamingItemAllowed(TreeViewItem item)
    {
      if (AssetDatabase.IsSubAsset(item.id))
        return false;
      return item.parent != null;
    }

    protected CreateAssetUtility GetCreateAssetUtility()
    {
      return this.m_TreeView.state.createAssetUtility;
    }

    public int GetInsertAfterItemIDForNewItem(string newName, TreeViewItem parentItem, bool isCreatingNewFolder, bool foldersFirst)
    {
      if (!parentItem.hasChildren)
        return parentItem.id;
      int num = parentItem.id;
      for (int index = 0; index < parentItem.children.Count; ++index)
      {
        int id = parentItem.children[index].id;
        bool flag = parentItem.children[index] is AssetsTreeViewDataSource.FolderTreeItem;
        if (foldersFirst && flag && !isCreatingNewFolder)
          num = id;
        else if ((!foldersFirst || flag || !isCreatingNewFolder) && EditorUtility.NaturalCompare(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(id)), newName) <= 0)
          num = id;
        else
          break;
      }
      return num;
    }

    public override void InsertFakeItem(int id, int parentID, string name, Texture2D icon)
    {
      bool isCreatingNewFolder = this.GetCreateAssetUtility().endAction is DoCreateFolder;
      TreeViewItem treeViewItem1 = this.FindItem(id);
      if (treeViewItem1 != null)
        Debug.LogError((object) ("Cannot insert fake Item because id is not unique " + (object) id + " Item already there: " + treeViewItem1.displayName));
      else if (this.FindItem(parentID) != null)
      {
        this.SetExpanded(parentID, true);
        List<TreeViewItem> rows = this.GetRows();
        int indexOfId1 = TreeView.GetIndexOfID(rows, parentID);
        TreeViewItem treeViewItem2 = indexOfId1 < 0 ? this.m_RootItem : rows[indexOfId1];
        int depth = treeViewItem2.depth + (treeViewItem2 != this.m_RootItem ? 1 : 0);
        this.m_FakeItem = new TreeViewItem(id, depth, treeViewItem2, name);
        this.m_FakeItem.icon = icon;
        int itemIdForNewItem = this.GetInsertAfterItemIDForNewItem(name, treeViewItem2, isCreatingNewFolder, this.foldersFirst);
        int indexOfId2 = TreeView.GetIndexOfID(rows, itemIdForNewItem);
        if (indexOfId2 >= 0)
        {
          do
            ;
          while (++indexOfId2 < rows.Count && rows[indexOfId2].depth > depth);
          if (indexOfId2 < rows.Count)
            rows.Insert(indexOfId2, this.m_FakeItem);
          else
            rows.Add(this.m_FakeItem);
        }
        else if (rows.Count > 0)
          rows.Insert(0, this.m_FakeItem);
        else
          rows.Add(this.m_FakeItem);
        this.m_NeedRefreshVisibleFolders = false;
        this.m_TreeView.Frame(this.m_FakeItem.id, true, false);
        this.m_TreeView.Repaint();
      }
      else
        Debug.LogError((object) "No parent Item found");
    }

    internal class SemiNumericDisplayNameListComparer : IComparer<TreeViewItem>
    {
      public int Compare(TreeViewItem x, TreeViewItem y)
      {
        if (x == y)
          return 0;
        if (x == null)
          return -1;
        if (y == null)
          return 1;
        return EditorUtility.NaturalCompare(x.displayName, y.displayName);
      }
    }

    private class FolderTreeItem : TreeViewItem
    {
      public FolderTreeItem(int id, int depth, TreeViewItem parent, string displayName)
        : base(id, depth, parent, displayName)
      {
      }
    }

    private class NonFolderTreeItem : TreeViewItem
    {
      public NonFolderTreeItem(int id, int depth, TreeViewItem parent, string displayName)
        : base(id, depth, parent, displayName)
      {
      }
    }
  }
}
