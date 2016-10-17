// Decompiled with JetBrains decompiler
// Type: UnityEditor.Toolbar
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Connect;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class Toolbar : GUIView
  {
    private static GUIContent[] s_ShownToolIcons = new GUIContent[5];
    public static Toolbar get = (Toolbar) null;
    private static GUIContent[] s_ToolIcons;
    private static GUIContent[] s_ViewToolIcons;
    private static GUIContent[] s_PivotIcons;
    private static GUIContent[] s_PivotRotation;
    private static GUIContent s_LayerContent;
    private static GUIContent[] s_PlayIcons;
    private static GUIContent s_CloudIcon;
    private bool t1;
    private bool t2;
    private bool t3;
    [SerializeField]
    private string m_LastLoadedLayoutName;

    internal static string lastLoadedLayoutName
    {
      get
      {
        if (string.IsNullOrEmpty(Toolbar.get.m_LastLoadedLayoutName))
          return "Layout";
        return Toolbar.get.m_LastLoadedLayoutName;
      }
      set
      {
        Toolbar.get.m_LastLoadedLayoutName = value;
        Toolbar.get.Repaint();
      }
    }

    private static void InitializeToolIcons()
    {
      if (Toolbar.s_ToolIcons != null)
        return;
      Toolbar.s_ToolIcons = new GUIContent[8]
      {
        EditorGUIUtility.IconContent("MoveTool", "|Move the selected objects."),
        EditorGUIUtility.IconContent("RotateTool", "|Rotate the selected objects."),
        EditorGUIUtility.IconContent("ScaleTool", "|Scale the selected objects."),
        EditorGUIUtility.IconContent("RectTool"),
        EditorGUIUtility.IconContent("MoveTool On"),
        EditorGUIUtility.IconContent("RotateTool On"),
        EditorGUIUtility.IconContent("ScaleTool On"),
        EditorGUIUtility.IconContent("RectTool On")
      };
      Toolbar.s_ViewToolIcons = new GUIContent[8]
      {
        EditorGUIUtility.IconContent("ViewToolOrbit", "|Orbit the Scene view."),
        EditorGUIUtility.IconContent("ViewToolMove"),
        EditorGUIUtility.IconContent("ViewToolZoom"),
        EditorGUIUtility.IconContent("ViewToolOrbit", "|Orbit the Scene view."),
        EditorGUIUtility.IconContent("ViewToolOrbit On"),
        EditorGUIUtility.IconContent("ViewToolMove On"),
        EditorGUIUtility.IconContent("ViewToolZoom On"),
        EditorGUIUtility.IconContent("ViewToolOrbit On")
      };
      Toolbar.s_PivotIcons = new GUIContent[2]
      {
        EditorGUIUtility.TextContentWithIcon("Center|The tool handle is placed at the center of the selection.", "ToolHandleCenter"),
        EditorGUIUtility.TextContentWithIcon("Pivot|The tool handle is placed at the active object's pivot point.", "ToolHandlePivot")
      };
      Toolbar.s_PivotRotation = new GUIContent[2]
      {
        EditorGUIUtility.TextContentWithIcon("Local|Tool handles are in active object's rotation.", "ToolHandleLocal"),
        EditorGUIUtility.TextContentWithIcon("Global|Tool handles are in global rotation.", "ToolHandleGlobal")
      };
      Toolbar.s_LayerContent = EditorGUIUtility.TextContent("Layers|Which layers are visible in the Scene views.");
      Toolbar.s_PlayIcons = new GUIContent[12]
      {
        EditorGUIUtility.IconContent("PlayButton"),
        EditorGUIUtility.IconContent("PauseButton"),
        EditorGUIUtility.IconContent("StepButton"),
        EditorGUIUtility.IconContent("PlayButtonProfile"),
        EditorGUIUtility.IconContent("PlayButton On"),
        EditorGUIUtility.IconContent("PauseButton On"),
        EditorGUIUtility.IconContent("StepButton On"),
        EditorGUIUtility.IconContent("PlayButtonProfile On"),
        EditorGUIUtility.IconContent("PlayButton Anim"),
        EditorGUIUtility.IconContent("PauseButton Anim"),
        EditorGUIUtility.IconContent("StepButton Anim"),
        EditorGUIUtility.IconContent("PlayButtonProfile Anim")
      };
      Toolbar.s_CloudIcon = EditorGUIUtility.IconContent("CloudConnect");
    }

    public void OnEnable()
    {
      EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(((GUIView) this).Repaint);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.OnSelectionChange);
      UnityConnect.instance.StateChanged += new StateChangedDelegate(this.OnUnityConnectStateChanged);
      Toolbar.get = this;
    }

    public void OnDisable()
    {
      EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(((GUIView) this).Repaint);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.OnSelectionChange);
      UnityConnect.instance.StateChanged -= new StateChangedDelegate(this.OnUnityConnectStateChanged);
    }

    protected override bool OnFocus()
    {
      return false;
    }

    private void OnSelectionChange()
    {
      Tools.OnSelectionChange();
      this.Repaint();
    }

    protected void OnUnityConnectStateChanged(ConnectInfo state)
    {
      this.Repaint();
    }

    private Rect GetThinArea(Rect pos)
    {
      return new Rect(pos.x, 7f, pos.width, 18f);
    }

    private Rect GetThickArea(Rect pos)
    {
      return new Rect(pos.x, 5f, pos.width, 24f);
    }

    private void ReserveWidthLeft(float width, ref Rect pos)
    {
      pos.x -= width;
      pos.width = width;
    }

    private void ReserveWidthRight(float width, ref Rect pos)
    {
      pos.x += pos.width;
      pos.width = width;
    }

    private void OnGUI()
    {
      float width1 = 10f;
      float width2 = 20f;
      float num1 = 32f;
      float num2 = 64f;
      float width3 = 80f;
      Toolbar.InitializeToolIcons();
      bool willChangePlaymode = EditorApplication.isPlayingOrWillChangePlaymode;
      if (willChangePlaymode)
        GUI.color = (Color) HostView.kPlayModeDarken;
      GUIStyle guiStyle = (GUIStyle) "AppToolbar";
      if (Event.current.type == EventType.Repaint)
        guiStyle.Draw(new Rect(0.0f, 0.0f, this.position.width, this.position.height), false, false, false, false);
      Rect pos = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      this.ReserveWidthRight(width1, ref pos);
      this.ReserveWidthRight(num1 * 5f, ref pos);
      this.DoToolButtons(this.GetThickArea(pos));
      this.ReserveWidthRight(width2, ref pos);
      this.ReserveWidthRight(num2 * 2f, ref pos);
      this.DoPivotButtons(this.GetThinArea(pos));
      pos = new Rect((float) (((double) this.position.width - 100.0) / 2.0), 0.0f, 140f, 0.0f);
      GUILayout.BeginArea(this.GetThickArea(pos));
      GUILayout.BeginHorizontal();
      this.DoPlayButtons(willChangePlaymode);
      GUILayout.EndHorizontal();
      GUILayout.EndArea();
      pos = new Rect(this.position.width, 0.0f, 0.0f, 0.0f);
      this.ReserveWidthLeft(width1, ref pos);
      this.ReserveWidthLeft(width3, ref pos);
      this.DoLayoutDropDown(this.GetThinArea(pos));
      this.ReserveWidthLeft(width1, ref pos);
      this.ReserveWidthLeft(width3, ref pos);
      this.DoLayersDropDown(this.GetThinArea(pos));
      this.ReserveWidthLeft(width2, ref pos);
      this.ReserveWidthLeft(width3, ref pos);
      if (EditorGUI.ButtonMouseDown(this.GetThinArea(pos), new GUIContent("Account"), FocusType.Passive, (GUIStyle) "Dropdown"))
        this.ShowUserMenu(this.GetThinArea(pos));
      this.ReserveWidthLeft(width1, ref pos);
      this.ReserveWidthLeft(32f, ref pos);
      if (GUI.Button(this.GetThinArea(pos), Toolbar.s_CloudIcon, (GUIStyle) "Button"))
        UnityConnectServiceCollection.instance.ShowService("Hub", true);
      EditorGUI.ShowRepaints();
      Highlighter.ControlHighlightGUI((GUIView) this);
    }

    private void ShowUserMenu(Rect dropDownRect)
    {
      GenericMenu genericMenu = new GenericMenu();
      if (!UnityConnect.instance.online)
      {
        genericMenu.AddDisabledItem(new GUIContent("Go to account"));
        genericMenu.AddDisabledItem(new GUIContent("Sign in..."));
        if (!Application.HasProLicense())
        {
          genericMenu.AddSeparator(string.Empty);
          genericMenu.AddDisabledItem(new GUIContent("Upgrade to Pro"));
        }
      }
      else
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        Toolbar.\u003CShowUserMenu\u003Ec__AnonStorey77 menuCAnonStorey77 = new Toolbar.\u003CShowUserMenu\u003Ec__AnonStorey77();
        // ISSUE: reference to a compiler-generated field
        menuCAnonStorey77.accountUrl = UnityConnect.instance.GetConfigurationURL(CloudConfigUrl.CloudWebauth);
        if (UnityConnect.instance.loggedIn)
        {
          // ISSUE: reference to a compiler-generated method
          genericMenu.AddItem(new GUIContent("Go to account"), false, new GenericMenu.MenuFunction(menuCAnonStorey77.\u003C\u003Em__10F));
        }
        else
          genericMenu.AddDisabledItem(new GUIContent("Go to account"));
        if (UnityConnect.instance.loggedIn)
        {
          string text = "Sign out " + UnityConnect.instance.userInfo.displayName;
          genericMenu.AddItem(new GUIContent(text), false, (GenericMenu.MenuFunction) (() => UnityConnect.instance.Logout()));
        }
        else
          genericMenu.AddItem(new GUIContent("Sign in..."), false, (GenericMenu.MenuFunction) (() => UnityConnect.instance.ShowLogin()));
        if (!Application.HasProLicense())
        {
          genericMenu.AddSeparator(string.Empty);
          genericMenu.AddItem(new GUIContent("Upgrade to Pro"), false, (GenericMenu.MenuFunction) (() => Application.OpenURL("https://store.unity3d.com/")));
        }
      }
      genericMenu.DropDown(dropDownRect);
    }

    private void DoToolButtons(Rect rect)
    {
      GUI.changed = false;
      int selected = !Tools.viewToolActive ? (int) Tools.current : 0;
      for (int index = 1; index < 5; ++index)
      {
        Toolbar.s_ShownToolIcons[index] = Toolbar.s_ToolIcons[index - 1 + (index != selected ? 0 : 4)];
        Toolbar.s_ShownToolIcons[index].tooltip = Toolbar.s_ToolIcons[index - 1].tooltip;
      }
      Toolbar.s_ShownToolIcons[0] = Toolbar.s_ViewToolIcons[(int) (Tools.viewTool + (selected != 0 ? 0 : 4))];
      int num = GUI.Toolbar(rect, selected, Toolbar.s_ShownToolIcons, (GUIStyle) "Command");
      if (!GUI.changed)
        return;
      Tools.current = (Tool) num;
    }

    private void DoPivotButtons(Rect rect)
    {
      Tools.pivotMode = (PivotMode) EditorGUI.CycleButton(new Rect(rect.x, rect.y, rect.width / 2f, rect.height), (int) Tools.pivotMode, Toolbar.s_PivotIcons, (GUIStyle) "ButtonLeft");
      if (Tools.current == Tool.Scale && Selection.transforms.Length < 2)
        GUI.enabled = false;
      PivotRotation pivotRotation = (PivotRotation) EditorGUI.CycleButton(new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height), (int) Tools.pivotRotation, Toolbar.s_PivotRotation, (GUIStyle) "ButtonRight");
      if (Tools.pivotRotation != pivotRotation)
      {
        Tools.pivotRotation = pivotRotation;
        if (pivotRotation == PivotRotation.Global)
          Tools.ResetGlobalHandleRotation();
      }
      if (Tools.current == Tool.Scale)
        GUI.enabled = true;
      if (!GUI.changed)
        return;
      Tools.RepaintAllToolViews();
    }

    private void DoPlayButtons(bool isOrWillEnterPlaymode)
    {
      bool isPlaying = EditorApplication.isPlaying;
      GUI.changed = false;
      int index = !isPlaying ? 0 : 4;
      if (AnimationMode.InAnimationMode())
        index = 8;
      Color color = GUI.color + new Color(0.01f, 0.01f, 0.01f, 0.01f);
      GUI.contentColor = new Color(1f / color.r, 1f / color.g, 1f / color.g, 1f / color.a);
      GUILayout.Toggle(isOrWillEnterPlaymode, Toolbar.s_PlayIcons[index], (GUIStyle) "CommandLeft", new GUILayoutOption[0]);
      GUI.backgroundColor = Color.white;
      if (GUI.changed)
      {
        Toolbar.TogglePlaying();
        GUIUtility.ExitGUI();
      }
      GUI.changed = false;
      bool flag = GUILayout.Toggle(EditorApplication.isPaused, Toolbar.s_PlayIcons[index + 1], (GUIStyle) "CommandMid", new GUILayoutOption[0]);
      if (GUI.changed)
      {
        EditorApplication.isPaused = flag;
        GUIUtility.ExitGUI();
      }
      if (!GUILayout.Button(Toolbar.s_PlayIcons[index + 2], (GUIStyle) "CommandRight", new GUILayoutOption[0]))
        return;
      EditorApplication.Step();
      GUIUtility.ExitGUI();
    }

    private void DoLayersDropDown(Rect rect)
    {
      GUIStyle style = (GUIStyle) "DropDown";
      if (!EditorGUI.ButtonMouseDown(rect, Toolbar.s_LayerContent, FocusType.Passive, style) || !LayerVisibilityWindow.ShowAtPosition(rect))
        return;
      GUIUtility.ExitGUI();
    }

    private void DoLayoutDropDown(Rect rect)
    {
      if (!EditorGUI.ButtonMouseDown(rect, GUIContent.Temp(Toolbar.lastLoadedLayoutName), FocusType.Passive, (GUIStyle) "DropDown"))
        return;
      Vector2 screenPoint = GUIUtility.GUIToScreenPoint(new Vector2(rect.x, rect.y));
      rect.x = screenPoint.x;
      rect.y = screenPoint.y;
      EditorUtility.Internal_DisplayPopupMenu(rect, "Window/Layouts", (Object) this, 0);
    }

    private static void InternalWillTogglePlaymode()
    {
      InternalEditorUtility.RepaintAllViews();
    }

    private static void TogglePlaying()
    {
      EditorApplication.isPlaying = !EditorApplication.isPlaying;
      Toolbar.InternalWillTogglePlaymode();
    }

    internal static void RepaintToolbar()
    {
      if (!((Object) Toolbar.get != (Object) null))
        return;
      Toolbar.get.Repaint();
    }

    public float CalcHeight()
    {
      return 30f;
    }
  }
}
