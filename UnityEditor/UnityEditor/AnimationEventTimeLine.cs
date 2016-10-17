// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationEventTimeLine
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class AnimationEventTimeLine
  {
    private int m_HoverEvent = -1;
    private Vector2 m_InstantTooltipPoint = Vector2.zero;
    [NonSerialized]
    private AnimationEvent[] m_EventsAtMouseDown;
    [NonSerialized]
    private float[] m_EventTimes;
    private bool[] m_EventsSelected;
    private bool m_DirtyTooltip;
    private string m_InstantTooltipText;
    private EditorWindow m_Owner;

    public AnimationEventTimeLine(EditorWindow owner)
    {
      this.m_Owner = owner;
    }

    public void AddEvent(AnimationWindowState state)
    {
      float time = (float) state.frame / state.frameRate;
      int index = AnimationEventPopup.Create(state.activeRootGameObject, state.activeAnimationClip, time, this.m_Owner);
      this.Select(state.activeAnimationClip, index);
    }

    public void DeselectAll()
    {
      this.m_EventsSelected = (bool[]) null;
    }

    private void Select(AnimationClip clip, int index)
    {
      this.m_EventsSelected = new bool[AnimationUtility.GetAnimationEvents(clip).Length];
      this.m_EventsSelected[index] = true;
    }

    public void EventLineGUI(Rect rect, AnimationWindowState state)
    {
      AnimationClip activeAnimationClip = state.activeAnimationClip;
      GameObject activeRootGameObject = state.activeRootGameObject;
      GUI.BeginGroup(rect);
      Color color = GUI.color;
      Rect rect1 = new Rect(0.0f, 0.0f, rect.width, rect.height);
      float time = (float) Mathf.RoundToInt(state.PixelToTime(Event.current.mousePosition.x, rect) * state.frameRate) / state.frameRate;
      if ((UnityEngine.Object) activeAnimationClip != (UnityEngine.Object) null)
      {
        AnimationEvent[] animationEvents = AnimationUtility.GetAnimationEvents(activeAnimationClip);
        Texture image = EditorGUIUtility.IconContent("Animation.EventMarker").image;
        Rect[] rectArray = new Rect[animationEvents.Length];
        Rect[] positions = new Rect[animationEvents.Length];
        int num1 = 1;
        int num2 = 0;
        for (int index = 0; index < animationEvents.Length; ++index)
        {
          AnimationEvent animationEvent = animationEvents[index];
          if (num2 == 0)
          {
            num1 = 1;
            while (index + num1 < animationEvents.Length && (double) animationEvents[index + num1].time == (double) animationEvent.time)
              ++num1;
            num2 = num1;
          }
          --num2;
          float num3 = Mathf.Floor(state.FrameToPixel(animationEvent.time * activeAnimationClip.frameRate, rect));
          int num4 = 0;
          if (num1 > 1)
            num4 = Mathf.FloorToInt(Mathf.Max(0.0f, (float) Mathf.Min((num1 - 1) * (image.width - 1), (int) ((double) state.FrameDeltaToPixel(rect) - (double) (image.width * 2))) - (float) ((image.width - 1) * num2)));
          Rect rect2 = new Rect(num3 + (float) num4 - (float) (image.width / 2), (rect.height - 10f) * (float) (num2 - num1 + 1) / (float) Mathf.Max(1, num1 - 1), (float) image.width, (float) image.height);
          rectArray[index] = rect2;
          positions[index] = rect2;
        }
        if (this.m_DirtyTooltip)
        {
          if (this.m_HoverEvent >= 0 && this.m_HoverEvent < rectArray.Length)
          {
            this.m_InstantTooltipText = AnimationEventPopup.FormatEvent(activeRootGameObject, animationEvents[this.m_HoverEvent]);
            this.m_InstantTooltipPoint = new Vector2((float) ((double) rectArray[this.m_HoverEvent].xMin + (double) (int) ((double) rectArray[this.m_HoverEvent].width / 2.0) + (double) rect.x - 30.0), rect.yMax);
          }
          this.m_DirtyTooltip = false;
        }
        if (this.m_EventsSelected == null || this.m_EventsSelected.Length != animationEvents.Length)
        {
          this.m_EventsSelected = new bool[animationEvents.Length];
          AnimationEventPopup.ClosePopup();
        }
        Vector2 offset = Vector2.zero;
        int clickedIndex;
        float startSelect;
        float endSelect;
        switch (EditorGUIExt.MultiSelection(rect, positions, new GUIContent(image), rectArray, ref this.m_EventsSelected, (bool[]) null, out clickedIndex, out offset, out startSelect, out endSelect, GUIStyle.none))
        {
          case HighLevelEvent.DoubleClick:
            if (clickedIndex != -1)
            {
              AnimationEventPopup.Edit(activeRootGameObject, state.activeAnimationClip, clickedIndex, this.m_Owner);
              break;
            }
            this.EventLineContextMenuAdd((object) new AnimationEventTimeLine.EventLineContextMenuObject(activeRootGameObject, activeAnimationClip, time, -1));
            break;
          case HighLevelEvent.ContextClick:
            GenericMenu genericMenu = new GenericMenu();
            AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject = new AnimationEventTimeLine.EventLineContextMenuObject(activeRootGameObject, activeAnimationClip, animationEvents[clickedIndex].time, clickedIndex);
            genericMenu.AddItem(new GUIContent("Edit Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuEdit), (object) contextMenuObject);
            genericMenu.AddItem(new GUIContent("Add Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuAdd), (object) contextMenuObject);
            genericMenu.AddItem(new GUIContent("Delete Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuDelete), (object) contextMenuObject);
            genericMenu.ShowAsContext();
            this.m_InstantTooltipText = (string) null;
            this.m_DirtyTooltip = true;
            state.Repaint();
            break;
          case HighLevelEvent.BeginDrag:
            this.m_EventsAtMouseDown = animationEvents;
            this.m_EventTimes = new float[animationEvents.Length];
            for (int index = 0; index < animationEvents.Length; ++index)
              this.m_EventTimes[index] = animationEvents[index].time;
            break;
          case HighLevelEvent.Drag:
            for (int index = animationEvents.Length - 1; index >= 0; --index)
            {
              if (this.m_EventsSelected[index])
              {
                AnimationEvent animationEvent = this.m_EventsAtMouseDown[index];
                animationEvent.time = this.m_EventTimes[index] + offset.x * state.PixelDeltaToTime(rect);
                animationEvent.time = Mathf.Max(0.0f, animationEvent.time);
                animationEvent.time = (float) Mathf.RoundToInt(animationEvent.time * activeAnimationClip.frameRate) / activeAnimationClip.frameRate;
              }
            }
            int[] numArray1 = new int[this.m_EventsSelected.Length];
            for (int index = 0; index < numArray1.Length; ++index)
              numArray1[index] = index;
            Array.Sort((Array) this.m_EventsAtMouseDown, (Array) numArray1, (IComparer) new AnimationEventTimeLine.EventComparer());
            bool[] flagArray = (bool[]) this.m_EventsSelected.Clone();
            float[] numArray2 = (float[]) this.m_EventTimes.Clone();
            for (int index = 0; index < numArray1.Length; ++index)
            {
              this.m_EventsSelected[index] = flagArray[numArray1[index]];
              this.m_EventTimes[index] = numArray2[numArray1[index]];
            }
            Undo.RegisterCompleteObjectUndo((UnityEngine.Object) activeAnimationClip, "Move Event");
            AnimationUtility.SetAnimationEvents(activeAnimationClip, this.m_EventsAtMouseDown);
            this.m_DirtyTooltip = true;
            break;
          case HighLevelEvent.Delete:
            this.DeleteEvents(activeAnimationClip, this.m_EventsSelected);
            break;
          case HighLevelEvent.SelectionChanged:
            state.ClearKeySelections();
            if (clickedIndex != -1)
            {
              AnimationEventPopup.UpdateSelection(activeRootGameObject, state.activeAnimationClip, clickedIndex, this.m_Owner);
              break;
            }
            break;
        }
        this.CheckRectsOnMouseMove(rect, animationEvents, rectArray);
      }
      if (Event.current.type == EventType.ContextClick && rect1.Contains(Event.current.mousePosition))
      {
        Event.current.Use();
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Animation Event"), false, new GenericMenu.MenuFunction2(this.EventLineContextMenuAdd), (object) new AnimationEventTimeLine.EventLineContextMenuObject(activeRootGameObject, activeAnimationClip, time, -1));
        genericMenu.ShowAsContext();
      }
      GUI.color = color;
      GUI.EndGroup();
    }

    public void DrawInstantTooltip(Rect position)
    {
      if (this.m_InstantTooltipText == null || !(this.m_InstantTooltipText != string.Empty))
        return;
      GUIStyle style = (GUIStyle) "AnimationEventTooltip";
      style.contentOffset = new Vector2(0.0f, 0.0f);
      style.overflow = new RectOffset(10, 10, 0, 0);
      Vector2 vector2 = style.CalcSize(new GUIContent(this.m_InstantTooltipText));
      Rect position1 = new Rect(this.m_InstantTooltipPoint.x - vector2.x * 0.5f, this.m_InstantTooltipPoint.y + 24f, vector2.x, vector2.y);
      if ((double) position1.xMax > (double) position.width)
        position1.x = position.width - position1.width;
      GUI.Label(position1, this.m_InstantTooltipText, style);
      position1 = new Rect(this.m_InstantTooltipPoint.x - 33f, this.m_InstantTooltipPoint.y, 7f, 25f);
      GUI.Label(position1, string.Empty, (GUIStyle) "AnimationEventTooltipArrow");
    }

    private void DeleteEvents(AnimationClip clip, bool[] deleteIndices)
    {
      bool flag = false;
      List<AnimationEvent> animationEventList = new List<AnimationEvent>((IEnumerable<AnimationEvent>) AnimationUtility.GetAnimationEvents(clip));
      for (int index = animationEventList.Count - 1; index >= 0; --index)
      {
        if (deleteIndices[index])
        {
          animationEventList.RemoveAt(index);
          flag = true;
        }
      }
      if (!flag)
        return;
      AnimationEventPopup.ClosePopup();
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) clip, "Delete Event");
      AnimationUtility.SetAnimationEvents(clip, animationEventList.ToArray());
      this.m_EventsSelected = new bool[animationEventList.Count];
      this.m_DirtyTooltip = true;
    }

    public void EventLineContextMenuAdd(object obj)
    {
      AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject = (AnimationEventTimeLine.EventLineContextMenuObject) obj;
      int index = AnimationEventPopup.Create(contextMenuObject.m_Animated, contextMenuObject.m_Clip, contextMenuObject.m_Time, this.m_Owner);
      this.Select(contextMenuObject.m_Clip, index);
      this.m_EventsSelected = new bool[AnimationUtility.GetAnimationEvents(contextMenuObject.m_Clip).Length];
      this.m_EventsSelected[index] = true;
    }

    public void EventLineContextMenuEdit(object obj)
    {
      AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject = (AnimationEventTimeLine.EventLineContextMenuObject) obj;
      AnimationEventPopup.Edit(contextMenuObject.m_Animated, contextMenuObject.m_Clip, contextMenuObject.m_Index, this.m_Owner);
      this.Select(contextMenuObject.m_Clip, contextMenuObject.m_Index);
    }

    public void EventLineContextMenuDelete(object obj)
    {
      AnimationEventTimeLine.EventLineContextMenuObject contextMenuObject = (AnimationEventTimeLine.EventLineContextMenuObject) obj;
      AnimationClip clip = contextMenuObject.m_Clip;
      if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        return;
      int index = contextMenuObject.m_Index;
      if (this.m_EventsSelected[index])
      {
        this.DeleteEvents(clip, this.m_EventsSelected);
      }
      else
      {
        bool[] deleteIndices = new bool[this.m_EventsSelected.Length];
        deleteIndices[index] = true;
        this.DeleteEvents(clip, deleteIndices);
      }
    }

    private void CheckRectsOnMouseMove(Rect eventLineRect, AnimationEvent[] events, Rect[] hitRects)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      bool flag = false;
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
              this.m_InstantTooltipPoint = new Vector2(hitRects[this.m_HoverEvent].xMin + (float) (int) ((double) hitRects[this.m_HoverEvent].width / 2.0) + eventLineRect.x, eventLineRect.yMax);
              this.m_DirtyTooltip = true;
            }
          }
        }
      }
      if (flag)
        return;
      this.m_HoverEvent = -1;
      this.m_InstantTooltipText = string.Empty;
    }

    public class EventComparer : IComparer
    {
      int IComparer.Compare(object objX, object objY)
      {
        AnimationEvent animationEvent1 = (AnimationEvent) objX;
        AnimationEvent animationEvent2 = (AnimationEvent) objY;
        float time1 = animationEvent1.time;
        float time2 = animationEvent2.time;
        if ((double) time1 != (double) time2)
          return (int) Mathf.Sign(time1 - time2);
        return animationEvent1.GetHashCode() - animationEvent2.GetHashCode();
      }
    }

    private class EventLineContextMenuObject
    {
      public GameObject m_Animated;
      public AnimationClip m_Clip;
      public float m_Time;
      public int m_Index;

      public EventLineContextMenuObject(GameObject animated, AnimationClip clip, float time, int index)
      {
        this.m_Animated = animated;
        this.m_Clip = clip;
        this.m_Time = time;
        this.m_Index = index;
      }
    }
  }
}
