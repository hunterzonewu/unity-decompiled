// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Text
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>The default Graphic to draw font data to screen.</para>
  /// </summary>
  [AddComponentMenu("UI/Text", 10)]
  public class Text : MaskableGraphic, ILayoutElement
  {
    [SerializeField]
    private FontData m_FontData = FontData.defaultFontData;
    [TextArea(3, 10)]
    [SerializeField]
    protected string m_Text = string.Empty;
    private readonly UIVertex[] m_TempVerts = new UIVertex[4];
    private Font m_LastTrackedFont;
    private TextGenerator m_TextCache;
    private TextGenerator m_TextCacheForLayout;
    protected static Material s_DefaultText;
    [NonSerialized]
    protected bool m_DisableFontTextureRebuiltCallback;

    /// <summary>
    ///   <para>The cached TextGenerator used when generating visible Text.</para>
    /// </summary>
    public TextGenerator cachedTextGenerator
    {
      get
      {
        return this.m_TextCache ?? (this.m_TextCache = this.m_Text.Length == 0 ? new TextGenerator() : new TextGenerator(this.m_Text.Length));
      }
    }

    /// <summary>
    ///   <para>The cached TextGenerator used when determine Layout.</para>
    /// </summary>
    public TextGenerator cachedTextGeneratorForLayout
    {
      get
      {
        return this.m_TextCacheForLayout ?? (this.m_TextCacheForLayout = new TextGenerator());
      }
    }

    /// <summary>
    ///   <para>The Texture that comes from the Font.</para>
    /// </summary>
    public override Texture mainTexture
    {
      get
      {
        if ((UnityEngine.Object) this.font != (UnityEngine.Object) null && (UnityEngine.Object) this.font.material != (UnityEngine.Object) null && (UnityEngine.Object) this.font.material.mainTexture != (UnityEngine.Object) null)
          return this.font.material.mainTexture;
        if ((UnityEngine.Object) this.m_Material != (UnityEngine.Object) null)
          return this.m_Material.mainTexture;
        return base.mainTexture;
      }
    }

    /// <summary>
    ///   <para>The Font used by the text.</para>
    /// </summary>
    public Font font
    {
      get
      {
        return this.m_FontData.font;
      }
      set
      {
        if ((UnityEngine.Object) this.m_FontData.font == (UnityEngine.Object) value)
          return;
        FontUpdateTracker.UntrackText(this);
        this.m_FontData.font = value;
        FontUpdateTracker.TrackText(this);
        this.m_LastTrackedFont = value;
        this.SetAllDirty();
      }
    }

    /// <summary>
    ///   <para>The string value this text will display.</para>
    /// </summary>
    public virtual string text
    {
      get
      {
        return this.m_Text;
      }
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          if (string.IsNullOrEmpty(this.m_Text))
            return;
          this.m_Text = string.Empty;
          this.SetVerticesDirty();
        }
        else
        {
          if (!(this.m_Text != value))
            return;
          this.m_Text = value;
          this.SetVerticesDirty();
          this.SetLayoutDirty();
        }
      }
    }

    /// <summary>
    ///   <para>Whether this Text will support rich text.</para>
    /// </summary>
    public bool supportRichText
    {
      get
      {
        return this.m_FontData.richText;
      }
      set
      {
        if (this.m_FontData.richText == value)
          return;
        this.m_FontData.richText = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>Should the text be allowed to auto resized.</para>
    /// </summary>
    public bool resizeTextForBestFit
    {
      get
      {
        return this.m_FontData.bestFit;
      }
      set
      {
        if (this.m_FontData.bestFit == value)
          return;
        this.m_FontData.bestFit = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>The minimum size the text is allowed to be.</para>
    /// </summary>
    public int resizeTextMinSize
    {
      get
      {
        return this.m_FontData.minSize;
      }
      set
      {
        if (this.m_FontData.minSize == value)
          return;
        this.m_FontData.minSize = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>The maximum size the text is allowed to be. 1 = infinitly large.</para>
    /// </summary>
    public int resizeTextMaxSize
    {
      get
      {
        return this.m_FontData.maxSize;
      }
      set
      {
        if (this.m_FontData.maxSize == value)
          return;
        this.m_FontData.maxSize = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>The positioning of the text reliative to its RectTransform.</para>
    /// </summary>
    public TextAnchor alignment
    {
      get
      {
        return this.m_FontData.alignment;
      }
      set
      {
        if (this.m_FontData.alignment == value)
          return;
        this.m_FontData.alignment = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>Use the extents of glyph geometry to perform horizontal alignment rather than glyph metrics.</para>
    /// </summary>
    public bool alignByGeometry
    {
      get
      {
        return this.m_FontData.alignByGeometry;
      }
      set
      {
        if (this.m_FontData.alignByGeometry == value)
          return;
        this.m_FontData.alignByGeometry = value;
        this.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>The size that the Font should render at.</para>
    /// </summary>
    public int fontSize
    {
      get
      {
        return this.m_FontData.fontSize;
      }
      set
      {
        if (this.m_FontData.fontSize == value)
          return;
        this.m_FontData.fontSize = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>Horizontal overflow mode.</para>
    /// </summary>
    public HorizontalWrapMode horizontalOverflow
    {
      get
      {
        return this.m_FontData.horizontalOverflow;
      }
      set
      {
        if (this.m_FontData.horizontalOverflow == value)
          return;
        this.m_FontData.horizontalOverflow = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>Vertical overflow mode.</para>
    /// </summary>
    public VerticalWrapMode verticalOverflow
    {
      get
      {
        return this.m_FontData.verticalOverflow;
      }
      set
      {
        if (this.m_FontData.verticalOverflow == value)
          return;
        this.m_FontData.verticalOverflow = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>Line spacing, specified as a factor of font line height. A value of 1 will produce normal line spacing.</para>
    /// </summary>
    public float lineSpacing
    {
      get
      {
        return this.m_FontData.lineSpacing;
      }
      set
      {
        if ((double) this.m_FontData.lineSpacing == (double) value)
          return;
        this.m_FontData.lineSpacing = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>FontStyle used by the text.</para>
    /// </summary>
    public FontStyle fontStyle
    {
      get
      {
        return this.m_FontData.fontStyle;
      }
      set
      {
        if (this.m_FontData.fontStyle == value)
          return;
        this.m_FontData.fontStyle = value;
        this.SetVerticesDirty();
        this.SetLayoutDirty();
      }
    }

    /// <summary>
    ///   <para>(Read Only) Provides information about how fonts are scale to the screen.</para>
    /// </summary>
    public float pixelsPerUnit
    {
      get
      {
        Canvas canvas = this.canvas;
        if (!(bool) ((UnityEngine.Object) canvas))
          return 1f;
        if (!(bool) ((UnityEngine.Object) this.font) || this.font.dynamic)
          return canvas.scaleFactor;
        if (this.m_FontData.fontSize <= 0 || this.font.fontSize <= 0)
          return 1f;
        return (float) this.font.fontSize / (float) this.m_FontData.fontSize;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float minWidth
    {
      get
      {
        return 0.0f;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float preferredWidth
    {
      get
      {
        return this.cachedTextGeneratorForLayout.GetPreferredWidth(this.m_Text, this.GetGenerationSettings(Vector2.zero)) / this.pixelsPerUnit;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float flexibleWidth
    {
      get
      {
        return -1f;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float minHeight
    {
      get
      {
        return 0.0f;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float preferredHeight
    {
      get
      {
        return this.cachedTextGeneratorForLayout.GetPreferredHeight(this.m_Text, this.GetGenerationSettings(new Vector2(this.rectTransform.rect.size.x, 0.0f))) / this.pixelsPerUnit;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual float flexibleHeight
    {
      get
      {
        return -1f;
      }
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual int layoutPriority
    {
      get
      {
        return 0;
      }
    }

    protected Text()
    {
      this.useLegacyMeshGeneration = false;
    }

    /// <summary>
    ///   <para>Called by the [FontUpdateTracker] when the texture associated with a font is modified.</para>
    /// </summary>
    public void FontTextureChanged()
    {
      if (!(bool) ((UnityEngine.Object) this))
      {
        FontUpdateTracker.UntrackText(this);
      }
      else
      {
        if (this.m_DisableFontTextureRebuiltCallback)
          return;
        this.cachedTextGenerator.Invalidate();
        if (!this.IsActive())
          return;
        if (CanvasUpdateRegistry.IsRebuildingGraphics() || CanvasUpdateRegistry.IsRebuildingLayout())
          this.UpdateGeometry();
        else
          this.SetAllDirty();
      }
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.cachedTextGenerator.Invalidate();
      FontUpdateTracker.TrackText(this);
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      FontUpdateTracker.UntrackText(this);
      base.OnDisable();
    }

    protected override void UpdateGeometry()
    {
      if (!((UnityEngine.Object) this.font != (UnityEngine.Object) null))
        return;
      base.UpdateGeometry();
    }

    protected override void Reset()
    {
      this.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    /// <summary>
    ///   <para>Convenience function to populate the generation setting for the text.</para>
    /// </summary>
    /// <param name="extents">The extents the text can draw in.</param>
    /// <returns>
    ///   <para>Generated settings.</para>
    /// </returns>
    public TextGenerationSettings GetGenerationSettings(Vector2 extents)
    {
      TextGenerationSettings generationSettings = new TextGenerationSettings();
      generationSettings.generationExtents = extents;
      if ((UnityEngine.Object) this.font != (UnityEngine.Object) null && this.font.dynamic)
      {
        generationSettings.fontSize = this.m_FontData.fontSize;
        generationSettings.resizeTextMinSize = this.m_FontData.minSize;
        generationSettings.resizeTextMaxSize = this.m_FontData.maxSize;
      }
      generationSettings.textAnchor = this.m_FontData.alignment;
      generationSettings.alignByGeometry = this.m_FontData.alignByGeometry;
      generationSettings.scaleFactor = this.pixelsPerUnit;
      generationSettings.color = this.color;
      generationSettings.font = this.font;
      generationSettings.pivot = this.rectTransform.pivot;
      generationSettings.richText = this.m_FontData.richText;
      generationSettings.lineSpacing = this.m_FontData.lineSpacing;
      generationSettings.fontStyle = this.m_FontData.fontStyle;
      generationSettings.resizeTextForBestFit = this.m_FontData.bestFit;
      generationSettings.updateBounds = false;
      generationSettings.horizontalOverflow = this.m_FontData.horizontalOverflow;
      generationSettings.verticalOverflow = this.m_FontData.verticalOverflow;
      return generationSettings;
    }

    /// <summary>
    ///   <para>Convenience function to determine the vector offset of the anchor.</para>
    /// </summary>
    /// <param name="anchor"></param>
    public static Vector2 GetTextAnchorPivot(TextAnchor anchor)
    {
      switch (anchor)
      {
        case TextAnchor.UpperLeft:
          return new Vector2(0.0f, 1f);
        case TextAnchor.UpperCenter:
          return new Vector2(0.5f, 1f);
        case TextAnchor.UpperRight:
          return new Vector2(1f, 1f);
        case TextAnchor.MiddleLeft:
          return new Vector2(0.0f, 0.5f);
        case TextAnchor.MiddleCenter:
          return new Vector2(0.5f, 0.5f);
        case TextAnchor.MiddleRight:
          return new Vector2(1f, 0.5f);
        case TextAnchor.LowerLeft:
          return new Vector2(0.0f, 0.0f);
        case TextAnchor.LowerCenter:
          return new Vector2(0.5f, 0.0f);
        case TextAnchor.LowerRight:
          return new Vector2(1f, 0.0f);
        default:
          return Vector2.zero;
      }
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
      if ((UnityEngine.Object) this.font == (UnityEngine.Object) null)
        return;
      this.m_DisableFontTextureRebuiltCallback = true;
      this.cachedTextGenerator.Populate(this.text, this.GetGenerationSettings(this.rectTransform.rect.size));
      Rect rect = this.rectTransform.rect;
      Vector2 textAnchorPivot = Text.GetTextAnchorPivot(this.m_FontData.alignment);
      Vector2 zero = Vector2.zero;
      zero.x = Mathf.Lerp(rect.xMin, rect.xMax, textAnchorPivot.x);
      zero.y = Mathf.Lerp(rect.yMin, rect.yMax, textAnchorPivot.y);
      Vector2 vector2 = this.PixelAdjustPoint(zero) - zero;
      IList<UIVertex> verts = this.cachedTextGenerator.verts;
      float num1 = 1f / this.pixelsPerUnit;
      int num2 = verts.Count - 4;
      toFill.Clear();
      if (vector2 != Vector2.zero)
      {
        for (int index1 = 0; index1 < num2; ++index1)
        {
          int index2 = index1 & 3;
          this.m_TempVerts[index2] = verts[index1];
          this.m_TempVerts[index2].position *= num1;
          this.m_TempVerts[index2].position.x += vector2.x;
          this.m_TempVerts[index2].position.y += vector2.y;
          if (index2 == 3)
            toFill.AddUIVertexQuad(this.m_TempVerts);
        }
      }
      else
      {
        for (int index1 = 0; index1 < num2; ++index1)
        {
          int index2 = index1 & 3;
          this.m_TempVerts[index2] = verts[index1];
          this.m_TempVerts[index2].position *= num1;
          if (index2 == 3)
            toFill.AddUIVertexQuad(this.m_TempVerts);
        }
      }
      this.m_DisableFontTextureRebuiltCallback = false;
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual void CalculateLayoutInputHorizontal()
    {
    }

    /// <summary>
    ///   <para>Called by the layout system.</para>
    /// </summary>
    public virtual void CalculateLayoutInputVertical()
    {
    }

    public override void OnRebuildRequested()
    {
      FontUpdateTracker.UntrackText(this);
      FontUpdateTracker.TrackText(this);
      this.cachedTextGenerator.Invalidate();
      base.OnRebuildRequested();
    }

    protected override void OnValidate()
    {
      if ((UnityEngine.Object) this.m_FontData.font != (UnityEngine.Object) this.m_LastTrackedFont)
      {
        Font font = this.m_FontData.font;
        this.m_FontData.font = this.m_LastTrackedFont;
        FontUpdateTracker.UntrackText(this);
        this.m_FontData.font = font;
        FontUpdateTracker.TrackText(this);
        this.m_LastTrackedFont = font;
      }
      base.OnValidate();
    }
  }
}
