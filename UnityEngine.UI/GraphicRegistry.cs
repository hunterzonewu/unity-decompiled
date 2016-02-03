using System;
using System.Collections.Generic;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Registry which maps a Graphic to the canvas it belongs to.</para>
	/// </summary>
	public class GraphicRegistry
	{
		private static GraphicRegistry s_Instance;

		private readonly Dictionary<Canvas, IndexedSet<Graphic>> m_Graphics = new Dictionary<Canvas, IndexedSet<Graphic>>();

		private static readonly List<Graphic> s_EmptyList = new List<Graphic>();

		/// <summary>
		///   <para>Singleton instance.</para>
		/// </summary>
		public static GraphicRegistry instance
		{
			get
			{
				if (GraphicRegistry.s_Instance == null)
				{
					GraphicRegistry.s_Instance = new GraphicRegistry();
				}
				return GraphicRegistry.s_Instance;
			}
		}

		protected GraphicRegistry()
		{
		}

		/// <summary>
		///   <para>Store a link between the given canvas and graphic in the registry.</para>
		/// </summary>
		/// <param name="c">Canvas to register.</param>
		/// <param name="graphic">Graphic to register.</param>
		public static void RegisterGraphicForCanvas(Canvas c, Graphic graphic)
		{
			if (c == null)
			{
				return;
			}
			IndexedSet<Graphic> indexedSet;
			GraphicRegistry.instance.m_Graphics.TryGetValue(c, out indexedSet);
			if (indexedSet != null)
			{
				indexedSet.AddUnique(graphic);
				return;
			}
			indexedSet = new IndexedSet<Graphic>();
			indexedSet.Add(graphic);
			GraphicRegistry.instance.m_Graphics.Add(c, indexedSet);
		}

		/// <summary>
		///   <para>Deregister the given Graphic from a Canvas.</para>
		/// </summary>
		/// <param name="c">Canvas.</param>
		/// <param name="graphic">Graphic to deregister.</param>
		public static void UnregisterGraphicForCanvas(Canvas c, Graphic graphic)
		{
			if (c == null)
			{
				return;
			}
			IndexedSet<Graphic> indexedSet;
			if (GraphicRegistry.instance.m_Graphics.TryGetValue(c, out indexedSet))
			{
				indexedSet.Remove(graphic);
			}
		}

		/// <summary>
		///   <para>Return a list of Graphics that are registered on the Canvas.</para>
		/// </summary>
		/// <param name="canvas">Input canvas.</param>
		/// <returns>
		///   <para>Graphics on the input canvas.</para>
		/// </returns>
		public static IList<Graphic> GetGraphicsForCanvas(Canvas canvas)
		{
			IndexedSet<Graphic> result;
			if (GraphicRegistry.instance.m_Graphics.TryGetValue(canvas, out result))
			{
				return result;
			}
			return GraphicRegistry.s_EmptyList;
		}
	}
}
