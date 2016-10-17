// Decompiled with JetBrains decompiler
// Type: UnityEngine.ConnectionTesterStatus
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The various test results the connection tester may return with.</para>
  /// </summary>
  public enum ConnectionTesterStatus
  {
    Error = -2,
    Undetermined = -1,
    [Obsolete("No longer returned, use newer connection tester enums instead.")] PrivateIPNoNATPunchthrough = 0,
    [Obsolete("No longer returned, use newer connection tester enums instead.")] PrivateIPHasNATPunchThrough = 1,
    PublicIPIsConnectable = 2,
    PublicIPPortBlocked = 3,
    PublicIPNoServerStarted = 4,
    LimitedNATPunchthroughPortRestricted = 5,
    LimitedNATPunchthroughSymmetric = 6,
    NATpunchthroughFullCone = 7,
    NATpunchthroughAddressRestrictedCone = 8,
  }
}
