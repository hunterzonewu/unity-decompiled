// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightmapParameters
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>A collection of parameters that impact lightmap and realtime GI computations.</para>
  /// </summary>
  public sealed class LightmapParameters : Object
  {
    /// <summary>
    ///   <para>The texel resolution per meter used for realtime lightmaps. This value is multiplied by LightmapEditorSettings.resolution.</para>
    /// </summary>
    public float resolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls the resolution at which Enlighten stores and can transfer input light.</para>
    /// </summary>
    public float clusterResolution { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The amount of data used for realtime GI texels. Specifies how detailed view of the scene a texel has. Small values mean more averaged out lighting.</para>
    /// </summary>
    public int irradianceBudget { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of rays to cast for computing irradiance form factors.</para>
    /// </summary>
    public int irradianceQuality { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The percentage of rays shot from a ray origin that must hit front faces to be considered usable.</para>
    /// </summary>
    public float backFaceTolerance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Maximum size of gaps that can be ignored for GI (multiplier on pixel size).</para>
    /// </summary>
    public float modellingTolerance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Whether pairs of edges should be stitched together.</para>
    /// </summary>
    public float edgeStitching { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>System tag is an integer identifier. It lets you force an object into a different Enlighten system even though all the other parameters are the same.</para>
    /// </summary>
    public int systemTag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If enabled, the object appears transparent during GlobalIllumination lighting calculations.</para>
    /// </summary>
    public bool isTransparent { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of rays to cast for computing ambient occlusion.</para>
    /// </summary>
    public int AOQuality { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum number of times to supersample a texel to reduce aliasing in AO.</para>
    /// </summary>
    public int AOAntiAliasingSamples { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The radius (in texels) of the post-processing filter that blurs baked direct lighting.</para>
    /// </summary>
    public int blurRadius { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of rays used for lights with an area. Allows for accurate soft shadowing.</para>
    /// </summary>
    public int directLightQuality { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum number of times to supersample a texel to reduce aliasing.</para>
    /// </summary>
    public int antiAliasingSamples { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>BakedLightmapTag is an integer that affects the assignment to baked lightmaps. Objects with different values for bakedLightmapTag are guaranteed to not be assigned to the same lightmap even if the other baking parameters are the same.</para>
    /// </summary>
    public int bakedLightmapTag { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public LightmapParameters()
    {
      LightmapParameters.Internal_CreateLightmapParameters(this);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_CreateLightmapParameters([Writable] LightmapParameters self);
  }
}
