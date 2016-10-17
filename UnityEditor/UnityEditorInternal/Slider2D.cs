// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Slider2D
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Slider2D
  {
    private static Vector2 s_CurrentMousePosition;
    private static Vector3 s_StartPosition;
    private static Vector2 s_StartPlaneOffset;

    public static Vector3 Do(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, float snap, bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, drawFunc, new Vector2(snap, snap), drawHelper);
    }

    public static Vector3 Do(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, float snap, bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, drawFunc, new Vector2(snap, snap), drawHelper);
    }

    public static Vector3 Do(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, bool drawHelper)
    {
      bool changed = GUI.changed;
      GUI.changed = false;
      Vector2 vector2 = Slider2D.CalcDeltaAlongDirections(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
      if (GUI.changed)
        handlePos = Slider2D.s_StartPosition + slideDir1 * vector2.x + slideDir2 * vector2.y;
      GUI.changed |= changed;
      return handlePos;
    }

    private static Vector2 CalcDeltaAlongDirections(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, bool drawHelper)
    {
      Vector2 vector2 = new Vector2(0.0f, 0.0f);
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if ((HandleUtility.nearestControl == id && current.button == 0 || GUIUtility.keyboardControl == id && current.button == 2) && GUIUtility.hotControl == 0)
          {
            Plane plane = new Plane(Handles.matrix.MultiplyVector(handleDir), Handles.matrix.MultiplyPoint(handlePos));
            Ray worldRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            float enter = 0.0f;
            plane.Raycast(worldRay, out enter);
            int num = id;
            GUIUtility.keyboardControl = num;
            GUIUtility.hotControl = num;
            Slider2D.s_CurrentMousePosition = current.mousePosition;
            Slider2D.s_StartPosition = handlePos;
            Vector3 lhs = Handles.s_InverseMatrix.MultiplyPoint(worldRay.GetPoint(enter)) - handlePos;
            Slider2D.s_StartPlaneOffset.x = Vector3.Dot(lhs, slideDir1);
            Slider2D.s_StartPlaneOffset.y = Vector3.Dot(lhs, slideDir2);
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            Slider2D.s_CurrentMousePosition += current.delta;
            Vector3 a = Handles.matrix.MultiplyPoint(handlePos);
            Vector3 normalized1 = Handles.matrix.MultiplyVector(slideDir1).normalized;
            Vector3 normalized2 = Handles.matrix.MultiplyVector(slideDir2).normalized;
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Slider2D.s_CurrentMousePosition);
            Plane plane = new Plane(a, a + normalized1, a + normalized2);
            float enter = 0.0f;
            if (plane.Raycast(worldRay, out enter))
            {
              Vector3 point = Handles.s_InverseMatrix.MultiplyPoint(worldRay.GetPoint(enter));
              vector2.x = HandleUtility.PointOnLineParameter(point, Slider2D.s_StartPosition, slideDir1);
              vector2.y = HandleUtility.PointOnLineParameter(point, Slider2D.s_StartPosition, slideDir2);
              vector2 -= Slider2D.s_StartPlaneOffset;
              if ((double) snap.x > 0.0 || (double) snap.y > 0.0)
              {
                vector2.x = Handles.SnapValue(vector2.x, snap.x);
                vector2.y = Handles.SnapValue(vector2.y, snap.y);
              }
              GUI.changed = true;
            }
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (drawFunc != null)
          {
            Vector3 position = handlePos + offset;
            Quaternion rotation = Quaternion.LookRotation(handleDir, slideDir1);
            Color color1 = Color.white;
            if (id == GUIUtility.keyboardControl)
            {
              color1 = Handles.color;
              Handles.color = Handles.selectedColor;
            }
            drawFunc(id, position, rotation, handleSize);
            if (id == GUIUtility.keyboardControl)
              Handles.color = color1;
            if (drawHelper && GUIUtility.hotControl == id)
            {
              Vector3[] verts = new Vector3[4];
              float num1 = handleSize * 10f;
              verts[0] = position + slideDir1 * num1 + slideDir2 * num1;
              verts[1] = verts[0] - slideDir1 * num1 * 2f;
              verts[2] = verts[1] - slideDir2 * num1 * 2f;
              verts[3] = verts[2] + slideDir1 * num1 * 2f;
              Color color2 = Handles.color;
              Handles.color = Color.white;
              float num2 = 0.6f;
              Handles.DrawSolidRectangleWithOutline(verts, new Color(1f, 1f, 1f, 0.05f), new Color(num2, num2, num2, 0.4f));
              Handles.color = color2;
              break;
            }
            break;
          }
          break;
        case EventType.Layout:
          if ((MulticastDelegate) drawFunc == (MulticastDelegate) new Handles.DrawCapFunction(Handles.ArrowCap))
          {
            HandleUtility.AddControl(id, HandleUtility.DistanceToLine(handlePos + offset, handlePos + handleDir * handleSize));
            HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(handlePos + offset + handleDir * handleSize, handleSize * 0.2f));
            break;
          }
          if ((MulticastDelegate) drawFunc == (MulticastDelegate) new Handles.DrawCapFunction(Handles.RectangleCap))
          {
            HandleUtility.AddControl(id, HandleUtility.DistanceToRectangle(handlePos + offset, Quaternion.LookRotation(handleDir, slideDir1), handleSize));
            break;
          }
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(handlePos + offset, handleSize * 0.5f));
          break;
      }
      return vector2;
    }
  }
}
