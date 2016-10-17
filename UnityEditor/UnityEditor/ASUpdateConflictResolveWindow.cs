// Decompiled with JetBrains decompiler
// Type: UnityEditor.ASUpdateConflictResolveWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class ASUpdateConflictResolveWindow
  {
    private static string[] conflictButtonTexts = new string[5]
    {
      "Skip Asset",
      "Discard My Changes",
      "Ignore Server Changes",
      "Merge",
      "Unresolved"
    };
    private static string[] nameConflictButtonTexts = new string[2]
    {
      "Rename Local Asset",
      "Rename Server Asset"
    };
    private ListViewState lv1 = new ListViewState();
    private ListViewState lv2 = new ListViewState();
    private int initialSelectedLV1Item = -1;
    private int initialSelectedLV2Item = -1;
    private SplitterState lvHeaderSplit1 = new SplitterState(new float[2]{ 20f, 80f }, new int[2]{ 100, 100 }, (int[]) null);
    private SplitterState lvHeaderSplit2 = new SplitterState(new float[2]{ 20f, 80f }, new int[2]{ 100, 100 }, (int[]) null);
    private string[] dropDownMenuItems = new string[2]
    {
      "Compare",
      "Compare Binary"
    };
    private string[] downloadConflicts = new string[0];
    private string[] nameConflicts = new string[0];
    private string[] dConflictPaths = new string[0];
    private string[] dNamingPaths = new string[0];
    private DownloadResolution[] downloadResolutions = new DownloadResolution[0];
    private NameConflictResolution[] namingResolutions = new NameConflictResolution[0];
    private bool enableMergeButton = true;
    private Vector2 iconSize = new Vector2(16f, 16f);
    private string[] downloadResolutionString = new string[5]
    {
      "Unresolved",
      "Skip Asset",
      "Discard My Changes",
      "Ignore Server Changes",
      "Merge"
    };
    private string[] namingResolutionString = new string[3]
    {
      "Unresolved",
      "Rename Local Asset",
      "Rename Server Asset"
    };
    private bool[] selectedLV1Items;
    private bool[] selectedLV2Items;
    private bool[] deletionConflict;
    private bool lv1HasSelection;
    private bool lv2HasSelection;
    private int downloadConflictsToResolve;
    private bool showDownloadConflicts;
    private bool showNamingConflicts;
    private bool mySelection;
    private bool enableContinueButton;
    private bool splittersOk;
    private ASUpdateConflictResolveWindow.Constants constants;

    public ASUpdateConflictResolveWindow(string[] conflicting)
    {
      this.downloadConflictsToResolve = 0;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      ArrayList arrayList4 = new ArrayList();
      for (int index = 0; index < conflicting.Length; ++index)
      {
        AssetStatus statusGuid = AssetServer.GetStatusGUID(conflicting[index]);
        if (statusGuid == AssetStatus.Conflict)
        {
          arrayList1.Add((object) conflicting[index]);
          DownloadResolution downloadResolution = AssetServer.GetDownloadResolution(conflicting[index]);
          arrayList2.Add((object) downloadResolution);
          if (downloadResolution == DownloadResolution.Unresolved)
            ++this.downloadConflictsToResolve;
        }
        if (AssetServer.GetPathNameConflict(conflicting[index]) != null && statusGuid != AssetStatus.ServerOnly)
        {
          arrayList4.Add((object) conflicting[index]);
          NameConflictResolution conflictResolution = AssetServer.GetNameConflictResolution(conflicting[index]);
          arrayList3.Add((object) conflictResolution);
          if (conflictResolution == NameConflictResolution.Unresolved)
            ++this.downloadConflictsToResolve;
        }
      }
      this.downloadConflicts = arrayList1.ToArray(typeof (string)) as string[];
      this.downloadResolutions = arrayList2.ToArray(typeof (DownloadResolution)) as DownloadResolution[];
      this.namingResolutions = arrayList3.ToArray(typeof (NameConflictResolution)) as NameConflictResolution[];
      this.nameConflicts = arrayList4.ToArray(typeof (string)) as string[];
      this.enableContinueButton = this.downloadConflictsToResolve == 0;
      this.dConflictPaths = new string[this.downloadConflicts.Length];
      this.deletionConflict = new bool[this.downloadConflicts.Length];
      for (int index = 0; index < this.downloadConflicts.Length; ++index)
      {
        if (AssetServer.HasDeletionConflict(this.downloadConflicts[index]))
        {
          this.dConflictPaths[index] = ParentViewFolder.MakeNiceName(AssetServer.GetDeletedItemPathAndName(this.downloadConflicts[index]));
          this.deletionConflict[index] = true;
        }
        else
        {
          this.dConflictPaths[index] = ParentViewFolder.MakeNiceName(AssetServer.GetAssetPathName(this.downloadConflicts[index]));
          this.deletionConflict[index] = false;
        }
      }
      this.dNamingPaths = new string[this.nameConflicts.Length];
      for (int index = 0; index < this.nameConflicts.Length; ++index)
        this.dNamingPaths[index] = ParentViewFolder.MakeNiceName(AssetServer.GetAssetPathName(this.nameConflicts[index]));
      this.showDownloadConflicts = this.downloadConflicts.Length > 0;
      this.showNamingConflicts = this.nameConflicts.Length > 0;
      this.lv1.totalRows = this.downloadConflicts.Length;
      this.lv2.totalRows = this.nameConflicts.Length;
      this.selectedLV1Items = new bool[this.downloadConflicts.Length];
      this.selectedLV2Items = new bool[this.nameConflicts.Length];
      this.DoSelectionChange();
    }

    public string[] GetDownloadConflicts()
    {
      return this.downloadConflicts;
    }

    public string[] GetNameConflicts()
    {
      return this.nameConflicts;
    }

    public bool CanContinue()
    {
      return this.enableContinueButton;
    }

    private void ContextMenuClick(object userData, string[] options, int selected)
    {
      if (selected < 0)
        return;
      string dropDownMenuItem = this.dropDownMenuItems[selected];
      if (dropDownMenuItem == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ASUpdateConflictResolveWindow.\u003C\u003Ef__switch\u0024map13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ASUpdateConflictResolveWindow.\u003C\u003Ef__switch\u0024map13 = new Dictionary<string, int>(2)
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
      if (!ASUpdateConflictResolveWindow.\u003C\u003Ef__switch\u0024map13.TryGetValue(dropDownMenuItem, out num))
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

    private void ResolveSelectedDownloadConflicts(DownloadResolution res)
    {
      int index1 = -1;
      bool flag = false;
      for (int index2 = 0; index2 < this.downloadConflicts.Length; ++index2)
      {
        if (this.selectedLV1Items[index2])
        {
          string downloadConflict = this.downloadConflicts[index2];
          if (res == DownloadResolution.Merge && (AssetServer.AssetIsBinaryByGUID(downloadConflict) || AssetServer.IsItemDeleted(downloadConflict)))
          {
            flag = true;
          }
          else
          {
            if (res != DownloadResolution.Unresolved)
            {
              if (AssetServer.GetDownloadResolution(downloadConflict) == DownloadResolution.Unresolved)
                --this.downloadConflictsToResolve;
            }
            else
              ++this.downloadConflictsToResolve;
            this.downloadResolutions[index2] = res;
            AssetServer.SetDownloadResolution(downloadConflict, res);
            index1 = index1 != -1 ? -2 : index2;
          }
        }
      }
      this.enableContinueButton = this.downloadConflictsToResolve == 0;
      if (index1 >= 0)
      {
        this.selectedLV1Items[index1] = false;
        if (index1 < this.selectedLV1Items.Length - 1)
          this.selectedLV1Items[index1 + 1] = true;
      }
      this.enableMergeButton = this.AtLeastOneSelectedAssetCanBeMerged();
      if (!flag)
        return;
      EditorUtility.DisplayDialog("Some conflicting changes cannot be merged", "Notice that not all selected changes where selected for merging. This happened because not all of them can be merged (e.g. assets are binary or deleted).", "OK");
    }

    private void ResolveSelectedNamingConflicts(NameConflictResolution res)
    {
      if (res == NameConflictResolution.Unresolved)
        return;
      for (int index = 0; index < this.nameConflicts.Length; ++index)
      {
        if (this.selectedLV2Items[index])
        {
          string nameConflict = this.nameConflicts[index];
          if (AssetServer.GetNameConflictResolution(nameConflict) == NameConflictResolution.Unresolved)
            --this.downloadConflictsToResolve;
          this.namingResolutions[index] = res;
          AssetServer.SetNameConflictResolution(nameConflict, res);
        }
      }
      this.enableContinueButton = this.downloadConflictsToResolve == 0;
    }

    private bool DoShowDiff(bool binary)
    {
      List<string> stringList = new List<string>();
      List<CompareInfo> compareInfoList = new List<CompareInfo>();
      for (int index = 0; index < this.selectedLV1Items.Length; ++index)
      {
        if (this.selectedLV1Items[index])
        {
          int serverItemChangeset = AssetServer.GetServerItemChangeset(this.downloadConflicts[index], -1);
          int ver2 = !AssetServer.HasDeletionConflict(this.downloadConflicts[index]) ? -1 : -2;
          stringList.Add(this.downloadConflicts[index]);
          compareInfoList.Add(new CompareInfo(serverItemChangeset, ver2, !binary ? 0 : 1, !binary ? 1 : 0));
        }
      }
      if (stringList.Count == 0)
        return false;
      AssetServer.CompareFiles(stringList.ToArray(), compareInfoList.ToArray());
      return true;
    }

    private string[] GetSelectedGUIDs()
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.downloadConflicts.Length; ++index)
      {
        if (this.selectedLV1Items[index])
          stringList.Add(this.downloadConflicts[index]);
      }
      return stringList.ToArray();
    }

    private string[] GetSelectedNamingGUIDs()
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.nameConflicts.Length; ++index)
      {
        if (this.selectedLV2Items[index])
          stringList.Add(this.nameConflicts[index]);
      }
      return stringList.ToArray();
    }

    private bool HasTrue(ref bool[] array)
    {
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index])
          return true;
      }
      return false;
    }

    private void DoSelectionChange()
    {
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      List<string> stringList = new List<string>(Selection.objects.Length);
      foreach (UnityEngine.Object @object in Selection.objects)
      {
        if (hierarchyProperty.Find(@object.GetInstanceID(), (int[]) null))
          stringList.Add(hierarchyProperty.guid);
      }
      for (int index = 0; index < this.downloadConflicts.Length; ++index)
        this.selectedLV1Items[index] = stringList.Contains(this.downloadConflicts[index]);
      for (int index = 0; index < this.nameConflicts.Length; ++index)
        this.selectedLV2Items[index] = stringList.Contains(this.nameConflicts[index]);
      this.lv1HasSelection = this.HasTrue(ref this.selectedLV1Items);
      this.lv2HasSelection = this.HasTrue(ref this.selectedLV2Items);
      this.enableMergeButton = this.AtLeastOneSelectedAssetCanBeMerged();
    }

    public void OnSelectionChange(ASUpdateWindow parentWin)
    {
      if (!this.mySelection)
      {
        this.DoSelectionChange();
        parentWin.Repaint();
      }
      else
        this.mySelection = false;
    }

    private bool AtLeastOneSelectedAssetCanBeMerged()
    {
      for (int index = 0; index < this.downloadConflicts.Length; ++index)
      {
        if (this.selectedLV1Items[index] && !AssetServer.AssetIsBinaryByGUID(this.downloadConflicts[index]) && !AssetServer.IsItemDeleted(this.downloadConflicts[index]))
          return true;
      }
      return false;
    }

    private void DoDownloadConflictsGUI()
    {
      bool enabled = GUI.enabled;
      bool shift = Event.current.shift;
      bool actionKey = EditorGUI.actionKey;
      GUILayout.BeginVertical();
      GUILayout.Label("The following assets have been changed both on the server and in the local project.\nPlease select a conflict resolution for each before continuing the update.");
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.enabled = this.lv1HasSelection && enabled;
      if (GUILayout.Button(ASUpdateConflictResolveWindow.conflictButtonTexts[0], this.constants.ButtonLeft, new GUILayoutOption[0]))
        this.ResolveSelectedDownloadConflicts(DownloadResolution.SkipAsset);
      if (GUILayout.Button(ASUpdateConflictResolveWindow.conflictButtonTexts[1], this.constants.ButtonMiddle, new GUILayoutOption[0]))
        this.ResolveSelectedDownloadConflicts(DownloadResolution.TrashMyChanges);
      if (GUILayout.Button(ASUpdateConflictResolveWindow.conflictButtonTexts[2], this.constants.ButtonMiddle, new GUILayoutOption[0]))
        this.ResolveSelectedDownloadConflicts(DownloadResolution.TrashServerChanges);
      if (!this.enableMergeButton)
        GUI.enabled = false;
      if (GUILayout.Button(ASUpdateConflictResolveWindow.conflictButtonTexts[3], this.constants.ButtonRight, new GUILayoutOption[0]))
        this.ResolveSelectedDownloadConflicts(DownloadResolution.Merge);
      GUI.enabled = enabled;
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      SplitterGUILayout.BeginHorizontalSplit(this.lvHeaderSplit1);
      GUILayout.Box("Action", this.constants.lvHeader, new GUILayoutOption[0]);
      GUILayout.Box("Asset", this.constants.lvHeader, new GUILayoutOption[0]);
      SplitterGUILayout.EndHorizontalSplit();
      int row = this.lv1.row;
      bool flag = false;
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.lv1, this.constants.background))
      {
        if (GUIUtility.keyboardControl == this.lv1.ID && Event.current.type == EventType.KeyDown && actionKey)
          Event.current.Use();
        if (this.selectedLV1Items[listViewElement.row] && Event.current.type == EventType.Repaint)
          this.constants.selected.Draw(listViewElement.position, false, false, false, false);
        if (ListViewGUILayout.HasMouseUp(listViewElement.position))
        {
          if (!shift && !actionKey)
            flag |= ListViewGUILayout.MultiSelection(row, this.lv1.row, ref this.initialSelectedLV1Item, ref this.selectedLV1Items);
        }
        else if (ListViewGUILayout.HasMouseDown(listViewElement.position))
        {
          if (Event.current.clickCount == 2 && !AssetServer.AssetIsDir(this.downloadConflicts[listViewElement.row]))
          {
            this.DoShowDiff(false);
            GUIUtility.ExitGUI();
          }
          else
          {
            if (!this.selectedLV1Items[listViewElement.row] || shift || actionKey)
              flag |= ListViewGUILayout.MultiSelection(row, listViewElement.row, ref this.initialSelectedLV1Item, ref this.selectedLV1Items);
            this.lv1.row = listViewElement.row;
          }
        }
        else if (ListViewGUILayout.HasMouseDown(listViewElement.position, 1))
        {
          if (!this.selectedLV1Items[listViewElement.row])
          {
            flag = true;
            for (int index = 0; index < this.selectedLV1Items.Length; ++index)
              this.selectedLV1Items[index] = false;
            this.lv1.selectionChanged = true;
            this.selectedLV1Items[listViewElement.row] = true;
            this.lv1.row = listViewElement.row;
          }
          GUIUtility.hotControl = 0;
          EditorUtility.DisplayCustomMenu(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), this.dropDownMenuItems, (int[]) null, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), (object) null);
          Event.current.Use();
        }
        GUILayout.Label(this.downloadResolutionString[(int) this.downloadResolutions[listViewElement.row]], new GUILayoutOption[2]
        {
          GUILayout.Width((float) this.lvHeaderSplit1.realSizes[0]),
          GUILayout.Height(18f)
        });
        if (this.deletionConflict[listViewElement.row] && Event.current.type == EventType.Repaint)
        {
          GUIContent badgeDelete = ASMainWindow.constants.badgeDelete;
          Rect position = new Rect((float) ((double) listViewElement.position.x + (double) this.lvHeaderSplit1.realSizes[0] - (double) badgeDelete.image.width - 5.0), listViewElement.position.y + listViewElement.position.height / 2f - (float) (badgeDelete.image.height / 2), (float) badgeDelete.image.width, (float) badgeDelete.image.height);
          EditorGUIUtility.SetIconSize(Vector2.zero);
          GUIStyle.none.Draw(position, badgeDelete, false, false, false, false);
          EditorGUIUtility.SetIconSize(this.iconSize);
        }
        GUILayout.Label(new GUIContent(this.dConflictPaths[listViewElement.row], !AssetServer.AssetIsDir(this.downloadConflicts[listViewElement.row]) ? (Texture) InternalEditorUtility.GetIconForFile(this.dConflictPaths[listViewElement.row]) : (Texture) EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName)), new GUILayoutOption[2]
        {
          GUILayout.Width((float) this.lvHeaderSplit1.realSizes[1]),
          GUILayout.Height(18f)
        });
      }
      GUILayout.EndVertical();
      if (GUIUtility.keyboardControl == this.lv1.ID)
      {
        if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll")
          Event.current.Use();
        else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SelectAll")
        {
          for (int index = 0; index < this.selectedLV1Items.Length; ++index)
            this.selectedLV1Items[index] = true;
          flag = true;
          Event.current.Use();
        }
        if (this.lv1.selectionChanged && !actionKey)
          flag |= ListViewGUILayout.MultiSelection(row, this.lv1.row, ref this.initialSelectedLV1Item, ref this.selectedLV1Items);
        else if (GUIUtility.keyboardControl == this.lv1.ID && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return && !AssetServer.AssetIsDir(this.downloadConflicts[this.lv1.row])))
        {
          this.DoShowDiff(false);
          GUIUtility.ExitGUI();
        }
      }
      if (!this.lv1.selectionChanged && !flag)
        return;
      this.mySelection = true;
      AssetServer.SetSelectionFromGUIDs(this.GetSelectedGUIDs());
      this.lv1HasSelection = this.HasTrue(ref this.selectedLV1Items);
      this.enableMergeButton = this.AtLeastOneSelectedAssetCanBeMerged();
    }

    private void DoNamingConflictsGUI()
    {
      bool enabled = GUI.enabled;
      bool shift = Event.current.shift;
      bool actionKey = EditorGUI.actionKey;
      GUILayout.BeginVertical();
      GUILayout.Space(10f);
      GUILayout.Label("The following assets have the same name as an existing asset on the server.\nPlease select which one to rename before continuing the update.");
      GUILayout.Space(10f);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.enabled = this.lv2HasSelection && enabled;
      if (GUILayout.Button(ASUpdateConflictResolveWindow.nameConflictButtonTexts[0], this.constants.ButtonLeft, new GUILayoutOption[0]))
        this.ResolveSelectedNamingConflicts(NameConflictResolution.RenameLocal);
      if (GUILayout.Button(ASUpdateConflictResolveWindow.nameConflictButtonTexts[1], this.constants.ButtonRight, new GUILayoutOption[0]))
        this.ResolveSelectedNamingConflicts(NameConflictResolution.RenameRemote);
      GUI.enabled = enabled;
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      SplitterGUILayout.BeginHorizontalSplit(this.lvHeaderSplit2);
      GUILayout.Box("Action", this.constants.lvHeader, new GUILayoutOption[0]);
      GUILayout.Box("Asset", this.constants.lvHeader, new GUILayoutOption[0]);
      SplitterGUILayout.EndHorizontalSplit();
      int row = this.lv2.row;
      bool flag = false;
      foreach (ListViewElement listViewElement in ListViewGUILayout.ListView(this.lv2, this.constants.background))
      {
        if (GUIUtility.keyboardControl == this.lv2.ID && Event.current.type == EventType.KeyDown && actionKey)
          Event.current.Use();
        if (this.selectedLV2Items[listViewElement.row] && Event.current.type == EventType.Repaint)
          this.constants.selected.Draw(listViewElement.position, false, false, false, false);
        if (ListViewGUILayout.HasMouseUp(listViewElement.position))
        {
          if (!shift && !actionKey)
            flag |= ListViewGUILayout.MultiSelection(row, this.lv2.row, ref this.initialSelectedLV2Item, ref this.selectedLV2Items);
        }
        else if (ListViewGUILayout.HasMouseDown(listViewElement.position))
        {
          if (!this.selectedLV2Items[listViewElement.row] || shift || actionKey)
            flag |= ListViewGUILayout.MultiSelection(row, listViewElement.row, ref this.initialSelectedLV2Item, ref this.selectedLV2Items);
          this.lv2.row = listViewElement.row;
        }
        GUILayout.Label(this.namingResolutionString[(int) this.namingResolutions[listViewElement.row]], new GUILayoutOption[2]
        {
          GUILayout.Width((float) this.lvHeaderSplit2.realSizes[0]),
          GUILayout.Height(18f)
        });
        GUILayout.Label(new GUIContent(this.dNamingPaths[listViewElement.row], !AssetServer.AssetIsDir(this.nameConflicts[listViewElement.row]) ? (Texture) InternalEditorUtility.GetIconForFile(this.dNamingPaths[listViewElement.row]) : (Texture) EditorGUIUtility.FindTexture(EditorResourcesUtility.folderIconName)), new GUILayoutOption[2]
        {
          GUILayout.Width((float) this.lvHeaderSplit2.realSizes[1]),
          GUILayout.Height(18f)
        });
      }
      GUILayout.EndVertical();
      if (GUIUtility.keyboardControl == this.lv2.ID)
      {
        if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "SelectAll")
          Event.current.Use();
        else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "SelectAll")
        {
          for (int index = 0; index < this.selectedLV2Items.Length; ++index)
            this.selectedLV2Items[index] = true;
          flag = true;
          Event.current.Use();
        }
        if (this.lv2.selectionChanged && !actionKey)
          flag |= ListViewGUILayout.MultiSelection(row, this.lv2.row, ref this.initialSelectedLV2Item, ref this.selectedLV2Items);
      }
      if (!this.lv2.selectionChanged && !flag)
        return;
      this.mySelection = true;
      AssetServer.SetSelectionFromGUIDs(this.GetSelectedNamingGUIDs());
      this.lv2HasSelection = this.HasTrue(ref this.selectedLV2Items);
    }

    public bool DoGUI(ASUpdateWindow parentWin)
    {
      if (this.constants == null)
        this.constants = new ASUpdateConflictResolveWindow.Constants();
      bool enabled = GUI.enabled;
      EditorGUIUtility.SetIconSize(this.iconSize);
      GUILayout.BeginVertical();
      if (this.showDownloadConflicts)
        this.DoDownloadConflictsGUI();
      if (this.showNamingConflicts)
        this.DoNamingConflictsGUI();
      GUILayout.EndVertical();
      EditorGUIUtility.SetIconSize(Vector2.zero);
      GUILayout.BeginHorizontal();
      GUI.enabled = this.lv1HasSelection && enabled;
      if (GUILayout.Button("Compare", this.constants.button, new GUILayoutOption[0]))
      {
        if (!this.DoShowDiff(false))
          Debug.Log((object) "No differences found");
        GUIUtility.ExitGUI();
      }
      GUI.enabled = enabled;
      GUILayout.FlexibleSpace();
      GUI.enabled = parentWin.CanContinue && enabled;
      if (GUILayout.Button("Continue", this.constants.bigButton, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(100f)
      }))
      {
        parentWin.DoUpdate(true);
        return false;
      }
      GUI.enabled = enabled;
      if (GUILayout.Button("Cancel", this.constants.bigButton, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(100f)
      }))
        return false;
      GUILayout.EndHorizontal();
      if (!this.splittersOk && Event.current.type == EventType.Repaint)
      {
        this.splittersOk = true;
        parentWin.Repaint();
      }
      return true;
    }

    private class Constants
    {
      public GUIStyle ButtonLeft = (GUIStyle) "ButtonLeft";
      public GUIStyle ButtonMiddle = (GUIStyle) "ButtonMid";
      public GUIStyle ButtonRight = (GUIStyle) "ButtonRight";
      public GUIStyle EntrySelected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle EntryNormal = (GUIStyle) "ServerUpdateInfo";
      public GUIStyle lvHeader = (GUIStyle) "OL title";
      public GUIStyle selected = (GUIStyle) "ServerUpdateChangesetOn";
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle button = (GUIStyle) "Button";
      public GUIStyle bigButton = (GUIStyle) "LargeButton";
    }
  }
}
