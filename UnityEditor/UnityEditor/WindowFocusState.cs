// Decompiled with JetBrains decompiler
// Type: UnityEditor.WindowFocusState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class WindowFocusState : ScriptableObject
  {
    internal string m_LastWindowTypeInSameDock = string.Empty;
    private static WindowFocusState m_Instance;
    internal bool m_WasMaximizedBeforePlay;
    internal bool m_CurrentlyInPlayMode;

    internal static WindowFocusState instance
    {
      get
      {
        if ((Object) WindowFocusState.m_Instance == (Object) null)
          WindowFocusState.m_Instance = Object.FindObjectOfType(typeof (WindowFocusState)) as WindowFocusState;
        if ((Object) WindowFocusState.m_Instance == (Object) null)
          WindowFocusState.m_Instance = ScriptableObject.CreateInstance<WindowFocusState>();
        return WindowFocusState.m_Instance;
      }
    }

    private void OnEnable()
    {
      this.hideFlags = HideFlags.HideAndDontSave;
      WindowFocusState.m_Instance = this;
    }
  }
}
