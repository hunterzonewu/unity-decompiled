// Decompiled with JetBrains decompiler
// Type: UnityEngine.RuntimePlatform
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.ComponentModel;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The platform application is running. Returned by Application.platform.</para>
  /// </summary>
  public enum RuntimePlatform
  {
    OSXEditor = 0,
    OSXPlayer = 1,
    WindowsPlayer = 2,
    OSXWebPlayer = 3,
    OSXDashboardPlayer = 4,
    WindowsWebPlayer = 5,
    WindowsEditor = 7,
    IPhonePlayer = 8,
    PS3 = 9,
    XBOX360 = 10,
    Android = 11,
    [Obsolete("NaCl export is no longer supported in Unity 5.0+.")] NaCl = 12,
    LinuxPlayer = 13,
    [Obsolete("FlashPlayer export is no longer supported in Unity 5.0+.")] FlashPlayer = 15,
    WebGLPlayer = 17,
    [Obsolete("Use WSAPlayerX86 instead")] MetroPlayerX86 = 18,
    WSAPlayerX86 = 18,
    [Obsolete("Use WSAPlayerX64 instead")] MetroPlayerX64 = 19,
    WSAPlayerX64 = 19,
    [Obsolete("Use WSAPlayerARM instead")] MetroPlayerARM = 20,
    WSAPlayerARM = 20,
    WP8Player = 21,
    [Obsolete("BB10Player has been deprecated. Use BlackBerryPlayer instead (UnityUpgradable) -> BlackBerryPlayer", true), EditorBrowsable(EditorBrowsableState.Never)] BB10Player = 22,
    BlackBerryPlayer = 22,
    TizenPlayer = 23,
    PSP2 = 24,
    PS4 = 25,
    PSM = 26,
    XboxOne = 27,
    SamsungTVPlayer = 28,
    WiiU = 30,
    tvOS = 31,
  }
}
