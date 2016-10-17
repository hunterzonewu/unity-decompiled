// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.DropConnectionRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>JSON object to request a UNET match drop a client.</para>
  /// </summary>
  public class DropConnectionRequest : Request
  {
    /// <summary>
    ///   <para>NetworkID of the match the client to drop is in.</para>
    /// </summary>
    public NetworkID networkId { get; set; }

    /// <summary>
    ///   <para>NodeID of the connection to drop.</para>
    /// </summary>
    public NodeID nodeId { get; set; }

    /// <summary>
    ///   <para>Provides string description of current class data.</para>
    /// </summary>
    public override string ToString()
    {
      return UnityString.Format("[{0}]-networkId:0x{1},nodeId:0x{2}", (object) base.ToString(), (object) this.networkId.ToString("X"), (object) this.nodeId.ToString("X"));
    }

    /// <summary>
    ///   <para>Accessor to verify if the contained data is a valid request with respect to initialized variables and accepted parameters.</para>
    /// </summary>
    public override bool IsValid()
    {
      if (base.IsValid() && this.networkId != NetworkID.Invalid)
        return this.nodeId != NodeID.Invalid;
      return false;
    }
  }
}
