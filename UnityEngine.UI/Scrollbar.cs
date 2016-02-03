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
	[AddComponentMenu("UI/Scrollbar", 34), RequireComponent(typeof(RectTransform))]
	public class Scrollbar : Selectable, IEventSystemHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, ICanvasElement
	{
		/// <summary>
		///   <para>Setting that indicates one of four directions.</para>
		/// </summary>
		public enum Direction
		{
			/// <summary>
			///   <para>From left to right.</para>
			/// </summary>
			LeftToRight,
			/// <summary>
			///   <para>From right to left.</para>
			/// </summary>
			RightToLeft,
			/// <summary>
			///   <para>From bottom to top.</para>
			/// </summary>
			BottomToTop,
			/// <summary>
			///   <para>From top to bottom.</para>
			/// </summary>
			TopToBottom
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
			Vertical
		}

		[SerializeField]
		private RectTransform m_HandleRect;

		[SerializeField]
		private Scrollbar.Direction m_Direction;

		[Range(0f, 1f), SerializeField]
		private float m_Value;

		[Range(0f, 1f), SerializeField]
		private float m_Size = 0.2f;

		[Range(0f, 11f), SerializeField]
		private int m_NumberOfSteps;

		[SerializeField, Space(6f)]
		private Scrollbar.ScrollEvent m_OnValueChanged = new Scrollbar.ScrollEvent();

		private RectTransform m_ContainerRect;

		private Vector2 m_Offset = Vector2.zero;

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
				if (SetPropertyUtility.SetClass<RectTransform>(ref this.m_HandleRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
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
				if (SetPropertyUtility.SetStruct<Scrollbar.Direction>(ref this.m_Direction, value))
				{
					this.UpdateVisuals();
				}
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
				{
					num = Mathf.Round(num * (float)(this.m_NumberOfSteps - 1)) / (float)(this.m_NumberOfSteps - 1);
				}
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
				if (SetPropertyUtility.SetStruct<float>(ref this.m_Size, Mathf.Clamp01(value)))
				{
					this.UpdateVisuals();
				}
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
				if (SetPropertyUtility.SetStruct<int>(ref this.m_NumberOfSteps, value))
				{
					this.Set(this.m_Value);
					this.UpdateVisuals();
				}
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
				return (this.m_NumberOfSteps <= 1) ? 0.1f : (1f / (float)(this.m_NumberOfSteps - 1));
			}
		}

		private Scrollbar.Axis axis
		{
			get
			{
				return (this.m_Direction != Scrollbar.Direction.LeftToRight && this.m_Direction != Scrollbar.Direction.RightToLeft) ? Scrollbar.Axis.Vertical : Scrollbar.Axis.Horizontal;
			}
		}

		private bool reverseValue
		{
			get
			{
				return this.m_Direction == Scrollbar.Direction.RightToLeft || this.m_Direction == Scrollbar.Direction.TopToBottom;
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
			PrefabType prefabType = PrefabUtility.GetPrefabType(this);
			if (prefabType != PrefabType.Prefab && !Application.isPlaying)
			{
				CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
			}
		}

		/// <summary>
		///   <para>Handling for when the canvas is rebuilt.</para>
		/// </summary>
		/// <param name="executing"></param>
		public virtual void Rebuild(CanvasUpdate executing)
		{
			if (executing == CanvasUpdate.Prelayout)
			{
				this.onValueChanged.Invoke(this.value);
			}
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
			if (this.m_HandleRect && this.m_HandleRect.parent != null)
			{
				this.m_ContainerRect = this.m_HandleRect.parent.GetComponent<RectTransform>();
			}
			else
			{
				this.m_ContainerRect = null;
			}
		}

		private void Set(float input)
		{
			this.Set(input, true);
		}

		private void Set(float input, bool sendCallback)
		{
			float value = this.m_Value;
			this.m_Value = Mathf.Clamp01(input);
			if (value == this.value)
			{
				return;
			}
			this.UpdateVisuals();
			if (sendCallback)
			{
				this.m_OnValueChanged.Invoke(this.value);
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateVisuals();
		}

		private void UpdateVisuals()
		{
			if (!Application.isPlaying)
			{
				this.UpdateCachedReferences();
			}
			this.m_Tracker.Clear();
			if (this.m_ContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				float num = this.value * (1f - this.size);
				if (this.reverseValue)
				{
					zero[(int)this.axis] = 1f - num - this.size;
					one[(int)this.axis] = 1f - num;
				}
				else
				{
					zero[(int)this.axis] = num;
					one[(int)this.axis] = num + this.size;
				}
				this.m_HandleRect.anchorMin = zero;
				this.m_HandleRect.anchorMax = one;
			}
		}

		private void UpdateDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (this.m_ContainerRect == null)
			{
				return;
			}
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_ContainerRect, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			Vector2 a2 = a - this.m_Offset - this.m_ContainerRect.rect.position;
			Vector2 vector = a2 - (this.m_HandleRect.rect.size - this.m_HandleRect.sizeDelta) * 0.5f;
			float num = (this.axis != Scrollbar.Axis.Horizontal) ? this.m_ContainerRect.rect.height : this.m_ContainerRect.rect.width;
			float num2 = num * (1f - this.size);
			if (num2 <= 0f)
			{
				return;
			}
			switch (this.m_Direction)
			{
			case Scrollbar.Direction.LeftToRight:
				this.Set(vector.x / num2);
				break;
			case Scrollbar.Direction.RightToLeft:
				this.Set(1f - vector.x / num2);
				break;
			case Scrollbar.Direction.BottomToTop:
				this.Set(vector.y / num2);
				break;
			case Scrollbar.Direction.TopToBottom:
				this.Set(1f - vector.y / num2);
				break;
			}
		}

		private bool MayDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		/// <summary>
		///   <para>Handling for when the scrollbar value is begin being dragged.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			this.isPointerDownAndNotDragging = false;
			if (!this.MayDrag(eventData))
			{
				return;
			}
			if (this.m_ContainerRect == null)
			{
				return;
			}
			this.m_Offset = Vector2.zero;
			Vector2 a;
			if (RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.position, eventData.enterEventCamera) && RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.position, eventData.pressEventCamera, out a))
			{
				this.m_Offset = a - this.m_HandleRect.rect.center;
			}
		}

		/// <summary>
		///   <para>Handling for when the scrollbar value is dragged.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			if (this.m_ContainerRect != null)
			{
				this.UpdateDrag(eventData);
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			this.isPointerDownAndNotDragging = true;
			this.m_PointerDownRepeat = base.StartCoroutine(this.ClickRepeat(eventData));
		}

		/// <summary>
		///   <para>Coroutine function for handling continual press during OnPointerDown.</para>
		/// </summary>
		/// <param name="eventData"></param>
		[DebuggerHidden]
		protected IEnumerator ClickRepeat(PointerEventData eventData)
		{
			Scrollbar.<ClickRepeat>c__Iterator5 <ClickRepeat>c__Iterator = new Scrollbar.<ClickRepeat>c__Iterator5();
			<ClickRepeat>c__Iterator.eventData = eventData;
			<ClickRepeat>c__Iterator.<$>eventData = eventData;
			<ClickRepeat>c__Iterator.<>f__this = this;
			return <ClickRepeat>c__Iterator;
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
				return;
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (this.axis == Scrollbar.Axis.Horizontal && this.FindSelectableOnLeft() == null)
				{
					this.Set((!this.reverseValue) ? (this.value - this.stepSize) : (this.value + this.stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Up:
				if (this.axis == Scrollbar.Axis.Vertical && this.FindSelectableOnUp() == null)
				{
					this.Set((!this.reverseValue) ? (this.value + this.stepSize) : (this.value - this.stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Right:
				if (this.axis == Scrollbar.Axis.Horizontal && this.FindSelectableOnRight() == null)
				{
					this.Set((!this.reverseValue) ? (this.value + this.stepSize) : (this.value - this.stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Down:
				if (this.axis == Scrollbar.Axis.Vertical && this.FindSelectableOnDown() == null)
				{
					this.Set((!this.reverseValue) ? (this.value - this.stepSize) : (this.value + this.stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			}
		}

		/// <summary>
		///   <para>See member in base class.</para>
		/// </summary>
		public override Selectable FindSelectableOnLeft()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnLeft();
		}

		/// <summary>
		///   <para>See member in base class.</para>
		/// </summary>
		public override Selectable FindSelectableOnRight()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnRight();
		}

		/// <summary>
		///   <para>See member in base class.</para>
		/// </summary>
		public override Selectable FindSelectableOnUp()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnUp();
		}

		/// <summary>
		///   <para>See member in base class.</para>
		/// </summary>
		public override Selectable FindSelectableOnDown()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Scrollbar.Axis.Vertical)
			{
				return null;
			}
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
			{
				return;
			}
			if (this.axis != axis)
			{
				RectTransformUtility.FlipLayoutAxes(base.transform as RectTransform, true, true);
			}
			if (this.reverseValue != reverseValue)
			{
				RectTransformUtility.FlipLayoutOnAxis(base.transform as RectTransform, (int)this.axis, true, true);
			}
		}

		virtual bool IsDestroyed()
		{
			return base.IsDestroyed();
		}

		virtual Transform get_transform()
		{
			return base.transform;
		}
	}
}
