// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Navigation
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Structure storing details related to navigation.</para>
  /// </summary>
  [Serializable]
  public struct Navigation : IEquatable<Navigation>
  {
    [SerializeField]
    [FormerlySerializedAs("mode")]
    private Navigation.Mode m_Mode;
    [FormerlySerializedAs("selectOnUp")]
    [SerializeField]
    private Selectable m_SelectOnUp;
    [SerializeField]
    [FormerlySerializedAs("selectOnDown")]
    private Selectable m_SelectOnDown;
    [SerializeField]
    [FormerlySerializedAs("selectOnLeft")]
    private Selectable m_SelectOnLeft;
    [FormerlySerializedAs("selectOnRight")]
    [SerializeField]
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
        return new Navigation() { m_Mode = Navigation.Mode.Automatic };
      }
    }

    public bool Equals(Navigation other)
    {
      if (this.mode == other.mode && (UnityEngine.Object) this.selectOnUp == (UnityEngine.Object) other.selectOnUp && ((UnityEngine.Object) this.selectOnDown == (UnityEngine.Object) other.selectOnDown && (UnityEngine.Object) this.selectOnLeft == (UnityEngine.Object) other.selectOnLeft))
        return (UnityEngine.Object) this.selectOnRight == (UnityEngine.Object) other.selectOnRight;
      return false;
    }

    /// <summary>
    ///   <para>Navigation mode. Used by Selectable.</para>
    /// </summary>
    [Flags]
    public enum Mode
    {
      None = 0,
      Horizontal = 1,
      Vertical = 2,
      Automatic = Vertical | Horizontal,
      Explicit = 4,
    }
  }
}
