// Decompiled with JetBrains decompiler
// Type: UnityEditor.FilteredHierarchyProperty
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class FilteredHierarchyProperty : IHierarchyProperty
  {
    private int m_Position = -1;
    private FilteredHierarchy m_Hierarchy;

    public int instanceID
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].instanceID;
      }
    }

    public Object pptrValue
    {
      get
      {
        return EditorUtility.InstanceIDToObject(this.instanceID);
      }
    }

    public string name
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].name;
      }
    }

    public bool hasChildren
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].hasChildren;
      }
    }

    public bool isMainRepresentation
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].isMainRepresentation;
      }
    }

    public bool hasFullPreviewImage
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].hasFullPreviewImage;
      }
    }

    public IconDrawStyle iconDrawStyle
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].iconDrawStyle;
      }
    }

    public bool isFolder
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].isFolder;
      }
    }

    public int depth
    {
      get
      {
        return 0;
      }
    }

    public int row
    {
      get
      {
        return this.m_Position;
      }
    }

    public int colorCode
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].colorCode;
      }
    }

    public string guid
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].guid;
      }
    }

    public bool isValid
    {
      get
      {
        if (this.m_Hierarchy.results != null && this.m_Position < this.m_Hierarchy.results.Length)
          return this.m_Position >= 0;
        return false;
      }
    }

    public Texture2D icon
    {
      get
      {
        return this.m_Hierarchy.results[this.m_Position].icon;
      }
    }

    public int[] ancestors
    {
      get
      {
        return new int[0];
      }
    }

    public FilteredHierarchyProperty(FilteredHierarchy filter)
    {
      this.m_Hierarchy = filter;
    }

    public static IHierarchyProperty CreateHierarchyPropertyForFilter(FilteredHierarchy filteredHierarchy)
    {
      if (filteredHierarchy.searchFilter.GetState() != SearchFilter.State.EmptySearchFilter)
        return (IHierarchyProperty) new FilteredHierarchyProperty(filteredHierarchy);
      return (IHierarchyProperty) new HierarchyProperty(filteredHierarchy.hierarchyType);
    }

    public void Reset()
    {
      this.m_Position = -1;
    }

    public bool IsExpanded(int[] expanded)
    {
      return false;
    }

    public bool Next(int[] expanded)
    {
      ++this.m_Position;
      return this.m_Position < this.m_Hierarchy.results.Length;
    }

    public bool NextWithDepthCheck(int[] expanded, int minDepth)
    {
      return this.Next(expanded);
    }

    public bool Previous(int[] expanded)
    {
      --this.m_Position;
      return this.m_Position >= 0;
    }

    public bool Parent()
    {
      return false;
    }

    public bool Find(int _instanceID, int[] expanded)
    {
      this.Reset();
      while (this.Next(expanded))
      {
        if (this.instanceID == _instanceID)
          return true;
      }
      return false;
    }

    public int[] FindAllAncestors(int[] instanceIDs)
    {
      return new int[0];
    }

    public bool Skip(int count, int[] expanded)
    {
      this.m_Position += count;
      return this.m_Position < this.m_Hierarchy.results.Length;
    }

    public int CountRemaining(int[] expanded)
    {
      return this.m_Hierarchy.results.Length - this.m_Position - 1;
    }
  }
}
