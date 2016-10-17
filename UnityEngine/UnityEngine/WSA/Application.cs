// Decompiled with JetBrains decompiler
// Type: UnityEngine.WSA.Application
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.WSA
{
  /// <summary>
  ///   <para>Provides essential methods related to Window Store application.</para>
  /// </summary>
  public sealed class Application
  {
    /// <summary>
    ///   <para>Arguments passed to application.</para>
    /// </summary>
    public static string arguments
    {
      get
      {
        return Application.GetAppArguments();
      }
    }

    /// <summary>
    ///   <para>Advertising ID.</para>
    /// </summary>
    public static string advertisingIdentifier
    {
      get
      {
        string advertisingIdentifier = Application.GetAdvertisingIdentifier();
        UnityEngine.Application.InvokeOnAdvertisingIdentifierCallback(advertisingIdentifier, true);
        return advertisingIdentifier;
      }
    }

    public static event WindowSizeChanged windowSizeChanged;

    public static event WindowActivated windowActivated;

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetAdvertisingIdentifier();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetAppArguments();

    internal static void InvokeWindowSizeChangedEvent(int width, int height)
    {
      if (Application.windowSizeChanged == null)
        return;
      Application.windowSizeChanged(width, height);
    }

    internal static void InvokeWindowActivatedEvent(WindowActivationState state)
    {
      if (Application.windowActivated == null)
        return;
      Application.windowActivated(state);
    }

    /// <summary>
    ///   <para>Executes callback item on application thread.</para>
    /// </summary>
    /// <param name="item">Item to execute.</param>
    /// <param name="waitUntilDone">Wait until item is executed.</param>
    public static void InvokeOnAppThread(AppCallbackItem item, bool waitUntilDone)
    {
      item();
    }

    /// <summary>
    ///   <para>Executes callback item on UI thread.</para>
    /// </summary>
    /// <param name="item">Item to execute.</param>
    /// <param name="waitUntilDone">Wait until item is executed.</param>
    public static void InvokeOnUIThread(AppCallbackItem item, bool waitUntilDone)
    {
      item();
    }

    /// <summary>
    ///   <para>[OBSOLETE] Tries to execute callback item on application thread.</para>
    /// </summary>
    /// <param name="item">Item to execute.</param>
    /// <param name="waitUntilDone">Wait until item is executed.</param>
    [Obsolete("TryInvokeOnAppThread is deprecated, use InvokeOnAppThread")]
    public static bool TryInvokeOnAppThread(AppCallbackItem item, bool waitUntilDone)
    {
      item();
      return true;
    }

    /// <summary>
    ///   <para>[OBSOLETE] Tries to execute callback item on UI thread.</para>
    /// </summary>
    /// <param name="item">Item to execute.</param>
    /// <param name="waitUntilDone">Wait until item is executed.</param>
    [Obsolete("TryInvokeOnUIThread is deprecated, use InvokeOnUIThread")]
    public static bool TryInvokeOnUIThread(AppCallbackItem item, bool waitUntilDone)
    {
      item();
      return true;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalTryInvokeOnAppThread(AppCallbackItem item, bool waitUntilDone);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool InternalTryInvokeOnUIThread(AppCallbackItem item, bool waitUntilDone);

    /// <summary>
    ///   <para>Returns true if you're running on application thread.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RunningOnAppThread();

    /// <summary>
    ///   <para>Returns true if you're running on UI thread.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RunningOnUIThread();
  }
}
