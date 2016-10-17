// Decompiled with JetBrains decompiler
// Type: UnityEditor.HandleUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Helper functions for Scene View style 3D GUI.</para>
  /// </summary>
  public sealed class HandleUtility
  {
    private static bool s_UseYSign = false;
    private static bool s_UseYSignZoom = false;
    private static Vector3[] points = new Vector3[5]
    {
      Vector3.zero,
      Vector3.zero,
      Vector3.zero,
      Vector3.zero,
      Vector3.zero
    };
    internal static float s_CustomPickDistance = 5f;
    private static Stack s_SavedCameras = new Stack();
    internal static Transform[] ignoreRaySnapObjects = (Transform[]) null;
    internal const float kPickDistance = 5f;
    private const float kHandleSize = 80f;
    private static int s_NearestControl;
    private static float s_NearestDistance;
    private static Material s_HandleMaterial;
    private static Material s_HandleWireMaterial;
    private static Material s_HandleWireMaterial2D;
    private static int s_HandleWireTextureIndex;
    private static int s_HandleWireTextureIndex2D;
    private static Material s_HandleDottedWireMaterial;
    private static Material s_HandleDottedWireMaterial2D;
    private static int s_HandleDottedWireTextureIndex;
    private static int s_HandleDottedWireTextureIndex2D;

    /// <summary>
    ///   <para>Get standard acceleration for dragging values (Read Only).</para>
    /// </summary>
    public static float acceleration
    {
      get
      {
        return (float) ((!Event.current.shift ? 1.0 : 4.0) * (!Event.current.alt ? 1.0 : 0.25));
      }
    }

    /// <summary>
    ///   <para>Get nice mouse delta to use for dragging a float value (Read Only).</para>
    /// </summary>
    public static float niceMouseDelta
    {
      get
      {
        Vector2 delta = Event.current.delta;
        delta.y = -delta.y;
        if ((double) Mathf.Abs(Mathf.Abs(delta.x) - Mathf.Abs(delta.y)) / (double) Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y)) > 0.100000001490116)
          HandleUtility.s_UseYSign = (double) Mathf.Abs(delta.x) <= (double) Mathf.Abs(delta.y);
        if (HandleUtility.s_UseYSign)
          return Mathf.Sign(delta.y) * delta.magnitude * HandleUtility.acceleration;
        return Mathf.Sign(delta.x) * delta.magnitude * HandleUtility.acceleration;
      }
    }

    /// <summary>
    ///   <para>Get nice mouse delta to use for zooming (Read Only).</para>
    /// </summary>
    public static float niceMouseDeltaZoom
    {
      get
      {
        Vector2 vector2 = -Event.current.delta;
        if ((double) Mathf.Abs(Mathf.Abs(vector2.x) - Mathf.Abs(vector2.y)) / (double) Mathf.Max(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y)) > 0.100000001490116)
          HandleUtility.s_UseYSignZoom = (double) Mathf.Abs(vector2.x) <= (double) Mathf.Abs(vector2.y);
        if (HandleUtility.s_UseYSignZoom)
          return Mathf.Sign(vector2.y) * vector2.magnitude * HandleUtility.acceleration;
        return Mathf.Sign(vector2.x) * vector2.magnitude * HandleUtility.acceleration;
      }
    }

    public static int nearestControl
    {
      get
      {
        if ((double) HandleUtility.s_NearestDistance <= 5.0)
          return HandleUtility.s_NearestControl;
        return 0;
      }
      set
      {
        HandleUtility.s_NearestControl = value;
      }
    }

    public static Material handleMaterial
    {
      get
      {
        if (!(bool) ((Object) HandleUtility.s_HandleMaterial))
          HandleUtility.s_HandleMaterial = (Material) EditorGUIUtility.Load("SceneView/Handles.mat");
        return HandleUtility.s_HandleMaterial;
      }
    }

    private static Material handleWireMaterial
    {
      get
      {
        HandleUtility.InitHandleMaterials();
        if ((bool) ((Object) Camera.current))
          return HandleUtility.s_HandleWireMaterial;
        return HandleUtility.s_HandleWireMaterial2D;
      }
    }

    private static Material handleDottedWireMaterial
    {
      get
      {
        HandleUtility.InitHandleMaterials();
        if ((bool) ((Object) Camera.current))
          return HandleUtility.s_HandleDottedWireMaterial;
        return HandleUtility.s_HandleDottedWireMaterial2D;
      }
    }

    /// <summary>
    ///   <para>Map a mouse drag onto a movement along a line in 3D space.</para>
    /// </summary>
    /// <param name="src">The source point of the drag.</param>
    /// <param name="dest">The destination point of the drag.</param>
    /// <param name="srcPosition">The 3D position the dragged object had at src ray.</param>
    /// <param name="constraintDir">3D direction of constrained movement.</param>
    /// <returns>
    ///   <para>The distance travelled along constraintDir.</para>
    /// </returns>
    public static float CalcLineTranslation(Vector2 src, Vector2 dest, Vector3 srcPosition, Vector3 constraintDir)
    {
      srcPosition = Handles.matrix.MultiplyPoint(srcPosition);
      constraintDir = Handles.matrix.MultiplyVector(constraintDir);
      float num = 1f;
      Vector3 forward = Camera.current.transform.forward;
      if ((double) Vector3.Dot(constraintDir, forward) < 0.0)
        num = -1f;
      Vector3 vector3 = constraintDir;
      vector3.y = -vector3.y;
      Camera current = Camera.current;
      Vector2 points1 = EditorGUIUtility.PixelsToPoints((Vector2) current.WorldToScreenPoint(srcPosition));
      Vector2 points2 = EditorGUIUtility.PixelsToPoints((Vector2) current.WorldToScreenPoint(srcPosition + constraintDir * num));
      Vector2 x0_1 = dest;
      Vector2 x0_2 = src;
      if (points1 == points2)
        return 0.0f;
      x0_1.y = -x0_1.y;
      x0_2.y = -x0_2.y;
      float parametrization = HandleUtility.GetParametrization(x0_2, points1, points2);
      return (HandleUtility.GetParametrization(x0_1, points1, points2) - parametrization) * num;
    }

    internal static float GetParametrization(Vector2 x0, Vector2 x1, Vector2 x2)
    {
      return (float) -((double) Vector2.Dot(x1 - x0, x2 - x1) / (double) (x2 - x1).sqrMagnitude);
    }

    /// <summary>
    ///   <para>Returns the parameter for the projection of the point on the given line.</para>
    /// </summary>
    /// <param name="point"></param>
    /// <param name="linePoint"></param>
    /// <param name="lineDirection"></param>
    public static float PointOnLineParameter(Vector3 point, Vector3 linePoint, Vector3 lineDirection)
    {
      return Vector3.Dot(lineDirection, point - linePoint) / lineDirection.sqrMagnitude;
    }

    /// <summary>
    ///   <para>Project point onto a line.</para>
    /// </summary>
    /// <param name="point"></param>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
      Vector3 rhs = point - lineStart;
      Vector3 vector3 = lineEnd - lineStart;
      float magnitude = vector3.magnitude;
      Vector3 lhs = vector3;
      if ((double) magnitude > 9.99999997475243E-07)
        lhs /= magnitude;
      float num = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0.0f, magnitude);
      return lineStart + lhs * num;
    }

    /// <summary>
    ///   <para>Calculate distance between a point and a line.</para>
    /// </summary>
    /// <param name="point"></param>
    /// <param name="lineStart"></param>
    /// <param name="lineEnd"></param>
    public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
      return Vector3.Magnitude(HandleUtility.ProjectPointLine(point, lineStart, lineEnd) - point);
    }

    /// <summary>
    ///   <para>Calculate distance between a point and a Bezier curve.</para>
    /// </summary>
    /// <param name="point"></param>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    /// <param name="startTangent"></param>
    /// <param name="endTangent"></param>
    public static float DistancePointBezier(Vector3 point, Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent)
    {
      return HandleUtility.INTERNAL_CALL_DistancePointBezier(ref point, ref startPosition, ref endPosition, ref startTangent, ref endTangent);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float INTERNAL_CALL_DistancePointBezier(ref Vector3 point, ref Vector3 startPosition, ref Vector3 endPosition, ref Vector3 startTangent, ref Vector3 endTangent);

    /// <summary>
    ///   <para>Pixel distance from mouse pointer to line.</para>
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public static float DistanceToLine(Vector3 p1, Vector3 p2)
    {
      p1 = (Vector3) HandleUtility.WorldToGUIPoint(p1);
      p2 = (Vector3) HandleUtility.WorldToGUIPoint(p2);
      float num = HandleUtility.DistancePointLine((Vector3) Event.current.mousePosition, p1, p2);
      if ((double) num < 0.0)
        num = 0.0f;
      return num;
    }

    /// <summary>
    ///   <para>Pixel distance from mouse pointer to camera facing circle.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    public static float DistanceToCircle(Vector3 position, float radius)
    {
      Vector2 guiPoint1 = HandleUtility.WorldToGUIPoint(position);
      Camera current = Camera.current;
      Vector2 zero = Vector2.zero;
      if ((bool) ((Object) current))
      {
        Vector2 guiPoint2 = HandleUtility.WorldToGUIPoint(position + current.transform.right * radius);
        radius = (guiPoint1 - guiPoint2).magnitude;
      }
      float magnitude = (guiPoint1 - Event.current.mousePosition).magnitude;
      if ((double) magnitude < (double) radius)
        return 0.0f;
      return magnitude - radius;
    }

    /// <summary>
    ///   <para>Pixel distance from mouse pointer to a rectangle on screen.</para>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="size"></param>
    public static float DistanceToRectangle(Vector3 position, Quaternion rotation, float size)
    {
      Vector3 vector3_1 = rotation * new Vector3(size, 0.0f, 0.0f);
      Vector3 vector3_2 = rotation * new Vector3(0.0f, size, 0.0f);
      HandleUtility.points[0] = (Vector3) HandleUtility.WorldToGUIPoint(position + vector3_1 + vector3_2);
      HandleUtility.points[1] = (Vector3) HandleUtility.WorldToGUIPoint(position + vector3_1 - vector3_2);
      HandleUtility.points[2] = (Vector3) HandleUtility.WorldToGUIPoint(position - vector3_1 - vector3_2);
      HandleUtility.points[3] = (Vector3) HandleUtility.WorldToGUIPoint(position - vector3_1 + vector3_2);
      HandleUtility.points[4] = HandleUtility.points[0];
      Vector2 mousePosition = Event.current.mousePosition;
      bool flag = false;
      int index1 = 4;
      for (int index2 = 0; index2 < 5; ++index2)
      {
        if ((double) HandleUtility.points[index2].y > (double) mousePosition.y != (double) HandleUtility.points[index1].y > (double) mousePosition.y && (double) mousePosition.x < ((double) HandleUtility.points[index1].x - (double) HandleUtility.points[index2].x) * ((double) mousePosition.y - (double) HandleUtility.points[index2].y) / ((double) HandleUtility.points[index1].y - (double) HandleUtility.points[index2].y) + (double) HandleUtility.points[index2].x)
          flag = !flag;
        index1 = index2;
      }
      if (flag)
        return 0.0f;
      float num1 = -1f;
      int num2 = 1;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        float lineSegment = HandleUtility.DistancePointToLineSegment(mousePosition, (Vector2) HandleUtility.points[index2], (Vector2) HandleUtility.points[num2++]);
        if ((double) lineSegment < (double) num1 || (double) num1 < 0.0)
          num1 = lineSegment;
      }
      return num1;
    }

    /// <summary>
    ///   <para>Distance from a point p in 2d to a line defined by two points a and b.</para>
    /// </summary>
    /// <param name="p"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static float DistancePointToLine(Vector2 p, Vector2 a, Vector2 b)
    {
      return Mathf.Abs((float) (((double) b.x - (double) a.x) * ((double) a.y - (double) p.y) - ((double) a.x - (double) p.x) * ((double) b.y - (double) a.y))) / (b - a).magnitude;
    }

    /// <summary>
    ///   <para>Distance from a point p in 2d to a line segment defined by two points a and b.</para>
    /// </summary>
    /// <param name="p"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static float DistancePointToLineSegment(Vector2 p, Vector2 a, Vector2 b)
    {
      float sqrMagnitude = (b - a).sqrMagnitude;
      if ((double) sqrMagnitude == 0.0)
        return (p - a).magnitude;
      float num = Vector2.Dot(p - a, b - a) / sqrMagnitude;
      if ((double) num < 0.0)
        return (p - a).magnitude;
      if ((double) num > 1.0)
        return (p - b).magnitude;
      Vector2 vector2 = a + num * (b - a);
      return (p - vector2).magnitude;
    }

    /// <summary>
    ///   <para>Pixel distance from mouse pointer to a 3D disc.</para>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="normal"></param>
    /// <param name="radius"></param>
    public static float DistanceToDisc(Vector3 center, Vector3 normal, float radius)
    {
      Vector3 from = Vector3.Cross(normal, Vector3.up);
      if ((double) from.sqrMagnitude < 1.0 / 1000.0)
        from = Vector3.Cross(normal, Vector3.right);
      return HandleUtility.DistanceToArc(center, normal, from, 360f, radius);
    }

    /// <summary>
    ///   <para>Get the point on an disc (in 3D space) which is closest to the current mouse position.</para>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="normal"></param>
    /// <param name="radius"></param>
    public static Vector3 ClosestPointToDisc(Vector3 center, Vector3 normal, float radius)
    {
      Vector3 from = Vector3.Cross(normal, Vector3.up);
      if ((double) from.sqrMagnitude < 1.0 / 1000.0)
        from = Vector3.Cross(normal, Vector3.right);
      return HandleUtility.ClosestPointToArc(center, normal, from, 360f, radius);
    }

    /// <summary>
    ///   <para>Pixel distance from mouse pointer to a 3D section of a disc.</para>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="normal"></param>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    /// <param name="radius"></param>
    public static float DistanceToArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
      Vector3[] dest = new Vector3[60];
      Handles.SetDiscSectionPoints(dest, 60, center, normal, from, angle, radius);
      return HandleUtility.DistanceToPolyLine(dest);
    }

    /// <summary>
    ///   <para>Get the point on an arc (in 3D space) which is closest to the current mouse position.</para>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="normal"></param>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    /// <param name="radius"></param>
    public static Vector3 ClosestPointToArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
      Vector3[] dest = new Vector3[60];
      Handles.SetDiscSectionPoints(dest, 60, center, normal, from, angle, radius);
      return HandleUtility.ClosestPointToPolyLine(dest);
    }

    /// <summary>
    ///   <para>Pixel distance from mouse pointer to a polyline.</para>
    /// </summary>
    /// <param name="points"></param>
    public static float DistanceToPolyLine(params Vector3[] points)
    {
      float num = HandleUtility.DistanceToLine(points[0], points[1]);
      for (int index = 2; index < points.Length; ++index)
      {
        float line = HandleUtility.DistanceToLine(points[index - 1], points[index]);
        if ((double) line < (double) num)
          num = line;
      }
      return num;
    }

    /// <summary>
    ///   <para>Get the point on a polyline (in 3D space) which is closest to the current mouse position.</para>
    /// </summary>
    /// <param name="vertices"></param>
    public static Vector3 ClosestPointToPolyLine(params Vector3[] vertices)
    {
      float num1 = HandleUtility.DistanceToLine(vertices[0], vertices[1]);
      int index1 = 0;
      for (int index2 = 2; index2 < vertices.Length; ++index2)
      {
        float line = HandleUtility.DistanceToLine(vertices[index2 - 1], vertices[index2]);
        if ((double) line < (double) num1)
        {
          num1 = line;
          index1 = index2 - 1;
        }
      }
      Vector3 vertex1 = vertices[index1];
      Vector3 vertex2 = vertices[index1 + 1];
      Vector2 vector2_1 = Event.current.mousePosition - HandleUtility.WorldToGUIPoint(vertex1);
      Vector2 vector2_2 = HandleUtility.WorldToGUIPoint(vertex2) - HandleUtility.WorldToGUIPoint(vertex1);
      float magnitude = vector2_2.magnitude;
      float num2 = Vector3.Dot((Vector3) vector2_2, (Vector3) vector2_1);
      if ((double) magnitude > 9.99999997475243E-07)
        num2 /= magnitude * magnitude;
      float t = Mathf.Clamp01(num2);
      return Vector3.Lerp(vertex1, vertex2, t);
    }

    /// <summary>
    ///   <para>Record a distance measurement from a handle.</para>
    /// </summary>
    /// <param name="controlId"></param>
    /// <param name="distance"></param>
    public static void AddControl(int controlId, float distance)
    {
      if ((double) distance < (double) HandleUtility.s_CustomPickDistance && (double) distance > 5.0)
        distance = 5f;
      if ((double) distance > (double) HandleUtility.s_NearestDistance)
        return;
      HandleUtility.s_NearestDistance = distance;
      HandleUtility.s_NearestControl = controlId;
    }

    /// <summary>
    ///   <para>Add the ID for a default control. This will be picked if nothing else is.</para>
    /// </summary>
    /// <param name="controlId"></param>
    public static void AddDefaultControl(int controlId)
    {
      HandleUtility.AddControl(controlId, 5f);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CameraNeedsToRenderIntoRT(Camera camera);

    [RequiredByNativeCode]
    private static void BeginHandles()
    {
      Handles.Init();
      if (Event.current.type == EventType.Layout)
      {
        HandleUtility.s_NearestControl = 0;
        HandleUtility.s_NearestDistance = 5f;
      }
      Handles.lighting = true;
      Handles.color = Color.white;
      HandleUtility.s_CustomPickDistance = 5f;
      Handles.Internal_SetCurrentCamera((Camera) null);
      EditorGUI.s_DelayedTextEditor.BeginGUI();
    }

    [RequiredByNativeCode]
    private static void SetViewInfo(Vector2 screenPosition)
    {
      GUIUtility.s_EditorScreenPointOffset = screenPosition;
    }

    [RequiredByNativeCode]
    private static void EndHandles()
    {
      EventType type = Event.current.type;
      if (type != EventType.Layout)
      {
        GUIUtility.s_HasKeyboardFocus = false;
        GUIUtility.s_EditorScreenPointOffset = Vector2.zero;
      }
      EditorGUI.s_DelayedTextEditor.EndGUI(type);
    }

    /// <summary>
    ///   <para>Get world space size of a manipulator handle at given position.</para>
    /// </summary>
    /// <param name="position"></param>
    public static float GetHandleSize(Vector3 position)
    {
      Camera current = Camera.current;
      position = Handles.matrix.MultiplyPoint(position);
      if (!(bool) ((Object) current))
        return 20f;
      Transform transform = current.transform;
      Vector3 position1 = transform.position;
      float z = Vector3.Dot(position - position1, transform.TransformDirection(new Vector3(0.0f, 0.0f, 1f)));
      return 80f / Mathf.Max((current.WorldToScreenPoint(position1 + transform.TransformDirection(new Vector3(0.0f, 0.0f, z))) - current.WorldToScreenPoint(position1 + transform.TransformDirection(new Vector3(1f, 0.0f, z)))).magnitude, 0.0001f) * EditorGUIUtility.pixelsPerPoint;
    }

    /// <summary>
    ///   <para>Convert world space point to a 2D GUI position.</para>
    /// </summary>
    /// <param name="world">Point in world space.</param>
    public static Vector2 WorldToGUIPoint(Vector3 world)
    {
      world = Handles.matrix.MultiplyPoint(world);
      Camera current = Camera.current;
      if (!(bool) ((Object) current))
        return new Vector2(world.x, world.y);
      Vector2 vector2 = (Vector2) current.WorldToScreenPoint(world);
      vector2.y = (float) Screen.height - vector2.y;
      vector2 = EditorGUIUtility.PixelsToPoints(vector2);
      return GUIClip.Clip(vector2);
    }

    /// <summary>
    ///   <para>Convert 2D GUI position to a world space ray.</para>
    /// </summary>
    /// <param name="position"></param>
    public static Ray GUIPointToWorldRay(Vector2 position)
    {
      if (!(bool) ((Object) Camera.current))
      {
        Debug.LogError((object) "Unable to convert GUI point to world ray if a camera has not been set up!");
        return new Ray(Vector3.zero, Vector3.forward);
      }
      Vector2 pixels = EditorGUIUtility.PointsToPixels(GUIClip.Unclip(position));
      pixels.y = (float) Screen.height - pixels.y;
      return Camera.current.ScreenPointToRay((Vector3) new Vector2(pixels.x, pixels.y));
    }

    /// <summary>
    ///   <para>Calculate a rectangle to display a 2D GUI element near a projected point in 3D space.</para>
    /// </summary>
    /// <param name="position">The world-space position to use.</param>
    /// <param name="content">The content to make room for.</param>
    /// <param name="style">The style to use. The style's alignment.</param>
    public static Rect WorldPointToSizedRect(Vector3 position, GUIContent content, GUIStyle style)
    {
      Vector2 guiPoint = HandleUtility.WorldToGUIPoint(position);
      Vector2 vector2 = style.CalcSize(content);
      Rect rect = new Rect(guiPoint.x, guiPoint.y, vector2.x, vector2.y);
      switch (style.alignment)
      {
        case TextAnchor.UpperCenter:
          rect.xMin -= rect.width * 0.5f;
          break;
        case TextAnchor.UpperRight:
          rect.xMin -= rect.width;
          break;
        case TextAnchor.MiddleLeft:
          rect.yMin -= rect.height * 0.5f;
          break;
        case TextAnchor.MiddleCenter:
          rect.xMin -= rect.width * 0.5f;
          rect.yMin -= rect.height * 0.5f;
          break;
        case TextAnchor.MiddleRight:
          rect.xMin -= rect.width;
          rect.yMin -= rect.height * 0.5f;
          break;
        case TextAnchor.LowerLeft:
          rect.yMin -= rect.height * 0.5f;
          break;
        case TextAnchor.LowerCenter:
          rect.xMin -= rect.width * 0.5f;
          rect.yMin -= rect.height;
          break;
        case TextAnchor.LowerRight:
          rect.xMin -= rect.width;
          rect.yMin -= rect.height;
          break;
      }
      return style.padding.Add(rect);
    }

    /// <summary>
    ///   <para>Pick GameObjects that lie within a specified screen rectangle.</para>
    /// </summary>
    /// <param name="rect">An screen rectangle specified with pixel coordinates.</param>
    public static GameObject[] PickRectObjects(Rect rect)
    {
      return HandleUtility.PickRectObjects(rect, true);
    }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="selectPrefabRootsOnly"></param>
    public static GameObject[] PickRectObjects(Rect rect, bool selectPrefabRootsOnly)
    {
      Camera current = Camera.current;
      rect = EditorGUIUtility.PointsToPixels(rect);
      rect.x /= (float) current.pixelWidth;
      rect.width /= (float) current.pixelWidth;
      rect.y /= (float) current.pixelHeight;
      rect.height /= (float) current.pixelHeight;
      return HandleUtility.Internal_PickRectObjects(current, rect, selectPrefabRootsOnly);
    }

    internal static GameObject[] Internal_PickRectObjects(Camera cam, Rect rect, bool selectPrefabRoots)
    {
      return HandleUtility.INTERNAL_CALL_Internal_PickRectObjects(cam, ref rect, selectPrefabRoots);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern GameObject[] INTERNAL_CALL_Internal_PickRectObjects(Camera cam, ref Rect rect, bool selectPrefabRoots);

    internal static bool FindNearestVertex(Vector2 screenPoint, Transform[] objectsToSearch, out Vector3 vertex)
    {
      Camera current = Camera.current;
      screenPoint.y = current.pixelRect.yMax - screenPoint.y;
      return HandleUtility.Internal_FindNearestVertex(current, screenPoint, objectsToSearch, HandleUtility.ignoreRaySnapObjects, out vertex);
    }

    private static bool Internal_FindNearestVertex(Camera cam, Vector2 point, Transform[] objectsToSearch, Transform[] ignoreObjects, out Vector3 vertex)
    {
      return HandleUtility.INTERNAL_CALL_Internal_FindNearestVertex(cam, ref point, objectsToSearch, ignoreObjects, out vertex);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_Internal_FindNearestVertex(Camera cam, ref Vector2 point, Transform[] objectsToSearch, Transform[] ignoreObjects, out Vector3 vertex);

    public static GameObject PickGameObject(Vector2 position, out int materialIndex)
    {
      return HandleUtility.PickGameObject(position, (GameObject[]) null, out materialIndex);
    }

    public static GameObject PickGameObject(Vector2 position, GameObject[] ignore, out int materialIndex)
    {
      return HandleUtility.PickGameObject(position, ignore, (GameObject[]) null, out materialIndex);
    }

    internal static GameObject PickGameObject(Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex)
    {
      Camera current = Camera.current;
      int cullingMask = current.cullingMask;
      position = GUIClip.Unclip(position);
      position = EditorGUIUtility.PointsToPixels(position);
      position.y = (float) Screen.height - position.y - current.pixelRect.yMin;
      return HandleUtility.Internal_PickClosestGO(current, cullingMask, position, ignore, filter, out materialIndex);
    }

    /// <summary>
    ///   <para>Pick game object closest to specified position.</para>
    /// </summary>
    /// <param name="selectPrefabRoot">Select prefab.</param>
    /// <param name="materialIndex">Returns index into material array of the Renderer component that is closest to specified position.</param>
    /// <param name="position"></param>
    public static GameObject PickGameObject(Vector2 position, bool selectPrefabRoot)
    {
      return HandleUtility.PickGameObject(position, selectPrefabRoot, (GameObject[]) null);
    }

    public static GameObject PickGameObject(Vector2 position, bool selectPrefabRoot, GameObject[] ignore)
    {
      return HandleUtility.PickGameObject(position, selectPrefabRoot, ignore, (GameObject[]) null);
    }

    internal static GameObject PickGameObject(Vector2 position, bool selectPrefabRoot, GameObject[] ignore, GameObject[] filter)
    {
      int materialIndex;
      GameObject go = HandleUtility.PickGameObject(position, ignore, filter, out materialIndex);
      if (!(bool) ((Object) go) || !selectPrefabRoot)
        return go;
      GameObject gameObject1 = HandleUtility.FindSelectionBase(go) ?? go;
      Transform activeTransform = Selection.activeTransform;
      GameObject gameObject2 = !(bool) ((Object) activeTransform) ? (GameObject) null : HandleUtility.FindSelectionBase(activeTransform.gameObject) ?? activeTransform.gameObject;
      if ((Object) gameObject1 == (Object) gameObject2)
        return go;
      return gameObject1;
    }

    internal static GameObject FindSelectionBase(GameObject go)
    {
      if ((Object) go == (Object) null)
        return (GameObject) null;
      Transform transform1 = (Transform) null;
      switch (PrefabUtility.GetPrefabType((Object) go))
      {
        case PrefabType.PrefabInstance:
        case PrefabType.ModelPrefabInstance:
          transform1 = PrefabUtility.FindPrefabRoot(go).transform;
          break;
      }
      for (Transform transform2 = go.transform; (Object) transform2 != (Object) null; transform2 = transform2.parent)
      {
        if ((Object) transform2 == (Object) transform1)
          return transform2.gameObject;
        if (AttributeHelper.GameObjectContainsAttribute(transform2.gameObject, typeof (SelectionBaseAttribute)))
          return transform2.gameObject;
      }
      return (GameObject) null;
    }

    internal static GameObject Internal_PickClosestGO(Camera cam, int layers, Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex)
    {
      return HandleUtility.INTERNAL_CALL_Internal_PickClosestGO(cam, layers, ref position, ignore, filter, out materialIndex);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern GameObject INTERNAL_CALL_Internal_PickClosestGO(Camera cam, int layers, ref Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex);

    private static void InitHandleMaterials()
    {
      if ((bool) ((Object) HandleUtility.s_HandleWireMaterial))
        return;
      HandleUtility.s_HandleWireMaterial = (Material) EditorGUIUtility.LoadRequired("SceneView/HandleLines.mat");
      HandleUtility.s_HandleWireMaterial2D = (Material) EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
      HandleUtility.s_HandleWireTextureIndex = ShaderUtil.GetTextureBindingIndex(HandleUtility.s_HandleWireMaterial.shader, Shader.PropertyToID("_MainTex"));
      HandleUtility.s_HandleWireTextureIndex2D = ShaderUtil.GetTextureBindingIndex(HandleUtility.s_HandleWireMaterial2D.shader, Shader.PropertyToID("_MainTex"));
      HandleUtility.s_HandleDottedWireMaterial = (Material) EditorGUIUtility.LoadRequired("SceneView/HandleDottedLines.mat");
      HandleUtility.s_HandleDottedWireMaterial2D = (Material) EditorGUIUtility.LoadRequired("SceneView/2DHandleDottedLines.mat");
      HandleUtility.s_HandleDottedWireTextureIndex = ShaderUtil.GetTextureBindingIndex(HandleUtility.s_HandleDottedWireMaterial.shader, Shader.PropertyToID("_MainTex"));
      HandleUtility.s_HandleDottedWireTextureIndex2D = ShaderUtil.GetTextureBindingIndex(HandleUtility.s_HandleDottedWireMaterial2D.shader, Shader.PropertyToID("_MainTex"));
    }

    internal static void ApplyWireMaterial()
    {
      HandleUtility.handleWireMaterial.SetPass(0);
      HandleUtility.Internal_SetHandleWireTextureIndex(!(bool) ((Object) Camera.current) ? HandleUtility.s_HandleWireTextureIndex2D : HandleUtility.s_HandleWireTextureIndex);
    }

    internal static void ApplyDottedWireMaterial()
    {
      HandleUtility.handleDottedWireMaterial.SetPass(0);
      HandleUtility.Internal_SetHandleWireTextureIndex(!(bool) ((Object) Camera.current) ? HandleUtility.s_HandleDottedWireTextureIndex2D : HandleUtility.s_HandleDottedWireTextureIndex);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_SetHandleWireTextureIndex(int textureIndex);

    /// <summary>
    ///   <para>Store all camera settings.</para>
    /// </summary>
    /// <param name="camera"></param>
    public static void PushCamera(Camera camera)
    {
      HandleUtility.s_SavedCameras.Push((object) new HandleUtility.SavedCamera(camera));
    }

    /// <summary>
    ///   <para>Retrieve all camera settings.</para>
    /// </summary>
    /// <param name="camera"></param>
    public static void PopCamera(Camera camera)
    {
      ((HandleUtility.SavedCamera) HandleUtility.s_SavedCameras.Pop()).Restore(camera);
    }

    /// <summary>
    ///   <para>Casts ray against the scene and report if an object lies in its path.</para>
    /// </summary>
    /// <param name="ray"></param>
    /// <returns>
    ///   <para>A boxed RaycastHit, null if nothing hit it.</para>
    /// </returns>
    public static object RaySnap(Ray ray)
    {
      RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, float.PositiveInfinity, Camera.current.cullingMask);
      float num = float.PositiveInfinity;
      int index1 = -1;
      if (HandleUtility.ignoreRaySnapObjects != null)
      {
        for (int index2 = 0; index2 < raycastHitArray.Length; ++index2)
        {
          if (!raycastHitArray[index2].collider.isTrigger && (double) raycastHitArray[index2].distance < (double) num)
          {
            bool flag = false;
            for (int index3 = 0; index3 < HandleUtility.ignoreRaySnapObjects.Length; ++index3)
            {
              if ((Object) raycastHitArray[index2].transform == (Object) HandleUtility.ignoreRaySnapObjects[index3])
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              num = raycastHitArray[index2].distance;
              index1 = index2;
            }
          }
        }
      }
      else
      {
        for (int index2 = 0; index2 < raycastHitArray.Length; ++index2)
        {
          if ((double) raycastHitArray[index2].distance < (double) num)
          {
            num = raycastHitArray[index2].distance;
            index1 = index2;
          }
        }
      }
      if (index1 >= 0)
        return (object) raycastHitArray[index1];
      return (object) null;
    }

    internal static float CalcRayPlaceOffset(Transform[] objects, Vector3 normal)
    {
      return HandleUtility.INTERNAL_CALL_CalcRayPlaceOffset(objects, ref normal);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern float INTERNAL_CALL_CalcRayPlaceOffset(Transform[] objects, ref Vector3 normal);

    /// <summary>
    ///   <para>Repaint the current view.</para>
    /// </summary>
    public static void Repaint()
    {
      HandleUtility.Internal_Repaint();
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Repaint();

    internal static bool IntersectRayMesh(Ray ray, Mesh mesh, Matrix4x4 matrix, out RaycastHit hit)
    {
      return HandleUtility.INTERNAL_CALL_IntersectRayMesh(ref ray, mesh, ref matrix, out hit);
    }

    [WrapperlessIcall]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_IntersectRayMesh(ref Ray ray, Mesh mesh, ref Matrix4x4 matrix, out RaycastHit hit);

    private sealed class SavedCamera
    {
      private float near;
      private float far;
      private Rect pixelRect;
      private Vector3 pos;
      private Quaternion rot;
      private CameraClearFlags clearFlags;
      private int cullingMask;
      private float fov;
      private float orthographicSize;
      private bool isOrtho;

      internal SavedCamera(Camera source)
      {
        this.near = source.nearClipPlane;
        this.far = source.farClipPlane;
        this.pixelRect = source.pixelRect;
        this.pos = source.transform.position;
        this.rot = source.transform.rotation;
        this.clearFlags = source.clearFlags;
        this.cullingMask = source.cullingMask;
        this.fov = source.fieldOfView;
        this.orthographicSize = source.orthographicSize;
        this.isOrtho = source.orthographic;
      }

      internal void Restore(Camera dest)
      {
        dest.nearClipPlane = this.near;
        dest.farClipPlane = this.far;
        dest.pixelRect = this.pixelRect;
        dest.transform.position = this.pos;
        dest.transform.rotation = this.rot;
        dest.clearFlags = this.clearFlags;
        dest.fieldOfView = this.fov;
        dest.orthographicSize = this.orthographicSize;
        dest.orthographic = this.isOrtho;
        dest.cullingMask = this.cullingMask;
      }
    }
  }
}
