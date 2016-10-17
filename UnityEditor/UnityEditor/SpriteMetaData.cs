// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteMetaData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Editor data used in producing a Sprite.</para>
  /// </summary>
  public struct SpriteMetaData
  {
    /// <summary>
    ///   <para>Name of the Sprite.</para>
    /// </summary>
    public string name;
    /// <summary>
    ///   <para>Bounding rectangle of the sprite's graphic within the atlas image.</para>
    /// </summary>
    public Rect rect;
    /// <summary>
    ///   <para>Edge-relative alignment of the sprite graphic.</para>
    /// </summary>
    public int alignment;
    /// <summary>
    ///   <para>Pivot point of the Sprite relative to its bounding rectangle.</para>
    /// </summary>
    public Vector2 pivot;
    /// <summary>
    ///   <para>Edge border size for a sprite (in pixels).</para>
    /// </summary>
    public Vector4 border;
  }
}
