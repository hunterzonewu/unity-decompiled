// Decompiled with JetBrains decompiler
// Type: UnityEngine.PointEffector2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Applies forces to attract/repulse against a point.</para>
  /// </summary>
  public sealed class PointEffector2D : Effector2D
  {
    /// <summary>
    ///   <para>The magnitude of the force to be applied.</para>
    /// </summary>
    public float forceMagnitude { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The variation of the magnitude of the force to be applied.</para>
    /// </summary>
    public float forceVariation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The scale applied to the calculated distance between source and target.</para>
    /// </summary>
    public float distanceScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The linear drag to apply to rigid-bodies.</para>
    /// </summary>
    public float drag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The angular drag to apply to rigid-bodies.</para>
    /// </summary>
    public float angularDrag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The source which is used to calculate the centroid point of the effector.  The distance from the target is defined from this point.</para>
    /// </summary>
    public EffectorSelection2D forceSource { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The target for where the effector applies any force.</para>
    /// </summary>
    public EffectorSelection2D forceTarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The mode used to apply the effector force.</para>
    /// </summary>
    public EffectorForceMode2D forceMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
