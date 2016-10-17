// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Clipping
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System.Collections.Generic;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>Utility class to help when clipping using IClipper.</para>
  /// </summary>
  public static class Clipping
  {
    public static Rect FindCullAndClipWorldRect(List<RectMask2D> rectMaskParents, out bool validRect)
    {
      if (rectMaskParents.Count == 0)
      {
        validRect = false;
        return new Rect();
      }
      Rect a = rectMaskParents[0].canvasRect;
      for (int index = 0; index < rectMaskParents.Count; ++index)
        a = Clipping.RectIntersect(a, rectMaskParents[index].canvasRect);
      if ((double) a.width <= 0.0 || (double) a.height <= 0.0)
      {
        validRect = false;
        return new Rect();
      }
      Vector3 vector3_1 = new Vector3(a.x, a.y, 0.0f);
      Vector3 vector3_2 = new Vector3(a.x + a.width, a.y + a.height, 0.0f);
      validRect = true;
      return new Rect(vector3_1.x, vector3_1.y, vector3_2.x - vector3_1.x, vector3_2.y - vector3_1.y);
    }

    private static Rect RectIntersect(Rect a, Rect b)
    {
      float x = Mathf.Max(a.x, b.x);
      float num1 = Mathf.Min(a.x + a.width, b.x + b.width);
      float y = Mathf.Max(a.y, b.y);
      float num2 = Mathf.Min(a.y + a.height, b.y + b.height);
      if ((double) num1 >= (double) x && (double) num2 >= (double) y)
        return new Rect(x, y, num1 - x, num2 - y);
      return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    }
  }
}
