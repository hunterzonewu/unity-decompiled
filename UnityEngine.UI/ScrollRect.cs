using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>A component for making a child RectTransform scroll.</para>
	/// </summary>
	[AddComponentMenu("UI/Scroll Rect", 37), DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(RectTransform)), SelectionBase]
	public class ScrollRect : UIBehaviour, IEventSystemHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, ICanvasElement, ILayoutElement, ILayoutController, ILayoutGroup
	{
		/// <summary>
		///   <para>A setting for which behavior to use when content moves beyond the confines of its container.</para>
		/// </summary>
		public enum MovementType
		{
			/// <summary>
			///   <para>Unrestricted movement. The content can move forever.</para>
			/// </summary>
			Unrestricted,
			/// <summary>
			///   <para>Elastic movement. The content is allowed to temporarily move beyond the container, but is pulled back elastically.</para>
			/// </summary>
			Elastic,
			/// <summary>
			///   <para>Clamped movement. The content can not be moved beyond its container.</para>
			/// </summary>
			Clamped
		}

		/// <summary>
		///   <para>Enum for which behavior to use for scrollbar visibility.</para>
		/// </summary>
		public enum ScrollbarVisibility
		{
			/// <summary>
			///   <para>Always show the scrollbar.</para>
			/// </summary>
			Permanent,
			/// <summary>
			///   <para>Automatically hide the scrollbar when no scrolling is needed on this axis. The viewport rect will not be changed.</para>
			/// </summary>
			AutoHide,
			/// <summary>
			///   <para>Automatically hide the scrollbar when no scrolling is needed on this axis, and expand the viewport rect accordingly.</para>
			/// </summary>
			AutoHideAndExpandViewport
		}

		/// <summary>
		///   <para>Event type used by the ScrollRect.</para>
		/// </summary>
		[Serializable]
		public class ScrollRectEvent : UnityEvent<Vector2>
		{
		}

		[SerializeField]
		private RectTransform m_Content;

		[SerializeField]
		private bool m_Horizontal = true;

		[SerializeField]
		private bool m_Vertical = true;

		[SerializeField]
		private ScrollRect.MovementType m_MovementType = ScrollRect.MovementType.Elastic;

		[SerializeField]
		private float m_Elasticity = 0.1f;

		[SerializeField]
		private bool m_Inertia = true;

		[SerializeField]
		private float m_DecelerationRate = 0.135f;

		[SerializeField]
		private float m_ScrollSensitivity = 1f;

		[SerializeField]
		private RectTransform m_Viewport;

		[SerializeField]
		private Scrollbar m_HorizontalScrollbar;

		[SerializeField]
		private Scrollbar m_VerticalScrollbar;

		[SerializeField]
		private ScrollRect.ScrollbarVisibility m_HorizontalScrollbarVisibility;

		[SerializeField]
		private ScrollRect.ScrollbarVisibility m_VerticalScrollbarVisibility;

		[SerializeField]
		private float m_HorizontalScrollbarSpacing;

		[SerializeField]
		private float m_VerticalScrollbarSpacing;

		[SerializeField]
		private ScrollRect.ScrollRectEvent m_OnValueChanged = new ScrollRect.ScrollRectEvent();

		private Vector2 m_PointerStartLocalCursor = Vector2.zero;

		private Vector2 m_ContentStartPosition = Vector2.zero;

		private RectTransform m_ViewRect;

		private Bounds m_ContentBounds;

		private Bounds m_ViewBounds;

		private Vector2 m_Velocity;

		private bool m_Dragging;

		private Vector2 m_PrevPosition = Vector2.zero;

		private Bounds m_PrevContentBounds;

		private Bounds m_PrevViewBounds;

		[NonSerialized]
		private bool m_HasRebuiltLayout;

		private bool m_HSliderExpand;

		private bool m_VSliderExpand;

		private float m_HSliderHeight;

		private float m_VSliderWidth;

		[NonSerialized]
		private RectTransform m_Rect;

		private RectTransform m_HorizontalScrollbarRect;

		private RectTransform m_VerticalScrollbarRect;

		private DrivenRectTransformTracker m_Tracker;

		private readonly Vector3[] m_Corners = new Vector3[4];

		/// <summary>
		///   <para>The content that can be scrolled. It should be a child of the GameObject with ScrollRect on it.</para>
		/// </summary>
		public RectTransform content
		{
			get
			{
				return this.m_Content;
			}
			set
			{
				this.m_Content = value;
			}
		}

		/// <summary>
		///   <para>Should horizontal scrolling be enabled?</para>
		/// </summary>
		public bool horizontal
		{
			get
			{
				return this.m_Horizontal;
			}
			set
			{
				this.m_Horizontal = value;
			}
		}

		/// <summary>
		///   <para>Should vertical scrolling be enabled?</para>
		/// </summary>
		public bool vertical
		{
			get
			{
				return this.m_Vertical;
			}
			set
			{
				this.m_Vertical = value;
			}
		}

		/// <summary>
		///   <para>The behavior to use when the content moves beyond the scroll rect.</para>
		/// </summary>
		public ScrollRect.MovementType movementType
		{
			get
			{
				return this.m_MovementType;
			}
			set
			{
				this.m_MovementType = value;
			}
		}

		/// <summary>
		///   <para>The amount of elasticity to use when the content moves beyond the scroll rect.</para>
		/// </summary>
		public float elasticity
		{
			get
			{
				return this.m_Elasticity;
			}
			set
			{
				this.m_Elasticity = value;
			}
		}

		/// <summary>
		///   <para>Should movement inertia be enabled?</para>
		/// </summary>
		public bool inertia
		{
			get
			{
				return this.m_Inertia;
			}
			set
			{
				this.m_Inertia = value;
			}
		}

		/// <summary>
		///   <para>The rate at which movement slows down.</para>
		/// </summary>
		public float decelerationRate
		{
			get
			{
				return this.m_DecelerationRate;
			}
			set
			{
				this.m_DecelerationRate = value;
			}
		}

		/// <summary>
		///   <para>The sensitivity to scroll wheel and track pad scroll events.</para>
		/// </summary>
		public float scrollSensitivity
		{
			get
			{
				return this.m_ScrollSensitivity;
			}
			set
			{
				this.m_ScrollSensitivity = value;
			}
		}

		/// <summary>
		///   <para>Reference to the viewport RectTransform that is the parent of the content RectTransform.</para>
		/// </summary>
		public RectTransform viewport
		{
			get
			{
				return this.m_Viewport;
			}
			set
			{
				this.m_Viewport = value;
				this.SetDirtyCaching();
			}
		}

		/// <summary>
		///   <para>Optional Scrollbar object linked to the horizontal scrolling of the ScrollRect.</para>
		/// </summary>
		public Scrollbar horizontalScrollbar
		{
			get
			{
				return this.m_HorizontalScrollbar;
			}
			set
			{
				if (this.m_HorizontalScrollbar)
				{
					this.m_HorizontalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.SetHorizontalNormalizedPosition));
				}
				this.m_HorizontalScrollbar = value;
				if (this.m_HorizontalScrollbar)
				{
					this.m_HorizontalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.SetHorizontalNormalizedPosition));
				}
				this.SetDirtyCaching();
			}
		}

		/// <summary>
		///   <para>Optional Scrollbar object linked to the vertical scrolling of the ScrollRect.</para>
		/// </summary>
		public Scrollbar verticalScrollbar
		{
			get
			{
				return this.m_VerticalScrollbar;
			}
			set
			{
				if (this.m_VerticalScrollbar)
				{
					this.m_VerticalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
				}
				this.m_VerticalScrollbar = value;
				if (this.m_VerticalScrollbar)
				{
					this.m_VerticalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
				}
				this.SetDirtyCaching();
			}
		}

		/// <summary>
		///   <para>The mode of visibility for the horizontal scrollbar.</para>
		/// </summary>
		public ScrollRect.ScrollbarVisibility horizontalScrollbarVisibility
		{
			get
			{
				return this.m_HorizontalScrollbarVisibility;
			}
			set
			{
				this.m_HorizontalScrollbarVisibility = value;
				this.SetDirtyCaching();
			}
		}

		/// <summary>
		///   <para>The mode of visibility for the vertical scrollbar.</para>
		/// </summary>
		public ScrollRect.ScrollbarVisibility verticalScrollbarVisibility
		{
			get
			{
				return this.m_VerticalScrollbarVisibility;
			}
			set
			{
				this.m_VerticalScrollbarVisibility = value;
				this.SetDirtyCaching();
			}
		}

		/// <summary>
		///   <para>The space between the scrollbar and the viewport.</para>
		/// </summary>
		public float horizontalScrollbarSpacing
		{
			get
			{
				return this.m_HorizontalScrollbarSpacing;
			}
			set
			{
				this.m_HorizontalScrollbarSpacing = value;
				this.SetDirty();
			}
		}

		/// <summary>
		///   <para>The space between the scrollbar and the viewport.</para>
		/// </summary>
		public float verticalScrollbarSpacing
		{
			get
			{
				return this.m_VerticalScrollbarSpacing;
			}
			set
			{
				this.m_VerticalScrollbarSpacing = value;
				this.SetDirty();
			}
		}

		/// <summary>
		///   <para>Callback executed when the scroll position of the slider is changed.</para>
		/// </summary>
		public ScrollRect.ScrollRectEvent onValueChanged
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

		protected RectTransform viewRect
		{
			get
			{
				if (this.m_ViewRect == null)
				{
					this.m_ViewRect = this.m_Viewport;
				}
				if (this.m_ViewRect == null)
				{
					this.m_ViewRect = (RectTransform)base.transform;
				}
				return this.m_ViewRect;
			}
		}

		/// <summary>
		///   <para>The current velocity of the content.</para>
		/// </summary>
		public Vector2 velocity
		{
			get
			{
				return this.m_Velocity;
			}
			set
			{
				this.m_Velocity = value;
			}
		}

		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		/// <summary>
		///   <para>The scroll position as a Vector2 between (0,0) and (1,1) with (0,0) being the lower left corner.</para>
		/// </summary>
		public Vector2 normalizedPosition
		{
			get
			{
				return new Vector2(this.horizontalNormalizedPosition, this.verticalNormalizedPosition);
			}
			set
			{
				this.SetNormalizedPosition(value.x, 0);
				this.SetNormalizedPosition(value.y, 1);
			}
		}

		/// <summary>
		///   <para>The horizontal scroll position as a value between 0 and 1, with 0 being at the left.</para>
		/// </summary>
		public float horizontalNormalizedPosition
		{
			get
			{
				this.UpdateBounds();
				if (this.m_ContentBounds.size.x <= this.m_ViewBounds.size.x)
				{
					return (float)((this.m_ViewBounds.min.x <= this.m_ContentBounds.min.x) ? 0 : 1);
				}
				return (this.m_ViewBounds.min.x - this.m_ContentBounds.min.x) / (this.m_ContentBounds.size.x - this.m_ViewBounds.size.x);
			}
			set
			{
				this.SetNormalizedPosition(value, 0);
			}
		}

		/// <summary>
		///   <para>The vertical scroll position as a value between 0 and 1, with 0 being at the bottom.</para>
		/// </summary>
		public float verticalNormalizedPosition
		{
			get
			{
				this.UpdateBounds();
				if (this.m_ContentBounds.size.y <= this.m_ViewBounds.size.y)
				{
					return (float)((this.m_ViewBounds.min.y <= this.m_ContentBounds.min.y) ? 0 : 1);
				}
				return (this.m_ViewBounds.min.y - this.m_ContentBounds.min.y) / (this.m_ContentBounds.size.y - this.m_ViewBounds.size.y);
			}
			set
			{
				this.SetNormalizedPosition(value, 1);
			}
		}

		private bool hScrollingNeeded
		{
			get
			{
				return !Application.isPlaying || this.m_ContentBounds.size.x > this.m_ViewBounds.size.x + 0.01f;
			}
		}

		private bool vScrollingNeeded
		{
			get
			{
				return !Application.isPlaying || this.m_ContentBounds.size.y > this.m_ViewBounds.size.y + 0.01f;
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float minWidth
		{
			get
			{
				return -1f;
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float preferredWidth
		{
			get
			{
				return -1f;
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
				return -1f;
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual float preferredHeight
		{
			get
			{
				return -1f;
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
				return -1;
			}
		}

		protected ScrollRect()
		{
		}

		/// <summary>
		///   <para>Rebuilds the scroll rect data after initialization.</para>
		/// </summary>
		/// <param name="executing">The current step of the rendering CanvasUpdate cycle.</param>
		public virtual void Rebuild(CanvasUpdate executing)
		{
			if (executing == CanvasUpdate.Prelayout)
			{
				this.UpdateCachedData();
			}
			if (executing == CanvasUpdate.PostLayout)
			{
				this.UpdateBounds();
				this.UpdateScrollbars(Vector2.zero);
				this.UpdatePrevData();
				this.m_HasRebuiltLayout = true;
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

		private void UpdateCachedData()
		{
			Transform transform = base.transform;
			this.m_HorizontalScrollbarRect = ((!(this.m_HorizontalScrollbar == null)) ? (this.m_HorizontalScrollbar.transform as RectTransform) : null);
			this.m_VerticalScrollbarRect = ((!(this.m_VerticalScrollbar == null)) ? (this.m_VerticalScrollbar.transform as RectTransform) : null);
			bool flag = this.viewRect.parent == transform;
			bool flag2 = !this.m_HorizontalScrollbarRect || this.m_HorizontalScrollbarRect.parent == transform;
			bool flag3 = !this.m_VerticalScrollbarRect || this.m_VerticalScrollbarRect.parent == transform;
			bool flag4 = flag && flag2 && flag3;
			this.m_HSliderExpand = (flag4 && this.m_HorizontalScrollbarRect && this.horizontalScrollbarVisibility == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
			this.m_VSliderExpand = (flag4 && this.m_VerticalScrollbarRect && this.verticalScrollbarVisibility == ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
			this.m_HSliderHeight = ((!(this.m_HorizontalScrollbarRect == null)) ? this.m_HorizontalScrollbarRect.rect.height : 0f);
			this.m_VSliderWidth = ((!(this.m_VerticalScrollbarRect == null)) ? this.m_VerticalScrollbarRect.rect.width : 0f);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.m_HorizontalScrollbar)
			{
				this.m_HorizontalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.SetHorizontalNormalizedPosition));
			}
			if (this.m_VerticalScrollbar)
			{
				this.m_VerticalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
			}
			CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnDisable.</para>
		/// </summary>
		protected override void OnDisable()
		{
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_HorizontalScrollbar)
			{
				this.m_HorizontalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.SetHorizontalNormalizedPosition));
			}
			if (this.m_VerticalScrollbar)
			{
				this.m_VerticalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
			}
			this.m_HasRebuiltLayout = false;
			this.m_Tracker.Clear();
			this.m_Velocity = Vector2.zero;
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		/// <summary>
		///   <para>See member in base class.</para>
		/// </summary>
		public override bool IsActive()
		{
			return base.IsActive() && this.m_Content != null;
		}

		private void EnsureLayoutHasRebuilt()
		{
			if (!this.m_HasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
			{
				Canvas.ForceUpdateCanvases();
			}
		}

		/// <summary>
		///   <para>Sets the velocity to zero on both axes so the content stops moving.</para>
		/// </summary>
		public virtual void StopMovement()
		{
			this.m_Velocity = Vector2.zero;
		}

		/// <summary>
		///   <para>See IScrollHandler.OnScroll.</para>
		/// </summary>
		/// <param name="data"></param>
		public virtual void OnScroll(PointerEventData data)
		{
			if (!this.IsActive())
			{
				return;
			}
			this.EnsureLayoutHasRebuilt();
			this.UpdateBounds();
			Vector2 scrollDelta = data.scrollDelta;
			scrollDelta.y *= -1f;
			if (this.vertical && !this.horizontal)
			{
				if (Mathf.Abs(scrollDelta.x) > Mathf.Abs(scrollDelta.y))
				{
					scrollDelta.y = scrollDelta.x;
				}
				scrollDelta.x = 0f;
			}
			if (this.horizontal && !this.vertical)
			{
				if (Mathf.Abs(scrollDelta.y) > Mathf.Abs(scrollDelta.x))
				{
					scrollDelta.x = scrollDelta.y;
				}
				scrollDelta.y = 0f;
			}
			Vector2 vector = this.m_Content.anchoredPosition;
			vector += scrollDelta * this.m_ScrollSensitivity;
			if (this.m_MovementType == ScrollRect.MovementType.Clamped)
			{
				vector += this.CalculateOffset(vector - this.m_Content.anchoredPosition);
			}
			this.SetContentAnchoredPosition(vector);
			this.UpdateBounds();
		}

		/// <summary>
		///   <para>See: IInitializePotentialDragHandler.OnInitializePotentialDrag.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.m_Velocity = Vector2.zero;
		}

		/// <summary>
		///   <para>Handling for when the content is beging being dragged.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateBounds();
			this.m_PointerStartLocalCursor = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, out this.m_PointerStartLocalCursor);
			this.m_ContentStartPosition = this.m_Content.anchoredPosition;
			this.m_Dragging = true;
		}

		/// <summary>
		///   <para>Handling for when the content has finished being dragged.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.m_Dragging = false;
		}

		/// <summary>
		///   <para>Handling for when the content is dragged.</para>
		/// </summary>
		/// <param name="eventData"></param>
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (!this.IsActive())
			{
				return;
			}
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			this.UpdateBounds();
			Vector2 b = a - this.m_PointerStartLocalCursor;
			Vector2 vector = this.m_ContentStartPosition + b;
			Vector2 b2 = this.CalculateOffset(vector - this.m_Content.anchoredPosition);
			vector += b2;
			if (this.m_MovementType == ScrollRect.MovementType.Elastic)
			{
				if (b2.x != 0f)
				{
					vector.x -= ScrollRect.RubberDelta(b2.x, this.m_ViewBounds.size.x);
				}
				if (b2.y != 0f)
				{
					vector.y -= ScrollRect.RubberDelta(b2.y, this.m_ViewBounds.size.y);
				}
			}
			this.SetContentAnchoredPosition(vector);
		}

		/// <summary>
		///   <para>Sets the anchored position of the content.</para>
		/// </summary>
		/// <param name="position"></param>
		protected virtual void SetContentAnchoredPosition(Vector2 position)
		{
			if (!this.m_Horizontal)
			{
				position.x = this.m_Content.anchoredPosition.x;
			}
			if (!this.m_Vertical)
			{
				position.y = this.m_Content.anchoredPosition.y;
			}
			if (position != this.m_Content.anchoredPosition)
			{
				this.m_Content.anchoredPosition = position;
				this.UpdateBounds();
			}
		}

		protected virtual void LateUpdate()
		{
			if (!this.m_Content)
			{
				return;
			}
			this.EnsureLayoutHasRebuilt();
			this.UpdateScrollbarVisibility();
			this.UpdateBounds();
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			Vector2 vector = this.CalculateOffset(Vector2.zero);
			if (!this.m_Dragging && (vector != Vector2.zero || this.m_Velocity != Vector2.zero))
			{
				Vector2 vector2 = this.m_Content.anchoredPosition;
				for (int i = 0; i < 2; i++)
				{
					if (this.m_MovementType == ScrollRect.MovementType.Elastic && vector[i] != 0f)
					{
						float value = this.m_Velocity[i];
						vector2[i] = Mathf.SmoothDamp(this.m_Content.anchoredPosition[i], this.m_Content.anchoredPosition[i] + vector[i], ref value, this.m_Elasticity, float.PositiveInfinity, unscaledDeltaTime);
						this.m_Velocity[i] = value;
					}
					else if (this.m_Inertia)
					{
						int index;
						int expr_11A = index = i;
						float num = this.m_Velocity[index];
						this.m_Velocity[expr_11A] = num * Mathf.Pow(this.m_DecelerationRate, unscaledDeltaTime);
						if (Mathf.Abs(this.m_Velocity[i]) < 1f)
						{
							this.m_Velocity[i] = 0f;
						}
						int expr_16E = index = i;
						num = vector2[index];
						vector2[expr_16E] = num + this.m_Velocity[i] * unscaledDeltaTime;
					}
					else
					{
						this.m_Velocity[i] = 0f;
					}
				}
				if (this.m_Velocity != Vector2.zero)
				{
					if (this.m_MovementType == ScrollRect.MovementType.Clamped)
					{
						vector = this.CalculateOffset(vector2 - this.m_Content.anchoredPosition);
						vector2 += vector;
					}
					this.SetContentAnchoredPosition(vector2);
				}
			}
			if (this.m_Dragging && this.m_Inertia)
			{
				Vector3 b = (this.m_Content.anchoredPosition - this.m_PrevPosition) / unscaledDeltaTime;
				this.m_Velocity = Vector3.Lerp(this.m_Velocity, b, unscaledDeltaTime * 10f);
			}
			if (this.m_ViewBounds != this.m_PrevViewBounds || this.m_ContentBounds != this.m_PrevContentBounds || this.m_Content.anchoredPosition != this.m_PrevPosition)
			{
				this.UpdateScrollbars(vector);
				this.m_OnValueChanged.Invoke(this.normalizedPosition);
				this.UpdatePrevData();
			}
		}

		private void UpdatePrevData()
		{
			if (this.m_Content == null)
			{
				this.m_PrevPosition = Vector2.zero;
			}
			else
			{
				this.m_PrevPosition = this.m_Content.anchoredPosition;
			}
			this.m_PrevViewBounds = this.m_ViewBounds;
			this.m_PrevContentBounds = this.m_ContentBounds;
		}

		private void UpdateScrollbars(Vector2 offset)
		{
			if (this.m_HorizontalScrollbar)
			{
				if (this.m_ContentBounds.size.x > 0f)
				{
					this.m_HorizontalScrollbar.size = Mathf.Clamp01((this.m_ViewBounds.size.x - Mathf.Abs(offset.x)) / this.m_ContentBounds.size.x);
				}
				else
				{
					this.m_HorizontalScrollbar.size = 1f;
				}
				this.m_HorizontalScrollbar.value = this.horizontalNormalizedPosition;
			}
			if (this.m_VerticalScrollbar)
			{
				if (this.m_ContentBounds.size.y > 0f)
				{
					this.m_VerticalScrollbar.size = Mathf.Clamp01((this.m_ViewBounds.size.y - Mathf.Abs(offset.y)) / this.m_ContentBounds.size.y);
				}
				else
				{
					this.m_VerticalScrollbar.size = 1f;
				}
				this.m_VerticalScrollbar.value = this.verticalNormalizedPosition;
			}
		}

		private void SetHorizontalNormalizedPosition(float value)
		{
			this.SetNormalizedPosition(value, 0);
		}

		private void SetVerticalNormalizedPosition(float value)
		{
			this.SetNormalizedPosition(value, 1);
		}

		private void SetNormalizedPosition(float value, int axis)
		{
			this.EnsureLayoutHasRebuilt();
			this.UpdateBounds();
			float num = this.m_ContentBounds.size[axis] - this.m_ViewBounds.size[axis];
			float num2 = this.m_ViewBounds.min[axis] - value * num;
			float num3 = this.m_Content.localPosition[axis] + num2 - this.m_ContentBounds.min[axis];
			Vector3 localPosition = this.m_Content.localPosition;
			if (Mathf.Abs(localPosition[axis] - num3) > 0.01f)
			{
				localPosition[axis] = num3;
				this.m_Content.localPosition = localPosition;
				this.m_Velocity[axis] = 0f;
				this.UpdateBounds();
			}
		}

		private static float RubberDelta(float overStretching, float viewSize)
		{
			return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching);
		}

		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
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

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual void SetLayoutHorizontal()
		{
			this.m_Tracker.Clear();
			if (this.m_HSliderExpand || this.m_VSliderExpand)
			{
				this.m_Tracker.Add(this, this.viewRect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
				this.viewRect.anchorMin = Vector2.zero;
				this.viewRect.anchorMax = Vector2.one;
				this.viewRect.sizeDelta = Vector2.zero;
				this.viewRect.anchoredPosition = Vector2.zero;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.content);
				this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				this.m_ContentBounds = this.GetBounds();
			}
			if (this.m_VSliderExpand && this.vScrollingNeeded)
			{
				this.viewRect.sizeDelta = new Vector2(-(this.m_VSliderWidth + this.m_VerticalScrollbarSpacing), this.viewRect.sizeDelta.y);
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.content);
				this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				this.m_ContentBounds = this.GetBounds();
			}
			if (this.m_HSliderExpand && this.hScrollingNeeded)
			{
				this.viewRect.sizeDelta = new Vector2(this.viewRect.sizeDelta.x, -(this.m_HSliderHeight + this.m_HorizontalScrollbarSpacing));
				this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				this.m_ContentBounds = this.GetBounds();
			}
			if (this.m_VSliderExpand && this.vScrollingNeeded && this.viewRect.sizeDelta.x == 0f && this.viewRect.sizeDelta.y < 0f)
			{
				this.viewRect.sizeDelta = new Vector2(-(this.m_VSliderWidth + this.m_VerticalScrollbarSpacing), this.viewRect.sizeDelta.y);
			}
		}

		/// <summary>
		///   <para>Called by the layout system.</para>
		/// </summary>
		public virtual void SetLayoutVertical()
		{
			this.UpdateScrollbarLayout();
			this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
			this.m_ContentBounds = this.GetBounds();
		}

		private void UpdateScrollbarVisibility()
		{
			if (this.m_VerticalScrollbar && this.m_VerticalScrollbarVisibility != ScrollRect.ScrollbarVisibility.Permanent && this.m_VerticalScrollbar.gameObject.activeSelf != this.vScrollingNeeded)
			{
				this.m_VerticalScrollbar.gameObject.SetActive(this.vScrollingNeeded);
			}
			if (this.m_HorizontalScrollbar && this.m_HorizontalScrollbarVisibility != ScrollRect.ScrollbarVisibility.Permanent && this.m_HorizontalScrollbar.gameObject.activeSelf != this.hScrollingNeeded)
			{
				this.m_HorizontalScrollbar.gameObject.SetActive(this.hScrollingNeeded);
			}
		}

		private void UpdateScrollbarLayout()
		{
			if (this.m_VSliderExpand && this.m_HorizontalScrollbar)
			{
				this.m_Tracker.Add(this, this.m_HorizontalScrollbarRect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.SizeDeltaX);
				this.m_HorizontalScrollbarRect.anchorMin = new Vector2(0f, this.m_HorizontalScrollbarRect.anchorMin.y);
				this.m_HorizontalScrollbarRect.anchorMax = new Vector2(1f, this.m_HorizontalScrollbarRect.anchorMax.y);
				this.m_HorizontalScrollbarRect.anchoredPosition = new Vector2(0f, this.m_HorizontalScrollbarRect.anchoredPosition.y);
				if (this.vScrollingNeeded)
				{
					this.m_HorizontalScrollbarRect.sizeDelta = new Vector2(-(this.m_VSliderWidth + this.m_VerticalScrollbarSpacing), this.m_HorizontalScrollbarRect.sizeDelta.y);
				}
				else
				{
					this.m_HorizontalScrollbarRect.sizeDelta = new Vector2(0f, this.m_HorizontalScrollbarRect.sizeDelta.y);
				}
			}
			if (this.m_HSliderExpand && this.m_VerticalScrollbar)
			{
				this.m_Tracker.Add(this, this.m_VerticalScrollbarRect, DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaY);
				this.m_VerticalScrollbarRect.anchorMin = new Vector2(this.m_VerticalScrollbarRect.anchorMin.x, 0f);
				this.m_VerticalScrollbarRect.anchorMax = new Vector2(this.m_VerticalScrollbarRect.anchorMax.x, 1f);
				this.m_VerticalScrollbarRect.anchoredPosition = new Vector2(this.m_VerticalScrollbarRect.anchoredPosition.x, 0f);
				if (this.hScrollingNeeded)
				{
					this.m_VerticalScrollbarRect.sizeDelta = new Vector2(this.m_VerticalScrollbarRect.sizeDelta.x, -(this.m_HSliderHeight + this.m_HorizontalScrollbarSpacing));
				}
				else
				{
					this.m_VerticalScrollbarRect.sizeDelta = new Vector2(this.m_VerticalScrollbarRect.sizeDelta.x, 0f);
				}
			}
		}

		private void UpdateBounds()
		{
			this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
			this.m_ContentBounds = this.GetBounds();
			if (this.m_Content == null)
			{
				return;
			}
			Vector3 size = this.m_ContentBounds.size;
			Vector3 center = this.m_ContentBounds.center;
			Vector3 vector = this.m_ViewBounds.size - size;
			if (vector.x > 0f)
			{
				center.x -= vector.x * (this.m_Content.pivot.x - 0.5f);
				size.x = this.m_ViewBounds.size.x;
			}
			if (vector.y > 0f)
			{
				center.y -= vector.y * (this.m_Content.pivot.y - 0.5f);
				size.y = this.m_ViewBounds.size.y;
			}
			this.m_ContentBounds.size = size;
			this.m_ContentBounds.center = center;
		}

		private Bounds GetBounds()
		{
			if (this.m_Content == null)
			{
				return default(Bounds);
			}
			Vector3 vector = new Vector3(3.40282347E+38f, 3.40282347E+38f, 3.40282347E+38f);
			Vector3 vector2 = new Vector3(-3.40282347E+38f, -3.40282347E+38f, -3.40282347E+38f);
			Matrix4x4 worldToLocalMatrix = this.viewRect.worldToLocalMatrix;
			this.m_Content.GetWorldCorners(this.m_Corners);
			for (int i = 0; i < 4; i++)
			{
				Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(this.m_Corners[i]);
				vector = Vector3.Min(lhs, vector);
				vector2 = Vector3.Max(lhs, vector2);
			}
			Bounds result = new Bounds(vector, Vector3.zero);
			result.Encapsulate(vector2);
			return result;
		}

		private Vector2 CalculateOffset(Vector2 delta)
		{
			Vector2 zero = Vector2.zero;
			if (this.m_MovementType == ScrollRect.MovementType.Unrestricted)
			{
				return zero;
			}
			Vector2 vector = this.m_ContentBounds.min;
			Vector2 vector2 = this.m_ContentBounds.max;
			if (this.m_Horizontal)
			{
				vector.x += delta.x;
				vector2.x += delta.x;
				if (vector.x > this.m_ViewBounds.min.x)
				{
					zero.x = this.m_ViewBounds.min.x - vector.x;
				}
				else if (vector2.x < this.m_ViewBounds.max.x)
				{
					zero.x = this.m_ViewBounds.max.x - vector2.x;
				}
			}
			if (this.m_Vertical)
			{
				vector.y += delta.y;
				vector2.y += delta.y;
				if (vector2.y < this.m_ViewBounds.max.y)
				{
					zero.y = this.m_ViewBounds.max.y - vector2.y;
				}
				else if (vector.y > this.m_ViewBounds.min.y)
				{
					zero.y = this.m_ViewBounds.min.y - vector.y;
				}
			}
			return zero;
		}

		/// <summary>
		///   <para>Override to alter or add to the code that keeps the appearance of the scroll rect synced with its data.</para>
		/// </summary>
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		/// <summary>
		///   <para>Override to alter or add to the code that caches data to avoid repeated heavy operations.</para>
		/// </summary>
		protected void SetDirtyCaching()
		{
			if (!this.IsActive())
			{
				return;
			}
			CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		protected override void OnValidate()
		{
			this.SetDirtyCaching();
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
