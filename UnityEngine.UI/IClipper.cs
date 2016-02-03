using System;

namespace UnityEngine.UI
{
	public interface IClipper
	{
		/// <summary>
		///   <para>Called after layout and before Graphic update of the Canvas update loop.</para>
		/// </summary>
		void PerformClipping();
	}
}
