using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>A component that represents a group of Toggles.</para>
	/// </summary>
	[AddComponentMenu("UI/Toggle Group", 32), DisallowMultipleComponent]
	public class ToggleGroup : UIBehaviour
	{
		[SerializeField]
		private bool m_AllowSwitchOff;

		private List<Toggle> m_Toggles = new List<Toggle>();

		/// <summary>
		///   <para>Is it allowed that no toggle is switched on?</para>
		/// </summary>
		public bool allowSwitchOff
		{
			get
			{
				return this.m_AllowSwitchOff;
			}
			set
			{
				this.m_AllowSwitchOff = value;
			}
		}

		protected ToggleGroup()
		{
		}

		private void ValidateToggleIsInGroup(Toggle toggle)
		{
			if (toggle == null || !this.m_Toggles.Contains(toggle))
			{
				throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[]
				{
					toggle,
					this
				}));
			}
		}

		/// <summary>
		///   <para>Notify the group that the given toggle is enabled.</para>
		/// </summary>
		/// <param name="toggle"></param>
		public void NotifyToggleOn(Toggle toggle)
		{
			this.ValidateToggleIsInGroup(toggle);
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				if (!(this.m_Toggles[i] == toggle))
				{
					this.m_Toggles[i].isOn = false;
				}
			}
		}

		/// <summary>
		///   <para>Toggle to unregister.</para>
		/// </summary>
		/// <param name="toggle">Unregister toggle.</param>
		public void UnregisterToggle(Toggle toggle)
		{
			if (this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Remove(toggle);
			}
		}

		/// <summary>
		///   <para>Register a toggle with the group.</para>
		/// </summary>
		/// <param name="toggle">To register.</param>
		public void RegisterToggle(Toggle toggle)
		{
			if (!this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Add(toggle);
			}
		}

		/// <summary>
		///   <para>Are any of the toggles on?</para>
		/// </summary>
		public bool AnyTogglesOn()
		{
			return this.m_Toggles.Find((Toggle x) => x.isOn) != null;
		}

		/// <summary>
		///   <para>Returns the toggles in this group that are active.</para>
		/// </summary>
		/// <returns>
		///   <para>The active toggles in the group.</para>
		/// </returns>
		public IEnumerable<Toggle> ActiveToggles()
		{
			return from x in this.m_Toggles
			where x.isOn
			select x;
		}

		/// <summary>
		///   <para>Switch all toggles off.</para>
		/// </summary>
		public void SetAllTogglesOff()
		{
			bool allowSwitchOff = this.m_AllowSwitchOff;
			this.m_AllowSwitchOff = true;
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				this.m_Toggles[i].isOn = false;
			}
			this.m_AllowSwitchOff = allowSwitchOff;
		}
	}
}
