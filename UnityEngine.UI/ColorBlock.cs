using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Structure to store the state of a color transition on a Selectable.</para>
	/// </summary>
	[Serializable]
	public struct ColorBlock
	{
		[FormerlySerializedAs("normalColor"), SerializeField]
		private Color m_NormalColor;

		[FormerlySerializedAs("m_SelectedColor"), FormerlySerializedAs("highlightedColor"), SerializeField]
		private Color m_HighlightedColor;

		[FormerlySerializedAs("pressedColor"), SerializeField]
		private Color m_PressedColor;

		[FormerlySerializedAs("disabledColor"), SerializeField]
		private Color m_DisabledColor;

		[Range(1f, 5f), SerializeField]
		private float m_ColorMultiplier;

		[FormerlySerializedAs("fadeDuration"), SerializeField]
		private float m_FadeDuration;

		/// <summary>
		///   <para>Normal Color.</para>
		/// </summary>
		public Color normalColor
		{
			get
			{
				return this.m_NormalColor;
			}
			set
			{
				this.m_NormalColor = value;
			}
		}

		/// <summary>
		///   <para>Highlighted Color.</para>
		/// </summary>
		public Color highlightedColor
		{
			get
			{
				return this.m_HighlightedColor;
			}
			set
			{
				this.m_HighlightedColor = value;
			}
		}

		/// <summary>
		///   <para>Pressed Color.</para>
		/// </summary>
		public Color pressedColor
		{
			get
			{
				return this.m_PressedColor;
			}
			set
			{
				this.m_PressedColor = value;
			}
		}

		/// <summary>
		///   <para>Disabled Color.</para>
		/// </summary>
		public Color disabledColor
		{
			get
			{
				return this.m_DisabledColor;
			}
			set
			{
				this.m_DisabledColor = value;
			}
		}

		/// <summary>
		///   <para>Multiplier applied to colors (allows brightening greater then base color).</para>
		/// </summary>
		public float colorMultiplier
		{
			get
			{
				return this.m_ColorMultiplier;
			}
			set
			{
				this.m_ColorMultiplier = value;
			}
		}

		/// <summary>
		///   <para>How long a color transition should take.</para>
		/// </summary>
		public float fadeDuration
		{
			get
			{
				return this.m_FadeDuration;
			}
			set
			{
				this.m_FadeDuration = value;
			}
		}

		/// <summary>
		///   <para>Simple getter for the default ColorBlock.</para>
		/// </summary>
		public static ColorBlock defaultColorBlock
		{
			get
			{
				return new ColorBlock
				{
					m_NormalColor = new Color32(255, 255, 255, 255),
					m_HighlightedColor = new Color32(245, 245, 245, 255),
					m_PressedColor = new Color32(200, 200, 200, 255),
					m_DisabledColor = new Color32(200, 200, 200, 128),
					colorMultiplier = 1f,
					fadeDuration = 0.1f
				};
			}
		}
	}
}
