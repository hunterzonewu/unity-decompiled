// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParticleSystemClipboard
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ParticleSystemClipboard
  {
    private static AnimationCurve m_AnimationCurve1;
    private static AnimationCurve m_AnimationCurve2;
    private static float m_AnimationCurveScalar;
    private static Gradient m_Gradient1;
    private static Gradient m_Gradient2;

    public static bool HasSingleGradient()
    {
      if (ParticleSystemClipboard.m_Gradient1 != null)
        return ParticleSystemClipboard.m_Gradient2 == null;
      return false;
    }

    public static bool HasDoubleGradient()
    {
      if (ParticleSystemClipboard.m_Gradient1 != null)
        return ParticleSystemClipboard.m_Gradient2 != null;
      return false;
    }

    public static void CopyGradient(Gradient gradient1, Gradient gradient2)
    {
      ParticleSystemClipboard.m_Gradient1 = gradient1;
      ParticleSystemClipboard.m_Gradient2 = gradient2;
    }

    public static void PasteGradient(SerializedProperty gradientProperty, SerializedProperty gradientProperty2)
    {
      if (gradientProperty != null && ParticleSystemClipboard.m_Gradient1 != null)
        gradientProperty.gradientValue = ParticleSystemClipboard.m_Gradient1;
      if (gradientProperty2 == null || ParticleSystemClipboard.m_Gradient2 == null)
        return;
      gradientProperty2.gradientValue = ParticleSystemClipboard.m_Gradient2;
    }

    public static bool HasSingleAnimationCurve()
    {
      if (ParticleSystemClipboard.m_AnimationCurve1 != null)
        return ParticleSystemClipboard.m_AnimationCurve2 == null;
      return false;
    }

    public static bool HasDoubleAnimationCurve()
    {
      if (ParticleSystemClipboard.m_AnimationCurve1 != null)
        return ParticleSystemClipboard.m_AnimationCurve2 != null;
      return false;
    }

    public static void CopyAnimationCurves(AnimationCurve animCurve, AnimationCurve animCurve2, float scalar)
    {
      ParticleSystemClipboard.m_AnimationCurve1 = animCurve;
      ParticleSystemClipboard.m_AnimationCurve2 = animCurve2;
      ParticleSystemClipboard.m_AnimationCurveScalar = scalar;
    }

    private static void ClampCurve(SerializedProperty animCurveProperty, Rect curveRanges)
    {
      AnimationCurve animationCurveValue = animCurveProperty.animationCurveValue;
      Keyframe[] keys = animationCurveValue.keys;
      for (int index = 0; index < keys.Length; ++index)
      {
        keys[index].time = Mathf.Clamp(keys[index].time, curveRanges.xMin, curveRanges.xMax);
        keys[index].value = Mathf.Clamp(keys[index].value, curveRanges.yMin, curveRanges.yMax);
      }
      animationCurveValue.keys = keys;
      animCurveProperty.animationCurveValue = animationCurveValue;
    }

    public static void PasteAnimationCurves(SerializedProperty animCurveProperty, SerializedProperty animCurveProperty2, SerializedProperty scalarProperty, Rect curveRanges, ParticleSystemCurveEditor particleSystemCurveEditor)
    {
      if (animCurveProperty != null && ParticleSystemClipboard.m_AnimationCurve1 != null)
      {
        animCurveProperty.animationCurveValue = ParticleSystemClipboard.m_AnimationCurve1;
        ParticleSystemClipboard.ClampCurve(animCurveProperty, curveRanges);
      }
      if (animCurveProperty2 != null && ParticleSystemClipboard.m_AnimationCurve2 != null)
      {
        animCurveProperty2.animationCurveValue = ParticleSystemClipboard.m_AnimationCurve2;
        ParticleSystemClipboard.ClampCurve(animCurveProperty2, curveRanges);
      }
      if (scalarProperty != null)
        scalarProperty.floatValue = ParticleSystemClipboard.m_AnimationCurveScalar;
      if (particleSystemCurveEditor == null)
        return;
      particleSystemCurveEditor.Refresh();
    }
  }
}
