// Decompiled with JetBrains decompiler
// Type: UnityEditor.ASMainWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Server", useTypeNameAsIconName = true)]
  internal class ASMainWindow : EditorWindow, IHasCustomMenu
  {
    public ASMainWindow.ShowSearchField m_SearchToShow = ASMainWindow.ShowSearchField.HistoryList;
    public ASMainWindow.SearchField m_SearchField = new ASMainWindow.SearchField();
    private string[] pageTitles = new string[4]
    {
      "Overview",
      "Update",
      "Commit",
      string.Empty
    };
    private string[] dropDownMenuItems = new string[6]
    {
      "Connection",
      string.Empty,
      "Show History",
      "Discard Changes",
      string.Empty,
      "Server Administration"
    };
    private string[] unconfiguredDropDownMenuItems = new string[3]
    {
      "Connection",
      string.Empty,
      "Server Administration"
    };
    private string[] commitDropDownMenuItems = new string[6]
    {
      "Commit",
      string.Empty,
      "Compare",
      "Compare Binary",
      string.Empty,
      "Discard"
    };
    private bool needsSetup = true;
    private string connectionString = string.Empty;
    private int maxNickLength = 1;
    private int widthToHideButtons = 591;
    private ASMainWindow.Page selectedPage = ASMainWindow.Page.NotInitialized;
    private ListViewState lv = new ListViewState(0);
    private ParentViewState pv = new ParentViewState();
    private Vector2 iconSize = new Vector2(16f, 16f);
    private SplitterState splitter = new SplitterState(new float[2]{ 50f, 50f }, new int[2]{ 80, 80 }, (int[]) null);
    private string commitMessage = string.Empty;
    private int lastRevertSelectionChanged = -1;
    private const ASMainWindow.Page lastMainPage = ASMainWindow.Page.Commit;
    public static ASMainWindow.Constants constants;
    public AssetsItem[] sharedCommits;
    public AssetsItem[] sharedDeletedItems;
    public Changeset[] sharedChangesets;
    private GUIContent[] changesetContents;
    public ASMainWindow.ShowSearchField m_ShowSearch;
    private bool showSmallWindow;
    private bool wasHidingButtons;
    internal ASHistoryWindow asHistoryWin;
    internal ASUpdateWindow asUpdateWin;
    internal ASCommitWindow asCommitWin;
    internal ASServerAdminWindow asAdminWin;
    internal ASConfigWindow asConfigWin;
    private bool error;
    private bool isInitialUpdate;
    private bool committing;
    private bool selectionChangedWhileCommitting;
    private bool pvHasSelection;
    private bool somethingDiscardableSelected;
    private bool mySelection;
    private bool focusCommitMessage;
    private bool m_CheckedMaint;

    public bool NeedsSetup
    {
      get
      {
        return this.needsSetup;
      }
      set
      {
        this.needsSetup = value;
      }
    }

    public bool Error
    {
      get
      {
        return this.error;
      }
    }

    public ASMainWindow()
    {
      this.position = new Rect(50f, 50f, 800f, 600f);
    }

    public void LogError(string errorStr)
    {
      Debug.LogError((object) errorStr);
      AssetServer.SetAssetServerError(errorStr, false);
      this.error = true;
    }

    private void Awake()
    {
      this.pv.lv = new ListViewState(0);
      this.isInitialUpdate = true;
    }

    private void NotifyClosingCommit()
    {
      if (this.asCommitWin == null)
        return;
      this.asCommitWin.OnClose();
    }

    private void OnDestroy()
    {
      this.sharedCommits = (AssetsItem[]) null;
      this.sharedDeletedItems = (AssetsItem[]) null;
      this.sharedChangesets = (Changeset[]) null;
      this.changesetContents = (GUIContent[]) null;
      if (this.selectedPage != ASMainWindow.Page.Commit)
        return;
      this.NotifyClosingCommit();
    }

    private void DoSelectionChange()
    {
      if (this.committing)
      {
        this.selectionChangedWhileCommitting = true;
      }
      else
      {
        HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
        List<string> guids = new List<string>(Selection.objects.Length);
        foreach (UnityEngine.Object @object in Selection.objects)
        {
          if (hierarchyProperty.Find(@object.GetInstanceID(), (int[]) null))
            guids.Add(hierarchyProperty.guid);
        }
        this.pvHasSelection = ASCommitWindow.MarkSelected(this.pv, guids);
      }
    }

    private void OnSelectionChange()
    {
      switch (this.selectedPage)
      {
        case ASMainWindow.Page.Overview:
          if (!this.mySelection)
          {
            this.DoSelectionChange();
            this.Repaint();
          }
          else
            this.mySelection = false;
          this.somethingDiscardableSelected = ASCommitWindow.SomethingDiscardableSelected(this.pv);
          break;
        case ASMainWindow.Page.Update:
          this.asUpdateWin.OnSelectionChange();
          break;
        case ASMainWindow.Page.Commit:
          this.asCommitWin.OnSelectionChange();
          break;
        case ASMainWindow.Page.History:
          this.asHistoryWin.OnSelectionChange();
          break;
      }
    }

    internal void Reinit()
    {
      this.SwitchSelectedPage(ASMainWindow.Page.Overview);
      this.Repaint();
    }

    public void DoDiscardChanges(bool lastActionsResult)
    {
      List<string> stringList = new List<string>();
      if (false)
      {
        stringList.AddRange((IEnumerable<string>) AssetServer.CollectDeepSelection());
      }
      else
      {
        stringList.AddRange((IEnumerable<string>) AssetServer.GetAllRootGUIDs());
        stringList.AddRange((IEnumerable<string>) AssetServer.CollectAllChildren(AssetServer.GetRootGUID(), AssetServer.GetAllRootGUIDs()));
      }
      if (stringList.Count == 0)
        stringList.AddRange((IEnumerable<string>) AssetServer.GetAllRootGUIDs());
      AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", "CBReinitOnSuccess");
      AssetServer.DoUpdateWithoutConflictResolutionOnNextTick(stringList.ToArray());
    }

    private bool WordWrappedLabelButton(string label, string buttonText)
    {
      GUILayout.BeginHorizontal();
      GUILayout.Label(label, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      bool flag = GUILayout.Button(buttonText, new GUILayoutOption[1]
      {
        GUILayout.Width(110f)
      });
      GUILayout.EndHorizontal();
      return flag;
    }

    private bool ToolbarToggle(bool pressed, string title, GUIStyle style)
    {
      bool changed = GUI.changed;
      GUI.changed = false;
      GUILayout.Toggle(pressed, title, style, new GUILayoutOption[0]);
      if (GUI.changed)
        return true;
      GUI.changed |= changed;
      return false;
    }

    private bool RightButton(string title)
    {
      return this.RightButton(title, ASMainWindow.constants.smallButton);
    }

    private bool RightButton(string title, GUIStyle style)
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      bool flag = GUILayout.Button(title, style, new GUILayoutOption[0]);
      GUILayout.EndHorizontal();
      return flag;
    }

    public void ShowConflictResolutions(string[] conflicting)
    {
      if (this.asUpdateWin == null)
        this.LogError("Found unexpected conflicts. Please use Bug Reporter to report a bug.");
      else
        this.asUpdateWin.ShowConflictResolutions(conflicting);
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      if (!this.needsSetup)
      {
        menu.AddItem(new GUIContent("Refresh"), false, new GenericMenu.MenuFunction(this.ActionRefresh));
        menu.AddSeparator(string.Empty);
      }
      menu.AddItem(new GUIContent("Connection"), false, new GenericMenu.MenuFunction2(this.ActionSwitchPage), (object) ASMainWindow.Page.ServerConfig);
      menu.AddSeparator(string.Empty);
      if (!this.needsSetup)
      {
        menu.AddItem(new GUIContent("Show History"), false, new GenericMenu.MenuFunction2(this.ActionSwitchPage), (object) ASMainWindow.Page.History);
        menu.AddItem(new GUIContent("Discard Changes"), false, new GenericMenu.MenuFunction(this.ActionDiscardChanges));
        menu.AddSeparator(string.Empty);
      }
      menu.AddItem(new GUIContent("Server Administration"), false, new GenericMenu.MenuFunction2(this.ActionSwitchPage), (object) ASMainWindow.Page.Admin);
    }

    public bool UpdateNeedsRefresh()
    {
      if (this.sharedChangesets != null)
        return AssetServer.GetRefreshUpdate();
      return true;
    }

    public bool CommitNeedsRefresh()
    {
      if (this.sharedCommits != null && this.sharedDeletedItems != null)
        return AssetServer.GetRefreshCommit();
      return true;
    }

    private void ContextMenuClick(object userData, string[] options, int selected)
    {
      if (selected < 0)
        return;
      string dropDownMenuItem = this.dropDownMenuItems[selected];
      if (dropDownMenuItem == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ASMainWindow.\u003C\u003Ef__switch\u0024map11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ASMainWindow.\u003C\u003Ef__switch\u0024map11 = new Dictionary<string, int>(4)
        {
          {
            "Connection",
            0
          },
          {
            "Show History",
            1
          },
          {
            "Discard Changes",
            2
          },
          {
            "Server Administration",
            3
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!ASMainWindow.\u003C\u003Ef__switch\u0024map11.TryGetValue(dropDownMenuItem, out num))
        return;
      switch (num)
      {
        case 0:
          this.ActionSwitchPage((object) ASMainWindow.Page.ServerConfig);
          break;
        case 1:
          this.ActionSwitchPage((object) ASMainWindow.Page.History);
          break;
        case 2:
          this.ActionDiscardChanges();
          break;
        case 3:
          this.ActionSwitchPage((object) ASMainWindow.Page.Admin);
          break;
      }
    }

    private void CommitContextMenuClick(object userData, string[] options, int selected)
    {
      if (selected < 0)
        return;
      string dropDownMenuItem = this.commitDropDownMenuItems[selected];
      if (dropDownMenuItem == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ASMainWindow.\u003C\u003Ef__switch\u0024map12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ASMainWindow.\u003C\u003Ef__switch\u0024map12 = new Dictionary<string, int>(4)
        {
          {
            "Commit",
            0
          },
          {
            "Compare",
            1
          },
          {
            "Compare Binary",
            2
          },
          {
            "Discard",
            3
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!ASMainWindow.\u003C\u003Ef__switch\u0024map12.TryGetValue(dropDownMenuItem, out num))
        return;
      switch (num)
      {
        case 0:
          this.StartCommitting();
          break;
        case 1:
          ASCommitWindow.DoShowDiff(ASCommitWindow.GetParentViewSelectedItems(this.pv, false, false), false);
          break;
        case 2:
          ASCommitWindow.DoShowDiff(ASCommitWindow.GetParentViewSelectedItems(this.pv, false, false), true);
          break;
        case 3:
          this.DoMyRevert(false);
          break;
      }
    }

    public void CommitItemsChanged()
    {
      this.InitCommits();
      this.DisplayedItemsChanged();
      if (this.selectedPage == ASMainWindow.Page.Commit)
        this.asCommitWin.Update();
      this.Repaint();
    }

    public void RevertProject(int toRevision, Changeset[] changesets)
    {
      AssetServer.SetStickyChangeset(toRevision);
      this.asUpdateWin = new ASUpdateWindow(this, changesets);
      this.asUpdateWin.SetSelectedRevisionLine(0);
      this.asUpdateWin.DoUpdate(false);
      this.selectedPage = ASMainWindow.Page.Update;
    }

    public void ShowHistory()
    {
      this.SwitchSelectedPage(ASMainWindow.Page.Overview);
      this.isInitialUpdate = false;
      this.SwitchSelectedPage(ASMainWindow.Page.History);
    }

    private void ActionRefresh()
    {
      switch (this.selectedPage)
      {
        case ASMainWindow.Page.Overview:
          AssetServer.CheckForServerUpdates();
          this.InitiateRefreshAssetsAndUpdateStatusWithCallback("CBInitOverviewPage");
          break;
        case ASMainWindow.Page.Update:
          AssetServer.CheckForServerUpdates();
          if (!this.UpdateNeedsRefresh())
            break;
          this.InitiateUpdateStatusWithCallback("CBInitUpdatePage");
          break;
        case ASMainWindow.Page.Commit:
          this.asCommitWin.InitiateReinit();
          break;
        case ASMainWindow.Page.History:
          AssetServer.CheckForServerUpdates();
          if (!this.UpdateNeedsRefresh())
            break;
          this.InitiateUpdateStatusWithCallback("CBInitHistoryPage");
          break;
        default:
          this.Reinit();
          break;
      }
    }

    private void ActionSwitchPage(object page)
    {
      this.SwitchSelectedPage((ASMainWindow.Page) page);
    }

    private void ActionDiscardChanges()
    {
      if (!EditorUtility.DisplayDialog("Discard all changes", "Are you sure you want to discard all local changes made in the project?", "Discard", "Cancel"))
        return;
      AssetServer.RemoveMaintErrorsFromConsole();
      if (!ASEditorBackend.SettingsIfNeeded())
      {
        Debug.Log((object) "Asset Server connection for current project is not set up");
        this.error = true;
      }
      else
      {
        this.error = false;
        AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", "CBDoDiscardChanges");
        AssetServer.DoUpdateStatusOnNextTick();
      }
    }

    private void SwitchSelectedPage(ASMainWindow.Page page)
    {
      ASMainWindow.Page selectedPage = this.selectedPage;
      this.selectedPage = page;
      this.SelectedPageChanged();
      if (!this.error)
        return;
      this.selectedPage = selectedPage;
      this.error = false;
    }

    private void InitiateUpdateStatusWithCallback(string callbackName)
    {
      if (!ASEditorBackend.SettingsIfNeeded())
      {
        Debug.Log((object) "Asset Server connection for current project is not set up");
        this.error = true;
      }
      else
      {
        this.error = false;
        AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", callbackName);
        AssetServer.DoUpdateStatusOnNextTick();
      }
    }

    private void InitiateRefreshAssetsWithCallback(string callbackName)
    {
      if (!ASEditorBackend.SettingsIfNeeded())
      {
        Debug.Log((object) "Asset Server connection for current project is not set up");
        this.error = true;
      }
      else
      {
        this.error = false;
        AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", callbackName);
        AssetServer.DoRefreshAssetsOnNextTick();
      }
    }

    private void InitiateRefreshAssetsAndUpdateStatusWithCallback(string callbackName)
    {
      if (!ASEditorBackend.SettingsIfNeeded())
      {
        Debug.Log((object) "Asset Server connection for current project is not set up");
        this.error = true;
      }
      else
      {
        this.error = false;
        AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", callbackName);
        AssetServer.DoRefreshAssetsAndUpdateStatusOnNextTick();
      }
    }

    private void SelectedPageChanged()
    {
      AssetServer.ClearAssetServerError();
      if (this.committing)
        this.CancelCommit();
      switch (this.selectedPage)
      {
        case ASMainWindow.Page.Overview:
          if (ASEditorBackend.SettingsAreValid())
          {
            AssetServer.CheckForServerUpdates();
            if (this.UpdateNeedsRefresh())
            {
              this.InitiateUpdateStatusWithCallback("CBInitOverviewPage");
              break;
            }
            this.InitOverviewPage(true);
            break;
          }
          this.connectionString = "Asset Server connection for current project is not set up";
          this.sharedChangesets = new Changeset[0];
          this.changesetContents = new GUIContent[0];
          this.needsSetup = true;
          break;
        case ASMainWindow.Page.Update:
          this.InitUpdatePage(true);
          break;
        case ASMainWindow.Page.Commit:
          this.asCommitWin = new ASCommitWindow(this, !this.pvHasSelection ? (string[]) null : ASCommitWindow.GetParentViewSelectedItems(this.pv, true, false).ToArray());
          this.asCommitWin.InitiateReinit();
          break;
        case ASMainWindow.Page.History:
          this.pageTitles[3] = "History";
          this.InitHistoryPage(true);
          break;
        case ASMainWindow.Page.ServerConfig:
          this.pageTitles[3] = "Connection";
          this.asConfigWin = new ASConfigWindow(this);
          break;
        case ASMainWindow.Page.Admin:
          this.pageTitles[3] = "Administration";
          this.asAdminWin = new ASServerAdminWindow(this);
          if (!this.error)
            break;
          break;
      }
    }

    public void InitUpdatePage(bool lastActionsResult)
    {
      if (!lastActionsResult)
      {
        this.Reinit();
      }
      else
      {
        if (this.UpdateNeedsRefresh())
          this.GetUpdates();
        if (this.sharedChangesets == null)
        {
          this.Reinit();
        }
        else
        {
          this.asUpdateWin = new ASUpdateWindow(this, this.sharedChangesets);
          this.asUpdateWin.SetSelectedRevisionLine(0);
        }
      }
    }

    private void InitCommits()
    {
      if (this.CommitNeedsRefresh())
      {
        if (AssetServer.GetAssetServerError() == string.Empty)
        {
          this.sharedCommits = ASCommitWindow.GetCommits();
          this.sharedDeletedItems = AssetServer.GetLocalDeletedItems();
        }
        else
        {
          this.sharedCommits = new AssetsItem[0];
          this.sharedDeletedItems = new AssetsItem[0];
        }
      }
      this.pv.Clear();
      this.pv.AddAssetItems(this.sharedCommits);
      this.pv.AddAssetItems(this.sharedDeletedItems);
      this.pv.SetLineCount();
      this.pv.selectedItems = new bool[this.pv.lv.totalRows];
      this.pv.initialSelectedItem = -1;
      AssetServer.ClearRefreshCommit();
    }

    private void GetUpdates()
    {
      AssetServer.ClearAssetServerError();
      this.sharedChangesets = AssetServer.GetNewItems();
      Array.Reverse((Array) this.sharedChangesets);
      this.changesetContents = (GUIContent[]) null;
      this.maxNickLength = 1;
      AssetServer.ClearRefreshUpdate();
      if (!(AssetServer.GetAssetServerError() != string.Empty))
        return;
      this.sharedChangesets = (Changeset[]) null;
    }

    public void DisplayedItemsChanged()
    {
      float[] relativeSizes = new float[2];
      bool flag1 = this.sharedChangesets != null && this.sharedChangesets.Length != 0;
      bool flag2 = this.pv.lv.totalRows != 0;
      if (flag1 && flag2 || !flag1 && !flag2)
      {
        relativeSizes[0] = relativeSizes[1] = 0.5f;
      }
      else
      {
        relativeSizes[0] = !flag1 ? 0.0f : 1f;
        relativeSizes[1] = !flag2 ? 0.0f : 1f;
      }
      this.splitter = new SplitterState(relativeSizes, new int[2]{ 80, 80 }, (int[]) null);
      this.DoSelectionChange();
    }

    public void InitOverviewPage(bool lastActionsResult)
    {
      if (!lastActionsResult)
      {
        this.needsSetup = true;
        this.sharedChangesets = (Changeset[]) null;
        this.sharedCommits = (AssetsItem[]) null;
        this.sharedDeletedItems = (AssetsItem[]) null;
      }
      else
      {
        PListConfig plistConfig = new PListConfig("Library/ServerPreferences.plist");
        this.connectionString = plistConfig["Maint UserName"] + " @ " + plistConfig["Maint Server"] + " : " + plistConfig["Maint project name"];
        if (this.UpdateNeedsRefresh())
          this.GetUpdates();
        this.needsSetup = this.sharedChangesets == null || AssetServer.HasConnectionError();
        this.InitCommits();
        this.DisplayedItemsChanged();
      }
    }

    public void InitHistoryPage(bool lastActionsResult)
    {
      if (!lastActionsResult)
      {
        this.Reinit();
      }
      else
      {
        this.asHistoryWin = new ASHistoryWindow((EditorWindow) this);
        if (this.asHistoryWin != null)
          return;
        this.Reinit();
      }
    }

    private void OverviewPageGUI()
    {
      bool enabled = GUI.enabled;
      this.showSmallWindow = (double) this.position.width <= (double) this.widthToHideButtons;
      if (Event.current.type == EventType.Layout)
        this.wasHidingButtons = this.showSmallWindow;
      else if (this.showSmallWindow != this.wasHidingButtons)
        GUIUtility.ExitGUI();
      GUILayout.BeginHorizontal();
      if (!this.showSmallWindow)
      {
        GUILayout.BeginVertical();
        this.ShortServerInfo();
        if (this.needsSetup)
          GUI.enabled = false;
        this.OtherServerCommands();
        GUI.enabled = enabled;
        this.ServerAdministration();
        GUI.enabled = !this.needsSetup && enabled;
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal(GUILayout.Width((float) (((double) this.position.width - 30.0) / 2.0)));
      }
      else
        GUILayout.BeginHorizontal();
      GUI.enabled = !this.needsSetup && enabled;
      SplitterGUILayout.BeginVerticalSplit(this.splitter);
      this.ShortUpdateList();
      this.ShortCommitList();
      SplitterGUILayout.EndVerticalSplit();
      GUILayout.EndHorizontal();
      GUILayout.EndHorizontal();
      GUI.enabled = enabled;
    }

    private void OtherServerCommands()
    {
      GUILayout.BeginVertical(ASMainWindow.constants.groupBox, new GUILayoutOption[0]);
      GUILayout.Label("Asset Server Actions", ASMainWindow.constants.title, new GUILayoutOption[0]);
      GUILayout.BeginVertical(ASMainWindow.constants.contentBox, new GUILayoutOption[0]);
      if (this.WordWrappedLabelButton("Browse the complete history of the project", "Show History"))
      {
        this.SwitchSelectedPage(ASMainWindow.Page.History);
        GUIUtility.ExitGUI();
      }
      GUILayout.Space(5f);
      if (this.WordWrappedLabelButton("Discard all local changes you made to the project", "Discard Changes"))
        this.ActionDiscardChanges();
      GUILayout.EndVertical();
      GUILayout.EndVertical();
    }

    private void ShortServerInfo()
    {
      GUILayout.BeginVertical(ASMainWindow.constants.groupBox, new GUILayoutOption[0]);
      GUILayout.Label("Current Project", ASMainWindow.constants.title, new GUILayoutOption[0]);
      GUILayout.BeginVertical(ASMainWindow.constants.contentBox, new GUILayoutOption[0]);
      if (this.WordWrappedLabelButton(this.connectionString, "Connection"))
        this.SwitchSelectedPage(ASMainWindow.Page.ServerConfig);
      if (AssetServer.GetAssetServerError() != string.Empty)
      {
        GUILayout.Space(10f);
        GUILayout.Label(AssetServer.GetAssetServerError(), ASMainWindow.constants.errorLabel, new GUILayoutOption[0]);
      }
      GUILayout.EndVertical();
      GUILayout.EndVertical();
    }

    private void ServerAdministration()
    {
      GUILayout.BeginVertical(ASMainWindow.constants.groupBox, new GUILayoutOption[0]);
      GUILayout.Label("Asset Server Administration", ASMainWindow.constants.title, new GUILayoutOption[0]);
      GUILayout.BeginVertical(ASMainWindow.constants.contentBox, new GUILayoutOption[0]);
      if (this.WordWrappedLabelButton("Create and administer Asset Server projects", "Administration"))
      {
        this.SwitchSelectedPage(ASMainWindow.Page.Admin);
        GUIUtility.ExitGUI();
      }
      GUILayout.EndVertical();
      GUILayout.EndVertical();
    }

    private bool HasFlag(ChangeFlags flags, ChangeFlags flagToCheck)
    {
      return (flagToCheck & flags) != ChangeFlags.None;
    }

    private void MySelectionToGlobalSelection()
    {
      this.mySelection = true;
      List<string> viewSelectedItems = ASCommitWindow.GetParentViewSelectedItems(this.pv, true, false);
      viewSelectedItems.Remove(AssetServer.GetRootGUID());
      if (viewSelectedItems.Count > 0)
        AssetServer.SetSelectionFromGUID(viewSelectedItems[0]);
      this.pvHasSelection = this.pv.HasTrue();
      this.somethingDiscardableSelected = ASCommitWindow.SomethingDiscardableSelected(this.pv);
    }

    private void DoCommitParentView()
    {
      bool shift = Event.current.shift;
      bool actionKey = EditorGUI.actionKey;
      int row = this.pv.lv.row;
      int folder1 = -1;
      int file = -1;
      bool flag = false;
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.pv.lv, ASMainWindow.constants.background))
      {
        if (GUIUtility.keyboardControl == this.pv.lv.ID && Event.current.type == EventType.KeyDown && actionKey)
          Event.current.Use();
        if (folder1 == -1)
        {
          if (!this.pv.IndexToFolderAndFile(listViewElement.row, ref folder1, ref file))
            break;
        }
        ParentViewFolder folder2 = this.pv.folders[folder1];
        if (this.pv.selectedItems[listViewElement.row] && Event.current.type == EventType.Repaint)
          ASMainWindow.constants.entrySelected.Draw(listViewElement.position, false, false, false, false);
        if (!this.committing)
        {
          if (ListViewGUILayout.HasMouseUp(listViewElement.position))
          {
            if (!shift && !actionKey)
              flag |= ListViewGUILayout.MultiSelection(row, this.pv.lv.row, ref this.pv.initialSelectedItem, ref this.pv.selectedItems);
          }
          else if (ListViewGUILayout.HasMouseDown(listViewElement.position))
          {
            if (Event.current.clickCount == 2)
            {
              ASCommitWindow.DoShowDiff(ASCommitWindow.GetParentViewSelectedItems(this.pv, false, false), false);
              GUIUtility.ExitGUI();
            }
            else
            {
              if (!this.pv.selectedItems[listViewElement.row] || shift || actionKey)
                flag |= ListViewGUILayout.MultiSelection(row, listViewElement.row, ref this.pv.initialSelectedItem, ref this.pv.selectedItems);
              this.pv.selectedFile = file;
              this.pv.selectedFolder = folder1;
              this.pv.lv.row = listViewElement.row;
            }
          }
          else if (ListViewGUILayout.HasMouseDown(listViewElement.position, 1))
          {
            if (!this.pv.selectedItems[listViewElement.row])
            {
              flag = true;
              this.pv.ClearSelection();
              this.pv.selectedItems[listViewElement.row] = true;
              this.pv.selectedFile = file;
              this.pv.selectedFolder = folder1;
              this.pv.lv.row = listViewElement.row;
            }
            GUIUtility.hotControl = 0;
            EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), this.commitDropDownMenuItems, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.CommitContextMenuClick), (object) null);
            Event.current.Use();
          }
        }
        ChangeFlags changeFlags;
        if (file != -1)
        {
          Texture2D texture2D = AssetDatabase.GetCachedIcon(folder2.name + "/" + folder2.files[file].name) as Texture2D;
          if ((UnityEngine.Object) texture2D == (UnityEngine.Object) null)
            texture2D = InternalEditorUtility.GetIconForFile(folder2.files[file].name);
          GUILayout.Label(new GUIContent(folder2.files[file].name, (Texture) texture2D), ASMainWindow.constants.element, new GUILayoutOption[0]);
          changeFlags = folder2.files[file].changeFlags;
        }
        else
        {
          GUILayout.Label(folder2.name, ASMainWindow.constants.header, new GUILayoutOption[0]);
          changeFlags = folder2.changeFlags;
        }
        GUIContent content = (GUIContent) null;
        if (this.HasFlag(changeFlags, ChangeFlags.Undeleted) || this.HasFlag(changeFlags, ChangeFlags.Created))
          content = ASMainWindow.constants.badgeNew;
        else if (this.HasFlag(changeFlags, ChangeFlags.Deleted))
          content = ASMainWindow.constants.badgeDelete;
        else if (this.HasFlag(changeFlags, ChangeFlags.Renamed) || this.HasFlag(changeFlags, ChangeFlags.Moved))
          content = ASMainWindow.constants.badgeMove;
        if (content != null && Event.current.type == EventType.Repaint)
        {
          Rect position = new Rect((float) ((double) listViewElement.position.x + (double) listViewElement.position.width - (double) content.image.width - 5.0), listViewElement.position.y + listViewElement.position.height / 2f - (float) (content.image.height / 2), (float) content.image.width, (float) content.image.height);
          EditorGUIUtility.SetIconSize(Vector2.zero);
          GUIStyle.none.Draw(position, content, false, false, false, false);
          EditorGUIUtility.SetIconSize(this.iconSize);
        }
        this.pv.NextFileFolder(ref folder1, ref file);
      }
      if (this.committing)
        return;
      if (GUIUtility.keyboardControl == this.pv.lv.ID)
      {
        if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll")
          Event.current.Use();
        else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SelectAll")
        {
          for (int index = 0; index < this.pv.selectedItems.Length; ++index)
            this.pv.selectedItems[index] = true;
          flag = true;
          Event.current.Use();
        }
      }
      if (GUIUtility.keyboardControl == this.pv.lv.ID && !actionKey && this.pv.lv.selectionChanged)
      {
        flag |= ListViewGUILayout.MultiSelection(row, this.pv.lv.row, ref this.pv.initialSelectedItem, ref this.pv.selectedItems);
        this.pv.IndexToFolderAndFile(this.pv.lv.row, ref this.pv.selectedFolder, ref this.pv.selectedFile);
      }
      if (!this.pv.lv.selectionChanged && !flag)
        return;
      this.MySelectionToGlobalSelection();
    }

    private void DoCommit()
    {
      if (this.commitMessage == string.Empty && !EditorUtility.DisplayDialog("Commit without description", "Are you sure you want to commit with empty commit description message?", "Commit", "Cancel"))
        GUIUtility.ExitGUI();
      bool refreshCommit = AssetServer.GetRefreshCommit();
      ASCommitWindow asCommitWindow = new ASCommitWindow(this, ASCommitWindow.GetParentViewSelectedItems(this.pv, true, false).ToArray());
      asCommitWindow.InitiateReinit();
      if ((refreshCommit || asCommitWindow.lastTransferMovedDependencies) && (!refreshCommit && !EditorUtility.DisplayDialog("Committing with dependencies", "Assets selected for committing have dependencies that will also be committed. Press Details to view full changeset", "Commit", "Details") || refreshCommit))
      {
        this.committing = false;
        this.selectedPage = ASMainWindow.Page.Commit;
        asCommitWindow.description = this.commitMessage;
        if (refreshCommit)
          asCommitWindow.showReinitedWarning = 1;
        this.asCommitWin = asCommitWindow;
        this.Repaint();
        GUIUtility.ExitGUI();
      }
      else
      {
        string[] itemsToCommit = asCommitWindow.GetItemsToCommit();
        AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", "CBOverviewsCommitFinished");
        AssetServer.DoCommitOnNextTick(this.commitMessage, itemsToCommit);
        AssetServer.SetLastCommitMessage(this.commitMessage);
        asCommitWindow.AddToCommitMessageHistory(this.commitMessage);
        this.committing = false;
        GUIUtility.ExitGUI();
      }
    }

    private void StartCommitting()
    {
      this.committing = true;
      this.commitMessage = string.Empty;
      this.selectionChangedWhileCommitting = false;
      this.focusCommitMessage = true;
    }

    internal void CommitFinished(bool actionResult)
    {
      if (actionResult)
      {
        AssetServer.ClearCommitPersistentData();
        this.InitOverviewPage(true);
      }
      else
        this.Repaint();
    }

    private void CancelCommit()
    {
      this.committing = false;
      if (!this.selectionChangedWhileCommitting)
        return;
      this.DoSelectionChange();
    }

    private void DoMyRevert(bool afterMarkingDependencies)
    {
      if (!afterMarkingDependencies)
      {
        List<string> viewSelectedItems1 = ASCommitWindow.GetParentViewSelectedItems(this.pv, true, false);
        if (ASCommitWindow.MarkAllFolderDependenciesForDiscarding(this.pv, (ParentViewState) null))
        {
          this.lastRevertSelectionChanged = 2;
          this.MySelectionToGlobalSelection();
        }
        else
          this.lastRevertSelectionChanged = -1;
        List<string> viewSelectedItems2 = ASCommitWindow.GetParentViewSelectedItems(this.pv, true, false);
        if (viewSelectedItems1.Count != viewSelectedItems2.Count)
          this.lastRevertSelectionChanged = 2;
      }
      if (!afterMarkingDependencies && this.lastRevertSelectionChanged != -1)
        return;
      ASCommitWindow.DoRevert(ASCommitWindow.GetParentViewSelectedItems(this.pv, true, true), "CBInitOverviewPage");
    }

    private void ShortCommitList()
    {
      bool enabled1 = GUI.enabled;
      GUILayout.BeginVertical(!this.showSmallWindow ? ASMainWindow.constants.groupBox : ASMainWindow.constants.groupBoxNoMargin, new GUILayoutOption[0]);
      GUILayout.Label("Local Changes", ASMainWindow.constants.title, new GUILayoutOption[0]);
      if (this.pv.lv.totalRows == 0)
      {
        GUILayout.BeginVertical(ASMainWindow.constants.contentBox, GUILayout.ExpandHeight(true));
        GUILayout.Label("Nothing to commit");
        GUILayout.EndVertical();
      }
      else
      {
        this.DoCommitParentView();
        GUILayout.BeginHorizontal(ASMainWindow.constants.contentBox, new GUILayoutOption[0]);
        Event current = Event.current;
        if (!this.committing)
        {
          GUI.enabled = this.pvHasSelection && enabled1;
          if (GUILayout.Button("Compare", ASMainWindow.constants.smallButton, new GUILayoutOption[0]))
          {
            ASCommitWindow.DoShowDiff(ASCommitWindow.GetParentViewSelectedItems(this.pv, false, false), false);
            GUIUtility.ExitGUI();
          }
          bool enabled2 = GUI.enabled;
          if (!this.somethingDiscardableSelected)
            GUI.enabled = false;
          if (GUILayout.Button("Discard", ASMainWindow.constants.smallButton, new GUILayoutOption[0]))
          {
            this.DoMyRevert(false);
            GUIUtility.ExitGUI();
          }
          GUI.enabled = enabled2;
          GUILayout.FlexibleSpace();
          if (GUILayout.Button("Commit...", ASMainWindow.constants.smallButton, new GUILayoutOption[0]) || this.pvHasSelection && current.type == EventType.KeyDown && current.keyCode == KeyCode.Return)
          {
            this.StartCommitting();
            current.Use();
          }
          if (current.type == EventType.KeyDown && ((int) current.character == 10 || (int) current.character == 3))
            current.Use();
          GUI.enabled = enabled1;
          if (GUILayout.Button("Details", ASMainWindow.constants.smallButton, new GUILayoutOption[0]))
          {
            this.SwitchSelectedPage(ASMainWindow.Page.Commit);
            this.Repaint();
            GUIUtility.ExitGUI();
          }
        }
        else
        {
          if (current.type == EventType.KeyDown)
          {
            switch (current.keyCode)
            {
              case KeyCode.Return:
                this.DoCommit();
                current.Use();
                break;
              case KeyCode.Escape:
                this.CancelCommit();
                current.Use();
                break;
              default:
                if ((int) current.character == 10 || (int) current.character == 3)
                {
                  current.Use();
                  break;
                }
                break;
            }
          }
          GUI.SetNextControlName("commitMessage");
          this.commitMessage = EditorGUILayout.TextField(this.commitMessage);
          if (GUILayout.Button("Commit", ASMainWindow.constants.smallButton, new GUILayoutOption[1]
          {
            GUILayout.Width(60f)
          }))
            this.DoCommit();
          if (GUILayout.Button("Cancel", ASMainWindow.constants.smallButton, new GUILayoutOption[1]
          {
            GUILayout.Width(60f)
          }))
            this.CancelCommit();
          if (this.focusCommitMessage)
          {
            EditorGUI.FocusTextInControl("commitMessage");
            this.focusCommitMessage = false;
            this.Repaint();
          }
        }
        GUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();
      if (this.lastRevertSelectionChanged == 0)
      {
        this.lastRevertSelectionChanged = -1;
        if (ASCommitWindow.ShowDiscardWarning())
          this.DoMyRevert(true);
      }
      if (this.lastRevertSelectionChanged <= 0)
        return;
      --this.lastRevertSelectionChanged;
      this.Repaint();
    }

    private void ShortUpdateList()
    {
      GUILayout.BeginVertical(!this.showSmallWindow ? ASMainWindow.constants.groupBox : ASMainWindow.constants.groupBoxNoMargin, new GUILayoutOption[0]);
      GUILayout.Label("Updates on Server", ASMainWindow.constants.title, new GUILayoutOption[0]);
      if (this.sharedChangesets == null)
      {
        GUILayout.BeginVertical(ASMainWindow.constants.contentBox, GUILayout.ExpandHeight(true));
        GUILayout.Label("Could not retrieve changes");
        GUILayout.EndVertical();
      }
      else if (this.sharedChangesets.Length == 0)
      {
        GUILayout.BeginVertical(ASMainWindow.constants.contentBox, GUILayout.ExpandHeight(true));
        GUILayout.Label("You are up to date");
        GUILayout.EndVertical();
      }
      else
      {
        this.lv.totalRows = this.sharedChangesets.Length;
        int num = (int) ASMainWindow.constants.entryNormal.CalcHeight(new GUIContent("X"), 100f);
        ASMainWindow.constants.serverUpdateLog.alignment = TextAnchor.MiddleLeft;
        ASMainWindow.constants.serverUpdateInfo.alignment = TextAnchor.MiddleLeft;
        foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.lv, ASMainWindow.constants.background))
        {
          Rect rect = GUILayoutUtility.GetRect(GUIClip.visibleRect.width, (float) num, new GUILayoutOption[1]{ GUILayout.MinHeight((float) num) });
          Rect position = rect;
          ++position.x;
          ++position.y;
          if (listViewElement.row % 2 == 0)
          {
            if (Event.current.type == EventType.Repaint)
              ASMainWindow.constants.entryEven.Draw(position, false, false, false, false);
            position.y += rect.height;
            if (Event.current.type == EventType.Repaint)
              ASMainWindow.constants.entryOdd.Draw(position, false, false, false, false);
          }
          position = rect;
          position.width -= (float) (this.maxNickLength + 25);
          position.x += 10f;
          GUI.Button(position, this.changesetContents[listViewElement.row], ASMainWindow.constants.serverUpdateLog);
          position = rect;
          position.x += (float) ((double) position.width - (double) this.maxNickLength - 5.0);
          GUI.Label(position, this.sharedChangesets[listViewElement.row].owner, ASMainWindow.constants.serverUpdateInfo);
        }
        ASMainWindow.constants.serverUpdateLog.alignment = TextAnchor.UpperLeft;
        ASMainWindow.constants.serverUpdateInfo.alignment = TextAnchor.UpperLeft;
        GUILayout.BeginHorizontal(ASMainWindow.constants.contentBox, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Update", ASMainWindow.constants.smallButton, new GUILayoutOption[0]))
        {
          this.selectedPage = ASMainWindow.Page.Update;
          this.InitUpdatePage(true);
          this.asUpdateWin.DoUpdate(false);
        }
        if (GUILayout.Button("Details", ASMainWindow.constants.smallButton, new GUILayoutOption[0]))
        {
          this.SwitchSelectedPage(ASMainWindow.Page.Update);
          this.Repaint();
          GUIUtility.ExitGUI();
        }
        GUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();
    }

    private void DoSelectedPageGUI()
    {
      switch (this.selectedPage)
      {
        case ASMainWindow.Page.Overview:
          this.OverviewPageGUI();
          break;
        case ASMainWindow.Page.Update:
          if (this.asUpdateWin == null || this.asUpdateWin == null)
            break;
          this.asUpdateWin.DoGUI();
          break;
        case ASMainWindow.Page.Commit:
          if (this.asCommitWin == null || this.asCommitWin == null)
            break;
          this.asCommitWin.DoGUI();
          break;
        case ASMainWindow.Page.History:
          if (this.asHistoryWin == null || this.asHistoryWin.DoGUI(this.m_Parent.hasFocus))
            break;
          this.SwitchSelectedPage(ASMainWindow.Page.Overview);
          GUIUtility.ExitGUI();
          break;
        case ASMainWindow.Page.ServerConfig:
          if (this.asConfigWin == null || this.asConfigWin.DoGUI())
            break;
          this.SwitchSelectedPage(ASMainWindow.Page.Overview);
          GUIUtility.ExitGUI();
          break;
        case ASMainWindow.Page.Admin:
          if (this.asAdminWin == null || this.asAdminWin.DoGUI())
            break;
          this.SwitchSelectedPage(ASMainWindow.Page.Overview);
          GUIUtility.ExitGUI();
          break;
      }
    }

    private void SetShownSearchField(ASMainWindow.ShowSearchField newShow)
    {
      EditorGUI.FocusTextInControl("SearchFilter");
      this.m_SearchField.Show = false;
      this.m_ShowSearch = newShow;
      this.m_SearchField.Show = true;
      this.asHistoryWin.FilterItems(false);
    }

    private void DoSearchToggle(ASMainWindow.ShowSearchField field)
    {
      if (this.selectedPage != ASMainWindow.Page.History)
        return;
      if (this.m_SearchField.DoGUI())
        this.asHistoryWin.FilterItems(false);
      GUILayout.Space(10f);
    }

    private bool IsLastOne(int f, int fl, ParentViewState st)
    {
      if (st.folders.Length - 1 == f)
        return st.folders[f].files.Length - 1 == fl;
      return false;
    }

    private void OnGUI()
    {
      if (EditorSettings.externalVersionControl != ExternalVersionControl.Disabled && EditorSettings.externalVersionControl != ExternalVersionControl.AssetServer)
      {
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Asset Server is disabled when external version control is used. Go to 'Edit -> Project Settings -> Editor' to re-enable it.");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
      }
      else
      {
        if (ASMainWindow.constants == null)
          ASMainWindow.constants = new ASMainWindow.Constants();
        if (!this.m_CheckedMaint && Event.current.type != EventType.Layout)
        {
          if (!InternalEditorUtility.HasTeamLicense())
          {
            this.Close();
            GUIUtility.ExitGUI();
          }
          this.m_CheckedMaint = true;
        }
        if (this.maxNickLength == 1 && this.sharedChangesets != null)
        {
          for (int index = 0; index < this.sharedChangesets.Length; ++index)
          {
            int x = (int) ASMainWindow.constants.serverUpdateInfo.CalcSize(new GUIContent(this.sharedChangesets[index].owner)).x;
            if (x > this.maxNickLength)
              this.maxNickLength = x;
          }
          this.changesetContents = new GUIContent[this.sharedChangesets.Length];
          ParentViewState st = new ParentViewState();
          for (int index = 0; index < this.changesetContents.Length; ++index)
          {
            int num1 = 15;
            Changeset sharedChangeset = this.sharedChangesets[index];
            string str1 = sharedChangeset.message.Split('\n')[0];
            string str2 = str1.Length >= 45 ? str1.Substring(0, 42) + "..." : str1;
            string tooltip = string.Format("[{0} {1}] {2}", (object) sharedChangeset.date, (object) sharedChangeset.owner, (object) str2);
            int num2 = num1 - 1;
            st.Clear();
            st.AddAssetItems(sharedChangeset);
            for (int f = 0; f < st.folders.Length; ++f)
            {
              if (--num2 == 0 && !this.IsLastOne(f, 0, st))
              {
                tooltip += "\n(and more...)";
                break;
              }
              tooltip = tooltip + "\n" + st.folders[f].name;
              for (int fl = 0; fl < st.folders[f].files.Length; ++fl)
              {
                if (--num2 == 0 && !this.IsLastOne(f, fl, st))
                {
                  tooltip += "\n(and more...)";
                  break;
                }
                tooltip = tooltip + "\n\t" + st.folders[f].files[fl].name;
              }
              if (num2 == 0)
                break;
            }
            this.changesetContents[index] = new GUIContent(this.sharedChangesets[index].message.Split('\n')[0], tooltip);
          }
          if (this.maxNickLength == 1)
            this.maxNickLength = 0;
        }
        if (AssetServer.IsControllerBusy() != 0)
        {
          this.Repaint();
        }
        else
        {
          if (this.isInitialUpdate)
          {
            this.isInitialUpdate = false;
            this.SwitchSelectedPage(ASMainWindow.Page.Overview);
          }
          if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "Find")
          {
            this.SetShownSearchField(this.m_SearchToShow);
            Event.current.Use();
          }
          GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
          int num = -1;
          bool enabled = GUI.enabled;
          if (this.ToolbarToggle(this.selectedPage == ASMainWindow.Page.Overview, this.pageTitles[0], EditorStyles.toolbarButton))
            num = 0;
          GUI.enabled = !this.needsSetup && this.sharedChangesets != null && this.sharedChangesets.Length != 0 && enabled;
          if (this.ToolbarToggle(this.selectedPage == ASMainWindow.Page.Update, this.pageTitles[1], EditorStyles.toolbarButton))
            num = 1;
          GUI.enabled = !this.needsSetup && this.pv.lv.totalRows != 0 && enabled;
          if (this.selectedPage > ASMainWindow.Page.Commit)
          {
            if (this.ToolbarToggle(this.selectedPage == ASMainWindow.Page.Commit, this.pageTitles[2], EditorStyles.toolbarButton))
              num = 2;
            GUI.enabled = enabled;
            if (this.ToolbarToggle(this.selectedPage > ASMainWindow.Page.Commit, this.pageTitles[3], EditorStyles.toolbarButton))
              num = 3;
          }
          else
          {
            if (this.ToolbarToggle(this.selectedPage == ASMainWindow.Page.Commit, this.pageTitles[2], EditorStyles.toolbarButton))
              num = 2;
            GUI.enabled = enabled;
          }
          if (num != -1 && (ASMainWindow.Page) num != this.selectedPage)
          {
            if (this.selectedPage == ASMainWindow.Page.Commit)
              this.NotifyClosingCommit();
            if (num <= 2)
            {
              this.SwitchSelectedPage((ASMainWindow.Page) num);
              GUIUtility.ExitGUI();
            }
          }
          GUILayout.FlexibleSpace();
          if (this.selectedPage == ASMainWindow.Page.History)
            this.DoSearchToggle(ASMainWindow.ShowSearchField.HistoryList);
          if (!this.needsSetup)
          {
            switch (this.selectedPage)
            {
              case ASMainWindow.Page.Overview:
              case ASMainWindow.Page.Update:
              case ASMainWindow.Page.History:
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, new GUILayoutOption[0]))
                {
                  this.ActionRefresh();
                  GUIUtility.ExitGUI();
                  break;
                }
                break;
            }
          }
          GUILayout.EndHorizontal();
          EditorGUIUtility.SetIconSize(this.iconSize);
          this.DoSelectedPageGUI();
          EditorGUIUtility.SetIconSize(Vector2.zero);
          if (Event.current.type != EventType.ContextClick)
            return;
          GUIUtility.hotControl = 0;
          EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), !this.needsSetup ? this.dropDownMenuItems : this.unconfiguredDropDownMenuItems, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), (object) null);
          Event.current.Use();
        }
      }
    }

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
    }

    internal class Constants
    {
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle contentBox = (GUIStyle) "GroupBox";
      public GUIStyle entrySelected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle entryNormal = (GUIStyle) "ServerUpdateChangeset";
      public GUIStyle element = (GUIStyle) "OL elem";
      public GUIStyle header = (GUIStyle) "OL header";
      public GUIStyle title = (GUIStyle) "OL Title";
      public GUIStyle columnHeader = (GUIStyle) "OL Title";
      public GUIStyle serverUpdateLog = (GUIStyle) "ServerUpdateLog";
      public GUIStyle serverUpdateInfo = (GUIStyle) "ServerUpdateInfo";
      public GUIStyle smallButton = (GUIStyle) "Button";
      public GUIStyle errorLabel = (GUIStyle) "ErrorLabel";
      public GUIStyle miniButton = (GUIStyle) "MiniButton";
      public GUIStyle button = (GUIStyle) "Button";
      public GUIStyle largeButton = (GUIStyle) "ButtonMid";
      public GUIStyle bigButton = (GUIStyle) "LargeButton";
      public GUIStyle entryEven = (GUIStyle) "CN EntryBackEven";
      public GUIStyle entryOdd = (GUIStyle) "CN EntryBackOdd";
      public GUIStyle dropDown = (GUIStyle) "MiniPullDown";
      public GUIStyle toggle = (GUIStyle) "Toggle";
      public GUIContent badgeDelete = EditorGUIUtility.IconContent("AS Badge Delete");
      public GUIContent badgeMove = EditorGUIUtility.IconContent("AS Badge Move");
      public GUIContent badgeNew = EditorGUIUtility.IconContent("AS Badge New");
      public GUIStyle groupBox;
      public GUIStyle groupBoxNoMargin;
      public Vector2 toggleSize;

      public Constants()
      {
        this.groupBoxNoMargin = new GUIStyle();
        this.groupBox = new GUIStyle();
        this.groupBox.margin = new RectOffset(10, 10, 10, 10);
        this.contentBox = new GUIStyle(this.contentBox);
        this.contentBox.margin = new RectOffset(0, 0, 0, 0);
        this.contentBox.overflow = new RectOffset(0, 1, 0, 1);
        this.contentBox.padding = new RectOffset(8, 8, 7, 7);
        this.title = new GUIStyle(this.title);
        RectOffset padding = this.title.padding;
        int num1 = this.contentBox.padding.left + 2;
        this.title.padding.right = num1;
        int num2 = num1;
        padding.left = num2;
        this.background = new GUIStyle(this.background);
        this.background.padding.top = 1;
      }
    }

    public enum ShowSearchField
    {
      None,
      ProjectView,
      HistoryList,
    }

    internal enum Page
    {
      NotInitialized = -1,
      Overview = 0,
      Update = 1,
      Commit = 2,
      History = 3,
      ServerConfig = 4,
      Admin = 5,
    }

    [Serializable]
    public class SearchField
    {
      private string m_FilterText = string.Empty;
      private bool m_Show;

      public string FilterText
      {
        get
        {
          return this.m_FilterText;
        }
      }

      public bool Show
      {
        get
        {
          return this.m_Show;
        }
        set
        {
          this.m_Show = value;
        }
      }

      public bool DoGUI()
      {
        GUI.SetNextControlName("SearchFilter");
        string str = EditorGUILayout.ToolbarSearchField(this.m_FilterText);
        if (!(this.m_FilterText != str))
          return false;
        this.m_FilterText = str;
        return true;
      }
    }
  }
}
