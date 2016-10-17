// Decompiled with JetBrains decompiler
// Type: UnityEditor.TimeArea
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TimeArea : ZoomableArea
  {
    private CurveEditorSettings m_Settings = new CurveEditorSettings();
    internal const int kTickRulerDistMin = 3;
    internal const int kTickRulerDistFull = 80;
    internal const int kTickRulerDistLabel = 40;
    internal const float kTickRulerHeightMax = 0.7f;
    internal const float kTickRulerFatThreshold = 0.5f;
    [SerializeField]
    private TickHandler m_HTicks;
    [SerializeField]
    private TickHandler m_VTicks;
    private static TimeArea.Styles2 styles;
    private static float s_OriginalTime;
    private static float s_PickOffset;

    public TickHandler hTicks
    {
      get
      {
        return this.m_HTicks;
      }
      set
      {
        this.m_HTicks = value;
      }
    }

    public TickHandler vTicks
    {
      get
      {
        return this.m_VTicks;
      }
      set
      {
        this.m_VTicks = value;
      }
    }

    public CurveEditorSettings settings
    {
      get
      {
        return this.m_Settings;
      }
      set
      {
        if (value == null)
          return;
        this.m_Settings = value;
        this.ApplySettings();
      }
    }

    public TimeArea(bool minimalGUI)
      : base(minimalGUI)
    {
      float[] tickModulos = new float[29]
      {
        1E-07f,
        5E-07f,
        1E-06f,
        5E-06f,
        1E-05f,
        5E-05f,
        0.0001f,
        0.0005f,
        1f / 1000f,
        0.005f,
        0.01f,
        0.05f,
        0.1f,
        0.5f,
        1f,
        5f,
        10f,
        50f,
        100f,
        500f,
        1000f,
        5000f,
        10000f,
        50000f,
        100000f,
        500000f,
        1000000f,
        5000000f,
        1E+07f
      };
      this.hTicks = new TickHandler();
      this.hTicks.SetTickModulos(tickModulos);
      this.vTicks = new TickHandler();
      this.vTicks.SetTickModulos(tickModulos);
    }

    private static void InitStyles()
    {
      if (TimeArea.styles != null)
        return;
      TimeArea.styles = new TimeArea.Styles2();
    }

    protected virtual void ApplySettings()
    {
      this.hRangeLocked = this.settings.hRangeLocked;
      this.vRangeLocked = this.settings.vRangeLocked;
      this.hRangeMin = this.settings.hRangeMin;
      this.hRangeMax = this.settings.hRangeMax;
      this.vRangeMin = this.settings.vRangeMin;
      this.vRangeMax = this.settings.vRangeMax;
      this.scaleWithWindow = this.settings.scaleWithWindow;
      this.hSlider = this.settings.hSlider;
      this.vSlider = this.settings.vSlider;
    }

    public void SetTickMarkerRanges()
    {
      this.hTicks.SetRanges(this.shownArea.xMin, this.shownArea.xMax, this.drawRect.xMin, this.drawRect.xMax);
      this.vTicks.SetRanges(this.shownArea.yMin, this.shownArea.yMax, this.drawRect.yMin, this.drawRect.yMax);
    }

    public void DrawMajorTicks(Rect position, float frameRate)
    {
      Color color = Handles.color;
      GUI.BeginGroup(position);
      if (Event.current.type != EventType.Repaint)
      {
        GUI.EndGroup();
      }
      else
      {
        TimeArea.InitStyles();
        this.SetTickMarkerRanges();
        this.hTicks.SetTickStrengths(3f, 80f, true);
        Color textColor = TimeArea.styles.TimelineTick.normal.textColor;
        textColor.a = 0.1f;
        Handles.color = textColor;
        for (int level = 0; level < this.hTicks.tickLevels; ++level)
        {
          if ((double) (this.hTicks.GetStrengthOfLevel(level) * 0.9f) > 0.5)
          {
            float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(level, true);
            for (int index = 0; index < ticksAtLevel.Length; ++index)
            {
              if ((double) ticksAtLevel[index] >= 0.0)
              {
                float pixel = this.FrameToPixel((float) Mathf.RoundToInt(ticksAtLevel[index] * frameRate), frameRate, position);
                Handles.DrawLine(new Vector3(pixel, 0.0f, 0.0f), new Vector3(pixel, position.height, 0.0f));
              }
            }
          }
        }
        GUI.EndGroup();
        Handles.color = color;
      }
    }

    public void TimeRuler(Rect position, float frameRate)
    {
      this.TimeRuler(position, frameRate, true, false, 1f);
    }

    public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha)
    {
      Color color = GUI.color;
      GUI.BeginGroup(position);
      if (Event.current.type != EventType.Repaint)
      {
        GUI.EndGroup();
      }
      else
      {
        TimeArea.InitStyles();
        HandleUtility.ApplyWireMaterial();
        if (Application.platform == RuntimePlatform.WindowsEditor)
          GL.Begin(7);
        else
          GL.Begin(1);
        Color backgroundColor = GUI.backgroundColor;
        this.SetTickMarkerRanges();
        this.hTicks.SetTickStrengths(3f, 80f, true);
        Color textColor = TimeArea.styles.TimelineTick.normal.textColor;
        textColor.a = 0.75f * alpha;
        for (int level = 0; level < this.hTicks.tickLevels; ++level)
        {
          float b = this.hTicks.GetStrengthOfLevel(level) * 0.9f;
          float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(level, true);
          for (int index = 0; index < ticksAtLevel.Length; ++index)
          {
            if ((double) ticksAtLevel[index] >= (double) this.hRangeMin && (double) ticksAtLevel[index] <= (double) this.hRangeMax)
            {
              int num1 = Mathf.RoundToInt(ticksAtLevel[index] * frameRate);
              float num2 = !useEntireHeight ? (float) ((double) position.height * (double) Mathf.Min(1f, b) * 0.699999988079071) : position.height;
              TimeArea.DrawVerticalLineFast(this.FrameToPixel((float) num1, frameRate, position), (float) ((double) position.height - (double) num2 + 0.5), position.height - 0.5f, new Color(1f, 1f, 1f, b / 0.5f) * textColor);
            }
          }
        }
        GL.End();
        if (labels)
        {
          float[] ticksAtLevel = this.hTicks.GetTicksAtLevel(this.hTicks.GetLevelWithMinSeparation(40f), false);
          for (int index = 0; index < ticksAtLevel.Length; ++index)
          {
            if ((double) ticksAtLevel[index] >= (double) this.hRangeMin && (double) ticksAtLevel[index] <= (double) this.hRangeMax)
            {
              int frame = Mathf.RoundToInt(ticksAtLevel[index] * frameRate);
              GUI.Label(new Rect(Mathf.Floor(this.FrameToPixel((float) frame, frameRate, position)) + 3f, -3f, 40f, 20f), this.FormatFrame(frame, frameRate), TimeArea.styles.TimelineTick);
            }
          }
        }
        GUI.EndGroup();
        GUI.backgroundColor = backgroundColor;
        GUI.color = color;
      }
    }

    public static void DrawVerticalLine(float x, float minY, float maxY, Color color)
    {
      HandleUtility.ApplyWireMaterial();
      if (Application.platform == RuntimePlatform.WindowsEditor)
        GL.Begin(7);
      else
        GL.Begin(1);
      TimeArea.DrawVerticalLineFast(x, minY, maxY, color);
      GL.End();
    }

    public static void DrawVerticalLineFast(float x, float minY, float maxY, Color color)
    {
      if (Application.platform == RuntimePlatform.WindowsEditor)
      {
        GL.Color(color);
        GL.Vertex(new Vector3(x - 0.5f, minY, 0.0f));
        GL.Vertex(new Vector3(x + 0.5f, minY, 0.0f));
        GL.Vertex(new Vector3(x + 0.5f, maxY, 0.0f));
        GL.Vertex(new Vector3(x - 0.5f, maxY, 0.0f));
      }
      else
      {
        GL.Color(color);
        GL.Vertex(new Vector3(x, minY, 0.0f));
        GL.Vertex(new Vector3(x, maxY, 0.0f));
      }
    }

    public TimeArea.TimeRulerDragMode BrowseRuler(Rect position, ref float time, float frameRate, bool pickAnywhere, GUIStyle thumbStyle)
    {
      int controlId = GUIUtility.GetControlID(3126789, FocusType.Passive);
      return this.BrowseRuler(position, controlId, ref time, frameRate, pickAnywhere, thumbStyle);
    }

    public TimeArea.TimeRulerDragMode BrowseRuler(Rect position, int id, ref float time, float frameRate, bool pickAnywhere, GUIStyle thumbStyle)
    {
      Event current = Event.current;
      Rect position1 = position;
      if ((double) time != -1.0)
      {
        position1.x = Mathf.Round(this.TimeToPixel(time, position)) - (float) thumbStyle.overflow.left;
        position1.width = thumbStyle.fixedWidth + (float) thumbStyle.overflow.horizontal;
      }
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (position1.Contains(current.mousePosition))
          {
            GUIUtility.hotControl = id;
            TimeArea.s_PickOffset = current.mousePosition.x - this.TimeToPixel(time, position);
            current.Use();
            return TimeArea.TimeRulerDragMode.Start;
          }
          if (pickAnywhere && position.Contains(current.mousePosition))
          {
            GUIUtility.hotControl = id;
            float wholeFps = TimeArea.SnapTimeToWholeFPS(this.PixelToTime(current.mousePosition.x, position), frameRate);
            TimeArea.s_OriginalTime = time;
            if ((double) wholeFps != (double) time)
              GUI.changed = true;
            time = wholeFps;
            TimeArea.s_PickOffset = 0.0f;
            current.Use();
            return TimeArea.TimeRulerDragMode.Start;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            return TimeArea.TimeRulerDragMode.End;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            float wholeFps = TimeArea.SnapTimeToWholeFPS(this.PixelToTime(current.mousePosition.x - TimeArea.s_PickOffset, position), frameRate);
            if ((double) wholeFps != (double) time)
              GUI.changed = true;
            time = wholeFps;
            current.Use();
            return TimeArea.TimeRulerDragMode.Dragging;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == id && current.keyCode == KeyCode.Escape)
          {
            if ((double) time != (double) TimeArea.s_OriginalTime)
              GUI.changed = true;
            time = TimeArea.s_OriginalTime;
            GUIUtility.hotControl = 0;
            current.Use();
            return TimeArea.TimeRulerDragMode.Cancel;
          }
          break;
        case EventType.Repaint:
          if ((double) time != -1.0)
          {
            bool flag = position.Contains(current.mousePosition);
            position1.x += (float) thumbStyle.overflow.left;
            thumbStyle.Draw(position1, id == GUIUtility.hotControl, flag || id == GUIUtility.hotControl, false, false);
            break;
          }
          break;
      }
      return TimeArea.TimeRulerDragMode.None;
    }

    public void SetTransform(Vector2 newTranslation, Vector2 newScale)
    {
      this.m_Scale = newScale;
      this.m_Translation = newTranslation;
      this.EnforceScaleAndRange();
    }

    private void DrawLine(Vector2 lhs, Vector2 rhs)
    {
      GL.Vertex(this.DrawingToViewTransformPoint(new Vector3(lhs.x, lhs.y, 0.0f)));
      GL.Vertex(this.DrawingToViewTransformPoint(new Vector3(rhs.x, rhs.y, 0.0f)));
    }

    public float FrameToPixel(float i, float frameRate, Rect rect)
    {
      return (float) (((double) i - (double) this.shownArea.xMin * (double) frameRate) * (double) rect.width / ((double) this.shownArea.width * (double) frameRate));
    }

    public string FormatFrame(int frame, float frameRate)
    {
      int length = ((int) frameRate).ToString().Length;
      string str = string.Empty;
      if (frame < 0)
      {
        str = "-";
        frame = -frame;
      }
      return str + (frame / (int) frameRate).ToString() + ":" + ((float) frame % frameRate).ToString().PadLeft(length, '0');
    }

    public static float SnapTimeToWholeFPS(float time, float frameRate)
    {
      if ((double) frameRate == 0.0)
        return time;
      return Mathf.Round(time * frameRate) / frameRate;
    }

    private class Styles2
    {
      public GUIStyle TimelineTick = (GUIStyle) "AnimationTimelineTick";
      public GUIStyle labelTickMarks = (GUIStyle) "CurveEditorLabelTickMarks";
    }

    public enum TimeRulerDragMode
    {
      None,
      Start,
      End,
      Dragging,
      Cancel,
    }
  }
}
