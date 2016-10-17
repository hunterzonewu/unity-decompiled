// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameObjectsTreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class GameObjectsTreeViewDragging : TreeViewDragging
  {
    private const string kSceneHeaderDragString = "SceneHeaderList";

    public bool allowDragBetween { get; set; }

    public GameObjectsTreeViewDragging(TreeView treeView)
      : base(treeView)
    {
      this.allowDragBetween = false;
    }

    public override bool CanStartDrag(TreeViewItem targetItem, List<int> draggedItemIDs, Vector2 mouseDownPosition)
    {
      return string.IsNullOrEmpty(((GameObjectTreeViewDataSource) this.m_TreeView.data).searchString);
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      DragAndDrop.PrepareStartDrag();
      draggedItemIDs = this.m_TreeView.SortIDsInVisiblityOrder(draggedItemIDs);
      if (!draggedItemIDs.Contains(draggedItem.id))
        draggedItemIDs = new List<int>()
        {
          draggedItem.id
        };
      UnityEngine.Object[] dragAndDropObjects = ProjectWindowUtil.GetDragAndDropObjects(draggedItem.id, draggedItemIDs);
      DragAndDrop.objectReferences = dragAndDropObjects;
      List<Scene> draggedScenes = this.GetDraggedScenes(draggedItemIDs);
      if (draggedScenes != null)
      {
        DragAndDrop.SetGenericData("SceneHeaderList", (object) draggedScenes);
        List<string> stringList = new List<string>();
        using (List<Scene>.Enumerator enumerator = draggedScenes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Scene current = enumerator.Current;
            if (current.path.Length > 0)
              stringList.Add(current.path);
          }
        }
        DragAndDrop.paths = stringList.ToArray();
      }
      else
        DragAndDrop.paths = new string[0];
      string title;
      if (draggedItemIDs.Count > 1)
        title = "<Multiple>";
      else if (dragAndDropObjects.Length == 1)
        title = ObjectNames.GetDragAndDropTitle(dragAndDropObjects[0]);
      else if (draggedScenes != null && draggedScenes.Count == 1)
      {
        title = draggedScenes[0].path;
      }
      else
      {
        title = "Unhandled dragged item";
        Debug.LogError((object) "Unhandled dragged item");
      }
      DragAndDrop.StartDrag(title);
      if (!(this.m_TreeView.data is GameObjectTreeViewDataSource))
        return;
      ((GameObjectTreeViewDataSource) this.m_TreeView.data).SetupChildParentReferencesIfNeeded();
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentItem, TreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      DragAndDropVisualMode andDropVisualMode = this.DoDragScenes(parentItem as GameObjectTreeViewItem, targetItem as GameObjectTreeViewItem, perform, dropPos);
      if (andDropVisualMode != DragAndDropVisualMode.None)
        return andDropVisualMode;
      if (parentItem == null || targetItem == null)
        return InternalEditorUtility.HierarchyWindowDrag((HierarchyProperty) null, perform, InternalEditorUtility.HierarchyDropMode.kHierarchyDropUpon);
      HierarchyProperty property = new HierarchyProperty(HierarchyType.GameObjects);
      if (this.allowDragBetween)
      {
        if (dropPos == TreeViewDragging.DropPosition.Above || !property.Find(targetItem.id, (int[]) null))
          property = (HierarchyProperty) null;
      }
      else if (dropPos == TreeViewDragging.DropPosition.Above || !property.Find(parentItem.id, (int[]) null))
        property = (HierarchyProperty) null;
      InternalEditorUtility.HierarchyDropMode dropMode = InternalEditorUtility.HierarchyDropMode.kHierarchyDragNormal;
      if (this.allowDragBetween)
        dropMode = dropPos != TreeViewDragging.DropPosition.Upon ? InternalEditorUtility.HierarchyDropMode.kHierarchyDropBetween : InternalEditorUtility.HierarchyDropMode.kHierarchyDropUpon;
      if (parentItem != null && parentItem == targetItem && dropPos != TreeViewDragging.DropPosition.Above)
        dropMode |= InternalEditorUtility.HierarchyDropMode.kHierarchyDropAfterParent;
      return InternalEditorUtility.HierarchyWindowDrag(property, perform, dropMode);
    }

    public override void DragCleanup(bool revertExpanded)
    {
      DragAndDrop.SetGenericData("SceneHeaderList", (object) null);
      base.DragCleanup(revertExpanded);
    }

    private List<Scene> GetDraggedScenes(List<int> draggedItemIDs)
    {
      List<Scene> sceneList = new List<Scene>();
      using (List<int>.Enumerator enumerator = draggedItemIDs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(enumerator.Current);
          if (!SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(sceneByHandle))
            return (List<Scene>) null;
          sceneList.Add(sceneByHandle);
        }
      }
      return sceneList;
    }

    private DragAndDropVisualMode DoDragScenes(GameObjectTreeViewItem parentItem, GameObjectTreeViewItem targetItem, bool perform, TreeViewDragging.DropPosition dropPos)
    {
      List<Scene> genericData = DragAndDrop.GetGenericData("SceneHeaderList") as List<Scene>;
      bool flag1 = genericData != null;
      bool flag2 = false;
      if (!flag1 && DragAndDrop.objectReferences.Length > 0)
      {
        int num = 0;
        foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
        {
          if (objectReference is SceneAsset)
            ++num;
        }
        flag2 = num == DragAndDrop.objectReferences.Length;
      }
      if (!flag1 && !flag2)
        return DragAndDropVisualMode.None;
      if (perform)
      {
        List<Scene> sceneList = (List<Scene>) null;
        if (flag2)
        {
          List<Scene> source = new List<Scene>();
          foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
          {
            string assetPath = AssetDatabase.GetAssetPath(objectReference);
            Scene sceneByPath = SceneManager.GetSceneByPath(assetPath);
            if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(sceneByPath))
            {
              this.m_TreeView.Frame(sceneByPath.handle, true, true);
            }
            else
            {
              Scene scene = !Event.current.alt ? EditorSceneManager.OpenScene(assetPath, OpenSceneMode.Additive) : EditorSceneManager.OpenScene(assetPath, OpenSceneMode.AdditiveWithoutLoading);
              if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(scene))
                source.Add(scene);
            }
          }
          if (targetItem != null)
            sceneList = source;
          if (SceneManager.sceneCount - source.Count == 1)
            ((TreeViewDataSource) this.m_TreeView.data).SetExpanded(SceneManager.GetSceneAt(0).handle, true);
          if (source.Count > 0)
          {
            Selection.instanceIDs = source.Select<Scene, int>((Func<Scene, int>) (x => x.handle)).ToArray<int>();
            this.m_TreeView.Frame(source.Last<Scene>().handle, true, false);
          }
        }
        else
          sceneList = genericData;
        if (sceneList != null)
        {
          if (targetItem != null)
          {
            Scene scene = targetItem.scene;
            if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(scene))
            {
              if (!targetItem.isSceneHeader || dropPos == TreeViewDragging.DropPosition.Upon)
                dropPos = TreeViewDragging.DropPosition.Below;
              if (dropPos == TreeViewDragging.DropPosition.Above)
              {
                for (int index = 0; index < sceneList.Count; ++index)
                  EditorSceneManager.MoveSceneBefore(sceneList[index], scene);
              }
              else if (dropPos == TreeViewDragging.DropPosition.Below)
              {
                for (int index = sceneList.Count - 1; index >= 0; --index)
                  EditorSceneManager.MoveSceneAfter(sceneList[index], scene);
              }
            }
          }
          else
          {
            Scene sceneAt = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            for (int index = sceneList.Count - 1; index >= 0; --index)
              EditorSceneManager.MoveSceneAfter(sceneList[index], sceneAt);
          }
        }
      }
      return DragAndDropVisualMode.Move;
    }
  }
}
