// Decompiled with JetBrains decompiler
// Type: UnityEngine.Cloth
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Cloth class provides an interface to cloth simulation physics.</para>
  /// </summary>
  public sealed class Cloth : Component
  {
    /// <summary>
    ///   <para>Cloth's sleep threshold.</para>
    /// </summary>
    public float sleepThreshold { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Bending stiffness of the cloth.</para>
    /// </summary>
    public float bendingStiffness { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Stretching stiffness of the cloth.</para>
    /// </summary>
    public float stretchingStiffness { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Damp cloth motion.</para>
    /// </summary>
    public float damping { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A constant, external acceleration applied to the cloth.</para>
    /// </summary>
    public Vector3 externalAcceleration
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_externalAcceleration(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_externalAcceleration(ref value);
      }
    }

    /// <summary>
    ///   <para>A random, external acceleration applied to the cloth.</para>
    /// </summary>
    public Vector3 randomAcceleration
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_randomAcceleration(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_randomAcceleration(ref value);
      }
    }

    /// <summary>
    ///   <para>Should gravity affect the cloth simulation?</para>
    /// </summary>
    public bool useGravity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("Deprecated. Cloth.selfCollisions is no longer supported since Unity 5.0.", true)]
    public bool selfCollision { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is this cloth enabled?</para>
    /// </summary>
    public bool enabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The current vertex positions of the cloth object.</para>
    /// </summary>
    public Vector3[] vertices { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current normals of the cloth object.</para>
    /// </summary>
    public Vector3[] normals { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The friction of the cloth when colliding with the character.</para>
    /// </summary>
    public float friction { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much to increase mass of colliding particles.</para>
    /// </summary>
    public float collisionMassScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enable continuous collision to improve collision stability.</para>
    /// </summary>
    public float useContinuousCollision { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Add one virtual particle per triangle to improve collision stability.</para>
    /// </summary>
    public float useVirtualParticles { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The cloth skinning coefficients used to set up how the cloth interacts with the skinned mesh.</para>
    /// </summary>
    public ClothSkinningCoefficient[] coefficients { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much world-space movement of the character will affect cloth vertices.</para>
    /// </summary>
    public float worldVelocityScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much world-space acceleration of the character will affect cloth vertices.</para>
    /// </summary>
    public float worldAccelerationScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Number of solver iterations per second.</para>
    /// </summary>
    public bool solverFrequency { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>An array of CapsuleColliders which this Cloth instance should collide with.</para>
    /// </summary>
    public CapsuleCollider[] capsuleColliders { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>An array of ClothSphereColliderPairs which this Cloth instance should collide with.</para>
    /// </summary>
    public ClothSphereColliderPair[] sphereColliders { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_externalAcceleration(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_externalAcceleration(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_randomAcceleration(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_randomAcceleration(ref Vector3 value);

    /// <summary>
    ///   <para>Clear the pending transform changes from affecting the cloth simulation.</para>
    /// </summary>
    public void ClearTransformMotion()
    {
      Cloth.INTERNAL_CALL_ClearTransformMotion(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_ClearTransformMotion(Cloth self);

    /// <summary>
    ///   <para>Fade the cloth simulation in or out.</para>
    /// </summary>
    /// <param name="enabled">Fading enabled or not.</param>
    /// <param name="interpolationTime"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetEnabledFading(bool enabled, [DefaultValue("0.5f")] float interpolationTime);

    [ExcludeFromDocs]
    public void SetEnabledFading(bool enabled)
    {
      float interpolationTime = 0.5f;
      this.SetEnabledFading(enabled, interpolationTime);
    }
  }
}
