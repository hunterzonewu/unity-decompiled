// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.SpriteState
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Structure to store the state of a sprite transition on a Selectable.</para>
  /// </summary>
  [Serializable]
  public struct SpriteState : IEquatable<SpriteState>
  {
    [SerializeField]
    [FormerlySerializedAs("highlightedSprite")]
    [FormerlySerializedAs("m_SelectedSprite")]
    private Sprite m_HighlightedSprite;
    [SerializeField]
    [FormerlySerializedAs("pressedSprite")]
    private Sprite m_PressedSprite;
    [SerializeField]
    [FormerlySerializedAs("disabledSprite")]
    private Sprite m_DisabledSprite;

    /// <summary>
    ///   <para>Highlighted sprite.</para>
    /// </summary>
    public Sprite highlightedSprite
    {
      get
      {
        return this.m_HighlightedSprite;
      }
      set
      {
        this.m_HighlightedSprite = value;
      }
    }

    /// <summary>
    ///   <para>Pressed sprite.</para>
    /// </summary>
    public Sprite pressedSprite
    {
      get
      {
        return this.m_PressedSprite;
      }
      set
      {
        this.m_PressedSprite = value;
      }
    }

    /// <summary>
    ///   <para>Disabled sprite.</para>
    /// </summary>
    public Sprite disabledSprite
    {
      get
      {
        return this.m_DisabledSprite;
      }
      set
      {
        this.m_DisabledSprite = value;
      }
    }

    public bool Equals(SpriteState other)
    {
      if ((UnityEngine.Object) this.highlightedSprite == (UnityEngine.Object) other.highlightedSprite && (UnityEngine.Object) this.pressedSprite == (UnityEngine.Object) other.pressedSprite)
        return (UnityEngine.Object) this.disabledSprite == (UnityEngine.Object) other.disabledSprite;
      return false;
    }
  }
}
