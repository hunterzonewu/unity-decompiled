// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.ListMatchResponse
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>JSON response for a ListMatchRequest. It contains a list of matches that can be parsed through to describe a page of matches.</para>
  /// </summary>
  public class ListMatchResponse : BasicResponse
  {
    /// <summary>
    ///   <para>List of matches fitting the requested description.</para>
    /// </summary>
    public List<MatchDesc> matches { get; set; }

    /// <summary>
    ///   <para>Constructor for response class.</para>
    /// </summary>
    /// <param name="matches">A list of matches to give to the object. Only used when generating a new response and not used by callers of a ListMatchRequest.</param>
    /// <param name="otherMatches"></param>
    public ListMatchResponse()
    {
    }

    public ListMatchResponse(List<MatchDesc> otherMatches)
    {
      this.matches = otherMatches;
    }

    /// <summary>
    ///   <para>Provides string description of current class data.</para>
    /// </summary>
    public override string ToString()
    {
      return UnityString.Format("[{0}]-matches.Count:{1}", (object) base.ToString(), (object) this.matches.Count);
    }

    public override void Parse(object obj)
    {
      base.Parse(obj);
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.matches = this.ParseJSONList<MatchDesc>("matches", obj, dictJsonObj);
    }
  }
}
