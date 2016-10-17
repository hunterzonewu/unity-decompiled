// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FreeMove
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class FreeMove
  {
    private static Vector2 s_StartMousePosition;
    private static Vector2 s_CurrentMousePosition;
    private static Vector3 s_StartPosition;

    public static Vector3 Do(int id, Vector3 position, Quaternion rotation, float size, Vector3 snap, Handles.DrawCapFunction capFunc)
    {
      Vector3 position1 = Handles.matrix.MultiplyPoint(position);
      Matrix4x4 matrix = Handles.matrix;
      VertexSnapping.HandleKeyAndMouseMove(id);
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0 || GUIUtility.keyboardControl == id && current.button == 2)
          {
            int num = id;
            GUIUtility.keyboardControl = num;
            GUIUtility.hotControl = num;
            FreeMove.s_CurrentMousePosition = FreeMove.s_StartMousePosition = current.mousePosition;
            FreeMove.s_StartPosition = position;
            HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            bool flag = EditorGUI.actionKey && current.shift;
            if (flag)
            {
              if (HandleUtility.ignoreRaySnapObjects == null)
                Handles.SetupIgnoreRaySnapObjects();
              object obj = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(current.mousePosition));
              if (obj != null)
              {
                RaycastHit raycastHit = (RaycastHit) obj;
                float num1 = 0.0f;
                if (Tools.pivotMode == PivotMode.Center)
                {
                  float num2 = HandleUtility.CalcRayPlaceOffset(HandleUtility.ignoreRaySnapObjects, raycastHit.normal);
                  if ((double) num2 != double.PositiveInfinity)
                    num1 = Vector3.Dot(position, raycastHit.normal) - num2;
                }
                position = Handles.s_InverseMatrix.MultiplyPoint(raycastHit.point + raycastHit.normal * num1);
              }
              else
                flag = false;
            }
            if (!flag)
            {
              FreeMove.s_CurrentMousePosition += new Vector2(current.delta.x, -current.delta.y);
              Vector3 position2 = Camera.current.WorldToScreenPoint(Handles.s_Matrix.MultiplyPoint(FreeMove.s_StartPosition)) + (Vector3) (FreeMove.s_CurrentMousePosition - FreeMove.s_StartMousePosition);
              position = Handles.s_InverseMatrix.MultiplyPoint(Camera.current.ScreenToWorldPoint(position2));
              if (Camera.current.transform.forward == Vector3.forward || Camera.current.transform.forward == -Vector3.forward)
                position.z = FreeMove.s_StartPosition.z;
              if (Camera.current.transform.forward == Vector3.up || Camera.current.transform.forward == -Vector3.up)
                position.y = FreeMove.s_StartPosition.y;
              if (Camera.current.transform.forward == Vector3.right || Camera.current.transform.forward == -Vector3.right)
                position.x = FreeMove.s_StartPosition.x;
              if (Tools.vertexDragging)
              {
                if (HandleUtility.ignoreRaySnapObjects == null)
                  Handles.SetupIgnoreRaySnapObjects();
                Vector3 vertex;
                if (HandleUtility.FindNearestVertex(current.mousePosition, (Transform[]) null, out vertex))
                  position = Handles.s_InverseMatrix.MultiplyPoint(vertex);
              }
              if (EditorGUI.actionKey && !current.shift)
              {
                Vector3 vector3 = position - FreeMove.s_StartPosition;
                vector3.x = Handles.SnapValue(vector3.x, snap.x);
                vector3.y = Handles.SnapValue(vector3.y, snap.y);
                vector3.z = Handles.SnapValue(vector3.z, snap.z);
                position = FreeMove.s_StartPosition + vector3;
              }
            }
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Color color = Color.white;
          if (id == GUIUtility.keyboardControl)
          {
            color = Handles.color;
            Handles.color = Handles.selectedColor;
          }
          Handles.matrix = Matrix4x4.identity;
          capFunc(id, position1, Camera.current.transform.rotation, size);
          Handles.matrix = matrix;
          if (id == GUIUtility.keyboardControl)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          Handles.matrix = Matrix4x4.identity;
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position1, size * 1.2f));
          Handles.matrix = matrix;
          break;
      }
      return position;
    }
  }
}
