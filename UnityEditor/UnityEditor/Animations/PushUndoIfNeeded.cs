// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.PushUndoIfNeeded
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Animations
{
  internal struct PushUndoIfNeeded
  {
    private PushUndoIfNeeded.PushUndoIfNeededImpl m_Impl;

    public bool pushUndo
    {
      get
      {
        return this.impl.m_PushUndo;
      }
      set
      {
        this.impl.m_PushUndo = value;
      }
    }

    private PushUndoIfNeeded.PushUndoIfNeededImpl impl
    {
      get
      {
        if (this.m_Impl == null)
          this.m_Impl = new PushUndoIfNeeded.PushUndoIfNeededImpl(true);
        return this.m_Impl;
      }
    }

    public PushUndoIfNeeded(bool pushUndo)
    {
      this.m_Impl = new PushUndoIfNeeded.PushUndoIfNeededImpl(pushUndo);
    }

    public void DoUndo(Object target, string undoOperation)
    {
      this.impl.DoUndo(target, undoOperation);
    }

    private class PushUndoIfNeededImpl
    {
      public bool m_PushUndo;

      public PushUndoIfNeededImpl(bool pushUndo)
      {
        this.m_PushUndo = pushUndo;
      }

      public void DoUndo(Object target, string undoOperation)
      {
        if (!this.m_PushUndo)
          return;
        Undo.RegisterCompleteObjectUndo(target, undoOperation);
      }
    }
  }
}
