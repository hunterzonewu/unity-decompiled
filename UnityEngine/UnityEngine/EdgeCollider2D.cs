// Decompiled with JetBrains decompiler
// Type: UnityEngine.EdgeCollider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Collider for 2D physics representing an arbitrary set of connected edges (lines) defined by its vertices.</para>
  /// </summary>
  public sealed class EdgeCollider2D : Collider2D
  {
    /// <summary>
    ///   <para>Gets the number of edges.</para>
    /// </summary>
    public int edgeCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Gets the number of points.</para>
    /// </summary>
    public int pointCount { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get or set the points defining multiple continuous edges.</para>
    /// </summary>
    public Vector2[] points { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Reset to a single edge consisting of two points.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Reset();
  }
}
