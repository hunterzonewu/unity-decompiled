// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEditor.Modules;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Game", useTypeNameAsIconName = true)]
  internal class GameView : EditorWindow, IHasCustomMenu
  {
    private static List<GameView> s_GameViews = new List<GameView>();
    private static GameView s_LastFocusedGameView = (GameView) null;
    private static Rect s_MainGameViewRect = new Rect(0.0f, 0.0f, 640f, 480f);
    [SerializeField]
    private int[] m_SelectedSizes = new int[0];
    private int m_SizeChangeID = int.MinValue;
    private GUIContent gizmosContent = new GUIContent("Gizmos");
    private Vector2 m_ShownResolution = Vector2.zero;
    private AnimBool m_ResolutionTooLargeWarning = new AnimBool(false);
    private const int kToolbarHeight = 17;
    private const int kBorderSize = 5;
    [SerializeField]
    private bool m_MaximizeOnPlay;
    [SerializeField]
    private bool m_Gizmos;
    [SerializeField]
    private bool m_Stats;
    [SerializeField]
    private int m_TargetDisplay;
    private GUIContent renderdocContent;
    private static GUIStyle s_GizmoButtonStyle;
    private static GUIStyle s_ResolutionWarningStyle;

    public bool maximizeOnPlay
    {
      get
      {
        return this.m_MaximizeOnPlay;
      }
      set
      {
        this.m_MaximizeOnPlay = value;
      }
    }

    private int selectedSizeIndex
    {
      get
      {
        return this.m_SelectedSizes[(int) GameView.currentSizeGroupType];
      }
      set
      {
        this.m_SelectedSizes[(int) GameView.currentSizeGroupType] = value;
      }
    }

    private static GameViewSizeGroupType currentSizeGroupType
    {
      get
      {
        return ScriptableSingleton<GameViewSizes>.instance.currentGroupType;
      }
    }

    private GameViewSize currentGameViewSize
    {
      get
      {
        return ScriptableSingleton<GameViewSizes>.instance.currentGroup.GetGameViewSize(this.selectedSizeIndex);
      }
    }

    private Rect gameViewRenderRect
    {
      get
      {
        return new Rect(0.0f, 17f, this.position.width, this.position.height - 17f);
      }
    }

    public GameView()
    {
      this.depthBufferBits = 32;
      this.antiAlias = -1;
      this.autoRepaintOnSceneChange = true;
      this.m_TargetDisplay = 0;
    }

    public void OnValidate()
    {
      this.EnsureSelectedSizeAreValid();
    }

    public void OnEnable()
    {
      this.depthBufferBits = 32;
      this.titleContent = this.GetLocalizedTitleContent();
      this.EnsureSelectedSizeAreValid();
      this.renderdocContent = EditorGUIUtility.IconContent("renderdoc", "Capture|Capture the current view and open in RenderDoc");
      this.dontClearBackground = true;
      GameView.s_GameViews.Add(this);
      this.m_ResolutionTooLargeWarning.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_ResolutionTooLargeWarning.speed = 0.3f;
    }

    public void OnDisable()
    {
      GameView.s_GameViews.Remove(this);
      this.m_ResolutionTooLargeWarning.valueChanged.RemoveListener(new UnityAction(((EditorWindow) this).Repaint));
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.DoDelayedGameViewChanged);
    }

    internal static GameView GetMainGameView()
    {
      if ((UnityEngine.Object) GameView.s_LastFocusedGameView == (UnityEngine.Object) null && GameView.s_GameViews != null && GameView.s_GameViews.Count > 0)
        GameView.s_LastFocusedGameView = GameView.s_GameViews[0];
      return GameView.s_LastFocusedGameView;
    }

    public static void RepaintAll()
    {
      if (GameView.s_GameViews == null)
        return;
      using (List<GameView>.Enumerator enumerator = GameView.s_GameViews.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Repaint();
      }
    }

    internal static Vector2 GetSizeOfMainGameView()
    {
      Rect gameViewRenderRect = GameView.GetMainGameViewRenderRect();
      return new Vector2(gameViewRenderRect.width, gameViewRenderRect.height);
    }

    internal static Rect GetMainGameViewRenderRect()
    {
      GameView mainGameView = GameView.GetMainGameView();
      if ((UnityEngine.Object) mainGameView != (UnityEngine.Object) null)
        GameView.s_MainGameViewRect = mainGameView.GetConstrainedGameViewRenderRect();
      return GameView.s_MainGameViewRect;
    }

    private void GameViewAspectWasChanged()
    {
      this.SetInternalGameViewRect(GameView.GetConstrainedGameViewRenderRect(this.gameViewRenderRect, this.selectedSizeIndex));
      EditorApplication.SetSceneRepaintDirty();
    }

    private void AllowCursorLockAndHide(bool enable)
    {
      Unsupported.SetAllowCursorLock(enable);
      Unsupported.SetAllowCursorHide(enable);
    }

    private void OnFocus()
    {
      this.AllowCursorLockAndHide(true);
      GameView.s_LastFocusedGameView = this;
      InternalEditorUtility.OnGameViewFocus(true);
    }

    private void OnLostFocus()
    {
      if (!EditorApplicationLayout.IsInitializingPlaymodeLayout())
        this.AllowCursorLockAndHide(false);
      InternalEditorUtility.OnGameViewFocus(false);
    }

    private void DelayedGameViewChanged()
    {
      EditorApplication.update += new EditorApplication.CallbackFunction(this.DoDelayedGameViewChanged);
    }

    private void DoDelayedGameViewChanged()
    {
      this.GameViewAspectWasChanged();
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.DoDelayedGameViewChanged);
    }

    internal override void OnResized()
    {
      this.DelayedGameViewChanged();
    }

    private void EnsureSelectedSizeAreValid()
    {
      int length = Enum.GetNames(typeof (GameViewSizeGroupType)).Length;
      if (this.m_SelectedSizes.Length != length)
        Array.Resize<int>(ref this.m_SelectedSizes, length);
      foreach (int num in Enum.GetValues(typeof (GameViewSizeGroupType)))
      {
        GameViewSizeGroupType gameViewSizeGroupType = (GameViewSizeGroupType) num;
        GameViewSizeGroup group = ScriptableSingleton<GameViewSizes>.instance.GetGroup(gameViewSizeGroupType);
        int index = (int) gameViewSizeGroupType;
        this.m_SelectedSizes[index] = Mathf.Clamp(this.m_SelectedSizes[index], 0, group.GetTotalCount() - 1);
      }
    }

    public bool IsShowingGizmos()
    {
      return this.m_Gizmos;
    }

    private void OnSelectionChange()
    {
      if (!this.m_Gizmos)
        return;
      this.Repaint();
    }

    private void LoadRenderDoc()
    {
      if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        return;
      RenderDoc.Load();
      ShaderUtil.RecreateGfxDevice();
    }

    public virtual void AddItemsToMenu(GenericMenu menu)
    {
      if (!RenderDoc.IsInstalled() || RenderDoc.IsLoaded())
        return;
      menu.AddItem(new GUIContent("Load RenderDoc"), false, new GenericMenu.MenuFunction(this.LoadRenderDoc));
    }

    private bool ShouldShowMultiDisplayOption()
    {
      GUIContent[] displayNames = ModuleManager.GetDisplayNames(EditorUserBuildSettings.activeBuildTarget.ToString());
      if (BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) != BuildTargetGroup.Standalone)
        return displayNames != null;
      return true;
    }

    internal static Rect GetConstrainedGameViewRenderRect(Rect renderRect, int sizeIndex)
    {
      bool fitsInsideRect;
      return GameView.GetConstrainedGameViewRenderRect(renderRect, sizeIndex, out fitsInsideRect);
    }

    internal static Rect GetConstrainedGameViewRenderRect(Rect renderRect, int sizeIndex, out bool fitsInsideRect)
    {
      return GameViewSizes.GetConstrainedRect(renderRect, GameView.currentSizeGroupType, sizeIndex, out fitsInsideRect);
    }

    internal Rect GetConstrainedGameViewRenderRect()
    {
      if ((UnityEngine.Object) this.m_Parent == (UnityEngine.Object) null)
        return GameView.s_MainGameViewRect;
      this.m_Pos = this.m_Parent.borderSize.Remove(this.m_Parent.position);
      return EditorGUIUtility.PixelsToPoints(GameView.GetConstrainedGameViewRenderRect(EditorGUIUtility.PointsToPixels(this.gameViewRenderRect), this.selectedSizeIndex));
    }

    private void SelectionCallback(int indexClicked, object objectSelected)
    {
      if (indexClicked == this.selectedSizeIndex)
        return;
      this.selectedSizeIndex = indexClicked;
      this.dontClearBackground = true;
      this.GameViewAspectWasChanged();
    }

    private void DoToolbarGUI()
    {
      ScriptableSingleton<GameViewSizes>.instance.RefreshStandaloneAndWebplayerDefaultSizes();
      if (ScriptableSingleton<GameViewSizes>.instance.GetChangeID() != this.m_SizeChangeID)
      {
        this.EnsureSelectedSizeAreValid();
        this.m_SizeChangeID = ScriptableSingleton<GameViewSizes>.instance.GetChangeID();
      }
      GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      if (this.ShouldShowMultiDisplayOption())
      {
        int num = EditorGUILayout.Popup(this.m_TargetDisplay, DisplayUtility.GetDisplayNames(), EditorStyles.toolbarPopup, new GUILayoutOption[1]{ GUILayout.Width(80f) });
        EditorGUILayout.Space();
        if (num != this.m_TargetDisplay)
        {
          this.m_TargetDisplay = num;
          this.GameViewAspectWasChanged();
        }
      }
      EditorGUILayout.GameViewSizePopup(GameView.currentSizeGroupType, this.selectedSizeIndex, new System.Action<int, object>(this.SelectionCallback), EditorStyles.toolbarDropDown, GUILayout.Width(160f));
      if (FrameDebuggerUtility.IsLocalEnabled())
      {
        GUILayout.FlexibleSpace();
        Color color = GUI.color;
        GUI.color *= AnimationMode.animatedPropertyColor;
        GUILayout.Label("Frame Debugger on", EditorStyles.miniLabel, new GUILayoutOption[0]);
        GUI.color = color;
        if (Event.current.type == EventType.Repaint)
          FrameDebuggerWindow.RepaintAll();
      }
      GUILayout.FlexibleSpace();
      if (RenderDoc.IsLoaded())
      {
        EditorGUI.BeginDisabledGroup(!RenderDoc.IsSupported());
        if (GUILayout.Button(this.renderdocContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        {
          this.m_Parent.CaptureRenderDoc();
          GUIUtility.ExitGUI();
        }
        EditorGUI.EndDisabledGroup();
      }
      this.m_MaximizeOnPlay = GUILayout.Toggle(this.m_MaximizeOnPlay, "Maximize on Play", EditorStyles.toolbarButton, new GUILayoutOption[0]);
      EditorUtility.audioMasterMute = GUILayout.Toggle(EditorUtility.audioMasterMute, "Mute audio", EditorStyles.toolbarButton, new GUILayoutOption[0]);
      this.m_Stats = GUILayout.Toggle(this.m_Stats, "Stats", EditorStyles.toolbarButton, new GUILayoutOption[0]);
      Rect rect = GUILayoutUtility.GetRect(this.gizmosContent, GameView.s_GizmoButtonStyle);
      if (EditorGUI.ButtonMouseDown(new Rect(rect.xMax - (float) GameView.s_GizmoButtonStyle.border.right, rect.y, (float) GameView.s_GizmoButtonStyle.border.right, rect.height), GUIContent.none, FocusType.Passive, GUIStyle.none) && AnnotationWindow.ShowAtPosition(GUILayoutUtility.topLevel.GetLast(), true))
        GUIUtility.ExitGUI();
      this.m_Gizmos = GUI.Toggle(rect, this.m_Gizmos, this.gizmosContent, GameView.s_GizmoButtonStyle);
      GUILayout.EndHorizontal();
    }

    private void OnGUI()
    {
      if (GameView.s_GizmoButtonStyle == null)
      {
        GameView.s_GizmoButtonStyle = (GUIStyle) "GV Gizmo DropDown";
        GameView.s_ResolutionWarningStyle = new GUIStyle((GUIStyle) "PreOverlayLabel");
        GameView.s_ResolutionWarningStyle.alignment = TextAnchor.UpperLeft;
        GameView.s_ResolutionWarningStyle.padding = new RectOffset(6, 6, 1, 1);
      }
      this.DoToolbarGUI();
      Rect gameViewRenderRect1 = this.gameViewRenderRect;
      bool fitsInsideRect;
      Rect gameViewRenderRect2 = GameView.GetConstrainedGameViewRenderRect(EditorGUIUtility.PointsToPixels(gameViewRenderRect1), this.selectedSizeIndex, out fitsInsideRect);
      Rect points = EditorGUIUtility.PixelsToPoints(gameViewRenderRect2);
      Rect rect = GUIClip.Unclip(points);
      Rect pixels = EditorGUIUtility.PointsToPixels(rect);
      this.SetInternalGameViewRect(rect);
      EditorGUIUtility.AddCursorRect(points, MouseCursor.CustomCursor);
      EventType type = Event.current.type;
      if (type == EventType.MouseDown && gameViewRenderRect1.Contains(Event.current.mousePosition))
        this.AllowCursorLockAndHide(true);
      else if (type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        Unsupported.SetAllowCursorLock(false);
      if (type == EventType.Repaint)
      {
        if (!this.currentGameViewSize.isFreeAspectRatio || !InternalEditorUtility.HasFullscreenCamera() || !EditorGUIUtility.IsDisplayReferencedByCameras(this.m_TargetDisplay))
        {
          GUI.Box(gameViewRenderRect1, GUIContent.none, (GUIStyle) "GameViewBackground");
          if (!InternalEditorUtility.HasFullscreenCamera())
          {
            float[] numArray = new float[3]{ 30f, (float) ((double) gameViewRenderRect1.height / 2.0 - 10.0), (float) ((double) gameViewRenderRect1.height - 10.0) };
            foreach (int num in numArray)
              GUI.Label(new Rect((float) ((double) gameViewRenderRect1.width / 2.0 - 100.0), (float) num, 300f, 20f), "Scene is missing a fullscreen camera", (GUIStyle) "WhiteLargeLabel");
          }
        }
        Vector2 screenPointOffset = GUIUtility.s_EditorScreenPointOffset;
        GUIUtility.s_EditorScreenPointOffset = Vector2.zero;
        SavedGUIState savedGuiState = SavedGUIState.Create();
        if (this.ShouldShowMultiDisplayOption())
          EditorGUIUtility.RenderGameViewCamerasInternal(pixels, this.m_TargetDisplay, this.m_Gizmos, true);
        else
          EditorGUIUtility.RenderGameViewCamerasInternal(pixels, 0, this.m_Gizmos, true);
        GL.sRGBWrite = false;
        savedGuiState.ApplyAndForget();
        GUIUtility.s_EditorScreenPointOffset = screenPointOffset;
      }
      else if (type != EventType.Layout && type != EventType.Used)
      {
        if (WindowLayout.s_MaximizeKey.activated && (!EditorApplication.isPlaying || EditorApplication.isPaused))
          return;
        bool flag1 = points.Contains(Event.current.mousePosition);
        if (Event.current.rawType == EventType.MouseDown && !flag1)
          return;
        Vector2 mousePosition = Event.current.mousePosition;
        Event.current.mousePosition = EditorGUIUtility.PointsToPixels(mousePosition - points.position);
        Event.current.displayIndex = this.m_TargetDisplay;
        EditorGUIUtility.QueueGameViewInputEvent(Event.current);
        bool flag2 = true;
        if (Event.current.rawType == EventType.MouseUp && !flag1)
          flag2 = false;
        if (type == EventType.ExecuteCommand || type == EventType.ValidateCommand)
          flag2 = false;
        if (flag2)
          Event.current.Use();
        else
          Event.current.mousePosition = mousePosition;
      }
      this.ShowResolutionWarning(new Rect(gameViewRenderRect1.x, gameViewRenderRect1.y, 200f, 20f), fitsInsideRect, gameViewRenderRect2.size);
      if (!this.m_Stats)
        return;
      GameViewGUI.GameViewStatsGUI();
    }

    private void ShowResolutionWarning(Rect position, bool fitsInsideRect, Vector2 shownSize)
    {
      if (!fitsInsideRect && shownSize != this.m_ShownResolution)
      {
        this.m_ShownResolution = shownSize;
        this.m_ResolutionTooLargeWarning.value = true;
      }
      if (fitsInsideRect && this.m_ShownResolution != Vector2.zero)
      {
        this.m_ShownResolution = Vector2.zero;
        this.m_ResolutionTooLargeWarning.value = false;
      }
      this.m_ResolutionTooLargeWarning.target = !fitsInsideRect && !EditorApplication.isPlaying;
      if ((double) this.m_ResolutionTooLargeWarning.faded <= 0.0)
        return;
      Color color = GUI.color;
      GUI.color = new Color(1f, 1f, 1f, Mathf.Clamp01(this.m_ResolutionTooLargeWarning.faded * 2f));
      EditorGUI.DropShadowLabel(position, string.Format("Using resolution {0}x{1}", (object) shownSize.x, (object) shownSize.y), GameView.s_ResolutionWarningStyle);
      GUI.color = color;
    }
  }
}
