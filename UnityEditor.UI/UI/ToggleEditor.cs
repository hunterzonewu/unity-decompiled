// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ToggleEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Toggle Component.</para>
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Toggle), true)]
  public class ToggleEditor : SelectableEditor
  {
    private SerializedProperty m_OnValueChangedProperty;
    private SerializedProperty m_TransitionProperty;
    private SerializedProperty m_GraphicProperty;
    private SerializedProperty m_GroupProperty;
    private SerializedProperty m_IsOnProperty;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_TransitionProperty = this.serializedObject.FindProperty("toggleTransition");
      this.m_GraphicProperty = this.serializedObject.FindProperty("graphic");
      this.m_GroupProperty = this.serializedObject.FindProperty("m_Group");
      this.m_IsOnProperty = this.serializedObject.FindProperty("m_IsOn");
      this.m_OnValueChangedProperty = this.serializedObject.FindProperty("onValueChanged");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      EditorGUILayout.Space();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_IsOnProperty);
      EditorGUILayout.PropertyField(this.m_TransitionProperty);
      EditorGUILayout.PropertyField(this.m_GraphicProperty);
      EditorGUILayout.PropertyField(this.m_GroupProperty);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_OnValueChangedProperty);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
