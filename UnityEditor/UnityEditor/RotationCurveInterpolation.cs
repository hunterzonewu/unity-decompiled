// Decompiled with JetBrains decompiler
// Type: UnityEditor.RotationCurveInterpolation
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class RotationCurveInterpolation
  {
    public static char[] kPostFix = new char[4]
    {
      'x',
      'y',
      'z',
      'w'
    };

    public static RotationCurveInterpolation.Mode GetModeFromCurveData(EditorCurveBinding data)
    {
      if (AnimationWindowUtility.IsTransformType(data.type) && data.propertyName.StartsWith("localEulerAngles"))
      {
        if (data.propertyName.StartsWith("localEulerAnglesBaked"))
          return RotationCurveInterpolation.Mode.Baked;
        return data.propertyName.StartsWith("localEulerAnglesRaw") ? RotationCurveInterpolation.Mode.RawEuler : RotationCurveInterpolation.Mode.NonBaked;
      }
      return AnimationWindowUtility.IsTransformType(data.type) && data.propertyName.StartsWith("m_LocalRotation") ? RotationCurveInterpolation.Mode.RawQuaternions : RotationCurveInterpolation.Mode.Undefined;
    }

    public static RotationCurveInterpolation.State GetCurveState(AnimationClip clip, EditorCurveBinding[] selection)
    {
      RotationCurveInterpolation.State state;
      state.allAreRaw = true;
      state.allAreNonBaked = true;
      state.allAreBaked = true;
      state.allAreRotations = true;
      foreach (EditorCurveBinding data in selection)
      {
        RotationCurveInterpolation.Mode modeFromCurveData = RotationCurveInterpolation.GetModeFromCurveData(data);
        state.allAreBaked &= modeFromCurveData == RotationCurveInterpolation.Mode.Baked;
        state.allAreNonBaked &= modeFromCurveData == RotationCurveInterpolation.Mode.NonBaked;
        state.allAreRaw &= modeFromCurveData == RotationCurveInterpolation.Mode.RawEuler;
        state.allAreRotations &= modeFromCurveData != RotationCurveInterpolation.Mode.Undefined;
      }
      return state;
    }

    public static int GetCurveIndexFromName(string name)
    {
      return (int) RotationCurveInterpolation.ExtractComponentCharacter(name) - 120;
    }

    public static char ExtractComponentCharacter(string name)
    {
      return name[name.Length - 1];
    }

    public static string GetPrefixForInterpolation(RotationCurveInterpolation.Mode newInterpolationMode)
    {
      if (newInterpolationMode == RotationCurveInterpolation.Mode.Baked)
        return "localEulerAnglesBaked";
      if (newInterpolationMode == RotationCurveInterpolation.Mode.NonBaked)
        return "localEulerAngles";
      if (newInterpolationMode == RotationCurveInterpolation.Mode.RawEuler)
        return "localEulerAnglesRaw";
      if (newInterpolationMode == RotationCurveInterpolation.Mode.RawQuaternions)
        return "m_LocalRotation";
      return (string) null;
    }

    internal static EditorCurveBinding[] ConvertRotationPropertiesToDefaultInterpolation(AnimationClip clip, EditorCurveBinding[] selection)
    {
      RotationCurveInterpolation.Mode newInterpolationMode = !clip.legacy ? RotationCurveInterpolation.Mode.RawEuler : RotationCurveInterpolation.Mode.Baked;
      return RotationCurveInterpolation.ConvertRotationPropertiesToInterpolationType(selection, newInterpolationMode);
    }

    internal static EditorCurveBinding[] ConvertRotationPropertiesToInterpolationType(EditorCurveBinding[] selection, RotationCurveInterpolation.Mode newInterpolationMode)
    {
      if (selection.Length != 4 || RotationCurveInterpolation.GetModeFromCurveData(selection[0]) != RotationCurveInterpolation.Mode.RawQuaternions)
        return selection;
      EditorCurveBinding[] editorCurveBindingArray = new EditorCurveBinding[3]
      {
        selection[0],
        selection[1],
        selection[2]
      };
      string forInterpolation = RotationCurveInterpolation.GetPrefixForInterpolation(newInterpolationMode);
      editorCurveBindingArray[0].propertyName = forInterpolation + ".x";
      editorCurveBindingArray[1].propertyName = forInterpolation + ".y";
      editorCurveBindingArray[2].propertyName = forInterpolation + ".z";
      return editorCurveBindingArray;
    }

    private static EditorCurveBinding[] GenerateTransformCurveBindingArray(string path, string property, System.Type type, int count)
    {
      EditorCurveBinding[] editorCurveBindingArray = new EditorCurveBinding[count];
      for (int index = 0; index < count; ++index)
        editorCurveBindingArray[index] = EditorCurveBinding.FloatCurve(path, type, property + (object) RotationCurveInterpolation.kPostFix[index]);
      return editorCurveBindingArray;
    }

    public static EditorCurveBinding[] RemapAnimationBindingForAddKey(EditorCurveBinding binding, AnimationClip clip)
    {
      if (!AnimationWindowUtility.IsTransformType(binding.type))
        return (EditorCurveBinding[]) null;
      if (binding.propertyName.StartsWith("m_LocalPosition."))
      {
        if (binding.type == typeof (Transform))
          return RotationCurveInterpolation.GenerateTransformCurveBindingArray(binding.path, "m_LocalPosition.", binding.type, 3);
        return (EditorCurveBinding[]) null;
      }
      if (binding.propertyName.StartsWith("m_LocalScale."))
        return RotationCurveInterpolation.GenerateTransformCurveBindingArray(binding.path, "m_LocalScale.", binding.type, 3);
      if (binding.propertyName.StartsWith("m_LocalRotation"))
        return RotationCurveInterpolation.SelectRotationBindingForAddKey(binding, clip);
      return (EditorCurveBinding[]) null;
    }

    public static EditorCurveBinding[] RemapAnimationBindingForRotationAddKey(EditorCurveBinding binding, AnimationClip clip)
    {
      if (!AnimationWindowUtility.IsTransformType(binding.type))
        return (EditorCurveBinding[]) null;
      if (binding.propertyName.StartsWith("m_LocalRotation"))
        return RotationCurveInterpolation.SelectRotationBindingForAddKey(binding, clip);
      return (EditorCurveBinding[]) null;
    }

    private static EditorCurveBinding[] SelectRotationBindingForAddKey(EditorCurveBinding binding, AnimationClip clip)
    {
      EditorCurveBinding binding1 = binding;
      binding1.propertyName = "localEulerAnglesBaked.x";
      if (AnimationUtility.GetEditorCurve(clip, binding1) != null)
        return RotationCurveInterpolation.GenerateTransformCurveBindingArray(binding.path, "localEulerAnglesBaked.", binding.type, 3);
      binding1.propertyName = "localEulerAngles.x";
      if (AnimationUtility.GetEditorCurve(clip, binding1) != null)
        return RotationCurveInterpolation.GenerateTransformCurveBindingArray(binding.path, "localEulerAngles.", binding.type, 3);
      binding1.propertyName = "localEulerAnglesRaw.x";
      if (clip.legacy && AnimationUtility.GetEditorCurve(clip, binding1) == null)
        return RotationCurveInterpolation.GenerateTransformCurveBindingArray(binding.path, "localEulerAnglesBaked.", binding.type, 3);
      return RotationCurveInterpolation.GenerateTransformCurveBindingArray(binding.path, "localEulerAnglesRaw.", binding.type, 3);
    }

    public static EditorCurveBinding RemapAnimationBindingForRotationCurves(EditorCurveBinding curveBinding, AnimationClip clip)
    {
      if (!AnimationWindowUtility.IsTransformType(curveBinding.type) || !curveBinding.propertyName.StartsWith("m_LocalRotation"))
        return curveBinding;
      string str = curveBinding.propertyName.Split('.')[1];
      EditorCurveBinding binding = curveBinding;
      binding.propertyName = "localEulerAngles." + str;
      if (AnimationUtility.GetEditorCurve(clip, binding) != null)
        return binding;
      binding.propertyName = "localEulerAnglesBaked." + str;
      if (AnimationUtility.GetEditorCurve(clip, binding) != null)
        return binding;
      binding.propertyName = "localEulerAnglesRaw." + str;
      if (AnimationUtility.GetEditorCurve(clip, binding) != null)
        return binding;
      return curveBinding;
    }

    internal static void SetInterpolation(AnimationClip clip, EditorCurveBinding[] curveBindings, RotationCurveInterpolation.Mode newInterpolationMode)
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) clip, "Rotation Interpolation");
      if (clip.legacy && newInterpolationMode == RotationCurveInterpolation.Mode.RawEuler)
        Debug.LogWarning((object) "Warning, Euler Angles interpolation mode is not fully supported for Legacy animation clips. If you mix clips using Euler Angles interpolation with clips using other interpolation modes (using Animation.CrossFade, Animation.Blend or other methods), you will get erroneous results. Use with caution.", (UnityEngine.Object) clip);
      List<EditorCurveBinding> editorCurveBindingList1 = new List<EditorCurveBinding>();
      List<AnimationCurve> animationCurveList = new List<AnimationCurve>();
      List<EditorCurveBinding> editorCurveBindingList2 = new List<EditorCurveBinding>();
      foreach (EditorCurveBinding curveBinding in curveBindings)
      {
        switch (RotationCurveInterpolation.GetModeFromCurveData(curveBinding))
        {
          case RotationCurveInterpolation.Mode.Undefined:
            continue;
          case RotationCurveInterpolation.Mode.RawQuaternions:
            Debug.LogWarning((object) ("Can't convert quaternion curve: " + curveBinding.propertyName));
            continue;
          default:
            AnimationCurve editorCurve = AnimationUtility.GetEditorCurve(clip, curveBinding);
            if (editorCurve != null)
            {
              string str = RotationCurveInterpolation.GetPrefixForInterpolation(newInterpolationMode) + (object) '.' + (object) RotationCurveInterpolation.ExtractComponentCharacter(curveBinding.propertyName);
              editorCurveBindingList1.Add(new EditorCurveBinding()
              {
                propertyName = str,
                type = curveBinding.type,
                path = curveBinding.path
              });
              animationCurveList.Add(editorCurve);
              editorCurveBindingList2.Add(new EditorCurveBinding()
              {
                propertyName = curveBinding.propertyName,
                type = curveBinding.type,
                path = curveBinding.path
              });
              continue;
            }
            continue;
        }
      }
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) clip, "Rotation Interpolation");
      using (List<EditorCurveBinding>.Enumerator enumerator = editorCurveBindingList2.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          EditorCurveBinding current = enumerator.Current;
          AnimationUtility.SetEditorCurve(clip, current, (AnimationCurve) null);
        }
      }
      using (List<EditorCurveBinding>.Enumerator enumerator = editorCurveBindingList1.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          EditorCurveBinding current = enumerator.Current;
          AnimationUtility.SetEditorCurve(clip, current, animationCurveList[editorCurveBindingList1.IndexOf(current)]);
        }
      }
    }

    public struct State
    {
      public bool allAreNonBaked;
      public bool allAreBaked;
      public bool allAreRaw;
      public bool allAreRotations;
    }

    public enum Mode
    {
      Baked,
      NonBaked,
      RawQuaternions,
      RawEuler,
      Undefined,
    }
  }
}
