// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.VRDeviceType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine.VR
{
  /// <summary>
  ///   <para>Supported VR devices.</para>
  /// </summary>
  public enum VRDeviceType
  {
    None = 0,
    Stereo = 1,
    Split = 2,
    Oculus = 3,
    [Obsolete("Enum member VRDeviceType.Morpheus has been deprecated. Use VRDeviceType.PlayStationVR instead (UnityUpgradable) -> PlayStationVR", true)] Morpheus = 4,
    PlayStationVR = 4,
  }
}
