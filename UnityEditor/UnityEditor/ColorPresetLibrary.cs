// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorPresetLibrary
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class ColorPresetLibrary : PresetLibrary
  {
    [SerializeField]
    private List<ColorPresetLibrary.ColorPreset> m_Presets = new List<ColorPresetLibrary.ColorPreset>();
    public const int kSwatchSize = 14;
    public const int kMiniSwatchSize = 8;
    private Texture2D m_ColorSwatch;
    private Texture2D m_ColorSwatchTriangular;
    private Texture2D m_MiniColorSwatchTriangular;
    private Texture2D m_CheckerBoard;

    private void OnDestroy()
    {
      if ((UnityEngine.Object) this.m_ColorSwatch != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ColorSwatch);
      if ((UnityEngine.Object) this.m_ColorSwatchTriangular != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ColorSwatchTriangular);
      if ((UnityEngine.Object) this.m_MiniColorSwatchTriangular != (UnityEngine.Object) null)
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_MiniColorSwatchTriangular);
      if (!((UnityEngine.Object) this.m_CheckerBoard != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_CheckerBoard);
    }

    public override int Count()
    {
      return this.m_Presets.Count;
    }

    public override object GetPreset(int index)
    {
      return (object) this.m_Presets[index].color;
    }

    public override void Add(object presetObject, string presetName)
    {
      this.m_Presets.Add(new ColorPresetLibrary.ColorPreset((Color) presetObject, presetName));
    }

    public override void Replace(int index, object newPresetObject)
    {
      Color color = (Color) newPresetObject;
      this.m_Presets[index].color = color;
    }

    public override void Remove(int index)
    {
      this.m_Presets.RemoveAt(index);
    }

    public override void Move(int index, int destIndex, bool insertAfterDestIndex)
    {
      PresetLibraryHelpers.MoveListItem<ColorPresetLibrary.ColorPreset>(this.m_Presets, index, destIndex, insertAfterDestIndex);
    }

    public override void Draw(Rect rect, int index)
    {
      this.DrawInternal(rect, this.m_Presets[index].color);
    }

    public override void Draw(Rect rect, object presetObject)
    {
      this.DrawInternal(rect, (Color) presetObject);
    }

    private void Init()
    {
      if ((UnityEngine.Object) this.m_ColorSwatch == (UnityEngine.Object) null)
        this.m_ColorSwatch = ColorPresetLibrary.CreateColorSwatchWithBorder(14, 14, false);
      if ((UnityEngine.Object) this.m_ColorSwatchTriangular == (UnityEngine.Object) null)
        this.m_ColorSwatchTriangular = ColorPresetLibrary.CreateColorSwatchWithBorder(14, 14, true);
      if ((UnityEngine.Object) this.m_MiniColorSwatchTriangular == (UnityEngine.Object) null)
        this.m_MiniColorSwatchTriangular = ColorPresetLibrary.CreateColorSwatchWithBorder(8, 8, true);
      if (!((UnityEngine.Object) this.m_CheckerBoard == (UnityEngine.Object) null))
        return;
      this.m_CheckerBoard = GradientEditor.CreateCheckerTexture(2, 2, 3, new Color(0.8f, 0.8f, 0.8f), new Color(0.5f, 0.5f, 0.5f));
    }

    private void DrawInternal(Rect rect, Color preset)
    {
      this.Init();
      bool flag = (double) preset.maxColorComponent > 1.0;
      if (flag)
        preset = preset.RGBMultiplied(1f / preset.maxColorComponent);
      Color color = GUI.color;
      if ((int) rect.height == 14)
      {
        if ((double) preset.a > 0.970000028610229)
          this.RenderSolidSwatch(rect, preset);
        else
          this.RenderSwatchWithAlpha(rect, preset, this.m_ColorSwatchTriangular);
        if (flag)
          GUI.Label(rect, "h");
      }
      else
        this.RenderSwatchWithAlpha(rect, preset, this.m_MiniColorSwatchTriangular);
      GUI.color = color;
    }

    private void RenderSolidSwatch(Rect rect, Color preset)
    {
      GUI.color = preset;
      GUI.DrawTexture(rect, (Texture) this.m_ColorSwatch);
    }

    private void RenderSwatchWithAlpha(Rect rect, Color preset, Texture2D swatchTexture)
    {
      Rect position = new Rect(rect.x + 1f, rect.y + 1f, rect.width - 2f, rect.height - 2f);
      GUI.color = Color.white;
      Rect texCoords = new Rect(0.0f, 0.0f, position.width / (float) this.m_CheckerBoard.width, position.height / (float) this.m_CheckerBoard.height);
      GUI.DrawTextureWithTexCoords(position, (Texture) this.m_CheckerBoard, texCoords, false);
      GUI.color = preset;
      GUI.DrawTexture(rect, (Texture) EditorGUIUtility.whiteTexture);
      GUI.color = new Color(preset.r, preset.g, preset.b, 1f);
      GUI.DrawTexture(rect, (Texture) swatchTexture);
    }

    public override string GetName(int index)
    {
      return this.m_Presets[index].name;
    }

    public override void SetName(int index, string presetName)
    {
      this.m_Presets[index].name = presetName;
    }

    public void CreateDebugColors()
    {
      for (int index = 0; index < 2000; ++index)
        this.m_Presets.Add(new ColorPresetLibrary.ColorPreset(new Color(UnityEngine.Random.Range(0.2f, 1f), UnityEngine.Random.Range(0.2f, 1f), UnityEngine.Random.Range(0.2f, 1f), 1f), "Preset Color " + (object) index));
    }

    private static Texture2D CreateColorSwatchWithBorder(int width, int height, bool triangular)
    {
      Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      Color[] colors = new Color[width * height];
      Color color = new Color(1f, 1f, 1f, 0.0f);
      if (triangular)
      {
        for (int index1 = 0; index1 < height; ++index1)
        {
          for (int index2 = 0; index2 < width; ++index2)
            colors[index2 + index1 * width] = index1 >= width - index2 ? color : Color.white;
        }
      }
      else
      {
        for (int index = 0; index < height * width; ++index)
          colors[index] = Color.white;
      }
      for (int index = 0; index < width; ++index)
        colors[index] = Color.black;
      for (int index = 0; index < width; ++index)
        colors[(height - 1) * width + index] = Color.black;
      for (int index = 0; index < height; ++index)
        colors[index * width] = Color.black;
      for (int index = 0; index < height; ++index)
        colors[index * width + width - 1] = Color.black;
      texture2D.SetPixels(colors);
      texture2D.Apply();
      return texture2D;
    }

    [Serializable]
    private class ColorPreset
    {
      [SerializeField]
      private string m_Name;
      [SerializeField]
      private Color m_Color;

      public Color color
      {
        get
        {
          return this.m_Color;
        }
        set
        {
          this.m_Color = value;
        }
      }

      public string name
      {
        get
        {
          return this.m_Name;
        }
        set
        {
          this.m_Name = value;
        }
      }

      public ColorPreset(Color preset, string presetName)
      {
        this.color = preset;
        this.name = presetName;
      }

      public ColorPreset(Color preset, Color preset2, string presetName)
      {
        this.color = preset;
        this.name = presetName;
      }
    }
  }
}
