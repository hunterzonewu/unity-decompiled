using System;

namespace UnityEngine.EventSystems
{
	/// <summary>
	///   <para>Event Data associated with Axis Events (Controller / Keyboard).</para>
	/// </summary>
	public class AxisEventData : BaseEventData
	{
		/// <summary>
		///   <para>Raw input vector associated with this event.</para>
		/// </summary>
		public Vector2 moveVector
		{
			get;
			set;
		}

		/// <summary>
		///   <para>MoveDirection for this event.</para>
		/// </summary>
		public MoveDirection moveDir
		{
			get;
			set;
		}

		public AxisEventData(EventSystem eventSystem) : base(eventSystem)
		{
			this.moveVector = Vector2.zero;
			this.moveDir = MoveDirection.None;
		}
	}
}
