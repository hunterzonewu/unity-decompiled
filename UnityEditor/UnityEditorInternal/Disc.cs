// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Disc
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Disc
  {
    private static Vector2 s_StartMousePosition;
    private static Vector2 s_CurrentMousePosition;
    private static Vector3 s_StartPosition;
    private static Vector3 s_StartAxis;
    private static Quaternion s_StartRotation;
    private static float s_RotationDist;

    public static Quaternion Do(int id, Quaternion rotation, Vector3 position, Vector3 axis, float size, bool cutoffPlane, float snap)
    {
      if ((double) Mathf.Abs(Vector3.Dot(Camera.current.transform.forward, axis)) > 0.999000012874603)
        cutoffPlane = false;
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0 || GUIUtility.keyboardControl == id && current.button == 2)
          {
            int num = id;
            GUIUtility.keyboardControl = num;
            GUIUtility.hotControl = num;
            Tools.LockHandlePosition();
            if (cutoffPlane)
            {
              Vector3 normalized = Vector3.Cross(axis, Camera.current.transform.forward).normalized;
              Disc.s_StartPosition = HandleUtility.ClosestPointToArc(position, axis, normalized, 180f, size);
            }
            else
              Disc.s_StartPosition = HandleUtility.ClosestPointToDisc(position, axis, size);
            Disc.s_RotationDist = 0.0f;
            Disc.s_StartRotation = rotation;
            Disc.s_StartAxis = axis;
            Disc.s_CurrentMousePosition = Disc.s_StartMousePosition = Event.current.mousePosition;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            Tools.UnlockHandlePosition();
            GUIUtility.hotControl = 0;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            if (EditorGUI.actionKey && current.shift)
            {
              if (HandleUtility.ignoreRaySnapObjects == null)
                Handles.SetupIgnoreRaySnapObjects();
              object obj = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(current.mousePosition));
              if (obj != null && (double) Vector3.Dot(axis.normalized, rotation * Vector3.forward) < 0.999)
              {
                Vector3 lhs = ((RaycastHit) obj).point - position;
                rotation = Quaternion.LookRotation(lhs - Vector3.Dot(lhs, axis.normalized) * axis.normalized, rotation * Vector3.up);
              }
            }
            else
            {
              Vector3 normalized = Vector3.Cross(axis, position - Disc.s_StartPosition).normalized;
              Disc.s_CurrentMousePosition += current.delta;
              Disc.s_RotationDist = (float) ((double) HandleUtility.CalcLineTranslation(Disc.s_StartMousePosition, Disc.s_CurrentMousePosition, Disc.s_StartPosition, normalized) / (double) size * 30.0);
              Disc.s_RotationDist = Handles.SnapValue(Disc.s_RotationDist, snap);
              rotation = Quaternion.AngleAxis(Disc.s_RotationDist * -1f, Disc.s_StartAxis) * Disc.s_StartRotation;
            }
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (current.keyCode == KeyCode.Escape && GUIUtility.hotControl == id)
          {
            Tools.UnlockHandlePosition();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.Repaint:
          Color color1 = Color.white;
          if (id == GUIUtility.keyboardControl)
          {
            color1 = Handles.color;
            Handles.color = Handles.selectedColor;
          }
          if (GUIUtility.hotControl == id)
          {
            Color color2 = Handles.color;
            Vector3 normalized = (Disc.s_StartPosition - position).normalized;
            Handles.color = Handles.secondaryColor;
            Handles.DrawLine(position, position + normalized * size * 1.1f);
            float angle = Mathf.Repeat((float) (-(double) Disc.s_RotationDist - 180.0), 360f) - 180f;
            Vector3 vector3 = Quaternion.AngleAxis(angle, axis) * normalized;
            Handles.DrawLine(position, position + vector3 * size * 1.1f);
            Handles.color = Handles.secondaryColor * new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawSolidArc(position, axis, normalized, angle, size);
            Handles.color = color2;
          }
          if (cutoffPlane)
          {
            Vector3 normalized = Vector3.Cross(axis, Camera.current.transform.forward).normalized;
            Handles.DrawWireArc(position, axis, normalized, 180f, size);
          }
          else
            Handles.DrawWireDisc(position, axis, size);
          if (id == GUIUtility.keyboardControl)
          {
            Handles.color = color1;
            break;
          }
          break;
        case EventType.Layout:
          float distance;
          if (cutoffPlane)
          {
            Vector3 normalized = Vector3.Cross(axis, Camera.current.transform.forward).normalized;
            distance = HandleUtility.DistanceToArc(position, axis, normalized, 180f, size) / 2f;
          }
          else
            distance = HandleUtility.DistanceToDisc(position, axis, size) / 2f;
          HandleUtility.AddControl(id, distance);
          break;
      }
      return rotation;
    }
  }
}
