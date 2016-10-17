// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Scrollbar
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A standard scrollbar with a variable sized handle that can be dragged between 0 and 1.</para>
  /// </summary>
  [RequireComponent(typeof (RectTransform))]
  [AddComponentMenu("UI/Scrollbar", 34)]
  public class Scrollbar : Selectable, IEventSystemHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, ICanvasElement
  {
    [SerializeField]
    [Range(0.0f, 1f)]
    private float m_Size = 0.2f;
    [Space(6f)]
    [SerializeField]
    private Scrollbar.ScrollEvent m_OnValueChanged = new Scrollbar.ScrollEvent();
    private Vector2 m_Offset = Vector2.zero;
    [SerializeField]
    private RectTransform m_HandleRect;
    [SerializeField]
    private Scrollbar.Direction m_Direction;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float m_Value;
    [SerializeField]
    [Range(0.0f, 11f)]
    private int m_NumberOfSteps;
    private RectTransform m_ContainerRect;
    private DrivenRectTransformTracker m_Tracker;
    private Coroutine m_PointerDownRepeat;
    private bool isPointerDownAndNotDragging;

    /// <summary>
    ///   <para>The RectTransform to use for the handle.</para>
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
    ///   <para>The direction of the scrollbar from minimum to maximum value.</para>
    /// </summary>
    public Scrollbar.Direction direction
    {
      get
      {
        return this.m_Direction;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<Scrollbar.Direction>(ref this.m_Direction, value))
          return;
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>The current value of the scrollbar, between 0 and 1.</para>
    /// </summary>
    public float value
    {
      get
      {
        float num = this.m_Value;
        if (this.m_NumberOfSteps > 1)
          num = Mathf.Round(num * (float) (this.m_NumberOfSteps - 1)) / (float) (this.m_NumberOfSteps - 1);
        return num;
      }
      set
      {
        this.Set(value);
      }
    }

    /// <summary>
    ///   <para>The size of the scrollbar handle where 1 means it fills the entire scrollbar.</para>
    /// </summary>
    public float size
    {
      get
      {
        return this.m_Size;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<float>(ref this.m_Size, Mathf.Clamp01(value)))
          return;
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>The number of steps to use for the value. A value of 0 disables use of steps.</para>
    /// </summary>
    public int numberOfSteps
    {
      get
      {
        return this.m_NumberOfSteps;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<int>(ref this.m_NumberOfSteps, value))
          return;
        this.Set(this.m_Value);
        this.UpdateVisuals();
      }
    }

    /// <summary>
    ///   <para>Handling for when the scrollbar value is changed.</para>
    /// </summary>
    public Scrollbar.ScrollEvent onValueChanged
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
        if (this.m_NumberOfSteps > 1)
          return 1f / (float) (this.m_NumberOfSteps - 1);
        return 0.1f;
      }
    }

    private Scrollbar.Axis axis
    {
      get
      {
        return this.m_Direction == Scrollbar.Direction.LeftToRight || this.m_Direction == Scrollbar.Direction.RightToLeft ? Scrollbar.Axis.Horizontal : Scrollbar.Axis.Vertical;
      }
    }

    private bool reverseValue
    {
      get
      {
        if (this.m_Direction != Scrollbar.Direction.RightToLeft)
          return this.m_Direction == Scrollbar.Direction.TopToBottom;
        return true;
      }
    }

    protected Scrollbar()
    {
    }

    protected override void OnValidate()
    {
      base.OnValidate();
      this.m_Size = Mathf.Clamp01(this.m_Size);
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

    private void UpdateCachedReferences()
    {
      if ((bool) ((UnityEngine.Object) this.m_HandleRect) && (UnityEngine.Object) this.m_HandleRect.parent != (UnityEngine.Object) null)
        this.m_ContainerRect = this.m_HandleRect.parent.GetComponent<RectTransform>();
      else
        this.m_ContainerRect = (RectTransform) null;
    }

    private void Set(float input)
    {
      this.Set(input, true);
    }

    private void Set(float input, bool sendCallback)
    {
      float num = this.m_Value;
      this.m_Value = Mathf.Clamp01(input);
      if ((double) num == (double) this.value)
        return;
      this.UpdateVisuals();
      if (!sendCallback)
        return;
      this.m_OnValueChanged.Invoke(this.value);
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
      if (!((UnityEngine.Object) this.m_ContainerRect != (UnityEngine.Object) null))
        return;
      this.m_Tracker.Add((UnityEngine.Object) this, this.m_HandleRect, DrivenTransformProperties.Anchors);
      Vector2 zero = Vector2.zero;
      Vector2 one = Vector2.one;
      float num = this.value * (1f - this.size);
      if (this.reverseValue)
      {
        zero[(int) this.axis] = 1f - num - this.size;
        one[(int) this.axis] = 1f - num;
      }
      else
      {
        zero[(int) this.axis] = num;
        one[(int) this.axis] = num + this.size;
      }
      this.m_HandleRect.anchorMin = zero;
      this.m_HandleRect.anchorMax = one;
    }

    private void UpdateDrag(PointerEventData eventData)
    {
      Vector2 localPoint;
      if (eventData.button != PointerEventData.InputButton.Left || (UnityEngine.Object) this.m_ContainerRect == (UnityEngine.Object) null || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_ContainerRect, eventData.position, eventData.pressEventCamera, out localPoint))
        return;
      Vector2 vector2 = localPoint - this.m_Offset - this.m_ContainerRect.rect.position - (this.m_HandleRect.rect.size - this.m_HandleRect.sizeDelta) * 0.5f;
      float num = (this.axis != Scrollbar.Axis.Horizontal ? this.m_ContainerRect.rect.height : this.m_ContainerRect.rect.width) * (1f - this.size);
      if ((double) num <= 0.0)
        return;
      switch (this.m_Direction)
      {
        case Scrollbar.Direction.LeftToRight:
          this.Set(vector2.x / num);
          break;
        case Scrollbar.Direction.RightToLeft:
          this.Set((float) (1.0 - (double) vector2.x / (double) num));
          break;
        case Scrollbar.Direction.BottomToTop:
          this.Set(vector2.y / num);
          break;
        case Scrollbar.Direction.TopToBottom:
          this.Set((float) (1.0 - (double) vector2.y / (double) num));
          break;
      }
    }

    private bool MayDrag(PointerEventData eventData)
    {
      if (this.IsActive() && this.IsInteractable())
        return eventData.button == PointerEventData.InputButton.Left;
      return false;
    }

    /// <summary>
    ///   <para>Handling for when the scrollbar value is begin being dragged.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      this.isPointerDownAndNotDragging = false;
      if (!this.MayDrag(eventData) || (UnityEngine.Object) this.m_ContainerRect == (UnityEngine.Object) null)
        return;
      this.m_Offset = Vector2.zero;
      Vector2 localPoint;
      if (!RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.position, eventData.enterEventCamera) || !RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.position, eventData.pressEventCamera, out localPoint))
        return;
      this.m_Offset = localPoint - this.m_HandleRect.rect.center;
    }

    /// <summary>
    ///   <para>Handling for when the scrollbar value is dragged.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDrag(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData) || !((UnityEngine.Object) this.m_ContainerRect != (UnityEngine.Object) null))
        return;
      this.UpdateDrag(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
      if (!this.MayDrag(eventData))
        return;
      base.OnPointerDown(eventData);
      this.isPointerDownAndNotDragging = true;
      this.m_PointerDownRepeat = this.StartCoroutine(this.ClickRepeat(eventData));
    }

    /// <summary>
    ///   <para>Coroutine function for handling continual press during OnPointerDown.</para>
    /// </summary>
    /// <param name="eventData"></param>
    [DebuggerHidden]
    protected IEnumerator ClickRepeat(PointerEventData eventData)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Scrollbar.\u003CClickRepeat\u003Ec__Iterator5() { eventData = eventData, \u003C\u0024\u003EeventData = eventData, \u003C\u003Ef__this = this };
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
      base.OnPointerUp(eventData);
      this.isPointerDownAndNotDragging = false;
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
            if (this.axis == Scrollbar.Axis.Horizontal && (UnityEngine.Object) this.FindSelectableOnLeft() == (UnityEngine.Object) null)
            {
              this.Set(!this.reverseValue ? this.value - this.stepSize : this.value + this.stepSize);
              break;
            }
            base.OnMove(eventData);
            break;
          case MoveDirection.Up:
            if (this.axis == Scrollbar.Axis.Vertical && (UnityEngine.Object) this.FindSelectableOnUp() == (UnityEngine.Object) null)
            {
              this.Set(!this.reverseValue ? this.value + this.stepSize : this.value - this.stepSize);
              break;
            }
            base.OnMove(eventData);
            break;
          case MoveDirection.Right:
            if (this.axis == Scrollbar.Axis.Horizontal && (UnityEngine.Object) this.FindSelectableOnRight() == (UnityEngine.Object) null)
            {
              this.Set(!this.reverseValue ? this.value + this.stepSize : this.value - this.stepSize);
              break;
            }
            base.OnMove(eventData);
            break;
          case MoveDirection.Down:
            if (this.axis == Scrollbar.Axis.Vertical && (UnityEngine.Object) this.FindSelectableOnDown() == (UnityEngine.Object) null)
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
      if (this.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Horizontal)
        return (Selectable) null;
      return base.FindSelectableOnLeft();
    }

    /// <summary>
    ///   <para>See member in base class.</para>
    /// </summary>
    public override Selectable FindSelectableOnRight()
    {
      if (this.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Horizontal)
        return (Selectable) null;
      return base.FindSelectableOnRight();
    }

    /// <summary>
    ///   <para>See member in base class.</para>
    /// </summary>
    public override Selectable FindSelectableOnUp()
    {
      if (this.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Vertical)
        return (Selectable) null;
      return base.FindSelectableOnUp();
    }

    /// <summary>
    ///   <para>See member in base class.</para>
    /// </summary>
    public override Selectable FindSelectableOnDown()
    {
      if (this.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Vertical)
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

    public void SetDirection(Scrollbar.Direction direction, bool includeRectLayouts)
    {
      Scrollbar.Axis axis = this.axis;
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
    ///   <para>UnityEvent callback for when a scrollbar is scrolled.</para>
    /// </summary>
    [Serializable]
    public class ScrollEvent : UnityEvent<float>
    {
    }

    private enum Axis
    {
      Horizontal,
      Vertical,
    }
  }
}
