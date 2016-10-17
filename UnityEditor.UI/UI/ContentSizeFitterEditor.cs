// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ContentSizeFitterEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///         <para>Custom Editor for the ContentSizeFitter Component.
  /// </para>
  ///       </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (ContentSizeFitter), true)]
  public class ContentSizeFitterEditor : SelfControllerEditor
  {
    private SerializedProperty m_HorizontalFit;
    private SerializedProperty m_VerticalFit;

    protected virtual void OnEnable()
    {
      this.m_HorizontalFit = this.serializedObject.FindProperty("m_HorizontalFit");
      this.m_VerticalFit = this.serializedObject.FindProperty("m_VerticalFit");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_HorizontalFit, true, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_VerticalFit, true, new GUILayoutOption[0]);
      this.serializedObject.ApplyModifiedProperties();
      base.OnInspectorGUI();
    }
  }
}
