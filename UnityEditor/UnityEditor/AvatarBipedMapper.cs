// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarBipedMapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class AvatarBipedMapper
  {
    private static string[] kBipedHumanBoneNames = new string[54]{ "Pelvis", "L Thigh", "R Thigh", "L Calf", "R Calf", "L Foot", "R Foot", "Spine", "Spine1", "Neck", "Head", "L Clavicle", "R Clavicle", "L UpperArm", "R UpperArm", "L Forearm", "R Forearm", "L Hand", "R Hand", "L Toe0", "R Toe0", string.Empty, string.Empty, string.Empty, "L Finger0", "L Finger01", "L Finger02", "L Finger1", "L Finger11", "L Finger12", "L Finger2", "L Finger21", "L Finger22", "L Finger3", "L Finger31", "L Finger32", "L Finger4", "L Finger41", "L Finger42", "R Finger0", "R Finger01", "R Finger02", "R Finger1", "R Finger11", "R Finger12", "R Finger2", "R Finger21", "R Finger22", "R Finger3", "R Finger31", "R Finger32", "R Finger4", "R Finger41", "R Finger42" };

    public static bool IsBiped(Transform root, List<string> report)
    {
      if (report != null)
        report.Clear();
      Transform[] humanToTransform = new Transform[HumanTrait.BoneCount];
      return AvatarBipedMapper.MapBipedBones(root, ref humanToTransform, report);
    }

    public static Dictionary<int, Transform> MapBones(Transform root)
    {
      Dictionary<int, Transform> dictionary = new Dictionary<int, Transform>();
      Transform[] humanToTransform = new Transform[HumanTrait.BoneCount];
      if (AvatarBipedMapper.MapBipedBones(root, ref humanToTransform, (List<string>) null))
      {
        for (int key = 0; key < HumanTrait.BoneCount; ++key)
        {
          if ((Object) humanToTransform[key] != (Object) null)
            dictionary.Add(key, humanToTransform[key]);
        }
      }
      return dictionary;
    }

    private static bool MapBipedBones(Transform root, ref Transform[] humanToTransform, List<string> report)
    {
      for (int index = 0; index < HumanTrait.BoneCount; ++index)
      {
        string bipedHumanBoneName = AvatarBipedMapper.kBipedHumanBoneNames[index];
        int parentBone1 = HumanTrait.GetParentBone(index);
        bool flag1 = HumanTrait.RequiredBone(index);
        bool flag2 = parentBone1 == -1 || HumanTrait.RequiredBone(parentBone1);
        Transform transform = parentBone1 == -1 ? root : humanToTransform[parentBone1];
        if ((Object) transform == (Object) null && !flag2)
        {
          int parentBone2 = HumanTrait.GetParentBone(parentBone1);
          transform = parentBone2 == -1 ? (Transform) null : humanToTransform[parentBone2];
        }
        if (bipedHumanBoneName != string.Empty)
        {
          humanToTransform[index] = AvatarBipedMapper.MapBipedBone(index, transform, transform, report);
          if ((Object) humanToTransform[index] == (Object) null && flag1)
            return false;
        }
      }
      return true;
    }

    private static Transform MapBipedBone(int boneIndex, Transform transform, Transform parentTransform, List<string> report)
    {
      Transform transform1 = (Transform) null;
      if ((Object) transform != (Object) null)
      {
        int childCount = transform.childCount;
        for (int index = 0; (Object) transform1 == (Object) null && index < childCount; ++index)
        {
          if (transform.GetChild(index).name.EndsWith(AvatarBipedMapper.kBipedHumanBoneNames[boneIndex]))
          {
            transform1 = transform.GetChild(index);
            if ((Object) transform1 != (Object) null && report != null && (boneIndex != 0 && (Object) transform != (Object) parentTransform))
            {
              string str1 = "- Invalid parent for " + transform1.name + ".Expected " + parentTransform.name + ", but found " + transform.name + ".";
              if (boneIndex == 1 || boneIndex == 2)
                str1 += " Disable Triangle Pelvis";
              else if (boneIndex == 11 || boneIndex == 12)
                str1 += " Enable Triangle Neck";
              else if (boneIndex == 9)
                str1 += " Preferred is two Spine Links";
              else if (boneIndex == 10)
                str1 += " Preferred is one Neck Links";
              string str2 = str1 + "\n";
              report.Add(str2);
            }
          }
        }
        for (int index = 0; (Object) transform1 == (Object) null && index < childCount; ++index)
          transform1 = AvatarBipedMapper.MapBipedBone(boneIndex, transform.GetChild(index), parentTransform, report);
      }
      return transform1;
    }

    public static void BipedPose(GameObject go)
    {
      AvatarBipedMapper.BipedPose(go.transform);
    }

    private static void BipedPose(Transform t)
    {
      if (t.name.EndsWith("Pelvis"))
      {
        t.localRotation = Quaternion.Euler(270f, 90f, 0.0f);
        t.parent.localRotation = Quaternion.Euler(270f, 90f, 0.0f);
      }
      else
        t.localRotation = !t.name.EndsWith("Thigh") ? (!t.name.EndsWith("Toe0") ? (!t.name.EndsWith("L Clavicle") ? (!t.name.EndsWith("R Clavicle") ? (!t.name.EndsWith("L Hand") ? (!t.name.EndsWith("R Hand") ? (!t.name.EndsWith("L Finger0") ? (!t.name.EndsWith("R Finger0") ? Quaternion.identity : Quaternion.Euler(0.0f, 45f, 0.0f)) : Quaternion.Euler(0.0f, 315f, 0.0f)) : Quaternion.Euler(90f, 0.0f, 0.0f)) : Quaternion.Euler(270f, 0.0f, 0.0f)) : Quaternion.Euler(0.0f, 90f, 180f)) : Quaternion.Euler(0.0f, 270f, 180f)) : Quaternion.Euler(0.0f, 0.0f, 270f)) : Quaternion.Euler(0.0f, 180f, 0.0f);
      foreach (Transform t1 in t)
        AvatarBipedMapper.BipedPose(t1);
    }
  }
}
