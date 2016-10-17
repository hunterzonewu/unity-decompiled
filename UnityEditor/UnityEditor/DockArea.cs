// Decompiled with JetBrains decompiler
// Type: UnityEditor.DockArea
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class DockArea : HostView, IDropArea
  {
    [SerializeField]
    internal List<EditorWindow> m_Panes = new List<EditorWindow>();
    internal const float kTabHeight = 17f;
    internal const float kDockHeight = 39f;
    private const float kSideBorders = 2f;
    private const float kBottomBorders = 2f;
    private const float kWindowButtonsWidth = 40f;
    private static int s_PlaceholderPos;
    private static EditorWindow s_DragPane;
    internal static DockArea s_OriginalDragSource;
    private static Vector2 s_StartDragPosition;
    private static int s_DragMode;
    internal static View s_IgnoreDockingForView;
    private static DropInfo s_DropInfo;
    [SerializeField]
    internal int m_Selected;
    [SerializeField]
    internal int m_LastSelected;
    [NonSerialized]
    internal GUIStyle tabStyle;

    public int selected
    {
      get
      {
        return this.m_Selected;
      }
      set
      {
        if (this.m_Selected == value)
          return;
        this.m_Selected = value;
        if (this.m_Selected < 0 || this.m_Selected >= this.m_Panes.Count)
          return;
        this.actualView = this.m_Panes[this.m_Selected];
      }
    }

    private Rect tabRect
    {
      get
      {
        return new Rect(0.0f, 0.0f, this.position.width, 17f);
      }
    }

    public DockArea()
    {
      if (this.m_Panes == null || this.m_Panes.Count == 0)
        return;
      Debug.LogError((object) "m_Panes is filled in DockArea constructor.");
    }

    private void RemoveNullWindows()
    {
      List<EditorWindow> editorWindowList = new List<EditorWindow>();
      using (List<EditorWindow>.Enumerator enumerator = this.m_Panes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          EditorWindow current = enumerator.Current;
          if ((UnityEngine.Object) current != (UnityEngine.Object) null)
            editorWindowList.Add(current);
        }
      }
      this.m_Panes = editorWindowList;
    }

    public new void OnDestroy()
    {
      if (this.hasFocus)
        this.Invoke("OnLostFocus");
      this.actualView = (EditorWindow) null;
      using (List<EditorWindow>.Enumerator enumerator = this.m_Panes.GetEnumerator())
      {
        while (enumerator.MoveNext())
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) enumerator.Current, true);
      }
      base.OnDestroy();
    }

    public new void OnEnable()
    {
      if (this.m_Panes != null && this.m_Panes.Count > this.m_Selected)
        this.actualView = this.m_Panes[this.m_Selected];
      base.OnEnable();
    }

    public void AddTab(EditorWindow pane)
    {
      this.AddTab(this.m_Panes.Count, pane);
    }

    public void AddTab(int idx, EditorWindow pane)
    {
      this.DeregisterSelectedPane(true);
      this.m_Panes.Insert(idx, pane);
      this.m_ActualView = pane;
      this.m_Panes[idx] = pane;
      this.m_Selected = idx;
      this.RegisterSelectedPane();
      this.Repaint();
    }

    public void RemoveTab(EditorWindow pane)
    {
      this.RemoveTab(pane, true);
    }

    public void RemoveTab(EditorWindow pane, bool killIfEmpty)
    {
      if ((UnityEngine.Object) this.m_ActualView == (UnityEngine.Object) pane)
        this.DeregisterSelectedPane(true);
      int num = this.m_Panes.IndexOf(pane);
      if (num == -1)
      {
        Debug.LogError((object) "Unable to remove Pane - it's not IN the window");
      }
      else
      {
        this.m_Panes.Remove(pane);
        if (num == this.m_Selected)
        {
          if (this.m_LastSelected >= this.m_Panes.Count - 1)
            this.m_LastSelected = this.m_Panes.Count - 1;
          this.m_Selected = this.m_LastSelected;
          if (this.m_Selected > -1)
            this.m_ActualView = this.m_Panes[this.m_Selected];
        }
        else if (num < this.m_Selected)
          --this.m_Selected;
        this.Repaint();
        pane.m_Parent = (HostView) null;
        if (killIfEmpty)
          this.KillIfEmpty();
        this.RegisterSelectedPane();
      }
    }

    private void KillIfEmpty()
    {
      if (this.m_Panes.Count != 0)
        return;
      if ((UnityEngine.Object) this.parent == (UnityEngine.Object) null)
      {
        this.window.InternalCloseWindow();
      }
      else
      {
        SplitView parent1 = this.parent as SplitView;
        ICleanuppable parent2 = this.parent as ICleanuppable;
        parent1.RemoveChildNice((View) this);
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this, true);
        if (parent2 == null)
          return;
        parent2.Cleanup();
      }
    }

    public DropInfo DragOver(EditorWindow window, Vector2 mouseScreenPosition)
    {
      Rect screenPosition = this.screenPosition;
      screenPosition.height = 39f;
      if (!screenPosition.Contains(mouseScreenPosition))
        return (DropInfo) null;
      if (this.background == null)
        this.background = (GUIStyle) "hostview";
      Rect rect = this.background.margin.Remove(this.screenPosition);
      Vector2 mousePos = mouseScreenPosition - new Vector2(rect.x, rect.y);
      Rect tabRect = this.tabRect;
      int tabAtMousePos = this.GetTabAtMousePos(mousePos, tabRect);
      float tabWidth = this.GetTabWidth(tabRect.width);
      if (DockArea.s_PlaceholderPos != tabAtMousePos)
      {
        this.Repaint();
        DockArea.s_PlaceholderPos = tabAtMousePos;
      }
      return new DropInfo((IDropArea) this) { type = DropInfo.Type.Tab, rect = new Rect(mousePos.x - tabWidth * 0.25f + rect.x, tabRect.y + rect.y, tabWidth, tabRect.height) };
    }

    public bool PerformDrop(EditorWindow w, DropInfo info, Vector2 screenPos)
    {
      DockArea.s_OriginalDragSource.RemoveTab(w, (UnityEngine.Object) DockArea.s_OriginalDragSource != (UnityEngine.Object) this);
      int idx = DockArea.s_PlaceholderPos <= this.m_Panes.Count ? DockArea.s_PlaceholderPos : this.m_Panes.Count;
      this.AddTab(idx, w);
      this.selected = idx;
      return true;
    }

    public void OnGUI()
    {
      this.ClearBackground();
      EditorGUIUtility.ResetGUIState();
      SplitView parent = this.parent as SplitView;
      if (Event.current.type == EventType.Repaint && (bool) ((UnityEngine.Object) parent))
      {
        View child = (View) this;
        for (; (bool) ((UnityEngine.Object) parent); parent = parent.parent as SplitView)
        {
          int controlId = parent.controlID;
          if (controlId == GUIUtility.hotControl || GUIUtility.hotControl == 0)
          {
            int num = parent.IndexOfChild(child);
            if (parent.vertical)
            {
              if (num != 0)
                EditorGUIUtility.AddCursorRect(new Rect(0.0f, 0.0f, this.position.width, 5f), MouseCursor.SplitResizeUpDown, controlId);
              if (num != parent.children.Length - 1)
                EditorGUIUtility.AddCursorRect(new Rect(0.0f, this.position.height - 5f, this.position.width, 5f), MouseCursor.SplitResizeUpDown, controlId);
            }
            else
            {
              if (num != 0)
                EditorGUIUtility.AddCursorRect(new Rect(0.0f, 0.0f, 5f, this.position.height), MouseCursor.SplitResizeLeftRight, controlId);
              if (num != parent.children.Length - 1)
                EditorGUIUtility.AddCursorRect(new Rect(this.position.width - 5f, 0.0f, 5f, this.position.height), MouseCursor.SplitResizeLeftRight, controlId);
            }
          }
          child = (View) parent;
        }
        parent = this.parent as SplitView;
      }
      bool flag = false;
      if (this.window.mainView.GetType() != typeof (MainWindow))
      {
        flag = true;
        if ((double) this.windowPosition.y == 0.0)
          this.background = (GUIStyle) "dockareaStandalone";
        else
          this.background = (GUIStyle) "dockarea";
      }
      else
        this.background = (GUIStyle) "dockarea";
      if ((bool) ((UnityEngine.Object) parent))
      {
        Event evt = new Event(Event.current);
        evt.mousePosition += new Vector2(this.position.x, this.position.y);
        parent.SplitGUI(evt);
        if (evt.type == EventType.Used)
          Event.current.Use();
      }
      GUIStyle style = (GUIStyle) "dockareaoverlay";
      Rect position = this.background.margin.Remove(new Rect(0.0f, 0.0f, this.position.width, this.position.height));
      position.x = (float) this.background.margin.left;
      position.y = (float) this.background.margin.top;
      Rect windowPosition = this.windowPosition;
      float num1 = 2f;
      if ((double) windowPosition.x == 0.0)
      {
        position.x -= num1;
        position.width += num1;
      }
      if ((double) windowPosition.xMax == (double) this.window.position.width)
        position.width += num1;
      if ((double) windowPosition.yMax == (double) this.window.position.height)
        position.height += !flag ? 2f : 2f;
      GUI.Box(position, GUIContent.none, this.background);
      if (this.tabStyle == null)
        this.tabStyle = (GUIStyle) "dragtab";
      this.DragTab(new Rect(position.x + 1f, position.y, position.width - 40f, 17f), this.tabStyle);
      this.tabStyle = (GUIStyle) "dragtab";
      this.ShowGenericMenu();
      this.DoWindowDecorationStart();
      if (this.m_Panes.Count > 0)
      {
        if (this.m_Panes[this.selected] is GameView)
          GUI.Box(position, GUIContent.none, style);
        DockArea.BeginOffsetArea(new Rect(position.x + 2f, position.y + 17f, position.width - 4f, (float) ((double) position.height - 17.0 - 2.0)), GUIContent.none, (GUIStyle) "TabWindowBackground");
        Vector2 screenPoint = GUIUtility.GUIToScreenPoint(Vector2.zero);
        Rect rect = this.borderSize.Remove(this.position);
        rect.x = screenPoint.x;
        rect.y = screenPoint.y;
        this.m_Panes[this.selected].m_Pos = rect;
        EditorGUIUtility.ResetGUIState();
        try
        {
          this.Invoke("OnGUI");
        }
        catch (TargetInvocationException ex)
        {
          throw ex.InnerException;
        }
        EditorGUIUtility.ResetGUIState();
        if ((UnityEngine.Object) this.actualView != (UnityEngine.Object) null && (double) this.actualView.m_FadeoutTime != 0.0 && (Event.current != null && Event.current.type == EventType.Repaint))
          this.actualView.DrawNotification();
        DockArea.EndOffsetArea();
      }
      this.DoWindowDecorationEnd();
      GUI.Box(position, GUIContent.none, style);
      EditorGUI.ShowRepaints();
      Highlighter.ControlHighlightGUI((GUIView) this);
    }

    private void Maximize(object userData)
    {
      WindowLayout.Maximize((EditorWindow) userData);
    }

    private void Close(object userData)
    {
      ((EditorWindow) userData).Close();
    }

    protected override void AddDefaultItemsToMenu(GenericMenu menu, EditorWindow view)
    {
      if (menu.GetItemCount() != 0)
        menu.AddSeparator(string.Empty);
      if (this.parent.window.showMode == ShowMode.MainWindow)
        menu.AddItem(EditorGUIUtility.TextContent("Maximize"), !(this.parent is SplitView), new GenericMenu.MenuFunction2(this.Maximize), (object) view);
      else
        menu.AddDisabledItem(EditorGUIUtility.TextContent("Maximize"));
      menu.AddItem(EditorGUIUtility.TextContent("Close Tab"), false, new GenericMenu.MenuFunction2(this.Close), (object) view);
      menu.AddSeparator(string.Empty);
      System.Type[] paneTypes = this.GetPaneTypes();
      GUIContent guiContent = EditorGUIUtility.TextContent("Add Tab");
      foreach (System.Type t in paneTypes)
      {
        if (t != null)
        {
          GUIContent content = new GUIContent(EditorWindow.GetLocalizedTitleContentFromType(t));
          content.text = guiContent.text + "/" + content.text;
          menu.AddItem(content, false, new GenericMenu.MenuFunction2(this.AddTabToHere), (object) t);
        }
      }
    }

    private void AddTabToHere(object userData)
    {
      this.AddTab((EditorWindow) ScriptableObject.CreateInstance((System.Type) userData));
    }

    public static void EndOffsetArea()
    {
      if (Event.current.type == EventType.Used)
        return;
      GUILayoutUtility.EndLayoutGroup();
      GUI.EndGroup();
    }

    public static void BeginOffsetArea(Rect screenRect, GUIContent content, GUIStyle style)
    {
      GUILayoutGroup guiLayoutGroup = EditorGUILayoutUtilityInternal.BeginLayoutArea(style, typeof (GUILayoutGroup));
      if (Event.current.type == EventType.Layout)
      {
        guiLayoutGroup.resetCoords = false;
        guiLayoutGroup.minWidth = guiLayoutGroup.maxWidth = screenRect.width + 1f;
        guiLayoutGroup.minHeight = guiLayoutGroup.maxHeight = screenRect.height + 2f;
        guiLayoutGroup.rect = Rect.MinMaxRect(-1f, -1f, guiLayoutGroup.rect.xMax, guiLayoutGroup.rect.yMax - 10f);
      }
      GUI.BeginGroup(screenRect, content, style);
    }

    private float GetTabWidth(float width)
    {
      int count = this.m_Panes.Count;
      if (DockArea.s_DropInfo != null && object.ReferenceEquals((object) DockArea.s_DropInfo.dropArea, (object) this))
        ++count;
      if (this.m_Panes.IndexOf(DockArea.s_DragPane) != -1)
        --count;
      return Mathf.Min(width / (float) count, 100f);
    }

    private int GetTabAtMousePos(Vector2 mousePos, Rect position)
    {
      return (int) Mathf.Min((mousePos.x - position.xMin) / this.GetTabWidth(position.width), 100f);
    }

    internal override void Initialize(ContainerWindow win)
    {
      base.Initialize(win);
      this.RemoveNullWindows();
      using (List<EditorWindow>.Enumerator enumerator = this.m_Panes.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.m_Parent = (HostView) this;
      }
    }

    private static void CheckDragWindowExists()
    {
      if (DockArea.s_DragMode != 1 || (bool) ((UnityEngine.Object) PaneDragTab.get.m_Window))
        return;
      DockArea.s_OriginalDragSource.RemoveTab(DockArea.s_DragPane);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) DockArea.s_DragPane);
      PaneDragTab.get.Close();
      GUIUtility.hotControl = 0;
      DockArea.ResetDragVars();
    }

    private void DragTab(Rect pos, GUIStyle tabStyle)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      float tabWidth = this.GetTabWidth(pos.width);
      Event current = Event.current;
      if (DockArea.s_DragMode != 0 && GUIUtility.hotControl == 0)
      {
        PaneDragTab.get.Close();
        DockArea.ResetDragVars();
      }
      EventType typeForControl = current.GetTypeForControl(controlId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (pos.Contains(current.mousePosition) && GUIUtility.hotControl == 0)
          {
            int tabAtMousePos = this.GetTabAtMousePos(current.mousePosition, pos);
            if (tabAtMousePos < this.m_Panes.Count)
            {
              switch (current.button)
              {
                case 0:
                  if (tabAtMousePos != this.selected)
                    this.selected = tabAtMousePos;
                  GUIUtility.hotControl = controlId;
                  DockArea.s_StartDragPosition = current.mousePosition;
                  DockArea.s_DragMode = 0;
                  current.Use();
                  break;
                case 2:
                  this.m_Panes[tabAtMousePos].Close();
                  current.Use();
                  break;
              }
            }
            else
              break;
          }
          else
            break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            Vector2 screenPoint = GUIUtility.GUIToScreenPoint(current.mousePosition);
            if (DockArea.s_DragMode != 0)
            {
              DockArea.s_DragMode = 0;
              PaneDragTab.get.Close();
              EditorApplication.update -= new EditorApplication.CallbackFunction(DockArea.CheckDragWindowExists);
              if (DockArea.s_DropInfo != null && DockArea.s_DropInfo.dropArea != null)
              {
                DockArea.s_DropInfo.dropArea.PerformDrop(DockArea.s_DragPane, DockArea.s_DropInfo, screenPoint);
              }
              else
              {
                EditorWindow dragPane = DockArea.s_DragPane;
                DockArea.ResetDragVars();
                this.RemoveTab(dragPane);
                Rect position = dragPane.position;
                position.x = screenPoint.x - position.width * 0.5f;
                position.y = screenPoint.y - position.height * 0.5f;
                if (Application.platform == RuntimePlatform.WindowsEditor)
                  position.y = Mathf.Max(InternalEditorUtility.GetBoundsOfDesktopAtPoint(screenPoint).y, position.y);
                EditorWindow.CreateNewWindowForEditorWindow(dragPane, false, false);
                dragPane.position = dragPane.m_Parent.window.FitWindowRectToScreen(position, true, true);
                GUIUtility.hotControl = 0;
                GUIUtility.ExitGUI();
              }
              DockArea.ResetDragVars();
            }
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            Vector2 vector2 = current.mousePosition - DockArea.s_StartDragPosition;
            current.Use();
            Rect screenPosition = this.screenPosition;
            if (DockArea.s_DragMode == 0 && (double) vector2.sqrMagnitude > 99.0)
            {
              DockArea.s_DragMode = 1;
              DockArea.s_PlaceholderPos = this.selected;
              DockArea.s_DragPane = this.m_Panes[this.selected];
              DockArea.s_IgnoreDockingForView = this.m_Panes.Count != 1 ? (View) null : (View) this;
              DockArea.s_OriginalDragSource = this;
              PaneDragTab.get.content = DockArea.s_DragPane.titleContent;
              this.Internal_SetAsActiveWindow();
              PaneDragTab.get.GrabThumbnail();
              PaneDragTab.get.Show(new Rect((float) ((double) pos.x + (double) screenPosition.x + (double) tabWidth * (double) this.selected), pos.y + screenPosition.y, tabWidth, pos.height), GUIUtility.GUIToScreenPoint(current.mousePosition));
              EditorApplication.update += new EditorApplication.CallbackFunction(DockArea.CheckDragWindowExists);
              GUIUtility.ExitGUI();
            }
            if (DockArea.s_DragMode == 1)
            {
              DropInfo di = (DropInfo) null;
              ContainerWindow[] windows = ContainerWindow.windows;
              Vector2 screenPoint = GUIUtility.GUIToScreenPoint(current.mousePosition);
              ContainerWindow inFrontOf = (ContainerWindow) null;
              foreach (ContainerWindow containerWindow in windows)
              {
                foreach (View allChild in containerWindow.mainView.allChildren)
                {
                  IDropArea dropArea = allChild as IDropArea;
                  if (dropArea != null)
                    di = dropArea.DragOver(DockArea.s_DragPane, screenPoint);
                  if (di != null)
                    break;
                }
                if (di != null)
                {
                  inFrontOf = containerWindow;
                  break;
                }
              }
              if (di == null)
                di = new DropInfo((IDropArea) null);
              if (di.type != DropInfo.Type.Tab)
                DockArea.s_PlaceholderPos = -1;
              DockArea.s_DropInfo = di;
              if ((bool) ((UnityEngine.Object) PaneDragTab.get.m_Window))
              {
                PaneDragTab.get.SetDropInfo(di, screenPoint, inFrontOf);
                break;
              }
              break;
            }
            break;
          }
          break;
        case EventType.Repaint:
          float xMin = pos.xMin;
          int num = 0;
          if ((bool) ((UnityEngine.Object) this.actualView))
          {
            for (int index = 0; index < this.m_Panes.Count; ++index)
            {
              if (!((UnityEngine.Object) DockArea.s_DragPane == (UnityEngine.Object) this.m_Panes[index]))
              {
                if (DockArea.s_DropInfo != null && object.ReferenceEquals((object) DockArea.s_DropInfo.dropArea, (object) this) && DockArea.s_PlaceholderPos == num)
                  xMin += tabWidth;
                Rect rect = new Rect(xMin, pos.yMin, tabWidth, pos.height);
                float x = Mathf.Round(rect.x);
                Rect position = new Rect(x, rect.y, Mathf.Round(rect.x + rect.width) - x, rect.height);
                tabStyle.Draw(position, this.m_Panes[index].titleContent, false, false, index == this.selected, this.hasFocus);
                xMin += tabWidth;
                ++num;
              }
            }
            break;
          }
          Rect rect1 = new Rect(xMin, pos.yMin, tabWidth, pos.height);
          float x1 = Mathf.Round(rect1.x);
          Rect position1 = new Rect(x1, rect1.y, Mathf.Round(rect1.x + rect1.width) - x1, rect1.height);
          tabStyle.Draw(position1, "Failed to load", false, false, true, false);
          break;
        default:
          if (typeForControl == EventType.ContextClick && pos.Contains(current.mousePosition) && GUIUtility.hotControl == 0)
          {
            int tabAtMousePos = this.GetTabAtMousePos(current.mousePosition, pos);
            if (tabAtMousePos < this.m_Panes.Count)
            {
              this.PopupGenericMenu(this.m_Panes[tabAtMousePos], new Rect(current.mousePosition.x, current.mousePosition.y, 0.0f, 0.0f));
              break;
            }
            break;
          }
          break;
      }
      this.selected = Mathf.Clamp(this.selected, 0, this.m_Panes.Count - 1);
    }

    protected override RectOffset GetBorderSize()
    {
      if (!(bool) ((UnityEngine.Object) this.window))
        return this.m_BorderSize;
      RectOffset borderSize = this.m_BorderSize;
      int num1 = 0;
      this.m_BorderSize.bottom = num1;
      int num2 = num1;
      this.m_BorderSize.top = num2;
      int num3 = num2;
      this.m_BorderSize.right = num3;
      int num4 = num3;
      borderSize.left = num4;
      Rect windowPosition = this.windowPosition;
      if ((double) windowPosition.xMin != 0.0)
        this.m_BorderSize.left += 2;
      if ((double) windowPosition.xMax != (double) this.window.position.width)
        this.m_BorderSize.right += 2;
      this.m_BorderSize.top = 17;
      bool flag1 = (double) this.windowPosition.y == 0.0;
      bool flag2 = (double) windowPosition.yMax == (double) this.window.position.height;
      this.m_BorderSize.bottom = 4;
      if (flag2)
        this.m_BorderSize.bottom -= 2;
      if (flag1)
        this.m_BorderSize.bottom += 3;
      return this.m_BorderSize;
    }

    private static void ResetDragVars()
    {
      DockArea.s_DragPane = (EditorWindow) null;
      DockArea.s_DropInfo = (DropInfo) null;
      DockArea.s_PlaceholderPos = -1;
      DockArea.s_DragMode = 0;
      DockArea.s_OriginalDragSource = (DockArea) null;
    }
  }
}
