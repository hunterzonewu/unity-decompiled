// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectTreeForSelector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [Serializable]
  internal class ObjectTreeForSelector
  {
    private int m_LastSelectedID = -1;
    private string m_SelectedPath = string.Empty;
    private const string kSearchFieldTag = "TreeSearchField";
    private const float kBottomBarHeight = 17f;
    private const float kTopBarHeight = 27f;
    private EditorWindow m_Owner;
    private TreeView m_TreeView;
    private TreeViewState m_TreeViewState;
    private bool m_FocusSearchFilter;
    private int m_ErrorCounter;
    private int m_OriginalSelectedID;
    private int m_UserData;
    private ObjectTreeForSelector.SelectionEvent m_SelectionEvent;
    private ObjectTreeForSelector.TreeViewNeededEvent m_TreeViewNeededEvent;
    private ObjectTreeForSelector.DoubleClickedEvent m_DoubleClickedEvent;
    private static ObjectTreeForSelector.Styles s_Styles;

    public bool IsInitialized()
    {
      return (UnityEngine.Object) this.m_Owner != (UnityEngine.Object) null;
    }

    public void Init(Rect position, EditorWindow owner, UnityAction<ObjectTreeForSelector.TreeSelectorData> treeViewNeededCallback, UnityAction<TreeViewItem> selectionCallback, UnityAction doubleClickedCallback, int initialSelectedTreeViewItemID, int userData)
    {
      this.Clear();
      this.m_Owner = owner;
      this.m_TreeViewNeededEvent = new ObjectTreeForSelector.TreeViewNeededEvent();
      this.m_TreeViewNeededEvent.AddPersistentListener(treeViewNeededCallback, UnityEventCallState.EditorAndRuntime);
      this.m_SelectionEvent = new ObjectTreeForSelector.SelectionEvent();
      this.m_SelectionEvent.AddPersistentListener(selectionCallback, UnityEventCallState.EditorAndRuntime);
      this.m_DoubleClickedEvent = new ObjectTreeForSelector.DoubleClickedEvent();
      this.m_DoubleClickedEvent.AddPersistentListener(doubleClickedCallback, UnityEventCallState.EditorAndRuntime);
      this.m_OriginalSelectedID = initialSelectedTreeViewItemID;
      this.m_UserData = userData;
      this.m_FocusSearchFilter = true;
      this.EnsureTreeViewIsValid(this.GetTreeViewRect(position));
      if (this.m_TreeView == null)
        return;
      this.m_TreeView.SetSelection(new int[1]
      {
        this.m_OriginalSelectedID
      }, 1 != 0);
      if (this.m_OriginalSelectedID != 0)
        return;
      this.m_TreeView.data.SetExpandedWithChildren(this.m_TreeView.data.root, true);
    }

    public void Clear()
    {
      this.m_Owner = (EditorWindow) null;
      this.m_TreeViewNeededEvent = (ObjectTreeForSelector.TreeViewNeededEvent) null;
      this.m_SelectionEvent = (ObjectTreeForSelector.SelectionEvent) null;
      this.m_DoubleClickedEvent = (ObjectTreeForSelector.DoubleClickedEvent) null;
      this.m_OriginalSelectedID = 0;
      this.m_UserData = 0;
      this.m_TreeView = (TreeView) null;
      this.m_TreeViewState = (TreeViewState) null;
      this.m_ErrorCounter = 0;
      this.m_FocusSearchFilter = false;
    }

    public int[] GetSelection()
    {
      if (this.m_TreeView != null)
        return this.m_TreeView.GetSelection();
      return new int[0];
    }

    public void SetTreeView(TreeView treeView)
    {
      this.m_TreeView = treeView;
      this.m_TreeView.selectionChangedCallback -= new System.Action<int[]>(this.OnItemSelectionChanged);
      this.m_TreeView.selectionChangedCallback += new System.Action<int[]>(this.OnItemSelectionChanged);
      this.m_TreeView.itemDoubleClickedCallback -= new System.Action<int>(this.OnItemDoubleClicked);
      this.m_TreeView.itemDoubleClickedCallback += new System.Action<int>(this.OnItemDoubleClicked);
    }

    private bool EnsureTreeViewIsValid(Rect treeViewRect)
    {
      if (this.m_TreeViewState == null)
        this.m_TreeViewState = new TreeViewState();
      if (this.m_TreeView == null)
      {
        this.m_TreeViewNeededEvent.Invoke(new ObjectTreeForSelector.TreeSelectorData()
        {
          state = this.m_TreeViewState,
          treeViewRect = treeViewRect,
          userData = this.m_UserData,
          objectTreeForSelector = this,
          editorWindow = this.m_Owner
        });
        if (this.m_TreeView != null && this.m_TreeView.data.root == null)
          this.m_TreeView.ReloadData();
        if (this.m_TreeView == null)
        {
          if (this.m_ErrorCounter == 0)
          {
            Debug.LogError((object) "ObjectTreeSelector is missing its tree view. Ensure to call 'SetTreeView()' when the treeViewNeededCallback is invoked!");
            ++this.m_ErrorCounter;
          }
          return false;
        }
      }
      return true;
    }

    private Rect GetTreeViewRect(Rect position)
    {
      return new Rect(0.0f, 27f, position.width, (float) ((double) position.height - 17.0 - 27.0));
    }

    public void OnGUI(Rect position)
    {
      if (ObjectTreeForSelector.s_Styles == null)
        ObjectTreeForSelector.s_Styles = new ObjectTreeForSelector.Styles();
      Rect rect = new Rect(0.0f, 0.0f, position.width, position.height);
      Rect toolbarRect = new Rect(rect.x, rect.y, rect.width, 27f);
      Rect bottomRect = new Rect(rect.x, rect.yMax - 17f, rect.width, 17f);
      Rect treeViewRect = this.GetTreeViewRect(position);
      if (!this.EnsureTreeViewIsValid(treeViewRect))
        return;
      int controlId = GUIUtility.GetControlID("Tree".GetHashCode(), FocusType.Keyboard);
      this.HandleCommandEvents();
      this.HandleKeyboard(controlId);
      this.SearchArea(toolbarRect);
      this.TreeViewArea(treeViewRect, controlId);
      this.BottomBar(bottomRect);
    }

    private void BottomBar(Rect bottomRect)
    {
      int id = ((IEnumerable<int>) this.m_TreeView.GetSelection()).FirstOrDefault<int>();
      if (id != this.m_LastSelectedID)
      {
        this.m_LastSelectedID = id;
        this.m_SelectedPath = string.Empty;
        TreeViewItem node = this.m_TreeView.FindNode(id);
        if (node != null)
        {
          StringBuilder stringBuilder = new StringBuilder();
          for (TreeViewItem treeViewItem = node; treeViewItem != null && treeViewItem != this.m_TreeView.data.root; treeViewItem = treeViewItem.parent)
          {
            if (treeViewItem != node)
              stringBuilder.Insert(0, "/");
            stringBuilder.Insert(0, treeViewItem.displayName);
          }
          this.m_SelectedPath = stringBuilder.ToString();
        }
      }
      GUI.Label(bottomRect, GUIContent.none, ObjectTreeForSelector.s_Styles.bottomBarBg);
      if (string.IsNullOrEmpty(this.m_SelectedPath))
        return;
      GUI.Label(bottomRect, GUIContent.Temp(this.m_SelectedPath), EditorStyles.miniLabel);
    }

    private void OnItemDoubleClicked(int id)
    {
      if (this.m_DoubleClickedEvent == null)
        return;
      this.m_DoubleClickedEvent.Invoke();
    }

    private void OnItemSelectionChanged(int[] selection)
    {
      if (this.m_SelectionEvent == null)
        return;
      TreeViewItem treeViewItem = (TreeViewItem) null;
      if (selection.Length > 0)
        treeViewItem = this.m_TreeView.FindNode(selection[0]);
      this.FireSelectionEvent(treeViewItem);
    }

    private void HandleKeyboard(int treeViewControlID)
    {
      if (Event.current.type != EventType.KeyDown)
        return;
      switch (Event.current.keyCode)
      {
        case KeyCode.UpArrow:
        case KeyCode.DownArrow:
          if (!(GUI.GetNameOfFocusedControl() == "TreeSearchField"))
            break;
          GUIUtility.keyboardControl = treeViewControlID;
          if (this.m_TreeView.IsLastClickedPartOfRows())
            this.FrameSelectedTreeViewItem();
          else
            this.m_TreeView.OffsetSelection(1);
          Event.current.Use();
          break;
      }
    }

    private void FrameSelectedTreeViewItem()
    {
      this.m_TreeView.Frame(this.m_TreeView.state.lastClickedID, true, false);
    }

    private void HandleCommandEvents()
    {
      Event current = Event.current;
      if (current.type != EventType.ExecuteCommand && current.type != EventType.ValidateCommand)
        return;
      if (current.commandName == "FrameSelected")
      {
        if (current.type == EventType.ExecuteCommand && this.m_TreeView.HasSelection())
        {
          this.m_TreeView.searchString = string.Empty;
          this.FrameSelectedTreeViewItem();
        }
        current.Use();
        GUIUtility.ExitGUI();
      }
      if (!(current.commandName == "Find"))
        return;
      if (current.type == EventType.ExecuteCommand)
        this.FocusSearchField();
      current.Use();
    }

    private void FireSelectionEvent(TreeViewItem item)
    {
      if (this.m_SelectionEvent == null)
        return;
      this.m_SelectionEvent.Invoke(item);
    }

    private void TreeViewArea(Rect treeViewRect, int treeViewControlID)
    {
      if (this.m_TreeView.data.rowCount <= 0)
        return;
      this.m_TreeView.OnGUI(treeViewRect, treeViewControlID);
    }

    private void SearchArea(Rect toolbarRect)
    {
      GUI.Label(toolbarRect, GUIContent.none, ObjectTreeForSelector.s_Styles.searchBg);
      bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape;
      GUI.SetNextControlName("TreeSearchField");
      string str = EditorGUI.SearchField(new Rect(5f, 5f, toolbarRect.width - 10f, 15f), this.m_TreeView.searchString);
      if (flag && Event.current.type == EventType.Used && this.m_TreeView.searchString != string.Empty)
        this.m_FocusSearchFilter = true;
      if (str != this.m_TreeView.searchString || this.m_FocusSearchFilter)
      {
        this.m_TreeView.searchString = str;
        HandleUtility.Repaint();
      }
      if (!this.m_FocusSearchFilter)
        return;
      EditorGUI.FocusTextInControl("TreeSearchField");
      if (Event.current.type != EventType.Repaint)
        return;
      this.m_FocusSearchFilter = false;
    }

    internal void FocusSearchField()
    {
      this.m_FocusSearchFilter = true;
    }

    internal class TreeSelectorData
    {
      public ObjectTreeForSelector objectTreeForSelector;
      public EditorWindow editorWindow;
      public TreeViewState state;
      public Rect treeViewRect;
      public int userData;
    }

    [Serializable]
    public class SelectionEvent : UnityEvent<TreeViewItem>
    {
    }

    [Serializable]
    public class TreeViewNeededEvent : UnityEvent<ObjectTreeForSelector.TreeSelectorData>
    {
    }

    [Serializable]
    public class DoubleClickedEvent : UnityEvent
    {
    }

    private class Styles
    {
      public GUIStyle searchBg = new GUIStyle((GUIStyle) "ProjectBrowserTopBarBg");
      public GUIStyle bottomBarBg = new GUIStyle((GUIStyle) "ProjectBrowserBottomBarBg");

      public Styles()
      {
        this.searchBg.border = new RectOffset(0, 0, 2, 2);
        this.searchBg.fixedHeight = 0.0f;
        this.bottomBarBg.alignment = TextAnchor.MiddleLeft;
        this.bottomBarBg.fontSize = EditorStyles.label.fontSize;
        this.bottomBarBg.padding = new RectOffset(5, 5, 0, 0);
      }
    }
  }
}
