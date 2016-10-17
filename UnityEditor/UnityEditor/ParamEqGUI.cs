// Decompiled with JetBrains decompiler
// Type: UnityEditor.ParamEqGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class ParamEqGUI : IAudioEffectPluginGUI
  {
    public static string kCenterFreqName = "Center freq";
    public static string kOctaveRangeName = "Octave range";
    public static string kFrequencyGainName = "Frequency gain";
    public static GUIStyle textStyle10 = ParamEqGUI.BuildGUIStyleForLabel(Color.grey, 10, false, FontStyle.Normal, TextAnchor.MiddleCenter);
    private const bool useLogScale = true;

    public override string Name
    {
      get
      {
        return "ParamEQ";
      }
    }

    public override string Description
    {
      get
      {
        return "Parametric equalizer";
      }
    }

    public override string Vendor
    {
      get
      {
        return "Firelight Technologies";
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

    private static void DrawFrequencyTickMarks(Rect r, float samplerate, bool logScale, Color col)
    {
      ParamEqGUI.textStyle10.normal.textColor = col;
      float num1 = r.x;
      float num2 = 60f;
      float num3 = 0.0f;
      while ((double) num3 < 1.0)
      {
        float num4 = (float) ParamEqGUI.MapNormalizedFrequency((double) num3, (double) samplerate, logScale, true);
        float x = r.x + num3 * r.width;
        if ((double) x - (double) num1 > (double) num2)
        {
          EditorGUI.DrawRect(new Rect(x, r.yMax - 5f, 1f, 5f), col);
          GUI.Label(new Rect(x, r.yMax - 22f, 1f, 15f), (double) num4 >= 1000.0 ? string.Format("{0:F0} kHz", (object) (float) ((double) num4 * (1.0 / 1000.0))) : string.Format("{0:F0} Hz", (object) num4), ParamEqGUI.textStyle10);
          num1 = x;
        }
        num3 += 0.01f;
      }
    }

    protected static Color ScaleAlpha(Color col, float blend)
    {
      return new Color(col.r, col.g, col.b, col.a * blend);
    }

    private static double MapNormalizedFrequency(double f, double sr, bool useLogScale, bool forward)
    {
      double num = 0.5 * sr;
      if (useLogScale)
      {
        if (forward)
          return 10.0 * Math.Pow(num / 10.0, f);
        return Math.Log(f / 10.0) / Math.Log(num / 10.0);
      }
      if (forward)
        return f * num;
      return f / num;
    }

    private static bool ParamEqualizerCurveEditor(IAudioEffectPlugin plugin, Rect r, ref float centerFreq, ref float bandwidth, ref float gain, float blend)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ParamEqGUI.\u003CParamEqualizerCurveEditor\u003Ec__AnonStorey5E editorCAnonStorey5E = new ParamEqGUI.\u003CParamEqualizerCurveEditor\u003Ec__AnonStorey5E();
      // ISSUE: reference to a compiler-generated field
      editorCAnonStorey5E.plugin = plugin;
      Event current = Event.current;
      int controlId = GUIUtility.GetControlID(FocusType.Passive);
      r = AudioCurveRendering.BeginCurveFrame(r);
      float minRange1;
      float maxRange1;
      float defaultValue1;
      // ISSUE: reference to a compiler-generated field
      editorCAnonStorey5E.plugin.GetFloatParameterInfo(ParamEqGUI.kCenterFreqName, out minRange1, out maxRange1, out defaultValue1);
      float minRange2;
      float maxRange2;
      float defaultValue2;
      // ISSUE: reference to a compiler-generated field
      editorCAnonStorey5E.plugin.GetFloatParameterInfo(ParamEqGUI.kOctaveRangeName, out minRange2, out maxRange2, out defaultValue2);
      float minRange3;
      float maxRange3;
      float defaultValue3;
      // ISSUE: reference to a compiler-generated field
      editorCAnonStorey5E.plugin.GetFloatParameterInfo(ParamEqGUI.kFrequencyGainName, out minRange3, out maxRange3, out defaultValue3);
      bool flag = false;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (r.Contains(Event.current.mousePosition) && current.button == 0)
          {
            GUIUtility.hotControl = controlId;
            EditorGUIUtility.SetWantsMouseJumping(1);
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId && current.button == 0)
          {
            GUIUtility.hotControl = 0;
            EditorGUIUtility.SetWantsMouseJumping(0);
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            float num = !Event.current.alt ? 1f : 0.25f;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            centerFreq = Mathf.Clamp((float) ParamEqGUI.MapNormalizedFrequency(ParamEqGUI.MapNormalizedFrequency((double) centerFreq, (double) editorCAnonStorey5E.plugin.GetSampleRate(), true, false) + (double) current.delta.x / (double) r.width, (double) editorCAnonStorey5E.plugin.GetSampleRate(), true, true), minRange1, maxRange1);
            if (Event.current.shift)
              bandwidth = Mathf.Clamp(bandwidth - current.delta.y * 0.02f * num, minRange2, maxRange2);
            else
              gain = Mathf.Clamp(gain - current.delta.y * 0.01f * num, minRange3, maxRange3);
            flag = true;
            current.Use();
            break;
          }
          break;
      }
      if (Event.current.type == EventType.Repaint)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ParamEqGUI.\u003CParamEqualizerCurveEditor\u003Ec__AnonStorey5F editorCAnonStorey5F = new ParamEqGUI.\u003CParamEqualizerCurveEditor\u003Ec__AnonStorey5F();
        // ISSUE: reference to a compiler-generated field
        editorCAnonStorey5F.\u003C\u003Ef__ref\u002494 = editorCAnonStorey5E;
        // ISSUE: reference to a compiler-generated field
        EditorGUI.DrawRect(new Rect((float) ParamEqGUI.MapNormalizedFrequency((double) centerFreq, (double) editorCAnonStorey5E.plugin.GetSampleRate(), true, false) * r.width + r.x, r.y, 1f, r.height), GUIUtility.hotControl != controlId ? new Color(0.4f, 0.4f, 0.4f) : new Color(0.6f, 0.6f, 0.6f));
        HandleUtility.ApplyWireMaterial();
        double num1 = 3.1415926;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        editorCAnonStorey5F.wm = -2.0 * num1 / (double) editorCAnonStorey5E.plugin.GetSampleRate();
        // ISSUE: reference to a compiler-generated field
        double num2 = 2.0 * num1 * (double) centerFreq / (double) editorCAnonStorey5E.plugin.GetSampleRate();
        double num3 = 1.0 / (double) bandwidth;
        double num4 = (double) gain;
        double num5 = Math.Sin(num2) / (2.0 * num3);
        // ISSUE: reference to a compiler-generated field
        editorCAnonStorey5F.b0 = 1.0 + num5 * num4;
        // ISSUE: reference to a compiler-generated field
        editorCAnonStorey5F.b1 = -2.0 * Math.Cos(num2);
        // ISSUE: reference to a compiler-generated field
        editorCAnonStorey5F.b2 = 1.0 - num5 * num4;
        // ISSUE: reference to a compiler-generated field
        editorCAnonStorey5F.a0 = 1.0 + num5 / num4;
        // ISSUE: reference to a compiler-generated field
        editorCAnonStorey5F.a1 = -2.0 * Math.Cos(num2);
        // ISSUE: reference to a compiler-generated field
        editorCAnonStorey5F.a2 = 1.0 - num5 / num4;
        // ISSUE: reference to a compiler-generated method
        AudioCurveRendering.DrawCurve(r, new AudioCurveRendering.AudioCurveEvaluator(editorCAnonStorey5F.\u003C\u003Em__9F), ParamEqGUI.ScaleAlpha(AudioCurveRendering.kAudioOrange, blend));
      }
      // ISSUE: reference to a compiler-generated field
      ParamEqGUI.DrawFrequencyTickMarks(r, (float) editorCAnonStorey5E.plugin.GetSampleRate(), true, new Color(1f, 1f, 1f, 0.3f * blend));
      AudioCurveRendering.EndCurveFrame();
      return flag;
    }

    public override bool OnGUI(IAudioEffectPlugin plugin)
    {
      float blend = !plugin.IsPluginEditableAndEnabled() ? 0.5f : 1f;
      float centerFreq;
      plugin.GetFloatParameter(ParamEqGUI.kCenterFreqName, out centerFreq);
      float bandwidth;
      plugin.GetFloatParameter(ParamEqGUI.kOctaveRangeName, out bandwidth);
      float gain;
      plugin.GetFloatParameter(ParamEqGUI.kFrequencyGainName, out gain);
      GUILayout.Space(5f);
      Rect rect = GUILayoutUtility.GetRect(200f, 100f, new GUILayoutOption[1]
      {
        GUILayout.ExpandWidth(true)
      });
      if (ParamEqGUI.ParamEqualizerCurveEditor(plugin, rect, ref centerFreq, ref bandwidth, ref gain, blend))
      {
        plugin.SetFloatParameter(ParamEqGUI.kCenterFreqName, centerFreq);
        plugin.SetFloatParameter(ParamEqGUI.kOctaveRangeName, bandwidth);
        plugin.SetFloatParameter(ParamEqGUI.kFrequencyGainName, gain);
      }
      return true;
    }
  }
}
