// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyPropertyNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyPropertyNode : AnimationWindowHierarchyNode
  {
    public bool isPptrNode;

    public AnimationWindowHierarchyPropertyNode(System.Type animatableObjectType, string propertyName, string path, TreeViewItem parent, EditorCurveBinding binding, bool isPptrNode)
      : base(AnimationWindowUtility.GetPropertyNodeID(path, animatableObjectType, propertyName), parent == null ? -1 : parent.depth + 1, parent, animatableObjectType, propertyName, path, AnimationWindowUtility.GetNicePropertyDisplayName(animatableObjectType, propertyName))
    {
      this.binding = new EditorCurveBinding?(binding);
      this.isPptrNode = isPptrNode;
    }
  }
}
