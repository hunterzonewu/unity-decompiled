// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioMixerExposedParametersPopup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEditor.Audio;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class AudioMixerExposedParametersPopup : PopupWindowContent
  {
    private static GUIContent m_ButtonContent = new GUIContent(string.Empty, "Audio Mixer parameters can be exposed to scripting. Select an Audio Mixer Group, right click one of its properties in the Inspector and select 'Expose ..'.");
    private static int m_LastNumExposedParams = -1;
    private readonly AudioMixerExposedParameterView m_ExposedParametersView;

    private AudioMixerExposedParametersPopup(AudioMixerController controller)
    {
      this.m_ExposedParametersView = new AudioMixerExposedParameterView(new ReorderableListWithRenameAndScrollView.State());
      this.m_ExposedParametersView.OnMixerControllerChanged(controller);
    }

    internal static void Popup(AudioMixerController controller, GUIStyle style, params GUILayoutOption[] options)
    {
      GUIContent buttonContent = AudioMixerExposedParametersPopup.GetButtonContent(controller);
      Rect rect = GUILayoutUtility.GetRect(buttonContent, style, options);
      if (!EditorGUI.ButtonMouseDown(rect, buttonContent, FocusType.Passive, style))
        return;
      PopupWindow.Show(rect, (PopupWindowContent) new AudioMixerExposedParametersPopup(controller));
    }

    private static GUIContent GetButtonContent(AudioMixerController controller)
    {
      if (controller.numExposedParameters != AudioMixerExposedParametersPopup.m_LastNumExposedParams)
      {
        AudioMixerExposedParametersPopup.m_ButtonContent.text = string.Format("Exposed Parameters ({0})", (object) controller.numExposedParameters);
        AudioMixerExposedParametersPopup.m_LastNumExposedParams = controller.numExposedParameters;
      }
      return AudioMixerExposedParametersPopup.m_ButtonContent;
    }

    public override void OnGUI(Rect rect)
    {
      this.m_ExposedParametersView.OnEvent();
      this.m_ExposedParametersView.OnGUI(rect);
    }

    public override Vector2 GetWindowSize()
    {
      Vector2 vector2 = this.m_ExposedParametersView.CalcSize();
      vector2.x = Math.Max(vector2.x, 125f);
      vector2.y = Math.Max(vector2.y, 23f);
      return vector2;
    }
  }
}
