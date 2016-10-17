// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleAnimator
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>(Legacy Particles) Particle animators move your particles over time, you use them to apply wind, drag &amp; color cycling to your particle emitters.</para>
  /// </summary>
  public sealed class ParticleAnimator : Component
  {
    /// <summary>
    ///   <para>Do particles cycle their color over their lifetime?</para>
    /// </summary>
    public bool doesAnimateColor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>World space axis the particles rotate around.</para>
    /// </summary>
    public Vector3 worldRotationAxis
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_worldRotationAxis(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_worldRotationAxis(ref value);
      }
    }

    /// <summary>
    ///   <para>Local space axis the particles rotate around.</para>
    /// </summary>
    public Vector3 localRotationAxis
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_localRotationAxis(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_localRotationAxis(ref value);
      }
    }

    /// <summary>
    ///   <para>How the particle sizes grow over their lifetime.</para>
    /// </summary>
    public float sizeGrow { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A random force added to particles every frame.</para>
    /// </summary>
    public Vector3 rndForce
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_rndForce(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_rndForce(ref value);
      }
    }

    /// <summary>
    ///   <para>The force being applied to particles every frame.</para>
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
    ///   <para>How much particles are slowed down every frame.</para>
    /// </summary>
    public float damping { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does the GameObject of this particle animator auto destructs?</para>
    /// </summary>
    public bool autodestruct { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Colors the particles will cycle through over their lifetime.</para>
    /// </summary>
    public Color[] colorAnimation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_worldRotationAxis(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_worldRotationAxis(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localRotationAxis(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_localRotationAxis(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_rndForce(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_rndForce(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_force(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_force(ref Vector3 value);
  }
}
