// Decompiled with JetBrains decompiler
// Type: UnityEngine.SamsungTV
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Interface into SamsungTV specific functionality.</para>
  /// </summary>
  public sealed class SamsungTV
  {
    /// <summary>
    ///   <para>The type of input the remote's touch pad produces.</para>
    /// </summary>
    public static SamsungTV.TouchPadMode touchPadMode
    {
      get
      {
        return SamsungTV.GetTouchPadMode();
      }
      set
      {
        if (!SamsungTV.SetTouchPadMode(value))
          throw new ArgumentException("Fail to set touchPadMode.");
      }
    }

    /// <summary>
    ///   <para>Changes the type of input the gesture camera produces.</para>
    /// </summary>
    public static extern SamsungTV.GestureMode gestureMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns true if there is an air mouse available.</para>
    /// </summary>
    public static extern bool airMouseConnected { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns true if the camera sees a hand.</para>
    /// </summary>
    public static extern bool gestureWorking { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Changes the type of input the gamepad produces.</para>
    /// </summary>
    public static extern SamsungTV.GamePadMode gamePadMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern SamsungTV.TouchPadMode GetTouchPadMode();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool SetTouchPadMode(SamsungTV.TouchPadMode value);

    /// <summary>
    ///   <para>Set the system language that is returned by Application.SystemLanguage.</para>
    /// </summary>
    /// <param name="language"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetSystemLanguage(SystemLanguage language);

    /// <summary>
    ///   <para>Types of input the remote's touchpad can produce.</para>
    /// </summary>
    public enum TouchPadMode
    {
      Dpad,
      Joystick,
      Mouse,
    }

    /// <summary>
    ///   <para>Types of input the gesture camera can produce.</para>
    /// </summary>
    public enum GestureMode
    {
      Off,
      Mouse,
      Joystick,
    }

    /// <summary>
    ///   <para>Types of input the gamepad can produce.</para>
    /// </summary>
    public enum GamePadMode
    {
      Default,
      Mouse,
    }

    /// <summary>
    ///   <para>Access to TV specific information.</para>
    /// </summary>
    public sealed class OpenAPI
    {
      /// <summary>
      ///         <para>The server type. Possible values:
      /// Developing, Development, Invalid, Operating.</para>
      ///       </summary>
      public static extern SamsungTV.OpenAPI.OpenAPIServerType serverType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      /// <summary>
      ///   <para>Get local time on TV.</para>
      /// </summary>
      public static extern string timeOnTV { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      /// <summary>
      ///   <para>Get UID from TV.</para>
      /// </summary>
      public static extern string uid { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      public static extern string dUid { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

      public enum OpenAPIServerType
      {
        Operating,
        Development,
        Developing,
        Invalid,
      }
    }
  }
}
