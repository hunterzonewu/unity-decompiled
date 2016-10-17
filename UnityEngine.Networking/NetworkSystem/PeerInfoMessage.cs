// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.PeerInfoMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>Information about another participant in the same network game.</para>
  /// </summary>
  public class PeerInfoMessage : MessageBase
  {
    /// <summary>
    ///   <para>The id of the NetworkConnection associated with the peer.</para>
    /// </summary>
    public int connectionId;
    /// <summary>
    ///   <para>The IP address of the peer.</para>
    /// </summary>
    public string address;
    /// <summary>
    ///   <para>The network port being used by the peer.</para>
    /// </summary>
    public int port;
    /// <summary>
    ///   <para>True if this peer is the host of the network game.</para>
    /// </summary>
    public bool isHost;
    /// <summary>
    ///   <para>True if the peer if the same as the current client.</para>
    /// </summary>
    public bool isYou;
    /// <summary>
    ///   <para>The players for this peer.</para>
    /// </summary>
    public PeerInfoPlayer[] playerIds;

    public override void Deserialize(NetworkReader reader)
    {
      this.connectionId = (int) reader.ReadPackedUInt32();
      this.address = reader.ReadString();
      this.port = (int) reader.ReadPackedUInt32();
      this.isHost = reader.ReadBoolean();
      this.isYou = reader.ReadBoolean();
      uint num = reader.ReadPackedUInt32();
      if (num <= 0U)
        return;
      List<PeerInfoPlayer> peerInfoPlayerList = new List<PeerInfoPlayer>();
      for (uint index = 0; index < num; ++index)
      {
        PeerInfoPlayer peerInfoPlayer;
        peerInfoPlayer.netId = reader.ReadNetworkId();
        peerInfoPlayer.playerControllerId = (short) reader.ReadPackedUInt32();
        peerInfoPlayerList.Add(peerInfoPlayer);
      }
      this.playerIds = peerInfoPlayerList.ToArray();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.WritePackedUInt32((uint) this.connectionId);
      writer.Write(this.address);
      writer.WritePackedUInt32((uint) this.port);
      writer.Write(this.isHost);
      writer.Write(this.isYou);
      if (this.playerIds == null)
      {
        writer.WritePackedUInt32(0U);
      }
      else
      {
        writer.WritePackedUInt32((uint) this.playerIds.Length);
        for (int index = 0; index < this.playerIds.Length; ++index)
        {
          writer.Write(this.playerIds[index].netId);
          writer.WritePackedUInt32((uint) this.playerIds[index].playerControllerId);
        }
      }
    }

    public override string ToString()
    {
      return "PeerInfo conn:" + (object) this.connectionId + " addr:" + this.address + ":" + (object) this.port + " host:" + (object) this.isHost + " isYou:" + (object) this.isYou;
    }
  }
}
