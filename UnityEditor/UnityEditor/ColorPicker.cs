// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorPicker
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ColorPicker : EditorWindow
  {
    private static readonly ColorPickerHDRConfig m_DefaultHDRConfig = new ColorPickerHDRConfig(0.0f, 99f, 1f / 99f, 3f);
    private static int s_Slider2Dhash = "Slider2D".GetHashCode();
    [SerializeField]
    private ColorPicker.HDRValues m_HDRValues = new ColorPicker.HDRValues();
    [SerializeField]
    private Color m_Color = Color.black;
    [SerializeField]
    private float m_A = 1f;
    [SerializeField]
    private float m_ColorSliderSize = 4f;
    [SerializeField]
    private bool m_ShowPresets = true;
    [SerializeField]
    private bool m_ShowAlpha = true;
    private float m_RTextureG = -1f;
    private float m_RTextureB = -1f;
    private float m_GTextureR = -1f;
    private float m_GTextureB = -1f;
    private float m_BTextureR = -1f;
    private float m_BTextureG = -1f;
    private float m_HueTextureS = -1f;
    private float m_HueTextureV = -1f;
    private float m_SatTextureH = -1f;
    private float m_SatTextureV = -1f;
    private float m_ValTextureH = -1f;
    private float m_ValTextureS = -1f;
    [SerializeField]
    private int m_TextureColorSliderMode = -1;
    [SerializeField]
    private Vector2 m_LastConstantValues = new Vector2(-1f, -1f);
    [NonSerialized]
    private int m_TextureColorBoxMode = -1;
    [SerializeField]
    private float m_LastConstant = -1f;
    private string[] m_ColorBoxXAxisLabels = new string[7]{ "Saturation", "Hue", "Hue", "Blue", "Blue", "Red", string.Empty };
    private string[] m_ColorBoxYAxisLabels = new string[7]{ "Brightness", "Brightness", "Saturation", "Green", "Red", "Green", string.Empty };
    private string[] m_ColorBoxZAxisLabels = new string[7]{ "Hue", "Saturation", "Brightness", "Red", "Green", "Blue", string.Empty };
    [SerializeField]
    private ColorPicker.ColorBoxMode m_ColorBoxMode = ColorPicker.ColorBoxMode.BG_R;
    [SerializeField]
    private ColorPicker.SliderMode m_SliderMode = ColorPicker.SliderMode.HSV;
    private float m_OldAlpha = -1f;
    [SerializeField]
    private int m_ModalUndoGroup = -1;
    private const int kHueRes = 64;
    private const int kColorBoxSize = 32;
    private const int kEyeDropperHeight = 95;
    private const int kSlidersHeight = 82;
    private const int kColorBoxHeight = 162;
    private const int kPresetsHeight = 300;
    private const float kFixedWindowWidth = 233f;
    private const float kHDRFieldWidth = 40f;
    private const float kLDRFieldWidth = 30f;
    private static ColorPicker s_SharedColorPicker;
    [SerializeField]
    private bool m_HDR;
    [SerializeField]
    private ColorPickerHDRConfig m_HDRConfig;
    [SerializeField]
    private Color m_OriginalColor;
    [SerializeField]
    private float m_R;
    [SerializeField]
    private float m_G;
    [SerializeField]
    private float m_B;
    [SerializeField]
    private float m_H;
    [SerializeField]
    private float m_S;
    [SerializeField]
    private float m_V;
    [SerializeField]
    private Texture2D m_ColorSlider;
    [SerializeField]
    private float m_SliderValue;
    [SerializeField]
    private Color[] m_Colors;
    [SerializeField]
    private Texture2D m_ColorBox;
    [SerializeField]
    private bool m_UseTonemappingPreview;
    [SerializeField]
    private bool m_IsOSColorPicker;
    [SerializeField]
    private bool m_resetKeyboardControl;
    private Texture2D m_RTexture;
    private Texture2D m_GTexture;
    private Texture2D m_BTexture;
    [SerializeField]
    private Texture2D m_HueTexture;
    [SerializeField]
    private Texture2D m_SatTexture;
    [SerializeField]
    private Texture2D m_ValTexture;
    [NonSerialized]
    private bool m_ColorSpaceBoxDirty;
    [NonSerialized]
    private bool m_ColorSliderDirty;
    [NonSerialized]
    private bool m_RGBHSVSlidersDirty;
    [SerializeField]
    private ContainerWindow m_TrackingWindow;
    [SerializeField]
    private ColorPicker.ColorBoxMode m_OldColorBoxMode;
    [SerializeField]
    private Texture2D m_AlphaTexture;
    [SerializeField]
    private GUIView m_DelegateView;
    private PresetLibraryEditor<ColorPresetLibrary> m_ColorLibraryEditor;
    private PresetLibraryEditorState m_ColorLibraryEditorState;
    private static ColorPicker.Styles styles;
    private static Texture2D s_LeftGradientTexture;
    private static Texture2D s_RightGradientTexture;

    public static string presetsEditorPrefID
    {
      get
      {
        return "Color";
      }
    }

    public static ColorPickerHDRConfig defaultHDRConfig
    {
      get
      {
        return ColorPicker.m_DefaultHDRConfig;
      }
    }

    private bool colorChanged { get; set; }

    private float fieldWidth
    {
      get
      {
        return this.m_HDR ? 40f : 30f;
      }
    }

    public static bool visible
    {
      get
      {
        return (UnityEngine.Object) ColorPicker.s_SharedColorPicker != (UnityEngine.Object) null;
      }
    }

    public static Color color
    {
      get
      {
        if ((double) ColorPicker.get.m_HDRValues.m_HDRScaleFactor > 1.0)
          return ColorPicker.get.m_Color.RGBMultiplied(ColorPicker.get.m_HDRValues.m_HDRScaleFactor);
        return ColorPicker.get.m_Color;
      }
      set
      {
        ColorPicker.get.SetColor(value);
      }
    }

    public static ColorPicker get
    {
      get
      {
        if (!(bool) ((UnityEngine.Object) ColorPicker.s_SharedColorPicker))
        {
          UnityEngine.Object[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof (ColorPicker));
          if (objectsOfTypeAll != null && objectsOfTypeAll.Length > 0)
            ColorPicker.s_SharedColorPicker = (ColorPicker) objectsOfTypeAll[0];
          if (!(bool) ((UnityEngine.Object) ColorPicker.s_SharedColorPicker))
          {
            ColorPicker.s_SharedColorPicker = ScriptableObject.CreateInstance<ColorPicker>();
            ColorPicker.s_SharedColorPicker.wantsMouseMove = true;
          }
        }
        return ColorPicker.s_SharedColorPicker;
      }
    }

    public string currentPresetLibrary
    {
      get
      {
        this.InitIfNeeded();
        return this.m_ColorLibraryEditor.currentLibraryWithoutExtension;
      }
      set
      {
        this.InitIfNeeded();
        this.m_ColorLibraryEditor.currentLibraryWithoutExtension = value;
      }
    }

    public ColorPicker()
    {
      this.m_IsOSColorPicker = EditorPrefs.GetBool("UseOSColorPicker");
      this.hideFlags = HideFlags.DontSave;
      EditorApplication.update += new EditorApplication.CallbackFunction(this.PollOSColorPicker);
      EditorGUIUtility.editingTextField = true;
    }

    private void OnSelectionChange()
    {
      this.m_resetKeyboardControl = true;
      this.Repaint();
    }

    private void RGBToHSV()
    {
      Color.RGBToHSV(new Color(this.m_R, this.m_G, this.m_B, 1f), out this.m_H, out this.m_S, out this.m_V);
    }

    private void HSVToRGB()
    {
      Color rgb = Color.HSVToRGB(this.m_H, this.m_S, this.m_V);
      this.m_R = rgb.r;
      this.m_G = rgb.g;
      this.m_B = rgb.b;
    }

    private static void swap(ref float f1, ref float f2)
    {
      float num = f1;
      f1 = f2;
      f2 = num;
    }

    private Vector2 Slider2D(Rect rect, Vector2 value, Vector2 maxvalue, Vector2 minvalue, GUIStyle backStyle, GUIStyle thumbStyle)
    {
      if (backStyle == null || thumbStyle == null)
        return value;
      int controlId = GUIUtility.GetControlID(ColorPicker.s_Slider2Dhash, FocusType.Native);
      if ((double) maxvalue.x < (double) minvalue.x)
        ColorPicker.swap(ref maxvalue.x, ref minvalue.x);
      if ((double) maxvalue.y < (double) minvalue.y)
        ColorPicker.swap(ref maxvalue.y, ref minvalue.y);
      float height = (double) thumbStyle.fixedHeight != 0.0 ? thumbStyle.fixedHeight : (float) thumbStyle.padding.vertical;
      float width = (double) thumbStyle.fixedWidth != 0.0 ? thumbStyle.fixedWidth : (float) thumbStyle.padding.horizontal;
      Vector2 vector2 = new Vector2((float) (((double) rect.width - (double) (backStyle.padding.right + backStyle.padding.left) - (double) width * 2.0) / ((double) maxvalue.x - (double) minvalue.x)), (float) (((double) rect.height - (double) (backStyle.padding.top + backStyle.padding.bottom) - (double) height * 2.0) / ((double) maxvalue.y - (double) minvalue.y)));
      Rect position = new Rect((float) ((double) rect.x + (double) value.x * (double) vector2.x + (double) width / 2.0 + (double) backStyle.padding.left - (double) minvalue.x * (double) vector2.x), (float) ((double) rect.y + (double) value.y * (double) vector2.y + (double) height / 2.0 + (double) backStyle.padding.top - (double) minvalue.y * (double) vector2.y), width, height);
      Event current = Event.current;
      switch (current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (rect.Contains(current.mousePosition))
          {
            GUIUtility.hotControl = controlId;
            GUIUtility.keyboardControl = 0;
            value.x = (current.mousePosition.x - rect.x - width - (float) backStyle.padding.left) / vector2.x + minvalue.x;
            value.y = (current.mousePosition.y - rect.y - height - (float) backStyle.padding.top) / vector2.y + minvalue.y;
            GUI.changed = true;
            Event.current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == controlId)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == controlId)
          {
            value.x = (current.mousePosition.x - rect.x - width - (float) backStyle.padding.left) / vector2.x + minvalue.x;
            value.y = (current.mousePosition.y - rect.y - height - (float) backStyle.padding.top) / vector2.y + minvalue.y;
            value.x = Mathf.Clamp(value.x, minvalue.x, maxvalue.x);
            value.y = Mathf.Clamp(value.y, minvalue.y, maxvalue.y);
            GUI.changed = true;
            Event.current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          backStyle.Draw(rect, GUIContent.none, controlId);
          Color color = GUI.color;
          bool flag = (double) ColorPicker.color.grayscale > 0.5;
          if (flag)
            GUI.color = Color.black;
          thumbStyle.Draw(position, GUIContent.none, controlId);
          if (flag)
          {
            GUI.color = color;
            break;
          }
          break;
      }
      return value;
    }

    private void OnFloatFieldChanged(float value)
    {
      if (!this.m_HDR || (double) value <= (double) this.m_HDRValues.m_HDRScaleFactor)
        return;
      this.SetHDRScaleFactor(value);
    }

    private void RGBSliders()
    {
      EditorGUI.BeginChangeCheck();
      float exposureAdjusment = this.GetTonemappingExposureAdjusment();
      float colorScale = this.GetColorScale();
      this.m_RTexture = ColorPicker.Update1DSlider(this.m_RTexture, 32, this.m_G, this.m_B, ref this.m_RTextureG, ref this.m_RTextureB, 0, false, colorScale, exposureAdjusment, this.m_RGBHSVSlidersDirty, this.m_HDRValues.m_TonemappingType);
      this.m_GTexture = ColorPicker.Update1DSlider(this.m_GTexture, 32, this.m_R, this.m_B, ref this.m_GTextureR, ref this.m_GTextureB, 1, false, colorScale, exposureAdjusment, this.m_RGBHSVSlidersDirty, this.m_HDRValues.m_TonemappingType);
      this.m_BTexture = ColorPicker.Update1DSlider(this.m_BTexture, 32, this.m_R, this.m_G, ref this.m_BTextureR, ref this.m_BTextureG, 2, false, colorScale, exposureAdjusment, this.m_RGBHSVSlidersDirty, this.m_HDRValues.m_TonemappingType);
      this.m_RGBHSVSlidersDirty = false;
      float displayScale = !this.m_HDR ? (float) byte.MaxValue : colorScale;
      string formatString = !this.m_HDR ? EditorGUI.kIntFieldFormatString : EditorGUI.kFloatFieldFormatString;
      this.m_R = this.TexturedSlider(this.m_RTexture, "R", this.m_R, 0.0f, 1f, displayScale, formatString, new System.Action<float>(this.OnFloatFieldChanged));
      this.m_G = this.TexturedSlider(this.m_GTexture, "G", this.m_G, 0.0f, 1f, displayScale, formatString, new System.Action<float>(this.OnFloatFieldChanged));
      this.m_B = this.TexturedSlider(this.m_BTexture, "B", this.m_B, 0.0f, 1f, displayScale, formatString, new System.Action<float>(this.OnFloatFieldChanged));
      if (!EditorGUI.EndChangeCheck())
        return;
      this.RGBToHSV();
    }

    private static Texture2D Update1DSlider(Texture2D tex, int xSize, float const1, float const2, ref float oldConst1, ref float oldConst2, int idx, bool hsvSpace, float scale, float exposureValue, bool forceUpdate, ColorPicker.TonemappingType tonemappingType)
    {
      if (!(bool) ((UnityEngine.Object) tex) || (double) const1 != (double) oldConst1 || ((double) const2 != (double) oldConst2 || forceUpdate))
      {
        if (!(bool) ((UnityEngine.Object) tex))
          tex = ColorPicker.MakeTexture(xSize, 2);
        Color[] colorArray = new Color[xSize * 2];
        Color topLeftColor = Color.black;
        Color rightGradient = Color.black;
        switch (idx)
        {
          case 0:
            topLeftColor = new Color(0.0f, const1, const2, 1f);
            rightGradient = new Color(1f, 0.0f, 0.0f, 0.0f);
            break;
          case 1:
            topLeftColor = new Color(const1, 0.0f, const2, 1f);
            rightGradient = new Color(0.0f, 1f, 0.0f, 0.0f);
            break;
          case 2:
            topLeftColor = new Color(const1, const2, 0.0f, 1f);
            rightGradient = new Color(0.0f, 0.0f, 1f, 0.0f);
            break;
          case 3:
            topLeftColor = new Color(0.0f, 0.0f, 0.0f, 1f);
            rightGradient = new Color(1f, 1f, 1f, 0.0f);
            break;
        }
        ColorPicker.FillArea(xSize, 2, colorArray, topLeftColor, rightGradient, new Color(0.0f, 0.0f, 0.0f, 0.0f));
        if (hsvSpace)
          ColorPicker.HSVToRGBArray(colorArray, scale);
        else
          ColorPicker.ScaleColors(colorArray, scale);
        ColorPicker.DoTonemapping(colorArray, exposureValue, tonemappingType);
        oldConst1 = const1;
        oldConst2 = const2;
        tex.SetPixels(colorArray);
        tex.Apply();
      }
      return tex;
    }

    private float TexturedSlider(Texture2D background, string text, float val, float min, float max, float displayScale, string formatString, System.Action<float> onFloatFieldChanged)
    {
      Rect rect = GUILayoutUtility.GetRect(16f, 16f, GUI.skin.label);
      GUI.Label(new Rect(rect.x, rect.y, 20f, 16f), text);
      rect.x += 14f;
      rect.width -= 20f + this.fieldWidth;
      if (Event.current.type == EventType.Repaint)
        Graphics.DrawTexture(new Rect(rect.x + 1f, rect.y + 2f, rect.width - 2f, rect.height - 4f), (Texture) background, new Rect(0.5f / (float) background.width, 0.5f / (float) background.height, (float) (1.0 - 1.0 / (double) background.width), (float) (1.0 - 1.0 / (double) background.height)), 0, 0, 0, 0, Color.grey);
      int controlId = GUIUtility.GetControlID(869045, EditorGUIUtility.native, this.position);
      EditorGUI.BeginChangeCheck();
      val = GUI.HorizontalSlider(new Rect(rect.x, rect.y + 1f, rect.width, rect.height - 2f), val, min, max, ColorPicker.styles.pickerBox, ColorPicker.styles.thumbHoriz);
      if (EditorGUI.EndChangeCheck())
      {
        if (EditorGUI.s_RecycledEditor.IsEditingControl(controlId))
          EditorGUI.s_RecycledEditor.EndEditing();
        val = (float) Math.Round((double) val, 3);
        GUIUtility.keyboardControl = 0;
      }
      Rect position = new Rect(rect.xMax + 6f, rect.y, this.fieldWidth, 16f);
      EditorGUI.BeginChangeCheck();
      val = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position, new Rect(0.0f, 0.0f, 0.0f, 0.0f), controlId, val * displayScale, formatString, EditorStyles.numberField, false);
      if (EditorGUI.EndChangeCheck() && onFloatFieldChanged != null)
        onFloatFieldChanged(val);
      val = Mathf.Clamp(val / displayScale, min, max);
      GUILayout.Space(3f);
      return val;
    }

    private void HSVSliders()
    {
      EditorGUI.BeginChangeCheck();
      float exposureAdjusment = this.GetTonemappingExposureAdjusment();
      float colorScale = this.GetColorScale();
      this.m_HueTexture = ColorPicker.Update1DSlider(this.m_HueTexture, 64, 1f, 1f, ref this.m_HueTextureS, ref this.m_HueTextureV, 0, true, 1f, -1f, this.m_RGBHSVSlidersDirty, this.m_HDRValues.m_TonemappingType);
      this.m_SatTexture = ColorPicker.Update1DSlider(this.m_SatTexture, 32, this.m_H, Mathf.Max(this.m_V, 0.2f), ref this.m_SatTextureH, ref this.m_SatTextureV, 1, true, colorScale, exposureAdjusment, this.m_RGBHSVSlidersDirty, this.m_HDRValues.m_TonemappingType);
      this.m_ValTexture = ColorPicker.Update1DSlider(this.m_ValTexture, 32, this.m_H, this.m_S, ref this.m_ValTextureH, ref this.m_ValTextureS, 2, true, colorScale, exposureAdjusment, this.m_RGBHSVSlidersDirty, this.m_HDRValues.m_TonemappingType);
      this.m_RGBHSVSlidersDirty = false;
      float displayScale = !this.m_HDR ? (float) byte.MaxValue : colorScale;
      string formatString = !this.m_HDR ? EditorGUI.kIntFieldFormatString : EditorGUI.kFloatFieldFormatString;
      this.m_H = this.TexturedSlider(this.m_HueTexture, "H", this.m_H, 0.0f, 1f, 359f, EditorGUI.kIntFieldFormatString, (System.Action<float>) null);
      this.m_S = this.TexturedSlider(this.m_SatTexture, "S", this.m_S, 0.0f, 1f, !this.m_HDR ? (float) byte.MaxValue : 1f, formatString, (System.Action<float>) null);
      this.m_V = this.TexturedSlider(this.m_ValTexture, "V", this.m_V, 0.0f, 1f, displayScale, formatString, (System.Action<float>) null);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.HSVToRGB();
    }

    private static void FillArea(int xSize, int ySize, Color[] retval, Color topLeftColor, Color rightGradient, Color downGradient)
    {
      Color color1 = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      Color color2 = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      if (xSize > 1)
        color1 = rightGradient / (float) (xSize - 1);
      if (ySize > 1)
        color2 = downGradient / (float) (ySize - 1);
      Color color3 = topLeftColor;
      int num = 0;
      for (int index1 = 0; index1 < ySize; ++index1)
      {
        Color color4 = color3;
        for (int index2 = 0; index2 < xSize; ++index2)
        {
          retval[num++] = color4;
          color4 += color1;
        }
        color3 += color2;
      }
    }

    private static void ScaleColors(Color[] colors, float scale)
    {
      int length = colors.Length;
      for (int index = 0; index < length; ++index)
        colors[index] = colors[index].RGBMultiplied(scale);
    }

    private static void HSVToRGBArray(Color[] colors, float scale)
    {
      int length = colors.Length;
      for (int index = 0; index < length; ++index)
      {
        Color color1 = colors[index];
        Color color2 = Color.HSVToRGB(color1.r, color1.g, color1.b);
        color2 = color2.RGBMultiplied(scale);
        color2.a = color1.a;
        colors[index] = color2;
      }
    }

    private static void LinearToGammaArray(Color[] colors)
    {
      int length = colors.Length;
      for (int index = 0; index < length; ++index)
      {
        Color color = colors[index];
        Color gamma = color.gamma;
        gamma.a = color.a;
        colors[index] = gamma;
      }
    }

    private float GetTonemappingExposureAdjusment()
    {
      if (this.m_HDR && this.m_UseTonemappingPreview)
        return this.m_HDRValues.m_ExposureAdjustment;
      return -1f;
    }

    private float GetColorScale()
    {
      if (this.m_HDR)
        return Mathf.Max(1f, this.m_HDRValues.m_HDRScaleFactor);
      return 1f;
    }

    private void DrawColorSlider(Rect colorSliderRect, Vector2 constantValues)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (this.m_ColorBoxMode != (ColorPicker.ColorBoxMode) this.m_TextureColorSliderMode)
      {
        int colorSliderSize = (int) this.m_ColorSliderSize;
        int height = this.m_ColorBoxMode != ColorPicker.ColorBoxMode.SV_H ? (int) this.m_ColorSliderSize : 64;
        if ((UnityEngine.Object) this.m_ColorSlider == (UnityEngine.Object) null)
          this.m_ColorSlider = ColorPicker.MakeTexture(colorSliderSize, height);
        if (this.m_ColorSlider.width != colorSliderSize || this.m_ColorSlider.height != height)
          this.m_ColorSlider.Resize(colorSliderSize, height);
      }
      if (this.m_ColorBoxMode != (ColorPicker.ColorBoxMode) this.m_TextureColorSliderMode || constantValues != this.m_LastConstantValues || this.m_ColorSliderDirty)
      {
        float exposureAdjusment = this.GetTonemappingExposureAdjusment();
        float colorScale = this.GetColorScale();
        Color[] pixels = this.m_ColorSlider.GetPixels(0);
        int width = this.m_ColorSlider.width;
        int height = this.m_ColorSlider.height;
        switch (this.m_ColorBoxMode)
        {
          case ColorPicker.ColorBoxMode.SV_H:
            ColorPicker.FillArea(width, height, pixels, new Color(0.0f, 1f, 1f, 1f), new Color(0.0f, 0.0f, 0.0f, 0.0f), new Color(1f, 0.0f, 0.0f, 0.0f));
            ColorPicker.HSVToRGBArray(pixels, 1f);
            break;
          case ColorPicker.ColorBoxMode.HV_S:
            ColorPicker.FillArea(width, height, pixels, new Color(this.m_H, 0.0f, Mathf.Max(this.m_V, 0.3f), 1f), new Color(0.0f, 0.0f, 0.0f, 0.0f), new Color(0.0f, 1f, 0.0f, 0.0f));
            ColorPicker.HSVToRGBArray(pixels, colorScale);
            break;
          case ColorPicker.ColorBoxMode.HS_V:
            ColorPicker.FillArea(width, height, pixels, new Color(this.m_H, this.m_S, 0.0f, 1f), new Color(0.0f, 0.0f, 0.0f, 0.0f), new Color(0.0f, 0.0f, 1f, 0.0f));
            ColorPicker.HSVToRGBArray(pixels, colorScale);
            break;
          case ColorPicker.ColorBoxMode.BG_R:
            ColorPicker.FillArea(width, height, pixels, new Color(0.0f, this.m_G * colorScale, this.m_B * colorScale, 1f), new Color(0.0f, 0.0f, 0.0f, 0.0f), new Color(colorScale, 0.0f, 0.0f, 0.0f));
            break;
          case ColorPicker.ColorBoxMode.BR_G:
            ColorPicker.FillArea(width, height, pixels, new Color(this.m_R * colorScale, 0.0f, this.m_B * colorScale, 1f), new Color(0.0f, 0.0f, 0.0f, 0.0f), new Color(0.0f, colorScale, 0.0f, 0.0f));
            break;
          case ColorPicker.ColorBoxMode.RG_B:
            ColorPicker.FillArea(width, height, pixels, new Color(this.m_R * colorScale, this.m_G * colorScale, 0.0f, 1f), new Color(0.0f, 0.0f, 0.0f, 0.0f), new Color(0.0f, 0.0f, colorScale, 0.0f));
            break;
        }
        if (QualitySettings.activeColorSpace == ColorSpace.Linear)
          ColorPicker.LinearToGammaArray(pixels);
        if (this.m_ColorBoxMode != ColorPicker.ColorBoxMode.SV_H)
          ColorPicker.DoTonemapping(pixels, exposureAdjusment, this.m_HDRValues.m_TonemappingType);
        this.m_ColorSlider.SetPixels(pixels, 0);
        this.m_ColorSlider.Apply(true);
      }
      Graphics.DrawTexture(colorSliderRect, (Texture) this.m_ColorSlider, new Rect(0.5f / (float) this.m_ColorSlider.width, 0.5f / (float) this.m_ColorSlider.height, (float) (1.0 - 1.0 / (double) this.m_ColorSlider.width), (float) (1.0 - 1.0 / (double) this.m_ColorSlider.height)), 0, 0, 0, 0, Color.grey);
    }

    public static Texture2D MakeTexture(int width, int height)
    {
      Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.wrapMode = TextureWrapMode.Clamp;
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      return texture2D;
    }

    private void DrawColorSpaceBox(Rect colorBoxRect, float constantValue)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (this.m_ColorBoxMode != (ColorPicker.ColorBoxMode) this.m_TextureColorBoxMode)
      {
        int height = 32;
        int width = this.m_ColorBoxMode == ColorPicker.ColorBoxMode.HV_S || this.m_ColorBoxMode == ColorPicker.ColorBoxMode.HS_V ? 64 : 32;
        if ((UnityEngine.Object) this.m_ColorBox == (UnityEngine.Object) null)
          this.m_ColorBox = ColorPicker.MakeTexture(width, height);
        if (this.m_ColorBox.width != width || this.m_ColorBox.height != height)
          this.m_ColorBox.Resize(width, height);
      }
      if (this.m_ColorBoxMode != (ColorPicker.ColorBoxMode) this.m_TextureColorBoxMode || (double) this.m_LastConstant != (double) constantValue || this.m_ColorSpaceBoxDirty)
      {
        float exposureAdjusment = this.GetTonemappingExposureAdjusment();
        float colorScale = this.GetColorScale();
        this.m_Colors = this.m_ColorBox.GetPixels(0);
        int width = this.m_ColorBox.width;
        int height = this.m_ColorBox.height;
        switch (this.m_ColorBoxMode)
        {
          case ColorPicker.ColorBoxMode.SV_H:
            ColorPicker.FillArea(width, height, this.m_Colors, new Color(this.m_H, 0.0f, 0.0f, 1f), new Color(0.0f, 1f, 0.0f, 0.0f), new Color(0.0f, 0.0f, 1f, 0.0f));
            ColorPicker.HSVToRGBArray(this.m_Colors, colorScale);
            break;
          case ColorPicker.ColorBoxMode.HV_S:
            ColorPicker.FillArea(width, height, this.m_Colors, new Color(0.0f, this.m_S, 0.0f, 1f), new Color(1f, 0.0f, 0.0f, 0.0f), new Color(0.0f, 0.0f, 1f, 0.0f));
            ColorPicker.HSVToRGBArray(this.m_Colors, colorScale);
            break;
          case ColorPicker.ColorBoxMode.HS_V:
            ColorPicker.FillArea(width, height, this.m_Colors, new Color(0.0f, 0.0f, this.m_V * colorScale, 1f), new Color(1f, 0.0f, 0.0f, 0.0f), new Color(0.0f, 1f, 0.0f, 0.0f));
            ColorPicker.HSVToRGBArray(this.m_Colors, 1f);
            break;
          case ColorPicker.ColorBoxMode.BG_R:
            ColorPicker.FillArea(width, height, this.m_Colors, new Color(this.m_R * colorScale, 0.0f, 0.0f, 1f), new Color(0.0f, 0.0f, colorScale, 0.0f), new Color(0.0f, colorScale, 0.0f, 0.0f));
            break;
          case ColorPicker.ColorBoxMode.BR_G:
            ColorPicker.FillArea(width, height, this.m_Colors, new Color(0.0f, this.m_G * colorScale, 0.0f, 1f), new Color(0.0f, 0.0f, colorScale, 0.0f), new Color(colorScale, 0.0f, 0.0f, 0.0f));
            break;
          case ColorPicker.ColorBoxMode.RG_B:
            ColorPicker.FillArea(width, height, this.m_Colors, new Color(0.0f, 0.0f, this.m_B * colorScale, 1f), new Color(colorScale, 0.0f, 0.0f, 0.0f), new Color(0.0f, colorScale, 0.0f, 0.0f));
            break;
        }
        if (QualitySettings.activeColorSpace == ColorSpace.Linear)
          ColorPicker.LinearToGammaArray(this.m_Colors);
        ColorPicker.DoTonemapping(this.m_Colors, exposureAdjusment, this.m_HDRValues.m_TonemappingType);
        this.m_ColorBox.SetPixels(this.m_Colors, 0);
        this.m_ColorBox.Apply(true);
        this.m_LastConstant = constantValue;
        this.m_TextureColorBoxMode = (int) this.m_ColorBoxMode;
      }
      Graphics.DrawTexture(colorBoxRect, (Texture) this.m_ColorBox, new Rect(0.5f / (float) this.m_ColorBox.width, 0.5f / (float) this.m_ColorBox.height, (float) (1.0 - 1.0 / (double) this.m_ColorBox.width), (float) (1.0 - 1.0 / (double) this.m_ColorBox.height)), 0, 0, 0, 0, Color.grey);
      ColorPicker.DrawLabelOutsideRect(colorBoxRect, this.GetXAxisLabel(this.m_ColorBoxMode), ColorPicker.LabelLocation.Bottom);
      ColorPicker.DrawLabelOutsideRect(colorBoxRect, this.GetYAxisLabel(this.m_ColorBoxMode), ColorPicker.LabelLocation.Left);
    }

    private string GetXAxisLabel(ColorPicker.ColorBoxMode colorBoxMode)
    {
      return this.m_ColorBoxXAxisLabels[(int) colorBoxMode];
    }

    private string GetYAxisLabel(ColorPicker.ColorBoxMode colorBoxMode)
    {
      return this.m_ColorBoxYAxisLabels[(int) colorBoxMode];
    }

    private string GetZAxisLabel(ColorPicker.ColorBoxMode colorBoxMode)
    {
      return this.m_ColorBoxZAxisLabels[(int) colorBoxMode];
    }

    private static void DrawLabelOutsideRect(Rect position, string label, ColorPicker.LabelLocation labelLocation)
    {
      Matrix4x4 matrix = GUI.matrix;
      Rect position1 = new Rect(position.x, position.y - 18f, position.width, 16f);
      switch (labelLocation)
      {
        case ColorPicker.LabelLocation.Bottom:
          position1 = new Rect(position.x, position.yMax, position.width, 16f);
          break;
        case ColorPicker.LabelLocation.Left:
          GUIUtility.RotateAroundPivot(-90f, position.center);
          break;
        case ColorPicker.LabelLocation.Right:
          GUIUtility.RotateAroundPivot(90f, position.center);
          break;
      }
      EditorGUI.BeginDisabledGroup(true);
      GUI.Label(position1, label, ColorPicker.styles.label);
      EditorGUI.EndDisabledGroup();
      GUI.matrix = matrix;
    }

    private void InitIfNeeded()
    {
      if (ColorPicker.styles == null)
        ColorPicker.styles = new ColorPicker.Styles();
      if (this.m_ColorLibraryEditorState == null)
      {
        this.m_ColorLibraryEditorState = new PresetLibraryEditorState(ColorPicker.presetsEditorPrefID);
        this.m_ColorLibraryEditorState.TransferEditorPrefsState(true);
      }
      if (this.m_ColorLibraryEditor != null)
        return;
      this.m_ColorLibraryEditor = new PresetLibraryEditor<ColorPresetLibrary>(new ScriptableObjectSaveLoadHelper<ColorPresetLibrary>("colors", SaveType.Text), this.m_ColorLibraryEditorState, new System.Action<int, object>(this.PresetClickedCallback));
      this.m_ColorLibraryEditor.previewAspect = 1f;
      this.m_ColorLibraryEditor.minMaxPreviewHeight = new Vector2(14f, 14f);
      this.m_ColorLibraryEditor.settingsMenuRightMargin = 2f;
      this.m_ColorLibraryEditor.useOnePixelOverlappedGrid = true;
      this.m_ColorLibraryEditor.alwaysShowScrollAreaHorizontalLines = false;
      this.m_ColorLibraryEditor.marginsForGrid = new RectOffset(0, 0, 0, 0);
      this.m_ColorLibraryEditor.marginsForList = new RectOffset(0, 5, 2, 2);
      this.m_ColorLibraryEditor.InitializeGrid(233f - (float) (ColorPicker.styles.background.padding.left + ColorPicker.styles.background.padding.right));
    }

    private void PresetClickedCallback(int clickCount, object presetObject)
    {
      Color c = (Color) presetObject;
      if (!this.m_HDR && (double) c.maxColorComponent > 1.0)
        c = c.RGBMultiplied(1f / c.maxColorComponent);
      this.SetColor(c);
      this.colorChanged = true;
    }

    private void DoColorSwatchAndEyedropper()
    {
      GUILayout.BeginHorizontal();
      if (GUILayout.Button(ColorPicker.styles.eyeDropper, GUIStyle.none, GUILayout.Width(40f), GUILayout.ExpandWidth(false)))
      {
        EyeDropper.Start((GUIView) this.m_Parent);
        this.m_ColorBoxMode = ColorPicker.ColorBoxMode.EyeDropper;
        GUIUtility.ExitGUI();
      }
      Color color = new Color(this.m_R, this.m_G, this.m_B, this.m_A);
      if (this.m_HDR)
        color = ColorPicker.color;
      Rect rect = GUILayoutUtility.GetRect(20f, 20f, 20f, 20f, ColorPicker.styles.colorPickerBox, GUILayout.ExpandWidth(true));
      EditorGUIUtility.DrawColorSwatch(rect, color, this.m_ShowAlpha, this.m_HDR);
      if (Event.current.type == EventType.Repaint)
        ColorPicker.styles.pickerBox.Draw(rect, GUIContent.none, false, false, false, false);
      GUILayout.EndHorizontal();
    }

    private void DoColorSpaceGUI()
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(ColorPicker.styles.colorCycle, GUIStyle.none, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
        this.m_OldColorBoxMode = this.m_ColorBoxMode = (ColorPicker.ColorBoxMode) ((int) (this.m_ColorBoxMode + 1) % 6);
      GUILayout.EndHorizontal();
      GUILayout.BeginHorizontal();
      GUILayout.Space(20f);
      GUILayout.BeginVertical();
      GUILayout.Space(7f);
      bool changed = GUI.changed;
      GUILayout.BeginHorizontal(GUILayout.ExpandHeight(false));
      Rect aspectRect = GUILayoutUtility.GetAspectRect(1f, ColorPicker.styles.pickerBox, GUILayout.MinWidth(64f), GUILayout.MinHeight(64f), GUILayout.MaxWidth(256f), GUILayout.MaxHeight(256f));
      EditorGUILayout.Space();
      Rect rect = GUILayoutUtility.GetRect(8f, 32f, 64f, 128f, ColorPicker.styles.pickerBox);
      rect.height = aspectRect.height;
      GUILayout.EndHorizontal();
      GUI.changed = false;
      switch (this.m_ColorBoxMode)
      {
        case ColorPicker.ColorBoxMode.SV_H:
          this.Slider3D(aspectRect, rect, ref this.m_S, ref this.m_V, ref this.m_H, ColorPicker.styles.pickerBox, ColorPicker.styles.thumb2D, ColorPicker.styles.thumbVert);
          if (GUI.changed)
          {
            this.HSVToRGB();
            break;
          }
          break;
        case ColorPicker.ColorBoxMode.HV_S:
          this.Slider3D(aspectRect, rect, ref this.m_H, ref this.m_V, ref this.m_S, ColorPicker.styles.pickerBox, ColorPicker.styles.thumb2D, ColorPicker.styles.thumbVert);
          if (GUI.changed)
          {
            this.HSVToRGB();
            break;
          }
          break;
        case ColorPicker.ColorBoxMode.HS_V:
          this.Slider3D(aspectRect, rect, ref this.m_H, ref this.m_S, ref this.m_V, ColorPicker.styles.pickerBox, ColorPicker.styles.thumb2D, ColorPicker.styles.thumbVert);
          if (GUI.changed)
          {
            this.HSVToRGB();
            break;
          }
          break;
        case ColorPicker.ColorBoxMode.BG_R:
          this.Slider3D(aspectRect, rect, ref this.m_B, ref this.m_G, ref this.m_R, ColorPicker.styles.pickerBox, ColorPicker.styles.thumb2D, ColorPicker.styles.thumbVert);
          if (GUI.changed)
          {
            this.RGBToHSV();
            break;
          }
          break;
        case ColorPicker.ColorBoxMode.BR_G:
          this.Slider3D(aspectRect, rect, ref this.m_B, ref this.m_R, ref this.m_G, ColorPicker.styles.pickerBox, ColorPicker.styles.thumb2D, ColorPicker.styles.thumbVert);
          if (GUI.changed)
          {
            this.RGBToHSV();
            break;
          }
          break;
        case ColorPicker.ColorBoxMode.RG_B:
          this.Slider3D(aspectRect, rect, ref this.m_R, ref this.m_G, ref this.m_B, ColorPicker.styles.pickerBox, ColorPicker.styles.thumb2D, ColorPicker.styles.thumbVert);
          if (GUI.changed)
          {
            this.RGBToHSV();
            break;
          }
          break;
        case ColorPicker.ColorBoxMode.EyeDropper:
          EyeDropper.DrawPreview(Rect.MinMaxRect(aspectRect.x, aspectRect.y, rect.xMax, aspectRect.yMax));
          break;
      }
      GUI.changed |= changed;
      GUILayout.Space(5f);
      GUILayout.EndVertical();
      GUILayout.Space(20f);
      GUILayout.EndHorizontal();
    }

    private void SetHDRScaleFactor(float value)
    {
      if (!this.m_HDR)
        Debug.LogError((object) "HDR scale is being set in LDR mode!");
      if ((double) value < 1.0)
        Debug.LogError((object) "SetHDRScaleFactor is below 1, should be >= 1!");
      this.m_HDRValues.m_HDRScaleFactor = Mathf.Clamp(value, 0.0f, this.m_HDRConfig.maxBrightness);
      this.m_ColorSliderDirty = true;
      this.m_ColorSpaceBoxDirty = true;
      this.m_RGBHSVSlidersDirty = true;
    }

    private void BrightnessField()
    {
      if (!this.m_HDR)
        return;
      EditorGUI.BeginChangeCheck();
      ++EditorGUI.indentLevel;
      Color color = EditorGUILayout.ColorBrightnessField(GUIContent.Temp("Current Brightness"), ColorPicker.color, this.m_HDRConfig.minBrightness, this.m_HDRConfig.maxBrightness);
      --EditorGUI.indentLevel;
      if (!EditorGUI.EndChangeCheck())
        return;
      float maxColorComponent = color.maxColorComponent;
      if ((double) maxColorComponent > (double) this.m_HDRValues.m_HDRScaleFactor)
        this.SetHDRScaleFactor(maxColorComponent);
      this.SetNormalizedColor(color.RGBMultiplied(1f / this.m_HDRValues.m_HDRScaleFactor));
    }

    private void SetMaxDisplayBrightness(float brightness)
    {
      brightness = Mathf.Clamp(brightness, 1f, this.m_HDRConfig.maxBrightness);
      if ((double) brightness == (double) this.m_HDRValues.m_HDRScaleFactor)
        return;
      Color c = ColorPicker.color.RGBMultiplied(1f / brightness);
      if ((double) c.maxColorComponent > 1.0)
        return;
      this.SetNormalizedColor(c);
      this.SetHDRScaleFactor(brightness);
      this.Repaint();
    }

    private void DoColorSliders()
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(ColorPicker.styles.sliderCycle, GUIStyle.none, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
        this.m_SliderMode = (ColorPicker.SliderMode) ((int) (this.m_SliderMode + 1) % 2);
      GUILayout.EndHorizontal();
      GUILayout.Space(7f);
      switch (this.m_SliderMode)
      {
        case ColorPicker.SliderMode.RGB:
          this.RGBSliders();
          break;
        case ColorPicker.SliderMode.HSV:
          this.HSVSliders();
          break;
      }
      if (!this.m_ShowAlpha)
        return;
      this.m_AlphaTexture = ColorPicker.Update1DSlider(this.m_AlphaTexture, 32, 0.0f, 0.0f, ref this.m_OldAlpha, ref this.m_OldAlpha, 3, false, 1f, -1f, false, this.m_HDRValues.m_TonemappingType);
      this.m_A = this.TexturedSlider(this.m_AlphaTexture, "A", this.m_A, 0.0f, 1f, !this.m_HDR ? (float) byte.MaxValue : 1f, !this.m_HDR ? EditorGUI.kIntFieldFormatString : EditorGUI.kFloatFieldFormatString, (System.Action<float>) null);
    }

    private void DoHexField(float availableWidth)
    {
      float labelWidth = EditorGUIUtility.labelWidth;
      float fieldWidth = EditorGUIUtility.fieldWidth;
      EditorGUIUtility.labelWidth = availableWidth - 85f;
      EditorGUIUtility.fieldWidth = 85f;
      ++EditorGUI.indentLevel;
      EditorGUI.BeginChangeCheck();
      Color c = EditorGUILayout.HexColorTextField(GUIContent.Temp("Hex Color"), ColorPicker.color, this.m_ShowAlpha);
      if (EditorGUI.EndChangeCheck())
      {
        this.SetNormalizedColor(c);
        if (this.m_HDR)
          this.SetHDRScaleFactor(1f);
      }
      --EditorGUI.indentLevel;
      EditorGUIUtility.labelWidth = labelWidth;
      EditorGUIUtility.fieldWidth = fieldWidth;
    }

    private void DoPresetsGUI()
    {
      GUILayout.BeginHorizontal();
      this.m_ShowPresets = GUILayout.Toggle(this.m_ShowPresets, ColorPicker.styles.presetsToggle, ColorPicker.styles.foldout, new GUILayoutOption[0]);
      GUILayout.Space(17f);
      GUILayout.EndHorizontal();
      if (!this.m_ShowPresets)
        return;
      GUILayout.Space(-18f);
      this.m_ColorLibraryEditor.OnGUI(GUILayoutUtility.GetRect(0.0f, Mathf.Clamp(this.m_ColorLibraryEditor.contentHeight, 20f, 250f)), (object) ColorPicker.color);
    }

    private void OnGUI()
    {
      this.InitIfNeeded();
      if (this.m_resetKeyboardControl)
      {
        GUIUtility.keyboardControl = 0;
        this.m_resetKeyboardControl = false;
      }
      if (Event.current.type == EventType.ExecuteCommand)
      {
        string commandName = Event.current.commandName;
        if (commandName != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (ColorPicker.\u003C\u003Ef__switch\u0024map17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ColorPicker.\u003C\u003Ef__switch\u0024map17 = new Dictionary<string, int>(3)
            {
              {
                "EyeDropperUpdate",
                0
              },
              {
                "EyeDropperClicked",
                1
              },
              {
                "EyeDropperCancelled",
                2
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (ColorPicker.\u003C\u003Ef__switch\u0024map17.TryGetValue(commandName, out num))
          {
            switch (num)
            {
              case 0:
                this.Repaint();
                break;
              case 1:
                Color lastPickedColor = EyeDropper.GetLastPickedColor();
                this.m_R = lastPickedColor.r;
                this.m_G = lastPickedColor.g;
                this.m_B = lastPickedColor.b;
                this.RGBToHSV();
                this.m_ColorBoxMode = this.m_OldColorBoxMode;
                this.m_Color = new Color(this.m_R, this.m_G, this.m_B, this.m_A);
                this.SendEvent(true);
                break;
              case 2:
                this.Repaint();
                this.m_ColorBoxMode = this.m_OldColorBoxMode;
                break;
            }
          }
        }
      }
      Rect rect = EditorGUILayout.BeginVertical(ColorPicker.styles.background, new GUILayoutOption[0]);
      float width = EditorGUILayout.GetControlRect(false, 1f, EditorStyles.numberField, new GUILayoutOption[0]).width;
      EditorGUIUtility.labelWidth = width - this.fieldWidth;
      EditorGUIUtility.fieldWidth = this.fieldWidth;
      EditorGUI.BeginChangeCheck();
      GUILayout.Space(10f);
      this.DoColorSwatchAndEyedropper();
      GUILayout.Space(10f);
      if (this.m_HDR)
      {
        this.TonemappingControls();
        GUILayout.Space(10f);
      }
      this.DoColorSpaceGUI();
      GUILayout.Space(10f);
      if (this.m_HDR)
      {
        GUILayout.Space(5f);
        this.BrightnessField();
        GUILayout.Space(10f);
      }
      this.DoColorSliders();
      GUILayout.Space(5f);
      this.DoHexField(width);
      GUILayout.Space(10f);
      if (EditorGUI.EndChangeCheck())
        this.colorChanged = true;
      this.DoPresetsGUI();
      this.HandleCopyPasteEvents();
      if (this.colorChanged)
      {
        this.colorChanged = false;
        this.m_Color = new Color(this.m_R, this.m_G, this.m_B, this.m_A);
        this.SendEvent(true);
      }
      EditorGUILayout.EndVertical();
      if ((double) rect.height > 0.0 && Event.current.type == EventType.Repaint)
        this.SetHeight(rect.height);
      if (Event.current.type == EventType.KeyDown)
      {
        switch (Event.current.keyCode)
        {
          case KeyCode.Return:
          case KeyCode.KeypadEnter:
            this.Close();
            break;
          case KeyCode.Escape:
            Undo.RevertAllDownToGroup(this.m_ModalUndoGroup);
            this.m_Color = this.m_OriginalColor;
            this.SendEvent(false);
            this.Close();
            GUIUtility.ExitGUI();
            break;
        }
      }
      if ((Event.current.type != EventType.MouseDown || Event.current.button == 1) && Event.current.type != EventType.ContextClick)
        return;
      GUIUtility.keyboardControl = 0;
      this.Repaint();
    }

    private void SetHeight(float newHeight)
    {
      if ((double) newHeight == (double) this.position.height)
        return;
      this.minSize = new Vector2(233f, newHeight);
      this.maxSize = new Vector2(233f, newHeight);
    }

    private void HandleCopyPasteEvents()
    {
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.ValidateCommand:
          string commandName1 = current.commandName;
          if (commandName1 == null)
            break;
          // ISSUE: reference to a compiler-generated field
          if (ColorPicker.\u003C\u003Ef__switch\u0024map18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ColorPicker.\u003C\u003Ef__switch\u0024map18 = new Dictionary<string, int>(2)
            {
              {
                "Copy",
                0
              },
              {
                "Paste",
                0
              }
            };
          }
          int num1;
          // ISSUE: reference to a compiler-generated field
          if (!ColorPicker.\u003C\u003Ef__switch\u0024map18.TryGetValue(commandName1, out num1) || num1 != 0)
            break;
          current.Use();
          break;
        case EventType.ExecuteCommand:
          string commandName2 = current.commandName;
          if (commandName2 == null)
            break;
          // ISSUE: reference to a compiler-generated field
          if (ColorPicker.\u003C\u003Ef__switch\u0024map19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ColorPicker.\u003C\u003Ef__switch\u0024map19 = new Dictionary<string, int>(2)
            {
              {
                "Copy",
                0
              },
              {
                "Paste",
                1
              }
            };
          }
          int num2;
          // ISSUE: reference to a compiler-generated field
          if (!ColorPicker.\u003C\u003Ef__switch\u0024map19.TryGetValue(commandName2, out num2))
            break;
          if (num2 != 0)
          {
            Color color;
            if (num2 != 1 || !ColorClipboard.TryGetColor(this.m_HDR, out color))
              break;
            if (!this.m_ShowAlpha)
              color.a = this.m_A;
            this.SetColor(color);
            this.colorChanged = true;
            GUI.changed = true;
            current.Use();
            break;
          }
          ColorClipboard.SetColor(ColorPicker.color);
          current.Use();
          break;
      }
    }

    private float GetScrollWheelDeltaInRect(Rect rect)
    {
      Event current = Event.current;
      if (current.type == EventType.ScrollWheel && rect.Contains(current.mousePosition))
        return current.delta.y;
      return 0.0f;
    }

    private void Slider3D(Rect boxPos, Rect sliderPos, ref float x, ref float y, ref float z, GUIStyle box, GUIStyle thumb2D, GUIStyle thumbHoriz)
    {
      Rect colorBoxRect = boxPos;
      ++colorBoxRect.x;
      ++colorBoxRect.y;
      colorBoxRect.width -= 2f;
      colorBoxRect.height -= 2f;
      this.DrawColorSpaceBox(colorBoxRect, z);
      Vector2 vector2_1 = new Vector2(x, 1f - y);
      Vector2 vector2_2 = this.Slider2D(boxPos, vector2_1, new Vector2(0.0f, 0.0f), new Vector2(1f, 1f), box, thumb2D);
      x = vector2_2.x;
      y = 1f - vector2_2.y;
      if (this.m_HDR)
        this.SpecialHDRBrightnessHandling(boxPos, sliderPos);
      this.DrawColorSlider(new Rect(sliderPos.x + 1f, sliderPos.y + 1f, sliderPos.width - 2f, sliderPos.height - 2f), new Vector2(x, y));
      if (Event.current.type == EventType.MouseDown && sliderPos.Contains(Event.current.mousePosition))
        this.RemoveFocusFromActiveTextField();
      z = GUI.VerticalSlider(sliderPos, z, 1f, 0.0f, box, thumbHoriz);
      ColorPicker.DrawLabelOutsideRect(new Rect(sliderPos.xMax - sliderPos.height, sliderPos.y, sliderPos.height + 1f, sliderPos.height + 1f), this.GetZAxisLabel(this.m_ColorBoxMode), ColorPicker.LabelLocation.Right);
    }

    private void RemoveFocusFromActiveTextField()
    {
      EditorGUI.EndEditingActiveTextField();
      GUIUtility.keyboardControl = 0;
    }

    public static Texture2D GetGradientTextureWithAlpha1To0()
    {
      return ColorPicker.s_LeftGradientTexture ?? (ColorPicker.s_LeftGradientTexture = ColorPicker.CreateGradientTexture("ColorPicker_1To0_Gradient", 4, 4, new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0.0f)));
    }

    public static Texture2D GetGradientTextureWithAlpha0To1()
    {
      return ColorPicker.s_RightGradientTexture ?? (ColorPicker.s_RightGradientTexture = ColorPicker.CreateGradientTexture("ColorPicker_0To1_Gradient", 4, 4, new Color(1f, 1f, 1f, 0.0f), new Color(1f, 1f, 1f, 1f)));
    }

    private static Texture2D CreateGradientTexture(string name, int width, int height, Color leftColor, Color rightColor)
    {
      Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
      texture2D.name = name;
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      Color[] colors = new Color[width * height];
      for (int index1 = 0; index1 < width; ++index1)
      {
        Color color = Color.Lerp(leftColor, rightColor, (float) index1 / (float) (width - 1));
        for (int index2 = 0; index2 < height; ++index2)
          colors[index2 * width + index1] = color;
      }
      texture2D.SetPixels(colors);
      texture2D.wrapMode = TextureWrapMode.Clamp;
      texture2D.Apply();
      return texture2D;
    }

    private void TonemappingControls()
    {
      bool flag = false;
      EditorGUI.BeginChangeCheck();
      this.m_UseTonemappingPreview = GUILayout.Toggle(this.m_UseTonemappingPreview, ColorPicker.styles.tonemappingToggle, ColorPicker.styles.toggle, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        flag = true;
      if (this.m_UseTonemappingPreview)
      {
        ++EditorGUI.indentLevel;
        EditorGUI.BeginChangeCheck();
        float power = QualitySettings.activeColorSpace != ColorSpace.Linear ? 2f : 1f;
        this.m_HDRValues.m_ExposureAdjustment = EditorGUILayout.PowerSlider(string.Empty, this.m_HDRValues.m_ExposureAdjustment, this.m_HDRConfig.minExposureValue, this.m_HDRConfig.maxExposureValue, power);
        if (Event.current.type == EventType.Repaint)
          GUI.Label(EditorGUILayout.s_LastRect, GUIContent.Temp(string.Empty, "Exposure value"));
        if (EditorGUI.EndChangeCheck())
          flag = true;
        Rect controlRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.numberField, new GUILayoutOption[0]);
        EditorGUI.LabelField(controlRect, GUIContent.Temp("Tonemapped Color"));
        Rect position = new Rect(controlRect.xMax - this.fieldWidth, controlRect.y, this.fieldWidth, controlRect.height);
        EditorGUIUtility.DrawColorSwatch(position, ColorPicker.DoTonemapping(ColorPicker.color, this.m_HDRValues.m_ExposureAdjustment), false, false);
        GUI.Label(position, GUIContent.none, ColorPicker.styles.colorPickerBox);
        --EditorGUI.indentLevel;
      }
      if (!flag)
        return;
      this.m_RGBHSVSlidersDirty = true;
      this.m_ColorSpaceBoxDirty = true;
      this.m_ColorSliderDirty = true;
    }

    private static float PhotographicTonemapping(float value, float exposureAdjustment)
    {
      return 1f - Mathf.Pow(2f, -exposureAdjustment * value);
    }

    private static Color DoTonemapping(Color col, float exposureAdjustment)
    {
      col.r = ColorPicker.PhotographicTonemapping(col.r, exposureAdjustment);
      col.g = ColorPicker.PhotographicTonemapping(col.g, exposureAdjustment);
      col.b = ColorPicker.PhotographicTonemapping(col.b, exposureAdjustment);
      return col;
    }

    private static void DoTonemapping(Color[] colors, float exposureAdjustment, ColorPicker.TonemappingType tonemappingType)
    {
      if ((double) exposureAdjustment < 0.0)
        return;
      if (tonemappingType == ColorPicker.TonemappingType.Linear)
      {
        for (int index = 0; index < colors.Length; ++index)
          colors[index] = colors[index].RGBMultiplied(exposureAdjustment);
      }
      else
      {
        for (int index = 0; index < colors.Length; ++index)
          colors[index] = ColorPicker.DoTonemapping(colors[index], exposureAdjustment);
      }
    }

    private void SpecialHDRBrightnessHandling(Rect boxPos, Rect sliderPos)
    {
      if (this.m_ColorBoxMode == ColorPicker.ColorBoxMode.SV_H || this.m_ColorBoxMode == ColorPicker.ColorBoxMode.HV_S)
      {
        float wheelDeltaInRect = this.GetScrollWheelDeltaInRect(boxPos);
        if ((double) wheelDeltaInRect != 0.0)
          this.SetMaxDisplayBrightness(this.m_HDRValues.m_HDRScaleFactor - wheelDeltaInRect * 0.05f);
        Rect rect = new Rect(0.0f, boxPos.y - 7f, boxPos.x - 2f, 14f);
        Rect dragRect = rect;
        dragRect.y += rect.height;
        EditorGUI.BeginChangeCheck();
        float brightness = ColorPicker.EditableAxisLabel(rect, dragRect, this.m_HDRValues.m_HDRScaleFactor, 1f, this.m_HDRConfig.maxBrightness, ColorPicker.styles.axisLabelNumberField);
        if (EditorGUI.EndChangeCheck())
          this.SetMaxDisplayBrightness(brightness);
      }
      if (this.m_ColorBoxMode != ColorPicker.ColorBoxMode.HS_V)
        return;
      Rect rect1 = new Rect(sliderPos.xMax + 2f, sliderPos.y - 7f, (float) ((double) this.position.width - (double) sliderPos.xMax - 2.0), 14f);
      Rect dragRect1 = rect1;
      dragRect1.y += rect1.height;
      EditorGUI.BeginChangeCheck();
      float brightness1 = ColorPicker.EditableAxisLabel(rect1, dragRect1, this.m_HDRValues.m_HDRScaleFactor, 1f, this.m_HDRConfig.maxBrightness, ColorPicker.styles.axisLabelNumberField);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.SetMaxDisplayBrightness(brightness1);
    }

    private static float EditableAxisLabel(Rect rect, Rect dragRect, float value, float minValue, float maxValue, GUIStyle style)
    {
      int controlId = GUIUtility.GetControlID(162594855, FocusType.Keyboard, rect);
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      EditorGUI.kFloatFieldFormatString = (double) value >= 10.0 ? "n0" : "n1";
      float num = EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, rect, dragRect, controlId, value, EditorGUI.kFloatFieldFormatString, style, true);
      EditorGUI.kFloatFieldFormatString = fieldFormatString;
      return Mathf.Clamp(num, minValue, maxValue);
    }

    private void SendEvent(bool exitGUI)
    {
      if (!(bool) ((UnityEngine.Object) this.m_DelegateView))
        return;
      Event e = EditorGUIUtility.CommandEvent("ColorPickerChanged");
      if (!this.m_IsOSColorPicker)
        this.Repaint();
      this.m_DelegateView.SendEvent(e);
      if (this.m_IsOSColorPicker || !exitGUI)
        return;
      GUIUtility.ExitGUI();
    }

    private void SetNormalizedColor(Color c)
    {
      if ((double) c.maxColorComponent > 1.0)
        Debug.LogError((object) ("Setting normalized color with a non-normalized color: " + (object) c));
      this.m_Color = c;
      this.m_R = c.r;
      this.m_G = c.g;
      this.m_B = c.b;
      this.RGBToHSV();
      this.m_A = c.a;
    }

    private void SetColor(Color c)
    {
      if (this.m_IsOSColorPicker)
      {
        OSColorPicker.color = c;
      }
      else
      {
        float hdrScaleFactor = this.m_HDRValues.m_HDRScaleFactor;
        if (this.m_HDR)
        {
          float maxColorComponent = c.maxColorComponent;
          if ((double) maxColorComponent > 1.0)
            c = c.RGBMultiplied(1f / maxColorComponent);
          this.SetHDRScaleFactor(Mathf.Max(1f, maxColorComponent));
        }
        if ((double) this.m_Color.r == (double) c.r && (double) this.m_Color.g == (double) c.g && ((double) this.m_Color.b == (double) c.b && (double) this.m_Color.a == (double) c.a) && (double) hdrScaleFactor == (double) this.m_HDRValues.m_HDRScaleFactor)
          return;
        if ((double) c.r > 1.0 || (double) c.g > 1.0 || (double) c.b > 1.0)
          Debug.LogError((object) string.Format("Invalid normalized color: {0}, normalize value: {1}", (object) c, (object) this.m_HDRValues.m_HDRScaleFactor));
        this.m_resetKeyboardControl = true;
        this.SetNormalizedColor(c);
        this.Repaint();
      }
    }

    public static void Show(GUIView viewToUpdate, Color col)
    {
      ColorPicker.Show(viewToUpdate, col, true, false, (ColorPickerHDRConfig) null);
    }

    public static void Show(GUIView viewToUpdate, Color col, bool showAlpha, bool hdr, ColorPickerHDRConfig hdrConfig)
    {
      ColorPicker get = ColorPicker.get;
      get.m_HDR = hdr;
      get.m_HDRConfig = new ColorPickerHDRConfig(hdrConfig ?? ColorPicker.defaultHDRConfig);
      get.m_DelegateView = viewToUpdate;
      get.SetColor(col);
      get.m_OriginalColor = ColorPicker.get.m_Color;
      get.m_ShowAlpha = showAlpha;
      get.m_ModalUndoGroup = Undo.GetCurrentGroup();
      if (get.m_HDR)
        get.m_IsOSColorPicker = false;
      if (get.m_IsOSColorPicker)
      {
        OSColorPicker.Show(showAlpha);
      }
      else
      {
        get.titleContent = !hdr ? EditorGUIUtility.TextContent("Color") : EditorGUIUtility.TextContent("HDR Color");
        float y = (float) EditorPrefs.GetInt("CPickerHeight", (int) get.position.height);
        get.minSize = new Vector2(233f, y);
        get.maxSize = new Vector2(233f, y);
        get.InitIfNeeded();
        get.ShowAuxWindow();
      }
    }

    private void PollOSColorPicker()
    {
      if (!this.m_IsOSColorPicker)
        return;
      if (!OSColorPicker.visible || Application.platform != RuntimePlatform.OSXEditor)
      {
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this);
      }
      else
      {
        Color color = OSColorPicker.color;
        if (!(this.m_Color != color))
          return;
        this.m_Color = color;
        this.SendEvent(true);
      }
    }

    private void OnEnable()
    {
      this.m_HDRValues.m_ExposureAdjustment = EditorPrefs.GetFloat("CPickerExposure", 1f);
      this.m_UseTonemappingPreview = EditorPrefs.GetInt("CPTonePreview", 0) != 0;
      this.m_SliderMode = (ColorPicker.SliderMode) EditorPrefs.GetInt("CPSliderMode", 0);
      this.m_ColorBoxMode = (ColorPicker.ColorBoxMode) EditorPrefs.GetInt("CPColorMode", 0);
      this.m_ShowPresets = EditorPrefs.GetInt("CPPresetsShow", 1) != 0;
    }

    private void OnDisable()
    {
      EditorPrefs.SetFloat("CPickerExposure", this.m_HDRValues.m_ExposureAdjustment);
      EditorPrefs.SetInt("CPTonePreview", !this.m_UseTonemappingPreview ? 0 : 1);
      EditorPrefs.SetInt("CPSliderMode", (int) this.m_SliderMode);
      EditorPrefs.SetInt("CPColorMode", (int) this.m_ColorBoxMode);
      EditorPrefs.SetInt("CPPresetsShow", !this.m_ShowPresets ? 0 : 1);
      EditorPrefs.SetInt("CPickerHeight", (int) this.position.height);
    }

    public void OnDestroy()
    {
      Undo.CollapseUndoOperations(this.m_ModalUndoGroup);
      if ((bool) ((UnityEngine.Object) this.m_ColorSlider))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ColorSlider);
      if ((bool) ((UnityEngine.Object) this.m_ColorBox))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ColorBox);
      if ((bool) ((UnityEngine.Object) this.m_RTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_RTexture);
      if ((bool) ((UnityEngine.Object) this.m_GTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_GTexture);
      if ((bool) ((UnityEngine.Object) this.m_BTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_BTexture);
      if ((bool) ((UnityEngine.Object) this.m_HueTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_HueTexture);
      if ((bool) ((UnityEngine.Object) this.m_SatTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_SatTexture);
      if ((bool) ((UnityEngine.Object) this.m_ValTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ValTexture);
      if ((bool) ((UnityEngine.Object) this.m_AlphaTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_AlphaTexture);
      ColorPicker.s_SharedColorPicker = (ColorPicker) null;
      if (this.m_IsOSColorPicker)
        OSColorPicker.Close();
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.PollOSColorPicker);
      if (this.m_ColorLibraryEditorState != null)
        this.m_ColorLibraryEditorState.TransferEditorPrefsState(false);
      if (this.m_ColorLibraryEditor != null)
        this.m_ColorLibraryEditor.UnloadUsedLibraries();
      if (this.m_ColorBoxMode != ColorPicker.ColorBoxMode.EyeDropper)
        return;
      EditorPrefs.SetInt("CPColorMode", (int) this.m_OldColorBoxMode);
    }

    internal enum TonemappingType
    {
      Linear,
      Photographic,
    }

    [Serializable]
    private class HDRValues
    {
      [NonSerialized]
      public ColorPicker.TonemappingType m_TonemappingType = ColorPicker.TonemappingType.Photographic;
      [SerializeField]
      public float m_ExposureAdjustment = 1.5f;
      [SerializeField]
      public float m_HDRScaleFactor;
    }

    private enum ColorBoxMode
    {
      SV_H,
      HV_S,
      HS_V,
      BG_R,
      BR_G,
      RG_B,
      EyeDropper,
    }

    private enum SliderMode
    {
      RGB,
      HSV,
    }

    private enum LabelLocation
    {
      Top,
      Bottom,
      Left,
      Right,
    }

    private class Styles
    {
      public GUIStyle pickerBox = (GUIStyle) "ColorPickerBox";
      public GUIStyle thumb2D = (GUIStyle) "ColorPicker2DThumb";
      public GUIStyle thumbHoriz = (GUIStyle) "ColorPickerHorizThumb";
      public GUIStyle thumbVert = (GUIStyle) "ColorPickerVertThumb";
      public GUIStyle headerLine = (GUIStyle) "IN Title";
      public GUIStyle colorPickerBox = (GUIStyle) "ColorPickerBox";
      public GUIStyle background = new GUIStyle((GUIStyle) "ColorPickerBackground");
      public GUIStyle label = new GUIStyle(EditorStyles.miniLabel);
      public GUIStyle axisLabelNumberField = new GUIStyle(EditorStyles.miniTextField);
      public GUIStyle foldout = new GUIStyle(EditorStyles.foldout);
      public GUIStyle toggle = new GUIStyle(EditorStyles.toggle);
      public GUIContent eyeDropper = EditorGUIUtility.IconContent("EyeDropper.Large", "|Pick a color from the screen.");
      public GUIContent colorCycle = EditorGUIUtility.IconContent("ColorPicker.CycleColor");
      public GUIContent colorToggle = EditorGUIUtility.TextContent("Colors");
      public GUIContent tonemappingToggle = new GUIContent("Tonemapped Preview", "When enabled preview colors are tonemapped using Photographic Tonemapping");
      public GUIContent sliderToggle = EditorGUIUtility.TextContent("Sliders|The RGB or HSV color sliders.");
      public GUIContent presetsToggle = new GUIContent("Presets");
      public GUIContent sliderCycle = EditorGUIUtility.IconContent("ColorPicker.CycleSlider");

      public Styles()
      {
        this.axisLabelNumberField.alignment = TextAnchor.UpperRight;
        this.axisLabelNumberField.normal.background = (Texture2D) null;
        this.label.alignment = TextAnchor.LowerCenter;
      }
    }
  }
}
