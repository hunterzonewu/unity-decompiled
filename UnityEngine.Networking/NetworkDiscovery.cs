// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkDiscovery
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>The NetworkDiscovery component allows Unity games to find each other on a local network. It can broadcast presence and listen for broadcasts, and optionally join matching games using the NetworkManager.</para>
  /// </summary>
  [AddComponentMenu("Network/NetworkDiscovery")]
  [DisallowMultipleComponent]
  public class NetworkDiscovery : MonoBehaviour
  {
    [SerializeField]
    private int m_BroadcastPort = 47777;
    [SerializeField]
    private int m_BroadcastKey = 2222;
    [SerializeField]
    private int m_BroadcastVersion = 1;
    [SerializeField]
    private int m_BroadcastSubVersion = 1;
    [SerializeField]
    private int m_BroadcastInterval = 1000;
    [SerializeField]
    private bool m_UseNetworkManager = true;
    [SerializeField]
    private string m_BroadcastData = "HELLO";
    [SerializeField]
    private bool m_ShowGUI = true;
    private int m_HostId = -1;
    private const int k_MaxBroadcastMsgSize = 1024;
    [SerializeField]
    private int m_OffsetX;
    [SerializeField]
    private int m_OffsetY;
    private bool m_Running;
    private bool m_IsServer;
    private bool m_IsClient;
    private byte[] m_MsgOutBuffer;
    private byte[] m_MsgInBuffer;
    private HostTopology m_DefaultTopology;
    private Dictionary<string, NetworkBroadcastResult> m_BroadcastsReceived;

    /// <summary>
    ///   <para>The network port to broadcast on and listen to.</para>
    /// </summary>
    public int broadcastPort
    {
      get
      {
        return this.m_BroadcastPort;
      }
      set
      {
        this.m_BroadcastPort = value;
      }
    }

    /// <summary>
    ///   <para>A key to identify this application in broadcasts.</para>
    /// </summary>
    public int broadcastKey
    {
      get
      {
        return this.m_BroadcastKey;
      }
      set
      {
        this.m_BroadcastKey = value;
      }
    }

    /// <summary>
    ///   <para>The version of the application to broadcast. This is used to match versions of the same application.</para>
    /// </summary>
    public int broadcastVersion
    {
      get
      {
        return this.m_BroadcastVersion;
      }
      set
      {
        this.m_BroadcastVersion = value;
      }
    }

    /// <summary>
    ///   <para>The sub-version of the application to broadcast. This is used to match versions of the same application.</para>
    /// </summary>
    public int broadcastSubVersion
    {
      get
      {
        return this.m_BroadcastSubVersion;
      }
      set
      {
        this.m_BroadcastSubVersion = value;
      }
    }

    /// <summary>
    ///   <para>How often in milliseconds to broadcast when running as a server.</para>
    /// </summary>
    public int broadcastInterval
    {
      get
      {
        return this.m_BroadcastInterval;
      }
      set
      {
        this.m_BroadcastInterval = value;
      }
    }

    /// <summary>
    ///   <para>True to integrate with the NetworkManager.</para>
    /// </summary>
    public bool useNetworkManager
    {
      get
      {
        return this.m_UseNetworkManager;
      }
      set
      {
        this.m_UseNetworkManager = value;
      }
    }

    /// <summary>
    ///   <para>The data to include in the broadcast message when running as a server.</para>
    /// </summary>
    public string broadcastData
    {
      get
      {
        return this.m_BroadcastData;
      }
      set
      {
        this.m_BroadcastData = value;
        this.m_MsgOutBuffer = NetworkDiscovery.StringToBytes(this.m_BroadcastData);
        if (!this.m_UseNetworkManager || !LogFilter.logWarn)
          return;
        Debug.LogWarning((object) "NetworkDiscovery broadcast data changed while using NetworkManager. This can prevent clients from finding the server. The format of the broadcast data must be 'NetworkManager:IPAddress:Port'.");
      }
    }

    /// <summary>
    ///   <para>True to draw the default Broacast control UI.</para>
    /// </summary>
    public bool showGUI
    {
      get
      {
        return this.m_ShowGUI;
      }
      set
      {
        this.m_ShowGUI = value;
      }
    }

    /// <summary>
    ///   <para>The horizontal offset of the GUI if active.</para>
    /// </summary>
    public int offsetX
    {
      get
      {
        return this.m_OffsetX;
      }
      set
      {
        this.m_OffsetX = value;
      }
    }

    /// <summary>
    ///   <para>The vertical offset of the GUI if active.</para>
    /// </summary>
    public int offsetY
    {
      get
      {
        return this.m_OffsetY;
      }
      set
      {
        this.m_OffsetY = value;
      }
    }

    /// <summary>
    ///   <para>The TransportLayer hostId being used (read-only).</para>
    /// </summary>
    public int hostId
    {
      get
      {
        return this.m_HostId;
      }
      set
      {
        this.m_HostId = value;
      }
    }

    /// <summary>
    ///   <para>True is broadcasting or listening (read-only).</para>
    /// </summary>
    public bool running
    {
      get
      {
        return this.m_Running;
      }
      set
      {
        this.m_Running = value;
      }
    }

    /// <summary>
    ///   <para>True if running in server mode (read-only).</para>
    /// </summary>
    public bool isServer
    {
      get
      {
        return this.m_IsServer;
      }
      set
      {
        this.m_IsServer = value;
      }
    }

    /// <summary>
    ///   <para>True if running in client mode (read-only).</para>
    /// </summary>
    public bool isClient
    {
      get
      {
        return this.m_IsClient;
      }
      set
      {
        this.m_IsClient = value;
      }
    }

    /// <summary>
    ///   <para>A dictionary of broadcasts received from servers.</para>
    /// </summary>
    public Dictionary<string, NetworkBroadcastResult> broadcastsReceived
    {
      get
      {
        return this.m_BroadcastsReceived;
      }
    }

    private static byte[] StringToBytes(string str)
    {
      byte[] numArray = new byte[str.Length * 2];
      Buffer.BlockCopy((Array) str.ToCharArray(), 0, (Array) numArray, 0, numArray.Length);
      return numArray;
    }

    private static string BytesToString(byte[] bytes)
    {
      char[] chArray = new char[bytes.Length / 2];
      Buffer.BlockCopy((Array) bytes, 0, (Array) chArray, 0, bytes.Length);
      return new string(chArray);
    }

    /// <summary>
    ///   <para>Initializes the NetworkDiscovery component.</para>
    /// </summary>
    /// <returns>
    ///   <para>Return true if the network port was available.</para>
    /// </returns>
    public bool Initialize()
    {
      if (this.m_BroadcastData.Length >= 1024)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("NetworkDiscovery Initialize - data too large. max is " + (object) 1024));
        return false;
      }
      if (!NetworkTransport.IsStarted)
        NetworkTransport.Init();
      if (this.m_UseNetworkManager && (UnityEngine.Object) NetworkManager.singleton != (UnityEngine.Object) null)
      {
        this.m_BroadcastData = "NetworkManager:" + NetworkManager.singleton.networkAddress + ":" + (object) NetworkManager.singleton.networkPort;
        if (LogFilter.logInfo)
          Debug.Log((object) ("NetwrokDiscovery set broadbast data to:" + this.m_BroadcastData));
      }
      this.m_MsgOutBuffer = NetworkDiscovery.StringToBytes(this.m_BroadcastData);
      this.m_MsgInBuffer = new byte[1024];
      this.m_BroadcastsReceived = new Dictionary<string, NetworkBroadcastResult>();
      ConnectionConfig defaultConfig = new ConnectionConfig();
      int num = (int) defaultConfig.AddChannel(QosType.Unreliable);
      this.m_DefaultTopology = new HostTopology(defaultConfig, 1);
      if (this.m_IsServer)
        this.StartAsServer();
      if (this.m_IsClient)
        this.StartAsClient();
      return true;
    }

    /// <summary>
    ///   <para>Starts listening for broadcasts messages.</para>
    /// </summary>
    /// <returns>
    ///   <para>True is able to listen.</para>
    /// </returns>
    public bool StartAsClient()
    {
      if (this.m_HostId != -1 || this.m_Running)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) "NetworkDiscovery StartAsClient already started");
        return false;
      }
      this.m_HostId = NetworkTransport.AddHost(this.m_DefaultTopology, this.m_BroadcastPort);
      if (this.m_HostId == -1)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkDiscovery StartAsClient - addHost failed");
        return false;
      }
      byte error;
      NetworkTransport.SetBroadcastCredentials(this.m_HostId, this.m_BroadcastKey, this.m_BroadcastVersion, this.m_BroadcastSubVersion, out error);
      this.m_Running = true;
      this.m_IsClient = true;
      if (LogFilter.logDebug)
        Debug.Log((object) "StartAsClient Discovery listening");
      return true;
    }

    /// <summary>
    ///   <para>Starts sending broadcast messages.</para>
    /// </summary>
    /// <returns>
    ///   <para>True is able to broadcast.</para>
    /// </returns>
    public bool StartAsServer()
    {
      if (this.m_HostId != -1 || this.m_Running)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) "NetworkDiscovery StartAsServer already started");
        return false;
      }
      this.m_HostId = NetworkTransport.AddHost(this.m_DefaultTopology, 0);
      if (this.m_HostId == -1)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkDiscovery StartAsServer - addHost failed");
        return false;
      }
      byte error;
      if (!NetworkTransport.StartBroadcastDiscovery(this.m_HostId, this.m_BroadcastPort, this.m_BroadcastKey, this.m_BroadcastVersion, this.m_BroadcastSubVersion, this.m_MsgOutBuffer, this.m_MsgOutBuffer.Length, this.m_BroadcastInterval, out error))
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("NetworkDiscovery StartBroadcast failed err: " + (object) error));
        return false;
      }
      this.m_Running = true;
      this.m_IsServer = true;
      if (LogFilter.logDebug)
        Debug.Log((object) "StartAsServer Discovery broadcasting");
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
      return true;
    }

    /// <summary>
    ///   <para>Stops listening and broadcasting.</para>
    /// </summary>
    public void StopBroadcast()
    {
      if (this.m_HostId == -1)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "NetworkDiscovery StopBroadcast not initialized");
      }
      else if (!this.m_Running)
      {
        Debug.LogWarning((object) "NetworkDiscovery StopBroadcast not started");
      }
      else
      {
        if (this.m_IsServer)
          NetworkTransport.StopBroadcastDiscovery();
        NetworkTransport.RemoveHost(this.m_HostId);
        this.m_HostId = -1;
        this.m_Running = false;
        this.m_IsServer = false;
        this.m_IsClient = false;
        this.m_MsgInBuffer = (byte[]) null;
        this.m_BroadcastsReceived = (Dictionary<string, NetworkBroadcastResult>) null;
        if (!LogFilter.logDebug)
          return;
        Debug.Log((object) "Stopped Discovery broadcasting");
      }
    }

    private void Update()
    {
      if (this.m_HostId == -1 || this.m_IsServer)
        return;
      NetworkEventType fromHost;
      do
      {
        int connectionId;
        int channelId;
        int receivedSize;
        byte error;
        fromHost = NetworkTransport.ReceiveFromHost(this.m_HostId, out connectionId, out channelId, this.m_MsgInBuffer, 1024, out receivedSize, out error);
        if (fromHost == NetworkEventType.BroadcastEvent)
        {
          NetworkTransport.GetBroadcastConnectionMessage(this.m_HostId, this.m_MsgInBuffer, 1024, out receivedSize, out error);
          string address;
          int port;
          NetworkTransport.GetBroadcastConnectionInfo(this.m_HostId, out address, out port, out error);
          NetworkBroadcastResult networkBroadcastResult = new NetworkBroadcastResult();
          networkBroadcastResult.serverAddress = address;
          networkBroadcastResult.broadcastData = new byte[receivedSize];
          Buffer.BlockCopy((Array) this.m_MsgInBuffer, 0, (Array) networkBroadcastResult.broadcastData, 0, receivedSize);
          this.m_BroadcastsReceived[address] = networkBroadcastResult;
          this.OnReceivedBroadcast(address, NetworkDiscovery.BytesToString(this.m_MsgInBuffer));
        }
      }
      while (fromHost != NetworkEventType.Nothing);
    }

    private void OnDestroy()
    {
      if (this.m_IsServer && this.m_Running && this.m_HostId != -1)
      {
        NetworkTransport.StopBroadcastDiscovery();
        NetworkTransport.RemoveHost(this.m_HostId);
      }
      if (!this.m_IsClient || !this.m_Running || this.m_HostId == -1)
        return;
      NetworkTransport.RemoveHost(this.m_HostId);
    }

    /// <summary>
    ///   <para>This is a virtual function that can be implemented to handle broadcast messages when running as a client.</para>
    /// </summary>
    /// <param name="fromAddress">The IP address of the server.</param>
    /// <param name="data">The data broadcast by the server.</param>
    public virtual void OnReceivedBroadcast(string fromAddress, string data)
    {
    }

    private void OnGUI()
    {
      if (!this.m_ShowGUI)
        return;
      int num1 = 10 + this.m_OffsetX;
      int num2 = 40 + this.m_OffsetY;
      if (Application.platform == RuntimePlatform.WebGLPlayer)
        GUI.Box(new Rect((float) num1, (float) num2, 200f, 20f), "( WebGL cannot broadcast )");
      else if (this.m_MsgInBuffer == null)
      {
        if (!GUI.Button(new Rect((float) num1, (float) num2, 200f, 20f), "Initialize Broadcast"))
          return;
        this.Initialize();
      }
      else
      {
        string str = string.Empty;
        if (this.m_IsServer)
          str = " (server)";
        if (this.m_IsClient)
          str = " (client)";
        GUI.Label(new Rect((float) num1, (float) num2, 200f, 20f), "initialized" + str);
        int num3 = num2 + 24;
        if (this.m_Running)
        {
          if (GUI.Button(new Rect((float) num1, (float) num3, 200f, 20f), "Stop"))
            this.StopBroadcast();
          int num4 = num3 + 24;
          if (this.m_BroadcastsReceived == null)
            return;
          using (Dictionary<string, NetworkBroadcastResult>.KeyCollection.Enumerator enumerator = this.m_BroadcastsReceived.Keys.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              NetworkBroadcastResult networkBroadcastResult = this.m_BroadcastsReceived[current];
              if (GUI.Button(new Rect((float) num1, (float) (num4 + 20), 200f, 20f), "Game at " + current) && this.m_UseNetworkManager)
              {
                string[] strArray = NetworkDiscovery.BytesToString(networkBroadcastResult.broadcastData).Split(':');
                if (strArray.Length == 3 && strArray[0] == "NetworkManager" && ((UnityEngine.Object) NetworkManager.singleton != (UnityEngine.Object) null && NetworkManager.singleton.client == null))
                {
                  NetworkManager.singleton.networkAddress = strArray[1];
                  NetworkManager.singleton.networkPort = Convert.ToInt32(strArray[2]);
                  NetworkManager.singleton.StartClient();
                }
              }
              num4 += 24;
            }
          }
        }
        else
        {
          if (GUI.Button(new Rect((float) num1, (float) num3, 200f, 20f), "Start Broadcasting"))
            this.StartAsServer();
          int num4 = num3 + 24;
          if (GUI.Button(new Rect((float) num1, (float) num4, 200f, 20f), "Listen for Broadcast"))
            this.StartAsClient();
          int num5 = num4 + 24;
        }
      }
    }
  }
}
