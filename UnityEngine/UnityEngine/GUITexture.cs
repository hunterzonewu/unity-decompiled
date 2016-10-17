// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUITexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A texture image used in a 2D GUI.</para>
  /// </summary>
  public sealed class GUITexture : GUIElement
  {
    /// <summary>
    ///   <para>The color of the GUI texture.</para>
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
    ///   <para>The texture used for drawing.</para>
    /// </summary>
    public Texture texture { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Pixel inset used for pixel adjustments for size and position.</para>
    /// </summary>
    public Rect pixelInset
    {
      get
      {
        Rect rect;
        this.INTERNAL_get_pixelInset(out rect);
        return rect;
      }
      set
      {
        this.INTERNAL_set_pixelInset(ref value);
      }
    }

    /// <summary>
    ///   <para>The border defines the number of pixels from the edge that are not affected by scale.</para>
    /// </summary>
    public RectOffset border { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_color(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_color(ref Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_pixelInset(out Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_pixelInset(ref Rect value);
  }
}
