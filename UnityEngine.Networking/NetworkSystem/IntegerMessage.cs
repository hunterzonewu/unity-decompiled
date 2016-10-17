// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.IntegerMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>A utility class to send simple network messages that only contain an integer.</para>
  /// </summary>
  public class IntegerMessage : MessageBase
  {
    /// <summary>
    ///   <para>The integer value to serialize.</para>
    /// </summary>
    public int value;

    public IntegerMessage()
    {
    }

    public IntegerMessage(int v)
    {
      this.value = v;
    }

    public override void Deserialize(NetworkReader reader)
    {
      this.value = (int) reader.ReadPackedUInt32();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.WritePackedUInt32((uint) this.value);
    }
  }
}
