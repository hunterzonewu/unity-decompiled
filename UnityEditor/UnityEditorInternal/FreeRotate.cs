// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.FreeRotate
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class FreeRotate
  {
    private static Vector2 s_CurrentMousePosition;

    public static Quaternion Do(int id, Quaternion rotation, Vector3 position, float size)
    {
      Vector3 vector3_1 = Handles.matrix.MultiplyPoint(position);
      Matrix4x4 matrix = Handles.matrix;
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
            FreeRotate.s_CurrentMousePosition = current.mousePosition;
            HandleUtility.ignoreRaySnapObjects = (Transform[]) null;
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
              if (obj != null)
              {
                Quaternion quaternion1 = Quaternion.LookRotation(((RaycastHit) obj).point - position);
                if (Tools.pivotRotation == PivotRotation.Global)
                {
                  Transform activeTransform = Selection.activeTransform;
                  if ((bool) ((Object) activeTransform))
                  {
                    Quaternion quaternion2 = Quaternion.Inverse(activeTransform.rotation) * rotation;
                    quaternion1 *= quaternion2;
                  }
                }
                rotation = quaternion1;
              }
            }
            else
            {
              FreeRotate.s_CurrentMousePosition += current.delta;
              Vector3 vector3_2 = Camera.current.transform.TransformDirection(new Vector3(-current.delta.y, -current.delta.x, 0.0f));
              rotation = Quaternion.AngleAxis(current.delta.magnitude, vector3_2.normalized) * rotation;
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
          Color color = Color.white;
          if (id == GUIUtility.keyboardControl)
          {
            color = Handles.color;
            Handles.color = Handles.selectedColor;
          }
          Handles.matrix = Matrix4x4.identity;
          Handles.DrawWireDisc(vector3_1, Camera.current.transform.forward, size);
          Handles.matrix = matrix;
          if (id == GUIUtility.keyboardControl)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          Handles.matrix = Matrix4x4.identity;
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(vector3_1, size) + 5f);
          Handles.matrix = matrix;
          break;
      }
      return rotation;
    }
  }
}
