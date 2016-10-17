// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.Physics2DRaycaster
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Raycaster for casting against 2D Physics components.</para>
  /// </summary>
  [RequireComponent(typeof (Camera))]
  [AddComponentMenu("Event/Physics 2D Raycaster")]
  public class Physics2DRaycaster : PhysicsRaycaster
  {
    protected Physics2DRaycaster()
    {
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
      if ((Object) this.eventCamera == (Object) null)
        return;
      RaycastHit2D[] rayIntersectionAll = Physics2D.GetRayIntersectionAll(this.eventCamera.ScreenPointToRay((Vector3) eventData.position), this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane, this.finalEventMask);
      if (rayIntersectionAll.Length == 0)
        return;
      int index = 0;
      for (int length = rayIntersectionAll.Length; index < length; ++index)
      {
        SpriteRenderer component = rayIntersectionAll[index].collider.gameObject.GetComponent<SpriteRenderer>();
        RaycastResult raycastResult = new RaycastResult() { gameObject = rayIntersectionAll[index].collider.gameObject, module = (BaseRaycaster) this, distance = Vector3.Distance(this.eventCamera.transform.position, rayIntersectionAll[index].transform.position), worldPosition = (Vector3) rayIntersectionAll[index].point, worldNormal = (Vector3) rayIntersectionAll[index].normal, screenPosition = eventData.position, index = (float) resultAppendList.Count, sortingLayer = !((Object) component != (Object) null) ? 0 : component.sortingLayerID, sortingOrder = !((Object) component != (Object) null) ? 0 : component.sortingOrder };
        resultAppendList.Add(raycastResult);
      }
    }
  }
}
