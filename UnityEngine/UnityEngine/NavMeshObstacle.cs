// Decompiled with JetBrains decompiler
// Type: UnityEngine.NavMeshObstacle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>An obstacle for NavMeshAgents to avoid.</para>
  /// </summary>
  public sealed class NavMeshObstacle : Behaviour
  {
    /// <summary>
    ///   <para>Height of the obstacle's cylinder shape.</para>
    /// </summary>
    public float height { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Radius of the obstacle's capsule shape.</para>
    /// </summary>
    public float radius { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Velocity at which the obstacle moves around the NavMesh.</para>
    /// </summary>
    public Vector3 velocity
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_velocity(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_velocity(ref value);
      }
    }

    /// <summary>
    ///   <para>Should this obstacle make a cut-out in the navmesh.</para>
    /// </summary>
    public bool carving { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should this obstacle be carved when it is constantly moving?</para>
    /// </summary>
    public bool carveOnlyStationary { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Threshold distance for updating a moving carved hole (when carving is enabled).</para>
    /// </summary>
    public float carvingMoveThreshold { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Time to wait until obstacle is treated as stationary (when carving and carveOnlyStationary are enabled).</para>
    /// </summary>
    public float carvingTimeToStationary { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Shape of the obstacle.</para>
    /// </summary>
    public NavMeshObstacleShape shape { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The center of the obstacle, measured in the object's local space.</para>
    /// </summary>
    public Vector3 center
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_center(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_center(ref value);
      }
    }

    /// <summary>
    ///   <para>The size of the obstacle, measured in the object's local space.</para>
    /// </summary>
    public Vector3 size
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_size(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_size(ref value);
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_velocity(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_velocity(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_center(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_center(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_size(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_size(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void FitExtents();
  }
}
