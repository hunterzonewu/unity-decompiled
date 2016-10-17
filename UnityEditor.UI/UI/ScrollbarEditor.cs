// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.ScrollbarEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the Scrollbar Component.</para>
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Scrollbar), true)]
  public class ScrollbarEditor : SelectableEditor
  {
    private SerializedProperty m_HandleRect;
    private SerializedProperty m_Direction;
    private SerializedProperty m_Value;
    private SerializedProperty m_Size;
    private SerializedProperty m_NumberOfSteps;
    private SerializedProperty m_OnValueChanged;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_HandleRect = this.serializedObject.FindProperty("m_HandleRect");
      this.m_Direction = this.serializedObject.FindProperty("m_Direction");
      this.m_Value = this.serializedObject.FindProperty("m_Value");
      this.m_Size = this.serializedObject.FindProperty("m_Size");
      this.m_NumberOfSteps = this.serializedObject.FindProperty("m_NumberOfSteps");
      this.m_OnValueChanged = this.serializedObject.FindProperty("m_OnValueChanged");
    }

    /// <summary>
    ///   <para>See: Editor.OnInspectorGUI.</para>
    /// </summary>
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      EditorGUILayout.Space();
      this.serializedObject.Update();
      EditorGUI.BeginChangeCheck();
      RectTransform rectTransform = EditorGUILayout.ObjectField("Handle Rect", this.m_HandleRect.objectReferenceValue, typeof (RectTransform), true, new GUILayoutOption[0]) as RectTransform;
      if (EditorGUI.EndChangeCheck())
      {
        List<Object> objectList = new List<Object>();
        objectList.Add((Object) rectTransform);
        foreach (Object targetObject in this.m_HandleRect.serializedObject.targetObjects)
        {
          MonoBehaviour monoBehaviour = targetObject as MonoBehaviour;
          if (!((Object) monoBehaviour == (Object) null))
          {
            objectList.Add((Object) monoBehaviour);
            objectList.Add((Object) monoBehaviour.GetComponent<RectTransform>());
          }
        }
        Undo.RecordObjects(objectList.ToArray(), "Change Handle Rect");
        this.m_HandleRect.objectReferenceValue = (Object) rectTransform;
      }
      if (this.m_HandleRect.objectReferenceValue != (Object) null)
      {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_Direction);
        if (EditorGUI.EndChangeCheck())
        {
          Scrollbar.Direction enumValueIndex = (Scrollbar.Direction) this.m_Direction.enumValueIndex;
          foreach (Object targetObject in this.serializedObject.targetObjects)
            (targetObject as Scrollbar).SetDirection(enumValueIndex, true);
        }
        EditorGUILayout.PropertyField(this.m_Value);
        EditorGUILayout.PropertyField(this.m_Size);
        EditorGUILayout.PropertyField(this.m_NumberOfSteps);
        bool flag = false;
        foreach (Object targetObject in this.serializedObject.targetObjects)
        {
          Scrollbar scrollbar = targetObject as Scrollbar;
          switch (scrollbar.direction)
          {
            case Scrollbar.Direction.LeftToRight:
            case Scrollbar.Direction.RightToLeft:
              flag = scrollbar.navigation.mode != Navigation.Mode.Automatic && ((Object) scrollbar.FindSelectableOnLeft() != (Object) null || (Object) scrollbar.FindSelectableOnRight() != (Object) null);
              break;
            default:
              flag = scrollbar.navigation.mode != Navigation.Mode.Automatic && ((Object) scrollbar.FindSelectableOnDown() != (Object) null || (Object) scrollbar.FindSelectableOnUp() != (Object) null);
              break;
          }
        }
        if (flag)
          EditorGUILayout.HelpBox("The selected scrollbar direction conflicts with navigation. Not all navigation options may work.", MessageType.Warning);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(this.m_OnValueChanged);
      }
      else
        EditorGUILayout.HelpBox("Specify a RectTransform for the scrollbar handle. It must have a parent RectTransform that the handle can slide within.", MessageType.Info);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
