// Decompiled with JetBrains decompiler
// Type: UnityEngine.FrictionJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Applies both force and torque to reduce both the linear and angular velocities to zero.</para>
  /// </summary>
  public sealed class FrictionJoint2D : AnchoredJoint2D
  {
    /// <summary>
    ///   <para>The maximum force that can be generated when trying to maintain the friction joint constraint.</para>
    /// </summary>
    public float maxForce { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum torque that can be generated when trying to maintain the friction joint constraint.</para>
    /// </summary>
    public float maxTorque { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
