// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixersTreeView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor
{
  internal class AudioMixersTreeView
  {
    private const int kObjectSelectorID = 1212;
    private const string kExpandedStateIdentifier = "AudioMixerWindowMixers";
    private TreeView m_TreeView;
    private int m_TreeViewKeyboardControlID;
    private List<AudioMixerController> m_DraggedMixers;
    private static AudioMixersTreeView.Styles s_Styles;

    public AudioMixersTreeView(AudioMixerWindow mixerWindow, TreeViewState treeState, Func<List<AudioMixerController>> getAllControllersCallback)
    {
      this.m_TreeView = new TreeView((EditorWindow) mixerWindow, treeState);
      this.m_TreeView.deselectOnUnhandledMouseDown = false;
      this.m_TreeView.selectionChangedCallback += new System.Action<int[]>(this.OnTreeSelectionChanged);
      this.m_TreeView.contextClickItemCallback += new System.Action<int>(this.OnTreeViewContextClick);
      AudioMixersTreeViewGUI mixersTreeViewGui = new AudioMixersTreeViewGUI(this.m_TreeView);
      AudioMixersDataSource mixersDataSource = new AudioMixersDataSource(this.m_TreeView, getAllControllersCallback);
      AudioMixerTreeViewDragging treeViewDragging = new AudioMixerTreeViewDragging(this.m_TreeView, new System.Action<List<AudioMixerController>, AudioMixerController>(this.OnMixersDroppedOnMixerCallback));
      this.m_TreeView.Init(mixerWindow.position, (ITreeViewDataSource) mixersDataSource, (ITreeViewGUI) mixersTreeViewGui, (ITreeViewDragging) treeViewDragging);
      this.m_TreeView.ReloadData();
    }

    public void ReloadTree()
    {
      this.m_TreeView.ReloadData();
      this.m_TreeView.Repaint();
    }

    public void OnMixerControllerChanged(AudioMixerController controller)
    {
      if (!((UnityEngine.Object) controller != (UnityEngine.Object) null))
        return;
      this.m_TreeView.SetSelection(new int[1]
      {
        controller.GetInstanceID()
      }, 1 != 0);
    }

    public void DeleteAudioMixerCallback(object obj)
    {
      AudioMixerController audioMixerController = (AudioMixerController) obj;
      if (!((UnityEngine.Object) audioMixerController != (UnityEngine.Object) null))
        return;
      Selection.activeObject = (UnityEngine.Object) audioMixerController;
      ProjectBrowser.DeleteSelectedAssets(true);
    }

    public void OnTreeViewContextClick(int index)
    {
      AudioMixerItem node = (AudioMixerItem) this.m_TreeView.FindNode(index);
      if (node == null)
        return;
      GenericMenu genericMenu = new GenericMenu();
      genericMenu.AddItem(new GUIContent("Delete AudioMixer"), false, new GenericMenu.MenuFunction2(this.DeleteAudioMixerCallback), (object) node.mixer);
      genericMenu.ShowAsContext();
    }

    public void OnTreeSelectionChanged(int[] selection)
    {
      Selection.instanceIDs = selection;
    }

    public float GetTotalHeight()
    {
      return 22f + Mathf.Max(20f, this.m_TreeView.gui.GetTotalSize().y);
    }

    public void OnGUI(Rect rect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard);
      if (AudioMixersTreeView.s_Styles == null)
        AudioMixersTreeView.s_Styles = new AudioMixersTreeView.Styles();
      this.m_TreeView.OnEvent();
      Rect headerRect;
      Rect contentRect;
      AudioMixerDrawUtils.DrawRegionBg(rect, out headerRect, out contentRect);
      AudioMixerDrawUtils.HeaderLabel(headerRect, AudioMixersTreeView.s_Styles.header, AudioMixersTreeView.s_Styles.audioMixerIcon);
      if (GUI.Button(new Rect(headerRect.xMax - 15f, headerRect.y + 3f, 15f, 15f), AudioMixersTreeView.s_Styles.addText, EditorStyles.label))
        (this.m_TreeView.gui as AudioMixersTreeViewGUI).BeginCreateNewMixer();
      this.m_TreeView.OnGUI(contentRect, controlId);
      if (this.m_TreeView.data.rowCount == 0)
      {
        EditorGUI.BeginDisabledGroup(true);
        GUI.Label(new RectOffset(-20, 0, -2, 0).Add(contentRect), "No mixers found");
        EditorGUI.EndDisabledGroup();
      }
      AudioMixerDrawUtils.DrawScrollDropShadow(contentRect, this.m_TreeView.state.scrollPos.y, this.m_TreeView.gui.GetTotalSize().y);
      this.HandleCommandEvents(controlId);
      this.HandleObjectSelectorResult();
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
            ProjectBrowser.DeleteSelectedAssets(true);
            break;
          }
          if (!(Event.current.commandName == "Duplicate"))
            break;
          Event.current.Use();
          if (!flag)
            break;
          ProjectWindowUtil.DuplicateSelectedAssets();
          break;
      }
    }

    public void EndRenaming()
    {
      this.m_TreeView.EndNameEditing(true);
    }

    public void OnUndoRedoPerformed()
    {
      this.ReloadTree();
    }

    private void OnMixersDroppedOnMixerCallback(List<AudioMixerController> draggedMixers, AudioMixerController droppedUponMixer)
    {
      int[] array = draggedMixers.Select<AudioMixerController, int>((Func<AudioMixerController, int>) (i => i.GetInstanceID())).ToArray<int>();
      this.m_TreeView.SetSelection(array, true);
      Selection.instanceIDs = array;
      if ((UnityEngine.Object) droppedUponMixer == (UnityEngine.Object) null)
      {
        Undo.RecordObjects((UnityEngine.Object[]) draggedMixers.ToArray(), "Set output group for mixer" + (draggedMixers.Count <= 1 ? string.Empty : "s"));
        using (List<AudioMixerController>.Enumerator enumerator = draggedMixers.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.outputAudioMixerGroup = (AudioMixerGroup) null;
        }
        this.ReloadTree();
      }
      else
      {
        this.m_DraggedMixers = draggedMixers;
        ObjectSelector.get.Show(draggedMixers.Count != 1 ? (UnityEngine.Object) null : (UnityEngine.Object) draggedMixers[0].outputAudioMixerGroup, typeof (AudioMixerGroup), (SerializedProperty) null, 0 != 0, new List<int>()
        {
          droppedUponMixer.GetInstanceID()
        });
        ObjectSelector.get.objectSelectorID = 1212;
        ObjectSelector.get.titleContent = new GUIContent("Select Output Audio Mixer Group");
        GUIUtility.ExitGUI();
      }
    }

    private void HandleObjectSelectorResult()
    {
      Event current1 = Event.current;
      if (current1.type != EventType.ExecuteCommand)
        return;
      string commandName = current1.commandName;
      if (commandName == "ObjectSelectorUpdated" && ObjectSelector.get.objectSelectorID == 1212)
      {
        if (this.m_DraggedMixers == null || this.m_DraggedMixers.Count == 0)
          Debug.LogError((object) "Unexpected invalid mixer list used for dragging");
        UnityEngine.Object currentObject = ObjectSelector.GetCurrentObject();
        AudioMixerGroup audioMixerGroup = !(currentObject != (UnityEngine.Object) null) ? (AudioMixerGroup) null : currentObject as AudioMixerGroup;
        Undo.RecordObjects((UnityEngine.Object[]) this.m_DraggedMixers.ToArray(), "Set output group for mixer" + (this.m_DraggedMixers.Count <= 1 ? string.Empty : "s"));
        using (List<AudioMixerController>.Enumerator enumerator = this.m_DraggedMixers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AudioMixerController current2 = enumerator.Current;
            if ((UnityEngine.Object) current2 != (UnityEngine.Object) null)
              current2.outputAudioMixerGroup = audioMixerGroup;
            else
              Debug.LogError((object) "invalid mixer: is null");
          }
        }
        GUI.changed = true;
        current1.Use();
        this.ReloadTree();
        this.m_TreeView.SetSelection(this.m_DraggedMixers.Select<AudioMixerController, int>((Func<AudioMixerController, int>) (i => i.GetInstanceID())).ToArray<int>(), true);
      }
      if (!(commandName == "ObjectSelectorClosed"))
        return;
      this.m_DraggedMixers = (List<AudioMixerController>) null;
    }

    private class Styles
    {
      public GUIStyle optionsButton = (GUIStyle) "PaneOptions";
      public GUIContent header = new GUIContent("Mixers", "All mixers in the project are shown here. By default a mixer outputs to the AudioListener but mixers can also route their output to other mixers. Each mixer shows where it outputs (in parenthesis). To reroute a mixer simply drag the mixer upon another mixer and select a group from the popup.");
      public GUIContent addText = new GUIContent("+", "Add mixer asset. The asset will be saved in the same folder as the current selected mixer or if none is selected saved in the Assets folder.");
      public Texture2D audioMixerIcon = EditorGUIUtility.FindTexture("AudioMixerController Icon");
    }
  }
}
