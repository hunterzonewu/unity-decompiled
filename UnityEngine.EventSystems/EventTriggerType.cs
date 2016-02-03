using System;

namespace UnityEngine.EventSystems
{
	/// <summary>
	///   <para>The type of event the TriggerEvent is intercepting.</para>
	/// </summary>
	public enum EventTriggerType
	{
		/// <summary>
		///   <para>Intercepts a IPointerEnterHandler.OnPointerEnter.</para>
		/// </summary>
		PointerEnter,
		/// <summary>
		///   <para>Intercepts a IPointerExitHandler.OnPointerExit.</para>
		/// </summary>
		PointerExit,
		/// <summary>
		///   <para>Intercepts a IPointerDownHandler.OnPointerDown.</para>
		/// </summary>
		PointerDown,
		/// <summary>
		///   <para>Intercepts a IPointerUpHandler.OnPointerUp.</para>
		/// </summary>
		PointerUp,
		/// <summary>
		///   <para>Intercepts a IPointerClickHandler.OnPointerClick.</para>
		/// </summary>
		PointerClick,
		/// <summary>
		///   <para>Intercepts a IDragHandler.OnDrag.</para>
		/// </summary>
		Drag,
		/// <summary>
		///   <para>Intercepts a IDropHandler.OnDrop.</para>
		/// </summary>
		Drop,
		/// <summary>
		///   <para>Intercepts a IScrollHandler.OnScroll.</para>
		/// </summary>
		Scroll,
		/// <summary>
		///   <para>Intercepts a IUpdateSelectedHandler.OnUpdateSelected.</para>
		/// </summary>
		UpdateSelected,
		/// <summary>
		///   <para>Intercepts a ISelectHandler.OnSelect.</para>
		/// </summary>
		Select,
		/// <summary>
		///   <para>Intercepts a IDeselectHandler.OnDeselect.</para>
		/// </summary>
		Deselect,
		/// <summary>
		///   <para>Intercepts a IMoveHandler.OnMove.</para>
		/// </summary>
		Move,
		/// <summary>
		///   <para>Intercepts IInitializePotentialDrag.InitializePotentialDrag.</para>
		/// </summary>
		InitializePotentialDrag,
		/// <summary>
		///   <para>Intercepts IBeginDragHandler.OnBeginDrag.</para>
		/// </summary>
		BeginDrag,
		/// <summary>
		///   <para>Intercepts IEndDragHandler.OnEndDrag.</para>
		/// </summary>
		EndDrag,
		/// <summary>
		///   <para>Intercepts ISubmitHandler.Submit.</para>
		/// </summary>
		Submit,
		/// <summary>
		///   <para>Intercepts ICancelHandler.OnCancel.</para>
		/// </summary>
		Cancel
	}
}
