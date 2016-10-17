// Decompiled with JetBrains decompiler
// Type: UnityEditor.BoxCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (BoxCollider2D))]
  [CanEditMultipleObjects]
  internal class BoxCollider2DEditor : Collider2DEditorBase
  {
    private static readonly int s_BoxHash = "BoxCollider2DEditor".GetHashCode();
    private readonly BoxEditor m_BoxEditor = new BoxEditor(true, BoxCollider2DEditor.s_BoxHash, true);

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_BoxEditor.OnEnable();
      this.m_BoxEditor.SetAlwaysDisplayHandles(true);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.InspectorEditButtonGUI();
      base.OnInspectorGUI();
      this.serializedObject.ApplyModifiedProperties();
    }

    public override void OnDisable()
    {
      base.OnDisable();
      this.m_BoxEditor.OnDisable();
    }

    public void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      BoxCollider2D target = (BoxCollider2D) this.target;
      Vector3 offset = (Vector3) target.offset;
      Vector3 size = (Vector3) target.size;
      if (!this.m_BoxEditor.OnSceneGUI(target.transform, Handles.s_ColliderHandleColor, ref offset, ref size))
        return;
      Undo.RecordObject((Object) target, "Modify collider");
      target.offset = (Vector2) offset;
      target.size = (Vector2) size;
    }
  }
}
