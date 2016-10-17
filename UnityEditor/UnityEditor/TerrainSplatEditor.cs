// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainSplatEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class TerrainSplatEditor : EditorWindow
  {
    private string m_ButtonTitle = string.Empty;
    private int m_Index = -1;
    private Vector2 m_ScrollPosition;
    private Terrain m_Terrain;
    public Texture2D m_Texture;
    public Texture2D m_NormalMap;
    private Vector2 m_TileSize;
    private Vector2 m_TileOffset;
    private Color m_Specular;
    private float m_Metallic;
    private float m_Smoothness;

    public TerrainSplatEditor()
    {
      this.position = new Rect(50f, 50f, 200f, 300f);
      this.minSize = new Vector2(200f, 300f);
    }

    internal static void ShowTerrainSplatEditor(string title, string button, Terrain terrain, int index)
    {
      TerrainSplatEditor window = EditorWindow.GetWindow<TerrainSplatEditor>(true, title);
      window.m_ButtonTitle = button;
      window.InitializeData(terrain, index);
    }

    private void InitializeData(Terrain terrain, int index)
    {
      this.m_Terrain = terrain;
      this.m_Index = index;
      SplatPrototype splatPrototype = index != -1 ? this.m_Terrain.terrainData.splatPrototypes[index] : new SplatPrototype();
      this.m_Texture = splatPrototype.texture;
      this.m_NormalMap = splatPrototype.normalMap;
      this.m_TileSize = splatPrototype.tileSize;
      this.m_TileOffset = splatPrototype.tileOffset;
      this.m_Specular = splatPrototype.specular;
      this.m_Metallic = splatPrototype.metallic;
      this.m_Smoothness = splatPrototype.smoothness;
    }

    private void ApplyTerrainSplat()
    {
      if ((UnityEngine.Object) this.m_Terrain == (UnityEngine.Object) null || (UnityEngine.Object) this.m_Terrain.terrainData == (UnityEngine.Object) null)
        return;
      SplatPrototype[] splatPrototypeArray1 = this.m_Terrain.terrainData.splatPrototypes;
      if (this.m_Index == -1)
      {
        SplatPrototype[] splatPrototypeArray2 = new SplatPrototype[splatPrototypeArray1.Length + 1];
        Array.Copy((Array) splatPrototypeArray1, 0, (Array) splatPrototypeArray2, 0, splatPrototypeArray1.Length);
        this.m_Index = splatPrototypeArray1.Length;
        splatPrototypeArray1 = splatPrototypeArray2;
        splatPrototypeArray1[this.m_Index] = new SplatPrototype();
      }
      splatPrototypeArray1[this.m_Index].texture = this.m_Texture;
      splatPrototypeArray1[this.m_Index].normalMap = this.m_NormalMap;
      splatPrototypeArray1[this.m_Index].tileSize = this.m_TileSize;
      splatPrototypeArray1[this.m_Index].tileOffset = this.m_TileOffset;
      splatPrototypeArray1[this.m_Index].specular = this.m_Specular;
      splatPrototypeArray1[this.m_Index].metallic = this.m_Metallic;
      splatPrototypeArray1[this.m_Index].smoothness = this.m_Smoothness;
      this.m_Terrain.terrainData.splatPrototypes = splatPrototypeArray1;
      EditorUtility.SetDirty((UnityEngine.Object) this.m_Terrain);
    }

    private bool ValidateTerrain()
    {
      if (!((UnityEngine.Object) this.m_Terrain == (UnityEngine.Object) null) && !((UnityEngine.Object) this.m_Terrain.terrainData == (UnityEngine.Object) null))
        return true;
      EditorGUILayout.HelpBox("Terrain does not exist", MessageType.Error);
      return false;
    }

    private bool ValidateMainTexture(Texture2D tex)
    {
      if ((UnityEngine.Object) tex == (UnityEngine.Object) null)
      {
        EditorGUILayout.HelpBox("Assign a tiling texture", MessageType.Warning);
        return false;
      }
      if (tex.wrapMode != TextureWrapMode.Repeat)
      {
        EditorGUILayout.HelpBox("Texture wrap mode must be set to Repeat", MessageType.Warning);
        return false;
      }
      if (tex.width != Mathf.ClosestPowerOfTwo(tex.width) || tex.height != Mathf.ClosestPowerOfTwo(tex.height))
      {
        EditorGUILayout.HelpBox("Texture size must be power of two", MessageType.Warning);
        return false;
      }
      if (tex.mipmapCount > 1)
        return true;
      EditorGUILayout.HelpBox("Texture must have mip maps", MessageType.Warning);
      return false;
    }

    private static void TextureFieldGUI(string label, ref Texture2D texture, float alignmentOffset)
    {
      GUILayout.Space(6f);
      GUILayout.BeginVertical(GUILayout.Width(80f));
      GUILayout.Label(label);
      System.Type objType = typeof (Texture2D);
      Rect rect = GUILayoutUtility.GetRect(64f, 64f, 64f, 64f, new GUILayoutOption[1]{ GUILayout.MaxWidth(64f) });
      rect.x += alignmentOffset;
      texture = EditorGUI.DoObjectField(rect, rect, GUIUtility.GetControlID(12354, EditorGUIUtility.native, rect), (UnityEngine.Object) texture, objType, (SerializedProperty) null, (EditorGUI.ObjectFieldValidator) null, false) as Texture2D;
      GUILayout.EndVertical();
    }

    private static void SplatSizeGUI(ref Vector2 scale, ref Vector2 offset)
    {
      GUILayoutOption guiLayoutOption1 = GUILayout.Width(10f);
      GUILayoutOption guiLayoutOption2 = GUILayout.MinWidth(32f);
      GUILayout.Space(6f);
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      GUILayout.Label(string.Empty, EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        guiLayoutOption1
      });
      GUILayout.Label("x", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        guiLayoutOption1
      });
      GUILayout.Label("y", EditorStyles.miniLabel, new GUILayoutOption[1]
      {
        guiLayoutOption1
      });
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      GUILayout.Label("Size", EditorStyles.miniLabel, new GUILayoutOption[0]);
      scale.x = EditorGUILayout.FloatField(scale.x, EditorStyles.miniTextField, new GUILayoutOption[1]
      {
        guiLayoutOption2
      });
      scale.y = EditorGUILayout.FloatField(scale.y, EditorStyles.miniTextField, new GUILayoutOption[1]
      {
        guiLayoutOption2
      });
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      GUILayout.Label("Offset", EditorStyles.miniLabel, new GUILayoutOption[0]);
      offset.x = EditorGUILayout.FloatField(offset.x, EditorStyles.miniTextField, new GUILayoutOption[1]
      {
        guiLayoutOption2
      });
      offset.y = EditorGUILayout.FloatField(offset.y, EditorStyles.miniTextField, new GUILayoutOption[1]
      {
        guiLayoutOption2
      });
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
    }

    private static bool IsUsingMetallic(Terrain.MaterialType materialType, Material materialTemplate)
    {
      if (materialType == Terrain.MaterialType.BuiltInStandard)
        return true;
      if (materialType == Terrain.MaterialType.Custom && (UnityEngine.Object) materialTemplate != (UnityEngine.Object) null)
        return materialTemplate.HasProperty("_Metallic0");
      return false;
    }

    private static bool IsUsingSpecular(Terrain.MaterialType materialType, Material materialTemplate)
    {
      if (materialType == Terrain.MaterialType.BuiltInStandard)
        return true;
      if (materialType == Terrain.MaterialType.Custom && (UnityEngine.Object) materialTemplate != (UnityEngine.Object) null)
        return materialTemplate.HasProperty("_Specular0");
      return false;
    }

    private static bool IsUsingSmoothness(Terrain.MaterialType materialType, Material materialTemplate)
    {
      if (materialType == Terrain.MaterialType.BuiltInStandard)
        return true;
      if (materialType == Terrain.MaterialType.Custom && (UnityEngine.Object) materialTemplate != (UnityEngine.Object) null)
        return materialTemplate.HasProperty("_Smoothness0");
      return false;
    }

    private void OnGUI()
    {
      EditorGUIUtility.labelWidth = (float) ((double) this.position.width - 64.0 - 20.0);
      bool flag1 = true;
      this.m_ScrollPosition = EditorGUILayout.BeginVerticalScrollView(this.m_ScrollPosition, false, GUI.skin.verticalScrollbar, GUI.skin.scrollView);
      bool flag2 = flag1 & this.ValidateTerrain();
      EditorGUI.BeginChangeCheck();
      GUILayout.BeginHorizontal();
      string label = string.Empty;
      float alignmentOffset = 0.0f;
      switch (this.m_Terrain.materialType)
      {
        case Terrain.MaterialType.BuiltInStandard:
          label = " Albedo (RGB)\nSmoothness (A)";
          alignmentOffset = 15f;
          break;
        case Terrain.MaterialType.BuiltInLegacyDiffuse:
          label = "\n Diffuse (RGB)";
          alignmentOffset = 15f;
          break;
        case Terrain.MaterialType.BuiltInLegacySpecular:
          label = "Diffuse (RGB)\n   Gloss (A)";
          alignmentOffset = 12f;
          break;
        case Terrain.MaterialType.Custom:
          label = " \n  Splat";
          alignmentOffset = 0.0f;
          break;
      }
      TerrainSplatEditor.TextureFieldGUI(label, ref this.m_Texture, alignmentOffset);
      TerrainSplatEditor.TextureFieldGUI("\nNormal", ref this.m_NormalMap, -4f);
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      bool flag3 = flag2 & this.ValidateMainTexture(this.m_Texture);
      if (flag3)
      {
        if (TerrainSplatEditor.IsUsingMetallic(this.m_Terrain.materialType, this.m_Terrain.materialTemplate))
        {
          EditorGUILayout.Space();
          float labelWidth = EditorGUIUtility.labelWidth;
          EditorGUIUtility.labelWidth = 75f;
          this.m_Metallic = EditorGUILayout.Slider("Metallic", this.m_Metallic, 0.0f, 1f, new GUILayoutOption[0]);
          EditorGUIUtility.labelWidth = labelWidth;
        }
        else if (TerrainSplatEditor.IsUsingSpecular(this.m_Terrain.materialType, this.m_Terrain.materialTemplate))
          this.m_Specular = EditorGUILayout.ColorField("Specular", this.m_Specular, new GUILayoutOption[0]);
        if (TerrainSplatEditor.IsUsingSmoothness(this.m_Terrain.materialType, this.m_Terrain.materialTemplate) && !TextureUtil.HasAlphaTextureFormat(this.m_Texture.format))
        {
          EditorGUILayout.Space();
          float labelWidth = EditorGUIUtility.labelWidth;
          EditorGUIUtility.labelWidth = 75f;
          this.m_Smoothness = EditorGUILayout.Slider("Smoothness", this.m_Smoothness, 0.0f, 1f, new GUILayoutOption[0]);
          EditorGUIUtility.labelWidth = labelWidth;
        }
      }
      TerrainSplatEditor.SplatSizeGUI(ref this.m_TileSize, ref this.m_TileOffset);
      bool flag4 = EditorGUI.EndChangeCheck();
      EditorGUILayout.EndScrollView();
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUI.enabled = flag3;
      if (GUILayout.Button(this.m_ButtonTitle, new GUILayoutOption[1]{ GUILayout.MinWidth(100f) }))
      {
        this.ApplyTerrainSplat();
        this.Close();
        GUIUtility.ExitGUI();
      }
      GUI.enabled = true;
      GUILayout.EndHorizontal();
      if (!flag4 || !flag3 || this.m_Index == -1)
        return;
      this.ApplyTerrainSplat();
    }
  }
}
