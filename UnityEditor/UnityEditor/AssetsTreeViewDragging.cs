// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetsTreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;

namespace UnityEditor
{
  internal class AssetsTreeViewDragging : TreeViewDragging
  {
    public AssetsTreeViewDragging(TreeView treeView)
      : base(treeView)
    {
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      DragAndDrop.PrepareStartDrag();
      DragAndDrop.objectReferences = ProjectWindowUtil.GetDragAndDropObjects(draggedItem.id, draggedItemIDs);
      DragAndDrop.paths = ProjectWindowUtil.GetDragAndDropPaths(draggedItem.id, draggedItemIDs);
      if (DragAndDrop.objectReferences.Length > 1)
        DragAndDrop.StartDrag("<Multiple>");
      else
        DragAndDrop.StartDrag(ObjectNames.GetDragAndDropTitle(InternalEditorUtility.GetObjectFromInstanceID(draggedItem.id)));
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      HierarchyProperty property = new HierarchyProperty(HierarchyType.Assets);
      if (parentItem == null || !property.Find(parentItem.id, (int[]) null))
        property = (HierarchyProperty) null;
      return InternalEditorUtility.ProjectWindowDrag(property, perform);
    }
  }
}
