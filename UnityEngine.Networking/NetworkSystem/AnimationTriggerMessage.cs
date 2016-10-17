// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.AnimationTriggerMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  internal class AnimationTriggerMessage : MessageBase
  {
    public NetworkInstanceId netId;
    public int hash;

    public override void Deserialize(NetworkReader reader)
    {
      this.netId = reader.ReadNetworkId();
      this.hash = (int) reader.ReadPackedUInt32();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.Write(this.netId);
      writer.WritePackedUInt32((uint) this.hash);
    }
  }
}
