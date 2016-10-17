// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimationEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Animation))]
  internal class AnimationEditor : Editor
  {
    private static int s_BoxHash = "AnimationBoundsEditorHash".GetHashCode();
    private BoxEditor m_BoxEditor = new BoxEditor(false, AnimationEditor.s_BoxHash);
    private int m_PrePreviewAnimationArraySize = -1;

    public void OnEnable()
    {
      this.m_PrePreviewAnimationArraySize = -1;
      this.m_BoxEditor.OnEnable();
    }

    public void OnDisable()
    {
      this.m_BoxEditor.OnDisable();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      SerializedProperty property1 = this.serializedObject.FindProperty("m_Animation");
      EditorGUILayout.PropertyField(property1, true, new GUILayoutOption[0]);
      int referenceInstanceIdValue = property1.objectReferenceInstanceIDValue;
      SerializedProperty property2 = this.serializedObject.FindProperty("m_Animations");
      int arraySize = property2.arraySize;
      if (ObjectSelector.isVisible && this.m_PrePreviewAnimationArraySize == -1)
        this.m_PrePreviewAnimationArraySize = arraySize;
      if (this.m_PrePreviewAnimationArraySize != -1)
      {
        if ((arraySize <= 0 ? -1 : property2.GetArrayElementAtIndex(arraySize - 1).objectReferenceInstanceIDValue) != referenceInstanceIdValue)
          property2.arraySize = this.m_PrePreviewAnimationArraySize;
        if (!ObjectSelector.isVisible)
          this.m_PrePreviewAnimationArraySize = -1;
      }
      Editor.DrawPropertiesExcluding(this.serializedObject, "m_Animation", "m_UserAABB");
      this.serializedObject.ApplyModifiedProperties();
    }

    internal override void OnAssetStoreInspectorGUI()
    {
      this.OnInspectorGUI();
    }
  }
}
