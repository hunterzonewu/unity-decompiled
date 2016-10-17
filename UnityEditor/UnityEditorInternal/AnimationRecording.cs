// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationRecording
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationRecording
  {
    private const string kLocalRotation = "m_LocalRotation";
    private const string kLocalEulerAnglesHint = "m_LocalEulerAnglesHint";

    private static bool HasAnyRecordableModifications(GameObject root, UndoPropertyModification[] modifications)
    {
      for (int index = 0; index < modifications.Length; ++index)
      {
        EditorCurveBinding binding;
        if ((modifications[index].currentValue == null || !(modifications[index].currentValue.target is Animator)) && AnimationUtility.PropertyModificationToEditorCurveBinding(modifications[index].previousValue, root, out binding) != null)
          return true;
      }
      return false;
    }

    private static PropertyModification FindPropertyModification(GameObject root, UndoPropertyModification[] modifications, EditorCurveBinding binding)
    {
      for (int index = 0; index < modifications.Length; ++index)
      {
        if (modifications[index].currentValue == null || !(modifications[index].currentValue.target is Animator))
        {
          EditorCurveBinding binding1;
          AnimationUtility.PropertyModificationToEditorCurveBinding(modifications[index].previousValue, root, out binding1);
          if (binding1 == binding)
            return modifications[index].previousValue;
        }
      }
      return (PropertyModification) null;
    }

    private static void CollectRotationModifications(AnimationWindowState state, ref UndoPropertyModification[] modifications, ref Dictionary<object, AnimationRecording.RotationModification> rotationModifications)
    {
      List<UndoPropertyModification> propertyModificationList = new List<UndoPropertyModification>();
      foreach (UndoPropertyModification propertyModification in modifications)
      {
        EditorCurveBinding binding = new EditorCurveBinding();
        PropertyModification previousValue = propertyModification.previousValue;
        AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, state.activeRootGameObject, out binding);
        if (binding.propertyName.StartsWith("m_LocalRotation"))
        {
          AnimationRecording.RotationModification rotationModification;
          if (!rotationModifications.TryGetValue((object) previousValue.target, out rotationModification))
          {
            rotationModification = new AnimationRecording.RotationModification();
            rotationModifications[(object) previousValue.target] = rotationModification;
          }
          if (binding.propertyName.EndsWith("x"))
            rotationModification.x = propertyModification;
          else if (binding.propertyName.EndsWith("y"))
            rotationModification.y = propertyModification;
          else if (binding.propertyName.EndsWith("z"))
            rotationModification.z = propertyModification;
          else if (binding.propertyName.EndsWith("w"))
            rotationModification.w = propertyModification;
          rotationModification.lastQuatModification = propertyModification;
        }
        else if (previousValue.propertyPath.StartsWith("m_LocalEulerAnglesHint"))
        {
          AnimationRecording.RotationModification rotationModification;
          if (!rotationModifications.TryGetValue((object) previousValue.target, out rotationModification))
          {
            rotationModification = new AnimationRecording.RotationModification();
            rotationModifications[(object) previousValue.target] = rotationModification;
          }
          if (previousValue.propertyPath.EndsWith("x"))
            rotationModification.eulerX = propertyModification;
          else if (previousValue.propertyPath.EndsWith("y"))
            rotationModification.eulerY = propertyModification;
          else if (previousValue.propertyPath.EndsWith("z"))
            rotationModification.eulerZ = propertyModification;
        }
        else
          propertyModificationList.Add(propertyModification);
      }
      if (propertyModificationList.Count <= 0)
        return;
      modifications = propertyModificationList.ToArray();
    }

    private static void ProcessRotationModifications(AnimationWindowState state, ref UndoPropertyModification[] modifications)
    {
      Dictionary<object, AnimationRecording.RotationModification> rotationModifications = new Dictionary<object, AnimationRecording.RotationModification>();
      AnimationRecording.CollectRotationModifications(state, ref modifications, ref rotationModifications);
      using (Dictionary<object, AnimationRecording.RotationModification>.Enumerator enumerator = rotationModifications.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AnimationRecording.RotationModification rotationModification = enumerator.Current.Value;
          EditorCurveBinding binding = new EditorCurveBinding();
          System.Type editorCurveBinding = AnimationUtility.PropertyModificationToEditorCurveBinding(rotationModification.lastQuatModification.currentValue, state.activeRootGameObject, out binding);
          Quaternion localRotation1 = state.activeRootGameObject.transform.localRotation;
          Quaternion localRotation2 = state.activeRootGameObject.transform.localRotation;
          object outObject1;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.x.previousValue, binding, out outObject1))
            localRotation1.x = (float) outObject1;
          object outObject2;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.y.previousValue, binding, out outObject2))
            localRotation1.y = (float) outObject2;
          object outObject3;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.z.previousValue, binding, out outObject3))
            localRotation1.z = (float) outObject3;
          object outObject4;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.w.previousValue, binding, out outObject4))
            localRotation1.w = (float) outObject4;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.x.currentValue, binding, out outObject1))
            localRotation2.x = (float) outObject1;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.y.currentValue, binding, out outObject2))
            localRotation2.y = (float) outObject2;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.z.currentValue, binding, out outObject3))
            localRotation2.z = (float) outObject3;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.w.currentValue, binding, out outObject4))
            localRotation2.w = (float) outObject4;
          Vector3 eulerAngles1 = localRotation1.eulerAngles;
          Vector3 eulerAngles2 = localRotation2.eulerAngles;
          object outObject5;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerX.previousValue, binding, out outObject5))
            eulerAngles1.x = (float) outObject5;
          object outObject6;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerY.previousValue, binding, out outObject6))
            eulerAngles1.y = (float) outObject6;
          object outObject7;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerZ.previousValue, binding, out outObject7))
            eulerAngles1.z = (float) outObject7;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerX.currentValue, binding, out outObject5))
            eulerAngles2.x = (float) outObject5;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerY.currentValue, binding, out outObject6))
            eulerAngles2.y = (float) outObject6;
          if (AnimationRecording.ValueFromPropertyModification(rotationModification.eulerZ.currentValue, binding, out outObject7))
            eulerAngles2.z = (float) outObject7;
          AnimationRecording.AddRotationKey(state, binding, editorCurveBinding, eulerAngles1, eulerAngles2);
        }
      }
    }

    public static UndoPropertyModification[] Process(AnimationWindowState state, UndoPropertyModification[] modifications)
    {
      GameObject activeRootGameObject = state.activeRootGameObject;
      if ((UnityEngine.Object) activeRootGameObject == (UnityEngine.Object) null)
        return modifications;
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      Animator component = activeRootGameObject.GetComponent<Animator>();
      if (!AnimationRecording.HasAnyRecordableModifications(activeRootGameObject, modifications))
        return modifications;
      AnimationRecording.ProcessRotationModifications(state, ref modifications);
      List<UndoPropertyModification> propertyModificationList = new List<UndoPropertyModification>();
      for (int index1 = 0; index1 < modifications.Length; ++index1)
      {
        EditorCurveBinding binding = new EditorCurveBinding();
        PropertyModification previousValue = modifications[index1].previousValue;
        System.Type editorCurveBinding = AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, activeRootGameObject, out binding);
        if (editorCurveBinding != null && editorCurveBinding != typeof (Animator))
        {
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.isHuman && (binding.type == typeof (Transform) && component.IsBoneTransform(previousValue.target as Transform)))
          {
            Debug.LogWarning((object) "Keyframing for humanoid rig is not supported!", (UnityEngine.Object) (previousValue.target as Transform));
          }
          else
          {
            AnimationMode.AddPropertyModification(binding, previousValue, modifications[index1].keepPrefabOverride);
            EditorCurveBinding[] editorCurveBindingArray = RotationCurveInterpolation.RemapAnimationBindingForAddKey(binding, activeAnimationClip);
            if (editorCurveBindingArray != null)
            {
              for (int index2 = 0; index2 < editorCurveBindingArray.Length; ++index2)
                AnimationRecording.AddKey(state, editorCurveBindingArray[index2], editorCurveBinding, AnimationRecording.FindPropertyModification(activeRootGameObject, modifications, editorCurveBindingArray[index2]));
            }
            else
              AnimationRecording.AddKey(state, binding, editorCurveBinding, previousValue);
          }
        }
        else
          propertyModificationList.Add(modifications[index1]);
      }
      return propertyModificationList.ToArray();
    }

    private static bool ValueFromPropertyModification(PropertyModification modification, EditorCurveBinding binding, out object outObject)
    {
      if (modification == null)
      {
        outObject = (object) null;
        return false;
      }
      if (binding.isPPtrCurve)
      {
        outObject = (object) modification.objectReference;
        return true;
      }
      float result;
      if (float.TryParse(modification.value, out result))
      {
        outObject = (object) result;
        return true;
      }
      outObject = (object) null;
      return false;
    }

    private static void AddKey(AnimationWindowState state, EditorCurveBinding binding, System.Type type, PropertyModification modification)
    {
      GameObject activeRootGameObject = state.activeRootGameObject;
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      if ((activeAnimationClip.hideFlags & HideFlags.NotEditable) != HideFlags.None)
        return;
      AnimationWindowCurve curve = new AnimationWindowCurve(activeAnimationClip, binding, type);
      object currentValue = CurveBindingUtility.GetCurrentValue(activeRootGameObject, binding);
      if (curve.length == 0)
      {
        object outObject = (object) null;
        if (!AnimationRecording.ValueFromPropertyModification(modification, binding, out outObject))
          outObject = currentValue;
        if (state.frame != 0)
          AnimationWindowUtility.AddKeyframeToCurve(curve, outObject, type, AnimationKeyTime.Frame(0, activeAnimationClip.frameRate));
      }
      AnimationWindowUtility.AddKeyframeToCurve(curve, currentValue, type, AnimationKeyTime.Frame(state.frame, activeAnimationClip.frameRate));
      state.SaveCurve(curve);
    }

    private static void AddRotationKey(AnimationWindowState state, EditorCurveBinding binding, System.Type type, Vector3 previousEulerAngles, Vector3 currentEulerAngles)
    {
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      if ((activeAnimationClip.hideFlags & HideFlags.NotEditable) != HideFlags.None)
        return;
      EditorCurveBinding[] editorCurveBindingArray = RotationCurveInterpolation.RemapAnimationBindingForRotationAddKey(binding, activeAnimationClip);
      for (int index = 0; index < 3; ++index)
      {
        AnimationWindowCurve curve = new AnimationWindowCurve(activeAnimationClip, editorCurveBindingArray[index], type);
        if (curve.length == 0 && state.frame != 0)
          AnimationWindowUtility.AddKeyframeToCurve(curve, (object) previousEulerAngles[index], type, AnimationKeyTime.Frame(0, activeAnimationClip.frameRate));
        AnimationWindowUtility.AddKeyframeToCurve(curve, (object) currentEulerAngles[index], type, AnimationKeyTime.Frame(state.frame, activeAnimationClip.frameRate));
        state.SaveCurve(curve);
      }
    }

    internal class RotationModification
    {
      public UndoPropertyModification x;
      public UndoPropertyModification y;
      public UndoPropertyModification z;
      public UndoPropertyModification w;
      public UndoPropertyModification lastQuatModification;
      public UndoPropertyModification eulerX;
      public UndoPropertyModification eulerY;
      public UndoPropertyModification eulerZ;
    }
  }
}
