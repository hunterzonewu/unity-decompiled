// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.CurveBindingUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal static class CurveBindingUtility
  {
    private static GameObject s_Root;

    public static System.Type GetEditorCurveValueType(GameObject rootGameObject, EditorCurveBinding curveBinding)
    {
      if ((UnityEngine.Object) rootGameObject != (UnityEngine.Object) null)
        return AnimationUtility.GetEditorCurveValueType(rootGameObject, curveBinding);
      return CurveBindingUtility.GetEditorCurveValueType(curveBinding);
    }

    public static object GetCurrentValue(GameObject rootGameObject, EditorCurveBinding curveBinding)
    {
      if ((UnityEngine.Object) rootGameObject != (UnityEngine.Object) null)
        return AnimationWindowUtility.GetCurrentValue(rootGameObject, curveBinding);
      return CurveBindingUtility.GetCurrentValue(curveBinding);
    }

    public static void SampleAnimationClip(GameObject rootGameObject, AnimationClip clip, float time)
    {
      if ((UnityEngine.Object) rootGameObject != (UnityEngine.Object) null)
        AnimationMode.SampleAnimationClip(rootGameObject, clip, time);
      else
        CurveBindingUtility.SampleAnimationClip(clip, time);
    }

    public static void Cleanup()
    {
      if (!((UnityEngine.Object) CurveBindingUtility.s_Root != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) CurveBindingUtility.s_Root);
      EditorUtility.UnloadUnusedAssetsImmediate();
    }

    private static System.Type GetEditorCurveValueType(EditorCurveBinding curveBinding)
    {
      CurveBindingUtility.PrepareHierarchy(curveBinding);
      return AnimationUtility.GetEditorCurveValueType(CurveBindingUtility.s_Root, curveBinding);
    }

    private static object GetCurrentValue(EditorCurveBinding curveBinding)
    {
      CurveBindingUtility.PrepareHierarchy(curveBinding);
      return AnimationWindowUtility.GetCurrentValue(CurveBindingUtility.s_Root, curveBinding);
    }

    private static void SampleAnimationClip(AnimationClip clip, float time)
    {
      CurveBindingUtility.PrepareHierarchy(clip);
      AnimationMode.SampleAnimationClip(CurveBindingUtility.s_Root, clip, time);
    }

    private static void PrepareHierarchy(AnimationClip clip)
    {
      foreach (EditorCurveBinding curveBinding in AnimationUtility.GetCurveBindings(clip))
      {
        GameObject orGetGameObject = CurveBindingUtility.CreateOrGetGameObject(curveBinding.path);
        if ((UnityEngine.Object) orGetGameObject.GetComponent(curveBinding.type) == (UnityEngine.Object) null)
          orGetGameObject.AddComponent(curveBinding.type);
      }
    }

    private static void PrepareHierarchy(EditorCurveBinding curveBinding)
    {
      GameObject orGetGameObject = CurveBindingUtility.CreateOrGetGameObject(curveBinding.path);
      if (!((UnityEngine.Object) orGetGameObject.GetComponent(curveBinding.type) == (UnityEngine.Object) null))
        return;
      orGetGameObject.AddComponent(curveBinding.type);
    }

    private static GameObject CreateOrGetGameObject(string path)
    {
      if ((UnityEngine.Object) CurveBindingUtility.s_Root == (UnityEngine.Object) null)
        CurveBindingUtility.s_Root = CurveBindingUtility.CreateNewGameObject((Transform) null, "Root");
      if (path.Length == 0)
        return CurveBindingUtility.s_Root;
      string[] strArray = path.Split('/');
      Transform parent = CurveBindingUtility.s_Root.transform;
      foreach (string name in strArray)
      {
        Transform child = parent.FindChild(name);
        parent = !((UnityEngine.Object) child == (UnityEngine.Object) null) ? child : CurveBindingUtility.CreateNewGameObject(parent, name).transform;
      }
      return parent.gameObject;
    }

    private static GameObject CreateNewGameObject(Transform parent, string name)
    {
      GameObject gameObject = new GameObject(name);
      if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
        gameObject.transform.parent = parent;
      gameObject.hideFlags = HideFlags.HideAndDontSave;
      return gameObject;
    }
  }
}
