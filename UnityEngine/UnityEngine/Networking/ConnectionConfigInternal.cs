// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ConnectionConfigInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
  internal sealed class ConnectionConfigInternal : IDisposable
  {
    internal IntPtr m_Ptr;

    public int ChannelSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private ConnectionConfigInternal()
    {
    }

    public ConnectionConfigInternal(ConnectionConfig config)
    {
      if (config == null)
        throw new NullReferenceException("config is not defined");
      this.InitWrapper();
      this.InitPacketSize(config.PacketSize);
      this.InitFragmentSize(config.FragmentSize);
      this.InitResendTimeout(config.ResendTimeout);
      this.InitDisconnectTimeout(config.DisconnectTimeout);
      this.InitConnectTimeout(config.ConnectTimeout);
      this.InitMinUpdateTimeout(config.MinUpdateTimeout);
      this.InitPingTimeout(config.PingTimeout);
      this.InitReducedPingTimeout(config.ReducedPingTimeout);
      this.InitAllCostTimeout(config.AllCostTimeout);
      this.InitNetworkDropThreshold(config.NetworkDropThreshold);
      this.InitOverflowDropThreshold(config.OverflowDropThreshold);
      this.InitMaxConnectionAttempt(config.MaxConnectionAttempt);
      this.InitAckDelay(config.AckDelay);
      this.InitMaxCombinedReliableMessageSize(config.MaxCombinedReliableMessageSize);
      this.InitMaxCombinedReliableMessageCount(config.MaxCombinedReliableMessageCount);
      this.InitMaxSentMessageQueueSize(config.MaxSentMessageQueueSize);
      this.InitIsAcksLong(config.IsAcksLong);
      this.InitUsePlatformSpecificProtocols(config.UsePlatformSpecificProtocols);
      this.InitWebSocketReceiveBufferMaxSize(config.WebSocketReceiveBufferMaxSize);
      for (byte idx = 0; (int) idx < config.ChannelCount; ++idx)
      {
        int num = (int) this.AddChannel(config.GetChannel(idx));
      }
    }

    ~ConnectionConfigInternal()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitWrapper();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public byte AddChannel(QosType value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public QosType GetChannel(int i);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitPacketSize(ushort value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitFragmentSize(ushort value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitResendTimeout(uint value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitDisconnectTimeout(uint value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitConnectTimeout(uint value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitMinUpdateTimeout(uint value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitPingTimeout(uint value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitReducedPingTimeout(uint value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitAllCostTimeout(uint value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitNetworkDropThreshold(byte value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitOverflowDropThreshold(byte value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitMaxConnectionAttempt(byte value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitAckDelay(uint value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitMaxCombinedReliableMessageSize(ushort value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitMaxCombinedReliableMessageCount(ushort value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitMaxSentMessageQueueSize(ushort value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitIsAcksLong(bool value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitUsePlatformSpecificProtocols(bool value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitWebSocketReceiveBufferMaxSize(ushort value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();
  }
}
