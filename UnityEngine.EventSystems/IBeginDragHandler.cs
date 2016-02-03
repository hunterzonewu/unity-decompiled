using System;

namespace UnityEngine.EventSystems
{
	public interface IBeginDragHandler : IEventSystemHandler
	{
		/// <summary>
		///   <para>Called by a BaseInputModule before a drag is started.</para>
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		void OnBeginDrag(PointerEventData eventData);
	}
}
