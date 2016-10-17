// Decompiled with JetBrains decompiler
// Type: UnityEditor.EventManipulationHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using UnityEngine;

namespace UnityEditor
{
  internal class EventManipulationHandler
  {
    private Rect[] m_EventRects = new Rect[0];
    private int m_HoverEvent = -1;
    private Vector2 m_InstantTooltipPoint = Vector2.zero;
    private static AnimationEvent[] m_EventsAtMouseDown;
    private static float[] m_EventTimes;
    private string m_InstantTooltipText;
    private bool[] m_EventsSelected;
    private TimeArea m_Timeline;

    public EventManipulationHandler(TimeArea timeArea)
    {
      this.m_Timeline = timeArea;
    }

    public void SelectEvent(AnimationEvent[] events, int index, AnimationClipInfoProperties clipInfo)
    {
      this.m_EventsSelected = new bool[events.Length];
      this.m_EventsSelected[index] = true;
      AnimationEventPopup.Edit(clipInfo, index);
    }

    public bool HandleEventManipulation(Rect rect, ref AnimationEvent[] events, AnimationClipInfoProperties clipInfo)
    {
      Texture image = EditorGUIUtility.IconContent("Animation.EventMarker").image;
      bool flag = false;
      Rect[] rectArray = new Rect[events.Length];
      Rect[] positions = new Rect[events.Length];
      int num1 = 1;
      int num2 = 0;
      for (int index = 0; index < events.Length; ++index)
      {
        AnimationEvent animationEvent = events[index];
        if (num2 == 0)
        {
          num1 = 1;
          while (index + num1 < events.Length && (double) events[index + num1].time == (double) animationEvent.time)
            ++num1;
          num2 = num1;
        }
        --num2;
        float num3 = Mathf.Floor(this.m_Timeline.TimeToPixel(animationEvent.time, rect));
        int num4 = 0;
        if (num1 > 1)
          num4 = Mathf.FloorToInt(Mathf.Max(0.0f, (float) Mathf.Min((num1 - 1) * (image.width - 1), (int) (1.0 / (double) this.m_Timeline.PixelDeltaToTime(rect) - (double) (image.width * 2))) - (float) ((image.width - 1) * num2)));
        Rect rect1 = new Rect(num3 + (float) num4 - (float) (image.width / 2), (rect.height - 10f) * (float) (num2 - num1 + 1) / (float) Mathf.Max(1, num1 - 1), (float) image.width, (float) image.height);
        rectArray[index] = rect1;
        positions[index] = rect1;
      }
      this.m_EventRects = new Rect[rectArray.Length];
      for (int index = 0; index < rectArray.Length; ++index)
        this.m_EventRects[index] = new Rect(rectArray[index].x + rect.x, rectArray[index].y + rect.y, rectArray[index].width, rectArray[index].height);
      if (this.m_EventsSelected == null || this.m_EventsSelected.Length != events.Length || this.m_EventsSelected.Length == 0)
      {
        this.m_EventsSelected = new bool[events.Length];
        AnimationEventPopup.ClosePopup();
      }
      Vector2 offset = Vector2.zero;
      int clickedIndex;
      float startSelect;
      float endSelect;
      switch (EditorGUIExt.MultiSelection(rect, positions, new GUIContent(image), rectArray, ref this.m_EventsSelected, (bool[]) null, out clickedIndex, out offset, out startSelect, out endSelect, GUIStyle.none))
      {
        case HighLevelEvent.ContextClick:
          GenericMenu genericMenu = new GenericMenu();
          genericMenu.AddItem(new GUIContent("Add Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuAdd), (object) new EventManipulationHandler.EventModificationContextMenuObjet(clipInfo, events[clickedIndex].time, clickedIndex));
          genericMenu.AddItem(new GUIContent("Delete Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuDelete), (object) new EventManipulationHandler.EventModificationContextMenuObjet(clipInfo, events[clickedIndex].time, clickedIndex));
          genericMenu.ShowAsContext();
          this.m_InstantTooltipText = (string) null;
          break;
        case HighLevelEvent.BeginDrag:
          EventManipulationHandler.m_EventsAtMouseDown = events;
          EventManipulationHandler.m_EventTimes = new float[events.Length];
          for (int index = 0; index < events.Length; ++index)
            EventManipulationHandler.m_EventTimes[index] = events[index].time;
          break;
        case HighLevelEvent.Drag:
          for (int index = events.Length - 1; index >= 0; --index)
          {
            if (this.m_EventsSelected[index])
              EventManipulationHandler.m_EventsAtMouseDown[index].time = Mathf.Clamp01(EventManipulationHandler.m_EventTimes[index] + offset.x / rect.width);
          }
          int[] numArray1 = new int[this.m_EventsSelected.Length];
          for (int index = 0; index < numArray1.Length; ++index)
            numArray1[index] = index;
          Array.Sort((Array) EventManipulationHandler.m_EventsAtMouseDown, (Array) numArray1, (IComparer) new AnimationEventTimeLine.EventComparer());
          bool[] flagArray = (bool[]) this.m_EventsSelected.Clone();
          float[] numArray2 = (float[]) EventManipulationHandler.m_EventTimes.Clone();
          for (int index = 0; index < numArray1.Length; ++index)
          {
            this.m_EventsSelected[index] = flagArray[numArray1[index]];
            EventManipulationHandler.m_EventTimes[index] = numArray2[numArray1[index]];
          }
          events = EventManipulationHandler.m_EventsAtMouseDown;
          flag = true;
          break;
        case HighLevelEvent.Delete:
          flag = this.DeleteEvents(ref events, this.m_EventsSelected);
          break;
        case HighLevelEvent.SelectionChanged:
          if (clickedIndex >= 0)
          {
            AnimationEventPopup.Edit(clipInfo, clickedIndex);
            break;
          }
          AnimationEventPopup.ClosePopup();
          break;
      }
      this.CheckRectsOnMouseMove(rect, events, rectArray);
      return flag;
    }

    public void EventLineContextMenuAdd(object obj)
    {
      EventManipulationHandler.EventModificationContextMenuObjet contextMenuObjet = (EventManipulationHandler.EventModificationContextMenuObjet) obj;
      contextMenuObjet.m_Info.AddEvent(contextMenuObjet.m_Time);
      this.SelectEvent(contextMenuObjet.m_Info.GetEvents(), contextMenuObjet.m_Info.GetEventCount() - 1, contextMenuObjet.m_Info);
    }

    public void EventLineContextMenuDelete(object obj)
    {
      EventManipulationHandler.EventModificationContextMenuObjet contextMenuObjet = (EventManipulationHandler.EventModificationContextMenuObjet) obj;
      contextMenuObjet.m_Info.RemoveEvent(contextMenuObjet.m_Index);
    }

    private void CheckRectsOnMouseMove(Rect eventLineRect, AnimationEvent[] events, Rect[] hitRects)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      bool flag = false;
      this.m_InstantTooltipText = string.Empty;
      if (events.Length == hitRects.Length)
      {
        for (int index = hitRects.Length - 1; index >= 0; --index)
        {
          if (hitRects[index].Contains(mousePosition))
          {
            flag = true;
            if (this.m_HoverEvent != index)
            {
              this.m_HoverEvent = index;
              this.m_InstantTooltipText = events[this.m_HoverEvent].functionName;
              this.m_InstantTooltipPoint = new Vector2(mousePosition.x, mousePosition.y);
            }
          }
        }
      }
      if (flag)
        return;
      this.m_HoverEvent = -1;
    }

    public void DrawInstantTooltip(Rect window)
    {
      if (this.m_InstantTooltipText == null || !(this.m_InstantTooltipText != string.Empty))
        return;
      GUIStyle style = (GUIStyle) "AnimationEventTooltip";
      Vector2 vector2 = style.CalcSize(new GUIContent(this.m_InstantTooltipText));
      Rect position = new Rect(window.x + this.m_InstantTooltipPoint.x, window.y + this.m_InstantTooltipPoint.y, vector2.x, vector2.y);
      if ((double) position.xMax > (double) window.width)
        position.x = window.width - position.width;
      GUI.Label(position, this.m_InstantTooltipText, style);
    }

    private bool DeleteEvents(ref AnimationEvent[] eventList, bool[] deleteIndices)
    {
      bool flag = false;
      for (int index = eventList.Length - 1; index >= 0; --index)
      {
        if (deleteIndices[index])
        {
          ArrayUtility.RemoveAt<AnimationEvent>(ref eventList, index);
          flag = true;
        }
      }
      if (flag)
      {
        AnimationEventPopup.ClosePopup();
        this.m_EventsSelected = new bool[eventList.Length];
      }
      return flag;
    }

    private class EventModificationContextMenuObjet
    {
      public AnimationClipInfoProperties m_Info;
      public float m_Time;
      public int m_Index;

      public EventModificationContextMenuObjet(AnimationClipInfoProperties info, float time, int index)
      {
        this.m_Info = info;
        this.m_Time = time;
        this.m_Index = index;
      }
    }
  }
}
