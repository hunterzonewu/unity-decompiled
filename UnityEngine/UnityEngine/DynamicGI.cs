// Decompiled with JetBrains decompiler
// Type: UnityEngine.DynamicGI
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Allows to control the dynamic Global Illumination.</para>
  /// </summary>
  public sealed class DynamicGI
  {
    /// <summary>
    ///   <para>Allows for scaling the contribution coming from realtime &amp; static  lightmaps.</para>
    /// </summary>
    public static extern float indirectScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Threshold for limiting updates of realtime GI.</para>
    /// </summary>
    public static extern float updateThreshold { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>When enabled, new dynamic Global Illumination output is shown in each frame.</para>
    /// </summary>
    public static extern bool synchronousMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static void SetEmissive(Renderer renderer, Color color)
    {
      DynamicGI.INTERNAL_CALL_SetEmissive(renderer, ref color);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetEmissive(Renderer renderer, ref Color color);

    /// <summary>
    ///   <para>Schedules an update of the albedo and emissive textures of a system that contains the renderer or the terrain.</para>
    /// </summary>
    /// <param name="renderer">The Renderer to use when searching for a system to update.</param>
    /// <param name="terrain">The Terrain to use when searching for systems to update.</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void UpdateMaterials(Renderer renderer)
    {
      DynamicGI.UpdateMaterialsForRenderer(renderer);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateMaterialsForRenderer(Renderer renderer);

    /// <summary>
    ///   <para>Schedules an update of the albedo and emissive textures of a system that contains the renderer or the terrain.</para>
    /// </summary>
    /// <param name="renderer">The Renderer to use when searching for a system to update.</param>
    /// <param name="terrain">The Terrain to use when searching for systems to update.</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void UpdateMaterials(Terrain terrain)
    {
      if ((Object) terrain == (Object) null)
        throw new ArgumentNullException("terrain");
      if ((Object) terrain.terrainData == (Object) null)
        throw new ArgumentException("Invalid terrainData.");
      DynamicGI.UpdateMaterialsForTerrain(terrain, new Rect(0.0f, 0.0f, 1f, 1f));
    }

    /// <summary>
    ///   <para>Schedules an update of the albedo and emissive textures of a system that contains the renderer or the terrain.</para>
    /// </summary>
    /// <param name="renderer">The Renderer to use when searching for a system to update.</param>
    /// <param name="terrain">The Terrain to use when searching for systems to update.</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void UpdateMaterials(Terrain terrain, int x, int y, int width, int height)
    {
      if ((Object) terrain == (Object) null)
        throw new ArgumentNullException("terrain");
      if ((Object) terrain.terrainData == (Object) null)
        throw new ArgumentException("Invalid terrainData.");
      float alphamapWidth = (float) terrain.terrainData.alphamapWidth;
      float alphamapHeight = (float) terrain.terrainData.alphamapHeight;
      DynamicGI.UpdateMaterialsForTerrain(terrain, new Rect((float) x / alphamapWidth, (float) y / alphamapHeight, (float) width / alphamapWidth, (float) height / alphamapHeight));
    }

    internal static void UpdateMaterialsForTerrain(Terrain terrain, Rect uvBounds)
    {
      DynamicGI.INTERNAL_CALL_UpdateMaterialsForTerrain(terrain, ref uvBounds);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_UpdateMaterialsForTerrain(Terrain terrain, ref Rect uvBounds);

    /// <summary>
    ///   <para>Schedules an update of the environment texture.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void UpdateEnvironment();
  }
}
