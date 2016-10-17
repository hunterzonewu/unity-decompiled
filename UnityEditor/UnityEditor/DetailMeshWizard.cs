// Decompiled with JetBrains decompiler
// Type: UnityEditor.DetailMeshWizard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class DetailMeshWizard : TerrainWizard
  {
    private int m_PrototypeIndex = -1;
    public GameObject m_Detail;
    public float m_NoiseSpread;
    public float m_MinWidth;
    public float m_MaxWidth;
    public float m_MinHeight;
    public float m_MaxHeight;
    public Color m_HealthyColor;
    public Color m_DryColor;
    public DetailMeshRenderMode m_RenderMode;

    public void OnEnable()
    {
      this.minSize = new Vector2(400f, 400f);
    }

    internal void InitializeDefaults(Terrain terrain, int index)
    {
      this.m_Terrain = terrain;
      this.m_PrototypeIndex = index;
      DetailPrototype detailPrototype = this.m_PrototypeIndex != -1 ? this.m_Terrain.terrainData.detailPrototypes[this.m_PrototypeIndex] : new DetailPrototype();
      this.m_Detail = detailPrototype.prototype;
      this.m_NoiseSpread = detailPrototype.noiseSpread;
      this.m_MinWidth = detailPrototype.minWidth;
      this.m_MaxWidth = detailPrototype.maxWidth;
      this.m_MinHeight = detailPrototype.minHeight;
      this.m_MaxHeight = detailPrototype.maxHeight;
      this.m_HealthyColor = detailPrototype.healthyColor;
      this.m_DryColor = detailPrototype.dryColor;
      switch (detailPrototype.renderMode)
      {
        case DetailRenderMode.GrassBillboard:
          Debug.LogError((object) "Detail meshes can't be rendered as billboards");
          this.m_RenderMode = DetailMeshRenderMode.Grass;
          break;
        case DetailRenderMode.VertexLit:
          this.m_RenderMode = DetailMeshRenderMode.VertexLit;
          break;
        case DetailRenderMode.Grass:
          this.m_RenderMode = DetailMeshRenderMode.Grass;
          break;
      }
      this.OnWizardUpdate();
    }

    private void DoApply()
    {
      if ((UnityEngine.Object) this.terrainData == (UnityEngine.Object) null)
        return;
      DetailPrototype[] detailPrototypeArray1 = this.m_Terrain.terrainData.detailPrototypes;
      if (this.m_PrototypeIndex == -1)
      {
        DetailPrototype[] detailPrototypeArray2 = new DetailPrototype[detailPrototypeArray1.Length + 1];
        Array.Copy((Array) detailPrototypeArray1, 0, (Array) detailPrototypeArray2, 0, detailPrototypeArray1.Length);
        this.m_PrototypeIndex = detailPrototypeArray1.Length;
        detailPrototypeArray1 = detailPrototypeArray2;
        detailPrototypeArray1[this.m_PrototypeIndex] = new DetailPrototype();
      }
      detailPrototypeArray1[this.m_PrototypeIndex].renderMode = DetailRenderMode.VertexLit;
      detailPrototypeArray1[this.m_PrototypeIndex].usePrototypeMesh = true;
      detailPrototypeArray1[this.m_PrototypeIndex].prototype = this.m_Detail;
      detailPrototypeArray1[this.m_PrototypeIndex].prototypeTexture = (Texture2D) null;
      detailPrototypeArray1[this.m_PrototypeIndex].noiseSpread = this.m_NoiseSpread;
      detailPrototypeArray1[this.m_PrototypeIndex].minWidth = this.m_MinWidth;
      detailPrototypeArray1[this.m_PrototypeIndex].maxWidth = this.m_MaxWidth;
      detailPrototypeArray1[this.m_PrototypeIndex].minHeight = this.m_MinHeight;
      detailPrototypeArray1[this.m_PrototypeIndex].maxHeight = this.m_MaxHeight;
      detailPrototypeArray1[this.m_PrototypeIndex].healthyColor = this.m_HealthyColor;
      detailPrototypeArray1[this.m_PrototypeIndex].dryColor = this.m_DryColor;
      detailPrototypeArray1[this.m_PrototypeIndex].renderMode = this.m_RenderMode != DetailMeshRenderMode.Grass ? DetailRenderMode.VertexLit : DetailRenderMode.Grass;
      this.m_Terrain.terrainData.detailPrototypes = detailPrototypeArray1;
      EditorUtility.SetDirty((UnityEngine.Object) this.m_Terrain);
    }

    private void OnWizardCreate()
    {
      this.DoApply();
    }

    private void OnWizardOtherButton()
    {
      this.DoApply();
    }

    internal override void OnWizardUpdate()
    {
      base.OnWizardUpdate();
      if ((UnityEngine.Object) this.m_Detail == (UnityEngine.Object) null)
      {
        this.errorString = "Please assign a detail prefab";
        this.isValid = false;
      }
      else
      {
        if (this.m_PrototypeIndex == -1)
          return;
        this.DoApply();
      }
    }
  }
}
