// Decompiled with JetBrains decompiler
// Type: UnityEngine.RPCMode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

namespace UnityEngine
{
  /// <summary>
  ///   <para>Option for who will receive an RPC, used by NetworkView.RPC.</para>
  /// </summary>
  public enum RPCMode
  {
    Server = 0,
    Others = 1,
    All = 2,
    OthersBuffered = 5,
    AllBuffered = 6,
  }
}
