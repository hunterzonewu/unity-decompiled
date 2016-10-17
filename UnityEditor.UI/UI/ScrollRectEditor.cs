// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ScrollRectEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Editor for the ScrollRect component.</para>
  /// </summary>
  [CustomEditor(typeof (ScrollRect), true)]
  [CanEditMultipleObjects]
  public class ScrollRectEditor : Editor
  {
    private static string s_HError = "For this visibility mode, the Viewport property and the Horizontal Scrollbar property both needs to be set to a Rect Transform that is a child to the Scroll Rect.";
    private static string s_VError = "For this visibility mode, the Viewport property and the Vertical Scrollbar property both needs to be set to a Rect Transform that is a child to the Scroll Rect.";
    private SerializedProperty m_Content;
    private SerializedProperty m_Horizontal;
    private SerializedProperty m_Vertical;
    private SerializedProperty m_MovementType;
    private SerializedProperty m_Elasticity;
    private SerializedProperty m_Inertia;
    private SerializedProperty m_DecelerationRate;
    private SerializedProperty m_ScrollSensitivity;
    private SerializedProperty m_Viewport;
    private SerializedProperty m_HorizontalScrollbar;
    private SerializedProperty m_VerticalScrollbar;
    private SerializedProperty m_HorizontalScrollbarVisibility;
    private SerializedProperty m_VerticalScrollbarVisibility;
    private SerializedProperty m_HorizontalScrollbarSpacing;
    private SerializedProperty m_VerticalScrollbarSpacing;
    private SerializedProperty m_OnValueChanged;
    private AnimBool m_ShowElasticity;
    private AnimBool m_ShowDecelerationRate;
    private bool m_ViewportIsNotChild;
    private bool m_HScrollbarIsNotChild;
    private bool m_VScrollbarIsNotChild;

    protected virtual void OnEnable()
    {
      this.m_Content = this.serializedObject.FindProperty("m_Content");
      this.m_Horizontal = this.serializedObject.FindProperty("m_Horizontal");
      this.m_Vertical = this.serializedObject.FindProperty("m_Vertical");
      this.m_MovementType = this.serializedObject.FindProperty("m_MovementType");
      this.m_Elasticity = this.serializedObject.FindProperty("m_Elasticity");
      this.m_Inertia = this.serializedObject.FindProperty("m_Inertia");
      this.m_DecelerationRate = this.serializedObject.FindProperty("m_DecelerationRate");
      this.m_ScrollSensitivity = this.serializedObject.FindProperty("m_ScrollSensitivity");
      this.m_Viewport = this.serializedObject.FindProperty("m_Viewport");
      this.m_HorizontalScrollbar = this.serializedObject.FindProperty("m_HorizontalScrollbar");
      this.m_VerticalScrollbar = this.serializedObject.FindProperty("m_VerticalScrollbar");
      this.m_HorizontalScrollbarVisibility = this.serializedObject.FindProperty("m_HorizontalScrollbarVisibility");
      this.m_VerticalScrollbarVisibility = this.serializedObject.FindProperty("m_VerticalScrollbarVisibility");
      this.m_HorizontalScrollbarSpacing = this.serializedObject.FindProperty("m_HorizontalScrollbarSpacing");
      this.m_VerticalScrollbarSpacing = this.serializedObject.FindProperty("m_VerticalScrollbarSpacing");
      this.m_OnValueChanged = this.serializedObject.FindProperty("m_OnValueChanged");
      this.m_ShowElasticity = new AnimBool(new UnityAction(((Editor) this).Repaint));
      this.m_ShowDecelerationRate = new AnimBool(new UnityAction(((Editor) this).Repaint));
      this.SetAnimBools(true);
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected virtual void OnDisable()
    {
      this.m_ShowElasticity.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowDecelerationRate.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    private void SetAnimBools(bool instant)
    {
      this.SetAnimBool(this.m_ShowElasticity, !this.m_MovementType.hasMultipleDifferentValues && this.m_MovementType.enumValueIndex == 1, instant);
      this.SetAnimBool(this.m_ShowDecelerationRate, !this.m_Inertia.hasMultipleDifferentValues && this.m_Inertia.boolValue, instant);
    }

    private void SetAnimBool(AnimBool a, bool value, bool instant)
    {
      if (instant)
        a.value = value;
      else
        a.target = value;
    }

    private void CalculateCachedValues()
    {
      this.m_ViewportIsNotChild = false;
      this.m_HScrollbarIsNotChild = false;
      this.m_VScrollbarIsNotChild = false;
      if (this.targets.Length != 1)
        return;
      Transform transform = ((Component) this.target).transform;
      if (this.m_Viewport.objectReferenceValue == (Object) null || (Object) ((Component) this.m_Viewport.objectReferenceValue).transform.parent != (Object) transform)
        this.m_ViewportIsNotChild = true;
      if (this.m_HorizontalScrollbar.objectReferenceValue == (Object) null || (Object) ((Component) this.m_HorizontalScrollbar.objectReferenceValue).transform.parent != (Object) transform)
        this.m_HScrollbarIsNotChild = true;
      if (!(this.m_VerticalScrollbar.objectReferenceValue == (Object) null) && !((Object) ((Component) this.m_VerticalScrollbar.objectReferenceValue).transform.parent != (Object) transform))
        return;
      this.m_VScrollbarIsNotChild = true;
    }

    /// <summary>
    ///   <para>See Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      this.SetAnimBools(false);
      this.serializedObject.Update();
      this.CalculateCachedValues();
      EditorGUILayout.PropertyField(this.m_Content);
      EditorGUILayout.PropertyField(this.m_Horizontal);
      EditorGUILayout.PropertyField(this.m_Vertical);
      EditorGUILayout.PropertyField(this.m_MovementType);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowElasticity.faded))
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_Elasticity);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.PropertyField(this.m_Inertia);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowDecelerationRate.faded))
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_DecelerationRate);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      EditorGUILayout.PropertyField(this.m_ScrollSensitivity);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_Viewport);
      EditorGUILayout.PropertyField(this.m_HorizontalScrollbar);
      if ((bool) this.m_HorizontalScrollbar.objectReferenceValue && !this.m_HorizontalScrollbar.hasMultipleDifferentValues)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_HorizontalScrollbarVisibility, new GUIContent("Visibility"), new GUILayoutOption[0]);
        if (this.m_HorizontalScrollbarVisibility.enumValueIndex == 2 && !this.m_HorizontalScrollbarVisibility.hasMultipleDifferentValues)
        {
          if (this.m_ViewportIsNotChild || this.m_HScrollbarIsNotChild)
            EditorGUILayout.HelpBox(ScrollRectEditor.s_HError, MessageType.Error);
          EditorGUILayout.PropertyField(this.m_HorizontalScrollbarSpacing, new GUIContent("Spacing"), new GUILayoutOption[0]);
        }
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.PropertyField(this.m_VerticalScrollbar);
      if ((bool) this.m_VerticalScrollbar.objectReferenceValue && !this.m_VerticalScrollbar.hasMultipleDifferentValues)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.PropertyField(this.m_VerticalScrollbarVisibility, new GUIContent("Visibility"), new GUILayoutOption[0]);
        if (this.m_VerticalScrollbarVisibility.enumValueIndex == 2 && !this.m_VerticalScrollbarVisibility.hasMultipleDifferentValues)
        {
          if (this.m_ViewportIsNotChild || this.m_VScrollbarIsNotChild)
            EditorGUILayout.HelpBox(ScrollRectEditor.s_VError, MessageType.Error);
          EditorGUILayout.PropertyField(this.m_VerticalScrollbarSpacing, new GUIContent("Spacing"), new GUILayoutOption[0]);
        }
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_OnValueChanged);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
