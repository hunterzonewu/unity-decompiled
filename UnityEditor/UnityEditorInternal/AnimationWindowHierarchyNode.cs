// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;

namespace UnityEditorInternal
{
  internal class AnimationWindowHierarchyNode : TreeViewItem
  {
    public string path;
    public System.Type animatableObjectType;
    public string propertyName;
    public EditorCurveBinding? binding;
    public AnimationWindowCurve[] curves;
    public float? topPixel;
    public int indent;

    public AnimationWindowHierarchyNode(int instanceID, int depth, TreeViewItem parent, System.Type animatableObjectType, string propertyName, string path, string displayName)
      : base(instanceID, depth, parent, displayName)
    {
      this.displayName = displayName;
      this.animatableObjectType = animatableObjectType;
      this.propertyName = propertyName;
      this.path = path;
    }
  }
}
