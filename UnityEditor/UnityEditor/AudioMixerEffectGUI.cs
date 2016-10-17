// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerEffectGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
  internal static class AudioMixerEffectGUI
  {
    private const string kAudioSliderFloatFormat = "F2";
    private const string kExposedParameterUnicodeChar = " ➔";

    private static AudioMixerDrawUtils.Styles styles
    {
      get
      {
        return AudioMixerDrawUtils.styles;
      }
    }

    public static void EffectHeader(string text)
    {
      GUILayout.Label(text, AudioMixerEffectGUI.styles.headerStyle, new GUILayoutOption[0]);
    }

    public static bool Slider(GUIContent label, ref float value, float displayScale, float displayExponent, string unit, float leftValue, float rightValue, AudioMixerController controller, AudioParameterPath path, params GUILayoutOption[] options)
    {
      EditorGUI.BeginChangeCheck();
      float fieldWidth = EditorGUIUtility.fieldWidth;
      string fieldFormatString = EditorGUI.kFloatFieldFormatString;
      bool flag = controller.ContainsExposedParameter(path.parameter);
      EditorGUIUtility.fieldWidth = 70f;
      EditorGUI.kFloatFieldFormatString = "F2";
      EditorGUI.s_UnitString = unit;
      GUIContent label1 = label;
      if (flag)
        label1 = GUIContent.Temp(label.text + " ➔", label.tooltip);
      float num1 = value * displayScale;
      float num2 = EditorGUILayout.PowerSlider(label1, num1, leftValue * displayScale, rightValue * displayScale, displayExponent, options);
      EditorGUI.s_UnitString = (string) null;
      EditorGUI.kFloatFieldFormatString = fieldFormatString;
      EditorGUIUtility.fieldWidth = fieldWidth;
      if (Event.current.type == EventType.ContextClick && GUILayoutUtility.topLevel.GetLast().Contains(Event.current.mousePosition))
      {
        Event.current.Use();
        GenericMenu genericMenu = new GenericMenu();
        if (!flag)
          genericMenu.AddItem(new GUIContent("Expose '" + path.ResolveStringPath(false) + "' to script"), false, new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ExposePopupCallback), (object) new AudioMixerEffectGUI.ExposedParamContext(controller, path));
        else
          genericMenu.AddItem(new GUIContent("Unexpose"), false, new GenericMenu.MenuFunction2(AudioMixerEffectGUI.UnexposePopupCallback), (object) new AudioMixerEffectGUI.ExposedParamContext(controller, path));
        ParameterTransitionType type;
        controller.TargetSnapshot.GetTransitionTypeOverride(path.parameter, out type);
        genericMenu.AddSeparator(string.Empty);
        genericMenu.AddItem(new GUIContent("Linear Snapshot Transition"), type == ParameterTransitionType.Lerp, new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback), (object) new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Lerp));
        genericMenu.AddItem(new GUIContent("Smoothstep Snapshot Transition"), type == ParameterTransitionType.Smoothstep, new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback), (object) new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Smoothstep));
        genericMenu.AddItem(new GUIContent("Squared Snapshot Transition"), type == ParameterTransitionType.Squared, new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback), (object) new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Squared));
        genericMenu.AddItem(new GUIContent("SquareRoot Snapshot Transition"), type == ParameterTransitionType.SquareRoot, new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback), (object) new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.SquareRoot));
        genericMenu.AddItem(new GUIContent("BrickwallStart Snapshot Transition"), type == ParameterTransitionType.BrickwallStart, new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback), (object) new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.BrickwallStart));
        genericMenu.AddItem(new GUIContent("BrickwallEnd Snapshot Transition"), type == ParameterTransitionType.BrickwallEnd, new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback), (object) new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.BrickwallEnd));
        genericMenu.AddSeparator(string.Empty);
        genericMenu.ShowAsContext();
      }
      if (!EditorGUI.EndChangeCheck())
        return false;
      value = num2 / displayScale;
      return true;
    }

    public static void ExposePopupCallback(object obj)
    {
      AudioMixerEffectGUI.ExposedParamContext exposedParamContext = (AudioMixerEffectGUI.ExposedParamContext) obj;
      Undo.RecordObject((Object) exposedParamContext.controller, "Expose Mixer Parameter");
      exposedParamContext.controller.AddExposedParameter(exposedParamContext.path);
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public static void UnexposePopupCallback(object obj)
    {
      AudioMixerEffectGUI.ExposedParamContext exposedParamContext = (AudioMixerEffectGUI.ExposedParamContext) obj;
      Undo.RecordObject((Object) exposedParamContext.controller, "Unexpose Mixer Parameter");
      exposedParamContext.controller.RemoveExposedParameter(exposedParamContext.path.parameter);
      AudioMixerUtility.RepaintAudioMixerAndInspectors();
    }

    public static void ParameterTransitionOverrideCallback(object obj)
    {
      AudioMixerEffectGUI.ParameterTransitionOverrideContext transitionOverrideContext = (AudioMixerEffectGUI.ParameterTransitionOverrideContext) obj;
      Undo.RecordObject((Object) transitionOverrideContext.controller, "Change Parameter Transition Type");
      if (transitionOverrideContext.type == ParameterTransitionType.Lerp)
        transitionOverrideContext.controller.TargetSnapshot.ClearTransitionTypeOverride(transitionOverrideContext.parameter);
      else
        transitionOverrideContext.controller.TargetSnapshot.SetTransitionTypeOverride(transitionOverrideContext.parameter, transitionOverrideContext.type);
    }

    public static bool PopupButton(GUIContent label, GUIContent buttonContent, GUIStyle style, out Rect buttonRect, params GUILayoutOption[] options)
    {
      if (label != null)
      {
        Rect rect = EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options);
        int controlId = GUIUtility.GetControlID("EditorPopup".GetHashCode(), EditorGUIUtility.native, rect);
        buttonRect = EditorGUI.PrefixLabel(rect, controlId, label);
      }
      else
      {
        Rect rect = GUILayoutUtility.GetRect(buttonContent, style, options);
        buttonRect = rect;
      }
      return EditorGUI.ButtonMouseDown(buttonRect, buttonContent, FocusType.Passive, style);
    }

    private class ExposedParamContext
    {
      public AudioMixerController controller;
      public AudioParameterPath path;

      public ExposedParamContext(AudioMixerController controller, AudioParameterPath path)
      {
        this.controller = controller;
        this.path = path;
      }
    }

    private class ParameterTransitionOverrideContext
    {
      public AudioMixerController controller;
      public GUID parameter;
      public ParameterTransitionType type;

      public ParameterTransitionOverrideContext(AudioMixerController controller, GUID parameter, ParameterTransitionType type)
      {
        this.controller = controller;
        this.parameter = parameter;
        this.type = type;
      }
    }

    private class ParameterTransitionOverrideRemoveContext
    {
      public AudioMixerController controller;
      public GUID parameter;

      public ParameterTransitionOverrideRemoveContext(AudioMixerController controller, GUID parameter)
      {
        this.controller = controller;
        this.parameter = parameter;
      }
    }
  }
}
