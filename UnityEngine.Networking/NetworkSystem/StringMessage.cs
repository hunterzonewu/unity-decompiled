// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.StringMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>This is a utility class for simple network messages that contain only a string.</para>
  /// </summary>
  public class StringMessage : MessageBase
  {
    /// <summary>
    ///   <para>The string that will be serialized.</para>
    /// </summary>
    public string value;

    public StringMessage()
    {
    }

    public StringMessage(string v)
    {
      this.value = v;
    }

    public override void Deserialize(NetworkReader reader)
    {
      this.value = reader.ReadString();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.Write(this.value);
    }
  }
}
