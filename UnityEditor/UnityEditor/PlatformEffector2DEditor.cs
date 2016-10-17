// Decompiled with JetBrains decompiler
// Type: UnityEditor.PlatformEffector2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (PlatformEffector2D), true)]
  [CanEditMultipleObjects]
  internal class PlatformEffector2DEditor : Effector2DEditor
  {
    public void OnSceneGUI()
    {
      PlatformEffector2D target = (PlatformEffector2D) this.target;
      if (!target.enabled)
        return;
      if (target.useOneWay)
        PlatformEffector2DEditor.DrawSurfaceArc(target);
      if (target.useSideBounce && target.useSideFriction)
        return;
      PlatformEffector2DEditor.DrawSideArc(target);
    }

    private static void DrawSurfaceArc(PlatformEffector2D effector)
    {
      Vector3 vector3_1 = effector.transform.TransformVector(Vector3.up);
      float f = (float) (Math.PI / 180.0 * (double) effector.surfaceArc * 0.5) - Mathf.Atan2(vector3_1.x, vector3_1.y);
      float angle = Mathf.Clamp(effector.surfaceArc, 0.5f, 360f);
      float num = angle * ((float) Math.PI / 180f);
      foreach (Collider2D collider2D in ((IEnumerable<Collider2D>) effector.gameObject.GetComponents<Collider2D>()).Where<Collider2D>((Func<Collider2D, bool>) (collider =>
      {
        if (collider.enabled)
          return collider.usedByEffector;
        return false;
      })))
      {
        Vector3 center = collider2D.bounds.center;
        float handleSize = HandleUtility.GetHandleSize(center);
        Vector3 from = new Vector3(-Mathf.Sin(f), Mathf.Cos(f), 0.0f);
        Vector3 vector3_2 = new Vector3(-Mathf.Sin(f - num), Mathf.Cos(f - num), 0.0f);
        Handles.color = new Color(0.0f, 1f, 1f, 0.03f);
        Handles.DrawSolidArc(center, Vector3.back, from, angle, handleSize);
        Handles.color = new Color(0.0f, 1f, 1f, 0.7f);
        Handles.DrawWireArc(center, Vector3.back, from, angle, handleSize);
        Handles.DrawDottedLine(center, center + from * handleSize, 5f);
        Handles.DrawDottedLine(center, center + vector3_2 * handleSize, 5f);
      }
    }

    private static void DrawSideArc(PlatformEffector2D effector)
    {
      Vector3 vector3_1 = effector.transform.TransformVector(Vector3.up);
      float f = (float) (Math.PI / 180.0 * (double) effector.sideArc * 0.5) - Mathf.Atan2(vector3_1.x, vector3_1.y);
      float angle = Mathf.Clamp(effector.sideArc, 0.5f, 180f);
      float num = angle * ((float) Math.PI / 180f);
      foreach (Collider2D collider2D in ((IEnumerable<Collider2D>) effector.gameObject.GetComponents<Collider2D>()).Where<Collider2D>((Func<Collider2D, bool>) (collider =>
      {
        if (collider.enabled)
          return collider.usedByEffector;
        return false;
      })))
      {
        Vector3 center = collider2D.bounds.center;
        float radius = HandleUtility.GetHandleSize(center) * 0.8f;
        Vector3 from1 = new Vector3(-Mathf.Cos(f), -Mathf.Sin(f), 0.0f);
        Vector3 vector3_2 = new Vector3(-Mathf.Cos(f - num), -Mathf.Sin(f - num), 0.0f);
        Vector3 from2 = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f);
        Vector3 vector3_3 = new Vector3(Mathf.Cos(f - num), Mathf.Sin(f - num), 0.0f);
        Handles.color = new Color(0.0f, 1f, 0.7f, 0.03f);
        Handles.DrawSolidArc(center, Vector3.back, from1, angle, radius);
        Handles.DrawSolidArc(center, Vector3.back, from2, angle, radius);
        Handles.color = new Color(0.0f, 1f, 0.7f, 0.7f);
        Handles.DrawWireArc(center, Vector3.back, from1, angle, radius);
        Handles.DrawWireArc(center, Vector3.back, from2, angle, radius);
        Handles.DrawDottedLine(center + from1 * radius, center + from2 * radius, 5f);
        Handles.DrawDottedLine(center + vector3_2 * radius, center + vector3_3 * radius, 5f);
      }
    }
  }
}
