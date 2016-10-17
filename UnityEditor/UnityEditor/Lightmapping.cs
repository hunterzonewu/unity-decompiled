// Decompiled with JetBrains decompiler
// Type: UnityEditor.Lightmapping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Allows to control the lightmapping job.</para>
  /// </summary>
  public sealed class Lightmapping
  {
    /// <summary>
    ///   <para>Delegate which is called when bake job is completed.</para>
    /// </summary>
    public static Lightmapping.OnCompletedFunction completed;

    /// <summary>
    ///   <para>The lightmap baking workflow mode used. Iterative mode is default, but you can switch to on demand mode which bakes only when the user presses the bake button.</para>
    /// </summary>
    public static extern Lightmapping.GIWorkflowMode giWorkflowMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool realtimeLightmapsEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool bakedLightmapsEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Scale for indirect lighting.</para>
    /// </summary>
    public static extern float indirectOutputScale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Boost the albedo.</para>
    /// </summary>
    public static extern float bounceBoost { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool openRLEnabled { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern Lightmapping.ConcurrentJobsType concurrentJobsType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern long diskCacheSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern string diskCachePath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern bool enlightenForceWhiteAlbedo { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool enlightenForceUpdates { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern UnityEngine.FilterMode filterMode { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns true when the bake job is running, false otherwise (Read Only).</para>
    /// </summary>
    public static extern bool isRunning { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the current lightmapping build progress or 0 if Lightmapping.isRunning is false.</para>
    /// </summary>
    public static extern float buildProgress { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The lighting data asset used by the active scene.</para>
    /// </summary>
    public static extern LightingDataAsset lightingDataAsset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("lightmapSnapshot has been deprecated. Use lightingDataAsset instead (UnityUpgradable) -> lightingDataAsset", true)]
    public static LightmapSnapshot lightmapSnapshot
    {
      get
      {
        return (LightmapSnapshot) null;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Clears the cache used by lightmaps, reflection probes and default reflection.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearDiskCache();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateCachePath();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void PrintStateToConsole();

    /// <summary>
    ///   <para>Starts an asynchronous bake job.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BakeAsync();

    /// <summary>
    ///   <para>Stars a synchronous bake job.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Bake();

    /// <summary>
    ///   <para>Starts an asynchronous bake job for the selected objects.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BakeSelectedAsync();

    /// <summary>
    ///   <para>Starts a synchronous bake job for the selected objects.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BakeSelected();

    /// <summary>
    ///   <para>Starts an asynchronous bake job, but only bakes light probes.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BakeLightProbesOnlyAsync();

    /// <summary>
    ///   <para>Starts a synchronous bake job, but only bakes light probes.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BakeLightProbesOnly();

    /// <summary>
    ///   <para>Cancels the currently running asynchronous bake job.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Cancel();

    private static void Internal_CallCompletedFunctions()
    {
      if (Lightmapping.completed == null)
        return;
      Lightmapping.completed();
    }

    /// <summary>
    ///   <para>Deletes all lightmap assets and makes all lights behave as if they weren't baked yet.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Clear();

    /// <summary>
    ///   <para>Remove the lighting data asset used by the current scene.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearLightingDataAsset();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Tetrahedralize(Vector3[] positions, out int[] outIndices, out Vector3[] outPositions);

    /// <summary>
    ///   <para>Starts a synchronous bake job for the probe.</para>
    /// </summary>
    /// <param name="probe">Target probe.</param>
    /// <param name="path">The location where cubemap will be saved.</param>
    /// <returns>
    ///   <para>Returns true if baking was succesful.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BakeReflectionProbe(ReflectionProbe probe, string path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool BakeReflectionProbeSnapshot(ReflectionProbe probe);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool BakeAllReflectionProbesSnapshots();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetTerrainGIChunks(Terrain terrain, ref int numChunksX, ref int numChunksY);

    /// <summary>
    ///   <para>Bakes an array of scenes.</para>
    /// </summary>
    /// <param name="paths">The path of the scenes that should be baked.</param>
    public static void BakeMultipleScenes(string[] paths)
    {
      if (paths.Length == 0)
        return;
      for (int index1 = 0; index1 < paths.Length; ++index1)
      {
        for (int index2 = index1 + 1; index2 < paths.Length; ++index2)
        {
          if (paths[index1] == paths[index2])
            throw new Exception("no duplication of scenes is allowed");
        }
      }
      if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        return;
      SceneSetup[] sceneManagerSetup = EditorSceneManager.GetSceneManagerSetup();
      EditorSceneManager.OpenScene(paths[0]);
      for (int index = 1; index < paths.Length; ++index)
        EditorSceneManager.OpenScene(paths[index], OpenSceneMode.Additive);
      Lightmapping.Bake();
      EditorSceneManager.SaveOpenScenes();
      EditorSceneManager.RestoreSceneManagerSetup(sceneManagerSetup);
    }

    internal enum ConcurrentJobsType
    {
      Min,
      Low,
      High,
    }

    /// <summary>
    ///   <para>Workflow mode for lightmap baking. Default is Iterative.</para>
    /// </summary>
    public enum GIWorkflowMode
    {
      Iterative,
      OnDemand,
      Legacy,
    }

    /// <summary>
    ///   <para>Delegate used by Lightmapping.completed callback.</para>
    /// </summary>
    public delegate void OnCompletedFunction();
  }
}
