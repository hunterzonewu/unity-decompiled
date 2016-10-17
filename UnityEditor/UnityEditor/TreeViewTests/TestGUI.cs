// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewTests.TestGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.TreeViewTests
{
  internal class TestGUI : TreeViewGUI
  {
    private Texture2D m_FolderIcon = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
    private Texture2D m_Icon = EditorGUIUtility.FindTexture("boo Script Icon");
    private GUIStyle m_LabelStyle;
    private GUIStyle m_LabelStyleRightAlign;

    private float[] columnWidths
    {
      get
      {
        return this.m_TreeView.state.columnWidths;
      }
    }

    public TestGUI(TreeView treeView)
      : base(treeView)
    {
    }

    protected override Texture GetIconForNode(TreeViewItem item)
    {
      if (item.hasChildren)
        return (Texture) this.m_FolderIcon;
      return (Texture) this.m_Icon;
    }

    protected override void RenameEnded()
    {
    }

    protected override void SyncFakeItem()
    {
    }

    protected override void DrawIconAndLabel(Rect rect, TreeViewItem item, string label, bool selected, bool focused, bool useBoldFont, bool isPinging)
    {
      if (this.m_LabelStyle == null)
      {
        this.m_LabelStyle = new GUIStyle(TreeViewGUI.s_Styles.lineStyle);
        RectOffset padding1 = this.m_LabelStyle.padding;
        int num1 = 6;
        this.m_LabelStyle.padding.right = num1;
        int num2 = num1;
        padding1.left = num2;
        this.m_LabelStyleRightAlign = new GUIStyle(TreeViewGUI.s_Styles.lineStyle);
        RectOffset padding2 = this.m_LabelStyleRightAlign.padding;
        int num3 = 6;
        this.m_LabelStyleRightAlign.padding.left = num3;
        int num4 = num3;
        padding2.right = num4;
        this.m_LabelStyleRightAlign.alignment = TextAnchor.MiddleRight;
      }
      if (isPinging || this.columnWidths == null || this.columnWidths.Length == 0)
      {
        base.DrawIconAndLabel(rect, item, label, selected, focused, useBoldFont, isPinging);
      }
      else
      {
        Rect rect1 = rect;
        for (int index = 0; index < this.columnWidths.Length; ++index)
        {
          rect1.width = this.columnWidths[index];
          if (index == 0)
            base.DrawIconAndLabel(rect1, item, label, selected, focused, useBoldFont, isPinging);
          else
            GUI.Label(rect1, "Zksdf SDFS DFASDF ", index % 2 != 0 ? this.m_LabelStyleRightAlign : this.m_LabelStyle);
          rect1.x += rect1.width;
        }
      }
    }
  }
}
