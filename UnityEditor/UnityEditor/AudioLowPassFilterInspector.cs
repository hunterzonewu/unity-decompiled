// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioLowPassFilterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AudioLowPassFilter))]
  [CanEditMultipleObjects]
  internal class AudioLowPassFilterInspector : Editor
  {
    private SerializedProperty m_LowpassResonanceQ;
    private SerializedProperty m_LowpassLevelCustomCurve;

    private void OnEnable()
    {
      this.m_LowpassResonanceQ = this.serializedObject.FindProperty("m_LowpassResonanceQ");
      this.m_LowpassLevelCustomCurve = this.serializedObject.FindProperty("lowpassLevelCustomCurve");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      AudioSourceInspector.AnimProp(new GUIContent("Cutoff Frequency"), this.m_LowpassLevelCustomCurve, 0.0f, 22000f, true);
      EditorGUILayout.PropertyField(this.m_LowpassResonanceQ);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
