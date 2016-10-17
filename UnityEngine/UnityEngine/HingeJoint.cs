// Decompiled with JetBrains decompiler
// Type: UnityEngine.HingeJoint
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The HingeJoint groups together 2 rigid bodies, constraining them to move like connected by a hinge.</para>
  /// </summary>
  public sealed class HingeJoint : Joint
  {
    /// <summary>
    ///   <para>The motor will apply a force up to a maximum force to achieve the target velocity in degrees per second.</para>
    /// </summary>
    public JointMotor motor
    {
      get
      {
        JointMotor jointMotor;
        this.INTERNAL_get_motor(out jointMotor);
        return jointMotor;
      }
      set
      {
        this.INTERNAL_set_motor(ref value);
      }
    }

    /// <summary>
    ///   <para>Limit of angular rotation (in degrees) on the hinge joint.</para>
    /// </summary>
    public JointLimits limits
    {
      get
      {
        JointLimits jointLimits;
        this.INTERNAL_get_limits(out jointLimits);
        return jointLimits;
      }
      set
      {
        this.INTERNAL_set_limits(ref value);
      }
    }

    /// <summary>
    ///   <para>The spring attempts to reach a target angle by adding spring and damping forces.</para>
    /// </summary>
    public JointSpring spring
    {
      get
      {
        JointSpring jointSpring;
        this.INTERNAL_get_spring(out jointSpring);
        return jointSpring;
      }
      set
      {
        this.INTERNAL_set_spring(ref value);
      }
    }

    /// <summary>
    ///   <para>Enables the joint's motor. Disabled by default.</para>
    /// </summary>
    public bool useMotor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables the joint's limits. Disabled by default.</para>
    /// </summary>
    public bool useLimits { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables the joint's spring. Disabled by default.</para>
    /// </summary>
    public bool useSpring { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The angular velocity of the joint in degrees per second.</para>
    /// </summary>
    public float velocity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current angle in degrees of the joint relative to its rest position. (Read Only)</para>
    /// </summary>
    public float angle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_motor(out JointMotor value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_motor(ref JointMotor value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_limits(out JointLimits value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_limits(ref JointLimits value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_spring(out JointSpring value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_spring(ref JointSpring value);
  }
}
