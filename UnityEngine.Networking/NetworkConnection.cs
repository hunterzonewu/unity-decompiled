// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkConnection
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>High level network connection.</para>
  /// </summary>
  public class NetworkConnection : IDisposable
  {
    private static int s_MaxPacketStats = (int) byte.MaxValue;
    private List<PlayerController> m_PlayerControllers = new List<PlayerController>();
    private NetworkMessage m_NetMsg = new NetworkMessage();
    private HashSet<NetworkIdentity> m_VisList = new HashSet<NetworkIdentity>();
    private NetworkMessage m_MessageInfo = new NetworkMessage();
    /// <summary>
    ///   <para>Transport level host ID for this connection.</para>
    /// </summary>
    public int hostId = -1;
    /// <summary>
    ///   <para>Unique identifier for this connection.</para>
    /// </summary>
    public int connectionId = -1;
    private Dictionary<short, NetworkConnection.PacketStat> m_PacketStats = new Dictionary<short, NetworkConnection.PacketStat>();
    private const int k_MaxMessageLogSize = 150;
    private ChannelBuffer[] m_Channels;
    private NetworkWriter m_Writer;
    private Dictionary<short, NetworkMessageDelegate> m_MessageHandlersDict;
    private NetworkMessageHandlers m_MessageHandlers;
    private HashSet<NetworkInstanceId> m_ClientOwnedObjects;
    /// <summary>
    ///   <para>Flag that tells if the connection has been marked as "ready" by a client calling ClientScene.Ready().</para>
    /// </summary>
    public bool isReady;
    /// <summary>
    ///   <para>The IP address associated with the connection.</para>
    /// </summary>
    public string address;
    /// <summary>
    ///   <para>The last time that a message was received on this connection.</para>
    /// </summary>
    public float lastMessageTime;
    /// <summary>
    ///   <para>Setting this to true will log the contents of network message to the console.</para>
    /// </summary>
    public bool logNetworkMessages;
    private bool m_Disposed;

    internal HashSet<NetworkIdentity> visList
    {
      get
      {
        return this.m_VisList;
      }
    }

    /// <summary>
    ///   <para>The list of players for this connection.</para>
    /// </summary>
    public List<PlayerController> playerControllers
    {
      get
      {
        return this.m_PlayerControllers;
      }
    }

    /// <summary>
    ///   <para>A list of the NetworkIdentity objects owned by this connection.</para>
    /// </summary>
    public HashSet<NetworkInstanceId> clientOwnedObjects
    {
      get
      {
        return this.m_ClientOwnedObjects;
      }
    }

    /// <summary>
    ///   <para>True if the connection is connected to a remote end-point.</para>
    /// </summary>
    public bool isConnected
    {
      get
      {
        return this.hostId != -1;
      }
    }

    internal Dictionary<short, NetworkConnection.PacketStat> packetStats
    {
      get
      {
        return this.m_PacketStats;
      }
    }

    public NetworkConnection()
    {
      this.m_Writer = new NetworkWriter();
    }

    ~NetworkConnection()
    {
      this.Dispose(false);
    }

    /// <summary>
    ///   <para>This inializes the internal data structures of a NetworkConnection object, including channel buffers.</para>
    /// </summary>
    /// <param name="address">The host or IP connected to.</param>
    /// <param name="hostId">The transport hostId for the connection.</param>
    /// <param name="connectionId">The transport connectionId for the connection.</param>
    /// <param name="hostTopology">The topology to be used.</param>
    /// <param name="networkAddress"></param>
    /// <param name="networkHostId"></param>
    /// <param name="networkConnectionId"></param>
    public virtual void Initialize(string networkAddress, int networkHostId, int networkConnectionId, HostTopology hostTopology)
    {
      this.m_Writer = new NetworkWriter();
      this.address = networkAddress;
      this.hostId = networkHostId;
      this.connectionId = networkConnectionId;
      int channelCount = hostTopology.DefaultConfig.ChannelCount;
      int packetSize = (int) hostTopology.DefaultConfig.PacketSize;
      if (hostTopology.DefaultConfig.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4)
        throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
      this.m_Channels = new ChannelBuffer[channelCount];
      for (int index = 0; index < channelCount; ++index)
      {
        ChannelQOS channel = hostTopology.DefaultConfig.Channels[index];
        int bufferSize = packetSize;
        if (channel.QOS == QosType.ReliableFragmented || channel.QOS == QosType.UnreliableFragmented)
          bufferSize = (int) hostTopology.DefaultConfig.FragmentSize * 128;
        this.m_Channels[index] = new ChannelBuffer(this, bufferSize, (byte) index, NetworkConnection.IsReliableQoS(channel.QOS));
      }
    }

    /// <summary>
    ///   <para>Disposes of this connection, releasing channel buffers that it holds.</para>
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.m_Disposed && this.m_Channels != null)
      {
        for (int index = 0; index < this.m_Channels.Length; ++index)
          this.m_Channels[index].Dispose();
      }
      this.m_Channels = (ChannelBuffer[]) null;
      if (this.m_ClientOwnedObjects != null)
      {
        using (HashSet<NetworkInstanceId>.Enumerator enumerator = this.m_ClientOwnedObjects.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject localObject = NetworkServer.FindLocalObject(enumerator.Current);
            if ((UnityEngine.Object) localObject != (UnityEngine.Object) null)
              localObject.GetComponent<NetworkIdentity>().ClearClientOwner();
          }
        }
      }
      this.m_ClientOwnedObjects = (HashSet<NetworkInstanceId>) null;
      this.m_Disposed = true;
    }

    private static bool IsReliableQoS(QosType qos)
    {
      if (qos != QosType.Reliable && qos != QosType.ReliableFragmented && qos != QosType.ReliableSequenced)
        return qos == QosType.ReliableStateUpdate;
      return true;
    }

    /// <summary>
    ///   <para>This sets an option on the network channel.</para>
    /// </summary>
    /// <param name="channelId">The channel the option will be set on.</param>
    /// <param name="option">The option to set.</param>
    /// <param name="value">The value for the option.</param>
    /// <returns>
    ///   <para>True if the option was set.</para>
    /// </returns>
    public bool SetChannelOption(int channelId, ChannelOption option, int value)
    {
      if (this.m_Channels == null || channelId < 0 || channelId >= this.m_Channels.Length)
        return false;
      return this.m_Channels[channelId].SetOption(option, value);
    }

    /// <summary>
    ///   <para>Disconnects this connection.</para>
    /// </summary>
    public void Disconnect()
    {
      this.address = string.Empty;
      this.isReady = false;
      ClientScene.HandleClientDisconnect(this);
      if (this.hostId == -1)
        return;
      byte error;
      NetworkTransport.Disconnect(this.hostId, this.connectionId, out error);
      this.RemoveObservers();
    }

    internal void SetHandlers(NetworkMessageHandlers handlers)
    {
      this.m_MessageHandlers = handlers;
      this.m_MessageHandlersDict = handlers.GetHandlers();
    }

    /// <summary>
    ///   <para>This function checks if there is a message handler registered for the message ID.</para>
    /// </summary>
    /// <param name="msgType">The message ID of the handler to look for.</param>
    /// <returns>
    ///   <para>True if a handler function was found.</para>
    /// </returns>
    public bool CheckHandler(short msgType)
    {
      return this.m_MessageHandlersDict.ContainsKey(msgType);
    }

    /// <summary>
    ///   <para>This function invokes the registered handler function for a message, without any message data.</para>
    /// </summary>
    /// <param name="msgType">The message ID of the handler to invoke.</param>
    /// <returns>
    ///   <para>True if a handler function was found and invoked.</para>
    /// </returns>
    public bool InvokeHandlerNoData(short msgType)
    {
      return this.InvokeHandler(msgType, (NetworkReader) null, 0);
    }

    /// <summary>
    ///   <para>This function invokes the registered handler function for a message.</para>
    /// </summary>
    /// <param name="msgType">The message type of the handler to use.</param>
    /// <param name="reader">The stream to read the contents of the message from.</param>
    /// <param name="channelId">The channel that the message arrived on.</param>
    /// <param name="netMsg">The message object to process.</param>
    /// <returns>
    ///   <para>True if a handler function was found and invoked.</para>
    /// </returns>
    public bool InvokeHandler(short msgType, NetworkReader reader, int channelId)
    {
      if (!this.m_MessageHandlersDict.ContainsKey(msgType))
        return false;
      this.m_MessageInfo.msgType = msgType;
      this.m_MessageInfo.conn = this;
      this.m_MessageInfo.reader = reader;
      this.m_MessageInfo.channelId = channelId;
      NetworkMessageDelegate networkMessageDelegate = this.m_MessageHandlersDict[msgType];
      if (networkMessageDelegate == null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("NetworkConnection InvokeHandler no handler for " + (object) msgType));
        return false;
      }
      networkMessageDelegate(this.m_MessageInfo);
      return true;
    }

    /// <summary>
    ///   <para>This function invokes the registered handler function for a message.</para>
    /// </summary>
    /// <param name="msgType">The message type of the handler to use.</param>
    /// <param name="reader">The stream to read the contents of the message from.</param>
    /// <param name="channelId">The channel that the message arrived on.</param>
    /// <param name="netMsg">The message object to process.</param>
    /// <returns>
    ///   <para>True if a handler function was found and invoked.</para>
    /// </returns>
    public bool InvokeHandler(NetworkMessage netMsg)
    {
      if (!this.m_MessageHandlersDict.ContainsKey(netMsg.msgType))
        return false;
      this.m_MessageHandlersDict[netMsg.msgType](netMsg);
      return true;
    }

    /// <summary>
    ///   <para>This registers a handler function for a message Id.</para>
    /// </summary>
    /// <param name="msgType">The message ID to register.</param>
    /// <param name="handler">The handler function to register.</param>
    public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
    {
      this.m_MessageHandlers.RegisterHandler(msgType, handler);
    }

    /// <summary>
    ///   <para>This removes the handler registered for a message Id.</para>
    /// </summary>
    /// <param name="msgType">The message ID to unregister.</param>
    public void UnregisterHandler(short msgType)
    {
      this.m_MessageHandlers.UnregisterHandler(msgType);
    }

    internal void SetPlayerController(PlayerController player)
    {
      while ((int) player.playerControllerId >= this.m_PlayerControllers.Count)
        this.m_PlayerControllers.Add(new PlayerController());
      this.m_PlayerControllers[(int) player.playerControllerId] = player;
    }

    internal void RemovePlayerController(short playerControllerId)
    {
      for (int count = this.m_PlayerControllers.Count; count >= 0; --count)
      {
        if ((int) playerControllerId == count && (int) playerControllerId == (int) this.m_PlayerControllers[count].playerControllerId)
        {
          this.m_PlayerControllers[count] = new PlayerController();
          return;
        }
      }
      if (!LogFilter.logError)
        return;
      Debug.LogError((object) ("RemovePlayer player at playerControllerId " + (object) playerControllerId + " not found"));
    }

    internal bool GetPlayerController(short playerControllerId, out PlayerController playerController)
    {
      playerController = (PlayerController) null;
      if (this.playerControllers.Count <= 0)
        return false;
      for (int index = 0; index < this.playerControllers.Count; ++index)
      {
        if (this.playerControllers[index].IsValid && (int) this.playerControllers[index].playerControllerId == (int) playerControllerId)
        {
          playerController = this.playerControllers[index];
          return true;
        }
      }
      return false;
    }

    /// <summary>
    ///   <para>This causes the channels of the network connection to flush their data to the transport layer.</para>
    /// </summary>
    public void FlushChannels()
    {
      if (this.m_Channels == null)
        return;
      foreach (ChannelBuffer channel in this.m_Channels)
        channel.CheckInternalBuffer();
    }

    /// <summary>
    ///   <para>The maximum time in seconds that messages are buffered before being sent.</para>
    /// </summary>
    /// <param name="seconds">Time in seconds.</param>
    public void SetMaxDelay(float seconds)
    {
      if (this.m_Channels == null)
        return;
      foreach (ChannelBuffer channel in this.m_Channels)
        channel.maxDelay = seconds;
    }

    /// <summary>
    ///   <para>This sends a network message with a message ID on the connection. This message is sent on channel zero, which by default is the reliable channel.</para>
    /// </summary>
    /// <param name="msgType">The ID of the message to send.</param>
    /// <param name="msg">The message to send.</param>
    /// <returns>
    ///   <para>True if the message was sent.</para>
    /// </returns>
    public virtual bool Send(short msgType, MessageBase msg)
    {
      return this.SendByChannel(msgType, msg, 0);
    }

    /// <summary>
    ///   <para>This sends a network message with a message ID on the connection. This message is sent on channel one, which by default is the unreliable channel.</para>
    /// </summary>
    /// <param name="msgType">The message ID to send.</param>
    /// <param name="msg">The message to send.</param>
    /// <returns>
    ///   <para>True if the message was sent.</para>
    /// </returns>
    public virtual bool SendUnreliable(short msgType, MessageBase msg)
    {
      return this.SendByChannel(msgType, msg, 1);
    }

    /// <summary>
    ///   <para>This sends a network message on the connection using a specific transport layer channel.</para>
    /// </summary>
    /// <param name="msgType">The message ID to send.</param>
    /// <param name="msg">The message to send.</param>
    /// <param name="channelId">The transport layer channel to send on.</param>
    /// <returns>
    ///   <para>True if the message was sent.</para>
    /// </returns>
    public virtual bool SendByChannel(short msgType, MessageBase msg, int channelId)
    {
      this.m_Writer.StartMessage(msgType);
      msg.Serialize(this.m_Writer);
      this.m_Writer.FinishMessage();
      return this.SendWriter(this.m_Writer, channelId);
    }

    /// <summary>
    ///   <para>This sends an array of bytes on the connection.</para>
    /// </summary>
    /// <param name="bytes">The array of data to be sent.</param>
    /// <param name="numBytes">The number of bytes in the array to be sent.</param>
    /// <param name="channelId">The transport channel to send on.</param>
    /// <returns>
    ///   <para>Success if data was sent.</para>
    /// </returns>
    public virtual bool SendBytes(byte[] bytes, int numBytes, int channelId)
    {
      if (this.logNetworkMessages)
        this.LogSend(bytes);
      if (this.CheckChannel(channelId))
        return this.m_Channels[channelId].SendBytes(bytes, numBytes);
      return false;
    }

    /// <summary>
    ///   <para>This sends the contents of a NetworkWriter object on the connection.</para>
    /// </summary>
    /// <param name="writer">A writer object containing data to send.</param>
    /// <param name="channelId">The transport channel to send on.</param>
    /// <returns>
    ///   <para>True if the data was sent.</para>
    /// </returns>
    public virtual bool SendWriter(NetworkWriter writer, int channelId)
    {
      if (this.logNetworkMessages)
        this.LogSend(writer.ToArray());
      if (this.CheckChannel(channelId))
        return this.m_Channels[channelId].SendWriter(writer);
      return false;
    }

    private void LogSend(byte[] bytes)
    {
      NetworkReader networkReader = new NetworkReader(bytes);
      ushort num1 = networkReader.ReadUInt16();
      ushort num2 = networkReader.ReadUInt16();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 4; index < 4 + (int) num1; ++index)
      {
        stringBuilder.AppendFormat("{0:X2}", (object) bytes[index]);
        if (index > 150)
          break;
      }
      Debug.Log((object) ("ConnectionSend con:" + (object) this.connectionId + " bytes:" + (object) num1 + " msgId:" + (object) num2 + " " + (object) stringBuilder));
    }

    private bool CheckChannel(int channelId)
    {
      if (this.m_Channels == null)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) ("Channels not initialized sending on id '" + (object) channelId));
        return false;
      }
      if (channelId >= 0 && channelId < this.m_Channels.Length)
        return true;
      if (LogFilter.logError)
        Debug.LogError((object) ("Invalid channel when sending buffered data, '" + (object) channelId + "'. Current channel count is " + (object) this.m_Channels.Length));
      return false;
    }

    /// <summary>
    ///   <para>Resets the statistics that are returned from NetworkClient.GetConnectionStats().</para>
    /// </summary>
    public void ResetStats()
    {
      for (short key = 0; (int) key < NetworkConnection.s_MaxPacketStats; ++key)
      {
        if (this.m_PacketStats.ContainsKey(key))
        {
          NetworkConnection.PacketStat packetStat = this.m_PacketStats[key];
          packetStat.count = 0;
          packetStat.bytes = 0;
          NetworkTransport.SetPacketStat(0, (int) key, 0, 0);
          NetworkTransport.SetPacketStat(1, (int) key, 0, 0);
        }
      }
    }

    /// <summary>
    ///   <para>This makes the connection process the data contained in the buffer, and call handler functions.</para>
    /// </summary>
    /// <param name="buffer">Data to process.</param>
    /// <param name="receivedSize">Size of the data to process.</param>
    /// <param name="channelId">Channel the data was recieved on.</param>
    protected void HandleBytes(byte[] buffer, int receivedSize, int channelId)
    {
      this.HandleReader(new NetworkReader(buffer), receivedSize, channelId);
    }

    /// <summary>
    ///   <para>This makes the connection process the data contained in the stream, and call handler functions.</para>
    /// </summary>
    /// <param name="reader">Stream that contains data.</param>
    /// <param name="receivedSize">Size of the data.</param>
    /// <param name="channelId">Channel the data was received on.</param>
    protected void HandleReader(NetworkReader reader, int receivedSize, int channelId)
    {
      while ((long) reader.Position < (long) receivedSize)
      {
        ushort num = reader.ReadUInt16();
        short key = reader.ReadInt16();
        byte[] buffer = reader.ReadBytes((int) num);
        NetworkReader networkReader = new NetworkReader(buffer);
        if (this.logNetworkMessages)
        {
          StringBuilder stringBuilder = new StringBuilder();
          for (int index = 0; index < (int) num; ++index)
          {
            stringBuilder.AppendFormat("{0:X2}", (object) buffer[index]);
            if (index > 150)
              break;
          }
          Debug.Log((object) ("ConnectionRecv con:" + (object) this.connectionId + " bytes:" + (object) num + " msgId:" + (object) key + " " + (object) stringBuilder));
        }
        NetworkMessageDelegate networkMessageDelegate = (NetworkMessageDelegate) null;
        if (this.m_MessageHandlersDict.ContainsKey(key))
          networkMessageDelegate = this.m_MessageHandlersDict[key];
        if (networkMessageDelegate != null)
        {
          this.m_NetMsg.msgType = key;
          this.m_NetMsg.reader = networkReader;
          this.m_NetMsg.conn = this;
          this.m_NetMsg.channelId = channelId;
          networkMessageDelegate(this.m_NetMsg);
          this.lastMessageTime = Time.time;
          NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 28, "msg", 1);
          if ((int) key > 47)
            NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 0, key.ToString() + ":" + key.GetType().Name, 1);
          if (this.m_PacketStats.ContainsKey(key))
          {
            NetworkConnection.PacketStat packetStat = this.m_PacketStats[key];
            ++packetStat.count;
            packetStat.bytes += (int) num;
          }
          else
          {
            NetworkConnection.PacketStat packetStat = new NetworkConnection.PacketStat();
            packetStat.msgType = key;
            ++packetStat.count;
            packetStat.bytes += (int) num;
            this.m_PacketStats[key] = packetStat;
          }
        }
        else
        {
          if (!LogFilter.logError)
            break;
          Debug.LogError((object) ("Unknown message ID " + (object) key + " connId:" + (object) this.connectionId));
          break;
        }
      }
    }

    public virtual void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
    {
      numMsgs = 0;
      numBufferedMsgs = 0;
      numBytes = 0;
      lastBufferedPerSecond = 0;
      foreach (ChannelBuffer channel in this.m_Channels)
      {
        numMsgs = numMsgs + channel.numMsgsOut;
        numBufferedMsgs = numBufferedMsgs + channel.numBufferedMsgsOut;
        numBytes = numBytes + channel.numBytesOut;
        lastBufferedPerSecond = lastBufferedPerSecond + channel.lastBufferedPerSecond;
      }
    }

    public virtual void GetStatsIn(out int numMsgs, out int numBytes)
    {
      numMsgs = 0;
      numBytes = 0;
      foreach (ChannelBuffer channel in this.m_Channels)
      {
        numMsgs = numMsgs + channel.numMsgsIn;
        numBytes = numBytes + channel.numBytesIn;
      }
    }

    /// <summary>
    ///   <para>Returns a string representation of the NetworkConnection object state.</para>
    /// </summary>
    public override string ToString()
    {
      return string.Format("hostId: {0} connectionId: {1} isReady: {2} channel count: {3}", (object) this.hostId, (object) this.connectionId, (object) this.isReady, (object) (this.m_Channels == null ? 0 : this.m_Channels.Length));
    }

    internal void AddToVisList(NetworkIdentity uv)
    {
      this.m_VisList.Add(uv);
      NetworkServer.ShowForConnection(uv, this);
    }

    internal void RemoveFromVisList(NetworkIdentity uv, bool isDestroyed)
    {
      this.m_VisList.Remove(uv);
      if (isDestroyed)
        return;
      NetworkServer.HideForConnection(uv, this);
    }

    internal void RemoveObservers()
    {
      using (HashSet<NetworkIdentity>.Enumerator enumerator = this.m_VisList.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.RemoveObserverInternal(this);
      }
      this.m_VisList.Clear();
    }

    /// <summary>
    ///   <para>This virtual function allows custom network connection classes to process data from the network before it is passed to the application.</para>
    /// </summary>
    /// <param name="bytes">The data recieved.</param>
    /// <param name="numBytes">The size of the data recieved.</param>
    /// <param name="channelId">The channel that the data was received on.</param>
    public virtual void TransportRecieve(byte[] bytes, int numBytes, int channelId)
    {
      this.HandleBytes(bytes, numBytes, channelId);
    }

    public virtual bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
    {
      return NetworkTransport.Send(this.hostId, this.connectionId, channelId, bytes, numBytes, out error);
    }

    internal void AddOwnedObject(NetworkIdentity obj)
    {
      if (this.m_ClientOwnedObjects == null)
        this.m_ClientOwnedObjects = new HashSet<NetworkInstanceId>();
      this.m_ClientOwnedObjects.Add(obj.netId);
    }

    internal void RemoveOwnedObject(NetworkIdentity obj)
    {
      if (this.m_ClientOwnedObjects == null)
        return;
      this.m_ClientOwnedObjects.Remove(obj.netId);
    }

    /// <summary>
    ///   <para>Structure used to track the number and size of packets of each packets type.</para>
    /// </summary>
    public class PacketStat
    {
      /// <summary>
      ///   <para>The message type these stats are for.</para>
      /// </summary>
      public short msgType;
      /// <summary>
      ///   <para>The total number of messages of this type.</para>
      /// </summary>
      public int count;
      /// <summary>
      ///   <para>Total bytes of all messages of this type.</para>
      /// </summary>
      public int bytes;

      public override string ToString()
      {
        return MsgType.MsgTypeToString(this.msgType) + ": count=" + (object) this.count + " bytes=" + (object) this.bytes;
      }
    }
  }
}
