// Decompiled with JetBrains decompiler
// Type: UnityEditor.GradientPresetLibrary
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class GradientPresetLibrary : PresetLibrary
  {
    [SerializeField]
    private List<GradientPresetLibrary.GradientPreset> m_Presets = new List<GradientPresetLibrary.GradientPreset>();

    public override int Count()
    {
      return this.m_Presets.Count;
    }

    public override object GetPreset(int index)
    {
      return (object) this.m_Presets[index].gradient;
    }

    public override void Add(object presetObject, string presetName)
    {
      Gradient gradient = presetObject as Gradient;
      if (gradient == null)
        Debug.LogError((object) "Wrong type used in GradientPresetLibrary");
      else
        this.m_Presets.Add(new GradientPresetLibrary.GradientPreset(new Gradient()
        {
          alphaKeys = gradient.alphaKeys,
          colorKeys = gradient.colorKeys
        }, presetName));
    }

    public override void Replace(int index, object newPresetObject)
    {
      Gradient gradient = newPresetObject as Gradient;
      if (gradient == null)
        Debug.LogError((object) "Wrong type used in GradientPresetLibrary");
      else
        this.m_Presets[index].gradient = new Gradient()
        {
          alphaKeys = gradient.alphaKeys,
          colorKeys = gradient.colorKeys
        };
    }

    public override void Remove(int index)
    {
      this.m_Presets.RemoveAt(index);
    }

    public override void Move(int index, int destIndex, bool insertAfterDestIndex)
    {
      PresetLibraryHelpers.MoveListItem<GradientPresetLibrary.GradientPreset>(this.m_Presets, index, destIndex, insertAfterDestIndex);
    }

    public override void Draw(Rect rect, int index)
    {
      this.DrawInternal(rect, this.m_Presets[index].gradient);
    }

    public override void Draw(Rect rect, object presetObject)
    {
      this.DrawInternal(rect, presetObject as Gradient);
    }

    private void DrawInternal(Rect rect, Gradient gradient)
    {
      if (gradient == null)
        return;
      GradientEditor.DrawGradientWithBackground(rect, GradientPreviewCache.GetGradientPreview(gradient));
    }

    public override string GetName(int index)
    {
      return this.m_Presets[index].name;
    }

    public override void SetName(int index, string presetName)
    {
      this.m_Presets[index].name = presetName;
    }

    public void DebugCreateTonsOfPresets()
    {
      int num1 = 150;
      string str = "Preset_";
      for (int index1 = 0; index1 < num1; ++index1)
      {
        List<GradientColorKey> gradientColorKeyList = new List<GradientColorKey>();
        int num2 = UnityEngine.Random.Range(3, 8);
        for (int index2 = 0; index2 < num2; ++index2)
          gradientColorKeyList.Add(new GradientColorKey(new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value), UnityEngine.Random.value));
        List<GradientAlphaKey> gradientAlphaKeyList = new List<GradientAlphaKey>();
        int num3 = UnityEngine.Random.Range(3, 8);
        for (int index2 = 0; index2 < num3; ++index2)
          gradientAlphaKeyList.Add(new GradientAlphaKey(UnityEngine.Random.value, UnityEngine.Random.value));
        this.Add((object) new Gradient()
        {
          colorKeys = gradientColorKeyList.ToArray(),
          alphaKeys = gradientAlphaKeyList.ToArray()
        }, str + (object) (index1 + 1));
      }
    }

    [Serializable]
    private class GradientPreset
    {
      [SerializeField]
      private string m_Name;
      [SerializeField]
      private Gradient m_Gradient;

      public Gradient gradient
      {
        get
        {
          return this.m_Gradient;
        }
        set
        {
          this.m_Gradient = value;
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

      public GradientPreset(Gradient preset, string presetName)
      {
        this.gradient = preset;
        this.name = presetName;
      }
    }
  }
}
