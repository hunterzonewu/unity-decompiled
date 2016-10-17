// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.AspectRatioFitterEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the AspectRatioFitter component.</para>
  /// </summary>
  [CustomEditor(typeof (AspectRatioFitter), true)]
  [CanEditMultipleObjects]
  public class AspectRatioFitterEditor : SelfControllerEditor
  {
    private SerializedProperty m_AspectMode;
    private SerializedProperty m_AspectRatio;

    protected virtual void OnEnable()
    {
      this.m_AspectMode = this.serializedObject.FindProperty("m_AspectMode");
      this.m_AspectRatio = this.serializedObject.FindProperty("m_AspectRatio");
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_AspectMode);
      EditorGUILayout.PropertyField(this.m_AspectRatio);
      this.serializedObject.ApplyModifiedProperties();
      base.OnInspectorGUI();
    }
  }
}
