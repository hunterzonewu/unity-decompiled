// Decompiled with JetBrains decompiler
// Type: UnityEngine.ParticleRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>(Legacy Particles) Renders particles on to the screen.</para>
  /// </summary>
  public sealed class ParticleRenderer : Renderer
  {
    /// <summary>
    ///   <para>How particles are drawn.</para>
    /// </summary>
    public ParticleRenderMode particleRenderMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles stretched in their direction of motion.</para>
    /// </summary>
    public float lengthScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles strectched depending on "how fast they move".</para>
    /// </summary>
    public float velocityScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How much are the particles strected depending on the Camera's speed.</para>
    /// </summary>
    public float cameraVelocityScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Clamp the maximum particle size.</para>
    /// </summary>
    public float maxParticleSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set horizontal tiling count.</para>
    /// </summary>
    public int uvAnimationXTile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set vertical tiling count.</para>
    /// </summary>
    public int uvAnimationYTile { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set uv animation cycles.</para>
    /// </summary>
    public float uvAnimationCycles { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("animatedTextureCount has been replaced by uvAnimationXTile and uvAnimationYTile.")]
    public int animatedTextureCount
    {
      get
      {
        return this.uvAnimationXTile;
      }
      set
      {
        this.uvAnimationXTile = value;
      }
    }

    public float maxPartileSize
    {
      get
      {
        return this.maxParticleSize;
      }
      set
      {
        this.maxParticleSize = value;
      }
    }

    public Rect[] uvTiles { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("This function has been removed.", true)]
    public AnimationCurve widthCurve
    {
      get
      {
        return (AnimationCurve) null;
      }
      set
      {
      }
    }

    [Obsolete("This function has been removed.", true)]
    public AnimationCurve heightCurve
    {
      get
      {
        return (AnimationCurve) null;
      }
      set
      {
      }
    }

    [Obsolete("This function has been removed.", true)]
    public AnimationCurve rotationCurve
    {
      get
      {
        return (AnimationCurve) null;
      }
      set
      {
      }
    }
  }
}
