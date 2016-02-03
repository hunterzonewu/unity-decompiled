using System;

namespace UnityEngine.EventSystems
{
	public interface IPointerEnterHandler : IEventSystemHandler
	{
		/// <summary>
		///   <para></para>
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void OnPointerEnter(PointerEventData eventData);
	}
}
