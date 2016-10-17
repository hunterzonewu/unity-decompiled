// Decompiled with JetBrains decompiler
// Type: UnityEditor.Rigidbody2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (Rigidbody2D))]
  [CanEditMultipleObjects]
  internal class Rigidbody2DEditor : Editor
  {
    private static readonly GUIContent m_FreezePositionLabel = new GUIContent("Freeze Position");
    private static readonly GUIContent m_FreezeRotationLabel = new GUIContent("Freeze Rotation");
    private readonly AnimBool m_ShowMass = new AnimBool();
    private const int k_ToggleOffset = 30;
    private SerializedProperty m_UseAutoMass;
    private SerializedProperty m_Mass;
    private SerializedProperty m_Constraints;

    public void OnEnable()
    {
      Rigidbody2D target = this.target as Rigidbody2D;
      this.m_UseAutoMass = this.serializedObject.FindProperty("m_UseAutoMass");
      this.m_Mass = this.serializedObject.FindProperty("m_Mass");
      this.m_Constraints = this.serializedObject.FindProperty("m_Constraints");
      this.m_ShowMass.value = !target.useAutoMass;
      this.m_ShowMass.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    public void OnDisable()
    {
      this.m_ShowMass.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      Rigidbody2D target = this.target as Rigidbody2D;
      this.serializedObject.Update();
      this.m_ShowMass.target = !target.useAutoMass;
      EditorGUILayout.PropertyField(this.m_UseAutoMass);
      bool enabled = GUI.enabled;
      GUI.enabled = !target.useAutoMass;
      EditorGUILayout.PropertyField(this.m_Mass);
      GUI.enabled = enabled;
      this.serializedObject.ApplyModifiedProperties();
      base.OnInspectorGUI();
      GUILayout.BeginHorizontal();
      this.m_Constraints.isExpanded = EditorGUILayout.Foldout(this.m_Constraints.isExpanded, "Constraints");
      GUILayout.EndHorizontal();
      RigidbodyConstraints2D intValue = (RigidbodyConstraints2D) this.m_Constraints.intValue;
      if (this.m_Constraints.isExpanded)
      {
        ++EditorGUI.indentLevel;
        this.ToggleFreezePosition(intValue, Rigidbody2DEditor.m_FreezePositionLabel, 0, 1);
        this.ToggleFreezeRotation(intValue, Rigidbody2DEditor.m_FreezeRotationLabel, 2);
        --EditorGUI.indentLevel;
      }
      if (intValue != RigidbodyConstraints2D.FreezeAll)
        return;
      EditorGUILayout.HelpBox("Rather than turning on all constraints, you may want to consider removing the Rigidbody2D component which makes any colliders static.  This gives far better performance overall.", MessageType.Info);
    }

    private void ConstraintToggle(Rect r, string label, RigidbodyConstraints2D value, int bit)
    {
      bool flag1 = (value & (RigidbodyConstraints2D) (1 << bit)) != RigidbodyConstraints2D.None;
      EditorGUI.showMixedValue = (this.m_Constraints.hasMultipleDifferentValuesBitwise & 1 << bit) != 0;
      EditorGUI.BeginChangeCheck();
      int indentLevel = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;
      bool flag2 = EditorGUI.ToggleLeft(r, label, flag1);
      EditorGUI.indentLevel = indentLevel;
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObjects(this.targets, "Edit Constraints2D");
        this.m_Constraints.SetBitAtIndexForAllTargetsImmediate(bit, flag2);
      }
      EditorGUI.showMixedValue = false;
    }

    private void ToggleFreezePosition(RigidbodyConstraints2D constraints, GUIContent label, int x, int y)
    {
      GUILayout.BeginHorizontal();
      Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.numberField);
      int controlId = GUIUtility.GetControlID(7231, FocusType.Keyboard, rect);
      Rect r = EditorGUI.PrefixLabel(rect, controlId, label);
      r.width = 30f;
      this.ConstraintToggle(r, "X", constraints, x);
      r.x += 30f;
      this.ConstraintToggle(r, "Y", constraints, y);
      GUILayout.EndHorizontal();
    }

    private void ToggleFreezeRotation(RigidbodyConstraints2D constraints, GUIContent label, int z)
    {
      GUILayout.BeginHorizontal();
      Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, EditorGUILayout.kLabelFloatMaxW, 16f, 16f, EditorStyles.numberField);
      int controlId = GUIUtility.GetControlID(7231, FocusType.Keyboard, rect);
      Rect r = EditorGUI.PrefixLabel(rect, controlId, label);
      r.width = 30f;
      this.ConstraintToggle(r, "Z", constraints, z);
      GUILayout.EndHorizontal();
    }
  }
}
