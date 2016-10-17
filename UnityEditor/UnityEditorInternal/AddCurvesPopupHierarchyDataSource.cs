// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopupHierarchyDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AddCurvesPopupHierarchyDataSource : TreeViewDataSource
  {
    private AnimationWindowState state { get; set; }

    public static bool showEntireHierarchy { get; set; }

    public AddCurvesPopupHierarchyDataSource(TreeView treeView, AnimationWindowState animationWindowState)
      : base(treeView)
    {
      this.showRootNode = false;
      this.rootIsCollapsable = false;
      this.state = animationWindowState;
    }

    private void SetupRootNodeSettings()
    {
      this.showRootNode = false;
      this.SetExpanded(this.root, true);
    }

    public override void FetchData()
    {
      if ((Object) AddCurvesPopup.gameObject == (Object) null)
        return;
      this.AddGameObjectToHierarchy(AddCurvesPopup.gameObject, (TreeViewItem) null);
      this.SetupRootNodeSettings();
      this.m_NeedRefreshVisibleFolders = true;
    }

    private TreeViewItem AddGameObjectToHierarchy(GameObject gameObject, TreeViewItem parent)
    {
      string transformPath = AnimationUtility.CalculateTransformPath(gameObject.transform, this.state.activeRootGameObject.transform);
      TreeViewItem treeViewItem = (TreeViewItem) new AddCurvesPopupGameObjectNode(gameObject, parent, gameObject.name);
      List<TreeViewItem> visibleItems = new List<TreeViewItem>();
      if (parent == null)
        this.m_RootItem = treeViewItem;
      EditorCurveBinding[] animatableBindings = AnimationUtility.GetAnimatableBindings(gameObject, this.state.activeRootGameObject);
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      for (int index = 0; index < animatableBindings.Length; ++index)
      {
        EditorCurveBinding binding = animatableBindings[index];
        editorCurveBindingList.Add(binding);
        if (binding.propertyName == "m_IsActive")
        {
          if (binding.path != string.Empty)
          {
            TreeViewItem node = this.CreateNode(editorCurveBindingList.ToArray(), treeViewItem);
            if (node != null)
              visibleItems.Add(node);
            editorCurveBindingList.Clear();
          }
          else
            editorCurveBindingList.Clear();
        }
        else
        {
          bool flag1 = index == animatableBindings.Length - 1;
          bool flag2 = false;
          if (!flag1)
            flag2 = animatableBindings[index + 1].type != binding.type;
          if (AnimationWindowUtility.IsCurveCreated(this.state.activeAnimationClip, binding))
            editorCurveBindingList.Remove(binding);
          if ((flag1 || flag2) && editorCurveBindingList.Count > 0)
          {
            visibleItems.Add(this.AddAnimatableObjectToHierarchy(this.state.activeRootGameObject, editorCurveBindingList.ToArray(), treeViewItem, transformPath));
            editorCurveBindingList.Clear();
          }
        }
      }
      if (AddCurvesPopupHierarchyDataSource.showEntireHierarchy)
      {
        for (int index = 0; index < gameObject.transform.childCount; ++index)
        {
          TreeViewItem hierarchy = this.AddGameObjectToHierarchy(gameObject.transform.GetChild(index).gameObject, treeViewItem);
          if (hierarchy != null)
            visibleItems.Add(hierarchy);
        }
      }
      TreeViewUtility.SetChildParentReferences(visibleItems, treeViewItem);
      return treeViewItem;
    }

    private static string GetClassName(GameObject root, EditorCurveBinding binding)
    {
      Object animatedObject = AnimationUtility.GetAnimatedObject(root, binding);
      if ((bool) animatedObject)
        return ObjectNames.GetInspectorTitle(animatedObject);
      return binding.type.Name;
    }

    private TreeViewItem AddAnimatableObjectToHierarchy(GameObject root, EditorCurveBinding[] curveBindings, TreeViewItem parentNode, string path)
    {
      TreeViewItem treeViewItem = (TreeViewItem) new AddCurvesPopupObjectNode(parentNode, path, AddCurvesPopupHierarchyDataSource.GetClassName(root, curveBindings[0]));
      treeViewItem.icon = AssetPreview.GetMiniThumbnail(AnimationUtility.GetAnimatedObject(root, curveBindings[0]));
      List<TreeViewItem> visibleItems = new List<TreeViewItem>();
      List<EditorCurveBinding> editorCurveBindingList = new List<EditorCurveBinding>();
      for (int index = 0; index < curveBindings.Length; ++index)
      {
        EditorCurveBinding curveBinding = curveBindings[index];
        editorCurveBindingList.Add(curveBinding);
        if (index == curveBindings.Length - 1 || AnimationWindowUtility.GetPropertyGroupName(curveBindings[index + 1].propertyName) != AnimationWindowUtility.GetPropertyGroupName(curveBinding.propertyName))
        {
          TreeViewItem node = this.CreateNode(editorCurveBindingList.ToArray(), treeViewItem);
          if (node != null)
            visibleItems.Add(node);
          editorCurveBindingList.Clear();
        }
      }
      visibleItems.Sort();
      TreeViewUtility.SetChildParentReferences(visibleItems, treeViewItem);
      return treeViewItem;
    }

    private TreeViewItem CreateNode(EditorCurveBinding[] curveBindings, TreeViewItem parentNode)
    {
      AddCurvesPopupPropertyNode popupPropertyNode = new AddCurvesPopupPropertyNode(parentNode, curveBindings);
      if (AnimationWindowUtility.IsRectTransformPosition(popupPropertyNode.curveBindings[0]))
        popupPropertyNode.curveBindings = new EditorCurveBinding[1]
        {
          popupPropertyNode.curveBindings[2]
        };
      popupPropertyNode.icon = parentNode.icon;
      return (TreeViewItem) popupPropertyNode;
    }

    public void UpdateData()
    {
      this.m_TreeView.ReloadData();
    }
  }
}
