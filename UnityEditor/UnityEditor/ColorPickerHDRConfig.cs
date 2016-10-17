// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorPickerHDRConfig
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Used as input to ColorField to configure the HDR color ranges in the ColorPicker.</para>
  /// </summary>
  [Serializable]
  public class ColorPickerHDRConfig
  {
    private static readonly ColorPickerHDRConfig s_Temp = new ColorPickerHDRConfig(0.0f, 0.0f, 0.0f, 0.0f);
    /// <summary>
    ///   <para>Minimum allowed color component value when using the ColorPicker.</para>
    /// </summary>
    [SerializeField]
    public float minBrightness;
    /// <summary>
    ///   <para>Maximum allowed color component value when using the ColorPicker.</para>
    /// </summary>
    [SerializeField]
    public float maxBrightness;
    /// <summary>
    ///   <para>Minimum exposure value allowed in the Color Picker.</para>
    /// </summary>
    [SerializeField]
    public float minExposureValue;
    /// <summary>
    ///   <para>Maximum exposure value allowed in the Color Picker.</para>
    /// </summary>
    [SerializeField]
    public float maxExposureValue;

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="minBrightness">Minimum brightness value allowed when using the Color Picker.</param>
    /// <param name="maxBrightness">Maximum brightness value allowed when using the Color Picker.</param>
    /// <param name="minExposureValue">Minimum exposure value used in the tonemapping section of the Color Picker.</param>
    /// <param name="maxExposureValue">Maximum exposure value used in the tonemapping section of the Color Picker.</param>
    public ColorPickerHDRConfig(float minBrightness, float maxBrightness, float minExposureValue, float maxExposureValue)
    {
      this.minBrightness = minBrightness;
      this.maxBrightness = maxBrightness;
      this.minExposureValue = minExposureValue;
      this.maxExposureValue = maxExposureValue;
    }

    internal ColorPickerHDRConfig(ColorPickerHDRConfig other)
    {
      this.minBrightness = other.minBrightness;
      this.maxBrightness = other.maxBrightness;
      this.minExposureValue = other.minExposureValue;
      this.maxExposureValue = other.maxExposureValue;
    }

    internal static ColorPickerHDRConfig Temp(float minBrightness, float maxBrightness, float minExposure, float maxExposure)
    {
      ColorPickerHDRConfig.s_Temp.minBrightness = minBrightness;
      ColorPickerHDRConfig.s_Temp.maxBrightness = maxBrightness;
      ColorPickerHDRConfig.s_Temp.minExposureValue = minExposure;
      ColorPickerHDRConfig.s_Temp.maxExposureValue = maxExposure;
      return ColorPickerHDRConfig.s_Temp;
    }
  }
}
