using System;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Registry class to keep track of all IClippers that exist in the scene.</para>
	/// </summary>
	public class ClipperRegistry
	{
		private static ClipperRegistry s_Instance;

		private readonly IndexedSet<IClipper> m_Clippers = new IndexedSet<IClipper>();

		/// <summary>
		///   <para>Singleton instance.</para>
		/// </summary>
		public static ClipperRegistry instance
		{
			get
			{
				if (ClipperRegistry.s_Instance == null)
				{
					ClipperRegistry.s_Instance = new ClipperRegistry();
				}
				return ClipperRegistry.s_Instance;
			}
		}

		protected ClipperRegistry()
		{
		}

		/// <summary>
		///   <para>Perform the clipping on all registered IClipper.</para>
		/// </summary>
		public void Cull()
		{
			for (int i = 0; i < this.m_Clippers.Count; i++)
			{
				this.m_Clippers[i].PerformClipping();
			}
		}

		/// <summary>
		///   <para>Register an IClipper.</para>
		/// </summary>
		/// <param name="c"></param>
		public static void Register(IClipper c)
		{
			if (c == null)
			{
				return;
			}
			ClipperRegistry.instance.m_Clippers.AddUnique(c);
		}

		/// <summary>
		///   <para>Unregister an IClipper.</para>
		/// </summary>
		/// <param name="c"></param>
		public static void Unregister(IClipper c)
		{
			ClipperRegistry.instance.m_Clippers.Remove(c);
		}
	}
}
