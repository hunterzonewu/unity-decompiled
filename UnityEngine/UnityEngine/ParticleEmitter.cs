// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleEmitter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>(Legacy Particles) Script interface for particle emitters.</para>
  /// </summary>
  public class ParticleEmitter : Component
  {
    /// <summary>
    ///   <para>Should particles be automatically emitted each frame?</para>
    /// </summary>
    public bool emit { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The minimum size each particle can be at the time when it is spawned.</para>
    /// </summary>
    public float minSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum size each particle can be at the time when it is spawned.</para>
    /// </summary>
    public float maxSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The minimum lifetime of each particle, measured in seconds.</para>
    /// </summary>
    public float minEnergy { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum lifetime of each particle, measured in seconds.</para>
    /// </summary>
    public float maxEnergy { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The minimum number of particles that will be spawned every second.</para>
    /// </summary>
    public float minEmission { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum number of particles that will be spawned every second.</para>
    /// </summary>
    public float maxEmission { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The amount of the emitter's speed that the particles inherit.</para>
    /// </summary>
    public float emitterVelocityScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The starting speed of particles in world space, along X, Y, and Z.</para>
    /// </summary>
    public Vector3 worldVelocity
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_worldVelocity(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_worldVelocity(ref value);
      }
    }

    /// <summary>
    ///   <para>The starting speed of particles along X, Y, and Z, measured in the object's orientation.</para>
    /// </summary>
    public Vector3 localVelocity
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_localVelocity(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_localVelocity(ref value);
      }
    }

    /// <summary>
    ///   <para>A random speed along X, Y, and Z that is added to the velocity.</para>
    /// </summary>
    public Vector3 rndVelocity
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_rndVelocity(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_rndVelocity(ref value);
      }
    }

    /// <summary>
    ///   <para>If enabled, the particles don't move when the emitter moves. If false, when you move the emitter, the particles follow it around.</para>
    /// </summary>
    public bool useWorldSpace { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If enabled, the particles will be spawned with random rotations.</para>
    /// </summary>
    public bool rndRotation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The angular velocity of new particles in degrees per second.</para>
    /// </summary>
    public float angularVelocity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>A random angular velocity modifier for new particles.</para>
    /// </summary>
    public float rndAngularVelocity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns a copy of all particles and assigns an array of all particles to be the current particles.</para>
    /// </summary>
    public Particle[] particles { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The current number of particles (Read Only).</para>
    /// </summary>
    public int particleCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Turns the ParticleEmitter on or off.</para>
    /// </summary>
    public bool enabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal ParticleEmitter()
    {
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_worldVelocity(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_worldVelocity(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localVelocity(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_localVelocity(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_rndVelocity(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_rndVelocity(ref Vector3 value);

    /// <summary>
    ///   <para>Removes all particles from the particle emitter.</para>
    /// </summary>
    public void ClearParticles()
    {
      ParticleEmitter.INTERNAL_CALL_ClearParticles(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_ClearParticles(ParticleEmitter self);

    /// <summary>
    ///   <para>Emit a number of particles.</para>
    /// </summary>
    public void Emit()
    {
      this.Emit2((int) Random.Range(this.minEmission, this.maxEmission));
    }

    /// <summary>
    ///   <para>Emit count particles immediately.</para>
    /// </summary>
    /// <param name="count"></param>
    public void Emit(int count)
    {
      this.Emit2(count);
    }

    /// <summary>
    ///   <para>Emit a single particle with given parameters.</para>
    /// </summary>
    /// <param name="pos">The position of the particle.</param>
    /// <param name="velocity">The velocity of the particle.</param>
    /// <param name="size">The size of the particle.</param>
    /// <param name="energy">The remaining lifetime of the particle.</param>
    /// <param name="color">The color of the particle.</param>
    public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color)
    {
      this.Emit3(ref new InternalEmitParticleArguments()
      {
        pos = pos,
        velocity = velocity,
        size = size,
        energy = energy,
        color = color,
        rotation = 0.0f,
        angularVelocity = 0.0f
      });
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="rotation">The initial rotation of the particle in degrees.</param>
    /// <param name="angularVelocity">The angular velocity of the particle in degrees per second.</param>
    /// <param name="pos"></param>
    /// <param name="velocity"></param>
    /// <param name="size"></param>
    /// <param name="energy"></param>
    /// <param name="color"></param>
    public void Emit(Vector3 pos, Vector3 velocity, float size, float energy, Color color, float rotation, float angularVelocity)
    {
      this.Emit3(ref new InternalEmitParticleArguments()
      {
        pos = pos,
        velocity = velocity,
        size = size,
        energy = energy,
        color = color,
        rotation = rotation,
        angularVelocity = angularVelocity
      });
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Emit2(int count);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Emit3(ref InternalEmitParticleArguments args);

    /// <summary>
    ///   <para>Advance particle simulation by given time.</para>
    /// </summary>
    /// <param name="deltaTime"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Simulate(float deltaTime);
  }
}
