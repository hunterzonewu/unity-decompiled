// Decompiled with JetBrains decompiler
// Type: UnityEditor.GradientEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class GradientEditor
  {
    private bool m_TextureDirty = true;
    private const int k_PreviewWidth = 256;
    private const int k_MaxNumKeys = 8;
    private static GradientEditor.Styles s_Styles;
    private static Texture2D s_BackgroundTexture;
    private List<GradientEditor.Swatch> m_RGBSwatches;
    private List<GradientEditor.Swatch> m_AlphaSwatches;
    [NonSerialized]
    private GradientEditor.Swatch m_SelectedSwatch;
    private Texture2D m_PreviewTex;
    private Gradient m_Gradient;
    private int m_NumSteps;

    public Gradient target
    {
      get
      {
        return this.m_Gradient;
      }
    }

    public void Clear()
    {
      if (!((UnityEngine.Object) this.m_PreviewTex != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_PreviewTex);
    }

    public void Init(Gradient gradient, int numSteps)
    {
      this.m_Gradient = gradient;
      this.m_TextureDirty = true;
      this.m_NumSteps = numSteps;
      this.BuildArrays();
      if (this.m_RGBSwatches.Count <= 0)
        return;
      this.m_SelectedSwatch = this.m_RGBSwatches[0];
    }

    private float GetTime(float actualTime)
    {
      actualTime = Mathf.Clamp01(actualTime);
      if (this.m_NumSteps <= 1)
        return actualTime;
      float num = 1f / (float) (this.m_NumSteps - 1);
      return (float) Mathf.RoundToInt(actualTime / num) / (float) (this.m_NumSteps - 1);
    }

    public static Texture2D CreateGradientTexture(Gradient gradient)
    {
      Texture2D preview = new Texture2D(256, 2, TextureFormat.ARGB32, false);
      preview.wrapMode = TextureWrapMode.Clamp;
      preview.hideFlags = HideFlags.HideAndDontSave;
      GradientEditor.RefreshPreview(gradient, preview);
      return preview;
    }

    public static void RefreshPreview(Gradient gradient, Texture2D preview)
    {
      Color[] colors = new Color[512];
      for (int index = 0; index < 256; ++index)
        colors[index] = colors[index + 256] = gradient.Evaluate((float) index / 256f);
      preview.SetPixels(colors);
      preview.Apply();
    }

    private void BuildTexture()
    {
      if ((UnityEngine.Object) this.m_PreviewTex == (UnityEngine.Object) null)
        this.m_PreviewTex = GradientEditor.CreateGradientTexture(this.m_Gradient);
      else
        GradientEditor.RefreshPreview(this.m_Gradient, this.m_PreviewTex);
      this.m_TextureDirty = false;
    }

    private void BuildArrays()
    {
      if (this.m_Gradient == null)
        return;
      GradientColorKey[] colorKeys = this.m_Gradient.colorKeys;
      this.m_RGBSwatches = new List<GradientEditor.Swatch>(colorKeys.Length);
      for (int index = 0; index < colorKeys.Length; ++index)
      {
        Color color = colorKeys[index].color;
        color.a = 1f;
        this.m_RGBSwatches.Add(new GradientEditor.Swatch(colorKeys[index].time, color, false));
      }
      GradientAlphaKey[] alphaKeys = this.m_Gradient.alphaKeys;
      this.m_AlphaSwatches = new List<GradientEditor.Swatch>(alphaKeys.Length);
      for (int index = 0; index < alphaKeys.Length; ++index)
      {
        float alpha = alphaKeys[index].alpha;
        this.m_AlphaSwatches.Add(new GradientEditor.Swatch(alphaKeys[index].time, new Color(alpha, alpha, alpha, 1f), true));
      }
    }

    public static void DrawGradientWithBackground(Rect position, Texture2D gradientTexture)
    {
      Rect position1 = new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f);
      Texture2D backgroundTexture = GradientEditor.GetBackgroundTexture();
      Rect texCoords = new Rect(0.0f, 0.0f, position1.width / (float) backgroundTexture.width, position1.height / (float) backgroundTexture.height);
      GUI.DrawTextureWithTexCoords(position1, (Texture) backgroundTexture, texCoords, false);
      if ((UnityEngine.Object) gradientTexture != (UnityEngine.Object) null)
        GUI.DrawTexture(position1, (Texture) gradientTexture, ScaleMode.StretchToFill, true);
      GUI.Label(position, GUIContent.none, EditorStyles.colorPickerBox);
    }

    public void OnGUI(Rect position)
    {
      if (GradientEditor.s_Styles == null)
        GradientEditor.s_Styles = new GradientEditor.Styles();
      float num1 = 16f;
      float num2 = 30f;
      float num3 = position.height - 2f * num1 - num2;
      position.height = num1;
      this.ShowSwatchArray(position, this.m_AlphaSwatches, true);
      position.y += num1;
      if (Event.current.type == EventType.Repaint)
      {
        position.height = num3;
        if (this.m_TextureDirty)
          this.BuildTexture();
        GradientEditor.DrawGradientWithBackground(position, this.m_PreviewTex);
      }
      position.y += num3;
      position.height = num1;
      this.ShowSwatchArray(position, this.m_RGBSwatches, false);
      if (this.m_SelectedSwatch == null)
        return;
      position.y += num1;
      position.height = num2;
      position.y += 10f;
      float num4 = 45f;
      float num5 = 60f;
      float num6 = 20f;
      float num7 = 50f;
      float num8 = num5 + num6 + num5 + num4;
      Rect position1 = position;
      position1.height = 18f;
      position1.x += 17f;
      position1.width -= num8;
      EditorGUIUtility.labelWidth = num7;
      if (this.m_SelectedSwatch.m_IsAlpha)
      {
        EditorGUIUtility.fieldWidth = 30f;
        EditorGUI.BeginChangeCheck();
        float num9 = (float) EditorGUI.IntSlider(position1, GradientEditor.s_Styles.alphaText, (int) ((double) this.m_SelectedSwatch.m_Value.r * (double) byte.MaxValue), 0, (int) byte.MaxValue) / (float) byte.MaxValue;
        if (EditorGUI.EndChangeCheck())
        {
          this.m_SelectedSwatch.m_Value.r = this.m_SelectedSwatch.m_Value.g = this.m_SelectedSwatch.m_Value.b = Mathf.Clamp01(num9);
          this.AssignBack();
          HandleUtility.Repaint();
        }
      }
      else
      {
        EditorGUI.BeginChangeCheck();
        this.m_SelectedSwatch.m_Value = EditorGUI.ColorField(position1, GradientEditor.s_Styles.colorText, this.m_SelectedSwatch.m_Value, true, false);
        if (EditorGUI.EndChangeCheck())
        {
          this.AssignBack();
          HandleUtility.Repaint();
        }
      }
      position1.x += position1.width + num6;
      position1.width = num4 + num5;
      EditorGUIUtility.labelWidth = num5;
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      EditorGUI.kFloatFieldFormatString = "f1";
      EditorGUI.BeginChangeCheck();
      float num10 = EditorGUI.FloatField(position1, GradientEditor.s_Styles.locationText, this.m_SelectedSwatch.m_Time * 100f) / 100f;
      if (EditorGUI.EndChangeCheck())
      {
        this.m_SelectedSwatch.m_Time = Mathf.Clamp(num10, 0.0f, 1f);
        this.AssignBack();
      }
      EditorGUI.kFloatFieldFormatString = fieldFormatString;
      position1.x += position1.width;
      position1.width = 20f;
      GUI.Label(position1, GradientEditor.s_Styles.percentText);
    }

    private void ShowSwatchArray(Rect position, List<GradientEditor.Swatch> swatches, bool isAlpha)
    {
      int controlId = GUIUtility.GetControlID(652347689, FocusType.Passive);
      Event current1 = Event.current;
      float time = this.GetTime((Event.current.mousePosition.x - position.x) / position.width);
      Vector2 point = (Vector2) new Vector3(position.x + time * position.width, Event.current.mousePosition.y);
      EventType typeForControl = current1.GetTypeForControl(controlId);
      switch (typeForControl)
      {
        case EventType.MouseDown:
          Rect rect = position;
          rect.xMin -= 10f;
          rect.xMax += 10f;
          if (!rect.Contains(current1.mousePosition))
            break;
          GUIUtility.hotControl = controlId;
          current1.Use();
          if (swatches.Contains(this.m_SelectedSwatch) && !this.m_SelectedSwatch.m_IsAlpha && this.CalcSwatchRect(position, this.m_SelectedSwatch).Contains(current1.mousePosition))
          {
            if (current1.clickCount != 2)
              break;
            GUIUtility.keyboardControl = controlId;
            ColorPicker.Show(GUIView.current, this.m_SelectedSwatch.m_Value, false, false, (ColorPickerHDRConfig) null);
            GUIUtility.ExitGUI();
            break;
          }
          bool flag1 = false;
          using (List<GradientEditor.Swatch>.Enumerator enumerator = swatches.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GradientEditor.Swatch current2 = enumerator.Current;
              if (this.CalcSwatchRect(position, current2).Contains(point))
              {
                flag1 = true;
                this.m_SelectedSwatch = current2;
                break;
              }
            }
          }
          if (flag1)
            break;
          if (swatches.Count < 8)
          {
            Color color = this.m_Gradient.Evaluate(time);
            if (isAlpha)
              color = new Color(color.a, color.a, color.a, 1f);
            else
              color.a = 1f;
            this.m_SelectedSwatch = new GradientEditor.Swatch(time, color, isAlpha);
            swatches.Add(this.m_SelectedSwatch);
            this.AssignBack();
            break;
          }
          Debug.LogWarning((object) ("Max " + (object) 8 + " color keys and " + (object) 8 + " alpha keys are allowed in a gradient."));
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          current1.Use();
          if (!swatches.Contains(this.m_SelectedSwatch))
            this.m_SelectedSwatch = (GradientEditor.Swatch) null;
          this.RemoveDuplicateOverlappingSwatches();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId || this.m_SelectedSwatch == null)
            break;
          current1.Use();
          if ((double) current1.mousePosition.y + 5.0 < (double) position.y || (double) current1.mousePosition.y - 5.0 > (double) position.yMax)
          {
            if (swatches.Count > 1)
            {
              swatches.Remove(this.m_SelectedSwatch);
              this.AssignBack();
              break;
            }
          }
          else if (!swatches.Contains(this.m_SelectedSwatch))
            swatches.Add(this.m_SelectedSwatch);
          this.m_SelectedSwatch.m_Time = time;
          this.AssignBack();
          break;
        case EventType.KeyDown:
          if (current1.keyCode != KeyCode.Delete)
            break;
          if (this.m_SelectedSwatch != null)
          {
            List<GradientEditor.Swatch> swatchList = !this.m_SelectedSwatch.m_IsAlpha ? this.m_RGBSwatches : this.m_AlphaSwatches;
            if (swatchList.Count > 1)
            {
              swatchList.Remove(this.m_SelectedSwatch);
              this.AssignBack();
              HandleUtility.Repaint();
            }
          }
          current1.Use();
          break;
        case EventType.Repaint:
          bool flag2 = false;
          using (List<GradientEditor.Swatch>.Enumerator enumerator = swatches.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              GradientEditor.Swatch current2 = enumerator.Current;
              if (this.m_SelectedSwatch == current2)
                flag2 = true;
              else
                this.DrawSwatch(position, current2, !isAlpha);
            }
          }
          if (!flag2 || this.m_SelectedSwatch == null)
            break;
          this.DrawSwatch(position, this.m_SelectedSwatch, !isAlpha);
          break;
        default:
          if (typeForControl != EventType.ValidateCommand)
          {
            if (typeForControl != EventType.ExecuteCommand)
              break;
            if (current1.commandName == "ColorPickerChanged")
            {
              GUI.changed = true;
              this.m_SelectedSwatch.m_Value = ColorPicker.color;
              this.AssignBack();
              HandleUtility.Repaint();
              break;
            }
            if (!(current1.commandName == "Delete") || swatches.Count <= 1)
              break;
            swatches.Remove(this.m_SelectedSwatch);
            this.AssignBack();
            HandleUtility.Repaint();
            break;
          }
          if (!(current1.commandName == "Delete"))
            break;
          Event.current.Use();
          break;
      }
    }

    private void DrawSwatch(Rect totalPos, GradientEditor.Swatch s, bool upwards)
    {
      Color backgroundColor = GUI.backgroundColor;
      Rect position = this.CalcSwatchRect(totalPos, s);
      GUI.backgroundColor = s.m_Value;
      GUIStyle guiStyle1 = !upwards ? GradientEditor.s_Styles.downSwatch : GradientEditor.s_Styles.upSwatch;
      GUIStyle guiStyle2 = !upwards ? GradientEditor.s_Styles.downSwatchOverlay : GradientEditor.s_Styles.upSwatchOverlay;
      guiStyle1.Draw(position, false, false, this.m_SelectedSwatch == s, false);
      GUI.backgroundColor = backgroundColor;
      guiStyle2.Draw(position, false, false, this.m_SelectedSwatch == s, false);
    }

    private Rect CalcSwatchRect(Rect totalRect, GradientEditor.Swatch s)
    {
      float time = s.m_Time;
      return new Rect((float) ((double) totalRect.x + (double) Mathf.Round(totalRect.width * time) - 5.0), totalRect.y, 10f, totalRect.height);
    }

    private int SwatchSort(GradientEditor.Swatch lhs, GradientEditor.Swatch rhs)
    {
      if ((double) lhs.m_Time == (double) rhs.m_Time && lhs == this.m_SelectedSwatch)
        return -1;
      if ((double) lhs.m_Time == (double) rhs.m_Time && rhs == this.m_SelectedSwatch)
        return 1;
      return lhs.m_Time.CompareTo(rhs.m_Time);
    }

    private void AssignBack()
    {
      this.m_RGBSwatches.Sort((Comparison<GradientEditor.Swatch>) ((a, b) => this.SwatchSort(a, b)));
      GradientColorKey[] gradientColorKeyArray = new GradientColorKey[this.m_RGBSwatches.Count];
      for (int index = 0; index < this.m_RGBSwatches.Count; ++index)
      {
        gradientColorKeyArray[index].color = this.m_RGBSwatches[index].m_Value;
        gradientColorKeyArray[index].time = this.m_RGBSwatches[index].m_Time;
      }
      this.m_AlphaSwatches.Sort((Comparison<GradientEditor.Swatch>) ((a, b) => this.SwatchSort(a, b)));
      GradientAlphaKey[] gradientAlphaKeyArray = new GradientAlphaKey[this.m_AlphaSwatches.Count];
      for (int index = 0; index < this.m_AlphaSwatches.Count; ++index)
      {
        gradientAlphaKeyArray[index].alpha = this.m_AlphaSwatches[index].m_Value.r;
        gradientAlphaKeyArray[index].time = this.m_AlphaSwatches[index].m_Time;
      }
      this.m_Gradient.colorKeys = gradientColorKeyArray;
      this.m_Gradient.alphaKeys = gradientAlphaKeyArray;
      this.m_TextureDirty = true;
      GUI.changed = true;
    }

    private void RemoveDuplicateOverlappingSwatches()
    {
      bool flag = false;
      for (int index = 1; index < this.m_RGBSwatches.Count; ++index)
      {
        if (Mathf.Approximately(this.m_RGBSwatches[index - 1].m_Time, this.m_RGBSwatches[index].m_Time))
        {
          this.m_RGBSwatches.RemoveAt(index);
          --index;
          flag = true;
        }
      }
      for (int index = 1; index < this.m_AlphaSwatches.Count; ++index)
      {
        if (Mathf.Approximately(this.m_AlphaSwatches[index - 1].m_Time, this.m_AlphaSwatches[index].m_Time))
        {
          this.m_AlphaSwatches.RemoveAt(index);
          --index;
          flag = true;
        }
      }
      if (!flag)
        return;
      this.AssignBack();
    }

    public static Texture2D GetBackgroundTexture()
    {
      if ((UnityEngine.Object) GradientEditor.s_BackgroundTexture == (UnityEngine.Object) null)
        GradientEditor.s_BackgroundTexture = GradientEditor.CreateCheckerTexture(32, 4, 4, Color.white, new Color(0.7f, 0.7f, 0.7f));
      return GradientEditor.s_BackgroundTexture;
    }

    public static Texture2D CreateCheckerTexture(int numCols, int numRows, int cellPixelWidth, Color col1, Color col2)
    {
      int height = numRows * cellPixelWidth;
      int width = numCols * cellPixelWidth;
      Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      Color[] colors = new Color[width * height];
      for (int index1 = 0; index1 < numRows; ++index1)
      {
        for (int index2 = 0; index2 < numCols; ++index2)
        {
          for (int index3 = 0; index3 < cellPixelWidth; ++index3)
          {
            for (int index4 = 0; index4 < cellPixelWidth; ++index4)
              colors[(index1 * cellPixelWidth + index3) * width + index2 * cellPixelWidth + index4] = (index1 + index2) % 2 != 0 ? col2 : col1;
          }
        }
      }
      texture2D.SetPixels(colors);
      texture2D.Apply();
      return texture2D;
    }

    public static void DrawGradientSwatch(Rect position, Gradient gradient, Color bgColor)
    {
      GradientEditor.DrawGradientSwatchInternal(position, gradient, (SerializedProperty) null, bgColor);
    }

    public static void DrawGradientSwatch(Rect position, SerializedProperty property, Color bgColor)
    {
      GradientEditor.DrawGradientSwatchInternal(position, (Gradient) null, property, bgColor);
    }

    private static void DrawGradientSwatchInternal(Rect position, Gradient gradient, SerializedProperty property, Color bgColor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      Texture2D backgroundTexture = GradientEditor.GetBackgroundTexture();
      if ((UnityEngine.Object) backgroundTexture != (UnityEngine.Object) null)
      {
        Color color = GUI.color;
        GUI.color = bgColor;
        EditorGUIUtility.GetBasicTextureStyle(backgroundTexture).Draw(position, false, false, false, false);
        GUI.color = color;
      }
      Texture2D tex = property == null ? GradientPreviewCache.GetGradientPreview(gradient) : GradientPreviewCache.GetPropertyPreview(property);
      if ((UnityEngine.Object) tex == (UnityEngine.Object) null)
        Debug.Log((object) "Warning: Could not create preview for gradient");
      else
        EditorGUIUtility.GetBasicTextureStyle(tex).Draw(position, false, false, false, false);
    }

    private class Styles
    {
      public GUIStyle upSwatch = (GUIStyle) "Grad Up Swatch";
      public GUIStyle upSwatchOverlay = (GUIStyle) "Grad Up Swatch Overlay";
      public GUIStyle downSwatch = (GUIStyle) "Grad Down Swatch";
      public GUIStyle downSwatchOverlay = (GUIStyle) "Grad Down Swatch Overlay";
      public GUIContent alphaText = new GUIContent("Alpha");
      public GUIContent colorText = new GUIContent("Color");
      public GUIContent locationText = new GUIContent("Location");
      public GUIContent percentText = new GUIContent("%");

      private static GUIStyle GetStyle(string name)
      {
        return ((GUISkin) EditorGUIUtility.LoadRequired("GradientEditor.GUISkin")).GetStyle(name);
      }
    }

    public class Swatch
    {
      public float m_Time;
      public Color m_Value;
      public bool m_IsAlpha;

      public Swatch(float time, Color value, bool isAlpha)
      {
        this.m_Time = time;
        this.m_Value = value;
        this.m_IsAlpha = isAlpha;
      }
    }
  }
}
