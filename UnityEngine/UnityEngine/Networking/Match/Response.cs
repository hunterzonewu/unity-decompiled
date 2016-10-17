// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Match.Response
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
  /// <summary>
  ///   <para>Abstract class that contains shared accessors for any response.</para>
  /// </summary>
  public abstract class Response : ResponseBase, IResponse
  {
    /// <summary>
    ///   <para>Bool describing if the request was successful.</para>
    /// </summary>
    public bool success { get; private set; }

    /// <summary>
    ///   <para>Extended string information that is returned when the server encounters an error processing a request.</para>
    /// </summary>
    public string extendedInfo { get; private set; }

    public void SetSuccess()
    {
      this.success = true;
      this.extendedInfo = string.Empty;
    }

    public void SetFailure(string info)
    {
      this.success = false;
      this.extendedInfo = info;
    }

    /// <summary>
    ///   <para>Provides string description of current class data.</para>
    /// </summary>
    public override string ToString()
    {
      return UnityString.Format("[{0}]-success:{1}-extendedInfo:{2}", (object) base.ToString(), (object) this.success, (object) this.extendedInfo);
    }

    public override void Parse(object obj)
    {
      IDictionary<string, object> dictJsonObj = obj as IDictionary<string, object>;
      if (dictJsonObj == null)
        return;
      this.success = this.ParseJSONBool("success", obj, dictJsonObj);
      this.extendedInfo = this.ParseJSONString("extendedInfo", obj, dictJsonObj);
      if (!this.success)
        throw new FormatException("FAILURE Returned from server: " + this.extendedInfo);
    }
  }
}
