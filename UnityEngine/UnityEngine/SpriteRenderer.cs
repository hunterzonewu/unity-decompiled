// Decompiled with JetBrains decompiler
// Type: UnityEngine.SpriteRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Renders a Sprite for 2D graphics.</para>
  /// </summary>
  public sealed class SpriteRenderer : Renderer
  {
    /// <summary>
    ///   <para>The Sprite to render.</para>
    /// </summary>
    public Sprite sprite
    {
      get
      {
        return this.GetSprite_INTERNAL();
      }
      set
      {
        this.SetSprite_INTERNAL(value);
      }
    }

    /// <summary>
    ///   <para>Rendering color for the Sprite graphic.</para>
    /// </summary>
    public Color color
    {
      get
      {
        Color color;
        this.INTERNAL_get_color(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_color(ref value);
      }
    }

    /// <summary>
    ///   <para>Flips the sprite on the X axis.</para>
    /// </summary>
    public bool flipX { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Flips the sprite on the Y axis.</para>
    /// </summary>
    public bool flipY { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Sprite GetSprite_INTERNAL();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetSprite_INTERNAL(Sprite sprite);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_color(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_color(ref Color value);
  }
}
