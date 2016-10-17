// Decompiled with JetBrains decompiler
// Type: UnityEditor.ITreeViewDragging
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal interface ITreeViewDragging
  {
    bool drawRowMarkerAbove { get; set; }

    void OnInitialize();

    bool CanStartDrag(TreeViewItem targetItem, List<int> draggedItemIDs, Vector2 mouseDownPosition);

    void StartDrag(TreeViewItem draggedItem, List<int> draggedItemIDs);

    bool DragElement(TreeViewItem targetItem, Rect targetItemRect, bool firstItem);

    void DragCleanup(bool revertExpanded);

    int GetDropTargetControlID();

    int GetRowMarkerControlID();
  }
}
