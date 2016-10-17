// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioFilterGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class AudioFilterGUI
  {
    private EditorGUI.VUMeter.SmoothingData[] dataOut;

    public void DrawAudioFilterGUI(MonoBehaviour behaviour)
    {
      int filterChannelCount = AudioUtil.GetCustomFilterChannelCount(behaviour);
      if (filterChannelCount <= 0)
        return;
      if (this.dataOut == null)
        this.dataOut = new EditorGUI.VUMeter.SmoothingData[filterChannelCount];
      double num = (double) AudioUtil.GetCustomFilterProcessTime(behaviour) / 1000000.0;
      float r = (float) num / ((float) AudioSettings.outputSampleRate / 1024f / (float) filterChannelCount);
      GUILayout.BeginHorizontal();
      GUILayout.Space(13f);
      GUILayout.BeginVertical();
      EditorGUILayout.Space();
      for (int channel = 0; channel < filterChannelCount; ++channel)
        EditorGUILayout.VUMeterHorizontal(AudioUtil.GetCustomFilterMaxOut(behaviour, channel), ref this.dataOut[channel], GUILayout.MinWidth(50f), GUILayout.Height(5f));
      GUILayout.EndVertical();
      Color color = GUI.color;
      GUI.color = new Color(r, 1f - r, 0.0f, 1f);
      GUILayout.Box(string.Format("{0:00.00}ms", (object) num), new GUILayoutOption[2]
      {
        GUILayout.MinWidth(40f),
        GUILayout.Height(20f)
      });
      GUI.color = color;
      GUILayout.EndHorizontal();
      EditorGUILayout.Space();
      GUIView.current.Repaint();
    }
  }
}
