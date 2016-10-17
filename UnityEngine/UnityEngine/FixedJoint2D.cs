// Decompiled with JetBrains decompiler
// Type: UnityEngine.FixedJoint2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Connects two Rigidbody2D together at their anchor points using a configurable spring.</para>
  /// </summary>
  public sealed class FixedJoint2D : AnchoredJoint2D
  {
    /// <summary>
    ///   <para>The amount by which the spring force is reduced in proportion to the movement speed.</para>
    /// </summary>
    public float dampingRatio { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The frequency at which the spring oscillates around the distance between the objects.</para>
    /// </summary>
    public float frequency { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The angle referenced between the two bodies used as the constraint for the joint.</para>
    /// </summary>
    public float referenceAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
