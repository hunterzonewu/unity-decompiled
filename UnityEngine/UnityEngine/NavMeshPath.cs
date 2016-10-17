// Decompiled with JetBrains decompiler
// Type: UnityEngine.NavMeshPath
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A path as calculated by the navigation system.</para>
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public sealed class NavMeshPath
  {
    internal IntPtr m_Ptr;
    internal Vector3[] m_corners;

    /// <summary>
    ///   <para>Corner points of the path. (Read Only)</para>
    /// </summary>
    public Vector3[] corners
    {
      get
      {
        this.CalculateCorners();
        return this.m_corners;
      }
    }

    /// <summary>
    ///   <para>Status of the path. (Read Only)</para>
    /// </summary>
    public NavMeshPathStatus status { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>NavMeshPath constructor.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public NavMeshPath();

    ~NavMeshPath()
    {
      this.DestroyNavMeshPath();
      this.m_Ptr = IntPtr.Zero;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void DestroyNavMeshPath();

    /// <summary>
    ///   <para>Calculate the corners for the path.</para>
    /// </summary>
    /// <param name="results">Array to store path corners.</param>
    /// <returns>
    ///   <para>The number of corners along the path - including start and end points.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public int GetCornersNonAlloc(Vector3[] results);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private Vector3[] CalculateCornersInternal();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void ClearCornersInternal();

    /// <summary>
    ///   <para>Erase all corner points from path.</para>
    /// </summary>
    public void ClearCorners()
    {
      this.ClearCornersInternal();
      this.m_corners = (Vector3[]) null;
    }

    private void CalculateCorners()
    {
      if (this.m_corners != null)
        return;
      this.m_corners = this.CalculateCornersInternal();
    }
  }
}
