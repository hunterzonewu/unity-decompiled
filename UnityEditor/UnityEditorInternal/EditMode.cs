// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.EditMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  [InitializeOnLoad]
  public class EditMode
  {
    private static Tool s_ToolBeforeEnteringEditMode = Tool.Move;
    private const string kEditModeStringKey = "EditModeState";
    private const string kPrevToolStringKey = "EditModePrevTool";
    private const string kOwnerStringKey = "EditModeOwner";
    private const float k_EditColliderbuttonWidth = 33f;
    private const float k_EditColliderbuttonHeight = 23f;
    private const float k_SpaceBetweenLabelAndButton = 5f;
    private static bool s_Debug;
    private static GUIStyle s_ToolbarBaseStyle;
    private static GUIStyle s_EditColliderButtonStyle;
    public static EditMode.OnEditModeStopFunc onEditModeEndDelegate;
    public static EditMode.OnEditModeStartFunc onEditModeStartDelegate;
    private static int s_OwnerID;
    private static EditMode.SceneViewEditMode s_EditMode;

    private static Tool toolBeforeEnteringEditMode
    {
      get
      {
        return EditMode.s_ToolBeforeEnteringEditMode;
      }
      set
      {
        EditMode.s_ToolBeforeEnteringEditMode = value;
        SessionState.SetInt("EditModePrevTool", (int) EditMode.s_ToolBeforeEnteringEditMode);
        if (!EditMode.s_Debug)
          return;
        Debug.Log((object) ("Set toolBeforeEnteringEditMode " + (object) value));
      }
    }

    private static int ownerID
    {
      get
      {
        return EditMode.s_OwnerID;
      }
      set
      {
        EditMode.s_OwnerID = value;
        SessionState.SetInt("EditModeOwner", EditMode.s_OwnerID);
        if (!EditMode.s_Debug)
          return;
        Debug.Log((object) ("Set ownerID " + (object) value));
      }
    }

    public static EditMode.SceneViewEditMode editMode
    {
      get
      {
        return EditMode.s_EditMode;
      }
      private set
      {
        if (EditMode.s_EditMode == EditMode.SceneViewEditMode.None && value != EditMode.SceneViewEditMode.None)
        {
          EditMode.toolBeforeEnteringEditMode = Tools.current == Tool.None ? Tool.Move : Tools.current;
          Tools.current = Tool.None;
        }
        else if (EditMode.s_EditMode != EditMode.SceneViewEditMode.None && value == EditMode.SceneViewEditMode.None)
          EditMode.ResetToolToPrevious();
        EditMode.s_EditMode = value;
        SessionState.SetInt("EditModeState", (int) EditMode.s_EditMode);
        if (!EditMode.s_Debug)
          return;
        Debug.Log((object) ("Set editMode " + (object) EditMode.s_EditMode));
      }
    }

    static EditMode()
    {
      EditMode.ownerID = SessionState.GetInt("EditModeOwner", EditMode.ownerID);
      EditMode.editMode = (EditMode.SceneViewEditMode) SessionState.GetInt("EditModeState", (int) EditMode.editMode);
      EditMode.toolBeforeEnteringEditMode = (Tool) SessionState.GetInt("EditModePrevTool", (int) EditMode.toolBeforeEnteringEditMode);
      Selection.selectionChanged += new System.Action(EditMode.OnSelectionChange);
      if (!EditMode.s_Debug)
        return;
      Debug.Log((object) ("EditMode static constructor: " + (object) EditMode.ownerID + " " + (object) EditMode.editMode + " " + (object) EditMode.toolBeforeEnteringEditMode));
    }

    public static bool IsOwner(Editor editor)
    {
      return editor.GetInstanceID() == EditMode.ownerID;
    }

    public static void ResetToolToPrevious()
    {
      if (Tools.current != Tool.None)
        return;
      Tools.current = EditMode.toolBeforeEnteringEditMode;
    }

    private static void EndSceneViewEditing()
    {
      EditMode.ChangeEditMode(EditMode.SceneViewEditMode.None, new Bounds(), (Editor) null);
    }

    public static void OnSelectionChange()
    {
      EditMode.QuitEditMode();
    }

    public static void QuitEditMode()
    {
      if (Tools.current == Tool.None && EditMode.editMode != EditMode.SceneViewEditMode.None)
        EditMode.ResetToolToPrevious();
      EditMode.EndSceneViewEditing();
    }

    private static void DetectMainToolChange()
    {
      if (Tools.current == Tool.None || EditMode.editMode == EditMode.SceneViewEditMode.None)
        return;
      EditMode.EndSceneViewEditing();
    }

    public static void DoEditModeInspectorModeButton(EditMode.SceneViewEditMode mode, string label, GUIContent icon, Bounds bounds, Editor caller)
    {
      if (EditorUtility.IsPersistent(caller.target))
        return;
      EditMode.DetectMainToolChange();
      if (EditMode.s_EditColliderButtonStyle == null)
      {
        EditMode.s_EditColliderButtonStyle = new GUIStyle((GUIStyle) "Button");
        EditMode.s_EditColliderButtonStyle.padding = new RectOffset(0, 0, 0, 0);
        EditMode.s_EditColliderButtonStyle.margin = new RectOffset(0, 0, 0, 0);
      }
      Rect controlRect = EditorGUILayout.GetControlRect(true, 23f, new GUILayoutOption[0]);
      Rect position1 = new Rect(controlRect.xMin + EditorGUIUtility.labelWidth, controlRect.yMin, 33f, 23f);
      Vector2 vector2 = GUI.skin.label.CalcSize(new GUIContent(label));
      Rect position2 = new Rect(position1.xMax + 5f, controlRect.yMin + (float) (((double) controlRect.height - (double) vector2.y) * 0.5), vector2.x, controlRect.height);
      int instanceId = caller.GetInstanceID();
      bool flag1 = EditMode.editMode == mode && EditMode.ownerID == instanceId;
      EditorGUI.BeginChangeCheck();
      bool flag2 = GUI.Toggle(position1, flag1, icon, EditMode.s_EditColliderButtonStyle);
      GUI.Label(position2, label);
      if (!EditorGUI.EndChangeCheck())
        return;
      EditMode.ChangeEditMode(!flag2 ? EditMode.SceneViewEditMode.None : mode, bounds, caller);
    }

    public static void DoInspectorToolbar(EditMode.SceneViewEditMode[] modes, GUIContent[] guiContents, Bounds bounds, Editor caller)
    {
      if (EditorUtility.IsPersistent(caller.target))
        return;
      EditMode.DetectMainToolChange();
      if (EditMode.s_ToolbarBaseStyle == null)
        EditMode.s_ToolbarBaseStyle = (GUIStyle) "Command";
      int instanceId = caller.GetInstanceID();
      int selected = ArrayUtility.IndexOf<EditMode.SceneViewEditMode>(modes, EditMode.editMode);
      if (EditMode.ownerID != instanceId)
        selected = -1;
      EditorGUI.BeginChangeCheck();
      int index = GUILayout.Toolbar(selected, guiContents, EditMode.s_ToolbarBaseStyle, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      EditMode.ChangeEditMode(index != selected ? modes[index] : EditMode.SceneViewEditMode.None, bounds, caller);
    }

    private static void ChangeEditMode(EditMode.SceneViewEditMode mode, Bounds bounds, Editor caller)
    {
      Editor objectFromInstanceId = InternalEditorUtility.GetObjectFromInstanceID(EditMode.ownerID) as Editor;
      EditMode.editMode = mode;
      EditMode.ownerID = mode == EditMode.SceneViewEditMode.None ? 0 : caller.GetInstanceID();
      if (EditMode.onEditModeEndDelegate != null)
        EditMode.onEditModeEndDelegate(objectFromInstanceId);
      if (EditMode.editMode != EditMode.SceneViewEditMode.None && EditMode.onEditModeStartDelegate != null)
        EditMode.onEditModeStartDelegate(caller, EditMode.editMode);
      EditMode.EditModeChanged(bounds);
      InspectorWindow.RepaintAllInspectors();
    }

    private static void EditModeChanged(Bounds bounds)
    {
      if (EditMode.editMode != EditMode.SceneViewEditMode.None && (UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null && ((UnityEngine.Object) SceneView.lastActiveSceneView.camera != (UnityEngine.Object) null && !EditMode.SeenByCamera(SceneView.lastActiveSceneView.camera, bounds)))
        SceneView.lastActiveSceneView.Frame(bounds);
      SceneView.RepaintAll();
    }

    private static bool SeenByCamera(Camera camera, Bounds bounds)
    {
      return EditMode.AnyPointSeenByCamera(camera, EditMode.GetPoints(bounds));
    }

    private static Vector3[] GetPoints(Bounds bounds)
    {
      return EditMode.BoundsToPoints(bounds);
    }

    private static Vector3[] BoundsToPoints(Bounds bounds)
    {
      return new Vector3[8]{ new Vector3(bounds.min.x, bounds.min.y, bounds.min.z), new Vector3(bounds.min.x, bounds.min.y, bounds.max.z), new Vector3(bounds.min.x, bounds.max.y, bounds.min.z), new Vector3(bounds.min.x, bounds.max.y, bounds.max.z), new Vector3(bounds.max.x, bounds.min.y, bounds.min.z), new Vector3(bounds.max.x, bounds.min.y, bounds.max.z), new Vector3(bounds.max.x, bounds.max.y, bounds.min.z), new Vector3(bounds.max.x, bounds.max.y, bounds.max.z) };
    }

    private static bool AnyPointSeenByCamera(Camera camera, Vector3[] points)
    {
      foreach (Vector3 point in points)
      {
        if (EditMode.PointSeenByCamera(camera, point))
          return true;
      }
      return false;
    }

    private static bool PointSeenByCamera(Camera camera, Vector3 point)
    {
      Vector3 viewportPoint = camera.WorldToViewportPoint(point);
      if ((double) viewportPoint.x > 0.0 && (double) viewportPoint.x < 1.0 && (double) viewportPoint.y > 0.0)
        return (double) viewportPoint.y < 1.0;
      return false;
    }

    public enum SceneViewEditMode
    {
      None,
      Collider,
      Cloth,
      ReflectionProbeBox,
      ReflectionProbeOrigin,
    }

    public delegate void OnEditModeStopFunc(Editor editor);

    public delegate void OnEditModeStartFunc(Editor editor, EditMode.SceneViewEditMode mode);
  }
}
