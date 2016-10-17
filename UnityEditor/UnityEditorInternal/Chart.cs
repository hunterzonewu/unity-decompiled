// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Chart
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Chart
  {
    private static int s_ChartHash = "Charts".GetHashCode();
    private static Chart.Styles ms_Styles = (Chart.Styles) null;
    private int m_DragItemIndex = -1;
    private int m_MouseDownIndex = -1;
    public const float kSideWidth = 170f;
    private const int kDistFromTopToFirstLabel = 20;
    private const int kLabelHeight = 11;
    private const int kCloseButtonSize = 13;
    private const float kLabelXOffset = 40f;
    private const float kWarningLabelHeightOffset = 43f;
    private Vector3[] m_CachedLineData;
    private string m_ChartSettingsName;
    private Vector2 m_DragDownPos;
    private int[] m_ChartOrderBackup;
    public string m_NotSupportedWarning;

    public void LoadAndBindSettings(string chartSettingsName, ChartData cdata)
    {
      this.m_ChartSettingsName = chartSettingsName;
      this.LoadChartsSettings(cdata);
    }

    private int MoveSelectedFrame(int selectedFrame, ChartData cdata, int direction)
    {
      int numberOfFrames = cdata.NumberOfFrames;
      int num = selectedFrame + direction;
      if (num < cdata.firstSelectableFrame || num > cdata.firstFrame + numberOfFrames)
        return selectedFrame;
      return num;
    }

    private int DoFrameSelectionDrag(float x, Rect r, ChartData cdata, int len)
    {
      int num = Mathf.RoundToInt((float) (((double) x - (double) r.x) / (double) r.width * (double) len - 0.5));
      GUI.changed = true;
      return Mathf.Clamp(num + cdata.firstFrame, cdata.firstSelectableFrame, cdata.firstFrame + len);
    }

    private int HandleFrameSelectionEvents(int selectedFrame, int chartControlID, Rect chartFrame, ChartData cdata, int len)
    {
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.MouseDown:
          if (chartFrame.Contains(current.mousePosition))
          {
            GUIUtility.keyboardControl = chartControlID;
            GUIUtility.hotControl = chartControlID;
            selectedFrame = this.DoFrameSelectionDrag(current.mousePosition.x, chartFrame, cdata, len);
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == chartControlID)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == chartControlID)
          {
            selectedFrame = this.DoFrameSelectionDrag(current.mousePosition.x, chartFrame, cdata, len);
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == chartControlID && selectedFrame >= 0)
          {
            if (current.keyCode == KeyCode.LeftArrow)
            {
              selectedFrame = this.MoveSelectedFrame(selectedFrame, cdata, -1);
              current.Use();
              break;
            }
            if (current.keyCode == KeyCode.RightArrow)
            {
              selectedFrame = this.MoveSelectedFrame(selectedFrame, cdata, 1);
              current.Use();
              break;
            }
            break;
          }
          break;
      }
      return selectedFrame;
    }

    public int DoGUI(Chart.ChartType type, int selectedFrame, ChartData cdata, ProfilerArea area, bool active, GUIContent icon, out Chart.ChartAction action)
    {
      action = Chart.ChartAction.None;
      if (cdata == null)
        return selectedFrame;
      int numberOfFrames = cdata.NumberOfFrames;
      if (Chart.ms_Styles == null)
        Chart.ms_Styles = new Chart.Styles();
      int controlId = GUIUtility.GetControlID(Chart.s_ChartHash, FocusType.Keyboard);
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, Chart.ms_Styles.background, new GUILayoutOption[1]{ GUILayout.MinHeight(120f) });
      Rect rect2 = rect1;
      rect2.x += 170f;
      rect2.width -= 170f;
      Event current = Event.current;
      if (current.GetTypeForControl(controlId) == EventType.MouseDown && rect1.Contains(current.mousePosition))
        action = Chart.ChartAction.Activated;
      if (this.m_DragItemIndex == -1)
        selectedFrame = this.HandleFrameSelectionEvents(selectedFrame, controlId, rect2, cdata, numberOfFrames);
      Rect rect3 = rect2;
      rect3.x -= 170f;
      rect3.width = 170f;
      GUI.Label(new Rect(rect3.x, rect3.y, rect3.width, 20f), GUIContent.Temp(string.Empty, icon.tooltip));
      if (current.type == EventType.Repaint)
      {
        Chart.ms_Styles.rightPane.Draw(rect2, false, false, active, false);
        Chart.ms_Styles.leftPane.Draw(rect3, EditorGUIUtility.TempContent(icon.text), false, false, active, false);
        if (this.m_NotSupportedWarning == null)
        {
          --rect2.height;
          if (type == Chart.ChartType.StackedFill)
            this.DrawChartStacked(selectedFrame, cdata, rect2);
          else
            this.DrawChartLine(selectedFrame, cdata, rect2);
        }
        else
        {
          Rect position = rect2;
          position.x += 56.1f;
          position.y += 43f;
          GUI.Label(position, this.m_NotSupportedWarning, EditorStyles.boldLabel);
        }
        rect3.x += 10f;
        rect3.y += 10f;
        GUIStyle.none.Draw(rect3, EditorGUIUtility.TempContent(icon.image), false, false, false, false);
        rect3.x += 40f;
        this.DrawLabelDragger(type, rect3, cdata);
      }
      else
      {
        rect3.y += 10f;
        this.LabelDraggerDrag(controlId, type, cdata, rect3, active);
      }
      if (area == ProfilerArea.GPU)
        GUI.Label(new Rect(rect1.x + 170f - (float) Chart.ms_Styles.performanceWarning.image.width, (float) ((double) rect1.yMax - (double) Chart.ms_Styles.performanceWarning.image.height - 2.0), (float) Chart.ms_Styles.performanceWarning.image.width, (float) Chart.ms_Styles.performanceWarning.image.height), Chart.ms_Styles.performanceWarning);
      if (GUI.Button(new Rect((float) ((double) rect1.x + 170.0 - 13.0 - 2.0), rect1.y + 2f, 13f, 13f), GUIContent.none, Chart.ms_Styles.closeButton))
        action = Chart.ChartAction.Closed;
      return selectedFrame;
    }

    private void DrawSelectedFrame(int selectedFrame, ChartData cdata, Rect r)
    {
      if (cdata.firstSelectableFrame == -1 || selectedFrame - cdata.firstSelectableFrame < 0)
        return;
      float numberOfFrames = (float) cdata.NumberOfFrames;
      selectedFrame -= cdata.firstFrame;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(7);
      GL.Color(new Color(1f, 1f, 1f, 0.6f));
      GL.Vertex3(r.x + r.width / numberOfFrames * (float) selectedFrame, r.y + 1f, 0.0f);
      GL.Vertex3((float) ((double) r.x + (double) r.width / (double) numberOfFrames * (double) selectedFrame + (double) r.width / (double) numberOfFrames), r.y + 1f, 0.0f);
      GL.Color(new Color(1f, 1f, 1f, 0.7f));
      GL.Vertex3((float) ((double) r.x + (double) r.width / (double) numberOfFrames * (double) selectedFrame + (double) r.width / (double) numberOfFrames), r.yMax, 0.0f);
      GL.Vertex3(r.x + r.width / numberOfFrames * (float) selectedFrame, r.yMax, 0.0f);
      GL.End();
    }

    private void DrawMaxValueScale(ChartData cdata, Rect r)
    {
      Handles.Label(new Vector3((float) ((double) r.x + (double) r.width / 2.0 - 20.0), r.yMin + 2f, 0.0f), "Scale: " + (object) cdata.maxValue);
    }

    private void DrawChartLine(int selectedFrame, ChartData cdata, Rect r)
    {
      for (int index = 0; index < cdata.charts.Length; ++index)
        this.DrawChartItemLine(r, cdata, index);
      if ((double) cdata.maxValue > 0.0)
        this.DrawMaxValueScale(cdata, r);
      this.DrawSelectedFrame(selectedFrame, cdata, r);
      this.DrawLabelsLine(selectedFrame, cdata, r);
    }

    private void DrawChartStacked(int selectedFrame, ChartData cdata, Rect r)
    {
      HandleUtility.ApplyWireMaterial();
      float[] sumbuf = new float[cdata.NumberOfFrames];
      for (int index = 0; index < cdata.charts.Length; ++index)
      {
        if (cdata.hasOverlay)
          this.DrawChartItemStackedOverlay(r, index, cdata, sumbuf);
        this.DrawChartItemStacked(r, index, cdata, sumbuf);
      }
      this.DrawSelectedFrame(selectedFrame, cdata, r);
      this.DrawGridStacked(r, cdata);
      this.DrawLabelsStacked(selectedFrame, cdata, r);
      if (!cdata.hasOverlay)
        return;
      string str = ProfilerDriver.selectedPropertyPath;
      if (str.Length <= 0)
        return;
      int num = str.LastIndexOf('/');
      if (num != -1)
        str = str.Substring(num + 1);
      GUIContent content = EditorGUIUtility.TempContent("Selected: " + str);
      Vector2 vector2 = EditorStyles.whiteBoldLabel.CalcSize(content);
      EditorGUI.DropShadowLabel(new Rect((float) ((double) r.x + (double) r.width - (double) vector2.x - 3.0), r.y + 3f, vector2.x, vector2.y), content, Chart.ms_Styles.selectedLabel);
    }

    internal static void DoLabel(float x, float y, string text, float alignment)
    {
      if (string.IsNullOrEmpty(text))
        return;
      GUIContent content = new GUIContent(text);
      Vector2 vector2 = Chart.ms_Styles.whiteLabel.CalcSize(content);
      EditorGUI.DoDropShadowLabel(new Rect(x + vector2.x * alignment, y, vector2.x, vector2.y), content, Chart.ms_Styles.whiteLabel, 0.3f);
    }

    private static void CorrectLabelPositions(float[] ypositions, float[] heights, float maxHeight)
    {
      int num1 = 5;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        bool flag = false;
        for (int index2 = 0; index2 < ypositions.Length; ++index2)
        {
          if ((double) heights[index2] > 0.0)
          {
            float num2 = heights[index2] / 2f;
            int index3 = index2 + 2;
            while (index3 < ypositions.Length)
            {
              if ((double) heights[index3] > 0.0)
              {
                float f = ypositions[index2] - ypositions[index3];
                float num3 = (float) (((double) heights[index2] + (double) heights[index3]) / 2.0);
                if ((double) Mathf.Abs(f) < (double) num3)
                {
                  float num4 = (float) (((double) num3 - (double) Mathf.Abs(f)) / 2.0) * Mathf.Sign(f);
                  ypositions[index2] += num4;
                  ypositions[index3] -= num4;
                  flag = true;
                }
              }
              index3 += 2;
            }
            if ((double) ypositions[index2] + (double) num2 > (double) maxHeight)
              ypositions[index2] = maxHeight - num2;
            if ((double) ypositions[index2] - (double) num2 < 0.0)
              ypositions[index2] = num2;
          }
        }
        if (!flag)
          break;
      }
    }

    private static float GetLabelHeight(string text)
    {
      GUIContent content = new GUIContent(text);
      return Chart.ms_Styles.whiteLabel.CalcSize(content).y;
    }

    private void DrawLabelsStacked(int selectedFrame, ChartData cdata, Rect r)
    {
      if (cdata.selectedLabels == null)
        return;
      int numberOfFrames = cdata.NumberOfFrames;
      if (selectedFrame < cdata.firstSelectableFrame || selectedFrame >= cdata.firstFrame + numberOfFrames)
        return;
      selectedFrame -= cdata.firstFrame;
      float num1 = r.width / (float) numberOfFrames;
      float num2 = r.x + num1 * (float) selectedFrame;
      float num3 = cdata.scale[0] * r.height;
      float[] ypositions = new float[cdata.charts.Length];
      float[] heights = new float[ypositions.Length];
      float num4 = 0.0f;
      for (int index1 = 0; index1 < cdata.charts.Length; ++index1)
      {
        ypositions[index1] = -1f;
        heights[index1] = 0.0f;
        int index2 = cdata.chartOrder[index1];
        if (cdata.charts[index2].enabled)
        {
          float num5 = cdata.charts[index2].data[selectedFrame];
          if ((double) num5 != -1.0)
          {
            float num6 = !cdata.hasOverlay ? num5 : cdata.charts[index2].overlayData[selectedFrame];
            if ((double) num6 * (double) num3 > 5.0)
            {
              ypositions[index1] = (num4 + num6 * 0.5f) * num3;
              heights[index1] = Chart.GetLabelHeight(cdata.selectedLabels[index2]);
            }
            num4 += num5;
          }
        }
      }
      Chart.CorrectLabelPositions(ypositions, heights, r.height);
      for (int index1 = 0; index1 < cdata.charts.Length; ++index1)
      {
        if ((double) heights[index1] > 0.0)
        {
          int index2 = cdata.chartOrder[index1];
          GUI.contentColor = cdata.charts[index2].color * 0.8f + Color.white * 0.2f;
          float alignment = (index2 & 1) != 0 ? 0.0f : -1f;
          float num5 = (index2 & 1) != 0 ? num1 + 1f : -1f;
          Chart.DoLabel(num2 + num5, (float) ((double) r.y + (double) r.height - (double) ypositions[index1] - 8.0), cdata.selectedLabels[index2], alignment);
        }
      }
      GUI.contentColor = Color.white;
    }

    private void DrawGridStacked(Rect r, ChartData cdata)
    {
      if (cdata.grid == null || cdata.gridLabels == null)
        return;
      GL.Begin(1);
      GL.Color(new Color(1f, 1f, 1f, 0.2f));
      for (int index = 0; index < cdata.grid.Length; ++index)
      {
        float y = (float) ((double) r.y + (double) r.height - (double) cdata.grid[index] * (double) cdata.scale[0] * (double) r.height);
        if ((double) y > (double) r.y)
        {
          GL.Vertex3(r.x + 80f, y, 0.0f);
          GL.Vertex3(r.x + r.width, y, 0.0f);
        }
      }
      GL.End();
      for (int index = 0; index < cdata.grid.Length; ++index)
      {
        float num = (float) ((double) r.y + (double) r.height - (double) cdata.grid[index] * (double) cdata.scale[0] * (double) r.height);
        if ((double) num > (double) r.y)
          Chart.DoLabel(r.x + 5f, num - 8f, cdata.gridLabels[index], 0.0f);
      }
    }

    private void DrawLabelsLine(int selectedFrame, ChartData cdata, Rect r)
    {
      if (cdata.selectedLabels == null)
        return;
      int numberOfFrames = cdata.NumberOfFrames;
      if (selectedFrame < cdata.firstSelectableFrame || selectedFrame >= cdata.firstFrame + numberOfFrames)
        return;
      selectedFrame -= cdata.firstFrame;
      float[] ypositions = new float[cdata.charts.Length];
      float[] heights = new float[ypositions.Length];
      for (int index = 0; index < cdata.charts.Length; ++index)
      {
        ypositions[index] = -1f;
        heights[index] = 0.0f;
        float num = cdata.charts[index].data[selectedFrame];
        if ((double) num != -1.0)
        {
          ypositions[index] = num * cdata.scale[index] * r.height;
          heights[index] = Chart.GetLabelHeight(cdata.selectedLabels[index]);
        }
      }
      Chart.CorrectLabelPositions(ypositions, heights, r.height);
      float num1 = r.width / (float) numberOfFrames;
      float num2 = r.x + num1 * (float) selectedFrame;
      for (int index = 0; index < cdata.charts.Length; ++index)
      {
        if ((double) heights[index] > 0.0)
        {
          GUI.contentColor = (cdata.charts[index].color + Color.white) * 0.5f;
          float alignment = (index & 1) != 0 ? 0.0f : -1f;
          float num3 = (index & 1) != 0 ? num1 + 1f : -1f;
          Chart.DoLabel(num2 + num3, (float) ((double) r.y + (double) r.height - (double) ypositions[index] - 8.0), cdata.selectedLabels[index], alignment);
        }
      }
      GUI.contentColor = Color.white;
    }

    private void DrawChartItemLine(Rect r, ChartData cdata, int index)
    {
      if (!cdata.charts[index].enabled)
        return;
      Color color = cdata.charts[index].color;
      int numberOfFrames = cdata.NumberOfFrames;
      int num1 = Mathf.Clamp(-cdata.firstFrame, 0, numberOfFrames);
      int actualNumberOfPoints = numberOfFrames - num1;
      if (actualNumberOfPoints <= 0)
        return;
      if (this.m_CachedLineData == null || numberOfFrames > this.m_CachedLineData.Length)
        this.m_CachedLineData = new Vector3[numberOfFrames];
      float num2 = r.width / (float) numberOfFrames;
      float new_x = (float) ((double) r.x + (double) num2 * 0.5 + (double) num1 * (double) num2);
      float height = r.height;
      float y = r.y;
      int index1 = num1;
      while (index1 < numberOfFrames)
      {
        float new_y = y + height;
        if ((double) cdata.charts[index].data[index1] != -1.0)
        {
          float num3 = cdata.charts[index].data[index1] * cdata.scale[index] * height;
          new_y -= num3;
        }
        this.m_CachedLineData[index1 - num1].Set(new_x, new_y, 0.0f);
        ++index1;
        new_x += num2;
      }
      Handles.color = color;
      Handles.DrawAAPolyLine(2f, actualNumberOfPoints, this.m_CachedLineData);
    }

    private void DrawChartItemStacked(Rect r, int index, ChartData cdata, float[] sumbuf)
    {
      int numberOfFrames = cdata.NumberOfFrames;
      float num1 = r.width / (float) numberOfFrames;
      index = cdata.chartOrder[index];
      if (!cdata.charts[index].enabled)
        return;
      Color color = cdata.charts[index].color;
      if (cdata.hasOverlay)
      {
        color.r *= 0.9f;
        color.g *= 0.9f;
        color.b *= 0.9f;
        color.a *= 0.4f;
      }
      Color c = color;
      c.r *= 0.8f;
      c.g *= 0.8f;
      c.b *= 0.8f;
      c.a *= 0.8f;
      GL.Begin(5);
      float x = r.x + num1 * 0.5f;
      float height = r.height;
      float y1 = r.y;
      int index1 = 0;
      while (index1 < numberOfFrames)
      {
        float y2 = y1 + height - sumbuf[index1];
        float num2 = cdata.charts[index].data[index1];
        if ((double) num2 != -1.0)
        {
          float num3 = num2 * cdata.scale[0] * height;
          if ((double) y2 - (double) num3 < (double) r.yMin)
            num3 = y2 - r.yMin;
          GL.Color(color);
          GL.Vertex3(x, y2 - num3, 0.0f);
          GL.Color(c);
          GL.Vertex3(x, y2, 0.0f);
          sumbuf[index1] += num3;
        }
        ++index1;
        x += num1;
      }
      GL.End();
    }

    private void DrawChartItemStackedOverlay(Rect r, int index, ChartData cdata, float[] sumbuf)
    {
      int numberOfFrames = cdata.NumberOfFrames;
      float num1 = r.width / (float) numberOfFrames;
      index = cdata.chartOrder[index];
      if (!cdata.charts[index].enabled)
        return;
      Color color = cdata.charts[index].color;
      Color c = color;
      c.r *= 0.8f;
      c.g *= 0.8f;
      c.b *= 0.8f;
      c.a *= 0.8f;
      GL.Begin(5);
      float x = r.x + num1 * 0.5f;
      float height = r.height;
      float y1 = r.y;
      int index1 = 0;
      while (index1 < numberOfFrames)
      {
        float y2 = y1 + height - sumbuf[index1];
        float num2 = cdata.charts[index].overlayData[index1];
        if ((double) num2 != -1.0)
        {
          float num3 = num2 * cdata.scale[0] * height;
          GL.Color(color);
          GL.Vertex3(x, y2 - num3, 0.0f);
          GL.Color(c);
          GL.Vertex3(x, y2, 0.0f);
        }
        ++index1;
        x += num1;
      }
      GL.End();
    }

    private void DrawLabelDragger(Chart.ChartType type, Rect r, ChartData cdata)
    {
      Vector2 mousePosition = Event.current.mousePosition;
      if (type == Chart.ChartType.StackedFill)
      {
        int num = 0;
        int index = cdata.charts.Length - 1;
        while (index >= 0)
        {
          Rect position = this.m_DragItemIndex != index ? new Rect(r.x, r.y + 20f + (float) (num * 11), 170f, 11f) : new Rect(r.x, mousePosition.y - this.m_DragDownPos.y, 170f, 11f);
          GUI.backgroundColor = !cdata.charts[cdata.chartOrder[index]].enabled ? Color.black : cdata.charts[cdata.chartOrder[index]].color;
          GUI.Label(position, cdata.charts[cdata.chartOrder[index]].name, Chart.ms_Styles.paneSubLabel);
          --index;
          ++num;
        }
      }
      else
      {
        for (int index = 0; index < cdata.charts.Length; ++index)
        {
          Rect position = new Rect(r.x, r.y + 20f + (float) (index * 11), 170f, 11f);
          GUI.backgroundColor = cdata.charts[index].color;
          GUI.Label(position, cdata.charts[index].name, Chart.ms_Styles.paneSubLabel);
        }
      }
      GUI.backgroundColor = Color.white;
    }

    private void LabelDraggerDrag(int chartControlID, Chart.ChartType chartType, ChartData cdata, Rect r, bool active)
    {
      if (chartType == Chart.ChartType.Line || !active)
        return;
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(chartControlID);
      switch (typeForControl)
      {
        case EventType.MouseDown:
        case EventType.MouseUp:
        case EventType.KeyDown:
        case EventType.MouseDrag:
          if (typeForControl == EventType.KeyDown && current.keyCode == KeyCode.Escape && this.m_DragItemIndex != -1)
          {
            GUIUtility.hotControl = 0;
            Array.Copy((Array) this.m_ChartOrderBackup, (Array) cdata.chartOrder, this.m_ChartOrderBackup.Length);
            this.m_DragItemIndex = -1;
            current.Use();
          }
          int num1 = 0;
          int index = cdata.charts.Length - 1;
          while (index >= 0)
          {
            if ((current.type == EventType.MouseUp && this.m_MouseDownIndex != -1 || current.type == EventType.MouseDown) && new Rect((float) ((double) r.x + 10.0 + 40.0), r.y + 20f + (float) (num1 * 11), 9f, 9f).Contains(current.mousePosition))
            {
              this.m_DragItemIndex = -1;
              if (current.type == EventType.MouseUp && this.m_MouseDownIndex == index)
              {
                this.m_MouseDownIndex = -1;
                cdata.charts[cdata.chartOrder[index]].enabled = !cdata.charts[cdata.chartOrder[index]].enabled;
                if (chartType == Chart.ChartType.StackedFill)
                  this.SaveChartsSettingsEnabled(cdata);
              }
              else
                this.m_MouseDownIndex = index;
              current.Use();
            }
            if (current.type == EventType.MouseDown)
            {
              Rect rect = new Rect(r.x, r.y + 20f + (float) (num1 * 11), 170f, 11f);
              if (rect.Contains(current.mousePosition))
              {
                this.m_MouseDownIndex = -1;
                this.m_DragItemIndex = index;
                this.m_DragDownPos = current.mousePosition;
                this.m_DragDownPos.x -= rect.x;
                this.m_DragDownPos.y -= rect.y;
                this.m_ChartOrderBackup = new int[cdata.chartOrder.Length];
                Array.Copy((Array) cdata.chartOrder, (Array) this.m_ChartOrderBackup, this.m_ChartOrderBackup.Length);
                GUIUtility.hotControl = chartControlID;
                Event.current.Use();
              }
            }
            else if (this.m_DragItemIndex != -1 && typeForControl == EventType.MouseDrag && index != this.m_DragItemIndex)
            {
              float y = current.mousePosition.y;
              float num2 = r.y + 20f + (float) (num1 * 11);
              if ((double) y >= (double) num2 && (double) y < (double) num2 + 11.0)
              {
                int num3 = cdata.chartOrder[index];
                cdata.chartOrder[index] = cdata.chartOrder[this.m_DragItemIndex];
                cdata.chartOrder[this.m_DragItemIndex] = num3;
                this.m_DragItemIndex = index;
                this.SaveChartsSettingsOrder(cdata);
              }
            }
            --index;
            ++num1;
          }
          if (typeForControl == EventType.MouseDrag && this.m_DragItemIndex != -1)
            current.Use();
          if (typeForControl != EventType.MouseUp || GUIUtility.hotControl != chartControlID)
            break;
          GUIUtility.hotControl = 0;
          this.m_DragItemIndex = -1;
          current.Use();
          break;
      }
    }

    private void LoadChartsSettings(ChartData cdata)
    {
      if (string.IsNullOrEmpty(this.m_ChartSettingsName))
        return;
      string str1 = EditorPrefs.GetString(this.m_ChartSettingsName + "Order");
      if (!string.IsNullOrEmpty(str1))
      {
        try
        {
          string[] strArray = str1.Split(',');
          if (strArray.Length == cdata.charts.Length)
          {
            for (int index = 0; index < cdata.charts.Length; ++index)
              cdata.chartOrder[index] = int.Parse(strArray[index]);
          }
        }
        catch (FormatException ex)
        {
        }
      }
      string str2 = EditorPrefs.GetString(this.m_ChartSettingsName + "Visible");
      for (int index = 0; index < cdata.charts.Length; ++index)
      {
        if (index < str2.Length && (int) str2[index] == 48)
          cdata.charts[index].enabled = false;
      }
    }

    private void SaveChartsSettingsOrder(ChartData cdata)
    {
      if (string.IsNullOrEmpty(this.m_ChartSettingsName))
        return;
      string empty = string.Empty;
      for (int index = 0; index < cdata.charts.Length; ++index)
      {
        if (empty.Length != 0)
          empty += ",";
        empty += (string) (object) cdata.chartOrder[index];
      }
      EditorPrefs.SetString(this.m_ChartSettingsName + "Order", empty);
    }

    private void SaveChartsSettingsEnabled(ChartData cdata)
    {
      string empty = string.Empty;
      for (int index = 0; index < cdata.charts.Length; ++index)
        empty += (string) (object) (char) (!cdata.charts[index].enabled ? 48 : 49);
      EditorPrefs.SetString(this.m_ChartSettingsName + "Visible", empty);
    }

    internal enum ChartAction
    {
      None,
      Activated,
      Closed,
    }

    internal enum ChartType
    {
      StackedFill,
      Line,
    }

    internal class Styles
    {
      public GUIContent performanceWarning = new GUIContent(string.Empty, (Texture) EditorGUIUtility.LoadIcon("console.warnicon.sml"), "Collecting GPU Profiler data might have overhead. Close graph if you don't need its data");
      public GUIStyle background = (GUIStyle) "OL Box";
      public GUIStyle leftPane = (GUIStyle) "ProfilerLeftPane";
      public GUIStyle rightPane = (GUIStyle) "ProfilerRightPane";
      public GUIStyle paneSubLabel = (GUIStyle) "ProfilerPaneSubLabel";
      public GUIStyle closeButton = (GUIStyle) "WinBtnClose";
      public GUIStyle whiteLabel = (GUIStyle) "ProfilerBadge";
      public GUIStyle selectedLabel = (GUIStyle) "ProfilerSelectedLabel";
    }
  }
}
