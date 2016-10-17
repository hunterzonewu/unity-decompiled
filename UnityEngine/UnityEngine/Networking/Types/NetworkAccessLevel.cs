// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.Types.NetworkAccessLevel
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.ComponentModel;

namespace UnityEngine.Networking.Types
{
  /// <summary>
  ///   <para>Describes the access levels granted to this client.</para>
  /// </summary>
  [DefaultValue(NetworkAccessLevel.Invalid)]
  public enum NetworkAccessLevel : ulong
  {
    Invalid = 0,
    User = 1,
    Owner = 2,
    Admin = 4,
  }
}
