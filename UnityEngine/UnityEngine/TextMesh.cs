// Decompiled with JetBrains decompiler
// Type: UnityEngine.TextMesh
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A script interface for the.</para>
  /// </summary>
  public sealed class TextMesh : Component
  {
    /// <summary>
    ///   <para>The text that is displayed.</para>
    /// </summary>
    public string text { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The Font used.</para>
    /// </summary>
    public Font font { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The font size to use (for dynamic fonts).</para>
    /// </summary>
    public int fontSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The font style to use (for dynamic fonts).</para>
    /// </summary>
    public FontStyle fontStyle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How far should the text be offset from the transform.position.z when drawing.</para>
    /// </summary>
    public float offsetZ { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How lines of text are aligned (Left, Right, Center).</para>
    /// </summary>
    public TextAlignment alignment { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Which point of the text shares the position of the Transform.</para>
    /// </summary>
    public TextAnchor anchor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The size of each character (This scales the whole text).</para>
    /// </summary>
    public float characterSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much space will be in-between lines of text.</para>
    /// </summary>
    public float lineSpacing { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much space will be inserted for a tab '\t' character. This is a multiplum of the 'spacebar' character offset.</para>
    /// </summary>
    public float tabSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

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
    private void INTERNAL_get_color(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_color(ref Color value);
  }
}
