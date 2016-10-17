// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkLobbyManager
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>This is a specialized NetworkManager that includes a networked lobby.</para>
  /// </summary>
  [AddComponentMenu("Network/NetworkLobbyManager")]
  public class NetworkLobbyManager : NetworkManager
  {
    private static LobbyReadyToBeginMessage s_ReadyToBeginMessage = new LobbyReadyToBeginMessage();
    private static IntegerMessage s_SceneLoadedMessage = new IntegerMessage();
    private static LobbyReadyToBeginMessage s_LobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();
    [SerializeField]
    private bool m_ShowLobbyGUI = true;
    [SerializeField]
    private int m_MaxPlayers = 4;
    [SerializeField]
    private int m_MaxPlayersPerConnection = 1;
    [SerializeField]
    private string m_LobbyScene = string.Empty;
    [SerializeField]
    private string m_PlayScene = string.Empty;
    private List<NetworkLobbyManager.PendingPlayer> m_PendingPlayers = new List<NetworkLobbyManager.PendingPlayer>();
    [SerializeField]
    private int m_MinPlayers;
    [SerializeField]
    private NetworkLobbyPlayer m_LobbyPlayerPrefab;
    [SerializeField]
    private GameObject m_GamePlayerPrefab;
    /// <summary>
    ///   <para>These slots track players that enter the lobby.</para>
    /// </summary>
    public NetworkLobbyPlayer[] lobbySlots;

    /// <summary>
    ///   <para>This flag enables display of the default lobby UI.</para>
    /// </summary>
    public bool showLobbyGUI
    {
      get
      {
        return this.m_ShowLobbyGUI;
      }
      set
      {
        this.m_ShowLobbyGUI = value;
      }
    }

    /// <summary>
    ///   <para>The maximum number of players allowed in the game.</para>
    /// </summary>
    public int maxPlayers
    {
      get
      {
        return this.m_MaxPlayers;
      }
      set
      {
        this.m_MaxPlayers = value;
      }
    }

    /// <summary>
    ///   <para>The maximum number of players per connection.</para>
    /// </summary>
    public int maxPlayersPerConnection
    {
      get
      {
        return this.m_MaxPlayersPerConnection;
      }
      set
      {
        this.m_MaxPlayersPerConnection = value;
      }
    }

    /// <summary>
    ///   <para>The minimum number of players required to be ready for the game to start.</para>
    /// </summary>
    public int minPlayers
    {
      get
      {
        return this.m_MinPlayers;
      }
      set
      {
        this.m_MinPlayers = value;
      }
    }

    /// <summary>
    ///   <para>This is the prefab of the player to be created in the LobbyScene.</para>
    /// </summary>
    public NetworkLobbyPlayer lobbyPlayerPrefab
    {
      get
      {
        return this.m_LobbyPlayerPrefab;
      }
      set
      {
        this.m_LobbyPlayerPrefab = value;
      }
    }

    /// <summary>
    ///   <para>This is the prefab of the player to be created in the PlayScene.</para>
    /// </summary>
    public GameObject gamePlayerPrefab
    {
      get
      {
        return this.m_GamePlayerPrefab;
      }
      set
      {
        this.m_GamePlayerPrefab = value;
      }
    }

    /// <summary>
    ///   <para>The scene to use for the lobby. This is similar to the offlineScene of the NetworkManager.</para>
    /// </summary>
    public string lobbyScene
    {
      get
      {
        return this.m_LobbyScene;
      }
      set
      {
        this.m_LobbyScene = value;
        this.offlineScene = value;
      }
    }

    /// <summary>
    ///   <para>The scene to use for the playing the game from the lobby. This is similar to the onlineScene of the NetworkManager.</para>
    /// </summary>
    public string playScene
    {
      get
      {
        return this.m_PlayScene;
      }
      set
      {
        this.m_PlayScene = value;
      }
    }

    private void OnValidate()
    {
      if (this.m_MaxPlayers <= 0)
        this.m_MaxPlayers = 1;
      if (this.m_MaxPlayersPerConnection <= 0)
        this.m_MaxPlayersPerConnection = 1;
      if (this.m_MaxPlayersPerConnection > this.maxPlayers)
        this.m_MaxPlayersPerConnection = this.maxPlayers;
      if (this.m_MinPlayers < 0)
        this.m_MinPlayers = 0;
      if (this.m_MinPlayers > this.m_MaxPlayers)
        this.m_MinPlayers = this.m_MaxPlayers;
      if ((Object) this.m_LobbyPlayerPrefab != (Object) null && (Object) this.m_LobbyPlayerPrefab.GetComponent<NetworkIdentity>() == (Object) null)
      {
        this.m_LobbyPlayerPrefab = (NetworkLobbyPlayer) null;
        Debug.LogWarning((object) "LobbyPlayer prefab must have a NetworkIdentity component.");
      }
      if (!((Object) this.m_GamePlayerPrefab != (Object) null) || !((Object) this.m_GamePlayerPrefab.GetComponent<NetworkIdentity>() == (Object) null))
        return;
      this.m_GamePlayerPrefab = (GameObject) null;
      Debug.LogWarning((object) "GamePlayer prefab must have a NetworkIdentity component.");
    }

    private byte FindSlot()
    {
      for (byte index = 0; (int) index < this.maxPlayers; ++index)
      {
        if ((Object) this.lobbySlots[(int) index] == (Object) null)
          return index;
      }
      return byte.MaxValue;
    }

    private void SceneLoadedForPlayer(NetworkConnection conn, GameObject lobbyPlayerGameObject)
    {
      if ((Object) lobbyPlayerGameObject.GetComponent<NetworkLobbyPlayer>() == (Object) null)
        return;
      string name = SceneManager.GetSceneAt(0).name;
      if (LogFilter.logDebug)
        Debug.Log((object) ("NetworkLobby SceneLoadedForPlayer scene:" + name + " " + (object) conn));
      if (name == this.m_LobbyScene)
      {
        NetworkLobbyManager.PendingPlayer pendingPlayer;
        pendingPlayer.conn = conn;
        pendingPlayer.lobbyPlayer = lobbyPlayerGameObject;
        this.m_PendingPlayers.Add(pendingPlayer);
      }
      else
      {
        short playerControllerId = lobbyPlayerGameObject.GetComponent<NetworkIdentity>().playerControllerId;
        GameObject gameObject = this.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
        if ((Object) gameObject == (Object) null)
        {
          Transform startPosition = this.GetStartPosition();
          gameObject = !((Object) startPosition != (Object) null) ? (GameObject) Object.Instantiate((Object) this.gamePlayerPrefab, Vector3.zero, Quaternion.identity) : (GameObject) Object.Instantiate((Object) this.gamePlayerPrefab, startPosition.position, startPosition.rotation);
        }
        if (!this.OnLobbyServerSceneLoadedForPlayer(lobbyPlayerGameObject, gameObject))
          return;
        NetworkServer.ReplacePlayerForConnection(conn, gameObject, playerControllerId);
      }
    }

    private static int CheckConnectionIsReadyToBegin(NetworkConnection conn)
    {
      int num = 0;
      using (List<PlayerController>.Enumerator enumerator = conn.playerControllers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          PlayerController current = enumerator.Current;
          if (current.IsValid && current.gameObject.GetComponent<NetworkLobbyPlayer>().readyToBegin)
            ++num;
        }
      }
      return num;
    }

    /// <summary>
    ///   <para>CheckReadyToBegin checks all of the players in the lobby to see if their readyToBegin flag is set.</para>
    /// </summary>
    public void CheckReadyToBegin()
    {
      if (SceneManager.GetSceneAt(0).name != this.m_LobbyScene)
        return;
      int num = 0;
      foreach (NetworkConnection connection in NetworkServer.connections)
      {
        if (connection != null)
          num += NetworkLobbyManager.CheckConnectionIsReadyToBegin(connection);
      }
      if (this.m_MinPlayers > 0 && num < this.m_MinPlayers)
        return;
      this.m_PendingPlayers.Clear();
      this.OnLobbyServerPlayersReady();
    }

    /// <summary>
    ///   <para>Calling this causes the server to switch back to the lobby scene.</para>
    /// </summary>
    public void ServerReturnToLobby()
    {
      if (!NetworkServer.active)
        Debug.Log((object) "ServerReturnToLobby called on client");
      else
        this.ServerChangeScene(this.m_LobbyScene);
    }

    private void CallOnClientEnterLobby()
    {
      this.OnLobbyClientEnter();
      foreach (NetworkLobbyPlayer lobbySlot in this.lobbySlots)
      {
        if (!((Object) lobbySlot == (Object) null))
        {
          lobbySlot.readyToBegin = false;
          lobbySlot.OnClientEnterLobby();
        }
      }
    }

    private void CallOnClientExitLobby()
    {
      this.OnLobbyClientExit();
      foreach (NetworkLobbyPlayer lobbySlot in this.lobbySlots)
      {
        if (!((Object) lobbySlot == (Object) null))
          lobbySlot.OnClientExitLobby();
      }
    }

    /// <summary>
    ///   <para>Sends a message to the server to make the game return to the lobby scene.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if message was sent.</para>
    /// </returns>
    public bool SendReturnToLobby()
    {
      if (this.client == null || !this.client.isConnected)
        return false;
      this.client.Send((short) 46, (MessageBase) new EmptyMessage());
      return true;
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
      if (this.numPlayers >= this.maxPlayers)
        conn.Disconnect();
      else if (SceneManager.GetSceneAt(0).name != this.m_LobbyScene)
      {
        conn.Disconnect();
      }
      else
      {
        base.OnServerConnect(conn);
        this.OnLobbyServerConnect(conn);
      }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
      base.OnServerDisconnect(conn);
      for (int index = 0; index < this.lobbySlots.Length; ++index)
      {
        NetworkLobbyPlayer lobbySlot = this.lobbySlots[index];
        if (!((Object) lobbySlot == (Object) null) && lobbySlot.connectionToClient == conn)
        {
          this.lobbySlots[index] = (NetworkLobbyPlayer) null;
          NetworkServer.Destroy(lobbySlot.gameObject);
        }
      }
      this.OnLobbyServerDisconnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
      if (SceneManager.GetSceneAt(0).name != this.m_LobbyScene)
        return;
      int num = 0;
      using (List<PlayerController>.Enumerator enumerator = conn.playerControllers.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.IsValid)
            ++num;
        }
      }
      if (num >= this.maxPlayersPerConnection)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) "NetworkLobbyManager no more players for this connection.");
        EmptyMessage emptyMessage = new EmptyMessage();
        conn.Send((short) 45, (MessageBase) emptyMessage);
      }
      else
      {
        byte slot = this.FindSlot();
        if ((int) slot == (int) byte.MaxValue)
        {
          if (LogFilter.logWarn)
            Debug.LogWarning((object) "NetworkLobbyManager no space for more players");
          EmptyMessage emptyMessage = new EmptyMessage();
          conn.Send((short) 45, (MessageBase) emptyMessage);
        }
        else
        {
          GameObject player = this.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
          if ((Object) player == (Object) null)
            player = (GameObject) Object.Instantiate((Object) this.lobbyPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
          NetworkLobbyPlayer component = player.GetComponent<NetworkLobbyPlayer>();
          component.slot = slot;
          this.lobbySlots[(int) slot] = component;
          NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
      }
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
      short playerControllerId = player.playerControllerId;
      this.lobbySlots[(int) player.gameObject.GetComponent<NetworkLobbyPlayer>().slot] = (NetworkLobbyPlayer) null;
      base.OnServerRemovePlayer(conn, player);
      foreach (NetworkLobbyPlayer lobbySlot in this.lobbySlots)
      {
        if ((Object) lobbySlot != (Object) null)
        {
          lobbySlot.GetComponent<NetworkLobbyPlayer>().readyToBegin = false;
          NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId = lobbySlot.slot;
          NetworkLobbyManager.s_LobbyReadyToBeginMessage.readyState = false;
          NetworkServer.SendToReady((GameObject) null, (short) 43, (MessageBase) NetworkLobbyManager.s_LobbyReadyToBeginMessage);
        }
      }
      this.OnLobbyServerPlayerRemoved(conn, playerControllerId);
    }

    public override void ServerChangeScene(string sceneName)
    {
      if (sceneName == this.m_LobbyScene)
      {
        foreach (NetworkLobbyPlayer lobbySlot in this.lobbySlots)
        {
          if (!((Object) lobbySlot == (Object) null))
          {
            NetworkIdentity component = lobbySlot.GetComponent<NetworkIdentity>();
            PlayerController playerController;
            if (component.connectionToClient.GetPlayerController(component.playerControllerId, out playerController))
              NetworkServer.Destroy(playerController.gameObject);
            if (NetworkServer.active)
            {
              lobbySlot.GetComponent<NetworkLobbyPlayer>().readyToBegin = false;
              NetworkServer.ReplacePlayerForConnection(component.connectionToClient, lobbySlot.gameObject, component.playerControllerId);
            }
          }
        }
      }
      base.ServerChangeScene(sceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
      if (sceneName != this.m_LobbyScene)
      {
        using (List<NetworkLobbyManager.PendingPlayer>.Enumerator enumerator = this.m_PendingPlayers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            NetworkLobbyManager.PendingPlayer current = enumerator.Current;
            this.SceneLoadedForPlayer(current.conn, current.lobbyPlayer);
          }
        }
        this.m_PendingPlayers.Clear();
      }
      this.OnLobbyServerSceneChanged(sceneName);
    }

    private void OnServerReadyToBeginMessage(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkLobbyManager OnServerReadyToBeginMessage");
      netMsg.ReadMessage<LobbyReadyToBeginMessage>(NetworkLobbyManager.s_ReadyToBeginMessage);
      PlayerController playerController;
      if (!netMsg.conn.GetPlayerController((short) NetworkLobbyManager.s_ReadyToBeginMessage.slotId, out playerController))
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("NetworkLobbyManager OnServerReadyToBeginMessage invalid playerControllerId " + (object) NetworkLobbyManager.s_ReadyToBeginMessage.slotId));
      }
      else
      {
        NetworkLobbyPlayer component = playerController.gameObject.GetComponent<NetworkLobbyPlayer>();
        component.readyToBegin = NetworkLobbyManager.s_ReadyToBeginMessage.readyState;
        NetworkServer.SendToReady((GameObject) null, (short) 43, (MessageBase) new LobbyReadyToBeginMessage()
        {
          slotId = component.slot,
          readyState = NetworkLobbyManager.s_ReadyToBeginMessage.readyState
        });
        this.CheckReadyToBegin();
      }
    }

    private void OnServerSceneLoadedMessage(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkLobbyManager OnSceneLoadedMessage");
      netMsg.ReadMessage<IntegerMessage>(NetworkLobbyManager.s_SceneLoadedMessage);
      PlayerController playerController;
      if (!netMsg.conn.GetPlayerController((short) NetworkLobbyManager.s_SceneLoadedMessage.value, out playerController))
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("NetworkLobbyManager OnServerSceneLoadedMessage invalid playerControllerId " + (object) NetworkLobbyManager.s_SceneLoadedMessage.value));
      }
      else
        this.SceneLoadedForPlayer(netMsg.conn, playerController.gameObject);
    }

    private void OnServerReturnToLobbyMessage(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkLobbyManager OnServerReturnToLobbyMessage");
      this.ServerReturnToLobby();
    }

    public override void OnStartServer()
    {
      if (string.IsNullOrEmpty(this.m_LobbyScene))
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "NetworkLobbyManager LobbyScene is empty. Set the LobbyScene in the inspector for the NetworkLobbyMangaer");
      }
      else if (string.IsNullOrEmpty(this.m_PlayScene))
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "NetworkLobbyManager PlayScene is empty. Set the PlayScene in the inspector for the NetworkLobbyMangaer");
      }
      else
      {
        if (this.lobbySlots.Length == 0)
          this.lobbySlots = new NetworkLobbyPlayer[this.maxPlayers];
        NetworkServer.RegisterHandler((short) 43, new NetworkMessageDelegate(this.OnServerReadyToBeginMessage));
        NetworkServer.RegisterHandler((short) 44, new NetworkMessageDelegate(this.OnServerSceneLoadedMessage));
        NetworkServer.RegisterHandler((short) 46, new NetworkMessageDelegate(this.OnServerReturnToLobbyMessage));
        this.OnLobbyStartServer();
      }
    }

    public override void OnStartHost()
    {
      this.OnLobbyStartHost();
    }

    public override void OnStopHost()
    {
      this.OnLobbyStopHost();
    }

    public override void OnStartClient(NetworkClient lobbyClient)
    {
      if (this.lobbySlots.Length == 0)
        this.lobbySlots = new NetworkLobbyPlayer[this.maxPlayers];
      if ((Object) this.m_LobbyPlayerPrefab == (Object) null || (Object) this.m_LobbyPlayerPrefab.gameObject == (Object) null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkLobbyManager no LobbyPlayer prefab is registered. Please add a LobbyPlayer prefab.");
      }
      else
        ClientScene.RegisterPrefab(this.m_LobbyPlayerPrefab.gameObject);
      if ((Object) this.m_GamePlayerPrefab == (Object) null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkLobbyManager no GamePlayer prefab is registered. Please add a GamePlayer prefab.");
      }
      else
        ClientScene.RegisterPrefab(this.m_GamePlayerPrefab);
      lobbyClient.RegisterHandler((short) 43, new NetworkMessageDelegate(this.OnClientReadyToBegin));
      lobbyClient.RegisterHandler((short) 45, new NetworkMessageDelegate(this.OnClientAddPlayerFailedMessage));
      this.OnLobbyStartClient(lobbyClient);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
      this.OnLobbyClientConnect(conn);
      this.CallOnClientEnterLobby();
      base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
      this.OnLobbyClientDisconnect(conn);
      base.OnClientDisconnect(conn);
    }

    public override void OnStopClient()
    {
      this.OnLobbyStopClient();
      this.CallOnClientExitLobby();
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
      if (SceneManager.GetSceneAt(0).name == this.m_LobbyScene)
      {
        if (this.client.isConnected)
          this.CallOnClientEnterLobby();
      }
      else
        this.CallOnClientExitLobby();
      base.OnClientSceneChanged(conn);
      this.OnLobbyClientSceneChanged(conn);
    }

    private void OnClientReadyToBegin(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<LobbyReadyToBeginMessage>(NetworkLobbyManager.s_LobbyReadyToBeginMessage);
      if ((int) NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId >= ((IEnumerable<NetworkLobbyPlayer>) this.lobbySlots).Count<NetworkLobbyPlayer>())
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("NetworkLobbyManager OnClientReadyToBegin invalid lobby slot " + (object) NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId));
      }
      else
      {
        NetworkLobbyPlayer lobbySlot = this.lobbySlots[(int) NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId];
        if ((Object) lobbySlot == (Object) null || (Object) lobbySlot.gameObject == (Object) null)
        {
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) ("NetworkLobbyManager OnClientReadyToBegin no player at lobby slot " + (object) NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId));
        }
        else
        {
          lobbySlot.readyToBegin = NetworkLobbyManager.s_LobbyReadyToBeginMessage.readyState;
          lobbySlot.OnClientReady(NetworkLobbyManager.s_LobbyReadyToBeginMessage.readyState);
        }
      }
    }

    private void OnClientAddPlayerFailedMessage(NetworkMessage netMsg)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) "NetworkLobbyManager Add Player failed.");
      this.OnLobbyClientAddPlayerFailed();
    }

    /// <summary>
    ///   <para>This is called on the host when a host is started.</para>
    /// </summary>
    public virtual void OnLobbyStartHost()
    {
    }

    /// <summary>
    ///   <para>This is called on the host when the host is stopped.</para>
    /// </summary>
    public virtual void OnLobbyStopHost()
    {
    }

    /// <summary>
    ///   <para>This is called on the server when the server is started - including when a host is started.</para>
    /// </summary>
    public virtual void OnLobbyStartServer()
    {
    }

    /// <summary>
    ///   <para>This is called on the server when a new client connects to the server.</para>
    /// </summary>
    /// <param name="conn">The new connection.</param>
    public virtual void OnLobbyServerConnect(NetworkConnection conn)
    {
    }

    /// <summary>
    ///   <para>This is called on the server when a client disconnects.</para>
    /// </summary>
    /// <param name="conn">The connection that disconnected.</param>
    public virtual void OnLobbyServerDisconnect(NetworkConnection conn)
    {
    }

    /// <summary>
    ///   <para>This is called on the server when a networked scene finishes loading.</para>
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public virtual void OnLobbyServerSceneChanged(string sceneName)
    {
    }

    /// <summary>
    ///   <para>This allows customization of the creation of the lobby-player object on the server.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    /// <param name="playerControllerId">The controllerId of the player.</param>
    /// <returns>
    ///   <para>The new lobby-player object.</para>
    /// </returns>
    public virtual GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
      return (GameObject) null;
    }

    /// <summary>
    ///   <para>This allows customization of the creation of the GamePlayer object on the server.</para>
    /// </summary>
    /// <param name="conn">The connection the player object is for.</param>
    /// <param name="playerControllerId">The controllerId of the player on the connnection.</param>
    /// <returns>
    ///   <para>A new GamePlayer object.</para>
    /// </returns>
    public virtual GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
      return (GameObject) null;
    }

    /// <summary>
    ///   <para>This is called on the server when a player is removed.</para>
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="playerControllerId"></param>
    public virtual void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
    {
    }

    /// <summary>
    ///   <para>This is called on the server when it is told that a client has finished switching from the lobby scene to a game player scene.</para>
    /// </summary>
    /// <param name="lobbyPlayer">The lobby player object.</param>
    /// <param name="gamePlayer">The game player object.</param>
    /// <returns>
    ///   <para>False to not allow this player to replace the lobby player.</para>
    /// </returns>
    public virtual bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
      return true;
    }

    /// <summary>
    ///   <para>This is called on the server when all the players in the lobby are ready.</para>
    /// </summary>
    public virtual void OnLobbyServerPlayersReady()
    {
      this.ServerChangeScene(this.m_PlayScene);
    }

    /// <summary>
    ///   <para>This is a hook to allow custom behaviour when the game client enters the lobby.</para>
    /// </summary>
    public virtual void OnLobbyClientEnter()
    {
    }

    /// <summary>
    ///   <para>This is a hook to allow custom behaviour when the game client exits the lobby.</para>
    /// </summary>
    public virtual void OnLobbyClientExit()
    {
    }

    /// <summary>
    ///   <para>This is called on the client when it connects to server.</para>
    /// </summary>
    /// <param name="conn">The connection that connected.</param>
    public virtual void OnLobbyClientConnect(NetworkConnection conn)
    {
    }

    /// <summary>
    ///   <para>This is called on the client when disconnected from a server.</para>
    /// </summary>
    /// <param name="conn">The connection that disconnected.</param>
    public virtual void OnLobbyClientDisconnect(NetworkConnection conn)
    {
    }

    /// <summary>
    ///   <para>This is called on the client when a client is started.</para>
    /// </summary>
    /// <param name="lobbyClient"></param>
    public virtual void OnLobbyStartClient(NetworkClient lobbyClient)
    {
    }

    /// <summary>
    ///   <para>This is called on the client when the client stops.</para>
    /// </summary>
    public virtual void OnLobbyStopClient()
    {
    }

    /// <summary>
    ///   <para>This is called on the client when the client is finished loading a new networked scene.</para>
    /// </summary>
    /// <param name="conn"></param>
    public virtual void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
    }

    /// <summary>
    ///   <para>Called on the client when adding a player to the lobby fails.</para>
    /// </summary>
    public virtual void OnLobbyClientAddPlayerFailed()
    {
    }

    private void OnGUI()
    {
      if (!this.showLobbyGUI || SceneManager.GetSceneAt(0).name != this.m_LobbyScene)
        return;
      GUI.Box(new Rect(90f, 180f, 500f, 150f), "Players:");
      if (!NetworkClient.active || !GUI.Button(new Rect(100f, 300f, 120f, 20f), "Add Player"))
        return;
      this.TryToAddPlayer();
    }

    /// <summary>
    ///   <para>This is used on clients to attempt to add a player to the game.</para>
    /// </summary>
    public void TryToAddPlayer()
    {
      if (NetworkClient.active)
      {
        short playerControllerId = -1;
        List<PlayerController> playerControllers = NetworkClient.allClients[0].connection.playerControllers;
        if (playerControllers.Count < this.maxPlayers)
        {
          playerControllerId = (short) playerControllers.Count;
        }
        else
        {
          for (short index = 0; (int) index < this.maxPlayers; ++index)
          {
            if (!playerControllers[(int) index].IsValid)
            {
              playerControllerId = index;
              break;
            }
          }
        }
        if (LogFilter.logDebug)
          Debug.Log((object) ("NetworkLobbyManager TryToAddPlayer controllerId " + (object) playerControllerId + " ready:" + (object) ClientScene.ready));
        if ((int) playerControllerId == -1)
        {
          if (!LogFilter.logDebug)
            return;
          Debug.Log((object) "NetworkLobbyManager No Space!");
        }
        else if (ClientScene.ready)
          ClientScene.AddPlayer(playerControllerId);
        else
          ClientScene.AddPlayer(NetworkClient.allClients[0].connection, playerControllerId);
      }
      else
      {
        if (!LogFilter.logDebug)
          return;
        Debug.Log((object) "NetworkLobbyManager NetworkClient not active!");
      }
    }

    private struct PendingPlayer
    {
      public NetworkConnection conn;
      public GameObject lobbyPlayer;
    }
  }
}
