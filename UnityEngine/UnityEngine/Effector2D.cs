// Decompiled with JetBrains decompiler
// Type: UnityEngine.Effector2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A base class for all 2D effectors.</para>
  /// </summary>
  public class Effector2D : Behaviour
  {
    /// <summary>
    ///   <para>Should the collider-mask be used or the global collision matrix?</para>
    /// </summary>
    public bool useColliderMask { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The mask used to select specific layers allowed to interact with the effector.</para>
    /// </summary>
    public int colliderMask { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal bool requiresCollider { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool designedForTrigger { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool designedForNonTrigger { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
