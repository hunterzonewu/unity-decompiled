// Decompiled with JetBrains decompiler
// Type: UnityEditor.DragRectGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class DragRectGUI
  {
    private static int dragRectHash = "DragRect".GetHashCode();
    private static int s_DragCandidateState = 0;
    private static float s_DragSensitivity = 1f;

    public static int DragRect(Rect position, int value, int minValue, int maxValue)
    {
      Event current = Event.current;
      int controlId = GUIUtility.GetControlID(DragRectGUI.dragRectHash, FocusType.Passive, position);
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (position.Contains(current.mousePosition) && current.button == 0)
          {
            GUIUtility.hotControl = controlId;
            DragRectGUI.s_DragCandidateState = 1;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId && DragRectGUI.s_DragCandidateState != 0)
          {
            GUIUtility.hotControl = 0;
            DragRectGUI.s_DragCandidateState = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId && DragRectGUI.s_DragCandidateState == 1)
          {
            value += (int) ((double) HandleUtility.niceMouseDelta * (double) DragRectGUI.s_DragSensitivity);
            GUI.changed = true;
            current.Use();
            if (value < minValue)
            {
              value = minValue;
              break;
            }
            if (value > maxValue)
            {
              value = maxValue;
              break;
            }
            break;
          }
          break;
        case EventType.Repaint:
          EditorGUIUtility.AddCursorRect(position, MouseCursor.SlideArrow);
          break;
      }
      return value;
    }
  }
}
