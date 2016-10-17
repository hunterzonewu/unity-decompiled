// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkManager
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>The NetworkManager is a convenience class for the HLAPI for managing networking systems.</para>
  /// </summary>
  [AddComponentMenu("Network/NetworkManager")]
  public class NetworkManager : MonoBehaviour
  {
    /// <summary>
    ///   <para>The name of the current network scene.</para>
    /// </summary>
    public static string networkSceneName = string.Empty;
    private static List<Transform> s_StartPositions = new List<Transform>();
    private static AddPlayerMessage s_AddPlayerMessage = new AddPlayerMessage();
    private static RemovePlayerMessage s_RemovePlayerMessage = new RemovePlayerMessage();
    private static ErrorMessage s_ErrorMessage = new ErrorMessage();
    [SerializeField]
    private int m_NetworkPort = 7777;
    [SerializeField]
    private string m_ServerBindAddress = string.Empty;
    [SerializeField]
    private string m_NetworkAddress = "localhost";
    [SerializeField]
    private bool m_DontDestroyOnLoad = true;
    [SerializeField]
    private bool m_RunInBackground = true;
    [SerializeField]
    private bool m_ScriptCRCCheck = true;
    [SerializeField]
    private float m_MaxDelay = 0.01f;
    [SerializeField]
    private LogFilter.FilterLevel m_LogLevel = LogFilter.FilterLevel.Info;
    [SerializeField]
    private bool m_AutoCreatePlayer = true;
    [SerializeField]
    private string m_OfflineScene = string.Empty;
    [SerializeField]
    private string m_OnlineScene = string.Empty;
    [SerializeField]
    private List<GameObject> m_SpawnPrefabs = new List<GameObject>();
    [SerializeField]
    private int m_MaxConnections = 4;
    [SerializeField]
    private List<QosType> m_Channels = new List<QosType>();
    [SerializeField]
    private int m_SimulatedLatency = 1;
    [SerializeField]
    private string m_MatchHost = "mm.unet.unity3d.com";
    [SerializeField]
    private int m_MatchPort = 443;
    /// <summary>
    ///   <para>The name of the current match.</para>
    /// </summary>
    public string matchName = "default";
    /// <summary>
    ///   <para>The maximum number of players in the current match.</para>
    /// </summary>
    public uint matchSize = 4;
    [SerializeField]
    private bool m_ServerBindToIP;
    [SerializeField]
    private GameObject m_PlayerPrefab;
    [SerializeField]
    private PlayerSpawnMethod m_PlayerSpawnMethod;
    [SerializeField]
    private bool m_CustomConfig;
    [SerializeField]
    private ConnectionConfig m_ConnectionConfig;
    [SerializeField]
    private GlobalConfig m_GlobalConfig;
    [SerializeField]
    private bool m_UseWebSockets;
    [SerializeField]
    private bool m_UseSimulator;
    [SerializeField]
    private float m_PacketLossPercentage;
    private NetworkMigrationManager m_MigrationManager;
    private EndPoint m_EndPoint;
    private bool m_ClientLoadedScene;
    /// <summary>
    ///   <para>True if the NetworkServer or NetworkClient isactive.</para>
    /// </summary>
    public bool isNetworkActive;
    /// <summary>
    ///   <para>The current NetworkClient being used by the manager.</para>
    /// </summary>
    public NetworkClient client;
    private static int s_StartPositionIndex;
    /// <summary>
    ///   <para>A MatchInfo instance that will be used when StartServer() or StartClient() are called.</para>
    /// </summary>
    public MatchInfo matchInfo;
    /// <summary>
    ///   <para>The UMatch matchmaker object.</para>
    /// </summary>
    public NetworkMatch matchMaker;
    /// <summary>
    ///   <para>The list of matches that are available to join.</para>
    /// </summary>
    public List<MatchDesc> matches;
    /// <summary>
    ///   <para>The NetworkManager singleton object.</para>
    /// </summary>
    public static NetworkManager singleton;
    private static AsyncOperation s_LoadingSceneAsync;
    private static NetworkConnection s_ClientReadyConnection;
    private static string s_Address;
    private static bool s_DomainReload;
    private static NetworkManager s_PendingSingleton;

    /// <summary>
    ///   <para>The network port currently in use.</para>
    /// </summary>
    public int networkPort
    {
      get
      {
        return this.m_NetworkPort;
      }
      set
      {
        this.m_NetworkPort = value;
      }
    }

    /// <summary>
    ///   <para>Flag to tell the server whether to bind to a specific IP address.</para>
    /// </summary>
    public bool serverBindToIP
    {
      get
      {
        return this.m_ServerBindToIP;
      }
      set
      {
        this.m_ServerBindToIP = value;
      }
    }

    /// <summary>
    ///   <para>The IP address to bind the server to.</para>
    /// </summary>
    public string serverBindAddress
    {
      get
      {
        return this.m_ServerBindAddress;
      }
      set
      {
        this.m_ServerBindAddress = value;
      }
    }

    /// <summary>
    ///   <para>The network address currently in use.</para>
    /// </summary>
    public string networkAddress
    {
      get
      {
        return this.m_NetworkAddress;
      }
      set
      {
        this.m_NetworkAddress = value;
      }
    }

    /// <summary>
    ///   <para>A flag to control whether the NetworkManager object is destroyed when the scene changes.</para>
    /// </summary>
    public bool dontDestroyOnLoad
    {
      get
      {
        return this.m_DontDestroyOnLoad;
      }
      set
      {
        this.m_DontDestroyOnLoad = value;
      }
    }

    /// <summary>
    ///   <para>Controls whether the program runs when it is in the background.</para>
    /// </summary>
    public bool runInBackground
    {
      get
      {
        return this.m_RunInBackground;
      }
      set
      {
        this.m_RunInBackground = value;
      }
    }

    /// <summary>
    ///   <para>Flag for using the script CRC check between server and clients.</para>
    /// </summary>
    public bool scriptCRCCheck
    {
      get
      {
        return this.m_ScriptCRCCheck;
      }
      set
      {
        this.m_ScriptCRCCheck = value;
      }
    }

    /// <summary>
    ///   <para>A flag to control sending the network information about every peer to all members of a match.</para>
    /// </summary>
    [Obsolete("moved to NetworkMigrationManager")]
    public bool sendPeerInfo
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
    ///   <para>The maximum delay before sending packets on connections.</para>
    /// </summary>
    public float maxDelay
    {
      get
      {
        return this.m_MaxDelay;
      }
      set
      {
        this.m_MaxDelay = value;
      }
    }

    /// <summary>
    ///   <para>The log level specifically to user for network log messages.</para>
    /// </summary>
    public LogFilter.FilterLevel logLevel
    {
      get
      {
        return this.m_LogLevel;
      }
      set
      {
        this.m_LogLevel = value;
        LogFilter.currentLogLevel = (int) value;
      }
    }

    /// <summary>
    ///   <para>The default prefab to be used to create player objects on the server.</para>
    /// </summary>
    public GameObject playerPrefab
    {
      get
      {
        return this.m_PlayerPrefab;
      }
      set
      {
        this.m_PlayerPrefab = value;
      }
    }

    /// <summary>
    ///   <para>A flag to control whether or not player objects are automatically created on connect, and on scene change.</para>
    /// </summary>
    public bool autoCreatePlayer
    {
      get
      {
        return this.m_AutoCreatePlayer;
      }
      set
      {
        this.m_AutoCreatePlayer = value;
      }
    }

    /// <summary>
    ///   <para>The current method of spawning players used by the NetworkManager.</para>
    /// </summary>
    public PlayerSpawnMethod playerSpawnMethod
    {
      get
      {
        return this.m_PlayerSpawnMethod;
      }
      set
      {
        this.m_PlayerSpawnMethod = value;
      }
    }

    /// <summary>
    ///   <para>The scene to switch to when offline.</para>
    /// </summary>
    public string offlineScene
    {
      get
      {
        return this.m_OfflineScene;
      }
      set
      {
        this.m_OfflineScene = value;
      }
    }

    /// <summary>
    ///   <para>The scene to switch to when online.</para>
    /// </summary>
    public string onlineScene
    {
      get
      {
        return this.m_OnlineScene;
      }
      set
      {
        this.m_OnlineScene = value;
      }
    }

    /// <summary>
    ///   <para>List of prefabs that will be registered with the spawning system.</para>
    /// </summary>
    public List<GameObject> spawnPrefabs
    {
      get
      {
        return this.m_SpawnPrefabs;
      }
    }

    /// <summary>
    ///   <para>The list of currently registered player start positions for the current scene.</para>
    /// </summary>
    public List<Transform> startPositions
    {
      get
      {
        return NetworkManager.s_StartPositions;
      }
    }

    /// <summary>
    ///   <para>Flag to enable custom network configuration.</para>
    /// </summary>
    public bool customConfig
    {
      get
      {
        return this.m_CustomConfig;
      }
      set
      {
        this.m_CustomConfig = value;
      }
    }

    /// <summary>
    ///   <para>The custom network configuration to use.</para>
    /// </summary>
    public ConnectionConfig connectionConfig
    {
      get
      {
        if (this.m_ConnectionConfig == null)
          this.m_ConnectionConfig = new ConnectionConfig();
        return this.m_ConnectionConfig;
      }
    }

    /// <summary>
    ///   <para>The transport layer global configuration to be used.</para>
    /// </summary>
    public GlobalConfig globalConfig
    {
      get
      {
        if (this.m_GlobalConfig == null)
          this.m_GlobalConfig = new GlobalConfig();
        return this.m_GlobalConfig;
      }
    }

    /// <summary>
    ///   <para>The maximum number of concurrent network connections to support.</para>
    /// </summary>
    public int maxConnections
    {
      get
      {
        return this.m_MaxConnections;
      }
      set
      {
        this.m_MaxConnections = value;
      }
    }

    /// <summary>
    ///   <para>The Quality-of-Service channels to use for the network transport layer.</para>
    /// </summary>
    public List<QosType> channels
    {
      get
      {
        return this.m_Channels;
      }
    }

    /// <summary>
    ///   <para>Allows you to specify an EndPoint object instead of setting networkAddress and networkPort (required for some platforms such as Xbox One).</para>
    /// </summary>
    public EndPoint secureTunnelEndpoint
    {
      get
      {
        return this.m_EndPoint;
      }
      set
      {
        this.m_EndPoint = value;
      }
    }

    /// <summary>
    ///   <para>This makes the NetworkServer listen for WebSockets connections instead of normal transport layer connections.</para>
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
    ///   <para>Flag that control whether clients started by this NetworkManager will use simulated latency and packet loss.</para>
    /// </summary>
    public bool useSimulator
    {
      get
      {
        return this.m_UseSimulator;
      }
      set
      {
        this.m_UseSimulator = value;
      }
    }

    /// <summary>
    ///   <para>The delay in milliseconds to be added to incoming and outgoing packets for clients.</para>
    /// </summary>
    public int simulatedLatency
    {
      get
      {
        return this.m_SimulatedLatency;
      }
      set
      {
        this.m_SimulatedLatency = value;
      }
    }

    /// <summary>
    ///   <para>The percentage of incoming and outgoing packets to be dropped for clients.</para>
    /// </summary>
    public float packetLossPercentage
    {
      get
      {
        return this.m_PacketLossPercentage;
      }
      set
      {
        this.m_PacketLossPercentage = value;
      }
    }

    /// <summary>
    ///   <para>The hostname of the matchmaking server.</para>
    /// </summary>
    public string matchHost
    {
      get
      {
        return this.m_MatchHost;
      }
      set
      {
        this.m_MatchHost = value;
      }
    }

    /// <summary>
    ///   <para>The port of the matchmaking service.</para>
    /// </summary>
    public int matchPort
    {
      get
      {
        return this.m_MatchPort;
      }
      set
      {
        this.m_MatchPort = value;
      }
    }

    /// <summary>
    ///   <para>This is true if the client loaded a new scene when connecting to the server.</para>
    /// </summary>
    public bool clientLoadedScene
    {
      get
      {
        return this.m_ClientLoadedScene;
      }
      set
      {
        this.m_ClientLoadedScene = value;
      }
    }

    /// <summary>
    ///   <para>The migration manager being used with the NetworkManager.</para>
    /// </summary>
    public NetworkMigrationManager migrationManager
    {
      get
      {
        return this.m_MigrationManager;
      }
    }

    /// <summary>
    ///   <para>NumPlayers is the number of active player objects across all connections on the server.</para>
    /// </summary>
    public int numPlayers
    {
      get
      {
        int num = 0;
        foreach (NetworkConnection connection in NetworkServer.connections)
        {
          if (connection != null)
          {
            using (List<PlayerController>.Enumerator enumerator = connection.playerControllers.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current.IsValid)
                  ++num;
              }
            }
          }
        }
        return num;
      }
    }

    public NetworkManager()
    {
      NetworkManager.s_PendingSingleton = this;
    }

    internal static void OnDomainReload()
    {
      NetworkManager.s_DomainReload = true;
    }

    private void Awake()
    {
      this.InitializeSingleton();
    }

    private void InitializeSingleton()
    {
      if ((UnityEngine.Object) NetworkManager.singleton != (UnityEngine.Object) null && (UnityEngine.Object) NetworkManager.singleton == (UnityEngine.Object) this)
        return;
      LogFilter.currentLogLevel = (int) this.m_LogLevel;
      if (this.m_DontDestroyOnLoad)
      {
        if ((UnityEngine.Object) NetworkManager.singleton != (UnityEngine.Object) null)
        {
          if (LogFilter.logDev)
            Debug.Log((object) "Multiple NetworkManagers detected in the scene. Only one NetworkManager can exist at a time. The duplicate NetworkManager will not be used.");
          UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
          return;
        }
        if (LogFilter.logDev)
          Debug.Log((object) "NetworkManager created singleton (DontDestroyOnLoad)");
        NetworkManager.singleton = this;
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
      }
      else
      {
        if (LogFilter.logDev)
          Debug.Log((object) "NetworkManager created singleton (ForScene)");
        NetworkManager.singleton = this;
      }
      if (this.m_NetworkAddress != string.Empty)
      {
        NetworkManager.s_Address = this.m_NetworkAddress;
      }
      else
      {
        if (!(NetworkManager.s_Address != string.Empty))
          return;
        this.m_NetworkAddress = NetworkManager.s_Address;
      }
    }

    private void OnValidate()
    {
      if (this.m_SimulatedLatency < 1)
        this.m_SimulatedLatency = 1;
      if (this.m_SimulatedLatency > 500)
        this.m_SimulatedLatency = 500;
      if ((double) this.m_PacketLossPercentage < 0.0)
        this.m_PacketLossPercentage = 0.0f;
      if ((double) this.m_PacketLossPercentage > 99.0)
        this.m_PacketLossPercentage = 99f;
      if (this.m_MaxConnections <= 0)
        this.m_MaxConnections = 1;
      if (this.m_MaxConnections > 32000)
        this.m_MaxConnections = 32000;
      if ((UnityEngine.Object) this.m_PlayerPrefab != (UnityEngine.Object) null && (UnityEngine.Object) this.m_PlayerPrefab.GetComponent<NetworkIdentity>() == (UnityEngine.Object) null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkManager - playerPrefab must have a NetworkIdentity.");
        this.m_PlayerPrefab = (GameObject) null;
      }
      if (this.m_ConnectionConfig.MinUpdateTimeout <= 0U)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkManager MinUpdateTimeout cannot be zero or less. The value will be reset to 1 millisecond");
        this.m_ConnectionConfig.MinUpdateTimeout = 1U;
      }
      if (this.m_GlobalConfig == null || this.m_GlobalConfig.ThreadAwakeTimeout > 0U)
        return;
      if (LogFilter.logError)
        Debug.LogError((object) "NetworkManager ThreadAwakeTimeout cannot be zero or less. The value will be reset to 1 millisecond");
      this.m_GlobalConfig.ThreadAwakeTimeout = 1U;
    }

    internal void RegisterServerMessages()
    {
      NetworkServer.RegisterHandler((short) 32, new NetworkMessageDelegate(this.OnServerConnectInternal));
      NetworkServer.RegisterHandler((short) 33, new NetworkMessageDelegate(this.OnServerDisconnectInternal));
      NetworkServer.RegisterHandler((short) 35, new NetworkMessageDelegate(this.OnServerReadyMessageInternal));
      NetworkServer.RegisterHandler((short) 37, new NetworkMessageDelegate(this.OnServerAddPlayerMessageInternal));
      NetworkServer.RegisterHandler((short) 38, new NetworkMessageDelegate(this.OnServerRemovePlayerMessageInternal));
      NetworkServer.RegisterHandler((short) 34, new NetworkMessageDelegate(this.OnServerErrorInternal));
    }

    /// <summary>
    ///   <para>This sets up a NetworkMigrationManager object to work with this NetworkManager.</para>
    /// </summary>
    /// <param name="man">The migration manager object to use with the NetworkManager.</param>
    public void SetupMigrationManager(NetworkMigrationManager man)
    {
      this.m_MigrationManager = man;
    }

    public bool StartServer(ConnectionConfig config, int maxConnections)
    {
      return this.StartServer((MatchInfo) null, config, maxConnections);
    }

    /// <summary>
    ///   <para>This starts a new server.</para>
    /// </summary>
    /// <returns>
    ///   <para>True is the server was started.</para>
    /// </returns>
    public bool StartServer()
    {
      return this.StartServer((MatchInfo) null);
    }

    public bool StartServer(MatchInfo info)
    {
      return this.StartServer(info, (ConnectionConfig) null, -1);
    }

    private bool StartServer(MatchInfo info, ConnectionConfig config, int maxConnections)
    {
      this.InitializeSingleton();
      this.OnStartServer();
      if (this.m_RunInBackground)
        Application.runInBackground = true;
      NetworkCRC.scriptCRCCheck = this.scriptCRCCheck;
      NetworkServer.useWebSockets = this.m_UseWebSockets;
      if (this.m_GlobalConfig != null)
        NetworkTransport.Init(this.m_GlobalConfig);
      if (this.m_CustomConfig && this.m_ConnectionConfig != null && config == null)
      {
        this.m_ConnectionConfig.Channels.Clear();
        using (List<QosType>.Enumerator enumerator = this.m_Channels.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int num = (int) this.m_ConnectionConfig.AddChannel(enumerator.Current);
          }
        }
        NetworkServer.Configure(this.m_ConnectionConfig, this.m_MaxConnections);
      }
      if (config != null)
        NetworkServer.Configure(config, maxConnections);
      if (info != null)
      {
        if (!NetworkServer.Listen(info, this.m_NetworkPort))
        {
          if (LogFilter.logError)
            Debug.LogError((object) "StartServer listen failed.");
          return false;
        }
      }
      else if (this.m_ServerBindToIP && !string.IsNullOrEmpty(this.m_ServerBindAddress))
      {
        if (!NetworkServer.Listen(this.m_ServerBindAddress, this.m_NetworkPort))
        {
          if (LogFilter.logError)
            Debug.LogError((object) ("StartServer listen on " + this.m_ServerBindAddress + " failed."));
          return false;
        }
      }
      else if (!NetworkServer.Listen(this.m_NetworkPort))
      {
        if (LogFilter.logError)
          Debug.LogError((object) "StartServer listen failed.");
        return false;
      }
      this.RegisterServerMessages();
      if (LogFilter.logDebug)
        Debug.Log((object) ("NetworkManager StartServer port:" + (object) this.m_NetworkPort));
      this.isNetworkActive = true;
      string name = SceneManager.GetSceneAt(0).name;
      if (this.m_OnlineScene != string.Empty && this.m_OnlineScene != name && this.m_OnlineScene != this.m_OfflineScene)
        this.ServerChangeScene(this.m_OnlineScene);
      else
        NetworkServer.SpawnObjects();
      return true;
    }

    internal void RegisterClientMessages(NetworkClient client)
    {
      client.RegisterHandler((short) 32, new NetworkMessageDelegate(this.OnClientConnectInternal));
      client.RegisterHandler((short) 33, new NetworkMessageDelegate(this.OnClientDisconnectInternal));
      client.RegisterHandler((short) 36, new NetworkMessageDelegate(this.OnClientNotReadyMessageInternal));
      client.RegisterHandler((short) 34, new NetworkMessageDelegate(this.OnClientErrorInternal));
      client.RegisterHandler((short) 39, new NetworkMessageDelegate(this.OnClientSceneInternal));
      if ((UnityEngine.Object) this.m_PlayerPrefab != (UnityEngine.Object) null)
        ClientScene.RegisterPrefab(this.m_PlayerPrefab);
      using (List<GameObject>.Enumerator enumerator = this.m_SpawnPrefabs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if ((UnityEngine.Object) current != (UnityEngine.Object) null)
            ClientScene.RegisterPrefab(current);
        }
      }
    }

    /// <summary>
    ///   <para>This allows the NetworkManager to use a client object created externally to the NetworkManager instead of using StartClient().</para>
    /// </summary>
    /// <param name="externalClient">The NetworkClient object to use.</param>
    public void UseExternalClient(NetworkClient externalClient)
    {
      if (this.m_RunInBackground)
        Application.runInBackground = true;
      if (externalClient != null)
      {
        this.client = externalClient;
        this.isNetworkActive = true;
        this.RegisterClientMessages(this.client);
        this.OnStartClient(this.client);
      }
      else
      {
        this.OnStopClient();
        ClientScene.DestroyAllClientObjects();
        ClientScene.HandleClientDisconnect(this.client.connection);
        this.client = (NetworkClient) null;
        if (this.m_OfflineScene != string.Empty)
          this.ClientChangeScene(this.m_OfflineScene, false);
      }
      NetworkManager.s_Address = this.m_NetworkAddress;
    }

    public NetworkClient StartClient(MatchInfo info, ConnectionConfig config)
    {
      this.InitializeSingleton();
      this.matchInfo = info;
      if (this.m_RunInBackground)
        Application.runInBackground = true;
      this.isNetworkActive = true;
      if (this.m_GlobalConfig != null)
        NetworkTransport.Init(this.m_GlobalConfig);
      this.client = new NetworkClient();
      if (config != null)
      {
        if (config.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4)
          throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
        this.client.Configure(config, 1);
      }
      else if (this.m_CustomConfig && this.m_ConnectionConfig != null)
      {
        this.m_ConnectionConfig.Channels.Clear();
        using (List<QosType>.Enumerator enumerator = this.m_Channels.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int num = (int) this.m_ConnectionConfig.AddChannel(enumerator.Current);
          }
        }
        if (this.m_ConnectionConfig.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4)
          throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
        this.client.Configure(this.m_ConnectionConfig, this.m_MaxConnections);
      }
      this.RegisterClientMessages(this.client);
      if (this.matchInfo != null)
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("NetworkManager StartClient match: " + (object) this.matchInfo));
        this.client.Connect(this.matchInfo);
      }
      else if (this.m_EndPoint != null)
      {
        if (LogFilter.logDebug)
          Debug.Log((object) "NetworkManager StartClient using provided SecureTunnel");
        this.client.Connect(this.m_EndPoint);
      }
      else
      {
        if (string.IsNullOrEmpty(this.m_NetworkAddress))
        {
          if (LogFilter.logError)
            Debug.LogError((object) "Must set the Network Address field in the manager");
          return (NetworkClient) null;
        }
        if (LogFilter.logDebug)
          Debug.Log((object) ("NetworkManager StartClient address:" + this.m_NetworkAddress + " port:" + (object) this.m_NetworkPort));
        if (this.m_UseSimulator)
          this.client.ConnectWithSimulator(this.m_NetworkAddress, this.m_NetworkPort, this.m_SimulatedLatency, this.m_PacketLossPercentage);
        else
          this.client.Connect(this.m_NetworkAddress, this.m_NetworkPort);
      }
      if ((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null)
        this.m_MigrationManager.Initialize(this.client, this.matchInfo);
      this.OnStartClient(this.client);
      NetworkManager.s_Address = this.m_NetworkAddress;
      return this.client;
    }

    public NetworkClient StartClient(MatchInfo matchInfo)
    {
      return this.StartClient(matchInfo, (ConnectionConfig) null);
    }

    /// <summary>
    ///   <para>This starts a network client. It uses the networkAddress and networkPort properties as the address to connect to.</para>
    /// </summary>
    /// <returns>
    ///   <para>The client object created.</para>
    /// </returns>
    public NetworkClient StartClient()
    {
      return this.StartClient((MatchInfo) null, (ConnectionConfig) null);
    }

    public virtual NetworkClient StartHost(ConnectionConfig config, int maxConnections)
    {
      this.OnStartHost();
      if (!this.StartServer(config, maxConnections))
        return (NetworkClient) null;
      NetworkClient client = this.ConnectLocalClient();
      this.OnServerConnect(client.connection);
      this.OnStartClient(client);
      return client;
    }

    public virtual NetworkClient StartHost(MatchInfo info)
    {
      this.OnStartHost();
      this.matchInfo = info;
      if (!this.StartServer(info))
        return (NetworkClient) null;
      NetworkClient client = this.ConnectLocalClient();
      this.OnServerConnect(client.connection);
      this.OnStartClient(client);
      return client;
    }

    /// <summary>
    ///   <para>This starts a network "host" - a server and client in the same application.</para>
    /// </summary>
    /// <returns>
    ///   <para>The client object created - this is a "local client".</para>
    /// </returns>
    public virtual NetworkClient StartHost()
    {
      this.OnStartHost();
      if (!this.StartServer())
        return (NetworkClient) null;
      NetworkClient client = this.ConnectLocalClient();
      this.OnServerConnect(client.connection);
      this.OnStartClient(client);
      return client;
    }

    private NetworkClient ConnectLocalClient()
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("NetworkManager StartHost port:" + (object) this.m_NetworkPort));
      this.m_NetworkAddress = "localhost";
      this.client = ClientScene.ConnectLocalServer();
      this.RegisterClientMessages(this.client);
      if ((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null)
        this.m_MigrationManager.Initialize(this.client, this.matchInfo);
      return this.client;
    }

    /// <summary>
    ///   <para>This stops both the client and the server that the manager is using.</para>
    /// </summary>
    public void StopHost()
    {
      bool active = NetworkServer.active;
      this.OnStopHost();
      this.StopServer();
      this.StopClient();
      if (!((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null) || !active)
        return;
      this.m_MigrationManager.LostHostOnHost();
    }

    /// <summary>
    ///   <para>Stops the server that the manager is using.</para>
    /// </summary>
    public void StopServer()
    {
      if (!NetworkServer.active)
        return;
      this.OnStopServer();
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager StopServer");
      this.isNetworkActive = false;
      NetworkServer.Shutdown();
      this.StopMatchMaker();
      if (!(this.m_OfflineScene != string.Empty))
        return;
      this.ServerChangeScene(this.m_OfflineScene);
    }

    /// <summary>
    ///   <para>Stops the client that the manager is using.</para>
    /// </summary>
    public void StopClient()
    {
      this.OnStopClient();
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager StopClient");
      this.isNetworkActive = false;
      if (this.client != null)
      {
        this.client.Disconnect();
        this.client.Shutdown();
        this.client = (NetworkClient) null;
      }
      this.StopMatchMaker();
      ClientScene.DestroyAllClientObjects();
      if (!(this.m_OfflineScene != string.Empty))
        return;
      this.ClientChangeScene(this.m_OfflineScene, false);
    }

    /// <summary>
    ///   <para>This causes the server to switch scenes and sets the networkSceneName.</para>
    /// </summary>
    /// <param name="newSceneName">The name of the scene to change to. The server will change scene immediately, and a message will be sent to connected clients to ask them to change scene also.</param>
    public virtual void ServerChangeScene(string newSceneName)
    {
      if (string.IsNullOrEmpty(newSceneName))
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "ServerChangeScene empty scene name");
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("ServerChangeScene " + newSceneName));
        NetworkServer.SetAllClientsNotReady();
        NetworkManager.networkSceneName = newSceneName;
        NetworkManager.s_LoadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
        NetworkServer.SendToAll((short) 39, (MessageBase) new StringMessage(NetworkManager.networkSceneName));
        NetworkManager.s_StartPositionIndex = 0;
        NetworkManager.s_StartPositions.Clear();
      }
    }

    internal void ClientChangeScene(string newSceneName, bool forceReload)
    {
      if (string.IsNullOrEmpty(newSceneName))
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "ClientChangeScene empty scene name");
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("ClientChangeScene newSceneName:" + newSceneName + " networkSceneName:" + NetworkManager.networkSceneName));
        if (newSceneName == NetworkManager.networkSceneName)
        {
          if ((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null)
          {
            this.FinishLoadScene();
            return;
          }
          if (!forceReload)
          {
            this.FinishLoadScene();
            return;
          }
        }
        NetworkManager.s_LoadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
        NetworkManager.networkSceneName = newSceneName;
      }
    }

    private void FinishLoadScene()
    {
      if (this.client != null)
      {
        if (NetworkManager.s_ClientReadyConnection != null)
        {
          this.m_ClientLoadedScene = true;
          this.OnClientConnect(NetworkManager.s_ClientReadyConnection);
          NetworkManager.s_ClientReadyConnection = (NetworkConnection) null;
        }
      }
      else if (LogFilter.logDev)
        Debug.Log((object) "FinishLoadScene client is null");
      if (NetworkServer.active)
      {
        NetworkServer.SpawnObjects();
        this.OnServerSceneChanged(NetworkManager.networkSceneName);
      }
      if (!this.IsClientConnected() || this.client == null)
        return;
      this.RegisterClientMessages(this.client);
      this.OnClientSceneChanged(this.client.connection);
    }

    internal static void UpdateScene()
    {
      if ((UnityEngine.Object) NetworkManager.singleton == (UnityEngine.Object) null && (UnityEngine.Object) NetworkManager.s_PendingSingleton != (UnityEngine.Object) null && NetworkManager.s_DomainReload)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) "NetworkManager detected a script reload in the editor. This has caused the network to be shut down.");
        NetworkManager.s_DomainReload = false;
        NetworkManager.s_PendingSingleton.InitializeSingleton();
        foreach (Component component in UnityEngine.Object.FindObjectsOfType<NetworkIdentity>())
          UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
        NetworkManager.singleton.StopHost();
        NetworkTransport.Shutdown();
      }
      if ((UnityEngine.Object) NetworkManager.singleton == (UnityEngine.Object) null || NetworkManager.s_LoadingSceneAsync == null || !NetworkManager.s_LoadingSceneAsync.isDone)
        return;
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientChangeScene done readyCon:" + (object) NetworkManager.s_ClientReadyConnection));
      NetworkManager.singleton.FinishLoadScene();
      NetworkManager.s_LoadingSceneAsync.allowSceneActivation = true;
      NetworkManager.s_LoadingSceneAsync = (AsyncOperation) null;
    }

    private void OnDestroy()
    {
      if (!LogFilter.logDev)
        return;
      Debug.Log((object) "NetworkManager destroyed");
    }

    /// <summary>
    ///   <para>Registers the transform of a game object as a player spawn location.</para>
    /// </summary>
    /// <param name="start">Transform to register.</param>
    public static void RegisterStartPosition(Transform start)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("RegisterStartPosition:" + (object) start));
      NetworkManager.s_StartPositions.Add(start);
    }

    /// <summary>
    ///   <para>Unregisters the transform of a game object as a player spawn location.</para>
    /// </summary>
    /// <param name="start"></param>
    public static void UnRegisterStartPosition(Transform start)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("UnRegisterStartPosition:" + (object) start));
      NetworkManager.s_StartPositions.Remove(start);
    }

    /// <summary>
    ///   <para>This checks if the NetworkManager has a client and that it is connected to a server.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if the NetworkManagers client is connected to a server.</para>
    /// </returns>
    public bool IsClientConnected()
    {
      if (this.client != null)
        return this.client.isConnected;
      return false;
    }

    /// <summary>
    ///   <para>Shuts down the NetworkManager completely and destroy the singleton.</para>
    /// </summary>
    public static void Shutdown()
    {
      if ((UnityEngine.Object) NetworkManager.singleton == (UnityEngine.Object) null)
        return;
      NetworkManager.s_StartPositions.Clear();
      NetworkManager.s_StartPositionIndex = 0;
      NetworkManager.s_ClientReadyConnection = (NetworkConnection) null;
      NetworkManager.singleton.StopHost();
      NetworkManager.singleton = (NetworkManager) null;
    }

    internal void OnServerConnectInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnServerConnectInternal");
      netMsg.conn.SetMaxDelay(this.m_MaxDelay);
      if (NetworkManager.networkSceneName != string.Empty && NetworkManager.networkSceneName != this.m_OfflineScene)
      {
        StringMessage stringMessage = new StringMessage(NetworkManager.networkSceneName);
        netMsg.conn.Send((short) 39, (MessageBase) stringMessage);
      }
      if ((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null)
        this.m_MigrationManager.SendPeerInfo();
      this.OnServerConnect(netMsg.conn);
    }

    internal void OnServerDisconnectInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnServerDisconnectInternal");
      if ((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null)
        this.m_MigrationManager.SendPeerInfo();
      this.OnServerDisconnect(netMsg.conn);
    }

    internal void OnServerReadyMessageInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnServerReadyMessageInternal");
      this.OnServerReady(netMsg.conn);
    }

    internal void OnServerAddPlayerMessageInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnServerAddPlayerMessageInternal");
      netMsg.ReadMessage<AddPlayerMessage>(NetworkManager.s_AddPlayerMessage);
      if (NetworkManager.s_AddPlayerMessage.msgSize != 0)
      {
        NetworkReader extraMessageReader = new NetworkReader(NetworkManager.s_AddPlayerMessage.msgData);
        this.OnServerAddPlayer(netMsg.conn, NetworkManager.s_AddPlayerMessage.playerControllerId, extraMessageReader);
      }
      else
        this.OnServerAddPlayer(netMsg.conn, NetworkManager.s_AddPlayerMessage.playerControllerId);
      if (!((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null))
        return;
      this.m_MigrationManager.SendPeerInfo();
    }

    internal void OnServerRemovePlayerMessageInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnServerRemovePlayerMessageInternal");
      netMsg.ReadMessage<RemovePlayerMessage>(NetworkManager.s_RemovePlayerMessage);
      PlayerController playerController;
      netMsg.conn.GetPlayerController(NetworkManager.s_RemovePlayerMessage.playerControllerId, out playerController);
      this.OnServerRemovePlayer(netMsg.conn, playerController);
      netMsg.conn.RemovePlayerController(NetworkManager.s_RemovePlayerMessage.playerControllerId);
      if (!((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null))
        return;
      this.m_MigrationManager.SendPeerInfo();
    }

    internal void OnServerErrorInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnServerErrorInternal");
      netMsg.ReadMessage<ErrorMessage>(NetworkManager.s_ErrorMessage);
      this.OnServerError(netMsg.conn, NetworkManager.s_ErrorMessage.errorCode);
    }

    internal void OnClientConnectInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnClientConnectInternal");
      netMsg.conn.SetMaxDelay(this.m_MaxDelay);
      if (string.IsNullOrEmpty(this.m_OnlineScene) || this.m_OnlineScene == this.m_OfflineScene || SceneManager.GetSceneAt(0).name == this.m_OnlineScene)
      {
        this.m_ClientLoadedScene = false;
        this.OnClientConnect(netMsg.conn);
      }
      else
        NetworkManager.s_ClientReadyConnection = netMsg.conn;
    }

    internal void OnClientDisconnectInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnClientDisconnectInternal");
      if ((UnityEngine.Object) this.m_MigrationManager != (UnityEngine.Object) null && this.m_MigrationManager.LostHostOnClient(netMsg.conn))
        return;
      if (this.m_OfflineScene != string.Empty)
        this.ClientChangeScene(this.m_OfflineScene, false);
      this.OnClientDisconnect(netMsg.conn);
    }

    internal void OnClientNotReadyMessageInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnClientNotReadyMessageInternal");
      ClientScene.SetNotReady();
      this.OnClientNotReady(netMsg.conn);
    }

    internal void OnClientErrorInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnClientErrorInternal");
      netMsg.ReadMessage<ErrorMessage>(NetworkManager.s_ErrorMessage);
      this.OnClientError(netMsg.conn, NetworkManager.s_ErrorMessage.errorCode);
    }

    internal void OnClientSceneInternal(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager:OnClientSceneInternal");
      string newSceneName = netMsg.reader.ReadString();
      if (!this.IsClientConnected() || NetworkServer.active)
        return;
      this.ClientChangeScene(newSceneName, true);
    }

    /// <summary>
    ///   <para>Called on the server when a new client connects.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public virtual void OnServerConnect(NetworkConnection conn)
    {
    }

    /// <summary>
    ///   <para>Called on the server when a client disconnects.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public virtual void OnServerDisconnect(NetworkConnection conn)
    {
      NetworkServer.DestroyPlayersForConnection(conn);
    }

    /// <summary>
    ///   <para>Called on the server when a client is ready.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public virtual void OnServerReady(NetworkConnection conn)
    {
      if (conn.playerControllers.Count == 0 && LogFilter.logDebug)
        Debug.Log((object) "Ready with no player object");
      NetworkServer.SetClientReady(conn);
    }

    public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
      this.OnServerAddPlayerInternal(conn, playerControllerId);
    }

    /// <summary>
    ///   <para>Called on the server when a client adds a new player with ClientScene.AddPlayer.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    /// <param name="playerControllerId">Id of the new player.</param>
    /// <param name="extraMessageReader">An extra message object passed for the new player.</param>
    public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
      this.OnServerAddPlayerInternal(conn, playerControllerId);
    }

    private void OnServerAddPlayerInternal(NetworkConnection conn, short playerControllerId)
    {
      if ((UnityEngine.Object) this.m_PlayerPrefab == (UnityEngine.Object) null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object.");
      }
      else if ((UnityEngine.Object) this.m_PlayerPrefab.GetComponent<NetworkIdentity>() == (UnityEngine.Object) null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab.");
      }
      else if ((int) playerControllerId < conn.playerControllers.Count && conn.playerControllers[(int) playerControllerId].IsValid && (UnityEngine.Object) conn.playerControllers[(int) playerControllerId].gameObject != (UnityEngine.Object) null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "There is already a player at that playerControllerId for this connections.");
      }
      else
      {
        Transform startPosition = this.GetStartPosition();
        GameObject player = !((UnityEngine.Object) startPosition != (UnityEngine.Object) null) ? (GameObject) UnityEngine.Object.Instantiate((UnityEngine.Object) this.m_PlayerPrefab, Vector3.zero, Quaternion.identity) : (GameObject) UnityEngine.Object.Instantiate((UnityEngine.Object) this.m_PlayerPrefab, startPosition.position, startPosition.rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
      }
    }

    /// <summary>
    ///   <para>This finds a spawn position based on NetworkStartPosition objects in the scene.</para>
    /// </summary>
    /// <returns>
    ///   <para>Returns the transform to spawn a player at, or null.</para>
    /// </returns>
    public Transform GetStartPosition()
    {
      if (NetworkManager.s_StartPositions.Count > 0)
      {
        for (int index = NetworkManager.s_StartPositions.Count - 1; index >= 0; --index)
        {
          if ((UnityEngine.Object) NetworkManager.s_StartPositions[index] == (UnityEngine.Object) null)
            NetworkManager.s_StartPositions.RemoveAt(index);
        }
      }
      if (this.m_PlayerSpawnMethod == PlayerSpawnMethod.Random && NetworkManager.s_StartPositions.Count > 0)
      {
        int index = UnityEngine.Random.Range(0, NetworkManager.s_StartPositions.Count);
        return NetworkManager.s_StartPositions[index];
      }
      if (this.m_PlayerSpawnMethod != PlayerSpawnMethod.RoundRobin || NetworkManager.s_StartPositions.Count <= 0)
        return (Transform) null;
      if (NetworkManager.s_StartPositionIndex >= NetworkManager.s_StartPositions.Count)
        NetworkManager.s_StartPositionIndex = 0;
      Transform startPosition = NetworkManager.s_StartPositions[NetworkManager.s_StartPositionIndex];
      ++NetworkManager.s_StartPositionIndex;
      return startPosition;
    }

    /// <summary>
    ///   <para>Called on the server when a client removes a player.</para>
    /// </summary>
    /// <param name="conn">The connection to remove the player from.</param>
    /// <param name="player">The player controller to remove.</param>
    public virtual void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
      if (!((UnityEngine.Object) player.gameObject != (UnityEngine.Object) null))
        return;
      NetworkServer.Destroy(player.gameObject);
    }

    /// <summary>
    ///   <para>Called on the server when a network error occurs for a client connection.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    /// <param name="errorCode">Error code.</param>
    public virtual void OnServerError(NetworkConnection conn, int errorCode)
    {
    }

    /// <summary>
    ///   <para>Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().</para>
    /// </summary>
    /// <param name="sceneName">The name of the new scene.</param>
    public virtual void OnServerSceneChanged(string sceneName)
    {
    }

    /// <summary>
    ///   <para>Called on the client when connected to a server.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public virtual void OnClientConnect(NetworkConnection conn)
    {
      if (this.clientLoadedScene)
        return;
      ClientScene.Ready(conn);
      if (!this.m_AutoCreatePlayer)
        return;
      ClientScene.AddPlayer((short) 0);
    }

    /// <summary>
    ///   <para>Called on clients when disconnected from a server.</para>
    /// </summary>
    /// <param name="conn">Connection to the server.</param>
    public virtual void OnClientDisconnect(NetworkConnection conn)
    {
      this.StopClient();
    }

    /// <summary>
    ///   <para>Called on clients when a network error occurs.</para>
    /// </summary>
    /// <param name="conn">Connection to a server.</param>
    /// <param name="errorCode">Error code.</param>
    public virtual void OnClientError(NetworkConnection conn, int errorCode)
    {
    }

    /// <summary>
    ///   <para>Called on clients when a servers tells the client it is no longer ready.</para>
    /// </summary>
    /// <param name="conn">Connection to a server.</param>
    public virtual void OnClientNotReady(NetworkConnection conn)
    {
    }

    /// <summary>
    ///   <para>Called on clients when a scene has completed loaded, when the scene load was initiated by the server.</para>
    /// </summary>
    /// <param name="conn">The network connection that the scene change message arrived on.</param>
    public virtual void OnClientSceneChanged(NetworkConnection conn)
    {
      ClientScene.Ready(conn);
      if (!this.m_AutoCreatePlayer)
        return;
      bool flag1 = ClientScene.localPlayers.Count == 0;
      bool flag2 = false;
      using (List<PlayerController>.Enumerator enumerator = ClientScene.localPlayers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if ((UnityEngine.Object) enumerator.Current.gameObject != (UnityEngine.Object) null)
          {
            flag2 = true;
            break;
          }
        }
      }
      if (!flag2)
        flag1 = true;
      if (!flag1)
        return;
      ClientScene.AddPlayer((short) 0);
    }

    /// <summary>
    ///   <para>This starts matchmaker for the NetworkManager.</para>
    /// </summary>
    public void StartMatchMaker()
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager StartMatchMaker");
      this.SetMatchHost(this.m_MatchHost, this.m_MatchPort, true);
    }

    /// <summary>
    ///   <para>Stops the matchmaker that the NetworkManager is using.</para>
    /// </summary>
    public void StopMatchMaker()
    {
      if ((UnityEngine.Object) this.matchMaker != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.matchMaker);
        this.matchMaker = (NetworkMatch) null;
      }
      this.matchInfo = (MatchInfo) null;
      this.matches = (List<MatchDesc>) null;
    }

    /// <summary>
    ///   <para>This set the address of the matchmaker service.</para>
    /// </summary>
    /// <param name="newHost">Hostname of matchmaker service.</param>
    /// <param name="port">Port of matchmaker service.</param>
    /// <param name="https">Protocol used by matchmaker service.</param>
    public void SetMatchHost(string newHost, int port, bool https)
    {
      if ((UnityEngine.Object) this.matchMaker == (UnityEngine.Object) null)
        this.matchMaker = this.gameObject.AddComponent<NetworkMatch>();
      if (newHost == "localhost" || newHost == "127.0.0.1")
        newHost = Environment.MachineName;
      string str = "http://";
      if (https)
        str = "https://";
      if (LogFilter.logDebug)
        Debug.Log((object) ("SetMatchHost:" + newHost));
      this.m_MatchHost = newHost;
      this.m_MatchPort = port;
      this.matchMaker.baseUri = new Uri(str + this.m_MatchHost + ":" + (object) this.m_MatchPort);
    }

    /// <summary>
    ///   <para>This hook is invoked when a host is started.</para>
    /// </summary>
    public virtual void OnStartHost()
    {
    }

    /// <summary>
    ///   <para>This hook is invoked when a server is started - including when a host is started.</para>
    /// </summary>
    public virtual void OnStartServer()
    {
    }

    /// <summary>
    ///   <para>This is a hook that is invoked when the client is started.</para>
    /// </summary>
    /// <param name="client">The NetworkClient object that was started.</param>
    public virtual void OnStartClient(NetworkClient client)
    {
    }

    /// <summary>
    ///   <para>This hook is called when a server is stopped - including when a host is stopped.</para>
    /// </summary>
    public virtual void OnStopServer()
    {
    }

    /// <summary>
    ///   <para>This hook is called when a client is stopped.</para>
    /// </summary>
    public virtual void OnStopClient()
    {
    }

    /// <summary>
    ///   <para>This hook is called when a host is stopped.</para>
    /// </summary>
    public virtual void OnStopHost()
    {
    }

    /// <summary>
    ///   <para>This is invoked when a match has been created.</para>
    /// </summary>
    /// <param name="matchInfo">Info about the match that has been created.</param>
    public virtual void OnMatchCreate(CreateMatchResponse matchInfo)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("NetworkManager OnMatchCreate " + (object) matchInfo));
      if (matchInfo.success)
      {
        Utility.SetAccessTokenForNetwork(matchInfo.networkId, new NetworkAccessToken(matchInfo.accessTokenString));
        this.StartHost(new MatchInfo(matchInfo));
      }
      else
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Create Failed:" + (object) matchInfo));
      }
    }

    /// <summary>
    ///   <para>This is invoked when a list of matches is returned from ListMatches().</para>
    /// </summary>
    /// <param name="matchList">A list of available matches.</param>
    public virtual void OnMatchList(ListMatchResponse matchList)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager OnMatchList ");
      this.matches = matchList.matches;
    }

    /// <summary>
    ///   <para>This is invoked when a match is joined.</para>
    /// </summary>
    /// <param name="matchInfo"></param>
    public void OnMatchJoined(JoinMatchResponse matchInfo)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkManager OnMatchJoined ");
      if (matchInfo.success)
      {
        Utility.SetAccessTokenForNetwork(matchInfo.networkId, new NetworkAccessToken(matchInfo.accessTokenString));
        this.StartClient(new MatchInfo(matchInfo));
      }
      else
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Join Failed:" + (object) matchInfo));
      }
    }
  }
}
