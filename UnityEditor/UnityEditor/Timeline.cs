// Decompiled with JetBrains decompiler
// Type: UnityEditor.Timeline
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class Timeline
  {
    private float m_Time = float.PositiveInfinity;
    private float m_StopTime = 1f;
    private string m_SrcName = "Left";
    private string m_DstName = "Right";
    private float m_SrcStopTime = 0.75f;
    private float m_DstStartTime = 0.25f;
    private float m_DstStopTime = 1f;
    private float m_TransitionStartTime = float.PositiveInfinity;
    private float m_TransitionStopTime = float.PositiveInfinity;
    private float m_SampleStopTime = float.PositiveInfinity;
    private int id = -1;
    private Rect m_Rect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    private List<Timeline.PivotSample> m_SrcPivotList = new List<Timeline.PivotSample>();
    private List<Timeline.PivotSample> m_DstPivotList = new List<Timeline.PivotSample>();
    private TimeArea m_TimeArea;
    private float m_StartTime;
    private bool m_SrcLoop;
    private bool m_DstLoop;
    private float m_SrcStartTime;
    private bool m_HasExitTime;
    private float m_DstDragOffset;
    private float m_LeftThumbOffset;
    private float m_RightThumbOffset;
    private Timeline.DragStates m_DragState;
    private Vector3[] m_SrcPivotVectors;
    private Vector3[] m_DstPivotVectors;
    private Timeline.Styles styles;

    public List<Timeline.PivotSample> SrcPivotList
    {
      get
      {
        return this.m_SrcPivotList;
      }
      set
      {
        this.m_SrcPivotList = value;
        this.m_SrcPivotVectors = (Vector3[]) null;
      }
    }

    public List<Timeline.PivotSample> DstPivotList
    {
      get
      {
        return this.m_DstPivotList;
      }
      set
      {
        this.m_DstPivotList = value;
        this.m_DstPivotVectors = (Vector3[]) null;
      }
    }

    public bool srcLoop
    {
      get
      {
        return this.m_SrcLoop;
      }
      set
      {
        this.m_SrcLoop = value;
      }
    }

    public bool dstLoop
    {
      get
      {
        return this.m_DstLoop;
      }
      set
      {
        this.m_DstLoop = value;
      }
    }

    public float Time
    {
      get
      {
        return this.m_Time;
      }
      set
      {
        this.m_Time = value;
      }
    }

    public float StartTime
    {
      get
      {
        return this.m_StartTime;
      }
      set
      {
        this.m_StartTime = value;
      }
    }

    public float StopTime
    {
      get
      {
        return this.m_StopTime;
      }
      set
      {
        this.m_StopTime = value;
      }
    }

    public string SrcName
    {
      get
      {
        return this.m_SrcName;
      }
      set
      {
        this.m_SrcName = value;
      }
    }

    public string DstName
    {
      get
      {
        return this.m_DstName;
      }
      set
      {
        this.m_DstName = value;
      }
    }

    public float SrcStartTime
    {
      get
      {
        return this.m_SrcStartTime;
      }
      set
      {
        this.m_SrcStartTime = value;
      }
    }

    public float SrcStopTime
    {
      get
      {
        return this.m_SrcStopTime;
      }
      set
      {
        this.m_SrcStopTime = value;
      }
    }

    public float SrcDuration
    {
      get
      {
        return this.SrcStopTime - this.SrcStartTime;
      }
    }

    public float DstStartTime
    {
      get
      {
        return this.m_DstStartTime;
      }
      set
      {
        this.m_DstStartTime = value;
      }
    }

    public float DstStopTime
    {
      get
      {
        return this.m_DstStopTime;
      }
      set
      {
        this.m_DstStopTime = value;
      }
    }

    public float DstDuration
    {
      get
      {
        return this.DstStopTime - this.DstStartTime;
      }
    }

    public float TransitionStartTime
    {
      get
      {
        return this.m_TransitionStartTime;
      }
      set
      {
        this.m_TransitionStartTime = value;
      }
    }

    public float TransitionStopTime
    {
      get
      {
        return this.m_TransitionStopTime;
      }
      set
      {
        this.m_TransitionStopTime = value;
      }
    }

    public bool HasExitTime
    {
      get
      {
        return this.m_HasExitTime;
      }
      set
      {
        this.m_HasExitTime = value;
      }
    }

    public float TransitionDuration
    {
      get
      {
        return this.TransitionStopTime - this.TransitionStartTime;
      }
    }

    public float SampleStopTime
    {
      get
      {
        return this.m_SampleStopTime;
      }
      set
      {
        this.m_SampleStopTime = value;
      }
    }

    public Timeline()
    {
      this.Init();
    }

    public void ResetRange()
    {
      this.m_TimeArea.SetShownHRangeInsideMargins(0.0f, this.StopTime);
    }

    private void Init()
    {
      if (this.id == -1)
        this.id = GUIUtility.GetPermanentControlID();
      if (this.m_TimeArea == null)
      {
        this.m_TimeArea = new TimeArea(false);
        this.m_TimeArea.hRangeLocked = false;
        this.m_TimeArea.vRangeLocked = true;
        this.m_TimeArea.hSlider = false;
        this.m_TimeArea.vSlider = false;
        this.m_TimeArea.margin = 10f;
        this.m_TimeArea.scaleWithWindow = true;
        this.m_TimeArea.hTicks.SetTickModulosForFrameRate(30f);
      }
      if (this.styles != null)
        return;
      this.styles = new Timeline.Styles();
    }

    private List<Vector3> GetControls(List<Vector3> segmentPoints, float scale)
    {
      List<Vector3> vector3List = new List<Vector3>();
      if (segmentPoints.Count < 2)
        return vector3List;
      for (int index = 0; index < segmentPoints.Count; ++index)
      {
        if (index == 0)
        {
          Vector3 segmentPoint = segmentPoints[index];
          Vector3 vector3_1 = segmentPoints[index + 1] - segmentPoint;
          Vector3 vector3_2 = segmentPoint + scale * vector3_1;
          vector3List.Add(segmentPoint);
          vector3List.Add(vector3_2);
        }
        else if (index == segmentPoints.Count - 1)
        {
          Vector3 segmentPoint1 = segmentPoints[index - 1];
          Vector3 segmentPoint2 = segmentPoints[index];
          Vector3 vector3_1 = segmentPoint2 - segmentPoint1;
          Vector3 vector3_2 = segmentPoint2 - scale * vector3_1;
          vector3List.Add(vector3_2);
          vector3List.Add(segmentPoint2);
        }
        else
        {
          Vector3 segmentPoint1 = segmentPoints[index - 1];
          Vector3 segmentPoint2 = segmentPoints[index];
          Vector3 segmentPoint3 = segmentPoints[index + 1];
          Vector3 normalized = (segmentPoint3 - segmentPoint1).normalized;
          Vector3 vector3_1 = segmentPoint2 - scale * normalized * (segmentPoint2 - segmentPoint1).magnitude;
          Vector3 vector3_2 = segmentPoint2 + scale * normalized * (segmentPoint3 - segmentPoint2).magnitude;
          vector3List.Add(vector3_1);
          vector3List.Add(segmentPoint2);
          vector3List.Add(vector3_2);
        }
      }
      return vector3List;
    }

    private Vector3 CalculatePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
      float num1 = 1f - t;
      float num2 = t * t;
      float num3 = num1 * num1;
      float num4 = num3 * num1;
      float num5 = num2 * t;
      return num4 * p0 + 3f * num3 * t * p1 + 3f * num1 * num2 * p2 + num5 * p3;
    }

    private Color[] GetPivotColors(Vector3[] vectors, float motionStart, float motionStop, Color fromColor, Color toColor, Color loopColor, float offset)
    {
      Color[] colorArray = new Color[vectors.Length];
      float num1 = this.m_TimeArea.TimeToPixel(this.m_TransitionStartTime, this.m_Rect) + this.m_LeftThumbOffset;
      float num2 = this.m_TimeArea.TimeToPixel(this.m_TransitionStopTime, this.m_Rect) + this.m_RightThumbOffset;
      float num3 = num2 - num1;
      for (int index = 0; index < colorArray.Length; ++index)
        colorArray[index] = (double) vectors[index].x < (double) num1 || (double) vectors[index].x > (double) num2 ? ((double) vectors[index].x >= (double) num1 || (double) vectors[index].x < (double) motionStart + (double) offset ? ((double) vectors[index].x <= (double) num2 || (double) vectors[index].x > (double) motionStop + (double) offset ? loopColor : toColor) : fromColor) : Color.Lerp(fromColor, toColor, (vectors[index].x - num1) / num3);
      return colorArray;
    }

    private Vector3[] GetPivotVectors(Timeline.PivotSample[] samples, float width, Rect rect, float height, bool loop)
    {
      if (samples.Length == 0 || (double) width < 0.330000013113022)
        return new Vector3[0];
      List<Vector3> segmentPoints = new List<Vector3>();
      for (int index = 0; index < samples.Length; ++index)
      {
        Timeline.PivotSample sample = samples[index];
        Vector3 zero = Vector3.zero;
        zero.x = this.m_TimeArea.TimeToPixel(sample.m_Time, rect);
        zero.y = (float) ((double) height / 16.0 + (double) sample.m_Weight * 12.0 * (double) height / 16.0);
        segmentPoints.Add(zero);
      }
      if (loop && (double) segmentPoints[segmentPoints.Count - 1].x <= (double) rect.width)
      {
        float x = segmentPoints[segmentPoints.Count - 1].x;
        int index = 0;
        int num = 1;
        List<Vector3> vector3List = new List<Vector3>();
        while ((double) x < (double) rect.width)
        {
          if (index > segmentPoints.Count - 1)
          {
            index = 0;
            ++num;
          }
          Vector3 vector3 = segmentPoints[index];
          vector3.x += (float) num * width;
          x = vector3.x;
          vector3List.Add(vector3);
          ++index;
        }
        segmentPoints.AddRange((IEnumerable<Vector3>) vector3List);
      }
      List<Vector3> controls = this.GetControls(segmentPoints, 0.5f);
      segmentPoints.Clear();
      int index1 = 0;
      while (index1 < controls.Count - 3)
      {
        Vector3 p0 = controls[index1];
        Vector3 p1 = controls[index1 + 1];
        Vector3 p2 = controls[index1 + 2];
        Vector3 p3 = controls[index1 + 3];
        if (index1 == 0)
          segmentPoints.Add(this.CalculatePoint(0.0f, p0, p1, p2, p3));
        for (int index2 = 1; index2 <= 10; ++index2)
          segmentPoints.Add(this.CalculatePoint((float) index2 / 10f, p0, p1, p2, p3));
        index1 += 3;
      }
      return segmentPoints.ToArray();
    }

    private Vector3[] OffsetPivotVectors(Vector3[] vectors, float offset)
    {
      for (int index = 0; index < vectors.Length; ++index)
        vectors[index].x += offset;
      return vectors;
    }

    private void DoPivotCurves()
    {
      Color white1 = Color.white;
      Color white2 = Color.white;
      Color toColor = new Color(1f, 1f, 1f, 0.1f);
      Color fromColor = new Color(1f, 1f, 1f, 0.1f);
      Color loopColor1 = new Color(0.75f, 0.75f, 0.75f, 0.2f);
      Color loopColor2 = new Color(0.75f, 0.75f, 0.75f, 0.2f);
      Rect rect = new Rect(0.0f, 18f, this.m_Rect.width, 66f);
      GUI.BeginGroup(rect);
      float pixel1 = this.m_TimeArea.TimeToPixel(this.SrcStartTime, rect);
      float pixel2 = this.m_TimeArea.TimeToPixel(this.SrcStopTime, rect);
      float pixel3 = this.m_TimeArea.TimeToPixel(this.DstStartTime, rect);
      float pixel4 = this.m_TimeArea.TimeToPixel(this.DstStopTime, rect);
      if (this.m_SrcPivotVectors == null)
        this.m_SrcPivotVectors = this.GetPivotVectors(this.m_SrcPivotList.ToArray(), pixel2 - pixel1, rect, rect.height, this.srcLoop);
      if (this.m_DstPivotVectors == null)
        this.m_DstPivotVectors = this.GetPivotVectors(this.m_DstPivotList.ToArray(), pixel4 - pixel3, rect, rect.height, this.dstLoop);
      this.m_DstPivotVectors = this.OffsetPivotVectors(this.m_DstPivotVectors, this.m_DstDragOffset + pixel3 - pixel1);
      Color[] pivotColors1 = this.GetPivotColors(this.m_SrcPivotVectors, pixel1, pixel2, white1, toColor, loopColor1, 0.0f);
      Color[] pivotColors2 = this.GetPivotColors(this.m_DstPivotVectors, pixel3, pixel4, fromColor, white2, loopColor2, this.m_DstDragOffset);
      Handles.DrawAAPolyLine(pivotColors1, this.m_SrcPivotVectors);
      Handles.DrawAAPolyLine(pivotColors2, this.m_DstPivotVectors);
      GUI.EndGroup();
    }

    private void EnforceConstraints()
    {
      Rect rect = new Rect(0.0f, 0.0f, this.m_Rect.width, 150f);
      if (this.m_DragState == Timeline.DragStates.LeftSelection)
        this.m_LeftThumbOffset = Mathf.Clamp(this.m_LeftThumbOffset, this.m_TimeArea.TimeToPixel(this.SrcStartTime, rect) - this.m_TimeArea.TimeToPixel(this.TransitionStartTime, rect), this.m_TimeArea.TimeToPixel(this.TransitionStopTime, rect) - this.m_TimeArea.TimeToPixel(this.TransitionStartTime, rect));
      if (this.m_DragState != Timeline.DragStates.RightSelection)
        return;
      float num = this.m_TimeArea.TimeToPixel(this.TransitionStartTime, rect) - this.m_TimeArea.TimeToPixel(this.TransitionStopTime, rect);
      if ((double) this.m_RightThumbOffset >= (double) num)
        return;
      this.m_RightThumbOffset = num;
    }

    private bool WasDraggingData()
    {
      if ((double) this.m_DstDragOffset == 0.0 && (double) this.m_LeftThumbOffset == 0.0)
        return (double) this.m_RightThumbOffset != 0.0;
      return true;
    }

    public bool DoTimeline(Rect timeRect)
    {
      bool flag1 = false;
      this.Init();
      this.m_Rect = timeRect;
      float time1 = this.m_TimeArea.PixelToTime(timeRect.xMin, timeRect);
      float time2 = this.m_TimeArea.PixelToTime(timeRect.xMax, timeRect);
      if (!Mathf.Approximately(time1, this.StartTime))
      {
        this.StartTime = time1;
        GUI.changed = true;
      }
      if (!Mathf.Approximately(time2, this.StopTime))
      {
        this.StopTime = time2;
        GUI.changed = true;
      }
      this.Time = Mathf.Max(this.Time, 0.0f);
      if (Event.current.type == EventType.Repaint)
        this.m_TimeArea.rect = timeRect;
      this.m_TimeArea.BeginViewGUI();
      this.m_TimeArea.EndViewGUI();
      GUI.BeginGroup(timeRect);
      Event current = Event.current;
      Rect rect1 = new Rect(0.0f, 0.0f, timeRect.width, timeRect.height);
      Rect position1 = new Rect(0.0f, 0.0f, timeRect.width, 18f);
      Rect position2 = new Rect(0.0f, 18f, timeRect.width, 132f);
      float pixel1 = this.m_TimeArea.TimeToPixel(this.SrcStartTime, rect1);
      float pixel2 = this.m_TimeArea.TimeToPixel(this.SrcStopTime, rect1);
      float num1 = this.m_TimeArea.TimeToPixel(this.DstStartTime, rect1) + this.m_DstDragOffset;
      float pixelX = this.m_TimeArea.TimeToPixel(this.DstStopTime, rect1) + this.m_DstDragOffset;
      float num2 = this.m_TimeArea.TimeToPixel(this.TransitionStartTime, rect1) + this.m_LeftThumbOffset;
      float num3 = this.m_TimeArea.TimeToPixel(this.TransitionStopTime, rect1) + this.m_RightThumbOffset;
      float pixel3 = this.m_TimeArea.TimeToPixel(this.Time, rect1);
      Rect rect2 = new Rect(pixel1, 85f, pixel2 - pixel1, 32f);
      Rect rect3 = new Rect(num1, 117f, pixelX - num1, 32f);
      Rect position3 = new Rect(num2, 0.0f, num3 - num2, 18f);
      Rect position4 = new Rect(num2, 18f, num3 - num2, rect1.height - 18f);
      Rect position5 = new Rect(num2 - 9f, 5f, 9f, 15f);
      Rect position6 = new Rect(num3, 5f, 9f, 15f);
      Rect position7 = new Rect(pixel3 - 7f, 4f, 15f, 15f);
      if (current.type == EventType.KeyDown)
      {
        if (GUIUtility.keyboardControl == this.id && this.m_DragState == Timeline.DragStates.Destination)
          this.m_DstDragOffset = 0.0f;
        if (this.m_DragState == Timeline.DragStates.LeftSelection)
          this.m_LeftThumbOffset = 0.0f;
        if (this.m_DragState == Timeline.DragStates.RightSelection)
          this.m_RightThumbOffset = 0.0f;
        if (this.m_DragState == Timeline.DragStates.FullSelection)
        {
          this.m_LeftThumbOffset = 0.0f;
          this.m_RightThumbOffset = 0.0f;
        }
      }
      if (current.type == EventType.MouseDown && rect1.Contains(current.mousePosition))
      {
        GUIUtility.hotControl = this.id;
        GUIUtility.keyboardControl = this.id;
        this.m_DragState = !position7.Contains(current.mousePosition) ? (!rect2.Contains(current.mousePosition) ? (!rect3.Contains(current.mousePosition) ? (!position5.Contains(current.mousePosition) ? (!position6.Contains(current.mousePosition) ? (!position3.Contains(current.mousePosition) ? (!position1.Contains(current.mousePosition) ? (!position2.Contains(current.mousePosition) ? Timeline.DragStates.None : Timeline.DragStates.TimeArea) : Timeline.DragStates.TimeArea) : Timeline.DragStates.FullSelection) : Timeline.DragStates.RightSelection) : Timeline.DragStates.LeftSelection) : Timeline.DragStates.Destination) : Timeline.DragStates.Source) : Timeline.DragStates.Playhead;
        current.Use();
      }
      if (current.type == EventType.MouseDrag && GUIUtility.hotControl == this.id)
      {
        switch (this.m_DragState)
        {
          case Timeline.DragStates.LeftSelection:
            if ((double) current.delta.x > 0.0 && (double) current.mousePosition.x > (double) pixel1 || (double) current.delta.x < 0.0 && (double) current.mousePosition.x < (double) num3)
              this.m_LeftThumbOffset += current.delta.x;
            this.EnforceConstraints();
            break;
          case Timeline.DragStates.RightSelection:
            if ((double) current.delta.x > 0.0 && (double) current.mousePosition.x > (double) num2 || (double) current.delta.x < 0.0)
              this.m_RightThumbOffset += current.delta.x;
            this.EnforceConstraints();
            break;
          case Timeline.DragStates.FullSelection:
            this.m_RightThumbOffset += current.delta.x;
            this.m_LeftThumbOffset += current.delta.x;
            this.EnforceConstraints();
            break;
          case Timeline.DragStates.Destination:
            this.m_DstDragOffset += current.delta.x;
            this.EnforceConstraints();
            break;
          case Timeline.DragStates.Source:
            this.m_TimeArea.m_Translation.x += current.delta.x;
            break;
          case Timeline.DragStates.Playhead:
            if ((double) current.delta.x > 0.0 && (double) current.mousePosition.x > (double) pixel1 || (double) current.delta.x < 0.0 && (double) current.mousePosition.x <= (double) this.m_TimeArea.TimeToPixel(this.SampleStopTime, rect1))
            {
              this.Time = this.m_TimeArea.PixelToTime(pixel3 + current.delta.x, rect1);
              break;
            }
            break;
          case Timeline.DragStates.TimeArea:
            this.m_TimeArea.m_Translation.x += current.delta.x;
            break;
        }
        current.Use();
        GUI.changed = true;
      }
      if (current.type == EventType.MouseUp && GUIUtility.hotControl == this.id)
      {
        this.SrcStartTime = this.m_TimeArea.PixelToTime(pixel1, rect1);
        this.SrcStopTime = this.m_TimeArea.PixelToTime(pixel2, rect1);
        this.DstStartTime = this.m_TimeArea.PixelToTime(num1, rect1);
        this.DstStopTime = this.m_TimeArea.PixelToTime(pixelX, rect1);
        this.TransitionStartTime = this.m_TimeArea.PixelToTime(num2, rect1);
        this.TransitionStopTime = this.m_TimeArea.PixelToTime(num3, rect1);
        GUI.changed = true;
        this.m_DragState = Timeline.DragStates.None;
        flag1 = this.WasDraggingData();
        this.m_LeftThumbOffset = 0.0f;
        this.m_RightThumbOffset = 0.0f;
        this.m_DstDragOffset = 0.0f;
        GUIUtility.hotControl = 0;
        current.Use();
      }
      GUI.Box(position1, GUIContent.none, this.styles.header);
      GUI.Box(position2, GUIContent.none, this.styles.background);
      this.m_TimeArea.DrawMajorTicks(position2, 30f);
      GUIContent content1 = EditorGUIUtility.TempContent(this.SrcName);
      int num4 = !this.srcLoop ? 1 : 1 + (int) (((double) num3 - (double) rect2.xMin) / ((double) rect2.xMax - (double) rect2.xMin));
      Rect position8 = rect2;
      if ((double) rect2.width < 10.0)
      {
        position8 = new Rect(rect2.x, rect2.y, (rect2.xMax - rect2.xMin) * (float) num4, rect2.height);
        num4 = 1;
      }
      for (int index = 0; index < num4; ++index)
      {
        GUI.BeginGroup(position8, GUIContent.none, this.styles.leftBlock);
        float num5 = num2 - position8.xMin;
        float width1 = num3 - num2;
        float width2 = (float) ((double) position8.xMax - (double) position8.xMin - ((double) num5 + (double) width1));
        if ((double) num5 > 0.0)
          GUI.Box(new Rect(0.0f, 0.0f, num5, rect2.height), GUIContent.none, this.styles.onLeft);
        if ((double) width1 > 0.0)
          GUI.Box(new Rect(num5, 0.0f, width1, rect2.height), GUIContent.none, this.styles.onOff);
        if ((double) width2 > 0.0)
          GUI.Box(new Rect(num5 + width1, 0.0f, width2, rect2.height), GUIContent.none, this.styles.offRight);
        float b = 1f;
        float x = this.styles.block.CalcSize(content1).x;
        float num6 = Mathf.Max(0.0f, num5) - 20f;
        float num7 = num6 + 15f;
        if ((double) num6 < (double) x && (double) num7 > 0.0 && this.m_DragState == Timeline.DragStates.LeftSelection)
          b = 0.0f;
        GUI.EndGroup();
        float a = this.styles.leftBlock.normal.textColor.a;
        if (!Mathf.Approximately(a, b) && Event.current.type == EventType.Repaint)
        {
          this.styles.leftBlock.normal.textColor = new Color(this.styles.leftBlock.normal.textColor.r, this.styles.leftBlock.normal.textColor.g, this.styles.leftBlock.normal.textColor.b, Mathf.Lerp(a, b, 0.1f));
          HandleUtility.Repaint();
        }
        GUI.Box(position8, content1, this.styles.leftBlock);
        position8 = new Rect(position8.xMax, 85f, position8.xMax - position8.xMin, 32f);
      }
      GUIContent content2 = EditorGUIUtility.TempContent(this.DstName);
      int num8 = !this.dstLoop ? 1 : 1 + (int) (((double) num3 - (double) rect3.xMin) / ((double) rect3.xMax - (double) rect3.xMin));
      position8 = rect3;
      if ((double) rect3.width < 10.0)
      {
        position8 = new Rect(rect3.x, rect3.y, (rect3.xMax - rect3.xMin) * (float) num8, rect3.height);
        num8 = 1;
      }
      for (int index = 0; index < num8; ++index)
      {
        GUI.BeginGroup(position8, GUIContent.none, this.styles.rightBlock);
        float num5 = num2 - position8.xMin;
        float width1 = num3 - num2;
        float width2 = (float) ((double) position8.xMax - (double) position8.xMin - ((double) num5 + (double) width1));
        if ((double) num5 > 0.0)
          GUI.Box(new Rect(0.0f, 0.0f, num5, rect3.height), GUIContent.none, this.styles.offLeft);
        if ((double) width1 > 0.0)
          GUI.Box(new Rect(num5, 0.0f, width1, rect3.height), GUIContent.none, this.styles.offOn);
        if ((double) width2 > 0.0)
          GUI.Box(new Rect(num5 + width1, 0.0f, width2, rect3.height), GUIContent.none, this.styles.onRight);
        float b = 1f;
        float x = this.styles.block.CalcSize(content2).x;
        float num6 = Mathf.Max(0.0f, num5) - 20f;
        float num7 = num6 + 15f;
        if ((double) num6 < (double) x && (double) num7 > 0.0 && (this.m_DragState == Timeline.DragStates.LeftSelection || this.m_DragState == Timeline.DragStates.Destination))
          b = 0.0f;
        GUI.EndGroup();
        float a = this.styles.rightBlock.normal.textColor.a;
        if (!Mathf.Approximately(a, b) && Event.current.type == EventType.Repaint)
        {
          this.styles.rightBlock.normal.textColor = new Color(this.styles.rightBlock.normal.textColor.r, this.styles.rightBlock.normal.textColor.g, this.styles.rightBlock.normal.textColor.b, Mathf.Lerp(a, b, 0.1f));
          HandleUtility.Repaint();
        }
        GUI.Box(position8, content2, this.styles.rightBlock);
        position8 = new Rect(position8.xMax, position8.yMin, position8.xMax - position8.xMin, 32f);
      }
      GUI.Box(position4, GUIContent.none, this.styles.select);
      GUI.Box(position3, GUIContent.none, this.styles.selectHead);
      this.m_TimeArea.TimeRuler(position1, 30f);
      GUI.Box(position5, GUIContent.none, !this.m_HasExitTime ? this.styles.handLeftPrev : this.styles.handLeft);
      GUI.Box(position6, GUIContent.none, this.styles.handRight);
      GUI.Box(position7, GUIContent.none, this.styles.playhead);
      Color color = Handles.color;
      Handles.color = Color.white;
      Handles.DrawLine(new Vector3(pixel3, 19f, 0.0f), new Vector3(pixel3, rect1.height, 0.0f));
      Handles.color = color;
      bool flag2 = (double) this.SrcStopTime - (double) this.SrcStartTime < 0.0333333350718021;
      bool flag3 = (double) this.DstStopTime - (double) this.DstStartTime < 0.0333333350718021;
      if (this.m_DragState == Timeline.DragStates.Destination && !flag3)
        GUI.Box(new Rect(num2 - 50f, rect3.y, 45f, rect3.height), EditorGUIUtility.TempContent(string.Format("{0:0%}", (object) (float) (((double) num2 - (double) num1) / ((double) pixelX - (double) num1)))), this.styles.timeBlockRight);
      if (this.m_DragState == Timeline.DragStates.LeftSelection)
      {
        if (!flag2)
          GUI.Box(new Rect(num2 - 50f, rect2.y, 45f, rect2.height), EditorGUIUtility.TempContent(string.Format("{0:0%}", (object) (float) (((double) num2 - (double) pixel1) / ((double) pixel2 - (double) pixel1)))), this.styles.timeBlockRight);
        if (!flag3)
          GUI.Box(new Rect(num2 - 50f, rect3.y, 45f, rect3.height), EditorGUIUtility.TempContent(string.Format("{0:0%}", (object) (float) (((double) num2 - (double) num1) / ((double) pixelX - (double) num1)))), this.styles.timeBlockRight);
      }
      if (this.m_DragState == Timeline.DragStates.RightSelection)
      {
        if (!flag2)
          GUI.Box(new Rect(num3 + 5f, rect2.y, 45f, rect2.height), EditorGUIUtility.TempContent(string.Format("{0:0%}", (object) (float) (((double) num3 - (double) pixel1) / ((double) pixel2 - (double) pixel1)))), this.styles.timeBlockLeft);
        if (!flag3)
          GUI.Box(new Rect(num3 + 5f, rect3.y, 45f, rect3.height), EditorGUIUtility.TempContent(string.Format("{0:0%}", (object) (float) (((double) num3 - (double) num1) / ((double) pixelX - (double) num1)))), this.styles.timeBlockLeft);
      }
      this.DoPivotCurves();
      GUI.EndGroup();
      return flag1;
    }

    private enum DragStates
    {
      None,
      LeftSelection,
      RightSelection,
      FullSelection,
      Destination,
      Source,
      Playhead,
      TimeArea,
    }

    private class Styles
    {
      public readonly GUIStyle block = new GUIStyle((GUIStyle) "MeTransitionBlock");
      public GUIStyle leftBlock = new GUIStyle((GUIStyle) "MeTransitionBlock");
      public GUIStyle rightBlock = new GUIStyle((GUIStyle) "MeTransitionBlock");
      public GUIStyle timeBlockRight = new GUIStyle((GUIStyle) "MeTimeLabel");
      public GUIStyle timeBlockLeft = new GUIStyle((GUIStyle) "MeTimeLabel");
      public readonly GUIStyle offLeft = new GUIStyle((GUIStyle) "MeTransOffLeft");
      public readonly GUIStyle offRight = new GUIStyle((GUIStyle) "MeTransOffRight");
      public readonly GUIStyle onLeft = new GUIStyle((GUIStyle) "MeTransOnLeft");
      public readonly GUIStyle onRight = new GUIStyle((GUIStyle) "MeTransOnRight");
      public readonly GUIStyle offOn = new GUIStyle((GUIStyle) "MeTransOff2On");
      public readonly GUIStyle onOff = new GUIStyle((GUIStyle) "MeTransOn2Off");
      public readonly GUIStyle background = new GUIStyle((GUIStyle) "MeTransitionBack");
      public readonly GUIStyle header = new GUIStyle((GUIStyle) "MeTransitionHead");
      public readonly GUIStyle handLeft = new GUIStyle((GUIStyle) "MeTransitionHandleLeft");
      public readonly GUIStyle handRight = new GUIStyle((GUIStyle) "MeTransitionHandleRight");
      public readonly GUIStyle handLeftPrev = new GUIStyle((GUIStyle) "MeTransitionHandleLeftPrev");
      public readonly GUIStyle playhead = new GUIStyle((GUIStyle) "MeTransPlayhead");
      public readonly GUIStyle selectHead = new GUIStyle((GUIStyle) "MeTransitionSelectHead");
      public readonly GUIStyle select = new GUIStyle((GUIStyle) "MeTransitionSelect");
      public readonly GUIStyle overlay = new GUIStyle((GUIStyle) "MeTransBGOver");

      public Styles()
      {
        this.timeBlockRight.alignment = TextAnchor.MiddleRight;
        this.timeBlockRight.normal.background = (Texture2D) null;
        this.timeBlockLeft.normal.background = (Texture2D) null;
      }
    }

    internal class PivotSample
    {
      public float m_Time;
      public float m_Weight;
    }
  }
}
