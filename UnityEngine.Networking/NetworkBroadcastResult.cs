// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkBroadcastResult
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A structure that contains data from a NetworkDiscovery server broadcast.</para>
  /// </summary>
  public struct NetworkBroadcastResult
  {
    /// <summary>
    ///   <para>The IP address of the server that broadcasts this data.</para>
    /// </summary>
    public string serverAddress;
    /// <summary>
    ///   <para>The data broadcast by the server.</para>
    /// </summary>
    public byte[] broadcastData;
  }
}
