// Decompiled with JetBrains decompiler
// Type: UnityEditor.MeshRendererEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (MeshRenderer))]
  internal class MeshRendererEditor : RendererEditorBase
  {
    private SerializedProperty m_CastShadows;
    private SerializedProperty m_ReceiveShadows;
    private SerializedProperty m_Materials;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_CastShadows = this.serializedObject.FindProperty("m_CastShadows");
      this.m_ReceiveShadows = this.serializedObject.FindProperty("m_ReceiveShadows");
      this.m_Materials = this.serializedObject.FindProperty("m_Materials");
      this.InitializeProbeFields();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      bool flag = false;
      SerializedProperty property = this.serializedObject.FindProperty("m_Materials");
      if (!property.hasMultipleDifferentValues)
      {
        MeshFilter component = ((Component) this.serializedObject.targetObject).GetComponent<MeshFilter>();
        flag = (Object) component != (Object) null && (Object) component.sharedMesh != (Object) null && property.arraySize > component.sharedMesh.subMeshCount;
      }
      EditorGUILayout.PropertyField(this.m_CastShadows, true, new GUILayoutOption[0]);
      EditorGUI.BeginDisabledGroup(SceneView.IsUsingDeferredRenderingPath());
      EditorGUILayout.PropertyField(this.m_ReceiveShadows, true, new GUILayoutOption[0]);
      EditorGUI.EndDisabledGroup();
      EditorGUILayout.PropertyField(this.m_Materials, true, new GUILayoutOption[0]);
      if (!this.m_Materials.hasMultipleDifferentValues && flag)
        EditorGUILayout.HelpBox("This renderer has more materials than the Mesh has submeshes. Multiple materials will be applied to the same submesh, which costs performance. Consider using multiple shader passes.", MessageType.Warning, true);
      this.RenderProbeFields();
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
