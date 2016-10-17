// Decompiled with JetBrains decompiler
// Type: UnityEditor.HostView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
  internal class HostView : GUIView
  {
    internal static Color kViewColor = new Color(0.76f, 0.76f, 0.76f, 1f);
    internal static PrefColor kPlayModeDarken = new PrefColor("Playmode tint", 0.8f, 0.8f, 0.8f, 1f);
    [NonSerialized]
    private Rect m_BackgroundClearRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    [NonSerialized]
    protected readonly RectOffset m_BorderSize = new RectOffset();
    internal GUIStyle background;
    [SerializeField]
    protected EditorWindow m_ActualView;

    internal EditorWindow actualView
    {
      get
      {
        return this.m_ActualView;
      }
      set
      {
        if ((UnityEngine.Object) this.m_ActualView == (UnityEngine.Object) value)
          return;
        this.DeregisterSelectedPane(true);
        this.m_ActualView = value;
        this.RegisterSelectedPane();
      }
    }

    internal RectOffset borderSize
    {
      get
      {
        return this.GetBorderSize();
      }
    }

    protected override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      if (!((UnityEngine.Object) this.m_ActualView != (UnityEngine.Object) null))
        return;
      this.m_ActualView.m_Pos = newPos;
      this.m_ActualView.OnResized();
    }

    public void OnEnable()
    {
      this.background = (GUIStyle) null;
      this.RegisterSelectedPane();
    }

    private void OnDisable()
    {
      this.DeregisterSelectedPane(false);
    }

    private void OnGUI()
    {
      EditorGUIUtility.ResetGUIState();
      this.DoWindowDecorationStart();
      if (this.background == null)
      {
        this.background = (GUIStyle) "hostview";
        this.background.padding.top = 0;
      }
      GUILayout.BeginVertical(this.background, new GUILayoutOption[0]);
      if ((bool) ((UnityEngine.Object) this.actualView))
        this.actualView.m_Pos = this.screenPosition;
      this.Invoke("OnGUI");
      EditorGUIUtility.ResetGUIState();
      if ((double) this.m_ActualView.m_FadeoutTime != 0.0 && Event.current.type == EventType.Repaint)
        this.m_ActualView.DrawNotification();
      GUILayout.EndVertical();
      this.DoWindowDecorationEnd();
      EditorGUI.ShowRepaints();
    }

    protected override bool OnFocus()
    {
      this.Invoke("OnFocus");
      if ((UnityEngine.Object) this == (UnityEngine.Object) null)
        return false;
      this.Repaint();
      return true;
    }

    private void OnLostFocus()
    {
      this.Invoke("OnLostFocus");
      this.Repaint();
    }

    public new void OnDestroy()
    {
      if ((bool) ((UnityEngine.Object) this.m_ActualView))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ActualView, true);
      base.OnDestroy();
    }

    protected System.Type[] GetPaneTypes()
    {
      return new System.Type[7]
      {
        typeof (SceneView),
        typeof (GameView),
        typeof (InspectorWindow),
        typeof (SceneHierarchyWindow),
        typeof (ProjectBrowser),
        typeof (ProfilerWindow),
        typeof (AnimationWindow)
      };
    }

    internal void OnProjectChange()
    {
      this.Invoke("OnProjectChange");
    }

    internal void OnSelectionChange()
    {
      this.Invoke("OnSelectionChange");
    }

    internal void OnDidOpenScene()
    {
      this.Invoke("OnDidOpenScene");
    }

    internal void OnInspectorUpdate()
    {
      this.Invoke("OnInspectorUpdate");
    }

    internal void OnHierarchyChange()
    {
      this.Invoke("OnHierarchyChange");
    }

    private MethodInfo GetPaneMethod(string methodName)
    {
      return this.GetPaneMethod(methodName, (object) this.m_ActualView);
    }

    private MethodInfo GetPaneMethod(string methodName, object obj)
    {
      if (obj == null)
        return (MethodInfo) null;
      System.Type type = obj.GetType();
      for (; type != null; type = type.BaseType)
      {
        MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (method != null)
          return method;
      }
      return (MethodInfo) null;
    }

    protected void Invoke(string methodName)
    {
      this.Invoke(methodName, (object) this.m_ActualView);
    }

    protected void Invoke(string methodName, object obj)
    {
      MethodInfo paneMethod = this.GetPaneMethod(methodName, obj);
      if (paneMethod == null)
        return;
      paneMethod.Invoke(obj, (object[]) null);
    }

    protected void RegisterSelectedPane()
    {
      if (!(bool) ((UnityEngine.Object) this.m_ActualView))
        return;
      this.m_ActualView.m_Parent = this;
      if (this.GetPaneMethod("Update") != null)
        EditorApplication.update += new EditorApplication.CallbackFunction(this.SendUpdate);
      if (this.GetPaneMethod("ModifierKeysChanged") != null)
        EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(this.SendModKeysChanged);
      this.m_ActualView.MakeParentsSettingsMatchMe();
      if ((double) this.m_ActualView.m_FadeoutTime != 0.0)
        EditorApplication.update += new EditorApplication.CallbackFunction(this.m_ActualView.CheckForWindowRepaint);
      try
      {
        this.Invoke("OnBecameVisible");
        this.Invoke("OnFocus");
      }
      catch (TargetInvocationException ex)
      {
        Debug.LogError((object) (ex.InnerException.GetType().Name + ":" + ex.InnerException.Message));
      }
    }

    protected void DeregisterSelectedPane(bool clearActualView)
    {
      if (!(bool) ((UnityEngine.Object) this.m_ActualView))
        return;
      if (this.GetPaneMethod("Update") != null)
        EditorApplication.update -= new EditorApplication.CallbackFunction(this.SendUpdate);
      if (this.GetPaneMethod("ModifierKeysChanged") != null)
        EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(this.SendModKeysChanged);
      if ((double) this.m_ActualView.m_FadeoutTime != 0.0)
        EditorApplication.update -= new EditorApplication.CallbackFunction(this.m_ActualView.CheckForWindowRepaint);
      if (!clearActualView)
        return;
      EditorWindow actualView = this.m_ActualView;
      this.m_ActualView = (EditorWindow) null;
      this.Invoke("OnLostFocus", (object) actualView);
      this.Invoke("OnBecameInvisible", (object) actualView);
    }

    private void SendUpdate()
    {
      this.Invoke("Update");
    }

    private void SendModKeysChanged()
    {
      this.Invoke("ModifierKeysChanged");
    }

    protected virtual RectOffset GetBorderSize()
    {
      return this.m_BorderSize;
    }

    protected void ShowGenericMenu()
    {
      GUIStyle guiStyle = (GUIStyle) "PaneOptions";
      Rect rect = new Rect((float) ((double) this.position.width - (double) guiStyle.fixedWidth - 4.0), Mathf.Floor((float) (this.background.margin.top + 20) - guiStyle.fixedHeight), guiStyle.fixedWidth, guiStyle.fixedHeight);
      if (EditorGUI.ButtonMouseDown(rect, GUIContent.none, FocusType.Passive, (GUIStyle) "PaneOptions"))
        this.PopupGenericMenu(this.m_ActualView, rect);
      MethodInfo paneMethod = this.GetPaneMethod("ShowButton", (object) this.m_ActualView);
      if (paneMethod == null)
        return;
      object[] parameters = new object[1]
      {
        (object) new Rect((float) ((double) this.position.width - (double) guiStyle.fixedWidth - 20.0), Mathf.Floor((float) (this.background.margin.top + 4)), 16f, 16f)
      };
      paneMethod.Invoke((object) this.m_ActualView, parameters);
    }

    public void PopupGenericMenu(EditorWindow view, Rect pos)
    {
      GenericMenu menu = new GenericMenu();
      IHasCustomMenu hasCustomMenu = view as IHasCustomMenu;
      if (hasCustomMenu != null)
        hasCustomMenu.AddItemsToMenu(menu);
      this.AddDefaultItemsToMenu(menu, view);
      menu.DropDown(pos);
      Event.current.Use();
    }

    protected virtual void AddDefaultItemsToMenu(GenericMenu menu, EditorWindow view)
    {
    }

    protected void ClearBackground()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      EditorWindow actualView = this.actualView;
      if ((UnityEngine.Object) actualView != (UnityEngine.Object) null && actualView.dontClearBackground && (this.backgroundValid && this.position == this.m_BackgroundClearRect))
        return;
      Color color = !EditorGUIUtility.isProSkin ? HostView.kViewColor : EditorGUIUtility.kDarkViewBackground;
      GL.Clear(true, true, !EditorApplication.isPlayingOrWillChangePlaymode ? color : color * (Color) HostView.kPlayModeDarken);
      this.backgroundValid = true;
      this.m_BackgroundClearRect = this.position;
    }
  }
}
