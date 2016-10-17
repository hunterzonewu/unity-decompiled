// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildTargetGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Build target group.</para>
  /// </summary>
  public enum BuildTargetGroup
  {
    Unknown = 0,
    Standalone = 1,
    WebPlayer = 2,
    iOS = 4,
    [Obsolete("Use iOS instead (UnityUpgradable) -> iOS", true)] iPhone = 4,
    PS3 = 5,
    XBOX360 = 6,
    Android = 7,
    GLESEmu = 9,
    WebGL = 13,
    [Obsolete("Use WSA instead")] Metro = 14,
    WSA = 14,
    WP8 = 15,
    BlackBerry = 16,
    Tizen = 17,
    PSP2 = 18,
    PS4 = 19,
    PSM = 20,
    XboxOne = 21,
    SamsungTV = 22,
    Nintendo3DS = 23,
    WiiU = 24,
    tvOS = 25,
  }
}
