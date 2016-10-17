// Decompiled with JetBrains decompiler
// Type: UnityEditor.NormalCurveRenderer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class NormalCurveRenderer : CurveRenderer
  {
    private WrapMode preWrapMode = WrapMode.Once;
    private WrapMode postWrapMode = WrapMode.Once;
    private const float kSegmentWindowResolution = 1000f;
    private const int kMaximumSampleCount = 50;
    private AnimationCurve m_Curve;
    private float m_CustomRangeStart;
    private float m_CustomRangeEnd;

    private float rangeStart
    {
      get
      {
        if ((double) this.m_CustomRangeStart == 0.0 && (double) this.m_CustomRangeEnd == 0.0 && this.m_Curve.length > 0)
          return this.m_Curve.keys[0].time;
        return this.m_CustomRangeStart;
      }
    }

    private float rangeEnd
    {
      get
      {
        if ((double) this.m_CustomRangeStart == 0.0 && (double) this.m_CustomRangeEnd == 0.0 && this.m_Curve.length > 0)
          return this.m_Curve.keys[this.m_Curve.length - 1].time;
        return this.m_CustomRangeEnd;
      }
    }

    public NormalCurveRenderer(AnimationCurve curve)
    {
      this.m_Curve = curve;
      if (this.m_Curve != null)
        return;
      this.m_Curve = new AnimationCurve();
    }

    public AnimationCurve GetCurve()
    {
      return this.m_Curve;
    }

    public float RangeStart()
    {
      return this.rangeStart;
    }

    public float RangeEnd()
    {
      return this.rangeEnd;
    }

    public void SetWrap(WrapMode wrap)
    {
      this.preWrapMode = wrap;
      this.postWrapMode = wrap;
    }

    public void SetWrap(WrapMode preWrap, WrapMode postWrap)
    {
      this.preWrapMode = preWrap;
      this.postWrapMode = postWrap;
    }

    public void SetCustomRange(float start, float end)
    {
      this.m_CustomRangeStart = start;
      this.m_CustomRangeEnd = end;
    }

    public float EvaluateCurveSlow(float time)
    {
      return this.m_Curve.Evaluate(time);
    }

    public float EvaluateCurveDeltaSlow(float time)
    {
      float num = 0.0001f;
      return (float) (((double) this.m_Curve.Evaluate(time + num) - (double) this.m_Curve.Evaluate(time - num)) / ((double) num * 2.0));
    }

    private Vector3[] GetPoints(float minTime, float maxTime)
    {
      List<Vector3> points = new List<Vector3>();
      if (this.m_Curve.length == 0)
        return points.ToArray();
      points.Capacity = 1000 + this.m_Curve.length;
      float[,] ranges = NormalCurveRenderer.CalculateRanges(minTime, maxTime, this.rangeStart, this.rangeEnd, this.preWrapMode, this.postWrapMode);
      for (int index = 0; index < ranges.GetLength(0); ++index)
        this.AddPoints(ref points, ranges[index, 0], ranges[index, 1], minTime, maxTime);
      if (points.Count > 0)
      {
        for (int index = 1; index < points.Count; ++index)
        {
          if ((double) points[index].x < (double) points[index - 1].x)
          {
            points.RemoveAt(index);
            --index;
          }
        }
      }
      return points.ToArray();
    }

    public static float[,] CalculateRanges(float minTime, float maxTime, float rangeStart, float rangeEnd, WrapMode preWrapMode, WrapMode postWrapMode)
    {
      WrapMode wrapMode = preWrapMode;
      if (postWrapMode != wrapMode)
        return new float[1, 2]
        {
          {
            rangeStart,
            rangeEnd
          }
        };
      if (wrapMode == WrapMode.Loop)
      {
        if ((double) maxTime - (double) minTime > (double) rangeEnd - (double) rangeStart)
          return new float[1, 2]
          {
            {
              rangeStart,
              rangeEnd
            }
          };
        minTime = Mathf.Repeat(minTime - rangeStart, rangeEnd - rangeStart) + rangeStart;
        maxTime = Mathf.Repeat(maxTime - rangeStart, rangeEnd - rangeStart) + rangeStart;
        if ((double) minTime < (double) maxTime)
          return new float[1, 2]
          {
            {
              minTime,
              maxTime
            }
          };
        return new float[2, 2]
        {
          {
            rangeStart,
            maxTime
          },
          {
            minTime,
            rangeEnd
          }
        };
      }
      if (wrapMode == WrapMode.PingPong)
        return new float[1, 2]
        {
          {
            rangeStart,
            rangeEnd
          }
        };
      return new float[1, 2]
      {
        {
          minTime,
          maxTime
        }
      };
    }

    private static int GetSegmentResolution(float minTime, float maxTime, float keyTime, float nextKeyTime)
    {
      float num = maxTime - minTime;
      return Mathf.Clamp(Mathf.RoundToInt((float) (1000.0 * ((double) (nextKeyTime - keyTime) / (double) num))), 1, 50);
    }

    private void AddPoints(ref List<Vector3> points, float minTime, float maxTime, float visibleMinTime, float visibleMaxTime)
    {
      if ((double) this.m_Curve[0].time >= (double) minTime)
      {
        points.Add(new Vector3(this.rangeStart, this.m_Curve[0].value));
        points.Add(new Vector3(this.m_Curve[0].time, this.m_Curve[0].value));
      }
      for (int index = 0; index < this.m_Curve.length - 1; ++index)
      {
        Keyframe keyframe1 = this.m_Curve[index];
        Keyframe keyframe2 = this.m_Curve[index + 1];
        if ((double) keyframe2.time >= (double) minTime && (double) keyframe1.time <= (double) maxTime)
        {
          points.Add(new Vector3(keyframe1.time, keyframe1.value));
          int segmentResolution = NormalCurveRenderer.GetSegmentResolution(visibleMinTime, visibleMaxTime, keyframe1.time, keyframe2.time);
          float num1 = Mathf.Lerp(keyframe1.time, keyframe2.time, 1f / 1000f / (float) segmentResolution);
          points.Add(new Vector3(num1, this.m_Curve.Evaluate(num1)));
          for (float num2 = 1f; (double) num2 < (double) segmentResolution; ++num2)
          {
            float num3 = Mathf.Lerp(keyframe1.time, keyframe2.time, num2 / (float) segmentResolution);
            points.Add(new Vector3(num3, this.m_Curve.Evaluate(num3)));
          }
          float num4 = Mathf.Lerp(keyframe1.time, keyframe2.time, (float) (1.0 - 1.0 / 1000.0 / (double) segmentResolution));
          points.Add(new Vector3(num4, this.m_Curve.Evaluate(num4)));
          float time = keyframe2.time;
          points.Add(new Vector3(time, keyframe2.value));
        }
      }
      if ((double) this.m_Curve[this.m_Curve.length - 1].time > (double) maxTime)
        return;
      points.Add(new Vector3(this.m_Curve[this.m_Curve.length - 1].time, this.m_Curve[this.m_Curve.length - 1].value));
      points.Add(new Vector3(this.rangeEnd, this.m_Curve[this.m_Curve.length - 1].value));
    }

    public void DrawCurve(float minTime, float maxTime, Color color, Matrix4x4 transform, Color wrapColor)
    {
      Vector3[] points = this.GetPoints(minTime, maxTime);
      NormalCurveRenderer.DrawCurveWrapped(minTime, maxTime, this.rangeStart, this.rangeEnd, this.preWrapMode, this.postWrapMode, color, transform, points, wrapColor);
    }

    public static void DrawPolyLine(Matrix4x4 transform, float minDistance, params Vector3[] points)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Color c = Handles.color * new Color(1f, 1f, 1f, 0.75f);
      HandleUtility.ApplyWireMaterial();
      GL.PushMatrix();
      GL.MultMatrix(Handles.matrix);
      GL.Begin(1);
      GL.Color(c);
      Vector3 v1 = transform.MultiplyPoint(points[0]);
      for (int index = 1; index < points.Length; ++index)
      {
        Vector3 v2 = transform.MultiplyPoint(points[index]);
        if ((double) (v1 - v2).magnitude > (double) minDistance)
        {
          GL.Vertex(v1);
          GL.Vertex(v2);
          v1 = v2;
        }
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawCurveWrapped(float minTime, float maxTime, float rangeStart, float rangeEnd, WrapMode preWrap, WrapMode postWrap, Color color, Matrix4x4 transform, Vector3[] points, Color wrapColor)
    {
      if (points.Length == 0)
        return;
      int num1;
      int num2;
      if ((double) rangeEnd - (double) rangeStart != 0.0)
      {
        num1 = Mathf.FloorToInt((float) (((double) minTime - (double) rangeStart) / ((double) rangeEnd - (double) rangeStart)));
        num2 = Mathf.CeilToInt((float) (((double) maxTime - (double) rangeEnd) / ((double) rangeEnd - (double) rangeStart)));
      }
      else
      {
        preWrap = WrapMode.Once;
        postWrap = WrapMode.Once;
        num1 = (double) minTime >= (double) rangeStart ? 0 : -1;
        num2 = (double) maxTime <= (double) rangeEnd ? 0 : 1;
      }
      int index1 = points.Length - 1;
      Handles.color = color;
      List<Vector3> vector3List1 = new List<Vector3>();
      if (num1 <= 0 && num2 >= 0)
        NormalCurveRenderer.DrawPolyLine(transform, 2f, points);
      else
        Handles.DrawPolyLine(points);
      Handles.color = new Color(wrapColor.r, wrapColor.g, wrapColor.b, wrapColor.a * color.a);
      if (preWrap == WrapMode.Loop)
      {
        List<Vector3> vector3List2 = new List<Vector3>();
        for (int index2 = num1; index2 < 0; ++index2)
        {
          for (int index3 = 0; index3 < points.Length; ++index3)
          {
            Vector3 point = points[index3];
            point.x += (float) index2 * (rangeEnd - rangeStart);
            Vector3 vector3 = transform.MultiplyPoint(point);
            vector3List2.Add(vector3);
          }
        }
        vector3List2.Add(transform.MultiplyPoint(points[0]));
        Handles.DrawPolyLine(vector3List2.ToArray());
      }
      else if (preWrap == WrapMode.PingPong)
      {
        List<Vector3> vector3List2 = new List<Vector3>();
        for (int index2 = num1; index2 < 0; ++index2)
        {
          for (int index3 = 0; index3 < points.Length; ++index3)
          {
            if ((double) (index2 / 2) == (double) index2 / 2.0)
            {
              Vector3 point = points[index3];
              point.x += (float) index2 * (rangeEnd - rangeStart);
              Vector3 vector3 = transform.MultiplyPoint(point);
              vector3List2.Add(vector3);
            }
            else
            {
              Vector3 point = points[index1 - index3];
              point.x = (float) (-(double) point.x + (double) (index2 + 1) * ((double) rangeEnd - (double) rangeStart) + (double) rangeStart * 2.0);
              Vector3 vector3 = transform.MultiplyPoint(point);
              vector3List2.Add(vector3);
            }
          }
        }
        Handles.DrawPolyLine(vector3List2.ToArray());
      }
      else if (num1 < 0)
        Handles.DrawPolyLine(transform.MultiplyPoint(new Vector3(minTime, points[0].y, 0.0f)), transform.MultiplyPoint(new Vector3(Mathf.Min(maxTime, points[0].x), points[0].y, 0.0f)));
      if (postWrap == WrapMode.Loop)
      {
        List<Vector3> vector3List2 = new List<Vector3>();
        vector3List2.Add(transform.MultiplyPoint(points[index1]));
        for (int index2 = 1; index2 <= num2; ++index2)
        {
          for (int index3 = 0; index3 < points.Length; ++index3)
          {
            Vector3 point = points[index3];
            point.x += (float) index2 * (rangeEnd - rangeStart);
            Vector3 vector3 = transform.MultiplyPoint(point);
            vector3List2.Add(vector3);
          }
        }
        Handles.DrawPolyLine(vector3List2.ToArray());
      }
      else if (postWrap == WrapMode.PingPong)
      {
        List<Vector3> vector3List2 = new List<Vector3>();
        for (int index2 = 1; index2 <= num2; ++index2)
        {
          for (int index3 = 0; index3 < points.Length; ++index3)
          {
            if ((double) (index2 / 2) == (double) index2 / 2.0)
            {
              Vector3 point = points[index3];
              point.x += (float) index2 * (rangeEnd - rangeStart);
              Vector3 vector3 = transform.MultiplyPoint(point);
              vector3List2.Add(vector3);
            }
            else
            {
              Vector3 point = points[index1 - index3];
              point.x = (float) (-(double) point.x + (double) (index2 + 1) * ((double) rangeEnd - (double) rangeStart) + (double) rangeStart * 2.0);
              Vector3 vector3 = transform.MultiplyPoint(point);
              vector3List2.Add(vector3);
            }
          }
        }
        Handles.DrawPolyLine(vector3List2.ToArray());
      }
      else
      {
        if (num2 <= 0)
          return;
        Handles.DrawPolyLine(transform.MultiplyPoint(new Vector3(Mathf.Max(minTime, points[index1].x), points[index1].y, 0.0f)), transform.MultiplyPoint(new Vector3(maxTime, points[index1].y, 0.0f)));
      }
    }

    public Bounds GetBounds()
    {
      return this.GetBounds(this.rangeStart, this.rangeEnd);
    }

    public Bounds GetBounds(float minTime, float maxTime)
    {
      Vector3[] points = this.GetPoints(minTime, maxTime);
      float num1 = float.PositiveInfinity;
      float num2 = float.NegativeInfinity;
      foreach (Vector3 vector3 in points)
      {
        if ((double) vector3.y > (double) num2)
          num2 = vector3.y;
        if ((double) vector3.y < (double) num1)
          num1 = vector3.y;
      }
      if ((double) num1 == double.PositiveInfinity)
      {
        num1 = 0.0f;
        num2 = 0.0f;
      }
      return new Bounds(new Vector3((float) (((double) maxTime + (double) minTime) * 0.5), (float) (((double) num2 + (double) num1) * 0.5), 0.0f), new Vector3(maxTime - minTime, num2 - num1, 0.0f));
    }
  }
}
