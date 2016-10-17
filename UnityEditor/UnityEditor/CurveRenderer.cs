// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveRenderer
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal interface CurveRenderer
  {
    void DrawCurve(float minTime, float maxTime, Color color, Matrix4x4 transform, Color wrapColor);

    AnimationCurve GetCurve();

    float RangeStart();

    float RangeEnd();

    void SetWrap(WrapMode wrap);

    void SetWrap(WrapMode preWrap, WrapMode postWrap);

    void SetCustomRange(float start, float end);

    float EvaluateCurveSlow(float time);

    float EvaluateCurveDeltaSlow(float time);

    Bounds GetBounds();

    Bounds GetBounds(float minTime, float maxTime);
  }
}
