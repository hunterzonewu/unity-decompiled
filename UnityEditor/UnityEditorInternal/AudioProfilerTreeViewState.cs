// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerTreeViewState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AudioProfilerTreeViewState : TreeViewState
  {
    [SerializeField]
    public int selectedColumn = 3;
    [SerializeField]
    public int prevSelectedColumn = 5;
    [SerializeField]
    public bool sortByDescendingOrder = true;

    public void SetSelectedColumn(int index)
    {
      if (index != this.selectedColumn)
        this.prevSelectedColumn = this.selectedColumn;
      else
        this.sortByDescendingOrder = !this.sortByDescendingOrder;
      this.selectedColumn = index;
    }
  }
}
