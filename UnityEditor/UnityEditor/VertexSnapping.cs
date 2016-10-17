// Decompiled with JetBrains decompiler
// Type: UnityEditor.VertexSnapping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class VertexSnapping
  {
    private static Vector3 s_VertexSnappingOffset = Vector3.zero;

    public static void HandleKeyAndMouseMove(int id)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseMove:
          if (!Tools.vertexDragging)
            break;
          VertexSnapping.EnableVertexSnapping(id);
          current.Use();
          break;
        case EventType.KeyDown:
          if (current.keyCode != KeyCode.V)
            break;
          if (!Tools.vertexDragging && !current.shift)
            VertexSnapping.EnableVertexSnapping(id);
          current.Use();
          break;
        case EventType.KeyUp:
          if (current.keyCode != KeyCode.V)
            break;
          if (current.shift)
            Tools.vertexDragging = !Tools.vertexDragging;
          else if (Tools.vertexDragging)
            Tools.vertexDragging = false;
          if (Tools.vertexDragging)
            VertexSnapping.EnableVertexSnapping(id);
          else
            VertexSnapping.DisableVertexSnapping(id);
          current.Use();
          break;
      }
    }

    private static void EnableVertexSnapping(int id)
    {
      Tools.vertexDragging = true;
      if (GUIUtility.hotControl == id)
      {
        Tools.handleOffset = VertexSnapping.s_VertexSnappingOffset;
      }
      else
      {
        VertexSnapping.UpdateVertexSnappingOffset();
        VertexSnapping.s_VertexSnappingOffset = Tools.handleOffset;
      }
    }

    private static void DisableVertexSnapping(int id)
    {
      Tools.vertexDragging = false;
      Tools.handleOffset = Vector3.zero;
      if (GUIUtility.hotControl == id)
        return;
      VertexSnapping.s_VertexSnappingOffset = Vector3.zero;
    }

    private static void UpdateVertexSnappingOffset()
    {
      Event current = Event.current;
      Tools.vertexDragging = true;
      Transform[] transforms = Selection.GetTransforms(SelectionMode.Deep | SelectionMode.ExcludePrefab | SelectionMode.Editable);
      HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
      Vector3 nearestPivot = VertexSnapping.FindNearestPivot(transforms, current.mousePosition);
      Vector3 vertex;
      Vector3 vector3 = !HandleUtility.FindNearestVertex(current.mousePosition, transforms, out vertex) || (double) (HandleUtility.WorldToGUIPoint(vertex) - current.mousePosition).magnitude >= (double) (HandleUtility.WorldToGUIPoint(nearestPivot) - current.mousePosition).magnitude ? nearestPivot : vertex;
      Tools.handleOffset = Vector3.zero;
      Tools.handleOffset = vector3 - Tools.handlePosition;
    }

    private static Vector3 FindNearestPivot(Transform[] transforms, Vector2 screenPosition)
    {
      bool flag = false;
      Vector3 vector3 = Vector3.zero;
      foreach (Transform transform in transforms)
      {
        Vector3 world = VertexSnapping.ScreenToWorld(screenPosition, transform);
        if (!flag || (double) (vector3 - world).magnitude > (double) (transform.position - world).magnitude)
        {
          vector3 = transform.position;
          flag = true;
        }
      }
      return vector3;
    }

    private static Vector3 ScreenToWorld(Vector2 screen, Transform target)
    {
      Ray worldRay = HandleUtility.GUIPointToWorldRay(screen);
      float enter = 0.0f;
      new Plane(target.forward, target.position).Raycast(worldRay, out enter);
      return worldRay.GetPoint(enter);
    }
  }
}
