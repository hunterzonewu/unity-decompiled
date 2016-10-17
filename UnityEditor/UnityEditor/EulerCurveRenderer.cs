// Decompiled with JetBrains decompiler
// Type: UnityEditor.EulerCurveRenderer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class EulerCurveRenderer : CurveRenderer
  {
    private int component;
    private EulerCurveCombinedRenderer renderer;

    public EulerCurveRenderer(int component, EulerCurveCombinedRenderer renderer)
    {
      this.component = component;
      this.renderer = renderer;
    }

    public AnimationCurve GetCurve()
    {
      return this.renderer.GetCurveOfComponent(this.component);
    }

    public float RangeStart()
    {
      return this.renderer.RangeStart();
    }

    public float RangeEnd()
    {
      return this.renderer.RangeEnd();
    }

    public void SetWrap(WrapMode wrap)
    {
      this.renderer.SetWrap(wrap);
    }

    public void SetWrap(WrapMode preWrapMode, WrapMode postWrapMode)
    {
      this.renderer.SetWrap(preWrapMode, postWrapMode);
    }

    public void SetCustomRange(float start, float end)
    {
      this.renderer.SetCustomRange(start, end);
    }

    public float EvaluateCurveSlow(float time)
    {
      return this.renderer.EvaluateCurveSlow(time, this.component);
    }

    public float EvaluateCurveDeltaSlow(float time)
    {
      return this.renderer.EvaluateCurveDeltaSlow(time, this.component);
    }

    public void DrawCurve(float minTime, float maxTime, Color color, Matrix4x4 transform, Color wrapColor)
    {
      this.renderer.DrawCurve(minTime, maxTime, color, transform, this.component, wrapColor);
    }

    public Bounds GetBounds()
    {
      return this.GetBounds(this.renderer.RangeStart(), this.renderer.RangeEnd());
    }

    public Bounds GetBounds(float minTime, float maxTime)
    {
      return this.renderer.GetBounds(minTime, maxTime, this.component);
    }
  }
}
