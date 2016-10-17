// Decompiled with JetBrains decompiler
// Type: UnityEngine.Renderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
  /// <summary>
  ///   <para>General functionality for all renderers.</para>
  /// </summary>
  public class Renderer : Component
  {
    internal Transform staticBatchRootTransform { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal int staticBatchIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Has this renderer been statically batched with any other renderers?</para>
    /// </summary>
    public bool isPartOfStaticBatch { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Matrix that transforms a point from world space into local space (Read Only).</para>
    /// </summary>
    public Matrix4x4 worldToLocalMatrix
    {
      get
      {
        Matrix4x4 matrix4x4;
        this.INTERNAL_get_worldToLocalMatrix(out matrix4x4);
        return matrix4x4;
      }
    }

    /// <summary>
    ///   <para>Matrix that transforms a point from local space into world space (Read Only).</para>
    /// </summary>
    public Matrix4x4 localToWorldMatrix
    {
      get
      {
        Matrix4x4 matrix4x4;
        this.INTERNAL_get_localToWorldMatrix(out matrix4x4);
        return matrix4x4;
      }
    }

    /// <summary>
    ///   <para>Makes the rendered 3D object visible if enabled.</para>
    /// </summary>
    public bool enabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does this object cast shadows?</para>
    /// </summary>
    public ShadowCastingMode shadowCastingMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("Property castShadows has been deprecated. Use shadowCastingMode instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool castShadows { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does this object receive shadows?</para>
    /// </summary>
    public bool receiveShadows { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns the first instantiated Material assigned to the renderer.</para>
    /// </summary>
    public Material material { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The shared material of this object.</para>
    /// </summary>
    public Material sharedMaterial { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns all the instantiated materials of this object.</para>
    /// </summary>
    public Material[] materials { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>All the shared materials of this object.</para>
    /// </summary>
    public Material[] sharedMaterials { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The bounding volume of the renderer (Read Only).</para>
    /// </summary>
    public Bounds bounds
    {
      get
      {
        Bounds bounds;
        this.INTERNAL_get_bounds(out bounds);
        return bounds;
      }
    }

    /// <summary>
    ///   <para>The index of the baked lightmap applied to this renderer.</para>
    /// </summary>
    public int lightmapIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The index of the realtime lightmap applied to this renderer.</para>
    /// </summary>
    public int realtimeLightmapIndex { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The UV scale &amp; offset used for a lightmap.</para>
    /// </summary>
    public Vector4 lightmapScaleOffset
    {
      get
      {
        Vector4 vector4;
        this.INTERNAL_get_lightmapScaleOffset(out vector4);
        return vector4;
      }
      set
      {
        this.INTERNAL_set_lightmapScaleOffset(ref value);
      }
    }

    /// <summary>
    ///   <para>The UV scale &amp; offset used for a realtime lightmap.</para>
    /// </summary>
    public Vector4 realtimeLightmapScaleOffset
    {
      get
      {
        Vector4 vector4;
        this.INTERNAL_get_realtimeLightmapScaleOffset(out vector4);
        return vector4;
      }
      set
      {
        this.INTERNAL_set_realtimeLightmapScaleOffset(ref value);
      }
    }

    /// <summary>
    ///   <para>Is this renderer visible in any camera? (Read Only)</para>
    /// </summary>
    public bool isVisible { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Should light probes be used for this Renderer?</para>
    /// </summary>
    public bool useLightProbes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set, Renderer will use this Transform's position to find the light or reflection probe.</para>
    /// </summary>
    public Transform probeAnchor { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should reflection probes be used for this Renderer?</para>
    /// </summary>
    public ReflectionProbeUsage reflectionProbeUsage { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Name of the Renderer's sorting layer.</para>
    /// </summary>
    public string sortingLayerName { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Unique ID of the Renderer's sorting layer.</para>
    /// </summary>
    public int sortingLayerID { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Renderer's order within a sorting layer.</para>
    /// </summary>
    public int sortingOrder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Property lightmapTilingOffset has been deprecated. Use lightmapScaleOffset (UnityUpgradable) -> lightmapScaleOffset", true)]
    public Vector4 lightmapTilingOffset
    {
      get
      {
        return Vector4.zero;
      }
      set
      {
      }
    }

    [Obsolete("Use probeAnchor instead (UnityUpgradable) -> probeAnchor", true)]
    public Transform lightProbeAnchor
    {
      get
      {
        return this.probeAnchor;
      }
      set
      {
        this.probeAnchor = value;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetSubsetIndex(int index, int subSetIndexForMaterial);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_worldToLocalMatrix(out Matrix4x4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_localToWorldMatrix(out Matrix4x4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_bounds(out Bounds value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_lightmapScaleOffset(out Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_lightmapScaleOffset(ref Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_realtimeLightmapScaleOffset(out Vector4 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_realtimeLightmapScaleOffset(ref Vector4 value);

    /// <summary>
    ///   <para>Lets you add per-renderer material parameters without duplicating a material.</para>
    /// </summary>
    /// <param name="properties"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetPropertyBlock(MaterialPropertyBlock properties);

    /// <summary>
    ///   <para>Get per-renderer material property block.</para>
    /// </summary>
    /// <param name="dest"></param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void GetPropertyBlock(MaterialPropertyBlock dest);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RenderNow(int material);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void GetClosestReflectionProbesInternal(object result);

    public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
    {
      this.GetClosestReflectionProbesInternal((object) result);
    }
  }
}
