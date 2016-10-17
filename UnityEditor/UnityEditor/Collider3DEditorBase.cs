// Decompiled with JetBrains decompiler
// Type: UnityEditor.Collider3DEditorBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

namespace UnityEditor
{
  internal class Collider3DEditorBase : ColliderEditorBase
  {
    protected SerializedProperty m_Material;
    protected SerializedProperty m_IsTrigger;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_Material = this.serializedObject.FindProperty("m_Material");
      this.m_IsTrigger = this.serializedObject.FindProperty("m_IsTrigger");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_IsTrigger);
      EditorGUILayout.PropertyField(this.m_Material);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
