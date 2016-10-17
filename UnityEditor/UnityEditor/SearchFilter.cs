// Decompiled with JetBrains decompiler
// Type: UnityEditor.SearchFilter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class SearchFilter
  {
    [SerializeField]
    private string m_NameFilter = string.Empty;
    [SerializeField]
    private string[] m_ClassNames = new string[0];
    [SerializeField]
    private string[] m_AssetLabels = new string[0];
    [SerializeField]
    private string[] m_AssetBundleNames = new string[0];
    [SerializeField]
    private int[] m_ReferencingInstanceIDs = new int[0];
    [SerializeField]
    private string[] m_Folders = new string[0];
    [SerializeField]
    private bool m_ShowAllHits;
    [SerializeField]
    private SearchFilter.SearchArea m_SearchArea;

    public string nameFilter
    {
      get
      {
        return this.m_NameFilter;
      }
      set
      {
        this.m_NameFilter = value;
      }
    }

    public string[] classNames
    {
      get
      {
        return this.m_ClassNames;
      }
      set
      {
        this.m_ClassNames = value;
      }
    }

    public string[] assetLabels
    {
      get
      {
        return this.m_AssetLabels;
      }
      set
      {
        this.m_AssetLabels = value;
      }
    }

    public string[] assetBundleNames
    {
      get
      {
        return this.m_AssetBundleNames;
      }
      set
      {
        this.m_AssetBundleNames = value;
      }
    }

    public int[] referencingInstanceIDs
    {
      get
      {
        return this.m_ReferencingInstanceIDs;
      }
      set
      {
        this.m_ReferencingInstanceIDs = value;
      }
    }

    public bool showAllHits
    {
      get
      {
        return this.m_ShowAllHits;
      }
      set
      {
        this.m_ShowAllHits = value;
      }
    }

    public string[] folders
    {
      get
      {
        return this.m_Folders;
      }
      set
      {
        this.m_Folders = value;
      }
    }

    public SearchFilter.SearchArea searchArea
    {
      get
      {
        return this.m_SearchArea;
      }
      set
      {
        this.m_SearchArea = value;
      }
    }

    public void ClearSearch()
    {
      this.m_NameFilter = string.Empty;
      this.m_ClassNames = new string[0];
      this.m_AssetLabels = new string[0];
      this.m_AssetBundleNames = new string[0];
      this.m_ReferencingInstanceIDs = new int[0];
      this.m_ShowAllHits = false;
    }

    private bool IsNullOrEmtpy<T>(T[] list)
    {
      if (list != null)
        return list.Length == 0;
      return true;
    }

    public SearchFilter.State GetState()
    {
      bool flag1 = !string.IsNullOrEmpty(this.m_NameFilter) || !this.IsNullOrEmtpy<string>(this.m_AssetLabels) || (!this.IsNullOrEmtpy<string>(this.m_ClassNames) || !this.IsNullOrEmtpy<string>(this.m_AssetBundleNames)) || !this.IsNullOrEmtpy<int>(this.m_ReferencingInstanceIDs);
      bool flag2 = !this.IsNullOrEmtpy<string>(this.m_Folders);
      if (flag1)
      {
        if (this.m_SearchArea == SearchFilter.SearchArea.AssetStore)
          return SearchFilter.State.SearchingInAssetStore;
        return flag2 && this.m_SearchArea == SearchFilter.SearchArea.SelectedFolders ? SearchFilter.State.SearchingInFolders : SearchFilter.State.SearchingInAllAssets;
      }
      return flag2 ? SearchFilter.State.FolderBrowsing : SearchFilter.State.EmptySearchFilter;
    }

    public bool IsSearching()
    {
      SearchFilter.State state = this.GetState();
      switch (state)
      {
        case SearchFilter.State.SearchingInAllAssets:
        case SearchFilter.State.SearchingInFolders:
          return true;
        default:
          return state == SearchFilter.State.SearchingInAssetStore;
      }
    }

    public bool SetNewFilter(SearchFilter newFilter)
    {
      bool flag = false;
      if (newFilter.m_NameFilter != this.m_NameFilter)
      {
        this.m_NameFilter = newFilter.m_NameFilter;
        flag = true;
      }
      if (newFilter.m_ClassNames != this.m_ClassNames)
      {
        this.m_ClassNames = newFilter.m_ClassNames;
        flag = true;
      }
      if (newFilter.m_Folders != this.m_Folders)
      {
        this.m_Folders = newFilter.m_Folders;
        flag = true;
      }
      if (newFilter.m_AssetLabels != this.m_AssetLabels)
      {
        this.m_AssetLabels = newFilter.m_AssetLabels;
        flag = true;
      }
      if (newFilter.m_AssetBundleNames != this.m_AssetBundleNames)
      {
        this.m_AssetBundleNames = newFilter.m_AssetBundleNames;
        flag = true;
      }
      if (newFilter.m_ReferencingInstanceIDs != this.m_ReferencingInstanceIDs)
      {
        this.m_ReferencingInstanceIDs = newFilter.m_ReferencingInstanceIDs;
        flag = true;
      }
      if (newFilter.m_SearchArea != this.m_SearchArea)
      {
        this.m_SearchArea = newFilter.m_SearchArea;
        flag = true;
      }
      this.m_ShowAllHits = newFilter.m_ShowAllHits;
      return flag;
    }

    public override string ToString()
    {
      string str = "SearchFilter: " + string.Format("[Area: {0}, State: {1}]", (object) this.m_SearchArea, (object) this.GetState());
      if (!string.IsNullOrEmpty(this.m_NameFilter))
        str = str + "[Name: " + this.m_NameFilter + "]";
      if (this.m_AssetLabels != null && this.m_AssetLabels.Length > 0)
        str = str + "[Labels: " + this.m_AssetLabels[0] + "]";
      if (this.m_AssetBundleNames != null && this.m_AssetBundleNames.Length > 0)
        str = str + "[AssetBundleNames: " + this.m_AssetBundleNames[0] + "]";
      if (this.m_ClassNames != null && this.m_ClassNames.Length > 0)
        str = str + "[Types: " + this.m_ClassNames[0] + " (" + (object) this.m_ClassNames.Length + ")]";
      if (this.m_ReferencingInstanceIDs != null && this.m_ReferencingInstanceIDs.Length > 0)
        str = str + "[RefIDs: " + (object) this.m_ReferencingInstanceIDs[0] + "]";
      if (this.m_Folders != null && this.m_Folders.Length > 0)
        str = str + "[Folders: " + this.m_Folders[0] + "]";
      return str + "[ShowAllHits: " + (object) this.showAllHits + "]";
    }

    internal string FilterToSearchFieldString()
    {
      string empty = string.Empty;
      if (!string.IsNullOrEmpty(this.m_NameFilter))
        empty += this.m_NameFilter;
      this.AddToString<string>("t:", this.m_ClassNames, ref empty);
      this.AddToString<string>("l:", this.m_AssetLabels, ref empty);
      this.AddToString<string>("b:", this.m_AssetBundleNames, ref empty);
      return empty;
    }

    private void AddToString<T>(string prefix, T[] list, ref string result)
    {
      if (list == null)
        return;
      if (result == null)
        result = string.Empty;
      foreach (T obj in list)
      {
        if (!string.IsNullOrEmpty(result))
          result = result + " ";
        result = result + prefix + (object) obj;
      }
    }

    internal void SearchFieldStringToFilter(string searchString)
    {
      this.ClearSearch();
      if (string.IsNullOrEmpty(searchString))
        return;
      SearchUtility.ParseSearchString(searchString, this);
    }

    internal static SearchFilter CreateSearchFilterFromString(string searchText)
    {
      SearchFilter filter = new SearchFilter();
      SearchUtility.ParseSearchString(searchText, filter);
      return filter;
    }

    public static string[] Split(string text)
    {
      if (string.IsNullOrEmpty(text))
        return new string[0];
      List<string> stringList = new List<string>();
      foreach (Match match in Regex.Matches(text, "\".+?\"|\\S+"))
        stringList.Add(match.Value.Replace("\"", string.Empty));
      return stringList.ToArray();
    }

    public enum SearchArea
    {
      AllAssets,
      SelectedFolders,
      AssetStore,
    }

    public enum State
    {
      EmptySearchFilter,
      FolderBrowsing,
      SearchingInAllAssets,
      SearchingInFolders,
      SearchingInAssetStore,
    }
  }
}
