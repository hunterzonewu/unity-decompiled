// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioCurveRendering
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Antialiased curve rendering functionality used by audio tools in the editor.</para>
  /// </summary>
  public class AudioCurveRendering
  {
    private static float pixelEpsilon = 0.005f;
    public static readonly Color kAudioOrange = new Color(1f, 0.6588235f, 0.02745098f);
    private static Vector3[] s_PointCache;

    public static Rect BeginCurveFrame(Rect r)
    {
      AudioCurveRendering.DrawCurveBackground(r);
      r = AudioCurveRendering.DrawCurveFrame(r);
      GUI.BeginGroup(r);
      return new Rect(0.0f, 0.0f, r.width, r.height);
    }

    public static void EndCurveFrame()
    {
      GUI.EndGroup();
    }

    public static Rect DrawCurveFrame(Rect r)
    {
      if (Event.current.type != EventType.Repaint)
        return r;
      EditorStyles.colorPickerBox.Draw(r, false, false, false, false);
      ++r.x;
      ++r.y;
      r.width -= 2f;
      r.height -= 2f;
      return r;
    }

    public static void DrawCurveBackground(Rect r)
    {
      EditorGUI.DrawRect(r, new Color(0.3f, 0.3f, 0.3f));
    }

    public static void DrawFilledCurve(Rect r, AudioCurveRendering.AudioCurveEvaluator eval, Color curveColor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      AudioCurveRendering.DrawFilledCurve(r, new AudioCurveRendering.AudioCurveAndColorEvaluator(new AudioCurveRendering.\u003CDrawFilledCurve\u003Ec__AnonStorey5B()
      {
        curveColor = curveColor,
        eval = eval
      }.\u003C\u003Em__9C));
    }

    public static void DrawFilledCurve(Rect r, AudioCurveRendering.AudioCurveAndColorEvaluator eval)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
      float num1 = 1f / pixelsPerPoint;
      float num2 = 0.5f * num1;
      float num3 = Mathf.Ceil(r.width) * pixelsPerPoint;
      float num4 = Mathf.Floor(r.x) + AudioCurveRendering.pixelEpsilon;
      float num5 = 1f / (num3 - 1f);
      float max = r.height * 0.5f;
      float num6 = r.y + 0.5f * r.height;
      float y = r.y + r.height;
      Color col;
      float b = Mathf.Clamp(max * eval(0.0f, out col), -max, max);
      for (int index = 0; (double) index < (double) num3; ++index)
      {
        float x = num4 + (float) index * num1;
        float a = Mathf.Clamp(max * eval((float) index * num5, out col), -max, max);
        float num7 = Mathf.Min(a, b) - num2;
        float num8 = Mathf.Max(a, b) + num2;
        GL.Color(new Color(col.r, col.g, col.b, 0.0f));
        AudioMixerDrawUtils.Vertex(x, num6 - num8);
        GL.Color(col);
        AudioMixerDrawUtils.Vertex(x, num6 - num7);
        AudioMixerDrawUtils.Vertex(x, num6 - num7);
        AudioMixerDrawUtils.Vertex(x, y);
        b = a;
      }
      GL.End();
    }

    private static void Sort2(ref float minValue, ref float maxValue)
    {
      if ((double) minValue <= (double) maxValue)
        return;
      float num = minValue;
      minValue = maxValue;
      maxValue = num;
    }

    public static void DrawMinMaxFilledCurve(Rect r, AudioCurveRendering.AudioMinMaxCurveAndColorEvaluator eval)
    {
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
      float num1 = 1f / pixelsPerPoint;
      float num2 = 0.5f * num1;
      float num3 = Mathf.Ceil(r.width) * pixelsPerPoint;
      float num4 = Mathf.Floor(r.x) + AudioCurveRendering.pixelEpsilon;
      float num5 = 1f / (num3 - 1f);
      float num6 = r.height * 0.5f;
      float num7 = r.y + 0.5f * r.height;
      Color col;
      float minValue1;
      float maxValue1;
      eval(0.0001f, out col, out minValue1, out maxValue1);
      AudioCurveRendering.Sort2(ref minValue1, ref maxValue1);
      float b1 = num7 - num6 * Mathf.Clamp(maxValue1, -1f, 1f);
      float b2 = num7 - num6 * Mathf.Clamp(minValue1, -1f, 1f);
      float y1 = r.y;
      float max = r.y + r.height;
      for (int index = 0; (double) index < (double) num3; ++index)
      {
        float x = num4 + (float) index * num1;
        eval((float) index * num5, out col, out minValue1, out maxValue1);
        AudioCurveRendering.Sort2(ref minValue1, ref maxValue1);
        Color c = new Color(col.r, col.g, col.b, 0.0f);
        float a1 = num7 - num6 * Mathf.Clamp(maxValue1, -1f, 1f);
        float a2 = num7 - num6 * Mathf.Clamp(minValue1, -1f, 1f);
        float minValue2 = Mathf.Clamp(Mathf.Min(a1, b1) - num2, y1, max);
        float y2 = Mathf.Clamp(Mathf.Max(a1, b1) + num2, y1, max);
        float y3 = Mathf.Clamp(Mathf.Min(a2, b2) - num2, y1, max);
        float maxValue2 = Mathf.Clamp(Mathf.Max(a2, b2) + num2, y1, max);
        AudioCurveRendering.Sort2(ref minValue2, ref y3);
        AudioCurveRendering.Sort2(ref y2, ref maxValue2);
        AudioCurveRendering.Sort2(ref minValue2, ref y2);
        AudioCurveRendering.Sort2(ref y3, ref maxValue2);
        AudioCurveRendering.Sort2(ref y2, ref y3);
        GL.Color(c);
        AudioMixerDrawUtils.Vertex(x, minValue2);
        GL.Color(col);
        AudioMixerDrawUtils.Vertex(x, y2);
        AudioMixerDrawUtils.Vertex(x, y2);
        AudioMixerDrawUtils.Vertex(x, y3);
        AudioMixerDrawUtils.Vertex(x, y3);
        GL.Color(c);
        AudioMixerDrawUtils.Vertex(x, maxValue2);
        b2 = a2;
        b1 = a1;
      }
      GL.End();
    }

    public static void DrawSymmetricFilledCurve(Rect r, AudioCurveRendering.AudioCurveAndColorEvaluator eval)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(1);
      float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
      float num1 = 1f / pixelsPerPoint;
      float num2 = 0.5f * num1;
      float num3 = Mathf.Ceil(r.width) * pixelsPerPoint;
      float num4 = Mathf.Floor(r.x) + AudioCurveRendering.pixelEpsilon;
      float num5 = 1f / (num3 - 1f);
      float num6 = r.height * 0.5f;
      float num7 = r.y + 0.5f * r.height;
      Color col;
      float b = Mathf.Clamp(num6 * eval(0.0001f, out col), 0.0f, num6);
      for (int index = 0; (double) index < (double) num3; ++index)
      {
        float x = num4 + (float) index * num1;
        float a = Mathf.Clamp(num6 * eval((float) index * num5, out col), 0.0f, num6);
        float num8 = Mathf.Max(Mathf.Min(a, b) - num2, 0.0f);
        float num9 = Mathf.Min(Mathf.Max(a, b) + num2, num6);
        Color c = new Color(col.r, col.g, col.b, 0.0f);
        GL.Color(c);
        AudioMixerDrawUtils.Vertex(x, num7 - num9);
        GL.Color(col);
        AudioMixerDrawUtils.Vertex(x, num7 - num8);
        AudioMixerDrawUtils.Vertex(x, num7 - num8);
        AudioMixerDrawUtils.Vertex(x, num7 + num8);
        AudioMixerDrawUtils.Vertex(x, num7 + num8);
        GL.Color(c);
        AudioMixerDrawUtils.Vertex(x, num7 + num9);
        b = a;
      }
      GL.End();
    }

    public static void DrawCurve(Rect r, AudioCurveRendering.AudioCurveEvaluator eval, Color curveColor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      int num1 = (int) Mathf.Ceil(r.width);
      float num2 = r.height * 0.5f;
      float num3 = 1f / (float) (num1 - 1);
      Vector3[] pointCache = AudioCurveRendering.GetPointCache(num1);
      for (int index = 0; index < num1; ++index)
      {
        pointCache[index].x = (float) index + r.x;
        pointCache[index].y = num2 - num2 * eval((float) index * num3) + r.y;
        pointCache[index].z = 0.0f;
      }
      GUI.BeginClip(r);
      Handles.color = curveColor;
      Handles.DrawAAPolyLine(3f, num1, pointCache);
      GUI.EndClip();
    }

    private static Vector3[] GetPointCache(int numPoints)
    {
      if (AudioCurveRendering.s_PointCache == null || AudioCurveRendering.s_PointCache.Length != numPoints)
        AudioCurveRendering.s_PointCache = new Vector3[numPoints];
      return AudioCurveRendering.s_PointCache;
    }

    public static void DrawGradientRect(Rect r, Color c1, Color c2, float blend, bool horizontal)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(7);
      if (horizontal)
      {
        GL.Color(new Color(c1.r, c1.g, c1.b, c1.a * blend));
        GL.Vertex3(r.x, r.y, 0.0f);
        GL.Vertex3(r.x + r.width, r.y, 0.0f);
        GL.Color(new Color(c2.r, c2.g, c2.b, c2.a * blend));
        GL.Vertex3(r.x + r.width, r.y + r.height, 0.0f);
        GL.Vertex3(r.x, r.y + r.height, 0.0f);
      }
      else
      {
        GL.Color(new Color(c1.r, c1.g, c1.b, c1.a * blend));
        GL.Vertex3(r.x, r.y + r.height, 0.0f);
        GL.Vertex3(r.x, r.y, 0.0f);
        GL.Color(new Color(c2.r, c2.g, c2.b, c2.a * blend));
        GL.Vertex3(r.x + r.width, r.y, 0.0f);
        GL.Vertex3(r.x + r.width, r.y + r.height, 0.0f);
      }
      GL.End();
    }

    /// <summary>
    ///   <para>Curve evaluation function used to evaluate the curve y-value and at the specified point.</para>
    /// </summary>
    /// <param name="x">Normalized x-position in the range [0; 1] at which the curve should be evaluated.</param>
    public delegate float AudioCurveEvaluator(float x);

    /// <summary>
    ///   <para>Curve evaluation function that allows simultaneous evaluation of the curve y-value and a color of the curve at that point.</para>
    /// </summary>
    /// <param name="x">Normalized x-position in the range [0; 1] at which the curve should be evaluated.</param>
    /// <param name="col">Color of the curve at the evaluated point.</param>
    public delegate float AudioCurveAndColorEvaluator(float x, out Color col);

    /// <summary>
    ///   <para>Curve evaluation function that allows simultaneous evaluation of the min- and max-curves. The returned minValue and maxValue values are expected to be in the range [-1; 1] and a value of 0 corresponds to the vertical center of the rectangle that is drawn into. Values outside of this range will be clamped. Additionally the color of the curve at this point is evaluated.</para>
    /// </summary>
    /// <param name="x">Normalized x-position in the range [0; 1] at which the min- and max-curves should be evaluated.</param>
    /// <param name="col">Color of the curve at the specified evaluation point.</param>
    /// <param name="minValue">Returned value of the minimum curve. Clamped to [-1; 1].</param>
    /// <param name="maxValue">Returned value of the maximum curve. Clamped to [-1; 1].</param>
    public delegate void AudioMinMaxCurveAndColorEvaluator(float x, out Color col, out float minValue, out float maxValue);
  }
}
