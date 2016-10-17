// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Scene", useTypeNameAsIconName = true)]
  public class SceneView : SearchableEditorWindow, IHasCustomMenu
  {
    private static readonly PrefColor kSceneViewBackground = new PrefColor("Scene/Background", 0.278431f, 0.278431f, 0.278431f, 0.0f);
    private static readonly PrefColor kSceneViewWire = new PrefColor("Scene/Wireframe", 0.0f, 0.0f, 0.0f, 0.5f);
    private static readonly PrefColor kSceneViewWireOverlay = new PrefColor("Scene/Wireframe Overlay", 0.0f, 0.0f, 0.0f, 0.25f);
    private static readonly PrefColor kSceneViewWireActive = new PrefColor("Scene/Wireframe Active", 0.4901961f, 0.6901961f, 0.9803922f, 0.372549f);
    private static readonly PrefColor kSceneViewWireSelected = new PrefColor("Scene/Wireframe Selected", 0.3686275f, 0.4666667f, 0.6078432f, 0.25f);
    internal static Color kSceneViewFrontLight = new Color(0.769f, 0.769f, 0.769f, 1f);
    internal static Color kSceneViewUpLight = new Color(0.212f, 0.227f, 0.259f, 1f);
    internal static Color kSceneViewMidLight = new Color(57f / 500f, 0.125f, 0.133f, 1f);
    internal static Color kSceneViewDownLight = new Color(0.047f, 0.043f, 0.035f, 1f);
    [NonSerialized]
    private static readonly Quaternion kDefaultRotation = Quaternion.LookRotation(new Vector3(-1f, -0.7f, -1f));
    [NonSerialized]
    private static readonly Vector3 kDefaultPivot = Vector3.zero;
    private static readonly PrefKey k2DMode = new PrefKey("Tools/2D Mode", "2");
    private static MouseCursor s_LastCursor = MouseCursor.Arrow;
    private static readonly List<SceneView.CursorRect> s_MouseRects = new List<SceneView.CursorRect>();
    private static ArrayList s_SceneViews = new ArrayList();
    [SerializeField]
    public bool m_SceneLighting = true;
    [SerializeField]
    private AnimVector3 m_Position = new AnimVector3(SceneView.kDefaultPivot);
    [SerializeField]
    internal AnimQuaternion m_Rotation = new AnimQuaternion(SceneView.kDefaultRotation);
    [SerializeField]
    private AnimFloat m_Size = new AnimFloat(10f);
    [SerializeField]
    internal AnimBool m_Ortho = new AnimBool();
    [NonSerialized]
    private Light[] m_Light = new Light[3];
    private double m_StartSearchFilterTime = -1.0;
    private const float kDefaultViewSize = 10f;
    private const float kOrthoThresholdAngle = 3f;
    private const float kOneOverSqrt2 = 0.7071068f;
    private const double k_MaxDoubleKeypressTime = 0.5;
    private const float kPerspectiveFov = 90f;
    public const float kToolbarHeight = 17f;
    private static SceneView s_LastActiveSceneView;
    private static SceneView s_CurrentDrawingSceneView;
    [NonSerialized]
    private ActiveEditorTracker m_Tracker;
    public double lastFramingTime;
    private static bool waitingFor2DModeKeyUp;
    [SerializeField]
    private bool m_2DMode;
    internal UnityEngine.Object m_OneClickDragObject;
    public bool m_AudioPlay;
    private static SceneView s_AudioSceneView;
    public static SceneView.OnSceneFunc onSceneGUIDelegate;
    public DrawCameraMode m_RenderMode;
    [SerializeField]
    internal SceneView.SceneViewState m_SceneViewState;
    [SerializeField]
    private SceneViewGrid grid;
    [SerializeField]
    internal SceneViewRotation svRot;
    [NonSerialized]
    private Camera m_Camera;
    [SerializeField]
    private Quaternion m_LastSceneViewRotation;
    [SerializeField]
    private bool m_LastSceneViewOrtho;
    private bool s_DraggingCursorIsCached;
    private RectSelection m_RectSelection;
    private static Material s_AlphaOverlayMaterial;
    private static Material s_DeferredOverlayMaterial;
    private static Shader s_ShowOverdrawShader;
    private static Shader s_ShowMipsShader;
    private static Shader s_ShowLightmapsShader;
    private static Shader s_AuraShader;
    private static Shader s_GrayScaleShader;
    private static Texture2D s_MipColorsTexture;
    private GUIContent m_Lighting;
    private GUIContent m_Fx;
    private GUIContent m_AudioPlayContent;
    private GUIContent m_GizmosContent;
    private GUIContent m_2DModeContent;
    private GUIContent m_RenderDocContent;
    private static Tool s_CurrentTool;
    private RenderTexture m_SceneTargetTexture;
    private RenderTexture m_SceneTargetTextureLDR;
    private int m_MainViewControlID;
    [SerializeField]
    private Shader m_ReplacementShader;
    [SerializeField]
    private string m_ReplacementString;
    internal bool m_ShowSceneViewWindows;
    private SceneViewOverlay m_SceneViewOverlay;
    private EditorCache m_DragEditorCache;
    private SceneView.DraggingLockedState m_DraggingLockedState;
    [SerializeField]
    private UnityEngine.Object m_LastLockedObject;
    [SerializeField]
    private bool m_ViewIsLockedToObject;
    private static GUIStyle s_DropDownStyle;
    private bool m_RequestedSceneViewFiltering;
    private double m_lastRenderedTime;

    public static SceneView lastActiveSceneView
    {
      get
      {
        return SceneView.s_LastActiveSceneView;
      }
    }

    public static SceneView currentDrawingSceneView
    {
      get
      {
        return SceneView.s_CurrentDrawingSceneView;
      }
    }

    public bool in2DMode
    {
      get
      {
        return this.m_2DMode;
      }
      set
      {
        if (this.m_2DMode == value || Tools.viewTool == ViewTool.FPS || Tools.viewTool == ViewTool.Orbit)
          return;
        this.m_2DMode = value;
        this.On2DModeChange();
      }
    }

    public DrawCameraMode renderMode
    {
      get
      {
        return this.m_RenderMode;
      }
      set
      {
        this.m_RenderMode = value;
      }
    }

    public Quaternion lastSceneViewRotation
    {
      get
      {
        if (this.m_LastSceneViewRotation == new Quaternion(0.0f, 0.0f, 0.0f, 0.0f))
          this.m_LastSceneViewRotation = Quaternion.identity;
        return this.m_LastSceneViewRotation;
      }
      set
      {
        this.m_LastSceneViewRotation = value;
      }
    }

    internal float cameraDistance
    {
      get
      {
        float num = this.m_Ortho.Fade(90f, 0.0f);
        if (!this.camera.orthographic)
          return this.size / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)));
        return this.size * 2f;
      }
    }

    public static ArrayList sceneViews
    {
      get
      {
        return SceneView.s_SceneViews;
      }
    }

    public Camera camera
    {
      get
      {
        return this.m_Camera;
      }
    }

    internal SceneView.DraggingLockedState draggingLocked
    {
      get
      {
        return this.m_DraggingLockedState;
      }
      set
      {
        this.m_DraggingLockedState = value;
      }
    }

    internal bool viewIsLockedToObject
    {
      get
      {
        return this.m_ViewIsLockedToObject;
      }
      set
      {
        this.m_LastLockedObject = !value ? (UnityEngine.Object) null : Selection.activeObject;
        this.m_ViewIsLockedToObject = value;
        this.draggingLocked = SceneView.DraggingLockedState.LookAt;
      }
    }

    private GUIStyle effectsDropDownStyle
    {
      get
      {
        if (SceneView.s_DropDownStyle == null)
          SceneView.s_DropDownStyle = (GUIStyle) "GV Gizmo DropDown";
        return SceneView.s_DropDownStyle;
      }
    }

    public Vector3 pivot
    {
      get
      {
        return this.m_Position.value;
      }
      set
      {
        this.m_Position.value = value;
      }
    }

    public Quaternion rotation
    {
      get
      {
        return this.m_Rotation.value;
      }
      set
      {
        this.m_Rotation.value = value;
      }
    }

    public float size
    {
      get
      {
        return this.m_Size.value;
      }
      set
      {
        if ((double) value > 40000.0)
          value = 40000f;
        this.m_Size.value = value;
      }
    }

    public bool orthographic
    {
      get
      {
        return this.m_Ortho.value;
      }
      set
      {
        this.m_Ortho.value = value;
      }
    }

    internal Quaternion cameraTargetRotation
    {
      get
      {
        return this.m_Rotation.target;
      }
    }

    internal Vector3 cameraTargetPosition
    {
      get
      {
        return this.m_Position.target + this.m_Rotation.target * new Vector3(0.0f, 0.0f, this.cameraDistance);
      }
    }

    public SceneView()
    {
      this.m_HierarchyType = HierarchyType.GameObjects;
    }

    internal static void AddCursorRect(Rect rect, MouseCursor cursor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      SceneView.s_MouseRects.Add(new SceneView.CursorRect(rect, cursor));
    }

    public void SetSceneViewShaderReplace(Shader shader, string replaceString)
    {
      this.m_ReplacementShader = shader;
      this.m_ReplacementString = replaceString;
    }

    public static bool FrameLastActiveSceneView()
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null)
        return false;
      return SceneView.lastActiveSceneView.SendEvent(EditorGUIUtility.CommandEvent("FrameSelected"));
    }

    public static bool FrameLastActiveSceneViewWithLock()
    {
      if ((UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) null)
        return false;
      return SceneView.lastActiveSceneView.SendEvent(EditorGUIUtility.CommandEvent("FrameSelectedWithLock"));
    }

    private Editor[] GetActiveEditors()
    {
      if (this.m_Tracker == null)
        this.m_Tracker = ActiveEditorTracker.sharedTracker;
      return this.m_Tracker.activeEditors;
    }

    public static Camera[] GetAllSceneCameras()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < SceneView.s_SceneViews.Count; ++index)
      {
        Camera camera = ((SceneView) SceneView.s_SceneViews[index]).m_Camera;
        if ((UnityEngine.Object) camera != (UnityEngine.Object) null)
          arrayList.Add((object) camera);
      }
      return (Camera[]) arrayList.ToArray(typeof (Camera));
    }

    public static void RepaintAll()
    {
      foreach (EditorWindow sceneView in SceneView.s_SceneViews)
        sceneView.Repaint();
    }

    internal override void SetSearchFilter(string searchFilter, SearchableEditorWindow.SearchMode mode, bool setAll)
    {
      if (this.m_SearchFilter == string.Empty || searchFilter == string.Empty)
        this.m_StartSearchFilterTime = EditorApplication.timeSinceStartup;
      base.SetSearchFilter(searchFilter, mode, setAll);
    }

    internal void OnFocus()
    {
      if (Application.isPlaying || !this.m_AudioPlay || !((UnityEngine.Object) this.m_Camera != (UnityEngine.Object) null))
        return;
      this.RefreshAudioPlay();
    }

    internal void OnLostFocus()
    {
      GameView editorWindowOfType = (GameView) WindowLayout.FindEditorWindowOfType(typeof (GameView));
      if ((bool) ((UnityEngine.Object) editorWindowOfType) && (UnityEngine.Object) editorWindowOfType.m_Parent != (UnityEngine.Object) null && ((UnityEngine.Object) this.m_Parent != (UnityEngine.Object) null && (UnityEngine.Object) editorWindowOfType.m_Parent == (UnityEngine.Object) this.m_Parent))
        editorWindowOfType.m_Parent.backgroundValid = false;
      if (!((UnityEngine.Object) SceneView.s_LastActiveSceneView == (UnityEngine.Object) this))
        return;
      SceneViewMotion.ResetMotion();
    }

    public override void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
      this.m_RectSelection = new RectSelection((EditorWindow) this);
      if (this.grid == null)
        this.grid = new SceneViewGrid();
      this.grid.Register(this);
      if (this.svRot == null)
        this.svRot = new SceneViewRotation();
      this.svRot.Register(this);
      this.autoRepaintOnSceneChange = true;
      this.m_Rotation.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_Position.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_Size.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.m_Ortho.valueChanged.AddListener(new UnityAction(((EditorWindow) this).Repaint));
      this.wantsMouseMove = true;
      this.dontClearBackground = true;
      SceneView.s_SceneViews.Add((object) this);
      this.m_Lighting = EditorGUIUtility.IconContent("SceneviewLighting", "Lighting|The scene lighting is used when toggled on. When toggled off a light attached to the scene view camera is used.");
      this.m_Fx = EditorGUIUtility.IconContent("SceneviewFx", "Fx|Toggles skybox, fog and lens flare effects.");
      this.m_AudioPlayContent = EditorGUIUtility.IconContent("SceneviewAudio", "AudioPlay|Toggles audio on or off.");
      this.m_GizmosContent = new GUIContent("Gizmos");
      this.m_2DModeContent = new GUIContent("2D");
      this.m_RenderDocContent = EditorGUIUtility.IconContent("renderdoc", "Capture|Capture the current view and open in RenderDoc");
      this.m_SceneViewOverlay = new SceneViewOverlay(this);
      EditorApplication.modifierKeysChanged += new EditorApplication.CallbackFunction(SceneView.RepaintAll);
      this.m_DraggingLockedState = SceneView.DraggingLockedState.NotDragging;
      this.CreateSceneCameraAndLights();
      if (this.m_2DMode)
        this.LookAt(this.pivot, Quaternion.identity, this.size, true, true);
      base.OnEnable();
    }

    internal void Awake()
    {
      if (this.m_SceneViewState == null)
        this.m_SceneViewState = new SceneView.SceneViewState();
      if (!this.m_2DMode && EditorSettings.defaultBehaviorMode != EditorBehaviorMode.Mode2D)
        return;
      this.m_LastSceneViewRotation = Quaternion.LookRotation(new Vector3(-1f, -0.7f, -1f));
      this.m_LastSceneViewOrtho = false;
      this.m_Rotation.value = Quaternion.identity;
      this.m_Ortho.value = true;
      this.m_2DMode = true;
      if (Tools.current != Tool.Move)
        return;
      Tools.current = Tool.Rect;
    }

    internal static void PlaceGameObjectInFrontOfSceneView(GameObject go)
    {
      if (SceneView.s_SceneViews.Count < 1)
        return;
      SceneView sceneView = SceneView.s_LastActiveSceneView;
      if (!(bool) ((UnityEngine.Object) sceneView))
        sceneView = SceneView.s_SceneViews[0] as SceneView;
      if (!(bool) ((UnityEngine.Object) sceneView))
        return;
      sceneView.MoveToView(go.transform);
    }

    public override void OnDisable()
    {
      EditorApplication.modifierKeysChanged -= new EditorApplication.CallbackFunction(SceneView.RepaintAll);
      if ((bool) ((UnityEngine.Object) this.m_Camera))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Camera.gameObject, true);
      if ((bool) ((UnityEngine.Object) this.m_Light[0]))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Light[0].gameObject, true);
      if ((bool) ((UnityEngine.Object) this.m_Light[1]))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Light[1].gameObject, true);
      if ((bool) ((UnityEngine.Object) this.m_Light[2]))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Light[2].gameObject, true);
      if ((bool) ((UnityEngine.Object) SceneView.s_MipColorsTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) SceneView.s_MipColorsTexture, true);
      SceneView.s_SceneViews.Remove((object) this);
      if ((UnityEngine.Object) SceneView.s_LastActiveSceneView == (UnityEngine.Object) this)
        SceneView.s_LastActiveSceneView = SceneView.s_SceneViews.Count <= 0 ? (SceneView) null : SceneView.s_SceneViews[0] as SceneView;
      this.CleanupEditorDragFunctions();
      base.OnDisable();
    }

    public void OnDestroy()
    {
      if (!this.m_AudioPlay)
        return;
      this.m_AudioPlay = false;
      this.RefreshAudioPlay();
    }

    private void DoToolbarGUI()
    {
      GUILayout.BeginHorizontal((GUIStyle) "toolbar", new GUILayoutOption[0]);
      GUIContent guiContent = SceneRenderModeWindow.GetGUIContent(this.m_RenderMode);
      if (EditorGUI.ButtonMouseDown(GUILayoutUtility.GetRect(guiContent, EditorStyles.toolbarDropDown, new GUILayoutOption[1]{ GUILayout.Width(120f) }), guiContent, FocusType.Passive, EditorStyles.toolbarDropDown))
      {
        PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new SceneRenderModeWindow(this));
        GUIUtility.ExitGUI();
      }
      EditorGUILayout.Space();
      this.in2DMode = GUILayout.Toggle(this.in2DMode, this.m_2DModeContent, (GUIStyle) "toolbarbutton", new GUILayoutOption[0]);
      EditorGUILayout.Space();
      this.m_SceneLighting = GUILayout.Toggle(this.m_SceneLighting, this.m_Lighting, (GUIStyle) "toolbarbutton", new GUILayoutOption[0]);
      if (this.renderMode == DrawCameraMode.ShadowCascades)
        this.m_SceneLighting = true;
      GUI.enabled = !Application.isPlaying;
      GUI.changed = false;
      this.m_AudioPlay = GUILayout.Toggle(this.m_AudioPlay, this.m_AudioPlayContent, EditorStyles.toolbarButton, new GUILayoutOption[0]);
      if (GUI.changed)
        this.RefreshAudioPlay();
      GUI.enabled = true;
      Rect rect = GUILayoutUtility.GetRect(this.m_Fx, this.effectsDropDownStyle);
      if (EditorGUI.ButtonMouseDown(new Rect(rect.xMax - (float) this.effectsDropDownStyle.border.right, rect.y, (float) this.effectsDropDownStyle.border.right, rect.height), GUIContent.none, FocusType.Passive, GUIStyle.none))
      {
        PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new SceneFXWindow(this));
        GUIUtility.ExitGUI();
      }
      bool flag = GUI.Toggle(rect, this.m_SceneViewState.IsAllOn(), this.m_Fx, this.effectsDropDownStyle);
      if (flag != this.m_SceneViewState.IsAllOn())
        this.m_SceneViewState.Toggle(flag);
      EditorGUILayout.Space();
      GUILayout.FlexibleSpace();
      if (this.m_MainViewControlID != GUIUtility.keyboardControl && Event.current.type == EventType.KeyDown && !string.IsNullOrEmpty(this.m_SearchFilter))
      {
        switch (Event.current.keyCode)
        {
          case KeyCode.UpArrow:
          case KeyCode.DownArrow:
            if (Event.current.keyCode == KeyCode.UpArrow)
              this.SelectPreviousSearchResult();
            else
              this.SelectNextSearchResult();
            this.FrameSelected(false);
            Event.current.Use();
            GUIUtility.ExitGUI();
            return;
        }
      }
      if (RenderDoc.IsLoaded())
      {
        EditorGUI.BeginDisabledGroup(!RenderDoc.IsSupported());
        if (GUILayout.Button(this.m_RenderDocContent, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        {
          this.m_Parent.CaptureRenderDoc();
          GUIUtility.ExitGUI();
        }
        EditorGUI.EndDisabledGroup();
      }
      if (EditorGUI.ButtonMouseDown(GUILayoutUtility.GetRect(this.m_GizmosContent, EditorStyles.toolbarDropDown), this.m_GizmosContent, FocusType.Passive, EditorStyles.toolbarDropDown) && AnnotationWindow.ShowAtPosition(GUILayoutUtility.topLevel.GetLast(), false))
        GUIUtility.ExitGUI();
      GUILayout.Space(6f);
      this.SearchFieldGUI(EditorGUILayout.kLabelFloatMaxW);
      GUILayout.EndHorizontal();
    }

    private void RefreshAudioPlay()
    {
      if ((UnityEngine.Object) SceneView.s_AudioSceneView != (UnityEngine.Object) null && (UnityEngine.Object) SceneView.s_AudioSceneView != (UnityEngine.Object) this && SceneView.s_AudioSceneView.m_AudioPlay)
      {
        SceneView.s_AudioSceneView.m_AudioPlay = false;
        SceneView.s_AudioSceneView.Repaint();
      }
      foreach (AudioSource audioSource in (AudioSource[]) UnityEngine.Object.FindObjectsOfType(typeof (AudioSource)))
      {
        if (audioSource.playOnAwake)
        {
          if (!this.m_AudioPlay)
            audioSource.Stop();
          else if (!audioSource.isPlaying)
            audioSource.Play();
        }
      }
      AudioUtil.SetListenerTransform(!this.m_AudioPlay ? (Transform) null : this.m_Camera.transform);
      SceneView.s_AudioSceneView = this;
    }

    public void OnSelectionChange()
    {
      if (Selection.activeObject != (UnityEngine.Object) null && this.m_LastLockedObject != Selection.activeObject)
        this.viewIsLockedToObject = false;
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

    [MenuItem("GameObject/Set as first sibling %=")]
    internal static void MenuMoveToFront()
    {
      foreach (Transform transform in Selection.transforms)
      {
        Undo.SetTransformParent(transform, transform.parent, "Set as first sibling");
        transform.SetAsFirstSibling();
      }
    }

    [MenuItem("GameObject/Set as first sibling %=", true)]
    internal static bool ValidateMenuMoveToFront()
    {
      if (!((UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null))
        return false;
      Transform parent = Selection.activeTransform.parent;
      if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
        return (UnityEngine.Object) parent.GetChild(0) != (UnityEngine.Object) Selection.activeTransform;
      return false;
    }

    [MenuItem("GameObject/Set as last sibling %-")]
    internal static void MenuMoveToBack()
    {
      foreach (Transform transform in Selection.transforms)
      {
        Undo.SetTransformParent(transform, transform.parent, "Set as last sibling");
        transform.SetAsLastSibling();
      }
    }

    [MenuItem("GameObject/Set as last sibling %-", true)]
    internal static bool ValidateMenuMoveToBack()
    {
      if (!((UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null))
        return false;
      Transform parent = Selection.activeTransform.parent;
      if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
        return (UnityEngine.Object) parent.GetChild(parent.childCount - 1) != (UnityEngine.Object) Selection.activeTransform;
      return false;
    }

    [MenuItem("GameObject/Move To View %&f")]
    internal static void MenuMoveToView()
    {
      if (!SceneView.ValidateMoveToView())
        return;
      SceneView.s_LastActiveSceneView.MoveToView();
    }

    [MenuItem("GameObject/Move To View %&f", true)]
    private static bool ValidateMoveToView()
    {
      if ((UnityEngine.Object) SceneView.s_LastActiveSceneView != (UnityEngine.Object) null)
        return Selection.transforms.Length != 0;
      return false;
    }

    [MenuItem("GameObject/Align With View %#f")]
    internal static void MenuAlignWithView()
    {
      if (!SceneView.ValidateAlignWithView())
        return;
      SceneView.s_LastActiveSceneView.AlignWithView();
    }

    [MenuItem("GameObject/Align With View %#f", true)]
    internal static bool ValidateAlignWithView()
    {
      if ((UnityEngine.Object) SceneView.s_LastActiveSceneView != (UnityEngine.Object) null)
        return (UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null;
      return false;
    }

    [MenuItem("GameObject/Align View to Selected")]
    internal static void MenuAlignViewToSelected()
    {
      if (!SceneView.ValidateAlignViewToSelected())
        return;
      SceneView.s_LastActiveSceneView.AlignViewToObject(Selection.activeTransform);
    }

    [MenuItem("GameObject/Align View to Selected", true)]
    internal static bool ValidateAlignViewToSelected()
    {
      if ((UnityEngine.Object) SceneView.s_LastActiveSceneView != (UnityEngine.Object) null)
        return (UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null;
      return false;
    }

    [MenuItem("GameObject/Toggle Active State &#a")]
    internal static void ActivateSelection()
    {
      if (!((UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null))
        return;
      GameObject[] gameObjects = Selection.gameObjects;
      Undo.RecordObjects((UnityEngine.Object[]) gameObjects, "Toggle Active State");
      bool flag = !Selection.activeGameObject.activeSelf;
      foreach (GameObject gameObject in gameObjects)
        gameObject.SetActive(flag);
    }

    [MenuItem("GameObject/Toggle Active State &#a", true)]
    internal static bool ValidateActivateSelection()
    {
      return (UnityEngine.Object) Selection.activeTransform != (UnityEngine.Object) null;
    }

    private static void CreateMipColorsTexture()
    {
      if ((bool) ((UnityEngine.Object) SceneView.s_MipColorsTexture))
        return;
      SceneView.s_MipColorsTexture = new Texture2D(32, 32, TextureFormat.ARGB32, true);
      SceneView.s_MipColorsTexture.hideFlags = HideFlags.HideAndDontSave;
      Color[] colorArray = new Color[6]{ new Color(0.0f, 0.0f, 1f, 0.8f), new Color(0.0f, 0.5f, 1f, 0.4f), new Color(1f, 1f, 1f, 0.0f), new Color(1f, 0.7f, 0.0f, 0.2f), new Color(1f, 0.3f, 0.0f, 0.6f), new Color(1f, 0.0f, 0.0f, 0.8f) };
      int num = Mathf.Min(6, SceneView.s_MipColorsTexture.mipmapCount);
      for (int miplevel = 0; miplevel < num; ++miplevel)
      {
        Color[] colors = new Color[Mathf.Max(SceneView.s_MipColorsTexture.width >> miplevel, 1) * Mathf.Max(SceneView.s_MipColorsTexture.height >> miplevel, 1)];
        for (int index = 0; index < colors.Length; ++index)
          colors[index] = colorArray[miplevel];
        SceneView.s_MipColorsTexture.SetPixels(colors, miplevel);
      }
      SceneView.s_MipColorsTexture.filterMode = UnityEngine.FilterMode.Trilinear;
      SceneView.s_MipColorsTexture.Apply(false);
      Shader.SetGlobalTexture("_SceneViewMipcolorsTexture", (Texture) SceneView.s_MipColorsTexture);
    }

    public void SetSceneViewFiltering(bool enable)
    {
      this.m_RequestedSceneViewFiltering = enable;
    }

    private bool UseSceneFiltering()
    {
      if (string.IsNullOrEmpty(this.m_SearchFilter))
        return this.m_RequestedSceneViewFiltering;
      return true;
    }

    internal bool SceneViewIsRenderingHDR()
    {
      if (this.UseSceneFiltering() || !((UnityEngine.Object) this.m_Camera != (UnityEngine.Object) null))
        return false;
      return this.m_Camera.hdr;
    }

    private void HandleClickAndDragToFocus()
    {
      Event current = Event.current;
      if (current.type == EventType.MouseDown || current.type == EventType.MouseDrag)
        SceneView.s_LastActiveSceneView = this;
      else if ((UnityEngine.Object) SceneView.s_LastActiveSceneView == (UnityEngine.Object) null)
        SceneView.s_LastActiveSceneView = this;
      if (current.type == EventType.MouseDrag)
        this.draggingLocked = SceneView.DraggingLockedState.Dragging;
      else if (GUIUtility.hotControl == 0 && this.draggingLocked == SceneView.DraggingLockedState.Dragging)
        this.draggingLocked = SceneView.DraggingLockedState.LookAt;
      if (current.type != EventType.MouseDown)
        return;
      Tools.s_ButtonDown = current.button;
      if (current.button != 1 || Application.platform != RuntimePlatform.OSXEditor)
        return;
      this.Focus();
    }

    private void SetupFogAndShadowDistance(out bool oldFog, out float oldShadowDistance)
    {
      oldFog = RenderSettings.fog;
      oldShadowDistance = QualitySettings.shadowDistance;
      if (Event.current.type != EventType.Repaint)
        return;
      if (!this.m_SceneViewState.showFog)
        Unsupported.SetRenderSettingsUseFogNoDirty(false);
      if (!this.m_Camera.orthographic)
        return;
      Unsupported.SetQualitySettingsShadowDistanceTemporarily(QualitySettings.shadowDistance + 0.5f * this.cameraDistance);
    }

    private void RestoreFogAndShadowDistance(bool oldFog, float oldShadowDistance)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Unsupported.SetRenderSettingsUseFogNoDirty(oldFog);
      Unsupported.SetQualitySettingsShadowDistanceTemporarily(oldShadowDistance);
    }

    private void CreateCameraTargetTexture(Rect cameraRect, bool hdr)
    {
      bool flag1 = QualitySettings.activeColorSpace == ColorSpace.Linear;
      int num = Mathf.Max(1, QualitySettings.antiAliasing);
      if (this.IsSceneCameraDeferred())
        num = 1;
      RenderTextureFormat format = !hdr ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf;
      if ((UnityEngine.Object) this.m_SceneTargetTexture != (UnityEngine.Object) null)
      {
        bool flag2 = !hdr ? (UnityEngine.Object) this.m_SceneTargetTextureLDR == (UnityEngine.Object) null && flag1 == this.m_SceneTargetTexture.sRGB : (UnityEngine.Object) this.m_SceneTargetTextureLDR != (UnityEngine.Object) null && flag1 == this.m_SceneTargetTextureLDR.sRGB;
        if (this.m_SceneTargetTexture.format != format || this.m_SceneTargetTexture.antiAliasing != num || !flag2)
        {
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_SceneTargetTexture);
          this.m_SceneTargetTexture = (RenderTexture) null;
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_SceneTargetTextureLDR);
          this.m_SceneTargetTextureLDR = (RenderTexture) null;
        }
      }
      Rect cameraRect1 = Handles.GetCameraRect(cameraRect);
      int width = (int) cameraRect1.width;
      int height = (int) cameraRect1.height;
      if ((UnityEngine.Object) this.m_SceneTargetTexture == (UnityEngine.Object) null)
      {
        this.m_SceneTargetTexture = new RenderTexture(0, 0, 24, format);
        this.m_SceneTargetTexture.name = "SceneView RT";
        this.m_SceneTargetTexture.antiAliasing = num;
        this.m_SceneTargetTexture.hideFlags = HideFlags.HideAndDontSave;
      }
      if (this.m_SceneTargetTexture.width != width || this.m_SceneTargetTexture.height != height)
      {
        this.m_SceneTargetTexture.Release();
        this.m_SceneTargetTexture.width = width;
        this.m_SceneTargetTexture.height = height;
      }
      this.m_SceneTargetTexture.Create();
      if (!hdr)
        return;
      if ((UnityEngine.Object) this.m_SceneTargetTextureLDR == (UnityEngine.Object) null)
      {
        this.m_SceneTargetTextureLDR = new RenderTexture(0, 0, 0, RenderTextureFormat.ARGB32);
        this.m_SceneTargetTextureLDR.name = "SceneView LDR RT";
        this.m_SceneTargetTextureLDR.antiAliasing = num;
        this.m_SceneTargetTextureLDR.hideFlags = HideFlags.HideAndDontSave;
      }
      if (this.m_SceneTargetTextureLDR.width != width || this.m_SceneTargetTextureLDR.height != height)
      {
        this.m_SceneTargetTextureLDR.Release();
        this.m_SceneTargetTextureLDR.width = width;
        this.m_SceneTargetTextureLDR.height = height;
      }
      this.m_SceneTargetTextureLDR.Create();
    }

    internal bool IsSceneCameraDeferred()
    {
      return !((UnityEngine.Object) this.m_Camera == (UnityEngine.Object) null) && (this.m_Camera.actualRenderingPath == RenderingPath.DeferredLighting || this.m_Camera.actualRenderingPath == RenderingPath.DeferredShading);
    }

    internal static bool DoesCameraDrawModeSupportDeferred(DrawCameraMode mode)
    {
      if (mode != DrawCameraMode.Normal && mode != DrawCameraMode.Textured && (mode != DrawCameraMode.ShadowCascades && mode != DrawCameraMode.RenderPaths) && (mode != DrawCameraMode.AlphaChannel && mode != DrawCameraMode.DeferredDiffuse && (mode != DrawCameraMode.DeferredSpecular && mode != DrawCameraMode.DeferredSmoothness)) && (mode != DrawCameraMode.DeferredNormal && mode != DrawCameraMode.Charting && (mode != DrawCameraMode.Systems && mode != DrawCameraMode.Albedo) && (mode != DrawCameraMode.Emissive && mode != DrawCameraMode.Irradiance && (mode != DrawCameraMode.Directionality && mode != DrawCameraMode.Baked))) && mode != DrawCameraMode.Clustering)
        return mode == DrawCameraMode.LitClustering;
      return true;
    }

    internal static bool DoesCameraDrawModeSupportHDR(DrawCameraMode mode)
    {
      if (mode != DrawCameraMode.Wireframe && mode != DrawCameraMode.TexturedWire && mode != DrawCameraMode.Overdraw)
        return mode != DrawCameraMode.Mipmaps;
      return false;
    }

    private void PrepareCameraTargetTexture(Rect cameraRect)
    {
      bool hdr = this.SceneViewIsRenderingHDR();
      this.CreateCameraTargetTexture(cameraRect, hdr);
      this.m_Camera.targetTexture = this.m_SceneTargetTexture;
      if (!this.UseSceneFiltering() && SceneView.DoesCameraDrawModeSupportDeferred(this.m_RenderMode) || !this.IsSceneCameraDeferred())
        return;
      this.m_Camera.renderingPath = RenderingPath.Forward;
    }

    private void PrepareCameraReplacementShader()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Handles.SetSceneViewColors((Color) SceneView.kSceneViewWire, (Color) SceneView.kSceneViewWireOverlay, (Color) SceneView.kSceneViewWireActive, (Color) SceneView.kSceneViewWireSelected);
      if (this.m_RenderMode == DrawCameraMode.Overdraw)
      {
        if (!(bool) ((UnityEngine.Object) SceneView.s_ShowOverdrawShader))
          SceneView.s_ShowOverdrawShader = EditorGUIUtility.LoadRequired("SceneView/SceneViewShowOverdraw.shader") as Shader;
        this.m_Camera.SetReplacementShader(SceneView.s_ShowOverdrawShader, "RenderType");
      }
      else if (this.m_RenderMode == DrawCameraMode.Mipmaps)
      {
        if (!(bool) ((UnityEngine.Object) SceneView.s_ShowMipsShader))
          SceneView.s_ShowMipsShader = EditorGUIUtility.LoadRequired("SceneView/SceneViewShowMips.shader") as Shader;
        if ((UnityEngine.Object) SceneView.s_ShowMipsShader != (UnityEngine.Object) null && SceneView.s_ShowMipsShader.isSupported)
        {
          SceneView.CreateMipColorsTexture();
          this.m_Camera.SetReplacementShader(SceneView.s_ShowMipsShader, "RenderType");
        }
        else
          this.m_Camera.SetReplacementShader(this.m_ReplacementShader, this.m_ReplacementString);
      }
      else
        this.m_Camera.SetReplacementShader(this.m_ReplacementShader, this.m_ReplacementString);
    }

    private bool SceneCameraRendersIntoRT()
    {
      if (!((UnityEngine.Object) this.m_Camera.targetTexture != (UnityEngine.Object) null))
        return HandleUtility.CameraNeedsToRenderIntoRT(this.m_Camera);
      return true;
    }

    private void DoDrawCamera(Rect cameraRect, out bool pushedGUIClip)
    {
      pushedGUIClip = false;
      if (!this.m_Camera.gameObject.activeInHierarchy)
        return;
      DrawGridParameters gridParam = this.grid.PrepareGridRender(this.camera, this.pivot, this.m_Rotation.target, this.m_Size.value, this.m_Ortho.target, AnnotationUtility.showGrid);
      Event current = Event.current;
      if (this.UseSceneFiltering())
      {
        if (current.type == EventType.Repaint)
        {
          Handles.EnableCameraFx(this.m_Camera, true);
          Handles.SetCameraFilterMode(this.m_Camera, Handles.FilterMode.ShowRest);
          float fade = Mathf.Clamp01((float) (EditorApplication.timeSinceStartup - this.m_StartSearchFilterTime));
          Handles.DrawCamera(cameraRect, this.m_Camera, this.m_RenderMode);
          Handles.DrawCameraFade(this.m_Camera, fade);
          Handles.EnableCameraFx(this.m_Camera, false);
          Handles.SetCameraFilterMode(this.m_Camera, Handles.FilterMode.ShowFiltered);
          if (!(bool) ((UnityEngine.Object) SceneView.s_AuraShader))
            SceneView.s_AuraShader = EditorGUIUtility.LoadRequired("SceneView/SceneViewAura.shader") as Shader;
          this.m_Camera.SetReplacementShader(SceneView.s_AuraShader, string.Empty);
          Handles.DrawCamera(cameraRect, this.m_Camera, this.m_RenderMode);
          this.m_Camera.SetReplacementShader(this.m_ReplacementShader, this.m_ReplacementString);
          Handles.DrawCamera(cameraRect, this.m_Camera, this.m_RenderMode, gridParam);
          if ((double) fade < 1.0)
            this.Repaint();
        }
        Rect position = cameraRect;
        if (current.type == EventType.Repaint)
          RenderTexture.active = (RenderTexture) null;
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(0.0f, 17f, this.position.width, this.position.height - 17f));
        if (current.type == EventType.Repaint)
        {
          GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
          GUI.DrawTexture(position, (Texture) this.m_SceneTargetTexture, ScaleMode.StretchToFill, false, 0.0f);
          GL.sRGBWrite = false;
        }
        Handles.SetCamera(cameraRect, this.m_Camera);
        this.HandleSelectionAndOnSceneGUI();
      }
      else
      {
        if (this.SceneCameraRendersIntoRT())
        {
          GUIClip.Push(new Rect(0.0f, 0.0f, this.position.width, this.position.height), Vector2.zero, Vector2.zero, true);
          pushedGUIClip = true;
        }
        Handles.DrawCameraStep1(cameraRect, this.m_Camera, this.m_RenderMode, gridParam);
        this.DrawRenderModeOverlay(cameraRect);
      }
    }

    private void DoClearCamera(Rect cameraRect)
    {
      float verticalFov = this.GetVerticalFOV(90f);
      float fieldOfView = this.m_Camera.fieldOfView;
      this.m_Camera.fieldOfView = verticalFov;
      Handles.ClearCamera(cameraRect, this.m_Camera);
      this.m_Camera.fieldOfView = fieldOfView;
    }

    private void SetupCustomSceneLighting()
    {
      if (this.m_SceneLighting)
        return;
      this.m_Light[0].transform.rotation = this.m_Camera.transform.rotation;
      if (Event.current.type != EventType.Repaint)
        return;
      InternalEditorUtility.SetCustomLighting(this.m_Light, SceneView.kSceneViewMidLight);
    }

    private void CleanupCustomSceneLighting()
    {
      if (this.m_SceneLighting || Event.current.type != EventType.Repaint)
        return;
      InternalEditorUtility.RemoveCustomLighting();
    }

    private void HandleViewToolCursor()
    {
      if (!Tools.viewToolActive || Event.current.type != EventType.Repaint)
        return;
      MouseCursor cursor = MouseCursor.Arrow;
      switch (Tools.viewTool)
      {
        case ViewTool.Orbit:
          cursor = MouseCursor.Orbit;
          break;
        case ViewTool.Pan:
          cursor = MouseCursor.Pan;
          break;
        case ViewTool.Zoom:
          cursor = MouseCursor.Zoom;
          break;
        case ViewTool.FPS:
          cursor = MouseCursor.FPS;
          break;
      }
      if (cursor == MouseCursor.Arrow)
        return;
      SceneView.AddCursorRect(new Rect(0.0f, 17f, this.position.width, this.position.height - 17f), cursor);
    }

    private void DoTonemapping()
    {
      if (Event.current.type != EventType.Repaint || !this.SceneViewIsRenderingHDR())
        return;
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      Camera mainCamera = SceneView.GetMainCamera();
      if ((UnityEngine.Object) mainCamera == (UnityEngine.Object) null || !Handles.DrawCameraTonemap(mainCamera, this.m_SceneTargetTexture, this.m_SceneTargetTextureLDR))
        Graphics.Blit((Texture) this.m_SceneTargetTexture, this.m_SceneTargetTextureLDR);
      GL.sRGBWrite = false;
      Graphics.SetRenderTarget(this.m_SceneTargetTextureLDR.colorBuffer, this.m_SceneTargetTexture.depthBuffer);
    }

    private void DoOnPreSceneGUICallbacks(Rect cameraRect)
    {
      if (this.UseSceneFiltering())
        return;
      Handles.SetCamera(cameraRect, this.m_Camera);
      this.CallOnPreSceneGUI();
    }

    private void RepaintGizmosThatAreRenderedOnTopOfSceneView()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.svRot.OnGUI(this);
    }

    private void InputForGizmosThatAreRenderedOnTopOfSceneView()
    {
      if (Event.current.type == EventType.Repaint)
        return;
      this.svRot.OnGUI(this);
    }

    internal void OnGUI()
    {
      SceneView.s_CurrentDrawingSceneView = this;
      Event current = Event.current;
      if (current.type == EventType.Repaint)
      {
        SceneView.s_MouseRects.Clear();
        Profiler.BeginSample("SceneView.Repaint");
      }
      Color color = GUI.color;
      this.HandleClickAndDragToFocus();
      if (current.type == EventType.Layout)
        this.m_ShowSceneViewWindows = (UnityEngine.Object) SceneView.lastActiveSceneView == (UnityEngine.Object) this;
      this.m_SceneViewOverlay.Begin();
      bool oldFog;
      float oldShadowDistance;
      this.SetupFogAndShadowDistance(out oldFog, out oldShadowDistance);
      this.DoToolbarGUI();
      GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
      GUI.color = Color.white;
      EditorGUIUtility.labelWidth = 100f;
      this.SetupCamera();
      RenderingPath renderingPath = this.m_Camera.renderingPath;
      this.SetupCustomSceneLighting();
      GUI.BeginGroup(new Rect(0.0f, 17f, this.position.width, this.position.height - 17f));
      Rect rect = new Rect(0.0f, 0.0f, this.position.width, this.position.height - 17f);
      Rect pixels = EditorGUIUtility.PointsToPixels(rect);
      this.HandleViewToolCursor();
      this.PrepareCameraTargetTexture(pixels);
      this.DoClearCamera(pixels);
      this.m_Camera.cullingMask = Tools.visibleLayers;
      this.InputForGizmosThatAreRenderedOnTopOfSceneView();
      this.DoOnPreSceneGUICallbacks(pixels);
      this.PrepareCameraReplacementShader();
      this.m_MainViewControlID = GUIUtility.GetControlID(FocusType.Keyboard);
      if (current.GetTypeForControl(this.m_MainViewControlID) == EventType.MouseDown)
        GUIUtility.keyboardControl = this.m_MainViewControlID;
      bool pushedGUIClip;
      this.DoDrawCamera(rect, out pushedGUIClip);
      this.CleanupCustomSceneLighting();
      if (!this.UseSceneFiltering())
      {
        Handles.DrawCameraStep2(this.m_Camera, this.m_RenderMode);
        this.DoTonemapping();
        this.HandleSelectionAndOnSceneGUI();
      }
      if (current.type == EventType.ExecuteCommand || current.type == EventType.ValidateCommand)
        this.CommandsGUI();
      this.RestoreFogAndShadowDistance(oldFog, oldShadowDistance);
      this.m_Camera.renderingPath = renderingPath;
      if (this.UseSceneFiltering())
        Handles.SetCameraFilterMode(Camera.current, Handles.FilterMode.ShowFiltered);
      else
        Handles.SetCameraFilterMode(Camera.current, Handles.FilterMode.Off);
      this.DefaultHandles();
      if (!this.UseSceneFiltering())
      {
        if (current.type == EventType.Repaint)
        {
          Profiler.BeginSample("SceneView.BlitRT");
          Graphics.SetRenderTarget((RenderTexture) null);
        }
        if (pushedGUIClip)
          GUIClip.Pop();
        if (current.type == EventType.Repaint)
        {
          GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
          GUI.DrawTexture(rect, !this.m_Camera.hdr ? (Texture) this.m_SceneTargetTexture : (Texture) this.m_SceneTargetTextureLDR, ScaleMode.StretchToFill, false);
          GL.sRGBWrite = false;
          Profiler.EndSample();
        }
      }
      Handles.SetCameraFilterMode(Camera.current, Handles.FilterMode.Off);
      Handles.SetCameraFilterMode(this.m_Camera, Handles.FilterMode.Off);
      this.HandleDragging();
      this.RepaintGizmosThatAreRenderedOnTopOfSceneView();
      if ((UnityEngine.Object) SceneView.s_LastActiveSceneView == (UnityEngine.Object) this)
      {
        SceneViewMotion.ArrowKeys(this);
        SceneViewMotion.DoViewTool(this);
      }
      this.Handle2DModeSwitch();
      GUI.EndGroup();
      GUI.color = color;
      this.m_SceneViewOverlay.End();
      this.HandleMouseCursor();
      if (current.type == EventType.Repaint)
        Profiler.EndSample();
      SceneView.s_CurrentDrawingSceneView = (SceneView) null;
    }

    private void Handle2DModeSwitch()
    {
      Event current = Event.current;
      if (SceneView.k2DMode.activated && !SceneView.waitingFor2DModeKeyUp)
      {
        SceneView.waitingFor2DModeKeyUp = true;
        this.in2DMode = !this.in2DMode;
        current.Use();
      }
      else
      {
        if (current.type != EventType.KeyUp || current.keyCode != SceneView.k2DMode.KeyboardEvent.keyCode)
          return;
        SceneView.waitingFor2DModeKeyUp = false;
      }
    }

    private void HandleMouseCursor()
    {
      Event current1 = Event.current;
      if (GUIUtility.hotControl == 0)
        this.s_DraggingCursorIsCached = false;
      Rect position = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      if (!this.s_DraggingCursorIsCached)
      {
        MouseCursor mouseCursor = MouseCursor.Arrow;
        if (current1.type == EventType.MouseMove || current1.type == EventType.Repaint)
        {
          using (List<SceneView.CursorRect>.Enumerator enumerator = SceneView.s_MouseRects.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              SceneView.CursorRect current2 = enumerator.Current;
              if (current2.rect.Contains(current1.mousePosition))
              {
                mouseCursor = current2.cursor;
                position = current2.rect;
              }
            }
          }
          if (GUIUtility.hotControl != 0)
            this.s_DraggingCursorIsCached = true;
          if (mouseCursor != SceneView.s_LastCursor)
          {
            SceneView.s_LastCursor = mouseCursor;
            InternalEditorUtility.ResetCursor();
            this.Repaint();
          }
        }
      }
      if (current1.type != EventType.Repaint || SceneView.s_LastCursor == MouseCursor.Arrow)
        return;
      EditorGUIUtility.AddCursorRect(position, SceneView.s_LastCursor);
    }

    private void DrawRenderModeOverlay(Rect cameraRect)
    {
      if (this.m_RenderMode == DrawCameraMode.AlphaChannel)
      {
        if (!(bool) ((UnityEngine.Object) SceneView.s_AlphaOverlayMaterial))
          SceneView.s_AlphaOverlayMaterial = EditorGUIUtility.LoadRequired("SceneView/SceneViewAlphaMaterial.mat") as Material;
        Handles.BeginGUI();
        if (Event.current.type == EventType.Repaint)
          Graphics.DrawTexture(cameraRect, (Texture) EditorGUIUtility.whiteTexture, SceneView.s_AlphaOverlayMaterial);
        Handles.EndGUI();
      }
      if (this.m_RenderMode != DrawCameraMode.DeferredDiffuse && this.m_RenderMode != DrawCameraMode.DeferredSpecular && (this.m_RenderMode != DrawCameraMode.DeferredSmoothness && this.m_RenderMode != DrawCameraMode.DeferredNormal))
        return;
      if (!(bool) ((UnityEngine.Object) SceneView.s_DeferredOverlayMaterial))
        SceneView.s_DeferredOverlayMaterial = EditorGUIUtility.LoadRequired("SceneView/SceneViewDeferredMaterial.mat") as Material;
      Handles.BeginGUI();
      if (Event.current.type == EventType.Repaint)
      {
        SceneView.s_DeferredOverlayMaterial.SetInt("_DisplayMode", (int) (this.m_RenderMode - 8));
        Graphics.DrawTexture(cameraRect, (Texture) EditorGUIUtility.whiteTexture, SceneView.s_DeferredOverlayMaterial);
      }
      Handles.EndGUI();
    }

    private void HandleSelectionAndOnSceneGUI()
    {
      this.m_RectSelection.OnGUI();
      this.CallOnSceneGUI();
    }

    public void FixNegativeSize()
    {
      float num = 90f;
      if ((double) this.size >= 0.0)
        return;
      Vector3 vector3 = this.m_Position.value + this.rotation * new Vector3(0.0f, 0.0f, -(this.size / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)))));
      this.size = -this.size;
      float z = this.size / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)));
      this.m_Position.value = vector3 + this.rotation * new Vector3(0.0f, 0.0f, z);
    }

    private float CalcCameraDist()
    {
      float num = this.m_Ortho.Fade(90f, 0.0f);
      if ((double) num <= 3.0)
        return 0.0f;
      this.m_Camera.orthographic = false;
      return this.size / Mathf.Tan((float) ((double) num * 0.5 * (Math.PI / 180.0)));
    }

    private void ResetIfNaN()
    {
      if (float.IsInfinity(this.m_Position.value.x) || float.IsNaN(this.m_Position.value.x))
        this.m_Position.value = Vector3.zero;
      if (!float.IsInfinity(this.m_Rotation.value.x) && !float.IsNaN(this.m_Rotation.value.x))
        return;
      this.m_Rotation.value = Quaternion.identity;
    }

    internal static Camera GetMainCamera()
    {
      Camera main = Camera.main;
      if ((UnityEngine.Object) main != (UnityEngine.Object) null)
        return main;
      Camera[] allCameras = Camera.allCameras;
      if (allCameras != null && allCameras.Length == 1)
        return allCameras[0];
      return (Camera) null;
    }

    internal static RenderingPath GetSceneViewRenderingPath()
    {
      Camera mainCamera = SceneView.GetMainCamera();
      if ((UnityEngine.Object) mainCamera != (UnityEngine.Object) null)
        return mainCamera.renderingPath;
      return RenderingPath.UsePlayerSettings;
    }

    internal static bool IsUsingDeferredRenderingPath()
    {
      switch (SceneView.GetSceneViewRenderingPath())
      {
        case RenderingPath.DeferredShading:
          return true;
        case RenderingPath.UsePlayerSettings:
          return PlayerSettings.renderingPath == RenderingPath.DeferredShading;
        default:
          return false;
      }
    }

    internal bool CheckDrawModeForRenderingPath(DrawCameraMode mode)
    {
      RenderingPath actualRenderingPath = this.m_Camera.actualRenderingPath;
      if (mode == DrawCameraMode.DeferredDiffuse || mode == DrawCameraMode.DeferredSpecular || (mode == DrawCameraMode.DeferredSmoothness || mode == DrawCameraMode.DeferredNormal))
        return actualRenderingPath == RenderingPath.DeferredShading;
      return true;
    }

    private void SetSceneCameraHDRAndDepthModes()
    {
      if (!this.m_SceneLighting || !SceneView.DoesCameraDrawModeSupportHDR(this.m_RenderMode))
      {
        this.m_Camera.hdr = false;
        this.m_Camera.depthTextureMode = DepthTextureMode.None;
        this.m_Camera.clearStencilAfterLightingPass = false;
      }
      else
      {
        Camera mainCamera = SceneView.GetMainCamera();
        if ((UnityEngine.Object) mainCamera == (UnityEngine.Object) null)
        {
          this.m_Camera.hdr = false;
          this.m_Camera.depthTextureMode = DepthTextureMode.None;
          this.m_Camera.clearStencilAfterLightingPass = false;
        }
        else
        {
          this.m_Camera.hdr = mainCamera.hdr;
          this.m_Camera.depthTextureMode = mainCamera.depthTextureMode;
          this.m_Camera.clearStencilAfterLightingPass = mainCamera.clearStencilAfterLightingPass;
        }
      }
    }

    private void SetupCamera()
    {
      this.m_Camera.backgroundColor = this.m_RenderMode != DrawCameraMode.Overdraw ? (Color) SceneView.kSceneViewBackground : Color.black;
      EditorUtility.SetCameraAnimateMaterials(this.m_Camera, this.m_SceneViewState.showMaterialUpdate);
      this.ResetIfNaN();
      this.m_Camera.transform.rotation = this.m_Rotation.value;
      float aspectNeutralFOV = this.m_Ortho.Fade(90f, 0.0f);
      if ((double) aspectNeutralFOV > 3.0)
      {
        this.m_Camera.orthographic = false;
        this.m_Camera.fieldOfView = this.GetVerticalFOV(aspectNeutralFOV);
      }
      else
      {
        this.m_Camera.orthographic = true;
        this.m_Camera.orthographicSize = this.GetVerticalOrthoSize();
      }
      this.m_Camera.transform.position = this.m_Position.value + this.m_Camera.transform.rotation * new Vector3(0.0f, 0.0f, -this.cameraDistance);
      float num = Mathf.Max(1000f, 2000f * this.size);
      this.m_Camera.nearClipPlane = num * 5E-06f;
      this.m_Camera.farClipPlane = num;
      this.m_Camera.renderingPath = SceneView.GetSceneViewRenderingPath();
      if (!this.CheckDrawModeForRenderingPath(this.m_RenderMode))
        this.m_RenderMode = DrawCameraMode.Textured;
      this.SetSceneCameraHDRAndDepthModes();
      Handles.EnableCameraFlares(this.m_Camera, this.m_SceneViewState.showFlares);
      Handles.EnableCameraSkybox(this.m_Camera, this.m_SceneViewState.showSkybox);
      this.m_Light[0].transform.position = this.m_Camera.transform.position;
      this.m_Light[0].transform.rotation = this.m_Camera.transform.rotation;
      if (this.m_AudioPlay)
      {
        AudioUtil.SetListenerTransform(this.m_Camera.transform);
        AudioUtil.UpdateAudio();
      }
      if (!this.m_ViewIsLockedToObject || Selection.gameObjects.Length <= 0)
        return;
      switch (this.m_DraggingLockedState)
      {
        case SceneView.DraggingLockedState.NotDragging:
          this.m_Position.value = Selection.activeGameObject.transform.position;
          break;
        case SceneView.DraggingLockedState.LookAt:
          if (!this.m_Position.value.Equals((object) Selection.activeGameObject.transform.position))
          {
            if (!EditorApplication.isPlaying)
            {
              this.m_Position.target = Selection.activeGameObject.transform.position;
              break;
            }
            this.m_Position.value = Selection.activeGameObject.transform.position;
            break;
          }
          this.m_DraggingLockedState = SceneView.DraggingLockedState.NotDragging;
          break;
      }
    }

    private void OnBecameVisible()
    {
      EditorApplication.update += new EditorApplication.CallbackFunction(this.UpdateAnimatedMaterials);
    }

    private void OnBecameInvisible()
    {
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.UpdateAnimatedMaterials);
    }

    private void UpdateAnimatedMaterials()
    {
      if (!this.m_SceneViewState.showMaterialUpdate || this.m_lastRenderedTime + 0.0329999998211861 >= EditorApplication.timeSinceStartup)
        return;
      this.m_lastRenderedTime = EditorApplication.timeSinceStartup;
      this.Repaint();
    }

    internal float GetVerticalFOV(float aspectNeutralFOV)
    {
      return (float) ((double) Mathf.Atan(Mathf.Tan((float) ((double) aspectNeutralFOV * 0.5 * (Math.PI / 180.0))) * 0.7071068f / Mathf.Sqrt(this.m_Camera.aspect)) * 2.0 * 57.2957801818848);
    }

    internal float GetVerticalOrthoSize()
    {
      return this.size * 0.7071068f / Mathf.Sqrt(this.m_Camera.aspect);
    }

    public void LookAt(Vector3 pos)
    {
      this.FixNegativeSize();
      this.m_Position.target = pos;
    }

    public void LookAt(Vector3 pos, Quaternion rot)
    {
      this.FixNegativeSize();
      this.m_Position.target = pos;
      this.m_Rotation.target = rot;
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    public void LookAtDirect(Vector3 pos, Quaternion rot)
    {
      this.FixNegativeSize();
      this.m_Position.value = pos;
      this.m_Rotation.value = rot;
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    public void LookAt(Vector3 pos, Quaternion rot, float newSize)
    {
      this.FixNegativeSize();
      this.m_Position.target = pos;
      this.m_Rotation.target = rot;
      this.m_Size.target = Mathf.Abs(newSize);
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    public void LookAtDirect(Vector3 pos, Quaternion rot, float newSize)
    {
      this.FixNegativeSize();
      this.m_Position.value = pos;
      this.m_Rotation.value = rot;
      this.m_Size.value = Mathf.Abs(newSize);
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    public void LookAt(Vector3 pos, Quaternion rot, float newSize, bool ortho)
    {
      this.LookAt(pos, rot, newSize, ortho, false);
    }

    public void LookAt(Vector3 pos, Quaternion rot, float newSize, bool ortho, bool instant)
    {
      this.FixNegativeSize();
      if (instant)
      {
        this.m_Position.value = pos;
        this.m_Rotation.value = rot;
        this.m_Size.value = Mathf.Abs(newSize);
        this.m_Ortho.value = ortho;
      }
      else
      {
        this.m_Position.target = pos;
        this.m_Rotation.target = rot;
        this.m_Size.target = Mathf.Abs(newSize);
        this.m_Ortho.target = ortho;
      }
      this.svRot.UpdateGizmoLabel(this, rot * Vector3.forward, this.m_Ortho.target);
    }

    private void DefaultHandles()
    {
      EditorGUI.BeginChangeCheck();
      bool flag1 = Event.current.GetTypeForControl(GUIUtility.hotControl) == EventType.MouseDrag;
      bool flag2 = Event.current.GetTypeForControl(GUIUtility.hotControl) == EventType.MouseUp;
      if (GUIUtility.hotControl == 0)
        SceneView.s_CurrentTool = !Tools.viewToolActive ? Tools.current : Tool.View;
      switch ((Event.current.type != EventType.Repaint ? (int) SceneView.s_CurrentTool : (int) Tools.current) + 1)
      {
        case 2:
          MoveTool.OnGUI(this);
          break;
        case 3:
          RotateTool.OnGUI(this);
          break;
        case 4:
          ScaleTool.OnGUI(this);
          break;
        case 5:
          RectTool.OnGUI(this);
          break;
      }
      if (EditorGUI.EndChangeCheck() && EditorApplication.isPlaying && flag1)
        Physics2D.SetEditorDragMovement(true, Selection.gameObjects);
      if (!EditorApplication.isPlaying || !flag2)
        return;
      Physics2D.SetEditorDragMovement(false, Selection.gameObjects);
    }

    private void CleanupEditorDragFunctions()
    {
      if (this.m_DragEditorCache != null)
        this.m_DragEditorCache.Dispose();
      this.m_DragEditorCache = (EditorCache) null;
    }

    private void CallEditorDragFunctions()
    {
      Event current = Event.current;
      SpriteUtility.OnSceneDrag(this);
      if (current.type == EventType.Used || DragAndDrop.objectReferences.Length == 0)
        return;
      if (this.m_DragEditorCache == null)
        this.m_DragEditorCache = new EditorCache(EditorFeatures.OnSceneDrag);
      foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
      {
        if (!(objectReference == (UnityEngine.Object) null))
        {
          EditorWrapper editorWrapper = this.m_DragEditorCache[objectReference];
          if (editorWrapper != null)
            editorWrapper.OnSceneDrag(this);
          if (current.type == EventType.Used)
            break;
        }
      }
    }

    private void HandleDragging()
    {
      Event current = Event.current;
      EventType type = current.type;
      switch (type)
      {
        case EventType.Repaint:
          this.CallEditorDragFunctions();
          break;
        case EventType.DragUpdated:
        case EventType.DragPerform:
          this.CallEditorDragFunctions();
          bool perform = current.type == EventType.DragPerform;
          SpriteUtility.OnSceneDrag(this);
          if (current.type == EventType.Used)
            break;
          if (DragAndDrop.visualMode != DragAndDropVisualMode.Copy)
            DragAndDrop.visualMode = InternalEditorUtility.SceneViewDrag((UnityEngine.Object) HandleUtility.PickGameObject(Event.current.mousePosition, true), this.pivot, Event.current.mousePosition, perform);
          if (perform && DragAndDrop.visualMode != DragAndDropVisualMode.None)
          {
            DragAndDrop.AcceptDrag();
            current.Use();
            GUIUtility.ExitGUI();
          }
          current.Use();
          break;
        default:
          if (type != EventType.DragExited)
            break;
          this.CallEditorDragFunctions();
          this.CleanupEditorDragFunctions();
          break;
      }
    }

    private void CommandsGUI()
    {
      bool flag = Event.current.type == EventType.ExecuteCommand;
      string commandName = Event.current.commandName;
      if (commandName == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (SceneView.\u003C\u003Ef__switch\u0024map20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SceneView.\u003C\u003Ef__switch\u0024map20 = new Dictionary<string, int>(9)
        {
          {
            "Find",
            0
          },
          {
            "FrameSelected",
            1
          },
          {
            "FrameSelectedWithLock",
            2
          },
          {
            "SoftDelete",
            3
          },
          {
            "Delete",
            3
          },
          {
            "Duplicate",
            4
          },
          {
            "Copy",
            5
          },
          {
            "Paste",
            6
          },
          {
            "SelectAll",
            7
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!SceneView.\u003C\u003Ef__switch\u0024map20.TryGetValue(commandName, out num))
        return;
      switch (num)
      {
        case 0:
          if (flag)
            this.FocusSearchField();
          Event.current.Use();
          break;
        case 1:
          if (flag)
          {
            this.FrameSelected(EditorApplication.timeSinceStartup - this.lastFramingTime < 0.5);
            this.lastFramingTime = EditorApplication.timeSinceStartup;
          }
          Event.current.Use();
          break;
        case 2:
          if (flag)
            this.FrameSelected(true);
          Event.current.Use();
          break;
        case 3:
          if (flag)
            Unsupported.DeleteGameObjectSelection();
          Event.current.Use();
          break;
        case 4:
          if (flag)
            Unsupported.DuplicateGameObjectsUsingPasteboard();
          Event.current.Use();
          break;
        case 5:
          if (flag)
            Unsupported.CopyGameObjectsToPasteboard();
          Event.current.Use();
          break;
        case 6:
          if (flag)
            Unsupported.PasteGameObjectsFromPasteboard();
          Event.current.Use();
          break;
        case 7:
          if (flag)
            Selection.objects = UnityEngine.Object.FindObjectsOfType(typeof (GameObject));
          Event.current.Use();
          break;
      }
    }

    public void AlignViewToObject(Transform t)
    {
      this.FixNegativeSize();
      this.size = 10f;
      this.LookAt(t.position + t.forward * this.CalcCameraDist(), t.rotation);
    }

    public void AlignWithView()
    {
      this.FixNegativeSize();
      Vector3 position = this.camera.transform.position;
      Vector3 vector3 = position - Tools.handlePosition;
      float angle;
      Vector3 axis1;
      (Quaternion.Inverse(Selection.activeTransform.rotation) * this.camera.transform.rotation).ToAngleAxis(out angle, out axis1);
      Vector3 axis2 = Selection.activeTransform.TransformDirection(axis1);
      Undo.RecordObjects((UnityEngine.Object[]) Selection.transforms, "Align with view");
      foreach (Transform transform in Selection.transforms)
      {
        transform.position += vector3;
        transform.RotateAround(position, axis2, angle);
      }
    }

    public void MoveToView()
    {
      this.FixNegativeSize();
      Vector3 vector3 = this.pivot - Tools.handlePosition;
      Undo.RecordObjects((UnityEngine.Object[]) Selection.transforms, "Move to view");
      foreach (Transform transform in Selection.transforms)
        transform.position += vector3;
    }

    public void MoveToView(Transform target)
    {
      target.position = this.pivot;
    }

    public bool FrameSelected()
    {
      return this.FrameSelected(false);
    }

    public bool FrameSelected(bool lockView)
    {
      this.viewIsLockedToObject = lockView;
      this.FixNegativeSize();
      Bounds bounds = InternalEditorUtility.CalculateSelectionBounds(false, Tools.pivotMode == PivotMode.Pivot);
      foreach (Editor activeEditor in this.GetActiveEditors())
      {
        MethodInfo method1 = activeEditor.GetType().GetMethod("HasFrameBounds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
        if (method1 != null)
        {
          object obj1 = method1.Invoke((object) activeEditor, (object[]) null);
          if (obj1 is bool && (bool) obj1)
          {
            MethodInfo method2 = activeEditor.GetType().GetMethod("OnGetFrameBounds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            if (method2 != null)
            {
              object obj2 = method2.Invoke((object) activeEditor, (object[]) null);
              if (obj2 is Bounds)
                bounds = (Bounds) obj2;
            }
          }
        }
      }
      return this.Frame(bounds);
    }

    internal bool Frame(Bounds bounds)
    {
      float num = bounds.extents.magnitude * 1.5f;
      if ((double) num == double.PositiveInfinity)
        return false;
      if ((double) num == 0.0)
        num = 10f;
      this.LookAt(bounds.center, this.m_Rotation.target, num * 2.2f, this.m_Ortho.value, EditorApplication.isPlaying);
      return true;
    }

    private void CreateSceneCameraAndLights()
    {
      GameObject objectWithHideFlags1 = EditorUtility.CreateGameObjectWithHideFlags("SceneCamera", HideFlags.HideAndDontSave, typeof (Camera));
      objectWithHideFlags1.AddComponentInternal("FlareLayer");
      objectWithHideFlags1.AddComponentInternal("HaloLayer");
      this.m_Camera = objectWithHideFlags1.GetComponent<Camera>();
      this.m_Camera.enabled = false;
      this.m_Camera.cameraType = CameraType.SceneView;
      for (int index = 0; index < 3; ++index)
      {
        GameObject objectWithHideFlags2 = EditorUtility.CreateGameObjectWithHideFlags("SceneLight", HideFlags.HideAndDontSave, typeof (Light));
        this.m_Light[index] = objectWithHideFlags2.GetComponent<Light>();
        this.m_Light[index].type = LightType.Directional;
        this.m_Light[index].intensity = 1f;
        this.m_Light[index].enabled = false;
      }
      this.m_Light[0].color = SceneView.kSceneViewFrontLight;
      this.m_Light[1].color = SceneView.kSceneViewUpLight - SceneView.kSceneViewMidLight;
      this.m_Light[1].transform.LookAt(Vector3.down);
      this.m_Light[1].renderMode = LightRenderMode.ForceVertex;
      this.m_Light[2].color = SceneView.kSceneViewDownLight - SceneView.kSceneViewMidLight;
      this.m_Light[2].transform.LookAt(Vector3.up);
      this.m_Light[2].renderMode = LightRenderMode.ForceVertex;
      HandleUtility.handleMaterial.SetColor("_SkyColor", SceneView.kSceneViewUpLight * 1.5f);
      HandleUtility.handleMaterial.SetColor("_GroundColor", SceneView.kSceneViewDownLight * 1.5f);
      HandleUtility.handleMaterial.SetColor("_Color", SceneView.kSceneViewFrontLight * 1.5f);
    }

    private void CallOnSceneGUI()
    {
      foreach (Editor activeEditor in this.GetActiveEditors())
      {
        if (EditorGUIUtility.IsGizmosAllowedForObject(activeEditor.target))
        {
          MethodInfo method = activeEditor.GetType().GetMethod("OnSceneGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          if (method != null)
          {
            Editor.m_AllowMultiObjectAccess = true;
            for (int index = 0; index < activeEditor.targets.Length; ++index)
            {
              this.ResetOnSceneGUIState();
              activeEditor.referenceTargetIndex = index;
              EditorGUI.BeginChangeCheck();
              Editor.m_AllowMultiObjectAccess = !activeEditor.canEditMultipleObjects;
              method.Invoke((object) activeEditor, (object[]) null);
              Editor.m_AllowMultiObjectAccess = true;
              if (EditorGUI.EndChangeCheck())
                activeEditor.serializedObject.SetIsDifferentCacheDirty();
            }
            this.ResetOnSceneGUIState();
          }
        }
      }
      if (SceneView.onSceneGUIDelegate == null)
        return;
      SceneView.onSceneGUIDelegate(this);
      this.ResetOnSceneGUIState();
    }

    private void ResetOnSceneGUIState()
    {
      Handles.matrix = Matrix4x4.identity;
      HandleUtility.s_CustomPickDistance = 5f;
      EditorGUIUtility.ResetGUIState();
      GUI.color = Color.white;
    }

    private void CallOnPreSceneGUI()
    {
      foreach (Editor activeEditor in this.GetActiveEditors())
      {
        Handles.matrix = Matrix4x4.identity;
        Component target = activeEditor.target as Component;
        if (!(bool) ((UnityEngine.Object) target) || target.gameObject.activeInHierarchy)
        {
          MethodInfo method = activeEditor.GetType().GetMethod("OnPreSceneGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
          if (method != null)
          {
            Editor.m_AllowMultiObjectAccess = true;
            for (int index = 0; index < activeEditor.targets.Length; ++index)
            {
              activeEditor.referenceTargetIndex = index;
              Editor.m_AllowMultiObjectAccess = !activeEditor.canEditMultipleObjects;
              method.Invoke((object) activeEditor, (object[]) null);
              Editor.m_AllowMultiObjectAccess = true;
            }
          }
        }
      }
      Handles.matrix = Matrix4x4.identity;
    }

    internal static void ShowNotification(string notificationText)
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (SceneView));
      List<EditorWindow> editorWindowList = new List<EditorWindow>();
      foreach (SceneView sceneView in objectsOfTypeAll)
      {
        if (sceneView.m_Parent is DockArea)
        {
          DockArea parent = (DockArea) sceneView.m_Parent;
          if ((bool) ((UnityEngine.Object) parent) && (UnityEngine.Object) parent.actualView == (UnityEngine.Object) sceneView)
            editorWindowList.Add((EditorWindow) sceneView);
        }
      }
      if (editorWindowList.Count > 0)
      {
        using (List<EditorWindow>.Enumerator enumerator = editorWindowList.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.ShowNotification(GUIContent.Temp(notificationText));
        }
      }
      else
        Debug.LogError((object) notificationText);
    }

    public static void ShowCompileErrorNotification()
    {
      SceneView.ShowNotification("All compiler errors have to be fixed before you can enter playmode!");
    }

    internal static void ShowSceneViewPlayModeSaveWarning()
    {
      GameView editorWindowOfType = (GameView) WindowLayout.FindEditorWindowOfType(typeof (GameView));
      if ((UnityEngine.Object) editorWindowOfType != (UnityEngine.Object) null)
        editorWindowOfType.ShowNotification(new GUIContent("You must exit play mode to save the scene!"));
      else
        SceneView.ShowNotification("You must exit play mode to save the scene!");
    }

    private void ResetToDefaults(EditorBehaviorMode behaviorMode)
    {
      switch (behaviorMode)
      {
        case EditorBehaviorMode.Mode2D:
          this.m_2DMode = true;
          this.m_Rotation.value = Quaternion.identity;
          this.m_Position.value = SceneView.kDefaultPivot;
          this.m_Size.value = 10f;
          this.m_Ortho.value = true;
          this.m_LastSceneViewRotation = SceneView.kDefaultRotation;
          this.m_LastSceneViewOrtho = false;
          break;
        default:
          this.m_2DMode = false;
          this.m_Rotation.value = SceneView.kDefaultRotation;
          this.m_Position.value = SceneView.kDefaultPivot;
          this.m_Size.value = 10f;
          this.m_Ortho.value = false;
          break;
      }
    }

    internal void OnNewProjectLayoutWasCreated()
    {
      this.ResetToDefaults(EditorSettings.defaultBehaviorMode);
    }

    private void On2DModeChange()
    {
      if (this.m_2DMode)
      {
        this.lastSceneViewRotation = this.rotation;
        this.m_LastSceneViewOrtho = this.orthographic;
        this.LookAt(this.pivot, Quaternion.identity, this.size, true);
        if (Tools.current == Tool.Move)
          Tools.current = Tool.Rect;
      }
      else
      {
        this.LookAt(this.pivot, this.lastSceneViewRotation, this.size, this.m_LastSceneViewOrtho);
        if (Tools.current == Tool.Rect)
          Tools.current = Tool.Move;
      }
      HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
      Tools.vertexDragging = false;
      Tools.handleOffset = Vector3.zero;
    }

    internal static void Report2DAnalytics()
    {
      UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (SceneView));
      if (objectsOfTypeAll.Length != 1 || !(objectsOfTypeAll[0] as SceneView).in2DMode)
        return;
      Analytics.Event("2D", "SceneView", "Single 2D", 1);
    }

    [Serializable]
    public class SceneViewState
    {
      public bool showFog = true;
      public bool showSkybox = true;
      public bool showFlares = true;
      public bool showMaterialUpdate;

      public SceneViewState()
      {
      }

      public SceneViewState(SceneView.SceneViewState other)
      {
        this.showFog = other.showFog;
        this.showMaterialUpdate = other.showMaterialUpdate;
        this.showSkybox = other.showSkybox;
        this.showFlares = other.showFlares;
      }

      public bool IsAllOn()
      {
        if (this.showFog && this.showMaterialUpdate && this.showSkybox)
          return this.showFlares;
        return false;
      }

      public void Toggle(bool value)
      {
        this.showFog = value;
        this.showMaterialUpdate = value;
        this.showSkybox = value;
        this.showFlares = value;
      }
    }

    private struct CursorRect
    {
      public Rect rect;
      public MouseCursor cursor;

      public CursorRect(Rect rect, MouseCursor cursor)
      {
        this.rect = rect;
        this.cursor = cursor;
      }
    }

    internal enum DraggingLockedState
    {
      NotDragging,
      Dragging,
      LookAt,
    }

    public delegate void OnSceneFunc(SceneView sceneView);
  }
}
