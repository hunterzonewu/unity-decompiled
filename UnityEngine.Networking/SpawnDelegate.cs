// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.SpawnDelegate
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>Signature of spawn functions that are passed to NetworkClient.RegisterSpawnFunction(). This is optional, as in most cases RegisterPrefab will be used instead.</para>
  /// </summary>
  /// <param name="position"></param>
  /// <param name="assetId"></param>
  public delegate GameObject SpawnDelegate(Vector3 position, NetworkHash128 assetId);
}
