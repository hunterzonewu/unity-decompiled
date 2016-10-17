// Decompiled with JetBrains decompiler
// Type: UnityEngine.GUIText
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A text string displayed in a GUI.</para>
  /// </summary>
  public sealed class GUIText : GUIElement
  {
    /// <summary>
    ///   <para>The text to display.</para>
    /// </summary>
    public string text { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The Material to use for rendering.</para>
    /// </summary>
    public Material material { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The pixel offset of the text.</para>
    /// </summary>
    public Vector2 pixelOffset
    {
      get
      {
        Vector2 output;
        this.Internal_GetPixelOffset(out output);
        return output;
      }
      set
      {
        this.Internal_SetPixelOffset(value);
      }
    }

    /// <summary>
    ///   <para>The font used for the text.</para>
    /// </summary>
    public Font font { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The alignment of the text.</para>
    /// </summary>
    public TextAlignment alignment { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The anchor of the text.</para>
    /// </summary>
    public TextAnchor anchor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The line spacing multiplier.</para>
    /// </summary>
    public float lineSpacing { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The tab width multiplier.</para>
    /// </summary>
    public float tabSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The font size to use (for dynamic fonts).</para>
    /// </summary>
    public int fontSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The font style to use (for dynamic fonts).</para>
    /// </summary>
    public FontStyle fontStyle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable HTML-style tags for Text Formatting Markup.</para>
    /// </summary>
    public bool richText { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The color used to render the text.</para>
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

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_GetPixelOffset(out Vector2 output);

    private void Internal_SetPixelOffset(Vector2 p)
    {
      GUIText.INTERNAL_CALL_Internal_SetPixelOffset(this, ref p);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetPixelOffset(GUIText self, ref Vector2 p);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_color(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_color(ref Color value);
  }
}
