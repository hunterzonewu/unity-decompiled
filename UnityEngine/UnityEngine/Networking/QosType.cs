// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.QosType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Descibed allowed types of quality of service for channels.</para>
  /// </summary>
  public enum QosType
  {
    Unreliable,
    UnreliableFragmented,
    UnreliableSequenced,
    Reliable,
    ReliableFragmented,
    ReliableSequenced,
    StateUpdate,
    ReliableStateUpdate,
    AllCostDelivery,
  }
}
