// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarMaskUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor.Animations;

namespace UnityEditor
{
  internal class AvatarMaskUtility
  {
    private static string sHuman = "m_HumanDescription.m_Human";
    private static string sBoneName = "m_BoneName";

    public static string[] GetAvatarHumanTransform(SerializedObject so, string[] refTransformsPath)
    {
      SerializedProperty property = so.FindProperty(AvatarMaskUtility.sHuman);
      if (property == null || !property.isArray)
        return (string[]) null;
      string[] array = new string[0];
      for (int index = 0; index < property.arraySize; ++index)
      {
        SerializedProperty propertyRelative = property.GetArrayElementAtIndex(index).FindPropertyRelative(AvatarMaskUtility.sBoneName);
        ArrayUtility.Add<string>(ref array, propertyRelative.stringValue);
      }
      return AvatarMaskUtility.TokeniseHumanTransformsPath(refTransformsPath, array);
    }

    public static void UpdateTransformMask(AvatarMask mask, string[] refTransformsPath, string[] humanTransforms)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey87 maskCAnonStorey87 = new AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey87();
      // ISSUE: reference to a compiler-generated field
      maskCAnonStorey87.refTransformsPath = refTransformsPath;
      // ISSUE: reference to a compiler-generated field
      mask.transformCount = maskCAnonStorey87.refTransformsPath.Length;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey88 maskCAnonStorey88 = new AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey88();
      // ISSUE: reference to a compiler-generated field
      maskCAnonStorey88.\u003C\u003Ef__ref\u0024135 = maskCAnonStorey87;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (maskCAnonStorey88.i = 0; maskCAnonStorey88.i < maskCAnonStorey87.refTransformsPath.Length; maskCAnonStorey88.i = maskCAnonStorey88.i + 1)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        mask.SetTransformPath(maskCAnonStorey88.i, maskCAnonStorey87.refTransformsPath[maskCAnonStorey88.i]);
        // ISSUE: reference to a compiler-generated method
        bool flag = humanTransforms == null || ArrayUtility.FindIndex<string>(humanTransforms, new Predicate<string>(maskCAnonStorey88.\u003C\u003Em__149)) != -1;
        // ISSUE: reference to a compiler-generated field
        mask.SetTransformActive(maskCAnonStorey88.i, flag);
      }
    }

    public static void UpdateTransformMask(SerializedProperty transformMask, string[] refTransformsPath, string[] humanTransforms)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey89 maskCAnonStorey89 = new AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey89();
      // ISSUE: reference to a compiler-generated field
      maskCAnonStorey89.refTransformsPath = refTransformsPath;
      transformMask.ClearArray();
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey8A maskCAnonStorey8A = new AvatarMaskUtility.\u003CUpdateTransformMask\u003Ec__AnonStorey8A();
      // ISSUE: reference to a compiler-generated field
      maskCAnonStorey8A.\u003C\u003Ef__ref\u0024137 = maskCAnonStorey89;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (maskCAnonStorey8A.i = 0; maskCAnonStorey8A.i < maskCAnonStorey89.refTransformsPath.Length; maskCAnonStorey8A.i = maskCAnonStorey8A.i + 1)
      {
        // ISSUE: reference to a compiler-generated field
        transformMask.InsertArrayElementAtIndex(maskCAnonStorey8A.i);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        transformMask.GetArrayElementAtIndex(maskCAnonStorey8A.i).FindPropertyRelative("m_Path").stringValue = maskCAnonStorey89.refTransformsPath[maskCAnonStorey8A.i];
        // ISSUE: reference to a compiler-generated method
        bool flag = humanTransforms == null || ArrayUtility.FindIndex<string>(humanTransforms, new Predicate<string>(maskCAnonStorey8A.\u003C\u003Em__14A)) != -1;
        // ISSUE: reference to a compiler-generated field
        transformMask.GetArrayElementAtIndex(maskCAnonStorey8A.i).FindPropertyRelative("m_Weight").floatValue = !flag ? 0.0f : 1f;
      }
    }

    public static void SetActiveHumanTransforms(AvatarMask mask, string[] humanTransforms)
    {
      for (int index = 0; index < mask.transformCount; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated method
        if (ArrayUtility.FindIndex<string>(humanTransforms, new Predicate<string>(new AvatarMaskUtility.\u003CSetActiveHumanTransforms\u003Ec__AnonStorey8B() { path = mask.GetTransformPath(index) }.\u003C\u003Em__14B)) != -1)
          mask.SetTransformActive(index, true);
      }
    }

    private static string[] TokeniseHumanTransformsPath(string[] refTransformsPath, string[] humanTransforms)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey8C pathCAnonStorey8C = new AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey8C();
      // ISSUE: reference to a compiler-generated field
      pathCAnonStorey8C.humanTransforms = humanTransforms;
      // ISSUE: reference to a compiler-generated field
      if (pathCAnonStorey8C.humanTransforms == null)
        return (string[]) null;
      string[] array = new string[1]{ string.Empty };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey8D pathCAnonStorey8D = new AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey8D();
      // ISSUE: reference to a compiler-generated field
      pathCAnonStorey8D.\u003C\u003Ef__ref\u0024140 = pathCAnonStorey8C;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (pathCAnonStorey8D.i = 0; pathCAnonStorey8D.i < pathCAnonStorey8C.humanTransforms.Length; pathCAnonStorey8D.i = pathCAnonStorey8D.i + 1)
      {
        // ISSUE: reference to a compiler-generated method
        int index = ArrayUtility.FindIndex<string>(refTransformsPath, new Predicate<string>(pathCAnonStorey8D.\u003C\u003Em__14C));
        if (index != -1)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey8E pathCAnonStorey8E = new AvatarMaskUtility.\u003CTokeniseHumanTransformsPath\u003Ec__AnonStorey8E();
          int length = array.Length;
          int num;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (pathCAnonStorey8E.path = refTransformsPath[index]; pathCAnonStorey8E.path.Length > 0; pathCAnonStorey8E.path = pathCAnonStorey8E.path.Substring(0, num == -1 ? 0 : num))
          {
            // ISSUE: reference to a compiler-generated method
            if (ArrayUtility.FindIndex<string>(array, new Predicate<string>(pathCAnonStorey8E.\u003C\u003Em__14D)) == -1)
            {
              // ISSUE: reference to a compiler-generated field
              ArrayUtility.Insert<string>(ref array, length, pathCAnonStorey8E.path);
            }
            // ISSUE: reference to a compiler-generated field
            num = pathCAnonStorey8E.path.LastIndexOf('/');
          }
        }
      }
      return array;
    }
  }
}
