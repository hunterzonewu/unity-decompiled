// Decompiled with JetBrains decompiler
// Type: UnityEngine.ColorUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A collection of common color functions.</para>
  /// </summary>
  public sealed class ColorUtility
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool DoTryParseHtmlColor(string htmlString, out Color32 color);

    public static bool TryParseHtmlString(string htmlString, out Color color)
    {
      Color32 color1;
      bool htmlColor = ColorUtility.DoTryParseHtmlColor(htmlString, out color1);
      color = (Color) color1;
      return htmlColor;
    }

    /// <summary>
    ///   <para>Returns the color as a hexadecimal string in the format "RRGGBB".</para>
    /// </summary>
    /// <param name="color">The color to be converted.</param>
    /// <returns>
    ///   <para>Hexadecimal string representing the color.</para>
    /// </returns>
    public static string ToHtmlStringRGB(Color color)
    {
      Color32 color32 = (Color32) color;
      return string.Format("{0:X2}{1:X2}{2:X2}", (object) color32.r, (object) color32.g, (object) color32.b);
    }

    /// <summary>
    ///   <para>Returns the color as a hexadecimal string in the format "RRGGBBAA".</para>
    /// </summary>
    /// <param name="color">The color to be converted.</param>
    /// <returns>
    ///   <para>Hexadecimal string representing the color.</para>
    /// </returns>
    public static string ToHtmlStringRGBA(Color color)
    {
      Color32 color32 = (Color32) color;
      return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", (object) color32.r, (object) color32.g, (object) color32.b, (object) color32.a);
    }
  }
}
