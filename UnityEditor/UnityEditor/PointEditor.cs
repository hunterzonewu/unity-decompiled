// Decompiled with JetBrains decompiler
// Type: UnityEditor.PointEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class PointEditor
  {
    private static Vector3 s_EditingScale = Vector3.one;
    private static Quaternion s_EditingRotation = Quaternion.identity;
    private static Vector2 s_StartMouseDragPosition;
    private static List<int> s_StartDragSelection;
    private static bool s_DidDrag;

    public static bool MovePoints(IEditablePoint points, Transform cloudTransform, List<int> selection)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PointEditor.\u003CMovePoints\u003Ec__AnonStorey40 pointsCAnonStorey40 = new PointEditor.\u003CMovePoints\u003Ec__AnonStorey40();
      // ISSUE: reference to a compiler-generated field
      pointsCAnonStorey40.points = points;
      if (selection.Count == 0)
        return false;
      if (Event.current.type == EventType.MouseUp)
      {
        PointEditor.s_EditingScale = Vector3.one;
        PointEditor.s_EditingRotation = Quaternion.identity;
      }
      if ((bool) ((UnityEngine.Object) Camera.current))
      {
        Vector3 zero = Vector3.zero;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        Vector3 position1 = Tools.pivotMode != PivotMode.Pivot ? selection.Aggregate<int, Vector3>(zero, new Func<Vector3, int, Vector3>(pointsCAnonStorey40.\u003C\u003Em__57)) / (float) selection.Count : pointsCAnonStorey40.points.GetPosition(selection[0]);
        Vector3 position2 = cloudTransform.TransformPoint(position1);
        switch (Tools.current)
        {
          case Tool.Move:
            Vector3 position3 = Handles.PositionHandle(position2, Tools.pivotRotation != PivotRotation.Local ? Quaternion.identity : cloudTransform.rotation);
            if (GUI.changed)
            {
              Vector3 vector3 = cloudTransform.InverseTransformPoint(position3) - cloudTransform.InverseTransformPoint(position2);
              using (List<int>.Enumerator enumerator = selection.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  int current = enumerator.Current;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  pointsCAnonStorey40.points.SetPosition(current, pointsCAnonStorey40.points.GetPosition(current) + vector3);
                }
              }
              return true;
            }
            break;
          case Tool.Rotate:
            Quaternion quaternion = Handles.RotationHandle(PointEditor.s_EditingRotation, position2);
            if (GUI.changed)
            {
              Vector3 vector3_1 = cloudTransform.InverseTransformPoint(position2);
              using (List<int>.Enumerator enumerator = selection.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  int current = enumerator.Current;
                  // ISSUE: reference to a compiler-generated field
                  Vector3 vector3_2 = pointsCAnonStorey40.points.GetPosition(current) - vector3_1;
                  Vector3 vector3_3 = Quaternion.Inverse(PointEditor.s_EditingRotation) * vector3_2;
                  Vector3 position4 = quaternion * vector3_3 + vector3_1;
                  // ISSUE: reference to a compiler-generated field
                  pointsCAnonStorey40.points.SetPosition(current, position4);
                }
              }
              PointEditor.s_EditingRotation = quaternion;
              return true;
            }
            break;
          case Tool.Scale:
            Vector3 vector3_4 = Handles.ScaleHandle(PointEditor.s_EditingScale, position2, Quaternion.identity, HandleUtility.GetHandleSize(position2));
            if (GUI.changed)
            {
              Vector3 vector3_1 = cloudTransform.InverseTransformPoint(position2);
              using (List<int>.Enumerator enumerator = selection.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  int current = enumerator.Current;
                  // ISSUE: reference to a compiler-generated field
                  Vector3 position4 = pointsCAnonStorey40.points.GetPosition(current) - vector3_1;
                  position4.x /= PointEditor.s_EditingScale.x;
                  position4.y /= PointEditor.s_EditingScale.y;
                  position4.z /= PointEditor.s_EditingScale.z;
                  position4.x *= vector3_4.x;
                  position4.y *= vector3_4.y;
                  position4.z *= vector3_4.z;
                  position4 += vector3_1;
                  // ISSUE: reference to a compiler-generated field
                  pointsCAnonStorey40.points.SetPosition(current, position4);
                }
              }
              PointEditor.s_EditingScale = vector3_4;
              return true;
            }
            break;
        }
      }
      return false;
    }

    public static int FindNearest(Vector2 point, Transform cloudTransform, IEditablePoint points)
    {
      Ray worldRay = HandleUtility.GUIPointToWorldRay(point);
      Dictionary<int, float> source = new Dictionary<int, float>();
      for (int index = 0; index < points.Count; ++index)
      {
        float t = 0.0f;
        Vector3 zero = Vector3.zero;
        if (MathUtils.IntersectRaySphere(worldRay, cloudTransform.TransformPoint(points.GetPosition(index)), points.GetPointScale() * 0.5f, ref t, ref zero) && (double) t > 0.0)
          source.Add(index, t);
      }
      if (source.Count <= 0)
        return -1;
      return source.OrderBy<KeyValuePair<int, float>, float>((Func<KeyValuePair<int, float>, float>) (x => x.Value)).First<KeyValuePair<int, float>>().Key;
    }

    public static bool SelectPoints(IEditablePoint points, Transform cloudTransform, ref List<int> selection, bool firstSelect)
    {
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      if (Event.current.alt && Event.current.type != EventType.Repaint)
        return false;
      bool flag = false;
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if ((HandleUtility.nearestControl == controlId || firstSelect) && current.button == 0)
          {
            if (!current.shift && !EditorGUI.actionKey)
            {
              selection.Clear();
              flag = true;
            }
            GUIUtility.hotControl = controlId;
            PointEditor.s_StartMouseDragPosition = current.mousePosition;
            PointEditor.s_StartDragSelection = new List<int>((IEnumerable<int>) selection);
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId && current.button == 0)
          {
            if (!PointEditor.s_DidDrag)
            {
              int nearest = PointEditor.FindNearest(PointEditor.s_StartMouseDragPosition, cloudTransform, points);
              if (nearest != -1)
              {
                if (!current.shift && !EditorGUI.actionKey)
                {
                  selection.Add(nearest);
                }
                else
                {
                  int index = selection.IndexOf(nearest);
                  if (index != -1)
                    selection.RemoveAt(index);
                  else
                    selection.Add(nearest);
                }
              }
              GUI.changed = true;
              flag = true;
            }
            PointEditor.s_StartDragSelection = (List<int>) null;
            PointEditor.s_StartMouseDragPosition = Vector2.zero;
            PointEditor.s_DidDrag = false;
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId && current.button == 0)
          {
            PointEditor.s_DidDrag = true;
            selection.Clear();
            selection.AddRange((IEnumerable<int>) PointEditor.s_StartDragSelection);
            Rect rect = PointEditor.FromToRect(PointEditor.s_StartMouseDragPosition, current.mousePosition);
            Matrix4x4 matrix = Handles.matrix;
            Handles.matrix = cloudTransform.localToWorldMatrix;
            for (int idx = 0; idx < points.Count; ++idx)
            {
              Vector2 guiPoint = HandleUtility.WorldToGUIPoint(points.GetPosition(idx));
              if (rect.Contains(guiPoint))
                selection.Add(idx);
            }
            Handles.matrix = matrix;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == controlId && current.mousePosition != PointEditor.s_StartMouseDragPosition)
          {
            GUIStyle guiStyle = (GUIStyle) "SelectionRect";
            Handles.BeginGUI();
            guiStyle.Draw(PointEditor.FromToRect(PointEditor.s_StartMouseDragPosition, current.mousePosition), false, false, false, false);
            Handles.EndGUI();
            break;
          }
          break;
        case EventType.Layout:
          HandleUtility.AddDefaultControl(controlId);
          break;
      }
      selection = selection.Distinct<int>().ToList<int>();
      return flag;
    }

    public static void Draw(IEditablePoint points, Transform cloudTransform, List<int> selection, bool twoPassDrawing)
    {
      LightmapVisualization.DrawPointCloud(points.GetUnselectedPositions(), points.GetSelectedPositions(), points.GetDefaultColor(), points.GetSelectedColor(), points.GetPointScale(), cloudTransform);
    }

    private static Rect FromToRect(Vector2 from, Vector2 to)
    {
      Rect rect = new Rect(from.x, from.y, to.x - from.x, to.y - from.y);
      if ((double) rect.width < 0.0)
      {
        rect.x += rect.width;
        rect.width = -rect.width;
      }
      if ((double) rect.height < 0.0)
      {
        rect.y += rect.height;
        rect.height = -rect.height;
      }
      return rect;
    }
  }
}
