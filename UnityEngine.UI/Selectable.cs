using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Simple selectable object - derived from to create a selectable control.</para>
	/// </summary>
	[AddComponentMenu("UI/Selectable", 70), DisallowMultipleComponent, ExecuteInEditMode, SelectionBase]
	public class Selectable : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, IMoveHandler
	{
		/// <summary>
		///   <para>Transition mode for a Selectable.</para>
		/// </summary>
		public enum Transition
		{
			/// <summary>
			///   <para>No Transition.</para>
			/// </summary>
			None,
			/// <summary>
			///   <para>Use an color tint transition.</para>
			/// </summary>
			ColorTint,
			/// <summary>
			///   <para>Use a sprite swap transition.</para>
			/// </summary>
			SpriteSwap,
			/// <summary>
			///   <para>Use an animation transition.</para>
			/// </summary>
			Animation
		}

		protected enum SelectionState
		{
			Normal,
			Highlighted,
			Pressed,
			Disabled
		}

		private static List<Selectable> s_List = new List<Selectable>();

		[FormerlySerializedAs("navigation"), SerializeField]
		private Navigation m_Navigation = Navigation.defaultNavigation;

		[FormerlySerializedAs("transition"), SerializeField]
		private Selectable.Transition m_Transition = Selectable.Transition.ColorTint;

		[FormerlySerializedAs("colors"), SerializeField]
		private ColorBlock m_Colors = ColorBlock.defaultColorBlock;

		[FormerlySerializedAs("spriteState"), SerializeField]
		private SpriteState m_SpriteState;

		[FormerlySerializedAs("animationTriggers"), SerializeField]
		private AnimationTriggers m_AnimationTriggers = new AnimationTriggers();

		[SerializeField, Tooltip("Can the Selectable be interacted with?")]
		private bool m_Interactable = true;

		[FormerlySerializedAs("m_HighlightGraphic"), FormerlySerializedAs("highlightGraphic"), SerializeField]
		private Graphic m_TargetGraphic;

		private bool m_GroupsAllowInteraction = true;

		private Selectable.SelectionState m_CurrentSelectionState;

		private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();

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
				if (SetPropertyUtility.SetStruct<Navigation>(ref this.m_Navigation, value))
				{
					this.OnSetProperty();
				}
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
				if (SetPropertyUtility.SetStruct<Selectable.Transition>(ref this.m_Transition, value))
				{
					this.OnSetProperty();
				}
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
				if (SetPropertyUtility.SetStruct<ColorBlock>(ref this.m_Colors, value))
				{
					this.OnSetProperty();
				}
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
				if (SetPropertyUtility.SetStruct<SpriteState>(ref this.m_SpriteState, value))
				{
					this.OnSetProperty();
				}
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
				if (SetPropertyUtility.SetClass<AnimationTriggers>(ref this.m_AnimationTriggers, value))
				{
					this.OnSetProperty();
				}
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
				if (SetPropertyUtility.SetClass<Graphic>(ref this.m_TargetGraphic, value))
				{
					this.OnSetProperty();
				}
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
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_Interactable, value))
				{
					this.OnSetProperty();
				}
			}
		}

		private bool isPointerInside
		{
			get;
			set;
		}

		private bool isPointerDown
		{
			get;
			set;
		}

		private bool hasSelection
		{
			get;
			set;
		}

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
				this.m_TargetGraphic = value;
			}
		}

		/// <summary>
		///   <para>Convenience function to get the Animator component on the GameObject.</para>
		/// </summary>
		public Animator animator
		{
			get
			{
				return base.GetComponent<Animator>();
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
			if (this.m_TargetGraphic == null)
			{
				this.m_TargetGraphic = base.GetComponent<Graphic>();
			}
		}

		protected override void OnCanvasGroupChanged()
		{
			bool flag = true;
			Transform transform = base.transform;
			while (transform != null)
			{
				transform.GetComponents<CanvasGroup>(this.m_CanvasGroupCache);
				bool flag2 = false;
				for (int i = 0; i < this.m_CanvasGroupCache.Count; i++)
				{
					if (!this.m_CanvasGroupCache[i].interactable)
					{
						flag = false;
						flag2 = true;
					}
					if (this.m_CanvasGroupCache[i].ignoreParentGroups)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					break;
				}
				transform = transform.parent;
			}
			if (flag != this.m_GroupsAllowInteraction)
			{
				this.m_GroupsAllowInteraction = flag;
				this.OnSetProperty();
			}
		}

		/// <summary>
		///   <para>UI.Selectable.IsInteractable.</para>
		/// </summary>
		public virtual bool IsInteractable()
		{
			return this.m_GroupsAllowInteraction && this.m_Interactable;
		}

		protected override void OnDidApplyAnimationProperties()
		{
			this.OnSetProperty();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			Selectable.s_List.Add(this);
			Selectable.SelectionState currentSelectionState = Selectable.SelectionState.Normal;
			if (this.hasSelection)
			{
				currentSelectionState = Selectable.SelectionState.Highlighted;
			}
			this.m_CurrentSelectionState = currentSelectionState;
			this.InternalEvaluateAndTransitionToSelectionState(true);
		}

		private void OnSetProperty()
		{
			if (!Application.isPlaying)
			{
				this.InternalEvaluateAndTransitionToSelectionState(true);
			}
			else
			{
				this.InternalEvaluateAndTransitionToSelectionState(false);
			}
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
			this.m_Colors.fadeDuration = Mathf.Max(this.m_Colors.fadeDuration, 0f);
			if (base.isActiveAndEnabled)
			{
				this.DoSpriteSwap(null);
				this.StartColorTween(Color.white, true);
				this.TriggerAnimation(this.m_AnimationTriggers.normalTrigger);
				this.InternalEvaluateAndTransitionToSelectionState(true);
			}
		}

		protected override void Reset()
		{
			this.m_TargetGraphic = base.GetComponent<Graphic>();
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
				this.DoSpriteSwap(null);
				break;
			case Selectable.Transition.Animation:
				this.TriggerAnimation(normalTrigger);
				break;
			}
		}

		protected virtual void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			Color a;
			Sprite newSprite;
			string triggername;
			switch (state)
			{
			case Selectable.SelectionState.Normal:
				a = this.m_Colors.normalColor;
				newSprite = null;
				triggername = this.m_AnimationTriggers.normalTrigger;
				break;
			case Selectable.SelectionState.Highlighted:
				a = this.m_Colors.highlightedColor;
				newSprite = this.m_SpriteState.highlightedSprite;
				triggername = this.m_AnimationTriggers.highlightedTrigger;
				break;
			case Selectable.SelectionState.Pressed:
				a = this.m_Colors.pressedColor;
				newSprite = this.m_SpriteState.pressedSprite;
				triggername = this.m_AnimationTriggers.pressedTrigger;
				break;
			case Selectable.SelectionState.Disabled:
				a = this.m_Colors.disabledColor;
				newSprite = this.m_SpriteState.disabledSprite;
				triggername = this.m_AnimationTriggers.disabledTrigger;
				break;
			default:
				a = Color.black;
				newSprite = null;
				triggername = string.Empty;
				break;
			}
			if (base.gameObject.activeInHierarchy)
			{
				switch (this.m_Transition)
				{
				case Selectable.Transition.ColorTint:
					this.StartColorTween(a * this.m_Colors.colorMultiplier, instant);
					break;
				case Selectable.Transition.SpriteSwap:
					this.DoSpriteSwap(newSprite);
					break;
				case Selectable.Transition.Animation:
					this.TriggerAnimation(triggername);
					break;
				}
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
			Vector3 v = Quaternion.Inverse(base.transform.rotation) * dir;
			Vector3 b = base.transform.TransformPoint(Selectable.GetPointOnRectEdge(base.transform as RectTransform, v));
			float num = float.NegativeInfinity;
			Selectable result = null;
			for (int i = 0; i < Selectable.s_List.Count; i++)
			{
				Selectable selectable = Selectable.s_List[i];
				if (!(selectable == this) && !(selectable == null))
				{
					if (selectable.IsInteractable() && selectable.navigation.mode != Navigation.Mode.None)
					{
						RectTransform rectTransform = selectable.transform as RectTransform;
						Vector3 position = (!(rectTransform != null)) ? Vector3.zero : rectTransform.rect.center;
						Vector3 rhs = selectable.transform.TransformPoint(position) - b;
						float num2 = Vector3.Dot(dir, rhs);
						if (num2 > 0f)
						{
							float num3 = num2 / rhs.sqrMagnitude;
							if (num3 > num)
							{
								num = num3;
								result = selectable;
							}
						}
					}
				}
			}
			return result;
		}

		private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
		{
			if (rect == null)
			{
				return Vector3.zero;
			}
			if (dir != Vector2.zero)
			{
				dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
			}
			dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
			return dir;
		}

		private void Navigate(AxisEventData eventData, Selectable sel)
		{
			if (sel != null && sel.IsActive())
			{
				eventData.selectedObject = sel.gameObject;
			}
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
			{
				return this.m_Navigation.selectOnLeft;
			}
			if ((this.m_Navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None)
			{
				return this.FindSelectable(base.transform.rotation * Vector3.left);
			}
			return null;
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
			{
				return this.m_Navigation.selectOnRight;
			}
			if ((this.m_Navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None)
			{
				return this.FindSelectable(base.transform.rotation * Vector3.right);
			}
			return null;
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
			{
				return this.m_Navigation.selectOnUp;
			}
			if ((this.m_Navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None)
			{
				return this.FindSelectable(base.transform.rotation * Vector3.up);
			}
			return null;
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
			{
				return this.m_Navigation.selectOnDown;
			}
			if ((this.m_Navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None)
			{
				return this.FindSelectable(base.transform.rotation * Vector3.down);
			}
			return null;
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
			if (this.m_TargetGraphic == null)
			{
				return;
			}
			this.m_TargetGraphic.CrossFadeColor(targetColor, (!instant) ? this.m_Colors.fadeDuration : 0f, true, true);
		}

		private void DoSpriteSwap(Sprite newSprite)
		{
			if (this.image == null)
			{
				return;
			}
			this.image.overrideSprite = newSprite;
		}

		private void TriggerAnimation(string triggername)
		{
			if (this.animator == null || !this.animator.isActiveAndEnabled || this.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
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
			if (!this.IsActive())
			{
				return false;
			}
			if (this.IsPressed())
			{
				return false;
			}
			bool flag = this.hasSelection;
			if (eventData is PointerEventData)
			{
				PointerEventData pointerEventData = eventData as PointerEventData;
				flag |= ((this.isPointerDown && !this.isPointerInside && pointerEventData.pointerPress == base.gameObject) || (!this.isPointerDown && this.isPointerInside && pointerEventData.pointerPress == base.gameObject) || (!this.isPointerDown && this.isPointerInside && pointerEventData.pointerPress == null));
			}
			else
			{
				flag |= this.isPointerInside;
			}
			return flag;
		}

		[Obsolete("Is Pressed no longer requires eventData", false)]
		protected bool IsPressed(BaseEventData eventData)
		{
			return this.IsPressed();
		}

		protected bool IsPressed()
		{
			return this.IsActive() && this.isPointerInside && this.isPointerDown;
		}

		/// <summary>
		///   <para>Internally update the selection state of the Selectable.</para>
		/// </summary>
		/// <param name="eventData"></param>
		protected void UpdateSelectionState(BaseEventData eventData)
		{
			if (this.IsPressed())
			{
				this.m_CurrentSelectionState = Selectable.SelectionState.Pressed;
				return;
			}
			if (this.IsHighlighted(eventData))
			{
				this.m_CurrentSelectionState = Selectable.SelectionState.Highlighted;
				return;
			}
			this.m_CurrentSelectionState = Selectable.SelectionState.Normal;
		}

		private void EvaluateAndTransitionToSelectionState(BaseEventData eventData)
		{
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateSelectionState(eventData);
			this.InternalEvaluateAndTransitionToSelectionState(false);
		}

		private void InternalEvaluateAndTransitionToSelectionState(bool instant)
		{
			Selectable.SelectionState state = this.m_CurrentSelectionState;
			if (this.IsActive() && !this.IsInteractable())
			{
				state = Selectable.SelectionState.Disabled;
			}
			this.DoStateTransition(state, instant);
		}

		/// <summary>
		///   <para>Evaluate current state and transition to pressed state.</para>
		/// </summary>
		/// <param name="eventData">The EventData usually sent by the EventSystem.</param>
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (this.IsInteractable() && this.navigation.mode != Navigation.Mode.None)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			}
			this.isPointerDown = true;
			this.EvaluateAndTransitionToSelectionState(eventData);
		}

		/// <summary>
		///   <para>Evaluate eventData and transition to appropriate state.</para>
		/// </summary>
		/// <param name="eventData">The EventData usually sent by the EventSystem.</param>
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.isPointerDown = false;
			this.EvaluateAndTransitionToSelectionState(eventData);
		}

		/// <summary>
		///   <para>Evaluate current state and transition to appropriate state.</para>
		/// </summary>
		/// <param name="eventData">The EventData usually sent by the EventSystem.</param>
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			this.isPointerInside = true;
			this.EvaluateAndTransitionToSelectionState(eventData);
		}

		/// <summary>
		///   <para>Evaluate current state and transition to normal state.</para>
		/// </summary>
		/// <param name="eventData">The EventData usually sent by the EventSystem.</param>
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.isPointerInside = false;
			this.EvaluateAndTransitionToSelectionState(eventData);
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
			if (EventSystem.current.alreadySelecting)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(base.gameObject);
		}
	}
}
