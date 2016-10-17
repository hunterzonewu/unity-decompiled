// Decompiled with JetBrains decompiler
// Type: UnityEngine.NetworkConnectionError
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Possible status messages returned by Network.Connect and in MonoBehaviour.OnFailedToConnect|OnFailedToConnect in case the error was not immediate.</para>
  /// </summary>
  public enum NetworkConnectionError
  {
    InternalDirectConnectFailed = -5,
    EmptyConnectTarget = -4,
    IncorrectParameters = -3,
    CreateSocketOrThreadFailure = -2,
    AlreadyConnectedToAnotherServer = -1,
    NoError = 0,
    ConnectionFailed = 15,
    AlreadyConnectedToServer = 16,
    TooManyConnectedPlayers = 18,
    RSAPublicKeyMismatch = 21,
    ConnectionBanned = 22,
    InvalidPassword = 23,
    NATTargetNotConnected = 69,
    NATTargetConnectionLost = 71,
    NATPunchthroughFailed = 73,
  }
}
