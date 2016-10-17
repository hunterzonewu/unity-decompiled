// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarSetupTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal static class AvatarSetupTool
  {
    private static string sHuman = "m_HumanDescription.m_Human";
    private static string sHasTranslationDoF = "m_HumanDescription.m_HasTranslationDoF";
    internal static string sSkeleton = "m_HumanDescription.m_Skeleton";
    internal static string sName = "m_Name";
    internal static string sPosition = "m_Position";
    internal static string sRotation = "m_Rotation";
    internal static string sScale = "m_Scale";
    internal static string sTransformModified = "m_TransformModified";
    internal static int[] sHumanParent = new int[54]{ 0, 0, 0, 1, 2, 3, 4, 0, 7, 8, 9, 8, 8, 11, 12, 13, 14, 15, 16, 5, 6, 10, 10, 10, 17, 24, 25, 17, 27, 28, 17, 30, 31, 17, 33, 34, 17, 36, 37, 18, 39, 40, 18, 42, 43, 18, 45, 46, 18, 48, 49, 18, 51, 52 };
    private static AvatarSetupTool.BonePoseData[] sBonePoses;

    static AvatarSetupTool()
    {
      AvatarSetupTool.BonePoseData[] bonePoseDataArray = new AvatarSetupTool.BonePoseData[54];
      bonePoseDataArray[0] = new AvatarSetupTool.BonePoseData(Vector3.up, true, 15f);
      bonePoseDataArray[1] = new AvatarSetupTool.BonePoseData(new Vector3(-0.05f, -1f, 0.0f), true, 15f);
      bonePoseDataArray[2] = new AvatarSetupTool.BonePoseData(new Vector3(0.05f, -1f, 0.0f), true, 15f);
      bonePoseDataArray[3] = new AvatarSetupTool.BonePoseData(new Vector3(-0.05f, -1f, -0.15f), true, 20f);
      bonePoseDataArray[4] = new AvatarSetupTool.BonePoseData(new Vector3(0.05f, -1f, -0.15f), true, 20f);
      bonePoseDataArray[5] = new AvatarSetupTool.BonePoseData(new Vector3(-0.05f, 0.0f, 1f), true, 20f, Vector3.up, (int[]) null);
      bonePoseDataArray[6] = new AvatarSetupTool.BonePoseData(new Vector3(0.05f, 0.0f, 1f), true, 20f, Vector3.up, (int[]) null);
      int index1 = 7;
      AvatarSetupTool.BonePoseData bonePoseData1 = new AvatarSetupTool.BonePoseData(Vector3.up, 1 != 0, 30f, new int[3]{ 8, 9, 10 });
      bonePoseDataArray[index1] = bonePoseData1;
      int index2 = 8;
      AvatarSetupTool.BonePoseData bonePoseData2 = new AvatarSetupTool.BonePoseData(Vector3.up, 1 != 0, 30f, new int[2]{ 9, 10 });
      bonePoseDataArray[index2] = bonePoseData2;
      int index3 = 9;
      AvatarSetupTool.BonePoseData bonePoseData3 = new AvatarSetupTool.BonePoseData(Vector3.up, true, 30f);
      bonePoseDataArray[index3] = bonePoseData3;
      int index4 = 11;
      AvatarSetupTool.BonePoseData bonePoseData4 = new AvatarSetupTool.BonePoseData(-Vector3.right, true, 20f);
      bonePoseDataArray[index4] = bonePoseData4;
      int index5 = 12;
      AvatarSetupTool.BonePoseData bonePoseData5 = new AvatarSetupTool.BonePoseData(Vector3.right, true, 20f);
      bonePoseDataArray[index5] = bonePoseData5;
      int index6 = 13;
      AvatarSetupTool.BonePoseData bonePoseData6 = new AvatarSetupTool.BonePoseData(-Vector3.right, true, 5f);
      bonePoseDataArray[index6] = bonePoseData6;
      int index7 = 14;
      AvatarSetupTool.BonePoseData bonePoseData7 = new AvatarSetupTool.BonePoseData(Vector3.right, true, 5f);
      bonePoseDataArray[index7] = bonePoseData7;
      int index8 = 15;
      AvatarSetupTool.BonePoseData bonePoseData8 = new AvatarSetupTool.BonePoseData(-Vector3.right, true, 5f);
      bonePoseDataArray[index8] = bonePoseData8;
      int index9 = 16;
      AvatarSetupTool.BonePoseData bonePoseData9 = new AvatarSetupTool.BonePoseData(Vector3.right, true, 5f);
      bonePoseDataArray[index9] = bonePoseData9;
      int index10 = 17;
      AvatarSetupTool.BonePoseData bonePoseData10 = new AvatarSetupTool.BonePoseData(-Vector3.right, 0 != 0, 10f, Vector3.forward, new int[1]{ 30 });
      bonePoseDataArray[index10] = bonePoseData10;
      int index11 = 18;
      AvatarSetupTool.BonePoseData bonePoseData11 = new AvatarSetupTool.BonePoseData(Vector3.right, 0 != 0, 10f, Vector3.forward, new int[1]{ 45 });
      bonePoseDataArray[index11] = bonePoseData11;
      int index12 = 24;
      AvatarSetupTool.BonePoseData bonePoseData12 = new AvatarSetupTool.BonePoseData(new Vector3(-1f, 0.0f, 1f), false, 10f);
      bonePoseDataArray[index12] = bonePoseData12;
      int index13 = 25;
      AvatarSetupTool.BonePoseData bonePoseData13 = new AvatarSetupTool.BonePoseData(new Vector3(-1f, 0.0f, 1f), false, 5f);
      bonePoseDataArray[index13] = bonePoseData13;
      int index14 = 26;
      AvatarSetupTool.BonePoseData bonePoseData14 = new AvatarSetupTool.BonePoseData(new Vector3(-1f, 0.0f, 1f), false, 5f);
      bonePoseDataArray[index14] = bonePoseData14;
      int index15 = 27;
      AvatarSetupTool.BonePoseData bonePoseData15 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 10f);
      bonePoseDataArray[index15] = bonePoseData15;
      int index16 = 28;
      AvatarSetupTool.BonePoseData bonePoseData16 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 5f);
      bonePoseDataArray[index16] = bonePoseData16;
      int index17 = 29;
      AvatarSetupTool.BonePoseData bonePoseData17 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 5f);
      bonePoseDataArray[index17] = bonePoseData17;
      int index18 = 30;
      AvatarSetupTool.BonePoseData bonePoseData18 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 10f);
      bonePoseDataArray[index18] = bonePoseData18;
      int index19 = 31;
      AvatarSetupTool.BonePoseData bonePoseData19 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 5f);
      bonePoseDataArray[index19] = bonePoseData19;
      int index20 = 32;
      AvatarSetupTool.BonePoseData bonePoseData20 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 5f);
      bonePoseDataArray[index20] = bonePoseData20;
      int index21 = 33;
      AvatarSetupTool.BonePoseData bonePoseData21 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 10f);
      bonePoseDataArray[index21] = bonePoseData21;
      int index22 = 34;
      AvatarSetupTool.BonePoseData bonePoseData22 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 5f);
      bonePoseDataArray[index22] = bonePoseData22;
      int index23 = 35;
      AvatarSetupTool.BonePoseData bonePoseData23 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 5f);
      bonePoseDataArray[index23] = bonePoseData23;
      int index24 = 36;
      AvatarSetupTool.BonePoseData bonePoseData24 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 10f);
      bonePoseDataArray[index24] = bonePoseData24;
      int index25 = 37;
      AvatarSetupTool.BonePoseData bonePoseData25 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 5f);
      bonePoseDataArray[index25] = bonePoseData25;
      int index26 = 38;
      AvatarSetupTool.BonePoseData bonePoseData26 = new AvatarSetupTool.BonePoseData(-Vector3.right, false, 5f);
      bonePoseDataArray[index26] = bonePoseData26;
      int index27 = 39;
      AvatarSetupTool.BonePoseData bonePoseData27 = new AvatarSetupTool.BonePoseData(new Vector3(1f, 0.0f, 1f), false, 10f);
      bonePoseDataArray[index27] = bonePoseData27;
      int index28 = 40;
      AvatarSetupTool.BonePoseData bonePoseData28 = new AvatarSetupTool.BonePoseData(new Vector3(1f, 0.0f, 1f), false, 5f);
      bonePoseDataArray[index28] = bonePoseData28;
      int index29 = 41;
      AvatarSetupTool.BonePoseData bonePoseData29 = new AvatarSetupTool.BonePoseData(new Vector3(1f, 0.0f, 1f), false, 5f);
      bonePoseDataArray[index29] = bonePoseData29;
      int index30 = 42;
      AvatarSetupTool.BonePoseData bonePoseData30 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 10f);
      bonePoseDataArray[index30] = bonePoseData30;
      int index31 = 43;
      AvatarSetupTool.BonePoseData bonePoseData31 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 5f);
      bonePoseDataArray[index31] = bonePoseData31;
      int index32 = 44;
      AvatarSetupTool.BonePoseData bonePoseData32 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 5f);
      bonePoseDataArray[index32] = bonePoseData32;
      int index33 = 45;
      AvatarSetupTool.BonePoseData bonePoseData33 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 10f);
      bonePoseDataArray[index33] = bonePoseData33;
      int index34 = 46;
      AvatarSetupTool.BonePoseData bonePoseData34 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 5f);
      bonePoseDataArray[index34] = bonePoseData34;
      int index35 = 47;
      AvatarSetupTool.BonePoseData bonePoseData35 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 5f);
      bonePoseDataArray[index35] = bonePoseData35;
      int index36 = 48;
      AvatarSetupTool.BonePoseData bonePoseData36 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 10f);
      bonePoseDataArray[index36] = bonePoseData36;
      int index37 = 49;
      AvatarSetupTool.BonePoseData bonePoseData37 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 5f);
      bonePoseDataArray[index37] = bonePoseData37;
      int index38 = 50;
      AvatarSetupTool.BonePoseData bonePoseData38 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 5f);
      bonePoseDataArray[index38] = bonePoseData38;
      int index39 = 51;
      AvatarSetupTool.BonePoseData bonePoseData39 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 10f);
      bonePoseDataArray[index39] = bonePoseData39;
      int index40 = 52;
      AvatarSetupTool.BonePoseData bonePoseData40 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 5f);
      bonePoseDataArray[index40] = bonePoseData40;
      int index41 = 53;
      AvatarSetupTool.BonePoseData bonePoseData41 = new AvatarSetupTool.BonePoseData(Vector3.right, false, 5f);
      bonePoseDataArray[index41] = bonePoseData41;
      AvatarSetupTool.sBonePoses = bonePoseDataArray;
    }

    public static Dictionary<Transform, bool> GetModelBones(Transform root, bool includeAll, AvatarSetupTool.BoneWrapper[] humanBones)
    {
      if ((UnityEngine.Object) root == (UnityEngine.Object) null)
        return (Dictionary<Transform, bool>) null;
      Dictionary<Transform, bool> bones1 = new Dictionary<Transform, bool>();
      List<Transform> skinnedBones = new List<Transform>();
      if (!includeAll)
      {
        foreach (SkinnedMeshRenderer componentsInChild in root.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
          Transform[] bones2 = componentsInChild.bones;
          bool[] flagArray = new bool[bones2.Length];
          foreach (BoneWeight boneWeight in componentsInChild.sharedMesh.boneWeights)
          {
            if ((double) boneWeight.weight0 != 0.0)
              flagArray[boneWeight.boneIndex0] = true;
            if ((double) boneWeight.weight1 != 0.0)
              flagArray[boneWeight.boneIndex1] = true;
            if ((double) boneWeight.weight2 != 0.0)
              flagArray[boneWeight.boneIndex2] = true;
            if ((double) boneWeight.weight3 != 0.0)
              flagArray[boneWeight.boneIndex3] = true;
          }
          for (int index = 0; index < bones2.Length; ++index)
          {
            if (flagArray[index] && !skinnedBones.Contains(bones2[index]))
              skinnedBones.Add(bones2[index]);
          }
        }
        AvatarSetupTool.DetermineIsActualBone(root, bones1, skinnedBones, false, humanBones);
      }
      if (bones1.Count < HumanTrait.RequiredBoneCount)
      {
        bones1.Clear();
        skinnedBones.Clear();
        AvatarSetupTool.DetermineIsActualBone(root, bones1, skinnedBones, true, humanBones);
      }
      return bones1;
    }

    private static bool DetermineIsActualBone(Transform tr, Dictionary<Transform, bool> bones, List<Transform> skinnedBones, bool includeAll, AvatarSetupTool.BoneWrapper[] humanBones)
    {
      bool flag1 = includeAll;
      bool flag2 = false;
      bool flag3 = false;
      int num = 0;
      foreach (Transform tr1 in tr)
      {
        if (AvatarSetupTool.DetermineIsActualBone(tr1, bones, skinnedBones, includeAll, humanBones))
          ++num;
      }
      if (num > 0)
        flag2 = true;
      if (num > 1)
        flag1 = true;
      if (!flag1 && skinnedBones.Contains(tr))
        flag1 = true;
      if (!flag1)
      {
        Component[] components = tr.GetComponents<Component>();
        if (components.Length > 1)
        {
          foreach (Component component in components)
          {
            if (component is Renderer && !(component is SkinnedMeshRenderer))
            {
              Bounds bounds = (component as Renderer).bounds;
              bounds.extents = bounds.size;
              if (tr.childCount == 0 && (bool) ((UnityEngine.Object) tr.parent) && bounds.Contains(tr.parent.position))
              {
                if ((UnityEngine.Object) tr.parent.GetComponent<Renderer>() != (UnityEngine.Object) null)
                  flag1 = true;
                else
                  flag3 = true;
              }
              else if (bounds.Contains(tr.position))
                flag1 = true;
            }
          }
        }
      }
      if (!flag1 && humanBones != null)
      {
        foreach (AvatarSetupTool.BoneWrapper humanBone in humanBones)
        {
          if ((UnityEngine.Object) tr == (UnityEngine.Object) humanBone.bone)
          {
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
        bones[tr] = true;
      else if (flag2)
      {
        if (!bones.ContainsKey(tr))
          bones[tr] = false;
      }
      else if (flag3)
        bones[tr.parent] = true;
      return bones.ContainsKey(tr);
    }

    public static int GetFirstHumanBoneAncestor(AvatarSetupTool.BoneWrapper[] bones, int boneIndex)
    {
      boneIndex = AvatarSetupTool.sHumanParent[boneIndex];
      while ((UnityEngine.Object) bones[boneIndex].bone == (UnityEngine.Object) null && boneIndex != 0)
        boneIndex = AvatarSetupTool.sHumanParent[boneIndex];
      return boneIndex;
    }

    public static int GetHumanBoneChild(AvatarSetupTool.BoneWrapper[] bones, int boneIndex)
    {
      for (int index = 0; index < AvatarSetupTool.sHumanParent.Length; ++index)
      {
        if (AvatarSetupTool.sHumanParent[index] == boneIndex)
          return index;
      }
      return -1;
    }

    public static AvatarSetupTool.BoneWrapper[] GetHumanBones(SerializedObject serializedObject, Dictionary<Transform, bool> actualBones)
    {
      string[] boneName = HumanTrait.BoneName;
      AvatarSetupTool.BoneWrapper[] boneWrapperArray = new AvatarSetupTool.BoneWrapper[boneName.Length];
      for (int index = 0; index < boneName.Length; ++index)
        boneWrapperArray[index] = new AvatarSetupTool.BoneWrapper(boneName[index], serializedObject, actualBones);
      return boneWrapperArray;
    }

    public static void ClearAll(SerializedObject serializedObject)
    {
      AvatarSetupTool.ClearHumanBoneArray(serializedObject);
      AvatarSetupTool.ClearSkeletonBoneArray(serializedObject);
    }

    public static void ClearHumanBoneArray(SerializedObject serializedObject)
    {
      SerializedProperty property = serializedObject.FindProperty(AvatarSetupTool.sHuman);
      if (property == null || !property.isArray)
        return;
      property.ClearArray();
    }

    public static void ClearSkeletonBoneArray(SerializedObject serializedObject)
    {
      SerializedProperty property = serializedObject.FindProperty(AvatarSetupTool.sSkeleton);
      if (property == null || !property.isArray)
        return;
      property.ClearArray();
    }

    public static void AutoSetupOnInstance(GameObject modelPrefab, SerializedObject modelImporterSerializedObject)
    {
      GameObject modelInstance = UnityEngine.Object.Instantiate<GameObject>(modelPrefab);
      modelInstance.hideFlags = HideFlags.HideAndDontSave;
      AvatarSetupTool.AutoSetup(modelPrefab, modelInstance, modelImporterSerializedObject);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) modelInstance);
    }

    public static bool IsPoseValidOnInstance(GameObject modelPrefab, SerializedObject modelImporterSerializedObject)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(modelPrefab);
      gameObject.hideFlags = HideFlags.HideAndDontSave;
      Dictionary<Transform, bool> modelBones = AvatarSetupTool.GetModelBones(gameObject.transform, false, (AvatarSetupTool.BoneWrapper[]) null);
      AvatarSetupTool.BoneWrapper[] humanBones = AvatarSetupTool.GetHumanBones(modelImporterSerializedObject, modelBones);
      AvatarSetupTool.TransferDescriptionToPose(modelImporterSerializedObject, gameObject.transform);
      bool flag = AvatarSetupTool.IsPoseValid(humanBones);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) gameObject);
      return flag;
    }

    public static void AutoSetup(GameObject modelPrefab, GameObject modelInstance, SerializedObject modelImporterSerializedObject)
    {
      SerializedProperty property1 = modelImporterSerializedObject.FindProperty(AvatarSetupTool.sHuman);
      SerializedProperty property2 = modelImporterSerializedObject.FindProperty(AvatarSetupTool.sHasTranslationDoF);
      if (property1 == null || !property1.isArray)
        return;
      AvatarSetupTool.ClearHumanBoneArray(modelImporterSerializedObject);
      bool flag = AvatarBipedMapper.IsBiped(modelInstance.transform, (List<string>) null);
      AvatarSetupTool.SampleBindPose(modelInstance);
      Dictionary<Transform, bool> modelBones = AvatarSetupTool.GetModelBones(modelInstance.transform, false, (AvatarSetupTool.BoneWrapper[]) null);
      Dictionary<int, Transform> dictionary;
      if (flag)
      {
        dictionary = AvatarBipedMapper.MapBones(modelInstance.transform);
        AvatarBipedMapper.BipedPose(modelInstance);
      }
      else
        dictionary = AvatarAutoMapper.MapBones(modelInstance.transform, modelBones);
      AvatarSetupTool.BoneWrapper[] humanBones = AvatarSetupTool.GetHumanBones(modelImporterSerializedObject, modelBones);
      for (int key = 0; key < humanBones.Length; ++key)
      {
        AvatarSetupTool.BoneWrapper boneWrapper = humanBones[key];
        boneWrapper.bone = !dictionary.ContainsKey(key) ? (Transform) null : dictionary[key];
        boneWrapper.Serialize(modelImporterSerializedObject);
      }
      if (!flag)
      {
        float poseError1 = AvatarSetupTool.GetPoseError(humanBones);
        AvatarSetupTool.CopyPose(modelInstance, modelPrefab);
        float poseError2 = AvatarSetupTool.GetPoseError(humanBones);
        if ((double) poseError1 < (double) poseError2)
          AvatarSetupTool.SampleBindPose(modelInstance);
        AvatarSetupTool.MakePoseValid(humanBones);
      }
      else
        property2.boolValue = true;
      AvatarSetupTool.TransferPoseToDescription(modelImporterSerializedObject, modelInstance.transform);
    }

    public static bool TestAndValidateAutoSetup(GameObject modelAsset)
    {
      if ((UnityEngine.Object) modelAsset == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "GameObject is null");
        return false;
      }
      if (PrefabUtility.GetPrefabType((UnityEngine.Object) modelAsset) != PrefabType.ModelPrefab)
      {
        Debug.LogError((object) (modelAsset.name + ": GameObject is not a ModelPrefab"), (UnityEngine.Object) modelAsset);
        return false;
      }
      if ((UnityEngine.Object) modelAsset.transform.parent != (UnityEngine.Object) null)
      {
        Debug.LogError((object) (modelAsset.name + ": GameObject is not the root"), (UnityEngine.Object) modelAsset);
        return false;
      }
      string assetPath1 = AssetDatabase.GetAssetPath((UnityEngine.Object) modelAsset);
      ModelImporter atPath = AssetImporter.GetAtPath(assetPath1) as ModelImporter;
      if ((UnityEngine.Object) atPath == (UnityEngine.Object) null)
      {
        Debug.LogError((object) (modelAsset.name + ": Could not load ModelImporter (path:" + assetPath1 + ")"), (UnityEngine.Object) modelAsset);
        return false;
      }
      SerializedObject serializedObject = new SerializedObject((UnityEngine.Object) atPath);
      SerializedProperty property = serializedObject.FindProperty("m_AnimationType");
      if (property == null)
      {
        Debug.LogError((object) (modelAsset.name + ": Could not find property m_AnimationType on ModelImporter"), (UnityEngine.Object) modelAsset);
        return false;
      }
      property.intValue = 2;
      AvatarSetupTool.ClearAll(serializedObject);
      serializedObject.ApplyModifiedProperties();
      AssetDatabase.ImportAsset(assetPath1);
      property.intValue = 3;
      AvatarSetupTool.AutoSetupOnInstance(modelAsset, serializedObject);
      serializedObject.ApplyModifiedProperties();
      AssetDatabase.ImportAsset(assetPath1);
      Avatar avatar = AssetDatabase.LoadAssetAtPath(assetPath1, typeof (Avatar)) as Avatar;
      if ((UnityEngine.Object) avatar == (UnityEngine.Object) null)
      {
        Debug.LogError((object) (modelAsset.name + ": Did not find Avatar after reimport with CreateAvatar enabled"), (UnityEngine.Object) modelAsset);
        return false;
      }
      if (!avatar.isHuman)
      {
        Debug.LogError((object) (modelAsset.name + ": Avatar is not valid after reimport"), (UnityEngine.Object) modelAsset);
        return false;
      }
      if (!AvatarSetupTool.IsPoseValidOnInstance(modelAsset, serializedObject))
      {
        Debug.LogError((object) (modelAsset.name + ": Avatar has invalid pose after reimport"), (UnityEngine.Object) modelAsset);
        return false;
      }
      string str = assetPath1.Substring(0, assetPath1.Length - Path.GetExtension(assetPath1).Length);
      string assetPath2 = str + ".ht";
      HumanTemplate humanTemplate = AssetDatabase.LoadMainAssetAtPath(assetPath2) as HumanTemplate;
      if ((UnityEngine.Object) humanTemplate == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) (modelAsset.name + ": Didn't find template at path " + assetPath2));
      }
      else
      {
        List<string> stringList = (List<string>) null;
        string path = str + ".ignore";
        if (File.Exists(path))
          stringList = new List<string>((IEnumerable<string>) File.ReadAllLines(path));
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(modelAsset);
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        Dictionary<Transform, bool> modelBones = AvatarSetupTool.GetModelBones(gameObject.transform, false, (AvatarSetupTool.BoneWrapper[]) null);
        AvatarSetupTool.BoneWrapper[] humanBones = AvatarSetupTool.GetHumanBones(serializedObject, modelBones);
        bool flag = false;
        for (int index = 0; index < humanBones.Length; ++index)
        {
          if (stringList == null || !stringList.Contains(humanBones[index].humanBoneName))
          {
            string boneName = humanTemplate.Find(humanBones[index].humanBoneName);
            string transformName = !((UnityEngine.Object) humanBones[index].bone == (UnityEngine.Object) null) ? humanBones[index].bone.name : string.Empty;
            if (!AvatarMappingEditor.MatchName(transformName, boneName))
            {
              flag = true;
              Debug.LogError((object) (modelAsset.name + ": Avatar has bone " + humanBones[index].humanBoneName + " mapped to \"" + transformName + "\" but expected \"" + boneName + "\""), (UnityEngine.Object) modelAsset);
            }
          }
        }
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) gameObject);
        if (flag)
          return false;
      }
      return true;
    }

    public static void DebugTransformTree(Transform tr, Dictionary<Transform, bool> bones, int level)
    {
      string str = "  ";
      if (bones.ContainsKey(tr))
        str = !bones[tr] ? ". " : "* ";
      Debug.Log((object) ("                                             ".Substring(0, level * 2) + str + tr.name + "\n\n"));
      foreach (Transform tr1 in tr)
        AvatarSetupTool.DebugTransformTree(tr1, bones, level + 1);
    }

    public static SerializedProperty FindSkeletonBone(SerializedObject serializedObject, Transform t, bool createMissing, bool isRoot)
    {
      SerializedProperty property = serializedObject.FindProperty(AvatarSetupTool.sSkeleton);
      if (property == null || !property.isArray)
        return (SerializedProperty) null;
      return AvatarSetupTool.FindSkeletonBone(property, t, createMissing, isRoot);
    }

    public static SerializedProperty FindSkeletonBone(SerializedProperty skeletonBoneArray, Transform t, bool createMissing, bool isRoot)
    {
      if (isRoot && skeletonBoneArray.arraySize > 0)
      {
        SerializedProperty arrayElementAtIndex = skeletonBoneArray.GetArrayElementAtIndex(0);
        if (arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sName).stringValue == t.name)
          return arrayElementAtIndex;
      }
      else
      {
        for (int index = 1; index < skeletonBoneArray.arraySize; ++index)
        {
          SerializedProperty arrayElementAtIndex = skeletonBoneArray.GetArrayElementAtIndex(index);
          if (arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sName).stringValue == t.name)
            return arrayElementAtIndex;
        }
      }
      if (createMissing)
      {
        ++skeletonBoneArray.arraySize;
        SerializedProperty arrayElementAtIndex = skeletonBoneArray.GetArrayElementAtIndex(skeletonBoneArray.arraySize - 1);
        if (arrayElementAtIndex != null)
        {
          arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sName).stringValue = t.name;
          arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sPosition).vector3Value = t.localPosition;
          arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sRotation).quaternionValue = t.localRotation;
          arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sScale).vector3Value = t.localScale;
          arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sTransformModified).boolValue = true;
          return arrayElementAtIndex;
        }
      }
      return (SerializedProperty) null;
    }

    public static void CopyPose(GameObject go, GameObject source)
    {
      GameObject go1 = UnityEngine.Object.Instantiate<GameObject>(source);
      go1.hideFlags = HideFlags.HideAndDontSave;
      AnimatorUtility.DeoptimizeTransformHierarchy(go1);
      AvatarSetupTool.CopyPose(go.transform, go1.transform);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) go1);
    }

    private static void CopyPose(Transform t, Transform source)
    {
      t.localPosition = source.localPosition;
      t.localRotation = source.localRotation;
      t.localScale = source.localScale;
      foreach (Transform t1 in t)
      {
        Transform child = source.FindChild(t1.name);
        if ((bool) ((UnityEngine.Object) child))
          AvatarSetupTool.CopyPose(t1, child);
      }
    }

    public static void GetBindPoseBonePositionRotation(Matrix4x4 skinMatrix, Matrix4x4 boneMatrix, Transform bone, out Vector3 position, out Quaternion rotation)
    {
      Matrix4x4 matrix4x4 = skinMatrix * boneMatrix.inverse;
      Vector3 lhs = new Vector3(matrix4x4.m00, matrix4x4.m10, matrix4x4.m20);
      Vector3 vector3_1 = new Vector3(matrix4x4.m01, matrix4x4.m11, matrix4x4.m21);
      Vector3 vector3_2 = new Vector3(matrix4x4.m02, matrix4x4.m12, matrix4x4.m22);
      Vector3 vector3_3 = new Vector3(matrix4x4.m03, matrix4x4.m13, matrix4x4.m23);
      float magnitude = vector3_2.magnitude;
      float num = Mathf.Abs(bone.lossyScale.z);
      position = vector3_3 * (num / magnitude);
      if ((double) Vector3.Dot(Vector3.Cross(lhs, vector3_1), vector3_2) >= 0.0)
        rotation = Quaternion.LookRotation(vector3_2, vector3_1);
      else
        rotation = Quaternion.LookRotation(-vector3_2, -vector3_1);
    }

    public static void SampleBindPose(GameObject go)
    {
      List<SkinnedMeshRenderer> skinnedMeshRendererList = new List<SkinnedMeshRenderer>((IEnumerable<SkinnedMeshRenderer>) go.GetComponentsInChildren<SkinnedMeshRenderer>());
      skinnedMeshRendererList.Sort((IComparer<SkinnedMeshRenderer>) new AvatarSetupTool.SkinTransformHierarchySorter());
      using (List<SkinnedMeshRenderer>.Enumerator enumerator1 = skinnedMeshRendererList.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          SkinnedMeshRenderer current1 = enumerator1.Current;
          Matrix4x4 localToWorldMatrix = current1.transform.localToWorldMatrix;
          List<Transform> transformList = new List<Transform>((IEnumerable<Transform>) current1.bones);
          Vector3[] vector3Array = new Vector3[transformList.Count];
          for (int index = 0; index < transformList.Count; ++index)
            vector3Array[index] = transformList[index].localPosition;
          Dictionary<Transform, Transform> dictionary = new Dictionary<Transform, Transform>();
          using (List<Transform>.Enumerator enumerator2 = transformList.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              Transform current2 = enumerator2.Current;
              dictionary[current2] = current2.parent;
              current2.parent = (Transform) null;
            }
          }
          for (int index = 0; index < transformList.Count; ++index)
          {
            Vector3 position;
            Quaternion rotation;
            AvatarSetupTool.GetBindPoseBonePositionRotation(localToWorldMatrix, current1.sharedMesh.bindposes[index], transformList[index], out position, out rotation);
            transformList[index].position = position;
            transformList[index].rotation = rotation;
          }
          using (List<Transform>.Enumerator enumerator2 = transformList.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              Transform current2 = enumerator2.Current;
              current2.parent = dictionary[current2];
            }
          }
          for (int index = 0; index < transformList.Count; ++index)
            transformList[index].localPosition = vector3Array[index];
        }
      }
    }

    public static void ShowBindPose(SkinnedMeshRenderer skin)
    {
      Matrix4x4 localToWorldMatrix = skin.transform.localToWorldMatrix;
      for (int index = 0; index < skin.bones.Length; ++index)
      {
        Vector3 position;
        Quaternion rotation;
        AvatarSetupTool.GetBindPoseBonePositionRotation(localToWorldMatrix, skin.sharedMesh.bindposes[index], skin.bones[index], out position, out rotation);
        float handleSize = HandleUtility.GetHandleSize(position);
        Handles.color = Handles.xAxisColor;
        Handles.DrawLine(position, position + rotation * Vector3.right * 0.3f * handleSize);
        Handles.color = Handles.yAxisColor;
        Handles.DrawLine(position, position + rotation * Vector3.up * 0.3f * handleSize);
        Handles.color = Handles.zAxisColor;
        Handles.DrawLine(position, position + rotation * Vector3.forward * 0.3f * handleSize);
      }
    }

    public static void TransferPoseToDescription(SerializedObject serializedObject, Transform root)
    {
      SkeletonBone[] skeletonBones = new SkeletonBone[0];
      if ((bool) ((UnityEngine.Object) root))
        AvatarSetupTool.TransferPoseToDescription(serializedObject, root, true, ref skeletonBones);
      SerializedProperty property = serializedObject.FindProperty(AvatarSetupTool.sSkeleton);
      ModelImporter.UpdateSkeletonPose(skeletonBones, property);
    }

    private static void TransferPoseToDescription(SerializedObject serializedObject, Transform transform, bool isRoot, ref SkeletonBone[] skeletonBones)
    {
      ArrayUtility.Add<SkeletonBone>(ref skeletonBones, new SkeletonBone()
      {
        name = transform.name,
        position = transform.localPosition,
        rotation = transform.localRotation,
        scale = transform.localScale,
        transformModified = 1
      });
      foreach (Transform transform1 in transform)
        AvatarSetupTool.TransferPoseToDescription(serializedObject, transform1, false, ref skeletonBones);
    }

    public static void TransferDescriptionToPose(SerializedObject serializedObject, Transform root)
    {
      if (!((UnityEngine.Object) root != (UnityEngine.Object) null))
        return;
      AvatarSetupTool.TransferDescriptionToPose(serializedObject, root, true);
    }

    private static void TransferDescriptionToPose(SerializedObject serializedObject, Transform transform, bool isRoot)
    {
      SerializedProperty skeletonBone = AvatarSetupTool.FindSkeletonBone(serializedObject, transform, false, isRoot);
      if (skeletonBone != null && skeletonBone.FindPropertyRelative(AvatarSetupTool.sTransformModified).boolValue)
      {
        SerializedProperty propertyRelative1 = skeletonBone.FindPropertyRelative(AvatarSetupTool.sPosition);
        SerializedProperty propertyRelative2 = skeletonBone.FindPropertyRelative(AvatarSetupTool.sRotation);
        SerializedProperty propertyRelative3 = skeletonBone.FindPropertyRelative(AvatarSetupTool.sScale);
        transform.localPosition = propertyRelative1.vector3Value;
        transform.localRotation = propertyRelative2.quaternionValue;
        transform.localScale = propertyRelative3.vector3Value;
      }
      foreach (Transform transform1 in transform)
        AvatarSetupTool.TransferDescriptionToPose(serializedObject, transform1, false);
    }

    public static bool IsPoseValid(AvatarSetupTool.BoneWrapper[] bones)
    {
      return (double) AvatarSetupTool.GetPoseError(bones) == 0.0;
    }

    public static float GetPoseError(AvatarSetupTool.BoneWrapper[] bones)
    {
      Quaternion orientation = AvatarSetupTool.AvatarComputeOrientation(bones);
      float num = 0.0f;
      for (int boneIndex = 0; boneIndex < AvatarSetupTool.sBonePoses.Length; ++boneIndex)
        num += AvatarSetupTool.GetBoneAlignmentError(bones, orientation, boneIndex);
      return num + AvatarSetupTool.GetCharacterPositionError(bones);
    }

    public static void MakePoseValid(AvatarSetupTool.BoneWrapper[] bones)
    {
      Quaternion orientation = AvatarSetupTool.AvatarComputeOrientation(bones);
      for (int boneIndex = 0; boneIndex < AvatarSetupTool.sBonePoses.Length; ++boneIndex)
      {
        AvatarSetupTool.MakeBoneAlignmentValid(bones, orientation, boneIndex);
        if (boneIndex == 0)
          orientation = AvatarSetupTool.AvatarComputeOrientation(bones);
      }
      AvatarSetupTool.MakeCharacterPositionValid(bones);
    }

    public static float GetBoneAlignmentError(AvatarSetupTool.BoneWrapper[] bones, Quaternion avatarOrientation, int boneIndex)
    {
      if (boneIndex < 0 || boneIndex >= AvatarSetupTool.sBonePoses.Length)
        return 0.0f;
      AvatarSetupTool.BoneWrapper bone = bones[boneIndex];
      AvatarSetupTool.BonePoseData sBonePose = AvatarSetupTool.sBonePoses[boneIndex];
      if ((UnityEngine.Object) bone.bone == (UnityEngine.Object) null || sBonePose == null)
        return 0.0f;
      if (boneIndex == 0)
        return Mathf.Max(0.0f, (float) ((double) Mathf.Max(Vector3.Angle(avatarOrientation * Vector3.right, Vector3.right), Vector3.Angle(avatarOrientation * Vector3.up, Vector3.up), Vector3.Angle(avatarOrientation * Vector3.forward, Vector3.forward)) - (double) sBonePose.maxAngle));
      Vector3 vector3 = AvatarSetupTool.GetBoneAlignmentDirection(bones, avatarOrientation, boneIndex);
      if (vector3 == Vector3.zero)
        return 0.0f;
      Quaternion rotationSpace = AvatarSetupTool.GetRotationSpace(bones, avatarOrientation, boneIndex);
      Vector3 to = rotationSpace * sBonePose.direction;
      if (sBonePose.planeNormal != Vector3.zero)
        vector3 = Vector3.ProjectOnPlane(vector3, rotationSpace * sBonePose.planeNormal);
      return Mathf.Max(0.0f, Vector3.Angle(vector3, to) - sBonePose.maxAngle);
    }

    public static void MakeBoneAlignmentValid(AvatarSetupTool.BoneWrapper[] bones, Quaternion avatarOrientation, int boneIndex)
    {
      if (boneIndex < 0 || boneIndex >= AvatarSetupTool.sBonePoses.Length || boneIndex >= bones.Length)
        return;
      AvatarSetupTool.BoneWrapper bone = bones[boneIndex];
      AvatarSetupTool.BonePoseData sBonePose = AvatarSetupTool.sBonePoses[boneIndex];
      if ((UnityEngine.Object) bone.bone == (UnityEngine.Object) null || sBonePose == null)
        return;
      if (boneIndex == 0)
      {
        float num1 = Vector3.Angle(avatarOrientation * Vector3.right, Vector3.right);
        float num2 = Vector3.Angle(avatarOrientation * Vector3.up, Vector3.up);
        float num3 = Vector3.Angle(avatarOrientation * Vector3.forward, Vector3.forward);
        if ((double) num1 <= (double) sBonePose.maxAngle && (double) num2 <= (double) sBonePose.maxAngle && (double) num3 <= (double) sBonePose.maxAngle)
          return;
        bone.bone.rotation = Quaternion.Inverse(avatarOrientation) * bone.bone.rotation;
      }
      else
      {
        Vector3 vector3_1 = AvatarSetupTool.GetBoneAlignmentDirection(bones, avatarOrientation, boneIndex);
        if (vector3_1 == Vector3.zero)
          return;
        Quaternion rotationSpace = AvatarSetupTool.GetRotationSpace(bones, avatarOrientation, boneIndex);
        Vector3 vector3_2 = rotationSpace * sBonePose.direction;
        if (sBonePose.planeNormal != Vector3.zero)
          vector3_1 = Vector3.ProjectOnPlane(vector3_1, rotationSpace * sBonePose.planeNormal);
        float num = Vector3.Angle(vector3_1, vector3_2);
        if ((double) num <= (double) sBonePose.maxAngle * 0.990000009536743)
          return;
        Quaternion rotation = Quaternion.FromToRotation(vector3_1, vector3_2);
        Transform transform = (Transform) null;
        Quaternion quaternion1 = Quaternion.identity;
        if (boneIndex == 1 || boneIndex == 3)
          transform = bones[5].bone;
        if (boneIndex == 2 || boneIndex == 4)
          transform = bones[6].bone;
        if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
          quaternion1 = transform.rotation;
        float t = Mathf.Clamp01((float) (1.04999995231628 - (double) sBonePose.maxAngle / (double) num));
        Quaternion quaternion2 = Quaternion.Slerp(Quaternion.identity, rotation, t);
        bone.bone.rotation = quaternion2 * bone.bone.rotation;
        if (!((UnityEngine.Object) transform != (UnityEngine.Object) null))
          return;
        transform.rotation = quaternion1;
      }
    }

    private static Quaternion GetRotationSpace(AvatarSetupTool.BoneWrapper[] bones, Quaternion avatarOrientation, int boneIndex)
    {
      Quaternion quaternion = Quaternion.identity;
      if (!AvatarSetupTool.sBonePoses[boneIndex].compareInGlobalSpace)
      {
        int boneIndex1 = AvatarSetupTool.sHumanParent[boneIndex];
        AvatarSetupTool.BonePoseData sBonePose = AvatarSetupTool.sBonePoses[boneIndex1];
        if ((UnityEngine.Object) bones[boneIndex1].bone != (UnityEngine.Object) null && sBonePose != null)
        {
          Vector3 alignmentDirection = AvatarSetupTool.GetBoneAlignmentDirection(bones, avatarOrientation, boneIndex1);
          if (alignmentDirection != Vector3.zero)
            quaternion = Quaternion.FromToRotation(avatarOrientation * sBonePose.direction, alignmentDirection);
        }
      }
      return quaternion * avatarOrientation;
    }

    private static Vector3 GetBoneAlignmentDirection(AvatarSetupTool.BoneWrapper[] bones, Quaternion avatarOrientation, int boneIndex)
    {
      if (AvatarSetupTool.sBonePoses[boneIndex] == null)
        return Vector3.zero;
      AvatarSetupTool.BoneWrapper bone = bones[boneIndex];
      AvatarSetupTool.BonePoseData sBonePose = AvatarSetupTool.sBonePoses[boneIndex];
      int index = -1;
      if (sBonePose.childIndices != null)
      {
        foreach (int childIndex in sBonePose.childIndices)
        {
          if ((UnityEngine.Object) bones[childIndex].bone != (UnityEngine.Object) null)
          {
            index = childIndex;
            break;
          }
        }
      }
      else
        index = AvatarSetupTool.GetHumanBoneChild(bones, boneIndex);
      Vector3 vector3;
      if (index >= 0 && bones[index] != null && (UnityEngine.Object) bones[index].bone != (UnityEngine.Object) null)
      {
        vector3 = bones[index].bone.position - bone.bone.position;
      }
      else
      {
        if (bone.bone.childCount != 1)
          return Vector3.zero;
        vector3 = Vector3.zero;
        IEnumerator enumerator = bone.bone.GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
            vector3 = ((Transform) enumerator.Current).position - bone.bone.position;
        }
        finally
        {
          IDisposable disposable = enumerator as IDisposable;
          if (disposable != null)
            disposable.Dispose();
        }
      }
      return vector3.normalized;
    }

    public static Quaternion AvatarComputeOrientation(AvatarSetupTool.BoneWrapper[] bones)
    {
      Transform bone1 = bones[1].bone;
      Transform bone2 = bones[2].bone;
      Transform bone3 = bones[13].bone;
      Transform bone4 = bones[14].bone;
      if ((UnityEngine.Object) bone1 != (UnityEngine.Object) null && (UnityEngine.Object) bone2 != (UnityEngine.Object) null && ((UnityEngine.Object) bone3 != (UnityEngine.Object) null && (UnityEngine.Object) bone4 != (UnityEngine.Object) null))
        return AvatarSetupTool.AvatarComputeOrientation(bone1.position, bone2.position, bone3.position, bone4.position);
      return Quaternion.identity;
    }

    public static Quaternion AvatarComputeOrientation(Vector3 leftUpLeg, Vector3 rightUpLeg, Vector3 leftArm, Vector3 rightArm)
    {
      Vector3 lhs = Vector3.Normalize(Vector3.Normalize(rightUpLeg - leftUpLeg) + Vector3.Normalize(rightArm - leftArm));
      bool flag = (double) Mathf.Abs(lhs.x * lhs.y) < 0.0500000007450581 && (double) Mathf.Abs(lhs.y * lhs.z) < 0.0500000007450581 && (double) Mathf.Abs(lhs.z * lhs.x) < 0.0500000007450581;
      Vector3 vector3_1 = (leftUpLeg + rightUpLeg) * 0.5f;
      Vector3 vector3_2 = Vector3.Normalize((leftArm + rightArm) * 0.5f - vector3_1);
      if (flag)
      {
        int index1 = 0;
        for (int index2 = 1; index2 < 3; ++index2)
        {
          if ((double) Mathf.Abs(vector3_2[index2]) > (double) Mathf.Abs(vector3_2[index1]))
            index1 = index2;
        }
        float num = Mathf.Sign(vector3_2[index1]);
        vector3_2 = Vector3.zero;
        vector3_2[index1] = num;
      }
      Vector3 forward = Vector3.Cross(lhs, vector3_2);
      if (forward == Vector3.zero || vector3_2 == Vector3.zero)
        return Quaternion.identity;
      return Quaternion.LookRotation(forward, vector3_2);
    }

    private static float GetCharacterPositionError(AvatarSetupTool.BoneWrapper[] bones)
    {
      float error;
      AvatarSetupTool.GetCharacterPositionAdjustVector(bones, out error);
      return error;
    }

    private static void MakeCharacterPositionValid(AvatarSetupTool.BoneWrapper[] bones)
    {
      float error;
      Vector3 positionAdjustVector = AvatarSetupTool.GetCharacterPositionAdjustVector(bones, out error);
      if (!(positionAdjustVector != Vector3.zero))
        return;
      bones[0].bone.position += positionAdjustVector;
    }

    private static Vector3 GetCharacterPositionAdjustVector(AvatarSetupTool.BoneWrapper[] bones, out float error)
    {
      error = 0.0f;
      Transform bone1 = bones[1].bone;
      Transform bone2 = bones[2].bone;
      if ((UnityEngine.Object) bone1 == (UnityEngine.Object) null || (UnityEngine.Object) bone2 == (UnityEngine.Object) null)
        return Vector3.zero;
      Vector3 vector3_1 = (bone1.position + bone2.position) * 0.5f;
      bool flag = true;
      Transform bone3 = bones[19].bone;
      Transform bone4 = bones[20].bone;
      if ((UnityEngine.Object) bone3 == (UnityEngine.Object) null || (UnityEngine.Object) bone4 == (UnityEngine.Object) null)
      {
        flag = false;
        bone3 = bones[5].bone;
        bone4 = bones[6].bone;
      }
      if ((UnityEngine.Object) bone3 == (UnityEngine.Object) null || (UnityEngine.Object) bone4 == (UnityEngine.Object) null)
        return Vector3.zero;
      Vector3 vector3_2 = (bone3.position + bone4.position) * 0.5f;
      float num1 = vector3_1.y - vector3_2.y;
      if ((double) num1 <= 0.0)
        return Vector3.zero;
      Vector3 zero = Vector3.zero;
      if ((double) vector3_2.y < 0.0 || (double) vector3_2.y > (double) num1 * (!flag ? 0.300000011920929 : 0.100000001490116))
      {
        float num2 = vector3_1.y - num1 * (!flag ? 1.13f : 1.03f);
        zero.y = -num2;
      }
      if ((double) Mathf.Abs(vector3_1.x) > 0.00999999977648258 * (double) num1)
        zero.x = -vector3_1.x;
      if ((double) Mathf.Abs(vector3_1.z) > 0.200000002980232 * (double) num1)
        zero.z = -vector3_1.z;
      error = zero.magnitude * 100f / num1;
      return zero;
    }

    [Serializable]
    internal class BoneWrapper
    {
      private static string sHumanName = "m_HumanName";
      private static string sBoneName = "m_BoneName";
      private static Color kBoneValid = new Color(0.0f, 0.75f, 0.0f, 1f);
      private static Color kBoneInvalid = new Color(1f, 0.3f, 0.25f, 1f);
      private static Color kBoneInactive = Color.gray;
      private static Color kBoneSelected = new Color(0.4f, 0.7f, 1f, 1f);
      private static Color kBoneDrop = new Color(0.1f, 0.7f, 1f, 1f);
      public string error = string.Empty;
      public const int kIconSize = 19;
      private string m_HumanBoneName;
      public Transform bone;
      public BoneState state;

      public string humanBoneName
      {
        get
        {
          return this.m_HumanBoneName;
        }
      }

      public string messageName
      {
        get
        {
          return ObjectNames.NicifyVariableName(this.m_HumanBoneName) + " Transform '" + (!(bool) ((UnityEngine.Object) this.bone) ? "None" : this.bone.name) + "'";
        }
      }

      public BoneWrapper(string humanBoneName, SerializedObject serializedObject, Dictionary<Transform, bool> bones)
      {
        this.m_HumanBoneName = humanBoneName;
        this.Reset(serializedObject, bones);
      }

      public void Reset(SerializedObject serializedObject, Dictionary<Transform, bool> bones)
      {
        this.bone = (Transform) null;
        SerializedProperty serializedProperty = this.GetSerializedProperty(serializedObject, false);
        if (serializedProperty != null)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: reference to a compiler-generated method
          this.bone = bones.Keys.FirstOrDefault<Transform>(new Func<Transform, bool>(new AvatarSetupTool.BoneWrapper.\u003CReset\u003Ec__AnonStorey9F()
          {
            boneName = serializedProperty.FindPropertyRelative(AvatarSetupTool.BoneWrapper.sBoneName).stringValue
          }.\u003C\u003Em__1CC));
        }
        this.state = BoneState.Valid;
      }

      public void Serialize(SerializedObject serializedObject)
      {
        if ((UnityEngine.Object) this.bone == (UnityEngine.Object) null)
        {
          this.DeleteSerializedProperty(serializedObject);
        }
        else
        {
          SerializedProperty serializedProperty = this.GetSerializedProperty(serializedObject, true);
          if (serializedProperty == null)
            return;
          serializedProperty.FindPropertyRelative(AvatarSetupTool.BoneWrapper.sBoneName).stringValue = this.bone.name;
        }
      }

      protected void DeleteSerializedProperty(SerializedObject serializedObject)
      {
        SerializedProperty property = serializedObject.FindProperty(AvatarSetupTool.sHuman);
        if (property == null || !property.isArray)
          return;
        for (int index = 0; index < property.arraySize; ++index)
        {
          if (property.GetArrayElementAtIndex(index).FindPropertyRelative(AvatarSetupTool.BoneWrapper.sHumanName).stringValue == this.humanBoneName)
          {
            property.DeleteArrayElementAtIndex(index);
            break;
          }
        }
      }

      public SerializedProperty GetSerializedProperty(SerializedObject serializedObject, bool createIfMissing)
      {
        SerializedProperty property = serializedObject.FindProperty(AvatarSetupTool.sHuman);
        if (property == null || !property.isArray)
          return (SerializedProperty) null;
        for (int index = 0; index < property.arraySize; ++index)
        {
          if (property.GetArrayElementAtIndex(index).FindPropertyRelative(AvatarSetupTool.BoneWrapper.sHumanName).stringValue == this.humanBoneName)
            return property.GetArrayElementAtIndex(index);
        }
        if (createIfMissing)
        {
          ++property.arraySize;
          SerializedProperty arrayElementAtIndex = property.GetArrayElementAtIndex(property.arraySize - 1);
          if (arrayElementAtIndex != null)
          {
            arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.BoneWrapper.sHumanName).stringValue = this.humanBoneName;
            return arrayElementAtIndex;
          }
        }
        return (SerializedProperty) null;
      }

      public void BoneDotGUI(Rect rect, int boneIndex, bool doClickSelect, bool doDragDrop, SerializedObject serializedObject, AvatarMappingEditor editor)
      {
        int controlId = GUIUtility.GetControlID(FocusType.Passive, rect);
        if (doClickSelect)
          this.HandleClickSelection(rect, boneIndex);
        if (doDragDrop)
          this.HandleDragDrop(rect, boneIndex, controlId, serializedObject, editor);
        Color color = GUI.color;
        if (AvatarMappingEditor.s_SelectedBoneIndex == boneIndex)
        {
          GUI.color = AvatarSetupTool.BoneWrapper.kBoneSelected;
          GUI.DrawTexture(rect, AvatarMappingEditor.styles.dotSelection.image);
        }
        GUI.color = DragAndDrop.activeControlID != controlId ? (this.state != BoneState.Valid ? (this.state != BoneState.None ? AvatarSetupTool.BoneWrapper.kBoneInvalid : AvatarSetupTool.BoneWrapper.kBoneInactive) : AvatarSetupTool.BoneWrapper.kBoneValid) : AvatarSetupTool.BoneWrapper.kBoneDrop;
        Texture image = !HumanTrait.RequiredBone(boneIndex) ? AvatarMappingEditor.styles.dotFrameDotted.image : AvatarMappingEditor.styles.dotFrame.image;
        GUI.DrawTexture(rect, image);
        if ((UnityEngine.Object) this.bone != (UnityEngine.Object) null || DragAndDrop.activeControlID == controlId)
          GUI.DrawTexture(rect, AvatarMappingEditor.styles.dotFill.image);
        GUI.color = color;
      }

      public void HandleClickSelection(Rect selectRect, int boneIndex)
      {
        Event current = Event.current;
        if (current.type != EventType.MouseDown || !selectRect.Contains(current.mousePosition))
          return;
        AvatarMappingEditor.s_SelectedBoneIndex = boneIndex;
        AvatarMappingEditor.s_DirtySelection = true;
        Selection.activeTransform = this.bone;
        if ((UnityEngine.Object) this.bone != (UnityEngine.Object) null)
          EditorGUIUtility.PingObject((UnityEngine.Object) this.bone);
        current.Use();
      }

      private void HandleDragDrop(Rect dropRect, int boneIndex, int id, SerializedObject serializedObject, AvatarMappingEditor editor)
      {
        EventType type = Event.current.type;
        switch (type)
        {
          case EventType.DragUpdated:
          case EventType.DragPerform:
            if (!dropRect.Contains(Event.current.mousePosition) || !GUI.enabled)
              break;
            UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
            UnityEngine.Object target = objectReferences.Length != 1 ? (UnityEngine.Object) null : objectReferences[0];
            if (target != (UnityEngine.Object) null && (!(target is Transform) && !(target is GameObject) || EditorUtility.IsPersistent(target)))
              target = (UnityEngine.Object) null;
            if (!(target != (UnityEngine.Object) null))
              break;
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            if (type == EventType.DragPerform)
            {
              Undo.RegisterCompleteObjectUndo((UnityEngine.Object) editor, "Avatar mapping modified");
              this.bone = !(target is GameObject) ? target as Transform : (target as GameObject).transform;
              this.Serialize(serializedObject);
              GUI.changed = true;
              DragAndDrop.AcceptDrag();
              DragAndDrop.activeControlID = 0;
            }
            else
              DragAndDrop.activeControlID = id;
            Event.current.Use();
            break;
          case EventType.DragExited:
            if (!GUI.enabled)
              break;
            HandleUtility.Repaint();
            break;
        }
      }
    }

    private class BonePoseData
    {
      public Vector3 direction = Vector3.zero;
      public Vector3 planeNormal = Vector3.zero;
      public bool compareInGlobalSpace;
      public float maxAngle;
      public int[] childIndices;

      public BonePoseData(Vector3 dir, bool globalSpace, float maxAngleDiff)
      {
        this.direction = !(dir == Vector3.zero) ? dir.normalized : dir;
        this.compareInGlobalSpace = globalSpace;
        this.maxAngle = maxAngleDiff;
      }

      public BonePoseData(Vector3 dir, bool globalSpace, float maxAngleDiff, int[] children)
        : this(dir, globalSpace, maxAngleDiff)
      {
        this.childIndices = children;
      }

      public BonePoseData(Vector3 dir, bool globalSpace, float maxAngleDiff, Vector3 planeNormal, int[] children)
        : this(dir, globalSpace, maxAngleDiff, children)
      {
        this.planeNormal = planeNormal;
      }
    }

    private class TransformHierarchySorter : IComparer<Transform>
    {
      public int Compare(Transform a, Transform b)
      {
        while (!((UnityEngine.Object) a == (UnityEngine.Object) null))
        {
          if ((UnityEngine.Object) b == (UnityEngine.Object) null)
            return 1;
          a = a.parent;
          b = b.parent;
        }
        return (UnityEngine.Object) b == (UnityEngine.Object) null ? 0 : -1;
      }
    }

    private class SkinTransformHierarchySorter : IComparer<SkinnedMeshRenderer>
    {
      public int Compare(SkinnedMeshRenderer skinA, SkinnedMeshRenderer skinB)
      {
        Transform transform1 = skinA.transform;
        Transform transform2 = skinB.transform;
        while (!((UnityEngine.Object) transform1 == (UnityEngine.Object) null))
        {
          if ((UnityEngine.Object) transform2 == (UnityEngine.Object) null)
            return 1;
          transform1 = transform1.parent;
          transform2 = transform2.parent;
        }
        return (UnityEngine.Object) transform2 == (UnityEngine.Object) null ? 0 : -1;
      }
    }
  }
}
