// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.PeerInfoPlayer
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>A structure used to identify player object on other peers for host migration.</para>
  /// </summary>
  public struct PeerInfoPlayer
  {
    /// <summary>
    ///   <para>The networkId of the player object.</para>
    /// </summary>
    public NetworkInstanceId netId;
    /// <summary>
    ///   <para>The playerControllerId of the player object.</para>
    /// </summary>
    public short playerControllerId;
  }
}
