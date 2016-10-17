// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkServerSimple
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>The NetworkServerSimple is a basic server class without the "game" related functionality that the NetworkServer class has.</para>
  /// </summary>
  public class NetworkServerSimple
  {
    private int m_ServerHostId = -1;
    private int m_RelaySlotId = -1;
    private System.Type m_NetworkConnectionClass = typeof (NetworkConnection);
    private List<NetworkConnection> m_Connections = new List<NetworkConnection>();
    private NetworkMessageHandlers m_MessageHandlers = new NetworkMessageHandlers();
    private bool m_Initialized;
    private int m_ListenPort;
    private bool m_UseWebSockets;
    private byte[] m_MsgBuffer;
    private NetworkReader m_MsgReader;
    private HostTopology m_HostTopology;
    private ReadOnlyCollection<NetworkConnection> m_ConnectionsReadOnly;

    /// <summary>
    ///   <para>The network port that the server is listening on.</para>
    /// </summary>
    public int listenPort
    {
      get
      {
        return this.m_ListenPort;
      }
      set
      {
        this.m_ListenPort = value;
      }
    }

    /// <summary>
    ///   <para>The transport layer hostId of the server.</para>
    /// </summary>
    public int serverHostId
    {
      get
      {
        return this.m_ServerHostId;
      }
      set
      {
        this.m_ServerHostId = value;
      }
    }

    /// <summary>
    ///   <para>The transport layer host-topology that the server is configured with.</para>
    /// </summary>
    public HostTopology hostTopology
    {
      get
      {
        return this.m_HostTopology;
      }
    }

    /// <summary>
    ///   <para>This causes the server to listen for WebSocket connections instead of regular transport layer connections.</para>
    /// </summary>
    public bool useWebSockets
    {
      get
      {
        return this.m_UseWebSockets;
      }
      set
      {
        this.m_UseWebSockets = value;
      }
    }

    /// <summary>
    ///   <para>A read-only list of the current connections being managed.</para>
    /// </summary>
    public ReadOnlyCollection<NetworkConnection> connections
    {
      get
      {
        return this.m_ConnectionsReadOnly;
      }
    }

    /// <summary>
    ///   <para>The message handler functions that are registered.</para>
    /// </summary>
    public Dictionary<short, NetworkMessageDelegate> handlers
    {
      get
      {
        return this.m_MessageHandlers.GetHandlers();
      }
    }

    /// <summary>
    ///   <para>The internal buffer that the server reads data from the network into. This will contain the most recent data read from the network when OnData() is called.</para>
    /// </summary>
    public byte[] messageBuffer
    {
      get
      {
        return this.m_MsgBuffer;
      }
    }

    /// <summary>
    ///   <para>A NetworkReader object that is bound to the server's messageBuffer.</para>
    /// </summary>
    public NetworkReader messageReader
    {
      get
      {
        return this.m_MsgReader;
      }
    }

    /// <summary>
    ///   <para>The type of class to be created for new network connections from clients.</para>
    /// </summary>
    public System.Type networkConnectionClass
    {
      get
      {
        return this.m_NetworkConnectionClass;
      }
    }

    public NetworkServerSimple()
    {
      this.m_ConnectionsReadOnly = new ReadOnlyCollection<NetworkConnection>((IList<NetworkConnection>) this.m_Connections);
    }

    public void SetNetworkConnectionClass<T>() where T : NetworkConnection
    {
      this.m_NetworkConnectionClass = typeof (T);
    }

    /// <summary>
    ///   <para>Initialization function that is invoked when the server starts listening. This can be overridden to perform custom initialization such as setting the NetworkConnectionClass.</para>
    /// </summary>
    public virtual void Initialize()
    {
      if (this.m_Initialized)
        return;
      this.m_Initialized = true;
      NetworkTransport.Init();
      this.m_MsgBuffer = new byte[(int) ushort.MaxValue];
      this.m_MsgReader = new NetworkReader(this.m_MsgBuffer);
      if (this.m_HostTopology == null)
      {
        ConnectionConfig defaultConfig = new ConnectionConfig();
        int num1 = (int) defaultConfig.AddChannel(QosType.Reliable);
        int num2 = (int) defaultConfig.AddChannel(QosType.Unreliable);
        this.m_HostTopology = new HostTopology(defaultConfig, 8);
      }
      if (!LogFilter.logDebug)
        return;
      Debug.Log((object) "NetworkServerSimple initialize.");
    }

    /// <summary>
    ///   <para>This configures the network transport layer of the server.</para>
    /// </summary>
    /// <param name="config">The transport layer configuration to use.</param>
    /// <param name="maxConnections">Maximum number of network connections to allow.</param>
    /// <param name="topology">The transport layer host topology to use.</param>
    /// <returns>
    ///   <para>True if configured.</para>
    /// </returns>
    public bool Configure(ConnectionConfig config, int maxConnections)
    {
      return this.Configure(new HostTopology(config, maxConnections));
    }

    /// <summary>
    ///   <para>This configures the network transport layer of the server.</para>
    /// </summary>
    /// <param name="config">The transport layer configuration to use.</param>
    /// <param name="maxConnections">Maximum number of network connections to allow.</param>
    /// <param name="topology">The transport layer host topology to use.</param>
    /// <returns>
    ///   <para>True if configured.</para>
    /// </returns>
    public bool Configure(HostTopology topology)
    {
      this.m_HostTopology = topology;
      return true;
    }

    public bool Listen(string ipAddress, int serverListenPort)
    {
      this.Initialize();
      this.m_ListenPort = serverListenPort;
      this.m_ServerHostId = !this.m_UseWebSockets ? NetworkTransport.AddHost(this.m_HostTopology, serverListenPort, ipAddress) : NetworkTransport.AddWebsocketHost(this.m_HostTopology, serverListenPort, ipAddress);
      if (this.m_ServerHostId == -1)
        return false;
      if (LogFilter.logDebug)
        Debug.Log((object) ("NetworkServerSimple listen: " + ipAddress + ":" + (object) this.m_ListenPort));
      return true;
    }

    /// <summary>
    ///   <para>This starts the server listening for connections on the specified port.</para>
    /// </summary>
    /// <param name="serverListenPort">The port to listen on.</param>
    /// <param name="topology">The transport layer host toplogy to configure with.</param>
    /// <returns>
    ///   <para>True if able to listen.</para>
    /// </returns>
    public bool Listen(int serverListenPort)
    {
      return this.Listen(serverListenPort, this.m_HostTopology);
    }

    /// <summary>
    ///   <para>This starts the server listening for connections on the specified port.</para>
    /// </summary>
    /// <param name="serverListenPort">The port to listen on.</param>
    /// <param name="topology">The transport layer host toplogy to configure with.</param>
    /// <returns>
    ///   <para>True if able to listen.</para>
    /// </returns>
    public bool Listen(int serverListenPort, HostTopology topology)
    {
      this.m_HostTopology = topology;
      this.Initialize();
      this.m_ListenPort = serverListenPort;
      this.m_ServerHostId = !this.m_UseWebSockets ? NetworkTransport.AddHost(this.m_HostTopology, serverListenPort) : NetworkTransport.AddWebsocketHost(this.m_HostTopology, serverListenPort);
      if (this.m_ServerHostId == -1)
        return false;
      if (LogFilter.logDebug)
        Debug.Log((object) ("NetworkServerSimple listen " + (object) this.m_ListenPort));
      return true;
    }

    /// <summary>
    ///   <para>Starts a server using a relay server. This is the manual way of using the relay server, as the regular NetworkServer.Connect() will automatically use the relay server if a match exists.</para>
    /// </summary>
    /// <param name="relayIp">Relay server IP Address.</param>
    /// <param name="relayPort">Relay server port.</param>
    /// <param name="netGuid">GUID of the network to create.</param>
    /// <param name="sourceId">This server's sourceId.</param>
    /// <param name="nodeId">The node to join the network with.</param>
    public void ListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
    {
      this.Initialize();
      this.m_ServerHostId = NetworkTransport.AddHost(this.m_HostTopology, this.listenPort);
      if (LogFilter.logDebug)
        Debug.Log((object) ("Server Host Slot Id: " + (object) this.m_ServerHostId));
      this.Update();
      byte error;
      NetworkTransport.ConnectAsNetworkHost(this.m_ServerHostId, relayIp, relayPort, netGuid, sourceId, nodeId, out error);
      this.m_RelaySlotId = 0;
      if (!LogFilter.logDebug)
        return;
      Debug.Log((object) ("Relay Slot Id: " + (object) this.m_RelaySlotId));
    }

    /// <summary>
    ///   <para>This stops a server from listening.</para>
    /// </summary>
    public void Stop()
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkServerSimple stop ");
      NetworkTransport.RemoveHost(this.m_ServerHostId);
      this.m_ServerHostId = -1;
    }

    internal void RegisterHandlerSafe(short msgType, NetworkMessageDelegate handler)
    {
      this.m_MessageHandlers.RegisterHandlerSafe(msgType, handler);
    }

    /// <summary>
    ///   <para>This registers a handler function for a message Id.</para>
    /// </summary>
    /// <param name="msgType">Message Id to register handler for.</param>
    /// <param name="handler">Handler function.</param>
    public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
    {
      this.m_MessageHandlers.RegisterHandler(msgType, handler);
    }

    /// <summary>
    ///   <para>This unregisters a registered message handler function.</para>
    /// </summary>
    /// <param name="msgType">The message id to unregister.</param>
    public void UnregisterHandler(short msgType)
    {
      this.m_MessageHandlers.UnregisterHandler(msgType);
    }

    /// <summary>
    ///   <para>Clears the message handlers that are registered.</para>
    /// </summary>
    public void ClearHandlers()
    {
      this.m_MessageHandlers.ClearMessageHandlers();
    }

    /// <summary>
    ///   <para>This function causes pending outgoing data on connections to be sent, but unlike Update() it works when the server is not listening.</para>
    /// </summary>
    public void UpdateConnections()
    {
      for (int index = 0; index < this.m_Connections.Count; ++index)
      {
        NetworkConnection connection = this.m_Connections[index];
        if (connection != null)
          connection.FlushChannels();
      }
    }

    /// <summary>
    ///   <para>This function pumps the server causing incoming network data to be processed, and pending outgoing data to be sent.</para>
    /// </summary>
    public void Update()
    {
      if (this.m_ServerHostId == -1)
        return;
      byte error;
      if (this.m_RelaySlotId != -1)
      {
        NetworkEventType relayEventFromHost = NetworkTransport.ReceiveRelayEventFromHost(this.m_ServerHostId, out error);
        if (relayEventFromHost != NetworkEventType.Nothing && LogFilter.logDebug)
          Debug.Log((object) ("NetGroup event:" + (object) relayEventFromHost));
        if (relayEventFromHost == NetworkEventType.ConnectEvent && LogFilter.logDebug)
          Debug.Log((object) "NetGroup server connected");
        if (relayEventFromHost == NetworkEventType.DisconnectEvent && LogFilter.logDebug)
          Debug.Log((object) "NetGroup server disconnected");
      }
      NetworkEventType fromHost;
      do
      {
        int connectionId;
        int channelId;
        int receivedSize;
        fromHost = NetworkTransport.ReceiveFromHost(this.m_ServerHostId, out connectionId, out channelId, this.m_MsgBuffer, this.m_MsgBuffer.Length, out receivedSize, out error);
        if (fromHost == NetworkEventType.Nothing)
          ;
        switch (fromHost)
        {
          case NetworkEventType.DataEvent:
            this.HandleData(connectionId, channelId, receivedSize, error);
            goto case NetworkEventType.Nothing;
          case NetworkEventType.ConnectEvent:
            this.HandleConnect(connectionId, error);
            goto case NetworkEventType.Nothing;
          case NetworkEventType.DisconnectEvent:
            this.HandleDisconnect(connectionId, error);
            goto case NetworkEventType.Nothing;
          case NetworkEventType.Nothing:
            continue;
          default:
            if (LogFilter.logError)
            {
              Debug.LogError((object) ("Unknown network message type received: " + (object) fromHost));
              goto case NetworkEventType.Nothing;
            }
            else
              goto case NetworkEventType.Nothing;
        }
      }
      while (fromHost != NetworkEventType.Nothing);
      this.UpdateConnections();
    }

    /// <summary>
    ///   <para>This looks up the network connection object for the specified connection Id.</para>
    /// </summary>
    /// <param name="connectionId">The connection id to look up.</param>
    /// <returns>
    ///   <para>A NetworkConnection objects, or null if no connection found.</para>
    /// </returns>
    public NetworkConnection FindConnection(int connectionId)
    {
      if (connectionId < 0 || connectionId >= this.m_Connections.Count)
        return (NetworkConnection) null;
      return this.m_Connections[connectionId];
    }

    /// <summary>
    ///   <para>This adds a connection created by external code to the server's list of connections, at the connection's connectionId index.</para>
    /// </summary>
    /// <param name="conn">A new connection object.</param>
    /// <returns>
    ///   <para>True if added.</para>
    /// </returns>
    public bool SetConnectionAtIndex(NetworkConnection conn)
    {
      while (this.m_Connections.Count <= conn.connectionId)
        this.m_Connections.Add((NetworkConnection) null);
      if (this.m_Connections[conn.connectionId] != null)
        return false;
      this.m_Connections[conn.connectionId] = conn;
      conn.SetHandlers(this.m_MessageHandlers);
      return true;
    }

    /// <summary>
    ///   <para>This removes a connection object from the server's list of connections.</para>
    /// </summary>
    /// <param name="connectionId">The id of the connection to remove.</param>
    /// <returns>
    ///   <para>True if removed.</para>
    /// </returns>
    public bool RemoveConnectionAtIndex(int connectionId)
    {
      if (connectionId < 0 || connectionId >= this.m_Connections.Count)
        return false;
      this.m_Connections[connectionId] = (NetworkConnection) null;
      return true;
    }

    private void HandleConnect(int connectionId, byte error)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("NetworkServerSimple accepted client:" + (object) connectionId));
      if ((int) error != 0)
      {
        this.OnConnectError(connectionId, error);
      }
      else
      {
        string address;
        int port;
        NetworkID network;
        NodeID dstNode;
        byte error1;
        NetworkTransport.GetConnectionInfo(this.m_ServerHostId, connectionId, out address, out port, out network, out dstNode, out error1);
        NetworkConnection instance = (NetworkConnection) Activator.CreateInstance(this.m_NetworkConnectionClass);
        instance.SetHandlers(this.m_MessageHandlers);
        instance.Initialize(address, this.m_ServerHostId, connectionId, this.m_HostTopology);
        while (this.m_Connections.Count <= connectionId)
          this.m_Connections.Add((NetworkConnection) null);
        this.m_Connections[connectionId] = instance;
        this.OnConnected(instance);
      }
    }

    private void HandleDisconnect(int connectionId, byte error)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("NetworkServerSimple disconnect client:" + (object) connectionId));
      NetworkConnection connection = this.FindConnection(connectionId);
      if (connection == null)
        return;
      if ((int) error != 0 && (int) error != 6)
      {
        this.m_Connections[connectionId] = (NetworkConnection) null;
        if (LogFilter.logError)
          Debug.LogError((object) ("Server client disconnect error:" + (object) connectionId));
        this.OnDisconnectError(connection, error);
      }
      else
      {
        connection.Disconnect();
        this.m_Connections[connectionId] = (NetworkConnection) null;
        if (LogFilter.logDebug)
          Debug.Log((object) ("Server lost client:" + (object) connectionId));
        this.OnDisconnected(connection);
      }
    }

    private void HandleData(int connectionId, int channelId, int receivedSize, byte error)
    {
      NetworkConnection connection = this.FindConnection(connectionId);
      if (connection == null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("HandleData Unknown connectionId:" + (object) connectionId));
      }
      else if ((int) error != 0)
      {
        this.OnDataError(connection, error);
      }
      else
      {
        this.m_MsgReader.SeekZero();
        this.OnData(connection, receivedSize, channelId);
      }
    }

    /// <summary>
    ///   <para>This sends the data in an array of bytes to the connected client.</para>
    /// </summary>
    /// <param name="connectionId">The id of the connection to send on.</param>
    /// <param name="bytes">The data to send.</param>
    /// <param name="numBytes">The size of the data to send.</param>
    /// <param name="channelId">The channel to send the data on.</param>
    public void SendBytesTo(int connectionId, byte[] bytes, int numBytes, int channelId)
    {
      NetworkConnection connection = this.FindConnection(connectionId);
      if (connection == null)
        return;
      connection.SendBytes(bytes, numBytes, channelId);
    }

    /// <summary>
    ///   <para>This sends the contents of a NetworkWriter object to the connected client.</para>
    /// </summary>
    /// <param name="connectionId">The id of the connection to send on.</param>
    /// <param name="writer">The writer object to send.</param>
    /// <param name="channelId">The channel to send the data on.</param>
    public void SendWriterTo(int connectionId, NetworkWriter writer, int channelId)
    {
      NetworkConnection connection = this.FindConnection(connectionId);
      if (connection == null)
        return;
      connection.SendWriter(writer, channelId);
    }

    /// <summary>
    ///   <para>This disconnects the connection of the corresponding connection id.</para>
    /// </summary>
    /// <param name="connectionId">The id of the connection to disconnect.</param>
    public void Disconnect(int connectionId)
    {
      NetworkConnection connection = this.FindConnection(connectionId);
      if (connection == null)
        return;
      connection.Disconnect();
      this.m_Connections[connectionId] = (NetworkConnection) null;
    }

    /// <summary>
    ///   <para>This disconnects all of the active connections.</para>
    /// </summary>
    public void DisconnectAllConnections()
    {
      for (int index = 0; index < this.m_Connections.Count; ++index)
      {
        NetworkConnection connection = this.m_Connections[index];
        if (connection != null)
        {
          connection.Disconnect();
          connection.Dispose();
        }
      }
    }

    /// <summary>
    ///   <para>A virtual function that is invoked when there is a connection error.</para>
    /// </summary>
    /// <param name="connectionId">The id of the connection with the error.</param>
    /// <param name="error">The error code.</param>
    public virtual void OnConnectError(int connectionId, byte error)
    {
      Debug.LogError((object) ("OnConnectError error:" + (object) error));
    }

    /// <summary>
    ///   <para>A virtual function that is called when a data error occurs on a connection.</para>
    /// </summary>
    /// <param name="conn">The connection object that the error occured on.</param>
    /// <param name="error">The error code.</param>
    public virtual void OnDataError(NetworkConnection conn, byte error)
    {
      Debug.LogError((object) ("OnDataError error:" + (object) error));
    }

    /// <summary>
    ///   <para>A virtual function that is called when a disconnect error happens.</para>
    /// </summary>
    /// <param name="conn">The connection object that the error occured on.</param>
    /// <param name="error">The error code.</param>
    public virtual void OnDisconnectError(NetworkConnection conn, byte error)
    {
      Debug.LogError((object) ("OnDisconnectError error:" + (object) error));
    }

    /// <summary>
    ///   <para>This virtual function can be overridden to perform custom functionality for new network connections.</para>
    /// </summary>
    /// <param name="conn">The new connection object.</param>
    public virtual void OnConnected(NetworkConnection conn)
    {
      conn.InvokeHandlerNoData((short) 32);
    }

    /// <summary>
    ///   <para>This virtual function can be overridden to perform custom functionality for disconnected network connections.</para>
    /// </summary>
    /// <param name="conn"></param>
    public virtual void OnDisconnected(NetworkConnection conn)
    {
      conn.InvokeHandlerNoData((short) 33);
    }

    /// <summary>
    ///   <para>This virtual function can be overridden to perform custom functionality when data is received for a connection.</para>
    /// </summary>
    /// <param name="conn">The connection that data was received on.</param>
    /// <param name="channelId">The channel that data was received on.</param>
    /// <param name="receivedSize">The amount of data received.</param>
    public virtual void OnData(NetworkConnection conn, int receivedSize, int channelId)
    {
      conn.TransportRecieve(this.m_MsgBuffer, receivedSize, channelId);
    }
  }
}
