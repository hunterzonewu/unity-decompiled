// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyDataSource : TreeViewDataSource
  {
    private AnimationWindowState state { get; set; }

    public bool showAll { get; set; }

    public AnimationWindowHierarchyDataSource(TreeView treeView, AnimationWindowState animationWindowState)
      : base(treeView)
    {
      this.state = animationWindowState;
    }

    private void SetupRootNodeSettings()
    {
      this.showRootNode = false;
      this.rootIsCollapsable = false;
      this.SetExpanded(this.m_RootItem, true);
    }

    private AnimationWindowHierarchyNode GetEmptyRootNode()
    {
      return new AnimationWindowHierarchyNode(0, -1, (TreeViewItem) null, (System.Type) null, string.Empty, string.Empty, "root");
    }

    public override void FetchData()
    {
      this.m_RootItem = (TreeViewItem) this.GetEmptyRootNode();
      this.SetupRootNodeSettings();
      this.m_NeedRefreshVisibleFolders = true;
      if ((UnityEngine.Object) this.state.activeRootGameObject == (UnityEngine.Object) null && (UnityEngine.Object) this.state.activeAnimationClip == (UnityEngine.Object) null)
        return;
      List<AnimationWindowHierarchyNode> windowHierarchyNodeList = new List<AnimationWindowHierarchyNode>();
      if (this.state.allCurves.Count > 0)
        windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) new AnimationWindowHierarchyMasterNode());
      windowHierarchyNodeList.AddRange((IEnumerable<AnimationWindowHierarchyNode>) this.CreateTreeFromCurves());
      windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) new AnimationWindowHierarchyAddButtonNode());
      TreeViewUtility.SetChildParentReferences(new List<TreeViewItem>((IEnumerable<TreeViewItem>) windowHierarchyNodeList.ToArray()), this.root);
    }

    public override bool IsRenamingItemAllowed(TreeViewItem item)
    {
      return !(item is AnimationWindowHierarchyAddButtonNode) && !(item is AnimationWindowHierarchyMasterNode) && (item as AnimationWindowHierarchyNode).path.Length != 0;
    }

    public List<AnimationWindowHierarchyNode> CreateTreeFromCurves()
    {
      List<AnimationWindowHierarchyNode> windowHierarchyNodeList = new List<AnimationWindowHierarchyNode>();
      List<AnimationWindowCurve> animationWindowCurveList = new List<AnimationWindowCurve>();
      AnimationWindowCurve[] array = this.state.allCurves.ToArray();
      for (int index = 0; index < array.Length; ++index)
      {
        AnimationWindowCurve animationWindowCurve1 = array[index];
        AnimationWindowCurve animationWindowCurve2 = index >= array.Length - 1 ? (AnimationWindowCurve) null : array[index + 1];
        animationWindowCurveList.Add(animationWindowCurve1);
        bool flag1 = animationWindowCurve2 != null && AnimationWindowUtility.GetPropertyGroupName(animationWindowCurve2.propertyName) == AnimationWindowUtility.GetPropertyGroupName(animationWindowCurve1.propertyName);
        bool flag2 = animationWindowCurve2 != null && animationWindowCurve1.path.Equals(animationWindowCurve2.path) && animationWindowCurve1.type == animationWindowCurve2.type;
        if (index == array.Length - 1 || !flag1 || !flag2)
        {
          if (animationWindowCurveList.Count > 1)
            windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) this.AddPropertyGroupToHierarchy(animationWindowCurveList.ToArray(), (AnimationWindowHierarchyNode) this.m_RootItem));
          else
            windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) this.AddPropertyToHierarchy(animationWindowCurveList[0], (AnimationWindowHierarchyNode) this.m_RootItem));
          animationWindowCurveList.Clear();
        }
      }
      return windowHierarchyNodeList;
    }

    private AnimationWindowHierarchyPropertyGroupNode AddPropertyGroupToHierarchy(AnimationWindowCurve[] curves, AnimationWindowHierarchyNode parentNode)
    {
      List<AnimationWindowHierarchyNode> windowHierarchyNodeList = new List<AnimationWindowHierarchyNode>();
      AnimationWindowHierarchyPropertyGroupNode propertyGroupNode = new AnimationWindowHierarchyPropertyGroupNode(curves[0].type, AnimationWindowUtility.GetPropertyGroupName(curves[0].propertyName), curves[0].path, (TreeViewItem) parentNode);
      propertyGroupNode.icon = this.GetIcon(curves[0].binding);
      propertyGroupNode.indent = curves[0].depth;
      propertyGroupNode.curves = curves;
      foreach (AnimationWindowCurve curve in curves)
      {
        AnimationWindowHierarchyPropertyNode hierarchy = this.AddPropertyToHierarchy(curve, (AnimationWindowHierarchyNode) propertyGroupNode);
        hierarchy.displayName = AnimationWindowUtility.GetPropertyDisplayName(hierarchy.propertyName);
        windowHierarchyNodeList.Add((AnimationWindowHierarchyNode) hierarchy);
      }
      TreeViewUtility.SetChildParentReferences(new List<TreeViewItem>((IEnumerable<TreeViewItem>) windowHierarchyNodeList.ToArray()), (TreeViewItem) propertyGroupNode);
      return propertyGroupNode;
    }

    private AnimationWindowHierarchyPropertyNode AddPropertyToHierarchy(AnimationWindowCurve curve, AnimationWindowHierarchyNode parentNode)
    {
      AnimationWindowHierarchyPropertyNode hierarchyPropertyNode = new AnimationWindowHierarchyPropertyNode(curve.type, curve.propertyName, curve.path, (TreeViewItem) parentNode, curve.binding, curve.isPPtrCurve);
      if ((UnityEngine.Object) parentNode.icon != (UnityEngine.Object) null)
        hierarchyPropertyNode.icon = parentNode.icon;
      else
        hierarchyPropertyNode.icon = this.GetIcon(curve.binding);
      hierarchyPropertyNode.indent = curve.depth;
      hierarchyPropertyNode.curves = new AnimationWindowCurve[1]
      {
        curve
      };
      return hierarchyPropertyNode;
    }

    public Texture2D GetIcon(EditorCurveBinding curveBinding)
    {
      if ((UnityEngine.Object) this.state.activeRootGameObject != (UnityEngine.Object) null && (object) AnimationUtility.GetAnimatedObject(this.state.activeRootGameObject, curveBinding) != null)
        return AssetPreview.GetMiniThumbnail(AnimationUtility.GetAnimatedObject(this.state.activeRootGameObject, curveBinding));
      return AssetPreview.GetMiniTypeThumbnail(curveBinding.type);
    }

    public void UpdateData()
    {
      this.m_TreeView.ReloadData();
    }
  }
}
