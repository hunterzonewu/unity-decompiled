// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuoyancyEffector2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (BuoyancyEffector2D), true)]
  [CanEditMultipleObjects]
  internal class BuoyancyEffector2DEditor : Effector2DEditor
  {
    public void OnSceneGUI()
    {
      BuoyancyEffector2D target = (BuoyancyEffector2D) this.target;
      if (!target.enabled)
        return;
      float y = target.transform.position.y + target.transform.lossyScale.y * target.surfaceLevel;
      List<Vector3> vector3List = new List<Vector3>();
      float num = float.NegativeInfinity;
      float x1 = num;
      foreach (Collider2D collider2D in ((IEnumerable<Collider2D>) target.gameObject.GetComponents<Collider2D>()).Where<Collider2D>((Func<Collider2D, bool>) (c =>
      {
        if (c.enabled)
          return c.usedByEffector;
        return false;
      })))
      {
        Bounds bounds = collider2D.bounds;
        float x2 = bounds.min.x;
        float x3 = bounds.max.x;
        if (float.IsNegativeInfinity(num))
        {
          num = x2;
          x1 = x3;
        }
        else
        {
          if ((double) x2 < (double) num)
            num = x2;
          if ((double) x3 > (double) x1)
            x1 = x3;
        }
        Vector3 vector3_1 = new Vector3(x2, y, 0.0f);
        Vector3 vector3_2 = new Vector3(x3, y, 0.0f);
        vector3List.Add(vector3_1);
        vector3List.Add(vector3_2);
      }
      Handles.color = Color.red;
      Handles.DrawAAPolyLine(new Vector3[2]
      {
        new Vector3(num, y, 0.0f),
        new Vector3(x1, y, 0.0f)
      });
      Handles.color = Color.cyan;
      int index = 0;
      while (index < vector3List.Count - 1)
      {
        Handles.DrawAAPolyLine(new Vector3[2]
        {
          vector3List[index],
          vector3List[index + 1]
        });
        index += 2;
      }
    }
  }
}
