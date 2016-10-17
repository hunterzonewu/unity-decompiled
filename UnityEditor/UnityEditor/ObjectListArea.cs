// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectListArea
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEditorInternal.VersionControl;
using UnityEngine;

namespace UnityEditor
{
  internal class ObjectListArea
  {
    private PingData m_Ping = new PingData();
    private Dictionary<int, string> m_InstanceIDToCroppedNameMap = new Dictionary<int, string>();
    private bool m_AllowRenameOnMouseUp = true;
    private Vector2 m_LastScrollPosition = new Vector2(0.0f, 0.0f);
    private string m_AssetStoreError = string.Empty;
    private int m_MinIconSize = 32;
    private int m_MinGridSize = 16;
    private int m_MaxGridSize = 96;
    private bool m_AllowThumbnails = true;
    private bool m_ShowLocalAssetsOnly = true;
    private string m_LastAssetStoreQuerySearchFilter = string.Empty;
    private string[] m_LastAssetStoreQueryClassName = new string[0];
    private string[] m_LastAssetStoreQueryLabels = new string[0];
    public float m_SpaceBetween = 6f;
    public float m_TopMargin = 10f;
    public float m_BottomMargin = 10f;
    public float m_RightMargin = 10f;
    public float m_LeftMargin = 10f;
    private const int kHome = -2147483648;
    private const int kEnd = 2147483647;
    private const int kPageDown = 2147483646;
    private const int kPageUp = -2147483647;
    private const float k_ListModeVersionControlOverlayPadding = 14f;
    private const double kDelayQueryAfterScroll = 0.0;
    private const int kListLineHeight = 16;
    private const int kSpaceForScrollBar = 16;
    private const double kQueryDelay = 0.2;
    private static ObjectListArea.Styles s_Styles;
    private ObjectListAreaState m_State;
    private int m_SelectionOffset;
    private static bool s_VCEnabled;
    private EditorWindow m_Owner;
    private int m_KeyboardControlID;
    private int m_WidthUsedForCroppingName;
    private double LastScrollTime;
    public bool selectedAssetStoreAsset;
    internal Texture m_SelectedObjectIcon;
    private ObjectListArea.LocalGroup m_LocalAssets;
    private List<ObjectListArea.AssetStoreGroup> m_StoreAssets;
    private List<ObjectListArea.Group> m_Groups;
    private Rect m_TotalRect;
    private Rect m_VisibleRect;
    private int m_pingIndex;
    private int m_LeftPaddingForPinging;
    private bool m_FrameLastClickedItem;
    public bool m_RequeryAssetStore;
    private bool m_QueryInProgress;
    private int m_ResizePreviewCacheTo;
    private double m_LastAssetStoreQueryChangeTime;
    private double m_NextDirtyCheck;
    private System.Action m_RepaintWantedCallback;
    private System.Action<bool> m_ItemSelectedCallback;
    private System.Action m_KeyboardInputCallback;
    private System.Action m_GotKeyboardFocus;
    private Func<Rect, float> m_DrawLocalAssetHeader;
    private System.Action m_AssetStoreSearchEnded;
    internal static bool s_Debug;

    public bool allowDragging { get; set; }

    public bool allowRenaming { get; set; }

    public bool allowMultiSelect { get; set; }

    public bool allowDeselection { get; set; }

    public bool allowFocusRendering { get; set; }

    public bool allowBuiltinResources { get; set; }

    public bool allowUserRenderingHook { get; set; }

    public bool allowFindNextShortcut { get; set; }

    public bool foldersFirst { get; set; }

    public System.Action repaintCallback
    {
      get
      {
        return this.m_RepaintWantedCallback;
      }
      set
      {
        this.m_RepaintWantedCallback = value;
      }
    }

    public System.Action<bool> itemSelectedCallback
    {
      get
      {
        return this.m_ItemSelectedCallback;
      }
      set
      {
        this.m_ItemSelectedCallback = value;
      }
    }

    public System.Action keyboardCallback
    {
      get
      {
        return this.m_KeyboardInputCallback;
      }
      set
      {
        this.m_KeyboardInputCallback = value;
      }
    }

    public System.Action gotKeyboardFocus
    {
      get
      {
        return this.m_GotKeyboardFocus;
      }
      set
      {
        this.m_GotKeyboardFocus = value;
      }
    }

    public System.Action assetStoreSearchEnded
    {
      get
      {
        return this.m_AssetStoreSearchEnded;
      }
      set
      {
        this.m_AssetStoreSearchEnded = value;
      }
    }

    public Func<Rect, float> drawLocalAssetHeader
    {
      get
      {
        return this.m_DrawLocalAssetHeader;
      }
      set
      {
        this.m_DrawLocalAssetHeader = value;
      }
    }

    public int gridSize
    {
      get
      {
        return this.m_State.m_GridSize;
      }
      set
      {
        if (this.m_State.m_GridSize == value)
          return;
        this.m_State.m_GridSize = value;
        this.m_FrameLastClickedItem = true;
      }
    }

    public int minGridSize
    {
      get
      {
        return this.m_MinGridSize;
      }
    }

    public int maxGridSize
    {
      get
      {
        return this.m_MaxGridSize;
      }
    }

    public int numItemsDisplayed
    {
      get
      {
        return this.m_LocalAssets.ItemCount;
      }
    }

    public ObjectListArea(ObjectListAreaState state, EditorWindow owner, bool showNoneItem)
    {
      this.m_State = state;
      this.m_Owner = owner;
      AssetStorePreviewManager.MaxCachedImages = 72;
      this.m_StoreAssets = new List<ObjectListArea.AssetStoreGroup>();
      this.m_RequeryAssetStore = false;
      this.m_LocalAssets = new ObjectListArea.LocalGroup(this, string.Empty, showNoneItem);
      this.m_Groups = new List<ObjectListArea.Group>();
      this.m_Groups.Add((ObjectListArea.Group) this.m_LocalAssets);
    }

    public void ShowObjectsInList(int[] instanceIDs)
    {
      this.Init(this.m_TotalRect, HierarchyType.Assets, new SearchFilter(), false);
      this.m_LocalAssets.ShowObjectsInList(instanceIDs);
    }

    public void Init(Rect rect, HierarchyType hierarchyType, SearchFilter searchFilter, bool checkThumbnails)
    {
      this.m_TotalRect = this.m_VisibleRect = rect;
      this.m_LocalAssets.UpdateFilter(hierarchyType, searchFilter, this.foldersFirst);
      this.m_LocalAssets.UpdateAssets();
      using (List<ObjectListArea.AssetStoreGroup>.Enumerator enumerator = this.m_StoreAssets.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.UpdateFilter(hierarchyType, searchFilter, this.foldersFirst);
      }
      bool flag = searchFilter.GetState() == SearchFilter.State.FolderBrowsing;
      if (flag)
      {
        this.m_LastAssetStoreQuerySearchFilter = string.Empty;
        this.m_LastAssetStoreQueryClassName = new string[0];
        this.m_LastAssetStoreQueryLabels = new string[0];
      }
      else
      {
        this.m_LastAssetStoreQuerySearchFilter = searchFilter.nameFilter != null ? searchFilter.nameFilter : string.Empty;
        this.m_LastAssetStoreQueryClassName = searchFilter.classNames != null && Array.IndexOf<string>(searchFilter.classNames, "Object") < 0 ? searchFilter.classNames : new string[0];
        this.m_LastAssetStoreQueryLabels = searchFilter.assetLabels != null ? searchFilter.assetLabels : new string[0];
      }
      this.m_LastAssetStoreQueryChangeTime = EditorApplication.timeSinceStartup;
      this.m_RequeryAssetStore = true;
      this.m_ShowLocalAssetsOnly = flag || searchFilter.GetState() != SearchFilter.State.SearchingInAssetStore;
      this.m_AssetStoreError = string.Empty;
      this.m_AllowThumbnails = !checkThumbnails || this.ObjectsHaveThumbnails(hierarchyType, searchFilter);
      this.Repaint();
      this.ClearCroppedLabelCache();
      this.SetupData(true);
    }

    private bool HasFocus()
    {
      if (!this.allowFocusRendering)
        return true;
      if (this.m_KeyboardControlID == GUIUtility.keyboardControl)
        return this.m_Owner.m_Parent.hasFocus;
      return false;
    }

    private void QueryAssetStore()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ObjectListArea.\u003CQueryAssetStore\u003Ec__AnonStorey3B storeCAnonStorey3B = new ObjectListArea.\u003CQueryAssetStore\u003Ec__AnonStorey3B();
      // ISSUE: reference to a compiler-generated field
      storeCAnonStorey3B.\u003C\u003Ef__this = this;
      bool requeryAssetStore = this.m_RequeryAssetStore;
      this.m_RequeryAssetStore = false;
      if (this.m_ShowLocalAssetsOnly && !this.ShowAssetStoreHitsWhileSearchingLocalAssets())
        return;
      bool flag = this.m_LastAssetStoreQuerySearchFilter != string.Empty || this.m_LastAssetStoreQueryClassName.Length != 0 || this.m_LastAssetStoreQueryLabels.Length != 0;
      if (this.m_QueryInProgress)
        return;
      if (!flag)
        this.ClearAssetStoreGroups();
      else if (this.m_LastAssetStoreQueryChangeTime + 0.2 > EditorApplication.timeSinceStartup)
      {
        this.m_RequeryAssetStore = true;
        this.Repaint();
      }
      else
      {
        this.m_QueryInProgress = true;
        // ISSUE: reference to a compiler-generated field
        storeCAnonStorey3B.queryFilter = this.m_LastAssetStoreQuerySearchFilter + (object) this.m_LastAssetStoreQueryClassName + (object) this.m_LastAssetStoreQueryLabels;
        // ISSUE: reference to a compiler-generated method
        AssetStoreResultBase<AssetStoreSearchResults>.Callback callback = new AssetStoreResultBase<AssetStoreSearchResults>.Callback(storeCAnonStorey3B.\u003C\u003Em__51);
        List<AssetStoreClient.SearchCount> counts = new List<AssetStoreClient.SearchCount>();
        if (!requeryAssetStore)
        {
          using (List<ObjectListArea.AssetStoreGroup>.Enumerator enumerator = this.m_StoreAssets.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ObjectListArea.AssetStoreGroup current = enumerator.Current;
              AssetStoreClient.SearchCount searchCount = new AssetStoreClient.SearchCount();
              if (current.Visible && current.NeedItems)
              {
                searchCount.offset = current.Assets.Count;
                searchCount.limit = current.ItemsWantedShown - searchCount.offset;
              }
              searchCount.name = current.Name;
              counts.Add(searchCount);
            }
          }
        }
        AssetStoreClient.SearchAssets(this.m_LastAssetStoreQuerySearchFilter, this.m_LastAssetStoreQueryClassName, this.m_LastAssetStoreQueryLabels, counts, callback);
      }
    }

    private void EnsureAssetStoreGroupsAreOpenIfAllClosed()
    {
      if (this.m_StoreAssets.Count <= 0)
        return;
      int num1 = 0;
      using (List<ObjectListArea.AssetStoreGroup>.Enumerator enumerator = this.m_StoreAssets.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.Visible)
            ++num1;
        }
      }
      if (num1 != 0)
        return;
      using (List<ObjectListArea.AssetStoreGroup>.Enumerator enumerator = this.m_StoreAssets.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ObjectListArea.AssetStoreGroup current = enumerator.Current;
          ObjectListArea.AssetStoreGroup assetStoreGroup = current;
          bool flag = true;
          current.visiblePreference = flag;
          int num2 = flag ? 1 : 0;
          assetStoreGroup.Visible = num2 != 0;
        }
      }
    }

    private void RequeryAssetStore()
    {
      this.m_RequeryAssetStore = true;
    }

    private void ClearAssetStoreGroups()
    {
      this.m_Groups.Clear();
      this.m_Groups.Add((ObjectListArea.Group) this.m_LocalAssets);
      this.m_StoreAssets.Clear();
      this.Repaint();
    }

    public string GetAssetStoreButtonText()
    {
      string str1 = "Asset Store";
      if (this.ShowAssetStoreHitsWhileSearchingLocalAssets())
      {
        for (int index = 0; index < this.m_StoreAssets.Count; ++index)
        {
          string str2 = index != 0 ? str1 + " ∕ " : str1 + ": ";
          ObjectListArea.AssetStoreGroup storeAsset = this.m_StoreAssets[index];
          str1 = str2 + (storeAsset.ItemsAvailable <= 999 ? storeAsset.ItemsAvailable.ToString() : "999+");
        }
      }
      return str1;
    }

    private bool ShowAssetStoreHitsWhileSearchingLocalAssets()
    {
      return EditorPrefs.GetBool("ShowAssetStoreSearchHits", true);
    }

    public void ShowAssetStoreHitCountWhileSearchingLocalAssetsChanged()
    {
      if (this.ShowAssetStoreHitsWhileSearchingLocalAssets())
        this.RequeryAssetStore();
      else if (this.m_ShowLocalAssetsOnly)
        this.ClearAssetStoreGroups();
      this.Repaint();
    }

    internal float GetVisibleWidth()
    {
      return this.m_VisibleRect.width;
    }

    public void OnGUI(Rect position, int keyboardControlID)
    {
      if (ObjectListArea.s_Styles == null)
        ObjectListArea.s_Styles = new ObjectListArea.Styles();
      ObjectListArea.s_VCEnabled = Provider.isActive;
      Event current = Event.current;
      this.m_TotalRect = position;
      this.FrameLastClickedItemIfWanted();
      GUI.Label(this.m_TotalRect, GUIContent.none, ObjectListArea.s_Styles.iconAreaBg);
      this.m_KeyboardControlID = keyboardControlID;
      if (current.type == EventType.MouseDown && position.Contains(Event.current.mousePosition))
      {
        GUIUtility.keyboardControl = this.m_KeyboardControlID;
        this.m_AllowRenameOnMouseUp = true;
        this.Repaint();
      }
      bool flag = this.m_KeyboardControlID == GUIUtility.keyboardControl;
      if (flag != this.m_State.m_HadKeyboardFocusLastEvent)
      {
        this.m_State.m_HadKeyboardFocusLastEvent = flag;
        if (flag)
        {
          if (current.type == EventType.MouseDown)
            this.m_AllowRenameOnMouseUp = false;
          if (this.m_GotKeyboardFocus != null)
            this.m_GotKeyboardFocus();
        }
      }
      int instanceID;
      if (current.keyCode == KeyCode.Tab && current.type == EventType.KeyDown && (!flag && !this.IsShowingAny(this.GetSelection())) && this.m_LocalAssets.InstanceIdAtIndex(0, out instanceID))
        Selection.activeInstanceID = instanceID;
      this.HandleKeyboard(true);
      this.HandleZoomScrolling();
      this.HandleListArea();
      this.DoOffsetSelection();
      this.HandleUnusedEvents();
    }

    private void FrameLastClickedItemIfWanted()
    {
      if (!this.m_FrameLastClickedItem || Event.current.type != EventType.Repaint)
        return;
      this.m_FrameLastClickedItem = false;
      if (this.m_State.m_SelectedInstanceIDs.Count <= 0 || EditorApplication.timeSinceStartup - this.m_LocalAssets.m_LastClickedDrawTime >= 0.2)
        return;
      this.Frame(this.m_State.m_LastClickedInstanceID, true, false);
    }

    private void HandleUnusedEvents()
    {
      if (!this.allowDeselection || Event.current.type != EventType.MouseDown || (Event.current.button != 0 || !this.m_TotalRect.Contains(Event.current.mousePosition)))
        return;
      this.SetSelection(new int[0], false);
    }

    public bool CanShowThumbnails()
    {
      return this.m_AllowThumbnails;
    }

    private static string CreateFilterString(string searchString, string requiredClassName)
    {
      string str = searchString;
      if (!string.IsNullOrEmpty(requiredClassName))
        str = str + " t:" + requiredClassName;
      return str;
    }

    private bool ObjectsHaveThumbnails(HierarchyType type, SearchFilter searchFilter)
    {
      if (this.m_LocalAssets.HasBuiltinResources)
        return true;
      IHierarchyProperty propertyForFilter = FilteredHierarchyProperty.CreateHierarchyPropertyForFilter(new FilteredHierarchy(type)
      {
        searchFilter = searchFilter
      });
      int[] expanded = new int[0];
      if (propertyForFilter.CountRemaining(expanded) == 0)
        return true;
      propertyForFilter.Reset();
      while (propertyForFilter.Next(expanded))
      {
        if (propertyForFilter.hasFullPreviewImage)
          return true;
      }
      return false;
    }

    internal void OnDestroy()
    {
      AssetPreview.DeletePreviewTextureManagerByID(this.GetAssetPreviewManagerID());
    }

    private void Repaint()
    {
      if (this.m_RepaintWantedCallback == null)
        return;
      this.m_RepaintWantedCallback();
    }

    public void OnEvent()
    {
      this.GetRenameOverlay().OnEvent();
    }

    private CreateAssetUtility GetCreateAssetUtility()
    {
      return this.m_State.m_CreateAssetUtility;
    }

    private RenameOverlay GetRenameOverlay()
    {
      return this.m_State.m_RenameOverlay;
    }

    internal void BeginNamingNewAsset(string newAssetName, int instanceID, bool isCreatingNewFolder)
    {
      this.m_State.m_NewAssetIndexInList = this.m_LocalAssets.IndexOfNewText(newAssetName, isCreatingNewFolder, this.foldersFirst);
      if (this.m_State.m_NewAssetIndexInList != -1)
      {
        this.Frame(instanceID, true, false);
        this.GetRenameOverlay().BeginRename(newAssetName, instanceID, 0.0f);
      }
      else
        Debug.LogError((object) "Failed to insert new asset into list");
      this.Repaint();
    }

    public bool BeginRename(float delay)
    {
      if (!this.allowRenaming || this.m_State.m_SelectedInstanceIDs.Count != 1)
        return false;
      int selectedInstanceId = this.m_State.m_SelectedInstanceIDs[0];
      if (AssetDatabase.IsSubAsset(selectedInstanceId) || this.m_LocalAssets.IsBuiltinAsset(selectedInstanceId) || !AssetDatabase.Contains(selectedInstanceId))
        return false;
      string nameOfLocalAsset = this.m_LocalAssets.GetNameOfLocalAsset(selectedInstanceId);
      if (nameOfLocalAsset == null)
        return false;
      return this.GetRenameOverlay().BeginRename(nameOfLocalAsset, selectedInstanceId, delay);
    }

    public void EndRename(bool acceptChanges)
    {
      if (!this.GetRenameOverlay().IsRenaming())
        return;
      this.GetRenameOverlay().EndRename(acceptChanges);
      this.RenameEnded();
    }

    private void RenameEnded()
    {
      string name = !string.IsNullOrEmpty(this.GetRenameOverlay().name) ? this.GetRenameOverlay().name : this.GetRenameOverlay().originalName;
      int userData = this.GetRenameOverlay().userData;
      if (this.GetCreateAssetUtility().IsCreatingNewAsset())
      {
        if (this.GetRenameOverlay().userAcceptedRename)
          this.GetCreateAssetUtility().EndNewAssetCreation(name);
      }
      else if (this.GetRenameOverlay().userAcceptedRename)
        ObjectNames.SetNameSmartWithInstanceID(userData, name);
      if (this.GetRenameOverlay().HasKeyboardFocus())
        GUIUtility.keyboardControl = this.m_KeyboardControlID;
      if (this.GetRenameOverlay().userAcceptedRename)
        this.Frame(userData, true, false);
      this.ClearRenameState();
    }

    private void ClearRenameState()
    {
      this.GetRenameOverlay().Clear();
      this.GetCreateAssetUtility().Clear();
      this.m_State.m_NewAssetIndexInList = -1;
    }

    internal void HandleRenameOverlay()
    {
      if (!this.GetRenameOverlay().IsRenaming() || this.GetRenameOverlay().OnGUI(!this.IsListMode() ? ObjectListArea.s_Styles.miniRenameField : (GUIStyle) null))
        return;
      this.RenameEnded();
      GUIUtility.ExitGUI();
    }

    public bool IsSelected(int instanceID)
    {
      return this.m_State.m_SelectedInstanceIDs.Contains(instanceID);
    }

    public int[] GetSelection()
    {
      return this.m_State.m_SelectedInstanceIDs.ToArray();
    }

    public bool IsLastClickedItemVisible()
    {
      return this.GetSelectedAssetIdx() >= 0;
    }

    public void SelectAll()
    {
      this.SetSelection(this.m_LocalAssets.GetInstanceIDs().ToArray(), false);
    }

    private void SetSelection(int[] selectedInstanceIDs, bool doubleClicked)
    {
      this.InitSelection(selectedInstanceIDs);
      if (this.m_ItemSelectedCallback == null)
        return;
      this.Repaint();
      this.m_ItemSelectedCallback(doubleClicked);
    }

    public void InitSelection(int[] selectedInstanceIDs)
    {
      this.m_State.m_SelectedInstanceIDs = new List<int>((IEnumerable<int>) selectedInstanceIDs);
      if (this.m_State.m_SelectedInstanceIDs.Count > 0)
      {
        if (!this.m_State.m_SelectedInstanceIDs.Contains(this.m_State.m_LastClickedInstanceID))
          this.m_State.m_LastClickedInstanceID = this.m_State.m_SelectedInstanceIDs[this.m_State.m_SelectedInstanceIDs.Count - 1];
      }
      else
        this.m_State.m_LastClickedInstanceID = 0;
      if (!(Selection.activeObject == (UnityEngine.Object) null) && Selection.activeObject.GetType() == typeof (AssetStoreAssetInspector))
        return;
      this.selectedAssetStoreAsset = false;
      AssetStoreAssetSelection.Clear();
    }

    private void SetSelection(AssetStoreAsset assetStoreResult, bool doubleClicked)
    {
      this.m_State.m_SelectedInstanceIDs.Clear();
      this.selectedAssetStoreAsset = true;
      AssetStoreAssetSelection.Clear();
      Texture2D image = AssetStorePreviewManager.TextureFromUrl(assetStoreResult.staticPreviewURL, assetStoreResult.name, this.gridSize, ObjectListArea.s_Styles.resultsGridLabel, ObjectListArea.s_Styles.resultsGrid, true).image;
      AssetStoreAssetSelection.AddAsset(assetStoreResult, image);
      if (this.m_ItemSelectedCallback == null)
        return;
      this.Repaint();
      this.m_ItemSelectedCallback(doubleClicked);
    }

    private void HandleZoomScrolling()
    {
      if (!EditorGUI.actionKey || Event.current.type != EventType.ScrollWheel || !this.m_TotalRect.Contains(Event.current.mousePosition))
        return;
      int num = (double) Event.current.delta.y <= 0.0 ? 1 : -1;
      this.gridSize = Mathf.Clamp(this.gridSize + num * 7, this.minGridSize, this.maxGridSize);
      if (num < 0 && this.gridSize < this.m_MinIconSize)
        this.gridSize = this.m_MinGridSize;
      if (num > 0 && this.gridSize < this.m_MinIconSize)
        this.gridSize = this.m_MinIconSize;
      Event.current.Use();
      GUI.changed = true;
    }

    private bool IsPreviewIconExpansionModifierPressed()
    {
      return Event.current.alt;
    }

    private bool AllowLeftRightArrowNavigation()
    {
      bool flag1 = !this.m_LocalAssets.ListMode && !this.IsPreviewIconExpansionModifierPressed();
      bool flag2 = !this.m_ShowLocalAssetsOnly || this.m_LocalAssets.ItemCount > 1;
      if (flag1)
        return flag2;
      return false;
    }

    public void HandleKeyboard(bool checkKeyboardControl)
    {
      if (checkKeyboardControl && GUIUtility.keyboardControl != this.m_KeyboardControlID || !GUI.enabled)
        return;
      if (this.m_KeyboardInputCallback != null)
        this.m_KeyboardInputCallback();
      if (Event.current.type != EventType.KeyDown)
        return;
      int num = 0;
      if (this.IsLastClickedItemVisible())
      {
        switch (Event.current.keyCode)
        {
          case KeyCode.UpArrow:
            num = -this.m_LocalAssets.m_Grid.columns;
            break;
          case KeyCode.DownArrow:
            num = this.m_LocalAssets.m_Grid.columns;
            break;
          case KeyCode.RightArrow:
            if (this.AllowLeftRightArrowNavigation())
            {
              num = 1;
              break;
            }
            break;
          case KeyCode.LeftArrow:
            if (this.AllowLeftRightArrowNavigation())
            {
              num = -1;
              break;
            }
            break;
          case KeyCode.Home:
            num = int.MinValue;
            break;
          case KeyCode.End:
            num = int.MaxValue;
            break;
          case KeyCode.PageUp:
            num = -2147483647;
            break;
          case KeyCode.PageDown:
            num = 2147483646;
            break;
        }
      }
      else
      {
        bool flag = false;
        switch (Event.current.keyCode)
        {
          case KeyCode.UpArrow:
          case KeyCode.DownArrow:
          case KeyCode.Home:
          case KeyCode.End:
          case KeyCode.PageUp:
          case KeyCode.PageDown:
            flag = true;
            break;
          case KeyCode.RightArrow:
          case KeyCode.LeftArrow:
            flag = this.AllowLeftRightArrowNavigation();
            break;
        }
        if (flag)
        {
          this.SelectFirst();
          Event.current.Use();
        }
      }
      if (num != 0)
      {
        if (this.GetSelectedAssetIdx() < 0 && !this.m_LocalAssets.ShowNone)
          this.SetSelectedAssetByIdx(0);
        else
          this.m_SelectionOffset = num;
        Event.current.Use();
        GUI.changed = true;
      }
      else
      {
        if (!this.allowFindNextShortcut || !this.m_LocalAssets.DoCharacterOffsetSelection())
          return;
        Event.current.Use();
      }
    }

    private void DoOffsetSelectionSpecialKeys(int idx, int maxIndex)
    {
      float num = this.m_LocalAssets.m_Grid.itemSize.y + this.m_LocalAssets.m_Grid.verticalSpacing;
      int columns = this.m_LocalAssets.m_Grid.columns;
      switch (this.m_SelectionOffset)
      {
        case 2147483646:
          if (Application.platform == RuntimePlatform.OSXEditor)
          {
            this.m_State.m_ScrollPosition.y += this.m_TotalRect.height;
            this.m_SelectionOffset = 0;
            break;
          }
          this.m_SelectionOffset = Mathf.RoundToInt(this.m_TotalRect.height / num) * columns;
          this.m_SelectionOffset = Mathf.Min(Mathf.FloorToInt((float) (maxIndex - idx) / (float) columns) * columns, this.m_SelectionOffset);
          break;
        case int.MaxValue:
          this.m_SelectionOffset = maxIndex - idx;
          break;
        case int.MinValue:
          this.m_SelectionOffset = 0;
          this.SetSelectedAssetByIdx(0);
          break;
        case -2147483647:
          if (Application.platform == RuntimePlatform.OSXEditor)
          {
            this.m_State.m_ScrollPosition.y -= this.m_TotalRect.height;
            this.m_SelectionOffset = 0;
            break;
          }
          this.m_SelectionOffset = -Mathf.RoundToInt(this.m_TotalRect.height / num) * columns;
          this.m_SelectionOffset = Mathf.Max(-Mathf.FloorToInt((float) idx / (float) columns) * columns, this.m_SelectionOffset);
          break;
      }
    }

    private void DoOffsetSelection()
    {
      if (this.m_SelectionOffset == 0)
        return;
      int maxIdx = this.GetMaxIdx();
      if (this.maxGridSize == -1)
        return;
      int selectedAssetIdx = this.GetSelectedAssetIdx();
      int idx = selectedAssetIdx >= 0 ? selectedAssetIdx : 0;
      this.DoOffsetSelectionSpecialKeys(idx, maxIdx);
      if (this.m_SelectionOffset == 0)
        return;
      int a = idx + this.m_SelectionOffset;
      this.m_SelectionOffset = 0;
      this.SetSelectedAssetByIdx(a >= 0 ? Mathf.Min(a, maxIdx) : idx);
    }

    public void OffsetSelection(int selectionOffset)
    {
      this.m_SelectionOffset = selectionOffset;
    }

    public void SelectFirst()
    {
      int selectedIdx = 0;
      if (this.m_ShowLocalAssetsOnly && this.m_LocalAssets.ShowNone && this.m_LocalAssets.ItemCount > 1)
        selectedIdx = 1;
      this.SetSelectedAssetByIdx(selectedIdx);
    }

    public int GetInstanceIDByIndex(int index)
    {
      int instanceID;
      if (this.m_LocalAssets.InstanceIdAtIndex(index, out instanceID))
        return instanceID;
      return 0;
    }

    private void SetSelectedAssetByIdx(int selectedIdx)
    {
      int instanceID;
      if (this.m_LocalAssets.InstanceIdAtIndex(selectedIdx, out instanceID))
      {
        this.ScrollToPosition(ObjectListArea.AdjustRectForFraming(this.m_LocalAssets.m_Grid.CalcRect(selectedIdx, 0.0f)));
        this.Repaint();
        int[] selectedInstanceIDs;
        if (this.IsLocalAssetsCurrentlySelected())
          selectedInstanceIDs = this.m_LocalAssets.GetNewSelection(instanceID, false, true).ToArray();
        else
          selectedInstanceIDs = new int[1]{ instanceID };
        this.SetSelection(selectedInstanceIDs, false);
        this.m_State.m_LastClickedInstanceID = instanceID;
      }
      else
      {
        selectedIdx -= this.m_LocalAssets.m_Grid.rows * this.m_LocalAssets.m_Grid.columns;
        float height = this.m_LocalAssets.Height;
        using (List<ObjectListArea.AssetStoreGroup>.Enumerator enumerator = this.m_StoreAssets.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ObjectListArea.AssetStoreGroup current = enumerator.Current;
            if (!current.Visible)
            {
              height += current.Height;
            }
            else
            {
              AssetStoreAsset assetStoreResult = current.AssetAtIndex(selectedIdx);
              if (assetStoreResult != null)
              {
                this.ScrollToPosition(ObjectListArea.AdjustRectForFraming(current.m_Grid.CalcRect(selectedIdx, height)));
                this.Repaint();
                this.SetSelection(assetStoreResult, false);
                break;
              }
              selectedIdx -= current.m_Grid.rows * current.m_Grid.columns;
              height += current.Height;
            }
          }
        }
      }
    }

    private void Reveal(int instanceID)
    {
      if (!AssetDatabase.Contains(instanceID))
        return;
      int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID(AssetDatabase.GetAssetPath(instanceID));
      if (mainAssetInstanceId == instanceID)
        return;
      this.m_LocalAssets.ChangeExpandedState(mainAssetInstanceId, true);
    }

    public bool Frame(int instanceID, bool frame, bool ping)
    {
      if (ObjectListArea.s_Styles == null)
        ObjectListArea.s_Styles = new ObjectListArea.Styles();
      int itemIdx = -1;
      if (this.GetCreateAssetUtility().IsCreatingNewAsset() && this.m_State.m_NewAssetIndexInList != -1 && this.GetCreateAssetUtility().instanceID == instanceID)
        itemIdx = this.m_State.m_NewAssetIndexInList;
      if (frame)
        this.Reveal(instanceID);
      if (itemIdx == -1)
        itemIdx = this.m_LocalAssets.IndexOf(instanceID);
      if (itemIdx == -1)
        return false;
      if (frame)
      {
        float yOffset = 0.0f;
        this.CenterRect(ObjectListArea.AdjustRectForFraming(this.m_LocalAssets.m_Grid.CalcRect(itemIdx, yOffset)));
        this.Repaint();
      }
      if (ping)
        this.BeginPing(instanceID);
      return true;
    }

    private int GetSelectedAssetIdx()
    {
      int num1 = this.m_LocalAssets.IndexOf(this.m_State.m_LastClickedInstanceID);
      if (num1 != -1)
        return num1;
      int num2 = this.m_LocalAssets.m_Grid.rows * this.m_LocalAssets.m_Grid.columns;
      if (AssetStoreAssetSelection.Count == 0)
        return -1;
      AssetStoreAsset firstAsset = AssetStoreAssetSelection.GetFirstAsset();
      if (firstAsset == null)
        return -1;
      int id = firstAsset.id;
      using (List<ObjectListArea.AssetStoreGroup>.Enumerator enumerator = this.m_StoreAssets.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ObjectListArea.AssetStoreGroup current = enumerator.Current;
          if (current.Visible)
          {
            int num3 = current.IndexOf(id);
            if (num3 != -1)
              return num2 + num3;
            num2 += current.m_Grid.rows * current.m_Grid.columns;
          }
        }
      }
      return -1;
    }

    private bool SkipGroup(ObjectListArea.Group group)
    {
      if (this.m_ShowLocalAssetsOnly)
      {
        if (group is ObjectListArea.AssetStoreGroup)
          return true;
      }
      else if (group is ObjectListArea.LocalGroup)
        return true;
      return false;
    }

    private int GetMaxIdx()
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      using (List<ObjectListArea.Group>.Enumerator enumerator = this.m_Groups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ObjectListArea.Group current = enumerator.Current;
          if (!this.SkipGroup(current) && current.Visible)
          {
            num2 += num3;
            num3 = current.m_Grid.rows * current.m_Grid.columns;
            num1 = current.ItemCount - 1;
          }
        }
      }
      int num4 = num2 + num1;
      if (num3 + num4 == 0)
        return -1;
      return num4;
    }

    private bool IsLocalAssetsCurrentlySelected()
    {
      int instanceID = this.m_State.m_SelectedInstanceIDs.FirstOrDefault<int>();
      if (instanceID != 0)
        return this.m_LocalAssets.IndexOf(instanceID) != -1;
      return false;
    }

    private void SetupData(bool forceReflow)
    {
      using (List<ObjectListArea.Group>.Enumerator enumerator = this.m_Groups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ObjectListArea.Group current = enumerator.Current;
          if (!this.SkipGroup(current))
            current.UpdateAssets();
        }
      }
      if (!forceReflow && Event.current.type != EventType.Repaint)
        return;
      this.Reflow();
    }

    private bool IsObjectSelector()
    {
      return this.m_LocalAssets.ShowNone;
    }

    private void HandleListArea()
    {
      this.SetupData(false);
      if (!this.IsObjectSelector() && !this.m_QueryInProgress && (this.m_StoreAssets.Exists((Predicate<ObjectListArea.AssetStoreGroup>) (g => g.NeedItems)) || this.m_RequeryAssetStore))
        this.QueryAssetStore();
      float height = 0.0f;
      using (List<ObjectListArea.Group>.Enumerator enumerator = this.m_Groups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ObjectListArea.Group current = enumerator.Current;
          if (!this.SkipGroup(current))
          {
            height += current.Height;
            if (this.m_LocalAssets.ShowNone)
              break;
          }
        }
      }
      Rect totalRect = this.m_TotalRect;
      Rect viewRect = new Rect(0.0f, 0.0f, 1f, height);
      bool flag1 = (double) height > (double) this.m_TotalRect.height;
      this.m_VisibleRect = this.m_TotalRect;
      if (flag1)
        this.m_VisibleRect.width -= 16f;
      double timeSinceStartup = EditorApplication.timeSinceStartup;
      this.m_LastScrollPosition = this.m_State.m_ScrollPosition;
      bool flag2 = false;
      this.m_State.m_ScrollPosition = GUI.BeginScrollView(totalRect, this.m_State.m_ScrollPosition, viewRect);
      Vector2 scrollPosition = this.m_State.m_ScrollPosition;
      if (this.m_LastScrollPosition != this.m_State.m_ScrollPosition)
        this.LastScrollTime = timeSinceStartup;
      float yOffset = 0.0f;
      using (List<ObjectListArea.Group>.Enumerator enumerator = this.m_Groups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ObjectListArea.Group current = enumerator.Current;
          if (!this.SkipGroup(current))
          {
            current.Draw(yOffset, scrollPosition);
            flag2 = flag2 || current.NeedsRepaint;
            yOffset += current.Height;
            if (this.m_LocalAssets.ShowNone)
              break;
          }
        }
      }
      this.HandlePing();
      if (flag2)
        this.Repaint();
      GUI.EndScrollView();
      if (this.m_ResizePreviewCacheTo > 0 && AssetStorePreviewManager.MaxCachedImages != this.m_ResizePreviewCacheTo)
        AssetStorePreviewManager.MaxCachedImages = this.m_ResizePreviewCacheTo;
      if (Event.current.type == EventType.Repaint)
        AssetStorePreviewManager.AbortOlderThan(timeSinceStartup);
      if (this.m_ShowLocalAssetsOnly || string.IsNullOrEmpty(this.m_AssetStoreError))
        return;
      Vector2 vector2 = EditorStyles.label.CalcSize(ObjectListArea.s_Styles.m_AssetStoreNotAvailableText);
      Rect position = new Rect(this.m_TotalRect.x + 2f + Mathf.Max(0.0f, (float) (((double) this.m_TotalRect.width - (double) vector2.x) * 0.5)), this.m_TotalRect.y + 10f, vector2.x, 20f);
      EditorGUI.BeginDisabledGroup(true);
      GUI.Label(position, ObjectListArea.s_Styles.m_AssetStoreNotAvailableText, EditorStyles.label);
      EditorGUI.EndDisabledGroup();
    }

    private bool IsListMode()
    {
      if (this.allowMultiSelect)
        return this.gridSize == 16;
      if (this.gridSize != 16)
        return !this.CanShowThumbnails();
      return true;
    }

    private void Reflow()
    {
      if (this.gridSize < 20)
        this.gridSize = this.m_MinGridSize;
      else if (this.gridSize < this.m_MinIconSize)
        this.gridSize = this.m_MinIconSize;
      if (this.IsListMode())
      {
        using (List<ObjectListArea.Group>.Enumerator enumerator = this.m_Groups.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ObjectListArea.Group current = enumerator.Current;
            if (!this.SkipGroup(current))
            {
              current.ListMode = true;
              this.UpdateGroupSizes(current);
              if (this.m_LocalAssets.ShowNone)
                break;
            }
          }
        }
        this.m_ResizePreviewCacheTo = Mathf.CeilToInt(this.m_TotalRect.height / 16f) + 10;
      }
      else
      {
        float num = 0.0f;
        using (List<ObjectListArea.Group>.Enumerator enumerator = this.m_Groups.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ObjectListArea.Group current = enumerator.Current;
            if (!this.SkipGroup(current))
            {
              current.ListMode = false;
              this.UpdateGroupSizes(current);
              num += current.Height;
              if (this.m_LocalAssets.ShowNone)
                break;
            }
          }
        }
        if ((double) this.m_TotalRect.height < (double) num)
        {
          using (List<ObjectListArea.Group>.Enumerator enumerator = this.m_Groups.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ObjectListArea.Group current = enumerator.Current;
              if (!this.SkipGroup(current))
              {
                current.m_Grid.fixedWidth = this.m_TotalRect.width - 16f;
                current.m_Grid.InitNumRowsAndColumns(current.ItemCount, current.m_Grid.CalcRows(current.ItemsWantedShown));
                current.UpdateHeight();
                if (this.m_LocalAssets.ShowNone)
                  break;
              }
            }
          }
        }
        int maxNumVisibleItems = this.GetMaxNumVisibleItems();
        this.m_ResizePreviewCacheTo = maxNumVisibleItems * 2;
        AssetPreview.SetPreviewTextureCacheSize(maxNumVisibleItems * 2 + 30, this.GetAssetPreviewManagerID());
      }
    }

    private void UpdateGroupSizes(ObjectListArea.Group g)
    {
      if (g.ListMode)
      {
        g.m_Grid.fixedWidth = this.m_VisibleRect.width;
        g.m_Grid.itemSize = new Vector2(this.m_VisibleRect.width, 16f);
        g.m_Grid.topMargin = 0.0f;
        g.m_Grid.bottomMargin = 0.0f;
        g.m_Grid.leftMargin = 0.0f;
        g.m_Grid.rightMargin = 0.0f;
        g.m_Grid.verticalSpacing = 0.0f;
        g.m_Grid.minHorizontalSpacing = 0.0f;
        g.m_Grid.InitNumRowsAndColumns(g.ItemCount, g.ItemsWantedShown);
        g.UpdateHeight();
      }
      else
      {
        g.m_Grid.fixedWidth = this.m_TotalRect.width;
        g.m_Grid.itemSize = new Vector2((float) this.gridSize, (float) (this.gridSize + 14));
        g.m_Grid.topMargin = 10f;
        g.m_Grid.bottomMargin = 10f;
        g.m_Grid.leftMargin = 10f;
        g.m_Grid.rightMargin = 10f;
        g.m_Grid.verticalSpacing = 15f;
        g.m_Grid.minHorizontalSpacing = 12f;
        g.m_Grid.InitNumRowsAndColumns(g.ItemCount, g.m_Grid.CalcRows(g.ItemsWantedShown));
        g.UpdateHeight();
      }
    }

    private int GetMaxNumVisibleItems()
    {
      using (List<ObjectListArea.Group>.Enumerator enumerator = this.m_Groups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ObjectListArea.Group current = enumerator.Current;
          if (!this.SkipGroup(current))
            return current.m_Grid.GetMaxVisibleItems(this.m_TotalRect.height);
        }
      }
      return 0;
    }

    private static Rect AdjustRectForFraming(Rect r)
    {
      r.height += ObjectListArea.s_Styles.resultsGridLabel.fixedHeight * 2f;
      r.y -= ObjectListArea.s_Styles.resultsGridLabel.fixedHeight;
      return r;
    }

    private void CenterRect(Rect r)
    {
      this.m_State.m_ScrollPosition.y = (float) (((double) r.yMax + (double) r.yMin) / 2.0) - this.m_TotalRect.height / 2f;
      this.ScrollToPosition(r);
    }

    private void ScrollToPosition(Rect r)
    {
      float y = r.y;
      float yMax = r.yMax;
      float height = this.m_TotalRect.height;
      if ((double) yMax > (double) height + (double) this.m_State.m_ScrollPosition.y)
        this.m_State.m_ScrollPosition.y = yMax - height;
      if ((double) y < (double) this.m_State.m_ScrollPosition.y)
        this.m_State.m_ScrollPosition.y = y;
      this.m_State.m_ScrollPosition.y = Mathf.Max(this.m_State.m_ScrollPosition.y, 0.0f);
    }

    public void OnInspectorUpdate()
    {
      if (EditorApplication.timeSinceStartup > this.m_NextDirtyCheck && this.m_LocalAssets.IsAnyLastRenderedAssetsDirty())
      {
        AssetPreview.ClearTemporaryAssetPreviews();
        this.Repaint();
        this.m_NextDirtyCheck = EditorApplication.timeSinceStartup + 0.77;
      }
      if (!AssetStorePreviewManager.CheckRepaint())
        return;
      this.Repaint();
    }

    private void ClearCroppedLabelCache()
    {
      this.m_InstanceIDToCroppedNameMap.Clear();
    }

    protected string GetCroppedLabelText(int instanceID, string fullText, float cropWidth)
    {
      if (this.m_WidthUsedForCroppingName != (int) cropWidth)
        this.ClearCroppedLabelCache();
      string str;
      if (!this.m_InstanceIDToCroppedNameMap.TryGetValue(instanceID, out str))
      {
        if (this.m_InstanceIDToCroppedNameMap.Count > this.GetMaxNumVisibleItems() * 2 + 30)
          this.ClearCroppedLabelCache();
        int thatFitWithinWidth = ObjectListArea.s_Styles.resultsGridLabel.GetNumCharactersThatFitWithinWidth(fullText, cropWidth);
        if (thatFitWithinWidth == -1)
        {
          this.Repaint();
          return fullText;
        }
        str = thatFitWithinWidth <= 1 || thatFitWithinWidth == fullText.Length ? fullText : fullText.Substring(0, thatFitWithinWidth - 1) + "…";
        this.m_InstanceIDToCroppedNameMap[instanceID] = str;
        this.m_WidthUsedForCroppingName = (int) cropWidth;
      }
      return str;
    }

    public bool IsShowing(int instanceID)
    {
      return this.m_LocalAssets.IndexOf(instanceID) >= 0;
    }

    public bool IsShowingAny(int[] instanceIDs)
    {
      if (instanceIDs.Length == 0)
        return false;
      foreach (int instanceId in instanceIDs)
      {
        if (this.IsShowing(instanceId))
          return true;
      }
      return false;
    }

    protected Texture GetIconByInstanceID(int instanceID)
    {
      Texture texture = (Texture) null;
      if (instanceID != 0)
        texture = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(instanceID));
      return texture;
    }

    internal int GetAssetPreviewManagerID()
    {
      return this.m_Owner.GetInstanceID();
    }

    public void BeginPing(int instanceID)
    {
      if (ObjectListArea.s_Styles == null)
        ObjectListArea.s_Styles = new ObjectListArea.Styles();
      int num1 = this.m_LocalAssets.IndexOf(instanceID);
      if (num1 == -1)
        return;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ObjectListArea.\u003CBeginPing\u003Ec__AnonStorey3F pingCAnonStorey3F = new ObjectListArea.\u003CBeginPing\u003Ec__AnonStorey3F();
      string fullText = (string) null;
      // ISSUE: reference to a compiler-generated field
      pingCAnonStorey3F.hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      // ISSUE: reference to a compiler-generated field
      if (pingCAnonStorey3F.hierarchyProperty.Find(instanceID, (int[]) null))
      {
        // ISSUE: reference to a compiler-generated field
        fullText = pingCAnonStorey3F.hierarchyProperty.name;
      }
      if (fullText == null)
        return;
      this.m_Ping.m_TimeStart = Time.realtimeSinceStartup;
      this.m_Ping.m_AvailableWidth = this.m_VisibleRect.width;
      this.m_pingIndex = num1;
      float num2 = !ObjectListArea.s_VCEnabled ? 0.0f : 14f;
      GUIContent content = new GUIContent(!this.m_LocalAssets.ListMode ? this.GetCroppedLabelText(instanceID, fullText, (float) this.m_WidthUsedForCroppingName) : fullText);
      // ISSUE: reference to a compiler-generated field
      pingCAnonStorey3F.label = content.text;
      if (this.m_LocalAssets.ListMode)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ObjectListArea.\u003CBeginPing\u003Ec__AnonStorey3E pingCAnonStorey3E = new ObjectListArea.\u003CBeginPing\u003Ec__AnonStorey3E();
        // ISSUE: reference to a compiler-generated field
        pingCAnonStorey3E.\u003C\u003Ef__ref\u002463 = pingCAnonStorey3F;
        this.m_Ping.m_PingStyle = ObjectListArea.s_Styles.ping;
        Vector2 vector2 = this.m_Ping.m_PingStyle.CalcSize(content);
        this.m_Ping.m_ContentRect.width = (float) ((double) vector2.x + (double) num2 + 16.0);
        this.m_Ping.m_ContentRect.height = vector2.y;
        // ISSUE: reference to a compiler-generated field
        this.m_LeftPaddingForPinging = !pingCAnonStorey3F.hierarchyProperty.isMainRepresentation ? 28 : 16;
        // ISSUE: reference to a compiler-generated field
        pingCAnonStorey3E.res = this.m_LocalAssets.LookupByInstanceID(instanceID);
        // ISSUE: reference to a compiler-generated method
        this.m_Ping.m_ContentDraw = new System.Action<Rect>(pingCAnonStorey3E.\u003C\u003Em__53);
      }
      else
      {
        this.m_Ping.m_PingStyle = ObjectListArea.s_Styles.miniPing;
        Vector2 vector2 = this.m_Ping.m_PingStyle.CalcSize(content);
        this.m_Ping.m_ContentRect.width = vector2.x;
        this.m_Ping.m_ContentRect.height = vector2.y;
        // ISSUE: reference to a compiler-generated method
        this.m_Ping.m_ContentDraw = new System.Action<Rect>(pingCAnonStorey3F.\u003C\u003Em__54);
      }
      Vector2 pingPosition = this.CalculatePingPosition();
      this.m_Ping.m_ContentRect.x = pingPosition.x;
      this.m_Ping.m_ContentRect.y = pingPosition.y;
      this.Repaint();
    }

    public void EndPing()
    {
      this.m_Ping.m_TimeStart = -1f;
    }

    private void HandlePing()
    {
      if (this.m_Ping.isPinging && !this.m_LocalAssets.ListMode)
      {
        Vector2 pingPosition = this.CalculatePingPosition();
        this.m_Ping.m_ContentRect.x = pingPosition.x;
        this.m_Ping.m_ContentRect.y = pingPosition.y;
      }
      this.m_Ping.HandlePing();
      if (!this.m_Ping.isPinging)
        return;
      this.Repaint();
    }

    private Vector2 CalculatePingPosition()
    {
      Rect rect = this.m_LocalAssets.m_Grid.CalcRect(this.m_pingIndex, 0.0f);
      if (this.m_LocalAssets.ListMode)
        return new Vector2((float) this.m_LeftPaddingForPinging, rect.y);
      float width = this.m_Ping.m_ContentRect.width;
      return new Vector2(rect.center.x - width / 2f + (float) this.m_Ping.m_PingStyle.padding.left, (float) ((double) rect.yMax - (double) ObjectListArea.s_Styles.resultsGridLabel.fixedHeight + 3.0));
    }

    private class Styles
    {
      public GUIStyle resultsLabel = new GUIStyle((GUIStyle) "PR Label");
      public GUIStyle resultsGridLabel = ObjectListArea.Styles.GetStyle("ProjectBrowserGridLabel");
      public GUIStyle resultsGrid = (GUIStyle) "ObjectPickerResultsGrid";
      public GUIStyle background = (GUIStyle) "ObjectPickerBackground";
      public GUIStyle previewTextureBackground = (GUIStyle) "ObjectPickerPreviewBackground";
      public GUIStyle groupHeaderMiddle = ObjectListArea.Styles.GetStyle("ProjectBrowserHeaderBgMiddle");
      public GUIStyle groupHeaderTop = ObjectListArea.Styles.GetStyle("ProjectBrowserHeaderBgTop");
      public GUIStyle groupHeaderLabel = (GUIStyle) "Label";
      public GUIStyle groupHeaderLabelCount = (GUIStyle) "MiniLabel";
      public GUIStyle groupFoldout = (GUIStyle) "Foldout";
      public GUIStyle toolbarBack = (GUIStyle) "ObjectPickerToolbar";
      public GUIStyle miniRenameField = new GUIStyle((GUIStyle) "PR TextField");
      public GUIStyle ping = new GUIStyle((GUIStyle) "PR Ping");
      public GUIStyle miniPing = new GUIStyle((GUIStyle) "PR Ping");
      public GUIStyle iconDropShadow = ObjectListArea.Styles.GetStyle("ProjectBrowserIconDropShadow");
      public GUIStyle textureIconDropShadow = ObjectListArea.Styles.GetStyle("ProjectBrowserTextureIconDropShadow");
      public GUIStyle iconAreaBg = ObjectListArea.Styles.GetStyle("ProjectBrowserIconAreaBg");
      public GUIStyle previewBg = ObjectListArea.Styles.GetStyle("ProjectBrowserPreviewBg");
      public GUIStyle subAssetBg = ObjectListArea.Styles.GetStyle("ProjectBrowserSubAssetBg");
      public GUIStyle subAssetBgOpenEnded = ObjectListArea.Styles.GetStyle("ProjectBrowserSubAssetBgOpenEnded");
      public GUIStyle subAssetBgCloseEnded = ObjectListArea.Styles.GetStyle("ProjectBrowserSubAssetBgCloseEnded");
      public GUIStyle subAssetBgMiddle = ObjectListArea.Styles.GetStyle("ProjectBrowserSubAssetBgMiddle");
      public GUIStyle subAssetBgDivider = ObjectListArea.Styles.GetStyle("ProjectBrowserSubAssetBgDivider");
      public GUIStyle subAssetExpandButton = ObjectListArea.Styles.GetStyle("ProjectBrowserSubAssetExpandBtn");
      public GUIContent m_AssetStoreNotAvailableText = new GUIContent("The Asset Store is not available");
      public GUIStyle resultsFocusMarker;

      public Styles()
      {
        this.resultsFocusMarker = new GUIStyle(this.resultsGridLabel);
        GUIStyle resultsFocusMarker = this.resultsFocusMarker;
        float num1 = 0.0f;
        this.resultsFocusMarker.fixedWidth = num1;
        double num2 = (double) num1;
        resultsFocusMarker.fixedHeight = (float) num2;
        this.miniRenameField.font = EditorStyles.miniLabel.font;
        this.miniRenameField.alignment = TextAnchor.LowerCenter;
        this.ping.fixedHeight = 16f;
        this.ping.padding.right = 10;
        this.miniPing.font = EditorStyles.miniLabel.font;
        this.miniPing.alignment = TextAnchor.MiddleCenter;
        this.resultsLabel.alignment = TextAnchor.MiddleLeft;
      }

      private static GUIStyle GetStyle(string styleName)
      {
        return (GUIStyle) styleName;
      }
    }

    private class AssetStoreGroup : ObjectListArea.Group
    {
      private GUIContent m_Content = new GUIContent();
      public const int kDefaultRowsShown = 3;
      public const int kDefaultRowsShownListMode = 10;
      private const int kMoreButtonOffset = 3;
      private const int kMoreRowsAdded = 10;
      private const int kMoreRowsAddedListMode = 75;
      private const int kMaxQueryItems = 1000;
      private List<AssetStoreAsset> m_Assets;
      private string m_Name;
      private bool m_ListMode;
      private Vector3 m_ShowMoreDims;

      public string Name
      {
        get
        {
          return this.m_Name;
        }
      }

      public List<AssetStoreAsset> Assets
      {
        get
        {
          return this.m_Assets;
        }
        set
        {
          this.m_Assets = value;
        }
      }

      public override int ItemCount
      {
        get
        {
          return Math.Min(this.m_Assets.Count, this.ItemsWantedShown);
        }
      }

      public override bool ListMode
      {
        get
        {
          return this.m_ListMode;
        }
        set
        {
          this.m_ListMode = value;
        }
      }

      public bool NeedItems
      {
        get
        {
          int num = Math.Min(1000, this.ItemsWantedShown);
          int count = this.Assets.Count;
          if (this.ItemsAvailable >= num && count < num)
            return true;
          if (this.ItemsAvailable < num)
            return count < this.ItemsAvailable;
          return false;
        }
      }

      public override bool NeedsRepaint { get; protected set; }

      public AssetStoreGroup(ObjectListArea owner, string groupTitle, string groupName)
        : base(owner, groupTitle)
      {
        this.m_Assets = new List<AssetStoreAsset>();
        this.m_Name = groupName;
        this.m_ListMode = false;
        this.m_ShowMoreDims = (Vector3) EditorStyles.miniButton.CalcSize(new GUIContent("Show more"));
        this.m_Owner.UpdateGroupSizes((ObjectListArea.Group) this);
        this.ItemsWantedShown = 3 * this.m_Grid.columns;
      }

      public override void UpdateAssets()
      {
      }

      protected override void DrawInternal(int itemIdx, int endItem, float yOffset)
      {
        int count = this.m_Assets.Count;
        int num1 = itemIdx;
        yOffset += this.kGroupSeparatorHeight;
        bool flag = Event.current.type == EventType.Repaint;
        Rect position;
        if (this.ListMode)
        {
          for (; itemIdx < endItem && itemIdx < count; ++itemIdx)
          {
            position = this.m_Grid.CalcRect(itemIdx, yOffset);
            int num2 = this.HandleMouse(position);
            if (num2 != 0)
              this.m_Owner.SetSelection(this.m_Assets[itemIdx], num2 == 2);
            if (flag)
            {
              bool selected = !AssetStoreAssetSelection.Empty && AssetStoreAssetSelection.ContainsAsset(this.m_Assets[itemIdx].id);
              this.DrawLabel(position, this.m_Assets[itemIdx], selected);
            }
          }
        }
        else
        {
          for (; itemIdx < endItem && itemIdx < count; ++itemIdx)
          {
            position = this.m_Grid.CalcRect(itemIdx, yOffset);
            int num2 = this.HandleMouse(position);
            if (num2 != 0)
              this.m_Owner.SetSelection(this.m_Assets[itemIdx], num2 == 2);
            if (flag)
              this.DrawIcon(new Rect(position.x, position.y, position.width, position.height - ObjectListArea.s_Styles.resultsGridLabel.fixedHeight), this.m_Assets[itemIdx]);
          }
          itemIdx = num1;
          if (flag)
          {
            for (; itemIdx < endItem && itemIdx < count; ++itemIdx)
            {
              position = this.m_Grid.CalcRect(itemIdx, yOffset);
              bool selected = !AssetStoreAssetSelection.Empty && AssetStoreAssetSelection.ContainsAsset(this.m_Assets[itemIdx].id);
              this.DrawLabel(position, this.m_Assets[itemIdx], selected);
            }
          }
        }
        if (this.ItemsAvailable <= this.m_Grid.rows * this.m_Grid.columns)
          return;
        position = new Rect((float) ((double) this.m_Owner.GetVisibleWidth() - (double) this.m_ShowMoreDims.x - 6.0), (float) ((double) yOffset + (double) this.m_Grid.height + 3.0), this.m_ShowMoreDims.x, this.m_ShowMoreDims.y);
        if (this.ItemsAvailable <= this.m_Grid.rows * this.m_Grid.columns || this.ItemsAvailable < this.Assets.Count || this.Assets.Count >= 1000)
          return;
        Event current = Event.current;
        switch (current.type)
        {
          case EventType.MouseDown:
            if (current.button != 0 || !position.Contains(current.mousePosition))
              break;
            if (this.ListMode)
              this.ItemsWantedShown += 75;
            else
              this.ItemsWantedShown += 10 * this.m_Grid.columns + (this.m_Grid.columns - this.ItemCount % this.m_Grid.columns) % this.m_Grid.columns;
            if (this.NeedItems)
              this.m_Owner.QueryAssetStore();
            current.Use();
            break;
          case EventType.Repaint:
            EditorStyles.miniButton.Draw(position, "More", false, false, false, false);
            break;
        }
      }

      private AssetStorePreviewManager.CachedAssetStoreImage GetIconForAssetStoreAsset(AssetStoreAsset assetStoreResource)
      {
        if (string.IsNullOrEmpty(assetStoreResource.staticPreviewURL))
          return (AssetStorePreviewManager.CachedAssetStoreImage) null;
        ++this.m_Owner.LastScrollTime;
        return AssetStorePreviewManager.TextureFromUrl(assetStoreResource.staticPreviewURL, assetStoreResource.name, this.m_Owner.gridSize, ObjectListArea.s_Styles.resultsGridLabel, ObjectListArea.s_Styles.previewBg, false);
      }

      private void DrawIcon(Rect position, AssetStoreAsset assetStoreResource)
      {
        bool flag = false;
        this.m_Content.text = (string) null;
        AssetStorePreviewManager.CachedAssetStoreImage forAssetStoreAsset = this.GetIconForAssetStoreAsset(assetStoreResource);
        if (forAssetStoreAsset == null)
        {
          Texture2D iconForFile = InternalEditorUtility.GetIconForFile(assetStoreResource.name);
          ObjectListArea.s_Styles.resultsGrid.Draw(position, (Texture) iconForFile, false, false, flag, flag);
        }
        else
        {
          this.m_Content.image = (Texture) forAssetStoreAsset.image;
          Color color1 = forAssetStoreAsset.color;
          Color color2 = GUI.color;
          if ((double) color1.a != 1.0)
            GUI.color = color1;
          ObjectListArea.s_Styles.resultsGrid.Draw(position, this.m_Content, false, false, flag, flag);
          if ((double) color1.a != 1.0)
          {
            GUI.color = color2;
            this.NeedsRepaint = true;
          }
          this.DrawDropShadowOverlay(position, flag, false, false);
        }
      }

      private void DrawLabel(Rect position, AssetStoreAsset assetStoreResource, bool selected)
      {
        if (this.ListMode)
        {
          position.width = Mathf.Max(position.width, 500f);
          this.m_Content.text = assetStoreResource.displayName;
          this.m_Content.image = (Texture) InternalEditorUtility.GetIconForFile(assetStoreResource.name);
          ObjectListArea.s_Styles.resultsLabel.Draw(position, this.m_Content, false, false, selected, selected);
        }
        else
        {
          string croppedLabelText = this.m_Owner.GetCroppedLabelText(assetStoreResource.id + 10000000, assetStoreResource.displayName, position.width);
          position.height -= ObjectListArea.s_Styles.resultsGridLabel.fixedHeight;
          ObjectListArea.s_Styles.resultsGridLabel.Draw(new Rect(position.x, position.yMax + 1f, position.width - 1f, ObjectListArea.s_Styles.resultsGridLabel.fixedHeight), croppedLabelText, false, false, selected, this.m_Owner.HasFocus());
        }
      }

      public override void UpdateFilter(HierarchyType hierarchyType, SearchFilter searchFilter, bool showFoldersFirst)
      {
        this.ItemsWantedShown = !this.ListMode ? 3 * this.m_Grid.columns : 10;
        this.Assets.Clear();
      }

      public override void UpdateHeight()
      {
        this.m_Height = (float) (int) this.kGroupSeparatorHeight;
        if (!this.Visible)
          return;
        this.m_Height += (float) (double) this.m_Grid.height;
        if (this.ItemsAvailable <= this.m_Grid.rows * this.m_Grid.columns)
          return;
        this.m_Height += (float) (double) (6 + (int) this.m_ShowMoreDims.y);
      }

      public int IndexOf(int assetID)
      {
        int num = 0;
        using (List<AssetStoreAsset>.Enumerator enumerator = this.m_Assets.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            if (enumerator.Current.id == assetID)
              return num;
            ++num;
          }
        }
        return -1;
      }

      public AssetStoreAsset AssetAtIndex(int selectedIdx)
      {
        if (selectedIdx >= this.m_Grid.rows * this.m_Grid.columns)
          return (AssetStoreAsset) null;
        if (selectedIdx < this.m_Grid.rows * this.m_Grid.columns && selectedIdx > this.ItemCount)
          return this.m_Assets.Last<AssetStoreAsset>();
        int num = 0;
        using (List<AssetStoreAsset>.Enumerator enumerator = this.m_Assets.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AssetStoreAsset current = enumerator.Current;
            if (selectedIdx == num)
              return current;
            ++num;
          }
        }
        return (AssetStoreAsset) null;
      }

      protected int HandleMouse(Rect position)
      {
        Event current = Event.current;
        switch (current.type)
        {
          case EventType.MouseDown:
            if (current.button == 0 && position.Contains(current.mousePosition))
            {
              this.m_Owner.Repaint();
              if (current.clickCount == 2)
              {
                current.Use();
                return 2;
              }
              this.m_Owner.ScrollToPosition(ObjectListArea.AdjustRectForFraming(position));
              current.Use();
              return 1;
            }
            break;
          case EventType.ContextClick:
            if (position.Contains(current.mousePosition))
              return 1;
            break;
        }
        return 0;
      }
    }

    private abstract class Group
    {
      protected readonly float kGroupSeparatorHeight = EditorStyles.toolbar.fixedHeight;
      public VerticalGrid m_Grid = new VerticalGrid();
      public bool Visible = true;
      protected bool m_Collapsable = true;
      protected string m_GroupSeparatorTitle;
      protected static int[] s_Empty;
      public ObjectListArea m_Owner;
      public float m_Height;
      public int ItemsAvailable;
      public int ItemsWantedShown;
      public double m_LastClickedDrawTime;

      public float Height
      {
        get
        {
          return this.m_Height;
        }
      }

      public abstract int ItemCount { get; }

      public abstract bool ListMode { get; set; }

      public abstract bool NeedsRepaint { get; protected set; }

      public bool visiblePreference
      {
        get
        {
          if (string.IsNullOrEmpty(this.m_GroupSeparatorTitle))
            return true;
          return EditorPrefs.GetBool(this.m_GroupSeparatorTitle, true);
        }
        set
        {
          if (string.IsNullOrEmpty(this.m_GroupSeparatorTitle))
            return;
          EditorPrefs.SetBool(this.m_GroupSeparatorTitle, value);
        }
      }

      public Group(ObjectListArea owner, string groupTitle)
      {
        this.m_GroupSeparatorTitle = groupTitle;
        if (ObjectListArea.Group.s_Empty == null)
          ObjectListArea.Group.s_Empty = new int[0];
        this.m_Owner = owner;
        this.Visible = this.visiblePreference;
      }

      public abstract void UpdateAssets();

      public abstract void UpdateHeight();

      protected abstract void DrawInternal(int itemIdx, int endItem, float yOffset);

      public abstract void UpdateFilter(HierarchyType hierarchyType, SearchFilter searchFilter, bool showFoldersFirst);

      protected virtual float GetHeaderHeight()
      {
        return this.kGroupSeparatorHeight;
      }

      protected virtual void HandleUnusedDragEvents(float yOffset)
      {
      }

      private int FirstVisibleRow(float yOffset, Vector2 scrollPos)
      {
        if (!this.Visible)
          return -1;
        float num1 = scrollPos.y - (yOffset + this.GetHeaderHeight());
        int num2 = 0;
        if ((double) num1 > 0.0)
        {
          float num3 = this.m_Grid.itemSize.y + this.m_Grid.verticalSpacing;
          num2 = (int) Mathf.Max(0.0f, Mathf.Floor(num1 / num3));
        }
        return num2;
      }

      private bool IsInView(float yOffset, Vector2 scrollPos, float scrollViewHeight)
      {
        return (double) scrollPos.y + (double) scrollViewHeight >= (double) yOffset && (double) yOffset + (double) this.Height >= (double) scrollPos.y;
      }

      public void Draw(float yOffset, Vector2 scrollPos)
      {
        this.NeedsRepaint = false;
        bool flag = Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout;
        if (!flag)
          this.DrawHeader(yOffset, this.m_Collapsable);
        if (!this.IsInView(yOffset, scrollPos, this.m_Owner.m_VisibleRect.height))
          return;
        int num1 = this.FirstVisibleRow(yOffset, scrollPos) * this.m_Grid.columns;
        int itemCount = this.ItemCount;
        if (num1 >= 0 && num1 < itemCount)
        {
          int itemIdx = num1;
          int val1 = Math.Min(itemCount, this.m_Grid.rows * this.m_Grid.columns);
          int num2 = (int) Math.Ceiling((double) this.m_Owner.m_VisibleRect.height / (double) (this.m_Grid.itemSize.y + this.m_Grid.verticalSpacing));
          int endItem = Math.Min(val1, itemIdx + num2 * this.m_Grid.columns + this.m_Grid.columns);
          this.DrawInternal(itemIdx, endItem, yOffset);
        }
        if (flag)
          this.DrawHeader(yOffset, this.m_Collapsable);
        this.HandleUnusedDragEvents(yOffset);
      }

      protected void DrawObjectIcon(Rect position, Texture icon)
      {
        if ((UnityEngine.Object) icon == (UnityEngine.Object) null)
          return;
        int width = icon.width;
        UnityEngine.FilterMode filterMode = icon.filterMode;
        icon.filterMode = UnityEngine.FilterMode.Point;
        GUI.DrawTexture(new Rect(position.x + (float) (((int) position.width - width) / 2), position.y + (float) (((int) position.height - width) / 2), (float) width, (float) width), icon, ScaleMode.ScaleToFit);
        icon.filterMode = filterMode;
      }

      protected void DrawDropShadowOverlay(Rect position, bool selected, bool isDropTarget, bool isRenaming)
      {
        float num = position.width / 128f;
        Rect position1 = new Rect(position.x - 4f * num, position.y - 2f * num, position.width + 8f * num, (float) ((double) position.height + 12.0 * (double) num - 0.5));
        ObjectListArea.s_Styles.iconDropShadow.Draw(position1, GUIContent.none, false, false, selected || isDropTarget, this.m_Owner.HasFocus() || isRenaming || isDropTarget);
      }

      protected void DrawHeaderBackground(Rect rect, bool firstHeader)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        GUI.Label(rect, GUIContent.none, !firstHeader ? ObjectListArea.s_Styles.groupHeaderMiddle : ObjectListArea.s_Styles.groupHeaderTop);
      }

      protected float GetHeaderYPosInScrollArea(float yOffset)
      {
        float num = yOffset;
        float y = this.m_Owner.m_State.m_ScrollPosition.y;
        if ((double) y > (double) yOffset)
          num = Mathf.Min(y, yOffset + this.Height - this.kGroupSeparatorHeight);
        return num;
      }

      protected virtual void DrawHeader(float yOffset, bool collapsable)
      {
        Rect rect = new Rect(0.0f, this.GetHeaderYPosInScrollArea(yOffset), this.m_Owner.GetVisibleWidth(), this.kGroupSeparatorHeight - 1f);
        this.DrawHeaderBackground(rect, (double) yOffset == 0.0);
        rect.x += 7f;
        if (collapsable)
        {
          bool visible = this.Visible;
          this.Visible = GUI.Toggle(rect, this.Visible, GUIContent.none, ObjectListArea.s_Styles.groupFoldout);
          if (visible ^ this.Visible)
            this.visiblePreference = this.Visible;
        }
        GUIStyle groupHeaderLabel = ObjectListArea.s_Styles.groupHeaderLabel;
        if (collapsable)
          rect.x += 14f;
        ++rect.y;
        if (!string.IsNullOrEmpty(this.m_GroupSeparatorTitle))
          GUI.Label(rect, this.m_GroupSeparatorTitle, groupHeaderLabel);
        if (ObjectListArea.s_Debug)
        {
          Rect position = rect;
          position.x += 120f;
          GUI.Label(position, AssetStorePreviewManager.StatsString());
        }
        --rect.y;
        if ((double) this.m_Owner.GetVisibleWidth() <= 150.0)
          return;
        this.DrawItemCount(rect);
      }

      protected void DrawItemCount(Rect rect)
      {
        string text = this.ItemsAvailable.ToString() + " Total";
        Vector2 vector2 = ObjectListArea.s_Styles.groupHeaderLabelCount.CalcSize(new GUIContent(text));
        if ((double) vector2.x < (double) rect.width)
          rect.x = (float) ((double) this.m_Owner.GetVisibleWidth() - (double) vector2.x - 4.0);
        rect.width = vector2.x;
        rect.y += 2f;
        GUI.Label(rect, text, ObjectListArea.s_Styles.groupHeaderLabelCount);
      }

      private UnityEngine.Object[] GetSelectedReferences()
      {
        return Selection.objects;
      }

      private static string[] GetMainSelectedPaths()
      {
        List<string> stringList = new List<string>();
        foreach (int instanceId in Selection.instanceIDs)
        {
          if (AssetDatabase.IsMainAsset(instanceId))
          {
            string assetPath = AssetDatabase.GetAssetPath(instanceId);
            stringList.Add(assetPath);
          }
        }
        return stringList.ToArray();
      }
    }

    private class LocalGroup : ObjectListArea.Group
    {
      private GUIContent m_Content = new GUIContent();
      private List<int> m_DragSelection = new List<int>();
      private List<int> m_LastRenderedAssetInstanceIDs = new List<int>();
      private List<int> m_LastRenderedAssetDirtyIDs = new List<int>();
      private ObjectListArea.LocalGroup.ItemFader m_ItemFader = new ObjectListArea.LocalGroup.ItemFader();
      public const int k_ListModeLeftPadding = 16;
      public const int k_ListModeLeftPaddingForSubAssets = 28;
      public const int k_ListModeVersionControlOverlayPadding = 14;
      private BuiltinResource[] m_NoneList;
      private int m_DropTargetControlID;
      private Dictionary<string, BuiltinResource[]> m_BuiltinResourceMap;
      private BuiltinResource[] m_CurrentBuiltinResources;
      private bool m_ShowNoneItem;
      public bool m_ListMode;
      private FilteredHierarchy m_FilteredHierarchy;
      private BuiltinResource[] m_ActiveBuiltinList;

      public bool ShowNone
      {
        get
        {
          return this.m_ShowNoneItem;
        }
      }

      public override bool NeedsRepaint
      {
        get
        {
          return false;
        }
        protected set
        {
        }
      }

      public SearchFilter searchFilter
      {
        get
        {
          return this.m_FilteredHierarchy.searchFilter;
        }
      }

      public override bool ListMode
      {
        get
        {
          return this.m_ListMode;
        }
        set
        {
          this.m_ListMode = value;
        }
      }

      public bool HasBuiltinResources
      {
        get
        {
          return this.m_CurrentBuiltinResources.Length > 0;
        }
      }

      public override int ItemCount
      {
        get
        {
          return this.m_FilteredHierarchy.results.Length + this.m_ActiveBuiltinList.Length + (!this.m_ShowNoneItem ? 0 : 1) + (this.m_Owner.m_State.m_NewAssetIndexInList == -1 ? 0 : 1);
        }
      }

      public LocalGroup(ObjectListArea owner, string groupTitle, bool showNone)
        : base(owner, groupTitle)
      {
        this.m_ShowNoneItem = showNone;
        this.m_ListMode = false;
        this.InitBuiltinResources();
        this.ItemsWantedShown = int.MaxValue;
        this.m_Collapsable = false;
      }

      public override void UpdateAssets()
      {
        this.m_ActiveBuiltinList = this.m_FilteredHierarchy.hierarchyType != HierarchyType.Assets ? new BuiltinResource[0] : this.m_CurrentBuiltinResources;
        this.ItemsAvailable = this.m_FilteredHierarchy.results.Length + this.m_ActiveBuiltinList.Length;
      }

      protected override float GetHeaderHeight()
      {
        return 0.0f;
      }

      protected override void DrawHeader(float yOffset, bool collapsable)
      {
        if ((double) this.GetHeaderHeight() <= 0.0)
          return;
        Rect rect = new Rect(0.0f, this.GetHeaderYPosInScrollArea(yOffset), this.m_Owner.GetVisibleWidth(), this.kGroupSeparatorHeight);
        this.DrawHeaderBackground(rect, true);
        if (collapsable)
        {
          rect.x += 7f;
          bool visible = this.Visible;
          this.Visible = GUI.Toggle(new Rect(rect.x, rect.y, 14f, rect.height), this.Visible, GUIContent.none, ObjectListArea.s_Styles.groupFoldout);
          if (visible ^ this.Visible)
            EditorPrefs.SetBool(this.m_GroupSeparatorTitle, this.Visible);
          rect.x += 7f;
        }
        float num = 0.0f;
        if (this.m_Owner.drawLocalAssetHeader != null)
          num = this.m_Owner.drawLocalAssetHeader(rect) + 10f;
        rect.x += num;
        rect.width -= num;
        if ((double) rect.width <= 0.0)
          return;
        this.DrawItemCount(rect);
      }

      public override void UpdateHeight()
      {
        this.m_Height = this.GetHeaderHeight();
        if (!this.Visible)
          return;
        this.m_Height += (float) (double) this.m_Grid.height;
      }

      private bool IsCreatingAtThisIndex(int itemIdx)
      {
        return this.m_Owner.m_State.m_NewAssetIndexInList == itemIdx;
      }

      protected override void DrawInternal(int beginIndex, int endIndex, float yOffset)
      {
        int itemIdx = beginIndex;
        int num1 = 0;
        FilteredHierarchy.FilterResult[] results = this.m_FilteredHierarchy.results;
        bool isFolderBrowsing = this.m_FilteredHierarchy.searchFilter.GetState() == SearchFilter.State.FolderBrowsing;
        yOffset += this.GetHeaderHeight();
        if (this.m_NoneList.Length > 0)
        {
          if (beginIndex < 1)
          {
            this.DrawItem(this.m_Grid.CalcRect(itemIdx, yOffset), (FilteredHierarchy.FilterResult) null, this.m_NoneList[0], isFolderBrowsing);
            ++itemIdx;
          }
          ++num1;
        }
        if (!this.ListMode && isFolderBrowsing)
          this.DrawSubAssetBackground(beginIndex, endIndex, yOffset);
        if (Event.current.type == EventType.Repaint)
          this.ClearDirtyStateTracking();
        int index1 = itemIdx - num1;
        while (true)
        {
          if (this.IsCreatingAtThisIndex(itemIdx))
          {
            this.DrawItem(this.m_Grid.CalcRect(itemIdx, yOffset), (FilteredHierarchy.FilterResult) null, new BuiltinResource()
            {
              m_Name = this.m_Owner.GetCreateAssetUtility().originalName,
              m_InstanceID = this.m_Owner.GetCreateAssetUtility().instanceID
            }, isFolderBrowsing);
            ++itemIdx;
            ++num1;
          }
          if (itemIdx <= endIndex && index1 < results.Length)
          {
            FilteredHierarchy.FilterResult filterItem = results[index1];
            this.DrawItem(this.m_Grid.CalcRect(itemIdx, yOffset), filterItem, (BuiltinResource) null, isFolderBrowsing);
            ++itemIdx;
            ++index1;
          }
          else
            break;
        }
        int num2 = num1 + results.Length;
        if (this.m_ActiveBuiltinList.Length > 0)
        {
          for (int index2 = Math.Max(beginIndex - num2, 0); index2 < this.m_ActiveBuiltinList.Length && itemIdx <= endIndex; ++index2)
          {
            this.DrawItem(this.m_Grid.CalcRect(itemIdx, yOffset), (FilteredHierarchy.FilterResult) null, this.m_ActiveBuiltinList[index2], isFolderBrowsing);
            ++itemIdx;
          }
        }
        if (this.ListMode || !AssetPreview.IsLoadingAssetPreviews(this.m_Owner.GetAssetPreviewManagerID()))
          return;
        this.m_Owner.Repaint();
      }

      private void ClearDirtyStateTracking()
      {
        this.m_LastRenderedAssetInstanceIDs.Clear();
        this.m_LastRenderedAssetDirtyIDs.Clear();
      }

      private void AddDirtyStateFor(int instanceID)
      {
        this.m_LastRenderedAssetInstanceIDs.Add(instanceID);
        this.m_LastRenderedAssetDirtyIDs.Add(EditorUtility.GetDirtyIndex(instanceID));
      }

      public bool IsAnyLastRenderedAssetsDirty()
      {
        for (int index = 0; index < this.m_LastRenderedAssetInstanceIDs.Count; ++index)
        {
          int dirtyIndex = EditorUtility.GetDirtyIndex(this.m_LastRenderedAssetInstanceIDs[index]);
          if (dirtyIndex != this.m_LastRenderedAssetDirtyIDs[index])
          {
            this.m_LastRenderedAssetDirtyIDs[index] = dirtyIndex;
            return true;
          }
        }
        return false;
      }

      protected override void HandleUnusedDragEvents(float yOffset)
      {
        if (!this.m_Owner.allowDragging)
          return;
        Event current = Event.current;
        switch (current.type)
        {
          case EventType.DragUpdated:
          case EventType.DragPerform:
            if (!new Rect(0.0f, yOffset, this.m_Owner.m_TotalRect.width, (double) this.m_Owner.m_TotalRect.height <= (double) this.Height ? this.Height : this.m_Owner.m_TotalRect.height).Contains(current.mousePosition))
              break;
            DragAndDropVisualMode andDropVisualMode;
            if (this.m_FilteredHierarchy.searchFilter.GetState() == SearchFilter.State.FolderBrowsing && this.m_FilteredHierarchy.searchFilter.folders.Length == 1)
            {
              int mainAssetInstanceId = AssetDatabase.GetMainAssetInstanceID(this.m_FilteredHierarchy.searchFilter.folders[0]);
              bool perform = current.type == EventType.DragPerform;
              andDropVisualMode = this.DoDrag(mainAssetInstanceId, perform);
              if (perform && andDropVisualMode != DragAndDropVisualMode.None)
                DragAndDrop.AcceptDrag();
            }
            else
              andDropVisualMode = DragAndDropVisualMode.None;
            DragAndDrop.visualMode = andDropVisualMode;
            current.Use();
            break;
        }
      }

      private void HandleMouseWithDragging(int instanceID, int controlID, Rect rect)
      {
        Event current = Event.current;
        EventType typeForControl = current.GetTypeForControl(controlID);
        switch (typeForControl)
        {
          case EventType.DragUpdated:
          case EventType.DragPerform:
            bool perform = current.type == EventType.DragPerform;
            if (rect.Contains(current.mousePosition))
            {
              DragAndDropVisualMode andDropVisualMode = this.DoDrag(instanceID, perform);
              if (andDropVisualMode != DragAndDropVisualMode.None)
              {
                if (perform)
                  DragAndDrop.AcceptDrag();
                this.m_DropTargetControlID = controlID;
                DragAndDrop.visualMode = andDropVisualMode;
                current.Use();
              }
              if (perform)
                this.m_DropTargetControlID = 0;
            }
            if (!perform)
              break;
            this.m_DragSelection.Clear();
            break;
          case EventType.DragExited:
            this.m_DragSelection.Clear();
            break;
          case EventType.ContextClick:
            Rect drawRect = rect;
            drawRect.x += 2f;
            Rect overlayRect = ProjectHooks.GetOverlayRect(drawRect);
            if ((double) overlayRect.width == (double) rect.width || !Provider.isActive || !overlayRect.Contains(current.mousePosition))
              break;
            EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), "Assets/Version Control", new MenuCommand((UnityEngine.Object) null, 0));
            current.Use();
            break;
          default:
            switch (typeForControl)
            {
              case EventType.MouseDown:
                if (Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
                {
                  if (current.clickCount == 2)
                  {
                    this.m_Owner.SetSelection(new int[1]{ instanceID }, 1 != 0);
                    this.m_DragSelection.Clear();
                  }
                  else
                  {
                    this.m_DragSelection = this.GetNewSelection(instanceID, true, false);
                    GUIUtility.hotControl = controlID;
                    ((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), controlID)).mouseDownPosition = Event.current.mousePosition;
                    this.m_Owner.ScrollToPosition(ObjectListArea.AdjustRectForFraming(rect));
                  }
                  current.Use();
                  return;
                }
                if (Event.current.button != 1 || !rect.Contains(Event.current.mousePosition))
                  return;
                this.m_Owner.SetSelection(this.GetNewSelection(instanceID, true, false).ToArray(), false);
                return;
              case EventType.MouseUp:
                if (GUIUtility.hotControl != controlID)
                  return;
                if (rect.Contains(current.mousePosition))
                {
                  bool flag;
                  if (this.ListMode)
                  {
                    rect.x += 28f;
                    rect.width += 28f;
                    flag = rect.Contains(current.mousePosition);
                  }
                  else
                  {
                    rect.y = rect.y + rect.height - ObjectListArea.s_Styles.resultsGridLabel.fixedHeight;
                    rect.height = ObjectListArea.s_Styles.resultsGridLabel.fixedHeight;
                    flag = rect.Contains(current.mousePosition);
                  }
                  List<int> selectedInstanceIds = this.m_Owner.m_State.m_SelectedInstanceIDs;
                  if (flag && this.m_Owner.allowRenaming && (this.m_Owner.m_AllowRenameOnMouseUp && selectedInstanceIds.Count == 1) && (selectedInstanceIds[0] == instanceID && !EditorGUIUtility.HasHolddownKeyModifiers(current)))
                    this.m_Owner.BeginRename(0.5f);
                  else
                    this.m_Owner.SetSelection(this.GetNewSelection(instanceID, false, false).ToArray(), false);
                  GUIUtility.hotControl = 0;
                  current.Use();
                }
                this.m_DragSelection.Clear();
                return;
              case EventType.MouseMove:
                return;
              case EventType.MouseDrag:
                if (GUIUtility.hotControl != controlID)
                  return;
                if (((DragAndDropDelay) GUIUtility.GetStateObject(typeof (DragAndDropDelay), controlID)).CanStartDrag())
                {
                  this.StartDrag(instanceID, this.m_DragSelection);
                  GUIUtility.hotControl = 0;
                }
                current.Use();
                return;
              default:
                return;
            }
        }
      }

      private void HandleMouseWithoutDragging(int instanceID, int controlID, Rect position)
      {
        Event current = Event.current;
        switch (current.GetTypeForControl(controlID))
        {
          case EventType.MouseDown:
            if (current.button != 0 || !position.Contains(current.mousePosition))
              break;
            this.m_Owner.Repaint();
            if (current.clickCount == 1)
              this.m_Owner.ScrollToPosition(ObjectListArea.AdjustRectForFraming(position));
            current.Use();
            this.m_Owner.SetSelection(this.GetNewSelection(instanceID, false, false).ToArray(), current.clickCount == 2);
            break;
          case EventType.ContextClick:
            if (!position.Contains(current.mousePosition))
              break;
            this.m_Owner.SetSelection(new int[1]{ instanceID }, 0 != 0);
            Rect drawRect = position;
            drawRect.x += 2f;
            Rect overlayRect = ProjectHooks.GetOverlayRect(drawRect);
            if ((double) overlayRect.width == (double) position.width || !Provider.isActive || !overlayRect.Contains(current.mousePosition))
              break;
            EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f), "Assets/Version Control", new MenuCommand((UnityEngine.Object) null, 0));
            current.Use();
            break;
        }
      }

      public void ChangeExpandedState(int instanceID, bool expanded)
      {
        this.m_Owner.m_State.m_ExpandedInstanceIDs.Remove(instanceID);
        if (expanded)
          this.m_Owner.m_State.m_ExpandedInstanceIDs.Add(instanceID);
        this.m_FilteredHierarchy.RefreshVisibleItems(this.m_Owner.m_State.m_ExpandedInstanceIDs);
      }

      private bool IsExpanded(int instanceID)
      {
        return this.m_Owner.m_State.m_ExpandedInstanceIDs.IndexOf(instanceID) >= 0;
      }

      private void SelectAndFrameParentOf(int instanceID)
      {
        int instanceID1 = 0;
        FilteredHierarchy.FilterResult[] results = this.m_FilteredHierarchy.results;
        for (int index = 0; index < results.Length; ++index)
        {
          if (results[index].instanceID == instanceID)
          {
            if (results[index].isMainRepresentation)
            {
              instanceID1 = 0;
              break;
            }
            break;
          }
          if (results[index].isMainRepresentation)
            instanceID1 = results[index].instanceID;
        }
        if (instanceID1 == 0)
          return;
        this.m_Owner.SetSelection(new int[1]{ instanceID1 }, 0 != 0);
        this.m_Owner.Frame(instanceID1, true, false);
      }

      private bool IsRenaming(int instanceID)
      {
        RenameOverlay renameOverlay = this.m_Owner.GetRenameOverlay();
        if (renameOverlay.IsRenaming() && renameOverlay.userData == instanceID)
          return !renameOverlay.isWaitingForDelay;
        return false;
      }

      protected void DrawSubAssetRowBg(int startSubAssetIndex, int endSubAssetIndex, bool continued, float yOffset)
      {
        Rect rect1 = this.m_Grid.CalcRect(startSubAssetIndex, yOffset);
        Rect rect2 = this.m_Grid.CalcRect(endSubAssetIndex, yOffset);
        float num1 = 30f;
        float num2 = 128f;
        float num3 = rect1.width / num2;
        float num4 = 9f * num3;
        float num5 = 4f;
        float num6 = startSubAssetIndex % this.m_Grid.columns != 0 ? this.m_Grid.horizontalSpacing + num3 * 10f : 18f * num3;
        Rect position1 = new Rect(rect1.x - num6, rect1.y + num5, num1 * num3, (float) ((double) rect1.width - (double) num5 * 2.0 + (double) num4 - 1.0));
        position1.y = Mathf.Round(position1.y);
        position1.height = Mathf.Ceil(position1.height);
        ObjectListArea.s_Styles.subAssetBg.Draw(position1, GUIContent.none, false, false, false, false);
        float width = num1 * num3;
        bool flag = endSubAssetIndex % this.m_Grid.columns == this.m_Grid.columns - 1;
        float num7 = continued || flag ? 16f * num3 : 8f * num3;
        Rect position2 = new Rect(rect2.xMax - width + num7, rect2.y + num5, width, position1.height);
        position2.y = Mathf.Round(position2.y);
        position2.height = Mathf.Ceil(position2.height);
        (!continued ? ObjectListArea.s_Styles.subAssetBgCloseEnded : ObjectListArea.s_Styles.subAssetBgOpenEnded).Draw(position2, GUIContent.none, false, false, false, false);
        position1 = new Rect(position1.xMax, position1.y, position2.xMin - position1.xMax, position1.height);
        position1.y = Mathf.Round(position1.y);
        position1.height = Mathf.Ceil(position1.height);
        ObjectListArea.s_Styles.subAssetBgMiddle.Draw(position1, GUIContent.none, false, false, false, false);
      }

      private void DrawSubAssetBackground(int beginIndex, int endIndex, float yOffset)
      {
        if (Event.current.type != EventType.Repaint)
          return;
        FilteredHierarchy.FilterResult[] results = this.m_FilteredHierarchy.results;
        int columns = this.m_Grid.columns;
        int num = (endIndex - beginIndex) / columns + 1;
        for (int index1 = 0; index1 < num; ++index1)
        {
          int startSubAssetIndex = -1;
          int endSubAssetIndex = -1;
          for (int index2 = 0; index2 < columns; ++index2)
          {
            int index3 = beginIndex + (index2 + index1 * columns);
            if (index3 < results.Length)
            {
              if (!results[index3].isMainRepresentation)
              {
                if (startSubAssetIndex == -1)
                  startSubAssetIndex = index3;
                endSubAssetIndex = index3;
              }
              else if (startSubAssetIndex != -1)
              {
                this.DrawSubAssetRowBg(startSubAssetIndex, endSubAssetIndex, false, yOffset);
                startSubAssetIndex = -1;
                endSubAssetIndex = -1;
              }
            }
            else
              break;
          }
          if (startSubAssetIndex != -1)
          {
            bool continued = false;
            if (index1 < num - 1)
            {
              int index2 = beginIndex + (index1 + 1) * columns;
              if (index2 < results.Length)
                continued = !results[index2].isMainRepresentation;
            }
            this.DrawSubAssetRowBg(startSubAssetIndex, endSubAssetIndex, continued, yOffset);
          }
        }
      }

      private void DrawItem(Rect position, FilteredHierarchy.FilterResult filterItem, BuiltinResource builtinResource, bool isFolderBrowsing)
      {
        Event current = Event.current;
        Rect selectionRect = position;
        int num1 = 0;
        bool flag1 = false;
        if (filterItem != null)
        {
          num1 = filterItem.instanceID;
          flag1 = filterItem.hasChildren && !filterItem.isFolder && isFolderBrowsing;
        }
        else if (builtinResource != null)
          num1 = builtinResource.m_InstanceID;
        int idFromInstanceId = ObjectListArea.LocalGroup.GetControlIDFromInstanceID(num1);
        bool flag2 = !this.m_Owner.allowDragging ? this.m_Owner.IsSelected(num1) : (this.m_DragSelection.Count <= 0 ? this.m_Owner.IsSelected(num1) : this.m_DragSelection.Contains(num1));
        if (flag2 && num1 == this.m_Owner.m_State.m_LastClickedInstanceID)
          this.m_LastClickedDrawTime = EditorApplication.timeSinceStartup;
        Rect position1 = new Rect(position.x + 2f, position.y, 16f, 16f);
        if (flag1 && !this.ListMode)
        {
          float num2 = position.height / 128f;
          float width = 28f * num2;
          float height = 32f * num2;
          position1 = new Rect(position.xMax - width * 0.5f, (float) ((double) position.y + ((double) position.height - (double) ObjectListArea.s_Styles.resultsGridLabel.fixedHeight) * 0.5 - (double) width * 0.5), width, height);
        }
        bool flag3 = false;
        if (flag2 && current.type == EventType.KeyDown && this.m_Owner.HasFocus())
        {
          switch (current.keyCode)
          {
            case KeyCode.RightArrow:
              if (this.ListMode || this.m_Owner.IsPreviewIconExpansionModifierPressed())
              {
                if (!this.IsExpanded(num1))
                  flag3 = true;
                current.Use();
                break;
              }
              break;
            case KeyCode.LeftArrow:
              if (this.ListMode || this.m_Owner.IsPreviewIconExpansionModifierPressed())
              {
                if (this.IsExpanded(num1))
                  flag3 = true;
                else
                  this.SelectAndFrameParentOf(num1);
                current.Use();
                break;
              }
              break;
          }
        }
        if (flag1 && current.type == EventType.MouseDown && (current.button == 0 && position1.Contains(current.mousePosition)))
          flag3 = true;
        if (flag3)
        {
          bool expanded = !this.IsExpanded(num1);
          if (expanded)
            this.m_ItemFader.Start(this.m_FilteredHierarchy.GetSubAssetInstanceIDs(num1));
          this.ChangeExpandedState(num1, expanded);
          current.Use();
          GUIUtility.ExitGUI();
        }
        bool flag4 = this.IsRenaming(num1);
        Rect position2 = position;
        if (!this.ListMode)
          position2 = new Rect(position.x, position.yMax + 1f - ObjectListArea.s_Styles.resultsGridLabel.fixedHeight, position.width - 1f, ObjectListArea.s_Styles.resultsGridLabel.fixedHeight);
        int num3 = !Provider.isActive || !this.ListMode ? 0 : 14;
        float x = 16f;
        if (this.ListMode)
        {
          selectionRect.x = 16f;
          if (filterItem != null && !filterItem.isMainRepresentation && isFolderBrowsing)
          {
            x = 28f;
            selectionRect.x = (float) (28.0 + (double) num3 * 0.5);
          }
          selectionRect.width -= selectionRect.x;
        }
        if (Event.current.type == EventType.Repaint)
        {
          if (this.m_DropTargetControlID == idFromInstanceId && !position.Contains(current.mousePosition))
            this.m_DropTargetControlID = 0;
          bool flag5 = idFromInstanceId == this.m_DropTargetControlID && this.m_DragSelection.IndexOf(this.m_DropTargetControlID) == -1;
          string str = filterItem == null ? builtinResource.m_Name : filterItem.name;
          if (this.ListMode)
          {
            if (flag4)
            {
              flag2 = false;
              str = string.Empty;
            }
            position.width = Mathf.Max(position.width, 500f);
            this.m_Content.text = str;
            this.m_Content.image = (Texture) null;
            Texture2D icon = filterItem == null ? AssetPreview.GetAssetPreview(num1, this.m_Owner.GetAssetPreviewManagerID()) : filterItem.icon;
            if ((UnityEngine.Object) icon == (UnityEngine.Object) null && (UnityEngine.Object) this.m_Owner.GetCreateAssetUtility().icon != (UnityEngine.Object) null)
              icon = this.m_Owner.GetCreateAssetUtility().icon;
            if (flag2)
              ObjectListArea.s_Styles.resultsLabel.Draw(position, GUIContent.none, false, false, flag2, this.m_Owner.HasFocus());
            if (flag5)
              ObjectListArea.s_Styles.resultsLabel.Draw(position, GUIContent.none, true, true, false, false);
            ObjectListArea.LocalGroup.DrawIconAndLabel(new Rect(x, position.y, position.width - x, position.height), filterItem, str, icon, flag2, this.m_Owner.HasFocus());
            if (flag1)
              ObjectListArea.s_Styles.groupFoldout.Draw(position1, !this.ListMode, !this.ListMode, this.IsExpanded(num1), false);
          }
          else
          {
            bool flag6 = false;
            if (this.m_Owner.GetCreateAssetUtility().instanceID == num1 && (UnityEngine.Object) this.m_Owner.GetCreateAssetUtility().icon != (UnityEngine.Object) null)
            {
              this.m_Content.image = (Texture) this.m_Owner.GetCreateAssetUtility().icon;
            }
            else
            {
              this.m_Content.image = (Texture) AssetPreview.GetAssetPreview(num1, this.m_Owner.GetAssetPreviewManagerID());
              if ((UnityEngine.Object) this.m_Content.image != (UnityEngine.Object) null)
                flag6 = true;
              if (filterItem != null)
              {
                if ((UnityEngine.Object) this.m_Content.image == (UnityEngine.Object) null)
                  this.m_Content.image = (Texture) filterItem.icon;
                if (isFolderBrowsing && !filterItem.isMainRepresentation)
                  flag6 = false;
              }
            }
            float num2 = !flag6 ? 0.0f : 2f;
            position.height -= ObjectListArea.s_Styles.resultsGridLabel.fixedHeight + 2f * num2;
            position.y += num2;
            Rect rect = !((UnityEngine.Object) this.m_Content.image == (UnityEngine.Object) null) ? ObjectListArea.LocalGroup.ActualImageDrawPosition(position, (float) this.m_Content.image.width, (float) this.m_Content.image.height) : new Rect();
            this.m_Content.text = (string) null;
            float a = 1f;
            if (filterItem != null)
            {
              this.AddDirtyStateFor(filterItem.instanceID);
              if (!filterItem.isMainRepresentation && isFolderBrowsing)
              {
                position.x += 4f;
                position.y += 4f;
                position.width -= 8f;
                position.height -= 8f;
                rect = !((UnityEngine.Object) this.m_Content.image == (UnityEngine.Object) null) ? ObjectListArea.LocalGroup.ActualImageDrawPosition(position, (float) this.m_Content.image.width, (float) this.m_Content.image.height) : new Rect();
                a = this.m_ItemFader.GetAlpha(filterItem.instanceID);
                if ((double) a < 1.0)
                  this.m_Owner.Repaint();
              }
              if (flag6 && filterItem.iconDrawStyle == IconDrawStyle.NonTexture)
                ObjectListArea.s_Styles.previewBg.Draw(rect, GUIContent.none, false, false, false, false);
            }
            Color color1 = GUI.color;
            if (flag2)
              GUI.color *= new Color(0.85f, 0.9f, 1f);
            if ((UnityEngine.Object) this.m_Content.image != (UnityEngine.Object) null)
            {
              Color color2 = GUI.color;
              if ((double) a < 1.0)
                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, a);
              ObjectListArea.s_Styles.resultsGrid.Draw(rect, this.m_Content, false, false, flag2, this.m_Owner.HasFocus());
              if ((double) a < 1.0)
                GUI.color = color2;
            }
            if (flag2)
              GUI.color = color1;
            if (flag6)
            {
              Rect position3 = new RectOffset(1, 1, 1, 1).Remove(ObjectListArea.s_Styles.textureIconDropShadow.border.Add(rect));
              ObjectListArea.s_Styles.textureIconDropShadow.Draw(position3, GUIContent.none, false, false, flag2 || flag5, this.m_Owner.HasFocus() || flag4 || flag5);
            }
            if (!flag4)
            {
              if (flag5)
                ObjectListArea.s_Styles.resultsLabel.Draw(new Rect(position2.x - 10f, position2.y, position2.width + 20f, position2.height), GUIContent.none, true, true, false, false);
              string croppedLabelText = this.m_Owner.GetCroppedLabelText(num1, str, position.width);
              ObjectListArea.s_Styles.resultsGridLabel.Draw(position2, croppedLabelText, false, false, flag2, this.m_Owner.HasFocus());
            }
            if (flag1)
              ObjectListArea.s_Styles.subAssetExpandButton.Draw(position1, !this.ListMode, !this.ListMode, this.IsExpanded(num1), false);
            if (filterItem != null && filterItem.isMainRepresentation)
              ProjectHooks.OnProjectWindowItem(filterItem.guid, position);
          }
        }
        if (flag4)
        {
          if (this.ListMode)
          {
            float num2 = (float) (num3 + 16);
            position2.x = selectionRect.x + num2;
            position2.width -= position2.x;
          }
          else
          {
            position2.x -= 4f;
            position2.width += 8f;
          }
          this.m_Owner.GetRenameOverlay().editFieldRect = position2;
          this.m_Owner.HandleRenameOverlay();
        }
        if (EditorApplication.projectWindowItemOnGUI != null && filterItem != null && this.m_Owner.allowUserRenderingHook)
          EditorApplication.projectWindowItemOnGUI(filterItem.guid, selectionRect);
        if (this.m_Owner.allowDragging)
          this.HandleMouseWithDragging(num1, idFromInstanceId, position);
        else
          this.HandleMouseWithoutDragging(num1, idFromInstanceId, position);
      }

      private static Rect ActualImageDrawPosition(Rect position, float imageWidth, float imageHeight)
      {
        if ((double) imageWidth <= (double) position.width && (double) imageHeight <= (double) position.height)
          return new Rect(position.x + Mathf.Round((float) (((double) position.width - (double) imageWidth) / 2.0)), position.y + Mathf.Round((float) (((double) position.height - (double) imageHeight) / 2.0)), imageWidth, imageHeight);
        Rect outScreenRect = new Rect();
        Rect outSourceRect = new Rect();
        float imageAspect = imageWidth / imageHeight;
        GUI.CalculateScaledTextureRects(position, ScaleMode.ScaleToFit, imageAspect, ref outScreenRect, ref outSourceRect);
        return outScreenRect;
      }

      public List<KeyValuePair<string, int>> GetVisibleNameAndInstanceIDs()
      {
        List<KeyValuePair<string, int>> keyValuePairList = new List<KeyValuePair<string, int>>();
        if (this.m_NoneList.Length > 0)
          keyValuePairList.Add(new KeyValuePair<string, int>(this.m_NoneList[0].m_Name, this.m_NoneList[0].m_InstanceID));
        foreach (FilteredHierarchy.FilterResult result in this.m_FilteredHierarchy.results)
          keyValuePairList.Add(new KeyValuePair<string, int>(result.name, result.instanceID));
        for (int index = 0; index < this.m_ActiveBuiltinList.Length; ++index)
          keyValuePairList.Add(new KeyValuePair<string, int>(this.m_ActiveBuiltinList[index].m_Name, this.m_ActiveBuiltinList[index].m_InstanceID));
        return keyValuePairList;
      }

      private void BeginPing(int instanceID)
      {
      }

      public List<int> GetInstanceIDs()
      {
        List<int> intList = new List<int>();
        if (this.m_NoneList.Length > 0)
          intList.Add(this.m_NoneList[0].m_InstanceID);
        foreach (FilteredHierarchy.FilterResult result in this.m_FilteredHierarchy.results)
          intList.Add(result.instanceID);
        if (this.m_Owner.m_State.m_NewAssetIndexInList >= 0)
          intList.Add(this.m_Owner.GetCreateAssetUtility().instanceID);
        for (int index = 0; index < this.m_ActiveBuiltinList.Length; ++index)
          intList.Add(this.m_ActiveBuiltinList[index].m_InstanceID);
        return intList;
      }

      public List<int> GetNewSelection(int clickedInstanceID, bool beginOfDrag, bool useShiftAsActionKey)
      {
        List<int> instanceIds = this.GetInstanceIDs();
        List<int> selectedInstanceIds = this.m_Owner.m_State.m_SelectedInstanceIDs;
        int clickedInstanceId = this.m_Owner.m_State.m_LastClickedInstanceID;
        bool allowMultiSelect = this.m_Owner.allowMultiSelect;
        return InternalEditorUtility.GetNewSelection(clickedInstanceID, instanceIds, selectedInstanceIds, clickedInstanceId, beginOfDrag, useShiftAsActionKey, allowMultiSelect);
      }

      public override void UpdateFilter(HierarchyType hierarchyType, SearchFilter searchFilter, bool foldersFirst)
      {
        this.RefreshHierarchy(hierarchyType, searchFilter, foldersFirst);
        this.RefreshBuiltinResourceList(searchFilter);
      }

      private void RefreshHierarchy(HierarchyType hierarchyType, SearchFilter searchFilter, bool foldersFirst)
      {
        this.m_FilteredHierarchy = new FilteredHierarchy(hierarchyType);
        this.m_FilteredHierarchy.foldersFirst = foldersFirst;
        this.m_FilteredHierarchy.searchFilter = searchFilter;
        this.m_FilteredHierarchy.RefreshVisibleItems(this.m_Owner.m_State.m_ExpandedInstanceIDs);
      }

      private void RefreshBuiltinResourceList(SearchFilter searchFilter)
      {
        if (!this.m_Owner.allowBuiltinResources || searchFilter.GetState() == SearchFilter.State.FolderBrowsing || searchFilter.GetState() == SearchFilter.State.EmptySearchFilter)
        {
          this.m_CurrentBuiltinResources = new BuiltinResource[0];
        }
        else
        {
          List<BuiltinResource> builtinResourceList1 = new List<BuiltinResource>();
          if (searchFilter.assetLabels != null && searchFilter.assetLabels.Length > 0)
          {
            this.m_CurrentBuiltinResources = builtinResourceList1.ToArray();
          }
          else
          {
            List<int> intList = new List<int>();
            foreach (string className in searchFilter.classNames)
            {
              int idCaseInsensitive = BaseObjectTools.StringToClassIDCaseInsensitive(className);
              if (idCaseInsensitive >= 0)
                intList.Add(idCaseInsensitive);
            }
            if (intList.Count > 0)
            {
              using (Dictionary<string, BuiltinResource[]>.Enumerator enumerator1 = this.m_BuiltinResourceMap.GetEnumerator())
              {
                while (enumerator1.MoveNext())
                {
                  KeyValuePair<string, BuiltinResource[]> current1 = enumerator1.Current;
                  int classId = BaseObjectTools.StringToClassID(current1.Key);
                  using (List<int>.Enumerator enumerator2 = intList.GetEnumerator())
                  {
                    while (enumerator2.MoveNext())
                    {
                      int current2 = enumerator2.Current;
                      if (BaseObjectTools.IsDerivedFromClassID(classId, current2))
                        builtinResourceList1.AddRange((IEnumerable<BuiltinResource>) current1.Value);
                    }
                  }
                }
              }
            }
            BuiltinResource[] array = builtinResourceList1.ToArray();
            if (array.Length > 0 && !string.IsNullOrEmpty(searchFilter.nameFilter))
            {
              List<BuiltinResource> builtinResourceList2 = new List<BuiltinResource>();
              string lower = searchFilter.nameFilter.ToLower();
              foreach (BuiltinResource builtinResource in array)
              {
                if (builtinResource.m_Name.ToLower().Contains(lower))
                  builtinResourceList2.Add(builtinResource);
              }
              array = builtinResourceList2.ToArray();
            }
            this.m_CurrentBuiltinResources = array;
          }
        }
      }

      public string GetNameOfLocalAsset(int instanceID)
      {
        foreach (FilteredHierarchy.FilterResult result in this.m_FilteredHierarchy.results)
        {
          if (result.instanceID == instanceID)
            return result.name;
        }
        return (string) null;
      }

      public bool IsBuiltinAsset(int instanceID)
      {
        using (Dictionary<string, BuiltinResource[]>.Enumerator enumerator = this.m_BuiltinResourceMap.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            foreach (BuiltinResource builtinResource in enumerator.Current.Value)
            {
              if (builtinResource.m_InstanceID == instanceID)
                return true;
            }
          }
        }
        return false;
      }

      private void InitBuiltinAssetType(System.Type type)
      {
        if (type == null)
        {
          Debug.LogWarning((object) "ObjectSelector::InitBuiltinAssetType: type is null!");
        }
        else
        {
          string str = type.ToString().Substring(type.Namespace.ToString().Length + 1);
          int classId = BaseObjectTools.StringToClassID(str);
          if (classId < 0)
          {
            Debug.LogWarning((object) ("ObjectSelector::InitBuiltinAssetType: class '" + str + "' not found"));
          }
          else
          {
            BuiltinResource[] builtinResourceList = EditorGUIUtility.GetBuiltinResourceList(classId);
            if (builtinResourceList == null)
              return;
            this.m_BuiltinResourceMap.Add(str, builtinResourceList);
          }
        }
      }

      public void InitBuiltinResources()
      {
        if (this.m_BuiltinResourceMap != null)
          return;
        this.m_BuiltinResourceMap = new Dictionary<string, BuiltinResource[]>();
        if (this.m_ShowNoneItem)
        {
          this.m_NoneList = new BuiltinResource[1];
          this.m_NoneList[0] = new BuiltinResource();
          this.m_NoneList[0].m_InstanceID = 0;
          this.m_NoneList[0].m_Name = "None";
        }
        else
          this.m_NoneList = new BuiltinResource[0];
        this.InitBuiltinAssetType(typeof (Mesh));
        this.InitBuiltinAssetType(typeof (Material));
        this.InitBuiltinAssetType(typeof (Texture2D));
        this.InitBuiltinAssetType(typeof (Font));
        this.InitBuiltinAssetType(typeof (Shader));
        this.InitBuiltinAssetType(typeof (Sprite));
        this.InitBuiltinAssetType(typeof (LightmapParameters));
      }

      public void PrintBuiltinResourcesAvailable()
      {
        string str = string.Empty + "ObjectSelector -Builtin Assets Available:\n";
        using (Dictionary<string, BuiltinResource[]>.Enumerator enumerator = this.m_BuiltinResourceMap.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, BuiltinResource[]> current = enumerator.Current;
            BuiltinResource[] builtinResourceArray = current.Value;
            str = str + "    " + current.Key + ": ";
            for (int index = 0; index < builtinResourceArray.Length; ++index)
            {
              if (index != 0)
                str += ", ";
              str += builtinResourceArray[index].m_Name;
            }
            str += "\n";
          }
        }
        Debug.Log((object) str);
      }

      public int IndexOfNewText(string newText, bool isCreatingNewFolder, bool foldersFirst)
      {
        int index = 0;
        if (this.m_ShowNoneItem)
          ++index;
        for (; index < this.m_FilteredHierarchy.results.Length; ++index)
        {
          FilteredHierarchy.FilterResult result = this.m_FilteredHierarchy.results[index];
          if ((!foldersFirst || !result.isFolder || isCreatingNewFolder) && (foldersFirst && !result.isFolder && isCreatingNewFolder || EditorUtility.NaturalCompare(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(result.instanceID)), newText) > 0))
            break;
        }
        return index;
      }

      public int IndexOf(int instanceID)
      {
        int num = 0;
        if (this.m_ShowNoneItem)
        {
          if (instanceID == 0)
            return 0;
          ++num;
        }
        else if (instanceID == 0)
          return -1;
        foreach (FilteredHierarchy.FilterResult result in this.m_FilteredHierarchy.results)
        {
          if (this.m_Owner.m_State.m_NewAssetIndexInList == num)
            ++num;
          if (result.instanceID == instanceID)
            return num;
          ++num;
        }
        foreach (BuiltinResource activeBuiltin in this.m_ActiveBuiltinList)
        {
          if (instanceID == activeBuiltin.m_InstanceID)
            return num;
          ++num;
        }
        return -1;
      }

      public FilteredHierarchy.FilterResult LookupByInstanceID(int instanceID)
      {
        if (instanceID == 0)
          return (FilteredHierarchy.FilterResult) null;
        int num = 0;
        foreach (FilteredHierarchy.FilterResult result in this.m_FilteredHierarchy.results)
        {
          if (this.m_Owner.m_State.m_NewAssetIndexInList == num)
            ++num;
          if (result.instanceID == instanceID)
            return result;
          ++num;
        }
        return (FilteredHierarchy.FilterResult) null;
      }

      public bool InstanceIdAtIndex(int index, out int instanceID)
      {
        instanceID = 0;
        if (index >= this.m_Grid.rows * this.m_Grid.columns)
          return false;
        int num = 0;
        if (this.m_ShowNoneItem)
        {
          if (index == 0)
            return true;
          ++num;
        }
        foreach (FilteredHierarchy.FilterResult result in this.m_FilteredHierarchy.results)
        {
          instanceID = result.instanceID;
          if (num == index)
            return true;
          ++num;
        }
        foreach (BuiltinResource activeBuiltin in this.m_ActiveBuiltinList)
        {
          instanceID = activeBuiltin.m_InstanceID;
          if (num == index)
            return true;
          ++num;
        }
        return index < this.m_Grid.rows * this.m_Grid.columns;
      }

      public virtual void StartDrag(int draggedInstanceID, List<int> selectedInstanceIDs)
      {
        ProjectWindowUtil.StartDrag(draggedInstanceID, selectedInstanceIDs);
      }

      public DragAndDropVisualMode DoDrag(int dragToInstanceID, bool perform)
      {
        HierarchyProperty property = new HierarchyProperty(HierarchyType.Assets);
        if (!property.Find(dragToInstanceID, (int[]) null))
          property = (HierarchyProperty) null;
        return InternalEditorUtility.ProjectWindowDrag(property, perform);
      }

      internal static int GetControlIDFromInstanceID(int instanceID)
      {
        return instanceID + 100000000;
      }

      public bool DoCharacterOffsetSelection()
      {
        if (Event.current.type == EventType.KeyDown && Event.current.shift)
        {
          StringComparison comparisonType = StringComparison.CurrentCultureIgnoreCase;
          string str1 = string.Empty;
          if (Selection.activeObject != (UnityEngine.Object) null)
            str1 = Selection.activeObject.name;
          string str2 = new string(new char[1]
          {
            Event.current.character
          });
          List<KeyValuePair<string, int>> nameAndInstanceIds = this.GetVisibleNameAndInstanceIDs();
          if (nameAndInstanceIds.Count == 0)
            return false;
          int num = 0;
          if (str1.StartsWith(str2, comparisonType))
          {
            for (int index = 0; index < nameAndInstanceIds.Count; ++index)
            {
              if (nameAndInstanceIds[index].Key == str1)
                num = index + 1;
            }
          }
          for (int index1 = 0; index1 < nameAndInstanceIds.Count; ++index1)
          {
            int index2 = (index1 + num) % nameAndInstanceIds.Count;
            if (nameAndInstanceIds[index2].Key.StartsWith(str2, comparisonType))
            {
              Selection.activeInstanceID = nameAndInstanceIds[index2].Value;
              this.m_Owner.Repaint();
              return true;
            }
          }
        }
        return false;
      }

      public void ShowObjectsInList(int[] instanceIDs)
      {
        this.m_FilteredHierarchy = new FilteredHierarchy(HierarchyType.Assets);
        this.m_FilteredHierarchy.SetResults(instanceIDs);
      }

      public static void DrawIconAndLabel(Rect rect, FilteredHierarchy.FilterResult filterItem, string label, Texture2D icon, bool selected, bool focus)
      {
        float num = !ObjectListArea.s_VCEnabled ? 0.0f : 14f;
        ObjectListArea.s_Styles.resultsLabel.padding.left = (int) ((double) num + 16.0 + 2.0);
        ObjectListArea.s_Styles.resultsLabel.Draw(rect, label, false, false, selected, focus);
        Rect position = rect;
        position.width = 16f;
        position.x += num * 0.5f;
        if ((UnityEngine.Object) icon != (UnityEngine.Object) null)
          GUI.DrawTexture(position, (Texture) icon);
        if (filterItem == null || !filterItem.isMainRepresentation)
          return;
        Rect drawRect = rect;
        drawRect.width = num + 16f;
        ProjectHooks.OnProjectWindowItem(filterItem.guid, drawRect);
      }

      private class ItemFader
      {
        private double m_FadeDuration = 0.3;
        private double m_FirstToLastDuration = 0.3;
        private double m_FadeStartTime;
        private double m_TimeBetweenEachItem;
        private List<int> m_InstanceIDs;

        public void Start(List<int> instanceIDs)
        {
          this.m_InstanceIDs = instanceIDs;
          this.m_FadeStartTime = EditorApplication.timeSinceStartup;
          this.m_FirstToLastDuration = Math.Min(0.5, (double) instanceIDs.Count * 0.03);
          this.m_TimeBetweenEachItem = 0.0;
          if (this.m_InstanceIDs.Count <= 1)
            return;
          this.m_TimeBetweenEachItem = this.m_FirstToLastDuration / (double) (this.m_InstanceIDs.Count - 1);
        }

        public float GetAlpha(int instanceID)
        {
          if (this.m_InstanceIDs == null)
            return 1f;
          if (EditorApplication.timeSinceStartup > this.m_FadeStartTime + this.m_FadeDuration + this.m_FirstToLastDuration)
          {
            this.m_InstanceIDs = (List<int>) null;
            return 1f;
          }
          int num1 = this.m_InstanceIDs.IndexOf(instanceID);
          if (num1 < 0)
            return 1f;
          double num2 = EditorApplication.timeSinceStartup - this.m_FadeStartTime;
          double num3 = this.m_TimeBetweenEachItem * (double) num1;
          float num4 = 0.0f;
          if (num3 < num2)
            num4 = Mathf.Clamp((float) ((num2 - num3) / this.m_FadeDuration), 0.0f, 1f);
          return num4;
        }
      }
    }
  }
}
