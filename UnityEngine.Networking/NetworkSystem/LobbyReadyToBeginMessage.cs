// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.LobbyReadyToBeginMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  internal class LobbyReadyToBeginMessage : MessageBase
  {
    public byte slotId;
    public bool readyState;

    public override void Deserialize(NetworkReader reader)
    {
      this.slotId = reader.ReadByte();
      this.readyState = reader.ReadBoolean();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.Write(this.slotId);
      writer.Write(this.readyState);
    }
  }
}
