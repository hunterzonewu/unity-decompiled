// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioReverbZoneEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioReverbZone))]
  [CanEditMultipleObjects]
  internal class AudioReverbZoneEditor : Editor
  {
    private SerializedProperty m_MinDistance;
    private SerializedProperty m_MaxDistance;
    private SerializedProperty m_ReverbPreset;
    private SerializedProperty m_Room;
    private SerializedProperty m_RoomHF;
    private SerializedProperty m_RoomLF;
    private SerializedProperty m_DecayTime;
    private SerializedProperty m_DecayHFRatio;
    private SerializedProperty m_Reflections;
    private SerializedProperty m_ReflectionsDelay;
    private SerializedProperty m_Reverb;
    private SerializedProperty m_ReverbDelay;
    private SerializedProperty m_HFReference;
    private SerializedProperty m_LFReference;
    private SerializedProperty m_RoomRolloffFactor;
    private SerializedProperty m_Diffusion;
    private SerializedProperty m_Density;

    private void OnEnable()
    {
      this.m_MinDistance = this.serializedObject.FindProperty("m_MinDistance");
      this.m_MaxDistance = this.serializedObject.FindProperty("m_MaxDistance");
      this.m_ReverbPreset = this.serializedObject.FindProperty("m_ReverbPreset");
      this.m_Room = this.serializedObject.FindProperty("m_Room");
      this.m_RoomHF = this.serializedObject.FindProperty("m_RoomHF");
      this.m_RoomLF = this.serializedObject.FindProperty("m_RoomLF");
      this.m_DecayTime = this.serializedObject.FindProperty("m_DecayTime");
      this.m_DecayHFRatio = this.serializedObject.FindProperty("m_DecayHFRatio");
      this.m_Reflections = this.serializedObject.FindProperty("m_Reflections");
      this.m_ReflectionsDelay = this.serializedObject.FindProperty("m_ReflectionsDelay");
      this.m_Reverb = this.serializedObject.FindProperty("m_Reverb");
      this.m_ReverbDelay = this.serializedObject.FindProperty("m_ReverbDelay");
      this.m_HFReference = this.serializedObject.FindProperty("m_HFReference");
      this.m_LFReference = this.serializedObject.FindProperty("m_LFReference");
      this.m_RoomRolloffFactor = this.serializedObject.FindProperty("m_RoomRolloffFactor");
      this.m_Diffusion = this.serializedObject.FindProperty("m_Diffusion");
      this.m_Density = this.serializedObject.FindProperty("m_Density");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_MinDistance);
      EditorGUILayout.PropertyField(this.m_MaxDistance);
      EditorGUILayout.Space();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_ReverbPreset);
      if (EditorGUI.EndChangeCheck())
        this.serializedObject.SetIsDifferentCacheDirty();
      EditorGUI.BeginDisabledGroup(this.m_ReverbPreset.enumValueIndex != 27 || this.m_ReverbPreset.hasMultipleDifferentValues);
      EditorGUILayout.IntSlider(this.m_Room, -10000, 0);
      EditorGUILayout.IntSlider(this.m_RoomHF, -10000, 0);
      EditorGUILayout.IntSlider(this.m_RoomLF, -10000, 0);
      EditorGUILayout.Slider(this.m_DecayTime, 0.1f, 20f);
      EditorGUILayout.Slider(this.m_DecayHFRatio, 0.1f, 2f);
      EditorGUILayout.IntSlider(this.m_Reflections, -10000, 1000);
      EditorGUILayout.Slider(this.m_ReflectionsDelay, 0.0f, 0.3f);
      EditorGUILayout.IntSlider(this.m_Reverb, -10000, 2000);
      EditorGUILayout.Slider(this.m_ReverbDelay, 0.0f, 0.1f);
      EditorGUILayout.Slider(this.m_HFReference, 1000f, 20000f);
      EditorGUILayout.Slider(this.m_LFReference, 20f, 1000f);
      EditorGUILayout.Slider(this.m_RoomRolloffFactor, 0.0f, 10f);
      EditorGUILayout.Slider(this.m_Diffusion, 0.0f, 100f);
      EditorGUILayout.Slider(this.m_Density, 0.0f, 100f);
      EditorGUI.EndDisabledGroup();
      this.serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
      AudioReverbZone target = (AudioReverbZone) this.target;
      Color color = Handles.color;
      Handles.color = !target.enabled ? new Color(0.3f, 0.4f, 0.6f, 0.5f) : new Color(0.5f, 0.7f, 1f, 0.5f);
      Vector3 position = target.transform.position;
      EditorGUI.BeginChangeCheck();
      float num1 = Handles.RadiusHandle(Quaternion.identity, position, target.minDistance, true);
      float num2 = Handles.RadiusHandle(Quaternion.identity, position, target.maxDistance, true);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject((Object) target, "Reverb Distance");
        target.minDistance = num1;
        target.maxDistance = num2;
      }
      Handles.color = color;
    }
  }
}
