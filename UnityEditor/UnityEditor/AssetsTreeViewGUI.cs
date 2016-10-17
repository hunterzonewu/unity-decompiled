// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetsTreeViewGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.ProjectWindowCallback;
using UnityEditor.VersionControl;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor
{
  internal class AssetsTreeViewGUI : TreeViewGUI
  {
    private const float k_IconOverlayPadding = 7f;
    private static bool s_VCEnabled;

    public AssetsTreeViewGUI(TreeView treeView)
      : base(treeView)
    {
      AssetsTreeViewGUI assetsTreeViewGui = this;
      System.Action<TreeViewItem, Rect> action = assetsTreeViewGui.iconOverlayGUI + new System.Action<TreeViewItem, Rect>(this.OnIconOverlayGUI);
      assetsTreeViewGui.iconOverlayGUI = action;
      this.k_TopRowMargin = 4f;
    }

    public override void BeginRowGUI()
    {
      AssetsTreeViewGUI.s_VCEnabled = Provider.isActive;
      float num = !AssetsTreeViewGUI.s_VCEnabled ? 0.0f : 7f;
      this.iconRightPadding = num;
      this.iconLeftPadding = num;
      base.BeginRowGUI();
    }

    protected CreateAssetUtility GetCreateAssetUtility()
    {
      return this.m_TreeView.state.createAssetUtility;
    }

    protected virtual bool IsCreatingNewAsset(int instanceID)
    {
      if (this.GetCreateAssetUtility().IsCreatingNewAsset())
        return this.IsRenaming(instanceID);
      return false;
    }

    protected override void ClearRenameAndNewNodeState()
    {
      this.GetCreateAssetUtility().Clear();
      base.ClearRenameAndNewNodeState();
    }

    protected override void RenameEnded()
    {
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      bool flag = this.GetCreateAssetUtility().IsCreatingNewAsset();
      if (!this.GetRenameOverlay().userAcceptedRename)
        return;
      if (flag)
        this.GetCreateAssetUtility().EndNewAssetCreation(name);
      else
        ObjectNames.SetNameSmartWithInstanceID(userData, name);
    }

    protected override void SyncFakeItem()
    {
      if (!this.m_TreeView.data.HasFakeItem() && this.GetCreateAssetUtility().IsCreatingNewAsset())
        this.m_TreeView.data.InsertFakeItem(this.GetCreateAssetUtility().instanceID, AssetDatabase.GetMainAssetInstanceID(this.GetCreateAssetUtility().folder), this.GetCreateAssetUtility().originalName, this.GetCreateAssetUtility().icon);
      if (!this.m_TreeView.data.HasFakeItem() || this.GetCreateAssetUtility().IsCreatingNewAsset())
        return;
      this.m_TreeView.data.RemoveFakeItem();
    }

    public virtual void BeginCreateNewAsset(int instanceID, EndNameEditAction endAction, string pathName, Texture2D icon, string resourceFile)
    {
      this.ClearRenameAndNewNodeState();
      if (!this.GetCreateAssetUtility().BeginNewAssetCreation(instanceID, endAction, pathName, icon, resourceFile))
        return;
      this.SyncFakeItem();
      if (this.GetRenameOverlay().BeginRename(this.GetCreateAssetUtility().originalName, instanceID, 0.0f))
        return;
      Debug.LogError((object) "Rename not started (when creating new asset)");
    }

    protected override Texture GetIconForNode(TreeViewItem item)
    {
      if (item == null)
        return (Texture) null;
      Texture texture = (Texture) null;
      if (this.IsCreatingNewAsset(item.id))
        texture = (Texture) this.GetCreateAssetUtility().icon;
      if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
        texture = (Texture) item.icon;
      if ((UnityEngine.Object) texture == (UnityEngine.Object) null && item.id != 0)
        texture = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(item.id));
      return texture;
    }

    private void OnIconOverlayGUI(TreeViewItem item, Rect overlayRect)
    {
      if (!AssetsTreeViewGUI.s_VCEnabled || !AssetDatabase.IsMainAsset(item.id))
        return;
      ProjectHooks.OnProjectWindowItem(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(item.id)), overlayRect);
    }
  }
}
