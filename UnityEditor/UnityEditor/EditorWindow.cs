// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Derive from this class to create an editor window.</para>
  /// </summary>
  public class EditorWindow : ScriptableObject
  {
    [SerializeField]
    [HideInInspector]
    private Vector2 m_MinSize = new Vector2(100f, 100f);
    [HideInInspector]
    [SerializeField]
    private Vector2 m_MaxSize = new Vector2(4000f, 4000f);
    [HideInInspector]
    [SerializeField]
    internal Rect m_Pos = new Rect(0.0f, 0.0f, 320f, 240f);
    private const double kWarningFadeoutWait = 4.0;
    private const double kWarningFadeoutTime = 1.0;
    [HideInInspector]
    [SerializeField]
    private bool m_AutoRepaintOnSceneChange;
    [SerializeField]
    [HideInInspector]
    internal GUIContent m_TitleContent;
    [HideInInspector]
    [SerializeField]
    private int m_DepthBufferBits;
    [SerializeField]
    [HideInInspector]
    private int m_AntiAlias;
    private Rect m_GameViewRect;
    private bool m_DontClearBackground;
    private bool m_WantsMouseMove;
    [NonSerialized]
    internal HostView m_Parent;
    internal GUIContent m_Notification;
    private Vector2 m_NotificationSize;
    internal float m_FadeoutTime;

    /// <summary>
    ///   <para>Does the GUI in this editor window want MouseMove events?</para>
    /// </summary>
    public bool wantsMouseMove
    {
      get
      {
        return this.m_WantsMouseMove;
      }
      set
      {
        this.m_WantsMouseMove = value;
        this.MakeParentsSettingsMatchMe();
      }
    }

    internal bool dontClearBackground
    {
      get
      {
        return this.m_DontClearBackground;
      }
      set
      {
        this.m_DontClearBackground = value;
        if (!(bool) ((UnityEngine.Object) this.m_Parent) || !((UnityEngine.Object) this.m_Parent.actualView == (UnityEngine.Object) this))
          return;
        this.m_Parent.backgroundValid = false;
      }
    }

    /// <summary>
    ///   <para>Does the window automatically repaint whenever the scene has changed?</para>
    /// </summary>
    public bool autoRepaintOnSceneChange
    {
      get
      {
        return this.m_AutoRepaintOnSceneChange;
      }
      set
      {
        this.m_AutoRepaintOnSceneChange = value;
        this.MakeParentsSettingsMatchMe();
      }
    }

    /// <summary>
    ///   <para>Is this window maximized.</para>
    /// </summary>
    public bool maximized
    {
      get
      {
        return WindowLayout.IsMaximized(this);
      }
      set
      {
        bool flag = WindowLayout.IsMaximized(this);
        if (value == flag)
          return;
        if (value)
          WindowLayout.Maximize(this);
        else
          WindowLayout.Unmaximize(this);
      }
    }

    internal bool hasFocus
    {
      get
      {
        if ((bool) ((UnityEngine.Object) this.m_Parent))
          return (UnityEngine.Object) this.m_Parent.actualView == (UnityEngine.Object) this;
        return false;
      }
    }

    internal bool docked
    {
      get
      {
        if ((UnityEngine.Object) this.m_Parent != (UnityEngine.Object) null && (UnityEngine.Object) this.m_Parent.window != (UnityEngine.Object) null)
          return !this.m_Parent.window.IsNotDocked();
        return false;
      }
    }

    /// <summary>
    ///   <para>The EditorWindow which currently has keyboard focus. (Read Only)</para>
    /// </summary>
    public static EditorWindow focusedWindow
    {
      get
      {
        HostView focusedView = GUIView.focusedView as HostView;
        if ((UnityEngine.Object) focusedView != (UnityEngine.Object) null)
          return focusedView.actualView;
        return (EditorWindow) null;
      }
    }

    /// <summary>
    ///   <para>The EditorWindow currently under the mouse cursor. (Read Only)</para>
    /// </summary>
    public static EditorWindow mouseOverWindow
    {
      get
      {
        HostView mouseOverView = GUIView.mouseOverView as HostView;
        if ((UnityEngine.Object) mouseOverView != (UnityEngine.Object) null)
          return mouseOverView.actualView;
        return (EditorWindow) null;
      }
    }

    /// <summary>
    ///   <para>The minimum size of this window.</para>
    /// </summary>
    public Vector2 minSize
    {
      get
      {
        return this.m_MinSize;
      }
      set
      {
        this.m_MinSize = value;
        this.MakeParentsSettingsMatchMe();
      }
    }

    /// <summary>
    ///   <para>The maximum size of this window.</para>
    /// </summary>
    public Vector2 maxSize
    {
      get
      {
        return this.m_MaxSize;
      }
      set
      {
        this.m_MaxSize = value;
        this.MakeParentsSettingsMatchMe();
      }
    }

    /// <summary>
    ///   <para>The title of this window.</para>
    /// </summary>
    [Obsolete("Use titleContent instead (it supports setting a title icon as well).")]
    public string title
    {
      get
      {
        return this.titleContent.text;
      }
      set
      {
        this.titleContent = EditorGUIUtility.TextContent(value);
      }
    }

    /// <summary>
    ///   <para>The GUIContent used for drawing the title of EditorWindows.</para>
    /// </summary>
    public GUIContent titleContent
    {
      get
      {
        return this.m_TitleContent ?? (this.m_TitleContent = new GUIContent());
      }
      set
      {
        this.m_TitleContent = value;
        if (this.m_TitleContent == null || !(bool) ((UnityEngine.Object) this.m_Parent) || (!(bool) ((UnityEngine.Object) this.m_Parent.window) || !((UnityEngine.Object) this.m_Parent.window.mainView == (UnityEngine.Object) this.m_Parent)))
          return;
        this.m_Parent.window.title = this.m_TitleContent.text;
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

    /// <summary>
    ///   <para>The position of the window in screen space.</para>
    /// </summary>
    public Rect position
    {
      get
      {
        return this.m_Pos;
      }
      set
      {
        this.m_Pos = value;
        if (!(bool) ((UnityEngine.Object) this.m_Parent))
          return;
        DockArea parent = this.m_Parent as DockArea;
        if (!(bool) ((UnityEngine.Object) parent))
          this.m_Parent.window.position = value;
        else if (!(bool) ((UnityEngine.Object) parent) || (bool) ((UnityEngine.Object) parent.parent) && parent.m_Panes.Count == 1 && !(bool) ((UnityEngine.Object) parent.parent.parent))
        {
          parent.window.position = parent.borderSize.Add(value);
        }
        else
        {
          parent.RemoveTab(this);
          EditorWindow.CreateNewWindowForEditorWindow(this, true, true);
        }
      }
    }

    public EditorWindow()
    {
      this.titleContent.text = this.GetType().ToString();
      this.hideFlags = HideFlags.DontSave;
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal void MakeModal(ContainerWindow win);

    /// <summary>
    ///   <para>Returns the first EditorWindow of type t which is currently on the screen.</para>
    /// </summary>
    /// <param name="t">The type of the window. Must derive from EditorWindow.</param>
    /// <param name="utility">Set this to true, to create a floating utility window, false to create a normal window.</param>
    /// <param name="title">If GetWindow creates a new window, it will get this title. If this value is null, use the class name as title.</param>
    /// <param name="focus">Whether to give the window focus, if it already exists. (If GetWindow creates a new window, it will always get focus).</param>
    [ExcludeFromDocs]
    public static EditorWindow GetWindow(System.Type t, bool utility, string title)
    {
      bool focus = true;
      return EditorWindow.GetWindow(t, utility, title, focus);
    }

    /// <summary>
    ///   <para>Returns the first EditorWindow of type t which is currently on the screen.</para>
    /// </summary>
    /// <param name="t">The type of the window. Must derive from EditorWindow.</param>
    /// <param name="utility">Set this to true, to create a floating utility window, false to create a normal window.</param>
    /// <param name="title">If GetWindow creates a new window, it will get this title. If this value is null, use the class name as title.</param>
    /// <param name="focus">Whether to give the window focus, if it already exists. (If GetWindow creates a new window, it will always get focus).</param>
    [ExcludeFromDocs]
    public static EditorWindow GetWindow(System.Type t, bool utility)
    {
      bool focus = true;
      string title = (string) null;
      return EditorWindow.GetWindow(t, utility, title, focus);
    }

    /// <summary>
    ///   <para>Returns the first EditorWindow of type t which is currently on the screen.</para>
    /// </summary>
    /// <param name="t">The type of the window. Must derive from EditorWindow.</param>
    /// <param name="utility">Set this to true, to create a floating utility window, false to create a normal window.</param>
    /// <param name="title">If GetWindow creates a new window, it will get this title. If this value is null, use the class name as title.</param>
    /// <param name="focus">Whether to give the window focus, if it already exists. (If GetWindow creates a new window, it will always get focus).</param>
    [ExcludeFromDocs]
    public static EditorWindow GetWindow(System.Type t)
    {
      bool focus = true;
      string title = (string) null;
      bool utility = false;
      return EditorWindow.GetWindow(t, utility, title, focus);
    }

    /// <summary>
    ///   <para>Returns the first EditorWindow of type t which is currently on the screen.</para>
    /// </summary>
    /// <param name="t">The type of the window. Must derive from EditorWindow.</param>
    /// <param name="utility">Set this to true, to create a floating utility window, false to create a normal window.</param>
    /// <param name="title">If GetWindow creates a new window, it will get this title. If this value is null, use the class name as title.</param>
    /// <param name="focus">Whether to give the window focus, if it already exists. (If GetWindow creates a new window, it will always get focus).</param>
    public static EditorWindow GetWindow(System.Type t, [DefaultValue("false")] bool utility, [DefaultValue("null")] string title, [DefaultValue("true")] bool focus)
    {
      return EditorWindow.GetWindowPrivate(t, utility, title, focus);
    }

    /// <summary>
    ///   <para>Returns the first EditorWindow of type t which is currently on the screen.</para>
    /// </summary>
    /// <param name="t">The type of the window. Must derive from EditorWindow.</param>
    /// <param name="rect">The position on the screen where a newly created window will show.</param>
    /// <param name="utility">Set this to true, to create a floating utility window, false to create a normal window.</param>
    /// <param name="title">If GetWindow creates a new window, it will get this title. If this value is null, use the class name as title.</param>
    [ExcludeFromDocs]
    public static EditorWindow GetWindowWithRect(System.Type t, Rect rect, bool utility)
    {
      string title = (string) null;
      return EditorWindow.GetWindowWithRect(t, rect, utility, title);
    }

    /// <summary>
    ///   <para>Returns the first EditorWindow of type t which is currently on the screen.</para>
    /// </summary>
    /// <param name="t">The type of the window. Must derive from EditorWindow.</param>
    /// <param name="rect">The position on the screen where a newly created window will show.</param>
    /// <param name="utility">Set this to true, to create a floating utility window, false to create a normal window.</param>
    /// <param name="title">If GetWindow creates a new window, it will get this title. If this value is null, use the class name as title.</param>
    [ExcludeFromDocs]
    public static EditorWindow GetWindowWithRect(System.Type t, Rect rect)
    {
      string title = (string) null;
      bool utility = false;
      return EditorWindow.GetWindowWithRect(t, rect, utility, title);
    }

    /// <summary>
    ///   <para>Returns the first EditorWindow of type t which is currently on the screen.</para>
    /// </summary>
    /// <param name="t">The type of the window. Must derive from EditorWindow.</param>
    /// <param name="rect">The position on the screen where a newly created window will show.</param>
    /// <param name="utility">Set this to true, to create a floating utility window, false to create a normal window.</param>
    /// <param name="title">If GetWindow creates a new window, it will get this title. If this value is null, use the class name as title.</param>
    public static EditorWindow GetWindowWithRect(System.Type t, Rect rect, [DefaultValue("false")] bool utility, [DefaultValue("null")] string title)
    {
      return EditorWindow.GetWindowWithRectPrivate(t, rect, utility, title);
    }

    /// <summary>
    ///   <para>Mark the beginning area of all popup windows.</para>
    /// </summary>
    public void BeginWindows()
    {
      EditorGUIInternal.BeginWindowsForward(1, this.GetInstanceID());
    }

    /// <summary>
    ///   <para>Close a window group started with EditorWindow.BeginWindows.</para>
    /// </summary>
    public void EndWindows()
    {
      GUI.EndWindows();
    }

    internal virtual void OnResized()
    {
    }

    internal void CheckForWindowRepaint()
    {
      double timeSinceStartup = EditorApplication.timeSinceStartup;
      if (timeSinceStartup < (double) this.m_FadeoutTime)
        return;
      if (timeSinceStartup > (double) this.m_FadeoutTime + 1.0)
        this.RemoveNotification();
      else
        this.Repaint();
    }

    internal GUIContent GetLocalizedTitleContent()
    {
      return EditorWindow.GetLocalizedTitleContentFromType(this.GetType());
    }

    internal static GUIContent GetLocalizedTitleContentFromType(System.Type t)
    {
      EditorWindowTitleAttribute windowTitleAttribute = EditorWindow.GetEditorWindowTitleAttribute(t);
      if (windowTitleAttribute == null)
        return new GUIContent(t.ToString());
      string icon = string.Empty;
      if (!string.IsNullOrEmpty(windowTitleAttribute.icon))
        icon = windowTitleAttribute.icon;
      else if (windowTitleAttribute.useTypeNameAsIconName)
        icon = t.ToString();
      if (!string.IsNullOrEmpty(icon))
        return EditorGUIUtility.TextContentWithIcon(windowTitleAttribute.title, icon);
      return EditorGUIUtility.TextContent(windowTitleAttribute.title);
    }

    private static EditorWindowTitleAttribute GetEditorWindowTitleAttribute(System.Type t)
    {
      foreach (object customAttribute in t.GetCustomAttributes(true))
      {
        if (((Attribute) customAttribute).TypeId == typeof (EditorWindowTitleAttribute))
          return (EditorWindowTitleAttribute) customAttribute;
      }
      return (EditorWindowTitleAttribute) null;
    }

    /// <summary>
    ///   <para>Show a notification message.</para>
    /// </summary>
    /// <param name="notification"></param>
    public void ShowNotification(GUIContent notification)
    {
      this.m_Notification = new GUIContent(notification);
      EditorStyles.notificationText.CalcMinMaxWidth(this.m_Notification, out this.m_NotificationSize.y, out this.m_NotificationSize.x);
      this.m_NotificationSize.y = EditorStyles.notificationText.CalcHeight(this.m_Notification, this.m_NotificationSize.x);
      if ((double) this.m_FadeoutTime == 0.0)
        EditorApplication.update += new EditorApplication.CallbackFunction(this.CheckForWindowRepaint);
      this.m_FadeoutTime = (float) (EditorApplication.timeSinceStartup + 4.0);
    }

    /// <summary>
    ///   <para>Stop showing notification message.</para>
    /// </summary>
    public void RemoveNotification()
    {
      if ((double) this.m_FadeoutTime == 0.0)
        return;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.CheckForWindowRepaint);
      this.m_Notification = (GUIContent) null;
      this.m_FadeoutTime = 0.0f;
    }

    internal void DrawNotification()
    {
      Vector2 notificationSize = this.m_NotificationSize;
      float num1 = this.position.width - (float) EditorStyles.notificationText.margin.horizontal;
      float num2 = (float) ((double) this.position.height - (double) EditorStyles.notificationText.margin.vertical - 20.0);
      if ((double) num1 < (double) this.m_NotificationSize.x)
      {
        float num3 = num1 / this.m_NotificationSize.x;
        notificationSize.x *= num3;
        notificationSize.y = EditorStyles.notificationText.CalcHeight(this.m_Notification, notificationSize.x);
      }
      if ((double) notificationSize.y > (double) num2)
        notificationSize.y = num2;
      Rect position = new Rect((float) (((double) this.position.width - (double) notificationSize.x) * 0.5), (float) (20.0 + ((double) this.position.height - 20.0 - (double) notificationSize.y) * 0.699999988079071), notificationSize.x, notificationSize.y);
      double timeSinceStartup = EditorApplication.timeSinceStartup;
      if (timeSinceStartup > (double) this.m_FadeoutTime)
        GUI.color = new Color(1f, 1f, 1f, 1f - (float) ((timeSinceStartup - (double) this.m_FadeoutTime) / 1.0));
      GUI.Label(position, GUIContent.none, EditorStyles.notificationBackground);
      EditorGUI.DoDropShadowLabel(position, this.m_Notification, EditorStyles.notificationText, 0.3f);
    }

    internal int GetNumTabs()
    {
      DockArea parent = this.m_Parent as DockArea;
      if ((bool) ((UnityEngine.Object) parent))
        return parent.m_Panes.Count;
      return 0;
    }

    internal bool ShowNextTabIfPossible()
    {
      DockArea parent = this.m_Parent as DockArea;
      if ((bool) ((UnityEngine.Object) parent))
      {
        int num = (parent.m_Panes.IndexOf(this) + 1) % parent.m_Panes.Count;
        if (parent.selected != num)
        {
          parent.selected = num;
          parent.Repaint();
          return true;
        }
      }
      return false;
    }

    public void ShowTab()
    {
      DockArea parent = this.m_Parent as DockArea;
      if ((bool) ((UnityEngine.Object) parent))
      {
        int num = parent.m_Panes.IndexOf(this);
        if (parent.selected != num)
          parent.selected = num;
      }
      this.Repaint();
    }

    /// <summary>
    ///   <para>Moves keyboard focus to this EditorWindow.</para>
    /// </summary>
    public void Focus()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Parent))
        return;
      this.ShowTab();
      this.m_Parent.Focus();
    }

    internal void MakeParentsSettingsMatchMe()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Parent) || (UnityEngine.Object) this.m_Parent.actualView != (UnityEngine.Object) this)
        return;
      this.m_Parent.SetTitle(this.GetType().FullName);
      this.m_Parent.autoRepaintOnSceneChange = this.m_AutoRepaintOnSceneChange;
      bool flag = this.m_Parent.antiAlias != this.m_AntiAlias || this.m_Parent.depthBufferBits != this.m_DepthBufferBits;
      this.m_Parent.antiAlias = this.m_AntiAlias;
      this.m_Parent.depthBufferBits = this.m_DepthBufferBits;
      this.m_Parent.SetInternalGameViewRect(this.m_GameViewRect);
      this.m_Parent.wantsMouseMove = this.m_WantsMouseMove;
      Vector2 vector2 = new Vector2((float) (this.m_Parent.borderSize.left + this.m_Parent.borderSize.right), (float) (this.m_Parent.borderSize.top + this.m_Parent.borderSize.bottom));
      this.m_Parent.SetMinMaxSizes(this.minSize + vector2, this.maxSize + vector2);
      if (!flag)
        return;
      this.m_Parent.RecreateContext();
    }

    /// <summary>
    ///   <para>Show the EditorWindow as a floating utility window.</para>
    /// </summary>
    public void ShowUtility()
    {
      this.ShowWithMode(ShowMode.Utility);
    }

    /// <summary>
    ///   <para>Shows an Editor window using popup-style framing.</para>
    /// </summary>
    public void ShowPopup()
    {
      if (!((UnityEngine.Object) this.m_Parent == (UnityEngine.Object) null))
        return;
      ContainerWindow instance1 = ScriptableObject.CreateInstance<ContainerWindow>();
      instance1.title = this.titleContent.text;
      HostView instance2 = ScriptableObject.CreateInstance<HostView>();
      instance2.actualView = this;
      Rect rect = this.m_Parent.borderSize.Add(new Rect(this.position.x, this.position.y, this.position.width, this.position.height));
      instance1.position = rect;
      instance1.mainView = (View) instance2;
      this.MakeParentsSettingsMatchMe();
      instance1.ShowPopup();
    }

    internal void ShowWithMode(ShowMode mode)
    {
      if (!((UnityEngine.Object) this.m_Parent == (UnityEngine.Object) null))
        return;
      SavedGUIState savedGuiState = SavedGUIState.Create();
      ContainerWindow instance1 = ScriptableObject.CreateInstance<ContainerWindow>();
      instance1.title = this.titleContent.text;
      HostView instance2 = ScriptableObject.CreateInstance<HostView>();
      instance2.actualView = this;
      Rect rect = this.m_Parent.borderSize.Add(new Rect(this.position.x, this.position.y, this.position.width, this.position.height));
      instance1.position = rect;
      instance1.mainView = (View) instance2;
      this.MakeParentsSettingsMatchMe();
      instance1.Show(mode, true, false);
      savedGuiState.ApplyAndForget();
    }

    /// <summary>
    ///   <para>Show window with dropdown behaviour (e.g. window is closed when it loses focus) and having.</para>
    /// </summary>
    /// <param name="buttonRect">Is used for positioning the window.</param>
    /// <param name="windowSize">Is used for setting up initial size of the window.</param>
    public void ShowAsDropDown(Rect buttonRect, Vector2 windowSize)
    {
      this.ShowAsDropDown(buttonRect, windowSize, (PopupLocationHelper.PopupLocation[]) null);
    }

    internal void ShowAsDropDown(Rect buttonRect, Vector2 windowSize, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
    {
      this.position = this.ShowAsDropDownFitToScreen(buttonRect, windowSize, locationPriorityOrder);
      this.ShowWithMode(ShowMode.PopupMenu);
      this.position = this.ShowAsDropDownFitToScreen(buttonRect, windowSize, locationPriorityOrder);
      this.minSize = new Vector2(this.position.width, this.position.height);
      this.maxSize = new Vector2(this.position.width, this.position.height);
      if ((UnityEngine.Object) EditorWindow.focusedWindow != (UnityEngine.Object) this)
        this.Focus();
      this.m_Parent.AddToAuxWindowList();
      this.m_Parent.window.m_DontSaveToLayout = true;
    }

    internal Rect ShowAsDropDownFitToScreen(Rect buttonRect, Vector2 windowSize, PopupLocationHelper.PopupLocation[] locationPriorityOrder)
    {
      if ((UnityEngine.Object) this.m_Parent == (UnityEngine.Object) null)
        return new Rect(buttonRect.x, buttonRect.yMax, windowSize.x, windowSize.y);
      return this.m_Parent.window.GetDropDownRect(buttonRect, windowSize, windowSize, locationPriorityOrder);
    }

    /// <summary>
    ///   <para>Show the EditorWindow.</para>
    /// </summary>
    /// <param name="immediateDisplay"></param>
    public void Show()
    {
      this.Show(false);
    }

    /// <summary>
    ///   <para>Show the EditorWindow.</para>
    /// </summary>
    /// <param name="immediateDisplay"></param>
    public void Show(bool immediateDisplay)
    {
      if (!((UnityEngine.Object) this.m_Parent == (UnityEngine.Object) null))
        return;
      EditorWindow.CreateNewWindowForEditorWindow(this, true, immediateDisplay);
    }

    /// <summary>
    ///   <para>Show the editor window in the auxiliary window.</para>
    /// </summary>
    public void ShowAuxWindow()
    {
      this.ShowWithMode(ShowMode.AuxWindow);
      this.Focus();
      this.m_Parent.AddToAuxWindowList();
    }

    internal void ShowModal()
    {
      this.ShowWithMode(ShowMode.AuxWindow);
      this.MakeModal(this.m_Parent.window);
    }

    private static EditorWindow GetWindowPrivate(System.Type t, bool utility, string title, bool focus)
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(t);
      EditorWindow editorWindow = objectsOfTypeAll.Length <= 0 ? (EditorWindow) null : (EditorWindow) objectsOfTypeAll[0];
      if (!(bool) ((UnityEngine.Object) editorWindow))
      {
        editorWindow = ScriptableObject.CreateInstance(t) as EditorWindow;
        if (title != null)
          editorWindow.titleContent = new GUIContent(title);
        if (utility)
          editorWindow.ShowUtility();
        else
          editorWindow.Show();
      }
      else if (focus)
      {
        editorWindow.Show();
        editorWindow.Focus();
      }
      return editorWindow;
    }

    public static T GetWindow<T>() where T : EditorWindow
    {
      return EditorWindow.GetWindow<T>(false, (string) null, true);
    }

    public static T GetWindow<T>(bool utility) where T : EditorWindow
    {
      return EditorWindow.GetWindow<T>(utility, (string) null, true);
    }

    public static T GetWindow<T>(bool utility, string title) where T : EditorWindow
    {
      return EditorWindow.GetWindow<T>(utility, title, true);
    }

    public static T GetWindow<T>(string title) where T : EditorWindow
    {
      return EditorWindow.GetWindow<T>(title, true);
    }

    public static T GetWindow<T>(string title, bool focus) where T : EditorWindow
    {
      return EditorWindow.GetWindow<T>(false, title, focus);
    }

    public static T GetWindow<T>(bool utility, string title, bool focus) where T : EditorWindow
    {
      return EditorWindow.GetWindow(typeof (T), utility, title, focus) as T;
    }

    public static T GetWindow<T>(params System.Type[] desiredDockNextTo) where T : EditorWindow
    {
      return EditorWindow.GetWindow<T>((string) null, true, desiredDockNextTo);
    }

    public static T GetWindow<T>(string title, params System.Type[] desiredDockNextTo) where T : EditorWindow
    {
      return EditorWindow.GetWindow<T>(title, true, desiredDockNextTo);
    }

    public static T GetWindow<T>(string title, bool focus, params System.Type[] desiredDockNextTo) where T : EditorWindow
    {
      T[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (T)) as T[];
      T obj = objectsOfTypeAll.Length <= 0 ? (T) null : objectsOfTypeAll[0];
      if ((UnityEngine.Object) obj != (UnityEngine.Object) null)
      {
        if (focus)
          obj.Focus();
        return obj;
      }
      T instance = ScriptableObject.CreateInstance<T>();
      if (title != null)
        instance.titleContent = new GUIContent(title);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EditorWindow.\u003CGetWindow\u003Ec__AnonStorey12<T> windowCAnonStorey12 = new EditorWindow.\u003CGetWindow\u003Ec__AnonStorey12<T>();
      foreach (System.Type type in desiredDockNextTo)
      {
        // ISSUE: reference to a compiler-generated field
        windowCAnonStorey12.desired = type;
        foreach (ContainerWindow window in ContainerWindow.windows)
        {
          foreach (View allChild in window.mainView.allChildren)
          {
            DockArea dockArea = allChild as DockArea;
            // ISSUE: reference to a compiler-generated method
            if (!((UnityEngine.Object) dockArea == (UnityEngine.Object) null) && dockArea.m_Panes.Any<EditorWindow>(new Func<EditorWindow, bool>(windowCAnonStorey12.\u003C\u003Em__13)))
            {
              dockArea.AddTab((EditorWindow) instance);
              return instance;
            }
          }
        }
      }
      instance.Show();
      return instance;
    }

    /// <summary>
    ///   <para>Focuses the first found EditorWindow of specified type if it is open.</para>
    /// </summary>
    /// <param name="t">The type of the window. Must derive from EditorWindow.</param>
    public static void FocusWindowIfItsOpen(System.Type t)
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(t);
      EditorWindow editorWindow = objectsOfTypeAll.Length <= 0 ? (EditorWindow) null : objectsOfTypeAll[0] as EditorWindow;
      if (!(bool) ((UnityEngine.Object) editorWindow))
        return;
      editorWindow.Focus();
    }

    public static void FocusWindowIfItsOpen<T>() where T : EditorWindow
    {
      EditorWindow.FocusWindowIfItsOpen(typeof (T));
    }

    internal void RemoveFromDockArea()
    {
      DockArea parent = this.m_Parent as DockArea;
      if (!(bool) ((UnityEngine.Object) parent))
        return;
      parent.RemoveTab(this, true);
    }

    private static EditorWindow GetWindowWithRectPrivate(System.Type t, Rect rect, bool utility, string title)
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(t);
      EditorWindow editorWindow = objectsOfTypeAll.Length <= 0 ? (EditorWindow) null : (EditorWindow) objectsOfTypeAll[0];
      if (!(bool) ((UnityEngine.Object) editorWindow))
      {
        editorWindow = ScriptableObject.CreateInstance(t) as EditorWindow;
        editorWindow.minSize = new Vector2(rect.width, rect.height);
        editorWindow.maxSize = new Vector2(rect.width, rect.height);
        editorWindow.position = rect;
        if (title != null)
          editorWindow.titleContent = new GUIContent(title);
        if (utility)
          editorWindow.ShowUtility();
        else
          editorWindow.Show();
      }
      else
        editorWindow.Focus();
      return editorWindow;
    }

    public static T GetWindowWithRect<T>(Rect rect) where T : EditorWindow
    {
      return EditorWindow.GetWindowWithRect<T>(rect, false, (string) null, true);
    }

    public static T GetWindowWithRect<T>(Rect rect, bool utility) where T : EditorWindow
    {
      return EditorWindow.GetWindowWithRect<T>(rect, utility, (string) null, true);
    }

    public static T GetWindowWithRect<T>(Rect rect, bool utility, string title) where T : EditorWindow
    {
      return EditorWindow.GetWindowWithRect<T>(rect, utility, title, true);
    }

    public static T GetWindowWithRect<T>(Rect rect, bool utility, string title, bool focus) where T : EditorWindow
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (T));
      T instance;
      if (objectsOfTypeAll.Length > 0)
      {
        instance = (T) objectsOfTypeAll[0];
        if (focus)
          instance.Focus();
      }
      else
      {
        instance = ScriptableObject.CreateInstance<T>();
        instance.minSize = new Vector2(rect.width, rect.height);
        instance.maxSize = new Vector2(rect.width, rect.height);
        instance.position = rect;
        if (title != null)
          instance.titleContent = new GUIContent(title);
        if (utility)
          instance.ShowUtility();
        else
          instance.Show();
      }
      return instance;
    }

    internal static T GetWindowDontShow<T>() where T : EditorWindow
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (T));
      if (objectsOfTypeAll.Length > 0)
        return (T) objectsOfTypeAll[0];
      return ScriptableObject.CreateInstance<T>();
    }

    /// <summary>
    ///   <para>Close the editor window.</para>
    /// </summary>
    public void Close()
    {
      if (WindowLayout.IsMaximized(this))
        WindowLayout.Unmaximize(this);
      DockArea parent = this.m_Parent as DockArea;
      if ((bool) ((UnityEngine.Object) parent))
        parent.RemoveTab(this, true);
      else
        this.m_Parent.window.Close();
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this, true);
    }

    /// <summary>
    ///   <para>Make the window repaint.</para>
    /// </summary>
    public void Repaint()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Parent) || !((UnityEngine.Object) this.m_Parent.actualView == (UnityEngine.Object) this))
        return;
      this.m_Parent.Repaint();
    }

    internal void RepaintImmediately()
    {
      if (!(bool) ((UnityEngine.Object) this.m_Parent) || !((UnityEngine.Object) this.m_Parent.actualView == (UnityEngine.Object) this))
        return;
      this.m_Parent.RepaintImmediately();
    }

    internal Rect GetCurrentGameViewRect()
    {
      return this.m_GameViewRect;
    }

    internal void SetInternalGameViewRect(Rect rect)
    {
      this.m_GameViewRect = rect;
      this.m_Parent.SetInternalGameViewRect(this.m_GameViewRect);
    }

    /// <summary>
    ///   <para>Sends an Event to a window.</para>
    /// </summary>
    /// <param name="e"></param>
    public bool SendEvent(Event e)
    {
      return this.m_Parent.SendEvent(e);
    }

    internal static void CreateNewWindowForEditorWindow(EditorWindow window, bool loadPosition, bool showImmediately)
    {
      EditorWindow.CreateNewWindowForEditorWindow(window, new Vector2(window.position.x, window.position.y), loadPosition, showImmediately);
    }

    internal static void CreateNewWindowForEditorWindow(EditorWindow window, Vector2 screenPosition, bool loadPosition, bool showImmediately)
    {
      ContainerWindow instance1 = ScriptableObject.CreateInstance<ContainerWindow>();
      SplitView instance2 = ScriptableObject.CreateInstance<SplitView>();
      instance1.mainView = (View) instance2;
      DockArea instance3 = ScriptableObject.CreateInstance<DockArea>();
      instance2.AddChild((View) instance3);
      instance3.AddTab(window);
      Rect rect = window.m_Parent.borderSize.Add(new Rect(screenPosition.x, screenPosition.y, window.position.width, window.position.height));
      instance1.position = rect;
      instance2.position = new Rect(0.0f, 0.0f, rect.width, rect.height);
      window.MakeParentsSettingsMatchMe();
      instance1.Show(ShowMode.NormalWindow, loadPosition, showImmediately);
      instance1.OnResize();
    }

    [ContextMenu("Add Scene")]
    internal void AddSceneTab()
    {
    }

    [ContextMenu("Add Game")]
    internal void AddGameTab()
    {
    }
  }
}
