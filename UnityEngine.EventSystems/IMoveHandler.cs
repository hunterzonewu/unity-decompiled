using System;

namespace UnityEngine.EventSystems
{
	public interface IMoveHandler : IEventSystemHandler
	{
		/// <summary>
		///   <para>Called by a BaseInputModule when a move event occurs.</para>
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void OnMove(AxisEventData eventData);
	}
}
