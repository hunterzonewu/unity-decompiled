// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurvePresetLibrary
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class CurvePresetLibrary : PresetLibrary
  {
    [SerializeField]
    private List<CurvePresetLibrary.CurvePreset> m_Presets = new List<CurvePresetLibrary.CurvePreset>();

    public override int Count()
    {
      return this.m_Presets.Count;
    }

    public override object GetPreset(int index)
    {
      return (object) this.m_Presets[index].curve;
    }

    public override void Add(object presetObject, string presetName)
    {
      AnimationCurve animationCurve = presetObject as AnimationCurve;
      if (animationCurve == null)
        Debug.LogError((object) "Wrong type used in CurvePresetLibrary");
      else
        this.m_Presets.Add(new CurvePresetLibrary.CurvePreset(new AnimationCurve(animationCurve.keys)
        {
          preWrapMode = animationCurve.preWrapMode,
          postWrapMode = animationCurve.postWrapMode
        }, presetName));
    }

    public override void Replace(int index, object newPresetObject)
    {
      AnimationCurve animationCurve = newPresetObject as AnimationCurve;
      if (animationCurve == null)
        Debug.LogError((object) "Wrong type used in CurvePresetLibrary");
      else
        this.m_Presets[index].curve = new AnimationCurve(animationCurve.keys)
        {
          preWrapMode = animationCurve.preWrapMode,
          postWrapMode = animationCurve.postWrapMode
        };
    }

    public override void Remove(int index)
    {
      this.m_Presets.RemoveAt(index);
    }

    public override void Move(int index, int destIndex, bool insertAfterDestIndex)
    {
      PresetLibraryHelpers.MoveListItem<CurvePresetLibrary.CurvePreset>(this.m_Presets, index, destIndex, insertAfterDestIndex);
    }

    public override void Draw(Rect rect, int index)
    {
      this.DrawInternal(rect, this.m_Presets[index].curve);
    }

    public override void Draw(Rect rect, object presetObject)
    {
      this.DrawInternal(rect, presetObject as AnimationCurve);
    }

    private void DrawInternal(Rect rect, AnimationCurve animCurve)
    {
      if (animCurve == null)
        return;
      EditorGUIUtility.DrawCurveSwatch(rect, animCurve, (SerializedProperty) null, new Color(0.8f, 0.8f, 0.8f, 1f), EditorGUI.kCurveBGColor);
    }

    public override string GetName(int index)
    {
      return this.m_Presets[index].name;
    }

    public override void SetName(int index, string presetName)
    {
      this.m_Presets[index].name = presetName;
    }

    [Serializable]
    private class CurvePreset
    {
      [SerializeField]
      private string m_Name;
      [SerializeField]
      private AnimationCurve m_Curve;

      public AnimationCurve curve
      {
        get
        {
          return this.m_Curve;
        }
        set
        {
          this.m_Curve = value;
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

      public CurvePreset(AnimationCurve preset, string presetName)
      {
        this.curve = preset;
        this.name = presetName;
      }

      public CurvePreset(AnimationCurve preset, AnimationCurve preset2, string presetName)
      {
        this.curve = preset;
        this.name = presetName;
      }
    }
  }
}
