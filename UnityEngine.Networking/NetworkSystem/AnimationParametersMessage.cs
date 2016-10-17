// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.AnimationParametersMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  internal class AnimationParametersMessage : MessageBase
  {
    public NetworkInstanceId netId;
    public byte[] parameters;

    public override void Deserialize(NetworkReader reader)
    {
      this.netId = reader.ReadNetworkId();
      this.parameters = reader.ReadBytesAndSize();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.Write(this.netId);
      if (this.parameters == null)
        writer.WriteBytesAndSize(this.parameters, 0);
      else
        writer.WriteBytesAndSize(this.parameters, this.parameters.Length);
    }
  }
}
