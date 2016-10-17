// Decompiled with JetBrains decompiler
// Type: UnityEditor.RectHandles
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class RectHandles
  {
    private static int s_LastCursorId = 0;
    private static Vector3[] s_TempVectors = new Vector3[0];
    private static RectHandles.Styles s_Styles;
    private static Vector2 s_StartMousePosition;
    private static Vector2 s_CurrentMousePosition;
    private static Vector3 s_StartPosition;
    private static float s_StartRotation;
    private static float s_RotationDist;

    internal static bool RaycastGUIPointToWorldHit(Vector2 guiPoint, Plane plane, out Vector3 hit)
    {
      Ray worldRay = HandleUtility.GUIPointToWorldRay(guiPoint);
      float enter = 0.0f;
      bool flag = plane.Raycast(worldRay, out enter);
      hit = !flag ? Vector3.zero : worldRay.GetPoint(enter);
      return flag;
    }

    internal static void DetectCursorChange(int id)
    {
      if (HandleUtility.nearestControl == id)
      {
        RectHandles.s_LastCursorId = id;
        Event.current.Use();
      }
      else
      {
        if (RectHandles.s_LastCursorId != id)
          return;
        RectHandles.s_LastCursorId = 0;
        Event.current.Use();
      }
    }

    internal static Vector3 SideSlider(int id, Vector3 position, Vector3 sideVector, Vector3 direction, float size, Handles.DrawCapFunction drawFunc, float snap)
    {
      return RectHandles.SideSlider(id, position, sideVector, direction, size, drawFunc, snap, 0.0f);
    }

    internal static Vector3 SideSlider(int id, Vector3 position, Vector3 sideVector, Vector3 direction, float size, Handles.DrawCapFunction drawFunc, float snap, float bias)
    {
      Event current = Event.current;
      Vector3 normalized1 = Vector3.Cross(sideVector, direction).normalized;
      Vector3 vector3_1 = Handles.Slider2D(id, position, normalized1, direction, sideVector, 0.0f, drawFunc, Vector2.one * snap);
      Vector3 vector3_2 = position + Vector3.Project(vector3_1 - position, direction);
      switch (current.type)
      {
        case EventType.Repaint:
          if (HandleUtility.nearestControl == id && GUIUtility.hotControl == 0 || GUIUtility.hotControl == id)
          {
            RectHandles.HandleDirectionalCursor(position, normalized1, direction);
            break;
          }
          break;
        case EventType.Layout:
          Vector3 normalized2 = sideVector.normalized;
          HandleUtility.AddControl(id, HandleUtility.DistanceToLine(position + sideVector * 0.5f - normalized2 * size * 2f, position - sideVector * 0.5f + normalized2 * size * 2f) - bias);
          break;
        case EventType.MouseMove:
          RectHandles.DetectCursorChange(id);
          break;
      }
      return vector3_2;
    }

    internal static Vector3 CornerSlider(int id, Vector3 cornerPos, Vector3 handleDir, Vector3 outwardsDir1, Vector3 outwardsDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap)
    {
      Event current = Event.current;
      Vector3 vector3 = Handles.Slider2D(id, cornerPos, handleDir, outwardsDir1, outwardsDir2, handleSize, drawFunc, snap);
      switch (current.type)
      {
        case EventType.MouseMove:
          RectHandles.DetectCursorChange(id);
          break;
        case EventType.Repaint:
          if (HandleUtility.nearestControl == id && GUIUtility.hotControl == 0 || GUIUtility.hotControl == id)
          {
            RectHandles.HandleDirectionalCursor(cornerPos, handleDir, outwardsDir1 + outwardsDir2);
            break;
          }
          break;
      }
      return vector3;
    }

    private static void HandleDirectionalCursor(Vector3 handlePosition, Vector3 handlePlaneNormal, Vector3 direction)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      Plane plane = new Plane(handlePlaneNormal, handlePosition);
      Vector3 hit;
      if (!RectHandles.RaycastGUIPointToWorldHit(mousePosition, plane, out hit))
        return;
      Vector2 screenSpaceDir = RectHandles.WorldToScreenSpaceDir(hit, direction);
      EditorGUIUtility.AddCursorRect(new Rect(mousePosition.x - 100f, mousePosition.y - 100f, 200f, 200f), RectHandles.GetScaleCursor(screenSpaceDir));
    }

    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
      dirA = Vector3.ProjectOnPlane(dirA, axis);
      dirB = Vector3.ProjectOnPlane(dirB, axis);
      return Vector3.Angle(dirA, dirB) * ((double) Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0.0 ? 1f : -1f);
    }

    public static float RotationSlider(int id, Vector3 cornerPos, float rotation, Vector3 pivot, Vector3 handleDir, Vector3 outwardsDir1, Vector3 outwardsDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap)
    {
      Vector3 vector3_1 = outwardsDir1 + outwardsDir2;
      Vector2 guiPoint = HandleUtility.WorldToGUIPoint(cornerPos);
      Vector2 vector2 = HandleUtility.WorldToGUIPoint(cornerPos + vector3_1) - guiPoint;
      vector2 = vector2.normalized * 15f;
      RectHandles.RaycastGUIPointToWorldHit(guiPoint + vector2, new Plane(handleDir, cornerPos), out cornerPos);
      Event current = Event.current;
      Vector3 vector3_2 = Handles.Slider2D(id, cornerPos, handleDir, outwardsDir1, outwardsDir2, handleSize, drawFunc, Vector2.zero);
      if (current.type == EventType.MouseMove)
        RectHandles.DetectCursorChange(id);
      if (current.type == EventType.Repaint && (HandleUtility.nearestControl == id && GUIUtility.hotControl == 0 || GUIUtility.hotControl == id))
        EditorGUIUtility.AddCursorRect(new Rect(current.mousePosition.x - 100f, current.mousePosition.y - 100f, 200f, 200f), MouseCursor.RotateArrow);
      return rotation - RectHandles.AngleAroundAxis(vector3_2 - pivot, cornerPos - pivot, handleDir);
    }

    private static Vector2 WorldToScreenSpaceDir(Vector3 worldPos, Vector3 worldDir)
    {
      Vector3 guiPoint = (Vector3) HandleUtility.WorldToGUIPoint(worldPos);
      Vector2 vector2 = (Vector2) ((Vector3) HandleUtility.WorldToGUIPoint(worldPos + worldDir) - guiPoint);
      vector2.y *= -1f;
      return vector2;
    }

    private static MouseCursor GetScaleCursor(Vector2 direction)
    {
      float num = Mathf.Atan2(direction.x, direction.y) * 57.29578f;
      if ((double) num < 0.0)
        num = 360f + num;
      if ((double) num < 27.5)
        return MouseCursor.ResizeVertical;
      if ((double) num < 72.5)
        return MouseCursor.ResizeUpRight;
      if ((double) num < 117.5)
        return MouseCursor.ResizeHorizontal;
      if ((double) num < 162.5)
        return MouseCursor.ResizeUpLeft;
      if ((double) num < 207.5)
        return MouseCursor.ResizeVertical;
      if ((double) num < 252.5)
        return MouseCursor.ResizeUpRight;
      if ((double) num < 297.5)
        return MouseCursor.ResizeHorizontal;
      return (double) num < 342.5 ? MouseCursor.ResizeUpLeft : MouseCursor.ResizeVertical;
    }

    public static void RectScalingCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (RectHandles.s_Styles == null)
        RectHandles.s_Styles = new RectHandles.Styles();
      RectHandles.DrawImageBasedCap(controlID, position, rotation, size, RectHandles.s_Styles.dragdot, RectHandles.s_Styles.dragdotactive);
    }

    public static void PivotCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
      if (RectHandles.s_Styles == null)
        RectHandles.s_Styles = new RectHandles.Styles();
      RectHandles.DrawImageBasedCap(controlID, position, rotation, size, RectHandles.s_Styles.pivotdot, RectHandles.s_Styles.pivotdotactive);
    }

    private static void DrawImageBasedCap(int controlID, Vector3 position, Quaternion rotation, float size, GUIStyle normal, GUIStyle active)
    {
      if ((bool) ((Object) Camera.current) && (double) Vector3.Dot(position - Camera.current.transform.position, Camera.current.transform.forward) < 0.0)
        return;
      Vector3 guiPoint = (Vector3) HandleUtility.WorldToGUIPoint(position);
      Handles.BeginGUI();
      float fixedWidth = normal.fixedWidth;
      float fixedHeight = normal.fixedHeight;
      Rect position1 = new Rect(guiPoint.x - fixedWidth / 2f, guiPoint.y - fixedHeight / 2f, fixedWidth, fixedHeight);
      if (GUIUtility.hotControl == controlID)
        active.Draw(position1, GUIContent.none, controlID);
      else
        normal.Draw(position1, GUIContent.none, controlID);
      Handles.EndGUI();
    }

    public static void RenderRectWithShadow(bool active, params Vector3[] corners)
    {
      Vector3[] vector3Array = new Vector3[5]{ corners[0], corners[1], corners[2], corners[3], corners[0] };
      Color color = Handles.color;
      Handles.color = new Color(1f, 1f, 1f, !active ? 0.5f : 1f);
      RectHandles.DrawPolyLineWithShadow(new Color(0.0f, 0.0f, 0.0f, !active ? 0.5f : 1f), new Vector2(1f, -1f), vector3Array);
      Handles.color = color;
    }

    public static void DrawPolyLineWithShadow(Color shadowColor, Vector2 screenOffset, params Vector3[] points)
    {
      Camera current = Camera.current;
      if (!(bool) ((Object) current) || Event.current.type != EventType.Repaint)
        return;
      if (RectHandles.s_TempVectors.Length != points.Length)
        RectHandles.s_TempVectors = new Vector3[points.Length];
      for (int index = 0; index < points.Length; ++index)
        RectHandles.s_TempVectors[index] = current.ScreenToWorldPoint(current.WorldToScreenPoint(points[index]) + (Vector3) screenOffset);
      Color color = Handles.color;
      shadowColor.a = shadowColor.a * color.a;
      Handles.color = shadowColor;
      Handles.DrawPolyLine(RectHandles.s_TempVectors);
      Handles.color = color;
      Handles.DrawPolyLine(points);
    }

    public static void DrawDottedLineWithShadow(Color shadowColor, Vector2 screenOffset, Vector3 p1, Vector3 p2, float screenSpaceSize)
    {
      Camera current = Camera.current;
      if (!(bool) ((Object) current) || Event.current.type != EventType.Repaint)
        return;
      Color color = Handles.color;
      shadowColor.a = shadowColor.a * color.a;
      Handles.color = shadowColor;
      Handles.DrawDottedLine(current.ScreenToWorldPoint(current.WorldToScreenPoint(p1) + (Vector3) screenOffset), current.ScreenToWorldPoint(current.WorldToScreenPoint(p2) + (Vector3) screenOffset), screenSpaceSize);
      Handles.color = color;
      Handles.DrawDottedLine(p1, p2, screenSpaceSize);
    }

    private class Styles
    {
      public readonly GUIStyle dragdot = (GUIStyle) "U2D.dragDot";
      public readonly GUIStyle pivotdot = (GUIStyle) "U2D.pivotDot";
      public readonly GUIStyle dragdotactive = (GUIStyle) "U2D.dragDotActive";
      public readonly GUIStyle pivotdotactive = (GUIStyle) "U2D.pivotDotActive";
    }
  }
}
