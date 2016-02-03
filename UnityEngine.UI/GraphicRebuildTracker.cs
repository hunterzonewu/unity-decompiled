using System;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>EditorOnly class for tracking all Graphics.</para>
	/// </summary>
	public static class GraphicRebuildTracker
	{
		private static IndexedSet<Graphic> m_Tracked = new IndexedSet<Graphic>();

		private static bool s_Initialized;

		/// <summary>
		///   <para>Track a Graphic.</para>
		/// </summary>
		/// <param name="g"></param>
		public static void TrackGraphic(Graphic g)
		{
			if (!GraphicRebuildTracker.s_Initialized)
			{
				CanvasRenderer.onRequestRebuild += new CanvasRenderer.OnRequestRebuild(GraphicRebuildTracker.OnRebuildRequested);
				GraphicRebuildTracker.s_Initialized = true;
			}
			GraphicRebuildTracker.m_Tracked.AddUnique(g);
		}

		/// <summary>
		///   <para>Untrack a Graphic.</para>
		/// </summary>
		/// <param name="g"></param>
		public static void UnTrackGraphic(Graphic g)
		{
			GraphicRebuildTracker.m_Tracked.Remove(g);
		}

		private static void OnRebuildRequested()
		{
			StencilMaterial.ClearAll();
			for (int i = 0; i < GraphicRebuildTracker.m_Tracked.Count; i++)
			{
				GraphicRebuildTracker.m_Tracked[i].OnRebuildRequested();
			}
		}
	}
}
