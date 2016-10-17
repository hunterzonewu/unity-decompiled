// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioGroupTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioGroupTreeViewGUI : TreeViewGUI
  {
    private readonly float column1Width = 20f;
    private readonly Texture2D k_VisibleON = EditorGUIUtility.FindTexture("VisibilityOn");
    public System.Action<AudioMixerTreeViewNode, bool> NodeWasToggled;
    public AudioMixerController m_Controller;

    public AudioGroupTreeViewGUI(TreeView treeView)
      : base(treeView)
    {
      this.k_BaseIndent = this.column1Width;
      this.k_IconWidth = 0.0f;
      this.k_TopRowMargin = this.k_BottomRowMargin = 2f;
    }

    private void OpenGroupContextMenu(AudioMixerTreeViewNode audioNode, bool visible)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AudioGroupTreeViewGUI.\u003COpenGroupContextMenu\u003Ec__AnonStorey64 menuCAnonStorey64 = new AudioGroupTreeViewGUI.\u003COpenGroupContextMenu\u003Ec__AnonStorey64();
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey64.audioNode = audioNode;
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey64.visible = visible;
      // ISSUE: reference to a compiler-generated field
      menuCAnonStorey64.\u003C\u003Ef__this = this;
      GenericMenu menu = new GenericMenu();
      if (this.NodeWasToggled != null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        menu.AddItem(new GUIContent(!menuCAnonStorey64.visible ? "Show Group" : "Hide group"), false, new GenericMenu.MenuFunction(menuCAnonStorey64.\u003C\u003Em__B2));
      }
      menu.AddSeparator(string.Empty);
      AudioMixerGroupController[] groups;
      // ISSUE: reference to a compiler-generated field
      if (this.m_Controller.CachedSelection.Contains(menuCAnonStorey64.audioNode.group))
      {
        groups = this.m_Controller.CachedSelection.ToArray();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        groups = new AudioMixerGroupController[1]
        {
          menuCAnonStorey64.audioNode.group
        };
      }
      AudioMixerColorCodes.AddColorItemsToGenericMenu(menu, groups);
      menu.ShowAsContext();
    }

    public override void OnRowGUI(Rect rowRect, TreeViewItem node, int row, bool selected, bool focused)
    {
      Event current = Event.current;
      this.DoNodeGUI(rowRect, row, node, selected, focused, false);
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      AudioMixerTreeViewNode audioNode = node as AudioMixerTreeViewNode;
      if (audioNode == null)
        return;
      bool visible = this.m_Controller.CurrentViewContainsGroup(audioNode.group.groupID);
      float num = 3f;
      Rect position = new Rect(rowRect.x + num, rowRect.y, 16f, 16f);
      Rect rect1 = new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f);
      int userColorIndex = audioNode.group.userColorIndex;
      if (userColorIndex > 0)
        EditorGUI.DrawRect(new Rect(rowRect.x, rect1.y, 2f, rect1.height), AudioMixerColorCodes.GetColor(userColorIndex));
      EditorGUI.DrawRect(rect1, new Color(0.5f, 0.5f, 0.5f, 0.2f));
      if (visible)
        GUI.DrawTexture(position, (Texture) this.k_VisibleON);
      Rect rect2 = new Rect(2f, rowRect.y, rowRect.height, rowRect.height);
      if (current.type == EventType.MouseUp && current.button == 0 && (rect2.Contains(current.mousePosition) && this.NodeWasToggled != null))
        this.NodeWasToggled(audioNode, !visible);
      if (current.type != EventType.ContextClick || !position.Contains(current.mousePosition))
        return;
      this.OpenGroupContextMenu(audioNode, visible);
      current.Use();
    }

    protected override Texture GetIconForNode(TreeViewItem node)
    {
      if (node != null && (UnityEngine.Object) node.icon != (UnityEngine.Object) null)
        return (Texture) node.icon;
      return (Texture) null;
    }

    protected override void SyncFakeItem()
    {
    }

    protected override void RenameEnded()
    {
      if (!this.GetRenameOverlay().userAcceptedRename)
        return;
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      AudioMixerTreeViewNode node = this.m_TreeView.FindNode(userData) as AudioMixerTreeViewNode;
      if (node == null)
        return;
      ObjectNames.SetNameSmartWithInstanceID(userData, name);
      foreach (AudioMixerEffectController effect in node.group.effects)
        effect.ClearCachedDisplayName();
      this.m_TreeView.ReloadData();
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null))
        return;
      this.m_Controller.OnSubAssetChanged();
    }
  }
}
