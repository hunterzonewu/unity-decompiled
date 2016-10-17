// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.ReconnectMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>This network message is used when a client reconnect to the new host of a game.</para>
  /// </summary>
  public class ReconnectMessage : MessageBase
  {
    /// <summary>
    ///   <para>This client's connectionId on the old host.</para>
    /// </summary>
    public int oldConnectionId;
    /// <summary>
    ///   <para>The playerControllerId of the player that is rejoining.</para>
    /// </summary>
    public short playerControllerId;
    /// <summary>
    ///   <para>The networkId of this player on the old host.</para>
    /// </summary>
    public NetworkInstanceId netId;
    /// <summary>
    ///   <para>Size of additional data.</para>
    /// </summary>
    public int msgSize;
    /// <summary>
    ///   <para>Additional data.</para>
    /// </summary>
    public byte[] msgData;

    public override void Deserialize(NetworkReader reader)
    {
      this.oldConnectionId = (int) reader.ReadPackedUInt32();
      this.playerControllerId = (short) reader.ReadPackedUInt32();
      this.netId = reader.ReadNetworkId();
      this.msgData = reader.ReadBytesAndSize();
      if (this.msgData == null)
        this.msgSize = 0;
      else
        this.msgSize = this.msgData.Length;
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.WritePackedUInt32((uint) this.oldConnectionId);
      writer.WritePackedUInt32((uint) this.playerControllerId);
      writer.Write(this.netId);
      writer.WriteBytesAndSize(this.msgData, this.msgSize);
    }
  }
}
