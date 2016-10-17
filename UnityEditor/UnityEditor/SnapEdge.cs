// Decompiled with JetBrains decompiler
// Type: UnityEditor.SnapEdge
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SnapEdge
  {
    private const float kSnapDist = 0.0f;
    public Object m_Object;
    public float pos;
    public float start;
    public float end;
    public float startDragPos;
    public float startDragStart;
    public SnapEdge.EdgeDir dir;

    internal SnapEdge(Object win, SnapEdge.EdgeDir _d, float _p, float _s, float _e)
    {
      this.dir = _d;
      this.m_Object = win;
      this.pos = _p;
      this.start = _s;
      this.end = _e;
    }

    public override string ToString()
    {
      if (!(this.m_Object != (Object) null))
        return "Edge: " + (object) this.dir + " of NULL - something is wrong!";
      return "Edge: " + (object) this.dir + " of " + this.m_Object.name + "    pos: " + (object) this.pos + " (" + (object) this.start + " - " + (object) this.end + ")";
    }

    internal static SnapEdge.EdgeDir OppositeEdge(SnapEdge.EdgeDir dir)
    {
      switch (dir)
      {
        case SnapEdge.EdgeDir.Left:
          return SnapEdge.EdgeDir.Right;
        case SnapEdge.EdgeDir.Right:
          return SnapEdge.EdgeDir.Left;
        case SnapEdge.EdgeDir.CenterX:
          return SnapEdge.EdgeDir.CenterX;
        case SnapEdge.EdgeDir.Up:
          return SnapEdge.EdgeDir.Down;
        case SnapEdge.EdgeDir.Down:
          return SnapEdge.EdgeDir.Up;
        default:
          return SnapEdge.EdgeDir.CenterY;
      }
    }

    private int EdgeCoordinateIndex()
    {
      return this.dir == SnapEdge.EdgeDir.Left || this.dir == SnapEdge.EdgeDir.Right || this.dir == SnapEdge.EdgeDir.CenterX ? 0 : 1;
    }

    internal static Vector2 Snap(List<SnapEdge> sourceEdges, List<SnapEdge> edgesToSnapAgainst, List<KeyValuePair<SnapEdge, SnapEdge>>[] activeEdges)
    {
      Vector2 zero = Vector2.zero;
      float num = 10f;
      activeEdges[0].Clear();
      activeEdges[1].Clear();
      float[] numArray1 = new float[2]{ num, num };
      float[] numArray2 = new float[2];
      using (List<SnapEdge>.Enumerator enumerator = sourceEdges.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SnapEdge current = enumerator.Current;
          int index = current.EdgeCoordinateIndex();
          SnapEdge.Snap(current, edgesToSnapAgainst, ref numArray1[index], ref numArray2[index], activeEdges[index]);
        }
      }
      zero.x = numArray2[0];
      zero.y = numArray2[1];
      return zero;
    }

    private static bool EdgeInside(SnapEdge edge, List<SnapEdge> frustum)
    {
      using (List<SnapEdge>.Enumerator enumerator = frustum.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SnapEdge current = enumerator.Current;
          if (!SnapEdge.ShouldEdgesSnap(edge, current))
            return false;
        }
      }
      return true;
    }

    private static bool ShouldEdgesSnap(SnapEdge a, SnapEdge b)
    {
      if ((a.dir == SnapEdge.EdgeDir.CenterX || a.dir == SnapEdge.EdgeDir.CenterY) && a.dir == b.dir)
        return true;
      if (a.dir == SnapEdge.OppositeEdge(b.dir))
        return ((double) a.start > (double) b.end ? 1 : ((double) a.end < (double) b.start ? 1 : 0)) == 0;
      return false;
    }

    internal static void Snap(SnapEdge edge, List<SnapEdge> otherEdges, ref float maxDist, ref float snapVal, List<KeyValuePair<SnapEdge, SnapEdge>> activeEdges)
    {
      using (List<SnapEdge>.Enumerator enumerator = otherEdges.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SnapEdge current = enumerator.Current;
          if (SnapEdge.ShouldEdgesSnap(edge, current))
          {
            float num = Mathf.Abs(current.pos - edge.pos);
            if ((double) num < (double) maxDist)
            {
              maxDist = num;
              snapVal = current.pos - edge.pos;
              activeEdges.Clear();
              activeEdges.Add(new KeyValuePair<SnapEdge, SnapEdge>(edge, current));
            }
            else if ((double) num == (double) maxDist)
              activeEdges.Add(new KeyValuePair<SnapEdge, SnapEdge>(edge, current));
          }
        }
      }
    }

    internal void ApplyOffset(Vector2 offset, bool windowMove)
    {
      offset = this.dir == SnapEdge.EdgeDir.Left || this.dir == SnapEdge.EdgeDir.Right ? offset : new Vector2(offset.y, offset.x);
      if (windowMove)
        this.pos += offset.x;
      else
        this.pos = offset.x + this.startDragPos;
      this.start += offset.y;
      this.end += offset.y;
    }

    public enum EdgeDir
    {
      Left,
      Right,
      CenterX,
      Up,
      Down,
      CenterY,
      None,
    }
  }
}
