// Decompiled with JetBrains decompiler
// Type: UnityEditor.SketchUpNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor
{
  internal class SketchUpNode : TreeViewItem
  {
    public SketchUpNodeInfo Info;

    public bool Enabled
    {
      get
      {
        return this.Info.enabled;
      }
      set
      {
        if (this.Info.enabled == value)
          return;
        if (value)
          this.ToggleParent(value);
        this.ToggleChildren(value);
        this.Info.enabled = value;
      }
    }

    public SketchUpNode(int id, int depth, TreeViewItem parent, string displayName, SketchUpNodeInfo info)
      : base(id, depth, parent, displayName)
    {
      this.Info = info;
      this.children = new List<TreeViewItem>();
    }

    private void ToggleParent(bool toggle)
    {
      SketchUpNode parent = this.parent as SketchUpNode;
      if (parent == null)
        return;
      parent.ToggleParent(toggle);
      parent.Info.enabled = toggle;
    }

    private void ToggleChildren(bool toggle)
    {
      using (List<TreeViewItem>.Enumerator enumerator = this.children.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SketchUpNode current = enumerator.Current as SketchUpNode;
          current.Info.enabled = toggle;
          current.ToggleChildren(toggle);
        }
      }
    }
  }
}
