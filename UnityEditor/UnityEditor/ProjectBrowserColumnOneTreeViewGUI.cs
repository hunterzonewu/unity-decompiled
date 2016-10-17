// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectBrowserColumnOneTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ProjectBrowserColumnOneTreeViewGUI : AssetsTreeViewGUI
  {
    private Texture2D k_FavoritesIcon = EditorGUIUtility.FindTexture("Favorite Icon");
    private Texture2D k_FavoriteFolderIcon = EditorGUIUtility.FindTexture("FolderFavorite Icon");
    private Texture2D k_FavoriteFilterIcon = EditorGUIUtility.FindTexture("Search Icon");
    private const float k_DistBetweenRootTypes = 15f;
    private bool m_IsCreatingSavedFilter;

    public ProjectBrowserColumnOneTreeViewGUI(TreeView treeView)
      : base(treeView)
    {
    }

    public override Vector2 GetTotalSize()
    {
      Vector2 totalSize = base.GetTotalSize();
      totalSize.y += 15f;
      return totalSize;
    }

    public override Rect GetRowRect(int row, float rowWidth)
    {
      List<TreeViewItem> rows = this.m_TreeView.data.GetRows();
      return new Rect(0.0f, this.GetTopPixelOfRow(row, rows), rowWidth, this.k_LineHeight);
    }

    private float GetTopPixelOfRow(int row, List<TreeViewItem> rows)
    {
      float num = (float) row * this.k_LineHeight;
      if (ProjectBrowser.GetItemType(rows[row].id) == ProjectBrowser.ItemType.Asset)
        num += 15f;
      return num;
    }

    public override int GetNumRowsOnPageUpDown(TreeViewItem fromItem, bool pageUp, float heightOfTreeView)
    {
      return (int) Mathf.Floor(heightOfTreeView / this.k_LineHeight) - 1;
    }

    public override void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible)
    {
      float y = this.m_TreeView.state.scrollPos.y;
      float height = this.m_TreeView.GetTotalRect().height;
      firstRowVisible = (int) Mathf.Floor(y / this.k_LineHeight);
      lastRowVisible = firstRowVisible + (int) Mathf.Ceil(height / this.k_LineHeight);
      float num = 15f / this.k_LineHeight;
      firstRowVisible = firstRowVisible - (int) Mathf.Ceil(2f * num);
      lastRowVisible = lastRowVisible + (int) Mathf.Ceil(2f * num);
      firstRowVisible = Mathf.Max(firstRowVisible, 0);
      lastRowVisible = Mathf.Min(lastRowVisible, this.m_TreeView.data.rowCount - 1);
    }

    public override void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused)
    {
      bool useBoldFont = this.IsVisibleRootNode(item);
      this.DoNodeGUI(rowRect, row, item, selected, focused, useBoldFont);
    }

    private bool IsVisibleRootNode(TreeViewItem item)
    {
      return (this.m_TreeView.data as ProjectBrowserColumnOneTreeViewDataSource).IsVisibleRootNode(item);
    }

    protected override Texture GetIconForNode(TreeViewItem item)
    {
      if (item != null && (Object) item.icon != (Object) null)
        return (Texture) item.icon;
      SearchFilterTreeItem searchFilterTreeItem = item as SearchFilterTreeItem;
      if (searchFilterTreeItem == null)
        return base.GetIconForNode(item);
      if (this.IsVisibleRootNode(item))
        return (Texture) this.k_FavoritesIcon;
      if (searchFilterTreeItem.isFolder)
        return (Texture) this.k_FavoriteFolderIcon;
      return (Texture) this.k_FavoriteFilterIcon;
    }

    public static float GetListAreaGridSize()
    {
      float num = -1f;
      if ((Object) ProjectBrowser.s_LastInteractedProjectBrowser != (Object) null)
        num = ProjectBrowser.s_LastInteractedProjectBrowser.listAreaGridSize;
      return num;
    }

    internal virtual void BeginCreateSavedFilter(SearchFilter filter)
    {
      string str = "New Saved Search";
      this.m_IsCreatingSavedFilter = true;
      int num = SavedSearchFilters.AddSavedFilter(str, filter, ProjectBrowserColumnOneTreeViewGUI.GetListAreaGridSize());
      this.m_TreeView.Frame(num, true, false);
      this.m_TreeView.state.renameOverlay.BeginRename(str, num, 0.0f);
    }

    protected override void RenameEnded()
    {
      int userData = this.GetRenameOverlay().userData;
      ProjectBrowser.ItemType itemType = ProjectBrowser.GetItemType(userData);
      if (this.m_IsCreatingSavedFilter)
      {
        this.m_IsCreatingSavedFilter = false;
        if (this.GetRenameOverlay().userAcceptedRename)
        {
          SavedSearchFilters.SetName(userData, this.GetRenameOverlay().name);
          this.m_TreeView.SetSelection(new int[1]{ userData }, 1 != 0);
        }
        else
          SavedSearchFilters.RemoveSavedFilter(userData);
      }
      else if (itemType == ProjectBrowser.ItemType.SavedFilter)
      {
        if (!this.GetRenameOverlay().userAcceptedRename)
          return;
        SavedSearchFilters.SetName(userData, this.GetRenameOverlay().name);
      }
      else
      {
        base.RenameEnded();
        if (!this.GetRenameOverlay().userAcceptedRename)
          return;
        this.m_TreeView.NotifyListenersThatSelectionChanged();
      }
    }
  }
}
