// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkIdentity
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A component used to add an object to the UNET networking system.</para>
  /// </summary>
  [ExecuteInEditMode]
  [DisallowMultipleComponent]
  [AddComponentMenu("Network/NetworkIdentity")]
  public sealed class NetworkIdentity : MonoBehaviour
  {
    private static uint s_NextNetworkId = 1;
    private static NetworkWriter s_UpdateWriter = new NetworkWriter();
    private short m_PlayerId = -1;
    [SerializeField]
    private NetworkSceneId m_SceneId;
    [SerializeField]
    private NetworkHash128 m_AssetId;
    [SerializeField]
    private bool m_ServerOnly;
    [SerializeField]
    private bool m_LocalPlayerAuthority;
    private bool m_IsClient;
    private bool m_IsServer;
    private bool m_HasAuthority;
    private NetworkInstanceId m_NetId;
    private bool m_IsLocalPlayer;
    private NetworkConnection m_ConnectionToServer;
    private NetworkConnection m_ConnectionToClient;
    private NetworkBehaviour[] m_NetworkBehaviours;
    private HashSet<int> m_ObserverConnections;
    private List<NetworkConnection> m_Observers;
    private NetworkConnection m_ClientAuthorityOwner;
    /// <summary>
    ///   <para>A callback that can be populated to be notified when the client-authority state of objects changes.</para>
    /// </summary>
    public static NetworkIdentity.ClientAuthorityCallback clientAuthorityCallback;

    /// <summary>
    ///   <para>Returns true if running as a client and this object was spawned by a server.</para>
    /// </summary>
    public bool isClient
    {
      get
      {
        return this.m_IsClient;
      }
    }

    /// <summary>
    ///   <para>Returns true if running as a server, which spawned the object.</para>
    /// </summary>
    public bool isServer
    {
      get
      {
        if (!this.m_IsServer || !NetworkServer.active)
          return false;
        return this.m_IsServer;
      }
    }

    /// <summary>
    ///   <para>This returns true if this object is the authoritative version of the object in the distributed network application.</para>
    /// </summary>
    public bool hasAuthority
    {
      get
      {
        return this.m_HasAuthority;
      }
    }

    /// <summary>
    ///   <para>Unique identifier for this particular object instance, used for tracking objects between networked clients and the server.</para>
    /// </summary>
    public NetworkInstanceId netId
    {
      get
      {
        return this.m_NetId;
      }
    }

    /// <summary>
    ///   <para>A unique identifier for NetworkIdentity objects within a scene.</para>
    /// </summary>
    public NetworkSceneId sceneId
    {
      get
      {
        return this.m_SceneId;
      }
    }

    /// <summary>
    ///   <para>Flag to make this object only exist when the game is running as a server (or host).</para>
    /// </summary>
    public bool serverOnly
    {
      get
      {
        return this.m_ServerOnly;
      }
      set
      {
        this.m_ServerOnly = value;
      }
    }

    /// <summary>
    ///   <para>LocalPlayerAuthority means that the client of the "owning" player has authority over their own player object.</para>
    /// </summary>
    public bool localPlayerAuthority
    {
      get
      {
        return this.m_LocalPlayerAuthority;
      }
      set
      {
        this.m_LocalPlayerAuthority = value;
      }
    }

    /// <summary>
    ///   <para>The client that has authority for this object. This will be null if no client has authority.</para>
    /// </summary>
    public NetworkConnection clientAuthorityOwner
    {
      get
      {
        return this.m_ClientAuthorityOwner;
      }
    }

    /// <summary>
    ///   <para>Unique identifier used to find the source assets when server spawns the on clients.</para>
    /// </summary>
    public NetworkHash128 assetId
    {
      get
      {
        if (!this.m_AssetId.IsValid())
          this.SetupIDs();
        return this.m_AssetId;
      }
    }

    /// <summary>
    ///   <para>This returns true if this object is the one that represents the player on the local machine.</para>
    /// </summary>
    public bool isLocalPlayer
    {
      get
      {
        return this.m_IsLocalPlayer;
      }
    }

    /// <summary>
    ///   <para>The id of the player associated with this object.</para>
    /// </summary>
    public short playerControllerId
    {
      get
      {
        return this.m_PlayerId;
      }
    }

    /// <summary>
    ///   <para>The UConnection associated with this NetworkIdentity. This is only valid for player objects on a local client.</para>
    /// </summary>
    public NetworkConnection connectionToServer
    {
      get
      {
        return this.m_ConnectionToServer;
      }
    }

    /// <summary>
    ///   <para>The UConnection associated with this NetworkIdentity. This is only valid for player objects on the server.</para>
    /// </summary>
    public NetworkConnection connectionToClient
    {
      get
      {
        return this.m_ConnectionToClient;
      }
    }

    /// <summary>
    ///   <para>The set of network connections (players) that can see this object.</para>
    /// </summary>
    public ReadOnlyCollection<NetworkConnection> observers
    {
      get
      {
        if (this.m_Observers == null)
          return (ReadOnlyCollection<NetworkConnection>) null;
        return new ReadOnlyCollection<NetworkConnection>((IList<NetworkConnection>) this.m_Observers);
      }
    }

    internal void SetDynamicAssetId(NetworkHash128 newAssetId)
    {
      if (!this.m_AssetId.IsValid() || this.m_AssetId.Equals((object) newAssetId))
      {
        this.m_AssetId = newAssetId;
      }
      else
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("SetDynamicAssetId object already has an assetId <" + (object) this.m_AssetId + ">"));
      }
    }

    internal void SetClientOwner(NetworkConnection conn)
    {
      if (this.m_ClientAuthorityOwner != null && LogFilter.logError)
        Debug.LogError((object) "SetClientOwner m_ClientAuthorityOwner already set!");
      this.m_ClientAuthorityOwner = conn;
      this.m_ClientAuthorityOwner.AddOwnedObject(this);
    }

    internal void ClearClientOwner()
    {
      this.m_ClientAuthorityOwner = (NetworkConnection) null;
    }

    internal void ForceAuthority(bool authority)
    {
      if (this.m_HasAuthority == authority)
        return;
      this.m_HasAuthority = authority;
      if (authority)
        this.OnStartAuthority();
      else
        this.OnStopAuthority();
    }

    internal static NetworkInstanceId GetNextNetworkId()
    {
      uint nextNetworkId = NetworkIdentity.s_NextNetworkId;
      ++NetworkIdentity.s_NextNetworkId;
      return new NetworkInstanceId(nextNetworkId);
    }

    private void CacheBehaviours()
    {
      if (this.m_NetworkBehaviours != null)
        return;
      this.m_NetworkBehaviours = this.GetComponents<NetworkBehaviour>();
    }

    internal static void AddNetworkId(uint id)
    {
      if (id < NetworkIdentity.s_NextNetworkId)
        return;
      NetworkIdentity.s_NextNetworkId = id + 1U;
    }

    internal void SetNetworkInstanceId(NetworkInstanceId newNetId)
    {
      this.m_NetId = newNetId;
      if ((int) newNetId.Value != 0)
        return;
      this.m_IsServer = false;
    }

    /// <summary>
    ///   <para>Force the scene ID to a specific value.</para>
    /// </summary>
    /// <param name="sceneId">The new scene ID.</param>
    /// <param name="newSceneId"></param>
    public void ForceSceneId(int newSceneId)
    {
      this.m_SceneId = new NetworkSceneId((uint) newSceneId);
    }

    internal void UpdateClientServer(bool isClientFlag, bool isServerFlag)
    {
      this.m_IsClient |= isClientFlag;
      this.m_IsServer |= isServerFlag;
    }

    internal void SetNoServer()
    {
      this.m_IsServer = false;
      this.SetNetworkInstanceId(NetworkInstanceId.Zero);
    }

    internal void SetNotLocalPlayer()
    {
      this.m_IsLocalPlayer = false;
      if (NetworkServer.active && NetworkServer.localClientActive)
        return;
      this.m_HasAuthority = false;
    }

    internal void RemoveObserverInternal(NetworkConnection conn)
    {
      if (this.m_Observers == null)
        return;
      this.m_Observers.Remove(conn);
      this.m_ObserverConnections.Remove(conn.connectionId);
    }

    private void OnValidate()
    {
      if (this.m_ServerOnly && this.m_LocalPlayerAuthority)
      {
        if (LogFilter.logWarn)
          Debug.LogWarning((object) ("Disabling Local Player Authority for " + (object) this.gameObject + " because it is server-only."));
        this.m_LocalPlayerAuthority = false;
      }
      this.SetupIDs();
    }

    private void AssignAssetID(GameObject prefab)
    {
      this.m_AssetId = NetworkHash128.Parse(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath((UnityEngine.Object) prefab)));
    }

    private bool ThisIsAPrefab()
    {
      return PrefabUtility.GetPrefabType((UnityEngine.Object) this.gameObject) == PrefabType.Prefab;
    }

    private bool ThisIsASceneObjectWithPrefabParent(out GameObject prefab)
    {
      prefab = (GameObject) null;
      if (PrefabUtility.GetPrefabType((UnityEngine.Object) this.gameObject) == PrefabType.None)
        return false;
      prefab = (GameObject) PrefabUtility.GetPrefabParent((UnityEngine.Object) this.gameObject);
      if (!((UnityEngine.Object) prefab == (UnityEngine.Object) null))
        return true;
      if (LogFilter.logError)
        Debug.LogError((object) ("Failed to find prefab parent for scene object [name:" + this.gameObject.name + "]"));
      return false;
    }

    private void SetupIDs()
    {
      if (this.ThisIsAPrefab())
      {
        if (LogFilter.logDev)
          Debug.Log((object) ("This is a prefab: " + this.gameObject.name));
        this.AssignAssetID(this.gameObject);
      }
      else
      {
        GameObject prefab;
        if (this.ThisIsASceneObjectWithPrefabParent(out prefab))
        {
          if (LogFilter.logDev)
            Debug.Log((object) ("This is a scene object with prefab link: " + this.gameObject.name));
          this.AssignAssetID(prefab);
        }
        else
        {
          if (LogFilter.logDev)
            Debug.Log((object) ("This is a pure scene object: " + this.gameObject.name));
          this.m_AssetId.Reset();
        }
      }
    }

    private void OnDestroy()
    {
      if (!this.m_IsServer || !NetworkServer.active)
        return;
      NetworkServer.Destroy(this.gameObject);
    }

    internal void OnStartServer(bool allowNonZeroNetId)
    {
      if (this.m_IsServer)
        return;
      this.m_IsServer = true;
      this.m_HasAuthority = !this.m_LocalPlayerAuthority;
      this.m_Observers = new List<NetworkConnection>();
      this.m_ObserverConnections = new HashSet<int>();
      this.CacheBehaviours();
      if (this.netId.IsEmpty())
        this.m_NetId = NetworkIdentity.GetNextNetworkId();
      else if (!allowNonZeroNetId)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Object has non-zero netId " + (object) this.netId + " for " + (object) this.gameObject));
        return;
      }
      if (LogFilter.logDev)
        Debug.Log((object) ("OnStartServer " + (object) this.gameObject + " GUID:" + (object) this.netId));
      NetworkServer.instance.SetLocalObjectOnServer(this.netId, this.gameObject);
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        try
        {
          networkBehaviour.OnStartServer();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Exception in OnStartServer:" + ex.Message + " " + ex.StackTrace));
        }
      }
      if (NetworkClient.active && NetworkServer.localClientActive)
      {
        ClientScene.SetLocalObject(this.netId, this.gameObject);
        this.OnStartClient();
      }
      if (!this.m_HasAuthority)
        return;
      this.OnStartAuthority();
    }

    internal void OnStartClient()
    {
      if (!this.m_IsClient)
        this.m_IsClient = true;
      this.CacheBehaviours();
      if (LogFilter.logDev)
        Debug.Log((object) ("OnStartClient " + (object) this.gameObject + " GUID:" + (object) this.netId + " localPlayerAuthority:" + (object) this.localPlayerAuthority));
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        try
        {
          networkBehaviour.PreStartClient();
          networkBehaviour.OnStartClient();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Exception in OnStartClient:" + ex.Message + " " + ex.StackTrace));
        }
      }
    }

    internal void OnStartAuthority()
    {
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        try
        {
          networkBehaviour.OnStartAuthority();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Exception in OnStartAuthority:" + ex.Message + " " + ex.StackTrace));
        }
      }
    }

    internal void OnStopAuthority()
    {
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        try
        {
          networkBehaviour.OnStopAuthority();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Exception in OnStopAuthority:" + ex.Message + " " + ex.StackTrace));
        }
      }
    }

    internal void OnSetLocalVisibility(bool vis)
    {
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        try
        {
          networkBehaviour.OnSetLocalVisibility(vis);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Exception in OnSetLocalVisibility:" + ex.Message + " " + ex.StackTrace));
        }
      }
    }

    internal bool OnCheckObserver(NetworkConnection conn)
    {
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        try
        {
          if (!networkBehaviour.OnCheckObserver(conn))
            return false;
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Exception in OnCheckObserver:" + ex.Message + " " + ex.StackTrace));
        }
      }
      return true;
    }

    internal void UNetSerializeAllVars(NetworkWriter writer)
    {
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
        this.m_NetworkBehaviours[index].OnSerialize(writer, true);
    }

    internal void HandleClientAuthority(bool authority)
    {
      if (!this.localPlayerAuthority)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("HandleClientAuthority " + (object) this.gameObject + " does not have localPlayerAuthority"));
      }
      else
        this.ForceAuthority(authority);
    }

    private bool GetInvokeComponent(int cmdHash, System.Type invokeClass, out NetworkBehaviour invokeComponent)
    {
      NetworkBehaviour networkBehaviour1 = (NetworkBehaviour) null;
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour2 = this.m_NetworkBehaviours[index];
        if (networkBehaviour2.GetType() == invokeClass || networkBehaviour2.GetType().IsSubclassOf(invokeClass))
        {
          networkBehaviour1 = networkBehaviour2;
          break;
        }
      }
      if ((UnityEngine.Object) networkBehaviour1 == (UnityEngine.Object) null)
      {
        string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
        if (LogFilter.logError)
          Debug.LogError((object) ("Found no behaviour for incoming [" + cmdHashHandlerName + "] on " + (object) this.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + (object) this.netId + "]."));
        invokeComponent = (NetworkBehaviour) null;
        return false;
      }
      invokeComponent = networkBehaviour1;
      return true;
    }

    internal void HandleSyncEvent(int cmdHash, NetworkReader reader)
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      {
        string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("SyncEvent [" + cmdHashHandlerName + "] received for deleted object [netId=" + (object) this.netId + "]"));
      }
      else
      {
        System.Type invokeClass;
        NetworkBehaviour.CmdDelegate invokeFunction;
        if (!NetworkBehaviour.GetInvokerForHashSyncEvent(cmdHash, out invokeClass, out invokeFunction))
        {
          string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) ("Found no receiver for incoming [" + cmdHashHandlerName + "] on " + (object) this.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + (object) this.netId + "]."));
        }
        else
        {
          NetworkBehaviour invokeComponent;
          if (!this.GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
          {
            string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
            if (!LogFilter.logWarn)
              return;
            Debug.LogWarning((object) ("SyncEvent [" + cmdHashHandlerName + "] handler not found [netId=" + (object) this.netId + "]"));
          }
          else
          {
            invokeFunction(invokeComponent, reader);
            NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 7, NetworkBehaviour.GetCmdHashEventName(cmdHash), 1);
          }
        }
      }
    }

    internal void HandleSyncList(int cmdHash, NetworkReader reader)
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      {
        string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("SyncList [" + cmdHashHandlerName + "] received for deleted object [netId=" + (object) this.netId + "]"));
      }
      else
      {
        System.Type invokeClass;
        NetworkBehaviour.CmdDelegate invokeFunction;
        if (!NetworkBehaviour.GetInvokerForHashSyncList(cmdHash, out invokeClass, out invokeFunction))
        {
          string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) ("Found no receiver for incoming [" + cmdHashHandlerName + "] on " + (object) this.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + (object) this.netId + "]."));
        }
        else
        {
          NetworkBehaviour invokeComponent;
          if (!this.GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
          {
            string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
            if (!LogFilter.logWarn)
              return;
            Debug.LogWarning((object) ("SyncList [" + cmdHashHandlerName + "] handler not found [netId=" + (object) this.netId + "]"));
          }
          else
          {
            invokeFunction(invokeComponent, reader);
            NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 9, NetworkBehaviour.GetCmdHashListName(cmdHash), 1);
          }
        }
      }
    }

    internal void HandleCommand(int cmdHash, NetworkReader reader)
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      {
        string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("Command [" + cmdHashHandlerName + "] received for deleted object [netId=" + (object) this.netId + "]"));
      }
      else
      {
        System.Type invokeClass;
        NetworkBehaviour.CmdDelegate invokeFunction;
        if (!NetworkBehaviour.GetInvokerForHashCommand(cmdHash, out invokeClass, out invokeFunction))
        {
          string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) ("Found no receiver for incoming [" + cmdHashHandlerName + "] on " + (object) this.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + (object) this.netId + "]."));
        }
        else
        {
          NetworkBehaviour invokeComponent;
          if (!this.GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
          {
            string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
            if (!LogFilter.logWarn)
              return;
            Debug.LogWarning((object) ("Command [" + cmdHashHandlerName + "] handler not found [netId=" + (object) this.netId + "]"));
          }
          else
          {
            invokeFunction(invokeComponent, reader);
            NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 5, NetworkBehaviour.GetCmdHashCmdName(cmdHash), 1);
          }
        }
      }
    }

    internal void HandleRPC(int cmdHash, NetworkReader reader)
    {
      if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      {
        string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) ("ClientRpc [" + cmdHashHandlerName + "] received for deleted object [netId=" + (object) this.netId + "]"));
      }
      else
      {
        System.Type invokeClass;
        NetworkBehaviour.CmdDelegate invokeFunction;
        if (!NetworkBehaviour.GetInvokerForHashClientRpc(cmdHash, out invokeClass, out invokeFunction))
        {
          string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) ("Found no receiver for incoming [" + cmdHashHandlerName + "] on " + (object) this.gameObject + ",  the server and client should have the same NetworkBehaviour instances [netId=" + (object) this.netId + "]."));
        }
        else
        {
          NetworkBehaviour invokeComponent;
          if (!this.GetInvokeComponent(cmdHash, invokeClass, out invokeComponent))
          {
            string cmdHashHandlerName = NetworkBehaviour.GetCmdHashHandlerName(cmdHash);
            if (!LogFilter.logWarn)
              return;
            Debug.LogWarning((object) ("ClientRpc [" + cmdHashHandlerName + "] handler not found [netId=" + (object) this.netId + "]"));
          }
          else
          {
            invokeFunction(invokeComponent, reader);
            NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 2, NetworkBehaviour.GetCmdHashRpcName(cmdHash), 1);
          }
        }
      }
    }

    internal void UNetUpdate()
    {
      uint num = 0;
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        int dirtyChannel = this.m_NetworkBehaviours[index].GetDirtyChannel();
        if (dirtyChannel != -1)
          num |= (uint) (1 << dirtyChannel);
      }
      if ((int) num == 0)
        return;
      for (int channelId = 0; channelId < NetworkServer.numChannels; ++channelId)
      {
        if (((int) num & 1 << channelId) != 0)
        {
          NetworkIdentity.s_UpdateWriter.StartMessage((short) 8);
          NetworkIdentity.s_UpdateWriter.Write(this.netId);
          bool flag = false;
          for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
          {
            short position = NetworkIdentity.s_UpdateWriter.Position;
            NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
            if (networkBehaviour.GetDirtyChannel() != channelId)
            {
              networkBehaviour.OnSerialize(NetworkIdentity.s_UpdateWriter, false);
            }
            else
            {
              if (networkBehaviour.OnSerialize(NetworkIdentity.s_UpdateWriter, false))
              {
                networkBehaviour.ClearAllDirtyBits();
                NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 8, networkBehaviour.GetType().Name, 1);
                flag = true;
              }
              if ((int) NetworkIdentity.s_UpdateWriter.Position - (int) position > (int) NetworkServer.maxPacketSize && LogFilter.logWarn)
                Debug.LogWarning((object) ("Large state update of " + (object) ((int) NetworkIdentity.s_UpdateWriter.Position - (int) position) + " bytes for netId:" + (object) this.netId + " from script:" + (object) networkBehaviour));
            }
          }
          if (flag)
          {
            NetworkIdentity.s_UpdateWriter.FinishMessage();
            NetworkServer.SendWriterToReady(this.gameObject, NetworkIdentity.s_UpdateWriter, channelId);
          }
        }
      }
    }

    internal void OnUpdateVars(NetworkReader reader, bool initialState)
    {
      if (initialState && this.m_NetworkBehaviours == null)
        this.m_NetworkBehaviours = this.GetComponents<NetworkBehaviour>();
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        uint position = reader.Position;
        networkBehaviour.OnDeserialize(reader, initialState);
        if (reader.Position - position > 1U)
          NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 8, networkBehaviour.GetType().Name, 1);
      }
    }

    internal void SetLocalPlayer(short localPlayerControllerId)
    {
      this.m_IsLocalPlayer = true;
      this.m_PlayerId = localPlayerControllerId;
      bool hasAuthority = this.m_HasAuthority;
      if (this.localPlayerAuthority)
        this.m_HasAuthority = true;
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        networkBehaviour.OnStartLocalPlayer();
        if (this.localPlayerAuthority && !hasAuthority)
          networkBehaviour.OnStartAuthority();
      }
    }

    internal void SetConnectionToServer(NetworkConnection conn)
    {
      this.m_ConnectionToServer = conn;
    }

    internal void SetConnectionToClient(NetworkConnection conn, short newPlayerControllerId)
    {
      this.m_PlayerId = newPlayerControllerId;
      this.m_ConnectionToClient = conn;
    }

    internal void OnNetworkDestroy()
    {
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
        this.m_NetworkBehaviours[index].OnNetworkDestroy();
      this.m_IsServer = false;
    }

    internal void ClearObservers()
    {
      if (this.m_Observers == null)
        return;
      int count = this.m_Observers.Count;
      for (int index = 0; index < count; ++index)
        this.m_Observers[index].RemoveFromVisList(this, true);
      this.m_Observers.Clear();
      this.m_ObserverConnections.Clear();
    }

    internal void AddObserver(NetworkConnection conn)
    {
      if (this.m_Observers == null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("AddObserver for " + (object) this.gameObject + " observer list is null"));
      }
      else if (this.m_ObserverConnections.Contains(conn.connectionId))
      {
        if (!LogFilter.logDebug)
          return;
        Debug.Log((object) ("Duplicate observer " + conn.address + " added for " + (object) this.gameObject));
      }
      else
      {
        if (LogFilter.logDev)
          Debug.Log((object) ("Added observer " + conn.address + " added for " + (object) this.gameObject));
        this.m_Observers.Add(conn);
        this.m_ObserverConnections.Add(conn.connectionId);
        conn.AddToVisList(this);
      }
    }

    internal void RemoveObserver(NetworkConnection conn)
    {
      if (this.m_Observers == null)
        return;
      this.m_Observers.Remove(conn);
      this.m_ObserverConnections.Remove(conn.connectionId);
      conn.RemoveFromVisList(this, false);
    }

    /// <summary>
    ///   <para>This causes the set of players that can see this object to be rebuild. The OnRebuildObservers callback function will be invoked on each NetworkBehaviour.</para>
    /// </summary>
    /// <param name="initialize">True if this is the first time.</param>
    public void RebuildObservers(bool initialize)
    {
      if (this.m_Observers == null)
        return;
      bool flag1 = false;
      bool flag2 = false;
      HashSet<NetworkConnection> observers = new HashSet<NetworkConnection>();
      HashSet<NetworkConnection> networkConnectionSet = new HashSet<NetworkConnection>((IEnumerable<NetworkConnection>) this.m_Observers);
      for (int index = 0; index < this.m_NetworkBehaviours.Length; ++index)
      {
        NetworkBehaviour networkBehaviour = this.m_NetworkBehaviours[index];
        flag2 |= networkBehaviour.OnRebuildObservers(observers, initialize);
      }
      if (!flag2)
      {
        if (!initialize)
          return;
        foreach (NetworkConnection connection in NetworkServer.connections)
        {
          if (connection != null && connection.isReady)
            this.AddObserver(connection);
        }
        using (List<NetworkConnection>.Enumerator enumerator = NetworkServer.localConnections.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            NetworkConnection current = enumerator.Current;
            if (current != null && current.isReady)
              this.AddObserver(current);
          }
        }
      }
      else
      {
        using (HashSet<NetworkConnection>.Enumerator enumerator = observers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            NetworkConnection current = enumerator.Current;
            if (current != null)
            {
              if (!current.isReady)
              {
                if (LogFilter.logWarn)
                  Debug.LogWarning((object) ("Observer is not ready for " + (object) this.gameObject + " " + (object) current));
              }
              else if (initialize || !networkConnectionSet.Contains(current))
              {
                current.AddToVisList(this);
                if (LogFilter.logDebug)
                  Debug.Log((object) ("New Observer for " + (object) this.gameObject + " " + (object) current));
                flag1 = true;
              }
            }
          }
        }
        using (HashSet<NetworkConnection>.Enumerator enumerator = networkConnectionSet.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            NetworkConnection current = enumerator.Current;
            if (!observers.Contains(current))
            {
              current.RemoveFromVisList(this, false);
              if (LogFilter.logDebug)
                Debug.Log((object) ("Removed Observer for " + (object) this.gameObject + " " + (object) current));
              flag1 = true;
            }
          }
        }
        if (initialize)
        {
          using (List<NetworkConnection>.Enumerator enumerator = NetworkServer.localConnections.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              NetworkConnection current = enumerator.Current;
              if (!observers.Contains(current))
                this.OnSetLocalVisibility(false);
            }
          }
        }
        if (!flag1)
          return;
        this.m_Observers = new List<NetworkConnection>((IEnumerable<NetworkConnection>) observers);
        this.m_ObserverConnections.Clear();
        using (List<NetworkConnection>.Enumerator enumerator = this.m_Observers.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.m_ObserverConnections.Add(enumerator.Current.connectionId);
        }
      }
    }

    /// <summary>
    ///   <para>Removes ownership for an object for a client by its conneciton.</para>
    /// </summary>
    /// <param name="conn">The connection of the client to remove authority for.</param>
    /// <returns>
    ///   <para>True if authority is removed.</para>
    /// </returns>
    public bool RemoveClientAuthority(NetworkConnection conn)
    {
      if (!this.isServer)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "RemoveClientAuthority can only be call on the server for spawned objects.");
        return false;
      }
      if (this.connectionToClient != null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "RemoveClientAuthority cannot remove authority for a player object");
        return false;
      }
      if (this.m_ClientAuthorityOwner == null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("RemoveClientAuthority for " + (object) this.gameObject + " has no clientAuthority owner."));
        return false;
      }
      if (this.m_ClientAuthorityOwner != conn)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("RemoveClientAuthority for " + (object) this.gameObject + " has different owner."));
        return false;
      }
      this.m_ClientAuthorityOwner.RemoveOwnedObject(this);
      this.m_ClientAuthorityOwner = (NetworkConnection) null;
      this.ForceAuthority(true);
      conn.Send((short) 15, (MessageBase) new ClientAuthorityMessage()
      {
        netId = this.netId,
        authority = false
      });
      if (NetworkIdentity.clientAuthorityCallback != null)
        NetworkIdentity.clientAuthorityCallback(conn, this, false);
      return true;
    }

    /// <summary>
    ///   <para>This assigns control of an object to a client via the client's NetworkConnection.</para>
    /// </summary>
    /// <param name="conn">The connection of the client to assign authority to.</param>
    /// <returns>
    ///   <para>True if authority was assigned.</para>
    /// </returns>
    public bool AssignClientAuthority(NetworkConnection conn)
    {
      if (!this.isServer)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "AssignClientAuthority can only be call on the server for spawned objects.");
        return false;
      }
      if (!this.localPlayerAuthority)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "AssignClientAuthority can only be used for NetworkIdentity component with LocalPlayerAuthority set.");
        return false;
      }
      if (this.m_ClientAuthorityOwner != null && conn != this.m_ClientAuthorityOwner)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("AssignClientAuthority for " + (object) this.gameObject + " already has an owner. Use RemoveClientAuthority() first."));
        return false;
      }
      if (conn == null)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("AssignClientAuthority for " + (object) this.gameObject + " owner cannot be null. Use RemoveClientAuthority() instead."));
        return false;
      }
      this.m_ClientAuthorityOwner = conn;
      this.m_ClientAuthorityOwner.AddOwnedObject(this);
      this.ForceAuthority(false);
      conn.Send((short) 15, (MessageBase) new ClientAuthorityMessage()
      {
        netId = this.netId,
        authority = true
      });
      if (NetworkIdentity.clientAuthorityCallback != null)
        NetworkIdentity.clientAuthorityCallback(conn, this, true);
      return true;
    }

    internal static void UNetDomainReload()
    {
      NetworkManager.OnDomainReload();
    }

    internal static void UNetStaticUpdate()
    {
      NetworkServer.Update();
      NetworkClient.UpdateClients();
      NetworkManager.UpdateScene();
      NetworkDetailStats.NewProfilerTick(Time.time);
    }

    /// <summary>
    ///   <para>The delegate type for the clientAuthorityCallback.</para>
    /// </summary>
    /// <param name="conn">The network connection that is gaining or losing authority.</param>
    /// <param name="uv">The object whose client authority status is being changed.</param>
    /// <param name="authorityState">The new state of client authority of the object for the connection.</param>
    public delegate void ClientAuthorityCallback(NetworkConnection conn, NetworkIdentity uv, bool authorityState);
  }
}
