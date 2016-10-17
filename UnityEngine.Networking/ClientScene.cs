// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ClientScene
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A client manager which contains non-instance centrict client information and functions.</para>
  /// </summary>
  public class ClientScene
  {
    private static List<PlayerController> s_LocalPlayers = new List<PlayerController>();
    private static NetworkScene s_NetworkScene = new NetworkScene();
    private static ObjectSpawnSceneMessage s_ObjectSpawnSceneMessage = new ObjectSpawnSceneMessage();
    private static ObjectSpawnFinishedMessage s_ObjectSpawnFinishedMessage = new ObjectSpawnFinishedMessage();
    private static ObjectDestroyMessage s_ObjectDestroyMessage = new ObjectDestroyMessage();
    private static ObjectSpawnMessage s_ObjectSpawnMessage = new ObjectSpawnMessage();
    private static OwnerMessage s_OwnerMessage = new OwnerMessage();
    private static ClientAuthorityMessage s_ClientAuthorityMessage = new ClientAuthorityMessage();
    private static int s_ReconnectId = -1;
    private static List<ClientScene.PendingOwner> s_PendingOwnerIds = new List<ClientScene.PendingOwner>();
    /// <summary>
    ///   <para>An invalid reconnect Id.</para>
    /// </summary>
    public const int ReconnectIdInvalid = -1;
    /// <summary>
    ///   <para>A constant ID used by the old host when it reconnects to the new host.</para>
    /// </summary>
    public const int ReconnectIdHost = 0;
    private static NetworkConnection s_ReadyConnection;
    private static Dictionary<NetworkSceneId, NetworkIdentity> s_SpawnableObjects;
    private static bool s_IsReady;
    private static bool s_IsSpawnFinished;
    private static PeerInfoMessage[] s_Peers;

    /// <summary>
    ///   <para>A list of all players added to the game.</para>
    /// </summary>
    public static List<PlayerController> localPlayers
    {
      get
      {
        return ClientScene.s_LocalPlayers;
      }
    }

    /// <summary>
    ///   <para>Returns true when a client's connection has been set to ready.</para>
    /// </summary>
    public static bool ready
    {
      get
      {
        return ClientScene.s_IsReady;
      }
    }

    /// <summary>
    ///   <para>The NetworkConnection object that is currently "ready". This is the connection being used connect to the server where objects are spawned from.</para>
    /// </summary>
    public static NetworkConnection readyConnection
    {
      get
      {
        return ClientScene.s_ReadyConnection;
      }
    }

    /// <summary>
    ///   <para>The reconnectId to use when a client reconnects to the new host of a game after the old host was lost.</para>
    /// </summary>
    public static int reconnectId
    {
      get
      {
        return ClientScene.s_ReconnectId;
      }
    }

    /// <summary>
    ///   <para>This is a dictionary of networked objects that have been spawned on the client.</para>
    /// </summary>
    public static Dictionary<NetworkInstanceId, NetworkIdentity> objects
    {
      get
      {
        return ClientScene.s_NetworkScene.localObjects;
      }
    }

    /// <summary>
    ///   <para>This is a dictionary of the prefabs that are registered on the client.</para>
    /// </summary>
    public static Dictionary<NetworkHash128, GameObject> prefabs
    {
      get
      {
        return NetworkScene.guidToPrefab;
      }
    }

    /// <summary>
    ///   <para>This is dictionary of the disabled NetworkIdentity objects in the scene that could be spawned by messages from the server.</para>
    /// </summary>
    public static Dictionary<NetworkSceneId, NetworkIdentity> spawnableObjects
    {
      get
      {
        return ClientScene.s_SpawnableObjects;
      }
    }

    /// <summary>
    ///   <para>Sets the Id that the ClientScene will use when reconnecting to a new host after host migration.</para>
    /// </summary>
    /// <param name="newReconnectId">The Id to use when reconnecting to a game.</param>
    /// <param name="peers">The set of known peers in the game. This may be null.</param>
    public static void SetReconnectId(int newReconnectId, PeerInfoMessage[] peers)
    {
      ClientScene.s_ReconnectId = newReconnectId;
      ClientScene.s_Peers = peers;
      if (!LogFilter.logDebug)
        return;
      Debug.Log((object) ("ClientScene::SetReconnectId: " + (object) newReconnectId));
    }

    internal static void SetNotReady()
    {
      ClientScene.s_IsReady = false;
    }

    internal static void Shutdown()
    {
      ClientScene.s_NetworkScene.Shutdown();
      ClientScene.s_LocalPlayers = new List<PlayerController>();
      ClientScene.s_PendingOwnerIds = new List<ClientScene.PendingOwner>();
      ClientScene.s_SpawnableObjects = (Dictionary<NetworkSceneId, NetworkIdentity>) null;
      ClientScene.s_ReadyConnection = (NetworkConnection) null;
      ClientScene.s_IsReady = false;
      ClientScene.s_IsSpawnFinished = false;
      ClientScene.s_ReconnectId = -1;
      NetworkTransport.Shutdown();
      NetworkTransport.Init();
    }

    internal static bool GetPlayerController(short playerControllerId, out PlayerController player)
    {
      player = (PlayerController) null;
      if ((int) playerControllerId >= ClientScene.localPlayers.Count)
      {
        if (LogFilter.logWarn)
          Debug.Log((object) ("ClientScene::GetPlayer: no local player found for: " + (object) playerControllerId));
        return false;
      }
      if (ClientScene.localPlayers[(int) playerControllerId] == null)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) ("ClientScene::GetPlayer: local player is null for: " + (object) playerControllerId));
        return false;
      }
      player = ClientScene.localPlayers[(int) playerControllerId];
      return (Object) player.gameObject != (Object) null;
    }

    internal static void InternalAddPlayer(NetworkIdentity view, short playerControllerId)
    {
      if (LogFilter.logDebug)
        Debug.LogWarning((object) ("ClientScene::InternalAddPlayer: playerControllerId : " + (object) playerControllerId));
      if ((int) playerControllerId >= ClientScene.s_LocalPlayers.Count)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) ("ClientScene::InternalAddPlayer: playerControllerId higher than expected: " + (object) playerControllerId));
        while ((int) playerControllerId >= ClientScene.s_LocalPlayers.Count)
          ClientScene.s_LocalPlayers.Add(new PlayerController());
      }
      PlayerController player = new PlayerController() { gameObject = view.gameObject, playerControllerId = playerControllerId, unetView = view };
      ClientScene.s_LocalPlayers[(int) playerControllerId] = player;
      ClientScene.s_ReadyConnection.SetPlayerController(player);
    }

    /// <summary>
    ///   <para>This adds a player object for this client. This causes an AddPlayer message to be sent to the server, and NetworkManager.OnServerAddPlayer will be called. If an extra message was passed to AddPlayer, then OnServerAddPlayer will be called with a NetworkReader that contains the contents of the message.</para>
    /// </summary>
    /// <param name="readyConn">The connection to become ready for this client.</param>
    /// <param name="playerControllerId">The local player ID number.</param>
    /// <param name="extraMessage">An extra message object that can be passed to the server for this player.</param>
    /// <returns>
    ///   <para>True if player was added.</para>
    /// </returns>
    public static bool AddPlayer(short playerControllerId)
    {
      return ClientScene.AddPlayer((NetworkConnection) null, playerControllerId);
    }

    /// <summary>
    ///   <para>This adds a player object for this client. This causes an AddPlayer message to be sent to the server, and NetworkManager.OnServerAddPlayer will be called. If an extra message was passed to AddPlayer, then OnServerAddPlayer will be called with a NetworkReader that contains the contents of the message.</para>
    /// </summary>
    /// <param name="readyConn">The connection to become ready for this client.</param>
    /// <param name="playerControllerId">The local player ID number.</param>
    /// <param name="extraMessage">An extra message object that can be passed to the server for this player.</param>
    /// <returns>
    ///   <para>True if player was added.</para>
    /// </returns>
    public static bool AddPlayer(NetworkConnection readyConn, short playerControllerId)
    {
      return ClientScene.AddPlayer(readyConn, playerControllerId, (MessageBase) null);
    }

    /// <summary>
    ///   <para>This adds a player object for this client. This causes an AddPlayer message to be sent to the server, and NetworkManager.OnServerAddPlayer will be called. If an extra message was passed to AddPlayer, then OnServerAddPlayer will be called with a NetworkReader that contains the contents of the message.</para>
    /// </summary>
    /// <param name="readyConn">The connection to become ready for this client.</param>
    /// <param name="playerControllerId">The local player ID number.</param>
    /// <param name="extraMessage">An extra message object that can be passed to the server for this player.</param>
    /// <returns>
    ///   <para>True if player was added.</para>
    /// </returns>
    public static bool AddPlayer(NetworkConnection readyConn, short playerControllerId, MessageBase extraMessage)
    {
      if ((int) playerControllerId < 0)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("ClientScene::AddPlayer: playerControllerId of " + (object) playerControllerId + " is negative"));
        return false;
      }
      if ((int) playerControllerId > 32)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("ClientScene::AddPlayer: playerControllerId of " + (object) playerControllerId + " is too high, max is " + (object) 32));
        return false;
      }
      if ((int) playerControllerId > 16 && LogFilter.logWarn)
        Debug.LogWarning((object) ("ClientScene::AddPlayer: playerControllerId of " + (object) playerControllerId + " is unusually high"));
      while ((int) playerControllerId >= ClientScene.s_LocalPlayers.Count)
        ClientScene.s_LocalPlayers.Add(new PlayerController());
      if (readyConn == null)
      {
        if (!ClientScene.s_IsReady)
        {
          if (LogFilter.logError)
            Debug.LogError((object) "Must call AddPlayer() with a connection the first time to become ready.");
          return false;
        }
      }
      else
      {
        ClientScene.s_IsReady = true;
        ClientScene.s_ReadyConnection = readyConn;
      }
      PlayerController playerController;
      if (ClientScene.s_ReadyConnection.GetPlayerController(playerControllerId, out playerController) && playerController.IsValid && (Object) playerController.gameObject != (Object) null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("ClientScene::AddPlayer: playerControllerId of " + (object) playerControllerId + " already in use."));
        return false;
      }
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::AddPlayer() for ID " + (object) playerControllerId + " called with connection [" + (object) ClientScene.s_ReadyConnection + "]"));
      if (ClientScene.s_ReconnectId == -1)
      {
        AddPlayerMessage addPlayerMessage = new AddPlayerMessage();
        addPlayerMessage.playerControllerId = playerControllerId;
        if (extraMessage != null)
        {
          NetworkWriter writer = new NetworkWriter();
          extraMessage.Serialize(writer);
          addPlayerMessage.msgData = writer.ToArray();
          addPlayerMessage.msgSize = (int) writer.Position;
        }
        ClientScene.s_ReadyConnection.Send((short) 37, (MessageBase) addPlayerMessage);
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("ClientScene::AddPlayer reconnect " + (object) ClientScene.s_ReconnectId));
        if (ClientScene.s_Peers == null)
        {
          ClientScene.SetReconnectId(-1, (PeerInfoMessage[]) null);
          if (LogFilter.logError)
            Debug.LogError((object) "ClientScene::AddPlayer: reconnecting, but no peers.");
          return false;
        }
        foreach (PeerInfoMessage peer in ClientScene.s_Peers)
        {
          if (peer.playerIds != null && peer.connectionId == ClientScene.s_ReconnectId)
          {
            foreach (PeerInfoPlayer playerId in peer.playerIds)
            {
              ReconnectMessage reconnectMessage = new ReconnectMessage();
              reconnectMessage.oldConnectionId = ClientScene.s_ReconnectId;
              reconnectMessage.netId = playerId.netId;
              reconnectMessage.playerControllerId = playerId.playerControllerId;
              if (extraMessage != null)
              {
                NetworkWriter writer = new NetworkWriter();
                extraMessage.Serialize(writer);
                reconnectMessage.msgData = writer.ToArray();
                reconnectMessage.msgSize = (int) writer.Position;
              }
              ClientScene.s_ReadyConnection.Send((short) 47, (MessageBase) reconnectMessage);
            }
          }
        }
        ClientScene.SetReconnectId(-1, (PeerInfoMessage[]) null);
      }
      return true;
    }

    /// <summary>
    ///   <para>Remove the specified player ID from the game.</para>
    /// </summary>
    /// <param name="id">The local player ID number	.</param>
    /// <param name="playerControllerId"></param>
    /// <returns>
    ///   <para>Returns true if the player was successfully destoyed and removed.</para>
    /// </returns>
    public static bool RemovePlayer(short playerControllerId)
    {
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::RemovePlayer() for ID " + (object) playerControllerId + " called with connection [" + (object) ClientScene.s_ReadyConnection + "]"));
      PlayerController playerController;
      if (ClientScene.s_ReadyConnection.GetPlayerController(playerControllerId, out playerController))
      {
        ClientScene.s_ReadyConnection.Send((short) 38, (MessageBase) new RemovePlayerMessage()
        {
          playerControllerId = playerControllerId
        });
        ClientScene.s_ReadyConnection.RemovePlayerController(playerControllerId);
        ClientScene.s_LocalPlayers[(int) playerControllerId] = new PlayerController();
        Object.Destroy((Object) playerController.gameObject);
        return true;
      }
      if (LogFilter.logError)
        Debug.LogError((object) ("Failed to find player ID " + (object) playerControllerId));
      return false;
    }

    /// <summary>
    ///   <para>Signal that the client connection is ready to enter the game.</para>
    /// </summary>
    /// <param name="conn">The client connection which is ready.</param>
    public static bool Ready(NetworkConnection conn)
    {
      if (ClientScene.s_IsReady)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "A connection has already been set as ready. There can only be one.");
        return false;
      }
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::Ready() called with connection [" + (object) conn + "]"));
      if (conn != null)
      {
        ReadyMessage readyMessage = new ReadyMessage();
        conn.Send((short) 35, (MessageBase) readyMessage);
        ClientScene.s_IsReady = true;
        ClientScene.s_ReadyConnection = conn;
        ClientScene.s_ReadyConnection.isReady = true;
        return true;
      }
      if (LogFilter.logError)
        Debug.LogError((object) "Ready() called with invalid connection object: conn=null");
      return false;
    }

    /// <summary>
    ///   <para>Create and connect a local client instance to the local server.</para>
    /// </summary>
    /// <returns>
    ///   <para>A client object for communicating with the local server.</para>
    /// </returns>
    public static NetworkClient ConnectLocalServer()
    {
      LocalClient localClient = new LocalClient();
      NetworkServer.instance.ActivateLocalClientScene();
      localClient.InternalConnectLocalServer(true);
      return (NetworkClient) localClient;
    }

    internal static NetworkClient ReconnectLocalServer()
    {
      LocalClient localClient = new LocalClient();
      NetworkServer.instance.ActivateLocalClientScene();
      localClient.InternalConnectLocalServer(false);
      return (NetworkClient) localClient;
    }

    internal static void ClearLocalPlayers()
    {
      ClientScene.s_LocalPlayers.Clear();
    }

    internal static void HandleClientDisconnect(NetworkConnection conn)
    {
      if (ClientScene.s_ReadyConnection != conn || !ClientScene.s_IsReady)
        return;
      ClientScene.s_IsReady = false;
      ClientScene.s_ReadyConnection = (NetworkConnection) null;
    }

    internal static void PrepareToSpawnSceneObjects()
    {
      ClientScene.s_SpawnableObjects = new Dictionary<NetworkSceneId, NetworkIdentity>();
      foreach (NetworkIdentity networkIdentity in Resources.FindObjectsOfTypeAll<NetworkIdentity>())
      {
        if (!networkIdentity.gameObject.activeSelf && networkIdentity.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity.gameObject.hideFlags != HideFlags.HideAndDontSave && !networkIdentity.sceneId.IsEmpty())
        {
          ClientScene.s_SpawnableObjects[networkIdentity.sceneId] = networkIdentity;
          if (LogFilter.logDebug)
            Debug.Log((object) ("ClientScene::PrepareSpawnObjects sceneId:" + (object) networkIdentity.sceneId));
        }
      }
    }

    internal static NetworkIdentity SpawnSceneObject(NetworkSceneId sceneId)
    {
      if (!ClientScene.s_SpawnableObjects.ContainsKey(sceneId))
        return (NetworkIdentity) null;
      NetworkIdentity spawnableObject = ClientScene.s_SpawnableObjects[sceneId];
      ClientScene.s_SpawnableObjects.Remove(sceneId);
      return spawnableObject;
    }

    internal static void RegisterSystemHandlers(NetworkClient client, bool localClient)
    {
      if (localClient)
      {
        client.RegisterHandlerSafe((short) 1, new NetworkMessageDelegate(ClientScene.OnLocalClientObjectDestroy));
        client.RegisterHandlerSafe((short) 13, new NetworkMessageDelegate(ClientScene.OnLocalClientObjectHide));
        client.RegisterHandlerSafe((short) 3, new NetworkMessageDelegate(ClientScene.OnLocalClientObjectSpawn));
        client.RegisterHandlerSafe((short) 10, new NetworkMessageDelegate(ClientScene.OnLocalClientObjectSpawnScene));
        client.RegisterHandlerSafe((short) 15, new NetworkMessageDelegate(ClientScene.OnClientAuthority));
      }
      else
      {
        client.RegisterHandlerSafe((short) 3, new NetworkMessageDelegate(ClientScene.OnObjectSpawn));
        client.RegisterHandlerSafe((short) 10, new NetworkMessageDelegate(ClientScene.OnObjectSpawnScene));
        client.RegisterHandlerSafe((short) 12, new NetworkMessageDelegate(ClientScene.OnObjectSpawnFinished));
        client.RegisterHandlerSafe((short) 1, new NetworkMessageDelegate(ClientScene.OnObjectDestroy));
        client.RegisterHandlerSafe((short) 13, new NetworkMessageDelegate(ClientScene.OnObjectDestroy));
        client.RegisterHandlerSafe((short) 8, new NetworkMessageDelegate(ClientScene.OnUpdateVarsMessage));
        client.RegisterHandlerSafe((short) 4, new NetworkMessageDelegate(ClientScene.OnOwnerMessage));
        client.RegisterHandlerSafe((short) 9, new NetworkMessageDelegate(ClientScene.OnSyncListMessage));
        client.RegisterHandlerSafe((short) 40, new NetworkMessageDelegate(NetworkAnimator.OnAnimationClientMessage));
        client.RegisterHandlerSafe((short) 41, new NetworkMessageDelegate(NetworkAnimator.OnAnimationParametersClientMessage));
        client.RegisterHandlerSafe((short) 15, new NetworkMessageDelegate(ClientScene.OnClientAuthority));
      }
      client.RegisterHandlerSafe((short) 2, new NetworkMessageDelegate(ClientScene.OnRPCMessage));
      client.RegisterHandlerSafe((short) 7, new NetworkMessageDelegate(ClientScene.OnSyncEventMessage));
      client.RegisterHandlerSafe((short) 42, new NetworkMessageDelegate(NetworkAnimator.OnAnimationTriggerClientMessage));
    }

    internal static string GetStringForAssetId(NetworkHash128 assetId)
    {
      GameObject prefab;
      if (NetworkScene.GetPrefab(assetId, out prefab))
        return prefab.name;
      SpawnDelegate handler;
      if (NetworkScene.GetSpawnHandler(assetId, out handler))
        return handler.Method.Name;
      return "unknown";
    }

    /// <summary>
    ///   <para>Registers a prefab with the UNET spawning system.</para>
    /// </summary>
    /// <param name="prefab">A Prefab that will be spawned.</param>
    /// <param name="spawnHandler">A method to use as a custom spawnhandler on clients.</param>
    /// <param name="unspawnHandler">A method to use as a custom un-spawnhandler on clients.</param>
    /// <param name="newAssetId">An assetId to be assigned to this prefab. This allows a dynamically created game object to be registered for an already known asset Id.</param>
    public static void RegisterPrefab(GameObject prefab, NetworkHash128 newAssetId)
    {
      NetworkScene.RegisterPrefab(prefab, newAssetId);
    }

    /// <summary>
    ///   <para>Registers a prefab with the UNET spawning system.</para>
    /// </summary>
    /// <param name="prefab">A Prefab that will be spawned.</param>
    /// <param name="spawnHandler">A method to use as a custom spawnhandler on clients.</param>
    /// <param name="unspawnHandler">A method to use as a custom un-spawnhandler on clients.</param>
    /// <param name="newAssetId">An assetId to be assigned to this prefab. This allows a dynamically created game object to be registered for an already known asset Id.</param>
    public static void RegisterPrefab(GameObject prefab)
    {
      NetworkScene.RegisterPrefab(prefab);
    }

    /// <summary>
    ///   <para>Registers a prefab with the UNET spawning system.</para>
    /// </summary>
    /// <param name="prefab">A Prefab that will be spawned.</param>
    /// <param name="spawnHandler">A method to use as a custom spawnhandler on clients.</param>
    /// <param name="unspawnHandler">A method to use as a custom un-spawnhandler on clients.</param>
    /// <param name="newAssetId">An assetId to be assigned to this prefab. This allows a dynamically created game object to be registered for an already known asset Id.</param>
    public static void RegisterPrefab(GameObject prefab, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
    {
      NetworkScene.RegisterPrefab(prefab, spawnHandler, unspawnHandler);
    }

    /// <summary>
    ///   <para>Removes a registered spawn prefab.</para>
    /// </summary>
    /// <param name="prefab"></param>
    public static void UnregisterPrefab(GameObject prefab)
    {
      NetworkScene.UnregisterPrefab(prefab);
    }

    /// <summary>
    ///   <para>This is an advanced spawning funciotn that registers a custom assetId with the UNET spawning system.</para>
    /// </summary>
    /// <param name="assetId">Custom assetId string.</param>
    /// <param name="spawnHandler">A method to use as a custom spawnhandler on clients.</param>
    /// <param name="unspawnHandler">A method to use as a custom un-spawnhandler on clients.</param>
    public static void RegisterSpawnHandler(NetworkHash128 assetId, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
    {
      NetworkScene.RegisterSpawnHandler(assetId, spawnHandler, unspawnHandler);
    }

    /// <summary>
    ///   <para>Removes a registered spawn handler function.</para>
    /// </summary>
    /// <param name="assetId"></param>
    public static void UnregisterSpawnHandler(NetworkHash128 assetId)
    {
      NetworkScene.UnregisterSpawnHandler(assetId);
    }

    /// <summary>
    ///   <para>This clears the registered spawn prefabs and spawn handler functions for this client.</para>
    /// </summary>
    public static void ClearSpawners()
    {
      NetworkScene.ClearSpawners();
    }

    /// <summary>
    ///   <para>Destroys all networked objects on the client.</para>
    /// </summary>
    public static void DestroyAllClientObjects()
    {
      ClientScene.s_NetworkScene.DestroyAllClientObjects();
    }

    public static void SetLocalObject(NetworkInstanceId netId, GameObject obj)
    {
      ClientScene.s_NetworkScene.SetLocalObject(netId, obj, ClientScene.s_IsSpawnFinished, false);
    }

    public static GameObject FindLocalObject(NetworkInstanceId netId)
    {
      return ClientScene.s_NetworkScene.FindLocalObject(netId);
    }

    private static void ApplySpawnPayload(NetworkIdentity uv, Vector3 position, byte[] payload, NetworkInstanceId netId, GameObject newGameObject)
    {
      if (!uv.gameObject.activeSelf)
        uv.gameObject.SetActive(true);
      uv.transform.position = position;
      if (payload != null && payload.Length > 0)
      {
        NetworkReader reader = new NetworkReader(payload);
        uv.OnUpdateVars(reader, true);
      }
      if ((Object) newGameObject == (Object) null)
        return;
      newGameObject.SetActive(true);
      uv.SetNetworkInstanceId(netId);
      ClientScene.SetLocalObject(netId, newGameObject);
      if (!ClientScene.s_IsSpawnFinished)
        return;
      uv.OnStartClient();
      ClientScene.CheckForOwner(uv);
    }

    private static void OnObjectSpawn(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ObjectSpawnMessage>(ClientScene.s_ObjectSpawnMessage);
      if (!ClientScene.s_ObjectSpawnMessage.assetId.IsValid())
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("OnObjSpawn netId: " + (object) ClientScene.s_ObjectSpawnMessage.netId + " has invalid asset Id"));
      }
      else
      {
        if (LogFilter.logDebug)
          Debug.Log((object) ("Client spawn handler instantiating [netId:" + (object) ClientScene.s_ObjectSpawnMessage.netId + " asset ID:" + (object) ClientScene.s_ObjectSpawnMessage.assetId + " pos:" + (object) ClientScene.s_ObjectSpawnMessage.position + "]"));
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 3, ClientScene.GetStringForAssetId(ClientScene.s_ObjectSpawnMessage.assetId), 1);
        NetworkIdentity uv;
        if (ClientScene.s_NetworkScene.GetNetworkIdentity(ClientScene.s_ObjectSpawnMessage.netId, out uv))
        {
          ClientScene.ApplySpawnPayload(uv, ClientScene.s_ObjectSpawnMessage.position, ClientScene.s_ObjectSpawnMessage.payload, ClientScene.s_ObjectSpawnMessage.netId, (GameObject) null);
        }
        else
        {
          GameObject prefab;
          if (NetworkScene.GetPrefab(ClientScene.s_ObjectSpawnMessage.assetId, out prefab))
          {
            GameObject newGameObject = (GameObject) Object.Instantiate((Object) prefab, ClientScene.s_ObjectSpawnMessage.position, Quaternion.identity);
            uv = newGameObject.GetComponent<NetworkIdentity>();
            if ((Object) uv == (Object) null)
            {
              if (!LogFilter.logError)
                return;
              Debug.LogError((object) ("Client object spawned for " + (object) ClientScene.s_ObjectSpawnMessage.assetId + " does not have a NetworkIdentity"));
            }
            else
              ClientScene.ApplySpawnPayload(uv, ClientScene.s_ObjectSpawnMessage.position, ClientScene.s_ObjectSpawnMessage.payload, ClientScene.s_ObjectSpawnMessage.netId, newGameObject);
          }
          else
          {
            SpawnDelegate handler;
            if (NetworkScene.GetSpawnHandler(ClientScene.s_ObjectSpawnMessage.assetId, out handler))
            {
              GameObject newGameObject = handler(ClientScene.s_ObjectSpawnMessage.position, ClientScene.s_ObjectSpawnMessage.assetId);
              if ((Object) newGameObject == (Object) null)
              {
                if (!LogFilter.logWarn)
                  return;
                Debug.LogWarning((object) ("Client spawn handler for " + (object) ClientScene.s_ObjectSpawnMessage.assetId + " returned null"));
              }
              else
              {
                uv = newGameObject.GetComponent<NetworkIdentity>();
                if ((Object) uv == (Object) null)
                {
                  if (!LogFilter.logError)
                    return;
                  Debug.LogError((object) ("Client object spawned for " + (object) ClientScene.s_ObjectSpawnMessage.assetId + " does not have a network identity"));
                }
                else
                {
                  uv.SetDynamicAssetId(ClientScene.s_ObjectSpawnMessage.assetId);
                  ClientScene.ApplySpawnPayload(uv, ClientScene.s_ObjectSpawnMessage.position, ClientScene.s_ObjectSpawnMessage.payload, ClientScene.s_ObjectSpawnMessage.netId, newGameObject);
                }
              }
            }
            else
            {
              if (!LogFilter.logError)
                return;
              Debug.LogError((object) ("Failed to spawn server object, assetId=" + (object) ClientScene.s_ObjectSpawnMessage.assetId + " netId=" + (object) ClientScene.s_ObjectSpawnMessage.netId));
            }
          }
        }
      }
    }

    private static void OnObjectSpawnScene(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ObjectSpawnSceneMessage>(ClientScene.s_ObjectSpawnSceneMessage);
      if (LogFilter.logDebug)
        Debug.Log((object) ("Client spawn scene handler instantiating [netId:" + (object) ClientScene.s_ObjectSpawnSceneMessage.netId + " sceneId:" + (object) ClientScene.s_ObjectSpawnSceneMessage.sceneId + " pos:" + (object) ClientScene.s_ObjectSpawnSceneMessage.position));
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 10, "sceneId", 1);
      NetworkIdentity uv1;
      if (ClientScene.s_NetworkScene.GetNetworkIdentity(ClientScene.s_ObjectSpawnSceneMessage.netId, out uv1))
      {
        ClientScene.ApplySpawnPayload(uv1, ClientScene.s_ObjectSpawnSceneMessage.position, ClientScene.s_ObjectSpawnSceneMessage.payload, ClientScene.s_ObjectSpawnSceneMessage.netId, uv1.gameObject);
      }
      else
      {
        NetworkIdentity uv2 = ClientScene.SpawnSceneObject(ClientScene.s_ObjectSpawnSceneMessage.sceneId);
        if ((Object) uv2 == (Object) null)
        {
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) ("Spawn scene object not found for " + (object) ClientScene.s_ObjectSpawnSceneMessage.sceneId));
        }
        else
        {
          if (LogFilter.logDebug)
            Debug.Log((object) ("Client spawn for [netId:" + (object) ClientScene.s_ObjectSpawnSceneMessage.netId + "] [sceneId:" + (object) ClientScene.s_ObjectSpawnSceneMessage.sceneId + "] obj:" + uv2.gameObject.name));
          ClientScene.ApplySpawnPayload(uv2, ClientScene.s_ObjectSpawnSceneMessage.position, ClientScene.s_ObjectSpawnSceneMessage.payload, ClientScene.s_ObjectSpawnSceneMessage.netId, uv2.gameObject);
        }
      }
    }

    private static void OnObjectSpawnFinished(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ObjectSpawnFinishedMessage>(ClientScene.s_ObjectSpawnFinishedMessage);
      if (LogFilter.logDebug)
        Debug.Log((object) ("SpawnFinished:" + (object) ClientScene.s_ObjectSpawnFinishedMessage.state));
      if ((int) ClientScene.s_ObjectSpawnFinishedMessage.state == 0)
      {
        ClientScene.PrepareToSpawnSceneObjects();
        ClientScene.s_IsSpawnFinished = false;
      }
      else
      {
        using (Dictionary<NetworkInstanceId, NetworkIdentity>.ValueCollection.Enumerator enumerator = ClientScene.objects.Values.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            NetworkIdentity current = enumerator.Current;
            if (!current.isClient)
            {
              current.OnStartClient();
              ClientScene.CheckForOwner(current);
            }
          }
        }
        ClientScene.s_IsSpawnFinished = true;
      }
    }

    private static void OnObjectDestroy(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ObjectDestroyMessage>(ClientScene.s_ObjectDestroyMessage);
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::OnObjDestroy netId:" + (object) ClientScene.s_ObjectDestroyMessage.netId));
      NetworkIdentity uv;
      if (ClientScene.s_NetworkScene.GetNetworkIdentity(ClientScene.s_ObjectDestroyMessage.netId, out uv))
      {
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 1, ClientScene.GetStringForAssetId(uv.assetId), 1);
        uv.OnNetworkDestroy();
        if (!NetworkScene.InvokeUnSpawnHandler(uv.assetId, uv.gameObject))
        {
          if (uv.sceneId.IsEmpty())
          {
            Object.Destroy((Object) uv.gameObject);
          }
          else
          {
            uv.gameObject.SetActive(false);
            ClientScene.s_SpawnableObjects[uv.sceneId] = uv;
          }
        }
        ClientScene.s_NetworkScene.RemoveLocalObject(ClientScene.s_ObjectDestroyMessage.netId);
      }
      else
      {
        if (!LogFilter.logDebug)
          return;
        Debug.LogWarning((object) ("Did not find target for destroy message for " + (object) ClientScene.s_ObjectDestroyMessage.netId));
      }
    }

    private static void OnLocalClientObjectDestroy(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ObjectDestroyMessage>(ClientScene.s_ObjectDestroyMessage);
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::OnLocalObjectObjDestroy netId:" + (object) ClientScene.s_ObjectDestroyMessage.netId));
      ClientScene.s_NetworkScene.RemoveLocalObject(ClientScene.s_ObjectDestroyMessage.netId);
    }

    private static void OnLocalClientObjectHide(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ObjectDestroyMessage>(ClientScene.s_ObjectDestroyMessage);
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::OnLocalObjectObjHide netId:" + (object) ClientScene.s_ObjectDestroyMessage.netId));
      NetworkIdentity uv;
      if (!ClientScene.s_NetworkScene.GetNetworkIdentity(ClientScene.s_ObjectDestroyMessage.netId, out uv))
        return;
      uv.OnSetLocalVisibility(false);
    }

    private static void OnLocalClientObjectSpawn(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ObjectSpawnMessage>(ClientScene.s_ObjectSpawnMessage);
      NetworkIdentity uv;
      if (!ClientScene.s_NetworkScene.GetNetworkIdentity(ClientScene.s_ObjectSpawnMessage.netId, out uv))
        return;
      uv.OnSetLocalVisibility(true);
    }

    private static void OnLocalClientObjectSpawnScene(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ObjectSpawnSceneMessage>(ClientScene.s_ObjectSpawnSceneMessage);
      NetworkIdentity uv;
      if (!ClientScene.s_NetworkScene.GetNetworkIdentity(ClientScene.s_ObjectSpawnSceneMessage.netId, out uv))
        return;
      uv.OnSetLocalVisibility(true);
    }

    private static void OnUpdateVarsMessage(NetworkMessage netMsg)
    {
      NetworkInstanceId netId = netMsg.reader.ReadNetworkId();
      if (LogFilter.logDev)
        Debug.Log((object) ("ClientScene::OnUpdateVarsMessage " + (object) netId + " channel:" + (object) netMsg.channelId));
      NetworkIdentity uv;
      if (ClientScene.s_NetworkScene.GetNetworkIdentity(netId, out uv))
      {
        uv.OnUpdateVars(netMsg.reader, false);
      }
      else
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("Did not find target for sync message for " + (object) netId));
      }
    }

    private static void OnRPCMessage(NetworkMessage netMsg)
    {
      int cmdHash = (int) netMsg.reader.ReadPackedUInt32();
      NetworkInstanceId netId = netMsg.reader.ReadNetworkId();
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::OnRPCMessage hash:" + (object) cmdHash + " netId:" + (object) netId));
      NetworkIdentity uv;
      if (ClientScene.s_NetworkScene.GetNetworkIdentity(netId, out uv))
      {
        uv.HandleRPC(cmdHash, netMsg.reader);
      }
      else
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("Did not find target for RPC message for " + (object) netId));
      }
    }

    private static void OnSyncEventMessage(NetworkMessage netMsg)
    {
      int cmdHash = (int) netMsg.reader.ReadPackedUInt32();
      NetworkInstanceId netId = netMsg.reader.ReadNetworkId();
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::OnSyncEventMessage " + (object) netId));
      NetworkIdentity uv;
      if (ClientScene.s_NetworkScene.GetNetworkIdentity(netId, out uv))
        uv.HandleSyncEvent(cmdHash, netMsg.reader);
      else if (LogFilter.logWarn)
        Debug.LogWarning((object) ("Did not find target for SyncEvent message for " + (object) netId));
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 7, NetworkBehaviour.GetCmdHashHandlerName(cmdHash), 1);
    }

    private static void OnSyncListMessage(NetworkMessage netMsg)
    {
      NetworkInstanceId netId = netMsg.reader.ReadNetworkId();
      int cmdHash = (int) netMsg.reader.ReadPackedUInt32();
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::OnSyncListMessage " + (object) netId));
      NetworkIdentity uv;
      if (ClientScene.s_NetworkScene.GetNetworkIdentity(netId, out uv))
        uv.HandleSyncList(cmdHash, netMsg.reader);
      else if (LogFilter.logWarn)
        Debug.LogWarning((object) ("Did not find target for SyncList message for " + (object) netId));
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 9, NetworkBehaviour.GetCmdHashHandlerName(cmdHash), 1);
    }

    private static void OnClientAuthority(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<ClientAuthorityMessage>(ClientScene.s_ClientAuthorityMessage);
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::OnClientAuthority for  connectionId=" + (object) netMsg.conn.connectionId + " netId: " + (object) ClientScene.s_ClientAuthorityMessage.netId));
      NetworkIdentity uv;
      if (!ClientScene.s_NetworkScene.GetNetworkIdentity(ClientScene.s_ClientAuthorityMessage.netId, out uv))
        return;
      uv.HandleClientAuthority(ClientScene.s_ClientAuthorityMessage.authority);
    }

    private static void OnOwnerMessage(NetworkMessage netMsg)
    {
      netMsg.ReadMessage<OwnerMessage>(ClientScene.s_OwnerMessage);
      if (LogFilter.logDebug)
        Debug.Log((object) ("ClientScene::OnOwnerMessage - connectionId=" + (object) netMsg.conn.connectionId + " netId: " + (object) ClientScene.s_OwnerMessage.netId));
      PlayerController playerController;
      if (netMsg.conn.GetPlayerController(ClientScene.s_OwnerMessage.playerControllerId, out playerController))
        playerController.unetView.SetNotLocalPlayer();
      NetworkIdentity uv;
      if (ClientScene.s_NetworkScene.GetNetworkIdentity(ClientScene.s_OwnerMessage.netId, out uv))
      {
        uv.SetConnectionToServer(netMsg.conn);
        uv.SetLocalPlayer(ClientScene.s_OwnerMessage.playerControllerId);
        ClientScene.InternalAddPlayer(uv, ClientScene.s_OwnerMessage.playerControllerId);
      }
      else
      {
        ClientScene.PendingOwner pendingOwner = new ClientScene.PendingOwner() { netId = ClientScene.s_OwnerMessage.netId, playerControllerId = ClientScene.s_OwnerMessage.playerControllerId };
        ClientScene.s_PendingOwnerIds.Add(pendingOwner);
      }
    }

    private static void CheckForOwner(NetworkIdentity uv)
    {
      for (int index = 0; index < ClientScene.s_PendingOwnerIds.Count; ++index)
      {
        ClientScene.PendingOwner pendingOwnerId = ClientScene.s_PendingOwnerIds[index];
        if (pendingOwnerId.netId == uv.netId)
        {
          uv.SetConnectionToServer(ClientScene.s_ReadyConnection);
          uv.SetLocalPlayer(pendingOwnerId.playerControllerId);
          if (LogFilter.logDev)
            Debug.Log((object) ("ClientScene::OnOwnerMessage - player=" + uv.gameObject.name));
          if (ClientScene.s_ReadyConnection.connectionId < 0)
          {
            if (!LogFilter.logError)
              break;
            Debug.LogError((object) "Owner message received on a local client.");
            break;
          }
          ClientScene.InternalAddPlayer(uv, pendingOwnerId.playerControllerId);
          ClientScene.s_PendingOwnerIds.RemoveAt(index);
          break;
        }
      }
    }

    private struct PendingOwner
    {
      public NetworkInstanceId netId;
      public short playerControllerId;
    }
  }
}
