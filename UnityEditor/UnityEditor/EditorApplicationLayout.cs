// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorApplicationLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class EditorApplicationLayout
  {
    private static GameView m_GameView;
    private static View m_RootSplit;

    internal static bool IsInitializingPlaymodeLayout()
    {
      return (Object) EditorApplicationLayout.m_GameView != (Object) null;
    }

    internal static void SetPlaymodeLayout()
    {
      EditorApplicationLayout.InitPlaymodeLayout();
      EditorApplicationLayout.FinalizePlaymodeLayout();
    }

    internal static void SetStopmodeLayout()
    {
      WindowLayout.ShowAppropriateViewOnEnterExitPlaymode(false);
      Toolbar.RepaintToolbar();
    }

    internal static void SetPausemodeLayout()
    {
      EditorApplicationLayout.SetStopmodeLayout();
    }

    internal static void InitPlaymodeLayout()
    {
      EditorApplicationLayout.m_GameView = WindowLayout.ShowAppropriateViewOnEnterExitPlaymode(true) as GameView;
      if ((Object) EditorApplicationLayout.m_GameView == (Object) null)
        return;
      if (EditorApplicationLayout.m_GameView.maximizeOnPlay)
      {
        DockArea parent = EditorApplicationLayout.m_GameView.m_Parent as DockArea;
        if ((Object) parent != (Object) null && !parent.actualView.m_Parent.window.maximized)
          EditorApplicationLayout.m_RootSplit = WindowLayout.MaximizePrepare(parent.actualView);
      }
      EditorApplicationLayout.m_GameView.m_Parent.SetAsStartView();
      Toolbar.RepaintToolbar();
    }

    internal static void FinalizePlaymodeLayout()
    {
      if ((Object) EditorApplicationLayout.m_GameView != (Object) null)
      {
        if ((Object) EditorApplicationLayout.m_RootSplit != (Object) null)
          WindowLayout.MaximizePresent((EditorWindow) EditorApplicationLayout.m_GameView, EditorApplicationLayout.m_RootSplit);
        EditorApplicationLayout.m_GameView.m_Parent.ClearStartView();
      }
      EditorApplicationLayout.Clear();
    }

    private static void Clear()
    {
      EditorApplicationLayout.m_RootSplit = (View) null;
      EditorApplicationLayout.m_GameView = (GameView) null;
    }
  }
}
