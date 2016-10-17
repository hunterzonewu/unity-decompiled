// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.HostTopologyInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
  internal sealed class HostTopologyInternal : IDisposable
  {
    internal IntPtr m_Ptr;

    public HostTopologyInternal(HostTopology topology)
    {
      this.InitWrapper(new ConnectionConfigInternal(topology.DefaultConfig), topology.MaxDefaultConnections);
      for (int i = 1; i <= topology.SpecialConnectionConfigsCount; ++i)
        this.AddSpecialConnectionConfig(new ConnectionConfigInternal(topology.GetSpecialConnectionConfig(i)));
      this.InitOtherParameters(topology);
    }

    ~HostTopologyInternal()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitWrapper(ConnectionConfigInternal config, int maxDefaultConnections);

    private int AddSpecialConnectionConfig(ConnectionConfigInternal config)
    {
      return this.AddSpecialConnectionConfigWrapper(config);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int AddSpecialConnectionConfigWrapper(ConnectionConfigInternal config);

    private void InitOtherParameters(HostTopology topology)
    {
      this.InitReceivedPoolSize(topology.ReceivedMessagePoolSize);
      this.InitSentMessagePoolSize(topology.SentMessagePoolSize);
      this.InitMessagePoolSizeGrowthFactor(topology.MessagePoolSizeGrowthFactor);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitReceivedPoolSize(ushort pool);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitSentMessagePoolSize(ushort pool);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitMessagePoolSizeGrowthFactor(float factor);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();
  }
}
