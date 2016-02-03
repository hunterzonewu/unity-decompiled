using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.EventSystems
{
	/// <summary>
	///   <para>A BaseInputModule for pointer input.</para>
	/// </summary>
	public abstract class PointerInputModule : BaseInputModule
	{
		protected class ButtonState
		{
			private PointerEventData.InputButton m_Button;

			private PointerInputModule.MouseButtonEventData m_EventData;

			public PointerInputModule.MouseButtonEventData eventData
			{
				get
				{
					return this.m_EventData;
				}
				set
				{
					this.m_EventData = value;
				}
			}

			public PointerEventData.InputButton button
			{
				get
				{
					return this.m_Button;
				}
				set
				{
					this.m_Button = value;
				}
			}
		}

		protected class MouseState
		{
			private List<PointerInputModule.ButtonState> m_TrackedButtons = new List<PointerInputModule.ButtonState>();

			public bool AnyPressesThisFrame()
			{
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.PressedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			public bool AnyReleasesThisFrame()
			{
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].eventData.ReleasedThisFrame())
					{
						return true;
					}
				}
				return false;
			}

			public PointerInputModule.ButtonState GetButtonState(PointerEventData.InputButton button)
			{
				PointerInputModule.ButtonState buttonState = null;
				for (int i = 0; i < this.m_TrackedButtons.Count; i++)
				{
					if (this.m_TrackedButtons[i].button == button)
					{
						buttonState = this.m_TrackedButtons[i];
						break;
					}
				}
				if (buttonState == null)
				{
					buttonState = new PointerInputModule.ButtonState
					{
						button = button,
						eventData = new PointerInputModule.MouseButtonEventData()
					};
					this.m_TrackedButtons.Add(buttonState);
				}
				return buttonState;
			}

			public void SetButtonState(PointerEventData.InputButton button, PointerEventData.FramePressState stateForMouseButton, PointerEventData data)
			{
				PointerInputModule.ButtonState buttonState = this.GetButtonState(button);
				buttonState.eventData.buttonState = stateForMouseButton;
				buttonState.eventData.buttonData = data;
			}
		}

		/// <summary>
		///   <para>Information about a mouse button event.</para>
		/// </summary>
		public class MouseButtonEventData
		{
			/// <summary>
			///   <para>The state of the button this frame.</para>
			/// </summary>
			public PointerEventData.FramePressState buttonState;

			/// <summary>
			///   <para>Pointer data associated with the mouse event.</para>
			/// </summary>
			public PointerEventData buttonData;

			/// <summary>
			///   <para>Was the button pressed this frame?</para>
			/// </summary>
			public bool PressedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Pressed || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}

			/// <summary>
			///   <para>Was the button released this frame?</para>
			/// </summary>
			public bool ReleasedThisFrame()
			{
				return this.buttonState == PointerEventData.FramePressState.Released || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
			}
		}

		/// <summary>
		///   <para>Id of the cached left mouse pointer event.</para>
		/// </summary>
		public const int kMouseLeftId = -1;

		/// <summary>
		///   <para>Id of the cached right mouse pointer event.</para>
		/// </summary>
		public const int kMouseRightId = -2;

		/// <summary>
		///   <para>Id of the cached middle mouse pointer event.</para>
		/// </summary>
		public const int kMouseMiddleId = -3;

		/// <summary>
		///   <para>Touch id for when simulating touches on a non touch device.</para>
		/// </summary>
		public const int kFakeTouchesId = -4;

		protected Dictionary<int, PointerEventData> m_PointerData = new Dictionary<int, PointerEventData>();

		private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

		protected bool GetPointerData(int id, out PointerEventData data, bool create)
		{
			if (!this.m_PointerData.TryGetValue(id, out data) && create)
			{
				data = new PointerEventData(base.eventSystem)
				{
					pointerId = id
				};
				this.m_PointerData.Add(id, data);
				return true;
			}
			return false;
		}

		/// <summary>
		///   <para>Remove the PointerEventData from the cache.</para>
		/// </summary>
		/// <param name="data"></param>
		protected void RemovePointerData(PointerEventData data)
		{
			this.m_PointerData.Remove(data.pointerId);
		}

		protected PointerEventData GetTouchPointerEventData(Touch input, out bool pressed, out bool released)
		{
			PointerEventData pointerEventData;
			bool pointerData = this.GetPointerData(input.fingerId, out pointerEventData, true);
			pointerEventData.Reset();
			pressed = (pointerData || input.phase == TouchPhase.Began);
			released = (input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended);
			if (pointerData)
			{
				pointerEventData.position = input.position;
			}
			if (pressed)
			{
				pointerEventData.delta = Vector2.zero;
			}
			else
			{
				pointerEventData.delta = input.position - pointerEventData.position;
			}
			pointerEventData.position = input.position;
			pointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			return pointerEventData;
		}

		/// <summary>
		///   <para>Copy one PointerEventData to another.</para>
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		protected void CopyFromTo(PointerEventData from, PointerEventData to)
		{
			to.position = from.position;
			to.delta = from.delta;
			to.scrollDelta = from.scrollDelta;
			to.pointerCurrentRaycast = from.pointerCurrentRaycast;
			to.pointerEnter = from.pointerEnter;
		}

		/// <summary>
		///   <para>Given a mouse button return the current state for the frame.</para>
		/// </summary>
		/// <param name="buttonId">Mouse Button id.</param>
		protected static PointerEventData.FramePressState StateForMouseButton(int buttonId)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(buttonId);
			bool mouseButtonUp = Input.GetMouseButtonUp(buttonId);
			if (mouseButtonDown && mouseButtonUp)
			{
				return PointerEventData.FramePressState.PressedAndReleased;
			}
			if (mouseButtonDown)
			{
				return PointerEventData.FramePressState.Pressed;
			}
			if (mouseButtonUp)
			{
				return PointerEventData.FramePressState.Released;
			}
			return PointerEventData.FramePressState.NotChanged;
		}

		/// <summary>
		///   <para>Return the current MouseState.</para>
		/// </summary>
		/// <param name="id"></param>
		protected virtual PointerInputModule.MouseState GetMousePointerEventData()
		{
			return this.GetMousePointerEventData(0);
		}

		/// <summary>
		///   <para>Return the current MouseState.</para>
		/// </summary>
		/// <param name="id"></param>
		protected virtual PointerInputModule.MouseState GetMousePointerEventData(int id)
		{
			PointerEventData pointerEventData;
			bool pointerData = this.GetPointerData(-1, out pointerEventData, true);
			pointerEventData.Reset();
			if (pointerData)
			{
				pointerEventData.position = Input.mousePosition;
			}
			Vector2 vector = Input.mousePosition;
			pointerEventData.delta = vector - pointerEventData.position;
			pointerEventData.position = vector;
			pointerEventData.scrollDelta = Input.mouseScrollDelta;
			pointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			PointerEventData pointerEventData2;
			this.GetPointerData(-2, out pointerEventData2, true);
			this.CopyFromTo(pointerEventData, pointerEventData2);
			pointerEventData2.button = PointerEventData.InputButton.Right;
			PointerEventData pointerEventData3;
			this.GetPointerData(-3, out pointerEventData3, true);
			this.CopyFromTo(pointerEventData, pointerEventData3);
			pointerEventData3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, PointerInputModule.StateForMouseButton(0), pointerEventData);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, PointerInputModule.StateForMouseButton(1), pointerEventData2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, PointerInputModule.StateForMouseButton(2), pointerEventData3);
			return this.m_MouseState;
		}

		/// <summary>
		///   <para>Return the last PointerEventData for the given touch / mouse id.</para>
		/// </summary>
		/// <param name="id"></param>
		protected PointerEventData GetLastPointerEventData(int id)
		{
			PointerEventData result;
			this.GetPointerData(id, out result, false);
			return result;
		}

		private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
		{
			return !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
		}

		/// <summary>
		///   <para>Process movement for the current frame with the given pointer event.</para>
		/// </summary>
		/// <param name="pointerEvent"></param>
		protected virtual void ProcessMove(PointerEventData pointerEvent)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			base.HandlePointerExitAndEnter(pointerEvent, gameObject);
		}

		/// <summary>
		///   <para>Process the drag for the current frame with the given pointer event.</para>
		/// </summary>
		/// <param name="pointerEvent"></param>
		protected virtual void ProcessDrag(PointerEventData pointerEvent)
		{
			bool flag = pointerEvent.IsPointerMoving();
			if (flag && pointerEvent.pointerDrag != null && !pointerEvent.dragging && PointerInputModule.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float)base.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
			{
				ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
				pointerEvent.dragging = true;
			}
			if (pointerEvent.dragging && flag && pointerEvent.pointerDrag != null)
			{
				if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
				{
					ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
					pointerEvent.eligibleForClick = false;
					pointerEvent.pointerPress = null;
					pointerEvent.rawPointerPress = null;
				}
				ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
			}
		}

		public override bool IsPointerOverGameObject(int pointerId)
		{
			PointerEventData lastPointerEventData = this.GetLastPointerEventData(pointerId);
			return lastPointerEventData != null && lastPointerEventData.pointerEnter != null;
		}

		/// <summary>
		///   <para>Clear all pointers and deselect any selected objects in the EventSystem.</para>
		/// </summary>
		protected void ClearSelection()
		{
			BaseEventData baseEventData = this.GetBaseEventData();
			foreach (PointerEventData current in this.m_PointerData.Values)
			{
				base.HandlePointerExitAndEnter(current, null);
			}
			this.m_PointerData.Clear();
			base.eventSystem.SetSelectedGameObject(null, baseEventData);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("<b>Pointer Input Module of type: </b>" + base.GetType());
			stringBuilder.AppendLine();
			foreach (KeyValuePair<int, PointerEventData> current in this.m_PointerData)
			{
				if (current.Value != null)
				{
					stringBuilder.AppendLine("<B>Pointer:</b> " + current.Key);
					stringBuilder.AppendLine(current.Value.ToString());
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		///   <para>Deselect the current selected GameObject if the currently pointed-at GameObject is different.</para>
		/// </summary>
		/// <param name="currentOverGo">The GameObject the pointer is currently over.</param>
		/// <param name="pointerEvent">Current event data.</param>
		protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
		{
			GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
			if (eventHandler != base.eventSystem.currentSelectedGameObject)
			{
				base.eventSystem.SetSelectedGameObject(null, pointerEvent);
			}
		}
	}
}
