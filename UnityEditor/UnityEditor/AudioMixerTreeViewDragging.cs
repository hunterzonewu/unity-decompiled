// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerTreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixerTreeViewDragging : TreeViewDragging
  {
    private const string k_AudioMixerDraggingID = "AudioMixerDragging";
    private System.Action<List<AudioMixerController>, AudioMixerController> m_MixersDroppedOnMixerCallback;

    public AudioMixerTreeViewDragging(TreeView treeView, System.Action<List<AudioMixerController>, AudioMixerController> mixerDroppedOnMixerCallback)
      : base(treeView)
    {
      this.m_MixersDroppedOnMixerCallback = mixerDroppedOnMixerCallback;
    }

    public override void StartDrag(TreeViewItem draggedNode, List<int> draggedNodes)
    {
      if (EditorApplication.isPlaying)
        return;
      List<AudioMixerItem> mixerItemsFromIds = this.GetAudioMixerItemsFromIDs(draggedNodes);
      DragAndDrop.PrepareStartDrag();
      DragAndDrop.SetGenericData("AudioMixerDragging", (object) new AudioMixerTreeViewDragging.DragData(mixerItemsFromIds));
      DragAndDrop.objectReferences = new UnityEngine.Object[0];
      DragAndDrop.StartDrag(draggedNodes.Count.ToString() + " AudioMixer" + (draggedNodes.Count <= 1 ? (object) string.Empty : (object) "s"));
    }

    public override bool DragElement(TreeViewItem targetItem, Rect targetItemRect, bool firstItem)
    {
      AudioMixerTreeViewDragging.DragData genericData = DragAndDrop.GetGenericData("AudioMixerDragging") as AudioMixerTreeViewDragging.DragData;
      if (genericData == null)
      {
        DragAndDrop.visualMode = DragAndDropVisualMode.None;
        return false;
      }
      if (targetItem != null || !this.m_TreeView.GetTotalRect().Contains(Event.current.mousePosition))
        return base.DragElement(targetItem, targetItemRect, firstItem);
      if (this.m_DropData != null)
      {
        this.m_DropData.dropTargetControlID = 0;
        this.m_DropData.rowMarkerControlID = 0;
      }
      if (Event.current.type == EventType.DragPerform)
      {
        DragAndDrop.AcceptDrag();
        if (this.m_MixersDroppedOnMixerCallback != null)
          this.m_MixersDroppedOnMixerCallback(this.GetAudioMixersFromItems(genericData.m_DraggedItems), (AudioMixerController) null);
      }
      DragAndDrop.visualMode = DragAndDropVisualMode.Move;
      Event.current.Use();
      return false;
    }

    public override DragAndDropVisualMode DoDrag(TreeViewItem parentNode, TreeViewItem targetNode, bool perform, TreeViewDragging.DropPosition dragPos)
    {
      AudioMixerTreeViewDragging.DragData genericData = DragAndDrop.GetGenericData("AudioMixerDragging") as AudioMixerTreeViewDragging.DragData;
      if (genericData == null)
        return DragAndDropVisualMode.None;
      List<AudioMixerItem> draggedItems = genericData.m_DraggedItems;
      AudioMixerItem audioMixerItem = parentNode as AudioMixerItem;
      if (audioMixerItem == null || genericData == null)
        return DragAndDropVisualMode.None;
      List<AudioMixerGroupController> list = draggedItems.Select<AudioMixerItem, AudioMixerGroupController>((Func<AudioMixerItem, AudioMixerGroupController>) (i => i.mixer.masterGroup)).ToList<AudioMixerGroupController>();
      bool flag1 = AudioMixerController.WillModificationOfTopologyCauseFeedback(audioMixerItem.mixer.GetAllAudioGroupsSlow(), list, audioMixerItem.mixer.masterGroup, (List<AudioMixerController.ConnectionNode>) null);
      bool flag2 = this.ValidDrag(parentNode, draggedItems) && !flag1;
      if (perform && flag2 && this.m_MixersDroppedOnMixerCallback != null)
        this.m_MixersDroppedOnMixerCallback(this.GetAudioMixersFromItems(draggedItems), audioMixerItem.mixer);
      return flag2 ? DragAndDropVisualMode.Move : DragAndDropVisualMode.Rejected;
    }

    private bool ValidDrag(TreeViewItem parent, List<AudioMixerItem> draggedItems)
    {
      List<int> list = draggedItems.Select<AudioMixerItem, int>((Func<AudioMixerItem, int>) (n => n.id)).ToList<int>();
      for (TreeViewItem treeViewItem = parent; treeViewItem != null; treeViewItem = treeViewItem.parent)
      {
        if (list.Contains(treeViewItem.id))
          return false;
      }
      return true;
    }

    private List<AudioMixerItem> GetAudioMixerItemsFromIDs(List<int> draggedMixers)
    {
      return TreeViewUtility.FindItemsInList((IEnumerable<int>) draggedMixers, this.m_TreeView.data.GetRows()).OfType<AudioMixerItem>().ToList<AudioMixerItem>();
    }

    private List<AudioMixerController> GetAudioMixersFromItems(List<AudioMixerItem> draggedItems)
    {
      return draggedItems.Select<AudioMixerItem, AudioMixerController>((Func<AudioMixerItem, AudioMixerController>) (i => i.mixer)).ToList<AudioMixerController>();
    }

    private class DragData
    {
      public List<AudioMixerItem> m_DraggedItems;

      public DragData(List<AudioMixerItem> draggedItems)
      {
        this.m_DraggedItems = draggedItems;
      }
    }
  }
}
