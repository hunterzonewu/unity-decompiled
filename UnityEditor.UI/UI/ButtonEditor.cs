// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ButtonEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Button Component.</para>
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Button), true)]
  public class ButtonEditor : SelectableEditor
  {
    private SerializedProperty m_OnClickProperty;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_OnClickProperty = this.serializedObject.FindProperty("m_OnClick");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      EditorGUILayout.Space();
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_OnClickProperty);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
