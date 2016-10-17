// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchy
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchy
  {
    private AnimationWindowState m_State;
    private TreeView m_TreeView;

    public AnimationWindowHierarchy(AnimationWindowState state, EditorWindow owner, Rect position)
    {
      this.m_State = state;
      this.Init(owner, position);
    }

    public void OnGUI(Rect position)
    {
      this.m_TreeView.OnEvent();
      this.m_TreeView.OnGUI(position, GUIUtility.GetControlID(FocusType.Keyboard));
    }

    public void Init(EditorWindow owner, Rect rect)
    {
      if (this.m_State.hierarchyState == null)
        this.m_State.hierarchyState = new AnimationWindowHierarchyState();
      this.m_TreeView = new TreeView(owner, (TreeViewState) this.m_State.hierarchyState);
      this.m_State.hierarchyData = new AnimationWindowHierarchyDataSource(this.m_TreeView, this.m_State);
      this.m_TreeView.Init(rect, (ITreeViewDataSource) this.m_State.hierarchyData, (ITreeViewGUI) new AnimationWindowHierarchyGUI(this.m_TreeView, this.m_State), (ITreeViewDragging) null);
      this.m_TreeView.deselectOnUnhandledMouseDown = true;
      this.m_TreeView.selectionChangedCallback += new System.Action<int[]>(this.m_State.OnHierarchySelectionChanged);
      this.m_TreeView.ReloadData();
    }

    internal virtual bool IsRenamingNodeAllowed(TreeViewItem node)
    {
      return true;
    }

    public bool IsIDVisible(int id)
    {
      if (this.m_TreeView == null)
        return false;
      return TreeView.GetIndexOfID(this.m_TreeView.data.GetRows(), id) >= 0;
    }
  }
}
