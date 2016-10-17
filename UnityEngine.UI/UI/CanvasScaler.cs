// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.CanvasScaler
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>The Canvas Scaler component is used for controlling the overall scale and pixel density of UI elements in the Canvas. This scaling affects everything under the Canvas, including font sizes and image borders.</para>
  /// </summary>
  [ExecuteInEditMode]
  [RequireComponent(typeof (Canvas))]
  [AddComponentMenu("Layout/Canvas Scaler", 101)]
  public class CanvasScaler : UIBehaviour
  {
    [SerializeField]
    [Tooltip("If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.")]
    protected float m_ReferencePixelsPerUnit = 100f;
    [Tooltip("Scales all UI elements in the Canvas by this factor.")]
    [SerializeField]
    protected float m_ScaleFactor = 1f;
    [Tooltip("The resolution the UI layout is designed for. If the screen resolution is larger, the UI will be scaled up, and if it's smaller, the UI will be scaled down. This is done in accordance with the Screen Match Mode.")]
    [SerializeField]
    protected Vector2 m_ReferenceResolution = new Vector2(800f, 600f);
    [Tooltip("The physical unit to specify positions and sizes in.")]
    [SerializeField]
    protected CanvasScaler.Unit m_PhysicalUnit = CanvasScaler.Unit.Points;
    [Tooltip("The DPI to assume if the screen DPI is not known.")]
    [SerializeField]
    protected float m_FallbackScreenDPI = 96f;
    [SerializeField]
    [Tooltip("The pixels per inch to use for sprites that have a 'Pixels Per Unit' setting that matches the 'Reference Pixels Per Unit' setting.")]
    protected float m_DefaultSpriteDPI = 96f;
    [Tooltip("The amount of pixels per unit to use for dynamically created bitmaps in the UI, such as Text.")]
    [SerializeField]
    protected float m_DynamicPixelsPerUnit = 1f;
    [NonSerialized]
    private float m_PrevScaleFactor = 1f;
    [NonSerialized]
    private float m_PrevReferencePixelsPerUnit = 100f;
    private const float kLogBase = 2f;
    [SerializeField]
    [Tooltip("Determines how UI elements in the Canvas are scaled.")]
    private CanvasScaler.ScaleMode m_UiScaleMode;
    [SerializeField]
    [Tooltip("A mode used to scale the canvas area if the aspect ratio of the current resolution doesn't fit the reference resolution.")]
    protected CanvasScaler.ScreenMatchMode m_ScreenMatchMode;
    [SerializeField]
    [Range(0.0f, 1f)]
    [Tooltip("Determines if the scaling is using the width or height as reference, or a mix in between.")]
    protected float m_MatchWidthOrHeight;
    private Canvas m_Canvas;

    /// <summary>
    ///   <para>Determines how UI elements in the Canvas are scaled.</para>
    /// </summary>
    public CanvasScaler.ScaleMode uiScaleMode
    {
      get
      {
        return this.m_UiScaleMode;
      }
      set
      {
        this.m_UiScaleMode = value;
      }
    }

    /// <summary>
    ///   <para>If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.</para>
    /// </summary>
    public float referencePixelsPerUnit
    {
      get
      {
        return this.m_ReferencePixelsPerUnit;
      }
      set
      {
        this.m_ReferencePixelsPerUnit = value;
      }
    }

    /// <summary>
    ///   <para>Scales all UI elements in the Canvas by this factor.</para>
    /// </summary>
    public float scaleFactor
    {
      get
      {
        return this.m_ScaleFactor;
      }
      set
      {
        this.m_ScaleFactor = Mathf.Max(0.01f, value);
      }
    }

    /// <summary>
    ///   <para>The resolution the UI layout is designed for.</para>
    /// </summary>
    public Vector2 referenceResolution
    {
      get
      {
        return this.m_ReferenceResolution;
      }
      set
      {
        this.m_ReferenceResolution = value;
        if ((double) this.m_ReferenceResolution.x > -9.99999974737875E-06 && (double) this.m_ReferenceResolution.x < 9.99999974737875E-06)
          this.m_ReferenceResolution.x = 1E-05f * Mathf.Sign(this.m_ReferenceResolution.x);
        if ((double) this.m_ReferenceResolution.y <= -9.99999974737875E-06 || (double) this.m_ReferenceResolution.y >= 9.99999974737875E-06)
          return;
        this.m_ReferenceResolution.y = 1E-05f * Mathf.Sign(this.m_ReferenceResolution.y);
      }
    }

    /// <summary>
    ///   <para>A mode used to scale the canvas area if the aspect ratio of the current resolution doesn't fit the reference resolution.</para>
    /// </summary>
    public CanvasScaler.ScreenMatchMode screenMatchMode
    {
      get
      {
        return this.m_ScreenMatchMode;
      }
      set
      {
        this.m_ScreenMatchMode = value;
      }
    }

    /// <summary>
    ///   <para>Setting to scale the Canvas to match the width or height of the reference resolution, or a combination.</para>
    /// </summary>
    public float matchWidthOrHeight
    {
      get
      {
        return this.m_MatchWidthOrHeight;
      }
      set
      {
        this.m_MatchWidthOrHeight = value;
      }
    }

    /// <summary>
    ///   <para>The physical unit to specify positions and sizes in.</para>
    /// </summary>
    public CanvasScaler.Unit physicalUnit
    {
      get
      {
        return this.m_PhysicalUnit;
      }
      set
      {
        this.m_PhysicalUnit = value;
      }
    }

    /// <summary>
    ///   <para>The DPI to assume if the screen DPI is not known.</para>
    /// </summary>
    public float fallbackScreenDPI
    {
      get
      {
        return this.m_FallbackScreenDPI;
      }
      set
      {
        this.m_FallbackScreenDPI = value;
      }
    }

    /// <summary>
    ///   <para>The pixels per inch to use for sprites that have a 'Pixels Per Unit' setting that matches the 'Reference Pixels Per Unit' setting.</para>
    /// </summary>
    public float defaultSpriteDPI
    {
      get
      {
        return this.m_DefaultSpriteDPI;
      }
      set
      {
        this.m_DefaultSpriteDPI = value;
      }
    }

    /// <summary>
    ///   <para>The amount of pixels per unit to use for dynamically created bitmaps in the UI, such as Text.</para>
    /// </summary>
    public float dynamicPixelsPerUnit
    {
      get
      {
        return this.m_DynamicPixelsPerUnit;
      }
      set
      {
        this.m_DynamicPixelsPerUnit = value;
      }
    }

    protected CanvasScaler()
    {
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_Canvas = this.GetComponent<Canvas>();
      this.Handle();
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      this.SetScaleFactor(1f);
      this.SetReferencePixelsPerUnit(100f);
      base.OnDisable();
    }

    /// <summary>
    ///   <para>Handles per-frame checking if the canvas scaling needs to be updated.</para>
    /// </summary>
    protected virtual void Update()
    {
      this.Handle();
    }

    /// <summary>
    ///   <para>Method that handles calculations of canvas scaling.</para>
    /// </summary>
    protected virtual void Handle()
    {
      if ((UnityEngine.Object) this.m_Canvas == (UnityEngine.Object) null || !this.m_Canvas.isRootCanvas)
        return;
      if (this.m_Canvas.renderMode == RenderMode.WorldSpace)
      {
        this.HandleWorldCanvas();
      }
      else
      {
        switch (this.m_UiScaleMode)
        {
          case CanvasScaler.ScaleMode.ConstantPixelSize:
            this.HandleConstantPixelSize();
            break;
          case CanvasScaler.ScaleMode.ScaleWithScreenSize:
            this.HandleScaleWithScreenSize();
            break;
          case CanvasScaler.ScaleMode.ConstantPhysicalSize:
            this.HandleConstantPhysicalSize();
            break;
        }
      }
    }

    /// <summary>
    ///   <para>Handles canvas scaling for world canvas.</para>
    /// </summary>
    protected virtual void HandleWorldCanvas()
    {
      this.SetScaleFactor(this.m_DynamicPixelsPerUnit);
      this.SetReferencePixelsPerUnit(this.m_ReferencePixelsPerUnit);
    }

    /// <summary>
    ///   <para>Handles canvas scaling for a constant pixel size.</para>
    /// </summary>
    protected virtual void HandleConstantPixelSize()
    {
      this.SetScaleFactor(this.m_ScaleFactor);
      this.SetReferencePixelsPerUnit(this.m_ReferencePixelsPerUnit);
    }

    /// <summary>
    ///   <para>Handles canvas scaling that scales with the screen size.</para>
    /// </summary>
    protected virtual void HandleScaleWithScreenSize()
    {
      Vector2 vector2 = new Vector2((float) Screen.width, (float) Screen.height);
      float scaleFactor = 0.0f;
      switch (this.m_ScreenMatchMode)
      {
        case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
          scaleFactor = Mathf.Pow(2f, Mathf.Lerp(Mathf.Log(vector2.x / this.m_ReferenceResolution.x, 2f), Mathf.Log(vector2.y / this.m_ReferenceResolution.y, 2f), this.m_MatchWidthOrHeight));
          break;
        case CanvasScaler.ScreenMatchMode.Expand:
          scaleFactor = Mathf.Min(vector2.x / this.m_ReferenceResolution.x, vector2.y / this.m_ReferenceResolution.y);
          break;
        case CanvasScaler.ScreenMatchMode.Shrink:
          scaleFactor = Mathf.Max(vector2.x / this.m_ReferenceResolution.x, vector2.y / this.m_ReferenceResolution.y);
          break;
      }
      this.SetScaleFactor(scaleFactor);
      this.SetReferencePixelsPerUnit(this.m_ReferencePixelsPerUnit);
    }

    /// <summary>
    ///   <para>Handles canvas scaling for a constant physical size.</para>
    /// </summary>
    protected virtual void HandleConstantPhysicalSize()
    {
      float dpi = Screen.dpi;
      float num1 = (double) dpi != 0.0 ? dpi : this.m_FallbackScreenDPI;
      float num2 = 1f;
      switch (this.m_PhysicalUnit)
      {
        case CanvasScaler.Unit.Centimeters:
          num2 = 2.54f;
          break;
        case CanvasScaler.Unit.Millimeters:
          num2 = 25.4f;
          break;
        case CanvasScaler.Unit.Inches:
          num2 = 1f;
          break;
        case CanvasScaler.Unit.Points:
          num2 = 72f;
          break;
        case CanvasScaler.Unit.Picas:
          num2 = 6f;
          break;
      }
      this.SetScaleFactor(num1 / num2);
      this.SetReferencePixelsPerUnit(this.m_ReferencePixelsPerUnit * num2 / this.m_DefaultSpriteDPI);
    }

    /// <summary>
    ///   <para>Sets the scale factor on the canvas.</para>
    /// </summary>
    /// <param name="scaleFactor">The scale factor to use.</param>
    protected void SetScaleFactor(float scaleFactor)
    {
      if ((double) scaleFactor == (double) this.m_PrevScaleFactor)
        return;
      this.m_Canvas.scaleFactor = scaleFactor;
      this.m_PrevScaleFactor = scaleFactor;
    }

    /// <summary>
    ///   <para>Sets the referencePixelsPerUnit on the Canvas.</para>
    /// </summary>
    /// <param name="referencePixelsPerUnit"></param>
    protected void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
    {
      if ((double) referencePixelsPerUnit == (double) this.m_PrevReferencePixelsPerUnit)
        return;
      this.m_Canvas.referencePixelsPerUnit = referencePixelsPerUnit;
      this.m_PrevReferencePixelsPerUnit = referencePixelsPerUnit;
    }

    protected override void OnValidate()
    {
      this.m_ScaleFactor = Mathf.Max(0.01f, this.m_ScaleFactor);
    }

    /// <summary>
    ///   <para>Determines how UI elements in the Canvas are scaled.</para>
    /// </summary>
    public enum ScaleMode
    {
      ConstantPixelSize,
      ScaleWithScreenSize,
      ConstantPhysicalSize,
    }

    /// <summary>
    ///   <para>Scale the canvas area with the width as reference, the height as reference, or something in between.</para>
    /// </summary>
    public enum ScreenMatchMode
    {
      MatchWidthOrHeight,
      Expand,
      Shrink,
    }

    /// <summary>
    ///   <para>Settings used to specify a physical unit.</para>
    /// </summary>
    public enum Unit
    {
      Centimeters,
      Millimeters,
      Inches,
      Points,
      Picas,
    }
  }
}
