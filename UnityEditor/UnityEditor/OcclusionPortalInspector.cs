// Decompiled with JetBrains decompiler
// Type: UnityEditor.OcclusionPortalInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (OcclusionPortal))]
  internal class OcclusionPortalInspector : Editor
  {
    private static readonly int s_BoxHash = "BoxColliderEditor".GetHashCode();
    private readonly BoxEditor m_BoxEditor = new BoxEditor(true, OcclusionPortalInspector.s_BoxHash);
    private SerializedProperty m_Center;
    private SerializedProperty m_Size;
    private SerializedObject m_Object;

    public void OnEnable()
    {
      this.m_Object = new SerializedObject(this.targets);
      this.m_Center = this.m_Object.FindProperty("m_Center");
      this.m_Size = this.m_Object.FindProperty("m_Size");
      this.m_BoxEditor.OnEnable();
      this.m_BoxEditor.SetAlwaysDisplayHandles(true);
    }

    private void OnSceneGUI()
    {
      OcclusionPortal target = this.target as OcclusionPortal;
      Vector3 vector3Value1 = this.m_Center.vector3Value;
      Vector3 vector3Value2 = this.m_Size.vector3Value;
      Color colliderHandleColor = Handles.s_ColliderHandleColor;
      if (!this.m_BoxEditor.OnSceneGUI(target.transform, colliderHandleColor, ref vector3Value1, ref vector3Value2))
        return;
      this.m_Center.vector3Value = vector3Value1;
      this.m_Size.vector3Value = vector3Value2;
      this.m_Object.ApplyModifiedProperties();
    }
  }
}
