// Decompiled with JetBrains decompiler
// Type: UnityEditor.ContainerWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor
{
  [StructLayout(LayoutKind.Sequential)]
  internal sealed class ContainerWindow : ScriptableObject
  {
    private static List<ContainerWindow> s_AllWindows = new List<ContainerWindow>();
    [SerializeField]
    private string m_Title = string.Empty;
    [SerializeField]
    private Vector2 m_MinSize = new Vector2(120f, 80f);
    [SerializeField]
    private Vector2 m_MaxSize = new Vector2(4000f, 4000f);
    private const float kBorderSize = 4f;
    private const float kTitleHeight = 24f;
    private const float kButtonWidth = 13f;
    private const float kButtonHeight = 13f;
    private const float kButtonSpacing = 3f;
    private const float kButtonTop = 0.0f;
    [SerializeField]
    private MonoReloadableIntPtr m_WindowPtr;
    [SerializeField]
    private Rect m_PixelRect;
    [SerializeField]
    private int m_ShowMode;
    [SerializeField]
    private View m_MainView;
    internal bool m_DontSaveToLayout;
    [SerializeField]
    private SnapEdge m_Left;
    [SerializeField]
    private SnapEdge m_Right;
    [SerializeField]
    private SnapEdge m_Top;
    [SerializeField]
    private SnapEdge m_Bottom;
    private SnapEdge[] m_EdgesCache;
    private int m_ButtonCount;
    private float m_TitleBarWidth;
    private static Vector2 s_LastDragMousePos;

    public bool maximized { [WrapperlessIcall, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Rect position
    {
      get
      {
        Rect rect;
        this.INTERNAL_get_position(out rect);
        return rect;
      }
      set
      {
        this.INTERNAL_set_position(ref value);
      }
    }

    private IEnumerable<SnapEdge> edges
    {
      get
      {
        if (this.m_EdgesCache == null)
          this.m_EdgesCache = new SnapEdge[4]
          {
            this.m_Left,
            this.m_Right,
            this.m_Top,
            this.m_Bottom
          };
        return (IEnumerable<SnapEdge>) this.m_EdgesCache;
      }
    }

    internal static bool macEditor
    {
      get
      {
        return Application.platform == RuntimePlatform.OSXEditor;
      }
    }

    internal ShowMode showMode
    {
      get
      {
        return (ShowMode) this.m_ShowMode;
      }
    }

    public string title
    {
      get
      {
        return this.m_Title;
      }
      set
      {
        this.m_Title = value;
        this.Internal_SetTitle(value);
      }
    }

    public static ContainerWindow[] windows
    {
      get
      {
        ContainerWindow.s_AllWindows.Clear();
        ContainerWindow.GetOrderedWindowList();
        return ContainerWindow.s_AllWindows.ToArray();
      }
    }

    public View mainView
    {
      get
      {
        return this.m_MainView;
      }
      set
      {
        this.m_MainView = value;
        this.m_MainView.SetWindowRecurse(this);
        this.m_MainView.position = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
        this.m_MinSize = value.minSize;
        this.m_MaxSize = value.maxSize;
      }
    }

    public ContainerWindow()
    {
      this.hideFlags = HideFlags.DontSave;
      this.m_PixelRect = new Rect(0.0f, 0.0f, 400f, 300f);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetAlpha(float alpha);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void SetInvisible();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public bool IsZoomed();

    private void Internal_SetMinMaxSizes(Vector2 minSize, Vector2 maxSize)
    {
      ContainerWindow.INTERNAL_CALL_Internal_SetMinMaxSizes(this, ref minSize, ref maxSize);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetMinMaxSizes(ContainerWindow self, ref Vector2 minSize, ref Vector2 maxSize);

    private void Internal_Show(Rect r, int showMode, Vector2 minSize, Vector2 maxSize)
    {
      ContainerWindow.INTERNAL_CALL_Internal_Show(this, ref r, showMode, ref minSize, ref maxSize);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_Show(ContainerWindow self, ref Rect r, int showMode, ref Vector2 minSize, ref Vector2 maxSize);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_BringLiveAfterCreation(bool displayImmediately, bool setFocus);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetFreezeDisplay(bool freeze);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void DisplayAllViews();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void Minimize();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void ToggleMaximize();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void MoveInFrontOf(ContainerWindow other);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void MoveBehindOf(ContainerWindow other);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void InternalClose();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public void OnDestroy();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_get_position(out Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void INTERNAL_set_position(ref Rect value);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_SetTitle(string title);

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void GetOrderedWindowList();

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private void Internal_GetTopleftScreenPosition(out Vector2 pos);

    internal Rect FitWindowRectToScreen(Rect r, bool forceCompletelyVisible, bool useMouseScreen)
    {
      Rect rect;
      ContainerWindow.INTERNAL_CALL_FitWindowRectToScreen(this, ref r, forceCompletelyVisible, useMouseScreen, out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_FitWindowRectToScreen(ContainerWindow self, ref Rect r, bool forceCompletelyVisible, bool useMouseScreen, out Rect value);

    internal static Rect FitRectToScreen(Rect defaultRect, bool forceCompletelyVisible, bool useMouseScreen)
    {
      Rect rect;
      ContainerWindow.INTERNAL_CALL_FitRectToScreen(ref defaultRect, forceCompletelyVisible, useMouseScreen, out rect);
      return rect;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_FitRectToScreen(ref Rect defaultRect, bool forceCompletelyVisible, bool useMouseScreen, out Rect value);

    internal void ShowPopup()
    {
      this.m_ShowMode = 1;
      this.Internal_Show(this.m_PixelRect, this.m_ShowMode, this.m_MinSize, this.m_MaxSize);
      if ((bool) ((Object) this.m_MainView))
        this.m_MainView.SetWindowRecurse(this);
      this.Internal_SetTitle(this.m_Title);
      this.Save();
      this.Internal_BringLiveAfterCreation(false, false);
    }

    public void Show(ShowMode showMode, bool loadPosition, bool displayImmediately)
    {
      if (showMode == ShowMode.AuxWindow)
        showMode = ShowMode.Utility;
      if (showMode == ShowMode.Utility || showMode == ShowMode.PopupMenu)
        this.m_DontSaveToLayout = true;
      this.m_ShowMode = (int) showMode;
      if (showMode != ShowMode.PopupMenu)
        this.Load(loadPosition);
      this.Internal_Show(this.m_PixelRect, this.m_ShowMode, this.m_MinSize, this.m_MaxSize);
      if ((bool) ((Object) this.m_MainView))
        this.m_MainView.SetWindowRecurse(this);
      this.Internal_SetTitle(this.m_Title);
      this.Internal_BringLiveAfterCreation(displayImmediately, true);
      if ((Object) this == (Object) null)
        return;
      this.position = this.FitWindowRectToScreen(this.m_PixelRect, true, false);
      this.mainView.position = new Rect(0.0f, 0.0f, this.m_PixelRect.width, this.m_PixelRect.height);
      this.mainView.Reflow();
      this.Save();
    }

    public void OnEnable()
    {
      if (!(bool) ((Object) this.m_MainView))
        return;
      this.m_MainView.Initialize(this);
    }

    public void SetMinMaxSizes(Vector2 min, Vector2 max)
    {
      this.m_MinSize = min;
      this.m_MaxSize = max;
      Rect position = this.position;
      Rect rect = position;
      rect.width = Mathf.Clamp(position.width, min.x, max.x);
      rect.height = Mathf.Clamp(position.height, min.y, max.y);
      if ((double) rect.width != (double) position.width || (double) rect.height != (double) position.height)
        this.position = rect;
      this.Internal_SetMinMaxSizes(min, max);
    }

    internal void InternalCloseWindow()
    {
      this.Save();
      if ((bool) ((Object) this.m_MainView))
      {
        if (this.m_MainView is GUIView)
          ((GUIView) this.m_MainView).RemoveFromAuxWindowList();
        Object.DestroyImmediate((Object) this.m_MainView, true);
        this.m_MainView = (View) null;
      }
      Object.DestroyImmediate((Object) this, true);
    }

    public void Close()
    {
      this.Save();
      this.InternalClose();
      Object.DestroyImmediate((Object) this, true);
    }

    internal bool IsNotDocked()
    {
      if (this.m_ShowMode == 2 || this.m_ShowMode == 5)
        return true;
      if ((Object) (this.mainView as SplitView) != (Object) null && this.mainView.children.Length == 1 && (this.mainView.children.Length == 1 && this.mainView.children[0] is DockArea))
        return ((DockArea) this.mainView.children[0]).m_Panes.Count == 1;
      return false;
    }

    private string NotDockedWindowID()
    {
      if (!this.IsNotDocked())
        return (string) null;
      HostView hostView = this.mainView as HostView;
      if ((Object) hostView == (Object) null)
      {
        if (!(this.mainView is SplitView))
          return this.mainView.GetType().ToString();
        hostView = (HostView) this.mainView.children[0];
      }
      if (this.m_ShowMode == 2 || this.m_ShowMode == 5)
        return hostView.actualView.GetType().ToString();
      return ((DockArea) this.mainView.children[0]).m_Panes[0].GetType().ToString();
    }

    public void Save()
    {
      if (this.m_ShowMode == 4 || !this.IsNotDocked() || this.IsZoomed())
        return;
      string str = this.NotDockedWindowID();
      EditorPrefs.SetFloat(str + "x", this.m_PixelRect.x);
      EditorPrefs.SetFloat(str + "y", this.m_PixelRect.y);
      EditorPrefs.SetFloat(str + "w", this.m_PixelRect.width);
      EditorPrefs.SetFloat(str + "h", this.m_PixelRect.height);
    }

    private void Load(bool loadPosition)
    {
      if (this.m_ShowMode == 4 || !this.IsNotDocked())
        return;
      string str = this.NotDockedWindowID();
      Rect pixelRect = this.m_PixelRect;
      if (loadPosition)
      {
        pixelRect.x = EditorPrefs.GetFloat(str + "x", this.m_PixelRect.x);
        pixelRect.y = EditorPrefs.GetFloat(str + "y", this.m_PixelRect.y);
      }
      pixelRect.width = Mathf.Max(EditorPrefs.GetFloat(str + "w", this.m_PixelRect.width), this.m_MinSize.x);
      pixelRect.width = Mathf.Min(pixelRect.width, this.m_MaxSize.x);
      pixelRect.height = Mathf.Max(EditorPrefs.GetFloat(str + "h", this.m_PixelRect.height), this.m_MinSize.y);
      pixelRect.height = Mathf.Min(pixelRect.height, this.m_MaxSize.y);
      this.m_PixelRect = pixelRect;
    }

    internal void OnResize()
    {
      if ((Object) this.mainView == (Object) null)
        return;
      this.mainView.position = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      this.mainView.Reflow();
      this.Save();
    }

    internal void AddToWindowList()
    {
      ContainerWindow.s_AllWindows.Add(this);
    }

    public Vector2 WindowToScreenPoint(Vector2 windowPoint)
    {
      Vector2 pos;
      this.Internal_GetTopleftScreenPosition(out pos);
      return windowPoint + pos;
    }

    internal string DebugHierarchy()
    {
      return this.mainView.DebugHierarchy(0);
    }

    internal Rect GetDropDownRect(Rect buttonRect, Vector2 minSize, Vector2 maxSize, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
    {
      return PopupLocationHelper.GetDropDownRect(buttonRect, minSize, maxSize, this, locationPriorityOrder);
    }

    internal Rect GetDropDownRect(Rect buttonRect, Vector2 minSize, Vector2 maxSize)
    {
      return PopupLocationHelper.GetDropDownRect(buttonRect, minSize, maxSize, this);
    }

    internal Rect FitPopupWindowRectToScreen(Rect rect, float minimumHeight)
    {
      float num1 = 0.0f;
      if (Application.platform == RuntimePlatform.OSXEditor)
        num1 = 10f;
      float b = minimumHeight + num1;
      Rect r = rect;
      r.height = Mathf.Min(r.height, 900f);
      r.height += num1;
      Rect screen = this.FitWindowRectToScreen(r, true, true);
      float num2 = Mathf.Max(screen.yMax - rect.y, b);
      screen.y = screen.yMax - num2;
      screen.height = num2 - num1;
      return screen;
    }

    public void HandleWindowDecorationEnd(Rect windowPosition)
    {
    }

    public void HandleWindowDecorationStart(Rect windowPosition)
    {
      if ((double) windowPosition.y != 0.0 || this.showMode == ShowMode.Utility || this.showMode == ShowMode.PopupMenu)
        return;
      if ((double) Mathf.Abs(windowPosition.xMax - this.position.width) < 2.0)
      {
        GUIStyle style1 = ContainerWindow.Styles.buttonClose;
        GUIStyle style2 = ContainerWindow.Styles.buttonMin;
        GUIStyle style3 = ContainerWindow.Styles.buttonMax;
        if (ContainerWindow.macEditor && ((Object) GUIView.focusedView == (Object) null || (Object) GUIView.focusedView.window != (Object) this))
        {
          GUIStyle buttonInactive;
          style3 = buttonInactive = ContainerWindow.Styles.buttonInactive;
          style2 = buttonInactive;
          style1 = buttonInactive;
        }
        this.BeginTitleBarButtons(windowPosition);
        if (this.TitleBarButton(style1))
          this.Close();
        if (ContainerWindow.macEditor && this.TitleBarButton(style2))
        {
          this.Minimize();
          GUIUtility.ExitGUI();
        }
        if (this.TitleBarButton(style3))
          this.ToggleMaximize();
      }
      this.HandleTitleBarDrag();
    }

    private void BeginTitleBarButtons(Rect windowPosition)
    {
      this.m_ButtonCount = 0;
      this.m_TitleBarWidth = windowPosition.width;
    }

    private bool TitleBarButton(GUIStyle style)
    {
      return GUI.Button(new Rect((float) ((double) this.m_TitleBarWidth - 13.0 * (double) ++this.m_ButtonCount - 4.0), 0.0f, 13f, 13f), GUIContent.none, style);
    }

    private void SetupWindowEdges()
    {
      Rect position = this.position;
      if (this.m_Left == null)
      {
        this.m_Left = new SnapEdge((Object) this, SnapEdge.EdgeDir.Left, position.xMin, position.yMin, position.yMax);
        this.m_Right = new SnapEdge((Object) this, SnapEdge.EdgeDir.Right, position.xMax, position.yMin, position.yMax);
        this.m_Top = new SnapEdge((Object) this, SnapEdge.EdgeDir.Up, position.yMin, position.xMin, position.xMax);
        this.m_Bottom = new SnapEdge((Object) this, SnapEdge.EdgeDir.Down, position.yMax, position.xMin, position.xMax);
      }
      this.m_Left.pos = position.xMin;
      this.m_Left.start = position.yMin;
      this.m_Left.end = position.yMax;
      this.m_Right.pos = position.xMax;
      this.m_Right.start = position.yMin;
      this.m_Right.end = position.yMax;
      this.m_Top.pos = position.yMin;
      this.m_Top.start = position.xMin;
      this.m_Top.end = position.xMax;
      this.m_Bottom.pos = position.yMax;
      this.m_Bottom.start = position.xMin;
      this.m_Bottom.end = position.xMax;
    }

    private void HandleTitleBarDrag()
    {
      this.SetupWindowEdges();
      EditorGUI.BeginChangeCheck();
      this.DragTitleBar(new Rect(0.0f, 0.0f, this.position.width, 24f));
      if (!EditorGUI.EndChangeCheck())
        return;
      Rect r = new Rect(this.m_Left.pos, this.m_Top.pos, this.m_Right.pos - this.m_Left.pos, this.m_Bottom.pos - this.m_Top.pos);
      if (ContainerWindow.macEditor)
        r = this.FitWindowRectToScreen(r, false, false);
      this.position = r;
    }

    private void DragTitleBar(Rect titleBarRect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      Event current1 = Event.current;
      switch (current1.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (!titleBarRect.Contains(current1.mousePosition) || GUIUtility.hotControl != 0 || current1.button != 0)
            break;
          GUIUtility.hotControl = controlId;
          Event.current.Use();
          ContainerWindow.s_LastDragMousePos = GUIUtility.GUIToScreenPoint(current1.mousePosition);
          using (IEnumerator<SnapEdge> enumerator = this.edges.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              SnapEdge current2 = enumerator.Current;
              current2.startDragPos = current2.pos;
              current2.startDragStart = current2.start;
            }
            break;
          }
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          Event.current.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId)
            break;
          Vector2 screenPoint = GUIUtility.GUIToScreenPoint(current1.mousePosition);
          Vector2 offset = screenPoint - ContainerWindow.s_LastDragMousePos;
          ContainerWindow.s_LastDragMousePos = screenPoint;
          GUI.changed = true;
          using (IEnumerator<SnapEdge> enumerator = this.edges.GetEnumerator())
          {
            while (enumerator.MoveNext())
              enumerator.Current.ApplyOffset(offset, true);
            break;
          }
        case EventType.Repaint:
          EditorGUIUtility.AddCursorRect(titleBarRect, MouseCursor.Arrow);
          break;
      }
    }

    private static class Styles
    {
      public static GUIStyle buttonClose = (GUIStyle) (!ContainerWindow.macEditor ? "WinBtnClose" : "WinBtnCloseMac");
      public static GUIStyle buttonMin = (GUIStyle) (!ContainerWindow.macEditor ? "WinBtnClose" : "WinBtnMinMac");
      public static GUIStyle buttonMax = (GUIStyle) (!ContainerWindow.macEditor ? "WinBtnMax" : "WinBtnMaxMac");
      public static GUIStyle buttonInactive = (GUIStyle) "WinBtnInactiveMac";
    }
  }
}
