// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioGroupTreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Audio;

namespace UnityEditor
{
  internal class AudioGroupTreeViewDragging : AssetsTreeViewDragging
  {
    private AudioMixerGroupTreeView m_owner;

    public AudioGroupTreeViewDragging(TreeView treeView, AudioMixerGroupTreeView owner)
      : base(treeView)
    {
      this.m_owner = owner;
    }

    public override void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs)
    {
      if (EditorApplication.isPlaying)
        return;
      base.StartDrag(draggedItem, draggedItemIDs);
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentNode, TreeViewItem targetNode, bool perform, TreeViewDragging.DropPosition dragPos)
    {
      AudioMixerTreeViewNode mixerTreeViewNode1 = targetNode as AudioMixerTreeViewNode;
      AudioMixerTreeViewNode mixerTreeViewNode2 = parentNode as AudioMixerTreeViewNode;
      List<AudioMixerGroupController> list1 = new List<UnityEngine.Object>((IEnumerable<UnityEngine.Object>) DragAndDrop.objectReferences).OfType<AudioMixerGroupController>().ToList<AudioMixerGroupController>();
      if (mixerTreeViewNode2 == null || list1.Count <= 0)
        return DragAndDropVisualMode.None;
      List<int> list2 = list1.Select<AudioMixerGroupController, int>((Func<AudioMixerGroupController, int>) (i => i.GetInstanceID())).ToList<int>();
      bool flag = this.ValidDrag(parentNode, list2) && !AudioMixerController.WillModificationOfTopologyCauseFeedback(this.m_owner.Controller.GetAllAudioGroupsSlow(), list1, mixerTreeViewNode2.group, (List<AudioMixerController.ConnectionNode>) null);
      if (perform && flag)
      {
        this.m_owner.Controller.ReparentSelection(mixerTreeViewNode2.group, mixerTreeViewNode1.group, list1);
        this.m_owner.ReloadTree();
        this.m_TreeView.SetSelection(list2.ToArray(), true);
      }
      return flag ? DragAndDropVisualMode.Move : DragAndDropVisualMode.Rejected;
    }

    private bool ValidDrag(TreeViewItem parent, List<int> draggedInstanceIDs)
    {
      for (TreeViewItem treeViewItem = parent; treeViewItem != null; treeViewItem = treeViewItem.parent)
      {
        if (draggedInstanceIDs.Contains(treeViewItem.id))
          return false;
      }
      return true;
    }
  }
}
