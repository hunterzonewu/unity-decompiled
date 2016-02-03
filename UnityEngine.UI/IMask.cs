using System;

namespace UnityEngine.UI
{
	[Obsolete("Not supported anymore.", true)]
	public interface IMask
	{
		/// <summary>
		///   <para>Return the RectTransform associated with this mask.</para>
		/// </summary>
		RectTransform rectTransform
		{
			get;
		}

		/// <summary>
		///   <para>Is the mask enabled.</para>
		/// </summary>
		bool Enabled();
	}
}
