using System;

namespace UnityEngine.EventSystems
{
	public interface IUpdateSelectedHandler : IEventSystemHandler
	{
		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void OnUpdateSelected(BaseEventData eventData);
	}
}
