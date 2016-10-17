// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (Terrain))]
  internal class TerrainInspector : Editor
  {
    internal static PrefKey[] s_ToolKeys = new PrefKey[6]{ new PrefKey("Terrain/Raise Height", "f1"), new PrefKey("Terrain/Set Height", "f2"), new PrefKey("Terrain/Smooth Height", "f3"), new PrefKey("Terrain/Texture Paint", "f4"), new PrefKey("Terrain/Tree Brush", "f5"), new PrefKey("Terrain/Detail Brush", "f6") };
    internal static PrefKey s_PrevBrush = new PrefKey("Terrain/Previous Brush", ",");
    internal static PrefKey s_NextBrush = new PrefKey("Terrain/Next Brush", ".");
    internal static PrefKey s_PrevTexture = new PrefKey("Terrain/Previous Detail", "#,");
    internal static PrefKey s_NextTexture = new PrefKey("Terrain/Next Detail", "#.");
    private static Texture2D[] s_BrushTextures = (Texture2D[]) null;
    private static int s_TerrainEditorHash = "TerrainEditor".GetHashCode();
    private SavedFloat m_TargetHeight = new SavedFloat("TerrainBrushTargetHeight", 0.2f);
    private SavedFloat m_Strength = new SavedFloat("TerrainBrushStrength", 0.5f);
    private SavedInt m_Size = new SavedInt("TerrainBrushSize", 25);
    private SavedFloat m_SplatAlpha = new SavedFloat("TerrainBrushSplatAlpha", 1f);
    private SavedFloat m_DetailOpacity = new SavedFloat("TerrainDetailOpacity", 1f);
    private SavedFloat m_DetailStrength = new SavedFloat("TerrainDetailStrength", 0.8f);
    private List<ReflectionProbeBlendInfo> m_BlendInfoList = new List<ReflectionProbeBlendInfo>();
    private AnimBool m_ShowBuiltinSpecularSettings = new AnimBool();
    private AnimBool m_ShowCustomMaterialSettings = new AnimBool();
    private AnimBool m_ShowReflectionProbesGUI = new AnimBool();
    private SavedInt m_SelectedTool = new SavedInt("TerrainSelectedTool", 0);
    private const float kHeightmapBrushScale = 0.01f;
    private const float kMinBrushStrength = 0.001678493f;
    private static TerrainInspector.Styles styles;
    private Terrain m_Terrain;
    private Texture2D[] m_SplatIcons;
    private GUIContent[] m_TreeContents;
    private GUIContent[] m_DetailContents;
    private Brush m_CachedBrush;
    private int m_SelectedBrush;
    private int m_SelectedSplat;
    private int m_SelectedDetail;
    private bool m_LODTreePrototypePresent;

    private TerrainTool selectedTool
    {
      get
      {
        if (Tools.current == Tool.None)
          return (TerrainTool) this.m_SelectedTool.value;
        return TerrainTool.None;
      }
      set
      {
        if (value != TerrainTool.None)
          Tools.current = Tool.None;
        this.m_SelectedTool.value = (int) value;
      }
    }

    private static float PercentSlider(GUIContent content, float valueInPercent, float minVal, float maxVal)
    {
      bool changed = GUI.changed;
      GUI.changed = false;
      float num = EditorGUILayout.Slider(content, Mathf.Round(valueInPercent * 100f), minVal * 100f, maxVal * 100f, new GUILayoutOption[0]);
      if (GUI.changed)
        return num / 100f;
      GUI.changed = changed;
      return valueInPercent;
    }

    private void CheckKeys()
    {
      for (int index = 0; index < TerrainInspector.s_ToolKeys.Length; ++index)
      {
        if (TerrainInspector.s_ToolKeys[index].activated)
        {
          this.selectedTool = (TerrainTool) index;
          this.Repaint();
          Event.current.Use();
        }
      }
      if (TerrainInspector.s_PrevBrush.activated)
      {
        --this.m_SelectedBrush;
        if (this.m_SelectedBrush < 0)
          this.m_SelectedBrush = TerrainInspector.s_BrushTextures.Length - 1;
        this.Repaint();
        Event.current.Use();
      }
      if (TerrainInspector.s_NextBrush.activated)
      {
        ++this.m_SelectedBrush;
        if (this.m_SelectedBrush >= TerrainInspector.s_BrushTextures.Length)
          this.m_SelectedBrush = 0;
        this.Repaint();
        Event.current.Use();
      }
      int num = 0;
      if (TerrainInspector.s_NextTexture.activated)
        num = 1;
      if (TerrainInspector.s_PrevTexture.activated)
        num = -1;
      if (num == 0)
        return;
      switch (this.selectedTool)
      {
        case TerrainTool.PaintTexture:
          this.m_SelectedSplat = (int) Mathf.Repeat((float) (this.m_SelectedSplat + num), (float) this.m_Terrain.terrainData.splatPrototypes.Length);
          Event.current.Use();
          this.Repaint();
          break;
        case TerrainTool.PlaceTree:
          if (TreePainter.selectedTree >= 0)
            TreePainter.selectedTree = (int) Mathf.Repeat((float) (TreePainter.selectedTree + num), (float) this.m_TreeContents.Length);
          else if (num == -1 && this.m_TreeContents.Length > 0)
            TreePainter.selectedTree = this.m_TreeContents.Length - 1;
          else if (num == 1 && this.m_TreeContents.Length > 0)
            TreePainter.selectedTree = 0;
          Event.current.Use();
          this.Repaint();
          break;
        case TerrainTool.PaintDetail:
          this.m_SelectedDetail = (int) Mathf.Repeat((float) (this.m_SelectedDetail + num), (float) this.m_Terrain.terrainData.detailPrototypes.Length);
          Event.current.Use();
          this.Repaint();
          break;
      }
    }

    private void LoadBrushIcons()
    {
      ArrayList arrayList = new ArrayList();
      int num1 = 1;
      Texture texture1;
      do
      {
        texture1 = (Texture) EditorGUIUtility.Load(EditorResourcesUtility.brushesPath + "builtin_brush_" + (object) num1 + ".png");
        if ((bool) ((UnityEngine.Object) texture1))
          arrayList.Add((object) texture1);
        ++num1;
      }
      while ((bool) ((UnityEngine.Object) texture1));
      int num2 = 0;
      Texture texture2;
      do
      {
        texture2 = (Texture) EditorGUIUtility.FindTexture("brush_" + (object) num2 + ".png");
        if ((bool) ((UnityEngine.Object) texture2))
          arrayList.Add((object) texture2);
        ++num2;
      }
      while ((bool) ((UnityEngine.Object) texture2));
      TerrainInspector.s_BrushTextures = arrayList.ToArray(typeof (Texture2D)) as Texture2D[];
    }

    private void Initialize()
    {
      this.m_Terrain = this.target as Terrain;
      if (TerrainInspector.s_BrushTextures != null)
        return;
      this.LoadBrushIcons();
    }

    public void OnEnable()
    {
      this.m_ShowBuiltinSpecularSettings.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCustomMaterialSettings.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowReflectionProbesGUI.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      Terrain target = this.target as Terrain;
      if (!((UnityEngine.Object) target != (UnityEngine.Object) null))
        return;
      this.m_ShowBuiltinSpecularSettings.value = target.materialType == Terrain.MaterialType.BuiltInLegacySpecular;
      this.m_ShowCustomMaterialSettings.value = target.materialType == Terrain.MaterialType.Custom;
      this.m_ShowReflectionProbesGUI.value = target.materialType == Terrain.MaterialType.BuiltInStandard || target.materialType == Terrain.MaterialType.Custom;
    }

    public void OnDisable()
    {
      this.m_ShowReflectionProbesGUI.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCustomMaterialSettings.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowBuiltinSpecularSettings.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      if (this.m_CachedBrush == null)
        return;
      this.m_CachedBrush.Dispose();
    }

    private static string IntString(float p)
    {
      return ((int) p).ToString();
    }

    public void MenuButton(GUIContent title, string menuName, int userData)
    {
      GUIContent content = new GUIContent(title.text, TerrainInspector.styles.settingsIcon, title.tooltip);
      Rect rect = GUILayoutUtility.GetRect(content, TerrainInspector.styles.largeSquare);
      if (!GUI.Button(rect, content, TerrainInspector.styles.largeSquare))
        return;
      MenuCommand command = new MenuCommand((UnityEngine.Object) this.m_Terrain, userData);
      EditorUtility.DisplayPopupMenu(new Rect(rect.x, rect.y, 0.0f, 0.0f), menuName, command);
    }

    public static int AspectSelectionGrid(int selected, Texture[] textures, int approxSize, GUIStyle style, string emptyString, out bool doubleClick)
    {
      GUILayout.BeginVertical((GUIStyle) "box", GUILayout.MinHeight(10f));
      int num1 = 0;
      doubleClick = false;
      if (textures.Length != 0)
      {
        float num2 = (float) ((Screen.width - 20) / approxSize);
        int num3 = (int) Mathf.Ceil((float) textures.Length / num2);
        Rect aspectRect = GUILayoutUtility.GetAspectRect(num2 / (float) num3);
        Event current = Event.current;
        if (current.type == EventType.MouseDown && current.clickCount == 2 && aspectRect.Contains(current.mousePosition))
        {
          doubleClick = true;
          current.Use();
        }
        num1 = GUI.SelectionGrid(aspectRect, selected, textures, (Screen.width - 20) / approxSize, style);
      }
      else
        GUILayout.Label(emptyString);
      GUILayout.EndVertical();
      return num1;
    }

    private static Rect GetBrushAspectRect(int elementCount, int approxSize, int extraLineHeight)
    {
      int num1 = (int) Mathf.Ceil((float) ((Screen.width - 20) / approxSize));
      int num2 = elementCount / num1;
      if (elementCount % num1 != 0)
        ++num2;
      Rect aspectRect = GUILayoutUtility.GetAspectRect((float) num1 / (float) num2);
      Rect rect = GUILayoutUtility.GetRect(10f, (float) (extraLineHeight * num2));
      aspectRect.height += rect.height;
      return aspectRect;
    }

    public static int AspectSelectionGridImageAndText(int selected, GUIContent[] textures, int approxSize, GUIStyle style, string emptyString, out bool doubleClick)
    {
      EditorGUILayout.BeginVertical(GUIContent.none, EditorStyles.helpBox, GUILayout.MinHeight(10f));
      int num = 0;
      doubleClick = false;
      if (textures.Length != 0)
      {
        Rect brushAspectRect = TerrainInspector.GetBrushAspectRect(textures.Length, approxSize, 12);
        Event current = Event.current;
        if (current.type == EventType.MouseDown && current.clickCount == 2 && brushAspectRect.Contains(current.mousePosition))
        {
          doubleClick = true;
          current.Use();
        }
        num = GUI.SelectionGrid(brushAspectRect, selected, textures, (int) Mathf.Ceil((float) ((Screen.width - 20) / approxSize)), style);
      }
      else
        GUILayout.Label(emptyString);
      GUILayout.EndVertical();
      return num;
    }

    private void LoadSplatIcons()
    {
      SplatPrototype[] splatPrototypes = this.m_Terrain.terrainData.splatPrototypes;
      this.m_SplatIcons = new Texture2D[splatPrototypes.Length];
      for (int index = 0; index < this.m_SplatIcons.Length; ++index)
        this.m_SplatIcons[index] = AssetPreview.GetAssetPreview((UnityEngine.Object) splatPrototypes[index].texture) ?? splatPrototypes[index].texture;
    }

    private void LoadTreeIcons()
    {
      TreePrototype[] treePrototypes = this.m_Terrain.terrainData.treePrototypes;
      this.m_TreeContents = new GUIContent[treePrototypes.Length];
      for (int index = 0; index < this.m_TreeContents.Length; ++index)
      {
        this.m_TreeContents[index] = new GUIContent();
        Texture assetPreview = (Texture) AssetPreview.GetAssetPreview((UnityEngine.Object) treePrototypes[index].prefab);
        if ((UnityEngine.Object) assetPreview != (UnityEngine.Object) null)
          this.m_TreeContents[index].image = assetPreview;
        this.m_TreeContents[index].text = !((UnityEngine.Object) treePrototypes[index].prefab != (UnityEngine.Object) null) ? "Missing" : treePrototypes[index].prefab.name;
      }
    }

    private void LoadDetailIcons()
    {
      DetailPrototype[] detailPrototypes = this.m_Terrain.terrainData.detailPrototypes;
      this.m_DetailContents = new GUIContent[detailPrototypes.Length];
      for (int index = 0; index < this.m_DetailContents.Length; ++index)
      {
        this.m_DetailContents[index] = new GUIContent();
        if (detailPrototypes[index].usePrototypeMesh)
        {
          Texture assetPreview = (Texture) AssetPreview.GetAssetPreview((UnityEngine.Object) detailPrototypes[index].prototype);
          if ((UnityEngine.Object) assetPreview != (UnityEngine.Object) null)
            this.m_DetailContents[index].image = assetPreview;
          this.m_DetailContents[index].text = !((UnityEngine.Object) detailPrototypes[index].prototype != (UnityEngine.Object) null) ? "Missing" : detailPrototypes[index].prototype.name;
        }
        else
        {
          Texture prototypeTexture = (Texture) detailPrototypes[index].prototypeTexture;
          if ((UnityEngine.Object) prototypeTexture != (UnityEngine.Object) null)
            this.m_DetailContents[index].image = prototypeTexture;
          this.m_DetailContents[index].text = !((UnityEngine.Object) prototypeTexture != (UnityEngine.Object) null) ? "Missing" : prototypeTexture.name;
        }
      }
    }

    public void ShowTrees()
    {
      this.LoadTreeIcons();
      GUI.changed = false;
      this.ShowUpgradeTreePrototypeScaleUI();
      GUILayout.Label(TerrainInspector.styles.trees, EditorStyles.boldLabel, new GUILayoutOption[0]);
      bool doubleClick;
      TreePainter.selectedTree = TerrainInspector.AspectSelectionGridImageAndText(TreePainter.selectedTree, this.m_TreeContents, 64, TerrainInspector.styles.gridListText, "No trees defined", out doubleClick);
      if (TreePainter.selectedTree >= this.m_TreeContents.Length)
        TreePainter.selectedTree = -1;
      if (doubleClick)
      {
        TerrainTreeContextMenus.EditTree(new MenuCommand((UnityEngine.Object) this.m_Terrain, TreePainter.selectedTree));
        GUIUtility.ExitGUI();
      }
      GUILayout.BeginHorizontal();
      this.ShowMassPlaceTrees();
      GUILayout.FlexibleSpace();
      this.MenuButton(TerrainInspector.styles.editTrees, "CONTEXT/TerrainEngineTrees", TreePainter.selectedTree);
      this.ShowRefreshPrototypes();
      GUILayout.EndHorizontal();
      if (TreePainter.selectedTree == -1)
        return;
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      TreePainter.brushSize = EditorGUILayout.Slider(TerrainInspector.styles.brushSize, TreePainter.brushSize, 1f, 100f, new GUILayoutOption[0]);
      float valueInPercent = (float) ((3.29999995231628 - (double) TreePainter.spacing) / 3.0);
      float num = TerrainInspector.PercentSlider(TerrainInspector.styles.treeDensity, valueInPercent, 0.1f, 1f);
      if ((double) num != (double) valueInPercent)
        TreePainter.spacing = (float) ((1.10000002384186 - (double) num) * 3.0);
      GUILayout.Space(5f);
      GUILayout.BeginHorizontal();
      GUILayout.Label(TerrainInspector.styles.treeHeight, new GUILayoutOption[1]
      {
        GUILayout.Width(EditorGUIUtility.labelWidth - 6f)
      });
      GUILayout.Label(TerrainInspector.styles.treeHeightRandomLabel, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      TreePainter.allowHeightVar = GUILayout.Toggle((TreePainter.allowHeightVar ? 1 : 0) != 0, TerrainInspector.styles.treeHeightRandomToggle, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      if (TreePainter.allowHeightVar)
      {
        EditorGUI.BeginChangeCheck();
        float minValue = TreePainter.treeHeight * (1f - TreePainter.treeHeightVariation);
        float maxValue = TreePainter.treeHeight * (1f + TreePainter.treeHeightVariation);
        EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0.01f, 2f);
        if (EditorGUI.EndChangeCheck())
        {
          TreePainter.treeHeight = (float) (((double) minValue + (double) maxValue) * 0.5);
          TreePainter.treeHeightVariation = (float) (((double) maxValue - (double) minValue) / ((double) minValue + (double) maxValue));
        }
      }
      else
      {
        TreePainter.treeHeight = EditorGUILayout.Slider(TreePainter.treeHeight, 0.01f, 2f);
        TreePainter.treeHeightVariation = 0.0f;
      }
      GUILayout.EndHorizontal();
      GUILayout.Space(5f);
      TreePainter.lockWidthToHeight = EditorGUILayout.Toggle(TerrainInspector.styles.lockWidth, TreePainter.lockWidthToHeight, new GUILayoutOption[0]);
      if (TreePainter.lockWidthToHeight)
      {
        TreePainter.treeWidth = TreePainter.treeHeight;
        TreePainter.treeWidthVariation = TreePainter.treeHeightVariation;
        TreePainter.allowWidthVar = TreePainter.allowHeightVar;
      }
      GUILayout.Space(5f);
      EditorGUI.BeginDisabledGroup(TreePainter.lockWidthToHeight);
      GUILayout.BeginHorizontal();
      GUILayout.Label(TerrainInspector.styles.treeWidth, new GUILayoutOption[1]
      {
        GUILayout.Width(EditorGUIUtility.labelWidth - 6f)
      });
      GUILayout.Label(TerrainInspector.styles.treeWidthRandomLabel, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      TreePainter.allowWidthVar = GUILayout.Toggle((TreePainter.allowWidthVar ? 1 : 0) != 0, TerrainInspector.styles.treeWidthRandomToggle, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(false)
      });
      if (TreePainter.allowWidthVar)
      {
        EditorGUI.BeginChangeCheck();
        float minValue = TreePainter.treeWidth * (1f - TreePainter.treeWidthVariation);
        float maxValue = TreePainter.treeWidth * (1f + TreePainter.treeWidthVariation);
        EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0.01f, 2f);
        if (EditorGUI.EndChangeCheck())
        {
          TreePainter.treeWidth = (float) (((double) minValue + (double) maxValue) * 0.5);
          TreePainter.treeWidthVariation = (float) (((double) maxValue - (double) minValue) / ((double) minValue + (double) maxValue));
        }
      }
      else
      {
        TreePainter.treeWidth = EditorGUILayout.Slider(TreePainter.treeWidth, 0.01f, 2f);
        TreePainter.treeWidthVariation = 0.0f;
      }
      GUILayout.EndHorizontal();
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(5f);
      if (TerrainEditorUtility.IsLODTreePrototype(this.m_Terrain.terrainData.treePrototypes[TreePainter.selectedTree].m_Prefab))
        TreePainter.randomRotation = EditorGUILayout.Toggle(TerrainInspector.styles.treeRotation, TreePainter.randomRotation, new GUILayoutOption[0]);
      else
        TreePainter.treeColorAdjustment = EditorGUILayout.Slider(TerrainInspector.styles.treeColorVar, TreePainter.treeColorAdjustment, 0.0f, 1f, new GUILayoutOption[0]);
    }

    public void ShowDetails()
    {
      this.LoadDetailIcons();
      this.ShowBrushes();
      GUI.changed = false;
      GUILayout.Label(TerrainInspector.styles.details, EditorStyles.boldLabel, new GUILayoutOption[0]);
      bool doubleClick;
      this.m_SelectedDetail = TerrainInspector.AspectSelectionGridImageAndText(this.m_SelectedDetail, this.m_DetailContents, 64, TerrainInspector.styles.gridListText, "No Detail Objects defined", out doubleClick);
      if (doubleClick)
      {
        TerrainDetailContextMenus.EditDetail(new MenuCommand((UnityEngine.Object) this.m_Terrain, this.m_SelectedDetail));
        GUIUtility.ExitGUI();
      }
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.MenuButton(TerrainInspector.styles.editDetails, "CONTEXT/TerrainEngineDetails", this.m_SelectedDetail);
      this.ShowRefreshPrototypes();
      GUILayout.EndHorizontal();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_Size.value = Mathf.RoundToInt(EditorGUILayout.Slider(TerrainInspector.styles.brushSize, (float) (int) this.m_Size, 1f, 100f, new GUILayoutOption[0]));
      this.m_DetailOpacity.value = EditorGUILayout.Slider(TerrainInspector.styles.opacity, (float) this.m_DetailOpacity, 0.0f, 1f, new GUILayoutOption[0]);
      this.m_DetailStrength.value = EditorGUILayout.Slider(TerrainInspector.styles.detailTargetStrength, (float) this.m_DetailStrength, 0.0f, 1f, new GUILayoutOption[0]);
      this.m_DetailStrength.value = Mathf.Round((float) this.m_DetailStrength * 16f) / 16f;
    }

    public void ShowSettings()
    {
      TerrainData terrainData = this.m_Terrain.terrainData;
      EditorGUI.BeginChangeCheck();
      GUILayout.Label("Base Terrain", EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_Terrain.drawHeightmap = EditorGUILayout.Toggle("Draw", this.m_Terrain.drawHeightmap, new GUILayoutOption[0]);
      this.m_Terrain.heightmapPixelError = EditorGUILayout.Slider("Pixel Error", this.m_Terrain.heightmapPixelError, 1f, 200f, new GUILayoutOption[0]);
      this.m_Terrain.basemapDistance = EditorGUILayout.Slider("Base Map Dist.", this.m_Terrain.basemapDistance, 0.0f, 2000f, new GUILayoutOption[0]);
      this.m_Terrain.castShadows = EditorGUILayout.Toggle("Cast Shadows", this.m_Terrain.castShadows, new GUILayoutOption[0]);
      this.m_Terrain.materialType = (Terrain.MaterialType) EditorGUILayout.EnumPopup("Material", (Enum) this.m_Terrain.materialType, new GUILayoutOption[0]);
      if (this.m_Terrain.materialType != Terrain.MaterialType.Custom)
        this.m_Terrain.materialTemplate = (Material) null;
      this.m_ShowBuiltinSpecularSettings.target = this.m_Terrain.materialType == Terrain.MaterialType.BuiltInLegacySpecular;
      this.m_ShowCustomMaterialSettings.target = this.m_Terrain.materialType == Terrain.MaterialType.Custom;
      this.m_ShowReflectionProbesGUI.target = this.m_Terrain.materialType == Terrain.MaterialType.BuiltInStandard || this.m_Terrain.materialType == Terrain.MaterialType.Custom;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowBuiltinSpecularSettings.faded))
      {
        ++EditorGUI.indentLevel;
        this.m_Terrain.legacySpecular = EditorGUILayout.ColorField("Specular Color", this.m_Terrain.legacySpecular, new GUILayoutOption[0]);
        this.m_Terrain.legacyShininess = EditorGUILayout.Slider("Shininess", this.m_Terrain.legacyShininess, 0.03f, 1f, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowCustomMaterialSettings.faded))
      {
        ++EditorGUI.indentLevel;
        this.m_Terrain.materialTemplate = EditorGUILayout.ObjectField("Custom Material", (UnityEngine.Object) this.m_Terrain.materialTemplate, typeof (Material), false, new GUILayoutOption[0]) as Material;
        if ((UnityEngine.Object) this.m_Terrain.materialTemplate != (UnityEngine.Object) null && ShaderUtil.HasTangentChannel(this.m_Terrain.materialTemplate.shader))
          EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("Can't use materials with shaders which need tangent geometry on terrain, use shaders in Nature/Terrain instead.").text, MessageType.Warning, false);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowReflectionProbesGUI.faded))
      {
        this.m_Terrain.reflectionProbeUsage = (ReflectionProbeUsage) EditorGUILayout.EnumPopup("Reflection Probes", (Enum) this.m_Terrain.reflectionProbeUsage, new GUILayoutOption[0]);
        if (this.m_Terrain.reflectionProbeUsage != ReflectionProbeUsage.Off)
        {
          ++EditorGUI.indentLevel;
          this.m_Terrain.GetClosestReflectionProbes(this.m_BlendInfoList);
          RendererEditorBase.Probes.ShowClosestReflectionProbes(this.m_BlendInfoList);
          --EditorGUI.indentLevel;
        }
      }
      EditorGUILayout.EndFadeGroup();
      terrainData.thickness = EditorGUILayout.FloatField("Thickness", terrainData.thickness, new GUILayoutOption[0]);
      GUILayout.Label("Tree & Detail Objects", EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_Terrain.drawTreesAndFoliage = EditorGUILayout.Toggle("Draw", this.m_Terrain.drawTreesAndFoliage, new GUILayoutOption[0]);
      this.m_Terrain.bakeLightProbesForTrees = EditorGUILayout.Toggle(TerrainInspector.styles.bakeLightProbesForTrees, this.m_Terrain.bakeLightProbesForTrees, new GUILayoutOption[0]);
      this.m_Terrain.detailObjectDistance = EditorGUILayout.Slider("Detail Distance", this.m_Terrain.detailObjectDistance, 0.0f, 250f, new GUILayoutOption[0]);
      this.m_Terrain.collectDetailPatches = EditorGUILayout.Toggle("Collect Detail Patches", this.m_Terrain.collectDetailPatches, new GUILayoutOption[0]);
      this.m_Terrain.detailObjectDensity = EditorGUILayout.Slider("Detail Density", this.m_Terrain.detailObjectDensity, 0.0f, 1f, new GUILayoutOption[0]);
      this.m_Terrain.treeDistance = EditorGUILayout.Slider("Tree Distance", this.m_Terrain.treeDistance, 0.0f, 2000f, new GUILayoutOption[0]);
      this.m_Terrain.treeBillboardDistance = EditorGUILayout.Slider("Billboard Start", this.m_Terrain.treeBillboardDistance, 5f, 2000f, new GUILayoutOption[0]);
      this.m_Terrain.treeCrossFadeLength = EditorGUILayout.Slider("Fade Length", this.m_Terrain.treeCrossFadeLength, 0.0f, 200f, new GUILayoutOption[0]);
      this.m_Terrain.treeMaximumFullLODCount = EditorGUILayout.IntSlider("Max Mesh Trees", this.m_Terrain.treeMaximumFullLODCount, 0, 10000, new GUILayoutOption[0]);
      if (Event.current.type == EventType.Layout)
      {
        this.m_LODTreePrototypePresent = false;
        for (int index = 0; index < terrainData.treePrototypes.Length; ++index)
        {
          if (TerrainEditorUtility.IsLODTreePrototype(terrainData.treePrototypes[index].prefab))
          {
            this.m_LODTreePrototypePresent = true;
            break;
          }
        }
      }
      if (this.m_LODTreePrototypePresent)
        EditorGUILayout.HelpBox("Tree Distance, Billboard Start, Fade Length and Max Mesh Trees have no effect on SpeedTree trees. Please use the LOD Group component on the tree prefab to control LOD settings.", MessageType.Info);
      if (EditorGUI.EndChangeCheck())
      {
        EditorApplication.SetSceneRepaintDirty();
        EditorUtility.SetDirty((UnityEngine.Object) this.m_Terrain);
        if (!EditorApplication.isPlaying)
          EditorSceneManager.MarkSceneDirty(this.m_Terrain.gameObject.scene);
      }
      EditorGUI.BeginChangeCheck();
      GUILayout.Label("Wind Settings for Grass", EditorStyles.boldLabel, new GUILayoutOption[0]);
      float num1 = EditorGUILayout.Slider("Speed", terrainData.wavingGrassStrength, 0.0f, 1f, new GUILayoutOption[0]);
      float num2 = EditorGUILayout.Slider("Size", terrainData.wavingGrassSpeed, 0.0f, 1f, new GUILayoutOption[0]);
      float num3 = EditorGUILayout.Slider("Bending", terrainData.wavingGrassAmount, 0.0f, 1f, new GUILayoutOption[0]);
      Color color = EditorGUILayout.ColorField("Grass Tint", terrainData.wavingGrassTint, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        terrainData.wavingGrassStrength = num1;
        terrainData.wavingGrassSpeed = num2;
        terrainData.wavingGrassAmount = num3;
        terrainData.wavingGrassTint = color;
        if (!EditorUtility.IsPersistent((UnityEngine.Object) terrainData) && !EditorApplication.isPlaying)
          EditorSceneManager.MarkSceneDirty(this.m_Terrain.gameObject.scene);
      }
      this.ShowResolution();
      this.ShowHeightmaps();
    }

    public void ShowRaiseHeight()
    {
      this.ShowBrushes();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.ShowBrushSettings();
    }

    public void ShowSmoothHeight()
    {
      this.ShowBrushes();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.ShowBrushSettings();
    }

    public void ShowTextures()
    {
      this.LoadSplatIcons();
      this.ShowBrushes();
      GUILayout.Label(TerrainInspector.styles.textures, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUI.changed = false;
      bool doubleClick;
      this.m_SelectedSplat = TerrainInspector.AspectSelectionGrid(this.m_SelectedSplat, (Texture[]) this.m_SplatIcons, 64, TerrainInspector.styles.gridList, "No terrain textures defined.", out doubleClick);
      if (doubleClick)
      {
        TerrainSplatContextMenus.EditSplat(new MenuCommand((UnityEngine.Object) this.m_Terrain, this.m_SelectedSplat));
        GUIUtility.ExitGUI();
      }
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.MenuButton(TerrainInspector.styles.editTextures, "CONTEXT/TerrainEngineSplats", this.m_SelectedSplat);
      GUILayout.EndHorizontal();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.ShowBrushSettings();
      this.m_SplatAlpha.value = EditorGUILayout.Slider("Target Strength", (float) this.m_SplatAlpha, 0.0f, 1f, new GUILayoutOption[0]);
    }

    public void ShowBrushes()
    {
      GUILayout.Label(TerrainInspector.styles.brushes, EditorStyles.boldLabel, new GUILayoutOption[0]);
      bool doubleClick;
      this.m_SelectedBrush = TerrainInspector.AspectSelectionGrid(this.m_SelectedBrush, (Texture[]) TerrainInspector.s_BrushTextures, 32, TerrainInspector.styles.gridList, "No brushes defined.", out doubleClick);
    }

    public void ShowHeightmaps()
    {
      GUILayout.Label(TerrainInspector.styles.heightmap, EditorStyles.boldLabel, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(TerrainInspector.styles.importRaw))
        TerrainMenus.ImportRaw();
      if (GUILayout.Button(TerrainInspector.styles.exportRaw))
        TerrainMenus.ExportHeightmapRaw();
      GUILayout.EndHorizontal();
    }

    public void ShowResolution()
    {
      GUILayout.Label("Resolution", EditorStyles.boldLabel, new GUILayoutOption[0]);
      float x1 = this.m_Terrain.terrainData.size.x;
      float y1 = this.m_Terrain.terrainData.size.y;
      float z1 = this.m_Terrain.terrainData.size.z;
      int heightmapResolution = this.m_Terrain.terrainData.heightmapResolution;
      int detailResolution = this.m_Terrain.terrainData.detailResolution;
      int resolutionPerPatch1 = this.m_Terrain.terrainData.detailResolutionPerPatch;
      int alphamapResolution = this.m_Terrain.terrainData.alphamapResolution;
      int baseMapResolution = this.m_Terrain.terrainData.baseMapResolution;
      EditorGUI.BeginChangeCheck();
      float x2 = EditorGUILayout.DelayedFloatField(EditorGUIUtility.TempContent("Terrain Width"), x1, new GUILayoutOption[0]);
      if ((double) x2 <= 0.0)
        x2 = 1f;
      if ((double) x2 > 100000.0)
        x2 = 100000f;
      float z2 = EditorGUILayout.DelayedFloatField(EditorGUIUtility.TempContent("Terrain Length"), z1, new GUILayoutOption[0]);
      if ((double) z2 <= 0.0)
        z2 = 1f;
      if ((double) z2 > 100000.0)
        z2 = 100000f;
      float y2 = EditorGUILayout.DelayedFloatField(EditorGUIUtility.TempContent("Terrain Height"), y1, new GUILayoutOption[0]);
      if ((double) y2 <= 0.0)
        y2 = 1f;
      if ((double) y2 > 10000.0)
        y2 = 10000f;
      int adjustedSize = this.m_Terrain.terrainData.GetAdjustedSize(Mathf.Clamp(EditorGUILayout.DelayedIntField(EditorGUIUtility.TempContent("Heightmap Resolution"), heightmapResolution, new GUILayoutOption[0]), 33, 4097));
      int resolution = Mathf.Clamp(EditorGUILayout.DelayedIntField(EditorGUIUtility.TempContent("Detail Resolution"), detailResolution, new GUILayoutOption[0]), 0, 4048);
      int resolutionPerPatch2 = Mathf.Clamp(EditorGUILayout.DelayedIntField(EditorGUIUtility.TempContent("Detail Resolution Per Patch"), resolutionPerPatch1, new GUILayoutOption[0]), 8, 128);
      int num1 = Mathf.Clamp(Mathf.ClosestPowerOfTwo(EditorGUILayout.DelayedIntField(EditorGUIUtility.TempContent("Control Texture Resolution"), alphamapResolution, new GUILayoutOption[0])), 16, 2048);
      int num2 = Mathf.Clamp(Mathf.ClosestPowerOfTwo(EditorGUILayout.DelayedIntField(EditorGUIUtility.TempContent("Base Texture Resolution"), baseMapResolution, new GUILayoutOption[0])), 16, 2048);
      if (EditorGUI.EndChangeCheck())
      {
        ArrayList arrayList = new ArrayList();
        arrayList.Add((object) this.m_Terrain.terrainData);
        arrayList.AddRange((ICollection) this.m_Terrain.terrainData.alphamapTextures);
        Undo.RegisterCompleteObjectUndo(arrayList.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[], "Set Resolution");
        if (this.m_Terrain.terrainData.heightmapResolution != adjustedSize)
          this.m_Terrain.terrainData.heightmapResolution = adjustedSize;
        this.m_Terrain.terrainData.size = new Vector3(x2, y2, z2);
        if (this.m_Terrain.terrainData.detailResolution != resolution || resolutionPerPatch2 != this.m_Terrain.terrainData.detailResolutionPerPatch)
          this.ResizeDetailResolution(this.m_Terrain.terrainData, resolution, resolutionPerPatch2);
        if (this.m_Terrain.terrainData.alphamapResolution != num1)
          this.m_Terrain.terrainData.alphamapResolution = num1;
        if (this.m_Terrain.terrainData.baseMapResolution != num2)
          this.m_Terrain.terrainData.baseMapResolution = num2;
        this.m_Terrain.Flush();
      }
      EditorGUILayout.HelpBox("Please note that modifying the resolution of the heightmap, detail map and control texture will clear their contents, respectively.", MessageType.Warning);
    }

    private void ResizeDetailResolution(TerrainData terrainData, int resolution, int resolutionPerPatch)
    {
      if (resolution == terrainData.detailResolution)
      {
        List<int[,]> numArrayList = new List<int[,]>();
        for (int layer = 0; layer < terrainData.detailPrototypes.Length; ++layer)
          numArrayList.Add(terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, layer));
        terrainData.SetDetailResolution(resolution, resolutionPerPatch);
        for (int layer = 0; layer < numArrayList.Count; ++layer)
          terrainData.SetDetailLayer(0, 0, layer, numArrayList[layer]);
      }
      else
        terrainData.SetDetailResolution(resolution, resolutionPerPatch);
    }

    public void ShowUpgradeTreePrototypeScaleUI()
    {
      if (!((UnityEngine.Object) this.m_Terrain.terrainData != (UnityEngine.Object) null) || !this.m_Terrain.terrainData.NeedUpgradeScaledTreePrototypes())
        return;
      GUIContent content = EditorGUIUtility.TempContent("Some of your prototypes have scaling values on the prefab. Since Unity 5.2 these scalings will be applied to terrain tree instances. Do you want to upgrade to this behaviour?", (Texture) EditorGUIUtility.GetHelpIcon(MessageType.Warning));
      GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
      GUILayout.Label(content, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
      GUILayout.Space(3f);
      if (GUILayout.Button("Upgrade", new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
      {
        this.m_Terrain.terrainData.UpgradeScaledTreePrototype();
        TerrainMenus.RefreshPrototypes();
      }
      GUILayout.Space(3f);
      GUILayout.EndVertical();
    }

    public void ShowRefreshPrototypes()
    {
      if (!GUILayout.Button(TerrainInspector.styles.refresh))
        return;
      TerrainMenus.RefreshPrototypes();
    }

    public void ShowMassPlaceTrees()
    {
      EditorGUI.BeginDisabledGroup(TreePainter.selectedTree == -1);
      if (GUILayout.Button(TerrainInspector.styles.massPlaceTrees))
        TerrainMenus.MassPlaceTrees();
      EditorGUI.EndDisabledGroup();
    }

    public void ShowBrushSettings()
    {
      this.m_Size.value = Mathf.RoundToInt(EditorGUILayout.Slider(TerrainInspector.styles.brushSize, (float) (int) this.m_Size, 1f, 100f, new GUILayoutOption[0]));
      this.m_Strength.value = TerrainInspector.PercentSlider(TerrainInspector.styles.opacity, (float) this.m_Strength, 0.001678493f, 1f);
    }

    public void ShowSetHeight()
    {
      this.ShowBrushes();
      GUILayout.Label(TerrainInspector.styles.settings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.ShowBrushSettings();
      GUILayout.BeginHorizontal();
      GUI.changed = false;
      float num = EditorGUILayout.Slider("Height", (float) this.m_TargetHeight * this.m_Terrain.terrainData.size.y, 0.0f, this.m_Terrain.terrainData.size.y, new GUILayoutOption[0]);
      if (GUI.changed)
        this.m_TargetHeight.value = num / this.m_Terrain.terrainData.size.y;
      if (GUILayout.Button(TerrainInspector.styles.flatten, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Terrain.terrainData, "Flatten Heightmap");
        HeightmapFilters.Flatten(this.m_Terrain.terrainData, this.m_TargetHeight.value);
      }
      GUILayout.EndHorizontal();
    }

    private void OnInspectorUpdate()
    {
      if (!AssetPreview.HasAnyNewPreviewTexturesAvailable())
        return;
      this.Repaint();
    }

    public override void OnInspectorGUI()
    {
      this.Initialize();
      if (TerrainInspector.styles == null)
        TerrainInspector.styles = new TerrainInspector.Styles();
      if (!(bool) ((UnityEngine.Object) this.m_Terrain.terrainData))
      {
        GUI.enabled = false;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Toolbar(-1, TerrainInspector.styles.toolIcons, TerrainInspector.styles.command, new GUILayoutOption[0]);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUI.enabled = true;
        GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
        GUILayout.Label("Terrain Asset Missing");
        this.m_Terrain.terrainData = EditorGUILayout.ObjectField("Assign:", (UnityEngine.Object) this.m_Terrain.terrainData, typeof (TerrainData), false, new GUILayoutOption[0]) as TerrainData;
        GUILayout.EndVertical();
      }
      else
      {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.changed = false;
        int selectedTool = (int) this.selectedTool;
        this.selectedTool = (TerrainTool) GUILayout.Toolbar(selectedTool, TerrainInspector.styles.toolIcons, TerrainInspector.styles.command, new GUILayoutOption[0]);
        if (this.selectedTool != (TerrainTool) selectedTool && (UnityEngine.Object) Toolbar.get != (UnityEngine.Object) null)
          Toolbar.get.Repaint();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        this.CheckKeys();
        GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
        if (selectedTool >= 0 && selectedTool < TerrainInspector.styles.toolIcons.Length)
        {
          GUILayout.Label(TerrainInspector.styles.toolNames[selectedTool].text);
          GUILayout.Label(TerrainInspector.styles.toolNames[selectedTool].tooltip, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
        }
        else
        {
          GUILayout.Label("No tool selected");
          GUILayout.Label("Please select a tool", EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
        }
        GUILayout.EndVertical();
        switch (selectedTool)
        {
          case 0:
            this.ShowRaiseHeight();
            break;
          case 1:
            this.ShowSetHeight();
            break;
          case 2:
            this.ShowSmoothHeight();
            break;
          case 3:
            this.ShowTextures();
            break;
          case 4:
            this.ShowTrees();
            break;
          case 5:
            this.ShowDetails();
            break;
          case 6:
            this.ShowSettings();
            break;
        }
        GUILayout.Space(5f);
      }
    }

    private Brush GetActiveBrush(int size)
    {
      if (this.m_CachedBrush == null)
        this.m_CachedBrush = new Brush();
      this.m_CachedBrush.Load(TerrainInspector.s_BrushTextures[this.m_SelectedBrush], size);
      return this.m_CachedBrush;
    }

    public bool Raycast(out Vector2 uv, out Vector3 pos)
    {
      RaycastHit hitInfo;
      if (this.m_Terrain.GetComponent<Collider>().Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hitInfo, float.PositiveInfinity))
      {
        uv = hitInfo.textureCoord;
        pos = hitInfo.point;
        return true;
      }
      uv = Vector2.zero;
      pos = Vector3.zero;
      return false;
    }

    public bool HasFrameBounds()
    {
      return (UnityEngine.Object) this.m_Terrain != (UnityEngine.Object) null;
    }

    public Bounds OnGetFrameBounds()
    {
      Vector2 uv;
      Vector3 pos;
      if ((bool) ((UnityEngine.Object) Camera.current) && this.Raycast(out uv, out pos))
      {
        if ((UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null)
          SceneView.lastActiveSceneView.viewIsLockedToObject = false;
        Bounds bounds = new Bounds();
        float num = this.selectedTool != TerrainTool.PlaceTree ? (float) (int) this.m_Size : TreePainter.brushSize;
        Vector3 vector3;
        vector3.x = num / (float) this.m_Terrain.terrainData.heightmapWidth * this.m_Terrain.terrainData.size.x;
        vector3.z = num / (float) this.m_Terrain.terrainData.heightmapHeight * this.m_Terrain.terrainData.size.z;
        vector3.y = (float) (((double) vector3.x + (double) vector3.z) * 0.5);
        bounds.center = pos;
        bounds.size = vector3;
        if (this.selectedTool == TerrainTool.PaintDetail && this.m_Terrain.terrainData.detailWidth != 0)
        {
          vector3.x = (float) ((double) num / (double) this.m_Terrain.terrainData.detailWidth * (double) this.m_Terrain.terrainData.size.x * 0.699999988079071);
          vector3.z = (float) ((double) num / (double) this.m_Terrain.terrainData.detailHeight * (double) this.m_Terrain.terrainData.size.z * 0.699999988079071);
          vector3.y = 0.0f;
          bounds.size = vector3;
        }
        return bounds;
      }
      Vector3 position = this.m_Terrain.transform.position;
      Vector3 size = this.m_Terrain.terrainData.size;
      float[,] heights = this.m_Terrain.terrainData.GetHeights(0, 0, this.m_Terrain.terrainData.heightmapWidth, this.m_Terrain.terrainData.heightmapHeight);
      float a = float.MinValue;
      for (int index1 = 0; index1 < this.m_Terrain.terrainData.heightmapHeight; ++index1)
      {
        for (int index2 = 0; index2 < this.m_Terrain.terrainData.heightmapWidth; ++index2)
          a = Mathf.Max(a, heights[index2, index1]);
      }
      size.y = a * size.y;
      return new Bounds(position + size * 0.5f, size);
    }

    private bool IsModificationToolActive()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Terrain))
        return false;
      TerrainTool selectedTool = this.selectedTool;
      return selectedTool != TerrainTool.TerrainSettings && selectedTool >= TerrainTool.PaintHeight && selectedTool < TerrainTool.TerrainToolCount;
    }

    private bool IsBrushPreviewVisible()
    {
      if (!this.IsModificationToolActive())
        return false;
      Vector2 uv;
      Vector3 pos;
      return this.Raycast(out uv, out pos);
    }

    private void DisableProjector()
    {
      if (this.m_CachedBrush == null)
        return;
      this.m_CachedBrush.GetPreviewProjector().enabled = false;
    }

    private void UpdatePreviewBrush()
    {
      if (!this.IsModificationToolActive() || (UnityEngine.Object) this.m_Terrain.terrainData == (UnityEngine.Object) null)
      {
        this.DisableProjector();
      }
      else
      {
        Projector previewProjector = this.GetActiveBrush((int) this.m_Size).GetPreviewProjector();
        float num1 = 1f;
        float num2 = this.m_Terrain.terrainData.size.x / this.m_Terrain.terrainData.size.z;
        Transform transform = previewProjector.transform;
        bool flag = true;
        Vector2 uv;
        Vector3 pos;
        if (this.Raycast(out uv, out pos))
        {
          if (this.selectedTool == TerrainTool.PlaceTree)
          {
            previewProjector.material.mainTexture = (Texture) EditorGUIUtility.Load(EditorResourcesUtility.brushesPath + "builtin_brush_4.png");
            num1 = TreePainter.brushSize / 0.8f;
            num2 = 1f;
          }
          else if (this.selectedTool == TerrainTool.PaintHeight || this.selectedTool == TerrainTool.SetHeight || this.selectedTool == TerrainTool.SmoothHeight)
          {
            if ((int) this.m_Size % 2 == 0)
            {
              float num3 = 0.5f;
              uv.x = (Mathf.Floor(uv.x * (float) (this.m_Terrain.terrainData.heightmapWidth - 1)) + num3) / (float) (this.m_Terrain.terrainData.heightmapWidth - 1);
              uv.y = (Mathf.Floor(uv.y * (float) (this.m_Terrain.terrainData.heightmapHeight - 1)) + num3) / (float) (this.m_Terrain.terrainData.heightmapHeight - 1);
            }
            else
            {
              uv.x = Mathf.Round(uv.x * (float) (this.m_Terrain.terrainData.heightmapWidth - 1)) / (float) (this.m_Terrain.terrainData.heightmapWidth - 1);
              uv.y = Mathf.Round(uv.y * (float) (this.m_Terrain.terrainData.heightmapHeight - 1)) / (float) (this.m_Terrain.terrainData.heightmapHeight - 1);
            }
            pos.x = uv.x * this.m_Terrain.terrainData.size.x;
            pos.z = uv.y * this.m_Terrain.terrainData.size.z;
            pos += this.m_Terrain.transform.position;
            num1 = (float) (int) this.m_Size * 0.5f / (float) this.m_Terrain.terrainData.heightmapWidth * this.m_Terrain.terrainData.size.x;
          }
          else if (this.selectedTool == TerrainTool.PaintTexture || this.selectedTool == TerrainTool.PaintDetail)
          {
            float num3 = (int) this.m_Size % 2 != 0 ? 0.5f : 0.0f;
            int num4;
            int num5;
            if (this.selectedTool == TerrainTool.PaintTexture)
            {
              num4 = this.m_Terrain.terrainData.alphamapWidth;
              num5 = this.m_Terrain.terrainData.alphamapHeight;
            }
            else
            {
              num4 = this.m_Terrain.terrainData.detailWidth;
              num5 = this.m_Terrain.terrainData.detailHeight;
            }
            if (num4 == 0 || num5 == 0)
              flag = false;
            uv.x = (Mathf.Floor(uv.x * (float) num4) + num3) / (float) num4;
            uv.y = (Mathf.Floor(uv.y * (float) num5) + num3) / (float) num5;
            pos.x = uv.x * this.m_Terrain.terrainData.size.x;
            pos.z = uv.y * this.m_Terrain.terrainData.size.z;
            pos += this.m_Terrain.transform.position;
            num1 = (float) (int) this.m_Size * 0.5f / (float) num4 * this.m_Terrain.terrainData.size.x;
            num2 = (float) num4 / (float) num5;
          }
        }
        else
          flag = false;
        previewProjector.enabled = flag;
        if (flag)
        {
          pos.y = this.m_Terrain.SampleHeight(pos);
          transform.position = pos + new Vector3(0.0f, 50f, 0.0f);
        }
        previewProjector.orthographicSize = num1 / num2;
        previewProjector.aspectRatio = num2;
      }
    }

    public void OnSceneGUI()
    {
      this.Initialize();
      if (!(bool) ((UnityEngine.Object) this.m_Terrain.terrainData))
        return;
      Event current = Event.current;
      this.CheckKeys();
      int controlId = GUIUtility.GetControlID(TerrainInspector.s_TerrainEditorHash, FocusType.Passive);
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != controlId || current.GetTypeForControl(controlId) == EventType.MouseDrag && GUIUtility.hotControl != controlId || (Event.current.alt || current.button != 0 || (!this.IsModificationToolActive() || HandleUtility.nearestControl != controlId)))
            break;
          if (current.type == EventType.MouseDown)
            GUIUtility.hotControl = controlId;
          Vector2 uv;
          Vector3 pos;
          if (!this.Raycast(out uv, out pos))
            break;
          if (this.selectedTool == TerrainTool.SetHeight && Event.current.shift)
          {
            this.m_TargetHeight.value = this.m_Terrain.terrainData.GetInterpolatedHeight(uv.x, uv.y) / this.m_Terrain.terrainData.size.y;
            InspectorWindow.RepaintAllInspectors();
          }
          else if (this.selectedTool == TerrainTool.PlaceTree)
          {
            if (current.type == EventType.MouseDown)
              Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Terrain.terrainData, "Place Tree");
            if (!Event.current.shift && !Event.current.control)
              TreePainter.PlaceTrees(this.m_Terrain, uv.x, uv.y);
            else
              TreePainter.RemoveTrees(this.m_Terrain, uv.x, uv.y, Event.current.control);
          }
          else if (this.selectedTool == TerrainTool.PaintTexture)
          {
            if (current.type == EventType.MouseDown)
            {
              List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
              objectList.Add((UnityEngine.Object) this.m_Terrain.terrainData);
              objectList.AddRange((IEnumerable<UnityEngine.Object>) this.m_Terrain.terrainData.alphamapTextures);
              Undo.RegisterCompleteObjectUndo(objectList.ToArray(), "Detail Edit");
            }
            SplatPainter splatPainter = new SplatPainter();
            splatPainter.size = (int) this.m_Size;
            splatPainter.strength = (float) this.m_Strength;
            splatPainter.terrainData = this.m_Terrain.terrainData;
            splatPainter.brush = this.GetActiveBrush(splatPainter.size);
            splatPainter.target = (float) this.m_SplatAlpha;
            splatPainter.tool = this.selectedTool;
            this.m_Terrain.editorRenderFlags = TerrainRenderFlags.heightmap;
            splatPainter.Paint(uv.x, uv.y, this.m_SelectedSplat);
            this.m_Terrain.terrainData.SetBasemapDirty(false);
          }
          else if (this.selectedTool == TerrainTool.PaintDetail)
          {
            if (current.type == EventType.MouseDown)
              Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Terrain.terrainData, "Detail Edit");
            DetailPainter detailPainter = new DetailPainter();
            detailPainter.size = (int) this.m_Size;
            detailPainter.targetStrength = (float) this.m_DetailStrength * 16f;
            detailPainter.opacity = (float) this.m_DetailOpacity;
            if (Event.current.shift || Event.current.control)
              detailPainter.targetStrength *= -1f;
            detailPainter.clearSelectedOnly = Event.current.control;
            detailPainter.terrainData = this.m_Terrain.terrainData;
            detailPainter.brush = this.GetActiveBrush(detailPainter.size);
            detailPainter.tool = this.selectedTool;
            detailPainter.randomizeDetails = true;
            detailPainter.Paint(uv.x, uv.y, this.m_SelectedDetail);
          }
          else
          {
            if (current.type == EventType.MouseDown)
              Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this.m_Terrain.terrainData, "Heightmap Edit");
            HeightmapPainter heightmapPainter = new HeightmapPainter();
            heightmapPainter.size = (int) this.m_Size;
            heightmapPainter.strength = (float) this.m_Strength * 0.01f;
            if (this.selectedTool == TerrainTool.SmoothHeight)
              heightmapPainter.strength = (float) this.m_Strength;
            heightmapPainter.terrainData = this.m_Terrain.terrainData;
            heightmapPainter.brush = this.GetActiveBrush((int) this.m_Size);
            heightmapPainter.targetHeight = (float) this.m_TargetHeight;
            heightmapPainter.tool = this.selectedTool;
            this.m_Terrain.editorRenderFlags = TerrainRenderFlags.heightmap;
            if (this.selectedTool == TerrainTool.PaintHeight && Event.current.shift)
              heightmapPainter.strength = -heightmapPainter.strength;
            heightmapPainter.PaintHeight(uv.x, uv.y);
          }
          current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          if (!this.IsModificationToolActive())
            break;
          if (this.selectedTool == TerrainTool.PaintTexture)
            this.m_Terrain.terrainData.SetBasemapDirty(true);
          this.m_Terrain.editorRenderFlags = TerrainRenderFlags.all;
          this.m_Terrain.ApplyDelayedHeightmapModification();
          current.Use();
          break;
        case EventType.MouseMove:
          if (!this.IsBrushPreviewVisible())
            break;
          HandleUtility.Repaint();
          break;
        case EventType.Repaint:
          this.DisableProjector();
          break;
        case EventType.Layout:
          if (!this.IsModificationToolActive())
            break;
          HandleUtility.AddDefaultControl(controlId);
          break;
      }
    }

    public void OnPreSceneGUI()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.UpdatePreviewBrush();
    }

    private class Styles
    {
      public GUIStyle gridList = (GUIStyle) "GridList";
      public GUIStyle gridListText = (GUIStyle) "GridListText";
      public GUIStyle label = (GUIStyle) "RightLabel";
      public GUIStyle largeSquare = (GUIStyle) "Button";
      public GUIStyle command = (GUIStyle) "Command";
      public Texture settingsIcon = EditorGUIUtility.IconContent("SettingsIcon").image;
      public GUIContent[] toolIcons = new GUIContent[7]{ EditorGUIUtility.IconContent("TerrainInspector.TerrainToolRaise", "|Raise and lower the terrain height."), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSetHeight", "|Set the terrain height."), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSmoothHeight", "|Smooth the terrain height."), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSplat", "|Paint the terrain texture."), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolTrees", "|Place trees"), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolPlants", "|Place plants, stones and other small foilage"), EditorGUIUtility.IconContent("TerrainInspector.TerrainToolSettings", "|Settings for the terrain") };
      public GUIContent[] toolNames = new GUIContent[7]{ EditorGUIUtility.TextContent("Raise / Lower Terrain|Click to raise. Hold down shift to lower."), EditorGUIUtility.TextContent("Paint Height|Hold shift to sample target height."), EditorGUIUtility.TextContent("Smooth Height"), EditorGUIUtility.TextContent("Paint Texture|Select a texture below, then click to paint"), EditorGUIUtility.TextContent("Place Trees|Hold down shift to erase trees.\nHold down ctrl to erase the selected tree type."), EditorGUIUtility.TextContent("Paint Details|Hold down shift to erase.\nHold down ctrl to erase the selected detail type."), EditorGUIUtility.TextContent("Terrain Settings") };
      public GUIContent brushSize = EditorGUIUtility.TextContent("Brush Size|Size of the brush used to paint");
      public GUIContent opacity = EditorGUIUtility.TextContent("Opacity|Strength of the applied effect");
      public GUIContent settings = EditorGUIUtility.TextContent("Settings");
      public GUIContent brushes = EditorGUIUtility.TextContent("Brushes");
      public GUIContent textures = EditorGUIUtility.TextContent("Textures");
      public GUIContent editTextures = EditorGUIUtility.TextContent("Edit Textures...");
      public GUIContent trees = EditorGUIUtility.TextContent("Trees");
      public GUIContent noTrees = EditorGUIUtility.TextContent("No Trees defined|Use edit button below to add new tree types.");
      public GUIContent editTrees = EditorGUIUtility.TextContent("Edit Trees...|Add/remove tree types.");
      public GUIContent treeDensity = EditorGUIUtility.TextContent("Tree Density|How dense trees are you painting");
      public GUIContent treeHeight = EditorGUIUtility.TextContent("Tree Height|Height of the planted trees");
      public GUIContent treeHeightRandomLabel = EditorGUIUtility.TextContent("Random?|Enable random variation in tree height (variation)");
      public GUIContent treeHeightRandomToggle = EditorGUIUtility.TextContent("|Enable random variation in tree height (variation)");
      public GUIContent lockWidth = EditorGUIUtility.TextContent("Lock Width to Height|Let the tree width be the same with height");
      public GUIContent treeWidth = EditorGUIUtility.TextContent("Tree Width|Width of the planted trees");
      public GUIContent treeWidthRandomLabel = EditorGUIUtility.TextContent("Random?|Enable random variation in tree width (variation)");
      public GUIContent treeWidthRandomToggle = EditorGUIUtility.TextContent("|Enable random variation in tree width (variation)");
      public GUIContent treeColorVar = EditorGUIUtility.TextContent("Color Variation|Amount of random shading applied to trees");
      public GUIContent treeRotation = EditorGUIUtility.TextContent("Random Tree Rotation|Enable?");
      public GUIContent massPlaceTrees = EditorGUIUtility.TextContent("Mass Place Trees");
      public GUIContent details = EditorGUIUtility.TextContent("Details");
      public GUIContent editDetails = EditorGUIUtility.TextContent("Edit Details...|Add/remove detail meshes");
      public GUIContent detailTargetStrength = EditorGUIUtility.TextContent("Target Strength|Target amount");
      public GUIContent heightmap = EditorGUIUtility.TextContent("Heightmap");
      public GUIContent importRaw = EditorGUIUtility.TextContent("Import Raw...");
      public GUIContent exportRaw = EditorGUIUtility.TextContent("Export Raw...");
      public GUIContent flatten = EditorGUIUtility.TextContent("Flatten");
      public GUIContent overrideSmoothness = EditorGUIUtility.TextContent("Override Smoothness|If checked, the smoothness value specified below will be used for all splat layers, otherwise smoothness of each individual splat layer will be controlled by the alpha channel of the splat texture.");
      public GUIContent bakeLightProbesForTrees = EditorGUIUtility.TextContent("Bake Light Probes For Trees|If the option is enabled, Unity will create internal light probes at the position of each tree (these probes are internal and will not affect other renderers in the scene) and apply them to tree renderers for lighting. Otherwise trees are still affected by LightProbeGroups. The option is only effective for trees that have LightProbe enabled on their prototype prefab.");
      public GUIContent resolution = EditorGUIUtility.TextContent("Resolution");
      public GUIContent refresh = EditorGUIUtility.TextContent("Refresh");
    }
  }
}
