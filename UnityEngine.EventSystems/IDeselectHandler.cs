using System;

namespace UnityEngine.EventSystems
{
	public interface IDeselectHandler : IEventSystemHandler
	{
		/// <summary>
		///   <para>Called by the EventSystem when a new object is being selected.</para>
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void OnDeselect(BaseEventData eventData);
	}
}
