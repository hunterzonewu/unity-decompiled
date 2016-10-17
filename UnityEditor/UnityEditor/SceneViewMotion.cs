// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneViewMotion
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SceneViewMotion
  {
    private static float s_FlySpeed = 0.0f;
    private static float s_StartZoom = 0.0f;
    private static float s_ZoomSpeed = 0.0f;
    private static float s_TotalMotion = 0.0f;
    private static bool s_Dragged = false;
    private static int s_ViewToolID = GUIUtility.GetPermanentControlID();
    private static PrefKey kFPSForward = new PrefKey("View/FPS Forward", "w");
    private static PrefKey kFPSBack = new PrefKey("View/FPS Back", "s");
    private static PrefKey kFPSLeft = new PrefKey("View/FPS Strafe Left", "a");
    private static PrefKey kFPSRight = new PrefKey("View/FPS Strafe Right", "d");
    private static PrefKey kFPSUp = new PrefKey("View/FPS Strafe Up", "e");
    private static PrefKey kFPSDown = new PrefKey("View/FPS Strafe Down", "q");
    private static TimeHelper s_FPSTiming = new TimeHelper();
    private const float kFlyAcceleration = 1.8f;
    private static Vector3 s_Motion;

    public static void ArrowKeys(SceneView sv)
    {
      Event current = Event.current;
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != controlId || EditorGUI.actionKey)
        return;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.KeyDown:
          switch (current.keyCode)
          {
            case KeyCode.UpArrow:
              sv.viewIsLockedToObject = false;
              if (sv.m_Ortho.value)
                SceneViewMotion.s_Motion.y = 1f;
              else
                SceneViewMotion.s_Motion.z = 1f;
              GUIUtility.hotControl = controlId;
              current.Use();
              return;
            case KeyCode.DownArrow:
              sv.viewIsLockedToObject = false;
              if (sv.m_Ortho.value)
                SceneViewMotion.s_Motion.y = -1f;
              else
                SceneViewMotion.s_Motion.z = -1f;
              GUIUtility.hotControl = controlId;
              current.Use();
              return;
            case KeyCode.RightArrow:
              sv.viewIsLockedToObject = false;
              SceneViewMotion.s_Motion.x = 1f;
              GUIUtility.hotControl = controlId;
              current.Use();
              return;
            case KeyCode.LeftArrow:
              sv.viewIsLockedToObject = false;
              SceneViewMotion.s_Motion.x = -1f;
              GUIUtility.hotControl = controlId;
              current.Use();
              return;
            default:
              return;
          }
        case EventType.KeyUp:
          if (GUIUtility.hotControl != controlId)
            break;
          switch (current.keyCode)
          {
            case KeyCode.UpArrow:
            case KeyCode.DownArrow:
              SceneViewMotion.s_Motion.z = 0.0f;
              SceneViewMotion.s_Motion.y = 0.0f;
              current.Use();
              return;
            case KeyCode.RightArrow:
            case KeyCode.LeftArrow:
              SceneViewMotion.s_Motion.x = 0.0f;
              current.Use();
              return;
            default:
              return;
          }
        case EventType.Layout:
          if (GUIUtility.hotControl != controlId)
            break;
          Vector3 forward;
          if (!sv.m_Ortho.value)
          {
            forward = Camera.current.transform.forward + Camera.current.transform.up * 0.3f;
            forward.y = 0.0f;
            forward.Normalize();
          }
          else
            forward = Camera.current.transform.forward;
          Vector3 movementDirection = SceneViewMotion.GetMovementDirection();
          sv.LookAtDirect(sv.pivot + Quaternion.LookRotation(forward) * movementDirection, sv.rotation);
          if ((double) SceneViewMotion.s_Motion.sqrMagnitude == 0.0)
          {
            sv.pivot = sv.pivot;
            SceneViewMotion.s_FlySpeed = 0.0f;
            GUIUtility.hotControl = 0;
            break;
          }
          sv.Repaint();
          break;
      }
    }

    public static void DoViewTool(SceneView view)
    {
      Event current = Event.current;
      int viewToolId = SceneViewMotion.s_ViewToolID;
      EventType typeForControl = current.GetTypeForControl(viewToolId);
      if ((bool) ((Object) view) && Tools.s_LockedViewTool == ViewTool.FPS)
        view.FixNegativeSize();
      switch (typeForControl)
      {
        case EventType.MouseDown:
          SceneViewMotion.HandleMouseDown(view, viewToolId, current.button);
          break;
        case EventType.MouseUp:
          SceneViewMotion.HandleMouseUp(view, viewToolId, current.button, current.clickCount);
          break;
        case EventType.MouseDrag:
          SceneViewMotion.HandleMouseDrag(view, viewToolId);
          break;
        case EventType.KeyDown:
          SceneViewMotion.HandleKeyDown(view);
          break;
        case EventType.KeyUp:
          SceneViewMotion.HandleKeyUp();
          break;
        case EventType.ScrollWheel:
          SceneViewMotion.HandleScrollWheel(view, !current.alt);
          break;
        case EventType.Layout:
          Vector3 movementDirection = SceneViewMotion.GetMovementDirection();
          if (GUIUtility.hotControl != viewToolId || (double) movementDirection.sqrMagnitude == 0.0)
            break;
          view.pivot = view.pivot + view.rotation * movementDirection;
          view.Repaint();
          break;
      }
    }

    private static Vector3 GetMovementDirection()
    {
      float p = SceneViewMotion.s_FPSTiming.Update();
      if ((double) SceneViewMotion.s_Motion.sqrMagnitude == 0.0)
      {
        SceneViewMotion.s_FlySpeed = 0.0f;
        return Vector3.zero;
      }
      float num = !Event.current.shift ? 1f : 5f;
      if ((double) SceneViewMotion.s_FlySpeed == 0.0)
        SceneViewMotion.s_FlySpeed = 9f;
      else
        SceneViewMotion.s_FlySpeed *= Mathf.Pow(1.8f, p);
      return SceneViewMotion.s_Motion.normalized * SceneViewMotion.s_FlySpeed * num * p;
    }

    private static void HandleMouseDown(SceneView view, int id, int button)
    {
      SceneViewMotion.s_Dragged = false;
      if (!Tools.viewToolActive)
        return;
      ViewTool viewTool = Tools.viewTool;
      if (Tools.s_LockedViewTool == viewTool)
        return;
      Event current = Event.current;
      GUIUtility.hotControl = id;
      Tools.s_LockedViewTool = viewTool;
      SceneViewMotion.s_StartZoom = view.size;
      SceneViewMotion.s_ZoomSpeed = Mathf.Max(Mathf.Abs(SceneViewMotion.s_StartZoom), 0.3f);
      SceneViewMotion.s_TotalMotion = 0.0f;
      if ((bool) ((Object) view))
        view.Focus();
      if ((bool) ((Object) Toolbar.get))
        Toolbar.get.Repaint();
      EditorGUIUtility.SetWantsMouseJumping(1);
      current.Use();
      GUIUtility.ExitGUI();
    }

    private static void ResetDragState()
    {
      GUIUtility.hotControl = 0;
      Tools.s_LockedViewTool = ViewTool.None;
      Tools.s_ButtonDown = -1;
      SceneViewMotion.s_Motion = Vector3.zero;
      if ((bool) ((Object) Toolbar.get))
        Toolbar.get.Repaint();
      EditorGUIUtility.SetWantsMouseJumping(0);
    }

    private static void HandleMouseUp(SceneView view, int id, int button, int clickCount)
    {
      if (GUIUtility.hotControl != id)
        return;
      SceneViewMotion.ResetDragState();
      RaycastHit hit;
      if (button == 2 && !SceneViewMotion.s_Dragged && SceneViewMotion.RaycastWorld(Event.current.mousePosition, out hit))
      {
        Vector3 vector3 = view.pivot - view.rotation * Vector3.forward * view.cameraDistance;
        float newSize = view.size;
        if (!view.orthographic)
          newSize = view.size * Vector3.Dot(hit.point - vector3, view.rotation * Vector3.forward) / view.cameraDistance;
        view.LookAt(hit.point, view.rotation, newSize);
      }
      Event.current.Use();
    }

    private static bool RaycastWorld(Vector2 position, out RaycastHit hit)
    {
      hit = new RaycastHit();
      GameObject gameObject = HandleUtility.PickGameObject(position, false);
      if (!(bool) ((Object) gameObject))
        return false;
      Ray worldRay = HandleUtility.GUIPointToWorldRay(position);
      MeshFilter[] componentsInChildren = gameObject.GetComponentsInChildren<MeshFilter>();
      float num = float.PositiveInfinity;
      foreach (MeshFilter meshFilter in componentsInChildren)
      {
        Mesh sharedMesh = meshFilter.sharedMesh;
        RaycastHit hit1;
        if ((bool) ((Object) sharedMesh) && HandleUtility.IntersectRayMesh(worldRay, sharedMesh, meshFilter.transform.localToWorldMatrix, out hit1) && (double) hit1.distance < (double) num)
        {
          hit = hit1;
          num = hit.distance;
        }
      }
      if ((double) num == double.PositiveInfinity)
        hit.point = Vector3.Project(gameObject.transform.position - worldRay.origin, worldRay.direction) + worldRay.origin;
      return true;
    }

    private static void OrbitCameraBehavior(SceneView view)
    {
      Event current = Event.current;
      view.FixNegativeSize();
      Quaternion target = view.m_Rotation.target;
      Quaternion quaternion1 = Quaternion.AngleAxis((float) ((double) current.delta.y * (3.0 / 1000.0) * 57.2957801818848), target * Vector3.right) * target;
      Quaternion quaternion2 = Quaternion.AngleAxis((float) ((double) current.delta.x * (3.0 / 1000.0) * 57.2957801818848), Vector3.up) * quaternion1;
      if ((double) view.size < 0.0)
      {
        view.pivot = view.camera.transform.position;
        view.size = 0.0f;
      }
      view.rotation = quaternion2;
    }

    private static void HandleMouseDrag(SceneView view, int id)
    {
      SceneViewMotion.s_Dragged = true;
      if (GUIUtility.hotControl != id)
        return;
      Event current = Event.current;
      switch (Tools.s_LockedViewTool)
      {
        case ViewTool.Orbit:
          if (!view.in2DMode)
          {
            SceneViewMotion.OrbitCameraBehavior(view);
            view.svRot.UpdateGizmoLabel(view, view.rotation * Vector3.forward, view.m_Ortho.target);
            break;
          }
          break;
        case ViewTool.Pan:
          view.viewIsLockedToObject = false;
          view.FixNegativeSize();
          Vector3 vector3_1 = Camera.current.ScreenToWorldPoint(view.camera.WorldToScreenPoint(view.pivot) + new Vector3(-Event.current.delta.x, Event.current.delta.y, 0.0f)) - view.pivot;
          if (current.shift)
            vector3_1 *= 4f;
          view.pivot += vector3_1;
          break;
        case ViewTool.Zoom:
          float num = HandleUtility.niceMouseDeltaZoom * (!current.shift ? 3f : 9f);
          if (view.orthographic)
          {
            view.size = Mathf.Max(0.0001f, view.size * (float) (1.0 + (double) num * (1.0 / 1000.0)));
            break;
          }
          SceneViewMotion.s_TotalMotion += num;
          view.size = (double) SceneViewMotion.s_TotalMotion >= 0.0 ? view.size + (float) ((double) num * (double) SceneViewMotion.s_ZoomSpeed * (3.0 / 1000.0)) : SceneViewMotion.s_StartZoom * (float) (1.0 + (double) SceneViewMotion.s_TotalMotion * (1.0 / 1000.0));
          break;
        case ViewTool.FPS:
          if (!view.in2DMode)
          {
            if (!view.orthographic)
            {
              view.viewIsLockedToObject = false;
              Vector3 vector3_2 = view.pivot - view.rotation * Vector3.forward * view.cameraDistance;
              Quaternion rotation = view.rotation;
              Quaternion quaternion1 = Quaternion.AngleAxis((float) ((double) current.delta.y * (3.0 / 1000.0) * 57.2957801818848), rotation * Vector3.right) * rotation;
              Quaternion quaternion2 = Quaternion.AngleAxis((float) ((double) current.delta.x * (3.0 / 1000.0) * 57.2957801818848), Vector3.up) * quaternion1;
              view.rotation = quaternion2;
              view.pivot = vector3_2 + quaternion2 * Vector3.forward * view.cameraDistance;
            }
            else
              SceneViewMotion.OrbitCameraBehavior(view);
            view.svRot.UpdateGizmoLabel(view, view.rotation * Vector3.forward, view.m_Ortho.target);
            break;
          }
          break;
        default:
          Debug.Log((object) "Enum value Tools.s_LockViewTool not handled");
          break;
      }
      current.Use();
    }

    private static void HandleKeyDown(SceneView sceneView)
    {
      if (Event.current.keyCode == KeyCode.Escape && GUIUtility.hotControl == SceneViewMotion.s_ViewToolID)
        SceneViewMotion.ResetDragState();
      if (Tools.s_LockedViewTool != ViewTool.FPS)
        return;
      Event current = Event.current;
      Vector3 motion = SceneViewMotion.s_Motion;
      if (current.keyCode == (Event) SceneViewMotion.kFPSForward.keyCode)
      {
        sceneView.viewIsLockedToObject = false;
        SceneViewMotion.s_Motion.z = 1f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSBack.keyCode)
      {
        sceneView.viewIsLockedToObject = false;
        SceneViewMotion.s_Motion.z = -1f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSLeft.keyCode)
      {
        sceneView.viewIsLockedToObject = false;
        SceneViewMotion.s_Motion.x = -1f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSRight.keyCode)
      {
        sceneView.viewIsLockedToObject = false;
        SceneViewMotion.s_Motion.x = 1f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSUp.keyCode)
      {
        sceneView.viewIsLockedToObject = false;
        SceneViewMotion.s_Motion.y = 1f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSDown.keyCode)
      {
        sceneView.viewIsLockedToObject = false;
        SceneViewMotion.s_Motion.y = -1f;
        current.Use();
      }
      if (current.type == EventType.KeyDown || (double) motion.sqrMagnitude != 0.0)
        return;
      SceneViewMotion.s_FPSTiming.Begin();
    }

    private static void HandleKeyUp()
    {
      if (Tools.s_LockedViewTool != ViewTool.FPS)
        return;
      Event current = Event.current;
      if (current.keyCode == (Event) SceneViewMotion.kFPSForward.keyCode)
      {
        SceneViewMotion.s_Motion.z = 0.0f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSBack.keyCode)
      {
        SceneViewMotion.s_Motion.z = 0.0f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSLeft.keyCode)
      {
        SceneViewMotion.s_Motion.x = 0.0f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSRight.keyCode)
      {
        SceneViewMotion.s_Motion.x = 0.0f;
        current.Use();
      }
      else if (current.keyCode == (Event) SceneViewMotion.kFPSUp.keyCode)
      {
        SceneViewMotion.s_Motion.y = 0.0f;
        current.Use();
      }
      else
      {
        if (current.keyCode != (Event) SceneViewMotion.kFPSDown.keyCode)
          return;
        SceneViewMotion.s_Motion.y = 0.0f;
        current.Use();
      }
    }

    private static void HandleScrollWheel(SceneView view, bool zoomTowardsCenter)
    {
      float cameraDistance = view.cameraDistance;
      Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
      Vector3 vector3 = worldRay.origin + worldRay.direction * view.cameraDistance - view.pivot;
      float y = Event.current.delta.y;
      if (!view.orthographic)
      {
        float num = (float) ((double) Mathf.Abs(view.size) * (double) y * 0.0149999996647239);
        if ((double) num > 0.0 && (double) num < 0.300000011920929)
          num = 0.3f;
        else if ((double) num < 0.0 && (double) num > -0.300000011920929)
          num = -0.3f;
        view.size += num;
      }
      else
        view.size = Mathf.Abs(view.size) * (float) ((double) y * 0.0149999996647239 + 1.0);
      float num1 = (float) (1.0 - (double) view.cameraDistance / (double) cameraDistance);
      if (!zoomTowardsCenter)
        view.pivot += vector3 * num1;
      Event.current.Use();
    }

    public static void ResetMotion()
    {
      SceneViewMotion.s_Motion = Vector3.zero;
    }
  }
}
