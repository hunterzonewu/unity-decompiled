// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.CreateDedicatedMatchRequest
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
  public class CreateDedicatedMatchRequest : Request
  {
    public string name { get; set; }

    public uint size { get; set; }

    public bool advertise { get; set; }

    public string password { get; set; }

    public string publicAddress { get; set; }

    public string privateAddress { get; set; }

    public int eloScore { get; set; }

    public Dictionary<string, long> matchAttributes { get; set; }

    public override bool IsValid()
    {
      if (this.matchAttributes == null)
        return true;
      return this.matchAttributes.Count <= 10;
    }
  }
}
