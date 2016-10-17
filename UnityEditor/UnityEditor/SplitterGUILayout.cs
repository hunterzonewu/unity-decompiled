// Decompiled with JetBrains decompiler
// Type: UnityEditor.SplitterGUILayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SplitterGUILayout
  {
    private static int splitterHash = "Splitter".GetHashCode();

    public static void BeginSplit(SplitterState state, GUIStyle style, bool vertical, params GUILayoutOption[] options)
    {
      SplitterGUILayout.GUISplitterGroup guiSplitterGroup = (SplitterGUILayout.GUISplitterGroup) GUILayoutUtility.BeginLayoutGroup(style, (GUILayoutOption[]) null, typeof (SplitterGUILayout.GUISplitterGroup));
      state.ID = GUIUtility.GetControlID(SplitterGUILayout.splitterHash, FocusType.Native);
      switch (Event.current.GetTypeForControl(state.ID))
      {
        case EventType.MouseDown:
          if (Event.current.button != 0 || Event.current.clickCount != 1)
            break;
          int num1 = !guiSplitterGroup.isVertical ? (int) guiSplitterGroup.rect.x : (int) guiSplitterGroup.rect.y;
          int num2 = !guiSplitterGroup.isVertical ? (int) Event.current.mousePosition.x : (int) Event.current.mousePosition.y;
          for (int index = 0; index < state.relativeSizes.Length - 1; ++index)
          {
            if ((!guiSplitterGroup.isVertical ? new Rect(state.xOffset + (float) num1 + (float) state.realSizes[index] - (float) (state.splitSize / 2), guiSplitterGroup.rect.y, (float) state.splitSize, guiSplitterGroup.rect.height) : new Rect(state.xOffset + guiSplitterGroup.rect.x, (float) (num1 + state.realSizes[index] - state.splitSize / 2), guiSplitterGroup.rect.width, (float) state.splitSize)).Contains(Event.current.mousePosition))
            {
              state.splitterInitialOffset = num2;
              state.currentActiveSplitter = index;
              GUIUtility.hotControl = state.ID;
              Event.current.Use();
              break;
            }
            num1 += state.realSizes[index];
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != state.ID)
            break;
          GUIUtility.hotControl = 0;
          state.currentActiveSplitter = -1;
          state.RealToRelativeSizes();
          Event.current.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != state.ID || state.currentActiveSplitter < 0)
            break;
          int num3 = !guiSplitterGroup.isVertical ? (int) Event.current.mousePosition.x : (int) Event.current.mousePosition.y;
          int diff = num3 - state.splitterInitialOffset;
          if (diff != 0)
          {
            state.splitterInitialOffset = num3;
            state.DoSplitter(state.currentActiveSplitter, state.currentActiveSplitter + 1, diff);
          }
          Event.current.Use();
          break;
        case EventType.Repaint:
          int num4 = !guiSplitterGroup.isVertical ? (int) guiSplitterGroup.rect.x : (int) guiSplitterGroup.rect.y;
          for (int index = 0; index < state.relativeSizes.Length - 1; ++index)
          {
            EditorGUIUtility.AddCursorRect(!guiSplitterGroup.isVertical ? new Rect(state.xOffset + (float) num4 + (float) state.realSizes[index] - (float) (state.splitSize / 2), guiSplitterGroup.rect.y, (float) state.splitSize, guiSplitterGroup.rect.height) : new Rect(state.xOffset + guiSplitterGroup.rect.x, (float) (num4 + state.realSizes[index] - state.splitSize / 2), guiSplitterGroup.rect.width, (float) state.splitSize), !guiSplitterGroup.isVertical ? MouseCursor.SplitResizeLeftRight : MouseCursor.ResizeVertical, state.ID);
            num4 += state.realSizes[index];
          }
          break;
        case EventType.Layout:
          guiSplitterGroup.state = state;
          guiSplitterGroup.resetCoords = false;
          guiSplitterGroup.isVertical = vertical;
          guiSplitterGroup.ApplyOptions(options);
          break;
      }
    }

    public static void BeginHorizontalSplit(SplitterState state, params GUILayoutOption[] options)
    {
      SplitterGUILayout.BeginSplit(state, GUIStyle.none, false, options);
    }

    public static void BeginVerticalSplit(SplitterState state, params GUILayoutOption[] options)
    {
      SplitterGUILayout.BeginSplit(state, GUIStyle.none, true, options);
    }

    public static void BeginHorizontalSplit(SplitterState state, GUIStyle style, params GUILayoutOption[] options)
    {
      SplitterGUILayout.BeginSplit(state, style, false, options);
    }

    public static void BeginVerticalSplit(SplitterState state, GUIStyle style, params GUILayoutOption[] options)
    {
      SplitterGUILayout.BeginSplit(state, style, true, options);
    }

    public static void EndVerticalSplit()
    {
      GUILayoutUtility.EndLayoutGroup();
    }

    public static void EndHorizontalSplit()
    {
      GUILayoutUtility.EndLayoutGroup();
    }

    internal class GUISplitterGroup : GUILayoutGroup
    {
      public SplitterState state;

      public override void SetHorizontal(float x, float width)
      {
        if (!this.isVertical)
        {
          this.state.xOffset = x;
          if ((double) width != (double) this.state.lastTotalSize)
          {
            this.state.RelativeToRealSizes((int) width);
            this.state.lastTotalSize = (int) width;
            for (int i1 = 0; i1 < this.state.realSizes.Length - 1; ++i1)
              this.state.DoSplitter(i1, i1 + 1, 0);
          }
          int index = 0;
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              float realSize = (float) this.state.realSizes[index];
              current.SetHorizontal(Mathf.Round(x), Mathf.Round(realSize));
              x += realSize + this.spacing;
              ++index;
            }
          }
        }
        else
          base.SetHorizontal(x, width);
      }

      public override void SetVertical(float y, float height)
      {
        this.rect.y = y;
        this.rect.height = height;
        RectOffset padding = this.style.padding;
        if (this.isVertical)
        {
          if (this.style != GUIStyle.none)
          {
            float a1 = (float) padding.top;
            float a2 = (float) padding.bottom;
            if (this.entries.Count != 0)
            {
              a1 = Mathf.Max(a1, (float) this.entries[0].margin.top);
              a2 = Mathf.Max(a2, (float) this.entries[this.entries.Count - 1].margin.bottom);
            }
            y += a1;
            height -= a2 + a1;
          }
          if ((double) height != (double) this.state.lastTotalSize)
          {
            this.state.RelativeToRealSizes((int) height);
            this.state.lastTotalSize = (int) height;
            for (int i1 = 0; i1 < this.state.realSizes.Length - 1; ++i1)
              this.state.DoSplitter(i1, i1 + 1, 0);
          }
          int index = 0;
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              float realSize = (float) this.state.realSizes[index];
              current.SetVertical(Mathf.Round(y), Mathf.Round(realSize));
              y += realSize + this.spacing;
              ++index;
            }
          }
        }
        else if (this.style != GUIStyle.none)
        {
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              float num = (float) Mathf.Max(current.margin.top, padding.top);
              float y1 = y + num;
              float height1 = height - (float) Mathf.Max(current.margin.bottom, padding.bottom) - num;
              if (current.stretchHeight != 0)
                current.SetVertical(y1, height1);
              else
                current.SetVertical(y1, Mathf.Clamp(height1, current.minHeight, current.maxHeight));
            }
          }
        }
        else
        {
          float num1 = y - (float) this.margin.top;
          float num2 = height + (float) this.margin.vertical;
          using (List<GUILayoutEntry>.Enumerator enumerator = this.entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GUILayoutEntry current = enumerator.Current;
              if (current.stretchHeight != 0)
                current.SetVertical(num1 + (float) current.margin.top, num2 - (float) current.margin.vertical);
              else
                current.SetVertical(num1 + (float) current.margin.top, Mathf.Clamp(num2 - (float) current.margin.vertical, current.minHeight, current.maxHeight));
            }
          }
        }
      }
    }
  }
}
