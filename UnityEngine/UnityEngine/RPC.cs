// Decompiled with JetBrains decompiler
// Type: UnityEngine.RPC
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Attribute for setting up RPC functions.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  [RequiredByNativeCode]
  [Obsolete("NetworkView RPC functions are deprecated. Refer to the new Multiplayer Networking system.")]
  public sealed class RPC : Attribute
  {
  }
}
