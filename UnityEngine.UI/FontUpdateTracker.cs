using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Utility class that is used to help with Text update.</para>
	/// </summary>
	public static class FontUpdateTracker
	{
		private static Dictionary<Font, List<Text>> m_Tracked = new Dictionary<Font, List<Text>>();

		/// <summary>
		///   <para>Register a Text element for receiving texture atlas rebuild calls.</para>
		/// </summary>
		/// <param name="t"></param>
		public static void TrackText(Text t)
		{
			if (t.font == null)
			{
				return;
			}
			List<Text> list;
			FontUpdateTracker.m_Tracked.TryGetValue(t.font, out list);
			if (list == null)
			{
				if (FontUpdateTracker.m_Tracked.Count == 0)
				{
					Font.textureRebuilt += new Action<Font>(FontUpdateTracker.RebuildForFont);
				}
				list = new List<Text>();
				FontUpdateTracker.m_Tracked.Add(t.font, list);
			}
			if (!list.Contains(t))
			{
				list.Add(t);
			}
		}

		private static void RebuildForFont(Font f)
		{
			List<Text> list;
			FontUpdateTracker.m_Tracked.TryGetValue(f, out list);
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].FontTextureChanged();
			}
		}

		/// <summary>
		///   <para>Deregister a Text element from receiving texture atlas rebuild calls.</para>
		/// </summary>
		/// <param name="t"></param>
		public static void UntrackText(Text t)
		{
			if (t.font == null)
			{
				return;
			}
			List<Text> list;
			FontUpdateTracker.m_Tracked.TryGetValue(t.font, out list);
			if (list == null)
			{
				return;
			}
			list.Remove(t);
			if (list.Count == 0)
			{
				FontUpdateTracker.m_Tracked.Remove(t.font);
				if (FontUpdateTracker.m_Tracked.Count == 0)
				{
					Font.textureRebuilt -= new Action<Font>(FontUpdateTracker.RebuildForFont);
				}
			}
		}
	}
}
