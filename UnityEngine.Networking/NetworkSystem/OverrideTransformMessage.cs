// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.OverrideTransformMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  internal class OverrideTransformMessage : MessageBase
  {
    public NetworkInstanceId netId;
    public byte[] payload;
    public bool teleport;
    public int time;

    public override void Deserialize(NetworkReader reader)
    {
      this.netId = reader.ReadNetworkId();
      this.payload = reader.ReadBytesAndSize();
      this.teleport = reader.ReadBoolean();
      this.time = (int) reader.ReadPackedUInt32();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.Write(this.netId);
      writer.WriteBytesFull(this.payload);
      writer.Write(this.teleport);
      writer.WritePackedUInt32((uint) this.time);
    }
  }
}
