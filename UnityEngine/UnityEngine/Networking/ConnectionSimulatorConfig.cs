// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.ConnectionSimulatorConfig
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Create configuration for network simulator; You can use this class in editor and developer build only.</para>
  /// </summary>
  public sealed class ConnectionSimulatorConfig : IDisposable
  {
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>Will create object describing network simulation parameters.</para>
    /// </summary>
    /// <param name="outMinDelay">Minimal simulation delay for outgoing traffic in ms.</param>
    /// <param name="outAvgDelay">Average simulation delay for outgoing traffic in ms.</param>
    /// <param name="inMinDelay">Minimal  simulation delay for incoming traffic in ms.</param>
    /// <param name="inAvgDelay">Average  simulation delay for incoming traffic in ms.</param>
    /// <param name="packetLossPercentage">Probability of packet loss  0 &lt;= p &lt;= 1.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public ConnectionSimulatorConfig(int outMinDelay, int outAvgDelay, int inMinDelay, int inAvgDelay, float packetLossPercentage);

    ~ConnectionSimulatorConfig()
    {
      this.Dispose();
    }

    /// <summary>
    ///   <para>Destructor.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Dispose();
  }
}
