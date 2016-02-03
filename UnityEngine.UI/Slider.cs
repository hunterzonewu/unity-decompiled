using System;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>A standard slider that can be moved between a minimum and maximum value.</para>
	/// </summary>
	[AddComponentMenu("UI/Slider", 33), RequireComponent(typeof(RectTransform))]
	public class Slider : Selectable, IEventSystemHandler, IInitializePotentialDragHandler, IDragHandler, ICanvasElement
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
		///   <para>Event type used by the Slider.</para>
		/// </summary>
		[Serializable]
		public class SliderEvent : UnityEvent<float>
		{
		}

		private enum Axis
		{
			Horizontal,
			Vertical
		}

		[SerializeField]
		private RectTransform m_FillRect;

		[SerializeField]
		private RectTransform m_HandleRect;

		[SerializeField, Space]
		private Slider.Direction m_Direction;

		[SerializeField]
		private float m_MinValue;

		[SerializeField]
		private float m_MaxValue = 1f;

		[SerializeField]
		private bool m_WholeNumbers;

		[SerializeField]
		protected float m_Value;

		[SerializeField, Space]
		private Slider.SliderEvent m_OnValueChanged = new Slider.SliderEvent();

		private Image m_FillImage;

		private Transform m_FillTransform;

		private RectTransform m_FillContainerRect;

		private Transform m_HandleTransform;

		private RectTransform m_HandleContainerRect;

		private Vector2 m_Offset = Vector2.zero;

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
				if (SetPropertyUtility.SetClass<RectTransform>(ref this.m_FillRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
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
				if (SetPropertyUtility.SetClass<RectTransform>(ref this.m_HandleRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
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
				if (SetPropertyUtility.SetStruct<Slider.Direction>(ref this.m_Direction, value))
				{
					this.UpdateVisuals();
				}
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
				if (SetPropertyUtility.SetStruct<float>(ref this.m_MinValue, value))
				{
					this.Set(this.m_Value);
					this.UpdateVisuals();
				}
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
				if (SetPropertyUtility.SetStruct<float>(ref this.m_MaxValue, value))
				{
					this.Set(this.m_Value);
					this.UpdateVisuals();
				}
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
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_WholeNumbers, value))
				{
					this.Set(this.m_Value);
					this.UpdateVisuals();
				}
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
				{
					return Mathf.Round(this.m_Value);
				}
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
				{
					return 0f;
				}
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
				return (!this.wholeNumbers) ? ((this.maxValue - this.minValue) * 0.1f) : 1f;
			}
		}

		private Slider.Axis axis
		{
			get
			{
				return (this.m_Direction != Slider.Direction.LeftToRight && this.m_Direction != Slider.Direction.RightToLeft) ? Slider.Axis.Vertical : Slider.Axis.Horizontal;
			}
		}

		private bool reverseValue
		{
			get
			{
				return this.m_Direction == Slider.Direction.RightToLeft || this.m_Direction == Slider.Direction.TopToBottom;
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

		protected override void OnDidApplyAnimationProperties()
		{
			this.m_Value = this.ClampValue(this.m_Value);
			float num = this.normalizedValue;
			if (this.m_FillContainerRect != null)
			{
				if (this.m_FillImage != null && this.m_FillImage.type == Image.Type.Filled)
				{
					num = this.m_FillImage.fillAmount;
				}
				else
				{
					num = ((!this.reverseValue) ? this.m_FillRect.anchorMax[(int)this.axis] : (1f - this.m_FillRect.anchorMin[(int)this.axis]));
				}
			}
			else if (this.m_HandleContainerRect != null)
			{
				num = ((!this.reverseValue) ? this.m_HandleRect.anchorMin[(int)this.axis] : (1f - this.m_HandleRect.anchorMin[(int)this.axis]));
			}
			this.UpdateVisuals();
			if (num != this.normalizedValue)
			{
				this.onValueChanged.Invoke(this.m_Value);
			}
		}

		private void UpdateCachedReferences()
		{
			if (this.m_FillRect)
			{
				this.m_FillTransform = this.m_FillRect.transform;
				this.m_FillImage = this.m_FillRect.GetComponent<Image>();
				if (this.m_FillTransform.parent != null)
				{
					this.m_FillContainerRect = this.m_FillTransform.parent.GetComponent<RectTransform>();
				}
			}
			else
			{
				this.m_FillContainerRect = null;
				this.m_FillImage = null;
			}
			if (this.m_HandleRect)
			{
				this.m_HandleTransform = this.m_HandleRect.transform;
				if (this.m_HandleTransform.parent != null)
				{
					this.m_HandleContainerRect = this.m_HandleTransform.parent.GetComponent<RectTransform>();
				}
			}
			else
			{
				this.m_HandleContainerRect = null;
			}
		}

		private float ClampValue(float input)
		{
			float num = Mathf.Clamp(input, this.minValue, this.maxValue);
			if (this.wholeNumbers)
			{
				num = Mathf.Round(num);
			}
			return num;
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
			if (this.m_Value == num)
			{
				return;
			}
			this.m_Value = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				this.m_OnValueChanged.Invoke(num);
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
			if (this.m_FillContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_FillRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				if (this.m_FillImage != null && this.m_FillImage.type == Image.Type.Filled)
				{
					this.m_FillImage.fillAmount = this.normalizedValue;
				}
				else if (this.reverseValue)
				{
					zero[(int)this.axis] = 1f - this.normalizedValue;
				}
				else
				{
					one[(int)this.axis] = this.normalizedValue;
				}
				this.m_FillRect.anchorMin = zero;
				this.m_FillRect.anchorMax = one;
			}
			if (this.m_HandleContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero2 = Vector2.zero;
				Vector2 one2 = Vector2.one;
				int arg_14C_1 = (int)this.axis;
				float value = (!this.reverseValue) ? this.normalizedValue : (1f - this.normalizedValue);
				one2[(int)this.axis] = value;
				zero2[arg_14C_1] = value;
				this.m_HandleRect.anchorMin = zero2;
				this.m_HandleRect.anchorMax = one2;
			}
		}

		private void UpdateDrag(PointerEventData eventData, Camera cam)
		{
			RectTransform rectTransform = this.m_HandleContainerRect ?? this.m_FillContainerRect;
			if (rectTransform != null && rectTransform.rect.size[(int)this.axis] > 0f)
			{
				Vector2 a;
				if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, cam, out a))
				{
					return;
				}
				a -= rectTransform.rect.position;
				float num = Mathf.Clamp01((a - this.m_Offset)[(int)this.axis] / rectTransform.rect.size[(int)this.axis]);
				this.normalizedValue = ((!this.reverseValue) ? num : (1f - num));
			}
		}

		private bool MayDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			this.m_Offset = Vector2.zero;
			if (this.m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.position, eventData.enterEventCamera))
			{
				Vector2 offset;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.position, eventData.pressEventCamera, out offset))
				{
					this.m_Offset = offset;
				}
			}
			else
			{
				this.UpdateDrag(eventData, eventData.pressEventCamera);
			}
		}

		/// <summary>
		///   <para>Handling for when the slider is dragged.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
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
				return;
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (this.axis == Slider.Axis.Horizontal && this.FindSelectableOnLeft() == null)
				{
					this.Set((!this.reverseValue) ? (this.value - this.stepSize) : (this.value + this.stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Up:
				if (this.axis == Slider.Axis.Vertical && this.FindSelectableOnUp() == null)
				{
					this.Set((!this.reverseValue) ? (this.value + this.stepSize) : (this.value - this.stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Right:
				if (this.axis == Slider.Axis.Horizontal && this.FindSelectableOnRight() == null)
				{
					this.Set((!this.reverseValue) ? (this.value + this.stepSize) : (this.value - this.stepSize));
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Down:
				if (this.axis == Slider.Axis.Vertical && this.FindSelectableOnDown() == null)
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
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Horizontal)
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
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Horizontal)
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
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Vertical)
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
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == Slider.Axis.Vertical)
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

		public void SetDirection(Slider.Direction direction, bool includeRectLayouts)
		{
			Slider.Axis axis = this.axis;
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
