using System;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Values of 'update' called on a Canvas update.</para>
	/// </summary>
	public enum CanvasUpdate
	{
		/// <summary>
		///   <para>Called before layout.</para>
		/// </summary>
		Prelayout,
		/// <summary>
		///   <para>Called for layout.</para>
		/// </summary>
		Layout,
		/// <summary>
		///   <para>Called after layout.</para>
		/// </summary>
		PostLayout,
		/// <summary>
		///   <para>Called before rendering.</para>
		/// </summary>
		PreRender,
		/// <summary>
		///   <para>Called late, before render.</para>
		/// </summary>
		LatePreRender,
		/// <summary>
		///   <para>Max enum value.</para>
		/// </summary>
		MaxUpdateValue
	}
}
