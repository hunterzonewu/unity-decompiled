// Decompiled with JetBrains decompiler
// Type: UnityEditor.DrivenRectTransformUndo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [InitializeOnLoad]
  internal class DrivenRectTransformUndo
  {
    static DrivenRectTransformUndo()
    {
      Undo.willFlushUndoRecord += new Undo.WillFlushUndoRecord(DrivenRectTransformUndo.ForceUpdateCanvases);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(DrivenRectTransformUndo.ForceUpdateCanvases);
    }

    private static void ForceUpdateCanvases()
    {
      Canvas.ForceUpdateCanvases();
    }
  }
}
