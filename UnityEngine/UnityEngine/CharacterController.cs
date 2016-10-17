// Decompiled with JetBrains decompiler
// Type: UnityEngine.CharacterController
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A CharacterController allows you to easily do movement constrained by collisions without having to deal with a rigidbody.</para>
  /// </summary>
  public sealed class CharacterController : Collider
  {
    /// <summary>
    ///   <para>Was the CharacterController touching the ground during the last move?</para>
    /// </summary>
    public bool isGrounded { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current relative velocity of the Character (see notes).</para>
    /// </summary>
    public Vector3 velocity
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_velocity(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>What part of the capsule collided with the environment during the last CharacterController.Move call.</para>
    /// </summary>
    public CollisionFlags collisionFlags { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The radius of the character's capsule.</para>
    /// </summary>
    public float radius { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The height of the character's capsule.</para>
    /// </summary>
    public float height { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The center of the character's capsule relative to the transform's position.</para>
    /// </summary>
    public Vector3 center
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_center(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_center(ref value);
      }
    }

    /// <summary>
    ///   <para>The character controllers slope limit in degrees.</para>
    /// </summary>
    public float slopeLimit { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The character controllers step offset in meters.</para>
    /// </summary>
    public float stepOffset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The character's collision skin width.</para>
    /// </summary>
    public float skinWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Determines whether other rigidbodies or character controllers collide with this character controller (by default this is always enabled).</para>
    /// </summary>
    public bool detectCollisions { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Moves the character with speed.</para>
    /// </summary>
    /// <param name="speed"></param>
    public bool SimpleMove(Vector3 speed)
    {
      return CharacterController.INTERNAL_CALL_SimpleMove(this, ref speed);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_SimpleMove(CharacterController self, ref Vector3 speed);

    /// <summary>
    ///   <para>A more complex move function taking absolute movement deltas.</para>
    /// </summary>
    /// <param name="motion"></param>
    public CollisionFlags Move(Vector3 motion)
    {
      return CharacterController.INTERNAL_CALL_Move(this, ref motion);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern CollisionFlags INTERNAL_CALL_Move(CharacterController self, ref Vector3 motion);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_velocity(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_center(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_center(ref Vector3 value);
  }
}
