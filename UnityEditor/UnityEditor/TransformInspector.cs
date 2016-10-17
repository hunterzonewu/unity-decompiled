// Decompiled with JetBrains decompiler
// Type: UnityEditor.TransformInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Transform))]
  [CanEditMultipleObjects]
  internal class TransformInspector : Editor
  {
    private SerializedProperty m_Position;
    private SerializedProperty m_Scale;
    private TransformRotationGUI m_RotationGUI;
    private static TransformInspector.Contents s_Contents;

    public void OnEnable()
    {
      this.m_Position = this.serializedObject.FindProperty("m_LocalPosition");
      this.m_Scale = this.serializedObject.FindProperty("m_LocalScale");
      if (this.m_RotationGUI == null)
        this.m_RotationGUI = new TransformRotationGUI();
      this.m_RotationGUI.OnEnable(this.serializedObject.FindProperty("m_LocalRotation"), new GUIContent(LocalizationDatabase.GetLocalizedString("Rotation")));
    }

    public override void OnInspectorGUI()
    {
      if (TransformInspector.s_Contents == null)
        TransformInspector.s_Contents = new TransformInspector.Contents();
      if (!EditorGUIUtility.wideMode)
      {
        EditorGUIUtility.wideMode = true;
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212f;
      }
      this.serializedObject.Update();
      this.Inspector3D();
      Vector3 position = (this.target as Transform).position;
      if ((double) Mathf.Abs(position.x) > 100000.0 || (double) Mathf.Abs(position.y) > 100000.0 || (double) Mathf.Abs(position.z) > 100000.0)
        EditorGUILayout.HelpBox(TransformInspector.s_Contents.floatingPointWarning, MessageType.Warning);
      this.serializedObject.ApplyModifiedProperties();
    }

    private void Inspector3D()
    {
      EditorGUILayout.PropertyField(this.m_Position, TransformInspector.s_Contents.positionContent, new GUILayoutOption[0]);
      this.m_RotationGUI.RotationField();
      EditorGUILayout.PropertyField(this.m_Scale, TransformInspector.s_Contents.scaleContent, new GUILayoutOption[0]);
    }

    private class Contents
    {
      public GUIContent positionContent = new GUIContent(LocalizationDatabase.GetLocalizedString("Position"), LocalizationDatabase.GetLocalizedString("The local position of this Game Object relative to the parent."));
      public GUIContent scaleContent = new GUIContent(LocalizationDatabase.GetLocalizedString("Scale"), LocalizationDatabase.GetLocalizedString("The local scaling of this Game Object relative to the parent."));
      public GUIContent[] subLabels = new GUIContent[2]{ new GUIContent("X"), new GUIContent("Y") };
      public string floatingPointWarning = LocalizationDatabase.GetLocalizedString("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");
    }
  }
}
