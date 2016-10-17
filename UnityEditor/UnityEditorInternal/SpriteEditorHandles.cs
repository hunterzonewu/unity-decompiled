// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.SpriteEditorHandles
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  public static class SpriteEditorHandles
  {
    private static int s_RectSelectionID = GUIUtility.GetPermanentControlID();
    private static Vector2 s_CurrentMousePosition;
    private static Vector2 s_DragStartScreenPosition;
    private static Vector2 s_DragScreenOffset;

    internal static Vector2 PointSlider(Vector2 pos, MouseCursor cursor, GUIStyle dragDot, GUIStyle dragDotActive)
    {
      int controlId = GUIUtility.GetControlID("Slider1D".GetHashCode(), FocusType.Keyboard);
      Vector2 vector2 = (Vector2) Handles.matrix.MultiplyPoint((Vector3) pos);
      Rect rect = new Rect(vector2.x - dragDot.fixedWidth * 0.5f, vector2.y - dragDot.fixedHeight * 0.5f, dragDot.fixedWidth, dragDot.fixedHeight);
      if (Event.current.GetTypeForControl(controlId) == EventType.Repaint)
      {
        if (GUIUtility.hotControl == controlId)
          dragDotActive.Draw(rect, GUIContent.none, controlId);
        else
          dragDot.Draw(rect, GUIContent.none, controlId);
      }
      return SpriteEditorHandles.ScaleSlider(pos, cursor, rect);
    }

    internal static Vector2 ScaleSlider(Vector2 pos, MouseCursor cursor, Rect cursorRect)
    {
      return SpriteEditorHandles.ScaleSlider(GUIUtility.GetControlID("Slider1D".GetHashCode(), FocusType.Keyboard), pos, cursor, cursorRect);
    }

    private static Vector2 ScaleSlider(int id, Vector2 pos, MouseCursor cursor, Rect cursorRect)
    {
      Vector2 vector2_1 = (Vector2) Handles.matrix.MultiplyPoint((Vector3) pos);
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (current.button == 0 && cursorRect.Contains(Event.current.mousePosition) && !current.alt)
          {
            int num = id;
            GUIUtility.keyboardControl = num;
            GUIUtility.hotControl = num;
            SpriteEditorHandles.s_CurrentMousePosition = current.mousePosition;
            SpriteEditorHandles.s_DragStartScreenPosition = current.mousePosition;
            SpriteEditorHandles.s_DragScreenOffset = SpriteEditorHandles.s_CurrentMousePosition - vector2_1;
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
            SpriteEditorHandles.s_CurrentMousePosition += current.delta;
            Vector2 vector2_2 = pos;
            pos = (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) SpriteEditorHandles.s_CurrentMousePosition);
            if (!Mathf.Approximately((vector2_2 - pos).magnitude, 0.0f))
              GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == id && current.keyCode == KeyCode.Escape)
          {
            pos = (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) (SpriteEditorHandles.s_DragStartScreenPosition - SpriteEditorHandles.s_DragScreenOffset));
            GUIUtility.hotControl = 0;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          EditorGUIUtility.AddCursorRect(cursorRect, cursor, id);
          break;
      }
      return pos;
    }

    internal static Vector2 PivotSlider(Rect sprite, Vector2 pos, GUIStyle pivotDot, GUIStyle pivotDotActive)
    {
      int controlId = GUIUtility.GetControlID("Slider1D".GetHashCode(), FocusType.Keyboard);
      pos = new Vector2(sprite.xMin + sprite.width * pos.x, sprite.yMin + sprite.height * pos.y);
      Vector2 vector2_1 = (Vector2) Handles.matrix.MultiplyPoint((Vector3) pos);
      Rect position = new Rect(vector2_1.x - pivotDot.fixedWidth * 0.5f, vector2_1.y - pivotDot.fixedHeight * 0.5f, pivotDotActive.fixedWidth, pivotDotActive.fixedHeight);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current.button == 0 && position.Contains(Event.current.mousePosition) && !current.alt)
          {
            int num = controlId;
            GUIUtility.keyboardControl = num;
            GUIUtility.hotControl = num;
            SpriteEditorHandles.s_CurrentMousePosition = current.mousePosition;
            SpriteEditorHandles.s_DragStartScreenPosition = current.mousePosition;
            Vector2 vector2_2 = (Vector2) Handles.matrix.MultiplyPoint((Vector3) pos);
            SpriteEditorHandles.s_DragScreenOffset = SpriteEditorHandles.s_CurrentMousePosition - vector2_2;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            SpriteEditorHandles.s_CurrentMousePosition += current.delta;
            Vector2 vector2_2 = pos;
            Vector3 vector3 = Handles.s_InverseMatrix.MultiplyPoint((Vector3) (SpriteEditorHandles.s_CurrentMousePosition - SpriteEditorHandles.s_DragScreenOffset));
            pos = new Vector2(vector3.x, vector3.y);
            if (!Mathf.Approximately((vector2_2 - pos).magnitude, 0.0f))
              GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == controlId && current.keyCode == KeyCode.Escape)
          {
            pos = (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) (SpriteEditorHandles.s_DragStartScreenPosition - SpriteEditorHandles.s_DragScreenOffset));
            GUIUtility.hotControl = 0;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          EditorGUIUtility.AddCursorRect(position, MouseCursor.Arrow, controlId);
          if (GUIUtility.hotControl == controlId)
          {
            pivotDotActive.Draw(position, GUIContent.none, controlId);
            break;
          }
          pivotDot.Draw(position, GUIContent.none, controlId);
          break;
      }
      pos = new Vector2((pos.x - sprite.xMin) / sprite.width, (pos.y - sprite.yMin) / sprite.height);
      return pos;
    }

    internal static Rect SliderRect(Rect pos)
    {
      int controlId = GUIUtility.GetControlID("SliderRect".GetHashCode(), FocusType.Keyboard);
      Event current = Event.current;
      if (SpriteEditorWindow.s_OneClickDragStarted && current.type == EventType.Repaint)
      {
        SpriteEditorHandles.HandleSliderRectMouseDown(controlId, current, pos);
        SpriteEditorWindow.s_OneClickDragStarted = false;
      }
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current.button == 0 && pos.Contains(Handles.s_InverseMatrix.MultiplyPoint((Vector3) Event.current.mousePosition)) && !current.alt)
          {
            SpriteEditorHandles.HandleSliderRectMouseDown(controlId, current, pos);
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            SpriteEditorHandles.s_CurrentMousePosition += current.delta;
            Vector2 center = pos.center;
            pos.center = (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) (SpriteEditorHandles.s_CurrentMousePosition - SpriteEditorHandles.s_DragScreenOffset));
            if (!Mathf.Approximately((center - pos.center).magnitude, 0.0f))
              GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == controlId && current.keyCode == KeyCode.Escape)
          {
            pos.center = (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) (SpriteEditorHandles.s_DragStartScreenPosition - SpriteEditorHandles.s_DragScreenOffset));
            GUIUtility.hotControl = 0;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Vector2 vector2_1 = (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) new Vector2(pos.xMin, pos.yMin));
          Vector2 vector2_2 = (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) new Vector2(pos.xMax, pos.yMax));
          EditorGUIUtility.AddCursorRect(new Rect(vector2_1.x, vector2_1.y, vector2_2.x - vector2_1.x, vector2_2.y - vector2_1.y), MouseCursor.Arrow, controlId);
          break;
      }
      return pos;
    }

    internal static void HandleSliderRectMouseDown(int id, Event evt, Rect pos)
    {
      int num = id;
      GUIUtility.keyboardControl = num;
      GUIUtility.hotControl = num;
      SpriteEditorHandles.s_CurrentMousePosition = evt.mousePosition;
      SpriteEditorHandles.s_DragStartScreenPosition = evt.mousePosition;
      Vector2 vector2 = (Vector2) Handles.matrix.MultiplyPoint((Vector3) pos.center);
      SpriteEditorHandles.s_DragScreenOffset = SpriteEditorHandles.s_CurrentMousePosition - vector2;
      EditorGUIUtility.SetWantsMouseJumping(1);
    }

    internal static Rect RectCreator(float textureWidth, float textureHeight, GUIStyle rectStyle)
    {
      Event current = Event.current;
      Vector2 mousePosition = current.mousePosition;
      int rectSelectionId = SpriteEditorHandles.s_RectSelectionID;
      Rect rect1 = new Rect();
      switch (current.GetTypeForControl(rectSelectionId))
      {
        case EventType.MouseDown:
          if (current.button == 0)
          {
            GUIUtility.hotControl = rectSelectionId;
            Rect rect2 = new Rect(0.0f, 0.0f, textureWidth, textureHeight);
            Vector2 vector2 = (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) mousePosition);
            vector2.x = Mathf.Min(Mathf.Max(vector2.x, rect2.xMin), rect2.xMax);
            vector2.y = Mathf.Min(Mathf.Max(vector2.y, rect2.yMin), rect2.yMax);
            SpriteEditorHandles.s_DragStartScreenPosition = (Vector2) Handles.s_Matrix.MultiplyPoint((Vector3) vector2);
            SpriteEditorHandles.s_CurrentMousePosition = mousePosition;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == rectSelectionId && current.button == 0)
          {
            if (SpriteEditorHandles.ValidRect(SpriteEditorHandles.s_DragStartScreenPosition, SpriteEditorHandles.s_CurrentMousePosition))
            {
              rect1 = SpriteEditorHandles.GetCurrentRect(false, textureWidth, textureHeight, SpriteEditorHandles.s_DragStartScreenPosition, SpriteEditorHandles.s_CurrentMousePosition);
              GUI.changed = true;
              current.Use();
            }
            GUIUtility.hotControl = 0;
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == rectSelectionId)
          {
            SpriteEditorHandles.s_CurrentMousePosition = new Vector2(mousePosition.x, mousePosition.y);
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == rectSelectionId && current.keyCode == KeyCode.Escape)
          {
            GUIUtility.hotControl = 0;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == rectSelectionId && SpriteEditorHandles.ValidRect(SpriteEditorHandles.s_DragStartScreenPosition, SpriteEditorHandles.s_CurrentMousePosition))
          {
            SpriteEditorUtility.BeginLines(Color.green * 1.5f);
            SpriteEditorUtility.DrawBox(SpriteEditorHandles.GetCurrentRect(false, textureWidth, textureHeight, SpriteEditorHandles.s_DragStartScreenPosition, SpriteEditorHandles.s_CurrentMousePosition));
            SpriteEditorUtility.EndLines();
            break;
          }
          break;
      }
      return rect1;
    }

    private static bool ValidRect(Vector2 startPoint, Vector2 endPoint)
    {
      if ((double) Mathf.Abs((endPoint - startPoint).x) > 5.0)
        return (double) Mathf.Abs((endPoint - startPoint).y) > 5.0;
      return false;
    }

    private static Rect GetCurrentRect(bool screenSpace, float textureWidth, float textureHeight, Vector2 startPoint, Vector2 endPoint)
    {
      Rect rect = SpriteEditorUtility.ClampedRect(SpriteEditorUtility.RoundToInt(EditorGUIExt.FromToRect((Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) startPoint), (Vector2) Handles.s_InverseMatrix.MultiplyPoint((Vector3) endPoint))), new Rect(0.0f, 0.0f, textureWidth, textureHeight), false);
      if (screenSpace)
      {
        Vector2 vector2_1 = (Vector2) Handles.matrix.MultiplyPoint((Vector3) new Vector2(rect.xMin, rect.yMin));
        Vector2 vector2_2 = (Vector2) Handles.matrix.MultiplyPoint((Vector3) new Vector2(rect.xMax, rect.yMax));
        rect = new Rect(vector2_1.x, vector2_1.y, vector2_2.x - vector2_1.x, vector2_2.y - vector2_1.y);
      }
      return rect;
    }
  }
}
