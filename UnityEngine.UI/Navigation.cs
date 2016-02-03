using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Structure storing details related to navigation.</para>
	/// </summary>
	[Serializable]
	public struct Navigation
	{
		/// <summary>
		///   <para>Navigation mode. Used by Selectable.</para>
		/// </summary>
		[Flags]
		public enum Mode
		{
			/// <summary>
			///   <para>No navigation.</para>
			/// </summary>
			None = 0,
			/// <summary>
			///   <para>Horizontal Navigation.</para>
			/// </summary>
			Horizontal = 1,
			/// <summary>
			///   <para>Vertical navigation.</para>
			/// </summary>
			Vertical = 2,
			/// <summary>
			///   <para>Automatic navigation.</para>
			/// </summary>
			Automatic = 3,
			/// <summary>
			///   <para>Explicit navigaion.</para>
			/// </summary>
			Explicit = 4
		}

		[FormerlySerializedAs("mode"), SerializeField]
		private Navigation.Mode m_Mode;

		[FormerlySerializedAs("selectOnUp"), SerializeField]
		private Selectable m_SelectOnUp;

		[FormerlySerializedAs("selectOnDown"), SerializeField]
		private Selectable m_SelectOnDown;

		[FormerlySerializedAs("selectOnLeft"), SerializeField]
		private Selectable m_SelectOnLeft;

		[FormerlySerializedAs("selectOnRight"), SerializeField]
		private Selectable m_SelectOnRight;

		/// <summary>
		///   <para>Navitation mode.</para>
		/// </summary>
		public Navigation.Mode mode
		{
			get
			{
				return this.m_Mode;
			}
			set
			{
				this.m_Mode = value;
			}
		}

		/// <summary>
		///   <para>Selectable to select on up.</para>
		/// </summary>
		public Selectable selectOnUp
		{
			get
			{
				return this.m_SelectOnUp;
			}
			set
			{
				this.m_SelectOnUp = value;
			}
		}

		/// <summary>
		///   <para>Selectable to select on down.</para>
		/// </summary>
		public Selectable selectOnDown
		{
			get
			{
				return this.m_SelectOnDown;
			}
			set
			{
				this.m_SelectOnDown = value;
			}
		}

		/// <summary>
		///   <para>Selectable to select on left.</para>
		/// </summary>
		public Selectable selectOnLeft
		{
			get
			{
				return this.m_SelectOnLeft;
			}
			set
			{
				this.m_SelectOnLeft = value;
			}
		}

		/// <summary>
		///   <para>Selectable to select on right.</para>
		/// </summary>
		public Selectable selectOnRight
		{
			get
			{
				return this.m_SelectOnRight;
			}
			set
			{
				this.m_SelectOnRight = value;
			}
		}

		/// <summary>
		///   <para>Return a Navigation with sensible default values.</para>
		/// </summary>
		public static Navigation defaultNavigation
		{
			get
			{
				return new Navigation
				{
					m_Mode = Navigation.Mode.Automatic
				};
			}
		}
	}
}
