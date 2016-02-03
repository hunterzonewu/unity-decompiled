using System;

namespace UnityEngine.EventSystems
{
	public interface IScrollHandler : IEventSystemHandler
	{
		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void OnScroll(PointerEventData eventData);
	}
}
