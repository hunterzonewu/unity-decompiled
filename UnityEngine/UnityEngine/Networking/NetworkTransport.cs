// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkTransport
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Internal;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Low level (transport layer) API.</para>
  /// </summary>
  public sealed class NetworkTransport
  {
    /// <summary>
    ///   <para>Obsolete, will be removed.</para>
    /// </summary>
    public static extern bool IsStarted { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private NetworkTransport()
    {
    }

    internal static bool DoesEndPointUsePlatformProtocols(EndPoint endPoint)
    {
      if (endPoint.GetType().FullName == "UnityEngine.PS4.SceEndPoint")
      {
        SocketAddress socketAddress = endPoint.Serialize();
        if ((int) socketAddress[8] != 0 || (int) socketAddress[9] != 0)
          return true;
      }
      return false;
    }

    public static int ConnectEndPoint(int hostId, EndPoint endPoint, int exceptionConnectionId, out byte error)
    {
      error = (byte) 0;
      byte[] numArray1 = new byte[4]{ (byte) 95, (byte) 36, (byte) 19, (byte) 246 };
      if (endPoint == null)
        throw new NullReferenceException("Null EndPoint provided");
      if (endPoint.GetType().FullName != "UnityEngine.XboxOne.XboxOneEndPoint" && endPoint.GetType().FullName != "UnityEngine.PS4.SceEndPoint")
        throw new ArgumentException("Endpoint of type XboxOneEndPoint or SceEndPoint  required");
      if (endPoint.GetType().FullName == "UnityEngine.XboxOne.XboxOneEndPoint")
      {
        EndPoint endPoint1 = endPoint;
        if (endPoint1.AddressFamily != AddressFamily.InterNetworkV6)
          throw new ArgumentException("XboxOneEndPoint has an invalid family");
        SocketAddress socketAddress = endPoint1.Serialize();
        if (socketAddress.Size != 14)
          throw new ArgumentException("XboxOneEndPoint has an invalid size");
        if ((int) socketAddress[0] != 0 || (int) socketAddress[1] != 0)
          throw new ArgumentException("XboxOneEndPoint has an invalid family signature");
        if ((int) socketAddress[2] != (int) numArray1[0] || (int) socketAddress[3] != (int) numArray1[1] || ((int) socketAddress[4] != (int) numArray1[2] || (int) socketAddress[5] != (int) numArray1[3]))
          throw new ArgumentException("XboxOneEndPoint has an invalid signature");
        byte[] numArray2 = new byte[8];
        for (int index = 0; index < numArray2.Length; ++index)
          numArray2[index] = socketAddress[6 + index];
        IntPtr num = new IntPtr(BitConverter.ToInt64(numArray2, 0));
        if (num == IntPtr.Zero)
          throw new ArgumentException("XboxOneEndPoint has an invalid SOCKET_STORAGE pointer");
        byte[] destination = new byte[2];
        Marshal.Copy(num, destination, 0, destination.Length);
        if (((int) destination[1] << 8) + (int) destination[0] != 23)
          throw new ArgumentException("XboxOneEndPoint has corrupt or invalid SOCKET_STORAGE pointer");
        return NetworkTransport.Internal_ConnectEndPoint(hostId, num, 128, exceptionConnectionId, out error);
      }
      SocketAddress socketAddress1 = endPoint.Serialize();
      if (socketAddress1.Size != 16)
        throw new ArgumentException("EndPoint has an invalid size");
      if ((int) socketAddress1[0] != socketAddress1.Size)
        throw new ArgumentException("EndPoint has an invalid size value");
      if ((int) socketAddress1[1] != 2)
        throw new ArgumentException("EndPoint has an invalid family value");
      byte[] source = new byte[16];
      for (int index = 0; index < source.Length; ++index)
        source[index] = socketAddress1[index];
      IntPtr num1 = Marshal.AllocHGlobal(source.Length);
      Marshal.Copy(source, 0, num1, source.Length);
      int num2 = NetworkTransport.Internal_ConnectEndPoint(hostId, num1, 16, exceptionConnectionId, out error);
      Marshal.FreeHGlobal(num1);
      return num2;
    }

    /// <summary>
    ///   <para>First function which should be called before any other NetworkTransport function.</para>
    /// </summary>
    public static void Init()
    {
      NetworkTransport.InitWithNoParameters();
    }

    public static void Init(GlobalConfig config)
    {
      NetworkTransport.InitWithParameters(new GlobalConfigInternal(config));
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InitWithNoParameters();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InitWithParameters(GlobalConfigInternal config);

    /// <summary>
    ///   <para>Shutdown the transport layer, after calling this function no any other function can be called.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Shutdown();

    /// <summary>
    ///   <para>The UNet spawning system uses assetIds to identify how spawn remote objects. This function allows you to get the assetId for the prefab associated with an object.</para>
    /// </summary>
    /// <param name="go">Target game object to get asset Id for.</param>
    /// <returns>
    ///   <para>The assetId of the game object's prefab.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetAssetId(GameObject go);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void AddSceneId(int id);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNextSceneId();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ConnectAsNetworkHost(int hostId, string address, int port, NetworkID network, SourceID source, NodeID node, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void DisconnectNetworkHost(int hostId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NetworkEventType ReceiveRelayEventFromHost(int hostId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int ConnectToNetworkPeer(int hostId, string address, int port, int exceptionConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, int bytesPerSec, float bucketSizeFactor, out byte error);

    public static int ConnectToNetworkPeer(int hostId, string address, int port, int exceptionConnectionId, int relaySlotId, NetworkID network, SourceID source, NodeID node, out byte error)
    {
      return NetworkTransport.ConnectToNetworkPeer(hostId, address, port, exceptionConnectionId, relaySlotId, network, source, node, 0, 0.0f, out error);
    }

    /// <summary>
    ///   <para>Return value of messages waiting for reading.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCurrentIncomingMessageAmount();

    /// <summary>
    ///   <para>Return total message amount waiting for sending.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCurrentOutgoingMessageAmount();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetCurrentRtt(int hostId, int connectionId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNetworkLostPacketNum(int hostId, int connectionId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetPacketSentRate(int hostId, int connectionId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetPacketReceivedRate(int hostId, int connectionId, out byte error);

    [WrapperlessIcall]
    [Obsolete("GetRemotePacketReceivedRate has been made obsolete. Please do not use this function.")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetRemotePacketReceivedRate(int hostId, int connectionId, out byte error);

    /// <summary>
    ///   <para>Function returns time spent on network io operations in micro seconds.</para>
    /// </summary>
    /// <returns>
    ///   <para>Time in micro seconds.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNetIOTimeuS();

    public static void GetConnectionInfo(int hostId, int connectionId, out string address, out int port, out NetworkID network, out NodeID dstNode, out byte error)
    {
      ulong network1;
      ushort dstNode1;
      address = NetworkTransport.GetConnectionInfo(hostId, connectionId, out port, out network1, out dstNode1, out error);
      network = (NetworkID) network1;
      dstNode = (NodeID) dstNode1;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetConnectionInfo(int hostId, int connectionId, out int port, out ulong network, out ushort dstNode, out byte error);

    /// <summary>
    ///   <para>Get UNET timestamp which can be added to message for further definitions of packet delaying.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetNetworkTimestamp();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetRemoteDelayTimeMS(int hostId, int connectionId, int remoteTime, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool StartSendMulticast(int hostId, int channelId, byte[] buffer, int size, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool SendMulticast(int hostId, int connectionId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool FinishSendMulticast(int hostId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetMaxPacketSize();

    private static void CheckTopology(HostTopology topology)
    {
      int maxPacketSize = NetworkTransport.GetMaxPacketSize();
      if ((int) topology.DefaultConfig.PacketSize > maxPacketSize)
        throw new ArgumentOutOfRangeException("Default config: packet size should be less than packet size defined in global config: " + maxPacketSize.ToString());
      for (int index = 0; index < topology.SpecialConnectionConfigs.Count; ++index)
      {
        if ((int) topology.SpecialConnectionConfigs[index].PacketSize > maxPacketSize)
          throw new ArgumentOutOfRangeException("Special config " + index.ToString() + ": packet size should be less than packet size defined in global config: " + maxPacketSize.ToString());
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int AddWsHostWrapper(HostTopologyInternal topologyInt, string ip, int port);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int AddWsHostWrapperWithoutIp(HostTopologyInternal topologyInt, int port);

    /// <summary>
    ///         <para>Created web socket host.
    /// This function is supported only for Editor (Win, Linux, Mac) and StandalonePlayers (Win, Linux, Mac)
    /// Topology is used to define how many client can connect, and how many messages should be preallocated in send and receive pool, all other parameters are ignored.</para>
    ///       </summary>
    /// <param name="port">Listening tcp port.</param>
    /// <param name="topology">Topology.</param>
    /// <param name="ip"></param>
    /// <returns>
    ///   <para>Web socket host id.</para>
    /// </returns>
    [ExcludeFromDocs]
    public static int AddWebsocketHost(HostTopology topology, int port)
    {
      string ip = (string) null;
      return NetworkTransport.AddWebsocketHost(topology, port, ip);
    }

    public static int AddWebsocketHost(HostTopology topology, int port, [DefaultValue("null")] string ip)
    {
      if (topology == null)
        throw new NullReferenceException("topology is not defined");
      if (ip == null)
        return NetworkTransport.AddWsHostWrapperWithoutIp(new HostTopologyInternal(topology), port);
      return NetworkTransport.AddWsHostWrapper(new HostTopologyInternal(topology), ip, port);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int AddHostWrapper(HostTopologyInternal topologyInt, string ip, int port, int minTimeout, int maxTimeout);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int AddHostWrapperWithoutIp(HostTopologyInternal topologyInt, int port, int minTimeout, int maxTimeout);

    [ExcludeFromDocs]
    public static int AddHost(HostTopology topology, int port)
    {
      string ip = (string) null;
      return NetworkTransport.AddHost(topology, port, ip);
    }

    [ExcludeFromDocs]
    public static int AddHost(HostTopology topology)
    {
      string ip = (string) null;
      int port = 0;
      return NetworkTransport.AddHost(topology, port, ip);
    }

    /// <summary>
    ///   <para>Will create a host (open socket) with given topology and optionally port and IP.</para>
    /// </summary>
    /// <param name="topology">The host topology for this host.</param>
    /// <param name="port">Bind to specific port, if 0 is selected the port will chosen by OS.</param>
    /// <param name="ip">Bind to specific IP address.</param>
    /// <returns>
    ///   <para>Returns host ID just created.</para>
    /// </returns>
    public static int AddHost(HostTopology topology, [DefaultValue("0")] int port, [DefaultValue("null")] string ip)
    {
      if (topology == null)
        throw new NullReferenceException("topology is not defined");
      if (ip == null)
        return NetworkTransport.AddHostWrapperWithoutIp(new HostTopologyInternal(topology), port, 0, 0);
      return NetworkTransport.AddHostWrapper(new HostTopologyInternal(topology), ip, port, 0, 0);
    }

    [ExcludeFromDocs]
    public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, int port)
    {
      string ip = (string) null;
      return NetworkTransport.AddHostWithSimulator(topology, minTimeout, maxTimeout, port, ip);
    }

    [ExcludeFromDocs]
    public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout)
    {
      string ip = (string) null;
      int port = 0;
      return NetworkTransport.AddHostWithSimulator(topology, minTimeout, maxTimeout, port, ip);
    }

    /// <summary>
    ///   <para>Create a host (open socket) and configure them to simulate internet latency (works on editor and development build only).</para>
    /// </summary>
    /// <param name="topology">The host topology for this host.</param>
    /// <param name="minTimeout">Minimum simulated delay.</param>
    /// <param name="maxTimeout">Maximum simulated delay.</param>
    /// <param name="port">Bind to specific port, if 0 is selected the port will chosen by OS.</param>
    /// <param name="ip">Bind to specific IP address.</param>
    /// <returns>
    ///   <para>Returns host ID just created.</para>
    /// </returns>
    public static int AddHostWithSimulator(HostTopology topology, int minTimeout, int maxTimeout, [DefaultValue("0")] int port, [DefaultValue("null")] string ip)
    {
      if (topology == null)
        throw new NullReferenceException("topology is not defined");
      if (ip == null)
        return NetworkTransport.AddHostWrapperWithoutIp(new HostTopologyInternal(topology), port, minTimeout, maxTimeout);
      return NetworkTransport.AddHostWrapper(new HostTopologyInternal(topology), ip, port, minTimeout, maxTimeout);
    }

    /// <summary>
    ///   <para>Close opened socket, close all connection belonging this socket.</para>
    /// </summary>
    /// <param name="hostId">If of opened udp socket.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RemoveHost(int hostId);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int Connect(int hostId, string address, int port, int exeptionConnectionId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int Internal_ConnectEndPoint(int hostId, IntPtr sockAddrStorage, int sockAddrStorageLen, int exceptionConnectionId, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int ConnectWithSimulator(int hostId, string address, int port, int exeptionConnectionId, out byte error, ConnectionSimulatorConfig conf);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Disconnect(int hostId, int connectionId, out byte error);

    public static bool Send(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error)
    {
      if (buffer == null)
        throw new NullReferenceException("send buffer is not initialized");
      return NetworkTransport.SendWrapper(hostId, connectionId, channelId, buffer, size, out error);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool SendWrapper(int hostId, int connectionId, int channelId, byte[] buffer, int size, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NetworkEventType Receive(out int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern NetworkEventType ReceiveFromHost(int hostId, out int connectionId, out int channelId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetPacketStat(int direction, int packetStatId, int numMsgs, int numBytes);

    public static bool StartBroadcastDiscovery(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error)
    {
      if (buffer != null)
      {
        if (buffer.Length < size)
          throw new ArgumentOutOfRangeException("Size: " + (object) size + " > buffer.Length " + (object) buffer.Length);
        if (size == 0)
          throw new ArgumentOutOfRangeException("Size is zero while buffer exists, please pass null and 0 as buffer and size parameters");
      }
      if (buffer == null)
        return NetworkTransport.StartBroadcastDiscoveryWithoutData(hostId, broadcastPort, key, version, subversion, timeout, out error);
      return NetworkTransport.StartBroadcastDiscoveryWithData(hostId, broadcastPort, key, version, subversion, buffer, size, timeout, out error);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool StartBroadcastDiscoveryWithoutData(int hostId, int broadcastPort, int key, int version, int subversion, int timeout, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool StartBroadcastDiscoveryWithData(int hostId, int broadcastPort, int key, int version, int subversion, byte[] buffer, int size, int timeout, out byte error);

    /// <summary>
    ///   <para>Stop sending broadcast discovery message.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void StopBroadcastDiscovery();

    /// <summary>
    ///   <para>Check if broadcastdiscovery sender works.</para>
    /// </summary>
    /// <returns>
    ///   <para>True if it works.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsBroadcastDiscoveryRunning();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetBroadcastCredentials(int hostId, int key, int version, int subversion, out byte error);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetBroadcastConnectionInfo(int hostId, out int port, out byte error);

    public static void GetBroadcastConnectionInfo(int hostId, out string address, out int port, out byte error)
    {
      address = NetworkTransport.GetBroadcastConnectionInfo(hostId, out port, out error);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetBroadcastConnectionMessage(int hostId, byte[] buffer, int bufferSize, out int receivedSize, out byte error);
  }
}
