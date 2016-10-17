// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioReverbFilterEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioReverbFilter))]
  [CanEditMultipleObjects]
  internal class AudioReverbFilterEditor : Editor
  {
    private SerializedProperty m_ReverbPreset;
    private SerializedProperty m_DryLevel;
    private SerializedProperty m_Room;
    private SerializedProperty m_RoomHF;
    private SerializedProperty m_RoomLF;
    private SerializedProperty m_DecayTime;
    private SerializedProperty m_DecayHFRatio;
    private SerializedProperty m_ReflectionsLevel;
    private SerializedProperty m_ReflectionsDelay;
    private SerializedProperty m_ReverbLevel;
    private SerializedProperty m_ReverbDelay;
    private SerializedProperty m_HFReference;
    private SerializedProperty m_LFReference;
    private SerializedProperty m_Diffusion;
    private SerializedProperty m_Density;

    private void OnEnable()
    {
      this.m_ReverbPreset = this.serializedObject.FindProperty("m_ReverbPreset");
      this.m_DryLevel = this.serializedObject.FindProperty("m_DryLevel");
      this.m_Room = this.serializedObject.FindProperty("m_Room");
      this.m_RoomHF = this.serializedObject.FindProperty("m_RoomHF");
      this.m_RoomLF = this.serializedObject.FindProperty("m_RoomLF");
      this.m_DecayTime = this.serializedObject.FindProperty("m_DecayTime");
      this.m_DecayHFRatio = this.serializedObject.FindProperty("m_DecayHFRatio");
      this.m_ReflectionsLevel = this.serializedObject.FindProperty("m_ReflectionsLevel");
      this.m_ReflectionsDelay = this.serializedObject.FindProperty("m_ReflectionsDelay");
      this.m_ReverbLevel = this.serializedObject.FindProperty("m_ReverbLevel");
      this.m_ReverbDelay = this.serializedObject.FindProperty("m_ReverbDelay");
      this.m_HFReference = this.serializedObject.FindProperty("m_HFReference");
      this.m_LFReference = this.serializedObject.FindProperty("m_LFReference");
      this.m_Diffusion = this.serializedObject.FindProperty("m_Diffusion");
      this.m_Density = this.serializedObject.FindProperty("m_Density");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_ReverbPreset);
      if (EditorGUI.EndChangeCheck())
        this.serializedObject.SetIsDifferentCacheDirty();
      EditorGUI.BeginDisabledGroup(this.m_ReverbPreset.enumValueIndex != 27 || this.m_ReverbPreset.hasMultipleDifferentValues);
      EditorGUILayout.Slider(this.m_DryLevel, -10000f, 0.0f);
      EditorGUILayout.Slider(this.m_Room, -10000f, 0.0f);
      EditorGUILayout.Slider(this.m_RoomHF, -10000f, 0.0f);
      EditorGUILayout.Slider(this.m_RoomLF, -10000f, 0.0f);
      EditorGUILayout.Slider(this.m_DecayTime, 0.1f, 20f);
      EditorGUILayout.Slider(this.m_DecayHFRatio, 0.1f, 2f);
      EditorGUILayout.Slider(this.m_ReflectionsLevel, -10000f, 1000f);
      EditorGUILayout.Slider(this.m_ReflectionsDelay, 0.0f, 0.3f);
      EditorGUILayout.Slider(this.m_ReverbLevel, -10000f, 2000f);
      EditorGUILayout.Slider(this.m_ReverbDelay, 0.0f, 0.1f);
      EditorGUILayout.Slider(this.m_HFReference, 1000f, 20000f);
      EditorGUILayout.Slider(this.m_LFReference, 20f, 1000f);
      EditorGUILayout.Slider(this.m_Diffusion, 0.0f, 100f);
      EditorGUILayout.Slider(this.m_Density, 0.0f, 100f);
      EditorGUI.EndDisabledGroup();
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
