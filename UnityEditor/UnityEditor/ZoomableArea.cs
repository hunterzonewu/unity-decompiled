// Decompiled with JetBrains decompiler
// Type: UnityEditor.ZoomableArea
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class ZoomableArea
  {
    private static Vector2 m_MouseDownPosition = new Vector2(-1000000f, -1000000f);
    private static int zoomableAreaHash = "ZoomableArea".GetHashCode();
    [SerializeField]
    private float m_HBaseRangeMax = 1f;
    [SerializeField]
    private float m_VBaseRangeMax = 1f;
    [SerializeField]
    private bool m_HAllowExceedBaseRangeMin = true;
    [SerializeField]
    private bool m_HAllowExceedBaseRangeMax = true;
    [SerializeField]
    private bool m_VAllowExceedBaseRangeMin = true;
    [SerializeField]
    private bool m_VAllowExceedBaseRangeMax = true;
    private float m_HScaleMin = 1f / 1000f;
    private float m_HScaleMax = 100000f;
    private float m_VScaleMin = 1f / 1000f;
    private float m_VScaleMax = 100000f;
    [SerializeField]
    private bool m_HSlider = true;
    [SerializeField]
    private bool m_VSlider = true;
    [SerializeField]
    private Rect m_DrawArea = new Rect(0.0f, 0.0f, 100f, 100f);
    [SerializeField]
    internal Vector2 m_Scale = new Vector2(1f, -1f);
    [SerializeField]
    internal Vector2 m_Translation = new Vector2(0.0f, 0.0f);
    [SerializeField]
    private Rect m_LastShownAreaInsideMargins = new Rect(0.0f, 0.0f, 100f, 100f);
    [SerializeField]
    private bool m_HRangeLocked;
    [SerializeField]
    private bool m_VRangeLocked;
    [SerializeField]
    private float m_HBaseRangeMin;
    [SerializeField]
    private float m_VBaseRangeMin;
    [SerializeField]
    private bool m_ScaleWithWindow;
    [SerializeField]
    private bool m_IgnoreScrollWheelUntilClicked;
    public bool m_UniformScale;
    [SerializeField]
    private float m_MarginLeft;
    [SerializeField]
    private float m_MarginRight;
    [SerializeField]
    private float m_MarginTop;
    [SerializeField]
    private float m_MarginBottom;
    private int verticalScrollbarID;
    private int horizontalScrollbarID;
    [SerializeField]
    private bool m_MinimalGUI;
    private ZoomableArea.Styles m_Styles;

    public bool hRangeLocked
    {
      get
      {
        return this.m_HRangeLocked;
      }
      set
      {
        this.m_HRangeLocked = value;
      }
    }

    public bool vRangeLocked
    {
      get
      {
        return this.m_VRangeLocked;
      }
      set
      {
        this.m_VRangeLocked = value;
      }
    }

    public float hBaseRangeMin
    {
      get
      {
        return this.m_HBaseRangeMin;
      }
      set
      {
        this.m_HBaseRangeMin = value;
      }
    }

    public float hBaseRangeMax
    {
      get
      {
        return this.m_HBaseRangeMax;
      }
      set
      {
        this.m_HBaseRangeMax = value;
      }
    }

    public float vBaseRangeMin
    {
      get
      {
        return this.m_VBaseRangeMin;
      }
      set
      {
        this.m_VBaseRangeMin = value;
      }
    }

    public float vBaseRangeMax
    {
      get
      {
        return this.m_VBaseRangeMax;
      }
      set
      {
        this.m_VBaseRangeMax = value;
      }
    }

    public bool hAllowExceedBaseRangeMin
    {
      get
      {
        return this.m_HAllowExceedBaseRangeMin;
      }
      set
      {
        this.m_HAllowExceedBaseRangeMin = value;
      }
    }

    public bool hAllowExceedBaseRangeMax
    {
      get
      {
        return this.m_HAllowExceedBaseRangeMax;
      }
      set
      {
        this.m_HAllowExceedBaseRangeMax = value;
      }
    }

    public bool vAllowExceedBaseRangeMin
    {
      get
      {
        return this.m_VAllowExceedBaseRangeMin;
      }
      set
      {
        this.m_VAllowExceedBaseRangeMin = value;
      }
    }

    public bool vAllowExceedBaseRangeMax
    {
      get
      {
        return this.m_VAllowExceedBaseRangeMax;
      }
      set
      {
        this.m_VAllowExceedBaseRangeMax = value;
      }
    }

    public float hRangeMin
    {
      get
      {
        if (this.hAllowExceedBaseRangeMin)
          return float.NegativeInfinity;
        return this.hBaseRangeMin;
      }
      set
      {
        this.SetAllowExceed(ref this.m_HBaseRangeMin, ref this.m_HAllowExceedBaseRangeMin, value);
      }
    }

    public float hRangeMax
    {
      get
      {
        if (this.hAllowExceedBaseRangeMax)
          return float.PositiveInfinity;
        return this.hBaseRangeMax;
      }
      set
      {
        this.SetAllowExceed(ref this.m_HBaseRangeMax, ref this.m_HAllowExceedBaseRangeMax, value);
      }
    }

    public float vRangeMin
    {
      get
      {
        if (this.vAllowExceedBaseRangeMin)
          return float.NegativeInfinity;
        return this.vBaseRangeMin;
      }
      set
      {
        this.SetAllowExceed(ref this.m_VBaseRangeMin, ref this.m_VAllowExceedBaseRangeMin, value);
      }
    }

    public float vRangeMax
    {
      get
      {
        if (this.vAllowExceedBaseRangeMax)
          return float.PositiveInfinity;
        return this.vBaseRangeMax;
      }
      set
      {
        this.SetAllowExceed(ref this.m_VBaseRangeMax, ref this.m_VAllowExceedBaseRangeMax, value);
      }
    }

    public bool scaleWithWindow
    {
      get
      {
        return this.m_ScaleWithWindow;
      }
      set
      {
        this.m_ScaleWithWindow = value;
      }
    }

    public bool hSlider
    {
      get
      {
        return this.m_HSlider;
      }
      set
      {
        Rect rect = this.rect;
        this.m_HSlider = value;
        this.rect = rect;
      }
    }

    public bool vSlider
    {
      get
      {
        return this.m_VSlider;
      }
      set
      {
        Rect rect = this.rect;
        this.m_VSlider = value;
        this.rect = rect;
      }
    }

    public bool ignoreScrollWheelUntilClicked
    {
      get
      {
        return this.m_IgnoreScrollWheelUntilClicked;
      }
      set
      {
        this.m_IgnoreScrollWheelUntilClicked = value;
      }
    }

    public bool uniformScale
    {
      get
      {
        return this.m_UniformScale;
      }
      set
      {
        this.m_UniformScale = value;
      }
    }

    public Vector2 scale
    {
      get
      {
        return this.m_Scale;
      }
    }

    public Vector2 translation
    {
      get
      {
        return this.m_Translation;
      }
    }

    public float margin
    {
      set
      {
        this.m_MarginLeft = this.m_MarginRight = this.m_MarginTop = this.m_MarginBottom = value;
      }
    }

    public float leftmargin
    {
      get
      {
        return this.m_MarginLeft;
      }
      set
      {
        this.m_MarginLeft = value;
      }
    }

    public float rightmargin
    {
      get
      {
        return this.m_MarginRight;
      }
      set
      {
        this.m_MarginRight = value;
      }
    }

    public float topmargin
    {
      get
      {
        return this.m_MarginTop;
      }
      set
      {
        this.m_MarginTop = value;
      }
    }

    public float bottommargin
    {
      get
      {
        return this.m_MarginBottom;
      }
      set
      {
        this.m_MarginBottom = value;
      }
    }

    private ZoomableArea.Styles styles
    {
      get
      {
        if (this.m_Styles == null)
          this.m_Styles = new ZoomableArea.Styles(this.m_MinimalGUI);
        return this.m_Styles;
      }
    }

    public Rect rect
    {
      get
      {
        return new Rect(this.drawRect.x, this.drawRect.y, this.drawRect.width + (!this.m_VSlider ? 0.0f : this.styles.visualSliderWidth), this.drawRect.height + (!this.m_HSlider ? 0.0f : this.styles.visualSliderWidth));
      }
      set
      {
        Rect rect = new Rect(value.x, value.y, value.width - (!this.m_VSlider ? 0.0f : this.styles.visualSliderWidth), value.height - (!this.m_HSlider ? 0.0f : this.styles.visualSliderWidth));
        if (rect != this.m_DrawArea)
        {
          if (this.m_ScaleWithWindow)
          {
            this.m_DrawArea = rect;
            this.shownAreaInsideMargins = this.m_LastShownAreaInsideMargins;
          }
          else
          {
            this.m_Translation += new Vector2((float) (((double) rect.width - (double) this.m_DrawArea.width) / 2.0), (float) (((double) rect.height - (double) this.m_DrawArea.height) / 2.0));
            this.m_DrawArea = rect;
          }
        }
        this.EnforceScaleAndRange();
      }
    }

    public Rect drawRect
    {
      get
      {
        return this.m_DrawArea;
      }
    }

    public Rect shownArea
    {
      get
      {
        return new Rect(-this.m_Translation.x / this.m_Scale.x, (float) -((double) this.m_Translation.y - (double) this.drawRect.height) / this.m_Scale.y, this.drawRect.width / this.m_Scale.x, this.drawRect.height / -this.m_Scale.y);
      }
      set
      {
        this.m_Scale.x = this.drawRect.width / value.width;
        this.m_Scale.y = -this.drawRect.height / value.height;
        this.m_Translation.x = -value.x * this.m_Scale.x;
        this.m_Translation.y = this.drawRect.height - value.y * this.m_Scale.y;
        this.EnforceScaleAndRange();
      }
    }

    public Rect shownAreaInsideMargins
    {
      get
      {
        return this.shownAreaInsideMarginsInternal;
      }
      set
      {
        this.shownAreaInsideMarginsInternal = value;
        this.EnforceScaleAndRange();
      }
    }

    private Rect shownAreaInsideMarginsInternal
    {
      get
      {
        float num1 = this.leftmargin / this.m_Scale.x;
        float num2 = this.rightmargin / this.m_Scale.x;
        float num3 = this.topmargin / this.m_Scale.y;
        float num4 = this.bottommargin / this.m_Scale.y;
        Rect shownArea = this.shownArea;
        shownArea.x += num1;
        shownArea.y -= num3;
        shownArea.width -= num1 + num2;
        shownArea.height += num3 + num4;
        return shownArea;
      }
      set
      {
        this.m_Scale.x = (this.drawRect.width - this.leftmargin - this.rightmargin) / value.width;
        this.m_Scale.y = (float) -((double) this.drawRect.height - (double) this.topmargin - (double) this.bottommargin) / value.height;
        this.m_Translation.x = -value.x * this.m_Scale.x + this.leftmargin;
        this.m_Translation.y = this.drawRect.height - value.y * this.m_Scale.y - this.topmargin;
      }
    }

    public virtual Bounds drawingBounds
    {
      get
      {
        return new Bounds(new Vector3((float) (((double) this.hBaseRangeMin + (double) this.hBaseRangeMax) * 0.5), (float) (((double) this.vBaseRangeMin + (double) this.vBaseRangeMax) * 0.5), 0.0f), new Vector3(this.hBaseRangeMax - this.hBaseRangeMin, this.vBaseRangeMax - this.vBaseRangeMin, 1f));
      }
    }

    public Matrix4x4 drawingToViewMatrix
    {
      get
      {
        return Matrix4x4.TRS((Vector3) this.m_Translation, Quaternion.identity, new Vector3(this.m_Scale.x, this.m_Scale.y, 1f));
      }
    }

    public Vector2 mousePositionInDrawing
    {
      get
      {
        return this.ViewToDrawingTransformPoint(Event.current.mousePosition);
      }
    }

    public ZoomableArea()
    {
      this.m_MinimalGUI = false;
    }

    public ZoomableArea(bool minimalGUI)
    {
      this.m_MinimalGUI = minimalGUI;
    }

    private void SetAllowExceed(ref float rangeEnd, ref bool allowExceed, float value)
    {
      if ((double) value == double.NegativeInfinity || (double) value == double.PositiveInfinity)
      {
        rangeEnd = (double) value != double.NegativeInfinity ? 1f : 0.0f;
        allowExceed = true;
      }
      else
      {
        rangeEnd = value;
        allowExceed = false;
      }
    }

    internal void SetDrawRectHack(Rect r)
    {
      this.m_DrawArea = r;
    }

    public void SetShownHRangeInsideMargins(float min, float max)
    {
      this.m_Scale.x = (float) (((double) this.drawRect.width - (double) this.leftmargin - (double) this.rightmargin) / ((double) max - (double) min));
      this.m_Translation.x = -min * this.m_Scale.x + this.leftmargin;
      this.EnforceScaleAndRange();
    }

    public void SetShownHRange(float min, float max)
    {
      this.m_Scale.x = this.drawRect.width / (max - min);
      this.m_Translation.x = -min * this.m_Scale.x;
      this.EnforceScaleAndRange();
    }

    public void SetShownVRangeInsideMargins(float min, float max)
    {
      this.m_Scale.y = (float) (-((double) this.drawRect.height - (double) this.topmargin - (double) this.bottommargin) / ((double) max - (double) min));
      this.m_Translation.y = this.drawRect.height - min * this.m_Scale.y - this.topmargin;
      this.EnforceScaleAndRange();
    }

    public void SetShownVRange(float min, float max)
    {
      this.m_Scale.y = (float) (-(double) this.drawRect.height / ((double) max - (double) min));
      this.m_Translation.y = this.drawRect.height - min * this.m_Scale.y;
      this.EnforceScaleAndRange();
    }

    public Vector2 DrawingToViewTransformPoint(Vector2 lhs)
    {
      return new Vector2(lhs.x * this.m_Scale.x + this.m_Translation.x, lhs.y * this.m_Scale.y + this.m_Translation.y);
    }

    public Vector3 DrawingToViewTransformPoint(Vector3 lhs)
    {
      return new Vector3(lhs.x * this.m_Scale.x + this.m_Translation.x, lhs.y * this.m_Scale.y + this.m_Translation.y, 0.0f);
    }

    public Vector2 ViewToDrawingTransformPoint(Vector2 lhs)
    {
      return new Vector2((lhs.x - this.m_Translation.x) / this.m_Scale.x, (lhs.y - this.m_Translation.y) / this.m_Scale.y);
    }

    public Vector3 ViewToDrawingTransformPoint(Vector3 lhs)
    {
      return new Vector3((lhs.x - this.m_Translation.x) / this.m_Scale.x, (lhs.y - this.m_Translation.y) / this.m_Scale.y, 0.0f);
    }

    public Vector2 DrawingToViewTransformVector(Vector2 lhs)
    {
      return new Vector2(lhs.x * this.m_Scale.x, lhs.y * this.m_Scale.y);
    }

    public Vector3 DrawingToViewTransformVector(Vector3 lhs)
    {
      return new Vector3(lhs.x * this.m_Scale.x, lhs.y * this.m_Scale.y, 0.0f);
    }

    public Vector2 ViewToDrawingTransformVector(Vector2 lhs)
    {
      return new Vector2(lhs.x / this.m_Scale.x, lhs.y / this.m_Scale.y);
    }

    public Vector3 ViewToDrawingTransformVector(Vector3 lhs)
    {
      return new Vector3(lhs.x / this.m_Scale.x, lhs.y / this.m_Scale.y, 0.0f);
    }

    public Vector2 NormalizeInViewSpace(Vector2 vec)
    {
      vec = Vector2.Scale(vec, this.m_Scale);
      vec /= vec.magnitude;
      return Vector2.Scale(vec, new Vector2(1f / this.m_Scale.x, 1f / this.m_Scale.y));
    }

    private bool IsZoomEvent()
    {
      if (Event.current.button == 1)
        return Event.current.alt;
      return false;
    }

    private bool IsPanEvent()
    {
      if (Event.current.button == 0 && Event.current.alt)
        return true;
      if (Event.current.button == 2)
        return !Event.current.command;
      return false;
    }

    public void BeginViewGUI()
    {
      if (this.styles.horizontalScrollbar == null)
        this.styles.InitGUIStyles(this.m_MinimalGUI);
      this.HandleZoomAndPanEvents(this.m_DrawArea);
      this.horizontalScrollbarID = GUIUtility.GetControlID(EditorGUIExt.s_MinMaxSliderHash, FocusType.Passive);
      this.verticalScrollbarID = GUIUtility.GetControlID(EditorGUIExt.s_MinMaxSliderHash, FocusType.Passive);
      if (this.m_MinimalGUI && Event.current.type == EventType.Repaint)
        return;
      this.SliderGUI();
    }

    public void HandleZoomAndPanEvents(Rect area)
    {
      GUILayout.BeginArea(area);
      area.x = 0.0f;
      area.y = 0.0f;
      int controlId = GUIUtility.GetControlID(ZoomableArea.zoomableAreaHash, FocusType.Native, area);
      switch (Event.current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (area.Contains(Event.current.mousePosition))
          {
            GUIUtility.keyboardControl = controlId;
            if (this.IsZoomEvent() || this.IsPanEvent())
            {
              GUIUtility.hotControl = controlId;
              ZoomableArea.m_MouseDownPosition = this.mousePositionInDrawing;
              Event.current.Use();
              break;
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            ZoomableArea.m_MouseDownPosition = new Vector2(-1000000f, -1000000f);
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            if (this.IsZoomEvent())
            {
              this.Zoom(ZoomableArea.m_MouseDownPosition, false);
              Event.current.Use();
              break;
            }
            if (this.IsPanEvent())
            {
              this.Pan();
              Event.current.Use();
              break;
            }
            break;
          }
          break;
        case EventType.ScrollWheel:
          if (area.Contains(Event.current.mousePosition) && (!this.m_IgnoreScrollWheelUntilClicked || GUIUtility.keyboardControl == controlId))
          {
            this.Zoom(this.mousePositionInDrawing, true);
            Event.current.Use();
            break;
          }
          break;
      }
      GUILayout.EndArea();
    }

    public void EndViewGUI()
    {
      if (!this.m_MinimalGUI || Event.current.type != EventType.Repaint)
        return;
      this.SliderGUI();
    }

    private void SliderGUI()
    {
      if (!this.m_HSlider && !this.m_VSlider)
        return;
      Bounds drawingBounds = this.drawingBounds;
      Rect areaInsideMargins = this.shownAreaInsideMargins;
      float num1 = this.styles.sliderWidth - this.styles.visualSliderWidth;
      float num2 = !this.vSlider || !this.hSlider ? 0.0f : num1;
      Vector2 scale = this.m_Scale;
      if (this.m_HSlider)
      {
        Rect position = new Rect(this.drawRect.x + 1f, this.drawRect.yMax - num1, this.drawRect.width - num2, this.styles.sliderWidth);
        float width = areaInsideMargins.width;
        float xMin = areaInsideMargins.xMin;
        EditorGUIExt.MinMaxScroller(position, this.horizontalScrollbarID, ref xMin, ref width, drawingBounds.min.x, drawingBounds.max.x, float.NegativeInfinity, float.PositiveInfinity, this.styles.horizontalScrollbar, this.styles.horizontalMinMaxScrollbarThumb, this.styles.horizontalScrollbarLeftButton, this.styles.horizontalScrollbarRightButton, true);
        float num3 = xMin;
        float num4 = xMin + width;
        if ((double) num3 > (double) areaInsideMargins.xMin)
          num3 = Mathf.Min(num3, num4 - this.m_HScaleMin);
        if ((double) num4 < (double) areaInsideMargins.xMax)
          num4 = Mathf.Max(num4, num3 + this.m_HScaleMin);
        this.SetShownHRangeInsideMargins(num3, num4);
      }
      if (this.m_VSlider)
      {
        Rect position = new Rect(this.drawRect.xMax - num1, this.drawRect.y, this.styles.sliderWidth, this.drawRect.height - num2);
        float height = areaInsideMargins.height;
        float num3 = -areaInsideMargins.yMax;
        EditorGUIExt.MinMaxScroller(position, this.verticalScrollbarID, ref num3, ref height, -drawingBounds.max.y, -drawingBounds.min.y, float.NegativeInfinity, float.PositiveInfinity, this.styles.verticalScrollbar, this.styles.verticalMinMaxScrollbarThumb, this.styles.verticalScrollbarUpButton, this.styles.verticalScrollbarDownButton, false);
        float num4 = (float) -((double) num3 + (double) height);
        float num5 = -num3;
        if ((double) num4 > (double) areaInsideMargins.yMin)
          num4 = Mathf.Min(num4, num5 - this.m_VScaleMin);
        if ((double) num5 < (double) areaInsideMargins.yMax)
          num5 = Mathf.Max(num5, num4 + this.m_VScaleMin);
        this.SetShownVRangeInsideMargins(num4, num5);
      }
      if (!this.uniformScale)
        return;
      float num6 = this.drawRect.width / this.drawRect.height;
      Vector2 vector2 = scale - this.m_Scale;
      this.m_Scale -= new Vector2(-vector2.y * num6, -vector2.x / num6);
      this.m_Translation.x -= vector2.y / 2f;
      this.m_Translation.y -= vector2.x / 2f;
      this.EnforceScaleAndRange();
    }

    private void Pan()
    {
      if (!this.m_HRangeLocked)
        this.m_Translation.x += Event.current.delta.x;
      if (!this.m_VRangeLocked)
        this.m_Translation.y += Event.current.delta.y;
      this.EnforceScaleAndRange();
    }

    private void Zoom(Vector2 zoomAround, bool scrollwhell)
    {
      float num1 = Event.current.delta.x + Event.current.delta.y;
      if (scrollwhell)
        num1 = -num1;
      float num2 = Mathf.Max(0.01f, (float) (1.0 + (double) num1 * 0.00999999977648258));
      if (!this.m_HRangeLocked && !Event.current.shift)
      {
        this.m_Translation.x -= zoomAround.x * (num2 - 1f) * this.m_Scale.x;
        this.m_Scale.x *= num2;
      }
      if (!this.m_VRangeLocked && !EditorGUI.actionKey)
      {
        this.m_Translation.y -= zoomAround.y * (num2 - 1f) * this.m_Scale.y;
        this.m_Scale.y *= num2;
      }
      this.EnforceScaleAndRange();
    }

    public void EnforceScaleAndRange()
    {
      float hscaleMin = this.m_HScaleMin;
      float vscaleMin = this.m_VScaleMin;
      float num1 = this.m_HScaleMax;
      float num2 = this.m_VScaleMax;
      if ((double) this.hRangeMax != double.PositiveInfinity && (double) this.hRangeMin != double.NegativeInfinity)
        num1 = Mathf.Min(this.m_HScaleMax, this.hRangeMax - this.hRangeMin);
      if ((double) this.vRangeMax != double.PositiveInfinity && (double) this.vRangeMin != double.NegativeInfinity)
        num2 = Mathf.Min(this.m_VScaleMax, this.vRangeMax - this.vRangeMin);
      Rect areaInsideMargins = this.m_LastShownAreaInsideMargins;
      Rect rect = this.shownAreaInsideMargins;
      if (rect == areaInsideMargins)
        return;
      float num3 = 1E-05f;
      if ((double) rect.width < (double) areaInsideMargins.width - (double) num3)
      {
        float t = Mathf.InverseLerp(areaInsideMargins.width, rect.width, hscaleMin);
        rect = new Rect(Mathf.Lerp(areaInsideMargins.x, rect.x, t), rect.y, Mathf.Lerp(areaInsideMargins.width, rect.width, t), rect.height);
      }
      if ((double) rect.height < (double) areaInsideMargins.height - (double) num3)
      {
        float t = Mathf.InverseLerp(areaInsideMargins.height, rect.height, vscaleMin);
        rect = new Rect(rect.x, Mathf.Lerp(areaInsideMargins.y, rect.y, t), rect.width, Mathf.Lerp(areaInsideMargins.height, rect.height, t));
      }
      if ((double) rect.width > (double) areaInsideMargins.width + (double) num3)
      {
        float t = Mathf.InverseLerp(areaInsideMargins.width, rect.width, num1);
        rect = new Rect(Mathf.Lerp(areaInsideMargins.x, rect.x, t), rect.y, Mathf.Lerp(areaInsideMargins.width, rect.width, t), rect.height);
      }
      if ((double) rect.height > (double) areaInsideMargins.height + (double) num3)
      {
        float t = Mathf.InverseLerp(areaInsideMargins.height, rect.height, num2);
        rect = new Rect(rect.x, Mathf.Lerp(areaInsideMargins.y, rect.y, t), rect.width, Mathf.Lerp(areaInsideMargins.height, rect.height, t));
      }
      if ((double) rect.xMin < (double) this.hRangeMin)
        rect.x = this.hRangeMin;
      if ((double) rect.xMax > (double) this.hRangeMax)
        rect.x = this.hRangeMax - rect.width;
      if ((double) rect.yMin < (double) this.vRangeMin)
        rect.y = this.vRangeMin;
      if ((double) rect.yMax > (double) this.vRangeMax)
        rect.y = this.vRangeMax - rect.height;
      this.shownAreaInsideMarginsInternal = rect;
      this.m_LastShownAreaInsideMargins = rect;
    }

    public float PixelToTime(float pixelX, Rect rect)
    {
      return (pixelX - rect.x) * this.shownArea.width / rect.width + this.shownArea.x;
    }

    public float TimeToPixel(float time, Rect rect)
    {
      return (time - this.shownArea.x) / this.shownArea.width * rect.width + rect.x;
    }

    public float PixelDeltaToTime(Rect rect)
    {
      return this.shownArea.width / rect.width;
    }

    [Serializable]
    public class Styles
    {
      public GUIStyle horizontalScrollbar;
      public GUIStyle horizontalMinMaxScrollbarThumb;
      public GUIStyle horizontalScrollbarLeftButton;
      public GUIStyle horizontalScrollbarRightButton;
      public GUIStyle verticalScrollbar;
      public GUIStyle verticalMinMaxScrollbarThumb;
      public GUIStyle verticalScrollbarUpButton;
      public GUIStyle verticalScrollbarDownButton;
      public float sliderWidth;
      public float visualSliderWidth;

      public Styles(bool minimalGUI)
      {
        if (minimalGUI)
        {
          this.visualSliderWidth = 0.0f;
          this.sliderWidth = 15f;
        }
        else
        {
          this.visualSliderWidth = 15f;
          this.sliderWidth = 15f;
        }
      }

      public void InitGUIStyles(bool minimalGUI)
      {
        if (minimalGUI)
        {
          this.horizontalMinMaxScrollbarThumb = (GUIStyle) "MiniMinMaxSliderHorizontal";
          this.horizontalScrollbarLeftButton = GUIStyle.none;
          this.horizontalScrollbarRightButton = GUIStyle.none;
          this.horizontalScrollbar = GUIStyle.none;
          this.verticalMinMaxScrollbarThumb = (GUIStyle) "MiniMinMaxSlidervertical";
          this.verticalScrollbarUpButton = GUIStyle.none;
          this.verticalScrollbarDownButton = GUIStyle.none;
          this.verticalScrollbar = GUIStyle.none;
        }
        else
        {
          this.horizontalMinMaxScrollbarThumb = (GUIStyle) "horizontalMinMaxScrollbarThumb";
          this.horizontalScrollbarLeftButton = (GUIStyle) "horizontalScrollbarLeftbutton";
          this.horizontalScrollbarRightButton = (GUIStyle) "horizontalScrollbarRightbutton";
          this.horizontalScrollbar = GUI.skin.horizontalScrollbar;
          this.verticalMinMaxScrollbarThumb = (GUIStyle) "verticalMinMaxScrollbarThumb";
          this.verticalScrollbarUpButton = (GUIStyle) "verticalScrollbarUpbutton";
          this.verticalScrollbarDownButton = (GUIStyle) "verticalScrollbarDownbutton";
          this.verticalScrollbar = GUI.skin.verticalScrollbar;
        }
      }
    }
  }
}
