// Decompiled with JetBrains decompiler
// Type: UnityEditor.Utility2D
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class Utility2D
  {
    public static Vector3 ScreenToLocal(Transform transform, Vector2 screenPosition)
    {
      Plane plane = new Plane(transform.forward * -1f, transform.position);
      Ray ray;
      if (Camera.current.orthographic)
      {
        Vector2 vector2 = GUIClip.Unclip(screenPosition);
        vector2.y = (float) Screen.height - vector2.y;
        ray = new Ray(Camera.current.ScreenToWorldPoint((Vector3) vector2), Camera.current.transform.forward);
      }
      else
        ray = HandleUtility.GUIPointToWorldRay(screenPosition);
      float enter;
      plane.Raycast(ray, out enter);
      Vector3 point = ray.GetPoint(enter);
      return transform.InverseTransformPoint(point);
    }
  }
}
