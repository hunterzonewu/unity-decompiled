using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.EventSystems
{
	/// <summary>
	///   <para>Event payload associated with pointer (mouse / touch) events.</para>
	/// </summary>
	public class PointerEventData : BaseEventData
	{
		/// <summary>
		///   <para>Input press tracking.</para>
		/// </summary>
		public enum InputButton
		{
			/// <summary>
			///   <para>Left button.</para>
			/// </summary>
			Left,
			/// <summary>
			///   <para>Right button.</para>
			/// </summary>
			Right,
			/// <summary>
			///   <para>Middle button.</para>
			/// </summary>
			Middle
		}

		/// <summary>
		///   <para>The state of a press for the given frame.</para>
		/// </summary>
		public enum FramePressState
		{
			/// <summary>
			///   <para>Button was pressed this frame.</para>
			/// </summary>
			Pressed,
			/// <summary>
			///   <para>Button was released this frame.</para>
			/// </summary>
			Released,
			/// <summary>
			///   <para>Button was pressed and released this frame.</para>
			/// </summary>
			PressedAndReleased,
			/// <summary>
			///   <para>Same as last frame.</para>
			/// </summary>
			NotChanged
		}

		private GameObject m_PointerPress;

		/// <summary>
		///   <para>List of objects in the hover stack.</para>
		/// </summary>
		public List<GameObject> hovered = new List<GameObject>();

		/// <summary>
		///   <para>The object that received 'OnPointerEnter'.</para>
		/// </summary>
		public GameObject pointerEnter
		{
			get;
			set;
		}

		/// <summary>
		///   <para>The GameObject for the last press event.</para>
		/// </summary>
		public GameObject lastPress
		{
			get;
			private set;
		}

		/// <summary>
		///   <para>The object that the press happened on even if it can not handle the press event.</para>
		/// </summary>
		public GameObject rawPointerPress
		{
			get;
			set;
		}

		/// <summary>
		///   <para>The object that is receiving 'OnDrag'.</para>
		/// </summary>
		public GameObject pointerDrag
		{
			get;
			set;
		}

		/// <summary>
		///   <para>RaycastResult associated with the current event.</para>
		/// </summary>
		public RaycastResult pointerCurrentRaycast
		{
			get;
			set;
		}

		/// <summary>
		///   <para>RaycastResult associated with the pointer press.</para>
		/// </summary>
		public RaycastResult pointerPressRaycast
		{
			get;
			set;
		}

		public bool eligibleForClick
		{
			get;
			set;
		}

		/// <summary>
		///   <para>Id of the pointer (touch id).</para>
		/// </summary>
		public int pointerId
		{
			get;
			set;
		}

		/// <summary>
		///   <para>Current pointer position.</para>
		/// </summary>
		public Vector2 position
		{
			get;
			set;
		}

		/// <summary>
		///   <para>Pointer delta since last update.</para>
		/// </summary>
		public Vector2 delta
		{
			get;
			set;
		}

		/// <summary>
		///   <para>Position of the press.</para>
		/// </summary>
		public Vector2 pressPosition
		{
			get;
			set;
		}

		[Obsolete("Use either pointerCurrentRaycast.worldPosition or pointerPressRaycast.worldPosition")]
		public Vector3 worldPosition
		{
			get;
			set;
		}

		[Obsolete("Use either pointerCurrentRaycast.worldNormal or pointerPressRaycast.worldNormal")]
		public Vector3 worldNormal
		{
			get;
			set;
		}

		/// <summary>
		///   <para>The last time a click event was sent.</para>
		/// </summary>
		public float clickTime
		{
			get;
			set;
		}

		/// <summary>
		///   <para>Number of clicks in a row.</para>
		/// </summary>
		public int clickCount
		{
			get;
			set;
		}

		/// <summary>
		///   <para>The amount of scroll since the last update.</para>
		/// </summary>
		public Vector2 scrollDelta
		{
			get;
			set;
		}

		/// <summary>
		///   <para>Should a drag threshold be used?</para>
		/// </summary>
		public bool useDragThreshold
		{
			get;
			set;
		}

		/// <summary>
		///   <para>Is a drag operation currently occuring.</para>
		/// </summary>
		public bool dragging
		{
			get;
			set;
		}

		/// <summary>
		///   <para>The EventSystems.PointerEventData.InputButton for this event.</para>
		/// </summary>
		public PointerEventData.InputButton button
		{
			get;
			set;
		}

		/// <summary>
		///   <para>The camera associated with the last OnPointerEnter event.</para>
		/// </summary>
		public Camera enterEventCamera
		{
			get
			{
				return (!(this.pointerCurrentRaycast.module == null)) ? this.pointerCurrentRaycast.module.eventCamera : null;
			}
		}

		/// <summary>
		///   <para>The camera associated with the last OnPointerPress event.</para>
		/// </summary>
		public Camera pressEventCamera
		{
			get
			{
				return (!(this.pointerPressRaycast.module == null)) ? this.pointerPressRaycast.module.eventCamera : null;
			}
		}

		/// <summary>
		///   <para>The GameObject that received the OnPointerDown.</para>
		/// </summary>
		public GameObject pointerPress
		{
			get
			{
				return this.m_PointerPress;
			}
			set
			{
				if (this.m_PointerPress == value)
				{
					return;
				}
				this.lastPress = this.m_PointerPress;
				this.m_PointerPress = value;
			}
		}

		public PointerEventData(EventSystem eventSystem) : base(eventSystem)
		{
			this.eligibleForClick = false;
			this.pointerId = -1;
			this.position = Vector2.zero;
			this.delta = Vector2.zero;
			this.pressPosition = Vector2.zero;
			this.clickTime = 0f;
			this.clickCount = 0;
			this.scrollDelta = Vector2.zero;
			this.useDragThreshold = true;
			this.dragging = false;
			this.button = PointerEventData.InputButton.Left;
		}

		/// <summary>
		///   <para>Is the pointer moving.</para>
		/// </summary>
		/// <returns>
		///   <para>Moving.</para>
		/// </returns>
		public bool IsPointerMoving()
		{
			return this.delta.sqrMagnitude > 0f;
		}

		/// <summary>
		///   <para>Is scroll being used on the input device.</para>
		/// </summary>
		/// <returns>
		///   <para>Scrolling.</para>
		/// </returns>
		public bool IsScrolling()
		{
			return this.scrollDelta.sqrMagnitude > 0f;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Position</b>: " + this.position);
			stringBuilder.AppendLine("<b>delta</b>: " + this.delta);
			stringBuilder.AppendLine("<b>eligibleForClick</b>: " + this.eligibleForClick);
			stringBuilder.AppendLine("<b>pointerEnter</b>: " + this.pointerEnter);
			stringBuilder.AppendLine("<b>pointerPress</b>: " + this.pointerPress);
			stringBuilder.AppendLine("<b>lastPointerPress</b>: " + this.lastPress);
			stringBuilder.AppendLine("<b>pointerDrag</b>: " + this.pointerDrag);
			stringBuilder.AppendLine("<b>Use Drag Threshold</b>: " + this.useDragThreshold);
			stringBuilder.AppendLine("<b>Current Rayast:</b>");
			stringBuilder.AppendLine(this.pointerCurrentRaycast.ToString());
			stringBuilder.AppendLine("<b>Press Rayast:</b>");
			stringBuilder.AppendLine(this.pointerPressRaycast.ToString());
			return stringBuilder.ToString();
		}
	}
}
