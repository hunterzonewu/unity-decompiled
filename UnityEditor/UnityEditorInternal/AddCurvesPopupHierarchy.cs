// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopupHierarchy
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AddCurvesPopupHierarchy
  {
    private TreeView m_TreeView;
    private TreeViewState m_TreeViewState;
    private AddCurvesPopupHierarchyDataSource m_TreeViewDataSource;

    private AnimationWindowState state { get; set; }

    public AddCurvesPopupHierarchy(AnimationWindowState state)
    {
      this.state = state;
    }

    public void OnGUI(Rect position, EditorWindow owner)
    {
      this.InitIfNeeded(owner, position);
      this.m_TreeView.OnEvent();
      this.m_TreeView.OnGUI(position, GUIUtility.GetControlID(FocusType.Keyboard));
    }

    public void InitIfNeeded(EditorWindow owner, Rect rect)
    {
      if (this.m_TreeViewState != null)
        return;
      this.m_TreeViewState = new TreeViewState();
      this.m_TreeView = new TreeView(owner, this.m_TreeViewState);
      this.m_TreeView.deselectOnUnhandledMouseDown = true;
      this.m_TreeViewDataSource = new AddCurvesPopupHierarchyDataSource(this.m_TreeView, this.state);
      TreeViewGUI treeViewGui = (TreeViewGUI) new AddCurvesPopupHierarchyGUI(this.m_TreeView, this.state, owner);
      this.m_TreeView.Init(rect, (ITreeViewDataSource) this.m_TreeViewDataSource, (ITreeViewGUI) treeViewGui, (ITreeViewDragging) null);
      this.m_TreeViewDataSource.UpdateData();
    }

    internal virtual bool IsRenamingNodeAllowed(TreeViewItem node)
    {
      return false;
    }
  }
}
