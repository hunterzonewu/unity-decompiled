// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.MatchDirectConnectInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>Class describing a client in a network match.</para>
  /// </summary>
  public class MatchDirectConnectInfo : ResponseBase
  {
    /// <summary>
    ///   <para>NodeID of the client described in this direct connect info.</para>
    /// </summary>
    public NodeID nodeId { get; set; }

    /// <summary>
    ///   <para>Public address the client described by this class provided.</para>
    /// </summary>
    public string publicAddress { get; set; }

    /// <summary>
    ///   <para>Private address the client described by this class provided.</para>
    /// </summary>
    public string privateAddress { get; set; }

    public override string ToString()
    {
      return UnityString.Format("[{0}]-nodeId:{1},publicAddress:{2},privateAddress:{3}", (object) base.ToString(), (object) this.nodeId, (object) this.publicAddress, (object) this.privateAddress);
    }

    public override void Parse(object obj)
    {
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.nodeId = (NodeID) this.ParseJSONUInt16("nodeId", obj, dictJsonObj);
      this.publicAddress = this.ParseJSONString("publicAddress", obj, dictJsonObj);
      this.privateAddress = this.ParseJSONString("privateAddress", obj, dictJsonObj);
    }
  }
}
