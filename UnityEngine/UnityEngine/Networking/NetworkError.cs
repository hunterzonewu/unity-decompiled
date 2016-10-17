// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkError
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Possible transport layer erors.</para>
  /// </summary>
  public enum NetworkError
  {
    Ok,
    WrongHost,
    WrongConnection,
    WrongChannel,
    NoResources,
    BadMessage,
    Timeout,
    MessageToLong,
    WrongOperation,
    VersionMismatch,
    CRCMismatch,
    DNSFailure,
  }
}
