// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewTests.TestGUICustomItemHeights
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.TreeViewTests
{
  internal class TestGUICustomItemHeights : TreeViewGUIWithCustomItemsHeights
  {
    private float m_Column1Width = 300f;
    protected Rect m_DraggingInsertionMarkerRect;

    public TestGUICustomItemHeights(TreeView treeView)
      : base(treeView)
    {
    }

    protected override Vector2 GetSizeOfRow(TreeViewItem item)
    {
      return new Vector2(this.m_TreeView.GetTotalRect().width, !item.hasChildren ? 36f : 20f);
    }

    public override void BeginRowGUI()
    {
      this.m_DraggingInsertionMarkerRect.x = -1f;
    }

    public override void EndRowGUI()
    {
      base.EndRowGUI();
      if ((double) this.m_DraggingInsertionMarkerRect.x < 0.0 || Event.current.type != EventType.Repaint)
        return;
      Rect insertionMarkerRect = this.m_DraggingInsertionMarkerRect;
      insertionMarkerRect.height = 2f;
      insertionMarkerRect.y -= insertionMarkerRect.height / 2f;
      if (!this.m_TreeView.dragging.drawRowMarkerAbove)
        insertionMarkerRect.y += this.m_DraggingInsertionMarkerRect.height;
      EditorGUI.DrawRect(insertionMarkerRect, Color.white);
    }

    public override void OnRowGUI(Rect rowRect, TreeViewItem item, int row, bool selected, bool focused)
    {
      --rowRect.height;
      Rect rect1 = rowRect;
      Rect rect2 = rowRect;
      rect1.width = this.m_Column1Width;
      rect1.xMin += this.GetFoldoutIndent(item);
      rect2.xMin += this.m_Column1Width + 1f;
      float foldoutIndent = this.GetFoldoutIndent(item);
      Rect position1 = rowRect;
      int itemControlId = TreeView.GetItemControlID(item);
      bool flag1 = false;
      if (this.m_TreeView.dragging != null)
        flag1 = this.m_TreeView.dragging.GetDropTargetControlID() == itemControlId && this.m_TreeView.data.CanBeParent(item);
      bool flag2 = this.m_TreeView.data.IsExpandable(item);
      Color color1 = new Color(0.0f, 0.22f, 0.44f);
      Color color2 = new Color(0.1f, 0.1f, 0.1f);
      EditorGUI.DrawRect(rect1, !selected ? color2 : color1);
      EditorGUI.DrawRect(rect2, !selected ? color2 : color1);
      if (flag1)
        EditorGUI.DrawRect(new Rect(rowRect.x, rowRect.y, 3f, rowRect.height), Color.yellow);
      if (Event.current.type == EventType.Repaint)
      {
        Rect position2 = rect1;
        position2.xMin += this.m_FoldoutWidth;
        GUI.Label(position2, item.displayName, EditorStyles.largeLabel);
        if ((double) rowRect.height > 20.0)
        {
          position2.y += 16f;
          GUI.Label(position2, "Ut tincidunt tortor. Donec nonummy, enim in lacinia pulvinar", EditorStyles.miniLabel);
        }
        if (this.m_TreeView.dragging != null && this.m_TreeView.dragging.GetRowMarkerControlID() == itemControlId)
          this.m_DraggingInsertionMarkerRect = new Rect(rowRect.x + foldoutIndent, rowRect.y, rowRect.width - foldoutIndent, rowRect.height);
      }
      if (!flag2)
        return;
      position1.x = foldoutIndent;
      position1.width = this.m_FoldoutWidth;
      EditorGUI.BeginChangeCheck();
      bool expand = GUI.Toggle(position1, this.m_TreeView.data.IsExpanded(item), GUIContent.none, TestGUICustomItemHeights.Styles.foldout);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_TreeView.UserInputChangedExpandedState(item, row, expand);
    }

    internal class Styles
    {
      public static GUIStyle foldout = (GUIStyle) "IN Foldout";
    }
  }
}
