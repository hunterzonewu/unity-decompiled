// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewTests.TreeViewTestWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.TreeViewTests
{
  internal class TreeViewTestWindow : EditorWindow, IHasCustomMenu
  {
    private TreeViewTestWindow.TestType m_TestType = TreeViewTestWindow.TestType.TreeWithCustomItemHeight;
    private BackendData m_BackendData;
    private TreeViewTest m_TreeViewTest;
    private TreeViewTest m_TreeViewTest2;
    private BackendData m_BackendData2;
    private TreeViewTestWithCustomHeight m_TreeViewWithCustomHeight;

    public TreeViewTestWindow()
    {
      this.titleContent = new GUIContent("TreeView Test");
    }

    private void OnEnable()
    {
      this.position = new Rect(100f, 100f, 600f, 600f);
    }

    private void OnGUI()
    {
      switch (this.m_TestType)
      {
        case TreeViewTestWindow.TestType.LargeTreesWithStandardGUI:
          this.TestLargeTreesWithFixedItemHeightAndPingingAndFraming();
          break;
        case TreeViewTestWindow.TestType.TreeWithCustomItemHeight:
          this.TestTreeWithCustomItemHeights();
          break;
      }
    }

    private void TestTreeWithCustomItemHeights()
    {
      Rect rect = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      if (this.m_TreeViewWithCustomHeight == null)
      {
        this.m_BackendData2 = new BackendData();
        this.m_BackendData2.GenerateData(300);
        this.m_TreeViewWithCustomHeight = new TreeViewTestWithCustomHeight((EditorWindow) this, this.m_BackendData2, rect);
      }
      this.m_TreeViewWithCustomHeight.OnGUI(rect);
    }

    private void TestLargeTreesWithFixedItemHeightAndPingingAndFraming()
    {
      Rect rect1 = new Rect(0.0f, 0.0f, this.position.width / 2f, this.position.height);
      Rect rect2 = new Rect(this.position.width / 2f, 0.0f, this.position.width / 2f, this.position.height);
      if (this.m_TreeViewTest == null)
      {
        this.m_BackendData = new BackendData();
        this.m_BackendData.GenerateData(1000000);
        this.m_TreeViewTest = new TreeViewTest((EditorWindow) this, false);
        this.m_TreeViewTest.Init(rect1, this.m_BackendData);
        this.m_TreeViewTest2 = new TreeViewTest((EditorWindow) this, true);
        this.m_TreeViewTest2.Init(rect2, this.m_BackendData);
      }
      this.m_TreeViewTest.OnGUI(rect1);
      this.m_TreeViewTest2.OnGUI(rect2);
      EditorGUI.DrawRect(new Rect(rect1.xMax - 1f, 0.0f, 2f, this.position.height), new Color(0.4f, 0.4f, 0.4f, 0.8f));
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Large TreeView"), this.m_TestType == TreeViewTestWindow.TestType.LargeTreesWithStandardGUI, (GenericMenu.MenuFunction) (() => this.m_TestType = TreeViewTestWindow.TestType.LargeTreesWithStandardGUI));
      menu.AddItem(new GUIContent("Custom Item Height TreeView"), this.m_TestType == TreeViewTestWindow.TestType.TreeWithCustomItemHeight, (GenericMenu.MenuFunction) (() => this.m_TestType = TreeViewTestWindow.TestType.TreeWithCustomItemHeight));
    }

    private enum TestType
    {
      LargeTreesWithStandardGUI,
      TreeWithCustomItemHeight,
    }
  }
}
