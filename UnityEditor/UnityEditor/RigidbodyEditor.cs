// Decompiled with JetBrains decompiler
// Type: UnityEditor.RigidbodyEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (Rigidbody))]
  [CanEditMultipleObjects]
  internal class RigidbodyEditor : Editor
  {
    private static GUIContent m_FreezePositionLabel = new GUIContent("Freeze Position");
    private static GUIContent m_FreezeRotationLabel = new GUIContent("Freeze Rotation");
    private SerializedProperty m_Constraints;

    public void OnEnable()
    {
      this.m_Constraints = this.serializedObject.FindProperty("m_Constraints");
    }

    private void ConstraintToggle(Rect r, string label, RigidbodyConstraints value, int bit)
    {
      bool flag1 = (value & (RigidbodyConstraints) (1 << bit)) != RigidbodyConstraints.None;
      EditorGUI.showMixedValue = (this.m_Constraints.hasMultipleDifferentValuesBitwise & 1 << bit) != 0;
      EditorGUI.BeginChangeCheck();
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      bool flag2 = EditorGUI.ToggleLeft(r, label, flag1);
      EditorGUI.indentLevel = indentLevel;
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObjects(this.targets, "Edit Constraints");
        this.m_Constraints.SetBitAtIndexForAllTargetsImmediate(bit, flag2);
      }
      EditorGUI.showMixedValue = false;
    }

    private void ToggleBlock(RigidbodyConstraints constraints, GUIContent label, int x, int y, int z)
    {
      GUILayout.BeginHorizontal();
      Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.numberField);
      int controlId = GUIUtility.GetControlID(7231, FocusType.Keyboard, rect);
      Rect r = EditorGUI.PrefixLabel(rect, controlId, label);
      r.width = 30f;
      this.ConstraintToggle(r, "X", constraints, x);
      r.x += 30f;
      this.ConstraintToggle(r, "Y", constraints, y);
      r.x += 30f;
      this.ConstraintToggle(r, "Z", constraints, z);
      GUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
      this.DrawDefaultInspector();
      GUILayout.BeginHorizontal();
      this.m_Constraints.isExpanded = EditorGUILayout.Foldout(this.m_Constraints.isExpanded, "Constraints");
      GUILayout.EndHorizontal();
      this.serializedObject.Update();
      RigidbodyConstraints intValue = (RigidbodyConstraints) this.m_Constraints.intValue;
      if (!this.m_Constraints.isExpanded)
        return;
      ++EditorGUI.indentLevel;
      this.ToggleBlock(intValue, RigidbodyEditor.m_FreezePositionLabel, 1, 2, 3);
      this.ToggleBlock(intValue, RigidbodyEditor.m_FreezeRotationLabel, 4, 5, 6);
      --EditorGUI.indentLevel;
    }
  }
}
