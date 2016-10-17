// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkBehaviour
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Base class which should be inherited by scripts which contain networking functionality.</para>
  /// </summary>
  [AddComponentMenu("")]
  [RequireComponent(typeof (NetworkIdentity))]
  public class NetworkBehaviour : MonoBehaviour
  {
    private static Dictionary<int, NetworkBehaviour.Invoker> s_CmdHandlerDelegates = new Dictionary<int, NetworkBehaviour.Invoker>();
    private const float k_DefaultSendInterval = 0.1f;
    private uint m_SyncVarDirtyBits;
    private float m_LastSendTime;
    private bool m_SyncVarGuard;
    private NetworkIdentity m_MyView;

    /// <summary>
    ///   <para>This value is set on the NetworkIdentity and is accessible here for convenient access for scripts.</para>
    /// </summary>
    public bool localPlayerAuthority
    {
      get
      {
        return this.myView.localPlayerAuthority;
      }
    }

    /// <summary>
    ///   <para>Returns true if this object is active on an active server.</para>
    /// </summary>
    public bool isServer
    {
      get
      {
        return this.myView.isServer;
      }
    }

    /// <summary>
    ///   <para>Returns true if running as a client and this object was spawned by a server.</para>
    /// </summary>
    public bool isClient
    {
      get
      {
        return this.myView.isClient;
      }
    }

    /// <summary>
    ///   <para>This returns true if this object is the one that represents the player on the local machine.</para>
    /// </summary>
    public bool isLocalPlayer
    {
      get
      {
        return this.myView.isLocalPlayer;
      }
    }

    /// <summary>
    ///   <para>This returns true if this object is the authoritative version of the object in the distributed network application.</para>
    /// </summary>
    public bool hasAuthority
    {
      get
      {
        return this.myView.hasAuthority;
      }
    }

    /// <summary>
    ///   <para>The unique network Id of this object.</para>
    /// </summary>
    public NetworkInstanceId netId
    {
      get
      {
        return this.myView.netId;
      }
    }

    /// <summary>
    ///   <para>The NetworkConnection associated with this NetworkIdentity. This is only valid for player objects on the server.</para>
    /// </summary>
    public NetworkConnection connectionToServer
    {
      get
      {
        return this.myView.connectionToServer;
      }
    }

    /// <summary>
    ///   <para>The NetworkConnection associated with this NetworkIdentity. This is only valid for player objects on the server.</para>
    /// </summary>
    public NetworkConnection connectionToClient
    {
      get
      {
        return this.myView.connectionToClient;
      }
    }

    /// <summary>
    ///   <para>The id of the player associated with thei behaviour.</para>
    /// </summary>
    public short playerControllerId
    {
      get
      {
        return this.myView.playerControllerId;
      }
    }

    protected uint syncVarDirtyBits
    {
      get
      {
        return this.m_SyncVarDirtyBits;
      }
    }

    protected bool syncVarHookGuard
    {
      get
      {
        return this.m_SyncVarGuard;
      }
      set
      {
        this.m_SyncVarGuard = value;
      }
    }

    private NetworkIdentity myView
    {
      get
      {
        if (!((UnityEngine.Object) this.m_MyView == (UnityEngine.Object) null))
          return this.m_MyView;
        this.m_MyView = this.GetComponent<NetworkIdentity>();
        if ((UnityEngine.Object) this.m_MyView == (UnityEngine.Object) null && LogFilter.logError)
          Debug.LogError((object) "There is no NetworkIdentity on this object. Please add one.");
        return this.m_MyView;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void SendCommandInternal(NetworkWriter writer, int channelId, string cmdName)
    {
      if (!this.isLocalPlayer && !this.hasAuthority)
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) "Trying to send command for object without authority.");
      }
      else if (ClientScene.readyConnection == null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) ("Send command attempted with no client running [client=" + (object) this.connectionToServer + "]."));
      }
      else
      {
        writer.FinishMessage();
        ClientScene.readyConnection.SendWriter(writer, channelId);
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 5, cmdName, 1);
      }
    }

    /// <summary>
    ///   <para>Manually invoke a Command.</para>
    /// </summary>
    /// <param name="cmdHash">Hash of the Command name.</param>
    /// <param name="reader">Parameters to pass to the command.</param>
    /// <returns>
    ///   <para>Returns true if successful.</para>
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual bool InvokeCommand(int cmdHash, NetworkReader reader)
    {
      return this.InvokeCommandDelegate(cmdHash, reader);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void SendRPCInternal(NetworkWriter writer, int channelId, string rpcName)
    {
      if (!this.isServer)
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) "ClientRpc call on un-spawned object");
      }
      else
      {
        writer.FinishMessage();
        NetworkServer.SendWriterToReady(this.gameObject, writer, channelId);
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 2, rpcName, 1);
      }
    }

    /// <summary>
    ///   <para>Manually invoke an RPC function.</para>
    /// </summary>
    /// <param name="cmdHash">Hash of the RPC name.</param>
    /// <param name="reader">Parameters to pass to the RPC function.</param>
    /// <returns>
    ///   <para>Returns true if successful.</para>
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual bool InvokeRPC(int cmdHash, NetworkReader reader)
    {
      return this.InvokeRpcDelegate(cmdHash, reader);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void SendEventInternal(NetworkWriter writer, int channelId, string eventName)
    {
      if (!NetworkServer.active)
      {
        if (!LogFilter.logWarn)
          return;
        Debug.LogWarning((object) "SendEvent no server?");
      }
      else
      {
        writer.FinishMessage();
        NetworkServer.SendWriterToReady(this.gameObject, writer, channelId);
        NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 7, eventName, 1);
      }
    }

    /// <summary>
    ///   <para>Manually invoke a SyncEvent.</para>
    /// </summary>
    /// <param name="cmdHash">Hash of the SyncEvent name.</param>
    /// <param name="reader">Parameters to pass to the SyncEvent.</param>
    /// <returns>
    ///   <para>Returns true if successful.</para>
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual bool InvokeSyncEvent(int cmdHash, NetworkReader reader)
    {
      return this.InvokeSyncEventDelegate(cmdHash, reader);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual bool InvokeSyncList(int cmdHash, NetworkReader reader)
    {
      return this.InvokeSyncListDelegate(cmdHash, reader);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected static void RegisterCommandDelegate(System.Type invokeClass, int cmdHash, NetworkBehaviour.CmdDelegate func)
    {
      if (NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return;
      NetworkBehaviour.s_CmdHandlerDelegates[cmdHash] = new NetworkBehaviour.Invoker()
      {
        invokeType = NetworkBehaviour.UNetInvokeType.Command,
        invokeClass = invokeClass,
        invokeFunction = func
      };
      if (!LogFilter.logDev)
        return;
      Debug.Log((object) ("RegisterCommandDelegate hash:" + (object) cmdHash + " " + func.Method.Name));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected static void RegisterRpcDelegate(System.Type invokeClass, int cmdHash, NetworkBehaviour.CmdDelegate func)
    {
      if (NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return;
      NetworkBehaviour.s_CmdHandlerDelegates[cmdHash] = new NetworkBehaviour.Invoker()
      {
        invokeType = NetworkBehaviour.UNetInvokeType.ClientRpc,
        invokeClass = invokeClass,
        invokeFunction = func
      };
      if (!LogFilter.logDev)
        return;
      Debug.Log((object) ("RegisterRpcDelegate hash:" + (object) cmdHash + " " + func.Method.Name));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected static void RegisterEventDelegate(System.Type invokeClass, int cmdHash, NetworkBehaviour.CmdDelegate func)
    {
      if (NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return;
      NetworkBehaviour.s_CmdHandlerDelegates[cmdHash] = new NetworkBehaviour.Invoker()
      {
        invokeType = NetworkBehaviour.UNetInvokeType.SyncEvent,
        invokeClass = invokeClass,
        invokeFunction = func
      };
      if (!LogFilter.logDev)
        return;
      Debug.Log((object) ("RegisterEventDelegate hash:" + (object) cmdHash + " " + func.Method.Name));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected static void RegisterSyncListDelegate(System.Type invokeClass, int cmdHash, NetworkBehaviour.CmdDelegate func)
    {
      if (NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return;
      NetworkBehaviour.s_CmdHandlerDelegates[cmdHash] = new NetworkBehaviour.Invoker()
      {
        invokeType = NetworkBehaviour.UNetInvokeType.SyncList,
        invokeClass = invokeClass,
        invokeFunction = func
      };
      if (!LogFilter.logDev)
        return;
      Debug.Log((object) ("RegisterSyncListDelegate hash:" + (object) cmdHash + " " + func.Method.Name));
    }

    internal static string GetInvoker(int cmdHash)
    {
      if (!NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return (string) null;
      return NetworkBehaviour.s_CmdHandlerDelegates[cmdHash].DebugString();
    }

    internal static bool GetInvokerForHashCommand(int cmdHash, out System.Type invokeClass, out NetworkBehaviour.CmdDelegate invokeFunction)
    {
      return NetworkBehaviour.GetInvokerForHash(cmdHash, NetworkBehaviour.UNetInvokeType.Command, out invokeClass, out invokeFunction);
    }

    internal static bool GetInvokerForHashClientRpc(int cmdHash, out System.Type invokeClass, out NetworkBehaviour.CmdDelegate invokeFunction)
    {
      return NetworkBehaviour.GetInvokerForHash(cmdHash, NetworkBehaviour.UNetInvokeType.ClientRpc, out invokeClass, out invokeFunction);
    }

    internal static bool GetInvokerForHashSyncList(int cmdHash, out System.Type invokeClass, out NetworkBehaviour.CmdDelegate invokeFunction)
    {
      return NetworkBehaviour.GetInvokerForHash(cmdHash, NetworkBehaviour.UNetInvokeType.SyncList, out invokeClass, out invokeFunction);
    }

    internal static bool GetInvokerForHashSyncEvent(int cmdHash, out System.Type invokeClass, out NetworkBehaviour.CmdDelegate invokeFunction)
    {
      return NetworkBehaviour.GetInvokerForHash(cmdHash, NetworkBehaviour.UNetInvokeType.SyncEvent, out invokeClass, out invokeFunction);
    }

    private static bool GetInvokerForHash(int cmdHash, NetworkBehaviour.UNetInvokeType invokeType, out System.Type invokeClass, out NetworkBehaviour.CmdDelegate invokeFunction)
    {
      NetworkBehaviour.Invoker invoker = (NetworkBehaviour.Invoker) null;
      if (!NetworkBehaviour.s_CmdHandlerDelegates.TryGetValue(cmdHash, out invoker))
      {
        if (LogFilter.logDev)
          Debug.Log((object) ("GetInvokerForHash hash:" + (object) cmdHash + " not found"));
        invokeClass = (System.Type) null;
        invokeFunction = (NetworkBehaviour.CmdDelegate) null;
        return false;
      }
      if (invoker == null)
      {
        if (LogFilter.logDev)
          Debug.Log((object) ("GetInvokerForHash hash:" + (object) cmdHash + " invoker null"));
        invokeClass = (System.Type) null;
        invokeFunction = (NetworkBehaviour.CmdDelegate) null;
        return false;
      }
      if (invoker.invokeType != invokeType)
      {
        if (LogFilter.logError)
          Debug.LogError((object) ("GetInvokerForHash hash:" + (object) cmdHash + " mismatched invokeType"));
        invokeClass = (System.Type) null;
        invokeFunction = (NetworkBehaviour.CmdDelegate) null;
        return false;
      }
      invokeClass = invoker.invokeClass;
      invokeFunction = invoker.invokeFunction;
      return true;
    }

    internal static void DumpInvokers()
    {
      Debug.Log((object) ("DumpInvokers size:" + (object) NetworkBehaviour.s_CmdHandlerDelegates.Count));
      using (Dictionary<int, NetworkBehaviour.Invoker>.Enumerator enumerator = NetworkBehaviour.s_CmdHandlerDelegates.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, NetworkBehaviour.Invoker> current = enumerator.Current;
          Debug.Log((object) ("  Invoker:" + (object) current.Value.invokeClass + ":" + current.Value.invokeFunction.Method.Name + " " + (object) current.Value.invokeType + " " + (object) current.Key));
        }
      }
    }

    internal bool ContainsCommandDelegate(int cmdHash)
    {
      return NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash);
    }

    internal bool InvokeCommandDelegate(int cmdHash, NetworkReader reader)
    {
      if (!NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return false;
      NetworkBehaviour.Invoker cmdHandlerDelegate = NetworkBehaviour.s_CmdHandlerDelegates[cmdHash];
      if (cmdHandlerDelegate.invokeType != NetworkBehaviour.UNetInvokeType.Command || this.GetType() != cmdHandlerDelegate.invokeClass && !this.GetType().IsSubclassOf(cmdHandlerDelegate.invokeClass))
        return false;
      cmdHandlerDelegate.invokeFunction(this, reader);
      return true;
    }

    internal bool InvokeRpcDelegate(int cmdHash, NetworkReader reader)
    {
      if (!NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return false;
      NetworkBehaviour.Invoker cmdHandlerDelegate = NetworkBehaviour.s_CmdHandlerDelegates[cmdHash];
      if (cmdHandlerDelegate.invokeType != NetworkBehaviour.UNetInvokeType.ClientRpc || this.GetType() != cmdHandlerDelegate.invokeClass && !this.GetType().IsSubclassOf(cmdHandlerDelegate.invokeClass))
        return false;
      cmdHandlerDelegate.invokeFunction(this, reader);
      return true;
    }

    internal bool InvokeSyncEventDelegate(int cmdHash, NetworkReader reader)
    {
      if (!NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return false;
      NetworkBehaviour.Invoker cmdHandlerDelegate = NetworkBehaviour.s_CmdHandlerDelegates[cmdHash];
      if (cmdHandlerDelegate.invokeType != NetworkBehaviour.UNetInvokeType.SyncEvent)
        return false;
      cmdHandlerDelegate.invokeFunction(this, reader);
      return true;
    }

    internal bool InvokeSyncListDelegate(int cmdHash, NetworkReader reader)
    {
      if (!NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return false;
      NetworkBehaviour.Invoker cmdHandlerDelegate = NetworkBehaviour.s_CmdHandlerDelegates[cmdHash];
      if (cmdHandlerDelegate.invokeType != NetworkBehaviour.UNetInvokeType.SyncList || this.GetType() != cmdHandlerDelegate.invokeClass)
        return false;
      cmdHandlerDelegate.invokeFunction(this, reader);
      return true;
    }

    internal static string GetCmdHashHandlerName(int cmdHash)
    {
      if (!NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return cmdHash.ToString();
      NetworkBehaviour.Invoker cmdHandlerDelegate = NetworkBehaviour.s_CmdHandlerDelegates[cmdHash];
      return ((int) cmdHandlerDelegate.invokeType).ToString() + ":" + cmdHandlerDelegate.invokeFunction.Method.Name;
    }

    private static string GetCmdHashPrefixName(int cmdHash, string prefix)
    {
      if (!NetworkBehaviour.s_CmdHandlerDelegates.ContainsKey(cmdHash))
        return cmdHash.ToString();
      string str = NetworkBehaviour.s_CmdHandlerDelegates[cmdHash].invokeFunction.Method.Name;
      if (str.IndexOf(prefix) > -1)
        str = str.Substring(prefix.Length);
      return str;
    }

    internal static string GetCmdHashCmdName(int cmdHash)
    {
      return NetworkBehaviour.GetCmdHashPrefixName(cmdHash, "InvokeCmd");
    }

    internal static string GetCmdHashRpcName(int cmdHash)
    {
      return NetworkBehaviour.GetCmdHashPrefixName(cmdHash, "InvokeRpc");
    }

    internal static string GetCmdHashEventName(int cmdHash)
    {
      return NetworkBehaviour.GetCmdHashPrefixName(cmdHash, "InvokeSyncEvent");
    }

    internal static string GetCmdHashListName(int cmdHash)
    {
      return NetworkBehaviour.GetCmdHashPrefixName(cmdHash, "InvokeSyncList");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void SetSyncVarGameObject(GameObject newGameObject, ref GameObject gameObjectField, uint dirtyBit, ref NetworkInstanceId netIdField)
    {
      if (this.m_SyncVarGuard)
        return;
      NetworkInstanceId networkInstanceId1 = new NetworkInstanceId();
      if ((UnityEngine.Object) newGameObject != (UnityEngine.Object) null)
      {
        NetworkIdentity component = newGameObject.GetComponent<NetworkIdentity>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          networkInstanceId1 = component.netId;
          if (networkInstanceId1.IsEmpty() && LogFilter.logWarn)
            Debug.LogWarning((object) ("SetSyncVarGameObject GameObject " + (object) newGameObject + " has a zero netId. Maybe it is not spawned yet?"));
        }
      }
      NetworkInstanceId networkInstanceId2 = new NetworkInstanceId();
      if ((UnityEngine.Object) gameObjectField != (UnityEngine.Object) null)
        networkInstanceId2 = gameObjectField.GetComponent<NetworkIdentity>().netId;
      if (!(networkInstanceId1 != networkInstanceId2))
        return;
      if (LogFilter.logDev)
        Debug.Log((object) ("SetSyncVar GameObject " + this.GetType().Name + " bit [" + (object) dirtyBit + "] netfieldId:" + (object) networkInstanceId2 + "->" + (object) networkInstanceId1));
      this.SetDirtyBit(dirtyBit);
      gameObjectField = newGameObject;
      netIdField = networkInstanceId1;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    protected void SetSyncVar<T>(T value, ref T fieldValue, uint dirtyBit)
    {
      if (value.Equals((object) fieldValue))
        return;
      if (LogFilter.logDev)
        Debug.Log((object) ("SetSyncVar " + this.GetType().Name + " bit [" + (object) dirtyBit + "] " + (object) fieldValue + "->" + (object) value));
      this.SetDirtyBit(dirtyBit);
      fieldValue = value;
    }

    /// <summary>
    ///   <para>Used to set the behaviour as dirty, so that a network update will be sent for the object.</para>
    /// </summary>
    /// <param name="dirtyBit">Bit mask to set.</param>
    public void SetDirtyBit(uint dirtyBit)
    {
      this.m_SyncVarDirtyBits |= dirtyBit;
    }

    /// <summary>
    ///   <para>This clears all the dirty bits that were set on this script by SetDirtyBits();</para>
    /// </summary>
    public void ClearAllDirtyBits()
    {
      this.m_LastSendTime = Time.time;
      this.m_SyncVarDirtyBits = 0U;
    }

    internal int GetDirtyChannel()
    {
      if ((double) Time.time - (double) this.m_LastSendTime > (double) this.GetNetworkSendInterval() && (int) this.m_SyncVarDirtyBits != 0)
        return this.GetNetworkChannel();
      return -1;
    }

    /// <summary>
    ///   <para>Virtual function to override to send custom serialization data.</para>
    /// </summary>
    /// <param name="writer">Writer to use to write to the stream.</param>
    /// <param name="initialState">If this is being called to send initial state.</param>
    /// <returns>
    ///   <para>True if data was written.</para>
    /// </returns>
    public virtual bool OnSerialize(NetworkWriter writer, bool initialState)
    {
      if (!initialState)
        writer.WritePackedUInt32(0U);
      return false;
    }

    /// <summary>
    ///   <para>Virtual function to override to receive custom serialization data.</para>
    /// </summary>
    /// <param name="reader">Reader to read from the stream.</param>
    /// <param name="initialState">True if being sent initial state.</param>
    public virtual void OnDeserialize(NetworkReader reader, bool initialState)
    {
      if (initialState)
        return;
      int num = (int) reader.ReadPackedUInt32();
    }

    /// <summary>
    ///   <para>An internal method called on client objects to resolve GameObject references.</para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void PreStartClient()
    {
    }

    /// <summary>
    ///   <para>This is invoked on clients when the server has caused this object to be destroyed.</para>
    /// </summary>
    public virtual void OnNetworkDestroy()
    {
    }

    /// <summary>
    ///   <para>Called when the server starts listening.</para>
    /// </summary>
    public virtual void OnStartServer()
    {
    }

    /// <summary>
    ///   <para>Called on every NetworkBehaviour when it is activated on a client.</para>
    /// </summary>
    public virtual void OnStartClient()
    {
    }

    /// <summary>
    ///   <para>Called when the local player object has been set up.</para>
    /// </summary>
    public virtual void OnStartLocalPlayer()
    {
    }

    /// <summary>
    ///   <para>This is invoked on behaviours that have authority, based on context and the LocalPlayerAuthority value on the NetworkIdentity.</para>
    /// </summary>
    public virtual void OnStartAuthority()
    {
    }

    /// <summary>
    ///   <para>This is invoked on behaviours when authority is removed.</para>
    /// </summary>
    public virtual void OnStopAuthority()
    {
    }

    public virtual bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
      return false;
    }

    /// <summary>
    ///   <para>Callback used by the visibility system for objects on a host.</para>
    /// </summary>
    /// <param name="vis">New visibility state.</param>
    public virtual void OnSetLocalVisibility(bool vis)
    {
    }

    /// <summary>
    ///   <para>Callback used by the visibility system to determine if an observer (player) can see this object.</para>
    /// </summary>
    /// <param name="conn">Network connection of a player.</param>
    /// <returns>
    ///   <para>True if the player can see this object.</para>
    /// </returns>
    public virtual bool OnCheckObserver(NetworkConnection conn)
    {
      return true;
    }

    /// <summary>
    ///   <para>This virtual function is used to specify the QoS channel to use for SyncVar updates for this script.</para>
    /// </summary>
    /// <returns>
    ///   <para>The QoS channel for this script.</para>
    /// </returns>
    public virtual int GetNetworkChannel()
    {
      return 0;
    }

    /// <summary>
    ///   <para>This virtual function is used to specify the send interval to use for SyncVar updates for this script.</para>
    /// </summary>
    /// <returns>
    ///   <para>The time in seconds between updates.</para>
    /// </returns>
    public virtual float GetNetworkSendInterval()
    {
      return 0.1f;
    }

    protected enum UNetInvokeType
    {
      Command,
      ClientRpc,
      SyncEvent,
      SyncList,
    }

    protected class Invoker
    {
      public NetworkBehaviour.UNetInvokeType invokeType;
      public System.Type invokeClass;
      public NetworkBehaviour.CmdDelegate invokeFunction;

      public string DebugString()
      {
        return ((int) this.invokeType).ToString() + ":" + (object) this.invokeClass + ":" + this.invokeFunction.Method.Name;
      }
    }

    /// <summary>
    ///   <para>Delegate for Command functions.</para>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="reader"></param>
    public delegate void CmdDelegate(NetworkBehaviour obj, NetworkReader reader);

    /// <summary>
    ///   <para>Delegate for Event functions.</para>
    /// </summary>
    /// <param name="targets"></param>
    /// <param name="reader"></param>
    protected delegate void EventDelegate(List<Delegate> targets, NetworkReader reader);
  }
}
