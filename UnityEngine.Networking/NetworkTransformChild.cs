// Decompiled with JetBrains decompiler
// Type: UnityEngine.Networking.NetworkTransformChild
// Assembly: UnityEngine.Networking, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B34E19C-EF53-416E-AE36-35C45BAFD2DE
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.Networking.dll

using System;
using UnityEditor;

namespace UnityEngine.Networking
{
  /// <summary>
  ///   <para>A component to synchronize the position of child transforms of networked objects.</para>
  /// </summary>
  [AddComponentMenu("Network/NetworkTransformChild")]
  public class NetworkTransformChild : NetworkBehaviour
  {
    [SerializeField]
    private float m_SendInterval = 0.1f;
    [SerializeField]
    private NetworkTransform.AxisSyncMode m_SyncRotationAxis = NetworkTransform.AxisSyncMode.AxisXYZ;
    [SerializeField]
    private float m_MovementThreshold = 1f / 1000f;
    [SerializeField]
    private float m_InterpolateRotation = 0.5f;
    [SerializeField]
    private float m_InterpolateMovement = 0.5f;
    private const float k_LocalMovementThreshold = 1E-05f;
    private const float k_LocalRotationThreshold = 1E-05f;
    [SerializeField]
    private Transform m_Target;
    [SerializeField]
    private uint m_ChildIndex;
    private NetworkTransform m_Root;
    [SerializeField]
    private NetworkTransform.CompressionSyncMode m_RotationSyncCompression;
    [SerializeField]
    private NetworkTransform.ClientMoveCallback3D m_ClientMoveCallback3D;
    private Vector3 m_TargetSyncPosition;
    private Quaternion m_TargetSyncRotation3D;
    private float m_LastClientSyncTime;
    private float m_LastClientSendTime;
    private Vector3 m_PrevPosition;
    private Quaternion m_PrevRotation;
    private NetworkWriter m_LocalTransformWriter;

    /// <summary>
    ///   <para>The child transform to be synchronized.</para>
    /// </summary>
    public Transform target
    {
      get
      {
        return this.m_Target;
      }
      set
      {
        this.m_Target = value;
        this.OnValidate();
      }
    }

    /// <summary>
    ///   <para>A unique Identifier for this NetworkTransformChild component on this root object.</para>
    /// </summary>
    public uint childIndex
    {
      get
      {
        return this.m_ChildIndex;
      }
    }

    /// <summary>
    ///   <para>The sendInterval controls how often state updates are sent for this object.</para>
    /// </summary>
    public float sendInterval
    {
      get
      {
        return this.m_SendInterval;
      }
      set
      {
        this.m_SendInterval = value;
      }
    }

    /// <summary>
    ///   <para>Which axis should rotation by synchronized for.</para>
    /// </summary>
    public NetworkTransform.AxisSyncMode syncRotationAxis
    {
      get
      {
        return this.m_SyncRotationAxis;
      }
      set
      {
        this.m_SyncRotationAxis = value;
      }
    }

    /// <summary>
    ///   <para>How much to compress rotation sync updates.</para>
    /// </summary>
    public NetworkTransform.CompressionSyncMode rotationSyncCompression
    {
      get
      {
        return this.m_RotationSyncCompression;
      }
      set
      {
        this.m_RotationSyncCompression = value;
      }
    }

    /// <summary>
    ///   <para>The distance that an object can move without sending a movement synchronization update.</para>
    /// </summary>
    public float movementThreshold
    {
      get
      {
        return this.m_MovementThreshold;
      }
      set
      {
        this.m_MovementThreshold = value;
      }
    }

    /// <summary>
    ///   <para>The rate to interpolate to the target rotation.</para>
    /// </summary>
    public float interpolateRotation
    {
      get
      {
        return this.m_InterpolateRotation;
      }
      set
      {
        this.m_InterpolateRotation = value;
      }
    }

    /// <summary>
    ///   <para>The rate to interpolate towards the target position.</para>
    /// </summary>
    public float interpolateMovement
    {
      get
      {
        return this.m_InterpolateMovement;
      }
      set
      {
        this.m_InterpolateMovement = value;
      }
    }

    /// <summary>
    ///   <para>A callback function to allow server side validation of the movement of the child object.</para>
    /// </summary>
    public NetworkTransform.ClientMoveCallback3D clientMoveCallback3D
    {
      get
      {
        return this.m_ClientMoveCallback3D;
      }
      set
      {
        this.m_ClientMoveCallback3D = value;
      }
    }

    /// <summary>
    ///   <para>The most recent time when a movement synchronization packet arrived for this object.</para>
    /// </summary>
    public float lastSyncTime
    {
      get
      {
        return this.m_LastClientSyncTime;
      }
    }

    /// <summary>
    ///   <para>The target position interpolating towards.</para>
    /// </summary>
    public Vector3 targetSyncPosition
    {
      get
      {
        return this.m_TargetSyncPosition;
      }
    }

    /// <summary>
    ///   <para>The target rotation interpolating towards.</para>
    /// </summary>
    public Quaternion targetSyncRotation3D
    {
      get
      {
        return this.m_TargetSyncRotation3D;
      }
    }

    private void OnValidate()
    {
      if ((UnityEngine.Object) this.m_Target != (UnityEngine.Object) null)
      {
        Transform parent = this.m_Target.parent;
        if ((UnityEngine.Object) parent == (UnityEngine.Object) null)
        {
          if (LogFilter.logError)
            Debug.LogError((object) "NetworkTransformChild target cannot be the root transform.");
          this.m_Target = (Transform) null;
          return;
        }
        while ((UnityEngine.Object) parent.parent != (UnityEngine.Object) null)
          parent = parent.parent;
        this.m_Root = parent.gameObject.GetComponent<NetworkTransform>();
        if ((UnityEngine.Object) this.m_Root == (UnityEngine.Object) null)
        {
          if (LogFilter.logError)
            Debug.LogError((object) "NetworkTransformChild root must have NetworkTransform");
          this.m_Target = (Transform) null;
          return;
        }
      }
      this.m_ChildIndex = uint.MaxValue;
      NetworkTransformChild[] components = this.m_Root.GetComponents<NetworkTransformChild>();
      for (uint index = 0; (long) index < (long) components.Length; ++index)
      {
        if ((UnityEngine.Object) components[(IntPtr) index] == (UnityEngine.Object) this)
        {
          this.m_ChildIndex = index;
          break;
        }
      }
      if ((int) this.m_ChildIndex == -1)
      {
        if (LogFilter.logError)
          Debug.LogError((object) "NetworkTransformChild component must be a child in the same hierarchy");
        this.m_Target = (Transform) null;
      }
      if ((double) this.m_SendInterval < 0.0)
        this.m_SendInterval = 0.0f;
      if (this.m_SyncRotationAxis < NetworkTransform.AxisSyncMode.None || this.m_SyncRotationAxis > NetworkTransform.AxisSyncMode.AxisXYZ)
        this.m_SyncRotationAxis = NetworkTransform.AxisSyncMode.None;
      if ((double) this.movementThreshold < 0.0)
        this.movementThreshold = 0.0f;
      if ((double) this.interpolateRotation < 0.0)
        this.interpolateRotation = 0.01f;
      if ((double) this.interpolateRotation > 1.0)
        this.interpolateRotation = 1f;
      if ((double) this.interpolateMovement < 0.0)
        this.interpolateMovement = 0.01f;
      if ((double) this.interpolateMovement <= 1.0)
        return;
      this.interpolateMovement = 1f;
    }

    private void Awake()
    {
      this.m_PrevPosition = this.m_Target.localPosition;
      this.m_PrevRotation = this.m_Target.localRotation;
      if (!this.localPlayerAuthority)
        return;
      this.m_LocalTransformWriter = new NetworkWriter();
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
      if (!initialState)
      {
        if ((int) this.syncVarDirtyBits == 0)
        {
          writer.WritePackedUInt32(0U);
          return false;
        }
        writer.WritePackedUInt32(1U);
      }
      this.SerializeModeTransform(writer);
      return true;
    }

    private void SerializeModeTransform(NetworkWriter writer)
    {
      writer.Write(this.m_Target.localPosition);
      if (this.m_SyncRotationAxis != NetworkTransform.AxisSyncMode.None)
        NetworkTransform.SerializeRotation3D(writer, this.m_Target.localRotation, this.syncRotationAxis, this.rotationSyncCompression);
      this.m_PrevPosition = this.m_Target.localPosition;
      this.m_PrevRotation = this.m_Target.localRotation;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
      if (this.isServer && NetworkServer.localClientActive || !initialState && (int) reader.ReadPackedUInt32() == 0)
        return;
      this.UnserializeModeTransform(reader, initialState);
      this.m_LastClientSyncTime = Time.time;
    }

    private void UnserializeModeTransform(NetworkReader reader, bool initialState)
    {
      if (this.hasAuthority)
      {
        reader.ReadVector3();
        if (this.syncRotationAxis == NetworkTransform.AxisSyncMode.None)
          return;
        NetworkTransform.UnserializeRotation3D(reader, this.syncRotationAxis, this.rotationSyncCompression);
      }
      else if (this.isServer && this.m_ClientMoveCallback3D != null)
      {
        Vector3 position = reader.ReadVector3();
        Vector3 zero = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        if (this.syncRotationAxis != NetworkTransform.AxisSyncMode.None)
          rotation = NetworkTransform.UnserializeRotation3D(reader, this.syncRotationAxis, this.rotationSyncCompression);
        if (!this.m_ClientMoveCallback3D(ref position, ref zero, ref rotation))
          return;
        this.m_TargetSyncPosition = position;
        if (this.syncRotationAxis == NetworkTransform.AxisSyncMode.None)
          return;
        this.m_TargetSyncRotation3D = rotation;
      }
      else
      {
        this.m_TargetSyncPosition = reader.ReadVector3();
        if (this.syncRotationAxis == NetworkTransform.AxisSyncMode.None)
          return;
        this.m_TargetSyncRotation3D = NetworkTransform.UnserializeRotation3D(reader, this.syncRotationAxis, this.rotationSyncCompression);
      }
    }

    private void FixedUpdate()
    {
      if (this.isServer)
        this.FixedUpdateServer();
      if (!this.isClient)
        return;
      this.FixedUpdateClient();
    }

    private void FixedUpdateServer()
    {
      if ((int) this.syncVarDirtyBits != 0 || !NetworkServer.active || (!this.isServer || (double) this.GetNetworkSendInterval() == 0.0) || (double) (this.m_Target.localPosition - this.m_PrevPosition).sqrMagnitude < (double) this.movementThreshold && (double) Quaternion.Angle(this.m_PrevRotation, this.m_Target.localRotation) < (double) this.movementThreshold)
        return;
      this.SetDirtyBit(1U);
    }

    private void FixedUpdateClient()
    {
      if ((double) this.m_LastClientSyncTime == 0.0 || !NetworkServer.active && !NetworkClient.active || (!this.isServer && !this.isClient || ((double) this.GetNetworkSendInterval() == 0.0 || this.hasAuthority)) || (double) this.m_LastClientSyncTime == 0.0)
        return;
      this.m_Target.localPosition = Vector3.Lerp(this.m_Target.localPosition, this.m_TargetSyncPosition, this.m_InterpolateMovement);
      this.m_Target.localRotation = Quaternion.Slerp(this.m_Target.localRotation, this.m_TargetSyncRotation3D, this.m_InterpolateRotation);
    }

    private void Update()
    {
      if (!this.hasAuthority || !this.localPlayerAuthority || (NetworkServer.active || (double) Time.time - (double) this.m_LastClientSendTime <= (double) this.GetNetworkSendInterval()))
        return;
      this.SendTransform();
      this.m_LastClientSendTime = Time.time;
    }

    private bool HasMoved()
    {
      return (double) (this.m_Target.localPosition - this.m_PrevPosition).sqrMagnitude > 9.99999974737875E-06 || (double) Quaternion.Angle(this.m_Target.localRotation, this.m_PrevRotation) > 9.99999974737875E-06;
    }

    [Client]
    private void SendTransform()
    {
      if (!this.HasMoved() || ClientScene.readyConnection == null)
        return;
      this.m_LocalTransformWriter.StartMessage((short) 16);
      this.m_LocalTransformWriter.Write(this.netId);
      this.m_LocalTransformWriter.WritePackedUInt32(this.m_ChildIndex);
      this.SerializeModeTransform(this.m_LocalTransformWriter);
      this.m_PrevPosition = this.m_Target.localPosition;
      this.m_PrevRotation = this.m_Target.localRotation;
      this.m_LocalTransformWriter.FinishMessage();
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Outgoing, (short) 16, "16:LocalChildTransform", 1);
      ClientScene.readyConnection.SendWriter(this.m_LocalTransformWriter, this.GetNetworkChannel());
    }

    internal static void HandleChildTransform(NetworkMessage netMsg)
    {
      NetworkInstanceId netId = netMsg.reader.ReadNetworkId();
      uint num = netMsg.reader.ReadPackedUInt32();
      NetworkDetailStats.IncrementStat(NetworkDetailStats.NetworkDirection.Incoming, (short) 16, "16:LocalChildTransform", 1);
      GameObject localObject = NetworkServer.FindLocalObject(netId);
      if ((UnityEngine.Object) localObject == (UnityEngine.Object) null)
      {
        if (!LogFilter.logError)
          return;
        Debug.LogError((object) "HandleChildTransform no gameObject");
      }
      else
      {
        NetworkTransformChild[] components = localObject.GetComponents<NetworkTransformChild>();
        if (components == null || components.Length == 0)
        {
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) "HandleChildTransform no children");
        }
        else if ((long) num >= (long) components.Length)
        {
          if (!LogFilter.logError)
            return;
          Debug.LogError((object) "HandleChildTransform childIndex invalid");
        }
        else
        {
          NetworkTransformChild networkTransformChild = components[(IntPtr) num];
          if ((UnityEngine.Object) networkTransformChild == (UnityEngine.Object) null)
          {
            if (!LogFilter.logError)
              return;
            Debug.LogError((object) "HandleChildTransform null target");
          }
          else if (!networkTransformChild.localPlayerAuthority)
          {
            if (!LogFilter.logError)
              return;
            Debug.LogError((object) "HandleChildTransform no localPlayerAuthority");
          }
          else if (!netMsg.conn.clientOwnedObjects.Contains(netId))
          {
            if (!LogFilter.logWarn)
              return;
            Debug.LogWarning((object) ("NetworkTransformChild netId:" + (object) netId + " is not for a valid player"));
          }
          else
          {
            networkTransformChild.UnserializeModeTransform(netMsg.reader, false);
            networkTransformChild.m_LastClientSyncTime = Time.time;
            if (networkTransformChild.isClient)
              return;
            networkTransformChild.m_Target.localPosition = networkTransformChild.m_TargetSyncPosition;
            networkTransformChild.m_Target.localRotation = networkTransformChild.m_TargetSyncRotation3D;
          }
        }
      }
    }

    public override int GetNetworkChannel()
    {
      return 1;
    }

    public override float GetNetworkSendInterval()
    {
      return this.m_SendInterval;
    }
  }
}
