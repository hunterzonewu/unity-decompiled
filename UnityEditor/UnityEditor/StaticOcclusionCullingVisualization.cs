// Decompiled with JetBrains decompiler
// Type: UnityEditor.StaticOcclusionCullingVisualization
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Used to visualize static occlusion culling at development time in scene view.</para>
  /// </summary>
  public sealed class StaticOcclusionCullingVisualization
  {
    /// <summary>
    ///   <para>If set to true, visualization of target volumes is enabled.</para>
    /// </summary>
    public static extern bool showOcclusionCulling { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, the visualization lines of the PVS volumes will show all cells rather than cells after culling.</para>
    /// </summary>
    public static extern bool showPreVisualization { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, visualization of view volumes is enabled.</para>
    /// </summary>
    public static extern bool showViewVolumes { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool showDynamicObjectBounds { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, visualization of portals is enabled.</para>
    /// </summary>
    public static extern bool showPortals { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, visualization of portals is enabled.</para>
    /// </summary>
    public static extern bool showVisibilityLines { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, culling of geometry is enabled.</para>
    /// </summary>
    public static extern bool showGeometryCulling { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool isPreviewOcclusionCullingCameraInPVS { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern Camera previewOcclusionCamera { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern Camera previewOcclucionCamera { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
