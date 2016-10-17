// Decompiled with JetBrains decompiler
// Type: UnityEditor.PackageImportTreeView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class PackageImportTreeView
  {
    private static readonly bool s_UseFoldouts = true;
    private List<PackageImportTreeView.PackageImportTreeViewItem> m_Selection = new List<PackageImportTreeView.PackageImportTreeViewItem>();
    private TreeView m_TreeView;
    private PackageImport m_PackageImport;

    public bool canReInstall
    {
      get
      {
        return this.m_PackageImport.canReInstall;
      }
    }

    public bool doReInstall
    {
      get
      {
        return this.m_PackageImport.doReInstall;
      }
    }

    public ImportPackageItem[] packageItems
    {
      get
      {
        return this.m_PackageImport.packageItems;
      }
    }

    public PackageImportTreeView(PackageImport packageImport, TreeViewState treeViewState, Rect startRect)
    {
      this.m_PackageImport = packageImport;
      this.m_TreeView = new TreeView((EditorWindow) this.m_PackageImport, treeViewState);
      PackageImportTreeView.PackageImportTreeViewDataSource treeViewDataSource = new PackageImportTreeView.PackageImportTreeViewDataSource(this.m_TreeView, this);
      PackageImportTreeView.PackageImportTreeViewGUI importTreeViewGui = new PackageImportTreeView.PackageImportTreeViewGUI(this.m_TreeView, this);
      this.m_TreeView.Init(startRect, (ITreeViewDataSource) treeViewDataSource, (ITreeViewGUI) importTreeViewGui, (ITreeViewDragging) null);
      this.m_TreeView.ReloadData();
      this.m_TreeView.selectionChangedCallback += new System.Action<int[]>(this.SelectionChanged);
      importTreeViewGui.itemWasToggled += new System.Action<PackageImportTreeView.PackageImportTreeViewItem>(this.ItemWasToggled);
      this.ComputeEnabledStateForFolders();
    }

    private void ComputeEnabledStateForFolders()
    {
      PackageImportTreeView.PackageImportTreeViewItem root = this.m_TreeView.data.root as PackageImportTreeView.PackageImportTreeViewItem;
      this.RecursiveComputeEnabledStateForFolders(root, new HashSet<PackageImportTreeView.PackageImportTreeViewItem>()
      {
        root
      });
    }

    private void RecursiveComputeEnabledStateForFolders(PackageImportTreeView.PackageImportTreeViewItem pitem, HashSet<PackageImportTreeView.PackageImportTreeViewItem> done)
    {
      if (pitem.item != null && !pitem.item.isFolder)
        return;
      if (pitem.hasChildren)
      {
        using (List<TreeViewItem>.Enumerator enumerator = pitem.children.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.RecursiveComputeEnabledStateForFolders(enumerator.Current as PackageImportTreeView.PackageImportTreeViewItem, done);
        }
      }
      if (done.Contains(pitem))
        return;
      PackageImportTreeView.EnabledState childrenEnabledState = this.GetFolderChildrenEnabledState(pitem);
      pitem.enableState = childrenEnabledState;
      if (childrenEnabledState != PackageImportTreeView.EnabledState.Mixed)
        return;
      done.Add(pitem);
      for (PackageImportTreeView.PackageImportTreeViewItem parent = pitem.parent as PackageImportTreeView.PackageImportTreeViewItem; parent != null; parent = parent.parent as PackageImportTreeView.PackageImportTreeViewItem)
      {
        if (!done.Contains(parent))
        {
          parent.enableState = PackageImportTreeView.EnabledState.Mixed;
          done.Add(parent);
        }
      }
    }

    private bool ItemShouldBeConsideredForEnabledCheck(PackageImportTreeView.PackageImportTreeViewItem pitem)
    {
      if (pitem == null)
        return false;
      if (pitem.item == null)
        return true;
      ImportPackageItem importPackageItem = pitem.item;
      return !importPackageItem.projectAsset && (importPackageItem.isFolder || importPackageItem.assetChanged || this.doReInstall);
    }

    private PackageImportTreeView.EnabledState GetFolderChildrenEnabledState(PackageImportTreeView.PackageImportTreeViewItem folder)
    {
      if (folder.item != null && !folder.item.isFolder)
        Debug.LogError((object) "Should be a folder item!");
      if (!folder.hasChildren)
        return PackageImportTreeView.EnabledState.None;
      PackageImportTreeView.EnabledState enabledState = PackageImportTreeView.EnabledState.NotSet;
      int index1;
      for (index1 = 0; index1 < folder.children.Count; ++index1)
      {
        PackageImportTreeView.PackageImportTreeViewItem child = folder.children[index1] as PackageImportTreeView.PackageImportTreeViewItem;
        if (this.ItemShouldBeConsideredForEnabledCheck(child))
        {
          enabledState = child.enableState;
          break;
        }
      }
      for (int index2 = index1 + 1; index2 < folder.children.Count; ++index2)
      {
        PackageImportTreeView.PackageImportTreeViewItem child = folder.children[index2] as PackageImportTreeView.PackageImportTreeViewItem;
        if (this.ItemShouldBeConsideredForEnabledCheck(child) && enabledState != child.enableState)
        {
          enabledState = PackageImportTreeView.EnabledState.Mixed;
          break;
        }
      }
      if (enabledState == PackageImportTreeView.EnabledState.NotSet)
        return PackageImportTreeView.EnabledState.None;
      return enabledState;
    }

    private void SelectionChanged(int[] selectedIDs)
    {
      this.m_Selection = new List<PackageImportTreeView.PackageImportTreeViewItem>();
      using (List<TreeViewItem>.Enumerator enumerator = this.m_TreeView.data.GetRows().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TreeViewItem current = enumerator.Current;
          if (((IEnumerable<int>) selectedIDs).Contains<int>(current.id))
          {
            PackageImportTreeView.PackageImportTreeViewItem importTreeViewItem = current as PackageImportTreeView.PackageImportTreeViewItem;
            if (importTreeViewItem != null)
              this.m_Selection.Add(importTreeViewItem);
          }
        }
      }
      ImportPackageItem importPackageItem = this.m_Selection[0].item;
      if (this.m_Selection.Count == 1 && importPackageItem != null && !string.IsNullOrEmpty(importPackageItem.previewPath))
        (this.m_TreeView.gui as PackageImportTreeView.PackageImportTreeViewGUI).showPreviewForID = this.m_Selection[0].id;
      else
        PopupWindowWithoutFocus.Hide();
    }

    public void OnGUI(Rect rect)
    {
      if (Event.current.type == EventType.ScrollWheel)
        PopupWindowWithoutFocus.Hide();
      int controlId = GUIUtility.GetControlID(FocusType.Keyboard);
      this.m_TreeView.OnGUI(rect, controlId);
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Space || (this.m_Selection == null || this.m_Selection.Count <= 0) || GUIUtility.keyboardControl != controlId)
        return;
      PackageImportTreeView.PackageImportTreeViewItem importTreeViewItem = this.m_Selection[0];
      if (importTreeViewItem != null)
      {
        PackageImportTreeView.EnabledState enabledState = importTreeViewItem.enableState != PackageImportTreeView.EnabledState.None ? PackageImportTreeView.EnabledState.None : PackageImportTreeView.EnabledState.All;
        importTreeViewItem.enableState = enabledState;
        this.ItemWasToggled(this.m_Selection[0]);
      }
      Event.current.Use();
    }

    public void SetAllEnabled(PackageImportTreeView.EnabledState state)
    {
      this.EnableChildrenRecursive(this.m_TreeView.data.root, state);
      this.ComputeEnabledStateForFolders();
    }

    private void ItemWasToggled(PackageImportTreeView.PackageImportTreeViewItem pitem)
    {
      if (this.m_Selection.Count <= 1)
      {
        this.EnableChildrenRecursive((TreeViewItem) pitem, pitem.enableState);
      }
      else
      {
        using (List<PackageImportTreeView.PackageImportTreeViewItem>.Enumerator enumerator = this.m_Selection.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.enableState = pitem.enableState;
        }
      }
      this.ComputeEnabledStateForFolders();
    }

    private void EnableChildrenRecursive(TreeViewItem parentItem, PackageImportTreeView.EnabledState state)
    {
      if (!parentItem.hasChildren)
        return;
      using (List<TreeViewItem>.Enumerator enumerator = parentItem.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          PackageImportTreeView.PackageImportTreeViewItem current = enumerator.Current as PackageImportTreeView.PackageImportTreeViewItem;
          current.enableState = state;
          this.EnableChildrenRecursive((TreeViewItem) current, state);
        }
      }
    }

    public enum EnabledState
    {
      NotSet = -1,
      None = 0,
      All = 1,
      Mixed = 2,
    }

    private class PackageImportTreeViewItem : TreeViewItem
    {
      private PackageImportTreeView.EnabledState m_EnableState;

      public ImportPackageItem item { get; set; }

      public PackageImportTreeView.EnabledState enableState
      {
        get
        {
          return this.m_EnableState;
        }
        set
        {
          if (this.item != null && this.item.projectAsset)
            return;
          this.m_EnableState = value;
          if (this.item == null)
            return;
          this.item.enabledStatus = (int) value;
        }
      }

      public PackageImportTreeViewItem(ImportPackageItem itemIn, int id, int depth, TreeViewItem parent, string displayName)
        : base(id, depth, parent, displayName)
      {
        this.item = itemIn;
        if (this.item == null)
          this.m_EnableState = PackageImportTreeView.EnabledState.All;
        else
          this.m_EnableState = (PackageImportTreeView.EnabledState) this.item.enabledStatus;
      }
    }

    private class PackageImportTreeViewGUI : TreeViewGUI
    {
      public System.Action<PackageImportTreeView.PackageImportTreeViewItem> itemWasToggled;
      private PackageImportTreeView m_PackageImportView;

      public int showPreviewForID { get; set; }

      public PackageImportTreeViewGUI(TreeView treeView, PackageImportTreeView view)
        : base(treeView)
      {
        this.m_PackageImportView = view;
        this.k_BaseIndent = 4f;
        if (PackageImportTreeView.s_UseFoldouts)
          return;
        this.k_FoldoutWidth = 0.0f;
      }

      public override void OnRowGUI(Rect rowRect, TreeViewItem tvItem, int row, bool selected, bool focused)
      {
        this.k_IndentWidth = 18f;
        this.k_FoldoutWidth = 18f;
        PackageImportTreeView.PackageImportTreeViewItem pitem = tvItem as PackageImportTreeView.PackageImportTreeViewItem;
        ImportPackageItem importPackageItem = pitem.item;
        bool flag1 = Event.current.type == EventType.Repaint;
        if (selected && flag1)
          TreeViewGUI.s_Styles.selectionStyle.Draw(rowRect, false, false, true, focused);
        bool flag2 = importPackageItem != null;
        bool flag3 = importPackageItem == null || importPackageItem.isFolder;
        bool flag4 = importPackageItem != null && importPackageItem.assetChanged;
        bool flag5 = importPackageItem != null && importPackageItem.pathConflict;
        bool flag6 = importPackageItem == null || importPackageItem.exists;
        bool flag7 = importPackageItem != null && importPackageItem.projectAsset;
        bool doReInstall = this.m_PackageImportView.doReInstall;
        if (this.m_TreeView.data.IsExpandable(tvItem))
          this.DoFoldout(rowRect, tvItem, row);
        Rect toggleRect = new Rect(this.k_BaseIndent + (float) tvItem.depth * this.indentWidth + this.k_FoldoutWidth, rowRect.y, 18f, rowRect.height);
        if (flag3 && !flag7 || flag2 && !flag7 && (flag4 || doReInstall))
          this.DoToggle(pitem, toggleRect);
        EditorGUI.BeginDisabledGroup(!flag2 || flag7);
        Rect contentRect = new Rect(toggleRect.xMax, rowRect.y, rowRect.width, rowRect.height);
        this.DoIconAndText(tvItem, contentRect, selected, focused);
        this.DoPreviewPopup(pitem, rowRect);
        if (flag1 && flag2 && flag5)
        {
          Rect position = new Rect(rowRect.xMax - 58f, rowRect.y, rowRect.height, rowRect.height);
          EditorGUIUtility.SetIconSize(new Vector2(rowRect.height, rowRect.height));
          GUI.Label(position, PackageImportTreeView.PackageImportTreeViewGUI.Constants.badgeWarn);
          EditorGUIUtility.SetIconSize(Vector2.zero);
        }
        if (flag1 && flag2 && (!flag6 && !flag5))
        {
          Texture image = PackageImportTreeView.PackageImportTreeViewGUI.Constants.badgeNew.image;
          GUI.Label(new Rect((float) ((double) rowRect.xMax - (double) image.width - 6.0), rowRect.y + (float) (((double) rowRect.height - (double) image.height) / 2.0), (float) image.width, (float) image.height), PackageImportTreeView.PackageImportTreeViewGUI.Constants.badgeNew, PackageImportTreeView.PackageImportTreeViewGUI.Constants.paddinglessStyle);
        }
        if (flag1 && doReInstall && flag7)
        {
          Texture image = PackageImportTreeView.PackageImportTreeViewGUI.Constants.badgeDelete.image;
          GUI.Label(new Rect((float) ((double) rowRect.xMax - (double) image.width - 6.0), rowRect.y + (float) (((double) rowRect.height - (double) image.height) / 2.0), (float) image.width, (float) image.height), PackageImportTreeView.PackageImportTreeViewGUI.Constants.badgeDelete, PackageImportTreeView.PackageImportTreeViewGUI.Constants.paddinglessStyle);
        }
        if (flag1 && flag2 && (flag6 || flag5) && flag4)
        {
          Texture image = PackageImportTreeView.PackageImportTreeViewGUI.Constants.badgeChange.image;
          GUI.Label(new Rect((float) ((double) rowRect.xMax - (double) image.width - 6.0), rowRect.y, rowRect.height, rowRect.height), PackageImportTreeView.PackageImportTreeViewGUI.Constants.badgeChange, PackageImportTreeView.PackageImportTreeViewGUI.Constants.paddinglessStyle);
        }
        EditorGUI.EndDisabledGroup();
      }

      private static void Toggle(ImportPackageItem[] items, PackageImportTreeView.PackageImportTreeViewItem pitem, Rect toggleRect)
      {
        bool flag1 = pitem.enableState > PackageImportTreeView.EnabledState.None;
        bool flag2 = pitem.item == null || pitem.item.isFolder;
        GUIStyle style = EditorStyles.toggle;
        if (flag2 && pitem.enableState == PackageImportTreeView.EnabledState.Mixed)
          style = EditorStyles.toggleMixed;
        bool flag3 = GUI.Toggle(toggleRect, flag1, GUIContent.none, style);
        if (flag3 == flag1)
          return;
        pitem.enableState = !flag3 ? PackageImportTreeView.EnabledState.None : PackageImportTreeView.EnabledState.All;
      }

      private void DoToggle(PackageImportTreeView.PackageImportTreeViewItem pitem, Rect toggleRect)
      {
        EditorGUI.BeginChangeCheck();
        PackageImportTreeView.PackageImportTreeViewGUI.Toggle(this.m_PackageImportView.packageItems, pitem, toggleRect);
        if (!EditorGUI.EndChangeCheck())
          return;
        if (this.m_TreeView.GetSelection().Length <= 1 || !((IEnumerable<int>) this.m_TreeView.GetSelection()).Contains<int>(pitem.id))
        {
          this.m_TreeView.SetSelection(new int[1]{ pitem.id }, 0 != 0);
          this.m_TreeView.NotifyListenersThatSelectionChanged();
        }
        if (this.itemWasToggled != null)
          this.itemWasToggled(pitem);
        Event.current.Use();
      }

      private void DoPreviewPopup(PackageImportTreeView.PackageImportTreeViewItem pitem, Rect rowRect)
      {
        ImportPackageItem importPackageItem = pitem.item;
        if (importPackageItem == null)
          return;
        if (Event.current.type == EventType.MouseDown && rowRect.Contains(Event.current.mousePosition) && !PopupWindowWithoutFocus.IsVisible())
          this.showPreviewForID = pitem.id;
        if (pitem.id != this.showPreviewForID || Event.current.type == EventType.Layout)
          return;
        this.showPreviewForID = 0;
        if (string.IsNullOrEmpty(importPackageItem.previewPath))
          return;
        Texture2D preview = PackageImport.GetPreview(importPackageItem.previewPath);
        Rect activatorRect = rowRect;
        activatorRect.width = EditorGUIUtility.currentViewWidth;
        PopupWindowWithoutFocus.Show(activatorRect, (PopupWindowContent) new PackageImportTreeView.PreviewPopup(preview), new PopupLocationHelper.PopupLocation[3]
        {
          PopupLocationHelper.PopupLocation.Right,
          PopupLocationHelper.PopupLocation.Left,
          PopupLocationHelper.PopupLocation.Below
        });
      }

      private void DoIconAndText(TreeViewItem item, Rect contentRect, bool selected, bool focused)
      {
        EditorGUIUtility.SetIconSize(new Vector2(this.k_IconWidth, this.k_IconWidth));
        GUIStyle lineStyle = TreeViewGUI.s_Styles.lineStyle;
        lineStyle.padding.left = 0;
        if (Event.current.type == EventType.Repaint)
          lineStyle.Draw(contentRect, GUIContent.Temp(item.displayName, this.GetIconForNode(item)), false, false, selected, focused);
        EditorGUIUtility.SetIconSize(Vector2.zero);
      }

      protected override Texture GetIconForNode(TreeViewItem tvItem)
      {
        ImportPackageItem importPackageItem = (tvItem as PackageImportTreeView.PackageImportTreeViewItem).item;
        if (importPackageItem == null || importPackageItem.isFolder)
          return (Texture) PackageImportTreeView.PackageImportTreeViewGUI.Constants.folderIcon;
        Texture cachedIcon = AssetDatabase.GetCachedIcon(importPackageItem.destinationAssetPath);
        if ((UnityEngine.Object) cachedIcon != (UnityEngine.Object) null)
          return cachedIcon;
        return (Texture) InternalEditorUtility.GetIconForFile(importPackageItem.destinationAssetPath);
      }

      protected override void RenameEnded()
      {
      }

      internal static class Constants
      {
        public static Texture2D folderIcon = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
        public static GUIContent badgeNew = EditorGUIUtility.IconContent("AS Badge New", "|This is a new Asset");
        public static GUIContent badgeDelete = EditorGUIUtility.IconContent("AS Badge Delete", "|These files will be deleted!");
        public static GUIContent badgeWarn = EditorGUIUtility.IconContent("console.warnicon", "|Warning: File exists in project, but with different GUID. Will override existing asset which may be undesired.");
        public static GUIContent badgeChange = EditorGUIUtility.IconContent("playLoopOff", "|This file is new or has changed.");
        public static GUIStyle paddinglessStyle = new GUIStyle();

        static Constants()
        {
          PackageImportTreeView.PackageImportTreeViewGUI.Constants.paddinglessStyle.padding = new RectOffset(0, 0, 0, 0);
        }
      }
    }

    private class PackageImportTreeViewDataSource : TreeViewDataSource
    {
      private PackageImportTreeView m_PackageImportView;

      public PackageImportTreeViewDataSource(TreeView treeView, PackageImportTreeView view)
        : base(treeView)
      {
        this.m_PackageImportView = view;
        this.rootIsCollapsable = false;
        this.showRootNode = false;
      }

      public override bool IsRenamingItemAllowed(TreeViewItem item)
      {
        return false;
      }

      public override bool IsExpandable(TreeViewItem item)
      {
        if (!PackageImportTreeView.s_UseFoldouts)
          return false;
        return base.IsExpandable(item);
      }

      public override void FetchData()
      {
        this.m_RootItem = (TreeViewItem) new PackageImportTreeView.PackageImportTreeViewItem((ImportPackageItem) null, "Assets".GetHashCode(), -1, (TreeViewItem) null, "InvisibleAssetsFolder");
        bool initExpandedState = true;
        if (initExpandedState)
          this.m_TreeView.state.expandedIDs.Add(this.m_RootItem.id);
        ImportPackageItem[] packageItems = this.m_PackageImportView.packageItems;
        Dictionary<string, PackageImportTreeView.PackageImportTreeViewItem> treeViewFolders = new Dictionary<string, PackageImportTreeView.PackageImportTreeViewItem>();
        for (int index = 0; index < packageItems.Length; ++index)
        {
          ImportPackageItem itemIn = packageItems[index];
          if (!PackageImport.HasInvalidCharInFilePath(itemIn.destinationAssetPath))
          {
            string fileName = Path.GetFileName(itemIn.destinationAssetPath);
            TreeViewItem parent = this.EnsureFolderPath(Path.GetDirectoryName(itemIn.destinationAssetPath), treeViewFolders, initExpandedState);
            if (parent != null)
            {
              int hashCode = itemIn.destinationAssetPath.GetHashCode();
              PackageImportTreeView.PackageImportTreeViewItem importTreeViewItem = new PackageImportTreeView.PackageImportTreeViewItem(itemIn, hashCode, parent.depth + 1, parent, fileName);
              parent.AddChild((TreeViewItem) importTreeViewItem);
              if (initExpandedState)
                this.m_TreeView.state.expandedIDs.Add(hashCode);
              if (itemIn.isFolder)
                treeViewFolders[itemIn.destinationAssetPath] = importTreeViewItem;
            }
          }
        }
        if (!initExpandedState)
          return;
        this.m_TreeView.state.expandedIDs.Sort();
      }

      private TreeViewItem EnsureFolderPath(string folderPath, Dictionary<string, PackageImportTreeView.PackageImportTreeViewItem> treeViewFolders, bool initExpandedState)
      {
        if (folderPath == string.Empty)
          return this.m_RootItem;
        TreeViewItem treeViewItem = TreeViewUtility.FindItem(folderPath.GetHashCode(), this.m_RootItem);
        if (treeViewItem != null)
          return treeViewItem;
        string[] strArray = folderPath.Split('/');
        string empty = string.Empty;
        TreeViewItem parent = this.m_RootItem;
        int depth = -1;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string displayName = strArray[index];
          if (empty != string.Empty)
            empty += (string) (object) '/';
          empty += displayName;
          if (index != 0 || !(empty == "Assets"))
          {
            ++depth;
            int hashCode = empty.GetHashCode();
            PackageImportTreeView.PackageImportTreeViewItem importTreeViewItem1;
            if (treeViewFolders.TryGetValue(empty, out importTreeViewItem1))
            {
              parent = (TreeViewItem) importTreeViewItem1;
            }
            else
            {
              PackageImportTreeView.PackageImportTreeViewItem importTreeViewItem2 = new PackageImportTreeView.PackageImportTreeViewItem((ImportPackageItem) null, hashCode, depth, parent, displayName);
              parent.AddChild((TreeViewItem) importTreeViewItem2);
              parent = (TreeViewItem) importTreeViewItem2;
              if (initExpandedState)
                this.m_TreeView.state.expandedIDs.Add(hashCode);
              treeViewFolders[empty] = importTreeViewItem2;
            }
          }
        }
        return parent;
      }
    }

    private class PreviewPopup : PopupWindowContent
    {
      private readonly Vector2 kPreviewSize = new Vector2(128f, 128f);
      private readonly Texture2D m_Preview;

      public PreviewPopup(Texture2D preview)
      {
        this.m_Preview = preview;
      }

      public override void OnGUI(Rect rect)
      {
        PackageImport.DrawTexture(rect, this.m_Preview, false);
      }

      public override Vector2 GetWindowSize()
      {
        return this.kPreviewSize;
      }
    }
  }
}
