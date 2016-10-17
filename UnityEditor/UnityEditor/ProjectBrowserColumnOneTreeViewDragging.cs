// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectBrowserColumnOneTreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnityEditor
{
  internal class ProjectBrowserColumnOneTreeViewDragging : AssetsTreeViewDragging
  {
    public ProjectBrowserColumnOneTreeViewDragging(TreeView treeView)
      : base(treeView)
    {
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      if (SavedSearchFilters.IsSavedFilter(draggedItem.id) && draggedItem.id == SavedSearchFilters.GetRootInstanceID())
        return;
      ProjectWindowUtil.StartDrag(draggedItem.id, draggedItemIDs);
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      if (targetItem == null)
        return DragAndDropVisualMode.None;
      object genericData = DragAndDrop.GetGenericData(ProjectWindowUtil.k_DraggingFavoriteGenericData);
      if (genericData != null)
      {
        int instanceID = (int) genericData;
        if (!(targetItem is SearchFilterTreeItem) || !(parentItem is SearchFilterTreeItem))
          return DragAndDropVisualMode.None;
        bool flag = SavedSearchFilters.CanMoveSavedFilter(instanceID, parentItem.id, targetItem.id, true);
        if (flag && perform)
          SavedSearchFilters.MoveSavedFilter(instanceID, parentItem.id, targetItem.id, true);
        return flag ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.None;
      }
      if (!(targetItem is SearchFilterTreeItem) || !(parentItem is SearchFilterTreeItem))
        return base.DoDrag(parentItem, targetItem, perform, dropPos);
      if (!(DragAndDrop.GetGenericData(ProjectWindowUtil.k_IsFolderGenericData) as string == "isFolder"))
        return DragAndDropVisualMode.None;
      if (perform)
      {
        Object[] objectReferences = DragAndDrop.objectReferences;
        if (objectReferences.Length > 0)
        {
          string assetPath = AssetDatabase.GetAssetPath(objectReferences[0].GetInstanceID());
          if (!string.IsNullOrEmpty(assetPath))
          {
            string name = new DirectoryInfo(assetPath).Name;
            SearchFilter filter = new SearchFilter();
            filter.folders = new string[1]{ assetPath };
            bool addAsChild = targetItem == parentItem;
            float listAreaGridSize = ProjectBrowserColumnOneTreeViewGUI.GetListAreaGridSize();
            Selection.activeInstanceID = SavedSearchFilters.AddSavedFilterAfterInstanceID(name, filter, listAreaGridSize, targetItem.id, addAsChild);
          }
          else
            Debug.Log((object) ("Could not get asset path from id " + (object) objectReferences[0].GetInstanceID()));
        }
      }
      return DragAndDropVisualMode.Copy;
    }
  }
}
