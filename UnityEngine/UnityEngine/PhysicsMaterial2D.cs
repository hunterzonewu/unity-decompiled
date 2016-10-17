// Decompiled with JetBrains decompiler
// Type: UnityEngine.PhysicsMaterial2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Asset type that defines the surface properties of a Collider2D.</para>
  /// </summary>
  public sealed class PhysicsMaterial2D : Object
  {
    /// <summary>
    ///   <para>The degree of elasticity during collisions.</para>
    /// </summary>
    public float bounciness { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Coefficient of friction.</para>
    /// </summary>
    public float friction { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public PhysicsMaterial2D()
    {
      PhysicsMaterial2D.Internal_Create(this, (string) null);
    }

    public PhysicsMaterial2D(string name)
    {
      PhysicsMaterial2D.Internal_Create(this, name);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] PhysicsMaterial2D mat, string name);
  }
}
