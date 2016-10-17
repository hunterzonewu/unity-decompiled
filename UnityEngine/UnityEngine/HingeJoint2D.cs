// Decompiled with JetBrains decompiler
// Type: UnityEngine.HingeJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Joint that allows a Rigidbody2D object to rotate around a point in space or a point on another object.</para>
  /// </summary>
  public sealed class HingeJoint2D : AnchoredJoint2D
  {
    /// <summary>
    ///   <para>Should the joint be rotated automatically by a motor torque?</para>
    /// </summary>
    public bool useMotor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should limits be placed on the range of rotation?</para>
    /// </summary>
    public bool useLimits { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Parameters for the motor force applied to the joint.</para>
    /// </summary>
    public JointMotor2D motor
    {
      get
      {
        JointMotor2D jointMotor2D;
        this.INTERNAL_get_motor(out jointMotor2D);
        return jointMotor2D;
      }
      set
      {
        this.INTERNAL_set_motor(ref value);
      }
    }

    /// <summary>
    ///   <para>Limit of angular rotation (in degrees) on the joint.</para>
    /// </summary>
    public JointAngleLimits2D limits
    {
      get
      {
        JointAngleLimits2D jointAngleLimits2D;
        this.INTERNAL_get_limits(out jointAngleLimits2D);
        return jointAngleLimits2D;
      }
      set
      {
        this.INTERNAL_set_limits(ref value);
      }
    }

    /// <summary>
    ///   <para>Gets the state of the joint limit.</para>
    /// </summary>
    public JointLimitState2D limitState { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The angle (in degrees) referenced between the two bodies used as the constraint for the joint.</para>
    /// </summary>
    public float referenceAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current joint angle (in degrees) with respect to the reference angle.</para>
    /// </summary>
    public float jointAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current joint speed.</para>
    /// </summary>
    public float jointSpeed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_motor(out JointMotor2D value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_motor(ref JointMotor2D value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_limits(out JointAngleLimits2D value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_limits(ref JointAngleLimits2D value);

    /// <summary>
    ///   <para>Gets the motor torque of the joint given the specified timestep.</para>
    /// </summary>
    /// <param name="timeStep">The time to calculate the motor torque for.</param>
    public float GetMotorTorque(float timeStep)
    {
      return HingeJoint2D.INTERNAL_CALL_GetMotorTorque(this, timeStep);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float INTERNAL_CALL_GetMotorTorque(HingeJoint2D self, float timeStep);
  }
}
