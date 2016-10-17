// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.MaskEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Mask component.</para>
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Mask), true)]
  public class MaskEditor : Editor
  {
    private SerializedProperty m_ShowMaskGraphic;

    protected virtual void OnEnable()
    {
      this.m_ShowMaskGraphic = this.serializedObject.FindProperty("m_ShowMaskGraphic");
    }

    public override void OnInspectorGUI()
    {
      Graphic component = (this.target as Mask).GetComponent<Graphic>();
      if ((bool) ((Object) component) && !component.IsActive())
        EditorGUILayout.HelpBox("Masking disabled due to Graphic component being disabled.", MessageType.Warning);
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_ShowMaskGraphic);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
