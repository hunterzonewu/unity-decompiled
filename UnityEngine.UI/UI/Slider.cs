// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Slider
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A standard slider that can be moved between a minimum and maximum value.</para>
  /// </summary>
  [AddComponentMenu("UI/Slider", 33)]
  [RequireComponent(typeof (RectTransform))]
  public class Slider : Selectable, IEventSystemHandler, IInitializePotentialDragHandler, IDragHandler, ICanvasElement
  {
    [SerializeField]
    private float m_MaxValue = 1f;
    [Space]
    [SerializeField]
    private Slider.SliderEvent m_OnValueChanged = new Slider.SliderEvent();
    private Vector2 m_Offset = Vector2.zero;
    [SerializeField]
    private RectTransform m_FillRect;
    [SerializeField]
    private RectTransform m_HandleRect;
    [SerializeField]
    [Space]
    private Slider.Direction m_Direction;
    [SerializeField]
    private float m_MinValue;
    [SerializeField]
    private bool m_WholeNumbers;
    [SerializeField]
    protected float m_Value;
    private Image m_FillImage;
    private Transform m_FillTransform;
    private RectTransform m_FillContainerRect;
    private Transform m_HandleTransform;
    private RectTransform m_HandleContainerRect;
    private DrivenRectTransformTracker m_Tracker;

    /// <summary>
    ///   <para>Optional RectTransform to use as fill for the slider.</para>
    /// </summary>
    public RectTransform fillRect
    {
      get
      {
        return this.m_FillRect;
      }
      set
      {
        if (!SetPropertyUtility.SetClass<RectTransform>(ref this.m_FillRect, value))
          return;
        this.UpdateCachedReferences();
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>Optional RectTransform to use as a handle for the slider.</para>
    /// </summary>
    public RectTransform handleRect
    {
      get
      {
        return this.m_HandleRect;
      }
      set
      {
        if (!SetPropertyUtility.SetClass<RectTransform>(ref this.m_HandleRect, value))
          return;
        this.UpdateCachedReferences();
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>The direction of the slider, from minimum to maximum value.</para>
    /// </summary>
    public Slider.Direction direction
    {
      get
      {
        return this.m_Direction;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<Slider.Direction>(ref this.m_Direction, value))
          return;
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>The minimum allowed value of the slider.</para>
    /// </summary>
    public float minValue
    {
      get
      {
        return this.m_MinValue;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_MinValue, value))
          return;
        this.Set(this.m_Value);
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>The maximum allowed value of the slider.</para>
    /// </summary>
    public float maxValue
    {
      get
      {
        return this.m_MaxValue;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_MaxValue, value))
          return;
        this.Set(this.m_Value);
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>Should the value only be allowed to be whole numbers?</para>
    /// </summary>
    public bool wholeNumbers
    {
      get
      {
        return this.m_WholeNumbers;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<bool>(ref this.m_WholeNumbers, value))
          return;
        this.Set(this.m_Value);
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>The current value of the slider.</para>
    /// </summary>
    public virtual float value
    {
      get
      {
        if (this.wholeNumbers)
          return Mathf.Round(this.m_Value);
        return this.m_Value;
      }
      set
      {
        this.Set(value);
      }
    }

    /// <summary>
    ///   <para>The current value of the slider normalized into a value between 0 and 1.</para>
    /// </summary>
    public float normalizedValue
    {
      get
      {
        if (Mathf.Approximately(this.minValue, this.maxValue))
          return 0.0f;
        return Mathf.InverseLerp(this.minValue, this.maxValue, this.value);
      }
      set
      {
        this.value = Mathf.Lerp(this.minValue, this.maxValue, value);
      }
    }

    /// <summary>
    ///   <para>Callback executed when the value of the slider is changed.</para>
    /// </summary>
    public Slider.SliderEvent onValueChanged
    {
      get
      {
        return this.m_OnValueChanged;
      }
      set
      {
        this.m_OnValueChanged = value;
      }
    }

    private float stepSize
    {
      get
      {
        if (this.wholeNumbers)
          return 1f;
        return (float) (((double) this.maxValue - (double) this.minValue) * 0.100000001490116);
      }
    }

    private Slider.Axis axis
    {
      get
      {
        return this.m_Direction == Slider.Direction.LeftToRight || this.m_Direction == Slider.Direction.RightToLeft ? Slider.Axis.Horizontal : Slider.Axis.Vertical;
      }
    }

    private bool reverseValue
    {
      get
      {
        if (this.m_Direction != Slider.Direction.RightToLeft)
          return this.m_Direction == Slider.Direction.TopToBottom;
        return true;
      }
    }

    protected Slider()
    {
    }

    protected override void OnValidate()
    {
      base.OnValidate();
      if (this.wholeNumbers)
      {
        this.m_MinValue = Mathf.Round(this.m_MinValue);
        this.m_MaxValue = Mathf.Round(this.m_MaxValue);
      }
      if (this.IsActive())
      {
        this.UpdateCachedReferences();
        this.Set(this.m_Value, false);
        this.UpdateVisuals();
      }
      if (PrefabUtility.GetPrefabType((UnityEngine.Object) this) == PrefabType.Prefab || Application.isPlaying)
        return;
      CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild((ICanvasElement) this);
    }

    /// <summary>
    ///   <para>Handling for when the canvas is rebuilt.</para>
    /// </summary>
    /// <param name="executing"></param>
    public virtual void Rebuild(CanvasUpdate executing)
    {
      if (executing != CanvasUpdate.Prelayout)
        return;
      this.onValueChanged.Invoke(this.value);
    }

    /// <summary>
    ///   <para>See ICanvasElement.LayoutComplete.</para>
    /// </summary>
    public virtual void LayoutComplete()
    {
    }

    /// <summary>
    ///   <para>See ICanvasElement.GraphicUpdateComplete.</para>
    /// </summary>
    public virtual void GraphicUpdateComplete()
    {
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.UpdateCachedReferences();
      this.Set(this.m_Value, false);
      this.UpdateVisuals();
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      this.m_Tracker.Clear();
      base.OnDisable();
    }

    protected override void OnDidApplyAnimationProperties()
    {
      this.m_Value = this.ClampValue(this.m_Value);
      float num = this.normalizedValue;
      if ((UnityEngine.Object) this.m_FillContainerRect != (UnityEngine.Object) null)
        num = !((UnityEngine.Object) this.m_FillImage != (UnityEngine.Object) null) || this.m_FillImage.type != Image.Type.Filled ? (!this.reverseValue ? this.m_FillRect.anchorMax[(int) this.axis] : 1f - this.m_FillRect.anchorMin[(int) this.axis]) : this.m_FillImage.fillAmount;
      else if ((UnityEngine.Object) this.m_HandleContainerRect != (UnityEngine.Object) null)
        num = !this.reverseValue ? this.m_HandleRect.anchorMin[(int) this.axis] : 1f - this.m_HandleRect.anchorMin[(int) this.axis];
      this.UpdateVisuals();
      if ((double) num == (double) this.normalizedValue)
        return;
      this.onValueChanged.Invoke(this.m_Value);
    }

    private void UpdateCachedReferences()
    {
      if ((bool) ((UnityEngine.Object) this.m_FillRect))
      {
        this.m_FillTransform = this.m_FillRect.transform;
        this.m_FillImage = this.m_FillRect.GetComponent<Image>();
        if ((UnityEngine.Object) this.m_FillTransform.parent != (UnityEngine.Object) null)
          this.m_FillContainerRect = this.m_FillTransform.parent.GetComponent<RectTransform>();
      }
      else
      {
        this.m_FillContainerRect = (RectTransform) null;
        this.m_FillImage = (Image) null;
      }
      if ((bool) ((UnityEngine.Object) this.m_HandleRect))
      {
        this.m_HandleTransform = this.m_HandleRect.transform;
        if (!((UnityEngine.Object) this.m_HandleTransform.parent != (UnityEngine.Object) null))
          return;
        this.m_HandleContainerRect = this.m_HandleTransform.parent.GetComponent<RectTransform>();
      }
      else
        this.m_HandleContainerRect = (RectTransform) null;
    }

    private float ClampValue(float input)
    {
      float f = Mathf.Clamp(input, this.minValue, this.maxValue);
      if (this.wholeNumbers)
        f = Mathf.Round(f);
      return f;
    }

    private void Set(float input)
    {
      this.Set(input, true);
    }

    /// <summary>
    ///   <para>Set the value of the slider.</para>
    /// </summary>
    /// <param name="input">The new value for the slider.</param>
    /// <param name="sendCallback">If the OnValueChanged callback should be invoked.</param>
    protected virtual void Set(float input, bool sendCallback)
    {
      float num = this.ClampValue(input);
      if ((double) this.m_Value == (double) num)
        return;
      this.m_Value = num;
      this.UpdateVisuals();
      if (!sendCallback)
        return;
      this.m_OnValueChanged.Invoke(num);
    }

    protected override void OnRectTransformDimensionsChange()
    {
      base.OnRectTransformDimensionsChange();
      if (!this.IsActive())
        return;
      this.UpdateVisuals();
    }

    private void UpdateVisuals()
    {
      if (!Application.isPlaying)
        this.UpdateCachedReferences();
      this.m_Tracker.Clear();
      if ((UnityEngine.Object) this.m_FillContainerRect != (UnityEngine.Object) null)
      {
        this.m_Tracker.Add((UnityEngine.Object) this, this.m_FillRect, DrivenTransformProperties.Anchors);
        Vector2 zero = Vector2.zero;
        Vector2 one = Vector2.one;
        if ((UnityEngine.Object) this.m_FillImage != (UnityEngine.Object) null && this.m_FillImage.type == Image.Type.Filled)
          this.m_FillImage.fillAmount = this.normalizedValue;
        else if (this.reverseValue)
          zero[(int) this.axis] = 1f - this.normalizedValue;
        else
          one[(int) this.axis] = this.normalizedValue;
        this.m_FillRect.anchorMin = zero;
        this.m_FillRect.anchorMax = one;
      }
      if (!((UnityEngine.Object) this.m_HandleContainerRect != (UnityEngine.Object) null))
        return;
      this.m_Tracker.Add((UnityEngine.Object) this, this.m_HandleRect, DrivenTransformProperties.Anchors);
      Vector2 zero1 = Vector2.zero;
      Vector2 one1 = Vector2.one;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Vector2& local = @zero1;
      int axis = (int) this.axis;
      float num1 = !this.reverseValue ? this.normalizedValue : 1f - this.normalizedValue;
      one1[(int) this.axis] = num1;
      double num2 = (double) num1;
      // ISSUE: explicit reference operation
      (^local)[axis] = (float) num2;
      this.m_HandleRect.anchorMin = zero1;
      this.m_HandleRect.anchorMax = one1;
    }

    private void UpdateDrag(PointerEventData eventData, Camera cam)
    {
      RectTransform rect = this.m_HandleContainerRect ?? this.m_FillContainerRect;
      Vector2 localPoint;
      if (!((UnityEngine.Object) rect != (UnityEngine.Object) null) || ((double) rect.rect.size[(int) this.axis] <= 0.0 || !RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, cam, out localPoint)))
        return;
      localPoint -= rect.rect.position;
      float num = Mathf.Clamp01((localPoint - this.m_Offset)[(int) this.axis] / rect.rect.size[(int) this.axis]);
      this.normalizedValue = !this.reverseValue ? num : 1f - num;
    }

    private bool MayDrag(PointerEventData eventData)
    {
      if (this.IsActive() && this.IsInteractable())
        return eventData.button == PointerEventData.InputButton.Left;
      return false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      base.OnPointerDown(eventData);
      this.m_Offset = Vector2.zero;
      if ((UnityEngine.Object) this.m_HandleContainerRect != (UnityEngine.Object) null && RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.position, eventData.enterEventCamera))
      {
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.position, eventData.pressEventCamera, out localPoint))
          return;
        this.m_Offset = localPoint;
      }
      else
        this.UpdateDrag(eventData, eventData.pressEventCamera);
    }

    /// <summary>
    ///   <para>Handling for when the slider is dragged.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      this.UpdateDrag(eventData, eventData.pressEventCamera);
    }

    /// <summary>
    ///   <para>Handling for movement events.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnMove(AxisEventData eventData)
    {
      if (!this.IsActive() || !this.IsInteractable())
      {
        base.OnMove(eventData);
      }
      else
      {
        switch (eventData.moveDir)
        {
          case MoveDirection.Left:
            if (this.axis == Slider.Axis.Horizontal && (UnityEngine.Object) this.FindSelectableOnLeft() == (UnityEngine.Object) null)
            {
              this.Set(!this.reverseValue ? this.value - this.stepSize : this.value + this.stepSize);
              break;
            }
            base.OnMove(eventData);
            break;
          case MoveDirection.Up:
            if (this.axis == Slider.Axis.Vertical && (UnityEngine.Object) this.FindSelectableOnUp() == (UnityEngine.Object) null)
            {
              this.Set(!this.reverseValue ? this.value + this.stepSize : this.value - this.stepSize);
              break;
            }
            base.OnMove(eventData);
            break;
          case MoveDirection.Right:
            if (this.axis == Slider.Axis.Horizontal && (UnityEngine.Object) this.FindSelectableOnRight() == (UnityEngine.Object) null)
            {
              this.Set(!this.reverseValue ? this.value + this.stepSize : this.value - this.stepSize);
              break;
            }
            base.OnMove(eventData);
            break;
          case MoveDirection.Down:
            if (this.axis == Slider.Axis.Vertical && (UnityEngine.Object) this.FindSelectableOnDown() == (UnityEngine.Object) null)
            {
              this.Set(!this.reverseValue ? this.value - this.stepSize : this.value + this.stepSize);
              break;
            }
            base.OnMove(eventData);
            break;
        }
      }
    }

    /// <summary>
    ///   <para>See member in base class.</para>
    /// </summary>
    public override Selectable FindSelectableOnLeft()
    {
      if (this.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Horizontal)
        return (Selectable) null;
      return base.FindSelectableOnLeft();
    }

    /// <summary>
    ///   <para>See member in base class.</para>
    /// </summary>
    public override Selectable FindSelectableOnRight()
    {
      if (this.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Horizontal)
        return (Selectable) null;
      return base.FindSelectableOnRight();
    }

    /// <summary>
    ///   <para>See member in base class.</para>
    /// </summary>
    public override Selectable FindSelectableOnUp()
    {
      if (this.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Vertical)
        return (Selectable) null;
      return base.FindSelectableOnUp();
    }

    /// <summary>
    ///   <para>See member in base class.</para>
    /// </summary>
    public override Selectable FindSelectableOnDown()
    {
      if (this.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Vertical)
        return (Selectable) null;
      return base.FindSelectableOnDown();
    }

    /// <summary>
    ///   <para>See: IInitializePotentialDragHandler.OnInitializePotentialDrag.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnInitializePotentialDrag(PointerEventData eventData)
    {
      eventData.useDragThreshold = false;
    }

    public void SetDirection(Slider.Direction direction, bool includeRectLayouts)
    {
      Slider.Axis axis = this.axis;
      bool reverseValue = this.reverseValue;
      this.direction = direction;
      if (!includeRectLayouts)
        return;
      if (this.axis != axis)
        RectTransformUtility.FlipLayoutAxes(this.transform as RectTransform, true, true);
      if (this.reverseValue == reverseValue)
        return;
      RectTransformUtility.FlipLayoutOnAxis(this.transform as RectTransform, (int) this.axis, true, true);
    }

    bool ICanvasElement.IsDestroyed()
    {
      return this.IsDestroyed();
    }

    Transform ICanvasElement.get_transform()
    {
      return this.transform;
    }

    /// <summary>
    ///   <para>Setting that indicates one of four directions.</para>
    /// </summary>
    public enum Direction
    {
      LeftToRight,
      RightToLeft,
      BottomToTop,
      TopToBottom,
    }

    /// <summary>
    ///   <para>Event type used by the Slider.</para>
    /// </summary>
    [Serializable]
    public class SliderEvent : UnityEvent<float>
    {
    }

    private enum Axis
    {
      Horizontal,
      Vertical,
    }
  }
}
