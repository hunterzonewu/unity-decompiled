// Decompiled with JetBrains decompiler
// Type: UnityEditor.DuckVolumeGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class DuckVolumeGUI : IAudioEffectPluginGUI
  {
    public static string kThresholdName = "Threshold";
    public static string kRatioName = "Ratio";
    public static string kMakeupGainName = "Make-up Gain";
    public static string kAttackTimeName = "Attack Time";
    public static string kReleaseTimeName = "Release Time";
    public static string kKneeName = "Knee";
    public static GUIStyle textStyle10 = DuckVolumeGUI.BuildGUIStyleForLabel(Color.grey, 10, false, FontStyle.Normal, TextAnchor.MiddleLeft);
    private static DuckVolumeGUI.DragType dragtype = DuckVolumeGUI.DragType.None;

    public override string Name
    {
      get
      {
        return "Duck Volume";
      }
    }

    public override string Description
    {
      get
      {
        return "Volume Ducking";
      }
    }

    public override string Vendor
    {
      get
      {
        return "Unity Technologies";
      }
    }

    public static GUIStyle BuildGUIStyleForLabel(Color color, int fontSize, bool wrapText, FontStyle fontstyle, TextAnchor anchor)
    {
      GUIStyle guiStyle = new GUIStyle();
      guiStyle.focused.background = guiStyle.onNormal.background;
      guiStyle.focused.textColor = color;
      guiStyle.alignment = anchor;
      guiStyle.fontSize = fontSize;
      guiStyle.fontStyle = fontstyle;
      guiStyle.wordWrap = wrapText;
      guiStyle.clipping = TextClipping.Overflow;
      guiStyle.normal.textColor = color;
      return guiStyle;
    }

    public static void DrawText(float x, float y, string text)
    {
      GUI.Label(new Rect(x, y - 5f, 200f, 10f), new GUIContent(text, string.Empty), DuckVolumeGUI.textStyle10);
    }

    public static void DrawLine(float x1, float y1, float x2, float y2, Color col)
    {
      Handles.color = col;
      Handles.DrawLine(new Vector3(x1, y1, 0.0f), new Vector3(x2, y2, 0.0f));
    }

    protected static Color ScaleAlpha(Color col, float blend)
    {
      return new Color(col.r, col.g, col.b, col.a * blend);
    }

    protected static void DrawVU(Rect r, float level, float blend, bool topdown)
    {
      level = 1f - level;
      Rect rect = new Rect(r.x + 1f, (float) ((double) r.y + 1.0 + (!topdown ? (double) level * (double) r.height : 0.0)), r.width - 2f, (float) ((double) r.y - 2.0 + (!topdown ? (double) r.height - (double) level * (double) r.height : (double) level * (double) r.height)));
      AudioMixerDrawUtils.DrawRect(r, new Color(0.1f, 0.1f, 0.1f));
      AudioMixerDrawUtils.DrawRect(rect, new Color(0.6f, 0.2f, 0.2f));
    }

    private static float EvaluateDuckingVolume(float x, float ratio, float threshold, float makeupGain, float knee, float dbRange, float dbMin)
    {
      float num1 = 1f / ratio;
      float num2 = threshold;
      float num3 = makeupGain;
      float num4 = knee;
      float num5 = (double) knee <= 0.0 ? 0.0f : (float) (((double) num1 - 1.0) / (4.0 * (double) knee));
      float num6 = num2 - knee;
      float num7 = x * dbRange + dbMin;
      float num8 = num7;
      float num9 = num7 - num2;
      if ((double) num9 > -(double) num4 && (double) num9 < (double) num4)
      {
        float num10 = num9 + num4;
        num8 = num10 * (float) ((double) num5 * (double) num10 + 1.0) + num6;
      }
      else if ((double) num9 > 0.0)
        num8 = num2 + num1 * num9;
      return (float) (2.0 * ((double) num8 + (double) num3 - (double) dbMin) / (double) dbRange - 1.0);
    }

    private static bool CurveDisplay(IAudioEffectPlugin plugin, Rect r0, ref float threshold, ref float ratio, ref float makeupGain, ref float attackTime, ref float releaseTime, ref float knee, float sidechainLevel, float outputLevel, float blend)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DuckVolumeGUI.\u003CCurveDisplay\u003Ec__AnonStorey5C displayCAnonStorey5C = new DuckVolumeGUI.\u003CCurveDisplay\u003Ec__AnonStorey5C();
      // ISSUE: reference to a compiler-generated field
      displayCAnonStorey5C.blend = blend;
      Event current = Event.current;
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      Rect r = AudioCurveRendering.BeginCurveFrame(r0);
      float num1 = 10f;
      float minRange1;
      float maxRange1;
      float defaultValue1;
      plugin.GetFloatParameterInfo(DuckVolumeGUI.kThresholdName, out minRange1, out maxRange1, out defaultValue1);
      float minRange2;
      float maxRange2;
      float defaultValue2;
      plugin.GetFloatParameterInfo(DuckVolumeGUI.kRatioName, out minRange2, out maxRange2, out defaultValue2);
      float minRange3;
      float maxRange3;
      float defaultValue3;
      plugin.GetFloatParameterInfo(DuckVolumeGUI.kMakeupGainName, out minRange3, out maxRange3, out defaultValue3);
      float minRange4;
      float maxRange4;
      float defaultValue4;
      plugin.GetFloatParameterInfo(DuckVolumeGUI.kKneeName, out minRange4, out maxRange4, out defaultValue4);
      // ISSUE: reference to a compiler-generated field
      displayCAnonStorey5C.dbRange = 100f;
      // ISSUE: reference to a compiler-generated field
      displayCAnonStorey5C.dbMin = -80f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float num2 = r.width * (threshold - displayCAnonStorey5C.dbMin) / displayCAnonStorey5C.dbRange;
      bool flag = false;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (r.Contains(Event.current.mousePosition) && current.button == 0)
          {
            DuckVolumeGUI.dragtype = DuckVolumeGUI.DragType.None;
            GUIUtility.hotControl = controlId;
            EditorGUIUtility.SetWantsMouseJumping(1);
            current.Use();
            DuckVolumeGUI.dragtype = (double) Mathf.Abs(r.x + num2 - current.mousePosition.x) < 10.0 ? DuckVolumeGUI.DragType.ThresholdAndKnee : ((double) current.mousePosition.x >= (double) r.x + (double) num2 ? DuckVolumeGUI.DragType.Ratio : DuckVolumeGUI.DragType.MakeupGain);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId && current.button == 0)
          {
            DuckVolumeGUI.dragtype = DuckVolumeGUI.DragType.None;
            GUIUtility.hotControl = 0;
            EditorGUIUtility.SetWantsMouseJumping(0);
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            float num3 = !current.alt ? 1f : 0.25f;
            if (DuckVolumeGUI.dragtype == DuckVolumeGUI.DragType.ThresholdAndKnee)
            {
              if ((double) Mathf.Abs(current.delta.x) < (double) Mathf.Abs(current.delta.y))
                knee = Mathf.Clamp(knee + current.delta.y * 0.5f * num3, minRange4, maxRange4);
              else
                threshold = Mathf.Clamp(threshold + current.delta.x * 0.1f * num3, minRange1, maxRange1);
            }
            else if (DuckVolumeGUI.dragtype == DuckVolumeGUI.DragType.Ratio)
              ratio = Mathf.Clamp(ratio + current.delta.y * ((double) ratio <= 1.0 ? 3f / 1000f : 0.05f) * num3, minRange2, maxRange2);
            else if (DuckVolumeGUI.dragtype == DuckVolumeGUI.DragType.MakeupGain)
              makeupGain = Mathf.Clamp(makeupGain - current.delta.y * 0.5f * num3, minRange3, maxRange3);
            else
              Debug.LogError((object) "Drag: Unhandled enum");
            flag = true;
            current.Use();
            break;
          }
          break;
      }
      if (current.type == EventType.Repaint)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        DuckVolumeGUI.\u003CCurveDisplay\u003Ec__AnonStorey5D displayCAnonStorey5D = new DuckVolumeGUI.\u003CCurveDisplay\u003Ec__AnonStorey5D();
        // ISSUE: reference to a compiler-generated field
        displayCAnonStorey5D.\u003C\u003Ef__ref\u002492 = displayCAnonStorey5C;
        HandleUtility.ApplyWireMaterial();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num3 = r.height * (float) (1.0 - ((double) threshold - (double) displayCAnonStorey5C.dbMin + (double) makeupGain) / (double) displayCAnonStorey5C.dbRange);
        Color col = new Color(0.7f, 0.7f, 0.7f);
        Color black = Color.black;
        // ISSUE: reference to a compiler-generated field
        displayCAnonStorey5D.duckGradient = 1f / ratio;
        // ISSUE: reference to a compiler-generated field
        displayCAnonStorey5D.duckThreshold = threshold;
        // ISSUE: reference to a compiler-generated field
        displayCAnonStorey5D.duckSidechainLevel = sidechainLevel;
        // ISSUE: reference to a compiler-generated field
        displayCAnonStorey5D.duckMakeupGain = makeupGain;
        // ISSUE: reference to a compiler-generated field
        displayCAnonStorey5D.duckKnee = knee;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        displayCAnonStorey5D.duckKneeC1 = (double) knee <= 0.0 ? 0.0f : (float) (((double) displayCAnonStorey5D.duckGradient - 1.0) / (4.0 * (double) knee));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        displayCAnonStorey5D.duckKneeC2 = displayCAnonStorey5D.duckThreshold - knee;
        // ISSUE: reference to a compiler-generated method
        AudioCurveRendering.DrawFilledCurve(r, new AudioCurveRendering.AudioCurveAndColorEvaluator(displayCAnonStorey5D.\u003C\u003Em__9D));
        if (DuckVolumeGUI.dragtype == DuckVolumeGUI.DragType.MakeupGain)
        {
          // ISSUE: reference to a compiler-generated method
          AudioCurveRendering.DrawCurve(r, new AudioCurveRendering.AudioCurveEvaluator(displayCAnonStorey5D.\u003C\u003Em__9E), Color.white);
        }
        // ISSUE: reference to a compiler-generated field
        DuckVolumeGUI.textStyle10.normal.textColor = DuckVolumeGUI.ScaleAlpha(col, displayCAnonStorey5C.blend);
        EditorGUI.DrawRect(new Rect(r.x + num2, r.y, 1f, r.height), DuckVolumeGUI.textStyle10.normal.textColor);
        DuckVolumeGUI.DrawText((float) ((double) r.x + (double) num2 + 4.0), r.y + 6f, string.Format("Threshold: {0:F1} dB", (object) threshold));
        // ISSUE: reference to a compiler-generated field
        DuckVolumeGUI.textStyle10.normal.textColor = DuckVolumeGUI.ScaleAlpha(black, displayCAnonStorey5C.blend);
        DuckVolumeGUI.DrawText(r.x + 4f, (float) ((double) r.y + (double) r.height - 10.0), (double) sidechainLevel >= -80.0 ? string.Format("Input: {0:F1} dB", (object) sidechainLevel) : "Input: None");
        if (DuckVolumeGUI.dragtype == DuckVolumeGUI.DragType.Ratio)
        {
          float num4 = r.height / r.width;
          Handles.DrawAAPolyLine(2f, new Color[2]
          {
            Color.black,
            Color.black
          }, new Vector3[2]
          {
            new Vector3(r.x + num2 + r.width, (float) ((double) r.y + (double) num3 - (double) num4 * (double) r.width), 0.0f),
            new Vector3(r.x + num2 - r.width, (float) ((double) r.y + (double) num3 + (double) num4 * (double) r.width), 0.0f)
          });
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Handles.DrawAAPolyLine(3f, new Color[2]
          {
            Color.white,
            Color.white
          }, new Vector3[2]
          {
            new Vector3(r.x + num2 + r.width, (float) ((double) r.y + (double) num3 - (double) num4 * (double) displayCAnonStorey5D.duckGradient * (double) r.width), 0.0f),
            new Vector3(r.x + num2 - r.width, (float) ((double) r.y + (double) num3 + (double) num4 * (double) displayCAnonStorey5D.duckGradient * (double) r.width), 0.0f)
          });
        }
        else if (DuckVolumeGUI.dragtype == DuckVolumeGUI.DragType.ThresholdAndKnee)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float x1 = (threshold - knee - displayCAnonStorey5C.dbMin) / displayCAnonStorey5C.dbRange;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float x2 = (threshold + knee - displayCAnonStorey5C.dbMin) / displayCAnonStorey5C.dbRange;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float duckingVolume1 = DuckVolumeGUI.EvaluateDuckingVolume(x1, ratio, threshold, makeupGain, knee, displayCAnonStorey5C.dbRange, displayCAnonStorey5C.dbMin);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float duckingVolume2 = DuckVolumeGUI.EvaluateDuckingVolume(x2, ratio, threshold, makeupGain, knee, displayCAnonStorey5C.dbRange, displayCAnonStorey5C.dbMin);
          float y1 = r.yMax - (float) (((double) duckingVolume1 + 1.0) * 0.5) * r.height;
          float y2 = r.yMax - (float) (((double) duckingVolume2 + 1.0) * 0.5) * r.height;
          EditorGUI.DrawRect(new Rect(r.x + x1 * r.width, y1, 1f, r.height - y1), new Color(0.0f, 0.0f, 0.0f, 0.5f));
          EditorGUI.DrawRect(new Rect((float) ((double) r.x + (double) x2 * (double) r.width - 1.0), y2, 1f, r.height - y2), new Color(0.0f, 0.0f, 0.0f, 0.5f));
          EditorGUI.DrawRect(new Rect((float) ((double) r.x + (double) num2 - 1.0), r.y, 3f, r.height), Color.white);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        outputLevel = (Mathf.Clamp(outputLevel - makeupGain, displayCAnonStorey5C.dbMin, displayCAnonStorey5C.dbMin + displayCAnonStorey5C.dbRange) - displayCAnonStorey5C.dbMin) / displayCAnonStorey5C.dbRange;
        if (EditorApplication.isPlaying)
        {
          // ISSUE: reference to a compiler-generated field
          DuckVolumeGUI.DrawVU(new Rect((float) ((double) r.x + (double) r.width - (double) num1 + 2.0), r.y + 2f, num1 - 4f, r.height - 4f), outputLevel, displayCAnonStorey5C.blend, true);
        }
      }
      AudioCurveRendering.EndCurveFrame();
      return flag;
    }

    public override bool OnGUI(IAudioEffectPlugin plugin)
    {
      float blend = !plugin.IsPluginEditableAndEnabled() ? 0.5f : 1f;
      float threshold;
      plugin.GetFloatParameter(DuckVolumeGUI.kThresholdName, out threshold);
      float ratio;
      plugin.GetFloatParameter(DuckVolumeGUI.kRatioName, out ratio);
      float makeupGain;
      plugin.GetFloatParameter(DuckVolumeGUI.kMakeupGainName, out makeupGain);
      float attackTime;
      plugin.GetFloatParameter(DuckVolumeGUI.kAttackTimeName, out attackTime);
      float releaseTime;
      plugin.GetFloatParameter(DuckVolumeGUI.kReleaseTimeName, out releaseTime);
      float knee;
      plugin.GetFloatParameter(DuckVolumeGUI.kKneeName, out knee);
      float[] data;
      plugin.GetFloatBuffer("Metering", out data, 2);
      GUILayout.Space(5f);
      Rect rect = GUILayoutUtility.GetRect(200f, 160f, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      if (DuckVolumeGUI.CurveDisplay(plugin, rect, ref threshold, ref ratio, ref makeupGain, ref attackTime, ref releaseTime, ref knee, data[0], data[1], blend))
      {
        plugin.SetFloatParameter(DuckVolumeGUI.kThresholdName, threshold);
        plugin.SetFloatParameter(DuckVolumeGUI.kRatioName, ratio);
        plugin.SetFloatParameter(DuckVolumeGUI.kMakeupGainName, makeupGain);
        plugin.SetFloatParameter(DuckVolumeGUI.kAttackTimeName, attackTime);
        plugin.SetFloatParameter(DuckVolumeGUI.kReleaseTimeName, releaseTime);
        plugin.SetFloatParameter(DuckVolumeGUI.kKneeName, knee);
      }
      return true;
    }

    public enum DragType
    {
      None,
      ThresholdAndKnee,
      Ratio,
      MakeupGain,
    }
  }
}
