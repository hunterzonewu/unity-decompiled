// Decompiled with JetBrains decompiler
// Type: UnityEditor.ObjectSelector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  internal class ObjectSelector : EditorWindow
  {
    private SavedInt m_StartGridSize = new SavedInt("ObjectSelector.GridSize", 64);
    private int m_ModalUndoGroup = -1;
    private PreviewResizer m_PreviewResizer = new PreviewResizer();
    private ObjectTreeForSelector m_ObjectTreeWithSearch = new ObjectTreeForSelector();
    private float m_ToolbarHeight = 44f;
    private AnimBool m_ShowWidePreview = new AnimBool();
    private AnimBool m_ShowOverlapPreview = new AnimBool();
    private const float kMinTopSize = 250f;
    private const float kMinWidth = 200f;
    private const float kPreviewMargin = 5f;
    private const float kPreviewExpandedAreaHeight = 75f;
    private ObjectSelector.Styles m_Styles;
    private string m_RequiredType;
    private string m_SearchFilter;
    private bool m_FocusSearchFilter;
    private bool m_AllowSceneObjects;
    private bool m_IsShowingAssets;
    internal int objectSelectorID;
    private UnityEngine.Object m_OriginalSelection;
    private EditorCache m_EditorCache;
    private GUIView m_DelegateView;
    private List<int> m_AllowedIDs;
    private ObjectListAreaState m_ListAreaState;
    private ObjectListArea m_ListArea;
    private float m_PreviewSize;
    private float m_TopSize;
    private static ObjectSelector s_SharedObjectSelector;

    private Rect listPosition
    {
      get
      {
        return new Rect(0.0f, this.m_ToolbarHeight, this.position.width, Mathf.Max(0.0f, this.m_TopSize - this.m_ToolbarHeight));
      }
    }

    public List<int> allowedInstanceIDs
    {
      get
      {
        return this.m_AllowedIDs;
      }
    }

    public static ObjectSelector get
    {
      get
      {
        if ((UnityEngine.Object) ObjectSelector.s_SharedObjectSelector == (UnityEngine.Object) null)
        {
          UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (ObjectSelector));
          if (objectsOfTypeAll != null && objectsOfTypeAll.Length > 0)
            ObjectSelector.s_SharedObjectSelector = (ObjectSelector) objectsOfTypeAll[0];
          if ((UnityEngine.Object) ObjectSelector.s_SharedObjectSelector == (UnityEngine.Object) null)
            ObjectSelector.s_SharedObjectSelector = ScriptableObject.CreateInstance<ObjectSelector>();
        }
        return ObjectSelector.s_SharedObjectSelector;
      }
    }

    public static bool isVisible
    {
      get
      {
        return (UnityEngine.Object) ObjectSelector.s_SharedObjectSelector != (UnityEngine.Object) null;
      }
    }

    internal string searchFilter
    {
      get
      {
        return this.m_SearchFilter;
      }
      set
      {
        this.m_SearchFilter = value;
        this.FilterSettingsChanged();
      }
    }

    private ObjectSelector()
    {
      this.hideFlags = HideFlags.DontSave;
    }

    private bool IsUsingTreeView()
    {
      return this.m_ObjectTreeWithSearch.IsInitialized();
    }

    private int GetSelectedInstanceID()
    {
      int[] numArray = !this.IsUsingTreeView() ? this.m_ListArea.GetSelection() : this.m_ObjectTreeWithSearch.GetSelection();
      if (numArray.Length >= 1)
        return numArray[0];
      return 0;
    }

    private void OnEnable()
    {
      this.m_ShowOverlapPreview.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_ShowOverlapPreview.speed = 1.5f;
      this.m_ShowWidePreview.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_ShowWidePreview.speed = 1.5f;
      this.m_PreviewResizer.Init("ObjectPickerPreview");
      this.m_PreviewSize = this.m_PreviewResizer.GetPreviewSize();
      AssetPreview.ClearTemporaryAssetPreviews();
      this.SetupPreview();
    }

    private void OnDisable()
    {
      this.SendEvent("ObjectSelectorClosed", false);
      if (this.m_ListArea != null)
        this.m_StartGridSize.value = this.m_ListArea.gridSize;
      Undo.CollapseUndoOperations(this.m_ModalUndoGroup);
      if ((UnityEngine.Object) ObjectSelector.s_SharedObjectSelector == (UnityEngine.Object) this)
        ObjectSelector.s_SharedObjectSelector = (ObjectSelector) null;
      if (this.m_EditorCache != null)
        this.m_EditorCache.Dispose();
      AssetPreview.ClearTemporaryAssetPreviews();
    }

    public void SetupPreview()
    {
      bool flag1 = this.PreviewIsOpen();
      bool flag2 = this.PreviewIsWide();
      AnimBool showOverlapPreview = this.m_ShowOverlapPreview;
      bool flag3 = flag1 && !flag2;
      this.m_ShowOverlapPreview.value = flag3;
      int num1 = flag3 ? 1 : 0;
      showOverlapPreview.target = num1 != 0;
      AnimBool showWidePreview = this.m_ShowWidePreview;
      bool flag4 = flag1 && flag2;
      this.m_ShowWidePreview.value = flag4;
      int num2 = flag4 ? 1 : 0;
      showWidePreview.target = num2 != 0;
    }

    private void ListAreaItemSelectedCallback(bool doubleClicked)
    {
      if (doubleClicked)
      {
        this.ItemWasDoubleClicked();
      }
      else
      {
        this.m_FocusSearchFilter = false;
        this.SendEvent("ObjectSelectorUpdated", true);
      }
    }

    private static bool GuessIfUserIsLookingForAnAsset(string requiredClassName, bool checkGameObject)
    {
      string[] strArray = new string[23]
      {
        "AnimationClip",
        "AnimatorController",
        "AnimatorOverrideController",
        "AudioClip",
        "Avatar",
        "Flare",
        "Font",
        "Material",
        "ProceduralMaterial",
        "Mesh",
        "PhysicMaterial",
        "GUISkin",
        "Shader",
        "TerrainData",
        "Texture",
        "Cubemap",
        "MovieTexture",
        "RenderTexture",
        "Texture2D",
        "ProceduralTexture",
        "Sprite",
        "AudioMixerGroup",
        "AudioMixerSnapshot"
      };
      if (checkGameObject && requiredClassName == "GameObject")
        return true;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] == requiredClassName)
          return true;
      }
      return false;
    }

    private void FilterSettingsChanged()
    {
      SearchFilter searchFilter = new SearchFilter();
      searchFilter.SearchFieldStringToFilter(this.m_SearchFilter);
      if (!string.IsNullOrEmpty(this.m_RequiredType))
        searchFilter.classNames = new string[1]
        {
          this.m_RequiredType
        };
      this.m_ListArea.Init(this.listPosition, !this.m_IsShowingAssets ? HierarchyType.GameObjects : HierarchyType.Assets, searchFilter, true);
    }

    private static bool ShouldTreeViewBeUsed(string className)
    {
      return className == "AudioMixerGroup";
    }

    public void Show(UnityEngine.Object obj, System.Type requiredType, SerializedProperty property, bool allowSceneObjects)
    {
      this.Show(obj, requiredType, property, allowSceneObjects, (List<int>) null);
    }

    internal void Show(UnityEngine.Object obj, System.Type requiredType, SerializedProperty property, bool allowSceneObjects, List<int> allowedInstanceIDs)
    {
      this.m_AllowSceneObjects = allowSceneObjects;
      this.m_IsShowingAssets = true;
      this.m_AllowedIDs = allowedInstanceIDs;
      string str = string.Empty;
      if (property != null)
      {
        str = property.objectReferenceTypeString;
        obj = property.objectReferenceValue;
        UnityEngine.Object targetObject = property.serializedObject.targetObject;
        if (targetObject != (UnityEngine.Object) null && EditorUtility.IsPersistent(targetObject))
          this.m_AllowSceneObjects = false;
      }
      else if (requiredType != null)
        str = requiredType.Name;
      if (this.m_AllowSceneObjects)
      {
        if (obj != (UnityEngine.Object) null)
        {
          if (typeof (Component).IsAssignableFrom(obj.GetType()))
            obj = (UnityEngine.Object) ((Component) obj).gameObject;
          this.m_IsShowingAssets = EditorUtility.IsPersistent(obj) || ObjectSelector.GuessIfUserIsLookingForAnAsset(str, false);
        }
        else
          this.m_IsShowingAssets = ObjectSelector.GuessIfUserIsLookingForAnAsset(str, true);
      }
      else
        this.m_IsShowingAssets = true;
      this.m_DelegateView = GUIView.current;
      this.m_RequiredType = str;
      this.m_SearchFilter = string.Empty;
      this.m_OriginalSelection = obj;
      this.m_ModalUndoGroup = Undo.GetCurrentGroup();
      ContainerWindow.SetFreezeDisplay(true);
      this.ShowWithMode(ShowMode.AuxWindow);
      this.titleContent = new GUIContent("Select " + str);
      Rect position = this.m_Parent.window.position;
      position.width = EditorPrefs.GetFloat("ObjectSelectorWidth", 200f);
      position.height = EditorPrefs.GetFloat("ObjectSelectorHeight", 390f);
      this.position = position;
      this.minSize = new Vector2(200f, 335f);
      this.maxSize = new Vector2(10000f, 10000f);
      this.SetupPreview();
      this.Focus();
      ContainerWindow.SetFreezeDisplay(false);
      this.m_FocusSearchFilter = true;
      this.m_Parent.AddToAuxWindowList();
      int num = !(obj != (UnityEngine.Object) null) ? 0 : obj.GetInstanceID();
      if (property != null && property.hasMultipleDifferentValues)
        num = 0;
      if (ObjectSelector.ShouldTreeViewBeUsed(str))
      {
        this.m_ObjectTreeWithSearch.Init(this.position, (EditorWindow) this, new UnityAction<ObjectTreeForSelector.TreeSelectorData>(this.CreateAndSetTreeView), new UnityAction<TreeViewItem>(this.TreeViewSelection), new UnityAction(this.ItemWasDoubleClicked), num, 0);
      }
      else
      {
        this.InitIfNeeded();
        this.m_ListArea.InitSelection(new int[1]{ num });
        if (num == 0)
          return;
        this.m_ListArea.Frame(num, true, false);
      }
    }

    private void ItemWasDoubleClicked()
    {
      this.Close();
      GUIUtility.ExitGUI();
    }

    private void CreateAndSetTreeView(ObjectTreeForSelector.TreeSelectorData data)
    {
      TreeViewForAudioMixerGroup.CreateAndSetTreeView(data);
    }

    private void TreeViewSelection(TreeViewItem item)
    {
      this.SendEvent("ObjectSelectorUpdated", true);
    }

    private void InitIfNeeded()
    {
      if (this.m_ListAreaState == null)
        this.m_ListAreaState = new ObjectListAreaState();
      if (this.m_ListArea != null)
        return;
      this.m_ListArea = new ObjectListArea(this.m_ListAreaState, (EditorWindow) this, true);
      this.m_ListArea.allowDeselection = false;
      this.m_ListArea.allowDragging = false;
      this.m_ListArea.allowFocusRendering = false;
      this.m_ListArea.allowMultiSelect = false;
      this.m_ListArea.allowRenaming = false;
      this.m_ListArea.allowBuiltinResources = true;
      this.m_ListArea.repaintCallback += new System.Action(((EditorWindow) this).Repaint);
      this.m_ListArea.itemSelectedCallback += new System.Action<bool>(this.ListAreaItemSelectedCallback);
      this.m_ListArea.gridSize = this.m_StartGridSize.value;
      SearchFilter searchFilter = new SearchFilter();
      searchFilter.nameFilter = this.m_SearchFilter;
      if (!string.IsNullOrEmpty(this.m_RequiredType))
        searchFilter.classNames = new string[1]
        {
          this.m_RequiredType
        };
      this.m_ListArea.Init(this.listPosition, !this.m_IsShowingAssets ? HierarchyType.GameObjects : HierarchyType.Assets, searchFilter, true);
    }

    public static UnityEngine.Object GetCurrentObject()
    {
      return EditorUtility.InstanceIDToObject(ObjectSelector.get.GetSelectedInstanceID());
    }

    public static UnityEngine.Object GetInitialObject()
    {
      return ObjectSelector.get.m_OriginalSelection;
    }

    private void SearchArea()
    {
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, this.m_ToolbarHeight), GUIContent.none, this.m_Styles.toolbarBack);
      bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape;
      GUI.SetNextControlName("SearchFilter");
      string str = EditorGUI.SearchField(new Rect(5f, 5f, this.position.width - 10f, 15f), this.m_SearchFilter);
      if (flag && Event.current.type == EventType.Used)
      {
        if (this.m_SearchFilter == string.Empty)
          this.Cancel();
        this.m_FocusSearchFilter = true;
      }
      if (str != this.m_SearchFilter || this.m_FocusSearchFilter)
      {
        this.m_SearchFilter = str;
        this.FilterSettingsChanged();
        this.Repaint();
      }
      if (this.m_FocusSearchFilter)
      {
        EditorGUI.FocusTextInControl("SearchFilter");
        this.m_FocusSearchFilter = false;
      }
      GUI.changed = false;
      GUILayout.BeginArea(new Rect(0.0f, 26f, this.position.width, this.m_ToolbarHeight - 26f));
      GUILayout.BeginHorizontal();
      if (!this.m_IsShowingAssets && GUILayout.Toggle(this.m_IsShowingAssets, "Assets", this.m_Styles.tab, new GUILayoutOption[0]))
        this.m_IsShowingAssets = true;
      if (!this.m_AllowSceneObjects)
      {
        GUI.enabled = false;
        GUI.color = new Color(1f, 1f, 1f, 0.0f);
      }
      if (this.m_IsShowingAssets && GUILayout.Toggle(!this.m_IsShowingAssets, "Scene", this.m_Styles.tab, new GUILayoutOption[0]))
        this.m_IsShowingAssets = false;
      if (!this.m_AllowSceneObjects)
      {
        GUI.color = new Color(1f, 1f, 1f, 1f);
        GUI.enabled = true;
      }
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
      if (GUI.changed)
        this.FilterSettingsChanged();
      if (!this.m_ListArea.CanShowThumbnails())
        return;
      this.m_ListArea.gridSize = (int) GUI.HorizontalSlider(new Rect(this.position.width - 60f, 26f, 55f, this.m_ToolbarHeight - 28f), (float) this.m_ListArea.gridSize, (float) this.m_ListArea.minGridSize, (float) this.m_ListArea.maxGridSize);
    }

    private void OnInspectorUpdate()
    {
      if (this.m_ListArea == null || !AssetPreview.HasAnyNewPreviewTexturesAvailable(this.m_ListArea.GetAssetPreviewManagerID()))
        return;
      this.Repaint();
    }

    private void PreviewArea()
    {
      GUI.Box(new Rect(0.0f, this.m_TopSize, this.position.width, this.m_PreviewSize), string.Empty, this.m_Styles.previewBackground);
      if (this.m_ListArea.GetSelection().Length == 0)
        return;
      EditorWrapper p = (EditorWrapper) null;
      UnityEngine.Object currentObject = ObjectSelector.GetCurrentObject();
      if ((double) this.m_PreviewSize < 75.0)
      {
        string s;
        if (currentObject != (UnityEngine.Object) null)
        {
          p = this.m_EditorCache[currentObject];
          string str = ObjectNames.NicifyVariableName(currentObject.GetType().Name);
          s = (p == null ? currentObject.name + " (" + str + ")" : p.name + " (" + str + ")") + "      " + AssetDatabase.GetAssetPath(currentObject);
        }
        else
          s = "None";
        this.LinePreview(s, currentObject, p);
      }
      else
      {
        if (this.m_EditorCache == null)
          this.m_EditorCache = new EditorCache(EditorFeatures.PreviewGUI);
        string s;
        if (currentObject != (UnityEngine.Object) null)
        {
          p = this.m_EditorCache[currentObject];
          string str1 = ObjectNames.NicifyVariableName(currentObject.GetType().Name);
          string str2;
          if (p != null)
          {
            string infoString = p.GetInfoString();
            if (infoString != string.Empty)
              str2 = p.name + "\n" + str1 + "\n" + infoString;
            else
              str2 = p.name + "\n" + str1;
          }
          else
            str2 = currentObject.name + "\n" + str1;
          s = str2 + "\n" + AssetDatabase.GetAssetPath(currentObject);
        }
        else
          s = "None";
        if ((double) this.m_ShowWidePreview.faded != 0.0)
        {
          GUI.color = new Color(1f, 1f, 1f, this.m_ShowWidePreview.faded);
          this.WidePreview(this.m_PreviewSize, s, currentObject, p);
        }
        if ((double) this.m_ShowOverlapPreview.faded != 0.0)
        {
          GUI.color = new Color(1f, 1f, 1f, this.m_ShowOverlapPreview.faded);
          this.OverlapPreview(this.m_PreviewSize, s, currentObject, p);
        }
        GUI.color = Color.white;
        this.m_EditorCache.CleanupUntouchedEditors();
      }
    }

    private void WidePreview(float actualSize, string s, UnityEngine.Object o, EditorWrapper p)
    {
      float x = 5f;
      Rect position1 = new Rect(x, this.m_TopSize + x, actualSize - x * 2f, actualSize - x * 2f);
      Rect position2 = new Rect(this.m_PreviewSize + 3f, this.m_TopSize + (float) (((double) this.m_PreviewSize - 75.0) * 0.5), (float) ((double) this.m_Parent.window.position.width - (double) this.m_PreviewSize - 3.0) - x, 75f);
      if (p != null && p.HasPreviewGUI())
        p.OnPreviewGUI(position1, this.m_Styles.previewTextureBackground);
      else if (o != (UnityEngine.Object) null)
        this.DrawObjectIcon(position1, this.m_ListArea.m_SelectedObjectIcon);
      if (EditorGUIUtility.isProSkin)
        EditorGUI.DropShadowLabel(position2, s, this.m_Styles.smallStatus);
      else
        GUI.Label(position2, s, this.m_Styles.smallStatus);
    }

    private void OverlapPreview(float actualSize, string s, UnityEngine.Object o, EditorWrapper p)
    {
      float x = 5f;
      Rect position = new Rect(x, this.m_TopSize + x, this.position.width - x * 2f, actualSize - x * 2f);
      if (p != null && p.HasPreviewGUI())
        p.OnPreviewGUI(position, this.m_Styles.previewTextureBackground);
      else if (o != (UnityEngine.Object) null)
        this.DrawObjectIcon(position, this.m_ListArea.m_SelectedObjectIcon);
      if (EditorGUIUtility.isProSkin)
        EditorGUI.DropShadowLabel(position, s, this.m_Styles.largeStatus);
      else
        EditorGUI.DoDropShadowLabel(position, EditorGUIUtility.TempContent(s), this.m_Styles.largeStatus, 0.3f);
    }

    private void LinePreview(string s, UnityEngine.Object o, EditorWrapper p)
    {
      if ((UnityEngine.Object) this.m_ListArea.m_SelectedObjectIcon != (UnityEngine.Object) null)
        GUI.DrawTexture(new Rect(2f, (float) (int) ((double) this.m_TopSize + 2.0), 16f, 16f), this.m_ListArea.m_SelectedObjectIcon, ScaleMode.StretchToFill);
      Rect position = new Rect(20f, this.m_TopSize + 1f, this.position.width - 22f, 18f);
      if (EditorGUIUtility.isProSkin)
        EditorGUI.DropShadowLabel(position, s, this.m_Styles.smallStatus);
      else
        GUI.Label(position, s, this.m_Styles.smallStatus);
    }

    private void DrawObjectIcon(Rect position, Texture icon)
    {
      if ((UnityEngine.Object) icon == (UnityEngine.Object) null)
        return;
      int num = Mathf.Min((int) position.width, (int) position.height);
      if (num >= icon.width * 2)
        num = icon.width * 2;
      UnityEngine.FilterMode filterMode = icon.filterMode;
      icon.filterMode = UnityEngine.FilterMode.Point;
      GUI.DrawTexture(new Rect(position.x + (float) (((int) position.width - num) / 2), position.y + (float) (((int) position.height - num) / 2), (float) num, (float) num), icon, ScaleMode.ScaleToFit);
      icon.filterMode = filterMode;
    }

    private void ResizeBottomPartOfWindow()
    {
      GUI.changed = false;
      this.m_PreviewSize = this.m_PreviewResizer.ResizeHandle(this.position, 65f, 270f, 20f) + 20f;
      this.m_TopSize = this.position.height - this.m_PreviewSize;
      bool flag1 = this.PreviewIsOpen();
      bool flag2 = this.PreviewIsWide();
      this.m_ShowOverlapPreview.target = flag1 && !flag2;
      this.m_ShowWidePreview.target = flag1 && flag2;
    }

    private bool PreviewIsOpen()
    {
      return (double) this.m_PreviewSize >= 37.0;
    }

    private bool PreviewIsWide()
    {
      return (double) this.position.width - (double) this.m_PreviewSize - 5.0 > (double) Mathf.Min((float) ((double) this.m_PreviewSize * 2.0 - 20.0), 256f);
    }

    private void SendEvent(string eventName, bool exitGUI)
    {
      if (!(bool) ((UnityEngine.Object) this.m_DelegateView))
        return;
      Event e = EditorGUIUtility.CommandEvent(eventName);
      try
      {
        this.m_DelegateView.SendEvent(e);
      }
      finally
      {
      }
      if (!exitGUI)
        return;
      GUIUtility.ExitGUI();
    }

    private void HandleKeyboard()
    {
      if (Event.current.type != EventType.KeyDown)
        return;
      switch (Event.current.keyCode)
      {
        case KeyCode.Return:
        case KeyCode.KeypadEnter:
          this.Close();
          GUI.changed = true;
          GUIUtility.ExitGUI();
          Event.current.Use();
          GUI.changed = true;
          break;
      }
    }

    private void Cancel()
    {
      Undo.RevertAllDownToGroup(this.m_ModalUndoGroup);
      this.Close();
      GUI.changed = true;
      GUIUtility.ExitGUI();
    }

    private void OnDestroy()
    {
      if (this.m_ListArea != null)
        this.m_ListArea.OnDestroy();
      this.m_ObjectTreeWithSearch.Clear();
    }

    private void OnGUI()
    {
      this.HandleKeyboard();
      if (this.m_ObjectTreeWithSearch.IsInitialized())
        this.OnObjectTreeGUI();
      else
        this.OnObjectGridGUI();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.Cancel();
    }

    private void OnObjectTreeGUI()
    {
      this.m_ObjectTreeWithSearch.OnGUI(new Rect(0.0f, 0.0f, this.position.width, this.position.height));
    }

    private void OnObjectGridGUI()
    {
      this.InitIfNeeded();
      if (this.m_Styles == null)
        this.m_Styles = new ObjectSelector.Styles();
      if (this.m_EditorCache == null)
        this.m_EditorCache = new EditorCache(EditorFeatures.PreviewGUI);
      this.ResizeBottomPartOfWindow();
      Rect position = this.position;
      EditorPrefs.SetFloat("ObjectSelectorWidth", position.width);
      EditorPrefs.SetFloat("ObjectSelectorHeight", position.height);
      GUI.BeginGroup(new Rect(0.0f, 0.0f, this.position.width, this.position.height), GUIContent.none);
      this.m_ListArea.HandleKeyboard(false);
      this.SearchArea();
      this.GridListArea();
      this.PreviewArea();
      GUI.EndGroup();
      GUI.Label(new Rect((float) ((double) this.position.width * 0.5 - 16.0), (float) ((double) this.position.height - (double) this.m_PreviewSize + 2.0), 32f, this.m_Styles.bottomResize.fixedHeight), GUIContent.none, this.m_Styles.bottomResize);
    }

    private void GridListArea()
    {
      this.m_ListArea.OnGUI(this.listPosition, GUIUtility.GetControlID(FocusType.Keyboard));
    }

    private class Styles
    {
      public GUIStyle smallStatus = (GUIStyle) "ObjectPickerSmallStatus";
      public GUIStyle largeStatus = (GUIStyle) "ObjectPickerLargeStatus";
      public GUIStyle toolbarBack = (GUIStyle) "ObjectPickerToolbar";
      public GUIStyle tab = (GUIStyle) "ObjectPickerTab";
      public GUIStyle bottomResize = (GUIStyle) "WindowBottomResize";
      public GUIStyle background = (GUIStyle) "ObjectPickerBackground";
      public GUIStyle previewBackground = (GUIStyle) "PopupCurveSwatchBackground";
      public GUIStyle previewTextureBackground = (GUIStyle) "ObjectPickerPreviewBackground";
    }
  }
}
