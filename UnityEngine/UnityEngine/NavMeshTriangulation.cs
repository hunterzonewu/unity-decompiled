// Decompiled with JetBrains decompiler
// Type: UnityEngine.NavMeshTriangulation
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Contains data describing a triangulation of a navmesh.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct NavMeshTriangulation
  {
    /// <summary>
    ///   <para>Vertices for the navmesh triangulation.</para>
    /// </summary>
    public Vector3[] vertices;
    /// <summary>
    ///   <para>Triangle indices for the navmesh triangulation.</para>
    /// </summary>
    public int[] indices;
    /// <summary>
    ///   <para>NavMesh area indices for the navmesh triangulation.</para>
    /// </summary>
    public int[] areas;

    /// <summary>
    ///   <para>NavMeshLayer values for the navmesh triangulation.</para>
    /// </summary>
    [Obsolete("Use areas instead.")]
    public int[] layers
    {
      get
      {
        return this.areas;
      }
    }
  }
}
