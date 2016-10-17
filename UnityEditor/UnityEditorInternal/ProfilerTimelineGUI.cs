// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerTimelineGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class ProfilerTimelineGUI
  {
    private float animationTime = 1f;
    private int m_SelectedID = -1;
    private const float kSmallWidth = 7f;
    private const float kTextFadeStartWidth = 50f;
    private const float kTextFadeOutWidth = 20f;
    private const float kTextLongWidth = 200f;
    private const float kLineHeight = 16f;
    private const float kGroupHeight = 20f;
    private double lastScrollUpdate;
    private List<ProfilerTimelineGUI.GroupInfo> groups;
    private static ProfilerTimelineGUI.Styles ms_Styles;
    [NonSerialized]
    private ZoomableArea m_TimeArea;
    private IProfilerWindowController m_Window;
    private int m_SelectedThread;
    private float m_SelectedTime;
    private float m_SelectedDur;

    private static ProfilerTimelineGUI.Styles styles
    {
      get
      {
        return ProfilerTimelineGUI.ms_Styles ?? (ProfilerTimelineGUI.ms_Styles = new ProfilerTimelineGUI.Styles());
      }
    }

    public ProfilerTimelineGUI(IProfilerWindowController window)
    {
      this.m_Window = window;
      this.groups = new List<ProfilerTimelineGUI.GroupInfo>();
    }

    private void CalculateBars(Rect r, int frameIndex, float time)
    {
      ProfilerFrameDataIterator frameDataIterator = new ProfilerFrameDataIterator();
      int groupCount = frameDataIterator.GetGroupCount(frameIndex);
      float num1 = 0.0f;
      frameDataIterator.SetRoot(frameIndex, 0);
      int threadCount = frameDataIterator.GetThreadCount(frameIndex);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ProfilerTimelineGUI.\u003CCalculateBars\u003Ec__AnonStoreyAD barsCAnonStoreyAd = new ProfilerTimelineGUI.\u003CCalculateBars\u003Ec__AnonStoreyAD();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      for (barsCAnonStoreyAd.i = 0; barsCAnonStoreyAd.i < threadCount; barsCAnonStoreyAd.i = barsCAnonStoreyAd.i + 1)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ProfilerTimelineGUI.\u003CCalculateBars\u003Ec__AnonStoreyAC barsCAnonStoreyAc = new ProfilerTimelineGUI.\u003CCalculateBars\u003Ec__AnonStoreyAC();
        // ISSUE: reference to a compiler-generated field
        barsCAnonStoreyAc.\u003C\u003Ef__ref\u0024173 = barsCAnonStoreyAd;
        // ISSUE: reference to a compiler-generated field
        frameDataIterator.SetRoot(frameIndex, barsCAnonStoreyAd.i);
        // ISSUE: reference to a compiler-generated field
        barsCAnonStoreyAc.groupname = frameDataIterator.GetGroupName();
        // ISSUE: reference to a compiler-generated method
        ProfilerTimelineGUI.GroupInfo groupInfo = this.groups.Find(new Predicate<ProfilerTimelineGUI.GroupInfo>(barsCAnonStoreyAc.\u003C\u003Em__1F5));
        if (groupInfo == null)
        {
          groupInfo = new ProfilerTimelineGUI.GroupInfo();
          // ISSUE: reference to a compiler-generated field
          groupInfo.name = barsCAnonStoreyAc.groupname;
          groupInfo.height = 20f;
          groupInfo.expanded = false;
          groupInfo.threads = new List<ProfilerTimelineGUI.ThreadInfo>();
          this.groups.Add(groupInfo);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (barsCAnonStoreyAc.groupname == string.Empty || barsCAnonStoreyAc.groupname == "Unity Job System")
            groupInfo.expanded = true;
        }
        // ISSUE: reference to a compiler-generated method
        ProfilerTimelineGUI.ThreadInfo threadInfo1 = groupInfo.threads.Find(new Predicate<ProfilerTimelineGUI.ThreadInfo>(barsCAnonStoreyAc.\u003C\u003Em__1F6));
        if (threadInfo1 == null)
        {
          threadInfo1 = new ProfilerTimelineGUI.ThreadInfo();
          threadInfo1.name = frameDataIterator.GetThreadName();
          threadInfo1.height = 0.0f;
          ProfilerTimelineGUI.ThreadInfo threadInfo2 = threadInfo1;
          ProfilerTimelineGUI.ThreadInfo threadInfo3 = threadInfo1;
          int num2 = !groupInfo.expanded ? 0 : 1;
          double num3;
          float num4 = (float) (num3 = (double) num2);
          threadInfo3.desiredWeight = (float) num3;
          double num5 = (double) num4;
          threadInfo2.weight = (float) num5;
          // ISSUE: reference to a compiler-generated field
          threadInfo1.threadIndex = barsCAnonStoreyAd.i;
          groupInfo.threads.Add(threadInfo1);
        }
        if ((double) threadInfo1.weight != (double) threadInfo1.desiredWeight)
          threadInfo1.weight = (float) ((double) threadInfo1.desiredWeight * (double) time + (1.0 - (double) threadInfo1.desiredWeight) * (1.0 - (double) time));
        num1 += threadInfo1.weight;
      }
      float num6 = 16f * (float) groupCount;
      float num7 = (r.height - num6) / (num1 + 2f);
      using (List<ProfilerTimelineGUI.GroupInfo>.Enumerator enumerator1 = this.groups.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          using (List<ProfilerTimelineGUI.ThreadInfo>.Enumerator enumerator2 = enumerator1.Current.threads.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              ProfilerTimelineGUI.ThreadInfo current = enumerator2.Current;
              current.height = num7 * current.weight;
            }
          }
        }
      }
      this.groups[0].expanded = true;
      this.groups[0].height = 0.0f;
      this.groups[0].threads[0].height = 3f * num7;
    }

    private void UpdateAnimatedFoldout()
    {
      this.animationTime = Math.Min(1f, this.animationTime + (float) (EditorApplication.timeSinceStartup - this.lastScrollUpdate));
      this.m_Window.Repaint();
      if ((double) this.animationTime != 1.0)
        return;
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.UpdateAnimatedFoldout);
    }

    private bool DrawBar(Rect r, float y, float height, string name, bool group, bool expanded, bool indent)
    {
      Rect position1 = new Rect(r.x - 170f, y, 170f, height);
      Rect position2 = new Rect(r.x, y, r.width, height);
      if (Event.current.type == EventType.Repaint)
      {
        ProfilerTimelineGUI.styles.rightPane.Draw(position2, false, false, false, false);
        bool flag1 = (double) height < 10.0;
        bool flag2 = (double) height < 25.0;
        GUIContent content = group || flag1 ? GUIContent.none : GUIContent.Temp(name);
        if (flag2)
          ProfilerTimelineGUI.styles.leftPane.padding.top -= (int) (25.0 - (double) height) / 2;
        if (indent)
          ProfilerTimelineGUI.styles.leftPane.padding.left += 10;
        ProfilerTimelineGUI.styles.leftPane.Draw(position1, content, false, false, false, false);
        if (indent)
          ProfilerTimelineGUI.styles.leftPane.padding.left -= 10;
        if (flag2)
          ProfilerTimelineGUI.styles.leftPane.padding.top += (int) (25.0 - (double) height) / 2;
      }
      if (!group)
        return false;
      --position1.width;
      ++position1.xMin;
      return GUI.Toggle(position1, expanded, GUIContent.Temp(name), ProfilerTimelineGUI.styles.foldout);
    }

    private void DrawBars(Rect r, int frameIndex)
    {
      float y = r.y;
      using (List<ProfilerTimelineGUI.GroupInfo>.Enumerator enumerator1 = this.groups.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          ProfilerTimelineGUI.GroupInfo current1 = enumerator1.Current;
          bool flag = current1.name == string.Empty;
          if (!flag)
          {
            float height = current1.height;
            bool expanded = current1.expanded;
            current1.expanded = this.DrawBar(r, y, height, current1.name, true, expanded, false);
            if (current1.expanded != expanded)
            {
              this.animationTime = 0.0f;
              this.lastScrollUpdate = EditorApplication.timeSinceStartup;
              EditorApplication.update += new EditorApplication.CallbackFunction(this.UpdateAnimatedFoldout);
              using (List<ProfilerTimelineGUI.ThreadInfo>.Enumerator enumerator2 = current1.threads.GetEnumerator())
              {
                while (enumerator2.MoveNext())
                  enumerator2.Current.desiredWeight = !current1.expanded ? 0.0f : 1f;
              }
            }
            y += height;
          }
          using (List<ProfilerTimelineGUI.ThreadInfo>.Enumerator enumerator2 = current1.threads.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              ProfilerTimelineGUI.ThreadInfo current2 = enumerator2.Current;
              float height = current2.height;
              if ((double) height != 0.0)
                this.DrawBar(r, y, height, current2.name, false, true, !flag);
              y += height;
            }
          }
        }
      }
    }

    private void DrawGrid(Rect r, int threadCount, float frameTime)
    {
      float num = 16.66667f;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      GL.Color(new Color(1f, 1f, 1f, 0.2f));
      float time1 = num;
      while ((double) time1 <= (double) frameTime)
      {
        float pixel = this.m_TimeArea.TimeToPixel(time1, r);
        GL.Vertex3(pixel, r.y, 0.0f);
        GL.Vertex3(pixel, r.y + r.height, 0.0f);
        time1 += num;
      }
      GL.Color(new Color(1f, 1f, 1f, 0.8f));
      float pixel1 = this.m_TimeArea.TimeToPixel(0.0f, r);
      GL.Vertex3(pixel1, r.y, 0.0f);
      GL.Vertex3(pixel1, r.y + r.height, 0.0f);
      float pixel2 = this.m_TimeArea.TimeToPixel(frameTime, r);
      GL.Vertex3(pixel2, r.y, 0.0f);
      GL.Vertex3(pixel2, r.y + r.height, 0.0f);
      GL.End();
      GUI.color = new Color(1f, 1f, 1f, 0.4f);
      float time2 = 0.0f;
      while ((double) time2 <= (double) frameTime)
      {
        Chart.DoLabel(this.m_TimeArea.TimeToPixel(time2, r) + 2f, r.yMax - 12f, string.Format("{0:f1}ms", (object) time2), 0.0f);
        time2 += num;
      }
      GUI.color = new Color(1f, 1f, 1f, 1f);
      float time3 = frameTime;
      Chart.DoLabel(this.m_TimeArea.TimeToPixel(time3, r) + 2f, r.yMax - 12f, string.Format("{0:f1}ms ({1:f0}FPS)", (object) time3, (object) (float) (1000.0 / (double) time3)), 0.0f);
    }

    private void DrawSmallGroup(float x1, float x2, float y, float height, int size)
    {
      if ((double) x2 - (double) x1 < 1.0)
        return;
      GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
      GUI.contentColor = Color.white;
      GUIContent content = GUIContent.none;
      if ((double) x2 - (double) x1 > 20.0)
        content = new GUIContent(size.ToString() + " items");
      GUI.Label(new Rect(x1, y, x2 - x1, height), content, ProfilerTimelineGUI.styles.bar);
    }

    private static float TimeToPixelCached(float time, float rectWidthDivShownWidth, float shownX, float rectX)
    {
      return (time - shownX) * rectWidthDivShownWidth + rectX;
    }

    private void DrawProfilingData(ProfilerFrameDataIterator iter, Rect r, int threadIdx, float timeOffset, bool ghost, bool includeSubSamples)
    {
      float num1 = !ghost ? 7f : 21f;
      string selectedPropertyPath = ProfilerDriver.selectedPropertyPath;
      Color color1 = GUI.color;
      Color contentColor = GUI.contentColor;
      Color[] colors = ProfilerColors.colors;
      bool flag1 = false;
      float x1 = -1f;
      float x2 = -1f;
      float y1 = -1f;
      int size = 0;
      float y2 = -1f;
      string str = (string) null;
      float num2 = !includeSubSamples ? r.height : 16f;
      float num3 = !includeSubSamples ? 0.0f : 1f;
      float height = num2 - 2f * num3;
      r.height -= num3;
      GUI.BeginGroup(r);
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Rect& local = @r;
      float num4 = 0.0f;
      r.y = num4;
      double num5 = (double) num4;
      // ISSUE: explicit reference operation
      (^local).x = (float) num5;
      bool flag2 = Event.current.clickCount == 1 && Event.current.type == EventType.MouseDown;
      bool flag3 = Event.current.clickCount == 2 && Event.current.type == EventType.MouseDown;
      Rect shownArea = this.m_TimeArea.shownArea;
      float rectWidthDivShownWidth = r.width / shownArea.width;
      float x3 = r.x;
      float x4 = shownArea.x;
      bool enterChildren = true;
      while (iter.Next(enterChildren))
      {
        enterChildren = includeSubSamples;
        float time = iter.startTimeMS + timeOffset;
        float durationMs = iter.durationMS;
        float num6 = Mathf.Max(durationMs, 0.0003f);
        float pixelCached = ProfilerTimelineGUI.TimeToPixelCached(time, rectWidthDivShownWidth, x4, x3);
        float num7 = ProfilerTimelineGUI.TimeToPixelCached(time + num6, rectWidthDivShownWidth, x4, x3) - 1f;
        float width = num7 - pixelCached;
        if ((double) pixelCached > (double) r.x + (double) r.width || (double) num7 < (double) r.x)
        {
          enterChildren = false;
        }
        else
        {
          float num8 = (float) (iter.depth - 1);
          float y3 = r.y + num8 * num2;
          if (flag1)
          {
            bool flag4 = false;
            if ((double) width >= (double) num1)
              flag4 = true;
            if ((double) y1 != (double) y3)
              flag4 = true;
            if ((double) pixelCached - (double) x2 > 6.0)
              flag4 = true;
            if (flag4)
            {
              this.DrawSmallGroup(x1, x2, y1, height, size);
              flag1 = false;
            }
          }
          if ((double) width < (double) num1)
          {
            enterChildren = false;
            if (!flag1)
            {
              flag1 = true;
              y1 = y3;
              x1 = pixelCached;
              size = 0;
            }
            x2 = num7;
            ++size;
          }
          else
          {
            int id = iter.id;
            string path = iter.path;
            bool flag4 = path == selectedPropertyPath && !ghost;
            if (this.m_SelectedID >= 0)
              flag4 &= id == this.m_SelectedID;
            bool flag5 = flag4 & threadIdx == this.m_SelectedThread;
            Color white = Color.white;
            Color color2 = colors[iter.group % colors.Length];
            color2.a = !flag5 ? 0.75f : 1f;
            if (ghost)
            {
              color2.a = 0.4f;
              white.a = 0.5f;
            }
            string text = iter.name;
            if (flag5)
            {
              str = text;
              this.m_SelectedTime = time;
              this.m_SelectedDur = durationMs;
              y2 = y3 + num2;
            }
            if ((double) width < 20.0 || !includeSubSamples)
            {
              text = string.Empty;
            }
            else
            {
              if ((double) width < 50.0 && !flag5)
                white.a *= (float) (((double) width - 20.0) / 30.0);
              if ((double) width > 200.0)
                text += string.Format(" ({0:f2}ms)", (object) durationMs);
            }
            GUI.color = color2;
            GUI.contentColor = white;
            Rect position = new Rect(pixelCached, y3, width, height);
            GUI.Label(position, text, ProfilerTimelineGUI.styles.bar);
            if ((flag2 || flag3) && position.Contains(Event.current.mousePosition))
            {
              this.m_Window.SetSelectedPropertyPath(path);
              this.m_SelectedThread = threadIdx;
              this.m_SelectedID = id;
              UnityEngine.Object gameObject = EditorUtility.InstanceIDToObject(iter.instanceId);
              if (gameObject is Component)
                gameObject = (UnityEngine.Object) ((Component) gameObject).gameObject;
              if (gameObject != (UnityEngine.Object) null)
              {
                if (flag2)
                  EditorGUIUtility.PingObject(gameObject.GetInstanceID());
                else if (flag3)
                  Selection.objects = new List<UnityEngine.Object>()
                  {
                    gameObject
                  }.ToArray();
              }
              Event.current.Use();
            }
            flag1 = false;
          }
        }
      }
      if (flag1)
        this.DrawSmallGroup(x1, x2, y1, height, size);
      GUI.color = color1;
      GUI.contentColor = contentColor;
      if (str != null && threadIdx == this.m_SelectedThread && includeSubSamples)
      {
        GUIContent content = new GUIContent(string.Format((double) this.m_SelectedDur < 1.0 ? "{0}\n{1:f3}ms" : "{0}\n{1:f2}ms", (object) str, (object) this.m_SelectedDur));
        GUIStyle tooltip = ProfilerTimelineGUI.styles.tooltip;
        Vector2 vector2 = tooltip.CalcSize(content);
        float x5 = this.m_TimeArea.TimeToPixel(this.m_SelectedTime + this.m_SelectedDur * 0.5f, r);
        if ((double) x5 < (double) r.x)
          x5 = r.x + 20f;
        if ((double) x5 > (double) r.xMax)
          x5 = r.xMax - 20f;
        Rect position;
        if ((double) y2 + 6.0 + (double) vector2.y < (double) r.yMax)
        {
          position = new Rect(x5 - 32f, y2, 50f, 7f);
          GUI.Label(position, GUIContent.none, ProfilerTimelineGUI.styles.tooltipArrow);
        }
        position = new Rect(x5, y2 + 6f, vector2.x, vector2.y);
        if ((double) position.xMax > (double) r.xMax + 20.0)
          position.x = (float) ((double) r.xMax - (double) position.width + 20.0);
        if ((double) position.yMax > (double) r.yMax)
          position.y = r.yMax - position.height;
        if ((double) position.y < (double) r.y)
          position.y = r.y;
        GUI.Label(position, content, tooltip);
      }
      if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
      {
        this.m_Window.ClearSelectedPropertyPath();
        this.m_SelectedID = -1;
        this.m_SelectedThread = threadIdx;
        Event.current.Use();
      }
      GUI.EndGroup();
    }

    private void PerformFrameSelected(float frameMS)
    {
      float num1 = this.m_SelectedTime;
      float num2 = this.m_SelectedDur;
      if (this.m_SelectedID < 0 || (double) num2 <= 0.0)
      {
        num1 = 0.0f;
        num2 = frameMS;
      }
      this.m_TimeArea.SetShownHRangeInsideMargins(num1 - num2 * 0.2f, num1 + num2 * 1.2f);
    }

    private void HandleFrameSelected(float frameMS)
    {
      Event current = Event.current;
      if (current.type != EventType.ValidateCommand && current.type != EventType.ExecuteCommand || !(current.commandName == "FrameSelected"))
        return;
      if (current.type == EventType.ExecuteCommand)
        this.PerformFrameSelected(frameMS);
      current.Use();
    }

    private void DoProfilerFrame(int frameIndex, Rect fullRect, bool ghost, ref int threadCount, float offset)
    {
      ProfilerFrameDataIterator iter = new ProfilerFrameDataIterator();
      int threadCount1 = iter.GetThreadCount(frameIndex);
      if (ghost && threadCount1 != threadCount)
        return;
      iter.SetRoot(frameIndex, 0);
      if (!ghost)
      {
        threadCount = threadCount1;
        this.DrawGrid(fullRect, threadCount, iter.frameTimeMS);
        this.HandleFrameSelected(iter.frameTimeMS);
      }
      float num1 = fullRect.y;
      using (List<ProfilerTimelineGUI.GroupInfo>.Enumerator enumerator1 = this.groups.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          ProfilerTimelineGUI.GroupInfo current1 = enumerator1.Current;
          Rect r = fullRect;
          bool expanded = current1.expanded;
          if (expanded)
            num1 += current1.height;
          float num2 = num1;
          int count = current1.threads.Count;
          using (List<ProfilerTimelineGUI.ThreadInfo>.Enumerator enumerator2 = current1.threads.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              ProfilerTimelineGUI.ThreadInfo current2 = enumerator2.Current;
              iter.SetRoot(frameIndex, current2.threadIndex);
              r.y = num1;
              r.height = !expanded ? Math.Max((float) ((double) current1.height / (double) count - 1.0), 2f) : current2.height;
              this.DrawProfilingData(iter, r, current2.threadIndex, offset, ghost, expanded);
              num1 += r.height;
            }
          }
          if (!expanded)
            num1 = num2 + current1.height;
        }
      }
    }

    public void DoGUI(int frameIndex, float width, float ypos, float height)
    {
      Rect position = new Rect(0.0f, ypos - 1f, width, height + 1f);
      float width1 = 169f;
      if (Event.current.type == EventType.Repaint)
      {
        ProfilerTimelineGUI.styles.profilerGraphBackground.Draw(position, false, false, false, false);
        EditorStyles.toolbar.Draw(new Rect(0.0f, (float) ((double) ypos + (double) height - 15.0), width1, 15f), false, false, false, false);
      }
      bool flag = false;
      if (this.m_TimeArea == null)
      {
        flag = true;
        this.m_TimeArea = new ZoomableArea();
        this.m_TimeArea.hRangeLocked = false;
        this.m_TimeArea.vRangeLocked = true;
        this.m_TimeArea.hSlider = true;
        this.m_TimeArea.vSlider = false;
        this.m_TimeArea.scaleWithWindow = true;
        this.m_TimeArea.rect = new Rect((float) ((double) position.x + (double) width1 - 1.0), position.y, position.width - width1, position.height);
        this.m_TimeArea.margin = 10f;
      }
      ProfilerFrameDataIterator frameDataIterator1 = new ProfilerFrameDataIterator();
      frameDataIterator1.SetRoot(frameIndex, 0);
      this.m_TimeArea.hBaseRangeMin = 0.0f;
      this.m_TimeArea.hBaseRangeMax = frameDataIterator1.frameTimeMS;
      if (flag)
        this.PerformFrameSelected(frameDataIterator1.frameTimeMS);
      this.m_TimeArea.rect = new Rect(position.x + width1, position.y, position.width - width1, position.height);
      this.m_TimeArea.BeginViewGUI();
      this.m_TimeArea.EndViewGUI();
      Rect drawRect = this.m_TimeArea.drawRect;
      this.CalculateBars(drawRect, frameIndex, this.animationTime);
      this.DrawBars(drawRect, frameIndex);
      GUI.BeginClip(this.m_TimeArea.drawRect);
      drawRect.x = 0.0f;
      drawRect.y = 0.0f;
      int threadCount = 0;
      this.DoProfilerFrame(frameIndex, drawRect, false, ref threadCount, 0.0f);
      bool enabled = GUI.enabled;
      GUI.enabled = false;
      int previousFrameIndex = ProfilerDriver.GetPreviousFrameIndex(frameIndex);
      if (previousFrameIndex != -1)
      {
        ProfilerFrameDataIterator frameDataIterator2 = new ProfilerFrameDataIterator();
        frameDataIterator2.SetRoot(previousFrameIndex, 0);
        this.DoProfilerFrame(previousFrameIndex, drawRect, true, ref threadCount, -frameDataIterator2.frameTimeMS);
      }
      int nextFrameIndex = ProfilerDriver.GetNextFrameIndex(frameIndex);
      if (nextFrameIndex != -1)
      {
        ProfilerFrameDataIterator frameDataIterator2 = new ProfilerFrameDataIterator();
        frameDataIterator2.SetRoot(frameIndex, 0);
        this.DoProfilerFrame(nextFrameIndex, drawRect, true, ref threadCount, frameDataIterator2.frameTimeMS);
      }
      GUI.enabled = enabled;
      GUI.EndClip();
    }

    internal class ThreadInfo
    {
      public float height;
      public float desiredWeight;
      public float weight;
      public int threadIndex;
      public string name;
    }

    internal class GroupInfo
    {
      public bool expanded;
      public string name;
      public float height;
      public List<ProfilerTimelineGUI.ThreadInfo> threads;
    }

    internal class Styles
    {
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle tooltip = (GUIStyle) "AnimationEventTooltip";
      public GUIStyle tooltipArrow = (GUIStyle) "AnimationEventTooltipArrow";
      public GUIStyle bar = (GUIStyle) "ProfilerTimelineBar";
      public GUIStyle leftPane = (GUIStyle) "ProfilerTimelineLeftPane";
      public GUIStyle rightPane = (GUIStyle) "ProfilerRightPane";
      public GUIStyle foldout = (GUIStyle) "ProfilerTimelineFoldout";
      public GUIStyle profilerGraphBackground = new GUIStyle((GUIStyle) "ProfilerScrollviewBackground");

      internal Styles()
      {
        GUIStyleState normal1 = this.bar.normal;
        Texture2D whiteTexture = EditorGUIUtility.whiteTexture;
        this.bar.active.background = whiteTexture;
        Texture2D texture2D1 = whiteTexture;
        this.bar.hover.background = texture2D1;
        Texture2D texture2D2 = texture2D1;
        normal1.background = texture2D2;
        GUIStyleState normal2 = this.bar.normal;
        Color black = Color.black;
        this.bar.active.textColor = black;
        Color color1 = black;
        this.bar.hover.textColor = color1;
        Color color2 = color1;
        normal2.textColor = color2;
        this.profilerGraphBackground.overflow.left = -169;
        this.leftPane.padding.left = 15;
      }
    }
  }
}
