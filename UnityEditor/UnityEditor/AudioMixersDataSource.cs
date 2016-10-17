// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixersDataSource
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixersDataSource : TreeViewDataSource
  {
    private Func<List<AudioMixerController>> m_GetAllControllersCallback;

    public AudioMixersDataSource(TreeView treeView, Func<List<AudioMixerController>> getAllControllersCallback)
      : base(treeView)
    {
      this.showRootNode = false;
      this.m_GetAllControllersCallback = getAllControllersCallback;
    }

    public override void FetchData()
    {
      int depth = -1;
      bool flag = this.m_TreeView.state.expandedIDs.Count == 0;
      this.m_RootItem = new TreeViewItem(1010101010, depth, (TreeViewItem) null, "InvisibleRoot");
      this.SetExpanded(this.m_RootItem.id, true);
      List<AudioMixerController> source = this.m_GetAllControllersCallback();
      this.m_NeedRefreshVisibleFolders = true;
      if (source.Count <= 0)
        return;
      List<AudioMixerItem> list = source.Select<AudioMixerController, AudioMixerItem>((Func<AudioMixerController, AudioMixerItem>) (mixer => new AudioMixerItem(mixer.GetInstanceID(), 0, this.m_RootItem, mixer.name, mixer, AudioMixersDataSource.GetInfoText(mixer)))).ToList<AudioMixerItem>();
      using (List<AudioMixerItem>.Enumerator enumerator = list.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.SetChildParentOfMixerItem(enumerator.Current, list);
      }
      this.SetItemDepthRecursive(this.m_RootItem, -1);
      this.SortRecursive(this.m_RootItem);
      if (!flag)
        return;
      this.m_TreeView.data.SetExpandedWithChildren(this.m_RootItem, true);
    }

    private static string GetInfoText(AudioMixerController controller)
    {
      return !((UnityEngine.Object) controller.outputAudioMixerGroup != (UnityEngine.Object) null) ? "(Audio Listener)" : string.Format("({0} of {1})", (object) controller.outputAudioMixerGroup.name, (object) controller.outputAudioMixerGroup.audioMixer.name);
    }

    private void SetChildParentOfMixerItem(AudioMixerItem item, List<AudioMixerItem> items)
    {
      if ((UnityEngine.Object) item.mixer.outputAudioMixerGroup != (UnityEngine.Object) null)
      {
        AudioMixerItem itemInList = TreeViewUtility.FindItemInList<AudioMixerItem>(item.mixer.outputAudioMixerGroup.audioMixer.GetInstanceID(), items) as AudioMixerItem;
        if (itemInList == null)
          return;
        itemInList.AddChild((TreeViewItem) item);
      }
      else
        this.m_RootItem.AddChild((TreeViewItem) item);
    }

    private void SetItemDepthRecursive(TreeViewItem item, int depth)
    {
      item.depth = depth;
      if (!item.hasChildren)
        return;
      using (List<TreeViewItem>.Enumerator enumerator = item.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.SetItemDepthRecursive(enumerator.Current, depth + 1);
      }
    }

    private void SortRecursive(TreeViewItem item)
    {
      if (!item.hasChildren)
        return;
      item.children.Sort((IComparer<TreeViewItem>) new TreeViewItemAlphaNumericSort());
      using (List<TreeViewItem>.Enumerator enumerator = item.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.SortRecursive(enumerator.Current);
      }
    }

    public override bool IsRenamingItemAllowed(TreeViewItem item)
    {
      return true;
    }

    public int GetInsertAfterItemIDForNewItem(string newName, TreeViewItem parentItem)
    {
      int num = parentItem.id;
      if (!parentItem.hasChildren)
        return num;
      for (int index = 0; index < parentItem.children.Count; ++index)
      {
        int id = parentItem.children[index].id;
        if (EditorUtility.NaturalCompare(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(id)), newName) <= 0)
          num = id;
        else
          break;
      }
      return num;
    }

    public override void InsertFakeItem(int id, int parentID, string name, Texture2D icon)
    {
      TreeViewItem treeViewItem1 = this.FindItem(id);
      if (treeViewItem1 != null)
        Debug.LogError((object) ("Cannot insert fake Item because id is not unique " + (object) id + " Item already there: " + treeViewItem1.displayName));
      else if (this.FindItem(parentID) != null)
      {
        this.SetExpanded(parentID, true);
        List<TreeViewItem> rows = this.GetRows();
        int indexOfId1 = TreeView.GetIndexOfID(rows, parentID);
        TreeViewItem treeViewItem2 = indexOfId1 < 0 ? this.m_RootItem : rows[indexOfId1];
        int depth = treeViewItem2.depth + 1;
        this.m_FakeItem = new TreeViewItem(id, depth, treeViewItem2, name);
        this.m_FakeItem.icon = icon;
        int itemIdForNewItem = this.GetInsertAfterItemIDForNewItem(name, treeViewItem2);
        int indexOfId2 = TreeView.GetIndexOfID(rows, itemIdForNewItem);
        if (indexOfId2 >= 0)
        {
          do
            ;
          while (++indexOfId2 < rows.Count && rows[indexOfId2].depth > depth);
          if (indexOfId2 < rows.Count)
            rows.Insert(indexOfId2, this.m_FakeItem);
          else
            rows.Add(this.m_FakeItem);
        }
        else if (rows.Count > 0)
          rows.Insert(0, this.m_FakeItem);
        else
          rows.Add(this.m_FakeItem);
        this.m_NeedRefreshVisibleFolders = false;
        this.m_TreeView.Frame(this.m_FakeItem.id, true, false);
        this.m_TreeView.Repaint();
      }
      else
        Debug.LogError((object) ("No parent Item found with ID: " + (object) parentID));
    }
  }
}
