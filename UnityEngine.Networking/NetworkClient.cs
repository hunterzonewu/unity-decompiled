// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkClient
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>High level UNET client.</para>
  /// </summary>
  public class NetworkClient
  {
    private static List<NetworkClient> s_Clients = new List<NetworkClient>();
    private static CRCMessage s_CRCMessage = new CRCMessage();
    private System.Type m_NetworkConnectionClass = typeof (NetworkConnection);
    private string m_ServerIp = string.Empty;
    private int m_ClientId = -1;
    private int m_ClientConnectionId = -1;
    private NetworkMessageHandlers m_MessageHandlers = new NetworkMessageHandlers();
    private string m_RequestedServerHost = string.Empty;
    private const int k_MaxEventsPerFrame = 500;
    private static bool s_IsActive;
    private HostTopology m_HostTopology;
    private bool m_UseSimulator;
    private int m_SimulatedLatency;
    private float m_PacketLoss;
    private int m_ServerPort;
    private int m_StatResetTime;
    private EndPoint m_RemoteEndPoint;
    protected NetworkConnection m_Connection;
    private byte[] m_MsgBuffer;
    private NetworkReader m_MsgReader;
    protected NetworkClient.ConnectState m_AsyncConnect;

    /// <summary>
    ///   <para>A list of all the active network clients in the current process.</para>
    /// </summary>
    public static List<NetworkClient> allClients
    {
      get
      {
        return NetworkClient.s_Clients;
      }
    }

    /// <summary>
    ///   <para>True if a network client is currently active.</para>
    /// </summary>
    public static bool active
    {
      get
      {
        return NetworkClient.s_IsActive;
      }
    }

    /// <summary>
    ///   <para>The IP address of the server that this client is connected to.</para>
    /// </summary>
    public string serverIp
    {
      get
      {
        return this.m_ServerIp;
      }
    }

    /// <summary>
    ///   <para>The port of the server that this client is connected to.</para>
    /// </summary>
    public int serverPort
    {
      get
      {
        return this.m_ServerPort;
      }
    }

    /// <summary>
    ///   <para>The NetworkConnection object this client is using.</para>
    /// </summary>
    public NetworkConnection connection
    {
      get
      {
        return this.m_Connection;
      }
    }

    /// <summary>
    ///   <para>The other network participants in the current game.</para>
    /// </summary>
    [Obsolete("Moved to NetworkMigrationManager.")]
    public PeerInfoMessage[] peers
    {
      get
      {
        return (PeerInfoMessage[]) null;
      }
    }

    internal int hostId
    {
      get
      {
        return this.m_ClientId;
      }
    }

    /// <summary>
    ///   <para>The registered network message handlers.</para>
    /// </summary>
    public Dictionary<short, NetworkMessageDelegate> handlers
    {
      get
      {
        return this.m_MessageHandlers.GetHandlers();
      }
    }

    /// <summary>
    ///   <para>The number of QoS channels currently configured for this client.</para>
    /// </summary>
    public int numChannels
    {
      get
      {
        return this.m_HostTopology.DefaultConfig.ChannelCount;
      }
    }

    /// <summary>
    ///   <para>The host topology that this client is using.</para>
    /// </summary>
    public HostTopology hostTopology
    {
      get
      {
        return this.m_HostTopology;
      }
    }

    /// <summary>
    ///   <para>This gives the current connection status of the client.</para>
    /// </summary>
    public bool isConnected
    {
      get
      {
        return this.m_AsyncConnect == NetworkClient.ConnectState.Connected;
      }
    }

    /// <summary>
    ///   <para>The class to use when creating new NetworkConnections.</para>
    /// </summary>
    public System.Type networkConnectionClass
    {
      get
      {
        return this.m_NetworkConnectionClass;
      }
    }

    /// <summary>
    ///   <para>Creates a new NetworkClient instance.</para>
    /// </summary>
    public NetworkClient()
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Client created version " + (object) Version.Current));
      this.m_MsgBuffer = new byte[(int) ushort.MaxValue];
      this.m_MsgReader = new NetworkReader(this.m_MsgBuffer);
      NetworkClient.AddClient(this);
    }

    public NetworkClient(NetworkConnection conn)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Client created version " + (object) Version.Current));
      this.m_MsgBuffer = new byte[(int) ushort.MaxValue];
      this.m_MsgReader = new NetworkReader(this.m_MsgBuffer);
      NetworkClient.AddClient(this);
      NetworkClient.SetActive(true);
      this.m_Connection = conn;
      this.m_AsyncConnect = NetworkClient.ConnectState.Connected;
      conn.SetHandlers(this.m_MessageHandlers);
      this.RegisterSystemHandlers(false);
    }

    internal void SetHandlers(NetworkConnection conn)
    {
      conn.SetHandlers(this.m_MessageHandlers);
    }

    public void SetNetworkConnectionClass<T>() where T : NetworkConnection
    {
      this.m_NetworkConnectionClass = typeof (T);
    }

    /// <summary>
    ///   <para>This configures the transport layer settings for a client.</para>
    /// </summary>
    /// <param name="config">Transport layer configuration object.</param>
    /// <param name="maxConnections">The maximum number of connections to allow.</param>
    /// <param name="topology">Transport layer topology object.</param>
    /// <returns>
    ///   <para>True if the configuration was successful.</para>
    /// </returns>
    public bool Configure(ConnectionConfig config, int maxConnections)
    {
      return this.Configure(new HostTopology(config, maxConnections));
    }

    /// <summary>
    ///   <para>This configures the transport layer settings for a client.</para>
    /// </summary>
    /// <param name="config">Transport layer configuration object.</param>
    /// <param name="maxConnections">The maximum number of connections to allow.</param>
    /// <param name="topology">Transport layer topology object.</param>
    /// <returns>
    ///   <para>True if the configuration was successful.</para>
    /// </returns>
    public bool Configure(HostTopology topology)
    {
      this.m_HostTopology = topology;
      return true;
    }

    public void Connect(MatchInfo matchInfo)
    {
      this.PrepareForConnect();
      this.ConnectWithRelay(matchInfo);
    }

    /// <summary>
    ///   <para>This is used by a client that has lost the connection to the old host, to reconnect to the new host of a game.</para>
    /// </summary>
    /// <param name="serverIp">The IP address of the new host.</param>
    /// <param name="serverPort">The port of the new host.</param>
    /// <returns>
    ///   <para>True if able to reconnect.</para>
    /// </returns>
    public bool ReconnectToNewHost(string serverIp, int serverPort)
    {
      if (!NetworkClient.active)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "Reconnect - NetworkClient must be active");
        return false;
      }
      if (this.m_Connection == null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "Reconnect - no old connection exists");
        return false;
      }
      if (LogFilter.logInfo)
        Debug.Log((object) ("NetworkClient Reconnect " + serverIp + ":" + (object) serverPort));
      ClientScene.HandleClientDisconnect(this.m_Connection);
      ClientScene.ClearLocalPlayers();
      this.m_Connection.Disconnect();
      this.m_Connection = (NetworkConnection) null;
      this.m_ClientId = NetworkTransport.AddHost(this.m_HostTopology, 0);
      string hostNameOrAddress = serverIp;
      this.m_ServerPort = serverPort;
      if (Application.platform == RuntimePlatform.WebGLPlayer)
      {
        this.m_ServerIp = hostNameOrAddress;
        this.m_AsyncConnect = NetworkClient.ConnectState.Resolved;
      }
      else if (serverIp.Equals("127.0.0.1") || serverIp.Equals("localhost"))
      {
        this.m_ServerIp = "127.0.0.1";
        this.m_AsyncConnect = NetworkClient.ConnectState.Resolved;
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("Async DNS START:" + hostNameOrAddress));
        this.m_AsyncConnect = NetworkClient.ConnectState.Resolving;
        Dns.BeginGetHostAddresses(hostNameOrAddress, new AsyncCallback(NetworkClient.GetHostAddressesCallback), (object) this);
      }
      return true;
    }

    /// <summary>
    ///   <para>Connect client to a NetworkServer instance with simulated latency and packet loss.</para>
    /// </summary>
    /// <param name="serverIp">Target IP address or hostname.</param>
    /// <param name="serverPort">Target port number.</param>
    /// <param name="latency">Simulated latency in milliseconds.</param>
    /// <param name="packetLoss">Simulated packet loss percentage.</param>
    public void ConnectWithSimulator(string serverIp, int serverPort, int latency, float packetLoss)
    {
      this.m_UseSimulator = true;
      this.m_SimulatedLatency = latency;
      this.m_PacketLoss = packetLoss;
      this.Connect(serverIp, serverPort);
    }

    private static bool IsValidIpV6(string address)
    {
      foreach (char ch in address)
      {
        if ((int) ch != 58 && ((int) ch < 48 || (int) ch > 57) && (((int) ch < 97 || (int) ch > 102) && ((int) ch < 65 || (int) ch > 70)))
          return false;
      }
      return true;
    }

    /// <summary>
    ///   <para>Connect client to a NetworkServer instance.</para>
    /// </summary>
    /// <param name="serverIp">Target IP address or hostname.</param>
    /// <param name="serverPort">Target port number.</param>
    public void Connect(string serverIp, int serverPort)
    {
      this.PrepareForConnect();
      if (LogFilter.logDebug)
        Debug.Log((object) ("Client Connect: " + serverIp + ":" + (object) serverPort));
      string hostNameOrAddress = serverIp;
      this.m_ServerPort = serverPort;
      if (Application.platform == RuntimePlatform.WebGLPlayer)
      {
        this.m_ServerIp = hostNameOrAddress;
        this.m_AsyncConnect = NetworkClient.ConnectState.Resolved;
      }
      else if (serverIp.Equals("127.0.0.1") || serverIp.Equals("localhost"))
      {
        this.m_ServerIp = "127.0.0.1";
        this.m_AsyncConnect = NetworkClient.ConnectState.Resolved;
      }
      else if (serverIp.IndexOf(":") != -1 && NetworkClient.IsValidIpV6(serverIp))
      {
        this.m_ServerIp = serverIp;
        this.m_AsyncConnect = NetworkClient.ConnectState.Resolved;
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("Async DNS START:" + hostNameOrAddress));
        this.m_RequestedServerHost = hostNameOrAddress;
        this.m_AsyncConnect = NetworkClient.ConnectState.Resolving;
        Dns.BeginGetHostAddresses(hostNameOrAddress, new AsyncCallback(NetworkClient.GetHostAddressesCallback), (object) this);
      }
    }

    public void Connect(EndPoint secureTunnelEndPoint)
    {
      this.PrepareForConnect(NetworkTransport.DoesEndPointUsePlatformProtocols(secureTunnelEndPoint));
      if (LogFilter.logDebug)
        Debug.Log((object) "Client Connect to remoteSockAddr");
      if (secureTunnelEndPoint == null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "Connect failed: null endpoint passed in");
        this.m_AsyncConnect = NetworkClient.ConnectState.Failed;
      }
      else if (secureTunnelEndPoint.AddressFamily != AddressFamily.InterNetwork && secureTunnelEndPoint.AddressFamily != AddressFamily.InterNetworkV6)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "Connect failed: Endpoint AddressFamily must be either InterNetwork or InterNetworkV6");
        this.m_AsyncConnect = NetworkClient.ConnectState.Failed;
      }
      else
      {
        string fullName = secureTunnelEndPoint.GetType().FullName;
        if (fullName == "System.Net.IPEndPoint")
        {
          IPEndPoint ipEndPoint = (IPEndPoint) secureTunnelEndPoint;
          this.Connect(ipEndPoint.Address.ToString(), ipEndPoint.Port);
        }
        else if (fullName != "UnityEngine.XboxOne.XboxOneEndPoint" && fullName != "UnityEngine.PS4.SceEndPoint")
        {
          if (LogFilter.logError)
            Debug.LogError((object) "Connect failed: invalid Endpoint (not IPEndPoint or XboxOneEndPoint or SceEndPoint)");
          this.m_AsyncConnect = NetworkClient.ConnectState.Failed;
        }
        else
        {
          byte error = 0;
          this.m_RemoteEndPoint = secureTunnelEndPoint;
          this.m_AsyncConnect = NetworkClient.ConnectState.Connecting;
          try
          {
            this.m_ClientConnectionId = NetworkTransport.ConnectEndPoint(this.m_ClientId, this.m_RemoteEndPoint, 0, out error);
          }
          catch (Exception ex)
          {
            Debug.LogError((object) ("Connect failed: Exception when trying to connect to EndPoint: " + (object) ex));
          }
          if (this.m_ClientConnectionId == 0 && LogFilter.logError)
            Debug.LogError((object) ("Connect failed: Unable to connect to EndPoint (" + (object) error + ")"));
          this.m_Connection = (NetworkConnection) Activator.CreateInstance(this.m_NetworkConnectionClass);
          this.m_Connection.SetHandlers(this.m_MessageHandlers);
          this.m_Connection.Initialize(this.m_ServerIp, this.m_ClientId, this.m_ClientConnectionId, this.m_HostTopology);
        }
      }
    }

    private void PrepareForConnect()
    {
      this.PrepareForConnect(false);
    }

    private void PrepareForConnect(bool usePlatformSpecificProtocols)
    {
      NetworkClient.SetActive(true);
      this.RegisterSystemHandlers(false);
      if (this.m_HostTopology == null)
      {
        ConnectionConfig defaultConfig = new ConnectionConfig();
        int num1 = (int) defaultConfig.AddChannel(QosType.Reliable);
        int num2 = (int) defaultConfig.AddChannel(QosType.Unreliable);
        defaultConfig.UsePlatformSpecificProtocols = usePlatformSpecificProtocols;
        this.m_HostTopology = new HostTopology(defaultConfig, 8);
      }
      if (this.m_UseSimulator)
      {
        int minTimeout = this.m_SimulatedLatency / 3 - 1;
        if (minTimeout < 1)
          minTimeout = 1;
        int maxTimeout = this.m_SimulatedLatency * 3;
        if (LogFilter.logDebug)
          Debug.Log((object) ("AddHost Using Simulator " + (object) minTimeout + "/" + (object) maxTimeout));
        this.m_ClientId = NetworkTransport.AddHostWithSimulator(this.m_HostTopology, minTimeout, maxTimeout, 0);
      }
      else
        this.m_ClientId = NetworkTransport.AddHost(this.m_HostTopology, 0);
    }

    internal static void GetHostAddressesCallback(IAsyncResult ar)
    {
      try
      {
        IPAddress[] hostAddresses = Dns.EndGetHostAddresses(ar);
        NetworkClient asyncState = (NetworkClient) ar.AsyncState;
        if (hostAddresses.Length == 0)
        {
          if (LogFilter.logError)
            Debug.LogError((object) ("DNS lookup failed for:" + asyncState.m_RequestedServerHost));
          asyncState.m_AsyncConnect = NetworkClient.ConnectState.Failed;
        }
        else
        {
          asyncState.m_ServerIp = hostAddresses[0].ToString();
          asyncState.m_AsyncConnect = NetworkClient.ConnectState.Resolved;
          if (!LogFilter.logDebug)
            return;
          Debug.Log((object) ("Async DNS Result:" + asyncState.m_ServerIp + " for " + asyncState.m_RequestedServerHost + ": " + asyncState.m_ServerIp));
        }
      }
      catch (SocketException ex)
      {
        NetworkClient asyncState = (NetworkClient) ar.AsyncState;
        if (LogFilter.logError)
          Debug.LogError((object) ("DNS resolution failed: " + (object) ex.ErrorCode));
        if (LogFilter.logDebug)
          Debug.Log((object) ("Exception:" + (object) ex));
        asyncState.m_AsyncConnect = NetworkClient.ConnectState.Failed;
      }
    }

    internal void ContinueConnect()
    {
      byte error;
      if (this.m_UseSimulator)
      {
        int num = this.m_SimulatedLatency / 3;
        if (num < 1)
          num = 1;
        if (LogFilter.logDebug)
          Debug.Log((object) ("Connect Using Simulator " + (object) (this.m_SimulatedLatency / 3) + "/" + (object) this.m_SimulatedLatency));
        ConnectionSimulatorConfig conf = new ConnectionSimulatorConfig(num, this.m_SimulatedLatency, num, this.m_SimulatedLatency, this.m_PacketLoss);
        this.m_ClientConnectionId = NetworkTransport.ConnectWithSimulator(this.m_ClientId, this.m_ServerIp, this.m_ServerPort, 0, out error, conf);
      }
      else
        this.m_ClientConnectionId = NetworkTransport.Connect(this.m_ClientId, this.m_ServerIp, this.m_ServerPort, 0, out error);
      this.m_Connection = (NetworkConnection) Activator.CreateInstance(this.m_NetworkConnectionClass);
      this.m_Connection.SetHandlers(this.m_MessageHandlers);
      this.m_Connection.Initialize(this.m_ServerIp, this.m_ClientId, this.m_ClientConnectionId, this.m_HostTopology);
    }

    private void ConnectWithRelay(MatchInfo info)
    {
      this.m_AsyncConnect = NetworkClient.ConnectState.Connecting;
      this.Update();
      byte error;
      this.m_ClientConnectionId = NetworkTransport.ConnectToNetworkPeer(this.m_ClientId, info.address, info.port, 0, 0, info.networkId, Utility.GetSourceID(), info.nodeId, out error);
      this.m_Connection = (NetworkConnection) Activator.CreateInstance(this.m_NetworkConnectionClass);
      this.m_Connection.SetHandlers(this.m_MessageHandlers);
      this.m_Connection.Initialize(info.address, this.m_ClientId, this.m_ClientConnectionId, this.m_HostTopology);
      if ((int) error == 0)
        return;
      Debug.LogError((object) ("ConnectToNetworkPeer Error: " + (object) error));
    }

    /// <summary>
    ///   <para>Disconnect from server.</para>
    /// </summary>
    public virtual void Disconnect()
    {
      this.m_AsyncConnect = NetworkClient.ConnectState.Disconnected;
      ClientScene.HandleClientDisconnect(this.m_Connection);
      if (this.m_Connection == null)
        return;
      this.m_Connection.Disconnect();
      this.m_Connection.Dispose();
      this.m_Connection = (NetworkConnection) null;
      NetworkTransport.RemoveHost(this.m_ClientId);
      this.m_ClientId = -1;
    }

    /// <summary>
    ///   <para>This sends a network message with a message Id to the server. This message is sent on channel zero, which by default is the reliable channel.</para>
    /// </summary>
    /// <param name="msgType">The id of the message to send.</param>
    /// <param name="msg">A message instance to send.</param>
    /// <returns>
    ///   <para>True if message was sent.</para>
    /// </returns>
    public bool Send(short msgType, MessageBase msg)
    {
      if (this.m_Connection != null)
      {
        if (this.m_AsyncConnect != NetworkClient.ConnectState.Connected)
        {
          if (LogFilter.logError)
            Debug.LogError((object) "NetworkClient Send when not connected to a server");
          return false;
        }
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 0, msgType.ToString() + ":" + msg.GetType().Name, 1);
        return this.m_Connection.Send(msgType, msg);
      }
      if (LogFilter.logError)
        Debug.LogError((object) "NetworkClient Send with no connection");
      return false;
    }

    /// <summary>
    ///   <para>This sends the contents of the NetworkWriter's buffer to the connected server on the specified channel.</para>
    /// </summary>
    /// <param name="writer">Writer object containing data to send.</param>
    /// <param name="channelId">QoS channel to send data on.</param>
    /// <returns>
    ///   <para>True if data successfully sent.</para>
    /// </returns>
    public bool SendWriter(NetworkWriter writer, int channelId)
    {
      if (this.m_Connection != null)
      {
        if (this.m_AsyncConnect == NetworkClient.ConnectState.Connected)
          return this.m_Connection.SendWriter(writer, channelId);
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkClient SendWriter when not connected to a server");
        return false;
      }
      if (LogFilter.logError)
        Debug.LogError((object) "NetworkClient SendWriter with no connection");
      return false;
    }

    /// <summary>
    ///   <para>This sends the data in an array of bytes to the server that the client is connected to.</para>
    /// </summary>
    /// <param name="data">Data to send.</param>
    /// <param name="numBytes">Number of bytes of data.</param>
    /// <param name="channelId">The QoS channel to send data on.</param>
    /// <returns>
    ///   <para>True if successfully sent.</para>
    /// </returns>
    public bool SendBytes(byte[] data, int numBytes, int channelId)
    {
      if (this.m_Connection != null)
      {
        if (this.m_AsyncConnect == NetworkClient.ConnectState.Connected)
          return this.m_Connection.SendBytes(data, numBytes, channelId);
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkClient SendBytes when not connected to a server");
        return false;
      }
      if (LogFilter.logError)
        Debug.LogError((object) "NetworkClient SendBytes with no connection");
      return false;
    }

    /// <summary>
    ///   <para>This sends a network message with a message Id to the server on channel one, which by default is the unreliable channel.</para>
    /// </summary>
    /// <param name="msgType">The message id to send.</param>
    /// <param name="msg">The message to send.</param>
    /// <returns>
    ///   <para>True if the message was sent.</para>
    /// </returns>
    public bool SendUnreliable(short msgType, MessageBase msg)
    {
      if (this.m_Connection != null)
      {
        if (this.m_AsyncConnect != NetworkClient.ConnectState.Connected)
        {
          if (LogFilter.logError)
            Debug.LogError((object) "NetworkClient SendUnreliable when not connected to a server");
          return false;
        }
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 0, msgType.ToString() + ":" + msg.GetType().Name, 1);
        return this.m_Connection.SendUnreliable(msgType, msg);
      }
      if (LogFilter.logError)
        Debug.LogError((object) "NetworkClient SendUnreliable with no connection");
      return false;
    }

    /// <summary>
    ///   <para>This sends a network message with a message Id to the server on a specific channel.</para>
    /// </summary>
    /// <param name="msgType">The id of the message to send.</param>
    /// <param name="msg">The message to send.</param>
    /// <param name="channelId">The channel to send the message on.</param>
    /// <returns>
    ///   <para>True if the message was sent.</para>
    /// </returns>
    public bool SendByChannel(short msgType, MessageBase msg, int channelId)
    {
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 0, msgType.ToString() + ":" + msg.GetType().Name, 1);
      if (this.m_Connection != null)
      {
        if (this.m_AsyncConnect == NetworkClient.ConnectState.Connected)
          return this.m_Connection.SendByChannel(msgType, msg, channelId);
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkClient SendByChannel when not connected to a server");
        return false;
      }
      if (LogFilter.logError)
        Debug.LogError((object) "NetworkClient SendByChannel with no connection");
      return false;
    }

    /// <summary>
    ///   <para>Set the maximum amount of time that can pass for transmitting the send buffer.</para>
    /// </summary>
    /// <param name="seconds">Delay in seconds.</param>
    public void SetMaxDelay(float seconds)
    {
      if (this.m_Connection == null)
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) "SetMaxDelay failed, not connected.");
      }
      else
        this.m_Connection.SetMaxDelay(seconds);
    }

    /// <summary>
    ///   <para>Shut down a client.</para>
    /// </summary>
    public void Shutdown()
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("Shutting down client " + (object) this.m_ClientId));
      if (this.m_ClientId != -1)
      {
        NetworkTransport.RemoveHost(this.m_ClientId);
        this.m_ClientId = -1;
      }
      NetworkClient.RemoveClient(this);
      if (NetworkClient.s_Clients.Count != 0)
        return;
      NetworkClient.SetActive(false);
    }

    internal virtual void Update()
    {
      if (this.m_ClientId == -1)
        return;
      switch (this.m_AsyncConnect)
      {
        case NetworkClient.ConnectState.None:
          break;
        case NetworkClient.ConnectState.Resolving:
          break;
        case NetworkClient.ConnectState.Resolved:
          this.m_AsyncConnect = NetworkClient.ConnectState.Connecting;
          this.ContinueConnect();
          break;
        case NetworkClient.ConnectState.Disconnected:
          break;
        case NetworkClient.ConnectState.Failed:
          this.GenerateConnectError(11);
          this.m_AsyncConnect = NetworkClient.ConnectState.Disconnected;
          break;
        default:
          if (this.m_Connection != null && (int) Time.time != this.m_StatResetTime)
          {
            this.m_Connection.ResetStats();
            this.m_StatResetTime = (int) Time.time;
          }
          NetworkEventType fromHost;
          do
          {
            int num1 = 0;
            int connectionId;
            int channelId;
            int receivedSize;
            byte error;
            fromHost = NetworkTransport.ReceiveFromHost(this.m_ClientId, out connectionId, out channelId, this.m_MsgBuffer, (int) (ushort) this.m_MsgBuffer.Length, out receivedSize, out error);
            if (fromHost != NetworkEventType.Nothing && LogFilter.logDev)
              Debug.Log((object) ("Client event: host=" + (object) this.m_ClientId + " event=" + (object) fromHost + " error=" + (object) error));
            switch (fromHost)
            {
              case NetworkEventType.DataEvent:
                if ((int) error != 0)
                {
                  this.GenerateDataError((int) error);
                  return;
                }
                NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 29, "msg", 1);
                this.m_MsgReader.SeekZero();
                this.m_Connection.TransportRecieve(this.m_MsgBuffer, receivedSize, channelId);
                goto case NetworkEventType.Nothing;
              case NetworkEventType.ConnectEvent:
                if (LogFilter.logDebug)
                  Debug.Log((object) "Client connected");
                if ((int) error != 0)
                {
                  this.GenerateConnectError((int) error);
                  return;
                }
                this.m_AsyncConnect = NetworkClient.ConnectState.Connected;
                this.m_Connection.InvokeHandlerNoData((short) 32);
                goto case NetworkEventType.Nothing;
              case NetworkEventType.DisconnectEvent:
                if (LogFilter.logDebug)
                  Debug.Log((object) "Client disconnected");
                this.m_AsyncConnect = NetworkClient.ConnectState.Disconnected;
                if ((int) error != 0 && (int) error != 6)
                  this.GenerateDisconnectError((int) error);
                ClientScene.HandleClientDisconnect(this.m_Connection);
                if (this.m_Connection != null)
                {
                  this.m_Connection.InvokeHandlerNoData((short) 33);
                  goto case NetworkEventType.Nothing;
                }
                else
                  goto case NetworkEventType.Nothing;
              case NetworkEventType.Nothing:
                int num2;
                if ((num2 = num1 + 1) >= 500)
                {
                  if (LogFilter.logDebug)
                  {
                    Debug.Log((object) ("MaxEventsPerFrame hit (" + (object) 500 + ")"));
                    goto label_34;
                  }
                  else
                    goto label_34;
                }
                else
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
          while (this.m_ClientId != -1 && fromHost != NetworkEventType.Nothing);
label_34:
          if (this.m_Connection == null || this.m_AsyncConnect != NetworkClient.ConnectState.Connected)
            break;
          this.m_Connection.FlushChannels();
          break;
      }
    }

    private void GenerateConnectError(int error)
    {
      if (LogFilter.logError)
        Debug.LogError((object) ("UNet Client Error Connect Error: " + (object) error));
      this.GenerateError(error);
    }

    private void GenerateDataError(int error)
    {
      NetworkError networkError = (NetworkError) error;
      if (LogFilter.logError)
        Debug.LogError((object) ("UNet Client Data Error: " + (object) networkError));
      this.GenerateError(error);
    }

    private void GenerateDisconnectError(int error)
    {
      NetworkError networkError = (NetworkError) error;
      if (LogFilter.logError)
        Debug.LogError((object) ("UNet Client Disconnect Error: " + (object) networkError));
      this.GenerateError(error);
    }

    private void GenerateError(int error)
    {
      NetworkMessageDelegate networkMessageDelegate = this.m_MessageHandlers.GetHandler((short) 34) ?? this.m_MessageHandlers.GetHandler((short) 34);
      if (networkMessageDelegate == null)
        return;
      ErrorMessage errorMessage = new ErrorMessage();
      errorMessage.errorCode = error;
      byte[] buffer = new byte[200];
      NetworkWriter writer = new NetworkWriter(buffer);
      errorMessage.Serialize(writer);
      NetworkReader networkReader = new NetworkReader(buffer);
      networkMessageDelegate(new NetworkMessage()
      {
        msgType = (short) 34,
        reader = networkReader,
        conn = this.m_Connection,
        channelId = 0
      });
    }

    public void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
    {
      numMsgs = 0;
      numBufferedMsgs = 0;
      numBytes = 0;
      lastBufferedPerSecond = 0;
      if (this.m_Connection == null)
        return;
      this.m_Connection.GetStatsOut(out numMsgs, out numBufferedMsgs, out numBytes, out lastBufferedPerSecond);
    }

    public void GetStatsIn(out int numMsgs, out int numBytes)
    {
      numMsgs = 0;
      numBytes = 0;
      if (this.m_Connection == null)
        return;
      this.m_Connection.GetStatsIn(out numMsgs, out numBytes);
    }

    /// <summary>
    ///   <para>Retrieves statistics about the network packets sent on this connection.</para>
    /// </summary>
    /// <returns>
    ///   <para>Dictionary of packet statistics for the client's connection.</para>
    /// </returns>
    public Dictionary<short, NetworkConnection.PacketStat> GetConnectionStats()
    {
      if (this.m_Connection == null)
        return (Dictionary<short, NetworkConnection.PacketStat>) null;
      return this.m_Connection.packetStats;
    }

    /// <summary>
    ///   <para>Resets the statistics return by NetworkClient.GetConnectionStats() to zero values.</para>
    /// </summary>
    public void ResetConnectionStats()
    {
      if (this.m_Connection == null)
        return;
      this.m_Connection.ResetStats();
    }

    /// <summary>
    ///   <para>Gets the Return Trip Time for this connection.</para>
    /// </summary>
    /// <returns>
    ///   <para>Return trip time in milliseconds.</para>
    /// </returns>
    public int GetRTT()
    {
      if (this.m_ClientId == -1)
        return 0;
      byte error;
      return NetworkTransport.GetCurrentRtt(this.m_ClientId, this.m_ClientConnectionId, out error);
    }

    internal void RegisterSystemHandlers(bool localClient)
    {
      ClientScene.RegisterSystemHandlers(this, localClient);
      this.RegisterHandlerSafe((short) 14, new NetworkMessageDelegate(this.OnCRC));
    }

    private void OnCRC(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<CRCMessage>(NetworkClient.s_CRCMessage);
      NetworkCRC.Validate(NetworkClient.s_CRCMessage.scripts, this.numChannels);
    }

    /// <summary>
    ///   <para>Register a handler for a particular message type.</para>
    /// </summary>
    /// <param name="msgType">Message type number.</param>
    /// <param name="handler">Function handler which will be invoked for when this message type is received.</param>
    public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
    {
      this.m_MessageHandlers.RegisterHandler(msgType, handler);
    }

    public void RegisterHandlerSafe(short msgType, NetworkMessageDelegate handler)
    {
      this.m_MessageHandlers.RegisterHandlerSafe(msgType, handler);
    }

    /// <summary>
    ///   <para>Unregisters a network message handler.</para>
    /// </summary>
    /// <param name="msgType">The message type to unregister.</param>
    public void UnregisterHandler(short msgType)
    {
      this.m_MessageHandlers.UnregisterHandler(msgType);
    }

    /// <summary>
    ///   <para>Retrieves statistics about the network packets sent on all connections.</para>
    /// </summary>
    /// <returns>
    ///   <para>Dictionary of stats.</para>
    /// </returns>
    public static Dictionary<short, NetworkConnection.PacketStat> GetTotalConnectionStats()
    {
      Dictionary<short, NetworkConnection.PacketStat> dictionary = new Dictionary<short, NetworkConnection.PacketStat>();
      using (List<NetworkClient>.Enumerator enumerator1 = NetworkClient.s_Clients.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          Dictionary<short, NetworkConnection.PacketStat> connectionStats = enumerator1.Current.GetConnectionStats();
          using (Dictionary<short, NetworkConnection.PacketStat>.KeyCollection.Enumerator enumerator2 = connectionStats.Keys.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              short current = enumerator2.Current;
              if (dictionary.ContainsKey(current))
              {
                NetworkConnection.PacketStat packetStat = dictionary[current];
                packetStat.count += connectionStats[current].count;
                packetStat.bytes += connectionStats[current].bytes;
                dictionary[current] = packetStat;
              }
              else
                dictionary[current] = connectionStats[current];
            }
          }
        }
      }
      return dictionary;
    }

    internal static void AddClient(NetworkClient client)
    {
      NetworkClient.s_Clients.Add(client);
    }

    internal static bool RemoveClient(NetworkClient client)
    {
      return NetworkClient.s_Clients.Remove(client);
    }

    internal static void UpdateClients()
    {
      for (int index = 0; index < NetworkClient.s_Clients.Count; ++index)
      {
        if (NetworkClient.s_Clients[index] != null)
          NetworkClient.s_Clients[index].Update();
        else
          NetworkClient.s_Clients.RemoveAt(index);
      }
    }

    /// <summary>
    ///   <para>Shuts down all network clients.</para>
    /// </summary>
    public static void ShutdownAll()
    {
      while (NetworkClient.s_Clients.Count != 0)
        NetworkClient.s_Clients[0].Shutdown();
      NetworkClient.s_Clients = new List<NetworkClient>();
      NetworkClient.s_IsActive = false;
      ClientScene.Shutdown();
      NetworkDetailStats.ResetAll();
    }

    internal static void SetActive(bool state)
    {
      if (!NetworkClient.s_IsActive && state)
        NetworkTransport.Init();
      NetworkClient.s_IsActive = state;
    }

    protected enum ConnectState
    {
      None,
      Resolving,
      Resolved,
      Connecting,
      Connected,
      Disconnected,
      Failed,
    }
  }
}
