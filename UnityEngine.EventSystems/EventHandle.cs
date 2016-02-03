using System;

namespace UnityEngine.EventSystems
{
	/// <summary>
	///   <para>Enum that tracks event State.</para>
	/// </summary>
	[Flags]
	public enum EventHandle
	{
		/// <summary>
		///   <para>Unused.</para>
		/// </summary>
		Unused = 0,
		/// <summary>
		///   <para>Used.</para>
		/// </summary>
		Used = 1
	}
}
