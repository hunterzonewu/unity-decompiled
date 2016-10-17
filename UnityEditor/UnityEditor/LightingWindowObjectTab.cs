// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingWindowObjectTab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineInternal;

namespace UnityEditor
{
  internal class LightingWindowObjectTab
  {
    private static GUIContent[] kObjectPreviewTextureOptions = new GUIContent[7]{ EditorGUIUtility.TextContent("Charting"), EditorGUIUtility.TextContent("Albedo"), EditorGUIUtility.TextContent("Emissive"), EditorGUIUtility.TextContent("Realtime Intensity"), EditorGUIUtility.TextContent("Realtime Direction"), EditorGUIUtility.TextContent("Baked Intensity"), EditorGUIUtility.TextContent("Baked Direction") };
    private GITextureType[] kObjectPreviewTextureTypes = new GITextureType[7]{ GITextureType.Charting, GITextureType.Albedo, GITextureType.Emissive, GITextureType.Irradiance, GITextureType.Directionality, GITextureType.Baked, GITextureType.BakedDirectional };
    private AnimBool m_ShowClampedSize = new AnimBool();
    private static LightingWindowObjectTab.Styles s_Styles;
    private ZoomableArea m_ZoomablePreview;
    private GUIContent m_SelectedObjectPreviewTexture;
    private int m_PreviousSelection;
    private bool m_ShowBakedLM;
    private bool m_ShowRealtimeLM;
    private bool m_HasSeparateIndirectUV;
    private Editor m_LightEditor;
    private Editor m_LightmapParametersEditor;

    public void OnEnable(EditorWindow window)
    {
      this.m_ShowClampedSize.value = false;
      this.m_ShowClampedSize.valueChanged.AddListener(new UnityAction(window.Repaint));
    }

    public void OnDisable()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_LightEditor);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_LightmapParametersEditor);
    }

    private Editor GetLightEditor(Light[] lights)
    {
      Editor.CreateCachedEditor((UnityEngine.Object[]) lights, typeof (LightEditor), ref this.m_LightEditor);
      return this.m_LightEditor;
    }

    private Editor GetLightmapParametersEditor(UnityEngine.Object[] lights)
    {
      Editor.CreateCachedEditor(lights, typeof (LightmapParametersEditor), ref this.m_LightmapParametersEditor);
      return this.m_LightmapParametersEditor;
    }

    public void ObjectPreview(Rect r)
    {
      if ((double) r.height <= 0.0)
        return;
      if (LightingWindowObjectTab.s_Styles == null)
        LightingWindowObjectTab.s_Styles = new LightingWindowObjectTab.Styles();
      List<Texture2D> texture2DList = new List<Texture2D>();
      foreach (GITextureType previewTextureType in this.kObjectPreviewTextureTypes)
        texture2DList.Add(LightmapVisualizationUtility.GetGITexture(previewTextureType));
      if (texture2DList.Count == 0)
        return;
      if (this.m_ZoomablePreview == null)
      {
        this.m_ZoomablePreview = new ZoomableArea(true);
        this.m_ZoomablePreview.hRangeMin = 0.0f;
        this.m_ZoomablePreview.vRangeMin = 0.0f;
        this.m_ZoomablePreview.hRangeMax = 1f;
        this.m_ZoomablePreview.vRangeMax = 1f;
        this.m_ZoomablePreview.SetShownHRange(0.0f, 1f);
        this.m_ZoomablePreview.SetShownVRange(0.0f, 1f);
        this.m_ZoomablePreview.uniformScale = true;
        this.m_ZoomablePreview.scaleWithWindow = true;
      }
      GUI.Box(r, string.Empty, (GUIStyle) "PreBackground");
      Rect position1 = new Rect(r);
      ++position1.y;
      position1.height = 18f;
      GUI.Box(position1, string.Empty, EditorStyles.toolbar);
      Rect position2 = new Rect(r);
      ++position2.y;
      position2.height = 18f;
      position2.width = 120f;
      Rect rect1 = new Rect(r);
      rect1.yMin += position2.height;
      rect1.yMax -= 14f;
      rect1.width -= 11f;
      int selectedIndex = Array.IndexOf<GUIContent>(LightingWindowObjectTab.kObjectPreviewTextureOptions, this.m_SelectedObjectPreviewTexture);
      if (selectedIndex < 0)
        selectedIndex = 0;
      int index = EditorGUI.Popup(position2, selectedIndex, LightingWindowObjectTab.kObjectPreviewTextureOptions, EditorStyles.toolbarPopup);
      if (index >= LightingWindowObjectTab.kObjectPreviewTextureOptions.Length)
        index = 0;
      this.m_SelectedObjectPreviewTexture = LightingWindowObjectTab.kObjectPreviewTextureOptions[index];
      LightmapType lightmapType = this.kObjectPreviewTextureTypes[index] == GITextureType.Baked || this.kObjectPreviewTextureTypes[index] == GITextureType.BakedDirectional ? LightmapType.StaticLightmap : LightmapType.DynamicLightmap;
      SerializedProperty property = new SerializedObject(LightmapEditorSettings.GetLightmapSettings()).FindProperty("m_LightmapsMode");
      bool flag = (this.kObjectPreviewTextureTypes[index] == GITextureType.Baked || this.kObjectPreviewTextureTypes[index] == GITextureType.BakedDirectional) && property.intValue == 2;
      if (flag)
      {
        GUIContent content = GUIContent.Temp("Indirect");
        Rect position3 = position2;
        position3.x += position2.width;
        position3.width = EditorStyles.toolbarButton.CalcSize(content).x;
        this.m_HasSeparateIndirectUV = GUI.Toggle(position3, this.m_HasSeparateIndirectUV, content.text, EditorStyles.toolbarButton);
      }
      switch (Event.current.type)
      {
        case EventType.ValidateCommand:
        case EventType.ExecuteCommand:
          if (Event.current.commandName == "FrameSelected")
          {
            Vector4 lightmapTilingOffset = LightmapVisualizationUtility.GetLightmapTilingOffset(lightmapType);
            Vector2 lhs1 = new Vector2(lightmapTilingOffset.z, lightmapTilingOffset.w);
            Vector2 lhs2 = lhs1 + new Vector2(lightmapTilingOffset.x, lightmapTilingOffset.y);
            lhs1 = Vector2.Max(lhs1, Vector2.zero);
            Vector2 vector2 = Vector2.Min(lhs2, Vector2.one);
            float num1 = 1f - lhs1.y;
            lhs1.y = 1f - vector2.y;
            vector2.y = num1;
            Rect rect2 = new Rect(lhs1.x, lhs1.y, vector2.x - lhs1.x, vector2.y - lhs1.y);
            rect2.x -= Mathf.Clamp(rect2.height - rect2.width, 0.0f, float.MaxValue) / 2f;
            rect2.y -= Mathf.Clamp(rect2.width - rect2.height, 0.0f, float.MaxValue) / 2f;
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            Rect& local = @rect2;
            float num2 = Mathf.Max(rect2.width, rect2.height);
            rect2.height = num2;
            double num3 = (double) num2;
            // ISSUE: explicit reference operation
            (^local).width = (float) num3;
            if (flag && this.m_HasSeparateIndirectUV)
              rect2.x += 0.5f;
            this.m_ZoomablePreview.shownArea = rect2;
            Event.current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Texture2D texture = texture2DList[index];
          if ((bool) ((UnityEngine.Object) texture) && Event.current.type == EventType.Repaint)
          {
            Rect position3 = new Rect(this.ScaleRectByZoomableArea(this.CenterToRect(this.ResizeRectToFit(new Rect(0.0f, 0.0f, (float) texture.width, (float) texture.height), rect1), rect1), this.m_ZoomablePreview));
            position3.x += 3f;
            position3.y += rect1.y + 20f;
            Rect drawableArea = new Rect(rect1);
            drawableArea.y += position2.height + 3f;
            float num = drawableArea.y - 14f;
            position3.y -= num;
            drawableArea.y -= num;
            UnityEngine.FilterMode filterMode = texture.filterMode;
            texture.filterMode = UnityEngine.FilterMode.Point;
            GITextureType previewTextureType = this.kObjectPreviewTextureTypes[index];
            bool drawSpecularUV = flag && this.m_HasSeparateIndirectUV;
            LightmapVisualizationUtility.DrawTextureWithUVOverlay(texture, Selection.activeGameObject, drawableArea, position3, previewTextureType, drawSpecularUV);
            texture.filterMode = filterMode;
            break;
          }
          break;
      }
      if (this.m_PreviousSelection != Selection.activeInstanceID)
      {
        this.m_PreviousSelection = Selection.activeInstanceID;
        this.m_ZoomablePreview.SetShownHRange(0.0f, 1f);
        this.m_ZoomablePreview.SetShownVRange(0.0f, 1f);
      }
      Rect rect3 = new Rect(r);
      rect3.yMin += position2.height;
      this.m_ZoomablePreview.rect = rect3;
      this.m_ZoomablePreview.BeginViewGUI();
      this.m_ZoomablePreview.EndViewGUI();
      GUILayoutUtility.GetRect(r.width, r.height);
    }

    public bool EditLights()
    {
      GameObject[] gameObjects;
      Light[] selectedObjectsOfType = SceneModeUtility.GetSelectedObjectsOfType<Light>(out gameObjects);
      if (gameObjects.Length == 0)
        return false;
      EditorGUILayout.InspectorTitlebar((UnityEngine.Object[]) selectedObjectsOfType);
      this.GetLightEditor(selectedObjectsOfType).OnInspectorGUI();
      GUILayout.Space(10f);
      return true;
    }

    public bool EditLightmapParameters()
    {
      UnityEngine.Object[] filtered = Selection.GetFiltered(typeof (LightmapParameters), SelectionMode.Unfiltered);
      if (filtered.Length == 0)
        return false;
      EditorGUILayout.MultiSelectionObjectTitleBar(filtered);
      this.GetLightmapParametersEditor(filtered).OnInspectorGUI();
      GUILayout.Space(10f);
      return true;
    }

    public bool EditTerrains()
    {
      GameObject[] gameObjects;
      Terrain[] selectedObjectsOfType = SceneModeUtility.GetSelectedObjectsOfType<Terrain>(out gameObjects);
      if (gameObjects.Length == 0)
        return false;
      EditorGUILayout.InspectorTitlebar((UnityEngine.Object[]) selectedObjectsOfType);
      SerializedObject serializedObject = new SerializedObject((UnityEngine.Object[]) gameObjects);
      EditorGUI.BeginDisabledGroup(!SceneModeUtility.StaticFlagField("Lightmap Static", serializedObject.FindProperty("m_StaticEditorFlags"), 1));
      if (GUI.enabled)
        this.ShowTerrainChunks(selectedObjectsOfType);
      SerializedObject so = new SerializedObject((UnityEngine.Object[]) ((IEnumerable<Terrain>) selectedObjectsOfType).ToArray<Terrain>());
      float lightmapScale = this.LightmapScaleGUI(so, 1f);
      TerrainData terrainData = selectedObjectsOfType[0].terrainData;
      float cachedSurfaceArea = !((UnityEngine.Object) terrainData != (UnityEngine.Object) null) ? 0.0f : terrainData.size.x * terrainData.size.z;
      this.ShowClampedSizeInLightmapGUI(lightmapScale, cachedSurfaceArea);
      LightingWindowObjectTab.LightmapParametersGUI(so.FindProperty("m_LightmapParameters"), LightingWindowObjectTab.s_Styles.LightmapParameters);
      if (GUI.enabled && selectedObjectsOfType.Length == 1 && (UnityEngine.Object) selectedObjectsOfType[0].terrainData != (UnityEngine.Object) null)
        this.ShowBakePerformanceWarning(so, selectedObjectsOfType[0]);
      this.m_ShowBakedLM = EditorGUILayout.Foldout(this.m_ShowBakedLM, LightingWindowObjectTab.s_Styles.Atlas);
      if (this.m_ShowBakedLM)
        this.ShowAtlasGUI(so);
      this.m_ShowRealtimeLM = EditorGUILayout.Foldout(this.m_ShowRealtimeLM, LightingWindowObjectTab.s_Styles.RealtimeLM);
      if (this.m_ShowRealtimeLM)
        this.ShowRealtimeLMGUI(selectedObjectsOfType[0]);
      serializedObject.ApplyModifiedProperties();
      so.ApplyModifiedProperties();
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(10f);
      return true;
    }

    public bool EditRenderers()
    {
      GameObject[] gameObjects;
      Renderer[] selectedObjectsOfType = SceneModeUtility.GetSelectedObjectsOfType<Renderer>(out gameObjects, typeof (MeshRenderer), typeof (SkinnedMeshRenderer));
      if (gameObjects.Length == 0)
        return false;
      EditorGUILayout.InspectorTitlebar((UnityEngine.Object[]) selectedObjectsOfType);
      SerializedObject serializedObject = new SerializedObject((UnityEngine.Object[]) gameObjects);
      EditorGUI.BeginDisabledGroup(!SceneModeUtility.StaticFlagField("Lightmap Static", serializedObject.FindProperty("m_StaticEditorFlags"), 1));
      SerializedObject so = new SerializedObject((UnityEngine.Object[]) selectedObjectsOfType);
      float num = LightmapVisualization.GetLightmapLODLevelScale(selectedObjectsOfType[0]);
      for (int index = 1; index < selectedObjectsOfType.Length; ++index)
      {
        if (!Mathf.Approximately(num, LightmapVisualization.GetLightmapLODLevelScale(selectedObjectsOfType[index])))
          num = 1f;
      }
      this.ShowClampedSizeInLightmapGUI(this.LightmapScaleGUI(so, num) * LightmapVisualization.GetLightmapLODLevelScale(selectedObjectsOfType[0]), !(selectedObjectsOfType[0] is MeshRenderer) ? InternalMeshUtil.GetCachedSkinnedMeshSurfaceArea(selectedObjectsOfType[0] as SkinnedMeshRenderer) : InternalMeshUtil.GetCachedMeshSurfaceArea(selectedObjectsOfType[0] as MeshRenderer));
      EditorGUILayout.PropertyField(so.FindProperty("m_ImportantGI"), LightingWindowObjectTab.s_Styles.ImportantGI, new GUILayoutOption[0]);
      LightingWindowObjectTab.LightmapParametersGUI(so.FindProperty("m_LightmapParameters"), LightingWindowObjectTab.s_Styles.LightmapParameters);
      GUILayout.Space(10f);
      this.RendererUVSettings(so);
      GUILayout.Space(10f);
      this.m_ShowBakedLM = EditorGUILayout.Foldout(this.m_ShowBakedLM, LightingWindowObjectTab.s_Styles.Atlas);
      if (this.m_ShowBakedLM)
        this.ShowAtlasGUI(so);
      this.m_ShowRealtimeLM = EditorGUILayout.Foldout(this.m_ShowRealtimeLM, LightingWindowObjectTab.s_Styles.RealtimeLM);
      if (this.m_ShowRealtimeLM)
        this.ShowRealtimeLMGUI(so, selectedObjectsOfType[0]);
      if (LightmapEditorSettings.HasZeroAreaMesh(selectedObjectsOfType[0]))
        EditorGUILayout.HelpBox(LightingWindowObjectTab.s_Styles.ZeroAreaPackingMesh.text, MessageType.Warning);
      if (LightmapEditorSettings.HasClampedResolution(selectedObjectsOfType[0]))
        EditorGUILayout.HelpBox(LightingWindowObjectTab.s_Styles.ClampedPackingResolution.text, MessageType.Warning);
      if (!LightingWindowObjectTab.HasNormals(selectedObjectsOfType[0]))
        EditorGUILayout.HelpBox(LightingWindowObjectTab.s_Styles.NoNormalsNoLightmapping.text, MessageType.Warning);
      serializedObject.ApplyModifiedProperties();
      so.ApplyModifiedProperties();
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(10f);
      return true;
    }

    public void ObjectSettings()
    {
      if (LightingWindowObjectTab.s_Styles == null)
        LightingWindowObjectTab.s_Styles = new LightingWindowObjectTab.Styles();
      SceneModeUtility.SearchBar(typeof (Light), typeof (Renderer), typeof (Terrain));
      EditorGUILayout.Space();
      if (false | this.EditRenderers() | this.EditLightmapParameters() | this.EditLights() | this.EditTerrains())
        return;
      GUILayout.Label(LightingWindowObjectTab.s_Styles.EmptySelection, EditorStyles.helpBox, new GUILayoutOption[0]);
    }

    private Rect ResizeRectToFit(Rect rect, Rect to)
    {
      float num = Mathf.Min(to.width / rect.width, to.height / rect.height);
      float width = (float) (int) Mathf.Round(rect.width * num);
      float height = (float) (int) Mathf.Round(rect.height * num);
      return new Rect(rect.x, rect.y, width, height);
    }

    private Rect CenterToRect(Rect rect, Rect to)
    {
      float num1 = Mathf.Clamp((float) (int) ((double) to.width - (double) rect.width) / 2f, 0.0f, (float) int.MaxValue);
      float num2 = Mathf.Clamp((float) (int) ((double) to.height - (double) rect.height) / 2f, 0.0f, (float) int.MaxValue);
      return new Rect(rect.x + num1, rect.y + num2, rect.width, rect.height);
    }

    private Rect ScaleRectByZoomableArea(Rect rect, ZoomableArea zoomableArea)
    {
      float num1 = (float) -((double) zoomableArea.shownArea.x / (double) zoomableArea.shownArea.width) * rect.width;
      float num2 = (zoomableArea.shownArea.y - (1f - zoomableArea.shownArea.height)) / zoomableArea.shownArea.height * rect.height;
      float width = rect.width / zoomableArea.shownArea.width;
      float height = rect.height / zoomableArea.shownArea.height;
      return new Rect(rect.x + num1, rect.y + num2, width, height);
    }

    private void RendererUVSettings(SerializedObject so)
    {
      SerializedProperty property1 = so.FindProperty("m_PreserveUVs");
      EditorGUILayout.PropertyField(property1, LightingWindowObjectTab.s_Styles.PreserveUVs, new GUILayoutOption[0]);
      EditorGUI.BeginDisabledGroup(property1.boolValue);
      SerializedProperty property2 = so.FindProperty("m_AutoUVMaxDistance");
      EditorGUILayout.PropertyField(property2, LightingWindowObjectTab.s_Styles.AutoUVMaxDistance, new GUILayoutOption[0]);
      if ((double) property2.floatValue < 0.0)
        property2.floatValue = 0.0f;
      EditorGUILayout.Slider(so.FindProperty("m_AutoUVMaxAngle"), 0.0f, 180f, LightingWindowObjectTab.s_Styles.AutoUVMaxAngle, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      EditorGUILayout.PropertyField(so.FindProperty("m_IgnoreNormalsForChartDetection"), LightingWindowObjectTab.s_Styles.IgnoreNormalsForChartDetection, new GUILayoutOption[0]);
      EditorGUILayout.IntPopup(so.FindProperty("m_MinimumChartSize"), LightingWindowObjectTab.s_Styles.MinimumChartSizeStrings, LightingWindowObjectTab.s_Styles.MinimumChartSizeValues, LightingWindowObjectTab.s_Styles.MinimumChartSize, new GUILayoutOption[0]);
    }

    private void ShowClampedSizeInLightmapGUI(float lightmapScale, float cachedSurfaceArea)
    {
      this.m_ShowClampedSize.target = (double) (Mathf.Sqrt(cachedSurfaceArea) * LightmapEditorSettings.bakeResolution * lightmapScale) > (double) Math.Min(LightmapEditorSettings.maxAtlasWidth, LightmapEditorSettings.maxAtlasHeight);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowClampedSize.faded))
        GUILayout.Label(LightingWindowObjectTab.s_Styles.ClampedSize, EditorStyles.helpBox, new GUILayoutOption[0]);
      EditorGUILayout.EndFadeGroup();
    }

    private float LightmapScaleGUI(SerializedObject so, float lodScale)
    {
      SerializedProperty property = so.FindProperty("m_ScaleInLightmap");
      float num1 = lodScale * property.floatValue;
      Rect controlRect = EditorGUILayout.GetControlRect();
      EditorGUI.BeginProperty(controlRect, LightingWindowObjectTab.s_Styles.ScaleInLightmap, property);
      EditorGUI.BeginChangeCheck();
      float num2 = EditorGUI.FloatField(controlRect, LightingWindowObjectTab.s_Styles.ScaleInLightmap, num1);
      if (EditorGUI.EndChangeCheck())
        property.floatValue = Mathf.Max(num2 / lodScale, 0.0f);
      EditorGUI.EndProperty();
      return num2;
    }

    private void ShowAtlasGUI(SerializedObject so)
    {
      ++EditorGUI.indentLevel;
      EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.AtlasIndex, new GUIContent(so.FindProperty("m_LightmapIndex").intValue.ToString()), new GUILayoutOption[0]);
      EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.AtlasTilingX, new GUIContent(so.FindProperty("m_LightmapTilingOffset.x").floatValue.ToString()), new GUILayoutOption[0]);
      EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.AtlasTilingY, new GUIContent(so.FindProperty("m_LightmapTilingOffset.y").floatValue.ToString()), new GUILayoutOption[0]);
      EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.AtlasOffsetX, new GUIContent(so.FindProperty("m_LightmapTilingOffset.z").floatValue.ToString()), new GUILayoutOption[0]);
      EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.AtlasOffsetY, new GUIContent(so.FindProperty("m_LightmapTilingOffset.w").floatValue.ToString()), new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
    }

    private void ShowRealtimeLMGUI(SerializedObject so, Renderer renderer)
    {
      ++EditorGUI.indentLevel;
      Hash128 instanceHash;
      if (LightmapEditorSettings.GetInstanceHash(renderer, out instanceHash))
        EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.RealtimeLMInstanceHash, new GUIContent(instanceHash.ToString()), new GUILayoutOption[0]);
      Hash128 geometryHash;
      if (LightmapEditorSettings.GetGeometryHash(renderer, out geometryHash))
        EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.RealtimeLMGeometryHash, new GUIContent(geometryHash.ToString()), new GUILayoutOption[0]);
      int width1;
      int height1;
      if (LightmapEditorSettings.GetInstanceResolution(renderer, out width1, out height1))
        EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.RealtimeLMInstanceResolution, new GUIContent(width1.ToString() + "x" + height1.ToString()), new GUILayoutOption[0]);
      Hash128 inputSystemHash;
      if (LightmapEditorSettings.GetInputSystemHash(renderer, out inputSystemHash))
        EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.RealtimeLMInputSystemHash, new GUIContent(inputSystemHash.ToString()), new GUILayoutOption[0]);
      int width2;
      int height2;
      if (LightmapEditorSettings.GetSystemResolution(renderer, out width2, out height2))
        EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.RealtimeLMResolution, new GUIContent(width2.ToString() + "x" + height2.ToString()), new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
    }

    private static bool HasNormals(Renderer renderer)
    {
      Mesh mesh = (Mesh) null;
      if (renderer is MeshRenderer)
      {
        MeshFilter component = renderer.GetComponent<MeshFilter>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          mesh = component.sharedMesh;
      }
      else if (renderer is SkinnedMeshRenderer)
        mesh = (renderer as SkinnedMeshRenderer).sharedMesh;
      return InternalMeshUtil.HasNormals(mesh);
    }

    private void ShowTerrainChunks(Terrain[] terrains)
    {
      int num1 = 0;
      int num2 = 0;
      foreach (Terrain terrain in terrains)
      {
        int numChunksX = 0;
        int numChunksY = 0;
        Lightmapping.GetTerrainGIChunks(terrain, ref numChunksX, ref numChunksY);
        if (num1 == 0 && num2 == 0)
        {
          num1 = numChunksX;
          num2 = numChunksY;
        }
        else if (num1 != numChunksX || num2 != numChunksY)
        {
          num1 = num2 = 0;
          break;
        }
      }
      if (num1 * num2 <= 1)
        return;
      GUILayout.Label(string.Format("Terrain is chunked up into {0} instances for baking.", (object) (num1 * num2)), EditorStyles.helpBox, new GUILayoutOption[0]);
    }

    private void ShowBakePerformanceWarning(SerializedObject so, Terrain terrain)
    {
      float x = terrain.terrainData.size.x;
      float z = terrain.terrainData.size.z;
      LightmapParameters lightmapParameters = (LightmapParameters) so.FindProperty("m_LightmapParameters").objectReferenceValue ?? new LightmapParameters();
      float num1 = x * lightmapParameters.resolution * LightmapEditorSettings.resolution;
      float num2 = z * lightmapParameters.resolution * LightmapEditorSettings.resolution;
      if ((double) num1 > 512.0 || (double) num2 > 512.0)
        EditorGUILayout.HelpBox("Baking resolution for this terrain probably is TOO HIGH. Try use a lower resolution parameter set otherwise it may take long or even infinite time to bake and memory consumption during baking may get greatly increased as well.", MessageType.Warning);
      float num3 = num1 * lightmapParameters.clusterResolution;
      float num4 = num2 * lightmapParameters.clusterResolution;
      if ((double) ((float) terrain.terrainData.heightmapResolution / num3) <= 51.2000007629395 && (double) ((float) terrain.terrainData.heightmapResolution / num4) <= 51.2000007629395)
        return;
      EditorGUILayout.HelpBox("Baking resolution for this terrain probably is TOO LOW. If it takes long time in Clustering stage, try use a higher resolution parameter set.", MessageType.Warning);
    }

    private void ShowRealtimeLMGUI(Terrain terrain)
    {
      ++EditorGUI.indentLevel;
      int width;
      int height;
      int numChunksInX;
      int numChunksInY;
      if (LightmapEditorSettings.GetTerrainSystemResolution(terrain, out width, out height, out numChunksInX, out numChunksInY))
      {
        string text = width.ToString() + "x" + height.ToString();
        if (numChunksInX > 1 || numChunksInY > 1)
          text += string.Format(" ({0}x{1} chunks)", (object) numChunksInX, (object) numChunksInY);
        EditorGUILayout.LabelField(LightingWindowObjectTab.s_Styles.RealtimeLMResolution, new GUIContent(text), new GUILayoutOption[0]);
      }
      --EditorGUI.indentLevel;
    }

    public static bool LightmapParametersGUI(SerializedProperty prop, GUIContent content)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUIInternal.AssetPopup<LightmapParameters>(prop, content, "giparams");
      EditorGUI.BeginDisabledGroup(prop.objectReferenceValue == (UnityEngine.Object) null);
      bool flag = false;
      if (GUILayout.Button("Edit...", EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
      {
        Selection.activeObject = prop.objectReferenceValue;
        flag = true;
      }
      EditorGUI.EndDisabledGroup();
      EditorGUILayout.EndHorizontal();
      return flag;
    }

    private class Styles
    {
      public GUIContent PreserveUVs = EditorGUIUtility.TextContent("Preserve UVs|Preserve the incoming lightmap UVs when generating realtime GI UVs. The incoming UVs are packed but charts are not scaled or merged. This is necessary for correct edge stitching of axis aligned chart edges.");
      public GUIContent IgnoreNormalsForChartDetection = EditorGUIUtility.TextContent("Ignore Normals|Do not compare normals when detecting charts for realtime GI. This can be necessary when using hand authored UVs to avoid unnecessary chart splits.");
      public int[] MinimumChartSizeValues = new int[2]{ 2, 4 };
      public GUIContent[] MinimumChartSizeStrings = new GUIContent[2]{ EditorGUIUtility.TextContent("2 (Minimum)"), EditorGUIUtility.TextContent("4 (Stitchable)") };
      public GUIContent MinimumChartSize = EditorGUIUtility.TextContent("Min Chart Size|Directionality is generated at half resolution so in order to stitch properly at least 4x4 texels are needed in a chart so that a gradient can be generated on all sides of the chart. If stitching is not needed set this value to 2 in order to save texels for better performance at runtime and a faster lighting build.");
      public GUIContent ImportantGI = EditorGUIUtility.TextContent("Important GI|Make all other objects dependent upon this object. Useful for objects that will be strongly emissive to make sure that other objects will be illuminated by it.");
      public GUIContent AutoUVMaxDistance = EditorGUIUtility.TextContent("Auto UV Max Distance|Enlighten automatically generates simplified UVs by merging UV charts. Charts will only be simplified if the worldspace distance between the charts is smaller than this value.");
      public GUIContent AutoUVMaxAngle = EditorGUIUtility.TextContent("Auto UV Max Angle|Enlighten automatically generates simplified UVs by merging UV charts. Charts will only be simplified if the angle between the charts is smaller than this value.");
      public GUIContent LightmapParameters = EditorGUIUtility.TextContent("Advanced Parameters|Lets you configure per instance lightmapping parameters. Objects will be automatically grouped by unique parameter sets.");
      public GUIContent AtlasTilingX = EditorGUIUtility.TextContent("Tiling X");
      public GUIContent AtlasTilingY = EditorGUIUtility.TextContent("Tiling Y");
      public GUIContent AtlasOffsetX = EditorGUIUtility.TextContent("Offset X");
      public GUIContent AtlasOffsetY = EditorGUIUtility.TextContent("Offset Y");
      public GUIContent ClampedSize = EditorGUIUtility.TextContent("Object's size in lightmap has reached the max atlas size.|If you need higher resolution for this object, divide it into smaller meshes or set higher max atlas size via the LightmapEditorSettings class.");
      public GUIContent ClampedPackingResolution = EditorGUIUtility.TextContent("Object's size in the realtime lightmap has reached the maximum size.|If you need higher resolution for this object, divide it into smaller meshes.");
      public GUIContent ZeroAreaPackingMesh = EditorGUIUtility.TextContent("Mesh used by the renderer has zero UV or surface area. Non zero area is required for lightmapping.");
      public GUIContent NoNormalsNoLightmapping = EditorGUIUtility.TextContent("Mesh used by the renderer doesn't have normals. Normals are needed for lightmapping.");
      public GUIContent Atlas = EditorGUIUtility.TextContent("Baked Lightmap");
      public GUIContent RealtimeLM = EditorGUIUtility.TextContent("Realtime Lightmap");
      public GUIContent ChunkSize = EditorGUIUtility.TextContent("Chunk Size");
      public GUIContent EmptySelection = EditorGUIUtility.TextContent("Select a Light, Mesh Renderer or a Terrain from the scene.");
      public GUIContent ScaleInLightmap = EditorGUIUtility.TextContent("Scale In Lightmap|Object's surface multiplied by this value determines it's size in the lightmap. 0 - don't lightmap this object.");
      public GUIContent TerrainLightmapSize = EditorGUIUtility.TextContent("Lightmap Size|Defines the size of the lightmap that will be used only by this terrain.");
      public GUIContent AtlasIndex = EditorGUIUtility.TextContent("Lightmap Index");
      public GUIContent RealtimeLMResolution = EditorGUIUtility.TextContent("System Resolution|The resolution in texels of the realtime lightmap that this renderer belongs to.");
      public GUIContent RealtimeLMInstanceResolution = EditorGUIUtility.TextContent("Instance Resolution|The resolution in texels of the realtime lightmap packed instance.");
      public GUIContent RealtimeLMInputSystemHash = EditorGUIUtility.TextContent("System Hash|The hash of the realtime system that the renderer belongs to.");
      public GUIContent RealtimeLMInstanceHash = EditorGUIUtility.TextContent("Instance Hash|The hash of the realtime GI instance.");
      public GUIContent RealtimeLMGeometryHash = EditorGUIUtility.TextContent("Geometry Hash|The hash of the realtime GI geometry that the renderer is using.");
    }
  }
}
