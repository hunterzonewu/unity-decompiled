// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewAnimationInput
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TreeViewAnimationInput
  {
    public System.Action<TreeViewAnimationInput> animationEnded;

    public float elapsedTimeNormalized
    {
      get
      {
        return Mathf.Clamp01((float) this.elapsedTime / (float) this.animationDuration);
      }
    }

    public double elapsedTime
    {
      get
      {
        return EditorApplication.timeSinceStartup - this.startTime;
      }
      set
      {
        this.startTime = EditorApplication.timeSinceStartup - value;
      }
    }

    public int startRow { get; set; }

    public int endRow { get; set; }

    public Rect rowsRect { get; set; }

    public Rect startRowRect { get; set; }

    public double startTime { get; set; }

    public double animationDuration { get; set; }

    public bool expanding { get; set; }

    public TreeViewItem item { get; set; }

    public TreeView treeView { get; set; }

    public void FireAnimationEndedEvent()
    {
      if (this.animationEnded == null)
        return;
      this.animationEnded(this);
    }

    public override string ToString()
    {
      return "Input: startRow " + (object) this.startRow + " endRow " + (object) this.endRow + " rowsRect " + (object) this.rowsRect + " startTime " + (object) this.startTime + " anitmationDuration" + (object) this.animationDuration + " " + (object) this.expanding + " " + this.item.displayName;
    }
  }
}
