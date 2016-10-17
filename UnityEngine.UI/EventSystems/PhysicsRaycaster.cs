// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.PhysicsRaycaster
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Raycaster for casting against 3D Physics components.</para>
  /// </summary>
  [RequireComponent(typeof (Camera))]
  [AddComponentMenu("Event/Physics Raycaster")]
  public class PhysicsRaycaster : BaseRaycaster
  {
    [SerializeField]
    protected LayerMask m_EventMask = (LayerMask) -1;
    protected const int kNoEventMaskSet = -1;
    protected Camera m_EventCamera;

    /// <summary>
    ///   <para>Get the camera that is used for this module.</para>
    /// </summary>
    public override Camera eventCamera
    {
      get
      {
        if ((UnityEngine.Object) this.m_EventCamera == (UnityEngine.Object) null)
          this.m_EventCamera = this.GetComponent<Camera>();
        return this.m_EventCamera ?? Camera.main;
      }
    }

    /// <summary>
    ///   <para>Get the depth of the configured camera.</para>
    /// </summary>
    public virtual int depth
    {
      get
      {
        if ((UnityEngine.Object) this.eventCamera != (UnityEngine.Object) null)
          return (int) this.eventCamera.depth;
        return 16777215;
      }
    }

    /// <summary>
    ///   <para>Logical and of Camera mask and eventMask.</para>
    /// </summary>
    public int finalEventMask
    {
      get
      {
        if ((UnityEngine.Object) this.eventCamera != (UnityEngine.Object) null)
          return this.eventCamera.cullingMask & (int) this.m_EventMask;
        return -1;
      }
    }

    /// <summary>
    ///   <para>Mask of allowed raycast events.</para>
    /// </summary>
    public LayerMask eventMask
    {
      get
      {
        return this.m_EventMask;
      }
      set
      {
        this.m_EventMask = value;
      }
    }

    protected PhysicsRaycaster()
    {
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
      if ((UnityEngine.Object) this.eventCamera == (UnityEngine.Object) null)
        return;
      RaycastHit[] array = Physics.RaycastAll(this.eventCamera.ScreenPointToRay((Vector3) eventData.position), this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane, this.finalEventMask);
      if (array.Length > 1)
        Array.Sort<RaycastHit>(array, (Comparison<RaycastHit>) ((r1, r2) => r1.distance.CompareTo(r2.distance)));
      if (array.Length == 0)
        return;
      int index = 0;
      for (int length = array.Length; index < length; ++index)
      {
        RaycastResult raycastResult = new RaycastResult() { gameObject = array[index].collider.gameObject, module = (BaseRaycaster) this, distance = array[index].distance, worldPosition = array[index].point, worldNormal = array[index].normal, screenPosition = eventData.position, index = (float) resultAppendList.Count, sortingLayer = 0, sortingOrder = 0 };
        resultAppendList.Add(raycastResult);
      }
    }
  }
}
