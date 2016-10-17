// Decompiled with JetBrains decompiler
// Type: UnityEngine.Light
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Script interface for.</para>
  /// </summary>
  public sealed class Light : Behaviour
  {
    /// <summary>
    ///   <para>The type of the light.</para>
    /// </summary>
    public LightType type { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The color of the light.</para>
    /// </summary>
    public Color color
    {
      get
      {
        Color color;
        this.INTERNAL_get_color(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_color(ref value);
      }
    }

    /// <summary>
    ///   <para>The Intensity of a light is multiplied with the Light color.</para>
    /// </summary>
    public float intensity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The multiplier that defines the strength of the bounce lighting.</para>
    /// </summary>
    public float bounceIntensity { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How this light casts shadows</para>
    /// </summary>
    public LightShadows shadows { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Strength of light's shadows.</para>
    /// </summary>
    public float shadowStrength { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Shadow mapping constant bias.</para>
    /// </summary>
    public float shadowBias { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Shadow mapping normal-based bias.</para>
    /// </summary>
    public float shadowNormalBias { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Near plane value to use for shadow frustums.</para>
    /// </summary>
    public float shadowNearPlane { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("Shadow softness is removed in Unity 5.0+")]
    public float shadowSoftness { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("Shadow softness is removed in Unity 5.0+")]
    public float shadowSoftnessFade { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The range of the light.</para>
    /// </summary>
    public float range { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The angle of the light's spotlight cone in degrees.</para>
    /// </summary>
    public float spotAngle { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The size of a directional light's cookie.</para>
    /// </summary>
    public float cookieSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The cookie texture projected by the light.</para>
    /// </summary>
    public Texture cookie { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The to use for this light.</para>
    /// </summary>
    public Flare flare { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>How to render the light.</para>
    /// </summary>
    public LightRenderMode renderMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Has the light already been lightmapped.</para>
    /// </summary>
    public bool alreadyLightmapped { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>This is used to light certain objects in the scene selectively.</para>
    /// </summary>
    public int cullingMask { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The size of the area light. Editor only.</para>
    /// </summary>
    public Vector2 areaSize
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_areaSize(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_areaSize(ref value);
      }
    }

    /// <summary>
    ///   <para>Number of command buffers set up on this light (Read Only).</para>
    /// </summary>
    public int commandBufferCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("Use QualitySettings.pixelLightCount instead.")]
    public static extern int pixelLightCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("light.shadowConstantBias was removed, use light.shadowBias", true)]
    public float shadowConstantBias
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("light.shadowObjectSizeBias was removed, use light.shadowBias", true)]
    public float shadowObjectSizeBias
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    [Obsolete("light.attenuate was removed; all lights always attenuate now", true)]
    public bool attenuate
    {
      get
      {
        return true;
      }
      set
      {
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_color(out Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_color(ref Color value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_areaSize(out Vector2 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_areaSize(ref Vector2 value);

    /// <summary>
    ///   <para>Add a command buffer to be executed at a specified place.</para>
    /// </summary>
    /// <param name="evt">When to execute the command buffer during rendering.</param>
    /// <param name="buffer">The buffer to execute.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void AddCommandBuffer(LightEvent evt, CommandBuffer buffer);

    /// <summary>
    ///   <para>Remove command buffer from execution at a specified place.</para>
    /// </summary>
    /// <param name="evt">When to execute the command buffer during rendering.</param>
    /// <param name="buffer">The buffer to execute.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RemoveCommandBuffer(LightEvent evt, CommandBuffer buffer);

    /// <summary>
    ///   <para>Remove command buffers from execution at a specified place.</para>
    /// </summary>
    /// <param name="evt">When to execute the command buffer during rendering.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RemoveCommandBuffers(LightEvent evt);

    /// <summary>
    ///   <para>Remove all command buffers set on this light.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RemoveAllCommandBuffers();

    /// <summary>
    ///   <para>Get command buffers to be executed at a specified place.</para>
    /// </summary>
    /// <param name="evt">When to execute the command buffer during rendering.</param>
    /// <returns>
    ///   <para>Array of command buffers.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public CommandBuffer[] GetCommandBuffers(LightEvent evt);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern Light[] GetLights(LightType type, int layer);
  }
}
