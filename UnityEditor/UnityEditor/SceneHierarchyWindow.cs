// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneHierarchyWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Hierarchy", useTypeNameAsIconName = true)]
  internal class SceneHierarchyWindow : SearchableEditorWindow, IHasCustomMenu
  {
    private static List<SceneHierarchyWindow> s_SceneHierarchyWindow = new List<SceneHierarchyWindow>();
    public static bool s_Debug = SessionState.GetBool("HierarchyWindowDebug", false);
    [SerializeField]
    private string m_CurrentSortMethod = string.Empty;
    [NonSerialized]
    private int m_LastFramedID = -1;
    private const int kInvalidSceneHandle = 0;
    private const float toolbarHeight = 17f;
    private static SceneHierarchyWindow s_LastInteractedHierarchy;
    private static SceneHierarchyWindow.Styles s_Styles;
    private TreeView m_TreeView;
    [SerializeField]
    private TreeViewState m_TreeViewState;
    private int m_TreeViewKeyboardControlID;
    [SerializeField]
    private int m_CurrenRootInstanceID;
    [SerializeField]
    private bool m_Locked;
    [NonSerialized]
    private bool m_TreeViewReloadNeeded;
    [NonSerialized]
    private bool m_SelectionSyncNeeded;
    [NonSerialized]
    private bool m_FrameOnSelectionSync;
    [NonSerialized]
    private bool m_DidSelectSearchResult;
    private Dictionary<string, BaseHierarchySort> m_SortingObjects;
    private bool m_AllowAlphaNumericalSort;
    [NonSerialized]
    private double m_LastUserInteractionTime;
    private bool m_Debug;

    public static SceneHierarchyWindow lastInteractedHierarchyWindow
    {
      get
      {
        return SceneHierarchyWindow.s_LastInteractedHierarchy;
      }
    }

    internal static bool debug
    {
      get
      {
        return SceneHierarchyWindow.lastInteractedHierarchyWindow.m_Debug;
      }
      set
      {
        SceneHierarchyWindow.lastInteractedHierarchyWindow.m_Debug = value;
      }
    }

    private bool treeViewReloadNeeded
    {
      get
      {
        return this.m_TreeViewReloadNeeded;
      }
      set
      {
        this.m_TreeViewReloadNeeded = value;
        if (!value)
          return;
        this.Repaint();
        if (!SceneHierarchyWindow.s_Debug)
          return;
        Debug.Log((object) "Reload treeview on next event");
      }
    }

    private bool selectionSyncNeeded
    {
      get
      {
        return this.m_SelectionSyncNeeded;
      }
      set
      {
        this.m_SelectionSyncNeeded = value;
        if (!value)
          return;
        this.Repaint();
        if (!SceneHierarchyWindow.s_Debug)
          return;
        Debug.Log((object) "Selection sync and frameing on next event");
      }
    }

    private string currentSortMethod
    {
      get
      {
        return this.m_CurrentSortMethod;
      }
      set
      {
        this.m_CurrentSortMethod = value;
        if (!this.m_SortingObjects.ContainsKey(this.m_CurrentSortMethod))
          this.m_CurrentSortMethod = this.GetNameForType(typeof (TransformSort));
        GameObjectTreeViewDataSource data = (GameObjectTreeViewDataSource) this.treeView.data;
        data.sortingState.sortingObject = this.m_SortingObjects[this.m_CurrentSortMethod];
        ((GameObjectsTreeViewDragging) this.treeView.dragging).allowDragBetween = !data.sortingState.implementsCompare;
      }
    }

    private bool hasSortMethods
    {
      get
      {
        return this.m_SortingObjects.Count > 1;
      }
    }

    private Rect treeViewRect
    {
      get
      {
        return new Rect(0.0f, 17f, this.position.width, this.position.height - 17f);
      }
    }

    private TreeView treeView
    {
      get
      {
        if (this.m_TreeView == null)
          this.Init();
        return this.m_TreeView;
      }
    }

    public static List<SceneHierarchyWindow> GetAllSceneHierarchyWindows()
    {
      return SceneHierarchyWindow.s_SceneHierarchyWindow;
    }

    private void Init()
    {
      if (this.m_TreeViewState == null)
        this.m_TreeViewState = new TreeViewState();
      this.m_TreeView = new TreeView((EditorWindow) this, this.m_TreeViewState);
      this.m_TreeView.itemDoubleClickedCallback += new System.Action<int>(this.TreeViewItemDoubleClicked);
      this.m_TreeView.selectionChangedCallback += new System.Action<int[]>(this.TreeViewSelectionChanged);
      this.m_TreeView.onGUIRowCallback += new System.Action<int, Rect>(this.OnGUIAssetCallback);
      this.m_TreeView.dragEndedCallback += new System.Action<int[], bool>(this.OnDragEndedCallback);
      this.m_TreeView.contextClickItemCallback += new System.Action<int>(this.ItemContextClick);
      this.m_TreeView.contextClickOutsideItemsCallback += new System.Action(this.ContextClickOutsideItems);
      this.m_TreeView.deselectOnUnhandledMouseDown = true;
      GameObjectTreeViewDataSource treeViewDataSource = new GameObjectTreeViewDataSource(this.m_TreeView, this.m_CurrenRootInstanceID, false, false);
      GameObjectsTreeViewDragging treeViewDragging = new GameObjectsTreeViewDragging(this.m_TreeView);
      GameObjectTreeViewGUI objectTreeViewGui = new GameObjectTreeViewGUI(this.m_TreeView, false);
      this.m_TreeView.Init(this.treeViewRect, (ITreeViewDataSource) treeViewDataSource, (ITreeViewGUI) objectTreeViewGui, (ITreeViewDragging) treeViewDragging);
      treeViewDataSource.searchMode = (int) this.m_SearchMode;
      treeViewDataSource.searchString = this.m_SearchFilter;
      this.m_AllowAlphaNumericalSort = EditorPrefs.GetBool("AllowAlphaNumericHierarchy", false) || InternalEditorUtility.inBatchMode;
      this.SetUpSortMethodLists();
      this.m_TreeView.ReloadData();
    }

    public void SetCurrentRootInstanceID(int instanceID)
    {
      this.m_CurrenRootInstanceID = instanceID;
      this.Init();
      GUIUtility.ExitGUI();
    }

    public UnityEngine.Object[] GetCurrentVisibleObjects()
    {
      List<TreeViewItem> rows = this.m_TreeView.data.GetRows();
      UnityEngine.Object[] objectArray = new UnityEngine.Object[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
        objectArray[index] = ((GameObjectTreeViewItem) rows[index]).objectPPTR;
      return objectArray;
    }

    internal void SelectPrevious()
    {
      this.m_TreeView.OffsetSelection(-1);
    }

    internal void SelectNext()
    {
      this.m_TreeView.OffsetSelection(1);
    }

    private void Awake()
    {
      this.m_HierarchyType = HierarchyType.GameObjects;
      if (this.m_TreeViewState == null)
        return;
      this.m_TreeViewState.OnAwake();
    }

    private void OnBecameVisible()
    {
      if (SceneManager.sceneCount <= 0)
        return;
      this.treeViewReloadNeeded = true;
    }

    public override void OnEnable()
    {
      base.OnEnable();
      this.titleContent = this.GetLocalizedTitleContent();
      SceneHierarchyWindow.s_SceneHierarchyWindow.Add(this);
      EditorApplication.projectWindowChanged += new EditorApplication.CallbackFunction(this.ReloadData);
      EditorApplication.searchChanged += new EditorApplication.CallbackFunction(this.SearchChanged);
      SceneHierarchyWindow.s_LastInteractedHierarchy = this;
    }

    public override void OnDisable()
    {
      EditorApplication.projectWindowChanged -= new EditorApplication.CallbackFunction(this.ReloadData);
      EditorApplication.searchChanged -= new EditorApplication.CallbackFunction(this.SearchChanged);
      SceneHierarchyWindow.s_SceneHierarchyWindow.Remove(this);
    }

    public void OnDestroy()
    {
      if (!((UnityEngine.Object) SceneHierarchyWindow.s_LastInteractedHierarchy == (UnityEngine.Object) this))
        return;
      SceneHierarchyWindow.s_LastInteractedHierarchy = (SceneHierarchyWindow) null;
      using (List<SceneHierarchyWindow>.Enumerator enumerator = SceneHierarchyWindow.s_SceneHierarchyWindow.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SceneHierarchyWindow current = enumerator.Current;
          if ((UnityEngine.Object) current != (UnityEngine.Object) this)
            SceneHierarchyWindow.s_LastInteractedHierarchy = current;
        }
      }
    }

    private void SetAsLastInteractedHierarchy()
    {
      SceneHierarchyWindow.s_LastInteractedHierarchy = this;
    }

    private void SyncIfNeeded()
    {
      if (this.treeViewReloadNeeded)
      {
        this.treeViewReloadNeeded = false;
        this.ReloadData();
      }
      if (!this.selectionSyncNeeded)
        return;
      this.selectionSyncNeeded = false;
      bool flag = EditorApplication.timeSinceStartup - this.m_LastUserInteractionTime < 0.2;
      bool revealSelectionAndFrameLastSelected = !this.m_Locked || this.m_FrameOnSelectionSync || flag;
      bool animatedFraming = flag && revealSelectionAndFrameLastSelected;
      this.m_FrameOnSelectionSync = false;
      this.treeView.SetSelection(Selection.instanceIDs, revealSelectionAndFrameLastSelected, animatedFraming);
    }

    private void DetectUserInteraction()
    {
      Event current = Event.current;
      if (current.type == EventType.Layout || current.type == EventType.Repaint)
        return;
      this.m_LastUserInteractionTime = EditorApplication.timeSinceStartup;
    }

    private void OnGUI()
    {
      if (SceneHierarchyWindow.s_Styles == null)
        SceneHierarchyWindow.s_Styles = new SceneHierarchyWindow.Styles();
      this.DetectUserInteraction();
      this.SyncIfNeeded();
      this.m_TreeViewKeyboardControlID = GUIUtility.GetControlID(FocusType.Keyboard);
      this.OnEvent();
      Rect rect = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      Event current = Event.current;
      if (current.type == EventType.MouseDown && rect.Contains(current.mousePosition))
      {
        this.treeView.EndPing();
        this.SetAsLastInteractedHierarchy();
      }
      this.DoToolbar();
      this.DoTreeView(this.DoSearchResultPathGUI());
      this.ExecuteCommands();
    }

    private void OnLostFocus()
    {
      this.treeView.EndNameEditing(true);
      EditorGUI.EndEditingActiveTextField();
    }

    public static bool IsSceneHeaderInHierarchyWindow(Scene scene)
    {
      return scene.IsValid();
    }

    private void TreeViewItemDoubleClicked(int instanceID)
    {
      Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(instanceID);
      if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(sceneByHandle))
      {
        if (!sceneByHandle.isLoaded)
          return;
        SceneManager.SetActiveScene(sceneByHandle);
      }
      else
        SceneView.FrameLastActiveSceneView();
    }

    public void SetExpandedRecursive(int id, bool expand)
    {
      TreeViewItem treeViewItem = this.treeView.data.FindItem(id);
      if (treeViewItem == null)
      {
        this.ReloadData();
        treeViewItem = this.treeView.data.FindItem(id);
      }
      if (treeViewItem == null)
        return;
      this.treeView.data.SetExpandedWithChildren(treeViewItem, expand);
    }

    private void OnGUIAssetCallback(int instanceID, Rect rect)
    {
      if (EditorApplication.hierarchyWindowItemOnGUI == null)
        return;
      EditorApplication.hierarchyWindowItemOnGUI(instanceID, rect);
    }

    private void OnDragEndedCallback(int[] draggedInstanceIds, bool draggedItemsFromOwnTreeView)
    {
      if (draggedInstanceIds == null || !draggedItemsFromOwnTreeView)
        return;
      this.ReloadData();
      this.treeView.SetSelection(draggedInstanceIds, true);
      this.treeView.NotifyListenersThatSelectionChanged();
      this.Repaint();
      GUIUtility.ExitGUI();
    }

    public void ReloadData()
    {
      if (this.m_TreeView == null)
        this.Init();
      else
        this.m_TreeView.ReloadData();
    }

    public void SearchChanged()
    {
      GameObjectTreeViewDataSource data = (GameObjectTreeViewDataSource) this.treeView.data;
      if ((SearchableEditorWindow.SearchMode) data.searchMode == this.searchMode && data.searchString == this.m_SearchFilter)
        return;
      data.searchMode = (int) this.searchMode;
      data.searchString = this.m_SearchFilter;
      if (this.m_SearchFilter == string.Empty)
        this.treeView.Frame(Selection.activeInstanceID, true, false);
      this.ReloadData();
    }

    private void TreeViewSelectionChanged(int[] ids)
    {
      Selection.instanceIDs = ids;
      this.m_DidSelectSearchResult = !string.IsNullOrEmpty(this.m_SearchFilter);
    }

    private bool IsTreeViewSelectionInSyncWithBackend()
    {
      if (this.m_TreeView != null)
        return this.m_TreeView.state.selectedIDs.SequenceEqual<int>((IEnumerable<int>) Selection.instanceIDs);
      return false;
    }

    private void OnSelectionChange()
    {
      if (!this.IsTreeViewSelectionInSyncWithBackend())
      {
        this.selectionSyncNeeded = true;
      }
      else
      {
        if (!SceneHierarchyWindow.s_Debug)
          return;
        Debug.Log((object) "OnSelectionChange: Selection is already in sync so no framing will happen");
      }
    }

    private void OnHierarchyChange()
    {
      if (this.m_TreeView != null)
        this.m_TreeView.EndNameEditing(false);
      this.treeViewReloadNeeded = true;
    }

    private float DoSearchResultPathGUI()
    {
      if (!this.hasSearchFilter)
        return 0.0f;
      GUILayout.FlexibleSpace();
      Rect rect = EditorGUILayout.BeginVertical(EditorStyles.inspectorBig, new GUILayoutOption[0]);
      GUILayout.Label("Path:");
      if (this.m_TreeView.HasSelection())
      {
        int instanceID = this.m_TreeView.GetSelection()[0];
        IHierarchyProperty hierarchyProperty = (IHierarchyProperty) new HierarchyProperty(HierarchyType.GameObjects);
        hierarchyProperty.Find(instanceID, (int[]) null);
        if (hierarchyProperty.isValid)
        {
          do
          {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label((Texture) hierarchyProperty.icon);
            GUILayout.Label(hierarchyProperty.name);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
          }
          while (hierarchyProperty.Parent());
        }
      }
      EditorGUILayout.EndVertical();
      GUILayout.Space(0.0f);
      return rect.height;
    }

    private void OnEvent()
    {
      this.treeView.OnEvent();
    }

    private void DoTreeView(float searchPathHeight)
    {
      Rect treeViewRect = this.treeViewRect;
      treeViewRect.height -= searchPathHeight;
      this.treeView.OnGUI(treeViewRect, this.m_TreeViewKeyboardControlID);
    }

    private void DoToolbar()
    {
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      this.CreateGameObjectPopup();
      GUILayout.Space(6f);
      if (SceneHierarchyWindow.s_Debug)
      {
        int firstRowVisible;
        int lastRowVisible;
        this.m_TreeView.gui.GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible);
        GUILayout.Label(string.Format("{0} ({1}, {2})", (object) this.m_TreeView.data.rowCount, (object) firstRowVisible, (object) lastRowVisible), EditorStyles.miniLabel, new GUILayoutOption[0]);
        GUILayout.Space(6f);
      }
      GUILayout.FlexibleSpace();
      Event current = Event.current;
      if (this.hasSearchFilterFocus && current.type == EventType.KeyDown && (current.keyCode == KeyCode.DownArrow || current.keyCode == KeyCode.UpArrow))
      {
        GUIUtility.keyboardControl = this.m_TreeViewKeyboardControlID;
        if (this.treeView.IsLastClickedPartOfRows())
        {
          this.treeView.Frame(this.treeView.state.lastClickedID, true, false);
          this.m_DidSelectSearchResult = !string.IsNullOrEmpty(this.m_SearchFilter);
        }
        else
          this.treeView.OffsetSelection(1);
        current.Use();
      }
      this.SearchFieldGUI();
      GUILayout.Space(6f);
      if (this.hasSortMethods)
      {
        if (Application.isPlaying && ((GameObjectTreeViewDataSource) this.treeView.data).isFetchAIssue)
          GUILayout.Toggle(false, SceneHierarchyWindow.s_Styles.fetchWarning, SceneHierarchyWindow.s_Styles.MiniButton, new GUILayoutOption[0]);
        this.SortMethodsDropDown();
      }
      GUILayout.EndHorizontal();
    }

    internal override void SetSearchFilter(string searchFilter, SearchableEditorWindow.SearchMode searchMode, bool setAll)
    {
      base.SetSearchFilter(searchFilter, searchMode, setAll);
      if (!this.m_DidSelectSearchResult || !string.IsNullOrEmpty(searchFilter))
        return;
      this.m_DidSelectSearchResult = false;
      this.FrameObjectPrivate(Selection.activeInstanceID, true, false, false);
      if (GUIUtility.keyboardControl != 0)
        return;
      GUIUtility.keyboardControl = this.m_TreeViewKeyboardControlID;
    }

    private void AddCreateGameObjectItemsToMenu(GenericMenu menu, UnityEngine.Object[] context, bool includeCreateEmptyChild, bool includeGameObjectInPath, int targetSceneHandle)
    {
      foreach (string submenu in Unsupported.GetSubmenus("GameObject"))
      {
        UnityEngine.Object[] temporaryContext = context;
        if (includeCreateEmptyChild || !(submenu.ToLower() == "GameObject/Create Empty Child".ToLower()))
        {
          if (submenu.EndsWith("..."))
            temporaryContext = (UnityEngine.Object[]) null;
          if (submenu.ToLower() == "GameObject/Center On Children".ToLower())
            break;
          string replacementMenuString = submenu;
          if (!includeGameObjectInPath)
            replacementMenuString = submenu.Substring(11);
          MenuUtils.ExtractMenuItemWithPath(submenu, menu, replacementMenuString, temporaryContext, targetSceneHandle, new System.Action<string, UnityEngine.Object[], int>(this.BeforeCreateGameObjectMenuItemWasExecuted), new System.Action<string, UnityEngine.Object[], int>(this.AfterCreateGameObjectMenuItemWasExecuted));
        }
      }
    }

    private void BeforeCreateGameObjectMenuItemWasExecuted(string menuPath, UnityEngine.Object[] contextObjects, int userData)
    {
      EditorSceneManager.SetTargetSceneForNewGameObjects(userData);
    }

    private void AfterCreateGameObjectMenuItemWasExecuted(string menuPath, UnityEngine.Object[] contextObjects, int userData)
    {
      EditorSceneManager.SetTargetSceneForNewGameObjects(0);
      if (!this.m_Locked)
        return;
      this.m_FrameOnSelectionSync = true;
    }

    private void CreateGameObjectPopup()
    {
      Rect rect = GUILayoutUtility.GetRect(SceneHierarchyWindow.s_Styles.createContent, EditorStyles.toolbarDropDown, (GUILayoutOption[]) null);
      if (Event.current.type == EventType.Repaint)
        EditorStyles.toolbarDropDown.Draw(rect, SceneHierarchyWindow.s_Styles.createContent, false, false, false, false);
      if (Event.current.type != EventType.MouseDown || !rect.Contains(Event.current.mousePosition))
        return;
      GUIUtility.hotControl = 0;
      GenericMenu menu = new GenericMenu();
      this.AddCreateGameObjectItemsToMenu(menu, (UnityEngine.Object[]) null, true, false, 0);
      menu.DropDown(rect);
      Event.current.Use();
    }

    private void SortMethodsDropDown()
    {
      if (!this.hasSortMethods)
        return;
      GUIContent content = this.m_SortingObjects[this.currentSortMethod].content;
      if (content == null)
      {
        content = SceneHierarchyWindow.s_Styles.defaultSortingContent;
        content.tooltip = this.currentSortMethod;
      }
      Rect rect = GUILayoutUtility.GetRect(content, EditorStyles.toolbarButton);
      if (!EditorGUI.ButtonMouseDown(rect, content, FocusType.Passive, EditorStyles.toolbarButton))
        return;
      List<SceneHierarchySortingWindow.InputData> data = new List<SceneHierarchySortingWindow.InputData>();
      using (Dictionary<string, BaseHierarchySort>.Enumerator enumerator = this.m_SortingObjects.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, BaseHierarchySort> current = enumerator.Current;
          data.Add(new SceneHierarchySortingWindow.InputData()
          {
            m_TypeName = current.Key,
            m_Name = ObjectNames.NicifyVariableName(current.Key),
            m_Selected = current.Key == this.m_CurrentSortMethod
          });
        }
      }
      if (!SceneHierarchySortingWindow.ShowAtPosition(new Vector2(rect.x, rect.y + rect.height), data, new SceneHierarchySortingWindow.OnSelectCallback(this.SortFunctionCallback)))
        return;
      GUIUtility.ExitGUI();
    }

    private void SetUpSortMethodLists()
    {
      this.m_SortingObjects = new Dictionary<string, BaseHierarchySort>();
      foreach (Assembly loadedAssembly in EditorAssemblies.loadedAssemblies)
      {
        foreach (BaseHierarchySort implementor in AssemblyHelper.FindImplementors<BaseHierarchySort>(loadedAssembly))
        {
          if (implementor.GetType() != typeof (AlphabeticalSort) || this.m_AllowAlphaNumericalSort)
            this.m_SortingObjects.Add(this.GetNameForType(implementor.GetType()), implementor);
        }
      }
      this.currentSortMethod = this.m_CurrentSortMethod;
    }

    private string GetNameForType(System.Type type)
    {
      return type.Name;
    }

    private void SortFunctionCallback(SceneHierarchySortingWindow.InputData data)
    {
      this.SetSortFunction(data.m_TypeName);
    }

    public void SetSortFunction(System.Type sortType)
    {
      this.SetSortFunction(this.GetNameForType(sortType));
    }

    private void SetSortFunction(string sortTypeName)
    {
      if (!this.m_SortingObjects.ContainsKey(sortTypeName))
      {
        Debug.LogError((object) ("Invalid search type name: " + sortTypeName));
      }
      else
      {
        this.currentSortMethod = sortTypeName;
        if (((IEnumerable<int>) this.treeView.GetSelection()).Any<int>())
          this.treeView.Frame(((IEnumerable<int>) this.treeView.GetSelection()).First<int>(), true, false);
        this.treeView.ReloadData();
      }
    }

    public void DirtySortingMethods()
    {
      this.m_AllowAlphaNumericalSort = EditorPrefs.GetBool("AllowAlphaNumericHierarchy", false);
      this.SetUpSortMethodLists();
      this.treeView.SetSelection(this.treeView.GetSelection(), true);
      this.treeView.ReloadData();
    }

    private void ExecuteCommands()
    {
      Event current = Event.current;
      if (current.type != EventType.ExecuteCommand && current.type != EventType.ValidateCommand)
        return;
      bool flag = current.type == EventType.ExecuteCommand;
      if (current.commandName == "Delete" || current.commandName == "SoftDelete")
      {
        if (flag)
          this.DeleteGO();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "Duplicate")
      {
        if (flag)
          this.DuplicateGO();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "Copy")
      {
        if (flag)
          this.CopyGO();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "Paste")
      {
        if (flag)
          this.PasteGO();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "SelectAll")
      {
        if (flag)
          this.SelectAll();
        current.Use();
        GUIUtility.ExitGUI();
      }
      else if (current.commandName == "FrameSelected")
      {
        if (current.type == EventType.ExecuteCommand)
          this.FrameObjectPrivate(Selection.activeInstanceID, true, true, true);
        current.Use();
        GUIUtility.ExitGUI();
      }
      else
      {
        if (!(current.commandName == "Find"))
          return;
        if (current.type == EventType.ExecuteCommand)
          this.FocusSearchField();
        current.Use();
      }
    }

    private void CreateGameObjectContextClick(GenericMenu menu, int contextClickedItemID)
    {
      menu.AddItem(EditorGUIUtility.TextContent("Copy"), false, new GenericMenu.MenuFunction(this.CopyGO));
      menu.AddItem(EditorGUIUtility.TextContent("Paste"), false, new GenericMenu.MenuFunction(this.PasteGO));
      menu.AddSeparator(string.Empty);
      if (!this.hasSearchFilter && this.m_TreeViewState.selectedIDs.Count == 1)
        menu.AddItem(EditorGUIUtility.TextContent("Rename"), false, new GenericMenu.MenuFunction(this.RenameGO));
      else
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Rename"));
      menu.AddItem(EditorGUIUtility.TextContent("Duplicate"), false, new GenericMenu.MenuFunction(this.DuplicateGO));
      menu.AddItem(EditorGUIUtility.TextContent("Delete"), false, new GenericMenu.MenuFunction(this.DeleteGO));
      menu.AddSeparator(string.Empty);
      bool flag = false;
      if (this.m_TreeViewState.selectedIDs.Count == 1)
      {
        GameObjectTreeViewItem node = this.treeView.FindNode(this.m_TreeViewState.selectedIDs[0]) as GameObjectTreeViewItem;
        if (node != null)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          SceneHierarchyWindow.\u003CCreateGameObjectContextClick\u003Ec__AnonStorey42 clickCAnonStorey42 = new SceneHierarchyWindow.\u003CCreateGameObjectContextClick\u003Ec__AnonStorey42();
          // ISSUE: reference to a compiler-generated field
          clickCAnonStorey42.prefab = PrefabUtility.GetPrefabParent(node.objectPPTR);
          // ISSUE: reference to a compiler-generated field
          if (clickCAnonStorey42.prefab != (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated method
            menu.AddItem(EditorGUIUtility.TextContent("Select Prefab"), false, new GenericMenu.MenuFunction(clickCAnonStorey42.\u003C\u003Em__65));
            flag = true;
          }
        }
      }
      if (!flag)
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Select Prefab"));
      menu.AddSeparator(string.Empty);
      this.AddCreateGameObjectItemsToMenu(menu, (UnityEngine.Object[]) ((IEnumerable<Transform>) Selection.transforms).Select<Transform, GameObject>((Func<Transform, GameObject>) (t => t.gameObject)).ToArray<GameObject>(), false, false, 0);
      menu.ShowAsContext();
    }

    private void CreateMultiSceneHeaderContextClick(GenericMenu menu, int contextClickedItemID)
    {
      Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(contextClickedItemID);
      if (!SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(sceneByHandle))
      {
        Debug.LogError((object) "Context clicked item is not a scene");
      }
      else
      {
        if (sceneByHandle.isLoaded)
        {
          menu.AddItem(EditorGUIUtility.TextContent("Set Active Scene"), false, new GenericMenu.MenuFunction2(this.SetSceneActive), (object) contextClickedItemID);
          menu.AddSeparator(string.Empty);
        }
        if (sceneByHandle.isLoaded)
        {
          if (!EditorApplication.isPlaying)
          {
            menu.AddItem(EditorGUIUtility.TextContent("Save Scene"), false, new GenericMenu.MenuFunction2(this.SaveSelectedScenes), (object) contextClickedItemID);
            menu.AddItem(EditorGUIUtility.TextContent("Save Scene As"), false, new GenericMenu.MenuFunction2(this.SaveSceneAs), (object) contextClickedItemID);
            menu.AddItem(EditorGUIUtility.TextContent("Save All"), false, new GenericMenu.MenuFunction2(this.SaveAllScenes), (object) contextClickedItemID);
          }
          else
          {
            menu.AddDisabledItem(EditorGUIUtility.TextContent("Save Scene"));
            menu.AddDisabledItem(EditorGUIUtility.TextContent("Save Scene As"));
            menu.AddDisabledItem(EditorGUIUtility.TextContent("Save All"));
          }
          menu.AddSeparator(string.Empty);
        }
        bool flag1 = EditorSceneManager.loadedSceneCount != this.GetNumLoadedScenesInSelection();
        if (sceneByHandle.isLoaded)
        {
          if (flag1 && !EditorApplication.isPlaying && !string.IsNullOrEmpty(sceneByHandle.path))
            menu.AddItem(EditorGUIUtility.TextContent("Unload Scene"), false, new GenericMenu.MenuFunction2(this.UnloadSelectedScenes), (object) contextClickedItemID);
          else
            menu.AddDisabledItem(EditorGUIUtility.TextContent("Unload Scene"));
        }
        else if (!EditorApplication.isPlaying)
          menu.AddItem(EditorGUIUtility.TextContent("Load Scene"), false, new GenericMenu.MenuFunction2(this.LoadSelectedScenes), (object) contextClickedItemID);
        else
          menu.AddDisabledItem(EditorGUIUtility.TextContent("Load Scene"));
        bool flag2 = this.GetSelectedScenes().Count == SceneManager.sceneCount;
        if (flag1 && !flag2 && !EditorApplication.isPlaying)
          menu.AddItem(EditorGUIUtility.TextContent("Remove Scene"), false, new GenericMenu.MenuFunction2(this.RemoveSelectedScenes), (object) contextClickedItemID);
        else
          menu.AddDisabledItem(EditorGUIUtility.TextContent("Remove Scene"));
        menu.AddSeparator(string.Empty);
        if (!string.IsNullOrEmpty(sceneByHandle.path))
          menu.AddItem(EditorGUIUtility.TextContent("Select Scene Asset"), false, new GenericMenu.MenuFunction2(this.SelectSceneAsset), (object) contextClickedItemID);
        else
          menu.AddDisabledItem(new GUIContent("Select Scene Asset"));
        if (!sceneByHandle.isLoaded)
          return;
        menu.AddSeparator(string.Empty);
        this.AddCreateGameObjectItemsToMenu(menu, (UnityEngine.Object[]) ((IEnumerable<Transform>) Selection.transforms).Select<Transform, GameObject>((Func<Transform, GameObject>) (t => t.gameObject)).ToArray<GameObject>(), false, true, sceneByHandle.handle);
      }
    }

    private int GetNumLoadedScenesInSelection()
    {
      int num = 0;
      using (List<int>.Enumerator enumerator = this.GetSelectedScenes().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (EditorSceneManager.GetSceneByHandle(enumerator.Current).isLoaded)
            ++num;
        }
      }
      return num;
    }

    private List<int> GetSelectedScenes()
    {
      List<int> intList = new List<int>();
      foreach (int handle in this.m_TreeView.GetSelection())
      {
        if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(EditorSceneManager.GetSceneByHandle(handle)))
          intList.Add(handle);
      }
      return intList;
    }

    private List<int> GetSelectedGameObjects()
    {
      List<int> intList = new List<int>();
      foreach (int handle in this.m_TreeView.GetSelection())
      {
        if (!SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(EditorSceneManager.GetSceneByHandle(handle)))
          intList.Add(handle);
      }
      return intList;
    }

    private void ContextClickOutsideItems()
    {
      Event.current.Use();
      GenericMenu menu = new GenericMenu();
      this.CreateGameObjectContextClick(menu, 0);
      menu.ShowAsContext();
    }

    private void ItemContextClick(int contextClickedItemID)
    {
      Event.current.Use();
      GenericMenu menu = new GenericMenu();
      if (SceneHierarchyWindow.IsSceneHeaderInHierarchyWindow(EditorSceneManager.GetSceneByHandle(contextClickedItemID)))
        this.CreateMultiSceneHeaderContextClick(menu, contextClickedItemID);
      else
        this.CreateGameObjectContextClick(menu, contextClickedItemID);
      menu.ShowAsContext();
    }

    private void CopyGO()
    {
      Unsupported.CopyGameObjectsToPasteboard();
    }

    private void PasteGO()
    {
      Unsupported.PasteGameObjectsFromPasteboard();
    }

    private void DuplicateGO()
    {
      Unsupported.DuplicateGameObjectsUsingPasteboard();
    }

    private void RenameGO()
    {
      this.treeView.BeginNameEditing(0.0f);
    }

    private void DeleteGO()
    {
      Unsupported.DeleteGameObjectSelection();
    }

    private void SetSceneActive(object userData)
    {
      SceneManager.SetActiveScene(EditorSceneManager.GetSceneByHandle((int) userData));
    }

    private void LoadSelectedScenes(object userdata)
    {
      using (List<int>.Enumerator enumerator = this.GetSelectedScenes().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(enumerator.Current);
          if (!sceneByHandle.isLoaded)
            EditorSceneManager.OpenScene(sceneByHandle.path, OpenSceneMode.Additive);
        }
      }
      EditorApplication.RequestRepaintAllViews();
    }

    private void SaveSceneAs(object userdata)
    {
      Scene sceneByHandle = EditorSceneManager.GetSceneByHandle((int) userdata);
      if (!sceneByHandle.isLoaded)
        return;
      EditorSceneManager.SaveSceneAs(sceneByHandle);
    }

    private void SaveAllScenes(object userdata)
    {
      EditorSceneManager.SaveOpenScenes();
    }

    private void SaveSelectedScenes(object userdata)
    {
      using (List<int>.Enumerator enumerator = this.GetSelectedScenes().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Scene sceneByHandle = EditorSceneManager.GetSceneByHandle(enumerator.Current);
          if (sceneByHandle.isLoaded)
            EditorSceneManager.SaveScene(sceneByHandle);
        }
      }
    }

    private void UnloadSelectedScenes(object userdata)
    {
      this.CloseSelectedScenes(false);
    }

    private void RemoveSelectedScenes(object userData)
    {
      this.CloseSelectedScenes(true);
    }

    private Scene[] GetModifiedScenes(List<int> handles)
    {
      return handles.Select<int, Scene>((Func<int, Scene>) (handle => EditorSceneManager.GetSceneByHandle(handle))).Where<Scene>((Func<Scene, bool>) (scene => scene.isDirty)).ToArray<Scene>();
    }

    private void CloseSelectedScenes(bool removeScenes)
    {
      List<int> selectedScenes = this.GetSelectedScenes();
      if (!EditorSceneManager.SaveModifiedScenesIfUserWantsTo(this.GetModifiedScenes(selectedScenes)))
        return;
      using (List<int>.Enumerator enumerator = selectedScenes.GetEnumerator())
      {
        while (enumerator.MoveNext())
          EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByHandle(enumerator.Current), removeScenes);
      }
      EditorApplication.RequestRepaintAllViews();
    }

    private void SelectSceneAsset(object userData)
    {
      int instanceIdFromGuid = AssetDatabase.GetInstanceIDFromGUID(AssetDatabase.AssetPathToGUID(EditorSceneManager.GetSceneByHandle((int) userData).path));
      Selection.activeInstanceID = instanceIdFromGuid;
      EditorGUIUtility.PingObject(instanceIdFromGuid);
    }

    private void SelectAll()
    {
      int[] rowIds = this.treeView.GetRowIDs();
      this.treeView.SetSelection(rowIds, false);
      this.TreeViewSelectionChanged(rowIds);
    }

    private static void ToggleDebugMode()
    {
      SceneHierarchyWindow.s_Debug = !SceneHierarchyWindow.s_Debug;
      SessionState.SetBool("HierarchyWindowDebug", SceneHierarchyWindow.s_Debug);
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      if (!Unsupported.IsDeveloperBuild())
        return;
      menu.AddItem(new GUIContent("DEVELOPER/Toggle DebugMode"), false, new GenericMenu.MenuFunction(SceneHierarchyWindow.ToggleDebugMode));
    }

    public void FrameObject(int instanceID, bool ping)
    {
      this.FrameObjectPrivate(instanceID, true, ping, true);
    }

    private void FrameObjectPrivate(int instanceID, bool frame, bool ping, bool animatedFraming)
    {
      if (instanceID == 0)
        return;
      if (this.m_LastFramedID != instanceID)
        this.treeView.EndPing();
      this.SetSearchFilter(string.Empty, SearchableEditorWindow.SearchMode.All, true);
      this.m_LastFramedID = instanceID;
      this.treeView.Frame(instanceID, frame, ping, animatedFraming);
      this.FrameObjectPrivate(InternalEditorUtility.GetGameObjectInstanceIDFromComponent(instanceID), frame, ping, animatedFraming);
    }

    protected virtual void ShowButton(Rect r)
    {
      if (SceneHierarchyWindow.s_Styles == null)
        SceneHierarchyWindow.s_Styles = new SceneHierarchyWindow.Styles();
      this.m_Locked = GUI.Toggle(r, this.m_Locked, GUIContent.none, SceneHierarchyWindow.s_Styles.lockButton);
    }

    private class Styles
    {
      public GUIContent defaultSortingContent = new GUIContent((Texture) EditorGUIUtility.FindTexture("CustomSorting"));
      public GUIContent createContent = new GUIContent("Create");
      public GUIContent fetchWarning = new GUIContent(string.Empty, (Texture) EditorGUIUtility.FindTexture("console.warnicon.sml"), "The current sorting method is taking a lot of time. Consider using 'Transform Sort' in playmode for better performance.");
      public GUIStyle lockButton = (GUIStyle) "IN LockButton";
      private const string kCustomSorting = "CustomSorting";
      private const string kWarningSymbol = "console.warnicon.sml";
      private const string kWarningMessage = "The current sorting method is taking a lot of time. Consider using 'Transform Sort' in playmode for better performance.";
      public GUIStyle MiniButton;

      public Styles()
      {
        this.MiniButton = (GUIStyle) "ToolbarButton";
      }
    }
  }
}
