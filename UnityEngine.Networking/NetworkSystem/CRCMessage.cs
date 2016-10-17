// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.CRCMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  internal class CRCMessage : MessageBase
  {
    public CRCMessageEntry[] scripts;

    public override void Deserialize(NetworkReader reader)
    {
      this.scripts = new CRCMessageEntry[(int) reader.ReadUInt16()];
      for (int index = 0; index < this.scripts.Length; ++index)
        this.scripts[index] = new CRCMessageEntry()
        {
          name = reader.ReadString(),
          channel = reader.ReadByte()
        };
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.Write((ushort) this.scripts.Length);
      foreach (CRCMessageEntry script in this.scripts)
      {
        writer.Write(script.name);
        writer.Write(script.channel);
      }
    }
  }
}
