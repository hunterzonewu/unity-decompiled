// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectBrowserColumnOneTreeViewDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class ProjectBrowserColumnOneTreeViewDataSource : TreeViewDataSource
  {
    private static string kProjectBrowserString = "ProjectBrowser";

    public ProjectBrowserColumnOneTreeViewDataSource(TreeView treeView)
      : base(treeView)
    {
      this.showRootNode = false;
      this.rootIsCollapsable = false;
      SavedSearchFilters.AddChangeListener(new System.Action(((TreeViewDataSource) this).ReloadData));
    }

    public override bool SetExpanded(int id, bool expand)
    {
      if (!base.SetExpanded(id, expand))
        return false;
      InternalEditorUtility.expandedProjectWindowItems = this.expandedIDs.ToArray();
      if (this.m_RootItem.hasChildren)
      {
        using (List<TreeViewItem>.Enumerator enumerator = this.m_RootItem.children.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            TreeViewItem current = enumerator.Current;
            if (current.id == id)
              EditorPrefs.SetBool(ProjectBrowserColumnOneTreeViewDataSource.kProjectBrowserString + current.displayName, expand);
          }
        }
      }
      return true;
    }

    public override bool IsExpandable(TreeViewItem item)
    {
      if (!item.hasChildren)
        return false;
      if (item == this.m_RootItem)
        return this.rootIsCollapsable;
      return true;
    }

    public override bool CanBeMultiSelected(TreeViewItem item)
    {
      return ProjectBrowser.GetItemType(item.id) != ProjectBrowser.ItemType.SavedFilter;
    }

    public override bool CanBeParent(TreeViewItem item)
    {
      if (item is SearchFilterTreeItem)
        return SavedSearchFilters.AllowsHierarchy();
      return true;
    }

    public bool IsVisibleRootNode(TreeViewItem item)
    {
      if (item.parent != null)
        return item.parent.parent == null;
      return false;
    }

    public override bool IsRenamingItemAllowed(TreeViewItem item)
    {
      if (this.IsVisibleRootNode(item))
        return false;
      return base.IsRenamingItemAllowed(item);
    }

    public static int GetAssetsFolderInstanceID()
    {
      return AssetDatabase.GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID("Assets"));
    }

    public override void FetchData()
    {
      this.m_RootItem = new TreeViewItem(int.MaxValue, 0, (TreeViewItem) null, "Invisible Root Item");
      this.SetExpanded(this.m_RootItem, true);
      List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
      int folderInstanceId = ProjectBrowserColumnOneTreeViewDataSource.GetAssetsFolderInstanceID();
      int depth = 0;
      string displayName = "Assets";
      TreeViewItem parent = new TreeViewItem(folderInstanceId, depth, this.m_RootItem, displayName);
      this.ReadAssetDatabase(parent, depth + 1);
      TreeViewItem treeView = SavedSearchFilters.ConvertToTreeView();
      treeView.parent = this.m_RootItem;
      treeViewItemList.Add(treeView);
      treeViewItemList.Add(parent);
      this.m_RootItem.children = treeViewItemList;
      using (List<TreeViewItem>.Enumerator enumerator = this.m_RootItem.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TreeViewItem current = enumerator.Current;
          bool expand = EditorPrefs.GetBool(ProjectBrowserColumnOneTreeViewDataSource.kProjectBrowserString + current.displayName, true);
          this.SetExpanded(current, expand);
        }
      }
      this.m_NeedRefreshVisibleFolders = true;
    }

    private void ReadAssetDatabase(TreeViewItem parent, int baseDepth)
    {
      IHierarchyProperty hierarchyProperty = (IHierarchyProperty) new HierarchyProperty(HierarchyType.Assets);
      hierarchyProperty.Reset();
      Texture2D texture1 = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
      Texture2D texture2 = EditorGUIUtility.FindTexture(EditorResourcesUtility.emptyFolderIconName);
      List<TreeViewItem> visibleItems = new List<TreeViewItem>();
      while (hierarchyProperty.Next((int[]) null))
      {
        if (hierarchyProperty.isFolder)
          visibleItems.Add(new TreeViewItem(hierarchyProperty.instanceID, baseDepth + hierarchyProperty.depth, (TreeViewItem) null, hierarchyProperty.name)
          {
            icon = !hierarchyProperty.hasChildren ? texture2 : texture1
          });
      }
      TreeViewUtility.SetChildParentReferences(visibleItems, parent);
    }
  }
}
