// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Button
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Button
  {
    public static bool Do(int id, Vector3 position, Quaternion direction, float size, float pickSize, Handles.DrawCapFunction capFunc)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id)
          {
            GUIUtility.hotControl = id;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            current.Use();
            if (HandleUtility.nearestControl == id)
              return true;
            break;
          }
          break;
        case EventType.MouseMove:
          if (HandleUtility.nearestControl == id && current.button == 0 || GUIUtility.keyboardControl == id && current.button == 2)
          {
            HandleUtility.Repaint();
            break;
          }
          break;
        case EventType.Repaint:
          Color color = Handles.color;
          if (HandleUtility.nearestControl == id && GUI.enabled)
            Handles.color = Handles.selectedColor;
          capFunc(id, position, direction, size);
          Handles.color = color;
          break;
        case EventType.Layout:
          if (GUI.enabled)
          {
            HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position, pickSize));
            break;
          }
          break;
      }
      return false;
    }
  }
}
