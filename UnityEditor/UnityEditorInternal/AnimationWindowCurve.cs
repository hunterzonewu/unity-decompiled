// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowCurve
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AnimationWindowCurve : IComparable<AnimationWindowCurve>
  {
    public const float timeEpsilon = 1E-05f;
    public List<AnimationWindowKeyframe> m_Keyframes;
    public System.Type m_ValueType;
    private EditorCurveBinding m_Binding;

    public EditorCurveBinding binding
    {
      get
      {
        return this.m_Binding;
      }
    }

    public bool isPPtrCurve
    {
      get
      {
        return this.m_Binding.isPPtrCurve;
      }
    }

    public string propertyName
    {
      get
      {
        return this.m_Binding.propertyName;
      }
    }

    public string path
    {
      get
      {
        return this.m_Binding.path;
      }
    }

    public System.Type type
    {
      get
      {
        return this.m_Binding.type;
      }
    }

    public int length
    {
      get
      {
        return this.m_Keyframes.Count;
      }
    }

    public int depth
    {
      get
      {
        if (this.path.Length <= 0)
          return 0;
        return this.path.Split('/').Length;
      }
    }

    public AnimationWindowCurve(AnimationClip clip, EditorCurveBinding binding, System.Type valueType)
    {
      binding = RotationCurveInterpolation.RemapAnimationBindingForRotationCurves(binding, clip);
      this.m_Binding = binding;
      this.m_ValueType = valueType;
      this.LoadKeyframes(clip);
    }

    public void LoadKeyframes(AnimationClip clip)
    {
      this.m_Keyframes = new List<AnimationWindowKeyframe>();
      if (!this.m_Binding.isPPtrCurve)
      {
        AnimationCurve editorCurve = AnimationUtility.GetEditorCurve(clip, this.binding);
        for (int index = 0; editorCurve != null && index < editorCurve.length; ++index)
          this.m_Keyframes.Add(new AnimationWindowKeyframe(this, editorCurve[index]));
      }
      else
      {
        ObjectReferenceKeyframe[] objectReferenceCurve = AnimationUtility.GetObjectReferenceCurve(clip, this.binding);
        for (int index = 0; objectReferenceCurve != null && index < objectReferenceCurve.Length; ++index)
          this.m_Keyframes.Add(new AnimationWindowKeyframe(this, objectReferenceCurve[index]));
      }
    }

    public override int GetHashCode()
    {
      return this.m_Binding.GetHashCode();
    }

    public int CompareTo(AnimationWindowCurve obj)
    {
      bool flag1 = this.path.Equals(obj.path);
      if (!flag1 && this.depth != obj.depth)
        return this.depth < obj.depth ? -1 : 1;
      bool flag2 = this.type == typeof (Transform) && obj.type == typeof (Transform) && flag1;
      bool flag3 = (this.type == typeof (Transform) || obj.type == typeof (Transform)) && flag1;
      if (flag2)
      {
        string groupDisplayName1 = AnimationWindowUtility.GetNicePropertyGroupDisplayName(typeof (Transform), AnimationWindowUtility.GetPropertyGroupName(this.propertyName));
        string groupDisplayName2 = AnimationWindowUtility.GetNicePropertyGroupDisplayName(typeof (Transform), AnimationWindowUtility.GetPropertyGroupName(obj.propertyName));
        if (groupDisplayName1.Contains("Position") && groupDisplayName2.Contains("Rotation"))
          return -1;
        if (groupDisplayName1.Contains("Rotation") && groupDisplayName2.Contains("Position"))
          return 1;
      }
      else
      {
        if (flag3)
          return this.type == typeof (Transform) ? -1 : 1;
        if (this.path == obj.path && obj.type == this.type)
        {
          int componentIndex1 = AnimationWindowUtility.GetComponentIndex(obj.propertyName);
          int componentIndex2 = AnimationWindowUtility.GetComponentIndex(this.propertyName);
          if (componentIndex1 != -1 && componentIndex2 != -1 && this.propertyName.Substring(0, this.propertyName.Length - 2) == obj.propertyName.Substring(0, obj.propertyName.Length - 2))
            return componentIndex2 - componentIndex1;
        }
      }
      return (this.path + (object) this.type + this.propertyName).CompareTo(obj.path + (object) obj.type + obj.propertyName);
    }

    public AnimationCurve ToAnimationCurve()
    {
      int count = this.m_Keyframes.Count;
      AnimationCurve animationCurve = new AnimationCurve();
      List<Keyframe> keyframeList = new List<Keyframe>();
      float num = float.MinValue;
      for (int index = 0; index < count; ++index)
      {
        if ((double) Mathf.Abs(this.m_Keyframes[index].time - num) > 9.99999974737875E-06)
        {
          keyframeList.Add(new Keyframe(this.m_Keyframes[index].time, (float) this.m_Keyframes[index].value, this.m_Keyframes[index].m_InTangent, this.m_Keyframes[index].m_OutTangent)
          {
            tangentMode = this.m_Keyframes[index].m_TangentMode
          });
          num = this.m_Keyframes[index].time;
        }
      }
      animationCurve.keys = keyframeList.ToArray();
      return animationCurve;
    }

    public ObjectReferenceKeyframe[] ToObjectCurve()
    {
      int count = this.m_Keyframes.Count;
      List<ObjectReferenceKeyframe> referenceKeyframeList = new List<ObjectReferenceKeyframe>();
      float num = float.MinValue;
      for (int index = 0; index < count; ++index)
      {
        if ((double) Mathf.Abs(this.m_Keyframes[index].time - num) > 9.99999974737875E-06)
        {
          ObjectReferenceKeyframe referenceKeyframe = new ObjectReferenceKeyframe();
          referenceKeyframe.time = this.m_Keyframes[index].time;
          referenceKeyframe.value = (UnityEngine.Object) this.m_Keyframes[index].value;
          num = referenceKeyframe.time;
          referenceKeyframeList.Add(referenceKeyframe);
        }
      }
      return referenceKeyframeList.ToArray();
    }

    public AnimationWindowKeyframe FindKeyAtTime(AnimationKeyTime keyTime)
    {
      int keyframeIndex = this.GetKeyframeIndex(keyTime);
      if (keyframeIndex == -1)
        return (AnimationWindowKeyframe) null;
      return this.m_Keyframes[keyframeIndex];
    }

    public void AddKeyframe(AnimationWindowKeyframe key, AnimationKeyTime keyTime)
    {
      this.RemoveKeyframe(keyTime);
      this.m_Keyframes.Add(key);
      this.m_Keyframes.Sort((Comparison<AnimationWindowKeyframe>) ((a, b) => a.time.CompareTo(b.time)));
    }

    public void RemoveKeyframe(AnimationKeyTime time)
    {
      for (int index = this.m_Keyframes.Count - 1; index >= 0; --index)
      {
        if (time.ContainsTime(this.m_Keyframes[index].time))
          this.m_Keyframes.RemoveAt(index);
      }
    }

    public bool HasKeyframe(AnimationKeyTime time)
    {
      return this.GetKeyframeIndex(time) != -1;
    }

    public int GetKeyframeIndex(AnimationKeyTime time)
    {
      for (int index = 0; index < this.m_Keyframes.Count; ++index)
      {
        if (time.ContainsTime(this.m_Keyframes[index].time))
          return index;
      }
      return -1;
    }

    public void RemoveKeysAtRange(float startTime, float endTime)
    {
      for (int index = this.m_Keyframes.Count - 1; index >= 0; --index)
      {
        if (Mathf.Approximately(endTime, this.m_Keyframes[index].time) || (double) this.m_Keyframes[index].time > (double) startTime && (double) this.m_Keyframes[index].time < (double) endTime)
          this.m_Keyframes.RemoveAt(index);
      }
    }
  }
}
