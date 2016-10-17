// Decompiled with JetBrains decompiler
// Type: UnityEditor.GUIView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  [UsedByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  internal class GUIView : View
  {
    private int m_DepthBufferBits;
    private int m_AntiAlias;
    private bool m_WantsMouseMove;
    private bool m_AutoRepaintOnSceneChange;
    private bool m_BackgroundValid;

    public static extern GUIView current { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern GUIView focusedView { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern GUIView mouseOverView { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public bool hasFocus { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal bool mouseRayInvisible { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public bool wantsMouseMove
    {
      get
      {
        return this.m_WantsMouseMove;
      }
      set
      {
        this.m_WantsMouseMove = value;
        this.Internal_SetWantsMouseMove(this.m_WantsMouseMove);
      }
    }

    internal bool backgroundValid
    {
      get
      {
        return this.m_BackgroundValid;
      }
      set
      {
        this.m_BackgroundValid = value;
      }
    }

    public bool autoRepaintOnSceneChange
    {
      get
      {
        return this.m_AutoRepaintOnSceneChange;
      }
      set
      {
        this.m_AutoRepaintOnSceneChange = value;
        this.Internal_SetAutoRepaint(this.m_AutoRepaintOnSceneChange);
      }
    }

    public int depthBufferBits
    {
      get
      {
        return this.m_DepthBufferBits;
      }
      set
      {
        this.m_DepthBufferBits = value;
      }
    }

    public int antiAlias
    {
      get
      {
        return this.m_AntiAlias;
      }
      set
      {
        this.m_AntiAlias = value;
      }
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void SetTitle(string title);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_Init(int depthBits, int antiAlias);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_Recreate(int depthBits, int antiAlias);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_Close();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private bool Internal_SendEvent(Event e);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void AddToAuxWindowList();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void RemoveFromAuxWindowList();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    protected void Internal_SetAsActiveWindow();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetWantsMouseMove(bool wantIt);

    public void SetInternalGameViewRect(Rect rect)
    {
      GUIView.INTERNAL_CALL_SetInternalGameViewRect(this, ref rect);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetInternalGameViewRect(GUIView self, ref Rect rect);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetAsStartView();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ClearStartView();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetAutoRepaint(bool doit);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetWindow(ContainerWindow win);

    private void Internal_SetPosition(Rect windowPosition)
    {
      GUIView.INTERNAL_CALL_Internal_SetPosition(this, ref windowPosition);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetPosition(GUIView self, ref Rect windowPosition);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Focus();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Repaint();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void RepaintImmediately();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void CaptureRenderDoc();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void MakeVistaDWMHappyDance();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void StealMouseCapture();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void ClearKeyboardControl();

    internal void GrabPixels(RenderTexture rd, Rect rect)
    {
      GUIView.INTERNAL_CALL_GrabPixels(this, rd, ref rect);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GrabPixels(GUIView self, RenderTexture rd, ref Rect rect);

    internal bool SendEvent(Event e)
    {
      bool flag;
      if (SavedGUIState.Internal_GetGUIDepth() > 0)
      {
        SavedGUIState savedGuiState = SavedGUIState.Create();
        flag = this.Internal_SendEvent(e);
        savedGuiState.ApplyAndForget();
      }
      else
        flag = this.Internal_SendEvent(e);
      return flag;
    }

    protected override void SetWindow(ContainerWindow win)
    {
      base.SetWindow(win);
      this.Internal_Init(this.m_DepthBufferBits, this.m_AntiAlias);
      if ((bool) ((Object) win))
        this.Internal_SetWindow(win);
      this.Internal_SetAutoRepaint(this.m_AutoRepaintOnSceneChange);
      this.Internal_SetPosition(this.windowPosition);
      this.Internal_SetWantsMouseMove(this.m_WantsMouseMove);
      this.m_BackgroundValid = false;
    }

    internal void RecreateContext()
    {
      this.Internal_Recreate(this.m_DepthBufferBits, this.m_AntiAlias);
      this.m_BackgroundValid = false;
    }

    protected override void SetPosition(Rect newPos)
    {
      Rect windowPosition = this.windowPosition;
      base.SetPosition(newPos);
      if (windowPosition == this.windowPosition)
      {
        this.Internal_SetPosition(this.windowPosition);
      }
      else
      {
        this.Repaint();
        this.Internal_SetPosition(this.windowPosition);
        this.m_BackgroundValid = false;
      }
    }

    public new void OnDestroy()
    {
      this.Internal_Close();
      base.OnDestroy();
    }

    internal void DoWindowDecorationStart()
    {
      if (!((Object) this.window != (Object) null))
        return;
      this.window.HandleWindowDecorationStart(this.windowPosition);
    }

    internal void DoWindowDecorationEnd()
    {
      if (!((Object) this.window != (Object) null))
        return;
      this.window.HandleWindowDecorationEnd(this.windowPosition);
    }
  }
}
