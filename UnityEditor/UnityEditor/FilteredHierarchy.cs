// Decompiled with JetBrains decompiler
// Type: UnityEditor.FilteredHierarchy
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class FilteredHierarchy
  {
    private SearchFilter m_SearchFilter = new SearchFilter();
    private FilteredHierarchy.FilterResult[] m_Results = new FilteredHierarchy.FilterResult[0];
    private FilteredHierarchy.FilterResult[] m_VisibleItems = new FilteredHierarchy.FilterResult[0];
    private HierarchyType m_HierarchyType;

    public HierarchyType hierarchyType
    {
      get
      {
        return this.m_HierarchyType;
      }
    }

    public FilteredHierarchy.FilterResult[] results
    {
      get
      {
        if (this.m_VisibleItems.Length > 0)
          return this.m_VisibleItems;
        return this.m_Results;
      }
    }

    public SearchFilter searchFilter
    {
      get
      {
        return this.m_SearchFilter;
      }
      set
      {
        if (!this.m_SearchFilter.SetNewFilter(value))
          return;
        this.ResultsChanged();
      }
    }

    public bool foldersFirst { get; set; }

    public FilteredHierarchy(HierarchyType type)
    {
      this.m_HierarchyType = type;
    }

    public void SetResults(int[] instanceIDs)
    {
      HierarchyProperty property = new HierarchyProperty(this.m_HierarchyType);
      property.Reset();
      Array.Resize<FilteredHierarchy.FilterResult>(ref this.m_Results, instanceIDs.Length);
      for (int index = 0; index < instanceIDs.Length; ++index)
      {
        if (property.Find(instanceIDs[index], (int[]) null))
          this.CopyPropertyData(ref this.m_Results[index], property);
      }
    }

    private void CopyPropertyData(ref FilteredHierarchy.FilterResult result, HierarchyProperty property)
    {
      if (result == null)
        result = new FilteredHierarchy.FilterResult();
      result.instanceID = property.instanceID;
      result.name = property.name;
      result.hasChildren = property.hasChildren;
      result.colorCode = property.colorCode;
      result.isMainRepresentation = property.isMainRepresentation;
      result.hasFullPreviewImage = property.hasFullPreviewImage;
      result.iconDrawStyle = property.iconDrawStyle;
      result.isFolder = property.isFolder;
      result.type = this.hierarchyType;
      if (!property.isMainRepresentation)
        result.icon = property.icon;
      else if (property.isFolder && !property.hasChildren)
        result.icon = EditorGUIUtility.FindTexture(EditorResourcesUtility.emptyFolderIconName);
      else
        result.icon = (Texture2D) null;
    }

    private void SearchAllAssets(HierarchyProperty property)
    {
      int num = Mathf.Min(property.CountRemaining((int[]) null), 3000);
      property.Reset();
      int length = this.m_Results.Length;
      Array.Resize<FilteredHierarchy.FilterResult>(ref this.m_Results, this.m_Results.Length + num);
      for (; property.Next((int[]) null) && length < this.m_Results.Length; ++length)
        this.CopyPropertyData(ref this.m_Results[length], property);
    }

    private void SearchInFolders(HierarchyProperty property)
    {
      List<FilteredHierarchy.FilterResult> filterResultList = new List<FilteredHierarchy.FilterResult>();
      foreach (string baseFolder in ProjectWindowUtil.GetBaseFolders(this.m_SearchFilter.folders))
      {
        property.SetSearchFilter(new SearchFilter());
        int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID(baseFolder);
        if (property.Find(mainAssetInstanceId, (int[]) null))
        {
          property.SetSearchFilter(this.m_SearchFilter);
          int depth = property.depth;
          int[] expanded = (int[]) null;
          while (property.NextWithDepthCheck(expanded, depth + 1))
          {
            FilteredHierarchy.FilterResult result = new FilteredHierarchy.FilterResult();
            this.CopyPropertyData(ref result, property);
            filterResultList.Add(result);
          }
        }
      }
      this.m_Results = filterResultList.ToArray();
    }

    private void FolderBrowsing(HierarchyProperty property)
    {
      List<FilteredHierarchy.FilterResult> filterResultList = new List<FilteredHierarchy.FilterResult>();
      foreach (string folder in this.m_SearchFilter.folders)
      {
        int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID(folder);
        if (property.Find(mainAssetInstanceId, (int[]) null))
        {
          int depth = property.depth;
          int[] array = new int[1]{ mainAssetInstanceId };
          while (property.Next(array) && property.depth > depth)
          {
            FilteredHierarchy.FilterResult result = new FilteredHierarchy.FilterResult();
            this.CopyPropertyData(ref result, property);
            filterResultList.Add(result);
            if (property.hasChildren && !property.isFolder)
            {
              Array.Resize<int>(ref array, array.Length + 1);
              array[array.Length - 1] = property.instanceID;
            }
          }
        }
      }
      this.m_Results = filterResultList.ToArray();
    }

    private void AddResults(HierarchyProperty property)
    {
      switch (this.m_SearchFilter.GetState())
      {
        case SearchFilter.State.EmptySearchFilter:
          break;
        case SearchFilter.State.FolderBrowsing:
          this.FolderBrowsing(property);
          break;
        case SearchFilter.State.SearchingInAllAssets:
          this.SearchAllAssets(property);
          break;
        case SearchFilter.State.SearchingInFolders:
          this.SearchInFolders(property);
          break;
        case SearchFilter.State.SearchingInAssetStore:
          break;
        default:
          Debug.LogError((object) "Unhandled enum!");
          break;
      }
    }

    public void ResultsChanged()
    {
      this.m_Results = new FilteredHierarchy.FilterResult[0];
      if (this.m_SearchFilter.GetState() != SearchFilter.State.EmptySearchFilter)
      {
        HierarchyProperty property = new HierarchyProperty(this.m_HierarchyType);
        property.SetSearchFilter(this.m_SearchFilter);
        this.AddResults(property);
        if (this.m_SearchFilter.IsSearching())
          Array.Sort<FilteredHierarchy.FilterResult>(this.m_Results, (Comparison<FilteredHierarchy.FilterResult>) ((result1, result2) => EditorUtility.NaturalCompare(result1.name, result2.name)));
        if (!this.foldersFirst)
          return;
        for (int sourceIndex = 0; sourceIndex < this.m_Results.Length; ++sourceIndex)
        {
          if (!this.m_Results[sourceIndex].isFolder)
          {
            for (int index = sourceIndex + 1; index < this.m_Results.Length; ++index)
            {
              if (this.m_Results[index].isFolder)
              {
                FilteredHierarchy.FilterResult result = this.m_Results[index];
                int length = index - sourceIndex;
                Array.Copy((Array) this.m_Results, sourceIndex, (Array) this.m_Results, sourceIndex + 1, length);
                this.m_Results[sourceIndex] = result;
                break;
              }
            }
          }
        }
      }
      else
      {
        if (this.m_HierarchyType != HierarchyType.GameObjects)
          return;
        new HierarchyProperty(HierarchyType.GameObjects).SetSearchFilter(this.m_SearchFilter);
      }
    }

    public void RefreshVisibleItems(List<int> expandedInstanceIDs)
    {
      bool flag1 = this.m_SearchFilter.IsSearching();
      List<FilteredHierarchy.FilterResult> filterResultList = new List<FilteredHierarchy.FilterResult>();
      for (int mainRepresentionIndex = 0; mainRepresentionIndex < this.m_Results.Length; ++mainRepresentionIndex)
      {
        filterResultList.Add(this.m_Results[mainRepresentionIndex]);
        if (this.m_Results[mainRepresentionIndex].isMainRepresentation && this.m_Results[mainRepresentionIndex].hasChildren && !this.m_Results[mainRepresentionIndex].isFolder)
        {
          bool flag2 = expandedInstanceIDs.IndexOf(this.m_Results[mainRepresentionIndex].instanceID) >= 0 || flag1;
          int num = this.AddSubItemsOfMainRepresentation(mainRepresentionIndex, !flag2 ? (List<FilteredHierarchy.FilterResult>) null : filterResultList);
          mainRepresentionIndex += num;
        }
      }
      this.m_VisibleItems = filterResultList.ToArray();
    }

    public List<int> GetSubAssetInstanceIDs(int mainAssetInstanceID)
    {
      for (int index1 = 0; index1 < this.m_Results.Length; ++index1)
      {
        if (this.m_Results[index1].instanceID == mainAssetInstanceID)
        {
          List<int> intList = new List<int>();
          for (int index2 = index1 + 1; index2 < this.m_Results.Length && !this.m_Results[index2].isMainRepresentation; ++index2)
            intList.Add(this.m_Results[index2].instanceID);
          return intList;
        }
      }
      Debug.LogError((object) ("Not main rep " + (object) mainAssetInstanceID));
      return new List<int>();
    }

    public int AddSubItemsOfMainRepresentation(int mainRepresentionIndex, List<FilteredHierarchy.FilterResult> visibleItems)
    {
      int num = 0;
      int index = mainRepresentionIndex + 1;
      while (index < this.m_Results.Length && !this.m_Results[index].isMainRepresentation)
      {
        if (visibleItems != null)
          visibleItems.Add(this.m_Results[index]);
        ++index;
        ++num;
      }
      return num;
    }

    public class FilterResult
    {
      public int instanceID;
      public string name;
      public bool hasChildren;
      public int colorCode;
      public bool isMainRepresentation;
      public bool hasFullPreviewImage;
      public IconDrawStyle iconDrawStyle;
      public bool isFolder;
      public HierarchyType type;
      private Texture2D m_Icon;

      public Texture2D icon
      {
        get
        {
          if ((UnityEngine.Object) this.m_Icon == (UnityEngine.Object) null)
          {
            if (this.type == HierarchyType.Assets)
            {
              string assetPath = AssetDatabase.GetAssetPath(this.instanceID);
              if (assetPath != null)
                return AssetDatabase.GetCachedIcon(assetPath) as Texture2D;
            }
            else if (this.type == HierarchyType.GameObjects)
              this.m_Icon = AssetPreview.GetMiniThumbnail(EditorUtility.InstanceIDToObject(this.instanceID));
          }
          return this.m_Icon;
        }
        set
        {
          this.m_Icon = value;
        }
      }

      public string guid
      {
        get
        {
          if (this.type == HierarchyType.Assets)
          {
            string assetPath = AssetDatabase.GetAssetPath(this.instanceID);
            if (assetPath != null)
              return AssetDatabase.AssetPathToGUID(assetPath);
          }
          return (string) null;
        }
      }
    }
  }
}
