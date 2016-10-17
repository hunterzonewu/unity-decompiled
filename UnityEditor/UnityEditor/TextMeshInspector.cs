// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextMeshInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (TextMesh))]
  [CanEditMultipleObjects]
  internal class TextMeshInspector : Editor
  {
    private SerializedProperty m_Font;

    private void OnEnable()
    {
      this.m_Font = this.serializedObject.FindProperty("m_Font");
    }

    public override void OnInspectorGUI()
    {
      Font font1 = !this.m_Font.hasMultipleDifferentValues ? this.m_Font.objectReferenceValue as Font : (Font) null;
      this.DrawDefaultInspector();
      Font font2 = !this.m_Font.hasMultipleDifferentValues ? this.m_Font.objectReferenceValue as Font : (Font) null;
      if (!((Object) font2 != (Object) null) || !((Object) font2 != (Object) font1))
        return;
      foreach (Component target in this.targets)
      {
        MeshRenderer component = target.GetComponent<MeshRenderer>();
        if ((bool) ((Object) component))
          component.sharedMaterial = font2.material;
      }
    }
  }
}
