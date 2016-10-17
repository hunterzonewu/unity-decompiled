// Decompiled with JetBrains decompiler
// Type: UnityEditor.ASUpdateWindow
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
  internal class ASUpdateWindow
  {
    private string[] dropDownMenuItems = new string[2]
    {
      "Compare",
      "Compare Binary"
    };
    private Vector2 iconSize = new Vector2(16f, 16f);
    private string selectedGUID = string.Empty;
    private ParentViewState pv = new ParentViewState();
    private SplitterState horSplit = new SplitterState(new float[2]{ 50f, 50f }, new int[2]{ 50, 50 }, (int[]) null);
    private SplitterState vertSplit = new SplitterState(new float[2]{ 60f, 30f }, new int[2]{ 32, 32 }, (int[]) null);
    private ASUpdateWindow.Constants constants;
    private ASUpdateConflictResolveWindow asResolveWin;
    private ASMainWindow parentWin;
    private Changeset[] changesets;
    private string[] messageFirstLines;
    private int maxNickLength;
    private bool isDirSelected;
    private ListViewState lv;
    private string totalUpdates;
    private bool showingConflicts;

    public bool ShowingConflicts
    {
      get
      {
        return this.showingConflicts;
      }
    }

    public bool CanContinue
    {
      get
      {
        return this.asResolveWin.CanContinue();
      }
    }

    public ASUpdateWindow(ASMainWindow parentWin, Changeset[] changesets)
    {
      this.changesets = changesets;
      this.parentWin = parentWin;
      this.lv = new ListViewState(changesets.Length, 5);
      this.pv.lv = new ListViewState(0, 5);
      this.messageFirstLines = new string[changesets.Length];
      for (int index = 0; index < changesets.Length; ++index)
        this.messageFirstLines[index] = changesets[index].message.Split('\n')[0];
      this.totalUpdates = changesets.Length.ToString() + (changesets.Length != 1 ? " Updates" : " Update");
    }

    private void ContextMenuClick(object userData, string[] options, int selected)
    {
      if (selected < 0)
        return;
      string dropDownMenuItem = this.dropDownMenuItems[selected];
      if (dropDownMenuItem == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ASUpdateWindow.\u003C\u003Ef__switch\u0024map14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ASUpdateWindow.\u003C\u003Ef__switch\u0024map14 = new Dictionary<string, int>(2)
        {
          {
            "Compare",
            0
          },
          {
            "Compare Binary",
            1
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!ASUpdateWindow.\u003C\u003Ef__switch\u0024map14.TryGetValue(dropDownMenuItem, out num))
        return;
      if (num != 0)
      {
        if (num != 1)
          return;
        this.DoShowDiff(true);
      }
      else
        this.DoShowDiff(false);
    }

    private void DoSelectionChange()
    {
      if (this.lv.row == -1)
        return;
      string firstSelected = this.GetFirstSelected();
      if (firstSelected != string.Empty)
        this.selectedGUID = firstSelected;
      if (AssetServer.IsGUIDValid(this.selectedGUID) != 0)
      {
        int num = 0;
        this.pv.lv.row = -1;
        foreach (ParentViewFolder folder in this.pv.folders)
        {
          if (folder.guid == this.selectedGUID)
          {
            this.pv.lv.row = num;
            break;
          }
          ++num;
          foreach (ParentViewFile file in folder.files)
          {
            if (file.guid == this.selectedGUID)
            {
              this.pv.lv.row = num;
              return;
            }
            ++num;
          }
        }
      }
      else
        this.pv.lv.row = -1;
    }

    private string GetFirstSelected()
    {
      UnityEngine.Object[] filtered = Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.Assets);
      if (filtered.Length != 0)
        return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(filtered[0]));
      return string.Empty;
    }

    public void OnSelectionChange()
    {
      if (this.showingConflicts)
      {
        this.asResolveWin.OnSelectionChange(this);
      }
      else
      {
        this.DoSelectionChange();
        this.parentWin.Repaint();
      }
    }

    public int GetSelectedRevisionNumber()
    {
      if (this.pv.lv.row > this.lv.totalRows - 1 || this.lv.row < 0)
        return -1;
      return this.changesets[this.lv.row].changeset;
    }

    public void SetSelectedRevisionLine(int selIndex)
    {
      if (selIndex >= this.lv.totalRows)
      {
        this.pv.Clear();
        this.lv.row = -1;
      }
      else
      {
        this.lv.row = selIndex;
        this.pv.Clear();
        this.pv.AddAssetItems(this.changesets[selIndex]);
        this.pv.SetLineCount();
      }
      this.pv.lv.scrollPos = Vector2.zero;
      this.pv.lv.row = -1;
      this.pv.selectedFolder = -1;
      this.pv.selectedFile = -1;
      this.DoSelectionChange();
    }

    public string[] GetGUIDs()
    {
      List<string> stringList = new List<string>();
      if (this.lv.row < 0)
        return (string[]) null;
      for (int row = this.lv.row; row < this.lv.totalRows; ++row)
      {
        for (int index = 0; index < this.changesets[row].items.Length; ++index)
        {
          if (!stringList.Contains(this.changesets[row].items[index].guid))
            stringList.Add(this.changesets[row].items[index].guid);
        }
      }
      return stringList.ToArray();
    }

    public bool DoUpdate(bool afterResolvingConflicts)
    {
      AssetServer.RemoveMaintErrorsFromConsole();
      if (!ASEditorBackend.SettingsIfNeeded())
        return true;
      this.showingConflicts = false;
      AssetServer.SetAfterActionFinishedCallback("ASEditorBackend", "CBReinitOnSuccess");
      AssetServer.DoUpdateOnNextTick(!afterResolvingConflicts, "ShowASConflictResolutionsWindow");
      return true;
    }

    public void ShowConflictResolutions(string[] conflicting)
    {
      this.asResolveWin = new ASUpdateConflictResolveWindow(conflicting);
      this.showingConflicts = true;
    }

    private bool HasFlag(ChangeFlags flags, ChangeFlags flagToCheck)
    {
      return (flagToCheck & flags) != ChangeFlags.None;
    }

    private void DoSelect(int folderI, int fileI, int row)
    {
      this.pv.selectedFile = fileI;
      this.pv.selectedFolder = folderI;
      this.pv.lv.row = row;
      this.pv.lv.selectionChanged = true;
      if (fileI == -1)
      {
        if (folderI != -1)
        {
          this.selectedGUID = this.pv.folders[folderI].guid;
          this.isDirSelected = true;
        }
        else
        {
          this.selectedGUID = string.Empty;
          this.isDirSelected = false;
        }
      }
      else
      {
        this.selectedGUID = this.pv.folders[folderI].files[fileI].guid;
        this.isDirSelected = false;
      }
    }

    public void UpdateGUI()
    {
      SplitterGUILayout.BeginHorizontalSplit(this.horSplit);
      GUILayout.BeginVertical(this.constants.box, new GUILayoutOption[0]);
      GUILayout.Label(this.totalUpdates, this.constants.title, new GUILayoutOption[0]);
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.lv, GUIStyle.none))
      {
        Rect position = listViewElement.position;
        ++position.x;
        ++position.y;
        if (Event.current.type == EventType.Repaint)
        {
          if (listViewElement.row % 2 == 0)
            this.constants.entryEven.Draw(position, false, false, false, false);
          else
            this.constants.entryOdd.Draw(position, false, false, false, false);
        }
        GUILayout.BeginVertical(listViewElement.row != this.lv.row ? this.constants.entryNormal : this.constants.entrySelected, new GUILayoutOption[0]);
        GUILayout.Label(this.messageFirstLines[listViewElement.row], this.constants.serverUpdateLog, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(50f)
        });
        GUILayout.BeginHorizontal();
        GUILayout.Label(this.changesets[listViewElement.row].changeset.ToString() + " " + this.changesets[listViewElement.row].date, this.constants.serverUpdateInfo, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(100f)
        });
        GUILayout.Label(this.changesets[listViewElement.row].owner, this.constants.serverUpdateInfo, new GUILayoutOption[1]
        {
          GUILayout.Width((float) this.maxNickLength)
        });
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
      }
      if (this.lv.selectionChanged)
        this.SetSelectedRevisionLine(this.lv.row);
      GUILayout.EndVertical();
      SplitterGUILayout.BeginVerticalSplit(this.vertSplit);
      GUILayout.BeginVertical(this.constants.box, new GUILayoutOption[0]);
      GUILayout.Label("Changeset", this.constants.title, new GUILayoutOption[0]);
      int folder1 = -1;
      int file = -1;
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.pv.lv, GUIStyle.none))
      {
        if (folder1 == -1 && !this.pv.IndexToFolderAndFile(listViewElement.row, ref folder1, ref file))
          return;
        ParentViewFolder folder2 = this.pv.folders[folder1];
        if (ListViewGUILayout.HasMouseDown(listViewElement.position))
        {
          if (Event.current.clickCount == 2)
          {
            if (!this.isDirSelected && this.selectedGUID != string.Empty)
            {
              this.DoShowDiff(false);
              GUIUtility.ExitGUI();
            }
          }
          else
          {
            this.pv.lv.scrollPos = ListViewShared.ListViewScrollToRow((ListViewShared.InternalListViewState) this.pv.lv.ilvState, listViewElement.row);
            this.DoSelect(folder1, file, listViewElement.row);
          }
        }
        else if (ListViewGUILayout.HasMouseDown(listViewElement.position, 1))
        {
          if (this.lv.row != listViewElement.row)
            this.DoSelect(folder1, file, listViewElement.row);
          if (!this.isDirSelected && this.selectedGUID != string.Empty)
          {
            GUIUtility.hotControl = 0;
            EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), this.dropDownMenuItems, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), (object) null);
            Event.current.Use();
          }
        }
        if (listViewElement.row == this.pv.lv.row && Event.current.type == EventType.Repaint)
          this.constants.entrySelected.Draw(listViewElement.position, false, false, false, false);
        ChangeFlags changeFlags;
        if (file != -1)
        {
          Texture2D texture2D = AssetDatabase.GetCachedIcon(folder2.name + "/" + folder2.files[file].name) as Texture2D;
          if ((UnityEngine.Object) texture2D == (UnityEngine.Object) null)
            texture2D = InternalEditorUtility.GetIconForFile(folder2.files[file].name);
          GUILayout.Label(new GUIContent(folder2.files[file].name, (Texture) texture2D), this.constants.element, new GUILayoutOption[0]);
          changeFlags = folder2.files[file].changeFlags;
        }
        else
        {
          GUILayout.Label(folder2.name, this.constants.header, new GUILayoutOption[0]);
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
      if (this.pv.lv.selectionChanged && this.selectedGUID != string.Empty)
      {
        if (this.selectedGUID != AssetServer.GetRootGUID())
          AssetServer.SetSelectionFromGUID(this.selectedGUID);
        else
          AssetServer.SetSelectionFromGUID(string.Empty);
      }
      if (GUIUtility.keyboardControl == this.pv.lv.ID && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return && !this.isDirSelected) && this.selectedGUID != string.Empty)
      {
        this.DoShowDiff(false);
        GUIUtility.ExitGUI();
      }
      GUILayout.EndVertical();
      GUILayout.BeginVertical(this.constants.box, new GUILayoutOption[0]);
      GUILayout.Label("Update Message", this.constants.title, new GUILayoutOption[0]);
      GUILayout.TextArea(this.lv.row < 0 ? string.Empty : this.changesets[this.lv.row].message, this.constants.wwText, new GUILayoutOption[0]);
      GUILayout.EndVertical();
      SplitterGUILayout.EndVerticalSplit();
      SplitterGUILayout.EndHorizontalSplit();
    }

    private bool DoShowDiff(bool binary)
    {
      List<string> stringList = new List<string>();
      List<CompareInfo> compareInfoList = new List<CompareInfo>();
      int ver1 = !AssetServer.IsItemDeleted(this.selectedGUID) ? AssetServer.GetServerItemChangeset(this.selectedGUID, AssetServer.GetWorkingItemChangeset(this.selectedGUID)) : -2;
      int serverItemChangeset = AssetServer.GetServerItemChangeset(this.selectedGUID, -1);
      int ver2 = serverItemChangeset != -1 ? serverItemChangeset : -2;
      stringList.Add(this.selectedGUID);
      compareInfoList.Add(new CompareInfo(ver1, ver2, !binary ? 0 : 1, !binary ? 1 : 0));
      if (stringList.Count == 0)
        return false;
      AssetServer.CompareFiles(stringList.ToArray(), compareInfoList.ToArray());
      return true;
    }

    public void Repaint()
    {
      this.parentWin.Repaint();
    }

    public bool DoGUI()
    {
      bool enabled = GUI.enabled;
      if (this.constants == null)
      {
        this.constants = new ASUpdateWindow.Constants();
        this.maxNickLength = 1;
        for (int index = 0; index < this.changesets.Length; ++index)
        {
          int x = (int) this.constants.serverUpdateInfo.CalcSize(new GUIContent(this.changesets[index].owner)).x;
          if (x > this.maxNickLength)
            this.maxNickLength = x;
        }
      }
      EditorGUIUtility.SetIconSize(this.iconSize);
      if (this.showingConflicts)
      {
        if (!this.asResolveWin.DoGUI(this))
          this.showingConflicts = false;
      }
      else
        this.UpdateGUI();
      EditorGUIUtility.SetIconSize(Vector2.zero);
      if (!this.showingConflicts)
      {
        GUILayout.BeginHorizontal();
        GUI.enabled = !this.isDirSelected && this.selectedGUID != string.Empty && enabled;
        if (GUILayout.Button("Compare", this.constants.button, new GUILayoutOption[0]))
        {
          this.DoShowDiff(false);
          GUIUtility.ExitGUI();
        }
        GUI.enabled = enabled;
        GUILayout.FlexibleSpace();
        if (this.changesets.Length == 0)
          GUI.enabled = false;
        if (GUILayout.Button("Update", this.constants.bigButton, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(100f)
        }))
        {
          if (this.changesets.Length == 0)
            Debug.Log((object) "Nothing to update.");
          else
            this.DoUpdate(false);
          this.parentWin.Repaint();
          GUIUtility.ExitGUI();
        }
        if (this.changesets.Length == 0)
          GUI.enabled = enabled;
        GUILayout.EndHorizontal();
        if (AssetServer.GetAssetServerError() != string.Empty)
        {
          GUILayout.Space(10f);
          GUILayout.Label(AssetServer.GetAssetServerError(), this.constants.errorLabel, new GUILayoutOption[0]);
          GUILayout.Space(10f);
        }
      }
      GUILayout.Space(10f);
      return true;
    }

    internal class Constants
    {
      public GUIStyle box = (GUIStyle) "OL Box";
      public GUIStyle entrySelected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle entryNormal = (GUIStyle) "ServerUpdateChangeset";
      public GUIStyle serverUpdateLog = (GUIStyle) "ServerUpdateLog";
      public GUIStyle serverChangeCount = (GUIStyle) "ServerChangeCount";
      public GUIStyle title = (GUIStyle) "OL title";
      public GUIStyle element = (GUIStyle) "OL elem";
      public GUIStyle header = (GUIStyle) "OL header";
      public GUIStyle serverUpdateInfo = (GUIStyle) "ServerUpdateInfo";
      public GUIStyle button = (GUIStyle) "Button";
      public GUIStyle errorLabel = (GUIStyle) "ErrorLabel";
      public GUIStyle bigButton = (GUIStyle) "LargeButton";
      public GUIStyle wwText = (GUIStyle) "AS TextArea";
      public GUIStyle entryEven = (GUIStyle) "CN EntryBackEven";
      public GUIStyle entryOdd = (GUIStyle) "CN EntryBackOdd";
    }
  }
}
