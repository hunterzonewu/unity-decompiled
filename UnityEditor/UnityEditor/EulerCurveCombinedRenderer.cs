// Decompiled with JetBrains decompiler
// Type: UnityEditor.EulerCurveCombinedRenderer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class EulerCurveCombinedRenderer
  {
    private float cachedEvaluationTime = float.PositiveInfinity;
    private float cachedRangeStart = float.PositiveInfinity;
    private float cachedRangeEnd = float.NegativeInfinity;
    private WrapMode preWrapMode = WrapMode.Once;
    private WrapMode postWrapMode = WrapMode.Once;
    private const int kSegmentResolution = 40;
    private const float epsilon = 0.001f;
    private AnimationCurve quaternionX;
    private AnimationCurve quaternionY;
    private AnimationCurve quaternionZ;
    private AnimationCurve quaternionW;
    private AnimationCurve eulerX;
    private AnimationCurve eulerY;
    private AnimationCurve eulerZ;
    private SortedDictionary<float, Vector3> points;
    private Vector3 cachedEvaluationValue;
    private Vector3 refEuler;
    private float m_CustomRangeStart;
    private float m_CustomRangeEnd;

    private float rangeStart
    {
      get
      {
        if ((double) this.m_CustomRangeStart == 0.0 && (double) this.m_CustomRangeEnd == 0.0 && this.eulerX.length > 0)
          return this.eulerX.keys[0].time;
        return this.m_CustomRangeStart;
      }
    }

    private float rangeEnd
    {
      get
      {
        if ((double) this.m_CustomRangeStart == 0.0 && (double) this.m_CustomRangeEnd == 0.0 && this.eulerX.length > 0)
          return this.eulerX.keys[this.eulerX.length - 1].time;
        return this.m_CustomRangeEnd;
      }
    }

    public EulerCurveCombinedRenderer(AnimationCurve quaternionX, AnimationCurve quaternionY, AnimationCurve quaternionZ, AnimationCurve quaternionW, AnimationCurve eulerX, AnimationCurve eulerY, AnimationCurve eulerZ)
    {
      this.quaternionX = quaternionX != null ? quaternionX : new AnimationCurve();
      this.quaternionY = quaternionY != null ? quaternionY : new AnimationCurve();
      this.quaternionZ = quaternionZ != null ? quaternionZ : new AnimationCurve();
      this.quaternionW = quaternionW != null ? quaternionW : new AnimationCurve();
      this.eulerX = eulerX != null ? eulerX : new AnimationCurve();
      this.eulerY = eulerY != null ? eulerY : new AnimationCurve();
      this.eulerZ = eulerZ != null ? eulerZ : new AnimationCurve();
    }

    public AnimationCurve GetCurveOfComponent(int component)
    {
      switch (component)
      {
        case 0:
          return this.eulerX;
        case 1:
          return this.eulerY;
        case 2:
          return this.eulerZ;
        default:
          return (AnimationCurve) null;
      }
    }

    public float RangeStart()
    {
      return this.rangeStart;
    }

    public float RangeEnd()
    {
      return this.rangeEnd;
    }

    public WrapMode PreWrapMode()
    {
      return this.preWrapMode;
    }

    public WrapMode PostWrapMode()
    {
      return this.postWrapMode;
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

    private Vector3 GetValues(float time, bool keyReference)
    {
      if (this.quaternionX == null)
        Debug.LogError((object) "X curve is null!");
      if (this.quaternionY == null)
        Debug.LogError((object) "Y curve is null!");
      if (this.quaternionZ == null)
        Debug.LogError((object) "Z curve is null!");
      if (this.quaternionW == null)
        Debug.LogError((object) "W curve is null!");
      if (this.quaternionX.length != 0 && this.quaternionY.length != 0 && (this.quaternionZ.length != 0 && this.quaternionW.length != 0))
      {
        Quaternion quaternionCurvesDirectly = this.EvaluateQuaternionCurvesDirectly(time);
        if (keyReference)
          this.refEuler = this.EvaluateEulerCurvesDirectly(time);
        this.refEuler = QuaternionCurveTangentCalculation.GetEulerFromQuaternion(quaternionCurvesDirectly, this.refEuler);
      }
      else
        this.refEuler = this.EvaluateEulerCurvesDirectly(time);
      return this.refEuler;
    }

    private Quaternion EvaluateQuaternionCurvesDirectly(float time)
    {
      return new Quaternion(this.quaternionX.Evaluate(time), this.quaternionY.Evaluate(time), this.quaternionZ.Evaluate(time), this.quaternionW.Evaluate(time));
    }

    private Vector3 EvaluateEulerCurvesDirectly(float time)
    {
      return new Vector3(this.eulerX.Evaluate(time), this.eulerY.Evaluate(time), this.eulerZ.Evaluate(time));
    }

    private void CalculateCurves(float minTime, float maxTime)
    {
      this.points = new SortedDictionary<float, Vector3>();
      float[,] ranges = NormalCurveRenderer.CalculateRanges(minTime, maxTime, this.rangeStart, this.rangeEnd, this.preWrapMode, this.postWrapMode);
      for (int index = 0; index < ranges.GetLength(0); ++index)
        this.AddPoints(ranges[index, 0], ranges[index, 1]);
    }

    private void AddPoints(float minTime, float maxTime)
    {
      AnimationCurve animationCurve = this.quaternionX;
      if (animationCurve.length == 0)
        animationCurve = this.eulerX;
      if (animationCurve.length == 0)
        return;
      if ((double) animationCurve[0].time >= (double) minTime)
      {
        Vector3 values = this.GetValues(animationCurve[0].time, true);
        this.points[this.rangeStart] = values;
        this.points[animationCurve[0].time] = values;
      }
      if ((double) animationCurve[animationCurve.length - 1].time <= (double) maxTime)
      {
        Vector3 values = this.GetValues(animationCurve[animationCurve.length - 1].time, true);
        this.points[animationCurve[animationCurve.length - 1].time] = values;
        this.points[this.rangeEnd] = values;
      }
      for (int index = 0; index < animationCurve.length - 1; ++index)
      {
        if ((double) animationCurve[index + 1].time >= (double) minTime && (double) animationCurve[index].time <= (double) maxTime)
        {
          float time1 = animationCurve[index].time;
          this.points[time1] = this.GetValues(time1, true);
          for (float num = 1f; (double) num <= 20.0; ++num)
          {
            float time2 = Mathf.Lerp(animationCurve[index].time, animationCurve[index + 1].time, (float) (((double) num - 1.0 / 1000.0) / 40.0));
            this.points[time2] = this.GetValues(time2, false);
          }
          float time3 = animationCurve[index + 1].time;
          this.points[time3] = this.GetValues(time3, true);
          for (float num = 1f; (double) num <= 20.0; ++num)
          {
            float time2 = Mathf.Lerp(animationCurve[index].time, animationCurve[index + 1].time, (float) (1.0 - ((double) num - 1.0 / 1000.0) / 40.0));
            this.points[time2] = this.GetValues(time2, false);
          }
        }
      }
    }

    public float EvaluateCurveDeltaSlow(float time, int component)
    {
      if (this.quaternionX == null)
        return 0.0f;
      return (float) (((double) this.EvaluateCurveSlow(time + 1f / 1000f, component) - (double) this.EvaluateCurveSlow(time - 1f / 1000f, component)) / (1.0 / 500.0));
    }

    public float EvaluateCurveSlow(float time, int component)
    {
      if (this.GetCurveOfComponent(component).length == 1)
        return this.GetCurveOfComponent(component)[0].value;
      if ((double) time == (double) this.cachedEvaluationTime)
        return this.cachedEvaluationValue[component];
      if ((double) time < (double) this.cachedRangeStart || (double) time > (double) this.cachedRangeEnd)
      {
        this.CalculateCurves(this.rangeStart, this.rangeEnd);
        this.cachedRangeStart = float.NegativeInfinity;
        this.cachedRangeEnd = float.PositiveInfinity;
      }
      float[] numArray = new float[this.points.Count];
      Vector3[] vector3Array = new Vector3[this.points.Count];
      int index1 = 0;
      using (SortedDictionary<float, Vector3>.Enumerator enumerator = this.points.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<float, Vector3> current = enumerator.Current;
          numArray[index1] = current.Key;
          vector3Array[index1] = current.Value;
          ++index1;
        }
      }
      for (int index2 = 0; index2 < numArray.Length - 1; ++index2)
      {
        if ((double) time < (double) numArray[index2 + 1])
        {
          float t = Mathf.InverseLerp(numArray[index2], numArray[index2 + 1], time);
          this.cachedEvaluationValue = Vector3.Lerp(vector3Array[index2], vector3Array[index2 + 1], t);
          this.cachedEvaluationTime = time;
          return this.cachedEvaluationValue[component];
        }
      }
      if (vector3Array.Length > 0)
        return vector3Array[vector3Array.Length - 1][component];
      Debug.LogError((object) "List of euler curve points is empty, probably caused by lack of euler curve key synching");
      return 0.0f;
    }

    public void DrawCurve(float minTime, float maxTime, Color color, Matrix4x4 transform, int component, Color wrapColor)
    {
      if ((double) minTime < (double) this.cachedRangeStart || (double) maxTime > (double) this.cachedRangeEnd)
      {
        this.CalculateCurves(minTime, maxTime);
        if ((double) minTime <= (double) this.rangeStart && (double) maxTime >= (double) this.rangeEnd)
        {
          this.cachedRangeStart = float.NegativeInfinity;
          this.cachedRangeEnd = float.PositiveInfinity;
        }
        else
        {
          this.cachedRangeStart = minTime;
          this.cachedRangeEnd = maxTime;
        }
      }
      List<Vector3> vector3List = new List<Vector3>();
      using (SortedDictionary<float, Vector3>.Enumerator enumerator = this.points.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<float, Vector3> current = enumerator.Current;
          vector3List.Add(new Vector3(current.Key, current.Value[component]));
        }
      }
      NormalCurveRenderer.DrawCurveWrapped(minTime, maxTime, this.rangeStart, this.rangeEnd, this.preWrapMode, this.postWrapMode, color, transform, vector3List.ToArray(), wrapColor);
    }

    public Bounds GetBounds(float minTime, float maxTime, int component)
    {
      this.CalculateCurves(minTime, maxTime);
      float num1 = float.PositiveInfinity;
      float num2 = float.NegativeInfinity;
      using (SortedDictionary<float, Vector3>.Enumerator enumerator = this.points.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<float, Vector3> current = enumerator.Current;
          if ((double) current.Value[component] > (double) num2)
            num2 = current.Value[component];
          if ((double) current.Value[component] < (double) num1)
            num1 = current.Value[component];
        }
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
