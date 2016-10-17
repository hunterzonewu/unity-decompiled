// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioManagerInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioManager))]
  internal class AudioManagerInspector : Editor
  {
    private SerializedProperty m_Volume;
    private SerializedProperty m_RolloffScale;
    private SerializedProperty m_DopplerFactor;
    private SerializedProperty m_DefaultSpeakerMode;
    private SerializedProperty m_SampleRate;
    private SerializedProperty m_DSPBufferSize;
    private SerializedProperty m_VirtualVoiceCount;
    private SerializedProperty m_RealVoiceCount;
    private SerializedProperty m_SpatializerPlugin;
    private SerializedProperty m_DisableAudio;
    private SerializedProperty m_VirtualizeEffects;

    private void OnEnable()
    {
      this.m_Volume = this.serializedObject.FindProperty("m_Volume");
      this.m_RolloffScale = this.serializedObject.FindProperty("Rolloff Scale");
      this.m_DopplerFactor = this.serializedObject.FindProperty("Doppler Factor");
      this.m_DefaultSpeakerMode = this.serializedObject.FindProperty("Default Speaker Mode");
      this.m_SampleRate = this.serializedObject.FindProperty("m_SampleRate");
      this.m_DSPBufferSize = this.serializedObject.FindProperty("m_DSPBufferSize");
      this.m_VirtualVoiceCount = this.serializedObject.FindProperty("m_VirtualVoiceCount");
      this.m_RealVoiceCount = this.serializedObject.FindProperty("m_RealVoiceCount");
      this.m_SpatializerPlugin = this.serializedObject.FindProperty("m_SpatializerPlugin");
      this.m_DisableAudio = this.serializedObject.FindProperty("m_DisableAudio");
      this.m_VirtualizeEffects = this.serializedObject.FindProperty("m_VirtualizeEffects");
    }

    private int FindPluginStringIndex(string[] strs, string element)
    {
      for (int index = 1; index < strs.Length; ++index)
      {
        if (element == strs[index])
          return index;
      }
      return 0;
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Volume, AudioManagerInspector.Styles.Volume, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_RolloffScale, AudioManagerInspector.Styles.RolloffScale, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_DopplerFactor, AudioManagerInspector.Styles.DopplerFactor, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_DefaultSpeakerMode, AudioManagerInspector.Styles.DefaultSpeakerMode, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_SampleRate, AudioManagerInspector.Styles.SampleRate, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_DSPBufferSize, AudioManagerInspector.Styles.DSPBufferSize, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_VirtualVoiceCount, AudioManagerInspector.Styles.VirtualVoiceCount, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_RealVoiceCount, AudioManagerInspector.Styles.RealVoiceCount, new GUILayoutOption[0]);
      List<string> stringList = new List<string>((IEnumerable<string>) AudioUtil.GetSpatializerPluginNames());
      stringList.Insert(0, "None");
      string[] array = stringList.ToArray();
      List<GUIContent> guiContentList = new List<GUIContent>();
      foreach (string text in array)
        guiContentList.Add(new GUIContent(text));
      EditorGUI.BeginChangeCheck();
      int pluginStringIndex = this.FindPluginStringIndex(array, this.m_SpatializerPlugin.stringValue);
      int index = EditorGUILayout.Popup(AudioManagerInspector.Styles.SpatializerPlugin, pluginStringIndex, guiContentList.ToArray(), new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.m_SpatializerPlugin.stringValue = index != 0 ? array[index] : string.Empty;
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_DisableAudio, AudioManagerInspector.Styles.DisableAudio, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_VirtualizeEffects, AudioManagerInspector.Styles.VirtualizeEffects, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }

    private class Styles
    {
      public static GUIContent Volume = EditorGUIUtility.TextContent("Global Volume|Initial volume multiplier (AudioListener.volume)");
      public static GUIContent RolloffScale = EditorGUIUtility.TextContent("Volume Rolloff Scale|Global volume rolloff multiplier (applies only to logarithmic volume curves).");
      public static GUIContent DopplerFactor = EditorGUIUtility.TextContent("Doppler Factor|Global Doppler speed multiplier for sounds in motion.");
      public static GUIContent DefaultSpeakerMode = EditorGUIUtility.TextContent("Default Speaker Mode|Speaker mode at start of the game. This may be changed at runtime using the AudioSettings.Reset function.");
      public static GUIContent SampleRate = EditorGUIUtility.TextContent("System Sample Rate|Sample rate at which the output device of the audio system runs. Individual sounds may run at different sample rates and will be slowed down/sped up accordingly to match the output rate.");
      public static GUIContent DSPBufferSize = EditorGUIUtility.TextContent("DSP Buffer Size|Length of mixing buffer. This determines the output latency of the game.");
      public static GUIContent VirtualVoiceCount = EditorGUIUtility.TextContent("Max Virtual Voices|Maximum number of sounds managed by the system. Even though at most RealVoiceCount of the loudest sounds will be physically playing, the remaining sounds will still be updating their play position.");
      public static GUIContent RealVoiceCount = EditorGUIUtility.TextContent("Max Real Voices|Maximum number of actual simultanously playing sounds.");
      public static GUIContent SpatializerPlugin = EditorGUIUtility.TextContent("Spatializer Plugin|Native audio plugin performing spatialized filtering of 3D sources.");
      public static GUIContent DisableAudio = EditorGUIUtility.TextContent("Disable Unity Audio|Prevent allocating the output device in the runtime. Use this if you want to use other sound systems than the built-in one.");
      public static GUIContent VirtualizeEffects = EditorGUIUtility.TextContent("Virtualize Effects|When enabled dynamically turn off effects and spatializers on AudioSources that are culled in order to save CPU.");
    }
  }
}
