// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SketchUpTreeViewGUI : TreeViewGUI
  {
    private readonly Texture2D k_Root = EditorGUIUtility.FindTexture("DefaultAsset Icon");
    private readonly Texture2D k_Icon = EditorGUIUtility.FindTexture("Mesh Icon");

    public SketchUpTreeViewGUI(TreeView treeView)
      : base(treeView)
    {
      this.k_BaseIndent = 20f;
    }

    protected override Texture GetIconForNode(TreeViewItem item)
    {
      if (item.children != null && item.children.Count > 0)
        return (Texture) this.k_Root;
      return (Texture) this.k_Icon;
    }

    protected override void RenameEnded()
    {
    }

    protected override void SyncFakeItem()
    {
    }

    public override void OnRowGUI(Rect rowRect, TreeViewItem node, int row, bool selected, bool focused)
    {
      this.DoNodeGUI(rowRect, row, node, selected, focused, false);
      SketchUpNode sketchUpNode = node as SketchUpNode;
      Rect position = new Rect(2f, rowRect.y, rowRect.height, rowRect.height);
      sketchUpNode.Enabled = GUI.Toggle(position, sketchUpNode.Enabled, GUIContent.none, SketchUpImportDlg.Styles.styles.toggleStyle);
    }
  }
}
