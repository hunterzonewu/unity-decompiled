using System;

namespace UnityEngine.UI
{
	public interface IMaskable
	{
		/// <summary>
		///   <para>Recalculate masking for this element and all children elements.</para>
		/// </summary>
		void RecalculateMasking();
	}
}
