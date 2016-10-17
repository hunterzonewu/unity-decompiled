// Decompiled with JetBrains decompiler
// Type: UnityEditor.ASCommitWindow
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
  internal class ASCommitWindow
  {
    private ParentViewState pv1state = new ParentViewState();
    private ParentViewState pv2state = new ParentViewState();
    private string[] dropDownMenuItems = new string[6]
    {
      string.Empty,
      string.Empty,
      "Compare",
      "Compare Binary",
      string.Empty,
      "Discard"
    };
    private string dragTitle = string.Empty;
    private Vector2 iconSize = new Vector2(16f, 16f);
    private SplitterState horSplit = new SplitterState(new float[2]{ 50f, 50f }, new int[2]{ 50, 50 }, (int[]) null);
    private SplitterState vertSplit = new SplitterState(new float[2]{ 60f, 30f }, new int[2]{ 32, 64 }, (int[]) null);
    internal string description = string.Empty;
    private Vector2 scrollPos = Vector2.zero;
    internal int lastRevertSelectionChanged = -1;
    internal int showReinitedWarning = -1;
    private const int listLenghts = 20;
    private const int widthToHideButtons = 432;
    private bool wasHidingButtons;
    private bool resetKeyboardControl;
    private static ASCommitWindow.Constants constants;
    private bool pv1hasSelection;
    private bool pv2hasSelection;
    private bool somethingDiscardableSelected;
    private bool mySelection;
    private string[] commitMessageList;
    private string[] guidsToTransferToTheRightSide;
    private string totalChanges;
    private ASMainWindow parentWin;
    private bool initialUpdate;
    internal bool lastTransferMovedDependencies;
    private static List<string> s_AssetGuids;
    private static string s_Callback;

    public ASCommitWindow(ASMainWindow parentWin, string[] guidsToTransfer)
    {
      this.guidsToTransferToTheRightSide = guidsToTransfer;
      this.parentWin = parentWin;
      this.initialUpdate = true;
    }

    internal void AssetItemsToParentViews()
    {
      this.pv1state.Clear();
      this.pv2state.Clear();
      this.pv1state.AddAssetItems(this.parentWin.sharedCommits);
      this.pv1state.AddAssetItems(this.parentWin.sharedDeletedItems);
      this.pv1state.lv = new ListViewState(0);
      this.pv2state.lv = new ListViewState(0);
      this.pv1state.SetLineCount();
      this.pv2state.SetLineCount();
      if (this.pv1state.lv.totalRows == 0)
      {
        this.parentWin.Reinit();
      }
      else
      {
        this.pv1state.selectedItems = new bool[this.pv1state.lv.totalRows];
        this.pv2state.selectedItems = new bool[this.pv1state.lv.totalRows];
        int num = 0;
        for (int index = 0; index < this.parentWin.sharedCommits.Length; ++index)
        {
          if (this.parentWin.sharedCommits[index].assetIsDir != 0)
            ++num;
        }
        for (int index = 0; index < this.parentWin.sharedDeletedItems.Length; ++index)
        {
          if (this.parentWin.sharedDeletedItems[index].assetIsDir != 0)
            ++num;
        }
        this.totalChanges = (this.pv1state.lv.totalRows - this.pv1state.GetFoldersCount() + num).ToString() + " Local Changes";
        this.GetPersistedData();
      }
    }

    internal void Reinit(bool lastActionsResult)
    {
      this.parentWin.sharedCommits = ASCommitWindow.GetCommits();
      this.parentWin.sharedDeletedItems = AssetServer.GetLocalDeletedItems();
      AssetServer.ClearRefreshCommit();
      this.AssetItemsToParentViews();
    }

    internal void Update()
    {
      this.SetPersistedData();
      this.AssetItemsToParentViews();
      this.GetPersistedData();
    }

    internal void CommitFinished(bool actionResult)
    {
      if (actionResult)
      {
        AssetServer.ClearCommitPersistentData();
        this.parentWin.Reinit();
      }
      else
        this.parentWin.Repaint();
    }

    internal void InitiateReinit()
    {
      if (this.parentWin.CommitNeedsRefresh())
      {
        if (!this.initialUpdate)
          this.SetPersistedData();
        else
          this.initialUpdate = false;
        this.Reinit(true);
      }
      else if (this.initialUpdate)
      {
        this.AssetItemsToParentViews();
        this.initialUpdate = false;
      }
      else
      {
        this.SetPersistedData();
        AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", "CBReinitCommitWindow");
        AssetServer.DoRefreshAssetsOnNextTick();
      }
    }

    private void GetPersistedData()
    {
      this.description = AssetServer.GetLastCommitMessage();
      string[] strArray;
      if (this.guidsToTransferToTheRightSide != null && this.guidsToTransferToTheRightSide.Length != 0)
      {
        strArray = this.guidsToTransferToTheRightSide;
        this.guidsToTransferToTheRightSide = (string[]) null;
      }
      else
        strArray = AssetServer.GetCommitSelectionGUIDs();
      int num = 0;
      foreach (ParentViewFolder folder in this.pv1state.folders)
      {
        this.pv1state.selectedItems[num++] = strArray.Contains((object) folder.guid) && AssetServer.IsGUIDValid(folder.guid) != 0;
        foreach (ParentViewFile file in folder.files)
          this.pv1state.selectedItems[num++] = strArray.Contains((object) file.guid) && AssetServer.IsGUIDValid(file.guid) != 0;
      }
      this.DoTransferAll(this.pv1state, this.pv2state, this.pv1state.selectedFolder, this.pv1state.selectedFile);
      this.commitMessageList = InternalEditorUtility.GetEditorSettingsList("ASCommitMsgs", 20);
      for (int index = 0; index < this.commitMessageList.Length; ++index)
        this.commitMessageList[index] = this.commitMessageList[index].Replace('/', '?').Replace('%', '?');
    }

    private void SetPersistedData()
    {
      AssetServer.SetLastCommitMessage(this.description);
      this.AddToCommitMessageHistory(this.description);
      List<string> stringList = new List<string>();
      foreach (ParentViewFolder folder in this.pv2state.folders)
      {
        if (AssetServer.IsGUIDValid(folder.guid) != 0)
          stringList.Add(folder.guid);
        foreach (ParentViewFile file in folder.files)
        {
          if (AssetServer.IsGUIDValid(file.guid) != 0)
            stringList.Add(file.guid);
        }
      }
      AssetServer.SetCommitSelectionGUIDs(stringList.ToArray());
    }

    internal void OnClose()
    {
      this.SetPersistedData();
    }

    private List<string> GetSelectedItems()
    {
      this.pv1hasSelection = this.pv1state.HasTrue();
      this.pv2hasSelection = this.pv2state.HasTrue();
      List<string> viewSelectedItems = ASCommitWindow.GetParentViewSelectedItems(!this.pv2hasSelection ? this.pv1state : this.pv2state, true, false);
      viewSelectedItems.Remove(AssetServer.GetRootGUID());
      return viewSelectedItems;
    }

    private void MySelectionToGlobalSelection()
    {
      this.mySelection = true;
      this.somethingDiscardableSelected = ASCommitWindow.SomethingDiscardableSelected(!this.pv2hasSelection ? this.pv1state : this.pv2state);
      List<string> selectedItems = this.GetSelectedItems();
      if (selectedItems.Count <= 0)
        return;
      AssetServer.SetSelectionFromGUID(selectedItems[0]);
    }

    internal static bool DoShowDiff(List<string> selectedAssets, bool binary)
    {
      List<string> stringList = new List<string>();
      List<CompareInfo> compareInfoList = new List<CompareInfo>();
      for (int index = 0; index < selectedAssets.Count; ++index)
      {
        int ver2 = -1;
        int workingItemChangeset = AssetServer.GetWorkingItemChangeset(selectedAssets[index]);
        int ver1 = AssetServer.GetServerItemChangeset(selectedAssets[index], workingItemChangeset);
        if (AssetServer.IsItemDeleted(selectedAssets[index]))
          ver2 = -2;
        if (ver1 == -1)
          ver1 = -2;
        stringList.Add(selectedAssets[index]);
        compareInfoList.Add(new CompareInfo(ver1, ver2, !binary ? 0 : 1, !binary ? 1 : 0));
      }
      if (stringList.Count == 0)
        return false;
      AssetServer.CompareFiles(stringList.ToArray(), compareInfoList.ToArray());
      return true;
    }

    internal static bool IsDiscardableAsset(string guid, ChangeFlags changeFlags)
    {
      if (AssetServer.IsConstantGUID(guid) == 0)
        return true;
      if (!ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Created))
        return !ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Undeleted);
      return false;
    }

    internal static List<string> GetParentViewSelectedItems(ParentViewState state, bool includeFolders, bool excludeUndiscardableOnes)
    {
      List<string> stringList = new List<string>();
      int index1 = 0;
      for (int index2 = 0; index2 < state.folders.Length; ++index2)
      {
        ParentViewFolder folder = state.folders[index2];
        bool flag1 = true;
        bool flag2 = true;
        int index3 = index1++;
        int count = stringList.Count;
        for (int index4 = 0; index4 < folder.files.Length; ++index4)
        {
          if (state.selectedItems[index1])
          {
            if (!excludeUndiscardableOnes || ASCommitWindow.IsDiscardableAsset(folder.files[index4].guid, folder.files[index4].changeFlags))
            {
              stringList.Add(folder.files[index4].guid);
              flag1 = false;
            }
          }
          else
            flag2 = false;
          ++index1;
        }
        if (includeFolders && state.selectedItems[index3] && (flag1 || flag2) && (AssetServer.IsGUIDValid(folder.guid) != 0 && count <= stringList.Count))
          stringList.Insert(count, folder.guid);
      }
      return stringList;
    }

    internal static void DoRevert(List<string> assetGuids, string callback)
    {
      if (assetGuids.Count == 0)
        return;
      ASCommitWindow.s_AssetGuids = assetGuids;
      ASCommitWindow.s_Callback = callback;
      AssetServer.SetAfterActionFinishedCallback("ASCommitWindow", "DoRevertAfterDialog");
      AssetServer.ShowDialogOnNextTick("Discard changes", "Are you really sure you want to discard selected changes?", "Discard", "Cancel");
    }

    internal static void DoRevertAfterDialog(bool result)
    {
      if (!result)
        return;
      AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", ASCommitWindow.s_Callback);
      AssetServer.DoUpdateWithoutConflictResolutionOnNextTick(ASCommitWindow.s_AssetGuids.ToArray());
    }

    internal static bool MarkSelected(ParentViewState activeState, List<string> guids)
    {
      int num = 0;
      bool flag1 = false;
      foreach (ParentViewFolder folder in activeState.folders)
      {
        bool flag2 = guids.Contains(folder.guid);
        activeState.selectedItems[num++] = flag2;
        flag1 |= flag2;
        foreach (ParentViewFile file in folder.files)
        {
          bool flag3 = guids.Contains(file.guid);
          activeState.selectedItems[num++] = flag3;
          flag1 |= flag3;
        }
      }
      return flag1;
    }

    internal static AssetsItem[] GetCommits()
    {
      return AssetServer.GetChangedAssetsItems();
    }

    internal void AddToCommitMessageHistory(string description)
    {
      if (!(description.Trim() != string.Empty))
        return;
      if (ArrayUtility.Contains<string>(this.commitMessageList, description))
        ArrayUtility.Remove<string>(ref this.commitMessageList, description);
      ArrayUtility.Insert<string>(ref this.commitMessageList, 0, description);
      InternalEditorUtility.SaveEditorSettingsList("ASCommitMsgs", this.commitMessageList, 20);
    }

    internal static bool ShowDiscardWarning()
    {
      return EditorUtility.DisplayDialog("Discard changes", "More items will be discarded then initially selected. Dependencies of selected items where all marked in commit window. Please review.", "Discard", "Cancel");
    }

    internal bool CanCommit()
    {
      return this.pv2state.folders.Length != 0;
    }

    internal string[] GetItemsToCommit()
    {
      List<string> stringList = new List<string>();
      for (int index1 = 0; index1 < this.pv2state.folders.Length; ++index1)
      {
        ParentViewFolder folder = this.pv2state.folders[index1];
        if (AssetServer.IsGUIDValid(folder.guid) != 0)
          stringList.Add(folder.guid);
        for (int index2 = 0; index2 < folder.files.Length; ++index2)
        {
          if (AssetServer.IsGUIDValid(folder.files[index2].guid) != 0)
            stringList.Add(folder.files[index2].guid);
        }
      }
      return stringList.ToArray();
    }

    internal void DoCommit()
    {
      if (AssetServer.GetRefreshCommit())
      {
        this.SetPersistedData();
        this.InitiateReinit();
        this.showReinitedWarning = 2;
        this.parentWin.Repaint();
        GUIUtility.ExitGUI();
      }
      if (this.description == string.Empty && !EditorUtility.DisplayDialog("Commit without description", "Are you sure you want to commit with empty commit description message?", "Commit", "Cancel"))
        return;
      string[] itemsToCommit = this.GetItemsToCommit();
      this.SetPersistedData();
      AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", "CBCommitFinished");
      AssetServer.DoCommitOnNextTick(this.description, itemsToCommit);
      GUIUtility.ExitGUI();
    }

    private bool TransferDependentParentFolders(ref List<string> guidsOfFoldersToRemove, string guid, bool leftToRight)
    {
      bool flag = false;
      if (leftToRight)
      {
        while (AssetServer.IsGUIDValid(guid = AssetServer.GetParentGUID(guid, -1)) != 0)
        {
          if (!ASCommitWindow.AllFolderWouldBeMovedAnyway(!leftToRight ? this.pv2state : this.pv1state, guid))
          {
            int index = ASCommitWindow.IndexOfFolderWithGUID(this.pv1state.folders, guid);
            int num = ASCommitWindow.IndexOfFolderWithGUID(this.pv2state.folders, guid);
            if ((index != -1 || num != -1) && (index != -1 && num == -1))
            {
              ChangeFlags changeFlags = this.pv1state.folders[index].changeFlags;
              if (ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Undeleted) || ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Created) || ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Moved))
              {
                ArrayUtility.Add<ParentViewFolder>(ref this.pv2state.folders, this.pv1state.folders[index].CloneWithoutFiles());
                flag = true;
                if (this.pv1state.folders[index].files.Length == 0)
                  this.AddFolderToRemove(ref guidsOfFoldersToRemove, this.pv1state.folders[index].guid);
              }
            }
          }
        }
      }
      else
      {
        ChangeFlags changeFlags = this.pv1state.folders[ASCommitWindow.IndexOfFolderWithGUID(this.pv1state.folders, guid)].changeFlags;
        if (!ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Undeleted) && !ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Created) && !ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Moved))
          return false;
        for (int index1 = 0; index1 < this.pv2state.folders.Length; ++index1)
        {
          string guid1 = this.pv2state.folders[index1].guid;
          if (AssetServer.GetParentGUID(guid1, -1) == guid)
          {
            int index2 = ASCommitWindow.IndexOfFolderWithGUID(this.pv1state.folders, guid1);
            if (index2 != -1)
              ArrayUtility.AddRange<ParentViewFile>(ref this.pv1state.folders[index2].files, this.pv2state.folders[index1].files);
            else
              ArrayUtility.Add<ParentViewFolder>(ref this.pv1state.folders, this.pv2state.folders[index1]);
            this.AddFolderToRemove(ref guidsOfFoldersToRemove, guid1);
            this.TransferDependentParentFolders(ref guidsOfFoldersToRemove, guid1, leftToRight);
            flag = true;
          }
        }
      }
      return flag;
    }

    private bool TransferDeletedDependentParentFolders(ref List<string> guidsOfFoldersToRemove, string guid, bool leftToRight)
    {
      bool flag1 = false;
      ParentViewState pvState = !leftToRight ? this.pv2state : this.pv1state;
      ParentViewState parentViewState = !leftToRight ? this.pv1state : this.pv2state;
      if (leftToRight)
      {
        for (int index1 = 0; index1 < pvState.folders.Length; ++index1)
        {
          ParentViewFolder folder = pvState.folders[index1];
          if (AssetServer.GetParentGUID(folder.guid, -1) == guid && !ASCommitWindow.AllFolderWouldBeMovedAnyway(pvState, folder.guid))
          {
            if (!ASCommitWindow.HasFlag(folder.changeFlags, ChangeFlags.Deleted))
            {
              Debug.LogError((object) ("Folder of nested deleted folders marked as not deleted (" + folder.name + ")"));
              return false;
            }
            for (int index2 = 0; index2 < folder.files.Length; ++index2)
            {
              if (!ASCommitWindow.HasFlag(folder.files[index2].changeFlags, ChangeFlags.Deleted))
              {
                Debug.LogError((object) ("File of nested deleted folder is marked as not deleted (" + folder.files[index2].name + ")"));
                return false;
              }
            }
            bool flag2 = flag1 | this.TransferDeletedDependentParentFolders(ref guidsOfFoldersToRemove, folder.guid, leftToRight);
            if (ASCommitWindow.IndexOfFolderWithGUID(parentViewState.folders, folder.guid) == -1)
              ArrayUtility.Add<ParentViewFolder>(ref parentViewState.folders, folder);
            this.AddFolderToRemove(ref guidsOfFoldersToRemove, folder.guid);
            flag1 = true;
          }
        }
      }
      else
      {
        while (AssetServer.IsGUIDValid(guid = AssetServer.GetParentGUID(guid, -1)) != 0)
        {
          int index = ASCommitWindow.IndexOfFolderWithGUID(this.pv2state.folders, guid);
          if (index != -1)
          {
            if (ASCommitWindow.HasFlag(this.pv2state.folders[index].changeFlags, ChangeFlags.Deleted))
            {
              ArrayUtility.Add<ParentViewFolder>(ref this.pv1state.folders, this.pv2state.folders[index]);
              flag1 = true;
              this.AddFolderToRemove(ref guidsOfFoldersToRemove, this.pv2state.folders[index].guid);
            }
          }
          else
            break;
        }
      }
      return flag1;
    }

    private bool DoTransfer(ref ParentViewFolder[] foldersFrom, ref ParentViewFolder[] foldersTo, int folder, int file, ref List<string> guidsOfFoldersToRemove, bool leftToRight)
    {
      ParentViewFolder parentViewFolder1 = foldersFrom[folder];
      ParentViewFolder parentViewFolder2 = (ParentViewFolder) null;
      string name = parentViewFolder1.name;
      bool flag1 = false;
      bool flag2 = false;
      if (file == -1)
      {
        this.AddFolderToRemove(ref guidsOfFoldersToRemove, foldersFrom[folder].guid);
        int index = ParentViewState.IndexOf(foldersTo, name);
        if (index != -1)
        {
          parentViewFolder2 = foldersTo[index];
          ArrayUtility.AddRange<ParentViewFile>(ref parentViewFolder2.files, parentViewFolder1.files);
        }
        else
        {
          ArrayUtility.Add<ParentViewFolder>(ref foldersTo, parentViewFolder1);
          flag2 = true;
          flag1 = ASCommitWindow.HasFlag(parentViewFolder1.changeFlags, ChangeFlags.Deleted) ? this.TransferDeletedDependentParentFolders(ref guidsOfFoldersToRemove, parentViewFolder1.guid, leftToRight) : this.TransferDependentParentFolders(ref guidsOfFoldersToRemove, parentViewFolder1.guid, leftToRight);
        }
      }
      else
      {
        int index = ParentViewState.IndexOf(foldersTo, name);
        if (index == -1)
        {
          if (ASCommitWindow.HasFlag(parentViewFolder1.files[file].changeFlags, ChangeFlags.Deleted) && ASCommitWindow.HasFlag(parentViewFolder1.changeFlags, ChangeFlags.Deleted))
          {
            ArrayUtility.Add<ParentViewFolder>(ref foldersTo, parentViewFolder1);
            this.AddFolderToRemove(ref guidsOfFoldersToRemove, parentViewFolder1.guid);
            index = foldersTo.Length - 1;
            if (!ASCommitWindow.AllFolderWouldBeMovedAnyway(!leftToRight ? this.pv2state : this.pv1state, parentViewFolder1.guid))
              flag1 = true;
            flag1 |= this.TransferDeletedDependentParentFolders(ref guidsOfFoldersToRemove, parentViewFolder1.guid, leftToRight);
          }
          else
          {
            ArrayUtility.Add<ParentViewFolder>(ref foldersTo, parentViewFolder1.CloneWithoutFiles());
            index = foldersTo.Length - 1;
            flag1 = this.TransferDependentParentFolders(ref guidsOfFoldersToRemove, parentViewFolder1.guid, leftToRight);
          }
          flag2 = true;
        }
        parentViewFolder2 = foldersTo[index];
        ArrayUtility.Add<ParentViewFile>(ref parentViewFolder2.files, parentViewFolder1.files[file]);
        ArrayUtility.RemoveAt<ParentViewFile>(ref parentViewFolder1.files, file);
        if (parentViewFolder1.files.Length == 0)
          this.AddFolderToRemove(ref guidsOfFoldersToRemove, foldersFrom[folder].guid);
      }
      if (parentViewFolder2 != null)
        Array.Sort<ParentViewFile>(parentViewFolder2.files, new Comparison<ParentViewFile>(ParentViewState.CompareViewFile));
      if (flag2)
        Array.Sort<ParentViewFolder>(foldersTo, new Comparison<ParentViewFolder>(ParentViewState.CompareViewFolder));
      return flag1;
    }

    private bool MarkDependantFiles(ParentViewState pvState)
    {
      string[] strArray1 = new string[0];
      bool flag = false;
      if (pvState == this.pv1state)
      {
        string[] strArray2 = AssetServer.CollectAllDependencies(ASCommitWindow.GetParentViewSelectedItems(this.pv1state, false, false).ToArray());
        if (strArray2.Length != 0)
        {
          int index1 = 1;
          int index2 = 0;
          for (; index1 < pvState.lv.totalRows; ++index1)
          {
            int index3 = 0;
            while (index3 < pvState.folders[index2].files.Length)
            {
              if (!pvState.selectedItems[index1])
              {
                for (int index4 = 0; index4 < strArray2.Length; ++index4)
                {
                  if (strArray2[index4] == pvState.folders[index2].files[index3].guid)
                  {
                    pvState.selectedItems[index1] = true;
                    flag = true;
                    break;
                  }
                }
              }
              ++index3;
              ++index1;
            }
            ++index2;
          }
        }
      }
      return flag;
    }

    private void DoTransferAll(ParentViewState pvState, ParentViewState anotherPvState, int selFolder, int selFile)
    {
      List<string> guidsOfFoldersToRemove = new List<string>();
      bool flag1 = this.MarkDependantFiles(pvState);
      int index1 = pvState.lv.totalRows - 1;
      for (int folder1 = pvState.folders.Length - 1; folder1 >= 0; --folder1)
      {
        ParentViewFolder folder2 = pvState.folders[folder1];
        bool flag2 = false;
        for (int file = folder2.files.Length - 1; file >= -1; --file)
        {
          if (!guidsOfFoldersToRemove.Contains(folder2.guid) && pvState.selectedItems[index1])
          {
            if (file != -1 || !flag2)
              flag1 |= this.DoTransfer(ref pvState.folders, ref anotherPvState.folders, folder1, file, ref guidsOfFoldersToRemove, pvState == this.pv1state);
            flag2 = true;
          }
          --index1;
        }
      }
      for (int index2 = pvState.folders.Length - 1; index2 >= 0; --index2)
      {
        if (guidsOfFoldersToRemove.Contains(pvState.folders[index2].guid))
        {
          guidsOfFoldersToRemove.Remove(pvState.folders[index2].guid);
          ArrayUtility.RemoveAt<ParentViewFolder>(ref pvState.folders, index2);
        }
      }
      this.pv1state.SetLineCount();
      this.pv2state.SetLineCount();
      this.pv1state.ClearSelection();
      this.pv2state.ClearSelection();
      pvState.selectedFile = -1;
      pvState.selectedFolder = -1;
      AssetServer.SetSelectionFromGUID(string.Empty);
      this.lastTransferMovedDependencies = flag1;
    }

    private static bool AnyOfTheParentsIsSelected(ref ParentViewState pvState, string guid)
    {
      string str = guid;
      while (AssetServer.IsGUIDValid(str = AssetServer.GetParentGUID(str, -1)) != 0)
      {
        if (ASCommitWindow.AllFolderWouldBeMovedAnyway(pvState, str))
          return true;
      }
      return false;
    }

    public static bool MarkAllFolderDependenciesForDiscarding(ParentViewState pvState, ParentViewState anotherPvState)
    {
      bool flag1 = false;
      bool flag2 = false;
      int index1 = 0;
      List<string> stringList = new List<string>();
      for (int index2 = 0; index2 < pvState.folders.Length; ++index2)
      {
        ParentViewFolder folder = pvState.folders[index2];
        if (ASCommitWindow.HasFlag(folder.changeFlags, ChangeFlags.Deleted))
        {
          bool flag3 = false;
          for (int index3 = 1; index3 <= folder.files.Length; ++index3)
          {
            if (pvState.selectedItems[index1 + index3])
            {
              flag3 = true;
              pvState.selectedItems[index1] = true;
              stringList.Add(folder.guid);
              break;
            }
          }
          if (pvState.selectedItems[index1] || flag3)
          {
            string str = folder.guid;
            while (AssetServer.IsGUIDValid(str = AssetServer.GetParentGUID(str, -1)) != 0)
            {
              int folderIndex = ASCommitWindow.IndexOfFolderWithGUID(pvState.folders, str);
              if (folderIndex != -1)
              {
                int totalIndex = ASCommitWindow.FolderIndexToTotalIndex(pvState.folders, folderIndex);
                if (!pvState.selectedItems[totalIndex] && ASCommitWindow.HasFlag(pvState.folders[totalIndex].changeFlags, ChangeFlags.Deleted))
                {
                  pvState.selectedItems[totalIndex] = true;
                  stringList.Add(str);
                  flag1 = true;
                }
              }
              else
                break;
            }
          }
        }
        else if (!ASCommitWindow.AllFolderWouldBeMovedAnyway(pvState, folder.guid))
        {
          if (ASCommitWindow.AnyOfTheParentsIsSelected(ref pvState, folder.guid))
          {
            pvState.selectedItems[index1] = true;
            stringList.Add(folder.guid);
            for (int index3 = 1; index3 <= folder.files.Length; ++index3)
              pvState.selectedItems[index1 + index3] = true;
            flag1 = true;
          }
        }
        else
        {
          for (int index3 = 1; index3 <= folder.files.Length; ++index3)
          {
            if (!pvState.selectedItems[index1 + index3])
              pvState.selectedItems[index1 + index3] = true;
          }
          stringList.Add(folder.guid);
        }
        index1 += 1 + pvState.folders[index2].files.Length;
      }
      if (anotherPvState != null)
      {
        for (int index2 = 0; index2 < anotherPvState.folders.Length; ++index2)
        {
          ParentViewFolder folder = anotherPvState.folders[index2];
          if (ASCommitWindow.AnyOfTheParentsIsSelected(ref pvState, folder.guid))
            stringList.Add(folder.guid);
        }
        for (int index2 = anotherPvState.folders.Length - 1; index2 >= 0; --index2)
        {
          if (stringList.Contains(anotherPvState.folders[index2].guid))
          {
            ParentViewFolder folder1 = anotherPvState.folders[index2];
            int index3 = ASCommitWindow.FolderSelectionIndexFromGUID(pvState.folders, folder1.guid);
            if (index3 != -1)
            {
              ParentViewFolder folder2 = pvState.folders[index3];
              int length = pvState.lv.totalRows - index3 - 1 - folder2.files.Length;
              int sourceIndex = index3 + 1 + folder2.files.Length;
              Array.Copy((Array) pvState.selectedItems, sourceIndex, (Array) pvState.selectedItems, sourceIndex + folder1.files.Length, length);
              ArrayUtility.AddRange<ParentViewFile>(ref folder2.files, folder1.files);
              for (int index4 = 1; index4 <= folder2.files.Length; ++index4)
                pvState.selectedItems[index3 + index4] = true;
              Array.Sort<ParentViewFile>(folder2.files, new Comparison<ParentViewFile>(ParentViewState.CompareViewFile));
            }
            else
            {
              int index4 = 0;
              for (int index5 = 0; index5 < pvState.folders.Length && ParentViewState.CompareViewFolder(pvState.folders[index4], folder1) <= 0; ++index5)
                index4 += 1 + pvState.folders[index5].files.Length;
              int length = pvState.lv.totalRows - index4;
              int sourceIndex = index4;
              Array.Copy((Array) pvState.selectedItems, sourceIndex, (Array) pvState.selectedItems, sourceIndex + 1 + folder1.files.Length, length);
              ArrayUtility.Add<ParentViewFolder>(ref pvState.folders, folder1);
              for (int index5 = 0; index5 <= folder1.files.Length; ++index5)
                pvState.selectedItems[index4 + index5] = true;
              flag2 = true;
            }
            ArrayUtility.RemoveAt<ParentViewFolder>(ref anotherPvState.folders, index2);
            flag1 = true;
          }
        }
        anotherPvState.SetLineCount();
      }
      pvState.SetLineCount();
      if (flag2)
        Array.Sort<ParentViewFolder>(pvState.folders, new Comparison<ParentViewFolder>(ParentViewState.CompareViewFolder));
      return flag1;
    }

    private static bool HasFlag(ChangeFlags flags, ChangeFlags flagToCheck)
    {
      return (flagToCheck & flags) != ChangeFlags.None;
    }

    private void DoSelectionChange()
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      List<string> guids = new List<string>(Selection.objects.Length);
      foreach (UnityEngine.Object @object in Selection.objects)
      {
        if (hierarchyProperty.Find(@object.GetInstanceID(), (int[]) null))
          guids.Add(hierarchyProperty.guid);
      }
      if (this.pv1hasSelection)
        this.pv1hasSelection = ASCommitWindow.MarkSelected(this.pv1state, guids);
      if (this.pv1hasSelection)
        return;
      if (this.pv2hasSelection)
        this.pv2hasSelection = ASCommitWindow.MarkSelected(this.pv2state, guids);
      if (this.pv2hasSelection)
        return;
      this.pv1hasSelection = ASCommitWindow.MarkSelected(this.pv1state, guids);
      if (this.pv1hasSelection)
        return;
      this.pv2hasSelection = ASCommitWindow.MarkSelected(this.pv2state, guids);
    }

    internal void OnSelectionChange()
    {
      if (!this.mySelection)
      {
        this.DoSelectionChange();
        this.parentWin.Repaint();
      }
      else
        this.mySelection = false;
      this.somethingDiscardableSelected = ASCommitWindow.SomethingDiscardableSelected(!this.pv2hasSelection ? this.pv1state : this.pv2state);
    }

    public static bool SomethingDiscardableSelected(ParentViewState st)
    {
      int num = 0;
      foreach (ParentViewFolder folder in st.folders)
      {
        if (st.selectedItems[num++])
          return true;
        foreach (ParentViewFile file in folder.files)
        {
          if (st.selectedItems[num++] && ASCommitWindow.IsDiscardableAsset(file.guid, file.changeFlags))
            return true;
        }
      }
      return false;
    }

    private static bool AllFolderWouldBeMovedAnyway(ParentViewState pvState, string guid)
    {
      int num1 = 0;
      for (int index1 = 0; index1 < pvState.folders.Length; ++index1)
      {
        if (pvState.folders[index1].guid == guid)
        {
          bool flag1 = true;
          bool flag2 = true;
          bool[] selectedItems = pvState.selectedItems;
          int index2 = num1;
          int num2 = 1;
          int num3 = index2 + num2;
          bool flag3 = selectedItems[index2];
          for (int index3 = 0; index3 < pvState.folders[index1].files.Length; ++index3)
          {
            if (pvState.selectedItems[num3++])
              flag1 = false;
            else
              flag2 = false;
          }
          if (!flag3)
            return false;
          if (!flag2)
            return flag1;
          return true;
        }
        num1 += 1 + pvState.folders[index1].files.Length;
      }
      return false;
    }

    private bool DoShowMyDiff(bool binary)
    {
      return ASCommitWindow.DoShowDiff(ASCommitWindow.GetParentViewSelectedItems(!this.pv2hasSelection ? this.pv1state : this.pv2state, false, false), binary);
    }

    private void DoMyRevert(bool afterMarkingDependencies)
    {
      if (!afterMarkingDependencies)
      {
        List<string> selectedItems1 = this.GetSelectedItems();
        bool flag = !this.pv2hasSelection ? ASCommitWindow.MarkAllFolderDependenciesForDiscarding(this.pv1state, this.pv2state) : ASCommitWindow.MarkAllFolderDependenciesForDiscarding(this.pv2state, this.pv1state);
        if (flag)
          this.MySelectionToGlobalSelection();
        List<string> selectedItems2 = this.GetSelectedItems();
        if (selectedItems1.Count != selectedItems2.Count)
          flag = true;
        this.lastRevertSelectionChanged = !flag ? -1 : 1;
      }
      if (!afterMarkingDependencies && this.lastRevertSelectionChanged != -1)
        return;
      this.SetPersistedData();
      ASCommitWindow.DoRevert(ASCommitWindow.GetParentViewSelectedItems(!this.pv2hasSelection ? this.pv1state : this.pv2state, true, true), "CBReinitCommitWindow");
    }

    private void MenuClick(object userData, string[] options, int selected)
    {
      if (selected < 0)
        return;
      this.description = this.commitMessageList[selected];
      this.resetKeyboardControl = true;
      this.parentWin.Repaint();
    }

    private void ContextMenuClick(object userData, string[] options, int selected)
    {
      if (selected < 0)
        return;
      string dropDownMenuItem = this.dropDownMenuItems[selected];
      if (dropDownMenuItem == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ASCommitWindow.\u003C\u003Ef__switch\u0024mapF == null)
      {
        // ISSUE: reference to a compiler-generated field
        ASCommitWindow.\u003C\u003Ef__switch\u0024mapF = new Dictionary<string, int>(5)
        {
          {
            "Compare",
            0
          },
          {
            "Compare Binary",
            1
          },
          {
            "Discard",
            2
          },
          {
            ">>>",
            3
          },
          {
            "<<<",
            4
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!ASCommitWindow.\u003C\u003Ef__switch\u0024mapF.TryGetValue(dropDownMenuItem, out num))
        return;
      switch (num)
      {
        case 0:
          this.DoShowMyDiff(false);
          break;
        case 1:
          this.DoShowMyDiff(true);
          break;
        case 2:
          this.DoMyRevert(false);
          break;
        case 3:
          this.DoTransferAll(this.pv1state, this.pv2state, this.pv1state.selectedFolder, this.pv1state.selectedFile);
          break;
        case 4:
          this.DoTransferAll(this.pv2state, this.pv1state, this.pv2state.selectedFolder, this.pv2state.selectedFile);
          break;
      }
    }

    private static int IndexOfFolderWithGUID(ParentViewFolder[] folders, string guid)
    {
      for (int index = 0; index < folders.Length; ++index)
      {
        if (folders[index].guid == guid)
          return index;
      }
      return -1;
    }

    private static int FolderIndexToTotalIndex(ParentViewFolder[] folders, int folderIndex)
    {
      int num = 0;
      for (int index = 0; index < folderIndex; ++index)
        num += folders[index].files.Length + 1;
      return num;
    }

    private static int FolderSelectionIndexFromGUID(ParentViewFolder[] folders, string guid)
    {
      int num = 0;
      for (int index = 0; index < folders.Length; ++index)
      {
        if (guid == folders[index].guid)
          return num;
        num += 1 + folders[index].files.Length;
      }
      return -1;
    }

    private void AddFolderToRemove(ref List<string> guidsOfFoldersToRemove, string guid)
    {
      if (guidsOfFoldersToRemove.Contains(guid))
        return;
      guidsOfFoldersToRemove.Add(guid);
    }

    private bool ParentViewGUI(ParentViewState pvState, ParentViewState anotherPvState, ref bool hasSelection)
    {
      bool flag1 = false;
      EditorGUIUtility.SetIconSize(this.iconSize);
      ListViewState lv = pvState.lv;
      bool shift = Event.current.shift;
      bool actionKey = EditorGUI.actionKey;
      int row = lv.row;
      int folder1 = -1;
      int file = -1;
      bool flag2 = false;
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(lv, ListViewOptions.wantsToStartCustomDrag | ListViewOptions.wantsToAcceptCustomDrag, this.dragTitle, GUIStyle.none, new GUILayoutOption[0]))
      {
        if (folder1 == -1 && !pvState.IndexToFolderAndFile(listViewElement.row, ref folder1, ref file))
        {
          flag1 = true;
          break;
        }
        if (GUIUtility.keyboardControl == lv.ID && Event.current.type == EventType.KeyDown && actionKey)
          Event.current.Use();
        ParentViewFolder folder2 = pvState.folders[folder1];
        if (pvState.selectedItems[listViewElement.row] && Event.current.type == EventType.Repaint)
          ASCommitWindow.constants.entrySelected.Draw(listViewElement.position, false, false, false, false);
        if (ListViewGUILayout.HasMouseUp(listViewElement.position))
        {
          if (!shift && !actionKey)
            flag2 |= ListViewGUILayout.MultiSelection(row, pvState.lv.row, ref pvState.initialSelectedItem, ref pvState.selectedItems);
        }
        else if (ListViewGUILayout.HasMouseDown(listViewElement.position))
        {
          if (Event.current.clickCount == 2)
          {
            this.DoShowMyDiff(false);
            GUIUtility.ExitGUI();
          }
          else
          {
            if (!pvState.selectedItems[listViewElement.row] || shift || actionKey)
              flag2 |= ListViewGUILayout.MultiSelection(row, listViewElement.row, ref pvState.initialSelectedItem, ref pvState.selectedItems);
            pvState.selectedFile = file;
            pvState.selectedFolder = folder1;
            lv.row = listViewElement.row;
          }
        }
        else if (ListViewGUILayout.HasMouseDown(listViewElement.position, 1))
        {
          if (!pvState.selectedItems[listViewElement.row])
          {
            flag2 = true;
            pvState.ClearSelection();
            pvState.selectedItems[listViewElement.row] = true;
            pvState.selectedFile = file;
            pvState.selectedFolder = folder1;
            lv.row = listViewElement.row;
          }
          this.dropDownMenuItems[0] = pvState != this.pv1state ? "<<<" : ">>>";
          GUIUtility.hotControl = 0;
          EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), this.dropDownMenuItems, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), (object) null);
          Event.current.Use();
        }
        ChangeFlags changeFlags;
        if (file != -1)
        {
          Texture2D texture2D = AssetDatabase.GetCachedIcon(folder2.name + "/" + folder2.files[file].name) as Texture2D;
          if ((UnityEngine.Object) texture2D == (UnityEngine.Object) null)
            texture2D = InternalEditorUtility.GetIconForFile(folder2.files[file].name);
          GUILayout.Label(new GUIContent(folder2.files[file].name, (Texture) texture2D), ASCommitWindow.constants.element, new GUILayoutOption[0]);
          changeFlags = folder2.files[file].changeFlags;
        }
        else
        {
          GUILayout.Label(folder2.name, ASCommitWindow.constants.header, new GUILayoutOption[0]);
          changeFlags = folder2.changeFlags;
        }
        GUIContent content = (GUIContent) null;
        if (ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Undeleted) || ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Created))
          content = ASMainWindow.constants.badgeNew;
        else if (ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Deleted))
          content = ASMainWindow.constants.badgeDelete;
        else if (ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Renamed) || ASCommitWindow.HasFlag(changeFlags, ChangeFlags.Moved))
          content = ASMainWindow.constants.badgeMove;
        if (content != null && Event.current.type == EventType.Repaint)
        {
          Rect position = new Rect((float) ((double) listViewElement.position.x + (double) listViewElement.position.width - (double) content.image.width - 5.0), listViewElement.position.y + listViewElement.position.height / 2f - (float) (content.image.height / 2), (float) content.image.width, (float) content.image.height);
          EditorGUIUtility.SetIconSize(Vector2.zero);
          GUIStyle.none.Draw(position, content, false, false, false, false);
          EditorGUIUtility.SetIconSize(this.iconSize);
        }
        pvState.NextFileFolder(ref folder1, ref file);
      }
      if (!flag1)
      {
        if (GUIUtility.keyboardControl == lv.ID)
        {
          if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll")
            Event.current.Use();
          else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SelectAll")
          {
            for (int index = 0; index < pvState.selectedItems.Length; ++index)
              pvState.selectedItems[index] = true;
            flag2 = true;
            Event.current.Use();
          }
        }
        if (lv.customDraggedFromID != 0 && lv.customDraggedFromID == anotherPvState.lv.ID)
          this.DoTransferAll(anotherPvState, pvState, pvState.selectedFolder, pvState.selectedFile);
        if (GUIUtility.keyboardControl == lv.ID && !actionKey)
        {
          if (lv.selectionChanged)
          {
            flag2 |= ListViewGUILayout.MultiSelection(row, lv.row, ref pvState.initialSelectedItem, ref pvState.selectedItems);
            if (!pvState.IndexToFolderAndFile(lv.row, ref pvState.selectedFolder, ref pvState.selectedFile))
              flag1 = true;
          }
          else if (pvState.selectedFolder != -1 && Event.current.type == EventType.KeyDown && (GUIUtility.keyboardControl == lv.ID && Event.current.keyCode == KeyCode.Return))
          {
            this.DoTransferAll(pvState, anotherPvState, pvState.selectedFolder, pvState.selectedFile);
            ListViewGUILayout.MultiSelection(row, lv.row, ref pvState.initialSelectedItem, ref pvState.selectedItems);
            pvState.IndexToFolderAndFile(lv.row, ref pvState.selectedFolder, ref pvState.selectedFile);
            Event.current.Use();
            flag1 = true;
          }
        }
        if (lv.selectionChanged || flag2)
        {
          if (pvState.IndexToFolderAndFile(lv.row, ref folder1, ref file))
            this.dragTitle = file != -1 ? pvState.folders[folder1].files[file].name : pvState.folders[folder1].name;
          anotherPvState.ClearSelection();
          anotherPvState.lv.row = -1;
          anotherPvState.selectedFile = -1;
          anotherPvState.selectedFolder = -1;
          this.MySelectionToGlobalSelection();
        }
      }
      EditorGUIUtility.SetIconSize(Vector2.zero);
      return !flag1;
    }

    internal bool DoGUI()
    {
      bool enabled1 = GUI.enabled;
      if (ASCommitWindow.constants == null)
        ASCommitWindow.constants = new ASCommitWindow.Constants();
      if (this.resetKeyboardControl)
      {
        this.resetKeyboardControl = false;
        GUIUtility.keyboardControl = 0;
      }
      bool flag = (double) this.parentWin.position.width <= 432.0;
      if (Event.current.type == EventType.Layout)
        this.wasHidingButtons = flag;
      else if (flag != this.wasHidingButtons)
        GUIUtility.ExitGUI();
      SplitterGUILayout.BeginHorizontalSplit(this.horSplit);
      GUILayout.BeginVertical(ASCommitWindow.constants.box, new GUILayoutOption[0]);
      GUILayout.Label(this.totalChanges, ASCommitWindow.constants.title, new GUILayoutOption[0]);
      if (!this.ParentViewGUI(this.pv1state, this.pv2state, ref this.pv1hasSelection))
        return true;
      GUILayout.EndVertical();
      SplitterGUILayout.BeginVerticalSplit(this.vertSplit);
      GUILayout.BeginVertical(ASCommitWindow.constants.box, new GUILayoutOption[0]);
      GUILayout.Label("Changeset", ASCommitWindow.constants.title, new GUILayoutOption[0]);
      if (!this.ParentViewGUI(this.pv2state, this.pv1state, ref this.pv2hasSelection))
        return true;
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      GUILayout.Label("Commit Message", ASCommitWindow.constants.title, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      if (this.commitMessageList.Length > 0)
      {
        GUIContent content = new GUIContent("Recent");
        Rect rect = GUILayoutUtility.GetRect(content, ASCommitWindow.constants.dropDown, (GUILayoutOption[]) null);
        if (GUI.Button(rect, content, ASCommitWindow.constants.dropDown))
        {
          GUIUtility.hotControl = 0;
          string[] options = new string[this.commitMessageList.Length];
          for (int index = 0; index < options.Length; ++index)
            options[index] = this.commitMessageList[index].Length <= 200 ? this.commitMessageList[index] : this.commitMessageList[index].Substring(0, 200) + " ... ";
          EditorUtility.DisplayCustomMenu(rect, options, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.MenuClick), (object) null);
          Event.current.Use();
        }
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal(ASCommitWindow.constants.box, new GUILayoutOption[0]);
      this.scrollPos = EditorGUILayout.BeginVerticalScrollView(this.scrollPos);
      this.description = EditorGUILayout.TextArea(this.description, ASCommitWindow.constants.wwText, new GUILayoutOption[0]);
      EditorGUILayout.EndScrollView();
      GUILayout.EndHorizontal();
      GUILayout.EndVertical();
      SplitterGUILayout.EndVerticalSplit();
      SplitterGUILayout.EndHorizontalSplit();
      if (!flag)
        GUILayout.Label("Please drag files you want to commit to Changeset and fill in commit description");
      GUILayout.BeginHorizontal();
      if (!this.pv1hasSelection && !this.pv2hasSelection)
        GUI.enabled = false;
      if (!flag && GUILayout.Button("Compare", ASCommitWindow.constants.button, new GUILayoutOption[0]))
      {
        this.DoShowMyDiff(false);
        GUIUtility.ExitGUI();
      }
      bool enabled2 = GUI.enabled;
      if (!this.somethingDiscardableSelected)
        GUI.enabled = false;
      if (GUILayout.Button(!flag ? "Discard Selected Changes" : "Discard", ASCommitWindow.constants.button, new GUILayoutOption[0]))
      {
        this.DoMyRevert(false);
        GUIUtility.ExitGUI();
      }
      GUI.enabled = enabled2;
      GUILayout.FlexibleSpace();
      GUI.enabled = this.pv1hasSelection && enabled1;
      if (GUILayout.Button(!flag ? ">>>" : ">", ASCommitWindow.constants.button, new GUILayoutOption[0]))
        this.DoTransferAll(this.pv1state, this.pv2state, this.pv1state.selectedFolder, this.pv1state.selectedFile);
      GUI.enabled = this.pv2hasSelection && enabled1;
      if (GUILayout.Button(!flag ? "<<<" : "<", ASCommitWindow.constants.button, new GUILayoutOption[0]))
        this.DoTransferAll(this.pv2state, this.pv1state, this.pv2state.selectedFolder, this.pv2state.selectedFile);
      GUI.enabled = this.pv1state.lv.totalRows != 0 && enabled1;
      if (GUILayout.Button("Add All", ASCommitWindow.constants.button, new GUILayoutOption[0]))
      {
        int num = 0;
        while (num < this.pv1state.selectedItems.Length)
          this.pv1state.selectedItems[num++] = true;
        this.DoTransferAll(this.pv1state, this.pv2state, this.pv1state.selectedFolder, this.pv1state.selectedFile);
      }
      GUI.enabled = this.pv2state.lv.totalRows != 0 && enabled1;
      if (GUILayout.Button("Remove All", ASCommitWindow.constants.button, new GUILayoutOption[0]))
      {
        int num = 0;
        while (num < this.pv2state.selectedItems.Length)
          this.pv2state.selectedItems[num++] = true;
        this.DoTransferAll(this.pv2state, this.pv1state, this.pv2state.selectedFolder, this.pv2state.selectedFile);
      }
      GUI.enabled = enabled1;
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (!this.CanCommit())
        GUI.enabled = false;
      if (GUILayout.Button("Commit", ASCommitWindow.constants.bigButton, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(100f)
      }))
        this.DoCommit();
      GUI.enabled = enabled1;
      if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.KeypadEnter && (Application.platform == RuntimePlatform.OSXEditor && this.CanCommit()))
        this.DoCommit();
      GUILayout.EndHorizontal();
      if (AssetServer.GetAssetServerError() != string.Empty)
      {
        GUILayout.Space(10f);
        GUILayout.Label(AssetServer.GetAssetServerError(), ASCommitWindow.constants.errorLabel, new GUILayoutOption[0]);
        GUILayout.Space(10f);
      }
      GUILayout.Space(10f);
      if (this.lastRevertSelectionChanged == 0)
      {
        this.lastRevertSelectionChanged = -1;
        if (ASCommitWindow.ShowDiscardWarning())
          this.DoMyRevert(true);
      }
      if (this.lastRevertSelectionChanged > 0)
      {
        if (Event.current.type == EventType.Repaint)
          --this.lastRevertSelectionChanged;
        this.parentWin.Repaint();
      }
      if (this.showReinitedWarning == 0)
      {
        EditorUtility.DisplayDialog("Commits updated", "Commits had to be updated to reflect latest changes", "OK", string.Empty);
        this.showReinitedWarning = -1;
      }
      if (this.showReinitedWarning > 0)
      {
        if (Event.current.type == EventType.Repaint)
          --this.showReinitedWarning;
        this.parentWin.Repaint();
      }
      return true;
    }

    private class Constants
    {
      public GUIStyle box = (GUIStyle) "OL Box";
      public GUIStyle entrySelected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle serverChangeCount = (GUIStyle) "ServerChangeCount";
      public GUIStyle title = (GUIStyle) "OL title";
      public GUIStyle element = (GUIStyle) "OL elem";
      public GUIStyle header = (GUIStyle) "OL header";
      public GUIStyle button = (GUIStyle) "Button";
      public GUIStyle serverUpdateInfo = (GUIStyle) "ServerUpdateInfo";
      public GUIStyle wwText = (GUIStyle) "AS TextArea";
      public GUIStyle errorLabel = (GUIStyle) "ErrorLabel";
      public GUIStyle dropDown = (GUIStyle) "MiniPullDown";
      public GUIStyle bigButton = (GUIStyle) "LargeButton";
    }
  }
}
