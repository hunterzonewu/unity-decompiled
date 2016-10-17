// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkServer
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>High level network server.</para>
  /// </summary>
  public sealed class NetworkServer
  {
    private static object s_Sync = (object) new UnityEngine.Object();
    private static RemovePlayerMessage s_RemovePlayerMessage = new RemovePlayerMessage();
    private List<NetworkConnection> m_LocalConnectionsFakeList = new List<NetworkConnection>();
    private float m_MaxDelay = 0.1f;
    private const int k_RemoveListInterval = 100;
    private static bool s_Active;
    private static volatile NetworkServer s_Instance;
    private static bool m_DontListen;
    private bool m_LocalClientActive;
    private ULocalConnectionToClient m_LocalConnection;
    private NetworkScene m_NetworkScene;
    private HashSet<int> m_ExternalConnections;
    private NetworkServer.ServerSimpleWrapper m_SimpleServerSimple;
    private HashSet<NetworkInstanceId> m_RemoveList;
    private int m_RemoveListCount;
    internal static ushort maxPacketSize;

    /// <summary>
    ///   <para>A list of local connections on the server.</para>
    /// </summary>
    public static List<NetworkConnection> localConnections
    {
      get
      {
        return NetworkServer.instance.m_LocalConnectionsFakeList;
      }
    }

    /// <summary>
    ///   <para>The port that the server is listening on.</para>
    /// </summary>
    public static int listenPort
    {
      get
      {
        return NetworkServer.instance.m_SimpleServerSimple.listenPort;
      }
    }

    /// <summary>
    ///   <para>The transport layer hostId used by this server.</para>
    /// </summary>
    public static int serverHostId
    {
      get
      {
        return NetworkServer.instance.m_SimpleServerSimple.serverHostId;
      }
    }

    /// <summary>
    ///   <para>A list of all the current connections from clients.</para>
    /// </summary>
    public static ReadOnlyCollection<NetworkConnection> connections
    {
      get
      {
        return NetworkServer.instance.m_SimpleServerSimple.connections;
      }
    }

    /// <summary>
    ///   <para>Dictionary of the message handlers registered with the server.</para>
    /// </summary>
    public static Dictionary<short, NetworkMessageDelegate> handlers
    {
      get
      {
        return NetworkServer.instance.m_SimpleServerSimple.handlers;
      }
    }

    /// <summary>
    ///   <para>The host topology that the server is using.</para>
    /// </summary>
    public static HostTopology hostTopology
    {
      get
      {
        return NetworkServer.instance.m_SimpleServerSimple.hostTopology;
      }
    }

    /// <summary>
    ///   <para>This is a dictionary of networked objects that have been spawned on the server.</para>
    /// </summary>
    public static Dictionary<NetworkInstanceId, NetworkIdentity> objects
    {
      get
      {
        return NetworkServer.instance.m_NetworkScene.localObjects;
      }
    }

    /// <summary>
    ///   <para>Setting this true will make the server send peer info to all participants of the network.</para>
    /// </summary>
    [Obsolete("Moved to NetworkMigrationManager")]
    public static bool sendPeerInfo
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>If you enable this, the server will not listen for incoming connections on the regular network port.</para>
    /// </summary>
    public static bool dontListen
    {
      get
      {
        return NetworkServer.m_DontListen;
      }
      set
      {
        NetworkServer.m_DontListen = value;
      }
    }

    /// <summary>
    ///   <para>This makes the server listen for WebSockets connections instead of normal transport layer connections.</para>
    /// </summary>
    public static bool useWebSockets
    {
      get
      {
        return NetworkServer.instance.m_SimpleServerSimple.useWebSockets;
      }
      set
      {
        NetworkServer.instance.m_SimpleServerSimple.useWebSockets = value;
      }
    }

    internal static NetworkServer instance
    {
      get
      {
        if (NetworkServer.s_Instance == null)
        {
          lock (NetworkServer.s_Sync)
          {
            if (NetworkServer.s_Instance == null)
              NetworkServer.s_Instance = new NetworkServer();
          }
        }
        return NetworkServer.s_Instance;
      }
    }

    /// <summary>
    ///   <para>Checks if the server has been started.</para>
    /// </summary>
    public static bool active
    {
      get
      {
        return NetworkServer.s_Active;
      }
    }

    /// <summary>
    ///   <para>True is a local client is currently active on the server.</para>
    /// </summary>
    public static bool localClientActive
    {
      get
      {
        return NetworkServer.instance.m_LocalClientActive;
      }
    }

    /// <summary>
    ///   <para>The number of channels the network is configure with.</para>
    /// </summary>
    public static int numChannels
    {
      get
      {
        return NetworkServer.instance.m_SimpleServerSimple.hostTopology.DefaultConfig.ChannelCount;
      }
    }

    /// <summary>
    ///   <para>The maximum delay before sending packets on connections.</para>
    /// </summary>
    public static float maxDelay
    {
      get
      {
        return NetworkServer.instance.m_MaxDelay;
      }
      set
      {
        NetworkServer.instance.InternalSetMaxDelay(value);
      }
    }

    /// <summary>
    ///   <para>The class to be used when creating new network connections.</para>
    /// </summary>
    public static System.Type networkConnectionClass
    {
      get
      {
        return NetworkServer.instance.m_SimpleServerSimple.networkConnectionClass;
      }
    }

    private NetworkServer()
    {
      NetworkTransport.Init();
      if (LogFilter.logDev)
        Debug.Log((object) ("NetworkServer Created version " + (object) Version.Current));
      this.m_RemoveList = new HashSet<NetworkInstanceId>();
      this.m_ExternalConnections = new HashSet<int>();
      this.m_NetworkScene = new NetworkScene();
      this.m_SimpleServerSimple = new NetworkServer.ServerSimpleWrapper(this);
    }

    public static void SetNetworkConnectionClass<T>() where T : NetworkConnection
    {
      NetworkServer.instance.m_SimpleServerSimple.SetNetworkConnectionClass<T>();
    }

    /// <summary>
    ///   <para>This configures the transport layer settings for the server.</para>
    /// </summary>
    /// <param name="config">Transport layer confuration object.</param>
    /// <param name="maxConnections">The maximum number of client connections to allow.</param>
    /// <param name="topology">Transport layer topology object to use.</param>
    /// <returns>
    ///   <para>True if successfully configured.</para>
    /// </returns>
    public static bool Configure(ConnectionConfig config, int maxConnections)
    {
      return NetworkServer.instance.m_SimpleServerSimple.Configure(config, maxConnections);
    }

    /// <summary>
    ///   <para>This configures the transport layer settings for the server.</para>
    /// </summary>
    /// <param name="config">Transport layer confuration object.</param>
    /// <param name="maxConnections">The maximum number of client connections to allow.</param>
    /// <param name="topology">Transport layer topology object to use.</param>
    /// <returns>
    ///   <para>True if successfully configured.</para>
    /// </returns>
    public static bool Configure(HostTopology topology)
    {
      return NetworkServer.instance.m_SimpleServerSimple.Configure(topology);
    }

    /// <summary>
    ///   <para>Reset the NetworkServer singleton.</para>
    /// </summary>
    public static void Reset()
    {
      NetworkDetailStats.ResetAll();
      NetworkTransport.Shutdown();
      NetworkTransport.Init();
      NetworkServer.s_Instance = (NetworkServer) null;
      NetworkServer.s_Active = false;
    }

    /// <summary>
    ///   <para>This shuts down the server and disconnects all clients.</para>
    /// </summary>
    public static void Shutdown()
    {
      if (NetworkServer.s_Instance != null)
      {
        NetworkServer.s_Instance.InternalDisconnectAll();
        if (!NetworkServer.m_DontListen)
          NetworkServer.s_Instance.m_SimpleServerSimple.Stop();
        NetworkServer.s_Instance = (NetworkServer) null;
      }
      NetworkServer.m_DontListen = false;
      NetworkServer.s_Active = false;
    }

    public static bool Listen(MatchInfo matchInfo, int listenPort)
    {
      if (!matchInfo.usingRelay)
        return NetworkServer.instance.InternalListen((string) null, listenPort);
      NetworkServer.instance.InternalListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId);
      return true;
    }

    internal void RegisterMessageHandlers()
    {
      this.m_SimpleServerSimple.RegisterHandlerSafe((short) 35, new NetworkMessageDelegate(NetworkServer.OnClientReadyMessage));
      this.m_SimpleServerSimple.RegisterHandlerSafe((short) 5, new NetworkMessageDelegate(NetworkServer.OnCommandMessage));
      this.m_SimpleServerSimple.RegisterHandlerSafe((short) 6, new NetworkMessageDelegate(NetworkTransform.HandleTransform));
      this.m_SimpleServerSimple.RegisterHandlerSafe((short) 16, new NetworkMessageDelegate(NetworkTransformChild.HandleChildTransform));
      this.m_SimpleServerSimple.RegisterHandlerSafe((short) 38, new NetworkMessageDelegate(NetworkServer.OnRemovePlayerMessage));
      this.m_SimpleServerSimple.RegisterHandlerSafe((short) 40, new NetworkMessageDelegate(NetworkAnimator.OnAnimationServerMessage));
      this.m_SimpleServerSimple.RegisterHandlerSafe((short) 41, new NetworkMessageDelegate(NetworkAnimator.OnAnimationParametersServerMessage));
      this.m_SimpleServerSimple.RegisterHandlerSafe((short) 42, new NetworkMessageDelegate(NetworkAnimator.OnAnimationTriggerServerMessage));
      NetworkServer.maxPacketSize = NetworkServer.hostTopology.DefaultConfig.PacketSize;
    }

    /// <summary>
    ///   <para>Starts a server using a relay server. This is the manual way of using the relay server, as the regular NetworkServer.Connect() will automatically use the relay server if a match exists.</para>
    /// </summary>
    /// <param name="relayIp">Relay server IP Address.</param>
    /// <param name="relayPort">Relay server port.</param>
    /// <param name="netGuid">GUID of the network to create.</param>
    /// <param name="sourceId">This server's sourceId.</param>
    /// <param name="nodeId">The node to join the network with.</param>
    public static void ListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
    {
      NetworkServer.instance.InternalListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId);
    }

    private void InternalListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
    {
      this.m_SimpleServerSimple.ListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId);
      NetworkServer.s_Active = true;
      this.RegisterMessageHandlers();
    }

    /// <summary>
    ///   <para>Start the server on the given port number. Note that if a match has been created, this will listen using the relay server instead of a local socket.</para>
    /// </summary>
    /// <param name="ipAddress">The IP address to bind to (optional).</param>
    /// <param name="serverPort">Listen port number.</param>
    /// <returns>
    ///   <para>True if listen succeeded.</para>
    /// </returns>
    public static bool Listen(int serverPort)
    {
      return NetworkServer.instance.InternalListen((string) null, serverPort);
    }

    /// <summary>
    ///   <para>Start the server on the given port number. Note that if a match has been created, this will listen using the relay server instead of a local socket.</para>
    /// </summary>
    /// <param name="ipAddress">The IP address to bind to (optional).</param>
    /// <param name="serverPort">Listen port number.</param>
    /// <returns>
    ///   <para>True if listen succeeded.</para>
    /// </returns>
    public static bool Listen(string ipAddress, int serverPort)
    {
      return NetworkServer.instance.InternalListen(ipAddress, serverPort);
    }

    internal bool InternalListen(string ipAddress, int serverPort)
    {
      if (NetworkServer.m_DontListen)
        this.m_SimpleServerSimple.Initialize();
      else if (!this.m_SimpleServerSimple.Listen(ipAddress, serverPort))
        return false;
      NetworkServer.maxPacketSize = NetworkServer.hostTopology.DefaultConfig.PacketSize;
      NetworkServer.s_Active = true;
      this.RegisterMessageHandlers();
      return true;
    }

    public static NetworkClient BecomeHost(NetworkClient oldClient, int port, MatchInfo matchInfo, int oldConnectionId, PeerInfoMessage[] peers)
    {
      return NetworkServer.instance.BecomeHostInternal(oldClient, port, matchInfo, oldConnectionId, peers);
    }

    internal NetworkClient BecomeHostInternal(NetworkClient oldClient, int port, MatchInfo matchInfo, int oldConnectionId, PeerInfoMessage[] peers)
    {
      if (NetworkServer.s_Active)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "BecomeHost already a server.");
        return (NetworkClient) null;
      }
      if (!NetworkClient.active)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "BecomeHost NetworkClient not active.");
        return (NetworkClient) null;
      }
      NetworkServer.Configure(NetworkServer.hostTopology);
      if (matchInfo == null)
      {
        if (LogFilter.logDev)
          Debug.Log((object) ("BecomeHost Listen on " + (object) port));
        if (!NetworkServer.Listen(port))
        {
          if (LogFilter.logError)
            Debug.LogError((object) "BecomeHost bind failed.");
          return (NetworkClient) null;
        }
      }
      else
      {
        if (LogFilter.logDev)
          Debug.Log((object) ("BecomeHost match:" + (object) matchInfo.networkId));
        NetworkServer.ListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId);
      }
      using (Dictionary<NetworkInstanceId, NetworkIdentity>.ValueCollection.Enumerator enumerator = ClientScene.objects.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkIdentity current = enumerator.Current;
          if (!((UnityEngine.Object) current == (UnityEngine.Object) null) && !((UnityEngine.Object) current.gameObject == (UnityEngine.Object) null))
          {
            NetworkIdentity.AddNetworkId(current.netId.Value);
            this.m_NetworkScene.SetLocalObject(current.netId, current.gameObject, false, false);
            current.OnStartServer(true);
          }
        }
      }
      if (LogFilter.logDev)
        Debug.Log((object) ("NetworkServer BecomeHost done. oldConnectionId:" + (object) oldConnectionId));
      this.RegisterMessageHandlers();
      if (!NetworkClient.RemoveClient(oldClient) && LogFilter.logError)
        Debug.LogError((object) "BecomeHost failed to remove client");
      if (LogFilter.logDev)
        Debug.Log((object) "BecomeHost localClient ready");
      NetworkClient networkClient = ClientScene.ReconnectLocalServer();
      ClientScene.Ready(networkClient.connection);
      ClientScene.SetReconnectId(oldConnectionId, peers);
      ClientScene.AddPlayer(ClientScene.readyConnection, (short) 0);
      return networkClient;
    }

    private void InternalSetMaxDelay(float seconds)
    {
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
          connection.SetMaxDelay(seconds);
      }
      this.m_MaxDelay = seconds;
    }

    internal int AddLocalClient(LocalClient localClient)
    {
      if (this.m_LocalConnectionsFakeList.Count != 0)
      {
        Debug.LogError((object) "Local Connection already exists");
        return -1;
      }
      this.m_LocalConnection = new ULocalConnectionToClient(localClient);
      this.m_LocalConnection.connectionId = 0;
      this.m_SimpleServerSimple.SetConnectionAtIndex((NetworkConnection) this.m_LocalConnection);
      this.m_LocalConnectionsFakeList.Add((NetworkConnection) this.m_LocalConnection);
      this.m_LocalConnection.InvokeHandlerNoData((short) 32);
      return 0;
    }

    internal void RemoveLocalClient(NetworkConnection localClientConnection)
    {
      for (int index = 0; index < this.m_LocalConnectionsFakeList.Count; ++index)
      {
        if (this.m_LocalConnectionsFakeList[index].connectionId == localClientConnection.connectionId)
        {
          this.m_LocalConnectionsFakeList.RemoveAt(index);
          break;
        }
      }
      if (this.m_LocalConnection != null)
      {
        this.m_LocalConnection.Disconnect();
        this.m_LocalConnection.Dispose();
        this.m_LocalConnection = (ULocalConnectionToClient) null;
      }
      this.m_LocalClientActive = false;
      this.m_SimpleServerSimple.RemoveConnectionAtIndex(0);
    }

    internal void SetLocalObjectOnServer(NetworkInstanceId netId, GameObject obj)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("SetLocalObjectOnServer " + (object) netId + " " + (object) obj));
      this.m_NetworkScene.SetLocalObject(netId, obj, false, true);
    }

    internal void ActivateLocalClientScene()
    {
      if (this.m_LocalClientActive)
        return;
      this.m_LocalClientActive = true;
      using (Dictionary<NetworkInstanceId, NetworkIdentity>.ValueCollection.Enumerator enumerator = NetworkServer.objects.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkIdentity current = enumerator.Current;
          if (!current.isClient)
          {
            if (LogFilter.logDev)
              Debug.Log((object) ("ActivateClientScene " + (object) current.netId + " " + (object) current.gameObject));
            ClientScene.SetLocalObject(current.netId, current.gameObject);
            current.OnStartClient();
          }
        }
      }
    }

    public static bool SendToAll(short msgType, MessageBase msg)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Server.SendToAll msgType:" + (object) msgType));
      bool flag = true;
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
          flag &= connection.Send(msgType, msg);
      }
      return flag;
    }

    private static bool SendToObservers(GameObject contextObj, short msgType, MessageBase msg)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Server.SendToObservers id:" + (object) msgType));
      bool flag = true;
      NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.observers == null)
        return false;
      int count = component.observers.Count;
      for (int index = 0; index < count; ++index)
      {
        NetworkConnection observer = component.observers[index];
        flag &= observer.Send(msgType, msg);
      }
      return flag;
    }

    public static bool SendToReady(GameObject contextObj, short msgType, MessageBase msg)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Server.SendToReady id:" + (object) msgType));
      if ((UnityEngine.Object) contextObj == (UnityEngine.Object) null)
      {
        for (int index = 0; index < NetworkServer.connections.Count; ++index)
        {
          NetworkConnection connection = NetworkServer.connections[index];
          if (connection != null && connection.isReady)
            connection.Send(msgType, msg);
        }
        return true;
      }
      bool flag = true;
      NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.observers == null)
        return false;
      int count = component.observers.Count;
      for (int index = 0; index < count; ++index)
      {
        NetworkConnection observer = component.observers[index];
        if (observer.isReady)
          flag &= observer.Send(msgType, msg);
      }
      return flag;
    }

    public static void SendWriterToReady(GameObject contextObj, NetworkWriter writer, int channelId)
    {
      if (writer.AsArraySegment().Count > (int) short.MaxValue)
        throw new UnityException("NetworkWriter used buffer is too big!");
      NetworkServer.SendBytesToReady(contextObj, writer.AsArraySegment().Array, writer.AsArraySegment().Count, channelId);
    }

    public static void SendBytesToReady(GameObject contextObj, byte[] buffer, int numBytes, int channelId)
    {
      if ((UnityEngine.Object) contextObj == (UnityEngine.Object) null)
      {
        bool flag = true;
        for (int index = 0; index < NetworkServer.connections.Count; ++index)
        {
          NetworkConnection connection = NetworkServer.connections[index];
          if (connection != null && connection.isReady && !connection.SendBytes(buffer, numBytes, channelId))
            flag = false;
        }
        if (flag || !LogFilter.logWarn)
          return;
        Debug.LogWarning((object) "SendBytesToReady failed");
      }
      else
      {
        NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
        try
        {
          bool flag = true;
          int count = component.observers.Count;
          for (int index = 0; index < count; ++index)
          {
            NetworkConnection observer = component.observers[index];
            if (observer.isReady && !observer.SendBytes(buffer, numBytes, channelId))
              flag = false;
          }
          if (flag || !LogFilter.logWarn)
            return;
          Debug.LogWarning((object) ("SendBytesToReady failed for " + (object) contextObj));
        }
        catch (NullReferenceException ex)
        {
          if (!LogFilter.logWarn)
            return;
          Debug.LogWarning((object) ("SendBytesToReady object " + (object) contextObj + " has not been spawned"));
        }
      }
    }

    /// <summary>
    ///   <para>This sends an array of bytes to a specific player.</para>
    /// </summary>
    /// <param name="player">The player to send he bytes to.</param>
    /// <param name="buffer">Array of bytes to send.</param>
    /// <param name="numBytes">Size of array.</param>
    /// <param name="channelId">Transport layer channel id to send bytes on.</param>
    public static void SendBytesToPlayer(GameObject player, byte[] buffer, int numBytes, int channelId)
    {
      for (int index1 = 0; index1 < NetworkServer.connections.Count; ++index1)
      {
        NetworkConnection connection = NetworkServer.connections[index1];
        if (connection != null)
        {
          for (int index2 = 0; index2 < connection.playerControllers.Count; ++index2)
          {
            if (connection.playerControllers[index2].IsValid && (UnityEngine.Object) connection.playerControllers[index2].gameObject == (UnityEngine.Object) player)
            {
              connection.SendBytes(buffer, numBytes, channelId);
              break;
            }
          }
        }
      }
    }

    public static bool SendUnreliableToAll(short msgType, MessageBase msg)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Server.SendUnreliableToAll msgType:" + (object) msgType));
      bool flag = true;
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
          flag &= connection.SendUnreliable(msgType, msg);
      }
      return flag;
    }

    public static bool SendUnreliableToReady(GameObject contextObj, short msgType, MessageBase msg)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Server.SendUnreliableToReady id:" + (object) msgType));
      if ((UnityEngine.Object) contextObj == (UnityEngine.Object) null)
      {
        for (int index = 0; index < NetworkServer.connections.Count; ++index)
        {
          NetworkConnection connection = NetworkServer.connections[index];
          if (connection != null && connection.isReady)
            connection.SendUnreliable(msgType, msg);
        }
        return true;
      }
      bool flag = true;
      NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
      int count = component.observers.Count;
      for (int index = 0; index < count; ++index)
      {
        NetworkConnection observer = component.observers[index];
        if (observer.isReady)
          flag &= observer.SendUnreliable(msgType, msg);
      }
      return flag;
    }

    /// <summary>
    ///   <para>Sends a network message to all connected clients on a specified transport layer QoS channel.</para>
    /// </summary>
    /// <param name="msgType">The message id.</param>
    /// <param name="msg">The message to send.</param>
    /// <param name="channelId">The transport layer channel to use.</param>
    /// <returns>
    ///   <para>True if the message was sent.</para>
    /// </returns>
    public static bool SendByChannelToAll(short msgType, MessageBase msg, int channelId)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Server.SendByChannelToAll id:" + (object) msgType));
      bool flag = true;
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
          flag &= connection.SendByChannel(msgType, msg, channelId);
      }
      return flag;
    }

    /// <summary>
    ///   <para>Sends a network message to all connected clients that are "ready" on a specified transport layer QoS channel.</para>
    /// </summary>
    /// <param name="contextObj">An object to use for context when calculating object visibility. If null, then the message is sent to all ready clients.</param>
    /// <param name="msgType">The message id.</param>
    /// <param name="msg">The message to send.</param>
    /// <param name="channelId">The transport layer channel to send on.</param>
    /// <returns>
    ///   <para>True if the message was sent.</para>
    /// </returns>
    public static bool SendByChannelToReady(GameObject contextObj, short msgType, MessageBase msg, int channelId)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("Server.SendByChannelToReady msgType:" + (object) msgType));
      if ((UnityEngine.Object) contextObj == (UnityEngine.Object) null)
      {
        for (int index = 0; index < NetworkServer.connections.Count; ++index)
        {
          NetworkConnection connection = NetworkServer.connections[index];
          if (connection != null && connection.isReady)
            connection.SendByChannel(msgType, msg, channelId);
        }
        return true;
      }
      bool flag = true;
      NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
      int count = component.observers.Count;
      for (int index = 0; index < count; ++index)
      {
        NetworkConnection observer = component.observers[index];
        if (observer.isReady)
          flag &= observer.SendByChannel(msgType, msg, channelId);
      }
      return flag;
    }

    /// <summary>
    ///   <para>Disconnect all currently connected clients.</para>
    /// </summary>
    public static void DisconnectAll()
    {
      NetworkServer.instance.InternalDisconnectAll();
    }

    internal void InternalDisconnectAll()
    {
      this.m_SimpleServerSimple.DisconnectAllConnections();
      if (this.m_LocalConnection != null)
      {
        this.m_LocalConnection.Disconnect();
        this.m_LocalConnection.Dispose();
        this.m_LocalConnection = (ULocalConnectionToClient) null;
      }
      NetworkServer.s_Active = false;
      this.m_LocalClientActive = false;
    }

    internal static void Update()
    {
      if (NetworkServer.s_Instance == null)
        return;
      NetworkServer.s_Instance.InternalUpdate();
    }

    private void UpdateServerObjects()
    {
      using (Dictionary<NetworkInstanceId, NetworkIdentity>.ValueCollection.Enumerator enumerator = NetworkServer.objects.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkIdentity current = enumerator.Current;
          try
          {
            current.UNetUpdate();
          }
          catch (NullReferenceException ex)
          {
          }
          catch (MissingReferenceException ex)
          {
          }
        }
      }
      if (this.m_RemoveListCount++ % 100 != 0)
        return;
      this.CheckForNullObjects();
    }

    private void CheckForNullObjects()
    {
      using (Dictionary<NetworkInstanceId, NetworkIdentity>.KeyCollection.Enumerator enumerator = NetworkServer.objects.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetworkInstanceId current = enumerator.Current;
          NetworkIdentity networkIdentity = NetworkServer.objects[current];
          if ((UnityEngine.Object) networkIdentity == (UnityEngine.Object) null || (UnityEngine.Object) networkIdentity.gameObject == (UnityEngine.Object) null)
            this.m_RemoveList.Add(current);
        }
      }
      if (this.m_RemoveList.Count <= 0)
        return;
      using (HashSet<NetworkInstanceId>.Enumerator enumerator = this.m_RemoveList.GetEnumerator())
      {
        while (enumerator.MoveNext())
          NetworkServer.objects.Remove(enumerator.Current);
      }
      this.m_RemoveList.Clear();
    }

    internal void InternalUpdate()
    {
      this.m_SimpleServerSimple.Update();
      if (NetworkServer.m_DontListen)
        this.m_SimpleServerSimple.UpdateConnections();
      this.UpdateServerObjects();
    }

    private void OnConnected(NetworkConnection conn)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("Server accepted client:" + (object) conn.connectionId));
      conn.SetMaxDelay(this.m_MaxDelay);
      conn.InvokeHandlerNoData((short) 32);
      NetworkServer.SendCrc(conn);
    }

    private void OnDisconnected(NetworkConnection conn)
    {
      conn.InvokeHandlerNoData((short) 33);
      for (int index = 0; index < conn.playerControllers.Count; ++index)
      {
        if ((UnityEngine.Object) conn.playerControllers[index].gameObject != (UnityEngine.Object) null && LogFilter.logWarn)
          Debug.LogWarning((object) "Player not destroyed when connection disconnected.");
      }
      if (LogFilter.logDebug)
        Debug.Log((object) ("Server lost client:" + (object) conn.connectionId));
      conn.RemoveObservers();
      conn.Dispose();
    }

    private void OnData(NetworkConnection conn, int receivedSize, int channelId)
    {
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 29, "msg", 1);
      conn.TransportRecieve(this.m_SimpleServerSimple.messageBuffer, receivedSize, channelId);
    }

    private void GenerateConnectError(int error)
    {
      if (LogFilter.logError)
        Debug.LogError((object) ("UNet Server Connect Error: " + (object) error));
      this.GenerateError((NetworkConnection) null, error);
    }

    private void GenerateDataError(NetworkConnection conn, int error)
    {
      NetworkError networkError = (NetworkError) error;
      if (LogFilter.logError)
        Debug.LogError((object) ("UNet Server Data Error: " + (object) networkError));
      this.GenerateError(conn, error);
    }

    private void GenerateDisconnectError(NetworkConnection conn, int error)
    {
      NetworkError networkError = (NetworkError) error;
      if (LogFilter.logError)
        Debug.LogError((object) ("UNet Server Disconnect Error: " + (object) networkError + " conn:[" + (object) conn + "]:" + (object) conn.connectionId));
      this.GenerateError(conn, error);
    }

    private void GenerateError(NetworkConnection conn, int error)
    {
      if (!NetworkServer.handlers.ContainsKey((short) 34))
        return;
      ErrorMessage errorMessage = new ErrorMessage();
      errorMessage.errorCode = error;
      NetworkWriter writer = new NetworkWriter();
      errorMessage.Serialize(writer);
      NetworkReader reader = new NetworkReader(writer);
      conn.InvokeHandler((short) 34, reader, 0);
    }

    /// <summary>
    ///   <para>Register a handler for a particular message type.</para>
    /// </summary>
    /// <param name="msgType">Message type number.</param>
    /// <param name="handler">Function handler which will be invoked for when this message type is received.</param>
    public static void RegisterHandler(short msgType, NetworkMessageDelegate handler)
    {
      NetworkServer.instance.m_SimpleServerSimple.RegisterHandler(msgType, handler);
    }

    /// <summary>
    ///   <para>Unregisters a handler for a particular message type.</para>
    /// </summary>
    /// <param name="msgType">The message type to remove the handler for.</param>
    public static void UnregisterHandler(short msgType)
    {
      NetworkServer.instance.m_SimpleServerSimple.UnregisterHandler(msgType);
    }

    /// <summary>
    ///   <para>Clear all registered callback handlers.</para>
    /// </summary>
    public static void ClearHandlers()
    {
      NetworkServer.instance.m_SimpleServerSimple.ClearHandlers();
    }

    /// <summary>
    ///   <para>Clears all registered spawn prefab and spawn handler functions for this server.</para>
    /// </summary>
    public static void ClearSpawners()
    {
      NetworkScene.ClearSpawners();
    }

    public static void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
    {
      numMsgs = 0;
      numBufferedMsgs = 0;
      numBytes = 0;
      lastBufferedPerSecond = 0;
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
        {
          int numMsgs1;
          int numBufferedMsgs1;
          int numBytes1;
          int lastBufferedPerSecond1;
          connection.GetStatsOut(out numMsgs1, out numBufferedMsgs1, out numBytes1, out lastBufferedPerSecond1);
          numMsgs = numMsgs + numMsgs1;
          numBufferedMsgs = numBufferedMsgs + numBufferedMsgs1;
          numBytes = numBytes + numBytes1;
          lastBufferedPerSecond = lastBufferedPerSecond + lastBufferedPerSecond1;
        }
      }
    }

    public static void GetStatsIn(out int numMsgs, out int numBytes)
    {
      numMsgs = 0;
      numBytes = 0;
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
        {
          int numMsgs1;
          int numBytes1;
          connection.GetStatsIn(out numMsgs1, out numBytes1);
          numMsgs = numMsgs + numMsgs1;
          numBytes = numBytes + numBytes1;
        }
      }
    }

    public static void SendToClientOfPlayer(GameObject player, short msgType, MessageBase msg)
    {
      for (int index1 = 0; index1 < NetworkServer.connections.Count; ++index1)
      {
        NetworkConnection connection = NetworkServer.connections[index1];
        if (connection != null)
        {
          for (int index2 = 0; index2 < connection.playerControllers.Count; ++index2)
          {
            if (connection.playerControllers[index2].IsValid && (UnityEngine.Object) connection.playerControllers[index2].gameObject == (UnityEngine.Object) player)
            {
              connection.Send(msgType, msg);
              return;
            }
          }
        }
      }
      if (!LogFilter.logError)
        return;
      Debug.LogError((object) ("Failed to send message to player object '" + player.name + ", not found in connection list"));
    }

    public static void SendToClient(int connectionId, short msgType, MessageBase msg)
    {
      if (connectionId < NetworkServer.connections.Count)
      {
        NetworkConnection connection = NetworkServer.connections[connectionId];
        if (connection != null)
        {
          connection.Send(msgType, msg);
          return;
        }
      }
      if (!LogFilter.logError)
        return;
      Debug.LogError((object) ("Failed to send message to connection ID '" + (object) connectionId + ", not found in connection list"));
    }

    public static bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
    {
      NetworkIdentity view;
      if (NetworkServer.GetNetworkIdentity(player, out view))
        view.SetDynamicAssetId(assetId);
      return NetworkServer.instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
    }

    /// <summary>
    ///   <para>This replaces the player object for a connection with a different player object. The old player object is not destroyed.</para>
    /// </summary>
    /// <param name="conn">Connection which is adding the player.</param>
    /// <param name="player">Player object spawned for the player.</param>
    /// <param name="playerControllerId">The player controller ID number as specified by client.</param>
    /// <returns>
    ///   <para>True if player was replaced.</para>
    /// </returns>
    public static bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
    {
      return NetworkServer.instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
    }

    public static bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
    {
      NetworkIdentity view;
      if (NetworkServer.GetNetworkIdentity(player, out view))
        view.SetDynamicAssetId(assetId);
      return NetworkServer.instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
    }

    /// <summary>
    ///   <para>When an AddPlayer message handler has received a request from a player, the server calls this to associate the player object with the connection.</para>
    /// </summary>
    /// <param name="conn">Connection which is adding the player.</param>
    /// <param name="player">Player object spawned for the player.</param>
    /// <param name="playerControllerId">The player controller ID number as specified by client.</param>
    /// <returns>
    ///   <para>True if player was added.</para>
    /// </returns>
    public static bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
    {
      return NetworkServer.instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
    }

    internal bool InternalAddPlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
    {
      NetworkIdentity view;
      if (!NetworkServer.GetNetworkIdentity(playerGameObject, out view))
      {
        if (LogFilter.logError)
          Debug.Log((object) ("AddPlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + (object) playerGameObject));
        return false;
      }
      if (!NetworkServer.CheckPlayerControllerIdForConnection(conn, playerControllerId))
        return false;
      PlayerController playerController1 = (PlayerController) null;
      GameObject gameObject = (GameObject) null;
      if (conn.GetPlayerController(playerControllerId, out playerController1))
        gameObject = playerController1.gameObject;
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        if (LogFilter.logError)
          Debug.Log((object) ("AddPlayer: player object already exists for playerControllerId of " + (object) playerControllerId));
        return false;
      }
      PlayerController playerController2 = new PlayerController(playerGameObject, playerControllerId);
      conn.SetPlayerController(playerController2);
      view.SetConnectionToClient(conn, playerController2.playerControllerId);
      NetworkServer.SetClientReady(conn);
      if (this.SetupLocalPlayerForConnection(conn, view, playerController2))
        return true;
      if (LogFilter.logDebug)
        Debug.Log((object) ("Adding new playerGameObject object netId: " + (object) playerGameObject.GetComponent<NetworkIdentity>().netId + " asset ID " + (object) playerGameObject.GetComponent<NetworkIdentity>().assetId));
      NetworkServer.FinishPlayerForConnection(conn, view, playerGameObject);
      if (view.localPlayerAuthority)
        view.SetClientOwner(conn);
      return true;
    }

    private static bool CheckPlayerControllerIdForConnection(NetworkConnection conn, short playerControllerId)
    {
      if ((int) playerControllerId < 0)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("AddPlayer: playerControllerId of " + (object) playerControllerId + " is negative"));
        return false;
      }
      if ((int) playerControllerId > 32)
      {
        if (LogFilter.logError)
          Debug.Log((object) ("AddPlayer: playerControllerId of " + (object) playerControllerId + " is too high. max is " + (object) 32));
        return false;
      }
      if ((int) playerControllerId > 16 && LogFilter.logWarn)
        Debug.LogWarning((object) ("AddPlayer: playerControllerId of " + (object) playerControllerId + " is unusually high"));
      return true;
    }

    private bool SetupLocalPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, PlayerController newPlayerController)
    {
      if (LogFilter.logDev)
        Debug.Log((object) ("NetworkServer SetupLocalPlayerForConnection netID:" + (object) uv.netId));
      ULocalConnectionToClient connectionToClient = conn as ULocalConnectionToClient;
      if (connectionToClient == null)
        return false;
      if (LogFilter.logDev)
        Debug.Log((object) "NetworkServer AddPlayer handling ULocalConnectionToClient");
      if (uv.netId.IsEmpty())
        uv.OnStartServer(true);
      uv.RebuildObservers(true);
      this.SendSpawnMessage(uv, (NetworkConnection) null);
      connectionToClient.localClient.AddLocalPlayer(newPlayerController);
      uv.SetClientOwner(conn);
      uv.ForceAuthority(true);
      uv.SetLocalPlayer(newPlayerController.playerControllerId);
      return true;
    }

    private static void FinishPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, GameObject playerGameObject)
    {
      if (uv.netId.IsEmpty())
        NetworkServer.Spawn(playerGameObject);
      conn.Send((short) 4, (MessageBase) new OwnerMessage()
      {
        netId = uv.netId,
        playerControllerId = uv.playerControllerId
      });
    }

    internal bool InternalReplacePlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
    {
      NetworkIdentity view;
      if (!NetworkServer.GetNetworkIdentity(playerGameObject, out view))
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("ReplacePlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + (object) playerGameObject));
        return false;
      }
      if (!NetworkServer.CheckPlayerControllerIdForConnection(conn, playerControllerId))
        return false;
      if (LogFilter.logDev)
        Debug.Log((object) "NetworkServer ReplacePlayer");
      PlayerController playerController1;
      if (conn.GetPlayerController(playerControllerId, out playerController1))
        playerController1.unetView.SetNotLocalPlayer();
      PlayerController playerController2 = new PlayerController(playerGameObject, playerControllerId);
      conn.SetPlayerController(playerController2);
      view.SetConnectionToClient(conn, playerController2.playerControllerId);
      if (LogFilter.logDev)
        Debug.Log((object) "NetworkServer ReplacePlayer setup local");
      if (this.SetupLocalPlayerForConnection(conn, view, playerController2))
        return true;
      if (LogFilter.logDebug)
        Debug.Log((object) ("Replacing playerGameObject object netId: " + (object) playerGameObject.GetComponent<NetworkIdentity>().netId + " asset ID " + (object) playerGameObject.GetComponent<NetworkIdentity>().assetId));
      NetworkServer.FinishPlayerForConnection(conn, view, playerGameObject);
      if (view.localPlayerAuthority)
        view.SetClientOwner(conn);
      return true;
    }

    private static bool GetNetworkIdentity(GameObject go, out NetworkIdentity view)
    {
      view = go.GetComponent<NetworkIdentity>();
      if (!((UnityEngine.Object) view == (UnityEngine.Object) null))
        return true;
      if (LogFilter.logError)
        Debug.LogError((object) "UNET failure. GameObject doesn't have NetworkIdentity.");
      return false;
    }

    /// <summary>
    ///   <para>Sets the client to be ready.</para>
    /// </summary>
    /// <param name="conn">The connection of the client to make ready.</param>
    public static void SetClientReady(NetworkConnection conn)
    {
      NetworkServer.instance.SetClientReadyInternal(conn);
    }

    internal void SetClientReadyInternal(NetworkConnection conn)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("SetClientReadyInternal for conn:" + (object) conn.connectionId));
      if (conn.isReady)
      {
        if (!LogFilter.logDebug)
          return;
        Debug.Log((object) ("SetClientReady conn " + (object) conn.connectionId + " already ready"));
      }
      else
      {
        if (conn.playerControllers.Count == 0 && LogFilter.logDebug)
          Debug.LogWarning((object) "Ready with no player object");
        conn.isReady = true;
        if (conn is ULocalConnectionToClient)
        {
          if (LogFilter.logDev)
            Debug.Log((object) "NetworkServer Ready handling ULocalConnectionToClient");
          using (Dictionary<NetworkInstanceId, NetworkIdentity>.ValueCollection.Enumerator enumerator = NetworkServer.objects.Values.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              NetworkIdentity current = enumerator.Current;
              if ((UnityEngine.Object) current != (UnityEngine.Object) null && (UnityEngine.Object) current.gameObject != (UnityEngine.Object) null)
              {
                if (current.OnCheckObserver(conn))
                  current.AddObserver(conn);
                if (!current.isClient)
                {
                  if (LogFilter.logDev)
                    Debug.Log((object) "LocalClient.SetSpawnObject calling OnStartClient");
                  current.OnStartClient();
                }
              }
            }
          }
        }
        else
        {
          if (LogFilter.logDebug)
            Debug.Log((object) ("Spawning " + (object) NetworkServer.objects.Count + " objects for conn " + (object) conn.connectionId));
          ObjectSpawnFinishedMessage spawnFinishedMessage = new ObjectSpawnFinishedMessage();
          spawnFinishedMessage.state = 0U;
          conn.Send((short) 12, (MessageBase) spawnFinishedMessage);
          using (Dictionary<NetworkInstanceId, NetworkIdentity>.ValueCollection.Enumerator enumerator = NetworkServer.objects.Values.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              NetworkIdentity current = enumerator.Current;
              if ((UnityEngine.Object) current == (UnityEngine.Object) null)
              {
                if (LogFilter.logWarn)
                  Debug.LogWarning((object) "Invalid object found in server local object list (null NetworkIdentity).");
              }
              else if (current.gameObject.activeSelf)
              {
                if (LogFilter.logDebug)
                  Debug.Log((object) ("Sending spawn message for current server objects name='" + current.gameObject.name + "' netId=" + (object) current.netId));
                if (current.OnCheckObserver(conn))
                  current.AddObserver(conn);
              }
            }
          }
          spawnFinishedMessage.state = 1U;
          conn.Send((short) 12, (MessageBase) spawnFinishedMessage);
        }
      }
    }

    internal static void ShowForConnection(NetworkIdentity uv, NetworkConnection conn)
    {
      if (!conn.isReady)
        return;
      NetworkServer.instance.SendSpawnMessage(uv, conn);
    }

    internal static void HideForConnection(NetworkIdentity uv, NetworkConnection conn)
    {
      conn.Send((short) 13, (MessageBase) new ObjectDestroyMessage()
      {
        netId = uv.netId
      });
    }

    /// <summary>
    ///   <para>Marks all connected clients as no longer ready.</para>
    /// </summary>
    public static void SetAllClientsNotReady()
    {
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
          NetworkServer.SetClientNotReady(connection);
      }
    }

    /// <summary>
    ///   <para>Sets the client of the connection to be not-ready.</para>
    /// </summary>
    /// <param name="conn">The connection of the client to make not ready.</param>
    public static void SetClientNotReady(NetworkConnection conn)
    {
      NetworkServer.instance.InternalSetClientNotReady(conn);
    }

    internal void InternalSetClientNotReady(NetworkConnection conn)
    {
      if (!conn.isReady)
        return;
      if (LogFilter.logDebug)
        Debug.Log((object) ("PlayerNotReady " + (object) conn));
      conn.isReady = false;
      conn.RemoveObservers();
      NotReadyMessage notReadyMessage = new NotReadyMessage();
      conn.Send((short) 36, (MessageBase) notReadyMessage);
    }

    private static void OnClientReadyMessage(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("Default handler for ready message from " + (object) netMsg.conn));
      NetworkServer.SetClientReady(netMsg.conn);
    }

    private static void OnRemovePlayerMessage(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<RemovePlayerMessage>(NetworkServer.s_RemovePlayerMessage);
      PlayerController playerController = (PlayerController) null;
      netMsg.conn.GetPlayerController(NetworkServer.s_RemovePlayerMessage.playerControllerId, out playerController);
      if (playerController != null)
      {
        netMsg.conn.RemovePlayerController(NetworkServer.s_RemovePlayerMessage.playerControllerId);
        NetworkServer.Destroy(playerController.gameObject);
      }
      else
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Received remove player message but could not find the player ID: " + (object) NetworkServer.s_RemovePlayerMessage.playerControllerId));
      }
    }

    private static void OnCommandMessage(NetworkMessage netMsg)
    {
      int cmdHash = (int) netMsg.reader.ReadPackedUInt32();
      NetworkInstanceId netId = netMsg.reader.ReadNetworkId();
      GameObject localObject = NetworkServer.FindLocalObject(netId);
      if ((UnityEngine.Object) localObject == (UnityEngine.Object) null)
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("Instance not found when handling Command message [netId=" + (object) netId + "]"));
      }
      else
      {
        NetworkIdentity component = localObject.GetComponent<NetworkIdentity>();
        if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        {
          if (!LogFilter.logWarn)
            return;
          Debug.LogWarning((object) ("NetworkIdentity deleted when handling Command message [netId=" + (object) netId + "]"));
        }
        else
        {
          bool flag = false;
          using (List<PlayerController>.Enumerator enumerator = netMsg.conn.playerControllers.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              PlayerController current = enumerator.Current;
              if ((UnityEngine.Object) current.gameObject != (UnityEngine.Object) null && current.gameObject.GetComponent<NetworkIdentity>().netId == component.netId)
              {
                flag = true;
                break;
              }
            }
          }
          if (!flag && component.clientAuthorityOwner != netMsg.conn)
          {
            if (!LogFilter.logWarn)
              return;
            Debug.LogWarning((object) ("Command for object without authority [netId=" + (object) netId + "]"));
          }
          else
          {
            if (LogFilter.logDev)
              Debug.Log((object) ("OnCommandMessage for netId=" + (object) netId + " conn=" + (object) netMsg.conn));
            component.HandleCommand(cmdHash, netMsg.reader);
          }
        }
      }
    }

    internal void SpawnObject(GameObject obj)
    {
      if (!NetworkServer.active)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("SpawnObject for " + (object) obj + ", NetworkServer is not active. Cannot spawn objects without an active server."));
      }
      else
      {
        NetworkIdentity view;
        if (!NetworkServer.GetNetworkIdentity(obj, out view))
        {
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) ("SpawnObject " + (object) obj + " has no NetworkIdentity. Please add a NetworkIdentity to " + (object) obj));
        }
        else
        {
          view.OnStartServer(false);
          if (LogFilter.logDebug)
            Debug.Log((object) ("SpawnObject instance ID " + (object) view.netId + " asset ID " + (object) view.assetId));
          view.RebuildObservers(true);
        }
      }
    }

    internal void SendSpawnMessage(NetworkIdentity uv, NetworkConnection conn)
    {
      if (uv.serverOnly)
        return;
      if (uv.sceneId.IsEmpty())
      {
        ObjectSpawnMessage objectSpawnMessage = new ObjectSpawnMessage();
        objectSpawnMessage.netId = uv.netId;
        objectSpawnMessage.assetId = uv.assetId;
        objectSpawnMessage.position = uv.transform.position;
        NetworkWriter writer = new NetworkWriter();
        uv.UNetSerializeAllVars(writer);
        if ((int) writer.Position > 0)
          objectSpawnMessage.payload = writer.ToArray();
        if (conn != null)
          conn.Send((short) 3, (MessageBase) objectSpawnMessage);
        else
          NetworkServer.SendToReady(uv.gameObject, (short) 3, (MessageBase) objectSpawnMessage);
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 3, uv.assetId.ToString(), 1);
      }
      else
      {
        ObjectSpawnSceneMessage spawnSceneMessage = new ObjectSpawnSceneMessage();
        spawnSceneMessage.netId = uv.netId;
        spawnSceneMessage.sceneId = uv.sceneId;
        spawnSceneMessage.position = uv.transform.position;
        NetworkWriter writer = new NetworkWriter();
        uv.UNetSerializeAllVars(writer);
        if ((int) writer.Position > 0)
          spawnSceneMessage.payload = writer.ToArray();
        if (conn != null)
          conn.Send((short) 10, (MessageBase) spawnSceneMessage);
        else
          NetworkServer.SendToReady(uv.gameObject, (short) 3, (MessageBase) spawnSceneMessage);
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 10, "sceneId", 1);
      }
    }

    /// <summary>
    ///   <para>This destroys all the player objects associated with a NetworkConnections on a server.</para>
    /// </summary>
    /// <param name="conn">The connections object to clean up for.</param>
    public static void DestroyPlayersForConnection(NetworkConnection conn)
    {
      if (conn.playerControllers.Count == 0)
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) "Empty player list given to NetworkServer.Destroy(), nothing to do.");
      }
      else
      {
        if (conn.clientOwnedObjects != null)
        {
          using (HashSet<NetworkInstanceId>.Enumerator enumerator = new HashSet<NetworkInstanceId>((IEnumerable<NetworkInstanceId>) conn.clientOwnedObjects).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GameObject localObject = NetworkServer.FindLocalObject(enumerator.Current);
              if ((UnityEngine.Object) localObject != (UnityEngine.Object) null)
                NetworkServer.DestroyObject(localObject);
            }
          }
        }
        using (List<PlayerController>.Enumerator enumerator = conn.playerControllers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PlayerController current = enumerator.Current;
            if (current.IsValid)
            {
              if (!((UnityEngine.Object) current.unetView == (UnityEngine.Object) null))
                NetworkServer.DestroyObject(current.unetView, true);
              current.gameObject = (GameObject) null;
            }
          }
        }
        conn.playerControllers.Clear();
      }
    }

    private static void UnSpawnObject(GameObject obj)
    {
      if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
      {
        if (!LogFilter.logDev)
          return;
        Debug.Log((object) "NetworkServer UnspawnObject is null");
      }
      else
      {
        NetworkIdentity view;
        if (!NetworkServer.GetNetworkIdentity(obj, out view))
          return;
        NetworkServer.UnSpawnObject(view);
      }
    }

    private static void UnSpawnObject(NetworkIdentity uv)
    {
      NetworkServer.DestroyObject(uv, false);
    }

    private static void DestroyObject(GameObject obj)
    {
      if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
      {
        if (!LogFilter.logDev)
          return;
        Debug.Log((object) "NetworkServer DestroyObject is null");
      }
      else
      {
        NetworkIdentity view;
        if (!NetworkServer.GetNetworkIdentity(obj, out view))
          return;
        NetworkServer.DestroyObject(view, true);
      }
    }

    private static void DestroyObject(NetworkIdentity uv, bool destroyServerObject)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("DestroyObject instance:" + (object) uv.netId));
      if (NetworkServer.objects.ContainsKey(uv.netId))
        NetworkServer.objects.Remove(uv.netId);
      if (uv.clientAuthorityOwner != null)
        uv.clientAuthorityOwner.RemoveOwnedObject(uv);
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 1, uv.assetId.ToString(), 1);
      ObjectDestroyMessage objectDestroyMessage = new ObjectDestroyMessage();
      objectDestroyMessage.netId = uv.netId;
      NetworkServer.SendToObservers(uv.gameObject, (short) 1, (MessageBase) objectDestroyMessage);
      uv.ClearObservers();
      if (NetworkClient.active && NetworkServer.instance.m_LocalClientActive)
      {
        uv.OnNetworkDestroy();
        ClientScene.SetLocalObject(objectDestroyMessage.netId, (GameObject) null);
      }
      if (destroyServerObject)
        UnityEngine.Object.Destroy((UnityEngine.Object) uv.gameObject);
      uv.SetNoServer();
    }

    /// <summary>
    ///   <para>This clears all of the networked objects that the server is aware of. This can be required if a scene change deleted all of the objects without destroying them in the normal manner.</para>
    /// </summary>
    public static void ClearLocalObjects()
    {
      NetworkServer.objects.Clear();
    }

    /// <summary>
    ///   <para>Spawn the given game object on all clients which are ready.</para>
    /// </summary>
    /// <param name="obj">Game object with NetworkIdentity to spawn.</param>
    public static void Spawn(GameObject obj)
    {
      NetworkServer.instance.SpawnObject(obj);
    }

    /// <summary>
    ///   <para>This spawns an object like NetworkServer.Spawn() but also assigns Client Authority to the specified client.</para>
    /// </summary>
    /// <param name="obj">The object to spawn.</param>
    /// <param name="player">The player object to set Client Authority to.</param>
    /// <param name="assetId">The assetId of the object to spawn. Used for custom spawn handlers.</param>
    /// <param name="conn">The connection to set Client Authority to.</param>
    /// <returns>
    ///   <para>True if the object was spawned.</para>
    /// </returns>
    public static bool SpawnWithClientAuthority(GameObject obj, GameObject player)
    {
      NetworkIdentity component = player.GetComponent<NetworkIdentity>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "SpawnWithClientAuthority player object has no NetworkIdentity");
        return false;
      }
      if (component.connectionToClient != null)
        return NetworkServer.SpawnWithClientAuthority(obj, component.connectionToClient);
      Debug.LogError((object) "SpawnWithClientAuthority player object is not a player.");
      return false;
    }

    /// <summary>
    ///   <para>This spawns an object like NetworkServer.Spawn() but also assigns Client Authority to the specified client.</para>
    /// </summary>
    /// <param name="obj">The object to spawn.</param>
    /// <param name="player">The player object to set Client Authority to.</param>
    /// <param name="assetId">The assetId of the object to spawn. Used for custom spawn handlers.</param>
    /// <param name="conn">The connection to set Client Authority to.</param>
    /// <returns>
    ///   <para>True if the object was spawned.</para>
    /// </returns>
    public static bool SpawnWithClientAuthority(GameObject obj, NetworkConnection conn)
    {
      NetworkServer.Spawn(obj);
      NetworkIdentity component = obj.GetComponent<NetworkIdentity>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null || !component.isServer)
        return false;
      return component.AssignClientAuthority(conn);
    }

    /// <summary>
    ///   <para>This spawns an object like NetworkServer.Spawn() but also assigns Client Authority to the specified client.</para>
    /// </summary>
    /// <param name="obj">The object to spawn.</param>
    /// <param name="player">The player object to set Client Authority to.</param>
    /// <param name="assetId">The assetId of the object to spawn. Used for custom spawn handlers.</param>
    /// <param name="conn">The connection to set Client Authority to.</param>
    /// <returns>
    ///   <para>True if the object was spawned.</para>
    /// </returns>
    public static bool SpawnWithClientAuthority(GameObject obj, NetworkHash128 assetId, NetworkConnection conn)
    {
      NetworkServer.Spawn(obj, assetId);
      NetworkIdentity component = obj.GetComponent<NetworkIdentity>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null || !component.isServer)
        return false;
      return component.AssignClientAuthority(conn);
    }

    public static void Spawn(GameObject obj, NetworkHash128 assetId)
    {
      NetworkIdentity view;
      if (NetworkServer.GetNetworkIdentity(obj, out view))
        view.SetDynamicAssetId(assetId);
      NetworkServer.instance.SpawnObject(obj);
    }

    /// <summary>
    ///   <para>Destroys this object and corresponding objects on all clients.</para>
    /// </summary>
    /// <param name="obj">Game object to destroy.</param>
    public static void Destroy(GameObject obj)
    {
      NetworkServer.DestroyObject(obj);
    }

    /// <summary>
    ///   <para>This takes an object that has been spawned and un-spawns it.</para>
    /// </summary>
    /// <param name="obj">The spawned object to be unspawned.</param>
    public static void UnSpawn(GameObject obj)
    {
      NetworkServer.UnSpawnObject(obj);
    }

    internal bool InvokeBytes(ULocalConnectionToServer conn, byte[] buffer, int numBytes, int channelId)
    {
      NetworkReader reader = new NetworkReader(buffer);
      int num1 = (int) reader.ReadInt16();
      short num2 = reader.ReadInt16();
      if (!NetworkServer.handlers.ContainsKey(num2) || this.m_LocalConnection == null)
        return false;
      this.m_LocalConnection.InvokeHandler(num2, reader, channelId);
      return true;
    }

    internal bool InvokeHandlerOnServer(ULocalConnectionToServer conn, short msgType, MessageBase msg, int channelId)
    {
      if (NetworkServer.handlers.ContainsKey(msgType) && this.m_LocalConnection != null)
      {
        NetworkWriter writer = new NetworkWriter();
        msg.Serialize(writer);
        NetworkReader reader = new NetworkReader(writer);
        this.m_LocalConnection.InvokeHandler(msgType, reader, channelId);
        return true;
      }
      if (LogFilter.logError)
        Debug.LogError((object) ("Local invoke: Failed to find local connection to invoke handler on [connectionId=" + (object) conn.connectionId + "] for MsgId:" + (object) msgType));
      return false;
    }

    public static GameObject FindLocalObject(NetworkInstanceId netId)
    {
      return NetworkServer.instance.m_NetworkScene.FindLocalObject(netId);
    }

    /// <summary>
    ///   <para>Gets aggregate packet stats for all connections.</para>
    /// </summary>
    /// <returns>
    ///   <para>Dictionary of msg types and packet statistics.</para>
    /// </returns>
    public static Dictionary<short, NetworkConnection.PacketStat> GetConnectionStats()
    {
      Dictionary<short, NetworkConnection.PacketStat> dictionary = new Dictionary<short, NetworkConnection.PacketStat>();
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
        {
          using (Dictionary<short, NetworkConnection.PacketStat>.KeyCollection.Enumerator enumerator = connection.packetStats.Keys.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              short current = enumerator.Current;
              if (dictionary.ContainsKey(current))
              {
                NetworkConnection.PacketStat packetStat = dictionary[current];
                packetStat.count += connection.packetStats[current].count;
                packetStat.bytes += connection.packetStats[current].bytes;
                dictionary[current] = packetStat;
              }
              else
                dictionary[current] = connection.packetStats[current];
            }
          }
        }
      }
      return dictionary;
    }

    /// <summary>
    ///   <para>Resets the packet stats on all connections.</para>
    /// </summary>
    public static void ResetConnectionStats()
    {
      for (int index = 0; index < NetworkServer.connections.Count; ++index)
      {
        NetworkConnection connection = NetworkServer.connections[index];
        if (connection != null)
          connection.ResetStats();
      }
    }

    /// <summary>
    ///   <para>This accepts a network connection from another external source and adds it to the server.</para>
    /// </summary>
    /// <param name="conn">Network connection to add.</param>
    /// <returns>
    ///   <para>True if added.</para>
    /// </returns>
    public static bool AddExternalConnection(NetworkConnection conn)
    {
      return NetworkServer.instance.AddExternalConnectionInternal(conn);
    }

    private bool AddExternalConnectionInternal(NetworkConnection conn)
    {
      if (conn.connectionId < 0)
        return false;
      if (conn.connectionId < NetworkServer.connections.Count && NetworkServer.connections[conn.connectionId] != null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("AddExternalConnection failed, already connection for id:" + (object) conn.connectionId));
        return false;
      }
      if (LogFilter.logDebug)
        Debug.Log((object) ("AddExternalConnection external connection " + (object) conn.connectionId));
      this.m_SimpleServerSimple.SetConnectionAtIndex(conn);
      this.m_ExternalConnections.Add(conn.connectionId);
      conn.InvokeHandlerNoData((short) 32);
      return true;
    }

    /// <summary>
    ///   <para>This removes an external connection added with AddExternalConnection().</para>
    /// </summary>
    /// <param name="connectionId">The id of the connection to remove.</param>
    public static void RemoveExternalConnection(int connectionId)
    {
      NetworkServer.instance.RemoveExternalConnectionInternal(connectionId);
    }

    private bool RemoveExternalConnectionInternal(int connectionId)
    {
      if (!this.m_ExternalConnections.Contains(connectionId))
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("RemoveExternalConnection failed, no connection for id:" + (object) connectionId));
        return false;
      }
      if (LogFilter.logDebug)
        Debug.Log((object) ("RemoveExternalConnection external connection " + (object) connectionId));
      NetworkConnection connection = this.m_SimpleServerSimple.FindConnection(connectionId);
      if (connection != null)
        connection.RemoveObservers();
      this.m_SimpleServerSimple.RemoveConnectionAtIndex(connectionId);
      return true;
    }

    /// <summary>
    ///   <para>This causes NetworkIdentity objects in a scene to be spawned on a server.</para>
    /// </summary>
    /// <returns>
    ///   <para>Success if objects where spawned.</para>
    /// </returns>
    public static bool SpawnObjects()
    {
      if (NetworkServer.active)
      {
        NetworkIdentity[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
        foreach (NetworkIdentity networkIdentity in objectsOfTypeAll)
        {
          if (networkIdentity.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity.gameObject.hideFlags != HideFlags.HideAndDontSave && !networkIdentity.sceneId.IsEmpty())
          {
            if (LogFilter.logDebug)
              Debug.Log((object) ("SpawnObjects sceneId:" + (object) networkIdentity.sceneId + " name:" + networkIdentity.gameObject.name));
            networkIdentity.gameObject.SetActive(true);
          }
        }
        foreach (NetworkIdentity networkIdentity in objectsOfTypeAll)
        {
          if (networkIdentity.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity.gameObject.hideFlags != HideFlags.HideAndDontSave && (!networkIdentity.sceneId.IsEmpty() && !networkIdentity.isServer && !((UnityEngine.Object) networkIdentity.gameObject == (UnityEngine.Object) null)))
          {
            NetworkServer.Spawn(networkIdentity.gameObject);
            networkIdentity.ForceAuthority(true);
          }
        }
      }
      return true;
    }

    private static void SendCrc(NetworkConnection targetConnection)
    {
      if (NetworkCRC.singleton == null || !NetworkCRC.scriptCRCCheck)
        return;
      CRCMessage crcMessage = new CRCMessage();
      List<CRCMessageEntry> crcMessageEntryList = new List<CRCMessageEntry>();
      using (Dictionary<string, int>.KeyCollection.Enumerator enumerator = NetworkCRC.singleton.scripts.Keys.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          crcMessageEntryList.Add(new CRCMessageEntry()
          {
            name = current,
            channel = (byte) NetworkCRC.singleton.scripts[current]
          });
        }
      }
      crcMessage.scripts = crcMessageEntryList.ToArray();
      targetConnection.Send((short) 14, (MessageBase) crcMessage);
    }

    /// <summary>
    ///   <para>This sends information about all participants in the current network game to the connection.</para>
    /// </summary>
    /// <param name="targetConnection">Connection to send peer info to.</param>
    [Obsolete("moved to NetworkMigrationManager")]
    public void SendNetworkInfo(NetworkConnection targetConnection)
    {
    }

    private class ServerSimpleWrapper : NetworkServerSimple
    {
      private NetworkServer m_Server;

      public ServerSimpleWrapper(NetworkServer server)
      {
        this.m_Server = server;
      }

      public override void OnConnectError(int connectionId, byte error)
      {
        this.m_Server.GenerateConnectError((int) error);
      }

      public override void OnDataError(NetworkConnection conn, byte error)
      {
        this.m_Server.GenerateDataError(conn, (int) error);
      }

      public override void OnConnected(NetworkConnection conn)
      {
        this.m_Server.OnConnected(conn);
      }

      public override void OnDisconnected(NetworkConnection conn)
      {
        this.m_Server.OnDisconnected(conn);
      }

      public override void OnData(NetworkConnection conn, int receivedSize, int channelId)
      {
        this.m_Server.OnData(conn, receivedSize, channelId);
      }
    }
  }
}
