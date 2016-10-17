// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.SliderScale
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class SliderScale
  {
    private static float s_ScaleDrawLength = 1f;
    private static float s_StartScale;
    private static float s_ValueDrag;
    private static Vector2 s_StartMousePosition;
    private static Vector2 s_CurrentMousePosition;

    public static float DoAxis(int id, float scale, Vector3 position, Vector3 direction, Quaternion rotation, float size, float snap)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0 || GUIUtility.keyboardControl == id && current.button == 2)
          {
            int num = id;
            GUIUtility.keyboardControl = num;
            GUIUtility.hotControl = num;
            SliderScale.s_CurrentMousePosition = SliderScale.s_StartMousePosition = current.mousePosition;
            SliderScale.s_StartScale = scale;
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
            SliderScale.s_CurrentMousePosition += current.delta;
            float num = Handles.SnapValue((float) (1.0 + (double) HandleUtility.CalcLineTranslation(SliderScale.s_StartMousePosition, SliderScale.s_CurrentMousePosition, position, direction) / (double) size), snap);
            scale = SliderScale.s_StartScale * num;
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
          float num1 = size;
          if (GUIUtility.hotControl == id)
            num1 = size * scale / SliderScale.s_StartScale;
          Handles.CubeCap(id, position + direction * num1 * SliderScale.s_ScaleDrawLength, rotation, size * 0.1f);
          Handles.DrawLine(position, position + direction * (float) ((double) num1 * (double) SliderScale.s_ScaleDrawLength - (double) size * 0.0500000007450581));
          if (id == GUIUtility.keyboardControl)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          HandleUtility.AddControl(id, HandleUtility.DistanceToLine(position, position + direction * size));
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position + direction * size, size * 0.2f));
          break;
      }
      return scale;
    }

    public static float DoCenter(int id, float value, Vector3 position, Quaternion rotation, float size, Handles.DrawCapFunction capFunc, float snap)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0 || GUIUtility.keyboardControl == id && current.button == 2)
          {
            int num = id;
            GUIUtility.keyboardControl = num;
            GUIUtility.hotControl = num;
            SliderScale.s_StartScale = value;
            SliderScale.s_ValueDrag = 0.0f;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            SliderScale.s_ScaleDrawLength = 1f;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            SliderScale.s_ValueDrag += HandleUtility.niceMouseDelta * 0.01f;
            value = (Handles.SnapValue(SliderScale.s_ValueDrag, snap) + 1f) * SliderScale.s_StartScale;
            SliderScale.s_ScaleDrawLength = value / SliderScale.s_StartScale;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == id && current.keyCode == KeyCode.Escape)
          {
            value = SliderScale.s_StartScale;
            SliderScale.s_ScaleDrawLength = 1f;
            GUIUtility.hotControl = 0;
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
          capFunc(id, position, rotation, size * 0.15f);
          if (id == GUIUtility.keyboardControl)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position, size * 0.15f));
          break;
      }
      return value;
    }
  }
}
