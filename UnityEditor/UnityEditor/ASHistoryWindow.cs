// Decompiled with JetBrains decompiler
// Type: UnityEditor.ASHistoryWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class ASHistoryWindow
  {
    private static ASHistoryWindow.Constants ms_Style = (ASHistoryWindow.Constants) null;
    private static int ms_HistoryControlHash = "HistoryControl".GetHashCode();
    private static Vector2 ms_IconSize = new Vector2(16f, 16f);
    private static GUIContent emptyGUIContent = new GUIContent();
    private SplitterState m_HorSplit = new SplitterState(new float[2]{ 30f, 70f }, new int[2]{ 60, 100 }, (int[]) null);
    private Vector2 m_ScrollPos = Vector2.zero;
    private int m_RowHeight = 16;
    private int m_HistoryControlID = -1;
    private int m_ChangesetSelectionIndex = -1;
    private int m_AssetSelectionIndex = -1;
    private int m_ChangeLogSelectionRev = -1;
    private int m_Rev1ForCustomDiff = -1;
    private string m_ChangeLogSelectionGUID = string.Empty;
    private string m_ChangeLogSelectionAssetName = string.Empty;
    private string m_SelectedPath = string.Empty;
    private string m_SelectedGUID = string.Empty;
    private GUIContent[] m_DropDownMenuItems = new GUIContent[9]
    {
      EditorGUIUtility.TextContent("Show History"),
      ASHistoryWindow.emptyGUIContent,
      EditorGUIUtility.TextContent("Compare to Local"),
      EditorGUIUtility.TextContent("Compare Binary to Local"),
      ASHistoryWindow.emptyGUIContent,
      EditorGUIUtility.TextContent("Compare to Another Revision"),
      EditorGUIUtility.TextContent("Compare Binary to Another Revision"),
      ASHistoryWindow.emptyGUIContent,
      EditorGUIUtility.TextContent("Download This File")
    };
    private GUIContent[] m_DropDownChangesetMenuItems = new GUIContent[1]
    {
      EditorGUIUtility.TextContent("Revert Entire Project to This Changeset")
    };
    private ASHistoryFileView m_FileViewWin = new ASHistoryFileView();
    private const int kFirst = -999999;
    private const int kLast = 999999;
    private const int kUncollapsedItemsCount = 5;
    private bool m_NextSelectionMine;
    private ASHistoryWindow.GUIHistoryListItem[] m_GUIItems;
    private int m_TotalHeight;
    private bool m_SplittersOk;
    private bool m_BinaryDiff;
    private int m_ScrollViewHeight;
    private bool m_FolderSelected;
    private bool m_InRevisionSelectMode;
    private EditorWindow m_ParentWindow;
    private Changeset[] m_Changesets;

    private int ChangeLogSelectionRev
    {
      get
      {
        return this.m_ChangeLogSelectionRev;
      }
      set
      {
        this.m_ChangeLogSelectionRev = value;
        if (!this.m_InRevisionSelectMode)
          return;
        this.FinishShowCustomDiff();
      }
    }

    public ASHistoryWindow(EditorWindow parent)
    {
      this.m_ParentWindow = parent;
      ASEditorBackend.SettingsIfNeeded();
      if (Selection.objects.Length == 0)
        return;
      this.m_FileViewWin.SelType = ASHistoryFileView.SelectionType.Items;
    }

    private void ContextMenuClick(object userData, string[] options, int selected)
    {
      switch (selected)
      {
        case 0:
          this.ShowAssetsHistory();
          break;
        case 2:
          this.DoShowDiff(false, this.ChangeLogSelectionRev, -1);
          break;
        case 3:
          this.DoShowDiff(true, this.ChangeLogSelectionRev, -1);
          break;
        case 5:
          this.DoShowCustomDiff(false);
          break;
        case 6:
          this.DoShowCustomDiff(true);
          break;
        case 8:
          this.DownloadFile();
          break;
      }
    }

    private void DownloadFile()
    {
      if (this.ChangeLogSelectionRev < 0 || this.m_ChangeLogSelectionGUID == string.Empty)
        return;
      if (!EditorUtility.DisplayDialog("Download file", "Are you sure you want to download '" + this.m_ChangeLogSelectionAssetName + "' from revision " + this.ChangeLogSelectionRev.ToString() + " and lose all changes?", "Download", "Cancel"))
        return;
      AssetServer.DoRevertOnNextTick(this.ChangeLogSelectionRev, this.m_ChangeLogSelectionGUID);
    }

    private void ShowAssetsHistory()
    {
      if (AssetServer.IsAssetAvailable(this.m_ChangeLogSelectionGUID) != 0)
      {
        string[] guids = new string[1]
        {
          this.m_ChangeLogSelectionGUID
        };
        this.m_FileViewWin.SelType = ASHistoryFileView.SelectionType.Items;
        AssetServer.SetSelectionFromGUIDs(guids);
      }
      else
      {
        this.m_FileViewWin.SelectDeletedItem(this.m_ChangeLogSelectionGUID);
        this.DoLocalSelectionChange();
      }
    }

    private void ChangesetContextMenuClick(object userData, string[] options, int selected)
    {
      if (selected < 0 || selected != 0)
        return;
      this.DoRevertProject();
    }

    private void DoRevertProject()
    {
      if (this.ChangeLogSelectionRev <= 0)
        return;
      ASEditorBackend.ASWin.RevertProject(this.ChangeLogSelectionRev, this.m_Changesets);
    }

    private int MarkBoldItemsBySelection(ASHistoryWindow.GUIHistoryListItem item)
    {
      List<string> stringList = new List<string>();
      ParentViewState assets = item.assets;
      int num = -1;
      int index1 = 0;
      if (Selection.instanceIDs.Length == 0)
        return 0;
      foreach (int instanceId in Selection.instanceIDs)
        stringList.Add(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(instanceId)));
      for (int index2 = 0; index2 < assets.folders.Length; ++index2)
      {
        ParentViewFolder folder = assets.folders[index2];
        if (stringList.Contains(folder.guid))
        {
          item.boldAssets[index1] = true;
          if (num == -1)
            num = index1;
        }
        ++index1;
        for (int index3 = 0; index3 < folder.files.Length; ++index3)
        {
          if (stringList.Contains(folder.files[index3].guid))
          {
            item.boldAssets[index1] = true;
            if (num == -1)
              num = index1;
          }
          ++index1;
        }
      }
      return num;
    }

    private int CheckParentViewInFilterAndMarkBoldItems(ASHistoryWindow.GUIHistoryListItem item, string text)
    {
      ParentViewState assets = item.assets;
      int num = -1;
      int index1 = 0;
      for (int index2 = 0; index2 < assets.folders.Length; ++index2)
      {
        ParentViewFolder folder = assets.folders[index2];
        if (folder.name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) != -1)
        {
          item.boldAssets[index1] = true;
          if (num == -1)
            num = index1;
        }
        ++index1;
        for (int index3 = 0; index3 < folder.files.Length; ++index3)
        {
          if (folder.files[index3].name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) != -1)
          {
            item.boldAssets[index1] = true;
            if (num == -1)
              num = index1;
          }
          ++index1;
        }
      }
      return num;
    }

    private void MarkBoldItemsByGUID(string guid)
    {
      for (int index1 = 0; index1 < this.m_GUIItems.Length; ++index1)
      {
        ASHistoryWindow.GUIHistoryListItem guiItem = this.m_GUIItems[index1];
        ParentViewState assets = guiItem.assets;
        int index2 = 0;
        guiItem.boldAssets = new bool[assets.GetLineCount()];
        for (int index3 = 0; index3 < assets.folders.Length; ++index3)
        {
          ParentViewFolder folder = assets.folders[index3];
          if (folder.guid == guid)
            guiItem.boldAssets[index2] = true;
          ++index2;
          for (int index4 = 0; index4 < folder.files.Length; ++index4)
          {
            if (folder.files[index4].guid == guid)
              guiItem.boldAssets[index2] = true;
            ++index2;
          }
        }
      }
    }

    public void FilterItems(bool recreateGUIItems)
    {
      this.m_TotalHeight = 0;
      if (this.m_Changesets == null || this.m_Changesets.Length == 0)
      {
        this.m_GUIItems = (ASHistoryWindow.GUIHistoryListItem[]) null;
      }
      else
      {
        if (recreateGUIItems)
          this.m_GUIItems = new ASHistoryWindow.GUIHistoryListItem[this.m_Changesets.Length];
        string filterText = ((ASMainWindow) this.m_ParentWindow).m_SearchField.FilterText;
        bool flag = filterText.Trim() == string.Empty;
        for (int index = 0; index < this.m_Changesets.Length; ++index)
        {
          if (recreateGUIItems)
          {
            this.m_GUIItems[index] = new ASHistoryWindow.GUIHistoryListItem();
            this.m_GUIItems[index].colAuthor = new GUIContent(this.m_Changesets[index].owner);
            this.m_GUIItems[index].colRevision = new GUIContent(this.m_Changesets[index].changeset.ToString());
            this.m_GUIItems[index].colDate = new GUIContent(this.m_Changesets[index].date);
            this.m_GUIItems[index].colDescription = new GUIContent(this.m_Changesets[index].message);
            this.m_GUIItems[index].assets = new ParentViewState();
            this.m_GUIItems[index].assets.AddAssetItems(this.m_Changesets[index]);
            this.m_GUIItems[index].totalLineCount = this.m_GUIItems[index].assets.GetLineCount();
            this.m_GUIItems[index].height = this.m_RowHeight * (1 + this.m_GUIItems[index].totalLineCount) + 20 + (int) ASHistoryWindow.ms_Style.descriptionLabel.CalcHeight(this.m_GUIItems[index].colDescription, float.MaxValue);
          }
          this.m_GUIItems[index].boldAssets = new bool[this.m_GUIItems[index].assets.GetLineCount()];
          int num = !flag ? this.CheckParentViewInFilterAndMarkBoldItems(this.m_GUIItems[index], filterText) : this.MarkBoldItemsBySelection(this.m_GUIItems[index]);
          this.m_GUIItems[index].inFilter = flag || num != -1 || (this.m_GUIItems[index].colDescription.text.IndexOf(filterText, StringComparison.InvariantCultureIgnoreCase) >= 0 || this.m_GUIItems[index].colRevision.text.IndexOf(filterText, StringComparison.InvariantCultureIgnoreCase) >= 0) || this.m_GUIItems[index].colAuthor.text.IndexOf(filterText, StringComparison.InvariantCultureIgnoreCase) >= 0 || this.m_GUIItems[index].colDate.text.IndexOf(filterText, StringComparison.InvariantCultureIgnoreCase) >= 0;
          if (recreateGUIItems && this.m_GUIItems[index].totalLineCount > 5)
          {
            this.m_GUIItems[index].collapsedItemCount = this.m_GUIItems[index].totalLineCount - 5 + 1;
            this.m_GUIItems[index].height = this.m_RowHeight * 6 + 20 + (int) ASHistoryWindow.ms_Style.descriptionLabel.CalcHeight(this.m_GUIItems[index].colDescription, float.MaxValue);
          }
          this.m_GUIItems[index].startShowingFrom = 0;
          if (this.m_GUIItems[index].collapsedItemCount != 0 && this.m_GUIItems[index].totalLineCount > 5 && num >= 4)
            this.m_GUIItems[index].startShowingFrom = num + 5 - 1 <= this.m_GUIItems[index].totalLineCount ? num : this.m_GUIItems[index].totalLineCount - 5 + 1;
          if (this.m_GUIItems[index].inFilter)
            this.m_TotalHeight += this.m_GUIItems[index].height;
        }
      }
    }

    private void UncollapseListItem(ref ASHistoryWindow.GUIHistoryListItem item)
    {
      int num = (item.collapsedItemCount - 1) * this.m_RowHeight;
      item.collapsedItemCount = 0;
      item.startShowingFrom = 0;
      item.height += num;
      this.m_TotalHeight += num;
    }

    private void ClearLV()
    {
      this.m_Changesets = new Changeset[0];
      this.m_TotalHeight = 5;
    }

    public void DoLocalSelectionChange()
    {
      if (this.m_NextSelectionMine)
      {
        this.m_NextSelectionMine = false;
      }
      else
      {
        UnityEngine.Object[] filtered = Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.Assets);
        string[] guids = new string[0];
        switch (this.m_FileViewWin.SelType)
        {
          case ASHistoryFileView.SelectionType.All:
            if (Selection.objects.Length != 0)
            {
              Selection.objects = new UnityEngine.Object[0];
              this.m_NextSelectionMine = true;
            }
            this.m_SelectedPath = string.Empty;
            this.m_SelectedGUID = string.Empty;
            this.ClearLV();
            break;
          case ASHistoryFileView.SelectionType.Items:
            if (filtered.Length < 1)
            {
              this.m_SelectedPath = string.Empty;
              this.m_SelectedGUID = string.Empty;
              this.ClearLV();
              return;
            }
            this.m_SelectedPath = AssetDatabase.GetAssetPath(filtered[0]);
            this.m_SelectedGUID = AssetDatabase.AssetPathToGUID(this.m_SelectedPath);
            guids = this.m_FileViewWin.GetImplicitProjectViewSelection();
            break;
          case ASHistoryFileView.SelectionType.DeletedItemsRoot:
            if (Selection.objects.Length != 0)
            {
              Selection.objects = new UnityEngine.Object[0];
              this.m_NextSelectionMine = true;
            }
            guids = this.m_FileViewWin.GetAllDeletedItemGUIDs();
            if (guids.Length == 0)
            {
              this.ClearLV();
              return;
            }
            break;
          case ASHistoryFileView.SelectionType.DeletedItems:
            if (Selection.objects.Length != 0)
            {
              Selection.objects = new UnityEngine.Object[0];
              this.m_NextSelectionMine = true;
            }
            guids = this.m_FileViewWin.GetSelectedDeletedItemGUIDs();
            break;
        }
        this.m_Changesets = AssetServer.GetHistorySelected(guids);
        if (this.m_Changesets != null)
          this.FilterItems(true);
        else
          this.ClearLV();
        if (guids != null && this.m_GUIItems != null && guids.Length == 1)
          this.MarkBoldItemsByGUID(this.m_SelectedGUID);
        this.m_ParentWindow.Repaint();
      }
    }

    public void OnSelectionChange()
    {
      if (Selection.objects.Length != 0)
        this.m_FileViewWin.SelType = ASHistoryFileView.SelectionType.Items;
      this.DoLocalSelectionChange();
    }

    private void DoShowDiff(bool binary, int ver1, int ver2)
    {
      List<string> stringList = new List<string>();
      List<CompareInfo> compareInfoList = new List<CompareInfo>();
      if (ver2 == -1 && AssetDatabase.GUIDToAssetPath(this.m_ChangeLogSelectionGUID) == string.Empty)
      {
        Debug.Log((object) ("Cannot compare asset " + this.m_ChangeLogSelectionAssetName + " to local version because it does not exists."));
      }
      else
      {
        stringList.Add(this.m_ChangeLogSelectionGUID);
        compareInfoList.Add(new CompareInfo(ver1, ver2, !binary ? 0 : 1, !binary ? 1 : 0));
        Debug.Log((object) ("Comparing asset " + this.m_ChangeLogSelectionAssetName + " revisions " + ver1.ToString() + " and " + (ver2 != -1 ? ver2.ToString() : "Local")));
        AssetServer.CompareFiles(stringList.ToArray(), compareInfoList.ToArray());
      }
    }

    private void DoShowCustomDiff(bool binary)
    {
      this.ShowAssetsHistory();
      this.m_InRevisionSelectMode = true;
      this.m_BinaryDiff = binary;
      this.m_Rev1ForCustomDiff = this.ChangeLogSelectionRev;
    }

    private void FinishShowCustomDiff()
    {
      if (this.m_Rev1ForCustomDiff != this.ChangeLogSelectionRev)
        this.DoShowDiff(this.m_BinaryDiff, this.m_Rev1ForCustomDiff, this.ChangeLogSelectionRev);
      else
        Debug.Log((object) "You chose to compare to the same revision.");
      this.m_InRevisionSelectMode = false;
    }

    private void CancelShowCustomDiff()
    {
      this.m_InRevisionSelectMode = false;
    }

    private bool IsComparableAssetSelected()
    {
      if (!this.m_FolderSelected)
        return this.m_ChangeLogSelectionGUID != string.Empty;
      return false;
    }

    private void DrawBadge(Rect offset, ChangeFlags flags, GUIStyle style, GUIContent content, float textColWidth)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      GUIContent content1 = (GUIContent) null;
      if (this.HasFlag(flags, ChangeFlags.Undeleted) || this.HasFlag(flags, ChangeFlags.Created))
        content1 = ASMainWindow.constants.badgeNew;
      else if (this.HasFlag(flags, ChangeFlags.Deleted))
        content1 = ASMainWindow.constants.badgeDelete;
      else if (this.HasFlag(flags, ChangeFlags.Renamed) || this.HasFlag(flags, ChangeFlags.Moved))
        content1 = ASMainWindow.constants.badgeMove;
      if (content1 == null)
        return;
      Rect position = new Rect((double) style.CalcSize(content).x <= (double) textColWidth - (double) content1.image.width ? textColWidth - (float) content1.image.width : (float) ((double) offset.xMax - (double) content1.image.width - 5.0), offset.y + offset.height / 2f - (float) (content1.image.height / 2), (float) content1.image.width, (float) content1.image.height);
      EditorGUIUtility.SetIconSize(Vector2.zero);
      GUIStyle.none.Draw(position, content1, false, false, false, false);
      EditorGUIUtility.SetIconSize(ASHistoryWindow.ms_IconSize);
    }

    private bool HasFlag(ChangeFlags flags, ChangeFlags flagToCheck)
    {
      return (flagToCheck & flags) != ChangeFlags.None;
    }

    private void ClearItemSelection()
    {
      this.m_ChangeLogSelectionGUID = string.Empty;
      this.m_ChangeLogSelectionAssetName = string.Empty;
      this.m_FolderSelected = false;
      this.m_AssetSelectionIndex = -1;
    }

    private void DrawParentView(Rect r, ref ASHistoryWindow.GUIHistoryListItem item, int changesetIndex, bool hasFocus)
    {
      ParentViewState assets = item.assets;
      GUIContent content = new GUIContent();
      Texture2D texture = EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName);
      Event current = Event.current;
      hasFocus &= this.m_HistoryControlID == GUIUtility.keyboardControl;
      r.height = (float) this.m_RowHeight;
      r.y += 3f;
      int index1 = -1;
      int num = (item.collapsedItemCount == 0 ? item.totalLineCount : 4) + item.startShowingFrom;
      for (int index2 = 0; index2 < assets.folders.Length; ++index2)
      {
        ParentViewFolder folder = assets.folders[index2];
        content.text = folder.name;
        content.image = (Texture) texture;
        ++index1;
        if (index1 != num)
        {
          if (index1 >= item.startShowingFrom)
          {
            GUIStyle label = ASHistoryWindow.ms_Style.label;
            if (current.type == EventType.MouseDown && r.Contains(current.mousePosition))
            {
              if (this.ChangeLogSelectionRev == this.m_Changesets[changesetIndex].changeset && this.m_ChangeLogSelectionGUID == folder.guid && EditorGUI.actionKey)
              {
                this.ClearItemSelection();
              }
              else
              {
                this.ChangeLogSelectionRev = this.m_Changesets[changesetIndex].changeset;
                this.m_ChangeLogSelectionGUID = folder.guid;
                this.m_ChangeLogSelectionAssetName = folder.name;
                this.m_FolderSelected = true;
                this.m_AssetSelectionIndex = index1;
              }
              this.m_ChangesetSelectionIndex = changesetIndex;
              GUIUtility.keyboardControl = this.m_HistoryControlID;
              ((ASMainWindow) this.m_ParentWindow).m_SearchToShow = ASMainWindow.ShowSearchField.HistoryList;
              if (current.clickCount == 2)
              {
                this.ShowAssetsHistory();
                GUIUtility.ExitGUI();
              }
              else if (current.button == 1)
              {
                GUIUtility.hotControl = 0;
                r = new Rect(current.mousePosition.x, current.mousePosition.y, 1f, 1f);
                EditorUtility.DisplayCustomMenu(r, this.m_DropDownMenuItems, -1, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), (object) null);
              }
              this.DoScroll();
              current.Use();
            }
            bool on = this.ChangeLogSelectionRev == this.m_Changesets[changesetIndex].changeset && this.m_ChangeLogSelectionGUID == folder.guid;
            if (item.boldAssets[index1] && !on)
              GUI.Label(r, string.Empty, ASHistoryWindow.ms_Style.ping);
            if (Event.current.type == EventType.Repaint)
            {
              label.Draw(r, content, false, false, on, hasFocus);
              this.DrawBadge(r, folder.changeFlags, label, content, GUIClip.visibleRect.width - 150f);
            }
            r.y += (float) this.m_RowHeight;
          }
          ASHistoryWindow.ms_Style.label.padding.left += 16;
          ASHistoryWindow.ms_Style.boldLabel.padding.left += 16;
          try
          {
            for (int index3 = 0; index3 < folder.files.Length; ++index3)
            {
              ++index1;
              if (index1 != num)
              {
                if (index1 >= item.startShowingFrom)
                {
                  GUIStyle label = ASHistoryWindow.ms_Style.label;
                  if (current.type == EventType.MouseDown && r.Contains(current.mousePosition))
                  {
                    if (this.ChangeLogSelectionRev == this.m_Changesets[changesetIndex].changeset && this.m_ChangeLogSelectionGUID == folder.files[index3].guid && EditorGUI.actionKey)
                    {
                      this.ClearItemSelection();
                    }
                    else
                    {
                      this.ChangeLogSelectionRev = this.m_Changesets[changesetIndex].changeset;
                      this.m_ChangeLogSelectionGUID = folder.files[index3].guid;
                      this.m_ChangeLogSelectionAssetName = folder.files[index3].name;
                      this.m_FolderSelected = false;
                      this.m_AssetSelectionIndex = index1;
                    }
                    this.m_ChangesetSelectionIndex = changesetIndex;
                    GUIUtility.keyboardControl = this.m_HistoryControlID;
                    ((ASMainWindow) this.m_ParentWindow).m_SearchToShow = ASMainWindow.ShowSearchField.HistoryList;
                    if (current.clickCount == 2)
                    {
                      if (this.IsComparableAssetSelected() && this.m_SelectedGUID == this.m_ChangeLogSelectionGUID)
                      {
                        this.DoShowDiff(false, this.ChangeLogSelectionRev, -1);
                      }
                      else
                      {
                        this.ShowAssetsHistory();
                        GUIUtility.ExitGUI();
                      }
                    }
                    else if (current.button == 1)
                    {
                      GUIUtility.hotControl = 0;
                      r = new Rect(current.mousePosition.x, current.mousePosition.y, 1f, 1f);
                      EditorUtility.DisplayCustomMenu(r, this.m_DropDownMenuItems, -1, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), (object) null);
                    }
                    this.DoScroll();
                    current.Use();
                  }
                  content.text = folder.files[index3].name;
                  content.image = (Texture) InternalEditorUtility.GetIconForFile(folder.files[index3].name);
                  bool on = this.ChangeLogSelectionRev == this.m_Changesets[changesetIndex].changeset && this.m_ChangeLogSelectionGUID == folder.files[index3].guid;
                  if (item.boldAssets[index1] && !on)
                    GUI.Label(r, string.Empty, ASHistoryWindow.ms_Style.ping);
                  if (Event.current.type == EventType.Repaint)
                  {
                    label.Draw(r, content, false, false, on, hasFocus);
                    this.DrawBadge(r, folder.files[index3].changeFlags, label, content, GUIClip.visibleRect.width - 150f);
                  }
                  r.y += (float) this.m_RowHeight;
                }
              }
              else
                break;
            }
            if (index1 == num)
              break;
          }
          finally
          {
            ASHistoryWindow.ms_Style.label.padding.left -= 16;
            ASHistoryWindow.ms_Style.boldLabel.padding.left -= 16;
          }
        }
        else
          break;
      }
      if (index1 != num && num < item.totalLineCount || item.collapsedItemCount == 0)
        return;
      r.x += 19f;
      if (!GUI.Button(r, item.collapsedItemCount.ToString() + " more...", ASHistoryWindow.ms_Style.foldout))
        return;
      GUIUtility.keyboardControl = this.m_HistoryControlID;
      this.UncollapseListItem(ref item);
    }

    private int FindFirstUnfilteredItem(int fromIndex, int direction)
    {
      int index = fromIndex;
      while (index >= 0 && index < this.m_GUIItems.Length)
      {
        if (this.m_GUIItems[index].inFilter)
          return index;
        index += direction;
      }
      return -1;
    }

    private void MoveSelection(int steps)
    {
      if (this.m_ChangeLogSelectionGUID == string.Empty)
      {
        int direction = (int) Mathf.Sign((float) steps);
        steps = Mathf.Abs(steps);
        for (int index = 0; index < steps; ++index)
        {
          int firstUnfilteredItem = this.FindFirstUnfilteredItem(this.m_ChangesetSelectionIndex + direction, direction);
          if (firstUnfilteredItem != -1)
            this.m_ChangesetSelectionIndex = firstUnfilteredItem;
          else
            break;
        }
        this.ChangeLogSelectionRev = this.m_Changesets[this.m_ChangesetSelectionIndex].changeset;
      }
      else
      {
        this.m_AssetSelectionIndex += steps;
        if (this.m_AssetSelectionIndex < this.m_GUIItems[this.m_ChangesetSelectionIndex].startShowingFrom)
        {
          this.m_AssetSelectionIndex = this.m_GUIItems[this.m_ChangesetSelectionIndex].startShowingFrom;
        }
        else
        {
          int lineCount = this.m_GUIItems[this.m_ChangesetSelectionIndex].assets.GetLineCount();
          if (this.m_AssetSelectionIndex >= 4 + this.m_GUIItems[this.m_ChangesetSelectionIndex].startShowingFrom && this.m_GUIItems[this.m_ChangesetSelectionIndex].collapsedItemCount != 0)
            this.UncollapseListItem(ref this.m_GUIItems[this.m_ChangesetSelectionIndex]);
          if (this.m_AssetSelectionIndex >= lineCount)
            this.m_AssetSelectionIndex = lineCount - 1;
        }
        int folder = 0;
        int file = 0;
        if (!this.m_GUIItems[this.m_ChangesetSelectionIndex].assets.IndexToFolderAndFile(this.m_AssetSelectionIndex, ref folder, ref file))
          return;
        if (file == -1)
          this.m_ChangeLogSelectionGUID = this.m_GUIItems[this.m_ChangesetSelectionIndex].assets.folders[folder].guid;
        else
          this.m_ChangeLogSelectionGUID = this.m_GUIItems[this.m_ChangesetSelectionIndex].assets.folders[folder].files[file].guid;
      }
    }

    private void HandleWebLikeKeyboard()
    {
      Event current = Event.current;
      if (current.GetTypeForControl(this.m_HistoryControlID) != EventType.KeyDown || this.m_GUIItems.Length == 0)
        return;
      KeyCode keyCode = current.keyCode;
      switch (keyCode)
      {
        case KeyCode.KeypadEnter:
          if (this.IsComparableAssetSelected())
          {
            this.DoShowDiff(false, this.ChangeLogSelectionRev, -1);
            break;
          }
          break;
        case KeyCode.UpArrow:
          this.MoveSelection(-1);
          break;
        case KeyCode.DownArrow:
          this.MoveSelection(1);
          break;
        case KeyCode.RightArrow:
          if (this.m_ChangeLogSelectionGUID == string.Empty && this.m_GUIItems.Length > 0)
          {
            this.m_ChangeLogSelectionGUID = this.m_GUIItems[this.m_ChangesetSelectionIndex].assets.folders[0].guid;
            this.m_ChangeLogSelectionAssetName = this.m_GUIItems[this.m_ChangesetSelectionIndex].assets.folders[0].name;
            this.m_FolderSelected = true;
            this.m_AssetSelectionIndex = 0;
            break;
          }
          break;
        case KeyCode.LeftArrow:
          this.m_ChangeLogSelectionGUID = string.Empty;
          break;
        case KeyCode.Home:
          if (this.m_ChangeLogSelectionGUID == string.Empty)
          {
            int firstUnfilteredItem = this.FindFirstUnfilteredItem(0, 1);
            if (firstUnfilteredItem != -1)
              this.m_ChangesetSelectionIndex = firstUnfilteredItem;
            this.ChangeLogSelectionRev = this.m_Changesets[this.m_ChangesetSelectionIndex].changeset;
            break;
          }
          this.MoveSelection(-999999);
          break;
        case KeyCode.End:
          if (this.m_ChangeLogSelectionGUID == string.Empty)
          {
            int firstUnfilteredItem = this.FindFirstUnfilteredItem(this.m_GUIItems.Length - 1, -1);
            if (firstUnfilteredItem != -1)
              this.m_ChangesetSelectionIndex = firstUnfilteredItem;
            this.ChangeLogSelectionRev = this.m_Changesets[this.m_ChangesetSelectionIndex].changeset;
            break;
          }
          this.MoveSelection(999999);
          break;
        case KeyCode.PageUp:
          if (Application.platform == RuntimePlatform.OSXEditor)
          {
            this.m_ScrollPos.y -= (float) this.m_ScrollViewHeight;
            if ((double) this.m_ScrollPos.y < 0.0)
            {
              this.m_ScrollPos.y = 0.0f;
              break;
            }
            break;
          }
          this.MoveSelection(-Mathf.RoundToInt((float) (this.m_ScrollViewHeight / this.m_RowHeight)));
          break;
        case KeyCode.PageDown:
          if (Application.platform == RuntimePlatform.OSXEditor)
          {
            this.m_ScrollPos.y += (float) this.m_ScrollViewHeight;
            break;
          }
          this.MoveSelection(Mathf.RoundToInt((float) (this.m_ScrollViewHeight / this.m_RowHeight)));
          break;
        default:
          if (keyCode != KeyCode.Return)
            return;
          goto case KeyCode.KeypadEnter;
      }
      this.DoScroll();
      current.Use();
    }

    private void WebLikeHistory(bool hasFocus)
    {
      if (this.m_Changesets == null)
        this.m_Changesets = new Changeset[0];
      if (this.m_GUIItems == null)
        return;
      this.m_HistoryControlID = GUIUtility.GetControlID(ASHistoryWindow.ms_HistoryControlHash, FocusType.Native);
      this.HandleWebLikeKeyboard();
      Event current = Event.current;
      if (current.GetTypeForControl(this.m_HistoryControlID) == EventType.ValidateCommand)
      {
        current.Use();
      }
      else
      {
        GUILayout.Space(1f);
        this.m_ScrollPos = GUILayout.BeginScrollView(this.m_ScrollPos);
        int num1 = 0;
        GUILayoutUtility.GetRect(1f, (float) (this.m_TotalHeight - 1));
        if ((current.type == EventType.Repaint || current.type == EventType.MouseDown || current.type == EventType.MouseUp) && this.m_GUIItems != null)
        {
          for (int changesetIndex = 0; changesetIndex < this.m_Changesets.Length; ++changesetIndex)
          {
            if (this.m_GUIItems[changesetIndex].inFilter)
            {
              if ((double) (num1 + this.m_GUIItems[changesetIndex].height) > (double) GUIClip.visibleRect.y && (double) num1 < (double) GUIClip.visibleRect.yMax)
              {
                float num2 = ASHistoryWindow.ms_Style.descriptionLabel.CalcHeight(this.m_GUIItems[changesetIndex].colDescription, float.MaxValue);
                Rect rect;
                if (current.type == EventType.Repaint)
                {
                  if (this.ChangeLogSelectionRev == this.m_Changesets[changesetIndex].changeset && Event.current.type == EventType.Repaint)
                  {
                    rect = new Rect(0.0f, (float) num1, GUIClip.visibleRect.width, (float) (this.m_GUIItems[changesetIndex].height - 10));
                    ASHistoryWindow.ms_Style.selected.Draw(rect, false, false, false, false);
                  }
                  rect = new Rect(0.0f, (float) (num1 + 3), GUIClip.visibleRect.width, (float) this.m_GUIItems[changesetIndex].height);
                  GUI.Label(rect, this.m_GUIItems[changesetIndex].colAuthor, ASHistoryWindow.ms_Style.boldLabel);
                  rect = new Rect(GUIClip.visibleRect.width - 160f, (float) (num1 + 3), 60f, (float) this.m_GUIItems[changesetIndex].height);
                  GUI.Label(rect, this.m_GUIItems[changesetIndex].colRevision, ASHistoryWindow.ms_Style.boldLabel);
                  rect.x += 60f;
                  rect.width = 100f;
                  GUI.Label(rect, this.m_GUIItems[changesetIndex].colDate, ASHistoryWindow.ms_Style.boldLabel);
                  rect.x = (float) ASHistoryWindow.ms_Style.boldLabel.margin.left;
                  rect.y += (float) this.m_RowHeight;
                  rect.width = GUIClip.visibleRect.width;
                  rect.height = num2;
                  GUI.Label(rect, this.m_GUIItems[changesetIndex].colDescription, ASHistoryWindow.ms_Style.descriptionLabel);
                  rect.y += num2;
                }
                rect = new Rect(0.0f, (float) num1 + num2 + (float) this.m_RowHeight, GUIClip.visibleRect.width, (float) this.m_GUIItems[changesetIndex].height - num2 - (float) this.m_RowHeight);
                this.DrawParentView(rect, ref this.m_GUIItems[changesetIndex], changesetIndex, hasFocus);
                if (current.type == EventType.MouseDown)
                {
                  rect = new Rect(0.0f, (float) num1, GUIClip.visibleRect.width, (float) (this.m_GUIItems[changesetIndex].height - 10));
                  if (rect.Contains(current.mousePosition))
                  {
                    this.ChangeLogSelectionRev = this.m_Changesets[changesetIndex].changeset;
                    this.m_ChangesetSelectionIndex = changesetIndex;
                    GUIUtility.keyboardControl = this.m_HistoryControlID;
                    ((ASMainWindow) this.m_ParentWindow).m_SearchToShow = ASMainWindow.ShowSearchField.HistoryList;
                    if (current.button == 1)
                    {
                      GUIUtility.hotControl = 0;
                      rect = new Rect(current.mousePosition.x, current.mousePosition.y, 1f, 1f);
                      EditorUtility.DisplayCustomMenu(rect, this.m_DropDownChangesetMenuItems, -1, new EditorUtility.SelectMenuItemFunction(this.ChangesetContextMenuClick), (object) null);
                      Event.current.Use();
                    }
                    this.DoScroll();
                    current.Use();
                  }
                }
              }
              num1 += this.m_GUIItems[changesetIndex].height;
            }
          }
        }
        else if (this.m_GUIItems == null)
          GUILayout.Label(EditorGUIUtility.TextContent("This item is not yet committed to the Asset Server"));
        if (Event.current.type == EventType.Repaint)
          this.m_ScrollViewHeight = (int) GUIClip.visibleRect.height;
        GUILayout.EndScrollView();
      }
    }

    private void DoScroll()
    {
      int num = 0;
      int index;
      for (index = 0; index < this.m_ChangesetSelectionIndex; ++index)
      {
        if (this.m_GUIItems[index].inFilter)
          num += this.m_GUIItems[index].height;
      }
      float max;
      float min;
      if (this.m_ChangeLogSelectionGUID != string.Empty)
      {
        max = (float) (num + (2 + this.m_AssetSelectionIndex) * this.m_RowHeight + 5);
        min = max - (float) this.m_ScrollViewHeight + (float) this.m_RowHeight;
      }
      else
      {
        max = (float) num;
        min = (float) ((double) max - (double) this.m_ScrollViewHeight + (double) this.m_GUIItems[index].height - 10.0);
      }
      this.m_ScrollPos.y = Mathf.Clamp(this.m_ScrollPos.y, min, max);
    }

    public bool DoGUI(bool hasFocus)
    {
      bool enabled = GUI.enabled;
      if (ASHistoryWindow.ms_Style == null)
      {
        ASHistoryWindow.ms_Style = new ASHistoryWindow.Constants();
        ASHistoryWindow.ms_Style.entryEven = new GUIStyle(ASHistoryWindow.ms_Style.entryEven);
        ASHistoryWindow.ms_Style.entryEven.padding.left = 3;
        ASHistoryWindow.ms_Style.entryOdd = new GUIStyle(ASHistoryWindow.ms_Style.entryOdd);
        ASHistoryWindow.ms_Style.entryOdd.padding.left = 3;
        ASHistoryWindow.ms_Style.label = new GUIStyle(ASHistoryWindow.ms_Style.label);
        ASHistoryWindow.ms_Style.boldLabel = new GUIStyle(ASHistoryWindow.ms_Style.boldLabel);
        ASHistoryWindow.ms_Style.label.padding.left = 3;
        ASHistoryWindow.ms_Style.boldLabel.padding.left = 3;
        ASHistoryWindow.ms_Style.boldLabel.padding.top = 0;
        ASHistoryWindow.ms_Style.boldLabel.padding.bottom = 0;
        this.DoLocalSelectionChange();
      }
      EditorGUIUtility.SetIconSize(ASHistoryWindow.ms_IconSize);
      if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
      {
        this.CancelShowCustomDiff();
        Event.current.Use();
      }
      SplitterGUILayout.BeginHorizontalSplit(this.m_HorSplit);
      GUILayout.BeginVertical();
      Rect rect = GUILayoutUtility.GetRect(0.0f, 0.0f, new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      this.m_FileViewWin.DoGUI(this, rect, hasFocus);
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      this.WebLikeHistory(hasFocus);
      GUILayout.EndVertical();
      SplitterGUILayout.EndHorizontalSplit();
      if (Event.current.type == EventType.Repaint)
      {
        Handles.color = Color.black;
        Handles.DrawLine(new Vector3((float) (this.m_HorSplit.realSizes[0] - 1), rect.y, 0.0f), new Vector3((float) (this.m_HorSplit.realSizes[0] - 1), rect.yMax, 0.0f));
        Handles.DrawLine(new Vector3(0.0f, rect.yMax, 0.0f), new Vector3((float) Screen.width, rect.yMax, 0.0f));
      }
      GUILayout.BeginHorizontal();
      GUI.enabled = this.m_FileViewWin.SelType == ASHistoryFileView.SelectionType.DeletedItems && enabled;
      if (GUILayout.Button(EditorGUIUtility.TextContent("Recover"), ASHistoryWindow.ms_Style.button, new GUILayoutOption[0]))
        this.m_FileViewWin.DoRecover();
      GUILayout.FlexibleSpace();
      if (this.m_InRevisionSelectMode)
      {
        GUI.enabled = enabled;
        GUILayout.Label(EditorGUIUtility.TextContent("Select revision to compare to"), ASHistoryWindow.ms_Style.boldLabel, new GUILayoutOption[0]);
      }
      GUILayout.Space(10f);
      GUI.enabled = this.IsComparableAssetSelected() && enabled;
      if (GUILayout.Button(EditorGUIUtility.TextContent("Compare to Local Version"), ASHistoryWindow.ms_Style.button, new GUILayoutOption[0]))
      {
        this.DoShowDiff(false, this.ChangeLogSelectionRev, -1);
        GUIUtility.ExitGUI();
      }
      GUI.enabled = this.ChangeLogSelectionRev > 0 && this.m_ChangeLogSelectionGUID != string.Empty && enabled;
      if (GUILayout.Button(EditorGUIUtility.TextContent("Download Selected File"), ASHistoryWindow.ms_Style.button, new GUILayoutOption[0]))
        this.DownloadFile();
      GUILayout.Space(10f);
      GUI.enabled = this.ChangeLogSelectionRev > 0 && enabled;
      if (GUILayout.Button(this.ChangeLogSelectionRev <= 0 ? "Revert Entire Project" : "Revert Entire Project to " + (object) this.ChangeLogSelectionRev, ASHistoryWindow.ms_Style.button, new GUILayoutOption[0]))
        this.DoRevertProject();
      GUI.enabled = enabled;
      GUILayout.EndHorizontal();
      GUILayout.Space(10f);
      if (!this.m_SplittersOk && Event.current.type == EventType.Repaint)
      {
        this.m_SplittersOk = true;
        HandleUtility.Repaint();
      }
      EditorGUIUtility.SetIconSize(Vector2.zero);
      return true;
    }

    internal class Constants
    {
      public GUIStyle selected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle lvHeader = (GUIStyle) "OL title";
      public GUIStyle button = (GUIStyle) "Button";
      public GUIStyle label = (GUIStyle) "PR Label";
      public GUIStyle descriptionLabel = (GUIStyle) "Label";
      public GUIStyle entryEven = (GUIStyle) "CN EntryBackEven";
      public GUIStyle entryOdd = (GUIStyle) "CN EntryBackOdd";
      public GUIStyle boldLabel = (GUIStyle) "BoldLabel";
      public GUIStyle foldout = (GUIStyle) "IN Foldout";
      public GUIStyle ping = new GUIStyle((GUIStyle) "PR Ping");

      public Constants()
      {
        this.ping.overflow.left = -2;
        this.ping.overflow.right = -21;
        this.ping.padding.left = 48;
        this.ping.padding.right = 0;
      }
    }

    [Serializable]
    private class GUIHistoryListItem
    {
      public GUIContent colAuthor;
      public GUIContent colRevision;
      public GUIContent colDate;
      public GUIContent colDescription;
      public ParentViewState assets;
      public int totalLineCount;
      public bool[] boldAssets;
      public int height;
      public bool inFilter;
      public int collapsedItemCount;
      public int startShowingFrom;
    }
  }
}
