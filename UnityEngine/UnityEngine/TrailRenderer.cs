// Decompiled with JetBrains decompiler
// Type: UnityEngine.TrailRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The trail renderer is used to make trails behind objects in the scene as they move about.</para>
  /// </summary>
  public sealed class TrailRenderer : Renderer
  {
    /// <summary>
    ///   <para>How long does the trail take to fade out.</para>
    /// </summary>
    public float time { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The width of the trail at the spawning point.</para>
    /// </summary>
    public float startWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The width of the trail at the end of the trail.</para>
    /// </summary>
    public float endWidth { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does the GameObject of this trail renderer auto destructs?</para>
    /// </summary>
    public bool autodestruct { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>Removes all points from the TrailRenderer.
    /// Useful for restarting a trail from a new position.</para>
    ///       </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Clear();
  }
}
