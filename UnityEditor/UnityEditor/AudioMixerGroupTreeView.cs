// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerGroupTreeView
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
  internal class AudioMixerGroupTreeView
  {
    private AudioMixerController m_Controller;
    private AudioGroupDataSource m_AudioGroupTreeDataSource;
    private TreeViewState m_AudioGroupTreeState;
    private TreeView m_AudioGroupTree;
    private int m_TreeViewKeyboardControlID;
    private AudioGroupTreeViewGUI m_TreeViewGUI;
    private AudioMixerGroupController m_ScrollToItem;
    private static AudioMixerGroupTreeView.Styles s_Styles;

    public AudioMixerController Controller
    {
      get
      {
        return this.m_Controller;
      }
    }

    public AudioMixerGroupController ScrollToItem
    {
      get
      {
        return this.m_ScrollToItem;
      }
    }

    public AudioMixerGroupTreeView(AudioMixerWindow mixerWindow, TreeViewState treeState)
    {
      this.m_AudioGroupTreeState = treeState;
      this.m_AudioGroupTree = new TreeView((EditorWindow) mixerWindow, this.m_AudioGroupTreeState);
      this.m_AudioGroupTree.deselectOnUnhandledMouseDown = false;
      this.m_AudioGroupTree.selectionChangedCallback += new System.Action<int[]>(this.OnTreeSelectionChanged);
      this.m_AudioGroupTree.contextClickItemCallback += new System.Action<int>(this.OnTreeViewContextClick);
      this.m_AudioGroupTree.expandedStateChanged += new System.Action(this.SaveExpandedState);
      this.m_TreeViewGUI = new AudioGroupTreeViewGUI(this.m_AudioGroupTree);
      this.m_TreeViewGUI.NodeWasToggled += new System.Action<AudioMixerTreeViewNode, bool>(this.OnNodeToggled);
      this.m_AudioGroupTreeDataSource = new AudioGroupDataSource(this.m_AudioGroupTree, this.m_Controller);
      this.m_AudioGroupTree.Init(mixerWindow.position, (ITreeViewDataSource) this.m_AudioGroupTreeDataSource, (ITreeViewGUI) this.m_TreeViewGUI, (ITreeViewDragging) new AudioGroupTreeViewDragging(this.m_AudioGroupTree, this));
      this.m_AudioGroupTree.ReloadData();
    }

    public void UseScrollView(bool useScrollView)
    {
      this.m_AudioGroupTree.SetUseScrollView(useScrollView);
    }

    public void ReloadTreeData()
    {
      this.m_AudioGroupTree.ReloadData();
    }

    public void ReloadTree()
    {
      this.m_AudioGroupTree.ReloadData();
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null))
        return;
      this.m_Controller.SanitizeGroupViews();
      this.m_Controller.OnSubAssetChanged();
    }

    public void AddChildGroupPopupCallback(object obj)
    {
      AudioMixerGroupPopupContext groupPopupContext = (AudioMixerGroupPopupContext) obj;
      if (groupPopupContext.groups == null || groupPopupContext.groups.Length <= 0)
        return;
      this.AddAudioMixerGroup(groupPopupContext.groups[0]);
    }

    public void AddSiblingGroupPopupCallback(object obj)
    {
      AudioMixerGroupPopupContext groupPopupContext = (AudioMixerGroupPopupContext) obj;
      if (groupPopupContext.groups == null || groupPopupContext.groups.Length <= 0)
        return;
      AudioMixerTreeViewNode node = this.m_AudioGroupTree.FindNode(groupPopupContext.groups[0].GetInstanceID()) as AudioMixerTreeViewNode;
      if (node == null)
        return;
      this.AddAudioMixerGroup((node.parent as AudioMixerTreeViewNode).group);
    }

    public void AddAudioMixerGroup(AudioMixerGroupController parent)
    {
      if ((UnityEngine.Object) parent == (UnityEngine.Object) null || (UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      Undo.RecordObjects(new UnityEngine.Object[2]
      {
        (UnityEngine.Object) this.m_Controller,
        (UnityEngine.Object) parent
      }, "Add Child Group");
      AudioMixerGroupController newGroup = this.m_Controller.CreateNewGroup("New Group", true);
      this.m_Controller.AddChildToParent(newGroup, parent);
      this.m_Controller.AddGroupToCurrentView(newGroup);
      Selection.objects = (UnityEngine.Object[]) new AudioMixerGroupController[1]
      {
        newGroup
      };
      this.m_Controller.OnUnitySelectionChanged();
      this.m_AudioGroupTree.SetSelection(new int[1]
      {
        newGroup.GetInstanceID()
      }, 1 != 0);
      this.ReloadTree();
      this.m_AudioGroupTree.BeginNameEditing(0.0f);
    }

    private static string PluralIfNeeded(int count)
    {
      if (count > 1)
        return "s";
      return string.Empty;
    }

    public void DeleteGroups(List<AudioMixerGroupController> groups, bool recordUndo)
    {
      using (List<AudioMixerGroupController>.Enumerator enumerator = groups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.HasDependentMixers())
          {
            if (!EditorUtility.DisplayDialog("Referenced Group", "Deleted group is referenced by another AudioMixer, are you sure?", "Delete", "Cancel"))
              return;
            break;
          }
        }
      }
      if (recordUndo)
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Controller, "Delete Group" + AudioMixerGroupTreeView.PluralIfNeeded(groups.Count));
      this.m_Controller.DeleteGroups(groups.ToArray());
      this.ReloadTree();
    }

    public void DuplicateGroups(List<AudioMixerGroupController> groups, bool recordUndo)
    {
      if (recordUndo)
        Undo.RecordObject((UnityEngine.Object) this.m_Controller, "Duplicate group" + AudioMixerGroupTreeView.PluralIfNeeded(groups.Count));
      List<AudioMixerGroupController> source = this.m_Controller.DuplicateGroups(groups.ToArray());
      if (source.Count <= 0)
        return;
      this.ReloadTree();
      int[] array = source.Select<AudioMixerGroupController, int>((Func<AudioMixerGroupController, int>) (audioMixerGroup => audioMixerGroup.GetInstanceID())).ToArray<int>();
      this.m_AudioGroupTree.SetSelection(array, false);
      this.m_AudioGroupTree.Frame(array[array.Length - 1], true, false);
    }

    private void DeleteGroupsPopupCallback(object obj)
    {
      ((AudioMixerGroupTreeView) obj).DeleteGroups(this.GetGroupSelectionWithoutMasterGroup(), true);
    }

    private void DuplicateGroupPopupCallback(object obj)
    {
      ((AudioMixerGroupTreeView) obj).DuplicateGroups(this.GetGroupSelectionWithoutMasterGroup(), true);
    }

    private void RenameGroupCallback(object obj)
    {
      this.m_AudioGroupTree.SetSelection(new int[1]
      {
        ((TreeViewItem) obj).id
      }, 0 != 0);
      this.m_AudioGroupTree.BeginNameEditing(0.0f);
    }

    private List<AudioMixerGroupController> GetGroupSelectionWithoutMasterGroup()
    {
      List<AudioMixerGroupController> groupsFromNodeIds = this.GetAudioMixerGroupsFromNodeIDs(this.m_AudioGroupTree.GetSelection());
      groupsFromNodeIds.Remove(this.m_Controller.masterGroup);
      return groupsFromNodeIds;
    }

    public void OnTreeViewContextClick(int index)
    {
      TreeViewItem node = this.m_AudioGroupTree.FindNode(index);
      if (node == null)
        return;
      AudioMixerTreeViewNode mixerTreeViewNode = node as AudioMixerTreeViewNode;
      if (mixerTreeViewNode == null || !((UnityEngine.Object) mixerTreeViewNode.group != (UnityEngine.Object) null))
        return;
      GenericMenu genericMenu = new GenericMenu();
      if (!EditorApplication.isPlaying)
      {
        genericMenu.AddItem(new GUIContent("Add child group"), false, new GenericMenu.MenuFunction2(this.AddChildGroupPopupCallback), (object) new AudioMixerGroupPopupContext(this.m_Controller, mixerTreeViewNode.group));
        if ((UnityEngine.Object) mixerTreeViewNode.group != (UnityEngine.Object) this.m_Controller.masterGroup)
        {
          genericMenu.AddItem(new GUIContent("Add sibling group"), false, new GenericMenu.MenuFunction2(this.AddSiblingGroupPopupCallback), (object) new AudioMixerGroupPopupContext(this.m_Controller, mixerTreeViewNode.group));
          genericMenu.AddSeparator(string.Empty);
          genericMenu.AddItem(new GUIContent("Rename"), false, new GenericMenu.MenuFunction2(this.RenameGroupCallback), (object) node);
          AudioMixerGroupController[] array = this.GetGroupSelectionWithoutMasterGroup().ToArray();
          genericMenu.AddItem(new GUIContent(array.Length <= 1 ? "Duplicate group (and children)" : "Duplicate groups (and children)"), false, new GenericMenu.MenuFunction2(this.DuplicateGroupPopupCallback), (object) this);
          genericMenu.AddItem(new GUIContent(array.Length <= 1 ? "Remove group (and children)" : "Remove groups (and children)"), false, new GenericMenu.MenuFunction2(this.DeleteGroupsPopupCallback), (object) this);
        }
      }
      else
        genericMenu.AddDisabledItem(new GUIContent("Modifying group topology in play mode is not allowed"));
      genericMenu.ShowAsContext();
    }

    private void OnNodeToggled(AudioMixerTreeViewNode node, bool nodeWasEnabled)
    {
      List<AudioMixerGroupController> mixerGroupControllerList = this.GetAudioMixerGroupsFromNodeIDs(this.m_AudioGroupTree.GetSelection());
      if (!mixerGroupControllerList.Contains(node.group))
        mixerGroupControllerList = new List<AudioMixerGroupController>()
        {
          node.group
        };
      List<GUID> guidList = new List<GUID>();
      using (List<AudioMixerGroupController>.Enumerator enumerator = this.m_Controller.GetAllAudioGroupsSlow().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          bool flag1 = this.m_Controller.CurrentViewContainsGroup(current.groupID);
          bool flag2 = mixerGroupControllerList.Contains(current);
          bool flag3 = flag1 && !flag2;
          if (!flag1 && flag2)
            flag3 = nodeWasEnabled;
          if (flag3)
            guidList.Add(current.groupID);
        }
      }
      this.m_Controller.SetCurrentViewVisibility(guidList.ToArray());
    }

    private List<AudioMixerGroupController> GetAudioMixerGroupsFromNodeIDs(int[] instanceIDs)
    {
      List<AudioMixerGroupController> mixerGroupControllerList = new List<AudioMixerGroupController>();
      foreach (int instanceId in instanceIDs)
      {
        TreeViewItem node = this.m_AudioGroupTree.FindNode(instanceId);
        if (node != null)
        {
          AudioMixerTreeViewNode mixerTreeViewNode = node as AudioMixerTreeViewNode;
          if (mixerTreeViewNode != null)
            mixerGroupControllerList.Add(mixerTreeViewNode.group);
        }
      }
      return mixerGroupControllerList;
    }

    public void OnTreeSelectionChanged(int[] selection)
    {
      List<AudioMixerGroupController> groupsFromNodeIds = this.GetAudioMixerGroupsFromNodeIDs(selection);
      Selection.objects = (UnityEngine.Object[]) groupsFromNodeIds.ToArray();
      this.m_Controller.OnUnitySelectionChanged();
      if (groupsFromNodeIds.Count == 1)
        this.m_ScrollToItem = groupsFromNodeIds[0];
      InspectorWindow.RepaintAllInspectors();
    }

    public void InitSelection(bool revealSelectionAndFrameLastSelected)
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_AudioGroupTree.SetSelection(this.m_Controller.CachedSelection.Select<AudioMixerGroupController, int>((Func<AudioMixerGroupController, int>) (x => x.GetInstanceID())).ToArray<int>(), revealSelectionAndFrameLastSelected);
    }

    public float GetTotalHeight()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return 0.0f;
      return this.m_AudioGroupTree.gui.GetTotalSize().y + 22f;
    }

    public void OnGUI(Rect rect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard);
      this.m_ScrollToItem = (AudioMixerGroupController) null;
      if (AudioMixerGroupTreeView.s_Styles == null)
        AudioMixerGroupTreeView.s_Styles = new AudioMixerGroupTreeView.Styles();
      this.m_AudioGroupTree.OnEvent();
      EditorGUI.BeginDisabledGroup((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null);
      Rect headerRect;
      Rect contentRect;
      AudioMixerDrawUtils.DrawRegionBg(rect, out headerRect, out contentRect);
      AudioMixerDrawUtils.HeaderLabel(headerRect, AudioMixerGroupTreeView.s_Styles.header, AudioMixerGroupTreeView.s_Styles.audioMixerGroupIcon);
      EditorGUI.EndDisabledGroup();
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null))
        return;
      AudioMixerGroupController parent = this.m_Controller.CachedSelection.Count != 1 ? this.m_Controller.masterGroup : this.m_Controller.CachedSelection[0];
      EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
      if (GUI.Button(new Rect(headerRect.xMax - 15f, headerRect.y + 3f, 15f, 15f), AudioMixerGroupTreeView.s_Styles.addText, EditorStyles.label))
        this.AddAudioMixerGroup(parent);
      EditorGUI.EndDisabledGroup();
      this.m_AudioGroupTree.OnGUI(contentRect, controlId);
      AudioMixerDrawUtils.DrawScrollDropShadow(contentRect, this.m_AudioGroupTree.state.scrollPos.y, this.m_AudioGroupTree.gui.GetTotalSize().y);
      this.HandleKeyboardEvents(controlId);
      this.HandleCommandEvents(controlId);
    }

    private void HandleCommandEvents(int treeViewKeyboardControlID)
    {
      if (GUIUtility.keyboardControl != treeViewKeyboardControlID)
        return;
      EventType type = Event.current.type;
      switch (type)
      {
        case EventType.ExecuteCommand:
        case EventType.ValidateCommand:
          bool flag = type == EventType.ExecuteCommand;
          if (Event.current.commandName == "Delete" || Event.current.commandName == "SoftDelete")
          {
            Event.current.Use();
            if (!flag)
              break;
            this.DeleteGroups(this.GetGroupSelectionWithoutMasterGroup(), true);
            GUIUtility.ExitGUI();
            break;
          }
          if (!(Event.current.commandName == "Duplicate"))
            break;
          Event.current.Use();
          if (!flag)
            break;
          this.DuplicateGroups(this.GetGroupSelectionWithoutMasterGroup(), true);
          break;
      }
    }

    private void HandleKeyboardEvents(int treeViewKeyboardControlID)
    {
      if (GUIUtility.keyboardControl != treeViewKeyboardControlID)
        return;
      Event current = Event.current;
      if (current.keyCode != KeyCode.Space || current.type != EventType.KeyDown)
        return;
      int[] selection = this.m_AudioGroupTree.GetSelection();
      if (selection.Length <= 0)
        return;
      AudioMixerTreeViewNode node = this.m_AudioGroupTree.FindNode(selection[0]) as AudioMixerTreeViewNode;
      bool flag = this.m_Controller.CurrentViewContainsGroup(node.group.groupID);
      this.OnNodeToggled(node, !flag);
      current.Use();
    }

    public void OnMixerControllerChanged(AudioMixerController controller)
    {
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) controller))
        return;
      this.m_TreeViewGUI.m_Controller = controller;
      this.m_Controller = controller;
      this.m_AudioGroupTreeDataSource.m_Controller = controller;
      if (!((UnityEngine.Object) controller != (UnityEngine.Object) null))
        return;
      this.ReloadTree();
      this.InitSelection(false);
      this.LoadExpandedState();
      this.m_AudioGroupTree.data.SetExpandedWithChildren(this.m_AudioGroupTree.data.root, true);
    }

    private static string GetUniqueAudioMixerName(AudioMixerController controller)
    {
      return "AudioMixer_" + (object) controller.GetInstanceID();
    }

    private void SaveExpandedState()
    {
      SessionState.SetIntArray(AudioMixerGroupTreeView.GetUniqueAudioMixerName(this.m_Controller), this.m_AudioGroupTreeState.expandedIDs.ToArray());
    }

    private void LoadExpandedState()
    {
      int[] intArray = SessionState.GetIntArray(AudioMixerGroupTreeView.GetUniqueAudioMixerName(this.m_Controller), (int[]) null);
      if (intArray != null)
      {
        this.m_AudioGroupTreeState.expandedIDs = new List<int>((IEnumerable<int>) intArray);
      }
      else
      {
        this.m_AudioGroupTree.state.expandedIDs = new List<int>();
        this.m_AudioGroupTree.data.SetExpandedWithChildren(this.m_AudioGroupTree.data.root, true);
      }
    }

    public void EndRenaming()
    {
      this.m_AudioGroupTree.EndNameEditing(true);
    }

    public void OnUndoRedoPerformed()
    {
      this.ReloadTree();
    }

    private class Styles
    {
      public GUIStyle optionsButton = (GUIStyle) "PaneOptions";
      public GUIContent header = new GUIContent("Groups", "An Audio Mixer Group is used by e.g Audio Sources to modify the audio output before it reaches the Audio Listener. An Audio Mixer Group will route its output to another Audio Mixer Group if it is made a child of that group. The Master Group will route its output to the Audio Listener if it doesn't route its output into another Mixer.");
      public GUIContent addText = new GUIContent("+", "Add child group");
      public Texture2D audioMixerGroupIcon = EditorGUIUtility.FindTexture("AudioMixerGroup Icon");
    }
  }
}
