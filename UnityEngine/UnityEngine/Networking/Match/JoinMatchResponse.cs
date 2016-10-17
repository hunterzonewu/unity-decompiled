// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.JoinMatchResponse
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>JSON response for a JoinMatchRequest. It contains all information necessdary to continue joining a match.</para>
  /// </summary>
  public class JoinMatchResponse : BasicResponse
  {
    /// <summary>
    ///   <para>Network address to connect to in order to join the match.</para>
    /// </summary>
    public string address { get; set; }

    /// <summary>
    ///   <para>Network port to connect to in order to join the match.</para>
    /// </summary>
    public int port { get; set; }

    /// <summary>
    ///   <para>NetworkID of the match.</para>
    /// </summary>
    public NetworkID networkId { get; set; }

    /// <summary>
    ///   <para>JSON encoding for the binary access token this client uses to authenticate its session for future commands.</para>
    /// </summary>
    public string accessTokenString { get; set; }

    /// <summary>
    ///   <para>NodeID for the requesting client in the mach that it is joining.</para>
    /// </summary>
    public NodeID nodeId { get; set; }

    /// <summary>
    ///   <para>If the match is hosted by a relay server.</para>
    /// </summary>
    public bool usingRelay { get; set; }

    /// <summary>
    ///   <para>Provides string description of current class data.</para>
    /// </summary>
    public override string ToString()
    {
      return UnityString.Format("[{0}]-address:{1},port:{2},networkId:0x{3},nodeId:0x{4},usingRelay:{5}", (object) base.ToString(), (object) this.address, (object) this.port, (object) this.networkId.ToString("X"), (object) this.nodeId.ToString("X"), (object) this.usingRelay);
    }

    public override void Parse(object obj)
    {
      base.Parse(obj);
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.address = this.ParseJSONString("address", obj, dictJsonObj);
      this.port = this.ParseJSONInt32("port", obj, dictJsonObj);
      this.networkId = (NetworkID) this.ParseJSONUInt64("networkId", obj, dictJsonObj);
      this.accessTokenString = this.ParseJSONString("accessTokenString", obj, dictJsonObj);
      this.nodeId = (NodeID) this.ParseJSONUInt16("nodeId", obj, dictJsonObj);
      this.usingRelay = this.ParseJSONBool("usingRelay", obj, dictJsonObj);
    }
  }
}
