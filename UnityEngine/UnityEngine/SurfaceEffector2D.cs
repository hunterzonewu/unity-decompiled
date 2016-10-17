// Decompiled with JetBrains decompiler
// Type: UnityEngine.SurfaceEffector2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Applies tangent forces along the surfaces of colliders.</para>
  /// </summary>
  public sealed class SurfaceEffector2D : Effector2D
  {
    /// <summary>
    ///   <para>The speed to be maintained along the surface.</para>
    /// </summary>
    public float speed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The speed variation (from zero to the variation) added to base speed to be applied.</para>
    /// </summary>
    public float speedVariation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The scale of the impulse force applied while attempting to reach the surface speed.</para>
    /// </summary>
    public float forceScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the impulse force but applied to the contact point?</para>
    /// </summary>
    public bool useContactForce { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should friction be used for any contact with the surface?</para>
    /// </summary>
    public bool useFriction { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should bounce be used for any contact with the surface?</para>
    /// </summary>
    public bool useBounce { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
