// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Selectable
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Simple selectable object - derived from to create a selectable control.</para>
  /// </summary>
  [ExecuteInEditMode]
  [SelectionBase]
  [AddComponentMenu("UI/Selectable", 70)]
  [DisallowMultipleComponent]
  public class Selectable : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IMoveHandler
  {
    private static List<Selectable> s_List = new List<Selectable>();
    [SerializeField]
    [FormerlySerializedAs("navigation")]
    private Navigation m_Navigation = Navigation.defaultNavigation;
    [SerializeField]
    [FormerlySerializedAs("transition")]
    private Selectable.Transition m_Transition = Selectable.Transition.ColorTint;
    [SerializeField]
    [FormerlySerializedAs("colors")]
    private ColorBlock m_Colors = ColorBlock.defaultColorBlock;
    [SerializeField]
    [FormerlySerializedAs("animationTriggers")]
    private AnimationTriggers m_AnimationTriggers = new AnimationTriggers();
    [SerializeField]
    [Tooltip("Can the Selectable be interacted with?")]
    private bool m_Interactable = true;
    private bool m_GroupsAllowInteraction = true;
    private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();
    [FormerlySerializedAs("spriteState")]
    [SerializeField]
    private SpriteState m_SpriteState;
    [FormerlySerializedAs("highlightGraphic")]
    [SerializeField]
    [FormerlySerializedAs("m_HighlightGraphic")]
    private Graphic m_TargetGraphic;
    private Selectable.SelectionState m_CurrentSelectionState;

    /// <summary>
    ///   <para>List of all the selectable objects currently active in the scene.</para>
    /// </summary>
    public static List<Selectable> allSelectables
    {
      get
      {
        return Selectable.s_List;
      }
    }

    /// <summary>
    ///   <para>The Navigation setting for this selectable object.</para>
    /// </summary>
    public Navigation navigation
    {
      get
      {
        return this.m_Navigation;
      }
      set
      {
        if (!SetPropertyUtility.SetEquatableStruct<Navigation>(ref this.m_Navigation, value))
          return;
        this.OnSetProperty();
      }
    }

    /// <summary>
    ///   <para>The type of transition that will be applied to the targetGraphic when the state changes.</para>
    /// </summary>
    public Selectable.Transition transition
    {
      get
      {
        return this.m_Transition;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<Selectable.Transition>(ref this.m_Transition, value))
          return;
        this.OnSetProperty();
      }
    }

    /// <summary>
    ///   <para>The ColorBlock for this selectable object.</para>
    /// </summary>
    public ColorBlock colors
    {
      get
      {
        return this.m_Colors;
      }
      set
      {
        if (!SetPropertyUtility.SetEquatableStruct<ColorBlock>(ref this.m_Colors, value))
          return;
        this.OnSetProperty();
      }
    }

    /// <summary>
    ///   <para>The SpriteState for this selectable object.</para>
    /// </summary>
    public SpriteState spriteState
    {
      get
      {
        return this.m_SpriteState;
      }
      set
      {
        if (!SetPropertyUtility.SetEquatableStruct<SpriteState>(ref this.m_SpriteState, value))
          return;
        this.OnSetProperty();
      }
    }

    /// <summary>
    ///   <para>The AnimationTriggers for this selectable object.</para>
    /// </summary>
    public AnimationTriggers animationTriggers
    {
      get
      {
        return this.m_AnimationTriggers;
      }
      set
      {
        if (!SetPropertyUtility.SetClass<AnimationTriggers>(ref this.m_AnimationTriggers, value))
          return;
        this.OnSetProperty();
      }
    }

    /// <summary>
    ///   <para>Graphic that will be transitioned upon.</para>
    /// </summary>
    public Graphic targetGraphic
    {
      get
      {
        return this.m_TargetGraphic;
      }
      set
      {
        if (!SetPropertyUtility.SetClass<Graphic>(ref this.m_TargetGraphic, value))
          return;
        this.OnSetProperty();
      }
    }

    /// <summary>
    ///   <para>UI.Selectable.interactable.</para>
    /// </summary>
    public bool interactable
    {
      get
      {
        return this.m_Interactable;
      }
      set
      {
        if (!SetPropertyUtility.SetStruct<bool>(ref this.m_Interactable, value))
          return;
        if (this.m_Interactable && (UnityEngine.Object) EventSystem.current != (UnityEngine.Object) null && (UnityEngine.Object) EventSystem.current.currentSelectedGameObject == (UnityEngine.Object) this.gameObject)
          EventSystem.current.SetSelectedGameObject((GameObject) null);
        this.OnSetProperty();
      }
    }

    private bool isPointerInside { get; set; }

    private bool isPointerDown { get; set; }

    private bool hasSelection { get; set; }

    /// <summary>
    ///   <para>Convenience function that converts the referenced Graphic to a Image, if possible.</para>
    /// </summary>
    public Image image
    {
      get
      {
        return this.m_TargetGraphic as Image;
      }
      set
      {
        this.m_TargetGraphic = (Graphic) value;
      }
    }

    /// <summary>
    ///   <para>Convenience function to get the Animator component on the GameObject.</para>
    /// </summary>
    public Animator animator
    {
      get
      {
        return this.GetComponent<Animator>();
      }
    }

    protected Selectable.SelectionState currentSelectionState
    {
      get
      {
        return this.m_CurrentSelectionState;
      }
    }

    protected Selectable()
    {
    }

    protected override void Awake()
    {
      if (!((UnityEngine.Object) this.m_TargetGraphic == (UnityEngine.Object) null))
        return;
      this.m_TargetGraphic = this.GetComponent<Graphic>();
    }

    protected override void OnCanvasGroupChanged()
    {
      bool flag1 = true;
      for (Transform transform = this.transform; (UnityEngine.Object) transform != (UnityEngine.Object) null; transform = transform.parent)
      {
        transform.GetComponents<CanvasGroup>(this.m_CanvasGroupCache);
        bool flag2 = false;
        for (int index = 0; index < this.m_CanvasGroupCache.Count; ++index)
        {
          if (!this.m_CanvasGroupCache[index].interactable)
          {
            flag1 = false;
            flag2 = true;
          }
          if (this.m_CanvasGroupCache[index].ignoreParentGroups)
            flag2 = true;
        }
        if (flag2)
          break;
      }
      if (flag1 == this.m_GroupsAllowInteraction)
        return;
      this.m_GroupsAllowInteraction = flag1;
      this.OnSetProperty();
    }

    /// <summary>
    ///   <para>UI.Selectable.IsInteractable.</para>
    /// </summary>
    public virtual bool IsInteractable()
    {
      if (this.m_GroupsAllowInteraction)
        return this.m_Interactable;
      return false;
    }

    protected override void OnDidApplyAnimationProperties()
    {
      this.OnSetProperty();
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      Selectable.s_List.Add(this);
      Selectable.SelectionState selectionState = Selectable.SelectionState.Normal;
      if (this.hasSelection)
        selectionState = Selectable.SelectionState.Highlighted;
      this.m_CurrentSelectionState = selectionState;
      this.InternalEvaluateAndTransitionToSelectionState(true);
    }

    private void OnSetProperty()
    {
      if (!Application.isPlaying)
        this.InternalEvaluateAndTransitionToSelectionState(true);
      else
        this.InternalEvaluateAndTransitionToSelectionState(false);
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
      Selectable.s_List.Remove(this);
      this.InstantClearState();
      base.OnDisable();
    }

    protected override void OnValidate()
    {
      base.OnValidate();
      this.m_Colors.fadeDuration = Mathf.Max(this.m_Colors.fadeDuration, 0.0f);
      if (!this.isActiveAndEnabled)
        return;
      if (!this.interactable && (UnityEngine.Object) EventSystem.current != (UnityEngine.Object) null && (UnityEngine.Object) EventSystem.current.currentSelectedGameObject == (UnityEngine.Object) this.gameObject)
        EventSystem.current.SetSelectedGameObject((GameObject) null);
      this.DoSpriteSwap((Sprite) null);
      this.StartColorTween(Color.white, true);
      this.TriggerAnimation(this.m_AnimationTriggers.normalTrigger);
      this.InternalEvaluateAndTransitionToSelectionState(true);
    }

    protected override void Reset()
    {
      this.m_TargetGraphic = this.GetComponent<Graphic>();
    }

    /// <summary>
    ///   <para>Clear any internal state from the Selectable (used when disabling).</para>
    /// </summary>
    protected virtual void InstantClearState()
    {
      string normalTrigger = this.m_AnimationTriggers.normalTrigger;
      this.isPointerInside = false;
      this.isPointerDown = false;
      this.hasSelection = false;
      switch (this.m_Transition)
      {
        case Selectable.Transition.ColorTint:
          this.StartColorTween(Color.white, true);
          break;
        case Selectable.Transition.SpriteSwap:
          this.DoSpriteSwap((Sprite) null);
          break;
        case Selectable.Transition.Animation:
          this.TriggerAnimation(normalTrigger);
          break;
      }
    }

    protected virtual void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      Color color;
      Sprite newSprite;
      string triggername;
      switch (state)
      {
        case Selectable.SelectionState.Normal:
          color = this.m_Colors.normalColor;
          newSprite = (Sprite) null;
          triggername = this.m_AnimationTriggers.normalTrigger;
          break;
        case Selectable.SelectionState.Highlighted:
          color = this.m_Colors.highlightedColor;
          newSprite = this.m_SpriteState.highlightedSprite;
          triggername = this.m_AnimationTriggers.highlightedTrigger;
          break;
        case Selectable.SelectionState.Pressed:
          color = this.m_Colors.pressedColor;
          newSprite = this.m_SpriteState.pressedSprite;
          triggername = this.m_AnimationTriggers.pressedTrigger;
          break;
        case Selectable.SelectionState.Disabled:
          color = this.m_Colors.disabledColor;
          newSprite = this.m_SpriteState.disabledSprite;
          triggername = this.m_AnimationTriggers.disabledTrigger;
          break;
        default:
          color = Color.black;
          newSprite = (Sprite) null;
          triggername = string.Empty;
          break;
      }
      if (!this.gameObject.activeInHierarchy)
        return;
      switch (this.m_Transition)
      {
        case Selectable.Transition.ColorTint:
          this.StartColorTween(color * this.m_Colors.colorMultiplier, instant);
          break;
        case Selectable.Transition.SpriteSwap:
          this.DoSpriteSwap(newSprite);
          break;
        case Selectable.Transition.Animation:
          this.TriggerAnimation(triggername);
          break;
      }
    }

    /// <summary>
    ///   <para>Finds the selectable object next to this one.</para>
    /// </summary>
    /// <param name="dir">The direction in which to search for a neighbouring Selectable object.</param>
    /// <returns>
    ///   <para>The neighbouring Selectable object. Null if none found.</para>
    /// </returns>
    public Selectable FindSelectable(Vector3 dir)
    {
      dir = dir.normalized;
      Vector3 vector3 = this.transform.TransformPoint(Selectable.GetPointOnRectEdge(this.transform as RectTransform, (Vector2) (Quaternion.Inverse(this.transform.rotation) * dir)));
      float num1 = float.NegativeInfinity;
      Selectable selectable1 = (Selectable) null;
      for (int index = 0; index < Selectable.s_List.Count; ++index)
      {
        Selectable selectable2 = Selectable.s_List[index];
        if (!((UnityEngine.Object) selectable2 == (UnityEngine.Object) this) && !((UnityEngine.Object) selectable2 == (UnityEngine.Object) null) && selectable2.IsInteractable() && selectable2.navigation.mode != Navigation.Mode.None)
        {
          RectTransform transform = selectable2.transform as RectTransform;
          Vector3 position = !((UnityEngine.Object) transform != (UnityEngine.Object) null) ? Vector3.zero : (Vector3) transform.rect.center;
          Vector3 rhs = selectable2.transform.TransformPoint(position) - vector3;
          float num2 = Vector3.Dot(dir, rhs);
          if ((double) num2 > 0.0)
          {
            float num3 = num2 / rhs.sqrMagnitude;
            if ((double) num3 > (double) num1)
            {
              num1 = num3;
              selectable1 = selectable2;
            }
          }
        }
      }
      return selectable1;
    }

    private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
    {
      if ((UnityEngine.Object) rect == (UnityEngine.Object) null)
        return Vector3.zero;
      if (dir != Vector2.zero)
        dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
      dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
      return (Vector3) dir;
    }

    private void Navigate(AxisEventData eventData, Selectable sel)
    {
      if (!((UnityEngine.Object) sel != (UnityEngine.Object) null) || !sel.IsActive())
        return;
      eventData.selectedObject = sel.gameObject;
    }

    /// <summary>
    ///   <para>Find the selectable object to the left of this one.</para>
    /// </summary>
    /// <returns>
    ///   <para>The Selectable object to the left of current.</para>
    /// </returns>
    public virtual Selectable FindSelectableOnLeft()
    {
      if (this.m_Navigation.mode == Navigation.Mode.Explicit)
        return this.m_Navigation.selectOnLeft;
      if ((this.m_Navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None)
        return this.FindSelectable(this.transform.rotation * Vector3.left);
      return (Selectable) null;
    }

    /// <summary>
    ///   <para>Find the selectable object to the right of this one.</para>
    /// </summary>
    /// <returns>
    ///   <para>The Selectable object to the right of current.</para>
    /// </returns>
    public virtual Selectable FindSelectableOnRight()
    {
      if (this.m_Navigation.mode == Navigation.Mode.Explicit)
        return this.m_Navigation.selectOnRight;
      if ((this.m_Navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None)
        return this.FindSelectable(this.transform.rotation * Vector3.right);
      return (Selectable) null;
    }

    /// <summary>
    ///   <para>Find the selectable object above this one.</para>
    /// </summary>
    /// <returns>
    ///   <para>The Selectable object above current.</para>
    /// </returns>
    public virtual Selectable FindSelectableOnUp()
    {
      if (this.m_Navigation.mode == Navigation.Mode.Explicit)
        return this.m_Navigation.selectOnUp;
      if ((this.m_Navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None)
        return this.FindSelectable(this.transform.rotation * Vector3.up);
      return (Selectable) null;
    }

    /// <summary>
    ///   <para>Find the selectable object below this one.</para>
    /// </summary>
    /// <returns>
    ///   <para>The Selectable object below current.</para>
    /// </returns>
    public virtual Selectable FindSelectableOnDown()
    {
      if (this.m_Navigation.mode == Navigation.Mode.Explicit)
        return this.m_Navigation.selectOnDown;
      if ((this.m_Navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None)
        return this.FindSelectable(this.transform.rotation * Vector3.down);
      return (Selectable) null;
    }

    /// <summary>
    ///   <para>Determine in which of the 4 move directions the next selectable object should be found.</para>
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public virtual void OnMove(AxisEventData eventData)
    {
      switch (eventData.moveDir)
      {
        case MoveDirection.Left:
          this.Navigate(eventData, this.FindSelectableOnLeft());
          break;
        case MoveDirection.Up:
          this.Navigate(eventData, this.FindSelectableOnUp());
          break;
        case MoveDirection.Right:
          this.Navigate(eventData, this.FindSelectableOnRight());
          break;
        case MoveDirection.Down:
          this.Navigate(eventData, this.FindSelectableOnDown());
          break;
      }
    }

    private void StartColorTween(Color targetColor, bool instant)
    {
      if ((UnityEngine.Object) this.m_TargetGraphic == (UnityEngine.Object) null)
        return;
      this.m_TargetGraphic.CrossFadeColor(targetColor, !instant ? this.m_Colors.fadeDuration : 0.0f, true, true);
    }

    private void DoSpriteSwap(Sprite newSprite)
    {
      if ((UnityEngine.Object) this.image == (UnityEngine.Object) null)
        return;
      this.image.overrideSprite = newSprite;
    }

    private void TriggerAnimation(string triggername)
    {
      if ((UnityEngine.Object) this.animator == (UnityEngine.Object) null || !this.animator.isActiveAndEnabled || ((UnityEngine.Object) this.animator.runtimeAnimatorController == (UnityEngine.Object) null || string.IsNullOrEmpty(triggername)))
        return;
      this.animator.ResetTrigger(this.m_AnimationTriggers.normalTrigger);
      this.animator.ResetTrigger(this.m_AnimationTriggers.pressedTrigger);
      this.animator.ResetTrigger(this.m_AnimationTriggers.highlightedTrigger);
      this.animator.ResetTrigger(this.m_AnimationTriggers.disabledTrigger);
      this.animator.SetTrigger(triggername);
    }

    /// <summary>
    ///   <para>Is the selectable currently 'highlighted'.</para>
    /// </summary>
    /// <param name="eventData"></param>
    protected bool IsHighlighted(BaseEventData eventData)
    {
      if (!this.IsActive() || this.IsPressed())
        return false;
      bool hasSelection = this.hasSelection;
      bool flag;
      if (eventData is PointerEventData)
      {
        PointerEventData pointerEventData = eventData as PointerEventData;
        flag = ((hasSelection ? 1 : 0) | (this.isPointerDown && !this.isPointerInside && (UnityEngine.Object) pointerEventData.pointerPress == (UnityEngine.Object) this.gameObject || !this.isPointerDown && this.isPointerInside && (UnityEngine.Object) pointerEventData.pointerPress == (UnityEngine.Object) this.gameObject ? 1 : (this.isPointerDown || !this.isPointerInside ? 0 : ((UnityEngine.Object) pointerEventData.pointerPress == (UnityEngine.Object) null ? 1 : 0)))) != 0;
      }
      else
        flag = hasSelection | this.isPointerInside;
      return flag;
    }

    [Obsolete("Is Pressed no longer requires eventData", false)]
    protected bool IsPressed(BaseEventData eventData)
    {
      return this.IsPressed();
    }

    protected bool IsPressed()
    {
      if (!this.IsActive() || !this.isPointerInside)
        return false;
      return this.isPointerDown;
    }

    /// <summary>
    ///   <para>Internally update the selection state of the Selectable.</para>
    /// </summary>
    /// <param name="eventData"></param>
    protected void UpdateSelectionState(BaseEventData eventData)
    {
      if (this.IsPressed())
        this.m_CurrentSelectionState = Selectable.SelectionState.Pressed;
      else if (this.IsHighlighted(eventData))
        this.m_CurrentSelectionState = Selectable.SelectionState.Highlighted;
      else
        this.m_CurrentSelectionState = Selectable.SelectionState.Normal;
    }

    private void EvaluateAndTransitionToSelectionState(BaseEventData eventData)
    {
      if (!this.IsActive())
        return;
      this.UpdateSelectionState(eventData);
      this.InternalEvaluateAndTransitionToSelectionState(false);
    }

    private void InternalEvaluateAndTransitionToSelectionState(bool instant)
    {
      Selectable.SelectionState state = this.m_CurrentSelectionState;
      if (this.IsActive() && !this.IsInteractable())
        state = Selectable.SelectionState.Disabled;
      this.DoStateTransition(state, instant);
    }

    /// <summary>
    ///   <para>Evaluate current state and transition to pressed state.</para>
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      if (this.IsInteractable() && (this.navigation.mode != Navigation.Mode.None && (UnityEngine.Object) EventSystem.current != (UnityEngine.Object) null))
        EventSystem.current.SetSelectedGameObject(this.gameObject, (BaseEventData) eventData);
      this.isPointerDown = true;
      this.EvaluateAndTransitionToSelectionState((BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Evaluate eventData and transition to appropriate state.</para>
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.isPointerDown = false;
      this.EvaluateAndTransitionToSelectionState((BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Evaluate current state and transition to appropriate state.</para>
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      this.isPointerInside = true;
      this.EvaluateAndTransitionToSelectionState((BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Evaluate current state and transition to normal state.</para>
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
      this.isPointerInside = false;
      this.EvaluateAndTransitionToSelectionState((BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Set selection and transition to appropriate state.</para>
    /// </summary>
    /// <param name="eventData">The EventData usually sent by the EventSystem.</param>
    public virtual void OnSelect(BaseEventData eventData)
    {
      this.hasSelection = true;
      this.EvaluateAndTransitionToSelectionState(eventData);
    }

    /// <summary>
    ///   <para>Unset selection and transition to appropriate state.</para>
    /// </summary>
    /// <param name="eventData">The eventData usually sent by the EventSystem.</param>
    public virtual void OnDeselect(BaseEventData eventData)
    {
      this.hasSelection = false;
      this.EvaluateAndTransitionToSelectionState(eventData);
    }

    /// <summary>
    ///   <para>Selects this Selectable.</para>
    /// </summary>
    public virtual void Select()
    {
      if ((UnityEngine.Object) EventSystem.current == (UnityEngine.Object) null || EventSystem.current.alreadySelecting)
        return;
      EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    /// <summary>
    ///   <para>Transition mode for a Selectable.</para>
    /// </summary>
    public enum Transition
    {
      None,
      ColorTint,
      SpriteSwap,
      Animation,
    }

    protected enum SelectionState
    {
      Normal,
      Highlighted,
      Pressed,
      Disabled,
    }
  }
}
