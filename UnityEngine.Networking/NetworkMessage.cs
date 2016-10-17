// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkMessage
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>The details of a network message received by a client or server on a network connection.</para>
  /// </summary>
  public class NetworkMessage
  {
    /// <summary>
    ///   <para>The size of the largest message in bytes that can be sent on a NetworkConnection.</para>
    /// </summary>
    public const int MaxMessageSize = 65535;
    /// <summary>
    ///   <para>The id of the message type of the message.</para>
    /// </summary>
    public short msgType;
    /// <summary>
    ///   <para>The connection the message was recieved on.</para>
    /// </summary>
    public NetworkConnection conn;
    /// <summary>
    ///   <para>A NetworkReader object that contains the contents of the message.</para>
    /// </summary>
    public NetworkReader reader;
    /// <summary>
    ///   <para>The transport layer channel the message was sent on.</para>
    /// </summary>
    public int channelId;

    /// <summary>
    ///   <para>Returns a string with the numeric representation of each byte in the payload.</para>
    /// </summary>
    /// <param name="payload">Network message payload to dump.</param>
    /// <param name="sz">Length of payload in bytes.</param>
    /// <returns>
    ///   <para>Dumped info from payload.</para>
    /// </returns>
    public static string Dump(byte[] payload, int sz)
    {
      string str = "[";
      for (int index = 0; index < sz; ++index)
        str = str + (object) payload[index] + " ";
      return str + "]";
    }

    public TMsg ReadMessage<TMsg>() where TMsg : MessageBase, new()
    {
      TMsg instance = Activator.CreateInstance<TMsg>();
      instance.Deserialize(this.reader);
      return instance;
    }

    public void ReadMessage<TMsg>(TMsg msg) where TMsg : MessageBase
    {
      msg.Deserialize(this.reader);
    }
  }
}
