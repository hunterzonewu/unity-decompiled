// Decompiled with JetBrains decompiler
// Type: UnityEngine.Collider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Parent class for collider types used with 2D gameplay.</para>
  /// </summary>
  public class Collider2D : Behaviour
  {
    /// <summary>
    ///   <para>TThe density of the collider used to calculate its mass (when auto mass is enabled)</para>
    /// </summary>
    public float density { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is this collider configured as a trigger?</para>
    /// </summary>
    public bool isTrigger { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Whether the collider is used by an attached effector or not.</para>
    /// </summary>
    public bool usedByEffector { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The local offset of the collider geometry.</para>
    /// </summary>
    public Vector2 offset
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_offset(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_offset(ref value);
      }
    }

    /// <summary>
    ///   <para>The Rigidbody2D attached to the Collider2D's GameObject.</para>
    /// </summary>
    public Rigidbody2D attachedRigidbody { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of separate shaped regions in the collider.</para>
    /// </summary>
    public int shapeCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The world space bounding area of the collider.</para>
    /// </summary>
    public Bounds bounds
    {
      get
      {
        Bounds bounds;
        this.INTERNAL_get_bounds(out bounds);
        return bounds;
      }
    }

    internal ColliderErrorState2D errorState { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The PhysicsMaterial2D that is applied to this collider.</para>
    /// </summary>
    public PhysicsMaterial2D sharedMaterial { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_offset(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_offset(ref Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_bounds(out Bounds value);

    /// <summary>
    ///   <para>Check if a collider overlaps a point in space.</para>
    /// </summary>
    /// <param name="point">A point in world space.</param>
    public bool OverlapPoint(Vector2 point)
    {
      return Collider2D.INTERNAL_CALL_OverlapPoint(this, ref point);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_OverlapPoint(Collider2D self, ref Vector2 point);

    /// <summary>
    ///   <para>Check whether this collider is touching the collider or not.</para>
    /// </summary>
    /// <param name="collider">The collider to check if it is touching this collider.</param>
    /// <returns>
    ///   <para>Whether the collider is touching this collider or not.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool IsTouching(Collider2D collider);

    /// <summary>
    ///   <para>Checks whether this collider is touching any colliders on the specified layerMask or not.</para>
    /// </summary>
    /// <param name="layerMask">Any colliders on any of these layers count as touching.</param>
    /// <returns>
    ///   <para>Whether this collider is touching any collider on the specified layerMask or not.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

    [ExcludeFromDocs]
    public bool IsTouchingLayers()
    {
      return this.IsTouchingLayers(-1);
    }
  }
}
