// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.GlobalConfigInternal
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
  internal sealed class GlobalConfigInternal : IDisposable
  {
    internal IntPtr m_Ptr;

    public GlobalConfigInternal(GlobalConfig config)
    {
      this.InitWrapper();
      this.InitThreadAwakeTimeout(config.ThreadAwakeTimeout);
      this.InitReactorModel((byte) config.ReactorModel);
      this.InitReactorMaximumReceivedMessages(config.ReactorMaximumReceivedMessages);
      this.InitReactorMaximumSentMessages(config.ReactorMaximumSentMessages);
      this.InitMaxPacketSize(config.MaxPacketSize);
    }

    ~GlobalConfigInternal()
    {
      this.Dispose();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitWrapper();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitThreadAwakeTimeout(uint ms);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitReactorModel(byte model);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitReactorMaximumReceivedMessages(ushort size);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitReactorMaximumSentMessages(ushort size);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InitMaxPacketSize(ushort size);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();
  }
}
