// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ClientRpcAttribute
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This is an attribute that can be put on methods of NetworkBehaviour classes to allow them to be invoked on clients from a server.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class ClientRpcAttribute : Attribute
  {
    /// <summary>
    ///   <para>The channel ID which this RPC transmission will use.</para>
    /// </summary>
    public int channel;
  }
}
