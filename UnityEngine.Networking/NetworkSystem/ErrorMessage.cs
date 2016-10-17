// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.ErrorMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>This is passed to handler functions registered for the SYSTEM_ERROR built-in message.</para>
  /// </summary>
  public class ErrorMessage : MessageBase
  {
    /// <summary>
    ///   <para>The error code.</para>
    /// </summary>
    public int errorCode;

    public override void Deserialize(NetworkReader reader)
    {
      this.errorCode = (int) reader.ReadUInt16();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.Write((ushort) this.errorCode);
    }
  }
}
