// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.MecanimUtilities
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Animations
{
  internal class MecanimUtilities
  {
    public static bool StateMachineRelativePath(AnimatorStateMachine parent, AnimatorStateMachine toFind, ref List<AnimatorStateMachine> hierarchy)
    {
      hierarchy.Add(parent);
      if ((Object) parent == (Object) toFind)
        return true;
      for (int index = 0; index < parent.stateMachines.Length; ++index)
      {
        if (MecanimUtilities.StateMachineRelativePath(parent.stateMachines[index].stateMachine, toFind, ref hierarchy))
          return true;
      }
      hierarchy.Remove(parent);
      return false;
    }

    internal static bool AreSameAsset(Object obj1, Object obj2)
    {
      return AssetDatabase.GetAssetPath(obj1) == AssetDatabase.GetAssetPath(obj2);
    }

    internal static void DestroyBlendTreeRecursive(BlendTree blendTree)
    {
      for (int index = 0; index < blendTree.children.Length; ++index)
      {
        BlendTree motion = blendTree.children[index].motion as BlendTree;
        if ((Object) motion != (Object) null && MecanimUtilities.AreSameAsset((Object) blendTree, (Object) motion))
          MecanimUtilities.DestroyBlendTreeRecursive(motion);
      }
      Undo.DestroyObjectImmediate((Object) blendTree);
    }
  }
}
