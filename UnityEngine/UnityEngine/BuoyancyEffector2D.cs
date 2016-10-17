// Decompiled with JetBrains decompiler
// Type: UnityEngine.BuoyancyEffector2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Applies forces to simulate buoyancy, fluid-flow and fluid drag.</para>
  /// </summary>
  public sealed class BuoyancyEffector2D : Effector2D
  {
    /// <summary>
    ///   <para>Defines an arbitrary horizontal line that represents the fluid surface level.</para>
    /// </summary>
    public float surfaceLevel { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The density of the fluid used to calculate the buoyancy forces.</para>
    /// </summary>
    public float density { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A force applied to slow linear movement of any Collider2D in contact with the effector.</para>
    /// </summary>
    public float linearDrag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A force applied to slow angular movement of any Collider2D in contact with the effector.</para>
    /// </summary>
    public float angularDrag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The angle of the force used to similate fluid flow.</para>
    /// </summary>
    public float flowAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The magnitude of the force used to similate fluid flow.</para>
    /// </summary>
    public float flowMagnitude { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The random variation of the force used to similate fluid flow.</para>
    /// </summary>
    public float flowVariation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
