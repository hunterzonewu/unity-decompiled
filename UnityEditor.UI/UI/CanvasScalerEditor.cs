// Decompiled with JetBrains decompiler
// Type: UnityEditor.UI.CanvasScalerEditor
// Assembly: UnityEditor.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02C415E4-CA90-4A5B-B8EB-A397F164EE08
// Assembly location: C:\Users\Blake\shadow-0\Library\UnityAssemblies\UnityEditor.UI.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
  /// <summary>
  ///   <para>Custom Editor for the CanvasScaler component.</para>
  /// </summary>
  [CanEditMultipleObjects]
  [CustomEditor(typeof (CanvasScaler), true)]
  public class CanvasScalerEditor : Editor
  {
    private const int kSliderEndpointLabelsHeight = 12;
    private SerializedProperty m_UiScaleMode;
    private SerializedProperty m_ScaleFactor;
    private SerializedProperty m_ReferenceResolution;
    private SerializedProperty m_ScreenMatchMode;
    private SerializedProperty m_MatchWidthOrHeight;
    private SerializedProperty m_PhysicalUnit;
    private SerializedProperty m_FallbackScreenDPI;
    private SerializedProperty m_DefaultSpriteDPI;
    private SerializedProperty m_DynamicPixelsPerUnit;
    private SerializedProperty m_ReferencePixelsPerUnit;
    private static CanvasScalerEditor.Styles s_Styles;

    protected virtual void OnEnable()
    {
      this.m_UiScaleMode = this.serializedObject.FindProperty("m_UiScaleMode");
      this.m_ScaleFactor = this.serializedObject.FindProperty("m_ScaleFactor");
      this.m_ReferenceResolution = this.serializedObject.FindProperty("m_ReferenceResolution");
      this.m_ScreenMatchMode = this.serializedObject.FindProperty("m_ScreenMatchMode");
      this.m_MatchWidthOrHeight = this.serializedObject.FindProperty("m_MatchWidthOrHeight");
      this.m_PhysicalUnit = this.serializedObject.FindProperty("m_PhysicalUnit");
      this.m_FallbackScreenDPI = this.serializedObject.FindProperty("m_FallbackScreenDPI");
      this.m_DefaultSpriteDPI = this.serializedObject.FindProperty("m_DefaultSpriteDPI");
      this.m_DynamicPixelsPerUnit = this.serializedObject.FindProperty("m_DynamicPixelsPerUnit");
      this.m_ReferencePixelsPerUnit = this.serializedObject.FindProperty("m_ReferencePixelsPerUnit");
    }

    public override void OnInspectorGUI()
    {
      if (CanvasScalerEditor.s_Styles == null)
        CanvasScalerEditor.s_Styles = new CanvasScalerEditor.Styles();
      bool flag1 = true;
      bool flag2 = false;
      bool flag3 = (this.target as CanvasScaler).GetComponent<Canvas>().renderMode == UnityEngine.RenderMode.WorldSpace;
      for (int index = 0; index < this.targets.Length; ++index)
      {
        Canvas component = (this.targets[index] as CanvasScaler).GetComponent<Canvas>();
        if (!component.isRootCanvas)
        {
          flag1 = false;
          break;
        }
        if (flag3 && component.renderMode != UnityEngine.RenderMode.WorldSpace || !flag3 && component.renderMode == UnityEngine.RenderMode.WorldSpace)
        {
          flag2 = true;
          break;
        }
      }
      if (!flag1)
      {
        EditorGUILayout.HelpBox("Non-root Canvases will not be scaled.", MessageType.Warning);
      }
      else
      {
        this.serializedObject.Update();
        EditorGUI.showMixedValue = flag2;
        EditorGUI.BeginDisabledGroup(flag3 || flag2);
        if (flag3 || flag2)
          EditorGUILayout.Popup(CanvasScalerEditor.s_Styles.uiScaleModeContent.text, 0, new string[1]{ "World" }, new GUILayoutOption[0]);
        else
          EditorGUILayout.PropertyField(this.m_UiScaleMode, CanvasScalerEditor.s_Styles.uiScaleModeContent, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();
        EditorGUI.showMixedValue = false;
        if (!flag2 && (flag3 || !this.m_UiScaleMode.hasMultipleDifferentValues))
        {
          EditorGUILayout.Space();
          if (flag3)
            EditorGUILayout.PropertyField(this.m_DynamicPixelsPerUnit);
          else if (this.m_UiScaleMode.enumValueIndex == 0)
            EditorGUILayout.PropertyField(this.m_ScaleFactor);
          else if (this.m_UiScaleMode.enumValueIndex == 1)
          {
            EditorGUILayout.PropertyField(this.m_ReferenceResolution);
            EditorGUILayout.PropertyField(this.m_ScreenMatchMode);
            if (this.m_ScreenMatchMode.enumValueIndex == 0 && !this.m_ScreenMatchMode.hasMultipleDifferentValues)
              CanvasScalerEditor.DualLabeledSlider(EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight + 12f, new GUILayoutOption[0]), this.m_MatchWidthOrHeight, CanvasScalerEditor.s_Styles.matchContent, CanvasScalerEditor.s_Styles.widthContent, CanvasScalerEditor.s_Styles.heightContent);
          }
          else if (this.m_UiScaleMode.enumValueIndex == 2)
          {
            EditorGUILayout.PropertyField(this.m_PhysicalUnit);
            EditorGUILayout.PropertyField(this.m_FallbackScreenDPI);
            EditorGUILayout.PropertyField(this.m_DefaultSpriteDPI);
          }
          EditorGUILayout.PropertyField(this.m_ReferencePixelsPerUnit);
        }
        this.serializedObject.ApplyModifiedProperties();
      }
    }

    private static void DualLabeledSlider(Rect position, SerializedProperty property, GUIContent mainLabel, GUIContent labelLeft, GUIContent labelRight)
    {
      position.height = EditorGUIUtility.singleLineHeight;
      Rect position1 = position;
      position.y += 12f;
      position.xMin += EditorGUIUtility.labelWidth;
      position.xMax -= EditorGUIUtility.fieldWidth;
      GUI.Label(position, labelLeft, CanvasScalerEditor.s_Styles.leftAlignedLabel);
      GUI.Label(position, labelRight, CanvasScalerEditor.s_Styles.rightAlignedLabel);
      EditorGUI.PropertyField(position1, property, mainLabel);
    }

    private class Styles
    {
      public GUIContent matchContent;
      public GUIContent widthContent;
      public GUIContent heightContent;
      public GUIContent uiScaleModeContent;
      public GUIStyle leftAlignedLabel;
      public GUIStyle rightAlignedLabel;

      public Styles()
      {
        this.matchContent = new GUIContent("Match");
        this.widthContent = new GUIContent("Width");
        this.heightContent = new GUIContent("Height");
        this.uiScaleModeContent = new GUIContent("UI Scale Mode");
        this.leftAlignedLabel = new GUIStyle(EditorStyles.label);
        this.rightAlignedLabel = new GUIStyle(EditorStyles.label);
        this.rightAlignedLabel.alignment = TextAnchor.MiddleRight;
      }
    }
  }
}
