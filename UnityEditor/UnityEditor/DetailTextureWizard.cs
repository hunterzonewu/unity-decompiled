// Decompiled with JetBrains decompiler
// Type: UnityEditor.DetailTextureWizard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class DetailTextureWizard : TerrainWizard
  {
    private int m_PrototypeIndex = -1;
    public Texture2D m_DetailTexture;
    public float m_MinWidth;
    public float m_MaxWidth;
    public float m_MinHeight;
    public float m_MaxHeight;
    public float m_NoiseSpread;
    public Color m_HealthyColor;
    public Color m_DryColor;
    public bool m_Billboard;

    public void OnEnable()
    {
      this.minSize = new Vector2(400f, 400f);
    }

    internal void InitializeDefaults(Terrain terrain, int index)
    {
      this.m_Terrain = terrain;
      this.m_PrototypeIndex = index;
      DetailPrototype detailPrototype;
      if (this.m_PrototypeIndex == -1)
      {
        detailPrototype = new DetailPrototype();
        detailPrototype.renderMode = DetailRenderMode.GrassBillboard;
      }
      else
        detailPrototype = this.m_Terrain.terrainData.detailPrototypes[this.m_PrototypeIndex];
      this.m_DetailTexture = detailPrototype.prototypeTexture;
      this.m_MinWidth = detailPrototype.minWidth;
      this.m_MaxWidth = detailPrototype.maxWidth;
      this.m_MinHeight = detailPrototype.minHeight;
      this.m_MaxHeight = detailPrototype.maxHeight;
      this.m_NoiseSpread = detailPrototype.noiseSpread;
      this.m_HealthyColor = detailPrototype.healthyColor;
      this.m_DryColor = detailPrototype.dryColor;
      this.m_Billboard = detailPrototype.renderMode == DetailRenderMode.GrassBillboard;
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
      detailPrototypeArray1[this.m_PrototypeIndex].prototype = (GameObject) null;
      detailPrototypeArray1[this.m_PrototypeIndex].prototypeTexture = this.m_DetailTexture;
      detailPrototypeArray1[this.m_PrototypeIndex].minWidth = this.m_MinWidth;
      detailPrototypeArray1[this.m_PrototypeIndex].maxWidth = this.m_MaxWidth;
      detailPrototypeArray1[this.m_PrototypeIndex].minHeight = this.m_MinHeight;
      detailPrototypeArray1[this.m_PrototypeIndex].maxHeight = this.m_MaxHeight;
      detailPrototypeArray1[this.m_PrototypeIndex].noiseSpread = this.m_NoiseSpread;
      detailPrototypeArray1[this.m_PrototypeIndex].healthyColor = this.m_HealthyColor;
      detailPrototypeArray1[this.m_PrototypeIndex].dryColor = this.m_DryColor;
      detailPrototypeArray1[this.m_PrototypeIndex].renderMode = !this.m_Billboard ? DetailRenderMode.Grass : DetailRenderMode.GrassBillboard;
      detailPrototypeArray1[this.m_PrototypeIndex].usePrototypeMesh = false;
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
      this.m_MinHeight = Mathf.Max(0.0f, this.m_MinHeight);
      this.m_MaxHeight = Mathf.Max(this.m_MinHeight, this.m_MaxHeight);
      this.m_MinWidth = Mathf.Max(0.0f, this.m_MinWidth);
      this.m_MaxWidth = Mathf.Max(this.m_MinWidth, this.m_MaxWidth);
      base.OnWizardUpdate();
      if ((UnityEngine.Object) this.m_DetailTexture == (UnityEngine.Object) null)
      {
        this.errorString = "Please assign a detail texture";
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
