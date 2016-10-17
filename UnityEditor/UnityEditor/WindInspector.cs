// Decompiled with JetBrains decompiler
// Type: UnityEditor.WindInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (WindZone))]
  [CanEditMultipleObjects]
  internal class WindInspector : Editor
  {
    private readonly AnimBool m_ShowRadius = new AnimBool();
    private SerializedProperty m_Mode;
    private SerializedProperty m_Radius;
    private SerializedProperty m_WindMain;
    private SerializedProperty m_WindTurbulence;
    private SerializedProperty m_WindPulseMagnitude;
    private SerializedProperty m_WindPulseFrequency;

    private void OnEnable()
    {
      this.m_Mode = this.serializedObject.FindProperty("m_Mode");
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
      this.m_WindMain = this.serializedObject.FindProperty("m_WindMain");
      this.m_WindTurbulence = this.serializedObject.FindProperty("m_WindTurbulence");
      this.m_WindPulseMagnitude = this.serializedObject.FindProperty("m_WindPulseMagnitude");
      this.m_WindPulseFrequency = this.serializedObject.FindProperty("m_WindPulseFrequency");
      this.m_ShowRadius.value = !this.m_Mode.hasMultipleDifferentValues && this.m_Mode.intValue == 1;
      this.m_ShowRadius.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    private void OnDisable()
    {
      this.m_ShowRadius.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Mode, WindInspector.Styles.Mode, new GUILayoutOption[0]);
      this.m_ShowRadius.target = !this.m_Mode.hasMultipleDifferentValues && this.m_Mode.intValue == 1;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowRadius.faded))
        EditorGUILayout.PropertyField(this.m_Radius, WindInspector.Styles.Radius, new GUILayoutOption[0]);
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.PropertyField(this.m_WindMain, WindInspector.Styles.WindMain, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_WindTurbulence, WindInspector.Styles.WindTurbulence, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_WindPulseMagnitude, WindInspector.Styles.WindPulseMagnitude, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_WindPulseFrequency, WindInspector.Styles.WindPulseFrequency, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
    }

    private class Styles
    {
      public static GUIContent Mode = EditorGUIUtility.TextContent("Mode|The wind blows towards a direction or outwards within a sphere");
      public static GUIContent Radius = EditorGUIUtility.TextContent("Radius|The radius of the spherical area");
      public static GUIContent WindMain = EditorGUIUtility.TextContent("Main|Overall strength of the wind");
      public static GUIContent WindTurbulence = EditorGUIUtility.TextContent("Turbulence|Randomness in strength");
      public static GUIContent WindPulseMagnitude = EditorGUIUtility.TextContent("Pulse Magnitude|Stength of the wind pulses");
      public static GUIContent WindPulseFrequency = EditorGUIUtility.TextContent("Pulse Frequency|Frequency of the wind pulses");
    }
  }
}
