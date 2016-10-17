// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.RemoteNotification
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.iOS
{
  /// <summary>
  ///   <para>RemoteNotification is only available on iPhoneiPadiPod Touch.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class RemoteNotification
  {
    private IntPtr notificationWrapper;

    /// <summary>
    ///   <para>The message displayed in the notification alert. (Read Only)</para>
    /// </summary>
    public string alertBody { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>A boolean value that controls whether the alert action is visible or not. (Read Only)</para>
    /// </summary>
    public bool hasAction { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number to display as the application's icon badge. (Read Only)</para>
    /// </summary>
    public int applicationIconBadgeNumber { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The name of the sound file to play when an alert is displayed. (Read Only)</para>
    /// </summary>
    public string soundName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>A dictionary for passing custom information to the notified application. (Read Only)</para>
    /// </summary>
    public IDictionary userInfo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private RemoteNotification()
    {
    }

    ~RemoteNotification()
    {
      this.Destroy();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Destroy();
  }
}
