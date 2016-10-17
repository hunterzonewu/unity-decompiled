// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.RectUtils
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class RectUtils
  {
    public static bool Contains(Rect a, Rect b)
    {
      return (double) a.xMin <= (double) b.xMin && (double) a.xMax >= (double) b.xMax && ((double) a.yMin <= (double) b.yMin && (double) a.yMax >= (double) b.yMax);
    }

    public static Rect Encompass(Rect a, Rect b)
    {
      Rect rect = a;
      rect.xMin = Math.Min(a.xMin, b.xMin);
      rect.yMin = Math.Min(a.yMin, b.yMin);
      rect.xMax = Math.Max(a.xMax, b.xMax);
      rect.yMax = Math.Max(a.yMax, b.yMax);
      return rect;
    }

    public static Rect Inflate(Rect a, float factor)
    {
      return RectUtils.Inflate(a, factor, factor);
    }

    public static Rect Inflate(Rect a, float factorX, float factorY)
    {
      float num1 = a.width * factorX;
      float num2 = a.height * factorY;
      float num3 = (float) (((double) num1 - (double) a.width) / 2.0);
      float num4 = (float) (((double) num2 - (double) a.height) / 2.0);
      Rect rect = a;
      rect.xMin -= num3;
      rect.yMin -= num4;
      rect.xMax += num3;
      rect.yMax += num4;
      return rect;
    }

    public static bool Intersects(Rect r1, Rect r2)
    {
      return r1.Overlaps(r2) || r2.Overlaps(r1);
    }

    public static bool Intersection(Rect r1, Rect r2, out Rect intersection)
    {
      if (!r1.Overlaps(r2) && !r2.Overlaps(r1))
      {
        intersection = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        return false;
      }
      float x = Mathf.Max(r1.xMin, r2.xMin);
      float y = Mathf.Max(r1.yMin, r2.yMin);
      float num1 = Mathf.Min(r1.xMax, r2.xMax);
      float num2 = Mathf.Min(r1.yMax, r2.yMax);
      intersection = new Rect(x, y, num1 - x, num2 - y);
      return true;
    }

    public static bool IntersectsSegment(Rect rect, Vector2 p1, Vector2 p2)
    {
      float num1 = Mathf.Min(p1.x, p2.x);
      float num2 = Mathf.Max(p1.x, p2.x);
      if ((double) num2 > (double) rect.xMax)
        num2 = rect.xMax;
      if ((double) num1 < (double) rect.xMin)
        num1 = rect.xMin;
      if ((double) num1 > (double) num2)
        return false;
      float num3 = Mathf.Min(p1.y, p2.y);
      float num4 = Mathf.Max(p1.y, p2.y);
      float f = p2.x - p1.x;
      if ((double) Mathf.Abs(f) > 1.0000000116861E-07)
      {
        float num5 = (p2.y - p1.y) / f;
        float num6 = p1.y - num5 * p1.x;
        num3 = num5 * num1 + num6;
        num4 = num5 * num2 + num6;
      }
      if ((double) num3 > (double) num4)
      {
        float num5 = num4;
        num4 = num3;
        num3 = num5;
      }
      if ((double) num4 > (double) rect.yMax)
        num4 = rect.yMax;
      if ((double) num3 < (double) rect.yMin)
        num3 = rect.yMin;
      return (double) num3 <= (double) num4;
    }

    public static Rect OffsetX(Rect r, float offsetX)
    {
      return RectUtils.Offset(r, offsetX, 0.0f);
    }

    public static Rect Offset(Rect r, float offsetX, float offsetY)
    {
      Rect rect = r;
      rect.xMin += offsetX;
      rect.yMin += offsetY;
      return rect;
    }

    public static Rect Offset(Rect a, Rect b)
    {
      Rect rect = a;
      rect.xMin += b.xMin;
      rect.yMin += b.yMin;
      return rect;
    }

    public static Rect Move(Rect r, Vector2 delta)
    {
      Rect rect = r;
      rect.xMin += delta.x;
      rect.yMin += delta.y;
      rect.xMax += delta.x;
      rect.yMax += delta.y;
      return rect;
    }
  }
}
