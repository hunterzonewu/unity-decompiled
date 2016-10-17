// Decompiled with JetBrains decompiler
// Type: UnityEditor.TrueTypeFontImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>AssetImporter for importing Fonts.</para>
  /// </summary>
  public sealed class TrueTypeFontImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Font size to use for importing the characters.</para>
    /// </summary>
    public int fontSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use this to adjust which characters should be imported.</para>
    /// </summary>
    public FontTextureCase fontTextureCase { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("FontRenderModes are no longer supported.", true)]
    private int fontRenderMode
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>If this is enabled, the actual font will be embedded into the asset for Dynamic fonts.</para>
    /// </summary>
    public bool includeFontData { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("use2xBehaviour is deprecated. Use ascentCalculationMode instead")]
    private bool use2xBehaviour
    {
      get
      {
        return this.ascentCalculationMode == AscentCalculationMode.Legacy2x;
      }
      set
      {
        if (value)
        {
          this.ascentCalculationMode = AscentCalculationMode.Legacy2x;
        }
        else
        {
          if (this.ascentCalculationMode != AscentCalculationMode.Legacy2x)
            return;
          this.ascentCalculationMode = AscentCalculationMode.FaceAscender;
        }
      }
    }

    /// <summary>
    ///   <para>Calculation mode for determining font's ascent.</para>
    /// </summary>
    public AscentCalculationMode ascentCalculationMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>An array of font names, to be used when includeFontData is set to false.</para>
    /// </summary>
    public string[] fontNames { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>References to other fonts to be used looking for fallbacks.</para>
    /// </summary>
    public Font[] fontReferences { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A custom set of characters to be included in the Font Texture.</para>
    /// </summary>
    public string customCharacters { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The internal font name of the TTF file.</para>
    /// </summary>
    public string fontTTFName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("Per-Font styles are no longer supported. Set the style in the rendering component, or import a styled version of the font.", true)]
    private FontStyle style
    {
      get
      {
        return FontStyle.Normal;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Spacing between character images in the generated texture in pixels. This is useful if you want to render text using a shader which samples pixels outside of the character area (like an outline shader).</para>
    /// </summary>
    public int characterSpacing { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Border pixels added to character images for padding. This is useful if you want to render text using a shader which needs to render outside of the character area (like an outline shader).</para>
    /// </summary>
    public int characterPadding { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Font rendering mode to use for this font.</para>
    /// </summary>
    public FontRenderingMode fontRenderingMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal Font[] LookupFallbackFontReferences(string[] _names);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal bool IsFormatSupported();

    /// <summary>
    ///   <para>Create an editable copy of the font asset at path.</para>
    /// </summary>
    /// <param name="path"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public Font GenerateEditableFont(string path);
  }
}
