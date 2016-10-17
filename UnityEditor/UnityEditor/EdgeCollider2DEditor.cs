// Decompiled with JetBrains decompiler
// Type: UnityEditor.EdgeCollider2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (EdgeCollider2D))]
  internal class EdgeCollider2DEditor : Collider2DEditorBase
  {
    private PolygonEditorUtility m_PolyUtility = new PolygonEditorUtility();
    private bool m_ShowColliderInfo;

    public override void OnEnable()
    {
      base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
      this.BeginColliderInspector();
      base.OnInspectorGUI();
      this.EndColliderInspector();
    }

    protected override void OnEditStart()
    {
      this.m_PolyUtility.StartEditing(this.target as Collider2D);
    }

    protected override void OnEditEnd()
    {
      this.m_PolyUtility.StopEditing();
    }

    public void OnSceneGUI()
    {
      if (!this.editingCollider)
        return;
      this.m_PolyUtility.OnSceneGUI();
    }
  }
}
