// Decompiled with JetBrains decompiler
// Type: UnityEditor.NavMeshEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(icon = "Navigation", title = "Navigation")]
  internal class NavMeshEditorWindow : EditorWindow, IHasCustomMenu
  {
    private Vector2 m_ScrollPos = Vector2.zero;
    private const string kRootPath = "m_BuildSettings.";
    private static NavMeshEditorWindow s_NavMeshEditorWindow;
    private SerializedObject m_Object;
    private SerializedProperty m_AgentRadius;
    private SerializedProperty m_AgentHeight;
    private SerializedProperty m_AgentSlope;
    private SerializedProperty m_AgentClimb;
    private SerializedProperty m_LedgeDropHeight;
    private SerializedProperty m_MaxJumpAcrossDistance;
    private SerializedProperty m_MinRegionArea;
    private SerializedProperty m_ManualCellSize;
    private SerializedProperty m_CellSize;
    private SerializedProperty m_AccuratePlacement;
    private SerializedObject m_NavMeshAreasObject;
    private SerializedProperty m_Areas;
    private static NavMeshEditorWindow.Styles s_Styles;
    private int m_SelectedNavMeshAgentCount;
    private int m_SelectedNavMeshObstacleCount;
    private bool m_Advanced;
    private bool m_HasPendingAgentDebugInfo;
    private bool m_HasRepaintedForPendingAgentDebugInfo;
    private ReorderableList m_AreasList;
    private NavMeshEditorWindow.Mode m_Mode;

    [MenuItem("Window/Navigation", false, 2100)]
    public static void SetupWindow()
    {
      EditorWindow.GetWindow<NavMeshEditorWindow>(new System.Type[1]
      {
        typeof (InspectorWindow)
      }).minSize = new Vector2(300f, 360f);
    }

    public void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      NavMeshEditorWindow.s_NavMeshEditorWindow = this;
      this.Init();
      EditorApplication.searchChanged += new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneViewGUI);
      this.UpdateSelectedAgentAndObstacleState();
      this.Repaint();
    }

    private void Init()
    {
      this.m_Object = new SerializedObject(NavMeshBuilder.navMeshSettingsObject);
      this.m_AgentRadius = this.m_Object.FindProperty("m_BuildSettings.agentRadius");
      this.m_AgentHeight = this.m_Object.FindProperty("m_BuildSettings.agentHeight");
      this.m_AgentSlope = this.m_Object.FindProperty("m_BuildSettings.agentSlope");
      this.m_LedgeDropHeight = this.m_Object.FindProperty("m_BuildSettings.ledgeDropHeight");
      this.m_AgentClimb = this.m_Object.FindProperty("m_BuildSettings.agentClimb");
      this.m_MaxJumpAcrossDistance = this.m_Object.FindProperty("m_BuildSettings.maxJumpAcrossDistance");
      this.m_MinRegionArea = this.m_Object.FindProperty("m_BuildSettings.minRegionArea");
      this.m_ManualCellSize = this.m_Object.FindProperty("m_BuildSettings.manualCellSize");
      this.m_CellSize = this.m_Object.FindProperty("m_BuildSettings.cellSize");
      this.m_AccuratePlacement = this.m_Object.FindProperty("m_BuildSettings.accuratePlacement");
      this.m_NavMeshAreasObject = new SerializedObject(Unsupported.GetSerializedAssetInterfaceSingleton("NavMeshAreas"));
      this.m_Areas = this.m_NavMeshAreasObject.FindProperty("areas");
      if (this.m_AreasList != null || this.m_NavMeshAreasObject == null || this.m_Areas == null)
        return;
      this.m_AreasList = new ReorderableList(this.m_NavMeshAreasObject, this.m_Areas, false, false, false, false);
      this.m_AreasList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawAreaListElement);
      this.m_AreasList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawAreaListHeader);
      this.m_AreasList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
    }

    private int Bit(int a, int b)
    {
      return (a & 1 << b) >> b;
    }

    private Color GetAreaColor(int i)
    {
      if (i == 0)
        return new Color(0.0f, 0.75f, 1f, 0.5f);
      return new Color((float) ((this.Bit(i, 4) + this.Bit(i, 1) * 2 + 1) * 63) / (float) byte.MaxValue, (float) ((this.Bit(i, 3) + this.Bit(i, 2) * 2 + 1) * 63) / (float) byte.MaxValue, (float) ((this.Bit(i, 5) + this.Bit(i, 0) * 2 + 1) * 63) / (float) byte.MaxValue, 0.5f);
    }

    public void OnDisable()
    {
      NavMeshEditorWindow.s_NavMeshEditorWindow = (NavMeshEditorWindow) null;
      EditorApplication.searchChanged -= new EditorApplication.CallbackFunction(((EditorWindow) this).Repaint);
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneViewGUI);
    }

    private void UpdateSelectedAgentAndObstacleState()
    {
      UnityEngine.Object[] filtered1 = Selection.GetFiltered(typeof (NavMeshAgent), SelectionMode.ExcludePrefab | SelectionMode.Editable);
      UnityEngine.Object[] filtered2 = Selection.GetFiltered(typeof (NavMeshObstacle), SelectionMode.ExcludePrefab | SelectionMode.Editable);
      this.m_SelectedNavMeshAgentCount = filtered1.Length;
      this.m_SelectedNavMeshObstacleCount = filtered2.Length;
    }

    private void OnSelectionChange()
    {
      this.UpdateSelectedAgentAndObstacleState();
      this.m_ScrollPos = Vector2.zero;
      if (this.m_Mode != NavMeshEditorWindow.Mode.ObjectSettings)
        return;
      this.Repaint();
    }

    private void ModeToggle()
    {
      this.m_Mode = (NavMeshEditorWindow.Mode) GUILayout.Toolbar((int) this.m_Mode, NavMeshEditorWindow.s_Styles.m_ModeToggles, (GUIStyle) "LargeButton", new GUILayoutOption[0]);
    }

    private void GetAreaListRects(Rect rect, out Rect stripeRect, out Rect labelRect, out Rect nameRect, out Rect costRect)
    {
      float num1 = EditorGUIUtility.singleLineHeight * 0.8f;
      float num2 = EditorGUIUtility.singleLineHeight * 5f;
      float width = EditorGUIUtility.singleLineHeight * 4f;
      float num3 = rect.width - num1 - num2 - width;
      float x1 = rect.x;
      stripeRect = new Rect(x1, rect.y, num1 - 4f, rect.height);
      float x2 = x1 + num1;
      labelRect = new Rect(x2, rect.y, num2 - 4f, rect.height);
      float x3 = x2 + num2;
      nameRect = new Rect(x3, rect.y, num3 - 4f, rect.height);
      float x4 = x3 + num3;
      costRect = new Rect(x4, rect.y, width, rect.height);
    }

    private void DrawAreaListHeader(Rect rect)
    {
      Rect stripeRect;
      Rect labelRect;
      Rect nameRect;
      Rect costRect;
      this.GetAreaListRects(rect, out stripeRect, out labelRect, out nameRect, out costRect);
      GUI.Label(nameRect, NavMeshEditorWindow.s_Styles.m_NameLabel);
      GUI.Label(costRect, NavMeshEditorWindow.s_Styles.m_CostLabel);
    }

    private void DrawAreaListElement(Rect rect, int index, bool selected, bool focused)
    {
      SerializedProperty arrayElementAtIndex = this.m_Areas.GetArrayElementAtIndex(index);
      if (arrayElementAtIndex == null)
        return;
      SerializedProperty propertyRelative1 = arrayElementAtIndex.FindPropertyRelative("name");
      SerializedProperty propertyRelative2 = arrayElementAtIndex.FindPropertyRelative("cost");
      if (propertyRelative1 == null || propertyRelative2 == null)
        return;
      rect.height -= 2f;
      bool flag1;
      bool flag2;
      bool flag3;
      switch (index)
      {
        case 0:
          flag1 = true;
          flag2 = false;
          flag3 = true;
          break;
        case 1:
          flag1 = true;
          flag2 = false;
          flag3 = false;
          break;
        case 2:
          flag1 = true;
          flag2 = false;
          flag3 = true;
          break;
        default:
          flag1 = false;
          flag2 = true;
          flag3 = true;
          break;
      }
      Rect stripeRect;
      Rect labelRect;
      Rect nameRect;
      Rect costRect;
      this.GetAreaListRects(rect, out stripeRect, out labelRect, out nameRect, out costRect);
      bool enabled = GUI.enabled;
      Color areaColor = this.GetAreaColor(index);
      Color color = new Color(areaColor.r * 0.1f, areaColor.g * 0.1f, areaColor.b * 0.1f, 0.6f);
      EditorGUI.DrawRect(stripeRect, areaColor);
      EditorGUI.DrawRect(new Rect(stripeRect.x, stripeRect.y, 1f, stripeRect.height), color);
      EditorGUI.DrawRect(new Rect((float) ((double) stripeRect.x + (double) stripeRect.width - 1.0), stripeRect.y, 1f, stripeRect.height), color);
      EditorGUI.DrawRect(new Rect(stripeRect.x + 1f, stripeRect.y, stripeRect.width - 2f, 1f), color);
      EditorGUI.DrawRect(new Rect(stripeRect.x + 1f, (float) ((double) stripeRect.y + (double) stripeRect.height - 1.0), stripeRect.width - 2f, 1f), color);
      if (flag1)
        GUI.Label(labelRect, EditorGUIUtility.TempContent("Built-in " + (object) index));
      else
        GUI.Label(labelRect, EditorGUIUtility.TempContent("User " + (object) index));
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      EditorGUI.BeginChangeCheck();
      GUI.enabled = enabled && flag2;
      EditorGUI.PropertyField(nameRect, propertyRelative1, GUIContent.none);
      GUI.enabled = enabled && flag3;
      EditorGUI.PropertyField(costRect, propertyRelative2, GUIContent.none);
      GUI.enabled = enabled;
      EditorGUI.indentLevel = indentLevel;
    }

    public void OnGUI()
    {
      if (this.m_Object.targetObject == (UnityEngine.Object) null)
        this.Init();
      if (NavMeshEditorWindow.s_Styles == null)
        NavMeshEditorWindow.s_Styles = new NavMeshEditorWindow.Styles();
      this.m_Object.Update();
      EditorGUILayout.Space();
      this.ModeToggle();
      EditorGUILayout.Space();
      this.m_ScrollPos = EditorGUILayout.BeginScrollView(this.m_ScrollPos);
      switch (this.m_Mode)
      {
        case NavMeshEditorWindow.Mode.ObjectSettings:
          NavMeshEditorWindow.ObjectSettings();
          break;
        case NavMeshEditorWindow.Mode.BakeSettings:
          this.BakeSettings();
          break;
        case NavMeshEditorWindow.Mode.AreaSettings:
          this.AreaSettings();
          break;
      }
      EditorGUILayout.EndScrollView();
      NavMeshEditorWindow.BakeButtons();
      this.m_Object.ApplyModifiedProperties();
    }

    public void OnBecameVisible()
    {
      if (NavMeshVisualizationSettings.showNavigation)
        return;
      NavMeshVisualizationSettings.showNavigation = true;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
    }

    public void OnBecameInvisible()
    {
      NavMeshVisualizationSettings.showNavigation = false;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
    }

    private static void RepaintSceneAndGameViews()
    {
      SceneView.RepaintAll();
      foreach (EditorWindow editorWindow in Resources.FindObjectsOfTypeAll(typeof (GameView)))
        editorWindow.Repaint();
    }

    public void OnSceneViewGUI(SceneView sceneView)
    {
      if (!NavMeshVisualizationSettings.showNavigation)
        return;
      SceneViewOverlay.Window(new GUIContent("Navmesh Display"), new SceneViewOverlay.WindowFunction(NavMeshEditorWindow.DisplayControls), 300, SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget);
      if (this.m_SelectedNavMeshAgentCount > 0)
        SceneViewOverlay.Window(new GUIContent("Agent Display"), new SceneViewOverlay.WindowFunction(NavMeshEditorWindow.DisplayAgentControls), 300, SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget);
      if (this.m_SelectedNavMeshObstacleCount <= 0)
        return;
      SceneViewOverlay.Window(new GUIContent("Obstacle Display"), new SceneViewOverlay.WindowFunction(NavMeshEditorWindow.DisplayObstacleControls), 300, SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget);
    }

    private static void DisplayControls(UnityEngine.Object target, SceneView sceneView)
    {
      EditorGUIUtility.labelWidth = 150f;
      bool flag = false;
      bool showNavMesh = NavMeshVisualizationSettings.showNavMesh;
      if (showNavMesh != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show NavMesh"), showNavMesh, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showNavMesh = !showNavMesh;
        flag = true;
      }
      EditorGUI.BeginDisabledGroup(!NavMeshVisualizationSettings.hasHeightMesh);
      bool showHeightMesh = NavMeshVisualizationSettings.showHeightMesh;
      if (showHeightMesh != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show HeightMesh"), showHeightMesh, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showHeightMesh = !showHeightMesh;
        flag = true;
      }
      EditorGUI.EndDisabledGroup();
      if (Unsupported.IsDeveloperBuild())
      {
        GUILayout.Label("Internal");
        bool showNavMeshPortals = NavMeshVisualizationSettings.showNavMeshPortals;
        if (showNavMeshPortals != EditorGUILayout.Toggle(new GUIContent("Show NavMesh Portals"), showNavMeshPortals, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showNavMeshPortals = !showNavMeshPortals;
          flag = true;
        }
        bool showNavMeshLinks = NavMeshVisualizationSettings.showNavMeshLinks;
        if (showNavMeshLinks != EditorGUILayout.Toggle(new GUIContent("Show NavMesh Links"), showNavMeshLinks, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showNavMeshLinks = !showNavMeshLinks;
          flag = true;
        }
        bool showProximityGrid = NavMeshVisualizationSettings.showProximityGrid;
        if (showProximityGrid != EditorGUILayout.Toggle(new GUIContent("Show Proximity Grid"), showProximityGrid, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showProximityGrid = !showProximityGrid;
          flag = true;
        }
        bool heightMeshBvTree = NavMeshVisualizationSettings.showHeightMeshBVTree;
        if (heightMeshBvTree != EditorGUILayout.Toggle(new GUIContent("Show HeightMesh BV-Tree"), heightMeshBvTree, new GUILayoutOption[0]))
        {
          NavMeshVisualizationSettings.showHeightMeshBVTree = !heightMeshBvTree;
          flag = true;
        }
      }
      if (!flag)
        return;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
    }

    private void OnInspectorUpdate()
    {
      if (this.m_SelectedNavMeshAgentCount <= 0)
        return;
      if (this.m_HasPendingAgentDebugInfo != NavMeshVisualizationSettings.hasPendingAgentDebugInfo)
      {
        if (this.m_HasRepaintedForPendingAgentDebugInfo)
          return;
        this.m_HasRepaintedForPendingAgentDebugInfo = true;
        NavMeshEditorWindow.RepaintSceneAndGameViews();
      }
      else
        this.m_HasRepaintedForPendingAgentDebugInfo = false;
    }

    private static void DisplayAgentControls(UnityEngine.Object target, SceneView sceneView)
    {
      EditorGUIUtility.labelWidth = 150f;
      bool flag = false;
      if (Event.current.type == EventType.Layout)
        NavMeshEditorWindow.s_NavMeshEditorWindow.m_HasPendingAgentDebugInfo = NavMeshVisualizationSettings.hasPendingAgentDebugInfo;
      bool showAgentPath = NavMeshVisualizationSettings.showAgentPath;
      if (showAgentPath != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Path Polygons|Shows the polygons leading to goal."), showAgentPath, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentPath = !showAgentPath;
        flag = true;
      }
      bool showAgentPathInfo = NavMeshVisualizationSettings.showAgentPathInfo;
      if (showAgentPathInfo != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Path Query Nodes|Shows the nodes expanded during last path query."), showAgentPathInfo, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentPathInfo = !showAgentPathInfo;
        flag = true;
      }
      bool showAgentNeighbours = NavMeshVisualizationSettings.showAgentNeighbours;
      if (showAgentNeighbours != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Neighbours|Show the agent neighbours cosidered during simulation."), showAgentNeighbours, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentNeighbours = !showAgentNeighbours;
        flag = true;
      }
      bool showAgentWalls = NavMeshVisualizationSettings.showAgentWalls;
      if (showAgentWalls != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Walls|Shows the wall segments handled during simulation."), showAgentWalls, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentWalls = !showAgentWalls;
        flag = true;
      }
      bool showAgentAvoidance = NavMeshVisualizationSettings.showAgentAvoidance;
      if (showAgentAvoidance != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Avoidance|Shows the processed avoidance geometry from simulation."), showAgentAvoidance, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showAgentAvoidance = !showAgentAvoidance;
        flag = true;
      }
      if (showAgentAvoidance)
      {
        if (NavMeshEditorWindow.s_NavMeshEditorWindow.m_HasPendingAgentDebugInfo)
        {
          EditorGUILayout.BeginVertical(GUILayout.MaxWidth(165f));
          EditorGUILayout.HelpBox("Avoidance display is not valid until after next game update.", MessageType.Warning);
          EditorGUILayout.EndVertical();
        }
        if (NavMeshEditorWindow.s_NavMeshEditorWindow.m_SelectedNavMeshAgentCount > 10)
        {
          EditorGUILayout.BeginVertical(GUILayout.MaxWidth(165f));
          EditorGUILayout.HelpBox("Avoidance visualization can be drawn for " + (object) 10 + " agents (" + (object) NavMeshEditorWindow.s_NavMeshEditorWindow.m_SelectedNavMeshAgentCount + " selected).", MessageType.Warning);
          EditorGUILayout.EndVertical();
        }
      }
      if (!flag)
        return;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
    }

    private static void DisplayObstacleControls(UnityEngine.Object target, SceneView sceneView)
    {
      EditorGUIUtility.labelWidth = 150f;
      bool flag = false;
      bool obstacleCarveHull = NavMeshVisualizationSettings.showObstacleCarveHull;
      if (obstacleCarveHull != EditorGUILayout.Toggle(EditorGUIUtility.TextContent("Show Carve Hull|Shows the hull used to carve the obstacle from navmesh."), obstacleCarveHull, new GUILayoutOption[0]))
      {
        NavMeshVisualizationSettings.showObstacleCarveHull = !obstacleCarveHull;
        flag = true;
      }
      if (!flag)
        return;
      NavMeshEditorWindow.RepaintSceneAndGameViews();
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      menu.AddItem(new GUIContent("Reset Bake Settings"), false, new GenericMenu.MenuFunction(this.ResetBakeSettings));
    }

    private void ResetBakeSettings()
    {
      Unsupported.SmartReset(NavMeshBuilder.navMeshSettingsObject);
    }

    public static void BackgroundTaskStatusChanged()
    {
      if (!((UnityEngine.Object) NavMeshEditorWindow.s_NavMeshEditorWindow != (UnityEngine.Object) null))
        return;
      NavMeshEditorWindow.s_NavMeshEditorWindow.Repaint();
    }

    private static IEnumerable<GameObject> GetObjectsRecurse(GameObject root)
    {
      List<GameObject> gameObjectList = new List<GameObject>() { root };
      foreach (Transform transform in root.transform)
        gameObjectList.AddRange(NavMeshEditorWindow.GetObjectsRecurse(transform.gameObject));
      return (IEnumerable<GameObject>) gameObjectList;
    }

    private static List<GameObject> GetObjects(bool includeChildren)
    {
      if (!includeChildren)
        return new List<GameObject>((IEnumerable<GameObject>) Selection.gameObjects);
      List<GameObject> gameObjectList = new List<GameObject>();
      foreach (GameObject gameObject in Selection.gameObjects)
        gameObjectList.AddRange(NavMeshEditorWindow.GetObjectsRecurse(gameObject));
      return gameObjectList;
    }

    private static bool SelectionHasChildren()
    {
      return ((IEnumerable<GameObject>) Selection.gameObjects).Any<GameObject>((Func<GameObject, bool>) (obj => obj.transform.childCount > 0));
    }

    private static void SetNavMeshArea(int area, bool includeChildren)
    {
      List<GameObject> objects = NavMeshEditorWindow.GetObjects(includeChildren);
      if (objects.Count <= 0)
        return;
      Undo.RecordObjects((UnityEngine.Object[]) objects.ToArray(), "Change NavMesh area");
      using (List<GameObject>.Enumerator enumerator = objects.GetEnumerator())
      {
        while (enumerator.MoveNext())
          GameObjectUtility.SetNavMeshArea(enumerator.Current, area);
      }
    }

    private static void ObjectSettings()
    {
      bool flag = true;
      SceneModeUtility.SearchBar(typeof (MeshRenderer), typeof (Terrain));
      EditorGUILayout.Space();
      GameObject[] gameObjects;
      MeshRenderer[] selectedObjectsOfType1 = SceneModeUtility.GetSelectedObjectsOfType<MeshRenderer>(out gameObjects);
      if (gameObjects.Length > 0)
      {
        flag = false;
        NavMeshEditorWindow.ObjectSettings((UnityEngine.Object[]) selectedObjectsOfType1, gameObjects);
      }
      Terrain[] selectedObjectsOfType2 = SceneModeUtility.GetSelectedObjectsOfType<Terrain>(out gameObjects);
      if (gameObjects.Length > 0)
      {
        flag = false;
        NavMeshEditorWindow.ObjectSettings((UnityEngine.Object[]) selectedObjectsOfType2, gameObjects);
      }
      if (!flag)
        return;
      GUILayout.Label("Select a MeshRenderer or a Terrain from the scene.", EditorStyles.helpBox, new GUILayoutOption[0]);
    }

    private static void ObjectSettings(UnityEngine.Object[] components, GameObject[] gos)
    {
      EditorGUILayout.MultiSelectionObjectTitleBar(components);
      SerializedObject serializedObject = new SerializedObject((UnityEngine.Object[]) gos);
      EditorGUI.BeginDisabledGroup(!SceneModeUtility.StaticFlagField("Navigation Static", serializedObject.FindProperty("m_StaticEditorFlags"), 8));
      SceneModeUtility.StaticFlagField("Generate OffMeshLinks", serializedObject.FindProperty("m_StaticEditorFlags"), 32);
      SerializedProperty property = serializedObject.FindProperty("m_NavMeshLayer");
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
      string[] navMeshAreaNames = GameObjectUtility.GetNavMeshAreaNames();
      int navMeshArea = GameObjectUtility.GetNavMeshArea(gos[0]);
      int selectedIndex = -1;
      for (int index = 0; index < navMeshAreaNames.Length; ++index)
      {
        if (GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[index]) == navMeshArea)
        {
          selectedIndex = index;
          break;
        }
      }
      int index1 = EditorGUILayout.Popup("Navigation Area", selectedIndex, navMeshAreaNames, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
      {
        int meshAreaFromName = GameObjectUtility.GetNavMeshAreaFromName(navMeshAreaNames[index1]);
        GameObjectUtility.ShouldIncludeChildren shouldIncludeChildren = GameObjectUtility.DisplayUpdateChildrenDialogIfNeeded((IEnumerable<GameObject>) Selection.gameObjects, "Change Navigation Area", "Do you want change the navigation area to " + navMeshAreaNames[index1] + " for all the child objects as well?");
        if (shouldIncludeChildren != GameObjectUtility.ShouldIncludeChildren.Cancel)
        {
          property.intValue = meshAreaFromName;
          NavMeshEditorWindow.SetNavMeshArea(meshAreaFromName, shouldIncludeChildren == GameObjectUtility.ShouldIncludeChildren.IncludeChildren);
        }
      }
      EditorGUI.EndDisabledGroup();
      serializedObject.ApplyModifiedProperties();
    }

    private void DrawAgentDiagram(Rect rect, float agentRadius, float agentHeight, float agentClimb, float agentSlope)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      float num1 = agentRadius;
      float num2 = agentHeight;
      float num3 = agentClimb;
      float num4 = 0.35f;
      float num5 = 15f;
      float num6 = rect.height - num5 * 2f;
      float num7 = Mathf.Min(num6 / (num2 + num1 * 2f * num4), num6 / (num1 * 2f));
      float num8 = num2 * num7;
      float num9 = num1 * num7;
      float num10 = num3 * num7;
      float x1 = rect.xMin + rect.width * 0.5f;
      float y1 = (float) ((double) rect.yMax - (double) num5 - (double) num9 * (double) num4);
      Vector3[] vector3Array1 = new Vector3[40];
      Vector3[] vector3Array2 = new Vector3[20];
      Vector3[] vector3Array3 = new Vector3[20];
      for (int index = 0; index < 20; ++index)
      {
        float f = (float) ((double) index / 19.0 * 3.14159274101257);
        float num11 = Mathf.Cos(f);
        float num12 = Mathf.Sin(f);
        vector3Array1[index] = new Vector3(x1 + num11 * num9, (float) ((double) y1 - (double) num8 - (double) num12 * (double) num9 * (double) num4), 0.0f);
        vector3Array1[index + 20] = new Vector3(x1 - num11 * num9, y1 + num12 * num9 * num4, 0.0f);
        vector3Array2[index] = new Vector3(x1 - num11 * num9, (float) ((double) y1 - (double) num8 + (double) num12 * (double) num9 * (double) num4), 0.0f);
        vector3Array3[index] = new Vector3(x1 - num11 * num9, (float) ((double) y1 - (double) num10 + (double) num12 * (double) num9 * (double) num4), 0.0f);
      }
      Color color = Handles.color;
      float xMin = rect.xMin;
      float y2 = y1 - num10;
      float x2 = x1 - num6 * 0.75f;
      float y3 = y1;
      float x3 = x1 + num6 * 0.75f;
      float y4 = y1;
      float num13 = x3;
      float num14 = y4;
      float num15 = rect.xMax - x3;
      float x4 = num13 + Mathf.Cos(agentSlope * ((float) Math.PI / 180f)) * num15;
      float y5 = num14 - Mathf.Sin(agentSlope * ((float) Math.PI / 180f)) * num15;
      Vector3[] vector3Array4 = new Vector3[2]{ new Vector3(xMin, y1, 0.0f), new Vector3(x4, y1, 0.0f) };
      Vector3[] vector3Array5 = new Vector3[5]{ new Vector3(xMin, y2, 0.0f), new Vector3(x2, y2, 0.0f), new Vector3(x2, y3, 0.0f), new Vector3(x3, y4, 0.0f), new Vector3(x4, y5, 0.0f) };
      Handles.color = !EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.5f) : new Color(0.0f, 0.0f, 0.0f, 0.5f);
      Handles.DrawAAPolyLine(2f, vector3Array4);
      Handles.color = !EditorGUIUtility.isProSkin ? new Color(0.0f, 0.0f, 0.0f, 0.5f) : new Color(1f, 1f, 1f, 0.5f);
      Handles.DrawAAPolyLine(3f, vector3Array5);
      Handles.color = Color.Lerp(new Color(0.0f, 0.75f, 1f, 1f), new Color(0.5f, 0.5f, 0.5f, 0.5f), 0.2f);
      Handles.DrawAAConvexPolygon(vector3Array1);
      Handles.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
      Handles.DrawAAPolyLine(2f, vector3Array3);
      Handles.color = new Color(1f, 1f, 1f, 0.4f);
      Handles.DrawAAPolyLine(2f, vector3Array2);
      Vector3[] vector3Array6 = new Vector3[2]{ new Vector3(x1, y1 - num8, 0.0f), new Vector3(x1 + num9, y1 - num8, 0.0f) };
      Handles.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
      Handles.DrawAAPolyLine(2f, vector3Array6);
      GUI.Label(new Rect((float) ((double) x1 + (double) num9 + 5.0), (float) ((double) y1 - (double) num8 * 0.5 - 10.0), 150f, 20f), string.Format("H = {0}", (object) agentHeight));
      GUI.Label(new Rect(x1, (float) ((double) y1 - (double) num8 - (double) num9 * (double) num4 - 15.0), 150f, 20f), string.Format("R = {0}", (object) agentRadius));
      GUI.Label(new Rect((float) (((double) xMin + (double) x2) * 0.5 - 20.0), y2 - 15f, 150f, 20f), string.Format("{0}", (object) agentClimb));
      GUI.Label(new Rect(x3 + 20f, y4 - 15f, 150f, 20f), string.Format("{0}°", (object) agentSlope));
      Handles.color = color;
    }

    private void BakeSettings()
    {
      EditorGUILayout.LabelField(NavMeshEditorWindow.s_Styles.m_AgentSizeHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.DrawAgentDiagram(EditorGUILayout.GetControlRect(false, 120f, new GUILayoutOption[0]), this.m_AgentRadius.floatValue, this.m_AgentHeight.floatValue, this.m_AgentClimb.floatValue, this.m_AgentSlope.floatValue);
      float num1 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentRadiusContent, this.m_AgentRadius.floatValue, new GUILayoutOption[0]);
      if ((double) num1 >= 1.0 / 1000.0 && !Mathf.Approximately(num1 - this.m_AgentRadius.floatValue, 0.0f))
      {
        this.m_AgentRadius.floatValue = num1;
        if (!this.m_ManualCellSize.boolValue)
          this.m_CellSize.floatValue = (float) (2.0 * (double) this.m_AgentRadius.floatValue / 6.0);
      }
      if ((double) this.m_AgentRadius.floatValue < 0.0500000007450581 && !this.m_ManualCellSize.boolValue)
        EditorGUILayout.HelpBox("The agent radius you've set is really small, this can slow down the build.\nIf you intended to allow the agent to move close to the borders and walls, please adjust voxel size in advaced settings to ensure correct bake.", MessageType.Warning);
      float num2 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentHeightContent, this.m_AgentHeight.floatValue, new GUILayoutOption[0]);
      if ((double) num2 >= 1.0 / 1000.0 && !Mathf.Approximately(num2 - this.m_AgentHeight.floatValue, 0.0f))
        this.m_AgentHeight.floatValue = num2;
      EditorGUILayout.Slider(this.m_AgentSlope, 0.0f, 60f, NavMeshEditorWindow.s_Styles.m_AgentSlopeContent, new GUILayoutOption[0]);
      if ((double) this.m_AgentSlope.floatValue > 60.0)
        EditorGUILayout.HelpBox("The maximum slope should be set to less than " + (object) 60f + " degrees to prevent NavMesh build artifacts on slopes. ", MessageType.Warning);
      float num3 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentClimbContent, this.m_AgentClimb.floatValue, new GUILayoutOption[0]);
      if ((double) num3 >= 0.0 && !Mathf.Approximately(this.m_AgentClimb.floatValue - num3, 0.0f))
        this.m_AgentClimb.floatValue = num3;
      if ((double) this.m_AgentClimb.floatValue > (double) this.m_AgentHeight.floatValue)
        EditorGUILayout.HelpBox("Step height should be less than agent height.\nClamping step height to " + (object) this.m_AgentHeight.floatValue + " internally when baking.", MessageType.Warning);
      float floatValue = this.m_CellSize.floatValue;
      float num4 = floatValue * 0.5f;
      int num5 = (int) Mathf.Ceil(this.m_AgentClimb.floatValue / num4);
      int num6 = (int) Mathf.Ceil(Mathf.Tan((float) ((double) this.m_AgentSlope.floatValue / 180.0 * 3.14159274101257)) * floatValue * 2f / num4);
      if (num6 > num5)
        EditorGUILayout.HelpBox("Step Height conflicts with Max Slope. This makes some slopes unwalkable.\nConsider decreasing Max Slope to < " + ((float) ((double) Mathf.Atan((float) ((double) num5 * (double) num4 / ((double) floatValue * 2.0))) / 3.14159274101257 * 180.0)).ToString("0.0") + " degrees.\nOr, increase Step Height to > " + ((float) (num6 - 1) * num4).ToString("0.00") + ".", MessageType.Warning);
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(NavMeshEditorWindow.s_Styles.m_OffmeshHeader, EditorStyles.boldLabel, new GUILayoutOption[0]);
      float num7 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentDropContent, this.m_LedgeDropHeight.floatValue, new GUILayoutOption[0]);
      if ((double) num7 >= 0.0 && !Mathf.Approximately(num7 - this.m_LedgeDropHeight.floatValue, 0.0f))
        this.m_LedgeDropHeight.floatValue = num7;
      float num8 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_AgentJumpContent, this.m_MaxJumpAcrossDistance.floatValue, new GUILayoutOption[0]);
      if ((double) num8 >= 0.0 && !Mathf.Approximately(num8 - this.m_MaxJumpAcrossDistance.floatValue, 0.0f))
        this.m_MaxJumpAcrossDistance.floatValue = num8;
      EditorGUILayout.Space();
      this.m_Advanced = GUILayout.Toggle(this.m_Advanced, NavMeshEditorWindow.s_Styles.m_AdvancedHeader, EditorStyles.foldout, new GUILayoutOption[0]);
      if (this.m_Advanced)
      {
        ++EditorGUI.indentLevel;
        bool flag1 = EditorGUILayout.Toggle(NavMeshEditorWindow.s_Styles.m_ManualCellSizeContent, this.m_ManualCellSize.boolValue, new GUILayoutOption[0]);
        if (flag1 != this.m_ManualCellSize.boolValue)
        {
          this.m_ManualCellSize.boolValue = flag1;
          if (!flag1)
            this.m_CellSize.floatValue = (float) (2.0 * (double) this.m_AgentRadius.floatValue / 6.0);
        }
        EditorGUI.BeginDisabledGroup(!this.m_ManualCellSize.boolValue);
        ++EditorGUI.indentLevel;
        float val2 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_CellSizeContent, this.m_CellSize.floatValue, new GUILayoutOption[0]);
        if ((double) val2 > 0.0 && !Mathf.Approximately(val2 - this.m_CellSize.floatValue, 0.0f))
          this.m_CellSize.floatValue = Math.Max(0.01f, val2);
        if ((double) val2 < 0.00999999977648258)
          EditorGUILayout.HelpBox("The voxel size should be larger than 0.01.", MessageType.Warning);
        float num9 = (double) this.m_CellSize.floatValue <= 0.0 ? 0.0f : this.m_AgentRadius.floatValue / this.m_CellSize.floatValue;
        EditorGUILayout.LabelField(" ", num9.ToString("0.00") + " voxels per agent radius", EditorStyles.miniLabel, new GUILayoutOption[0]);
        if (this.m_ManualCellSize.boolValue)
        {
          if ((int) Mathf.Floor(this.m_AgentHeight.floatValue / (this.m_CellSize.floatValue * 0.5f)) > 250)
            EditorGUILayout.HelpBox("The number of voxels per agent height is too high. This will reduce the accuracy of the navmesh. Consider using voxel size of at least " + ((float) ((double) this.m_AgentHeight.floatValue / 250.0 / 0.5)).ToString("0.000") + ".", MessageType.Warning);
          if ((double) num9 < 1.0)
            EditorGUILayout.HelpBox("The number of voxels per agent radius is too small. The agent may not avoid walls and ledges properly. Consider using voxel size of at least " + (this.m_AgentRadius.floatValue / 2f).ToString("0.000") + " (2 voxels per agent radius).", MessageType.Warning);
          else if ((double) num9 > 8.0)
            EditorGUILayout.HelpBox("The number of voxels per agent radius is too high. It can cause excessive build times. Consider using voxel size closer to " + (this.m_AgentRadius.floatValue / 8f).ToString("0.000") + " (8 voxels per radius).", MessageType.Warning);
        }
        if (this.m_ManualCellSize.boolValue)
          EditorGUILayout.HelpBox("Voxel size controls how accurately the navigation mesh is generated from the level geometry. A good voxel size is 2-4 voxels per agent radius. Making voxel size smaller will increase build time.", MessageType.None);
        --EditorGUI.indentLevel;
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
        float num10 = EditorGUILayout.FloatField(NavMeshEditorWindow.s_Styles.m_MinRegionAreaContent, this.m_MinRegionArea.floatValue, new GUILayoutOption[0]);
        if ((double) num10 >= 0.0 && (double) num10 != (double) this.m_MinRegionArea.floatValue)
          this.m_MinRegionArea.floatValue = num10;
        EditorGUILayout.Space();
        bool flag2 = EditorGUILayout.Toggle(NavMeshEditorWindow.s_Styles.m_AgentPlacementContent, this.m_AccuratePlacement.boolValue, new GUILayoutOption[0]);
        if (flag2 != this.m_AccuratePlacement.boolValue)
          this.m_AccuratePlacement.boolValue = flag2;
        --EditorGUI.indentLevel;
      }
      if (!Unsupported.IsDeveloperBuild())
        return;
      EditorGUILayout.Space();
      GUILayout.Label("Internal Bake Debug Options", EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUILayout.HelpBox("Note: The debug visualization is build during bake, so you'll need to bake for these settings to take effect.", MessageType.None);
      bool meshLinkSampling = NavMeshVisualizationSettings.showAutoOffMeshLinkSampling;
      if (meshLinkSampling != EditorGUILayout.Toggle(new GUIContent("Show Auto-Off-MeshLink Sampling"), meshLinkSampling, new GUILayoutOption[0]))
        NavMeshVisualizationSettings.showAutoOffMeshLinkSampling = !meshLinkSampling;
      bool showVoxels = NavMeshVisualizationSettings.showVoxels;
      if (showVoxels != EditorGUILayout.Toggle(new GUIContent("Show Voxels"), showVoxels, new GUILayoutOption[0]))
        NavMeshVisualizationSettings.showVoxels = !showVoxels;
      bool showWalkable = NavMeshVisualizationSettings.showWalkable;
      if (showWalkable != EditorGUILayout.Toggle(new GUIContent("Show Walkable"), showWalkable, new GUILayoutOption[0]))
        NavMeshVisualizationSettings.showWalkable = !showWalkable;
      bool showRawContours = NavMeshVisualizationSettings.showRawContours;
      if (showRawContours != EditorGUILayout.Toggle(new GUIContent("Show Raw Contours"), showRawContours, new GUILayoutOption[0]))
        NavMeshVisualizationSettings.showRawContours = !showRawContours;
      bool showContours = NavMeshVisualizationSettings.showContours;
      if (showContours != EditorGUILayout.Toggle(new GUIContent("Show Contours"), showContours, new GUILayoutOption[0]))
        NavMeshVisualizationSettings.showContours = !showContours;
      bool showInputs = NavMeshVisualizationSettings.showInputs;
      if (showInputs != EditorGUILayout.Toggle(new GUIContent("Show Inputs"), showInputs, new GUILayoutOption[0]))
        NavMeshVisualizationSettings.showInputs = !showInputs;
      if (GUILayout.Button("Clear Visualiation Data"))
      {
        NavMeshVisualizationSettings.ClearVisualizationData();
        NavMeshEditorWindow.RepaintSceneAndGameViews();
      }
      EditorGUILayout.Space();
    }

    private void AreaSettings()
    {
      if (this.m_NavMeshAreasObject == null || this.m_AreasList == null)
        return;
      this.m_NavMeshAreasObject.Update();
      this.m_AreasList.DoLayoutList();
      this.m_NavMeshAreasObject.ApplyModifiedProperties();
    }

    private static void BakeButtons()
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      bool enabled1 = GUI.enabled;
      GUI.enabled &= !Application.isPlaying;
      if (GUILayout.Button("Clear", new GUILayoutOption[1]{ GUILayout.Width(95f) }))
        NavMeshBuilder.ClearAllNavMeshes();
      GUI.enabled = enabled1;
      if (NavMeshBuilder.isRunning)
      {
        if (GUILayout.Button("Cancel", new GUILayoutOption[1]{ GUILayout.Width(95f) }))
          NavMeshBuilder.Cancel();
      }
      else
      {
        bool enabled2 = GUI.enabled;
        GUI.enabled &= !Application.isPlaying;
        if (GUILayout.Button("Bake", new GUILayoutOption[1]{ GUILayout.Width(95f) }))
          NavMeshBuilder.BuildNavMeshAsync();
        GUI.enabled = enabled2;
      }
      GUILayout.EndHorizontal();
      EditorGUILayout.Space();
    }

    private enum Mode
    {
      ObjectSettings,
      BakeSettings,
      AreaSettings,
    }

    private class Styles
    {
      public readonly GUIContent m_AgentRadiusContent = EditorGUIUtility.TextContent("Agent Radius|How close to the walls navigation mesh exist.");
      public readonly GUIContent m_AgentHeightContent = EditorGUIUtility.TextContent("Agent Height|How much vertical clearance space must exist.");
      public readonly GUIContent m_AgentSlopeContent = EditorGUIUtility.TextContent("Max Slope|Maximum slope the agent can walk up.");
      public readonly GUIContent m_AgentDropContent = EditorGUIUtility.TextContent("Drop Height|Maximum agent drop height.");
      public readonly GUIContent m_AgentClimbContent = EditorGUIUtility.TextContent("Step Height|The height of discontinuities in the level the agent can climb over (i.e. steps and stairs).");
      public readonly GUIContent m_AgentJumpContent = EditorGUIUtility.TextContent("Jump Distance|Maximum agent jump distance.");
      public readonly GUIContent m_AgentPlacementContent = EditorGUIUtility.TextContent("Height Mesh|Generate an accurate height mesh for precise agent placement (slower).");
      public readonly GUIContent m_MinRegionAreaContent = EditorGUIUtility.TextContent("Min Region Area|Minimum area that a navmesh region can be.");
      public readonly GUIContent m_ManualCellSizeContent = EditorGUIUtility.TextContent("Manual Voxel Size|Enable to set voxel size manually.");
      public readonly GUIContent m_CellSizeContent = EditorGUIUtility.TextContent("Voxel Size|Specifies at the voxelization resolution at which the NavMesh is build.");
      public readonly GUIContent m_AgentSizeHeader = new GUIContent("Baked Agent Size");
      public readonly GUIContent m_OffmeshHeader = new GUIContent("Generated Off Mesh Links");
      public readonly GUIContent m_AdvancedHeader = new GUIContent("Advanced");
      public readonly GUIContent m_NameLabel = new GUIContent("Name");
      public readonly GUIContent m_CostLabel = new GUIContent("Cost");
      public readonly GUIContent[] m_ModeToggles = new GUIContent[3]{ EditorGUIUtility.TextContent("Object|Bake settings for the currently selected object."), EditorGUIUtility.TextContent("Bake|Navmesh bake settings."), EditorGUIUtility.TextContent("Areas|Navmesh area settings.") };
    }
  }
}
