// Decompiled with JetBrains decompiler
// Type: UnityEngine.AreaEffector2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Applies forces within an area.</para>
  /// </summary>
  public sealed class AreaEffector2D : Effector2D
  {
    /// <summary>
    ///   <para>The angle of the force to be applied.</para>
    /// </summary>
    public float forceAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the forceAngle use global space?</para>
    /// </summary>
    public bool useGlobalAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The magnitude of the force to be applied.</para>
    /// </summary>
    public float forceMagnitude { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The variation of the magnitude of the force to be applied.</para>
    /// </summary>
    public float forceVariation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The linear drag to apply to rigid-bodies.</para>
    /// </summary>
    public float drag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The angular drag to apply to rigid-bodies.</para>
    /// </summary>
    public float angularDrag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The target for where the effector applies any force.</para>
    /// </summary>
    public EffectorSelection2D forceTarget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("AreaEffector2D.forceDirection has been deprecated. Use AreaEffector2D.forceAngle instead (UnityUpgradable) -> forceAngle", true)]
    public float forceDirection
    {
      get
      {
        return this.forceAngle;
      }
      set
      {
        this.forceAngle = value;
      }
    }
  }
}
