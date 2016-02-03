using System;

namespace UnityEngine.EventSystems
{
	public interface IInitializePotentialDragHandler : IEventSystemHandler
	{
		/// <summary>
		///   <para>Called by a BaseInputModule when a drag has been found but before it is valid to begin the drag.</para>
		/// </summary>
		/// <param name="eventData"></param>
		void OnInitializePotentialDrag(PointerEventData eventData);
	}
}
