// Decompiled with JetBrains decompiler
// Type: UnityEditor.RectTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class RectTool : ManipulationTool
  {
    private static int s_ResizeHandlesHash = "ResizeHandles".GetHashCode();
    private static int s_RotationHandlesHash = "RotationHandles".GetHashCode();
    private static int s_MoveHandleHash = "MoveHandle".GetHashCode();
    private static int s_PivotHandleHash = "PivotHandle".GetHashCode();
    private static Rect s_StartRect = new Rect();
    private static bool s_Moving = false;
    private static int s_LockAxis = -1;
    internal const string kChangingLeft = "ChangingLeft";
    internal const string kChangingRight = "ChangingRight";
    internal const string kChangingTop = "ChangingTop";
    internal const string kChangingBottom = "ChangingBottom";
    internal const string kChangingPosX = "ChangingPosX";
    internal const string kChangingPosY = "ChangingPosY";
    internal const string kChangingWidth = "ChangingWidth";
    internal const string kChangingHeight = "ChangingHeight";
    internal const string kChangingPivot = "ChangingPivot";
    private const float kMinVisibleSize = 0.2f;
    private static RectTool s_Instance;
    private static Vector3 s_StartMouseWorldPos;
    private static Vector3 s_StartPosition;
    private static Vector2 s_StartMousePos;
    private static Vector3 s_StartRectPosition;
    private static Vector2 s_CurrentMousePos;

    public static void OnGUI(SceneView view)
    {
      if (RectTool.s_Instance == null)
        RectTool.s_Instance = new RectTool();
      RectTool.s_Instance.OnToolGUI(view);
    }

    public static Vector2 GetLocalRectPoint(Rect rect, int index)
    {
      switch (index)
      {
        case 0:
          return new Vector2(rect.xMin, rect.yMax);
        case 1:
          return new Vector2(rect.xMax, rect.yMax);
        case 2:
          return new Vector2(rect.xMax, rect.yMin);
        case 3:
          return new Vector2(rect.xMin, rect.yMin);
        default:
          return (Vector2) Vector3.zero;
      }
    }

    public override void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic)
    {
      Rect handleRect = Tools.handleRect;
      Quaternion handleRectRotation = Tools.handleRectRotation;
      Vector3[] vector3Array = new Vector3[4];
      for (int index = 0; index < 4; ++index)
      {
        Vector3 localRectPoint = (Vector3) RectTool.GetLocalRectPoint(handleRect, index);
        vector3Array[index] = handleRectRotation * localRectPoint + handlePosition;
      }
      RectHandles.RenderRectWithShadow(false, vector3Array);
      Color color1 = GUI.color;
      if ((bool) ((Object) Camera.current))
      {
        Vector3 planeNormal = !Camera.current.orthographic ? handlePosition + handleRectRotation * (Vector3) handleRect.center - Camera.current.transform.position : Camera.current.transform.forward;
        Vector3 vector1 = handleRectRotation * Vector3.right * handleRect.width;
        Vector3 vector2 = handleRectRotation * Vector3.up * handleRect.height;
        float num = Mathf.Clamp01((float) (((double) (Mathf.Sqrt(Vector3.Cross(Vector3.ProjectOnPlane(vector1, planeNormal), Vector3.ProjectOnPlane(vector2, planeNormal)).magnitude) / HandleUtility.GetHandleSize(handlePosition)) - 0.200000002980232) / 0.200000002980232 * 2.0));
        Color color2 = color1;
        color2.a *= num;
        GUI.color = color2;
      }
      Vector3 handlePosition1 = Tools.GetHandlePosition();
      if (!Tools.vertexDragging)
      {
        RectTransform component = Selection.activeTransform.GetComponent<RectTransform>();
        bool flag1 = Selection.transforms.Length > 1;
        bool flag2 = !flag1 && Tools.pivotMode == PivotMode.Pivot && (Object) component != (Object) null;
        EditorGUI.BeginDisabledGroup(!flag1 && !flag2);
        EditorGUI.BeginChangeCheck();
        Vector3 vector3 = RectTool.PivotHandleGUI(handleRect, handlePosition1, handleRectRotation);
        if (EditorGUI.EndChangeCheck() && !isStatic)
        {
          if (flag1)
            Tools.localHandleOffset += Quaternion.Inverse(Tools.handleRotation) * (vector3 - handlePosition1);
          else if (flag2)
          {
            Transform activeTransform = Selection.activeTransform;
            Undo.RecordObject((Object) component, "Move Rectangle Pivot");
            Transform transform = !Tools.rectBlueprintMode || !InternalEditorUtility.SupportsRectLayout(activeTransform) ? activeTransform : activeTransform.parent;
            Vector2 vector2_1 = (Vector2) transform.InverseTransformVector(vector3 - handlePosition1);
            vector2_1.x /= component.rect.width;
            vector2_1.y /= component.rect.height;
            Vector2 vector2_2 = component.pivot + vector2_1;
            RectTransformEditor.SetPivotSmart(component, vector2_2.x, 0, true, (Object) transform != (Object) component.transform);
            RectTransformEditor.SetPivotSmart(component, vector2_2.y, 1, true, (Object) transform != (Object) component.transform);
          }
        }
        EditorGUI.EndDisabledGroup();
      }
      TransformManipulator.BeginManipulationHandling(true);
      if (!Tools.vertexDragging)
      {
        EditorGUI.BeginChangeCheck();
        Vector3 scalePivot = handlePosition;
        Vector3 scaleDelta = RectTool.ResizeHandlesGUI(handleRect, handlePosition, handleRectRotation, out scalePivot);
        if (EditorGUI.EndChangeCheck() && !isStatic)
          TransformManipulator.SetResizeDelta(scaleDelta, scalePivot, handleRectRotation);
        bool flag = true;
        if (Tools.rectBlueprintMode)
        {
          foreach (Component transform in Selection.transforms)
          {
            if ((Object) transform.GetComponent<RectTransform>() != (Object) null)
              flag = false;
          }
        }
        if (flag)
        {
          EditorGUI.BeginChangeCheck();
          Quaternion quaternion = RectTool.RotationHandlesGUI(handleRect, handlePosition, handleRectRotation);
          if (EditorGUI.EndChangeCheck() && !isStatic)
          {
            float angle;
            Vector3 axis1;
            (Quaternion.Inverse(handleRectRotation) * quaternion).ToAngleAxis(out angle, out axis1);
            Vector3 axis2 = handleRectRotation * axis1;
            Undo.RecordObjects((Object[]) Selection.transforms, "Rotate");
            foreach (Transform transform in Selection.transforms)
            {
              transform.RotateAround(handlePosition, axis2, angle);
              if ((Object) transform.parent != (Object) null)
                transform.SendTransformChangedScale();
            }
            Tools.handleRotation = Quaternion.AngleAxis(angle, axis2) * Tools.handleRotation;
          }
        }
      }
      int num1 = (int) TransformManipulator.EndManipulationHandling();
      TransformManipulator.BeginManipulationHandling(false);
      EditorGUI.BeginChangeCheck();
      Vector3 vector3_1 = RectTool.MoveHandlesGUI(handleRect, handlePosition, handleRectRotation);
      if (EditorGUI.EndChangeCheck() && !isStatic)
        TransformManipulator.SetPositionDelta(vector3_1 - TransformManipulator.mouseDownHandlePosition);
      int num2 = (int) TransformManipulator.EndManipulationHandling();
      GUI.color = color1;
    }

    private static Vector3 GetRectPointInWorld(Rect rect, Vector3 pivot, Quaternion rotation, int xHandle, int yHandle)
    {
      Vector3 vector3_1;
      Vector3 vector3_2 = (Vector3) new Vector2(vector3_1.x = Mathf.Lerp(rect.xMin, rect.xMax, (float) xHandle * 0.5f), vector3_1.y = Mathf.Lerp(rect.yMin, rect.yMax, (float) yHandle * 0.5f));
      return rotation * vector3_2 + pivot;
    }

    private static Vector3 ResizeHandlesGUI(Rect rect, Vector3 pivot, Quaternion rotation, out Vector3 scalePivot)
    {
      if (Event.current.type == EventType.MouseDown)
        RectTool.s_StartRect = rect;
      scalePivot = pivot;
      Vector3 vector3_1 = Vector3.one;
      Quaternion quaternion = Quaternion.Inverse(rotation);
      for (int xHandle = 0; xHandle <= 2; ++xHandle)
      {
        for (int yHandle = 0; yHandle <= 2; ++yHandle)
        {
          if (xHandle != 1 || yHandle != 1)
          {
            Vector3 rectPointInWorld1 = RectTool.GetRectPointInWorld(RectTool.s_StartRect, pivot, rotation, xHandle, yHandle);
            Vector3 rectPointInWorld2 = RectTool.GetRectPointInWorld(rect, pivot, rotation, xHandle, yHandle);
            float num = 0.05f * HandleUtility.GetHandleSize(rectPointInWorld2);
            int controlId = GUIUtility.GetControlID(RectTool.s_ResizeHandlesHash, FocusType.Passive);
            if ((double) GUI.color.a > 0.0 || GUIUtility.hotControl == controlId)
            {
              EditorGUI.BeginChangeCheck();
              EventType type = Event.current.type;
              Vector3 position;
              if (xHandle == 1 || yHandle == 1)
              {
                Vector3 sideVector = xHandle != 1 ? rotation * Vector3.up * rect.height : rotation * Vector3.right * rect.width;
                Vector3 direction = xHandle != 1 ? rotation * Vector3.right : rotation * Vector3.up;
                position = RectHandles.SideSlider(controlId, rectPointInWorld2, sideVector, direction, num, (Handles.DrawCapFunction) null, 0.0f);
              }
              else
              {
                Vector3 outwardsDir1 = rotation * Vector3.right * (float) (xHandle - 1);
                Vector3 outwardsDir2 = rotation * Vector3.up * (float) (yHandle - 1);
                position = RectHandles.CornerSlider(controlId, rectPointInWorld2, rotation * Vector3.forward, outwardsDir1, outwardsDir2, num, new Handles.DrawCapFunction(RectHandles.RectScalingCap), Vector2.zero);
              }
              bool flag1 = Selection.transforms.Length == 1 && InternalEditorUtility.SupportsRectLayout(Selection.activeTransform) && Selection.activeTransform.parent.rotation == rotation;
              if (flag1)
              {
                Transform activeTransform = Selection.activeTransform;
                RectTransform component1 = activeTransform.GetComponent<RectTransform>();
                Transform parent = activeTransform.parent;
                RectTransform component2 = parent.GetComponent<RectTransform>();
                if (type == EventType.MouseDown && Event.current.type != EventType.MouseDown)
                  RectTransformSnapping.CalculateOffsetSnapValues(parent, activeTransform, component2, component1, xHandle, yHandle);
              }
              if (EditorGUI.EndChangeCheck())
              {
                ManipulationToolUtility.SetMinDragDifferenceForPos(rectPointInWorld2);
                if (flag1)
                {
                  Transform parent = Selection.activeTransform.parent;
                  RectTransform component = parent.GetComponent<RectTransform>();
                  Vector2 snapDistance = Vector2.one * HandleUtility.GetHandleSize(position) * 0.05f;
                  snapDistance.x /= (quaternion * parent.TransformVector(Vector3.right)).x;
                  snapDistance.y /= (quaternion * parent.TransformVector(Vector3.up)).y;
                  Vector3 positionBeforeSnapping = parent.InverseTransformPoint(position) - (Vector3) component.rect.min;
                  Vector3 positionAfterSnapping = (Vector3) RectTransformSnapping.SnapToGuides((Vector2) positionBeforeSnapping, snapDistance) + Vector3.forward * positionBeforeSnapping.z;
                  ManipulationToolUtility.DisableMinDragDifferenceBasedOnSnapping(positionBeforeSnapping, positionAfterSnapping);
                  position = parent.TransformPoint(positionAfterSnapping + (Vector3) component.rect.min);
                }
                bool alt = Event.current.alt;
                bool actionKey = EditorGUI.actionKey;
                bool flag2 = Event.current.shift && !actionKey;
                if (!alt)
                  scalePivot = RectTool.GetRectPointInWorld(RectTool.s_StartRect, pivot, rotation, 2 - xHandle, 2 - yHandle);
                if (flag2)
                  position = Vector3.Project(position - scalePivot, rectPointInWorld1 - scalePivot) + scalePivot;
                Vector3 vector3_2 = quaternion * (rectPointInWorld1 - scalePivot);
                Vector3 vector3_3 = quaternion * (position - scalePivot);
                if (xHandle != 1)
                  vector3_1.x = vector3_3.x / vector3_2.x;
                if (yHandle != 1)
                  vector3_1.y = vector3_3.y / vector3_2.y;
                if (flag2)
                  vector3_1 = Vector3.one * (xHandle != 1 ? vector3_1.x : vector3_1.y);
                if (actionKey && xHandle == 1)
                  vector3_1.x = !Event.current.shift ? 1f / Mathf.Max(vector3_1.y, 0.0001f) : (vector3_1.z = 1f / Mathf.Sqrt(Mathf.Max(vector3_1.y, 0.0001f)));
                if (flag2)
                  vector3_1 = Vector3.one * (xHandle != 1 ? vector3_1.x : vector3_1.y);
                if (actionKey && xHandle == 1)
                  vector3_1.x = !Event.current.shift ? 1f / Mathf.Max(vector3_1.y, 0.0001f) : (vector3_1.z = 1f / Mathf.Sqrt(Mathf.Max(vector3_1.y, 0.0001f)));
                if (actionKey && yHandle == 1)
                  vector3_1.y = !Event.current.shift ? 1f / Mathf.Max(vector3_1.x, 0.0001f) : (vector3_1.z = 1f / Mathf.Sqrt(Mathf.Max(vector3_1.x, 0.0001f)));
              }
              if (xHandle == 0)
                ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingLeft", type);
              if (xHandle == 2)
                ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingRight", type);
              if (xHandle != 1)
                ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingWidth", type);
              if (yHandle == 0)
                ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingBottom", type);
              if (yHandle == 2)
                ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingTop", type);
              if (yHandle != 1)
                ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingHeight", type);
            }
          }
        }
      }
      return vector3_1;
    }

    private static Vector3 MoveHandlesGUI(Rect rect, Vector3 pivot, Quaternion rotation)
    {
      int controlId = GUIUtility.GetControlID(RectTool.s_MoveHandleHash, FocusType.Passive);
      Vector3 position = pivot;
      float num1 = HandleUtility.GetHandleSize(pivot) * 0.2f;
      float num2 = 1f - GUI.color.a;
      Vector3[] worldPoints = new Vector3[4]{ rotation * (Vector3) new Vector2(rect.x, rect.y) + pivot, rotation * (Vector3) new Vector2(rect.xMax, rect.y) + pivot, rotation * (Vector3) new Vector2(rect.xMax, rect.yMax) + pivot, rotation * (Vector3) new Vector2(rect.x, rect.yMax) + pivot };
      VertexSnapping.HandleKeyAndMouseMove(controlId);
      bool flag = Selection.transforms.Length == 1 && InternalEditorUtility.SupportsRectLayout(Selection.activeTransform) && Selection.activeTransform.parent.rotation == rotation;
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(controlId);
      Plane plane = new Plane(worldPoints[0], worldPoints[1], worldPoints[2]);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (Tools.vertexDragging || current.button == 0 && current.modifiers == EventModifiers.None && RectHandles.RaycastGUIPointToWorldHit(current.mousePosition, plane, out RectTool.s_StartMouseWorldPos) && ((double) RectTool.SceneViewDistanceToRectangle(worldPoints, current.mousePosition) == 0.0 || (double) num2 > 0.0 && (double) RectTool.SceneViewDistanceToDisc(pivot, rotation * Vector3.forward, num1, current.mousePosition) == 0.0))
          {
            RectTool.s_StartPosition = pivot;
            RectTool.s_StartMousePos = RectTool.s_CurrentMousePos = current.mousePosition;
            RectTool.s_Moving = false;
            RectTool.s_LockAxis = -1;
            int num3 = controlId;
            GUIUtility.keyboardControl = num3;
            GUIUtility.hotControl = num3;
            EditorGUIUtility.SetWantsMouseJumping(1);
            HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
            current.Use();
            if (flag)
            {
              Transform activeTransform = Selection.activeTransform;
              RectTransform component1 = activeTransform.GetComponent<RectTransform>();
              Transform parent = activeTransform.parent;
              RectTransform component2 = parent.GetComponent<RectTransform>();
              RectTool.s_StartRectPosition = (Vector3) component1.anchoredPosition;
              RectTransformSnapping.CalculatePositionSnapValues(parent, activeTransform, component2, component1);
              break;
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            if (!RectTool.s_Moving)
              Selection.activeGameObject = SceneViewPicking.PickGameObject(current.mousePosition);
            GUIUtility.hotControl = 0;
            EditorGUIUtility.SetWantsMouseJumping(0);
            HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            RectTool.s_CurrentMousePos += current.delta;
            if (!RectTool.s_Moving && (double) (RectTool.s_CurrentMousePos - RectTool.s_StartMousePos).magnitude > 3.0)
            {
              RectTool.s_Moving = true;
              RectHandles.RaycastGUIPointToWorldHit(RectTool.s_CurrentMousePos, plane, out RectTool.s_StartMouseWorldPos);
            }
            if (RectTool.s_Moving)
            {
              if (Tools.vertexDragging)
              {
                if (HandleUtility.ignoreRaySnapObjects == null)
                  Handles.SetupIgnoreRaySnapObjects();
                Vector3 vertex;
                if (HandleUtility.FindNearestVertex(RectTool.s_CurrentMousePos, (Transform[]) null, out vertex))
                {
                  position = vertex;
                  GUI.changed = true;
                }
                ManipulationToolUtility.minDragDifference = (Vector3) Vector2.zero;
              }
              else
              {
                ManipulationToolUtility.SetMinDragDifferenceForPos(pivot);
                Vector3 hit;
                if (RectHandles.RaycastGUIPointToWorldHit(RectTool.s_CurrentMousePos, plane, out hit))
                {
                  Vector3 vector = hit - RectTool.s_StartMouseWorldPos;
                  if (current.shift)
                  {
                    vector = Quaternion.Inverse(rotation) * vector;
                    if (RectTool.s_LockAxis == -1)
                      RectTool.s_LockAxis = (double) Mathf.Abs(vector.x) <= (double) Mathf.Abs(vector.y) ? 1 : 0;
                    vector[1 - RectTool.s_LockAxis] = 0.0f;
                    vector = rotation * vector;
                  }
                  else
                    RectTool.s_LockAxis = -1;
                  if (flag)
                  {
                    Transform parent = Selection.activeTransform.parent;
                    Vector3 positionBeforeSnapping = RectTool.s_StartRectPosition + parent.InverseTransformVector(vector);
                    positionBeforeSnapping.z = 0.0f;
                    Quaternion quaternion = Quaternion.Inverse(rotation);
                    Vector2 snapDistance = Vector2.one * HandleUtility.GetHandleSize(position) * 0.05f;
                    snapDistance.x /= (quaternion * parent.TransformVector(Vector3.right)).x;
                    snapDistance.y /= (quaternion * parent.TransformVector(Vector3.up)).y;
                    Vector3 guides = (Vector3) RectTransformSnapping.SnapToGuides((Vector2) positionBeforeSnapping, snapDistance);
                    ManipulationToolUtility.DisableMinDragDifferenceBasedOnSnapping(positionBeforeSnapping, guides);
                    vector = parent.TransformVector(guides - RectTool.s_StartRectPosition);
                  }
                  position = RectTool.s_StartPosition + vector;
                  GUI.changed = true;
                }
              }
            }
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (Tools.vertexDragging)
          {
            RectHandles.RectScalingCap(controlId, pivot, rotation, 1f);
            break;
          }
          Handles.color = Handles.secondaryColor * new Color(1f, 1f, 1f, 1.5f * num2);
          Handles.CircleCap(controlId, pivot, rotation, num1);
          Handles.color = Handles.secondaryColor * new Color(1f, 1f, 1f, 0.3f * num2);
          Handles.DrawSolidDisc(pivot, rotation * Vector3.forward, num1);
          break;
      }
      ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingPosX", typeForControl);
      ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingLeft", typeForControl);
      ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingRight", typeForControl);
      ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingPosY", typeForControl);
      ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingTop", typeForControl);
      ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingBottom", typeForControl);
      return position;
    }

    private static float SceneViewDistanceToDisc(Vector3 center, Vector3 normal, float radius, Vector2 mousePos)
    {
      Plane plane = new Plane(normal, center);
      Ray worldRay = HandleUtility.GUIPointToWorldRay(mousePos);
      float enter;
      if (plane.Raycast(worldRay, out enter))
        return Mathf.Max(0.0f, (worldRay.GetPoint(enter) - center).magnitude - radius);
      return float.PositiveInfinity;
    }

    private static float SceneViewDistanceToRectangle(Vector3[] worldPoints, Vector2 mousePos)
    {
      Vector2[] screenPoints = new Vector2[4];
      for (int index = 0; index < 4; ++index)
        screenPoints[index] = HandleUtility.WorldToGUIPoint(worldPoints[index]);
      return RectTool.DistanceToRectangle(screenPoints, mousePos);
    }

    private static float DistancePointToLineSegment(Vector2 point, Vector2 a, Vector2 b)
    {
      float sqrMagnitude = (b - a).sqrMagnitude;
      if ((double) sqrMagnitude == 0.0)
        return (point - a).magnitude;
      float num = Vector2.Dot(point - a, b - a) / sqrMagnitude;
      if ((double) num < 0.0)
        return (point - a).magnitude;
      if ((double) num > 1.0)
        return (point - b).magnitude;
      Vector2 vector2 = a + num * (b - a);
      return (point - vector2).magnitude;
    }

    private static float DistanceToRectangle(Vector2[] screenPoints, Vector2 mousePos)
    {
      bool flag = false;
      int num1 = 4;
      for (int index = 0; index < 5; ++index)
      {
        Vector3 screenPoint1 = (Vector3) screenPoints[index % 4];
        Vector3 screenPoint2 = (Vector3) screenPoints[num1 % 4];
        if ((double) screenPoint1.y > (double) mousePos.y != (double) screenPoint2.y > (double) mousePos.y && (double) mousePos.x < ((double) screenPoint2.x - (double) screenPoint1.x) * ((double) mousePos.y - (double) screenPoint1.y) / ((double) screenPoint2.y - (double) screenPoint1.y) + (double) screenPoint1.x)
          flag = !flag;
        num1 = index;
      }
      if (flag)
        return 0.0f;
      float num2 = -1f;
      for (int index = 0; index < 4; ++index)
      {
        Vector3 screenPoint1 = (Vector3) screenPoints[index];
        Vector3 screenPoint2 = (Vector3) screenPoints[(index + 1) % 4];
        float lineSegment = RectTool.DistancePointToLineSegment(mousePos, (Vector2) screenPoint1, (Vector2) screenPoint2);
        if ((double) lineSegment < (double) num2 || (double) num2 < 0.0)
          num2 = lineSegment;
      }
      return num2;
    }

    private static Quaternion RotationHandlesGUI(Rect rect, Vector3 pivot, Quaternion rotation)
    {
      Vector3 eulerAngles = rotation.eulerAngles;
      int xHandle = 0;
      while (xHandle <= 2)
      {
        int yHandle = 0;
        while (yHandle <= 2)
        {
          Vector3 rectPointInWorld = RectTool.GetRectPointInWorld(rect, pivot, rotation, xHandle, yHandle);
          float handleSize = 0.05f * HandleUtility.GetHandleSize(rectPointInWorld);
          int controlId = GUIUtility.GetControlID(RectTool.s_RotationHandlesHash, FocusType.Passive);
          if ((double) GUI.color.a > 0.0 || GUIUtility.hotControl == controlId)
          {
            EditorGUI.BeginChangeCheck();
            Vector3 outwardsDir1 = rotation * Vector3.right * (float) (xHandle - 1);
            Vector3 outwardsDir2 = rotation * Vector3.up * (float) (yHandle - 1);
            float num = RectHandles.RotationSlider(controlId, rectPointInWorld, eulerAngles.z, pivot, rotation * Vector3.forward, outwardsDir1, outwardsDir2, handleSize, (Handles.DrawCapFunction) null, Vector2.zero);
            if (EditorGUI.EndChangeCheck())
            {
              if (Event.current.shift)
                num = Mathf.Round((float) (((double) num - (double) eulerAngles.z) / 15.0)) * 15f + eulerAngles.z;
              eulerAngles.z = num;
              rotation = Quaternion.Euler(eulerAngles);
            }
          }
          yHandle += 2;
        }
        xHandle += 2;
      }
      return rotation;
    }

    private static Vector3 PivotHandleGUI(Rect rect, Vector3 pivot, Quaternion rotation)
    {
      int controlId = GUIUtility.GetControlID(RectTool.s_PivotHandleHash, FocusType.Passive);
      EventType typeForControl = Event.current.GetTypeForControl(controlId);
      if ((double) GUI.color.a > 0.0 || GUIUtility.hotControl == controlId)
      {
        EventType eventType = typeForControl;
        EditorGUI.BeginChangeCheck();
        Vector3 vector3 = Handles.Slider2D(controlId, pivot, rotation * Vector3.forward, rotation * Vector3.right, rotation * Vector3.up, HandleUtility.GetHandleSize(pivot) * 0.1f, new Handles.DrawCapFunction(RectHandles.PivotCap), Vector2.zero);
        if (eventType == EventType.MouseDown && GUIUtility.hotControl == controlId)
          RectTransformSnapping.CalculatePivotSnapValues(rect, pivot, rotation);
        if (EditorGUI.EndChangeCheck())
        {
          Vector2 vector2_1 = (Vector2) (Quaternion.Inverse(rotation) * (vector3 - pivot));
          vector2_1.x /= rect.width;
          vector2_1.y /= rect.height;
          Vector2 vector2_2 = new Vector2(-rect.x / rect.width, -rect.y / rect.height);
          Vector2 vector2_3 = RectTransformSnapping.SnapToGuides(vector2_2 + vector2_1, HandleUtility.GetHandleSize(pivot) * 0.05f * new Vector2(1f / rect.width, 1f / rect.height)) - vector2_2;
          vector2_3.x *= rect.width;
          vector2_3.y *= rect.height;
          pivot += rotation * (Vector3) vector2_3;
        }
      }
      ManipulationToolUtility.DetectDraggingBasedOnMouseDownUp("ChangingPivot", typeForControl);
      return pivot;
    }
  }
}
