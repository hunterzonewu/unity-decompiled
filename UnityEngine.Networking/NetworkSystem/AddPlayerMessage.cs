// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.AddPlayerMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>This is passed to handler funtions registered for the AddPlayer built-in message.</para>
  /// </summary>
  public class AddPlayerMessage : MessageBase
  {
    /// <summary>
    ///   <para>The playerId of the new player.</para>
    /// </summary>
    public short playerControllerId;
    /// <summary>
    ///   <para>The size of the extra message data included in the AddPlayerMessage.</para>
    /// </summary>
    public int msgSize;
    /// <summary>
    ///   <para>The extra message data included in the AddPlayerMessage.</para>
    /// </summary>
    public byte[] msgData;

    public override void Deserialize(NetworkReader reader)
    {
      this.playerControllerId = (short) reader.ReadUInt16();
      this.msgData = reader.ReadBytesAndSize();
      if (this.msgData == null)
        this.msgSize = 0;
      else
        this.msgSize = this.msgData.Length;
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.Write((ushort) this.playerControllerId);
      writer.WriteBytesAndSize(this.msgData, this.msgSize);
    }
  }
}
