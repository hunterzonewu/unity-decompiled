// Decompiled with JetBrains decompiler
// Type: UnityEditor.StaticOcclusionCulling
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>StaticOcclusionCulling lets you perform static occlusion culling operations.</para>
  /// </summary>
  public sealed class StaticOcclusionCulling
  {
    /// <summary>
    ///   <para>Used to check if asynchronous generation of static occlusion culling data is still running.</para>
    /// </summary>
    public static extern bool isRunning { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern float smallestOccluder { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern float smallestHole { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern float backfaceThreshold { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does the scene contain any occlusion portals that were added manually rather than automatically?</para>
    /// </summary>
    public static extern bool doesSceneHaveManualPortals { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the size in bytes that the PVS data is currently taking up in this scene on disk.</para>
    /// </summary>
    public static extern int umbraDataSize { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Used to generate static occlusion culling data.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Compute();

    /// <summary>
    ///   <para>Used to compute static occlusion culling data asynchronously.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GenerateInBackground();

    /// <summary>
    ///   <para>Used to cancel asynchronous generation of static occlusion culling data.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Cancel();

    /// <summary>
    ///   <para>Clears the PVS of the opened scene.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Clear();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetDefaultOcclusionBakeSettings();
  }
}
