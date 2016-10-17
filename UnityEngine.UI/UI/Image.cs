// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Image
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEngine.Serialization;
using UnityEngine.Sprites;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Displays a Sprite for the UI System.</para>
  /// </summary>
  [AddComponentMenu("UI/Image", 11)]
  public class Image : MaskableGraphic, ICanvasRaycastFilter, ISerializationCallbackReceiver, ILayoutElement
  {
    private static readonly Vector2[] s_VertScratch = new Vector2[4];
    private static readonly Vector2[] s_UVScratch = new Vector2[4];
    private static readonly Vector3[] s_Xy = new Vector3[4];
    private static readonly Vector3[] s_Uv = new Vector3[4];
    [SerializeField]
    private bool m_FillCenter = true;
    [SerializeField]
    private Image.FillMethod m_FillMethod = Image.FillMethod.Radial360;
    [Range(0.0f, 1f)]
    [SerializeField]
    private float m_FillAmount = 1f;
    [SerializeField]
    private bool m_FillClockwise = true;
    private float m_EventAlphaThreshold = 1f;
    [FormerlySerializedAs("m_Frame")]
    [SerializeField]
    private Sprite m_Sprite;
    [NonSerialized]
    private Sprite m_OverrideSprite;
    [SerializeField]
    private Image.Type m_Type;
    [SerializeField]
    private bool m_PreserveAspect;
    [SerializeField]
    private int m_FillOrigin;

    /// <summary>
    ///   <para>The sprite that is used to render this image.</para>
    /// </summary>
    public Sprite sprite
    {
      get
      {
        return this.m_Sprite;
      }
      set
      {
        if (!SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
          return;
        this.SetAllDirty();
      }
    }

    /// <summary>
    ///   <para>Set an override sprite to be used for rendering.</para>
    /// </summary>
    public Sprite overrideSprite
    {
      get
      {
        if ((UnityEngine.Object) this.m_OverrideSprite == (UnityEngine.Object) null)
          return this.sprite;
        return this.m_OverrideSprite;
      }
      set
      {
        if (!SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
          return;
        this.SetAllDirty();
      }
    }

    /// <summary>
    ///   <para>How the Image is draw.</para>
    /// </summary>
    public Image.Type type
    {
      get
      {
        return this.m_Type;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<Image.Type>(ref this.m_Type, value))
          return;
        this.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>Whether this image should preserve its Sprite aspect ratio.</para>
    /// </summary>
    public bool preserveAspect
    {
      get
      {
        return this.m_PreserveAspect;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<bool>(ref this.m_PreserveAspect, value))
          return;
        this.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>Whether or not to render the center of a Tiled or Sliced image.</para>
    /// </summary>
    public bool fillCenter
    {
      get
      {
        return this.m_FillCenter;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<bool>(ref this.m_FillCenter, value))
          return;
        this.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>What type of fill method to use.</para>
    /// </summary>
    public Image.FillMethod fillMethod
    {
      get
      {
        return this.m_FillMethod;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<Image.FillMethod>(ref this.m_FillMethod, value))
          return;
        this.SetVerticesDirty();
        this.m_FillOrigin = 0;
      }
    }

    /// <summary>
    ///   <para>Amount of the Image shown when the Image.type is set to Image.Type.Filled.</para>
    /// </summary>
    public float fillAmount
    {
      get
      {
        return this.m_FillAmount;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_FillAmount, Mathf.Clamp01(value)))
          return;
        this.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>Whether the Image should be filled clockwise (true) or counter-clockwise (false).</para>
    /// </summary>
    public bool fillClockwise
    {
      get
      {
        return this.m_FillClockwise;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<bool>(ref this.m_FillClockwise, value))
          return;
        this.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>Controls the origin point of the Fill process. Value means different things with each fill method.</para>
    /// </summary>
    public int fillOrigin
    {
      get
      {
        return this.m_FillOrigin;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<int>(ref this.m_FillOrigin, value))
          return;
        this.SetVerticesDirty();
      }
    }

    /// <summary>
    ///   <para>The alpha threshold specifying the minimum alpha a pixel must have for the event to be passed through.</para>
    /// </summary>
    public float eventAlphaThreshold
    {
      get
      {
        return this.m_EventAlphaThreshold;
      }
      set
      {
        this.m_EventAlphaThreshold = value;
      }
    }

    /// <summary>
    ///   <para>The image's texture. (ReadOnly).</para>
    /// </summary>
    public override Texture mainTexture
    {
      get
      {
        if (!((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null))
          return (Texture) this.overrideSprite.texture;
        if ((UnityEngine.Object) this.material != (UnityEngine.Object) null && (UnityEngine.Object) this.material.mainTexture != (UnityEngine.Object) null)
          return this.material.mainTexture;
        return (Texture) Graphic.s_WhiteTexture;
      }
    }

    /// <summary>
    ///   <para>True if the sprite used has borders.</para>
    /// </summary>
    public bool hasBorder
    {
      get
      {
        if ((UnityEngine.Object) this.overrideSprite != (UnityEngine.Object) null)
          return (double) this.overrideSprite.border.sqrMagnitude > 0.0;
        return false;
      }
    }

    public float pixelsPerUnit
    {
      get
      {
        float num1 = 100f;
        if ((bool) ((UnityEngine.Object) this.sprite))
          num1 = this.sprite.pixelsPerUnit;
        float num2 = 100f;
        if ((bool) ((UnityEngine.Object) this.canvas))
          num2 = this.canvas.referencePixelsPerUnit;
        return num1 / num2;
      }
    }

    /// <summary>
    ///   <para>See ILayoutElement.minWidth.</para>
    /// </summary>
    public virtual float minWidth
    {
      get
      {
        return 0.0f;
      }
    }

    /// <summary>
    ///   <para>See ILayoutElement.preferredWidth.</para>
    /// </summary>
    public virtual float preferredWidth
    {
      get
      {
        if ((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null)
          return 0.0f;
        if (this.type == Image.Type.Sliced || this.type == Image.Type.Tiled)
          return DataUtility.GetMinSize(this.overrideSprite).x / this.pixelsPerUnit;
        return this.overrideSprite.rect.size.x / this.pixelsPerUnit;
      }
    }

    /// <summary>
    ///   <para>See ILayoutElement.flexibleWidth.</para>
    /// </summary>
    public virtual float flexibleWidth
    {
      get
      {
        return -1f;
      }
    }

    /// <summary>
    ///   <para>See ILayoutElement.minHeight.</para>
    /// </summary>
    public virtual float minHeight
    {
      get
      {
        return 0.0f;
      }
    }

    /// <summary>
    ///   <para>See ILayoutElement.preferredHeight.</para>
    /// </summary>
    public virtual float preferredHeight
    {
      get
      {
        if ((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null)
          return 0.0f;
        if (this.type == Image.Type.Sliced || this.type == Image.Type.Tiled)
          return DataUtility.GetMinSize(this.overrideSprite).y / this.pixelsPerUnit;
        return this.overrideSprite.rect.size.y / this.pixelsPerUnit;
      }
    }

    /// <summary>
    ///   <para>See ILayoutElement.flexibleHeight.</para>
    /// </summary>
    public virtual float flexibleHeight
    {
      get
      {
        return -1f;
      }
    }

    /// <summary>
    ///   <para>See ILayoutElement.layoutPriority.</para>
    /// </summary>
    public virtual int layoutPriority
    {
      get
      {
        return 0;
      }
    }

    protected Image()
    {
      this.useLegacyMeshGeneration = false;
    }

    /// <summary>
    ///   <para>Serialization Callback.</para>
    /// </summary>
    public virtual void OnBeforeSerialize()
    {
    }

    /// <summary>
    ///   <para>Serialization Callback.</para>
    /// </summary>
    public virtual void OnAfterDeserialize()
    {
      if (this.m_FillOrigin < 0)
        this.m_FillOrigin = 0;
      else if (this.m_FillMethod == Image.FillMethod.Horizontal && this.m_FillOrigin > 1)
        this.m_FillOrigin = 0;
      else if (this.m_FillMethod == Image.FillMethod.Vertical && this.m_FillOrigin > 1)
        this.m_FillOrigin = 0;
      else if (this.m_FillOrigin > 3)
        this.m_FillOrigin = 0;
      this.m_FillAmount = Mathf.Clamp(this.m_FillAmount, 0.0f, 1f);
    }

    private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
    {
      Vector4 vector4_1 = !((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null) ? DataUtility.GetPadding(this.overrideSprite) : Vector4.zero;
      Vector2 vector2 = !((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null) ? new Vector2(this.overrideSprite.rect.width, this.overrideSprite.rect.height) : Vector2.zero;
      Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
      int num1 = Mathf.RoundToInt(vector2.x);
      int num2 = Mathf.RoundToInt(vector2.y);
      Vector4 vector4_2 = new Vector4(vector4_1.x / (float) num1, vector4_1.y / (float) num2, ((float) num1 - vector4_1.z) / (float) num1, ((float) num2 - vector4_1.w) / (float) num2);
      if (shouldPreserveAspect && (double) vector2.sqrMagnitude > 0.0)
      {
        float num3 = vector2.x / vector2.y;
        float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
        if ((double) num3 > (double) num4)
        {
          float height = pixelAdjustedRect.height;
          pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
          pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * this.rectTransform.pivot.y;
        }
        else
        {
          float width = pixelAdjustedRect.width;
          pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
          pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * this.rectTransform.pivot.x;
        }
      }
      vector4_2 = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector4_2.x, pixelAdjustedRect.y + pixelAdjustedRect.height * vector4_2.y, pixelAdjustedRect.x + pixelAdjustedRect.width * vector4_2.z, pixelAdjustedRect.y + pixelAdjustedRect.height * vector4_2.w);
      return vector4_2;
    }

    /// <summary>
    ///   <para>Adjusts the image size to make it pixel-perfect.</para>
    /// </summary>
    public override void SetNativeSize()
    {
      if (!((UnityEngine.Object) this.overrideSprite != (UnityEngine.Object) null))
        return;
      float x = this.overrideSprite.rect.width / this.pixelsPerUnit;
      float y = this.overrideSprite.rect.height / this.pixelsPerUnit;
      this.rectTransform.anchorMax = this.rectTransform.anchorMin;
      this.rectTransform.sizeDelta = new Vector2(x, y);
      this.SetAllDirty();
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
      if ((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null)
      {
        base.OnPopulateMesh(toFill);
      }
      else
      {
        switch (this.type)
        {
          case Image.Type.Simple:
            this.GenerateSimpleSprite(toFill, this.m_PreserveAspect);
            break;
          case Image.Type.Sliced:
            this.GenerateSlicedSprite(toFill);
            break;
          case Image.Type.Tiled:
            this.GenerateTiledSprite(toFill);
            break;
          case Image.Type.Filled:
            this.GenerateFilledSprite(toFill, this.m_PreserveAspect);
            break;
        }
      }
    }

    private void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
    {
      Vector4 drawingDimensions = this.GetDrawingDimensions(lPreserveAspect);
      Vector4 vector4 = !((UnityEngine.Object) this.overrideSprite != (UnityEngine.Object) null) ? Vector4.zero : DataUtility.GetOuterUV(this.overrideSprite);
      Color color = this.color;
      vh.Clear();
      vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), (Color32) color, new Vector2(vector4.x, vector4.y));
      vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), (Color32) color, new Vector2(vector4.x, vector4.w));
      vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), (Color32) color, new Vector2(vector4.z, vector4.w));
      vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), (Color32) color, new Vector2(vector4.z, vector4.y));
      vh.AddTriangle(0, 1, 2);
      vh.AddTriangle(2, 3, 0);
    }

    private void GenerateSlicedSprite(VertexHelper toFill)
    {
      if (!this.hasBorder)
      {
        this.GenerateSimpleSprite(toFill, false);
      }
      else
      {
        Vector4 vector4_1;
        Vector4 vector4_2;
        Vector4 vector4_3;
        Vector4 vector4_4;
        if ((UnityEngine.Object) this.overrideSprite != (UnityEngine.Object) null)
        {
          vector4_1 = DataUtility.GetOuterUV(this.overrideSprite);
          vector4_2 = DataUtility.GetInnerUV(this.overrideSprite);
          vector4_3 = DataUtility.GetPadding(this.overrideSprite);
          vector4_4 = this.overrideSprite.border;
        }
        else
        {
          vector4_1 = Vector4.zero;
          vector4_2 = Vector4.zero;
          vector4_3 = Vector4.zero;
          vector4_4 = Vector4.zero;
        }
        Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
        Vector4 adjustedBorders = this.GetAdjustedBorders(vector4_4 / this.pixelsPerUnit, pixelAdjustedRect);
        Vector4 vector4_5 = vector4_3 / this.pixelsPerUnit;
        Image.s_VertScratch[0] = new Vector2(vector4_5.x, vector4_5.y);
        Image.s_VertScratch[3] = new Vector2(pixelAdjustedRect.width - vector4_5.z, pixelAdjustedRect.height - vector4_5.w);
        Image.s_VertScratch[1].x = adjustedBorders.x;
        Image.s_VertScratch[1].y = adjustedBorders.y;
        Image.s_VertScratch[2].x = pixelAdjustedRect.width - adjustedBorders.z;
        Image.s_VertScratch[2].y = pixelAdjustedRect.height - adjustedBorders.w;
        for (int index = 0; index < 4; ++index)
        {
          Image.s_VertScratch[index].x += pixelAdjustedRect.x;
          Image.s_VertScratch[index].y += pixelAdjustedRect.y;
        }
        Image.s_UVScratch[0] = new Vector2(vector4_1.x, vector4_1.y);
        Image.s_UVScratch[1] = new Vector2(vector4_2.x, vector4_2.y);
        Image.s_UVScratch[2] = new Vector2(vector4_2.z, vector4_2.w);
        Image.s_UVScratch[3] = new Vector2(vector4_1.z, vector4_1.w);
        toFill.Clear();
        for (int index1 = 0; index1 < 3; ++index1)
        {
          int index2 = index1 + 1;
          for (int index3 = 0; index3 < 3; ++index3)
          {
            if (this.m_FillCenter || index1 != 1 || index3 != 1)
            {
              int index4 = index3 + 1;
              Image.AddQuad(toFill, new Vector2(Image.s_VertScratch[index1].x, Image.s_VertScratch[index3].y), new Vector2(Image.s_VertScratch[index2].x, Image.s_VertScratch[index4].y), (Color32) this.color, new Vector2(Image.s_UVScratch[index1].x, Image.s_UVScratch[index3].y), new Vector2(Image.s_UVScratch[index2].x, Image.s_UVScratch[index4].y));
            }
          }
        }
      }
    }

    private void GenerateTiledSprite(VertexHelper toFill)
    {
      Vector4 vector4_1;
      Vector4 vector4_2;
      Vector4 vector4_3;
      Vector2 vector2_1;
      if ((UnityEngine.Object) this.overrideSprite != (UnityEngine.Object) null)
      {
        vector4_1 = DataUtility.GetOuterUV(this.overrideSprite);
        vector4_2 = DataUtility.GetInnerUV(this.overrideSprite);
        vector4_3 = this.overrideSprite.border;
        vector2_1 = this.overrideSprite.rect.size;
      }
      else
      {
        vector4_1 = Vector4.zero;
        vector4_2 = Vector4.zero;
        vector4_3 = Vector4.zero;
        vector2_1 = Vector2.one * 100f;
      }
      Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
      float num1 = (vector2_1.x - vector4_3.x - vector4_3.z) / this.pixelsPerUnit;
      float num2 = (vector2_1.y - vector4_3.y - vector4_3.w) / this.pixelsPerUnit;
      vector4_3 = this.GetAdjustedBorders(vector4_3 / this.pixelsPerUnit, pixelAdjustedRect);
      Vector2 uvMin = new Vector2(vector4_2.x, vector4_2.y);
      Vector2 vector2_2 = new Vector2(vector4_2.z, vector4_2.w);
      UIVertex.simpleVert.color = (Color32) this.color;
      float x1 = vector4_3.x;
      float x2 = pixelAdjustedRect.width - vector4_3.z;
      float y1 = vector4_3.y;
      float y2 = pixelAdjustedRect.height - vector4_3.w;
      toFill.Clear();
      Vector2 uvMax = vector2_2;
      if ((double) num1 == 0.0)
        num1 = x2 - x1;
      if ((double) num2 == 0.0)
        num2 = y2 - y1;
      if (this.m_FillCenter)
      {
        float y3 = y1;
        while ((double) y3 < (double) y2)
        {
          float y4 = y3 + num2;
          if ((double) y4 > (double) y2)
          {
            uvMax.y = uvMin.y + (float) (((double) vector2_2.y - (double) uvMin.y) * ((double) y2 - (double) y3) / ((double) y4 - (double) y3));
            y4 = y2;
          }
          uvMax.x = vector2_2.x;
          float x3 = x1;
          while ((double) x3 < (double) x2)
          {
            float x4 = x3 + num1;
            if ((double) x4 > (double) x2)
            {
              uvMax.x = uvMin.x + (float) (((double) vector2_2.x - (double) uvMin.x) * ((double) x2 - (double) x3) / ((double) x4 - (double) x3));
              x4 = x2;
            }
            Image.AddQuad(toFill, new Vector2(x3, y3) + pixelAdjustedRect.position, new Vector2(x4, y4) + pixelAdjustedRect.position, (Color32) this.color, uvMin, uvMax);
            x3 += num1;
          }
          y3 += num2;
        }
      }
      if (!this.hasBorder)
        return;
      Vector2 vector2_3 = vector2_2;
      float y5 = y1;
      while ((double) y5 < (double) y2)
      {
        float y3 = y5 + num2;
        if ((double) y3 > (double) y2)
        {
          vector2_3.y = uvMin.y + (float) (((double) vector2_2.y - (double) uvMin.y) * ((double) y2 - (double) y5) / ((double) y3 - (double) y5));
          y3 = y2;
        }
        Image.AddQuad(toFill, new Vector2(0.0f, y5) + pixelAdjustedRect.position, new Vector2(x1, y3) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, uvMin.y), new Vector2(uvMin.x, vector2_3.y));
        Image.AddQuad(toFill, new Vector2(x2, y5) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y3) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, uvMin.y), new Vector2(vector4_1.z, vector2_3.y));
        y5 += num2;
      }
      vector2_3 = vector2_2;
      float x5 = x1;
      while ((double) x5 < (double) x2)
      {
        float x3 = x5 + num1;
        if ((double) x3 > (double) x2)
        {
          vector2_3.x = uvMin.x + (float) (((double) vector2_2.x - (double) uvMin.x) * ((double) x2 - (double) x5) / ((double) x3 - (double) x5));
          x3 = x2;
        }
        Image.AddQuad(toFill, new Vector2(x5, 0.0f) + pixelAdjustedRect.position, new Vector2(x3, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(uvMin.x, vector4_1.y), new Vector2(vector2_3.x, uvMin.y));
        Image.AddQuad(toFill, new Vector2(x5, y2) + pixelAdjustedRect.position, new Vector2(x3, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(uvMin.x, vector2_2.y), new Vector2(vector2_3.x, vector4_1.w));
        x5 += num1;
      }
      Image.AddQuad(toFill, new Vector2(0.0f, 0.0f) + pixelAdjustedRect.position, new Vector2(x1, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, vector4_1.y), new Vector2(uvMin.x, uvMin.y));
      Image.AddQuad(toFill, new Vector2(x2, 0.0f) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y1) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, vector4_1.y), new Vector2(vector4_1.z, uvMin.y));
      Image.AddQuad(toFill, new Vector2(0.0f, y2) + pixelAdjustedRect.position, new Vector2(x1, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector4_1.x, vector2_2.y), new Vector2(uvMin.x, vector4_1.w));
      Image.AddQuad(toFill, new Vector2(x2, y2) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, pixelAdjustedRect.height) + pixelAdjustedRect.position, (Color32) this.color, new Vector2(vector2_2.x, vector2_2.y), new Vector2(vector4_1.z, vector4_1.w));
    }

    private static void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs)
    {
      int currentVertCount = vertexHelper.currentVertCount;
      for (int index = 0; index < 4; ++index)
        vertexHelper.AddVert(quadPositions[index], color, (Vector2) quadUVs[index]);
      vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
      vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    private static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
    {
      int currentVertCount = vertexHelper.currentVertCount;
      vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0.0f), color, new Vector2(uvMin.x, uvMin.y));
      vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0.0f), color, new Vector2(uvMin.x, uvMax.y));
      vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0.0f), color, new Vector2(uvMax.x, uvMax.y));
      vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0.0f), color, new Vector2(uvMax.x, uvMin.y));
      vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
      vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
    {
      for (int index = 0; index <= 1; ++index)
      {
        float num1 = border[index] + border[index + 2];
        if ((double) rect.size[index] < (double) num1 && (double) num1 != 0.0)
        {
          float num2 = rect.size[index] / num1;
          border[index] *= num2;
          border[index + 2] *= num2;
        }
      }
      return border;
    }

    private void GenerateFilledSprite(VertexHelper toFill, bool preserveAspect)
    {
      toFill.Clear();
      if ((double) this.m_FillAmount < 1.0 / 1000.0)
        return;
      Vector4 drawingDimensions = this.GetDrawingDimensions(preserveAspect);
      Vector4 vector4 = !((UnityEngine.Object) this.overrideSprite != (UnityEngine.Object) null) ? Vector4.zero : DataUtility.GetOuterUV(this.overrideSprite);
      UIVertex.simpleVert.color = (Color32) this.color;
      float num1 = vector4.x;
      float num2 = vector4.y;
      float num3 = vector4.z;
      float num4 = vector4.w;
      if (this.m_FillMethod == Image.FillMethod.Horizontal || this.m_FillMethod == Image.FillMethod.Vertical)
      {
        if (this.fillMethod == Image.FillMethod.Horizontal)
        {
          float num5 = (num3 - num1) * this.m_FillAmount;
          if (this.m_FillOrigin == 1)
          {
            drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.m_FillAmount;
            num1 = num3 - num5;
          }
          else
          {
            drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.m_FillAmount;
            num3 = num1 + num5;
          }
        }
        else if (this.fillMethod == Image.FillMethod.Vertical)
        {
          float num5 = (num4 - num2) * this.m_FillAmount;
          if (this.m_FillOrigin == 1)
          {
            drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.m_FillAmount;
            num2 = num4 - num5;
          }
          else
          {
            drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.m_FillAmount;
            num4 = num2 + num5;
          }
        }
      }
      Image.s_Xy[0] = (Vector3) new Vector2(drawingDimensions.x, drawingDimensions.y);
      Image.s_Xy[1] = (Vector3) new Vector2(drawingDimensions.x, drawingDimensions.w);
      Image.s_Xy[2] = (Vector3) new Vector2(drawingDimensions.z, drawingDimensions.w);
      Image.s_Xy[3] = (Vector3) new Vector2(drawingDimensions.z, drawingDimensions.y);
      Image.s_Uv[0] = (Vector3) new Vector2(num1, num2);
      Image.s_Uv[1] = (Vector3) new Vector2(num1, num4);
      Image.s_Uv[2] = (Vector3) new Vector2(num3, num4);
      Image.s_Uv[3] = (Vector3) new Vector2(num3, num2);
      if ((double) this.m_FillAmount < 1.0 && this.m_FillMethod != Image.FillMethod.Horizontal && this.m_FillMethod != Image.FillMethod.Vertical)
      {
        if (this.fillMethod == Image.FillMethod.Radial90)
        {
          if (!Image.RadialCut(Image.s_Xy, Image.s_Uv, this.m_FillAmount, this.m_FillClockwise, this.m_FillOrigin))
            return;
          Image.AddQuad(toFill, Image.s_Xy, (Color32) this.color, Image.s_Uv);
        }
        else if (this.fillMethod == Image.FillMethod.Radial180)
        {
          for (int index = 0; index < 2; ++index)
          {
            int num5 = this.m_FillOrigin <= 1 ? 0 : 1;
            float t1;
            float t2;
            float t3;
            float t4;
            if (this.m_FillOrigin == 0 || this.m_FillOrigin == 2)
            {
              t1 = 0.0f;
              t2 = 1f;
              if (index == num5)
              {
                t3 = 0.0f;
                t4 = 0.5f;
              }
              else
              {
                t3 = 0.5f;
                t4 = 1f;
              }
            }
            else
            {
              t3 = 0.0f;
              t4 = 1f;
              if (index == num5)
              {
                t1 = 0.5f;
                t2 = 1f;
              }
              else
              {
                t1 = 0.0f;
                t2 = 0.5f;
              }
            }
            Image.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
            Image.s_Xy[1].x = Image.s_Xy[0].x;
            Image.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
            Image.s_Xy[3].x = Image.s_Xy[2].x;
            Image.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t1);
            Image.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
            Image.s_Xy[2].y = Image.s_Xy[1].y;
            Image.s_Xy[3].y = Image.s_Xy[0].y;
            Image.s_Uv[0].x = Mathf.Lerp(num1, num3, t3);
            Image.s_Uv[1].x = Image.s_Uv[0].x;
            Image.s_Uv[2].x = Mathf.Lerp(num1, num3, t4);
            Image.s_Uv[3].x = Image.s_Uv[2].x;
            Image.s_Uv[0].y = Mathf.Lerp(num2, num4, t1);
            Image.s_Uv[1].y = Mathf.Lerp(num2, num4, t2);
            Image.s_Uv[2].y = Image.s_Uv[1].y;
            Image.s_Uv[3].y = Image.s_Uv[0].y;
            float num6 = !this.m_FillClockwise ? this.m_FillAmount * 2f - (float) (1 - index) : this.fillAmount * 2f - (float) index;
            if (Image.RadialCut(Image.s_Xy, Image.s_Uv, Mathf.Clamp01(num6), this.m_FillClockwise, (index + this.m_FillOrigin + 3) % 4))
              Image.AddQuad(toFill, Image.s_Xy, (Color32) this.color, Image.s_Uv);
          }
        }
        else
        {
          if (this.fillMethod != Image.FillMethod.Radial360)
            return;
          for (int index = 0; index < 4; ++index)
          {
            float t1;
            float t2;
            if (index < 2)
            {
              t1 = 0.0f;
              t2 = 0.5f;
            }
            else
            {
              t1 = 0.5f;
              t2 = 1f;
            }
            float t3;
            float t4;
            if (index == 0 || index == 3)
            {
              t3 = 0.0f;
              t4 = 0.5f;
            }
            else
            {
              t3 = 0.5f;
              t4 = 1f;
            }
            Image.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t1);
            Image.s_Xy[1].x = Image.s_Xy[0].x;
            Image.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t2);
            Image.s_Xy[3].x = Image.s_Xy[2].x;
            Image.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t3);
            Image.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t4);
            Image.s_Xy[2].y = Image.s_Xy[1].y;
            Image.s_Xy[3].y = Image.s_Xy[0].y;
            Image.s_Uv[0].x = Mathf.Lerp(num1, num3, t1);
            Image.s_Uv[1].x = Image.s_Uv[0].x;
            Image.s_Uv[2].x = Mathf.Lerp(num1, num3, t2);
            Image.s_Uv[3].x = Image.s_Uv[2].x;
            Image.s_Uv[0].y = Mathf.Lerp(num2, num4, t3);
            Image.s_Uv[1].y = Mathf.Lerp(num2, num4, t4);
            Image.s_Uv[2].y = Image.s_Uv[1].y;
            Image.s_Uv[3].y = Image.s_Uv[0].y;
            float num5 = !this.m_FillClockwise ? this.m_FillAmount * 4f - (float) (3 - (index + this.m_FillOrigin) % 4) : this.m_FillAmount * 4f - (float) ((index + this.m_FillOrigin) % 4);
            if (Image.RadialCut(Image.s_Xy, Image.s_Uv, Mathf.Clamp01(num5), this.m_FillClockwise, (index + 2) % 4))
              Image.AddQuad(toFill, Image.s_Xy, (Color32) this.color, Image.s_Uv);
          }
        }
      }
      else
        Image.AddQuad(toFill, Image.s_Xy, (Color32) this.color, Image.s_Uv);
    }

    private static bool RadialCut(Vector3[] xy, Vector3[] uv, float fill, bool invert, int corner)
    {
      if ((double) fill < 1.0 / 1000.0)
        return false;
      if ((corner & 1) == 1)
        invert = !invert;
      if (!invert && (double) fill > 0.999000012874603)
        return true;
      float num = Mathf.Clamp01(fill);
      if (invert)
        num = 1f - num;
      float f = num * 1.570796f;
      float cos = Mathf.Cos(f);
      float sin = Mathf.Sin(f);
      Image.RadialCut(xy, cos, sin, invert, corner);
      Image.RadialCut(uv, cos, sin, invert, corner);
      return true;
    }

    private static void RadialCut(Vector3[] xy, float cos, float sin, bool invert, int corner)
    {
      int index1 = corner;
      int index2 = (corner + 1) % 4;
      int index3 = (corner + 2) % 4;
      int index4 = (corner + 3) % 4;
      if ((corner & 1) == 1)
      {
        if ((double) sin > (double) cos)
        {
          cos /= sin;
          sin = 1f;
          if (invert)
          {
            xy[index2].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
            xy[index3].x = xy[index2].x;
          }
        }
        else if ((double) cos > (double) sin)
        {
          sin /= cos;
          cos = 1f;
          if (!invert)
          {
            xy[index3].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
            xy[index4].y = xy[index3].y;
          }
        }
        else
        {
          cos = 1f;
          sin = 1f;
        }
        if (!invert)
          xy[index4].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
        else
          xy[index2].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
      }
      else
      {
        if ((double) cos > (double) sin)
        {
          sin /= cos;
          cos = 1f;
          if (!invert)
          {
            xy[index2].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
            xy[index3].y = xy[index2].y;
          }
        }
        else if ((double) sin > (double) cos)
        {
          cos /= sin;
          sin = 1f;
          if (invert)
          {
            xy[index3].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
            xy[index4].x = xy[index3].x;
          }
        }
        else
        {
          cos = 1f;
          sin = 1f;
        }
        if (invert)
          xy[index4].y = Mathf.Lerp(xy[index1].y, xy[index3].y, sin);
        else
          xy[index2].x = Mathf.Lerp(xy[index1].x, xy[index3].x, cos);
      }
    }

    /// <summary>
    ///   <para>See ILayoutElement.CalculateLayoutInputHorizontal.</para>
    /// </summary>
    public virtual void CalculateLayoutInputHorizontal()
    {
    }

    /// <summary>
    ///   <para>See ILayoutElement.CalculateLayoutInputVertical.</para>
    /// </summary>
    public virtual void CalculateLayoutInputVertical()
    {
    }

    /// <summary>
    ///   <para>See:ICanvasRaycastFilter.</para>
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <param name="eventCamera"></param>
    public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
      if ((double) this.m_EventAlphaThreshold >= 1.0)
        return true;
      Sprite overrideSprite = this.overrideSprite;
      if ((UnityEngine.Object) overrideSprite == (UnityEngine.Object) null)
        return true;
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, screenPoint, eventCamera, out localPoint);
      Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
      localPoint.x += this.rectTransform.pivot.x * pixelAdjustedRect.width;
      localPoint.y += this.rectTransform.pivot.y * pixelAdjustedRect.height;
      localPoint = this.MapCoordinate(localPoint, pixelAdjustedRect);
      Rect textureRect = overrideSprite.textureRect;
      Vector2 vector2 = new Vector2(localPoint.x / textureRect.width, localPoint.y / textureRect.height);
      float u = Mathf.Lerp(textureRect.x, textureRect.xMax, vector2.x) / (float) overrideSprite.texture.width;
      float v = Mathf.Lerp(textureRect.y, textureRect.yMax, vector2.y) / (float) overrideSprite.texture.height;
      try
      {
        return (double) overrideSprite.texture.GetPixelBilinear(u, v).a >= (double) this.m_EventAlphaThreshold;
      }
      catch (UnityException ex)
      {
        Debug.LogError((object) ("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite."), (UnityEngine.Object) this);
        return true;
      }
    }

    private Vector2 MapCoordinate(Vector2 local, Rect rect)
    {
      Rect rect1 = this.sprite.rect;
      if (this.type == Image.Type.Simple || this.type == Image.Type.Filled)
        return new Vector2(local.x * rect1.width / rect.width, local.y * rect1.height / rect.height);
      Vector4 border = this.sprite.border;
      Vector4 adjustedBorders = this.GetAdjustedBorders(border / this.pixelsPerUnit, rect);
      for (int index1 = 0; index1 < 2; ++index1)
      {
        if ((double) local[index1] > (double) adjustedBorders[index1])
        {
          if ((double) rect.size[index1] - (double) local[index1] <= (double) adjustedBorders[index1 + 2])
          {
            // ISSUE: variable of a reference type
            Vector2& local1;
            int index2;
            // ISSUE: explicit reference operation
            // ISSUE: explicit reference operation
            double num = (double) (^(local1 = @local))[index2 = index1] - ((double) rect.size[index1] - (double) rect1.size[index1]);
            // ISSUE: explicit reference operation
            (^local1)[index2] = (float) num;
          }
          else if (this.type == Image.Type.Sliced)
          {
            float t = Mathf.InverseLerp(adjustedBorders[index1], rect.size[index1] - adjustedBorders[index1 + 2], local[index1]);
            local[index1] = Mathf.Lerp(border[index1], rect1.size[index1] - border[index1 + 2], t);
          }
          else
          {
            local[index1] -= adjustedBorders[index1];
            local[index1] = Mathf.Repeat(local[index1], rect1.size[index1] - border[index1] - border[index1 + 2]);
            local[index1] += border[index1];
          }
        }
      }
      return local;
    }

    /// <summary>
    ///   <para>Image Type.</para>
    /// </summary>
    public enum Type
    {
      Simple,
      Sliced,
      Tiled,
      Filled,
    }

    /// <summary>
    ///   <para>Fill method to be used by the Image.</para>
    /// </summary>
    public enum FillMethod
    {
      Horizontal,
      Vertical,
      Radial90,
      Radial180,
      Radial360,
    }

    /// <summary>
    ///   <para>Origin for the Image.FillMethod.Horizontal.</para>
    /// </summary>
    public enum OriginHorizontal
    {
      Left,
      Right,
    }

    /// <summary>
    ///   <para>Origin for the Image.FillMethod.Vertical.</para>
    /// </summary>
    public enum OriginVertical
    {
      Bottom,
      Top,
    }

    /// <summary>
    ///   <para>Origin for the Image.FillMethod.Radial90.</para>
    /// </summary>
    public enum Origin90
    {
      BottomLeft,
      TopLeft,
      TopRight,
      BottomRight,
    }

    /// <summary>
    ///   <para>Origin for the Image.FillMethod.Radial180.</para>
    /// </summary>
    public enum Origin180
    {
      Bottom,
      Left,
      Top,
      Right,
    }

    /// <summary>
    ///   <para>One of the points of the Arc for the Image.FillMethod.Radial360.</para>
    /// </summary>
    public enum Origin360
    {
      Bottom,
      Right,
      Top,
      Left,
    }
  }
}
