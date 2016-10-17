// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkSystem.PeerAuthorityMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking.NetworkSystem
{
  /// <summary>
  ///   <para>Information about a change in authority of a non-player in the same network game.</para>
  /// </summary>
  public class PeerAuthorityMessage : MessageBase
  {
    /// <summary>
    ///   <para>The connection Id (on the server) of the peer whose authority is changing for the object.</para>
    /// </summary>
    public int connectionId;
    /// <summary>
    ///   <para>The network id of the object whose authority state changed.</para>
    /// </summary>
    public NetworkInstanceId netId;
    /// <summary>
    ///   <para>The new state of authority for the object referenced by this message.</para>
    /// </summary>
    public bool authorityState;

    public override void Deserialize(NetworkReader reader)
    {
      this.connectionId = (int) reader.ReadPackedUInt32();
      this.netId = reader.ReadNetworkId();
      this.authorityState = reader.ReadBoolean();
    }

    public override void Serialize(NetworkWriter writer)
    {
      writer.WritePackedUInt32((uint) this.connectionId);
      writer.Write(this.netId);
      writer.Write(this.authorityState);
    }
  }
}
