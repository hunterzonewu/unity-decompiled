// Decompiled with JetBrains decompiler
// Type: UnityEngine.ConstantForce
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A force applied constantly.</para>
  /// </summary>
  public sealed class ConstantForce : Behaviour
  {
    /// <summary>
    ///   <para>The force applied to the rigidbody every frame.</para>
    /// </summary>
    public Vector3 force
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_force(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_force(ref value);
      }
    }

    /// <summary>
    ///   <para>The force - relative to the rigid bodies coordinate system - applied every frame.</para>
    /// </summary>
    public Vector3 relativeForce
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_relativeForce(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_relativeForce(ref value);
      }
    }

    /// <summary>
    ///   <para>The torque applied to the rigidbody every frame.</para>
    /// </summary>
    public Vector3 torque
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_torque(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_torque(ref value);
      }
    }

    /// <summary>
    ///   <para>The torque - relative to the rigid bodies coordinate system - applied every frame.</para>
    /// </summary>
    public Vector3 relativeTorque
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_relativeTorque(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_relativeTorque(ref value);
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_force(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_force(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_relativeForce(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_relativeForce(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_torque(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_torque(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_relativeTorque(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_relativeTorque(ref Vector3 value);
  }
}
