// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AudioProfilerView
  {
    private TreeView m_TreeView;
    private AudioProfilerTreeViewState m_TreeViewState;
    private EditorWindow m_EditorWindow;
    private AudioProfilerView.AudioProfilerViewColumnHeader m_ColumnHeader;
    private AudioProfilerBackend m_Backend;
    private GUIStyle m_HeaderStyle;
    private int delayedPingObject;

    public AudioProfilerView(EditorWindow editorWindow, AudioProfilerTreeViewState state)
    {
      this.m_EditorWindow = editorWindow;
      this.m_TreeViewState = state;
    }

    public int GetNumItemsInData()
    {
      return this.m_Backend.items.Count;
    }

    public void Init(Rect rect, AudioProfilerBackend backend)
    {
      if (this.m_HeaderStyle == null)
        this.m_HeaderStyle = new GUIStyle((GUIStyle) "OL title");
      this.m_HeaderStyle.alignment = TextAnchor.MiddleLeft;
      if (this.m_TreeView != null)
        return;
      this.m_Backend = backend;
      if (this.m_TreeViewState.columnWidths == null)
      {
        int length = AudioProfilerInfoHelper.GetLastColumnIndex() + 1;
        this.m_TreeViewState.columnWidths = new float[length];
        for (int index = 2; index < length; ++index)
          this.m_TreeViewState.columnWidths[index] = index == 2 || index == 3 || index >= 11 && index <= 16 ? 75f : 60f;
        this.m_TreeViewState.columnWidths[0] = 140f;
        this.m_TreeViewState.columnWidths[1] = 140f;
      }
      this.m_TreeView = new TreeView(this.m_EditorWindow, (TreeViewState) this.m_TreeViewState);
      ITreeViewGUI gui = (ITreeViewGUI) new AudioProfilerView.AudioProfilerViewGUI(this.m_TreeView);
      ITreeViewDataSource data = (ITreeViewDataSource) new AudioProfilerView.AudioProfilerDataSource(this.m_TreeView, this.m_Backend);
      this.m_TreeView.Init(rect, data, gui, (ITreeViewDragging) null);
      this.m_ColumnHeader = new AudioProfilerView.AudioProfilerViewColumnHeader(this.m_TreeViewState, this.m_Backend);
      this.m_ColumnHeader.columnWidths = this.m_TreeViewState.columnWidths;
      this.m_ColumnHeader.minColumnWidth = 30f;
      this.m_TreeView.selectionChangedCallback += new System.Action<int[]>(this.OnTreeSelectionChanged);
    }

    private void PingObjectDelayed()
    {
      EditorGUIUtility.PingObject(this.delayedPingObject);
    }

    public void OnTreeSelectionChanged(int[] selection)
    {
      if (selection.Length != 1)
        return;
      AudioProfilerView.AudioProfilerTreeViewItem node = this.m_TreeView.FindNode(selection[0]) as AudioProfilerView.AudioProfilerTreeViewItem;
      if (node == null)
        return;
      EditorGUIUtility.PingObject(node.info.info.assetInstanceId);
      this.delayedPingObject = node.info.info.objectInstanceId;
      EditorApplication.CallDelayed(new EditorApplication.CallbackFunction(this.PingObjectDelayed), 1f);
    }

    public void OnGUI(Rect rect, bool allowSorting)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard, rect);
      Rect rect1 = new Rect(rect.x, rect.y, rect.width, this.m_HeaderStyle.fixedHeight);
      GUI.Label(rect1, string.Empty, this.m_HeaderStyle);
      this.m_ColumnHeader.OnGUI(rect1, allowSorting, this.m_HeaderStyle);
      rect.y += rect1.height;
      rect.height -= rect1.height;
      this.m_TreeView.OnEvent();
      this.m_TreeView.OnGUI(rect, controlId);
    }

    internal class AudioProfilerTreeViewItem : TreeViewItem
    {
      public AudioProfilerInfoWrapper info { get; set; }

      public AudioProfilerTreeViewItem(int id, int depth, TreeViewItem parent, string displayName, AudioProfilerInfoWrapper info)
        : base(id, depth, parent, displayName)
      {
        this.info = info;
      }
    }

    internal class AudioProfilerDataSource : TreeViewDataSource
    {
      private AudioProfilerBackend m_Backend;

      public AudioProfilerDataSource(TreeView treeView, AudioProfilerBackend backend)
        : base(treeView)
      {
        this.m_Backend = backend;
        this.m_Backend.OnUpdate = new AudioProfilerBackend.DataUpdateDelegate(this.FetchData);
        this.showRootNode = false;
        this.rootIsCollapsable = false;
        this.FetchData();
      }

      private void FillTreeItems(AudioProfilerView.AudioProfilerTreeViewItem parentNode, int depth, int parentId, List<AudioProfilerInfoWrapper> items)
      {
        int capacity = 0;
        using (List<AudioProfilerInfoWrapper>.Enumerator enumerator = items.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AudioProfilerInfoWrapper current = enumerator.Current;
            if (parentId == (!current.addToRoot ? current.info.parentId : 0))
              ++capacity;
          }
        }
        if (capacity <= 0)
          return;
        parentNode.children = new List<TreeViewItem>(capacity);
        using (List<AudioProfilerInfoWrapper>.Enumerator enumerator = items.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AudioProfilerInfoWrapper current = enumerator.Current;
            if (parentId == (!current.addToRoot ? current.info.parentId : 0))
            {
              AudioProfilerView.AudioProfilerTreeViewItem parentNode1 = new AudioProfilerView.AudioProfilerTreeViewItem(current.info.uniqueId, !current.addToRoot ? depth : 1, (TreeViewItem) parentNode, current.objectName, current);
              parentNode.children.Add((TreeViewItem) parentNode1);
              this.FillTreeItems(parentNode1, depth + 1, current.info.uniqueId, items);
            }
          }
        }
      }

      public override void FetchData()
      {
        AudioProfilerView.AudioProfilerTreeViewItem parentNode = new AudioProfilerView.AudioProfilerTreeViewItem(1, 0, (TreeViewItem) null, "ROOT", new AudioProfilerInfoWrapper(new AudioProfilerInfo(), "ROOT", "ROOT", false));
        this.FillTreeItems(parentNode, 1, 0, this.m_Backend.items);
        this.m_RootItem = (TreeViewItem) parentNode;
        this.SetExpandedWithChildren(this.m_RootItem, true);
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

    internal class AudioProfilerViewColumnHeader
    {
      private AudioProfilerTreeViewState m_TreeViewState;
      private AudioProfilerBackend m_Backend;

      public float[] columnWidths { get; set; }

      public float minColumnWidth { get; set; }

      public float dragWidth { get; set; }

      public AudioProfilerViewColumnHeader(AudioProfilerTreeViewState state, AudioProfilerBackend backend)
      {
        this.m_TreeViewState = state;
        this.m_Backend = backend;
        this.minColumnWidth = 10f;
        this.dragWidth = 6f;
      }

      public void OnGUI(Rect rect, bool allowSorting, GUIStyle headerStyle)
      {
        GUIClip.Push(rect, Vector2.zero, Vector2.zero, false);
        float x1 = -this.m_TreeViewState.scrollPos.x;
        int lastColumnIndex = AudioProfilerInfoHelper.GetLastColumnIndex();
        for (int index = 0; index <= lastColumnIndex; ++index)
        {
          Rect position1 = new Rect(x1, 0.0f, this.columnWidths[index], rect.height - 1f);
          x1 += this.columnWidths[index];
          Rect position2 = new Rect(x1 - this.dragWidth / 2f, 0.0f, 3f, rect.height);
          float x2 = EditorGUI.MouseDeltaReader(position2, true).x;
          if ((double) x2 != 0.0)
          {
            this.columnWidths[index] += x2;
            this.columnWidths[index] = Mathf.Max(this.columnWidths[index], this.minColumnWidth);
          }
          string text = new string[23]{ "Object", "Asset", "Volume", "Audibility", "Plays", "3D", "Paused", "Muted", "Virtual", "OneShot", "Looped", "Distance", "MinDist", "MaxDist", "Time", "Duration", "Frequency", "Stream", "Compressed", "NonBlocking", "User", "Memory", "MemoryPoint" }[index];
          if (allowSorting && index == this.m_TreeViewState.selectedColumn)
            text += !this.m_TreeViewState.sortByDescendingOrder ? " ▲" : " ▼";
          GUI.Box(position1, text, headerStyle);
          if (allowSorting && Event.current.type == EventType.MouseDown && position1.Contains(Event.current.mousePosition))
          {
            this.m_TreeViewState.SetSelectedColumn(index);
            this.m_Backend.UpdateSorting();
          }
          if (Event.current.type == EventType.Repaint)
            EditorGUIUtility.AddCursorRect(position2, MouseCursor.SplitResizeLeftRight);
        }
        GUIClip.Pop();
      }
    }

    internal class AudioProfilerViewGUI : TreeViewGUI
    {
      private float[] columnWidths
      {
        get
        {
          return this.m_TreeView.state.columnWidths;
        }
      }

      public AudioProfilerViewGUI(TreeView treeView)
        : base(treeView)
      {
        this.k_IconWidth = 0.0f;
      }

      protected override Texture GetIconForNode(TreeViewItem item)
      {
        return (Texture) null;
      }

      protected override void RenameEnded()
      {
      }

      protected override void SyncFakeItem()
      {
      }

      public override Vector2 GetTotalSize()
      {
        Vector2 totalSize = base.GetTotalSize();
        totalSize.x = 0.0f;
        foreach (float columnWidth in this.columnWidths)
          totalSize.x += columnWidth;
        return totalSize;
      }

      protected override void DrawIconAndLabel(Rect rect, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
      {
        GUIStyle guiStyle = !useBoldFont ? TreeViewGUI.s_Styles.lineStyle : TreeViewGUI.s_Styles.lineBoldStyle;
        guiStyle.alignment = TextAnchor.MiddleLeft;
        guiStyle.padding.left = 0;
        int num = 2;
        base.DrawIconAndLabel(new Rect(rect.x, rect.y, this.columnWidths[0] - (float) num, rect.height), item, label, selected, focused, useBoldFont, isPinging);
        rect.x += this.columnWidths[0] + (float) num;
        AudioProfilerView.AudioProfilerTreeViewItem profilerTreeViewItem = item as AudioProfilerView.AudioProfilerTreeViewItem;
        for (int index = 1; index < this.columnWidths.Length; ++index)
        {
          rect.width = this.columnWidths[index] - (float) (2 * num);
          guiStyle.Draw(rect, AudioProfilerInfoHelper.GetColumnString(profilerTreeViewItem.info, (AudioProfilerInfoHelper.ColumnIndices) index), false, false, selected, focused);
          Handles.color = Color.black;
          Handles.DrawLine(new Vector3((float) ((double) rect.x - (double) num + 1.0), rect.y, 0.0f), new Vector3((float) ((double) rect.x - (double) num + 1.0), rect.y + rect.height, 0.0f));
          rect.x += this.columnWidths[index];
          guiStyle.alignment = TextAnchor.MiddleRight;
        }
        guiStyle.alignment = TextAnchor.MiddleLeft;
      }
    }
  }
}
