// Decompiled with JetBrains decompiler
// Type: UnityEditor.SavedSearchFilters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [FilePath("SearchFilters", FilePathAttribute.Location.PreferencesFolder)]
  internal class SavedSearchFilters : ScriptableSingleton<SavedSearchFilters>
  {
    [SerializeField]
    private List<SavedFilter> m_SavedFilters;
    private System.Action m_SavedFiltersChanged;
    private bool m_AllowHierarchy;

    public static int AddSavedFilter(string displayName, SearchFilter filter, float previewSize)
    {
      return ScriptableSingleton<SavedSearchFilters>.instance.Add(displayName, filter, previewSize, SavedSearchFilters.GetRootInstanceID(), true);
    }

    public static int AddSavedFilterAfterInstanceID(string displayName, SearchFilter filter, float previewSize, int insertAfterID, bool addAsChild)
    {
      return ScriptableSingleton<SavedSearchFilters>.instance.Add(displayName, filter, previewSize, insertAfterID, addAsChild);
    }

    public static void RemoveSavedFilter(int instanceID)
    {
      ScriptableSingleton<SavedSearchFilters>.instance.Remove(instanceID);
    }

    public static bool IsSavedFilter(int instanceID)
    {
      return ScriptableSingleton<SavedSearchFilters>.instance.IndexOf(instanceID) >= 0;
    }

    public static int GetRootInstanceID()
    {
      return ScriptableSingleton<SavedSearchFilters>.instance.GetRoot();
    }

    public static SearchFilter GetFilter(int instanceID)
    {
      SavedFilter savedFilter = ScriptableSingleton<SavedSearchFilters>.instance.Find(instanceID);
      if (savedFilter != null && savedFilter.m_Filter != null)
        return ObjectCopier.DeepClone<SearchFilter>(savedFilter.m_Filter);
      return (SearchFilter) null;
    }

    public static float GetPreviewSize(int instanceID)
    {
      SavedFilter savedFilter = ScriptableSingleton<SavedSearchFilters>.instance.Find(instanceID);
      if (savedFilter != null)
        return savedFilter.m_PreviewSize;
      return -1f;
    }

    public static string GetName(int instanceID)
    {
      SavedFilter savedFilter = ScriptableSingleton<SavedSearchFilters>.instance.Find(instanceID);
      if (savedFilter != null)
        return savedFilter.m_Name;
      Debug.LogError((object) ("Could not find saved filter " + (object) instanceID + " " + ScriptableSingleton<SavedSearchFilters>.instance.ToString()));
      return string.Empty;
    }

    public static void SetName(int instanceID, string name)
    {
      SavedFilter savedFilter = ScriptableSingleton<SavedSearchFilters>.instance.Find(instanceID);
      if (savedFilter != null)
      {
        savedFilter.m_Name = name;
        ScriptableSingleton<SavedSearchFilters>.instance.Changed();
      }
      else
        Debug.LogError((object) ("Could not set name of saved filter " + (object) instanceID + " " + ScriptableSingleton<SavedSearchFilters>.instance.ToString()));
    }

    public static void UpdateExistingSavedFilter(int instanceID, SearchFilter filter, float previewSize)
    {
      ScriptableSingleton<SavedSearchFilters>.instance.UpdateFilter(instanceID, filter, previewSize);
    }

    public static TreeViewItem ConvertToTreeView()
    {
      return ScriptableSingleton<SavedSearchFilters>.instance.BuildTreeView();
    }

    public static void AddChangeListener(System.Action callback)
    {
      ScriptableSingleton<SavedSearchFilters>.instance.m_SavedFiltersChanged -= callback;
      ScriptableSingleton<SavedSearchFilters>.instance.m_SavedFiltersChanged += callback;
    }

    public static void MoveSavedFilter(int instanceID, int parentInstanceID, int targetInstanceID, bool after)
    {
      ScriptableSingleton<SavedSearchFilters>.instance.Move(instanceID, parentInstanceID, targetInstanceID, after);
    }

    public static bool CanMoveSavedFilter(int instanceID, int parentInstanceID, int targetInstanceID, bool after)
    {
      return ScriptableSingleton<SavedSearchFilters>.instance.IsValidMove(instanceID, parentInstanceID, targetInstanceID, after);
    }

    public static bool AllowsHierarchy()
    {
      return ScriptableSingleton<SavedSearchFilters>.instance.m_AllowHierarchy;
    }

    private bool IsValidMove(int instanceID, int parentInstanceID, int targetInstanceID, bool after)
    {
      int index1 = this.IndexOf(instanceID);
      int num1 = this.IndexOf(parentInstanceID);
      int num2 = this.IndexOf(targetInstanceID);
      if (index1 < 0 || num1 < 0 || num2 < 0)
      {
        Debug.LogError((object) ("Move of a SavedFilter has invalid input: " + (object) index1 + " " + (object) num1 + " " + (object) num2));
        return false;
      }
      if (instanceID == targetInstanceID)
        return false;
      for (int index2 = index1 + 1; index2 < this.m_SavedFilters.Count && this.m_SavedFilters[index2].m_Depth > this.m_SavedFilters[index1].m_Depth; ++index2)
      {
        if (index2 == num2 || index2 == num1)
          return false;
      }
      return true;
    }

    private void Move(int instanceID, int parentInstanceID, int targetInstanceID, bool after)
    {
      if (!this.IsValidMove(instanceID, parentInstanceID, targetInstanceID, after))
        return;
      int index1 = this.IndexOf(instanceID);
      int index2 = this.IndexOf(parentInstanceID);
      this.IndexOf(targetInstanceID);
      SavedFilter savedFilter1 = this.m_SavedFilters[index1];
      SavedFilter savedFilter2 = this.m_SavedFilters[index2];
      int num1 = 0;
      if (this.m_AllowHierarchy)
        num1 = savedFilter2.m_Depth + 1 - savedFilter1.m_Depth;
      List<SavedFilter> filterAndChildren = this.GetSavedFilterAndChildren(instanceID);
      this.m_SavedFilters.RemoveRange(index1, filterAndChildren.Count);
      using (List<SavedFilter>.Enumerator enumerator = filterAndChildren.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.m_Depth += num1;
      }
      int num2 = this.IndexOf(targetInstanceID);
      if (num2 != -1)
        this.m_SavedFilters.InsertRange(num2 + 1, (IEnumerable<SavedFilter>) filterAndChildren);
      this.Changed();
    }

    private void UpdateFilter(int instanceID, SearchFilter filter, float previewSize)
    {
      SavedFilter savedFilter = this.Find(instanceID);
      if (savedFilter != null)
      {
        if (filter != null)
        {
          SearchFilter searchFilter = ObjectCopier.DeepClone<SearchFilter>(filter);
          savedFilter.m_Filter = searchFilter;
        }
        savedFilter.m_PreviewSize = previewSize;
        this.Changed();
      }
      else
        Debug.LogError((object) ("Could not find saved filter " + (object) instanceID + " " + ScriptableSingleton<SavedSearchFilters>.instance.ToString()));
    }

    private int GetNextAvailableID()
    {
      List<int> intList = new List<int>();
      using (List<SavedFilter>.Enumerator enumerator = this.m_SavedFilters.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SavedFilter current = enumerator.Current;
          if (current.m_ID >= ProjectWindowUtil.k_FavoritesStartInstanceID)
            intList.Add(current.m_ID);
        }
      }
      intList.Sort();
      int favoritesStartInstanceId = ProjectWindowUtil.k_FavoritesStartInstanceID;
      for (int index = 0; index < 1000; ++index)
      {
        if (intList.BinarySearch(favoritesStartInstanceId) < 0)
          return favoritesStartInstanceId;
        ++favoritesStartInstanceId;
      }
      Debug.LogError((object) ("Could not assign valid ID to saved filter " + DebugUtils.ListToString<int>((IEnumerable<int>) intList) + " " + (object) favoritesStartInstanceId));
      return ProjectWindowUtil.k_FavoritesStartInstanceID + 1000;
    }

    private int Add(string displayName, SearchFilter filter, float previewSize, int insertAfterInstanceID, bool addAsChild)
    {
      SearchFilter filter1 = (SearchFilter) null;
      if (filter != null)
        filter1 = ObjectCopier.DeepClone<SearchFilter>(filter);
      if (filter1.GetState() == SearchFilter.State.SearchingInAllAssets || filter1.GetState() == SearchFilter.State.SearchingInAssetStore)
        filter1.folders = new string[0];
      int index = 0;
      if (insertAfterInstanceID != 0)
      {
        index = this.IndexOf(insertAfterInstanceID);
        if (index == -1)
        {
          Debug.LogError((object) "Invalid insert position");
          return 0;
        }
      }
      int depth = this.m_SavedFilters[index].m_Depth + (!addAsChild ? 0 : 1);
      SavedFilter savedFilter = new SavedFilter(displayName, filter1, depth, previewSize);
      savedFilter.m_ID = this.GetNextAvailableID();
      if (this.m_SavedFilters.Count == 0)
        this.m_SavedFilters.Add(savedFilter);
      else
        this.m_SavedFilters.Insert(index + 1, savedFilter);
      this.Changed();
      return savedFilter.m_ID;
    }

    private List<SavedFilter> GetSavedFilterAndChildren(int instanceID)
    {
      List<SavedFilter> savedFilterList = new List<SavedFilter>();
      int index1 = this.IndexOf(instanceID);
      if (index1 >= 0)
      {
        savedFilterList.Add(this.m_SavedFilters[index1]);
        for (int index2 = index1 + 1; index2 < this.m_SavedFilters.Count && this.m_SavedFilters[index2].m_Depth > this.m_SavedFilters[index1].m_Depth; ++index2)
          savedFilterList.Add(this.m_SavedFilters[index2]);
      }
      return savedFilterList;
    }

    private void Remove(int instanceID)
    {
      int index = this.IndexOf(instanceID);
      if (index < 1)
        return;
      List<SavedFilter> filterAndChildren = this.GetSavedFilterAndChildren(instanceID);
      if (filterAndChildren.Count <= 0)
        return;
      this.m_SavedFilters.RemoveRange(index, filterAndChildren.Count);
      this.Changed();
    }

    private int IndexOf(int instanceID)
    {
      for (int index = 0; index < this.m_SavedFilters.Count; ++index)
      {
        if (this.m_SavedFilters[index].m_ID == instanceID)
          return index;
      }
      return -1;
    }

    private SavedFilter Find(int instanceID)
    {
      int index = this.IndexOf(instanceID);
      if (index >= 0)
        return this.m_SavedFilters[index];
      return (SavedFilter) null;
    }

    private void Init()
    {
      if (this.m_SavedFilters == null || this.m_SavedFilters.Count == 0)
      {
        this.m_SavedFilters = new List<SavedFilter>();
        this.m_SavedFilters.Add(new SavedFilter("Favorites", (SearchFilter) null, 0, -1f));
      }
      SearchFilter searchFilter = new SearchFilter();
      searchFilter.classNames = new string[0];
      this.m_SavedFilters[0].m_Name = "Favorites";
      this.m_SavedFilters[0].m_Filter = searchFilter;
      this.m_SavedFilters[0].m_Depth = 0;
      this.m_SavedFilters[0].m_ID = ProjectWindowUtil.k_FavoritesStartInstanceID;
      for (int index = 0; index < this.m_SavedFilters.Count; ++index)
      {
        if (this.m_SavedFilters[index].m_ID < ProjectWindowUtil.k_FavoritesStartInstanceID)
          this.m_SavedFilters[index].m_ID = this.GetNextAvailableID();
      }
      if (this.m_AllowHierarchy)
        return;
      for (int index = 1; index < this.m_SavedFilters.Count; ++index)
        this.m_SavedFilters[index].m_Depth = 1;
    }

    private int GetRoot()
    {
      if (this.m_SavedFilters != null && this.m_SavedFilters.Count > 0)
        return this.m_SavedFilters[0].m_ID;
      return 0;
    }

    private TreeViewItem BuildTreeView()
    {
      this.Init();
      if (this.m_SavedFilters.Count == 0)
      {
        Debug.LogError((object) "BuildTreeView: No saved filters! We should at least have a root");
        return (TreeViewItem) null;
      }
      TreeViewItem root = (TreeViewItem) null;
      List<TreeViewItem> visibleItems = new List<TreeViewItem>();
      for (int index = 0; index < this.m_SavedFilters.Count; ++index)
      {
        SavedFilter savedFilter = this.m_SavedFilters[index];
        int id = savedFilter.m_ID;
        int depth = savedFilter.m_Depth;
        bool isFolder = savedFilter.m_Filter.GetState() == SearchFilter.State.FolderBrowsing;
        TreeViewItem treeViewItem = (TreeViewItem) new SearchFilterTreeItem(id, depth, (TreeViewItem) null, savedFilter.m_Name, isFolder);
        if (index == 0)
          root = treeViewItem;
        else
          visibleItems.Add(treeViewItem);
      }
      TreeViewUtility.SetChildParentReferences(visibleItems, root);
      return root;
    }

    private void Changed()
    {
      this.Save(true);
      if (this.m_SavedFiltersChanged == null)
        return;
      this.m_SavedFiltersChanged();
    }

    public override string ToString()
    {
      string str = "Saved Filters ";
      for (int index = 0; index < this.m_SavedFilters.Count; ++index)
      {
        int id = this.m_SavedFilters[index].m_ID;
        SavedFilter savedFilter = this.m_SavedFilters[index];
        str += string.Format(": {0} ({1})({2})({3}) ", (object) savedFilter.m_Name, (object) id, (object) savedFilter.m_Depth, (object) savedFilter.m_PreviewSize);
      }
      return str;
    }
  }
}
