// Decompiled with JetBrains decompiler
// Type: UnityEditor.TreeViewTests.TreeViewColumnHeader
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.TreeViewTests
{
  internal class TreeViewColumnHeader
  {
    public float[] columnWidths { get; set; }

    public float minColumnWidth { get; set; }

    public float dragWidth { get; set; }

    public System.Action<int, Rect> columnRenderer { get; set; }

    public TreeViewColumnHeader()
    {
      this.minColumnWidth = 10f;
      this.dragWidth = 6f;
    }

    public void OnGUI(Rect rect)
    {
      float x1 = rect.x;
      for (int index = 0; index < this.columnWidths.Length; ++index)
      {
        Rect rect1 = new Rect(x1, rect.y, this.columnWidths[index], rect.height);
        x1 += this.columnWidths[index];
        Rect position = new Rect(x1 - this.dragWidth / 2f, rect.y, 3f, rect.height);
        float x2 = EditorGUI.MouseDeltaReader(position, true).x;
        if ((double) x2 != 0.0)
        {
          this.columnWidths[index] += x2;
          this.columnWidths[index] = Mathf.Max(this.columnWidths[index], this.minColumnWidth);
        }
        if (this.columnRenderer != null)
          this.columnRenderer(index, rect1);
        if (Event.current.type == EventType.Repaint)
          EditorGUIUtility.AddCursorRect(position, MouseCursor.SplitResizeLeftRight);
      }
    }
  }
}
