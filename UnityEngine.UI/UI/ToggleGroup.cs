// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ToggleGroup
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A component that represents a group of Toggles.</para>
  /// </summary>
  [DisallowMultipleComponent]
  [AddComponentMenu("UI/Toggle Group", 32)]
  public class ToggleGroup : UIBehaviour
  {
    private List<Toggle> m_Toggles = new List<Toggle>();
    [SerializeField]
    private bool m_AllowSwitchOff;

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
      if ((UnityEngine.Object) toggle == (UnityEngine.Object) null || !this.m_Toggles.Contains(toggle))
        throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[2]{ (object) toggle, (object) this }));
    }

    /// <summary>
    ///   <para>Notify the group that the given toggle is enabled.</para>
    /// </summary>
    /// <param name="toggle"></param>
    public void NotifyToggleOn(Toggle toggle)
    {
      this.ValidateToggleIsInGroup(toggle);
      for (int index = 0; index < this.m_Toggles.Count; ++index)
      {
        if (!((UnityEngine.Object) this.m_Toggles[index] == (UnityEngine.Object) toggle))
          this.m_Toggles[index].isOn = false;
      }
    }

    /// <summary>
    ///   <para>Toggle to unregister.</para>
    /// </summary>
    /// <param name="toggle">Unregister toggle.</param>
    public void UnregisterToggle(Toggle toggle)
    {
      if (!this.m_Toggles.Contains(toggle))
        return;
      this.m_Toggles.Remove(toggle);
    }

    /// <summary>
    ///   <para>Register a toggle with the group.</para>
    /// </summary>
    /// <param name="toggle">To register.</param>
    public void RegisterToggle(Toggle toggle)
    {
      if (this.m_Toggles.Contains(toggle))
        return;
      this.m_Toggles.Add(toggle);
    }

    /// <summary>
    ///   <para>Are any of the toggles on?</para>
    /// </summary>
    public bool AnyTogglesOn()
    {
      return (UnityEngine.Object) this.m_Toggles.Find((Predicate<Toggle>) (x => x.isOn)) != (UnityEngine.Object) null;
    }

    /// <summary>
    ///   <para>Returns the toggles in this group that are active.</para>
    /// </summary>
    /// <returns>
    ///   <para>The active toggles in the group.</para>
    /// </returns>
    public IEnumerable<Toggle> ActiveToggles()
    {
      return this.m_Toggles.Where<Toggle>((Func<Toggle, bool>) (x => x.isOn));
    }

    /// <summary>
    ///   <para>Switch all toggles off.</para>
    /// </summary>
    public void SetAllTogglesOff()
    {
      bool allowSwitchOff = this.m_AllowSwitchOff;
      this.m_AllowSwitchOff = true;
      for (int index = 0; index < this.m_Toggles.Count; ++index)
        this.m_Toggles[index].isOn = false;
      this.m_AllowSwitchOff = allowSwitchOff;
    }
  }
}
