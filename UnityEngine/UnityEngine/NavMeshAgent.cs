// Decompiled with JetBrains decompiler
// Type: UnityEngine.NavMeshAgent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8FF7A2C-E4EE-4232-AB17-3FCABEC16496
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Navigation mesh agent.</para>
  /// </summary>
  public sealed class NavMeshAgent : Behaviour
  {
    /// <summary>
    ///   <para>Gets or attempts to set the destination of the agent in world-space units.</para>
    /// </summary>
    public Vector3 destination
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_destination(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_destination(ref value);
      }
    }

    /// <summary>
    ///   <para>Stop within this distance from the target position.</para>
    /// </summary>
    public float stoppingDistance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Access the current velocity of the NavMeshAgent component, or set a velocity to control the agent manually.</para>
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
    ///   <para>Gets or sets the simulation position of the navmesh agent.</para>
    /// </summary>
    public Vector3 nextPosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_nextPosition(out vector3);
        return vector3;
      }
      set
      {
        this.INTERNAL_set_nextPosition(ref value);
      }
    }

    /// <summary>
    ///   <para>Get the current steering target along the path. (Read Only)</para>
    /// </summary>
    public Vector3 steeringTarget
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_steeringTarget(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>The desired velocity of the agent including any potential contribution from avoidance. (Read Only)</para>
    /// </summary>
    public Vector3 desiredVelocity
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_desiredVelocity(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>The distance between the agent's position and the destination on the current path. (Read Only)</para>
    /// </summary>
    public float remainingDistance { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The relative vertical displacement of the owning GameObject.</para>
    /// </summary>
    public float baseOffset { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the agent currently positioned on an OffMeshLink? (Read Only)</para>
    /// </summary>
    public bool isOnOffMeshLink { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The current OffMeshLinkData.</para>
    /// </summary>
    public OffMeshLinkData currentOffMeshLinkData
    {
      get
      {
        return this.GetCurrentOffMeshLinkDataInternal();
      }
    }

    /// <summary>
    ///   <para>The next OffMeshLinkData on the current path.</para>
    /// </summary>
    public OffMeshLinkData nextOffMeshLinkData
    {
      get
      {
        return this.GetNextOffMeshLinkDataInternal();
      }
    }

    /// <summary>
    ///   <para>Should the agent move across OffMeshLinks automatically?</para>
    /// </summary>
    public bool autoTraverseOffMeshLink { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the agent brake automatically to avoid overshooting the destination point?</para>
    /// </summary>
    public bool autoBraking { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the agent attempt to acquire a new path if the existing path becomes invalid?</para>
    /// </summary>
    public bool autoRepath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does the agent currently have a path? (Read Only)</para>
    /// </summary>
    public bool hasPath { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is a path in the process of being computed but not yet ready? (Read Only)</para>
    /// </summary>
    public bool pathPending { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Is the current path stale. (Read Only)</para>
    /// </summary>
    public bool isPathStale { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The status of the current path (complete, partial or invalid).</para>
    /// </summary>
    public NavMeshPathStatus pathStatus { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Vector3 pathEndPosition
    {
      get
      {
        Vector3 vector3;
        this.INTERNAL_get_pathEndPosition(out vector3);
        return vector3;
      }
    }

    /// <summary>
    ///   <para>Property to get and set the current path.</para>
    /// </summary>
    public NavMeshPath path
    {
      get
      {
        NavMeshPath path = new NavMeshPath();
        this.CopyPathTo(path);
        return path;
      }
      set
      {
        if (value == null)
          throw new NullReferenceException();
        this.SetPath(value);
      }
    }

    /// <summary>
    ///   <para>Specifies which NavMesh layers are passable (bitfield). Changing walkableMask will make the path stale (see isPathStale).</para>
    /// </summary>
    [Obsolete("Use areaMask instead.")]
    public int walkableMask { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Specifies which NavMesh areas are passable. Changing areaMask will make the path stale (see isPathStale).</para>
    /// </summary>
    public int areaMask { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Maximum movement speed when following a path.</para>
    /// </summary>
    public float speed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Maximum turning speed in (deg/s) while following a path.</para>
    /// </summary>
    public float angularSpeed { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum acceleration of an agent as it follows a path, given in units / sec^2.</para>
    /// </summary>
    public float acceleration { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Gets or sets whether the transform position is synchronized with the simulated agent position. The default value is true.</para>
    /// </summary>
    public bool updatePosition { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should the agent update the transform orientation?</para>
    /// </summary>
    public bool updateRotation { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The avoidance radius for the agent.</para>
    /// </summary>
    public float radius { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The height of the agent for purposes of passing under obstacles, etc.</para>
    /// </summary>
    public float height { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The level of quality of avoidance.</para>
    /// </summary>
    public ObstacleAvoidanceType obstacleAvoidanceType { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The avoidance priority level.</para>
    /// </summary>
    public int avoidancePriority { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is the agent currently bound to the navmesh? (Read Only)</para>
    /// </summary>
    public bool isOnNavMesh { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Sets or updates the destination thus triggering the calculation for a new path.</para>
    /// </summary>
    /// <param name="target">The target point to navigate to.</param>
    /// <returns>
    ///   <para>True if the destination was requested successfully, otherwise false.</para>
    /// </returns>
    public bool SetDestination(Vector3 target)
    {
      return NavMeshAgent.INTERNAL_CALL_SetDestination(this, ref target);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_SetDestination(NavMeshAgent self, ref Vector3 target);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_destination(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_destination(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_velocity(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_velocity(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_nextPosition(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_nextPosition(ref Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_steeringTarget(out Vector3 value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_desiredVelocity(out Vector3 value);

    /// <summary>
    ///   <para>Enables or disables the current off-mesh link.</para>
    /// </summary>
    /// <param name="activated">Is the link activated?</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ActivateCurrentOffMeshLink(bool activated);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal OffMeshLinkData GetCurrentOffMeshLinkDataInternal();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal OffMeshLinkData GetNextOffMeshLinkDataInternal();

    /// <summary>
    ///   <para>Completes the movement on the current OffMeshLink.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void CompleteOffMeshLink();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_pathEndPosition(out Vector3 value);

    /// <summary>
    ///   <para>Warps agent to the provided position.</para>
    /// </summary>
    /// <param name="newPosition">New position to warp the agent to.</param>
    /// <returns>
    ///   <para>True if agent is successfully warped, otherwise false.</para>
    /// </returns>
    public bool Warp(Vector3 newPosition)
    {
      return NavMeshAgent.INTERNAL_CALL_Warp(this, ref newPosition);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Warp(NavMeshAgent self, ref Vector3 newPosition);

    /// <summary>
    ///   <para>Apply relative movement to current position.</para>
    /// </summary>
    /// <param name="offset">The relative movement vector.</param>
    public void Move(Vector3 offset)
    {
      NavMeshAgent.INTERNAL_CALL_Move(this, ref offset);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Move(NavMeshAgent self, ref Vector3 offset);

    /// <summary>
    ///   <para>Stop movement of this agent along its current path.</para>
    /// </summary>
    public void Stop()
    {
      this.StopInternal();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void StopInternal();

    [Obsolete("Use Stop() instead")]
    public void Stop(bool stopUpdates)
    {
      this.StopInternal();
    }

    /// <summary>
    ///   <para>Resumes the movement along the current path after a pause.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Resume();

    /// <summary>
    ///   <para>Clears the current path.</para>
    /// </summary>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ResetPath();

    /// <summary>
    ///   <para>Assign a new path to this agent.</para>
    /// </summary>
    /// <param name="path">New path to follow.</param>
    /// <returns>
    ///   <para>True if the path is succesfully assigned.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool SetPath(NavMeshPath path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void CopyPathTo(NavMeshPath path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool FindClosestEdge(out NavMeshHit hit);

    public bool Raycast(Vector3 targetPosition, out NavMeshHit hit)
    {
      return NavMeshAgent.INTERNAL_CALL_Raycast(this, ref targetPosition, out hit);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Raycast(NavMeshAgent self, ref Vector3 targetPosition, out NavMeshHit hit);

    /// <summary>
    ///   <para>Calculate a path to a specified point and store the resulting path.</para>
    /// </summary>
    /// <param name="targetPosition">The final position of the path requested.</param>
    /// <param name="path">The resulting path.</param>
    /// <returns>
    ///   <para>True if a path is found.</para>
    /// </returns>
    public bool CalculatePath(Vector3 targetPosition, NavMeshPath path)
    {
      path.ClearCorners();
      return this.CalculatePathInternal(targetPosition, path);
    }

    private bool CalculatePathInternal(Vector3 targetPosition, NavMeshPath path)
    {
      return NavMeshAgent.INTERNAL_CALL_CalculatePathInternal(this, ref targetPosition, path);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_CalculatePathInternal(NavMeshAgent self, ref Vector3 targetPosition, NavMeshPath path);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool SamplePathPosition(int areaMask, float maxDistance, out NavMeshHit hit);

    /// <summary>
    ///   <para>Sets the cost for traversing over geometry of the layer type.</para>
    /// </summary>
    /// <param name="layer">Layer index.</param>
    /// <param name="cost">New cost for the specified layer.</param>
    [WrapperlessIcall]
    [Obsolete("Use SetAreaCost instead.")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetLayerCost(int layer, float cost);

    /// <summary>
    ///   <para>Gets the cost for crossing ground of a particular type.</para>
    /// </summary>
    /// <param name="layer">Layer index.</param>
    /// <returns>
    ///   <para>Current cost of specified layer.</para>
    /// </returns>
    [Obsolete("Use GetAreaCost instead.")]
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetLayerCost(int layer);

    /// <summary>
    ///   <para>Sets the cost for traversing over areas of the area type.</para>
    /// </summary>
    /// <param name="areaIndex">Area cost.</param>
    /// <param name="areaCost">New cost for the specified area index.</param>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetAreaCost(int areaIndex, float areaCost);

    /// <summary>
    ///   <para>Gets the cost for path calculation when crossing area of a particular type.</para>
    /// </summary>
    /// <param name="areaIndex">Area Index.</param>
    /// <returns>
    ///   <para>Current cost for specified area index.</para>
    /// </returns>
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public float GetAreaCost(int areaIndex);
  }
}
