// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveRendererCache
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections;
using UnityEngine;

namespace UnityEditor
{
  internal class CurveRendererCache
  {
    private static Hashtable m_CombiRenderers = new Hashtable();
    private static Hashtable m_NormalRenderers = new Hashtable();

    public static void ClearCurveRendererCache()
    {
      CurveRendererCache.m_CombiRenderers = new Hashtable();
      CurveRendererCache.m_NormalRenderers = new Hashtable();
    }

    public static CurveRenderer GetCurveRenderer(AnimationClip clip, EditorCurveBinding curveBinding)
    {
      if (curveBinding.type == typeof (Transform) && curveBinding.propertyName.StartsWith("localEulerAngles."))
      {
        int curveIndexFromName = RotationCurveInterpolation.GetCurveIndexFromName(curveBinding.propertyName);
        string str = CurveUtility.GetCurveGroupID(clip, curveBinding).ToString();
        EulerCurveCombinedRenderer renderer = (EulerCurveCombinedRenderer) CurveRendererCache.m_CombiRenderers[(object) str];
        if (renderer == null)
        {
          renderer = new EulerCurveCombinedRenderer(AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(curveBinding.path, typeof (Transform), "m_LocalRotation.x")), AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(curveBinding.path, typeof (Transform), "m_LocalRotation.y")), AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(curveBinding.path, typeof (Transform), "m_LocalRotation.z")), AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(curveBinding.path, typeof (Transform), "m_LocalRotation.w")), AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(curveBinding.path, typeof (Transform), "localEulerAngles.x")), AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(curveBinding.path, typeof (Transform), "localEulerAngles.y")), AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(curveBinding.path, typeof (Transform), "localEulerAngles.z")));
          CurveRendererCache.m_CombiRenderers.Add((object) str, (object) renderer);
        }
        return (CurveRenderer) new EulerCurveRenderer(curveIndexFromName, renderer);
      }
      string str1 = CurveUtility.GetCurveID(clip, curveBinding).ToString();
      NormalCurveRenderer normalCurveRenderer = (NormalCurveRenderer) CurveRendererCache.m_NormalRenderers[(object) str1];
      if (normalCurveRenderer == null)
      {
        normalCurveRenderer = new NormalCurveRenderer(AnimationUtility.GetEditorCurve(clip, curveBinding));
        CurveRendererCache.m_NormalRenderers.Add((object) str1, (object) normalCurveRenderer);
      }
      return (CurveRenderer) normalCurveRenderer;
    }
  }
}
