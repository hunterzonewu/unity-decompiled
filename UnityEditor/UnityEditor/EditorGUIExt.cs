// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorGUIExt
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class EditorGUIExt
  {
    private static EditorGUIExt.Styles ms_Styles = new EditorGUIExt.Styles();
    private static int repeatButtonHash = "repeatButton".GetHashCode();
    private static float nextScrollStepTime = 0.0f;
    private static int firstScrollWait = 250;
    private static int scrollWait = 30;
    private static int kFirstScrollWait = 250;
    private static int kScrollWait = 30;
    private static DateTime s_NextScrollStepTime = DateTime.Now;
    private static Vector2 s_MouseDownPos = Vector2.zero;
    private static EditorGUIExt.DragSelectionState s_MultiSelectDragSelection = EditorGUIExt.DragSelectionState.None;
    private static Vector2 s_StartSelectPos = Vector2.zero;
    private static List<bool> s_SelectionBackup = (List<bool>) null;
    private static List<bool> s_LastFrameSelections = (List<bool>) null;
    internal static int s_MinMaxSliderHash = "MinMaxSlider".GetHashCode();
    private static bool adding = false;
    private static int initIndex = 0;
    private static int scrollControlID;
    private static EditorGUIExt.MinMaxSliderState s_MinMaxSliderState;
    private static bool[] initSelections;

    private static bool DoRepeatButton(Rect position, GUIContent content, GUIStyle style, FocusType focusType)
    {
      int controlId = GUIUtility.GetControlID(EditorGUIExt.repeatButtonHash, focusType, position);
      switch (Event.current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (position.Contains(Event.current.mousePosition))
          {
            GUIUtility.hotControl = controlId;
            Event.current.Use();
          }
          return false;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            return false;
          GUIUtility.hotControl = 0;
          Event.current.Use();
          return position.Contains(Event.current.mousePosition);
        case EventType.Repaint:
          style.Draw(position, content, controlId);
          if (controlId == GUIUtility.hotControl)
            return position.Contains(Event.current.mousePosition);
          return false;
        default:
          return false;
      }
    }

    private static bool ScrollerRepeatButton(int scrollerID, Rect rect, GUIStyle style)
    {
      bool flag1 = false;
      if (EditorGUIExt.DoRepeatButton(rect, GUIContent.none, style, FocusType.Passive))
      {
        bool flag2 = EditorGUIExt.scrollControlID != scrollerID;
        EditorGUIExt.scrollControlID = scrollerID;
        if (flag2)
        {
          flag1 = true;
          EditorGUIExt.nextScrollStepTime = Time.realtimeSinceStartup + 1f / 1000f * (float) EditorGUIExt.firstScrollWait;
        }
        else if ((double) Time.realtimeSinceStartup >= (double) EditorGUIExt.nextScrollStepTime)
        {
          flag1 = true;
          EditorGUIExt.nextScrollStepTime = Time.realtimeSinceStartup + 1f / 1000f * (float) EditorGUIExt.scrollWait;
        }
        if (Event.current.type == EventType.Repaint)
          HandleUtility.Repaint();
      }
      return flag1;
    }

    public static void MinMaxScroller(Rect position, int id, ref float value, ref float size, float visualStart, float visualEnd, float startLimit, float endLimit, GUIStyle slider, GUIStyle thumb, GUIStyle leftButton, GUIStyle rightButton, bool horiz)
    {
      float num1 = !horiz ? size * 10f / position.height : size * 10f / position.width;
      Rect position1;
      Rect rect1;
      Rect rect2;
      if (horiz)
      {
        position1 = new Rect(position.x + leftButton.fixedWidth, position.y, position.width - leftButton.fixedWidth - rightButton.fixedWidth, position.height);
        rect1 = new Rect(position.x, position.y, leftButton.fixedWidth, position.height);
        rect2 = new Rect(position.xMax - rightButton.fixedWidth, position.y, rightButton.fixedWidth, position.height);
      }
      else
      {
        position1 = new Rect(position.x, position.y + leftButton.fixedHeight, position.width, position.height - leftButton.fixedHeight - rightButton.fixedHeight);
        rect1 = new Rect(position.x, position.y, position.width, leftButton.fixedHeight);
        rect2 = new Rect(position.x, position.yMax - rightButton.fixedHeight, position.width, rightButton.fixedHeight);
      }
      float num2 = Mathf.Min(visualStart, value);
      float num3 = Mathf.Max(visualEnd, value + size);
      EditorGUIExt.MinMaxSlider(position1, ref value, ref size, num2, num3, num2, num3, slider, thumb, horiz);
      bool flag = false;
      if (Event.current.type == EventType.MouseUp)
        flag = true;
      if (EditorGUIExt.ScrollerRepeatButton(id, rect1, leftButton))
        value = value - num1 * ((double) visualStart >= (double) visualEnd ? -1f : 1f);
      if (EditorGUIExt.ScrollerRepeatButton(id, rect2, rightButton))
        value = value + num1 * ((double) visualStart >= (double) visualEnd ? -1f : 1f);
      if (flag && Event.current.type == EventType.Used)
        EditorGUIExt.scrollControlID = 0;
      if ((double) startLimit < (double) endLimit)
        value = Mathf.Clamp(value, startLimit, endLimit - size);
      else
        value = Mathf.Clamp(value, endLimit, startLimit - size);
    }

    public static void MinMaxSlider(Rect position, ref float value, ref float size, float visualStart, float visualEnd, float startLimit, float endLimit, GUIStyle slider, GUIStyle thumb, bool horiz)
    {
      EditorGUIExt.DoMinMaxSlider(position, GUIUtility.GetControlID(EditorGUIExt.s_MinMaxSliderHash, FocusType.Passive), ref value, ref size, visualStart, visualEnd, startLimit, endLimit, slider, thumb, horiz);
    }

    internal static void DoMinMaxSlider(Rect position, int id, ref float value, ref float size, float visualStart, float visualEnd, float startLimit, float endLimit, GUIStyle slider, GUIStyle thumb, bool horiz)
    {
      Event current = Event.current;
      bool flag = (double) size == 0.0;
      float min1 = Mathf.Min(visualStart, visualEnd);
      float max = Mathf.Max(visualStart, visualEnd);
      float min2 = Mathf.Min(startLimit, endLimit);
      float num1 = Mathf.Max(startLimit, endLimit);
      EditorGUIExt.MinMaxSliderState minMaxSliderState = EditorGUIExt.s_MinMaxSliderState;
      if (GUIUtility.hotControl == id && minMaxSliderState != null)
      {
        min1 = minMaxSliderState.dragStartLimit;
        min2 = minMaxSliderState.dragStartLimit;
        max = minMaxSliderState.dragEndLimit;
        num1 = minMaxSliderState.dragEndLimit;
      }
      float num2 = 0.0f;
      float num3 = Mathf.Clamp(value, min1, max);
      float num4 = Mathf.Clamp(value + size, min1, max) - num3;
      float num5 = (double) visualStart <= (double) visualEnd ? 1f : -1f;
      if (slider == null || thumb == null)
        return;
      float num6;
      Rect position1;
      Rect rect1;
      Rect rect2;
      float num7;
      if (horiz)
      {
        float num8 = (double) thumb.fixedWidth == 0.0 ? (float) thumb.padding.horizontal : thumb.fixedWidth;
        num6 = (float) (((double) position.width - (double) slider.padding.horizontal - (double) num8) / ((double) max - (double) min1));
        position1 = new Rect((num3 - min1) * num6 + position.x + (float) slider.padding.left, position.y + (float) slider.padding.top, num4 * num6 + num8, position.height - (float) slider.padding.vertical);
        rect1 = new Rect(position1.x, position1.y, (float) thumb.padding.left, position1.height);
        rect2 = new Rect(position1.xMax - (float) thumb.padding.right, position1.y, (float) thumb.padding.right, position1.height);
        num7 = current.mousePosition.x - position.x;
      }
      else
      {
        float num8 = (double) thumb.fixedHeight == 0.0 ? (float) thumb.padding.vertical : thumb.fixedHeight;
        num6 = (float) (((double) position.height - (double) slider.padding.vertical - (double) num8) / ((double) max - (double) min1));
        position1 = new Rect(position.x + (float) slider.padding.left, (num3 - min1) * num6 + position.y + (float) slider.padding.top, position.width - (float) slider.padding.horizontal, num4 * num6 + num8);
        rect1 = new Rect(position1.x, position1.y, position1.width, (float) thumb.padding.top);
        rect2 = new Rect(position1.x, position1.yMax - (float) thumb.padding.bottom, position1.width, (float) thumb.padding.bottom);
        num7 = current.mousePosition.y - position.y;
      }
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (!position.Contains(current.mousePosition) || (double) min1 - (double) max == 0.0)
            break;
          if (minMaxSliderState == null)
            minMaxSliderState = EditorGUIExt.s_MinMaxSliderState = new EditorGUIExt.MinMaxSliderState();
          minMaxSliderState.dragStartLimit = startLimit;
          minMaxSliderState.dragEndLimit = endLimit;
          if (position1.Contains(current.mousePosition))
          {
            minMaxSliderState.dragStartPos = num7;
            minMaxSliderState.dragStartValue = value;
            minMaxSliderState.dragStartSize = size;
            minMaxSliderState.dragStartValuesPerPixel = num6;
            minMaxSliderState.whereWeDrag = !rect1.Contains(current.mousePosition) ? (!rect2.Contains(current.mousePosition) ? 0 : 2) : 1;
            GUIUtility.hotControl = id;
            current.Use();
            break;
          }
          if (slider == GUIStyle.none)
            break;
          if ((double) size != 0.0 && flag)
          {
            value = !horiz ? ((double) num7 <= (double) position1.yMax - (double) position.y ? value - (float) ((double) size * (double) num5 * 0.899999976158142) : value + (float) ((double) size * (double) num5 * 0.899999976158142)) : ((double) num7 <= (double) position1.xMax - (double) position.x ? value - (float) ((double) size * (double) num5 * 0.899999976158142) : value + (float) ((double) size * (double) num5 * 0.899999976158142));
            minMaxSliderState.whereWeDrag = 0;
            GUI.changed = true;
            EditorGUIExt.s_NextScrollStepTime = DateTime.Now.AddMilliseconds((double) EditorGUIExt.kFirstScrollWait);
            float num8 = !horiz ? current.mousePosition.y : current.mousePosition.x;
            float num9 = !horiz ? position1.y : position1.x;
            minMaxSliderState.whereWeDrag = (double) num8 <= (double) num9 ? 3 : 4;
          }
          else
          {
            value = !horiz ? (float) (((double) num7 - (double) position1.height * 0.5) / (double) num6 + (double) min1 - (double) size * 0.5) : (float) (((double) num7 - (double) position1.width * 0.5) / (double) num6 + (double) min1 - (double) size * 0.5);
            minMaxSliderState.dragStartPos = num7;
            minMaxSliderState.dragStartValue = value;
            minMaxSliderState.dragStartSize = size;
            minMaxSliderState.dragStartValuesPerPixel = num6;
            minMaxSliderState.whereWeDrag = 0;
            GUI.changed = true;
          }
          GUIUtility.hotControl = id;
          value = Mathf.Clamp(value, min2, num1 - size);
          current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != id)
            break;
          current.Use();
          GUIUtility.hotControl = 0;
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != id)
            break;
          float num10 = (num7 - minMaxSliderState.dragStartPos) / minMaxSliderState.dragStartValuesPerPixel;
          switch (minMaxSliderState.whereWeDrag)
          {
            case 0:
              value = Mathf.Clamp(minMaxSliderState.dragStartValue + num10, min2, num1 - size);
              break;
            case 1:
              value = minMaxSliderState.dragStartValue + num10;
              size = minMaxSliderState.dragStartSize - num10;
              if ((double) value < (double) min2)
              {
                size = size - (min2 - value);
                value = min2;
              }
              if ((double) size < (double) num2)
              {
                value = value - (num2 - size);
                size = num2;
                break;
              }
              break;
            case 2:
              size = minMaxSliderState.dragStartSize + num10;
              if ((double) value + (double) size > (double) num1)
                size = num1 - value;
              if ((double) size < (double) num2)
              {
                size = num2;
                break;
              }
              break;
          }
          GUI.changed = true;
          current.Use();
          break;
        case EventType.Repaint:
          slider.Draw(position, GUIContent.none, id);
          thumb.Draw(position1, GUIContent.none, id);
          if (GUIUtility.hotControl != id || !position.Contains(current.mousePosition) || (double) min1 - (double) max == 0.0)
            break;
          if (position1.Contains(current.mousePosition))
          {
            if (minMaxSliderState == null || minMaxSliderState.whereWeDrag != 3 && minMaxSliderState.whereWeDrag != 4)
              break;
            GUIUtility.hotControl = 0;
            break;
          }
          if (DateTime.Now < EditorGUIExt.s_NextScrollStepTime || ((!horiz ? (double) current.mousePosition.y : (double) current.mousePosition.x) <= (!horiz ? (double) position1.y : (double) position1.x) ? 3 : 4) != minMaxSliderState.whereWeDrag)
            break;
          if ((double) size != 0.0 && flag)
          {
            value = !horiz ? ((double) num7 <= (double) position1.yMax - (double) position.y ? value - (float) ((double) size * (double) num5 * 0.899999976158142) : value + (float) ((double) size * (double) num5 * 0.899999976158142)) : ((double) num7 <= (double) position1.xMax - (double) position.x ? value - (float) ((double) size * (double) num5 * 0.899999976158142) : value + (float) ((double) size * (double) num5 * 0.899999976158142));
            minMaxSliderState.whereWeDrag = -1;
            GUI.changed = true;
          }
          value = Mathf.Clamp(value, min2, num1 - size);
          EditorGUIExt.s_NextScrollStepTime = DateTime.Now.AddMilliseconds((double) EditorGUIExt.kScrollWait);
          break;
      }
    }

    public static bool DragSelection(Rect[] positions, ref bool[] selections, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(34553287, FocusType.Keyboard);
      Event current = Event.current;
      int b = -1;
      for (int index = positions.Length - 1; index >= 0; --index)
      {
        if (positions[index].Contains(current.mousePosition))
        {
          b = index;
          break;
        }
      }
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (current.button == 0 && b >= 0)
          {
            GUIUtility.keyboardControl = 0;
            bool flag1 = false;
            if (selections[b])
            {
              int num = 0;
              foreach (bool flag2 in selections)
              {
                if (flag2)
                {
                  ++num;
                  if (num > 1)
                    break;
                }
              }
              if (num == 1)
                flag1 = true;
            }
            if (!current.shift && !EditorGUI.actionKey)
            {
              for (int index = 0; index < positions.Length; ++index)
                selections[index] = false;
            }
            EditorGUIExt.initIndex = b;
            EditorGUIExt.initSelections = (bool[]) selections.Clone();
            EditorGUIExt.adding = true;
            if ((current.shift || EditorGUI.actionKey) && selections[b])
              EditorGUIExt.adding = false;
            selections[b] = !flag1 && EditorGUIExt.adding;
            GUIUtility.hotControl = controlId;
            current.Use();
            return true;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId && current.button == 0)
          {
            if (b < 0)
            {
              Rect rect = new Rect(positions[0].x, positions[0].y - 200f, positions[0].width, 200f);
              if (rect.Contains(current.mousePosition))
                b = 0;
              rect.y = positions[positions.Length - 1].yMax;
              if (rect.Contains(current.mousePosition))
                b = selections.Length - 1;
            }
            if (b < 0)
              return false;
            int num1 = Mathf.Min(EditorGUIExt.initIndex, b);
            int num2 = Mathf.Max(EditorGUIExt.initIndex, b);
            for (int index = 0; index < selections.Length; ++index)
              selections[index] = index < num1 || index > num2 ? EditorGUIExt.initSelections[index] : EditorGUIExt.adding;
            current.Use();
            return true;
          }
          break;
        case EventType.Repaint:
          for (int index = 0; index < positions.Length; ++index)
            style.Draw(positions[index], GUIContent.none, controlId, selections[index]);
          break;
      }
      return false;
    }

    private static bool Any(bool[] selections)
    {
      for (int index = 0; index < selections.Length; ++index)
      {
        if (selections[index])
          return true;
      }
      return false;
    }

    public static HighLevelEvent MultiSelection(Rect rect, Rect[] positions, GUIContent content, Rect[] hitPositions, ref bool[] selections, bool[] readOnly, out int clickedIndex, out Vector2 offset, out float startSelect, out float endSelect, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(41623453, FocusType.Keyboard);
      Event current = Event.current;
      offset = Vector2.zero;
      clickedIndex = -1;
      startSelect = endSelect = 0.0f;
      if (current.type == EventType.Used)
        return HighLevelEvent.None;
      bool flag1 = false;
      if (Event.current.type != EventType.Layout)
      {
        if (GUIUtility.keyboardControl == controlId)
          flag1 = true;
        else
          selections = new bool[selections.Length];
      }
      EventType typeForControl = current.GetTypeForControl(controlId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          if (current.button == 0)
          {
            GUIUtility.hotControl = controlId;
            GUIUtility.keyboardControl = controlId;
            EditorGUIExt.s_StartSelectPos = current.mousePosition;
            int indexUnderMouse = EditorGUIExt.GetIndexUnderMouse(hitPositions, readOnly);
            if (Event.current.clickCount == 2 && indexUnderMouse >= 0)
            {
              for (int index = 0; index < selections.Length; ++index)
                selections[index] = false;
              selections[indexUnderMouse] = true;
              current.Use();
              clickedIndex = indexUnderMouse;
              return HighLevelEvent.DoubleClick;
            }
            if (indexUnderMouse >= 0)
            {
              if (!current.shift && !EditorGUI.actionKey && !selections[indexUnderMouse])
              {
                for (int index = 0; index < hitPositions.Length; ++index)
                  selections[index] = false;
              }
              selections[indexUnderMouse] = !current.shift && !EditorGUI.actionKey || !selections[indexUnderMouse];
              EditorGUIExt.s_MouseDownPos = current.mousePosition;
              EditorGUIExt.s_MultiSelectDragSelection = EditorGUIExt.DragSelectionState.None;
              current.Use();
              clickedIndex = indexUnderMouse;
              return HighLevelEvent.SelectionChanged;
            }
            bool flag2;
            if (!current.shift && !EditorGUI.actionKey)
            {
              for (int index = 0; index < hitPositions.Length; ++index)
                selections[index] = false;
              flag2 = true;
            }
            else
              flag2 = false;
            EditorGUIExt.s_SelectionBackup = new List<bool>((IEnumerable<bool>) selections);
            EditorGUIExt.s_LastFrameSelections = new List<bool>((IEnumerable<bool>) selections);
            EditorGUIExt.s_MultiSelectDragSelection = EditorGUIExt.DragSelectionState.DragSelecting;
            current.Use();
            return flag2 ? HighLevelEvent.SelectionChanged : HighLevelEvent.None;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            if (EditorGUIExt.s_StartSelectPos != current.mousePosition)
              current.Use();
            if (EditorGUIExt.s_MultiSelectDragSelection == EditorGUIExt.DragSelectionState.None)
            {
              clickedIndex = EditorGUIExt.GetIndexUnderMouse(hitPositions, readOnly);
              if (current.clickCount == 1)
                return HighLevelEvent.Click;
              break;
            }
            EditorGUIExt.s_MultiSelectDragSelection = EditorGUIExt.DragSelectionState.None;
            EditorGUIExt.s_SelectionBackup = (List<bool>) null;
            EditorGUIExt.s_LastFrameSelections = (List<bool>) null;
            return HighLevelEvent.EndDrag;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            if (EditorGUIExt.s_MultiSelectDragSelection == EditorGUIExt.DragSelectionState.DragSelecting)
            {
              float num1 = Mathf.Min(EditorGUIExt.s_StartSelectPos.x, current.mousePosition.x);
              float num2 = Mathf.Max(EditorGUIExt.s_StartSelectPos.x, current.mousePosition.x);
              EditorGUIExt.s_SelectionBackup.CopyTo(selections);
              for (int index = 0; index < hitPositions.Length; ++index)
              {
                if (!selections[index])
                {
                  float num3 = hitPositions[index].x + hitPositions[index].width * 0.5f;
                  if ((double) num3 >= (double) num1 && (double) num3 <= (double) num2)
                    selections[index] = true;
                }
              }
              current.Use();
              startSelect = num1;
              endSelect = num2;
              bool flag2 = false;
              for (int index = 0; index < selections.Length; ++index)
              {
                if (selections[index] != EditorGUIExt.s_LastFrameSelections[index])
                {
                  flag2 = true;
                  EditorGUIExt.s_LastFrameSelections[index] = selections[index];
                }
              }
              return flag2 ? HighLevelEvent.SelectionChanged : HighLevelEvent.None;
            }
            offset = current.mousePosition - EditorGUIExt.s_MouseDownPos;
            current.Use();
            if (EditorGUIExt.s_MultiSelectDragSelection != EditorGUIExt.DragSelectionState.None)
              return HighLevelEvent.Drag;
            EditorGUIExt.s_MultiSelectDragSelection = EditorGUIExt.DragSelectionState.Dragging;
            return HighLevelEvent.BeginDrag;
          }
          break;
        case EventType.KeyDown:
          if (flag1 && (current.keyCode == KeyCode.Backspace || current.keyCode == KeyCode.Delete))
          {
            current.Use();
            return HighLevelEvent.Delete;
          }
          break;
        case EventType.Repaint:
          if (GUIUtility.hotControl == controlId && EditorGUIExt.s_MultiSelectDragSelection == EditorGUIExt.DragSelectionState.DragSelecting)
          {
            float num1 = Mathf.Min(EditorGUIExt.s_StartSelectPos.x, current.mousePosition.x);
            float num2 = Mathf.Max(EditorGUIExt.s_StartSelectPos.x, current.mousePosition.x);
            Rect position = new Rect(0.0f, 0.0f, rect.width, rect.height);
            position.x = num1;
            position.width = num2 - num1;
            if ((double) position.width != 0.0)
              GUI.Box(position, string.Empty, EditorGUIExt.ms_Styles.selectionRect);
          }
          Color color = GUI.color;
          for (int index = 0; index < positions.Length; ++index)
          {
            GUI.color = readOnly == null || !readOnly[index] ? (!selections[index] ? color * new Color(0.9f, 0.9f, 0.9f, 1f) : color * new Color(0.3f, 0.55f, 0.95f, 1f)) : color * new Color(0.9f, 0.9f, 0.9f, 0.5f);
            style.Draw(positions[index], content, controlId, selections[index]);
          }
          GUI.color = color;
          break;
        default:
          switch (typeForControl - 13)
          {
            case EventType.MouseDown:
            case EventType.MouseUp:
              if (flag1)
              {
                bool flag2 = current.type == EventType.ExecuteCommand;
                string commandName = current.commandName;
                if (commandName != null)
                {
                  // ISSUE: reference to a compiler-generated field
                  if (EditorGUIExt.\u003C\u003Ef__switch\u0024mapE == null)
                  {
                    // ISSUE: reference to a compiler-generated field
                    EditorGUIExt.\u003C\u003Ef__switch\u0024mapE = new Dictionary<string, int>(1)
                    {
                      {
                        "Delete",
                        0
                      }
                    };
                  }
                  int num;
                  // ISSUE: reference to a compiler-generated field
                  if (EditorGUIExt.\u003C\u003Ef__switch\u0024mapE.TryGetValue(commandName, out num) && num == 0)
                  {
                    current.Use();
                    if (flag2)
                      return HighLevelEvent.Delete;
                    break;
                  }
                  break;
                }
                break;
              }
              break;
            case EventType.MouseDrag:
              int indexUnderMouse1 = EditorGUIExt.GetIndexUnderMouse(hitPositions, readOnly);
              if (indexUnderMouse1 >= 0)
              {
                clickedIndex = indexUnderMouse1;
                GUIUtility.keyboardControl = controlId;
                current.Use();
                return HighLevelEvent.ContextClick;
              }
              break;
          }
      }
      return HighLevelEvent.None;
    }

    private static int GetIndexUnderMouse(Rect[] hitPositions, bool[] readOnly)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      for (int index = hitPositions.Length - 1; index >= 0; --index)
      {
        if ((readOnly == null || !readOnly[index]) && hitPositions[index].Contains(mousePosition))
          return index;
      }
      return -1;
    }

    internal static Rect FromToRect(Vector2 start, Vector2 end)
    {
      Rect rect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
      if ((double) rect.width < 0.0)
      {
        rect.x += rect.width;
        rect.width = -rect.width;
      }
      if ((double) rect.height < 0.0)
      {
        rect.y += rect.height;
        rect.height = -rect.height;
      }
      return rect;
    }

    private class Styles
    {
      public GUIStyle selectionRect = (GUIStyle) "SelectionRect";
    }

    private class MinMaxSliderState
    {
      public int whereWeDrag = -1;
      public float dragStartPos;
      public float dragStartValue;
      public float dragStartSize;
      public float dragStartValuesPerPixel;
      public float dragStartLimit;
      public float dragEndLimit;
    }

    private enum DragSelectionState
    {
      None,
      DragSelecting,
      Dragging,
    }
  }
}
