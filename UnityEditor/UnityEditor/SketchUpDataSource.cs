// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SketchUpDataSource : TreeViewDataSource
  {
    private const int k_ProgressUpdateStep = 50;
    private SketchUpNodeInfo[] m_Nodes;

    public SketchUpDataSource(TreeView treeView, SketchUpNodeInfo[] nodes)
      : base(treeView)
    {
      this.m_Nodes = nodes;
      this.FetchData();
    }

    public int[] FetchEnableNodes()
    {
      List<int> enable = new List<int>();
      this.InternalFetchEnableNodes(this.m_RootItem as SketchUpNode, enable);
      return enable.ToArray();
    }

    private void InternalFetchEnableNodes(SketchUpNode node, List<int> enable)
    {
      if (node.Enabled)
        enable.Add(node.Info.nodeIndex);
      using (List<TreeViewItem>.Enumerator enumerator = node.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.InternalFetchEnableNodes(enumerator.Current as SketchUpNode, enable);
      }
    }

    public override void FetchData()
    {
      this.m_RootItem = (TreeViewItem) new SketchUpNode(this.m_Nodes[0].nodeIndex, 0, (TreeViewItem) null, this.m_Nodes[0].name, this.m_Nodes[0]);
      List<SketchUpNode> sketchUpNodeList = new List<SketchUpNode>();
      sketchUpNodeList.Add(this.m_RootItem as SketchUpNode);
      this.SetExpanded(this.m_RootItem, true);
      int nodeIndex = this.m_Nodes[0].nodeIndex;
      for (int index = 1; index < this.m_Nodes.Length; ++index)
      {
        SketchUpNodeInfo node = this.m_Nodes[index];
        if (node.parent >= 0 && node.parent <= sketchUpNodeList.Count)
        {
          if (node.parent >= index)
            Debug.LogError((object) "Parent node index is greater than child node");
          else if (nodeIndex >= node.nodeIndex)
          {
            Debug.LogError((object) "Node array is not sorted by nodeIndex");
          }
          else
          {
            SketchUpNode sketchUpNode1 = sketchUpNodeList[node.parent];
            SketchUpNode sketchUpNode2 = new SketchUpNode(node.nodeIndex, sketchUpNode1.depth + 1, (TreeViewItem) sketchUpNode1, node.name, node);
            sketchUpNode1.children.Add((TreeViewItem) sketchUpNode2);
            this.SetExpanded((TreeViewItem) sketchUpNode2, sketchUpNode2.Info.enabled);
            sketchUpNodeList.Add(sketchUpNode2);
            if (index % 50 == 0)
              EditorUtility.DisplayProgressBar("SketchUp Import", "Building Node Selection", (float) index / (float) this.m_Nodes.Length);
          }
        }
      }
      EditorUtility.ClearProgressBar();
      this.m_NeedRefreshVisibleFolders = true;
    }

    public override bool CanBeParent(TreeViewItem item)
    {
      return item.hasChildren;
    }

    public override bool IsRenamingItemAllowed(TreeViewItem item)
    {
      return false;
    }
  }
}
