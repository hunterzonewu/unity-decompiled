// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleSystemRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Renders particles on to the screen (Shuriken).</para>
  /// </summary>
  public sealed class ParticleSystemRenderer : Renderer
  {
    /// <summary>
    ///   <para>How particles are drawn.</para>
    /// </summary>
    public ParticleSystemRenderMode renderMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles stretched in their direction of motion.</para>
    /// </summary>
    public float lengthScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles stretched depending on "how fast they move".</para>
    /// </summary>
    public float velocityScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles stretched depending on the Camera's speed.</para>
    /// </summary>
    public float cameraVelocityScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are billboard particle normals oriented towards the camera.</para>
    /// </summary>
    public float normalDirection { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Control the direction that particles face.</para>
    /// </summary>
    public ParticleSystemRenderSpace alignment { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Modify the pivot point used for rotating particles.</para>
    /// </summary>
    public Vector3 pivot
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_pivot(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_pivot(ref value);
      }
    }

    /// <summary>
    ///   <para>Sort particles within a system.</para>
    /// </summary>
    public ParticleSystemSortMode sortMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Biases particle system sorting amongst other transparencies.</para>
    /// </summary>
    public float sortingFudge { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Clamp the minimum particle size.</para>
    /// </summary>
    public float minParticleSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Clamp the maximum particle size.</para>
    /// </summary>
    public float maxParticleSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mesh used as particle instead of billboarded texture.</para>
    /// </summary>
    public Mesh mesh { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal bool editorEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_pivot(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_pivot(ref Vector3 value);
  }
}
