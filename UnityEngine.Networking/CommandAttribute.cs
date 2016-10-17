// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.CommandAttribute
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This is an attribute that can be put on methods of NetworkBehaviour classes to allow them to be invoked on the server by sending a command from a client.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class CommandAttribute : Attribute
  {
    /// <summary>
    ///   <para>The QoS channel to use to send this command on, see Networking.QosType.</para>
    /// </summary>
    public int channel;
  }
}
