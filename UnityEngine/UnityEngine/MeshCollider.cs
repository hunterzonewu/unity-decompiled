// Decompiled with JetBrains decompiler
// Type: UnityEngine.MeshCollider
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A mesh collider allows you to do between meshes and primitives.</para>
  /// </summary>
  public sealed class MeshCollider : Collider
  {
    /// <summary>
    ///   <para>The mesh object used for collision detection.</para>
    /// </summary>
    public Mesh sharedMesh { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use a convex collider from the mesh.</para>
    /// </summary>
    public bool convex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Uses interpolated normals for sphere collisions instead of flat polygonal normals.</para>
    /// </summary>
    [Obsolete("Configuring smooth sphere collisions is no longer needed. PhysX3 has a better behaviour in place.")]
    public bool smoothSphereCollisions
    {
      get
      {
        return true;
      }
      set
      {
      }
    }
  }
}
