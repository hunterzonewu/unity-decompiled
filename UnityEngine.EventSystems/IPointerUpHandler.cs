using System;

namespace UnityEngine.EventSystems
{
	public interface IPointerUpHandler : IEventSystemHandler
	{
		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void OnPointerUp(PointerEventData eventData);
	}
}
