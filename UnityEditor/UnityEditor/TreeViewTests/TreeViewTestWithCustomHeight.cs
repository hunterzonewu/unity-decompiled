// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewTests.TreeViewTestWithCustomHeight
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.TreeViewTests
{
  internal class TreeViewTestWithCustomHeight
  {
    private BackendData m_BackendData;
    private TreeView m_TreeView;

    public TreeViewTestWithCustomHeight(EditorWindow editorWindow, BackendData backendData, Rect rect)
    {
      this.m_BackendData = backendData;
      TreeViewState treeViewState = new TreeViewState();
      this.m_TreeView = new TreeView(editorWindow, treeViewState);
      TestGUICustomItemHeights customItemHeights = new TestGUICustomItemHeights(this.m_TreeView);
      TestDragging testDragging = new TestDragging(this.m_TreeView, this.m_BackendData);
      TestDataSource testDataSource1 = new TestDataSource(this.m_TreeView, this.m_BackendData);
      TestDataSource testDataSource2 = testDataSource1;
      System.Action action = testDataSource2.onVisibleRowsChanged + new System.Action(((TreeViewGUIWithCustomItemsHeights) customItemHeights).CalculateRowRects);
      testDataSource2.onVisibleRowsChanged = action;
      this.m_TreeView.Init(rect, (ITreeViewDataSource) testDataSource1, (ITreeViewGUI) customItemHeights, (ITreeViewDragging) testDragging);
      testDataSource1.SetExpanded(testDataSource1.root, true);
    }

    public void OnGUI(Rect rect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard, rect);
      this.m_TreeView.OnGUI(rect, controlId);
    }
  }
}
