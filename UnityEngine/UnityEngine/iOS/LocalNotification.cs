// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.LocalNotification
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
  ///   <para>iOS.LocalNotification is a wrapper around the UILocalNotification class found in the Apple UIKit framework and is only available on iPhoneiPadiPod Touch.</para>
  /// </summary>
  [RequiredByNativeCode]
  public sealed class LocalNotification
  {
    private static long m_NSReferenceDateTicks = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
    private IntPtr notificationWrapper;

    /// <summary>
    ///   <para>The date and time when the system should deliver the notification.</para>
    /// </summary>
    public DateTime fireDate
    {
      get
      {
        return new DateTime((long) (this.GetFireDate() * 10000000.0) + LocalNotification.m_NSReferenceDateTicks);
      }
      set
      {
        this.SetFireDate((double) (value.ToUniversalTime().Ticks - LocalNotification.m_NSReferenceDateTicks) / 10000000.0);
      }
    }

    /// <summary>
    ///   <para>The time zone of the notification's fire date.</para>
    /// </summary>
    public string timeZone { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The calendar interval at which to reschedule the notification.</para>
    /// </summary>
    public CalendarUnit repeatInterval { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The calendar type (Gregorian, Chinese, etc) to use for rescheduling the notification.</para>
    /// </summary>
    public CalendarIdentifier repeatCalendar { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The message displayed in the notification alert.</para>
    /// </summary>
    public string alertBody { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The title of the action button or slider.</para>
    /// </summary>
    public string alertAction { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A boolean value that controls whether the alert action is visible or not.</para>
    /// </summary>
    public bool hasAction { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Identifies the image used as the launch image when the user taps the action button.</para>
    /// </summary>
    public string alertLaunchImage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number to display as the application's icon badge.</para>
    /// </summary>
    public int applicationIconBadgeNumber { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The name of the sound file to play when an alert is displayed.</para>
    /// </summary>
    public string soundName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The default system sound. (Read Only)</para>
    /// </summary>
    public static extern string defaultSoundName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>A dictionary for passing custom information to the notified application.</para>
    /// </summary>
    public IDictionary userInfo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Creates a new local notification.</para>
    /// </summary>
    public LocalNotification()
    {
      this.InitWrapper();
    }

    ~LocalNotification()
    {
      this.Destroy();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private double GetFireDate();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void SetFireDate(double dt);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Destroy();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void InitWrapper();
  }
}
