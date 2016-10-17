// Decompiled with JetBrains decompiler
// Type: UnityEditor.MuscleClipEditorUtilities
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
  internal sealed class MuscleClipEditorUtilities
  {
    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern MuscleClipQualityInfo GetMuscleClipQualityInfo(AnimationClip clip, float startTime, float stopTime);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void CalculateQualityCurves(AnimationClip clip, QualityCurvesTime time, Vector2[] poseCurve, Vector2[] rotationCurve, Vector2[] heightCurve, Vector2[] positionCurve);
  }
}
