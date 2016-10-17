// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Slider1D
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Slider1D
  {
    private static Vector2 s_StartMousePosition;
    private static Vector2 s_CurrentMousePosition;
    private static Vector3 s_StartPosition;

    internal static Vector3 Do(int id, Vector3 position, Vector3 direction, float size, Handles.DrawCapFunction drawFunc, float snap)
    {
      return Slider1D.Do(id, position, direction, direction, size, drawFunc, snap);
    }

    internal static Vector3 Do(int id, Vector3 position, Vector3 handleDirection, Vector3 slideDirection, float size, Handles.DrawCapFunction drawFunc, float snap)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if ((HandleUtility.nearestControl == id && current.button == 0 || GUIUtility.keyboardControl == id && current.button == 2) && GUIUtility.hotControl == 0)
          {
            int num = id;
            GUIUtility.keyboardControl = num;
            GUIUtility.hotControl = num;
            Slider1D.s_CurrentMousePosition = Slider1D.s_StartMousePosition = current.mousePosition;
            Slider1D.s_StartPosition = position;
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
            Slider1D.s_CurrentMousePosition += current.delta;
            float num = Handles.SnapValue(HandleUtility.CalcLineTranslation(Slider1D.s_StartMousePosition, Slider1D.s_CurrentMousePosition, Slider1D.s_StartPosition, slideDirection), snap);
            Vector3 vector3 = Handles.matrix.MultiplyVector(slideDirection);
            Vector3 v = Handles.s_Matrix.MultiplyPoint(Slider1D.s_StartPosition) + vector3 * num;
            position = Handles.s_InverseMatrix.MultiplyPoint(v);
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Color color = Color.white;
          if (id == GUIUtility.keyboardControl && GUI.enabled)
          {
            color = Handles.color;
            Handles.color = Handles.selectedColor;
          }
          drawFunc(id, position, Quaternion.LookRotation(handleDirection), size);
          if (id == GUIUtility.keyboardControl)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          if ((MulticastDelegate) drawFunc == (MulticastDelegate) new Handles.DrawCapFunction(Handles.ArrowCap))
          {
            HandleUtility.AddControl(id, HandleUtility.DistanceToLine(position, position + slideDirection * size));
            HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position + slideDirection * size, size * 0.2f));
            break;
          }
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position, size * 0.2f));
          break;
      }
      return position;
    }
  }
}
