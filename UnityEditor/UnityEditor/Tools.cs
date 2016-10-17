// Decompiled with JetBrains decompiler
// Type: UnityEditor.Tools
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Class used to manipulate the tools used in Unity's Scene View.</para>
  /// </summary>
  public sealed class Tools : ScriptableObject
  {
    internal static ViewTool s_LockedViewTool = ViewTool.None;
    internal static int s_ButtonDown = -1;
    internal static bool s_Hidden = false;
    private static bool s_LockHandlePositionActive = false;
    private static bool s_LockHandleRectAxisActive = false;
    private static PrefKey kViewKey = new PrefKey("Tools/View", "q");
    private static PrefKey kMoveKey = new PrefKey("Tools/Move", "w");
    private static PrefKey kRotateKey = new PrefKey("Tools/Rotate", "e");
    private static PrefKey kScaleKey = new PrefKey("Tools/Scale", "r");
    private static PrefKey kRectKey = new PrefKey("Tools/Rect Handles", "t");
    private static PrefKey kPivotMode = new PrefKey("Tools/Pivot Mode", "z");
    private static PrefKey kPivotRotation = new PrefKey("Tools/Pivot Rotation", "x");
    private int m_VisibleLayers = -1;
    private int m_LockedLayers = -1;
    internal Quaternion m_GlobalHandleRotation = Quaternion.identity;
    private Tool currentTool = Tool.Move;
    private ViewTool m_ViewTool = ViewTool.Pan;
    private static Tools s_Get;
    internal static Tools.OnToolChangedFunc onToolChanged;
    private PivotMode m_PivotMode;
    private bool m_RectBlueprintMode;
    private PivotRotation m_PivotRotation;
    internal static bool vertexDragging;
    private static Vector3 s_LockHandlePosition;
    private static int s_LockHandleRectAxis;
    private static int originalTool;
    internal static Vector3 handleOffset;
    internal static Vector3 localHandleOffset;

    private static Tools get
    {
      get
      {
        if (!(bool) ((Object) Tools.s_Get))
        {
          Tools.s_Get = ScriptableObject.CreateInstance<Tools>();
          Tools.s_Get.hideFlags = HideFlags.HideAndDontSave;
        }
        return Tools.s_Get;
      }
    }

    /// <summary>
    ///   <para>The tool that is currently selected for the Scene View.</para>
    /// </summary>
    public static Tool current
    {
      get
      {
        return Tools.get.currentTool;
      }
      set
      {
        if (Tools.get.currentTool == value)
          return;
        Tool currentTool = Tools.get.currentTool;
        Tools.get.currentTool = value;
        if (Tools.onToolChanged != null)
          Tools.onToolChanged(currentTool, value);
        Tools.RepaintAllToolViews();
      }
    }

    /// <summary>
    ///   <para>The option that is currently active for the View tool in the Scene view.</para>
    /// </summary>
    public static ViewTool viewTool
    {
      get
      {
        Event current = Event.current;
        if (current != null && Tools.viewToolActive)
        {
          if (Tools.s_LockedViewTool == ViewTool.None)
          {
            bool flag1 = current.control && Application.platform == RuntimePlatform.OSXEditor;
            bool actionKey = EditorGUI.actionKey;
            bool flag2 = !actionKey && !flag1 && !current.alt;
            if (Tools.s_ButtonDown <= 0 && flag2 || Tools.s_ButtonDown <= 0 && actionKey || Tools.s_ButtonDown == 2 || (Object) SceneView.lastActiveSceneView != (Object) null && SceneView.lastActiveSceneView.in2DMode && (Tools.s_ButtonDown != 1 || !current.alt) && (Tools.s_ButtonDown > 0 || !flag1))
              Tools.get.m_ViewTool = ViewTool.Pan;
            else if (Tools.s_ButtonDown <= 0 && flag1 || Tools.s_ButtonDown == 1 && current.alt)
              Tools.get.m_ViewTool = ViewTool.Zoom;
            else if (Tools.s_ButtonDown <= 0 && current.alt)
              Tools.get.m_ViewTool = ViewTool.Orbit;
            else if (Tools.s_ButtonDown == 1 && !current.alt)
              Tools.get.m_ViewTool = ViewTool.FPS;
          }
        }
        else
          Tools.get.m_ViewTool = ViewTool.Pan;
        return Tools.get.m_ViewTool;
      }
      set
      {
        Tools.get.m_ViewTool = value;
      }
    }

    internal static bool viewToolActive
    {
      get
      {
        if (GUIUtility.hotControl != 0 && Tools.s_LockedViewTool == ViewTool.None)
          return false;
        if (Tools.s_LockedViewTool == ViewTool.None && Tools.current != Tool.View && (!Event.current.alt && Tools.s_ButtonDown != 1))
          return Tools.s_ButtonDown == 2;
        return true;
      }
    }

    /// <summary>
    ///   <para>The position of the tool handle in world space.</para>
    /// </summary>
    public static Vector3 handlePosition
    {
      get
      {
        if (!(bool) ((Object) Selection.activeTransform))
          return new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        if (Tools.s_LockHandlePositionActive)
          return Tools.s_LockHandlePosition;
        return Tools.GetHandlePosition();
      }
    }

    /// <summary>
    ///   <para>The rectangle used for the rect tool.</para>
    /// </summary>
    public static Rect handleRect
    {
      get
      {
        Bounds selectionBoundsInSpace = InternalEditorUtility.CalculateSelectionBoundsInSpace(Tools.handlePosition, Tools.handleRotation, Tools.rectBlueprintMode);
        int rectAxisForViewDir = Tools.GetRectAxisForViewDir(selectionBoundsInSpace, Tools.handleRotation, SceneView.currentDrawingSceneView.camera.transform.forward);
        return Tools.GetRectFromBoundsForAxis(selectionBoundsInSpace, rectAxisForViewDir);
      }
    }

    /// <summary>
    ///   <para>The rotation of the rect tool handle in world space.</para>
    /// </summary>
    public static Quaternion handleRectRotation
    {
      get
      {
        return Tools.GetRectRotationForAxis(Tools.handleRotation, Tools.GetRectAxisForViewDir(InternalEditorUtility.CalculateSelectionBoundsInSpace(Tools.handlePosition, Tools.handleRotation, Tools.rectBlueprintMode), Tools.handleRotation, SceneView.currentDrawingSceneView.camera.transform.forward));
      }
    }

    /// <summary>
    ///   <para>Are we in Center or Pivot mode.</para>
    /// </summary>
    public static PivotMode pivotMode
    {
      get
      {
        return Tools.get.m_PivotMode;
      }
      set
      {
        if (Tools.get.m_PivotMode == value)
          return;
        Tools.get.m_PivotMode = value;
        EditorPrefs.SetInt("PivotMode", (int) Tools.pivotMode);
      }
    }

    /// <summary>
    ///   <para>Is the rect handle in blueprint mode?</para>
    /// </summary>
    public static bool rectBlueprintMode
    {
      get
      {
        return Tools.get.m_RectBlueprintMode;
      }
      set
      {
        if (Tools.get.m_RectBlueprintMode == value)
          return;
        Tools.get.m_RectBlueprintMode = value;
        EditorPrefs.SetBool("RectBlueprintMode", Tools.rectBlueprintMode);
      }
    }

    /// <summary>
    ///   <para>The rotation of the tool handle in world space.</para>
    /// </summary>
    public static Quaternion handleRotation
    {
      get
      {
        switch (Tools.get.m_PivotRotation)
        {
          case PivotRotation.Local:
            return Tools.handleLocalRotation;
          case PivotRotation.Global:
            return Tools.get.m_GlobalHandleRotation;
          default:
            return Quaternion.identity;
        }
      }
      set
      {
        if (Tools.get.m_PivotRotation != PivotRotation.Global)
          return;
        Tools.get.m_GlobalHandleRotation = value;
      }
    }

    /// <summary>
    ///   <para>What's the rotation of the tool handle.</para>
    /// </summary>
    public static PivotRotation pivotRotation
    {
      get
      {
        return Tools.get.m_PivotRotation;
      }
      set
      {
        if (Tools.get.m_PivotRotation == value)
          return;
        Tools.get.m_PivotRotation = value;
        EditorPrefs.SetInt("PivotRotation", (int) Tools.pivotRotation);
      }
    }

    /// <summary>
    ///   <para>Hides the Tools(Move, Rotate, Resize) on the Scene view.</para>
    /// </summary>
    public static bool hidden
    {
      get
      {
        return Tools.s_Hidden;
      }
      set
      {
        Tools.s_Hidden = value;
      }
    }

    /// <summary>
    ///   <para>Which layers are visible in the scene view.</para>
    /// </summary>
    public static int visibleLayers
    {
      get
      {
        return Tools.get.m_VisibleLayers;
      }
      set
      {
        if (Tools.get.m_VisibleLayers == value)
          return;
        Tools.get.m_VisibleLayers = value;
        EditorGUIUtility.SetVisibleLayers(value);
        EditorPrefs.SetInt("VisibleLayers", Tools.visibleLayers);
      }
    }

    public static int lockedLayers
    {
      get
      {
        return Tools.get.m_LockedLayers;
      }
      set
      {
        if (Tools.get.m_LockedLayers == value)
          return;
        Tools.get.m_LockedLayers = value;
        EditorGUIUtility.SetLockedLayers(value);
        EditorPrefs.SetInt("LockedLayers", Tools.lockedLayers);
      }
    }

    internal static Quaternion handleLocalRotation
    {
      get
      {
        Transform activeTransform = Selection.activeTransform;
        if (!(bool) ((Object) activeTransform))
          return Quaternion.identity;
        if (Tools.rectBlueprintMode && InternalEditorUtility.SupportsRectLayout(activeTransform))
          return activeTransform.parent.rotation;
        return activeTransform.rotation;
      }
    }

    internal static Vector3 GetHandlePosition()
    {
      Transform activeTransform = Selection.activeTransform;
      if (!(bool) ((Object) activeTransform))
        return new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
      Vector3 vector3 = Tools.handleOffset + Tools.handleRotation * Tools.localHandleOffset;
      switch (Tools.get.m_PivotMode)
      {
        case PivotMode.Center:
          if (Tools.current == Tool.Rect)
            return Tools.handleRotation * InternalEditorUtility.CalculateSelectionBoundsInSpace(Vector3.zero, Tools.handleRotation, Tools.rectBlueprintMode).center + vector3;
          return InternalEditorUtility.CalculateSelectionBounds(true, false).center + vector3;
        case PivotMode.Pivot:
          if (Tools.current == Tool.Rect && Tools.rectBlueprintMode && InternalEditorUtility.SupportsRectLayout(activeTransform))
            return activeTransform.parent.TransformPoint(new Vector3(activeTransform.localPosition.x, activeTransform.localPosition.y, 0.0f)) + vector3;
          return activeTransform.position + vector3;
        default:
          return new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
      }
    }

    private static int GetRectAxisForViewDir(Bounds bounds, Quaternion rotation, Vector3 viewDir)
    {
      if (Tools.s_LockHandleRectAxisActive)
        return Tools.s_LockHandleRectAxis;
      if (viewDir == Vector3.zero)
        return 2;
      if (bounds.size == Vector3.zero)
        bounds.size = Vector3.one;
      int num1 = -1;
      float num2 = -1f;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        Vector3 zero1 = Vector3.zero;
        Vector3 zero2 = Vector3.zero;
        int index2 = (index1 + 1) % 3;
        int index3 = (index1 + 2) % 3;
        zero1[index2] = bounds.size[index2];
        zero2[index3] = bounds.size[index3];
        float magnitude = Vector3.Cross(Vector3.ProjectOnPlane(rotation * zero1, viewDir), Vector3.ProjectOnPlane(rotation * zero2, viewDir)).magnitude;
        if ((double) magnitude > (double) num2)
        {
          num2 = magnitude;
          num1 = index1;
        }
      }
      return num1;
    }

    private static Rect GetRectFromBoundsForAxis(Bounds bounds, int axis)
    {
      switch (axis)
      {
        case 0:
          return new Rect(-bounds.max.z, bounds.min.y, bounds.size.z, bounds.size.y);
        case 1:
          return new Rect(bounds.min.x, -bounds.max.z, bounds.size.x, bounds.size.z);
        default:
          return new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
      }
    }

    private static Quaternion GetRectRotationForAxis(Quaternion rotation, int axis)
    {
      switch (axis)
      {
        case 0:
          return rotation * Quaternion.Euler(0.0f, 90f, 0.0f);
        case 1:
          return rotation * Quaternion.Euler(-90f, 0.0f, 0.0f);
        default:
          return rotation;
      }
    }

    internal static void LockHandleRectRotation()
    {
      Tools.s_LockHandleRectAxis = Tools.GetRectAxisForViewDir(InternalEditorUtility.CalculateSelectionBoundsInSpace(Tools.handlePosition, Tools.handleRotation, Tools.rectBlueprintMode), Tools.handleRotation, SceneView.currentDrawingSceneView.camera.transform.forward);
      Tools.s_LockHandleRectAxisActive = true;
    }

    internal static void UnlockHandleRectRotation()
    {
      Tools.s_LockHandleRectAxisActive = false;
    }

    internal static int GetPivotMode()
    {
      return (int) Tools.pivotMode;
    }

    private void OnEnable()
    {
      Tools.s_Get = this;
      EditorApplication.globalEventHandler += new EditorApplication.CallbackFunction(Tools.ControlsHack);
      Tools.pivotMode = (PivotMode) EditorPrefs.GetInt("PivotMode", 0);
      Tools.rectBlueprintMode = EditorPrefs.GetBool("RectBlueprintMode", false);
      Tools.pivotRotation = (PivotRotation) EditorPrefs.GetInt("PivotRotation", 0);
      Tools.visibleLayers = EditorPrefs.GetInt("VisibleLayers", -1);
      Tools.lockedLayers = EditorPrefs.GetInt("LockedLayers", 0);
    }

    private void OnDisable()
    {
      EditorApplication.globalEventHandler -= new EditorApplication.CallbackFunction(Tools.ControlsHack);
    }

    internal static void OnSelectionChange()
    {
      Tools.ResetGlobalHandleRotation();
      Tools.localHandleOffset = Vector3.zero;
    }

    internal static void ResetGlobalHandleRotation()
    {
      Tools.get.m_GlobalHandleRotation = Quaternion.identity;
    }

    internal static void ControlsHack()
    {
      Event current = Event.current;
      if (Tools.kViewKey.activated)
      {
        Tools.current = Tool.View;
        Tools.ResetGlobalHandleRotation();
        current.Use();
        if ((bool) ((Object) Toolbar.get))
          Toolbar.get.Repaint();
        else
          Debug.LogError((object) "Press Play twice for sceneview keyboard shortcuts to work");
      }
      if (Tools.kMoveKey.activated)
      {
        Tools.current = Tool.Move;
        Tools.ResetGlobalHandleRotation();
        current.Use();
        if ((bool) ((Object) Toolbar.get))
          Toolbar.get.Repaint();
        else
          Debug.LogError((object) "Press Play twice for sceneview keyboard shortcuts to work");
      }
      if (Tools.kRotateKey.activated)
      {
        Tools.current = Tool.Rotate;
        Tools.ResetGlobalHandleRotation();
        current.Use();
        if ((bool) ((Object) Toolbar.get))
          Toolbar.get.Repaint();
        else
          Debug.LogError((object) "Press Play twice for sceneview keyboard shortcuts to work");
      }
      if (Tools.kScaleKey.activated)
      {
        Tools.current = Tool.Scale;
        Tools.ResetGlobalHandleRotation();
        current.Use();
        if ((bool) ((Object) Toolbar.get))
          Toolbar.get.Repaint();
        else
          Debug.LogError((object) "Press Play twice for sceneview keyboard shortcuts to work");
      }
      if (Tools.kRectKey.activated)
      {
        Tools.current = Tool.Rect;
        Tools.ResetGlobalHandleRotation();
        current.Use();
        if ((bool) ((Object) Toolbar.get))
          Toolbar.get.Repaint();
        else
          Debug.LogError((object) "Press Play twice for sceneview keyboard shortcuts to work");
      }
      if (Tools.kPivotMode.activated)
      {
        Tools.pivotMode = 1 - Tools.pivotMode;
        Tools.ResetGlobalHandleRotation();
        current.Use();
        Tools.RepaintAllToolViews();
      }
      if (!Tools.kPivotRotation.activated)
        return;
      Tools.pivotRotation = 1 - Tools.pivotRotation;
      Tools.ResetGlobalHandleRotation();
      current.Use();
      Tools.RepaintAllToolViews();
    }

    internal static void RepaintAllToolViews()
    {
      if ((bool) ((Object) Toolbar.get))
        Toolbar.get.Repaint();
      SceneView.RepaintAll();
      InspectorWindow.RepaintAllInspectors();
    }

    internal static void HandleKeys()
    {
      Tools.ControlsHack();
    }

    internal static void LockHandlePosition(Vector3 pos)
    {
      Tools.s_LockHandlePosition = pos;
      Tools.s_LockHandlePositionActive = true;
    }

    internal static void LockHandlePosition()
    {
      Tools.LockHandlePosition(Tools.handlePosition);
    }

    internal static void UnlockHandlePosition()
    {
      Tools.s_LockHandlePositionActive = false;
    }

    internal delegate void OnToolChangedFunc(Tool from, Tool to);
  }
}
