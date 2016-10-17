// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Audio;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(icon = "Audio Mixer", title = "Audio Mixer")]
  internal class AudioMixerWindow : EditorWindow, IHasCustomMenu
  {
    private static string kAudioMixerUseRMSMetering = "AudioMixerUseRMSMetering";
    [SerializeField]
    private AudioMixerWindow.SectionType[] m_SectionOrder = new AudioMixerWindow.SectionType[4]{ AudioMixerWindow.SectionType.MixerTree, AudioMixerWindow.SectionType.SnapshotList, AudioMixerWindow.SectionType.GroupTree, AudioMixerWindow.SectionType.ViewList };
    [SerializeField]
    private AudioMixerWindow.LayoutMode m_LayoutMode = AudioMixerWindow.LayoutMode.Vertical;
    [SerializeField]
    private bool m_ShowReferencedBuses = true;
    private Vector2 m_SectionsScrollPosition = Vector2.zero;
    private int m_RepaintCounter = 2;
    private bool m_GroupsRenderedAboveSections = true;
    private readonly TickTimerHelper m_Ticker = new TickTimerHelper(0.05);
    private const float kToolbarHeight = 17f;
    private static AudioMixerWindow s_Instance;
    [NonSerialized]
    private bool m_Initialized;
    private AudioMixerController m_Controller;
    private List<AudioMixerController> m_AllControllers;
    private AudioMixerChannelStripView.State m_ChannelStripViewState;
    private AudioMixerChannelStripView m_ChannelStripView;
    private TreeViewState m_AudioGroupTreeState;
    private AudioMixerGroupTreeView m_GroupTree;
    [SerializeField]
    private TreeViewState m_MixersTreeState;
    private AudioMixersTreeView m_MixersTree;
    private ReorderableListWithRenameAndScrollView.State m_ViewsState;
    private AudioMixerGroupViewList m_GroupViews;
    private ReorderableListWithRenameAndScrollView.State m_SnapshotState;
    private AudioMixerSnapshotListView m_SnapshotListView;
    [SerializeField]
    private AudioMixerWindow.Layout m_LayoutStripsOnTop;
    [SerializeField]
    private AudioMixerWindow.Layout m_LayoutStripsOnRight;
    [SerializeField]
    private bool m_SortGroupsAlphabetically;
    [SerializeField]
    private bool m_ShowBusConnections;
    [SerializeField]
    private bool m_ShowBusConnectionsOfSelection;
    private Vector2 m_LastSize;
    [NonSerialized]
    private bool m_ShowDeveloperOverlays;
    private static AudioMixerWindow.GUIContents s_GuiContents;

    public AudioMixerController controller
    {
      get
      {
        return this.m_Controller;
      }
    }

    private AudioMixerWindow.LayoutMode layoutMode
    {
      get
      {
        return this.m_LayoutMode;
      }
      set
      {
        this.m_LayoutMode = value;
        this.m_RepaintCounter = 2;
      }
    }

    private void UpdateAfterAssetChange()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_Controller.SanitizeGroupViews();
      this.m_Controller.OnUnitySelectionChanged();
      if (this.m_GroupTree != null)
        this.m_GroupTree.ReloadTreeData();
      if (this.m_GroupViews != null)
        this.m_GroupViews.RecreateListControl();
      if (this.m_SnapshotListView != null)
        this.m_SnapshotListView.LoadFromBackend();
      if (this.m_MixersTree != null)
        this.m_MixersTree.ReloadTree();
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public static void Create()
    {
      AudioMixerWindow window = EditorWindow.GetWindow<AudioMixerWindow>(new System.Type[1]{ typeof (ProjectBrowser) });
      if ((double) window.m_Pos.width >= 400.0)
        return;
      window.m_Pos = new Rect(window.m_Pos.x, window.m_Pos.y, 800f, 450f);
    }

    public static void RepaintAudioMixerWindow()
    {
      if (!((UnityEngine.Object) AudioMixerWindow.s_Instance != (UnityEngine.Object) null))
        return;
      AudioMixerWindow.s_Instance.Repaint();
    }

    private void Init()
    {
      if (this.m_Initialized)
        return;
      if (this.m_LayoutStripsOnTop == null)
        this.m_LayoutStripsOnTop = new AudioMixerWindow.Layout();
      if (this.m_LayoutStripsOnTop.m_VerticalSplitter == null || this.m_LayoutStripsOnTop.m_VerticalSplitter.realSizes.Length != 2)
        this.m_LayoutStripsOnTop.m_VerticalSplitter = new SplitterState(new int[2]{ 65, 35 }, new int[2]{ 85, 105 }, (int[]) null);
      if (this.m_LayoutStripsOnTop.m_HorizontalSplitter == null || this.m_LayoutStripsOnTop.m_HorizontalSplitter.realSizes.Length != 4)
        this.m_LayoutStripsOnTop.m_HorizontalSplitter = new SplitterState(new int[4]{ 60, 60, 60, 60 }, new int[4]{ 85, 85, 85, 85 }, (int[]) null);
      if (this.m_LayoutStripsOnRight == null)
        this.m_LayoutStripsOnRight = new AudioMixerWindow.Layout();
      if (this.m_LayoutStripsOnRight.m_HorizontalSplitter == null || this.m_LayoutStripsOnRight.m_HorizontalSplitter.realSizes.Length != 2)
        this.m_LayoutStripsOnRight.m_HorizontalSplitter = new SplitterState(new int[2]{ 30, 70 }, new int[2]{ 160, 160 }, (int[]) null);
      if (this.m_LayoutStripsOnRight.m_VerticalSplitter == null || this.m_LayoutStripsOnRight.m_VerticalSplitter.realSizes.Length != 4)
        this.m_LayoutStripsOnRight.m_VerticalSplitter = new SplitterState(new int[4]{ 60, 60, 60, 60 }, new int[4]{ 100, 85, 85, 85 }, (int[]) null);
      if (this.m_AudioGroupTreeState == null)
        this.m_AudioGroupTreeState = new TreeViewState();
      this.m_GroupTree = new AudioMixerGroupTreeView(this, this.m_AudioGroupTreeState);
      if (this.m_MixersTreeState == null)
        this.m_MixersTreeState = new TreeViewState();
      this.m_MixersTree = new AudioMixersTreeView(this, this.m_MixersTreeState, new Func<List<AudioMixerController>>(this.GetAllControllers));
      if (this.m_ViewsState == null)
        this.m_ViewsState = new ReorderableListWithRenameAndScrollView.State();
      this.m_GroupViews = new AudioMixerGroupViewList(this.m_ViewsState);
      if (this.m_SnapshotState == null)
        this.m_SnapshotState = new ReorderableListWithRenameAndScrollView.State();
      this.m_SnapshotListView = new AudioMixerSnapshotListView(this.m_SnapshotState);
      if (this.m_ChannelStripViewState == null)
        this.m_ChannelStripViewState = new AudioMixerChannelStripView.State();
      this.m_ChannelStripView = new AudioMixerChannelStripView(this.m_ChannelStripViewState);
      this.OnMixerControllerChanged();
      this.m_Initialized = true;
    }

    private List<AudioMixerController> GetAllControllers()
    {
      return this.m_AllControllers;
    }

    private static List<AudioMixerController> FindAllAudioMixerControllers()
    {
      List<AudioMixerController> audioMixerControllerList = new List<AudioMixerController>();
      HierarchyProperty hierarchyProperty = new HierarchyProperty(HierarchyType.Assets);
      hierarchyProperty.SetSearchFilter(new SearchFilter()
      {
        classNames = new string[1]
        {
          "AudioMixerController"
        }
      });
      while (hierarchyProperty.Next((int[]) null))
      {
        AudioMixerController pptrValue = hierarchyProperty.pptrValue as AudioMixerController;
        if ((bool) ((UnityEngine.Object) pptrValue))
          audioMixerControllerList.Add(pptrValue);
      }
      return audioMixerControllerList;
    }

    public void Awake()
    {
      this.m_AllControllers = AudioMixerWindow.FindAllAudioMixerControllers();
      if (this.m_MixersTreeState == null)
        return;
      this.m_MixersTreeState.OnAwake();
      this.m_MixersTreeState.selectedIDs = new List<int>();
    }

    public void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      AudioMixerWindow.s_Instance = this;
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      EditorApplication.playmodeStateChanged += new EditorApplication.CallbackFunction(this.PlaymodeChanged);
      EditorApplication.projectWindowChanged += new EditorApplication.CallbackFunction(this.OnProjectChanged);
    }

    public void OnDisable()
    {
      EditorApplication.playmodeStateChanged -= new EditorApplication.CallbackFunction(this.PlaymodeChanged);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      EditorApplication.projectWindowChanged -= new EditorApplication.CallbackFunction(this.OnProjectChanged);
    }

    private void PlaymodeChanged()
    {
      this.m_Ticker.Reset();
      if ((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null)
        this.Repaint();
      this.EndRenaming();
    }

    private void OnLostFocus()
    {
      this.EndRenaming();
    }

    private void EndRenaming()
    {
      if (this.m_GroupTree != null)
        this.m_GroupTree.EndRenaming();
      if (this.m_MixersTree == null)
        return;
      this.m_MixersTree.EndRenaming();
    }

    public void UndoRedoPerformed()
    {
      if ((UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null)
        return;
      this.m_Controller.SanitizeGroupViews();
      this.m_Controller.OnUnitySelectionChanged();
      this.m_Controller.OnSubAssetChanged();
      if (this.m_GroupTree != null)
        this.m_GroupTree.OnUndoRedoPerformed();
      if (this.m_GroupViews != null)
        this.m_GroupViews.OnUndoRedoPerformed();
      if (this.m_SnapshotListView != null)
        this.m_SnapshotListView.OnUndoRedoPerformed();
      if (this.m_MixersTree != null)
        this.m_MixersTree.OnUndoRedoPerformed();
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    private void OnMixerControllerChanged()
    {
      if ((bool) ((UnityEngine.Object) this.m_Controller))
        this.m_Controller.ClearEventHandlers();
      this.m_MixersTree.OnMixerControllerChanged(this.m_Controller);
      this.m_GroupTree.OnMixerControllerChanged(this.m_Controller);
      this.m_GroupViews.OnMixerControllerChanged(this.m_Controller);
      this.m_ChannelStripView.OnMixerControllerChanged(this.m_Controller);
      this.m_SnapshotListView.OnMixerControllerChanged(this.m_Controller);
      if (!(bool) ((UnityEngine.Object) this.m_Controller))
        return;
      this.m_Controller.ForceSetView(this.m_Controller.currentViewIndex);
    }

    private void OnProjectChanged()
    {
      if (this.m_MixersTree == null)
        this.Init();
      this.m_AllControllers = AudioMixerWindow.FindAllAudioMixerControllers();
      this.m_MixersTree.ReloadTree();
    }

    public void Update()
    {
      if (!this.m_Ticker.DoTick() || !EditorApplication.isPlaying && (this.m_ChannelStripView == null || !this.m_ChannelStripView.requiresRepaint))
        return;
      this.Repaint();
    }

    private void DetectControllerChange()
    {
      AudioMixerController controller = this.m_Controller;
      if (Selection.activeObject is AudioMixerController)
        this.m_Controller = Selection.activeObject as AudioMixerController;
      if (!((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) controller))
        return;
      this.OnMixerControllerChanged();
    }

    private void OnSelectionChange()
    {
      if ((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null)
        this.m_Controller.OnUnitySelectionChanged();
      if (this.m_GroupTree != null)
        this.m_GroupTree.InitSelection(true);
      this.Repaint();
    }

    private Dictionary<AudioMixerEffectController, AudioMixerGroupController> GetEffectMap(List<AudioMixerGroupController> allGroups)
    {
      Dictionary<AudioMixerEffectController, AudioMixerGroupController> dictionary = new Dictionary<AudioMixerEffectController, AudioMixerGroupController>();
      using (List<AudioMixerGroupController>.Enumerator enumerator = allGroups.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioMixerGroupController current = enumerator.Current;
          foreach (AudioMixerEffectController effect in current.effects)
            dictionary[effect] = current;
        }
      }
      return dictionary;
    }

    private void DoToolbar()
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(17f));
      GUILayout.FlexibleSpace();
      if ((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null)
      {
        if (Application.isPlaying)
        {
          Color backgroundColor = GUI.backgroundColor;
          if (AudioSettings.editingInPlaymode)
            GUI.backgroundColor = AnimationMode.animatedPropertyColor;
          EditorGUI.BeginChangeCheck();
          AudioSettings.editingInPlaymode = GUILayout.Toggle(AudioSettings.editingInPlaymode, AudioMixerWindow.s_GuiContents.editSnapShots, EditorStyles.toolbarButton, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
            InspectorWindow.RepaintAllInspectors();
          GUI.backgroundColor = backgroundColor;
        }
        GUILayout.FlexibleSpace();
        AudioMixerExposedParametersPopup.Popup(this.m_Controller, EditorStyles.toolbarPopup);
      }
      EditorGUILayout.EndHorizontal();
    }

    private void RepaintIfNeeded()
    {
      if (this.m_RepaintCounter <= 0)
        return;
      if (Event.current.type == EventType.Repaint)
        --this.m_RepaintCounter;
      this.Repaint();
    }

    public void OnGUI()
    {
      this.Init();
      if (AudioMixerWindow.s_GuiContents == null)
        AudioMixerWindow.s_GuiContents = new AudioMixerWindow.GUIContents();
      AudioMixerDrawUtils.InitStyles();
      this.DetectControllerChange();
      this.m_GroupViews.OnEvent();
      this.m_SnapshotListView.OnEvent();
      this.DoToolbar();
      List<AudioMixerGroupController> allGroups = !((UnityEngine.Object) this.m_Controller != (UnityEngine.Object) null) ? new List<AudioMixerGroupController>() : this.m_Controller.GetAllAudioGroupsSlow();
      Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap = this.GetEffectMap(allGroups);
      this.m_GroupTree.UseScrollView(this.m_LayoutMode == AudioMixerWindow.LayoutMode.Horizontal);
      if (this.m_LayoutMode == AudioMixerWindow.LayoutMode.Horizontal)
        this.LayoutWithStripsOnTop(allGroups, effectMap);
      else
        this.LayoutWithStripsOnRightSideOneScrollBar(allGroups, effectMap);
      if ((double) this.m_LastSize.x != (double) this.position.width || (double) this.m_LastSize.y != (double) this.position.height)
      {
        this.m_RepaintCounter = 2;
        this.m_LastSize = new Vector2(this.position.width, this.position.height);
      }
      this.RepaintIfNeeded();
    }

    private void LayoutWithStripsOnRightSideOneScrollBar(List<AudioMixerGroupController> allGroups, Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap)
    {
      SplitterState horizontalSplitter = this.m_LayoutStripsOnRight.m_HorizontalSplitter;
      SplitterGUILayout.BeginHorizontalSplit(horizontalSplitter, new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      SplitterGUILayout.EndHorizontalSplit();
      float realSize = (float) horizontalSplitter.realSizes[0];
      float width = this.position.width - realSize;
      Rect rect1 = new Rect(0.0f, 17f, realSize, this.position.height - 17f);
      Rect rect2 = new Rect(realSize, 17f, width, rect1.height);
      if (EditorGUIUtility.isProSkin)
        EditorGUI.DrawRect(rect1, !EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f, 0.0f) : new Color(0.19f, 0.19f, 0.19f));
      float num = 10f;
      Rect[] sectionRects = new Rect[this.m_SectionOrder.Length];
      float y = 0.0f;
      for (int index = 0; index < this.m_SectionOrder.Length; ++index)
      {
        y += num;
        if (index > 0)
          y += sectionRects[index - 1].height;
        sectionRects[index] = new Rect(0.0f, y, rect1.width, this.GetHeightOfSection(this.m_SectionOrder[index]));
        sectionRects[index].x += 4f;
        sectionRects[index].width -= 8f;
      }
      Rect viewRect = new Rect(0.0f, 0.0f, 1f, ((IEnumerable<Rect>) sectionRects).Last<Rect>().yMax);
      if ((double) viewRect.height > (double) rect1.height)
      {
        for (int index = 0; index < sectionRects.Length; ++index)
          sectionRects[index].width -= 14f;
      }
      this.m_SectionsScrollPosition = GUI.BeginScrollView(rect1, this.m_SectionsScrollPosition, viewRect);
      this.DoSections(rect1, sectionRects, this.m_SectionOrder);
      GUI.EndScrollView();
      this.m_ChannelStripView.OnGUI(rect2, this.m_ShowReferencedBuses, this.m_ShowBusConnections, this.m_ShowBusConnectionsOfSelection, allGroups, effectMap, this.m_SortGroupsAlphabetically, this.m_ShowDeveloperOverlays, this.m_GroupTree.ScrollToItem);
      EditorGUI.DrawRect(new Rect(rect1.xMax - 1f, 17f, 1f, this.position.height - 17f), !EditorGUIUtility.isProSkin ? new Color(0.6f, 0.6f, 0.6f) : new Color(0.15f, 0.15f, 0.15f));
    }

    private void LayoutWithStripsOnTop(List<AudioMixerGroupController> allGroups, Dictionary<AudioMixerEffectController, AudioMixerGroupController> effectMap)
    {
      SplitterState horizontalSplitter = this.m_LayoutStripsOnTop.m_HorizontalSplitter;
      SplitterState verticalSplitter = this.m_LayoutStripsOnTop.m_VerticalSplitter;
      SplitterGUILayout.BeginVerticalSplit(verticalSplitter, new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      if (this.m_GroupsRenderedAboveSections)
      {
        GUILayout.BeginVertical();
        GUILayout.EndVertical();
      }
      SplitterGUILayout.BeginHorizontalSplit(horizontalSplitter, new GUILayoutOption[2]
      {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true)
      });
      if (!this.m_GroupsRenderedAboveSections)
      {
        GUILayout.BeginVertical();
        GUILayout.EndVertical();
      }
      SplitterGUILayout.EndHorizontalSplit();
      SplitterGUILayout.EndVerticalSplit();
      float y1 = !this.m_GroupsRenderedAboveSections ? 17f + (float) verticalSplitter.realSizes[0] : 17f;
      float height = !this.m_GroupsRenderedAboveSections ? (float) verticalSplitter.realSizes[1] : (float) verticalSplitter.realSizes[0];
      float y2 = this.m_GroupsRenderedAboveSections ? 17f + (float) verticalSplitter.realSizes[0] : 17f;
      float num = this.m_GroupsRenderedAboveSections ? (float) verticalSplitter.realSizes[1] : (float) verticalSplitter.realSizes[0];
      Rect rect = new Rect(0.0f, y1, this.position.width, height);
      Rect totalRectOfSections = new Rect(0.0f, rect.yMax, this.position.width, this.position.height - rect.height);
      Rect[] sectionRects = new Rect[this.m_SectionOrder.Length];
      for (int index = 0; index < sectionRects.Length; ++index)
      {
        float x = index <= 0 ? 0.0f : sectionRects[index - 1].xMax;
        sectionRects[index] = new Rect(x, y2, (float) horizontalSplitter.realSizes[index], num - 12f);
      }
      sectionRects[0].x += 8f;
      sectionRects[0].width -= 12f;
      sectionRects[sectionRects.Length - 1].x += 4f;
      sectionRects[sectionRects.Length - 1].width -= 12f;
      for (int index = 1; index < sectionRects.Length - 1; ++index)
      {
        sectionRects[index].x += 4f;
        sectionRects[index].width -= 8f;
      }
      this.DoSections(totalRectOfSections, sectionRects, this.m_SectionOrder);
      this.m_ChannelStripView.OnGUI(rect, this.m_ShowReferencedBuses, this.m_ShowBusConnections, this.m_ShowBusConnectionsOfSelection, allGroups, effectMap, this.m_SortGroupsAlphabetically, this.m_ShowDeveloperOverlays, this.m_GroupTree.ScrollToItem);
      EditorGUI.DrawRect(new Rect(0.0f, (float) (17.0 + (double) verticalSplitter.realSizes[0] - 1.0), this.position.width, 1f), new Color(0.0f, 0.0f, 0.0f, 0.4f));
    }

    private float GetHeightOfSection(AudioMixerWindow.SectionType sectionType)
    {
      switch (sectionType)
      {
        case AudioMixerWindow.SectionType.MixerTree:
          return this.m_MixersTree.GetTotalHeight();
        case AudioMixerWindow.SectionType.GroupTree:
          return this.m_GroupTree.GetTotalHeight();
        case AudioMixerWindow.SectionType.ViewList:
          return this.m_GroupViews.GetTotalHeight();
        case AudioMixerWindow.SectionType.SnapshotList:
          return this.m_SnapshotListView.GetTotalHeight();
        default:
          Debug.LogError((object) "Unhandled enum value");
          return 0.0f;
      }
    }

    private void DoSections(Rect totalRectOfSections, Rect[] sectionRects, AudioMixerWindow.SectionType[] sectionOrder)
    {
      Event current = Event.current;
      bool flag = (UnityEngine.Object) this.m_Controller == (UnityEngine.Object) null || AudioMixerController.EditingTargetSnapshot();
      for (int sectionIndex = 0; sectionIndex < sectionOrder.Length; ++sectionIndex)
      {
        Rect sectionRect = sectionRects[sectionIndex];
        if ((double) sectionRect.height > 0.0)
        {
          switch (sectionOrder[sectionIndex])
          {
            case AudioMixerWindow.SectionType.MixerTree:
              this.m_MixersTree.OnGUI(sectionRect);
              break;
            case AudioMixerWindow.SectionType.GroupTree:
              this.m_GroupTree.OnGUI(sectionRect);
              break;
            case AudioMixerWindow.SectionType.ViewList:
              this.m_GroupViews.OnGUI(sectionRect);
              break;
            case AudioMixerWindow.SectionType.SnapshotList:
              EditorGUI.BeginDisabledGroup(!flag);
              this.m_SnapshotListView.OnGUI(sectionRect);
              EditorGUI.EndDisabledGroup();
              break;
            default:
              Debug.LogError((object) "Unhandled enum value");
              break;
          }
          if (current.type == EventType.ContextClick)
          {
            Rect rect = new Rect(sectionRect.x, sectionRect.y, sectionRect.width - 15f, 22f);
            if (rect.Contains(current.mousePosition))
            {
              this.ReorderContextMenu(rect, sectionIndex);
              current.Use();
            }
          }
        }
      }
    }

    private void ReorderContextMenu(Rect rect, int sectionIndex)
    {
      Event current = Event.current;
      if (Event.current.type != EventType.ContextClick || !rect.Contains(current.mousePosition))
        return;
      GUIContent content1 = new GUIContent(this.m_LayoutMode != AudioMixerWindow.LayoutMode.Horizontal ? "Move Up" : "Move Left");
      GUIContent content2 = new GUIContent(this.m_LayoutMode != AudioMixerWindow.LayoutMode.Horizontal ? "Move Down" : "Move Right");
      GenericMenu genericMenu = new GenericMenu();
      if (sectionIndex > 1)
        genericMenu.AddItem(content1, false, new GenericMenu.MenuFunction2(this.ChangeSectionOrder), (object) new Vector2((float) sectionIndex, -1f));
      else
        genericMenu.AddDisabledItem(content1);
      if (sectionIndex > 0 && sectionIndex < this.m_SectionOrder.Length - 1)
        genericMenu.AddItem(content2, false, new GenericMenu.MenuFunction2(this.ChangeSectionOrder), (object) new Vector2((float) sectionIndex, 1f));
      else
        genericMenu.AddDisabledItem(content2);
      genericMenu.ShowAsContext();
    }

    private void ChangeSectionOrder(object userData)
    {
      Vector2 vector2 = (Vector2) userData;
      int x = (int) vector2.x;
      int y = (int) vector2.y;
      int index = Mathf.Clamp(x + y, 0, this.m_SectionOrder.Length - 1);
      if (index == x)
        return;
      AudioMixerWindow.SectionType sectionType = this.m_SectionOrder[x];
      this.m_SectionOrder[x] = this.m_SectionOrder[index];
      this.m_SectionOrder[index] = sectionType;
    }

    public MixerParameterDefinition ParamDef(string name, string desc, string units, float displayScale, float minRange, float maxRange, float defaultValue)
    {
      return new MixerParameterDefinition() { name = name, description = desc, units = units, displayScale = displayScale, minRange = minRange, maxRange = maxRange, defaultValue = defaultValue };
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Sort groups alphabetically"), this.m_SortGroupsAlphabetically, (GenericMenu.MenuFunction) (() => this.m_SortGroupsAlphabetically = !this.m_SortGroupsAlphabetically));
      menu.AddItem(new GUIContent("Show referenced groups"), this.m_ShowReferencedBuses, (GenericMenu.MenuFunction) (() => this.m_ShowReferencedBuses = !this.m_ShowReferencedBuses));
      menu.AddItem(new GUIContent("Show group connections"), this.m_ShowBusConnections, (GenericMenu.MenuFunction) (() => this.m_ShowBusConnections = !this.m_ShowBusConnections));
      if (this.m_ShowBusConnections)
        menu.AddItem(new GUIContent("Only highlight selected group connections"), this.m_ShowBusConnectionsOfSelection, (GenericMenu.MenuFunction) (() => this.m_ShowBusConnectionsOfSelection = !this.m_ShowBusConnectionsOfSelection));
      menu.AddSeparator(string.Empty);
      menu.AddItem(new GUIContent("Vertical layout"), this.layoutMode == AudioMixerWindow.LayoutMode.Vertical, (GenericMenu.MenuFunction) (() => this.layoutMode = AudioMixerWindow.LayoutMode.Vertical));
      menu.AddItem(new GUIContent("Horizontal layout"), this.layoutMode == AudioMixerWindow.LayoutMode.Horizontal, (GenericMenu.MenuFunction) (() => this.layoutMode = AudioMixerWindow.LayoutMode.Horizontal));
      menu.AddSeparator(string.Empty);
      menu.AddItem(new GUIContent("Use RMS metering for display"), EditorPrefs.GetBool(AudioMixerWindow.kAudioMixerUseRMSMetering, true), (GenericMenu.MenuFunction) (() => EditorPrefs.SetBool(AudioMixerWindow.kAudioMixerUseRMSMetering, true)));
      menu.AddItem(new GUIContent("Use peak metering for display"), !EditorPrefs.GetBool(AudioMixerWindow.kAudioMixerUseRMSMetering, true), (GenericMenu.MenuFunction) (() => EditorPrefs.SetBool(AudioMixerWindow.kAudioMixerUseRMSMetering, false)));
      if (!Unsupported.IsDeveloperBuild())
        return;
      menu.AddSeparator(string.Empty);
      menu.AddItem(new GUIContent("DEVELOPER/Groups Rendered Above"), this.m_GroupsRenderedAboveSections, (GenericMenu.MenuFunction) (() => this.m_GroupsRenderedAboveSections = !this.m_GroupsRenderedAboveSections));
      menu.AddItem(new GUIContent("DEVELOPER/Build 10 groups"), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(0, 7, 10)));
      menu.AddItem(new GUIContent("DEVELOPER/Build 20 groups"), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(0, 7, 20)));
      menu.AddItem(new GUIContent("DEVELOPER/Build 40 groups"), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(0, 7, 40)));
      menu.AddItem(new GUIContent("DEVELOPER/Build 80 groups"), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(0, 7, 80)));
      menu.AddItem(new GUIContent("DEVELOPER/Build 160 groups"), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(0, 7, 160)));
      menu.AddItem(new GUIContent("DEVELOPER/Build chain of 10 groups"), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(1, 1, 10)));
      menu.AddItem(new GUIContent("DEVELOPER/Build chain of 20 groups "), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(1, 1, 20)));
      menu.AddItem(new GUIContent("DEVELOPER/Build chain of 40 groups"), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(1, 1, 40)));
      menu.AddItem(new GUIContent("DEVELOPER/Build chain of 80 groups"), false, (GenericMenu.MenuFunction) (() => this.m_Controller.BuildTestSetup(1, 1, 80)));
      menu.AddItem(new GUIContent("DEVELOPER/Show overlays"), this.m_ShowDeveloperOverlays, (GenericMenu.MenuFunction) (() => this.m_ShowDeveloperOverlays = !this.m_ShowDeveloperOverlays));
    }

    private enum SectionType
    {
      MixerTree,
      GroupTree,
      ViewList,
      SnapshotList,
    }

    public enum LayoutMode
    {
      Horizontal,
      Vertical,
    }

    [Serializable]
    private class Layout
    {
      [SerializeField]
      public SplitterState m_VerticalSplitter;
      [SerializeField]
      public SplitterState m_HorizontalSplitter;
    }

    private class GUIContents
    {
      public GUIStyle toolbarObjectField = new GUIStyle((GUIStyle) "ShurikenObjectField");
      public GUIStyle toolbarLabel = new GUIStyle(EditorStyles.miniLabel);
      public GUIStyle mixerHeader = new GUIStyle(EditorStyles.largeLabel);
      public GUIContent rms;
      public GUIContent editSnapShots;
      public GUIContent infoText;
      public GUIContent selectAudioMixer;
      public GUIContent output;

      public GUIContents()
      {
        this.rms = new GUIContent("RMS", "Switches between RMS (Root Mean Square) metering and peak metering. RMS is closer to the energy level and perceived loudness of the sound (hence lower than the peak meter), while peak-metering is useful for monitoring spikes in the signal that can cause clipping.");
        this.editSnapShots = new GUIContent("Edit in Play Mode", EditorGUIUtility.IconContent("Animation.Record", "|Are scene and inspector changes recorded into the animation curves?").image, "Edit in playmode and your changes are automatically saved. Note when editting is disabled then live values are shown.");
        this.infoText = new GUIContent("Create an AudioMixer asset from the Project Browser to get started");
        this.selectAudioMixer = new GUIContent(string.Empty, "Select an Audio Mixer");
        this.output = new GUIContent("Output", "Select an Audio Mixer Group from another Audio Mixer to output to. If 'None' is selected then output is routed directly to the Audio Listener.");
        this.toolbarLabel.alignment = TextAnchor.MiddleLeft;
        this.toolbarObjectField.normal.textColor = this.toolbarLabel.normal.textColor;
        this.mixerHeader.fontStyle = FontStyle.Bold;
        this.mixerHeader.fontSize = 17;
        this.mixerHeader.margin = new RectOffset();
        this.mixerHeader.padding = new RectOffset();
        this.mixerHeader.alignment = TextAnchor.MiddleLeft;
        if (!EditorGUIUtility.isProSkin)
          this.mixerHeader.normal.textColor = new Color(0.4f, 0.4f, 0.4f, 1f);
        else
          this.mixerHeader.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1f);
      }
    }

    private class AudioMixerPostprocessor : AssetPostprocessor
    {
      private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
      {
        if (!((UnityEngine.Object) AudioMixerWindow.s_Instance != (UnityEngine.Object) null) || !(((IEnumerable<string>) importedAssets).Any<string>((Func<string, bool>) (val => val.EndsWith(".mixer"))) | ((IEnumerable<string>) deletedAssets).Any<string>((Func<string, bool>) (val => val.EndsWith(".mixer"))) | ((IEnumerable<string>) movedAssets).Any<string>((Func<string, bool>) (val => val.EndsWith(".mixer"))) | ((IEnumerable<string>) movedFromPath).Any<string>((Func<string, bool>) (val => val.EndsWith(".mixer")))))
          return;
        AudioMixerWindow.s_Instance.UpdateAfterAssetChange();
      }
    }
  }
}
