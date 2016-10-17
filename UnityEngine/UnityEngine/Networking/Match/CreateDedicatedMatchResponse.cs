// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.CreateDedicatedMatchResponse
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match
{
  public class CreateDedicatedMatchResponse : BasicResponse
  {
    public NetworkID networkId { get; set; }

    public NodeID nodeId { get; set; }

    public string address { get; set; }

    public int port { get; set; }

    public string accessTokenString { get; set; }

    public override void Parse(object obj)
    {
      base.Parse(obj);
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        throw new FormatException("While parsing JSON response, found obj is not of type IDictionary<string,object>:" + obj.ToString());
      this.address = this.ParseJSONString("address", obj, dictJsonObj);
      this.port = this.ParseJSONInt32("port", obj, dictJsonObj);
      this.accessTokenString = this.ParseJSONString("accessTokenString", obj, dictJsonObj);
      this.networkId = (NetworkID) this.ParseJSONUInt64("networkId", obj, dictJsonObj);
      this.nodeId = (NodeID) this.ParseJSONUInt16("nodeId", obj, dictJsonObj);
    }
  }
}
