// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.PeerListMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>Internal UNET message for sending information about network peers to clients.</para>
  /// </summary>
  public class PeerListMessage : MessageBase
  {
    /// <summary>
    ///   <para>The list of participants in a networked game.</para>
    /// </summary>
    public PeerInfoMessage[] peers;
    /// <summary>
    ///   <para>The connectionId of this client on the old host.</para>
    /// </summary>
    public int oldServerConnectionId;

    public override void Deserialize(NetworkReader reader)
    {
      this.oldServerConnectionId = (int) reader.ReadPackedUInt32();
      this.peers = new PeerInfoMessage[(int) reader.ReadUInt16()];
      for (int index = 0; index < this.peers.Length; ++index)
      {
        PeerInfoMessage peerInfoMessage = new PeerInfoMessage();
        peerInfoMessage.Deserialize(reader);
        this.peers[index] = peerInfoMessage;
      }
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.WritePackedUInt32((uint) this.oldServerConnectionId);
      writer.Write((ushort) this.peers.Length);
      foreach (PeerInfoMessage peer in this.peers)
        peer.Serialize(writer);
    }
  }
}
