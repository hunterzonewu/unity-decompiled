// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildTarget
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Target build platform.</para>
  /// </summary>
  public enum BuildTarget
  {
    [Obsolete("Use BlackBerry instead (UnityUpgradable) -> BlackBerry", true)] BB10 = -1,
    [Obsolete("Use WSAPlayer instead (UnityUpgradable) -> WSAPlayer", true)] MetroPlayer = -1,
    [Obsolete("Use iOS instead (UnityUpgradable) -> iOS", true)] iPhone = -1,
    StandaloneOSXUniversal = 2,
    StandaloneOSXIntel = 4,
    StandaloneWindows = 5,
    WebPlayer = 6,
    WebPlayerStreamed = 7,
    iOS = 9,
    PS3 = 10,
    XBOX360 = 11,
    Android = 13,
    StandaloneGLESEmu = 14,
    StandaloneLinux = 17,
    StandaloneWindows64 = 19,
    WebGL = 20,
    WSAPlayer = 21,
    StandaloneLinux64 = 24,
    StandaloneLinuxUniversal = 25,
    WP8Player = 26,
    StandaloneOSXIntel64 = 27,
    BlackBerry = 28,
    Tizen = 29,
    PSP2 = 30,
    PS4 = 31,
    PSM = 32,
    XboxOne = 33,
    SamsungTV = 34,
    Nintendo3DS = 35,
    WiiU = 36,
    tvOS = 37,
  }
}
