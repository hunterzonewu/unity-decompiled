// Decompiled with JetBrains decompiler
// Type: UnityEngine.TouchScreenKeyboard
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Interface into the native iPhone, Android, Windows Phone and Windows Store Apps on-screen keyboards - it is not available on other platforms.</para>
  /// </summary>
  public sealed class TouchScreenKeyboard
  {
    [NonSerialized]
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>Is touch screen keyboard supported.</para>
    /// </summary>
    public static bool isSupported
    {
      get
      {
        RuntimePlatform platform = Application.platform;
        switch (platform)
        {
          case RuntimePlatform.MetroPlayerX86:
          case RuntimePlatform.MetroPlayerX64:
          case RuntimePlatform.MetroPlayerARM:
            return false;
          case RuntimePlatform.WP8Player:
          case RuntimePlatform.BB10Player:
          case RuntimePlatform.TizenPlayer:
          case RuntimePlatform.PSM:
          case RuntimePlatform.WiiU:
          case RuntimePlatform.tvOS:
label_2:
            return true;
          default:
            switch (platform - 8)
            {
              case RuntimePlatform.OSXEditor:
              case RuntimePlatform.OSXWebPlayer:
                goto label_2;
              default:
                return false;
            }
        }
      }
    }

    /// <summary>
    ///   <para>Returns the text displayed by the input field of the keyboard.</para>
    /// </summary>
    public string text { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Will text input field above the keyboard be hidden when the keyboard is on screen?</para>
    /// </summary>
    public static extern bool hideInput { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the keyboard visible or sliding into the position on the screen?</para>
    /// </summary>
    public bool active { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Specifies if input process was finished. (Read Only)</para>
    /// </summary>
    public bool done { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Specifies if input process was canceled. (Read Only)</para>
    /// </summary>
    public bool wasCanceled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Specified on which display the software keyboard will appear.</para>
    /// </summary>
    public int targetDisplay { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns portion of the screen which is covered by the keyboard.</para>
    /// </summary>
    public static Rect area
    {
      get
      {
        Rect rect;
        TouchScreenKeyboard.INTERNAL_get_area(out rect);
        return rect;
      }
    }

    /// <summary>
    ///   <para>Returns true whenever any keyboard is completely visible on the screen.</para>
    /// </summary>
    public static extern bool visible { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public TouchScreenKeyboard(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert, string textPlaceholder)
    {
      this.TouchScreenKeyboard_InternalConstructorHelper(ref new TouchScreenKeyboard_InternalConstructorHelperArguments()
      {
        keyboardType = Convert.ToUInt32((object) keyboardType),
        autocorrection = Convert.ToUInt32(autocorrection),
        multiline = Convert.ToUInt32(multiline),
        secure = Convert.ToUInt32(secure),
        alert = Convert.ToUInt32(alert)
      }, text, textPlaceholder);
    }

    ~TouchScreenKeyboard()
    {
      this.Destroy();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Destroy();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void TouchScreenKeyboard_InternalConstructorHelper(ref TouchScreenKeyboard_InternalConstructorHelperArguments arguments, string text, string textPlaceholder);

    /// <summary>
    ///   <para>Opens the native keyboard provided by OS on the screen.</para>
    /// </summary>
    /// <param name="text">Text to edit.</param>
    /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
    /// <param name="autocorrection">Is autocorrection applied?</param>
    /// <param name="multiline">Can more than one line of text be entered?</param>
    /// <param name="secure">Is the text masked (for passwords, etc)?</param>
    /// <param name="alert">Is the keyboard opened in alert mode?</param>
    /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
    [ExcludeFromDocs]
    public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert)
    {
      string empty = string.Empty;
      return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
    }

    /// <summary>
    ///   <para>Opens the native keyboard provided by OS on the screen.</para>
    /// </summary>
    /// <param name="text">Text to edit.</param>
    /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
    /// <param name="autocorrection">Is autocorrection applied?</param>
    /// <param name="multiline">Can more than one line of text be entered?</param>
    /// <param name="secure">Is the text masked (for passwords, etc)?</param>
    /// <param name="alert">Is the keyboard opened in alert mode?</param>
    /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
    [ExcludeFromDocs]
    public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure)
    {
      string empty = string.Empty;
      bool alert = false;
      return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
    }

    /// <summary>
    ///   <para>Opens the native keyboard provided by OS on the screen.</para>
    /// </summary>
    /// <param name="text">Text to edit.</param>
    /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
    /// <param name="autocorrection">Is autocorrection applied?</param>
    /// <param name="multiline">Can more than one line of text be entered?</param>
    /// <param name="secure">Is the text masked (for passwords, etc)?</param>
    /// <param name="alert">Is the keyboard opened in alert mode?</param>
    /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
    [ExcludeFromDocs]
    public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline)
    {
      string empty = string.Empty;
      bool alert = false;
      bool secure = false;
      return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
    }

    /// <summary>
    ///   <para>Opens the native keyboard provided by OS on the screen.</para>
    /// </summary>
    /// <param name="text">Text to edit.</param>
    /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
    /// <param name="autocorrection">Is autocorrection applied?</param>
    /// <param name="multiline">Can more than one line of text be entered?</param>
    /// <param name="secure">Is the text masked (for passwords, etc)?</param>
    /// <param name="alert">Is the keyboard opened in alert mode?</param>
    /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
    [ExcludeFromDocs]
    public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection)
    {
      string empty = string.Empty;
      bool alert = false;
      bool secure = false;
      bool multiline = false;
      return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
    }

    /// <summary>
    ///   <para>Opens the native keyboard provided by OS on the screen.</para>
    /// </summary>
    /// <param name="text">Text to edit.</param>
    /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
    /// <param name="autocorrection">Is autocorrection applied?</param>
    /// <param name="multiline">Can more than one line of text be entered?</param>
    /// <param name="secure">Is the text masked (for passwords, etc)?</param>
    /// <param name="alert">Is the keyboard opened in alert mode?</param>
    /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
    [ExcludeFromDocs]
    public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType)
    {
      string empty = string.Empty;
      bool alert = false;
      bool secure = false;
      bool multiline = false;
      bool autocorrection = true;
      return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
    }

    /// <summary>
    ///   <para>Opens the native keyboard provided by OS on the screen.</para>
    /// </summary>
    /// <param name="text">Text to edit.</param>
    /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
    /// <param name="autocorrection">Is autocorrection applied?</param>
    /// <param name="multiline">Can more than one line of text be entered?</param>
    /// <param name="secure">Is the text masked (for passwords, etc)?</param>
    /// <param name="alert">Is the keyboard opened in alert mode?</param>
    /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
    [ExcludeFromDocs]
    public static TouchScreenKeyboard Open(string text)
    {
      string empty = string.Empty;
      bool alert = false;
      bool secure = false;
      bool multiline = false;
      bool autocorrection = true;
      TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default;
      return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, empty);
    }

    /// <summary>
    ///   <para>Opens the native keyboard provided by OS on the screen.</para>
    /// </summary>
    /// <param name="text">Text to edit.</param>
    /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
    /// <param name="autocorrection">Is autocorrection applied?</param>
    /// <param name="multiline">Can more than one line of text be entered?</param>
    /// <param name="secure">Is the text masked (for passwords, etc)?</param>
    /// <param name="alert">Is the keyboard opened in alert mode?</param>
    /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
    public static TouchScreenKeyboard Open(string text, [DefaultValue("TouchScreenKeyboardType.Default")] TouchScreenKeyboardType keyboardType, [DefaultValue("true")] bool autocorrection, [DefaultValue("false")] bool multiline, [DefaultValue("false")] bool secure, [DefaultValue("false")] bool alert, [DefaultValue("\"\"")] string textPlaceholder)
    {
      return new TouchScreenKeyboard(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_get_area(out Rect value);
  }
}
