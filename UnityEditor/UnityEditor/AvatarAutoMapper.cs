// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarAutoMapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityEditor
{
  internal class AvatarAutoMapper
  {
    private static bool kDebug = false;
    private static string[] kShoulderKeywords = new string[3]{ "shoulder", "collar", "clavicle" };
    private static string[] kUpperArmKeywords = new string[1]{ "up" };
    private static string[] kLowerArmKeywords = new string[3]{ "lo", "fore", "elbow" };
    private static string[] kHandKeywords = new string[2]{ "hand", "wrist" };
    private static string[] kUpperLegKeywords = new string[2]{ "up", "thigh" };
    private static string[] kLowerLegKeywords = new string[4]{ "lo", "calf", "knee", "shin" };
    private static string[] kFootKeywords = new string[2]{ "foot", "ankle" };
    private static string[] kToeKeywords = new string[4]{ "toe", "!end", "!top", "!nub" };
    private static string[] kNeckKeywords = new string[1]{ "neck" };
    private static string[] kHeadKeywords = new string[1]{ "head" };
    private static string[] kJawKeywords = new string[9]{ "jaw", "open", "!teeth", "!tongue", "!pony", "!braid", "!end", "!top", "!nub" };
    private static string[] kEyeKeywords = new string[9]{ "eye", "ball", "!brow", "!lid", "!pony", "!braid", "!end", "!top", "!nub" };
    private static string[] kThumbKeywords = new string[6]{ "thu", "!palm", "!wrist", "!end", "!top", "!nub" };
    private static string[] kIndexFingerKeywords = new string[7]{ "ind", "point", "!palm", "!wrist", "!end", "!top", "!nub" };
    private static string[] kMiddleFingerKeywords = new string[7]{ "mid", "long", "!palm", "!wrist", "!end", "!top", "!nub" };
    private static string[] kRingFingerKeywords = new string[6]{ "rin", "!palm", "!wrist", "!end", "!top", "!nub" };
    private static string[] kLittleFingerKeywords = new string[7]{ "lit", "pin", "!palm", "!wrist", "!end", "!top", "!nub" };
    private static AvatarAutoMapper.BoneMappingItem[] s_MappingDataBody = new AvatarAutoMapper.BoneMappingItem[17]{ new AvatarAutoMapper.BoneMappingItem(-1, 0, 1, 3, 0.0f, AvatarAutoMapper.Side.None, new string[0]), new AvatarAutoMapper.BoneMappingItem(0, 2, 1, 2, 0.0f, Vector3.right, AvatarAutoMapper.Side.Right, AvatarAutoMapper.kUpperLegKeywords), new AvatarAutoMapper.BoneMappingItem(2, 4, 1, 2, 3f, -Vector3.up, AvatarAutoMapper.Side.Right, AvatarAutoMapper.kLowerLegKeywords), new AvatarAutoMapper.BoneMappingItem(4, 6, 1, 2, 1f, -Vector3.up, AvatarAutoMapper.Side.Right, AvatarAutoMapper.kFootKeywords), new AvatarAutoMapper.BoneMappingItem(6, 20, 1, 2, 0.5f, Vector3.forward, AvatarAutoMapper.Side.Right, true, true, AvatarAutoMapper.kToeKeywords), new AvatarAutoMapper.BoneMappingItem(0, 7, 1, 3, 0.0f, Vector3.up, AvatarAutoMapper.Side.None, new string[0]), new AvatarAutoMapper.BoneMappingItem(7, 8, 0, 3, 1.4f, Vector3.up, AvatarAutoMapper.Side.None, true, false, new string[0]), new AvatarAutoMapper.BoneMappingItem(8, 12, 1, 3, 0.0f, Vector3.right, AvatarAutoMapper.Side.Right, true, false, AvatarAutoMapper.kShoulderKeywords), new AvatarAutoMapper.BoneMappingItem(12, 14, 0, 2, 0.5f, Vector3.right, AvatarAutoMapper.Side.Right, AvatarAutoMapper.kUpperArmKeywords), new AvatarAutoMapper.BoneMappingItem(14, 16, 1, 2, 2f, Vector3.right, AvatarAutoMapper.Side.Right, AvatarAutoMapper.kLowerArmKeywords), new AvatarAutoMapper.BoneMappingItem(16, 18, 1, 2, 1f, Vector3.right, AvatarAutoMapper.Side.Right, AvatarAutoMapper.kHandKeywords), new AvatarAutoMapper.BoneMappingItem(8, 9, 1, 3, 1.8f, Vector3.up, AvatarAutoMapper.Side.None, true, false, AvatarAutoMapper.kNeckKeywords), new AvatarAutoMapper.BoneMappingItem(9, 10, 0, 2, 0.3f, Vector3.up, AvatarAutoMapper.Side.None, AvatarAutoMapper.kHeadKeywords), new AvatarAutoMapper.BoneMappingItem(10, 23, 1, 2, 0.0f, Vector3.forward, AvatarAutoMapper.Side.None, true, false, AvatarAutoMapper.kJawKeywords), new AvatarAutoMapper.BoneMappingItem(10, 22, 1, 2, 0.0f, new Vector3(1f, 1f, 1f), AvatarAutoMapper.Side.Right, true, false, AvatarAutoMapper.kEyeKeywords), new AvatarAutoMapper.BoneMappingItem(18, -2, 1, 2, 0.0f, new Vector3(1f, -1f, 2f), AvatarAutoMapper.Side.Right, true, false, AvatarAutoMapper.kThumbKeywords), new AvatarAutoMapper.BoneMappingItem(18, -3, 1, 2, 0.0f, new Vector3(3f, 0.0f, 1f), AvatarAutoMapper.Side.Right, true, false, AvatarAutoMapper.kIndexFingerKeywords) };
    private static AvatarAutoMapper.BoneMappingItem[] s_LeftMappingDataHand = new AvatarAutoMapper.BoneMappingItem[16]{ new AvatarAutoMapper.BoneMappingItem(-2, -1, 1, 2, 0.0f, AvatarAutoMapper.Side.None, new string[0]), new AvatarAutoMapper.BoneMappingItem(-1, 24, 1, 3, 0.0f, new Vector3(2f, 0.0f, 1f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kThumbKeywords), new AvatarAutoMapper.BoneMappingItem(-1, 27, 1, 3, 0.0f, new Vector3(4f, 0.0f, 1f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kIndexFingerKeywords), new AvatarAutoMapper.BoneMappingItem(-1, 30, 1, 3, 0.0f, new Vector3(4f, 0.0f, 0.0f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kMiddleFingerKeywords), new AvatarAutoMapper.BoneMappingItem(-1, 33, 1, 3, 0.0f, new Vector3(4f, 0.0f, -1f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kRingFingerKeywords), new AvatarAutoMapper.BoneMappingItem(-1, 36, 1, 3, 0.0f, new Vector3(4f, 0.0f, -2f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kLittleFingerKeywords), new AvatarAutoMapper.BoneMappingItem(24, 25, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(27, 28, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(30, 31, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(33, 34, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(36, 37, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(25, 26, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(28, 29, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(31, 32, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(34, 35, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(37, 38, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]) };
    private static AvatarAutoMapper.BoneMappingItem[] s_RightMappingDataHand = new AvatarAutoMapper.BoneMappingItem[16]{ new AvatarAutoMapper.BoneMappingItem(-2, -1, 1, 2, 0.0f, AvatarAutoMapper.Side.None, new string[0]), new AvatarAutoMapper.BoneMappingItem(-1, 39, 1, 3, 0.0f, new Vector3(2f, 0.0f, 1f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kThumbKeywords), new AvatarAutoMapper.BoneMappingItem(-1, 42, 1, 3, 0.0f, new Vector3(4f, 0.0f, 1f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kIndexFingerKeywords), new AvatarAutoMapper.BoneMappingItem(-1, 45, 1, 3, 0.0f, new Vector3(4f, 0.0f, 0.0f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kMiddleFingerKeywords), new AvatarAutoMapper.BoneMappingItem(-1, 48, 1, 3, 0.0f, new Vector3(4f, 0.0f, -1f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kRingFingerKeywords), new AvatarAutoMapper.BoneMappingItem(-1, 51, 1, 3, 0.0f, new Vector3(4f, 0.0f, -2f), AvatarAutoMapper.Side.None, AvatarAutoMapper.kLittleFingerKeywords), new AvatarAutoMapper.BoneMappingItem(39, 40, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(42, 43, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(45, 46, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(48, 49, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(51, 52, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(40, 41, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(43, 44, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(46, 47, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(49, 50, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]), new AvatarAutoMapper.BoneMappingItem(52, 53, 1, 1, 0.0f, AvatarAutoMapper.Side.None, false, true, new string[0]) };
    private static bool s_DidPerformInit = false;
    private const string kLeftMatch = "(^|.*[ \\.:_-])[lL]($|[ \\.:_-].*)";
    private const string kRightMatch = "(^|.*[ \\.:_-])[rR]($|[ \\.:_-].*)";
    private Dictionary<Transform, bool> m_ValidBones;
    private bool m_TreatDummyBonesAsReal;
    private Quaternion m_Orientation;
    private int m_MappingIndexOffset;
    private AvatarAutoMapper.BoneMappingItem[] m_MappingData;
    private Dictionary<string, int> m_BoneHasKeywordDict;
    private Dictionary<string, int> m_BoneHasBadKeywordDict;
    private Dictionary<int, AvatarAutoMapper.BoneMatch> m_BoneMatchDict;

    public AvatarAutoMapper(Dictionary<Transform, bool> validBones)
    {
      this.m_BoneHasKeywordDict = new Dictionary<string, int>();
      this.m_BoneHasBadKeywordDict = new Dictionary<string, int>();
      this.m_BoneMatchDict = new Dictionary<int, AvatarAutoMapper.BoneMatch>();
      this.m_ValidBones = validBones;
    }

    private static int GetLeftBoneIndexFromRight(int rightIndex)
    {
      if (rightIndex < 0)
        return rightIndex;
      if (rightIndex < 54)
        return (int) Enum.Parse(typeof (HumanBodyBones), Enum.GetName(typeof (HumanBodyBones), (object) rightIndex).Replace("Right", "Left"));
      return rightIndex + 24 - 39;
    }

    public static void InitGlobalMappingData()
    {
      if (AvatarAutoMapper.s_DidPerformInit)
        return;
      List<AvatarAutoMapper.BoneMappingItem> boneMappingItemList = new List<AvatarAutoMapper.BoneMappingItem>((IEnumerable<AvatarAutoMapper.BoneMappingItem>) AvatarAutoMapper.s_MappingDataBody);
      int count = boneMappingItemList.Count;
      for (int index = 0; index < count; ++index)
      {
        AvatarAutoMapper.BoneMappingItem boneMappingItem = boneMappingItemList[index];
        if (boneMappingItem.side == AvatarAutoMapper.Side.Right)
        {
          int boneIndexFromRight1 = AvatarAutoMapper.GetLeftBoneIndexFromRight(boneMappingItem.bone);
          int boneIndexFromRight2 = AvatarAutoMapper.GetLeftBoneIndexFromRight(boneMappingItem.parent);
          boneMappingItemList.Add(new AvatarAutoMapper.BoneMappingItem(boneIndexFromRight2, boneIndexFromRight1, boneMappingItem.minStep, boneMappingItem.maxStep, boneMappingItem.lengthRatio, new Vector3(-boneMappingItem.dir.x, boneMappingItem.dir.y, boneMappingItem.dir.z), AvatarAutoMapper.Side.Left, boneMappingItem.optional, boneMappingItem.alwaysInclude, boneMappingItem.keywords));
        }
      }
      AvatarAutoMapper.s_MappingDataBody = boneMappingItemList.ToArray();
      for (int index = 0; index < AvatarAutoMapper.s_MappingDataBody.Length; ++index)
        AvatarAutoMapper.s_MappingDataBody[index].GetChildren(AvatarAutoMapper.s_MappingDataBody);
      for (int index = 0; index < AvatarAutoMapper.s_LeftMappingDataHand.Length; ++index)
        AvatarAutoMapper.s_LeftMappingDataHand[index].GetChildren(AvatarAutoMapper.s_LeftMappingDataHand);
      for (int index = 0; index < AvatarAutoMapper.s_RightMappingDataHand.Length; ++index)
        AvatarAutoMapper.s_RightMappingDataHand[index].GetChildren(AvatarAutoMapper.s_RightMappingDataHand);
      AvatarAutoMapper.s_DidPerformInit = true;
    }

    public static Dictionary<int, Transform> MapBones(Transform root, Dictionary<Transform, bool> validBones)
    {
      return new AvatarAutoMapper(validBones).MapBones(root);
    }

    public Dictionary<int, Transform> MapBones(Transform root)
    {
      AvatarAutoMapper.InitGlobalMappingData();
      Dictionary<int, Transform> mapping = new Dictionary<int, Transform>();
      this.m_Orientation = Quaternion.identity;
      this.m_MappingData = AvatarAutoMapper.s_MappingDataBody;
      this.m_MappingIndexOffset = 0;
      this.m_BoneMatchDict.Clear();
      AvatarAutoMapper.BoneMatch rootMatch1 = new AvatarAutoMapper.BoneMatch((AvatarAutoMapper.BoneMatch) null, root, this.m_MappingData[0]);
      this.m_TreatDummyBonesAsReal = false;
      this.MapBonesFromRootDown(rootMatch1, mapping);
      if (mapping.Count < 15)
      {
        this.m_TreatDummyBonesAsReal = true;
        this.MapBonesFromRootDown(rootMatch1, mapping);
      }
      if (mapping.ContainsKey(1) && mapping.ContainsKey(2) && (mapping.ContainsKey(13) && mapping.ContainsKey(14)))
      {
        this.m_Orientation = AvatarSetupTool.AvatarComputeOrientation(mapping[1].position, mapping[2].position, mapping[13].position, mapping[14].position);
        if ((double) Vector3.Angle(this.m_Orientation * Vector3.up, Vector3.up) > 20.0 || (double) Vector3.Angle(this.m_Orientation * Vector3.forward, Vector3.forward) > 20.0)
        {
          if (AvatarAutoMapper.kDebug)
            Debug.Log((object) "*** Mapping with new computed orientation");
          mapping.Clear();
          this.m_BoneMatchDict.Clear();
          this.MapBonesFromRootDown(rootMatch1, mapping);
        }
      }
      if ((!this.m_ValidBones.ContainsKey(root) ? 0 : (this.m_ValidBones[root] ? 1 : 0)) == 0 && mapping.Count > 0 && mapping.ContainsKey(0))
      {
        while (true)
        {
          Transform parent = mapping[0].parent;
          if ((UnityEngine.Object) parent != (UnityEngine.Object) null && (UnityEngine.Object) parent != (UnityEngine.Object) rootMatch1.bone && (this.m_ValidBones.ContainsKey(parent) && this.m_ValidBones[parent]))
            mapping[0] = parent;
          else
            break;
        }
      }
      int num = 3;
      Quaternion orientation = this.m_Orientation;
      if (mapping.ContainsKey(17))
      {
        Transform bone = mapping[15];
        Transform transform = mapping[17];
        this.m_Orientation = Quaternion.FromToRotation(orientation * -Vector3.right, transform.position - bone.position) * orientation;
        this.m_MappingData = AvatarAutoMapper.s_LeftMappingDataHand;
        this.m_MappingIndexOffset = 24;
        this.m_BoneMatchDict.Clear();
        AvatarAutoMapper.BoneMatch rootMatch2 = new AvatarAutoMapper.BoneMatch((AvatarAutoMapper.BoneMatch) null, bone, this.m_MappingData[0]);
        this.m_TreatDummyBonesAsReal = true;
        int count = mapping.Count;
        this.MapBonesFromRootDown(rootMatch2, mapping);
        if (mapping.Count < count + num)
        {
          for (int key = 24; key <= 38; ++key)
            mapping.Remove(key);
        }
      }
      if (mapping.ContainsKey(18))
      {
        Transform bone = mapping[16];
        Transform transform = mapping[18];
        this.m_Orientation = Quaternion.FromToRotation(orientation * Vector3.right, transform.position - bone.position) * orientation;
        this.m_MappingData = AvatarAutoMapper.s_RightMappingDataHand;
        this.m_MappingIndexOffset = 39;
        this.m_BoneMatchDict.Clear();
        AvatarAutoMapper.BoneMatch rootMatch2 = new AvatarAutoMapper.BoneMatch((AvatarAutoMapper.BoneMatch) null, bone, this.m_MappingData[0]);
        this.m_TreatDummyBonesAsReal = true;
        int count = mapping.Count;
        this.MapBonesFromRootDown(rootMatch2, mapping);
        if (mapping.Count < count + num)
        {
          for (int key = 39; key <= 53; ++key)
            mapping.Remove(key);
        }
      }
      return mapping;
    }

    private void MapBonesFromRootDown(AvatarAutoMapper.BoneMatch rootMatch, Dictionary<int, Transform> mapping)
    {
      List<AvatarAutoMapper.BoneMatch> potentialBoneMatches = this.RecursiveFindPotentialBoneMatches(rootMatch, this.m_MappingData[0], true);
      if (potentialBoneMatches == null || potentialBoneMatches.Count <= 0)
        return;
      if (AvatarAutoMapper.kDebug)
        this.EvaluateBoneMatch(potentialBoneMatches[0], true);
      this.ApplyMapping(potentialBoneMatches[0], mapping);
    }

    private void ApplyMapping(AvatarAutoMapper.BoneMatch match, Dictionary<int, Transform> mapping)
    {
      if (match.doMap)
        mapping[match.item.bone] = match.bone;
      using (List<AvatarAutoMapper.BoneMatch>.Enumerator enumerator = match.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.ApplyMapping(enumerator.Current, mapping);
      }
    }

    private string GetStrippedAndNiceBoneName(Transform bone)
    {
      string[] strArray = bone.name.Split(':');
      return ObjectNames.NicifyVariableName(strArray[strArray.Length - 1]);
    }

    private int BoneHasBadKeyword(Transform bone, params string[] keywords)
    {
      string key = bone.GetInstanceID().ToString() + ":" + string.Concat(keywords);
      if (this.m_BoneHasBadKeywordDict.ContainsKey(key))
        return this.m_BoneHasBadKeywordDict[key];
      int num1 = 0;
      Transform parent = bone.parent;
      while ((UnityEngine.Object) parent.parent != (UnityEngine.Object) null && this.m_ValidBones.ContainsKey(parent) && !this.m_ValidBones[parent])
        parent = parent.parent;
      string lower1 = this.GetStrippedAndNiceBoneName(parent).ToLower();
      foreach (string keyword in keywords)
      {
        if ((int) keyword[0] != 33 && lower1.Contains(keyword))
        {
          int num2 = -20;
          this.m_BoneHasBadKeywordDict[key] = num2;
          return num2;
        }
      }
      string lower2 = this.GetStrippedAndNiceBoneName(bone).ToLower();
      foreach (string keyword in keywords)
      {
        if ((int) keyword[0] == 33 && lower2.Contains(keyword.Substring(1)))
        {
          int num2 = -1000;
          this.m_BoneHasBadKeywordDict[key] = num2;
          return num2;
        }
      }
      this.m_BoneHasBadKeywordDict[key] = num1;
      return num1;
    }

    private int BoneHasKeyword(Transform bone, params string[] keywords)
    {
      string key = bone.GetInstanceID().ToString() + ":" + string.Concat(keywords);
      if (this.m_BoneHasKeywordDict.ContainsKey(key))
        return this.m_BoneHasKeywordDict[key];
      int num1 = 0;
      string lower = this.GetStrippedAndNiceBoneName(bone).ToLower();
      foreach (string keyword in keywords)
      {
        if ((int) keyword[0] != 33 && lower.Contains(keyword))
        {
          int num2 = 20;
          this.m_BoneHasKeywordDict[key] = num2;
          return num2;
        }
      }
      this.m_BoneHasKeywordDict[key] = num1;
      return num1;
    }

    private bool MatchesSideKeywords(string boneName, bool left)
    {
      return boneName.ToLower().Contains(!left ? "right" : "left") || Regex.Match(boneName, !left ? "(^|.*[ \\.:_-])[rR]($|[ \\.:_-].*)" : "(^|.*[ \\.:_-])[lL]($|[ \\.:_-].*)").Length > 0;
    }

    private int GetBoneSideMatchPoints(AvatarAutoMapper.BoneMatch match)
    {
      string name = match.bone.name;
      if (match.item.side == AvatarAutoMapper.Side.None && (this.MatchesSideKeywords(name, false) || this.MatchesSideKeywords(name, true)))
        return -1000;
      bool left = match.item.side == AvatarAutoMapper.Side.Left;
      if (this.MatchesSideKeywords(name, left))
        return 15;
      return this.MatchesSideKeywords(name, !left) ? -1000 : 0;
    }

    private int GetMatchKey(AvatarAutoMapper.BoneMatch parentMatch, Transform t, AvatarAutoMapper.BoneMappingItem goalItem)
    {
      int num = goalItem.bone + t.GetInstanceID() * 1000;
      if (parentMatch != null)
      {
        num += parentMatch.bone.GetInstanceID() * 1000000;
        if (parentMatch.parent != null)
          num += parentMatch.parent.bone.GetInstanceID() * 1000000000;
      }
      return num;
    }

    private List<AvatarAutoMapper.BoneMatch> RecursiveFindPotentialBoneMatches(AvatarAutoMapper.BoneMatch parentMatch, AvatarAutoMapper.BoneMappingItem goalItem, bool confirmedChoice)
    {
      List<AvatarAutoMapper.BoneMatch> matches = new List<AvatarAutoMapper.BoneMatch>();
      Queue<AvatarAutoMapper.QueuedBone> queuedBoneQueue = new Queue<AvatarAutoMapper.QueuedBone>();
      queuedBoneQueue.Enqueue(new AvatarAutoMapper.QueuedBone(parentMatch.bone, 0));
      while (queuedBoneQueue.Count > 0)
      {
        AvatarAutoMapper.QueuedBone queuedBone = queuedBoneQueue.Dequeue();
        Transform bone = queuedBone.bone;
        if (queuedBone.level >= goalItem.minStep && (this.m_TreatDummyBonesAsReal || this.m_ValidBones == null || this.m_ValidBones.ContainsKey(bone) && this.m_ValidBones[bone]))
        {
          int matchKey = this.GetMatchKey(parentMatch, bone, goalItem);
          AvatarAutoMapper.BoneMatch match;
          if (this.m_BoneMatchDict.ContainsKey(matchKey))
          {
            match = this.m_BoneMatchDict[matchKey];
          }
          else
          {
            match = new AvatarAutoMapper.BoneMatch(parentMatch, bone, goalItem);
            this.EvaluateBoneMatch(match, false);
            this.m_BoneMatchDict[matchKey] = match;
          }
          if ((double) match.score > 0.0 || AvatarAutoMapper.kDebug)
            matches.Add(match);
        }
        if (queuedBone.level < goalItem.maxStep)
        {
          foreach (Transform index in bone)
          {
            if (this.m_ValidBones == null || this.m_ValidBones.ContainsKey(index))
            {
              if (!this.m_TreatDummyBonesAsReal && this.m_ValidBones != null && !this.m_ValidBones[index])
                queuedBoneQueue.Enqueue(new AvatarAutoMapper.QueuedBone(index, queuedBone.level));
              else
                queuedBoneQueue.Enqueue(new AvatarAutoMapper.QueuedBone(index, queuedBone.level + 1));
            }
          }
        }
      }
      if (matches.Count == 0)
        return (List<AvatarAutoMapper.BoneMatch>) null;
      matches.Sort();
      if ((double) matches[0].score <= 0.0)
        return (List<AvatarAutoMapper.BoneMatch>) null;
      if (AvatarAutoMapper.kDebug && confirmedChoice)
        this.DebugMatchChoice(matches);
      while (matches.Count > 3)
        matches.RemoveAt(matches.Count - 1);
      matches.TrimExcess();
      return matches;
    }

    private string GetNameOfBone(int boneIndex)
    {
      if (boneIndex < 0)
        return string.Empty + (object) boneIndex;
      return string.Empty + (object) (HumanBodyBones) boneIndex;
    }

    private string GetMatchString(AvatarAutoMapper.BoneMatch match)
    {
      return this.GetNameOfBone(match.item.bone) + ":" + (!((UnityEngine.Object) match.bone == (UnityEngine.Object) null) ? match.bone.name : "null");
    }

    private void DebugMatchChoice(List<AvatarAutoMapper.BoneMatch> matches)
    {
      string str = this.GetNameOfBone(matches[0].item.bone) + " preferred order: ";
      for (int index = 0; index < matches.Count; ++index)
        str = str + matches[index].bone.name + " (" + matches[index].score.ToString("0.0") + " / " + matches[index].totalSiblingScore.ToString("0.0") + "), ";
      using (List<AvatarAutoMapper.BoneMatch>.Enumerator enumerator1 = matches.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          AvatarAutoMapper.BoneMatch current1 = enumerator1.Current;
          str = str + "\n   Match " + current1.bone.name + " (" + current1.score.ToString("0.0") + " / " + current1.totalSiblingScore.ToString("0.0") + "):";
          using (List<string>.Enumerator enumerator2 = current1.debugTracker.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              string current2 = enumerator2.Current;
              str = str + "\n    - " + current2;
            }
          }
        }
      }
      Debug.Log((object) str);
    }

    private AvatarAutoMapper.BoneMappingItem GetBoneMappingItem(int bone)
    {
      foreach (AvatarAutoMapper.BoneMappingItem boneMappingItem in this.m_MappingData)
      {
        if (boneMappingItem.bone == bone)
          return boneMappingItem;
      }
      return new AvatarAutoMapper.BoneMappingItem();
    }

    private bool IsParentOfOther(Transform knownCommonParent, Transform potentialParent, Transform potentialChild)
    {
      for (Transform transform = potentialChild; (UnityEngine.Object) transform != (UnityEngine.Object) knownCommonParent; transform = transform.parent)
      {
        if ((UnityEngine.Object) transform == (UnityEngine.Object) potentialParent)
          return true;
        if ((UnityEngine.Object) transform == (UnityEngine.Object) knownCommonParent)
          return false;
      }
      return false;
    }

    private bool ShareTransformPath(Transform commonParent, Transform childA, Transform childB)
    {
      if (!this.IsParentOfOther(commonParent, childA, childB))
        return this.IsParentOfOther(commonParent, childB, childA);
      return true;
    }

    private List<AvatarAutoMapper.BoneMatch> GetBestChildMatches(AvatarAutoMapper.BoneMatch parentMatch, List<List<AvatarAutoMapper.BoneMatch>> childMatchesLists)
    {
      List<AvatarAutoMapper.BoneMatch> boneMatchList = new List<AvatarAutoMapper.BoneMatch>();
      if (childMatchesLists.Count == 1)
      {
        boneMatchList.Add(childMatchesLists[0][0]);
        return boneMatchList;
      }
      int[] choices = new int[childMatchesLists.Count];
      float score;
      int[] childMatchChoices = this.GetBestChildMatchChoices(parentMatch, childMatchesLists, choices, out score);
      for (int index = 0; index < childMatchChoices.Length; ++index)
      {
        if (childMatchChoices[index] >= 0)
          boneMatchList.Add(childMatchesLists[index][childMatchChoices[index]]);
      }
      return boneMatchList;
    }

    private int[] GetBestChildMatchChoices(AvatarAutoMapper.BoneMatch parentMatch, List<List<AvatarAutoMapper.BoneMatch>> childMatchesLists, int[] choices, out float score)
    {
      List<int> intList = new List<int>();
      for (int index1 = 0; index1 < choices.Length; ++index1)
      {
        if (choices[index1] >= 0)
        {
          intList.Clear();
          intList.Add(index1);
          for (int index2 = index1 + 1; index2 < choices.Length; ++index2)
          {
            if (choices[index2] >= 0 && this.ShareTransformPath(parentMatch.bone, childMatchesLists[index1][choices[index1]].bone, childMatchesLists[index2][choices[index2]].bone))
              intList.Add(index2);
          }
          if (intList.Count > 1)
            break;
        }
      }
      if (intList.Count <= 1)
      {
        score = 0.0f;
        for (int index = 0; index < choices.Length; ++index)
        {
          if (choices[index] >= 0)
            score = score + childMatchesLists[index][choices[index]].totalSiblingScore;
        }
        return choices;
      }
      float num = 0.0f;
      int[] numArray = choices;
      for (int index1 = 0; index1 < intList.Count; ++index1)
      {
        int[] choices1 = new int[choices.Length];
        Array.Copy((Array) choices, (Array) choices1, choices.Length);
        for (int index2 = 0; index2 < intList.Count; ++index2)
        {
          if (index1 != index2)
          {
            if (intList[index2] >= choices1.Length)
              Debug.LogError((object) ("sharedIndices[j] (" + (object) intList[index2] + ") >= altChoices.Length (" + (object) choices1.Length + ")"));
            if (intList[index2] >= childMatchesLists.Count)
              Debug.LogError((object) ("sharedIndices[j] (" + (object) intList[index2] + ") >= childMatchesLists.Count (" + (object) childMatchesLists.Count + ")"));
            if (choices1[intList[index2]] < childMatchesLists[intList[index2]].Count - 1)
              ++choices1[intList[index2]];
            else
              choices1[intList[index2]] = -1;
          }
        }
        float score1;
        int[] childMatchChoices = this.GetBestChildMatchChoices(parentMatch, childMatchesLists, choices1, out score1);
        if ((double) score1 > (double) num)
        {
          num = score1;
          numArray = childMatchChoices;
        }
      }
      score = num;
      return numArray;
    }

    private void EvaluateBoneMatch(AvatarAutoMapper.BoneMatch match, bool confirmedChoice)
    {
      match.score = 0.0f;
      match.siblingScore = 0.0f;
      List<List<AvatarAutoMapper.BoneMatch>> childMatchesLists = new List<List<AvatarAutoMapper.BoneMatch>>();
      int num1 = 0;
      foreach (int child in match.item.GetChildren(this.m_MappingData))
      {
        AvatarAutoMapper.BoneMappingItem goalItem = this.m_MappingData[child];
        if (goalItem.parent == match.item.bone)
        {
          ++num1;
          List<AvatarAutoMapper.BoneMatch> potentialBoneMatches = this.RecursiveFindPotentialBoneMatches(match, goalItem, confirmedChoice);
          if (potentialBoneMatches != null && potentialBoneMatches.Count != 0)
            childMatchesLists.Add(potentialBoneMatches);
        }
      }
      bool flag = (UnityEngine.Object) match.bone == (UnityEngine.Object) match.humanBoneParent.bone;
      int num2 = 0;
      if (childMatchesLists.Count > 0)
      {
        match.children = this.GetBestChildMatches(match, childMatchesLists);
        using (List<AvatarAutoMapper.BoneMatch>.Enumerator enumerator = match.children.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AvatarAutoMapper.BoneMatch current = enumerator.Current;
            if (AvatarAutoMapper.kDebug && confirmedChoice)
              this.EvaluateBoneMatch(current, confirmedChoice);
            ++num2;
            match.score += current.score;
            if (AvatarAutoMapper.kDebug)
              match.debugTracker.AddRange((IEnumerable<string>) current.debugTracker);
            if ((UnityEngine.Object) current.bone == (UnityEngine.Object) match.bone && current.item.bone >= 0)
              flag = true;
          }
        }
      }
      if (!match.item.optional || !flag)
        this.ScoreBoneMatch(match);
      if (match.item.dir != Vector3.zero)
      {
        Vector3 dir = match.item.dir;
        if (this.m_MappingIndexOffset >= 24 && this.m_MappingIndexOffset < 39)
          dir.x *= -1f;
        float num3 = Vector3.Dot(Quaternion.Inverse(this.m_Orientation) * (match.bone.position - match.humanBoneParent.bone.position).normalized, dir) * (!match.item.optional ? 10f : 5f);
        match.siblingScore += num3;
        if (AvatarAutoMapper.kDebug)
          match.debugTracker.Add("* " + (object) num3 + ": " + this.GetMatchString(match) + " matched dir (" + (object) (match.bone.position - match.humanBoneParent.bone.position).normalized + " , " + (object) dir + ")");
        if ((double) num3 > 0.0)
        {
          match.score += 10f;
          if (AvatarAutoMapper.kDebug)
            match.debugTracker.Add(10.ToString() + ": " + this.GetMatchString(match) + " matched dir (" + (object) (match.bone.position - match.humanBoneParent.bone.position).normalized + " , " + (object) dir + ")");
        }
      }
      if (this.m_MappingIndexOffset == 0)
      {
        int boneSideMatchPoints = this.GetBoneSideMatchPoints(match);
        if (match.parent.item.side == AvatarAutoMapper.Side.None || boneSideMatchPoints < 0)
        {
          match.siblingScore += (float) boneSideMatchPoints;
          if (AvatarAutoMapper.kDebug)
            match.debugTracker.Add("* " + (object) boneSideMatchPoints + ": " + this.GetMatchString(match) + " matched side");
        }
      }
      if ((double) match.score > 0.0)
      {
        if (match.item.optional && !flag)
        {
          match.score += 5f;
          if (AvatarAutoMapper.kDebug)
            match.debugTracker.Add(5.ToString() + ": " + this.GetMatchString(match) + " optional bone is included");
        }
        if (num1 == 0 && match.bone.childCount > 0)
        {
          ++match.score;
          if (AvatarAutoMapper.kDebug)
            match.debugTracker.Add(1.ToString() + ": " + this.GetMatchString(match) + " has dummy child bone");
        }
        if ((double) match.item.lengthRatio != 0.0)
        {
          float num3 = Vector3.Distance(match.bone.position, match.humanBoneParent.bone.position);
          if ((double) num3 == 0.0 && (UnityEngine.Object) match.bone != (UnityEngine.Object) match.humanBoneParent.bone)
          {
            match.score -= 1000f;
            if (AvatarAutoMapper.kDebug)
              match.debugTracker.Add(-1000.ToString() + ": " + this.GetMatchString(match.humanBoneParent) + " has zero length");
          }
          float num4 = Vector3.Distance(match.humanBoneParent.bone.position, match.humanBoneParent.humanBoneParent.bone.position);
          if ((double) num4 > 0.0)
          {
            float num5 = Mathf.Log(num3 / num4, 2f);
            float num6 = Mathf.Log(match.item.lengthRatio, 2f);
            float num7 = 10f * Mathf.Clamp((float) (1.0 - 0.600000023841858 * (double) Mathf.Abs(num5 - num6)), 0.0f, 1f);
            match.score += num7;
            if (AvatarAutoMapper.kDebug)
              match.debugTracker.Add(((double) num7).ToString() + ": parent " + this.GetMatchString(match.humanBoneParent) + " matched lengthRatio - " + (object) num3 + " / " + (object) num4 + " = " + (object) (float) ((double) num3 / (double) num4) + " (" + (object) num5 + ") goal: " + (object) match.item.lengthRatio + " (" + (object) num6 + ")");
          }
        }
      }
      if (match.item.bone < 0 || match.item.optional && flag)
        return;
      match.doMap = true;
    }

    private void ScoreBoneMatch(AvatarAutoMapper.BoneMatch match)
    {
      int num1 = this.BoneHasBadKeyword(match.bone, match.item.keywords);
      match.score += (float) num1;
      if (AvatarAutoMapper.kDebug && num1 != 0)
        match.debugTracker.Add(num1.ToString() + ": " + this.GetMatchString(match) + " matched bad keywords");
      if (num1 < 0)
        return;
      int num2 = this.BoneHasKeyword(match.bone, match.item.keywords);
      match.score += (float) num2;
      if (AvatarAutoMapper.kDebug && num2 != 0)
        match.debugTracker.Add(num2.ToString() + ": " + this.GetMatchString(match) + " matched keywords");
      if (match.item.keywords.Length != 0 || !match.item.alwaysInclude)
        return;
      ++match.score;
      if (!AvatarAutoMapper.kDebug)
        return;
      match.debugTracker.Add(1.ToString() + ": " + this.GetMatchString(match) + " always-include point");
    }

    private enum Side
    {
      None,
      Left,
      Right,
    }

    private struct BoneMappingItem
    {
      public int parent;
      public int bone;
      public int minStep;
      public int maxStep;
      public float lengthRatio;
      public Vector3 dir;
      public AvatarAutoMapper.Side side;
      public bool optional;
      public bool alwaysInclude;
      public string[] keywords;
      private int[] children;

      public BoneMappingItem(int parent, int bone, int minStep, int maxStep, float lengthRatio, Vector3 dir, AvatarAutoMapper.Side side, bool optional, bool alwaysInclude, params string[] keywords)
      {
        this.parent = parent;
        this.bone = bone;
        this.minStep = minStep;
        this.maxStep = maxStep;
        this.lengthRatio = lengthRatio;
        this.dir = dir;
        this.side = side;
        this.optional = optional;
        this.alwaysInclude = alwaysInclude;
        this.keywords = keywords;
        this.children = (int[]) null;
      }

      public BoneMappingItem(int parent, int bone, int minStep, int maxStep, float lengthRatio, AvatarAutoMapper.Side side, bool optional, bool alwaysInclude, params string[] keywords)
      {
        this = new AvatarAutoMapper.BoneMappingItem(parent, bone, minStep, maxStep, lengthRatio, Vector3.zero, side, optional, alwaysInclude, keywords);
      }

      public BoneMappingItem(int parent, int bone, int minStep, int maxStep, float lengthRatio, Vector3 dir, AvatarAutoMapper.Side side, params string[] keywords)
      {
        this = new AvatarAutoMapper.BoneMappingItem(parent, bone, minStep, maxStep, lengthRatio, dir, side, false, false, keywords);
      }

      public BoneMappingItem(int parent, int bone, int minStep, int maxStep, float lengthRatio, AvatarAutoMapper.Side side, params string[] keywords)
      {
        this = new AvatarAutoMapper.BoneMappingItem(parent, bone, minStep, maxStep, lengthRatio, Vector3.zero, side, false, false, keywords);
      }

      public int[] GetChildren(AvatarAutoMapper.BoneMappingItem[] mappingData)
      {
        if (this.children == null)
        {
          List<int> intList = new List<int>();
          for (int index = 0; index < mappingData.Length; ++index)
          {
            if (mappingData[index].parent == this.bone)
              intList.Add(index);
          }
          this.children = intList.ToArray();
        }
        return this.children;
      }
    }

    private class BoneMatch : IComparable<AvatarAutoMapper.BoneMatch>
    {
      public List<AvatarAutoMapper.BoneMatch> children = new List<AvatarAutoMapper.BoneMatch>();
      public List<string> debugTracker = new List<string>();
      public AvatarAutoMapper.BoneMatch parent;
      public bool doMap;
      public AvatarAutoMapper.BoneMappingItem item;
      public Transform bone;
      public float score;
      public float siblingScore;

      public AvatarAutoMapper.BoneMatch humanBoneParent
      {
        get
        {
          AvatarAutoMapper.BoneMatch parent = this.parent;
          while (parent.parent != null && parent.item.bone < 0)
            parent = parent.parent;
          return parent;
        }
      }

      public float totalSiblingScore
      {
        get
        {
          return this.score + this.siblingScore;
        }
      }

      public BoneMatch(AvatarAutoMapper.BoneMatch parent, Transform bone, AvatarAutoMapper.BoneMappingItem item)
      {
        this.parent = parent;
        this.bone = bone;
        this.item = item;
      }

      public int CompareTo(AvatarAutoMapper.BoneMatch other)
      {
        if (other == null)
          return 1;
        return other.totalSiblingScore.CompareTo(this.totalSiblingScore);
      }
    }

    private struct QueuedBone
    {
      public Transform bone;
      public int level;

      public QueuedBone(Transform bone, int level)
      {
        this.bone = bone;
        this.level = level;
      }
    }
  }
}
