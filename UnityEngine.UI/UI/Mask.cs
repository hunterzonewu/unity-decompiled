// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Mask
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A component for masking children elements.</para>
  /// </summary>
  [AddComponentMenu("UI/Mask", 13)]
  [ExecuteInEditMode]
  [DisallowMultipleComponent]
  [RequireComponent(typeof (RectTransform))]
  public class Mask : UIBehaviour, ICanvasRaycastFilter, IMaterialModifier
  {
    [FormerlySerializedAs("m_ShowGraphic")]
    [SerializeField]
    private bool m_ShowMaskGraphic = true;
    [NonSerialized]
    private RectTransform m_RectTransform;
    [NonSerialized]
    private Graphic m_Graphic;
    [NonSerialized]
    private Material m_MaskMaterial;
    [NonSerialized]
    private Material m_UnmaskMaterial;

    /// <summary>
    ///   <para>Cached RectTransform.</para>
    /// </summary>
    public RectTransform rectTransform
    {
      get
      {
        return this.m_RectTransform ?? (this.m_RectTransform = this.GetComponent<RectTransform>());
      }
    }

    /// <summary>
    ///   <para>Show the graphic that is associated with the Mask render area.</para>
    /// </summary>
    public bool showMaskGraphic
    {
      get
      {
        return this.m_ShowMaskGraphic;
      }
      set
      {
        if (this.m_ShowMaskGraphic == value)
          return;
        this.m_ShowMaskGraphic = value;
        if (!((UnityEngine.Object) this.graphic != (UnityEngine.Object) null))
          return;
        this.graphic.SetMaterialDirty();
      }
    }

    /// <summary>
    ///   <para>The graphic associated with the Mask.</para>
    /// </summary>
    public Graphic graphic
    {
      get
      {
        return this.m_Graphic ?? (this.m_Graphic = this.GetComponent<Graphic>());
      }
    }

    protected Mask()
    {
    }

    /// <summary>
    ///   <para>See:IMask.</para>
    /// </summary>
    [Obsolete("use Mask.enabled instead", true)]
    public virtual bool MaskEnabled()
    {
      throw new NotSupportedException();
    }

    /// <summary>
    ///   <para>See:IGraphicEnabledDisabled.</para>
    /// </summary>
    [Obsolete("Not used anymore.")]
    public virtual void OnSiblingGraphicEnabledDisabled()
    {
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      if ((UnityEngine.Object) this.graphic != (UnityEngine.Object) null)
      {
        this.graphic.canvasRenderer.hasPopInstruction = true;
        this.graphic.SetMaterialDirty();
      }
      MaskUtilities.NotifyStencilStateChanged((Component) this);
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      base.OnDisable();
      if ((UnityEngine.Object) this.graphic != (UnityEngine.Object) null)
      {
        this.graphic.SetMaterialDirty();
        this.graphic.canvasRenderer.hasPopInstruction = false;
        this.graphic.canvasRenderer.popMaterialCount = 0;
      }
      StencilMaterial.Remove(this.m_MaskMaterial);
      this.m_MaskMaterial = (Material) null;
      StencilMaterial.Remove(this.m_UnmaskMaterial);
      this.m_UnmaskMaterial = (Material) null;
      MaskUtilities.NotifyStencilStateChanged((Component) this);
    }

    protected override void OnValidate()
    {
      base.OnValidate();
      if (!this.IsActive())
        return;
      if ((UnityEngine.Object) this.graphic != (UnityEngine.Object) null)
        this.graphic.SetMaterialDirty();
      MaskUtilities.NotifyStencilStateChanged((Component) this);
    }

    /// <summary>
    ///   <para>See:ICanvasRaycastFilter.</para>
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="eventCamera"></param>
    public virtual bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
      if (!this.isActiveAndEnabled)
        return true;
      return RectTransformUtility.RectangleContainsScreenPoint(this.rectTransform, sp, eventCamera);
    }

    /// <summary>
    ///   <para>See: IMaterialModifier.</para>
    /// </summary>
    /// <param name="baseMaterial"></param>
    public virtual Material GetModifiedMaterial(Material baseMaterial)
    {
      if ((UnityEngine.Object) this.graphic == (UnityEngine.Object) null || !this.isActiveAndEnabled)
        return baseMaterial;
      int stencilDepth = MaskUtilities.GetStencilDepth(this.transform, MaskUtilities.FindRootSortOverrideCanvas(this.transform));
      if (stencilDepth >= 8)
      {
        Debug.LogError((object) "Attempting to use a stencil mask with depth > 8", (UnityEngine.Object) this.gameObject);
        return baseMaterial;
      }
      int num = 1 << stencilDepth;
      if (num == 1)
      {
        Material material1 = StencilMaterial.Add(baseMaterial, 1, StencilOp.Replace, CompareFunction.Always, !this.m_ShowMaskGraphic ? (ColorWriteMask) 0 : ColorWriteMask.All);
        StencilMaterial.Remove(this.m_MaskMaterial);
        this.m_MaskMaterial = material1;
        Material material2 = StencilMaterial.Add(baseMaterial, 1, StencilOp.Zero, CompareFunction.Always, (ColorWriteMask) 0);
        StencilMaterial.Remove(this.m_UnmaskMaterial);
        this.m_UnmaskMaterial = material2;
        this.graphic.canvasRenderer.popMaterialCount = 1;
        this.graphic.canvasRenderer.SetPopMaterial(this.m_UnmaskMaterial, 0);
        return this.m_MaskMaterial;
      }
      Material material3 = StencilMaterial.Add(baseMaterial, num | num - 1, StencilOp.Replace, CompareFunction.Equal, !this.m_ShowMaskGraphic ? (ColorWriteMask) 0 : ColorWriteMask.All, num - 1, num | num - 1);
      StencilMaterial.Remove(this.m_MaskMaterial);
      this.m_MaskMaterial = material3;
      this.graphic.canvasRenderer.hasPopInstruction = true;
      Material material4 = StencilMaterial.Add(baseMaterial, num - 1, StencilOp.Replace, CompareFunction.Equal, (ColorWriteMask) 0, num - 1, num | num - 1);
      StencilMaterial.Remove(this.m_UnmaskMaterial);
      this.m_UnmaskMaterial = material4;
      this.graphic.canvasRenderer.popMaterialCount = 1;
      this.graphic.canvasRenderer.SetPopMaterial(this.m_UnmaskMaterial, 0);
      return this.m_MaskMaterial;
    }
  }
}
