// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.MatchDesc
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>A member contained in a ListMatchResponse.matches list. Each element describes an individual match.</para>
  /// </summary>
  public class MatchDesc : ResponseBase
  {
    /// <summary>
    ///   <para>NetworkID of the match.</para>
    /// </summary>
    public NetworkID networkId { get; set; }

    /// <summary>
    ///   <para>Name of the match.</para>
    /// </summary>
    public string name { get; set; }

    /// <summary>
    ///         <para>The optional game defined Elo score for the match as a whole. The Elo score is averaged against all clients in a match and that value is used to produce better search results when listing available matches.
    /// If the Elo is provided the result set will be ordered according to the magnitude of the absoloute value of the difference of the a client searching for a match and the network average for all clients in each match. If the Elo score is not provided (and therefore 0 for all matches) the Elo score will not affect the search results.
    /// Each game can calculate this value as they wish according to whatever scale is best for that game.</para>
    ///       </summary>
    public int averageEloScore { get; set; }

    /// <summary>
    ///   <para>Max number of users that may connect to a match.</para>
    /// </summary>
    public int maxSize { get; set; }

    /// <summary>
    ///   <para>Current number of users connected to a match.</para>
    /// </summary>
    public int currentSize { get; set; }

    /// <summary>
    ///   <para>Describes if this match is considered private.</para>
    /// </summary>
    public bool isPrivate { get; set; }

    /// <summary>
    ///         <para>Match attributes describing game specific features for this match. Each attribute is a key/value pair of a string key with a long value. Each match may have up to 10 of these values.
    /// The game is free to use this as desired to assist in finding better match results when clients search for matches to join.</para>
    ///       </summary>
    public Dictionary<string, long> matchAttributes { get; set; }

    /// <summary>
    ///   <para>The NodeID of the host in a matchmaker match.</para>
    /// </summary>
    public NodeID hostNodeId { get; set; }

    /// <summary>
    ///   <para>Direct connection info for network games; This is not required for games utilizing matchmaker.</para>
    /// </summary>
    public List<MatchDirectConnectInfo> directConnectInfos { get; set; }

    /// <summary>
    ///   <para>Provides string description of current class data.</para>
    /// </summary>
    public override string ToString()
    {
      return UnityString.Format("[{0}]-networkId:0x{1},name:{2},averageEloScore:{3},maxSize:{4},currentSize:{5},isPrivate:{6},matchAttributes.Count:{7},directConnectInfos.Count:{8}", (object) base.ToString(), (object) this.networkId.ToString("X"), (object) this.name, (object) this.averageEloScore, (object) this.maxSize, (object) this.currentSize, (object) this.isPrivate, (object) (this.matchAttributes != null ? this.matchAttributes.Count : 0), (object) this.directConnectInfos.Count);
    }

    public override void Parse(object obj)
    {
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.networkId = (NetworkID) this.ParseJSONUInt64("networkId", obj, dictJsonObj);
      this.name = this.ParseJSONString("name", obj, dictJsonObj);
      this.maxSize = this.ParseJSONInt32("maxSize", obj, dictJsonObj);
      this.currentSize = this.ParseJSONInt32("currentSize", obj, dictJsonObj);
      this.isPrivate = this.ParseJSONBool("isPrivate", obj, dictJsonObj);
      this.directConnectInfos = this.ParseJSONList<MatchDirectConnectInfo>("directConnectInfos", obj, dictJsonObj);
    }
  }
}
