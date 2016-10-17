// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyPropertyGroupNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyPropertyGroupNode : AnimationWindowHierarchyNode
  {
    public AnimationWindowHierarchyPropertyGroupNode(System.Type animatableObjectType, string propertyName, string path, TreeViewItem parent)
      : base(AnimationWindowUtility.GetPropertyNodeID(path, animatableObjectType, propertyName), parent == null ? -1 : parent.depth + 1, parent, animatableObjectType, AnimationWindowUtility.GetPropertyGroupName(propertyName), path, AnimationWindowUtility.GetNicePropertyGroupDisplayName(animatableObjectType, propertyName))
    {
    }
  }
}
