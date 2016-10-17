// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TreeViewState
  {
    [SerializeField]
    private List<int> m_SelectedIDs = new List<int>();
    [SerializeField]
    private List<int> m_ExpandedIDs = new List<int>();
    [SerializeField]
    private RenameOverlay m_RenameOverlay = new RenameOverlay();
    [SerializeField]
    private CreateAssetUtility m_CreateAssetUtility = new CreateAssetUtility();
    public Vector2 scrollPos;
    [SerializeField]
    private int m_LastClickedID;
    [SerializeField]
    private string m_SearchString;
    [SerializeField]
    private float[] m_ColumnWidths;

    public List<int> selectedIDs
    {
      get
      {
        return this.m_SelectedIDs;
      }
      set
      {
        this.m_SelectedIDs = value;
      }
    }

    public int lastClickedID
    {
      get
      {
        return this.m_LastClickedID;
      }
      set
      {
        this.m_LastClickedID = value;
      }
    }

    public List<int> expandedIDs
    {
      get
      {
        return this.m_ExpandedIDs;
      }
      set
      {
        this.m_ExpandedIDs = value;
      }
    }

    public RenameOverlay renameOverlay
    {
      get
      {
        return this.m_RenameOverlay;
      }
      set
      {
        this.m_RenameOverlay = value;
      }
    }

    public CreateAssetUtility createAssetUtility
    {
      get
      {
        return this.m_CreateAssetUtility;
      }
      set
      {
        this.m_CreateAssetUtility = value;
      }
    }

    public float[] columnWidths
    {
      get
      {
        return this.m_ColumnWidths;
      }
      set
      {
        this.m_ColumnWidths = value;
      }
    }

    public string searchString
    {
      get
      {
        return this.m_SearchString;
      }
      set
      {
        this.m_SearchString = value;
      }
    }

    public void OnAwake()
    {
      this.m_RenameOverlay.Clear();
      this.m_CreateAssetUtility = new CreateAssetUtility();
    }
  }
}
