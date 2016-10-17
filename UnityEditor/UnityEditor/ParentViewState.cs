// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParentViewState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Serializable]
  internal class ParentViewState
  {
    public int selectedFolder = -1;
    public int selectedFile = -1;
    public int initialSelectedItem = -1;
    public ParentViewFolder[] folders = new ParentViewFolder[0];
    public ListViewState lv;
    public bool[] selectedItems;

    public int GetLineCount()
    {
      int num = 0;
      for (int index = 0; index < this.folders.Length; ++index)
        num += this.folders[index].files.Length + 1;
      return num;
    }

    public bool HasTrue()
    {
      for (int index = 0; index < this.selectedItems.Length; ++index)
      {
        if (this.selectedItems[index])
          return true;
      }
      return false;
    }

    public void SetLineCount()
    {
      this.lv.totalRows = this.GetLineCount();
    }

    public int GetFoldersCount()
    {
      return this.folders.Length;
    }

    public void ClearSelection()
    {
      for (int index = 0; index < this.selectedItems.Length; ++index)
        this.selectedItems[index] = false;
      this.initialSelectedItem = -1;
    }

    internal static int IndexOf(ParentViewFolder[] foldersFrom, string lfname)
    {
      for (int index = 0; index < foldersFrom.Length; ++index)
      {
        if (string.Compare(foldersFrom[index].name, lfname, true) == 0)
          return index;
      }
      return -1;
    }

    internal static int IndexOf(ParentViewFile[] filesFrom, string lfname)
    {
      for (int index = 0; index < filesFrom.Length; ++index)
      {
        if (string.Compare(filesFrom[index].name, lfname, true) == 0)
          return index;
      }
      return -1;
    }

    internal static int CompareViewFolder(ParentViewFolder p1, ParentViewFolder p2)
    {
      return string.Compare(p1.name, p2.name, true);
    }

    internal static int CompareViewFile(ParentViewFile p1, ParentViewFile p2)
    {
      return string.Compare(p1.name, p2.name, true);
    }

    private void AddAssetItem(string guid, string pathName, bool isDir, ChangeFlags changeFlags, int changeset)
    {
      if (pathName == string.Empty)
        return;
      if (isDir)
      {
        string str = ParentViewFolder.MakeNiceName(pathName);
        int index = ParentViewState.IndexOf(this.folders, str);
        if (index == -1)
        {
          ArrayUtility.Add<ParentViewFolder>(ref this.folders, new ParentViewFolder(str, guid, changeFlags));
        }
        else
        {
          this.folders[index].changeFlags = changeFlags;
          this.folders[index].guid = guid;
        }
      }
      else
      {
        string str1 = ParentViewFolder.MakeNiceName(FileUtil.DeleteLastPathNameComponent(pathName));
        string str2 = pathName.Substring(pathName.LastIndexOf("/") + 1);
        int index1 = ParentViewState.IndexOf(this.folders, str1);
        ParentViewFolder parentViewFolder;
        if (index1 == -1)
        {
          parentViewFolder = new ParentViewFolder(str1, AssetServer.GetParentGUID(guid, changeset));
          ArrayUtility.Add<ParentViewFolder>(ref this.folders, parentViewFolder);
        }
        else
          parentViewFolder = this.folders[index1];
        int index2 = ParentViewState.IndexOf(parentViewFolder.files, str2);
        if (index2 != -1)
        {
          if ((parentViewFolder.files[index2].changeFlags & ChangeFlags.Deleted) != ChangeFlags.None)
            return;
          parentViewFolder.files[index2].guid = guid;
          parentViewFolder.files[index2].changeFlags = changeFlags;
        }
        else
          ArrayUtility.Add<ParentViewFile>(ref parentViewFolder.files, new ParentViewFile(str2, guid, changeFlags));
      }
    }

    public void AddAssetItems(AssetsItem[] assets)
    {
      foreach (AssetsItem asset in assets)
        this.AddAssetItem(asset.guid, asset.pathName, asset.assetIsDir != 0, (ChangeFlags) asset.changeFlags, -1);
      Array.Sort<ParentViewFolder>(this.folders, new Comparison<ParentViewFolder>(ParentViewState.CompareViewFolder));
      for (int index = 0; index < this.folders.Length; ++index)
        Array.Sort<ParentViewFile>(this.folders[index].files, new Comparison<ParentViewFile>(ParentViewState.CompareViewFile));
    }

    public void AddAssetItems(Changeset assets)
    {
      foreach (ChangesetItem changesetItem in assets.items)
        this.AddAssetItem(changesetItem.guid, changesetItem.fullPath, changesetItem.assetIsDir != 0, changesetItem.changeFlags, assets.changeset);
      Array.Sort<ParentViewFolder>(this.folders, new Comparison<ParentViewFolder>(ParentViewState.CompareViewFolder));
      for (int index = 0; index < this.folders.Length; ++index)
        Array.Sort<ParentViewFile>(this.folders[index].files, new Comparison<ParentViewFile>(ParentViewState.CompareViewFile));
    }

    public void AddAssetItems(DeletedAsset[] assets)
    {
      foreach (DeletedAsset asset in assets)
        this.AddAssetItem(asset.guid, asset.fullPath, asset.assetIsDir != 0, ChangeFlags.None, -1);
      Array.Sort<ParentViewFolder>(this.folders, new Comparison<ParentViewFolder>(ParentViewState.CompareViewFolder));
      for (int index = 0; index < this.folders.Length; ++index)
        Array.Sort<ParentViewFile>(this.folders[index].files, new Comparison<ParentViewFile>(ParentViewState.CompareViewFile));
    }

    public void Clear()
    {
      this.folders = new ParentViewFolder[0];
      this.selectedFolder = -1;
      this.selectedFile = -1;
      this.initialSelectedItem = -1;
    }

    public bool NextFileFolder(ref int folder, ref int file)
    {
      if (folder >= this.folders.Length)
        return false;
      ParentViewFolder folder1 = this.folders[folder];
      if (file >= folder1.files.Length - 1)
      {
        folder = folder + 1;
        file = -1;
        if (folder >= this.folders.Length)
          return false;
      }
      else
        file = file + 1;
      return true;
    }

    public bool IndexToFolderAndFile(int index, ref int folder, ref int file)
    {
      folder = 0;
      file = -1;
      for (int index1 = 0; index1 < index; ++index1)
      {
        if (!this.NextFileFolder(ref folder, ref file))
          return false;
      }
      return true;
    }
  }
}
