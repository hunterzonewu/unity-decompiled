// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerSnapshotListView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Audio;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

namespace UnityEditor
{
  internal class AudioMixerSnapshotListView
  {
    private ReorderableListWithRenameAndScrollView m_ReorderableListWithRenameAndScrollView;
    private AudioMixerController m_Controller;
    private List<AudioMixerSnapshotController> m_Snapshots;
    private ReorderableListWithRenameAndScrollView.State m_State;
    private static AudioMixerSnapshotListView.Styles s_Styles;

    public AudioMixerSnapshotListView(ReorderableListWithRenameAndScrollView.State state)
    {
      this.m_State = state;
    }

    public void OnMixerControllerChanged(AudioMixerController controller)
    {
      this.m_Controller = controller;
      this.RecreateListControl();
    }

    private int GetSnapshotIndex(AudioMixerSnapshotController snapshot)
    {
      for (int index = 0; index < this.m_Snapshots.Count; ++index)
      {
        if ((UnityEngine.Object) this.m_Snapshots[index] == (UnityEngine.Object) snapshot)
          return index;
      }
      return 0;
    }

    private void RecreateListControl()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_Snapshots = new List<AudioMixerSnapshotController>((IEnumerable<AudioMixerSnapshotController>) this.m_Controller.snapshots);
      this.m_ReorderableListWithRenameAndScrollView = new ReorderableListWithRenameAndScrollView(new ReorderableList((IList) this.m_Snapshots, typeof (AudioMixerSnapshotController), true, false, false, false)
      {
        onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.EndDragChild),
        elementHeight = 16f,
        headerHeight = 0.0f,
        footerHeight = 0.0f,
        showDefaultBackground = false,
        index = this.GetSnapshotIndex(this.m_Controller.TargetSnapshot)
      }, this.m_State);
      this.m_ReorderableListWithRenameAndScrollView.onSelectionChanged += new System.Action<int>(this.SelectionChanged);
      this.m_ReorderableListWithRenameAndScrollView.onNameChangedAtIndex += new System.Action<int, string>(this.NameChanged);
      this.m_ReorderableListWithRenameAndScrollView.onDeleteItemAtIndex += new System.Action<int>(this.Delete);
      this.m_ReorderableListWithRenameAndScrollView.onGetNameAtIndex += new Func<int, string>(this.GetNameOfElement);
      this.m_ReorderableListWithRenameAndScrollView.onCustomDrawElement += new ReorderableList.ElementCallbackDelegate(this.CustomDrawElement);
    }

    private void SaveToBackend()
    {
      this.m_Controller.snapshots = this.m_Snapshots.ToArray();
      this.m_Controller.OnSubAssetChanged();
    }

    public void LoadFromBackend()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_Snapshots.Clear();
      this.m_Snapshots.AddRange((IEnumerable<AudioMixerSnapshotController>) this.m_Controller.snapshots);
    }

    public void OnEvent()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_ReorderableListWithRenameAndScrollView.OnEvent();
    }

    public void CustomDrawElement(Rect r, int index, bool isActive, bool isFocused)
    {
      Event current = Event.current;
      if (current.type == EventType.MouseUp && current.button == 1 && r.Contains(current.mousePosition))
      {
        AudioMixerSnapshotListView.SnapshotMenu.Show(r, this.m_Snapshots[index], this);
        current.Use();
      }
      bool isSelected = index == this.m_ReorderableListWithRenameAndScrollView.list.index && !this.m_ReorderableListWithRenameAndScrollView.IsRenamingIndex(index);
      r.width -= 19f;
      this.m_ReorderableListWithRenameAndScrollView.DrawElementText(r, index, isActive, isSelected, isFocused);
      if (!((UnityEngine.Object) this.m_Controller.startSnapshot == (UnityEngine.Object) this.m_Snapshots[index]))
        return;
      r.x = (float) ((double) r.xMax + 5.0 + 5.0);
      r.y = r.y + (float) (((double) r.height - 14.0) / 2.0);
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Rect& local = @r;
      float num1 = 14f;
      r.height = num1;
      double num2 = (double) num1;
      // ISSUE: explicit reference operation
      (^local).width = (float) num2;
      GUI.Label(r, AudioMixerSnapshotListView.s_Styles.starIcon, GUIStyle.none);
    }

    public float GetTotalHeight()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return 0.0f;
      return this.m_ReorderableListWithRenameAndScrollView.list.GetHeight() + 22f;
    }

    public void OnGUI(Rect rect)
    {
      if (AudioMixerSnapshotListView.s_Styles == null)
        AudioMixerSnapshotListView.s_Styles = new AudioMixerSnapshotListView.Styles();
      EditorGUI.BeginDisabledGroup((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null);
      Rect headerRect;
      Rect contentRect;
      AudioMixerDrawUtils.DrawRegionBg(rect, out headerRect, out contentRect);
      AudioMixerDrawUtils.HeaderLabel(headerRect, AudioMixerSnapshotListView.s_Styles.header, AudioMixerSnapshotListView.s_Styles.snapshotsIcon);
      EditorGUI.EndDisabledGroup();
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null))
        return;
      int snapshotIndex = this.GetSnapshotIndex(this.m_Controller.TargetSnapshot);
      if (snapshotIndex != this.m_ReorderableListWithRenameAndScrollView.list.index)
      {
        this.m_ReorderableListWithRenameAndScrollView.list.index = snapshotIndex;
        this.m_ReorderableListWithRenameAndScrollView.FrameItem(snapshotIndex);
      }
      this.m_ReorderableListWithRenameAndScrollView.OnGUI(contentRect);
      if (!GUI.Button(new Rect(headerRect.xMax - 15f, headerRect.y + 3f, 15f, 15f), AudioMixerSnapshotListView.s_Styles.addButton, EditorStyles.label))
        return;
      this.Add();
    }

    public void SelectionChanged(int index)
    {
      if (index >= this.m_Snapshots.Count)
        index = this.m_Snapshots.Count - 1;
      this.m_Controller.TargetSnapshot = this.m_Snapshots[index];
      this.UpdateViews();
    }

    private string GetNameOfElement(int index)
    {
      return this.m_Snapshots[index].name;
    }

    public void NameChanged(int index, string newName)
    {
      this.m_Snapshots[index].name = newName;
      this.SaveToBackend();
    }

    private void DuplicateCurrentSnapshot()
    {
      Undo.RecordObject((UnityEngine.Object) this.m_Controller, "Duplicate current snapshot");
      this.m_Controller.CloneNewSnapshotFromTarget(true);
      this.LoadFromBackend();
      this.UpdateViews();
    }

    private void Add()
    {
      Undo.RecordObject((UnityEngine.Object) this.m_Controller, "Add new snapshot");
      this.m_Controller.CloneNewSnapshotFromTarget(true);
      this.LoadFromBackend();
      this.Rename(this.m_Controller.TargetSnapshot);
      this.UpdateViews();
    }

    private void DeleteSnapshot(AudioMixerSnapshotController snapshot)
    {
      if (this.m_Controller.snapshots.Length <= 1)
      {
        Debug.Log((object) "You must have at least 1 snapshot in an AudioMixer.");
      }
      else
      {
        this.m_Controller.RemoveSnapshot(snapshot);
        this.LoadFromBackend();
        this.m_ReorderableListWithRenameAndScrollView.list.index = this.GetSnapshotIndex(this.m_Controller.TargetSnapshot);
        this.UpdateViews();
      }
    }

    private void Delete(int index)
    {
      this.DeleteSnapshot(this.m_Snapshots[index]);
    }

    public void EndDragChild(ReorderableList list)
    {
      this.m_Snapshots = this.m_ReorderableListWithRenameAndScrollView.list.list as List<AudioMixerSnapshotController>;
      this.SaveToBackend();
    }

    private void UpdateViews()
    {
      AudioMixerWindow editorWindowOfType = (AudioMixerWindow) WindowLayout.FindEditorWindowOfType(typeof (AudioMixerWindow));
      if ((UnityEngine.Object) editorWindowOfType != (UnityEngine.Object) null)
        editorWindowOfType.Repaint();
      InspectorWindow.RepaintAllInspectors();
    }

    private void SetAsStartupSnapshot(AudioMixerSnapshotController snapshot)
    {
      this.m_Controller.startSnapshot = (AudioMixerSnapshot) snapshot;
    }

    private void Rename(AudioMixerSnapshotController snapshot)
    {
      this.m_ReorderableListWithRenameAndScrollView.BeginRename(this.GetSnapshotIndex(snapshot), 0.0f);
    }

    public void OnUndoRedoPerformed()
    {
      this.LoadFromBackend();
    }

    private class Styles
    {
      public GUIContent starIcon = new GUIContent((Texture) EditorGUIUtility.FindTexture("Favorite"), "Start snapshot");
      public GUIContent header = new GUIContent("Snapshots", "A snapshot is a set of values for all parameters in the mixer. When using the mixer you modify parameters in the selected snapshot. Blend between multiple snapshots at runtime.");
      public GUIContent addButton = new GUIContent("+");
      public Texture2D snapshotsIcon = EditorGUIUtility.FindTexture("AudioMixerSnapshot Icon");
    }

    internal class SnapshotMenu
    {
      public static void Show(Rect buttonRect, AudioMixerSnapshotController snapshot, AudioMixerSnapshotListView list)
      {
        GenericMenu genericMenu = new GenericMenu();
        AudioMixerSnapshotListView.SnapshotMenu.data data = new AudioMixerSnapshotListView.SnapshotMenu.data() { snapshot = snapshot, list = list };
        genericMenu.AddItem(new GUIContent("Set as start Snapshot"), false, new GenericMenu.MenuFunction2(AudioMixerSnapshotListView.SnapshotMenu.SetAsStartupSnapshot), (object) data);
        genericMenu.AddSeparator(string.Empty);
        genericMenu.AddItem(new GUIContent("Rename"), false, new GenericMenu.MenuFunction2(AudioMixerSnapshotListView.SnapshotMenu.Rename), (object) data);
        genericMenu.AddItem(new GUIContent("Duplicate"), false, new GenericMenu.MenuFunction2(AudioMixerSnapshotListView.SnapshotMenu.Duplicate), (object) data);
        genericMenu.AddItem(new GUIContent("Delete"), false, new GenericMenu.MenuFunction2(AudioMixerSnapshotListView.SnapshotMenu.Delete), (object) data);
        genericMenu.DropDown(buttonRect);
      }

      private static void SetAsStartupSnapshot(object userData)
      {
        AudioMixerSnapshotListView.SnapshotMenu.data data = userData as AudioMixerSnapshotListView.SnapshotMenu.data;
        data.list.SetAsStartupSnapshot(data.snapshot);
      }

      private static void Rename(object userData)
      {
        AudioMixerSnapshotListView.SnapshotMenu.data data = userData as AudioMixerSnapshotListView.SnapshotMenu.data;
        data.list.Rename(data.snapshot);
      }

      private static void Duplicate(object userData)
      {
        (userData as AudioMixerSnapshotListView.SnapshotMenu.data).list.DuplicateCurrentSnapshot();
      }

      private static void Delete(object userData)
      {
        AudioMixerSnapshotListView.SnapshotMenu.data data = userData as AudioMixerSnapshotListView.SnapshotMenu.data;
        data.list.DeleteSnapshot(data.snapshot);
      }

      private class data
      {
        public AudioMixerSnapshotController snapshot;
        public AudioMixerSnapshotListView list;
      }
    }
  }
}
