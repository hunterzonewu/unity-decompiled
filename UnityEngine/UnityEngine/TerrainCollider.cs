// Decompiled with JetBrains decompiler
// Type: UnityEngine.TerrainCollider
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A heightmap based collider.</para>
  /// </summary>
  public sealed class TerrainCollider : Collider
  {
    /// <summary>
    ///   <para>The terrain that stores the heightmap.</para>
    /// </summary>
    public TerrainData terrainData { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
