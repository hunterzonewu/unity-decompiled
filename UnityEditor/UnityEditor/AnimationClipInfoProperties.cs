// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationClipInfoProperties
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Animations;
using UnityEngine;

namespace UnityEditor
{
  internal class AnimationClipInfoProperties
  {
    private SerializedProperty m_Property;

    public string name
    {
      get
      {
        return this.Get("name").stringValue;
      }
      set
      {
        this.Get("name").stringValue = value;
      }
    }

    public string takeName
    {
      get
      {
        return this.Get("takeName").stringValue;
      }
      set
      {
        this.Get("takeName").stringValue = value;
      }
    }

    public float firstFrame
    {
      get
      {
        return this.Get("firstFrame").floatValue;
      }
      set
      {
        this.Get("firstFrame").floatValue = value;
      }
    }

    public float lastFrame
    {
      get
      {
        return this.Get("lastFrame").floatValue;
      }
      set
      {
        this.Get("lastFrame").floatValue = value;
      }
    }

    public int wrapMode
    {
      get
      {
        return this.Get("wrapMode").intValue;
      }
      set
      {
        this.Get("wrapMode").intValue = value;
      }
    }

    public bool loop
    {
      get
      {
        return this.Get("loop").boolValue;
      }
      set
      {
        this.Get("loop").boolValue = value;
      }
    }

    public float orientationOffsetY
    {
      get
      {
        return this.Get("orientationOffsetY").floatValue;
      }
      set
      {
        this.Get("orientationOffsetY").floatValue = value;
      }
    }

    public float level
    {
      get
      {
        return this.Get("level").floatValue;
      }
      set
      {
        this.Get("level").floatValue = value;
      }
    }

    public float cycleOffset
    {
      get
      {
        return this.Get("cycleOffset").floatValue;
      }
      set
      {
        this.Get("cycleOffset").floatValue = value;
      }
    }

    public float additiveReferencePoseFrame
    {
      get
      {
        return this.Get("additiveReferencePoseFrame").floatValue;
      }
      set
      {
        this.Get("additiveReferencePoseFrame").floatValue = value;
      }
    }

    public bool hasAdditiveReferencePose
    {
      get
      {
        return this.Get("hasAdditiveReferencePose").boolValue;
      }
      set
      {
        this.Get("hasAdditiveReferencePose").boolValue = value;
      }
    }

    public bool loopTime
    {
      get
      {
        return this.Get("loopTime").boolValue;
      }
      set
      {
        this.Get("loopTime").boolValue = value;
      }
    }

    public bool loopBlend
    {
      get
      {
        return this.Get("loopBlend").boolValue;
      }
      set
      {
        this.Get("loopBlend").boolValue = value;
      }
    }

    public bool loopBlendOrientation
    {
      get
      {
        return this.Get("loopBlendOrientation").boolValue;
      }
      set
      {
        this.Get("loopBlendOrientation").boolValue = value;
      }
    }

    public bool loopBlendPositionY
    {
      get
      {
        return this.Get("loopBlendPositionY").boolValue;
      }
      set
      {
        this.Get("loopBlendPositionY").boolValue = value;
      }
    }

    public bool loopBlendPositionXZ
    {
      get
      {
        return this.Get("loopBlendPositionXZ").boolValue;
      }
      set
      {
        this.Get("loopBlendPositionXZ").boolValue = value;
      }
    }

    public bool keepOriginalOrientation
    {
      get
      {
        return this.Get("keepOriginalOrientation").boolValue;
      }
      set
      {
        this.Get("keepOriginalOrientation").boolValue = value;
      }
    }

    public bool keepOriginalPositionY
    {
      get
      {
        return this.Get("keepOriginalPositionY").boolValue;
      }
      set
      {
        this.Get("keepOriginalPositionY").boolValue = value;
      }
    }

    public bool keepOriginalPositionXZ
    {
      get
      {
        return this.Get("keepOriginalPositionXZ").boolValue;
      }
      set
      {
        this.Get("keepOriginalPositionXZ").boolValue = value;
      }
    }

    public bool heightFromFeet
    {
      get
      {
        return this.Get("heightFromFeet").boolValue;
      }
      set
      {
        this.Get("heightFromFeet").boolValue = value;
      }
    }

    public bool mirror
    {
      get
      {
        return this.Get("mirror").boolValue;
      }
      set
      {
        this.Get("mirror").boolValue = value;
      }
    }

    public ClipAnimationMaskType maskType
    {
      get
      {
        return (ClipAnimationMaskType) this.Get("maskType").intValue;
      }
      set
      {
        this.Get("maskType").intValue = (int) value;
      }
    }

    public SerializedProperty maskTypeProperty
    {
      get
      {
        return this.Get("maskType");
      }
    }

    public AvatarMask maskSource
    {
      get
      {
        return this.Get("maskSource").objectReferenceValue as AvatarMask;
      }
      set
      {
        this.Get("maskSource").objectReferenceValue = (Object) value;
      }
    }

    public SerializedProperty maskSourceProperty
    {
      get
      {
        return this.Get("maskSource");
      }
    }

    public SerializedProperty bodyMaskProperty
    {
      get
      {
        return this.Get("bodyMask");
      }
    }

    public SerializedProperty transformMaskProperty
    {
      get
      {
        return this.Get("transformMask");
      }
    }

    public AnimationClipInfoProperties(SerializedProperty prop)
    {
      this.m_Property = prop;
    }

    private SerializedProperty Get(string property)
    {
      return this.m_Property.FindPropertyRelative(property);
    }

    public bool MaskNeedsUpdating()
    {
      AvatarMask maskSource = this.maskSource;
      if ((Object) maskSource == (Object) null)
        return false;
      SerializedProperty serializedProperty1 = this.Get("bodyMask");
      if (serializedProperty1 == null || !serializedProperty1.isArray)
        return true;
      for (AvatarMaskBodyPart index = AvatarMaskBodyPart.Root; index < AvatarMaskBodyPart.LastBodyPart; ++index)
      {
        if (maskSource.GetHumanoidBodyPartActive(index) != (serializedProperty1.GetArrayElementAtIndex((int) index).intValue != 0))
          return true;
      }
      SerializedProperty serializedProperty2 = this.Get("transformMask");
      if (serializedProperty2 == null || !serializedProperty2.isArray || serializedProperty2.arraySize > 0 && maskSource.transformCount != serializedProperty2.arraySize)
        return true;
      int arraySize = serializedProperty2.arraySize;
      for (int index = 0; index < arraySize; ++index)
      {
        SerializedProperty propertyRelative1 = serializedProperty2.GetArrayElementAtIndex(index).FindPropertyRelative("m_Path");
        SerializedProperty propertyRelative2 = serializedProperty2.GetArrayElementAtIndex(index).FindPropertyRelative("m_Weight");
        if (maskSource.GetTransformPath(index) != propertyRelative1.stringValue || maskSource.GetTransformActive(index) != (double) propertyRelative2.floatValue > 0.5)
          return true;
      }
      return false;
    }

    public void MaskFromClip(AvatarMask mask)
    {
      SerializedProperty serializedProperty1 = this.Get("bodyMask");
      if (serializedProperty1 != null && serializedProperty1.isArray)
      {
        for (AvatarMaskBodyPart index = AvatarMaskBodyPart.Root; index < AvatarMaskBodyPart.LastBodyPart; ++index)
          mask.SetHumanoidBodyPartActive(index, serializedProperty1.GetArrayElementAtIndex((int) index).intValue != 0);
      }
      SerializedProperty serializedProperty2 = this.Get("transformMask");
      if (serializedProperty2 == null || !serializedProperty2.isArray)
        return;
      if (serializedProperty2.arraySize > 0 && mask.transformCount != serializedProperty2.arraySize)
        mask.transformCount = serializedProperty2.arraySize;
      int arraySize = serializedProperty2.arraySize;
      for (int index = 0; index < arraySize; ++index)
      {
        SerializedProperty propertyRelative1 = serializedProperty2.GetArrayElementAtIndex(index).FindPropertyRelative("m_Path");
        SerializedProperty propertyRelative2 = serializedProperty2.GetArrayElementAtIndex(index).FindPropertyRelative("m_Weight");
        mask.SetTransformPath(index, propertyRelative1.stringValue);
        mask.SetTransformActive(index, (double) propertyRelative2.floatValue > 0.5);
      }
    }

    public void MaskToClip(AvatarMask mask)
    {
      SerializedProperty serializedProperty1 = this.Get("bodyMask");
      if (serializedProperty1 != null && serializedProperty1.isArray)
      {
        for (AvatarMaskBodyPart index = AvatarMaskBodyPart.Root; index < AvatarMaskBodyPart.LastBodyPart; ++index)
        {
          if (index >= (AvatarMaskBodyPart) serializedProperty1.arraySize)
            serializedProperty1.InsertArrayElementAtIndex((int) index);
          serializedProperty1.GetArrayElementAtIndex((int) index).intValue = !mask.GetHumanoidBodyPartActive(index) ? 0 : 1;
        }
      }
      SerializedProperty serializedProperty2 = this.Get("transformMask");
      ModelImporter.UpdateTransformMask(mask, serializedProperty2);
    }

    public void ClearCurves()
    {
      SerializedProperty serializedProperty = this.Get("curves");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      serializedProperty.ClearArray();
    }

    public int GetCurveCount()
    {
      int num = 0;
      SerializedProperty serializedProperty = this.Get("curves");
      if (serializedProperty != null && serializedProperty.isArray)
        num = serializedProperty.arraySize;
      return num;
    }

    public SerializedProperty GetCurveProperty(int index)
    {
      SerializedProperty serializedProperty1 = (SerializedProperty) null;
      SerializedProperty serializedProperty2 = this.Get("curves");
      if (serializedProperty2 != null && serializedProperty2.isArray)
        serializedProperty1 = serializedProperty2.GetArrayElementAtIndex(index).FindPropertyRelative("curve");
      return serializedProperty1;
    }

    public string GetCurveName(int index)
    {
      string str = string.Empty;
      SerializedProperty serializedProperty = this.Get("curves");
      if (serializedProperty != null && serializedProperty.isArray)
        str = serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("name").stringValue;
      return str;
    }

    public void SetCurveName(int index, string name)
    {
      SerializedProperty serializedProperty = this.Get("curves");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("name").stringValue = name;
    }

    public AnimationCurve GetCurve(int index)
    {
      AnimationCurve animationCurve = (AnimationCurve) null;
      SerializedProperty curveProperty = this.GetCurveProperty(index);
      if (curveProperty != null)
        animationCurve = curveProperty.animationCurveValue;
      return animationCurve;
    }

    public void SetCurve(int index, AnimationCurve curveValue)
    {
      SerializedProperty curveProperty = this.GetCurveProperty(index);
      if (curveProperty == null)
        return;
      curveProperty.animationCurveValue = curveValue;
    }

    public void AddCurve()
    {
      SerializedProperty serializedProperty = this.Get("curves");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
      serializedProperty.GetArrayElementAtIndex(serializedProperty.arraySize - 1).FindPropertyRelative("name").stringValue = "Curve";
      serializedProperty.GetArrayElementAtIndex(serializedProperty.arraySize - 1).FindPropertyRelative("curve").animationCurveValue = new AnimationCurve(new Keyframe[2]
      {
        new Keyframe(0.0f, 0.0f),
        new Keyframe(1f, 0.0f)
      })
      {
        preWrapMode = WrapMode.Default,
        postWrapMode = WrapMode.Default
      };
    }

    public void RemoveCurve(int index)
    {
      SerializedProperty serializedProperty = this.Get("curves");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      serializedProperty.DeleteArrayElementAtIndex(index);
    }

    public AnimationEvent GetEvent(int index)
    {
      AnimationEvent animationEvent = new AnimationEvent();
      SerializedProperty serializedProperty = this.Get("events");
      if (serializedProperty != null && serializedProperty.isArray)
      {
        if (index < serializedProperty.arraySize)
        {
          animationEvent.floatParameter = serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("floatParameter").floatValue;
          animationEvent.functionName = serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("functionName").stringValue;
          animationEvent.intParameter = serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("intParameter").intValue;
          animationEvent.objectReferenceParameter = serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("objectReferenceParameter").objectReferenceValue;
          animationEvent.stringParameter = serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("data").stringValue;
          animationEvent.time = serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("time").floatValue;
        }
        else
          Debug.LogWarning((object) "Invalid Event Index");
      }
      return animationEvent;
    }

    public void SetEvent(int index, AnimationEvent animationEvent)
    {
      SerializedProperty serializedProperty = this.Get("events");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      if (index < serializedProperty.arraySize)
      {
        serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("floatParameter").floatValue = animationEvent.floatParameter;
        serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("functionName").stringValue = animationEvent.functionName;
        serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("intParameter").intValue = animationEvent.intParameter;
        serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("objectReferenceParameter").objectReferenceValue = animationEvent.objectReferenceParameter;
        serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("data").stringValue = animationEvent.stringParameter;
        serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("time").floatValue = animationEvent.time;
      }
      else
        Debug.LogWarning((object) "Invalid Event Index");
    }

    public void ClearEvents()
    {
      SerializedProperty serializedProperty = this.Get("events");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      serializedProperty.ClearArray();
    }

    public int GetEventCount()
    {
      int num = 0;
      SerializedProperty serializedProperty = this.Get("events");
      if (serializedProperty != null && serializedProperty.isArray)
        num = serializedProperty.arraySize;
      return num;
    }

    public void AddEvent(float time)
    {
      SerializedProperty serializedProperty = this.Get("events");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
      serializedProperty.GetArrayElementAtIndex(serializedProperty.arraySize - 1).FindPropertyRelative("functionName").stringValue = "NewEvent";
      serializedProperty.GetArrayElementAtIndex(serializedProperty.arraySize - 1).FindPropertyRelative("time").floatValue = time;
    }

    public void RemoveEvent(int index)
    {
      SerializedProperty serializedProperty = this.Get("events");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      serializedProperty.DeleteArrayElementAtIndex(index);
    }

    public void SetEvents(AnimationEvent[] newEvents)
    {
      SerializedProperty serializedProperty = this.Get("events");
      if (serializedProperty == null || !serializedProperty.isArray)
        return;
      serializedProperty.ClearArray();
      foreach (AnimationEvent newEvent in newEvents)
      {
        serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
        this.SetEvent(serializedProperty.arraySize - 1, newEvent);
      }
    }

    public AnimationEvent[] GetEvents()
    {
      AnimationEvent[] animationEventArray = new AnimationEvent[this.GetEventCount()];
      SerializedProperty serializedProperty = this.Get("events");
      if (serializedProperty != null && serializedProperty.isArray)
      {
        for (int index = 0; index < this.GetEventCount(); ++index)
          animationEventArray[index] = this.GetEvent(index);
      }
      return animationEventArray;
    }

    public void AssignToPreviewClip(AnimationClip clip)
    {
      AnimationUtility.SetAnimationClipSettingsNoDirty(clip, new AnimationClipSettings()
      {
        startTime = this.firstFrame / clip.frameRate,
        stopTime = this.lastFrame / clip.frameRate,
        orientationOffsetY = this.orientationOffsetY,
        level = this.level,
        cycleOffset = this.cycleOffset,
        loopTime = this.loopTime,
        loopBlend = this.loopBlend,
        loopBlendOrientation = this.loopBlendOrientation,
        loopBlendPositionY = this.loopBlendPositionY,
        loopBlendPositionXZ = this.loopBlendPositionXZ,
        keepOriginalOrientation = this.keepOriginalOrientation,
        keepOriginalPositionY = this.keepOriginalPositionY,
        keepOriginalPositionXZ = this.keepOriginalPositionXZ,
        heightFromFeet = this.heightFromFeet,
        mirror = this.mirror,
        hasAdditiveReferencePose = this.hasAdditiveReferencePose,
        additiveReferencePoseTime = this.additiveReferencePoseFrame / clip.frameRate
      });
    }

    private float FixPrecisionErrors(float f)
    {
      float num = Mathf.Round(f);
      if ((double) Mathf.Abs(f - num) < 9.99999974737875E-05)
        return num;
      return f;
    }

    public void ExtractFromPreviewClip(AnimationClip clip)
    {
      AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(clip);
      if ((double) this.firstFrame / (double) clip.frameRate != (double) animationClipSettings.startTime)
        this.firstFrame = this.FixPrecisionErrors(animationClipSettings.startTime * clip.frameRate);
      if ((double) this.lastFrame / (double) clip.frameRate != (double) animationClipSettings.stopTime)
        this.lastFrame = this.FixPrecisionErrors(animationClipSettings.stopTime * clip.frameRate);
      this.orientationOffsetY = animationClipSettings.orientationOffsetY;
      this.level = animationClipSettings.level;
      this.cycleOffset = animationClipSettings.cycleOffset;
      this.loopTime = animationClipSettings.loopTime;
      this.loopBlend = animationClipSettings.loopBlend;
      this.loopBlendOrientation = animationClipSettings.loopBlendOrientation;
      this.loopBlendPositionY = animationClipSettings.loopBlendPositionY;
      this.loopBlendPositionXZ = animationClipSettings.loopBlendPositionXZ;
      this.keepOriginalOrientation = animationClipSettings.keepOriginalOrientation;
      this.keepOriginalPositionY = animationClipSettings.keepOriginalPositionY;
      this.keepOriginalPositionXZ = animationClipSettings.keepOriginalPositionXZ;
      this.heightFromFeet = animationClipSettings.heightFromFeet;
      this.mirror = animationClipSettings.mirror;
      this.hasAdditiveReferencePose = animationClipSettings.hasAdditiveReferencePose;
      if ((double) this.additiveReferencePoseFrame / (double) clip.frameRate == (double) animationClipSettings.additiveReferencePoseTime)
        return;
      this.additiveReferencePoseFrame = this.FixPrecisionErrors(animationClipSettings.additiveReferencePoseTime * clip.frameRate);
    }
  }
}
