// Decompiled with JetBrains decompiler
// Type: UnityEditor.IncrementalInitialize
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class IncrementalInitialize
  {
    [SerializeField]
    private IncrementalInitialize.State m_InitState;
    [NonSerialized]
    private bool m_IncrementOnNextEvent;

    public IncrementalInitialize.State state
    {
      get
      {
        return this.m_InitState;
      }
    }

    public void Restart()
    {
      this.m_InitState = IncrementalInitialize.State.PreInitialize;
    }

    public void OnEvent()
    {
      if (this.m_IncrementOnNextEvent)
      {
        ++this.m_InitState;
        this.m_IncrementOnNextEvent = false;
      }
      switch (this.m_InitState)
      {
        case IncrementalInitialize.State.PreInitialize:
          if (Event.current.type != EventType.Repaint)
            break;
          this.m_IncrementOnNextEvent = true;
          HandleUtility.Repaint();
          break;
        case IncrementalInitialize.State.Initialize:
          this.m_IncrementOnNextEvent = true;
          break;
      }
    }

    public enum State
    {
      PreInitialize,
      Initialize,
      Initialized,
    }
  }
}
